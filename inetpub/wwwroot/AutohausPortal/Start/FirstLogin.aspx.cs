using System;
using System.Data;
using System.Drawing;
using System.Web.UI;
using CKG.Base.Kernel.Logging;
using CKG.Base.Kernel.Security;

namespace AutohausPortal.Start
{
    public partial class FirstLogin : Page
    {
        private User m_User ;

        protected void Page_Load(object sender, EventArgs e)
        {

            if (Session["objUser"]!=null) 
            {
                Title = "Passwort ändern";

                if (!IsPostBack) 
                { 
                    StandardLogin.Visible = false;
                    RequestQuestion.Visible = false;
                    StandardLogin.Visible = true;
                    lblHead.Text = Title;
                    txtNewPwd.Focus();    
    
                    lblLength.Text = "1.) Das Kennwort muss " + m_User.Customer.CustomerPasswordRules.Length + " lang sein.";
                    lblSpecial.Text = "2.) Das Kennwort muss " + m_User.Customer.CustomerPasswordRules.SpecialCharacter + " Sonderzeichen enthalten(Sonderzeichen: !§$%&/()=?#*<>@).";
                    lblUpperCase.Text = "3.) Das Kennwort muss " + m_User.Customer.CustomerPasswordRules.CapitalLetters + " Großbuchstaben enthalten.";
                    lblNumeric.Text = "4.) Das Kennwort muss " + m_User.Customer.CustomerPasswordRules.Numeric + " Zahlen enthalten.";

                    txtNewPwd.Attributes.Add("onkeyup", "checkPassword(" + m_User.Customer.CustomerPasswordRules.Length +
                    ", 1," + m_User.Customer.CustomerPasswordRules.CapitalLetters + "," + m_User.Customer.CustomerPasswordRules.Numeric +
                    "," + m_User.Customer.CustomerPasswordRules.SpecialCharacter + ")");              
                }
            }

        }

        protected void btnChange_Click(object sender, EventArgs e)
        {
            try
            {
                if (m_User.ChangePasswordFirstLogin(txtNewPwd.Text, txtNewPwdConfirm.Text, m_User.UserName)) 
                {
                    trPwdExp.Visible = false;
                    txtNewPwd.Enabled = false;
                    txtNewPwd.BackColor = Color.LightGray;
                    txtNewPwdConfirm.Enabled = false;
                    txtNewPwdConfirm.BackColor = Color.LightGray;
                    btnChange.Enabled = false;
                    lblMessage.Text = "" ;                                       
                }
                m_User.Login(m_User.UserName, txtNewPwd.Text, Session.SessionID.ToString());
                Log(m_User.UserID.ToString(), "Eigenes Kennwort ändern", "APP");
                if (m_User.Customer.ForcePasswordQuestion == true)
                {
                    StandardLogin.Visible = false;
                    RequestQuestion.Visible = true;
                    ddlFrage.DataSource = m_User.GetQuestions();
                    ddlFrage.DataTextField = "QuestionText";
                    ddlFrage.DataValueField = "QuestionID";
                    ddlFrage.DataBind();

                    ddlFrage.Items.FindByValue(m_User.QuestionID.ToString()).Selected = true;
                    SetFocus(txtAnfordernSpeichern);
                }
                else
                {
                    Response.Redirect("../Start/Selection.aspx");
                }


            }
            catch (Exception ex)
            {
                lblError.Text = ex.Message;
                Log(m_User.UserID.ToString(), lblError.Text, "ERR");
            }
        }

        private void Log(string strIdentification, string strDescription, string strCategory)
        {
            Trace logApp = new Trace(m_User.App.Connectionstring, m_User.App.SaveLogAccessSAP, m_User.App.LogLevel);
            string strUserName = m_User.UserName;
            // strUserName
            string strSessionID = Session.SessionID;
            // strSessionID
            int intSource = Convert.ToInt32(Request.QueryString["AppID"]);
            // intSource 
            string strTask = "Admin - Kennwortänderung";
            // strTask
            string strCustomerName = m_User.CustomerName;
            // strCustomername
            bool blnIsTestUser = m_User.IsTestUser;
            // blnIsTestUser
            int intSeverity = 0;
            // intSeverity 
            DataTable tblParameters = GetLogParameters();
            // tblParameters

            logApp.WriteEntry(strCategory, strUserName, strSessionID, intSource, strTask, strIdentification, strDescription, strCustomerName, m_User.Customer.CustomerId, blnIsTestUser,
            intSeverity, tblParameters);
        }
        private DataTable GetLogParameters()
        {
            try
            {
                DataTable tblPar = new DataTable();
                tblPar.Columns.Add("neues Kennwort", typeof(String));
                tblPar.Columns.Add("Kennwortbestätigung", typeof(String));
                tblPar.Rows.Add(tblPar.NewRow());
                string strPw = "";
                int intCount = 0;
                for (intCount = 1; intCount <= txtNewPwd.Text.Length; intCount++)
                {
                    strPw += "*";
                }
                tblPar.Rows[0]["neues Kennwort"] = strPw;
                string strPw2 = "";
                for (intCount = 1; intCount <= txtNewPwdConfirm.Text.Length; intCount++)
                {
                    strPw2 += "*";
                }
                return tblPar;
            }
            catch (Exception ex)
            {
                DataTable dt = new DataTable();
                dt.Columns.Add("Fehler beim erstellen der Log-Parameter", Type.GetType("System.String"));
                dt.Rows.Add(dt.NewRow());
                string str = ex.Message;
                if ((ex.InnerException != null))
                {
                    str += ": " + ex.InnerException.Message;
                }
                dt.Rows[0]["Fehler beim erstellen der Log-Parameter"] = str;
                return dt;
            }
        }

        protected void cmdSetzeFrageAntwort_Click(object sender, EventArgs e)
        {
            try
            {
                if (ddlFrage.SelectedItem.Value == "-1" | txtAnfordernSpeichern.Text.Trim(' ').Length == 0)
                {
                    this.lblError.Text = "Bitte wählen und beantworten Sie die Frage.";
                }
                else
                {
                    m_User.SaveQuestion(Convert.ToInt32(ddlFrage.SelectedItem.Value), txtAnfordernSpeichern.Text);
                    Response.Redirect("../Start/Selection.aspx");
                }
            }
            catch (Exception ex)
            {
                this.lblError.Text = ex.Message;
                Log(m_User.UserID.ToString(), this.lblError.Text, "ERR");
            }
        }

        protected void lnkShowPassword_Click(object sender, EventArgs e)
        {

        }

    }
}