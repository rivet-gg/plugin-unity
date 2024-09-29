using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Rivet.Editor;
using Rivet.Editor.UI;
using Unity.VisualScripting.YamlDotNet.Core.Tokens;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace Rivet.Editor.UI.Dock.Tabs
{
    public class SetupController
    {
        private readonly Dock _dock;
        private readonly VisualElement _root;

        public SetupController(Dock dock, VisualElement root)
        {
            _dock = dock;
            _root = root;

            InitUI();
        }

        void InitUI()
        {
        }
    }
}