
Imports CKG.Base.Kernel.Admin
Imports CKG.Base.Kernel.Security
Imports CKG.Base.Kernel.Common.Common

Partial Public Class Groupmanagement
    Inherits System.Web.UI.Page

#Region " Membervariables "

    Private m_User As User
    Private m_App As App
    Protected WithEvents GridNavigation1 As Global.Admin.GridNavigation

#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        m_User = GetUser(Me)
        lblHead.Text = "Gruppenverwaltung"
        AdminAuth(Me, m_User, AdminLevel.Customer)
        GridNavigation1.setGridElment(dgSearchResult)
        Try
            m_App = New App(m_User)

            lblError.Text = ""
            lblMessage.Text = ""

            If Not IsPostBack Then
                'Fülle ddlAuthorizationright mit festen Vorgaben
                Dim strAuthorRights(2) As String
                strAuthorRights(0) = "Ja"
                strAuthorRights(1) = "Nein"
                Dim i As Int32
                For i = 0 To 1
                    Dim listitem As New ListItem()
                    listitem.Value = CStr(i)
                    listitem.Text = strAuthorRights(i)
                    ddlAuthorizationright.Items.Add(listitem)
                Next

                FillForm()
            End If
        Catch ex As Exception
            m_App.WriteErrorText(1, m_User.UserName, "GroupManagement", "Page_Load", ex.ToString)
            lblError.Text = ex.ToString
        End Try
    End Sub

