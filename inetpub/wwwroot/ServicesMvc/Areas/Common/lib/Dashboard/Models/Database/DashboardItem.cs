using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CkgDomainLogic.General.Contracts;

namespace CkgDomainLogic.General.Database.Models
{
    public class DashboardItem : IDashboardItem
    {
        [Key]
        public int ID { get; set; }

        public string ItemKey { get; set; }

        [NotMapped]
        public string Title { get; set; }

        public string RelatedAppUrl { get; set; }

        public string RelatedSelectorModel { get; set; }

        public string ChartJsonOptions { get; set; }

        public int? InitialSort { get; set; }

        public string ChartJsonDataCustomizingScriptFunction { get; set; }

        [NotMapped]
        public int UserSort { get { return ItemAnnotator == null ? 0 : ItemAnnotator.UserSort; } }

        [NotMapped]
        public bool IsUserVisible { get { return ItemAnnotator != null && ItemAnnotator.IsUserVisible; } }
 
        [NotMapped]
        public IDashboardItemAnnotator ItemAnnotator { get; set; }
    }
}