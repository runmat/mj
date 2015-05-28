using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using CkgDomainLogic.General.ViewModels;
using GeneralTools.Resources;
using CkgDomainLogic.FzgModelle.Contracts;
using CkgDomainLogic.FzgModelle.Models;
using GeneralTools.Models;

namespace CkgDomainLogic.FzgModelle.ViewModels
{
    public class StatusEinsteuerungViewModel : CkgBaseViewModel
    {

        [XmlIgnore]
        public IStatusEinsteuerungDataService DataService { get { return CacheGet<IStatusEinsteuerungDataService>(); } }

                   
        [XmlIgnore]
        public List<StatusEinsteuerung> StatusEinsteuerungs
        {
            get { return PropertyCacheGet(() => new List<StatusEinsteuerung>()); }
            private set { PropertyCacheSet(value); }
        }

        [XmlIgnore]
        public List<StatusEinsteuerung> StatusEinsteuerungsFiltered
        {
            get { return PropertyCacheGet(() => StatusEinsteuerungs); }
            private set { PropertyCacheSet(value); }
        }

        [LocalizedDisplay(LocalizeConstants.VehiclesWithoutZb2)]
        public int ZB2OhneFahrzeugCount {
            get { return PropertyCacheGet(() => DataService.GetZbIIOhneFzgCount()); }
        }

        [LocalizedDisplay(LocalizeConstants.VehiclesBlocked)]
        public int AnzahlGesperrte {
            get { return StatusEinsteuerungsFiltered.Count(s => s.Gesperrt == 1); }
        }

        public void Init()
        {
        
        }

        public void DataInit()
        {          
            DataMarkForRefresh();
        }

        public void DataMarkForRefresh()
        {
            PropertyCacheClear(this, m => m.StatusEinsteuerungsFiltered);
        }


        public void LoadStatusEinsteuerung()
        {
            StatusEinsteuerungs = DataService.GetStatusbericht().Where(s => s.Bestand > 0).ToList();
            DataMarkForRefresh();        
        }

        public void LoadStatusbericht()
        {            
            StatusEinsteuerungs = DataService.GetStatusbericht(); 
            DataMarkForRefresh();

            //XmlService.XmlSerializeToFile(StatusEinsteuerungs, Path.Combine(AppSettings.DataPath, @"StatusEinsteuerungs.xml"));
        }

        public void FilterStatusEinsteuerung(string filterValue, string filterProperties)
        {
            StatusEinsteuerungsFiltered = StatusEinsteuerungs.SearchPropertiesWithOrCondition(filterValue, filterProperties);
        }

       
    }
}
