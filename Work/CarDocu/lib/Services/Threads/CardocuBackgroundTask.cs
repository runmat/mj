using System;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Xml.Serialization;
using CarDocu.Models;
using GeneralTools.Services;
using WpfTools4.Commands;

namespace CarDocu.Services
{
    /// <summary>
    /// Die Klasse stellt eine Cardocu Hintergrundaufgabe dar.
    /// </summary>
    public abstract class CardocuBackgroundTask : BackgroundTaskBase<CardocuQueueEntity>, INotifyPropertyChanged, IDisposable  
    {
        private Task _nativeTask;
        private readonly CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();

        protected static readonly object SyncLock = new object();

        protected CardocuQueueEntity CurrentQueuedItem;

        private bool _isOnline;
        public bool IsOnline
        {
            get { return _isOnline; }
            set
            {
                // ensure that these UI relevant properties are set only inside the UI thread - and inside no other (background) threads: 
                if (SynchronizationContext.Current == null) return;

                _isOnline = value; 
                SendPropertyChanged("IsOnline");
                SendPropertyChanged("IsOnlineIconSource");
                SendPropertyChanged("OnlineToolTip");
            }
        }

        private bool _isBusy;
        public bool IsBusy
        {
            get { return _isBusy; }
            set
            {
                // ensure that these UI relevant properties are set only inside the UI thread - and inside no other (background) threads: 
                if (SynchronizationContext.Current == null) return;

                _isBusy = value; 
                SendPropertyChanged("IsBusy");
                SendPropertyChanged("IsBusyIconSource");
                SendPropertyChanged("BusyToolTip");
            }
        }

        private int _queueItemCount;
        public int QueueItemCount
        {
            get { return _queueItemCount; }
            set
            {
                // ensure that these UI relevant properties are set only inside the UI thread - and inside no other (background) threads: 
                if (SynchronizationContext.Current == null) return;
                
                _queueItemCount = value; SendPropertyChanged("QueueItemCount");
            }
        }

        [XmlIgnore]
        public string IsBusyIconSource => ((Image)Application.Current.TryFindResource(GetBusyIconSourceKey())).Source.ToString();

        public string GetBusyIconSourceKey()
        {
            return $"image/16x16/{(IsBusy ? "hourglass" : "empty")}";
        }

        public string BusyToolTip => IsBusy ? "1 Hintergrundprozess noch aktiv!" : "";

        [XmlIgnore]
        public string IsOnlineIconSource => ((Image)Application.Current.TryFindResource(GetOnlineIconSourceKey())).Source.ToString();

        public string GetOnlineIconSourceKey()
        {
            return $"image/16x16/ball_{(IsOnline ? "green" : "red")}";
        }

        public string OnlineToolTip => IsOnline ? "Service ist online!" : "Service ist aktuell nicht verfügbar bzw. offline!";

        public bool NativeThreadLoopIsEnabled { get; set; }

        public abstract string Name { get; }

        protected virtual int OnlineStatusIntervalSeconds => 300;

        protected DateTime? OnlineStatusLastCheckDate { get; set; }

        protected string LogPath { get; set; }

        protected bool ActiveJobFreeze { get; set; }

        [XmlIgnore]
        public ICommand ActiveJobKillAndRemoveCommand { get; private set; }

        [XmlIgnore]
        public ICommand AllJobsKillAndRemoveCommand { get; private set; }


        protected CardocuBackgroundTask()
        {
            ActiveJobKillAndRemoveCommand = new DelegateCommand(e => ActiveJobKillAndRemove());
            AllJobsKillAndRemoveCommand = new DelegateCommand(e => AllJobsKillAndRemove());

            CreateNativeTask();
        }

        void CreateNativeTask()
        {
            _nativeTask = TaskService.StartLongRunningTask(NativeTaskActionLoop, _cancellationTokenSource.Token);
        }

        void CancelNativeTask()
        {
            _cancellationTokenSource.Cancel();
        }

        void RecreateNativeTask()
        {
            CreateNativeTask();
            IsBusy = false;
        }

        protected void NativeTaskActionLoop()
        {
            while (NativeThreadLoopUnlocked())
                Thread.Sleep(500);
        }

        bool NativeThreadLoopUnlocked()
        {
            bool retVal;
            
            lock (SyncLock)
            {
                retVal = NativeThreadLoop();
            }

            return retVal;
        }

        protected override bool NativeThreadLoop()
        {
            CheckOnlineStatus();

            return true;
        }
     
        public void Dispose()
        {
            try { _nativeTask.Dispose(); }
            catch
            {
                // ignored
            }
        }
        
        public override void Enqueue(CardocuQueueEntity item)
        {
            // Ensure unique key constraint for queue items:
            if (!Any(queueItem => queueItem.DocumentID == item.DocumentID))
                base.Enqueue(item);

            var count = QueueCount;
            TaskService.StartUiTask(() => QueueItemCount = count);
        }

        public override CardocuQueueEntity Dequeue(int sleepMilliseconds = 1000)
        {
            var returnItem = base.Dequeue(sleepMilliseconds);
            if (returnItem != null)
                Thread.Sleep(sleepMilliseconds);

            var count = QueueCount;
            TaskService.StartUiTask(() => QueueItemCount = count);
            return returnItem;
        }

