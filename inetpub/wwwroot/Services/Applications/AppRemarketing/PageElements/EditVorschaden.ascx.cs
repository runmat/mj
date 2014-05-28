using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using AppRemarketing.lib;

namespace AppRemarketing.PageElements
{
    public partial class EditVorschaden : System.Web.UI.UserControl
    {

        public System.Web.UI.Page TestPage;
        public System.Web.UI.WebControls.TextBox txtFin;
        public System.Web.UI.WebControls.TextBox txtKennzeichen;
        public System.Web.UI.WebControls.TextBox txtBeschreibung;
        public System.Web.UI.WebControls.TextBox txtPreis;
        public System.Web.UI.WebControls.TextBox txtDatum;
        public System.Web.UI.WebControls.TextBox txtLfdNummer;
        public System.Web.UI.WebControls.LinkButton lbCreate;
 
        public delegate void Cancel_Clicked();
        public event Cancel_Clicked CancelClicked;
        public delegate void Set_Clicked();
        public event Set_Clicked SetClicked;

        protected void Page_Load(object sender, EventArgs e)
        {
            
        }

          protected void lbCreate_Click(object sender, EventArgs e)
        {

            SetClicked();

        }

        public void SetVorschaden(System.Web.UI.Page SourcePage)
        {
            Vorschaden UseClass = (Vorschaden)Session["Vorschaden"];


            UseClass.FahrgestellnummerEdit = txtFin.Text;
            UseClass.KennzeichenEdit = txtKennzeichen.Text;
            UseClass.PreisEdit = txtPreis.Text;
            UseClass.SchadensdatumEdit = txtDatum.Text;
            UseClass.BeschreibungEdit = txtBeschreibung.Text;
            UseClass.LfdNummerEdit = txtLfdNummer.Text;

            UseClass.ChangeVorschaden((string)Session["AppID"], (string)Session.SessionID, SourcePage);

            if (UseClass.IsError == true)
            {
                lblError.Text = "Die Daten konnten nicht gespeichert werden. Bitten überprüfen Sie Ihre Eingaben.";
            }
            else
            {
                lblMessage.Text = "Ihre Daten wurden gespeichert.";
                txtBeschreibung.Enabled = false;
                txtDatum.Enabled = false;
                txtPreis.Enabled = false;
                lbCreate.Visible = false;
            }
        }

        protected void lbCancel_Click(object sender, EventArgs e)
        {

            txtBeschreibung.Enabled = true;
            txtDatum.Enabled = true;
            txtPreis.Enabled = true;
            lbCreate.Visible = true;

            CancelClicked();
           
        }


    }
}