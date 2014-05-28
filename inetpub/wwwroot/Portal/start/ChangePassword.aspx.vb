Imports CKG.Base.Kernel
Imports CKG.Base.Kernel.Common.Common

Namespace Start
    Public Class ChangePassword
        Inherits System.Web.UI.Page

#Region " Vom Web Form Designer generierter Code "

        Protected WithEvents lblError As System.Web.UI.WebControls.Label
        Protected WithEvents txtNewPwd As System.Web.UI.WebControls.TextBox
        Protected WithEvents txtNewPwdConfirm As System.Web.UI.WebControls.TextBox
        Protected WithEvents txtOldPwd As System.Web.UI.WebControls.TextBox
        Protected WithEvents trPwdExp As System.Web.UI.HtmlControls.HtmlTableRow
        Protected WithEvents lblMessage As System.Web.UI.WebControls.Label
        Protected WithEvents lnkLogout As System.Web.UI.WebControls.HyperLink
        Protected WithEvents Form1 As System.Web.UI.HtmlControls.HtmlForm
        Protected WithEvents btnChange As System.Web.UI.WebControls.LinkButton
        Protected WithEvents tblChange As System.Web.UI.HtmlControls.HtmlTable
        Protected WithEvents Tr1 As System.Web.UI.HtmlControls.HtmlTableRow
        Protected WithEvents lnkRequest As System.Web.UI.WebControls.LinkButton
        Protected WithEvents lblFrage As System.Web.UI.WebControls.Label
        Protected WithEvents tblRequestPassword As System.Web.UI.HtmlControls.HtmlTable
        Protected WithEvents tblRequestQuestion As System.Web.UI.HtmlControls.HtmlTable
        Protected WithEvents Tr2 As System.Web.UI.HtmlControls.HtmlTableRow
        Protected WithEvents cmdSetzeFrageAntwort As System.Web.UI.WebControls.LinkButton
        Protected WithEvents ddlFrage As System.Web.UI.WebControls.DropDownList
        Protected WithEvents txtAntwortAnforderung As System.Web.UI.WebControls.TextBox
        Protected WithEvents txtAnfordernSpeichern As System.Web.UI.WebControls.TextBox
        Protected WithEvents Literal1 As System.Web.UI.WebControls.Literal
        Protected WithEvents lnkShowQuestion As System.Web.UI.WebControls.LinkButton
        Protected WithEvents lnkShowPassword As System.Web.UI.WebControls.LinkButton
        Protected WithEvents lblLength As System.Web.UI.WebControls.Label
        Protected WithEvents lblSpecial As System.Web.UI.WebControls.Label
        Protected WithEvents lblUpperCase As System.Web.UI.WebControls.Label
        Protected WithEvents lblNumeric As System.Web.UI.WebControls.Label
        Protected WithEvents trValidation As System.Web.UI.HtmlControls.HtmlTableRow

        Protected WithEvents ucHeader As PageElements.Header
        'Dieser Aufruf ist für den Web Form-Designer erforderlich.
        <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

        End Sub

        Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
            'CODEGEN: Diese Methode ist für den Web Form-Designer erforderlich
            'Verwenden Sie nicht den Code-Editor zur Bearbeitung.
            InitializeComponent()
        End Sub