        public void CheckOnlineStatus()
        {
            var lastOnlineCheckSeconds = DateTime.Now - OnlineStatusLastCheckDate.GetValueOrDefault(new DateTime(2000,1,1));
            if (lastOnlineCheckSeconds.TotalSeconds > OnlineStatusIntervalSeconds)
            {
                var isOnline = GetOnlineStatus();
                TaskService.StartUiTask(() => IsOnline = isOnline);
                OnlineStatusLastCheckDate = DateTime.Now;
            }
        }

        public abstract bool GetOnlineStatus();

        public virtual void LogItemsLoad(string path)
        {
            LogPath = path;
        }

        public void AllJobsKillAndRemove()
        {
            if (!Tools.Confirm("Wollen Sie wirklich alle(!) offenen Jobs löschen?\r\n\r\n(Hinweis: Sie können einzelne Jobs löschen indem Sie auf das rote Kreuz rechts neben dem aktiven Job klicken)"))
                return;

            AllJobsKillAndRemoveCore();
        }

        void AllJobsKillAndRemoveCore()
        {
            if (!IsBusy)
            {
                TaskService.StartDelayedUiTask(200, AllJobsKillAndRemoveCore);
                return;
            }

            Mouse.OverrideCursor = Cursors.Wait;

            ActiveJobKillAndRemove(true);
            LogItemsOuterRemoveCurrentQueuedItem();

            while (QueueCount > 0)
            {
                var item = Dequeue(0);
                if (item.DeliveryDate == null)
                {
                    RemoveQueuedItem(item, true);
                    LogItemsRemoveItem(item);
                }
            }
            
            LogItemsSave();

            Mouse.OverrideCursor = Cursors.Arrow;
        }

        public void ActiveJobKillAndRemove(bool forceRemove = false)
        {
            if (IsBusy || forceRemove)
                RemoveQueuedItem(GetCurrentQueuedItem());
        }

        void RemoveQueuedItem(CardocuQueueEntity logItem, bool preventSaveItems = false)
        {
            if (logItem == null)
                return;
            
            var logItemID = logItem.DocumentID;
            var scanDocument = DomainService.Repository.ScanDocumentRepository.ScanDocuments.FirstOrDefault(sd => sd.DocumentID == logItemID);
            if (scanDocument != null)
            {
                if (!IsOnline)
                {
                    ActiveJobFreeze = true;
                    if (!Tools.Confirm("Achtung:\r\n\r\nSobald der Service wieder online ist (Ampel = grün), werden alle Jobs aus der Warteschlange automatisch abgearbeitet!\r\nWenn Sie den Job jetzt entfernen, wird dieser auch bei grüner Ampel nicht mehr abgearbeitet.\r\nIhre Arbeit ist allerdings lokal gespeichert!\r\n\r\n==> Wollen Sie diesen Job trotzdem entfernen?"))
                    {
                        ActiveJobFreeze = false;
                        return;
                    }
                    ActiveJobFreeze = false;
                }

                SetDeliveryDate(scanDocument, DomainService.JobCancelDate);
                DomainService.Repository.ScanDocumentRepositorySave();

                CancelNativeTask();

                LogItemsOuterRemoveCurrentQueuedItem();
                if (!preventSaveItems)
                    LogItemsSave();

                RecreateNativeTask();
            }
        }

        protected CardocuQueueEntity GetCurrentQueuedItem()
        {
            return CurrentQueuedItem;
        }

        public void LogItemsOuterRemoveCurrentQueuedItem()
        {
            if (CurrentQueuedItem == null)
                return;

            LogItemsRemoveCurrentQueuedItem();
            CurrentQueuedItem = null;
        }

        public abstract void LogItemsRemoveItem(CardocuQueueEntity item);

        public abstract void LogItemsSave();

        public abstract void LogItemFileDelete(string path);

        public abstract void LogItemsRemoveCurrentQueuedItem();

        protected abstract void SetDeliveryDate(ScanDocument scanDocument, DateTime deliveryDate);

        protected void DelayAsShortAsPossibleButLongerIfAdminKeyPressed()
        {
            var delay = 200;
            TaskService.StartUiTask(() => delay = Keyboard.IsKeyDown(Key.F8) ? 2000 : 200);

            Thread.Sleep(200);
            Thread.Sleep(delay);

            while (ActiveJobFreeze) 
                Thread.Sleep(500);
        }


        #region INotifyPropertyChanged

        /// <summary>
        /// Raised when a property on this object has a new value
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Raises this ViewModels PropertyChanged event
        /// </summary>
        /// <param name="propertyName">Name of the property that has a new value</param>
        protected void SendPropertyChanged(string propertyName)
        {
            SendPropertyChanged(new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// Raises this Models PropertyChanged event
        /// </summary>
        /// <param name="e">Arguments detailing the change</param>
        protected virtual void SendPropertyChanged(PropertyChangedEventArgs e)
        {
            var handler = PropertyChanged;
            handler?.Invoke(this, e);
        }

        public void SendPropertyChanged<TProperty>(Expression<Func<TProperty>> property)
        {
            var lambda = (LambdaExpression)property;

            MemberExpression memberExpression;
            var body = lambda.Body as UnaryExpression;
            if (body != null)
            {
                var unaryExpression = body;
                memberExpression = (MemberExpression)unaryExpression.Operand;
            }
            else
            {
                memberExpression = (MemberExpression)lambda.Body;
            }
            SendPropertyChanged(memberExpression.Member.Name);
        }

        #endregion
    }
}
