Imports CKG.Base.Kernel
Imports CKG.Portal.PageElements
Imports CKG.Base.Kernel.Common.Common

Public Class Helpdesk01
    Inherits System.Web.UI.Page

    Protected WithEvents ucStyles As Styles
    Protected WithEvents ucHeader As Header

    Private m_App As Security.App
    Protected WithEvents Label1 As System.Web.UI.WebControls.Label
    Protected WithEvents Label6 As System.Web.UI.WebControls.Label
    Protected WithEvents rbVorgang As System.Web.UI.WebControls.RadioButtonList
    Protected WithEvents txtGroup As System.Web.UI.WebControls.TextBox
    Protected WithEvents btnConfirm As System.Web.UI.WebControls.LinkButton
    Protected WithEvents Table1 As System.Web.UI.HtmlControls.HtmlTable
    Protected WithEvents lblMessage As System.Web.UI.WebControls.Label
    Protected WithEvents ddlGroups As System.Web.UI.WebControls.DropDownList
    Protected WithEvents lblError As System.Web.UI.WebControls.Label
    Protected WithEvents dgApps As System.Web.UI.WebControls.DataGrid
    Protected WithEvents lblHead As System.Web.UI.WebControls.Label
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

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        ' Hier Benutzercode zur Seiteninitialisierung einfügen
        Session.Add("AppID", Request.QueryString("AppID"))

        m_User = GetUser(Me)
        ucHeader.InitUser(m_User)
        FormAuth(Me, m_User)
        lblHead.Text = m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString
        ucStyles.TitleText = lblHead.Text


        m_App = New Security.App(m_User)

        If Not IsPostBack Then
            Dim logApp As New Logging.Trace(m_App.Connectionstring, m_App.SaveLogAccessSAP, m_App.LogLevel)
            logApp.InitEntry(m_User.UserName, Session.SessionID, CInt(Session("AppID")), m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString, m_User.CustomerName, m_User.Customer.CustomerId, m_User.IsTestUser, 0)
            logApp.UpdateEntry("APP", Session("AppID").ToString, "Report: Optisches Archiv")
            loadForm()
        Else
            lblError.Text = String.Empty
            'sendform()
        End If
       
    End Sub

    Private Sub loadForm()

        Dim apps As DataTable
        Dim status As String = ""


        apps = loadApps(status, 0)

        If (status = String.Empty) AndAlso (apps.Rows.Count > 0) Then
            dgApps.DataSource = apps
            dgApps.DataBind()
        Else
            lblError.Text = "Fehler beim Laden der Seite."
        End If
    End Sub

    Private Sub tableRefresh(ByRef table As DataTable)
        Dim row As DataRow

        For Each row In table.Rows
            If row("AppType") = "Change" Then
                row("AppType") = "Dateneingabe"
            End If
        Next
        table.Columns.Add("Zugriff", System.Type.GetType("System.Boolean"))
        table.AcceptChanges()
    End Sub


    Private Function loadAppsUnassigned(ByRef status As String, ByVal group As Integer) As DataTable
        Dim conn As New SqlClient.SqlConnection()
        Dim command As New SqlClient.SqlCommand()
        Dim adapter As SqlClient.SqlDataAdapter
        Dim result As New DataTable()

        status = String.Empty
        conn.ConnectionString = ConfigurationManager.AppSettings("Connectionstring")

        command.Connection = conn
        command.CommandType = CommandType.Text
        If (group = 0) Then
            'Alle Anwendungen holen...
            command.CommandText = "SELECT DISTINCT AppID,AppFriendlyName,AppType,AppInMenu,CustomerID FROM vwGroupAppUnAssigned WHERE CustomerID = @Customer AND AppInMenu = 1 ORDER BY AppType,AppFriendlyName ASC"
        Else
            'Anwendujngen für eine bestimmte Gruppe holen...
            command.CommandText = "SELECT DISTINCT AppID,AppFriendlyName,AppType,AppInMenu,CustomerID FROM vwGroupAppUnAssigned WHERE CustomerID = @Customer AND GroupID = @GroupID AND AppInMenu = 1 ORDER BY AppType,AppFriendlyName ASC"
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
            conn.Dispose()
            Return Nothing
        Finally
            conn.Close()
        End Try
    End Function

    Private Function loadApps(ByRef status As String, ByVal group As Integer) As DataTable
        Dim conn As New SqlClient.SqlConnection()
        Dim command As New SqlClient.SqlCommand()
        Dim adapter As SqlClient.SqlDataAdapter
        Dim result As New DataTable()

        status = String.Empty
        conn.ConnectionString = ConfigurationManager.AppSettings("Connectionstring")

        command.Connection = conn
        command.CommandType = CommandType.Text
        If (group = 0) Then
            'Alle Anwendungen holen...
            command.CommandText = "SELECT DISTINCT AppID,AppFriendlyName,AppType,AppInMenu,CustomerID FROM vwGroupAppAssigned WHERE CustomerID = @Customer AND AppInMenu = 1 ORDER BY AppType,AppFriendlyName ASC"
        Else
            'Anwendujngen für eine bestimmte Gruppe holen...
            command.CommandText = "SELECT DISTINCT AppID,AppFriendlyName,AppType,AppInMenu,CustomerID FROM vwGroupAppAssigned WHERE CustomerID = @Customer AND GroupID = @GroupID AND AppInMenu = 1 ORDER BY AppType,AppFriendlyName ASC"
            command.Parameters.AddWithValue("@GroupID", group)
        End If

        command.Parameters.AddWithValue("@Customer", m_User.Customer.CustomerId)

        adapter = New SqlClient.SqlDataAdapter(command)
        Try
            conn.Open()
            adapter.Fill(result)
            tableRefresh(result)
            Return result
        Catch ex As Exception
            status = ex.Message
            conn.Dispose()
            Return Nothing
        Finally
            conn.Close()
        End Try
    End Function

    Private Function loadGroups(ByRef status As String) As DataTable
        Dim conn As New SqlClient.SqlConnection()
        Dim command As New SqlClient.SqlCommand()
        Dim adapter As SqlClient.SqlDataAdapter
        Dim result As New DataTable()

        status = String.Empty
        conn.ConnectionString = ConfigurationManager.AppSettings("Connectionstring")

        With command
            .Connection = conn
            .CommandType = CommandType.Text
            .CommandText = "SELECT GroupName,GroupID,CustomerID FROM WebGroup WHERE CustomerID = @Customer ORDER BY GroupName ASC"
            .Parameters.AddWithValue("@Customer", m_User.Customer.CustomerId)
        End With

        adapter = New SqlClient.SqlDataAdapter(command)
        Try
            conn.Open()
            adapter.Fill(result)
            Return result
        Catch ex As Exception
            status = ex.Message
            conn.Dispose()
            Return Nothing
        Finally
            conn.Close()
        End Try
    End Function

    Private Sub btnConfirm_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnConfirm.Click
        Dim ctl As Control
        Dim item As DataGridItem
        Dim table As DataTable
        Dim status As String
        Dim row As DataRow
        Dim cell As TableCell
        Dim cbx As CheckBox

        status = String.Empty

        'If txtGroup.Visible = True Then
        table = loadApps(status, 0)
            'Else
            '    table = loadApps(status, ddlGroups.SelectedItem.Value)
            'End If

        If (status = String.Empty) Then
            For Each item In dgApps.Items
                For Each cell In item.Cells
                    For Each ctl In cell.Controls
                        If (TypeOf ctl Is CheckBox) AndAlso (ctl.ID = "cbxAccess") Then
                            cbx = CType(ctl, CheckBox)
                            row = table.Select("AppID=" & item.Cells(0).Text)(0)
                            row("Zugriff") = False
                            If cbx.Checked Then
                                row("Zugriff") = True
                            End If
                        End If
                    Next
                Next
            Next
            sendform(table)
        Else
            lblError.Text = "Fehler beim Laden der Seite."
        End If
    End Sub

    Private Sub sendform(ByVal table As DataTable)
        dgApps.Visible = False
        Table1.Visible = False
        btnConfirm.Visible = False

        Dim str As String
        Dim row As DataRow

        '§§§ JVE 06.09.2006: Mail anonymisieren (Arval raus)
        'str = "ARVAL Helpdesk-Auftrag" & vbCrLf & "----------------------" & vbCrLf & vbCrLf
        str = "Helpdesk-Auftrag" & vbCrLf & "----------------------" & vbCrLf & vbCrLf
        str &= "Verfasser  : " & m_User.UserName & ", " & m_User.CustomerName & vbCrLf
        str &= "Vorgang    : " & rbVorgang.SelectedItem.Text.ToUpper & vbCrLf
        If rbVorgang.SelectedItem.Value > 1 Then     'löschen
            str &= "Gruppenname: " & ddlGroups.SelectedItem.Text.ToUpper & vbCrLf & vbCrLf
        Else
            str &= "Gruppenname: " & txtGroup.Text.ToUpper & vbCrLf & vbCrLf
        End If

        If rbVorgang.SelectedItem.Value < 3 Then   'Nur wenn nicht Löschen, Anwendungen anfügen.
            For Each row In table.Rows
                If CType(row("Zugriff"), Boolean) = True Then
                    str &= "X"
                End If
                str &= vbTab & row("AppFriendlyName")
                str &= vbCrLf
            Next
        End If

        str &= vbCrLf & "AUF DIESE MAIL NICHT ANTWORTEN."


        'Absenden
        Dim client As New System.Net.Mail.SmtpClient(ConfigurationManager.AppSettings("SmtpMailServer"))
        client.Send(ConfigurationManager.AppSettings("SmtpMailSender"), ConfigurationManager.AppSettings("SmtpMailAddress"), "Helpdesk (Gruppenverwaltung) - " & rbVorgang.SelectedItem.Text, str)

        lblMessage.Visible = True
       
    End Sub

    Private Sub rbVorgang_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rbVorgang.SelectedIndexChanged
        Dim table As DataTable
        Dim status As String = ""

        If rbVorgang.SelectedItem.Value = 1 Then
            'Neuanlage
            dgApps.Visible = True
            ddlGroups.Visible = False
            txtGroup.Visible = True
            loadForm()
        Else
            table = loadGroups(status)
            If (status = String.Empty) Then
                If rbVorgang.SelectedItem.Value = 3 Then    'Löschen
                    dgApps.Visible = False
                Else
                    dgApps.Visible = True
                End If
                txtGroup.Visible = False

                With ddlGroups
                    .DataSource = table
                    .DataTextField = "GroupName"
                    .DataValueField = "GroupID"
                    .DataBind()
                    .Visible = True
                End With

                'item = New ListItem()
                'item.Value = 0
                'item.Text = "-alle-"
                'ddlGroups.Items.Add(item)

                If rbVorgang.SelectedItem.Value = 2 Then    'Ändern
                    refreshApps()
                End If
            Else
                lblError.Text = "Fehler beim Laden der Seite."
            End If
        End If
    End Sub

    Private Sub refreshApps()
        Dim apps As DataTable
        Dim status As String = ""
        Dim row As DataRow
        Dim rowUnassigned As DataRow()
        Dim tableUnassigned As DataTable

        apps = loadApps(status, 0)

        If (status = String.Empty) AndAlso (apps.Rows.Count > 0) Then
            dgApps.DataSource = apps
            dgApps.DataBind()

            tableUnassigned = loadAppsUnassigned(status, ddlGroups.SelectedItem.Value)   'Nicht zugewiesene Anwendungen holen

            For Each row In apps.Rows
                rowUnassigned = tableUnassigned.Select("AppID=" & row("AppID"))
                If (rowUnassigned.Length > 0) Then  'Anwendung nicht zugewiesen
                    row("Zugriff") = False
                Else
                    row("Zugriff") = True
                End If
            Next
            updateView(apps)
        Else
            lblError.Text = "Fehler beim Laden der Seite."
        End If
    End Sub

    Private Sub updateView(ByVal table As DataTable)
        Dim ctl As Control
        Dim item As DataGridItem
        Dim row As DataRow
        Dim cell As TableCell
        Dim cell2 As TableCell
        Dim cbx As CheckBox

        For Each item In dgApps.Items
            For Each cell In item.Cells
                For Each ctl In cell.Controls
                    If (TypeOf ctl Is CheckBox) AndAlso (ctl.ID = "cbxAccess") Then
                        cbx = CType(ctl, CheckBox)
                        row = table.Select("AppID=" & item.Cells(0).Text)(0)

                        If CType(row("Zugriff"), Boolean) = True Then
                            For Each cell2 In item.Cells
                                cell2.BackColor = System.Drawing.Color.LightGray
                            Next
                            'cell.BackColor = System.Drawing.Color.LightGray
                            cbx.Checked = True
                        Else
                            cbx.Checked = False
                        End If
                    End If
                Next
            Next
        Next
    End Sub

    Private Sub ddlGroups_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ddlGroups.SelectedIndexChanged
        refreshApps()
    End Sub

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        SetEndASPXAccess(Me)
    End Sub

    Private Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Unload
        SetEndASPXAccess(Me)
    End Sub
End Class

' ************************************************
' $History: Helpdesk01.aspx.vb $
' 
' *****************  Version 4  *****************
' User: Jungj        Date: 5.06.09    Time: 15:58
' Updated in $/CKAG/Components/ComCommon
' mögliche try catches entfernt
' 
' *****************  Version 3  *****************
' User: Rudolpho     Date: 11.04.08   Time: 13:43
' Updated in $/CKAG/Components/ComCommon
' Migration
' 
' *****************  Version 2  *****************
' User: Rudolpho     Date: 10.04.08   Time: 17:37
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
