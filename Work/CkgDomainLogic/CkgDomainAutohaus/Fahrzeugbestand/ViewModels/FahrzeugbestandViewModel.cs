// ReSharper disable ConvertIfStatementToNullCoalescingExpression
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;
using CkgDomainLogic.DomainCommon.Contracts;
using CkgDomainLogic.DomainCommon.Models;
using CkgDomainLogic.DomainCommon.ViewModels;
using CkgDomainLogic.General.Services;
using CkgDomainLogic.Fahrzeugbestand.Contracts;
using CkgDomainLogic.Fahrzeugbestand.Models;
using GeneralTools.Models;
using System.Web.Script.Serialization;
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

        public void DataInit(string partnerId, string fzgId)
        {
            DataMarkForRefresh();

            if (!string.IsNullOrEmpty(partnerId))
            {
                var partnerIdClear = CryptoMd5.Decrypt(partnerId);

                if (FahrzeugAkteBestandSelektor.HalterForSelection.Any(h => h.KundenNr.NotNullOrEmpty().TrimStart('0') == partnerIdClear.TrimStart('0')))
                    FahrzeugAkteBestandSelektor.Halter = partnerIdClear.ToSapKunnr();
            }

            if (!string.IsNullOrEmpty(fzgId))
                FahrzeugAkteBestandSelektor.FIN = CryptoMd5.Decrypt(fzgId);
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
            FahrzeugeAkteBestand = DataService.GetFahrzeuge(FahrzeugAkteBestandSelektor);

            DataMarkForRefresh();
        }

        private List<Adresse> GetPartnerAdressenForSelection(string partnerArt)
        {
            AdressenDataInit(partnerArt, LogonContext.KundenNr);
            return Adressen;
        }

        public FahrzeugAkteBestand GetTypDaten(string herstellerSchluessel, string typSchluessel, string vvsSchluessel)
        {
            return DataService.GetTypDaten(herstellerSchluessel, typSchluessel, vvsSchluessel);
        }

        FahrzeugAkteBestand GetFahrzeugBestandUsingFinId(string finId)
        {
            return FahrzeugeAkteBestand.FirstOrDefault(f => f.FinID == finId);
        }

        public FahrzeugAkteBestand TryLoadFahrzeugDetailsUsingFinId(string finId)
        {
            CurrentFahrzeug = GetFahrzeugBestandUsingFinId(finId);
            if (CurrentFahrzeug == null)
                CurrentFahrzeug = DataService.GetTypDaten(null, null, null) ?? new FahrzeugAkteBestand { FinID = finId }; 

            return CurrentFahrzeug;
        }

        public void UpdateFahrzeugDetails(FahrzeugAkteBestand model, string finId, Action<string, string> addModelError)
        {
            var savedModel = GetFahrzeugBestandUsingFinId(finId);

            var bestandAlreadyExistedForThisCustomer = (savedModel != null);
            
            if (!bestandAlreadyExistedForThisCustomer)
                savedModel = new FahrzeugAkteBestand { FinID = finId };

            if (model != null)
                ModelMapping.Copy(model, savedModel);

            CurrentFahrzeug = savedModel;

            var errorMessage = DataService.SaveFahrzeuge(new List<FahrzeugAkteBestand> { savedModel });
            if (errorMessage.IsNotNullOrEmpty() && addModelError != null)
                addModelError("", errorMessage);

            DataMarkForRefresh();

            if (!bestandAlreadyExistedForThisCustomer)
                LoadFahrzeuge();
        }

        public void FilterFahrzeugeAkteBestand(string filterValue, string filterProperties)
        {
            FahrzeugeAkteBestandFiltered = FahrzeugeAkteBestand.SearchPropertiesWithOrCondition(filterValue, filterProperties);
        }

        // 20150618 MMA ITA8076 Massenzulassung
        public void SelectFahrzeuge(bool select, Predicate<FahrzeugAkteBestand> filter, out int allSelectionCount, out int allCount)
        {
            FahrzeugeAkteBestandFiltered.Where(f => filter(f)).ToListOrEmptyList().ForEach(f => f.IsSelected = select);
            allSelectionCount = FahrzeugeAkteBestand.Count(x => x.IsSelected);  
            allCount = FahrzeugeAkteBestandFiltered.Count;
        }

        public void SelectFahrzeug(string finId, bool select, out int allSelectionCount)
        {
            allSelectionCount = 0;
            var fzg = FahrzeugeAkteBestand.FirstOrDefault(f => f.FinID == finId);
            if (fzg == null)
                return;

            fzg.IsSelected = select;
            allSelectionCount = FahrzeugeAkteBestand.Count(c => c.IsSelected);
        }

        public string DeleteSelectedVehicles()
        {
            var fzgeToDelete = FahrzeugeAkteBestand.Where(f => f.IsSelected);

            foreach (var fzg in fzgeToDelete)
            {
                fzg.Loeschen = true;
            }

            var errorMessage = DataService.SaveFahrzeuge(fzgeToDelete);

            if (!string.IsNullOrEmpty(errorMessage))
                return errorMessage;

            FahrzeugeAkteBestand.RemoveAll(f => f.IsSelected);

            PropertyCacheClear(this, m => m.FahrzeugeAkteBestandFiltered);

            return "";
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

        [XmlIgnore, ScriptIgnore]
        public List<Adresse> HalterForSelectionFiltered
        {
            get { return PropertyCacheGet(() => HalterForSelection); }
            private set { PropertyCacheSet(value); }
        }

        public void FilterHalterForSelection(string filterValue, string filterProperties)
        {
            HalterForSelectionFiltered = HalterForSelection.SearchPropertiesWithOrCondition(filterValue, filterProperties);
        }

        [XmlIgnore, ScriptIgnore]
        public List<Adresse> KaeuferForSelectionFiltered
        {
            get { return PropertyCacheGet(() => KaeuferForSelection); }
            private set { PropertyCacheSet(value); }
        }

        public void FilterKaeuferForSelection(string filterValue, string filterProperties)
        {
            KaeuferForSelectionFiltered = KaeuferForSelection.SearchPropertiesWithOrCondition(filterValue, filterProperties);
        }
    }
}
