using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CKG.Base.Business;
using CKG.Base.Kernel;
using CKG.Services.PageElements;
using CKG.Base.Kernel.Common;
using System.Configuration;
namespace Leasing.forms
{
	public partial class Report_002_01 : System.Web.UI.Page
	{
		private CKG.Base.Kernel.Security.App m_App; 
		private CKG.Base.Kernel.Security.User m_User;
        protected global::CKG.Services.GridNavigation GridNavigation1;
		private DataTable m_objTable;

		protected void Page_Load(object sender, EventArgs e)
		{
			
			m_User = Common.GetUser(this);
			
			Common.FormAuth(this, m_User);
			if (Session["ResultTable"] == null)
			{
				Response.Redirect(Request.UrlReferrer.ToString());
			}
			else
			{
				m_objTable = (DataTable)Session["ResultTable"];
			}
			lblHead.Text = m_User.Applications.Select("AppID = '" + Session["AppID"].ToString() + "'")[0]["AppFriendlyName"].ToString();
			
			GridNavigation1.setGridElment(ref GridView1);
            GridNavigation1.PagerChanged += GridView1_PageIndexChanged;
            GridNavigation1.PageSizeChanged += GridView1_ddlPageSizeChanged;

			try
			{
				m_App = new CKG.Base.Kernel.Security.App(m_User);

				if(!IsPostBack)
				{
					FillGrid(0,"");
				}
			}
			catch (Exception ex)
			{
                lblError.Text = "Beim Laden der Seite ist ein Fehler aufgetreten: " + ex.Message;
			}
		}

