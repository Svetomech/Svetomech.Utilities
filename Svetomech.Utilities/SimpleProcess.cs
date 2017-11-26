using Svetomech.Utilities.Types;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using static Svetomech.Utilities.SimplePlatform;

namespace Svetomech.Utilities
{
    public static class SimpleProcess
    {
        private static readonly bool runningWindows = RunningPlatform() == Platform.Windows;

        public static IEnumerable<IWindow> GetAllWindows(string processName)
        {
            if (String.IsNullOrWhiteSpace(processName))
            {
                throw new ArgumentException(nameof(processName));
            }

            string processNameSanitized = Path.GetFileNameWithoutExtension(processName);
            Process[] processInstances = Process.GetProcessesByName(processNameSanitized);
            var processWindowHandles = new List<IntPtr>();

            foreach (var instance in processInstances)
            {
                processWindowHandles.AddRange(runningWindows
                    ? WindowsProcess.GetRootWindowHandles(instance.Id)
                    : LinuxProcess.GetRootWindowHandles(instance.Id));
            }

            return WindowFactory.CreateMultiple(processWindowHandles);
        }

        private static class WindowsProcess
        {
            internal static IEnumerable<IntPtr> GetRootWindowHandles(int pid)
            {
                IEnumerable<IntPtr> rootWindows = GetChildWindowHandles(IntPtr.Zero);
                var dsProcRootWindows = new List<IntPtr>();
                foreach (IntPtr hWnd in rootWindows)
                {
                    uint lpdwProcessId;
                    GetWindowThreadProcessId(hWnd, out lpdwProcessId);
                    if (lpdwProcessId == pid)
                        dsProcRootWindows.Add(hWnd);
                }
                return dsProcRootWindows;
            }
            private static IEnumerable<IntPtr> GetChildWindowHandles(IntPtr parent)
            {
                var result = new List<IntPtr>();
                GCHandle listHandle = GCHandle.Alloc(result);
                try
                {
                    var childProc = new Win32Callback(EnumWindow);
                    EnumChildWindows(parent, childProc, GCHandle.ToIntPtr(listHandle));
                }
                finally
                {
                    if (listHandle.IsAllocated)
                        listHandle.Free();
                }
                return result;
            }
            private static bool EnumWindow(IntPtr handle, IntPtr pointer)
            {
                GCHandle gch = GCHandle.FromIntPtr(pointer);
                var list = gch.Target as List<IntPtr>;
                if (list == null)
                {
                    throw new InvalidCastException("GCHandle Target could not be cast as List<IntPtr>");
                }
                list.Add(handle);
                //  You can modify this to check to see if you want to cancel the operation, then return a null here
                return true;
            }

            private delegate bool Win32Callback(IntPtr hwnd, IntPtr lParam);
            [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
            private static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint lpdwProcessId);
            [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
            [return: MarshalAs(UnmanagedType.Bool)]
            private static extern bool EnumChildWindows(IntPtr parentHandle, Win32Callback callback, IntPtr lParam);
        }
        // TODO: Find the actual Linux APIs equivalent to Windows ones
        private static class LinuxProcess
        {
            internal static IEnumerable<IntPtr> GetRootWindowHandles(int pid) => new[] { IntPtr.Zero };
        }
    }
}
