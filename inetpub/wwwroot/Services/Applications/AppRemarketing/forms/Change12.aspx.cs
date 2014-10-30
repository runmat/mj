using System;
using System.Data;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using CKG.Base.Kernel.Common;
using CKG.Base.Kernel.Security;
using Telerik.Web.UI;
using Telerik.Web.UI.GridExcelBuilder;
using AppRemarketing.lib;

namespace AppRemarketing.forms
{
    public partial class Change12 : Page
    {
        private User m_User;
        private App m_App;
        private bool isExcelExportConfigured;
        private Carport m_Report;
        private Belastungsanzeigen m_Nachbelastung;

        protected void Page_Load(object sender, EventArgs e)
        {
            m_User = Common.GetUser(this);
            Common.FormAuth(this, m_User);

            m_App = new App(m_User);
            Common.GetAppIDFromQueryString(this);

            lblHead.Text = (string)m_User.Applications.Select("AppID = '" + Session["AppID"] + "'")[0]["AppFriendlyName"];
            lblError.Text = "";

            try
            {
                if (!IsPostBack)
                {
                    Common.TranslateTelerikColumns(rgGrid1);

                    var persister = new GridSettingsPersister(rgGrid1, GridSettingsType.All);
                    Session["rgGrid1_original"] = persister.LoadForUser(m_User, (string)Session["AppID"], GridSettingsType.All.ToString());

                    String strFileName = String.Format("{0:yyyyMMdd_HHmmss_}", DateTime.Now) + m_User.UserName + ".xls";

                    // String strFileName; // = Format(Now, "yyyyMMdd_HHmmss_") & m_User.UserName & ".xls";
                    m_Report = new Carport(ref m_User, m_App, (string)Session["AppID"], Session.SessionID, strFileName);
                    Session.Add("Dropdowns", m_Report);
                    m_Report.SessionID = this.Session.SessionID;
                    m_Report.AppID = (string)Session["AppID"];

                    m_Nachbelastung = new Belastungsanzeigen(ref m_User, m_App, (string)Session["AppID"], Session.SessionID, strFileName);
                    Session.Add("Nachbelastung", m_Nachbelastung);
                    m_Nachbelastung.SessionID = this.Session.SessionID;
                    m_Nachbelastung.AppID = (string)Session["AppID"];

                    FillDate();
                    FillVermieter(); 
                    FillEmpfaenger();
                    FillHC();
                }
                else
                {
                    if ((Session["Dropdowns"] != null))
                    {
                        m_Report = (Carport)Session["Dropdowns"];
                    }
                    if ((Session["Nachbelastung"] != null))
                    {
                        m_Nachbelastung = (Belastungsanzeigen)Session["Nachbelastung"];
                    }
                }
            }
            catch
            {
                lblError.Text = "Keine Dokumente zur Anzeige gefunden.";
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
            DoSubmit();
        }

        private void DoSubmit()
        {
            if ((txtKennzeichen.Text.Length == 0) && (txtFahrgestellnummer.Text.Length == 0) && (txtRechnungsnummer.Text.Length == 0) && (txtInventarnummer.Text.Length == 0))
            {
                if ((txtDatumVon.Text.Length == 0) || (txtDatumBis.Text.Length == 0))
                {
                    lblError.Text = "Bitte geben Sie einen Zeitraum für Ihre Selektion an.";
                    return;
                }

                if (((txtDatumVon.Text.Length == 0) && txtDatumBis.Text.Length != 0) || ((txtDatumVon.Text.Length == 0) && (txtDatumBis.Text.Length != 0)))
                {
                    lblError.Text = "Bitte geben Sie einen Zeitraum für Ihre Selektion an.";
                    return;
                }
            }

            if ((txtDatumVon.Text.Length > 0) && (txtDatumBis.Text.Length > 0))
            {
                DateTime DateFrom = DateTime.Parse(txtDatumVon.Text).Date;
                DateTime DateTo = DateTime.Parse(txtDatumBis.Text).Date;

                if (DateTo < DateFrom)
                {
                    lblError.Text = "Datum von ist größer als Datum bis.";
                    return;
                }
            }

            m_Nachbelastung.CarportNr = ddlHC.SelectedValue;
            m_Nachbelastung.AVNR = ddlVermieter.SelectedValue;
            m_Nachbelastung.Gutachter = ddlGutachter.SelectedValue;
            m_Nachbelastung.Kennzeichen = txtKennzeichen.Text;
            m_Nachbelastung.Fahrgestellnummer = txtFahrgestellnummer.Text;
            m_Nachbelastung.Rechnungsnummer = txtRechnungsnummer.Text;
            m_Nachbelastung.Inventarnummer = txtInventarnummer.Text;
            m_Nachbelastung.DatumVon = txtDatumVon.Text;
            m_Nachbelastung.DatumBis = txtDatumBis.Text;

            m_Nachbelastung.ShowNachbelastung((string)Session["AppID"], Session.SessionID, this);

            if (m_Nachbelastung.Status != 0)
            {
                lblError.Text = m_Nachbelastung.Message;
            }
            else
            {
                Session["Nachbelastung"] = m_Nachbelastung;
                Fillgrid();
            }
        }

        private void Fillgrid()
        {
            if (m_Nachbelastung.Result.Rows.Count == 0)
            {
                SearchMode();
                lblError.Text = "Keine Dokumente zur Anzeige gefunden.";
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
            if (m_Nachbelastung.Result != null)
            {
                rgGrid1.DataSource = m_Nachbelastung.Result.DefaultView;
            }
            else
            {
                rgGrid1.DataSource = null;
            }
        }

        protected void NewSearch_Click(object sender, ImageClickEventArgs e)
        {
            SearchMode();
        }

        protected void NewSearchUp_Click(object sender, ImageClickEventArgs e)
        {
            SearchMode(false);
        }

        protected void lbBack_Click(object sender, EventArgs e)
        {
            Session["Nachbelastung"] = null;
            Response.Redirect("/Services/(S(" + Session.SessionID + "))/Start/Selection.aspx");
        }

        private void FillHC()
        {
            HC mHC = new HC(ref m_User, m_App, (string)Session["AppID"], Session.SessionID, "");

            mHC.getHC((string)Session["AppID"], Session.SessionID, this);

            if (mHC.Status > 0)
            {
                lblError.Text = mHC.Message;
            }
            else
            {
                if (mHC.Hereinnahmecenter.Rows.Count > 0)
                {
                    ListItem litHC;
                    litHC = new ListItem();
                    litHC.Text = "- alle -";
                    litHC.Value = "00";
                    ddlHC.Items.Add(litHC);

                    foreach (DataRow drow in mHC.Hereinnahmecenter.Rows)
                    {
                        litHC = new ListItem();
                        litHC.Text = (string)drow["POS_KURZTEXT"] + " " + (string)drow["POS_TEXT"];
                        litHC.Value = (string)drow["POS_KURZTEXT"];
                        ddlHC.Items.Add(litHC);
                    }
                }
            }
        }

        private void FillVermieter()
        {
            m_Report.getVermieter((string)Session["AppID"], (string)Session.SessionID, this);

            if (m_Report.Status > 0)
            {
                lblError.Text = m_Report.Message;
            }
            else
            {
                if (m_Report.Vermieter.Rows.Count > 0)
                {
                    ListItem litVermiet;
                    litVermiet = new ListItem();
                    litVermiet.Text = "- alle -";
                    litVermiet.Value = "00";
                    ddlVermieter.Items.Add(litVermiet);

                    foreach (DataRow drow in m_Report.Vermieter.Rows)
                    {
                        litVermiet = new ListItem();
                        litVermiet.Text = (string)drow["POS_KURZTEXT"] + " " + (string)drow["POS_TEXT"];
                        litVermiet.Value = (string)drow["POS_KURZTEXT"];
                        ddlVermieter.Items.Add(litVermiet);
                    }
                }
            }
        }

        private void FillEmpfaenger()
        {
            m_Report.getEmpfaenger((string)Session["AppID"], (string)Session.SessionID, this);

            if (m_Report.Status > 0)
            {
                lblError.Text = m_Report.Message;
            }
            else
            {
                if (m_Report.Empfaenger.Rows.Count > 0)
                {
                    ListItem litEmpf;
                    litEmpf = new ListItem();
                    litEmpf.Text = "- Auswahl -";
                    litEmpf.Value = "00";
                    ddlEmpfaenger.Items.Add(litEmpf);

                    foreach (DataRow drow in m_Report.Empfaenger.Rows)
                    {
                        litEmpf = new ListItem();
                        litEmpf.Text = (string)drow["NAME1"];
                        litEmpf.Value = (string)drow["POS_KURZTEXT"];
                        ddlEmpfaenger.Items.Add(litEmpf);
                    }
                }
            }
        }

        private void FillDate()
        {
            txtDatumVon.Text = Helper.DateFrom;
            txtDatumBis.Text = Helper.DateTo;
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            ModalPopupExtender2.Show();
        }

        protected void btnOK_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtBemerkung.Text.Length == 0)
                {
                    lblSaveInfo.Visible = true;
                    lblSaveInfo.ForeColor = System.Drawing.Color.Red;
                    lblSaveInfo.Text = "Bitte geben Sie eine Bemerkung ein.";
                    ModalPopupExtender2.Show();
                    return;
                }

                if (txtBetrag.Text.Length == 0)
                {
                    lblSaveInfo.Visible = true;
                    lblSaveInfo.ForeColor = System.Drawing.Color.Red; ;
                    lblSaveInfo.Text = "Bitte geben Sie einen Betrag ein.";
                    ModalPopupExtender2.Show();
                    return;
                }

                double DummyDouble;

                if (!Double.TryParse(txtBetrag.Text, out DummyDouble))
                {
                    lblSaveInfo.Visible = true;
                    lblSaveInfo.ForeColor = System.Drawing.Color.Red; ;
                    lblSaveInfo.Text = "Bitte geben Sie gültigen Betrag ein.";
                    ModalPopupExtender2.Show();
                    return;
                }

                if (trEmpfaenger.Visible)
                {
                    if (ddlEmpfaenger.SelectedValue == "00")
                    {
                        lblSaveInfo.Visible = true;
                        lblSaveInfo.ForeColor = System.Drawing.Color.Red; ;
                        lblSaveInfo.Text = "Bitte geben Sie einen Empfänger an.";
                        ModalPopupExtender2.Show();
                        return;

                    }
                }

                m_Nachbelastung = (Belastungsanzeigen)Session["Nachbelastung"];
                m_Nachbelastung.Fahrgestellnummer = lblFin.Text;
                m_Nachbelastung.Rechnungsnummer = lblRechnr.Text;
                m_Nachbelastung.GutschriftBetrag = txtBetrag.Text;

                if (txtBemerkung.Text.Length > 199)
                {
                    txtBemerkung.Text = txtBemerkung.Text.Substring(0, 199);
                }

                m_Nachbelastung.GutschriftBemerkung = txtBemerkung.Text;
                m_Nachbelastung.Belegart = "1";

                if (trEmpfaenger.Visible)
                {
                    m_Nachbelastung.Belegart = "2";
                    m_Nachbelastung.Empfaenger = ddlEmpfaenger.SelectedValue;
                }
                else
                {
                    m_Nachbelastung.Empfaenger = "";
                }

                if (cbxMinderwert.Checked == true)
                {
                    m_Nachbelastung.MerkantilerMinderwert = "X";
                }
                else
                {
                    m_Nachbelastung.MerkantilerMinderwert = "";
                }


                btnOK.Enabled = false;
                m_Nachbelastung.SetGutschrift((string)Session["AppID"], (string)Session.SessionID, this.Page);

                if (m_Nachbelastung.Status == 101)
                {
                    lblSaveInfo.ForeColor = System.Drawing.Color.Blue;
                    lblSaveInfo.Visible = true;
                    btnCancel.Text = "Schließen";
                    //btnOK.Width = 0;
                    btnOK.Attributes.Add("style", "display:none");

                    ModalPopupExtender2.Show();
                    if (!trEmpfaenger.Visible)
                    {
                        lblSaveInfo.Text = "Die Gutschrift wurde gespeichert.";
                    }
                    else
                    {
                        lblSaveInfo.Text = "Die Nachbelastung wurde gespeichert.";
                    }
                }
                else
                {
                    lblSaveInfo.Visible = true;
                    lblSaveInfo.ForeColor = System.Drawing.Color.Red;
                    lblSaveInfo.Text = m_Nachbelastung.Message;
                    ModalPopupExtender2.Show();
                }
            }
            catch (Exception)
            {
            }
            finally
            {
                btnOK.Enabled = true;
            }
        }

