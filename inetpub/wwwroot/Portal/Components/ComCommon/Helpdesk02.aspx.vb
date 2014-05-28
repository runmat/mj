Imports CKG.Base.Kernel
Imports CKG.Portal.PageElements
Imports CKG.Base.Kernel.Common.Common
Imports System.Net.Mail

Public Class Helpdesk02
    Inherits Page

    Protected WithEvents ucHeader As Header
    Protected WithEvents ucStyles As Styles

    Private m_App As Security.App
    Protected WithEvents Label6 As Label
    Protected WithEvents rbVorgang As RadioButtonList
    Protected WithEvents btnConfirm As LinkButton
    Protected WithEvents Table1 As HtmlTable
    Protected WithEvents lblMessage As Label
    Protected WithEvents lblHead As Label
    Protected WithEvents ddlGroups As DropDownList
    Protected WithEvents lblError As Label
    Protected WithEvents lblGroup As Label
    Protected WithEvents lblUser As Label
    Protected WithEvents lblOrganization As Label
    Protected WithEvents ddlOrganization As DropDownList
    Protected WithEvents trOrganization As HtmlTableRow
    Protected WithEvents txtUser As TextBox
    Protected WithEvents ddlUsers As DropDownList
    Protected WithEvents txtVorname As TextBox
    Protected WithEvents txtEmail As TextBox
    Protected WithEvents txtReferenz As TextBox
    Protected WithEvents trName As HtmlTableRow
    Protected WithEvents trVorName As HtmlTableRow
    Protected WithEvents trEmail As HtmlTableRow
    Protected WithEvents trReferenz As HtmlTableRow
    Protected WithEvents trGruppe As HtmlTableRow
    Protected WithEvents trBenutzer As HtmlTableRow
    Protected WithEvents txtName As TextBox
    Protected WithEvents trLegende As HtmlTableRow
    Protected WithEvents ddlAnrede As DropDownList
    Protected WithEvents txtTelefon As TextBox
    Protected WithEvents txtBemerk As TextBox
    Protected WithEvents trTelefon As HtmlTableRow
    Protected WithEvents trBemerkung As HtmlTableRow
    Protected WithEvents trAnrede As HtmlTableRow

    Private m_User As Security.User

#Region " Vom Web Form Designer generierter Code "

    'Dieser Aufruf ist für den Web Form-Designer erforderlich.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: Diese Methode ist für den Web Form-Designer erforderlich
        'Verwenden Sie nicht den Code-Editor zur Bearbeitung.
        InitializeComponent()
    End Sub

