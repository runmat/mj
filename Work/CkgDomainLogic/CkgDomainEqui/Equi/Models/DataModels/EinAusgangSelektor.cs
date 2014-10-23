using System.Xml.Serialization;
using CkgDomainLogic.General.Services;
using GeneralTools.Models;
using GeneralTools.Resources;
using GeneralTools.Services;

namespace CkgDomainLogic.Equi.Models
{
    public class EinAusgangSelektor : Store
    {
        [LocalizedDisplay(LocalizeConstants.Type)]
        public string FilterEinAusgangsTyp { get { return PropertyCacheGet(() => "Inputs"); } set { PropertyCacheSet(value); } }

        [LocalizedDisplay(LocalizeConstants.OutputType)]
        public string FilterAusgangsTyp { get { return PropertyCacheGet(() => "ALL"); } set { PropertyCacheSet(value); } }

        [LocalizedDisplay(LocalizeConstants.DateRange)]
        public DateRange DatumRange { get { return PropertyCacheGet(() => new DateRange(DateRangeType.Last3Months) { IsSelected = true }); } set { PropertyCacheSet(value); } }


        [XmlIgnore]
        public string EinAusgangssTypen
        {
            get
            {
                return string.Format("{0},{1};{2},{3}",
                                        "Inputs", Localize.Inputs,
                                        "Outputs", Localize.Outputs);
            }
        }

        [XmlIgnore]
        public string AusgangssTypen
        {
            get
            {
                return string.Format("{0},{1};{2},{3};{4},{5}",
                                        "ALL", Localize.All,
                                        "Temporary", Localize.Temporary,
                                        "Final", Localize.Final);
            }
        }
    }
}
