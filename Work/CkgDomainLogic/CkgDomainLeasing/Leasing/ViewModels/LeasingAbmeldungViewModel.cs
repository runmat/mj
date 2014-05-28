using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Xml.Serialization;
using CkgDomainLogic.General.Services;
using CkgDomainLogic.General.ViewModels;
using CkgDomainLogic.Leasing.Contracts;
using CkgDomainLogic.Leasing.Models;
using GeneralTools.Models;
using MvcTools.Web;

namespace CkgDomainLogic.Leasing.ViewModels
{
    public class LeasingAbmeldungViewModel : CkgBaseViewModel
    {
        [XmlIgnore]
        public ILeasingAbmeldungDataService DataService { get { return CacheGet<ILeasingAbmeldungDataService>(); } }

        [XmlIgnore]
        public List<Abmeldedaten> AbzumeldendeFzge { get { return DataService.AbzumeldendeFzge; } }

        [XmlIgnore]
        public List<Abmeldedaten> AbzumeldendeFzgeToSubmit
        {
            get { return PropertyCacheGet(() => AbzumeldendeFzge); }
            private set { PropertyCacheSet(value); }
        }

        public bool SubmitMode { get; set; }

        [XmlIgnore]
        public List<Abmeldedaten> GridItems
        {
            get
            {
                if (SubmitMode)
                {
                    return AbzumeldendeFzgeToSubmit;
                }
                else
                {
                    return AbzumeldendeFzgeFiltered;
                }
            }
        }

        public void LoadAbzumeldendeFzge(ModelStateDictionary state)
        {
            DataService.MarkForRefreshAbzumeldendeFzge();
            PropertyCacheClear(this, m => m.AbzumeldendeFzgeFiltered);

            if (AbzumeldendeFzge.Count == 0)
            {
                state.AddModelError("", Localize.NoDataFound);
            }
        }

        public void SwitchToSubmitMode(string selectedItems)
        {
            var liste = JSon.Deserialize<string[]>(selectedItems);
            foreach (var fzg in AbzumeldendeFzge)
            {
                fzg.Freigabe = liste.Contains(fzg.Fahrgestellnummer);
            }
            AbzumeldendeFzgeToSubmit = AbzumeldendeFzge.FindAll(f => f.Freigabe);
            SubmitMode = true;
        }

        public void SaveChangesToSap(ModelStateDictionary state)
        {
            if ((AbzumeldendeFzgeToSubmit != null) && (AbzumeldendeFzgeToSubmit.Count > 0))
            {
                AbzumeldendeFzgeToSubmit = DataService.SaveAbmeldungen(AbzumeldendeFzgeToSubmit);
            }
            else
            {
                state.AddModelError("", Localize.NoDataChanged);
            }
        }

        public void ResetSubmitState()
        {
            AbzumeldendeFzgeToSubmit.Clear();
            SubmitMode = false;
        }

        public List<string> GetFahrgestellnummern()
        {
            var items = from f in AbzumeldendeFzge
                        select f.Fahrgestellnummer;
            return items.ToList();
        } 

        #region Filter

        public string FilterKennzeichen { get; set; }

        public string FilterLeasingvertragsnummer { get; set; }

        public string FilterBriefnummer { get; set; }

        public string FilterFahrgestellnummer { get; set; }

        [XmlIgnore]
        public List<Abmeldedaten> AbzumeldendeFzgeFiltered
        {
            get { return PropertyCacheGet(() => AbzumeldendeFzge); }
            private set { PropertyCacheSet(value); }
        }

        public void FilterAbzumeldendeFzge(string filterValue, string filterProperties)
        {
            AbzumeldendeFzgeFiltered = AbzumeldendeFzge.SearchPropertiesWithOrCondition(filterValue, filterProperties);
        }

        #endregion
    }
}