#Region " Data and Function "

    Private Sub FillForm()
        Dim cn As New SqlClient.SqlConnection(m_User.App.Connectionstring)
        Try
            cn.Open()
            FillCustomer(cn)
            trEditUser.Visible = False
            trSearchResult.Visible = False
            trApp.Visible = False
            trArchiv.Visible = False
            trEmployee.Visible = False
            trMeldung.Visible = False
            If m_User.HighestAdminLevel = AdminLevel.Master Then
                'wenn SuperUser und übergeordnete Firma
                If m_User.Customer.AccountingArea = -1 Then
                    lnkAppManagement.Visible = True
                End If
                lblCustomer.Visible = False
                ddlFilterCustomer.Visible = True
                txtFilterGroupName.Visible = True
            Else
                trStartMethod.Visible = False
                lnkCustomerManagement.Visible = False
                lnkArchivManagement.Visible = False
                trCustomer.Visible = False 'Customer-Auswahl im Edit-bereich ausblenden
                lnkAppManagement.Visible = False
                trTVShow.Visible = False
                If m_User.IsCustomerAdmin Then
                    txtFilterGroupName.Visible = True
                Else
                    GroupAdminMode()
                End If
            End If
            lblCustomer.Text = m_User.Customer.CustomerName
            lblGroupName.Visible = Not txtFilterGroupName.Visible
            '##################################################################
            If Not m_User.Customer.ShowOrganization Then        '23.05.2005 JVE
                lnkOrganizationManagement.Visible = False
            End If
            '#################################################################
        Finally
            If cn.State <> ConnectionState.Closed Then
                cn.Close()
            End If
        End Try
    End Sub

    Private Sub FillCustomer(ByVal cn As SqlClient.SqlConnection)
        Dim dtCustomers As Kernel.CustomerList
        dtCustomers = New Kernel.CustomerList(m_User.Customer.AccountingArea, cn, True, False)

        Dim dv As DataView = dtCustomers.DefaultView
        dv.Sort = "Customername"
        ' m_context.Cache.Insert("myCustomerListView", dv, Nothing, DateTime.Now.AddMinutes(20), TimeSpan.Zero)
        Session.Add("myCustomerListView", dv)

        With ddlFilterCustomer
            .DataSource = dv
            .DataTextField = "Customername"
            .DataValueField = "CustomerID"
            .DataBind()
            .Items.FindByValue(m_User.Customer.CustomerId.ToString).Selected = True
        End With
    End Sub

    Private Sub FillDataGrid()
        Dim strSort As String = "GroupID"
        If Not Session("ResultSort") Is Nothing Then
            strSort = Session("ResultSort").ToString
        End If
        FillDataGrid(strSort)
    End Sub

    Private Sub FillDataGrid(ByVal strSort As String)
        trSearchResult.Visible = True
        Dim dvGroup As DataView

        If Not Session("myGroupListView") Is Nothing Then
            dvGroup = CType(Session("myGroupListView"), DataView)
        Else
            Dim dtGroup As Kernel.GroupList
            Dim cn As New SqlClient.SqlConnection(m_User.App.Connectionstring)
            Try
                cn.Open()

                Dim intTemp As Integer
                If m_User.HighestAdminLevel = AdminLevel.Master Then
                    intTemp = CInt(ddlFilterCustomer.SelectedItem.Value)
                Else
                    intTemp = m_User.Customer.CustomerId
                End If

                dtGroup = New Kernel.GroupList(txtFilterGroupName.Text, _
                                                    intTemp, _
                                                    cn, _
                                                    m_User.Customer.AccountingArea)
                dvGroup = dtGroup.DefaultView
                Session.Add("myGroupListView", dvGroup)
                ' m_context.Cache.Insert("myGroupListView", dvGroup, Nothing, DateTime.Now.AddMinutes(20), TimeSpan.Zero)
            Finally
                If cn.State <> ConnectionState.Closed Then
                    cn.Close()
                End If
            End Try
        End If
        dvGroup.Sort = strSort

        With dgSearchResult
            .DataSource = dvGroup
            .DataBind()
        End With
    End Sub

    Private Function FillEdit(ByVal intGroupId As Integer) As Boolean
        Dim cn As SqlClient.SqlConnection
        cn = New SqlClient.SqlConnection(m_User.App.Connectionstring)
        Try
            SearchMode(False)
            cn.Open()
            Dim _Group As New Group(intGroupId, cn)
            txtGroupID.Text = _Group.GroupId.ToString
            txtGroupName.Text = _Group.GroupName
            If Not ddlAuthorizationright.SelectedItem Is Nothing Then
                ddlAuthorizationright.SelectedItem.Selected = False
            End If
            Dim _li As ListItem = ddlAuthorizationright.Items.FindByValue(_Group.Authorizationright.ToString)
            If Not _li Is Nothing Then
                _li.Selected = True
            End If
            cbxIsCustomerGroup.Checked = _Group.IsCustomerGroup
            txtDocuPath.Text = _Group.DocuPath
            Dim dvCustomer As New DataView
            If Not Session("myCustomerListView") Is Nothing Then
                dvCustomer = CType(Session("myCustomerListView"), DataView)
            Else
                Dim dtCustomers As Kernel.CustomerList
                dtCustomers = New Kernel.CustomerList(m_User.Customer.AccountingArea, cn, True, False)

                dvCustomer.Sort = "Customername"
                dvCustomer = dtCustomers.DefaultView
                Session.Add("myCustomerListView", dvCustomer)
            End If
            txtCustomerID.Text = _Group.CustomerId.ToString
            If _Group.CustomerId > 0 Then
                dvCustomer.Sort = "CustomerID"
                txtCustomer.Text = dvCustomer(dvCustomer.Find(_Group.CustomerId)).Item("CustomerName").ToString
            End If
            FillAssigned(_Group.GroupId, _Group.CustomerId, cn)
            FillUnAssigned(_Group.GroupId, _Group.CustomerId, cn)
            txtStartMethod.Text = _Group.StartMethod
            txtMessage.Text = _Group.Message
            txtMessageOld.Text = _Group.Message
            txtMaxReadMessageCount.Text = _Group.MaxReadMessageCount.ToString
            cbxTeamViewer.Checked = _Group.ShowsTeamViewer
            cbxIsServiceGroup.Checked = _Group.IsServiceGroup
            Return True
        Catch ex As Exception
            lblError.Text = "Fehler: " & ex.Message
            m_App.WriteErrorText(1, m_User.UserName, "GroupManagement", "FillEdit", ex.ToString)
        Finally
            If cn.State <> ConnectionState.Closed Then
                cn.Close()
            End If
        End Try
    End Function

    Private Function GetAppAssignedView(ByVal intGroupID As Integer, ByVal intCustomerID As Integer, ByVal cn As SqlClient.SqlConnection) As DataView
        'Als Extra-Funktion ausgelagert, da beim Speichern evtl.
        'benoetigt, wenn Cache leer ist (in jenem Fall soll die
        'List kein DataBind ausfuehren).
        Dim _AppAssigned As New ApplicationList(intGroupID, intCustomerID, cn)
        _AppAssigned.GetAssigned()
        _AppAssigned.DefaultView.Sort = "AppFriendlyName"
        'Cache wird befuellt, um spaeter beim Speichern darauf
        'zu zugreifen.
        Session.Add("myAppAssigned", _AppAssigned.DefaultView)
        Return _AppAssigned.DefaultView
    End Function

    Private Function GetArchivAssignedView(ByVal intGroupID As Integer, ByVal intCustomerID As Integer, ByVal cn As SqlClient.SqlConnection) As DataView
        'Als Extra-Funktion ausgelagert, da beim Speichern evtl.
        'benoetigt, wenn Cache leer ist (in jenem Fall soll die
        'List kein DataBind ausfuehren).
        Dim _ArchivAssigned As New ArchivList(intGroupID, intCustomerID, cn)
        _ArchivAssigned.GetAssigned()
        _ArchivAssigned.DefaultView.Sort = "EasyArchivName"
        'Cache wird befuellt, um spaeter beim Speichern darauf
        'zu zugreifen.
        Session.Add("myArchivAssigned", _ArchivAssigned.DefaultView)
        Return _ArchivAssigned.DefaultView
    End Function

    Private Function GetEmployeeAssignedView(ByVal intGroupID As Integer, ByVal intAccountingArea As Integer, ByVal cn As SqlClient.SqlConnection) As DataView
        'Als Extra-Funktion ausgelagert, da beim Speichern evtl.
        'benoetigt, wenn Cache leer ist (in jenem Fall soll die
        'List kein DataBind ausfuehren).
        Dim _EmployeeAssigned As New EmployeeList(intGroupID, intAccountingArea, cn)
        _EmployeeAssigned.GetAssigned()
        _EmployeeAssigned.DefaultView.Sort = "EmployeeName"
        'Cache wird befuellt, um spaeter beim Speichern darauf
        'zu zugreifen.
        Session.Add("myEmployeeAssigned", _EmployeeAssigned.DefaultView)
        Return _EmployeeAssigned.DefaultView
    End Function

    Private Sub FillAssigned(ByVal intGroupID As Integer, ByVal intCustomerID As Integer, ByVal cn As SqlClient.SqlConnection)
        Dim dvAppAssigned As DataView = GetAppAssignedView(intGroupID, intCustomerID, cn)
        lstAppAssigned.DataSource = dvAppAssigned
        lstAppAssigned.DataTextField = "AppFriendlyName"
        lstAppAssigned.DataValueField = "AppID"
        lstAppAssigned.DataBind()

        Dim dvArchivAssigned As DataView = GetArchivAssignedView(intGroupID, intCustomerID, cn)
        lstArchivAssigned.DataSource = dvArchivAssigned
        lstArchivAssigned.DataTextField = "EasyArchivName"
        lstArchivAssigned.DataValueField = "ArchivID"
        lstArchivAssigned.DataBind()

        Dim dvEmployeeAssigned As DataView = GetEmployeeAssignedView(intGroupID, m_User.Customer.AccountingArea, cn)
        lstEmployeeAssigned.DataSource = dvEmployeeAssigned
        lstEmployeeAssigned.DataTextField = "EmployeeName"
        lstEmployeeAssigned.DataValueField = "UserID"
        lstEmployeeAssigned.DataBind()
    End Sub

    Private Sub FillUnAssigned(ByVal intGroupID As Integer, ByVal intCustomerID As Integer, ByVal cn As SqlClient.SqlConnection)
        Dim _AppUnAssigned As New ApplicationList(intGroupID, intCustomerID, cn)
        _AppUnAssigned.GetUnassigned()
        _AppUnAssigned.DefaultView.Sort = "AppFriendlyName"
        lstAppUnAssigned.DataSource = _AppUnAssigned.DefaultView
        lstAppUnAssigned.DataTextField = "AppFriendlyName"
        lstAppUnAssigned.DataValueField = "AppID"
        lstAppUnAssigned.DataBind()

        Dim _ArchivUnAssigned As New ArchivList(intGroupID, intCustomerID, cn)
        _ArchivUnAssigned.GetUnassigned()
        _ArchivUnAssigned.DefaultView.Sort = "EasyArchivName"
        lstArchivUnAssigned.DataSource = _ArchivUnAssigned.DefaultView
        lstArchivUnAssigned.DataTextField = "EasyArchivName"
        lstArchivUnAssigned.DataValueField = "ArchivID"
        lstArchivUnAssigned.DataBind()

        Dim _EmployeeUnAssigned As New EmployeeList(intGroupID, m_User.Customer.AccountingArea, cn)
        _EmployeeUnAssigned.GetUnassigned()
        _EmployeeUnAssigned.DefaultView.Sort = "EmployeeName"
        lstEmployeeUnAssigned.DataSource = _EmployeeUnAssigned.DefaultView
        lstEmployeeUnAssigned.DataTextField = "EmployeeName"
        lstEmployeeUnAssigned.DataValueField = "UserID"
        lstEmployeeUnAssigned.DataBind()
    End Sub

    Private Sub ClearEdit()
        txtGroupID.Text = "-1"
        txtGroupName.Text = ""
        txtDocuPath.Text = ""
        txtStartMethod.Text = ""
        txtMessage.Text = ""
        txtMessageOld.Text = ""
        txtMaxReadMessageCount.Text = "3"
        Dim intCustomerID As Integer = CInt(ddlFilterCustomer.SelectedItem.Value)
        If intCustomerID > 0 Then
            txtCustomerID.Text = intCustomerID.ToString
            txtCustomer.Text = ddlFilterCustomer.SelectedItem.Text
        Else
            txtCustomerID.Text = m_User.Customer.CustomerId.ToString
            txtCustomer.Text = m_User.Customer.CustomerName
        End If
        If Not ddlAuthorizationright.SelectedItem Is Nothing Then
            ddlAuthorizationright.SelectedItem.Selected = False
        End If
        ddlAuthorizationright.Items(0).Selected = True
        cbxIsCustomerGroup.Checked = False
        lstAppAssigned.Items.Clear()
        lstAppUnAssigned.Items.Clear()
        lbtnSave.Visible = True
        lbtnDelete.Visible = False
        LockEdit(False)
        cbxTeamViewer.Checked = New Customer(intCustomerID).ShowsTeamViewer
        cbxIsServiceGroup.Checked = False
    End Sub

    Private Sub LockEdit(ByVal blnLock As Boolean)
        Dim strBackColor As String = "White"
        If blnLock Then
            strBackColor = "LightGray"
        End If
        txtGroupID.Enabled = Not blnLock
        txtGroupID.BackColor = System.Drawing.Color.FromName(strBackColor)
        txtGroupName.Enabled = Not blnLock
        txtGroupName.BackColor = System.Drawing.Color.FromName(strBackColor)
        txtMessage.Enabled = Not blnLock
        txtMessage.BackColor = System.Drawing.Color.FromName(strBackColor)
        txtMaxReadMessageCount.Enabled = Not blnLock
        txtMaxReadMessageCount.BackColor = System.Drawing.Color.FromName(strBackColor)
        ddlAuthorizationright.Enabled = Not blnLock
        ddlAuthorizationright.BackColor = System.Drawing.Color.FromName(strBackColor)
        cbxIsCustomerGroup.Enabled = Not blnLock
        txtDocuPath.Enabled = Not blnLock
        txtDocuPath.BackColor = System.Drawing.Color.FromName(strBackColor)
        lstAppAssigned.Enabled = Not blnLock
        lstAppAssigned.BackColor = System.Drawing.Color.FromName(strBackColor)
        lstAppUnAssigned.Enabled = Not blnLock
        lstAppUnAssigned.BackColor = System.Drawing.Color.FromName(strBackColor)
        btnAssign.Enabled = Not blnLock
        btnUnAssign.Enabled = Not blnLock
        cbxTeamViewer.Enabled = False 'Not blnLock
        cbxIsServiceGroup.Enabled = Not blnLock
    End Sub

    Private Sub GroupAdminMode()
        SearchMode(False)
        trApp.Visible = False

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
        If Not FillEdit(intGroupId) Then
            lbtnDelete.Enabled = False
        Else
            lblMessage.Text = "Möchten Sie die Gruppe wirklich löschen?"
            lbtnDelete.Enabled = True
        End If
        LockEdit(True)
        lbtnCancel.Text = "Abbrechen"
        lbtnSave.Visible = False
        lbtnDelete.Visible = True
    End Sub

    Private Sub SearchMode(Optional ByVal blnSearchMode As Boolean = True)
        trEditUser.Visible = Not blnSearchMode
        tableSearch.Visible = blnSearchMode
        trSearchSpacer1.Visible = blnSearchMode
        trSearchResult.Visible = blnSearchMode
        trApp.Visible = Not blnSearchMode
        trArchiv.Visible = Not blnSearchMode
        trEmployee.Visible = Not blnSearchMode
        trMeldung.Visible = Not blnSearchMode
        lbtnSave.Visible = Not blnSearchMode
        lbtnCancel.Visible = Not blnSearchMode
        lbtnNew.Visible = blnSearchMode
        DivSearch1.Visible = blnSearchMode
        QueryFooter.Visible = blnSearchMode
        Result.Visible = blnSearchMode
        Input.Visible = Not blnSearchMode
    End Sub

    Private Sub Search(Optional ByVal blnRefillDataGrid As Boolean = False, Optional ByVal blnResetSelectedIndex As Boolean = False, Optional ByVal blnResetPageIndex As Boolean = False, Optional ByVal blnClearCache As Boolean = False)
        ClearEdit()
        If blnClearCache Then
            Session("myGroupListView") = Nothing
        End If
        If blnResetSelectedIndex Then dgSearchResult.SelectedIndex = -1
        If blnResetPageIndex Then dgSearchResult.PageIndex = 0
        If m_User.HighestAdminLevel > AdminLevel.Organization Then
            SearchMode()
            If blnRefillDataGrid Then FillDataGrid()
        Else
            GroupAdminMode()
        End If
    End Sub

