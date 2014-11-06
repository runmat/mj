using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CKGDatabaseAdminLib.Models
{
    [Table("Application")]
    public class ApplicationInfo : DbModelBase
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
    }
}
