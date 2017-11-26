using System;

namespace Svetomech.Utilities.Types
{
    public interface IWindow
    {
        IntPtr Handle { get; set; }
        bool Visible { get; }
        string Title { get; }

        void Hide();
        void SetPosition(int X, int Y);
        void Show();
        string ToString();
    }
}