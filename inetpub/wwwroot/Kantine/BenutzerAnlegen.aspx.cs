using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Basics.Kundenkarten;
using System.Configuration;
using System.Security.Authentication;
using System.Security.Principal;

namespace Kantine
{
	public partial class BenutzerAnlegen : KantinePage
	{
		DataTable dt;
		string Kundennummer;

		protected void Page_Load(object sender, EventArgs e)
		{ 
            if (KB != null)
            {
                if (!(KB.IsAdmin || KB.IsUseradmin))
                {
                    Response.Redirect("Default.aspx");
                }
                else 
                {
                    if (KB.IsAdmin)
                    {
                        trAdmin.Visible = true;
                    }
                    else 
                    { 
                        trAdmin.Visible = false;
                        chkAdmin.Checked = false;
                    }
                }
            }
       
            if (Session["UserToEdit"] != null)
            {
                trPWReset.Visible = true;

                Kundennummer = Convert.ToString(Session["UserToEdit"]);
                lblÜberschrift.Text = "Benutzer bearbeiten";
                staticData.Visible = true;
                dt = DB.GetBenutzerAllByKundennummer(Kundennummer);
                FillStaticData();

                if (!Page.IsPostBack)
                {
                    
                    FillDynamicData();
                }
            }
		}

		private void FillDynamicData()
		{
			if (dt.Rows.Count > 0)
			{
				txtBenutzername.Text = dt.Rows[0]["Benutzername"].ToString();
				txtNachname.Text = dt.Rows[0]["Nachname"].ToString();
				txtVorname.Text = dt.Rows[0]["Vorname"].ToString();
                //txtPasswort.Text = dt.Rows[0]["Passwort"].ToString();
                chkAdmin.Checked = Convert.ToBoolean(dt.Rows[0]["Admin"]);
                chkUseradmin.Checked = Convert.ToBoolean(dt.Rows[0]["Useradmin"]);
				chkSeller.Checked = Convert.ToBoolean(dt.Rows[0]["Seller"]);
				chkGesperrt.Checked = Convert.ToBoolean(dt.Rows[0]["Gesperrt"]);				
			}
		}

		private void FillStaticData()
		{			
			if(dt.Rows.Count > 0)
			{
				tblStaticData.Visible = true;
				lblKundennummer.Text = dt.Rows[0]["Kundennummer"].ToString();
				lblGuthaben.Text = dt.Rows[0]["Guthaben"].ToString();
				lblKartennummer.Text = dt.Rows[0]["Kartennummer"].ToString();
			}
		}
		
