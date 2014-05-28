using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CKG.Base.Kernel.Security;
using AutohausPortal.lib;
using CKG.Base.Kernel.Common;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using Telerik.Web.UI;

namespace AutohausPortal.forms
{
    /// <summary>
    ///  Abmeldung firmeneigene Zulassungen.
    ///  Löschen firmeneigene Zulassungen. 
    ///  Benutzte Klassen AHErfassung und objCommon.
    /// </summary>
    public partial class AbmeldungAHZul : System.Web.UI.Page
    {
        private User m_User;
        private App m_App;
        private AHErfassung objVorerf;
        private ZLDCommon objCommon;


        /// <summary>
        /// Page_Load Ereignis
        /// Überprüfung ob dem User diese Applikation zugeordnet ist
        /// Laden der Stammdaten wenn noch nicht im Session-Object
        /// Laden der firmeneig. Zulassungen aus SAP (objVorerf.GetAbmeldungAH)
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        protected void Page_Load(object sender, EventArgs e)
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
                if (!objCommon.Init(Session["AppID"].ToString(), Session.SessionID.ToString(), this))
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

            if (Session["objVorerf"] != null)
            {

                objVorerf = (AHErfassung)Session["objVorerf"];
            }
            else 
            {
                objVorerf = new AHErfassung(ref m_User, m_App, "");
            }
            

          if (IsPostBack == false)
            {
              // Laden der zum Abmelden bereiten Zulassungen
                objVorerf.GetAbmeldungAH(Session["AppID"].ToString(), Session.SessionID.ToString(), this, m_User.Reference.Substring(0, 4), m_User.Reference.Substring(4, 4));
                Session["objVorerf"] = objVorerf;
                if (objVorerf.Status != 0)
                {
                    lblError.Text = objVorerf.Message;
                    cmdSave.Visible = false;
                }
                else { Fillgrid(0, ""); }
            }
          }

        /// <summary>
        /// bindet die Abmeldedaten an das Gridview(gvZuldienst) 
        /// </summary>
        /// <param name="intPageIndex">Int32</param>
        /// <param name="strSort">String</param>
        private void Fillgrid(Int32 intPageIndex, String strSort)
        {

            DataView tmpDataView = new DataView();
            tmpDataView = objVorerf.Abmeldedaten.DefaultView;
            tmpDataView.RowFilter = "";

            if (tmpDataView.Count == 0)
            {
                gvZuldienst.Visible = false;
                lblError.Text = "Keine Daten vorhanden!";
            }
            else
            {
                gvZuldienst.Visible = true;

                Int32 intTempPageIndex = intPageIndex;
                String strTempSort = "";
                String strDirection = null;

                if (strSort.Trim(' ').Length > 0)
                {
                    intTempPageIndex = 0;
                    strTempSort = strSort.Trim(' ');
                    if ((Session["AbmListeSort"] == null) || ((String)Session["AbmListeSort"] == strTempSort))
                    {
                        if (Session["AbmListeDirection"] == null)
                        {
                            strDirection = "desc";
                        }
                        else
                        {
                            strDirection = (String)Session["AbmListeDirection"];
                        }
                    }
                    else
                    {
                        strDirection = "desc";
                    }

                    if (strDirection == "asc")
                    {
                        strDirection = "desc";
                    }
                    else
                    {
                        strDirection = "asc";
                    }

                    Session["AbmListeSort"] = strTempSort;
                    Session["AbmListeDirection"] = strDirection;
                }
                else if (Session["AbmListeSort"] != null)
                {
                    strTempSort = Session["VorerfSort"].ToString();
                    strDirection = Session["AbmListeDirection"].ToString();
                }
                if (strTempSort.Length != 0)
                {
                    tmpDataView.Sort = strTempSort + " " + strDirection;
                }

                gvZuldienst.PageIndex = intTempPageIndex;
                gvZuldienst.DataSource = tmpDataView;
                gvZuldienst.DataBind();

            }
        }

