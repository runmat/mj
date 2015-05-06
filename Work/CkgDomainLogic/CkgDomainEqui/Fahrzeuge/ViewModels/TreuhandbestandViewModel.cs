using System.Collections.Generic;
using System.Xml.Serialization;
using CkgDomainLogic.General.ViewModels;
using CkgDomainLogic.Fahrzeuge.Contracts;
using CkgDomainLogic.Fahrzeuge.Models;
using GeneralTools.Models;
using GeneralTools.Resources;

namespace CkgDomainLogic.Fahrzeuge.ViewModels
{
    public class TreuhandbestandViewModel : CkgBaseViewModel
    {

        [XmlIgnore]
        public IFahrzeugeDataService DataService { get { return CacheGet<IFahrzeugeDataService>(); } }

      
        [XmlIgnore]
        public List<Treuhandbestand> Treuhandbestands
        {
            get { return PropertyCacheGet(() => new List<Treuhandbestand>()); }
            private set { PropertyCacheSet(value); }
        }

        #region Filter
       
        [XmlIgnore]
        public List<Treuhandbestand> TreuhandbestandsFiltered
        {
            get { return PropertyCacheGet(() => Treuhandbestands); }
            private set { PropertyCacheSet(value); }
        }

        public void DataMarkForRefresh()
        {
            PropertyCacheClear(this, m => m.TreuhandbestandsFiltered);
        }

        public void LoadTreuhandbestand()
        {
            Treuhandbestands = DataService.GetTreuhandbestandFromSap();

            DataMarkForRefresh();

            //XmlService.XmlSerializeToFile(Fahrzeuge, Path.Combine(AppSettings.DataPath, @"Fahrzeuge.xml"));
        }

        public void FilterTreuhandbestands(string filterValue, string filterProperties)
        {
            TreuhandbestandsFiltered = Treuhandbestands.SearchPropertiesWithOrCondition(filterValue, filterProperties);
        }

        #endregion

    }
}
