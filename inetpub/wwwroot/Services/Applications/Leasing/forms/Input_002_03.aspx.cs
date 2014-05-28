using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CKG;
using CKG.Base.Business;
using CKG.Base.Kernel;
using CKG.Base.Kernel.Common;
using CKG.Base.Kernel.Security;
using Leasing.lib;
using System.Configuration;
using System.Data;

namespace Leasing.forms
{
    public partial class Input_002_03 : System.Web.UI.Page
    {
        private CKG.Base.Kernel.Security.User m_User;
        private CKG.Base.Kernel.Security.App m_App;
        private DataTable m_objTable;

        protected CKG.Services.GridNavigation GridNavigation1;

        protected void Page_Load(object sender, EventArgs e)
        {
            Session["ShowLink"] = "False";
            m_User = Common.GetUser(this);
            Common.FormAuth(this, m_User);

            Common.GetAppIDFromQueryString(this);
            try
            {
                lblHead.Text = m_User.Applications.Select("AppID = '" + Session["AppID"].ToString() + "'")[0]["AppFriendlyName"].ToString();
                m_App = new CKG.Base.Kernel.Security.App(m_User);


                GridNavigation1.setGridElment(ref GridView1);
                GridNavigation1.PagerChanged += GridView1_PageIndexChanged;
                GridNavigation1.PageSizeChanged += GridView1_ddlPageSizeChanged;

                if (!IsPostBack)
                {
                    DoSubmit();
                }

                //;
            }
            catch (Exception ex)
            {
                lblError.Text = "Beim Laden der Seite ist ein Fehler aufgetreten.<br>(" + ex.Message + ")";
            }
        }



        private void DoSubmit()
        {
            
            try
            {
                DateTime now = DateTime.Now;
                string strFileName = now.Year.ToString() + now.Month.ToString() + now.Day.ToString() + "_" + now.Hour.ToString() +
                                        now.Minute.ToString() + now.Second.ToString() + "_" + m_User.UserName + ".xls";
                LP_03 m_Report = new LP_03(m_User, m_App, strFileName);
                //---> Eingabedaten

                m_Report.PKennzeichenVon = "";
                m_Report.PKennzeichenBis = "";
                m_Report.PFahrgestellVon = "";
                m_Report.PFahrgestellBis = "";
                m_Report.PLeasingNrVon = "";
                m_Report.PLeasingNrBis = "";

                m_Report.PKundenNr = "";
                m_Report.PKlaerfall = true;


                m_Report.Fill(Session["AppID"].ToString(), Session.SessionID.ToString(), this, "ALL", "HM", true);
                Session["ResultTable"] = m_Report.Result;
                Session["ResultTableNative"] = m_Report.getNativeData(); //Alle Spalten merken 
                m_objTable = m_Report.getNativeData();


                if (m_Report.Status != 0)
                {
                    lblError.Text = "Fehler: " + m_Report.Message;
                }
                else
                {
                    if (m_Report.Result.Rows.Count == 0)
                    {
                        lblError.Text = "Keine Ergebnisse für die gewählten Kriterien.";
                    }
                    else
                    {
                        Session["ExcelTable"] = m_Report.Result;
                        FillGrid(0,"");
                    }
                }

            }
            catch (Exception ex)
            {
                lblError.Text = "Beim Erstellen des Reportes ist ein Fehler aufgetreten.<br>(" + ex.Message + ")";
            }
        }


