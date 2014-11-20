using System;
using System.Collections.Generic;
using CKG.Base.Kernel.Common;
using CKG.Base.Kernel.Security;
using AppZulassungsdienst.lib;
using System.Data;
using System.IO;
using SmartSoft.PdfLibrary;

namespace AppZulassungsdienst.forms
{
    /// <summary>
    /// Report für das Drucken von Lieferscheinen.
    /// </summary>
    public partial class ReportLieferschein : System.Web.UI.Page
    {
        private User m_User;
        private App m_App;
        private Listen objListe; 
        private ZLDCommon objCommon;

        /// <summary>
        /// Page_Load Ereignis. Prüfen ob die Anwendung dem Benutzer zugeordnet ist. Stammdaten laden.
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            m_User = Common.GetUser(this);

            Common.FormAuth(this, m_User);

            m_App = new App(m_User); //erzeugt ein App_objekt 

            Common.GetAppIDFromQueryString(this);

            lblHead.Text = (string)m_User.Applications.Select("AppID = '" + Session["AppID"] + "'")[0]["AppFriendlyName"];
            if (m_User.Reference.Trim(' ').Length == 0)
            {
                lblError.Text = "Es wurde keine Benutzerreferenz angegeben! Somit können keine Stammdaten ermittelt werden!";
                return;
            }

            if (Session["objCommon"] == null)
            {

                objCommon = new ZLDCommon(ref m_User, m_App);
                objCommon.VKBUR = m_User.Reference.Substring(4, 4);
                objCommon.VKORG = m_User.Reference.Substring(0, 4);
                objCommon.getSAPDatenStamm(Session["AppID"].ToString(), Session.SessionID, this);
                objCommon.getSAPZulStellen(Session["AppID"].ToString(), Session.SessionID, this);
                objCommon.LadeKennzeichenGroesse();
                objCommon.GetGruppen_Touren(Session["AppID"].ToString(), Session.SessionID, this,"K");
                objCommon.GetGruppen_Touren(Session["AppID"].ToString(), Session.SessionID, this, "T");
                Session["objCommon"] = objCommon;
            }
            else
            {
                objCommon = (ZLDCommon)Session["objCommon"];
                if (objCommon.tblKdGruppeforSelection == null) 
                { 
                    objCommon.GetGruppen_Touren(Session["AppID"].ToString(), Session.SessionID, this, "K");
                }
                if (objCommon.tblTourenforSelection == null)
                {
                    objCommon.GetGruppen_Touren(Session["AppID"].ToString(), Session.SessionID, this, "T");
                }
                
            }

            if (IsPostBack == false)
            {
                FillDropDowns();
            }
            SetAttributes();
        }

        /// <summary>
        /// Javascript-Funktionen an Controls binden.
        /// </summary>
        private void SetAttributes() 
        {
            txtStVavon.Attributes.Add("onkeyup", "FilterKennz(this,event)");
            txtStVaBis.Attributes.Add("onkeyup", "FilterKennz(this,event)");
            lbtnGestern.Attributes.Add("onclick", "SetDate( -1,'" + txtZulDate.ClientID + "'); return false;");
            lbtnHeute.Attributes.Add("onclick", "SetDate( 0,'" + txtZulDate.ClientID + "'); return false;");
            lbtnMorgen.Attributes.Add("onclick", "SetDate( +1,'" + txtZulDate.ClientID + "'); return false;");
            lbtnGesternbis.Attributes.Add("onclick", "SetDate( -1,'" + txtZulDateBis.ClientID + "'); return false;");
            lbtnHeutebis.Attributes.Add("onclick", "SetDate( 0,'" + txtZulDateBis.ClientID + "'); return false;");
            lbtnMorgenbis.Attributes.Add("onclick", "SetDate( +1,'" + txtZulDateBis.ClientID + "'); return false;");
        }

        /// <summary>
        /// DropDowns an Stammdatentabellen binden.
        /// </summary>
        private void FillDropDowns()
        {

            
            ddlGruppe.DataSource = objCommon.tblKdGruppeforSelection;
            ddlGruppe.DataValueField = "GRUPPE";
            ddlGruppe.DataTextField = "BEZEI";
            ddlGruppe.DataBind();
            ddlGruppe.SelectedValue = "0";          

            ddlTour.DataSource = objCommon.tblTourenforSelection;
            ddlTour.DataValueField = "GRUPPE";
            ddlTour.DataTextField = "BEZEI";
            ddlTour.DataBind();
            ddlTour.SelectedValue = "0";

        }

        /// <summary>
        /// Funktionsaufruf DoSubmit().
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void cmdCreate_Click(object sender, EventArgs e)
        {
            DoSubmit();
        }

