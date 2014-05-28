using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using Telerik.Web.UI;

namespace Kantine.Warengruppe
{
	public partial class Warengruppe : KantinePage
	{
        //DB_Kantine DB; // = new DB_Kantine(ConfigurationManager.ConnectionStrings["CurrentCon"].ConnectionString);
        //KantinenBenutzer KB;
        //Kantine Kan;

        //protected void Page_Init(object sender, EventArgs e)
        //{
        //    Kan = (Kantine)this.Master;
        //    KB = Kan.User;
        //    DB = Kan.Database;
        //}

		protected void Page_Load(object sender, EventArgs e)
		{
            //if (Session["Benutzer"] != null)
            //{
            //    KantinenBenutzer KB = (KantinenBenutzer)Session["Benutzer"];
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
            //}
		}

        protected void rgWarengruppenNeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            FillGrid(false);
        }

        protected void rgWarengruppenPageIndexChanged(object sender, GridPageChangedEventArgs e)
        {

        }

        protected void rgWarengruppenPageSizeChanged(object sender, GridPageSizeChangedEventArgs e)
        {

        }

        private void FillGrid(bool Rebind = true)
		{         
            try
            {
                DataTable dtWarengruppe = DB.GetAllWarengruppen();
                dtWarengruppe.DefaultView.Sort = "BezeichnungWarengruppe ASC";
                Session["WarengruppenListe"] = dtWarengruppe.DefaultView;

                DataView dv = (DataView)Session["WarengruppenListe"];

                // RadGrid
                rgWarengruppen.DataSource = dv;
                if (Rebind)
                {
                    rgWarengruppen.Rebind();
                }
            }
            catch (Exception ex)
            {
                lblError.Text = ex.Message;
            }		

		}

		protected void btnNewMaterialGroup_Click(object sender, EventArgs e)
		{
			Response.Redirect("./WarengruppeAnlegen.aspx");
		}

        public void rgWarengruppen_ItemCommand(object sender, GridCommandEventArgs e)
		{
			string Command = e.CommandName.ToString();
			string Argument = e.CommandArgument.ToString();

            if (Command == "Bearbeiten")
			{
				int WarengruppenID = Convert.ToInt32(e.Item.Cells[2].Text);
				Session["MaterialgroupToEdit"] = WarengruppenID;
				Response.Redirect("./WarengruppeAnlegen.aspx");
			}
			else if (Command == "Löschen")
			{
				int Artikelnummer = Convert.ToInt32(e.Item.Cells[2].Text);
				DB.DeleteArtikel(Artikelnummer, true);
				lblAusgabe.Text = "Warengruppe gelöscht!";
				lblAusgabe.Visible = true;
				Response.Redirect("./Warengruppe.aspx");
				UP1.Update();
			}
        }
          
        protected void rgWarengruppen_ItemDataBound(object sender, GridItemEventArgs e)
        {
            if (e.Item is GridPagerItem)
            {
                RadComboBox PageSizeCombo = (RadComboBox)e.Item.FindControl("PageSizeComboBox");

                PageSizeCombo.Items.Clear();
                PageSizeCombo.Items.Add(new RadComboBoxItem("20"));
                PageSizeCombo.FindItemByText("20").Attributes.Add("ownerTableViewId", rgWarengruppen.MasterTableView.ClientID);
                PageSizeCombo.Items.Add(new RadComboBoxItem("50"));
                PageSizeCombo.FindItemByText("50").Attributes.Add("ownerTableViewId", rgWarengruppen.MasterTableView.ClientID);
                PageSizeCombo.Items.Add(new RadComboBoxItem("100"));
                PageSizeCombo.FindItemByText("100").Attributes.Add("ownerTableViewId", rgWarengruppen.MasterTableView.ClientID);
                PageSizeCombo.Items.Add(new RadComboBoxItem("200"));
                PageSizeCombo.FindItemByText("200").Attributes.Add("ownerTableViewId", rgWarengruppen.MasterTableView.ClientID);
                PageSizeCombo.Items.Add(new RadComboBoxItem("300"));
                PageSizeCombo.FindItemByText("300").Attributes.Add("ownerTableViewId", rgWarengruppen.MasterTableView.ClientID);
                if (PageSizeCombo.FindItemByText(e.Item.OwnerTableView.PageSize.ToString()) != null)
                {
                    PageSizeCombo.FindItemByText(e.Item.OwnerTableView.PageSize.ToString()).Selected = true;
                }
            }
        }
	 }
}
