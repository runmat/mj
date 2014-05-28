using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CKG;
using CKG.Base.Business;
using CKG.Base.Kernel;
using CKG.Base.Kernel.Common;
using CKG.Base.Kernel.Security;
using CKG.Services.PageElements;
using Leasing.lib;
using System.Configuration;

namespace Leasing.forms
{
	public partial class Input_002_02 : System.Web.UI.Page
	{
		public Input_002_02()
		{
			this.PreRender += new EventHandler(Page_PreRender);
			this.Unload += new EventHandler(Page_Unload);
		}
		
		private CKG.Base.Kernel.Security.User m_User; 
		private CKG.Base.Kernel.Security.App m_App;

		private string flag;

		protected void Page_Load(object sender, EventArgs e)
		{
			//System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(ConfigurationManager.AppSettings["Connectionstring"]);

			Session["ShowLink"] = "False";
			m_User = Common.GetUser(this);
			Common.FormAuth(this, m_User);

			if( Request.QueryString["FLAG"] != null )
			{
				flag = Request.QueryString["FLAG"].ToString();
			}

			Common.GetAppIDFromQueryString(this);
			try
			{
				lblHead.Text = m_User.Applications.Select("AppID = '" + Session["AppID"].ToString() + "'")[0]["AppFriendlyName"].ToString();
				m_App = new CKG.Base.Kernel.Security.App(m_User);

				if (flag == "1")
				{
					DoSubmit(1);
				}
			}
			catch (Exception ex)
			{
				lblError.Text = "Beim Laden der Seite ist ein Fehler aufgetreten.";
			}
		}

		private void DoSubmit(int direct)
		{
			Session["lnkExcel"] = "";
			try
			{
				DateTime now = DateTime.Now;
				string strFileName =  now.Year.ToString()+ now.Month.ToString()+now.Day.ToString()+"_"+now.Hour.ToString()+
									now.Minute.ToString()+now.Second.ToString()+"_" + m_User.UserName + ".xls";
				LP_03 m_Report = new LP_03(m_User, m_App, strFileName);				
				//---> Eingabedaten

				string status = "";
				string art = "";

				m_Report.PKennzeichenVon = txtKennzeichenVon.Text;
				m_Report.PKennzeichenBis = txtKennzeichenBis.Text;
				m_Report.PFahrgestellVon = txtFahrgestellVon.Text;
				m_Report.PFahrgestellBis = txtFahrgestellBis.Text;
				m_Report.PLeasingNrVon = txtLeasVVon.Text;
				m_Report.PLeasingNrBis = txtLeasVBis.Text;


				m_Report.PKundenNr = txtKundennr.Text;
				m_Report.PKlaerfall = lblKF.Checked;
            

				art = rbSelect.SelectedItem.Value;
				art = "M";

				status = rbMahnung.SelectedItem.Value.ToString();
		        m_Report.Fill(Session["AppID"].ToString(), Session.SessionID.ToString(), this, status, art, true);

				Session["ResultTable"] = m_Report.Result;
				Session["ResultTableNative"] = m_Report.getNativeData(); //Alle Spalten merken 

				if (m_Report.Status != 0)
				{
					lblError.Text = m_Report.Message;
				}
				else
				{
					if( m_Report.Result.Rows.Count == 0)
					{
						lblError.Text = "Keine Ergebnisse für die gewählten Kriterien.";						
					}
					else
					{
                        if (direct == 0)
                        { 
                            Response.Redirect("Report_002_03.aspx?AppID=" + Session["AppID"].ToString());
                        }
                        //else
                        //{
                        //    Response.Redirect("Report_002_04.aspx?AppID=" + Session["AppID"].ToString());
                        //}
						
					}
				}

			}
			catch (Exception ex)
			{
				lblError.Text = "Beim Erstellen des Reportes ist ein Fehler aufgetreten.<br>(" + ex.Message + ")";
			}
		}

        protected void lbBack_Click(object sender, EventArgs e)
        {
            Response.Redirect("/Services/(S(" + Session.SessionID + "))/Start/Selection.aspx");
        }

		protected void cmdCreate_Click(object sender, EventArgs e) 
		{
			Session["ShowLink"] = "True";
			DoSubmit(0);
		}

		private void Page_PreRender(object sender, EventArgs e) 
		{
			Common.SetEndASPXAccess(this);
		}

		private void Page_Unload(object sender, EventArgs e) 
		{
			Common.SetEndASPXAccess(this); 
		}
	}
}
