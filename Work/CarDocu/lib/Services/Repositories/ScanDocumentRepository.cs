using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using CarDocu.Models;
using GeneralTools.Services;

namespace CarDocu.Services
{
    public class ScanDocumentRepository 
    {
        #region Properties

        public List<ScanDocument> ScanDocuments { get; set; } = new List<ScanDocument>();

        [XmlIgnore]
        public Action<ScanDocument> OnAddScanDocument;

        [XmlIgnore]
        public Action<ScanDocument> OnDeleteScanDocument;

        #endregion


        public bool TryAddScanDocument(ScanDocument scanDocument)
        {
            if (ScanDocuments.Any(sd => sd.DocumentID == scanDocument.DocumentID))
                return false;

            ScanDocuments.Add(scanDocument);
            OnAddScanDocument?.Invoke(scanDocument);

            return true;
        }

        public bool TryDeleteScanDocument(ScanDocument scanDocument, bool deleteAlsoNetworkDeliveryPdfFiles)
        {
            var itemToDelete = scanDocument;

            scanDocument.EnsureDocumentType();
            var pdfFileNames = scanDocument.GetPdfFileNames();

            try
            {
                FileService.TryDirectoryDelete(itemToDelete.GetDocumentPrivateDirectoryName()); 

                if (ScanDocuments.Remove(itemToDelete))
                    new ArchiveNetworkService().DeletePdfFilesFor(itemToDelete, pdfFileNames, deleteAlsoNetworkDeliveryPdfFiles);

                OnDeleteScanDocument?.Invoke(itemToDelete);
            }
            catch { /**/ }

            return true;
        }

        public void Save(string directoryName, string fileName=null)
        {
            if (fileName == null)
                fileName = GetType().Name;

            XmlService.XmlSerializeToPath(this, directoryName, fileName);
            ScanDocuments.ForEach(sd => sd.XmlSaveScanImages());
        }
    }
}
