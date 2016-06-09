using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Xml.Serialization;
using CkgDomainLogic.Archive.Contracts;
using CkgDomainLogic.General.Services;
using CkgDomainLogic.Remarketing.Contracts;
using CkgDomainLogic.Remarketing.Models;
using CkgDomainLogic.General.ViewModels;
using DocumentTools.Services;
using GeneralTools.Models;
using GeneralTools.Services;

namespace CkgDomainLogic.Remarketing.ViewModels
{
    public class BelastungsanzeigenViewModel : CkgBaseViewModel
    {
        [XmlIgnore]
        public IBelastungsanzeigenDataService DataService { get { return CacheGet<IBelastungsanzeigenDataService>(); } }

        [XmlIgnore]
        public IEasyAccessDataService EasyAccessDataService { get { return CacheGet<IEasyAccessDataService>(); } }

        public BelastungsanzeigenSelektor Selektor
        {
            get { return PropertyCacheGet(() => new BelastungsanzeigenSelektor()); }
            set { PropertyCacheSet(value); }
        }

        [XmlIgnore]
        public List<Vermieter> Vermieter
        {
            get { return PropertyCacheGet(() => DataService.GetVermieter()); }
        }

        [XmlIgnore]
        public List<Hereinnahmecenter> Hereinnahmecenter
        {
            get { return PropertyCacheGet(() => DataService.GetHereinnahmecenter()); }
        }

        [XmlIgnore]
        public List<SelectItem> StatusList
        {
            get
            {
                var liste = new List<SelectItem>
                {
                    new SelectItem("0", Localize.Open),
                    new SelectItem("1", Localize.Release),
                    new SelectItem("2", Localize.Vetoed),
                    new SelectItem("4", Localize.Billed),
                    new SelectItem("5", Localize.NotBilled),
                    new SelectItem("9", Localize.BlockedBlockiert)
                };

                if (IsAv)
                    liste.Add(new SelectItem("3", Localize.WorkInProgress));

                return liste.OrderBy(x => x.Key).ToList();
            }
        }

        [XmlIgnore]
        public List<Belastungsanzeige> Belastungsanzeigen
        {
            get { return PropertyCacheGet(() => new List<Belastungsanzeige>()); }
            protected set { PropertyCacheSet(value); }
        }

        [XmlIgnore]
        public List<Belastungsanzeige> BelastungsanzeigenFiltered
        {
            get { return PropertyCacheGet(() => Belastungsanzeigen); }
            protected set { PropertyCacheSet(value); }
        }

        [XmlIgnore]
        public List<Belastungsanzeige> BelastungsanzeigenSelected
        {
            get { return Belastungsanzeigen.Where(b => b.Auswahl).ToList(); }
        }

        [XmlIgnore]
        public List<Gutachten> Gutachten
        {
            get { return PropertyCacheGet(() => new List<Gutachten>()); }
            protected set { PropertyCacheSet(value); }
        }

        [XmlIgnore]
        public List<Gutachten> GutachtenFiltered
        {
            get { return PropertyCacheGet(() => Gutachten); }
            protected set { PropertyCacheSet(value); }
        }

        public bool IsAv
        {
            get { return LogonContext.GroupName.NotNullOrEmpty().StartsWith("AV"); }
        }

        public bool ShowCheckBoxColumn
        {
            get { return (IsAv && Selektor.Status == "0") || (!IsAv && !string.IsNullOrEmpty(Selektor.Status) && Selektor.Status != "4"); }
        }

        public string TuevGutachtenBaseUrl {
            get { return PropertyCacheGet(() => ApplicationConfiguration.GetApplicationConfigValue("TuevGutachtenUrl", "0", LogonContext.CustomerID)); }
        }

        public void DataInit()
        {
            DataMarkForRefresh();
        }

        private void DataMarkForRefresh()
        {
            PropertyCacheClear(this, m => m.BelastungsanzeigenFiltered);
            PropertyCacheClear(this, m => m.GutachtenFiltered);
        }

