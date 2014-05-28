using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using CKG.Base.Business;
using CKG.Services;

namespace AppMBB.elements
{
    public class PagerGridView : GridView
    {
        public string GridViewNavigationId { get; set; }
        public bool FixGridViewCols { get; set; }

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
                if ((this.ViewState["Sort"] == null) || ((string)this.ViewState["Sort"] == sortKey))
                {
                    if (this.ViewState["Direction"] == null)
                    {
                        strDirection = "desc";
                    }
                    else
                    {
                        strDirection = this.ViewState["Direction"].ToString();
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

                this.ViewState["Sort"] = sortKey;
                this.ViewState["Direction"] = strDirection;
            }
            else
            {
                if ((this.ViewState["Sort"] != null))
                {
                    sortKey = this.ViewState["Sort"].ToString();
                    if (this.ViewState["Direction"] == null)
                    {
                        strDirection = "asc";
                        this.ViewState["Direction"] = strDirection;
                    }
                    else
                    {
                        strDirection = this.ViewState["Direction"].ToString();
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