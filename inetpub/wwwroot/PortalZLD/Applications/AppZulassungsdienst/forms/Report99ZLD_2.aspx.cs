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
        private User m_User;

        #region Events

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
            Common.GetAppIDFromQueryString(this);
            lblHead.Text = (string)m_User.Applications.Select("AppID = '" + Session["AppID"] + "'")[0]["AppFriendlyName"];

            if (!String.IsNullOrEmpty(Request.QueryString["AppID"]))
            {
                Session["AppID"] = Request.QueryString["AppID"];
                fillTable();
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Bindet Repeater1 mit den aus dem Ordner "Docs\\Lastschrift" beinhaltenen Dokumente(LinkTable).
        /// </summary>
        private void fillTable()
        {
            DataTable LinkTable = new DataTable();
            LinkTable.Columns.Add("Bundesland", typeof(String));
            LinkTable.Columns.Add("Pfad", typeof(String));

            String path = Request.PhysicalApplicationPath + "Docs\\Lastschrift";
            System.IO.DirectoryInfo dirInfo = new System.IO.DirectoryInfo(path);
            System.IO.FileInfo[] fInfo = dirInfo.GetFiles("*.*");

            if (fInfo.Length > 0)
            {
                for (int i = 0; i < fInfo.Length; i++)
                {
                    DataRow dRow = LinkTable.NewRow();
                    dRow["Bundesland"] = fInfo[i].Name.Substring(0, fInfo[i].Name.IndexOf("."));
                    dRow["Pfad"] = path + "\\" + fInfo[i].Name;
                    LinkTable.Rows.Add(dRow);
                }
            }
            Repeater1.DataSource = LinkTable;
            Repeater1.DataBind();
        }

        #endregion
    }
}
