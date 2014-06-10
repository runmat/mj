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
	public partial class Login : System.Web.UI.Page
	{
        DB_Kantine DB;
        Kantine Kan;

        protected void Page_Init(object sender, EventArgs e)
        {
            Kan = (Kantine)this.Master;            
            DB = Kan.Database;
        }

		protected void Page_Load(object sender, EventArgs e)
		{
            if (ConfigurationManager.AppSettings["Wartungsmodus"].ToString() == "1")
            {
                FormsAuthentication.RedirectFromLoginPage("Wartung", false);
            }
			txtLogin.Focus();
		}

		protected void btnLogin_Click(object sender, EventArgs e)
		{
			string User = txtLogin.Text.Trim();
			DataTable dt = new DataTable();
			dt = DB.GetBenutzerAllByBenutzername(User);
			if (dt.Rows.Count == 1)
			{
				if (!(bool)dt.Rows[0]["Gesperrt"])
				{
					if (txtPasswort.Text.Trim() != "")
					{
                        if ((string)dt.Rows[0]["Passwort"] == KantinenBenutzer.PasswortVerschlüsseln(txtPasswort.Text.Trim()) || (string)dt.Rows[0]["Passwort"] == txtPasswort.Text.Trim())
                        {
                            bool admin = Convert.ToBoolean(dt.Rows[0]["Admin"]);
                            bool seller = Convert.ToBoolean(dt.Rows[0]["Seller"]);
                            bool useradmin = Convert.ToBoolean(dt.Rows[0]["Useradmin"]);

                            Session["Benutzer"] = new KantinenBenutzer(User, admin, seller, useradmin);

                            FormsAuthentication.RedirectFromLoginPage(txtLogin.Text, false);
                        }
                        else
                        {
                            lblError.Text = "Passwort falsch!";
                        }
					}
					else
					{
						lblError.Text = "Geben Sie ein Passwort ein!";
					}
				}
				else 
				{
					lblError.Text = "Benutzerkonto ist gesperrt!";
				}		
			}
			else 
			{
				lblError.Text = "Login falsch!";
			}
		}
	}
}
