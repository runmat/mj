using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security;
using System.Threading;
using System.Windows;
using System.Windows.Input;
using Microsoft.Win32;
using WpfTools4.Services;

namespace MyBoss
{
    public partial class MainWindow
    {
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        static extern IntPtr SendMessage(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam);
        const int WmClose = 0x0010;

        private LowLevelKeyboardListener _listener;
        private System.Windows.Forms.NotifyIcon _notifyIcon;
        private System.Windows.Forms.Timer _t;
        private double _lastTicks1, _lastTicks2, _lastTicks3, _lastTicks4;

        public MainWindow()
        {
            InitializeComponent();

            HookKeyboard();
            HookKeyboardTimered();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            _notifyIcon = new System.Windows.Forms.NotifyIcon();
            _notifyIcon.Click += notifyIcon_Click;
            var icon = Application.GetResourceStream(new Uri("pack://application:,,,/MyBoss;component/boss.ico"));
            if (icon != null)
                _notifyIcon.Icon = new System.Drawing.Icon(icon.Stream);

            _notifyIcon.Visible = true;
            Hide();
        }


        private void HookKeyboard()
        {
            _listener = new LowLevelKeyboardListener();
            _listener.OnKeyPressed += _listener_OnKeyPressed;
            _listener.HookKeyboard();
        }

        private void HookKeyboardTimerTick(object sender, EventArgs e)
        {
            HookKeyboard();
        }

        private void HookKeyboardTimered()
        {
            var timer = new System.Windows.Forms.Timer
            {
                Enabled = true,
                Interval = 60000
            };
            timer.Tick += HookKeyboardTimerTick;
            timer.Start();
        }

        const string OutlookProcessFullName = "outlook";

        static void KillOutlook()
        {
            var outlookProcess = Process.GetProcesses().FirstOrDefault(p => OutlookProcessFullName.Contains(p.ProcessName.ToLower()));
            if (outlookProcess != null)
                SendMessage(outlookProcess.MainWindowHandle, WmClose, (IntPtr) 0, (IntPtr) 0);
        }

        public static void StartOutlook()
        {
            var pi = new ProcessStartInfo
            {
                FileName = OutlookProcessFullName + ".exe",
                WindowStyle = ProcessWindowStyle.Maximized,
                UseShellExecute = true,
            };
            Process.Start(pi);
        }

        void SetIeProxyRegKey(int value)
        {
            var regKey = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Internet Settings", true);
            if (regKey == null)
                return;

            regKey.SetValue("ProxyEnable", value);
            regKey.Close();
        }

        void OpenIeUrlWithProxyEnabled(string ieUrl)
        {
            notifyIcon_Click(null, null);

            SetIeProxyRegKey(1);

            Process.Start(@"C:\Program Files\Internet Explorer\iexplore.exe", ieUrl);
            Thread.Sleep(2000);

            SetIeProxyRegKey(0);
        }

        bool _listener_OnKeyPressed(KeyPressedArgs e)
        {
            if (LowLevelKeyboardListener.Disabled)
                return false;

            if (TryCheckCtrlAltKeyPressAction(e, Key.U, () =>
            {
                OpenIeUrlWithProxyEnabled("http://vms037.kroschke.de/SelfServices/default.aspx");
            }))
                return true;
            if (TryCheckCtrlAltKeyPressAction(e, Key.I, () =>
            {
                if (Clipboard.ContainsText())
                {
                    var clipboardUrl = Clipboard.GetText();
                    if (clipboardUrl.ToLower().StartsWith("https://"))
                        OpenIeUrlWithProxyEnabled(clipboardUrl);
                }
            }))
                return true;

            if (TryCheckCtrlAltKeyPressAction(e, Key.T, () =>
            {
                notifyIcon_Click(null, null);
                Process.Start(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "todo.txt"));
            }))
                return true;

            if (TryCheckCtrlAltKeyPressAction(e, Key.V, () =>
                    {
                        var pi = new ProcessStartInfo
                        {
                            FileName = @"C:\Users\JenzenM\Documents\mj\dev\VsSolutionPersister\VsSolutionPersister\bin\Debug\VsSolutionPersister.exe",
                            WorkingDirectory = @"C:\Users\JenzenM\Documents\mj\dev\VsSolutionPersister\VsSolutionPersister\bin\Debug"
                        };
                        //notifyIcon_Click(null, null);
                        Process.Start(pi);
                    }))
                return false;

            if (TryCheckCtrlAltKeyPressAction(e, Key.C, () =>
            {
                Clipboard.SetText("seE17igEl");
            }))
                return false;
            if (TryCheckCtrlAltKeyPressAction(e, Key.X, () =>
            {
                Clipboard.SetText(_pwd);
            }))
                return false;



            TryCheckDoubleTimeKeyPressAction(e, ref _lastTicks1, Key.LeftAlt, () =>
            {
                LowLevelKeyboardListener.Disabled = true;
                Tools.ShowDesktop();
                Thread.Sleep(50);
                new FakeWindow("fake_wallpaper.png").Show();
            });

            TryCheckDoubleTimeKeyPressAction(e, ref _lastTicks2, Key.LeftShift, () =>
            {
                LowLevelKeyboardListener.Disabled = true;
                KillOutlook();
                Tools.ShowDesktop();
                Thread.Sleep(50);
                new FakeWindow("fake_lockscreen_win10.png").Show();
            });

            TryCheckDoubleTimeKeyPressAction(e, ref _lastTicks3, Key.RightCtrl, () =>
            {
                notifyIcon_Click(null, null);
            });

            TryCheckDoubleTimeKeyPressAction(e, ref _lastTicks4, Key.RightShift, Close);

            return false;
        }

        static bool TryCheckCtrlAltKeyPressAction(KeyPressedArgs e, Key key, Action action)
        {
            if (!(Keyboard.IsKeyDown(Key.LeftCtrl) && Keyboard.IsKeyDown(Key.LeftAlt) && e.KeyPressed == key))
                return false;

            action();

            Thread.Sleep(1000);
            return true;
        }

        static void TryCheckDoubleTimeKeyPressAction(KeyPressedArgs e, ref double lastTicks, Key key, Action action)
        {
            if (e.KeyPressed != key)
            {
                lastTicks = 0;
                return;
            }

            var ticksNow = (DateTime.Now - DateTime.MinValue).TotalMilliseconds;
            if (lastTicks > 0 && (ticksNow - lastTicks > 100) && (ticksNow - lastTicks < 200))
                action();

            lastTicks = ticksNow;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            _listener.UnHookKeyboard();

            _notifyIcon.Visible = false;
            _notifyIcon = null;
        }

        void notifyIcon_Click(object sender, EventArgs e)
        {
            Show();
            WindowState = WindowState.Normal;

            if (sender != null)
                HookKeyboard();

            _t = new System.Windows.Forms.Timer
            {
                Enabled = true,
                Interval = 4000
            };
            _t.Tick += T_Tick;
        }

        private void T_Tick(object sender, EventArgs e)
        {
            Hide();

            _t.Stop();
            _t.Dispose();
        }

        private void RehookKeyboard(object sender, RoutedEventArgs e)
        {
            HookKeyboard();
        }
        private void Quit(object sender, RoutedEventArgs e)
        {
            Process.GetCurrentProcess().Kill();
        }

        private static string _pwd = "Walter@!3697";
        static SecureString ConvertToSecureString(string strPassword)
        {
            var secureStr = new SecureString();
            if (strPassword.Length > 0)
            {
                foreach (var c in strPassword.ToCharArray()) secureStr.AppendChar(c);
            }
            return secureStr;
        }
    }
}
