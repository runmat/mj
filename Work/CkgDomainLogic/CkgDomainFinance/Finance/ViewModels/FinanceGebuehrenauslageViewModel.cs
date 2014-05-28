using System.Collections.Generic;
using System.Xml.Serialization;
using CkgDomainLogic.General.Services;
using CkgDomainLogic.General.ViewModels;
using CkgDomainLogic.Finance.Contracts;
using CkgDomainLogic.Finance.Models;
using GeneralTools.Models;

namespace CkgDomainLogic.Finance.ViewModels
{
    public class FinanceGebuehrenauslageViewModel : CkgBaseViewModel
    {
        [XmlIgnore]
        public IFinanceGebuehrenauslageDataService DataService { get { return CacheGet<IFinanceGebuehrenauslageDataService>(); } }

        [XmlIgnore]
        public List<Auftrag> Auftraege { get { return DataService.Auftraege; } }

        public void LoadAuftraege(AuftragSuchparameter suchparameter)
        {
            DataService.Suchparameter = suchparameter;
            DataService.MarkForRefreshAuftraege();
            PropertyCacheClear(this, m => m.AuftraegeFiltered);
        }

        public Auftrag GetAuftrag(string auftragsnr)
        {
            return Auftraege.Find(a => a.Auftragsnummer == auftragsnr);
        }

        public void SaveAuftragChanges(Auftrag auftr)
        {
            var au = Auftraege.Find(a => a.Auftragsnummer == auftr.Auftragsnummer);
            au.BemerkungZeile1 = auftr.BemerkungZeile1;
            au.BemerkungZeile2 = auftr.BemerkungZeile2;
            au.Erledigt = auftr.Erledigt;
            DataService.SaveAuftrag(au);
        }

        #region Filter

        [XmlIgnore]
        public List<Auftrag> AuftraegeFiltered
        {
            get { return PropertyCacheGet(() => Auftraege); }
            private set { PropertyCacheSet(value); }
        }

        public void FilterAuftraege(string filterValue, string filterProperties)
        {
            AuftraegeFiltered = Auftraege.SearchPropertiesWithOrCondition(filterValue, filterProperties);
        }

        #endregion
    }
}
