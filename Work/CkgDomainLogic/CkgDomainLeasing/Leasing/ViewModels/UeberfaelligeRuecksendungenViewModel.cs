using System.Collections.Generic;
using System.Web.Mvc;
using System.Xml.Serialization;
using CkgDomainLogic.General.Services;
using CkgDomainLogic.General.ViewModels;
using CkgDomainLogic.Leasing.Contracts;
using CkgDomainLogic.Leasing.Models;
using GeneralTools.Models;

namespace CkgDomainLogic.Leasing.ViewModels
{
    public class UeberfaelligeRuecksendungenViewModel : CkgBaseViewModel
    {
        [XmlIgnore]
        public IUeberfaelligeRuecksendungenDataService DataService { get { return CacheGet<IUeberfaelligeRuecksendungenDataService>(); } }

        [XmlIgnore]
        public List<UeberfaelligeRuecksendung> UeberfaelligeRuecksendungen { get { return DataService.UeberfaelligeRuecksendungen; } }

        public void LoadUeberfaelligeRuecksendungen(UeberfaelligeRuecksendungenSuchparameter suchparameter, ModelStateDictionary state)
        {
            DataService.Suchparameter = suchparameter;

            DataService.MarkForRefreshUeberfaelligeRuecksendungen();
            PropertyCacheClear(this, m => m.UeberfaelligeRuecksendungenFiltered);

            if (UeberfaelligeRuecksendungen.None())
                state.AddModelError("", Localize.NoDataFound);
        }

        public void SaveUeberfaelligeRuecksendung(string equiNr, string memo, bool fristVerlaengern = false)
        {
            var item = UeberfaelligeRuecksendungen.Find(u => u.EquiNr == equiNr);
            item.Memo = memo;

            DataService.SaveUeberfaelligeRuecksendung(item);
        }

        public void FristVerlaengern(string equiNr)
        {
            var item = UeberfaelligeRuecksendungen.Find(u => u.EquiNr == equiNr);
            item.FristVerlaengert = true;

            DataService.SaveUeberfaelligeRuecksendung(item, true);
        }

        #region Filter

        [XmlIgnore]
        public List<UeberfaelligeRuecksendung> UeberfaelligeRuecksendungenFiltered
        {
            get { return PropertyCacheGet(() => UeberfaelligeRuecksendungen); }
            private set { PropertyCacheSet(value); }
        }

        public void FilterUeberfaelligeRuecksendungen(string filterValue, string filterProperties)
        {
            UeberfaelligeRuecksendungenFiltered = UeberfaelligeRuecksendungen.SearchPropertiesWithOrCondition(filterValue, filterProperties);
        }

        #endregion
    }
}
