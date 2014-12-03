// ReSharper disable RedundantUsingDirective
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Xml.Serialization;
using CkgDomainLogic.Autohaus.Contracts;
using CkgDomainLogic.DomainCommon.Contracts;
using CkgDomainLogic.DomainCommon.Models;
using CkgDomainLogic.DomainCommon.ViewModels;
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
using SapORM.Contracts;

namespace CkgDomainLogic.Fahrzeugbestand.ViewModels
{
    public class FahrzeugbestandViewModel : AdressenPflegeViewModel
    {
        [XmlIgnore]
        public IFahrzeugAkteBestandDataService DataService { get { return CacheGet<IFahrzeugAkteBestandDataService>(); } }

        [XmlIgnore]
        public override IAdressenDataService AdressenDataService { get { return DataService; } }

        public IHtmlString AdressenKennungLocalized { get { return new HtmlString(AdressenKennung == "HALTER" ? Localize.Holder : Localize.Buyer); } }


        public FahrzeugAkteBestandSelektor FahrzeugAkteBestandSelektor
        {
            get { return PropertyCacheGet(() => new FahrzeugAkteBestandSelektor()); }
            set { PropertyCacheSet(value); }
        }

        public FahrzeugAkteBestandSelektor FinSearchSelektor
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

        public FahrzeugAkteBestand CurrentFahrzeug { get; set; }

        public List<Adresse> HalterForSelection { get { return PropertyCacheGet(() => GetPartnerAdressenForSelection("HALTER")); } }

        public List<Adresse> KaeuferForSelection { get { return PropertyCacheGet(() => GetPartnerAdressenForSelection("KAEUFER")); } }


        public void DataInit()
        {
            DataMarkForRefresh();
        }

        public override void DataMarkForRefresh()
        {
            base.DataMarkForRefresh();

            DataMarkForRefreshPartnerAdressen();
            PropertyCacheClear(this, m => m.FahrzeugeAkteBestandFiltered);
        }

        public void DataMarkForRefreshPartnerAdressen()
        {
            PropertyCacheClear(this, m => m.HalterForSelection);
            PropertyCacheClear(this, m => m.KaeuferForSelection);
        }

        public void LoadFahrzeuge()
        {
            FahrzeugeAkteBestand = DataService.GetFahrzeugeAkteBestand(FahrzeugAkteBestandSelektor);

            DataMarkForRefresh();
        }

        private List<Adresse> GetPartnerAdressenForSelection(string partnerArt)
        {
            AdressenDataInit(partnerArt, LogonContext.KundenNr);
            return Adressen.CopyAndInsertAtTop(new Adresse { Name1 = Localize.DropdownDefaultOptionPleaseChoose });
        }

        public void ValidateSearch(Action<string, string> addModelError)
        {
        }

        public void ValidateFinSearch(Action<string, string> addModelError)
        {
            if (FinSearchSelektor.FIN.IsNullOrEmpty())
                addModelError("", Localize.PleaseFillOutForm);
        }

        public FahrzeugAkteBestand LoadFahrzeugDetailsUsingFin(string fin)
        {
            return CurrentFahrzeug =
                FahrzeugeAkteBestand.FirstOrDefault(f => f.FIN == fin)
                ?? new FahrzeugAkteBestand { FIN = fin };
        }

        public void UpdateFahrzeugDetails(FahrzeugAkteBestand model, Action<string, string> addModelError)
        {
            var savedModel = LoadFahrzeugDetailsUsingFin(model.FIN);

            ModelMapping.Copy(model, savedModel);
            CurrentFahrzeug = savedModel;

            var errorMessage = DataService.SaveFahrzeugAkteBestand(savedModel);
            if (errorMessage.IsNotNullOrEmpty())
                addModelError("", errorMessage);

            if (savedModel.FinID.IsNullOrEmpty())
                LoadFahrzeuge();
            else
                DataMarkForRefresh();
        }

        public void FilterFahrzeugeAkteBestand(string filterValue, string filterProperties)
        {
            FahrzeugeAkteBestandFiltered = FahrzeugeAkteBestand.SearchPropertiesWithOrCondition(filterValue, filterProperties);
        }

        public Adresse GetPartnerAdresse(string partnerArt, string id)
        {
            if (id.IsNullOrEmpty())
                return new Adresse { Name1 = Localize.DropdownDefaultOptionPleaseChoose };

            AdressenDataInit(partnerArt, LogonContext.KundenNr);
            return Adressen.FirstOrDefault(a => a.KundenNr.ToSapKunnr() == id.ToSapKunnr()) ?? new Adresse { Name1 = Localize.DropdownDefaultOptionPleaseChoose };
        }

        public Adresse PickPartnerAddressFinished(int partnerID)
        {
            return GetPartnerAdresse(AdressenKennung, partnerID.ToString());
        }

        public override Adresse GetItem(int id)
        {
            return Adressen.FirstOrDefault(c => c.KundenNr.ToSapKunnr() == id.ToString().ToSapKunnr());
        }
    }
}