#End Region

#Region " Events "

    Private Sub lbtnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lbtnCancel.Click
        Search(, True)
    End Sub

    Private Sub lbtnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lbtnNew.Click
        Dim intCustomer As Integer = CInt(ddlFilterCustomer.SelectedItem.Value)
        Session("myAppAssigned") = Nothing
        txtGroupID.Text = -1

        If intCustomer < 1 Then
            lblError.Text = "Wählen Sie bitte zunächst eine Firma aus!"
        Else
            SearchMode(False)
            ClearEdit()
            Dim cn As New SqlClient.SqlConnection(m_User.App.Connectionstring)
            Try
                cn.Open()
                FillUnAssigned(CInt(txtGroupID.Text), CInt(ddlFilterCustomer.SelectedItem.Value), cn)
            Finally
                If cn.State <> ConnectionState.Closed Then
                    cn.Close()
                End If
            End Try
        End If
    End Sub

    Private Sub lbtnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lbtnSave.Click
        Dim cn As SqlClient.SqlConnection

        If Not IsNumeric(txtMaxReadMessageCount.Text) Then
            lblMessage.Text = "Bitte geben Sie einen Zahlenwert für die Häufigkeit ein."
            Exit Sub
        End If
        cn = New SqlClient.SqlConnection(m_User.App.Connectionstring)
        Try

            cn.Open()
            Dim intGroupId As Integer = CInt(txtGroupID.Text)

            Dim blnNew As Boolean = False
            If CInt(txtGroupID.Text) < 1 Then blnNew = True
            Dim _group As New Group(intGroupId, _
                                                txtGroupName.Text.ToUpper, _
                                                CInt(txtCustomerID.Text), _
                                                txtDocuPath.Text, _
                                                CInt(ddlAuthorizationright.SelectedItem.Value.ToString), _
                                                cbxIsCustomerGroup.Checked, _
                                                blnNew, _
                                                txtStartMethod.Text, _
                                                txtMessage.Text.Trim.TrimEnd, _
                                                CInt(txtMaxReadMessageCount.Text), _
                                                cbxTeamViewer.Checked, _
                                                cbxIsServiceGroup.Checked)
            _group.Save(cn)

            If (Not (txtMessage.Text = txtMessageOld.Text)) OrElse (Not blnNew) Then
                'User Count zurücksetzen
                Dim cmdUpdateUser As SqlClient.SqlCommand
                Dim cmdGetUser As New SqlClient.SqlCommand("SELECT UserID FROM WebMember WHERE GroupID=@GroupID", cn)
                cmdGetUser.Parameters.AddWithValue("@GroupID", intGroupId)
                Dim dt As New DataTable()
                Dim da As New SqlClient.SqlDataAdapter()
                da.SelectCommand = cmdGetUser
                da.Fill(dt)
                Dim dr As DataRow
                For Each dr In dt.Rows
                    cmdUpdateUser = New SqlClient.SqlCommand("UPDATE WebUser SET ReadMessageCount=@ReadMessageCount WHERE UserID=@UserID", cn)
                    Dim intReadMessageCount As Int32 = 0
                    cmdUpdateUser.Parameters.AddWithValue("@ReadMessageCount", intReadMessageCount)
                    cmdUpdateUser.Parameters.AddWithValue("@UserID", CInt(dr("UserID")))
                    cmdUpdateUser.ExecuteNonQuery()
                Next
            End If

            'Anwendungen zuordnen
            Dim dvAppAssigned As New DataView
            If blnNew Then
                intGroupId = _group.GroupId
                txtGroupID.Text = intGroupId.ToString
            Else
                If Not Session("myAppAssigned") Is Nothing Then
                    dvAppAssigned = CType(Session("myAppAssigned"), DataView)
                Else
                    dvAppAssigned = GetAppAssignedView(intGroupId, _group.CustomerId, cn)
                End If
            End If
            Dim _assignment As New Kernel.AppAssignments(intGroupId, Kernel.AssignmentType.Group)
            _assignment.Save(dvAppAssigned, lstAppAssigned.Items, cn)

            'Archive zuordnen
            Dim dvArchivAssigned As New DataView
            If blnNew Then
                intGroupId = _group.GroupId
                txtGroupID.Text = intGroupId.ToString
            Else
                If Not Session("myArchivAssigned") Is Nothing Then
                    dvArchivAssigned = CType(Session("myArchivAssigned"), DataView)
                Else
                    dvArchivAssigned = GetArchivAssignedView(intGroupId, _group.CustomerId, cn)
                End If
            End If
            Dim _archivassignment As New Kernel.ArchivAssignments(intGroupId, Kernel.AssignmentType.Group)
            _archivassignment.Save(dvArchivAssigned, lstArchivAssigned.Items, cn)

            'Employees zuordnen
            Dim dvEmployeeAssigned As New DataView
            If blnNew Then
                intGroupId = _group.GroupId
                txtGroupID.Text = intGroupId.ToString
            Else
                If Not Session("myEmployeeAssigned") Is Nothing Then
                    dvEmployeeAssigned = CType(Session("myEmployeeAssigned"), DataView)
                Else
                    dvEmployeeAssigned = GetEmployeeAssignedView(intGroupId, m_User.Customer.AccountingArea, cn)
                End If
            End If
            Dim _Employeeassignment As New Kernel.EmployeeAssignments(intGroupId)
            _Employeeassignment.Save(dvEmployeeAssigned, lstEmployeeAssigned.Items, cn)

            Search(True, True, , True)
            lblMessage.Text = "Die Änderungen wurden gespeichert."
        Catch ex As Exception
            m_App.WriteErrorText(1, m_User.UserName, "GroupManagement", "lbtnSave_Click", ex.ToString)

            lblError.Text = ex.Message
            If Not ex.InnerException Is Nothing Then
                lblError.Text &= ": " & ex.InnerException.Message
            End If
        Finally
            If cn.State <> ConnectionState.Closed Then
                cn.Close()
            End If
        End Try
    End Sub

    Private Sub lbtnDelete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lbtnDelete.Click
        Dim cn As SqlClient.SqlConnection
        cn = New SqlClient.SqlConnection(m_User.App.Connectionstring)
        Try
            Dim _Group As New Group(CInt(txtGroupID.Text), CInt(ddlFilterCustomer.SelectedItem.Value))

            cn.Open()
            If Not _Group.HasUser(cn) Then
                _Group.Delete(cn)

                Search(True, True, True, True)
                lblMessage.Text = "Die Gruppe wurde gelöscht."
            Else
                lblMessage.Text = "Die Gruppe kann nicht gelöscht werden, da ihr noch Benutzer zugeordnet sind."
            End If
        Catch ex As Exception
            m_App.WriteErrorText(1, m_User.UserName, "GroupManagement", "lbtnDelete_Click", ex.ToString)

            lblError.Text = ex.Message
            If Not ex.InnerException Is Nothing Then
                lblError.Text &= ": " & ex.InnerException.Message
            End If
        Finally
            If cn.State <> ConnectionState.Closed Then
                cn.Close()
            End If
        End Try
    End Sub

