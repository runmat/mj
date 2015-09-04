using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using CKG.Base.Kernel.Security;
using AutohausPortal.lib;
using CKG.Base.Kernel.Common;
using System.Data;
using GeneralTools.Models;
using Telerik.Web.UI;

namespace AutohausPortal.forms
{

    /// <summary>
    /// Selektion Zulassungsstatistik. 
    /// Benutzte Klassen ZLD_Suche und ZLDCommon.
    /// </summary>
    public partial class Zulassungsstatistik : Page
    {
        private User m_User;
        private App m_App;
        private ZLDCommon objCommon;
        private ZLD_Suche objZLDSuche;

        /// <summary>
        /// Page_Load Ereignis.
        /// Überprüfung ob dem User diese Applikation zugeordnet ist. Laden der Stammdaten wenn noch nicht in der Session(objCommon) vorhanden. 
        /// Füllen der DropDowns mit den Stammdaten(fillDropDowns). Eventuell voherige Selektionsdaten wieder anzeigen(fillOldValues).
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        protected void Page_Init(object sender, EventArgs e)
        {
            m_User = Common.GetUser(this);
            Common.FormAuth(this, m_User);
            m_App = new App(m_User);
            Common.GetAppIDFromQueryString(this);

            if (m_User.Reference.Trim(' ').Length == 0)
            {
                lblError.Text = "Es wurde keine Benutzerreferenz angegeben! Somit können keine Stammdaten ermittelt werden!";
                return;
            }
            if (Session["objCommon"] == null)
            {
                objCommon = new ZLDCommon(ref m_User, m_App);
                if (!objCommon.Init(Session["AppID"].ToString(), Session.SessionID, this))
                {
                    lblError.Visible = true;
                    lblError.Text = objCommon.Message;
                    return;
                }
                Session["objCommon"] = objCommon;
            }
            else
            {
                objCommon = (ZLDCommon)Session["objCommon"];

            }

            fillDropDowns();
            if (Session["objZLDSuche"] != null)
            {
                objZLDSuche = (ZLD_Suche)Session["objZLDSuche"];
                fillOldValues();
            }
        }

        /// <summary>
        ///  Füllen der DropDowns mit den Stammdaten
        /// </summary>
        private void fillDropDowns()
        {

            try
            {
                DataView tmpDView = new DataView();

                tmpDView = objCommon.tblKundenStamm.DefaultView;
                tmpDView.Sort = "NAME1";
                ddlKunnr1.DataSource = tmpDView;
                ddlKunnr1.DataValueField = "KUNNR";
                ddlKunnr1.DataTextField = "NAME1";
                ddlKunnr1.DataBind();

                addAttributes(txtKennz1);
                addAttributes(txtKennz2);
            }


            catch (Exception ex)
            {

                lblError.Text = ex.Message;
            }

        }

