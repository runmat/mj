using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Serialization;
using CKGDatabaseAdminLib.ViewModels;
using GeneralTools.Models;
using GeneralTools.Services;

namespace CKGDatabaseAdminLib.Models
{
    [Table("GitBranchInfo")]
    public class GitBranchInfo : DbModelBase
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

        public string PortalListe { get; set; }

        private GitBranchInfoListItems _portalBoolListe;

        [NotMapped]
        public GitBranchInfoListItems PortalBoolListe
        {
            get
            {
                if (PortalListe.IsNullOrEmpty())
                    PortalBoolListe = new GitBranchInfoListItems
                    {
                        new GitBranchInfoListItem { Key = "Portal" },
                        new GitBranchInfoListItem { Key = "Services" },
                        new GitBranchInfoListItem { Key = "MVC" },
                    };

                _portalBoolListe = XmlService.XmlDeserializeFromString<GitBranchInfoListItems>(PortalListe);
                _portalBoolListe.ForEach(e => e.OnChange = PortalBoolListeOnChange);

                return _portalBoolListe;
            }
            set
            {
                if (_portalBoolListe == value)
                    return;

                _portalBoolListe = value;
                PortalListe = XmlService.XmlSerializeToString(value);

                //OnPropertyChanged("PortalBoolListe");
            }
        }

        public void PortalBoolListeOnChange(GitBranchInfoListItem listItem)
        {
            
        }

        private string _serverListe;

        public string ServerListe
        {
            get { return _serverListe; }
            set
            {
                if (_serverListe == value)
                    return;

                _serverListe = value;
                OnPropertyChanged("ServerListe");
            }
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

        private string _isChecked;

        public string IsChecked
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
