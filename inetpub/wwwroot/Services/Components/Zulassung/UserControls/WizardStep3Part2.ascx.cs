using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.IO;

namespace CKG.Components.Zulassung.UserControls
{
    public partial class WizardStep3Part2 : System.Web.UI.UserControl, IWizardStepPart
    {
        private IWizardPage page;

        private const string validationScript = @"function showUploadStartMessage(sender, args) {
                                                    var filename = args.get_fileName();
                                                    var filext = filename.substring(filename.lastIndexOf(""."") + 1);
                                                    if (filext == ""pdf"") {
                                                        return true;
                                                    } else {
                                                        alert('Fehler beim Hochladen der Datei(nur PDF-Format erlaubt!)');
                                                        return false;
                                                    }
                                                }";
        protected const string ValidationGroup = "ZulassungStep3Part2";

        private const string SortDirectionViewStateKey = "SortDirection";
        private const string SortKeyViewStateKey = "SortKey";
        private const string AscendingSort = "asc";
        private const string DescendingSort = "desc";
        private bool dataLoaded = false;

        private bool SortAscending
        {
            get
            {
                var isAscending = ViewState[SortDirectionViewStateKey];
                if (isAscending is bool)
                {
                    return (bool)isAscending;
                }
                return true;
            }
            set
            {
                ViewState[SortDirectionViewStateKey] = value;
            }
        }

        private string SortField
        {
            get
            {
                var field = ViewState[SortKeyViewStateKey] as string;
                return field ?? string.Empty;
            }
            set
            {
                ViewState[SortKeyViewStateKey] = value;
            }
        }

        public void Validate()
        {
            Page.Validate(ValidationGroup);
        }

        public void Save()
        {

        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            page = (IWizardPage)Page;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            Page.ClientScript.RegisterClientScriptBlock(GetType(), "validation", validationScript, true);
            GridNavigation1.setGridElment(ref GridView1);
            GridNavigation1.PagerChanged += PageIndexChanged;
            GridNavigation1.PageSizeChanged += PageSizeChanged;
            GridView1.Sorting += SortingChanged;
        }

        protected override void OnPreRender(EventArgs e)
        {
            if (!dataLoaded)
            {
                FillGrid(GridView1.PageIndex);
            }
            base.OnPreRender(e);
        }

        protected void GridView1_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            GridViewRow row = GridView1.Rows[e.RowIndex];

            var fileUpload = row.Cells[3].FindControl("AsyncFileUpload1") as Label;
            if (fileUpload != null)
            {
                string filepath = Page.MapPath("~\\Components\\Zulassung\\Dokumente\\TempDoc\\" + page.User.KUNNR);
                filepath = Path.Combine(filepath, fileUpload.Text);
                if (File.Exists(filepath))
                {
                    try
                    {
                        File.Delete(filepath);
                    }
                    catch
                    {
                        // do nothing.
                    }
                    finally
                    {
                        string lfdNr = row.Cells[0].Text;
                        page.DAL.Protokollarten.Select("ID=" + lfdNr)[0]["Filename"] = string.Empty;
                        page.DAL.Protokollarten.AcceptChanges();
                    }
                }
            }
        }

        protected void GridView1_RowEditing(object sender, GridViewEditEventArgs e)
        {
            GridView1.EditIndex = e.NewEditIndex;
            FillGrid(GridView1.PageIndex);
        }

        protected void GridView1_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            GridView1.EditIndex = -1;
            FillGrid(GridView1.PageIndex);
        }

        protected void GridView1_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            GridViewRow row = GridView1.Rows[e.RowIndex];

            var fileUpload = row.Cells[3].FindControl("AsyncFileUpload1") as AjaxControlToolkit.AsyncFileUpload;
            if (fileUpload != null)
            {
                if (fileUpload.HasFile == true)
                {
                    if (fileUpload.ContentType == "application/pdf")
                    {
                        Label lblFahrt = (Label)row.Cells[1].FindControl("lblFahrt");
                        string fahrt = lblFahrt.Text;
                        string lfdNr = row.Cells[0].Text;
                        string dokArt = row.Cells[2].Text;

                        string filepath = Page.MapPath("~\\Components\\Zulassung\\Dokumente\\TempDoc\\" + page.User.KUNNR);
                        if (!System.IO.Directory.Exists(filepath))
                        {
                            System.IO.Directory.CreateDirectory(filepath);
                        }
                        string filename = string.Format("{0}_{1}_{2:yyyyMMddhhmmss}.pdf", fahrt, dokArt.Replace(".", ""), DateTime.Now);
                        fileUpload.SaveAs(filepath + "\\" + filename);
                        var fileRow = page.DAL.Protokollarten.Select("ID=" + lfdNr)[0];
                        var oldFile = fileRow["Filename"] as string;
                        if (!string.IsNullOrEmpty(oldFile))
                        {
                            var oldPath = Path.Combine(filepath, oldFile);
                            if (File.Exists(oldPath))
                            {
                                try
                                {
                                    File.Delete(oldPath);
                                }
                                catch
                                {
                                    // do nothing.
                                }
                            }
                        }
                        fileRow["Filename"] = filename;
                        page.DAL.Protokollarten.AcceptChanges();
                    }
                }

            }

            GridView1.EditIndex = -1;

            FillGrid(GridView1.PageIndex);
        }

        private void PageIndexChanged(int pageIndex)
        {
            FillGrid(pageIndex);
        }

        private void PageSizeChanged()
        {
            FillGrid(GridView1.PageIndex);
        }

        private void SortingChanged(object sender, System.Web.UI.WebControls.GridViewSortEventArgs e)
        {
            FillGrid(GridView1.PageIndex, e.SortExpression);
        }

        private void FillGrid(int pageIndex)
        {
            FillGrid(pageIndex, string.Empty);
        }

        private void FillGrid(int pageIndex, string sort)
        {
            var data = page.DAL.Protokollarten;
            if (data != null)
            {
                var dataView = data.DefaultView;
                if (dataView != null)
                {
                    if (dataView.Count == 0)
                    {
                        Result.Visible = false;
                    }
                    else
                    {
                        Result.Visible = true;
                        if (string.IsNullOrEmpty(sort))
                        {
                            sort = SortField;
                        }
                        else
                        {
                            if (sort.Equals(SortField, StringComparison.OrdinalIgnoreCase))
                            {
                                SortAscending = !SortAscending;
                            }
                            else
                            {
                                SortAscending = !string.IsNullOrEmpty(SortField);
                            }
                        }

                        if (!string.IsNullOrEmpty(sort) && dataView.Table.Columns.Contains(sort))
                        {
                            SortField = sort;
                        }
                        else
                        {
                            SortField = "ID";
                        }

                        dataView.Sort = string.Format("{0} {1}", SortField, SortAscending ? AscendingSort : DescendingSort);

                        GridView1.PageIndex = pageIndex;
                        GridView1.DataSource = dataView;
                        GridView1.Visible = true;
                        GridNavigation1.Visible = (dataView.Count > GridView1.PageSize);
                        GridView1.DataBind();
                        dataLoaded = true;
                        return;
                    }
                }
            }
        }
    }
}