using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace Rivet.Editor.UI.TaskPopup
{
    public enum LogType
    {
        META,
        STDOUT,
        STDERR
    }

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

    public class TaskPopupWindow : EditorWindow
    {
        // GUI
        private ListView _logListView;
        private Button _doneButton;

        // State
        private List<LogEntry> _logEntries = new();


        // Task
        public string TaskName;
        public JObject TaskInput;
        private RivetTask _task;

        // Events
        public event Action<object> TaskOutput;

        public static void ShowWindowInCenter(string title, string taskName, JObject taskInput)
        {
            var window = CreateInstance<TaskPopupWindow>();
            window.TaskName = taskName;
            window.TaskInput = taskInput;
            window.titleContent = new GUIContent(title);

            var size = new Vector2(400, 500);
            var mainWindowCenter = EditorGUIUtility.GetMainWindowPosition().center;
            var position = new Rect(
                mainWindowCenter.x - size.x / 2f,
                mainWindowCenter.y - size.y / 2f,
                size.x,
                size.y
            );

            window.position = position;
            window.ShowUtility();

            window.InitializeAndStartTask();
        }

        public void CreateGUI()
        {
            var visualTreeAsset = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/Rivet/Editor/UI/TaskPopup/TaskPopup.uxml");
            visualTreeAsset.CloneTree(rootVisualElement);

            _logListView = rootVisualElement.Q<ListView>("LogList");
            _doneButton = rootVisualElement.Q<Button>("DoneButton");

            _doneButton.clicked += OnDonePressed;

            _logListView.itemsSource = _logEntries;
            _logListView.makeItem = () => new Label();
            _logListView.bindItem = (element, i) =>
            {
                var label = element as Label;
                var entry = _logEntries[i];
                label.text = entry.Message;
                label.style.fontSize = 12;
                label.style.whiteSpace = WhiteSpace.Normal;
                label.style.overflow = Overflow.Hidden;

                label.ClearClassList();
                switch (entry.Type)
                {
                    case LogType.META:
                        label.AddToClassList("logMeta");
                        break;
                    case LogType.STDOUT:
                        label.AddToClassList("logStdOut");
                        break;
                    case LogType.STDERR:
                        label.AddToClassList("logStdErr");
                        break;
                }
            };
        }

        private async void InitializeAndStartTask()
        {
            _task = new RivetTask(TaskName, TaskInput);
            _task.OnLog += OnTaskLog;

            AddLogLine("Starting task...", LogType.META);
            UpdateUI();

            try
            {
                var output = await _task.RunAsync();
                OnTaskCompleted(output);
            }
            catch (Exception ex)
            {
                AddLogLine($"Task failed with error: {ex.Message}", LogType.STDERR);
                UpdateUI();
            }
        }

        private void UpdateUI()
        {
            bool running = _task != null && _task.IsRunning;
            _doneButton.text = running ? "Cancel" : "Close";
        }

        private void OnTaskCompleted(Result<JObject> output)
        {
            switch (output)
            {
                case ResultOk<JObject> ok:
                    EditorApplication.delayCall += () => AddLogLine($"Exited with {ok.Data.ToString(Formatting.None)}", LogType.META);

                    break;
                case ResultErr<JObject> err:
                    AddLogLine(err.Message, LogType.STDERR);
                    break;
            }

            UpdateUI();
        }

        private void StopTask()
        {
            if (_task == null)
                return;

            _task.Kill();
            AddLogLine("Stopped task", LogType.META);
            UpdateUI();
        }

        private void OnTaskLog(string logs, RivetTask.LogType type)
        {
            LogType logType = type switch
            {
                RivetTask.LogType.STDOUT => LogType.STDOUT,
                RivetTask.LogType.STDERR => LogType.STDERR,
                _ => LogType.META
            };

            EditorApplication.delayCall += () =>
            {
                AddLogLine(logs, logType);
            };
        }

        private void AddLogLine(string message, LogType type)
        {
            _logEntries.Add(new LogEntry(message, type));
            _logListView.Rebuild();
            _logListView.ScrollToItem(-1);
        }

        private void OnDonePressed()
        {
            StopTask();
            Close();
        }

        private void OnDestroy()
        {
            StopTask();
        }
    }
}