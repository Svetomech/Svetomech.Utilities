using System;

namespace Svetomech.Utilities.Types
{
    public interface IWindow
    {
        IntPtr Handle { get; set; }
        bool IsShown { get; }
        string Title { get; }

        void Hide();
        void SetPosition(int X, int Y);
        void Show();
        string ToString();
    }
}