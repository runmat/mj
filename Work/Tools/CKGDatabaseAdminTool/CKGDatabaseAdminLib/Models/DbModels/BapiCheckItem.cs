using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace CKGDatabaseAdminLib.Models
{
    [Table("BapiStrukturCheck")]
    public class BapiCheckItem : ModelBase
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

        private string _bapiName;
        [Required]
        [MaxLength(50)]
        public string BapiName
        {
            get { return _bapiName; }
            set
            {
                if (_bapiName != value)
                {
                    _bapiName = value;
                    OnPropertyChanged("BapiName");
                }
            }
        }

        private byte[] _importStruktur;
        [Required]
        public byte[] ImportStruktur
        {
            get { return _importStruktur; }
            set
            {
                if (ImportStruktur == null || !_importStruktur.SequenceEqual(value))
                {
                    _importStruktur = value;
                    OnPropertyChanged("ImportStruktur");
                }
            }
        }

        private byte[] _exportStruktur;
        [Required]
        public byte[] ExportStruktur
        {
            get { return _exportStruktur; }
            set
            {
                if (ExportStruktur == null || !_exportStruktur.SequenceEqual(value))
                {
                    _exportStruktur = value;
                    OnPropertyChanged("ExportStruktur");
                }
            }
        }

        private bool _testSap;
        [Required]
        public bool TestSap
        {
            get { return _testSap; }
            set
            {
                if (_testSap != value)
                {
                    _testSap = value;
                    OnPropertyChanged("TestSap");
                }
            }
        }

        private DateTime _updated;
        [Required]
        public DateTime Updated
        {
            get { return _updated; }
            set
            {
                if (_updated != value)
                {
                    _updated = value;
                    OnPropertyChanged("Updated");
                }
            }
        }
    }
}
