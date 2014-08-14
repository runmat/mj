
Imports CKG.Base.Kernel.Admin
Imports CKG.Base.Kernel.Security
Imports CKG.Base.Kernel.Common.Common
Imports CKG.Base.Business
Imports CKG.Base.Business.HelpProcedures
Imports Telerik.Web.UI
Imports System.Data.SqlClient

Partial Public Class Groupmanagement
    Inherits System.Web.UI.Page
#Region " Membervariables "
    Private m_User As User
    Private m_App As App
    Private m_context As HttpContext = HttpContext.Current
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
                lnkContact.Visible = False
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
        If Not ViewState("ResultSort") Is Nothing Then
            strSort = ViewState("ResultSort").ToString
        End If
        FillDataGrid(strSort)
    End Sub

    Private Sub FillDataGrid(ByVal strSort As String)
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

            'editMessage.Content = TranslateHTML(_Group.Message, TranslationDirection.ReadHTML)
            radMessage.Content = TranslateHTML(_Group.Message, TranslationDirection.ReadHTML)
            txtMessageOld.Text = TranslateHTML(_Group.Message, TranslationDirection.ReadHTML)

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

    End Sub

    Private Sub ClearEdit()
        txtGroupID.Text = "-1"
        txtGroupName.Text = ""
        txtDocuPath.Text = ""
        txtStartMethod.Text = ""
        radMessage.Content = ""
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

        radMessage.Enabled = Not blnLock
        radMessage.BackColor = System.Drawing.Color.FromName(strBackColor)

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
        cbxTeamViewer.Enabled = Not blnLock
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

    Private Sub SearchMode(Optional ByVal blnSearchMode As Boolean = True, Optional ByVal blnNewSearch As Boolean = False)
        'gewünschte Expand/Collapse-Stati für die Seitenabschnitte in hidden fields setzen, werden dann von JQuery ausgewertet
        trRights.Visible = False
        If blnSearchMode Then
            If blnNewSearch Then
                ihExpandstatusSearchFilterArea.Value = "1"
                ihExpandstatusSearchResultArea.Value = "0"
            Else
                ihExpandstatusSearchFilterArea.Value = "0"
                ihExpandstatusSearchResultArea.Value = "1"
            End If
            ihExpandStatusInputArea.Value = "0"
        Else
            ihExpandstatusSearchFilterArea.Value = "0"
            ihExpandstatusSearchResultArea.Value = "0"
            ihExpandStatusInputArea.Value = "1"
        End If
    End Sub

    Private Sub Search(Optional ByVal blnShowDataGrid As Boolean = False, Optional ByVal blnResetSelectedIndex As Boolean = False, Optional ByVal blnResetPageIndex As Boolean = False, Optional ByVal blnClearCache As Boolean = False)
        ClearEdit()
        If blnClearCache Then
            Session("myGroupListView") = Nothing
        End If
        If blnResetSelectedIndex Then dgSearchResult.SelectedIndex = -1
        If blnResetPageIndex Then dgSearchResult.PageIndex = 0
        If m_User.HighestAdminLevel > AdminLevel.Organization Then
            If blnShowDataGrid Then
                FillDataGrid()
                If dgSearchResult.Rows.Count > 0 Then
                    SearchMode()
                Else
                    SearchMode(, True)
                End If
            Else
                SearchMode(, True)
            End If
        Else
            GroupAdminMode()
        End If
    End Sub

    Private Sub Log(ByVal strIdentification As String, ByVal strDescription As String, ByVal tblParameters As DataTable, Optional ByVal strCategory As String = "APP")
        Dim logApp As New CKG.Base.Kernel.Logging.Trace(m_User.App.Connectionstring, m_User.App.SaveLogAccessSAP, m_User.App.LogLevel)

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

    Private Function SetOldLogParameters(ByVal intGroupId As Int32) As DataTable
        Dim cn As SqlClient.SqlConnection
        cn = New SqlClient.SqlConnection(m_User.App.Connectionstring)
        Try

            cn.Open()
            Dim _Group As New Group(intGroupId, cn)

            Dim tblPar = CreateLogTableStructure()

            With tblPar
                .Rows.Add(.NewRow)
                .Rows(.Rows.Count - 1)("Status") = "Alt"
                .Rows(.Rows.Count - 1)("Gruppenname") = _Group.GroupName
                .Rows(.Rows.Count - 1)("Aut.- Recht") = _Group.Authorizationright.ToString
                .Rows(.Rows.Count - 1)("Kunden- Gruppe") = _Group.IsCustomerGroup
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
                    .Rows(.Rows.Count - 1)("Firma") = dvCustomer(dvCustomer.Find(_Group.CustomerId)).Item("CustomerName").ToString
                End If

                Dim dvAppAssigned As DataView = GetAppAssignedView(intGroupId, _Group.CustomerId, cn)
                Dim strAnwendungen As String = ""
                Dim j As Int32
                For j = 0 To dvAppAssigned.Count - 1
                    strAnwendungen &= dvAppAssigned(j)("AppFriendlyName").ToString & vbNewLine
                Next
                .Rows(.Rows.Count - 1)("Anwendungen") = strAnwendungen
                .Rows(.Rows.Count - 1)("Handbuch") = _Group.DocuPath
                .Rows(.Rows.Count - 1)("Startmethode") = _Group.StartMethod
                .Rows(.Rows.Count - 1)("Message") = _Group.Message
                .Rows(.Rows.Count - 1)("MaxReadMessageCount") = _Group.MaxReadMessageCount
            End With
            Return tblPar
        Catch ex As Exception
            m_App.WriteErrorText(1, m_User.UserName, "GroupManagement", "SetOldLogParameters", ex.ToString)

            Dim dt As New DataTable()
            dt.Columns.Add("Fehler beim Erstellen der Log-Parameter", System.Type.GetType("System.String"))
            dt.Rows.Add(dt.NewRow)
            Dim str As String = ex.Message
            If Not ex.InnerException Is Nothing Then
                str &= ": " & ex.InnerException.Message
            End If
            dt.Rows(0)("Fehler beim Erstellen der Log-Parameter") = str
            Return dt
        Finally
            If cn.State <> ConnectionState.Closed Then
                cn.Close()
            End If
        End Try
    End Function

    Private Function SetNewLogParameters() As DataTable
        Try
            Dim tblPar = CreateLogTableStructure()

            With tblPar
                .Rows.Add(.NewRow)
                .Rows(.Rows.Count - 1)("Status") = "Neu"
                .Rows(.Rows.Count - 1)("Gruppenname") = txtGroupName.Text
                .Rows(.Rows.Count - 1)("Aut.- Recht") = ddlAuthorizationright.SelectedItem.Text
                .Rows(.Rows.Count - 1)("Kunden- Gruppe") = cbxIsCustomerGroup.Checked
                .Rows(.Rows.Count - 1)("Firma") = txtCustomer.Text
                Dim _li As ListItem
                Dim strAnwendungen As String = ""
                For Each _li In lstAppAssigned.Items
                    strAnwendungen &= _li.Text & vbNewLine
                Next
                .Rows(.Rows.Count - 1)("Anwendungen") = strAnwendungen
                .Rows(.Rows.Count - 1)("Handbuch") = txtDocuPath.Text
                .Rows(.Rows.Count - 1)("Startmethode") = txtStartMethod.Text
                .Rows(.Rows.Count - 1)("Message") = TranslateHTML(radMessage.Content.Trim, TranslationDirection.SaveHTML)
                .Rows(.Rows.Count - 1)("MaxReadMessageCount") = CInt(txtMaxReadMessageCount.Text)
            End With
            Return tblPar
        Catch ex As Exception
            m_App.WriteErrorText(1, m_User.UserName, "GroupManagement", "SetNewLogParameters", ex.ToString)

            Dim dt As New DataTable()
            dt.Columns.Add("Fehler beim Erstellen der Log-Parameter", System.Type.GetType("System.String"))
            dt.Rows.Add(dt.NewRow)
            Dim str As String = ex.Message
            If Not ex.InnerException Is Nothing Then
                str &= ": " & ex.InnerException.Message
            End If
            dt.Rows(0)("Fehler beim Erstellen der Log-Parameter") = str
            Return dt
        End Try
    End Function

    Private Function CreateLogTableStructure() As DataTable
        Dim tblPar As New DataTable()
        With tblPar
            .Columns.Add("Status", System.Type.GetType("System.String"))
            .Columns.Add("Gruppenname", System.Type.GetType("System.String"))
            .Columns.Add("Aut.- Recht", System.Type.GetType("System.String"))
            .Columns.Add("Kunden- Gruppe", System.Type.GetType("System.Boolean"))
            .Columns.Add("Firma", System.Type.GetType("System.String"))
            .Columns.Add("Anwendungen", System.Type.GetType("System.String"))
            .Columns.Add("Handbuch", System.Type.GetType("System.String"))
            .Columns.Add("Startmethode", System.Type.GetType("System.String"))
            .Columns.Add("Message", System.Type.GetType("System.String"))
            .Columns.Add("MaxReadMessageCount", System.Type.GetType("System.Int32"))
        End With
        Return tblPar
    End Function
