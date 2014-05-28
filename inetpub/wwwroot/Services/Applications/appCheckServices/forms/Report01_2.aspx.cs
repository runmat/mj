using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using CKG.Base.Kernel.Common;
using CKG.Base.Business;
using CKG.Base.Kernel;


namespace appCheckServices.forms
{
    public partial class Report01_2 : System.Web.UI.Page
    {
        private CKG.Base.Kernel.Security.User m_User;

        protected void Page_Load(object sender, EventArgs e)
        {
            m_User = Common.GetUser(this);

            if ((Session["App_Filepath"] != null))
            {
                String sPfad = (String)Session["App_Filepath"];
                Response.ContentType = (String)Session["App_ContentType"];
                String sName = sPfad.Substring(sPfad.LastIndexOf(@"\") + 1);

                Response.AppendHeader("content-disposition", "attachment; filename=" + sName);
                //Get the physical path to the file. 
                String FilePath = sPfad;
                //Write the file directly to the HTTP output stream. 
                Response.WriteFile(FilePath);
                Response.End();
            } 

        }
    }
}
