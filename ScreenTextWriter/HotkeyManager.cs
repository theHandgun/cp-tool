using System;
using System.Runtime.InteropServices;
using System.Windows.Interop;

namespace ScreenTextWriter
{
    public class HotkeyManager
    {
        [DllImport("user32.dll")]
        private static extern bool RegisterHotKey(IntPtr hWnd, int id, uint fsModifiers, uint vk);

        [DllImport("user32.dll")]
        private static extern bool UnregisterHotKey(IntPtr hWnd, int id);

        public static void RegisterHotkeys(IntPtr windowHandle)
        {
            RegisterHotKey(windowHandle, HotkeyConstants.HOTKEY_ID_1, HotkeyConstants.MOD_NONE, HotkeyConstants.VK_DEL);
            RegisterHotKey(windowHandle, HotkeyConstants.HOTKEY_ID_1, HotkeyConstants.MOD_NONE, HotkeyConstants.VK_F2);
            RegisterHotKey(windowHandle, HotkeyConstants.HOTKEY_ID_1, HotkeyConstants.MOD_NONE, HotkeyConstants.VK_F3);
            RegisterHotKey(windowHandle, HotkeyConstants.HOTKEY_ID_1, HotkeyConstants.MOD_NONE, HotkeyConstants.VK_F4);
            RegisterHotKey(windowHandle, HotkeyConstants.HOTKEY_ID_2, HotkeyConstants.MOD_SHIFT, HotkeyConstants.VK_F2);
            RegisterHotKey(windowHandle, HotkeyConstants.HOTKEY_ID_2, HotkeyConstants.MOD_SHIFT, HotkeyConstants.VK_F3);
        }

        public static void UnregisterHotkeys(IntPtr windowHandle)
        {
            UnregisterHotKey(windowHandle, HotkeyConstants.HOTKEY_ID_1);
            UnregisterHotKey(windowHandle, HotkeyConstants.HOTKEY_ID_2);
        }
    }
}