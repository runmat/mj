using CKG.Base.Kernel;
using CKG.Base.Kernel.Common;
using System.Data.OleDb;
using System.IO;
using System.Configuration;
using Microsoft.Office;
using System;
using System.Text.RegularExpressions;
using System.Data;
using System.Web.UI.WebControls;
using System.Web.UI;

namespace appCheckServices.forms
{
    public partial class Change01 : System.Web.UI.Page
    {
 
        #region "Declarations
        
            private CKG.Base.Kernel.Security.App m_App;
            private CKG.Base.Kernel.Security.User m_User;
            DataTable table;
            String fileSourcePath; 
        #endregion 



        protected void Page_Load(object sender, EventArgs e)
        {

            m_User = Common.GetUser(this);
            // füllen Form.Session("objUser"), rückgabe eines UserObjekte 
            Common.FormAuth(this, m_User);
            m_App = new CKG.Base.Kernel.Security.App(m_User);
            //erzeugt ein App_objekt 
            Common.GetAppIDFromQueryString(this);

            lblHead.Text = (string)m_User.Applications.Select("AppID = '" + Session["AppID"] + "'")[0]["AppFriendlyName"];
            lblNoData.Visible = true;
            lblNoData.Text = "Bitte wählen Sie eine Datei aus.";

                            try { 
                    if ((string)Request.QueryString["check"] == "1") { 
                        // Kunde 
                        fileSourcePath = ConfigurationManager.AppSettings["UpDownKundeXL"]; 
                        fileSourcePath += m_User.Organization.OrganizationReference + "\\export\\" + m_User.Reference + "\\vorlagen\\";
                        lblNoData.Text = "Bitte laden Sie hier Ihre Sammelanfrage zur Bearbeitung hoch.(Vorlage zum Upload finden sie im unteren Bereich der Seite)";
                    } 
                    else { 
                        fileSourcePath = ConfigurationManager.AppSettings["DownloadPathZulXL"];
                        lblNoData.Text = "Bitte laden Sie hier Ihre Sammelanfrage zur Bearbeitung hoch.";
                    } 
                    
                    
                    if (!IsPostBack) { 
                        DoSubmit(); 
                    } 
                }
                            catch
                            {
                                lblNoData.Visible = true;
                                lblNoData.Text = "Keine Dokumente zur Anzeige gefunden.";
                            } 
        }

