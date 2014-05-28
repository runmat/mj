using System;

namespace Kantine
{
	public partial class Default : System.Web.UI.Page
	{
        KantinenBenutzer KB;
        Kantine Kan;

        public Default()
        {
            Init += Page_Init;
            Load += Page_Load;
        }

        protected void Page_Init(object sender, EventArgs e)
        {
            Kan = (Kantine)this.Master;
            KB = Kan.User;
        }

		protected void Page_Load(object sender, EventArgs e)
		{
            if (Convert.ToBoolean(Session["IsWartungRedirect"]))
            {
                return;
            }
            
            if (KB != null)
            {
                switch (KB.HighestUserLevel)
                {
                    case UserLevel.Administration:
                        Response.Redirect("./Benutzer.aspx");
                        break;
                    case UserLevel.Benutzersteuerung:
                        Response.Redirect("./Benutzer.aspx");
                        break;
                    case UserLevel.Nachmittagsmodus:
                        Response.Redirect("./MainPage.aspx");
                        break;
                    case UserLevel.Seller:
                        Response.Redirect("./MainPage.aspx");
                        break;
                    default:
                        Response.Redirect("./Login.aspx");
                        break;
                }
            }
            Response.Redirect("./Login.aspx");
            
		}
	}
}
