using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Documents;
using System.Windows.Xps.Packaging;
using GeneralTools.Services;

namespace CarDocu.Services
{
    public class UIHintService
    {
        private static string _uiHintKeyCurrent;
        private static string _uiHintDocumentResourceKeyCurrent;
        private static string _uiHintDocumentTempFileName;
        private static XpsDocument _uiHintDocumentXpsDocument;

        public static void TryShowNextUIHintForUser(Action<FixedDocumentSequence> onUIHintConfirmed)
        {
            if (onUIHintConfirmed == null)
                return;

            var hintDocumentNames = GetResourceUiHintDocumentNames();
            foreach (var hintDocumentName in hintDocumentNames)
            {
                var rawName = GetResourceUiHintDocumentRawName(hintDocumentName);
                if (!DomainService.Repository.UserSettings.UIHelpHintIsConfirmed(rawName))
                {
                    // user didn't confirm reading this document:
                    _uiHintKeyCurrent = rawName;
                    _uiHintDocumentResourceKeyCurrent = hintDocumentName;
                    TaskService.StartDelayedUiTask(1000, () => onUIHintConfirmed(GetUIHintXpsDocument()));
                    return;
                }
            }
        }

        public static void PersistConfirmationUIHintForUser()
        {
            if (string.IsNullOrEmpty(_uiHintKeyCurrent) || _uiHintDocumentXpsDocument == null)
                return;

            DomainService.Repository.UserSettings.UIHelpHintConfirm(_uiHintKeyCurrent, _uiHintDocumentXpsDocument.CoreDocumentProperties.Title);
            
            _uiHintKeyCurrent = null;
            _uiHintDocumentResourceKeyCurrent = null;

            if (_uiHintDocumentXpsDocument != null)
                _uiHintDocumentXpsDocument.Close();
            _uiHintDocumentXpsDocument = null;

            FileService.TryFileDelete(_uiHintDocumentTempFileName);
            _uiHintDocumentTempFileName = null;
        }

        private static FixedDocumentSequence GetUIHintXpsDocument()
        {
            if (string.IsNullOrEmpty(_uiHintKeyCurrent) || string.IsNullOrEmpty(_uiHintDocumentResourceKeyCurrent))
                return null;

            var path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            if (string.IsNullOrEmpty(path))
                return null;

            FixedDocumentSequence xpsDocumentSequence = null;
            FileService.ProvideTempFileForResource(_uiHintDocumentResourceKeyCurrent, 
                tmpFileName =>
                    {
                        _uiHintDocumentTempFileName = tmpFileName;
                        _uiHintDocumentXpsDocument = new XpsDocument(tmpFileName, FileAccess.Read);
                        xpsDocumentSequence = _uiHintDocumentXpsDocument.GetFixedDocumentSequence();
                    });
            return xpsDocumentSequence;
        }

        static IEnumerable<string> GetResourceUiHintDocumentNames()
        {
            var list = new List<string>();

            var assembly = Assembly.GetExecutingAssembly();
            foreach (var resourceName in assembly.GetManifestResourceNames().ToList().OrderBy(name => name))
            {
                var resourceNameLower = resourceName.ToLower();
                if (resourceNameLower.Contains("uihints.hintdocuments.") && resourceNameLower.EndsWith(".xps"))
                    // i.e. "CarDocu.Services.UIHints.HintDocuments.DocuArtenBigSelectionHint.xps"
                    list.Add(resourceName);
            }

            return list;
        }

        static string GetResourceUiHintDocumentRawName(string resourceUiHintDocumentName)
        {
            var rawName = resourceUiHintDocumentName.ToLower().Replace(".xps", "");
            return rawName.Substring(rawName.LastIndexOf(".", StringComparison.Ordinal) + 1);
        }
    }
}
