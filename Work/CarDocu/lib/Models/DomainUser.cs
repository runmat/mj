using System.Xml.Serialization;
using CarDocu.Services;
using GeneralTools.Models;

namespace CarDocu.Models
{
    public class DomainUser : ModelBase 
    {
        private string _loginID;
        public string LoginID
        {
            get { return _loginID; }
            set { _loginID = value; SendPropertyChanged("LoginID"); }
        }

        private string _loginName;
        public string LoginName
        {
            get { return _loginName; }
            set { _loginName = value; SendPropertyChanged("LoginName"); }
        }

        private string _nachName;
        public string NachName
        {
            get { return _nachName; }
            set { _nachName = value; SendPropertyChanged("NachName"); }
        }

        private string _vorName;
        public string VorName
        {
            get { return _vorName; }
            set { _vorName = value; SendPropertyChanged("VorName"); }
        }

        private string _email;
        public string Email
        {
            get { return _email; }
            set { _email = value; SendPropertyChanged("Email"); }
        }

        //public string Password { get; set; }

        private bool _isAdmin;
        public bool IsAdmin
        {
            get { return _isAdmin; }
            set { _isAdmin = value; SendPropertyChanged("IsAdmin"); }
        }

        private bool _isDefaultUser;
        public bool IsDefaultUser
        {
            get { return _isDefaultUser; }
            set { _isDefaultUser = value; SendPropertyChanged("IsDefaultUser"); }
        }

        private string _domainLocationCode; 
        public string DomainLocationCode 
        { 
            get { return _domainLocationCode; }
            set { _domainLocationCode = value; SendPropertyChanged("DomainLocationCode"); }
        }

        [XmlIgnore]
        public bool IsLogonUser { get { return DomainService.Repository != null && DomainService.Repository.LogonUser == this; } }

        [XmlIgnore]
        public bool IsMaster { get; set; }

        [XmlIgnore]
        public string FullName { get { return string.Format("{0} {1}", VorName, NachName); } }

        private DomainLocation _domainLocation;
        [XmlIgnore]
        public DomainLocation DomainLocation
        {
            get { return _domainLocation; }
            set
            {
                _domainLocation = value;
                if (_domainLocation == null)
                    return;
                SendPropertyChanged("DomainLocation");
                SendPropertyChanged("DomainLocationFriendlyName");
                DomainLocationCode = DomainLocation.SapCode;
            }
        }

        public string DomainLocationFriendlyName { get { return (string.IsNullOrEmpty(DomainLocationCode) ? "" : string.Format("{0} {1}", DomainLocation.SapCode, DomainLocation.Name)); } }
    }
}

