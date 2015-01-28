using System;
using System.Collections.Generic;
using System.Web.Script.Serialization;
using System.Xml.Serialization;
using CkgDomainLogic.General.ViewModels;
using CkgDomainLogic.Autohaus.Contracts;
using CkgDomainLogic.Autohaus.Models;
using GeneralTools.Models;

namespace CkgDomainLogic.Autohaus.ViewModels
{
    public class FormulareViewModel : CkgBaseViewModel
    {
        [XmlIgnore, ScriptIgnore]
        public IZulassungDataService DataService { get { return CacheGet<IZulassungDataService>(); } }


        public FormulareSelektor Selektor
        {
            get { return PropertyCacheGet(() => new FormulareSelektor()); }
            set { PropertyCacheSet(value); }
        }

        [XmlIgnore]
        public List<PdfFormular> Formulare
        {
            get { return PropertyCacheGet(() => new List<PdfFormular>()); }
            private set { PropertyCacheSet(value); }
        }

        [XmlIgnore]
        public List<PdfFormular> FormulareFiltered
        {
            get { return PropertyCacheGet(() => Formulare); }
            private set { PropertyCacheSet(value); }
        }

        [XmlIgnore, ScriptIgnore]
        public List<Zulassungskreis> Zulassungskreise { get { return DataService.Zulassungskreise; } }


        public void DataInit()
        {
            DataMarkForRefresh();
        }

        public void DataMarkForRefresh()
        {
            PropertyCacheClear(this, m => m.FormulareFiltered);
        }

        public void LoadFormulare(Action<string, string> addModelError)
        {
            Formulare = DataService.GetFormulare(Selektor, addModelError);

            DataMarkForRefresh();
        }

        public void FilterFormulare(string filterValue, string filterProperties)
        {
            FormulareFiltered = Formulare.SearchPropertiesWithOrCondition(filterValue, filterProperties);
        }
    }
}
