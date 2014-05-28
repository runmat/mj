using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CKG.Components.Zulassung.DAL;
using System.Data;

namespace CKG.Components.Zulassung.UserControls
{
    public partial class WizardStep2Part3 : System.Web.UI.UserControl, IWizardStepPart
    {
        private const string SortDirectionViewStateKey = "SortDirection";
        private const string SortKeyViewStateKey = "SortKey";
        private const string AscendingSort = "asc";
        private const string DescendingSort = "desc";
        IWizardPage page;
        protected const string ValidationGroup = "ZulassungStep2Part3";
        private bool dataLoaded = false;

        public void Validate()
        {
            Page.Validate(ValidationGroup);
        }

        public void Save()
        {
        }

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

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            page = (IWizardPage)Page;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
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

        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            var row = e.Row;

            if (row.RowType == DataControlRowType.DataRow && row.RowIndex == GridView1.EditIndex)
            {
                var item = (DataRowView)row.DataItem;
                var prefix = this.page.DAL.Stva + "-";

                if (prefix.Length > 1)
                {
                    if (String.IsNullOrEmpty(Convert.ToString(item["Wunschkennz1"])))
                    {
                        ((TextBox)(row.Cells[1].Controls[0])).Text = prefix;
                    }
                    if (String.IsNullOrEmpty(Convert.ToString(item["Wunschkennz2"])))
                    {
                        ((TextBox)(row.Cells[2].Controls[0])).Text = prefix;
                    }
                    if (String.IsNullOrEmpty(Convert.ToString(item["Wunschkennz3"])))
                    {
                        ((TextBox)(row.Cells[3].Controls[0])).Text = prefix;
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
            var dt = page.DAL.SelectedVehicles;
            var prefix = this.page.DAL.Stva + "-";

            GridViewRow row = GridView1.Rows[e.RowIndex];
            var kennz = ((TextBox)(row.Cells[1].Controls[0])).Text;
            kennz = kennz.Equals(prefix, StringComparison.OrdinalIgnoreCase) ? String.Empty : kennz;
            dt[row.DataItemIndex]["Wunschkennz1"] = kennz;

            kennz = ((TextBox)(row.Cells[2].Controls[0])).Text;
            kennz = kennz.Equals(prefix, StringComparison.OrdinalIgnoreCase) ? String.Empty : kennz;
            dt[row.DataItemIndex]["Wunschkennz2"] = kennz;

            kennz = ((TextBox)(row.Cells[3].Controls[0])).Text;
            kennz = kennz.Equals(prefix, StringComparison.OrdinalIgnoreCase) ? String.Empty : kennz;
            dt[row.DataItemIndex]["Wunschkennz3"] = kennz;

            dt[row.DataItemIndex]["ResNr"] = ((TextBox)(row.Cells[4].Controls[0])).Text;
            dt[row.DataItemIndex]["ResName"] = ((TextBox)(row.Cells[5].Controls[0])).Text;

            GridView1.EditIndex = -1;

            FillGrid(GridView1.PageIndex);
        }

        public void FillGrid()
        {
            FillGrid(0);
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
            var dataView = page.DAL.SelectedVehicles;
            if (dataView != null)
            {
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
                    SortField = "CHASSIS_NUM";
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