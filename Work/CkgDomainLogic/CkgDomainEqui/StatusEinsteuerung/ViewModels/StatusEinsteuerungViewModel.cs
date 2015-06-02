using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using CkgDomainLogic.General.ViewModels;
using GeneralTools.Resources;
using CkgDomainLogic.FzgModelle.Contracts;
using CkgDomainLogic.FzgModelle.Models;
using GeneralTools.Models;

namespace CkgDomainLogic.FzgModelle.ViewModels
{
    public class StatusEinsteuerungViewModel : CkgBaseViewModel
    {
        [XmlIgnore]
        public IStatusEinsteuerungDataService DataService { get { return CacheGet<IStatusEinsteuerungDataService>(); } }

        [XmlIgnore]
        public List<StatusEinsteuerung> StatusEinsteuerungsInklSummenzeilen
        {
            get { return PropertyCacheGet(() => new List<StatusEinsteuerung>()); }
            private set { PropertyCacheSet(value); }
        }
  
        [XmlIgnore]
        public List<StatusEinsteuerung> StatusEinsteuerungs { get { return StatusEinsteuerungsInklSummenzeilen.Where(s => s.ModellCode.IsNotNullOrEmpty()).ToList(); } }

        [XmlIgnore]
        public List<StatusEinsteuerung> StatusEinsteuerungsFiltered
        {
            get { return PropertyCacheGet(() => StatusEinsteuerungs); }
            private set { PropertyCacheSet(value); }
        }

        [LocalizedDisplay(LocalizeConstants.VehiclesWithoutZb2)]
        public int ZB2OhneFahrzeugCount {
            get { return PropertyCacheGet(() => DataService.GetZbIIOhneFzgCount()); }
        }

        [LocalizedDisplay(LocalizeConstants.VehiclesBlocked)]
        public int AnzahlGesperrte {
            get
            {
                var gesamt = StatusEinsteuerungsInklSummenzeilen.FirstOrDefault(s => s.PDINummer == "GESAMT");
                if (gesamt != null)
                    return gesamt.Gesperrt;

                return 0;
            }
        }

        public bool ModusStatusReport { get; set; }

        public string ReportAsHtml { get { return WriteData2Html(); } }

        public string ReportAsHtmlFilename { get { return String.Format("Statusbericht{0}{1}", (ModusStatusReport ? "EC" : "Fahrzeugeinsteuerung"), DateTime.Now.ToString("yyyyMMddHHmmss")); } }

        public string ReportAsExcelFilename { get { return String.Format("{0}_{1}", DateTime.Now.ToString("yyyyMMdd_HHmmss"), LogonContext.UserName); } }

        public void DataMarkForRefresh()
        {
            PropertyCacheClear(this, m => m.StatusEinsteuerungsFiltered);
        }

        public void LoadStatusEinsteuerung()
        {
            if (ModusStatusReport)
            {
                StatusEinsteuerungsInklSummenzeilen = DataService.GetStatusbericht();
                StatusEinsteuerungsInklSummenzeilen.Add(new StatusEinsteuerung());
                StatusEinsteuerungsInklSummenzeilen.Add(new StatusEinsteuerung { PDINummer = String.Format("ZBII ohne Fahrzeug: {0}", ZB2OhneFahrzeugCount) });
                StatusEinsteuerungsInklSummenzeilen.Add(new StatusEinsteuerung { PDINummer = String.Format("Gesperrte Fahrzeuge: {0}", AnzahlGesperrte) });
            }
            else
            {
                StatusEinsteuerungsInklSummenzeilen = DataService.GetStatusEinsteuerung();
            }
            DataMarkForRefresh();
        }

