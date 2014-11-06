using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CKGDatabaseAdminLib.Models
{
    /// <summary>
    /// Feldübersetzung für Portal-/Services-Anwendungen
    /// </summary>
    [Table("ApplicationField")]
    public class ApplicationField : DbModelBase
    {
        private int _applicationFieldID;
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ApplicationFieldID
        {
            get { return _applicationFieldID; }
            set
            {
                if (_applicationFieldID != value)
                {
                    _applicationFieldID = value;
                    OnPropertyChanged("ApplicationFieldID");
                }
            }
        }

        private string _appURL;
        [Required]
        [MaxLength(150)]
        public string AppURL
        {
            get { return _appURL; }
            set
            {
                if (_appURL != value)
                {
                    _appURL = value;
                    OnPropertyChanged("AppURL");
                }
            }
        }

        private string _fieldType;
        [Required]
        [MaxLength(3)]
        public string FieldType
        {
            get { return _fieldType; }
            set
            {
                if (_fieldType != value)
                {
                    _fieldType = value;
                    OnPropertyChanged("FieldType");
                }
            }
        }

        private string _fieldName;
        [Required]
        [MaxLength(50)]
        public string FieldName
        {
            get { return _fieldName; }
            set
            {
                if (_fieldName != value)
                {
                    _fieldName = value;
                    OnPropertyChanged("FieldName");
                }
            }
        }

        private int _customerID;
        [Required]
        public int CustomerID
        {
            get { return _customerID; }
            set
            {
                if (_customerID != value)
                {
                    _customerID = value;
                    OnPropertyChanged("CustomerID");
                }
            }
        }

        private int _languageID;
        [Required]
        public int LanguageID
        {
            get { return _languageID; }
            set
            {
                if (_languageID != value)
                {
                    _languageID = value;
                    OnPropertyChanged("LanguageID");
                }
            }
        }

        private bool _visibility;
        [Required]
        public bool Visibility
        {
            get { return _visibility; }
            set
            {
                if (_visibility != value)
                {
                    _visibility = value;
                    OnPropertyChanged("Visibility");
                }
            }
        }

        private string _content;
        [MaxLength(150)]
        public string Content
        {
            get { return _content; }
            set
            {
                if (_content != value)
                {
                    _content = value;
                    OnPropertyChanged("Content");
                }
            }
        }

        private string _toolTip;
        [MaxLength(150)]
        public string ToolTip
        {
            get { return _toolTip; }
            set
            {
                if (_toolTip != value)
                {
                    _toolTip = value;
                    OnPropertyChanged("ToolTip");
                }
            }
        }

        private int? _groupID;
        public int? GroupID
        {
            get { return _groupID; }
            set
            {
                if (_groupID != value)
                {
                    _groupID = value;
                    OnPropertyChanged("GroupID");
                }
            }
        }
    }
}