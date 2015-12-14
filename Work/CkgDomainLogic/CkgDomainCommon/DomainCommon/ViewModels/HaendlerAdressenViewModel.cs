// ReSharper disable RedundantEmptyObjectOrCollectionInitializer
using System.Linq.Expressions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using CkgDomainLogic.DomainCommon.Contracts;
using CkgDomainLogic.DomainCommon.Models;
using CkgDomainLogic.General.Services;
using CkgDomainLogic.General.ViewModels;
using GeneralTools.Models;

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
        public List<LandExt> LaenderList
        {
            get { return PropertyCacheGet(() => DataService.GetLaenderList()); }
        }

        [XmlIgnore]
        public List<LandExt> LaenderListWithOptionAll
        {
            get { return LaenderList.CopyAndInsertAtTop(new LandExt { Code = "", CodeExt = "", Bezeichnung = Localize.DropdownDefaultOptionAll }); }
        }

        [XmlIgnore]
        public List<LandExt> LaenderListWithOptionPleaseChoose
        {
            get { return LaenderList.CopyAndInsertAtTop(new LandExt { Code = "", CodeExt = "", Bezeichnung = Localize.DropdownDefaultOptionPleaseChoose }); }
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

            list = list.Where(a => LaenderList.Any(land => land.CodeExt == a.LaenderCode)).ToListOrEmptyList();

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
            var selectedLand = HaendlerAdressenSelektor.Land;

            return new HaendlerAdresse
            {
                LaenderCode = HaendlerAdressenSelektor.LandCode,
                LandBrief = (selectedLand != null ? selectedLand.Code : "DE"),
                LandSchluessel = (selectedLand != null ? selectedLand.Code : "DE"),
                SpracheAnschreiben = "DE"
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
            if (insertMode)
            {
                if (HaendlerAdressen.Any(m => m.ID.ToLowerAndNotEmpty() == model.ID.ToLowerAndNotEmpty()))
                    addModelError(m => m.ID, Localize.ItemAlreadyExistsWithThisID);
            }

            if (HaendlerAdressenModus && model.HaendlerNr.IsNullOrEmpty())
                addModelError(m => m.HaendlerNr, Localize.FieldIsRequired);

            if (HaendlerAdressenModus && model.ClientNr.IsNullOrEmpty())
                addModelError(m => m.ClientNr, Localize.FieldIsRequired);

            if (!String.IsNullOrEmpty(model.PlzBrief))
            {
                var plzBriefErg = DataService.CountryPlzValidate(model.LandBrief, model.PlzBrief);
                if (!String.IsNullOrEmpty(plzBriefErg))
                    addModelError(m => m.PlzBrief, plzBriefErg);
            }

            if (!String.IsNullOrEmpty(model.PlzSchluessel))
            {
                var plzSchluesselErg = DataService.CountryPlzValidate(model.LandSchluessel, model.PlzSchluessel);
                if (!String.IsNullOrEmpty(plzSchluesselErg))
                    addModelError(m => m.PlzSchluessel, plzSchluesselErg);
            }
        }

        public void FilterHaendlerAdressen(string filterValue, string filterProperties)
        {
            HaendlerAdressenFiltered = HaendlerAdressen.SearchPropertiesWithOrCondition(filterValue, filterProperties);
        }
    }
}
