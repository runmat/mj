using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using CKG.Base.Kernel.Common;
using CKG.Base.Business;
using Leasing.lib;
using CKG.Base.Kernel.Security;
using System.Configuration;
using System.Data.OleDb;
using System.Data;

namespace Leasing.forms
{
    public partial class Change03 : System.Web.UI.Page
    {

        private CKG.Base.Kernel.Security.User m_User;
        private CKG.Base.Kernel.Security.App m_App;

        override protected void OnInit(EventArgs e)
        {
            this.Load += new System.EventHandler(this.Page_Load);
        }


        protected void Page_Load(object sender, EventArgs e)
        {
            m_User = Common.GetUser(this);

            Common.FormAuth(this, m_User);

            m_App = new App(m_User);

            Common.GetAppIDFromQueryString(this);

            lblHead.Text = (string)m_User.Applications.Select("AppID = '" + Session["AppID"] + "'")[0]["AppFriendlyName"];

        }

        private void Page_PreRender(object sender, System.EventArgs e)
        {
            Common.SetEndASPXAccess(this);
            
        }

        private void Page_Unload(object sender, System.EventArgs e)
        {
            Common.SetEndASPXAccess(this);
        }

        protected void NewSearch_Click(object sender, ImageClickEventArgs e)
        {
            NewSearch.Visible = false;
            NewSearchUp.Visible = true;
            lbCreate.Visible = true;
            tab1.Visible = true;
            Queryfooter.Visible = true;
           
        }

        protected void NewSearchUp_Click(object sender, ImageClickEventArgs e)
        {
            NewSearch.Visible = true;
            NewSearchUp.Visible = false;
            lbCreate.Visible = false;
            tab1.Visible = false;
            Queryfooter.Visible = false;
           
        }

        //protected void lbCreate_Click(object sender, System.EventArgs e)
        //{
        //    m_report = new LP_01(ref m_User, m_App, "");

        //    m_report.Vertragsnummer = txtVertragsnummer.Text;
        //    m_report.Fahrgestellnummer = txtFahrgestellnummer.Text;
        //    m_report.Kennzeichen = txtKennzeichen.Text;




        //    if (m_report.Status != 0)
        //    {
        //        lblError.Visible = true;
        //        lblError.Text = m_report.Message;
        //        Result.Visible = false;
        //        NewSearchUp.Visible = false;
        //    }
        //    else
        //    {
                
        //    }


        //}

        private void DoSubmit()
        {
            
            try
            {

                if ((txtFahrgestellnummer.Text + txtKennzeichen.Text + txtVertragsnummer.Text).ToString().Length == 0)
                {
                    lblError.Text = "Es wurden keine Suchkriterien eingegeben.";
                    return;
                }

                ClearVertragsdaten();

                LP_01 Vertrag = new LP_01(ref m_User, m_App, "");

                lblError.Text = "";


                Vertrag.Fahrgestellnummer = txtFahrgestellnummer.Text;
                Vertrag.Kennzeichen = txtKennzeichen.Text;
                Vertrag.Vertragsnummer = txtVertragsnummer.Text;

                Vertrag.FillVertragsdaten((string)Session["AppID"], (string)Session.SessionID, this.Page);


                if (Vertrag.Status == 0)
                {

                    if (Vertrag.VertragTable.Rows.Count > 0)
                    {

                        if (Session["ArvalVertragsdaten"] == null)
                        {
                            Session.Add("ArvalVertragsdaten", Vertrag.VertragTable);
                        }
                        else
                        {
                            Session["ArvalVertragsdaten"] = Vertrag.VertragTable;
                        }

                        txtFahrgestellnummer.Text = "";
                        txtKennzeichen.Text = "";
                        txtVertragsnummer.Text = "";


                        FillVertragsdaten(Vertrag.VertragTable);



                    }
                    else
                    {
                        ResetPage();


                        lblError.Text = "Keine Ergebnisse für die gewählten Kriterien.";
                        return;
                    }


                    if (Vertrag.Adressen.Rows.Count > 0)
                    {

                        if (Session["LnAdressen"] == null)
                        {
                            string[] arrAdressen = null;
                            int i = 0;


                            foreach (DataRow Row in Vertrag.Adressen.Rows)
                            {
                                Array.Resize(ref arrAdressen, i + 1);

                                arrAdressen[i] = Row["NAME1"].ToString();
                                i += 1;
                            }

                            Session.Add("LnAdressen", arrAdressen);
                            Session.Add("ArvalAdressen", Vertrag.Adressen);
                        }


                    }

                }
                else
                {
                    lblError.Text = "Fehler: " + Vertrag.Message;

                }

            }
            catch (Exception ex)
            {
                lblError.Text = "Beim Erstellen des Reportes ist ein Fehler aufgetreten.<br>(" + ex.Message + ")";
            }
        }




        private void ClearVertragsdaten()
        {
            txtNewVertragsnummer.Text = "";
            txtLeasingnehmer.Text = "";
            lblVetragsnummer.Text = "";
            lblFahrgestellnummer.Text = "";
            lblKennzeichen.Text = "";
            lblHalterName1.Text = "";
            lblHalterOrt.Text = "";
            lblName1.Text = "";
            lblName2.Text = "";
            lblStrasse.Text = "";
            lblPlzOrt.Text = "";

        }


