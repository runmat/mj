Imports CKG.Base.Kernel

Partial Public Class FirstLogin
    Inherits System.Web.UI.Page

    Private m_User As Security.User

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Session("objUser") Is Nothing Then
            m_User = CType(Session("objUser"), Security.User)
        End If
        'InitHeader.InitUser(m_User)

        lblHead.Text = "Verwaltung von Zugangsdaten"
        Me.Title = "Passwort ändern"
        If Not IsPostBack Then
            StandardLogin.Visible = False
            RequestQuestion.Visible = False

            StandardLogin.Visible = True
            lblHead.Text = "Passwort ändern"
            txtNewPwd.Focus()
            lblLength.Text = "1.) Das Kennwort muss " & m_User.Customer.CustomerPasswordRules.Length & " lang sein."
            lblSpecial.Text = "2.) Das Kennwort muss " & m_User.Customer.CustomerPasswordRules.SpecialCharacter & " Sonderzeichen enthalten(Sonderzeichen: !§$%&/()=?#*<>@)."
            lblUpperCase.Text = "3.) Das Kennwort muss " & m_User.Customer.CustomerPasswordRules.CapitalLetters & " Großbuchstaben enthalten."
            lblNumeric.Text = "4.) Das Kennwort muss " & m_User.Customer.CustomerPasswordRules.Numeric & " Zahlen enthalten."

            txtNewPwd.Attributes.Add("onkeyup", "checkPassword(" & m_User.Customer.CustomerPasswordRules.Length & _
            ", 1," & m_User.Customer.CustomerPasswordRules.CapitalLetters & "," & m_User.Customer.CustomerPasswordRules.Numeric & _
            "," & m_User.Customer.CustomerPasswordRules.SpecialCharacter & ")")
        End If
    End Sub

    Protected Sub btnChange_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnChange.Click
        Try
            If m_User.ChangePasswordFirstLogin(Me.txtNewPwd.Text, Me.txtNewPwdConfirm.Text, m_User.UserName) Then
                With Me
                    .trPwdExp.Visible = False
                    .txtNewPwd.Enabled = False
                    .txtNewPwd.BackColor = System.Drawing.Color.LightGray
                    .txtNewPwdConfirm.Enabled = False
                    .txtNewPwdConfirm.BackColor = System.Drawing.Color.LightGray
                    .btnChange.Enabled = False
                    .lblMessage.Text = ""
                End With
                m_User.Login(m_User.UserName, txtNewPwd.Text, Session.SessionID.ToString)

                If m_User.Customer.ForcePasswordQuestion = True Then
                    StandardLogin.Visible = False
                    RequestQuestion.Visible = True
                    ddlFrage.DataSource = m_User.GetQuestions
                    ddlFrage.DataTextField = "QuestionText"
                    ddlFrage.DataValueField = "QuestionID"
                    ddlFrage.DataBind()

                    ddlFrage.Items.FindByValue(m_User.QuestionID.ToString).Selected = True
                    SetFocus(txtAnfordernSpeichern)
                Else
                    Response.Redirect("../Start/Selection.aspx")
                End If


            Else
                Throw New System.Exception(m_User.ErrorMessage)
            End If
        Catch ex As Exception
            Me.lblError.Text = ex.Message
        End Try
    End Sub

    Protected Sub cmdSetzeFrageAntwort_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdSetzeFrageAntwort.Click
        Try
            If ddlFrage.SelectedItem.Value = "-1" Or txtAnfordernSpeichern.Text.Trim(" "c).Length = 0 Then
                Me.lblError.Text = "Bitte wählen und beantworten Sie die Frage."
            Else
                m_User.SaveQuestion(CInt(ddlFrage.SelectedItem.Value), txtAnfordernSpeichern.Text)
                Response.Redirect("../Start/Selection.aspx")
            End If
        Catch ex As Exception
            Me.lblError.Text = ex.Message
        End Try
    End Sub

End Class