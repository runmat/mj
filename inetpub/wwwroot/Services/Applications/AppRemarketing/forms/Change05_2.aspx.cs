using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CKG.Base.Kernel.Common;
using CKG.Base.Kernel.Security;
using CKG.Base.Business;
using Telerik.Web.UI;
using Telerik.Web.UI.GridExcelBuilder;
using AppRemarketing.lib;

namespace AppRemarketing.forms
{
    public partial class Change05_2 : System.Web.UI.Page
    {
        private CKG.Base.Kernel.Security.User m_User;
        private CKG.Base.Kernel.Security.App m_App;
        private bool isExcelExportConfigured;
        private VersandStorno m_Report;
        private DataTable tblSelection;

        protected void Page_Load(object sender, EventArgs e)
        {
            m_User = Common.GetUser(this);
            Common.FormAuth(this, m_User);

            m_App = new App(m_User); //erzeugt ein App_objekt 
            Common.GetAppIDFromQueryString(this);

            lblHead.Text = (string)m_User.Applications.Select("AppID = '" + Session["AppID"] + "'")[0]["AppFriendlyName"];
            lblError.Text = "Bitte wählen Sie Fahrzeuge zur Änderung aus!";

            try
            {
                if (!IsPostBack)
                {
                    Common.TranslateTelerikColumns(rgGrid1);

                    var persister = new GridSettingsPersister(rgGrid1, GridSettingsType.All);
                    Session["rgGrid1_original"] = persister.LoadForUser(m_User, (string)Session["AppID"], GridSettingsType.All.ToString());

                    if ((Session["objVersandStornoTable"] == null))
                    {
                        lblError.Text = "Keine Dokumente zur Anzeige gefunden.";  
                    }
                    else
                    {
                        m_Report = (VersandStorno)Session["objVersandStorno"];
                        tblSelection = (DataTable)Session["objVersandStornoTable"];
                        if (m_Report.Status == 103)
                        {
                            lblError.Text = m_Report.Message;
                        }
                        m_Report.GetMaterial((string)Session["AppID"], (string)Session.SessionID, this);
                        if (m_Report.Status != 0)
                        {
                            lblError.Text = "Fehler beim lesen der Versandoptionen!";
                        }
                        Session["objVersandStorno"] = m_Report;
                        FillRadioList();
                        Fillgrid();
                    }
                }
                else
                {
                    m_Report = (VersandStorno)Session["objVersandStorno"];
                    if (m_Report.Status == 103)
                    {
                        lblError.Text = m_Report.Message;
                    }
                    tblSelection = (DataTable)Session["objVersandStornoTable"];
                }
            }
            catch
            {
                lblError.Text = "Keine Dokumente zur Anzeige gefunden.";
            }
        }

        private void Page_PreRender(object sender, System.EventArgs e)
        {
            Common.SetEndASPXAccess(this);
        }

        private void Page_Unload(object sender, System.EventArgs e)
        {
            Common.SetEndASPXAccess(this);
        }

        private void FillRadioList() 
        {
            rbListMaterial.DataSource = m_Report.tblMaterial;
            rbListMaterial.DataTextField = "Materialtext";
            rbListMaterial.DataValueField = "Matnr";
            rbListMaterial.DataBind();
            rbListMaterial.SelectedValue = "5530";
        }

        private void Fillgrid()
        {

            if (tblSelection.Rows.Count == 0)
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

        protected void rgGrid1_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (tblSelection != null)
            {
                rgGrid1.DataSource = tblSelection.DefaultView;
            }
            else
            {
                rgGrid1.DataSource = null;
            }
        }

        private void SearchMode(bool search = true)
        {
            Result.Visible = !search;
            dataFooter.Visible = !search;
        }

        protected void lbBack_Click(object sender, EventArgs e)
        {
            Response.Redirect("Change05.aspx?AppID=" + (string)Session["AppID"]);
        }

        protected void cmdSpeichern_Click(object sender, EventArgs e)
        {
            if (CheckGrid() > 0)
            {
                m_Report.MaterialNr = rbListMaterial.SelectedValue;
                m_Report.ChangeMaterial((string)Session["AppID"], (string)Session.SessionID, this);
                if (m_Report.Status != 0 )
                {
                    lblError.Text = m_Report.Message;
                }
                else
                {
                    Session["objVersandStorno"] = m_Report;
                    Session["objVersandStornoTable"] = m_Report.tblAnforderungen;
                    for (Int32 i = m_Report.tblAnforderungen.Rows.Count - 1; i >= 0; i--)
                    {
                        if (m_Report.tblAnforderungen.Rows[i]["Select"].ToString() == "99")
                        {
                            m_Report.tblAnforderungen.Rows[i]["Select"]= "";
                            m_Report.tblAnforderungen.Rows[i]["Materialnr"] = m_Report.MaterialNr;

                        }
                    }
                    rgGrid1.Rebind();
                }
            }
            else
            {
                lblError.Text = "Kein Fahrzeug ausgewählt!";
            }
        }

        private int CheckGrid()
        {	
            int intReturn = 0;
            DataRow tmpRow = null;
            CheckBox chbox;

            foreach (GridDataItem gridRow in rgGrid1.Items)
            {
                chbox = gridRow.FindControl("chkAuswahl") as CheckBox;

                tmpRow = tblSelection.Select("Fahrgestellnummer = '" + gridRow["Fahrgestellnummer"].Text + "'")[0];
                tmpRow.BeginEdit();

                if (chbox.Checked)
                {
                    tmpRow["Select"] = "99";
                    intReturn++;
                }
                else
                {
                    tmpRow["Select"] = "";
                }

                tmpRow.EndEdit();
                tblSelection.AcceptChanges();
            }

            Session["objVersandStornoTable"] = tblSelection;

            return intReturn;
	    }

        protected void rgGrid1_ItemDataBound(object sender, GridItemEventArgs e)
        {
            if (e.Item is GridDataItem)
            {
                GridDataItem item = e.Item as GridDataItem;
                string strMatnr = item["Material"].Text;

                try
                {
                    item["Material"].Text = m_Report.tblMaterial.Select("Matnr = '" + strMatnr.TrimStart('0') + "'")[0]["Materialtext"].ToString();
                }
                catch { }
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

                var saveLayoutButton = new Button() { ToolTip = "Layout speichern", CommandName = "SaveGridLayout", CssClass = "rgSaveLayout" };
                rbutton_parent.Controls.AddAt(0, saveLayoutButton);

                var resetLayoutButton = new Button() { ToolTip = "Layout zurücksetzen", CommandName = "ResetGridLayout", CssClass = "rgResetLayout" };
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
                    eSettings.FileName = string.Format("AenderungVersandoptionen_{0:yyyyMMdd}", DateTime.Now);
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

