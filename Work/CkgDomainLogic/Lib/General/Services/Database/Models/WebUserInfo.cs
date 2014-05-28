using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CkgDomainLogic.General.Database.Models
{
    [Table("WebUserInfo")]
    public class WebUserInfo
    {
        [Key]
        public int ID { get; set; }

        public int ID_User { get; set; }

        public string Mail { get; set; }

        public bool Employee { get; set; }

        public bool Picture { get; set; }

        public int EmployeeHierarchy { get; set; }

        public string Department { get; set; }

        public string Position { get; set; }

        public string Telephone { get; set; }

        public string Fax { get; set; }

        public string LastChangedBy { get; set; }

        public string Telephone2 { get; set; }
    }
}
