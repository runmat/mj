using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using CKG.Base.Kernel.Common;
using CKG.Base.Kernel.Security;

namespace AppMBB.forms
{
    public partial class Report02s_2 : System.Web.UI.Page
    {

        private CKG.Base.Kernel.Security.User m_User;
        private CKG.Base.Kernel.Security.App m_App;
        private DataTable m_objTable;
        private string strKennzeichen;

        protected void Page_Load(object sender, EventArgs e)
        {
            m_User = Common.GetUser(this);

            try
            {
                string AppName = Request.Url.LocalPath;

                AppName = AppName.Replace(".aspx", "");


                AppName = "AppName = '" + AppName.Substring(AppName.LastIndexOf("/") + 1) + "'";



                if (m_User.Applications.Select(AppName).Length == 0)
                {
                    this.lblError.Visible = true;
                    
                }
                else
                {
                    if ((Request.QueryString["strKennzeichen"] == null))
                    {
                        this.lblError.Visible = true;
                        //Me.Response.Redirect("../../../Start/Selection.aspx")
                    }
                    else
                    {
                        strKennzeichen = Request.QueryString["strKennzeichen"];
                        if ((Session["Report"] == null))
                        {
                            this.lblError.Visible = true;
                            //Me.Response.Redirect("../../../Start/Selection.aspx")
                        }
                        else
                        {
                            m_objTable = (DataTable)Session["Report"];

                            m_App = new CKG.Base.Kernel.Security.App(m_User);

                            if (!IsPostBack)
                            {
                                DataRow[] rows = m_objTable.Select("[ZZKENN]='" + strKennzeichen + "'");
                                if (rows.Length == 1)
                                {
                                    if ((rows[0]["ZZKENN"] != null) && Convert.ToString(rows[0]["ZZKENN"]).Trim().Length > 0)
                                    {
                                        lblFahrzeugkennzeichen.Text = Convert.ToString(rows[0]["ZZKENN"]) + ".";
                                    }
                                }
                                else
                                {
                                    this.lblError.Visible = true;
                                }
                            }
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                this.lblError.Visible = true;
            }

        }


        private void Page_PreRender(object sender, System.EventArgs e)
        {
            Common.SetEndASPXAccess(this);
        }

        private void Page_Unload(object sender, System.EventArgs e)
        {
            Common.SetEndASPXAccess(this);
        }




    }
}