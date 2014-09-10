Imports CKG.Base.Business
Imports CKG.Base.Kernel
Imports CKG.Base.Kernel.Common.Common
Imports CKG.Portal.PageElements
Imports AppSIXT.SIXT_PDI

Public Class admZulassung
    Inherits System.Web.UI.Page

    Protected WithEvents ucStyles As Styles

#Region " Vom Web Form Designer generierter Code "
    Protected WithEvents lblError As System.Web.UI.WebControls.Label
    Protected WithEvents dgSearchResult As System.Web.UI.WebControls.DataGrid
    Protected WithEvents lblMessage As System.Web.UI.WebControls.Label
    Protected WithEvents trEditUser As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents trSearchSpacer As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents lbtnSave As System.Web.UI.WebControls.LinkButton
    Protected WithEvents lbtnCancel As System.Web.UI.WebControls.LinkButton
    Protected WithEvents Form1 As System.Web.UI.HtmlControls.HtmlForm
    Protected WithEvents CheckBox2 As System.Web.UI.WebControls.CheckBox
    Protected WithEvents CheckBox3 As System.Web.UI.WebControls.CheckBox
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

#Region " Membervariables "
    Private m_User As Base.Kernel.Security.User
    Private m_App As Base.Kernel.Security.App
    Private m_context As HttpContext = HttpContext.Current
#End Region

