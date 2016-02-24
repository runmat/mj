using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;
using CkgDomainLogic.DomainCommon.Contracts;
using CkgDomainLogic.DomainCommon.Models;
using CkgDomainLogic.DomainCommon.ViewModels;
using CkgDomainLogic.General.Services;
using CkgDomainLogic.Partner.Contracts;
using CkgDomainLogic.Partner.Models;
using GeneralTools.Models;
using GeneralTools.Services;
using SapORM.Contracts;
using WebTools.Services;

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
            get { return PropertyCacheGet(() => new PartnerSelektor { PartnerKennung = "HALTER" }); }
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


        public void DataInit(string partnerId)
        {
            DataMarkForRefresh();

            if (!String.IsNullOrEmpty(partnerId))
                PartnerSelektor.Kunnr = CryptoMd5.Decrypt(partnerId).ToSapKunnr();
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
            AdressenDataInit(PartnerSelektor.PartnerKennung, KundennrOverride, PartnerSelektor.Kunnr);
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
