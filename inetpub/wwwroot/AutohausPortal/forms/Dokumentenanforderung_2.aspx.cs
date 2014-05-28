using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CKG.Base.Kernel;
using CKG.Base.Kernel.Common;
using CKG.Base.Kernel.Security;
using AutohausPortal.lib;
using System.Data;

namespace AutohausPortal.forms
{      
    /// <summary>
    /// Anzeige der Dokumentenanforderung der Zulassungsstellen. Benutzte Klassen Report99.
    /// </summary>
    public partial class Dokumentenanforderung_2 : System.Web.UI.Page
    {
        private User m_User;
        private App m_App;
        private Report99 objSuche;

        /// <summary>
        /// Page_Load Ereignis.
        /// Überprüfung ob dem User diese Applikation zugeordnet ist.
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            m_User = Common.GetUser(this);
            Common.FormAuth(this, m_User);
            m_App = new App(m_User);
            Common.GetAppIDFromQueryString(this);

            if (Session["objSuche"] == null)
            {
                lblError.Text = "Daten konnten nicht geladen werden! Session-Objekt nicht gefüllt!";
                return;
            }

            if (Request.QueryString["AppID"].Length > 0)
            {
                Session["AppID"] = Request.QueryString["AppID"].ToString();
                lnkEinzug.NavigateUrl = "Dokumentenanforderung_3.aspx?AppID=" + Session["AppID"];
            }
            else
            {
                lnkEinzug.NavigateUrl = String.Empty;
            }

            if (!IsPostBack) { FillForm(); }
        }

