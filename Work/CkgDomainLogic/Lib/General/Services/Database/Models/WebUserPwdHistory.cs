using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CkgDomainLogic.General.Database.Models
{
    [Table("WebUserPwdHistory")]
    public class WebUserPwdHistory
    {
        [Key]
        public int ID { get; set; }

        public int UserID { get; set; }

        public string Password { get; set; }

        public DateTime DateOfChange { get; set; }

        public bool InitialPwd { get; set; }
    }
}
