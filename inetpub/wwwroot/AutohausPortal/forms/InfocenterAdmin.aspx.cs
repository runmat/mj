using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using CKG.Base.Kernel.Common;
using CKG.Base.Kernel.Security;
using AutohausPortal.lib;
using System.Data;
using System.Configuration;
using System.IO;
using System.Data.SqlClient;

namespace AutohausPortal.forms
{
    /// <summary>
    /// Infocenter (Admin) 
    /// </summary>
    public partial class InfocenterAdmin : System.Web.UI.Page
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

            fileSourcePath = ConfigurationManager.AppSettings["DownloadPathInfocenter"] + m_User.KUNNR + "\\";
            Literal1.Text = "";

            if (!IsPostBack)
            {
                FillGrouplist();
                FillDocumentTypes();
                FillDocuments();
            }
            else if (Session["objInfoCenter"] != null)
            {
                icDocs = (InfoCenterData)Session["objInfoCenter"];
            }
        }
        
        private void FillGrouplist()
        {
            Groups listGroups;

            lbxDocumentGroups.Items.Clear();

            using (SqlConnection cn = new SqlConnection(m_User.App.Connectionstring))
            {
                cn.Open();
                listGroups = new Groups(ref m_User, m_User.Customer.CustomerId, cn);
                cn.Close();
            }

            foreach (Group gr in listGroups)
            {
                lbxDocumentGroups.Items.Add(new ListItem(gr.GroupName, gr.GroupId.ToString()));
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

                UpdateDocumentTypeDropDowns();
            }
            catch (Exception ex)
            {
                data.Visible = false;
                lblError.Text = "Fehler beim Lesen der Dokumenttypen: " + ex.Message;
            }
        }

        private void UpdateDocumentTypeDropDowns()
        {
            try
            {
                if (icDocs != null)
                {
                    ddlDokumentart.Items.Clear();
                    ddlDocTypeSelection.Items.Clear();
                    foreach (DataRow dr in icDocs.DocumentTypes.Rows)
                    {
                        ddlDokumentart.Items.Add(new ListItem(dr["doctypeName"].ToString(), dr["documentTypeId"].ToString()));
                        ddlDocTypeSelection.Items.Add(new ListItem(dr["doctypeName"].ToString(), dr["documentTypeId"].ToString()));
                    }
                }
            }
            catch (Exception ex)
            {
                lblError.Text = "Fehler beim Füllen der Dokumenttyp-Auswahl: " + ex.Message;
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

                icDocs.GetDocuments();

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

        protected void lbtnLoeschen_Click(object sender, EventArgs e)
        {
            CheckBox chkSel;
            LinkButton lButton;
            string fName;
            string fExtension;
            string sPfad;

            foreach (Telerik.Web.UI.GridDataItem gridRow in rgDokumente.Items)
            {
                chkSel = (CheckBox)gridRow.FindControl("rb_sel");
                if ((chkSel != null) && (chkSel.Checked))
                {
                    lButton = (LinkButton)gridRow.FindControl("lbtDateiOeffnen");
                    if (lButton != null)
                    {
                        fName = lButton.Text;
                        fExtension = "." + gridRow["FileType"].Text;
                        sPfad = fileSourcePath + fName + fExtension;

                        if (File.Exists(sPfad))
                        {
                            File.Delete(sPfad);
                        }
                        icDocs.DeleteDocument(Int32.Parse(gridRow["DocumentId"].Text));
                    }
                }
            }

            Session["objInfoCenter"] = icDocs;
            rgDokumente.Rebind();
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
                    Telerik.Web.UI.GridGroupHeaderItem item = (Telerik.Web.UI.GridGroupHeaderItem) e.Item;
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
                case "editDocument":
                    index = e.Item.ItemIndex;
                    gridRow = rgDokumente.Items[index];
                    lButton = (LinkButton)e.Item.FindControl("lbtDateiOeffnen");

                    if (lButton != null)
                    {
                        int fId = Int32.Parse(gridRow["DocumentId"].Text);
                        string fName = lButton.Text;
                        string fType = gridRow["FileType"].Text;
                        string fExtension = "." + fType;

                        txtFileName.Text = fName + fExtension;
                        ihSelectedDocumentId.Value = fId.ToString();

                        // Dokumentart in DropDown selektieren
                        ddlDokumentart.SelectedValue = gridRow["docTypeId"].Text;

                        lbxDocumentGroups.ClearSelection();

                        // Gruppenzuordnungen für das gewählte Dokument laden
                        icDocs.GetDocumentRights(fId);

                        // zugeordnete Gruppen in ListBox selektieren
                        foreach (ListItem item in lbxDocumentGroups.Items)
                        {
                            if (icDocs.DocumentRights.Select("DocumentId=" + fId + " AND GroupId=" + item.Value).Length > 0)
                            {
                                item.Selected = true;
                            }
                        }

                        Result.Visible = false;
                        divEditDocTypes.Visible = false;
                        divEditDocument.Visible = true;
                    }
                    break;
            }
        }

        protected void ckb_SelectAll_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox chkSel;
            bool bChecked = ((CheckBox)sender).Checked;

            foreach (Telerik.Web.UI.GridDataItem gridRow in rgDokumente.Items)
            {
                chkSel = (CheckBox) (gridRow["colLoeschen"].FindControl("rb_sel"));
                if (chkSel != null)
                {
                    chkSel.Checked = bChecked;
                }
            }
        }

        #endregion

        #region Upload

        protected void lbtnUpload_Click(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(upFile.PostedFile.FileName))
            {
                if (!Directory.Exists(fileSourcePath))
                {
                    try
                    {
                        Directory.CreateDirectory(fileSourcePath);
                    }
                    catch (Exception ex)
                    {
                        lblError.Text = "Dokumentenverzeichnis konnte nicht angelegt werden. Bitte wenden Sie sich an Ihren System-Administrator.";
                        return;
                    }
                }

                FileInfo fileInfo = new FileInfo(upFile.PostedFile.FileName);
                string fExtension = fileInfo.Extension;
                string fType = fExtension.ToLower().Trim('.');
                string fName = fileInfo.Name.Replace(fExtension, "");
                fName = fName.Substring(fName.LastIndexOf("\\") + 1);

                switch (fType)
                {
                    case "jpg":
                        break;
                    case "jpeg":
                        break;
                    case "pdf":
                        break;
                    case "doc":
                        break;
                    case "docx":
                        break;
                    case "xls":
                        break;
                    case "xlsx":
                        break;
                    case "gif":
                        break;
                    default:
                        lblError.Text = "Es können nur Dateien im Format jpg, jpeg, gif, doc, docx, xls, xlsx und pdf hochgeladen werden.";
                        return;
                }

                // Datei in temp-Verzeichnis zwischenspeichern
                string tempVerzeichnis = ConfigurationManager.AppSettings["UploadPathInfocenterLocal"];
                if (String.IsNullOrEmpty(tempVerzeichnis))
                {
                    tempVerzeichnis = @"c:\inetpub\wwwroot\services\temp\infocenter\";
                }

                if (!Directory.Exists(tempVerzeichnis))
                {
                    lblError.Text = "Upload-Verzeichnis nicht auf dem Server vorhanden. Bitte wenden Sie sich an Ihren Systemadministrator.";
                    return;
                }

                if (!Directory.Exists(tempVerzeichnis + m_User.KUNNR))
                {
                    Directory.CreateDirectory(tempVerzeichnis + m_User.KUNNR);
                }
                upFile.SaveAs(tempVerzeichnis + m_User.KUNNR + "\\" + fName + fExtension);

                // Dateiname in Session merken
                icDocs.UploadFile = fName + fExtension;
                Session["objInfoCenter"] = icDocs;

                if (File.Exists(fileSourcePath + fName + fExtension))
                {
                    rblPopupOptions.SelectedValue = "Beibehalten";
                    ModalPopupExtender1.Show();
                }
                else
                {
                    SaveUploadedFile();
                }
            }
            else
            {
                lblError.Text = "Keine Datei ausgewählt.";
            }
        }

        protected void lbtnPopupOK_Click(object sender, EventArgs e)
        {
            switch (rblPopupOptions.SelectedValue)
            {
                case "Beibehalten":
                    SaveUploadedFile();
                    break;
                case "Ersetzen":
                    SaveUploadedFile(true);
                    break;
            }
        }

        protected void lbtnPopupCancel_Click(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(icDocs.UploadFile))
            {
                string tempVerzeichnis = ConfigurationManager.AppSettings["UploadPathInfocenterLocal"];
                if (String.IsNullOrEmpty(tempVerzeichnis))
                {
                    tempVerzeichnis = @"c:\inetpub\wwwroot\services\temp\infocenter\";
                }

                if (File.Exists(tempVerzeichnis + m_User.KUNNR + "\\" + icDocs.UploadFile))
                {
                    File.Delete(tempVerzeichnis + m_User.KUNNR + "\\" + icDocs.UploadFile);
                }
                icDocs.UploadFile = "";
                Session["objInfoCenter"] = icDocs;
            }
        }

        private void SaveUploadedFile(bool blnDateiErsetzen = false)
        {
            if (!String.IsNullOrEmpty(icDocs.UploadFile))
            {
                string tempVerzeichnis = ConfigurationManager.AppSettings["UploadPathInfocenterLocal"];
                if (String.IsNullOrEmpty(tempVerzeichnis))
                {
                    tempVerzeichnis = @"c:\inetpub\wwwroot\autohausportal\temp\infocenter\";
                }

                FileInfo fileInfo = new FileInfo(tempVerzeichnis + m_User.KUNNR + "\\" + icDocs.UploadFile);
                string fExtension = fileInfo.Extension;
                string fType = fExtension.ToLower().Trim('.');
                DateTime fLastEdited = fileInfo.LastWriteTime;
                long fSize = fileInfo.Length;
                string fName = fileInfo.Name.Replace(fExtension, "");
                fName = fName.Substring(fName.LastIndexOf("\\") + 1);

                // ggf. Datei umbenennen, um vorhandene Datei nicht zu überschreiben
                if ((File.Exists(fileSourcePath + fName + fExtension)) && (!blnDateiErsetzen))
                {
                    bool blnVorhanden = true;
                    int intSuffix = 0;
                    while (blnVorhanden)
                    {
                        intSuffix++;
                        if (!File.Exists(fileSourcePath + fName + "_" + intSuffix.ToString() + fExtension))
                        {
                            blnVorhanden = false;
                        }
                    }
                    fName = fName + "_" + intSuffix.ToString();
                }

                File.Copy(tempVerzeichnis + m_User.KUNNR + "\\" + icDocs.UploadFile, fileSourcePath + fName + fExtension, true);
                File.Delete(tempVerzeichnis + m_User.KUNNR + "\\" + icDocs.UploadFile);

                icDocs.SaveDocument(-1, 1, fName, fType, fLastEdited, fSize);
                icDocs.UploadFile = "";
                Session["objInfoCenter"] = icDocs;

                rgDokumente.Rebind();
            }
        }

        #endregion

        #region Edit Document

        protected void lbtnSaveDocument_Click(object sender, EventArgs e)
        {
            List<int> listeAdd = new List<int>();
            List<int> listeDelete = new List<int>();
            int anzTreffer;

            int documentId = Int32.Parse(ihSelectedDocumentId.Value);

            // Dokumententyp abgleichen und ggf. speichern
            string docTypeId = icDocs.Documents.Select("DocumentId=" + documentId)[0]["docTypeId"].ToString();
            if (ddlDokumentart.SelectedValue != docTypeId)
            {
                icDocs.SaveDocument(documentId, Int32.Parse(ddlDokumentart.SelectedValue));
            }

            // Gruppenzuordnung abgleichen und ggf. speichern
            foreach (ListItem item in lbxDocumentGroups.Items)
            {
                anzTreffer = icDocs.DocumentRights.Select("DocumentId=" + documentId + " AND GroupId=" + item.Value).Length;
                if ((item.Selected) && (anzTreffer == 0))
                {
                    listeAdd.Add(Int32.Parse(item.Value));
                }
                else if ((!item.Selected) && (anzTreffer > 0))
                {
                    listeDelete.Add(Int32.Parse(item.Value));
                }
            }

            if (listeAdd.Count > 0)
            {
                icDocs.SaveDocumentRights(documentId, listeAdd);
            }
            if (listeDelete.Count > 0)
            {
                icDocs.DeleteDocumentRights(documentId, listeDelete);
            }

            divEditDocument.Visible = false;
            divEditDocTypes.Visible = false;
            Result.Visible = true;

            Session["objInfoCenter"] = icDocs;
            rgDokumente.Rebind();
        }

        protected void lbtnCancelEditDocument_Click(object sender, EventArgs e)
        {
            divEditDocument.Visible = false;
            divEditDocTypes.Visible = false;
            Result.Visible = true;
        }

        #endregion

        #region Edit DocType

        protected void ddlDocTypeSelection_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlDocTypeSelection.SelectedIndex >= 0)
            {
                txtDocTypeName.Text = ddlDocTypeSelection.SelectedItem.Text;
            }
            else
            {
                txtDocTypeName.Text = "";
            }
        }

        protected void lbtnEditDocTypes_Click(object sender, EventArgs e)
        {
            if (ddlDocTypeSelection.SelectedIndex >= 0)
            {
                txtDocTypeName.Text = ddlDocTypeSelection.SelectedItem.Text;
            }
            else
            {
                txtDocTypeName.Text = "";
            }

            Result.Visible = false;
            divEditDocument.Visible = false;
            divEditDocTypes.Visible = true;
            trNewDocType.Visible = false;
            lbtnSaveNewDocType.Visible = false;
            trEditDocType.Visible = true;
            lbtnAddNewDocType.Visible = true;
        }

        protected void lbtnSaveDocType_Click(object sender, EventArgs e)
        {
            if (ddlDocTypeSelection.SelectedIndex >= 0)
            {
                // Dokumententypbezeichnung abgleichen und ggf. speichern
                if (ddlDocTypeSelection.SelectedItem.Text != txtDocTypeName.Text)
                {
                    icDocs.SaveDocumentType(Int32.Parse(ddlDocTypeSelection.SelectedItem.Value), txtDocTypeName.Text);
                }
            }

            Session["objInfoCenter"] = icDocs;
            UpdateDocumentTypeDropDowns();
            rgDokumente.Rebind();
        }

        protected void lbtnDeleteDocType_Click(object sender, EventArgs e)
        {
            bool blnSuccess = false;

            if (ddlDocTypeSelection.SelectedIndex >= 0)
            {
                if (ddlDocTypeSelection.SelectedItem.Value == "1")
                {
                    lblError.Text = "Die Default-Dokumentenart kann nicht gelöscht werden.";
                }
                else
                {
                    blnSuccess = icDocs.DeleteDocumentType(Int32.Parse(ddlDocTypeSelection.SelectedItem.Value));

                    if (blnSuccess)
                    {
                        Session["objInfoCenter"] = icDocs;
                        UpdateDocumentTypeDropDowns();
                        rgDokumente.Rebind();
                    }
                    else
                    {
                        lblError.Text = "Die Dokumentenart konnte nicht gelöscht werden, da sie noch von Dokumenten genutzt wird.";
                    }
                }
            }
        }

        protected void lbtnAddNewDocType_Click(object sender, EventArgs e)
        {
            trEditDocType.Visible = false;
            lbtnAddNewDocType.Visible = false;
            trNewDocType.Visible = true;
            lbtnSaveNewDocType.Visible = true;
        }

        protected void lbtnSaveNewDocType_Click(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(txtNewDocType.Text))
            {
                icDocs.SaveDocumentType(-1, txtNewDocType.Text.Trim());
            }
            else
            {
                lblError.Text = "Es wurde keine Bezeichnung für die neue Dokumentenart angegeben";
            }

            Session["objInfoCenter"] = icDocs;
            UpdateDocumentTypeDropDowns();

            trNewDocType.Visible = false;
            lbtnSaveNewDocType.Visible = false;
            trEditDocType.Visible = true;
            lbtnAddNewDocType.Visible = true;
        }

        protected void lbtnCancelEditDocType_Click(object sender, EventArgs e)
        {
            divEditDocument.Visible = false;
            divEditDocTypes.Visible = false;
            Result.Visible = true;
        }

        #endregion

    }
}