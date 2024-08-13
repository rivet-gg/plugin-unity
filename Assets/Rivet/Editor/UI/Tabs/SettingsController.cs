using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Rivet.Editor;
using Rivet.Editor.UI;
using Rivet.Editor.Util;
using Unity.VisualScripting.YamlDotNet.Core.Tokens;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace Rivet.UI.Tabs
{
    public class SettingsController
    {
        private readonly RivetPlugin _pluginWindow;
        private readonly VisualElement _root;

        public SettingsController(RivetPlugin window, VisualElement root)
        {
            _pluginWindow = window;
            _root = root;

            InitUI();
        }

        void InitUI()
        {
            _root.Q("AccountBody").Q("SignOutButton").RegisterCallback<ClickEvent>(ev => { _ = OnUnlinkGame(); });
        }

        private async Task OnBackendStart()
        {
            // HACK: Show term instead of running inline

            var input = new JObject { ["port"] = 6420, ["cwd"] = Builder.ProjectRoot() };
            await new ToolchainTask("show_term", new JObject
            {
                ["command"] = ToolchainTask.GetRivetCLIPath(),
                ["args"] = new JArray { "task", "run", "--run-config", "{}", "--name", "backend_dev", "--input", input.ToString(Formatting.None) },
            }).RunAsync();
        }


        private async Task OnBackendStop()
        {
            RivetLogger.Error("UNIMPLEMENTED");
        }

        private async Task OnUnlinkGame()
        {
            await new ToolchainTask("unlink", new JObject()).RunAsync();
            _pluginWindow.SetScreen(Editor.UI.Screen.Login);
        }
    }
}