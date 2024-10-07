using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using Newtonsoft.Json.Linq;

namespace Rivet.Editor
{
    internal static class RivetToolchainFFI
    {
        const string RustLibrary = "__Internal";

        [StructLayout(LayoutKind.Sequential)]
        public struct RunTaskResult
        {
            public ulong TaskId;
            public byte ErrorCode;
        }

        [DllImport(RustLibrary, CallingConvention = CallingConvention.Cdecl, EntryPoint = "rivet_run_task")]
        public static extern RunTaskResult run_task(
            [MarshalAs(UnmanagedType.LPStr)] string name,
            [MarshalAs(UnmanagedType.LPStr)] string inputJson
        );

        [DllImport(RustLibrary, CallingConvention = CallingConvention.Cdecl, EntryPoint = "rivet_abort_task")]
        public static extern byte abort_task(ulong taskId);

        [DllImport(RustLibrary, CallingConvention = CallingConvention.Cdecl, EntryPoint = "rivet_shutdown")]
        public static extern void shutdown();

        [DllImport(RustLibrary, CallingConvention = CallingConvention.Cdecl, EntryPoint = "rivet_free_rust_string")]
        private static extern void free_rust_string(IntPtr str);

        public static string PtrToString(IntPtr ptr)
        {
            string result = Marshal.PtrToStringAnsi(ptr);
            free_rust_string(ptr);
            return result;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct TaskEvent
        {
            public ulong TaskId;
            public IntPtr EventJson;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct PollTaskEventsResult
        {
            public UIntPtr Count;
            public byte ErrorCode;
        }

        [DllImport(RustLibrary, CallingConvention = CallingConvention.Cdecl, EntryPoint = "rivet_poll_task_events")]
        public static extern PollTaskEventsResult poll_task_events(
            [In, Out] TaskEvent[] events,
            UIntPtr maxEvents
        );

        public const uint POLL_TASK_EVENT_BUFFER = 128;
    }

    public static class RivetToolchain
    {
        private static Dictionary<ulong, Action<JObject>> taskCallbacks = new Dictionary<ulong, Action<JObject>>();
        private static readonly object taskCallbacksLock = new object();

        public static ulong RunTask(string name, string inputJson, Action<JObject> callback)
        {
            RivetToolchainFFI.RunTaskResult result = RivetToolchainFFI.run_task(name, inputJson);
            if (result.ErrorCode != 0)
            {
                throw new Exception($"Rivet task '{name}' failed with error code: {result.ErrorCode}");
            }

            lock (taskCallbacksLock)
            {
                taskCallbacks[result.TaskId] = callback;
            }

            return result.TaskId;
        }

        public static void AbortTask(ulong taskId)
        {
            var error_code = RivetToolchainFFI.abort_task(taskId);
            if (error_code != 0)
            {
                throw new Exception($"Failed to abort Rivet task with ID: {taskId}");
            }
        }

        public static void Shutdown()
        {
            // RivetToolchainFFI.shutdown();
        }

        public class TaskEventData
        {
            public ulong TaskId;
            public JObject EventJson;
        }

        public static List<TaskEventData> PollTaskEvents()
        {
            List<TaskEventData> allEvents = new List<TaskEventData>();
            RivetToolchainFFI.TaskEvent[] buffer = new RivetToolchainFFI.TaskEvent[RivetToolchainFFI.POLL_TASK_EVENT_BUFFER];

            uint totalPolled = 0;
            while (true)
            {
                RivetToolchainFFI.PollTaskEventsResult result = RivetToolchainFFI.poll_task_events(buffer, new UIntPtr(RivetToolchainFFI.POLL_TASK_EVENT_BUFFER));

                if (result.ErrorCode != 0)
                {
                    throw new Exception($"Failed to poll Rivet task events with error code: {result.ErrorCode}");
                }

                uint polledCount = result.Count.ToUInt32();
                totalPolled += polledCount;

                for (int i = 0; i < polledCount; i++)
                {
                    string eventJsonString = RivetToolchainFFI.PtrToString(buffer[i].EventJson);
                    JObject eventJson = JObject.Parse(eventJsonString);

                    // Attempt to get callback
                    Action<JObject> callback = null;
                    lock (taskCallbacksLock)
                    {
                        if (!taskCallbacks.TryGetValue(buffer[i].TaskId, out callback))
                        {
                            RivetLogger.Warning($"Missing callback for task ID: {buffer[i].TaskId}");
                        }
                    }

                    // Call callback
                    callback?.Invoke(eventJson);
                }

                if (polledCount == 0)
                {
                    // No more events to poll
                    break;
                }
            }

            // Log task event
            if (totalPolled > 0)
            {
                RivetLogger.Log($"Polled {totalPolled} task events");
            }

            return allEvents;
        }
    }
}