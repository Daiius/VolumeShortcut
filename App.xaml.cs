using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

using System.Runtime.InteropServices;
using System.Windows.Interop;

namespace VolumeShortcut
{
    /// <summary>
    /// App.xaml の相互作用ロジック
    /// </summary>
    public partial class App : Application
    {
        [DllImport("user32.dll")]
        public static extern IntPtr SendMessageW(IntPtr hWnd, int Msg,
            IntPtr wParam, IntPtr lParam);

        private const int APPCOMMAND_VOLUME_MUTE = 0x80000;
        private const int APPCOMMAND_VOLUME_UP = 0xA0000;
        private const int APPCOMMAND_VOLUME_DOWN = 0x90000;
        private const int WM_APPCOMMAND = 0x319;

        KeyboardHooker hooker = new KeyboardHooker();
        System.Windows.Forms.KeysConverter converter = new System.Windows.Forms.KeysConverter();

        bool isScrollLockPressed = false;

        MainWindow window = new MainWindow();
        IntPtr handle;

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var icon = GetResourceStream(new Uri("outline_keyboard_alt_white_48dp.ico", UriKind.Relative)).Stream;
            var menu = new System.Windows.Forms.ContextMenuStrip();
            menu.Items.Add("修了", null, Exit_Click);

            var notifyIcon = new System.Windows.Forms.NotifyIcon
            {
                Visible = true,
                Icon = new System.Drawing.Icon(icon),
                Text = "Volume Shortcut"
            };

            handle = new WindowInteropHelper(window).Handle;

            notifyIcon.MouseClick += NotifyIcon_MouseClick;

            hooker.KeyDownEvent += Hooker_KeyDownEvent;
            hooker.KeyUpEvent += Hooker_KeyUpEvent;
            hooker.Hook();
        }

        private void Hooker_KeyUpEvent(object sender, KeyEventArgs e)
        {
            //var str = converter.ConvertToString(e.KeyCode);
            if (e.KeyCode == 145 || e.KeyCode == 45) isScrollLockPressed = false;
        }

        private void Hooker_KeyDownEvent(object sender, KeyEventArgs e)
        {
            //var str = converter.ConvertToString(e.KeyCode) + " Down";
            if (e.KeyCode == 145 || e.KeyCode == 45) isScrollLockPressed = true;

            if (isScrollLockPressed && e.KeyCode == 101)
            {
                SendMessageW(handle, WM_APPCOMMAND, handle, (IntPtr)APPCOMMAND_VOLUME_UP);
            }
            else if (isScrollLockPressed && e.KeyCode == 40)
            {
                SendMessageW(handle, WM_APPCOMMAND, handle, (IntPtr)APPCOMMAND_VOLUME_DOWN);
            }
        }

        private void NotifyIcon_MouseClick(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                
                window.Show();
            }
        }

        private void Exit_Click(object sender, EventArgs e)
        {
            hooker.UnHook();
            Shutdown();
        }
    }
}
