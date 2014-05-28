using System;
using System.Web.UI;
using AutohausPortal.lib;
using CKG.Base.Kernel.Security;

namespace AutohausPortal.forms
{   /// <summary>
    /// Druckdialog für Kundenformular (mit Bankdaten, hauptsächlich für Sepa) 
    /// Aufruf aus den Bearbeitungsseiten
    /// </summary>
    public partial class PrintDialogKundenformular : System.Web.UI.Page
    {
        private AHErfassung objVorerf;
        private User m_User;

        /// <summary>
        /// Öffnen der PDF-Datei
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void cmdPrint_Click(object sender, EventArgs e)
        {
            GetPdf();
        }

        /// <summary>
        /// Schließen des Dialogs
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void cmdCloseDialog_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Close",
                "<script type='text/javascript'>returnToParent();</script>", false);
        }

        private void GetPdf()
        {
            if (Session["PDFXString"] != null)
            {
                byte[] pdfBytes = (byte[])Session["PDFXString"];

                // Header bereinigen
                Response.Clear();
                Response.ClearContent();
                Response.ClearHeaders();
                Response.ContentType = "application/pdf";
                Response.AddHeader("Content-Disposition", "attachment; filename=" + "Formular.pdf");
                Response.AddHeader("Expires", "0");
                Response.AddHeader("Pragma", "cache");
                Response.AddHeader("Cache-Control", "private");
                Response.BinaryWrite(pdfBytes);
                if (Response.IsClientConnected)
                {
                    Response.Flush();
                }
                Response.End();
                Session["PDFXString"] = null;
            }
        }
    }
}