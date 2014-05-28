using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Xml.Serialization;
using CkgDomainLogic.CoC.Contracts;
using CkgDomainLogic.General.ViewModels;
using GeneralTools.Models;
using CkgDomainLogic.CoC.Models;

namespace CkgDomainLogic.CoC.ViewModels
{
    public class CocTypenViewModel : CkgBaseViewModel, ICocEntityViewModel
    {
        [XmlIgnore]
        public ICocTypenDataService DataService { get { return CacheGet<ICocTypenDataService>(); } }

        [XmlIgnore]
        public List<CocEntity> CocTypen
        {
            get { return DataService.CocTypen; }
        }

        public bool InsertMode { get; set; }


        #region Repository

        public void RemoveItem(int id)
        {
            DataService.DeleteCocTyp(GetItem(id));
        }

        public CocEntity DuplicateItem(int id)
        {
            return PrepareNewItem(() => ModelMapping.Copy(CocTypen.FirstOrDefault(c => c.ID == id)));
        }

        public CocEntity NewItem()
        {
            return PrepareNewItem(() => new CocEntity());
        }

        public CocEntity GetItem(int id)
        {
            return CocTypen.FirstOrDefault(c => c.ID == id);
        }

        CocEntity PrepareNewItem(Func<CocEntity> newItemFunc)
        {
            var newItem = newItemFunc();

            newItem.ID = GetNewID();

            newItem.ERDAT = DateTime.Now;
            newItem.KUNNR = DataService.ToDataStoreKundenNr(LogonContext.KundenNr);

            return newItem;
        }

        public void AddItem(CocEntity newItem)
        {
            CocTypen.Add(newItem);
        }

        public CocEntity SaveItem(CocEntity item, Action<string, string> addModelError)
        {
            return DataService.SaveCocTyp(item, addModelError);
        }

        int GetNewID()
        {
            return CocTypen.None() ? 1 : CocTypen.Max(c => c.ID) + 1;
        }

        public void ValidateModel(CocEntity model, bool insertMode, Action<Expression<Func<CocEntity, object>>, string> addModelError)
        {
            //if (model.COC_34_JA && model.COC_34_NEIN)
            //{
            //    addModelError(m => m.COC_34_JA, "Bitte eindeutig beantworten.");
            //    addModelError(m => m.COC_34_NEIN, null);
            //}
            //if (model.COC_50_JA && model.COC_50_NEIN)
            //{
            //    addModelError(m => m.COC_50_JA, "Bitte eindeutig beantworten.");
            //    addModelError(m => m.COC_50_NEIN, null);
            //}

            var existingItemsOfThisKey = CocTypen.Where(t => t.PrimaryKeyCocTyp == model.PrimaryKeyCocTyp);
            var primaryKeyViolation = (insertMode && existingItemsOfThisKey.Any() ||
                                       !insertMode && existingItemsOfThisKey.Any(t => t.ID != model.ID));
            if (primaryKeyViolation)
            {
                addModelError(m => m.COC_0_2_TYP, "Diese Kombination Typ / Variante / Version ist bereits vorhanden.");
                addModelError(m => m.COC_0_2_VAR, null);
                addModelError(m => m.COC_0_2_VERS, null);
            }
        }

        public void DataMarkForRefresh()
        {
            DataService.CocTypenMarkForRefresh();
        }

        #endregion


        #region Filter

        public string FilterCocTyp { get; set; }

        public string FilterCocVar { get; set; }

        public string FilterCocVers { get; set; }

        public List<CocEntity> CocTypenFiltered
        {
            get
            {
                var data = CocTypen.AsEnumerable();
                if (FilterCocTyp.IsNotNullOrEmpty())
                    data = data.Where(c => c.COC_0_2_TYP.ToLower().Contains(FilterCocTyp.ToLower()));
                if (FilterCocVar.IsNotNullOrEmpty())
                    data = data.Where(c => c.COC_0_2_VAR.ToLower().Contains(FilterCocVar.ToLower()));
                if (FilterCocVers.IsNotNullOrEmpty())
                    data = data.Where(c => c.COC_0_2_VERS.ToLower().Contains(FilterCocVers.ToLower()));

                return data.OrderBy(c => c.COC_0_2_TYP).ThenBy(c => c.COC_0_2_VAR).ThenBy(c => c.COC_0_2_VERS).ToList();
            }
        }

        public List<string> FilterCocTypGroups
        {
            get { return PrepareFilterList(CocTypen.GroupBy(c => c.COC_0_2_TYP).Select(c => c.Key)); }
        }

        public List<string> FilterCocVarGroups
        {
            get { return PrepareFilterList(CocTypenFiltered.GroupBy(c => c.COC_0_2_VAR).Select(c => c.Key)); }
        }

        public List<string> FilterCocVersGroups
        {
            get { return PrepareFilterList(CocTypenFiltered.GroupBy(c => c.COC_0_2_VERS).Select(c => c.Key)); }
        }

        public void SetCascadedFilter(string searchCascadedTyp, string searchCascadedVar, string searchCascadedVers)
        {
            FilterCocTyp = searchCascadedTyp.NotNullOrEmptyOrNullString(NullSelectionString);
            FilterCocVar = searchCascadedVar.NotNullOrEmptyOrNullString(NullSelectionString);
            FilterCocVers = searchCascadedVers.NotNullOrEmptyOrNullString(NullSelectionString);
        }

        #endregion


        #region Helpers


        public string NullSelectionString { get { return "(alle)"; } }

        private static List<string> PrepareFilterList(IEnumerable<string> filterList)
        {
            //return new List<string> { NullSelectionString }.Concat(filterList).ToList();
            return filterList.ToList();
        }

        #endregion

        #region Test

        private LogItem _searchFilterLogItem = new LogItem
        {
            WebAppName = "Autohaus",
            DomainAppName = "Infocenter",
            Message = "SAP Error",
            ExceptionMessage = "option2",
        };
        public LogItem SearchFilterLogItem
        {
            get { return _searchFilterLogItem; }
            set { _searchFilterLogItem = value; }
        }

        #endregion
    }
}
