using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using Rivet.Editor.UI.Dock;

namespace Rivet.Editor.UI.TaskPanel
{
    public abstract class TaskPanelWindow : EditorWindow
    {
        internal abstract TaskManager? TaskManager { get; }

        // GUI
        private ListView _logListView;
        private Button _clearButton;

        // Task

        public void CreateGUI()
        {
            var visualTreeAsset = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/Rivet/Editor/UI/TaskPanel/TaskPanel.uxml");
            visualTreeAsset.CloneTree(rootVisualElement);

            _logListView = rootVisualElement.Q<ListView>("LogList");
            _clearButton = rootVisualElement.Q<Button>("ClearButton");

            _clearButton.clicked += OnClearPressed;

            _logListView.makeItem = () => new Label();
            _logListView.bindItem = (element, i) =>
            {
                var label = element as Label;
                var entry = TaskManager.LogEntries[i];
                label.text = entry.Message;
                label.style.fontSize = 12;
                label.style.whiteSpace = WhiteSpace.Normal;
                label.style.overflow = Overflow.Hidden;

                label.ClearClassList();
                switch (entry.Type)
                {
                    case TaskManager.LogType.META:
                        label.AddToClassList("logMeta");
                        break;
                    case TaskManager.LogType.STDOUT:
                        label.AddToClassList("logStdOut");
                        break;
                    case TaskManager.LogType.STDERR:
                        label.AddToClassList("logStdErr");
                        break;
                }
            };

            UpdateLogs();
        }

        public void UpdateLogs()
        {
            // In case the singleton doesn't exist yet
            if (TaskManager != null)
            {
                _logListView.itemsSource = TaskManager.LogEntries;
            }

            // Rebuild view
            _logListView.Rebuild();
            _logListView.ScrollToItem(-1);
        }

        private void OnClearPressed()
        {
            TaskManager?.ClearLogs();
        }
    }

    public class GameServerWindow : TaskPanelWindow
    {
        internal override TaskManager? TaskManager => Rivet.Editor.UI.Dock.Dock.Singleton?.LocalGameServerManager;

        [MenuItem("Window/Rivet/Game Server Logs", false, 20)]
        public static void ShowGameServer()
        {
            var panel = GetWindow<GameServerWindow>(utility: false, null, focus: true);
            panel.titleContent = new GUIContent("Game Server Logs");
        }

        public static GameServerWindow? GetWindowIfExists()
        {
            if (HasOpenInstances<GameServerWindow>())
            {
                return GetWindow<GameServerWindow>(utility: false, null, focus: false);
            }
            else
            {
                return null;
            }
        }
    }

    public class BackendWindow : TaskPanelWindow
    {
        internal override TaskManager? TaskManager => Rivet.Editor.UI.Dock.Dock.Singleton?.BackendManager;

        [MenuItem("Window/Rivet/Backend Logs", false, 10)]
        public static void ShowBackend()
        {
            var panel = GetWindow<BackendWindow>(utility: false, null, focus: true);
            panel.titleContent = new GUIContent("Backend Logs");
        }

        public static BackendWindow? GetWindowIfExists()
        {
            if (HasOpenInstances<BackendWindow>())
            {
                return GetWindow<BackendWindow>(utility: false, null, focus: false);
            }
            else
            {
                return null;
            }
        }
    }
}