        /// <summary>
        /// Voherige Selektionsdaten wieder anzeigen.
        /// </summary>
        private void fillOldValues()
        {

            try
            {
                ddlKunnr1.SelectedValue = objZLDSuche.Kunnr; disableDefaultValueDDL("ctl00_ContentPlaceHolder1_ddlKunnr1_Input");
                String Ref1 = "", Ref2 = "", Ref3 = "", Ref4 = "";
                DataRow[] drow = objCommon.tblKundenStamm.Select("KUNNR ='" + ddlKunnr1.SelectedValue + "'");
                if (drow.Length > 0)
                {
                    Ref1 = drow[0]["REF_NAME_01"].ToString();
                    Ref2 = drow[0]["REF_NAME_02"].ToString();
                    Ref3 = drow[0]["REF_NAME_03"].ToString();
                    Ref4 = drow[0]["REF_NAME_04"].ToString();
                }

                if (objZLDSuche.Referenz1 == String.Empty) { addAttributes(txtReferenz1); txtReferenz1.Text = Ref1; } else { txtReferenz1.Text = objZLDSuche.Referenz1; disableDefaultValue(txtReferenz1); }
                if (objZLDSuche.Referenz2 == String.Empty) { addAttributes(txtReferenz2); txtReferenz2.Text = Ref2; } else { txtReferenz2.Text = objZLDSuche.Referenz2; disableDefaultValue(txtReferenz2); }
                if (objZLDSuche.Referenz3 == String.Empty) { addAttributes(txtReferenz3); txtReferenz3.Text = Ref3; } else { txtReferenz3.Text = objZLDSuche.Referenz3; disableDefaultValue(txtReferenz3); }
                if (objZLDSuche.Referenz4 == String.Empty) { addAttributes(txtReferenz4); txtReferenz4.Text = Ref4; } else { txtReferenz4.Text = objZLDSuche.Referenz4; disableDefaultValue(txtReferenz4); }

                if (Ref1.Length == 0) { txtReferenz1.Enabled = false; }
                if (Ref2.Length == 0) { txtReferenz2.Enabled = false; }
                if (Ref3.Length == 0) { txtReferenz3.Enabled = false; }
                if (Ref4.Length == 0) { txtReferenz4.Enabled = false; }

                String[] tmpKennz = objZLDSuche.Kennz.Split('-');
                txtKennz1.Text = "";
                txtKennz2.Text = "";

                if (tmpKennz.Length == 1)
                {
                    if (tmpKennz[0].ToString() == String.Empty) { txtKennz1.Text = "CK"; addAttributes(txtKennz1); } else { txtKennz1.Text = tmpKennz[0].ToString(); disableDefaultValue(txtKennz1); }
                }
                else if (tmpKennz.Length == 2)
                {
                    if (tmpKennz[0].ToString() == String.Empty) { txtKennz1.Text = "CK";  addAttributes(txtKennz1); } else { txtKennz1.Text = tmpKennz[0].ToString(); disableDefaultValue(txtKennz1); }
                    if (tmpKennz[1].ToString() == String.Empty) { txtKennz2.Text = "XX9999";  addAttributes(txtKennz2); } else { txtKennz2.Text = tmpKennz[1].ToString(); disableDefaultValue(txtKennz2); }
                }

                txtBeauftragtVon.Text = objZLDSuche.BeaufVon;
                txtBeauftragtBis.Text = objZLDSuche.BeaufBis;
                txtZuldateVon.Text = objZLDSuche.ZulVon;
                txtZuldateBis.Text = objZLDSuche.ZulBis;
            }


            catch (Exception ex)
            {

                lblError.Text = ex.Message;
            }

        }

        /// <summary>
        /// Fügt Javascript-Funktionen einer Textbox hinzu
        /// </summary>
        /// <param name="txtBox">Control</param>
        private void addAttributes(TextBox txtBox)
        {
            txtBox.Attributes.Add("onblur", "if(this.value=='')this.value=this.defaultValue");
            txtBox.Attributes.Add("onfocus", "if(this.value==this.defaultValue)this.value=''");
        }

