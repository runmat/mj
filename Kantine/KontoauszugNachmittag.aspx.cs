using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Configuration;
using Telerik.Web.UI;

namespace Kantine
{
	public partial class KontoauszugNachmittag : KantinePage
	{
       	protected void Page_Load(object sender, EventArgs e)
		{
            if (KB != null)
            {
                if (!(KB.IsAdmin || KB.IsSeller))
                {
                    Response.Redirect("Default.aspx");
                }
                else
                {
                    if (!IsPostBack)
                    {
                        FillGrid(true);
                    }
                }
            }
		}

        private void FillGrid(bool Rebind = true)
        {
            try
            {
                if (Session["KontoauszugNachmittag"] == null)
                {
                    DataTable dtAuszug = DB.GetKontoauszug("Nachmittagsmodus", Zeitraum.letzerMonat, LogFilter.Kassierer);
                    dtAuszug.DefaultView.Sort = "Datum DESC";
                    Session["KontoauszugNachmittag"] = dtAuszug.DefaultView;
                }
               
                DataView dv = (DataView)Session["KontoauszugNachmittag"];
                               
                // RadGrid
                rgAuszug.DataSource = dv;
                if (Rebind)
                {
                    rgAuszug.Rebind();
                }               
            }
            catch (Exception ex)
            {
                lblError.Text = ex.Message;
            }
        }

        protected void rgAuszugNeedDataSource(object sender , GridNeedDataSourceEventArgs e)
        {
            FillGrid(false);           
        }

        protected void rgAuszugPageIndexChanged(object sender, GridPageChangedEventArgs e)
        { 
            
        }

        protected void rgAuszugPageSizeChanged(object sender, GridPageSizeChangedEventArgs e)
        { 
        
        }

        protected void rgAuszug_ItemDataBound(object sender, GridItemEventArgs e)
        {
            if (e.Item is GridPagerItem)
            {
                RadComboBox PageSizeCombo = (RadComboBox)e.Item.FindControl("PageSizeComboBox");

                PageSizeCombo.Items.Clear();
                PageSizeCombo.Items.Add(new RadComboBoxItem("20"));
                PageSizeCombo.FindItemByText("20").Attributes.Add("ownerTableViewId", rgAuszug.MasterTableView.ClientID);
                PageSizeCombo.Items.Add(new RadComboBoxItem("50"));
                PageSizeCombo.FindItemByText("50").Attributes.Add("ownerTableViewId", rgAuszug.MasterTableView.ClientID);
                PageSizeCombo.Items.Add(new RadComboBoxItem("100"));
                PageSizeCombo.FindItemByText("100").Attributes.Add("ownerTableViewId", rgAuszug.MasterTableView.ClientID);
                PageSizeCombo.Items.Add(new RadComboBoxItem("200"));
                PageSizeCombo.FindItemByText("200").Attributes.Add("ownerTableViewId", rgAuszug.MasterTableView.ClientID);
                PageSizeCombo.Items.Add(new RadComboBoxItem("300"));
                PageSizeCombo.FindItemByText("300").Attributes.Add("ownerTableViewId", rgAuszug.MasterTableView.ClientID);
                if (PageSizeCombo.FindItemByText(e.Item.OwnerTableView.PageSize.ToString()) != null)
                {
                    PageSizeCombo.FindItemByText(e.Item.OwnerTableView.PageSize.ToString()).Selected = true;
                }
            }
        } 
	}
}
