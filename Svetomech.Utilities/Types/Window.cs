using System;
using System.Runtime.InteropServices;
using System.Text;
using static Svetomech.Utilities.SimplePlatform;

namespace Svetomech.Utilities.Types
{
    public class Window : IWindow
    {
        private readonly bool runningWindows = (RunningPlatform() == Platform.Windows);

        public string Title => runningWindows ? WindowsWindow.GetTitle(this) : LinuxWindow.GetTitle(this);
        public bool Visible => runningWindows ? WindowsWindow.Visible(this) : LinuxWindow.Visible(this);
        public IntPtr Handle { get; set; } = IntPtr.Zero;

        public Window() { }
        public Window(IntPtr handle)
        {
            Handle = handle;
        }

        public override string ToString() => $"\"{Title}\" - {Handle}";
        public void Hide()
        {
            if (runningWindows)
                WindowsWindow.Hide(this);
            else
                LinuxWindow.Hide(this);
        }
        public void Show()
        {
            if (runningWindows)
                WindowsWindow.Show(this);
            else
                LinuxWindow.Show(this);
        }
        public void SetPosition(int X, int Y)
        {
            if (runningWindows)
                WindowsWindow.SetPosition(this, X, Y);
            else
                LinuxWindow.SetPosition(this, X, Y);
        }

        private class WindowsWindow
        {
            internal static string GetTitle(Window window)
            {
                return GetCaptionOfWindow(window.Handle);

                // https://code.msdn.microsoft.com/windowsapps/C-Getting-the-Windows-da1bd524
                string GetCaptionOfWindow(IntPtr hwnd)
                {
                    string caption = "";
                    StringBuilder windowText = null;
                    try
                    {
                        int max_length = GetWindowTextLength(hwnd);
                        windowText = new StringBuilder("", max_length + 5);
                        GetWindowText(hwnd, windowText, max_length + 2);

                        if (!String.IsNullOrEmpty(windowText.ToString()) && !String.IsNullOrWhiteSpace(windowText.ToString()))
                            caption = windowText.ToString();
                    }
                    catch (Exception ex)
                    {
                        caption = ex.Message;
                    }
                    finally
                    {
                        windowText = null;
                    }
                    return caption;
                }
            }
            internal static bool Visible(Window window)
            {
                return IsWindowVisible(window.Handle);
            }
            internal static bool Hide(Window window)
            {
                const int SW_HIDE = 0;

                return ShowWindow(window.Handle, SW_HIDE);
            }
            internal static bool Show(Window window)
            {
                const int SW_SHOW = 5;

                return ShowWindow(window.Handle, SW_SHOW);
            }
            internal static bool SetPosition(Window window, int X, int Y)
            {
                const int SWP_NOSIZE = 1;
                const int SWP_NOZORDER = 4;
                const int SWP_SHOWWINDOW = 64;

                return SetWindowPos(window.Handle, IntPtr.Zero, X, Y, 0, 0, SWP_NOZORDER | SWP_NOSIZE | SWP_SHOWWINDOW);
            }

            [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
            private static extern int GetWindowTextLength(IntPtr hWnd);
            [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
            private static extern long GetWindowText(IntPtr hWnd, StringBuilder lpString, int nMaxCount);
            [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
            private static extern bool IsWindowVisible(IntPtr hWnd);
            [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
            private static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);
            [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
            private static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, int wFlags);
        }
        // TODO: Find the actual Linux APIs equivalent to Windows ones
        private class LinuxWindow
        {
            internal static string GetTitle(Window window) => null;
            internal static bool Visible(Window window) => true;
            internal static bool Hide(Window window) => false;
            internal static bool Show(Window window) => false;
            internal static bool SetPosition(Window window, int X, int Y) => false;
        }
    }
}
