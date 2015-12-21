using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using CarDocu.Models;
using GeneralTools.Services;

namespace CarDocu.Services
{
    public class ScanDocumentRepository 
    {
        #region Properties

        private List<ScanDocument> _scanDocuments = new List<ScanDocument>();
        public List<ScanDocument> ScanDocuments
        {
            get { return _scanDocuments; }
            set { _scanDocuments = value; }
        }

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
            if (OnAddScanDocument != null)
                OnAddScanDocument(scanDocument);

            return true;
        }

        public bool TryDeleteScanDocument(ScanDocument scanDocument) 
        {
            var itemToDelete = scanDocument;

            scanDocument.EnsureDocumentType();
            var pdfFileNames = scanDocument.GetPdfFileNames();

            try { Directory.Delete(itemToDelete.GetDocumentPrivateDirectoryName(), true); }
            catch { /**/ }

            if (ScanDocuments.Remove(itemToDelete))
                new ArchiveNetworkService().DeletePdfFilesFor(itemToDelete, pdfFileNames);

            if (OnDeleteScanDocument != null)
                OnDeleteScanDocument(itemToDelete);

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
