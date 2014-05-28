using System;
using System.Configuration;
using System.Data;
using System.IO;
using System.Web.UI.WebControls;
using CKG.Base.Business;
using CKG.Base.Kernel.Common;
using CKG.Base.Kernel.Security;

namespace AppZulassungsdienst.forms
{
    /// <summary>
    /// Infocenter (Formulare)
    /// </summary>
    public partial class Infocenter : System.Web.UI.Page
    {
        private User m_User;
        private App m_App;
        private InfoCenterData icDocs;
        private string fileSourcePath;

        protected void Page_Load(object sender, EventArgs e)
        {
            m_User = Common.GetUser(this);
            Common.FormAuth(this, m_User);
            m_App = new App(m_User);
            Common.GetAppIDFromQueryString(this);

            lblError.Text = "";
            lblHead.Text = (string)m_User.Applications.Select("AppID = '" + Session["AppID"] + "'")[0]["AppFriendlyName"];

            fileSourcePath = ConfigurationManager.AppSettings["DownloadPathInfocenter"] + m_User.KUNNR + "\\";
            Literal1.Text = "";

            if (!IsPostBack)
            {
                FillDocumentTypes();
                FillDocuments();
            }
            else if (Session["objInfoCenter"] != null)
            {
                icDocs = (InfoCenterData)Session["objInfoCenter"];
            }
        }

        private void FillDocumentTypes()
        {
            try
            {
                if (icDocs == null)
                {
                    string strFileName = DateTime.Now.ToString("yyyyMMdd_HHmmss_") + m_User.UserName + ".xls";
                    icDocs = new InfoCenterData(ref m_User, ref m_App, Session["AppID"].ToString(), Session.SessionID, strFileName);
                }

                icDocs.GetDocumentTypes();
                Session["objInfoCenter"] = icDocs;
            }
            catch (Exception ex)
            {
                data.Visible = false;
                lblError.Text = "Fehler beim Lesen der Dokumenttypen: " + ex.Message;
            }
        }

        private void FillDocuments()
        {
            try
            {
                if (icDocs == null)
                {
                    string strFileName = DateTime.Now.ToString("yyyyMMdd_HHmmss_") + m_User.UserName + ".xls";
                    icDocs = new InfoCenterData(ref m_User, ref m_App, Session["AppID"].ToString(), Session.SessionID, strFileName);
                }

                icDocs.GetDocuments(m_User.GroupID);

                Session["objInfoCenter"] = icDocs;
                // Befüllen des Grids läuft über das "NeedDataSource"-Event

                if ((icDocs.Documents != null) && (icDocs.Documents.Rows.Count > 0))
                {
                    rgDokumente.Rebind();
                }
                else
                {
                    lblError.Text = "Keine Dokumente zur Anzeige gefunden.";
                }
            }
            catch (Exception ex)
            {
                data.Visible = false;
                lblError.Text = "Fehler beim Lesen der Dokumente: " + ex.Message;
            }
        }

        protected void lb_zurueck_Click(object sender, EventArgs e)
        {
            if (Session["objInfoCenter"] != null)
            {
                Session.Remove("objInfoCenter");
            }
            Response.Redirect("/PortalZLD/Start/Selection.aspx?AppID=" + Session["AppID"]);
        }

        #region Grid

        protected void rgDokumente_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            if (icDocs != null)
            {
                rgDokumente.DataSource = icDocs.Documents.DefaultView;
            }
        }

        protected void rgDokumente_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
        {
            if (e.Item is Telerik.Web.UI.GridGroupHeaderItem)
            {
                if ((icDocs != null) && (icDocs.Documents != null))
                {
                    Telerik.Web.UI.GridGroupHeaderItem item = (Telerik.Web.UI.GridGroupHeaderItem)e.Item;
                    string strText = item.DataCell.Text.Split(':')[1];

                    int tmpInt = 0;
                    if (Int32.TryParse(strText, out tmpInt))
                    {
                        DataRow docType = icDocs.DocumentTypes.Select("documentTypeId=" + tmpInt)[0];
                        item.DataCell.Text = docType["docTypeName"].ToString();
                    }
                }
            }
        }

        protected void rgDokumente_ItemCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
        {
            int index;
            Telerik.Web.UI.GridDataItem gridRow;
            LinkButton lButton;

            switch (e.CommandName)
            {
                case "showDocument":
                    index = e.Item.ItemIndex;
                    gridRow = rgDokumente.Items[index];
                    lButton = (LinkButton)e.Item.FindControl("lbtDateiOeffnen");

                    if (lButton != null)
                    {
                        string fName = lButton.Text;
                        string fType = gridRow["FileType"].Text;
                        string fExtension = "." + fType;
                        string sPfad = fileSourcePath + fName + "." + fType;

                        if (File.Exists(sPfad))
                        {
                            Session["App_Filepath"] = sPfad;

                            switch (gridRow["FileType"].Text)
                            {
                                case "pdf":
                                    Session["App_ContentType"] = "Application/pdf";
                                    Session["App_ContentDisposition"] = "inline";
                                    break;
                                case "xls":
                                case "xlsx":
                                    Session["App_ContentType"] = "Application/vnd.ms-excel";
                                    Session["App_ContentDisposition"] = "attachment";
                                    break;
                                case "doc":
                                case "docx":
                                    Session["App_ContentType"] = "Application/msword";
                                    Session["App_ContentDisposition"] = "attachment";
                                    break;
                                case "jpg":
                                case "jpeg":
                                    Session["App_ContentType"] = "image/jpeg";
                                    Session["App_ContentDisposition"] = "inline";
                                    break;
                                case "gif":
                                    Session["App_ContentType"] = "image/gif";
                                    Session["App_ContentDisposition"] = "inline";
                                    break;
                            }

                            Literal1.Text = "						<script language=\"Javascript\">" + Environment.NewLine;
                            Literal1.Text += "						  <!-- //" + Environment.NewLine;
                            Literal1.Text += "                          window.open(\"DownloadFile.aspx?AppID=" + Session["AppID"].ToString() + "\", \"_blank\", \"left=0,top=0,resizable=YES,scrollbars=YES\");" + Environment.NewLine;
                            Literal1.Text += "						  //-->" + Environment.NewLine;
                            Literal1.Text += "						</script>" + Environment.NewLine;
                        }
                        else
                        {
                            lblError.Text = "Die angeforderte Datei wurde nicht auf dem Server gefunden";
                        }
                    }
                    break;
            }
        }

        #endregion

    }
}
