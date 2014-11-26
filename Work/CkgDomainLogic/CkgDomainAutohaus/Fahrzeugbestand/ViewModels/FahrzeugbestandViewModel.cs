// ReSharper disable RedundantUsingDirective
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using CkgDomainLogic.Autohaus.Contracts;
using CkgDomainLogic.DomainCommon.Models;
using CkgDomainLogic.General.Models;
using CkgDomainLogic.General.Services;
using CkgDomainLogic.General.ViewModels;
using System.Web.Mvc;
using CkgDomainLogic.Fahrzeugbestand.Contracts;
using CkgDomainLogic.Fahrzeugbestand.Models;
using CkgDomainLogic.Fahrzeugbestand.Services;
using GeneralTools.Models;
using System.IO;
using GeneralTools.Resources;
using GeneralTools.Services;

namespace CkgDomainLogic.Fahrzeugbestand.ViewModels
{
    public class FahrzeugbestandViewModel : CkgBaseViewModel
    {
        [XmlIgnore]
        public IFahrzeugAkteBestandDataService DataService
        {
            get { return CacheGet<IFahrzeugAkteBestandDataService>(); }
        }

        public Adresse SelectedHalter
        {
            get { return PropertyCacheGet(() => new Adresse { Name1 = Localize.DropdownDefaultOptionPleaseChoose }); }
            set { PropertyCacheSet(value); }
        }

        public Adresse SelectedKaeufer
        {
            get { return PropertyCacheGet(() => new Adresse { Name1 = Localize.DropdownDefaultOptionPleaseChoose }); }
            set { PropertyCacheSet(value); }
        }
        public FahrzeugAkteBestandSelektor FahrzeugAkteBestandSelektor
        {
            get { return PropertyCacheGet(() => new FahrzeugAkteBestandSelektor()); }
            set { PropertyCacheSet(value); }
        }

        [XmlIgnore]
        public List<FahrzeugAkteBestand> FahrzeugeAkteBestand
        {
            get { return PropertyCacheGet(() => new List<FahrzeugAkteBestand>()); }
            private set { PropertyCacheSet(value); }
        }

        [XmlIgnore]
        public List<FahrzeugAkteBestand> FahrzeugeAkteBestandFiltered
        {
            get { return PropertyCacheGet(() => FahrzeugeAkteBestand); }
            private set { PropertyCacheSet(value); }
        }


        public void DataInit()
        {
            DataMarkForRefresh();
        }

        public void DataMarkForRefresh()
        {
            PropertyCacheClear(this, m => m.FahrzeugeAkteBestandFiltered);
        }

        public void LoadFahrzeugAkteBestand()
        {
            FahrzeugeAkteBestand = DataService.GetFahrzeugeAkteBestand(FahrzeugAkteBestandSelektor);

            DataMarkForRefresh();
        }

        public void Validate(Action<string, string> addModelError)
        {
        }

        public void FilterFahrzeugeAkteBestand(string filterValue, string filterProperties)
        {
            FahrzeugeAkteBestandFiltered = FahrzeugeAkteBestand.SearchPropertiesWithOrCondition(filterValue, filterProperties);
        }

        public Adresse PickPartnerAddressFinished(string partnerKennung, Adresse partner)
        {
            if (partnerKennung == "HALTER")
                SelectedHalter = partner;
            else
                SelectedKaeufer = partner;

            return partner;
        }
    }
}
