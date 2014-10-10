using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Xml.Serialization;
using CkgDomainLogic.General.Services;
using CkgDomainLogic.General.ViewModels;
using CkgDomainLogic.Finance.Contracts;
using CkgDomainLogic.Finance.Models;
using GeneralTools.Models;

namespace CkgDomainLogic.Finance.ViewModels
{
    public class FinanceMahnstopViewModel : CkgBaseViewModel
    {
        [XmlIgnore]
        public IFinanceMahnstopDataService DataService { get { return CacheGet<IFinanceMahnstopDataService>(); } }

        [XmlIgnore]
        public List<Mahnstop> Mahnstops { get { return DataService.Mahnstops; } }

        [XmlIgnore]
        public List<Mahnstop> EditierteMahnstops { get { return Mahnstops.FindAll(m => m.IsEdited); } }

        public bool EditMode { get; set; }

        [XmlIgnore]
        public List<Mahnstop> GridItems
        {
            get
            {
                if (EditMode)
                    return MahnstopsFiltered;

                return EditierteMahnstops;
            }
        }

        public void LoadMahnstops(MahnstopSuchparameter suchparameter, ModelStateDictionary state)
        {
            EditMode = true;
            DataService.Suchparameter = suchparameter;
            DataService.MarkForRefreshMahnstops();
            PropertyCacheClear(this, m => m.MahnstopsFiltered);

            if (Mahnstops.Count == 0)
                state.AddModelError("", Localize.NoDataFound);
        }

        public MahnstopEdit GetMahnstopEditModel(string paid, string equinr, string matnr)
        {
            var mst = Mahnstops.Find(m => m.PAID.NotNullOrEmpty() == paid.NotNullOrEmpty() 
                                        && m.EquiNr.NotNullOrEmpty() == equinr.NotNullOrEmpty() 
                                        && m.MaterialNr.NotNullOrEmpty() == matnr.NotNullOrEmpty());
            return new MahnstopEdit
                {
                    PAID = mst.PAID, 
                    EquiNr = mst.EquiNr, 
                    MaterialNr = mst.MaterialNr, 
                    Mahnsperre = mst.Mahnsperre, 
                    MahnstopBis = mst.MahnstopBis, 
                    Bemerkung = mst.Bemerkung
                };
        }

        public void EditMahnstop(MahnstopEdit me, ModelStateDictionary state)
        {
            var mst = Mahnstops.Find(m => m.PAID.NotNullOrEmpty() == me.PAID.NotNullOrEmpty() 
                                        && m.EquiNr.NotNullOrEmpty() == me.EquiNr.NotNullOrEmpty() 
                                        && m.MaterialNr.NotNullOrEmpty() == me.MaterialNr.NotNullOrEmpty());

            // Mahnsperre und MahnstopBis müssen innerhalb einer PAID gleich sein/bleiben
            var betrifftGesamtePaid = (mst.Mahnsperre != me.Mahnsperre || mst.MahnstopBis != me.MahnstopBis);

            mst.Mahnsperre = me.Mahnsperre;
            mst.MahnstopBis = me.MahnstopBis;
            mst.Bemerkung = me.Bemerkung;
            mst.IsEdited = true;

            if (betrifftGesamtePaid)
            {
                var msts = Mahnstops.FindAll(m => m.PAID.NotNullOrEmpty() == me.PAID.NotNullOrEmpty());

                foreach (Mahnstop m in msts)
                {
                    m.Mahnsperre = me.Mahnsperre;
                    m.MahnstopBis = me.MahnstopBis;
                    m.IsEdited = true;
                }
            }
        }

        public void SaveMahnstops(ModelStateDictionary state)
        {
            if ((EditierteMahnstops != null) && (EditierteMahnstops.Count > 0))
            {
                if (!CheckMahnstops())
                {
                    state.AddModelError("", Localize.DunningBlockAndDunningStopDateMustBeEqualPerPAID);
                    return;
                }

                EditMode = false;
                var message = DataService.SaveMahnstops(EditierteMahnstops);
                if (!String.IsNullOrEmpty(message))
                {
                    state.AddModelError("", message);
                }
            }
            else
            {
                state.AddModelError("", Localize.NoDataChanged);
            }
        }

        private bool CheckMahnstops()
        {
            foreach (Mahnstop mst in EditierteMahnstops)
            {
                // Zur einer PAID dürfen keine unterschiedlichen Sperrdaten existieren
                if (Mahnstops.Any(m => m.PAID.NotNullOrEmpty() == mst.PAID.NotNullOrEmpty() 
                                    && (m.Mahnsperre != mst.Mahnsperre || m.MahnstopBis != mst.MahnstopBis)))
                    return false;
            }

            return true;
        }

        #region Filter

        [XmlIgnore]
        public List<Mahnstop> MahnstopsFiltered
        {
            get { return PropertyCacheGet(() => Mahnstops); }
            private set { PropertyCacheSet(value); }
        }

        public void FilterMahnstops(string filterValue, string filterProperties)
        {
            MahnstopsFiltered = Mahnstops.SearchPropertiesWithOrCondition(filterValue, filterProperties);
        }

        #endregion
    }
}
