// ReSharper disable RedundantUsingDirective
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Xml.Serialization;
using CkgDomainLogic.General.Models;
using CkgDomainLogic.General.ViewModels;
using System.Web.Mvc;
using CkgDomainLogic.FzgModelle.Contracts;
using CkgDomainLogic.FzgModelle.Models;
using CkgDomainLogic.FzgModelle.Models;
using GeneralTools.Models;
using System.IO;
using GeneralTools.Services;
// ReSharper restore RedundantUsingDirective

namespace CkgDomainLogic.FzgModelle.ViewModels
{
    public class ModellIdViewModel : CkgBaseViewModel
    {
        [XmlIgnore]
        public IModellIdDataService DataService { get { return CacheGet<IModellIdDataService>(); } }

        public bool InsertMode { get; set; }

        [XmlIgnore]
        public List<ModellId> ModellIds
        {
            get { return PropertyCacheGet(() => new List<ModellId>()); }
            private set { PropertyCacheSet(value); }
        }

        [XmlIgnore]
        public List<ModellId> ModellIdsFiltered
        {
            get { return PropertyCacheGet(() => ModellIds); }
            private set { PropertyCacheSet(value); }
        }

        public void DataInit()
        {
            DataMarkForRefresh();
        }

        public void DataMarkForRefresh()
        {
            PropertyCacheClear(this, m => m.ModellIdsFiltered);
        }

        public void Validate(Action<string, string> addModelError)
        {
        }

        public void LoadModellIds()
        {
            ModellIds = DataService.GetModellIds();

            DataMarkForRefresh();

            //XmlService.XmlSerializeToFile(ModellId, Path.Combine(AppSettings.DataPath, @"ModellId.xml"));
        }

        public ModellId GetItem(string id)
        {
            return ModellIds.FirstOrDefault(m => m.ID == id);
        }

        public void AddItem(ModellId newItem)
        {
            ModellIds.Add(newItem);
        }

        public ModellId NewItem()
        {
            return new ModellId
            {
                ID = "",
                Bezeichnung = "[Bez]",
            };
        }

        public void SaveItem(ModellId item, Action<string, string> addModelError)
        {
            var errorMessage = DataService.SaveModellId(item);

            DataMarkForRefresh();
            
            if (errorMessage.IsNotNullOrEmpty())
                addModelError("", errorMessage);
        }

        public void ValidateModel(ModellId model, bool insertMode, Action<Expression<Func<ModellId, object>>, string> addModelError)
        {
        }

        public void FilterModellIds(string filterValue, string filterProperties)
        {
            ModellIdsFiltered = ModellIds.SearchPropertiesWithOrCondition(filterValue, filterProperties);
        }
    }
}
