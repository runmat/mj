using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using CkgDomainLogic.General.Services;
using CkgDomainLogic.Remarketing.Contracts;
using CkgDomainLogic.Remarketing.Models;
using CkgDomainLogic.General.ViewModels;
using GeneralTools.Models;

namespace CkgDomainLogic.Remarketing.ViewModels
{
    public class GemeldeteVorschaedenViewModel : CkgBaseViewModel
    {
        [XmlIgnore]
        public IGemeldeteVorschaedenDataService DataService { get { return CacheGet<IGemeldeteVorschaedenDataService>(); } }

        public GemeldeteVorschaedenSelektor Selektor
        {
            get { return PropertyCacheGet(() => new GemeldeteVorschaedenSelektor()); }
            set { PropertyCacheSet(value); }
        }

        [XmlIgnore]
        public List<Vermieter> Vermieter
        {
            get { return PropertyCacheGet(() => DataService.GetVermieter()); }
        }

        [XmlIgnore]
        public List<Schadensmeldung> GemeldeteVorschaeden
        {
            get { return PropertyCacheGet(() => new List<Schadensmeldung>()); }
            protected set { PropertyCacheSet(value); }
        }

        [XmlIgnore]
        public List<Schadensmeldung> GemeldeteVorschaedenFiltered
        {
            get { return PropertyCacheGet(() => GemeldeteVorschaeden); }
            protected set { PropertyCacheSet(value); }
        }

        public bool IsAv
        {
            get { return LogonContext.GroupName.NotNullOrEmpty().StartsWith("AV"); }
        }

        public void DataInit()
        {
            DataMarkForRefresh();
        }

        private void DataMarkForRefresh()
        {
            PropertyCacheClear(this, m => m.GemeldeteVorschaedenFiltered);
        }

        public void LoadGemeldeteVorschaeden(GemeldeteVorschaedenSelektor selektor, Action<string, string> addModelError)
        {
            if (IsAv)
                selektor.Vermieter = LogonContext.GroupName;

            Selektor = selektor;

            GemeldeteVorschaeden = DataService.GetGemeldeteVorschaeden(Selektor);

            DataMarkForRefresh();

            if (GemeldeteVorschaeden.None())
                addModelError("", Localize.NoDataFound);
        }

        public EditVorschadenModel GetEditVorschadenModel(string fahrgestellNr, string kennzeichen, string laufendeNr)
        {
            var item = GemeldeteVorschaeden.FirstOrDefault(v => v.FahrgestellNr == fahrgestellNr && v.Kennzeichen == kennzeichen && v.LaufendeNr == laufendeNr);

            return (item != null ? new EditVorschadenModel(item) : new EditVorschadenModel());
        }

        public void UpdateVorschaden(EditVorschadenModel model, Action<string, string> addModelError)
        {
            var ergMessage = DataService.UpdateVorschaden(model);

            if (!string.IsNullOrEmpty(ergMessage))
            {
                addModelError("", ergMessage);
                return;
            }

            var item = GemeldeteVorschaeden.FirstOrDefault(v => v.FahrgestellNr == model.FahrgestellNr && v.Kennzeichen == model.Kennzeichen && v.LaufendeNr == model.LaufendeNr);
            if (item != null)
            {
                item.Beschreibung = model.Beschreibung;
                item.Schadensbetrag = model.Schadensbetrag;
                item.Schadensdatum = model.Schadensdatum;
            }

            DataMarkForRefresh();
        }

        public void FilterGemeldeteVorschaeden(string filterValue, string filterProperties)
        {
            GemeldeteVorschaedenFiltered = GemeldeteVorschaeden.SearchPropertiesWithOrCondition(filterValue, filterProperties);
        }
    }
}