        /// <summary>
        /// Selektionsdaten sammeln, validieren und an SAP übergeben(Z_ZLD_EXPORT_LS).
        /// </summary>
        private void DoSubmit()
        {

            lblError.Text = "";
            objListe = new Listen(ref m_User, m_App, Session["AppID"].ToString(), Session.SessionID, "");
            objListe.KennzeichenVon = "";
            objListe.KennzeichenBis = "";
            objListe.KundeVon = "";
            objListe.KundeBis = "";
            objListe.Zuldat = "";
            objListe.ZuldatBis = "";
            objListe.SelGroupTourID = "";

            objListe.KennzeichenVon = txtStVavon.Text.Trim(' ');
            objListe.KennzeichenBis = txtStVaBis.Text.Trim(' ');

            if (txtKunnr.Text.Trim(' ').Length + txtKunnrBis.Text.Trim(' ').Length == 0 && ddlTour.SelectedValue == "0" && ddlGruppe.SelectedValue =="0")
            {
                lblError.Text = "Bitte geben Sie min. eine Kundenummer ein!";
                return;
            }

            if ((txtKunnr.Text.Trim(' ').Length + txtKunnrBis.Text.Trim(' ').Length > 0 && ddlGruppe.SelectedValue != "0"))
            {
                lblError.Text = "Bitte wählen Sie entweder Gruppe oder Kunde aus!";
                return;
            }

            if ((txtKunnr.Text.Trim(' ').Length + txtKunnrBis.Text.Trim(' ').Length > 0 && ddlTour.SelectedValue != "0"))
            {
                lblError.Text = "Bitte wählen Sie entweder Tour oder Kunde aus!";
                return;
            }

            if (ddlTour.SelectedValue != "0" && ddlGruppe.SelectedValue != "0")
            {
                lblError.Text = "Bitte wählen Sie entweder Tour oder Gruppe aus!";
                return;
            }

            if (ddlGruppe.SelectedValue != "0")
            {
                objListe.SelGroupTourID = ddlGruppe.SelectedValue;
            }
            else if (ddlTour.SelectedValue != "0" )
            {
                objListe.SelGroupTourID = ddlTour.SelectedValue;
            }

            objListe.KundeVon = txtKunnr.Text.Trim(' ');
            objListe.KundeBis = txtKunnrBis.Text.Trim(' ');

            if (txtZulDate.Text.Trim(' ').Length == 0)
            {
                lblError.Text = "Bitte geben Sie ein Zulassungsdatum ein!";
                return;
            }

            if (txtZulDate.Text.Trim(' ').Length < 6)
            {
                lblError.Text = "Bitte geben Sie das Zulassungsdatum 6-stellig ein!";
                return;
            }

            if (txtZulDateBis.Text.Trim(' ').Length > 0 && txtZulDateBis.Text.Trim(' ').Length < 6)
            {
                lblError.Text = "Bitte geben Sie das Zulassungsdatum bis 6-stellig ein!";
                return;
            }

            if (txtZulDateBis.Text.Trim(' ').Length == 0)
            {
                objListe.ZuldatBis = "";
            }
            else 
            {
                objListe.ZuldatBis = ZLDCommon.toShortDateStr(txtZulDateBis.Text);
            }
            objListe.Zuldat = ZLDCommon.toShortDateStr(txtZulDate.Text);

            String Liefart = "1";

            if (rbohneGeb.Checked) { Liefart = "1"; }
            if (rbmitGebHoch.Checked) { Liefart = "3"; }
            if (rbmitGebQuer.Checked) { Liefart = "2"; }

            objListe.VKBUR = m_User.Reference.Substring(4, 4);
            objListe.VKORG = m_User.Reference.Substring(0, 4);
            objListe.FillLieferschein(Session["AppID"].ToString(), Session.SessionID, this, Liefart);

            if (objListe.Status != 0)
            {
                lblError.Text = "Fehler: " + objListe.Message;
            }
            else
            {
                if (objListe.Lieferscheine.Rows.Count == 0)
                {
                    lblError.Text = "Keine Ergebnisse für die gewählten Kriterien.";
                }
                else
                {
                    Session["objListe"] = objListe;
                    MergePDFs();
                    GetPDF();
                }
            }

        }

        /// <summary>
        /// Einzeilne Lieferscheine zusammenfügen.
        /// </summary>
        private void MergePDFs()
        {
            try
            {
                List<String> files = new List<String>();
                List<byte[]> filesByte = new List<byte[]>();
                string sPath;

                if (m_User.IsTestUser)
                {
                    sPath = "\\\\192.168.10.96\\test\\portal\\zld\\lieferscheine\\";
                }
                else
                {
                    sPath = "\\\\192.168.10.96\\prod\\portal\\zld\\lieferscheine\\";
                }

                foreach (DataRow item in objListe.Lieferscheine.Rows)
                {
                    string dateiPfad = sPath + item["FILENAME"];

                    if (File.Exists(dateiPfad))
                    {
                        files.Add(dateiPfad);
                    }

                }

                foreach (string sFile in files)
                {
                    filesByte.Add(File.ReadAllBytes(sFile));
                }

                string speicherPfad = sPath + "Lieferschein_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".pdf";

                File.WriteAllBytes(speicherPfad, PdfMerger.MergeFiles(filesByte, false));

                Session["App_ContentType"] = "Application/pdf";
                Session["App_Filepath"] = speicherPfad;

                foreach (string sFile in files)
                {
                   File.Delete(sFile) ;
                }

            }
            catch (Exception Ex)
            {
                lblError.Text = "Generierung des Dokumentes fehlgeschlagen: " + Ex.Message;
            }

        }

        /// <summary>
        /// Aufruf von PrintPDF.aspx zur Anzeige des PDF´s.
        /// </summary>
        private void GetPDF()
        {

            try
            {
                string sPath = Session["App_Filepath"].ToString();
                Session["App_ContentType"] = "Application/pdf";
                Session["App_Filepath"] = sPath;
                Session["App_FileDelete"] = "X";

                ResponseHelper.Redirect("Printpdf.aspx", "_blank", "left=0,top=0,resizable=YES,scrollbars=YES");

            }
            catch (Exception Ex)
            {
                lblError.Text = "Generierung des Dokumentes fehlgeschlagen: " + Ex.Message;
            }

        }
    }
}
