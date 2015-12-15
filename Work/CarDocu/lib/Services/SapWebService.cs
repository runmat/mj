using System;
using System.Linq;
using CarDocu.Models;
using GeneralTools.Models;

namespace CarDocu.Services
{
    public class SapWebService
    {
        private de.kroschke.sgw.Service _service;
        private de.kroschke.sgw.Service Service
        {
            get
            {
                return (_service ?? (_service = new de.kroschke.sgw.Service { Url = DomainService.Repository.GlobalSettings.SapSettings.WebServiceUrl }));
            }
        }

        public bool IsOnline()
        {
            bool isOnline;

            try
            {
                if (DomainService.Repository.AppSettings.OnlineStatusAutoCheckDisabled)
                    isOnline = true;
                else
                    isOnline = DomainService.CheckOnlineState();
            }
            catch { isOnline = false; }

            return isOnline;
        }

        public bool ProcessWebServiceSapMeldung(ref CardocuQueueEntity baseLogItem)
        {
            var logItem = (SapLogItem) baseLogItem;

            var logItemID = logItem.DocumentID;
            var scanDocument = DomainService.Repository.ScanDocumentRepository.ScanDocuments.FirstOrDefault(sd => sd.DocumentID == logItemID);
            if (scanDocument == null)
                return false;

            scanDocument.EnsureDocumentType();
            if (scanDocument.ScanImages.Count == 0)
                scanDocument.XmlLoadScanImages();

            string commaSeparatedReturnCodes;
            string commaSeparatedReturnMessages;

            try
            {
                commaSeparatedReturnCodes = "";
                commaSeparatedReturnMessages = "";

                var webServiceFuntionID = scanDocument.SelectedDocumentType == null ? "" : scanDocument.SelectedDocumentType.WebServiceFunction;

                if (webServiceFuntionID == "CARDOCU")
                    if (!Service.ProcessArchivMeldung(scanDocument.KundenNr,
                                                      scanDocument.DocumentID, scanDocument.FinNumber,
                                                      scanDocument.StandortCode,
                                                      string.Join(",", scanDocument.ScanDocumentTypeCodesSAP),
                                                      out commaSeparatedReturnCodes, out commaSeparatedReturnMessages))
                        return false;

                if (webServiceFuntionID == "VWL")
                    if (!Service.ProcessVwlKlaerfallMeldung(scanDocument.KundenNr,
                                                      scanDocument.DocumentID, scanDocument.FinNumber,
                                                      scanDocument.StandortCode,
                                                      string.Join(",", scanDocument.ScanDocumentTypeCodesSAP),
                                                      out commaSeparatedReturnCodes, out commaSeparatedReturnMessages))
                        return false;

                if (webServiceFuntionID == "WKDA")
                    if (!Service.ProcessWkdaWiesbaden(scanDocument.KundenNr,
                                                      scanDocument.DocumentID, scanDocument.FinNumber,
                                                      scanDocument.StandortCode,
                                                      string.Join(",", scanDocument.ScanDocumentTypeCodesSAP),
                                                      out commaSeparatedReturnCodes, out commaSeparatedReturnMessages))
                        return false;

            }
            catch(Exception) { return false; }

            //
            // delivery successfull => let's mark all corresponding entries with an apropiate delivery date right here:
            //
            var deliveryDate = DateTime.Now;
            logItem.DeliveryDate = deliveryDate;
            SetDeliveryDate(scanDocument, deliveryDate);

            logItem.FinNummer = scanDocument.FinNumber;
            logItem.KundenNr = scanDocument.KundenNr;
            logItem.StandortCode = scanDocument.StandortCode;

            logItem.DocumentCodes = scanDocument.ScanDocumentTypeCodes;
            logItem.ResultCodes = commaSeparatedReturnCodes.Split(',').Select(c => c.ToInt(0)).ToList();
            logItem.ResultMessages = commaSeparatedReturnMessages.Split(',').ToList(); 
            
            return true;
        }

        public static void SetDeliveryDate(ScanDocument scanDocument, DateTime deliveryDate)
        {
            scanDocument.SapDeliveryDate = deliveryDate;
        }
    }
}
