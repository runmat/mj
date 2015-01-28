using System.ComponentModel.DataAnnotations;
using CkgDomainLogic.General.Contracts;

namespace CkgDomainLogic.General.Database.Models
{
    public class DashboardItem : IDashboardItem
    {
        [Key]
        public int ID { get; set; }

        public string Title { get; set; }

        public string RelatedAppUrl { get; set; }

        public string RelatedSelectorModel { get; set; }

        public string ChartJsonOptions { get; set; }
    }
}