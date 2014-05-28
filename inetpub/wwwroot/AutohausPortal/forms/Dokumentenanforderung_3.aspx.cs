using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CKG.Base.Kernel;
using CKG.Base.Kernel.Common;
using CKG.Base.Kernel.Security;
using AutohausPortal.lib;
using System.Data;

namespace AutohausPortal.forms
{
    /// <summary>
    /// Anzeige Vollmachten/Einzugsermächtigung der Bundesländer.
    /// </summary>
    public partial class Dokumentenanforderung_3 : System.Web.UI.Page
    {
        private User m_User;
        private App m_App;

        /// <summary>
        /// Page_Load Ereignis.
        /// Überprüfung ob dem User diese Applikation zugeordnet ist.
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            m_User = Common.GetUser(this);
            Common.FormAuth(this, m_User);
            m_App = new App(m_User); //erzeugt ein App_objekt 
            Common.GetAppIDFromQueryString(this);

            String strAppID = String.Empty;
            if (Request.QueryString["AppID"].Length > 0)
            {
                strAppID = Request.QueryString["AppID"].ToString();
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
                    dRow["Bundesland"] = fInfo[i].Name.Substring(0, fInfo[i].Name.IndexOf(".")).ToString();
                    dRow["Pfad"] = path + "\\" + fInfo[i].Name;
                    LinkTable.Rows.Add(dRow);
                }

            }
            Repeater1.DataSource = LinkTable;
            Repeater1.DataBind();
        }
    }
}