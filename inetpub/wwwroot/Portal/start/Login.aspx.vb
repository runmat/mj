Imports CKG.Portal.PageElements
Imports CKG.Base.Kernel.Common.Common
Imports CKG.Base
Imports System.Web.Security
Imports System.Drawing.Imaging
Imports CKG.Base.Business
Imports System.Security.Cryptography
Imports System.Text
Imports System.IO

Public Class Login
    Inherits System.Web.UI.Page

#Region " Vom Web Form Designer generierter Code "
    Protected WithEvents txtUsername As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtPassword As System.Web.UI.WebControls.TextBox
    Protected WithEvents cmdLogin As System.Web.UI.WebControls.Button
    Protected WithEvents lblError As System.Web.UI.WebControls.Label
    Protected WithEvents Form1 As System.Web.UI.HtmlControls.HtmlForm
    Protected WithEvents Repeater1 As System.Web.UI.WebControls.Repeater
    Protected WithEvents cbxLogin_TEST As System.Web.UI.WebControls.CheckBox
    Protected WithEvents cbxLogin_PROD As System.Web.UI.WebControls.CheckBox
    Protected WithEvents litAlert As System.Web.UI.WebControls.Literal
    Protected WithEvents Image2 As System.Web.UI.WebControls.Image
    Protected WithEvents StandardLogin As System.Web.UI.HtmlControls.HtmlTable
    Protected WithEvents Textbox2 As System.Web.UI.WebControls.TextBox
    Protected WithEvents Textbox1 As System.Web.UI.WebControls.TextBox
    Protected WithEvents HyperLink1 As System.Web.UI.WebControls.HyperLink
    Protected WithEvents Button1 As System.Web.UI.WebControls.Button
    Protected WithEvents DoubleLogin As System.Web.UI.HtmlControls.HtmlTable
    Protected WithEvents cmdContinue As System.Web.UI.WebControls.Button
    Protected WithEvents cmdBack As System.Web.UI.WebControls.Button
    Protected WithEvents lnkPasswortVergessen As System.Web.UI.WebControls.LinkButton
    Protected WithEvents lbtnHelpCenter As System.Web.UI.WebControls.LinkButton
    Protected WithEvents tblHelp As System.Web.UI.HtmlControls.HtmlTable
    Protected WithEvents CodeNumberTextBox As System.Web.UI.WebControls.TextBox
    Protected WithEvents MessageLabel As System.Web.UI.WebControls.Label
    Protected WithEvents trMail As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents trAnrede As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents trWebUserName As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents trName As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents trVorName As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents trTelefon As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents trFirma As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents trProblem As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents trPasswortVergessen As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents trHelpCenter As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents imgCatcha1 As System.Web.UI.HtmlControls.HtmlImage
    Protected WithEvents imgCatcha2 As System.Web.UI.HtmlControls.HtmlImage
    Protected WithEvents lnkImpressum As System.Web.UI.WebControls.HyperLink
    Protected WithEvents cmdSend As System.Web.UI.WebControls.Button
    Protected WithEvents ibtnRefresh As System.Web.UI.WebControls.ImageButton
    Protected WithEvents ddlAnrede As System.Web.UI.WebControls.DropDownList
    Protected WithEvents txtName As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtVorname As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtTelefon As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtEmail As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtFirma As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtProblem As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtWebUserName As System.Web.UI.WebControls.TextBox
    Protected WithEvents lblRedStar As System.Web.UI.WebControls.Label
    Protected WithEvents imgPasswort As System.Web.UI.HtmlControls.HtmlImage
    Protected WithEvents imgHelp As System.Web.UI.HtmlControls.HtmlImage
    Protected WithEvents ucHeader As Header
    'Dieser Aufruf ist für den Web Form-Designer erforderlich.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: Diese Methode ist für den Web Form-Designer erforderlich
        'Verwenden Sie nicht den Code-Editor zur Bearbeitung.
        InitializeComponent()
    End Sub

