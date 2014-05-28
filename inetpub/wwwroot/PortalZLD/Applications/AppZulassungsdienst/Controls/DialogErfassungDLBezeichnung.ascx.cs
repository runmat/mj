using System;

namespace AppZulassungsdienst.Controls
{
    public partial class DialogErfassungDLBezeichnung : System.Web.UI.UserControl
    {
        public event EventHandler TexteingabeBestaetigt;

        public string DLBezeichnung
        {
            get { return txtDLBezeichnung.Text; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnClose_Click(object sender, EventArgs e)
        {
            TexteingabeBestaetigt(this, new EventArgs());
        }

    }
}