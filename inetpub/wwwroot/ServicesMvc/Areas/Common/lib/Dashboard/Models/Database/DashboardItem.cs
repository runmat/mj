using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CkgDomainLogic.General.Contracts;
using GeneralTools.Models;
using GeneralTools.Services;

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

        public string ItemOptions { get; set; }

        public IDashboardItemOptions Options
        {
            get { return ItemOptions.IsNullOrEmpty() ? new DashboardItemOptions() : XmlService.XmlDeserializeFromString<DashboardItemOptions>(ItemOptions); }
        }

        [NotMapped]
        public int UserSort { get { return ItemAnnotator == null ? 0 : ItemAnnotator.UserSort; } }

        [NotMapped]
        public bool IsUserVisible { get { return ItemAnnotator != null && ItemAnnotator.IsUserVisible; } }

        public bool IsChart { get { return Options.IsChart; } }

        public bool IsPartialView { get { return Options.IsPartialView; } }

        public int RowSpanReal { get { return ItemAnnotator.RowSpanOverride > 0 ? ItemAnnotator.RowSpanOverride : Options.RowSpan; } }

        [NotMapped]
        public IDashboardItemAnnotator ItemAnnotator { get; set; }
    }
}