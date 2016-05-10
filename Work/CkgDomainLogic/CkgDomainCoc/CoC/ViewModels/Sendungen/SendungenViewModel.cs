// ReSharper disable RedundantUsingDirective
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using CkgDomainLogic.CoC.Contracts;
using CkgDomainLogic.CoC.Models;
using CkgDomainLogic.General.Services;
using CkgDomainLogic.General.ViewModels;
using GeneralTools.Models;
using GeneralTools.Services;

// ReSharper restore RedundantUsingDirective

namespace CkgDomainLogic.CoC.ViewModels
{
    public class SendungenViewModel : CkgBaseViewModel
    {
        [XmlIgnore]
        public IZulassungDataService DataService { get { return CacheGet<IZulassungDataService>(); } }

        public List<SelectItem> Standorte
        {
            get { return PropertyCacheGet(() => DataService.GetFahrzeugstandorte()); }
            set { PropertyCacheSet(value); }
        }


        public int CurrentAppID { get; set; }

        public bool ShowStatusInGrid { get; set; }

        public Func<IEnumerable> FilteredObjectsCurrent
        {
            get { return PropertyCacheGet(() => (Func<IEnumerable>)(() => SendungenFiltered)); }
            set { PropertyCacheSet(value); }
        }


        #region Sendungen

        public SendungsAuftragSelektor SendungsAuftragSelektor
        {
            get { return PropertyCacheGet(() => new SendungsAuftragSelektor()); }
            set { PropertyCacheSet(value); }
        }

        [XmlIgnore]
        public List<SendungsAuftrag> Sendungen
        {
            get { return PropertyCacheGet(() => new List<SendungsAuftrag>()); }
            private set { PropertyCacheSet(value); }
        }

        [XmlIgnore]
        public List<SendungsAuftrag> SendungenFiltered
        {
            get
            {
                FilteredObjectsCurrent = () => SendungenFiltered;
                return PropertyCacheGet(() => Sendungen);
            }
            private set { PropertyCacheSet(value); }
        }

        public void DataMarkForRefresh()
        {
            PropertyCacheClear(this, m => m.SendungenFiltered);
            PropertyCacheClear(this, m => m.SendungsAuftragSelektor);
        }

        public void LoadSendungen(SendungsAuftragSelektor model, Action<string, string> addModelError)
        {
            Sendungen = DataService.GetSendungsAuftraege(model);

            if (Sendungen.None())
                addModelError("", Localize.NoDataFound);

            DataMarkForRefresh();
        }

        public void FilterSendungen(string filterValue, string filterProperties)
        {
            SendungenFiltered = Sendungen.SearchPropertiesWithOrCondition(filterValue, filterProperties);
        }

        #endregion


        public void DataMarkForRefreshMulti()
        {
            GetCurrentAppID();
            var tmpWert = ApplicationConfiguration.GetApplicationConfigValue("SendungsstatusImGridAnzeigen", CurrentAppID.ToString(), LogonContext.Customer.CustomerID, LogonContext.Group.GroupID);
            ShowStatusInGrid = (!String.IsNullOrEmpty(tmpWert) && tmpWert.ToUpper() == "TRUE");

            PropertyCacheClear(this, m => m.SendungenIdFiltered);
            PropertyCacheClear(this, m => m.SendungsAuftragIdSelektor);
            
            PropertyCacheClear(this, m => m.SendungenDocsFiltered);
            PropertyCacheClear(this, m => m.SendungsAuftragDocsSelektor);

           // PropertyCacheClear(this, m => m.SendungenFinFiltered);
            PropertyCacheClear(this, m => m.SendungsAuftragFinSelektor);
        }


        #region Sendungen, Suche nach ID

        public SendungsAuftragIdSelektor SendungsAuftragIdSelektor
        {
            get { return PropertyCacheGet(() => new SendungsAuftragIdSelektor()); }
            set { PropertyCacheSet(value); }
        }

        [XmlIgnore]
        public List<SendungsAuftrag> SendungenId
        {
            get { return PropertyCacheGet(() => new List<SendungsAuftrag>()); }
            private set { PropertyCacheSet(value); }
        }

        [XmlIgnore]
        public List<SendungsAuftrag> SendungenIdFiltered
        {
            get
            {
                FilteredObjectsCurrent = () => SendungenIdFiltered;
                return PropertyCacheGet(() => SendungenId);
            }
            private set { PropertyCacheSet(value); }
        }