        private void FillVertragsdaten(DataTable TempTable)
        {

        lblVetragsnummer.Text = TempTable.Rows[0]["LIZNR"].ToString();
        lblFahrgestellnummer.Text = TempTable.Rows[0]["CHASSIS_NUM"].ToString();
        lblKennzeichen.Text = TempTable.Rows[0]["LICENSE_NUM"].ToString();
        lblHalterName1.Text = TempTable.Rows[0]["NAME1_ZH"].ToString();
        lblHalterOrt.Text = TempTable.Rows[0]["CITY1_ZH"].ToString();
        lblName1.Text = TempTable.Rows[0]["NAME1_ZL"].ToString();
        lblName2.Text = TempTable.Rows[0]["NAME2_ZL"].ToString();
        lblStrasse.Text = TempTable.Rows[0]["STREET_ZL"].ToString();
        lblPlzOrt.Text = TempTable.Rows[0]["POST_CODE1_ZL"].ToString() + " " + TempTable.Rows[0]["CITY1_ZL"].ToString();

        Result.Visible = true;


        if (hField.Value == "0")
        {
            lblNoData.Visible = false;
            lbCreate.Visible = false;
            tab1.Visible = false;
            Queryfooter.Visible = false;
        }

        hField.Value = "1";

        if (tab1.Visible == false)
        {
            NewSearch.Visible = true;
            NewSearchUp.Visible = false;
        }
        else
        {
            NewSearch.Visible = false;
            NewSearchUp.Visible = true;
        }


        }

        private void ResetPage()
        {
        Result.Visible = false;

        NewSearch.Visible = false;
        NewSearchUp.Visible = false;
        lbCreate.Visible = true;
        tab1.Visible = true;
        Queryfooter.Visible = true;

        hField.Value = "0";

        Session["ArvalVertragsdaten"] = null;
        Session["LnAdressen"] = null;
        Session["ArvalAdressen"] = null;

        }

        protected void lbCreate_Click(object sender, System.EventArgs e)
        {

           DoSubmit();

        }

        protected void lbBack_Click(object sender, EventArgs e)
        {
            Session["ArvalVertragsdaten"] = null;
            Session["LnAdressen"] = null;
            Session["ArvalAdressen"] = null;
            Response.Redirect("/Services/(S(" + Session.SessionID + "))/Start/Selection.aspx");
        }


        public static bool IsNumeric(object Expression) { bool isNum; double retNum; isNum = Double.TryParse(Convert.ToString(Expression), System.Globalization.NumberStyles.Any, System.Globalization.NumberFormatInfo.InvariantInfo, out retNum); return isNum; }


        protected void txtLeasingnehmer_TextChanged(object sender, EventArgs e)
        {
            if (txtLeasingnehmer.Text.Length == 0 || txtLeasingnehmer.Text == "Geben Sie mindestens 3 Zeichen ein um eine Auswahlliste zu erhalten.")
                return;

            if (Session["LnAdressen"] == null)
                return;

            string[] Adressliste = (string[])Session["LnAdressen"];

            int i = Array.IndexOf(Adressliste, txtLeasingnehmer.Text);

            if (i == -1)
            {
                lblError.Text = "Ungültige Auswahl des Leasingnehmers.";

                txtLeasingnehmer.Text = "";

            }

        }

        [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethod()] 
        public string[] GetAdressList(string prefixText, int count)
        {
            string[] NewArr = new string[1];
            string[] BaseArr = null;
            int i = 0;

            BaseArr = (string[])Session["LnAdressen"];

            try
            {

                if (prefixText.Length > 2)
                {
                    for (i = 0; i <= BaseArr.Length - 1; i++)
                    {

                        if (BaseArr[i].ToUpper().Contains(prefixText.ToUpper()))
                        {
                            if (NewArr[0] == null)
                            {
                                NewArr[0] = BaseArr[i];

                            }
                            else
                            {
                                Array.Resize(ref NewArr, NewArr.Length + 1);

                                NewArr[NewArr.Length - 1] = BaseArr[i];

                            }

                        }
                    }
                }

                if (NewArr[0] == null)
                {
                    NewArr = null;
                }

            }
            catch (Exception ex)
            {
                NewArr = null;
                return NewArr;
            }


            return NewArr;

        }

        protected void lbSave_Click(object sender, EventArgs e)
        {

            bool booError = false;

            lblError.Text = "";

            LP_01 Vertrag = new LP_01(ref m_User, m_App, "");



            DataTable TempTable = (DataTable)Session["ArvalVertragsdaten"];


            Vertrag.Equinr = TempTable.Rows[0]["EQUNR"].ToString();



            if (txtNewVertragsnummer.Text.Length > 0)
            {
                if (IsNumeric(txtNewVertragsnummer.Text) == false)
                {
                    lblError.Text = @"Bitte geben Sie eine numerische Vertragsnummer ein.";
                    booError = true;
                }
                else
                {
                    Vertrag.Vertragsnummer = txtNewVertragsnummer.Text;
                }

            }
            else
            {
                Vertrag.Vertragsnummer = TempTable.Rows[0]["LIZNR"].ToString();

            }


            if (txtLeasingnehmer.Text.Length > 0)
            {
                Vertrag.KunnrZL = txtLeasingnehmer.Text.Substring(txtLeasingnehmer.Text.IndexOf("~") + 2);
            }
            else
            {
                Vertrag.KunnrZL = TempTable.Rows[0]["KUNNR_ZL"].ToString();
            }


            if (booError == false)
            {
                Vertrag.Change(Session["AppID"].ToString(), Session.SessionID.ToString(), this.Page);

                if (Vertrag.Status == 0)
                {

                    lblInfo.Visible = true;
                    lblInfo.Text = @"Die Daten wurden gespeichert.";
                    ClearVertragsdaten();
                    ResetPage();
                }
                else
                {
                    lblError.Text = @"Fehler: " + Vertrag.Message;
                }



            }

        }




    }
}