#End Region

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As EventArgs) Handles MyBase.Load
        ' Hier Benutzercode zur Seiteninitialisierung einfügen
        Session.Add("AppID", Request.QueryString("AppID"))

        m_User = GetUser(Me)
        ucHeader.InitUser(m_User)
        FormAuth(Me, m_User)
        lblHead.Text = m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString
        ucStyles.TitleText = lblHead.Text

        lblError.Text = ""

        m_App = New Security.App(m_User)

        If Not IsPostBack Then
            Dim logApp As New Logging.Trace(m_App.Connectionstring, m_App.SaveLogAccessSAP, m_App.LogLevel)
            logApp.InitEntry(m_User.UserName, Session.SessionID, CInt(Session("AppID")), m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString, m_User.CustomerName, m_User.Customer.CustomerId, m_User.IsTestUser, 0)
            logApp.UpdateEntry("APP", Session("AppID").ToString, "Report: Optisches Archiv")
            loadForm()
        End If
    End Sub

    Private Sub loadForm()
        ddlUsers.Visible = False

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
        groups = loadGroups(status, ddlOrganization.SelectedItem.Value)

        If (status = String.Empty) Then
            With ddlGroups
                .DataSource = groups
                .DataTextField = "GroupName"
                .DataValueField = "GroupID"
                .DataBind()
            End With
        Else
            lblError.Text = "Fehler beim Laden der Seite."
        End If
    End Sub

    Private Sub refreshUsers()
        Dim users As DataTable
        Dim status As String = ""
        'Benutzer
        users = loadUsers(status, ddlOrganization.SelectedItem.Value, ddlGroups.SelectedItem.Value)

        If (status = String.Empty) Then
            With ddlUsers
                .DataSource = users
                .DataTextField = "Username"
                .DataValueField = "UserID"
                .DataBind()
            End With
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
            'Anwendungen für eine bestimmte Gruppe holen...
            command.CommandText = "SELECT DISTINCT GroupName,GroupID,OrganizationID,CustomerID FROM vwWebUserWebMember WHERE CustomerID = @Customer AND OrganizationID = @OrganizationID ORDER BY GroupName ASC"
            command.Parameters.AddWithValue("@OrganizationID", organization)
        End If

        command.Parameters.AddWithValue("@Customer", m_User.Customer.CustomerId)

        adapter = New SqlClient.SqlDataAdapter(command)
        Try
            conn.Open()
            adapter.Fill(result)
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
            'Anwendungen für eine bestimmte Gruppe holen...
            command.CommandText = "SELECT Username,UserID FROM vwWebUserWebMember WHERE CustomerID = @Customer AND OrganizationID = @OrganizationID AND GroupID = @GroupID ORDER BY Username ASC"
            command.Parameters.AddWithValue("@OrganizationID", organization)
            command.Parameters.AddWithValue("@GroupID", group)
        End If

        command.Parameters.AddWithValue("@Customer", m_User.Customer.CustomerId)

        adapter = New SqlClient.SqlDataAdapter(command)
        Try
            conn.Open()
            adapter.Fill(result)
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
            Return result
        Catch ex As Exception
            status = ex.Message
            Return Nothing
        Finally
            conn.Close()
            conn.Dispose()
        End Try
    End Function

    Private Sub btnConfirm_Click(ByVal sender As System.Object, ByVal e As EventArgs) Handles btnConfirm.Click
        Dim status As String = ""
        Dim user As String = ""
        Dim result As Boolean
        Dim selection As Integer
        Dim str As String = ""
        Dim strUser As String = ""

        lblMessage.Visible = False
        selection = rbVorgang.SelectedItem.Value
        user = txtUser.Text

        strUser = String.Empty
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
                    lblError.Text = "Fehler beim Absendeen."
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
        Dim clsMail As New Common.GetMailTexte(m_App, m_User, Session.SessionID.ToString, Session("AppID").ToString)

        If clsMail.Status = 0 Then
            Try
                Dim Mail As MailMessage
                Dim smtpMailSender As String = ConfigurationManager.AppSettings("SmtpMailSender")

                clsMail.LeseMailTexte("HD")

                clsMail.Betreff = "Helpdesk (Benutzerverwaltung) - " & rbVorgang.SelectedItem.Text
                clsMail.MailBody = message

                If String.IsNullOrEmpty(clsMail.MailAdress) Then
                    lblError.Text = "Es ist kein Mailempfänger in der Datenbank hinterlegt."
                    Exit Sub
                End If

                Dim Adressen() As String = Split(clsMail.MailAdress.Trim, ";")
                If Adressen.Length > 1 Then

                    Mail = New MailMessage()

                    Dim Mailsender As New MailAddress(smtpMailSender)

                    Mail.Sender = Mailsender
                    Mail.From = Mailsender
                    Mail.Subject = clsMail.Betreff
                    Mail.Body = clsMail.MailBody

                    For Each tmpStr As String In Adressen
                        Mail.To.Add(tmpStr)
                    Next

                Else
                    Mail = New MailMessage(smtpMailSender, clsMail.MailAdress.Trim, clsMail.Betreff, clsMail.MailBody)
                End If

                Adressen = Split(clsMail.MailAdressCC.Trim, ";")

                If Adressen.Length > 1 Then
                    For Each tmpStr As String In Adressen
                        Mail.CC.Add(tmpStr)
                    Next
                Else
                    If clsMail.MailAdressCC.Length > 0 Then
                        Mail.CC.Add(clsMail.MailAdressCC)
                    End If
                End If

                Mail.IsBodyHtml = False
                Dim smtpMailServer As String = ConfigurationManager.AppSettings("SmtpMailServer")
                Dim client As New SmtpClient(smtpMailServer)
                client.Send(Mail)
            Catch ex As Exception
                lblError.Text = "Fehler beim Versenden der E-Mail (" & ex.Message & ")"
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

    Private Sub rbVorgang_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As EventArgs) Handles rbVorgang.SelectedIndexChanged
        Dim selection As Integer
        lblMessage.Visible = False
        selection = rbVorgang.SelectedItem.Value

        Select Case selection
            Case 1  'Neuanlage
                trOrganization.Visible = True
                trGruppe.Visible = True
                trBenutzer.Visible = True
                ddlUsers.Visible = False
                txtUser.Visible = True
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
            Case 3, 4   'Löschung, Sperrung
                trOrganization.Visible = True
                trGruppe.Visible = True
                trBenutzer.Visible = True
                ddlUsers.Visible = True
                txtUser.Visible = False
                disableInput(True)
        End Select
    End Sub

    Private Sub ddlGroups_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As EventArgs) Handles ddlGroups.SelectedIndexChanged
        lblMessage.Visible = False
        Dim selection As Integer
        'Nur bei Änderung oder Löschung neu laden
        selection = rbVorgang.SelectedItem.Value

        Select Case selection
            Case 1, 2               'Neuanlage oder Änderung
                refreshUsers()
                disableInput(False) 'Felder einblenden
            Case 3, 4                  'Löschung
                refreshUsers()
                disableInput(True) 'Felder ausblenden
        End Select
    End Sub

    Private Sub ddlOrganization_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As EventArgs) Handles ddlOrganization.SelectedIndexChanged
        lblMessage.Visible = False
        'Nur bei Änderung oder Löschung neu laden
        If rbVorgang.SelectedItem.Value > 1 Then
            refreshGroups()
            refreshUsers()
        End If
    End Sub

    Private Sub ddlUsers_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As EventArgs) Handles ddlUsers.SelectedIndexChanged
        lblMessage.Visible = False
        showUser()
    End Sub

    Private Sub showUser()
        Dim status As String = ""
        Dim table As DataTable
        Dim tableInfo As DataTable
        Dim row As DataRow
        Dim rowInfo As DataRow

        table = loadUser(status, ddlUsers.SelectedItem.Value)
        tableInfo = loadUserInfo(status, ddlUsers.SelectedItem.Value)
        If (status = String.Empty) Then
            If (rbVorgang.SelectedItem.Value <> 3) AndAlso (rbVorgang.SelectedItem.Value <> 4) Then
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

    Private Sub disableInput(ByVal status As Boolean)
        trEmail.Visible = Not status
        trName.Visible = Not status
        trVorName.Visible = Not status
        trReferenz.Visible = Not status
        trLegende.Visible = Not status
        trTelefon.Visible = Not status
        trBemerkung.Visible = Not status
        trAnrede.Visible = Not status
    End Sub

    Private Sub clearInput()
        txtName.Text = String.Empty
        txtVorname.Text = String.Empty
        txtTelefon.Text = String.Empty
        txtEmail.Text = String.Empty
        txtReferenz.Text = String.Empty
        txtBemerk.Text = String.Empty
    End Sub

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As EventArgs) Handles MyBase.PreRender
        SetEndASPXAccess(Me)
    End Sub

    Private Sub Page_Unload(ByVal sender As Object, ByVal e As EventArgs) Handles MyBase.Unload
        SetEndASPXAccess(Me)
    End Sub