        public void LoadBelastungsanzeigen(BelastungsanzeigenSelektor selektor, Action<string, string> addModelError)
        {
            if (IsAv)
                selektor.Vermieter = LogonContext.GroupName;

            Selektor = selektor;

            Belastungsanzeigen = DataService.GetBelastungsanzeigen(Selektor);

            DataMarkForRefresh();

            if (Belastungsanzeigen.None())
                addModelError("", Localize.NoDataFound);
        }

        public void LoadGutachten(string fahrgestellNr, Action<string, string> addModelError)
        {
            Gutachten = DataService.GetGutachten(fahrgestellNr);

            PropertyCacheClear(this, m => m.GutachtenFiltered);

            if (Gutachten.None())
                addModelError("", Localize.NoSurveysFound);
        }

        public void SelectBelastungsanzeige(string fin, bool select, out int allSelectionCount)
        {
            allSelectionCount = 0;
            var item = Belastungsanzeigen.FirstOrDefault(f => f.FahrgestellNr == fin);
            if (item == null)
                return;

            item.Auswahl = select;
            allSelectionCount = Belastungsanzeigen.Count(c => c.Auswahl);
        }

        public void SelectBelastungsanzeigen(bool select, Predicate<Belastungsanzeige> filter, out int allSelectionCount)
        {
            BelastungsanzeigenFiltered.Where(f => filter(f)).ToListOrEmptyList().ForEach(f => f.Auswahl = select);
            allSelectionCount = Belastungsanzeigen.Count(x => x.Auswahl);
        }

        public byte[] GetBelastungsanzeigePdf(string fahrgestellNr)
        {
            var item = Belastungsanzeigen.FirstOrDefault(b => b.FahrgestellNr == fahrgestellNr);
            var datum = (item != null && item.Freigabedatum.HasValue ? item.Freigabedatum.Value : DateTime.Now);

            var lagerort = ApplicationConfiguration.GetApplicationConfigValue("ArchivBelastungsanzeigenLagerort", "0", LogonContext.CustomerID);
            var archiv = ApplicationConfiguration.GetApplicationConfigValue("ArchivBelastungsanzeigenName", "0", LogonContext.CustomerID);

            if (string.IsNullOrEmpty(lagerort) || string.IsNullOrEmpty(archiv))
                return null;

            var mitJahr = ApplicationConfiguration.GetApplicationConfigValue("ArchivBelastungsanzeigenMitJahr", "0", LogonContext.CustomerID);
            if (string.Compare(mitJahr, "true", true) == 0)
                archiv += datum.ToString("yy");

            var relDocPaths = EasyAccessDataService.GetDocuments(lagerort, archiv, "SGW", string.Format(".1001={0}", fahrgestellNr));

            var fileList = new List<byte[]>();

            relDocPaths.ForEach(d => fileList.Add(File.ReadAllBytes(HttpContext.Current.Server.MapPath(d.Replace("\\", "/")))));

            return (fileList.None() ? null : PdfDocumentFactory.MergePdfDocuments(fileList));
        }

        public byte[] GetReparaturKalkulationPdf(string fahrgestellNr)
        {
            var item = Belastungsanzeigen.FirstOrDefault(b => b.FahrgestellNr == fahrgestellNr);
            var anzahlRepKalk = (item != null ? item.AnzahlReparaturKalkulationen : 0);

            var repKalkUrl = ApplicationConfiguration.GetApplicationConfigValue("RepKalkUrl", "0", LogonContext.CustomerID, LogonContext.Group.GroupID);
            var fileList = new List<byte[]>();

            using (var clnt = new WebClient())
            {
                for (var i = 0; i < anzahlRepKalk; i++)
                {
                    var downloadUrl = string.Format("{0}?fin={1}&nummer={2}", repKalkUrl, fahrgestellNr, i + 1);
                    var downloadFile = clnt.DownloadData(downloadUrl);
                    if (downloadFile != null)
                        fileList.Add(downloadFile);
                }
            }

            return (fileList.None() ? null : PdfDocumentFactory.MergePdfDocuments(fileList));
        }

