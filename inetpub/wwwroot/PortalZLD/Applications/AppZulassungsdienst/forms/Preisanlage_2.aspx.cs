using System;
using System.Web.UI.WebControls;
using AppZulassungsdienst.lib;
using CKG.Base.Kernel.Common;
using CKG.Base.Kernel.Security;
using System.Data;
using System.Configuration;
using CKG.Base.Kernel.DocumentGeneration;

namespace AppZulassungsdienst.forms
{
    /// <summary>
    /// Preisanlage pro Landkreis.
    /// </summary>
    public partial class Preisanlage_2 : System.Web.UI.Page
    {
        private CKG.Base.Kernel.Security.User m_User;
        private CKG.Base.Kernel.Security.App m_App;
        private VoerfZLD objVorerf;
        private ZLDCommon objCommon;
        /// <summary>
        /// Page_Load Ereignis. Prüfen ob die Anwendung dem Benutzer zugeordnet ist. Evtl. Stammdaten laden.
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            m_User = Common.GetUser(this);
            Common.FormAuth(this, m_User);
            m_App = new App(m_User);
            Common.GetAppIDFromQueryString(this);
            lblHead.Text = (string)m_User.Applications.Select("AppID = '" + Session["AppID"] + "'")[0]["AppFriendlyName"];
            if (Session["objVorerf"] != null)
            {
                objVorerf = (VoerfZLD)Session["objVorerf"];
            }
            else
            {
                lblError.Text = "Benötigtes Session-Objekt fehlt!";
            }
            if (Session["objCommon"] == null)
            {
                objCommon = new ZLDCommon(ref m_User, m_App);
                objCommon.VKBUR = m_User.Reference.Substring(4, 4);
                objCommon.VKORG = m_User.Reference.Substring(0, 4);
                objCommon.getSAPDatenStamm(Session["AppID"].ToString(), Session.SessionID, this);
                objCommon.getSAPZulStellen(Session["AppID"].ToString(), Session.SessionID, this);
                objCommon.LadeKennzeichenGroesse();
                Session["objCommon"] = objCommon;
            }
            else
            {
                objCommon = (ZLDCommon)Session["objCommon"];

            }
            if (IsPostBack != true)
            {
                fillForm();
            }
        }
        /// <summary>
        /// Spaltenübersetzung
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        private void Page_PreRender(object sender, System.EventArgs e)
        {
            Common.SetEndASPXAccess(this);

        }
        /// <summary>
        /// Spaltenübersetzung
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        private void Page_Unload(object sender, System.EventArgs e)
        {
            Common.SetEndASPXAccess(this);
        }
        /// <summary>
        /// Daten aufbereiten und Gridview füllen.
        /// </summary>
        private void fillForm()
        {


            lblKunnr.Text = objVorerf.NeueKundenNr;
            lblKunnname.Text = objVorerf.NeueKundenName;

            Session["objVorerf"] = objVorerf;
            if (objVorerf.Status > 0)
            {
                lblError.Text = objVorerf.Message;
                return;
            }
            else

            {

                
                DataTable tblData = new DataTable();
                tblData.Columns.Add("Matnr", typeof(String));
                tblData.Columns.Add("Maktx", typeof(String));
                for (int i = 1; i < 31; i++)
			        {
                        tblData.Columns.Add("Stva" + i.ToString(), typeof(String));
			        } 
                int iCount = 1;
                foreach (DataRow item in objCommon.tblMaterialtextohneMatNr.Rows)
                {

                    DataRow tblRow = tblData.NewRow();
                    
                     if (iCount == 1)
                     {
                         tblRow["Matnr"] = "Material";
                         tblRow["Maktx"] = "Bezeichnung";
                     }
                     else
	                 {
		               tblRow["Matnr"] = item["Matnr"].ToString().TrimStart('0');
                       tblRow["Maktx"] = item["Maktx"].ToString();
	                 }

                     for (int i = 1; i < 31; i++)
                     {
                        tblRow["Stva" + i.ToString()] = "";
                      }

                    tblData.Rows.Add(tblRow);
                    iCount++;
                }
                Session["tblData"] = tblData;
                GridView1.DataSource = tblData;
                GridView1.DataBind();

                

                Label lbl = (Label)GridView1.Rows[0].FindControl("lblDienstNr");
                lbl.Attributes.Add("style", "font-weight: bold");
                lbl = (Label)GridView1.Rows[0].FindControl("lblDienst");
                lbl.Attributes.Add("style", "font-weight: bold");

                TextBox txtStva1 = (TextBox)GridView1.Rows[0].FindControl("txtInput1");
                txtStva1.Focus();
                int Rows = 0;
                foreach (GridViewRow Row in GridView1.Rows)
                {
                    int iRows = 1;



                    for (int i = 2; i < Row.Cells.Count; i++)
                    {
                        TextBox txtInput = (TextBox)Row.Cells[i].FindControl("txtInput" + (iRows).ToString());
                        txtInput.Attributes.Add("onkeyup", "keyPressed(this.id, event)");
                        if (Rows == 0)
                        {
                            txtInput.Attributes.Remove("onkeypress");
                        }
                        iRows++;
                    }

                    Rows++;
                }
                    
            }
           

        }
        /// <summary>
        /// Daten auslesen und in Excel-Vorlage schreiben und an den Innendienst senden.
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        protected void cmdCreate_Click(object sender, EventArgs e)
        {
            lblMessage.Text = "";
            DataTable tblData = new DataTable();
            ExcelDocumentFactory excel = new ExcelDocumentFactory();
            String Filename = "Zugriff1_" + objVorerf.NeueKundenNr + "_" + m_User.UserName + "_" + String.Format("{0:yyyyMMdd_HHmmss}", System.DateTime.Now) + ".xls";
            DataTable tblHEAD = new DataTable();
            tblHEAD.Columns.Add("Kunnr", typeof(String));
            tblHEAD.Columns.Add("vkbur", typeof(String));
            tblHEAD.Columns.Add("vkorg", typeof(String));
            tblHEAD.Columns.Add("Zugriff", typeof(String));
            tblHEAD.TableName = "Head";
            DataRow tblRowT = tblHEAD.NewRow();
            tblRowT["Kunnr"] = objVorerf.NeueKundenNr;
            tblRowT["vkbur"] = objVorerf.VKBUR;
            tblRowT["vkorg"] = objVorerf.VKORG;
            tblRowT["Zugriff"] = "Zugriff1";
            
            tblHEAD.Rows.Add(tblRowT);
            tblData = CreateTableFromGridView();
            DataSet OutputSet = new DataSet();
            OutputSet.Tables.Add(tblData);
            OutputSet.Tables.Add(tblHEAD);

            excel.CreateDocumentAndWriteToFilesystemTemplate(Filename, OutputSet, this, true, "C:\\inetpub\\wwwroot\\PortalZLD\\Applications\\AppZulassungsdienst\\Documents\\Mappe1.xlt", 0, 2, true);
          if(  Sendmail(ConfigurationManager.AppSettings["ExcelPath"] + Filename))
          {
              lblMessage.Text = "Preise gesendet!";
          
          }
        }
        /// <summary>
        /// Mail mit Anhang an Innendienst senden.
        /// </summary>
        /// <param name="Filenname">Datei</param>
        /// <returns>true bei Erfolg, false bei Fehler</returns>
        private Boolean Sendmail(String Filenname)
        {
            System.Net.Mail.Attachment file;

            try
            {
                System.Net.Mail.MailMessage Mail;
                ZLD_Suche objZLDSuche = new ZLD_Suche(ref m_User, m_App, "");

                objZLDSuche.LeseMailTexte("2");

                String smtpMailSender = "";
                String smtpMailServer = "";

                smtpMailSender = ConfigurationManager.AppSettings["SmtpMailSender"];
                smtpMailServer = ConfigurationManager.AppSettings["SmtpMailServer"];

                String MailText = "Kundennr.: " + objVorerf.NeueKundenNr + "<br />";
                MailText += "Kunde: " + objVorerf.NeueKundenName + "<br />";
                MailText += "Filiale: " + objVorerf.VKBUR + "<br /><br />";
                MailText += "Datum: " + System.DateTime.Now.ToShortDateString() + "<br />";
                MailText += "Uhrzeit: " + System.DateTime.Now.ToShortDateString() + "<br />";
                MailText += "Web-Benutzer: " + m_User.UserName + "<br />";

                String[] Adressen;
                if (objZLDSuche.MailAdress.Trim().Split(';').Length > 1)
                {
                    Mail = new System.Net.Mail.MailMessage();
                    System.Net.Mail.MailAddress Mailsender = new System.Net.Mail.MailAddress(smtpMailSender);
                    Mail.Sender = Mailsender;
                    Mail.From = Mailsender;
                    Mail.Body = MailText;


                    Mail.Subject = "Preisanlage Neukunden(" + objVorerf.NeueKundenNr + ") Filiale: " + m_User.Reference.Substring(4, 4);
                    Adressen = objZLDSuche.MailAdress.Trim().Split(';');
                    foreach (String tmpStr in Adressen)
                    {
                        Mail.To.Add(tmpStr);
                    }

                }
                else
                {
                    Mail = new System.Net.Mail.MailMessage(smtpMailSender, objZLDSuche.MailAdress.Trim(), "Preisanlage Neukunden(" + objVorerf.NeueKundenNr + ") Filiale: " + m_User.Reference.Substring(4, 4), MailText);
                }
                if (objZLDSuche.MailAdressCC.Trim().Split(';').Length > 1)
                {
                    Adressen = objZLDSuche.MailAdressCC.Trim().Split(';');
                    foreach (String tmpStr in Adressen)
                    {
                        Mail.CC.Add(tmpStr);
                    }

                }
                else if (objZLDSuche.MailAdressCC.Length > 0)
                {
                    Mail.CC.Add(objZLDSuche.MailAdressCC);
                }

                Mail.IsBodyHtml = true;
                System.Net.Mail.SmtpClient client = new System.Net.Mail.SmtpClient(smtpMailServer);
                file = new System.Net.Mail.Attachment(Filenname);
                Mail.Attachments.Add(file);

                client.Send(Mail);
                Mail.Attachments.Dispose();
                Mail.Dispose();
                return true;
            }
            catch (Exception ex)
            {
                lblError.Text = "Fehler beim Senden! " + ex.Message;
                return false;

            }
        }
        /// <summary>
        /// Daten aus dem Gridview in eine Tabelle schreiben.
        /// </summary>
        /// <returns>Datentabelle</returns>
        private DataTable CreateTableFromGridView()
        
