using System;
using System.Runtime.InteropServices;
using static Svetomech.Utilities.SimplePlatform;

namespace Svetomech.Utilities.Types
{
    public class Window
    {
        public IntPtr Handle { get; set; } = IntPtr.Zero;

        public Window() { }

        public Window(IntPtr handle)
        {
            Handle = handle;
        }

        public override string ToString()
        {
            return Handle.ToString();
        }

        public bool IsShown => runningWindows ? WindowsWindow.IsShown(this) : LinuxWindow.IsShown(this);

        public bool Hide() => runningWindows ? WindowsWindow.Hide(this) : LinuxWindow.Hide(this);

        public bool Show() => runningWindows ? WindowsWindow.Show(this) : LinuxWindow.Show(this);

        private readonly bool runningWindows = (RunningPlatform() == Platform.Windows);


        private class WindowsWindow
        {
            internal static bool IsShown(Window window) => IsWindowVisible(window.Handle);

            internal static bool Hide(Window window)
            {
                int SW_HIDE = 0;

                return ShowWindow(window.Handle, SW_HIDE);
            }

            internal static bool Show(Window window)
            {
                int SW_SHOW = 5;

                return ShowWindow(window.Handle, SW_SHOW);
            }

            [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
            private static extern bool IsWindowVisible(IntPtr hWnd);

            [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
            private static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);
        }

        // TODO: Find the actual Linux APIs equivalent to Windows ones
        private class LinuxWindow
        {
            internal static bool IsShown(Window window) => true;

            internal static bool Hide(Window window) => false;

            internal static bool Show(Window window) => false;
        }
    }
}