        /// <summary>
        /// cmdCancel_Click Ereignis - zurück
        /// Zurück zur Auftragsliste(Auftraege.aspx) 
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        protected void cmdCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("/AutohausPortal/(S(" + Session.SessionID + "))/Start/Selection.aspx"); 
        }

        /// <summary>
        /// Button "Abmelden"
        /// ausgewählte Zulassungen in SAP abmelden(objVorerf.SaveAbmeldungAH)
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        protected void cmdSave_Click(object sender, EventArgs e)
        {
            string errmsg = "";

            lblError.Text = "";
            lblMessage.Text = "";
            objVorerf = (AHErfassung)Session["objVorerf"];
            objVorerf.VKBUR = m_User.Reference.Substring(4, 4);
            objVorerf.VKORG = m_User.Reference.Substring(0, 4);

            if (checkToSave("A", ref errmsg))
            {
                objVorerf.SaveAbmeldungAH(Session["AppID"].ToString(), Session.SessionID.ToString(), this);
                if (objVorerf.Status != 0)
                {

                    if (objVorerf.Status == -5555)
                    {
                        lblError.Text = "Kommunikationfehler: Daten konnten nicht in SAP gespeichert werden!" + objVorerf.Message;
                        return;
                    }
                     

                    lblError.Text = objVorerf.Message;
                    Fillgrid(0, "");
                    gvZuldienst.Columns[1].Visible = true;
                }
                else
                {
                    lblMessage.Visible = true;
                    lblMessage.ForeColor = System.Drawing.ColorTranslator.FromHtml("#269700");
                    lblMessage.Text = "Datensätze gespeichert. Keine Fehler aufgetreten.";
                    DataRow[] rowListe = objVorerf.Abmeldedaten.Select("Status = ''");

                    if (rowListe.Length > 0)
                    {
                        foreach (DataRow dRow in rowListe)
                        {
                            Int32 id;
                            Int32.TryParse(dRow["ZULBELN"].ToString(), out id);
                            if (dRow["Auswahl"].ToString() == "A")
                            {
                                objVorerf.Abmeldedaten.Rows.Remove(dRow);
                            }

                        }

                    }

                    if (objVorerf.Abmeldedaten.Rows.Count > 0)
                    {
                        Fillgrid(0, "");
                        gvZuldienst.Columns[1].Visible = false;
                    }
                    else 
                    {
                        cmdSave.Enabled = false;
                        cmdDelete.Enabled = false;
                        gvZuldienst.Visible = false;                    
                    }

                    Session["objVorerf"] = objVorerf;

                    RadWindow downloaddoc = RadWindowManager1.Windows[0];
                    downloaddoc.Visible = true;
                    downloaddoc.VisibleOnPageLoad = true;

                }

            }
            else
            {
                if (String.IsNullOrEmpty(errmsg))
                {
                    lblError.Text = "Sie haben keine Auträge zum Abmelden markiert!";
                }
                else
                {
                    lblError.Text = errmsg;
                }
            }
        }

        /// <summary>
        /// Setzt das Tabellenfeld "Auswahl" mit Parameter "Auswahl" gleich.
        /// Um im Bapi mitzugeben ob der Vorgang gelöscht oder abgemeldet werden kann.
        /// </summary>
        /// <param name="Auswahl">A für Abmelden oder L für Löschen</param>
        /// <param name="fehlermeldung"></param>
        /// <returns>Auswahl getroffen und Daten ok: true/false</returns>
        private Boolean checkToSave(String Auswahl, ref string fehlermeldung)
        {
            fehlermeldung = "";
            Boolean breturn = false;
            foreach (GridViewRow drow in gvZuldienst.Rows) 
            {
                Label lblID = (Label)drow.FindControl("lblID");
                CheckBox chkAuswahl = (CheckBox)drow.FindControl("chkAuswahl");
                TextBox txtKennz = (TextBox)drow.FindControl("txtKennzeichen");

                if (chkAuswahl != null && chkAuswahl.Checked) 
                {
                    DataRow[] rowListe = objVorerf.Abmeldedaten.Select("ZULBELN = " + lblID.Text);
                    if (rowListe.Length == 1)
                    {
                        rowListe[0]["Auswahl"] = Auswahl;
                        // bei Abmeldung Kennzeichen prüfen/übernehmen
                        if (Auswahl == "A")
                        {
                            string kennz = txtKennz.Text.ToUpper();
                            if (objCommon.checkKennzeichenformat(kennz))
                            {
                                rowListe[0]["ZZKENN"] = kennz;
                            }
                            else
                            {
                                fehlermeldung = "Das Kennzeichen hat das falsche Format";
                                return false;
                            }
                        }
                        breturn = true;
                    }
                }
            
            }
            return breturn;
        }

        /// <summary>
        /// gvZuldienst_Sorting - Ereignis
        /// Sortieren der Eintäge im Gridview(gvZuldienst)
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">GridViewSortEventArgs</param>
        protected void gvZuldienst_Sorting(object sender, GridViewSortEventArgs e)
        {
            Fillgrid(0, e.SortExpression);
        }

        /// <summary>
        ///  cmdDelete_Click - Ereignis
        ///  ausgewählte Zulassungen in SAP löschen(objVorerf.SaveAbmeldungAH)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void cmdDelete_Click(object sender, EventArgs e)
        {
            string errmsg = "";

            // ausgewählte Zulassungen löschen
            lblError.Text = "";
            lblMessage.Text = "";
            objVorerf = (AHErfassung)Session["objVorerf"];
            objVorerf.VKBUR = m_User.Reference.Substring(4, 4);
            objVorerf.VKORG = m_User.Reference.Substring(0, 4);
            if (checkToSave("L", ref errmsg))
            {
                objVorerf.SaveAbmeldungAH(Session["AppID"].ToString(), Session.SessionID.ToString(), this);
                if (objVorerf.Status != 0)
                {

                    if (objVorerf.Status == -5555)
                    {
                        lblError.Text = "Kommunikationfehler: Daten konnten nicht in SAP gespeichert werden!" + objVorerf.Message;
                        return;
                    }


                    lblError.Text = objVorerf.Message;
                    Fillgrid(0, "");
                    gvZuldienst.Columns[1].Visible = true;
                }
                else
                {
                    lblMessage.Visible = true;
                    lblMessage.ForeColor = System.Drawing.ColorTranslator.FromHtml("#269700");
                    lblMessage.Text = "Datensätze gespeichert. Keine Fehler aufgetreten.";
                    DataRow[] rowListe = objVorerf.Abmeldedaten.Select("Status = ''");

                    if (rowListe.Length > 0)
                    {
                        foreach (DataRow dRow in rowListe)
                        {
                            Int32 id;
                            Int32.TryParse(dRow["ZULBELN"].ToString(), out id);
                            if (dRow["Auswahl"].ToString() == "L")
                            {
                                objVorerf.Abmeldedaten.Rows.Remove(dRow);
                            }

                        }

                    }

                    if (objVorerf.Abmeldedaten.Rows.Count > 0)
                    {
                        Fillgrid(0, "");
                        gvZuldienst.Columns[1].Visible = false;
                    }
                    else
                    {
                        cmdSave.Enabled = false;
                        cmdDelete.Enabled = false;
                        gvZuldienst.Visible = false;
                    }

                    Session["objVorerf"] = objVorerf;
                    RadWindow downloaddoc = RadWindowManager1.Windows[0];
                    downloaddoc.Visible = false;
                    downloaddoc.VisibleOnPageLoad = false;
                }

            }
            else
            {
                lblError.Text = "Sie haben keine Auträge zum Löschen markiert!";

            }
        }

    }
}