        /// <summary>
        /// füllt die Controls der Seite mit den erhaltenen Daten(objSuche.Result).
        /// </summary>
        private void FillForm()
        {
            objSuche = (Report99)(Session["objSuche"]);

            lblKreisKZ.Text = objSuche.PKennzeichen;

            DataRow[] SelectRow;
            DataRow resultRow;

            if (objSuche.Result.Rows.Count == 1)
            {
                resultRow = objSuche.Result.Rows[0];
            }
            else
            {
                SelectRow = objSuche.Result.Select("Zkba2='00'");
                if (SelectRow.Length > 0)
                {
                    resultRow = SelectRow[0];
                }
                else
                {
                    lblError.Text = "Keine Ergebnisse für die gewählten Kriterien.";
                    return;
                }
            }


            //Privat Zulassung
            Label00.Text = resultRow["PZUL_BRIEF"].ToString();
            Label01.Text = resultRow["PZUL_SCHEIN"].ToString();
            Label02.Text = resultRow["PZUL_COC"].ToString();
            Label03.Text = resultRow["PZUL_DECK"].ToString();
            Label04.Text = resultRow["PZUL_VOLLM"].ToString();
            Label05.Text = resultRow["PZUL_AUSW"].ToString();
            Label06.Text = resultRow["PZUL_GEWERB"].ToString();
            Label07.Text = resultRow["PZUL_HANDEL"].ToString();
            Label08.Text = resultRow["PZUL_LAST"].ToString();
            Label09.Text = resultRow["PZUL_BEM"].ToString();

            //Privat Umschreibung
            Label10.Text = resultRow["PUMSCHR_BRIEF"].ToString();
            Label11.Text = resultRow["PUMSCHR_SCHEIN"].ToString();
            Label12.Text = resultRow["PUMSCHR_COC"].ToString();
            Label13.Text = resultRow["PUMSCHR_DECK"].ToString();
            Label14.Text = resultRow["PUMSCHR_VOLLM"].ToString();
            Label15.Text = resultRow["PUMSCHR_AUSW"].ToString();
            Label16.Text = resultRow["PUMSCHR_GEWERB"].ToString();
            Label17.Text = resultRow["PUMSCHR_HANDEL"].ToString();
            Label18.Text = resultRow["PUMSCHR_LAST"].ToString();
            Label19.Text = resultRow["PUMSCHR_BEM"].ToString();

            //Privat Umkennzeichnung
            Label20.Text = resultRow["PUMK_BRIEF"].ToString();
            Label21.Text = resultRow["PUMK_SCHEIN"].ToString();
            Label22.Text = resultRow["PUMK_COC"].ToString();
            Label23.Text = resultRow["PUMK_DECK"].ToString();
            Label24.Text = resultRow["PUMK_VOLLM"].ToString();
            Label25.Text = resultRow["PUMK_AUSW"].ToString();
            Label26.Text = resultRow["PUMK_GEWERB"].ToString();
            Label27.Text = resultRow["PUMK_HANDEL"].ToString();
            Label28.Text = resultRow["PUMK_LAST"].ToString();
            Label29.Text = resultRow["PUMK_BEM"].ToString();

            //Privat Ersatzfahrzeugschein
            Label30.Text = resultRow["PERS_BRIEF"].ToString();
            Label31.Text = resultRow["PERS_SCHEIN"].ToString();
            Label32.Text = resultRow["PERS_COC"].ToString();
            Label33.Text = resultRow["PERS_DECK"].ToString();
            Label34.Text = resultRow["PERS_VOLLM"].ToString();
            Label35.Text = resultRow["PERS_AUSW"].ToString();
            Label36.Text = resultRow["PERS_GEWERB"].ToString();
            Label37.Text = resultRow["PERS_HANDEL"].ToString();
            Label38.Text = resultRow["PERS_LAST"].ToString();
            Label39.Text = resultRow["PERS_BEM"].ToString();

            //Unternehmen Zulassung
            Label40.Text = resultRow["UZUL_BRIEF"].ToString();
            Label41.Text = resultRow["UZUL_SCHEIN"].ToString();
            Label42.Text = resultRow["UZUL_COC"].ToString();
            Label43.Text = resultRow["UZUL_DECK"].ToString();
            Label44.Text = resultRow["UZUL_VOLLM"].ToString();
            Label45.Text = resultRow["UZUL_AUSW"].ToString();
            Label46.Text = resultRow["UZUL_GEWERB"].ToString();
            Label47.Text = resultRow["UZUL_HANDEL"].ToString();
            Label48.Text = resultRow["UZUL_LAST"].ToString();
            Label49.Text = resultRow["UZUL_BEM"].ToString();

            //Unternehmen Umschreibung
            Label50.Text = resultRow["UUMSCHR_BRIEF"].ToString();
            Label51.Text = resultRow["UUMSCHR_SCHEIN"].ToString();
            Label52.Text = resultRow["UUMSCHR_COC"].ToString();
            Label53.Text = resultRow["UUMSCHR_DECK"].ToString();
            Label54.Text = resultRow["UUMSCHR_VOLLM"].ToString();
            Label55.Text = resultRow["UUMSCHR_AUSW"].ToString();
            Label56.Text = resultRow["UUMSCHR_GEWERB"].ToString();
            Label57.Text = resultRow["UUMSCHR_HANDEL"].ToString();
            Label58.Text = resultRow["UUMSCHR_LAST"].ToString();
            Label59.Text = resultRow["UUMSCHR_BEM"].ToString();
            //Unternehmen Umkennzeichnung
            Label60.Text = resultRow["UUMK_BRIEF"].ToString();
            Label61.Text = resultRow["UUMK_SCHEIN"].ToString();
            Label62.Text = resultRow["UUMK_COC"].ToString();
            Label63.Text = resultRow["UUMK_DECK"].ToString();
            Label64.Text = resultRow["UUMK_VOLLM"].ToString();
            Label65.Text = resultRow["UUMK_AUSW"].ToString();
            Label66.Text = resultRow["UUMK_GEWERB"].ToString();
            Label67.Text = resultRow["UUMK_HANDEL"].ToString();
            Label68.Text = resultRow["UUMK_LAST"].ToString();
            Label69.Text = resultRow["UUMK_BEM"].ToString();

            //Unternehmen Ersatzfahrzeugschein
            Label70.Text = resultRow["UERS_BRIEF"].ToString();
            Label71.Text = resultRow["UERS_SCHEIN"].ToString();
            Label72.Text = resultRow["UERS_COC"].ToString();
            Label73.Text = resultRow["UERS_DECK"].ToString();
            Label74.Text = resultRow["UERS_VOLLM"].ToString();
            Label75.Text = resultRow["UERS_AUSW"].ToString();
            Label76.Text = resultRow["UERS_GEWERB"].ToString();
            Label77.Text = resultRow["UERS_HANDEL"].ToString();
            Label78.Text = resultRow["UERS_LAST"].ToString();
            Label79.Text = resultRow["UERS_BEM"].ToString();

            cmdWunsch.Enabled = true;
            cmdFormulare.Enabled = true;
            cmdGebuehr.Enabled = true;
            cmdAmt.Enabled = true;
            if (String.IsNullOrEmpty(Label05.Text))
            {
                lblError.Text = "Es konnten keine Dokumentenanforderungen gefunden werden, <br> benutzen Sie bitte die bereitgestellten Links des Zulassungskreises! ";
            }
        }

