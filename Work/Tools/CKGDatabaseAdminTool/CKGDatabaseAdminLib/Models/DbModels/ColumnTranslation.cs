using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CKGDatabaseAdminLib.Models
{
    /// <summary>
    /// Spaltenübersetzung für Portal-/Services-Anwendungen
    /// </summary>
    [Table("ColumnTranslation")]
    public class ColumnTranslation : DbModelBase
    {
        private int _appID;
        [Key, Column(Order = 0)]
        [Required]
        public int AppID
        {
            get { return _appID; }
            set
            {
                if (_appID != value)
                {
                    _appID = value;
                    OnPropertyChanged("AppID");
                }
            }
        }

        private string _orgName;
        [Key, Column(Order = 1)]
        [Required]
        [MaxLength(50)]
        public string OrgName
        {
            get { return _orgName; }
            set
            {
                if (_orgName != value)
                {
                    _orgName = value;
                    OnPropertyChanged("OrgName");
                }
            }
        }

        private string _newName;
        [Required]
        [MaxLength(50)]
        public string NewName
        {
            get { return _newName; }
            set
            {
                if (_newName != value)
                {
                    _newName = value;
                    OnPropertyChanged("NewName");
                }
            }
        }

        private int? _displayOrder;
        public int? DisplayOrder
        {
            get { return _displayOrder; }
            set
            {
                if (_displayOrder != value)
                {
                    _displayOrder = value;
                    OnPropertyChanged("DisplayOrder");
                }
            }
        }

        private bool _nullenEntfernen;
        [Required]
        [Column("NULLENENTFERNEN")]
        public bool NullenEntfernen
        {
            get { return _nullenEntfernen; }
            set
            {
                if (_nullenEntfernen != value)
                {
                    _nullenEntfernen = value;
                    OnPropertyChanged("NullenEntfernen");
                }
            }
        }

        private bool _textBereinigen;
        [Required]
        [Column("TEXTBEREINIGEN")]
        public bool TextBereinigen
        {
            get { return _textBereinigen; }
            set
            {
                if (_textBereinigen != value)
                {
                    _textBereinigen = value;
                    OnPropertyChanged("TextBereinigen");
                }
            }
        }

        private bool _istDatum;
        [Required]
        [Column("ISTDATUM")]
        public bool IstDatum
        {
            get { return _istDatum; }
            set
            {
                if (_istDatum != value)
                {
                    _istDatum = value;
                    OnPropertyChanged("IstDatum");
                }
            }
        }


        private bool _abeDaten;
        [Required]
        public bool ABEDaten
        {
            get { return _abeDaten; }
            set
            {
                if (_abeDaten != value)
                {
                    _abeDaten = value;
                    OnPropertyChanged("ABEDaten");
                }
            }
        }

        private string _alignment;
        [Required]
        [MaxLength(10)]
        public string Alignment
        {
            get { return _alignment; }
            set
            {
                if (_alignment != value)
                {
                    _alignment = value;
                    OnPropertyChanged("Alignment");
                }
            }
        }

        private bool _istZeit;
        [Required]
        [Column("ISTZEIT")]
        public bool IstZeit
        {
            get { return _istZeit; }
            set
            {
                if (_istZeit != value)
                {
                    _istZeit = value;
                    OnPropertyChanged("IstZeit");
                }
            }
        }
    }
}