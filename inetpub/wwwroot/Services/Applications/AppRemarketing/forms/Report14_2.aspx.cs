using System;
using System.Linq;
using AppRemarketing.lib;
using CKG.Base.Business;
using CKG.Base.Kernel.Common;
using CKG.Base.Kernel.Security;

namespace AppRemarketing.forms
{
    public partial class Report14_2 : System.Web.UI.Page
    {
        private CKG.Base.Kernel.Security.User m_User;
        private CKG.Base.Kernel.Security.App m_App;
        private Historie m_report;
        private ABEDaten Typ;

        protected void Page_Load(object sender, EventArgs e)
        {
            m_User = Common.GetUser(this);

            Common.FormAuth(this, m_User);

            m_App = new App(m_User);

            Common.GetAppIDFromQueryString(this);

            lblHead.Text = (string)m_User.Applications.Select("AppID = '" + Session["AppID"] + "'")[0]["AppFriendlyName"];

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
            m_report = (Historie)Session["Historie"];

            //Oberste Zeile Übersicht
            lblFahrgestellnummerShow.Text = (String)m_report.CommonData.Rows[0]["FAHRGNR"];
            lblKennzeichenShow.Text = (String)m_report.CommonData.Rows[0]["KENNZ"];
            lblBriefnummerShow.Text = (String)m_report.CommonData.Rows[0]["BRIEFNR"];
            //Ereignisart nur anzeigen, wenn gesetzt
            string strEreignisText = (string)m_report.CommonData.Rows[0]["EREIGNIS_TEXT"];
            if ((String.IsNullOrEmpty(strEreignisText)) || (strEreignisText.Equals("Kein Eintrag")))
            {
                lblEreignisartShow.Text = "";
            }
            else
            {
                lblEreignisartShow.Text = (string)m_report.CommonData.Rows[0]["EREIGNIS_TEXT"];
            }
            //lblLizNrShow.Text = (String)m_report.CommonData.Rows[0]["LIZNR"];

            //Brief- und Schlüsseldaten
            lblEingangBriefShow.Text = CheckDate(m_report.CommonData.Rows[0]["EGZB2DAT"].ToString());
            lblEingangSchlueShow.Text = CheckDate(m_report.CommonData.Rows[0]["EGZWSLDAT"].ToString());

            if (m_report.Versand.Rows.Count > 0)
            {
                lblVersandBriefShow.Text = CheckDate(m_report.Versand.Rows[0]["B_VERSAUFTR_DAT"].ToString());
                lblVersandSchlueShow.Text = CheckDate(m_report.Versand.Rows[0]["T_VERSAUFTR_DAT"].ToString());

                lblExBelegNrShow.Text = FillWithSpace((String)m_report.Versand.Rows[0]["BELNR"]);
                lblRechBetragShow.Text = m_report.Versand.Rows[0]["BETRAG_RE"].ToString();
                lblBelegdatumShow.Text = CheckDate(m_report.Versand.Rows[0]["BELDT"].ToString());
                lblValutaShow.Text = CheckDate(m_report.Versand.Rows[0]["VALDT"].ToString());
                lblFreigabedatumShow.Text = CheckDate(m_report.Versand.Rows[0]["RELDT"].ToString());
                lblZahlungsartShow.Text = FillWithSpace((String)m_report.Versand.Rows[0]["DZLART"]);

                lblHaendlerShow.Text = FillWithSpace((String)m_report.Versand.Rows[0]["RDEALER"]);
                lblHaName1Show.Text = FillWithSpace((String)m_report.Versand.Rows[0]["NAME1_ZF"]);
                lblHaName2Show.Text = FillWithSpace((String)m_report.Versand.Rows[0]["NAME2_ZF"]);
                lblHaName3Show.Text = FillWithSpace((String)m_report.Versand.Rows[0]["NAME3_ZF"]);
                lblHaStrasseShow.Text = FillWithSpace((String)m_report.Versand.Rows[0]["STREET_ZF"]);
                lblHaPLZShow.Text = FillWithSpace((String)m_report.Versand.Rows[0]["POST_CODE1_ZF"]);
                lblHaOrtShow.Text = FillWithSpace((String)m_report.Versand.Rows[0]["CITY1_ZF"]);
                lblHaLandShow.Text = FillWithSpace((String)m_report.Versand.Rows[0]["LAND_BEZ_ZF"]);

                lblBaName1Show.Text = FillWithSpace((String)m_report.Versand.Rows[0]["NAME1_BANK"]);
                lblBaName2Show.Text = FillWithSpace((String)m_report.Versand.Rows[0]["NAME2_BANK"]);
                lblBaName3Show.Text = FillWithSpace((String)m_report.Versand.Rows[0]["NAME3_BANK"]);
                lblBaStrasseShow.Text = FillWithSpace((String)m_report.Versand.Rows[0]["STREET_BANK"]);
                lblBaPLZShow.Text = FillWithSpace((String)m_report.Versand.Rows[0]["POST_CODE1_BANK"]);
                lblBaOrtShow.Text = FillWithSpace((String)m_report.Versand.Rows[0]["CITY1_BANK"]);
                lblBaLandShow.Text = FillWithSpace((String)m_report.Versand.Rows[0]["LAND_BEZ_BANK"]);
            }


            //Typdaten
            String Space = "";
            String KUNNR = m_User.KUNNR.ToString();
            String ZDAD = "ZDAD";

            Typ = new ABEDaten(ref m_User, ref m_App, (string)Session["AppID"], (string)Session.SessionID, "", ref KUNNR, ref ZDAD, ref Space, ref Space);

            Typ.FillDatenABE((string)Session["AppID"], Session.SessionID.ToString(), this.Page, m_report.CommonData.Rows[0]["EQUNR"].ToString());

            lbl_0.Text = Typ.ABE_Daten.ZZKLARTEXT_TYP;
            lbl_1.Text = Typ.ABE_Daten.ZZHERST_TEXT;
            lbl_2.Text = Typ.ABE_Daten.ZZHERSTELLER_SCH;
            lbl_3.Text = Typ.ABE_Daten.ZZHANDELSNAME;
            lbl_4.Text = Typ.ABE_Daten.ZZGENEHMIGNR;
            lbl_5.Text = Typ.ABE_Daten.ZZGENEHMIGDAT;
            lbl_6.Text = Typ.ABE_Daten.ZZFHRZKLASSE_TXT;
            lbl_7.Text = Typ.ABE_Daten.ZZTEXT_AUFBAU;
            lbl_8.Text = Typ.ABE_Daten.ZZFABRIKNAME;
            lbl_9.Text = Typ.ABE_Daten.ZZVARIANTE;
            lbl_10.Text = Typ.ABE_Daten.ZZVERSION;
            lbl_11.Text = Typ.ABE_Daten.ZZHUBRAUM.TrimStart('0');
            lbl_13.Text = Typ.ABE_Daten.ZZNENNLEISTUNG.TrimStart('0');
            lbl_14.Text = Typ.ABE_Daten.ZZBEIUMDREH.TrimStart('0');
            lbl_12.Text = Typ.ABE_Daten.ZZHOECHSTGESCHW;
            lbl_19.Text = Typ.ABE_Daten.ZZSTANDGERAEUSCH.TrimStart('0');
            lbl_20.Text = Typ.ABE_Daten.ZZFAHRGERAEUSCH.TrimStart('0');
            lbl_15.Text = Typ.ABE_Daten.ZZKRAFTSTOFF_TXT;
            lbl_16.Text = Typ.ABE_Daten.ZZCODE_KRAFTSTOF;
            lbl_21.Text = Typ.ABE_Daten.ZZFASSVERMOEGEN;
            lbl_17.Text = Typ.ABE_Daten.ZZCO2KOMBI;
            lbl_18.Text = Typ.ABE_Daten.ZZSLD + " / " + Typ.ABE_Daten.ZZNATIONALE_EMIK;
            lbl_22.Text = Typ.ABE_Daten.ZZABGASRICHTL_TG;
            lbl_23.Text = Typ.ABE_Daten.ZZANZACHS.TrimStart('0');
            lbl_24.Text = Typ.ABE_Daten.ZZANTRIEBSACHS.TrimStart('0');
            lbl_26.Text = Typ.ABE_Daten.ZZANZSITZE.TrimStart('0');
            var achslast = new[] { Typ.ABE_Daten.ZZACHSL_A1_STA.TrimStart('0'), Typ.ABE_Daten.ZZACHSL_A2_STA.TrimStart('0'), Typ.ABE_Daten.ZZACHSL_A3_STA.TrimStart('0') }.Where(a => !string.IsNullOrEmpty(a)).ToArray();
            lbl_25.Text = string.Join(", ", achslast);
            var bereifung = new[]{Typ.ABE_Daten.ZZBEREIFACHSE1 ,Typ.ABE_Daten.ZZBEREIFACHSE2 ,Typ.ABE_Daten.ZZBEREIFACHSE3}.Where(b => !string.IsNullOrEmpty(b)).ToArray();
            lbl_27.Text = string.Join(", ", bereifung);
            lbl_28.Text = Typ.ABE_Daten.ZZZULGESGEW.TrimStart('0');
            lbl_29.Text = Typ.ABE_Daten.ZZTYP_SCHL;
            lbl_30.Text = string.Join("<br>", new[] { 
                Typ.ABE_Daten.ZZBEMER1, Typ.ABE_Daten.ZZBEMER2, Typ.ABE_Daten.ZZBEMER3, Typ.ABE_Daten.ZZBEMER4, 
                Typ.ABE_Daten.ZZBEMER5, Typ.ABE_Daten.ZZBEMER6, Typ.ABE_Daten.ZZBEMER7, Typ.ABE_Daten.ZZBEMER8, 
                Typ.ABE_Daten.ZZBEMER9, Typ.ABE_Daten.ZZBEMER10, Typ.ABE_Daten.ZZBEMER11, Typ.ABE_Daten.ZZBEMER12, 
                Typ.ABE_Daten.ZZBEMER13, Typ.ABE_Daten.ZZBEMER14 });
            lbl_31.Text = Typ.ABE_Daten.ZZLAENGEMIN.TrimStart('0');
            lbl_32.Text = Typ.ABE_Daten.ZZBREITEMIN.TrimStart('0');
            lbl_33.Text = Typ.ABE_Daten.ZZHOEHEMIN;

            lbl_00.Text = Typ.ABE_Daten.ZZFARBE + " (" + Typ.ABE_Daten.Farbziffer + ")";
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

            switch (Typ.ABE_Daten.Farbziffer)
            {
                case "0":
                    lbl_99.Visible = true;
                    //lbl_199.Visible = true;
                    break;
                case "1":
                    lbl_98.Visible = true;
                    //lbl_198.Visible = true;
                    break;
                case "2":
                    lbl_97.Visible = true;
                    //lbl_197.Visible = true;
                    break;
                case "3":
                    lbl_96.Visible = true;
                    //lbl_196.Visible = true;
                    break;
                case "4":
                    lbl_95.Visible = true;
                    //lbl_195.Visible = true;
                    break;
                case "5":
                    lbl_94.Visible = true;
                    //lbl_194.Visible = true;
                    break;
                case "6":
                    lbl_93.Visible = true;
                    //lbl_193.Visible = true;
                    break;
                case "7":
                    lbl_92.Visible = true;
                    //lbl_192.Visible = true;
                    break;
                case "8":
                    lbl_91.Visible = true;
                    //lbl_191.Visible = true;
                    break;
                case "9":
                    lbl_55.Visible = true;
                    //lbl_155.Visible = true;
                    break;
                default:

                    break;
            }

            // Lebenslauf
            LebenslaufRepeater.DataSource = m_report.Lebenslauf;
            LebenslaufRepeater.DataBind();

            // -> im aspx gebundene Felder befüllen (z.B. Belastungsanzeige)
            Page.DataBind();
        }


