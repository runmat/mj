using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Xml.Serialization;
using CkgDomainLogic.General.Services;
using CkgDomainLogic.General.ViewModels;
using CkgDomainLogic.Leasing.Contracts;
using CkgDomainLogic.Leasing.Models;
using CkgDomainLogic.Leasing.Models.UIModels;
using DocumentTools.Services;
using GeneralTools.Models;
using MvcTools.Web;

namespace CkgDomainLogic.Leasing.ViewModels
{
    public class LeasingEndgueltigerVersandViewModel : CkgBaseViewModel
    {
        [XmlIgnore]
        public ILeasingEndgueltigerVersandDataService DataService
        {
            get { return CacheGet<ILeasingEndgueltigerVersandDataService>(); }
        }

        [XmlIgnore]
        public List<EndgueltigerVersandModel> EndgueltigerVersandInfos
        {
            get { return PropertyCacheGet(() => new List<EndgueltigerVersandModel>()); }
            private set { PropertyCacheSet(value); }
        }


        [XmlIgnore]
        public List<EndgueltigerVersandModel> EndgueltigerVersandInfosFiltered
        {
            get { return PropertyCacheGet(() => EndgueltigerVersandInfos); }
            private set { PropertyCacheSet(value); }
        }

        public void FilterEndgueltigerVersandInfos(string filterValue, string filterProperties)
        {
            EndgueltigerVersandInfosFiltered = EndgueltigerVersandInfos.SearchPropertiesWithOrCondition(filterValue, filterProperties);
        }

        public EndgueltigerVersandSuchParameter EndgueltigerVersandSelektor
        {
            get
            {
                return PropertyCacheGet(() => new EndgueltigerVersandSuchParameter());
            }
            set { PropertyCacheSet(value); }
        }

        public void GetTemporaryMarkedVersand()
        {
            EndgueltigerVersandInfos = DataService.GetTempVersandInfos(EndgueltigerVersandSelektor);

            // wenn nur 1 Datensatz (über FIN bspw.) vorhanden, direkt selektieren.
            if (EndgueltigerVersandInfos.Count == 1)
                EndgueltigerVersandInfos.FirstOrDefault().IsSelected = true;

            DataMarkForRefresh();
        }

        public void DataMarkForRefresh()
        {
            PropertyCacheClear(this, m => m.EndgueltigerVersandInfosFiltered);
        }

        public void SelectVersandDatensaetze(bool select, Predicate<EndgueltigerVersandModel> filter, out int allSelectionCount, out int allCount)
        {
            EndgueltigerVersandInfosFiltered.Where(f => filter(f)).ToListOrEmptyList().ForEach(f => f.IsSelected = select);
            allSelectionCount = EndgueltigerVersandInfos.Count(x => x.IsSelected);
            allCount = EndgueltigerVersandInfosFiltered.Count;
        }

        public void SelectVersandDatensatz(string finId, bool select, out int allSelectionCount)
        {
            allSelectionCount = 0;
            var evi = EndgueltigerVersandInfos.FirstOrDefault(f => f.EQUNR == finId);
            if (evi == null)
                return;

            evi.IsSelected = select;
            allSelectionCount = EndgueltigerVersandInfos.Count(c => c.IsSelected);
        }

        public void VersendeSelektierteEndgueltig()
        {

            var test = EndgueltigerVersandInfos.Where(x => x.IsSelected).ToList();
            DataMarkForRefresh();
            
            DataService.Save(test);
            EndgueltigerVersandInfos = DataService.GetTempVersandInfos(EndgueltigerVersandSelektor);
        }
    }


}
