using System;
using System.ComponentModel;

namespace WpfTools4.Services
{
    public class ProgressBarOperation
    {
        private readonly Func<ProgressBarOperation, bool> _backgroundTask;
        private readonly Action<ProgressBarOperation> _complete;

        public event PropertyChangedEventHandler PropertyChanged;

        public bool IsCancellationPending { get; private set; }

        public bool TaskResult { get; private set; }

        public bool OperationCompleted { get; private set; }

        private static ProgressBarOperation _progressBarOperation;

        private static ProgressWindow _progressWindow;

        private BackgroundWorker _worker;

        private int _total;
        public int Total
        {
            get { return this._total; }
            set
            {
                this._total = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Total"));
            }
        }

        private int _current;
        public int Current
        {
            get { return this._current; }
            set
            {
                this._current = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Current"));
            }
        }

        private string _header;
        public string Header
        {
            get { return _header; }
            set
            {
                _header = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Header"));
            }
        }

        private string _details;
        public string Details
        {
            get { return _details; }
            set
            {
                _details = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Details"));
            }
        }

        private bool _progressInfoVisible;
        public bool ProgressInfoVisible
        {
            get { return _progressInfoVisible; }
            set
            {
                _progressInfoVisible = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ProgressInfoVisible"));
            }
        }

        private bool _busyCircleVisible; 
        public bool BusyCircleVisible 
        { 
            get { return _busyCircleVisible; }
            set
            {
                _busyCircleVisible = value; 
                OnPropertyChanged(new PropertyChangedEventArgs("BusyCircleVisible"));
            }
        }


        public ProgressBarOperation(Func<ProgressBarOperation, bool> backgroundTask, Action<ProgressBarOperation> complete)
        {
            Total = 0;
            Current = 0;
            IsCancellationPending = false;

            _backgroundTask = backgroundTask;
            _complete = complete;
        }

        public static void Start(Func<ProgressBarOperation, bool> backgroundTask, Action<ProgressBarOperation> complete)
        {
            _progressBarOperation = new ProgressBarOperation(backgroundTask, complete);
            _progressWindow = new ProgressWindow(_progressBarOperation);

            //runs the progress operation upon window loaded event
            _progressWindow.ShowDialog();
        }

        protected void BackgroundTask(object sender, DoWorkEventArgs e)
        {
            if (_backgroundTask != null)
                TaskResult = _backgroundTask(this);
        }

        void TaskRunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (OperationCompleted)
                return;

            _progressWindow.Close();
            _complete(_progressBarOperation);

            OperationCompleted = true;
        }

        protected virtual void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (this.PropertyChanged != null)
                this.PropertyChanged(this, e);
        }

        /// <summary>
        /// Starts the background operation 
        /// </summary>
        public void TaskStart()
        {
            _worker = new BackgroundWorker { WorkerSupportsCancellation = true };
            _worker.DoWork += BackgroundTask;
            _worker.RunWorkerCompleted += TaskRunWorkerCompleted;
            _worker.RunWorkerAsync();
        }

        /// <summary>
        /// Requests cancellation 
        /// </summary>
        public void CancelAsync()
        {
            IsCancellationPending = true;
            _worker.CancelAsync();
            _worker.RunWorkerCompleted -= TaskRunWorkerCompleted;
            
            _progressBarOperation.TaskResult = false;
            TaskRunWorkerCompleted(null, null);
        }
    }
}
