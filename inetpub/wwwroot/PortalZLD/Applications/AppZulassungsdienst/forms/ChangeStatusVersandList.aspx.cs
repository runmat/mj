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
    public partial class ChangeStatusVersandList : Page
    {
        private User m_User;
        private VorVersand objVersandZul;

        #region Events

        /// <summary>
        /// Page_Load-Ereignis. Prüfen ob die Anwendung dem Benutzer zugeordnet ist.
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            m_User = Common.GetUser(this);
            Common.FormAuth(this, m_User);
            Common.GetAppIDFromQueryString(this);
            lblHead.Text = (string)m_User.Applications.Select("AppID = '" + Session["AppID"] + "'")[0]["AppFriendlyName"];

            if (Session["objVersandZul"] != null)
            {
                objVersandZul = (VorVersand)Session["objVersandZul"];

                if (!IsPostBack)
                {
                    Fillgrid(0, "");
                }
            }
            else
            {
                lblError.Text = "Keine Daten übergeben!";
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
        private void Page_PreRender(object sender, EventArgs e)
        {
            Common.SetEndASPXAccess(this);
        }

        /// <summary>
        /// Page_Unload-Ereignis.
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        private void Page_Unload(object sender, EventArgs e)
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
            DataTable tblTemp = objVersandZul.ExcelListe;

            CKG.Base.Kernel.DocumentGeneration.ExcelDocumentFactory excelFactory = new CKG.Base.Kernel.DocumentGeneration.ExcelDocumentFactory();
            string filename = String.Format("{0:yyyyMMdd_HHmmss_}", DateTime.Now) + m_User.UserName;
            excelFactory.CreateDocumentAndSendAsResponse(filename, tblTemp, this.Page);
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
                lblError.Text = "";
                Int32 Index;
                Int32.TryParse(e.CommandArgument.ToString(), out Index);
                Label lblID = (Label)GridView1.Rows[Index].FindControl("lblsapID");
                Label lblIDPos = (Label)GridView1.Rows[Index].FindControl("lblid_pos");
                Label lblLoeschKZ = (Label)GridView1.Rows[Index].FindControl("lblLoeschKZ");

                String Loeschkz = (lblLoeschKZ.Visible ? "" : "L");

                DataRow[] RowsEdit;
                if (lblIDPos.Text != "10")
                {
                    RowsEdit = objVersandZul.Liste.Select("ZULBELN=" + lblID.Text + " AND ZULPOSNR ='" + lblIDPos.Text + "'");
                }
                else
                {
                    RowsEdit = objVersandZul.Liste.Select("ZULBELN=" + lblID.Text);
                }

                DataTable mTable = CreateTable();

                foreach (var Row in RowsEdit)
                {
                    DataRow mNewRow = mTable.NewRow();
                    mNewRow["ZULBELN"] = Row["ZULBELN"];
                    mNewRow["ZULPOSNR"] = Row["ZULPOSNR"];
                    mNewRow["LOEKZ"] = Loeschkz;
                    mNewRow["STATUS"] = Row["STATUS"];
                    mTable.Rows.Add(mNewRow);
                }

                objVersandZul.UpdateStatus(mTable);

                if (objVersandZul.ErrorOccured)
                {
                    lblError.Text = objVersandZul.Message;
                }
                else
                {
                    foreach (DataRow Row in RowsEdit)
                    {
                        Row["LOEKZ"] = Loeschkz;
                    }
                    Fillgrid(GridView1.PageIndex, "");
                }
                Session["objVersandZul"] = objVersandZul;
            }
        }

        /// <summary>
        /// Setzen des Status in SAP.
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        protected void cmdCreate_Click(object sender, EventArgs e)
        {
            try
            {
                lblMessage.Visible = false;
                DataTable mTable = CreateTable();

                foreach (GridViewRow gvRow in GridView1.Rows)
                {
                    lblError.Text = "";
                    Label lblID = (Label)gvRow.FindControl("lblsapID");
                    Label lblIDPos = (Label)gvRow.FindControl("lblid_pos");
                    Label lblLoeschKZ = (Label)gvRow.FindControl("lblLoeschKZ");
                    DropDownList ddStatus = (DropDownList)gvRow.FindControl("ddlStatus");

                    String Loeschkz = "";
                    if (lblLoeschKZ.Visible)
                    {
                        Loeschkz = "L";
                    }
                    if (ddStatus.Visible)
                    {
                        DataRow mNewRow = mTable.NewRow();
                        mNewRow["ZULBELN"] = lblID.Text;
                        mNewRow["ZULPOSNR"] = lblIDPos.Text;
                        mNewRow["LOEKZ"] = Loeschkz;
                        mNewRow["STATUS"] = ddStatus.SelectedValue;
                        mTable.Rows.Add(mNewRow);

                        if (ddStatus.SelectedValue == "S")
                        {
                            DataRow[] RowsEdit = objVersandZul.Liste.Select("ZULBELN='" + lblID.Text + "'");
                            objVersandZul.Liste.Rows.Remove(RowsEdit[0]);
                            RowsEdit = objVersandZul.ExcelListe.Select("ZULBELN='" + lblID.Text + "'");
                            objVersandZul.ExcelListe.Rows.Remove(RowsEdit[0]);
                        }
                        else
                        {
                            DataRow[] RowsEdit = objVersandZul.Liste.Select("ZULBELN='" + lblID.Text + "'");
                            RowsEdit[0]["Status"] = ddStatus.SelectedValue;
                            RowsEdit = objVersandZul.ExcelListe.Select("ZULBELN='" + lblID.Text + "'");
                            RowsEdit[0]["Status"] = ddStatus.SelectedValue;
                        }
                    }
                    Session["objVersandZul"] = objVersandZul;
                }

                objVersandZul.UpdateStatus(mTable);

                if (objVersandZul.ErrorOccured)
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

        #endregion

        #region Methods

        private DataTable CreateTable()
        {
            DataTable tbl = new DataTable();
            tbl.Columns.Add("ZULBELN", typeof(String));
            tbl.Columns.Add("ZULPOSNR", typeof(String));
            tbl.Columns.Add("STATUS", typeof(String));
            tbl.Columns.Add("LOEKZ", typeof(String));
            tbl.Columns.Add("VZERDAT", typeof(String));
            return tbl;
        }

        /// <summary>
        /// Binden der selektierten Daten an das Grid.
        /// </summary>
        /// <param name="intPageIndex">Seitenindex</param>
        /// <param name="strSort">Sortierung nach</param>
        private void Fillgrid(Int32 intPageIndex, String strSort)
        {
            DataView tmpDataView = new DataView(objVersandZul.Liste);
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

                if (!String.IsNullOrEmpty(strSort))
                {
                    intTempPageIndex = 0;
                    strTempSort = strSort.Trim(' ');
                    if ((Session["Sort"] == null) || ((String)Session["Sort"] == strTempSort))
                    {
                        if (Session["Direction"] == null)
                        {
                            strDirection = "desc";
                        }
                        else
                        {
                            strDirection = (String)Session["Direction"];
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

                    Session["Sort"] = strTempSort;
                    Session["Direction"] = strDirection;
                }

                if (!String.IsNullOrEmpty(strTempSort))
                {
                    tmpDataView.Sort = strTempSort + " " + strDirection;
                }

                GridView1.PageIndex = intTempPageIndex;
                GridView1.DataSource = tmpDataView;
                GridView1.DataBind();
                ColumnsVisibility();

                // Zeilen mit gleicher ID gleich färben
                if (GridView1.DataKeys.Count > 0 && GridView1.DataKeys[0] != null)
                {
                    String myId = GridView1.DataKeys[0]["ZULBELN"].ToString();
                    String Css = "ItemStyle";
                    foreach (GridViewRow row in GridView1.Rows)
                    {
                        HiddenField hfStatus = (HiddenField)row.FindControl("hfStatus");
                        HiddenField VersandStatus = (HiddenField)row.FindControl("STATUSVERSAND");
                        DropDownList ddlStatus = (DropDownList)row.FindControl("ddlStatus");
                        Label lblWBKunde = (Label)row.FindControl("lblWBKunde");

                        if (lblWBKunde.Text == "J")
                        {
                            ddlStatus.Items.Add(new ListItem("in Ordnung(i.O.)", "S"));
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
                        }

                        if (GridView1.DataKeys[row.RowIndex] != null)
                        {
                            if (GridView1.DataKeys[row.RowIndex]["ZULBELN"].ToString() == myId)
                            {
                                row.CssClass = Css;
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

                                myId = GridView1.DataKeys[row.RowIndex]["ZULBELN"].ToString();
                            }
                        }
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
                // Status
                GridView1.Columns[1].Visible = false;
            }
            else if (objVersandZul.SelStatus == "3")
            {
                // Lösch-Button
                GridView1.Columns[0].Visible = false;
                // Status
                GridView1.Columns[1].Visible = false;
                cmdCreate.Visible = false;
            }
        }

        #endregion
    }
}
