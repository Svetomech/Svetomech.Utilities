using System;
using System.IO;
using System.Runtime.InteropServices;
using static Microsoft.Win32.Registry;
using static Svetomech.Utilities.SimplePlatform;

namespace Svetomech.Utilities
{
    public static class App
    {
        public static bool IsElevated() => runningWindows ? WindowsApp.IsElevated() : LinuxApp.IsElevated();

        public static bool VerifyAutorun(string appName, string appPath) => runningWindows ?
            WindowsApp.VerifyAutorun(appName, appPath) : LinuxApp.VerifyAutorun(appName, appPath);

        public static void SwitchAutorun(string appName, string appPath = null, bool isConsoleApp = false)
        {
            if (runningWindows)
                WindowsApp.SwitchAutorun(appName, appPath, isConsoleApp);
            else
                LinuxApp.SwitchAutorun(appName, appPath, isConsoleApp);
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
                string autorunAppPath = readAutorunAppPath(appName);

                return SimpleIO.Path.Equals(appPath, autorunAppPath);
            }

            internal static void SwitchAutorun(string appName, string appPath = null, bool isConsoleApp = false)
            {
                string autorunAppPath = readAutorunAppPath(appName);

                if (SimpleIO.Path.Equals(appPath, autorunAppPath))
                {
                    return;
                }

                using (var regKey = CurrentUser.CreateSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Run"))
                {
                    if (appPath != null)
                    {
                        appPath = SimpleIO.Path.AddQuotesIfNeeded(appPath);

                        regKey.SetValue(appName, isConsoleApp ? $"cmd {appPath}" : appPath);
                    }
                    else
                    {
                        regKey.DeleteValue(appName);
                    }
                }
            }

            private static string readAutorunAppPath(string appName)
            {
                string autorunRegValue = null;
                try
                {
                    using (var regKey = CurrentUser.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Run", false))
                    {
                        autorunRegValue = regKey?.GetValue(appName)?.ToString();
                    }
                }
                catch { }

                if (String.IsNullOrWhiteSpace(autorunRegValue))
                {
                    return null;
                }

                int index = autorunRegValue.IndexOf(':') - 1;
                index = (index < 0) ? autorunRegValue.IndexOf(Path.DirectorySeparatorChar) : index;

                return (index != -1) ? autorunRegValue.Substring(index).TrimEnd('"') : null;
            }
        }

        private static class LinuxApp
        {
            internal static bool IsElevated() => (getuid() == 0);

            internal static bool VerifyAutorun(string appName, string appPath)
            {
                string autorunAppPath = readAutorunAppPath(appName);

                return SimpleIO.Path.Equals(appPath, autorunAppPath);
            }

            internal static void SwitchAutorun(string appName, string appPath = null, bool isConsoleApp = false)
            {
                string autorunAppPath = readAutorunAppPath(appName);

                if (SimpleIO.Path.Equals(appPath, autorunAppPath))
                {
                    return;
                }

                if (appPath != null)
                {
                    string ica = isConsoleApp.ToString().ToLower();
                    
                    string[] autorunFileLines = { "[Desktop Entry]", "Type=Application", $"Name={appName}",
                                                 $"Comment={appName} Startup", "X-GNOME-Autostart-enabled=true",
                                                 $"Exec={appPath}", "NoDisplay=true", $"Terminal={ica}" };

                    File.WriteAllLines(autorunFilePath, autorunFileLines);
                }
                else
                {
                    File.Delete(autorunFilePath);
                }
            }

            [DllImport("libc")]
            private static extern uint getuid();

            private static string autorunFilePath;
            private static string readAutorunAppPath(string appName)
            {
                string autorunFolderName = "autostart";
                string autorunFolderPath = Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), autorunFolderName);

                autorunFilePath = Path.Combine(autorunFolderPath, $"{appName.ToLower()}-{autorunFolderName}.desktop");

                string autorunFileLine = null;
                try
                {
                    using (var reader = new StreamReader(autorunFilePath))
                    {
                        while (!(autorunFileLine = reader.ReadLine()).StartsWith("Exec")) ;
                    }
                }
                catch { }

                if (String.IsNullOrWhiteSpace(autorunFileLine))
                {
                    return null;
                }

                int index = autorunFileLine.IndexOf('~');
                index = (index < 0) ? autorunFileLine.IndexOf(Path.DirectorySeparatorChar) : index;

                return (index != -1) ? autorunFileLine.Substring(index).TrimEnd('"') : null;
            }
        }
    }
}
