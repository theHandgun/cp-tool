using System;
using System.Windows;
using System.Windows.Interop;
using ScreenTextWriter;

namespace ScreenTextWriter
{
    public partial class HUDWindow : Window
    {
        private IntPtr _windowHandle;
        private HwndSource _source;

        public HUDWindow()
        {
            InitializeComponent();

            // Set window size to cover the entire screen
            Width = SystemParameters.PrimaryScreenWidth;
            Height = SystemParameters.PrimaryScreenHeight;
            Left = 0;
            Top = 0;
            Topmost = true;

            CollapseAll();
            RadioTxt.Visibility = Visibility.Visible;
        }

        void CollapseAll()
        {
            RadioTxt.Visibility = Visibility.Collapsed;
            CodesTxt.Visibility = Visibility.Collapsed;
            AbbreviationsTxt.Visibility = Visibility.Collapsed;
            MapImg.Visibility = Visibility.Collapsed;
            ChargesImg.Visibility = Visibility.Collapsed;
        }

        protected override void OnClosed(EventArgs e)
        {
            _source.RemoveHook(HwndHook);
            HotkeyManager.UnregisterHotkeys(_windowHandle);
            base.OnClosed(e);
        }

        protected override void OnSourceInitialized(EventArgs e)
        {
            base.OnSourceInitialized(e);

            _windowHandle = new WindowInteropHelper(this).Handle;
            _source = HwndSource.FromHwnd(_windowHandle);
            _source.AddHook(HwndHook);

            // Register hotkeys
            HotkeyManager.RegisterHotkeys(_windowHandle);
        }

        private IntPtr HwndHook(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            const int WM_HOTKEY = 0x0312;
            if (msg == WM_HOTKEY)
            {
                int vkey = (((int)lParam >> 16) & 0xFFFF);
                int hotkeyId = wParam.ToInt32();

                CollapseAll();

                if (hotkeyId == HotkeyConstants.HOTKEY_ID_1)
                {
                    switch (vkey)
                    {
                        case (int)HotkeyConstants.VK_F2:
                            RadioTxt.Visibility = Visibility.Visible;
                            break;
                        case (int)HotkeyConstants.VK_F3:
                            CodesTxt.Visibility = Visibility.Visible;
                            break;
                        case (int)HotkeyConstants.VK_F4:
                            AbbreviationsTxt.Visibility = Visibility.Visible;
                            break;
                    }
                }
                else if (hotkeyId == HotkeyConstants.HOTKEY_ID_2)
                {
                    switch (vkey)
                    {
                        case (int)HotkeyConstants.VK_F2:
                            ChargesImg.Visibility = Visibility.Visible;
                            break;
                        case (int)HotkeyConstants.VK_F3:
                            MapImg.Visibility = Visibility.Visible;
                            break;
                    }
                }

                handled = true;
            }

            return IntPtr.Zero;
        }
    }
}
