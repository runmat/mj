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
    public partial class Change02 : System.Web.UI.Page
    {
        private CKG.Base.Kernel.Security.User m_User;
        private CKG.Base.Kernel.Security.App m_App;
        private Lieferprognosen objLieferprog;

        protected void Page_Load(object sender, EventArgs e)
        {
            m_User = Common.GetUser(this);

            Common.FormAuth(this, m_User);

            m_App = new App(m_User); //erzeugt ein App_objekt 

            Common.GetAppIDFromQueryString(this);

            lblHead.Text = (string)m_User.Applications.Select("AppID = '" + Session["AppID"] + "'")[0]["AppFriendlyName"];
            
            FillControlls();


       }

        private void Page_PreRender(object sender, System.EventArgs e)
        {
            Common.SetEndASPXAccess(this);
        }

        private void Page_Unload(object sender, System.EventArgs e)
        {
            Common.SetEndASPXAccess(this);
        }

        private void FillControlls()
        {
            DataTable tblMarke;


            objLieferprog = new Lieferprognosen(ref m_User, m_App, "");
            tblMarke = objLieferprog.getMarke(Session["AppID"].ToString(), Session.SessionID.ToString(), this);
          
            Int32 e;

            e = 0;
            drpMarke.Items.Add(new ListItem("Keine Auswahl", "0"));

            if ((tblMarke != null) && (tblMarke.Rows.Count > 0))
            {
                do
                {
                    drpMarke.Items.Add(new ListItem(tblMarke.Rows[e]["MARKE_TXT"].ToString(), tblMarke.Rows[e]["MARKE"].ToString()));
                    e += 1;
                } while (e < tblMarke.Rows.Count);
            }

            if (e==0)
            {
                drpMarke.Enabled = false;
            }
            
        }

        protected void btnConfirm_Click(object sender, EventArgs e)
        {
            if (drpMarke.SelectedItem.Value == "0")
            {
                objLieferprog.Marke = "";                
            }
            else
            {
                objLieferprog.Marke = drpMarke.SelectedItem.Value;  
            }

            DoSubmit();
        }
        private void DoSubmit()
        {

            lblError.Text = "";
            lblError.Visible = false;

            if (objLieferprog == null)
            {
                objLieferprog = new Lieferprognosen(ref m_User, m_App, "");
            }
                objLieferprog.Aktion = rbAktion.SelectedItem.Value;

                objLieferprog.GiveCars(Session["AppID"].ToString(), Session.SessionID.ToString(), this);

                if (objLieferprog.Master == null)
                {
                    lblError.Visible = true;
                    lblError.Text = objLieferprog.Message;
                }
                else 
                {
                    Session["objLieferprog"] = objLieferprog;    
                    Response.Redirect("Change02_2.aspx?AppID=" + Session["AppID"].ToString());
                }

        }

        protected void lbBack_Click(object sender, EventArgs e)
        {
            objLieferprog = null;
            Session["objLieferprog"] = null;
            Response.Redirect("/Services/(S(" + Session.SessionID + "))/Start/Selection.aspx");
        }
    }
}
