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

namespace Leasing.forms
{
	public partial class Input_002_01 : System.Web.UI.Page
	{
		private CKG.Base.Kernel.Security.User m_User;
		private CKG.Base.Kernel.Security.App m_App;
		
		protected void Page_Load(object sender, EventArgs e)
		{
			Session["ShowLink"] = "False";
			m_User = Common.GetUser(this);
			Common.FormAuth(this, m_User);

			Common.GetAppIDFromQueryString(this);
			try
			{
				lblHead.Text = m_User.Applications.Select("AppID = '" + Session["AppID"].ToString() + "'")[0]["AppFriendlyName"].ToString();
				m_App = new CKG.Base.Kernel.Security.App(m_User);

				if( !IsPostBack)
				{
					Session["ShowLink"] = "False";
					Session["ResultTable"] = null;
				}

            //DoSubmit();
			}
			catch (Exception ex)
			{
				lblError.Text = "Beim Laden der Seite ist ein Fehler aufgetreten.<br>(" + ex.Message + ")";
			}
		}

		

		private void DoSubmit()
		{
			Session["lnkExcel"] = "";
			try
			{
				DateTime now = DateTime.Now;
				string strFileName =  now.Year.ToString()+ now.Month.ToString()+now.Day.ToString()+"_"+now.Hour.ToString()+
										now.Minute.ToString()+now.Second.ToString()+"_" + m_User.UserName + ".xls";
				LP_03 m_Report = new LP_03(m_User, m_App, strFileName);				
				//---> Eingabedaten
				string status;

				m_Report.PKennzeichenVon = txtKennzeichenVon.Text;
				m_Report.PKennzeichenBis = txtKennzeichenBis.Text;
				m_Report.PFahrgestellVon = txtFahrgestellVon.Text;
				m_Report.PFahrgestellBis = txtFahrgestellBis.Text;
				m_Report.PLeasingNrVon = txtLeasVVon.Text;
				m_Report.PLeasingNrBis = txtLeasVBis.Text;

				m_Report.PKundenNr = txtKundennr.Text;
				m_Report.PKlaerfall = lblKF.Checked;
            

				if (rbSelect.SelectedItem.Value == "H")
				{    
					status = rbStatus.SelectedItem.Value.ToString();
				}
				else
				{
					status = rbMahnung.SelectedItem.Value.ToString();
				}

				m_Report.Fill(Session["AppID"].ToString(), Session.SessionID.ToString(),this, status, rbSelect.SelectedItem.Value,false);
				Session["ResultTable"] = m_Report.Result;
				Session["ResultTableNative"] = m_Report.getNativeData(); //Alle Spalten merken 
                

				if (m_Report.Status != 0)
				{
					lblError.Text = "Fehler: " + m_Report.Message;
				}
				else
				{
					if (m_Report.Result.Rows.Count == 0)
					{
						lblError.Text = "Keine Ergebnisse für die gewählten Kriterien.";
					}
					else
					{
						Session["ExcelTable"] = m_Report.Result;
						Response.Redirect("Report_002_01.aspx?AppID=" + Session["AppID"].ToString() + "&typ=" + rbSelect.SelectedItem.Value);
					}
				}

			}
			catch (Exception ex)
			{
				lblError.Text = "Beim Erstellen des Reportes ist ein Fehler aufgetreten.<br>(" + ex.Message + ")";
			}
		}

		private void lnkAuswahl_Click(object sender, EventArgs e)
		{
			if (rbSelect.SelectedItem.Value == "H")
			{
				rbStatus.Visible = true;
				rbMahnung.Visible = false;
				//lblTitel.Text = "Historie"
			}
			else
			{
				rbStatus.Visible = false;
				rbMahnung.Visible = true;
				//lblTitel.Text = "Mahnstufen"
			}
		}

        protected void lbBack_Click(object sender, EventArgs e)
        {
            Session["ResultTable"] = null;
            Session["ResultTableNative"] = null;
            Session["ExcelTable"] = null;
            Response.Redirect("/Services/(S(" + Session.SessionID + "))/Start/Selection.aspx");
        }

		private void Page_PreRender(object sender, EventArgs e) //MyBase.PreRender
		{
			Common.SetEndASPXAccess(this);
		}

		private void Page_Unload(object sender, EventArgs e) //Handles MyBase.Unload
        {
			Common.SetEndASPXAccess(this);
		}

		protected void cmdCreate_Click(object sender, EventArgs e)
		{		
			rbSelect.SelectedItem.Value = "H";
			Session["ShowLink"] = "True";
			DoSubmit();
		}
	}
}
