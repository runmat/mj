using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Data;
using Telerik.Web.UI;

namespace Kantine
{
    public partial class Verkaufsübersicht : KantinePage
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
                        txtVon.Text = "01." + DateTime.Today.Month.ToString().PadLeft(2,'0') + "." + DateTime.Today.Year.ToString();
                        txtBis.Text = DateTime.DaysInMonth(DateTime.Today.Year, DateTime.Today.Month) + "." + DateTime.Today.Month.ToString().PadLeft(2, '0') + "." + DateTime.Today.Year.ToString();

                        FillListArtikel();
                        FillGrid(true);
                    }
                }
            } 
        }

        private void FillListArtikel()
        {
            DataTable dtTemp = DB.GetAllArtikel();
            DataTable dtArtikel = dtTemp.Clone();

            DataRow Row = dtArtikel.NewRow();
            Row["Artikelbezeichnung"] = "-- Alle --";
            Row["ArtikelID"] = 0;
            dtArtikel.Rows.Add(Row);

            foreach (DataRow row in dtTemp.Rows)
            {
                DataRow NewRow = dtArtikel.NewRow();
                NewRow["Artikelbezeichnung"] = row["Artikelbezeichnung"];
                NewRow["ArtikelID"] = row["ArtikelID"];
                dtArtikel.Rows.Add(NewRow);
            }

            ddlArtikel.DataTextField = "Artikelbezeichnung";
            ddlArtikel.DataValueField = "ArtikelID";
            DataView dv = dtArtikel.DefaultView;
            dv.Sort = "Artikelbezeichnung asc";
            ddlArtikel.DataSource = dv;
            ddlArtikel.DataBind();
        }

        protected void rgAuszugNeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            FillGrid(false);
        }

        protected void rgAuszugPageIndexChanged(object sender, GridPageChangedEventArgs e)
        {

        }

        protected void rgAuszugPageSizeChanged(object sender, GridPageSizeChangedEventArgs e)
        {

        }

        private void FillGrid(bool Rebind = true)
        {
            try
            {
                DataTable dt;
                DateTime Von;
                DateTime Bis;

                if (DateTime.TryParse(txtVon.Text, out Von) && DateTime.TryParse(txtBis.Text, out Bis))
                {
                    string strArtikel = ddlArtikel.SelectedItem.Text;
                    if (strArtikel != "" && strArtikel != "-- Alle --")
                    {
                        dt = DB.GetKontoauszug(Von, Bis, strArtikel);
                    }
                    else
                    {
                        dt = DB.GetKontoauszug(Von, Bis);
                    }

                    dt.DefaultView.Sort = "Datum DESC";
                    Session["VerkaufsübersichtListe"] = dt.DefaultView;
                    DataView dv = (DataView)Session["VerkaufsübersichtListe"];

                    // RadGrid
                    rgAuszug.DataSource = dv;
                    if (Rebind)
                    {
                        rgAuszug.Rebind();
                    }

                    //Label lblSumme = (Label)UP1.FindControl("lblSumme");
                    
                    if (dt.Rows.Count > 0)
                    {
                        decimal Summe = 0m;
                        foreach (DataRow row in dt.Rows)
                        {
                            Summe += Convert.ToDecimal(row["Betrag"]);
                        }
                        lblSumme.Text = Summe.ToString().Replace('.', ',');
                    }
                    else { lblSumme.Text = "0,00"; }
                }
                else
                {
                    lblError.Text = "Kein gültiger Zeitraum. Bitte prüfen Sie die Datumsfelder!";
                }

               
            }
            catch (Exception ex)
            {
                lblError.Text = ex.Message;
            }		
                      
        }

        protected void btnShow_Click(object sender, EventArgs e)
        {
            FillGrid();
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