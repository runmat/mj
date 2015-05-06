using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Xml.Serialization;
using CkgDomainLogic.General.Models;
using CkgDomainLogic.General.Services;
using CkgDomainLogic.General.ViewModels;
using System.Web.Mvc;
using CkgDomainLogic.Fahrzeuge.Contracts;
using CkgDomainLogic.Fahrzeuge.Models;
using GeneralTools.Models;
using System.IO;

namespace CkgDomainLogic.Fahrzeuge.ViewModels
{
    public class DispositionslisteViewModel : CkgBaseViewModel
    {

        [XmlIgnore]
        public IDispositionslisteDataService DataService { get { return CacheGet<IDispositionslisteDataService>(); } }

              
        public DispositionslisteSelektor DispositionslisteSelektor
        {
            get
            {
                return PropertyCacheGet(() => new DispositionslisteSelektor());                                                   
            }
            set { PropertyCacheSet(value); }
        }

        [XmlIgnore]
        public List<Dispositionsliste> Dispositionslistes
        {
            get { return PropertyCacheGet(() => new List<Dispositionsliste>()); }
            private set { PropertyCacheSet(value); }
        }

        [XmlIgnore]
        public List<Dispositionsliste> DispositionslistesFiltered
        {
            get { return PropertyCacheGet(() => Dispositionslistes); }
            private set { PropertyCacheSet(value); }
        }

        public void Init()
        {
            DispositionslisteSelektor.ZulassungsdatumRange.IsSelected = true;
        }

        public void DataInit()
        {          
            DataMarkForRefresh();
        }

        public void DataMarkForRefresh()
        {
            PropertyCacheClear(this, m => m.DispositionslistesFiltered);
        }

        public void Validate(Action<Expression<Func<DispositionslisteSelektor, object>>, string> addModelError)
        {           
        }

        public void LoadDispositionsliste()
        {            
            Dispositionslistes = DataService.GetDispositionsliste(DispositionslisteSelektor); 
            DataMarkForRefresh();

            //XmlService.XmlSerializeToFile(Dispositionslistes, Path.Combine(AppSettings.DataPath, @"Dispositionslistes.xml"));
        }

        public void FilterDispositionsliste(string filterValue, string filterProperties)
        {
            DispositionslistesFiltered = Dispositionslistes.SearchPropertiesWithOrCondition(filterValue, filterProperties);
        }

       



    }
}
