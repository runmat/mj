using System;
using CKG.Base.Kernel.Common;
using CKG.Base.Kernel.Security;
using System.Data;

namespace AppZulassungsdienst.forms
{   
    /// <summary>
    /// Detailseite des gewählten Zulassungsdienstes(Adresse, Telefon usw. ).
    /// </summary>
    public partial class Report30ZLD_2 : System.Web.UI.Page
    {
        private CKG.Base.Kernel.Security.User m_User;
        private CKG.Base.Kernel.Security.App m_App;
        
        /// <summary>
        /// Daten aus der Session ziehen und anzeigen
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            m_User = Common.GetUser(this);

            m_App = new App(m_User); //erzeugt ein App_objekt 

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
                    if ((Request.QueryString["ID"] == null) || (Request.QueryString["ID"].Length == 0))
                    {
                        lblError.Text = "Feher: Die Seite wurde ohne Angaben zum Zulassungsdienst aufgerufen.";
                    }
                    else 
                    { 
                        DataRow [] rows = tblResultTableRaw.Select("ID = " + Request.QueryString["ID"]);
                        Label1.Text = rows[0]["NAME1"].ToString();
                        Label2.Text = rows[0]["ANSPRECHPARTNER"].ToString();
                        Label3.Text = rows[0]["NAME1"].ToString();
                        Label4.Text = rows[0]["NAME2"].ToString();
                        Label5.Text = rows[0]["STREET"].ToString();
                        Label6.Text = rows[0]["HOUSE_NUM1"].ToString();
                        Label7.Text = rows[0]["POST_CODE1"].ToString();
                        Label8.Text = rows[0]["CITY1"].ToString();
                        Label9.Text = rows[0]["TELE1"].ToString();
                        Label10.Text = rows[0]["TELE2"].ToString();
                        Label11.Text = rows[0]["TELE3"].ToString();
                        Label12.Text = rows[0]["FAX_NUMBER"].ToString();
                        Label13.Text = rows[0]["SMTP_ADDR"].ToString();
                        Label14.Text = rows[0]["ZTXT1"].ToString();
                        Label15.Text = rows[0]["ZTXT2"].ToString();
                        Label16.Text = rows[0]["ZTXT3"].ToString();
                        Label17.Text = rows[0]["BEMERKUNG"].ToString();
                    
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
