Imports CKG.Base.Kernel
Imports CKG.Base.Kernel.Common.Common
Imports CKG.Services.Design
Partial Public Class ChangePassword
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
            RequestPassword.Visible = False
            RequestQuestion.Visible = False

            If (Not Request.QueryString("pwdreq") Is Nothing) _
                AndAlso (Request.QueryString("pwdreq") = "true") Then

                RequestPassword.Visible = True
                lblHead.Text = "Passwort anfordern"

                Me.Master.FindControl("tdChangePasword").Visible = False
                Me.Master.FindControl("tdHandbuch").Visible = False
                Me.Master.FindControl("tdHauptmenue").Visible = False

                lblFrage.Text = m_User.GetQuestionText
                txtAntwortAnforderung.Focus()

            ElseIf (Not Request.QueryString("qstreq") Is Nothing) _
                AndAlso (Request.QueryString("qstreq") = "true") Then

                RequestQuestion.Visible = True
                ddlFrage.DataSource = m_User.GetQuestions
                ddlFrage.DataTextField = "QuestionText"
                ddlFrage.DataValueField = "QuestionID"
                ddlFrage.DataBind()

                ddlFrage.Items.FindByValue(m_User.QuestionID.ToString).Selected = True
                txtAnfordernSpeichern.Focus()
            Else
                StandardLogin.Visible = True
                If (Not Request.QueryString("pwdexp") Is Nothing) _
                   AndAlso (Request.QueryString("pwdexp") = "true") Then
                    Me.trPwdExp.Visible = True
                Else
                    Me.trPwdExp.Visible = False
                    If m_User.Email.Length > 0 And m_User.Customer.ForcePasswordQuestion And m_User.QuestionID > -1 Then
                        lnkShowQuestion.Visible = True
                    Else
                        lnkShowQuestion.Visible = False
                    End If
                End If
                lblHead.Text = "Passwort ändern"
                txtOldPwd.Focus()
                lblLength.Text = "1.) Das Passwort muss " & m_User.Customer.CustomerPasswordRules.Length & " Zeichen lang sein."
                lblSpecial.Text = "2.) Das Passwort muss " & m_User.Customer.CustomerPasswordRules.SpecialCharacter & " Sonderzeichen enthalten(Sonderzeichen: !§$%&/()=?#*<>@)."
                lblUpperCase.Text = "3.) Das Passwort muss " & m_User.Customer.CustomerPasswordRules.CapitalLetters & " Großbuchstaben enthalten."
                lblNumeric.Text = "4.) Das Passwort muss " & m_User.Customer.CustomerPasswordRules.Numeric & " Zahlen)  enthalten."

                txtNewPwd.Attributes.Add("onkeyup", "checkPassword(" & m_User.Customer.CustomerPasswordRules.Length & _
                ", 1," & m_User.Customer.CustomerPasswordRules.CapitalLetters & "," & m_User.Customer.CustomerPasswordRules.Numeric & _
                "," & m_User.Customer.CustomerPasswordRules.SpecialCharacter & ")")
            End If
        End If
    End Sub
    Private Sub Log(ByVal strIdentification As String, ByVal strDescription As String, Optional ByVal strCategory As String = "APP")
        Dim logApp As New Logging.Trace(m_User.App.Connectionstring, m_User.App.SaveLogAccessSAP, m_User.App.LogLevel)
        Dim strUserName As String = m_User.UserName ' strUserName
        Dim strSessionID As String = Session.SessionID ' strSessionID
        Dim intSource As Integer = CInt(Request.QueryString("AppID")) ' intSource 
        Dim strTask As String = "Admin - Kennwortänderung" ' strTask
        Dim strCustomerName As String = m_User.CustomerName ' strCustomername
        Dim blnIsTestUser As Boolean = m_User.IsTestUser ' blnIsTestUser
        Dim intSeverity As Integer = 0 ' intSeverity 
        Dim tblParameters As DataTable = GetLogParameters() ' tblParameters

        logApp.WriteEntry(strCategory, strUserName, strSessionID, intSource, strTask, strIdentification, strDescription, strCustomerName, m_User.Customer.CustomerId, blnIsTestUser, intSeverity, tblParameters)
    End Sub
    Private Function GetLogParameters() As DataTable
        Try
            Dim tblPar As New DataTable()
            With tblPar
                .Columns.Add("altes Kennwort", System.Type.GetType("System.String"))
                .Columns.Add("neues Kennwort", System.Type.GetType("System.String"))
                .Columns.Add("Kennwortbestätigung", System.Type.GetType("System.String"))
                .Rows.Add(.NewRow)
                Dim strPw As String = ""
                Dim intCount As Integer
                For intCount = 1 To txtNewPwd.Text.Length
                    strPw &= "*"
                Next
                .Rows(0)("neues Kennwort") = strPw
                Dim strPw2 As String = ""
                For intCount = 1 To txtNewPwdConfirm.Text.Length
                    strPw2 &= "*"
                Next
                .Rows(0)("Kennwortbestätigung") = strPw2
                Dim strPw3 As String = ""
                For intCount = 1 To txtOldPwd.Text.Length
                    strPw3 &= "*"
                Next
                .Rows(0)("altes Kennwort") = strPw3
            End With
            Return tblPar
        Catch ex As Exception
            Dim dt As New DataTable()
            dt.Columns.Add("Fehler beim erstellen der Log-Parameter", System.Type.GetType("System.String"))
            dt.Rows.Add(dt.NewRow)
            Dim str As String = ex.Message
            If Not ex.InnerException Is Nothing Then
                str &= ": " & ex.InnerException.Message
            End If
            dt.Rows(0)("Fehler beim erstellen der Log-Parameter") = str
            Return dt
        End Try
    End Function


    Private Sub btnChange_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnChange.Click
        Try
            If m_User.ChangePassword(Me.txtOldPwd.Text, Me.txtNewPwd.Text, Me.txtNewPwdConfirm.Text, m_User.UserName) Then
                With Me
                    .trPwdExp.Visible = False
                    .txtNewPwd.Enabled = False
                    .txtNewPwd.BackColor = System.Drawing.Color.LightGray
                    .txtNewPwdConfirm.Enabled = False
                    .txtNewPwdConfirm.BackColor = System.Drawing.Color.LightGray
                    .txtOldPwd.Enabled = False
                    .txtOldPwd.BackColor = System.Drawing.Color.LightGray
                    .btnChange.Enabled = False
                    .tdValidation1.Visible = False
                    .tdValidation2.Visible = False
                    .lblMessage.Text = "Passwort wurde erfolgreich geändert.<br />Bitte klicken Sie auf ""Startseite""."
                End With
                Log(m_User.UserID.ToString, "Eigenes Passwort ändern")

            Else
                Throw New System.Exception(m_User.ErrorMessage)
            End If
        Catch ex As Exception
            Me.lblError.Text = ex.Message
            Log(m_User.UserID.ToString, Me.lblError.Text, "ERR")
        End Try
    End Sub

    Private Sub lnkRequest_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lnkRequest.Click
        Dim intTemp As Integer = m_User.RequestNewPassword(txtAntwortAnforderung.Text)
        lnkRequest.Enabled = False
        Select Case intTemp
            Case -9999
                Me.lblError.Text = "Beim Anfordern des Passwortes ist ein Fehler aufgetreten. (" & m_User.ErrorMessage & ")"
            Case 0
                Me.lblError.Text = "Ein vorläufiges Passwort wurde erzeugt und versendet."
                txtAntwortAnforderung.Text = ""
                lnkLogout.Visible = True
            Case 1
                Me.lblError.Text = "Die Anwort stimmt nicht mit der gespeicherten überein. (Noch ein Versuch möglich.)"
                lnkRequest.Enabled = True
            Case Else
                If intTemp < 0 Then
                    Me.lblError.Text = "Beim Anfordern des Passwortes ist ein Fehler aufgetreten. (" & m_User.ErrorMessage & ")"
                Else
                    Me.lblError.Text = "Die Anwort stimmt nicht mit der gespeicherten überein. (Noch " & intTemp.ToString & " Versuche möglich.)"
                    lnkRequest.Enabled = True
                End If
        End Select
    End Sub

    Private Sub cmdSetzeFrageAntwort_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSetzeFrageAntwort.Click
        Try
            If ddlFrage.SelectedItem.Value = "-1" Or txtAnfordernSpeichern.Text.Trim(" "c).Length = 0 Then
                Me.lblError.Text = "Bitte wählen und beantworten Sie die Frage."
            Else
                m_User.SaveQuestion(CInt(ddlFrage.SelectedItem.Value), txtAnfordernSpeichern.Text)
                Response.Redirect("Selection.aspx")
            End If
        Catch ex As Exception
            Me.lblError.Text = ex.Message
            Log(m_User.UserID.ToString, Me.lblError.Text, "ERR")
        End Try
    End Sub

    Private Sub lnkShowQuestion_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lnkShowQuestion.Click
        StandardLogin.Visible = False
        RequestPassword.Visible = False

        RequestQuestion.Visible = True
        ddlFrage.DataSource = m_User.GetQuestions
        ddlFrage.DataTextField = "QuestionText"
        ddlFrage.DataValueField = "QuestionID"
        ddlFrage.DataBind()

        ddlFrage.Items.FindByValue(m_User.QuestionID.ToString).Selected = True

        SetFocus(txtAnfordernSpeichern)


        lnkShowPassword.Visible = True
    End Sub

    Private Sub lnkShowPassword_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lnkShowPassword.Click
        RequestPassword.Visible = False
        RequestQuestion.Visible = False

        StandardLogin.Visible = True
        If (Not Request.QueryString("pwdexp") Is Nothing) _
           AndAlso (Request.QueryString("pwdexp") = "true") Then
            Me.trPwdExp.Visible = True
        Else
            Me.trPwdExp.Visible = False
        End If
        SetFocus(txtOldPwd)
    End Sub
End Class