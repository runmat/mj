using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Calendar = System.Windows.Controls.Calendar;

namespace SimDate
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        [DllImport("Kernel32.dll")]
        public static extern bool SetLocalTime(ref SystemTime time);
        [DllImport("Kernel32.dll")]
        public static extern void GetLocalTime(ref SystemTime time); 
        
        private DateTime _startTime;
        private readonly int _ticksSinceSystemStart;

        private float _fromTime;

        public MainWindow()
        {
            InitializeComponent();
            this.FontSize = 18; 

            _startTime = DateTime.Now;
            _ticksSinceSystemStart = Environment.TickCount;

            Calendar.SelectedDate = DateTime.Today;
        }

        private void RestoreDateClick(object sender, RoutedEventArgs e)
        {
            RestoreSystemTime();
        }

        private void SetDateClick(object sender, RoutedEventArgs e)
        {
            var dateTime = Calendar.SelectedDate.GetValueOrDefault();

            var rangeSedonds = 7200;
            var r = new Random();
            var seconds = r.Next(0, rangeSedonds);
            dateTime = dateTime.AddHours(_fromTime).AddMinutes(0).AddSeconds(seconds);

            SetSystemTime(dateTime);
        }

        private void RestoreSystemTime()
        {
            var ticksNow = Environment.TickCount;
            var dateTime = _startTime.AddMilliseconds(ticksNow - _ticksSinceSystemStart);

            SetSystemTime(dateTime);
        }

        private void SetSystemTime(DateTime dateTime)
        {
            DatumText.Text = string.Format("{0:ddd, dd.MM.yyyy HH:mm:ss}", dateTime);

            var st = new SystemTime();
            st.FromDateTime(dateTime);
            SetLocalTime(ref st);
        }


        protected override void OnContentRendered(EventArgs e)
        {
            Left = 200;

            base.OnContentRendered(e);
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            RestoreSystemTime();

            base.OnClosing(e);
        }

        protected override void OnPreviewMouseUp(MouseButtonEventArgs e)
        {
            base.OnPreviewMouseUp(e);
            if (Mouse.Captured is Calendar || Mouse.Captured is System.Windows.Controls.Primitives.CalendarItem)
            {
                Mouse.Capture(null);
            }
        }

        private void RbFromTimeOnChecked(object sender, RoutedEventArgs e)
        {
            var radioButton = sender as RadioButton;
            if (radioButton == null)
                return;

            var rbFromTime = radioButton.Name.Substring(2);
            _fromTime = Convert.ToInt32(rbFromTime);
        }
    }
}
