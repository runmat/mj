using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Xml.Serialization;
using GeneralTools.Models;

namespace CarDocu.Models
{
    public class Archive : ModelBase 
    {
        private string _id; 
        public string ID 
        { 
            get { return _id; }
            set { _id = value; SendPropertyChanged("ID"); }
        }

        private string _name; 
        public string Name 
        { 
            get { return _name; }
            set { _name = value; SendPropertyChanged("Name"); }
        }

        private string _path; 
        public string Path 
        { 
            get { return _path; }
            set { _path = value; SendPropertyChanged("Path"); }
        }

        private bool _mailDeliveryNeeded; 
        public bool MailDeliveryNeeded 
        { 
            get { return _mailDeliveryNeeded; }
            set { _mailDeliveryNeeded = value; SendPropertyChanged("MailDeliveryNeeded"); }
        }

        private bool _isBackgroundDeliveryDisabled;
        public bool BackgroundDeliveryDisabled
        {
            get { return _isBackgroundDeliveryDisabled; }
            set { _isBackgroundDeliveryDisabled = value; SendPropertyChanged("BackgroundDeliveryDisabled"); }
        }

        private bool _isInternal;
        public bool IsInternal
        {
            get { return _isInternal; }
            set { _isInternal = value; SendPropertyChanged("IsInternal"); }
        }

        private bool _isOptional;
        public bool IsOptional
        {
            get { return _isOptional; }
            set { _isOptional = value; SendPropertyChanged("IsOptional"); }
        }

        [XmlIgnore]
        public static List<Archive> FixedAvailableArchives
        {
            get
            {
                return new List<Archive>
                           {
                               new Archive {ID = "ELO", Name = "ELO Archiv (Audi)", MailDeliveryNeeded = true},
                               new Archive {ID = "EASY", Name = "Easy Archiv (Kroschke)", MailDeliveryNeeded = false},
                               new Archive {ID = "ZIP", Name = "ZIP-Archivierung", MailDeliveryNeeded = false, BackgroundDeliveryDisabled = true, IsOptional = true},
                               new Archive {ID = "BACKUP", Name = "Backup", MailDeliveryNeeded = false, BackgroundDeliveryDisabled = true, IsInternal = true, IsOptional = true},
                           };
            }
        }

        [XmlIgnore]
        public string IconSource { get { return ((Image)Application.Current.TryFindResource(GetIconSourceKey(ID))).Source.ToString(); } }

        public static string GetIconSourceKey(string archiveCode)
        {
            if (archiveCode.ToUpper() == "ELO")
                return "image/24x24/email";

            if (archiveCode.ToUpper() == "ZIP")
                return "image/24x24/zip";

            if (archiveCode.ToUpper() == "BACKUP")
                return "image/24x24/file_copy";

            return "image/24x24/internet";
        }
    }
}
