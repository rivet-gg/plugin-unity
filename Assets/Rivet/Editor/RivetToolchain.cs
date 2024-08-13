// namespace Rivet.Editor
// {
//     public static class RivetToolchain
//     {
//         public const string NativeLib = "rivet_toolchain";

//         [DIlImport(NativeLib, CallingConvention = CallingConvention.Cdecl, EntryPoint = "run_task")]
//         public static extern uint run_task();
//     }
// }

using System;
using System.Runtime.InteropServices;

namespace Rivet.Editor
{
    public class RivetToolchain
    {
        const string RustLibrary = "__Internal";


        [DllImport(RustLibrary, CallingConvention = CallingConvention.Cdecl, EntryPoint = "run_task")]
        private static extern IntPtr run_task(
            [MarshalAs(UnmanagedType.LPStr)] string run_config,
            [MarshalAs(UnmanagedType.LPStr)] string name,
            [MarshalAs(UnmanagedType.LPStr)] string input_json
        );

        [DllImport(RustLibrary, CallingConvention = CallingConvention.Cdecl, EntryPoint = "free_rust_string")]
        private static extern void free_rust_string(IntPtr str);

        public static string RunTaskRaw(string runConfig, string name, string inputJson)
        {
            IntPtr resultPtr = run_task(runConfig, name, inputJson);
            string result = Marshal.PtrToStringAnsi(resultPtr);
            free_rust_string(resultPtr);
            return result;
        }
    }
}