using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CKGDatabaseAdminLib.Models
{
    [Table("LoginUserMessage")]
    public class LoginUserMessage : DbModelBase
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

        private DateTime? _created;
        public DateTime? Created
        {
            get { return _created; }
            set
            {
                if (_created != value)
                {
                    _created = value;
                    OnPropertyChanged("Created");
                }
            }
        }

        private DateTime? _showMessageFrom;
        public DateTime? ShowMessageFrom
        {
            get { return _showMessageFrom; }
            set
            {
                if (_showMessageFrom != value)
                {
                    _showMessageFrom = value;
                    OnPropertyChanged("ShowMessageFrom");
                }
            }
        }

        private DateTime? _showMessageTo;
        public DateTime? ShowMessageTo
        {
            get { return _showMessageTo; }
            set
            {
                if (_showMessageTo != value)
                {
                    _showMessageTo = value;
                    OnPropertyChanged("ShowMessageTo");
                }
            }
        }

        private DateTime? _lockLoginFrom;
        public DateTime? LockLoginFrom
        {
            get { return _lockLoginFrom; }
            set
            {
                if (_lockLoginFrom != value)
                {
                    _lockLoginFrom = value;
                    OnPropertyChanged("LockLoginFrom");
                }
            }
        }

        private DateTime? _lockLoginTo;
        public DateTime? LockLoginTo
        {
            get { return _lockLoginTo; }
            set
            {
                if (_lockLoginTo != value)
                {
                    _lockLoginTo = value;
                    OnPropertyChanged("LockLoginTo");
                }
            }
        }

        private string _message;
        [MaxLength(500)]
        public string Message
        {
            get { return _message; }
            set
            {
                if (_message != value)
                {
                    _message = value;
                    OnPropertyChanged("Message");
                }
            }
        }

        private string _title;
        [MaxLength(50)]
        public string Title
        {
            get { return _title; }
            set
            {
                if (_title != value)
                {
                    _title = value;
                    OnPropertyChanged("Title");
                }
            }
        }

        private bool _lockForProd;
        [Required]
        public bool LockForProd 
        { 
            get { return _lockForProd; } 
            set
            {
                if (_lockForProd != value)
                {
                    _lockForProd = value;
                    OnPropertyChanged("LockForProd");
                }
            } 
        }

        private bool _lockForTest;
        [Required]
        public bool LockForTest
        {
            get { return _lockForTest; }
            set
            {
                if (_lockForTest != value)
                {
                    _lockForTest = value;
                    OnPropertyChanged("LockForTest");
                }
            }
        }

        private bool _editMode;
        [NotMapped]
        public bool EditMode
        {
            get { return _editMode; } 
            set
            {
                if (_editMode != value)
                {
                    _editMode = value;
                    OnPropertyChanged("EditMode");
                }      
            }
        }
    }
}