        public void LoadSendungenId(SendungsAuftragIdSelektor model, Action<string, string> addModelError)
        {
            PropertyCacheClear(this, m => m.SendungenIdFiltered);
            SendungenId = DataService.GetSendungsAuftraegeId(model);

            if (SendungenId.None())
                addModelError("", Localize.NoDataFound);
            else if (!ShowStatusInGrid)
                SendungenId.ForEach(x => x.StatusText = "");

            DataMarkForRefresh();
        }

        public void FilterSendungenId(string filterValue, string filterProperties)
        {
            SendungenIdFiltered = SendungenId.SearchPropertiesWithOrCondition(filterValue, filterProperties);
        }

        #endregion


        #region Sendungen, Suche nach Docs

        public SendungsAuftragDocsSelektor SendungsAuftragDocsSelektor
        {
            get { return PropertyCacheGet(() => new SendungsAuftragDocsSelektor()); }
            set { PropertyCacheSet(value); }
        }

        [XmlIgnore]
        public List<SendungsAuftrag> SendungenDocs
        {
            get { return PropertyCacheGet(() => new List<SendungsAuftrag>()); }
            private set { PropertyCacheSet(value); }
        }

        [XmlIgnore]
        public List<SendungsAuftrag> SendungenDocsFiltered
        {
            get
            {
                FilteredObjectsCurrent = () => SendungenDocsFiltered;
                return PropertyCacheGet(() => SendungenDocs);
            }
            private set { PropertyCacheSet(value); }
        }

        public void LoadSendungenDocs(SendungsAuftragDocsSelektor model, Action<string, string> addModelError)
        {
            PropertyCacheClear(this, m => m.SendungenDocsFiltered);
            SendungenDocs = DataService.GetSendungsAuftraegeDocs(model);

            if (SendungenDocs.None())
                addModelError("", Localize.NoDataFound);
            else if (!ShowStatusInGrid)
                SendungenDocs.ForEach(x => x.StatusText = "");

            DataMarkForRefresh();
        }
      
        public void FilterSendungenDocs(string filterValue, string filterProperties)
        {
            SendungenDocsFiltered = SendungenDocs.SearchPropertiesWithOrCondition(filterValue, filterProperties);
        }

        #endregion


        #region Sendungen, Suche nach Fin (ViewModel wie Docs)

        public SendungsAuftragFinSelektor SendungsAuftragFinSelektor
        {
            get { return PropertyCacheGet(() => new SendungsAuftragFinSelektor()); }
            set { PropertyCacheSet(value); }
        }

        public SendungsAuftragPlaceSelektor SendungsAuftragPlaceSelektor
        {
            get { return PropertyCacheGet(() => new SendungsAuftragPlaceSelektor()); }
            set { PropertyCacheSet(value); }
        }


        public void LoadSendungenFin(SendungsAuftragFinSelektor model, Action<string, string> addModelError)
        {
            PropertyCacheClear(this, m => m.SendungenDocsFiltered);
            SendungenDocs = DataService.GetSendungsAuftraegeFin(model);

            if (SendungenDocs.None())
                addModelError("", Localize.NoDataFound);
            else if (!ShowStatusInGrid)
                SendungenDocs.ForEach(x => x.StatusText = "");

            DataMarkForRefresh();
        }

        
        #endregion



        private void GetCurrentAppID()
        {
            CurrentAppID = LogonContext.GetAppIdCurrent();
        }


        [XmlIgnore]
        public List<SendungsAuftrag> SendungenPlaces
        {
            get { return PropertyCacheGet(() => new List<SendungsAuftrag>()); }
            private set { PropertyCacheSet(value); }
        }

        public void LoadSendungenPlace(SendungsAuftragPlaceSelektor model, Action<string, string> addModelError)
        {
            PropertyCacheClear(this, m => m.SendungenDocsFiltered);
            SendungenPlaces = DataService.GetSendungsAuftraegePlace(model);

            if (SendungenPlaces.None())
                addModelError("", Localize.NoDataFound);
            else if (!ShowStatusInGrid)
                SendungenPlaces.ForEach(x => x.StatusText = "");

            DataMarkForRefresh();
        }

        public void InitStandorte()
        {
            Standorte = DataService.GetFahrzeugstandorte();
        }
    }
}