        private string WriteData2Html()
        {
            var pdiOld = "";
            var strHtmlStart = "<html><head><title>Statusbericht {0}(Druckversion)</title></head><body bgcolor=\"#FFFFFF\">";
            var strHtmlEnd = "</body></html>";
            var strTableStart = "<table cellspacing=\"0\" cellpadding=\"3\" border=\"0\">";
            var strTableEnd = "</table>";
            var strLogo = "<tr><td colspan=\"{0}\" align=\"center\"><img alt=\"\" src=\"/Portal/Images/dadlogo.jpg\" /></td></tr><tr><td colspan=\"{0}\">&nbsp;</td></tr>";
            var strTrStart = "<tr>";
            var strTrEnd = "</tr>";
            var strHeaderTd = "<td valign=\"bottom\"><font size=\"1\" style=\"Arial\"><b>{0}</font></b></td>";
            var strRowTd = "<td><font size=\"1\" style=\"Arial\">{0}</font></td>";
            var strRowTdBold = "<td><b><font size=\"1\" style=\"Arial\">{0}</font></b></td>";
            var strHorizLine = "<tr><td colspan=\"{0}\"><hr></td></tr>";
            var strFooterPdi = "<tr><td style=\"font-size:10; font-weight:bold\">{0}:</td><td colspan=\"{1}\" style=\"font-size:10; font-weight:bold\">{2}</td> </tr>";

            var lstHeaders = new List<string>();
            
            if (ModusStatusReport)
            {
                lstHeaders.AddRange(new []{
                    "PDI",
                    "PDI Name",
                    "Fahrzeugart",
                    "SIPP <br>Cd.",
                    "Hersteller Name",
                    "Model ID",
                    "Modellbezeichnung",
                    "Eing.<br>ges.",
                    "aus<br>Vorjahr",
                    "Zul.<br>Vorm.",
                    "Lfd.<br> Monat",
                    "Zul.<br> ges.",
                    "Proz.<br>lfd. Monat",
                    "Proz.<br>folg. Monat",
                    "Bestand",
                    "ausge-<br>ruestet",
                    "mit<br>Brief",
                    "zul.-<br>bereit",
                    "ohne<br>Unitnr."
                });
            }
            else
            {
                lstHeaders.AddRange(new[]{
                    "PDI",
                    "PDI Name",
                    "SIPP <br>Cd.",
                    "Gruppe",
                    "Hersteller Name",
                    "Model ID",
                    "Modellbezeichnung",
                    "Bestand",
                    "ausge-<br>ruestet",
                    "mit<br>Brief",
                    "zul.-<br>bereit",
                    "ohne<br>Unitnr.",
                    "gesperrt"
                });
            }

            var strBuild = new StringBuilder(String.Empty);

            strBuild.Append(String.Format(strHtmlStart, (ModusStatusReport ? "" : "Fahrzeugeinsteuerung")));
            strBuild.Append(strTableStart);

            strBuild.Append(String.Format(strLogo, lstHeaders.Count));

            var strHeader = strTrStart;
            foreach (var header in lstHeaders)
            {
                strHeader += String.Format(strHeaderTd, header);
            }
            strHeader += strTrEnd;

            for (var i = 0; i < StatusEinsteuerungsInklSummenzeilen.Count; i++)
            {
                var item = StatusEinsteuerungsInklSummenzeilen[i];

                var pdiName = item.PDIBezeichnung.NotNullOrEmpty().Replace("ä", "ae").Replace("ö", "oe").Replace("ü", "ue").Replace("Ö", "OE").Replace("Ä", "AE").Replace("Ü", "UE");

                if (i < (StatusEinsteuerungsInklSummenzeilen.Count - 2) || !ModusStatusReport)
                {
                    if (item.PDINummer.IsNullOrEmpty())
                        continue;

                    if (item.PDINummer != pdiOld)
                    {
                        if (pdiOld.IsNotNullOrEmpty())
                            strBuild.Append(String.Format(strHorizLine, lstHeaders.Count));

                        pdiOld = item.PDINummer;

                        if (item.PDINummer == "GESAMT")
                            strBuild.Append(String.Format(strHorizLine, lstHeaders.Count));

                        strBuild.Append(strHeader);
                    }

                    var strThisRowTd = (item.Fahrzeuggruppe.IsNullOrEmpty() || item.Sipp.IsNullOrEmpty() ? strRowTdBold : strRowTd);

                    strBuild.Append(strTrStart);
                    if (ModusStatusReport)
                    {
                        strBuild.Append(String.Format(strThisRowTd, item.PDINummer));
                        strBuild.Append(String.Format(strThisRowTd, pdiName));
                        strBuild.Append(String.Format(strThisRowTd, item.Fahrzeuggruppe));
                        strBuild.Append(String.Format(strThisRowTd, item.Sipp));
                        strBuild.Append(String.Format(strThisRowTd, item.Hersteller));
                        strBuild.Append(String.Format(strThisRowTd, item.ModellCode));
                        strBuild.Append(String.Format(strThisRowTd, item.Modellbezeichnung));
                        strBuild.Append(String.Format(strThisRowTd, item.EingangGesamt));
                        strBuild.Append(String.Format(strThisRowTd, item.AusVorjahr));
                        strBuild.Append(String.Format(strThisRowTd, item.ZulassungVormonat));
                        strBuild.Append(String.Format(strThisRowTd, item.ZulassungLfdMonat));
                        strBuild.Append(String.Format(strThisRowTd, item.ZulassungGesamtMonat));
                        strBuild.Append(String.Format(strThisRowTd, item.ZulassungProzLfdMonat));
                        strBuild.Append(String.Format(strThisRowTd, item.ZulassungProzFolgeMonat));
                        strBuild.Append(String.Format(strThisRowTd, item.Bestand));
                        strBuild.Append(String.Format(strThisRowTd, item.Ausgerüstet));
                        strBuild.Append(String.Format(strThisRowTd, item.MitBrief));
                        strBuild.Append(String.Format(strThisRowTd, item.Zulassungsbereit));
                        strBuild.Append(String.Format(strThisRowTd, item.OhneUnitnummer));
                    }
                    else
                    {
                        strBuild.Append(String.Format(strThisRowTd, item.PDINummer));
                        strBuild.Append(String.Format(strThisRowTd, pdiName));
                        strBuild.Append(String.Format(strThisRowTd, item.Sipp));
                        strBuild.Append(String.Format(strThisRowTd, item.Fahrzeuggruppe));
                        strBuild.Append(String.Format(strThisRowTd, item.Hersteller));
                        strBuild.Append(String.Format(strThisRowTd, item.ModellCode));
                        strBuild.Append(String.Format(strThisRowTd, item.Modellbezeichnung));
                        strBuild.Append(String.Format(strThisRowTd, item.Bestand));
                        strBuild.Append(String.Format(strThisRowTd, item.Ausgerüstet));
                        strBuild.Append(String.Format(strThisRowTd, item.MitBrief));
                        strBuild.Append(String.Format(strThisRowTd, item.Zulassungsbereit));
                        strBuild.Append(String.Format(strThisRowTd, item.OhneUnitnummer));
                        strBuild.Append(String.Format(strThisRowTd, item.Gesperrt));
                    }
                    strBuild.Append(strTrEnd);
                }
                else
                {
                    // für Statusreport die beiden letzten Zeilen gesondert verarbeiten
                    if (i < (StatusEinsteuerungsInklSummenzeilen.Count - 1))
                        strBuild.Append(String.Format(strHorizLine, lstHeaders.Count));

                    strBuild.Append(String.Format(strFooterPdi, item.PDINummer, lstHeaders.Count - 1, pdiName));
                }
            }

            strBuild.Append(strTableEnd);
            strBuild.Append(strHtmlEnd);

            return strBuild.ToString();
        }

        public DataTable GetReportDataAsDataTable()
        {
            var dt = StatusEinsteuerungsInklSummenzeilen.ToDataTable();

            if (ModusStatusReport)
            {
                dt.Columns.RemoveAt(4);
                dt.Columns.RemoveAt(19);
            }
            else
            {
                dt.Columns[2].SetOrdinal(3);
                dt.Columns.RemoveAt(4);
                for (var i = 0; i < 7; i++)
                {
                    dt.Columns.RemoveAt(7);
                }
            } 

            return dt;
        }

        #region Filter

        public void FilterStatusEinsteuerung(string filterValue, string filterProperties)
        {
            StatusEinsteuerungsFiltered = StatusEinsteuerungs.SearchPropertiesWithOrCondition(filterValue, filterProperties);
        }

        #endregion
    }
}
