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
using AppModelMappings = CkgDomainLogic.DomainCommon.Models.AppModelMappings;

namespace CkgDomainLogic.DomainCommon.ViewModels
{
    public class AdressenPflegeViewModel : CkgBaseViewModel
    {
        [XmlIgnore]
        public IAdressenDataService DataService { get { return CacheGet<IAdressenDataService>(); } }

        public string AdressenKennung { get; set; }

        [XmlIgnore]
        public List<Land> Laender { get { return DataService.Laender; } }

        [XmlIgnore]
        public List<Adresse> Adressen
        {
            get
            {
                DataService.KundennrOverride = KundennrOverride;
                return DataService.Adressen.Where(a => a.Kennung == AdressenKennung).ToList();
            }
        }

        public bool InsertMode { get; set; }

        public string KundennrOverride { get; set; }


        public void DataInit(string adressenKennung, string kundennrOverride)
        {
            AdressenKennung = adressenKennung.ToUpper();
            DataService.KundennrOverride = KundennrOverride = kundennrOverride;

            Adresse.Laender = Laender;

            DataMarkForRefresh();
        }

        public void DataMarkForRefresh()
        {
            DataService.MarkForRefreshAdressen();
            PropertyCacheClear(this, m => m.AdressenFiltered);
        }


        #region Repository

        public Adresse GetItem(int id)
        {
            return Adressen.FirstOrDefault(c => c.ID == id);
        }

        public void RemoveItem(int id)
        {
            DataService.KundennrOverride = KundennrOverride;
            DataService.DeleteAdresse(GetItem(id));
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
            DataService.KundennrOverride = KundennrOverride;
            var savdItem = DataService.SaveAdresse(item, addModelError);
            DataMarkForRefresh();
            return savdItem;
        }

        public void ValidateModel(Adresse model, bool insertMode, Action<Expression<Func<Adresse, object>>, string> addModelError)
        {
            //var existingItemsOfThisKey = Adressen.Where(t => t.ID == model.ID);
            //var primaryKeyViolation = (insertMode && existingItemsOfThisKey.Any() ||
            //                           !insertMode && existingItemsOfThisKey.Any(t => t.ID != model.ID));
            //if (primaryKeyViolation)
            //{
            //    addModelError(m => m.COC_0_2_TYP, "Diese Kombination Typ / Variante / Version ist bereits vorhanden.");
            //    addModelError(m => m.COC_0_2_VAR, null);
            //    addModelError(m => m.COC_0_2_VERS, null);
            //}
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
