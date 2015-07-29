using System;
using System.Globalization;
using System.Web.UI;
using CKG.Base.Kernel.Common;
using CKG.Base.Kernel.Security;
using Telerik.Web.UI;
using System.Configuration;
using AppRemarketing.lib;

namespace AppRemarketing.forms
{
    public partial class Change15 : Page
    {
        private User m_User;
        private App m_App;
        private Schadensgutachten m_Schadensgutachten;

        protected void Page_Load(object sender, EventArgs e)
        {
            m_User = Common.GetUser(this);
            Common.FormAuth(this, m_User);

            m_App = new App(m_User);
            Common.GetAppIDFromQueryString(this);

            lblHead.Text = (string)m_User.Applications.Select("AppID = '" + Session["AppID"] + "'")[0]["AppFriendlyName"];
            lblError.Text = "";

            if (Session["objSchadensgutachten"] != null)
            {
                m_Schadensgutachten = (Schadensgutachten)Session["objSchadensgutachten"];
            }
            else
            {
                m_Schadensgutachten = new Schadensgutachten(ref m_User, m_App, Session["AppID"].ToString(), Session.SessionID, "");
                Session["objSchadensgutachten"] = m_Schadensgutachten;
            }

            if (!IsPostBack)
            {
                txtSelbstvermarkterdatum.Text = DateTime.Today.ToShortDateString();

                Common.TranslateTelerikColumns(rgGrid1);
            }
        }

        private void Page_PreRender(object sender, EventArgs e)
        {
            Common.SetEndASPXAccess(this);
        }

        private void Page_Unload(object sender, EventArgs e)
        {
            Common.SetEndASPXAccess(this);
        }

        protected void lbCreate_Click(object sender, EventArgs e)
        {
            DateTime dummyDate;
            if ((String.IsNullOrEmpty(txtSelbstvermarkterdatum.Text)) || (!DateTime.TryParseExact(txtSelbstvermarkterdatum.Text, "dd.MM.yyyy", CultureInfo.CurrentCulture, DateTimeStyles.None, out dummyDate)))
            {
                lblError.Text = "Bitte geben Sie ein gültiges Datum an!";
                return;
            }

            m_Schadensgutachten.SvArt = rblSelbstvermarktungsart.SelectedValue;
            m_Schadensgutachten.SvDatum = txtSelbstvermarkterdatum.Text;

            m_Schadensgutachten.tblUploads.Clear();

            string ablagePfad = ConfigurationManager.AppSettings["UploadPathVWRGutachtenSelbstvermarkter"];

            foreach (UploadedFile uFile in RadAsyncUpload1.UploadedFiles)
            {
                var nameOhneEndung = uFile.GetNameWithoutExtension();

                var newRow = m_Schadensgutachten.tblUploads.NewRow();
                newRow["FAHRGESTELLNUMMER"] = uFile.GetNameWithoutExtension();

                if ((nameOhneEndung.Length != 17) || (nameOhneEndung.Contains(" ")))
                    newRow["STATUS"] = "Dateiname hat falsches Format";
                else
                    newRow["STATUS"] = "OK";

                m_Schadensgutachten.tblUploads.Rows.Add(newRow);
            }

            m_Schadensgutachten.setUploaddatum(Session["AppID"].ToString(), Session.SessionID, this);

            if (m_Schadensgutachten.Status == 0)
            {
                foreach (UploadedFile uFile in RadAsyncUpload1.UploadedFiles)
                {
                    var dateiname = uFile.GetName();

                    var rows = m_Schadensgutachten.tblUploads.Select("FAHRGESTELLNUMMER='" + uFile.GetNameWithoutExtension() + "' AND STATUS = 'OK'");
                    if (rows.Length > 0)
                    {
                        try
                        {
                            uFile.SaveAs(ablagePfad + dateiname);
                            rows[0]["STATUS"] = "OK";
                        }
                        catch (Exception ex)
                        {
                            rows[0]["STATUS"] = "FEHLER: " + ex.Message;
                        }
                    }
                }
            }
            else
            {
                lblError.Text = "Fehler beim Speichern der Daten in SAP: " + m_Schadensgutachten.Message;
            }

            Session["objSchadensgutachten"] = m_Schadensgutachten;

            Fillgrid();
        }

        private void Fillgrid()
        {
            if ((m_Schadensgutachten.tblUploads == null) || (m_Schadensgutachten.tblUploads.Rows.Count == 0))
            {
                SearchMode();
                lblError.Text = "Keine Dateien zur Anzeige gefunden.";
            }
            else
            {
                SearchMode(false);
                rgGrid1.Rebind();
                //Setzen der DataSource geschieht durch das NeedDataSource-Event
            }
        }

        private void SearchMode(bool search = true)
        {
            NewSearch.Visible = !search;
            NewSearchUp.Visible = search;
            Panel1.Visible = search;
            lbCreate.Visible = search;
            Result.Visible = !search;
        }

        protected void rgGrid1_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (m_Schadensgutachten.tblUploads != null)
            {
                rgGrid1.DataSource = m_Schadensgutachten.tblUploads.DefaultView;
            }
            else
            {
                rgGrid1.DataSource = null;
            }
        }

        protected void lbBack_Click(object sender, EventArgs e)
        {
            Session["SVGutachtenUploadTable"] = null;
            Response.Redirect("/Services/(S(" + Session.SessionID + "))/Start/Selection.aspx");
        }

        protected void NewSearch_Click(object sender, ImageClickEventArgs e)
        {
            SearchMode();
        }

        protected void NewSearchUp_Click(object sender, ImageClickEventArgs e)
        {
            SearchMode(false);
        }

    }
}
