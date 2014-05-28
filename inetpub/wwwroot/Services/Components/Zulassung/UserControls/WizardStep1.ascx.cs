using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CKG.Base.Kernel.Common;
using CKG.Components.Zulassung.DAL;

namespace CKG.Components.Zulassung.UserControls
{
    public partial class WizardStep1 : System.Web.UI.UserControl, IWizardStep
    {
        private IWizardPage page;
        protected const string ValidationGroup = "ZulassungStep1";

        public void Validate()
        {
            Page.Validate(ValidationGroup);
        }

        public void ResetNavigation()
        {
            Wizard.SelectedIndex = 0;
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            page = (IWizardPage)Page;
            page.DAL.PropertyChanged += new System.ComponentModel.PropertyChangedEventHandler(DAL_PropertyChanged);
        }

        protected override void OnLoad(EventArgs e)
        {
            lblErrorStamm.Text = string.Empty;
            lblErrorStamm.Visible = false;
            base.OnLoad(e);
            if (!Page.IsPostBack && page.DAL.Vehicles != null)
            {
                SearchResult1.FillGrid();
                SearchResult1.Visible = true;
            }
        }


        void DAL_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if(e.PropertyName.Equals("vehicles", StringComparison.OrdinalIgnoreCase)) 
            {
                if (page.DAL.Vehicles != null)
                {
                    SearchResult1.FillGrid();
                    SearchResult1.Visible = true;
                }
                else
                {
                    SearchResult1.Visible = false;
                }
            }
        }

        protected override void OnPreRender(EventArgs e)
        {
            if (page.DAL.SelectedVehicles != null)
            {
                var count = page.DAL.SelectedVehicles.Count;
                if (count > 0)
                {
                    SelectionCount.Text = string.Format("Sie haben {0} Fahrzeuge für die Zulassung ausgewählt.", count);
                    SelectionCount.Visible = true;
                }
                else
                {
                    SelectionCount.Visible = false;
                }
            }
            UpdatePanel1.Update();
            UpdatePanel2.Update();
            base.OnPreRender(e);
        }

        public event EventHandler<EventArgs> NavigateBack;

        void OnNavigateBack(object sender, EventArgs e)
        {
            if (NavigateBack != null)
            {
                NavigateBack(this, EventArgs.Empty);
            }
        }

        public event EventHandler<EventArgs> Completed;

        private void RaiseCompleted()
        {
            var eh = this.Completed;

            if (eh != null)
            {
                eh(this, EventArgs.Empty);
            }
        }

        protected void buttonNext_Click(object sender, EventArgs e)
        {
            if (page.DAL.SelectedVehicles != null && page.DAL.SelectedVehicles.Count > 0)
            {
                lblErrorStamm.Visible = false;
                page.DAL.ExtractAddressAndInsuranceData();

                if (String.Equals(page.DAL.SelectedCountry, "DE", StringComparison.OrdinalIgnoreCase))
                {
                    this.RaiseCompleted();
                }
                else
                {
                    var dm = new DocumentManager(this.Page.MapPath("~/Components/Zulassung/Dokumente/Download"));
                    var ds = dm.GetDocumentsForCountry(this.page.DAL.SelectedCountry).ToArray();

                    if (ds.Length > 0)
                    {
                        this.dlZulassung.DataSource = from d in ds
                                        select new GroupingWrapper(d);
                        this.dlZulassung.DataBind();
                        ScriptManager.RegisterStartupScript(this.moDocuments, typeof(CKG.Components.Controls.ModalOverlay), this.moDocuments.ID, this.moDocuments.ShowOverlayClientScript, true);
                    }
                    else
                    {
                        this.RaiseCompleted();
                    }
                }
            }
            else
            {
                lblErrorStamm.Text = "Bitte wählen Sie mindestens ein Fahrzeug für die Zulassung aus.";
                lblErrorStamm.Visible = true;
            }
        }

        protected void OnDocumentListCommand(object sender, CommandEventArgs e)
        {
            var dl = (CKG.Components.Controls.DocumentList)sender;

            if (e.CommandName.Equals("SelectAll", StringComparison.OrdinalIgnoreCase))
            {
                var cbs = from item in dl.Items
                          let cb = (CheckBox)item.FindControl("cbTest")
                          select cb;

                foreach (var cb in cbs)
                {
                    cb.Checked = ((string)e.CommandArgument).Equals("Select", StringComparison.OrdinalIgnoreCase);
                }
            }
            else if (e.CommandName.Equals("Save", StringComparison.OrdinalIgnoreCase))
            {
                var files = from item in dl.Items
                            let cb = (CheckBox)item.FindControl("cbTest")
                            where cb.Checked
                            let hf = (HiddenField)item.FindControl("hfTest")
                            select hf.Value;

                var dm = new CKG.Components.Zulassung.DAL.DocumentManager(this.MapPath("~/Components/Zulassung/Dokumente/Download"));
                dm.ValidateAndDownload(files, "Zulassungsdokumente.zip", new HttpResponseWrapper(this.Response));
            }

            ScriptManager.RegisterStartupScript(this.moDocuments, typeof(CKG.Components.Controls.ModalOverlay), this.moDocuments.ID, this.moDocuments.ShowOverlayInstantClientScript, true);
        }

        protected void OnLayoutCreated(object sender, CKG.Components.Controls.DocumentListLayoutEventArgs e)
        {
            var lbSave = e.Layout.FindControl("lbSave");
            ScriptManager.GetCurrent(this.Page).RegisterPostBackControl(lbSave);

            var lbClose = this.moDocuments.ContentTemplateContainer.FindControl("lbClose");
            ScriptManager.GetCurrent(this.Page).RegisterPostBackControl(lbClose);
        }

        protected void OnClose(object sender, EventArgs e)
        {
            this.RaiseCompleted();
        }
    }
}