#End Region

#Region " Events "
    Private Sub lbtnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lbtnCancel.Click
        Search(True, True)
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
        Dim tblLogParameter As DataTable
        Dim cn As SqlClient.SqlConnection

        If String.IsNullOrEmpty(txtGroupName.Text) Then
            lblError.Text = "Bitte geben Sie einen Gruppennamen an!"
            Exit Sub
        End If

        If Not IsNumeric(txtMaxReadMessageCount.Text) Then
            lblMessage.Text = "Bitte geben Sie einen Zahlenwert für die Häufigkeit ein."
            Exit Sub
        End If
        cn = New SqlClient.SqlConnection(m_User.App.Connectionstring)
        Try

            cn.Open()
            Dim intGroupId As Integer = CInt(txtGroupID.Text)
            Dim strLogMsg As String = "Gruppe anlegen"
            If Not (intGroupId = -1) Then
                strLogMsg = "Gruppe ändern"
                tblLogParameter = SetOldLogParameters(intGroupId)
            End If

            Dim htmlMessage As String = TranslateHTML(radMessage.Content.Trim, TranslationDirection.SaveHTML)

            If htmlMessage.Length > 1500 Then
                lblInfo.Text = "Die Nachricht (inkl. Formatierungen) ist mit akt. '" + htmlMessage.Length.ToString + "' Zeichen zu lang zum Speichern."
                Return
            Else
                lblInfo.Text = String.Empty
            End If



            If txtGroupID.Text = "-1" Then

                'bereits genutzte
                Dim cnGivven As New SqlClient.SqlConnection(m_User.App.Connectionstring)
                cnGivven.Open()

                Dim dtGivvenGroupNameAll As Kernel.GivvenGroupAllList = New Kernel.GivvenGroupAllList(cn, txtCustomerID.Text)
                Dim dvGivvenGroupName As DataView = dtGivvenGroupNameAll.DefaultView
                For intLoop = 0 To dvGivvenGroupName.Count - 1
                    If UCase(txtGroupName.Text) = UCase(dvGivvenGroupName(intLoop)("GroupName")) Then
                        lblError.Text = "Bitte wählen Sie einen anderen Namen für die neue Gruppe!<br />(Der Gruppename wird bereits benutzt.)<br /><br />"
                        Exit For
                    End If
                Next

            End If

            Dim blnNew As Boolean = False
            If CInt(txtGroupID.Text) < 1 Then blnNew = True
            Dim _group As New Group(intGroupId, _
                                                txtGroupName.Text, _
                                                CInt(txtCustomerID.Text), _
                                                txtDocuPath.Text, _
                                                CInt(ddlAuthorizationright.SelectedItem.Value.ToString), _
                                                cbxIsCustomerGroup.Checked, _
                                                blnNew, _
                                                txtStartMethod.Text, _
                                                htmlMessage, _
                                                CInt(txtMaxReadMessageCount.Text), _
                                                cbxTeamViewer.Checked, _
                                                cbxIsServiceGroup.Checked)



            If lblError.Text = "" Then
                _group.Save(cn)
            Else
                Exit Sub
            End If

            If (Not (radMessage.Content = txtMessageOld.Text)) OrElse (Not blnNew) Then
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
            Dim lstAssignedApps As New List(Of String)
            For Each li As ListItem In lstAppAssigned.Items
                lstAssignedApps.Add(li.Value)
            Next
            Dim _assignment As New Kernel.AppAssignments(intGroupId, Kernel.AssignmentType.Group)
            _assignment.Save(dvAppAssigned, lstAssignedApps, cn)

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

            tblLogParameter = SetNewLogParameters()
            Log(_group.GroupId.ToString, strLogMsg, tblLogParameter)

            Search(True, True, , True)
            lblMessage.Text = "Die Änderungen wurden gespeichert."
        Catch ex As Exception
            m_App.WriteErrorText(1, m_User.UserName, "GroupManagement", "lbtnSave_Click", ex.ToString)

            lblError.Text &= ex.Message
            If Not ex.InnerException Is Nothing Then
                lblError.Text &= ": " & ex.InnerException.Message
            End If
            tblLogParameter = New DataTable
            Log(txtGroupID.Text, lblError.Text, tblLogParameter, "ERR")
        Finally
            If cn.State <> ConnectionState.Closed Then
                cn.Close()
            End If
        End Try
    End Sub

    Private Sub lbtnDelete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lbtnDelete.Click
        Dim tblLogParameter As DataTable
        Dim cn As SqlClient.SqlConnection
        cn = New SqlClient.SqlConnection(m_User.App.Connectionstring)
        Try

            Dim _Group As New Group(CInt(txtGroupID.Text), CInt(ddlFilterCustomer.SelectedItem.Value))

            cn.Open()
            tblLogParameter = SetOldLogParameters(_Group.GroupId)
            If Not _Group.HasUser(cn) Then
                _Group.Delete(cn)
                Log(_Group.GroupId.ToString, "Gruppe löschen", tblLogParameter)

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
            tblLogParameter = New DataTable
            Log(txtGroupID.Text, lblError.Text, tblLogParameter, "ERR")
        Finally
            If cn.State <> ConnectionState.Closed Then
                cn.Close()
            End If
        End Try
    End Sub
