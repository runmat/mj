using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Xml.Serialization;
using CkgDomainLogic.DomainCommon.Contracts;
using CkgDomainLogic.DomainCommon.Models;
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
    [DashboardProviderViewModel]
    public class WfmViewModel : CkgBaseViewModel
    {
        [XmlIgnore]
        public IWfmDataService DataService { get { return CacheGet<IWfmDataService>(); } }

        [XmlIgnore]
        public IAdressenDataService AdressenDataService { get { return CacheGet<IAdressenDataService>(); } }

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

        public string Title
        {
            get
            {
                switch (Selektor.Modus)
                {
                    case SelektionsModus.KlaerfallWorkplace:
                        return Localize.Wfm_KlaerfallWorkplace;
                    case SelektionsModus.Abmeldevorgaenge:
                        return Localize.Wfm_Abmeldevorgaenge;
                    case SelektionsModus.Durchlauf:
                        return Localize.Wfm_Durchlaufzeiten;
                }

                return "";
            }
        }

        public string DokArten { get { return "VER;SIP;SOS"; } }

        [LocalizedDisplay(LocalizeConstants.DocumentType)]
        public string SelectedDokArt
        {
            get { return PropertyCacheGet(() => DokArten.ToSelectList().First().Value); }
            set { PropertyCacheSet(value); }
        }


        public void DataInit(SelektionsModus modus)
        {
            Selektor.Modus = modus;
            Selektor.ToDoWer = "";

            InitFeldnamen();
            DataMarkForRefresh();
        }

        public void DataMarkForRefresh()
        {
            PropertyCacheClear(this, m => m.AuftraegeFiltered);
            PropertyCacheClear(this, m => m.DurchlaufDetailsFiltered);

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

            if (Auftraege.None() && state != null)
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
            LoadRechercheprotokoll();
        }

        public void FilterAuftraege(string filterValue, string filterProperties)
        {
            AuftraegeFiltered = Auftraege.SearchPropertiesWithOrCondition(filterValue, filterProperties);
        }

        private ChartItemsPackage AuftraegeForChart(string abmeldeStatus)
        {
            var selector = new WfmAuftragSelektor
            {
                Selektionsfeld1 = true,
                Selektionsfeld1Name = "X",
                Abmeldestatus = abmeldeStatus == null ? new List<string>() : new List <string> { abmeldeStatus },
                SolldatumVonBis = new DateRange(DateRangeType.CurrentYear, true)
            };
            DashboardSessionSaveCurrentReportSelector(selector);

            var items = DataService.GetAbmeldeauftraege(selector)
                                    .Where(data => data.Solldatum.GetValueOrDefault() > DateTime.MinValue)
                                    .OrderBy(data => data.Solldatum)
                                    .ToListOrEmptyList();
            
            Func<DateTime, string> xAxisKeyFormat = (itemKey => itemKey.ToString("yyyyMM"));
            Func<WfmAuftrag, DateTime> xAxisKeyModel = (groupKey => groupKey.Solldatum.ToFirstDayOfMonth());

            return ChartService.GetBarChartGroupedStackedItemsWithLabels(
                items,
                xAxisKey => xAxisKeyFormat(xAxisKeyModel(xAxisKey)),
                xAxisList => xAxisList.Insert(0, xAxisKeyFormat(items.Min(xAxisKeyModel).AddMonths(-1)))
                );
        }


        #region Chart Functions, accessed from extern

        [DashboardItemsLoadMethod("WFM_Auftraege_DiesesJahr_Alle")]
        public ChartItemsPackage NameNotRelevant11()
        {
            return AuftraegeForChart(null);
        }

        [DashboardItemsLoadMethod("WFM_Auftraege_DiesesJahr_InArbeit")]
        public ChartItemsPackage NameNotRelevant12()
        {
            return AuftraegeForChart("1");
        }

        [DashboardItemsLoadMethod("WFM_Auftraege_DiesesJahr_Abgemeldet")]
        public ChartItemsPackage NameNotRelevant13()
        {
            return AuftraegeForChart("2");
        }

        [DashboardItemsLoadMethod("WFM_Auftraege_DiesesJahr_Storniert")]
        public ChartItemsPackage NameNotRelevant14()
        {
            return AuftraegeForChart("3");
        }

        #endregion


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

        private string StornoAuftragCurrent { get; set; }

        public void VersandOptionChanged(string versandOption)
        {
            VersandOption = GetVersandOption(versandOption).ID;
        }

        public string CreateVersandAdresse()
        {
            var vorgangNr = StornoAuftragCurrent;

            var auftrag = AuftraegeFiltered.FirstOrDefault(a => a.VorgangsNrAbmeldeauftrag == vorgangNr);
            if (auftrag == null)
                return Localize.OrderDoesNotExist;

            var message = DataService.CreateVersandAdresse(vorgangNr.ToInt(), auftrag, VersandAdresse, GetVersandOption(VersandOption).MaterialCode);
            if (!message.IsNullOrEmpty())
                return message;

            LoadAuftraege(null);

            return message;
        }

        private VersandOption GetVersandOption(string versandOption = null)
        {
            VersandOption vOption;
            if (versandOption == null)
                vOption = VersandOptionenList.FirstOrDefault();
            else
                vOption = VersandOptionenList.FirstOrDefault(v => v.ID.NotNullOrEmpty().TrimStart('0') == versandOption.NotNullOrEmpty().TrimStart('0'));

            return vOption ?? VersandOptionenList.FirstOrDefault() ?? new VersandOption();
        }

        public string StornoAuftrag(string vorgangNr)
        {
            if (vorgangNr.IsNotNullOrEmpty())
                StornoAuftragCurrent = vorgangNr;

            var auftrag = AuftraegeFiltered.FirstOrDefault(a => a.VorgangsNrAbmeldeauftrag == vorgangNr);
            if (auftrag == null)
                return Localize.OrderDoesNotExist;

            if (auftrag.AbmeldeStatusCode.NotNullOrEmpty() == "2")
                return Localize.CancellationNotPossible + ". " + Localize.OrderAlreadyDone;

            if (auftrag.AbmeldeStatusCode.NotNullOrEmpty() == "0" || auftrag.AbmeldeStatusCode.NotNullOrEmpty() == "1")
                if (auftrag.Zb1VorhandenDatum != null || auftrag.KennzeichenVornVorhandenDatum != null || auftrag.KennzeichenHintenVorhandenDatum != null)
                    return "ENFORCE_DELIVERY_ADDRESS";

            var message = DataService.StornoAuftrag(vorgangNr.ToInt());
            if (!message.IsNullOrEmpty())
                return message;

            auftrag.StornoDatum = DateTime.Today;
            auftrag.AbmeldeStatusCode = "3";

            LoadAuftraege(null);

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


        #region Versandadresse

        [XmlIgnore]
        public List<Adresse> VersandAdressen
        {
            get { return AdressenDataService.Adressen.Where(a => a.Kennung == "VERSANDADRESSE").ToListOrEmptyList(); }
        }

        [XmlIgnore]
        public List<string> VersandAdressenAsAutoCompleteItems
        {
            get { return VersandAdressen.Select(a => a.GetAutoSelectString()).ToList(); }
        }

        [XmlIgnore]
        public List<Adresse> VersandAdressenFiltered
        {
            get { return PropertyCacheGet(() => VersandAdressen); }
            private set { PropertyCacheSet(value); }
        }

        [Required]
        [LocalizedDisplay(LocalizeConstants.ShippingOption)]
        public string VersandOption
        {
            get { return PropertyCacheGet(() => GetVersandOption().ID); }
            set { PropertyCacheSet(value); }
        }

        [XmlIgnore]
        public List<VersandOption> VersandOptionenList
        {
            get
            {
                return DataService.VersandOptionen.Where(vo => vo.IstEndgueltigerVersand).OrderBy(w => w.Name).ToList();
            }
        }

        public Adresse VersandAdresse
        {
            get { return PropertyCacheGet(() => new Adresse {Land = "DE", Kennung = "VERSANDADRESSE"}); }
            set { PropertyCacheSet(value); }
        }

        public Adresse GetVersandAdresseFromKey(string key)
        {
            int id;
            Adresse adr = null;
            if (int.TryParse(key, out id))
                adr = VersandAdressen.FirstOrDefault(v => v.ID == id);

            if (adr == null)
                adr = VersandAdressen.FirstOrDefault(a => a.GetAutoSelectString() == key);

            if (adr != null)
                if (adr.Land.IsNullOrEmpty())
                    adr.Land = "DE";

            return adr;
        }

        public void FilterVersandAdressen(string filterValue, string filterProperties)
        {
            VersandAdressenFiltered = VersandAdressen.SearchPropertiesWithOrCondition(filterValue, filterProperties);
        }

        public void DataMarkForRefreshVersandAdressenFiltered()
        {
            PropertyCacheClear(this, m => m.VersandAdressenFiltered);
        }

        public void PrepareVersandAdresse()
        {
            PropertyCacheClear(this, m => m.VersandAdresse);
            PropertyCacheClear(this, m => m.VersandOption);
            Adresse.Laender = AdressenDataService.Laender;
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
                Zeit = DateTime.Now.ToString("HH:mm:ss"),
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
        public List<WfmDurchlaufSingle> DurchlaufDetailsForChart
        {
            get { return PropertyCacheGet(() => new List<WfmDurchlaufSingle>()); }
            private set { PropertyCacheSet(value); }
        }


        [XmlIgnore]
        public List<WfmDurchlaufStatistik> DurchlaufStatistiken
        {
            get { return PropertyCacheGet(() => new List<WfmDurchlaufStatistik>()); }
            private set { PropertyCacheSet(value); }
        }

        public void LoadDurchlauf(ModelStateDictionary state, bool chartMode)
        {
            DataMarkForRefresh();

            DataService.GetDurchlauf(Selektor, (details, statistiken) =>
            {
                if (chartMode)
                    DurchlaufDetailsForChart = details.ToListOrEmptyList();
                else
                {
                    DurchlaufDetails = details.ToListOrEmptyList();
                    DurchlaufStatistiken = statistiken.ToListOrEmptyList();
                    DurchlaufDetailsForChart = DurchlaufDetails;
                }
            });

            if (DurchlaufDetails.None() && state != null)
                state.AddModelError("", Localize.NoDataFound);
        }

        public void FilterDurchlaufDetails(string filterValue, string filterProperties)
        {
            DurchlaufDetailsFiltered = DurchlaufDetails.SearchPropertiesWithOrCondition(filterValue, filterProperties);
        }

        private ChartItemsPackage GetBarChartGroupedItemsWithLabels(List<WfmDurchlaufSingle> items)
        {
            var xAxisMonthDates = items
                .OrderBy(it => it.ErledigtDatum).GroupBy(g => g.ErledigtDatum.ToFirstDayOfMonth()).Select(it => it.Key).ToArray();

            var xAxisGroups = items
                .GroupBy(g => g.XaxisLabel).Select(it => it.Key)
                .OrderBy(WfmDurchlaufSingle.GetSortByXaxisLabel).ToArray();

            const double xAxisStart = 3.5;
            var data = new object[xAxisMonthDates.Length];
            for (var month = 0; month < xAxisMonthDates.Length; month++)
            {
                var groupArray = new object[xAxisGroups.Length];

                var monthItems = items.Where(monthItem => monthItem.ErledigtDatum.ToFirstDayOfMonth() == xAxisMonthDates[month]);
                var tageDiesesMonatsGesamt = monthItems.Sum(g => g.DurchlaufzeitTage.ToInt());

                for (var group = 0; group < xAxisGroups.Length; group++)
                {
                    var groupMonthItems = monthItems.Where(monthItem => monthItem.XaxisLabel == xAxisGroups[group]);
                    var tageDiesesMonatsUndGruppeGesamt = groupMonthItems.Sum(g => g.DurchlaufzeitTage.ToInt());

                    var tageDiesesMonatsUndGruppeProzent = 0.0;
                    if (tageDiesesMonatsGesamt > 0)
                        tageDiesesMonatsUndGruppeProzent = tageDiesesMonatsUndGruppeGesamt * 100.0 / tageDiesesMonatsGesamt;

                    double incGroupX = group * xAxisMonthDates.Length + group;
                    groupArray[group] = new[] { xAxisStart + incGroupX + month, tageDiesesMonatsUndGruppeProzent };
                }

                data[month] = new { data = groupArray, label = xAxisMonthDates[month].ToString("MMMM yyyy") };
            }

            var tickOfset = (xAxisMonthDates.Length / 2.0) - 0.5;
            var tickStart = xAxisStart + tickOfset;
            double tickInc = xAxisMonthDates.Length + 1, tickPos = 0.0;
            var ticksArray = xAxisGroups.Select(group => new ChartItemsTick
            {
                Pos = tickStart + (tickInc * tickPos++),
                Label = string.Format("{0} {1}", Selektor.AbmeldeartDurchlauf.AppendIfNotNullAndNot("Alle", "."), group)
            }).ToArray();

            return new ChartItemsPackage
            {
                data = data,
                labels = null,
                ticks = ticksArray
            };
        }

        private ChartItemsPackage GetRawChartData(List<WfmDurchlaufSingle> items)
        {
            var data = GetBarChartGroupedItemsWithLabels(items);

            return ChartService.PrepareChartDataAndOptions(data, AppSettings.DataPath, "bar", "WfmDurchlaufzeiten");
        }

        private ChartItemsPackage GetExpliciteChartDataForDashboard(string abmeldeArt)
        {
            Selektor = new WfmAuftragSelektor
            {
                Selektionsfeld1 = true,
                Selektionsfeld1Name = "X",
                ErledigtDatumVonBis = new DateRange(DateRangeType.Last3Months, true),
                AbmeldeartDurchlauf = abmeldeArt
            };
            DashboardSessionSaveCurrentReportSelector(Selektor);

            LoadDurchlauf(null, true);

            return GetRawChartData(DurchlaufDetailsForChart);
        }


        #region Chart Functions, accessed from extern

        [HttpPost]
        public object GetChartData(int chartID)
        {
            var items = DurchlaufDetailsForChart;
            if (chartID > 1)
            {
                Selektor.AbmeldeartDurchlauf = Selektor.GetAlleAbmeldeartenDurchlaufNextKeyFor(Selektor.AbmeldeartDurchlauf);

                LoadDurchlauf(null, true);

                items = DurchlaufDetailsForChart;
            }

            return GetRawChartData(items);
        }

        [DashboardItemsLoadMethod("WFM_Durchlaufzeiten_Alle")]
        public ChartItemsPackage NameNotRelevant01()
        {
            return GetExpliciteChartDataForDashboard("Alle");
        }

        [DashboardItemsLoadMethod("WFM_Durchlaufzeiten_Klaer")]
        public ChartItemsPackage NameNotRelevant02()
        {
            return GetExpliciteChartDataForDashboard("Klär");
        }

        [DashboardItemsLoadMethod("WFM_Durchlaufzeiten_Std")]
        public ChartItemsPackage NameNotRelevant03()
        {
            return GetExpliciteChartDataForDashboard("Std");
        }

        #endregion

        #endregion


        #region Rechercheprotokoll

        [XmlIgnore]
        public List<WfmRechercheprotokoll> RechercheprotokollDatenList
        {
            get { return PropertyCacheGet(() => new List<WfmRechercheprotokoll>()); }
            private set { PropertyCacheSet(value); }
        }

        public WfmRechercheprotokoll RechercheprotokollKunde
        {
            get { return RechercheprotokollDatenList.FirstOrDefault(r => r.KennungAnsprechpartner == "KUNDE", new WfmRechercheprotokoll()); }
        }

        public bool RechercheprotokollKundeVorhanden
        {
            get { return RechercheprotokollDatenList.Any(r => r.KennungAnsprechpartner == "KUNDE"); }
        }

        public WfmRechercheprotokoll RechercheprotokollHaendler
        {
            get { return RechercheprotokollDatenList.FirstOrDefault(r => r.KennungAnsprechpartner == "HAENDLER", new WfmRechercheprotokoll()); }
        }

        public bool RechercheprotokollHaendlerVorhanden
        {
            get { return RechercheprotokollDatenList.Any(r => r.KennungAnsprechpartner == "HAENDLER"); }
        }

        public WfmRechercheprotokoll RechercheprotokollSachverstaendiger
        {
            get { return RechercheprotokollDatenList.FirstOrDefault(r => r.KennungAnsprechpartner == "SACHV", new WfmRechercheprotokoll()); }
        }

        public bool RechercheprotokollSachverstaendigerVorhanden
        {
            get { return RechercheprotokollDatenList.Any(r => r.KennungAnsprechpartner == "SACHV"); }
        }

        public WfmRechercheprotokoll RechercheprotokollSonstige
        {
            get { return RechercheprotokollDatenList.FirstOrDefault(r => r.KennungAnsprechpartner == "SONST", new WfmRechercheprotokoll()); }
        }

        public bool RechercheprotokollSonstigeVorhanden
        {
            get { return RechercheprotokollDatenList.Any(r => r.KennungAnsprechpartner == "SONST"); }
        }

        private void LoadRechercheprotokoll()
        {
            RechercheprotokollDatenList = DataService.GetRechercheprotokollDaten(AktuellerAuftragVorgangsNr);
        }

        #endregion
    }
}
