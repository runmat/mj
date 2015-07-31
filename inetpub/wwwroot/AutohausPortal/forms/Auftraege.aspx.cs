using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using CKG.Base.Kernel.Security;
using AutohausPortal.lib;
using CKG.Base.Kernel.Common;
using System.Data;
using Telerik.Web.UI;

namespace AutohausPortal.forms
{
    /// <summary>
    /// Übersicht der erfassten Vorgänge des Benutzers. Benutzte Klassen AHErfassung und objCommon.
    /// </summary>
    public partial class Auftraege : Page
    {
        private User m_User;
        private App m_App;
        private AHErfassung objVorerf;
        private ZLDCommon objCommon;

        /// <summary>
        /// Page_Load Ereignis
        /// Überprüfung ob dem User diese Applikation zugeordnet ist
        /// Laden der Stammdaten wenn noch nicht im Session-Object
        /// Laden der erfassten Vorgänge aus der SQL-DB (objVorerf.LadeVorerfassungDB_ZLD)
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

            if (Session["objVorerf"] != null)
            {
                objVorerf = (AHErfassung)Session["objVorerf"];
            }
            else 
            {
                objVorerf = new AHErfassung(ref m_User, m_App, "", "");
            }
            
            if (!IsPostBack)
            {
                objVorerf.LoadVorgaengeFromSap(Session["AppID"].ToString(), Session.SessionID, this, objCommon.tblKundenStamm);
                Session["objVorerf"] = objVorerf;
                if (objVorerf.Status != 0)
                {
                    lblError.Text = objVorerf.Message;
                    cmdSave.Visible = false;
                }
                else 
                { 
                    Fillgrid(0, "", null); 
                }   
            }
            else
            {
                // beim AsyncPostBack Controls neu rendern
                Boolean IsInAsync = ScriptManager.GetCurrent(this).IsInAsyncPostBack;
                if (IsInAsync) 
                { 
                ScriptManager.RegisterStartupScript(phrJsRunner, phrJsRunner.GetType(), "initiate3",
                    "initiate3();", true);
                }
            }
        }

