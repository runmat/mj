using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Xml.Serialization;
using CkgDomainLogic.General.Services;
using CkgDomainLogic.General.ViewModels;
using CkgDomainLogic.WFM.Contracts;
using CkgDomainLogic.WFM.Models;
using GeneralTools.Models;

namespace CkgDomainLogic.WFM.ViewModels
{
    public class WfmViewModel : CkgBaseViewModel
    {
        [XmlIgnore]
        public IWfmDataService DataService { get { return CacheGet<IWfmDataService>(); } }

        public WfmAuftragSelektor Selektor
        {
            get { return PropertyCacheGet(() => new WfmAuftragSelektor()); }
            set { PropertyCacheSet(value); }
        }

        [XmlIgnore]
        public List<WfmAuftragFeldname> Feldnamen
        {
            get { return PropertyCacheGet(() => new List<WfmAuftragFeldname>()); }
            private set { PropertyCacheSet(value); }
        }

        [XmlIgnore]
        public List<WfmAuftrag> Auftraege
        {
            get { return PropertyCacheGet(() => new List<WfmAuftrag>()); }
            private set { PropertyCacheSet(value); }
        }

        [XmlIgnore]
        public List<WfmAuftrag> AuftraegeFiltered
        {
            get { return PropertyCacheGet(() => Auftraege); }
            private set { PropertyCacheSet(value); }
        }

        public void DataInit()
        {
            InitFeldnamen();
            DataMarkForRefresh();
        }

        public void DataMarkForRefresh()
        {
            PropertyCacheClear(this, m => m.AuftraegeFiltered);
        }

        private void InitFeldnamen()
        {
            PropertyCacheClear(this, m => m.Feldnamen);

            Feldnamen = DataService.GetFeldnamen();

            Selektor.Selektionsfeld1Name = (Feldnamen.Any(f => f.Feldname == "SELEKTION1") ? Feldnamen.First(f => f.Feldname == "SELEKTION1").Anzeigename : "");
            Selektor.Selektionsfeld2Name = (Feldnamen.Any(f => f.Feldname == "SELEKTION2") ? Feldnamen.First(f => f.Feldname == "SELEKTION2").Anzeigename : "");
            Selektor.Selektionsfeld3Name = (Feldnamen.Any(f => f.Feldname == "SELEKTION3") ? Feldnamen.First(f => f.Feldname == "SELEKTION3").Anzeigename : "");

            Selektor.Referenz1Name = (Feldnamen.Any(f => f.Feldname == "REFERENZ1") ? Feldnamen.First(f => f.Feldname == "REFERENZ1").Anzeigename : "");
            Selektor.Referenz2Name = (Feldnamen.Any(f => f.Feldname == "REFERENZ2") ? Feldnamen.First(f => f.Feldname == "REFERENZ2").Anzeigename : "");
            Selektor.Referenz3Name = (Feldnamen.Any(f => f.Feldname == "REFERENZ3") ? Feldnamen.First(f => f.Feldname == "REFERENZ3").Anzeigename : "");
        }

        public void LoadAuftraege(ModelStateDictionary state)
        {
            DataMarkForRefresh();

            Auftraege = DataService.GetAbmeldeauftraege(Selektor);

            if (Auftraege.None())
                state.AddModelError("", Localize.NoDataFound);
        }

        public void FilterAuftraege(string filterValue, string filterProperties)
        {
            AuftraegeFiltered = Auftraege.SearchPropertiesWithOrCondition(filterValue, filterProperties);
        }
    }
}
