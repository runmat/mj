using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using AppRemarketing.lib;
using CKG.Base.Business;
using CKG.Base.Kernel.Common;
using CKG.Base.Kernel.Security;
using GeneralTools.Services;
using SmartSoft.PdfLibrary;

namespace AppRemarketing.forms
{
    public partial class Report14_2 : System.Web.UI.Page
    {
        private User _mUser;
        private App _mApp;
        private Historie _mReport;
        private ABEDaten _typ;

        protected void Page_Load(object sender, EventArgs e)
        {
            _mUser = Common.GetUser(this);

            Common.FormAuth(this, _mUser);

            _mApp = new App(_mUser);

            Common.GetAppIDFromQueryString(this);

            lblHead.Text = (string)_mUser.Applications.Select("AppID = '" + Session["AppID"] + "'")[0]["AppFriendlyName"];

            if (!IsPostBack)
            {
                if (Request.QueryString["Linked"] != null && Request.QueryString["Linked"] != string.Empty)
                {
                    lbBack.Text = "schließen";
                }

                Fill();
            }
        }

        private void Fill()
        {
            _mReport = (Historie)Session["Historie"];

            //Oberste Zeile Übersicht
            lblFahrgestellnummerShow.Text = (String)_mReport.CommonData.Rows[0]["FAHRGNR"];
            lblKennzeichenShow.Text = (String)_mReport.CommonData.Rows[0]["KENNZ"];
            lblBriefnummerShow.Text = (String)_mReport.CommonData.Rows[0]["BRIEFNR"];
            //Ereignisart nur anzeigen, wenn gesetzt
            string strEreignisText = (string)_mReport.CommonData.Rows[0]["EREIGNIS_TEXT"];
            if ((String.IsNullOrEmpty(strEreignisText)) || (strEreignisText.Equals("Kein Eintrag")))
            {
                lblEreignisartShow.Text = "";
            }
            else
            {
                lblEreignisartShow.Text = (string)_mReport.CommonData.Rows[0]["EREIGNIS_TEXT"];
            }

            //Brief- und Schlüsseldaten
            lblEingangBriefShow.Text = CheckDate(_mReport.CommonData.Rows[0]["EGZB2DAT"].ToString());
            lblEingangSchlueShow.Text = CheckDate(_mReport.CommonData.Rows[0]["EGZWSLDAT"].ToString());

            if (_mReport.Versand.Rows.Count > 0)
            {
                lblVersandBriefShow.Text = CheckDate(_mReport.Versand.Rows[0]["B_VERSAUFTR_DAT"].ToString());
                lblVersandSchlueShow.Text = CheckDate(_mReport.Versand.Rows[0]["T_VERSAUFTR_DAT"].ToString());

                lblExBelegNrShow.Text = FillWithSpace((String)_mReport.Versand.Rows[0]["BELNR"]);
                lblRechBetragShow.Text = _mReport.Versand.Rows[0]["BETRAG_RE"].ToString();
                lblBelegdatumShow.Text = CheckDate(_mReport.Versand.Rows[0]["BELDT"].ToString());
                lblValutaShow.Text = CheckDate(_mReport.Versand.Rows[0]["VALDT"].ToString());
                lblFreigabedatumShow.Text = CheckDate(_mReport.Versand.Rows[0]["RELDT"].ToString());
                lblZahlungsartShow.Text = FillWithSpace((String)_mReport.Versand.Rows[0]["DZLART"]);

                lblHaendlerShow.Text = FillWithSpace((String)_mReport.Versand.Rows[0]["RDEALER"]);
                lblHaName1Show.Text = FillWithSpace((String)_mReport.Versand.Rows[0]["NAME1_ZF"]);
                lblHaName2Show.Text = FillWithSpace((String)_mReport.Versand.Rows[0]["NAME2_ZF"]);
                lblHaName3Show.Text = FillWithSpace((String)_mReport.Versand.Rows[0]["NAME3_ZF"]);
                lblHaStrasseShow.Text = FillWithSpace((String)_mReport.Versand.Rows[0]["STREET_ZF"]);
                lblHaPLZShow.Text = FillWithSpace((String)_mReport.Versand.Rows[0]["POST_CODE1_ZF"]);
                lblHaOrtShow.Text = FillWithSpace((String)_mReport.Versand.Rows[0]["CITY1_ZF"]);
                lblHaLandShow.Text = FillWithSpace((String)_mReport.Versand.Rows[0]["LAND_BEZ_ZF"]);

                lblBaName1Show.Text = FillWithSpace((String)_mReport.Versand.Rows[0]["NAME1_BANK"]);
                lblBaName2Show.Text = FillWithSpace((String)_mReport.Versand.Rows[0]["NAME2_BANK"]);
                lblBaName3Show.Text = FillWithSpace((String)_mReport.Versand.Rows[0]["NAME3_BANK"]);
                lblBaStrasseShow.Text = FillWithSpace((String)_mReport.Versand.Rows[0]["STREET_BANK"]);
                lblBaPLZShow.Text = FillWithSpace((String)_mReport.Versand.Rows[0]["POST_CODE1_BANK"]);
                lblBaOrtShow.Text = FillWithSpace((String)_mReport.Versand.Rows[0]["CITY1_BANK"]);
                lblBaLandShow.Text = FillWithSpace((String)_mReport.Versand.Rows[0]["LAND_BEZ_BANK"]);
            }

            //Typdaten
            String space = "";
            String kunnr = _mUser.KUNNR;
            String zdad = "ZDAD";

            _typ = new ABEDaten(ref _mUser, ref _mApp, (string)Session["AppID"], Session.SessionID, "", ref kunnr, ref zdad, ref space, ref space);

            _typ.FillDatenABE((string)Session["AppID"], Session.SessionID, Page, _mReport.CommonData.Rows[0]["EQUNR"].ToString());

            lbl_0.Text = _typ.ABE_Daten.ZZKLARTEXT_TYP;
            lbl_1.Text = _typ.ABE_Daten.ZZHERST_TEXT;
            lbl_2.Text = _typ.ABE_Daten.ZZHERSTELLER_SCH;
            lbl_3.Text = _typ.ABE_Daten.ZZHANDELSNAME;
            lbl_4.Text = _typ.ABE_Daten.ZZGENEHMIGNR;
            lbl_5.Text = _typ.ABE_Daten.ZZGENEHMIGDAT;
            lbl_6.Text = _typ.ABE_Daten.ZZFHRZKLASSE_TXT;
            lbl_7.Text = _typ.ABE_Daten.ZZTEXT_AUFBAU;
            lbl_8.Text = _typ.ABE_Daten.ZZFABRIKNAME;
            lbl_9.Text = _typ.ABE_Daten.ZZVARIANTE;
            lbl_10.Text = _typ.ABE_Daten.ZZVERSION;
            lbl_11.Text = _typ.ABE_Daten.ZZHUBRAUM.TrimStart('0');
            lbl_13.Text = _typ.ABE_Daten.ZZNENNLEISTUNG.TrimStart('0');
            lbl_14.Text = _typ.ABE_Daten.ZZBEIUMDREH.TrimStart('0');
            lbl_12.Text = _typ.ABE_Daten.ZZHOECHSTGESCHW;
            lbl_19.Text = _typ.ABE_Daten.ZZSTANDGERAEUSCH.TrimStart('0');
            lbl_20.Text = _typ.ABE_Daten.ZZFAHRGERAEUSCH.TrimStart('0');
            lbl_15.Text = _typ.ABE_Daten.ZZKRAFTSTOFF_TXT;
            lbl_16.Text = _typ.ABE_Daten.ZZCODE_KRAFTSTOF;
            lbl_21.Text = _typ.ABE_Daten.ZZFASSVERMOEGEN;
            lbl_17.Text = _typ.ABE_Daten.ZZCO2KOMBI;
            lbl_18.Text = _typ.ABE_Daten.ZZSLD + " / " + _typ.ABE_Daten.ZZNATIONALE_EMIK;
            lbl_22.Text = _typ.ABE_Daten.ZZABGASRICHTL_TG;
            lbl_23.Text = _typ.ABE_Daten.ZZANZACHS.TrimStart('0');
            lbl_24.Text = _typ.ABE_Daten.ZZANTRIEBSACHS.TrimStart('0');
            lbl_26.Text = _typ.ABE_Daten.ZZANZSITZE.TrimStart('0');
            var achslast = new[] { _typ.ABE_Daten.ZZACHSL_A1_STA.TrimStart('0'), _typ.ABE_Daten.ZZACHSL_A2_STA.TrimStart('0'), _typ.ABE_Daten.ZZACHSL_A3_STA.TrimStart('0') }.Where(a => !string.IsNullOrEmpty(a)).ToArray();
            lbl_25.Text = string.Join(", ", achslast);
            var bereifung = new[]{_typ.ABE_Daten.ZZBEREIFACHSE1 ,_typ.ABE_Daten.ZZBEREIFACHSE2 ,_typ.ABE_Daten.ZZBEREIFACHSE3}.Where(b => !string.IsNullOrEmpty(b)).ToArray();
            lbl_27.Text = string.Join(", ", bereifung);
            lbl_28.Text = _typ.ABE_Daten.ZZZULGESGEW.TrimStart('0');
            lbl_29.Text = _typ.ABE_Daten.ZZTYP_SCHL;
            lbl_30.Text = string.Join("<br>", new[] { 
                _typ.ABE_Daten.ZZBEMER1, _typ.ABE_Daten.ZZBEMER2, _typ.ABE_Daten.ZZBEMER3, _typ.ABE_Daten.ZZBEMER4, 
                _typ.ABE_Daten.ZZBEMER5, _typ.ABE_Daten.ZZBEMER6, _typ.ABE_Daten.ZZBEMER7, _typ.ABE_Daten.ZZBEMER8, 
                _typ.ABE_Daten.ZZBEMER9, _typ.ABE_Daten.ZZBEMER10, _typ.ABE_Daten.ZZBEMER11, _typ.ABE_Daten.ZZBEMER12, 
                _typ.ABE_Daten.ZZBEMER13, _typ.ABE_Daten.ZZBEMER14 });
            lbl_31.Text = _typ.ABE_Daten.ZZLAENGEMIN.TrimStart('0');
            lbl_32.Text = _typ.ABE_Daten.ZZBREITEMIN.TrimStart('0');
            lbl_33.Text = _typ.ABE_Daten.ZZHOEHEMIN;

            lbl_00.Text = _typ.ABE_Daten.ZZFARBE + " (" + _typ.ABE_Daten.Farbziffer + ")";
            lbl_55.Text = "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;";
            lbl_91.Text = "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;";
            lbl_92.Text = "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;";
            lbl_93.Text = "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;";
            lbl_94.Text = "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;";
            lbl_95.Text = "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;";
            lbl_96.Text = "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;";
            lbl_97.Text = "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;";
            lbl_98.Text = "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;";
            lbl_99.Text = "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;";

            lbl_99.Visible = false;
            lbl_98.Visible = false;
            lbl_97.Visible = false;
            lbl_96.Visible = false;
            lbl_95.Visible = false;
            lbl_94.Visible = false;
            lbl_93.Visible = false;
            lbl_92.Visible = false;
            lbl_91.Visible = false;
            lbl_55.Visible = false;

            switch (_typ.ABE_Daten.Farbziffer)
            {
                case "0":
                    lbl_99.Visible = true;
                    break;
                case "1":
                    lbl_98.Visible = true;
                    break;
                case "2":
                    lbl_97.Visible = true;
                    break;
                case "3":
                    lbl_96.Visible = true;
                    break;
                case "4":
                    lbl_95.Visible = true;
                    break;
                case "5":
                    lbl_94.Visible = true;
                    break;
                case "6":
                    lbl_93.Visible = true;
                    break;
                case "7":
                    lbl_92.Visible = true;
                    break;
                case "8":
                    lbl_91.Visible = true;
                    break;
                case "9":
                    lbl_55.Visible = true;
                    break;
            }

            // Lebenslauf
            LebenslaufRepeater.DataSource = _mReport.Lebenslauf;
            LebenslaufRepeater.DataBind();

            //Vorschäden
            VorschadenRepeater.DataSource = _mReport.Vorschaden;
            VorschadenRepeater.DataBind();

            // -> im aspx gebundene Felder befüllen (z.B. Belastungsanzeige)
            Page.DataBind();
        }

