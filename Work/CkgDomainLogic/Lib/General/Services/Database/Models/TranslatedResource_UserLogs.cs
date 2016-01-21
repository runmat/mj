using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CkgDomainLogic.General.Database.Models
{
    [Table("TranslatedResource_UserLogs")]
    public class TranslatedResourceUserLogs 
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        public string Resource { get; set; }

        public string Format { get; set; }

        public string de_alt { get; set; }
        public string de_kurz_alt { get; set; }

        public string de_neu { get; set; }
        public string de_kurz_neu { get; set; }

        public DateTime? ChangeDate { get; set; }

        public string ChangeUser { get; set; }
    }
}


