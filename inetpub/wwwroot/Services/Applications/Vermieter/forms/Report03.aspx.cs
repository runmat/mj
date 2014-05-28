using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CKG.Base.Kernel.Common;
using CKG.Base.Kernel.Security;
using System.Configuration;
using Vermieter.lib;
using System.Data;
using CKG.Base.Kernel;

namespace Vermieter.forms
{
    public partial class Report03 : System.Web.UI.Page
    {

        private CKG.Base.Kernel.Security.User m_User;
        private CKG.Base.Kernel.Security.App m_App;
        protected global::CKG.Services.GridNavigation GridNavigation1;
        protected global::CKG.Services.GridNavigation GridNavigation2;


        #region Events
            protected void Page_Load(object sender, EventArgs e)
            {
                m_User = Common.GetUser(this);

                Common.FormAuth(this, m_User);

                m_App = new App(m_User);

                Common.GetAppIDFromQueryString(this);

                lblHead.Text = (string)m_User.Applications.Select("AppID = '" + Session["AppID"] + "'")[0]["AppFriendlyName"];

                txtDatumVon.Attributes.Add("readonly", "readonly");
                txtDatumBis.Attributes.Add("readonly", "readonly");
                txtBereitDatumVon.Attributes.Add("readonly", "readonly");
                txtBereitDatumBis.Attributes.Add("readonly", "readonly");


                GridNavigation1.setGridElment(ref gvCarport);
                
                GridNavigation1.PagerChanged += gvCarport_PageIndexChanged;

                GridNavigation1.PageSizeChanged += gvCarport_ddlPageSizeChanged;

                GridNavigation2.setGridElment(ref GridView1);

                GridNavigation2.PagerChanged += GridView1_PageIndexChanged;

                GridNavigation2.PageSizeChanged += GridView1_ddlPageSizeChanged;



                if (IsPostBack == false)
                {
                    InitializeForm();
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

        #endregion


        #region Selection

                protected void ibtnDelEingVon_Click(object sender, ImageClickEventArgs e)
                {
                    txtDatumVon.Text = "";
                    SetFilter();
                }

                protected void ibtnDelEingBis_Click(object sender, ImageClickEventArgs e)
                {
                    txtDatumBis.Text = "";
                    SetFilter();
                }

                protected void ibtnDelBereitVon_Click(object sender, ImageClickEventArgs e)
                {
                    txtBereitDatumVon.Text = "";
                    SetFilter();
                }

                protected void ibtnDelBereitBis_Click(object sender, ImageClickEventArgs e)
                {
                    txtBereitDatumBis.Text = "";
                    SetFilter();
                }


            #endregion


        #region gvCarport

            private void gvCarport_PageIndexChanged(Int32 pageindex)
            {
                FillGridKum(pageindex, "");
            }

            private void gvCarport_ddlPageSizeChanged()
            {
                FillGridKum(0, "");

            }

            protected void gvCarport_Sorting(object sender, GridViewSortEventArgs e)
            {
                FillGridKum(gvCarport.PageIndex, e.SortExpression);

            }


            private void GridView1_PageIndexChanged(Int32 pageindex)
            {
                FillGrid(pageindex, "");
                
            }

            private void GridView1_ddlPageSizeChanged()
            {
                FillGrid(0, "");

            }

            protected void GridView1_Sorting(object sender, GridViewSortEventArgs e)
            {
                FillGrid(GridView1.PageIndex, e.SortExpression);

            }


            protected void ddlCarports_SelectedIndexChanged(object sender, EventArgs e)
            {
                SetFilter();
            }



        #endregion

 


            private void InitializeForm()
            {
                Carportbestand cb = new Carportbestand(ref m_User, m_App, (string)Session["AppID"], (string)Session.SessionID, "");

                cb.FILL(Session["AppID"].ToString(), Session.SessionID.ToString(), this.Page);

                Session["Carportbestand"] = cb;


                ListItem litCp;
                litCp = new ListItem();
                litCp.Text = "- alle -";
                litCp.Value = "00";

                ddlCarports.Items.Add(litCp);

                foreach (DataRow drow in cb.Carports.Rows)
                {
                    litCp = new ListItem();
                    litCp.Text = (string)drow["CarportName"];
                    litCp.Value = (string)drow["Carport"];
                    ddlCarports.Items.Add(litCp);
                }

                FillGridKum(0, "");
                FillGrid(0, "");


            }

            protected void lbBack_Click(object sender, EventArgs e)
            {
                Response.Redirect("/Services/(S(" + Session.SessionID + "))/Start/Selection.aspx");
            }

            protected void lbCreate_Click(object sender, EventArgs e)
            {

            }

            protected void NewSearch_Click1(object sender, ImageClickEventArgs e)
            {
                if (pnlSelection.Visible == true)
                {
                    pnlSelection.Visible = false;
                    lblSelection.Text = "Selektion öffnen...";
                }
                else
                {
                    pnlSelection.Visible = true;
                    lblSelection.Text = "Ausblenden...";
                }
            }


            private void FillGridKum(Int32 intPageIndex, string strSort)
            {
                DataView tmpDataView = new DataView();



                Carportbestand cb =  (Carportbestand)Session["Carportbestand"];

                tmpDataView = cb.CarportsFiltered.DefaultView;


                tmpDataView.RowFilter = "";

                if (tmpDataView.Count == 0)
                {
                    Result.Visible = false;
                    
                }
                else
                {
                    Result.Visible = true;
                    //lbCreate.Visible = false;
                    //tab1.Visible = False
                    //Queryfooter.Visible = false;
                    string strTempSort = "";
                    string strDirection = "";
                    Int32 intTempPageIndex = intPageIndex;

                    if (strSort.Trim(' ').Length > 0)
                    {
                        strTempSort = strSort.Trim(' ');
                        if ((ViewState["SortKum"] == null) || (ViewState["SortKum"].ToString() == strTempSort))
                        {
                            if (ViewState["DirectionKum"] == null)
                            {
                                strDirection = "desc";
                            }
                            else
                            {
                                strDirection = ViewState["DirectionKum"].ToString();
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

                        ViewState["SortKum"] = strTempSort;
                        ViewState["DirectionKum"] = strDirection;
                    }
                    else
                    {
                        if ((ViewState["SortKum"] != null))
                        {
                            strTempSort = ViewState["SortKum"].ToString();
                            if (ViewState["DirectionKum"] == null)
                            {
                                strDirection = "asc";
                                ViewState["DirectionKum"] = strDirection;
                            }
                            else
                            {
                                strDirection = ViewState["DirectionKum"].ToString();
                            }
                        }
                    }

                    if (!(strTempSort.Length == 0))
                    {
                        tmpDataView.Sort = strTempSort + " " + strDirection;
                    }
                    gvCarport.PageIndex = intTempPageIndex;
                    gvCarport.DataSource = tmpDataView;
                    gvCarport.DataBind();

                }

            }



            private void FillGrid(Int32 intPageIndex, string strSort)
            {
                DataView tmpDataView = new DataView();



                Carportbestand cb = (Carportbestand)Session["Carportbestand"];

                tmpDataView = cb.FahrzeugeFiltered.DefaultView;

                

                tmpDataView.RowFilter = "";

                if (tmpDataView.Count == 0)
                {
                    ResultDetail.Visible = false;
                }
                else
                {
                    //ResultDetail.Visible = true;
                    //lbCreate.Visible = false;
                    //tab1.Visible = False
                    //Queryfooter.Visible = false;
                    string strTempSort = "";
                    string strDirection = "";
                    Int32 intTempPageIndex = intPageIndex;

                    if (strSort.Trim(' ').Length > 0)
                    {
                        strTempSort = strSort.Trim(' ');
                        if ((ViewState["Sort"] == null) || (ViewState["Sort"].ToString() == strTempSort))
                        {
                            if (ViewState["Direction"] == null)
                            {
                                strDirection = "desc";
                            }
                            else
                            {
                                strDirection = ViewState["Direction"].ToString();
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

                        ViewState["Sort"] = strTempSort;
                        ViewState["Direction"] = strDirection;
                    }
                    else
                    {
                        if ((ViewState["Sort"] != null))
                        {
                            strTempSort = ViewState["Sort"].ToString();
                            if (ViewState["Direction"] == null)
                            {
                                strDirection = "asc";
                                ViewState["Direction"] = strDirection;
                            }
                            else
                            {
                                strDirection = ViewState["Direction"].ToString();
                            }
                        }
                    }

                    if (!(strTempSort.Length == 0))
                    {
                        tmpDataView.Sort = strTempSort + " " + strDirection;
                    }
                    GridView1.PageIndex = intTempPageIndex;
                    GridView1.DataSource = tmpDataView;
                    GridView1.DataBind();

                }

            }


            protected void ibtUebersicht_Click(object sender, ImageClickEventArgs e)
            {

                Carportbestand cb = (Carportbestand)Session["Carportbestand"];

                DataTable TempTable = new DataTable();

                TempTable = cb.CarportsFiltered;


                if (TempTable.Rows.Count == 0)
                {
                    return;
                }


                if (Result.Visible == true)
                {
                    Result.Visible = false;
                    lblUebersicht.Text = "Übersicht öffnen...";
                }
                else
                {
                    Result.Visible = true;
                    lblUebersicht.Text = "Übersicht ausblenden...";
                }
            }

            protected void ibtDetail_Click(object sender, ImageClickEventArgs e)
            {

                Carportbestand cb = (Carportbestand)Session["Carportbestand"];

                DataTable TempTable = new DataTable();

                TempTable = cb.CarportsFiltered;


                if (TempTable.Rows.Count == 0)
                {
                    return;
                }


                if (ResultDetail.Visible == true)
                {
                    ResultDetail.Visible = false;
                    lblDetail.Text = "Details öffnen...";
                }
                else
                {
                    ResultDetail.Visible = true;
                    lblDetail.Text = "Details ausblenden...";
                }

            }

            protected void lnkCreateExcel1_Click(object sender, EventArgs e)
            {

                Carportbestand cb = (Carportbestand)Session["Carportbestand"];


                Control control = new Control();
                DataTable tblTranslations = new DataTable();
                DataTable tblTemp = cb.FahrzeugeFiltered;




                string AppURL = null;
                DataColumn col2 = null;
                int bVisibility = 0;
                int i = 0;
                string sColName = "";
                AppURL = this.Request.Url.LocalPath.Replace("/Services", "..");
                tblTranslations = (DataTable)this.Session[AppURL];
                foreach (DataControlField col in GridView1.Columns)
                {
                    for (i = tblTemp.Columns.Count - 1; i >= 0; i += -1)
                    {
                        bVisibility = 0;
                        col2 = tblTemp.Columns[i];
                        if (col2.ColumnName.ToUpper() == col.SortExpression.ToUpper())
                        {
                            sColName = Common.TranslateColLbtn(GridView1, tblTranslations, col.HeaderText, ref bVisibility);
                            if (bVisibility == 0)
                            {
                                tblTemp.Columns.Remove(col2);
                            }
                            else if (sColName.Length > 0)
                            {
                                col2.ColumnName = sColName;
                            }
                        }
                    }
                    tblTemp.AcceptChanges();
                }
                CKG.Base.Kernel.DocumentGeneration.ExcelDocumentFactory excelFactory = new CKG.Base.Kernel.DocumentGeneration.ExcelDocumentFactory();
                string filename = String.Format("{0:yyyyMMdd_HHmmss_}", System.DateTime.Now) + m_User.UserName;
                excelFactory.CreateDocumentAndSendAsResponse(filename, tblTemp, this.Page, false, null, 0, 0);


                

            }


            private void SetFilter()
            {

                Carportbestand cb = (Carportbestand)Session["Carportbestand"];

                DataTable TempTable = new DataTable();

                TempTable = cb.Result;

                TempTable.DefaultView.Sort = "ZZCARPORT";

                TempTable = TempTable.DefaultView.ToTable();


                //Filter auf die Tabelle setzen

                string FilterEx = "";

                if (ddlCarports.SelectedValue != "00")
                {
                    FilterEx = "ZZCARPORT = '" + ddlCarports.SelectedValue + "'";
                }

                if(txtFahrzeugnummer.Text.Length > 0)
                {
                    if (FilterEx.Length == 0)
                    {
                        FilterEx = "Fahrzeugnr = '" + txtFahrzeugnummer.Text + "'";
                    }

                    else
                    {
                        FilterEx += " AND Fahrzeugnr = '" + txtFahrzeugnummer.Text + "'";
                    }

                }

                if (txtDatumVon.Text.Length > 0)
                {
                    if (FilterEx.Length == 0)
                    {
                        FilterEx = "EingangsdatumDetail >= '" + txtDatumVon.Text + "'";
                    }

                    else
                    {
                        FilterEx += " AND EingangsdatumDetail >= '" + txtDatumVon.Text + "'";
                    }

                }

                if (txtDatumBis.Text.Length > 0)
                {
                    if (FilterEx.Length == 0)
                    {
                        FilterEx = "EingangsdatumDetail <= '" + txtDatumBis.Text + "'";
                    }

                    else
                    {
                        FilterEx += " AND EingangsdatumDetail <= '" + txtDatumBis.Text + "'";
                    }

                }


                if (txtBereitDatumVon.Text.Length > 0)
                {
                    if (FilterEx.Length == 0)
                    {
                        FilterEx = "BereitdatumDetail >= '" + txtBereitDatumVon.Text + "'";
                    }

                    else
                    {
                        FilterEx += " AND BereitdatumDetail >= '" + txtBereitDatumVon.Text + "'";
                    }

                }

                if (txtBereitDatumBis.Text.Length > 0)
                {
                    if (FilterEx.Length == 0)
                    {
                        FilterEx = "BereitdatumDetail <= '" + txtBereitDatumBis.Text + "'";
                    }

                    else
                    {
                        FilterEx += " AND BereitdatumDetail <= '" + txtBereitDatumBis.Text + "'";
                    }

                }


                if (rblFahrzeug.SelectedValue == "1")
                {
                    if (FilterEx.Length == 0)
                    {
                        FilterEx = "Briefnummer <> ''";
                    }

                    else
                    {
                        FilterEx += " AND Briefnummer <> ''";
                    }
                }

                if (rblFahrzeug.SelectedValue == "2")
                {
                    if (FilterEx.Length == 0)
                    {
                        FilterEx = "Briefnummer = ''";
                    }

                    else
                    {
                        FilterEx += " AND Briefnummer = ''";
                    }
                }


                if (chkZulBereit.Checked == true)
                {
                    if (FilterEx.Length == 0)
                    {
                        FilterEx = "ZULBEREIT = 'X'";
                    }

                    else
                    {
                        FilterEx += " AND ZULBEREIT = 'X'";
                    }
                }

                if (chkGesperrt.Checked == true)
                {
                    if (FilterEx.Length == 0)
                    {
                        FilterEx = "Gesperrt = 'X'";
                    }

                    else
                    {
                        FilterEx += " AND Gesperrt = 'X'";
                    }
                }



                TempTable.DefaultView.RowFilter = FilterEx;


                TempTable = TempTable.DefaultView.ToTable();

                if (TempTable.Rows.Count == 0)
                {
                    lblError.Visible = true;
                    lblError.Text = "Es wurden keine Fahrzeuge gefunden.";
                    lblUebersicht.Text = "Übersicht öffnen...";
                    lblDetail.Text = "Details öffnen...";
                }

                cb.FahrzeugeFiltered = TempTable;

                cb.CarportsFiltered = cb.Carports.Clone();


                DataRow CarRow;

                string CpFound = "";


                foreach (DataRow dr in cb.FahrzeugeFiltered.Rows)
                {

                    if (CpFound != dr["ZZCARPORT"].ToString())
                    {


                        //Neue Row hinzufügen
                        CarRow = cb.CarportsFiltered.NewRow();

                        CpFound = cb.FahrzeugeFiltered.Select("ZZCARPORT = '" + dr["ZZCARPORT"].ToString() + "'")[0]["ZZCARPORT"].ToString();

                        CarRow["Carport"] = CpFound;
                        CarRow["Carportname"] = cb.FahrzeugeFiltered.Select("ZZCARPORT = '" + dr["ZZCARPORT"].ToString() + "'")[0]["CarportNameDetail"];
                        CarRow["AnzFahrzeuge"] = cb.FahrzeugeFiltered.Select("ZZCARPORT = '" + dr["ZZCARPORT"].ToString() + "'").Count().ToString();

                        cb.CarportsFiltered.Rows.Add(CarRow);

                        cb.CarportsFiltered.AcceptChanges();

                    }

                }


                Session["Carportbestand"] = cb;

                FillGridKum(0, "");
                FillGrid(0, "");


            }

            protected void btnCal1_Click(object sender, ImageClickEventArgs e)
            {
                SetFilter();
            }

            protected void rblFahrzeug_SelectedIndexChanged(object sender, EventArgs e)
            {
                SetFilter();
            }

            protected void chkZulBereit_CheckedChanged(object sender, EventArgs e)
            {
                SetFilter();
            }

            protected void chkGesperrt_CheckedChanged(object sender, EventArgs e)
            {
                SetFilter();
            }

            protected void ibtnSetFahrzeugnummer_Click(object sender, ImageClickEventArgs e)
            {
                SetFilter();
            }

            protected void txtDatum_TextChanged(object sender, EventArgs e)
            {
                SetFilter();
            }



 
    }
}
