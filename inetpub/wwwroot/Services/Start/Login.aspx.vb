Imports CKG.Base.Kernel
Imports CKG.Base.Kernel.Security
Imports CKG.Base.Kernel.Common.Common
Imports CKG.Base
Imports System.Drawing.Imaging
Imports CKG.Base.Business
Imports System.Security.Cryptography
Imports System.IO
Imports System.Reflection
Imports WebTools.Services
Imports CKG.Base.Business.HelpProcedures
Imports GeneralTools.Services
Imports GeneralTools.Models

Partial Public Class Login
    Inherits System.Web.UI.Page

    Private m_User As New User()
    Private m_App As Base.Kernel.Security.App
    Private random As New Random

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

    Private Function GiveIpStandardUser(ByVal intCust As Integer) As String
        'Ermittele IpStandardUser des Kunden

        Dim result As Object
        Dim conn As New SqlClient.SqlConnection(ConfigurationManager.AppSettings("Connectionstring"))
        Dim command As SqlClient.SqlCommand
        Dim strReturn As String = ""

        Try
            conn.Open()

            command = New SqlClient.SqlCommand("SELECT IpStandardUser FROM Customer" &
                    " WHERE" &
                    " CustomerID = " & intCust.ToString,
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

            command = New SqlClient.SqlCommand("SELECT CustomerID FROM IpAddresses" &
                    " WHERE" &
                    " IpAddress = '" & Request.UserHostAddress & "'",
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
            command.CommandText = "SELECT id FROM LogWebAccess" &
                    " WHERE" &
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

    ''' <summary>
    ''' Füllt die DropdownList mit den Login-Links
    ''' </summary>
    ''' <remarks></remarks>

    Private Function LoadLoginLinks()

        Dim TempTable As New DataTable
        Dim cn As New SqlClient.SqlConnection(m_User.App.Connectionstring)
        cn.Open()

        Dim daLoginLink As SqlClient.SqlDataAdapter
        daLoginLink = New SqlClient.SqlDataAdapter("SELECT * FROM WebUserUploadLoginLink", cn)

        daLoginLink.Fill(TempTable)

        cn.Close()
        cn.Dispose()

        Return TempTable

    End Function

    Private Sub displayMessages()
        Try
            Dim table As New DataTable()
            table.Columns.Add("Created", GetType(DateTime))
            table.Columns.Add("Title", GetType(String))
            table.Columns.Add("Message", GetType(String))

            Dim jetzt As DateTime = DateTime.Now
            Dim text As String
            Dim htext As String

            cbxLogin_TEST.Checked = True
            cbxLogin_PROD.Checked = True

            Using conn As New SqlClient.SqlConnection(ConfigurationManager.AppSettings("Connectionstring"))
                conn.Open()

                Dim command As SqlClient.SqlCommand = conn.CreateCommand()

                command.CommandText = "SELECT * FROM LoginUserMessage" &
                    " WHERE (@jetzt BETWEEN ShowMessageFrom AND ShowMessageTo) OR (@jetzt BETWEEN LockLoginFrom AND LockLoginTo)" &
                    " ORDER BY ID DESC"

                command.Parameters.AddWithValue("@jetzt", jetzt)

                Using dr As SqlClient.SqlDataReader = command.ExecuteReader()
                    While dr.Read()

                        'Nachricht anzeigen?
                        If Not IsDBNull(dr("ShowMessageFrom")) AndAlso jetzt > CType(dr("ShowMessageFrom"), DateTime) AndAlso Not IsDBNull(dr("ShowMessageTo")) AndAlso jetzt < CType(dr("ShowMessageTo"), DateTime) Then
                            Dim newRow As DataRow = table.NewRow()

                            newRow("Created") = CType(dr("Created"), DateTime)

                            '--- Überschrift formatieren      
                            text = dr("Title").ToString()
                            text = text.Replace("{c=", "{font color=")
                            text = text.Replace("{/c}", "{/font}")
                            text = text.Replace("{", "<")
                            text = text.Replace("}", ">")
                            newRow("Title") = text

                            '--- Nachricht formatieren
                            text = dr("Message").ToString()
                            If text.Contains("{h}") Then
                                htext = text.Substring(text.IndexOf("{h}") + 3, text.IndexOf("{/h}") - text.IndexOf("{h}") - 3)
                                text = text.Replace("{h}", "<a href=""")
                                text = text.Replace("{/h}", """ target = ""_BLANK"">" & htext & "</a>")
                            End If
                            text = text.Replace("{c=", "{font color=")
                            text = text.Replace("{/c}", "{/font}")
                            text = text.Replace("{", "<")
                            text = text.Replace("}", ">")
                            newRow("Message") = text

                            table.Rows.Add(newRow)
                        End If

                        'Login sperren?
                        If Not IsDBNull(dr("LockLoginFrom")) AndAlso jetzt > CType(dr("LockLoginFrom"), DateTime) AndAlso Not IsDBNull(dr("LockLoginTo")) AndAlso jetzt < CType(dr("LockLoginTo"), DateTime) Then
                            If CType(dr("LockForTest"), Boolean) Then
                                cbxLogin_TEST.Checked = False
                            End If
                            If CType(dr("LockForProd"), Boolean) Then
                                cbxLogin_PROD.Checked = False
                            End If
                        End If

                    End While
                End Using

                conn.Close()
            End Using

            Repeater1.DataSource = table
            Repeater1.DataBind()
        Catch
        End Try
    End Sub

    Private Sub displaySecurityCertificate()
        Try
            divSicherheitszertifikat.InnerHtml = GeneralTools.Services.GeneralConfiguration.GetConfigValue("Login", "SecurityCertificate")
        Catch
        End Try
    End Sub

    Private Function checkLogin() As Boolean
        If (m_User.HighestAdminLevel = Security.AdminLevel.Master) Then
            Return True
        Else
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
            MessageLabel.Text = ""
            Dim blnPasswdlink As Boolean = trPasswortVergessen.Visible
            If Not Me.Session("objUser") Is Nothing AndAlso
                Me.User.Identity.IsAuthenticated = False AndAlso blnPasswdlink = False Then
                '---JVE: User nicht mehr in der Session gespeichert bzw. nicht Authentifiziert---
                Response.Redirect(BouncePage(Me), True)
                Exit Sub
            End If
            lnkPasswortVergessen.Text = "Passwort vergessen?"
            trPasswortVergessen.Visible = False
            If m_User.Login(txtUsername.Text, txtPassword.Text, Session.SessionID.ToString, Request.Url.AbsoluteUri, blnPasswdlink) Then

                Session("UrlRemoteLogin_LogoutUrl") = m_User.Customer.LogoutLink

                ' --- 20.06.2013, MJE
                ' --- MVC Integration:
                ' ---
                If Not (Request.QueryString("ReturnURL") Is Nothing) Then
                    Dim returnUrl As String = Request.QueryString("ReturnURL").ToLower()
                    If (returnUrl.Contains("mvc/")) Then

                        If (m_User.Customer.PortalType <> "MVC") Then
                            ' --- 27.06.2014, MJE
                            ' --- Fix für 
                            ' --- Wenn Management sich
                            ' --- 1.       mich in einem Metronic Kunden anmelde
                            ' --- 2.       mich dort per „logout“ wieder abmelde
                            ' --- 3.       mich danach bei einem Services-Kunden anmelde
                            ' --- landet man auf der letzten Metronic-Seite ohne Rahmenlayout, nicht aber auf der gewünschten Services-Seite.
                            ' ---
                            ' --- Lösung: 
                            ' --- Die ReturnUrl aus dem Request.QueryString entfernen, wenn Kunde kein MVC Metronic Layout hat
                            Dim isreadonly As PropertyInfo = GetType(NameValueCollection).GetProperty("IsReadOnly", BindingFlags.Instance Or BindingFlags.NonPublic)
                            isreadonly.SetValue(Request.QueryString, False, Nothing)
                            Request.QueryString.Remove("ReturnUrl")
                            isreadonly.SetValue(Request.QueryString, True, Nothing)
                        Else
                            Dim urlParameterChar As String = "?"
                            If (returnUrl.Contains("?")) Then
                                urlParameterChar = "&"
                            End If

                            m_User.SetLastLogin(DateTime.Now)
                            Session("objUser") = m_User

                            'FormsAuthentication.RedirectFromLoginPage(m_User.UserID.ToString(), False)
                            Response.Redirect(String.Format("{0}{1}un={2}", returnUrl, urlParameterChar, CryptoMd5Web.EncryptToUrlEncoded(m_User.UserName)))
                            Exit Sub
                        End If
                    End If
                End If



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



                'User korrekt authentifiziert
                SaveAndRedirectUser(txtUsername.Text, txtPassword.Text)
            Else
                '############################################################
                'Error-Property bei User-Objekt einfügen und hier darstellen
                If Len(m_User.ErrorMessage) > 0 Then
                    If m_User.ErrorMessage = "4174" Then
                        'Benutzer existiert und die Voraussetzungen zur Passwortanforderung
                        'per geheimer Frage sind gegeben
                        Session("objUser") = m_User
                        lblError.Text = "Fehler bei der Anmeldung"
                        trPasswortVergessen.Visible = True
                        trHelpCenter.Visible = False
                    ElseIf m_User.ErrorMessage = "9999" Then
                        lblError.Text = "Fehler bei der Anmeldung"
                        trPasswortVergessen.Visible = True
                        trHelpCenter.Visible = False
                    Else
                        ' Wenn User sich selbst gesperrt hat oder vom Regelprozess gesperrt wurde -> Anzeige Kundenadmin-Kontakt oder Passwortneuanforderung
                        If m_User.AccountIsLockedOut AndAlso (m_User.AccountIsLockedBy = "User" OrElse m_User.AccountIsLockedBy = "Now" OrElse m_User.AccountIsLockedBy = "Regelprozess") Then

                            If m_User.Customer.CustomerPasswordRules.DontSendEmail OrElse m_User.Customer.Selfadministration > 0 Then
                                ' Kontaktdaten bzw. Hinweistext des Kunden anzeigen
                                trRequestNewPassword.Visible = False
                                If Not String.IsNullOrEmpty(m_User.Customer.SelfadministrationContact) Then
                                    trSelfadminContact.Visible = True
                                    divSelfadminContact.InnerHtml = TranslateHTML(m_User.Customer.SelfadministrationContact, TranslationDirection.ReadHTML)
                                Else
                                    trSelfadminContact.Visible = False
                                End If
                                trHelpCenter.Visible = True
                            Else
                                ' Passwortneuanforderung ermöglichen
                                trRequestNewPassword.Visible = True
                                trSelfadminContact.Visible = False
                                trHelpCenter.Visible = False
                                Session("objUser") = m_User
                            End If

                            trPasswortVergessen.Visible = True
                        Else
                            trRequestNewPassword.Visible = False
                            trSelfadminContact.Visible = False
                            trHelpCenter.Visible = True
                            trPasswortVergessen.Visible = False
                        End If

                        lblError.Text = "Fehler bei der Anmeldung (" & m_User.ErrorMessage & ")"

                    End If
                Else
                    lblError.Text = "Fehler bei der Anmeldung"
                    trPasswortVergessen.Visible = True
                End If
            End If
        Catch ex As Exception
            m_App = New Base.Kernel.Security.App(m_User)
            m_App.WriteErrorText(1, txtUsername.Text, "Login", "cmdLogin_Click", ex.ToString)

            lblError.Text = "Fehler bei der Anmeldung (" & ex.Message & ")"

        End Try
    End Sub

    Private Sub SaveAndRedirectUser(userName As String, password As String)
        m_User.SetLastLogin(Now)
        Session("objUser") = m_User

        If m_User.Customer.LoginLinkID <> 0 AndAlso m_User.Customer.LoginLinkID <> 2 Then
            'Ein doppeltes Login wird dadurch ohnehin nicht verhindert
            m_User.DoubleLoginTry = False
        End If

        If m_User.DoubleLoginTry Then
            Me.StandardLogin.Visible = False
            Me.DoubleLogin2.Visible = True
        Else
            If m_User.Customer.LoginLinkID <> 0 Then ''AndAlso m_User.Customer.LoginLinkID <> 2 Then

                If (Not Request.QueryString("portal") Is Nothing AndAlso Not Request.QueryString("portal") Is String.Empty) Then
                    If (Not m_User.Customer.AccountingArea = -1 AndAlso Not Request.QueryString("portal") = False) Then
                        SetRedirect(userName, password)
                    End If
                Else
                    ' Services Check
                    Dim urlLink = DbGetStringValue(String.Format("select isnull(Text,'') from WebUserUploadLoginLink where ID = {0}", m_User.Customer.LoginLinkID))

                    If (Not urlLink Is Nothing AndAlso Not urlLink.ToLower().Contains("/services/start/")) Then
                        SetRedirect(userName, password)
                    End If
                End If

            End If

            RedirectFromLoginPage(m_User)
        End If
    End Sub

    Private Sub cmdBack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdBack.Click
        Response.Redirect(BouncePage(Me), True)
    End Sub

    Private Sub cmdContinue_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdContinue.Click
        m_User.DoubleLoginTry = False

        If m_User.Customer.LoginLinkID <> 0 AndAlso m_User.Customer.LoginLinkID <> 2 Then
            SetRedirect()
        End If

        m_User.SetLoggedOn(m_User.UserName, True)
        Session("objUser") = m_User
        'System.Web.Security.FormsAuthentication.RedirectFromLoginPage(m_User.UserID.ToString, False)
        RedirectFromLoginPage(m_User)
    End Sub

    Private Sub cmdRequestNewPassword_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdRequestNewPassword.Click
        MessageLabel.Text = ""
        trHelpCenter.Visible = False
        If Not String.IsNullOrEmpty(txtConfirmEmail.Text) AndAlso txtConfirmEmail.Text.ToUpper() = m_User.Email.ToUpper() Then

            Dim errorMessage As String = ""
            If m_User.SendPasswordResetMail(errorMessage, Security.User.PasswordMailMode.Zuruecksetzen) Then
                m_User.UnlockAccount()
            Else
                lblError.Text = "Es ist ein Fehler aufgetreten: " & errorMessage
            End If

        Else
            lblError.Text = "Die Email-Adresse stimmt nicht mit der hinterlegten Adresse überein."
        End If

    End Sub

    Private Sub lnkPasswortVergessen_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lnkPasswortVergessen.Click
        MessageLabel.Text = ""
        If m_User.UserID = -1 Then
            Session("LostPassword") = 1
            txtWebUserName.Text = txtUsername.Text
            lblRedStar.Visible = True
            tblHelp.Visible = Not tblHelp.Visible
            If tblHelp.Visible = True Then
                GenerateCaptcha()
            End If
        ElseIf m_User.Email.Length > 0 And m_User.Customer.ForcePasswordQuestion And m_User.QuestionID > -1 Then
            'System.Web.Security.FormsAuthentication.RedirectFromLoginPage(m_User.UserID.ToString, False)
            RedirectFromLoginPage(m_User)
        Else
            Session("LostPassword") = 1
            txtWebUserName.Text = txtUsername.Text
            lblRedStar.Visible = True
            tblHelp.Visible = Not tblHelp.Visible
            If tblHelp.Visible = True Then
                GenerateCaptcha()
            End If
        End If
    End Sub

    Protected Sub btnEmpty_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnEmpty.Click
        cmdLogin_Click(sender, e)
    End Sub

    Private Sub Login_PreLoad(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreLoad

        Dim redirectOccurred As Boolean = False
        If Not Me.Session("objUser") Is Nothing Then
            m_User = CType(Session("objUser"), Base.Kernel.Security.User)
            If Not (m_User.LoggedOn And m_User.DoubleLoginTry) Then
                If Me.User.Identity.IsAuthenticated = False Then
                    If trPasswortVergessen.Visible = False Then
                        Response.Redirect(BouncePage(Me), True)
                        redirectOccurred = True
                    End If
                End If
            End If
        End If


        If (UrlRemoteUserProcessLogin()) Then Return

        litAlert.Text = ""
        txtUsername.Focus()
        Page.Title = "Anmeldung"
        If Not IsPostBack Then
            If Not CheckUniqueSessionID() Then
                Response.Redirect(BouncePage(Me), True)
                redirectOccurred = True
            End If
            displaySecurityCertificate()
            displayMessages()
            Me.StandardLogin.Visible = True
            Me.DoubleLogin2.Visible = False
            Session("CaptchaGen1") = GenerateRandomCode()
            Session("CaptchaGen2") = GenerateRandomCode()

            'Prüfe zugreifende IP
            If (Not Request.QueryString("IFrameLogon") Is Nothing) Then
                FormsAuthentication.RedirectFromLoginPage("IFrameLogon", False)
                redirectOccurred = True
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
                        If m_User.Login(strIpStandardUser, Session.SessionID.ToString, Request.Url.AbsoluteUri) Then
                            'System.Web.Security.FormsAuthentication.RedirectFromLoginPage(m_User.UserID.ToString, False)
                            RedirectFromLoginPage(m_User)
                            redirectOccurred = True
                        Else
                            lblError.Text = strIpError & "<br>(" & m_User.ErrorMessage & ")"
                            cmdLogin.Enabled = False
                        End If
                    End If
                End If

            End If

        End If

        If (redirectOccurred) Then
            Return
        End If

        Dim urlReferrer As String = ""
        If Not (Request.UrlReferrer Is Nothing) Then
            urlReferrer = Request.UrlReferrer.ToString()
        End If
        Dim requestReturnUrl As String = ""
        If Not (Request("ReturnUrl") Is Nothing) Then
            requestReturnUrl = Request("ReturnUrl").ToString()
        End If
        Dim url As String = ""
        If Not (Request.Url Is Nothing) Then
            url = Request.Url.ToString()
        End If

        Dim userIsEmpty As Boolean = (Session("objUser") Is Nothing)
        Dim urlReferrerIsValid As Boolean = (urlReferrer <> "")
        Dim urlReferrerIsServicesLogin As Boolean = (urlReferrer.ToLower().Contains("start/login.aspx") And Not requestReturnUrl.ToLower().Contains("servicesmvc"))
        Dim urlIsNewDadPortalLink As Boolean = (url.ToLower().Contains("portal.dad.de") Or url.ToLower().Contains("on.kroschke.de") Or url.ToLower().Contains("on2.kroschke.de") Or url.ToLower().Contains("vms012.kroschke.de") Or url.ToLower().Contains("vms026.kroschke.de") Or url.ToLower().Contains("localhost") Or ConfigurationManager.AppSettings("ForceMvcLogin").NotNullOrEmpty().ToUpper() = "TRUE")

        If (userIsEmpty And Not urlReferrerIsServicesLogin And (urlIsNewDadPortalLink Or urlReferrerIsValid)) Then
            Try
                Dim dtStart As DateTime = New DateTime(2014, 10, 27)
                If (DateTime.Now <= dtStart.AddDays(5)) Then
                    Dim info As String = String.Format("urlReferrer: {0} ; " +
                                                       "requestReturnUrl: {1} ; " +
                                                       "urlReferrerIsServicesLogin: {2} ; " +
                                                       "urlIsNewDadPortalLink: {3} ; " +
                                                       "url: {4} ; " +
                                                       "IP-Address: {5} ; ",
                                                       urlReferrer, requestReturnUrl, urlReferrerIsServicesLogin, urlIsNewDadPortalLink, url, Request.UserHostAddress)
                    Dim logService As GeneralTools.Services.LogService = New GeneralTools.Services.LogService("Services-Login", "c:\tmp\Log-IpAddress.log")
                    logService.LogInfo(Nothing, info)
                End If
            Catch
            End Try

            Response.Redirect("/ServicesMvc/Login/Index")
        End If
    End Sub

    Function UrlRemoteUserProcessLogin() As Boolean
        Dim remoteUserName As String = "", remoteUserPwdHashed As String = ""
        UrlRemoteUserTryLogin(remoteUserName, remoteUserPwdHashed)

        If (Not String.IsNullOrEmpty(remoteUserName)) Then
            If m_User.Login(remoteUserName, remoteUserPwdHashed, Session.SessionID.ToString, Request.Url.AbsoluteUri, False) Then
                Session("UrlRemoteLogin_LogoutUrl") = m_User.Customer.LogoutLink
                'RedirectFromLoginPage(m_User)
                SaveAndRedirectUser(remoteUserName, remoteUserPwdHashed)
            Else
                lblError.Text = "URL Remote-Login fehlgeschlagen! " & "<br>(" & m_User.ErrorMessage & ")"
                cmdLogin.Enabled = False
            End If
            Return True
        End If

        Return False
    End Function

    Sub UrlRemoteUserTryLogin(ByRef userName As String, ByRef userPwd As String)

        If (IsPostBack) Then Return

        If (UrlRemoteUserTryLoginFromMvc(userName, userPwd)) Then
            Return
        End If

        If (Request.QueryString("ra") Is Nothing) Then Return
        If (Request.QueryString("rb") Is Nothing) Then Return

        Dim rid As String = Request.QueryString("ra").ToString
        Dim dat As String = Request.QueryString("rb").ToString

        If (String.IsNullOrEmpty(rid)) Then Return
        If (String.IsNullOrEmpty(dat)) Then Return

        If (rid.Length < 30) Then Return
        If (Not UrlRemoteHashedDateIsValid(dat)) Then Return

        Dim webUserID As Integer = DbGetIntValue(String.Format("select isnull(UserID,-1) from WebUser where UrlRemoteLoginKey = '{0}'", rid))
        If (webUserID = -1) Then Return

        Dim customerID As Integer = DbGetIntValue(String.Format("select isnull(CustomerID,-1) from WebUser where UserID = {0}", webUserID))
        If (customerID = -1) Then Return

        Dim customerRemoteLoginAllowed As Boolean = (0 < DbGetIntValue(String.Format("select isnull(AllowUrlRemoteLogin,-1) from Customer where CustomerID = {0}", customerID)))
        If (Not customerRemoteLoginAllowed) Then Return

        userName = DbGetStringValue(String.Format("select isnull(Username,'') from WebUser where UserID = {0}", webUserID))
        userPwd = DbGetStringValue(String.Format("select isnull(Password,'') from WebUser where UserID = {0}", webUserID))

        If (Not Request.QueryString("logouturl") Is Nothing) Then
            Session("UrlRemoteLogin_LogoutUrl") = HttpUtility.UrlDecode(Request.QueryString("logouturl").ToString)
        End If
    End Sub

    Function UrlRemoteUserTryLoginFromMvc(ByRef userName As String, ByRef userPwd As String)

        If (Request.QueryString("unm") Is Nothing) Then Return False

        Dim un As String = Request.QueryString("unm").ToString
        un = un.Replace(" ", "%2b")
        Dim unDecoded As String = CryptoMd5Web.DecryptFromUrlEncoded(un)

        Dim ar = Split(unDecoded, "_")
        If (ar(0).ToString.ToLower() <> "frommvc") Then Return False

        Dim userId As Integer = Int32.Parse(ar(1))
        Dim loginDateTime As DateTime = DateTime.Parse(ar(2).Replace("-", " "))
        Dim differenceToNow As TimeSpan = loginDateTime - DateTime.Now

        If (Math.Abs(differenceToNow.TotalMinutes) > 2) Then Return False

        userName = DbGetStringValue(String.Format("select isnull(Username,'') from WebUser where UserID = {0}", userId))
        userPwd = DbGetStringValue(String.Format("select isnull(Password,'') from WebUser where UserID = {0}", userId))

        If (userName Is Nothing Or userName = "") Then Return False
        If (userPwd Is Nothing Or userPwd = "") Then Return False

        Return True
    End Function

    Function UrlRemoteHashedDateIsValid(strHashedDate As String) As Boolean

        If (String.IsNullOrEmpty(strHashedDate)) Then Return False
        If (strHashedDate.Length <> 10) Then Return False

        Dim strEncryptedDate As String = ""
        Dim i As Integer, reversal As Boolean = False
        For i = 0 To strHashedDate.Length - 1
            Dim hashedChar As Char = strHashedDate(i)
            Dim hashedVal As Integer = Asc(hashedChar)
            strEncryptedDate = strEncryptedDate & Chr(IIf(Not reversal, hashedVal, Asc("A") + Asc("Z") - hashedVal) - 30)
            reversal = Not reversal
        Next

        If (String.IsNullOrEmpty(strEncryptedDate)) Then Return False
        If (strEncryptedDate.Length <> 10) Then Return False

        Dim intHour As Integer
        If (Not Int32.TryParse(strEncryptedDate.Substring(0, 2), intHour)) Then Return False

        Dim strDate As String = strEncryptedDate.Substring(2, 8)
        If (Not IsNumeric(strDate)) Then Return False

        Dim dDate As DateTime
        Try
            dDate = New DateTime(Int32.Parse(strDate.Substring(4, 4)), Int32.Parse(strDate.Substring(2, 2)), Int32.Parse(strDate.Substring(0, 2)), intHour, 0, 0)
        Catch
            Return False
        End Try

        Dim differenceToNow As TimeSpan = dDate - DateTime.Now
        If (Math.Abs(differenceToNow.TotalMinutes) > 120) Then Return False

        Return True

    End Function

    Private Sub SetRedirect()
        SetRedirect(txtUsername.Text, txtPassword.Text)
    End Sub

    Private Sub SetRedirect(userName As String, password As String)

        Dim TempTable As DataTable = LoadLoginLinks()
        Dim LoginLink As String = TempTable.Select("ID = " & m_User.Customer.LoginLinkID)(0)("Text")

        m_User.SetLoggedOn(m_User.UserName, False)

        If (LoginLink.Contains("://")) Then
            Dim loginLink2 As String = LoginLink.Substring(LoginLink.IndexOf("://", 0, StringComparison.InvariantCultureIgnoreCase) + 3)
            If (loginLink2.Contains("/")) Then
                LoginLink = loginLink2.Substring(loginLink2.IndexOf("/", 0, StringComparison.InvariantCultureIgnoreCase))
            End If
        End If

        Dim qs As String = Request.QueryString.ToString()

        Dim EncrUserLink As String = EncrData(userName & "|" & password)
        EncrUserLink = Replace(EncrUserLink, "+", "-")
        EncrUserLink = Replace(EncrUserLink, "/", "_")
        Dim redirectLink As String = String.Format("{0}?ReUser={1}", LoginLink, EncrUserLink)
        Response.Redirect(redirectLink, True)

    End Sub

    Private Function GenerateRandomCode() As String
        Return String.Format("{0:00}", random.Next(20))
    End Function

    Private Function GetCaptchaURL(imagekey As String) As String
        Dim captchaPath = HttpContext.Current.Server.MapPath("/Services/Images/Captcha/")

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

        'Dim relFile = localFile.Replace(appRoot, "~/")
        Dim relFile = Replace(localFile, appRoot, "~/", , , CompareMethod.Text)
        relFile = relFile.Replace("\"c, "/"c)

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

    Protected Sub lbtnHelpCenter_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lbtnHelpCenter.Click
        MessageLabel.Text = ""
        Session("LostPassword") = 0
        lblRedStar.Visible = False
        tblHelp.Visible = Not tblHelp.Visible
        If tblHelp.Visible = True Then
            trProblem.Visible = True
            GenerateCaptcha()
        End If
    End Sub

    Protected Sub ibtnRefresh_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtnRefresh.Click
        Session("CaptchaGen1") = GenerateRandomCode()
        Session("CaptchaGen2") = GenerateRandomCode()
        Me.MessageLabel.Text = ""
        Me.CodeNumberTextBox.Text = ""
        GenerateCaptcha()
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

    Protected Sub cmdSend_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdSend.Click
        Dim num1 As Integer = 0
        Dim num2 As Integer = 0
        MessageLabel.Text = ""
        If Not validateHelpData() AndAlso
            Int32.TryParse(Me.Session("CaptchaGen1"), num1) AndAlso
            Int32.TryParse(Me.Session("CaptchaGen2"), num2) Then

            If Me.CodeNumberTextBox.Text.Trim = (num1 + num2).ToString() Then
                GenerateMailBody()
                Session("CaptchaGen1") = GenerateRandomCode()
                Session("CaptchaGen2") = GenerateRandomCode()
                Me.MessageLabel.Text = "E-Mail wurde versandt!"
                Me.CodeNumberTextBox.Text = ""
                ddlAnrede.SelectedValue = "-"
                txtName.Text = ""
                txtVorname.Text = ""
                txtTelefon.Text = ""
                txtEmail.Text = ""
                txtFirma.Text = ""
                txtProblem.Text = ""
                tblHelp.Visible = False
            Else
                ' Display an error message.
                Me.MessageLabel.Text = "Versuchen Sie es normal oder generieren Sie neu."
                Me.Session("CaptchaGen1") = GenerateRandomCode()
                Me.Session("CaptchaGen2") = GenerateRandomCode()
                GenerateCaptcha()
            End If
        End If
    End Sub

    Private Function EncrData(ByVal TextToEncrypt As String) As String

        Dim EncText As String
        Dim rd As New RijndaelManaged

        Dim md5 As New MD5CryptoServiceProvider
        Dim key() As Byte = md5.ComputeHash(Encoding.UTF8.GetBytes("@S33wolf"))

        md5.Clear()
        rd.Key = key
        rd.GenerateIV()

        Dim iv() As Byte = rd.IV
        Dim ms As New MemoryStream

        ms.Write(iv, 0, iv.Length)

        Dim cs As New CryptoStream(ms, rd.CreateEncryptor, CryptoStreamMode.Write)
        Dim data() As Byte = System.Text.Encoding.UTF8.GetBytes(TextToEncrypt)

        cs.Write(data, 0, data.Length)
        cs.FlushFinalBlock()

        Dim encdata() As Byte = ms.ToArray()
        EncText = Convert.ToBase64String(encdata)
        cs.Close()
        rd.Clear()

        Return EncText
    End Function

    Private Function DbGetStringValue(sql As String) As String
        Dim result As Object
        Dim conn As New SqlClient.SqlConnection(ConfigurationManager.AppSettings("Connectionstring"))
        Dim command As SqlClient.SqlCommand
        Dim strReturn As String = ""

        Try
            conn.Open()

            command = New SqlClient.SqlCommand(sql, conn)

            result = command.ExecuteScalar

            strReturn = CStr(result)

        Catch ex As Exception
            strReturn = ""
        Finally
            conn.Close()
            conn.Dispose()
        End Try
        Return strReturn
    End Function

    Private Function DbGetIntValue(sql As String) As String

        Dim iValue As Integer
        Dim sValue As String = DbGetStringValue(sql)

        If (String.IsNullOrEmpty(sValue)) Then
            Return -1
        End If

        If (sValue.ToLower().Contains("false") Or sValue.ToLower().Contains("true")) Then
            Return IIf(sValue.ToLower() = "true", 1, 0)
        End If

        Try
            iValue = Int32.Parse(sValue)
        Catch ex As Exception
            iValue = -1
        End Try

        Return iValue

    End Function

    Public Sub RedirectFromLoginPage(webUser As User)
        If (webUser.Customer.MvcLayoutAsWebFormsInline Or Not webUser.Customer.HasMvcApplicationsOnly Or webUser.HighestAdminLevel > AdminLevel.None) Then
            FormsAuthentication.RedirectFromLoginPage(webUser.UserID.ToString, False)
        Else
            Dim servicesMvsRootUrl As String = ErpBaseMvc.MVC.MvcPrepareUrl("", "", webUser.UserName)
            Response.Redirect(servicesMvsRootUrl)
        End If
    End Sub

    Private Sub SetzeHinweisPflichtfeld()
        MessageLabel.Text = "Bitte Pflichtfelder ausfüllen!"
    End Sub

End Class