        /// <summary>
        /// entfernt den Vorschlagswert der Textbox, wenn Eingabe erfolgte
        /// </summary>
        /// <param name="txtBox">Control</param>
        private void disableDefaultValue(TextBox txtBox)
        {
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), txtBox.ClientID,
        "<script type='text/javascript'>disableDefaultValue('" + txtBox.ClientID + "');</script>", false);
        }

        /// <summary>
        ///  Entfernt den Vorschlagswert der Textbox der gerenderten DropDown, wenn Eingabe erfolgte 
        /// </summary>
        /// <param name="txtBox">z.B. ctl00_ContentPlaceHolder1_ddlKunnr1_Input</param>
        private void disableDefaultValueDDL(String txtBox)
        {
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), txtBox,
                "<script type='text/javascript'>disableDefaultValue('" + txtBox + "');</script>", false);
        }

        /// <summary>
        /// Fügt Vorschlagswert einer Textbox hinzu 
        /// </summary>
        /// <param name="txtBox">Control</param>
        private void enableDefaultValue(TextBox txtBox)
        {
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), txtBox.ClientID,
                "<script type='text/javascript'>enableDefaultValue('" + txtBox.ClientID + "');</script>", false);
        }

        /// <summary>
        /// Validierung Zulassungsdatum
        /// </summary>
        /// <returns></returns>
        private Boolean checkZuldate ()
        { 
            DateTime datZulVon;
            DateTime datZulBis;

            String ZDat = txtZuldateVon.Text;
            if (!ZDat.IsDate())
            {
                divZulDateVon.Attributes["class"] = "formfeld error";
                lblError.Text = "Bitte geben Sie ein Datum im Feld -Zulassungsdatum von:- ein.";
                return true;
            }

            DateTime.TryParse(ZDat, out datZulVon);

            ZDat = txtZuldateBis.Text;
            if (!ZDat.IsDate())
            {
                divZulDateBis.Attributes["class"] = "formfeld error";
                lblError.Text = "Bitte geben Sie ein Datum im Feld -Zulassungsdatum bis:- ein.";
                return true;
            }

            DateTime.TryParse(ZDat, out datZulBis);

            if (datZulVon > datZulBis)
            {
                divZulDateVon.Attributes["class"] = "formfeld error";
                divZulDateBis.Attributes["class"] = "formfeld error";
                lblError.Text ="Zulassungssdatum (von) muß kleiner oder gleich Zulassungsdatum (bis) sein!";
                return true;
            }

            if (datZulBis.Subtract(datZulVon).Days > 90)
            {
                divZulDateVon.Attributes["class"] = "formfeld error";
                divZulDateBis.Attributes["class"] = "formfeld error";
                lblError.Text = "Der angegebene Zeitraum umfasst mehr als 90 Tage!";
                return true;                
            }

            return false;  
        }

        /// <summary>
        /// Validierung Beauftragungsdatum 
        /// </summary>
        /// <returns></returns>
        private Boolean checkBeaufdate()
        {
            DateTime datBeauftrVon;
            DateTime datBeauftrBis;

            String BDat = txtBeauftragtVon.Text;
            if (!BDat.IsDate())
            {
                divBDateVon.Attributes["class"] = "formfeld error";
                lblError.Text = "Bitte geben Sie ein Datum im Feld -Beauftragungsdatum von:- ein.";
                return true;
            }

            DateTime.TryParse(BDat, out datBeauftrVon);

            BDat = txtBeauftragtBis.Text;
            if (!BDat.IsDate())
            {
                divBDateBis.Attributes["class"] = "formfeld error";
                lblError.Text = "Bitte geben Sie ein Datum im Feld -Beauftragungsdatum bis:- ein.";
                return true;
            }
            DateTime.TryParse(BDat, out datBeauftrBis);

            if (datBeauftrVon > datBeauftrBis)
            {
                divBDateVon.Attributes["class"] = "formfeld error";
                divBDateBis.Attributes["class"] = "formfeld error";
                lblError.Text = "Beauftragungsdatum (von) muß kleiner oder gleich Beauftragungsdatum (bis) sein!";
                return true;
            }
            if (datBeauftrBis.Subtract(datBeauftrVon).Days > 90)
            {
                divBDateVon.Attributes["class"] = "formfeld error";
                divBDateBis.Attributes["class"] = "formfeld error";
                lblError.Text = "Der angegebene Zeitraum umfasst mehr als 90 Tage!";
                return true;
            }

            return false;
        }

        /// <summary>
        /// Validierung der Eingabedaten.
        /// </summary>
        /// <returns></returns>
        private Boolean checkInput()
        {

            String ZulVon = txtZuldateVon.Text;
            String ZulBis = txtZuldateBis.Text;
            String BeaufVon = txtBeauftragtVon.Text;
            String BeaufBis = txtBeauftragtBis.Text;
            String Referenz1 = txtReferenz1.Text.ToUpper().Trim();
            String Referenz2 = txtReferenz2.Text.ToUpper().Trim();
            String Referenz3 = txtReferenz3.Text.ToUpper().Trim();
            String Referenz4 = txtReferenz4.Text.ToUpper().Trim();
            String Kunnr = ddlKunnr1.SelectedValue;
            String Kennz1 = txtKennz1.Text.ToUpper().Trim().Replace("CK", "");
            String Kennz2 = txtKennz2.Text.ToUpper().Trim().Replace("XX9999","");
            int inputLength = ZulVon.Length + ZulBis.Length + BeaufVon.Length + BeaufBis.Length +
                                Referenz1.Length + Referenz2.Length + Referenz3.Length + Referenz4.Length +
                                    Kunnr.Length   + Kennz1.Length  + Kennz2.Length ;

            if (inputLength == 0)
                return true;

            return false;
        }

        /// <summary>
        /// Eingabedaten sammeln und in die Klasse(ZLD_Suche) übernehmen. 
        /// Aufruf SAP objZLDSuche.FillStatistik aus der Klasse ZLD_Suche. 
        /// Auswerten des Fehlerstatus und der Ergebnisse (objZLDSuche.Auftragsdaten).
        /// Weiterleitung zur Listenansicht "Zulassungsstatistik_02.aspx".
        /// </summary>
        private void DoSubmit() 
        {
            objZLDSuche = new ZLD_Suche(ref m_User, m_App, "");

            ClearError();
            lblError.Text = "";
            if (checkInput()) 
            {
            lblError.Text = "Bitte geben Sie Daten für die Selektion ein!";
            return;         
            }

            objZLDSuche.Kunnr = ddlKunnr1.SelectedValue;
            objZLDSuche.Referenz1 = "";
            objZLDSuche.Referenz2 = "";
            objZLDSuche.Referenz3 = "";
            objZLDSuche.Referenz4 = "";
            if (objZLDSuche.Kunnr != "")
            {
                DataRow[] drow = objCommon.tblKundenStamm.Select("KUNNR ='" + ddlKunnr1.SelectedValue + "'");
                if (drow.Length > 0)
                {
                    if (txtReferenz1.Text != drow[0]["REF_NAME_01"].ToString())
                    { objZLDSuche.Referenz1 = txtReferenz1.Text.ToUpper().Trim(); }
                    if (txtReferenz2.Text != drow[0]["REF_NAME_02"].ToString())
                    { objZLDSuche.Referenz2 = txtReferenz2.Text.ToUpper(); }
                    if (txtReferenz3.Text != drow[0]["REF_NAME_03"].ToString())
                    { objZLDSuche.Referenz3 = txtReferenz3.Text.ToUpper().Trim(); }
                    if (txtReferenz4.Text != drow[0]["REF_NAME_04"].ToString())
                    { objZLDSuche.Referenz4 = txtReferenz4.Text.ToUpper().Trim(); }
                }
            }
            else 
            {
                objZLDSuche.Referenz1 = txtReferenz1.Text.ToUpper().Trim();
                objZLDSuche.Referenz2 = txtReferenz2.Text.ToUpper().Trim();
                objZLDSuche.Referenz3 = txtReferenz3.Text.ToUpper().Trim();
                objZLDSuche.Referenz4 = txtReferenz4.Text.ToUpper().Trim();
            }
            txtKennz1.Text=txtKennz1.Text.ToUpper().Trim().Replace("CK", "");
            txtKennz2.Text=txtKennz2.Text.ToUpper().Trim().Replace("XX9999", "");
            
            objZLDSuche.Kennz = "";

            if (txtKennz1.Text.ToUpper().Trim().Length > 0 && txtKennz2.Text.Trim().Length == 0)
            {
                lblError.Text = "Bitte geben Sie ein vollständiges Kennzeichen ein!";
                return;
            }
            
            if (txtKennz1.Text.ToUpper().Trim().Length == 0 && txtKennz2.Text.Trim().Length > 0)
            {
                lblError.Text = "Bitte geben Sie ein vollständiges Kennzeichen ein!";
                return;
            }
            
            if (txtKennz1.Text.ToUpper().Trim().Length > 0 && txtKennz2.Text.Trim().Length > 0)
            {
                objZLDSuche.Kennz = txtKennz1.Text.ToUpper().Trim() + "-" + txtKennz2.Text.ToUpper().Trim();
            }

            if ((!String.IsNullOrEmpty(txtZuldateVon.Text)) || (!String.IsNullOrEmpty(txtZuldateBis.Text)))
            {
                if (checkZuldate()) { return; }
                objZLDSuche.ZulVon = txtZuldateVon.Text;
                objZLDSuche.ZulBis = txtZuldateBis.Text;
            }
            if ((!String.IsNullOrEmpty(txtBeauftragtVon.Text)) || (!String.IsNullOrEmpty(txtBeauftragtBis.Text)))
            {
                if (checkBeaufdate()) { return; }
                objZLDSuche.BeaufVon = txtBeauftragtVon.Text;
                objZLDSuche.BeaufBis = txtBeauftragtBis.Text;
            }

            // Es muss mindestens ein Kennz. bzw. eine Referenz oder ein Zeitraum selektiert werden
            if ((String.IsNullOrEmpty(objZLDSuche.Kennz)) && (String.IsNullOrEmpty(objZLDSuche.Referenz1)) && (String.IsNullOrEmpty(objZLDSuche.Referenz2))
                && (String.IsNullOrEmpty(objZLDSuche.Referenz3)) && (String.IsNullOrEmpty(objZLDSuche.Referenz4)))
            {
                if ((String.IsNullOrEmpty(objZLDSuche.ZulVon)) && (String.IsNullOrEmpty(objZLDSuche.BeaufVon)))
                {
                    divZulDateVon.Attributes["class"] = "formfeld error";
                    lblError.Text = "Bitte geben Sie zumindest ein Zulassungs- oder Beauftragungsdatum ein.";
                    return;
                }
            }

            if (rbAlle.Checked) { objZLDSuche.Liste = "1"; }
            if (rbDurch.Checked) { objZLDSuche.Liste = "2"; }
            if (rbOffen.Checked) { objZLDSuche.Liste = "3"; }

            objZLDSuche.FillStatistik(Session["AppID"].ToString(), Session.SessionID, this, objCommon.VKORG, objCommon.VKBUR);
            Session["objZLDSuche"] = objZLDSuche;
            if (objZLDSuche.Status != 0)
            {
                lblError.Text = "Fehler: " + objZLDSuche.Message;
            }
            else
            {
                if (objZLDSuche.Auftragsdaten.Rows.Count == 0)
                {
                    lblError.Text = "Keine Ergebnisse für die gewählten Kriterien.";
                }
                else
                {
                    Response.Redirect("Zulassungsstatistik_02.aspx?AppID=" + Session["AppID"].ToString());
                }
            }
            

        }

        /// <summary>
        /// entfernt das Errorstyle der Controls
        /// </summary>
        private void ClearError()
        {
            divZulDateVon.Attributes["class"] = "formfeld";
            divZulDateBis.Attributes["class"] = "formfeld";
            divBDateVon.Attributes["class"] = "formfeld";
            divZulDateBis.Attributes["class"] = "formfeld";
           
        }

        /// <summary>
        /// cmdSearch_Click - Ereignis. 
        ///  DoSubmit().
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        protected void cmdSearch_Click(object sender, EventArgs e)
        {
            DoSubmit();
        }

        /// <summary>
        /// ddlKunnr1_ItemsRequested - Ereignis
        /// Suchanfrage in der Kundendropdown(Eingabe, Auswahl)
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">RadComboBoxItemsRequestedEventArgs</param>
        protected void ddlKunnr1_ItemsRequested(object sender, RadComboBoxItemsRequestedEventArgs e)
        {

            ddlKunnr1.Items.Clear();

            string text = e.Text;
            DataRow[] rows = objCommon.tblKundenStamm.Select("NAME1 Like '" + text + "*'");
            int itemsPerRequest = 10;
            int itemOffset = e.NumberOfItems;
            int endOffset = itemOffset + itemsPerRequest;
            if (endOffset > rows.Length)
            {
                endOffset = rows.Length;
            }

            for (int i = itemOffset; i < endOffset; i++)
            {
                ddlKunnr1.Items.Add(new RadComboBoxItem(rows[i]["NAME1"].ToString(), rows[i]["KUNNR"].ToString()));
            }

        }

        /// <summary>
        /// ddlKunnr1_SelectedIndexChanged - Ereignis.
        /// Funktionsaufruf: setReferenz().
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">RadComboBoxSelectedIndexChangedEventArgs</param>
        protected void ddlKunnr1_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            setReferenz();
        }

        /// <summary>
        /// Setzen der Hilfstexte(TextBox DefaultValue) für die Referenzfelder je nach Kundenauswahl.
        /// </summary>
        private void setReferenz()
        {
            DataRow[] drow = objCommon.tblKundenStamm.Select("KUNNR ='" + ddlKunnr1.SelectedValue + "'");
            if (drow.Length > 0)
            {
                txtReferenz1.Text = drow[0]["REF_NAME_01"].ToString();
                txtReferenz2.Text = drow[0]["REF_NAME_02"].ToString();
                txtReferenz3.Text = drow[0]["REF_NAME_03"].ToString();
                txtReferenz4.Text = drow[0]["REF_NAME_04"].ToString();
                if (String.IsNullOrEmpty(txtReferenz1.Text)) { txtReferenz1.Enabled = false; } else { txtReferenz1.Enabled = true; }
                if (String.IsNullOrEmpty(txtReferenz2.Text)) { txtReferenz2.Enabled = false; } else { txtReferenz2.Enabled = true; }
                if (String.IsNullOrEmpty(txtReferenz3.Text)) { txtReferenz3.Enabled = false; } else { txtReferenz3.Enabled = true; }
                if (String.IsNullOrEmpty(txtReferenz4.Text)) { txtReferenz4.Enabled = false; } else { txtReferenz4.Enabled = true; }
            }

            addAttributes(txtReferenz1); enableDefaultValue(txtReferenz1);
            addAttributes(txtReferenz2); enableDefaultValue(txtReferenz2);
            addAttributes(txtReferenz3); enableDefaultValue(txtReferenz3);
            addAttributes(txtReferenz4); enableDefaultValue(txtReferenz4);

        }
    }
}