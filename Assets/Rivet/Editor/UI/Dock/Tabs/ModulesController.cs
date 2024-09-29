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

        public void OnBootstrap(BootstrapData data)
        {
            // Add environments
            List<string> environments = new();
            foreach (var env in data.Environments)
            {
                environments.Add(env.Name);
            }
            environments.Add("+ New Environment");
            _environmentDropdown.choices = environments;

            if (environments.Count > 0)
            {
                _environmentDropdown.index = 0;
                _dock.RemoteEnvironmentIndex = 0;
            }

            // OnSelectedEnvironmentChange();
        }
    }
}