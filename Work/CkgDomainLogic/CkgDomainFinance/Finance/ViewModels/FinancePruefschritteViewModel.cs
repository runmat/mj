using System.Collections.Generic;
using System.Xml.Serialization;
using CkgDomainLogic.General.ViewModels;
using CkgDomainLogic.Finance.Contracts;
using CkgDomainLogic.Finance.Models;
using GeneralTools.Models;
using GeneralTools.Resources;

namespace CkgDomainLogic.Finance.ViewModels
{
    public class FinancePruefschritteViewModel : CkgBaseViewModel
    {
        [XmlIgnore]
        public IFinancePruefschritteDataService DataService { get { return CacheGet<IFinancePruefschritteDataService>(); } }

        [XmlIgnore]
        public List<Pruefschritt> Pruefschritte { get { return DataService.Pruefschritte; } }

        [XmlIgnore]
        [LocalizedDisplay(LocalizeConstants.AccountNo)]
        public string Kontonummer { get { return (Pruefschritte != null && Pruefschritte.Count > 0 ? Pruefschritte[0].Kontonummer : ""); } }

        [XmlIgnore]
        [LocalizedDisplay(LocalizeConstants.PAID)]
        public string PAID { get { return (Pruefschritte != null && Pruefschritte.Count > 0 ? Pruefschritte[0].PAID : ""); } }

        public void LoadPruefschritte(PruefschrittSuchparameter suchparameter)
        {
            DataService.Suchparameter.Kontonummer = suchparameter.Kontonummer;
            DataService.Suchparameter.PAID = suchparameter.PAID;
            DataService.MarkForRefreshPruefschritte();
            PropertyCacheClear(this, m => m.PruefschritteFiltered);
        }

        public string PruefschrittErledigen(string aktionsnr, string bucid)
        {
            var schritt = Pruefschritte.Find(p => p.Kontonummer == Kontonummer && p.PAID == PAID && p.Aktionsnummer == aktionsnr && p.BucID == bucid);
            var erg = DataService.PruefschrittErledigen(schritt);
            if (erg == "OK")
            {
                schritt.Erledigt = true;
                schritt.Webuser = LogonContext.UserName;
            }
            return erg;
        }

        #region Filter

        [XmlIgnore]
        public List<Pruefschritt> PruefschritteFiltered
        {
            get { return PropertyCacheGet(() => Pruefschritte); }
            private set { PropertyCacheSet(value); }
        }

        public void FilterPruefschritte(string filterValue, string filterProperties)
        {
            PruefschritteFiltered = Pruefschritte.SearchPropertiesWithOrCondition(filterValue, filterProperties);
        }

        #endregion
    }
}
