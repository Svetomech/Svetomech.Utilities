using System;

namespace Svetomech.Utilities
{
    public static class WindowFactory
    {
        public static class Populate
        {
            public static Window[] FromHandles(string[] handles)
            {
                var windows = new Window[handles.Length];

                for (int i = 0; i < windows.Length; ++i)
                {
                    windows[i] = new Window(new IntPtr(Convert.ToInt32(handles[i])));
                }

                return windows;
            }
        }
    }
}
