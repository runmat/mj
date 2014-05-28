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

            try { Directory.Delete(itemToDelete.GetDocumentPrivateDirectoryName(), true); }
            catch
            {
                //Tools.AlertError("Das Scan-Document kann nicht gelöscht werden!\r\n\r\nIst das Verzeichnis '" + itemToDelete.GetDocumentDirectoryName() + "' ist in u. U. Bearbeitung ?!?");
                //return false;
            }

            ScanDocuments.Remove(itemToDelete);
            if (OnDeleteScanDocument != null)
                OnDeleteScanDocument(itemToDelete);

            return true;
        }

        public void Save(string directoryName, string fileName=null)
        {
            if (fileName == null)
                fileName = this.GetType().Name;

            XmlService.XmlSerializeToPath(this, directoryName, fileName);
            ScanDocuments.ForEach(sd => sd.XmlSaveScanImages());
        }
    }
}
