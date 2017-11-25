using Svetomech.Utilities.Types;
using System;
using System.Collections.Generic;

namespace Svetomech.Utilities
{
    public static class WindowFactory
    {
        public static IWindow Create(string handle)
        {
            if (String.IsNullOrWhiteSpace(handle))
            {
                throw new ArgumentException(nameof(handle));
            }

            return Create(new IntPtr(int.Parse(handle)));
        }
        public static IWindow Create(IntPtr handle)
        {
            return new Window(handle);
        }
        /// <summary>
        /// Use with accuracy. Only accepts string and IntPtr as T.
        /// </summary>
        public static IEnumerable<IWindow> CreateMultiple<T>(IEnumerable<T> handles)
        {
            foreach (var handle in handles)
            {
                yield return Create(handle.ToString());
            }
        }
    }
}