        /// <summary>
        /// Öffnet ein neues Browserfenster zu den Seiten der Zulassungstelle(Wunschkenzeichen, Gebühren, Formulare). Information aus der Result-Tabelle.
        /// Aufgerufen von cmdWunsch_Click(), cmdFormulare_Click(), cmdGebuehr_Click().
        /// </summary>
        /// <param name="RowName">URL oder STVALNFORM oder STVALNGEB </param>
        private void ClickLink(String RowName)
        {
            objSuche = (Report99)(Session["objSuche"]);
            String sUrl;
            DataRow resultRow;
            String sTempUrl;
            String sAmt;
            lblInfo.Text = "";
            resultRow = objSuche.Result.Rows[0];
            sAmt = objSuche.PKennzeichen;
            if (resultRow[RowName].ToString().Length > 0)
            {
                sTempUrl = resultRow[RowName].ToString().Substring(0, 7);


                if (sTempUrl.Length != 0)
                {
                    sUrl = resultRow[RowName].ToString();
                    ResponseHelper.Redirect(sUrl, "_blank", "left=0,top=0,resizable=YES,scrollbars=YES,menubar=YES,resizable=yes,scrollbars=YES,status=YES,toolbar=YES");
                }
                else
                {
                    lblInfo.Text = "Das Straßenverkehrsamt für das amtliche Kennzeichen " + sAmt + " bietet keine Weblink hierfür an.<br>" +
                        "Möchten Sie auf die Standardstartseite dies Verkehrsamts wechseln, so klicken Sie bitte auf den Link Amt. ";
                }
            }
            else
            {
                lblInfo.Text = "Das Straßenverkehrsamt für das amtliche Kennzeichen " + sAmt + " bietet keine Weblink hierfür an.<br>" +
                    "Möchten Sie auf die Standardstartseite dies Verkehrsamts wechseln, so klicken Sie bitte auf den Link Amt. ";
            }
        }

        /// <summary>
        /// Öffnet ein neues Browserfenster zu den Seiten der Zulassungstelle(Hauptseite des Amtes). Information aus der Result-Tabelle.
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        protected void cmdAmt_Click(object sender, EventArgs e)
        {
            objSuche = (Report99)(Session["objSuche"]);
            String sUrl;
            DataRow resultRow;
            String sAmt;
            lblInfo.Text = "";
            resultRow = objSuche.Result.Rows[0];
            sUrl = resultRow["STVALN"].ToString();//"http://" +
            sAmt = objSuche.PKennzeichen;

            if (sUrl.Length != 0)
            {
                ResponseHelper.Redirect(sUrl, "_blank", "left=0,top=0,resizable=YES,scrollbars=YES,menubar=YES,resizable=yes,scrollbars=YES,status=YES,toolbar=YES");

            }
            else
            {
                lblInfo.Text = "Das Straßenverkehrsamt für das amtliche Kennzeichen " + sAmt + " bietet keine Weblink hierfür an.";
                //<br>" +                "Möchten Sie auf die Standardstartseite dies Verkehrsamts wechseln, so klicken Sie bitte auf den Link Amt. ";
            }
        }

        /// <summary>
        /// Aufruf von ClickLink("URL"). Wunschkennzeichen reservieren auf der entspr. Seite der Zulassungsstelle.
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        protected void cmdWunsch_Click(object sender, EventArgs e)
        {
            ClickLink("URL");
        }

        /// <summary>
        /// Aufruf von ClickLink("STVALNFORM"). Formulare(Einzugsermächtigung) auf der entspr. Seite der Zulassungsstelle.
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        protected void cmdFormulare_Click(object sender, EventArgs e)
        {
            ClickLink("STVALNFORM");
        }
        /// <summary>
        /// Aufruf von ClickLink("STVALNFORM").Informationen zur Gebührenauslage auf der entspr. Seite der Zulassungsstelle.
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        protected void cmdGebuehr_Click(object sender, EventArgs e)
        {
            ClickLink("STVALNGEB");
        }
    }
}