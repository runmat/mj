using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CKG.Base.Kernel.Common;
using CKG.Base.Business;
using CKG.Base.Kernel.Security;
using System.Configuration;

namespace Leasing.forms
{

    public partial class Report01Formular2 : System.Web.UI.Page
    {
        private CKG.Base.Kernel.Security.User m_User;
        private CKG.Base.Kernel.Security.App m_App;
        private DataTable m_objTable;
        private DataTable m_objAddressTable;
        private String strKennzeichen;

        protected void Page_Load(object sender, EventArgs e)
        {
            m_User = Common.GetUser(this);

            String AppName = Request.AppRelativeCurrentExecutionFilePath;
            if (AppName == "Selection" && m_User.Applications.Select(AppName).Length == 0)
            {
                lblError.Visible = true;
            }
            else if (Request.QueryString["strKennzeichen"] == null)
            {
                lblError.Visible = true;
            }
            else
            {
                strKennzeichen = Request.QueryString["strKennzeichen"].ToString();
                m_objTable = (DataTable)(Session["ResultTable"]);
                if (Session["ResultAddressTable"] != null)
                {
                    m_objAddressTable = (DataTable)(Session["ResultAddressTable"]);
                }
                m_App = new App(m_User);
            }
            if (!IsPostBack)
            {
                DataRow[] rows = m_objTable.Select("Kennzeichen='" + strKennzeichen + "'");
                if (rows.Length==1)
                {
                    if (rows[0]["Kennzeichen"] != null && rows[0]["Kennzeichen"].ToString().Trim().Length > 0)
                    {
                        lblFahrzeugkennzeichen.Text = rows[0]["Kennzeichen"].ToString().Trim();
                    }
                }
                else
                {
                    lblError.Visible = true;
                }

                if ((m_objAddressTable != null) && (m_objAddressTable.Rows.Count > 0))
                {
                    DataRow dRow = m_objAddressTable.Rows[0];

                    lblName1.Text = dRow["NAME1"].ToString();
                    lblStrasse.Text = dRow["STREET"].ToString() + " " + dRow["HOUSE_NUM1"].ToString();
                    lblPlzOrt.Text = dRow["POST_CODE1"].ToString() + " " + dRow["CITY1"].ToString();
                }
            }
        }
    }
}
