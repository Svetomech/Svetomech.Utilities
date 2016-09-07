using System;
using System.Runtime.InteropServices;

namespace Svetomech.Utilities
{
  public static class NativeMethods
  {
    public static IntPtr GetConsoleWindow()
    {
      return runningWindows ? WindowsNative.GetConsoleWindow() : LinuxNative.GetConsoleWindow();
    }

    public static bool ShowWindow(IntPtr hWnd, int nCmdShow)
    {
      return runningWindows ? WindowsNative.ShowWindow(hWnd, nCmdShow) : LinuxNative.ShowWindow(hWnd, nCmdShow);
    }
    public const int SW_HIDE = 0;
    public const int SW_SHOW = 5;

    private static readonly bool runningWindows = (SimplePlatform.RunningPlatform() == SimplePlatform.Platform.Windows);


    private static class WindowsNative
    {
      [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
      internal static extern IntPtr GetConsoleWindow();

      [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
      internal static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);
    }

    // TODO: Find the actual Linux APIs equivalent to Windows ones
    private static class LinuxNative
    {
      internal static IntPtr GetConsoleWindow() => IntPtr.Zero;

      internal static bool ShowWindow(IntPtr hWnd, int nCmdShow) => true;
    }
  }
}
