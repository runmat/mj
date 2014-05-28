using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Objects.DataClasses;
using System.Linq;
using System.Text;
using System.Windows;

namespace CKGDatabaseAdminLib.DbEntities
{
    public partial class LoginUserMessage : EntityObject
    {
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
