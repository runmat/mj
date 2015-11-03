// ReSharper disable RedundantUsingDirective
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Xml.Serialization;
using CkgDomainLogic.General.Models;
using CkgDomainLogic.General.Services;
using CkgDomainLogic.General.ViewModels;
using System.Web.Mvc;
using CkgDomainLogic.FzgModelle.Contracts;
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

        [XmlIgnore]
        public List<Hersteller> HerstellerList
        {
            get { return PropertyCacheGet(() => DataService.Hersteller); }
        }

        public List<SelectItem> AntriebeList
        {
            get
            {
                return PropertyCacheGet(() => new List<SelectItem>
                    {
                        new SelectItem ("", Localize.DropdownDefaultOptionNotSpecified),
                        new SelectItem ("B", Localize.EngineGasoline),
                        new SelectItem ("D", Localize.EngineDiesel),
                        new SelectItem ("K", Localize.EngineCompressor),
                    });
            }
        }

        public void DataInit()
        {
            LoadModellIds();
        }

        public void LoadModellIds()
        {
            ModellIds = DataService.GetModellIds();

            DataMarkForRefresh();

            //XmlService.XmlSerializeToFile(ModellId, Path.Combine(AppSettings.DataPath, @"ModellId.xml"));
        }

        public void DataMarkForRefresh()
        {
            PropertyCacheClear(this, m => m.ModellIdsFiltered);
        }

        public void Validate(Action<string, string> addModelError)
        {
        }

        public ModellId GetItem(string id)
        {
            var model = ModellIds.FirstOrDefault(m => m.ID == id) ?? new ModellId();

            // ggfls. Hersteller Code aktualiseren:
            var herstellerModel = HerstellerList.FirstOrDefault( h => h.Name.NotNullOrEmpty().ToLower() == model.HerstellerName.NotNullOrEmpty().ToLower());
            model.HerstellerCode = "";
            if (herstellerModel != null && model.HerstellerCode != herstellerModel.Code)
                model.HerstellerCode = herstellerModel.Code;

            return model;
        }

        public void AddItem(ModellId newItem)
        {
            ModellIds.Add(newItem);
        }

        public ModellId NewItem(string idToDuplicate)
        {
            if (idToDuplicate.IsNullOrEmpty())
                return new ModellId
                {
                    ID = "",
                };

            var itemToDuplicate = ModellIds.FirstOrDefault(m => m.ID == idToDuplicate);
            if (itemToDuplicate != null)
            {
                var newItem = ModelMapping.Copy(itemToDuplicate);

                newItem.ID = "";
                newItem.ObjectKey = null;

                return newItem;
            }

            return null;
        }

        public void SaveItem(ModellId item, Action<string, string> addModelError)
        {
            var errorMessage = DataService.SaveModellId(item);

            if (errorMessage.IsNotNullOrEmpty())
                addModelError("", errorMessage);
            else
                LoadModellIds();
        }

        public void ValidateModel(ModellId model, bool insertMode, Action<Expression<Func<ModellId, object>>, string> addModelError)
        {
            if (!insertMode)
                return;

            if (ModellIds.Any(m => m.ID.ToLowerAndNotEmpty() == model.ID.ToLowerAndNotEmpty()))
                addModelError(m => m.ID, Localize.ItemAlreadyExistsWithThisID);
        }

        public void FilterModellIds(string filterValue, string filterProperties)
        {
            ModellIdsFiltered = ModellIds.SearchPropertiesWithOrCondition(filterValue, filterProperties);
        }
    }
}