		protected void btnSpeichern_Click(object sender, EventArgs e)
		{
			if (Convert.ToString(Session["UserToEdit"]) != string.Empty)
			{
				if (Convert.ToBoolean(Session["BenNaChg"]))
				{
					DB.UpdateUniversal("Benutzer", "Kundennummer", Kundennummer, "Benutzername", txtBenutzername.Text);
					Session["BenNaChg"] = null;
				}
				if (Convert.ToBoolean(Session["NacNamChg"]))
				{
					DB.UpdateUniversal("Benutzer", "Kundennummer", Kundennummer, "Nachname", txtNachname.Text);
					Session["NacNamChg"] = null;
				}
				if (Convert.ToBoolean(Session["VorNamChg"] ))
				{					
					DB.UpdateUniversal("Benutzer", "Kundennummer", Kundennummer, "Vorname", txtVorname.Text);
					Session["VorNamChg"] = null;
				}
                if (Convert.ToBoolean(Session["PasWorChg"]))
                {
                    if (txtPasswort.Text.Trim().Length > 0)
                    {
                        DB.UpdateUniversal("Benutzer", "Kundennummer", Kundennummer, "Passwort", KantinenBenutzer.PasswortVerschlüsseln(txtPasswort.Text.Trim()));   //Session["Passwort"].ToString()
                        Session["PasWorChg"] = null;
                    }
                }
				if (Convert.ToBoolean(Session["SelChg"] ))
				{
					DB.UpdateUniversal("Benutzer", "Kundennummer", Kundennummer, "Seller", Convert.ToInt16(chkSeller.Checked));
					Session["SelChg"] = null;
				}
                if (Convert.ToBoolean(Session["AdmChg"]))
                {
                    DB.UpdateUniversal("Benutzer", "Kundennummer", Kundennummer, "Admin", Convert.ToInt16(chkAdmin.Checked));
                    Session["AdmChg"] = null;
                }
                if (Convert.ToBoolean(Session["UseAdmChg"]))
                {
                    DB.UpdateUniversal("Benutzer", "Kundennummer", Kundennummer, "Useradmin", Convert.ToInt16(chkUseradmin.Checked));
                    Session["UseAdmChg"] = null;
                }                
				if (Convert.ToBoolean(Session["GspChg"]))
				{
					DB.UpdateUniversal("Benutzer", "Kundennummer", Kundennummer, "Gesperrt", Convert.ToInt16(chkGesperrt.Checked));
					Session["GspChg"] = null;
				}
			}
			else 
			{
                // Neuer Benutzer
				if (txtBenutzername.Text != "")
				{
					if (txtNachname.Text != "")
					{
						DB.AddBenutzer(txtBenutzername.Text, txtNachname.Text);
						if (Session["VorNamChg"] != null)
						{
							int BenutzerID = (int)DB.GetBenutzerAllByBenutzername(txtBenutzername.Text).Rows[0]["BenutzerID"];

							DB.UpdateUniversal("Benutzer", "BenutzerID", BenutzerID, "Vorname", txtVorname.Text);
                            DB.UpdateUniversal("Benutzer", "BenutzerID", BenutzerID, "Passwort",txtPasswort.Text.Trim());
							DB.UpdateUniversal("Benutzer", "BenutzerID", BenutzerID, "Seller", Convert.ToInt16(chkSeller.Checked));
                            DB.UpdateUniversal("Benutzer", "BenutzerID", BenutzerID, "Admin", Convert.ToInt16(chkAdmin.Checked));
                            DB.UpdateUniversal("Benutzer", "BenutzerID", BenutzerID, "Useradmin", Convert.ToInt16(chkUseradmin.Checked));
						}
					}
					else
					{
						lblError.Text = "Geben Sie einen Nachnamen ein!";
						return;
					}
				}
				else 
				{
					lblError.Text = "Geben Sie einen Benutzernamen ein!";
					return;
				}
			}

			Session["UserToEdit"] = null;
			Session["BenNaChg"] = null;
			Session["NacNamChg"] = null;
			Session["VorNamChg"] = null;
            Session["PasWorChg"] = null;
			Session["SelChg"] = null;
            Session["AdmChg"] = null;
            Session["UseAdmChg"] = null; 
			Session["GspChg"] = null;

			Response.Redirect("./Benutzer.aspx");
		}

		protected void btnBack_Click(object sender, EventArgs e)
		{
			Session["UserToEdit"] = null;
			Session["BenNaChg"] = null;
			Session["NacNamChg"] = null;
			Session["VorNamChg"] = null;
            Session["PasWorChg"] = null;
			Session["SelChg"] = null;
            Session["AdmChg"] = null;
            Session["UseAdmChg"] = null; 
            Session["GspChg"] = null;

			Response.Redirect("./Benutzer.aspx");
		}

		protected void txtBenutzername_TextChanged(object sender, EventArgs e)
		{
			Session["BenNaChg"] = true;
		}

		protected void txtNachname_TextChanged(object sender, EventArgs e)
		{
			Session["NacNamChg"] = true;
		}

		protected void txtVorname_TextChanged(object sender, EventArgs e)
		{
			Session["VorNamChg"] = true;
		}

        protected void txtPasswort_TextChanged(object sender, EventArgs e)
        {
            Session["PasWorChg"] = true;            
        }	

		protected void chkSeller_CheckedChanged(object sender, EventArgs e)
		{
			Session["SelChg"] = true;
		}

        protected void chkAdmin_CheckedChanged(object sender, EventArgs e)
        {
            Session["AdmChg"] = true;
        }

        protected void chkUseradmin_CheckedChanged(object sender, EventArgs e)
        {
            Session["UseAdmChg"] = true;
        }

		protected void chkGesperrt_CheckedChanged(object sender, EventArgs e)
		{
			Session["GspChg"] = true;
		}

		protected void btnNeueKarte_Click(object sender, EventArgs e)
		{
            btnPrintNeu.Visible = true;
            btnPrint.Visible = false;

			try
			{
                FillPrinterlist();
                mpepReminder.Show();
			}
			catch (Exception ex)
			{
				lblError.Text = ex.Message;
				lblError.Visible = true;
			}
		}

