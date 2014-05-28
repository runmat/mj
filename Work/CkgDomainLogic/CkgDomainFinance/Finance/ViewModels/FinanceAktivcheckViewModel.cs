using System.Collections.Generic;
using System.Xml.Serialization;
using CkgDomainLogic.DomainCommon.Models;
using CkgDomainLogic.General.Services;
using CkgDomainLogic.General.ViewModels;
using CkgDomainLogic.Finance.Contracts;
using CkgDomainLogic.Finance.Models;
using GeneralTools.Models;

namespace CkgDomainLogic.Finance.ViewModels
{
    public class FinanceAktivcheckViewModel : CkgBaseViewModel
    {
        [XmlIgnore]
        public IFinanceAktivcheckDataService DataService { get { return CacheGet<IFinanceAktivcheckDataService>(); } }

        [XmlIgnore]
        public List<AktivcheckTreffer> Treffer { get { return DataService.Treffer; } }

        [XmlIgnore]
        public List<Domaenenfestwert> Klassifizierungen { get { return DataService.Klassifizierungen; } }

        public void LoadTreffer()
        {
            DataService.MarkForRefreshTreffer();
            PropertyCacheClear(this, m => m.TrefferFiltered);
        }

        public AktivcheckTreffer GetVorgang(string vorgangId)
        {
            return Treffer.Find(v => v.VorgangsID == vorgangId);
        }

        public void SaveChanges(AktivcheckTreffer vorgang)
        {
            var vg = Treffer.Find(v => v.VorgangsID == vorgang.VorgangsID);
            vg.Klassifizierung = vorgang.Klassifizierung;
            var kl = Klassifizierungen.Find(k => k.Wert == vorgang.Klassifizierung);
            if (kl != null)
            {
                vg.Klassifizierungstext = kl.Beschreibung;
            }
            vg.Bemerkung = vorgang.Bemerkung;
            DataService.SaveVorgang(vg);
        }

        public bool SendRequestMail(string vorgangId)
        {
            var vg = Treffer.Find(v => v.VorgangsID == vorgangId);
            if (DataService.SendRequestMail(vg))
            {
                vg.Kontaktanfrage = true;
                DataService.SaveVorgang(vg);

                return true;
            }

            return false;
        }

        #region Filter

        [XmlIgnore]
        public List<AktivcheckTreffer> TrefferFiltered
        {
            get { return PropertyCacheGet(() => Treffer); }
            private set { PropertyCacheSet(value); }
        }

        public void FilterTreffer(string filterValue, string filterProperties)
        {
            TrefferFiltered = Treffer.SearchPropertiesWithOrCondition(filterValue, filterProperties);
        }

        #endregion
    }
}
