using System;
using System.Xml.Serialization;
using GeneralTools.Models;

namespace CarDocu.Models
{
    public class DomainUserSettings : ModelBase
    {
        #region Properties

        #region User definded UI Settings

        private bool _uIDocuArtBigSelection; 
        public bool UIDocuArtBigSelection 
        { 
            get { return _uIDocuArtBigSelection; }
            set { _uIDocuArtBigSelection = value; SendPropertyChanged("UIDocuArtBigSelection"); }
        }

        private string _selectedDocumentTypeCode; 
        public string SelectedDocumentTypeCode 
        { 
            get { return _selectedDocumentTypeCode; }
            set { _selectedDocumentTypeCode = value; SendPropertyChanged("SelectedDocumentTypeCode"); }
        }

        #endregion

        #region User definded UI Hints

        private XmlDictionary<string, string> _uIHelpHints = new XmlDictionary<string, string>();
        public XmlDictionary<string, string> UIHelpHints 
        { 
            get { return _uIHelpHints; }
            set { _uIHelpHints = value; SendPropertyChanged("UIHelpHints"); }
        }

        #endregion

        [XmlIgnore]
        public Action SaveSettings { get; set; }

        #endregion


        #region User definded UI Hints

        public bool UIHelpHintIsConfirmed(string helpHintKey)
        {
            return UIHelpHints.ContainsKey(helpHintKey);
        }

        public void UIHelpHintConfirm(string helpHintKey, string documentTitle)
        {
            if (UIHelpHints.ContainsKey(helpHintKey))
                UIHelpHints.Remove(helpHintKey);

            UIHelpHints.Add(helpHintKey, documentTitle);

            if (SaveSettings != null)
                SaveSettings();
        }

        public void UIHelpHintRemove(string helpHintKey)
        {
            if (UIHelpHints.ContainsKey(helpHintKey))
                UIHelpHints.Remove(helpHintKey);

            if (SaveSettings != null)
                SaveSettings();
        }

        #endregion
    }
}
