Imports CKG.Base.Kernel
Imports CKG.Base.Kernel.Common.Common

Partial Public Class Helpdesk02s
    Inherits System.Web.UI.Page
    Private m_App As Security.App
    Private m_User As Security.User

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        m_User = GetUser(Me) ' füllen Form.Session("objUser"), rückgabe eines UserObjekte
        FormAuth(Me, m_User)
        m_App = New Base.Kernel.Security.App(m_User) 'erzeugt ein App_objekt 
        GetAppIDFromQueryString(Me) ' füllen page.Session("AppID")
        lblHead.Text = m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString


        If Not IsPostBack Then
            Dim logApp As New Logging.Trace(m_App.Connectionstring, m_App.SaveLogAccessSAP, m_App.LogLevel)
            logApp.InitEntry(m_User.UserName, Session.SessionID, CInt(Session("AppID")), m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString, m_User.CustomerName, m_User.Customer.CustomerId, m_User.IsTestUser, 0)
            logApp.UpdateEntry("APP", Session("AppID").ToString, "Report: Optisches Archiv")
            loadForm()
        Else
            lblError.Text = String.Empty
        End If

    End Sub

    Private Sub loadForm()
        txtOrganization.Visible = False
        txtGroup.Visible = False
        ddlUsers.Visible = False
        'btnShow.Visible = False

        refreshOrgs()
        refreshGroups()
        refreshUsers()

        disableInput(False)
    End Sub

    Private Sub refreshOrgs()
        'Organisationen
        Dim orgs As DataTable
        Dim status As String = ""

        orgs = loadOrgs(status)

        If (status = String.Empty) Then
            With ddlOrganization
                .DataSource = orgs
                .DataTextField = "OrganizationName"
                .DataValueField = "OrganizationID"
                .DataBind()
            End With
        Else
            lblError.Text = "Fehler beim Laden der Seite."
        End If
    End Sub

    Private Sub refreshGroups()
        Dim groups As DataTable
        Dim status As String = ""

        'Gruppen
        groups = loadGroups(status, CInt(ddlOrganization.SelectedItem.Value))

        If (status = String.Empty) Then
            With ddlGroups
                .DataSource = groups
                .DataTextField = "GroupName"
                .DataValueField = "GroupID"
                .DataBind()
            End With
            'refreshUsers()
        Else
            lblError.Text = "Fehler beim Laden der Seite."
        End If
    End Sub

    Private Sub refreshUsers()
        Dim users As DataTable
        Dim status As String = ""
        'Benutzer
        users = loadUsers(status, CInt(ddlOrganization.SelectedItem.Value), CInt(ddlGroups.SelectedItem.Value))

        If (status = String.Empty) Then
            With ddlUsers
                .DataSource = users
                .DataTextField = "Username"
                .DataValueField = "UserID"
                .DataBind()
            End With
            'If (users.Rows.Count > 0) Then
            '    showUser()
            'End If
        Else
            lblError.Text = "Fehler beim Laden der Seite."
        End If
    End Sub

    Private Function loadOrgs(ByRef status As String) As DataTable
        Dim conn As New SqlClient.SqlConnection()
        Dim command As New SqlClient.SqlCommand()
        Dim adapter As SqlClient.SqlDataAdapter
        Dim result As New DataTable()

        status = String.Empty
        conn.ConnectionString = ConfigurationManager.AppSettings("Connectionstring")

        command.Connection = conn
        command.CommandType = CommandType.Text
        'Alle Organisationen holen...
        command.CommandText = "SELECT * FROM vwOrganization WHERE CustomerID = @Customer ORDER BY OrganizationName ASC"
        command.Parameters.AddWithValue("@Customer", m_User.Customer.CustomerId)

        adapter = New SqlClient.SqlDataAdapter(command)
        Try
            conn.Open()
            adapter.Fill(result)
            'tableRefresh(result)
            Return result
        Catch ex As Exception
            status = ex.Message
            Return Nothing
        Finally
            conn.Close()
            conn.Dispose()
        End Try
    End Function

    Private Function loadGroups(ByRef status As String, ByVal organization As Integer) As DataTable
        Dim conn As New SqlClient.SqlConnection()
        Dim command As New SqlClient.SqlCommand()
        Dim adapter As SqlClient.SqlDataAdapter
        Dim result As New DataTable()

        status = String.Empty
        conn.ConnectionString = ConfigurationManager.AppSettings("Connectionstring")

        command.Connection = conn
        command.CommandType = CommandType.Text
        If (organization = 0) Then
            'Alle Anwendungen holen...
            command.CommandText = "SELECT DISTINCT GroupName,GroupID,OrganizationID,CustomerID FROM vwWebUserWebMember WHERE CustomerID = @Customer ORDER BY GroupName ASC"
        Else
            'Anwendujngen für eine bestimmte Gruppe holen...
            command.CommandText = "SELECT DISTINCT GroupName,GroupID,OrganizationID,CustomerID FROM vwWebUserWebMember WHERE CustomerID = @Customer AND OrganizationID = @OrganizationID ORDER BY GroupName ASC"
            command.Parameters.AddWithValue("@OrganizationID", organization)
        End If

        command.Parameters.AddWithValue("@Customer", m_User.Customer.CustomerId)

        adapter = New SqlClient.SqlDataAdapter(command)
        Try
            conn.Open()
            adapter.Fill(result)
            'tableRefresh(result)
            Return result
        Catch ex As Exception
            status = ex.Message
            Return Nothing
        Finally
            conn.Close()
            conn.Dispose()
        End Try
    End Function

    Private Function loadUsers(ByRef status As String, ByVal organization As Integer, ByVal group As Integer) As DataTable
        Dim conn As New SqlClient.SqlConnection()
        Dim command As New SqlClient.SqlCommand()
        Dim adapter As SqlClient.SqlDataAdapter
        Dim result As New DataTable()

        status = String.Empty
        conn.ConnectionString = ConfigurationManager.AppSettings("Connectionstring")

        command.Connection = conn
        command.CommandType = CommandType.Text
        If (group = 0) Then
            'Alle User der Organisazion holen...
            command.CommandText = "SELECT Username,UserID FROM vwWebUserWebMember WHERE CustomerID = @Customer AND OrganizationID = @OrganizationID ORDER BY Username ASC"
        Else
            'Anwendujngen für eine bestimmte Gruppe holen...
            command.CommandText = "SELECT Username,UserID FROM vwWebUserWebMember WHERE CustomerID = @Customer AND OrganizationID = @OrganizationID AND GroupID = @GroupID ORDER BY Username ASC"
            command.Parameters.AddWithValue("@OrganizationID", organization)
            command.Parameters.AddWithValue("@GroupID", group)
        End If

        command.Parameters.AddWithValue("@Customer", m_User.Customer.CustomerId)

        adapter = New SqlClient.SqlDataAdapter(command)
        Try
            conn.Open()
            adapter.Fill(result)
            'tableRefresh(result)
            Return result
        Catch ex As Exception
            status = ex.Message
            Return Nothing
        Finally
            conn.Close()
            conn.Dispose()
        End Try
    End Function

    Private Function loadUser(ByRef status As String, ByVal userID As Integer) As DataTable
        Dim conn As New SqlClient.SqlConnection()
        Dim command As New SqlClient.SqlCommand()
        Dim adapter As SqlClient.SqlDataAdapter
        Dim result As New DataTable()

        status = String.Empty
        conn.ConnectionString = ConfigurationManager.AppSettings("Connectionstring")

        command.Connection = conn
        command.CommandType = CommandType.Text
        command.CommandText = "SELECT * from WebUser WHERE UserID = @UserID"

        command.Parameters.AddWithValue("@UserID", userID)

        adapter = New SqlClient.SqlDataAdapter(command)
        Try
            conn.Open()
            adapter.Fill(result)
            'tableRefresh(result)
            Return result
        Catch ex As Exception
            status = ex.Message
            Return Nothing
        Finally
            conn.Close()
            conn.Dispose()
        End Try
    End Function

    Private Function loadUserInfo(ByRef status As String, ByVal userID As Integer) As DataTable
        Dim conn As New SqlClient.SqlConnection()
        Dim command As New SqlClient.SqlCommand()
        Dim adapter As SqlClient.SqlDataAdapter
        Dim result As New DataTable()

        status = String.Empty
        conn.ConnectionString = ConfigurationManager.AppSettings("Connectionstring")

        With command
            .Connection = conn
            .CommandType = CommandType.Text
            .CommandText = "SELECT * from WebUserInfo WHERE id_user = @UserID"
            .Parameters.AddWithValue("@UserID", userID)
        End With

        adapter = New SqlClient.SqlDataAdapter(command)
        Try
            conn.Open()
            adapter.Fill(result)
            'tableRefresh(result)
            Return result
        Catch ex As Exception
            status = ex.Message
            Return Nothing
        Finally
            conn.Close()
            conn.Dispose()
        End Try
    End Function

    Protected Sub lbBack_Click(sender As Object, e As EventArgs) Handles lbBack.Click
        Response.Redirect("../../Start/Selection.aspx?AppID=" & Session("AppID").ToString, False)
    End Sub

    Private Sub btnConfirm_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnConfirm.Click
        Dim status As String = ""
        Dim user As String = ""
        Dim result As Boolean
        Dim selection As Integer
        Dim str As String = ""
        Dim strUser As String = ""

        lblMessage.Visible = False

        selection = CInt(rbVorgang.SelectedItem.Value)
        user = txtUser.Text

        strUser = String.Empty
        'str = "ARVAL Helpdesk-Auftrag" & vbCrLf & "----------------------" & vbCrLf & vbCrLf 
        str = "Helpdesk-Auftrag" & vbCrLf & "----------------------" & vbCrLf & vbCrLf
        str &= "Verfasser      : " & m_User.UserName & ", " & m_User.CustomerName & vbCrLf
        str &= "Vorgang        : " & rbVorgang.SelectedItem.Text.ToUpper & vbCrLf

        str &= vbCrLf & vbCrLf

        str &= "ORGANISATION   : " & ddlOrganization.SelectedItem.Text & vbCrLf
        str &= "GRUPPE         : " & ddlGroups.SelectedItem.Text & vbCrLf
        str &= "BENUTZERNAME   : "
        If txtUser.Visible = True Then
            str &= txtUser.Text
        Else
            str &= ddlUsers.SelectedItem.Text
        End If

        str &= vbCrLf
        'Benutzerdaten
        strUser &= "Anrede         : " & ddlAnrede.SelectedItem.Text & vbCrLf
        strUser &= "NAME           : " & txtName.Text & vbCrLf
        strUser &= "VORNAME        : " & txtVorname.Text & vbCrLf
        strUser &= "TELEFON        : " & txtTelefon.Text & vbCrLf
        strUser &= "EMAILADRESSE   : " & txtEmail.Text & vbCrLf
        strUser &= "REFERENZ       : " & txtReferenz.Text & vbCrLf
        strUser &= "Bemerkung      : " & txtBemerk.Text & vbCrLf

        Select Case selection
            Case 1                 'Neu
                If (status = String.Empty) Then
                    If (result = True) Then
                        lblError.Text = "Dieser Benutzer ist im System bereits vorhanden (Benutzername)."
                    Else
                        str &= vbCrLf & vbCrLf & strUser
                    End If
                Else
                    lblError.Text = "Fehler beim Absenden."
                End If
            Case 2                  'Ändern
                str &= vbCrLf & vbCrLf & strUser
            Case 3                  'Löschen
                str = str
        End Select
        str &= vbCrLf & "AUF DIESE MAIL NICHT ANTWORTEN."
        SendMailToDAD(str)

    End Sub

    Private Sub SendMailToDAD(ByVal message As String)
        Dim clsMail As New Base.Kernel.Common.GetMailTexte(m_App, m_User, Session.SessionID.ToString, Session("AppID").ToString)

        If clsMail.Status = 0 Then
            Try
                Dim Mail As System.Net.Mail.MailMessage
                Dim smtpMailSender As String = ""
                Dim smtpMailServer As String = ""

                smtpMailSender = ConfigurationManager.AppSettings("SmtpMailSender")

                clsMail.LeseMailTexte("HD")

                clsMail.Betreff = "Helpdesk (Benutzerverwaltung) - " & rbVorgang.SelectedItem.Text
                clsMail.MailBody = message
                Dim Adressen() As String
                If Split(clsMail.MailAdress.Trim, ";").Length > 1 Then

                    Mail = New System.Net.Mail.MailMessage()

                    Dim Mailsender As New System.Net.Mail.MailAddress(smtpMailSender)

                    Mail.Sender = Mailsender
                    Mail.From = Mailsender

                    Adressen = Split(clsMail.MailAdress.Trim, ";")
                    For Each tmpStr As String In Adressen
                        Mail.To.Add(tmpStr)
                    Next
                Else
                    Mail = New System.Net.Mail.MailMessage(smtpMailSender, clsMail.MailAdress.Trim, clsMail.Betreff, clsMail.MailBody)
                End If

                If Split(clsMail.MailAdressCC.Trim, ";").Length > 1 Then
                    Adressen = Split(clsMail.MailAdressCC.Trim, ";")
                    For Each tmpStr As String In Adressen
                        Mail.CC.Add(tmpStr)
                    Next
                Else
                    If clsMail.MailAdressCC.Length > 0 Then
                        Mail.CC.Add(clsMail.MailAdressCC)
                    End If
                End If

                Mail.IsBodyHtml = False
                smtpMailServer = ConfigurationManager.AppSettings("SmtpMailServer")
                Dim client As New System.Net.Mail.SmtpClient(smtpMailServer)
                client.Send(Mail)
            Catch ex As Exception
                lblError.Text = "Fehler beim Versenden der E-Mail."
            End Try
            'wieder auf Anfang(Neuanlage)!!
            ddlOrganization.SelectedIndex = 0
            ddlGroups.SelectedIndex = 0
            ddlUsers.SelectedIndex = 0
            trOrganization.Visible = True
            trGruppe.Visible = True
            trBenutzer.Visible = True
            ddlUsers.Visible = False
            txtUser.Visible = True
            txtUser.Text = String.Empty
            ddlAnrede.SelectedIndex = 0
            rbVorgang.SelectedIndex = 0
            disableInput(False)
            clearInput()
            lblMessage.Visible = True
        End If
    End Sub

    Private Sub disableInput(ByVal status As Boolean)
        trEmail.Visible = Not status
        trName.Visible = Not status
        trVorName.Visible = Not status
        trReferenz.Visible = Not status
        trTelefon.Visible = Not status
        trBemerkung.Visible = Not status
        trAnrede.Visible = Not status
        trLegende.Visible = Not status
    End Sub

    Private Sub clearInput()
        txtEmail.Text = String.Empty
        txtName.Text = String.Empty
        txtVorname.Text = String.Empty
        txtReferenz.Text = String.Empty
        txtTelefon.Text = String.Empty
        txtBemerk.Text = String.Empty
    End Sub

    Private Sub rbVorgang_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rbVorgang.SelectedIndexChanged
        Dim selection As Integer
        lblMessage.Visible = False
        selection = CInt(rbVorgang.SelectedItem.Value)

        Select Case selection
            Case 1  'Neuanlage
                trOrganization.Visible = True
                trGruppe.Visible = True
                trBenutzer.Visible = True
                ddlUsers.Visible = False
                txtUser.Visible = True
                txtUser.Enabled = False
                txtUser.Text = String.Empty
                disableInput(False)
                clearInput()
            Case 2  'Änderung
                trOrganization.Visible = True
                trGruppe.Visible = True
                trBenutzer.Visible = True
                ddlUsers.Visible = True
                txtUser.Visible = False
                showUser()
                'disableInput(True)
            Case 3, 4  'Löschung
                trOrganization.Visible = True
                trGruppe.Visible = True
                trBenutzer.Visible = True
                ddlUsers.Visible = True
                txtUser.Visible = False
                disableInput(True)
        End Select
    End Sub

    Private Sub ddlGroups_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ddlGroups.SelectedIndexChanged
        Dim selection As Integer
        lblMessage.Visible = False
        'Nur bei Änderung oder Löschung neu laden
        selection = CInt(rbVorgang.SelectedItem.Value)

        Select Case selection
            Case 1, 2               'Neuanlage oder Änderung
                refreshUsers()
                disableInput(False) 'Felder einblenden
            Case 3, 4                  'Löschung
                refreshUsers()
                disableInput(True) 'Felder ausblenden
        End Select
    End Sub

    Private Sub ddlOrganization_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ddlOrganization.SelectedIndexChanged
        'Nur bei Änderung oder Löschung neu laden
        lblMessage.Visible = False
        If CInt(rbVorgang.SelectedItem.Value) > 1 Then
            refreshGroups()
            refreshUsers()
        End If
    End Sub

    Private Sub ddlUsers_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ddlUsers.SelectedIndexChanged
        lblMessage.Visible = False
        showUser()
    End Sub

    Private Sub showUser()
        Dim status As String = ""
        Dim table As DataTable
        Dim tableInfo As DataTable
        Dim row As DataRow
        Dim rowInfo As DataRow

        table = loadUser(status, CInt(ddlUsers.SelectedItem.Value))
        tableInfo = loadUserInfo(status, CInt(ddlUsers.SelectedItem.Value))
        If (status = String.Empty) Then
            If (CInt(rbVorgang.SelectedItem.Value) <> 3) AndAlso (CInt(rbVorgang.SelectedItem.Value) <> 4) Then
                row = table.Rows(0)
                If (tableInfo.Rows.Count > 0) Then
                    rowInfo = tableInfo.Rows(0)
                    txtEmail.Text = rowInfo("mail").ToString
                Else
                    txtEmail.Text = String.Empty
                End If
                txtUser.Text = row("Username").ToString
                txtReferenz.Text = row("Reference").ToString
                disableInput(False)
            End If
        Else
            lblError.Text = "Fehler beim Laden der Seite."
        End If
    End Sub
    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        SetEndASPXAccess(Me)
    End Sub

    Private Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Unload
        SetEndASPXAccess(Me)
    End Sub
End Class