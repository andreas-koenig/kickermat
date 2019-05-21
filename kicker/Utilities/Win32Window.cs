namespace Utilities
{
    using System;
    using System.Diagnostics;
    using System.Windows.Forms;
    
    /// <summary>
    /// Helper class to manage WIN32 window handles
    /// as instances of <see cref="IWin32Window"/>.
    /// </summary>
    public sealed class Win32Window : IWin32Window
    {
        /// <summary>
        /// Singleton object to manage the handle of the application's main window.
        /// </summary>
        private static Win32Window mainWindow = new Win32Window();
           
        /// <summary>
        /// Prevents a default instance of the <see cref="Win32Window"/> class from being created.
        /// </summary>
        private Win32Window()
        {
        }

        /// <summary>
        /// Gets the <see cref="Win32Window"/> instance for the application's main window.
        /// </summary>
        public static Win32Window MainWindow
        {
            get
            {
                mainWindow.Handle = Process.GetCurrentProcess().MainWindowHandle;                
                return mainWindow;
            }

            private set
            {
            }
        }

        /// <summary>
        /// Gets the window handle of this instance.
        /// </summary>
        public System.IntPtr Handle { get; private set; }

        /// <summary>
        /// Gets the <see cref="Win32Window"/> instance for the specified window handle.
        /// </summary>
        /// <param name="hWnd">The handle to the window.</param>
        /// <returns>An instance of <see cref="Win32Window"/> for the window handle.</returns>
        public static Win32Window CreateFromIntPtr(IntPtr hWnd)
        {
            Win32Window window = new Win32Window();
            window.Handle = hWnd;
            return window;
        }    
    }
}