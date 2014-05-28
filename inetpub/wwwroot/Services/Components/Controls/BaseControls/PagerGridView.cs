using System;
using System.Data;
using System.Web.UI.WebControls;

using CKG.Base.Business;
using CKG.Services;

namespace CKG.Components.Controls
{
    public class PagerGridView : GridView
    {
        public string GridViewNavigationId { get; set; }
        public bool FixGridViewCols { get; set; }

        private string SortField
        {
            get { return (string)this.ViewState["Sort"]; }
            set { this.ViewState["Sort"] = value; }
        }

        private string Direction
        {
            get { return (string)this.ViewState["Direction"]; }
            set { this.ViewState["Direction"] = value; }
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.PagerSettings.Visible = false;
            this.AutoGenerateColumns = false;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            var gvn = this.NamingContainer.FindControl(this.GridViewNavigationId) as GridNavigation;
            GridView temp = this;
            gvn.setGridElment(ref temp);

            if (temp != this)
            {
                throw new InvalidOperationException("GridViewNavigation hat GridElement verändert!");
            }
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            if (this.FixGridViewCols)
            {
                HelpProcedures.FixedGridViewCols(this);
            }
        }

        public void Fill(DataView data, string sortKey)
        {
            sortKey = sortKey ?? String.Empty;
            sortKey = sortKey.Trim();
            string strDirection = "";

            if (!String.IsNullOrEmpty(sortKey))
            {
                if (this.SortField == null || this.SortField == sortKey)
                {
                    if (this.Direction == null)
                    {
                        strDirection = "desc";
                    }
                    else
                    {
                        strDirection = this.Direction;
                    }
                }
                else
                {
                    strDirection = "desc";
                }

                if (strDirection == "asc")
                {
                    strDirection = "desc";
                }
                else
                {
                    strDirection = "asc";
                }

                this.SortField = sortKey;
                this.Direction = strDirection;
            }
            else
            {
                if (this.SortField != null)
                {
                    sortKey = this.SortField;
                    if (this.Direction == null)
                    {
                        strDirection = "asc";
                        this.Direction = strDirection;
                    }
                    else
                    {
                        strDirection = this.Direction;
                    }
                }
            }

            if (!String.IsNullOrEmpty(sortKey))
            {
                data.Sort = sortKey + " " + strDirection;
            }

            this.DataSource = data;
            this.DataBind();
        }
    }
}