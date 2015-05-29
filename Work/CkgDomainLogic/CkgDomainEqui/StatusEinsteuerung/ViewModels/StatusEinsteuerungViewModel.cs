using System.Collections.Generic;
using System.Linq;
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
        public List<StatusEinsteuerung> StatusEinsteuerungs
        {
            get { return PropertyCacheGet(() => new List<StatusEinsteuerung>()); }
            private set { PropertyCacheSet(value); }
        }

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
            get { return StatusEinsteuerungsFiltered.Count(s => s.Gesperrt == 1); }
        }

        public void Init()
        {
        
        }

        public void DataInit()
        {          
            DataMarkForRefresh();
        }

        public void DataMarkForRefresh()
        {
            PropertyCacheClear(this, m => m.StatusEinsteuerungsFiltered);
        }



        #region Baustelle 

        // TODO -> kann man das so machen (Bestand-Regel OK, aber die Summen ggf. wietere Kriterien wg. GESAMT-Zeile)
        // Das sollte anhand der Übersetzung zu ermitteln sein
        public void LoadStatusEinsteuerungOhneSummen()
        {
            StatusEinsteuerungs = DataService.GetStatusbericht().Where(s => s.Bestand > 0 && s.Sipp.IsNotNullOrEmpty() && s.ModellCode.IsNotNullOrEmpty()).ToList();
            DataMarkForRefresh();
        }

        public void LoadStatusberichtOhneSummen()
        {
            StatusEinsteuerungs = DataService.GetStatusbericht().Where(s => s.Sipp.IsNotNullOrEmpty() && s.ModellCode.IsNotNullOrEmpty()).ToList();
            DataMarkForRefresh();          
        }

        // Liefert den gesamten BAPI-Satz incl. Summen
        // werden vermutlich nicht explizit benötigt -> nur zur Demonstration
        List<StatusEinsteuerung> LoadStatusEinsteuerung4Export()
        {
            return DataService.GetStatusbericht().Where(s => s.Bestand > 0).ToList();                 
        }

        List<StatusEinsteuerung> LoadStatusbericht4Export()
        {
            return DataService.GetStatusbericht();                    
        }


        private void WriteData2Html()
        {
            // aus C:\dev\inetpub\wwwroot\Portal\Applications\appec\Forms\Report07.aspx.vb

            /*   
            
            Dim strTemplate As String
                Dim objFileInfo As System.IO.FileInfo
                Dim objStreamWriter As System.IO.StreamWriter
                Dim tblOutput As DataTable
                Dim row As DataRow
                Dim strDate As String
                Dim strTemp As String
                Dim strBuild As New System.Text.StringBuilder(String.Empty)
                Dim strFA As String = "<font size=""1"" style=""Arial""><b>"
                Dim strFZ As String = "</font></b>"

                Dim strFAA As String = "<font size=""1"" style=""Arial"">"
                Dim strFZZ As String = "</font>"
                Dim strPDIOld As String = ""

                Dim strBoldAA As String
                Dim strBoldZZ As String

                Dim strHeader As String

                Dim bbegin As Boolean = True

                tblOutput = CType(Session("ResultTable"), DataTable)
                strBuild.Append("<table cellspacing=""0"" cellpadding=""3"" border=""0"">")

                strBuild.Append("<tr><td colspan=""19"" align=""center""><img alt="""" src=""/Portal/Images/dadlogo.jpg"" />" & _
                                "</td></tr><tr><td colspan=""19"">&nbsp;</td></tr>")

                'Header
                strHeader = "<tr><td valign=""bottom"">" & strFA & "PDI" & strFZ & "</td>" & _
                            "<td valign=""bottom"">" & strFA & "PDI Name" & strFZ & "</td>" & _
                            "<td valign=""bottom"">" & strFA & "Fahrzeugart" & strFZ & "</td>" & _
                            "<td valign=""bottom"">" & strFA & "SIPP <br>Cd." & strFZ & "</td>" & _
                            "<td valign=""bottom"">" & strFA & "Hersteller Name" & strFZ & "</td>" & _
                            "<td valign=""bottom"">" & strFA & "Model ID" & strFZ & "</td>" & _
                            "<td valign=""bottom"">" & strFA & "Modellbezeichnung" & strFZ & "</td>" & _
                            "<td valign=""bottom"">" & strFA & "Eing.<br>ges." & strFZ & "</td>" & _
                            "<td valign=""bottom"">" & strFA & "aus<br>Vorjahr" & strFZ & "</td>" & _
                            "<td valign=""bottom"">" & strFA & "Zul.<br>Vorm." & strFZ & "</td>" & _
                            "<td valign=""bottom"">" & strFA & "Lfd.<br> Monat" & strFZ & "</td>" & _
                            "<td valign=""bottom"">" & strFA & "Zul.<br> ges." & strFZ & "</td>" & _
                            "<td valign=""bottom"">" & strFA & "Proz.<br>lfd. Monat" & strFZ & "</td>" & _
                            "<td valign=""bottom"">" & strFA & "Proz.<br>folg. Monat" & strFZ & "</td>" & _
                            "<td valign=""bottom"">" & strFA & "Bestand" & strFZ & "</td>" & _
                            "<td valign=""bottom"">" & strFA & "ausge-<br>ruestet" & strFZ & "</td>" & _
                            "<td valign=""bottom"">" & strFA & "mit<br>Brief" & strFZ & "</td>" & _
                            "<td valign=""bottom"">" & strFA & "zul.-<br>bereit" & strFZ & "</td>" & _
                            "<td valign=""bottom"">" & strFA & "ohne<br>Unitnr." & strFZ & "</td></tr>"
                strBuild.Append(strHeader)

                Dim LastRowIndex As Integer = tblOutput.Rows.Count
                Dim RowCount As Integer = 1
                For Each row In tblOutput.Rows

                    'die beiden letzten Zeilen gesondert verarbeiten
                    If RowCount < (LastRowIndex - 1) Then
                        'Einträge  
                        If (TypeOf row("PDI") Is System.DBNull) Then
                            ' strBuild.Append(strHeader)
                        Else
                            If bbegin = True Then
                                strPDIOld = row("PDI")
                            End If

                            row("PDI Name") = row("PDI Name").ToString.Replace("ä", "ae").Replace("ö", "oe").Replace("ü", "ue").Replace("Ö", "OE").Replace("Ä", "AE").Replace("Ü", "UE")


                            If Not (strPDIOld.Equals(row("PDI")) = True) Then
                                strTemp = " <tr><td colspan=""19""><hr></td></tr>"
                                strBuild.Append(strTemp)


                                If "GESAMT".Equals(row("PDI")) Then
                                    strTemp = " <tr><td colspan=""19""><hr></td></tr>"
                                    strBuild.Append(strTemp)
                                End If

                                strBuild.Append(strHeader)

                            End If


                            If (row("PDI") = "----------") Then
                                row("PDI") = String.Empty
                                row("PDI Name") = String.Empty
                                row("Hersteller Name") = String.Empty
                                row("Model ID") = String.Empty
                                row("Modellbezeichnung") = String.Empty
                                'strTemp = "<tr><td>" & strFAA & row("PDI") & strFZZ & "</td>"
                                'strBoldAA = "<b>"
                                'strBoldZZ = "</b>"
                                'Else
                                '    strTemp = "<tr><td>" & strFAA & row("PDI") & strFZZ & "</td>"
                                '    strBoldAA = String.Empty
                                '    strBoldZZ = String.Empty
                            End If


                            If String.Empty.Equals(row("Fahrzeugart")) = True Or String.Empty.Equals(row("SIPP Code")) = True Then
                                strBoldAA = "<b>"
                                strBoldZZ = "</b>"
                            Else
                                strBoldAA = String.Empty
                                strBoldZZ = String.Empty
                            End If




                            strTemp = "<tr><td>" & strBoldAA & strFAA & row("PDI") & strFZZ & strBoldZZ & "</td>"
                            strBuild.Append(strTemp)
                            strTemp = "<td>" & strBoldAA & strFAA & row("PDI Name") & strFZZ & strBoldZZ & "</td>"
                            strBuild.Append(strTemp)
                            strTemp = "<td>" & strBoldAA & strFAA & row("Fahrzeugart") & strFZZ & strBoldZZ & "</td>"
                            strBuild.Append(strTemp)
                            strTemp = "<td>" & strBoldAA & strFAA & row("SIPP Code") & strFZZ & strBoldZZ & "</td>"
                            strBuild.Append(strTemp)
                            strTemp = "<td>" & strBoldAA & strFAA & row("Hersteller Name") & strFZZ & strBoldZZ & "</td>"
                            strBuild.Append(strTemp)
                            strTemp = "<td>" & strBoldAA & strFAA & row("Model ID") & strFZZ & strBoldZZ & "</td>"
                            strBuild.Append(strTemp)
                            strTemp = "<td>" & strBoldAA & strFAA & row("Modellbezeichnung") & strFZZ & strBoldZZ & "</td>"
                            strBuild.Append(strTemp)
                            strTemp = "<td>" & strBoldAA & strFAA & row("Eing ges") & strFZZ & strBoldZZ & "</td>"
                            strBuild.Append(strTemp)
                            strTemp = "<td>" & strBoldAA & strFAA & row("aus Vorjahr") & strFZZ & strBoldZZ & "</td>"
                            strBuild.Append(strTemp)
                            strTemp = "<td>" & strBoldAA & strFAA & row("Zul Vorm") & strFZZ & strBoldZZ & "</td>"
                            strBuild.Append(strTemp)
                            strTemp = "<td>" & strBoldAA & strFAA & row("Lfd Monat") & strFZZ & strBoldZZ & "</td>"
                            strBuild.Append(strTemp)
                            strTemp = "<td>" & strBoldAA & strFAA & row("Zul ges") & strFZZ & strBoldZZ & "</td>"
                            strBuild.Append(strTemp)
                            strTemp = "<td>" & strBoldAA & strFAA & row("Proz lfd Monat") & strFZZ & strBoldZZ & "</td>"
                            strBuild.Append(strTemp)
                            strTemp = "<td>" & strBoldAA & strFAA & row("Proz folg Monat") & strFZZ & strBoldZZ & "</td>"
                            strBuild.Append(strTemp)
                            strTemp = "<td>" & strBoldAA & strFAA & row("Bestand") & strFZZ & strBoldZZ & "</td>"
                            strBuild.Append(strTemp)
                            strTemp = "<td>" & strBoldAA & strFAA & row("ausgerüstet") & strFZZ & strBoldZZ & "</td>"
                            strBuild.Append(strTemp)
                            strTemp = "<td>" & strBoldAA & strFAA & row("mit Brief") & strFZZ & strBoldZZ & "</td>"
                            strBuild.Append(strTemp)
                            strTemp = "<td>" & strBoldAA & strFAA & row("zul-bereit") & strFZZ & strBoldZZ & "</td>"
                            strBuild.Append(strTemp)
                            strTemp = "<td>" & strBoldAA & strFAA & row("ohne Unitnr") & strFZZ & strBoldZZ & "</td></tr>"
                            strBuild.Append(strTemp)
                            ''Trennstrich einfügen...
                            'If (row("PDI") = String.Empty) Then
                            '    strBuild.Append("<tr><td colspan=""18""><hr></td></tr>")
                            'End If
                            bbegin = False
                            strPDIOld = row("PDI")
                        End If
                    Else
                        'zwischen den einzelnen Summenzeilen keine Trennstriche, nur darüber einen
                        If Not RowCount = LastRowIndex Then
                            strTemp = " <tr><td colspan=""19""><hr></td></tr>"
                            strBuild.Append(strTemp)
                        End If
                        strTemp = " <tr><td style=""font-size:10; font-weight:bold"">" & row("PDI").ToString & ":</td><td colspan=""17"" style=""font-size:10; font-weight:bold"">" & row("PDI Name").ToString & "</td> </tr>"
                        strBuild.Append(strTemp)
                    End If
                    RowCount += 1
                Next
                strBuild.Append("</table>")

                strTemplate = "<html><head><title>Statusbericht (Druckversion)</title></head><body bgcolor=""#FFFFFF"">" & strBuild.ToString & "</body></html>"

                strDate = Date.Now.Year.ToString & Date.Now.Month.ToString & Date.Now.Day.ToString & Date.Now.Hour.ToString & Date.Now.Minute.ToString & Date.Now.Second.ToString
                objFileInfo = New System.IO.FileInfo(ConfigurationManager.AppSettings("ExcelPath") & "StatusberichtEC" & strDate & ".htm")

                objStreamWriter = objFileInfo.CreateText()
                objStreamWriter.Write(strTemplate)
                objStreamWriter.Close()

                'änderung Excel Pfade JJ2007.12.14
                Dim AbsoluterPfadZumVirituellenVerz As String = ConfigurationManager.AppSettings("ReplaceExcelPath")
                Hyperlink1.NavigateUrl = ConfigurationManager.AppSettings("ExcelPath").Replace(AbsoluterPfadZumVirituellenVerz, "") & "StatusberichtEC" & strDate & ".htm".Replace("/", "\")
                Hyperlink1.Visible = True

            */

            // Übersetzung Teil 1 Header

                string strTemplate;
                System.IO.FileInfo objFileInfo;
                System.IO.StreamWriter objStreamWriter;
                string strDate;
                string strTemp;
                var strBuild = new System.Text.StringBuilder(string.Empty);
                string strFA = "<font size=\"1\" style=\"Arial\"><b>";
                string strFZ = "</font></b>";
                string strFAA = "<font size=\"1\" style=\"Arial\">";
                string strFZZ = "</font>";
                string strPDIOld = "";
                string strBoldAA;
                string strBoldZZ;
                string strHeader;
                bool bbegin = true;


            strBuild.Append("<table cellspacing=\"0\" cellpadding=\"3\" border=\"0\">");

            strBuild.Append(
                "<tr><td colspan=\"19\" align=\"center\"><img alt=\"\" src=\"/Portal/Images/dadlogo.jpg\" />" +
                "</td></tr><tr><td colspan=\"19\">&nbsp;</td></tr>");

            //Header
            strHeader = "<tr><td valign=\"bottom\">" + strFA + "PDI" + strFZ + "</td>" + 
                        "<td valign=\"bottom\">" + strFA + "PDI Name" + strFZ + "</td>" + 
                        "<td valign=\"bottom\">" + strFA + "Fahrzeugart" + strFZ + "</td>" + 
                        "<td valign=\"bottom\">" + strFA + "SIPP <br>Cd." + strFZ + "</td>" + 
                        "<td valign=\"bottom\">" + strFA + "Hersteller Name" + strFZ + "</td>" + 
                        "<td valign=\"bottom\">" + strFA + "Model ID" + strFZ + "</td>" + 
                        "<td valign=\"bottom\">" + strFA + "Modellbezeichnung" + strFZ + "</td>" + 
                        "<td valign=\"bottom\">" + strFA + "Eing.<br>ges." + strFZ + "</td>" + 
                        "<td valign=\"bottom\">" + strFA + "aus<br>Vorjahr" + strFZ + "</td>" + 
                        "<td valign=\"bottom\">" + strFA + "Zul.<br>Vorm." + strFZ + "</td>" + 
                        "<td valign=\"bottom\">" + strFA + "Lfd.<br> Monat" + strFZ + "</td>" + 
                        "<td valign=\"bottom\">" + strFA + "Zul.<br> ges." + strFZ + "</td>" + 
                        "<td valign=\"bottom\">" + strFA + "Proz.<br>lfd. Monat" + strFZ + "</td>" +
                        "<td valign=\"bottom\">" + strFA + "Proz.<br>folg. Monat" + strFZ + "</td>" + 
                        "<td valign=\"bottom\">" + strFA + "Bestand" + strFZ + "</td>" + 
                        "<td valign=\"bottom\">" + strFA + "ausge-<br>ruestet" + strFZ + "</td>" + 
                        "<td valign=\"bottom\">" + strFA + "mit<br>Brief" + strFZ + "</td>" + 
                        "<td valign=\"bottom\">" + strFA + "zul.-<br>bereit" + strFZ + "</td>" + 
                        "<td valign=\"bottom\">" + strFA + "ohne<br>Unitnr." + strFZ + "</td></tr>";

            strBuild.Append(strHeader);

            // Teil 2 -> die Table

            // -> zwischen beiden reports muss noch unterschieden werden
            var items = DataService.GetStatusbericht();

            
            int LastRowIndex = items.Count();
            int RowCount = 1;


            foreach (var item in items)
            {

                //  'die beiden letzten Zeilen gesondert verarbeiten
                if(RowCount < (LastRowIndex - 1))
                {
                    if(bbegin) strPDIOld = item.PDINummer;

                    item.PDIBezeichnung =
                        item.PDIBezeichnung.ToString()
                            .Replace("ä", "ae")
                            .Replace("ö", "oe")
                            .Replace("ü", "ue")
                            .Replace("Ö", "OE")
                            .Replace("Ä", "AE")
                            .Replace("Ü", "UE"); // -> refactor

                    if (!strPDIOld.Equals(item.PDINummer))
                    {
                        strTemp = " <tr><td colspan=\"19\"><hr></td></tr>";
                        strBuild.Append(strTemp);
                    }

                    if("GESAMT".Equals(item.PDINummer))
                    {
                        strTemp = " <tr><td colspan=\"19\"><hr></td></tr>";
                        strBuild.Append(strTemp);
                    }

                    strBuild.Append(strHeader);

                    // das heist hier lässt sich auch die Logik für das Lesen des Bapis entnehmen
                    // -> PDI = GESAMT -> Summe über alles


                }
                else
                {
                    // die überigen Zeilen behandeln -> TODO
                }

                // -> besser separieren statt Riesenschleifen
            }

           
        }

        #endregion Baustelle


        #region filter

        public void FilterStatusEinsteuerung(string filterValue, string filterProperties)
        {
            StatusEinsteuerungsFiltered = StatusEinsteuerungs.SearchPropertiesWithOrCondition(filterValue, filterProperties);
        }

        #endregion


    }
}
