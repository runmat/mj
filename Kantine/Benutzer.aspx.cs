using System;
using System.Data;
using Telerik.Web.UI;

namespace Kantine
{
	public partial class Benutzer : KantinePage
	{
        protected void Page_Load(object sender, EventArgs e)
		{
            if (KB != null)
            {
                if (!(KB.IsAdmin || KB.IsSeller || KB.IsUseradmin))
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
                DataTable dtBenutzer = DB.GetAllBenutzerWithoutBC();
                dtBenutzer.DefaultView.Sort = "Benutzername ASC";
                Session["BenutzerListe"] = dtBenutzer.DefaultView;

                DataView dv = (DataView)Session["BenutzerListe"];

                // RadGrid
                rgBenutzer.DataSource = dv;
                if (Rebind)
                {
                    rgBenutzer.Rebind();
                }
            }
            catch (Exception ex)
            {
                lblError.Text = ex.Message;
            }
		}

		protected void btnNewUser_Click(object sender, EventArgs e)
		{
			Response.Redirect("./BenutzerAnlegen.aspx");
		}

        public void rgBenutzer_ItemCommand(object sender, GridCommandEventArgs e)
		{
			string Command = e.CommandName.ToString();
          
            if (Command == "Bearbeiten")
			{
				string Kundennummer = e.Item.Cells[5].Text;
				Session["UserToEdit"] = Kundennummer;
				Response.Redirect("./BenutzerAnlegen.aspx");
			}
			else if(Command == "Löschen")
			{
				string Kundennummer = e.Item.Cells[5].Text;
				DB.DeleteBenutzer(Kundennummer,false);
				lblAusgabe.Text = "Benutzer gelöscht!";
				lblAusgabe.Visible = true;
				Response.Redirect("./Benutzer.aspx");
				UP1.Update();
			}
			else if (Command == "PWReset")
			{
				string Kundennummer = e.Item.Cells[3].Text;
				DB.UpdateUniversal("Benutzer", "Kundennummer", Kundennummer, "Passwort", "");
				lblAusgabe.Text = "Passwort zurückgesetzt!";
				lblAusgabe.Visible = true;
				UP1.Update();
			}		
		}

        protected void rgBenutzerNeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            FillGrid(false);
        }

        protected void rgBenutzerPageIndexChanged(object sender, GridPageChangedEventArgs e)
        {

        }

        protected void rgBenutzerPageSizeChanged(object sender, GridPageSizeChangedEventArgs e)
        {

        }

        protected void rgBenutzer_ItemDataBound(object sender, GridItemEventArgs e)
        {
            if (e.Item is GridPagerItem)
            {
                RadComboBox PageSizeCombo = (RadComboBox)e.Item.FindControl("PageSizeComboBox");

                PageSizeCombo.Items.Clear();
                PageSizeCombo.Items.Add(new RadComboBoxItem("20"));
                PageSizeCombo.FindItemByText("20").Attributes.Add("ownerTableViewId", rgBenutzer.MasterTableView.ClientID);
                PageSizeCombo.Items.Add(new RadComboBoxItem("50"));
                PageSizeCombo.FindItemByText("50").Attributes.Add("ownerTableViewId", rgBenutzer.MasterTableView.ClientID);
                PageSizeCombo.Items.Add(new RadComboBoxItem("100"));
                PageSizeCombo.FindItemByText("100").Attributes.Add("ownerTableViewId", rgBenutzer.MasterTableView.ClientID);
                PageSizeCombo.Items.Add(new RadComboBoxItem("200"));
                PageSizeCombo.FindItemByText("200").Attributes.Add("ownerTableViewId", rgBenutzer.MasterTableView.ClientID);
                PageSizeCombo.Items.Add(new RadComboBoxItem("300"));
                PageSizeCombo.FindItemByText("300").Attributes.Add("ownerTableViewId", rgBenutzer.MasterTableView.ClientID);
                if (PageSizeCombo.FindItemByText(e.Item.OwnerTableView.PageSize.ToString()) != null)
                {
                    PageSizeCombo.FindItemByText(e.Item.OwnerTableView.PageSize.ToString()).Selected = true;
                }
            }
        } 
	}
}
