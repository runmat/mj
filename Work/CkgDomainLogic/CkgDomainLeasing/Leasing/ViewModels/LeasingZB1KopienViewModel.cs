using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Mvc;
using System.Xml.Serialization;
using CkgDomainLogic.General.ViewModels;
using CkgDomainLogic.Leasing.Contracts;
using CkgDomainLogic.Leasing.Models;
using GeneralTools.Models;

namespace CkgDomainLogic.Leasing.ViewModels
{
    public class LeasingZB1KopienViewModel : CkgBaseViewModel
    {
        [XmlIgnore]
        public ILeasingZB1KopienDataService DataService { get { return CacheGet<ILeasingZB1KopienDataService>(); } }

        [XmlIgnore]
        public List<ZB1Kopie> ZB1Kopien { get { return DataService.ZB1Kopien; } }

        public void LoadZB1Kopien(ZB1KopieSuchparameter suchparameter)
        {
            DataService.Suchparameter = suchparameter;
            DataService.MarkForRefreshZB1Kopien();
            PropertyCacheClear(this, m => m.ZB1KopienFiltered);
        }


        #region Filter

        public string FilterVertragsnummer { get; set; }

        public string FilterKennzeichen { get; set; }

        public string FilterFahrgestellnummer { get; set; }

        public string FilterHaltername { get; set; }

        [XmlIgnore]
        public List<ZB1Kopie> ZB1KopienFiltered
        {
            get { return PropertyCacheGet(() => ZB1Kopien); }
            private set { PropertyCacheSet(value); }
        }

        public void FilterZB1Kopien(string filterValue, string filterProperties)
        {
            ZB1KopienFiltered = ZB1Kopien.SearchPropertiesWithOrCondition(filterValue, filterProperties);
        }

        #endregion
    }
}
