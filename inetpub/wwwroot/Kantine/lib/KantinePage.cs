using System;

namespace Kantine
{
    public class KantinePage : System.Web.UI.Page
    {
        protected DB_Kantine DB;
        protected KantinenBenutzer KB;
        protected Kantine Kan;

        protected void Page_Init(object sender, EventArgs e)
        {
            Kan = (Kantine)this.Master;
            KB = Kan.User;
            DB = Kan.Database;
        }
    }
}