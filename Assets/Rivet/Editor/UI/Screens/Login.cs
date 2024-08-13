using UnityEngine;
using UnityEditor;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;

namespace Rivet.Editor.UI.Screens.Login
{
    public class Window : RivetPluginWindow.IState
    {
        private RivetPluginWindow _pluginWindow;
        private string _url;
        private string _apiEndpoint = "https://api.rivet.gg";
        private bool _loginButtonEnabled = true;
        private bool showAdvancedOptions = false;

        public async void OnEnter(RivetPluginWindow pluginWindow)
        {
            // Initialize the URL
            _url = "";

            _pluginWindow = pluginWindow;

            // First, check if we're already logged in
            var result = await new ToolchainTask(
                "check_login_state",
                new JObject()
            ).RunAsync();

            switch (result)
            {
                case ResultOk<JObject> getLinkSuccessResult:
                    if (getLinkSuccessResult.Data["logged_in"].ToObject<bool>() == true)
                    {
                        _pluginWindow.TransitionToState(new Main.Window());
                    }
                    else
                    {
                        _loginButtonEnabled = true;
                    }

                    break;
            }
        }

        public void OnGUI()
        {
            EditorGUI.BeginDisabledGroup(!_loginButtonEnabled);
            if (GUILayout.Button("Sign in to Rivet"))
            {
                _ = SignIn();
            }
            EditorGUI.EndDisabledGroup();

            // Display the Advanced Options dropdown
            showAdvancedOptions = EditorGUILayout.Foldout(showAdvancedOptions, "Advanced");

            if (showAdvancedOptions)
            {
                _apiEndpoint = EditorGUILayout.TextField("API Endpoint", _apiEndpoint);
            }
        }

        private async Task SignIn()
        {
            // Disable the button sign in button
            _loginButtonEnabled = false;

            // Get link token
            var getLinkResult = await new ToolchainTask(
                "start_device_link",
                new JObject
                {
                    ["api_endpoint"] = _apiEndpoint
                }
            ).RunAsync();

            string deviceLinkToken;
            switch (getLinkResult)
            {
                case ResultOk<JObject> ok:
                    deviceLinkToken = ok.Data["device_link_token"].ToString();

                    // Open browser on main thread
                    EditorApplication.delayCall += () =>
                    {
                        Application.OpenURL(ok.Data["device_link_url"].ToString());
                    };

                    break;
                case ResultErr<JObject> err:
                    _loginButtonEnabled = true;
                    return;
                default:
                    throw new System.Exception("unreachable");
            }

            // Wait until the user has logged in
            var waitForLoginResult = await new ToolchainTask(
                "wait_for_login",
                new JObject
                {
                    ["api_endpoint"] = _apiEndpoint,
                    ["device_link_token"] = deviceLinkToken,
                }
            ).RunAsync();
            switch (getLinkResult)
            {
                case ResultOk<JObject> ok:
                    _pluginWindow.ApiEndpoint = _apiEndpoint;
                    _pluginWindow.TransitionToState(new Main.Window());
                    break;
                case ResultErr<JObject> err:
                    _loginButtonEnabled = true;
                    return;

            }
        }
    }
}