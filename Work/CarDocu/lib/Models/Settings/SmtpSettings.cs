using System.Xml.Serialization;
using GeneralTools.Contracts;
using GeneralTools.Models;

namespace CarDocu.Models.Settings
{
    public class SmtpSettings : ModelBase, ISmtpSettings  
    {
        private string _server = "192.168.10.30";
        public string Server
        {
            get { return _server; }
            set { _server = value; SendPropertyChanged("Server"); }
        }

        private string _port;
        public string Port
        {
            get { return _port; }
            set { _port = value; SendPropertyChanged("Port"); }
        }

        private bool _enableSsl;
        public bool EnableSsl
        {
            get { return _enableSsl; }
            set { _enableSsl = value; SendPropertyChanged("EnableSsl"); }
        }

        private string _userName;
        public string UserName
        {
            get { return _userName; }
            set { _userName = value; SendPropertyChanged("UserName"); }
        }

        private string _from = "noreply.sgw@kroschke.de";
        public string From
        {
            get { return _from; }
            set { _from = value; SendPropertyChanged("From"); }
        }

        private string _password;
        public string Password
        {
            get { return _password; }
            set { _password = value; SendPropertyChanged("Password"); }
        }

        private int _attachmentMaxSizeMB = 10; 
        public int AttachmentMaxSizeMB 
        { 
            get { return _attachmentMaxSizeMB; }
            set { _attachmentMaxSizeMB = value; SendPropertyChanged("AttachmentMaxSizeMB"); }
        }

        private string _emailSubjectText = "CarDocu PDF-Dateien für ID '{0}'{1}"; 
        public string EmailSubjectText 
        { 
            get { return _emailSubjectText; }
            set { _emailSubjectText = value; SendPropertyChanged("EmailSubjectText"); }
        }

        private string _emailBodyText = "Guten Tag, anbei die CarDocu PDF-Dateien für das von Ihnen gescannte Dokument mit der ID '{0}'{1}."; 
        public string EmailBodyText 
        { 
            get { return _emailBodyText; }
            set { _emailBodyText = value; SendPropertyChanged("EmailBodyText"); }
        }

        private string _emailRecipient; 
        public string EmailRecipient 
        { 
            get { return _emailRecipient; }
            set { _emailRecipient = value; SendPropertyChanged("EmailRecipient"); }
        }

        private bool _emailSendAlsoToUser; 
        public bool EmailSendAlsoToUser 
        { 
            get { return _emailSendAlsoToUser; }
            set { _emailSendAlsoToUser = value; SendPropertyChanged("EmailSendAlsoToUser"); }
        }

        [XmlIgnore]
        public string SmtpServer { get { return Server; } set { Server = value; } }

        [XmlIgnore]
        public string SmtpSender { get { return From; } set { From = value; } }
    }
}