#End Region

    Private m_User As New Base.Kernel.Security.User()
    Private m_App As Base.Kernel.Security.App
    Private random As New Random

    Protected WithEvents ucStyles As Styles

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Me.Session("objUser") Is Nothing Then
            m_User = CType(Session("objUser"), Base.Kernel.Security.User)
            If Not (m_User.LoggedOn And m_User.DoubleLoginTry) Then
                If Me.User.Identity.IsAuthenticated = False Then
                    If trPasswortVergessen.Visible = False Then
                        If (Request.QueryString("reuser") Is Nothing OrElse Request.QueryString("reuser") Is String.Empty) Then
                            Response.Redirect(BouncePage(Me), True)
                        End If
                    End If
                End If
            End If
        End If
        ucStyles.TitleText = "Internet Reports - Anmeldung"
        litAlert.Text = ""
        If Not IsPostBack Then
            If Not CheckUniqueSessionID() Then
                Response.Redirect(BouncePage(Me), True)
            End If
            Session("CaptchaGen1") = GenerateRandomCode()
            Session("CaptchaGen2") = GenerateRandomCode()
            displayMessages()
            Me.StandardLogin.Visible = True
            Me.DoubleLogin.Visible = False
            'Prüfe zugreifende IP
            If (Me.ClientQueryString.ToString.Contains("IframeLogin.aspx")) Then
                System.Web.Security.FormsAuthentication.RedirectFromLoginPage("IFrameLogon", False)

            End If
            Dim intRestrictedCustomerId As Integer = CheckRestrictedIP()
            If intRestrictedCustomerId > -1 Then
                'Aha, Benutzer greift von IP zu, die Beschränkungen unterliegt

                'Prüfe, ob per Link doch das normale Logon angeboten werden soll
                If (Request.QueryString.Item("Logon") Is Nothing) OrElse (Not CStr(Request.QueryString.Item("Logon")).ToUpper = "OPEN") Then
                    Dim strIpError As String = "Die Anmeldung ist in der aktuellen Konfiguration nicht möglich.<br>Setzen Sie sich bitte mit Ihrer Kontaktperson beim DAD bzw. der Christoph Kroschke GmbH in Verbindung."

                    'Ermittele Standard-Benutzer
                    Dim strIpStandardUser As String = GiveIpStandardUser(intRestrictedCustomerId)
                    If strIpStandardUser.Length = 0 Then
                        'Kein Standard-Benutzer konfiguriert
                        '=> Biete Standard-Login an

                    Else
                        'Melde Standard-Benutzer an und geh' weiter
                        If m_User.Login(strIpStandardUser, Session.SessionID, Request.Url.AbsoluteUri) Then
                            System.Web.Security.FormsAuthentication.RedirectFromLoginPage(m_User.UserID.ToString, False)
                        Else
                            lblError.Text = strIpError & "<br>(" & m_User.ErrorMessage & ")"
                            cmdLogin.Enabled = False
                        End If
                    End If
                End If
            Else
                If (Not Request.QueryString("reuser") Is Nothing AndAlso Not Request.QueryString("reuser") Is String.Empty) Then

                    LoginFromURL()

                Else
                    If ClientQueryString.ToString.Contains("bounce.aspx") = False Then
                        Response.Redirect("/services", True)
                    End If

                End If
            End If

        Else

        End If
    End Sub

    Private Function GiveIpStandardUser(ByVal intCust As Integer) As String
        'Ermittele IpStandardUser des Kunden

        Dim result As Object
        Dim conn As New SqlClient.SqlConnection(ConfigurationManager.AppSettings("Connectionstring"))
        Dim command As SqlClient.SqlCommand
        Dim strReturn As String = ""

        Try
            conn.Open()

            command = New SqlClient.SqlCommand("SELECT IpStandardUser FROM Customer" & _
                    " WHERE" & _
                    " CustomerID = " & intCust.ToString, _
                    conn)

            result = command.ExecuteScalar
            If Not result Is Nothing Then
                strReturn = CStr(result)
            End If
        Catch ex As Exception
            lblError.Text = ex.Message
        Finally
            conn.Close()
            conn.Dispose()
        End Try
        Return strReturn
    End Function

    Private Function CheckRestrictedIP() As Integer
        'Prüfe, ob IP in DB existent

        Dim result As Object
        Dim conn As New SqlClient.SqlConnection(ConfigurationManager.AppSettings("Connectionstring"))
        Dim command As SqlClient.SqlCommand
        Dim intReturn As Integer = -1

        Try
            conn.Open()

            command = New SqlClient.SqlCommand("SELECT CustomerID FROM IpAddresses" & _
                    " WHERE" & _
                    " IpAddress = '" & Request.UserHostAddress & "'", _
                    conn)

            result = command.ExecuteScalar
            If Not result Is Nothing AndAlso IsNumeric(result) Then
                intReturn = CInt(result)
            End If
        Catch ex As Exception
            lblError.Text = ex.Message
        Finally
            conn.Close()
            conn.Dispose()
        End Try
        Return intReturn
    End Function

    Private Function CheckUniqueSessionID() As Boolean
        'Prüfe, ob SessionId schon in DB existent
        Dim table As DataTable
        Dim command As New SqlClient.SqlCommand()
        Dim blnReturn As Boolean = True

        Try
            command.CommandText = "SELECT id FROM LogWebAccess" & _
                    " WHERE" & _
                    " idSession = @idSession"

            command.Parameters.AddWithValue("@idSession", CStr(Session.SessionID))

            Dim conn As New SqlClient.SqlConnection(ConfigurationManager.AppSettings("Connectionstring"))
            Dim da As New SqlClient.SqlDataAdapter(command)
            command.Connection = conn
            conn.Open()
            table = New DataTable()
            da.Fill(table)
            If table.Rows.Count > 0 Then
                blnReturn = False
            End If
            conn.Close()
            conn.Dispose()
            da.Dispose()
        Catch ex As Exception

        End Try
        Return blnReturn
    End Function

    Private Sub displayMessages()
        'Nachrichten generieren  

        Dim table As DataTable
        Dim row As System.Data.DataRow
        Dim command As New SqlClient.SqlCommand()

        Dim datTime As DateTime = CDate("01.01.1900 " & Now.ToShortTimeString)  '01.01.1900 !!!

        command.CommandText = "SELECT id,(convert(varchar,creationDate,104) + ' ' + convert(varchar,creationdate,108) + ' - ' + titleText) as titleText,messageText,enableLogin,onlyTEST,onlyPROD FROM LoginMessage" & _
                " WHERE" & _
                " datediff(minute,getdate(),convert(varchar,activeDateFrom,104)+' '+convert(varchar,activeTimeFrom,108)) <=0" & _
                " and" & _
                " datediff(minute,getdate(),convert(varchar,activeDateTo,104)+' '+convert(varchar,activeTimeTo,108)) >=0" & _
                " and active <> 0 and messagecolor = 0" & _
                " ORDER BY id DESC"

        command.Parameters.AddWithValue("@now", Now)
        command.Parameters.AddWithValue("@time", datTime)

        Try
            Dim conn As New SqlClient.SqlConnection(ConfigurationManager.AppSettings("Connectionstring"))
            Dim da As New SqlClient.SqlDataAdapter(command)
            command.Connection = conn
            conn.Open()
            table = New DataTable()
            da.Fill(table)
            conn.Close()
            conn.Dispose()
            da.Dispose()
            'Login erlaubt? 
            cbxLogin_TEST.Checked = True
            cbxLogin_PROD.Checked = True

            Dim text As String
            Dim htext As String

            For Each row In table.Rows
                text = CType(row.Item("titleText"), String)     '--- Überschrift formatieren                
                text = text.Replace("{c=", "{font color=")
                text = text.Replace("{/c}", "{/font}")
                text = text.Replace("{", "<")
                text = text.Replace("}", ">")
                row.Item("titleText") = text

                text = CType(row.Item("messageText"), String)   '--- Nachricht formatieren
                If text.IndexOf("{h}") > 0 Then
                    htext = text.Substring(text.IndexOf("{h}") + 3, text.IndexOf("{/h}") - text.IndexOf("{h}") - 3)
                    text = text.Replace("{h}", "<a href=""")
                    text = text.Replace("{/h}", """ target = ""_BLANK"">" & htext & "</a>")
                End If
                text = text.Replace("{c=", "{font color=")
                text = text.Replace("{/c}", "{/font}")
                text = text.Replace("{", "<")
                text = text.Replace("}", ">")
                row.Item("messageText") = text

                table.AcceptChanges()
                If (CType(row.Item("onlyTEST"), Integer) = 0) Then
                    cbxLogin_TEST.Checked = False
                End If
                If (CType(row.Item("onlyPROD"), Integer) = 0) Then
                    cbxLogin_PROD.Checked = False
                End If
            Next
            Repeater1.DataSource = table
            Repeater1.DataBind()
        Catch ex As Exception
            'm_App = New App(m_User)
            'm_App.WriteErrorText(1, "Not logged in", "Login", "displayMessages", ex.ToString)

            lblError.Text = "Systemfehler! Anmeldedaten konnten nicht geprüft werden."
        End Try

    End Sub

    Private Function checkLogin() As Boolean
        If (m_User.HighestAdminLevel = Kernel.Security.AdminLevel.Master) Then
            Return True
        Else
            '----------------------------------------------------------
            Try
                Base.Kernel.Common.Alert.alert(litAlert, m_User.Customer.CustomerId)
            Catch ex As Exception
                Return False  'Response.Redirect("..\Start\Login.aspx")
            End Try
            '-----------------------------------------------------------


            If m_User.Customer.Locked Then
                'wenn der Kunde komplett gesperrt ist
                Me.lblError.Text = "Die Anwendung steht z.Z. nicht zur Verfügung"
                Return False
            End If


            If (Not cbxLogin_TEST.Checked) And (Not cbxLogin_PROD.Checked) Then
                'Weder CKE noch CKP - Login erlaubt (nur DAD-Admin)
                Me.lblError.Text = "Die Anmeldung ist z.Z. gesperrt."
                Return False
            End If
            If (cbxLogin_TEST.Checked) And (Not cbxLogin_PROD.Checked) Then
                'Nur CKE - Login erlaubt
                If Not (m_User.IsTestUser) Then
                    Me.lblError.Text = "Die Anmeldung ist z.Z. gesperrt."
                    Return False
                End If
            End If
            If (Not cbxLogin_TEST.Checked) And (cbxLogin_PROD.Checked) Then
                'Nur CKP - Login erlaubt
                If (m_User.IsTestUser) Then
                    Me.lblError.Text = "Die Anmeldung ist z.Z. gesperrt."
                    Return False
                End If
            End If
        End If
        'Sonst Anmeldung für alle erlaubt
        Return True
    End Function

    Private Sub cmdLogin_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdLogin.Click
        Try
            Dim blnPasswdlink As Boolean = trPasswortVergessen.Visible
            If Not Me.Session("objUser") Is Nothing AndAlso _
                Me.User.Identity.IsAuthenticated = False AndAlso blnPasswdlink = False Then
                '---JVE: User nicht mehr in der Session gespeichert bzw. nicht Authentifiziert---
                Response.Redirect(BouncePage(Me), True)
                Exit Sub
            End If
            lnkPasswortVergessen.Text = "Passwort vergessen?"
            trPasswortVergessen.Visible = False
            If m_User.Login(txtUsername.Text, txtPassword.Text, Session.SessionID.ToString, Request.Url.AbsoluteUri, blnPasswdlink) Then
                '    If m_User.Login(txtUsername.Text, Session.SessionID.ToString) Then

                'Prüfe IP-Adress-Regelung
                If m_User.Customer.IpRestriction Then
                    Dim strIpError As String = "Die Anmeldung ist in der aktuellen Konfiguration nicht möglich.<br>Setzen Sie sich bitte mit Ihrer Kontaktperson beim DAD bzw. der Christoph Kroschke GmbH in Verbindung."
                    If m_User.Customer.IpAddresses.Select("IpAddress='" & Request.UserHostAddress & "'").Length = 0 Then
                        lblError.Text = strIpError
                        Exit Sub
                    End If
                End If

                If Not checkLogin() Then    'Prüfen, ob Anmeldung erlaubt...
                    Exit Sub
                End If

                m_User.SetLastLogin(Now)
                Session("objUser") = m_User

                If m_User.DoubleLoginTry Then
                    Me.StandardLogin.Visible = False
                    Me.DoubleLogin.Visible = True
                Else
                    System.Web.Security.FormsAuthentication.RedirectFromLoginPage(m_User.UserID.ToString, False)
                End If
            Else
                '############################################################
                'Error-Property bei User-Objekt einfügen und hier darstellen
                If Len(m_User.ErrorMessage) > 0 Then
                    If m_User.ErrorMessage = "4174" Then
                        'Benutzer existiert und die Voraussetzungen zur Passwortanforderung
                        'per geheimer Frage sind gegeben
                        Session("objUser") = m_User
                        lblError.Text = "Fehler bei der Anmeldung."
                        trPasswortVergessen.Visible = True
                        trHelpCenter.Visible = False
                    ElseIf m_User.ErrorMessage = "9999" Then
                        lblError.Text = "Fehler bei der Anmeldung. Prüfen Sie Ihre Eingaben!"
                        trPasswortVergessen.Visible = True
                        trHelpCenter.Visible = False
                    Else
                        If m_User.AccountIsLockedOut AndAlso m_User.AccountIsLockedBy = "User" Then
                            If m_User.Email.Length > 0 And m_User.Customer.ForcePasswordQuestion And m_User.QuestionID > -1 Then
                                System.Web.Security.FormsAuthentication.RedirectFromLoginPage(m_User.UserID.ToString, False)
                            Else
                                lblError.Text = "Fehler bei der Anmeldung<br>(" & m_User.ErrorMessage & ")"
                                trPasswortVergessen.Visible = True
                                trHelpCenter.Visible = False
                                lnkPasswortVergessen_Click(sender, e)
                            End If
                        ElseIf m_User.AccountIsLockedOut AndAlso m_User.AccountIsLockedBy = "Now" Then
                            lblError.Text = "Fehler bei der Anmeldung<br>(" & m_User.ErrorMessage & ")"
                            trPasswortVergessen.Visible = True
                            trHelpCenter.Visible = False
                            lnkPasswortVergessen.Text = "Entsperren"

                        Else
                            lblError.Text = "Fehler bei der Anmeldung<br>(" & m_User.ErrorMessage & ")"
                            trPasswortVergessen.Visible = True
                            trHelpCenter.Visible = False
                        End If

                    End If
                Else
                    lblError.Text = "Fehler bei der Anmeldung."
                    trPasswortVergessen.Visible = False
                    trHelpCenter.Visible = True
                End If
            End If
        Catch ex As Exception
            m_App = New Base.Kernel.Security.App(m_User)
            m_App.WriteErrorText(1, txtUsername.Text, "Login", "cmdLogin_Click", ex.ToString)

            lblError.Text = "Fehler bei der Anmeldung (" & ex.Message & ")"
        End Try
    End Sub


    Private Sub LoginFromURL()

        Try

            Dim cUser As String = ""
            Dim cPass As String = ""

            Dim UserString As String = Request.QueryString("reuser")

            UserString = Replace(UserString, "-", "+")
            UserString = Replace(UserString, "_", "/")

            UserString = Decrypt(UserString)
            Dim UserArr() As String

            UserArr = UserString.Split("|")

            txtUsername.Text = UserArr(0)
            txtPassword.Text = UserArr(1)


            Dim blnPasswdlink As Boolean = trPasswortVergessen.Visible
            'If Not Me.Session("objUser") Is Nothing AndAlso _
            '    Me.User.Identity.IsAuthenticated = False AndAlso blnPasswdlink = False Then
            '    '---JVE: User nicht mehr in der Session gespeichert bzw. nicht Authentifiziert---
            '    Response.Redirect(BouncePage(Me), True)
            '    Exit Sub
            'End If
            lnkPasswortVergessen.Text = "Passwort vergessen?"
            trPasswortVergessen.Visible = False
            If m_User.Login(txtUsername.Text, txtPassword.Text, Session.SessionID.ToString, Request.Url.AbsoluteUri, blnPasswdlink) Then
                '    If m_User.Login(txtUsername.Text, Session.SessionID.ToString) Then

                'Prüfe IP-Adress-Regelung
                'If m_User.Customer.IpRestriction Then
                '    Dim strIpError As String = "Die Anmeldung ist in der aktuellen Konfiguration nicht möglich.<br>Setzen Sie sich bitte mit Ihrer Kontaktperson beim DAD bzw. der Christoph Kroschke GmbH in Verbindung."
                '    If m_User.Customer.IpAddresses.Select("IpAddress='" & Request.UserHostAddress & "'").Length = 0 Then
                '        lblError.Text = strIpError
                '        Exit Sub
                '    End If
                'End If

                If Not checkLogin() Then    'Prüfen, ob Anmeldung erlaubt...
                    Exit Sub
                End If

                m_User.SetLastLogin(Now)
                Session("objUser") = m_User

                If m_User.DoubleLoginTry Then
                    Me.StandardLogin.Visible = False
                    Me.DoubleLogin.Visible = True
                Else
                    System.Web.Security.FormsAuthentication.RedirectFromLoginPage(m_User.UserID.ToString, False)
                End If
                'Else
                '    '############################################################
                '    'Error-Property bei User-Objekt einfügen und hier darstellen
                '    If Len(m_User.ErrorMessage) > 0 Then
                '        If m_User.ErrorMessage = "4174" Then
                '            'Benutzer existiert und die Voraussetzungen zur Passwortanforderung
                '            'per geheimer Frage sind gegeben
                '            Session("objUser") = m_User
                '            lblError.Text = "Fehler bei der Anmeldung."
                '            trPasswortVergessen.Visible = True
                '            trHelpCenter.Visible = False
                '        ElseIf m_User.ErrorMessage = "9999" Then
                '            lblError.Text = "Fehler bei der Anmeldung. Prüfen Sie Ihre Eingaben!"
                '            trPasswortVergessen.Visible = True
                '            trHelpCenter.Visible = False
                '        Else
                '            If m_User.AccountIsLockedOut AndAlso m_User.AccountIsLockedBy = "User" Then
                '                If m_User.Email.Length > 0 And m_User.Customer.ForcePasswordQuestion And m_User.QuestionID > -1 Then
                '                    System.Web.Security.FormsAuthentication.RedirectFromLoginPage(m_User.UserID.ToString, False)
                '                Else
                '                    lblError.Text = "Fehler bei der Anmeldung<br>(" & m_User.ErrorMessage & ")"
                '                    trPasswortVergessen.Visible = True
                '                    trHelpCenter.Visible = False
                '                    lnkPasswortVergessen_Click(sender, e)
                '                End If
                '            ElseIf m_User.AccountIsLockedOut AndAlso m_User.AccountIsLockedBy = "Now" Then
                '                lblError.Text = "Fehler bei der Anmeldung<br>(" & m_User.ErrorMessage & ")"
                '                trPasswortVergessen.Visible = True
                '                trHelpCenter.Visible = False
                '                lnkPasswortVergessen.Text = "Entsperren" 

                '            Else
                '                lblError.Text = "Fehler bei der Anmeldung<br>(" & m_User.ErrorMessage & ")"
                '                trPasswortVergessen.Visible = True
                '                trHelpCenter.Visible = False
                '            End If

                '        End If
                '    Else
                '        lblError.Text = "Fehler bei der Anmeldung."
                '        trPasswortVergessen.Visible = False
                '        trHelpCenter.Visible = True
                '    End If
            End If
        Catch ex As Exception
            m_App = New Base.Kernel.Security.App(m_User)
            m_App.WriteErrorText(1, txtUsername.Text, "Login", "cmdLogin_Click", ex.ToString)

            lblError.Text = "Fehler bei der Anmeldung (" & ex.Message & ")"
        End Try

    End Sub

    Private Function Decrypt(EncryptedString As String) As String
        Dim DecryptedString As [String]


        Dim rd As New RijndaelManaged()
        Dim rijndaelIvLength As Integer = 16
        Dim md5 As New MD5CryptoServiceProvider()
        Dim key As Byte() = md5.ComputeHash(System.Text.Encoding.UTF8.GetBytes("@S33wolf"))

        md5.Clear()

        Dim encdata As Byte() = Convert.FromBase64String(EncryptedString)
        Dim ms As New MemoryStream(encdata)
        Dim iv As Byte() = New Byte(15) {}

        ms.Read(iv, 0, rijndaelIvLength)
        rd.IV = iv
        rd.Key = key

        Dim cs As New CryptoStream(ms, rd.CreateDecryptor(), CryptoStreamMode.Read)


        Dim DataLength As Integer = (CInt(ms.Length) - rijndaelIvLength)

        Dim Data As Byte() = New Byte(DataLength) {}

        Dim i As Integer = cs.Read(Data, 0, Data.Length)

        DecryptedString = System.Text.Encoding.UTF8.GetString(Data, 0, i)
        cs.Close()
        rd.Clear()

        Return DecryptedString

    End Function

    'Public Function psDecrypt(ByVal sQueryString As String) As String

    '    Dim buffer() As Byte
    '    Dim loCryptoClass As New TripleDESCryptoServiceProvider
    '    Dim loCryptoProvider As New MD5CryptoServiceProvider

    '    Try

    '        buffer = Convert.FromBase64String(sQueryString)
    '        loCryptoClass.Key = loCryptoProvider.ComputeHash(ASCIIEncoding.ASCII.GetBytes(lscryptoKey))
    '        loCryptoClass.IV = lbtVector
    '        Return Encoding.ASCII.GetString(loCryptoClass.CreateDecryptor().TransformFinalBlock(buffer, 0, buffer.Length()))
    '    Catch ex As Exception
    '        Throw ex
    '    Finally
    '        loCryptoClass.Clear()
    '        loCryptoProvider.Clear()
    '        loCryptoClass = Nothing
    '        loCryptoProvider = Nothing
    '    End Try


    'End Function

    Private Sub Repeater1_ItemCommand(ByVal source As System.Object, ByVal e As System.Web.UI.WebControls.RepeaterCommandEventArgs) Handles Repeater1.ItemCommand

    End Sub

    Private Sub cmdBack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdBack.Click
        Response.Redirect(BouncePage(Me), True)
    End Sub

    Private Sub cmdContinue_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdContinue.Click
        m_User.DoubleLoginTry = False
        m_User.SetLoggedOn(m_User.UserName, True)
        Session("objUser") = m_User
        System.Web.Security.FormsAuthentication.RedirectFromLoginPage(m_User.UserID.ToString, False)
    End Sub

    Private Sub lnkPasswortVergessen_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lnkPasswortVergessen.Click
        If m_User.UserID = -1 Then
            Session("LostPassword") = 1
            txtWebUserName.Text = txtUsername.Text
            lblRedStar.Visible = True
            tblHelp.Visible = Not tblHelp.Visible
            If tblHelp.Visible = True Then
                GenerateCaptcha()
            End If

        ElseIf m_User.Email.Length > 0 And m_User.Customer.ForcePasswordQuestion And m_User.QuestionID > -1 Then
            System.Web.Security.FormsAuthentication.RedirectFromLoginPage(m_User.UserID.ToString, False)
        Else
            Session("LostPassword") = 1
            tblHelp.Visible = Not tblHelp.Visible
            txtWebUserName.Text = txtUsername.Text
            lblRedStar.Visible = True
            If tblHelp.Visible = True Then
                GenerateCaptcha()
            End If
        End If

    End Sub

    Protected Sub lbtnHelpCenter_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lbtnHelpCenter.Click
        Session("LostPassword") = 0
        lblRedStar.Visible = False
        tblHelp.Visible = Not tblHelp.Visible
        If tblHelp.Visible = True Then
            trProblem.Visible = True
            GenerateCaptcha()
        End If
    End Sub

    Protected Sub cmdSend_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdSend.Click
        Dim num1 As Integer = 0
        Dim num2 As Integer = 0
        If Not validateHelpData() Then
            Int32.TryParse(Me.Session("CaptchaGen1").ToString(), num1)
            Int32.TryParse(Me.Session("CaptchaGen2").ToString(), num2)

            If Me.CodeNumberTextBox.Text = (num1 + num2).ToString() Then
                GenerateMailBody()
                Session("CaptchaGen1") = GenerateRandomCode()
                Session("CaptchaGen2") = GenerateRandomCode()
                Me.MessageLabel.Text = "E-Mail wurde versand!"
                Me.CodeNumberTextBox.Text = ""
                ddlAnrede.SelectedValue = "-"
                txtName.Text = ""
                txtVorname.Text = ""
                txtTelefon.Text = ""
                txtEmail.Text = ""
                txtFirma.Text = ""
                tblHelp.Visible = False

            Else
                ' Display an error message.
                Me.MessageLabel.Text = "Versuchen Sie es nochmal oder generieren Sie neu."
                Me.Session("CaptchaGen1") = GenerateRandomCode()
                Me.Session("CaptchaGen2") = GenerateRandomCode()
                GenerateCaptcha()

            End If
        End If
    End Sub

    Protected Sub ibtnRefresh_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtnRefresh.Click
        Session("CaptchaGen1") = GenerateRandomCode()
        Session("CaptchaGen2") = GenerateRandomCode()
        Me.MessageLabel.Text = ""
        Me.CodeNumberTextBox.Text = ""
        GenerateCaptcha()
    End Sub

    Private Function GenerateRandomCode() As String
        Return String.Format("{0:00}", random.Next(20))
    End Function

    Private Function GetCaptchaURL(imagekey As String) As String
        Dim captchaPath = ConfigurationManager.AppSettings("ExcelPath")

        If String.IsNullOrEmpty(captchaPath) Then
            captchaPath = "C:\inetpub\wwwroot\Portal\Temp\Excel\" ' (default)
        End If

        ' ensure that we have a full, rooted path..
        captchaPath = Path.GetFullPath(captchaPath)

        If Not Directory.Exists(captchaPath) Then Directory.CreateDirectory(captchaPath)

        ' .... filename==content - not that much of a secure captcha ....
        Dim localFile = Path.Combine(captchaPath, imagekey & ".jpg")

        ' generate captcha if it doesn't exists
        If Not File.Exists(localFile) Then
            Dim ci = New CaptchaImage(imagekey, 80, 30, "Century Schoolbook")
            ci.Image.Save(localFile, ImageFormat.Jpeg)
            ci.Dispose()
        End If

        ' get the full, rooted path of application root
        Dim appRoot = HttpContext.Current.Server.MapPath("~/")
        appRoot = Path.GetFullPath(appRoot)

        Dim relFile = localFile.Replace(appRoot, "~/").Replace("\"c, "/"c)

        Return relFile
    End Function

    Private Sub GenerateCaptcha()
        If Session("CaptchaGen1") IsNot Nothing Then
            Dim imagekey As String = Session("CaptchaGen1").ToString
            imgCatcha1.Src = GetCaptchaURL(imagekey)
        End If
        If Session("CaptchaGen2") IsNot Nothing Then
            Dim imagekey As String = Session("CaptchaGen2").ToString
            imgCatcha2.Src = GetCaptchaURL(imagekey)
        End If
    End Sub

    Private Function validateHelpData() As Boolean
        MessageLabel.Text = ""
        Dim breturn As Boolean = False
        If CInt(Session("LostPassword")) = 1 Then
            If txtWebUserName.Text.Trim.Length = 0 Then
                MessageLabel.Text = "Bitte geben Sie Ihren Benutzernamen ein!"
                txtProblem.BorderColor = System.Drawing.ColorTranslator.FromHtml("#C40000")
                breturn = True
            End If
        End If
        If ddlAnrede.SelectedValue = "-" Then
            SetzeHinweisPflichtfeld()
            ddlAnrede.BorderColor = System.Drawing.ColorTranslator.FromHtml("#C40000")
            breturn = True
        End If
        If txtName.Text.Trim.Length = 0 Then
            SetzeHinweisPflichtfeld()
            txtName.BorderColor = System.Drawing.ColorTranslator.FromHtml("#C40000")
            breturn = True
        End If
        If txtVorname.Text.Trim.Length = 0 Then
            SetzeHinweisPflichtfeld()
            txtVorname.BorderColor = System.Drawing.ColorTranslator.FromHtml("#C40000")
            breturn = True
        End If
        If txtFirma.Text.Trim.Length = 0 Then
            SetzeHinweisPflichtfeld()
            txtFirma.BorderColor = System.Drawing.ColorTranslator.FromHtml("#C40000")
            breturn = True
        End If
        If txtTelefon.Text.Trim.Length = 0 Then
            SetzeHinweisPflichtfeld()
            txtTelefon.BorderColor = System.Drawing.ColorTranslator.FromHtml("#C40000")
            breturn = True
        End If
        If txtEmail.Text.Trim.Length = 0 Then
            SetzeHinweisPflichtfeld()

            txtEmail.BorderColor = System.Drawing.ColorTranslator.FromHtml("#C40000")
            breturn = True
        Else
            If HelpProcedures.EmailAddressCheck(txtEmail.Text.Trim) = False Then
                MessageLabel.Text = "<br />Email-Adresse nicht im richtigen Format(yxz@firma.de))"
                txtEmail.BorderColor = System.Drawing.ColorTranslator.FromHtml("#C40000")
                breturn = True
            End If
        End If
        If CInt(Session("LostPassword")) = 0 Then
            If txtProblem.Text.Trim.Length = 0 Then
                SetzeHinweisPflichtfeld()
                txtProblem.BorderColor = System.Drawing.ColorTranslator.FromHtml("#C40000")
                breturn = True
            End If
        End If
        validateHelpData = breturn
    End Function

    Private Sub GenerateMailBody()
        Dim str As String = ""

        If CInt(Session("LostPassword")) = 1 Then
            str = "Helpdesk-Auftrag: Passwort vergessen" & vbCrLf & "----------------------" & vbCrLf & vbCrLf
        Else
            str = "Helpdesk-Auftrag" & vbCrLf & "----------------------" & vbCrLf & vbCrLf
        End If

        If txtWebUserName.Text.Trim.Length > 0 Then
            str &= "Benutzername: " & txtWebUserName.Text & vbCrLf
        Else
            str &= "Benutzername: nicht angegeben" & vbCrLf
        End If

        str &= vbCrLf
        'Benutzerdaten
        str &= "ANREDE         : " & ddlAnrede.SelectedItem.Text & vbCrLf
        str &= "NAME           : " & txtName.Text & vbCrLf
        str &= "VORNAME        : " & txtVorname.Text & vbCrLf
        str &= "FIRMA          : " & txtFirma.Text & vbCrLf
        str &= "TELEFON        : " & txtTelefon.Text & vbCrLf
        str &= "EMAILADRESSE   : " & txtEmail.Text & vbCrLf
        If CInt(Session("LostPassword")) = 0 Then
            str &= "Frage/Problem  : " & txtProblem.Text & vbCrLf
        End If


        str &= vbCrLf & "AUF DIESE MAIL NICHT ANTWORTEN."
        SendMailToDAD(str)
    End Sub
    Private Sub SendMailToDAD(ByVal message As String)

        Try
            Dim Mail As System.Net.Mail.MailMessage
            Dim smtpMailSender As String = ""
            Dim smtpMailServer As String = ""

            smtpMailSender = ConfigurationManager.AppSettings("SmtpMailSender")
            Mail = New System.Net.Mail.MailMessage(smtpMailSender, "Web-Administrator@dad.de", "Helpdeskanfrage der Login-Seite", message)
            'Mail = New System.Net.Mail.MailMessage(smtpMailSender, "oliver.rudolph@kroschke.de", "Helpdeskanfrage der Login-Seite", message)
            Mail.IsBodyHtml = False
            smtpMailServer = ConfigurationManager.AppSettings("SmtpMailServer")
            Dim client As New System.Net.Mail.SmtpClient(smtpMailServer)
            client.Send(Mail)
        Catch ex As Exception
            MessageLabel.Text = "Fehler beim Versenden der E-Mail."
        End Try


    End Sub

    Private Sub SetzeHinweisPflichtfeld()
        MessageLabel.Text = "Bitte Pflichtfelder ausfüllen!"
    End Sub

End Class

' ************************************************
' $History: Login.aspx.vb $
' 
' *****************  Version 17  *****************
' User: Rudolpho     Date: 17.05.11   Time: 16:22
' Updated in $/CKAG/portal/Start
' 
' *****************  Version 16  *****************
' User: Rudolpho     Date: 3.03.11    Time: 17:02
' Updated in $/CKAG/portal/Start
' 
' *****************  Version 15  *****************
' User: Rudolpho     Date: 8.09.10    Time: 12:30
' Updated in $/CKAG/portal/Start
' 
' *****************  Version 14  *****************
' User: Rudolpho     Date: 8.09.10    Time: 10:12
' Updated in $/CKAG/portal/Start
' 
' *****************  Version 13  *****************
' User: Rudolpho     Date: 7.09.10    Time: 14:17
' Updated in $/CKAG/portal/Start
' 
' *****************  Version 12  *****************
' User: Rudolpho     Date: 7.09.10    Time: 10:57
' Updated in $/CKAG/portal/Start
' 
' *****************  Version 11  *****************
' User: Rudolpho     Date: 28.06.10   Time: 9:50
' Updated in $/CKAG/portal/Start
' 
' *****************  Version 10  *****************
' User: Rudolpho     Date: 23.06.10   Time: 15:16
' Updated in $/CKAG/portal/Start
' ITA:  3794
' 
' *****************  Version 9  *****************
' User: Rudolpho     Date: 18.06.10   Time: 17:05
' Updated in $/CKAG/portal/Start
' 
' *****************  Version 8  *****************
' User: Rudolpho     Date: 1.09.09    Time: 13:19
' Updated in $/CKAG/portal/Start
' 
' *****************  Version 7  *****************
' User: Rudolpho     Date: 28.04.09   Time: 13:45
' Updated in $/CKAG/portal/Start
' 
' *****************  Version 6  *****************
' User: Rudolpho     Date: 27.04.09   Time: 11:14
' Updated in $/CKAG/portal/Start
' 
' *****************  Version 5  *****************
' User: Jungj        Date: 19.03.09   Time: 16:25
' Updated in $/CKAG/portal/Start
' ITA 2156 testfertig
' 
' *****************  Version 4  *****************
' User: Rudolpho     Date: 27.11.08   Time: 9:45
' Updated in $/CKAG/portal/Start
' 
' *****************  Version 3  *****************
' User: Rudolpho     Date: 11.04.08   Time: 11:57
' Updated in $/CKAG/portal/Start
' Migration OR
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
' *****************  Version 20  *****************
' User: Rudolpho     Date: 5.02.08    Time: 13:46
' Updated in $/CKG/Portal/Start
' ORU 20080205: von UHa einfügte Abfrage für Testzwecke???  entfernt
' 
' *****************  Version 19  *****************
' User: Uha          Date: 22.01.08   Time: 11:40
' Updated in $/CKG/Portal/Start
' 'ITA 1644: Logout für Standarduser (mit hinterlegter IP) führt auf
' Logout-Seite ohne Weiterleitung
' 
' *****************  Version 18  *****************
' User: Uha          Date: 22.01.08   Time: 10:02
' Updated in $/CKG/Portal/Start
' ITA 1644: Ermöglicht Login nur mit IP und festgelegtem Benutzer -
' Standard-Benutzer darf leer sein
' 
' *****************  Version 17  *****************
' User: Uha          Date: 22.01.08   Time: 9:24
' Updated in $/CKG/Portal/Start
' ITA 1644: Ermöglicht Login nur mit IP und festgelegtem Benutzer - Jetzt
' per URL auch Anmeldung möglich
' 
' *****************  Version 16  *****************
' User: Uha          Date: 21.01.08   Time: 18:09
' Updated in $/CKG/Portal/Start
' ITA 1644: Ermöglicht Login nur mit IP und festgelegtem Benutzer
' 
' *****************  Version 15  *****************
' User: Uha          Date: 30.08.07   Time: 12:36
' Updated in $/CKG/Portal/Start
' ITA 1280: Paßwortversand im Web auf Benutzerwunsch
' 
' *****************  Version 14  *****************
' User: Uha          Date: 31.05.07   Time: 11:41
' Updated in $/CKG/Portal/Start
' ITA 1077 - Login bei bereits aktiver Anmeldung ermöglichen
' 
' *****************  Version 13  *****************
' User: Uha          Date: 3.05.07    Time: 18:37
' Updated in $/CKG/Portal/Start
' Änderungen aus StartApplication vom 02.05.2007 Mittags übernommen
' 
' ************************************************
