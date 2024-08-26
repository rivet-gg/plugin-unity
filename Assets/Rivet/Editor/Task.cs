using UnityEngine;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Diagnostics;
using Unity.VisualScripting;

namespace Rivet.Editor
{
    public class RivetTask : IDisposable
    {
        public enum LogType { STDOUT, STDERR }

        private const float POLL_LOGS_INTERVAL = 0.25f;

        // Config
        private string _name;
        private JObject _input;

        // State
        public bool IsRunning { get; private set; } = false;
        public bool IsFinished { get; private set; } = false;
        private StateFiles _stateFiles;
        private CancellationTokenSource _cts;
        private FileStream _logFileStream;
        private long _logLastPosition;
        private bool _disposed = false;

        public delegate void LogHandler(string message, LogType type);
        public event LogHandler OnLog;

        private class StateFiles
        {
            public string BasePath;
            public string AbortPath;
            public string OutputPath;
        }

        public RivetTask(string name, JObject input)
        {
            if (string.IsNullOrEmpty(name) || input == null)
            {
                throw new ArgumentException("RivetTask initiated without required args");
            }

            _name = name;
            _input = input;
            _stateFiles = GenerateStateFilesDir();
        }

        public async Task<Result<JObject>> RunAsync(CancellationToken cancellationToken = default)
        {
            if (IsRunning)
            {
                throw new InvalidOperationException("Task is already running");
            }

            IsRunning = true;
            _cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);

            try
            {
                var runConfig = new JObject
                {
                    ["abort_path"] = _stateFiles.AbortPath,
                    ["output_path"] = _stateFiles.OutputPath,
                };

                RivetLogger.Log($"Task {_name} Request: {_input.ToString(Formatting.None)}");

                var logPollingTask = StartLogPolling(_cts.Token);
                var result = await Task.Run(() => RunTask(_name, _input, runConfig), _cts.Token);
                IsRunning = false;
                IsFinished = true;

                // Read end of logs immediately. Log polling task will cancel
                // itself.
                await Task.Run(() => ReadLogTail());

                return result;
            }
            finally
            {
                IsRunning = false;
                IsFinished = true;
                FinishLogs();
                Dispose();  // Self-dispose after the task is complete
            }
        }

