using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CKGDatabaseAdminLib.Models
{
    [Table("BAPI")]
    public class BapiTable : DbModelBase
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

        private string _bapi;
        [Required]
        [MaxLength(50)]
        public string BAPI
        {
            get { return _bapi; }
            set
            {
                if (_bapi != value)
                {
                    _bapi = value;
                    OnPropertyChanged("BAPI");
                }
            }
        }
    }
}
