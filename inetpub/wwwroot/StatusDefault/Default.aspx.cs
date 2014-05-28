using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using StatusDefault.lib;

namespace StatusDefault
{
    public partial class _Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            bool DbConnect = false;
            bool SAPConnect = false;
            Interface ConnInterface = new Interface();

            DbConnect=ConnInterface.CheckSQLConnection();
            if (DbConnect  == true) 
            {
                lblStatusSQL.Text = "SQL-Connection: OK";
            }
            else
            {
                lblStatusSQL.Text = "SQL-Connection: CRITICAL";
                lblErrorSQL.Text = ConnInterface.ErrorSql;
            }
            
            SAPConnect =ConnInterface.CheckSAPConnection() ;
            if (SAPConnect == true)
            {
                lblStatusSAP.Text = "SAP-Connection: OK";
            }
            else
            {
                lblStatusSAP.Text = "SAP-Connection: CRITICAL";
                lblErrorSAP.Text = ConnInterface.ErrorSap;
            }
            if (SAPConnect == true && DbConnect == true)
            {
                lblStatus.Text = "Status Applikation: OK";

            }
            else
            {
                lblStatus.Text = "Status Applikation: CRITICAL";
            }

        }
    }
}