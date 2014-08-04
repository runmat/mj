using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CKGDatabaseAdminLib.Models.DbModels
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

        private bool _inaktiv;
        [Required]
        public bool Inaktiv
        {
            get { return _inaktiv; }
            set
            {
                if (_inaktiv != value)
                {
                    _inaktiv = value;
                    OnPropertyChanged("Inaktiv");
                }
            }
        }
    }
}