		private void FillGrid(int intPageIndex,string strSort)
		{
			if(m_objTable.Rows.Count == 0)
			{
                GridView1.Visible = false;
				lblNoData.Visible = true;
				lblNoData.Text = "Keine Daten zur Anzeige gefunden.";
				//ShowScript.Visible = false;
			}
			else
			{

                GridView1.Visible = true;
				lblNoData.Visible = false;

				DataView tmpDataView = new DataView();
				tmpDataView = m_objTable.DefaultView;

				int intTempPageIndex = intPageIndex;
				string strTempSort = "";
				string strDirection = "";

				if( strSort.Trim().Length > 0)
				{
					intTempPageIndex = 0;
					strTempSort = strSort.Trim();
					if(ViewState["Sort"] == null || ViewState["Sort"].ToString() == strTempSort)
					{
						if(ViewState["Direction"] == null)
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
					if(ViewState["Sort"] != null)
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

				if(strTempSort.Length != 0)
				{
					tmpDataView.Sort = strTempSort + " " + strDirection;
				}

                GridView1.PageIndex = intTempPageIndex;
                GridView1.DataSource = tmpDataView;
                GridView1.DataBind();


                    GridView1.Columns[1].Visible = false;     //Equipmentnr ausblenden
                    GridView1.Columns[GridView1.Columns.Count - 2].Visible = false;     //Klärfallspalte am Ende ausblenden
                    GridView1.Columns[GridView1.Columns.Count - 1].Visible = false;     //KlärfallspalteInfo am Ende ausblenden


				if (Session["ShowOtherString"] != null && Session["ShowOtherString"].ToString().Length > 0)
				{
				    lblNoData.Text = Session["ShowOtherString"].ToString();
				}
				else
				{
				    lblNoData.Text = "Es wurden " + tmpDataView.Count.ToString() + " Einträge zu '" + 
						m_User.Applications.Select("AppID = '" + Session["AppID"].ToString() + "'")[0]["AppFriendlyName"].ToString() + "' gefunden.";
				}
                //if (Session["BackLink"] != null && Session["BackLink"].ToString() == "HistoryBack")
                //{
                //    lnkZurueck.Text = "Zurück";
                //    lnkZurueck.NavigateUrl = "javascript:history.back()";
                //}
				lblNoData.Visible = true;

	
			}
		}

		private void Page_PreRender(object sender, EventArgs e) //Handles MyBase.PreRender
		{   
			Common.SetEndASPXAccess(this);
            HelpProcedures.FixedGridViewCols(GridView1);
		}

		private void Page_Unload(object sender, EventArgs e) //Handles MyBase.Unload
		{   
			Common.SetEndASPXAccess(this);

		}

        private void GridView1_PageIndexChanged(Int32 pageindex)
        {
            FillGrid(pageindex, "");
        }

        private void GridView1_ddlPageSizeChanged()
        {
            FillGrid(0, "");
        }

        protected void lbBack_Click(object sender, EventArgs e)
        {
            Response.Redirect("Input_002_01.aspx?AppID=" + (string)Session["AppID"]);
        }

        protected void GridView1_Sorting1(object sender, GridViewSortEventArgs e)
        {
            FillGrid(GridView1.PageIndex, e.SortExpression);
        }

        protected void lnkCreateExcel1_Click(object sender, EventArgs e)
        {
            Control control = new Control();
            DataTable tblTranslations = new DataTable();
            DataTable tblTemp = m_objTable;
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

        protected void GridView1_RowCommand(object sender, GridViewCommandEventArgs e)
        {

            DataTable tblData;
            DataRow selectedRow;
            tblData = (DataTable)Session["ResultTableNative"];
            hfEquinr.Value = e.CommandArgument.ToString();
            selectedRow = tblData.Select("Equipment='" + hfEquinr.Value + "'")[0];
            lblLVNr.Text = selectedRow["LVNr"].ToString();
            ModalPopupExtender.Show();
        }

        protected void cmdSave_Click(object sender, EventArgs e)
        {
            String status = "";
            if (checkDate())
            {
                sendMail(ref status);
                if (status != String.Empty)
                {
                    lblError.Text = status;
                    ModalPopupExtender.Show();
                }
                else
                {
                    lblError.Text = "Vorgang erfolgreich.";
                                      
                }
            }
            else
            {
                lblMessagePopUp.Text = "Keine gültigen Eingabedaten.(z.B. Datumsformat)";
                ModalPopupExtender.Show();
            }
        }
        protected Boolean checkDate()
        {
            Boolean blnResult = false;

            if (txtBemerkung.Text.Trim() != String.Empty)
            {
                blnResult = true;
            }
            if (IsDate(txtDatum.Text))
            {
                blnResult = true;
            }
            if (cbxEnt.Checked)
            { blnResult = true; }

            if (cbxFahrz.Checked)
            { blnResult = true; }

            if (cbxSB.Checked)
            { blnResult = true; }

            if (cbxVers.Checked)
            { blnResult = true; }

            return blnResult;

        }

        protected void sendMail(ref String status)
        {
            status = "";
            try
            {
                String strMailAdresse = Common.GetGeneralConfigValue("SicherungsscheinKlaerfaelle", "MailEmpfaenger");
                if (String.IsNullOrEmpty(strMailAdresse))
                {
                    // Fallback, falls keine Einstellung gepflegt
                    strMailAdresse = ConfigurationManager.AppSettings["SmtpMailAddress"];
                }

                String strMailBody = "Benutzername: " + m_User.UserName + "\r\n";

                strMailBody += "LV-Nr.: " + lblLVNr.Text + "\r\n";
                strMailBody += "LV beendet zum: " + txtDatum.Text + "\r\n";
                strMailBody += "SB ist in Ordnung: ";

                if (cbxSB.Checked)
                {
                    strMailBody += "(X)";
                }
                else
                {
                    strMailBody += "()";
                }

                strMailBody += "\r\nHöhe der Entschädigung im Schadensfall ist in Ordnung: ";

                if (cbxEnt.Checked)
                {
                    strMailBody += "(X)";
                }
                else
                {
                    strMailBody += "()";
                }

                strMailBody += "\r\nVersichererwechsel: ";
                if (cbxVers.Checked)
                {
                    strMailBody += "(X)";
                }
                else
                {
                    strMailBody += "()";
                }

                strMailBody += "\r\nFahrzeugwechsel: ";
                if (cbxFahrz.Checked)
                {
                    strMailBody += "(X)";
                }
                else
                {
                    strMailBody += "()";
                }

                strMailBody += "Sonstiges: " + txtBemerkung.Text + "\r\n";

                System.Net.Mail.MailMessage mail = new System.Net.Mail.MailMessage(ConfigurationManager.AppSettings["SmtpMailSender"], strMailAdresse,  //
                                                    "Klärfallmail Leaseplan", strMailBody);

                System.Net.Mail.SmtpClient client = new System.Net.Mail.SmtpClient(ConfigurationManager.AppSettings["SmtpMailServer"]);
                client.Send(mail);
            }
            catch (Exception Ex)
            {
                status = "Fehler beim Versenden der Mail." + Ex.Message;
                throw;
            }

        }
        public static bool IsDate(string strDate)
        {
            if (strDate == null)
            {
                strDate = "";
            }
            if (strDate.Length > 0)
            {
                DateTime dummyDate;
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
