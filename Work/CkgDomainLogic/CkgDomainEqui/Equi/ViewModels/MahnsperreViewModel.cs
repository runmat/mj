using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Xml.Serialization;
using CkgDomainLogic.General.Services;
using CkgDomainLogic.General.ViewModels;
using CkgDomainLogic.Equi.Contracts;
using CkgDomainLogic.Equi.Models;
using GeneralTools.Models;
using MvcTools.Web;
using System.Linq;

namespace CkgDomainLogic.Equi.ViewModels
{
    public class MahnsperreViewModel : CkgBaseViewModel
    {
        [XmlIgnore]
        public IMahnsperreDataService DataService { get { return CacheGet<IMahnsperreDataService>(); } }

        [XmlIgnore]
        public List<EquiMahnsperre> MahnsperreEquis { get { return DataService.MahnsperreEquis; } }

        [XmlIgnore]
        public List<EquiMahnsperre> SelektierteMahnsperreEquis
        {
            get { return PropertyCacheGet(() => MahnsperreEquis); }
            private set { PropertyCacheSet(value); }
        }

        public bool EditMode { get; set; }

        public bool MahnsperreVorhanden { get { return MahnsperreEquis.Any(e => e.Mahnsperre || e.MahnsperreBis.HasValue); } }

        [XmlIgnore]
        public List<EquiMahnsperre> GridItems
        {
            get
            {
                if (EditMode)
                {
                    return MahnsperreEquisFiltered;
                }
                else
                {
                    return SelektierteMahnsperreEquis;
                }
            }
        }

        public void LoadMahnsperreEquis(MahnsperreSuchparameter suchparameter, ModelStateDictionary state)
        {
            DataService.Suchparameter.FahrgestellNr = suchparameter.FahrgestellNr;
            DataService.Suchparameter.VertragsNr = suchparameter.VertragsNr;
            DataService.Suchparameter.Kennzeichen = suchparameter.Kennzeichen;
            DataService.Suchparameter.BriefNr = suchparameter.BriefNr;
            EditMode = true;
            DataService.MarkForRefreshMahnsperreEquis();
            PropertyCacheClear(this, m => m.MahnsperreEquisFiltered);

            if (MahnsperreEquis.Count == 0)
            {
                state.AddModelError("", Localize.NoDataFound);
            }
        }

        public void SelectMahnsperreEquis(string selectedItems, ModelStateDictionary state)
        {
            var liste = JSon.Deserialize<string[]>(selectedItems);
            foreach (var me in MahnsperreEquis)
            {
                me.IsSelected = liste.Contains(me.FahrgestellNr + "|" + me.KomponentenID + "|" + me.StuecklistenPosKnotenNr);
            }
            SelektierteMahnsperreEquis = MahnsperreEquis.FindAll(e => e.IsSelected);

            if ((SelektierteMahnsperreEquis == null) || (SelektierteMahnsperreEquis.Count == 0))
            {
                state.AddModelError("", Localize.NoDataSelected);
            }
        }

        public MahnsperreEdit GetMahnsperreCreateModel()
        {
            var item = GetEditModel();
            item.Modus = MahnsperreEditMode.Create;
            return item;
        }

        public MahnsperreEdit GetMahnsperreEditModel()
        {
            var item = GetEditModel();
            item.Modus = MahnsperreEditMode.Edit;
            return item;
        }

        private MahnsperreEdit GetEditModel()
        {
            var item = new MahnsperreEdit();

            if (SelektierteMahnsperreEquis.Any(e => e.Mahnsperre))
            {
                item.Mahnsperre = true;
            }

            if (SelektierteMahnsperreEquis.Any(e => e.MahnsperreBis.HasValue))
            {
                item.MahnsperreBis = SelektierteMahnsperreEquis.First(e => e.MahnsperreBis.HasValue).MahnsperreBis.Value;
            }

            return item;
        }

        public void EditOrCreateMahnsperre(MahnsperreEdit daten, ModelStateDictionary state)
        {
            EditMode = false;

            foreach (var me in SelektierteMahnsperreEquis)
            {
                me.Mahnsperre = (daten.Modus == MahnsperreEditMode.Create || daten.Mahnsperre);
                me.MahnsperreBis = daten.MahnsperreBis;
            }

            SaveMahnsperreEquis(state);
        }

        public void DeleteMahnsperre(ModelStateDictionary state)
        {
            EditMode = false;

            foreach (var me in SelektierteMahnsperreEquis)
            {
                me.Mahnsperre = false;
                me.MahnsperreBis = null;
            }

            SaveMahnsperreEquis(state);
        }

        private void SaveMahnsperreEquis(ModelStateDictionary state)
        {
            var message = "";
            SelektierteMahnsperreEquis = DataService.SaveMahnsperreEquis(SelektierteMahnsperreEquis, ref message);
            if (!String.IsNullOrEmpty(message))
            {
                state.AddModelError("", message);
            }
        }

        public List<string> GetDatensatzIds()
        {
            var items = from e in MahnsperreEquisFiltered
                        select (e.FahrgestellNr + "|" + e.KomponentenID + "|" + e.StuecklistenPosKnotenNr);
            return items.ToList();
        }

        #region Filter

        [XmlIgnore]
        public List<EquiMahnsperre> MahnsperreEquisFiltered
        {
            get { return PropertyCacheGet(() => MahnsperreEquis); }
            private set { PropertyCacheSet(value); }
        }

        public void FilterMahnsperreEquis(string filterValue, string filterProperties)
        {
            MahnsperreEquisFiltered = MahnsperreEquis.SearchPropertiesWithOrCondition(filterValue, filterProperties);
        }

        #endregion
    }
}
