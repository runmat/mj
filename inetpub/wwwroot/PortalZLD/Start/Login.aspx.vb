Imports CKG.Base.Kernel
Imports CKG.Base.Kernel.Common.Common
Imports CKG.Base
Imports System.Drawing.Imaging
Imports CKG.Base.Business
Imports CKG
Imports System.Data.SqlClient
Imports System.Net
Imports System.IO

Partial Public Class Login
    Inherits System.Web.UI.Page
    Private m_User As New Base.Kernel.Security.User()
    Private m_App As Base.Kernel.Security.App
    Private cke As Integer
    Private ckp As Integer
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

            'lblError.Text = "Systemfehler! Anmeldedaten konnten nicht geprüft werden."
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
            If Not Me.Session("objUser") Is Nothing AndAlso _
                Me.User.Identity.IsAuthenticated = False AndAlso blnPasswdlink = False Then
                '---JVE: User nicht mehr in der Session gespeichert bzw. nicht Authentifiziert---
                Response.Redirect(BouncePage(Me), True)
                Exit Sub
            End If
            lnkPasswortVergessen.Text = "Passwort vergessen?"
            trPasswortVergessen.Visible = False
            If m_User.Login(txtUsername.Text, txtPassword.Text, Session.SessionID.ToString, blnPasswdlink) Then
                'If m_User.Login(txtUsername.Text, Session.SessionID.ToString) Then

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
                    Me.DoubleLogin2.Visible = True
                Else
                    'Application kann erst an dieser Stelle gesetzt werden, weil erst hier der User (ggf. mit Testflag!!!) feststeht
                    m_App = New Base.Kernel.Security.App(m_User)
                    If Session("AppId") Is Nothing Then
                        Session("AppId") = "0"
                    End If

                    'Prüfen, ob zu bewertende KVPs vorliegen
                    checkForKVP()

                    'ggf. Kst.-Auswahl für ZLD-Leiter 
                    If m_User.IsLeiterZLD Then
                        Dim lzldDaten As New UserdatenSAP(m_User, m_App)
                        Dim tblKostenstellen As DataTable = lzldDaten.getKostenstellenLZLD(0, Session.SessionID.ToString)

                        If tblKostenstellen.Rows.Count > 1 OrElse (tblKostenstellen.Rows.Count > 0 AndAlso Not tblKostenstellen.Rows(0)("VKBUR").ToString() = m_User.Kostenstelle) Then
                            divKstAuswahlLZLD.Visible = True
                            FillKostenstellen(tblKostenstellen)
                        Else
                            System.Web.Security.FormsAuthentication.RedirectFromLoginPage(m_User.UserID.ToString, False)
                        End If
                    Else
                        System.Web.Security.FormsAuthentication.RedirectFromLoginPage(m_User.UserID.ToString, False)
                    End If

                    'zur späteren Benutzung (iframe)
                    'FormsAuthentication.SetAuthCookie(m_User.UserID.ToString, False)
                    'Response.Write("<script language='javascript'>")
                    'Response.Write("window.open('Selection.aspx' ,'Zoic','width=600, height=400,toobar=yes,addressbar=yes,menubar=yes,scrollbars=yes,resizable=yes');")
                    'Response.Write("window.location.href ='Login.aspx';")
                    'Response.Write("<" + "/" + "script" + ">")

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
                        If m_User.AccountIsLockedOut AndAlso m_User.AccountIsLockedBy = "User" Then ' Gleich weiter zur Ensperrung!
                            If m_User.Email.Length > 0 And m_User.Customer.ForcePasswordQuestion And m_User.QuestionID > -1 Then
                                System.Web.Security.FormsAuthentication.RedirectFromLoginPage(m_User.UserID.ToString, False)
                            Else
                                lblError.Text = "Fehler bei der Anmeldung<br>(" & m_User.ErrorMessage & ")"
                                trPasswortVergessen.Visible = True
                                trHelpCenter.Visible = False
                                lnkPasswortVergessen_Click(sender, e)
                            End If
                        ElseIf m_User.AccountIsLockedOut AndAlso m_User.AccountIsLockedBy = "Now" Then ' gerade gesperrt? Sperrung anzeigen!
                            lblError.Text = "Fehler bei der Anmeldung<br>(" & m_User.ErrorMessage & ")"
                            trPasswortVergessen.Visible = True
                            trHelpCenter.Visible = False
                            lnkPasswortVergessen.Text = "Entsperren"
                        Else
                            lblError.Text = "Fehler bei der Anmeldung<br>(" & m_User.ErrorMessage & ")"
                            trPasswortVergessen.Visible = False
                            trHelpCenter.Visible = True

                        End If

                    End If
                Else
                    lblError.Text = "Fehler bei der Anmeldung."
                    trPasswortVergessen.Visible = True
                End If
            End If
        Catch ex As Exception
            m_App = New Base.Kernel.Security.App(m_User)
            m_App.WriteErrorText(1, txtUsername.Text, "Login", "cmdLogin_Click", ex.ToString)

            lblError.Text = "Fehler bei der Anmeldung (" & ex.Message & ")"
        End Try
    End Sub

    Private Sub checkForKVP()
        Try
            Dim objKVP As New KVP(m_User, m_App, "", Session.SessionID, "")
            objKVP.KVPLogin(m_User.Kostenstelle, m_User.UserName, m_User.LastName + ", " + m_User.FirstName, "", Session.SessionID, Me.Page)
            If Not objKVP.HasError Then
                If objKVP.ZuBewertendeKVPs > 0 Then
                    Session("ZuBewertendeKVPs") = objKVP.ZuBewertendeKVPs
                End If
            Else
                Session("ZuBewertendeKVPs") = "Ob zu bewertende KVPs vorliegen, konnte nicht ermittelt werden (Grund: " & objKVP.Message
            End If
        Catch ex As Exception
            Session("ZuBewertendeKVPs") = "Ob zu bewertende KVPs vorliegen, konnte nicht ermittelt werden (Grund: " & ex.Message
        End Try
       
    End Sub

    Private Sub FillKostenstellen(ByVal tbl As DataTable)
        Dim vkbur As String
        Dim ktext As String
        Dim vertretung As String
        ddlKostenstelleLZLD.Items.Clear()
        For Each row As DataRow In tbl.Rows
            vkbur = row("VKBUR").ToString()
            ktext = row("KTEXT").ToString()
            vertretung = row("VERTRETUNG").ToString()

            If vertretung = "X" Then
                ddlKostenstelleLZLD.Items.Add(New ListItem(row("VKBUR").ToString() & " ~ " & row("KTEXT").ToString() & " (Vertretung)", row("VKBUR").ToString()))
            Else
                ddlKostenstelleLZLD.Items.Add(New ListItem(row("VKBUR").ToString() & " ~ " & row("KTEXT").ToString(), row("VKBUR").ToString()))
            End If
        Next
    End Sub

    ''' <summary>
    ''' Vom LZLD ausgewählte Kst. übernehmen und ihn zur Anwendung weiterleiten
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub lbtnChooseKst_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lbtnChooseKst.Click
        If Not ddlKostenstelleLZLD.SelectedValue = "0" Then
            m_User.ChangeUserReference(m_User.Buchungskreis & ddlKostenstelleLZLD.SelectedValue.PadLeft(4, "0"c))
            System.Web.Security.FormsAuthentication.RedirectFromLoginPage(m_User.UserID.ToString, False)
        End If
    End Sub

    Private Sub cmdBack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdBack.Click
        Response.Redirect(BouncePage(Me), True)
    End Sub

    Private Sub cmdContinue_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdContinue.Click
        m_User.DoubleLoginTry = False
        m_User.SetLoggedOn(m_User.UserName, True, Session.SessionID.ToString)
        m_User.SessionID = Session.SessionID.ToString
        Session("objUser") = m_User
        System.Web.Security.FormsAuthentication.RedirectFromLoginPage(m_User.UserID.ToString, False)
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
            System.Web.Security.FormsAuthentication.RedirectFromLoginPage(m_User.UserID.ToString, False)
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


        If Not Me.Session("objUser") Is Nothing Then
            m_User = CType(Session("objUser"), Base.Kernel.Security.User)
            If Not (m_User.LoggedOn And m_User.DoubleLoginTry) Then
                If Me.User.Identity.IsAuthenticated = False Then
                    If trPasswortVergessen.Visible = False And Not divKstAuswahlLZLD.Visible Then
                        Response.Redirect(BouncePage(Me), True)

                    End If
                End If
            End If
        End If

        litAlert.Text = ""
        txtUsername.Focus()
        Page.Title = "Anmeldung"
        If Not IsPostBack Then
            If Not CheckUniqueSessionID() Then
                Response.Redirect(BouncePage(Me), True)

            End If
            displayMessages()
            Me.StandardLogin.Visible = True
            Me.DoubleLogin2.Visible = False
            Session("CaptchaGen1") = GenerateRandomCode()
            Session("CaptchaGen2") = GenerateRandomCode()
            'Prüfe zugreifenden Key/IP

            Dim sSSORemoteHost As String = ConfigurationManager.AppSettings("SSORemoteHost").ToString
            If (Not Request.QueryString("key") Is Nothing) Then

                Dim sKey As String = Request.QueryString("key").ToString()

                Dim clsCrypt As New Base.Kernel.Security.CryptNew()

                Dim DeCryptedKey As String = clsCrypt.psDecrypt(sKey)
                Dim strDomainError As String = "Die Anmeldung ist in der aktuellen Konfiguration nicht möglich.<br>Setzen Sie sich bitte mit Ihrer Kontaktperson bei der Christoph Kroschke GmbH in Verbindung."

                Dim strUserName As String = GetDomainUser(DeCryptedKey.ToUpper)
                If m_User.Login(strUserName, Session.SessionID.ToString) Then
                    System.Web.Security.FormsAuthentication.RedirectFromLoginPage(m_User.UserID.ToString, False)
                Else
                    lblError.Text = strDomainError & "<br>(" & m_User.ErrorMessage & ")"
                    cmdLogin.Enabled = False
                End If
                Exit Sub
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
                        If m_User.Login(strIpStandardUser, Session.SessionID.ToString) Then
                            System.Web.Security.FormsAuthentication.RedirectFromLoginPage(m_User.UserID.ToString, False)
                        Else
                            lblError.Text = strIpError & "<br>(" & m_User.ErrorMessage & ")"
                            cmdLogin.Enabled = False
                        End If
                    End If
                End If

            End If

        End If
    End Sub

    Private Function GenerateRandomCode() As String
        Dim s As String = ""
        For i As Integer = 0 To 1
            If i = 0 Then
                s = [String].Concat(s, Me.random.[Next](2).ToString())
            Else
                s = [String].Concat(s, Me.random.[Next](10).ToString())
            End If
        Next
        Return s
    End Function

    Private Sub GenerateCaptcha()
        If Session("CaptchaGen1") IsNot Nothing Then
            Dim imagekey As String = Session("CaptchaGen1").ToString
            '// Create a CAPTCHA image using the text stored in the Session object.
            Dim ci As CaptchaImage = New CaptchaImage(imagekey, 80, 30, "Century Schoolbook")

            ci.Image.Save("C:\inetpub\wwwroot\PortalZLD\temp\excel\" & imagekey & ".jpg", ImageFormat.Jpeg)

            '// Dispose of the CAPTCHA image object.
            ci.Dispose()

            imgCatcha1.Src = "..\temp\excel\" & imagekey & ".jpg"
        End If
        If Session("CaptchaGen2") IsNot Nothing Then
            Dim imagekey As String = Session("CaptchaGen2").ToString
            '// Create a CAPTCHA image using the text stored in the Session object.
            Dim ci As CaptchaImage = New CaptchaImage(imagekey, 80, 30, "Century Schoolbook")

            ci.Image.Save("C:\inetpub\wwwroot\PortalZLD\temp\excel\" & imagekey & ".jpg", ImageFormat.Jpeg)

            '// Dispose of the CAPTCHA image object.
            ci.Dispose()

            imgCatcha2.Src = "..\temp\excel\" & imagekey & ".jpg"
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
            MessageLabel.Text = "Bitte Plichfelder ausfüllen!"
            ddlAnrede.BorderColor = System.Drawing.ColorTranslator.FromHtml("#C40000")
            breturn = True
        End If
        If txtName.Text.Trim.Length = 0 Then
            MessageLabel.Text = "Bitte Plichfelder ausfüllen!"
            txtName.BorderColor = System.Drawing.ColorTranslator.FromHtml("#C40000")
            breturn = True
        End If
        If txtVorname.Text.Trim.Length = 0 Then
            MessageLabel.Text = "Bitte Plichfelder ausfüllen!"
            txtVorname.BorderColor = System.Drawing.ColorTranslator.FromHtml("#C40000")
            breturn = True
        End If
        If txtFirma.Text.Trim.Length = 0 Then
            MessageLabel.Text = "Bitte Plichfelder ausfüllen!"
            txtFirma.BorderColor = System.Drawing.ColorTranslator.FromHtml("#C40000")
            breturn = True
        End If
        If txtTelefon.Text.Trim.Length = 0 Then
            MessageLabel.Text = "Bitte Plichfelder ausfüllen!"
            txtTelefon.BorderColor = System.Drawing.ColorTranslator.FromHtml("#C40000")
            breturn = True
        End If
        If txtEmail.Text.Trim.Length = 0 Then
            MessageLabel.Text = "Bitte Plichfelder ausfüllen!"

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
                MessageLabel.Text = "Bitte Plichfelder ausfüllen!"
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
            Dim smtpMailSender As New System.Net.Mail.MailAddress(ConfigurationManager.AppSettings("SmtpMailSender"))
            Dim smtpMailServer As String = ""
            Dim MailAdresses As String = ""


            ' Mail = New System.Net.Mail.MailMessage(smtpMailSender, "Web-Administrator@dad.de", "Helpdeskanfrage der Login-Seite", message)
            Mail = New System.Net.Mail.MailMessage()
            Mail.Body = message
            Mail.From = smtpMailSender

            LeseMailTexte("1", "", MailAdresses, "", "")

            If MailAdresses.Trim().Split(";").Length > 0 Then

                Dim Adressen As String() = MailAdresses.Trim().Split(";")
                Dim tmpStr As String
                For Each tmpStr In Adressen
                    Mail.To.Add(tmpStr)
                Next
            Else
                MessageLabel.Text = "Fehler beim Versenden der E-Mail."
                Exit Sub
            End If
            Mail.Subject = "Helpdeskanfrage der Login-Seite ZLD-Portal"

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
                Me.MessageLabel.Text = "Versuchen Sie es normal oder generieren Sie neu."
                Me.Session("CaptchaGen1") = GenerateRandomCode()
                Me.Session("CaptchaGen2") = GenerateRandomCode()
                GenerateCaptcha()

            End If
        End If
    End Sub

    Public Sub LeseMailTexte(ByVal InputVorgang As String, ByRef MailAdressCC As String, ByRef MailAdress As String,
                             ByRef MailBody As String, ByRef Betreff As String)

        Dim strTempVorgang As String = InputVorgang

        MailAdressCC = ""
        MailAdress = ""
        Dim Mailings = New DataTable()
        Dim connection As New System.Data.SqlClient.SqlConnection()
        connection.ConnectionString = ConfigurationManager.AppSettings("Connectionstring").ToString()
        Try

            Dim adapter As New SqlDataAdapter("SELECT * FROM vwGetMailTexte WHERE KundenID=@KundenID " & "AND Vorgangsnummer Like @Vorgangsnummer " & "AND Aktiv=1", connection)


            adapter.SelectCommand.Parameters.AddWithValue("@KundenID", "1")
            adapter.SelectCommand.Parameters.AddWithValue("@Vorgangsnummer", strTempVorgang)

            adapter.Fill(Mailings)

            If Mailings.Rows.Count > 0 Then
                For Each dRow As DataRow In Mailings.Rows
                    MailBody = dRow("Text").ToString()
                    Betreff = dRow("Betreff").ToString()
                    Dim boolCC As [Boolean] = False
                    [Boolean].TryParse(dRow("CC").ToString(), boolCC)
                    If boolCC Then
                        If MailAdressCC = "" Then
                            MailAdressCC = dRow("EmailAdresse").ToString()
                        Else
                            MailAdressCC += ";" & dRow("EmailAdresse").ToString()
                        End If
                    ElseIf MailAdress = "" Then
                        MailAdress = dRow("EmailAdresse").ToString()
                    Else
                        MailAdress += ";" & dRow("EmailAdresse").ToString()
                    End If


                Next
            Else
                lblError.Text = "Keine Mailvorlagen für diesen Kunden."

            End If
        Catch ex As Exception
            lblError.Text = "Fehler beim Laden der Mailadressen: " + ex.Message
        Finally

            connection.Close()
        End Try


    End Sub

    Private Function GetDomainUser(ByVal strDomainUser As String) As String
        Dim Connection As New SqlClient.SqlConnection()
        Dim Command As New SqlClient.SqlCommand()
        Dim Applications As ArrayList = New ArrayList()

        Try
            Connection.ConnectionString = ConfigurationManager.AppSettings("Connectionstring")

            With Command
                .Connection = Connection
                .CommandType = CommandType.Text

                .CommandText = "SELECT UserName" & _
                                " FROM DomainUser" & _
                                " Where DomainName = '" & strDomainUser & "'"
            End With

            Dim DomainUser As String = ""
            Connection.Open()
            DomainUser = Command.ExecuteScalar()
            Connection.Close()
            Return DomainUser

        Catch ex As Exception
            If Connection.State = ConnectionState.Open Then
                Connection.Close()
            End If
            Return ""
            lblError.Text = "Beim Laden der Anwendungen ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
        End Try

    End Function

End Class