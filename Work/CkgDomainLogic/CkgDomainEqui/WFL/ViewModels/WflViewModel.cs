// ReSharper disable RedundantUsingDirective
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Xml.Serialization;
using CkgDomainLogic.General.Models;
using CkgDomainLogic.General.ViewModels;
using System.Web.Mvc;
using CkgDomainLogic.WFL.Contracts;
using CkgDomainLogic.WFL.Models;
using GeneralTools.Models;
using System.IO;
using GeneralTools.Services;

namespace CkgDomainLogic.Wfl.ViewModels
{
    public class WflViewModel : CkgBaseViewModel
    {
        [XmlIgnore]
        public IWflDataService DataService { get { return CacheGet<IWflDataService>(); } }

        public WflAbmeldungSelektor WflAbmeldungSelektor
        {
            get { return PropertyCacheGet(() => new WflAbmeldungSelektor()); }
            set { PropertyCacheSet(value); }
        }

        [XmlIgnore]
        public List<WflAbmeldung> WflAbmeldungen
        {
            get { return PropertyCacheGet(() => new List<WflAbmeldung>()); }
            private set { PropertyCacheSet(value); }
        }

        [XmlIgnore]
        public List<WflAbmeldung> WflAbmeldungenFiltered
        {
            get { return PropertyCacheGet(() => WflAbmeldungen); }
            private set { PropertyCacheSet(value); }
        }

        public void DataInit()
        {
            DataMarkForRefresh();
        }

        public void DataMarkForRefresh()
        {
            PropertyCacheClear(this, m => m.WflAbmeldungenFiltered);
        }

        public void Validate(Action<Expression<Func<WflAbmeldungSelektor, object>>, string> addModelError)
        {
        }

        public void LoadWflAbmeldungen()
        {
            WflAbmeldungen = DataService.GetAbmeldungen(WflAbmeldungSelektor);

            DataMarkForRefresh();

            //XmlService.XmlSerializeToFile(Wfl, Path.Combine(AppSettings.DataPath, @"Wfl.xml"));
        }

        public void FilterWflAbmeldungen(string filterValue, string filterProperties)
        {
            WflAbmeldungenFiltered = WflAbmeldungen.SearchPropertiesWithOrCondition(filterValue, filterProperties);
        }
    }
}
