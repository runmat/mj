// ReSharper disable RedundantUsingDirective
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Xml.Serialization;
using CkgDomainLogic.General.Models;
using CkgDomainLogic.General.ViewModels;
using System.Web.Mvc;
using CkgDomainLogic.WFM.Contracts;
using CkgDomainLogic.WFM.Models;
using GeneralTools.Models;
using System.IO;
using GeneralTools.Services;

namespace CkgDomainLogic.WFM.ViewModels
{
    public class WfmViewModel : CkgBaseViewModel
    {
        [XmlIgnore]
        public IWfmDataService DataService { get { return CacheGet<IWfmDataService>(); } }

        public WfmAbmeldungSelektor WfmAbmeldungSelektor
        {
            get { return PropertyCacheGet(() => new WfmAbmeldungSelektor()); }
            set { PropertyCacheSet(value); }
        }

        [XmlIgnore]
        public List<WfmAbmeldung> WfmAbmeldungen
        {
            get { return PropertyCacheGet(() => new List<WfmAbmeldung>()); }
            private set { PropertyCacheSet(value); }
        }

        [XmlIgnore]
        public List<WfmAbmeldung> WfmAbmeldungenFiltered
        {
            get { return PropertyCacheGet(() => WfmAbmeldungen); }
            private set { PropertyCacheSet(value); }
        }

        public void DataInit()
        {
            DataMarkForRefresh();
        }

        public void DataMarkForRefresh()
        {
            PropertyCacheClear(this, m => m.WfmAbmeldungenFiltered);
        }

        public void Validate(Action<Expression<Func<WfmAbmeldungSelektor, object>>, string> addModelError)
        {
        }

        public void LoadWflAbmeldungen()
        {
            WfmAbmeldungen = DataService.GetAbmeldungen(WfmAbmeldungSelektor);

            DataMarkForRefresh();

            //XmlService.XmlSerializeToFile(Wfl, Path.Combine(AppSettings.DataPath, @"Wfl.xml"));
        }

        public void FilterWflAbmeldungen(string filterValue, string filterProperties)
        {
            WfmAbmeldungenFiltered = WfmAbmeldungen.SearchPropertiesWithOrCondition(filterValue, filterProperties);
        }
    }
}