        private async Task StartLogPolling(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested && IsRunning)
            {
                ReadLogTail();
                await Task.Delay(TimeSpan.FromSeconds(POLL_LOGS_INTERVAL), cancellationToken);
            }
        }

        private Result<JObject> RunTask(string name, JObject input, JObject runConfig)
        {
            try
            {
                var outputRaw = RivetToolchain.RunTaskRaw(runConfig.ToString(Formatting.None), name, input.ToString(Formatting.None));
                var output = JObject.Parse(outputRaw);
                if (output.GetValue("Ok")?.Value<JObject>() is { } okInner)
                {
                    RivetLogger.Log($"Task {name} Success: {okInner.ToString(Formatting.None)}");
                    return new ResultOk<JObject>(okInner);
                }
                else if (output.GetValue("Err") is { } errInner)
                {
                    RivetLogger.Log($"Task {name} Err: {errInner}");
                    return new ResultErr<JObject>(errInner.ToString(Formatting.None));
                }
                else
                {
                    throw new Exception("unreachable");
                }
            }
            catch (Exception error)
            {
                UnityEngine.Debug.LogError($"Error running task: {error}");
                return new ResultErr<JObject>(error.ToString());
            }
        }

        // private Result<JObject> RunTask(string name, JObject input, JObject runConfig)
        // {
        //     var output = RunRivetCLI("task", "run", "--run-config", runConfig.ToString(Formatting.None), "--name", name, "--input", input.ToString(Formatting.None));
        //     switch (output)
        //     {
        //         case ResultOk<JObject> ok:
        //             if (ok.Data.GetValue("Ok")?.Value<JObject>() is { } okInner)
        //             {
        //                 RivetLogger.Log($"Task {name} Success: {okInner.ToString(Formatting.None)}");
        //                 return new ResultOk<JObject>(okInner);
        //             }
        //             else if (ok.Data.GetValue("Err") is { } errInner)
        //             {
        //                 RivetLogger.Log($"Task {name} Err: {errInner}");
        //                 return new ResultErr<JObject>(errInner.ToString(Formatting.None));
        //             }
        //             else
        //             {
        //                 throw new Exception("unreachable");
        //             }
        //         case ResultErr<JObject> err:
        //             RivetLogger.Log($"Task {name} Command Err: {err.Message}");
        //             return err;
        //         default:
        //             throw new Exception("unreachable");
        //     }
        // }

        // TODO: Remove this
        public static string GetRivetCLIPath()
        {
            // TODO: Update this path as needed
            // return "/Users/nathan/rivet/cli/target/debug/rivet-cli";
            return "/Users/nathan/rivet/cli/target/debug/rivet";
        }

        // private static Result<JObject> RunRivetCLI(params string[] args)
        // {
        //     // TODO: Turn this on if debug is enabled
        //     RivetLogger.Log($"Running Rivet CLI: {GetRivetCLIPath()} {string.Join(" ", args)}");

        //     if (!File.Exists(GetRivetCLIPath()))
        //     {
        //         return new ResultErr<JObject>("File does not exist: " + GetRivetCLIPath());
        //     }

        //     var startInfo = new ProcessStartInfo
        //     {
        //         FileName = GetRivetCLIPath(),
        //         RedirectStandardOutput = true,
        //         RedirectStandardError = true,
        //         UseShellExecute = false,
        //         CreateNoWindow = true,
        //     };
        //     foreach (var arg in args) {
        //         startInfo.ArgumentList.Add(arg);
        //     }

        //     try
        //     {
        //         using var process = Process.Start(startInfo);
        //         var stdout = process.StandardOutput.ReadToEnd();
        //         var stderr = process.StandardError.ReadToEnd();
        //         process.WaitForExit();

        //         RivetLogger.Log($"Process output:\n\n{stdout}\n\n{stderr}");

        //         if (process.ExitCode == 0)
        //         {
        //             return new ResultOk<JObject>(JObject.Parse(stdout));
        //         }
        //         else
        //         {
        //             return new ResultErr<JObject>($"Process failed with exit code {process.ExitCode}\n\nstdout:\n{stdout}\n\nstderr:\n{stderr}");
        //         }
        //     }
        //     catch (System.Exception ex)
        //     {
        //         return new ResultErr<JObject>("Failed to start process: " + ex.Message);
        //     }

        // }

        public void Kill()
        {
            if (!IsRunning) return;

            RivetLogger.Log("Aborting process");
            File.Create(_stateFiles.AbortPath).Dispose();
            _cts?.Cancel();
        }

        private StateFiles GenerateStateFilesDir()
        {
            var tempBaseDir = Path.Combine(Application.temporaryCachePath, "tmp");
            var uniqueId = DateTime.Now.Ticks;
            var tempDirPath = Path.Combine(tempBaseDir, $"task_{_name}_{uniqueId}");

            try
            {
                Directory.CreateDirectory(tempDirPath);
                var tempAbortPath = Path.Combine(tempDirPath, "abort");
                var tempOutputPath = Path.Combine(tempDirPath, "output");
                File.Create(tempOutputPath).Dispose();

                return new StateFiles
                {
                    BasePath = tempDirPath,
                    AbortPath = tempAbortPath,
                    OutputPath = tempOutputPath
                };
            }
            catch (Exception e)
            {
                RivetLogger.Error($"Failed to create temporary directory: {e.Message}");
                return null;
            }
        }

        private string StripAnsiCodes(string input)
        {
            return System.Text.RegularExpressions.Regex.Replace(input, @"\x1b\[[0-9;]*m", string.Empty);
        }

        private void ReadLogTail()
        {
            if (_logFileStream == null)
            {
                _logFileStream = new FileStream(_stateFiles.OutputPath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            }

            long currentPosition = _logFileStream.Length;
            if (currentPosition > _logLastPosition)
            {
                _logFileStream.Seek(_logLastPosition, SeekOrigin.Begin);
                byte[] buffer = new byte[currentPosition - _logLastPosition];
                _logFileStream.Read(buffer, 0, buffer.Length);
                _logLastPosition = currentPosition;

                string newContent = System.Text.Encoding.UTF8.GetString(buffer);
                string[] lines = newContent.Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);

                foreach (var line in lines)
                {
                    try
                    {
                        var parsed = JsonConvert.DeserializeObject<Dictionary<string, string>>(line);
                        if (parsed.ContainsKey("Stdout"))
                        {
                            OnLog?.Invoke(StripAnsiCodes(parsed["Stdout"]), LogType.STDOUT);
                        }
                        else if (parsed.ContainsKey("Stderr"))
                        {
                            OnLog?.Invoke(StripAnsiCodes(parsed["Stderr"]), LogType.STDERR);
                        }
                    }
                    catch (JsonException)
                    {
                        RivetLogger.Error($"Failed to parse: {line}");
                    }
                }
            }
        }

        private void FinishLogs()
        {
            _logFileStream?.Dispose();
            _logFileStream = null;
        }

        public void Dispose()
        {
            if (_disposed) return;
            _cts?.Dispose();
            _logFileStream?.Dispose();
            CleanupTempFiles();
            _disposed = true;
        }

        private void CleanupTempFiles()
        {
            if (_stateFiles == null) return;
            try
            {
                if (File.Exists(_stateFiles.AbortPath))
                    File.Delete(_stateFiles.AbortPath);
                if (File.Exists(_stateFiles.OutputPath))
                    File.Delete(_stateFiles.OutputPath);
                if (Directory.Exists(_stateFiles.BasePath))
                    Directory.Delete(_stateFiles.BasePath, true);
            }
            catch (IOException ex)
            {
                RivetLogger.Error($"Error cleaning up temporary files: {ex.Message}");
            }
        }
    }
}