        public InfoAnzeigeModel GetReklamationstext(string fahrgestellNr)
        {
            var reklaText = DataService.GetReklamationstext(fahrgestellNr);

            return new InfoAnzeigeModel
            {
                FahrgestellNr = fahrgestellNr,
                Titel = Localize.ClaimText,
                Infotext = reklaText
            };
        }

        public InfoAnzeigeModel GetBlockadetext(string fahrgestellNr)
        {
            var blockText = DataService.GetBlockadetext(fahrgestellNr);

            return new InfoAnzeigeModel
            {
                FahrgestellNr = fahrgestellNr,
                Titel = Localize.BlockText,
                Infotext = blockText
            };
        }

        public SetReklamationModel GetSetReklamationModel(string fahrgestellNr)
        {
            return new SetReklamationModel { FahrgestellNr = fahrgestellNr };
        }

        public void SetReklamation(SetReklamationModel model, Action<string, string> addModelError)
        {
            var ergMessage = DataService.SetReklamation(model);

            if (!string.IsNullOrEmpty(ergMessage))
            {
                addModelError("", ergMessage);
                return;
            }
            
            var item = Belastungsanzeigen.FirstOrDefault(b => b.FahrgestellNr == model.FahrgestellNr && b.Status.In("0,3"));
            if (item != null)
            {
                item.Status = "2";
                item.StatusText = "Widersprochen";

                PropertyCacheClear(this, m => m.BelastungsanzeigenFiltered);
            }
        }

        public SetBlockadeModel GetSetBlockadeModel()
        {
            return new SetBlockadeModel();
        }

        public void SetBlockade(SetBlockadeModel model, Action<string, string> addModelError)
        {
            BelastungsanzeigenSelected.ForEach(b =>
            {
                b.Status = "9";
                b.BlockadeText = model.BlockadeText;
            });

            UpdateBelastungsanzeigen(addModelError);
        }

        public void ResetBlockade(Action<string, string> addModelError)
        {
            BelastungsanzeigenSelected.ForEach(b => b.Status = "0");

            UpdateBelastungsanzeigen(addModelError);
        }

        public void SetInBearbeitung(Action<string, string> addModelError)
        {
            BelastungsanzeigenSelected.ForEach(b => b.Status = "3");

            UpdateBelastungsanzeigen(addModelError);
        }

        public void SetOffen(Action<string, string> addModelError)
        {
            var ergMessage = DataService.SetBelastungsanzeigenOffen(BelastungsanzeigenSelected);

            if (!string.IsNullOrEmpty(ergMessage))
            {
                addModelError("", ergMessage);
                return;
            }

            Belastungsanzeigen.RemoveAll(m => m.Auswahl);

            PropertyCacheClear(this, m => m.BelastungsanzeigenFiltered);
        }

        private void UpdateBelastungsanzeigen(Action<string, string> addModelError)
        {
            var ergMessage = DataService.UpdateBelastungsanzeigen(BelastungsanzeigenSelected);

            if (!string.IsNullOrEmpty(ergMessage))
            {
                addModelError("", ergMessage);
                return;
            }

            Belastungsanzeigen.RemoveAll(b => b.Auswahl);

            PropertyCacheClear(this, m => m.BelastungsanzeigenFiltered);
        }

        public void FilterBelastungsanzeigen(string filterValue, string filterProperties)
        {
            BelastungsanzeigenFiltered = Belastungsanzeigen.SearchPropertiesWithOrCondition(filterValue, filterProperties);
        }

        public void FilterGutachten(string filterValue, string filterProperties)
        {
            GutachtenFiltered = Gutachten.SearchPropertiesWithOrCondition(filterValue, filterProperties);
        }
    }
}