        private String CheckDate(String conDate)
        {
            if (conDate.Length > 0)
            {
                return conDate.Substring(0, 10);
            }

            return "&nbsp;";
        }

        private String FillWithSpace(String conString)
        {
            if (conString.Length > 0)
            {
                return conString;
            }

            return "&nbsp;";
        }

        protected void lbBack_Click(object sender, EventArgs e)
        {

            Session["Historie"] = null;

            if (Request.QueryString["Linked"] !=null)
            {
                string strscript = "<script language=javascript>window.top.close();</script>";

                if (!ClientScript.IsStartupScriptRegistered("clientScript"))
                {
                    ClientScript.RegisterStartupScript(GetType(), "clientScript", strscript);
                }

                return;
            }

            Response.Redirect("Report14.aspx?AppID=" + (string)Session["AppID"]);
        }

        protected HistorieBelastungsanzeige Belastungsanzeige { get { return _mReport == null ? null : _mReport.Belastungsanzeige; } }
        protected HistorieUebersicht Uebersicht { get { return _mReport == null ? null : _mReport.Uebersicht; } }
        protected HistorieLinks Links { get { return _mReport == null ? null : _mReport.Links; } }

        protected void ShowBelastungsanzeige(object sender, EventArgs e)
        {
            _mReport = (Historie)Session["Historie"];
            if (Links == null) return;

            Links.OpenBelastungsanzeige(ShowReportHelper, this);
        }

