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
    public partial class Report02s_1 : System.Web.UI.Page
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
                        
                    }
                    else
                    {
                        strKennzeichen = Request.QueryString["strKennzeichen"];
                        if ((Session["Report"] == null))
                        {
                            this.lblError.Visible = true;
                           
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
                                        lblFahrzeugkennzeichen.Text = Convert.ToString(rows[0]["ZZKENN"]);
                                    }
                                    if ((rows[0]["EARTX"] != null) && Convert.ToString(rows[0]["EARTX"]).Trim().Length > 0)
                                    {
                                        lblFahrzeugUndAufbauart.Text = Convert.ToString(rows[0]["EARTX"]);
                                    }
                                    if ((rows[0]["ZKLTXT"] != null) && Convert.ToString(rows[0]["ZKLTXT"]).Trim().Length > 0)
                                    {
                                        lblHersteller.Text = Convert.ToString(rows[0]["ZKLTXT"]);
                                    }
                                    if ((rows[0]["ZTYP"] != null) && Convert.ToString(rows[0]["ZTYP"]).Trim().Length > 0)
                                    {
                                        lblTypUndAusfuehrung.Text = "Typ Schlüssel: " + Convert.ToString(rows[0]["ZTYP"]);
                                    }
                                    if ((rows[0]["ZAUSF"] != null) && Convert.ToString(rows[0]["ZAUSF"]).Trim().Length > 0)
                                    {
                                        if (lblTypUndAusfuehrung.Text.Substring(1) == "T")
                                        {
                                            lblTypUndAusfuehrung.Text += ", Ausführung: " + Convert.ToString(rows[0]["ZAUSF"]);
                                        }
                                        else
                                        {
                                            lblTypUndAusfuehrung.Text = "Ausführung: " + Convert.ToString(rows[0]["ZAUSF"]);
                                        }
                                    }
                                    if ((rows[0]["CHASSIS_NUM"] != null) && Convert.ToString(rows[0]["CHASSIS_NUM"]).Trim().Length > 0)
                                    {
                                        lblFIN.Text = Convert.ToString(rows[0]["CHASSIS_NUM"]);
                                    }
                                    if ((rows[0]["NAME1"] != null) && Convert.ToString(rows[0]["NAME1"]).Trim().Length > 0)
                                    {
                                        lblName.Text = Convert.ToString(rows[0]["NAME1"]);
                                    }
                                    if ((rows[0]["PSTLZ"] != null) && Convert.ToString(rows[0]["PSTLZ"]).Trim().Length > 0)
                                    {
                                        lblWohnhaft.Text = Convert.ToString(rows[0]["PSTLZ"]);
                                    }
                                    if ((rows[0]["ORT01"] != null) && Convert.ToString(rows[0]["ORT01"]).Trim().Length > 0)
                                    {
                                        lblWohnhaft.Text += " " + Convert.ToString(rows[0]["ORT01"]);
                                    }
                                    if ((rows[0]["STRAS"] != null) && Convert.ToString(rows[0]["STRAS"]).Trim().Length > 0)
                                    {
                                        lblWohnhaft.Text += ", " + Convert.ToString(rows[0]["STRAS"]);
                                    }
                                    
                                    if ((rows[0]["SCHILD"].ToString().Trim() == "V"))
                                    {
                                        CheckBox1.Checked = false;
                                    }
                                    if ((rows[0]["SCHILD"].ToString().Trim() == "H"))
                                    {
                                        CheckBox2.Checked = false;
                                    }
                                    //---------------------------------------------------
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