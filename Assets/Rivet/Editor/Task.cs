using UnityEngine;
using System;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using UnityEditor;
using Unity.VisualScripting.YamlDotNet.Serialization.ObjectGraphVisitors;

namespace Rivet.Editor
{
    public class RivetTask : IDisposable
    {
        public enum LogType { STDOUT, STDERR }

        // Events
        public event Action<string, LogType>? TaskLog;
        public event Action<JObject>? TaskOk;
        public event Action<string>? TaskError;
        public event Action<JObject>? TaskOutput;

        // Config
        private string _name;
        private JObject _input;

        // State
        public bool IsRunning { get; private set; } = true;
        public bool IsKilled { get; private set; } = false;
        private JObject _logResult;
        private ulong _taskId;

        public RivetTask(string name, JObject input)
        {
            if (string.IsNullOrEmpty(name) || input == null)
            {
                RivetLogger.Error("RivetTask initiated without required args");
                return;
            }

            _name = name;
            _input = input;

            var inputJson = _input.ToString(Newtonsoft.Json.Formatting.None);

            RivetLogger.Log($"[{name}] Request: {inputJson}");
            Task.Run(() => Run(name, inputJson));
        }

        private void Run(string name, string inputJson)
        {
            _taskId = RivetToolchain.RunTask(name, inputJson, OnOutputEvent);
        }

        private void OnOutputEvent(JObject eventObj)
        {
            if (eventObj.ContainsKey("log"))
            {
                OnLogEvent((string)eventObj["log"]);
            }
            else if (eventObj.ContainsKey("result"))
            {
                // 1st result = event enum type
                // 2nd result = result enum type
                _logResult = eventObj["result"]["result"] as JObject;
                OnFinish();
            }
            else if (eventObj.ContainsKey("port_update"))
            {
                var plugin = RivetGlobal.Singleton;
                plugin.LocalBackendPort = eventObj["port_update"]["backend_port"].Value<int>();
                plugin.LocalEditorPort = eventObj["port_update"]["editor_port"].Value<int>();
                SharedSettings.UpdateFromPlugin();

                RivetLogger.Log($"Port update: backend={plugin.LocalBackendPort} editor={plugin.LocalEditorPort}");
            }
            else if (eventObj.ContainsKey("backend_config_update"))
            {
                var updateEvent = eventObj["backend_config_update"].Value<JObject>();
                // TODO:
            }
            else
            {
                RivetLogger.Warning($"Unknown event {eventObj.ToString(Newtonsoft.Json.Formatting.None)}");
            }
        }

        private void OnLogEvent(string log)
        {
            TaskLog?.Invoke(log, LogType.STDOUT);
        }

        private void OnFinish()
        {
            IsRunning = false;

            JObject outputResult = null;
            if (IsKilled)
            {
                outputResult = new JObject { ["Err"] = "Task killed" };
            }
            else
            {
                if (_logResult == null)
                {
                    RivetLogger.Error("Received no output from task");
                    TaskError?.Invoke("Received no output from task");
                    return;
                }

                outputResult = _logResult;
            }

            TaskOutput?.Invoke(outputResult);
            if (outputResult.ContainsKey("Ok"))
            {
                RivetLogger.Log($"[{_name}] Success: {outputResult["Ok"].ToString(Newtonsoft.Json.Formatting.None)}");
                TaskOk?.Invoke(outputResult["Ok"] as JObject);
            }
            else if (outputResult.ContainsKey("Err"))
            {
                RivetLogger.Error($"[{_name}] Error: {outputResult["Err"].ToString(Newtonsoft.Json.Formatting.None)}");
                TaskError?.Invoke(outputResult["Err"].ToString());
            }
            else
            {
                RivetLogger.Error($"[{_name}] Missing Err or Ok in result: {outputResult.ToString(Newtonsoft.Json.Formatting.None)}");

            }
        }

        public async Task<Result<JObject>> RunAsync()
        {
            var tcs = new TaskCompletionSource<Result<JObject>>();

            TaskOk += (result) => tcs.TrySetResult(new ResultOk<JObject>(result));
            TaskError += (error) => tcs.TrySetResult(new ResultErr<JObject>(error));

            try
            {
                return await tcs.Task;
            }
            finally
            {
                Dispose();
            }
        }

        public void Kill()
        {
            if (!IsRunning || IsKilled)
                return;

            IsKilled = true;

            if (_taskId != 0)
            {
                RivetLogger.Log("Killing task");
                RivetToolchain.AbortTask(_taskId);
            }
            else
            {
                RivetLogger.Warning("No task handle");
            }
        }

        public void Dispose()
        {
            Kill();
        }
    }
}
