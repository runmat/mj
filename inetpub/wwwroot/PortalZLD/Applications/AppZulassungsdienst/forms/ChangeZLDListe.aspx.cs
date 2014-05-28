using System;
using System.Web.UI.WebControls;
using CKG.Base.Kernel.Common;
using CKG.Base.Kernel.Security;
using AppZulassungsdienst.lib;
using System.Data;
using CKG.Base.Business;


namespace AppZulassungsdienst.forms
{
    public partial class ChangeZLDListe : System.Web.UI.Page
    {
        private CKG.Base.Kernel.Security.User m_User;
        private CKG.Base.Kernel.Security.App m_App;
        private VoerfZLD objVorerf;
        private ZLDCommon objCommon;

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
                Session["objCommon"] = objCommon;
            }
            else
            {
                objCommon = (ZLDCommon)Session["objCommon"];

            }

            if (Session["objVorerf"] == null)
            {
                //Session-Variable weg (Session vermutlich abgelaufen) -> zurück zur 1. Seite
                Response.Redirect("Change01ZLD.aspx?AppID=" + Session["AppID"].ToString());
            }

            objVorerf = (VoerfZLD)Session["objVorerf"];

            if (IsPostBack == false)
            {
                objVorerf.LadeVorerfassungDB_ZLD(objVorerf.Vorgang);
                Session["objVorerf"] = objVorerf;
                if (objVorerf.Status != 0)
                {
                    tab1.Visible = true;
                    tab1.Height = "250px";
                    lblError.Text = objVorerf.Message;
                    cmdSend.Visible = false;
                }
                else { Fillgrid(0, ""); }
            }
        }

        private void Page_PreRender(object sender, System.EventArgs e)
        {
            Common.SetEndASPXAccess(this);
            HelpProcedures.FixedGridViewCols(gvZuldienst);

        }

        private void Page_Unload(object sender, System.EventArgs e)
        {
            Common.SetEndASPXAccess(this);
        }

        private void Fillgrid(Int32 intPageIndex, String strSort)
        {

            DataView tmpDataView = new DataView();
            tmpDataView = objVorerf.tblEingabeListe.DefaultView;
            tmpDataView.RowFilter = "";

            if (tmpDataView.Count == 0)
            {
                gvZuldienst.Visible = false;
                Result.Visible = false;
            }
            else
            {
                Result.Visible = true;
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

                String myId = gvZuldienst.DataKeys[0]["ID"].ToString();
                String Css = "ItemStyle";
                foreach (GridViewRow row in gvZuldienst.Rows)
                {

                    if (gvZuldienst.DataKeys[row.RowIndex]["ID"].ToString() == myId)
                    {
                        row.CssClass = Css;
                        myId = gvZuldienst.DataKeys[row.RowIndex]["ID"].ToString();
                    }
                    else
                    {
                        if (Css == "ItemStyle")
                        {
                            Css = "GridTableAlternate2";
                        }
                        else
                        {
                            Css = "ItemStyle";
                        }
                        row.CssClass = Css;
                        myId = gvZuldienst.DataKeys[row.RowIndex]["ID"].ToString();

                    }
                }
                lblAnzahl.Text = "Anzahl Vorgänge: " + objVorerf.IDCount;

            }
        }

        protected void gvZuldienst_Sorting(object sender, GridViewSortEventArgs e)
        {
            Fillgrid(0, e.SortExpression);
        }

        protected void gvZuldienst_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Bearbeiten")
            {
                Response.Redirect("Change01ZLD.aspx?AppID=" + Session["AppID"].ToString() + "&ID=" + e.CommandArgument.ToString() + "&B=true");
            }
            if (e.CommandName == "Loeschen")
            {
                objVorerf = (VoerfZLD)Session["objVorerf"];
                Int32 Index;
                Int32.TryParse(e.CommandArgument.ToString(), out Index);
                Label ID = (Label)gvZuldienst.Rows[Index].FindControl("lblID");
                Label lblLoeschKZ = (Label)gvZuldienst.Rows[Index].FindControl("lblLoeschKZ");
                Label lblIDPos = (Label)gvZuldienst.Rows[Index].FindControl("lblid_pos");
                String Loeschkz = "";
                Int32 IDSatz;
                Int32 IDPos;
                Int32.TryParse(ID.Text, out IDSatz);
                Int32.TryParse(lblIDPos.Text, out IDPos);
                if (lblLoeschKZ.Text == "L")
                {
                    objVorerf.UpdateDB_LoeschKennzeichen(IDSatz, Loeschkz, IDPos);
                    lblLoeschKZ.Text = Loeschkz;
                    // Wenn LöschKz für Unterposition entfernt -> auch bei Hauptposition rausnehmen
                    if (IDPos > 10)
                    {
                        objVorerf.UpdateDB_LoeschKennzeichen(IDSatz, Loeschkz, 10);
                        foreach (GridViewRow dRow in gvZuldienst.Rows)
                        {
                            Label zeileID = (Label)dRow.FindControl("lblID");
                            Label zeilelblIDPos = (Label)dRow.FindControl("lblid_pos");
                            if ((zeileID.Text == IDSatz.ToString()) && (zeilelblIDPos.Text == "10"))
                            {
                                Label zeilelblLoeschKZ = (Label)dRow.FindControl("lblLoeschKZ");
                                zeilelblLoeschKZ.Text = Loeschkz;
                                break;
                            }
                        }
                    }
                }
                else
                {
                    Loeschkz = "L";
                    objVorerf.UpdateDB_LoeschKennzeichen(IDSatz, Loeschkz, IDPos);
                    lblLoeschKZ.Text = Loeschkz;
                    
                }

                if (objVorerf.Status != 0)
                {
                    lblError.Text = objVorerf.Message;

                }
                DataRow[] RowsEdit;
                if (IDPos != 10)
                {
                    if (Loeschkz == "")
                    {
                        RowsEdit = objVorerf.tblEingabeListe.Select("ID=" + IDSatz + " AND (id_pos =" + IDPos + " OR id_pos = 10)");
                    }
                    else
                    {
                        RowsEdit = objVorerf.tblEingabeListe.Select("ID=" + IDSatz + " AND id_pos =" + IDPos);
                    }
                }
                else
                {
                    foreach (GridViewRow row in gvZuldienst.Rows)
                    {

                        if (gvZuldienst.DataKeys[row.RowIndex]["ID"].ToString() == IDSatz.ToString())
                        {
                            lblLoeschKZ = (Label)row.FindControl("lblLoeschKZ");
                            lblLoeschKZ.Text = Loeschkz;
                            ImageButton ibtnedt = (ImageButton)row.FindControl("ibtnEdit");
                            ibtnedt.Visible = true;
                            if (Loeschkz == "L")
                            {
                                ibtnedt.Visible = false;
                            }
                        }

                    }
                    RowsEdit = objVorerf.tblEingabeListe.Select("ID=" + IDSatz);
                }

                foreach (DataRow Row in RowsEdit)
                {
                    Row["PosLoesch"] = Loeschkz;
                    Row["bearbeitet"] = true;
                }
                Session["objVorerf"] = objVorerf;
            }
        }

        protected void lb_zurueck_Click(object sender, EventArgs e)
        {
            Response.Redirect("Change01ZLD.aspx?AppID=" + Session["AppID"].ToString());
        }

        protected void cmdSend_Click(object sender, EventArgs e)
        {
            Response.Redirect("Change01ZLD.aspx?AppID=" + Session["AppID"].ToString());
        }

        protected void cmdCreate_Click(object sender, EventArgs e)
        {

        }

        protected void cmdCreate_Click1(object sender, EventArgs e)
        {
            objVorerf = (VoerfZLD)Session["objVorerf"];

            objVorerf.SaveZLDVorerfassung(Session["AppID"].ToString(), Session.SessionID, this,objCommon.tblKundenStamm,objCommon.tblMaterialStamm,objCommon.tblStvaStamm);
            if (objVorerf.Status != 0)
            {

                tab1.Visible = true;
                if (objVorerf.Status == -5555)
                {
                    lblError.Text = "Kommunikationfehler: Daten konnten nicht in SAP gespeichert werden!" + objVorerf.Message;
                    return;
                }


                lblError.Text = objVorerf.Message;

                DataRow[] rowListe = objVorerf.tblEingabeListe.Select("Status = '' OR toDelete ='X'");

                if (rowListe.Length > 0)
                {
                    foreach (DataRow dRow in rowListe) 
                    {
                        Int32 id;
                        Int32.TryParse(dRow["ID"].ToString(), out id);
                        objVorerf.DeleteRecordSet(id);
                        objVorerf.tblEingabeListe.Rows.Remove(dRow);
                    }

                }
                Fillgrid(0, "");
                gvZuldienst.Columns[1].Visible = true;
                gvZuldienst.Columns[2].Visible = false;
                gvZuldienst.Columns[3].Visible = false;

                if (gvZuldienst.Columns[10] != null) { gvZuldienst.Columns[10].Visible = false; }
                if (gvZuldienst.Columns[11] != null) { gvZuldienst.Columns[11].Visible = false; }
                if (gvZuldienst.Columns[12] != null) { gvZuldienst.Columns[12].Visible = false; }
                
            }
            else
            {
                tab1.Visible = true;
                tab1.Height = "250px";
                lblMessage.Visible = true;
                lblMessage.ForeColor = System.Drawing.ColorTranslator.FromHtml("#269700");
                lblMessage.Text = "Datensätze in SAP gespeichert. Keine Fehler aufgetreten.";
               // Tracing();
                DataRow[] rowListe = objVorerf.tblEingabeListe.Select("Status = ''");

                if (rowListe.Length > 0)
                {
                    foreach (DataRow dRow in rowListe)
                    {
                        Int32 id;
                        Int32.TryParse(dRow["ID"].ToString(), out id);
                        objVorerf.DeleteRecordSet(id);
                        objVorerf.tblEingabeListe.Rows.Remove(dRow);
                    }

                }
                Result.Visible = false;
                
                cmdSend.Enabled = false;
                
            }
        }

        //private void Tracing() 
        //{
            
        //    int AppID =0;
        //    int.TryParse( Session["AppID"].ToString(), out AppID);
        //    CKG.Base.Kernel.Logging.Trace logApp = new CKG.Base.Kernel.Logging.Trace(m_App.Connectionstring, m_App.SaveLogAccessSAP, m_App.LogLevel);
        //    logApp.WriteEntry("APP", m_User.UserName, Session.SessionID, AppID, lblHead.Text, m_User.Reference,
        //        "Absendung Vorerfassungsliste am " + System.DateTime.Now.ToString() + " erforgreich.", m_User.CustomerName, m_User.Customer.CustomerId, m_User.IsTestUser, 0, objVorerf.tblEingabeListe);
        
        //}


    }
}