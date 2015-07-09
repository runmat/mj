// ReSharper disable RedundantUsingDirective
// ReSharper disable RedundantEmptyObjectOrCollectionInitializer
using System.Linq.Expressions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Xml.Serialization;
using CkgDomainLogic.DomainCommon.Contracts;
using CkgDomainLogic.DomainCommon.Models;
using CkgDomainLogic.DomainCommon.ViewModels;
using CkgDomainLogic.General.Models;
using CkgDomainLogic.General.Services;
using CkgDomainLogic.General.ViewModels;
using System.Web.Mvc;
using CkgDomainLogic.DomainCommon.Contracts;
using CkgDomainLogic.DomainCommon.Models;
using CkgDomainLogic.DomainCommon.Services;
using GeneralTools.Models;
using System.IO;
using GeneralTools.Resources;
using GeneralTools.Services;
using SapORM.Contracts;

namespace CkgDomainLogic.DomainCommon.ViewModels
{
    public class HaendlerAdressenViewModel : CkgBaseViewModel
    {
        [XmlIgnore]
        public IHaendlerAdressenDataService DataService { get { return CacheGet<IHaendlerAdressenDataService>(); } }

        public bool InsertMode { get; set; }

        public HaendlerAdressenSelektor HaendlerAdressenSelektor
        {
            get { return PropertyCacheGet(() => new HaendlerAdressenSelektor { AdressenTyp = "HAENDLER" }); }
            set { PropertyCacheSet(value); }
        }

        [XmlIgnore]
        public List<HaendlerAdresse> HaendlerAdressen
        {
            get { return PropertyCacheGet(() => new List<HaendlerAdresse>()); }
            private set { PropertyCacheSet(value); }
        }

        [XmlIgnore]
        public List<HaendlerAdresse> HaendlerAdressenFiltered
        {
            get { return PropertyCacheGet(() => HaendlerAdressen); }
            private set { PropertyCacheSet(value); }
        }

        [XmlIgnore]
        public List<SelectItem> LaenderList
        {
            get { return PropertyCacheGet(() => DataService.GetLaenderList()); }
        }

        [XmlIgnore]
        public List<SelectItem> LaenderListWithOptionAll
        {
            get { return LaenderList.CopyAndInsertAtTop(new SelectItem { Key = "", Text = Localize.DropdownDefaultOptionAll }); }
        }

        [XmlIgnore]
        public List<SelectItem> LaenderListWithOptionPleaseChoose
        {
            get { return LaenderList.CopyAndInsertAtTop(new SelectItem { Key = "", Text = Localize.DropdownDefaultOptionPleaseChoose }); }
        }

        public bool LandAdressenModus { get { return HaendlerAdressenSelektor.LandAdressenModus; } }
        public bool HaendlerAdressenModus { get { return HaendlerAdressenSelektor.HaendlerAdressenModus; } }


        public void DataInit()
        {
            LoadHaendlerAdressen();
        }

        public void LoadHaendlerAdressen()
        {
            var list = DataService.GetHaendlerAdressen(HaendlerAdressenSelektor);

            if (LandAdressenModus)
                list = list.Where(a => a.HaendlerNr.IsNullOrEmpty()).ToListOrEmptyList();

            if (HaendlerAdressenModus)
                list = list.Where(a => a.HaendlerNr.IsNotNullOrEmpty()).ToListOrEmptyList();

            list = list.Where(a => LaenderList.Any(land => land.Key == a.LaenderCode)).ToListOrEmptyList();

            HaendlerAdressen = list;

            DataMarkForRefresh();
        }

        public void DataMarkForRefresh()
        {
            PropertyCacheClear(this, m => m.HaendlerAdressenFiltered);
        }

        public void Validate(Action<string, string> addModelError)
        {
        }

        public HaendlerAdresse GetItem(string id)
        {
            var model = HaendlerAdressen.FirstOrDefault(m => m.ID == id) ?? new HaendlerAdresse();

            return model;
        }

        public void AddItem(HaendlerAdresse newItem)
        {
            HaendlerAdressen.Add(newItem);
        }

        public HaendlerAdresse NewItem()
        {
            return new HaendlerAdresse
            {
                LaenderCode = HaendlerAdressenSelektor.LandCode,
                LandBrief = "DE",
                LandSchluessel = "DE",
            };
        }

        public void SaveItem(HaendlerAdresse item, Action<string, string> addModelError)
        {
            var errorMessage = DataService.SaveHaendlerAdresse(item);

            if (errorMessage.IsNotNullOrEmpty())
                addModelError("", errorMessage);
            else
                LoadHaendlerAdressen();
        }

        public void ValidateModel(HaendlerAdresse model, bool insertMode, Action<Expression<Func<HaendlerAdresse, object>>, string> addModelError)
        {
            if (!insertMode)
                return;

            if (HaendlerAdressen.Any(m => m.ID.ToLowerAndNotEmpty() == model.ID.ToLowerAndNotEmpty()))
                addModelError(m => m.ID, Localize.ItemAlreadyExistsWithThisID);

            if (HaendlerAdressenModus && model.HaendlerNr.IsNullOrEmpty())
                addModelError(m => m.HaendlerNr, Localize.Required);
        }

        public void FilterHaendlerAdressen(string filterValue, string filterProperties)
        {
            HaendlerAdressenFiltered = HaendlerAdressen.SearchPropertiesWithOrCondition(filterValue, filterProperties);
        }
    }
}
