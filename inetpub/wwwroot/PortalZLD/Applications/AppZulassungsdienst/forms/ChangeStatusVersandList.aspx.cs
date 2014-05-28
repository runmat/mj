using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using CKG.Base.Kernel.Common;
using CKG.Base.Kernel.Security;
using AppZulassungsdienst.lib;
using System.Data;

namespace AppZulassungsdienst.forms
{
    /// <summary>
    /// Listenansicht Übersicht offener Versandzulassungen und Rechnungsprüfung.
    /// </summary>
    public partial class ChangeStatusVersandList : System.Web.UI.Page
    {
        private CKG.Base.Kernel.Security.User m_User;
        private CKG.Base.Kernel.Security.App m_App;
        private VorVersand objVersandZul;
        /// <summary>
        /// Page_Load-Ereignis. Prüfen ob die Anwendung dem Benutzer zugeordnet ist.
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            m_User = Common.GetUser(this);
            Common.FormAuth(this, m_User);
            m_App = new App(m_User);
            Common.GetAppIDFromQueryString(this);
            lblHead.Text = (string)m_User.Applications.Select("AppID = '" + Session["AppID"] + "'")[0]["AppFriendlyName"];

            if (Session["objVersandZul"] != null)
            {
                objVersandZul = (VorVersand)Session["objVersandZul"];
                if (IsPostBack == false) 
                { 
                    Fillgrid(0,"");
                }
                
            }
            else 
            {
                lblError.Text = "Keine Daten übergeben!";
            }

        }

