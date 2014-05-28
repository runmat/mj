using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using AutohausPortal.lib;

namespace AutohausPortal.Controls
{
    public partial class BemerkungenNotizen : System.Web.UI.UserControl
    {
        #region Properties

        public string Bemerkung { get { return txtBemerk.Text; } }
        public string Notiz { get { return txtNotiz.Text; } }
        public string VKKurz { get { return txtVKkurz.Text; } }
        public string KunRef { get { return txtKunRef.Text; } }

        #endregion

        #region Events

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        #endregion

        #region Methods

        public void ResetFields()
        {
            txtBemerk.Text = "";
            txtNotiz.Text = "";
            txtVKkurz.Text = "";
            txtKunRef.Text = "Kundeninterne Referenz";
            addAttributesKunRef();
        }

        public void SelectValues(AHErfassung objVorerf)
        {
            txtBemerk.Text = objVorerf.Bemerkung;
            if (objVorerf.InternRef == String.Empty) { addAttributes(txtKunRef); } else { txtKunRef.Text = objVorerf.InternRef; }
            txtVKkurz.Text = objVorerf.VkKurz;
            txtNotiz.Text = objVorerf.Notiz;
        }

        public void addAttributesKunRef()
        {
            addAttributes(txtKunRef);
        }

        public void proofInserted()
        {
            if (txtBemerk.Text != "") { disableDefaultValue(txtBemerk); }
            if (txtVKkurz.Text != "") { disableDefaultValue(txtVKkurz); }
            if (txtNotiz.Text != "") { disableDefaultValue(txtNotiz); }
            if (txtKunRef.Text != "Kundeninterne Referenz") { disableDefaultValue(txtKunRef); }
        }

        /// <summary>
        /// Fügt Javascript-Funktionen einer Textbox hinzu
        /// </summary>
        /// <param name="txtBox">Control</param>
        private void addAttributes(TextBox txtBox)
        {
            txtBox.Attributes.Add("onblur", "if(this.value=='')this.value=this.defaultValue");
            txtBox.Attributes.Add("onfocus", "if(this.value==this.defaultValue)this.value=''");
        }

        /// <summary>
        /// entfernt den Vorschlagswert der Textbox, wenn Eingabe erfolgte
        /// </summary>
        /// <param name="txtBox">Control</param>
        private void disableDefaultValue(TextBox txtBox)
        {
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), txtBox.ClientID,
                "<script type='text/javascript'>disableDefaultValue('" + txtBox.ClientID + "');</script>", false);
        }

        #endregion
    }
}