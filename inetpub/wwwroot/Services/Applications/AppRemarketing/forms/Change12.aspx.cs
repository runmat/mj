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
        private User _mUser;
        private App _mApp;
        private bool _isExcelExportConfigured;
        private Carport _mReport;
        private Belastungsanzeigen _mNachbelastung;

        protected void Page_Load(object sender, EventArgs e)
        {
            _mUser = Common.GetUser(this);
            Common.FormAuth(this, _mUser);

            _mApp = new App(_mUser);
            Common.GetAppIDFromQueryString(this);

            lblHead.Text = (string)_mUser.Applications.Select("AppID = '" + Session["AppID"] + "'")[0]["AppFriendlyName"];
            lblError.Text = "";

            try
            {
                if (!IsPostBack)
                {
                    Common.TranslateTelerikColumns(rgGrid1);

                    var persister = new GridSettingsPersister(rgGrid1, GridSettingsType.All);
                    Session["rgGrid1_original"] = persister.LoadForUser(_mUser, (string)Session["AppID"], GridSettingsType.All.ToString());

                    String strFileName = String.Format("{0:yyyyMMdd_HHmmss_}", DateTime.Now) + _mUser.UserName + ".xls";

                    // String strFileName; // = Format(Now, "yyyyMMdd_HHmmss_") & m_User.UserName & ".xls";
                    _mReport = new Carport(ref _mUser, _mApp, (string)Session["AppID"], Session.SessionID, strFileName);
                    Session.Add("Dropdowns", _mReport);
                    _mReport.SessionID = this.Session.SessionID;
                    _mReport.AppID = (string)Session["AppID"];

                    _mNachbelastung = new Belastungsanzeigen(ref _mUser, _mApp, (string)Session["AppID"], Session.SessionID, strFileName);
                    Session.Add("Nachbelastung", _mNachbelastung);
                    _mNachbelastung.SessionID = this.Session.SessionID;
                    _mNachbelastung.AppID = (string)Session["AppID"];

                    FillDate();
                    FillVermieter(); 
                    FillEmpfaenger();
                    FillHc();
                }
                else
                {
                    if ((Session["Dropdowns"] != null))
                    {
                        _mReport = (Carport)Session["Dropdowns"];
                    }
                    if ((Session["Nachbelastung"] != null))
                    {
                        _mNachbelastung = (Belastungsanzeigen)Session["Nachbelastung"];
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

        protected void LbCreateClick(object sender, EventArgs e)
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
                DateTime dateFrom = DateTime.Parse(txtDatumVon.Text).Date;
                DateTime dateTo = DateTime.Parse(txtDatumBis.Text).Date;

                if (dateTo < dateFrom)
                {
                    lblError.Text = "Datum von ist größer als Datum bis.";
                    return;
                }
            }

            _mNachbelastung.CarportNr = ddlHC.SelectedValue;
            _mNachbelastung.AVNR = ddlVermieter.SelectedValue;
            _mNachbelastung.Gutachter = ddlGutachter.SelectedValue;
            _mNachbelastung.Kennzeichen = txtKennzeichen.Text;
            _mNachbelastung.Fahrgestellnummer = txtFahrgestellnummer.Text;
            _mNachbelastung.Rechnungsnummer = txtRechnungsnummer.Text;
            _mNachbelastung.Inventarnummer = txtInventarnummer.Text;
            _mNachbelastung.DatumVon = txtDatumVon.Text;
            _mNachbelastung.DatumBis = txtDatumBis.Text;

            _mNachbelastung.ShowNachbelastung((string)Session["AppID"], Session.SessionID, this);

            if (_mNachbelastung.Status != 0)
            {
                lblError.Text = _mNachbelastung.Message;
            }
            else
            {
                Session["Nachbelastung"] = _mNachbelastung;
                Fillgrid();
            }
        }

        private void Fillgrid()
        {
            if (_mNachbelastung.Result.Rows.Count == 0)
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

        protected void RgGrid1NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (_mNachbelastung.Result != null)
            {
                rgGrid1.DataSource = _mNachbelastung.Result.DefaultView;
            }
            else
            {
                rgGrid1.DataSource = null;
            }
        }

        protected void NewSearchClick(object sender, ImageClickEventArgs e)
        {
            SearchMode();
        }

        protected void NewSearchUpClick(object sender, ImageClickEventArgs e)
        {
            SearchMode(false);
        }

        protected void LbBackClick(object sender, EventArgs e)
        {
            Session["Nachbelastung"] = null;
            Response.Redirect("/Services/(S(" + Session.SessionID + "))/Start/Selection.aspx");
        }

        private void FillHc()
        {
            var mHc = new HC(ref _mUser, _mApp, (string)Session["AppID"], Session.SessionID, "");

            mHc.getHC((string)Session["AppID"], Session.SessionID, this);

            if (mHc.Status > 0)
            {
                lblError.Text = mHc.Message;
            }
            else
            {
                if (mHc.Hereinnahmecenter.Rows.Count > 0)
                {
                    ListItem litHc;
                    litHc = new ListItem {Text = "- alle -", Value = "00"};
                    ddlHC.Items.Add(litHc);

                    foreach (DataRow drow in mHc.Hereinnahmecenter.Rows)
                    {
                        litHc = new ListItem
                            {
                                Text = (string) drow["POS_KURZTEXT"] + " " + (string) drow["POS_TEXT"],
                                Value = (string) drow["POS_KURZTEXT"]
                            };
                        ddlHC.Items.Add(litHc);
                    }
                }
            }
        }

        private void FillVermieter()
        {
            _mReport.getVermieter((string)Session["AppID"], Session.SessionID, this);

            if (_mReport.Status > 0)
            {
                lblError.Text = _mReport.Message;
            }
            else
            {
                if (_mReport.Vermieter.Rows.Count > 0)
                {
                    ListItem litVermiet;
                    litVermiet = new ListItem {Text = "- alle -", Value = "00"};
                    ddlVermieter.Items.Add(litVermiet);

                    foreach (DataRow drow in _mReport.Vermieter.Rows)
                    {
                        litVermiet = new ListItem
                            {
                                Text = (string) drow["POS_KURZTEXT"] + " " + (string) drow["POS_TEXT"],
                                Value = (string) drow["POS_KURZTEXT"]
                            };
                        ddlVermieter.Items.Add(litVermiet);
                    }
                }
            }
        }

        private void FillEmpfaenger()
        {
            _mReport.getEmpfaenger((string)Session["AppID"], Session.SessionID, this);

            if (_mReport.Status > 0)
            {
                lblError.Text = _mReport.Message;
            }
            else
            {
                if (_mReport.Empfaenger.Rows.Count > 0)
                {
                    ListItem litEmpf;
                    litEmpf = new ListItem {Text = "- Auswahl -", Value = "00"};
                    ddlEmpfaenger.Items.Add(litEmpf);

                    foreach (DataRow drow in _mReport.Empfaenger.Rows)
                    {
                        litEmpf = new ListItem {Text = (string) drow["NAME1"], Value = (string) drow["POS_KURZTEXT"]};
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

        protected void Button1Click(object sender, EventArgs e)
        {
            ModalPopupExtender2.Show();
        }

        protected void BtnOkClick(object sender, EventArgs e)
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
                    lblSaveInfo.ForeColor = System.Drawing.Color.Red;
                    lblSaveInfo.Text = "Bitte geben Sie einen Betrag ein.";
                    ModalPopupExtender2.Show();
                    return;
                }

                double dummyDouble;

                if (!Double.TryParse(txtBetrag.Text, out dummyDouble))
                {
                    lblSaveInfo.Visible = true;
                    lblSaveInfo.ForeColor = System.Drawing.Color.Red;
                    lblSaveInfo.Text = "Bitte geben Sie gültigen Betrag ein.";
                    ModalPopupExtender2.Show();
                    return;
                }

                if (trEmpfaenger.Visible)
                {
                    if (ddlEmpfaenger.SelectedValue == "00")
                    {
                        lblSaveInfo.Visible = true;
                        lblSaveInfo.ForeColor = System.Drawing.Color.Red;
                        lblSaveInfo.Text = "Bitte geben Sie einen Empfänger an.";
                        ModalPopupExtender2.Show();
                        return;

                    }
                }

                _mNachbelastung = (Belastungsanzeigen)Session["Nachbelastung"];
                _mNachbelastung.Fahrgestellnummer = lblFin.Text;
                _mNachbelastung.Rechnungsnummer = lblRechnr.Text;
                _mNachbelastung.GutschriftBetrag = txtBetrag.Text;

                if (txtBemerkung.Text.Length > 199)
                {
                    txtBemerkung.Text = txtBemerkung.Text.Substring(0, 199);
                }

                _mNachbelastung.GutschriftBemerkung = txtBemerkung.Text;
                _mNachbelastung.Belegart = "1";

                if (trEmpfaenger.Visible)
                {
                    _mNachbelastung.Belegart = "2";
                    _mNachbelastung.Empfaenger = ddlEmpfaenger.SelectedValue;
                }
                else
                {
                    _mNachbelastung.Empfaenger = "";
                }

                if (cbxMinderwert.Checked)
                {
                    _mNachbelastung.MerkantilerMinderwert = "X";
                }
                else
                {
                    _mNachbelastung.MerkantilerMinderwert = "";
                }


                btnOK.Enabled = false;
                _mNachbelastung.SetGutschrift((string)Session["AppID"], Session.SessionID, this.Page);

                if (_mNachbelastung.Status == 101)
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
                    lblSaveInfo.Text = _mNachbelastung.Message;
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
            persister.SaveForUser(_mUser, (string)Session["AppID"], settingsType.ToString());
        }

        protected void RgGrid1ItemCreated(object sender, GridItemEventArgs e)
        {
            if (e.Item.ItemType == GridItemType.CommandItem)
            {
                var gcitem = e.Item as GridCommandItem;

                if (gcitem != null)
                {
                    var rbutton = gcitem.FindControl("RefreshButton") ?? gcitem.FindControl("RebindGridButton");
                    if (rbutton == null) return;

                    var rbuttonParent = rbutton.Parent;

                    var saveLayoutButton = new Button { ToolTip = "Layout speichern", CommandName = "SaveGridLayout", CssClass = "rgSaveLayout" };
                    rbuttonParent.Controls.AddAt(0, saveLayoutButton);

                    var resetLayoutButton = new Button { ToolTip = "Layout zurücksetzen", CommandName = "ResetGridLayout", CssClass = "rgResetLayout" };
                    rbuttonParent.Controls.AddAt(1, resetLayoutButton);
                }
            }
        }

        protected void RgGrid1ItemCommand(object sender, GridCommandEventArgs e)
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
                    lblFin.Text = e.CommandArgument.ToString().Split('|')[0];
                    lblRechnr.Text = e.CommandArgument.ToString().Split('|')[1];
                    lblSumme.Text = e.CommandArgument.ToString().Split('|')[2];
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
                    lblFin.Text = e.CommandArgument.ToString().Split('|')[0];
                    lblRechnr.Text = e.CommandArgument.ToString().Split('|')[1];
                    lblSumme.Text = e.CommandArgument.ToString().Split('|')[2];
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

        protected void RgGrid1ExcelMlExportRowCreated(object sender, GridExportExcelMLRowCreatedArgs e)
        {
            Helper.radGridExcelMLExportRowCreated(ref _isExcelExportConfigured, ref e);
        }

        protected void RgGrid1ExcelMlExportStylesCreated(object sender, GridExportExcelMLStyleCreatedArgs e)
        {
            Helper.radGridExcelMLExportStylesCreated(ref e);
        }

    }
}
