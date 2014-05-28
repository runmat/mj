using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CkgDomainLogic.General.Database.Models
{
    [Table("ColumnTranslationsMvc")]
    public class ColumnTranslationsMvc : IEquatable<ColumnTranslationsMvc>
    {
        [Key]
        public int ID { get; set; }

        public string GridGroup { get; set; }

        public int CustomerNo { get; set; }

        public string UserGroupName { get; set; }

        public string UserName { get; set; }

        public string ColumnNames { get; set; }

        public bool Equals(ColumnTranslationsMvc ct)
        {
            return ct.GridGroup == this.GridGroup 
                    && ct.CustomerNo == this.CustomerNo 
                    && ct.UserGroupName == this.UserGroupName 
                    && ct.UserName == this.UserName; 
        }
    }
}
