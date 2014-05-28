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
    public partial class Change01 : System.Web.UI.Page
    {
        private CKG.Base.Kernel.Security.User m_User;
        private CKG.Base.Kernel.Security.App m_App;
        private LP_01 objHandler;

        protected void Page_Load(object sender, EventArgs e)
        {
            m_User = Common.GetUser(this);

            Common.FormAuth(this, m_User);

            m_App = new App(m_User); //erzeugt ein App_objekt 

            Common.GetAppIDFromQueryString(this);

            lblHead.Text = (string)m_User.Applications.Select("AppID = '" + Session["AppID"] + "'")[0]["AppFriendlyName"];

            if (Session["objHandler"]!=null)
            {
                if (IsPostBack == false)
                {
                
                }
                
            }
            if (objHandler != null)
            {
                objHandler = null;
            }

         }

        protected void cmdSearch_Click(object sender, EventArgs e)
        {
            DoSubmit();
        }

        private void DoSubmit()
        {
    
            lblError.Text = "";
            lblError.Visible = false;

            objHandler = new LP_01(ref m_User, m_App, "");
            objHandler.Leasingvertragsnummer = txtLeasingnummer.Text;

            if (txtKunde.Text.Length > 0) 
            {
                objHandler.Leasingnehmer = txtKunde.Text.Replace("*","%");
            }
            objHandler.Aktion = rbAktion.SelectedItem.Value;
            objHandler.GiveCars(Session["AppID"].ToString(),Session.SessionID.ToString(), this);

            if (objHandler.Fahrzeuge == null)
            {
                lblError.Visible = true;
                lblError.Text = objHandler.Message;
            }
            else 
            {
                Session["objHandler"] = objHandler;
                Response.Redirect("Change01_2.aspx?AppID=" + Session["AppID"].ToString());
            
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

        protected void lbBack_Click(object sender, EventArgs e)
        {
            Session["objHandler"] = null;
            Response.Redirect("/Services/(S(" + Session.SessionID + "))/Start/Selection.aspx");
        }

    }
}