        /// <summary>
        /// Binden der selektierten Daten an das Grid.
        /// </summary>
        /// <param name="intPageIndex">Seitenindex</param>
        /// <param name="strSort">Sortierung nach</param>
        private void Fillgrid(Int32 intPageIndex, String strSort)
        {
            DataView tmpDataView = new DataView();
            tmpDataView = objVersandZul.Liste.DefaultView;
            String strFilter = "";
            tmpDataView.RowFilter = strFilter;

            if (tmpDataView.Count == 0)
            {
                GridView1.Visible = false;
                Result.Visible = false;
            }
            else
            {
                Result.Visible = true;
                GridView1.Visible = true;
                Int32 intTempPageIndex = intPageIndex;
                String strTempSort = "";
                String strDirection = null;

                if (strSort.Trim(' ').Length > 0)
                {
                    intTempPageIndex = 0;
                    strTempSort = strSort.Trim(' ');
                    if ((this.ViewState["Sort"] == null) || ((String)this.ViewState["Sort"] == strTempSort))
                    {
                        if (this.ViewState["Direction"] == null)
                        {
                            strDirection = "desc";
                        }
                        else
                        {
                            strDirection = (String)this.ViewState["Direction"];
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

                    this.ViewState["Sort"] = strTempSort;
                    this.ViewState["Direction"] = strDirection;
                }

                if (strTempSort.Length != 0)
                {
                    tmpDataView.Sort = strTempSort + " " + strDirection;
                }

                GridView1.PageIndex = intTempPageIndex;
                GridView1.DataSource = tmpDataView;
                GridView1.DataBind();
                ColumnsVisibility();
                String myId = "";
                String Css = "";
                foreach (GridViewRow item in GridView1.Rows)
                {
                    HiddenField hfStatus = (HiddenField)item.FindControl("hfStatus");
                    HiddenField VersandStatus = (HiddenField)item.FindControl("STATUSVERSAND");
                    DropDownList ddlStatus = (DropDownList)item.FindControl("ddlStatus");
                    Label lblWBKunde = (Label)item.FindControl("lblWBKunde");

                    if (lblWBKunde.Text == "J")
                    {
                        ListItem lstItem = new ListItem("in Ordnung(i.O.)", "S");
                        ddlStatus.Items.Add(lstItem);
                    }
                    if (hfStatus.Value == "VZ")
                    {
                        ddlStatus.Items.Clear();
                        ddlStatus.Visible = false;
                    }

                    switch (VersandStatus.Value)
                    {
                        case ("N"):
                            ddlStatus.SelectedValue = "N";
                            break;
                        case ("R"):
                            ddlStatus.SelectedValue = "R";
                            break;
                        default:
                           break;
                    }
                    
                     
                    if (myId.Length > 0)
                    {
                        if (GridView1.DataKeys[item.RowIndex]["ID"].ToString() == myId)
                        {
                            item.CssClass = Css;
                            myId = GridView1.DataKeys[item.RowIndex]["ID"].ToString();
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
                            item.CssClass = Css;
                            myId = GridView1.DataKeys[item.RowIndex]["ID"].ToString();

                        }
                    }
                    else 
                    {
                        Css = "ItemStyle";
                        item.CssClass = Css;
                        myId = GridView1.DataKeys[item.RowIndex]["ID"].ToString();
                    }

                }
            }
        }
        /// <summary>
        /// Je nach selektierten Status Spalten ein- und ausblenden.
        /// </summary>
        private void ColumnsVisibility() 
        {

            if (objVersandZul.SelStatus == "1") 
            {
                GridView1.Columns[1].Visible = false;
               // cmdCreate.Visible = false;
            }
            if (objVersandZul.SelStatus == "3")
            {
                GridView1.Columns[0].Visible = false;
                GridView1.Columns[1].Visible = false;
                cmdCreate.Visible = false;
            }
        
        }

        /// <summary>
        /// Zurück zur Selektionsseite.
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        protected void lb_zurueck_Click(object sender, EventArgs e)
        {
            Response.Redirect("ChangeStatusVersand.aspx?AppID=" + Session["AppID"].ToString());
        }
        /// <summary>
        /// Sortierung nach einer bestimmten Spalte.
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">GridViewSortEventArgs</param>
       protected void GridView1_Sorting(object sender, GridViewSortEventArgs e)
        {
            Fillgrid(0, e.SortExpression);
        }
        /// <summary>
        /// Spaltenüberstzung
        /// </summary>
       /// <param name="sender">object</param>
       /// <param name="e">EventArgs</param>
        private void Page_PreRender(object sender, System.EventArgs e)
        {
            Common.SetEndASPXAccess(this);

        }
        /// <summary>
        /// Page_Unload-Ereignis.
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        private void Page_Unload(object sender, System.EventArgs e)
        {
            Common.SetEndASPXAccess(this);
        }
        /// <summary>
        /// Genieren der Excel-Datei.
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        protected void lnkCreateExcel_Click(object sender, EventArgs e)
        {
            Control control = new Control();
            DataTable tblTranslations = new DataTable();
            DataTable tblTemp = objVersandZul.ExcelListe;

            CKG.Base.Kernel.DocumentGeneration.ExcelDocumentFactory excelFactory = new CKG.Base.Kernel.DocumentGeneration.ExcelDocumentFactory();
            string filename = String.Format("{0:yyyyMMdd_HHmmss_}", System.DateTime.Now) + m_User.UserName;
            excelFactory.CreateDocumentAndSendAsResponse(filename, tblTemp, this.Page, false, null, 0, 0);
        }
        /// <summary>
        /// Setzen des Löschenkennz. im Auftrag.
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">GridViewCommandEventArgs</param>
        protected void GridView1_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Del")
            {
                objVersandZul = (VorVersand)Session["objVersandZul"];
                lblError.Text = "";
                Int32 Index;
                Int32.TryParse(e.CommandArgument.ToString(), out Index);
                Label ID = (Label)GridView1.Rows[Index].FindControl("lblsapID");
                Label lblIDPos = (Label)GridView1.Rows[Index].FindControl("lblid_pos");
                Label lblLoeschKZ = (Label)GridView1.Rows[Index].FindControl("lblLoeschKZ");
                DropDownList ddStatus = (DropDownList)GridView1.Rows[Index].FindControl("ddlStatus");

                String Loeschkz = "";
                if (lblLoeschKZ.Visible)
                {
                    objVersandZul.UpdateStatus(Session["AppID"].ToString(), Session.SessionID, this, ID.Text, lblIDPos.Text
                                                        , ddStatus.SelectedValue, Loeschkz, null);
                }
                else
                {
                    Loeschkz = "X";
                    objVersandZul.UpdateStatus(Session["AppID"].ToString(), Session.SessionID, this, ID.Text, lblIDPos.Text
                                                        , ddStatus.SelectedValue,Loeschkz,null);
                }

                if (objVersandZul.Status != 0)
                {
                    lblError.Text = objVersandZul.Message;

                }

                else
                {
                    DataRow[] RowsEdit;
                    if (lblIDPos.Text != "10")
                    {
                        RowsEdit = objVersandZul.Liste.Select("ID=" + ID.Text + " AND ZULPOSNR ='" + lblIDPos.Text + "'");
                    }
                    else
                    {
                        RowsEdit = objVersandZul.Liste.Select("ID=" + ID.Text);
                    }

                    foreach (DataRow Row in RowsEdit)
                    {
                        Row["LoeschKZ"] = Loeschkz;
                    }
                    Fillgrid(GridView1.PageIndex, "");
                }
                Session["objVersandZul"] = objVersandZul;
            }
        }
        /// <summary>
        /// Setzen des Status in SAP(Z_ZLD_CHANGE_VZOZUERL).
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        protected void cmdCreate_Click(object sender, EventArgs e)
        {
            try                                  

            {
                lblMessage.Visible = false;
                DataTable mTable = new DataTable();
                mTable.Columns.Add("ZULBELN", typeof(String));
                mTable.Columns.Add("ZULPOSNR", typeof(String));
                mTable.Columns.Add("STATUS", typeof(String));
                mTable.Columns.Add("LOEKZ", typeof(String));
                mTable.Columns.Add("VZERDAT", typeof(String));
                foreach (GridViewRow gvRow in GridView1.Rows)
                {
                    objVersandZul = (VorVersand)Session["objVersandZul"];
                    lblError.Text = "";
                    Label ID = (Label)gvRow.FindControl("lblsapID");
                    Label lblIDPos = (Label)gvRow.FindControl("lblid_pos");
                    Label lblLoeschKZ = (Label)gvRow.FindControl("lblLoeschKZ");
                    DropDownList ddStatus = (DropDownList)gvRow.FindControl("ddlStatus");

                    String Loeschkz = "";
                    if (lblLoeschKZ.Visible)
                    {
                        Loeschkz = "X";
                    }
                    if (ddStatus.Visible) 
                    { 
                        DataRow mNewRow = mTable.NewRow();
                        mNewRow["ZULBELN"] = ID.Text;
                        mNewRow["ZULPOSNR"] = lblIDPos.Text;
                        mNewRow["LOEKZ"] = Loeschkz;
                        mNewRow["STATUS"] = ddStatus.SelectedValue;
                        mTable.Rows.Add(mNewRow);

                        if (ddStatus.SelectedValue == "S")
                        {
                            DataRow[] RowsEdit = objVersandZul.Liste.Select("ID='" + ID.Text + "'");
                            objVersandZul.Liste.Rows.Remove(RowsEdit[0]);
                            RowsEdit = objVersandZul.ExcelListe.Select("ID='" + ID.Text + "'");
                            objVersandZul.ExcelListe.Rows.Remove(RowsEdit[0]);
                        }
                        else
                        {
                            DataRow[] RowsEdit = objVersandZul.Liste.Select("ID='" + ID.Text + "'");
                            RowsEdit[0]["Status"] = ddStatus.SelectedValue;
                            RowsEdit = objVersandZul.ExcelListe.Select("ID='" + ID.Text + "'");
                            RowsEdit[0]["Status"] = ddStatus.SelectedValue;
                    
                        }

                    }
                    Session["objVersandZul"] = objVersandZul;
                }

                objVersandZul.UpdateStatus(Session["AppID"].ToString(), Session.SessionID, this, "", ""
                                                            , "", "", mTable);
                if (objVersandZul.Status != 0)
                {
                    lblError.Text = objVersandZul.Message;
                }
                else 
                {
                    Fillgrid(0, "");

                    lblMessage.Visible = true;
                    lblMessage.ForeColor = System.Drawing.ColorTranslator.FromHtml("#269700");
                    lblMessage.Text = "Änderungen erfolgreich gespeichert!";
                }

            }
            catch (Exception ex)
            {
                lblError.Text = "Fehler beim Speichern des Status!" + ex.Message;

            }
        }


    }
}