        protected void cmdSearch_Click(object sender, System.EventArgs e)
        {


            string strMessage = "";
            //string status = "";
            FileInfo fi = default(FileInfo);
            //string FileVersion = "";
            string path = null;
            lblNoData.Text = "";
            try
            {
                if ((upFile.PostedFile != null))
                {
                    //Dim objFile As Object 
                    fi = new FileInfo(upFile.PostedFile.FileName);
                    path = ConfigurationManager.AppSettings["ExcelPath"];

                   //DSOFile.OleDocumentProperties objFile = new DSOFile.OleDocumentProperties();
                    
                    switch (fi.Extension.ToUpper(System.Globalization.CultureInfo.InvariantCulture))
                    {


                        case ".XLS":
                            upFile.PostedFile.SaveAs(path + fi.Name);
                            fi = new FileInfo(path + fi.Name);

                            CKG.Base.Kernel.DocumentGeneration.ExcelDocumentFactory Exc = new CKG.Base.Kernel.DocumentGeneration.ExcelDocumentFactory();

                            string Message = "";

                            Message = Exc.CheckExcelVersion(path + fi.Name);

                            if (Message == "Error")
                            {
                                lblError.Text = "Die Exceldatei benutzt ein veraltetes Format. Bitte benutzen Sie Excelversion 8 (Excel 97) oder höher!";
                                return;
                            }

                            break;
                        case ".XLSX":

                            upFile.PostedFile.SaveAs(path + fi.Name);
 
                            break;
                        default:
                            lblError.Text = "Es können nur Exceldateien hochgeldaden werden.";
                            return;

                    }

                    string upPath = null;
                    if ((string)Request.QueryString["check"] == "1")
                    {
                        // Kunde 
                        upPath = ConfigurationManager.AppSettings["UpDownKundeXL"];
                        upPath += m_User.Organization.OrganizationReference + @"\import\" + m_User.Reference + @"\";
                    }
                    else
                    {
                        //ZulStelle 
                        upPath = ConfigurationManager.AppSettings["UploadPathZulXL"];

                        Upload();
                    }

                    string sStatus = upFile.PostedFile.FileName.Substring(upFile.PostedFile.FileName.LastIndexOf(@"\") + 1);


                    upFile.PostedFile.SaveAs(upPath + sStatus);

                    strMessage = strMessage + "Es sind neue Daten für XL-Check vorhanden!" + Environment.NewLine + Environment.NewLine;
                    if ((string)Request.QueryString["check"] == "1")
                    {
                        strMessage = strMessage + "Bitte überprüfen Sie den Eingangsordner des Kunden " + m_User.Organization.OrganizationReference + "!" + Environment.NewLine + Environment.NewLine + Environment.NewLine;
                    }
                    else
                    {
                        strMessage = strMessage + "Bitte überprüfen Sie den Eingangsordner der Zulassungsstelle!" + Environment.NewLine + Environment.NewLine + Environment.NewLine;
                    }

                    strMessage = strMessage + Environment.NewLine + "Achtung: Diese Nachricht wurde automatisch generiert! Bitte antworten Sie nicht darauf!";


                    System.Net.Mail.MailMessage Mail = default(System.Net.Mail.MailMessage);
                    string smtpMailSender = "";
                    string smtpMailServer = "";
                    string EmailAdresse = "";

                    if ((string)Request.QueryString["check"] == "1")
                    {
                        EmailAdresse = ConfigurationManager.AppSettings["MailDad"];
                    }
                    else
                    {
                        //EmailAdresse = ConfigurationManager.AppSettings("MailZulStelle")' 
                        EmailAdresse = ConfigurationManager.AppSettings["MailDad"];
                    }

                    smtpMailSender = ConfigurationManager.AppSettings["SmtpMailSender"];
                    Mail = new System.Net.Mail.MailMessage(smtpMailSender, EmailAdresse, "Neue Daten XL-Check 2.0", strMessage);
                    //Mail.Priority = Net.Mail.MailPriority.High 

                    smtpMailServer = ConfigurationManager.AppSettings["SmtpMailServer"];
                    System.Net.Mail.SmtpClient client = new System.Net.Mail.SmtpClient(smtpMailServer);
                    client.Send(Mail);

                    lblNoData.Text = "Datei erfolgreich hochgeladen!";

                }
            }
            catch (System.Exception ex)
            {
                lblError.Visible = true;
                lblError.Text = "Fehler beim Hochladen der Datei." + ex.Message;
            }
            finally
            {
            } 

        }

        private void Upload()
        {
            string fname = null;
            string fnameNew = null;
            string path = null;
            string Extension = null;
            
            System.Web.HttpPostedFile File = default(System.Web.HttpPostedFile);
            lblError.Text = string.Empty;

            if (!(upFile.PostedFile.FileName == string.Empty))
            {
                try
                {
                    fname = upFile.PostedFile.FileName;
                    //Dateigröße prüfen 
                    //if ((upFile.PostedFile.ContentLength > ConfigurationManager.AppSettings["MaxUploadSizeUeberf"]))
                    //{
                    //    lblError.Text = "Datei '" + Strings.Right(fname, fname.Length - fname.LastIndexOf("\\") - 1).ToUpper + "' ist zu gross (>200 KB).";
                    //    return;
                    //}
                    //'------------------ 
                    //Extension = Strings.Right(upFile.PostedFile.FileName, 4);
                    Extension = upFile.PostedFile.FileName.Substring(fname.LastIndexOf("."));

                    //path = ConfigurationManager.AppSettings("UploadPathLocal") '"\\192.168.10.79\\Datawizard\\Upload\\Ueberfuehrungen\\" 

                    path = ConfigurationManager.AppSettings["UploadPathZulXL"];

                    File = (System.Web.HttpPostedFile)upFile.PostedFile;
                    fnameNew = DateTime.Now.ToShortDateString() + "_" + DateTime.Now.ToShortTimeString().Replace(":", "-") + "_" + m_User.KUNNR + "-" + m_User.UserName;
                    
                    File.SaveAs(path + fnameNew + Extension);
                    
                    string sUrl = ConfigurationManager.AppSettings["DataWizard"];
                    sUrl = sUrl + "/fpruef_Upload_from_Zulassungsstelle?erfasser=" + m_User.UserName;
                    System.Net.WebRequest request = System.Net.WebRequest.Create(sUrl);


                    FileStream stream = new FileStream(path + fnameNew + Extension, FileMode.Open);
                    StreamReader reader = new StreamReader(File.InputStream);
                    int lBytes = (int)stream.Length;
                    byte[] byte1 = new byte[lBytes];
                    //Lesen der Datei in ein Bytearray 
                    stream.Read(byte1, 0, lBytes);
                    stream.Close();

                    request.Method = System.Net.WebRequestMethods.Http.Post;
                    request.ContentType = File.ContentType;
                    request.ContentLength = byte1.Length;

                    //Schreiben des Bytearray in den erwateten Stream des Zielservers 
                    Stream newStream = request.GetRequestStream();

                    newStream.Write(byte1, 0, byte1.Length);

                    newStream.Close();

                    System.Net.WebResponse response = request.GetResponse();
                    Stream responseStream = response.GetResponseStream();

                    StreamReader reader2 = new StreamReader(responseStream);
                    //' Antwort lesen 
                    string responseFromServer = reader2.ReadToEnd();
                    //' Streams schließen. 
                    reader.Close();
                    response.Close();
                    if (responseFromServer == "Error")
                    {
                        lblError.Text = "Fehler beim verarbeiten Ihter Datei!";
                    }
                }
                catch (Exception ex)
                {
                    lblError.Text = ex.Message;
                }
            }
            else
            {
                lblError.Text = "Keine Datei ausgewählt.";
            }
        }

        protected void lbBack_Click(object sender, EventArgs e)
        {
            Response.Redirect("../../../Start/Selection.aspx?AppID=" + Session["AppID"], false);
        }

        private void Fillgrid(Int32 intPageIndex, DataTable Table, String strSort)
        {

            if (Table.Rows.Count == 0)
            {
                DataGrid1.Visible = false;
                lblNoData.Visible = true;

                lblNoData.Text = "Keine Dokumente zur Anzeige gefunden.";
            }
            else
            {
                DataGrid1.Visible = true;
                lblNoData.Visible = false;

                DataView tmpDataView = new DataView();
                tmpDataView = Table.DefaultView;

                Int32 intTempPageIndex = intPageIndex;
                String strTempSort = "";
                String strDirection = null;

                if (strSort.Trim(' ').Length > 0)
                {
                    intTempPageIndex = 0;
                    strTempSort = strSort.Trim(' ');
                    if ((this.ViewState["Sort"] == null) || ((String)this.ViewState["Sort"] == strTempSort))
                    {
                        if (this.ViewState["Direction"] == null)
                        {
                            strDirection = "desc";
                        }
                        else
                        {
                            strDirection = (String)this.ViewState["Direction"];
                        }
                    }
                    else
                    {
                        strDirection = "desc";
                    }

                    if (strDirection == "asc")
                    {
                        strDirection = "desc";
                    }
                    else
                    {
                        strDirection = "asc";
                    }

                    this.ViewState["Sort"] = strTempSort;
                    this.ViewState["Direction"] = strDirection;
                }
                DataGrid1.DataSource = tmpDataView;
                DataGrid1.DataBind();

                //DataGridItem item = default(DataGridItem); 
                TableCell cell = default(TableCell);
                ImageButton linBut = default(ImageButton);
                Control control = default(Control);
                string sPattern = null;

                foreach (DataGridItem item in DataGrid1.Items)
                {
                    sPattern = item.Cells[2].Text;
                    cell = item.Cells[0];
                    if (sPattern == "xls" | sPattern == "csv")
                    {
                        foreach (Control ctrl in cell.Controls)
                        {
                            if (ctrl is ImageButton)
                            {
                                linBut = (ImageButton)ctrl;
                                if (linBut.ID == "lbtExcel")
                                {
                                    linBut.Visible = true;
                                }
                            }
                        }
                    }
                    else if (sPattern == "doc")
                    {
                        foreach (Control ctrl in cell.Controls)
                        {
                            if (ctrl is ImageButton)
                            {
                                linBut = (ImageButton)ctrl;
                                if (linBut.ID == "lbtWord")
                                {
                                    linBut.Visible = true;
                                }
                            }

                        }
                    }
                    else if ((sPattern.ToLower() == "jpg") | (sPattern == "jepg"))
                    {
                        foreach (Control ctrl in cell.Controls)
                        {
                            if (ctrl is ImageButton)
                            {
                                linBut = (ImageButton)ctrl;
                                if (linBut.ID == "lbtJepg")
                                {
                                    linBut.Visible = true;
                                }
                            }
                        }
                    }
                    else if (sPattern.ToLower() == "pdf")
                    {
                        foreach (Control ctrl in cell.Controls)
                        {
                            if (ctrl is ImageButton)
                            {
                                linBut = (ImageButton)ctrl;
                                if (linBut.ID == "lbtPDF")
                                {
                                    linBut.Visible = true;
                                }
                            }
                        }
                    }
                    else if (sPattern.ToLower() == "gif")
                    {
                        foreach (Control ctrl in cell.Controls)
                        {
                            if (ctrl is ImageButton)
                            {
                                linBut = (ImageButton)control;
                                if (linBut.ID == "lbtGif")
                                {
                                    linBut.Visible = true;
                                }
                            }
                        }
                    }
                    else if (sPattern.ToLower() == "zip")
                    {
                        foreach (Control ctrl in cell.Controls)
                        {
                            if (ctrl is ImageButton)
                            {
                                linBut = (ImageButton)ctrl;
                                if (linBut.ID == "lbtZip")
                                {
                                    linBut.Visible = true;
                                }
                            }
                        }
                    }
                }
                Result.Visible = true;
            }
        }

        protected void DataGrid1_ItemCommand(Object sender, DataGridCommandEventArgs e)
        {
            if (e.CommandName == "open")
            {
                string sPfad = null;
                string fname = null;
                try
                {
                    if (e.Item.Cells[2].Text == "pdf")
                    {
                        sPfad = e.Item.Cells[1].Text;
                        Session["App_Filepath"] = sPfad;
                        Session["App_ContentType"] = "Application/pdf";

                        Literal1.Text = " <script language=\"Javascript\">" + Environment.NewLine;
                        Literal1.Text += " <!-- //" + Environment.NewLine;
                        Literal1.Text += " window.open(\"Report01_2.aspx?AppID=" + (String)Session["AppID"] + "\", \"_blank\", \"left=0,top=0,resizable=YES,scrollbars=YES\");" + Environment.NewLine;
                        Literal1.Text += " //-->" + Environment.NewLine;

                        Literal1.Text += " </script>" + Environment.NewLine;
                    }
                    else if (e.Item.Cells[2].Text == "xls")
                    {
                        CKG.Base.Kernel.DocumentGeneration.ExcelDocumentFactory ExcelFactory = new CKG.Base.Kernel.DocumentGeneration.ExcelDocumentFactory();
                        sPfad = e.Item.Cells[1].Text;
                        fname = sPfad.Substring(sPfad.LastIndexOf(@"\") + 1);
                        //upFile.PostedFile.FileName.Substring(upFile.PostedFile.FileName.LastIndexOf(@"\") + 1);

                        ExcelFactory.ReturnExcelTab(sPfad, fname, this.Page);
                    }

                }
                catch (Exception ex)
                {
                    lblError.Text = "Fehler beim Erstellen der Excel-Datei: " + ex.Message;
                }
            }
        }

        private void DoSubmit()
        {
            DataRow row = default(DataRow);
            DataColumn column = default(DataColumn);
            string[] files = null;
            string[] files2 = null;
            System.IO.FileInfo info = default(System.IO.FileInfo);
            int i = 0;
            string fname = null;
            string fname1 = null;

            try
            {
                table = new DataTable();
                column = new DataColumn("Serverpfad", System.Type.GetType("System.String"));
                table.Columns.Add(column);
                column = new DataColumn("Filename", System.Type.GetType("System.String"));
                table.Columns.Add(column);
                column = new DataColumn("Filedate", System.Type.GetType("System.DateTime"));
                table.Columns.Add(column);
                column = new DataColumn("Pfad", System.Type.GetType("System.String"));
                table.Columns.Add(column);
                column = new DataColumn("Pattern", System.Type.GetType("System.String"));
                table.Columns.Add(column);


                files = System.IO.Directory.GetFiles(fileSourcePath, "*.pdf");
                files2 = System.IO.Directory.GetFiles(fileSourcePath, "*.xls");
 

                for (i = 0; i <= files.Length - 1; i++)
                {
                    info = new System.IO.FileInfo((String)files.GetValue(i));
                    fname = (String)files.GetValue(i);
                    fname1 = fname.Substring(fname.LastIndexOf(@"\") + 1); ;
                    row = table.NewRow();
                    row["Serverpfad"] = fileSourcePath + "\\" + fname1;
                    fname1 = fname1.Substring(0, fname1.LastIndexOf("."));
                    row["Filename"] = fname1;
                    row["Filedate"] = info.CreationTime;
                    row["Pattern"] = "pdf";
                    table.Rows.Add(row);
                }

                for (i = 0; i <= files2.Length - 1; i++)
                {
                    info = new System.IO.FileInfo((String)files2.GetValue(i));
                    fname = (String)files2.GetValue(i);
                    fname1 = fname.Substring(fname.LastIndexOf(@"\") + 1);
                    row = table.NewRow();
                    row["Serverpfad"] = fileSourcePath + "\\" + fname1;
                    fname1 = fname1.Substring(0, fname1.LastIndexOf("."));

                    row["Filename"] = fname1;
                    row["Filedate"] = info.CreationTime;
                    row["Pattern"] = "xls";

                    table.Rows.Add(row);
                }

                Fillgrid(0, table, "");
            }
            catch
            {
                lblNoData.Visible = true;
                lblNoData.Text = "Keine Dokumente zur Anzeige gefunden.";
            }
        }

        protected void DataGrid1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

    }
}