#Region " Data and Function "
    Private Sub FillForm()
        Dim cn As New SqlClient.SqlConnection(m_User.App.Connectionstring)
        Try
            cn.Open()
            FillCustomer(cn)
            trEditUser.Visible = False
            'trSearchResult.Visible = False

            If m_User.HighestAdminLevel = Security.AdminLevel.Master Then
                'lblCustomer.Visible = False
                'ddlFilterCustomer.Visible = True
                'txtFilterGroupName.Visible = True
            Else
                'trStartMethod.Visible = False
                'lnkCustomerManagement.Visible = False
                'trCustomer.Visible = False 'Customer-Auswahl im Edit-bereich ausblenden
                'lnkAppManagement.Visible = False
                If m_User.IsCustomerAdmin Then
                    'txtFilterGroupName.Visible = True
                Else
                    GroupAdminMode()
                End If
            End If
            'lblCustomer.Text = m_User.Customer.CustomerName
            'lblGroupName.Visible = Not txtFilterGroupName.Visible
            '##################################################################
            If Not m_User.Customer.ShowOrganization Then        '23.05.2005 JVE
                'lnkOrganizationManagement.Visible = False
            End If
            '#################################################################
            Search(True, True, True, True)
        Finally
            cn.Close()
            cn.Dispose()
        End Try
    End Sub

    Private Sub FillCustomer(ByVal cn As SqlClient.SqlConnection)
        Dim dtCustomers As New CKG.Admin.Kernel.CustomerList(m_User.Customer.AccountingArea, cn, True, False)
        Dim dv As DataView = dtCustomers.DefaultView
        dv.Sort = "Customername"
        m_context.Cache.Insert("myCustomerListView", dv, Nothing, DateTime.Now.AddMinutes(20), TimeSpan.Zero)
        'With ddlFilterCustomer
        '    .DataSource = dv
        '    .DataTextField = "Customername"
        '    .DataValueField = "CustomerID"
        '    .DataBind()
        '    .Items.FindByValue(m_User.Customer.CustomerId.ToString).Selected = True
        'End With
    End Sub

    Private Sub FillDataGrid()
        Dim strSort As String = "GroupID"
        If Not ViewState("ResultSort") Is Nothing Then
            strSort = ViewState("ResultSort").ToString
        End If
        FillDataGrid(strSort)
    End Sub

    Private Sub FillDataGrid(ByVal strSort As String)
        Dim dvGroup As DataView
        Dim row As DataRow

        If Not m_context.Cache("myGroupListView") Is Nothing Then
            dvGroup = CType(m_context.Cache("myGroupListView"), DataView)
        Else
            Dim dtGroup As CKG.Admin.Kernel.GroupList
            Dim cn As New SqlClient.SqlConnection(m_User.App.Connectionstring)
            Try
                cn.Open()

                Dim intTemp As Integer
                If m_User.HighestAdminLevel = Security.AdminLevel.Master Then
                    'intTemp = CInt(ddlFilterCustomer.SelectedItem.Value)
                Else
                    intTemp = m_User.Customer.CustomerId
                End If

                dtGroup = New CKG.Admin.Kernel.GroupList("*", intTemp, cn, m_User.Customer.AccountingArea)       'Gruppen holen

                Dim flag As Int32   'Flags setzen...
                For Each row In dtGroup.Rows
                    flag = getFlag(CType(row("GroupID"), Int32))
                    If flag >= 0 Then
                        row("IsCustomerGroup") = flag
                    Else
                        row("IsCustomerGroup") = 0
                    End If
                Next
                dvGroup = dtGroup.DefaultView
                m_context.Cache.Insert("myGroupListView", dvGroup, Nothing, DateTime.Now.AddMinutes(20), TimeSpan.Zero)
            Finally
                cn.Close()
                cn.Dispose()
            End Try
        End If

        dvGroup.Sort = "GroupName"
        If dvGroup.Count > dgSearchResult.PageSize Then
            dgSearchResult.PagerStyle.Visible = True
        Else
            dgSearchResult.PagerStyle.Visible = False
        End If

        'If Not fehler Then
        With dgSearchResult
            .DataSource = dvGroup
            .DataBind()
        End With
        'Else
        'lblError.Text = "Keine Daten gefunden."
        'dgSearchResult.Visible = False
        'End If
    End Sub

    Private Function FillEdit(ByVal intGroupId As Integer) As Boolean
        'SearchMode(False)
        'Dim cn As New SqlClient.SqlConnection(m_User.App.Connectionstring)
        'cn.Open()
        'Dim _Group As New Group(intGroupId, cn)
        'txtGroupID.Text = _Group.GroupId.ToString
        'txtGroupName.Text = _Group.GroupName
        'If Not ddlAuthorizationright.SelectedItem Is Nothing Then
        '    ddlAuthorizationright.SelectedItem.Selected = False
        'End If
        'Dim _li As ListItem = ddlAuthorizationright.Items.FindByValue(_Group.Authorizationright.ToString)
        'If Not _li Is Nothing Then
        '    _li.Selected = True
        'End If
        'cbxIsCustomerGroup.Checked = _Group.IsCustomerGroup
        'txtDocuPath.Text = _Group.DocuPath
        'Dim dvCustomer As DataView
        'If Not m_context.Cache("myCustomerListView") Is Nothing Then
        '    dvCustomer = CType(m_context.Cache("myCustomerListView"), DataView)
        'Else
        '    Dim dtCustomers As New CustomerList(cn, True, False)
        '    dvCustomer.Sort = "Customername"
        '    dvCustomer = dtCustomers.DefaultView
        '    m_context.Cache.Insert("myCustomerListView", dvCustomer, Nothing, DateTime.Now.AddMinutes(20), TimeSpan.Zero)
        'End If
        'txtCustomerID.Text = _Group.CustomerId.ToString
        'If _Group.CustomerId > 0 Then
        '    dvCustomer.Sort = "CustomerID"
        '    txtCustomer.Text = dvCustomer(dvCustomer.Find(_Group.CustomerId)).Item("CustomerName").ToString
        'End If
        'FillAssigned(_Group.GroupId, _Group.CustomerId, cn)
        'FillUnAssigned(_Group.GroupId, _Group.CustomerId, cn)
        'txtStartMethod.Text = _Group.StartMethod
        'txtMessage.Text = _Group.Message
        'txtMessageOld.Text = _Group.Message
        'txtMaxReadMessageCount.Text = _Group.MaxReadMessageCount.ToString
        'Return True
    End Function

    Private Function GetAppAssignedView(ByVal intGroupID As Integer, ByVal intCustomerID As Integer, ByVal cn As SqlClient.SqlConnection) As DataView
        'Als Extra-Funktion ausgelagert, da beim Speichern evtl.
        'benoetigt, wenn Cache leer ist (in jenem Fall soll die
        'List kein DataBind ausfuehren).
        Dim _AppAssigned As New Admin.ApplicationList(intGroupID, intCustomerID, cn)
        _AppAssigned.GetAssigned()
        _AppAssigned.DefaultView.Sort = "AppFriendlyName"
        'Cache wird befuellt, um spaeter beim Speichern darauf
        'zu zugreifen.
        m_context.Cache.Insert("myAppAssigned", _AppAssigned.DefaultView, Nothing, DateTime.Now.AddMinutes(20), TimeSpan.Zero)
        Return _AppAssigned.DefaultView
    End Function

    Private Sub FillAssigned(ByVal intGroupID As Integer, ByVal intCustomerID As Integer, ByVal cn As SqlClient.SqlConnection)
        Dim dvAppAssigned As DataView = GetAppAssignedView(intGroupID, intCustomerID, cn)
        'lstAppAssigned.DataSource = dvAppAssigned
        'lstAppAssigned.DataTextField = "AppFriendlyName"
        'lstAppAssigned.DataValueField = "AppID"
        'lstAppAssigned.DataBind()
    End Sub

    Private Sub FillUnAssigned(ByVal intGroupID As Integer, ByVal intCustomerID As Integer, ByVal cn As SqlClient.SqlConnection)
        Dim _AppUnAssigned As New Admin.ApplicationList(intGroupID, intCustomerID, cn)
        _AppUnAssigned.GetUnassigned()
        _AppUnAssigned.DefaultView.Sort = "AppFriendlyName"
        'lstAppUnAssigned.DataSource = _AppUnAssigned.DefaultView
        'lstAppUnAssigned.DataTextField = "AppFriendlyName"
        'lstAppUnAssigned.DataValueField = "AppID"
        'lstAppUnAssigned.DataBind()
    End Sub

    Private Sub ClearEdit()
        'txtGroupID.Text = "-1"
        'txtGroupName.Text = ""
        'txtDocuPath.Text = ""
        'txtStartMethod.Text = ""
        'txtMessage.Text = ""
        'txtMessageOld.Text = ""
        'txtMaxReadMessageCount.Text = "3"
        'Dim intCustomerID As Integer = CInt(ddlFilterCustomer.SelectedItem.Value)
        'If intCustomerID > 0 Then
        '    txtCustomerID.Text = intCustomerID.ToString
        '    txtCustomer.Text = ddlFilterCustomer.SelectedItem.Text
        'Else
        '    txtCustomerID.Text = m_User.Customer.CustomerId.ToString
        '    txtCustomer.Text = m_User.Customer.CustomerName
        'End If
        'If Not ddlAuthorizationright.SelectedItem Is Nothing Then
        '    ddlAuthorizationright.SelectedItem.Selected = False
        'End If
        'ddlAuthorizationright.Items(0).Selected = True
        'cbxIsCustomerGroup.Checked = False
        'lstAppAssigned.Items.Clear()
        'lstAppUnAssigned.Items.Clear()
        'lbtnSave.Visible = True
        'lbtnDelete.Visible = False
        'LockEdit(False)
    End Sub

    Private Sub LockEdit(ByVal blnLock As Boolean)
        'Dim strBackColor As String = "White"
        'If blnLock Then
        '    strBackColor = "LightGray"
        'End If
        'txtGroupID.Enabled = Not blnLock
        'txtGroupID.BackColor = System.Drawing.Color.FromName(strBackColor)
        'txtGroupName.Enabled = Not blnLock
        'txtGroupName.BackColor = System.Drawing.Color.FromName(strBackColor)
        'txtMessage.Enabled = Not blnLock
        'txtMessage.BackColor = System.Drawing.Color.FromName(strBackColor)
        'txtMaxReadMessageCount.Enabled = Not blnLock
        'txtMaxReadMessageCount.BackColor = System.Drawing.Color.FromName(strBackColor)
        'ddlAuthorizationright.Enabled = Not blnLock
        'ddlAuthorizationright.BackColor = System.Drawing.Color.FromName(strBackColor)
        'cbxIsCustomerGroup.Enabled = Not blnLock
        'txtDocuPath.Enabled = Not blnLock
        'txtDocuPath.BackColor = System.Drawing.Color.FromName(strBackColor)
        'lstAppAssigned.Enabled = Not blnLock
        'lstAppAssigned.BackColor = System.Drawing.Color.FromName(strBackColor)
        'lstAppUnAssigned.Enabled = Not blnLock
        'lstAppUnAssigned.BackColor = System.Drawing.Color.FromName(strBackColor)
        'btnAssign.Enabled = Not blnLock
        'btnUnAssign.Enabled = Not blnLock
    End Sub

    Private Sub GroupAdminMode()
        SearchMode(False)
        'trApp.Visible = False

        If m_User.Groups.Count > 0 Then
            If m_User.Groups(0).IsCustomerGroup Then
                EditEditMode(m_User.Groups(0).GroupId)
            End If
        End If
    End Sub

    Private Sub EditEditMode(ByVal intGroupId As Integer)
        If Not FillEdit(intGroupId) Then
            LockEdit(True)
            lbtnSave.Enabled = False
        Else
            lbtnSave.Enabled = True
        End If
        lbtnCancel.Text = "Verwerfen"
    End Sub

    Private Sub EditDeleteMode(ByVal intGroupId As Integer)
        'If Not FillEdit(intGroupId) Then
        '    lbtnDelete.Enabled = False
        'Else
        '    lblMessage.Text = "Möchten Sie die Gruppe wirklich löschen?"
        '    lbtnDelete.Enabled = True
        'End If
        'LockEdit(True)
        'lbtnCancel.Text = "Abbrechen"
        'lbtnSave.Visible = False
        'lbtnDelete.Visible = True
    End Sub

    Private Sub SearchMode(Optional ByVal blnSearchMode As Boolean = True)
        trEditUser.Visible = Not blnSearchMode
        'trSearch.Visible = blnSearchMode
        trSearchSpacer.Visible = blnSearchMode
        'trSearchResult.Visible = blnSearchMode
        'lbtnSave.Visible = Not blnSearchMode
        lbtnCancel.Visible = Not blnSearchMode
        'lbtnNew.Visible = blnSearchMode
    End Sub

    Private Sub Search(Optional ByVal blnRefillDataGrid As Boolean = False, Optional ByVal blnResetSelectedIndex As Boolean = False, Optional ByVal blnResetPageIndex As Boolean = False, Optional ByVal blnClearCache As Boolean = False)
        ClearEdit()
        If blnClearCache Then
            m_context.Cache.Remove("myGroupListView")
        End If
        If blnResetSelectedIndex Then dgSearchResult.SelectedIndex = -1
        If blnResetPageIndex Then dgSearchResult.CurrentPageIndex = 0
        If m_User.HighestAdminLevel > Security.AdminLevel.Organization Then
            SearchMode()
            If blnRefillDataGrid Then FillDataGrid()
        Else
            GroupAdminMode()
        End If
    End Sub

    Private Sub Log(ByVal strIdentification As String, ByVal strDescription As String, ByVal tblParameters As DataTable, Optional ByVal strCategory As String = "APP")
        Dim logApp As New Base.Kernel.Logging.Trace(m_User.App.Connectionstring, m_User.App.SaveLogAccessSAP, m_User.App.LogLevel)

        ' strCategory
        Dim strUserName As String = m_User.UserName ' strUserName
        Dim strSessionID As String = Session.SessionID ' strSessionID
        Dim intSource As Integer = CInt(Request.QueryString("AppID")) ' intSource 
        Dim strTask As String = "Admin - Gruppenverwaltung" ' strTask
        ' strIdentification
        ' strDescription
        Dim strCustomerName As String = m_User.CustomerName ' strCustomername
        Dim blnIsTestUser As Boolean = m_User.IsTestUser ' blnIsTestUser
        Dim intSeverity As Integer = 0 ' intSeverity 

        logApp.WriteEntry(strCategory, strUserName, strSessionID, intSource, strTask, strIdentification, strDescription, strCustomerName, m_User.Customer.CustomerId, blnIsTestUser, intSeverity, tblParameters)
    End Sub

    'Private Function SetOldLogParameters(ByVal intGroupId As Int32, ByVal tblPar As DataTable) As DataTable
    'Try
    '    Dim cn As New SqlClient.SqlConnection(m_User.App.Connectionstring)
    '    cn.Open()
    '    Dim _Group As New Group(intGroupId, cn)

    '    If tblPar Is Nothing Then
    '        tblPar = CreateLogTableStructure()
    '    End If
    '    With tblPar
    '        .Rows.Add(.NewRow)
    '        .Rows(.Rows.Count - 1)("Status") = "Alt"
    '        .Rows(.Rows.Count - 1)("Gruppenname") = _Group.GroupName
    '        .Rows(.Rows.Count - 1)("Aut.- Recht") = _Group.Authorizationright.ToString
    '        .Rows(.Rows.Count - 1)("Kunden- Gruppe") = _Group.IsCustomerGroup
    '        Dim dvCustomer As DataView
    '        If Not m_context.Cache("myCustomerListView") Is Nothing Then
    '            dvCustomer = CType(m_context.Cache("myCustomerListView"), DataView)
    '        Else
    '            Dim dtCustomers As New CustomerList(cn, True, False)
    '            dvCustomer.Sort = "Customername"
    '            dvCustomer = dtCustomers.DefaultView
    '            m_context.Cache.Insert("myCustomerListView", dvCustomer, Nothing, DateTime.Now.AddMinutes(20), TimeSpan.Zero)
    '        End If
    '        'txtCustomerID.Text = _Group.CustomerId.ToString
    '        If _Group.CustomerId > 0 Then
    '            dvCustomer.Sort = "CustomerID"
    '            .Rows(.Rows.Count - 1)("Firma") = dvCustomer(dvCustomer.Find(_Group.CustomerId)).Item("CustomerName").ToString
    '        End If

    '        Dim dvAppAssigned As DataView = GetAppAssignedView(intGroupId, _Group.CustomerId, cn)
    '        Dim strAnwendungen As String
    '        Dim j As Int32
    '        For j = 0 To dvAppAssigned.Count - 1
    '            strAnwendungen &= dvAppAssigned(j)("AppFriendlyName").ToString & vbNewLine
    '        Next
    '        .Rows(.Rows.Count - 1)("Anwendungen") = strAnwendungen
    '        .Rows(.Rows.Count - 1)("Handbuch") = _Group.DocuPath
    '        .Rows(.Rows.Count - 1)("Startmethode") = _Group.StartMethod
    '        .Rows(.Rows.Count - 1)("Message") = _Group.Message
    '        .Rows(.Rows.Count - 1)("MaxReadMessageCount") = _Group.MaxReadMessageCount
    '    End With
    '    Return tblPar
    '    Catch ex As Exception
    '    m_App.WriteErrorText(1, m_User.UserName, "GroupManagement", "SetOldLogParameters", ex.ToString)

    '    Dim dt As New DataTable()
    '    dt.Columns.Add("Fehler beim Erstellen der Log-Parameter", System.Type.GetType("System.String"))
    '    dt.Rows.Add(dt.NewRow)
    '    Dim str As String = ex.Message
    '    If Not ex.InnerException Is Nothing Then
    '        str &= ": " & ex.InnerException.Message
    '    End If
    '    dt.Rows(0)("Fehler beim Erstellen der Log-Parameter") = str
    '    Return dt
    '    End Try
    'End Function

    'Private Function SetNewLogParameters(ByVal tblPar As DataTable) As DataTable
    ' Try
    '    If tblPar Is Nothing Then
    '        tblPar = CreateLogTableStructure()
    '    End If
    '    With tblPar
    '        .Rows.Add(.NewRow)
    '        .Rows(.Rows.Count - 1)("Gruppenname") = ""
    '        .Rows(.Rows.Count - 1)("Flag") = ""
    '    End With
    '    Return tblPar
    '   Catch ex As Exception
    '    m_App.WriteErrorText(1, m_User.UserName, "GroupManagement", "SetNewLogParameters", ex.ToString)

    '    Dim dt As New DataTable()
    '    dt.Columns.Add("Fehler beim Erstellen der Log-Parameter", System.Type.GetType("System.String"))
    '    dt.Rows.Add(dt.NewRow)
    '    Dim str As String = ex.Message
    '    If Not ex.InnerException Is Nothing Then
    '        str &= ": " & ex.InnerException.Message
    '    End If
    '    dt.Rows(0)("Fehler beim Erstellen der Log-Parameter") = str
    '    Return dt
    '    End Try
    'End Function

    'Private Function CreateLogTableStructure() As DataTable
    'Dim tblPar As New DataTable()
    'With tblPar
    '    .Columns.Add("Gruppenname", System.Type.GetType("System.String"))
    '    .Columns.Add("Flag", System.Type.GetType("System.String"))
    'End With
    'Return tblPar
    'End Function
