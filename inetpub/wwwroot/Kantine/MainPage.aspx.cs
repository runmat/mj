using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace Kantine
{
    /// <summary>
    /// Verkaufsseite / Hauptseite
    /// </summary>
	public partial class MainPage : KantinePage
	{
		DataTable dtArtikel = new DataTable();
		DataTable dtBestellung = new DataTable();
        Zahlenfeld zfSG;
        
        /// <summary>
        /// PageLoad-Event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
		protected void Page_Load(object sender, EventArgs e)
		{
            if (KB != null)
            {
                if (!(KB.IsSeller || KB.IsNachmittag))
                {
                    Response.Redirect("Default.aspx");
                }                    
            }

            txtKundennummer.Focus();
            zfSG = (Zahlenfeld)lstvArtikel.FindControl("zfSG");                
            if (!IsPostBack)
            {
                ViewManager("Start");
            }
            else
            {
                PrepareBestellung(false);
            }     			
		}

		protected void UpArtikel_Load(object sender, EventArgs e)
		{
			FillArtikel();
			FillArtikelButtons();
		}

		private void FillArtikelButtons()
		{
            if (KB != null)
            {
                if (!KB.IsNachmittag)
                {
                    //Mittag
                    lstvArtikel.DataSource = DB.GetArtikelAllByWarengruppeID(1);
                    lstvArtikel.DataBind();
                }

                //if (!KB.IsNachmittag)
                //{
                //    //Brötchen
                //    lstvArtikel2.DataSource = DB.GetArtikelAllByWarengruppeID(2);
                //    lstvArtikel2.DataBind();
                //}

                //Getränke
                DataTable tmpDt = DB.GetArtikelAllByWarengruppeID(3);
                if (KB.IsNachmittag)
                {
                    foreach (DataRow row in tmpDt.Rows)
                    {
                        if ((string)row["Artikelbezeichnung"] == "Pfand")
                        {
                            row.Delete();
                            break;
                        }
                    }
                }
                lstvArtikel5.DataSource = tmpDt;
                lstvArtikel5.DataBind();
            }

            //Backwaren
            lstvArtikel2.DataSource = DB.GetArtikelAllByWarengruppeID(2);
            lstvArtikel2.DataBind();

			//Süßwaren
			lstvArtikel3.DataSource = DB.GetArtikelAllByWarengruppeID(5);
			lstvArtikel3.DataBind();

			//Eis
			lstvArtikel4.DataSource = DB.GetArtikelAllByWarengruppeID(4);
			lstvArtikel4.DataBind();

		}

		protected void UpArtikel_Unload(object sender, EventArgs e)
		{
			Session["offeneArtikel"] = dtBestellung;
		}

		protected void FillArtikel()
		{
			dtArtikel = DB.GetAllArtikel();
		}

		protected void Button_Click(object sender, EventArgs e)
		{
			AusgabeManager("none",null);

			if (sender is Button)
			{
				Button bt = (Button)sender;

				DataRow[] Rows;
				Rows = dtArtikel.Select("Artikelbezeichnung = '" + bt.Text + "'");
				for (int i = 0; i < Rows.GetLength(0); i++)
				{
					if (Rows[i]["Artikelbezeichnung"].ToString() == bt.Text)
					{
						dtBestellung.Rows.Add(new object[] { (string)Rows[i]["Artikelbezeichnung"], (decimal)(Rows[i]["Preis"]) });
						lstvRechnung.DataSource = dtBestellung.DefaultView;
						lstvRechnung.DataBind();
					}
				}
				SummeBilden();
			}
		}

        /// <summary>
        /// Bildet die Summe für die Einkaufsliste
        /// </summary>
		protected void SummeBilden()
		{
			decimal Summe = 0.00m;
			foreach (DataRow Row in dtBestellung.Rows)
			{
				if (!(Row["Preis"] is DBNull))
				{
					Summe += Convert.ToDecimal(Row["Preis"]);
				}
			}
			lblSumme.Text = Summe.ToString();
		}

        /// <summary>
        /// Schreibt einen Eintrag ins Log
        /// </summary>
        /// <param name="Aktion"></param>
        /// <param name="User"></param>
        /// <param name="Row"></param>
        /// <param name="Kundennummer"></param>
	    protected void LogdatenSchreiben(string Aktion,string User,DataRow Row,string Kundennummer)
		{
			DB.AddVerkaufsLogEntry(DateTime.Now, Kundennummer, User, Aktion, Convert.ToString(Row["Artikel"]), Convert.ToDecimal(Row["Preis"]));
		}

		protected void btnBack_Click(object sender, EventArgs e)
		{
			AusgabeManager("none", null);

			dtBestellung.Rows.RemoveAt(dtBestellung.Rows.Count - 1);
			lstvRechnung.DataBind();
			SummeBilden();
		}

		protected void btnBuchen_Click(object sender, EventArgs e)
		{
			bool offeneArtikel = false;

			// neue Tabelle erstellen für offene Artikel
			DataTable dtOffen = new DataTable();
			dtOffen.Columns.Add("Artikel");
			dtOffen.Columns.Add("Preis");
			dtOffen.Columns.Add("Delete");

			AusgabeManager("none", null);

			decimal dGuthaben = Convert.ToDecimal(lblGuthaben.Text);
			foreach (DataRow Row in dtBestellung.Rows)
			{
				try
				{					
					decimal dPreis = Convert.ToDecimal(Row["Preis"]);
					if (dGuthaben - dPreis > 0)
					{
						DB.Buchen(lblKundennummer.Text, dPreis);
						dGuthaben = dGuthaben - dPreis;
						AusgabeManager("message", "Buchung  erfolgreich! Restguthaben: " + DB.GetBenutzerAllByKundennummer(lblKundennummer.Text).Rows[0]["Guthaben"] + "€"); 
						Row["Delete"]=true;
					}
					else if (dGuthaben - dPreis < -5)
					{ 						
						AusgabeManager("error", "Der Artikel " + (string)Row["Artikel"] + " konnte nicht gebucht werden. Die Kreditgrenze ist erreicht!");
						offeneArtikel = true;
						break;
					}
					else
					{
						offeneArtikel = true;
						DB.Buchen(lblKundennummer.Text, dPreis);
						Row["Delete"] = true;
						ViewManager("UpdateUserdaten");
						AusgabeManager("error","Der aktuelle Kontostand beträgt " + lblGuthaben.Text + "€!");
						
						//lblMessageboxText.Text = "Das Guthaben reicht nicht aus um den Artikel " + (string)Row["Artikel"] + " zukaufen! Trotzdem Buchen?";
						//divMessagebox.Visible = true;
						//Session["ActionCaller"] = "btnBuchen";

						//mpeOK.Show();
						//UpMessagebox.Update();
						//Label lblTemp = (Label)mpeOK.TemplateControl.FindControl("lblMessageboxText");
						//lblTemp.Text = "Das Guthaben reicht nicht aus um den Artikel " + (string)Row["Artikel"] + " zukaufen! Trotzdem Buchen?";
					}										
				}
				catch
				{
					if (lblKundennummer.Text == "")
					{
						lblError.Text = "Es wurde keine Kundennummer eingegeben auf die gebucht werden soll.";
					}
					else
					{
						lblError.Text = "Es konnten nicht alle Artikel gebucht werden!";
					}
					break;
				}

				try
				{
					LogdatenSchreiben("Buchung", Page.User.Identity.Name, Row, lblKundennummer.Text);
				}
				catch { lblError.Text = "Fehler beim Schreiben des Logfiles!"; }
			}

			if (offeneArtikel)
			{
				// Filtern der offenen Artikel
				foreach (DataRow Row2 in dtBestellung.Rows)
				{
					if (Row2["Delete"] is DBNull || Convert.ToBoolean(Row2["Delete"]) == false)
					{
						object[] objRow = new object[] { Row2["Artikel"], Row2["Preis"], false };
						dtOffen.Rows.Add(objRow);
					}
				}
				dtBestellung = dtOffen;
				Session["offeneArtikel"] = dtBestellung; 
				lstvRechnung.DataBind();

				if (dtOffen.Rows.Count > 0)
				{ 
					ViewManager("Artikelwahl"); 
				}
				else 
				{
					ViewManager("Start");
				}
			}
			else
			{
				ClearData();				
			}
			
		}

		//public void mpeOK_Init(object sender, EventArgs e)
		//{
		//    if ((string)Session["ActionCaller"] == "btnBuchen")
		//    {
		//        CssStyleCollection csc = divMessagebox.Style;
		//        csc.Remove("visibility");
		//        csc.Add("visibility", "true");
		//    }
		//    //else{divMessagebox.Visible=false;}
		//}

		private void ClearData()
		{
		    lblSumme.Text = "0,00";
		    txtKundennummer.Text = null;
			lblKundennummer.Text = null;
		    lblKundenname.Text = null;
		    lblGuthaben.Text = "0,00";
			ViewManager("Start");
		}

		protected void AusgabeManager(string Ausgabe, string Text)
		{
			switch (Ausgabe) 
			{ 
				case "none":
					lblAusgabe.Text = null;
					lblError.Text = null;
					lblAusgabe.Visible = false;
					lblError.Visible = false;
					break;
				case "error":
					lblAusgabe.Text = null;
					lblError.Text = Text;
					lblAusgabe.Visible = false;
					lblError.Visible = true;
					break;
				case "message":
					lblAusgabe.Text = Text;
					lblError.Text = null;
					lblAusgabe.Visible = true;
					lblError.Visible = false;
					break;
				default:
					lblAusgabe.Text = null;
					lblError.Text = null;
					lblAusgabe.Visible = false;
					lblError.Visible = false;
					break;
			}
		}

		protected void txtKundennummer_Changed(object sender, EventArgs e)
		{
			KundennummerEntered();
		}

		private void KundennummerEntered()
		{
			AusgabeManager("none", null);

			if (txtKundennummer.Text.Length == 10)
			{
				try
				{
					DataTable dt = new DataTable();
					dt = DB.GetBenutzerAllByKundennummer(txtKundennummer.Text.Substring(0, 8));
					if (dt.Rows.Count == 0)
					{
						AusgabeManager("error", "Es existiert kein Benutzer mit dieser Kundennummer!");
					}
					else
					{
						if (txtKundennummer.Text.Substring(8) == Convert.ToString(dt.Rows[0]["Kartennummer"]) || 
							txtKundennummer.Text.Substring(8) == (Convert.ToString(0) + Convert.ToString(dt.Rows[0]["Kartennummer"])))
						{
							lblKundennummer.Text = txtKundennummer.Text.Substring(0, 8);
							lblKundenname.Text = (string)dt.Rows[0]["Benutzername"];
							lblGuthaben.Text = dt.Rows[0]["Guthaben"].ToString();
							
							ViewManager("Artikelwahl");
						}
						else 
						{
							AusgabeManager("error", "Die verwendete Kundenkarte ist nicht aktuell!");
						}
					}
				}
				catch
				{
					AusgabeManager("error", "Die eingegebene Kundennummer ist nicht korrekt!");
				}
			}
			else
			{
				AusgabeManager("error", "Die eingegebene Kundennummer ist nicht korrekt!");

			}
		}

		protected void btn5Euro_Click(object sender, EventArgs e)
		{
			if (sender is Button)
			{
				Button bt = (Button)sender;

				dtBestellung.Rows.Add(new object[] { "Guthaben +5€", -5.00m });
				lstvRechnung.DataSource = dtBestellung.DefaultView;
				lstvRechnung.DataBind();
				
				SummeBilden();
			}
		}

		protected void btn10Euro_Click(object sender, EventArgs e)
		{
			if (sender is Button)
			{
				Button bt = (Button)sender;

				dtBestellung.Rows.Add(new object[] { "Guthaben +10€", -10.00m });
				lstvRechnung.DataSource = dtBestellung.DefaultView;
				lstvRechnung.DataBind();

				SummeBilden();
			}
		}

		protected void btn15Euro_Click(object sender, EventArgs e)
		{
			if (sender is Button)
			{
				Button bt = (Button)sender;

				dtBestellung.Rows.Add(new object[] { "Guthaben +15€", -15.00m });
				lstvRechnung.DataSource = dtBestellung.DefaultView;
				lstvRechnung.DataBind();

				SummeBilden();
			}
		}

		protected void btn20Euro_Click(object sender, EventArgs e)
		{
			if (sender is Button)
			{
				Button bt = (Button)sender;

				dtBestellung.Rows.Add(new object[] { "Guthaben +20€", -20.00m });
				lstvRechnung.DataSource = dtBestellung.DefaultView;
				lstvRechnung.DataBind();

				SummeBilden();
			}
		}

		protected void btn50Euro_Click(object sender, EventArgs e)
		{
			if (sender is Button)
			{
				Button bt = (Button)sender;

				dtBestellung.Rows.Add(new object[] { "Guthaben +50€", -50.00m });
				lstvRechnung.DataSource = dtBestellung.DefaultView;
				lstvRechnung.DataBind();

				SummeBilden();
			}
		}

		protected void btnFreierBetrag_Click(object sender, EventArgs e)
		{
			zfFreierBetrag.ChangeVisibility();
		}

		protected void  btnSondergericht_Click(object sender, EventArgs e)
		{
			zfSG.ChangeVisibility();			
		}

		protected void zfSondergericht_Commit(object sender, EventArgs e)
		{
			dtBestellung.Rows.Add(new object[] { "Sondergericht ", zfSG.Value });
			lstvRechnung.DataBind();

			SummeBilden();
		}

		protected void zfFreierBetrag_Commit(object sender, EventArgs e)
		{
			dtBestellung.Rows.Add(new object[] { "Guthaben " + zfFreierBetrag.Value + "€", -zfFreierBetrag.Value });
			lstvRechnung.DataBind();

			SummeBilden();
		}

        /// <summary>
        /// Kontoauszug-Button-Klick
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
		protected void ibtKontoauszug_Click(object sender, EventArgs e)
		{
			if (!LayerKontoauszug.Visible)
			{
				DataTable dt = new DataTable();
				dt = DB.GetKontoauszug(lblKundennummer.Text, Zeitraum.letzerMonat,LogFilter.Kundennummer);
                dt.DefaultView.Sort = "Datum DESC";
                gvKontoauszug.DataSource = dt.DefaultView;
				gvKontoauszug.DataBind();
			}
			LayerKontoauszug.Visible = !LayerKontoauszug.Visible;	
		}

		protected void btnCancel_Click(object sender, EventArgs e)
		{
			ClearData();
		}

		protected void ibtRechner_Click(object sender, ImageClickEventArgs e)
		{
			zfKundennummer.ChangeVisibility();
		}

		protected void zfKundennummer_Commit(object sender, EventArgs e)
		{
			if (zfKundennummer.Value.ToString().Length > 10)
			{
				txtKundennummer.Text = zfKundennummer.Value.ToString().Remove(10);
			}
			else 
			{ 
				txtKundennummer.Text = zfKundennummer.Value.ToString();
			}

			KundennummerEntered();
		}

		//protected void lbtnOK_Click(object sender, EventArgs e)
		//{
		//    if((string)Session["ActionCaller"] == "btnBuchen")
		//    {
		//        decimal dPreis = Convert.ToDecimal(dtBestellung.Rows[0]["Preis"]);
		//        DB.Buchen(Convert.ToInt32(lblKundennummer.Text), dPreis);
		//        dtBestellung.Rows[0].Delete();
		//        lstvRechnung.DataBind();

		//        //Reset
		//        Session["ActionCaller"] = null;
		//    }
		//}

		//protected void lbtnAbbruch_Click(object sender, EventArgs e)
		//{
		//    if ((string)Session["ActionCaller"] == "btnBuchen")
		//    {
		//        //Reset
		//        Session["ActionCaller"] = null;
		//    }
		//}

		//protected void mpeOK_disposed(object sender, EventArgs e)
		//{
		//    ShowVerkauf();
		//}

		protected void PrepareBestellung(bool Clear)
		{
			if (Clear)
			{
				dtBestellung = null;
				Session["offeneArtikel"] = null;
				lstvRechnung.DataBind();
			}
			else
			{
				if (Session["offeneArtikel"] != null)
				{
					dtBestellung = (DataTable)Session["offeneArtikel"];
				}
				else if(dtBestellung.Columns.Count==0)
				{
					dtBestellung.Columns.Add("Artikel");
					dtBestellung.Columns.Add("Preis");
					dtBestellung.Columns.Add("Delete");
				}
				lstvRechnung.DataSource = dtBestellung.DefaultView;
				lstvRechnung.DataBind();
			}
		}

		protected void ViewManager(string Status)
		{
			switch (Status)
			{ 
				case "Start":
					//Artikel ausblenden
					tblVerkauf.Visible = false;
					tblButtons.Visible = false;

					//Kundendaten leeren
					txtKundennummer.Text = null;
					lblGuthaben.Text = "0,00";
					lblSumme.Text = "0,00";
					lblKundennummer.Text = null;
					lblKundenname.Text = null;
					PrepareBestellung(true);
					
					//visible
					txtKundennummer.Visible = true;
					lblGuthaben.Visible=true;

					//hidden
					lblKundennummer.Visible = false;
					tdKontoauszug.Visible = false;

                    
                    if (KB != null && !KB.IsNachmittag)
                    {
                        ibtRechner.Visible = true;
                    }
                    else
                    {
                        ibtRechner.Visible = false;
                    }
                  
					break;
				case "Artikelwahl":
					//Artikel einblenden
					tblVerkauf.Visible = true;
					tblButtons.Visible = true;

					//visible
					lblKundennummer.Visible = true;
					tdKontoauszug.Visible = true;
					lblGuthaben.Visible = true;
					tdKontoauszug.Visible = true;
					
					//hidden
					txtKundennummer.Visible = false;
					ibtRechner.Visible = false;
					LayerKontoauszug.Visible = false;

                   	if (KB != null && KB.IsNachmittag)
					{
						tpGuthaben.Enabled = false;
						tpMittag.Enabled = false;
					}
					else 
					{
						tpGuthaben.Enabled = true;
						tpMittag.Enabled = true;
					}
					
					//Daten
					PrepareBestellung(false);					
					break;
				case "UpdateUserdaten":
					DataTable dtTemp = DB.GetBenutzerAllByKundennummer(txtKundennummer.Text.Substring(0,8));
					lblGuthaben.Text = Convert.ToString(dtTemp.Rows[0]["Guthaben"]);
					lblKundenname.Text = (string)dtTemp.Rows[0]["Benutzername"];
					break;
				default:
					break;
			}
		}
	}
}
