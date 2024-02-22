using System;
using System.Windows;
using System.Runtime.InteropServices;
using System.Windows.Media;
using System.Windows.Interop;

namespace CreativeArt.Utilities
{
    public static class Win32Helper
    {
        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]

        private static extern bool GetCursorPos(out Win32Point point);

        [DllImport("user32.dll", SetLastError = true)]
        static extern bool GetWindowRect(IntPtr hwnd, out W32Rect lpRect);

        public const uint GW_HWNDNEXT = 2;

        [DllImport("User32")]
        public static extern IntPtr GetTopWindow(IntPtr hWnd);

        [DllImport("User32")]
        public static extern IntPtr GetWindow(IntPtr hWnd, uint wCmd);

        public static Point GetMousePosition(Visual relativeVisual)
        {
            if (GetCursorPos(out Win32Point point))
            {
                Matrix matrixScreen = PresentationSource.FromVisual(relativeVisual).CompositionTarget.TransformToDevice;
                return new Point(point.X / matrixScreen.M11, point.Y / matrixScreen.M22);
            }
            else
            {
                throw new InvalidOperationException("Failed to get mouse position.");
            }
        }

        public static Rect GetWindowRect(Window window)
        {
            var hwnd = new WindowInteropHelper(window).Handle;
            GetWindowRect(hwnd, out var windowRect);
            Matrix matrixScreen = PresentationSource.FromVisual(window).CompositionTarget.TransformToDevice;
            return new Rect(windowRect.Left / matrixScreen.M11, windowRect.Top / matrixScreen.M22, windowRect.Right - windowRect.Left / matrixScreen.M11, windowRect.Bottom - windowRect.Top / matrixScreen.M22);
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    internal struct Win32Point
    {
        public int X;

        public int Y;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct W32Rect
    {
        public int Left;

        public int Top;

        public int Right;

        public int Bottom;
    }
}
