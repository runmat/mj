// ReSharper disable RedundantUsingDirective
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Xml.Serialization;
using CkgDomainLogic.Autohaus.Contracts;
using CkgDomainLogic.DomainCommon.Contracts;
using CkgDomainLogic.DomainCommon.Models;
using CkgDomainLogic.DomainCommon.ViewModels;
using CkgDomainLogic.General.Models;
using CkgDomainLogic.General.Services;
using CkgDomainLogic.General.ViewModels;
using System.Web.Mvc;
using CkgDomainLogic.Fahrzeugbestand.Contracts;
using CkgDomainLogic.Fahrzeugbestand.Models;
using CkgDomainLogic.Fahrzeugbestand.Services;
using CkgDomainLogic.Partner.Contracts;
using CkgDomainLogic.Partner.Models;
using GeneralTools.Models;
using System.IO;
using GeneralTools.Resources;
using GeneralTools.Services;
using SapORM.Contracts;

namespace CkgDomainLogic.Partner.ViewModels
{
    public class PartnerViewModel : AdressenPflegeViewModel
    {
        [XmlIgnore]
        public IPartnerDataService DataService { get { return CacheGet<IPartnerDataService>(); } }

        [XmlIgnore]
        public override IAdressenDataService AdressenDataService { get { return DataService; } }

        public IHtmlString AdressenKennungLocalized { get { return new HtmlString(AdressenKennung == "HALTER" ? Localize.Holder : Localize.Buyer); } }


        public PartnerSelektor PartnerSelektor
        {
            get { return PropertyCacheGet(() => new PartnerSelektor()); }
            set { PropertyCacheSet(value); }
        }

        [XmlIgnore]
        public List<Adresse> Partners
        {
            get { return Adressen; }
        }

        [XmlIgnore]
        public List<Adresse> PartnersFiltered
        {
            get { return PropertyCacheGet(() => Partners); }
            private set { PropertyCacheSet(value); }
        }


        public void DataInit()
        {
            DataMarkForRefresh();
        }

        public override void DataMarkForRefresh()
        {
            base.DataMarkForRefresh();

            PropertyCacheClear(this, m => m.PartnersFiltered);
        }

        public void ValidateSearch(Action<string, string> addModelError)
        {
        }

        public void LoadPartners()
        {
            AdressenDataInit(PartnerSelektor.PartnerKennung, KundennrOverride);
        }

        public void FilterPartners(string filterValue, string filterProperties)
        {
            PartnersFiltered = Partners.SearchPropertiesWithOrCondition(filterValue, filterProperties);
        }

        public Adresse GetPartnerAdresse(string partnerArt, string id)
        {
            if (id.IsNullOrEmpty())
                return new Adresse { Name1 = Localize.DropdownDefaultOptionPleaseChoose };

            AdressenDataInit(partnerArt, LogonContext.KundenNr);
            return Adressen.FirstOrDefault(a => a.KundenNr.ToSapKunnr() == id.ToSapKunnr()) ?? new Adresse { Name1 = Localize.DropdownDefaultOptionPleaseChoose };
        }

        public override Adresse GetItem(int id)
        {
            return Adressen.FirstOrDefault(c => c.KundenNr.ToSapKunnr() == id.ToString().ToSapKunnr());
        }
    }
}