        protected void btnPWReset_Click(object sender, EventArgs e)
        {
            DB.UpdateUniversal("Benutzer", "Kundennummer", Kundennummer, "Passwort", "");
            if (DB.Exception.Length > 0)
            {
                lblPWReset.Text = "Fehler beim zurücksetzen des Passwortes";
                lblPWReset.ForeColor = System.Drawing.Color.Red;
            }
            else 
            { 
                lblPWReset.Text = "Passwort zurückgesetzt!";
                lblPWReset.ForeColor = System.Drawing.Color.Green;
                txtPasswort.Text = string.Empty;
            }            
        }

        protected void btnCardLost_Click(object sender, EventArgs e)
        {
            btnPrintNeu.Visible = false;
            btnPrint.Visible = true;

            try
			{
                FillPrinterlist();
                mpepReminder.Show();
            }
            catch (Exception ex)
            {
                lblError.Text = ex.Message;
                lblError.Visible = true;
            }
        }

        protected void btnPrintNeu_Click(object sender, EventArgs e)
        {
            try
            {
#if DEBUG
                //new Kundenkarte((string)dt.Rows[0]["Benutzername"], Kundennummer, (int)dt.Rows[0]["Kartennummer"]).Drucken(ddlPrinters.SelectedValue, "VMS052");
                new Kundenkarte((string)dt.Rows[0]["Benutzername"], Kundennummer, (int)dt.Rows[0]["Kartennummer"]).Drucken(ddlPrinters.SelectedValue);//"IT_Kyo_1020_P128 auf VMS052"
#else
                new Kundenkarte((string)dt.Rows[0]["Benutzername"], Kundennummer, (int)dt.Rows[0]["Kartennummer"]).Drucken(ddlPrinters.SelectedValue);
                //new Kundenkarte((string)dt.Rows[0]["Benutzername"], Kundennummer, (int)dt.Rows[0]["Kartennummer"]).Drucken("Zebra P330i Card Printer USB");
# endif

            }
            catch (Exception ex)
            {
                lblError.Text = ex.Message;
                lblError.Visible = true;
            }
        }

        protected void btnPrint_Click(object sender, EventArgs e)
        {
            try
            {
                int KartennrNeu = DB.KartennummerNeuGenerieren(Kundennummer);
                DB.UpdateUniversal("Benutzer", "Kundennummer", Kundennummer, "Kartennummer", KartennrNeu);
                dt = DB.GetBenutzerAllByKundennummer(Kundennummer);
                FillStaticData();
#if DEBUG
                //new Kundenkarte((string)dt.Rows[0]["Benutzername"], Kundennummer, KartennrNeu).Drucken(ddlPrinters.SelectedValue,"VMS052");
                new Kundenkarte((string)dt.Rows[0]["Benutzername"], Kundennummer, KartennrNeu).Drucken(ddlPrinters.SelectedValue);//"IT_Kyo_1020_P128 auf VMS052"
#else
                new Kundenkarte((string)dt.Rows[0]["Benutzername"], Kundennummer, KartennrNeu).Drucken(ddlPrinters.SelectedValue);
                // new Kundenkarte((string)dt.Rows[0]["Benutzername"], Kundennummer, KartennrNeu).Drucken("Zebra P330i Card Printer USB");
# endif

                
            }
            catch (Exception ex)
            {
                lblError.Text = ex.Message;
                lblError.Visible = true;
            }
        }

        protected void ibtnRefreshPrinter_Click(object sender, ImageClickEventArgs e)
        {
            FillPrinterlist();
            mpepReminder.Show();
        }

        private void FillPrinterlist()
        {
            ddlPrinters.Items.Clear();            
            ddlPrinters.Items.Add("Zebra P330i Card Printer USB");
            
            //foreach (string s in System.Drawing.Printing.PrinterSettings.InstalledPrinters)
            //{
            //    ddlPrinters.Items.Add(s);
            //}

            //List<string> list = new Kundenkarte((string)dt.Rows[0]["Benutzername"], Kundennummer, (int)dt.Rows[0]["Kartennummer"]).GetDrucker("VMS052");
            //foreach (string s in list)
            //{
            //    ddlPrinters.Items.Add(s);
            //}       

            foreach (ListItem item in ddlPrinters.Items)
            {
                if (item.Text.ToUpper().Contains("ZEBRA"))
                {
                    item.Selected = true;
                }
            }
        }
	}
}
