using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using CKG.Components.Controls;

namespace CKG.Components.Zulassung
{
    public partial class Dokumentencenter : Controls.ReportPage
    {
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            if (!this.IsPostBack)
            {
                var user = this.CKGUser;
                var dal = new DAL.ZulassungDal(ref user, this.App, String.Empty, this);
                this.ddlLand.DataTextField = "FullDesc";
                this.ddlLand.DataValueField = "Land1";
                this.ddlLand.DataSource = dal.Countries;
                this.ddlLand.DataBind();
            }
        }


        protected void OnSearch(object sender, EventArgs e)
        {
            Page.Validate();

            if (Page.IsValid)
            {
                var dm = new CKG.Components.Zulassung.DAL.DocumentManager(this.MapPath("~/Components/Zulassung/Dokumente/Download"));
                var ds = from d in dm.GetDocumentsForCountry(this.ddlLand.SelectedValue)
                         select new GroupingWrapper(d);

                this.dlResult.SelectCountry = this.ddlLand.SelectedItem.Text;
                this.dlResult.DataSource = ds;

                this.dlResult.DataBind();
                this.upMain.Update();
            }
            this.upValidation.Update();
        }

        protected void OnDocumentListCommand(object sender, CommandEventArgs e)
        {
            if (e.CommandName.Equals("SelectAll", StringComparison.OrdinalIgnoreCase))
            {
                var cbs = from item in this.dlResult.Items
                          let cb = (CheckBox)item.FindControl("cbTest")
                          select cb;

                foreach (var cb in cbs)
                {
                    cb.Checked = ((string)e.CommandArgument).Equals("Select", StringComparison.OrdinalIgnoreCase);
                }
            }
            else if (e.CommandName.Equals("Save", StringComparison.OrdinalIgnoreCase))
            {
                var files = from item in this.dlResult.Items
                            let cb = (CheckBox)item.FindControl("cbTest")
                            where cb.Checked
                            let hf = (HiddenField)item.FindControl("hfTest")
                            select hf.Value;

                var dm = new CKG.Components.Zulassung.DAL.DocumentManager(this.MapPath("~/Components/Zulassung/Dokumente/Download"));
                dm.ValidateAndDownload(files, "Zulassungsdokumente.zip", new HttpResponseWrapper(this.Response));
            }
        }

        protected void OnLayoutCreated(object sender, DocumentListLayoutEventArgs e)
        {
            var lbSave = e.Layout.FindControl("lbSave");
            ScriptManager.GetCurrent(this).RegisterPostBackControl(lbSave);
        }

    }

    internal class GroupingWrapper : CKG.Components.Controls.IDocumentGroup
    {
        private object[] documents;
        private readonly IGrouping<string, DAL.DocumentProperties> grouping;

        public GroupingWrapper(IGrouping<string, DAL.DocumentProperties> grouping)
        {
            this.grouping = grouping;
        }

        public string GroupName { get { return this.grouping.Key; } }

        public System.Collections.IList Documents
        {
            get
            {
                if (this.documents == null)
                {
                    this.documents = this.grouping.ToArray();
                }

                return this.documents;
            }
        }
    }
}