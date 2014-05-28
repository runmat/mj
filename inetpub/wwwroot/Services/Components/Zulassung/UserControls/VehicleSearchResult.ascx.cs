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
    public partial class VehicleSearchResult : System.Web.UI.UserControl
    {
        private const string SortDirectionViewStateKey = "SortDirection";
        private const string SortKeyViewStateKey = "SortKey";
        private const string AscendingSort = "asc";
        private const string DescendingSort = "desc";
        IWizardPage page;

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
            var dal = page.DAL;
            if (dal.Status == 0 || dal.Status == 102 || dal.Status == 101)
            {
                var vehicles = dal.Vehicles;
                if (vehicles != null)
                {
                    var dataView = vehicles.DefaultView;

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

                    if (!string.IsNullOrEmpty(sort) && vehicles.Columns.Contains(sort))
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
                    GridNavigation1.Visible = (dataView.Count > 3);
                    GridView1.DataBind();
                    return;
                }
            }
            GridNavigation1.Visible = false;
            GridView1.Visible = false;
        }

        protected void CheckBox_Check(object sender, EventArgs e)
        {
            var checkBox = sender as CheckBox;

            if (checkBox != null)
            {
                var gridRow = checkBox.BindingContainer as GridViewRow;
                if (gridRow != null)
                {
                    var dataIndex = gridRow.DataItemIndex;
                    if (dataIndex < page.DAL.Vehicles.DefaultView.Count)
                    {
                        page.DAL.Vehicles.DefaultView[dataIndex]["AUSWAHL"] = (checkBox.Checked) ? "99" : string.Empty;
                        string t = page.DAL.Vehicles.DefaultView[dataIndex]["AUSWAHL"].ToString();
                    }
                }
            }
        }

        protected void ibtAuswahl_Click(object sender, System.Web.UI.ImageClickEventArgs e) 
        {
            foreach (GridViewRow gridRow in GridView1.Rows)
            {
                var chk = gridRow.FindControl("chk0000") as CheckBox;
                if (chk != null)
                {
                    chk.Checked = true;
                }
            }

            var vehicles = page.DAL.Vehicles;
            if (vehicles != null)
            {
                foreach (DataRow row in page.DAL.Vehicles.Rows)
                {
                    row["AUSWAHL"] = "99";
                }
                vehicles.AcceptChanges();
            }
        }

        protected void ibtnAbwahl_Click(object sender, System.Web.UI.ImageClickEventArgs e) 
        {
            foreach (GridViewRow gridRow in GridView1.Rows)
            {
                var chk = gridRow.FindControl("chk0000") as CheckBox;
                if (chk != null)
                {
                    chk.Checked = false;
                }
            }

            var vehicles = page.DAL.Vehicles;
            if (vehicles != null)
            {
                foreach (DataRow row in vehicles.Rows)
                {
                    row["AUSWAHL"] = string.Empty;
                }
                vehicles.AcceptChanges();
            }
        }
    }
}