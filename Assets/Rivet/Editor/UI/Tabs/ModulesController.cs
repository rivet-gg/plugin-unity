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
using Rivet.UI.Screens;
using Unity.VisualScripting.YamlDotNet.Core.Tokens;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using Rivet.Editor.UI.TaskPopup;

namespace Rivet.UI.Tabs
{
    public class ModulesController
    {
        private readonly RivetPlugin _pluginWindow;
        private MainController _mainController;
        private readonly VisualElement _root;

        public ModulesController(RivetPlugin window, MainController mainController, VisualElement root)
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
                _mainController.RemoteEnvironmentIndex = 0;
            }

            OnSelectedEnvironmentChange();
        }
    }
}