        private void FillGrid(int intPageIndex, string strSort)
        {

            if (m_objTable == null)
            {
                m_objTable = (DataTable)Session["ResultTableNative"];
            }



            if (m_objTable.Rows.Count == 0)
            {
                GridView1.Visible = false;
                lblNoData.Visible = true;
                lblNoData.Text = "Keine Daten zur Anzeige gefunden.";
                //ShowScript.Visible = false;
            }
            else
            {

                GridView1.Visible = true;
                lblNoData.Visible = false;

                DataView tmpDataView = new DataView();
                tmpDataView = m_objTable.DefaultView;

                int intTempPageIndex = intPageIndex;
                string strTempSort = "";
                string strDirection = "";

                if (strSort.Trim().Length > 0)
                {
                    intTempPageIndex = 0;
                    strTempSort = strSort.Trim();
                    if (ViewState["Sort"] == null || ViewState["Sort"].ToString() == strTempSort)
                    {
                        if (ViewState["Direction"] == null)
                        {
                            strDirection = "desc";
                        }
                        else
                        {
                            strDirection = ViewState["Direction"].ToString();
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

                    ViewState["Sort"] = strTempSort;
                    ViewState["Direction"] = strDirection;
                }
                else
                {
                    if (ViewState["Sort"] != null)
                    {
                        strTempSort = ViewState["Sort"].ToString();
                        if (ViewState["Direction"] == null)
                        {
                            strDirection = "asc";
                            ViewState["Direction"] = strDirection;
                        }
                        else
                        {
                            strDirection = ViewState["Direction"].ToString();
                        }
                    }
                }

                if (strTempSort.Length != 0)
                {
                    tmpDataView.Sort = strTempSort + " " + strDirection;
                }

                GridView1.PageIndex = intTempPageIndex;
                GridView1.DataSource = tmpDataView;
                GridView1.DataBind();


                GridView1.Columns[1].Visible = false;     //Equipmentnr ausblenden
                GridView1.Columns[GridView1.Columns.Count - 2].Visible = false;     //Klärfallspalte am Ende ausblenden
                GridView1.Columns[GridView1.Columns.Count - 1].Visible = false;     //KlärfallspalteInfo am Ende ausblenden


                if (Session["ShowOtherString"] != null && Session["ShowOtherString"].ToString().Length > 0)
                {
                    lblNoData.Text = Session["ShowOtherString"].ToString();
                }
                else
                {
                    lblNoData.Text = "Es wurden " + tmpDataView.Count.ToString() + " Einträge zu '" +
                        m_User.Applications.Select("AppID = '" + Session["AppID"].ToString() + "'")[0]["AppFriendlyName"].ToString() + "' gefunden.";
                }
                //if (Session["BackLink"] != null && Session["BackLink"].ToString() == "HistoryBack")
                //{
                //    lnkZurueck.Text = "Zurück";
                //    lnkZurueck.NavigateUrl = "javascript:history.back()";
                //}
                lblNoData.Visible = true;


            }
        }

        private void Page_PreRender(object sender, EventArgs e) //Handles MyBase.PreRender
        {
            Common.SetEndASPXAccess(this);
            HelpProcedures.FixedGridViewCols(GridView1);
        }

        private void Page_Unload(object sender, EventArgs e) //Handles MyBase.Unload
        {
            Common.SetEndASPXAccess(this);

        }

        private void GridView1_PageIndexChanged(Int32 pageindex)
        {
            FillGrid(pageindex, "");
        }

        private void GridView1_ddlPageSizeChanged()
        {
            FillGrid(0, "");
        }

        protected void lbBack_Click(object sender, EventArgs e)
        {
            Response.Redirect("/Services/(S(" + Session.SessionID + "))/Start/Selection.aspx");
        }

        protected void lnkCreateExcel1_Click(object sender, EventArgs e)
        {
            Control control = new Control();
            DataTable tblTranslations = new DataTable();
            DataTable tblTemp = (DataTable)Session["ExcelTable"];
            string AppURL = null;
            DataColumn col2 = null;
            int bVisibility = 0;
            int i = 0;
            string sColName = "";
            AppURL = this.Request.Url.LocalPath.Replace("/Services", "..");
            tblTranslations = (DataTable)this.Session[AppURL];
            foreach (DataControlField col in GridView1.Columns)
            {
                for (i = tblTemp.Columns.Count - 1; i >= 0; i += -1)
                {
                    bVisibility = 0;
                    col2 = tblTemp.Columns[i];
                    if (col2.ColumnName.ToUpper() == col.SortExpression.ToUpper())
                    {
                        sColName = Common.TranslateColLbtn(GridView1, tblTranslations, col.HeaderText, ref bVisibility);
                        if (bVisibility == 0)
                        {
                            tblTemp.Columns.Remove(col2);
                        }
                        else if (sColName.Length > 0)
                        {
                            col2.ColumnName = sColName;
                        }
                    }
                }
                tblTemp.AcceptChanges();
            }
            CKG.Base.Kernel.DocumentGeneration.ExcelDocumentFactory excelFactory = new CKG.Base.Kernel.DocumentGeneration.ExcelDocumentFactory();
            string filename = String.Format("{0:yyyyMMdd_HHmmss_}", System.DateTime.Now) + m_User.UserName;
            excelFactory.CreateDocumentAndSendAsResponse(filename, tblTemp, this.Page, false, null, 0, 0);
        }

        protected void GridView1_RowCommand(object sender, GridViewCommandEventArgs e)
        {

            if (e.CommandName.ToUpper() != "SORT")
            {
                DataTable tblData;
                DataRow selectedRow;
                tblData = (DataTable)Session["ResultTableNative"];
                hfEquinr.Value = e.CommandArgument.ToString();
                selectedRow = tblData.Select("Equipment='" + hfEquinr.Value + "'")[0];
                lblLVNr.Text = selectedRow["LVNr"].ToString();
                ModalPopupExtender.Show();
            
            }

            
        }


        protected void GridView1_Sorting(object sender, GridViewSortEventArgs e)
        {
            FillGrid(GridView1.PageIndex, e.SortExpression);
        }

        protected void cmdSave_Click(object sender, EventArgs e)
        {
            String status = "";
            if (checkDate())
            {
                sendMail(ref status);
                if (status != String.Empty)
                {
                    lblError.Text = status;
                    ModalPopupExtender.Show();
                }
                else
                {
                    lblError.Text = "Vorgang erfolgreich.";

                }
            }
            else
            {
                lblMessagePopUp.Text = "Keine gültigen Eingabedaten.(z.B. Datumsformat)";
                ModalPopupExtender.Show();
            }
        }
        protected Boolean checkDate()
        {
            Boolean blnResult = false;

            if (txtBemerkung.Text.Trim() != String.Empty)
            {
                blnResult = true;
            }
            if (IsDate(txtDatum.Text))
            {
                blnResult = true;
            }
            if (cbxEnt.Checked)
            { blnResult = true; }

            if (cbxFahrz.Checked)
            { blnResult = true; }

            if (cbxSB.Checked)
            { blnResult = true; }

            if (cbxVers.Checked)
            { blnResult = true; }

            return blnResult;

        }

        protected void sendMail(ref String status)
        {
            System.Net.Mail.MailMessage mail;
            String strMailAdresse;
            String strMailBody;
            status = "";
            try
            {
                strMailAdresse = ConfigurationManager.AppSettings["SmtpMailAddress"];
                strMailBody = "Benutzername: " + m_User.UserName + "\r\n";

                strMailBody += "LV-Nr.: " + lblLVNr.Text + "\r\n";
                strMailBody += "LV beendet zum: " + txtDatum.Text + "\r\n";
                strMailBody += "SB ist in Ordnung: ";

                if (cbxSB.Checked)
                {
                    strMailBody += "(X)";
                }
                else
                {
                    strMailBody += "()";
                }

                strMailBody += "\r\nHöhe der Entschädigung im Schadensfall ist in Ordnung: ";

                if (cbxEnt.Checked)
                {
                    strMailBody += "(X)";
                }
                else
                {
                    strMailBody += "()";
                }

                strMailBody += "\r\nVersichererwechsel: ";
                if (cbxVers.Checked)
                {
                    strMailBody += "(X)";
                }
                else
                {
                    strMailBody += "()";
                }

                strMailBody += "\r\nFahrzeugwechsel: ";
                if (cbxFahrz.Checked)
                {
                    strMailBody += "(X)";
                }
                else
                {
                    strMailBody += "()";
                }

                strMailBody += "Sonstiges: " + txtBemerkung.Text + "\r\n";

                mail = new System.Net.Mail.MailMessage(ConfigurationManager.AppSettings["SmtpMailSender"], strMailAdresse,  //
                                                    "Klärfallmail Leaseplan", strMailBody);

                System.Net.Mail.SmtpClient client = new System.Net.Mail.SmtpClient(ConfigurationManager.AppSettings["SmtpMailServer"]);
                client.Send(mail);
            }
            catch (Exception Ex)
            {
                status = "Fehler beim Versenden der Mail." + Ex.Message;
                throw;
            }

        }
        public static bool IsDate(string strDate)
        {
            if (strDate == null)
            {
                strDate = "";
            }
            if (strDate.Length > 0)
            {
                DateTime dummyDate;
                try
                {
                    dummyDate = DateTime.Parse(strDate);
                }
                catch
                {
                    return false;
                }
                return true;
            }
            else
            {
                return false;
            }
        }

        


    }

}
