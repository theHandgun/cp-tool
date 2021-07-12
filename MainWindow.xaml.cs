using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interop;

namespace ScreenTextWriter
{

    public partial class MainWindow : Window
    {
        // Fields for global keybinding.
        private IntPtr _windowHandle;
        private HwndSource _source;
        //-----------------------------
        public MainWindow()
        {
            double screenWidth = System.Windows.SystemParameters.PrimaryScreenWidth;

            InitializeComponent();
            Left = screenWidth - Width;
            Top = 35; // Not smart to use a direct value here but since it is very small, shouldn't be a problem on different sized monitors.
            Topmost = true;

            RadioTxt.Visibility = Visibility.Visible;
            CodesTxt.Visibility = Visibility.Collapsed;
            AbbreviationsTxt.Visibility = Visibility.Collapsed;
            
        }

        void ChangeTextByIndex(int index){
            switch (index)
            {
                case 0:
                    CollapseAllText();
                    RadioTxt.Visibility = Visibility.Visible;
                    break;
                case 1:
                    CollapseAllText();
                    CodesTxt.Visibility = Visibility.Visible;
                    break;
                case 2:
                    CollapseAllText();
                    AbbreviationsTxt.Visibility = Visibility.Visible;
                    break;
                case 3:
                    CollapseAllText();
                    break;
            }
        }

        void CollapseAllText()
        {
            RadioTxt.Visibility = Visibility.Collapsed;
            CodesTxt.Visibility = Visibility.Collapsed;
            AbbreviationsTxt.Visibility = Visibility.Collapsed;
        }
        protected override void OnClosed(EventArgs e)
        {
            _source.RemoveHook(HwndHook);
            UnregisterHotKey(_windowHandle, HOTKEY_ID);
            base.OnClosed(e);
        }


        // -- Low level global keybinding, let's us process keybinds even when the system is not focused on the form.
        // -- Code bellow really should be in another file but I am too lazy to do it. (the tool is finalized in one file anyways)

        [DllImport("user32.dll")]
        private static extern bool RegisterHotKey(IntPtr hWnd, int id, uint fsModifiers, uint vk);

        [DllImport("user32.dll")]
        private static extern bool UnregisterHotKey(IntPtr hWnd, int id);

        private const int HOTKEY_ID = 9000;

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

        protected override void OnSourceInitialized(EventArgs e)
        {
            base.OnSourceInitialized(e);

            _windowHandle = new WindowInteropHelper(this).Handle;
            _source = HwndSource.FromHwnd(_windowHandle);
            _source.AddHook(HwndHook);

            RegisterHotKey(_windowHandle, HOTKEY_ID, MOD_NONE, VK_DEL); // Delete
            RegisterHotKey(_windowHandle, HOTKEY_ID, MOD_NONE, VK_F2); // F2
            RegisterHotKey(_windowHandle, HOTKEY_ID, MOD_NONE, VK_F3); // F3
            RegisterHotKey(_windowHandle, HOTKEY_ID, MOD_NONE, VK_F4); // F4
        }

        private IntPtr HwndHook(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            const int WM_HOTKEY = 0x0312;
            switch (msg)
            {
                case WM_HOTKEY:
                    switch (wParam.ToInt32())
                    {
                        case HOTKEY_ID:
                            int vkey = (((int)lParam >> 16) & 0xFFFF);
                            switch (vkey)
                            {
                                case (int)VK_F2:
                                    ChangeTextByIndex(0);
                                    break;
                                case (int)VK_F3:
                                    ChangeTextByIndex(1);
                                    break;
                                case (int)VK_F4:
                                    ChangeTextByIndex(2);
                                    break;
                                case (int)VK_DEL:
                                    ChangeTextByIndex(3);
                                    break;
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