        /// <summary>
        /// bindet die Vorgangsdaten an das Gridview(gvZuldienst) 
        /// </summary>
        /// <param name="intPageIndex">Int32</param>
        /// <param name="strSort">String</param>
        /// <param name="Rowfilter"></param>
        private void Fillgrid(Int32 intPageIndex, String strSort, String Rowfilter)
        {
            DataView tmpDataView = new DataView();
            tmpDataView = objVorerf.tblEingabeListe.DefaultView;
            String strFilter = "";
            if (Rowfilter != null)
            {
                strFilter = Rowfilter;
            }

            tmpDataView.RowFilter = strFilter;

            if (tmpDataView.Count == 0)
            {
                divExcelExport.Visible = false;
                gvZuldienst.Visible = false;         
                lblError.Text = "Keine Daten vorhanden!";
            }
            else
            {
                divExcelExport.Visible = true;
                gvZuldienst.Visible = true;

                Int32 intTempPageIndex = intPageIndex;
                String strTempSort = "";
                String strDirection = null;

                if (strSort.Trim(' ').Length > 0)
                {
                    intTempPageIndex = 0;
                    strTempSort = strSort.Trim(' ');
                    if ((Session["VorerfSort"] == null) || ((String)Session["VorerfSort"] == strTempSort))
                    {
                        if (Session["VorerfDirection"] == null)
                        {
                            strDirection = "desc";
                        }
                        else
                        {
                            strDirection = (String)Session["VorerfDirection"];
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

                    Session["VorerfSort"] = strTempSort;
                    Session["VorerfDirection"] = strDirection;
                }
                else if (Session["VorerfSort"] != null)
                {
                    strTempSort = Session["VorerfSort"].ToString();
                    strDirection = Session["VorerfDirection"].ToString();
                }
                if (strTempSort.Length != 0)
                {
                    tmpDataView.Sort = strTempSort + " " + strDirection;
                }

                gvZuldienst.PageIndex = intTempPageIndex;
                gvZuldienst.DataSource = tmpDataView;
                gvZuldienst.DataBind();

                // Erfasser nur anzeigen, wenn Benutzer alle Vorgänge des VkBurs bzw. der Gruppe bearbeitet
                gvZuldienst.Columns[8].Visible = m_User.Organization.OrganizationName.ToUpper().Contains(("SENDFORALL"));
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
        /// cmdSave_Click - Ereignis("Absenden")
        /// ausgewählte Vorgänge in das SAP schieben(objVorerf.SaveZLDVorerfassung)
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        protected void cmdSave_Click(object sender, EventArgs e)
        {
            // Absenden der ausgewählten Aufträge
            lblError.Text = "";
            lblMessage.Text = "";
            objVorerf = (AHErfassung)Session["objVorerf"];
            objVorerf.VKBUR = m_User.Reference.Substring(4, 4);
            objVorerf.VKORG = m_User.Reference.Substring(0, 4);
            if (checkToSave())
            {
                objVorerf.SendVorgaengeToSap(Session["AppID"].ToString(), Session.SessionID, this, objCommon.tblStvaStamm);
                if (objVorerf.Status != 0)
                {
                    if (objVorerf.Status == -5555)
                    {
                        lblError.Text = "Kommunikationfehler: Daten konnten nicht in SAP gespeichert werden!" + objVorerf.Message;
                        return;
                    }
                     
                    DataRow[] rowListe = objVorerf.tblEingabeListe.Select("Status <> 'OK' AND Status <>''");

                    if (rowListe.Length > 0)
                    {
                        lblError.Text = "Es konnten ein oder mehrere Aufträge nicht in SAP gespeichert werden";

                    }
                    rowListe = objVorerf.tblEingabeListe.Select("Status = 'OK'");

                    if (rowListe.Length > 0)
                    {
                        foreach (DataRow dRow in rowListe)
                        {
                            objVorerf.tblEingabeListe.Rows.Remove(dRow);
                        }

                        lblMessage.Visible = true;
                        lblMessage.ForeColor = System.Drawing.ColorTranslator.FromHtml("#269700");
                        lblMessage.Text = "Datensätze in SAP gespeichert. Keine Fehler aufgetreten.";
                        getAuftraege();
                        Session["objVorerf"] = objVorerf;

                        RadWindow downloaddoc = RadWindowManager1.Windows[0];
                        downloaddoc.Visible = true;
                        downloaddoc.VisibleOnPageLoad = true;
                    }
                    Fillgrid(0, "", "Status = 'OK' OR Status <>''  ");
                    gvZuldienst.Columns[0].Visible = true;
                    gvZuldienst.Columns[1].Visible = false;
                    gvZuldienst.Columns[9].Visible = false;
                    cmdSave.Enabled = false;
                    cmdContinue.Visible = true;
                }
                else
                {
                    lblMessage.Visible = true;
                    lblMessage.ForeColor = System.Drawing.ColorTranslator.FromHtml("#269700");
                    lblMessage.Text = "Datensätze gespeichert. Keine Fehler aufgetreten.";
                    DataRow[] rowListe = objVorerf.tblEingabeListe.Select("Status = ''");

                    if (rowListe.Length > 0)
                    {
                        foreach (DataRow dRow in rowListe)
                        {
                            if (dRow["toSave"].ToString() == "1")
                            {
                                objVorerf.tblEingabeListe.Rows.Remove(dRow);
                            }
                        }
                    }

                    if (objVorerf.tblEingabeListe.Rows.Count > 0)
                    {
                        Fillgrid(0, "", null);
                    }
                    else 
                    {
                        cmdSave.Enabled = false;
                        gvZuldienst.Visible = false;                    
                    }

                    getAuftraege();
                    Session["objVorerf"] = objVorerf;
                    //Öffnen des Druckdialogs: PrintDialog.aspx
                    RadWindow downloaddoc = RadWindowManager1.Windows[0];
                    downloaddoc.Visible = true;
                    downloaddoc.VisibleOnPageLoad = true;
                }
            }
            else
            {
                lblError.Text = "Sie haben keine Auträge zum Absenden markiert!";
            }
        }

        /// <summary>
        /// Setzt das Tabellenfeld "toSave" auf "1" wenn der Vorgang zum speichern markiert ist.
        /// </summary>
        /// <returns>Auswahl getroffen true/false</returns>
        private Boolean checkToSave() 
        {
            Boolean breturn = false;
            foreach (GridViewRow drow in gvZuldienst.Rows) 
            {
                Label lblID = (Label)drow.FindControl("lblID");
                CheckBox chkAuswahl = (CheckBox)drow.FindControl("chkAuswahl");

                if (chkAuswahl != null && chkAuswahl.Checked) 
                {
                    DataRow[] rowListe = objVorerf.tblEingabeListe.Select("ID = " + lblID.Text);
                    if (rowListe.Length == 1)
                    { 
                        rowListe[0]["toSave"] = 1;
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
            Fillgrid(0, e.SortExpression, null);
        }

        /// <summary>
        /// gvZuldienst_RowCommand - Ereignis. 
        /// e.CommandName == "Bearbeiten" - Selectieren der ID und der AppID des gewählten Vorganges.
        /// Weiterleiten auf die jeweilige Seite(AppURL) um den Vorgang zu bearbeiten. 
        /// e.CommandName == "Loeschen" - Loeschen des ausgewählten Vorganges.
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">GridViewCommandEventArgs</param>
        protected void gvZuldienst_RowCommand(object sender, GridViewCommandEventArgs e)
        {  
            Int32 Index;
            Int32.TryParse(e.CommandArgument.ToString(), out Index);
            Label ID = (Label)gvZuldienst.Rows[Index].FindControl("lblID");
            objVorerf = (AHErfassung )Session["objVorerf"];
            if (e.CommandName == "Bearbeiten")
            {
                DataRow [] drow = objVorerf.tblEingabeListe.Select("ID=" + ID.Text);
                if (drow.Length==0){ lblError.Text = "Es ist ein Fehler aufgetreten. Der Vorgang konnte nicht geladen werden"; return;}
                DataTable AppTable = m_User.Applications.Copy();
                DataRow[] appRows;
                appRows = AppTable.Select("AppID=" + drow[0]["AppID"].ToString() + " AND AppInMenu=1");
                Response.Redirect(appRows[0]["AppURL"].ToString() + "?AppID=" + appRows[0]["AppID"].ToString() + "&ID=" + ID.Text + "&B=true&BackAppID="+ Session["AppID"].ToString() );
            }
            if (e.CommandName == "Loeschen")
            {
                Label lblIDPos = (Label)gvZuldienst.Rows[Index].FindControl("lblid_pos");
                Int32 IDSatz;
                Int32 IDPos;
                Int32.TryParse(ID.Text, out IDSatz);
                Int32.TryParse(lblIDPos.Text, out IDPos);

                hfID.Value = ID.Text;

                // MessageBox initialisieren
                ScriptManager.RegisterStartupScript(phrJsRunner, phrJsRunner.GetType(), "jsopenkDialog",
                                    "openDialogAndBlock(' ');", true);
                
                ScriptManager.RegisterStartupScript(phrJsRunner, phrJsRunner.GetType(), "jsUnblockDialog",
                                    "unblockDialog();", true);
            }
        }

        protected void gvZuldienst_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                // auch ID-Label auf Click reagieren lassen -> Auswahl-Checkbox entspr. umschalten
                Label lbl = (Label) e.Row.FindControl("lblName");
                lbl.Attributes["onclick"] = "ChangeCheckState(" + (e.Row.RowIndex + 1).ToString() + ");";
            }
        }

        /// <summary>
        /// Sicherheitsabfrage ob man den Vorgang löschen möchte. 
        /// Löschen des Vorgangs, Grid neu binden, Auftragsanzahl neu ermitteln. 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void cmdSavePos_Click(object sender, EventArgs e)
        {
            Int32 id;
            Int32.TryParse(hfID.Value, out id);
            objVorerf.DeleteVorgang(Session["AppID"].ToString(), Session.SessionID, this, id);
            DataRow[] rowListe = objVorerf.tblEingabeListe.Select("ID = " + id);
            if (rowListe.Length > 0)
            {
                foreach (DataRow dRow in rowListe)
                {
                    objVorerf.tblEingabeListe.Rows.Remove(dRow);
                }
            }
            Fillgrid(0, "", null);
            getAuftraege();
            ScriptManager.RegisterStartupScript(phrJsRunner, phrJsRunner.GetType(), "jsCloseDialg", "closeDialog();", true);
        }

        /// <summary>
        /// Sicherheitsabfrage ob man den Vorgang löschen möchte. 
        /// Schliessen des Dialoges. 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void cmdCloseDialog_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(phrJsRunner, phrJsRunner.GetType(), "jsCloseDialg", "closeDialog();", true);
        }

        /// <summary>
        /// Läd Anzahl der angelegten Aufträge / Anzeige in der Masterpage 
        /// </summary>
        private void getAuftraege()
        {
            HyperLink lnkMenge = (HyperLink)Master.FindControl("lnkMenge");
            var menge = objVorerf.GetAnzahlAuftraege(Session["AppID"].ToString(), Session.SessionID, this);
            Session["AnzahlAuftraege"] = menge;
            lnkMenge.Text = menge;
        }

        /// <summary>
        /// Nach dem absenden der Vorgänge werden alle Vorgängen mit dem Status OK werden aus der Tabelle entfernt. 
        /// Anzeige der noch vorhandenen Vorgänge im Grid aktualisiert
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        protected void cmdContinue_Click(object sender, EventArgs e)
        {
            lblMessage.Text = "";
            lblError.Text = "";
            objVorerf = (AHErfassung )Session["objVorerf"];
            DataRow[] rowListe = objVorerf.tblEingabeListe.Select("Status = 'OK'");

            if (rowListe.Length > 0)
            {
                foreach (DataRow dRow in rowListe)
                {
                    Int32 id;
                    Int32.TryParse(dRow["id"].ToString(), out id);
                    DataRow[] rowPos = objVorerf.tblEingabeListe.Select("id = " + id);
                    foreach (DataRow dRowsToDel in rowPos)
                    {
                        objVorerf.tblEingabeListe.Rows.Remove(dRowsToDel);
                    }                
                }
            }
            String strFilter= "" ;
            objVorerf.tblEingabeListe.DefaultView.RowFilter = strFilter;
            if (objVorerf.tblEingabeListe.DefaultView.Count == 0)
            {
                Fillgrid(0, "", null);
                lblError.Text = "Keine Daten vorhanden!";
            }
            else 
            {
                cmdSave.Enabled = true;
                Fillgrid(0, "", null);
            }
            cmdContinue.Visible = false;
            gvZuldienst.Columns[0].Visible = false;
            gvZuldienst.Columns[1].Visible = true;
            gvZuldienst.Columns[9].Visible = true;
            RadWindow downloaddoc = RadWindowManager1.Windows[0];
            downloaddoc.Visible = false;
            downloaddoc.VisibleOnPageLoad = false ;
        }

        protected void lnkCreateExcel_Click(object sender, EventArgs e)
        {
            bool found;

            DataTable tblTemp = ((AHErfassung)Session["objVorerf"]).tblEingabeListe.Copy();

            for (int i = tblTemp.Columns.Count - 1; i >= 0; i--)
            {
                found = false;

                foreach (DataControlField dcf in gvZuldienst.Columns)
                {
                    if ((!String.IsNullOrEmpty(dcf.SortExpression)) && (dcf.SortExpression.ToUpper() == tblTemp.Columns[i].ColumnName.ToUpper()))
                    {
                        found = true;

                        if (!dcf.Visible)
                        {
                            tblTemp.Columns.Remove(tblTemp.Columns[i]);
                        }
                        else
                        {
                            tblTemp.Columns[i].ColumnName = dcf.HeaderText;
                        }

                        break;
                    }
                }

                if (!found)
                {
                    tblTemp.Columns.Remove(tblTemp.Columns[i]);
                }
            }
            tblTemp.AcceptChanges();
            
            CKG.Base.Kernel.DocumentGeneration.ExcelDocumentFactory excelFactory = new CKG.Base.Kernel.DocumentGeneration.ExcelDocumentFactory();
            string filename = String.Format("{0:yyyyMMdd_HHmmss_}", DateTime.Now) + m_User.UserName;
            excelFactory.CreateDocumentAndSendAsResponse(filename, tblTemp, this.Page);
        }
    }
}