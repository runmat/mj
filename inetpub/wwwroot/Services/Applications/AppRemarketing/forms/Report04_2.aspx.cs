using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CKG.Base.Kernel.Common;
using CKG.Base.Kernel.Security;
using CKG.Base.Business;
using System.Data;
using AppRemarketing.lib;

namespace AppRemarketing.forms
{
    public partial class Report04_2 : System.Web.UI.Page
    {
        private CKG.Base.Kernel.Security.User m_User;
        private CKG.Base.Kernel.Security.App m_App;
        private Historie m_report;
        private ABEDaten Typ;

        protected void Page_Load(object sender, EventArgs e)
        {
            m_User = Common.GetUser(this);

            if (!(m_User.UserName == "mjeaudi"))
                Common.FormAuth(this, m_User);

            m_App = new App(m_User);

            Common.GetAppIDFromQueryString(this);

            lblHead.Text = (string)m_User.Applications.Select("AppID = '" + Session["AppID"] + "'")[0]["AppFriendlyName"];

            if (!IsPostBack)
            {
                Fill();
            }

            

        }

        private void Fill()
        {
            m_report = (Historie) Session["Historie"];

            //Oberste Zeile Übersicht
            lblFahrgestellnummerShow.Text = (String)m_report.CommonData.Rows[0]["FAHRGNR"];
            lblKennzeichenShow.Text = (String)m_report.CommonData.Rows[0]["KENNZ"];
            lblBriefnummerShow.Text = (String)m_report.CommonData.Rows[0]["BRIEFNR"];
            lblLizNrShow.Text = (String)m_report.CommonData.Rows[0]["LIZNR"];

            //Übersicht allgemeine Daten
            lblModellShow.Text = FillWithSpace((String)m_report.CommonData.Rows[0]["MODELL"]);
            lblFarbeShow.Text = FillWithSpace((String)m_report.CommonData.Rows[0]["FARBE"]);
            lblInnenausstattungShow.Text = FillWithSpace((String)m_report.CommonData.Rows[0]["INAUST"]);
            lblPrNrShow.Text = FillWithSpace((String)m_report.CommonData.Rows[0]["PRNR"]);
            lblInventarShow.Text = FillWithSpace((String)m_report.CommonData.Rows[0]["INVENTAR"]);
            lblAvIDShow.Text = FillWithSpace((String)m_report.CommonData.Rows[0]["AVNR"]);
            lblAuslieferungsdatumShow.Text = CheckDate(m_report.CommonData.Rows[0]["AUSLDAT"].ToString());
            lblEingangsdatumShow.Text = CheckDate(m_report.CommonData.Rows[0]["INDATUM"].ToString());
            lblZulassungsdatumShow.Text = CheckDate(m_report.CommonData.Rows[0]["ZULDAT"].ToString());
            lblHcEingangShow.Text = CheckDate(m_report.CommonData.Rows[0]["HCEINGDAT"].ToString());
            lblHereinnahmeOrtShow.Text = FillWithSpace((String)m_report.CommonData.Rows[0]["HCORT"]);
            lblKmStandShow.Text = FillWithSpace(Convert.ToInt32(m_report.CommonData.Rows[0]["KMSTAND"]).ToString());
            lblStilllegungShow.Text = CheckDate(m_report.CommonData.Rows[0]["STILLDAT"].ToString());
            lblEreignisartShow.Text = FillWithSpace((String)m_report.CommonData.Rows[0]["EREIGNIS"]);
            lblSchadbetragSelbstVerShow.Text = FillWithSpace((String)m_report.CommonData.Rows[0]["S_BETRAG"]);
            lblRechnungsdatumShow.Text = CheckDate(m_report.CommonData.Rows[0]["DATUM_RE"].ToString());

            if (m_report.CommonData.Rows[0]["EREIGNIS"].ToString().Length > 0)
            {
                cbxSchaden.Checked = true;
            }

            lblRechnungUebermShow.Text = CheckDate(m_report.CommonData.Rows[0]["UEBERM_RE"].ToString());
            lblRechnungsbetragShow.Text = FillWithSpace((String)m_report.CommonData.Rows[0]["BETRAG_RE"]);
            lblRechnungsnummerShow.Text = FillWithSpace(m_report.CommonData.Rows[0]["NUMMER_RE"].ToString());
            lblErfassungNaviShow.Text = CheckDate(m_report.CommonData.Rows[0]["EGNCDDAT"].ToString());
            lblUebermSelbstvermShow.Text = CheckDate(m_report.CommonData.Rows[0]["UESVM"].ToString());
            lblUebermVerlustShow.Text = CheckDate(m_report.CommonData.Rows[0]["UEVERLUST"].ToString());
            lblSchadensmeldungAudiShow.Text = CheckDate(m_report.CommonData.Rows[0]["SCHADMELDDAT"].ToString());
            lblZulassungAnAGShow.Text = CheckDate(m_report.CommonData.Rows[0]["UEBMZLDAT"].ToString());
            lblImportSchaedenShow.Text = CheckDate(m_report.CommonData.Rows[0]["IMPDAT_SCHAEDEN"].ToString());
            lblBemerkung1Show.Text = FillWithSpace((String)m_report.CommonData.Rows[0]["BEM_1"]);
            lblBemerkung2Show.Text = FillWithSpace((String)m_report.CommonData.Rows[0]["BEM_2"]);
            lblMeldungAnAudiShow.Text = CheckDate(m_report.CommonData.Rows[0]["MELDDATGUTA"].ToString());
            lblVerlustdatumShow.Text = CheckDate(m_report.CommonData.Rows[0]["VLMDAT"].ToString());

            //Übersicht Gutachtendaten
            if (m_report.Gutachten.Rows.Count > 0)
            {
                lblLaufNummerShow.Text = FillWithSpace((String)m_report.Gutachten.Rows[0]["LFDNR"]);
                lblGutEingangsdatumShow.Text = CheckDate(m_report.Gutachten.Rows[0]["INDATUM"].ToString());
                lblGutEingangszeitShow.Text = ChangeTime(m_report.Gutachten.Rows[0]["INTIME"].ToString());
                lblGutachterShow.Text = FillWithSpace((String)m_report.Gutachten.Rows[0]["GUTA"]);
                lblGutachtenIDShow.Text = FillWithSpace((String)m_report.Gutachten.Rows[0]["GUTAID"]);
                lblGutKMStandShow.Text = FillWithSpace(Convert.ToInt32(m_report.Gutachten.Rows[0]["KMSTAND"]).ToString());
                lblGutachtendatumShow.Text = CheckDate(m_report.Gutachten.Rows[0]["GUTADAT"].ToString());
                lblSchadenskennzeichenShow.Text = FillWithSpace((String)m_report.Gutachten.Rows[0]["SCHADKZ"]);
                lblSchadensbetragShow.Text = FillWithSpace(m_report.Gutachten.Rows[0]["SCHADBETR_AV"].ToString());
                lblRepKennzeichenShow.Text = FillWithSpace((String)m_report.Gutachten.Rows[0]["REPKZ"]);
                lblWertminderungBetragShow.Text = FillWithSpace(m_report.Gutachten.Rows[0]["WRTMBETR_AV"].ToString());
                lblFehlbetragShow.Text = FillWithSpace(m_report.Gutachten.Rows[0]["FEHLTBETR"].ToString());
                lblFehlbetragAVShow.Text = FillWithSpace(m_report.Gutachten.Rows[0]["FEHLTBETR_AV"].ToString());

            }
            //Brief- und Schlüsseldaten
            lblEingangBriefShow.Text = CheckDate(m_report.CommonData.Rows[0]["EGZB2DAT"].ToString());
            lblEingangSchlueShow.Text = CheckDate(m_report.CommonData.Rows[0]["EGZWSLDAT"].ToString());

            if (m_report.Versand.Rows.Count > 0)
            {
                lblVersandBriefShow.Text = CheckDate(m_report.Versand.Rows[0]["B_VERSAUFTR_DAT"].ToString());
                lblVersandSchlueShow.Text = CheckDate(m_report.Versand.Rows[0]["T_VERSAUFTR_DAT"].ToString());

                lblExBelegNrShow.Text = FillWithSpace((String)m_report.Versand.Rows[0]["BELNR"]);
                lblRechBetragShow.Text = FillWithSpace(m_report.Versand.Rows[0]["BETRAG_RE"].ToString());
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
                        lbl_25.Text = Typ.ABE_Daten.ZZACHSL_A1_STA.TrimStart('0') + ", " + Typ.ABE_Daten.ZZACHSL_A2_STA.TrimStart('0') + ", " + Typ.ABE_Daten.ZZACHSL_A3_STA.TrimStart('0');




                        if (lbl_25.Text.Substring((lbl_25.Text.Length - 2),2) == ", ")
            
                        {
                            
                            lbl_25.Text = lbl_25.Text.Substring(0,lbl_25.Text.Length - 2);
                        }
       

                        lbl_27.Text = Typ.ABE_Daten.ZZBEREIFACHSE1 + ", " + Typ.ABE_Daten.ZZBEREIFACHSE2 + ", " + Typ.ABE_Daten.ZZBEREIFACHSE3;


            if (lbl_27.Text.Substring((lbl_27.Text.Length - 2),2) == ", ")
            
                        {
                            
                            lbl_27.Text = lbl_27.Text.Substring(0,lbl_27.Text.Length - 2);
                        }


            lbl_28.Text = Typ.ABE_Daten.ZZZULGESGEW.TrimStart('0');
                        lbl_29.Text = Typ.ABE_Daten.ZZTYP_SCHL;
                        lbl_30.Text = Typ.ABE_Daten.ZZBEMER1 + "<br>" + Typ.ABE_Daten.ZZBEMER2 + "<br>" + Typ.ABE_Daten.ZZBEMER3 + "<br>" + Typ.ABE_Daten.ZZBEMER4 + "<br>" + Typ.ABE_Daten.ZZBEMER5 + "<br>" + Typ.ABE_Daten.ZZBEMER6 + "<br>" + Typ.ABE_Daten.ZZBEMER7 + "<br>" + Typ.ABE_Daten.ZZBEMER8 + "<br>" + Typ.ABE_Daten.ZZBEMER9 + "<br>" + Typ.ABE_Daten.ZZBEMER10 + "<br>" + Typ.ABE_Daten.ZZBEMER11 + "<br>" + Typ.ABE_Daten.ZZBEMER12 + "<br>" + Typ.ABE_Daten.ZZBEMER13 + "<br>" + Typ.ABE_Daten.ZZBEMER14;
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


                       // Model / Ausstattung

                       if (m_report.ModelTable.Rows.Count == 1)
                       {
                           lbl_ModelCode.Text = m_report.ModelTable.Rows[0]["PACKIDENT"].ToString();
                           lbl_ModelBezeichnung.Text = m_report.ModelTable.Rows[0]["BEZ_PRNR"].ToString();
                       }

                       if (m_report.AussenFarbeTable.Rows.Count == 1)
                       {
                           lbl_FarbCode_Aussen.Text = m_report.AussenFarbeTable.Rows[0]["PACKIDENT"].ToString();
                           lbl_FarbBezeichnung_Aussen.Text = m_report.AussenFarbeTable.Rows[0]["BEZ_PRNR"].ToString();
                       }

                       if (m_report.InnenFarbeTable.Rows.Count == 1)
                       {
                           lbl_FarbCode_Innen.Text = m_report.InnenFarbeTable.Rows[0]["PACKIDENT"].ToString();
                           lbl_FarbBezeichnung_Innen.Text = m_report.InnenFarbeTable.Rows[0]["BEZ_PRNR"].ToString();
                       }

                       AusstattungRepeater.DataSource = m_report.AusstattungTable;
                       AusstattungRepeater.DataBind();
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
            Response.Redirect("Report04.aspx?AppID=" + (string)Session["AppID"]);
        }


    }
}
