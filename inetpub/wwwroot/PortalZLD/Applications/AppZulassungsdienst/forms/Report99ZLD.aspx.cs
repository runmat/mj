using System;
using System.Web.UI;
using CKG.Base.Kernel.Common;
using CKG.Base.Kernel.Security;
using AppZulassungsdienst.lib;
using System.Data;

namespace AppZulassungsdienst.forms
{
    /// <summary>
    /// Selektion Dokumentenanforderung der Zulassungsstellen.
    /// </summary>
    public partial class Report99ZLD : Page
    {
        private User m_User;
        private Report99 objSuche;

        #region Events

        /// <summary>
        /// Page_Load Ereignis. Überprüfung ob dem User diese Applikation zugeordnet ist.
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            m_User = Common.GetUser(this);
            Common.FormAuth(this, m_User);
            Common.GetAppIDFromQueryString(this);

            lblHead.Text = (string)m_User.Applications.Select("AppID = '" + Session["AppID"] + "'")[0]["AppFriendlyName"];

            if (Session["objSuche"] != null)
            {
                objSuche = (Report99)Session["objSuche"];
            }
            else
            {
                objSuche = new Report99();
                Session["objSuche"] = objSuche;
            }

            if (!String.IsNullOrEmpty(Request.QueryString["AppID"]))
            {
                Session["AppID"] = Request.QueryString["AppID"];
                lnkEinzug.NavigateUrl = "Report99ZLD_2.aspx?AppID=" + Session["AppID"];
            }
            else
            {
                lnkEinzug.NavigateUrl = "";
            }
            txtKennzeichen.Attributes.Add("onkeyup", "FilterKennz(this,event)");
        }

        /// <summary>
        ///  Aufruf von DoSubmit() wenn txtKennz gefüllt.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void cmdCreate_Click(object sender, EventArgs e)
        {
            ClearForm();
            txtKennzeichen.Text = txtKennzeichen.Text.Replace(" ", "");
            if (!String.IsNullOrEmpty(txtKennzeichen.Text))
            {
                DoSubmit();
            }
            else
            {
                lblError.Text = "Bitte ein Ortskennzeichen eingeben.";
            }
        }