        protected void ShowSchadensgutachten(object sender, EventArgs e)
        {
            _mReport = (Historie)Session["Historie"];
            if (Links == null) return;

            Links.OpenSchadensgutachten(ShowReportHelper, this);
        }

        protected void ShowRechnung(object sender, EventArgs e)
        {
            _mReport = (Historie)Session["Historie"];
            if (Links == null) return;

            Links.OpenRechnung(ShowReportHelper, this);
        }

        protected void ShowRepKalk(object sender, EventArgs e)
        {
            _mReport = (Historie)Session["Historie"];
            if (Links == null) return;

            var repKalkUrl = ApplicationConfiguration.GetApplicationConfigValue("RepKalkUrl", "0", _mUser.Customer.CustomerId, _mUser.GroupID);
            var filesByte = new List<byte[]>();

            using (var clnt = new WebClient())
            {
                for (var i = 0; i < Links.AnzahlRepKalk; i++)
                {
                    var downloadUrl = String.Format("{0}?fin={1}&nummer={2}", repKalkUrl, Links.FahrgestellNr, (i + 1).ToString());
                    var downloadFile = clnt.DownloadData(downloadUrl);
                    if (downloadFile != null)
                        filesByte.Add(downloadFile);
                }
            }

            if (filesByte.Count == 0)
            {
                lblError.Text = "Das Dokument wurde nicht gefunden.";
                return;
            }

            var pdfBytes = PdfMerger.MergeFiles(filesByte, false);

            Response.Clear();
            Response.ClearContent();
            Response.ClearHeaders();
            Response.ContentType = "application/pdf";
            Response.AddHeader("Content-Disposition", "attachment; filename=" + "RepKalk_" + Links.FahrgestellNr + ".pdf");
            Response.AddHeader("Expires", "0");
            Response.AddHeader("Pragma", "cache");
            Response.AddHeader("Cache-Control", "private");
            Response.BinaryWrite(pdfBytes);
            if (Response.IsClientConnected)
            {
                Response.Flush();
            }
            Response.End();
        }
    }
}