        private void StoreGridSettings(RadGrid grid, GridSettingsType settingsType)
        {
            var persister = new GridSettingsPersister(grid, settingsType);
            persister.SaveForUser(m_User, (string)Session["AppID"], settingsType.ToString());
        }

        protected void rgGrid1_ItemCreated(object sender, GridItemEventArgs e)
        {
            if (e.Item.ItemType == GridItemType.CommandItem)
            {
                var gcitem = e.Item as GridCommandItem;

                var rbutton = gcitem.FindControl("RefreshButton") ?? gcitem.FindControl("RebindGridButton");
                if (rbutton == null) return;

                var rbutton_parent = rbutton.Parent;

                var saveLayoutButton = new Button { ToolTip = "Layout speichern", CommandName = "SaveGridLayout", CssClass = "rgSaveLayout" };
                rbutton_parent.Controls.AddAt(0, saveLayoutButton);

                var resetLayoutButton = new Button { ToolTip = "Layout zurücksetzen", CommandName = "ResetGridLayout", CssClass = "rgResetLayout" };
                rbutton_parent.Controls.AddAt(1, resetLayoutButton);
            }
        }

        protected void rgGrid1_ItemCommand(object sender, GridCommandEventArgs e)
        {
            switch (e.CommandName)
            {
                case RadGrid.ExportToExcelCommandName:
                    var eSettings = rgGrid1.ExportSettings;
                    eSettings.ExportOnlyData = true;
                    eSettings.FileName = string.Format("GutschrNachbel_{0:yyyyMMdd}", DateTime.Now);
                    eSettings.HideStructureColumns = true;
                    eSettings.IgnorePaging = true;
                    eSettings.OpenInNewWindow = true;
                    // hide non display columns from excel export
                    var nonDisplayColumns = rgGrid1.MasterTableView.Columns.OfType<GridEditableColumn>().Where(c => !c.Display).Select(c => c.UniqueName).ToArray();
                    foreach (var col in nonDisplayColumns)
                    {
                        rgGrid1.Columns.FindByUniqueName(col).Visible = false;
                    }
                    rgGrid1.Rebind();
                    rgGrid1.MasterTableView.ExportToExcel();
                    break;

                case "SaveGridLayout":
                    StoreGridSettings(rgGrid1, GridSettingsType.All);
                    break;

                case "ResetGridLayout":
                    var settings = (string)Session["rgGrid1_original"];
                    var persister = new GridSettingsPersister(rgGrid1, GridSettingsType.All);
                    persister.LoadSettings(settings);

                    Fillgrid();
                    break;

                case "Gutschrift":
                    lblFin.Text = e.CommandArgument.ToString().Split('|')[0].ToString();
                    lblRechnr.Text = e.CommandArgument.ToString().Split('|')[1].ToString();
                    lblSumme.Text = e.CommandArgument.ToString().Split('|')[2].ToString();
                    lblAdressMessage.Text = "Gutschrift";
                    trEmpfaenger.Visible = false;
                    txtBetrag.Text = "";
                    txtBemerkung.Text = "";
                    cbxMinderwert.Checked = false;
                    btnOK.Attributes.Add("style", "display:inline");
                    btnCancel.Text = "Abbrechen";

                    ModalPopupExtender2.Show();
                    break;

                case "Nachbelastung":
                    lblFin.Text = e.CommandArgument.ToString().Split('|')[0].ToString();
                    lblRechnr.Text = e.CommandArgument.ToString().Split('|')[1].ToString();
                    lblSumme.Text = e.CommandArgument.ToString().Split('|')[2].ToString();
                    lblAdressMessage.Text = "Nachbelastung";
                    trEmpfaenger.Visible = true;
                    txtBetrag.Text = "";
                    txtBemerkung.Text = "";
                    cbxMinderwert.Checked = false;
                    btnOK.Attributes.Add("style", "display:inline");
                    btnCancel.Text = "Abbrechen";

                    ModalPopupExtender2.Show();
                    break;
            }
        }

        protected void rgGrid1_ExcelMLExportRowCreated(object sender, GridExportExcelMLRowCreatedArgs e)
        {
            Helper.radGridExcelMLExportRowCreated(ref isExcelExportConfigured, ref e);
        }

        protected void rgGrid1_ExcelMLExportStylesCreated(object sender, GridExportExcelMLStyleCreatedArgs e)
        {
            Helper.radGridExcelMLExportStylesCreated(ref e);
        }

    }
}
