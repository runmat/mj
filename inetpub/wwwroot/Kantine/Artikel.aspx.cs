using System;
using System.Data;
using Telerik.Web.UI;

namespace Kantine
{
	public partial class Artikel : KantinePage
	{
        protected void Page_Load(object sender, EventArgs e)
		{
            if (KB != null)
            {
                if (!(KB.IsAdmin || KB.IsSeller ))
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

        protected void rgArtikelNeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            FillGrid(false);
        }

        protected void rgArtikelPageIndexChanged(object sender, GridPageChangedEventArgs e)
        {

        }

        protected void rgArtikelPageSizeChanged(object sender, GridPageSizeChangedEventArgs e)
        {

        }

        private void FillGrid(bool Rebind = true)
		{
            try
            {
                DataTable dtArtikel = DB.GetAllArtikel();
                dtArtikel.DefaultView.Sort = "Artikelbezeichnung ASC";
                Session["Artikel"] = dtArtikel.DefaultView;

                DataView dv = (DataView)Session["Artikel"];

                // RadGrid
                rgArtikel.DataSource = dv;
                if (Rebind)
                {
                    rgArtikel.Rebind();
                }
            }
            catch (Exception ex)
            {
                lblError.Text = ex.Message;
            }
		}

		protected void btnNewArticle_Click(object sender, EventArgs e)
		{
			Response.Redirect("./ArtikelAnlegen.aspx");
		}

        public void rgArtikel_ItemCommand(object sender, GridCommandEventArgs e)
		{
			string Command = e.CommandName.ToString();
			//string Argument = e.CommandArgument.ToString();

            if (Command == "Bearbeiten")
			{
				int Artikelnummer = Convert.ToInt32(e.Item.Cells[2].Text);
				Session["ArticleToEdit"] = Artikelnummer;
				Response.Redirect("./ArtikelAnlegen.aspx");
			}
			else if (Command == "Löschen")
			{
				int Artikelnummer = Convert.ToInt32(e.Item.Cells[2].Text);
				DB.DeleteArtikel(Artikelnummer, true);
				lblAusgabe.Text = "Artikel gelöscht!";
				lblAusgabe.Visible = true;
				Response.Redirect("./Artikel.aspx");
				UP1.Update();
			}		
			
		}

        protected void rgArtikel_ItemDataBound(object sender, GridItemEventArgs e)
        {
            if (e.Item is GridPagerItem)
            {
                RadComboBox PageSizeCombo = (RadComboBox)e.Item.FindControl("PageSizeComboBox");

                PageSizeCombo.Items.Clear();
                PageSizeCombo.Items.Add(new RadComboBoxItem("20"));
                PageSizeCombo.FindItemByText("20").Attributes.Add("ownerTableViewId", rgArtikel.MasterTableView.ClientID);
                PageSizeCombo.Items.Add(new RadComboBoxItem("50"));
                PageSizeCombo.FindItemByText("50").Attributes.Add("ownerTableViewId", rgArtikel.MasterTableView.ClientID);
                PageSizeCombo.Items.Add(new RadComboBoxItem("100"));
                PageSizeCombo.FindItemByText("100").Attributes.Add("ownerTableViewId", rgArtikel.MasterTableView.ClientID);
                PageSizeCombo.Items.Add(new RadComboBoxItem("200"));
                PageSizeCombo.FindItemByText("200").Attributes.Add("ownerTableViewId", rgArtikel.MasterTableView.ClientID);
                PageSizeCombo.Items.Add(new RadComboBoxItem("300"));
                PageSizeCombo.FindItemByText("300").Attributes.Add("ownerTableViewId", rgArtikel.MasterTableView.ClientID);
                if (PageSizeCombo.FindItemByText(e.Item.OwnerTableView.PageSize.ToString()) != null)
                {
                    PageSizeCombo.FindItemByText(e.Item.OwnerTableView.PageSize.ToString()).Selected = true;
                }
            }
        } 
              
	}
}
