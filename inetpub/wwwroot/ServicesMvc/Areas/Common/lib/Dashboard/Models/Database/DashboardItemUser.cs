using System.ComponentModel.DataAnnotations;

namespace CkgDomainLogic.General.Database.Models
{
    public class DashboardItemUser
    {
        [Key]
        public int ID { get; set; }

        public string UserName { get; set; }

        public string ItemsXml { get; set; }
    }
}