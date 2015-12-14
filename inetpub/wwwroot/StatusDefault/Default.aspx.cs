using System;
using StatusDefault.lib;

namespace StatusDienste
{
    public partial class Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Interface connInterface = new Interface();

            var dbConnect = connInterface.CheckSQLConnection();
            if (dbConnect) 
            {
                lblStatusSQL.Text = "SQL-Connection: OK";
            }
            else
            {
                lblStatusSQL.Text = "SQL-Connection: CRITICAL";
                lblErrorSQL.Text = connInterface.ErrorSql;
            }
            
            var sapConnect = connInterface.CheckSAPConnection();
            if (sapConnect)
            {
                lblStatusSAP.Text = "SAP-Connection: OK";
            }
            else
            {
                lblStatusSAP.Text = "SAP-Connection: CRITICAL";
                lblErrorSAP.Text = connInterface.ErrorSap;
            }
            if (sapConnect && dbConnect)
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