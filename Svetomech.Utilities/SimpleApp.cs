using System.Runtime.InteropServices;
using static Microsoft.Win32.Registry;

namespace Svetomech.Utilities
{
  public static class SimpleApp
  {
    public static bool IsElevated()
    {
      return runningWindows ? WindowsApp.IsElevated() : LinuxApp.IsElevated();
    }

    // TODO: Add cross-platform support
    public static void SwitchAutorun(string appName, string appPath = null)
    {
      string regPath = null;

      using (var regKey = CurrentUser.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Run", false))
      {
        regPath = regKey.GetValue(appName)?.ToString();
      }

      if (SimpleIO.Path.Equals(appPath, regPath))
      {
        return;
      }

      using (var regKey = CurrentUser.CreateSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Run"))
      {
        if (appPath != null)
        {
          regKey.SetValue(appName, appPath);
        }
        else
        {
          regKey.DeleteValue(appName);
        }
      }
    }

    // TODO: Add cross-platform support
    public static bool VerifyAutorun(string appName, string appPath)
    {
      string regPath = null;

      using (var regKey = CurrentUser.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Run", false))
      {
        regPath = regKey.GetValue(appName)?.ToString();
      }

      return SimpleIO.Path.Equals(appPath, regPath);
    }

    private static readonly bool runningWindows = (SimplePlatform.RunningPlatform() == SimplePlatform.Platform.Windows);


    private static class WindowsApp
    {
      internal static bool IsElevated()
      {
        try { using (LocalMachine.OpenSubKey(@"SOFTWARE\", true)) ; }
        catch { return false; }
        return true;
      }
    }

    private static class LinuxApp
    {
      [DllImport("libc")]
      private static extern uint getuid();

      internal static bool IsElevated() => (getuid() == 0);
    }
  }
}
