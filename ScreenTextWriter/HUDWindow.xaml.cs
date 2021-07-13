using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interop;

namespace ScreenTextWriter
{

    public partial class HUDWindow : Window
    {
        // Fields for global keybinding.
        private IntPtr _windowHandle;
        private HwndSource _source;
        //-----------------------------
        public HUDWindow()
        {
            InitializeComponent();
            double screenWidth = SystemParameters.PrimaryScreenWidth;
            double screenHeight = SystemParameters.PrimaryScreenHeight;

            Width = screenWidth;
            Height = screenHeight;

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
            SewersImg.Visibility = Visibility.Collapsed;
            ChargesImg.Visibility = Visibility.Collapsed;
        }
        protected override void OnClosed(EventArgs e)
        {
            _source.RemoveHook(HwndHook);
            UnregisterHotKey(_windowHandle, HOTKEY_ID_1);
            UnregisterHotKey(_windowHandle, HOTKEY_ID_2);
            base.OnClosed(e);
        }


        // -- Low level global keybinding, let's us process keybinds even when the system is not focused on the form.
        // -- Code bellow really should be in another file but I am too lazy to do it. (the tool is finalized in one file anyways)

        [DllImport("user32.dll")]
        private static extern bool RegisterHotKey(IntPtr hWnd, int id, uint fsModifiers, uint vk);

        [DllImport("user32.dll")]
        private static extern bool UnregisterHotKey(IntPtr hWnd, int id);

        private const int HOTKEY_ID_1 = 9000;
        private const int HOTKEY_ID_2 = 9001;

        //Modifiers:
        private const uint MOD_NONE = 0x0000; //(none)
        private const uint MOD_ALT = 0x0001; //ALT
        private const uint MOD_CONTROL = 0x0002; //CTRL
        private const uint MOD_SHIFT = 0x0004; //SHIFT
        private const uint MOD_WIN = 0x0008; //WINDOWS

        private const uint VK_DEL = 0x2E;
        private const uint VK_F2 = 0x71;
        private const uint VK_F3 = 0x72;
        private const uint VK_F4 = 0x73;
        private const uint VK_M = 0x4D;
        private const uint VK_N = 0x4E;

        protected override void OnSourceInitialized(EventArgs e)
        {
            base.OnSourceInitialized(e);

            _windowHandle = new WindowInteropHelper(this).Handle;
            _source = HwndSource.FromHwnd(_windowHandle);
            _source.AddHook(HwndHook);

            RegisterHotKey(_windowHandle, HOTKEY_ID_1, MOD_NONE, VK_DEL); // Delete
            RegisterHotKey(_windowHandle, HOTKEY_ID_1, MOD_NONE, VK_F2); // F2
            RegisterHotKey(_windowHandle, HOTKEY_ID_1, MOD_NONE, VK_F3); // F3
            RegisterHotKey(_windowHandle, HOTKEY_ID_1, MOD_NONE, VK_F4); // F4
            RegisterHotKey(_windowHandle, HOTKEY_ID_1, MOD_SHIFT, VK_M); // M
            RegisterHotKey(_windowHandle, HOTKEY_ID_1, MOD_SHIFT, VK_N); // N

            RegisterHotKey(_windowHandle, HOTKEY_ID_2, MOD_SHIFT, VK_F2); // Shift + F2
        }

        private IntPtr HwndHook(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            const int WM_HOTKEY = 0x0312;
            switch (msg)
            {
                case WM_HOTKEY:
                    int vkey = (((int)lParam >> 16) & 0xFFFF);
                    switch (wParam.ToInt32())
                    {
                        case HOTKEY_ID_1:
                            CollapseAll();
                            switch (vkey)
                            {
                                case (int)VK_F2:
                                    RadioTxt.Visibility = Visibility.Visible;
                                    break;
                                case (int)VK_F3:
                                    CodesTxt.Visibility = Visibility.Visible;
                                    break;
                                case (int)VK_F4:
                                    AbbreviationsTxt.Visibility = Visibility.Visible;
                                    break;
                                case(int)VK_M:
                                    MapImg.Visibility = Visibility.Visible;
                                    break;
                                case (int)VK_N:
                                    SewersImg.Visibility = Visibility.Visible;
                                    break;
                            }
                            break;

                        case HOTKEY_ID_2:
                            CollapseAll();
                            if (vkey == (int)VK_F2)
                            {
                                ChargesImg.Visibility = Visibility.Visible;
                            }
                            handled = true;
                            break;

                    }
                    break;
            }
            return IntPtr.Zero;
        }
    }


}