        {
            DataTable test = new DataTable();
            test.Columns.Add("Material", typeof(String));
            test.Columns.Add("Bezeichnung", typeof(String));
            for (int i = 2; i < GridView1.Rows[0].Cells.Count; i++)
            {
                TableCell gridCell = GridView1.Rows[0].Cells[i];
                TextBox txtInput = (TextBox)gridCell.FindControl("txtInput" + (i-1).ToString());
                if (txtInput.Text != "")
                {
                    test.Columns.Add(txtInput.Text, typeof(String));
                }
            }

            int iCount = 0;
            foreach (GridViewRow Row in GridView1.Rows)
            {
                if (iCount > 0)
                {
                    DataRow NewRow = test.NewRow();

                    for (int i = 0; i < test.Columns.Count; i++)
                    {
                        if (i == 0)
                        {
                            Label lbl = (Label)Row.Cells[i].FindControl("lblDienstNr");
                            NewRow[i] = lbl.Text;
                        }
                        if (i == 1)
                        {
                            Label lbl = (Label)Row.Cells[i].FindControl("lblDienst");
                            NewRow[i] = lbl.Text;
                        }

                        if (i > 1)
                        {
                            TextBox txtInput = (TextBox)Row.Cells[i].FindControl("txtInput" + (i - 1).ToString());
                            NewRow[i] = txtInput.Text;
                        }
                    }

                    test.Rows.Add(NewRow); 
                }
                iCount++;
            }
            return test;
                
       }
        /// <summary>
        /// Zurück zur Preisanlage Seite1.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lb_zurueck_Click(object sender, EventArgs e)
        {
            Response.Redirect("Preisanlage.aspx?AppID=" + Session["AppID"].ToString());
        }
    }
}
