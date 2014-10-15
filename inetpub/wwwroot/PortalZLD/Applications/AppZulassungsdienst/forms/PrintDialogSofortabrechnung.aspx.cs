using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Web.UI;
using System.Web.UI.WebControls;
using AppZulassungsdienst.lib;
using CKG.Base.Kernel.Common;
using CKG.Base.Kernel.Security;
using SmartSoft.PdfLibrary;

namespace AppZulassungsdienst.forms
{
    public partial class PrintDialogSofortabrechnung : Page
    {
        private User m_User;
        private App m_App;
        private NacherfZLD objNacherf;

        protected void Page_Load(object sender, EventArgs e)
        {
            //m_User = Common.GetUser(this);
            //Common.FormAuth(this, m_User, "");
            //m_App = new App(m_User);
            //Common.GetAppIDFromQueryString(this);
            //lblHead.Text = (string)m_User.Applications.Select("AppID = '" + Session["AppID"] + "'")[0]["AppFriendlyName"];

            //if (Session["objNacherf"] == null)
            //{
            //    //Session-Variable weg (Session vermutlich abgelaufen) -> zurück zum Hauptmenü
            //    Response.Redirect("/PortalZLD/Start/Selection.aspx?AppID=" + Session["AppID"].ToString());
            //}

            //objNacherf = (NacherfZLD)Session["objNacherf"];

            //if (!IsPostBack)
            //{
            //    if (objVorerf.tblPrint.Rows.Count > 0)
            //    {
            //        DataTable showTable = new DataTable();
            //        showTable.Columns.Add("ZULBELN", typeof(String));
            //        showTable.Columns.Add("FILENAME", typeof(String));
            //        showTable.Columns.Add("Path", typeof(String));

            //        if (objVorerf.tblPrint.Rows.Count > 0)
            //        {
            //            // Pfad zu den PDF´s aufbauen
            //            String NetworkPath = "";
            //            if (m_User.IsTestUser)
            //            {
            //                NetworkPath = "\\\\192.168.10.96\\test\\portal\\zld\\ah_auftrag\\";
            //            }
            //            else
            //            {
            //                NetworkPath = "\\\\192.168.10.96\\prod\\portal\\zld\\ah_auftrag\\";
            //            }

            //            string[] files = null;
            //            List<byte[]> filesByte = new List<byte[]>();
            //            String FolderName = objVorerf.tblPrint.Rows[0]["FILENAME"].ToString().Split('/')[1].ToString();
            //            if (Directory.Exists(NetworkPath + FolderName))
            //            {
            //                files = Directory.GetFiles(NetworkPath + FolderName + "\\", "*.pdf");
            //                foreach (string sFile in files)
            //                {
            //                    filesByte.Add(File.ReadAllBytes(sFile));
            //                }

            //                string sPath = null;

            //                sPath = NetworkPath + FolderName + "\\Auftragsliste_" + FolderName + ".pdf";
            //                // Mergen der einzelnen PDF´s in ein großes PDF
            //                File.WriteAllBytes(sPath, PdfMerger.MergeFiles(filesByte, true));
            //                DataRow PrintRow = showTable.NewRow();
            //                PrintRow["ZULBELN"] = "1";
            //                PrintRow["FILENAME"] = "Einzelaufträge_" + FolderName + ".pdf";
            //                PrintRow["Path"] = sPath;
            //                showTable.Rows.Add(PrintRow);

            //                // Tagesliste separat anbieten
            //                sPath = NetworkPath + FolderName + "\\Auftragsliste\\Auftragsliste.pdf";
            //                PrintRow = showTable.NewRow();
            //                PrintRow["ZULBELN"] = "2";
            //                PrintRow["FILENAME"] = "Gesamtliste_" + FolderName + ".pdf";
            //                PrintRow["Path"] = sPath;
            //                showTable.Rows.Add(PrintRow);
            //            }
            //        }

            //        GridView2.DataSource = showTable;
            //        GridView2.DataBind();
            //    }
            //}
        }

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
                Response.Clear();
                Response.ClearContent();
                Response.ClearHeaders();
                Response.ContentType = Session["App_ContentType"].ToString();
                Session["App_Filepath"] = null;
                Session["App_ContentType"] = null;

                String fname = sPfad.Substring(sPfad.LastIndexOf("\\") + 1);

                // Datei direkt an Client senden, nicht im Browser anzeigen (führte teilweise zu Problemen)
                Response.AppendHeader("Content-Disposition", "attachment; filename=" + fname);
                Response.TransmitFile(sPfad);
                Response.End();
            }
        }

        protected void cmdCloseDialog_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Close",
            "<script type='text/javascript'>returnToParent();</script>", false);
        }
    }
}