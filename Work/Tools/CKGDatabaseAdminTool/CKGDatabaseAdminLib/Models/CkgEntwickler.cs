using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CKGDatabaseAdminLib.Models
{
    public class CkgEntwickler : DbModelBase
    {
        private string _id;

        public string ID
        {
            get { return _id; }
            set
            {
                _id = value;
                SendPropertyChanged("ID");
            }
        }

        private string _userName;

        public string UserName
        {
            get { return _userName; }
            set
            {
                _userName = value;
                SendPropertyChanged("UserName");
            }
        }

        private string _nachname;

        public string Nachname
        {
            get { return _nachname; }
            set
            {
                _nachname = value;
                SendPropertyChanged("Nachname");
            }
        }

        private string _vorname;

        public string Vorname
        {
            get { return _vorname; }
            set
            {
                _vorname = value;
                SendPropertyChanged("Vorname");
            }
        }
    }
}
