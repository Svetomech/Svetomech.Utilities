using Svetomech.Utilities.Types;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace Svetomech.Utilities
{
    public static class SimpleProcess
    {
        public static IEnumerable<IWindow> GetVisibleWindows(string processName)
        {
            if (String.IsNullOrWhiteSpace(processName))
            {
                throw new ArgumentException(nameof(processName));
            }

            string processNameSanitized = Path.GetFileNameWithoutExtension(processName);
            Process[] processInstances = Process.GetProcessesByName(processNameSanitized);
            var processWindowHandles = new List<IntPtr>();

            foreach(var instance in processInstances)
            {
                processWindowHandles.Add(instance.MainWindowHandle);
            }

            return WindowFactory.CreateMultiple(processWindowHandles);
        }
        public static IEnumerable<IWindow> GetVisibleWindows(this Process proc) => GetVisibleWindows(proc.ProcessName);
    }
}
