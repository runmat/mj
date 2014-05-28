using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CKG.Base.Kernel.Common;
using CKG.Base.Kernel.Security;
using Leasing.lib;
using Telerik.Web.UI;

namespace Leasing.forms
{
    public partial class Report12 : Page
    {
        private User user;
        private App app;
        private Versandstatuswechsel versandstatus;

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            user = Common.GetUser(this);
            Common.FormAuth(this, user);

            app = new App(user);
            Common.GetAppIDFromQueryString(this);
            var appID = (string)Session["AppID"];

            lblHead.Text = (string)user.Applications.Select("AppID = '" + appID + "'")[0]["AppFriendlyName"];
            Common.TranslateTelerikColumns(tempBriefGrid);

            versandstatus = Common.GetOrCreateObject("Versandstatuswechsel",
                                                     () => new Versandstatuswechsel(ref user, ref app, appID));
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            Common.SetEndASPXAccess(this);
        }

        protected override void OnUnload(EventArgs e)
        {
            base.OnUnload(e);

            Common.SetEndASPXAccess(this);
        }

        protected void NewSearchOpen(object sender, ImageClickEventArgs e)
        {
            ShowSearch(true);
        }

        protected void NewSearchClose(object sender, ImageClickEventArgs e)
        {
            ShowSearch(false);
        }

        protected void lbBack_Click(object sender, EventArgs e)
        {
            Response.Redirect("/Services/(S(" + Session.SessionID + "))/Start/Selection.aspx");
        }

        private void ShowSearch(bool open)
        {
            cmdNewSearchClose.Visible = open;
            cmdNewSearchOpen.Visible = !open;
            SearchParams.Visible = open;
        }

        protected void SearchClick(object sender, EventArgs e)
        {
            DateTime tmp;

            var von = DateTime.TryParse(txtDatumVon.Text, out tmp) ? tmp : (DateTime?)null;
            var bis = DateTime.TryParse(txtDatumBis.Text, out tmp) ? tmp : (DateTime?)null;

            versandstatus.GetData(this, txtFahrgestellnummer.Text, txtKennzeichen.Text, txtVertragsnummer.Text, txtObjektnummer.Text, von, bis);

            if (versandstatus.Status != 0)
            {
                tempBriefGrid.Visible = false;
                lblError.Visible = true;
                lblError.Text = versandstatus.Message;
                return;
            }

            LoadData();

            ShowSearch(versandstatus.Result.Rows.Count == 0);
        }

        private void LoadData(bool rebind = true)
        {
            tempBriefGrid.Visible = true;
            tempBriefGrid.DataSource = versandstatus.Result;
            if (rebind)
                tempBriefGrid.DataBind();
        }

        protected void TempBriefItemCommand(object sender, GridCommandEventArgs e)
        {
            //throw new NotImplementedException();
        }

        protected void TempBriefNeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            LoadData(false);
        }

        protected void ToggleBrief(object sender, EventArgs e)
        {
            var box = (CheckBox)sender;
            var item = (GridDataItem)box.NamingContainer;

            var equnr = item["EQUNR"].Text;
            var chass = item["CHASSIS_NUM"].Text;

            var row =
                versandstatus.Result.Select(String.Format("EQUNR='{0}' and CHASSIS_NUM='{1}'", equnr, chass)).
                    FirstOrDefault();
            if (row != null)
                row["Status"] = box.Checked ? "X" : "-";

            cmdEnd.Enabled = versandstatus.Result.Select("Status='X'").Length > 0;

            LoadData();
        }

        protected void EndVersendet(object sender, EventArgs e)
        {
            versandstatus.EndVersand(this);
            if (versandstatus.Status != 0)
            {
                tempBriefGrid.Visible = false;
                lblError.Visible = true;
                lblError.Text = versandstatus.Message;
            }

            cmdEnd.Enabled = versandstatus.Result.Select("Status='X'").Length > 0;

            LoadData();
        }
    }
}