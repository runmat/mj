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
                return AdressenDataService.Adressen.ToList();
            }
        }

        public bool InsertMode { get; set; }

        public string KundennrOverride { get; set; }

        public string SubKundennr { get; set; }


        public void AdressenDataInit(string adressenKennung, string kundennrOverride, string subKundennr = null)
        {
            if (adressenKennung.IsNullOrEmpty() && AdressenKennungenLocalized.None(kennung => kennung.Key.ToUpper() == adressenKennung.ToUpper()) && AdressenKennungenLocalized.Any())
                adressenKennung = AdressenKennungenLocalized.First().Key;

            AdressenDataService.AdressenKennung = adressenKennung.ToUpper();
            AdressenKennungTemp = adressenKennung;
            AdressenDataService.KundennrOverride = KundennrOverride = kundennrOverride;
            AdressenDataService.SubKundennr = SubKundennr = subKundennr;

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

        public Adresse NewItem()
        {
            return new Adresse
                {
                    ID = AppModelMappings.CreateNewID(),
                    Kennung = AdressenKennung,
                    KennungenToInsert = new List<string> { AdressenKennung },
                    Land = "DE",
                };
        }

        public Adresse SaveItem(Adresse item, Action<string, string> addModelError)
        {
            AdressenDataService.KundennrOverride = KundennrOverride;

            if (item.KennungenToInsert.AnyAndNotNull() && !item.KennungenToInsert.Contains(item.Kennung))
                item.Kennung = item.KennungenToInsert.First();

            var adrList = new List<Adresse> { item };

            if (item.KennungenToInsert.AnyAndNotNull() && item.KennungenToInsert.Count > 1)
            {
                var zusKennungen = item.KennungenToInsert.Where(k => k != item.Kennung).ToList();

                foreach (var zusKennung in zusKennungen)
                {
                    var newItem = ModelMapping.Copy(item);
                    newItem.Kennung = zusKennung;
                    adrList.Add(newItem);
                }
            }

            var savedItems = AdressenDataService.SaveAdressen(adrList, addModelError);        

            DataMarkForRefresh();
            return savedItems.FirstOrDefault(i => i.Kennung == item.Kennung);
        }

        public void ValidateModel(Adresse model, bool insertMode, Action<Expression<Func<Adresse, object>>, string> addModelError)
        {
        }

        #endregion


        #region 

        public void SelectAdresse(string id, bool select, out int allSelectionCount)
        {
            allSelectionCount = 0;
            var sl = Adressen.FirstOrDefault(f => f.KundenNr == id);
            if (sl == null)
                return;

            sl.IsSelected = select;
            allSelectionCount = Adressen.Count(c => c.IsSelected);
        }

        public void SelectAdressen(bool select, out int allSelectionCount, out int allCount, out int allFoundCount)
        {
            Adressen.ToListOrEmptyList().ForEach(sl => (sl.IsSelected) = select);
                      
            allSelectionCount = Adressen.Count(c => c.IsSelected);
            allCount = Adressen.Count;
            allFoundCount = Adressen.Count;
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

        #region EVB-Prüfung -> Rückgabe der Versicherung
        public void GetEvbInstantInfo(string evb, out string message, out bool isValid)
        {
            AdressenDataService.GetEvbVersInfo(evb, out message, out isValid);
        }
        #endregion

    }
}
