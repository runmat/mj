using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace AppZulassungsdienst.forms
{
    /// <summary>
    /// Senden von Dateien an den Client
    /// </summary>
    public partial class DownloadFile : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["App_Filepath"] != null)
            {
                string sPfad = Session["App_Filepath"].ToString();

                if ((Session["App_ContentDisposition"] != null) && (!String.IsNullOrEmpty(Session["App_ContentDisposition"].ToString())))
                {
                    string strContentDisposition = Session["App_ContentDisposition"].ToString();
                    if (sPfad.Contains("\\"))
                    {
                        Response.AddHeader("Content-Disposition", strContentDisposition + "; filename=" + sPfad.Substring(sPfad.LastIndexOf('\\') + 1, sPfad.Length - (sPfad.LastIndexOf('\\') + 1)));
                    }
                    else
                    {
                        Response.AddHeader("Content-Disposition", strContentDisposition + "; filename=" + sPfad);
                    }
                }

                Response.ContentType = Session["App_ContentType"].ToString();
                // Get the physical path to the file.
                string FilePath = sPfad;
                // Write the file directly to the HTTP output stream.
                Response.WriteFile(FilePath);
                Response.End();
            }
        }
    }
}