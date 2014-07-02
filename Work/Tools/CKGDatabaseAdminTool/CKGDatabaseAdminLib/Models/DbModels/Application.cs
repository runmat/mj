using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CKGDatabaseAdminLib.Models
{
    /// <summary>
    /// ACHTUNG: Diese Klasse entspricht der Tabellendefinition auf den DAD-Servern und ist u.U. nicht 1:1 auf die anderen SQL-Datenbanken anwendbar!
    /// </summary>
    [Table("Application")]
    public class Application : ModelBase
    {
        private int _appId;
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AppID 
        {
            get { return _appId; }
            set
            {
                if (_appId != value)
                {
                    _appId = value;
                    OnPropertyChanged("AppID");
                }
            }
        }

        private string _appName;
        [Required]
        [MaxLength(50)]
        public string AppName
        {
            get { return _appName; }
            set
            {
                if (_appName != value)
                {
                    _appName = value;
                    OnPropertyChanged("AppName");
                }
            }
        }

        private string _appFriendlyName;
        [Required]
        [MaxLength(50)]
        public string AppFriendlyName
        {
            get { return _appFriendlyName; }
            set
            {
                if (_appFriendlyName != value)
                {
                    _appFriendlyName = value;
                    OnPropertyChanged("AppFriendlyName");
                }
            }
        }

        private string _appType;
        [Required]
        [MaxLength(50)]
        public string AppType
        {
            get { return _appType; }
            set
            {
                if (_appType != value)
                {
                    _appType = value;
                    OnPropertyChanged("AppType");
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

        private bool _appInMenu;
        [Required]
        public bool AppInMenu
        {
            get { return _appInMenu; }
            set
            {
                if (_appInMenu != value)
                {
                    _appInMenu = value;
                    OnPropertyChanged("AppInMenu");
                }
            }
        }

        private string _appComment;
        [MaxLength(150)]
        public string AppComment
        {
            get { return _appComment; }
            set
            {
                if (_appComment != value)
                {
                    _appComment = value;
                    OnPropertyChanged("AppComment");
                }
            }
        }

        private int _appParent;
        [Required]
        public int AppParent
        {
            get { return _appParent; }
            set
            {
                if (_appParent != value)
                {
                    _appParent = value;
                    OnPropertyChanged("AppParent");
                }
            }
        }

        private int _appRank;
        [Required]
        public int AppRank
        {
            get { return _appRank; }
            set
            {
                if (_appRank != value)
                {
                    _appRank = value;
                    OnPropertyChanged("AppRank");
                }
            }
        }

        private int _authorizationLevel;
        [Required]
        public int AuthorizationLevel
        {
            get { return _authorizationLevel; }
            set
            {
                if (_authorizationLevel != value)
                {
                    _authorizationLevel = value;
                    OnPropertyChanged("AuthorizationLevel");
                }
            }
        }

        private bool _batchAuthorization;
        [Required]
        public bool BatchAuthorization
        {
            get { return _batchAuthorization; }
            set
            {
                if (_batchAuthorization != value)
                {
                    _batchAuthorization = value;
                    OnPropertyChanged("BatchAuthorization");
                }
            }
        }

        private bool _logDuration;
        [Required]
        public bool LogDuration
        {
            get { return _logDuration; }
            set
            {
                if (_logDuration != value)
                {
                    _logDuration = value;
                    OnPropertyChanged("LogDuration");
                }
            }
        }

        private int? _appSchwellwert;
        public int? AppSchwellwert
        {
            get { return _appSchwellwert; }
            set
            {
                if (_appSchwellwert != value)
                {
                    _appSchwellwert = value;
                    OnPropertyChanged("AppSchwellwert");
                }
            }
        }

        private int? _maxLevel;
        public int? MaxLevel
        {
            get { return _maxLevel; }
            set
            {
                if (_maxLevel != value)
                {
                    _maxLevel = value;
                    OnPropertyChanged("MaxLevel");
                }
            }
        }

        private int? _maxLevelsPerGroup;
        public int? MaxLevelsPerGroup
        {
            get { return _maxLevelsPerGroup; }
            set
            {
                if (_maxLevelsPerGroup != value)
                {
                    _maxLevelsPerGroup = value;
                    OnPropertyChanged("MaxLevelsPerGroup");
                }
            }
        }

        private string _appTechType;
        [MaxLength(20)]
        public string AppTechType
        {
            get { return _appTechType; }
            set
            {
                if (_appTechType != value)
                {
                    _appTechType = value;
                    OnPropertyChanged("AppTechType");
                }
            }
        }

        private string _appDescription;
        [MaxLength(150)]
        public string AppDescription
        {
            get { return _appDescription; }
            set
            {
                if (_appDescription != value)
                {
                    _appDescription = value;
                    OnPropertyChanged("AppDescription");
                }
            }
        }
    }
}
