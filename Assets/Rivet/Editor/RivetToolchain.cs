using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Rivet.Editor
{
    public static class RivetToolchain
    {
        const string RustLibrary = "__Internal";

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void EventCallback(ulong taskId, IntPtr eventJson);

        [DllImport(RustLibrary, CallingConvention = CallingConvention.Cdecl, EntryPoint = "rivet_run_task")]
        private static extern ulong run_task(
            [MarshalAs(UnmanagedType.LPStr)] string name,
            [MarshalAs(UnmanagedType.LPStr)] string inputJson,
            EventCallback callback
        );

        [DllImport(RustLibrary, CallingConvention = CallingConvention.Cdecl, EntryPoint = "rivet_abort_task")]
        private static extern bool abort_task(ulong taskId);

        [DllImport(RustLibrary, CallingConvention = CallingConvention.Cdecl, EntryPoint = "rivet_shutdown")]
        private static extern void shutdown();

        [DllImport(RustLibrary, CallingConvention = CallingConvention.Cdecl, EntryPoint = "rivet_free_rust_string")]
        private static extern void free_rust_string(IntPtr str);

        public static ulong RunTask(string name, string inputJson, EventCallback callback)
        {
            return run_task(name, inputJson, callback);
        }

        public static bool AbortTask(ulong taskId)
        {
            return abort_task(taskId);
        }

        public static void Shutdown()
        {
            shutdown();
        }

        public static string PtrToString(IntPtr ptr)
        {
            string result = Marshal.PtrToStringAnsi(ptr);
            free_rust_string(ptr);
            return result;
        }
    }
}