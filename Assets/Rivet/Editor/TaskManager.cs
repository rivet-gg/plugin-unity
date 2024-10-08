using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Codice.Client.BaseCommands;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Rivet.Editor.UI.TaskPanel;
using UnityEditor;

namespace Rivet.Editor
{
    public struct TaskConfig
    {
        public string Name;
        public JObject Input;
    }


    /// <summary>
    /// Persistent task that can be stopped and restarted with logs.
    /// </summary>
    public class TaskManager
    {
        const int MAX_LOG_LEN = 10_000;

        public enum LogType { META, STDOUT, STDERR }

        public struct LogEntry
        {
            public string Message;
            public LogType Type;

            public LogEntry(string message, LogType type)
            {
                Message = message;
                Type = type;
            }
        }

        public delegate Task<TaskConfig?> GetTaskConfigDelegate();
        public delegate TaskPanelWindow? GetTaskPanelDelegate();
        public delegate void StateChangeHandler(bool running);

        // Config
        private GetTaskConfigDelegate _getTaskConfig;
        private GetTaskPanelDelegate _getTaskPanel;
        private bool _autoRestart = false;
        private bool _taskStopping = false;

        // Events
        public event StateChangeHandler StateChange;

        // State
        private readonly object _taskLock = new();
        private RivetTask _task;
        public List<LogEntry> LogEntries = new();

        public TaskManager(string initMessage, GetTaskConfigDelegate getTaskConfig, GetTaskPanelDelegate getTaskPanel, bool autoRestart = false)
        {
            _getTaskConfig = getTaskConfig;
            _getTaskPanel = getTaskPanel;
            _autoRestart = autoRestart;

            LogEntries.Add(new LogEntry(initMessage, LogType.META));
        }

        public async Task StartTask(bool restart = true)
        {
            lock (_taskLock)
            {
                // Do nothing if already stopping another task
                if (_taskStopping)
                    return;

                // Do nothing if task already running
                if (!restart && _task != null)
                    return;
            }

            // Kill old task
            StopTask();

            // Start task
            var config = await _getTaskConfig();
            if (config == null)
            {
                RivetLogger.Log("No task config provided.");
            }
            lock (_taskLock)
            {
                _task = new RivetTask(config.Value.Name, config.Value.Input);
                _taskStopping = false;
                _task.TaskLog += OnTaskLog;
                OnStateChange();
            }

            AddLogLine("Start", LogType.META);

            // Run task
            var output = await _task.RunAsync();
            lock (_taskLock)
            {
                _task = null;
                OnStateChange();
            }

            // Log output
            switch (output)
            {
                case ResultOk<JObject> ok:
                    AddLogLine($"Exited with {ok.Data.ToString(Formatting.None)}", LogType.META);
                    break;
                case ResultErr<JObject> err:
                    AddLogLine($"Task error: {err.Message}", LogType.META);
                    break;
            }

            // TODO: Re-enable task restarting
            // // Restart if task was not stopped
            // bool shouldRestart;
            // lock (_taskLock) shouldRestart = _autoRestart && _task == null && !_taskStopping;
            // if (shouldRestart)
            // {
            //     AddLogLine("Restarting in 2 seconds", LogType.META);
            //     await Task.Delay(2000);
            //     await StartTask();
            // }
        }

        public void StopTask()
        {
            lock (_taskLock)
            {
                if (_task != null)
                {
                    _taskStopping = true;
                    _task.Kill();
                    _task = null;

                    AddLogLine("Stop", LogType.META);

                    OnStateChange();
                }
            }
        }

        private void OnTaskLog(string message, RivetTask.LogType type)
        {
            LogType logType = type switch
            {
                RivetTask.LogType.STDOUT => LogType.STDOUT,
                RivetTask.LogType.STDERR => LogType.STDERR,
                _ => LogType.META
            };
            AddLogLine(message, logType);
        }

        public void AddLogLine(string message, LogType type)
        {
            LogEntries.Add(new LogEntry(message, type));
            if (LogEntries.Count > MAX_LOG_LEN)
            {
                LogEntries.RemoveRange(0, LogEntries.Count - MAX_LOG_LEN);
            }

            // This might not be called on the main thread
            EditorApplication.delayCall += () => _getTaskPanel()?.UpdateLogs();
        }

        public void ClearLogs()
        {
            LogEntries.Clear();
            _getTaskPanel()?.UpdateLogs();
        }

        private void OnStateChange()
        {
            // HACK: Ignore _task.IsRunning because we can't hook in to the
            // event right after the task is started (where IsRunning is set to
            // true). To fix this, we need to be able to hook in to state change
            // events on RivetTask.
            StateChange.Invoke(_task != null);
        }
    }
}