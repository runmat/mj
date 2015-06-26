// ReSharper disable RedundantUsingDirective
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
    public class HaendlerAdressenViewModel : AdressenPflegeViewModel
    {
        [XmlIgnore]
        public IHaendlerAdressenDataService DataService { get { return CacheGet<IHaendlerAdressenDataService>(); } }

        [XmlIgnore]
        public override IAdressenDataService AdressenDataService { get { return DataService; } }

        public IHtmlString AdressenKennungLocalized { get { return new HtmlString(Localize.DeliveryAddress); } }


        public HaendlerAdressenSelektor HaendlerAdressenSelektor
        {
            get { return PropertyCacheGet(() => new HaendlerAdressenSelektor { HaendlerAdressenKennung = "VERSANDADRESSE" }); }
            set { PropertyCacheSet(value); }
        }

        [XmlIgnore]
        public List<Adresse> HaendlerAdressen
        {
            get { return Adressen; }
        }

        [XmlIgnore]
        public List<Adresse> HaendlerAdressenFiltered
        {
            get { return PropertyCacheGet(() => HaendlerAdressen); }
            private set { PropertyCacheSet(value); }
        }


        public void DataInit()
        {
            DataMarkForRefresh();
        }

        public override void DataMarkForRefresh()
        {
            base.DataMarkForRefresh();

            PropertyCacheClear(this, m => m.HaendlerAdressenFiltered);
        }

        public void ValidateSearch(Action<string, string> addModelError)
        {
        }

        public void LoadHaendlerAdressens()
        {
            AdressenDataInit(HaendlerAdressenSelektor.HaendlerAdressenKennung, KundennrOverride);
        }

        public void FilterHaendlerAdressens(string filterValue, string filterProperties)
        {
            HaendlerAdressenFiltered = HaendlerAdressen.SearchPropertiesWithOrCondition(filterValue, filterProperties);
        }

        public Adresse GetHaendlerAdressenAdresse(string haendlerAdressenArt, string id)
        {
            if (id.IsNullOrEmpty())
                return new Adresse { Name1 = Localize.DropdownDefaultOptionPleaseChoose };

            AdressenDataInit(haendlerAdressenArt, LogonContext.KundenNr);
            return Adressen.FirstOrDefault(a => a.KundenNr.ToSapKunnr() == id.ToSapKunnr()) ?? new Adresse { Name1 = Localize.DropdownDefaultOptionPleaseChoose };
        }

        public override Adresse GetItem(int id)
        {
            return Adressen.FirstOrDefault(c => c.KundenNr.ToSapKunnr() == id.ToString().ToSapKunnr());
        }
    }
}
