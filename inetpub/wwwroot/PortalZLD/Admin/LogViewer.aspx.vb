Imports CKG.Base.Kernel.Admin
Imports CKG.Base.Kernel.Security
Imports CKG.Base.Kernel.Common.Common
Imports CKG.Base.Kernel
Imports CKG

Public Class LogViewer
    Inherits System.Web.UI.Page

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

    Private m_context As HttpContext = HttpContext.Current
    Private m_User As User
    Private m_App As App

    Private m_blnShowDetails() As Boolean

    Protected WithEvents DataGrid1 As System.Web.UI.WebControls.DataGrid
    Protected WithEvents Label2 As System.Web.UI.WebControls.Label
    Protected WithEvents TblLog As System.Web.UI.HtmlControls.HtmlTable
    Protected WithEvents lblDownloadTip As System.Web.UI.WebControls.Label
    Protected WithEvents lnkExcel As System.Web.UI.WebControls.HyperLink
    Protected WithEvents GridNavigation1 As Global.CKG.PortalZLD.GridNavigation

#Region "Events"

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        m_User = GetUser(Me)
        AdminAuth(Me, m_User, AdminLevel.Organization)

        GetAppIDFromQueryString(Me)
        GridNavigation1.setGridElment(dgSearchResult)
        Try
            m_App = New App(m_User)

            If Not IsPostBack Then
                lblError.Text = ""

                FillForm()
                TblLog.Visible = False
                trGruppe.Visible = False
                trOrganisation.Visible = False
            End If

            ReDim m_blnShowDetails(DataGrid1.PageSize)
            Dim i As Int32
            For i = 0 To DataGrid1.PageSize - 1
                m_blnShowDetails(i) = False
            Next
        Catch ex As Exception
            m_App.WriteErrorText(1, m_User.UserName, "LogViewer", "Page_Load", ex.ToString)

            lblError.Text = ex.ToString
            lblError.Visible = True
        End Try
    End Sub

    Private Sub DataGrid1_SortCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs) Handles DataGrid1.SortCommand
        ShrinkGrid()
        FillGrid(DataGrid1.CurrentPageIndex, e.SortExpression)
    End Sub

    Private Sub DataGrid1_PageIndexChanged(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles DataGrid1.PageIndexChanged
        ShrinkGrid()
        FillGrid(e.NewPageIndex)
    End Sub

    Private Sub DataGrid1_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles DataGrid1.ItemCommand
        If e.CommandSource.ToString = "System.Web.UI.WebControls.ImageButton" Then
            Dim item As DataGridItem
            Dim cell As TableCell
            Dim checkbox As CheckBox
            Dim control As Control

            For Each item In DataGrid1.Items
                cell = item.Cells(0)
                For Each control In cell.Controls
                    If TypeOf control Is CheckBox Then
                        checkbox = CType(control, CheckBox)
                        If checkbox.Checked Then
                            m_blnShowDetails(item.ItemIndex) = checkbox.Checked
                        End If
                    End If
                Next
            Next

            m_blnShowDetails(e.Item.ItemIndex) = Not m_blnShowDetails(e.Item.ItemIndex)
            FillGrid(DataGrid1.CurrentPageIndex)
        End If
    End Sub

    Private Sub btnSuche_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSuche.Click
        If Not Me.txtFilterUserName.Visible Then
            ClearEdit()
            txtFilterUserName.Text = "*"
        Else
            'Dim _context As HttpContext = HttpContext.Current
            '_context.Cache.Remove("myUserListView")
            Session.Remove("myUserListView")
            dgSearchResult.SelectedIndex = -1
            dgSearchResult.CurrentPageIndex = 0
            FillDataGrid()
        End If
        Me.TblLog.Visible = False
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
            FillEdit(e.Item.Cells(0).Text, CType(e.Item.Cells(1).Controls(0), LinkButton).Text)
            dgSearchResult.SelectedIndex = e.Item.ItemIndex
        End If
    End Sub

    Private Sub dgSearchResult_PageIndexChanged(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles dgSearchResult.PageIndexChanged
        Me.dgSearchResult.CurrentPageIndex = e.NewPageIndex
        FillDataGrid()
    End Sub

    Private Sub lbcreate_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lbCreate.Click
        lblError.Text = "Dieses Logging ist veraltet. Bitte verwenden Sie die Logs aus MySQL."
    End Sub

    Private Sub ddlFilterCustomer_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ddlFilterCustomer.SelectedIndexChanged
        Dim cn As New SqlClient.SqlConnection(m_User.App.Connectionstring)
        cn.Open()
        FillGroups(CInt(ddlFilterCustomer.SelectedItem.Value), cn) 'DropDowns fuer Group fuellen
        FillOrganizations(CInt(ddlFilterCustomer.SelectedItem.Value), cn)
        FillAction(cn)
    End Sub

    Protected Sub lnkCreateExcel1_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lnkCreateExcel1.Click
        Dim reportExcel As New DataTable()
        Dim dvUser As DataView

        dvUser = CType(Session("myUserListView"), DataView)
        If dvUser.Count > 0 Then
            reportExcel = dvUser.Table

            Try
                Dim excelFactory As New DocumentGeneration.ExcelDocumentFactory()
                Dim strFileName As String = Format(Now, "yyyyMMdd_HHmmss_") & m_User.UserName
                excelFactory.CreateDocumentAndSendAsResponse(strFileName, reportExcel, Me)
            Catch ex As Exception
                lblError.Text = ex.Message
            End Try

        End If
    End Sub

#End Region

#Region "Methods"

    Private Sub FillGrid(ByVal intPageIndex As Int32, Optional ByVal strSort As String = "")
        TblLog.Visible = False
        NavExcel.Visible = False
        lblError.Text = "Keine Daten zur Anzeige gefunden."
        lblError.Visible = True
    End Sub

    Private Sub ShrinkGrid()
        Dim item As DataGridItem
        Dim cell As TableCell
        Dim checkbox As CheckBox
        Dim control As Control

        For Each item In DataGrid1.Items
            cell = item.Cells(0)
            For Each control In cell.Controls
                If TypeOf control Is CheckBox Then
                    checkbox = CType(control, CheckBox)
                    checkbox.Checked = False
                End If
            Next
        Next
    End Sub

    Private Sub FillForm()
        Dim cn As New SqlClient.SqlConnection(m_User.App.Connectionstring)
        cn.Open()
        FillCustomer(cn) 'DropDowns fuer Customer fuellen
        FillGroups(CInt(ddlFilterCustomer.SelectedItem.Value), cn) 'DropDowns fuer Group fuellen
        FillOrganizations(CInt(ddlFilterCustomer.SelectedItem.Value), cn)
        FillAction(cn)

        If m_User.HighestAdminLevel = AdminLevel.Master Then
            'Wenn DAD-SuperUser:
            lblCustomer.Visible = False 'Label mit Customer-Namen ausblenden
            ddlFilterCustomer.Visible = True 'DropDown zur Customer-Auswahl einblenden
        Else
            'Wenn nicht DAD-Super-User:
            lblCustomer.Text = m_User.Customer.CustomerName 'Customer des angemeldeten Benutzers
            dgSearchResult.Columns(5).Visible = False 'Spalte "Test-Zugang" ausblenden
            If Not m_User.IsCustomerAdmin Then
                'Wenn nicht Customer-Admin:
                dgSearchResult.Columns(4).Visible = False 'Spalte "Customer-Admin" ausblenden
                If m_User.Groups.Count > 0 Then lblGroup.Text = m_User.Groups(0).GroupName 'Gruppe des angemeldeten Benutzers
                lblGroup.Visible = True 'Label mit Gruppen-Namen einblenden
                ddlFilterGroup.Visible = False 'DropDown zur Gruppenauswahl ausblenden
            End If
        End If

        Result.Visible = False 'Suchergebnis ausblenden

        txtVonDatum_CalendarExtender.SelectedDate = Today
        'txtVonDatum.Text = txtVonDatum_CalendarExtender.SelectedDate.ToShortDateString

        txtBisDatum_CalendarExtender.SelectedDate = Today
        'txtBisDatum.Text = txtBisDatum_CalendarExtender.SelectedDate.ToShortDateString
    End Sub

    Private Sub FillGroups(ByVal intCustomerID As Integer, ByVal cn As SqlClient.SqlConnection)
        Dim dtGroups As New Kernel.GroupList(intCustomerID, cn, m_User.Customer.AccountingArea)
        FillGroup(ddlFilterGroup, True, dtGroups)
    End Sub

    Private Sub FillGroup(ByVal ddl As DropDownList, ByVal blnAllNone As Boolean, ByVal dtgroups As Kernel.GroupList)
        If blnAllNone Then dtgroups.AddAllNone(True, False)
        With ddl
            .Items.Clear()
            Dim dv As DataView = dtgroups.DefaultView
            dv.Sort = "GroupName"
            .DataSource = dv
            .DataTextField = "GroupName"
            .DataValueField = "GroupID"
            .DataBind()
            'If m_User.Groups.HasGroups Then
            '    Dim _li As ListItem = .Items.FindByValue(m_User.Groups(0).GroupId.ToString)
            '    If Not _li Is Nothing Then _li.Selected = True
            'Else
            '    If blnAllNone Then .Items.FindByValue("-1").Selected = True
            'End If
            If blnAllNone Then .Items.FindByValue("0").Selected = True
        End With
    End Sub

    Private Sub FillOrganizations(ByVal intCustomerID As Integer, ByVal cn As SqlClient.SqlConnection)
        Dim dtOrganizations As New OrganizationList(intCustomerID, cn, m_User.Customer.AccountingArea)
        FillOrganization(ddlFilterOrganization, True, dtOrganizations)
    End Sub

    Private Sub FillOrganization(ByVal ddl As DropDownList, ByVal blnAllNone As Boolean, ByVal dtOrganizations As OrganizationList)
        If blnAllNone Then dtOrganizations.AddAllNone(True, False)
        With ddl
            .Items.Clear()
            Dim dv As DataView = dtOrganizations.DefaultView
            dv.Sort = "OrganizationName"
            .DataSource = dv
            .DataTextField = "OrganizationName"
            .DataValueField = "OrganizationID"
            .DataBind()

            If blnAllNone Then .Items.FindByValue("0").Selected = True
        End With
    End Sub

    Private Sub FillCustomer(ByVal cn As SqlClient.SqlConnection)
        Dim dtCustomers As Kernel.CustomerList
        dtCustomers = New Kernel.CustomerList(m_User.Customer.AccountingArea, cn)

        dtCustomers.AddAllNone(True, False)
        With ddlFilterCustomer
            Dim dv As DataView = dtCustomers.DefaultView
            dv.Sort = "Customername"
            .DataSource = dv
            .DataTextField = "Customername"
            .DataValueField = "CustomerID"
            .DataBind()
            .Items.FindByValue(m_User.Customer.CustomerId.ToString).Selected = True
        End With
    End Sub

    Private Sub FillDataGrid()
        Dim strSort As String = "UserID"
        If Not ViewState("ResultSort") Is Nothing Then
            strSort = ViewState("ResultSort").ToString
        End If
        FillDataGrid(strSort)

    End Sub

    Private Sub FillDataGrid(ByVal strSort As String)
        Me.Result.Visible = True

        Dim _context As HttpContext = HttpContext.Current
        Dim dvUser As DataView
        'If Not _context.Cache("myUserListView") Is Nothing Then
        '    dvUser = CType(_context.Cache("myUserListView"), DataView)
        If Not Session("myUserListView") Is Nothing Then
            dvUser = CType(Session("myUserListView"), DataView)
        Else
            Dim cn As New SqlClient.SqlConnection(m_User.App.Connectionstring)
            cn.Open()

            Dim dtUser As New Kernel.UserList(txtFilterUserName.Text, _
                                                   CInt(ddlFilterCustomer.SelectedItem.Value), _
                                                   CInt(ddlFilterGroup.SelectedItem.Value), _
                                                   CInt(ddlFilterOrganization.SelectedItem.Value), _
                                                   cn, _
                                                   False, _
                                                   -1, _
                                                   m_User.Customer.AccountingArea)
            dvUser = dtUser.DefaultView
            '_context.Cache.Insert("myUserListView", dvUser, Nothing, DateTime.Now.AddMinutes(20), TimeSpan.Zero)
            Session("myUserListView") = dvUser
        End If

        dvUser.Sort = strSort
        If dvUser.Count > dgSearchResult.PageSize Then
            dgSearchResult.PagerStyle.Visible = True
        Else
            dgSearchResult.PagerStyle.Visible = False
        End If

        With dgSearchResult
            .DataSource = dvUser
            .DataBind()
        End With
        NavExcel.Visible = True

    End Sub

    Private Sub FillAction(ByVal cn As SqlClient.SqlConnection)
        Dim dt As ApplicationList = Nothing
        Select Case m_User.HighestAdminLevel
            Case AdminLevel.Master
                If Not ddlFilterCustomer.SelectedIndex = 0 OrElse Not ddlFilterCustomer.SelectedIndex = 1 Then
                    dt = New ApplicationList(CInt(ddlFilterCustomer.SelectedValue), cn)
                    dt.GetAssigned()
                Else
                    dt = New ApplicationList(cn)
                End If
            Case AdminLevel.FirstLevel
                If Not ddlFilterCustomer.SelectedIndex = 0 OrElse Not ddlFilterCustomer.SelectedIndex = 1 Then
                    dt = New ApplicationList(CInt(ddlFilterCustomer.SelectedValue), cn)
                    dt.GetAssigned()
                Else
                    dt = New ApplicationList(cn)
                End If
            Case AdminLevel.Customer
                dt = New ApplicationList(m_User.Customer.CustomerId, cn)
                dt.GetAssigned()
            Case AdminLevel.Organization
                dt = New ApplicationList(m_User.Groups(0).GroupId, m_User.Customer.CustomerId, cn)
                dt.GetAssigned()
        End Select

        If dt.Rows.Count = 0 Then
            ddlAction.Items.Clear()
            lbCreate.Enabled = False
            Exit Sub
        Else
            lbCreate.Enabled = True
        End If

        Dim str(7) As String
        If m_User.HighestAdminLevel = AdminLevel.Master Then
            str(0) = "Admin - Anwendungsverwaltung"
        Else
            str(0) = "-"
        End If
        If m_User.HighestAdminLevel = AdminLevel.Master Then
            str(1) = "Admin - Spaltenübersetzungen"
        Else
            str(1) = "-"
        End If
        If m_User.HighestAdminLevel >= AdminLevel.Customer Then
            str(2) = "Admin - Kundenverwaltung"
        Else
            str(2) = "-"
        End If
        If m_User.HighestAdminLevel >= AdminLevel.Organization Then
            str(3) = "Admin - Gruppenverwaltung"
            str(4) = "Admin - Benutzerverwaltung"
            str(5) = "Admin - Organisationsverwaltung"
        Else
            str(3) = "-"
            str(4) = "-"
            str(5) = "-"
        End If
        str(6) = ""
        str(7) = "Admin - Kennwortänderung"

        Dim strI As String
        Dim dr As DataRow
        For Each strI In str
            If strI <> "-" Then
                dr = dt.NewRow
                dr("AppFriendlyName") = strI
                If strI = "" Then
                    dr("AppType") = "None"
                Else
                    dr("AppType") = "Admin"
                End If
                dt.Rows.Add(dr)
            End If
        Next

        dt.DefaultView.Sort = "AppFriendlyName"
        dt.DefaultView.RowFilter = "AppType='Change'"
        With ddlAction
            .DataTextField = "AppFriendlyName"
            .DataValueField = "AppFriendlyName"
            .DataSource = dt.DefaultView
            .DataBind()
        End With
    End Sub

    Private Sub SearchMode(Optional ByVal blnSearchMode As Boolean = True)
        With Me
            .lblUserName.Visible = Not blnSearchMode
            .txtFilterUserName.Visible = blnSearchMode
            If blnSearchMode Then
                .btnSuche.Text = ">> Benutzer suchen"
            Else
                .btnSuche.Text = ">> Deselektieren"
                .Result.Visible = blnSearchMode
            End If
        End With
    End Sub

    Private Function FillEdit(ByVal strId As String, ByVal strName As String) As Boolean
        With Me
            .txtUserID.Text = strId
            .lblUserName.Text = strName
        End With
        SearchMode(False)
    End Function

    Private Sub ClearEdit()
        With Me
            .txtUserID.Text = "-1"
            .lblUserName.Text = ""
        End With
        SearchMode(True)
    End Sub

#End Region

End Class

