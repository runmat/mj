using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;

namespace Kantine
{
	public partial class ArtikelAnlegen : KantinePage
	{
        DataTable dt;
		int Artikelnummer;

        protected void Page_Load(object sender, EventArgs e)
		{
            if (KB != null)
            {
                if (!(KB.IsAdmin))
                {
                    Response.Redirect("Default.aspx");
                }
            }
            
			if (!Page.IsPostBack)
			{
				FillddlWarengruppen();
			}

			if (Session["ArticleToEdit"] != null)
			{
				Artikelnummer = Convert.ToInt32(Session["ArticleToEdit"]);
				lblÜberschrift.Text = "Artikel bearbeiten";
				dt = DB.GetArtikelAllByID(Artikelnummer);
				
				if (!Page.IsPostBack)
				{
					FillDynamicData();
				}
			}
		}

		private void FillddlWarengruppen()
		{
			DataTable dtWarengruppen = new DataTable();
			dtWarengruppen = DB.GetAllWarengruppen();
			dtWarengruppen.Rows.Add();
			dtWarengruppen.Rows[dtWarengruppen.Rows.Count - 1]["BezeichnungWarengruppe"] = "-- Keine --";
			ddlWarengruppe.DataSource = dtWarengruppen;
			ddlWarengruppe.DataBind();
		}

		private void FillDynamicData()
		{
			if (dt.Rows.Count > 0)
			{
				txtArtikelbezeichnung.Text = dt.Rows[0]["Artikelbezeichnung"].ToString();
				txtPreis.Text = dt.Rows[0]["Preis"].ToString();
				txtEAN.Text = dt.Rows[0]["EAN"].ToString();
				ddlWarengruppe.SelectedValue = Convert.ToString(dt.Rows[0]["WarengruppeID"]);
			}
		}

		protected void btnSpeichern_Click(object sender, EventArgs e)
		{
			if (Session["ArticleToEdit"] != null) //Wenn null neuer Artikel
			{
				if (Session["ArtBeChg"] != null)
				{
					DB.UpdateUniversal("Artikel", "ArtikelID", Artikelnummer, "Artikelbezeichnung", txtArtikelbezeichnung.Text);
					Session["ArtBeChg"] = null;
				}
				if (Session["PreisChg"] != null)
				{
					DB.UpdateUniversal("Artikel", "ArtikelID", Artikelnummer, "Preis", Convert.ToDecimal(txtPreis.Text));
					Session["PreisChg"] = null;
				}
				if (Session["EANChg"] != null)
				{
					if (txtEAN.Text != "")
					{
                        long uiEAN;
                        if (long.TryParse(txtEAN.Text, out uiEAN))
                        {
                            DB.UpdateUniversal("Artikel", "ArtikelID", Artikelnummer, "EAN", uiEAN);
                        }
                        else
                        {
                            DB.UpdateUniversal("Artikel", "ArtikelID", Artikelnummer, "EAN", null);
                        }
					}
					else 
					{
						DB.UpdateUniversal("Artikel", "ArtikelID", Artikelnummer, "EAN", null);
					}
					Session["EANChg"] = null;
				}
				if (Session["WarGrpChg"] != null)
				{
					if(ddlWarengruppe.SelectedValue != "")
					{
						DB.UpdateUniversal("Artikel", "ArtikelID", Artikelnummer, "WarengruppeID", Convert.ToInt32(ddlWarengruppe.SelectedValue));
						Session["WarGrpChg"] = null;
					}
					else
					{
						DB.UpdateUniversal("Artikel", "ArtikelID", Artikelnummer, "WarengruppeID", null);
					}
				}
				
			}
			else
			{
				if (txtArtikelbezeichnung.Text != "")
				{
					DB.AddArtikel(txtArtikelbezeichnung.Text);
					Artikelnummer = (int)DB.GetArtikelAllByBezeichnung(txtArtikelbezeichnung.Text).Rows[0]["ArtikelID"];
					
					if (Session["PreisChg"] != null)
					{
						DB.UpdateUniversal("Artikel", "ArtikelID", Artikelnummer, "Preis", Convert.ToDecimal(txtPreis.Text));
					}

					if(Session["EANChg"] != null)
					{
                        if (txtEAN.Text != "")
                        {
                            long uiEAN;
                            if (long.TryParse(txtEAN.Text, out uiEAN))
                            {
                                DB.UpdateUniversal("Artikel", "ArtikelID", Artikelnummer, "EAN", uiEAN);
                            }
                            else
                            {
                                DB.UpdateUniversal("Artikel", "ArtikelID", Artikelnummer, "EAN", null);
                            }
                        }
                        else
                        {
                            DB.UpdateUniversal("Artikel", "ArtikelID", Artikelnummer, "EAN", null);
                        }                        
					}

                    // # Warengruppe immer aktuell halten
                    //if( Session["WarGrpChg"] != null)
                    //{
						if (ddlWarengruppe.SelectedValue != "")
						{
							DB.UpdateUniversal("Artikel", "ArtikelID", Artikelnummer, "WarengruppeID", Convert.ToInt32(ddlWarengruppe.SelectedValue));
						}
						else 
						{
							DB.UpdateUniversal("Artikel", "ArtikelID", Artikelnummer, "WarengruppeID", null);
						}
					//}
                    // #
				}
			}

			Session["ArticleToEdit"] = null;
			Session["ArtBeChg"] = null;
			Session["PreisChg"] = null;
			Session["EANChg"] = null;
			Session["WarGrpChg"] = null;

			Response.Redirect("./Artikel.aspx");
		}

		protected void btnBack_Click(object sender, EventArgs e)
		{
			Session["ArticleToEdit"] = null;
			Session["ArtBeChg"] = null;
			Session["PreisChg"] = null;
			Session["EANChg"] = null;
			Session["WarGrpChg"] = null;

			Response.Redirect("./Artikel.aspx");
		}

		protected void txtArtikelbezeichnung_TextChanged(object sender, EventArgs e)
		{
			Session["ArtBeChg"] = true;
		}

		protected void txtPreis_TextChanged(object sender, EventArgs e)
		{
			Session["PreisChg"] = true;
		}

		protected void txtEAN_TextChanged(object sender, EventArgs e)
		{
			Session["EANChg"] = true;
		}

		public void ddlWarengruppe_TextChanged(object sender, EventArgs e)
		{
			Session["WarGrpChg"] = true;
		}
	}
}
