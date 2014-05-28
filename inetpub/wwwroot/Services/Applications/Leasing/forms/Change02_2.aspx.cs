using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CKG.Base.Kernel.Common;
using CKG.Base.Kernel.Security;
using System.Configuration;
using Leasing.lib;

namespace Leasing.forms
{
    public partial class Change02_2 : System.Web.UI.Page
    {
        private CKG.Base.Kernel.Security.User m_User;
        private CKG.Base.Kernel.Security.App m_App;
        protected global::CKG.Services.GridNavigation GridNavigation1;
        private Lieferprognosen objLieferprog;
        private String strError;

        protected void Page_Load(object sender, EventArgs e)
        {
            m_User = Common.GetUser(this);

            Common.FormAuth(this, m_User);


            m_App = new App(m_User); //erzeugt ein App_objekt 

            Common.GetAppIDFromQueryString(this);

            lblHead.Text = (string)m_User.Applications.Select("AppID = '" + Session["AppID"] + "'")[0]["AppFriendlyName"];
            GridNavigation1.setGridElment(ref GridView1);

            GridNavigation1.PagerChanged += GridView1_PageIndexChanged;

            GridNavigation1.PageSizeChanged += GridView1_ddlPageSizeChanged;

            if (Session["objLieferprog"] != null)
            {
                if (IsPostBack == false)
                {
                    Fillgrid(0, "");
                }
                else
                { 
                    //CheckDataGrid1()
                    //CheckDataGrid2()
                }

            }
            else 
            {
                Response.Redirect("Change02.aspx?AppID=" + Session["AppID"].ToString());
            
            }
        }

        private void Fillgrid(Int32 intPageIndex, String strSort)
        {


            if (Session["objLieferprog"] != null)
            {
                objLieferprog = (Lieferprognosen)Session["objLieferprog"];
            }
            switch (objLieferprog.Aktion)
            {
                case "ALLE":
                     lblHead.Text += " (alle)";
                    break;
                case "UNB":
                    lblHead.Text += " (unbearbeitet)";
                    break;
                case "BEARB":
                    lblHead.Text += " (bearbeitet)";
                    break;
                default:
                    break;
            }
            
            DataView tmpDataView = new DataView();
            tmpDataView = objLieferprog.Master.DefaultView;
            tmpDataView.RowFilter = "";

            try
            {
                if (tmpDataView.Count == 0)
                {
                    Result.Visible = false;
                    lblNoData.Visible = true;
                    lblNoData.Text = "Keine Dokumente zur Anzeige gefunden.";
                }
                else
                {
                    Result.Visible = true;
                    lblNoData.Visible = false;

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

                }
            }
            catch (Exception)
            {

                throw;
            }


            
        }


        private void Page_PreRender(object sender, System.EventArgs e)
        {
            Common.SetEndASPXAccess(this);
        }

        private void Page_Unload(object sender, System.EventArgs e)
        {
            Common.SetEndASPXAccess(this);
        }


        private void GridView1_PageIndexChanged(Int32 pageindex)
        {
            Fillgrid(pageindex, "");
        }

        private void GridView1_ddlPageSizeChanged()
        {
            Fillgrid(0, "");
        }

        protected void GridView1_RowCommand(object sender, GridViewCommandEventArgs e)
        {
                    if (e.CommandName == "OpenTyp" || e.CommandName == "OpenBem")
                    {
                        if (Session["objLieferprog"] != null)
                        {
                            objLieferprog = (Lieferprognosen)Session["objLieferprog"];
                        }

                        DataView dv2;
                        DataRow drow = objLieferprog.Detail.Select("ID='" + e.CommandArgument + "'")[0];
                        dv2 = objLieferprog.Detail.DefaultView;
                        dv2.RowFilter = "ID='" + e.CommandArgument + "'";
                        lblID.Text = drow["ID"].ToString();
                        lblAktion.Text = e.CommandName;
                        GridView2.DataSource = dv2;
                        GridView2.DataBind();
                        if (e.CommandName == "OpenTyp")
                        {
                            GridView2.Columns[GridView2.Columns.Count-1].Visible = true;
                            trBEM.Visible = false;
                            trBEMEdit.Visible = false;  
                        }
                        else
                        {
                            txtBEM.Text = String.Empty;
                            txtBEMLW.Text = String.Empty;
                            lblBem.Text = drow["BEMERKUNG"].ToString();
                            GridView2.Columns[GridView2.Columns.Count - 1].Visible =false;
                            trBEM.Visible = true;
                            trBEMEdit.Visible = true;                  
                        }
                        mpeDetails.Show();
                    }
        }

        protected void lbBack_Click(object sender, EventArgs e)
        {
            objLieferprog= null;
            Session["objLieferprog"] = null;
            Response.Redirect("Change02.aspx?AppID=" + Session["AppID"].ToString());
        }

        protected void GridView1_Sorting(object sender, GridViewSortEventArgs e)
        {
            Fillgrid(GridView1.PageIndex, e.SortExpression);
        }

