using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Xml.Serialization;
using CkgDomainLogic.General.Models;
using CkgDomainLogic.General.Services;
using CkgDomainLogic.General.ViewModels;
using System.Web.Mvc;
using CkgDomainLogic.FzgModelle.Contracts;
using CkgDomainLogic.FzgModelle.Models;
using GeneralTools.Models;
using System.IO;

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
            StatusEinsteuerungs = DataService.GetStatusEinsteuerung(); 
            DataMarkForRefresh();

            //XmlService.XmlSerializeToFile(StatusEinsteuerungs, Path.Combine(AppSettings.DataPath, @"StatusEinsteuerungs.xml"));
        }

        public void FilterStatusEinsteuerung(string filterValue, string filterProperties)
        {
            StatusEinsteuerungsFiltered = StatusEinsteuerungs.SearchPropertiesWithOrCondition(filterValue, filterProperties);
        }

       
    }
}
