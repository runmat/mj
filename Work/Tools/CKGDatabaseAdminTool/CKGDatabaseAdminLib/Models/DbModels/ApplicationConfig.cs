using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CKGDatabaseAdminLib.Models
{
    [Table("ApplicationConfig")]
    public class ApplicationConfig : DbModelBase
    {
        private int _configID;
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ConfigID
        {
            get { return _configID; }
            set
            {
                if (_configID != value)
                {
                    _configID = value;
                    OnPropertyChanged("ConfigID");
                }
            }
        }

        private int _appID;
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

        private int _groupID;
        [Required]
        public int GroupID
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

        private string _configKey;
        [Required]
        [MaxLength(50)]
        public string ConfigKey
        {
            get { return _configKey; }
            set
            {
                if (_configKey != value)
                {
                    _configKey = value;
                    OnPropertyChanged("ConfigKey");
                }
            }
        }

        private string _configType;
        [Required]
        [MaxLength(10)]
        public string ConfigType
        {
            get { return _configType; }
            set
            {
                if (_configType != value)
                {
                    _configType = value;
                    OnPropertyChanged("ConfigType");
                }
            }
        }

        private string _configValue;
        [MaxLength(200)]
        public string ConfigValue
        {
            get { return _configValue; }
            set
            {
                if (_configValue != value)
                {
                    _configValue = value;
                    OnPropertyChanged("ConfigValue");
                }
            }
        }

        private string _description;
        [MaxLength(200)]
        public string Description
        {
            get { return _description; }
            set
            {
                if (_description != value)
                {
                    _description = value;
                    OnPropertyChanged("Description");
                }
            }
        }
    }
}