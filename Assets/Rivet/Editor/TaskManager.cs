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

        public delegate Task<TaskConfig?> GetStartConfigDelegate();
        public delegate Task<TaskConfig?> GetHookConfigDelegate();
        public delegate Task<TaskConfig?> GetStopConfigDelegate();
        public delegate TaskPanelWindow? GetTaskPanelDelegate();
        public delegate void StateChangeHandler(bool running);

        // Config
        private GetStartConfigDelegate _getStartConfig;
        private GetHookConfigDelegate? _getHookConfig;
        private GetStopConfigDelegate _getStopConfig;
        private GetTaskPanelDelegate _getTaskPanel;
        private bool _autoRestart = false;

        // Events
        public event StateChangeHandler StateChange;

        // State
        private readonly object _taskLock = new();
        private RivetTask? _task;
        private RivetTask? _stopTask;
        public List<LogEntry> LogEntries = new();

        public bool IsRunning {
            get {
                return _task != null || _stopTask != null;
            }
        }

        public TaskManager(string initMessage, GetStartConfigDelegate getStartConfig, GetHookConfigDelegate? getHookConfig, GetStopConfigDelegate getStopConfig, GetTaskPanelDelegate getTaskPanel, bool autoRestart = false)
        {
            _getStartConfig = getStartConfig;
            _getHookConfig = getHookConfig;
            _getStopConfig = getStopConfig;
            _getTaskPanel = getTaskPanel;
            _autoRestart = autoRestart;

            LogEntries.Add(new LogEntry(initMessage, LogType.META));
        }

        public async Task StartTask(bool hook = false)
        {
            lock (_taskLock)
            {
                // Do nothing if task already running
                if (_task != null || _stopTask != null)
                    return;
            }

            // Kill old task
            if (!hook) {
                _ = StopTask();
            }

            // Start new task
            var config = hook ? await _getHookConfig() : await _getStartConfig();
            if (config == null)
            {
                RivetLogger.Log("No task config provided.");
                return;
            }
            lock (_taskLock)
            {
                _task = new RivetTask(config.Value.Name, config.Value.Input);
                _task.TaskLog += OnTaskLog;
                RivetLogger.Log($"Running {config.Value.Name}");
                OnStateChange();
                RivetLogger.Log($"After state change {config.Value.Name}");
            }

            AddLogLine(hook ? "Hook" : "Start", LogType.META);

            // Run task
            var output = await _task.RunAsync();

            RivetLogger.Log($"Finished {config.Value.Name}");
            await OnTaskOutput(output, _task);
        }

        private async Task OnTaskOutput(Result<JObject> output, RivetTask sourceTask)
        {
            lock (_taskLock)
            {
                if (_task != sourceTask)
                {
                    return;
                }

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

            // Restart if task was not stopped
            if (_autoRestart)
            {
                AddLogLine("Restarting in 2 seconds", LogType.META);
                await Task.Delay(2000);
                await StartTask();
            }

        }

        public async Task StopTask()
        {
            bool shouldStop;
            lock (_taskLock)
            {
                shouldStop = _task != null && _stopTask == null;
            }

            if (shouldStop)
            {
                // Abort running task
                var localTask = _task;
                _task = null;
                localTask.Kill();

                AddLogLine("Stopping", LogType.META);

                // Run stop task
                //
                // Save in global scope so it doesn't get dropped before getting called
                var config = await _getStopConfig();
                if (config == null)
                {
                    RivetLogger.Log("No task config provided.");
                    return;
                }
                lock (_taskLock)
                {
                    _stopTask = new RivetTask(config.Value.Name, config.Value.Input);
                    OnStateChange();
                }

                await _stopTask.RunAsync();

                OnStopFinish();
            }
        }

        private void OnStopFinish()
        {
            AddLogLine("Stopped", LogType.META);

            lock (_taskLock)
            {
                _stopTask = null;
                OnStateChange();
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

            // Clean up message
            message = StripLogPrefix(message);

            AddLogLine(message, logType);
        }

        private string StripLogPrefix(string message)
        {
            if (message.StartsWith("[stdout] "))
            {
                return message.Substring(9);
            }
            else if (message.StartsWith("[stderr] "))
            {
                return message.Substring(9);
            }
            return message;
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
            var isRunning = IsRunning;
            EditorApplication.delayCall += () => StateChange.Invoke(isRunning);
            RivetLogger.Log($"State change: {IsRunning} ({_task != null} {_stopTask != null})");
        }
    }
}