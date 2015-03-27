using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Xml.Serialization;
using CkgDomainLogic.General.Services;
using CkgDomainLogic.General.ViewModels;
using CkgDomainLogic.WFM.Contracts;
using CkgDomainLogic.WFM.Models;
using GeneralTools.Models;

namespace CkgDomainLogic.WFM.ViewModels
{
    public class WfmViewModel : CkgBaseViewModel
    {
        [XmlIgnore]
        public IWfmDataService DataService { get { return CacheGet<IWfmDataService>(); } }

        public WfmAuftragSelektor Selektor
        {
            get { return PropertyCacheGet(() => new WfmAuftragSelektor()); }
            set { PropertyCacheSet(value); }
        }

        [XmlIgnore]
        public List<WfmAuftragFeldname> Feldnamen
        {
            get { return PropertyCacheGet(() => new List<WfmAuftragFeldname>()); }
            private set { PropertyCacheSet(value); }
        }

        [XmlIgnore]
        public List<WfmAuftrag> Auftraege
        {
            get { return PropertyCacheGet(() => new List<WfmAuftrag>()); }
            private set { PropertyCacheSet(value); }
        }

        [XmlIgnore]
        public List<WfmAuftrag> AuftraegeFiltered
        {
            get { return PropertyCacheGet(() => Auftraege); }
            private set { PropertyCacheSet(value); }
        }

        public string AktuellerAuftragVorgangsNr { get; set; }

        public WfmAuftrag AktuellerAuftrag { get { return Auftraege.FirstOrDefault(a => a.VorgangsNrAbmeldeauftrag == AktuellerAuftragVorgangsNr); } }

        public string Title { get { return (Selektor.Modus == SelektionsModus.KlaerfallWorkplace ? Localize.Wfm_KlaerfallWorkplace : Localize.Wfm_Abmeldevorgaenge); } }

        public void DataInit(SelektionsModus modus)
        {
            Selektor = new WfmAuftragSelektor { Modus = modus, ToDoWer = "ALLE" };
            InitFeldnamen();
            DataMarkForRefresh();
        }

        public void DataMarkForRefresh()
        {
            PropertyCacheClear(this, m => m.AuftraegeFiltered);
            DataMarkForRefreshDetails();
        }

        private void DataMarkForRefreshDetails()
        {
            PropertyCacheClear(this, m => m.InformationenFiltered);
            PropertyCacheClear(this, m => m.DokumenteFiltered);
            PropertyCacheClear(this, m => m.AufgabenFiltered);
        }

        private void InitFeldnamen()
        {
            PropertyCacheClear(this, m => m.Feldnamen);

            Feldnamen = DataService.GetFeldnamen();

            Selektor.Selektionsfeld1Name = (Feldnamen.Any(f => f.Feldname == "SELEKTION1") ? Feldnamen.First(f => f.Feldname == "SELEKTION1").Anzeigename : "");
            Selektor.Selektionsfeld2Name = (Feldnamen.Any(f => f.Feldname == "SELEKTION2") ? Feldnamen.First(f => f.Feldname == "SELEKTION2").Anzeigename : "");
            Selektor.Selektionsfeld3Name = (Feldnamen.Any(f => f.Feldname == "SELEKTION3") ? Feldnamen.First(f => f.Feldname == "SELEKTION3").Anzeigename : "");

            Selektor.Referenz1Name = (Feldnamen.Any(f => f.Feldname == "REFERENZ1") ? Feldnamen.First(f => f.Feldname == "REFERENZ1").Anzeigename : "");
            Selektor.Referenz2Name = (Feldnamen.Any(f => f.Feldname == "REFERENZ2") ? Feldnamen.First(f => f.Feldname == "REFERENZ2").Anzeigename : "");
            Selektor.Referenz3Name = (Feldnamen.Any(f => f.Feldname == "REFERENZ3") ? Feldnamen.First(f => f.Feldname == "REFERENZ3").Anzeigename : "");
        }

        public void LoadAuftraege(ModelStateDictionary state)
        {
            DataMarkForRefresh();

            Auftraege = DataService.GetAbmeldeauftraege(Selektor);

            if (Auftraege.None())
                state.AddModelError("", Localize.NoDataFound);
        }

