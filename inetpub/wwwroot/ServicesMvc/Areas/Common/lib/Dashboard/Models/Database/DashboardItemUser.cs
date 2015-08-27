using System.ComponentModel.DataAnnotations;

namespace CkgDomainLogic.General.Database.Models
{
    public class DashboardItemUser
    {
        [Key]
        public string UserName { get; set; }

        public string AnnotatorItemsXml { get; set; }
    }
}