#End Region
        Private m_User As Security.User
        Protected WithEvents ucStyles As PageElements.Styles

        Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load, Me.Load
            Literal1.Text = ""
            If Not Session("objUser") Is Nothing Then
                m_User = CType(Session("objUser"), Security.User)
            End If
            'InitHeader.InitUser(m_User)

            ucHeader.InitUser(m_User)
            ucStyles.TitleText = "Verwaltung von Zugangsdaten"

            If Not IsPostBack Then
                tblChange.Visible = False
                tblRequestPassword.Visible = False
                tblRequestQuestion.Visible = False

                If (Not Request.QueryString("pwdreq") Is Nothing) _
                    AndAlso (Request.QueryString("pwdreq") = "true") Then

                    tblRequestPassword.Visible = True
                    ucStyles.TitleText = "Passwort anfordern"

                    ucHeader.HideLinks()

                    'Frage füllen
                    lblFrage.Text = m_User.GetQuestionText

                    Literal1.Text = "<script language=""JavaScript"">" & vbCrLf
                    Literal1.Text &= "	<!-- //" & vbCrLf
                    Literal1.Text &= "	window.document.Form1.txtAntwortAnforderung.focus();" & vbCrLf
                    Literal1.Text &= "	//-->" & vbCrLf
                    Literal1.Text &= "</script>" & vbCrLf
                ElseIf (Not Request.QueryString("qstreq") Is Nothing) _
                    AndAlso (Request.QueryString("qstreq") = "true") Then

                    tblRequestQuestion.Visible = True
                    ddlFrage.DataSource = m_User.GetQuestions
                    ddlFrage.DataTextField = "QuestionText"
                    ddlFrage.DataValueField = "QuestionID"
                    ddlFrage.DataBind()

                    ddlFrage.Items.FindByValue(m_User.QuestionID.ToString).Selected = True

                    Literal1.Text = "<script language=""JavaScript"">" & vbCrLf
                    Literal1.Text &= "	<!-- //" & vbCrLf
                    Literal1.Text &= "	window.document.Form1.txtAnfordernSpeichern.focus();" & vbCrLf
                    Literal1.Text &= "	//-->" & vbCrLf
                    Literal1.Text &= "</script>" & vbCrLf
                Else
                    tblChange.Visible = True
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
                    lblLength.Text = "1.) Das Passwort muss " & m_User.Customer.CustomerPasswordRules.Length & " Zeichen lang sein."
                    lblSpecial.Text = "2.) Das Passwort muss " & m_User.Customer.CustomerPasswordRules.SpecialCharacter & " Sonderzeichen enthalten(Sonderzeichen: !§$%&/()=?#*<>@)."
                    lblUpperCase.Text = "3.) Das Passwort muss " & m_User.Customer.CustomerPasswordRules.CapitalLetters & " Großbuchstaben enthalten."
                    lblNumeric.Text = "4.) Das Passwort muss " & m_User.Customer.CustomerPasswordRules.Numeric & " Zahl(en) enthalten."

                    txtNewPwd.Attributes.Add("onkeyup", "checkPassword(" & m_User.Customer.CustomerPasswordRules.Length & _
                    ", 1," & m_User.Customer.CustomerPasswordRules.CapitalLetters & "," & m_User.Customer.CustomerPasswordRules.Numeric & _
                    "," & m_User.Customer.CustomerPasswordRules.SpecialCharacter & ")")

                    ucStyles.TitleText = "Änderung Passwort"
                    Literal1.Text = "<script language=""JavaScript"">" & vbCrLf
                    Literal1.Text &= "	<!-- //" & vbCrLf
                    Literal1.Text &= "	window.document.Form1.txtOldPwd.focus();" & vbCrLf
                    Literal1.Text &= "	//-->" & vbCrLf
                    Literal1.Text &= "</script>" & vbCrLf
                End If
            End If


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
                        .trValidation.Visible = False
                        '.lnkLogout.Visible = True
                        .lblMessage.Text = "Passwort wurde erfolgreich geändert.<br />Über das Hauptmenü gelangen Sie zu Ihren Anwendungen."
                    End With
                    Log(m_User.UserID.ToString, "Eigenes Kennwort ändern")
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
                    Response.Redirect("../Start/Selection.aspx")
                End If
            Catch ex As Exception
                Me.lblError.Text = ex.Message
                Log(m_User.UserID.ToString, Me.lblError.Text, "ERR")
            End Try
        End Sub

        Private Sub lnkShowQuestion_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lnkShowQuestion.Click
            tblChange.Visible = False
            tblRequestPassword.Visible = False

            tblRequestQuestion.Visible = True
            ddlFrage.DataSource = m_User.GetQuestions
            ddlFrage.DataTextField = "QuestionText"
            ddlFrage.DataValueField = "QuestionID"
            ddlFrage.DataBind()

            ddlFrage.Items.FindByValue(m_User.QuestionID.ToString).Selected = True

            Literal1.Text = "<script language=""JavaScript"">" & vbCrLf
            Literal1.Text &= "	<!-- //" & vbCrLf
            Literal1.Text &= "	window.document.Form1.txtAnfordernSpeichern.focus();" & vbCrLf
            Literal1.Text &= "	//-->" & vbCrLf
            Literal1.Text &= "</script>" & vbCrLf

            lnkShowPassword.Visible = True
        End Sub

        Private Sub lnkShowPassword_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lnkShowPassword.Click
            tblRequestPassword.Visible = False
            tblRequestQuestion.Visible = False

            tblChange.Visible = True
            If (Not Request.QueryString("pwdexp") Is Nothing) _
               AndAlso (Request.QueryString("pwdexp") = "true") Then
                Me.trPwdExp.Visible = True
            Else
                Me.trPwdExp.Visible = False
            End If
            ucStyles.TitleText = "Änderung Passwort"
            Literal1.Text = "<script language=""JavaScript"">" & vbCrLf
            Literal1.Text &= "	<!-- //" & vbCrLf
            Literal1.Text &= "	window.document.Form1.txtOldPwd.focus();" & vbCrLf
            Literal1.Text &= "	//-->" & vbCrLf
            Literal1.Text &= "</script>" & vbCrLf
        End Sub

    End Class
