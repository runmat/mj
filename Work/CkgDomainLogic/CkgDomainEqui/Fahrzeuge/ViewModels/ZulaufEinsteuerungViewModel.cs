using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Xml.Serialization;
using CkgDomainLogic.General.Models;
using CkgDomainLogic.General.Services;
using CkgDomainLogic.General.ViewModels;
using System.Web.Mvc;
using CkgDomainLogic.Fahrzeuge.Contracts;
using CkgDomainLogic.Fahrzeuge.Models;
using GeneralTools.Models;
using System.IO;

namespace CkgDomainLogic.Fahrzeuge.ViewModels
{
    public class ZulaufEinsteuerungViewModel : CkgBaseViewModel
    {

        [XmlIgnore]
        public IZulaufEinsteuerungDataService DataService { get { return CacheGet<IZulaufEinsteuerungDataService>(); } }

                    
        [XmlIgnore]
        public List<ZulaufEinsteuerung> ZulaufEinsteuerungs
        {
            get { return PropertyCacheGet(() => new List<ZulaufEinsteuerung>()); }
            private set { PropertyCacheSet(value); }
        }

        [XmlIgnore]
        public List<ZulaufEinsteuerung> ZulaufEinsteuerungsFiltered
        {
            get { return PropertyCacheGet(() => ZulaufEinsteuerungs); }
            private set { PropertyCacheSet(value); }
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
            PropertyCacheClear(this, m => m.ZulaufEinsteuerungsFiltered);
        }

       
        public void LoadZulaufEinsteuerung()
        {            
            ZulaufEinsteuerungs = DataService.GetZulaufEinsteuerung(); 
            DataMarkForRefresh();

            //XmlService.XmlSerializeToFile(ZulaufEinsteuerungs, Path.Combine(AppSettings.DataPath, @"ZulaufEinsteuerungs.xml"));
        }

        public void FilterZulaufEinsteuerung(string filterValue, string filterProperties)
        {
            ZulaufEinsteuerungsFiltered = ZulaufEinsteuerungs.SearchPropertiesWithOrCondition(filterValue, filterProperties);
        }

       



    }
}
