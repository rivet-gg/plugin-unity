using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Rivet.Editor;
using Rivet.Editor.UI;
using Unity.VisualScripting.YamlDotNet.Core.Tokens;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace Rivet.UI.Tabs
{
    public class SetupController
    {
        private readonly RivetPlugin _pluginWindow;
        private readonly VisualElement _root;

        public SetupController(RivetPlugin window, VisualElement root)
        {
            _pluginWindow = window;
            _root = root;

            InitUI();
        }

        void InitUI()
        {
        }
    }
}