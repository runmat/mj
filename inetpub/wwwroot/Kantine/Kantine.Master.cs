using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using System.Configuration;

namespace Kantine
{
    public partial class Kantine : System.Web.UI.MasterPage
	{
		DB_Kantine DB = new DB_Kantine(ConfigurationManager.ConnectionStrings["CurrentCon"].ConnectionString);
        KantinenBenutzer KB;

        #region Properties

        /// <summary>
        /// Objekt zur Datenbankkommunikation
        /// </summary> 
        public DB_Kantine Database {
            get { return DB; }
        }

        /// <summary>
        /// Daten des aktuellen Benutzers
        /// </summary>
        public KantinenBenutzer User {
            get {
                return KB;
            }
        }

        #endregion


        protected void Page_Init(object sender, EventArgs e)
		{
            if (! (this.Page is Login))
            {
                if (Session["Benutzer"] != null)
                {
                    KB = (KantinenBenutzer)Session["Benutzer"];                   
                }
                else
                {
                    FormsAuthentication.RedirectToLoginPage();
                }
            }
		}

		protected void Page_Load(object sender, EventArgs e)
		{
            if (Session["IsWartungRedirect"] == null || !(bool)Session["IsWartungRedirect"])
            {
                if (ConfigurationManager.AppSettings["Wartungsmodus"].ToString() == "1")
                {
                    LiBenutzer.Visible = false;
                    LiVerkauf.Visible = false;
                    LiArtikel.Visible = false;
                    LiWarengruppe.Visible = false;
                    LiKontoNachm.Visible = false;
                    LiVerkaufsuebersicht.Visible = false;
                    LiNachmittagsmodus.Visible = false;

                    object obj = this;
                   
                    
                    if (!IsPostBack)
                    {
                        Session["IsWartungRedirect"] = true;
                        Response.Redirect("Wartung.aspx");
                    }
                    else { return; }
                }
            }
            else 
            { 
                Session["IsWartungRedirect"] = false;
                return;
            }
            
            // Berechtigungen aus Login prüfen und Seiten einblenden

            LiBenutzer.Visible = false;
            LiVerkauf.Visible = false;
            LiArtikel.Visible = false;
            LiWarengruppe.Visible = false;
            LiKontoNachm.Visible = false;
            LiVerkaufsuebersicht.Visible = false;
            LiNachmittagsmodus.Visible = true;

            if (KB != null)
            {
                lblUserName.Text = KB.Name;

                if (KB.IsSeller)
                {
                    LiBenutzer.Visible = true;
                    LiVerkauf.Visible = true;
                    LiArtikel.Visible = true;
                    LiWarengruppe.Visible = true;
                    LiKontoNachm.Visible = true;
                    LiVerkaufsuebersicht.Visible = true;
                    LiNachmittagsmodus.Visible = false;
                }
                if (KB.IsUseradmin)
                {
                    LiBenutzer.Visible = true;
                    LiNachmittagsmodus.Visible = false;
                }
                if (KB.IsAdmin)
                {
                    LiBenutzer.Visible = true;
                    LiVerkauf.Visible = true;
                    LiArtikel.Visible = true;
                    LiWarengruppe.Visible = true;
                    LiKontoNachm.Visible = true;
                    LiVerkaufsuebersicht.Visible = true;
                    LiNachmittagsmodus.Visible = false;
                }
            }
        }

		protected void lbtnNachmittag_Click(object sender, EventArgs e)
		{
            KantinenBenutzer KB = new KantinenBenutzer("Nachmittagsmodus",false,false,false);
            Session["Benutzer"] = KB;
			FormsAuthentication.RedirectFromLoginPage("Nachmittagsmodus", false);
			Response.Redirect("MainPage.aspx");
		}


		protected void btnLogout_Click(object sender, EventArgs e)
		{
			Session.Clear();
            
			FormsAuthentication.SignOut();
			FormsAuthentication.RedirectToLoginPage();		}
	}
}
