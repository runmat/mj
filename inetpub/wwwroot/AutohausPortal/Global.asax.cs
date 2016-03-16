using System;
using System.Web;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
namespace AutohausPortal
{
    public class Global : HttpApplication
    {
        private long m_lngCurrentDate;

        void Application_Start(object sender, EventArgs e)
        {
            SqlConnection conn = new SqlConnection(ConfigurationManager.AppSettings["Connectionstring"].ToString());
            try
            {
                SqlCommand cmdUnlockUser = new SqlCommand("update LogWebAccess set endTime=@endTime, hostname='Restart' where endTime is null", conn);
                SqlCommand command = new SqlCommand("update WebUser set LoggedOn=0, LastChangedBy='Restart'", conn);

                conn.Open();

                cmdUnlockUser.Parameters.AddWithValue("@endTime", DateTime.Now);
                cmdUnlockUser.ExecuteNonQuery();
                cmdUnlockUser.Dispose();

                command.ExecuteNonQuery();
                command.Dispose();
            }
            catch (Exception)
            {

            }
            finally 
            { 
                conn.Close();
                conn.Dispose();            
            }
        }

        void Application_End(object sender, EventArgs e)
        {
            //  Code, der beim Herunterfahren der Anwendung ausgeführt wird.
        }

        void Application_Error(object sender, EventArgs e)
        {
            // Wird ausgelöst, wenn ein Fehler auftritt.
            Exception LastError = Server.GetLastError();

            GeneralTools.Services.LogService logService = new GeneralTools.Services.LogService("/AutohausPortal", String.Empty);
            logService.LogElmahError(LastError, null);
        }

        void Session_Start(object sender, EventArgs e)
        {
            SqlConnection conn = new SqlConnection(ConfigurationManager.AppSettings["Connectionstring"].ToString());
            long lNow = 0;
            String SDate = DateTime.Now.ToString("yyyyMMdd");
            long.TryParse(SDate, out lNow);
            if (m_lngCurrentDate != lNow)
            {   
                m_lngCurrentDate = lNow;
                try
                {
                    SqlCommand cmdlockUser = new SqlCommand("UPDATE WebUser SET AccountIsLockedOut=1, LastChangedBy='Inaktivität' WHERE AccountIsLockedOut=0 AND UserID IN (SELECT UserID From vwLastUserAccess2Lock)", conn);

                    conn.Open();

                    cmdlockUser.ExecuteNonQuery();
                    cmdlockUser.Dispose();
                }
                catch (Exception)
                {
                }
                finally 
                { 
                    if( conn.State != ConnectionState.Closed ){conn.Close();}
                    conn.Dispose();
                }
            }
        }

        void Session_End(object sender, EventArgs e)
        {
            // Code, der am Ende einer Sitzung ausgeführt wird. 
            // Hinweis: Das Session_End-Ereignis wird nur ausgelöst, wenn der sessionstate-Modus
            // in der Datei "Web.config" auf InProc festgelegt wird. Wenn der Sitzungsmodus auf StateServer festgelegt wird
            // oder auf SQLServer, wird das Ereignis nicht ausgelöst.

            try
            {
                //if (HttpContext.Current != null) 
                //{ 
                //    HttpContext.Current.Cache.Remove("myAppListView");
                //    HttpContext.Current.Cache.Remove("myAppParentView");
                //    HttpContext.Current.Cache.Remove("myColListView");
                //    HttpContext.Current.Cache.Remove("myCustomerListView");
                //    HttpContext.Current.Cache.Remove("myCustomerAppAssigned");
                //    HttpContext.Current.Cache.Remove("myGroupListView");
                //    HttpContext.Current.Cache.Remove("myOrganizationListView");
                //    HttpContext.Current.Cache.Remove("myAppAssigned");
                //    HttpContext.Current.Cache.Remove("myUserListView");
                //    HttpContext.Current.Cache.Remove("m_objTrace");           
                //}

                //Session.RemoveAll();

                //GC.Collect();
            }
            catch (Exception)
            {
                
            }
        }
    }
}
