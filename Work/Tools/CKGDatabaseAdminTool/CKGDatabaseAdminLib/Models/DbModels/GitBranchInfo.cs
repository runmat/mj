using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Serialization;
using CKGDatabaseAdminLib.ViewModels;
using DevExpress.XtraEditors.DXErrorProvider;
using GeneralTools.Models;
using GeneralTools.Services;

namespace CKGDatabaseAdminLib.Models
{
    [Table("GitBranchInfo")]
    public class GitBranchInfo : DbModelBase, IDXDataErrorInfo
    {
        private int _id;
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID
        {
            get { return _id; }
            set
            {
                if (_id != value)
                {
                    _id = value;
                    OnPropertyChanged("ID");
                }
            }
        }

        private string _name;
        [Required]
        [MaxLength(50)]
        public string Name
        {
            get { return _name; }
            set
            {
                if (_name != value)
                {
                    _name = value;
                    OnPropertyChanged("Name");
                }
            }
        }

        private string _entwickler;
        [MaxLength(20)]
        public string Entwickler
        {
            get { return _entwickler; }
            set
            {
                if (_entwickler != value)
                {
                    _entwickler = value;
                    OnPropertyChanged("Entwickler");
                }
            }
        }

        private string _pm;
        [MaxLength(20)]
        public string PM
        {
            get { return _pm; }
            set
            {
                if (_pm != value)
                {
                    _pm = value;
                    OnPropertyChanged("PM");
                }
            }
        }

        private string _anwendung;
        [MaxLength(50)]
        public string Anwendung
        {
            get { return _anwendung; }
            set
            {
                if (_anwendung != value)
                {
                    _anwendung = value;
                    OnPropertyChanged("Anwendung");
                }
            }
        }

        private string _bemerkung;
        [MaxLength(200)]
        public string Bemerkung
        {
            get { return _bemerkung; }
            set
            {
                if (_bemerkung != value)
                {
                    _bemerkung = value;
                    OnPropertyChanged("Bemerkung");
                }
            }
        }

        private DateTime? _imTestSeit;
        public DateTime? ImTestSeit
        {
            get { return _imTestSeit; }
            set
            {
                if (_imTestSeit != value)
                {
                    _imTestSeit = value;
                    OnPropertyChanged("ImTestSeit");
                }
            }
        }

        private string _freigegebenDurch;
        [MaxLength(20)]
        public string FreigegebenDurch
        {
            get { return _freigegebenDurch; }
            set
            {
                if (_freigegebenDurch != value)
                {
                    _freigegebenDurch = value;
                    OnPropertyChanged("FreigegebenDurch");
                }
            }
        }

        private bool _imMaster;
        [Required]
        public bool ImMaster
        {
            get { return _imMaster; }
            set
            {
                if (_imMaster != value)
                {
                    _imMaster = value;
                    OnPropertyChanged("ImMaster");
                }
            }
        }

        private DateTime? _produktivSeit;
        public DateTime? ProduktivSeit
        {
            get { return _produktivSeit; }
            set
            {
                if (_produktivSeit != value)
                {
                    _produktivSeit = value;
                    OnPropertyChanged("ProduktivSeit");
                }
            }
        }

        private bool _deaktiviert;
        [Required]
        public bool Deaktiviert
        {
            get { return _deaktiviert; }
            set
            {
                if (_deaktiviert != value)
                {
                    _deaktiviert = value;
                    OnPropertyChanged("Deaktiviert");
                }
            }
        }

        [GridExportIgnore]
        public string PortalListe { get; set; }

        private GitBranchInfoListItems _portalBoolListe;

