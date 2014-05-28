using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace AppRemarketing.forms
{
    public partial class Report11Formular : System.Web.UI.Page
    {
        protected void Page_Load(object sender, System.EventArgs e)
        {
            
            
            if ((Session["App_Filepath"] != null))
            {
                string sPfad = Session["App_Filepath"].ToString();
                Response.ContentType = "Application/pdf";
                //Get the physical path to the file.
                string FilePath = sPfad;
                //Write the file directly to the HTTP output stream.
                Response.WriteFile(FilePath);
                Response.End();
            }
        }

    }
}