End Class

' ************************************************
' $History: Helpdesk02.aspx.vb $
' 
' *****************  Version 7  *****************
' User: Rudolpho     Date: 9.03.11    Time: 14:57
' Updated in $/CKAG/Components/ComCommon
' 
' *****************  Version 6  *****************
' User: Rudolpho     Date: 17.06.10   Time: 9:54
' Updated in $/CKAG/Components/ComCommon
' 
' *****************  Version 5  *****************
' User: Rudolpho     Date: 16.06.10   Time: 17:04
' Updated in $/CKAG/Components/ComCommon
' 
' *****************  Version 4  *****************
' User: Rudolpho     Date: 16.06.10   Time: 15:33
' Updated in $/CKAG/Components/ComCommon
' 
' *****************  Version 3  *****************
' User: Jungj        Date: 5.06.09    Time: 15:58
' Updated in $/CKAG/Components/ComCommon
' mögliche try catches entfernt
' 
' *****************  Version 2  *****************
' User: Rudolpho     Date: 11.04.08   Time: 13:43
' Updated in $/CKAG/Components/ComCommon
' Migration
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 4.04.08    Time: 13:32
' Created in $/CKAG/Components/ComCommon
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 4.04.08    Time: 10:47
' Created in $/CKAG/portal/Components/ComCommon
' 
' *****************  Version 8  *****************
' User: Rudolpho     Date: 6.12.07    Time: 12:47
' Updated in $/CKG/Components/ComCommon/ComCommonWeb
' ITA:1440
' 
' *****************  Version 7  *****************
' User: Uha          Date: 12.07.07   Time: 9:28
' Updated in $/CKG/Components/ComCommon/ComCommonWeb
' Logging der Laufzeiten der ASPX-Seiten eingeführt
' 
' *****************  Version 6  *****************
' User: Uha          Date: 5.03.07    Time: 14:57
' Updated in $/CKG/Components/ComCommon/ComCommonWeb
' 
' ************************************************