        protected void btnOK_Click(object sender, EventArgs e)
        {
            if (Session["objLieferprog"] != null)
            {
                objLieferprog = (Lieferprognosen)Session["objLieferprog"];
            }

            trError.Visible = false;
            lblErrorDetail.Text = "";
            String[] strArrLWA;
            if (objLieferprog.Detail.Select("ID='" + lblID.Text + "'")[0] != null) 
            {
                switch (lblAktion.Text)
                {
                    case "OpenTyp":
                        CheckGrid();
                         break;
                    case "OpenBem":
                        if (txtBEM.Text == String.Empty)
                        {
                            lblErrorDetail.Text = "Bemerkungstext fehlt.";
                            txtBEM.BackColor = System.Drawing.ColorTranslator.FromHtml("#C42D2D");
                            trError.Visible = true;
                            mpeDetails.Show();
                        }
                        if (txtBEMLW.Text == String.Empty)
                        {
                            lblErrorDetail.Text = "Lieferwoche fehlt.";
                        }
                        else if (txtBEMLW.Text.Contains(".") && txtBEMLW.Text.Length == 5)
                        {
                            strArrLWA = txtBEMLW.Text.Split('.');
                            if (strArrLWA[0].Length == 2 && strArrLWA[1].Length == 2)
                            {
                                if (IsInteger(strArrLWA[0].ToString()) && IsInteger(strArrLWA[1].ToString()))
                                {
                                    DataRow[] rowsDetail;
                                    rowsDetail = objLieferprog.Detail.Select("ID = '" + lblID.Text + "'");

                                    foreach (DataRow tmpRow in rowsDetail)
                                    {
                                        tmpRow["BEMERKUNG"] = txtBEM.Text + " " + strArrLWA[0].ToString() + "/" + strArrLWA[1].ToString();
                                        tmpRow["EDITED"] = "X";
                                    }
                                    objLieferprog.Detail.AcceptChanges();
                                    Session["objLieferprog"] = objLieferprog;
                                    
                                    objLieferprog.SaveLieferprognose(Session["AppID"].ToString(), Session.SessionID.ToString(), this);
                                    if (objLieferprog.Status != 0)
                                    {
                                        lblErrorDetail.Text = objLieferprog.Message;
                                        trError.Visible = true;
                                        mpeDetails.Show();
                                    
                                    }
                                    else{Fillgrid(0, "");}
                                    
                                }
                                else
                                {
                                    txtBEMLW.BackColor = System.Drawing.ColorTranslator.FromHtml("#C42D2D");
                                    lblErrorDetail.Text = "Bitte geben Sie nur numerische Werte für Lieferwoche und Jahr ein.";
                                    trError.Visible = true;
                                    mpeDetails.Show();
                                }
                            }
                            else
                            {
                                txtBEMLW.BackColor = System.Drawing.ColorTranslator.FromHtml("#C42D2D");
                                trError.Visible = true;
                                lblErrorDetail.Text = "Falsches Format. Bitte verwenden Sie folgendes Format: WW.JJ";
                                mpeDetails.Show();
                            }
                        }
                        
                        break;
                    default:
                        break;
                }
            }
        }
        public static bool IsInteger(string theValue)
        {
            try
            {
                Convert.ToInt32(theValue);
                return true;
            }
            catch
            {
                return false;
            }
        }
        private Boolean CheckGrid()
        {
            Boolean bReturn = false;
            Boolean booDiff = false;
            String strCompare = "";
            TextBox txtBox = default(TextBox);
            Boolean booError = false;
            String strLW = null;
            Int32 e;
            e = 0;
            foreach (GridViewRow Row in GridView2.Rows)
            {

                txtBox = (TextBox)Row.Cells[4].FindControl("txtLWGrid");

                Label lblUID = (Label)Row.Cells[0].FindControl("lblUID");
                DataRow FahrzeugeRow = objLieferprog.Detail.Select("UID='" + lblUID.Text + "'")[0];


                if (txtBox != null)
                {

                    if (txtBox.Text != String.Empty)
                    {
                            strLW = txtBox.Text;
                            String[] strArrLWA;

                            if (strLW.Contains(".") && strLW.Length == 5)
                            {
                                strArrLWA  = strLW.Split('.');
                                if (strArrLWA[0].Length == 2 && strArrLWA[1].Length == 2)
                                {
                                    if (IsInteger(strArrLWA[0].ToString()) && IsInteger(strArrLWA[1].ToString()))
                                    {
                                        DataRow[] rowsDetail;
                                        rowsDetail = objLieferprog.Detail.Select("UID = '" + lblUID.Text + "'");
                                        strCompare = rowsDetail[0]["LIEFERWOCHE"].ToString();
                                        rowsDetail[0]["LIEFERWOCHE"] = strLW;
                                        rowsDetail[0]["EDITED"] = "X";
                                        rowsDetail[0].AcceptChanges();
                                        if (GridView2.Rows.Count > 0)
                                        {
                                            if (e>0)
                                            {
                                                if (booDiff==false)
                                                    if (strLW != strCompare)
	                                                    {
                                                            booDiff = true;

	                                                    }                                                {
                                                }
                                            }
                                            else { strCompare = strLW; }

                                            e += 1;
                                        }


                                    }
                                    else
                                    {
                                        strError = "Bitte geben Sie nur numerische Werte für Lieferwoche und Jahr ein.";
                                        booError = true;
                                    }
                                }
                                else
                                {
                                    strError = "Falsches Format. Bitte verwenden Sie folgendes Format: WW.JJ";
                                    booError = true;                                    
                                }
                            }
                            else 
                            {                              
                                strError = "Falsches Format. Bitte verwenden Sie folgendes Format: WW.JJ";
                                booError = true; 
                            }
                    }


                }



                if (booError == true)
                {
                    txtBox.BackColor = System.Drawing.ColorTranslator.FromHtml("#C42D2D");
                    lblErrorDetail.Text = strError;
                    trError.Visible = true;
                    mpeDetails.Show();
                }
                else 
                {
                    DataRow [] RowsMaster; 
                    RowsMaster = objLieferprog.Master.Select("ID = '" + lblID.Text + "'");
                    if (booDiff == true)
	                    {
                            RowsMaster[0]["LIEFERWOCHE"] = "Diff.";
                            
	                    }else if (strLW != null)
	                    {
                            RowsMaster[0]["LIEFERWOCHE"] = strLW;
                            
	                    }
                        RowsMaster[0].AcceptChanges();
                        Session["objLieferprog"] = objLieferprog;                 
                    }
            }
            if (booError==false)
            {
                objLieferprog.SaveLieferprognose(Session["AppID"].ToString(), Session.SessionID.ToString(), this);
                if (objLieferprog.Status != 0)
                {
                    lblErrorDetail.Text = objLieferprog.Message;
                    trError.Visible = true;
                    mpeDetails.Show();

                }
                else { Session["objLieferprog"] = objLieferprog; Fillgrid(0, ""); }                
            }


            bReturn = booError;

            return bReturn;

        }
        private Boolean CheckGridview1()
        {
            Boolean bReturn = false;
            TextBox txtBox = default(TextBox);
            Boolean booError = false;
            String strLW = null;
            foreach (GridViewRow Row in GridView1.Rows)
            {
                Boolean tmpError = false;
                txtBox = (TextBox)Row.Cells[3].FindControl("txtLW");

                Label lblID = (Label)Row.Cells[0].FindControl("lblID");

                if (txtBox != null)
                {

                    if (txtBox.Text != String.Empty && txtBox.Enabled == true)
                    {
                        strLW = txtBox.Text;
                        String[] strArrLWA;

                        if (strLW.Contains(".") && strLW.Length == 5)
                        {
                            strArrLWA = strLW.Split('.');
                            if (strArrLWA[0].Length == 2 && strArrLWA[1].Length == 2)
                            {
                                if (IsInteger(strArrLWA[0].ToString()) && IsInteger(strArrLWA[1].ToString()))
                                {
                                    DataRow[] rowsDetail;
                                    rowsDetail = objLieferprog.Detail.Select("ID = '" + lblID.Text + "'");
                                    foreach (DataRow tmpRow in rowsDetail)
                                    {
                                        tmpRow["LIEFERWOCHE"] = strLW;
                                        tmpRow["EDITED"] = "X";
                                    }

                                    rowsDetail[0].AcceptChanges();

                                    DataRow[] RowsMaster;
                                    RowsMaster = objLieferprog.Master.Select("ID = '" + lblID.Text + "'");

                                    RowsMaster[0]["LIEFERWOCHE"] = strLW;

                                    RowsMaster[0].AcceptChanges();

                                }
                                else
                                {
                                    strError = "Bitte geben Sie nur numerische Werte für Lieferwoche und Jahr ein.";
                                    tmpError = true;
                                }
                            }
                            else
                            {
                                strError = "Falsches Format. Bitte verwenden Sie folgendes Format: WW.JJ";
                                tmpError = true;
                            }
                        }
                        else
                        {
                            strError = "Falsches Format. Bitte verwenden Sie folgendes Format: WW.JJ";
                            tmpError = true;
                        }
                    }


                }



                if (tmpError == true)
                {
                    txtBox.BackColor = System.Drawing.ColorTranslator.FromHtml("#C42D2D");
                    lblError.Text = strError;
                    trError.Visible = true;
                    booError = tmpError;
                }

            }

            if (booError == false)
            {
                objLieferprog.SaveLieferprognose(Session["AppID"].ToString(), Session.SessionID.ToString(), this);
                if (objLieferprog.Status != 0)
                {
                    lblError.Text = objLieferprog.Message;

                }
                else { Session["objLieferprog"] = objLieferprog; Fillgrid(0, ""); }
            }


            bReturn = booError;

            return bReturn;

        }

        protected void cmdSend_Click(object sender, EventArgs e)
        {
            if (Session["objLieferprog"] != null)
            {
                objLieferprog = (Lieferprognosen)Session["objLieferprog"];
                CheckGridview1();
            }

           
        }

    }
}
