using System;
using System.Diagnostics;
using System.IO;

namespace Svetomech.Utilities
{
    public static class SimpleProcess
    {
        public static Window[] GetVisibleWindows(string processName)
        {
            Process[] instances = Process.GetProcessesByName(Path.GetFileNameWithoutExtension(processName));
            Window[] windows = new Window[instances.Length];

            for (int i = 0; i < windows.Length; ++i)
            {
                windows[i] = new Window(instances[i].MainWindowHandle);
            }

            return windows;
        }


        public static Window[] GetVisibleWindows(this Process proc)
        {
            return GetVisibleWindows(proc.ProcessName);
        }
    }
}
