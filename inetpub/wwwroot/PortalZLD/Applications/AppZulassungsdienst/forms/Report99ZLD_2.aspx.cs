using System;
using CKG.Base.Kernel.Common;
using CKG.Base.Kernel.Security;
using System.Data;

namespace AppZulassungsdienst.forms
{
    /// <summary>
    /// Anzeige Vollmachten/Einzugsermächtigung der Bundesländer. Dokumentenanforderung der Zulassungsstellen.
    /// </summary>
    public partial class Report99ZLD_2 : System.Web.UI.Page
    {
        private CKG.Base.Kernel.Security.User m_User;
        private CKG.Base.Kernel.Security.App m_App;
        /// <summary>
        /// Page_Load Ereignis.
        /// Überprüfung ob dem User diese Applikation zugeordnet ist.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            m_User = Common.GetUser(this);
            Common.FormAuth(this, m_User);
            m_App = new App(m_User); //erzeugt ein App_objekt 
            Common.GetAppIDFromQueryString(this);
            lblHead.Text = (string)m_User.Applications.Select("AppID = '" + Session["AppID"] + "'")[0]["AppFriendlyName"];

            String strAppID = String.Empty;
            if (Request.QueryString["AppID"].Length > 0)
            {
                strAppID = Request.QueryString["AppID"];
                Session["AppID"] = strAppID;
                fillTable();
            }
            
        }
        /// <summary>
        /// Bindet Repeater1 mit den aus dem Ordner "Docs\\Lastschrift" beinhaltenen Dokumente(LinkTable).
        /// </summary>
        private void fillTable() 
        {
            System.IO.DirectoryInfo dirInfo;
            System.IO.FileInfo[] fInfo;
            String trString;
            String path;
            DataTable LinkTable = new DataTable(); 
            LinkTable.Columns.Add("Bundesland", typeof(String));
            LinkTable.Columns.Add("Pfad", typeof(String));

            path = Request.PhysicalApplicationPath + "Docs\\Lastschrift";
            dirInfo = new System.IO.DirectoryInfo(path);
            fInfo = dirInfo.GetFiles("*.*");
            trString = String.Empty;

            if (fInfo.Length > 0)
	            {
                    for (int i = 0; i < fInfo.Length; i++)
			        { 
                        DataRow dRow = LinkTable.NewRow();
                        dRow["Bundesland"] = fInfo[i].Name.Substring(0,fInfo[i].Name.IndexOf("."));
                        dRow["Pfad"] = path + "\\" + fInfo[i].Name;
                        LinkTable.Rows.Add(dRow);
			        }
            		 
	            }
            Repeater1.DataSource = LinkTable;
            Repeater1.DataBind();
        }
    }
}