#End Region

    Private Sub btnSuche_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSuche.Click
        Search(True, True, True, True)
        If dgSearchResult.Rows.Count = 0 Then
            lblError.Text = "Keine Datensätze gefunden."
        End If
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
        If Not ViewState("ResultSort") Is Nothing AndAlso ViewState("ResultSort").ToString = strSort Then
            strSort &= " DESC"
        End If
        ViewState("ResultSort") = strSort
        FillDataGrid(strSort)
    End Sub

    Protected Sub btnEmpty_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnEmpty.Click
        btnSuche_Click(sender, e)
    End Sub

    Protected Sub cbxLevel_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs) Handles cbxLevel.CheckedChanged

        trApp.Visible = Not cbxLevel.Checked
        trArchiv.Visible = Not cbxLevel.Checked
        trMeldung.Visible = Not cbxLevel.Checked
        lbtnSave.Visible = Not cbxLevel.Checked
        lbtnCancel.Visible = Not cbxLevel.Checked
        trRights.Visible = cbxLevel.Checked

        FillApps()
        LoadLevel()

    End Sub

    Private Sub FillApps()
        If cbxLevel.Checked = True Then
            If Not Session("myAppAssigned") Is Nothing Then
                ddlAnwendung.Items.Clear()

                Dim Apps As DataView = CType(Session("myAppAssigned"), DataView)

                If Apps.Count > 0 Then
                    Dim lItem = New ListItem("---Auswahl----", "0")
                    ddlAnwendung.Items.Add(lItem)

                    For i = 0 To Apps.Count - 1
                        lItem = New ListItem(Apps(i)("AppFriendlyName").ToString, Apps(i)("AppID").ToString)
                        ddlAnwendung.Items.Add(lItem)
                    Next
                Else
                    lblError.Text = "Der Gruppe wurden noch keine Anwendungen zugeordnet oder diese wurden noch nicht gespeichert."
                End If
            End If
        End If
    End Sub

    Protected Sub ddlAnwendung_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ddlAnwendung.SelectedIndexChanged

        LoadLevel()
        If ddlAnwendung.SelectedItem.ToString() = "Fahrzeugbestand" Then
            tableAuthLevel.Visible = True
            Image2.Visible = False
        Else
            tableAuthLevel.Visible = False
            Image2.Visible = True
        End If

    End Sub

    Private Sub LoadLevel()
        Dim Filter As String = ""

        Dim dt As New DataTable

        dt.Columns.Add("Name", GetType(System.String))
        dt.Columns.Add("Level", GetType(System.String))
        dt.Columns.Add("Autorisierung", GetType(System.String))


        If ddlAnwendung.SelectedIndex > 0 Then
            Dim LevelAut() As String
            Dim Apps As DataView = CType(Session("myAppAssigned"), DataView)

            Apps.RowFilter = "AppID = " & ddlAnwendung.SelectedValue

            dt.Columns("Name").DefaultValue = ddlAnwendung.SelectedItem.Text

            dt.AcceptChanges()

            Session("Level") = dt

            Dim LevelString As String = GetLevel()

            If String.IsNullOrEmpty(LevelString) = False Then

                'Beinhaltet 2 Arrays: Level-Array und Autorisierungsarray(1 zu 1) getrennt durch |
                LevelAut = Split(LevelString, "|")

                'Level-Array
                Dim Level() As String = Split(LevelAut(0), ",")

                'Autorisieungsarray
                Dim Aut() As String = Split(LevelAut(1), ",")

                Dim dr As DataRow

                'Level aus Level-Array
                For i = 0 To Level.Length - 1
                    dr = dt.NewRow

                    dr("Level") = Level(i) 'Level hinzufügen
                    dr("Autorisierung") = Aut(i) 'Zum Level gehörende Autorisierung hinzufügen

                    dt.Rows.Add(dr)
                Next
            End If
        End If

        gvAutorisierung.DataSource = dt.DefaultView
        gvAutorisierung.DataBind()

        Session("Level") = dt
    End Sub

    Protected Sub ibtNew_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtNew.Click

        LoadLevel()

        If ddlAnwendung.SelectedValue = "0" Then
            lblAutError.Text = "Bitte wählen Sie eine Anwendung aus."
            Exit Sub
        End If

        Dim dt As DataTable = Session("Level")

        Dim assignedApps = CType(Session("myAppAssigned"), DataView).Table
        Dim app = assignedApps.Select("AppID=" & ddlAnwendung.SelectedValue).First()
        Dim maxGroupLevels = CInt(app("MaxLevelsPerGroup"))

        If dt.Rows.Count >= maxGroupLevels Then
            lblAutError.Text = "Maximale Anzahl der Level erreicht."
        Else
            Dim dr As DataRow = dt.NewRow
            dr("Name") = ddlAnwendung.SelectedItem.Text
            dr("Level") = "0"
            dr("Autorisierung") = "1"
            dt.Rows.Add(dr)

            gvAutorisierung.EditIndex = dt.Rows.Count - 1
            gvAutorisierung.DataSource = dt.DefaultView
            gvAutorisierung.DataBind()

            Dim maxLevel = CInt(app("MaxLevel"))
            Dim ddlLevels = CType(gvAutorisierung.Rows(dt.Rows.Count - 1).FindControl("ddlEditLevel"), DropDownList)

            While ddlLevels.Items.Count > 1
                ddlLevels.Items.RemoveAt(1)
            End While

            For i = 1 To maxLevel
                ddlLevels.Items.Add(New ListItem("Level " & i, i))
            Next
        End If
    End Sub

    Protected Sub ddlLevel_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs)

        Dim ddl As DropDownList
        Dim CountLevel As Integer = 0
        Dim SelectedLevel As String
        Dim booErr As Boolean = False

        SelectedLevel = CType(sender, DropDownList).SelectedValue.ToString

        For Each gvRow As GridViewRow In gvAutorisierung.Rows

            ddl = CType(gvRow.FindControl("ddlLevel"), DropDownList)

            If ddl.SelectedValue = "0" Then
                lblAutError.Text = "Bitte treffen Sie eine Auswahl."
                booErr = True
                Exit For
            ElseIf ddl.SelectedValue = SelectedLevel Then
                CountLevel += 1
            End If
        Next

        If booErr = True Then
            lblAutError.Text = "Bitte treffen Sie eine Auswahl."
        ElseIf CountLevel > 1 Then
            lblAutError.Text = "Eintrag bereits vorhanden. Bitte korrigieren Sie Ihre Auswahl."
        Else
            UpdateLevel()
        End If


    End Sub

    Protected Sub ddlAutorisierungChanged(ByVal sender As Object, ByVal e As EventArgs)
        UpdateLevel()
    End Sub

    Private Function GetLevel() As String
        Dim cn = New SqlClient.SqlConnection(m_User.App.Connectionstring)
        Dim da As New SqlClient.SqlDataAdapter()
        da.SelectCommand = New SqlClient.SqlCommand()
        da.SelectCommand.Connection = cn

        Dim SQL As String = ""
        Dim RetValue As String = ""

        SQL = "Select NewLevel from Rights where groupid = " & CInt(txtGroupID.Text) & " and appid = " & CInt(ddlAnwendung.SelectedValue)

        Try
            da.SelectCommand.CommandText = SQL

            Dim dt As New DataTable

            da.Fill(dt)

            If dt.Rows.Count > 0 Then
                RetValue = dt.Rows(0)("NewLevel").ToString
            End If


        Catch ex As Exception


        Finally
            cn.Close()

        End Try

        Return RetValue

    End Function

    Private Sub UpdateLevel()
        Dim appName = ddlAnwendung.SelectedItem.Text

        Dim levels = String.Empty
        Dim autorisierungen = String.Empty

        Dim dt As DataTable = CType(Session("Level"), DataTable).Clone
        For Each dr As GridViewRow In gvAutorisierung.Rows
            Dim level As String = DirectCast(dr.FindControl("litItemLevel"), Literal).Text.Trim
            Dim autorisierung As String = DirectCast(dr.FindControl("ddlItemAutorisierung"), DropDownList).SelectedValue

            levels &= level & ","
            autorisierungen &= autorisierung & ","

            Dim drLevel = dt.NewRow

            drLevel("Name") = appName
            drLevel("Level") = level
            drLevel("Autorisierung") = autorisierung

            dt.Rows.Add(drLevel)
        Next

        levels = levels.TrimEnd(","c)
        autorisierungen = autorisierungen.TrimEnd(","c)

        Dim sqlString = String.Empty
        If (levels & autorisierungen).Length > 0 Then
            sqlString = levels & "|" & autorisierungen
        End If

        Dim cn = New SqlConnection(m_User.App.Connectionstring)
        Try
            cn.Open()

            Dim cmd = cn.CreateCommand()
            cmd.CommandText = "Update Rights set newLevel = '" & sqlString & "' where groupid = " & CInt(txtGroupID.Text) & " and appid = " & CInt(ddlAnwendung.SelectedValue)
            cmd.ExecuteNonQuery()

            Session("Level") = dt

            gvAutorisierung.DataSource = dt.DefaultView
            gvAutorisierung.DataBind()

        Catch ex As Exception
            lblAutError.Text = ex.Message
        Finally
            cn.Close()
        End Try
    End Sub


    Private Sub gvAutorisierung_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvAutorisierung.RowCommand
        Dim levelData = CType(Session("Level"), DataTable)

        If e.CommandName = "Del" Then
            Dim index = CInt(e.CommandArgument)

            gvAutorisierung.DeleteRow(index)

            If (index + 1) > levelData.Rows.Count Then
                gvAutorisierung.DataSource = levelData
                gvAutorisierung.DataBind()
            Else
                levelData.Rows(index).Delete()
                levelData.AcceptChanges()

                gvAutorisierung.DataSource = levelData
                gvAutorisierung.DataBind()

                UpdateLevel()
            End If
        ElseIf e.CommandName = "Add" Then
            Dim index = gvAutorisierung.EditIndex
            If index <> -1 Then
                Dim ddlLevel = CType(gvAutorisierung.Rows(index).FindControl("ddlEditLevel"), DropDownList)

                If ddlLevel.SelectedValue = "0" Then
                    lblAutError.Text = "Bitte treffen Sie eine Auswahl."
                    Return
                End If

                Dim levelRow = levelData.Rows(index)
                levelRow("Level") = ddlLevel.SelectedValue
                levelRow("Autorisierung") = CType(gvAutorisierung.Rows(index).FindControl("ddlEditAutorisierung"), DropDownList).SelectedValue

                gvAutorisierung.EditIndex = -1
                gvAutorisierung.DataSource = levelData
                gvAutorisierung.DataBind()

                UpdateLevel()
            End If
        End If
    End Sub

    Private Sub gvAutorisierung_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs) Handles gvAutorisierung.RowDeleting

    End Sub

    Private Sub Groupmanagement_PreRender(sender As Object, e As System.EventArgs) Handles Me.PreRender
        HelpProcedures.FixedGridViewCols(dgSearchResult)
    End Sub
End Class

