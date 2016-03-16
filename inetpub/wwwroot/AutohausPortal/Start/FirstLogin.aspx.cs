using System;
using System.Drawing;
using System.Web.UI;
using CKG.Base.Kernel.Security;

namespace AutohausPortal.Start
{
    public partial class FirstLogin : Page
    {
        private User m_User;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["objUser"] != null)
            {
                m_User = Session["objUser"] as User;

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
                m_User.Login(m_User.UserName, txtNewPwd.Text, Session.SessionID);
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
            }
        }

        protected void lnkShowPassword_Click(object sender, EventArgs e)
        {

        }
    }
}