#End Region

#Region " Events "
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        ' Hier Benutzercode zur Seiteninitialisierung einfügen
        m_User = GetUser(Me)
        ucHeader.InitUser(m_User)
        ucStyles.TitleText = "Gruppenverwaltung"
        AdminAuth(Me, m_User, Security.AdminLevel.Customer)

        'tbl1.Visible = False

        Try
            m_App = New Base.Kernel.Security.App(m_User)

            If Not IsPostBack Then
                lblError.Text = ""

                'Fülle ddlAuthorizationright mit festen Vorgaben
                Dim strAuthorRights(4) As String
                strAuthorRights(0) = "0 - keine"
                strAuthorRights(1) = "0..1 - wenig"
                strAuthorRights(2) = "0..2 - mittel"
                strAuthorRights(3) = "0..3 - viel"
                Dim i As Int32
                For i = 0 To 3
                    Dim listitem As New ListItem()
                    listitem.Value = CStr(i)
                    listitem.Text = strAuthorRights(i)
                    ' ddlAuthorizationright.Items.Add(listitem)
                Next

                FillForm()
            End If
        Catch ex As Exception
            m_App.WriteErrorText(1, m_User.UserName, "GroupManagement", "Page_Load", ex.ToString)
            lblError.Text = ex.ToString
            lblError.Visible = True
        End Try
    End Sub

    Private Sub dgSearchResult_SortCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs) Handles dgSearchResult.SortCommand
        Dim strSort As String = e.SortExpression
        If Not ViewState("ResultSort") Is Nothing AndAlso ViewState("ResultSort").ToString = strSort Then
            strSort &= " DESC"
        End If
        ViewState("ResultSort") = strSort
        FillDataGrid(strSort)
    End Sub

    Private Sub dgSearchResult_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles dgSearchResult.ItemCommand
        If e.CommandName = "Edit" Then
            EditEditMode(CInt(e.Item.Cells(0).Text))
            dgSearchResult.SelectedIndex = e.Item.ItemIndex
        ElseIf e.CommandName = "Delete" Then
            EditDeleteMode(CInt(e.Item.Cells(0).Text))
            dgSearchResult.SelectedIndex = e.Item.ItemIndex
        End If
    End Sub

    Private Sub dgSearchResult_PageIndexChanged(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles dgSearchResult.PageIndexChanged
        dgSearchResult.CurrentPageIndex = e.NewPageIndex
        FillDataGrid()
    End Sub

    Private Sub lbtnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lbtnCancel.Click
        Search(, True)
    End Sub

    Private Function collectGroups() As ArrayList
        Dim item As DataGridItem
        Dim cell As TableCell
        Dim checkbox As CheckBox
        Dim control As Control
        Dim id_gruppe As String
        Dim resultList As New ArrayList()
        Dim resultElement As ArrayList

        'ids = String.Empty

        For Each item In dgSearchResult.Items
            id_gruppe = "GroupID = '" & item.Cells(0).Text & "'"
            'Prüft, welche Häkchen gesetzt sind
            For Each cell In item.Cells
                For Each control In cell.Controls
                    If TypeOf control Is CheckBox Then
                        checkbox = CType(control, CheckBox)
                        resultElement = New ArrayList(2)
                        resultElement.Add(CType(item.Cells(0).Text, Int32))
                        If checkbox.Checked Then
                            resultElement.Add(1)
                        Else
                            resultElement.Add(0)
                        End If
                        resultList.Add(resultElement)
                    End If
                Next
            Next
        Next
        Return resultList
    End Function

    Private Sub lbtnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lbtnSave.Click

        Dim ids As ArrayList
        Dim element As ArrayList
        Dim index As Integer
        Dim sql_insert As String
        Dim sql_update As String
        Dim sql_select As String
        Dim result As Int32
        Dim conn As New SqlClient.SqlConnection(ConfigurationManager.AppSettings("Connectionstring"))
        Dim command As New SqlClient.SqlCommand()

        sql_insert = "INSERT INTO Flags (id_group,flag1) VALUES (@id,@flag)"
        sql_update = "UPDATE Flags SET flag1 = @flag WHERE id_group = @id"
        sql_select = "SELECT count(id) FROM Flags WHERE id_group = @id"

        command.Parameters.AddWithValue("@id", System.DBNull.Value)
        command.Parameters.AddWithValue("@flag", System.DBNull.Value)

        command.Connection = conn

        ids = collectGroups()

        Try
            conn.Open()

            For index = 0 To ids.Count - 1
                command.CommandText = sql_select    'Zuerst prüfen, ob Eintrag (gruppe) bereits vorhanden.
                element = CType(ids(index), ArrayList)

                command.Parameters("@id").Value = CType(element(0), Int32)
                result = command.ExecuteScalar()
                If result = 0 Then                      'Nicht vorhanden, einfügen!
                    command.CommandText = sql_insert
                Else                                    'Vorhanden, nur updaten!
                    command.CommandText = sql_update
                End If
                command.Parameters("@id").Value = element(0)        'id_group
                command.Parameters("@flag").Value = element(1)      'flag
                command.ExecuteScalar()                             'exekutieren.
            Next
            lblMessage.Text = "Daten gespeichert."
        Catch ex As Exception
            lblError.Text = "Fehler beim Speichern der Daten."
        Finally
            conn.Close()
            conn.Dispose()
        End Try
    End Sub

    Private Sub lbtnDelete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        'Dim tblLogParameter As DataTable
        'Try
        '    Dim _Group As New Group(CInt(txtGroupID.Text), CInt(ddlFilterCustomer.SelectedItem.Value))
        '    Dim cn As New SqlClient.SqlConnection(m_User.App.Connectionstring)
        '    cn.Open()
        '    tblLogParameter = SetOldLogParameters(_Group.GroupId, tblLogParameter)
        '    If Not _Group.HasUser(cn) Then
        '        _Group.Delete(cn)
        '        Log(_Group.GroupId.ToString, "Gruppe löschen", tblLogParameter)

        '        Search(True, True, True, True)
        '        lblMessage.Text = "Die Gruppe wurde gelöscht."
        '    Else
        '        lblMessage.Text = "Die Gruppe kann nicht gelöscht werden, da ihr noch Benutzer zugeordnet sind."
        '    End If
        'Catch ex As Exception
        '    m_App.WriteErrorText(1, m_User.UserName, "GroupManagement", "lbtnDelete_Click", ex.ToString)

        '    lblError.Text = ex.Message
        '    If Not ex.InnerException Is Nothing Then
        '        lblError.Text &= ": " & ex.InnerException.Message
        '    End If
        '    Log(txtGroupID.Text, lblError.Text, tblLogParameter, "ERR")
        'End Try
    End Sub

    Private Sub btnAssign_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        'Dim _item As ListItem
        'Dim _coll As New ListItemCollection()

        'For Each _item In lstAppUnAssigned.Items
        '    If _item.Selected = True Then
        '        _item.Selected = False
        '        _coll.Add(_item)
        '    End If
        'Next

        'For Each _item In _coll
        '    lstAppAssigned.Items.Add(_item)
        '    lstAppUnAssigned.Items.Remove(_item)
        'Next
    End Sub

    Private Sub btnUnAssign_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        'Dim _item As ListItem
        'Dim _coll As New ListItemCollection()

        'For Each _item In lstAppAssigned.Items
        '    If _item.Selected = True Then
        '        _item.Selected = False
        '        _coll.Add(_item)
        '    End If
        'Next

        'For Each _item In _coll
        '    lstAppUnAssigned.Items.Add(_item)
        '    lstAppAssigned.Items.Remove(_item)
        'Next
    End Sub

    Private Sub btnSuche_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Search(True, True, True, True)
    End Sub

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        SetEndASPXAccess(Me)
    End Sub

    Private Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Unload
        SetEndASPXAccess(Me)
    End Sub
#End Region
End Class

' ************************************************
' $History: admZulassung.aspx.vb $
' 
' *****************  Version 3  *****************
' User: Rudolpho     Date: 4.05.09    Time: 10:16
' Updated in $/CKAG/Applications/appsixt/Forms
' ITA:2837
' 
' *****************  Version 2  *****************
' User: Hartmannu    Date: 9.09.08    Time: 17:01
' Updated in $/CKAG/Applications/appsixt/Forms
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 8.04.08    Time: 13:37
' Created in $/CKAG/Applications/appsixt/Forms
' 
' *****************  Version 6  *****************
' User: Rudolpho     Date: 6.12.07    Time: 14:36
' Updated in $/CKG/Applications/AppSIXT/AppSIXTWeb/Forms
' ITA: 1440
' 
' *****************  Version 5  *****************
' User: Uha          Date: 2.07.07    Time: 14:09
' Updated in $/CKG/Applications/AppSIXT/AppSIXTWeb/Forms
' Logging der Laufzeiten der ASPX-Seiten eingeführt
' 
' *****************  Version 4  *****************
' User: Uha          Date: 8.03.07    Time: 13:15
' Updated in $/CKG/Applications/AppSIXT/AppSIXTWeb/Forms
' 
' ************************************************