        private String CheckDate(String ConDate)
        {
            if (ConDate.Length > 0)
            {
                return ConDate.Substring(0, 10);
            }
            else
            {
                return "&nbsp;";
            }

        }

        private String ChangeTime(String ConTime)
        {
            if (ConTime.Length > 0)
            {
                ConTime = ConTime.Substring(0, 2) + ":" + ConTime.Substring(2, 2);
                return ConTime;
            }
            else
            {
                return "&nbsp;";
            }
        }

        private String FillWithSpace(String ConString)
        {
            if (ConString.Length > 0)
            {
                return ConString;
            }
            else
            {
                return "&nbsp;";
            }
        }

        protected void lbBack_Click(object sender, EventArgs e)
        {

            Session["Historie"] = null;

            if (Request.QueryString["Linked"] !=null)
            {
                string strscript = "<script language=javascript>window.top.close();</script>";

                if (!ClientScript.IsStartupScriptRegistered("clientScript"))
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "clientScript", strscript);
                }


                return;
            }


            Response.Redirect("Report14.aspx?AppID=" + (string)Session["AppID"]);
        }

        protected HistorieBelastungsanzeige Belastungsanzeige { get { return m_report==null?null:m_report.Belastungsanzeige; } }
        protected HistorieUebersicht Uebersicht { get { return m_report == null ? null : m_report.Uebersicht; } }
        protected HistorieLinks Links { get { return m_report == null ? null : m_report.Links; } }

        protected void ShowBelastungsanzeige(object sender, EventArgs e)
        {
            m_report = (Historie)Session["Historie"];
            if (Links == null) return;

            Links.OpenBelastungsanzeige(ShowReportHelper);
        }

        protected void ShowSchadensgutachten(object sender, EventArgs e)
        {
            m_report = (Historie)Session["Historie"];
            if (Links == null) return;

            Links.OpenSchadensgutachten(ShowReportHelper);
        }

        protected void ShowRechnung(object sender, EventArgs e)
        {
            m_report = (Historie)Session["Historie"];
            if (Links == null) return;

            Links.OpenRechnung(ShowReportHelper);
        }
    }
}