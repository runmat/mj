using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CkgDomainLogic.General.Database.Models
{
    [Table("WebUserSessionContext")]
    public class WebUserSessionContext
    {
        [Key]
        public int ID { get; set; }

        public string UserName { get; set; }

        public string ContextKey { get; set; }

        public string ContextData { get; set; }

        public DateTime? InsertDatum { get; set; }

        public DateTime? UpdDatum { get; set; }
    }
}
