using System.Collections.Generic;
using CkgDomainLogic.General.Models;
using GeneralTools.Models;
using GeneralTools.Resources;
using GeneralTools.Services;
using System.Linq;

namespace CkgDomainLogic.Fahrzeuge.Models
{
    public class AbgemeldeteFahrzeugeSelektor : Store 
    {
        [LocalizedDisplay(LocalizeConstants.CancellationDate)]
        public DateRange AbmeldeDatumRange { get { return PropertyCacheGet(() => new DateRange(DateRangeType.Last30Days)); } set { PropertyCacheSet(value); } }

        [LocalizedDisplay(LocalizeConstants.ExpiryDate)]
        public DateRange GueltigkeitsEndeDatum { get { return PropertyCacheGet(() => new DateRange(DateRangeType.Last30Days)); } set { PropertyCacheSet(value); } }  

        [LocalizedDisplay(LocalizeConstants.OnlyItemsToClarify)]
        public bool NurKlaerfaelle { get; set; }

        [LocalizedDisplay(LocalizeConstants.VIN17)]
        public string Fin
        {
            get { return PropertyCacheGet(() => ""); }
            set { PropertyCacheSet(value.NotNullOrEmpty().ToUpper()); }
        }

        [LocalizedDisplay(LocalizeConstants.VIN10)]
        public string Fin10
        {
            get { return PropertyCacheGet(() => ""); }
            set { PropertyCacheSet(value.NotNullOrEmpty().ToUpper()); }
        }

        static public List<FahrzeugStatus> AlleFahrzeugStatusWerteStatic { get; set; }
        public List<FahrzeugStatus> AlleFahrzeugStatusWerte { get { return AlleFahrzeugStatusWerteStatic.Where(s => s.ID.IsNotNullOrEmpty()).ToList(); } }

        [LocalizedDisplay(LocalizeConstants.Status)]
        public List<string> FahrzeugStatusWerte { get { return PropertyCacheGet(() => new List<string>()); } set { PropertyCacheSet(value); } }  

        [LocalizedDisplay(LocalizeConstants.CostCenter)]
        public string Kostenstelle { get; set; }

        [LocalizedDisplay(LocalizeConstants.Destination)]
        public string Zielort { get; set; }

        [LocalizedDisplay(LocalizeConstants.FactoryNo)]
        public string Betrieb { get; set; }

        [LocalizedDisplay(LocalizeConstants.Department)]
        public string Abteilung { get; set; }

        [LocalizedDisplay(LocalizeConstants.DepartmentHead)]
        public string AbteilungsLeiter { get; set; }
    }
}
