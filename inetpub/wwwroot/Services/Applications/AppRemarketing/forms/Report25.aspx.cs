using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.UI;
using System.Web.UI.WebControls;
using CKG.Base.Kernel.Common;
using AppRemarketing.lib;
using CKG.Base.Kernel.Security;
using System.Configuration;
using System.Data;
using GeneralTools.Models;
using GeneralTools.Services;
using SmartSoft.PdfLibrary;
using Telerik.Web.UI;
using Telerik.Web.UI.GridExcelBuilder;

namespace AppRemarketing.forms
{
    public partial class Report25 : Page
    {
        private User m_User;
        private App m_App;
        private bool isExcelExportConfigured;
        private Belastungsanzeigen2 m_Report;

        protected void Page_Load(object sender, EventArgs e)
        {
            m_User = Common.GetUser(this);
            Common.FormAuth(this, m_User);

            m_App = new App(m_User);
            Common.GetAppIDFromQueryString(this);

            lblHead.Text = (string)m_User.Applications.Select("AppID = '" + Session["AppID"] + "'")[0]["AppFriendlyName"];
            lblError.Text = "";
            Literal1.Text = "";

            try
            {
                String strFileName = String.Format("{0:yyyyMMdd_HHmmss_}", DateTime.Now) + m_User.UserName + ".xls";
                m_Report = Session["Belastungsanzeigen2"] as Belastungsanzeigen2 ?? new Belastungsanzeigen2(ref m_User, m_App, (string)Session["AppID"], Session.SessionID, strFileName);
                m_Report.SessionID = Session.SessionID;
                m_Report.AppID = (string)Session["AppID"];
                Session["Belastungsanzeigen2"] = m_Report;

                if (!IsPostBack)
                {
                    Common.TranslateTelerikColumns(rgGrid1);
                    Common.TranslateTelerikColumns(rgGrid2);

                    var persister = new GridSettingsPersister(rgGrid1, GridSettingsType.All);
                    Session["rgGrid1_original"] = persister.LoadForUser(m_User, (string)Session["AppID"], GridSettingsType.All.ToString());

                    FillDate();
                    FillVermieter();
                    FillHC();
                    FillStatus();
                    if (!IsAV())
                    {
                        tr_Vermieter.Visible = true;
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
            if ((txtKennzeichen.Text.Length == 0) && (txtFahrgestellnummer.Text.Length == 0))
            {
                if (txtVertragsjahr.Text.Length < 4)
                {
                    if ((txtDatumVon.Text.Length == 0) || (txtDatumBis.Text.Length == 0))
                    {
                        lblError.Text = "Bitte geben Sie einen Zeitraum für Ihre Selektion an.";
                        return;
                    }
                }
            }

            if (txtVertragsjahr.Text.Length < 4)
            {
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

            m_Report.AVNR = ddlVermieter.SelectedValue;
            m_Report.CarportNr = ddlHC.SelectedValue;
            m_Report.Kennzeichen = txtKennzeichen.Text;
            m_Report.Fahrgestellnummer = txtFahrgestellnummer.Text;
            m_Report.Vertragsjahr = txtVertragsjahr.Text;

            if (ddlStatus.SelectedValue != "00")
            {
                m_Report.AuswahlStatus = ddlStatus.SelectedValue;
            }
            else
            {
                m_Report.AuswahlStatus = null;
            }

            if (!IsAV())
            {
                if (ddlVermieter.SelectedValue != "00")
                {
                    m_Report.AVNR = ddlVermieter.SelectedValue;
                }
                else
                {
                    m_Report.Vermieter = null;
                }
            }
            else
            {
                m_Report.AVNR = m_User.Groups[0].GroupName.ToString();
            }

            m_Report.DatumVon = txtDatumVon.Text;
            m_Report.DatumBis = txtDatumBis.Text;

            m_Report.Show((string)Session["AppID"], Session.SessionID, this);

            if (m_Report.Status == 0)
            {
                Session["Belastungsanzeigen2"] = m_Report;
                Fillgrid();
            }
            else
            {
                lblError.Text = m_Report.Message;
            }
        }

        private void Fillgrid()
        {
            rgGrid2.Visible = false;
            cmdBlock.Visible = false;
            cmdNoBlock.Visible = false;
            cmdEdit.Visible = false;
            cmdSetOpen.Visible = false;

            if (m_Report.Result.Rows.Count == 0)
            {
                SearchMode();
                lblError.Text = "Keine Dokumente zur Anzeige gefunden.";
            }
            else
            {
                SearchMode(false);

                rgGrid1.Rebind();
                //Setzen der DataSource geschieht durch das NeedDataSource-Event

                if (!IsAV())
                {
                    rgGrid1.Columns[1].Visible = false;
                    if (ddlStatus.SelectedValue != "00" && ddlStatus.SelectedValue != "4")
                    {
                        if (ddlStatus.SelectedValue == "1")
                        {
                            cmdSetOpen.Visible = true;
                            rgGrid1.Columns[0].Visible = true;
                        }
                        else if (ddlStatus.SelectedValue != "9")
                        {
                            cmdBlock.Visible = true;
                            rgGrid1.Columns[0].Visible = true;
                        }
                        else
                        {
                            cmdNoBlock.Visible = true;
                            rgGrid1.Columns[0].Visible = true;
                        }
                    }
                    else
                    {
                        rgGrid1.Columns[0].Visible = false;
                    }
                }
                else
                {
                    rgGrid1.Columns[0].Visible = false;
                    if (ddlStatus.SelectedValue == "0")
                    {
                        cmdEdit.Visible = true;
                        rgGrid1.Columns[0].Visible = true;
                    }
                }
            }
        }

        private void SearchMode(bool search = true)
        {
            NewSearch.Visible = !search;
            NewSearchUp.Visible = search;
            Panel1.Visible = search;
            lbCreate.Visible = search;
            Result.Visible = !search;
            dataFooter.Visible = !search;
        }

        protected void rgGrid1_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (m_Report.Result != null)
            {
                rgGrid1.DataSource = m_Report.Result.DefaultView;
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
            Session["Belastungsanzeigen2"] = null;
            Response.Redirect("/Services/(S(" + Session.SessionID + "))/Start/Selection.aspx");
        }

        private bool IsAV()
        {
            return m_User.Groups[0].GroupName.ToString().Substring(0, 2) == "AV";
        }

        private void FillVermieter()
        {
            Fahrzeugbestand mVermieter = new Fahrzeugbestand(ref m_User, m_App, (string)Session["AppID"], Session.SessionID, "");
            m_Report = new Belastungsanzeigen2(ref m_User, m_App, (string)Session["AppID"], Session.SessionID, "");

            mVermieter.getVermieter((string)Session["AppID"], Session.SessionID, this);

            if (mVermieter.Status > 0)
            {
                lblError.Text = mVermieter.Message;
                lblError.Visible = true;
            }
            else
            {
                if (mVermieter.Vermieter.Rows.Count > 0)
                {
                    m_Report.Vermieter = mVermieter.Vermieter;

                    Session["Vorschaden"] = m_Report;

                    ddlVermieter.Items.Add(new ListItem("- alle -", "00"));

                    foreach (DataRow drow in mVermieter.Vermieter.Rows)
                    {
                        ddlVermieter.Items.Add(new ListItem((string)drow["POS_KURZTEXT"] + " " + (string)drow["POS_TEXT"], (string)drow["POS_KURZTEXT"]));
                    }
                }
            }
        }

        private void FillHC()
        {
            HC mHC = new HC(ref m_User, m_App, (string)Session["AppID"], Session.SessionID, "");

            mHC.getHC((string)Session["AppID"], Session.SessionID, this);

            if (mHC.Status > 0)
            {
                lblError.Text = mHC.Message;
                lblError.Visible = true;
            }
            else
            {
                if (mHC.Hereinnahmecenter.Rows.Count > 0)
                {
                    ddlHC.Items.Add(new ListItem("- alle -", "00"));

                    foreach (DataRow drow in mHC.Hereinnahmecenter.Rows)
                    {
                        ddlHC.Items.Add(new ListItem((string)drow["POS_KURZTEXT"] + " " + (string)drow["POS_TEXT"], (string)drow["POS_KURZTEXT"]));
                    }
                }
            }
        }

        private void FillStatus()
        {
            ddlStatus.Items.Clear();

            var statusListe = new[] { 
                new { Text = "- alle -",            Value = "00" }, 
                new { Text = "Offen",               Value = "0" },
                new { Text = "Freigabe",            Value = "1"},
                new { Text = "Widersprochen",       Value = "2" },
                IsAV()?new { Text = "In Bearbeitung", Value = "3" }:null,
                new { Text = "Abgerechnet",         Value = "4" },
                new { Text = "Nicht abgerechnet",   Value = "5" },
                new { Text = "Blockiert",           Value = "9"}
            };

            foreach (var status in statusListe)
            {
                if (status != null)
                    ddlStatus.Items.Add(new ListItem(status.Text, status.Value));
            }
        }

        protected void lbtnBack_Click(object sender, EventArgs e)
        {
            rgGrid1.Visible = true;
            rgGrid2.Visible = false;
            lbtnBack.Visible = false;
            divGutachtenHeader.Visible = false;
        }

        protected void btnOK_Click(object sender, EventArgs e)
        {
            if (txtBeschreibung.Text.Length > 255)
            {
                lblSaveInfo.Visible = true;
                lblSaveInfo.ForeColor = System.Drawing.Color.Red;
                lblSaveInfo.Text = "Eingabe überschreitet 255 Zeichen.";
                ModalPopupExtender2.Show();
                return;
            }

            if (txtBeschreibung.Text.Length == 0)
            {
                lblSaveInfo.Visible = true;
                lblSaveInfo.ForeColor = System.Drawing.Color.Red;
                lblSaveInfo.Text = "Bitte geben Sie eine Beschreibung ein.";
                ModalPopupExtender2.Show();
            }
            else
            {
                m_Report.Reklamationstext = txtBeschreibung.Text;
                m_Report.Sachbearbeiter = txtSachbearbeiter.Text;
                m_Report.Telefon = txtTelefon.Text;
                m_Report.Mail = txtMail.Text;

                m_Report.setReklamation((string)Session["AppID"], Session.SessionID, this.Page);

                if (m_Report.TableReklamation.Rows.Count > 0)
                {
                    lblSaveInfo.ForeColor = System.Drawing.Color.Blue;
                    lblSaveInfo.Visible = true;
                    btnCancel.Text = "Schließen";
                    btnOK.Width = 0;
                    ModalPopupExtender2.Show();
                    lblSaveInfo.Text = "Die Reklamation wurde gespeichert.";

                    foreach (GridDataItem item in rgGrid1.Items)
                    {
                        HyperLink lnkFin = (HyperLink)item.FindControl("lnkHistorie");
                        ImageButton ibtn = (ImageButton)item.FindControl("ibtnRekla");

                        if (item["STATU"].Text == "0" && lnkFin.Text == m_Report.FinGutachten || item["STATU"].Text == "3" && lnkFin.Text == m_Report.FinGutachten)
                        {
                            DataRow SelRow = m_Report.Result.Select("FAHRGNR = '" + lnkFin.Text + "' and STATU = 0")[0];

                            SelRow["STATU"] = "2";
                            SelRow["DDTEXT"] = "Widersprochen";

                            ibtn.Visible = false;
                            item["STATU"].Text = "2";
                            item["DDTEXT"].Text = "Widersprochen";

                            break;
                        }
                    }

                    ModalPopupExtender2.Show();
                }
                else
                {
                    lblSaveInfo.Visible = true;
                    lblSaveInfo.ForeColor = System.Drawing.Color.Red;
                    lblSaveInfo.Text = "Fehler beim Speichern der Reklamation.";
                    ModalPopupExtender2.Show();
                }
            }
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            ModalPopupExtender2.Show();
        }

        protected void InfoPopupOpener_Click(object sender, EventArgs e)
        {
            InfoPopup.Show();
        }

        protected void OpenBlockPopup_Click(object sender, EventArgs e)
        {
            BlockPopup.Show();
        }

        protected void BlockAcceptClick(object sender, EventArgs e)
        {
            ChangeSelectedRows("9", BlockText.Text);
        }

        protected void cmdBlock_Click(object sender, EventArgs e)
        {
            UpdateTableGrid();

            DataRow[] dRows = m_Report.Result.Select("Auswahl = '1'");

            if (dRows.Length > 0)
            {
                BlockText.Text = string.Empty;
                BlockHeader.Text = string.Format("Bitte den Blockadegrund für {0} {1} angeben:",
                    dRows.Length == 1 ? "das" : "die " + dRows.Length.ToString(),
                    dRows.Length == 1 ? "Fahrzeug" : "Fahrzeuge");
                BlockPopup.Show();
            }
            else
            {
                lblError.Text = "Bitte wählen Sie min. ein Fahrzeug aus!";
                lblError.Visible = true;
            }
        }

        private void UpdateTableGrid()
        {
            foreach (GridDataItem item in rgGrid1.Items)
            {
                HyperLink lnkHistorie = (HyperLink)item.FindControl("lnkHistorie");

                CheckBox chkAuswahl = (CheckBox)item.FindControl("chkAuswahl");

                DataRow[] dRows = m_Report.Result.Select("FAHRGNR = '" + lnkHistorie.Text + "'");

                if (dRows.Length == 1)
                {
                    if (chkAuswahl.Checked)
                    {
                        dRows[0]["Auswahl"] = "1";
                    }
                    else
                    {
                        dRows[0]["Auswahl"] = "0";
                    }
                }

            }
            Session["Belastungsanzeigen2"] = m_Report;
        }

        protected void cmdNoBlock_Click(object sender, EventArgs e)
        {
            ChangeSelectedRows("0");
        }

        protected void cmdEdit_Click(object sender, EventArgs e)
        {
            ChangeSelectedRows("3");
        }

        protected void cmdSetOpen_Click(object sender, EventArgs e)
        {
            UpdateTableGrid();

            DataRow[] dRows = m_Report.Result.Select("Auswahl = '1'");

            if (dRows.Length > 0)
            {
                m_Report.setOpen((string)Session["AppID"], Session.SessionID, this);
                if (m_Report.Status != 0)
                {
                    lblError.Text = m_Report.Message;
                    lblError.Visible = true;
                }
                else
                {
                    foreach (DataRow item in dRows)
                    {
                        m_Report.Result.Rows.Remove(item);
                    }
                    Session["Belastungsanzeigen2"] = m_Report;
                    Fillgrid();
                }
            }
            else
            {
                lblError.Text = "Bitte wählen Sie min. ein Fahrzeug aus!";
            }
        }

        private void ChangeSelectedRows(string newStatus, string blockText = null)
        {
            UpdateTableGrid();

            DataRow[] dRows = m_Report.Result.Select("Auswahl = '1'");

            if (dRows.Length > 0)
            {
                m_Report.Change((string)Session["AppID"], Session.SessionID, this, newStatus, blockText);
                if (m_Report.Status != 0)
                {
                    lblError.Text = m_Report.Message;
                    lblError.Visible = true;
                }
                else
                {
                    foreach (DataRow item in dRows)
                    {
                        m_Report.Result.Rows.Remove(item);
                    }
                    Session["Belastungsanzeigen2"] = m_Report;
                    Fillgrid();
                }
            }
            else
            {
                lblError.Text = "Bitte wählen Sie min. ein Fahrzeug aus!";
            }
        }

        private void FillDate()
        {
            txtDatumVon.Text = Helper.DateFrom;
            txtDatumBis.Text = Helper.DateTo;
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
                    eSettings.FileName = string.Format("Belastungsanzeigen_{0:yyyyMMdd}", DateTime.Now);
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

                case "Show":
                    m_Report.FinGutachten = e.CommandArgument.ToString();

                    m_Report.getGutachten((string)Session["AppID"], Session.SessionID, this.Page);

                    if (m_Report.Gutachten.Rows.Count > 0)
                    {
                        rgGrid1.Visible = false;
                        rgGrid2.Visible = true;
                        Result.Visible = true;
                        lbtnBack.Visible = true;
                        divGutachtenHeader.Visible = true;

                        rgGrid2.Rebind();
                        //Setzen der DataSource geschieht durch das NeedDataSource-Event
                    }
                    else
                    {
                        Result.Visible = false;
                        lblError.Text = "Keine Gutachtendaten gefunden.";
                        lblError.Visible = true;
                    }
                    break;

                case "Rekla":
                    m_Report.FinGutachten = e.CommandArgument.ToString();

                    Session["Belastungsanzeigen2"] = m_Report;

                    lblSaveInfo.Visible = false;
                    btnCancel.Text = "Schließen";
                    btnOK.Width = new Unit("90px");
                    lblSaveInfo.Text = "";
                    txtBeschreibung.Text = "";
                    ModalPopupExtender2.Show();
                    break;

                case "ShowReklamation":
                    var reklaText = m_Report.ReadReklamationstext((string)Session["AppID"], Session.SessionID, this.Page, e.CommandArgument as string);

                    InfoPopupHeader.Text = "Reklamationstext";

                    if (!string.IsNullOrEmpty(reklaText))
                    {
                        InfoPopupText.Text = reklaText;
                        InfoPopupText.CssClass = string.Empty;
                    }
                    else
                    {
                        InfoPopupText.Text = "Kein Reklamationstext angegeben.";
                        InfoPopupText.CssClass = "TextError";
                    }

                    InfoPopup.Show();
                    break;

                case "PDF":
                    QuickEasy.Documents qe = new QuickEasy.Documents(".1001=" + e.CommandArgument.ToString(),
                        ConfigurationManager.AppSettings["EasyRemoteHosts"].ToString(),
                        60, ConfigurationManager.AppSettings["EasySessionId"],
                        ConfigurationManager.AppSettings["ExcelPath"].ToString(),
                        "C:\\TEMP",
                        "SYSTEM",
                        ConfigurationManager.AppSettings["EasyPwdClear"].ToString(),
                        "C:\\TEMP",
                        "AUDI",
                        "AUDI" + DateTime.Now.ToString("yy"),
                        "SGW");

                    qe.GetDocument();

                    if (qe.ReturnStatus == 2)
                    {
                        Helper.GetPDF(this, qe.path, "Belastungsanzeige_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".pdf");
                    }
                    else
                    {
                        lblError.Text = "Das Dokument wurde nicht gefunden.";
                    }
                    break;

                case "BlockText":
                    var blockText = m_Report.ReadBlockadetext((string)Session["AppID"], Session.SessionID, this.Page, e.CommandArgument as string);

                    InfoPopupHeader.Text = "Blockadegrund";
                    if (!string.IsNullOrEmpty(blockText))
                    {
                        InfoPopupText.Text = blockText;
                        InfoPopupText.CssClass = string.Empty;
                    }
                    else
                    {
                        InfoPopupText.Text = "Kein Blockadegrund angegeben.";
                        InfoPopupText.CssClass = "TextError";
                    }

                    InfoPopup.Show();
                    break;

                case "RepKalk":
                    var cmdArgs = e.CommandArgument.ToString().Split('_');
                    var fin = cmdArgs[0];
                    var anz = cmdArgs[1];

                    ShowRepKalk(fin, anz.ToInt(0));

                    break;
            }
        }

        protected void rgGrid1_ItemDataBound(object sender, GridItemEventArgs e)
        {
            if (m_User.Applications.Select("AppName = 'Report14'").Length > 0)
            {
                string strHistoryLink = "Report14.aspx?AppID=" + m_User.Applications.Select("AppName = 'Report14'")[0]["AppID"].ToString() + "&VIN=";

                if (e.Item is GridDataItem)
                {
                    GridDataItem item = e.Item as GridDataItem;
                    HyperLink lnkFahrgestellnummer = (HyperLink)item.FindControl("lnkHistorie");
                    if (lnkFahrgestellnummer != null)
                    {
                        lnkFahrgestellnummer.NavigateUrl = strHistoryLink + lnkFahrgestellnummer.Text;
                    }
                }
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

        protected void rgGrid2_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (m_Report.Gutachten != null)
            {
                rgGrid2.DataSource = m_Report.Gutachten.DefaultView;
            }
            else
            {
                rgGrid2.DataSource = null;
            }
        }

        private void ShowRepKalk(string fin, int anz)
        {
            var repKalkUrl = ApplicationConfiguration.GetApplicationConfigValue("RepKalkUrl", "0", m_User.Customer.CustomerId, m_User.GroupID);
            var filesByte = new List<byte[]>();
            
            using (var clnt = new WebClient())
            {
                for (var i = 0; i < anz; i++)
                {
                    var downloadUrl = String.Format("{0}?fin={1}&nummer={2}", repKalkUrl, fin, (i + 1).ToString());
                    var downloadFile = clnt.DownloadData(downloadUrl);
                    if (downloadFile != null)
                        filesByte.Add(downloadFile);
                }
            }

            if (filesByte.Count == 0)
            {
                lblError.Text = "Das Dokument wurde nicht gefunden.";
                return;
            }

            var pdfBytes = PdfMerger.MergeFiles(filesByte, false);

            Response.Clear();
            Response.ClearContent();
            Response.ClearHeaders();
            Response.ContentType = "application/pdf";
            Response.AddHeader("Content-Disposition", "attachment; filename=" + "RepKalk_" + fin + ".pdf");
            Response.AddHeader("Expires", "0");
            Response.AddHeader("Pragma", "cache");
            Response.AddHeader("Cache-Control", "private");
            Response.BinaryWrite(pdfBytes);
            if (Response.IsClientConnected)
            {
                Response.Flush();
            }
            Response.End();
        }
    }
}
