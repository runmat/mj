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
    public partial class Change02 : System.Web.UI.Page
    {

        private CKG.Base.Kernel.Security.User m_User;
        private CKG.Base.Kernel.Security.App m_App;
        protected global::CKG.Services.GridNavigation GridNavigation1;
        private int Status = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            m_User = Common.GetUser(this);

            Common.FormAuth(this, m_User);

            m_App = new App(m_User);

            Common.GetAppIDFromQueryString(this);

            lblHead.Text = (string)m_User.Applications.Select("AppID = '" + Session["AppID"] + "'")[0]["AppFriendlyName"];


            GridNavigation1.setGridElment(ref gvAusgabe);

            GridNavigation1.PagerChanged += gvAusgabe_PageIndexChanged;

            GridNavigation1.PageSizeChanged += gvAusgabe_ddlPageSizeChanged;
        }


        private void Page_PreRender(object sender, System.EventArgs e)
        {
            Common.SetEndASPXAccess(this);
        }

        private void Page_Unload(object sender, System.EventArgs e)
        {
            Common.SetEndASPXAccess(this);
        }




        private void gvAusgabe_PageIndexChanged(Int32 pageindex)
        {
            FillGrid(pageindex, "");
        }

        private void gvAusgabe_ddlPageSizeChanged()
        {
            FillGrid(0, "");
        }

        protected void gvAusgabe_Sorting(object sender, GridViewSortEventArgs e)
        {
           
                FillGrid(gvAusgabe.PageIndex, e.SortExpression);
           

        }

        protected void btnConfirm_Click(object sender, EventArgs e)
        {
            Haltefrist objHaltefrist = new Haltefrist(ref m_User, m_App, Session["AppID"].ToString(), Session.SessionID.ToString(), "");
            DataTable tblExport = new DataTable();

            objHaltefrist.GetData(this.Page, Session["AppID"].ToString(), Session.SessionID.ToString());

            Session["Haltefrist"] = objHaltefrist.Bestand;

            Status = 1;

           

            FillGrid(0, "");
            btnConfirm.Visible = false;
            btnUebernahme.Visible = true;
        }

        private void DoSubmit()
        {

            CheckGrid();

            DataTable TempTable = (DataTable)Session["Haltefrist"];

            DataView tmpDataView = new DataView();
            tmpDataView = TempTable.DefaultView;

            tmpDataView.RowFilter = "ActionNOTHING = 0";
            Int32 intFahrzeugBriefe = tmpDataView.Count;
            tmpDataView.RowFilter = "";

            if (intFahrzeugBriefe == 0)
            {
                lblError.Text = "Bitte wählen Sie erst Fahrzeuge aus.";
                FillGrid(gvAusgabe.PageIndex,"");
            }
            else
            {
                btnUebernahme.Visible = false;
                btnSave.Visible = true;
                btnReset.Visible = true;

                FillGrid(0,"");
                //Session("objFahrzeuge") = objFahrzeuge;
            }


        }


        private void CheckGrid()
        {
	        RadioButton chbox = default(RadioButton);
            Label lbl = default(Label);

            DataTable TempTable = (DataTable)Session["Haltefrist"];

	        try {
		        foreach ( GridViewRow gvr in gvAusgabe.Rows) {

                    lbl = (Label)gvr.FindControl("lblEQUNR");

			        string strEquipmentnummer = "EQUNR='" + lbl.Text + "'";

			        foreach (TableCell tCell in gvr.Cells) {
				        foreach (Control ctrl in tCell.Controls) {
                            if (ctrl is RadioButton)
                            {
                                chbox = (RadioButton)ctrl;
                                TempTable.AcceptChanges();
						        DataRow[] tmpRows = null;

                                tmpRows = TempTable.Select(strEquipmentnummer);
						        if (tmpRows.Length > 0) {
							        tmpRows[0].BeginEdit();
							        switch (chbox.ID) {
								        case "chkActionDELE":
									        if (chbox.Checked) {
										        tmpRows[0]["ActionDELE"] = true;
										        tmpRows[0]["ActionNOTHING"] = false;
										        //intReturn += 1;
									        }
									        break;
								        default:
									        if (chbox.Checked) {
										        tmpRows[0]["ActionNOTHING"] = true;
										        tmpRows[0]["ActionDELE"] = false;
									        }
									        break;
							        }
							        tmpRows[0].EndEdit();
						        } else {
							        throw new Exception("Equipmentnummer nicht gefunden!");
						        }
                                TempTable.AcceptChanges();
					        }
				        }
				        //intZaehl += 1;
			        }
		        }
	        } catch (Exception ex) {
		        lblError.Text = ex.Message;
	        }
            Session["Haltefrist"] = TempTable;
	        //return intReturn;
        }






        private void FillGrid(Int32 intPageIndex, string strSort)
        {
            DataView tmpDataView = new DataView();

            tmpDataView = ((DataTable)Session["Haltefrist"]).DefaultView;



            if (btnSave.Visible) {
	            tmpDataView.RowFilter = "ActionNOTHING = 0";
	            //ShowScript.Visible = false;
            } else {
	            tmpDataView.RowFilter = "";
            }

            if (tmpDataView.Count == 0) {
	            gvAusgabe.Visible = false;
	            lblError.Text = "Keine Daten zur Anzeige gefunden.";
	            lblError.Visible = true;
	            //ShowScript.Visible = false;
            } else {
	            Int32 intTempPageIndex = intPageIndex;
	            string strTempSort = "";
	            string strDirection = "";

	            if (strSort.Trim(' ').Length > 0) {
		            intTempPageIndex = 0;
		            strTempSort = strSort.Trim(' ');
		            if ((ViewState["Sort"] == null) || (ViewState["Sort"].ToString() == strTempSort)) {
			            if (ViewState["Direction"] == null) {
				            strDirection = "desc";
			            } else {
				            strDirection = ViewState["Direction"].ToString();
			            }
		            } else {
			            strDirection = "desc";
		            }

		            if (strDirection == "asc") {
			            strDirection = "desc";
		            } else {
			            strDirection = "asc";
		            }

		            ViewState["Sort"] = strTempSort;
		            ViewState["Direction"] = strDirection;
	            } else {
		            if ((ViewState["Sort"] != null)) {
			            strTempSort = ViewState["Sort"].ToString();
			            if (ViewState["Direction"] == null) {
				            strDirection = "asc";
				            ViewState["Direction"] = strDirection;
			            } else {
				            strDirection = ViewState["Direction"].ToString();
			            }
		            }
	            }

	            if (!(strTempSort.Length == 0)) {
		            tmpDataView.Sort = strTempSort + " " + strDirection;
	            }

	            gvAusgabe.PageIndex = intTempPageIndex;
	            if (btnSave.Visible) {
		            gvAusgabe.AllowPaging = false;
		            gvAusgabe.AllowSorting = false;
	            } else {
		            gvAusgabe.AllowPaging = true;
		            gvAusgabe.AllowSorting = true;
	            }

                DivPlaceholder.Visible = false;
                Result.Visible = true;

           		gvAusgabe.PageIndex = intTempPageIndex;
	            gvAusgabe.DataSource = tmpDataView;
	            gvAusgabe.DataBind();


                if (Status == 1)
                {
                    lblNoData.Text = "Es wurden " + tmpDataView.Count.ToString() + " Vorgänge gefunden.";
                }

                if (btnSave.Visible == true)
                {
                    lblNoData.Text = "Sie haben die folgenden Vorgänge ausgewählt (Anzahl: " + tmpDataView.Count.ToString() + ").";
                }

                if (Status == 3) //Gespeichert
                {
                    lblNoData.Text = "Sie haben die folgenden Vorgänge beauftragt (Anzahl: " + tmpDataView.Count.ToString() + ").";

                    gvAusgabe.Columns[gvAusgabe.Columns.Count - 1].Visible = true;
                    gvAusgabe.Columns[gvAusgabe.Columns.Count - 2].Visible = true;
                    Int32 k = default(Int32);
                    for (k = 1; k <= 5; k++)
                    {
                        gvAusgabe.Columns[gvAusgabe.Columns.Count - 2 - k].Visible = false;
                    }


                }

          


	            RadioButton chkBox = default(RadioButton);


	            foreach ( GridViewRow gvr in gvAusgabe.Rows) {
		            string strEquipmentnummer = "Equipmentnummer = '" + gvr.Cells[0].Text + "'";


		            foreach (TableCell tCell in gvr.Cells) {
                        foreach (Control ctrl in tCell.Controls)
                        {
                            if (ctrl is RadioButton)
                            {
                                chkBox = (RadioButton)ctrl;

					            if (btnSave.Visible) {
						            chkBox.Enabled = false;
					            }
				            }
			            }
		            }
	            }
            }


        }

        protected void lbBack_Click(object sender, EventArgs e)
        {
            Response.Redirect("/Services/(S(" + Session.SessionID + "))/Start/Selection.aspx");
        }

        protected void btnReset_Click(object sender, EventArgs e)
        {
        btnSave.Visible = false;
        btnReset.Visible = false;
        btnUebernahme.Visible = true;


        FillGrid(gvAusgabe.PageIndex,"");
        }

        protected void btnUebernahme_Click(object sender, EventArgs e)
        {
            DoSubmit();
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            Status = 3;

            btnConfirm.Visible = false;
            btnUebernahme.Visible = false;
            btnReset.Visible = false;
            btnSave.Visible = false;
            btnBack.Visible = true;

            Int32 i = default(Int32);

            Haltefrist objFahrzeuge = new Haltefrist(ref m_User, m_App, Session["AppID"].ToString(), Session.SessionID.ToString(), "");

            objFahrzeuge.Bestand = (DataTable)Session["Haltefrist"];
            objFahrzeuge.Change(this.Page);


            DataTable TempTable = ((DataTable)Session["Haltefrist"]).Copy();

            for (i = TempTable.Rows.Count - 1; i >= 0; i += -1)
            {
                if (Convert.ToBoolean(TempTable.Rows[i]["ActionNOTHING"]))
                {
                    TempTable.Rows[i].Delete();
                }
            }
            TempTable.Columns.Remove("ActionDELE");
            TempTable.Columns.Remove("ActionNOTHING");

            string strTemp = "Es wurden " + TempTable.Rows.Count.ToString() + " Vorgänge beauftragt.";

            if (!(objFahrzeuge.Status == 0))
            {
                lblError.Text = objFahrzeuge.Message;
                strTemp += " Es traten Fehler auf.";
            }

            FillGrid(0,"");


        }

        protected void lbCreateExcel_Click(object sender, EventArgs e)
        {

            //Control control = new Control();
            //DataTable tblTranslations = new DataTable();
            DataTable TempTable = ((DataTable)Session["Haltefrist"]).Copy();



            //string AppURL = null;
            DataColumn col2 = null;
            //int bVisibility = 0;
            //int i = 0;
            //string sColName = "";
            //AppURL = this.Request.Url.LocalPath.Replace("/Services", "..");
            //tblTranslations = (DataTable)this.Session[AppURL];


            foreach (DataControlField col in gvAusgabe.Columns)
            {

                col2 = TempTable.Columns[col.SortExpression.ToUpper()];

                if (col2 != null)
                {
                    col2.ColumnName = col.HeaderText;
                    TempTable.AcceptChanges();
                }

            }

            DataColumn EditCol;

            for (int i = TempTable.Columns.Count - 1; i >= 0; i += -1)
            {

                if (TempTable.Columns[i].ColumnName.Contains("col_") == false)
                {
                    EditCol = TempTable.Columns[i];

                    TempTable.Columns.Remove(EditCol);
                    TempTable.AcceptChanges();
                }
                else
                {
                    TempTable.Columns[i].ColumnName = TempTable.Columns[i].ColumnName.Replace("col_", "");
                    TempTable.AcceptChanges();
                }


            }





            //foreach (DataColumn TCol in TempTable.Columns)
            //{
            //    if (TCol.ColumnName.Contains("col_") == false)
            //    {
            //        EditCol = TCol;

            //        TempTable.Columns.Remove(EditCol);
            //        TempTable.AcceptChanges();
            //    }
            //    else
            //    {
            //        TCol.ColumnName = TCol.ColumnName.Replace("col_", "");
            //        TempTable.AcceptChanges();
            //    }
                

            //}


            //foreach (DataControlField col in gvAusgabe.Columns)
            //{

            //    for (i = TempTable.Columns.Count - 1; i >= 0; i += -1)
            //    {
            //        bVisibility = 0;
            //        col2 = TempTable.Columns[i];
            //        if (col2.ColumnName.ToUpper() == col.SortExpression.ToUpper())
            //        {
            //            sColName = Common.TranslateColLbtn(gvAusgabe, tblTranslations, col.HeaderText, ref bVisibility);
            //            if (bVisibility == 0)
            //            {
            //                TempTable.Columns.Remove(col2);
            //            }
            //            else if (sColName.Length > 0)
            //            {
            //                col2.ColumnName = sColName;
            //            }
            //        }
            //    }
            //    TempTable.AcceptChanges();
            //}



            CKG.Base.Kernel.DocumentGeneration.ExcelDocumentFactory excelFactory = new CKG.Base.Kernel.DocumentGeneration.ExcelDocumentFactory();
            string filename = String.Format("{0:yyyyMMdd_HHmmss_}", System.DateTime.Now) + m_User.UserName;
            excelFactory.CreateDocumentAndSendAsResponse(filename, TempTable, this.Page, false, null, 0, 0);




        }

        protected void btnBack_Click(object sender, EventArgs e)
        {
            Response.Redirect("Change02.aspx?AppID=" + Session["AppID"].ToString());
        }




    }
}
