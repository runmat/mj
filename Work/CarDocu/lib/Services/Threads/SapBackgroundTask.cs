using System;
using System.Collections.Generic;
using System.Linq;
using CarDocu.Models;
using GeneralTools.Services;

namespace CarDocu.Services.Threads
{
    /// <summary>
    /// Die Klasse stellt eine abgeleitete Hintergrundaufgabe zur Datenübertragung an SAP dar.
    /// </summary>
    public class SapBackgroundTask : CardocuBackgroundTask
    {
        public SapLogItems LogItems { get; set; } = new SapLogItems();

        public override string Name => "SAP Meldung";

        protected override int OnlineStatusIntervalSeconds => 3;

        protected override bool NativeThreadLoop()
        {
            base.NativeThreadLoop();

            if (!NativeThreadLoopIsEnabled)
                return true;

            // keep outa here if we are not online to avoid "blinking" of hourglass in case we are offline: 
            if (!IsOnline)
                return true;

            CurrentQueuedItem = Dequeue();
            if (CurrentQueuedItem == null)
                return true;

            TaskService.StartUiTask(() => IsBusy = true);

            // here is our long operation:
            var success = new SapWebService().ProcessWebServiceSapMeldung(ref CurrentQueuedItem);

            DelayAsShortAsPossibleButLongerIfAdminKeyPressed();

            if (CurrentQueuedItem != null)
                if (!success)
                    Enqueue(CurrentQueuedItem);
                else
                {
                    // success, so let's log:
                    LogItems.Add((SapLogItem)CurrentQueuedItem);
                    LogItemsSave();
                }

            CurrentQueuedItem = null;

            TaskService.StartUiTask(() => IsBusy = false);

            return true;
        }

        public override bool GetOnlineStatus()
        {
            return new SapWebService().IsOnline();
        }

        public SapLogItem Cast(CardocuQueueEntity item)
        {
            return (SapLogItem)item;
        }

        public override void LogItemsLoad(string path)
        {
            lock (SyncLock)
            {
                var logItems = XmlService.XmlTryDeserializeFromPath<SapLogItems>(path);
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
            XmlService.XmlTryDeleteFile<SapLogItems>(path);
        }

        public override void LogItemsRemoveCurrentQueuedItem()
        {
            LogItems.Remove((SapLogItem)CurrentQueuedItem);
        }

        public override void LogItemsRemoveItem(CardocuQueueEntity item)
        {
            LogItems.Remove((SapLogItem)item);
        }
        
        protected override void SetDeliveryDate(ScanDocument scanDocument, DateTime deliveryDate)
        {
            SapWebService.SetDeliveryDate(scanDocument, deliveryDate);
        }

        public virtual List<SapLogItem> DeliveryReSyncToMasterItems(List<SapLogItem> masterItems)
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
