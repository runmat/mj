using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Xml.Serialization;
using CkgDomainLogic.DomainCommon.Contracts;
using CkgDomainLogic.DomainCommon.Models;
using CkgDomainLogic.General.Models;
using CkgDomainLogic.General.Services;
using CkgDomainLogic.General.ViewModels;
using GeneralTools.Models;
using GeneralTools.Resources;
using AppModelMappings = CkgDomainLogic.DomainCommon.Models.AddressModelMappings;

namespace CkgDomainLogic.DomainCommon.ViewModels
{
    public class AdressenPflegeViewModel : CkgBaseViewModel
    {
        [XmlIgnore]
        public virtual IAdressenDataService AdressenDataService { get { return CacheGet<IAdressenDataService>(); } }

        [LocalizedDisplay(LocalizeConstants.AddressType)]
        public string AdressenKennung { get { return AdressenDataService.AdressenKennung; } }

        [LocalizedDisplay(LocalizeConstants.AddressType)]
        public string AdressenKennungTemp { get; set; } 

        [LocalizedDisplay(LocalizeConstants.AddressGroup)]
        public string AdressenKennungGruppe { get { return PropertyCacheGet(() => "VERSAND"); } set { PropertyCacheSet(value); } }

        [XmlIgnore]
        public static List<SelectItem> AllAdressenKennungGruppenLocalized
        {
            get
            {
                return new List<SelectItem>
                    {
                        new SelectItem ("VERSAND", Localize.Shipping),
                        new SelectItem ("UEBERFUEHRUNG", Localize.Overpass),
                    };
            }
        }

        [XmlIgnore]
        public List<SelectItem> AdressenKennungGruppenLocalized
        {
            get { return PropertyCacheGet(() => new List<SelectItem>()); } 
            set { PropertyCacheSet(value); }
        }

        [XmlIgnore]
        public static List<SelectItem> AllAdressenKennungenLocalized
        {
            get
            {
                return new List<SelectItem>
                    {
                        new SelectItem ("VERSANDADRESSE", Localize.ShippingAddress, "VERSAND"),
                        new SelectItem ("ABHOLADRESSE", Localize.PickupAddress, "UEBERFUEHRUNG"),
                        new SelectItem ("AUSLIEFERUNG", Localize.DeliveryAddress, "UEBERFUEHRUNG"),
                        new SelectItem ("RÜCKHOLUNG", Localize.ReturnAddress, "UEBERFUEHRUNG"),
                    };
            }
        }

        [XmlIgnore]
        public List<SelectItem> AdressenKennungenLocalized
        {
            get { return AllAdressenKennungenLocalized.Where(a => a.Group == AdressenKennungGruppe).ToList(); }
        }
            
        [XmlIgnore]
        public List<Land> Laender { get { return AdressenDataService.Laender; } }

        [XmlIgnore]
        public List<Adresse> Adressen
        {
            get
            {
                AdressenDataService.KundennrOverride = KundennrOverride;
                return AdressenDataService.Adressen.Where(a => a.Kennung == AdressenKennung).ToList();
            }
        }

        public bool InsertMode { get; set; }

        public string KundennrOverride { get; set; }


        public void AdressenDataInit(string adressenKennung, string kundennrOverride)
        {
            if (AdressenKennungenLocalized.None(kennung => kennung.Key.ToUpper() == adressenKennung.ToUpper()) && AdressenKennungenLocalized.Any())
                adressenKennung = AdressenKennungenLocalized.First().Key;

            AdressenDataService.AdressenKennung = adressenKennung.ToUpper();
            AdressenKennungTemp = adressenKennung;
            AdressenDataService.KundennrOverride = KundennrOverride = kundennrOverride;

            Adresse.Laender = Laender;

            DataMarkForRefresh();
        }

        public void AdressenKennungGruppeInit()
        {
            AdressenKennungGruppenLocalized = new List<SelectItem>();

            var versandAdressenAvailable = GetApplicationConfigValueForCustomer("AdressenPflegeVersandAdressen").ToBool();
            if (versandAdressenAvailable)
                AdressenKennungGruppenLocalized = AdressenKennungGruppenLocalized.Concat(AllAdressenKennungGruppenLocalized.Where(a => a.Key == "VERSAND")).ToList();

            var uebfuehrgAdressenAvailable = GetApplicationConfigValueForCustomer("AdressenPflegeUeberfuehrungsAdressen").ToBool();
            if (uebfuehrgAdressenAvailable)
                AdressenKennungGruppenLocalized = AdressenKennungGruppenLocalized.Concat(AllAdressenKennungGruppenLocalized.Where(a => a.Key == "UEBERFUEHRUNG")).ToList();

            if (AdressenKennungGruppenLocalized.Any())
                AdressenKennungGruppeChange(AdressenKennungGruppenLocalized.First().Key, "");
        }

        public void AdressenKennungGruppeChange(string adressGruppe, string adressKennung)
        {
            if (adressGruppe == AdressenKennungGruppe)
            {
                if (adressKennung.IsNotNullOrEmpty())
                    AdressenKennungTemp = adressKennung;
            }
            else
            {
                AdressenKennungGruppe = adressGruppe;
                AdressenKennungTemp = AdressenKennungenLocalized.First().Key;
            }

            AdressenDataInit(AdressenKennungTemp, AdressenDataService.KundennrOverride);
        }

        public virtual void DataMarkForRefresh()
        {
            AdressenDataService.MarkForRefreshAdressen();
            PropertyCacheClear(this, m => m.AdressenFiltered);
        }


        #region Repository

        public virtual Adresse GetItem(int id)
        {
            return Adressen.FirstOrDefault(c => c.ID == id);
        }

        public void RemoveItem(int id)
        {
            AdressenDataService.KundennrOverride = KundennrOverride;
            AdressenDataService.DeleteAdresse(GetItem(id));
            DataMarkForRefresh();
        }

        public void AddItem(Adresse newItem)
        {
            Adressen.Add(newItem);
        }

        public Adresse NewItem()
        {
            return new Adresse
                {
                    ID = AppModelMappings.CreateNewID(),
                    Kennung = AdressenKennung,
                    Land = "DE",
                };
        }

        public Adresse SaveItem(Adresse item, Action<string, string> addModelError)
        {
            AdressenDataService.KundennrOverride = KundennrOverride;
            var savedItem = AdressenDataService.SaveAdresse(item, addModelError);
            DataMarkForRefresh();
            return savedItem;
        }

        public void ValidateModel(Adresse model, bool insertMode, Action<Expression<Func<Adresse, object>>, string> addModelError)
        {
        }

        #endregion


        #region Filter

        public string FilterAdresse { get; set; }

        public string FilterCocVar { get; set; }

        public string FilterCocVers { get; set; }

        public List<Adresse> AdressenFiltered
        {
            get { return PropertyCacheGet(() => Adressen); }
            private set { PropertyCacheSet(value); }
        }

        public void FilterVersandAdressen(string filterValue, string filterProperties)
        {
            AdressenFiltered = Adressen.SearchPropertiesWithOrCondition(filterValue, filterProperties);
        }

        #endregion
    }
}
