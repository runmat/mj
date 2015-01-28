using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Web;
using System.Web.UI;
using AutohausPortal.lib;
using CKG.Base.Kernel.Security;

namespace AutohausPortal.Controls
{
    public partial class Startmenue : UserControl
    {
        private User m_User;

        public User User { get { return m_User; } }

        public DataView MenuChangeSource;
        public DataView MenuChangeAHSource;
        public DataView MenuReportSource;
        public DataView MenuToolsSource;

        protected void Page_Load(object sender, EventArgs e)
        {
            m_User = MVC.GetSessionUserObject();

            SqlConnection conn = new SqlConnection(ConfigurationManager.AppSettings["Connectionstring"]);
            conn.Open();
            try
            {
                SqlCommand command = new SqlCommand("SELECT AppType,DisplayName FROM ApplicationType ORDER BY Rank", conn);
                SqlDataAdapter da = new SqlDataAdapter(command);

                DataTable table = new DataTable();
                da.Fill(table);

                DataTable appTable = m_User.Applications.Copy();
                MVC.MvcPrepareDataRowsUrl(appTable, m_User.UserName);

                foreach (DataRow dRow in appTable.Rows)
                {
                    // PageVisit soll auf dem Server geloggt werden und nicht auf via JS
                    // Umschreiben als Aufruf an Log.aspx der dann einen Redirect zu dieser Adresse erstellt
                    var url = dRow["AppURL"].ToString();

                    // Url encoden für die Verwendung als Query Params
                    url = HttpUtility.UrlEncode(url);
                    url = Convert.ToBase64String(Encoding.UTF8.GetBytes(url.ToCharArray()));

                    // Jetzt besteht die neue url aus: appid, original url unverändert übernehmen
                    dRow["AppURL"] = String.Concat("../Start/Log.aspx?", "APP-ID=", dRow["AppID"], "&url=", url);
                }

                DataView dvAppLinks = new DataView(appTable);
                dvAppLinks.RowFilter = "AppType='Change' AND AppInMenu=1";
                MenuChangeSource = new DataView(appTable);
                MenuChangeSource.RowFilter = "AppType='Change' AND AppInMenu=1";

                MenuChangeAHSource = new DataView(appTable);
                MenuChangeAHSource.RowFilter = "AppType='ChangeAH' AND AppInMenu=1";

                MenuReportSource = new DataView(appTable);
                MenuReportSource.RowFilter = "AppType='Report' AND AppInMenu=1";

                MenuToolsSource = new DataView(appTable);
                MenuToolsSource.RowFilter = "AppType='Tools' AND AppInMenu=1";
            }
            catch (Exception)
            {
            }
            finally
            {
                conn.Close();
            }
        }
    }
}