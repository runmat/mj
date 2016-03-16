using System;
using System.Drawing;
using System.Web.UI;
using AutohausPortal.lib;
using CKG.Base.Kernel.Security;

namespace AutohausPortal.Start
{
    public partial class ChangePassword : Page
    {
        private User m_User;

        protected void Page_Load(object sender, EventArgs e)
        {
            //m_User = (CKG.Base.Kernel.Security.User)Session["objUser"]; 
            m_User = MVC.GetSessionUserObject();

            Title = "Passwort ändern";
            if (!IsPostBack)
            {
                StandardLogin.Visible = false;
                StandardLogin2.Visible = false;
                RequestQuestion.Visible = false;
                RequestPassword.Visible = false;

                lblHead.Text = "Passwort ändern";

                if (Request.QueryString["pwdreq"] != null && Request.QueryString["pwdreq"].ToString()=="true")
                {
                    RequestPassword.Visible = true;   
                    lblHead.Text = "Passwort anfordern";

                    //Master.FindControl("tdChangePasword").Visible = false;
                    //Master.FindControl("tdHandbuch").Visible = false;
                    //Master.FindControl("tdHauptmenue").Visible =false;

                    lblFrage.Text = m_User.GetQuestionText();
                    txtAntwortAnforderung.Focus();          
                    
                }
                else if (Request.QueryString["qstreq"] != null && Request.QueryString["qstreq"].ToString() == "true")
                {

                    RequestQuestion.Visible = true;
                    ddlFrage.DataSource = m_User.GetQuestions();
                    ddlFrage.DataTextField = "QuestionText";
                    ddlFrage.DataValueField = "QuestionID";
                    ddlFrage.DataBind();

                    ddlFrage.Items.FindByValue(m_User.QuestionID.ToString()).Selected = true;
                    txtAnfordernSpeichern.Focus();
                }
                else 
                {
                    StandardLogin.Visible = true;
                    StandardLogin2.Visible = true;
                    if (Request.QueryString["pwdreq"] != null && Request.QueryString["pwdreq"].ToString() == "true")
                    {
                        lblPwdExp.Visible = true;
                    }
                    else
                    {
                        lblPwdExp.Visible = false;
                        if (m_User.Email.Length > 0 && m_User.Customer.ForcePasswordQuestion && m_User.QuestionID > -1)
                        {
                            cmdShowQuestion.Visible = true;
                        }
                        else
                        {
                            cmdShowQuestion.Visible = false;
                        }
                    }
                }

                txtOldPwd.Focus();

                lblLength.Text = "1.) Das Kennwort muss " + m_User.Customer.CustomerPasswordRules.Length + " lang sein.";
                lblSpecial.Text = "2.) Das Kennwort muss " + m_User.Customer.CustomerPasswordRules.SpecialCharacter + " Sonderzeichen enthalten(Sonderzeichen: !§$%&/()=?#*<>@).";
                lblUpperCase.Text = "3.) Das Kennwort muss " + m_User.Customer.CustomerPasswordRules.CapitalLetters + " Großbuchstaben enthalten.";
                lblNumeric.Text = "4.) Das Kennwort muss " + m_User.Customer.CustomerPasswordRules.Numeric + " Zahlen enthalten.";
                
                txtNewPwd.Attributes.Add("onkeyup", "checkPassword(" + m_User.Customer.CustomerPasswordRules.Length +
                ", 1," + m_User.Customer.CustomerPasswordRules.CapitalLetters + "," + m_User.Customer.CustomerPasswordRules.Numeric +
                "," + m_User.Customer.CustomerPasswordRules.SpecialCharacter + ")");
            }
            
        }

        protected void btnChange_Click(object sender, EventArgs e)
        {
            try
            {
                if (m_User.ChangePasswordFirstLogin(txtNewPwd.Text, txtNewPwdConfirm.Text, m_User.UserName))
                {
                    lblPwdExp.Visible = false;
                    txtNewPwd.Enabled = false;
                    txtNewPwd.BackColor = Color.LightGray;
                    txtNewPwdConfirm.Enabled = false;
                    txtNewPwdConfirm.BackColor = Color.LightGray;
                    txtOldPwd.Enabled = false;
                    txtOldPwd.BackColor = Color.LightGray;
                    //tdValidation1.Visible = false;
                    //tdValidation2.Visible = false;
                    cmdSave.Enabled = false;
                    lblMessage.Text = "Ihr Kennwort wurde erfolgreich geändert";
                }
                else 
                {
                    throw new Exception(m_User.ErrorMessage);
                }

            }
            catch (Exception ex)
            {
                lblError.Text = ex.Message;
            }
        }

        protected void lnkShowQuestion_Click(object sender, EventArgs e)
        {
            StandardLogin.Visible = false;
            RequestPassword.Visible = false;

            RequestQuestion.Visible = true;
            ddlFrage.DataSource = m_User.GetQuestions();
            ddlFrage.DataTextField = "QuestionText";
            ddlFrage.DataValueField = "QuestionID";
            ddlFrage.DataBind();

            ddlFrage.Items.FindByValue(m_User.QuestionID.ToString()).Selected = true;

            SetFocus(txtAnfordernSpeichern);


            cmdShowPassword.Visible = true;
        }

        protected void lnkRequest_Click(object sender, EventArgs e)
        {
            int intTemp = m_User.RequestNewPassword(txtAntwortAnforderung.Text);
            cmdRequest.Enabled = false;
            switch (intTemp)
            {
                case -9999:
                    this.lblError.Text = "Beim Anfordern des Passwortes ist ein Fehler aufgetreten. (" + m_User.ErrorMessage + ")";
                    break;
                case 0:
                    this.lblError.Text = "Ein vorläufiges Passwort wurde erzeugt und versendet.";
                    txtAntwortAnforderung.Text = "";
                    cmdLogout.Visible = true;
                    break;
                case 1:
                    this.lblError.Text = "Die Anwort stimmt nicht mit der gespeicherten überein. (Noch ein Versuch möglich.)";
                    cmdRequest.Enabled = true;
                    break;
                default:
                    if (intTemp < 0)
                    {
                        this.lblError.Text = "Beim Anfordern des Passwortes ist ein Fehler aufgetreten. (" + m_User.ErrorMessage + ")";
                    }
                    else
                    {
                        this.lblError.Text = "Die Anwort stimmt nicht mit der gespeicherten überein. (Noch " + intTemp.ToString() + " Versuche möglich.)";
                        cmdRequest.Enabled = true;
                    }
                    break;
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
            RequestPassword.Visible = false;
            RequestQuestion.Visible = false;

            StandardLogin.Visible = true;
            StandardLogin2.Visible = true;
            if (((Request.QueryString["pwdexp"] != null)) && (Request.QueryString["pwdexp"] == "true"))
            {
                lblPwdExp.Visible = true;
            }
            else
            {
                lblPwdExp.Visible = false;
            }
            SetFocus(txtOldPwd);
        }

        protected void cmdCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("/AutohausPortal/(S(" + Session.SessionID + "))/Start/Selection.aspx");
        }

        protected void cmdLogout_Click(object sender, EventArgs e)
        {
            Response.Redirect("/AutohausPortal/(S(" + Session.SessionID + "))/Start/Logout.aspx");
        }
    }
}