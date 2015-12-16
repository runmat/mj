Imports CKG.Base.Kernel
Imports CKG.Base.Kernel.Common.Common

Partial Public Class FirstLogin
    Inherits System.Web.UI.Page

    Private m_User As Security.User

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Session("objUser") Is Nothing Then
            m_User = CType(Session("objUser"), Security.User)
        End If
        'InitHeader.InitUser(m_User)

        ucHeader.InitUser(m_User)
        ucHeader.LockLinks()

        ucStyles.TitleText = "Verwaltung von Zugangsdaten"
        If Not IsPostBack Then
            tblChange.Visible = True

            lblLength.Text = "1.) Das Passwort muss " & m_User.Customer.CustomerPasswordRules.Length & " Zeichen lang sein."
            lblSpecial.Text = "2.) Das Passwort muss " & m_User.Customer.CustomerPasswordRules.SpecialCharacter & " Sonderzeichen enthalten(Sonderzeichen: !§$%&/()=?#*<>@)."
            lblUpperCase.Text = "3.) Das Passwort muss " & m_User.Customer.CustomerPasswordRules.CapitalLetters & " Großbuchstaben enthalten."
            lblNumeric.Text = "4.) Das Passwort muss " & m_User.Customer.CustomerPasswordRules.Numeric & " Zahl(en) enthalten."

            txtNewPwd.Attributes.Add("onkeyup", "checkPassword(" & m_User.Customer.CustomerPasswordRules.Length & _
            ", 1," & m_User.Customer.CustomerPasswordRules.CapitalLetters & "," & m_User.Customer.CustomerPasswordRules.Numeric & _
            "," & m_User.Customer.CustomerPasswordRules.SpecialCharacter & ")")

            ucStyles.TitleText = "Änderung Passwort"
            SetFocus(txtNewPwd)

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
                m_User.Login(m_User.UserName, txtNewPwd.Text, Session.SessionID.ToString, Request.Url.AbsoluteUri)
                Log(m_User.UserID.ToString, "Eigenes Kennwort ändern")

                If m_User.Customer.ForcePasswordQuestion = True Then
                    tblChange.Visible = False
                    tblRequestQuestion.Visible = True
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
            Log(m_User.UserID.ToString, Me.lblError.Text, "ERR")
            CType(ucHeader.FindControl("lnkLogout"), HyperLink).Visible = True
        End Try
    End Sub
    Private Sub Log(ByVal strIdentification As String, ByVal strDescription As String, Optional ByVal strCategory As String = "APP")
        Dim logApp As New Logging.Trace(m_User.App.Connectionstring, m_User.App.SaveLogAccessSAP, m_User.App.LogLevel)
        Dim strUserName As String = m_User.UserName ' strUserName
        Dim strSessionID As String = Session.SessionID ' strSessionID
        Dim intSource As Integer = CInt(Request.QueryString("AppID")) ' intSource 
        Dim strTask As String = "Admin - Passwortänderung" ' strTask
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
            CType(ucHeader.FindControl("lnkLogout"), HyperLink).Visible = True
            Return dt
        End Try
    End Function

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
            Log(m_User.UserID.ToString, Me.lblError.Text, "ERR")
            CType(ucHeader.FindControl("lnkLogout"), HyperLink).Visible = True
        End Try
    End Sub
End Class