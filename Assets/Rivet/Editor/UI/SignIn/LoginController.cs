using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Rivet.Editor;
using Rivet.Editor.UI;
using Rivet.Editor.UI.Dock;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace Rivet.Editor.UI.Dock.Tabs
{
    public class LoginController
    {
        private readonly Dock _dock;
        private readonly VisualElement _root;

        private Button _elSignIn;
        private TextField _elApiEndpoint;

        public LoginController(Dock dock, VisualElement root)
        {
            _dock = dock;
            _root = root;

            InitUI();
        }

        public void OnShow()
        {
            _ = CheckLoginState();
        }

        void InitUI()
        {
            _elSignIn = _root.Q<Button>(name: "SignIn");
            _elApiEndpoint = _root.Q(name: "Advanced").Q(name: "unity-content").Q<TextField>(name: "ApiEndpoint");

            // _elSignIn.RegisterCallback<ClickEvent>((ev) =>
            // {
            //     _ = SignIn();
            // });
            // _elApiEndpoint.value = _dock.ApiEndpoint;
        }

        private async Task CheckLoginState()
        {
            // var result = await new RivetTask(
            //     "check_login_state",
            //     new JObject()
            // ).RunAsync();

            // switch (result)
            // {
            //     case ResultOk<JObject> getLinkSuccessResult:
            //         if (getLinkSuccessResult.Data["logged_in"].ToObject<bool>() == true)
            //         {
            //             _dock.SetScreen(Editor.UI.Screen.Main);
            //         }

            //         break;
            // }
        }

        private async Task SignIn()
        {
            // // Get link token
            // var getLinkResult = await new RivetTask(
            //     "start_device_link",
            //     new JObject
            //     {
            //         ["api_endpoint"] = _elApiEndpoint.value,
            //     }
            // ).RunAsync();

            // string deviceLinkToken;
            // switch (getLinkResult)
            // {
            //     case ResultOk<JObject> ok:
            //         deviceLinkToken = ok.Data["device_link_token"].ToString();

            //         // Open browser on main thread
            //         EditorApplication.delayCall += () =>
            //         {
            //             Application.OpenURL(ok.Data["device_link_url"].ToString());
            //         };

            //         break;
            //     case ResultErr<JObject> err:
            //         return;
            //     default:
            //         throw new System.Exception("unreachable");
            // }

            // // Wait until the user has logged in
            // var waitForLoginResult = await new RivetTask(
            //     "wait_for_login",
            //     new JObject
            //     {
            //         ["api_endpoint"] = _elApiEndpoint.value,
            //         ["device_link_token"] = deviceLinkToken,
            //     }
            // ).RunAsync();
            // switch (getLinkResult)
            // {
            //     case ResultOk<JObject> ok:
            //         _dock.ApiEndpoint = _elApiEndpoint.value;
            //         // _dock.SetScreen(Editor.UI.Screen.Main);
            //         break;
            //     case ResultErr<JObject> err:
            //         return;

            // }
        }
    }
}