using System.Runtime.InteropServices;
using static Microsoft.Win32.Registry;
using static Svetomech.Utilities.SimplePlatform;
using static Svetomech.Utilities.SimpleIO;

namespace Svetomech.Utilities
{
    public static class SimpleApp
    {
        public static bool IsElevated() => runningWindows ? WindowsApp.IsElevated() : LinuxApp.IsElevated();

        public static bool VerifyAutorun(string appName, string appPath) => runningWindows ?
            WindowsApp.VerifyAutorun(appName, appPath) : LinuxApp.VerifyAutorun(appName, appPath);

        public static void SwitchAutorun(string appName, string appPath = null)
        {
            if (runningWindows)
                WindowsApp.SwitchAutorun(appName, appPath);
            else
                LinuxApp.SwitchAutorun(appName, appPath);
        }

        private static readonly bool runningWindows = (RunningPlatform() == Platform.Windows);


        private static class WindowsApp
        {
            internal static bool IsElevated()
            {
                try { using (LocalMachine.OpenSubKey(@"SOFTWARE\", true)) ; }
                catch { return false; }
                return true;
            }

            internal static bool VerifyAutorun(string appName, string appPath)
            {
                string regPath = null;

                using (var regKey = CurrentUser.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Run", false))
                {
                    regPath = regKey.GetValue(appName)?.ToString();
                }

                return Path.Equals(appPath, regPath);
            }

            internal static void SwitchAutorun(string appName, string appPath = null)
            {
                string regPath = null;

                using (var regKey = CurrentUser.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Run", false))
                {
                    regPath = regKey.GetValue(appName)?.ToString();
                }

                if (Path.Equals(appPath, regPath))
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
        }

        private static class LinuxApp
        {
            internal static bool IsElevated() => (getuid() == 0);

            internal static bool VerifyAutorun(string appName, string appPath)
            {
                //.config/autostart/simplemaid-autostart.desktop
            }

            internal static void SwitchAutorun(string appName, string appPath = null)
            {
                string autorunFileLines = { "[Desktop Entry]", "Type=Application", $"Name={appName}",
                    $"Comment={appName} Startup", $"Exec=mono {appPath}", "NoDisplay=true", 
                    "X-GNOME-Autostart-enabled=true", "Terminal=true" };
            }

            [DllImport("libc")]
            private static extern uint getuid();
        }
    }
}