        [NotMapped, GridExportIgnore]
        public GitBranchInfoListItems PortalBoolListe
        {
            get
            {
                if (_portalBoolListe != null)
                    return _portalBoolListe;

                if (PortalListe.IsNullOrEmpty())
                    _portalBoolListe = new GitBranchInfoListItems
                    {
                        new GitBranchInfoListItem { Key = "MVC" },
                        new GitBranchInfoListItem { Key = "Services" },
                        new GitBranchInfoListItem { Key = "Portal" },
                    };
                else
                    _portalBoolListe = XmlService.XmlDeserializeFromString<GitBranchInfoListItems>(XmlService.DecompressString(PortalListe));

                _portalBoolListe.ForEach(e => e.OnChange = PortalBoolListeOnChange);

                return _portalBoolListe;
            }
        }

        public void PortalBoolListeOnChange(GitBranchInfoListItem listItem)
        {
            PortalListe = XmlService.CompressString(XmlService.XmlSerializeToString(PortalBoolListe));
            FakeSwapName();
            OnPropertyChanged("Name");
        }

        [GridExportIgnore]
        public string ServerListe { get; set; }

        private GitBranchInfoListItems _serverBoolListe;

        [NotMapped, GridExportIgnore]
        public GitBranchInfoListItems ServerBoolListe
        {
            get
            {
                if (_serverBoolListe != null)
                    return _serverBoolListe;

                if (ServerListe.IsNullOrEmpty())
                    _serverBoolListe = new GitBranchInfoListItems
                    {
                        new GitBranchInfoListItem { Key = "SGW" },
                        new GitBranchInfoListItem { Key = "ON" },
                        new GitBranchInfoListItem { Key = "Partner" },
                    };
                else
                    _serverBoolListe = XmlService.XmlDeserializeFromString<GitBranchInfoListItems>(XmlService.DecompressString(ServerListe));

                _serverBoolListe.ForEach(e => e.OnChange = ServerBoolListeOnChange);

                return _serverBoolListe;
            }
        }

        public void ServerBoolListeOnChange(GitBranchInfoListItem listItem)
        {
            ServerListe = XmlService.CompressString(XmlService.XmlSerializeToString(ServerBoolListe));
            FakeSwapName();
            OnPropertyChanged("Name");
        }

        void FakeSwapName()
        {
            var nameOrg = Name;
            Name = "";
            Name = nameOrg;
        }

        [NotMapped]
        public bool Erledigt { get { return (Deaktiviert || (ImMaster && ProduktivSeit.HasValue)); } }


        public GitBranchInfo()
        {
            if (MainViewModel.Instance.Developer.IsNotNullOrEmpty())
            {
                Entwickler = MainViewModel.Instance.Developer;

                if (Entwickler.ToLower() == "mje")
                    Anwendung = "ServicesMvc";
            }
        }

        public void GetPropertyError(string propertyName, ErrorInfo info)
        {
            switch (propertyName)
            {
                case "Name":
                    ValidatePortalAndServer(info);
                    break;
            }
        }

        public void GetError(ErrorInfo info)
        {
            ValidatePortalAndServer(info);
        }

        private void ValidatePortalAndServer(ErrorInfo info)
        {
            var error = "";

            if (PortalBoolListe.None(e => e.IsChecked))
                error += "Bitte mindestens ein Portal angeben. ";

            if (ServerBoolListe.None(e => e.IsChecked))
                error += "Bitte mindestens einen Server angeben. ";

            SetErrorInfo(info, error, ErrorType.Critical);
        }

        static void SetErrorInfo(ErrorInfo info, string errorText, ErrorType errorType)
        {
            info.ErrorText = errorText;
            info.ErrorType = errorType;
        }
    }

    public class GitBranchInfoListItem : DbModelBase
    {
        [XmlIgnore]
        public Action<GitBranchInfoListItem> OnChange { get; set; }


        private string _key;

        public string Key
        {
            get { return _key; }
            set
            {
                _key = value;
                OnPropertyChanged("Key");
            }
        }

        private bool _isChecked;

        public bool IsChecked
        {
            get { return _isChecked; }
            set
            {
                _isChecked = value;
                OnPropertyChanged("IsChecked");

                if (OnChange != null)
                    OnChange(this);
            }
        }
    }

    public class GitBranchInfoListItems : List<GitBranchInfoListItem>
    {

    }

}
