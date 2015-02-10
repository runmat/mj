using System.ComponentModel.DataAnnotations;
using CkgDomainLogic.General.Contracts;

namespace CkgDomainLogic.General.Database.Models
{
    public class DashboardItemUser : IDashboardItemUser
    {
        [Key]
        public int ID { get; set; }

        public string UserName { get; set; }

        public string ItemsXml { get; set; }
    }
}