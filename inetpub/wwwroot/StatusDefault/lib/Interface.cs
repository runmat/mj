using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using System.Data;
using ERPConnect;
namespace StatusDefault.lib
{
    public class Interface
    {

        public String ErrorSql
        {
            get;
            set;
        }

        public String ErrorSap
        {
            get;
            set;
        }

       public bool CheckSQLConnection() 
        {
            ErrorSql = "";
            System.Data.SqlClient.SqlConnection connection = new System.Data.SqlClient.SqlConnection();
            try
            {
                connection.ConnectionString = ConfigurationManager.ConnectionStrings["Connectionstring"].ToString();
		        connection.Open();
		        System.Data.SqlClient.SqlCommand command = new System.Data.SqlClient.SqlCommand();

                String str = "Select *from Customer where CustomerID = 1";
		        command = new System.Data.SqlClient.SqlCommand();
		        command.Connection = connection;
		        command.CommandType = CommandType.Text;
		        command.CommandText = str;
		        command.ExecuteNonQuery();
                return true;
	        }
	        catch (Exception ex)
	        {
                ErrorSql = ex.Message;
                return false;
				
	        }
	        finally { connection.Close(); 
            }
        }
         public bool CheckSAPConnection() 
         {

            ErrorSap = "";
            String SAPAppServerHost = ConfigurationManager.AppSettings["SAPAppServerHost"].ToString();
            String SAPSystemNumber = ConfigurationManager.AppSettings["SAPSystemNumber"].ToString();
            String SAPUsername= ConfigurationManager.AppSettings["SAPUsername"].ToString();
            String SAPPassword = ConfigurationManager.AppSettings["SAPPassword"].ToString();
            String SAPClient = ConfigurationManager.AppSettings["SAPClient"].ToString();

            
            try 
                {
                    int SystemNumber;

                    int.TryParse(SAPSystemNumber, out SystemNumber);

                    R3Connection conSAP = new R3Connection(SAPAppServerHost, SystemNumber, SAPUsername,SAPPassword, "DE", SAPClient);
                    ERPConnect.LIC.SetLic(ConfigurationManager.AppSettings["ErpConnectLicense"].ToString());
                    conSAP.Open(false);
                    conSAP.Close();
                    conSAP.Dispose();
                    return true;
                }
            catch (Exception ex)
                {
                    ErrorSap = ex.Message;
                    return false;
                }
            finally
                {

                }
         }
    }
}