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
using System.Globalization;
using CKG.Base.Business;

namespace Leasing.forms
{
    public partial class Change01_2 : System.Web.UI.Page
    {
        private CKG.Base.Kernel.Security.User m_User;
        private CKG.Base.Kernel.Security.App m_App;
        protected global::CKG.Services.GridNavigation GridNavigation1;
        private LP_01 objHandler;
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

            try
            {

                if (!IsPostBack)
                {
                    if ((Session["objHandler"] == null))
                    {
                        lblError.Visible = true;
                        lblError.Text = "Keine Dokumente zur Anzeige gefunden.";
                    }
                    else
                    {
                        objHandler = (LP_01)Session["objHandler"];
                        Fillgrid(0, "");
                    }

                }
                else
                {
                    objHandler = (LP_01)Session["objHandler"];
                }

            }
            catch
            {
                lblError.Visible = true;
                lblError.Text = "Keine Dokumente zur Anzeige gefunden.";
            }

        }

        private void Fillgrid(Int32 intPageIndex, String strSort)
        {
            if (objHandler.Fahrzeuge.Rows.Count == 0)
            {
                Result.Visible = false;
                lblNoData.Visible = true;
                lblNoData.Text = "Keine Dokumente zur Anzeige gefunden.";
            }
            else
            {
                Result.Visible = true;
                lblNoData.Visible = false;
                DataView tmpDataView = new DataView();
                tmpDataView = objHandler.Fahrzeuge.DefaultView;

                
				if (cmdSend.Visible == false)
				{
					tmpDataView.RowFilter = "STATUS <> ''";
				}

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


        private void Page_PreRender(object sender, System.EventArgs e)
        {
            Common.SetEndASPXAccess(this);
			HelpProcedures.FixedGridViewCols(GridView1);
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

        protected void lbBack_Click(object sender, EventArgs e)
        {
            Response.Redirect("Change01.aspx?AppID=" + (string)Session["AppID"]);
        }


        protected void GridView1_Sorting1(object sender, GridViewSortEventArgs e)
        {
            Fillgrid(GridView1.PageIndex, e.SortExpression);
        }

        protected void cmdSend_Click(object sender, EventArgs e)
        {
			if (CheckGrid() ==true)
			{
				lblError.Text = "Die Daten können nicht gespeichert werden. Bitte überprüfen Sie Ihre Eingaben.";
			}

			else
			{
				objHandler.SaveLiefertermin(Session["AppID"].ToString(), Session.SessionID.ToString(), this);
				Session["objHandler"] = objHandler;
				cmdSend.Visible = false;
				Fillgrid(0,"");
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


        private Boolean CheckGrid ()  
        {
            Boolean bReturn = false;

            TextBox txtBox = default(TextBox);
            TextBox txtBoxJahr = default(TextBox);
            TextBox txtBoxLiefertermin = default(TextBox);
            Boolean booError = false;
            Int32 NowWeek = AbsoluteGetWeekOfYear(DateTime.Now);
            Int32 NowYear = DateTime.Now.Year;
            DateTime Liefertermin;


            foreach (GridViewRow Row in GridView1.Rows)
            {

                txtBox = (TextBox)Row.Cells[6].FindControl("txtLwNeu");
                txtBoxJahr = (TextBox)Row.Cells[6].FindControl("txtLwNeuJahr");
                txtBoxLiefertermin = (TextBox)Row.Cells[8].FindControl("txtLiefertermin");

                Label lblLeasingnr = (Label)Row.Cells[1].FindControl("lblLeasingnummer");
                DataRow FahrzeugeRow = objHandler.Fahrzeuge.Select("Leasingnummer='" + lblLeasingnr.Text + "'")[0];


                if (txtBox != null)
                {

                    if (txtBox.Text != String.Empty)
                    {
                      if (txtBox.Text.Equals(FahrzeugeRow["LWWoche"].ToString()) == false)
                        {
                       if (IsInteger(txtBox.Text)==false)
                        {
                            CreateErrString("LW: nicht numerisch.");
                            booError = true;
                        }


                         if (Convert.ToInt32(txtBox.Text) < 1 || Convert.ToInt32(txtBox.Text) > 52)
                        {
                            CreateErrString("LW: Falscher Wert.");
                            booError = true;
                        }

                               else if (Convert.ToInt32(txtBox.Text) < Convert.ToInt32(NowWeek))
                            {
                                if (Convert.ToInt32(txtBoxJahr.Text) <= Convert.ToInt32(NowYear)) 
                                { 
                                    CreateErrString("LW: liegt in der Vergangenheit");
                                    booError = true;                                
                                }
                            }
                             else if (Convert.ToInt32(txtBoxJahr.Text) < Convert.ToInt32(NowYear))
                             {
                                 CreateErrString("LW: liegt in der Vergangenheit");
                                 booError = true;
                             }
                      }
 
                    }
                    if (txtBoxJahr.Text != String.Empty)
                    {
                        if (txtBoxJahr.Text.Equals(FahrzeugeRow["LWJahr"].ToString()) == false)
                        { 
                            if (IsInteger(txtBoxJahr.Text) == false)
                            {
                                CreateErrString("LW: Jahreszahl nicht numerisch");
                                booError = true;
                            }
                        else if (txtBoxJahr.Text.Length!=4)
                          {
                              CreateErrString("LW: Jahreszahl ist nicht vierstellig.");
                              booError = true;
                          }   
                        }
                    }
                    if (txtBoxLiefertermin.Text != String.Empty)
                    {
                        if (IsDate(txtBoxLiefertermin.Text) == false)
                        {
                            CreateErrString("Liefertermin: Falsches Datumsformat.");
                            booError = true;
                        }
                        else 
                        {
                            Liefertermin = DateTime.Parse(txtBoxLiefertermin.Text);
                            if (Liefertermin < DateTime.Now)
                            { 
                                CreateErrString("Lieferdatum: Termin liegt in der Vergangenheit.");
                                booError = true;                                
                            }

                        }
                    }
                    

                }
            


            if (strError != String.Empty) 
            {
                Label lblStatus = (Label)Row.Cells[9].FindControl("lblStatus");
                lblStatus.Text = strError;
            }
        }

        if (booError == true){ GridView1.Columns[9].Visible = true;}
        else { CheckChanges();}


        bReturn = booError;

        return bReturn;
            
        }
        private void CheckChanges() 
        {
          
           DataRow FahrzeugeRow;
           String strLiefertermin;
           Label lblLeasingnr;

            if (Session["objHandler"] == null)
            {
                objHandler = (LP_01)Session["objHandler"];
            }

            foreach (GridViewRow Row in GridView1.Rows)
            {
                Boolean booChanged; 
                strLiefertermin = string.Empty;
                booChanged = false;

                lblLeasingnr = (Label)Row.Cells[1].FindControl("lblLeasingnummer");
                FahrzeugeRow = objHandler.Fahrzeuge.Select("Leasingnummer='" + lblLeasingnr.Text + "'")[0];

                FahrzeugeRow.BeginEdit();

                FahrzeugeRow["STATUS"] = String.Empty;
                CheckBox chkConfirm = (CheckBox)(Row.Cells[7].FindControl("chkConfirm"));
                FahrzeugeRow["LWCONFIRM"] = chkConfirm.Checked;

                FahrzeugeRow.EndEdit();

                objHandler.Fahrzeuge.AcceptChanges();
            

                if (chkConfirm.Checked == true )
                {
                    booChanged = true;
                }

               TextBox txtLwNeu = (TextBox)Row.Cells[1].FindControl("txtLwNeu");
               TextBox txtLwNeuJahr = (TextBox)Row.Cells[1].FindControl("txtLwNeuJahr");
               TextBox txtLiefertermin = (TextBox)Row.Cells[1].FindControl("txtLiefertermin");
               TextBox txtIntNr = (TextBox)Row.Cells[1].FindControl("txtIntNr");


               if (txtLwNeu.Text.Equals(FahrzeugeRow["LWWoche"].ToString()) == false)
                {
                    booChanged = true;
                }

               if (txtLwNeuJahr.Text.Equals(FahrzeugeRow["LWJahr"].ToString()) == false)
                {
                    booChanged = true;
                }
                
                if (txtLiefertermin.Text.Equals(string.Empty) == false)
                {
                    booChanged = true;
                }
                if (txtIntNr.Text.Equals(FahrzeugeRow["NummerIntern"].ToString()) == false)
                {
                    booChanged = true;
                }


                if (booChanged == true) 
                {
                    DataRow SapExportRow = objHandler.SapImportTable.NewRow();
                    SapExportRow["Liznr"] = lblLeasingnr.Text;

                    if (txtLwNeu.Text.Trim().Length == 0)
                    {
                        SapExportRow["Dat_Lw"] = DBNull.Value;

                    }
                    else 
                    {
                        SapExportRow["Dat_Lw"] = txtLwNeuJahr.Text + txtLwNeu.Text;
                    
                    }
                    if (txtLiefertermin.Text.Trim().Length == 0)
                    {
                        SapExportRow["Dat_Lt"] = DBNull.Value;
                    }
                    else 
                    {
                        SapExportRow["Dat_Lt"] = txtLiefertermin.Text;
                    
                    }
                    SapExportRow["Ref3_Herst"] = txtIntNr.Text.Trim();

                    objHandler.SapImportTable.Rows.Add(SapExportRow);

                    SapExportRow = null;
                }

            }


            if (objHandler.SapImportTable.Rows.Count == 0) 
            {
                lblError.Text = "Sie haben keine Änderungen vorgenommen.";
            }
            

        
        }
        private void CreateErrString(String ErrText)
        {

            if (ErrText != string.Empty)
            {
                strError += "<br>" + ErrText;
            }
            else 
            {
                strError =  ErrText;
            }

        }
        public static int AbsoluteGetWeekOfYear(DateTime dt)
        {
            GregorianCalendar cal = new GregorianCalendar();

            int nWeek = cal.GetWeekOfYear(dt,
                    CalendarWeekRule.FirstDay,
                    DayOfWeek.Monday);
            return nWeek;
        }

        public static bool IsDate(string strDate)
        {
            if (strDate == null)
            {
                strDate = "";
            }
            if (strDate.Length > 0)
            {
                DateTime dummyDate ;
                try
                {
                    dummyDate = DateTime.Parse(strDate);
                }
                catch
                {
                    return false;
                }
                return true;
            }
            else
            {
                return false;
            }
        }

    }
}
