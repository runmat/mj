using System.Xml.Serialization;
using GeneralTools.Models;

namespace CarDocu.Models
{
    public class AppDomain : ModelBase
    {
        private string _domainName;
        public string DomainName
        {
            get { return _domainName; }
            set
            {
                _domainName = value;
                SendPropertyChanged("DomainName");
                SendPropertyChanged("UiText");
            }
        }

        private string _domainPath;
        public string DomainPath
        {
            get { return _domainPath; }
            set
            {
                DomainPathIsDirty = (DomainPath != null && value.NotNullOrEmpty() != DomainPath.NotNullOrEmpty());

                _domainPath = value;
                SendPropertyChanged("DomainPath");
                SendPropertyChanged("UiText");
            }
        }

        private bool _domainPathIsDirty;
        [XmlIgnore]
        public bool DomainPathIsDirty
        {
            get { return _domainPathIsDirty; }
            set
            {
                _domainPathIsDirty = value;
                SendPropertyChanged("DomainPathIsDirty");
            }
        }

        [XmlIgnore]
        public bool DomainNameIsValid { get { return !string.IsNullOrEmpty(DomainName); } }

        public bool DomainPathIsValid { get { return !string.IsNullOrEmpty(DomainPath); } }

        public string UiText
        {
            get
            {
                if (DomainName.NotNullOrEmpty() == "")
                    return "( bitte wählen )";

                if (DomainName.NotNullOrEmpty() == "-")
                    return "( manuell eingeben )";

                return $"{DomainName} ({DomainPath})";
            }
        }
    }
}
