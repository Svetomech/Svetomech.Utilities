using Svetomech.Utilities.Types;
using System;
using System.Runtime.InteropServices;
using static Svetomech.Utilities.SimplePlatform;

namespace Svetomech.Utilities
{
    public static class NativeMethods
    {
        public static Window GetConsoleWindow() => runningWindows ? new Window(WindowsNative.GetConsoleWindow()) 
            : new Window(LinuxNative.GetConsoleWindow());

        private static readonly bool runningWindows = (RunningPlatform() == Platform.Windows);


        private static class WindowsNative
        {
            [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
            internal static extern IntPtr GetConsoleWindow();
        }

        // TODO: Find the actual Linux APIs equivalent to Windows ones
        private static class LinuxNative
        {
            internal static IntPtr GetConsoleWindow() => IntPtr.Zero;
        }
    }
}
