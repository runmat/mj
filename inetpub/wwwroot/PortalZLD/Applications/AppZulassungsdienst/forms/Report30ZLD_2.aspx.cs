using System;
using System.Data;
using GeneralTools.Models;

namespace AppZulassungsdienst.forms
{   
    /// <summary>
    /// Detailseite des gewählten Zulassungsdienstes(Adresse, Telefon usw. ).
    /// </summary>
    public partial class Report30ZLD_2 : System.Web.UI.Page
    {
        /// <summary>
        /// Daten aus der Session ziehen und anzeigen
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            DataTable tblResultTableRaw = new DataTable();
            try
            {
                if (Session["ResultTableRaw"] == null)
                {
                    lblError.Text = "Fehler: Die Seite wurde ohne Kontext aufgerufen.";
                }
                else
                {
                    tblResultTableRaw = (DataTable)(Session["ResultTableRaw"]);
                }
                if (tblResultTableRaw != null) 
                {
                    if (String.IsNullOrEmpty(Request.QueryString["ID"]))
                    {
                        lblError.Text = "Feher: Die Seite wurde ohne Angaben zum Zulassungsdienst aufgerufen.";
                    }
                    else 
                    { 
                        var row = tblResultTableRaw.Select("ID = " + Request.QueryString["ID"])[0];
                        Label1.Text = row["NAME1"].ToString();
                        Label2.Text = row["ANSPRECHPARTNER"].ToString();
                        Label3.Text = row["NAME1"].ToString();
                        Label4.Text = row["NAME2"].ToString();
                        Label5.Text = row["STREET"].ToString();
                        Label6.Text = row["HOUSE_NUM1"].ToString();
                        Label7.Text = row["POST_CODE1"].ToString();
                        Label8.Text = row["CITY1"].ToString();
                        Label9.Text = row["TELE1"].ToString();
                        Label10.Text = row["TELE2"].ToString();
                        Label11.Text = row["TELE3"].ToString();
                        Label12.Text = row["FAX_NUMBER"].ToString();
                        Label13.Text = row["SMTP_ADDR"].ToString();
                        Label14.Text = row["ZTXT1"].ToString();
                        Label15.Text = row["ZTXT2"].ToString();
                        Label16.Text = row["ZTXT3"].ToString();
                        Label17.Text = row["BEMERKUNG"].ToString();
                        var bln48h = row["Z48H"].ToString().XToBool();
                        Label18.Text = (bln48h ? "Ja" : "Nein");
                        var blnGenerellAbwAdresse = row["ABW_ADR_GENERELL"].ToString().XToBool();
                        if (bln48h || blnGenerellAbwAdresse)
                        {
                            trAbwAdresse.Visible = true;
                            Label21.Text = row["Z48H_NAME1"].ToString();
                            Label22.Text = row["Z48H_NAME2"].ToString();
                            Label23.Text = row["Z48H_STREET"].ToString();
                            Label24.Text = row["Z48H_POST_CODE1"].ToString();
                            Label25.Text = row["Z48H_CITY1"].ToString();
                        }
                        else
                        {
                            trAbwAdresse.Visible = false;
                        }
                        var lifZeit = row["LIFUHRBIS"].ToString();
                        if (!String.IsNullOrEmpty(lifZeit) && lifZeit.Length > 3)
                            lifZeit = String.Format("{0}:{1}", lifZeit.Substring(0, 2), lifZeit.Substring(2, 2));
                        Label19.Text = lifZeit;
                        var blnNachreich = row["NACHREICH"].ToString().XToBool();
                        Label20.Text = (blnNachreich ? "Ja" : "Nein");
                    }
                }
            }
            catch (Exception ex)
            {
                lblError.Text = "Beim Laden der Seite ist ein Fehler aufgetreten.<br>(" + ex.Message + ")";
            }
        }
    }
}