        /// <summary>
        /// Öffnet ein neues Browserfenster zu den Seiten der Zulassungstelle(Hauptseite des Amtes). Information aus der Result-Tabelle.
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        protected void cmdAmt_Click(object sender, EventArgs e)
        {
            string sAmt = txtKennzeichen.Text;

            if (objSuche.tblResult.Rows.Count > 0 && !String.IsNullOrEmpty(objSuche.tblResult.Rows[0]["STVALN"].ToString()))
            {
                lblInfo.Text = "";
                string sUrl = objSuche.tblResult.Rows[0]["STVALN"].ToString();
                ResponseHelper.Redirect(sUrl, "_blank", "left=0,top=0,resizable=YES,scrollbars=YES,menubar=YES,resizable=yes,scrollbars=YES,status=YES,toolbar=YES");
            }
            else
            {
                lblInfo.Text = "Das Straßenverkehrsamt für das amtliche Kennzeichen " + sAmt + " bietet keine Weblink hierfür an.";
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

        /// <summary>
        /// On Enter Buttondummy.
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">ImageClickEventArgs</param>
        protected void btnEmpty_Click(object sender, ImageClickEventArgs e)
        {
            cmdCreate_Click(sender, e);
        }

        #endregion

        #region Methods

        private void ClearForm()
        {
            Session["objSuche"] = null;

            //Privat Zulassung
            Label00.Text = "";
            Label01.Text = "";
            Label02.Text = "";
            Label03.Text = "";
            Label04.Text = "";
            Label05.Text = "";
            Label06.Text = "";
            Label07.Text = "";
            Label08.Text = "";
            Label09.Text = "";

            //Privat Umschreibung
            Label10.Text = "";
            Label11.Text = "";
            Label12.Text = "";
            Label13.Text = "";
            Label14.Text = "";
            Label15.Text = "";
            Label16.Text = "";
            Label17.Text = "";
            Label18.Text = "";
            Label19.Text = "";

            //Privat Umkennzeichnung
            Label20.Text = "";
            Label21.Text = "";
            Label22.Text = "";
            Label23.Text = "";
            Label24.Text = "";
            Label25.Text = "";
            Label26.Text = "";
            Label27.Text = "";
            Label28.Text = "";
            Label29.Text = "";

            //Privat Ersatzfahrzeugschein
            Label30.Text = "";
            Label31.Text = "";
            Label32.Text = "";
            Label33.Text = "";
            Label34.Text = "";
            Label35.Text = "";
            Label36.Text = "";
            Label37.Text = "";
            Label38.Text = "";
            Label39.Text = "";

            //Unternehmen Zulassung
            Label40.Text = "";
            Label41.Text = "";
            Label42.Text = "";
            Label43.Text = "";
            Label44.Text = "";
            Label45.Text = "";
            Label46.Text = "";
            Label47.Text = "";
            Label48.Text = "";
            Label49.Text = "";

            //Unternehmen Umschreibung
            Label50.Text = "";
            Label51.Text = "";
            Label52.Text = "";
            Label53.Text = "";
            Label54.Text = "";
            Label55.Text = "";
            Label56.Text = "";
            Label57.Text = "";
            Label58.Text = "";
            Label59.Text = "";

            //Unternehmen Umkennzeichnung
            Label60.Text = "";
            Label61.Text = "";
            Label62.Text = "";
            Label63.Text = "";
            Label64.Text = "";
            Label65.Text = "";
            Label66.Text = "";
            Label67.Text = "";
            Label68.Text = "";
            Label69.Text = "";

            //Unternehmen Ersatzfahrzeugschein
            Label70.Text = "";
            Label71.Text = "";
            Label72.Text = "";
            Label73.Text = "";
            Label74.Text = "";
            Label75.Text = "";
            Label76.Text = "";
            Label77.Text = "";
            Label78.Text = "";
            Label79.Text = "";

            lblInfo.Text = "";
            cmdWunsch.Enabled = false;
            cmdFormulare.Enabled = false;
            cmdGebuehr.Enabled = false;
            cmdAmt.Enabled = false;
        }

        /// <summary>
        /// SAP- Aufruf mit Importparameter Kennzeichen. Bei positiven Ergebnis Aufruf FillGrid().
        /// </summary>
        private void DoSubmit()
        {
            lblError.Text = "";

            objSuche.PKennzeichen = txtKennzeichen.Text;
            objSuche.Fill();

            Session["objSuche"] = objSuche;

            if (objSuche.ErrorOccured)
            {
                lblError.Text = "Fehler: " + objSuche.Message;
            }
            else
            {
                if (objSuche.tblResult.Rows.Count == 0)
                {
                    lblError.Text = "Keine Ergebnisse für die gewählten Kriterien.";
                    txtKennzeichen.Text = "";
                }
                else
                {
                    FillForm();
                }
            }
        }

        /// <summary>
        ///  Füllen der Controls der Seite mit den erhaltenen Daten(objSuche.Result).
        /// </summary>
        private void FillForm()
        {
            DataRow resultRow;

            if (objSuche.tblResult.Rows.Count > 0)
            {
                resultRow = objSuche.tblResult.Rows[0];
            }
            else
            {
                DataRow[] SelectRow = objSuche.tblResult.Select("Zkba2='00'");
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

            Result.Visible = true;

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
        /// <param name="colName">URL oder STVALNFORM oder STVALNGEB </param>
        private void ClickLink(String colName)
        {
            string sAmt = txtKennzeichen.Text;

            if (objSuche.tblResult.Rows.Count > 0 && !String.IsNullOrEmpty(objSuche.tblResult.Rows[0][colName].ToString()))
            {
                lblInfo.Text = "";
                string sUrl = objSuche.tblResult.Rows[0][colName].ToString();
                ResponseHelper.Redirect(sUrl, "_blank", "left=0,top=0,resizable=YES,scrollbars=YES,menubar=YES,resizable=yes,scrollbars=YES,status=YES,toolbar=YES");
            }
            else
            {
                lblInfo.Text = "Das Straßenverkehrsamt für das amtliche Kennzeichen " + sAmt + " bietet keine Weblink hierfür an.<br>" +
                    "Möchten Sie auf die Standardstartseite dies Verkehrsamts wechseln, so klicken Sie bitte auf den Link Amt. ";
            }
        }

        #endregion
    }
}
