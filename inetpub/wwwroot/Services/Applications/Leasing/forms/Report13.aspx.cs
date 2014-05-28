using System;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using CKG.Base.Kernel.Common;
using CKG.Base.Kernel.Security;
using Leasing.lib;
using Telerik.Web.UI;

namespace Leasing.forms
{
    public partial class Report13 : Page
    {
        private User user;
        private App app;
        private Versandsperre versandsperre;

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            user = Common.GetUser(this);
            Common.FormAuth(this, user);

            app = new App(user);
            Common.GetAppIDFromQueryString(this);
            var appId = (string)Session["AppID"];

            lblHead.Text = (string)user.Applications.Select("AppID = '" + appId + "'")[0]["AppFriendlyName"];
            Common.TranslateTelerikColumns(tempBriefGrid);

            versandsperre = Common.GetOrCreateObject("Versandsperre",
                                                     () => new Versandsperre(ref user, ref app, appId));
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

        protected void lbBack_Click(object sender, EventArgs e)
        {
            Response.Redirect("/Services/(S(" + Session.SessionID + "))/Start/Selection.aspx");
        }

        protected void NewSearchOpen(object sender, ImageClickEventArgs e)
        {
            ShowSearch(true);
        }

        protected void NewSearchClose(object sender, ImageClickEventArgs e)
        {
            ShowSearch(false);
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

            versandsperre.GetData(this, txtFahrgestellnummer.Text, txtKennzeichen.Text, txtVertragsnummer.Text, txtObjektnummer.Text, von, bis);

            if (versandsperre.Status != 0)
            {
                tempBriefGrid.Visible = false;
                lblError.Visible = true;
                lblError.Text = versandsperre.Message;
                return;
            }

            LoadData();

            ShowSearch(versandsperre.Result.Rows.Count == 0);
        }

        private void LoadData(bool rebind = true)
        {
            tempBriefGrid.Visible = true;
            tempBriefGrid.DataSource = versandsperre.Result;
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

        protected void ToggleSelection(object sender, EventArgs e)
        {
            var box = (CheckBox)sender;
            var item = (GridDataItem)box.NamingContainer;

            var equnr = item["EQUNR"].Text;
            var chass = item["CHASSIS_NUM"].Text;

            var row =
                versandsperre.Result.Select(String.Format("EQUNR='{0}' and CHASSIS_NUM='{1}'", equnr, chass)).
                    FirstOrDefault();
            if (row != null)
                row["Status"] = box.Checked ? "X" : "-";

            cmdEnd.Enabled = versandsperre.Result.Select("Status='X'").Length > 0;

            LoadData();
        }

        protected void Entsperren(object sender, EventArgs e)
        {
            versandsperre.Entsperren(this);
            if (versandsperre.Status != 0)
            {
                tempBriefGrid.Visible = false;
                lblError.Visible = true;
                lblError.Text = versandsperre.Message;
            }

            cmdEnd.Enabled = versandsperre.Result.Select("Status='X'").Length > 0;

            LoadData();
        }
    }
}