#End Region

    Private Sub btnSuche_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSuche.Click
        Search(True, True, True, True)
    End Sub

    Protected Sub btnAssign_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnAssign.Click
        Dim _item As ListItem
        Dim _coll As New ListItemCollection()

        For Each _item In lstAppUnAssigned.Items
            If _item.Selected = True Then
                _item.Selected = False
                _coll.Add(_item)
            End If
        Next

        For Each _item In _coll
            lstAppAssigned.Items.Add(_item)
            lstAppUnAssigned.Items.Remove(_item)
        Next
    End Sub

    Protected Sub btnUnAssign_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnUnAssign.Click
        Dim _item As ListItem
        Dim _coll As New ListItemCollection()

        For Each _item In lstAppAssigned.Items
            If _item.Selected = True Then
                _item.Selected = False
                _coll.Add(_item)
            End If
        Next

        For Each _item In _coll
            lstAppUnAssigned.Items.Add(_item)
            lstAppAssigned.Items.Remove(_item)
        Next
    End Sub

    Protected Sub btnAssignArchiv_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnAssignArchiv.Click
        Dim _item As ListItem
        Dim _coll As New ListItemCollection()

        For Each _item In lstArchivUnAssigned.Items
            If _item.Selected = True Then
                _item.Selected = False
                _coll.Add(_item)
            End If
        Next

        For Each _item In _coll
            lstArchivAssigned.Items.Add(_item)
            lstArchivUnAssigned.Items.Remove(_item)
        Next
    End Sub

    Protected Sub btnUnAssignArchiv_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnUnAssignArchiv.Click
        Dim _item As ListItem
        Dim _coll As New ListItemCollection()

        For Each _item In lstArchivAssigned.Items
            If _item.Selected = True Then
                _item.Selected = False
                _coll.Add(_item)
            End If
        Next

        For Each _item In _coll
            lstArchivUnAssigned.Items.Add(_item)
            lstArchivAssigned.Items.Remove(_item)
        Next
    End Sub

    Protected Sub btnAssignEmployee_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnAssignEmployee.Click
        Dim _item As ListItem
        Dim _coll As New ListItemCollection()

        For Each _item In lstEmployeeUnAssigned.Items
            If _item.Selected = True Then
                _item.Selected = False
                _coll.Add(_item)
            End If
        Next

        For Each _item In _coll
            lstEmployeeAssigned.Items.Add(_item)
            lstEmployeeUnAssigned.Items.Remove(_item)
        Next
    End Sub

    Protected Sub btnUnAssignEmployee_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnUnAssignEmployee.Click
        Dim _item As ListItem
        Dim _coll As New ListItemCollection()

        For Each _item In lstEmployeeAssigned.Items
            If _item.Selected = True Then
                _item.Selected = False
                _coll.Add(_item)
            End If
        Next

        For Each _item In _coll
            lstEmployeeUnAssigned.Items.Add(_item)
            lstEmployeeAssigned.Items.Remove(_item)
        Next
    End Sub

    Private Sub GridNavigation1_PagerChanged(ByVal PageIndex As Integer) Handles GridNavigation1.PagerChanged
        dgSearchResult.PageIndex = PageIndex
        FillDataGrid()
    End Sub

    Private Sub GridNavigation1_PageSizeChanged() Handles GridNavigation1.PageSizeChanged
        FillDataGrid()
    End Sub

    Private Sub dgSearchResult_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgSearchResult.RowCommand
        Dim index As Integer
        Dim row As GridViewRow
        Dim CtrlLabel As Label


        If e.CommandName = "Edit" Then
            index = Convert.ToInt32(e.CommandArgument)
            row = dgSearchResult.Rows(index)
            CtrlLabel = row.Cells(0).FindControl("lblGroupID")
            EditEditMode(CInt(CtrlLabel.Text))
            dgSearchResult.SelectedIndex = row.RowIndex
        ElseIf e.CommandName = "Del" Then
            index = Convert.ToInt32(e.CommandArgument)
            row = dgSearchResult.Rows(index)
            CtrlLabel = row.Cells(0).FindControl("lblGroupID")
            EditDeleteMode(CInt(CtrlLabel.Text))
            dgSearchResult.SelectedIndex = row.RowIndex
        End If
    End Sub

    Private Sub dgSearchResult_RowEditing(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewEditEventArgs) Handles dgSearchResult.RowEditing

    End Sub

    Private Sub dgSearchResult_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles dgSearchResult.Sorting
        Dim strSort As String = e.SortExpression
        If Not Session("ResultSort") Is Nothing AndAlso Session("ResultSort").ToString = strSort Then
            strSort &= " DESC"
        End If
        Session("ResultSort") = strSort
        FillDataGrid(strSort)
    End Sub

    Private Sub dgSearchResult_PageIndexChanging(ByVal sender As Object, ByVal e As EventArgs) Handles dgSearchResult.PageIndexChanging

    End Sub

    Private Sub dgSearchResult_PageIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles dgSearchResult.PageIndexChanged

    End Sub

    Protected Sub btnEmpty_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnEmpty.Click
        btnSuche_Click(sender, e)
    End Sub
  
End Class