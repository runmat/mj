using System;
using System.Windows;
using System.ComponentModel;
using WpfTools4.Services;

namespace WpfTools4
{
    public partial class ProgressWindow : INotifyPropertyChanged
    {
        private readonly ProgressBarOperation _operation;

        public int Current { get { return this._operation.Current; } }

        public int Total { get { return this._operation.Total; } }

        public string Header { get { return this._operation.Header; } }

        public string Details { get { return this._operation.Details; } }

        public Visibility BusyCircleVisible { get { return this._operation.BusyCircleVisible ? Visibility.Visible : Visibility.Collapsed; } }

        public Visibility ProgressInfoVisible { get { return this._operation.ProgressInfoVisible ? Visibility.Visible : Visibility.Collapsed; } }


        public ProgressWindow(ProgressBarOperation operation)
        {
            _operation = operation;
            _operation.PropertyChanged += OperationPropertyChanged;

            InitializeComponent();

            Loaded += ProgressWindowLoaded;
        }

        void ProgressWindowLoaded(object sender, RoutedEventArgs e)
        {
            _operation.TaskStart();
        }

        void OperationPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            OnPropertyChanged(e.PropertyName);
        }

        private void CancelClick(object sender, RoutedEventArgs e)
        {
            _operation.CancelAsync();
        }


        #region INotifyPropertyChanged Members

        /// <summary>
        /// Notify property changed
        /// </summary>
        /// <param name="propertyName">Property name</param>
        protected void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion       
    }
}
