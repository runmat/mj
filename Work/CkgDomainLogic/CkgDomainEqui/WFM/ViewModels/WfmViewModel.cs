using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Xml.Serialization;
using CkgDomainLogic.General.Models;
using CkgDomainLogic.General.Services;
using CkgDomainLogic.General.ViewModels;
using CkgDomainLogic.WFM.Contracts;
using CkgDomainLogic.WFM.Models;
using GeneralTools.Models;
using GeneralTools.Resources;
using MvcTools.Web;

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

        public string DokArten { get { return "VER;SIP;SOS"; } }

        [LocalizedDisplay(LocalizeConstants.DocumentType)]
        public string SelectedDokArt
        {
            get { return PropertyCacheGet(() => DokArten.ToSelectList().First().Value); }
            set { PropertyCacheSet(value); }
        }


        public void DataInit(SelektionsModus modus)
        {
            Selektor = new WfmAuftragSelektor { Modus = modus, ToDoWer = "" };
            InitFeldnamen();
            DataMarkForRefresh();
        }

        public void DataMarkForRefresh()
        {
            PropertyCacheClear(this, m => m.AuftraegeFiltered);
            PropertyCacheClear(this, m => m.DurchlaufDetailsFiltered);
            PropertyCacheClear(this, m => m.DurchlaufStatistikenFiltered);

            DataMarkForRefreshDetails();
        }

        private void DataMarkForRefreshDetails()
        {
            PropertyCacheClear(this, m => m.InformationenFiltered);
            PropertyCacheClear(this, m => m.DokumenteFiltered);
            PropertyCacheClear(this, m => m.AufgabenFiltered);
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
            RefreshAufgaben();
        }

        public void FilterAuftraege(string filterValue, string filterProperties)
        {
            AuftraegeFiltered = Auftraege.SearchPropertiesWithOrCondition(filterValue, filterProperties);
        }


        #region Misc

        public string GetHeaderText(string columnName)
        {
            return GetFeldname(columnName);
        }

        public bool HeaderTextAvailable(string columnName)
        {
            return GetHeaderText(columnName).IsNotNullOrEmpty();
        }

        private string GetFeldname(string columnName)
        {
            return Feldnamen.Any(f => f.Feldname == columnName) ? Feldnamen.First(f => f.Feldname == columnName).Anzeigename : "";
        }

        private void InitFeldnamen()
        {
            PropertyCacheClear(this, m => m.Feldnamen);

            Feldnamen = DataService.GetFeldnamen();

            Selektor.Selektionsfeld1Name = GetFeldname("SELEKTION1");
            Selektor.Selektionsfeld2Name = GetFeldname("SELEKTION2"); 
            Selektor.Selektionsfeld3Name = GetFeldname("SELEKTION3");

            Selektor.Referenz1Name = GetFeldname("REFERENZ1");
            Selektor.Referenz2Name = GetFeldname("REFERENZ2");
            Selektor.Referenz3Name = GetFeldname("REFERENZ3");
        }

        #endregion


        #region Übersicht/Storno

        public string StornoAuftrag(string vorgangNr)
        {
            var message = DataService.StornoAuftrag(vorgangNr.ToInt());
            if (message.IsNullOrEmpty())
            {
                var auftrag = AuftraegeFiltered.FirstOrDefault(a => a.VorgangsNrAbmeldeauftrag == vorgangNr);
                if (auftrag != null)
                {
                    auftrag.StornoDatum = DateTime.Today;
                    auftrag.AbmeldeStatusCode = "3";
                }

                DataMarkForRefresh();
            }

            return message;
        }

        public string SetOrderToKlaerfall(string vorgangsNr, string remark)
        {
            var message = DataService.SetOrderToKlaerfall(vorgangsNr.ToInt(), remark);
            if (message.IsNullOrEmpty())
            {
                AktuellerAuftrag.AbmeldeArtCode = "2";

                RefreshAufgaben();
            }

            return message;
        }
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
                LaufendeNr = (Informationen.None() ? 1 : Informationen.Max(i => i.LaufendeNr.ToInt(0)) + 1).ToString(),
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
                return dok.FileBytes;

            return null;
        }

        public string SaveDokument(HttpPostedFileBase file)
        {
            byte[] fileBytes;
            using (var binReader = new BinaryReader(file.InputStream))
            {
                fileBytes = binReader.ReadBytes(file.ContentLength);
            }

            var neuesDok = new WfmDokument
                {
                    DocumentType = SelectedDokArt,
                    FileName = file.FileName,
                    FileBytes = fileBytes,
                };

            var neueDokInfo = DataService.SaveDokument(AktuellerAuftragVorgangsNr, neuesDok);
            if (neueDokInfo.ObjectId == null)
                return neueDokInfo.ErrorMessage;

            Dokumente.Add(neueDokInfo);
            return "";
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

        public bool IstHoechsteLfdNr(int lfdNr)
        {
            if (Aufgaben.ToListOrEmptyList().None())
                return false;

            var hoechsteNummer = Aufgaben.Select(a => a.LaufendeNr.ToInt()).Max();
            
            return (hoechsteNummer == lfdNr);
        }

        private void RefreshAufgaben()
        {
            Aufgaben = DataService.GetToDos(AktuellerAuftragVorgangsNr);
            PropertyCacheClear(this, m => m.AufgabenFiltered);
        }

        public string ConfirmToDo(int lfdNr, string remark)
        {
            var message = DataService.ConfirmToDo(AktuellerAuftragVorgangsNr.ToInt(), lfdNr, remark);
            if (message.IsNullOrEmpty())
            {
                RefreshAufgaben();
            }

            return message;
        }

        #endregion


        #region Durchlauf

        [XmlIgnore]
        public List<WfmDurchlaufSingle> DurchlaufDetails
        {
            get { return PropertyCacheGet(() => new List<WfmDurchlaufSingle>()); }
            private set { PropertyCacheSet(value); }
        }

        [XmlIgnore]
        public List<WfmDurchlaufSingle> DurchlaufDetailsFiltered
        {
            get { return PropertyCacheGet(() => DurchlaufDetails); }
            private set { PropertyCacheSet(value); }
        }

        [XmlIgnore]
        public List<WfmDurchlaufStatistik> DurchlaufStatistiken
        {
            get { return PropertyCacheGet(() => new List<WfmDurchlaufStatistik>()); }
            private set { PropertyCacheSet(value); }
        }

        [XmlIgnore]
        public List<WfmDurchlaufStatistik> DurchlaufStatistikenFiltered
        {
            get { return PropertyCacheGet(() => DurchlaufStatistiken); }
            private set { PropertyCacheSet(value); }
        }

        public void LoadDurchlauf(ModelStateDictionary state)
        {
            DataMarkForRefresh();

            DataService.GetDurchlauf(Selektor, (details, statistiken) =>
            {
                DurchlaufDetails = details.ToListOrEmptyList();
                DurchlaufStatistiken = statistiken.ToListOrEmptyList();
            });

            if (DurchlaufDetails.None() && state != null)
                state.AddModelError("", Localize.NoDataFound);
        }

        public void FilterDurchlaufDetails(string filterValue, string filterProperties)
        {
            DurchlaufDetailsFiltered = DurchlaufDetails.SearchPropertiesWithOrCondition(filterValue, filterProperties);
        }

        public void FilterDurchlaufStatistiken(string filterValue, string filterProperties)
        {
            DurchlaufStatistikenFiltered = DurchlaufStatistiken.SearchPropertiesWithOrCondition(filterValue, filterProperties);
        }

        public object GetChartData()
        {
            var data = GetBarChartGroupedItemsWithLabels(DurchlaufDetails);

            return ChartService.PrepareChartDataAndOptions(data, AppSettings.DataPath, "bar");
        }

        public ChartItemsPackage GetBarChartGroupedItemsWithLabels(List<WfmDurchlaufSingle> items) 
        {
            var xAxisMonthDates = items
                .OrderBy(it => it.ErledigtDatum).GroupBy(g => g.ErledigtDatum.ToFirstDayOfMonth()).Select(it => it.Key).ToArray();

            var xAxisGroups = items
                .OrderBy(it => it.XaxisLabelSort).GroupBy(g => g.XaxisLabel).Select(it => it.Key).ToArray();

            var xAxisStart = 3.5;
            var data = new object[xAxisMonthDates.Length];
            for (int month = 0; month < xAxisMonthDates.Length; month++)
            {
                var groupArray = new object[xAxisGroups.Length];

                var monthItems = items.Where(monthItem => monthItem.ErledigtDatum.ToFirstDayOfMonth() == xAxisMonthDates[month]);
                var tageDiesesMonatsGesamt = monthItems.Sum(g => g.DurchlaufzeitTage.ToInt());

                for (int group = 0; group < xAxisGroups.Length; group++)
                {
                    var groupMonthItems = monthItems.Where(monthItem => monthItem.XaxisLabel == xAxisGroups[group]);
                    var tageDiesesMonatsUndGruppeGesamt = groupMonthItems.Sum(g => g.DurchlaufzeitTage.ToInt());

                    var tageDiesesMonatsUndGruppeProzent = 0.0;
                    if (tageDiesesMonatsGesamt > 0)
                        tageDiesesMonatsUndGruppeProzent = tageDiesesMonatsUndGruppeGesamt * 100.0 / tageDiesesMonatsGesamt;

                    double incGroupX = group * xAxisMonthDates.Length + group;
                    groupArray[group] = new double[2] { xAxisStart + incGroupX + month, tageDiesesMonatsUndGruppeProzent };
                }

                data[month] = new { data = groupArray, label = xAxisMonthDates[month].ToString("MMMM yyyy") };
            }

            double tickOfset = (xAxisMonthDates.Length / 2.0) - 0.5;
            double tickStart = xAxisStart + tickOfset;
            double tickInc = xAxisMonthDates.Length + 1, tickPos = 0.0;
            var ticksArray = xAxisGroups.Select(group => new ChartItemsTick
            {
                Pos = tickStart + (tickInc * tickPos++),
                Label = string.Format("{0} {1}", Selektor.AbmeldeartDurchlauf.AppendIfNotNullAndNot("Alle", "."), group)
            }).ToArray();

            return new ChartItemsPackage
            {
                data = data, labels = null, ticks = ticksArray
            };
        }

        #endregion
    }
}
