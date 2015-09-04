using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Web.UI;
using System.Web.UI.WebControls;
using AutohausPortal.lib;
using CKG.Base.Kernel.Common;
using CKG.Base.Kernel.Security;
using SmartSoft.PdfLibrary;

namespace AutohausPortal.forms
{
    /// <summary>
    /// Druckdialog für Kundenformulare
    /// </summary>
    public partial class PrintDialogKundenformulare : Page
    {
        private AHErfassung objVorerf;
        private User m_User;

        /// <summary>
        /// Page_Load Ereignis
        /// Überprüfung ob dem User diese Applikation zugeordnet ist.
        /// Überprüft ob in der Tabelle objVorerf.tblPrint Vorgänge zum Drucken vorhanden sind. 
        /// Wenn ja dann Pfad und Dateinamen zusammen bauen und neu erstellete Tabelle an das
        /// Gridview binden.
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            m_User = Common.GetUser(this);

            objVorerf = (AHErfassung)Session["objVorerf"];

            if (!IsPostBack)
            {
                DataTable showTable = new DataTable();
                showTable.Columns.Add("ZULBELN", typeof(String));
                showTable.Columns.Add("FILENAME", typeof(String));
                showTable.Columns.Add("Path", typeof(String));

                if (objVorerf != null)
                {
                    var pdfId = 1;

                    // CPD-Endkundenformular
                    if (objVorerf.KundenformularPDF != null && objVorerf.KundenformularPDF.Length > 0)
                    {
                        DataRow PrintRow = showTable.NewRow();
                        PrintRow["ZULBELN"] = pdfId;
                        PrintRow["FILENAME"] = "Endkundenformular.pdf";
                        PrintRow["Path"] = "PDFXString";
                        showTable.Rows.Add(PrintRow);

                        pdfId++;
                    }

                    // Zusatzformulare
                    if (objVorerf.tblPrintKundenformulare != null && objVorerf.tblPrintKundenformulare.Rows.Count > 0)
                    {
                        // Pfad zu den PDF´s aufbauen
                        String NetworkPath;
                        if (m_User.IsTestUser)
                        {
                            NetworkPath = "\\\\192.168.10.96\\test\\portal\\zld\\ah_auftrag\\";
                        }
                        else
                        {
                            NetworkPath = "\\\\192.168.10.96\\prod\\portal\\zld\\ah_auftrag\\";
                        }

                        List<byte[]> filesByte = new List<byte[]>();
                        String FolderName = objVorerf.tblPrintKundenformulare.Rows[0]["FILENAME"].ToString().Split('/')[1].ToString();
                        if (Directory.Exists(NetworkPath + FolderName))
                        {
                            string[] files = Directory.GetFiles(NetworkPath + FolderName + "\\", "*.pdf");
                            foreach (string sFile in files)
                            {
                                filesByte.Add(File.ReadAllBytes(sFile));
                            }

                            string sPath = NetworkPath + FolderName + "\\Zusatzformulare_" + FolderName + ".pdf";
                            // Mergen der einzelnen PDF´s in ein großes PDF
                            File.WriteAllBytes(sPath, PdfMerger.MergeFiles(filesByte, true));
                            DataRow PrintRow = showTable.NewRow();
                            PrintRow["ZULBELN"] = pdfId;
                            PrintRow["FILENAME"] = "Zusatzformulare.pdf";
                            PrintRow["Path"] = sPath;
                            showTable.Rows.Add(PrintRow);
                        }
                    }
                }

                GridView2.DataSource = showTable;
                GridView2.DataBind();
            }
        }

        /// <summary>
        /// Dokument drucken
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">GridViewCommandEventArgs</param>
        protected void GridView2_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Print")
            {
                Session["App_ContentType"] = "Application/pdf";
                Session["App_Filepath"] = e.CommandArgument;

                GetPdf();
            }
        }

        private void GetPdf()
        {
            if (Session["App_Filepath"] != null)
            {
                String sPfad = Session["App_Filepath"].ToString();

                // Header bereinigen
                Response.Clear();
                Response.ClearContent();
                Response.ClearHeaders();
                Response.ContentType = Session["App_ContentType"].ToString();
                Session["App_Filepath"] = null;
                Session["App_ContentType"] = null;

                // Datei direkt an Client senden, nicht im Browser anzeigen (führte teilweise zu Problemen)
                if (sPfad == "PDFXString")
                {
                    Response.AppendHeader("Content-Disposition", "attachment; filename=" + "Formular.pdf");
                    Response.BinaryWrite(objVorerf.KundenformularPDF);
                    if (Response.IsClientConnected)
                        Response.Flush();
                }
                else
                {
                    String fname = sPfad.Substring(sPfad.LastIndexOf("\\") + 1);
                    Response.AppendHeader("Content-Disposition", "attachment; filename=" + fname);
                    Response.TransmitFile(sPfad);
                }
                Response.End();
            }
        }

        /// <summary>
        /// Schliessen des Dialogs und zurück zu Auftraege.aspx()
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void cmdCloseDialog_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Close",
            "<script type='text/javascript'>returnToParent();</script>", false);
        }
    }
}