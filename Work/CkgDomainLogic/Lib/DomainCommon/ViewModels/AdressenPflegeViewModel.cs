using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Xml.Serialization;
using CkgDomainLogic.DomainCommon.Contracts;
using CkgDomainLogic.DomainCommon.Models;
using CkgDomainLogic.General.Models;
using CkgDomainLogic.General.ViewModels;
using GeneralTools.Models;
using AppModelMappings = CkgDomainLogic.DomainCommon.Models.AddressModelMappings;

namespace CkgDomainLogic.DomainCommon.ViewModels
{
    public class AdressenPflegeViewModel : CkgBaseViewModel
    {
        [XmlIgnore]
        public virtual IAdressenDataService AdressenDataService { get { return CacheGet<IAdressenDataService>(); } }

        public string AdressenKennung { get { return AdressenDataService.AdressenKennung; } }

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
            AdressenDataService.AdressenKennung = adressenKennung.ToUpper();
            AdressenDataService.KundennrOverride = KundennrOverride = kundennrOverride;

            Adresse.Laender = Laender;

            DataMarkForRefresh();
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
            var savdItem = AdressenDataService.SaveAdresse(item, addModelError);
            DataMarkForRefresh();
            return savdItem;
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
