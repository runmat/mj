using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Windows.Input;
using CarDocu.Models;
using GeneralTools.Services;

namespace CarDocu.Services.Threads
{
    /// <summary>
    /// Die Klasse stellt eine abgeleitete Hintergrundaufgabe zur Archivierung dar.
    /// </summary>
    public class ArchiveBackgroundTask : CardocuBackgroundTask
    {
        private ArchiveLogItems _logItems = new ArchiveLogItems(); 
        public ArchiveLogItems LogItems 
        { 
            get { return _logItems; }
            set { _logItems = value; }
        }

        public override string Name { get { return "Netzwerk-Versand"; } }

        // keep this interval nearly endlees, to avoid sending to many test-mails from our smtp server.
        // (currently only at application startup one test-mail will be sent to check availability of our smtp server)
        protected override int OnlineStatusIntervalSeconds { get { return 999999; } }


        protected override bool NativeThreadLoop()
        {
            base.NativeThreadLoop();

            if (!NativeThreadLoopIsEnabled)
                return true;

            var isOnline = GetOnlineStatus();
            TaskService.StartUiTask(() => IsOnline = isOnline);

            CurrentQueuedItem = Dequeue();
            if (CurrentQueuedItem == null)
                return true;

            TaskService.StartUiTask(() => IsBusy = true);

            // here is our long operation:
            var success = new ArchiveNetworkService().ProcessArchivMeldung(ref CurrentQueuedItem);

            DelayAsShortAsPossibleButLongerIfAdminKeyPressed();

            if (CurrentQueuedItem != null)
                if (!success)
                    Enqueue(CurrentQueuedItem);
                else
                {
                    // success, so let's log:
                    LogItems.Add((ArchiveLogItem)CurrentQueuedItem);
                    LogItemsSave();
                }

            CurrentQueuedItem = null;

            TaskService.StartUiTask(() => IsBusy = false);

            return true;
        }

        public override bool GetOnlineStatus()
        {
            return new ArchiveNetworkService().IsOnline();
        }

        public ArchiveLogItem Cast(CardocuQueueEntity item)
        {
            return (ArchiveLogItem)item;
        }

        public override void LogItemsLoad(string path)
        {
            lock (SyncLock)
            {
                var logItems = XmlService.XmlTryDeserializeFromPath<ArchiveLogItems>(path);
                if (logItems == null)
                    return;

                LogItems = logItems;

                base.LogItemsLoad(path);

                TaskService.StartDelayedUiTask(3000, () => NativeThreadLoopIsEnabled = true);
            }
        }

        public override void LogItemsSave()
        {
            XmlService.XmlTrySerializeToPath(LogItems, LogPath);
        }

        public override void LogItemFileDelete(string path)
        {
            XmlService.XmlTryDeleteFile<ArchiveLogItems>(path);
        }

        public override void LogItemsRemoveCurrentQueuedItem()
        {
            LogItems.Remove((ArchiveLogItem)CurrentQueuedItem);
        }

        public override void LogItemsRemoveItem(CardocuQueueEntity item)
        {
            LogItems.Remove((ArchiveLogItem)item);
        }

        protected override void SetDeliveryDate(ScanDocument scanDocument, DateTime deliveryDate)
        {
            ArchiveNetworkService.SetDeliveryDate(scanDocument, deliveryDate);
        }

        public virtual List<ArchiveLogItem> DeliveryReSyncToMasterItems(List<ArchiveLogItem> masterItems)
        {
            LogItems.ForEach(myLogItem =>
            {
                var masterItem = masterItems.FirstOrDefault(mi => mi.DocumentID == myLogItem.DocumentID);
                if (masterItem != null)
                    masterItem.DeliveryDate = myLogItem.DeliveryDate;
            });

            return masterItems.Where(mi => mi.DeliveryDate != null).ToList();
        }
    }
}
