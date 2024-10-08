using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Rivet.Editor;
using Rivet.Editor.Types;
using Rivet.Editor.UI;
using Rivet.Editor.Util;
using Unity.VisualScripting.YamlDotNet.Core.Tokens;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using Rivet.Editor.UI.TaskPopup;

namespace Rivet.Editor.UI.Dock.Tabs
{
    public class ModulesController
    {
        private readonly Dock _dock;
        private readonly VisualElement _root;

        private readonly DropdownField _environmentDropdown;

        public ModulesController(Dock dock, VisualElement root)
        {
            InitUI();
        }

        void InitUI()
        {
        }
    }
}