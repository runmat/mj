using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CKG.Base.Kernel;
using CKG.Base.Kernel.Common;
using CKG.Base.Kernel.Security;
namespace AutohausPortal.Info
{
    public partial class _Error : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //if (Session["objUser"] !=null)
            //{
            //    m_User = (User)Session["objUser"];
            //}

            //m_User = Common.GetUser(this);



            try
            {
                String strID = "";

                if (Request.QueryString["ID"] != null)
                {
                    strID = Request.QueryString["ID"].ToString();
                }

                switch (strID)
                {
                    case "404":
                        lblErrorMessage.Text = "Die angeforderte Seite wurde nicht gefunden! (HTTP-Error 404)";
                        break;
                    case "403":
                        lblErrorMessage.Text = "Sie haben keinen Zugriff auf die angeforderte Seite oder das Verzeichnis! (HTTP-Error 403)";
                        break;
                    case "500":
                        lblErrorMessage.Text = "Ein interner Server-Fehler ist aufgetreten! (HTTP-Error 500)";
                        break;
                    default:
                        lblErrorMessage.Text = "Ein Fehler ist aufgetreten!" + "<br /><br />";
                        Exception ex = Server.GetLastError();
                        Exception exIn = ex;
                        while (exIn != null)
                        {
                            lblErrorMessage.Text = exIn.Message + "<br /><br />";
                            exIn = exIn.InnerException;
                        }
                        Response.Write("Ein interner Server-Fehler ist aufgetreten! (HTTP-Error 500)");
                        Server.ClearError();

                        break;
                }
            }
            catch (Exception)
            {


            }

        }
    }
}