End Namespace

' ************************************************
' $History: ChangePassword.aspx.vb $
' 
' *****************  Version 9  *****************
' User: Rudolpho     Date: 9.09.10    Time: 11:57
' Updated in $/CKAG/portal/Start
' 
' *****************  Version 8  *****************
' User: Rudolpho     Date: 8.09.10    Time: 16:02
' Updated in $/CKAG/portal/Start
' 
' *****************  Version 7  *****************
' User: Rudolpho     Date: 8.09.10    Time: 10:12
' Updated in $/CKAG/portal/Start
' 
' *****************  Version 6  *****************
' User: Rudolpho     Date: 7.09.10    Time: 10:57
' Updated in $/CKAG/portal/Start
' 
' *****************  Version 5  *****************
' User: Rudolpho     Date: 28.06.10   Time: 9:50
' Updated in $/CKAG/portal/Start
' 
' *****************  Version 4  *****************
' User: Rudolpho     Date: 23.06.10   Time: 15:16
' Updated in $/CKAG/portal/Start
' ITA:  3794
' 
' *****************  Version 3  *****************
' User: Rudolpho     Date: 7.10.09    Time: 14:52
' Updated in $/CKAG/portal/Start
' 
' *****************  Version 2  *****************
' User: Rudolpho     Date: 10.04.08   Time: 17:37
' Updated in $/CKAG/portal/Start
' Migration
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 3.04.08    Time: 17:21
' Created in $/CKAG/portal/start
' 
' *****************  Version 14  *****************
' User: Uha          Date: 4.09.07    Time: 9:59
' Updated in $/CKG/Portal/Start
' ITA 1280: Bugfixing
' 
' *****************  Version 13  *****************
' User: Uha          Date: 30.08.07   Time: 15:17
' Updated in $/CKG/Portal/Start
' ITA 1280: Bugfix
' 
' *****************  Version 12  *****************
' User: Uha          Date: 30.08.07   Time: 12:36
' Updated in $/CKG/Portal/Start
' ITA 1280: Paßwortversand im Web auf Benutzerwunsch
' 
' *****************  Version 11  *****************
' User: Uha          Date: 22.05.07   Time: 9:31
' Updated in $/CKG/Portal/Start
' Nacharbeiten + Bereinigungen
' 
' *****************  Version 10  *****************
' User: Uha          Date: 1.03.07    Time: 16:38
' Updated in $/CKG/Portal/Start
' 
' ************************************************