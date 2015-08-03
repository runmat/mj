using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Serialization;
using CkgDomainLogic.General.Contracts;

namespace CkgDomainLogic.General.Database.Models
{
    public class DashboardItem : IDashboardItem
    {
        [Key]
        public int ID { get; set; }

        [XmlIgnore]
        public string Title { get; set; }

        [XmlIgnore]
        public string RelatedAppUrl { get; set; }

        [XmlIgnore]
        public string RelatedSelectorModel { get; set; }

        [XmlIgnore]
        public string ChartJsonOptions { get; set; }

        [XmlIgnore]
        public int? InitialSort { get; set; }

        [NotMapped]
        public int UserSort { get; set; }

        [NotMapped]
        public bool IsUserVisible { get; set; }
    }
}