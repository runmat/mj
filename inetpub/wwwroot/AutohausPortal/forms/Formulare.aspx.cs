using System;
using CKG.Base.Kernel.Common;
using CKG.Base.Kernel.Security;
using AutohausPortal.lib;
using System.Data;
using System.Configuration;
using System.IO;
namespace AutohausPortal.forms
{
    /// <summary>
    /// Download von Formularen(Einzugsermächtigung Kfz-Steuer,Zulassungsvollmacht) 
    /// </summary>
    public partial class Formulare : System.Web.UI.Page
    {
        private User m_User;
        private App m_App;
        private DataTable showTable;

        /// <summary>
        /// Page_Load- Ereignis. Überprüfung ob dem User diese Applikation zugeordnet ist.
        /// Aufruf von FillRepeater().
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            m_User = Common.GetUser(this);
            Common.FormAuth(this, m_User);
            m_App = new App(m_User);
            Common.GetAppIDFromQueryString(this);

            FillRepeater();


        }
        /// <summary>
        /// Dokumente sammeln, aufbereiten einer Tabelle und an den Repaeter binden.
        /// </summary>
        private void FillRepeater()
        {

             String[] sfolder ;
             String NetworkPath =  ConfigurationManager.AppSettings["DownloadPathSamba"].ToString();

            NetworkPath =  NetworkPath + "\\" +  m_User.Groups[0].GroupName + "\\";

            if (Directory.Exists(NetworkPath))
            {
                sfolder = System.IO.Directory.GetDirectories(NetworkPath);
            }
            else 
            {
                lblError.Text = "Es stehen keine Formulare zum Download bereit!";
                return;
            }
                showTable = new DataTable();
                showTable.Columns.Add("Serverpfad", typeof(String));
                showTable.Columns.Add("Filename", typeof(String));


                String[] files = Directory.GetFiles(NetworkPath + "\\", "*.pdf");
                if (files.Length > 0) 
                { 
                    foreach (string sFile in files)
                    {
                    
                        DataRow PrintRow = showTable.NewRow();
                        PrintRow["Serverpfad"] = sFile;
                        String filename = Path.GetFileNameWithoutExtension(sFile);
                        PrintRow["Filename"] = filename;
                        showTable.Rows.Add(PrintRow);                          
                    }
                
                    Repeater1.DataSource = showTable;
                    Repeater1.DataBind();
                }
                else
                {
                    lblError.Text = "Es stehen keine Formulare zum Download bereit!";
                    return;
                }
        }
        /// <summary>
        /// Weiterleiten des Dateipfades der PDF an Printpdf.aspx(FileStream als PDF laden)
        /// </summary>
        /// <param name="source">object</param>
        /// <param name="e">RepeaterCommandEventArgs</param>
        protected void Repeater1_ItemCommand(object source, System.Web.UI.WebControls.RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "show") 
            {
                Session["App_ContentType"] = "Application/pdf";
                Session["App_Filepath"] = e.CommandArgument;
                ResponseHelper.Redirect("Printpdf.aspx", "_blank", "left=0,top=0,resizable=YES,scrollbars=YES");
            }
        }
    }
}