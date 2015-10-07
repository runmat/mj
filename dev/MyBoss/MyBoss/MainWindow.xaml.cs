using System;
using System.Drawing;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using WpfTools4.Services;
using Point = System.Drawing.Point;

namespace MyBoss
{
    public partial class MainWindow
    {
        private LowLevelKeyboardListener _listener;
        private System.Windows.Forms.NotifyIcon _notifyIcon;
        private System.Windows.Forms.Timer _t;
        private double _lastTicks1, _lastTicks2, _lastTicks3;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            _listener = new LowLevelKeyboardListener();
            _listener.OnKeyPressed += _listener_OnKeyPressed;

            _listener.HookKeyboard();

            _notifyIcon = new System.Windows.Forms.NotifyIcon();
            _notifyIcon.Click += notifyIcon_Click;
            var icon = Application.GetResourceStream(new Uri("pack://application:,,,/MyBoss;component/boss.ico"));
            if (icon != null)
                _notifyIcon.Icon = new System.Drawing.Icon(icon.Stream);

            _notifyIcon.Visible = true;
            Hide();
        }

        void _listener_OnKeyPressed(object sender, KeyPressedArgs e)
        {
            TryCheckAction(e, ref _lastTicks1, Key.LeftCtrl, () =>
            {
                Tools.ShowDesktop();
                Thread.Sleep(50);
                new FakeWindow().Show();
            });

            TryCheckAction(e, ref _lastTicks2, Key.RightCtrl, () =>
            {
                notifyIcon_Click(null, null);
            });

            TryCheckAction(e, ref _lastTicks3, Key.RightShift, Close);
        }

        static void TryCheckAction(KeyPressedArgs e, ref double lastTicks, Key key, Action action)
        {
            if (e.KeyPressed != key)
            {
                lastTicks = 0;
                return;
            }

            var ticksNow = (DateTime.Now - DateTime.MinValue).TotalMilliseconds;
            if (lastTicks > 0 && (ticksNow - lastTicks > 100) && (ticksNow - lastTicks < 300))
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

            _t = new System.Windows.Forms.Timer
            {
                Enabled = true,
                Interval = 3000
            };
            _t.Tick += T_Tick;
        }

        private void T_Tick(object sender, EventArgs e)
        {
            Hide();

            _t.Stop();
            _t.Dispose();
        }
    }
}