        public void LoadAuftragsDetails(string vorgangsNr, ModelStateDictionary state)
        {
            DataMarkForRefreshDetails();

            AktuellerAuftragVorgangsNr = vorgangsNr;

            if (AktuellerAuftrag == null)
            {
                state.AddModelError("", Localize.NoDataFound);
                return;
            }

            Informationen = DataService.GetInfos(AktuellerAuftragVorgangsNr);
            Dokumente = DataService.GetDokumentInfos(AktuellerAuftragVorgangsNr);
            Aufgaben = DataService.GetToDos(AktuellerAuftragVorgangsNr);
        }

        public void FilterAuftraege(string filterValue, string filterProperties)
        {
            AuftraegeFiltered = Auftraege.SearchPropertiesWithOrCondition(filterValue, filterProperties);
        }

        #region Übersicht/Storno



        #endregion

        #region Informationen

        [XmlIgnore]
        public List<WfmInfo> Informationen
        {
            get { return PropertyCacheGet(() => new List<WfmInfo>()); }
            private set { PropertyCacheSet(value); }
        }

        [XmlIgnore]
        public List<WfmInfo> InformationenFiltered
        {
            get { return PropertyCacheGet(() => Informationen); }
            private set { PropertyCacheSet(value); }
        }

        public string SaveNeueInformation(string neueInfo)
        {
            var neueInformation = new WfmInfo
            {
                VorgangsNrAbmeldeauftrag = AktuellerAuftragVorgangsNr,
                Text = neueInfo,
                Datum = DateTime.Today,
                Zeit = DateTime.Now.ToString("HHmmss"),
                LaufendeNr = (Informationen.Max(i => i.LaufendeNr.ToInt(0)) + 1).ToString(),
                User = LogonContext.UserName
            };

            var saveErg = DataService.SaveNeueInformation(neueInformation);

            if (String.IsNullOrEmpty(saveErg))
                Informationen.Add(neueInformation);

            return saveErg;
        }

        public void FilterInformationen(string filterValue, string filterProperties)
        {
            InformationenFiltered = Informationen.SearchPropertiesWithOrCondition(filterValue, filterProperties);
        }

        #endregion

        #region Dokumente

        [XmlIgnore]
        public List<WfmDokumentInfo> Dokumente
        {
            get { return PropertyCacheGet(() => new List<WfmDokumentInfo>()); }
            private set { PropertyCacheSet(value); }
        }

        [XmlIgnore]
        public List<WfmDokumentInfo> DokumenteFiltered
        {
            get { return PropertyCacheGet(() => Dokumente); }
            private set { PropertyCacheSet(value); }
        }

        public byte[] GetDokument(string documentId)
        {
            var dokInfo = Dokumente.FirstOrDefault(d => d.ObjectId == documentId);

            var dok = DataService.GetDokument(dokInfo);
            if (dok != null)
                return Encoding.UTF8.GetBytes(dok.DocumentAsString);

            return null;
        }

        public string CurrentDocumentType { get; set; }

        public bool SaveDokument(HttpPostedFileBase file)
        {
            byte[] dateiBytes = null;
            using (var binReader = new BinaryReader(file.InputStream))
            {
                dateiBytes = binReader.ReadBytes(file.ContentLength);
            }

            var neuesDok = new WfmDokument
                {
                    DocumentType = CurrentDocumentType,
                    FileName = file.FileName,
                    DocumentAsString = Encoding.UTF8.GetString(dateiBytes)
                };

            var neueDokInfo = DataService.SaveDokument(AktuellerAuftragVorgangsNr, neuesDok);
            if (neueDokInfo != null)
            {
                Dokumente.Add(neueDokInfo);
                return true;
            }

            return false;
        }

        public void FilterDokumente(string filterValue, string filterProperties)
        {
            DokumenteFiltered = Dokumente.SearchPropertiesWithOrCondition(filterValue, filterProperties);
        }

        #endregion
        
        #region Aufgaben

        [XmlIgnore]
        public List<WfmToDo> Aufgaben
        {
            get { return PropertyCacheGet(() => new List<WfmToDo>()); }
            private set { PropertyCacheSet(value); }
        }

        [XmlIgnore]
        public List<WfmToDo> AufgabenFiltered
        {
            get { return PropertyCacheGet(() => Aufgaben); }
            private set { PropertyCacheSet(value); }
        }

        public void FilterAufgaben(string filterValue, string filterProperties)
        {
            AufgabenFiltered = Aufgaben.SearchPropertiesWithOrCondition(filterValue, filterProperties);
        }

        #endregion
    }
}
