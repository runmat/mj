
Imports CKG.Base.Kernel.Security
Imports CKG.Base.Kernel.Common.Common
Imports CKG.Base.Kernel
Imports CKG
Imports CKG.Services

Public Class UserHistory
    Inherits System.Web.UI.Page

#Region " Vom Web Form Designer generierter Code "
    Protected WithEvents tblEditUser As System.Web.UI.HtmlControls.HtmlTable
    Protected WithEvents tblSearchResult As System.Web.UI.HtmlControls.HtmlTable
    Protected WithEvents dgSearchResult As System.Web.UI.WebControls.DataGrid
    Protected WithEvents cbxTestUser As System.Web.UI.WebControls.CheckBox
    Protected WithEvents cbxCustomerAdmin As System.Web.UI.WebControls.CheckBox
    Protected WithEvents lblLastPwdChange As System.Web.UI.WebControls.Label
    Protected WithEvents cbxPwdNeverExpires As System.Web.UI.WebControls.CheckBox
    Protected WithEvents lblFailedLogins As System.Web.UI.WebControls.Label
    Protected WithEvents cbxAccountIsLockedOut As System.Web.UI.WebControls.CheckBox
    Protected WithEvents trCustomerAdmin As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents trPwdNeverExpires As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents trTestUser As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents trCustomer As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents lblGroup As System.Web.UI.WebControls.Label
    Protected WithEvents lbtnCancel As System.Web.UI.WebControls.LinkButton
    Protected WithEvents Form1 As System.Web.UI.HtmlControls.HtmlForm
    Protected WithEvents cbxOrganizationAdmin As System.Web.UI.WebControls.CheckBox
    Protected WithEvents trOrganization As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents trOrganizationAdministrator As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents trGroup As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents trMail As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents txtUserHistoryID As System.Web.UI.WebControls.TextBox
    Protected WithEvents ddlCustomer As System.Web.UI.WebControls.Label
    Protected WithEvents txtUserName As System.Web.UI.WebControls.Label
    Protected WithEvents txtReference As System.Web.UI.WebControls.Label
    Protected WithEvents ddlGroups As System.Web.UI.WebControls.Label
    Protected WithEvents ddlOrganizations As System.Web.UI.WebControls.Label
    Protected WithEvents txtPassword As System.Web.UI.WebControls.Label
    Protected WithEvents txtCreated As System.Web.UI.WebControls.Label
    Protected WithEvents cbxDeleted As System.Web.UI.WebControls.CheckBox
    Protected WithEvents txtLastChange As System.Web.UI.WebControls.Label
    Protected WithEvents txtLastChanged As System.Web.UI.WebControls.Label
    Protected WithEvents txtLastChangedBy As System.Web.UI.WebControls.Label
    Protected WithEvents txtDeleteDate As System.Web.UI.WebControls.Label
    Protected WithEvents GridNavigation1 As GridNavigation

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
#End Region

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        ' Hier Benutzercode zur Seiteninitialisierung einfügen
        m_User = GetUser(Me)
        AdminAuth(Me, m_User, AdminLevel.Organization)
        GridNavigation1.setGridElment(dgSearchResult)

        Try
            m_App = New Base.Kernel.Security.App(m_User)

            lblError.Text = ""

            If Not IsPostBack Then
                FillForm()
            End If
        Catch ex As Exception
            m_App.WriteErrorText(1, m_User.UserName, "UserHistory", "Page_Load", ex.ToString)

            lblError.Text = ex.ToString
        End Try
    End Sub

#Region " Data and Function "
    Private Sub FillForm()
        txtPassword.Visible = True

        Dim cn As New SqlClient.SqlConnection(m_User.App.Connectionstring)
        cn.Open()
        FillCustomer(cn) 'DropDowns fuer Customer fuellen
        FillGroups(CStr(ddlFilterCustomer.SelectedItem.Value), cn) 'DropDowns fuer Group fuellen
        FillOrganizations(CStr(ddlFilterCustomer.SelectedItem.Value), cn) 'DropDowns fuer Group fuellen

        ddlFilterCustomer.Visible = True 'DropDown zur Customer-Auswahl einblenden
        trMail.Visible = True   'Mailadresse einblenden...

        EditUser.Visible = False 'Editbereich ausblenden
        Result.Visible = False 'Suchergebnis ausblenden
    End Sub

    Private Sub FillGroups(ByVal strCustomerName As String, ByVal cn As SqlClient.SqlConnection)
        Dim dtGroups As New Kernel.HistoryGroupList(strCustomerName, cn)
        FillGroup(ddlFilterGroup, True, dtGroups)
        If ddlFilterGroup.Items.Count = 0 Then
            ddlFilterGroup.Enabled = False
            btnSuche.Enabled = False
        Else
            If ddlFilterGroup.SelectedIndex > 0 Then
                ddlGroups.Text = ddlFilterGroup.SelectedItem.Value
            Else
                ddlGroups.Text = String.Empty
            End If
        End If
    End Sub
    Private Sub FillGroup(ByVal ddl As DropDownList, ByVal blnAllNone As Boolean, ByVal dtgroups As Kernel.HistoryGroupList)
        If blnAllNone Then dtgroups.AddAllNone(True, True)
        With ddl
            .Items.Clear()
            Dim dv As DataView = dtgroups.DefaultView
            dv.Sort = "GroupName"
            .DataSource = dv
            .DataTextField = "GroupName"
            .DataValueField = "GroupName"
            .DataBind()

            If .Items.Count <> 0 Then
                If blnAllNone Then
                    Dim _li As ListItem = .Items.FindByValue(" - alle - ")
                    If Not _li Is Nothing Then
                        _li.Selected = True
                    End If
                End If
            End If
        End With
    End Sub

    Private Sub FillOrganizations(ByVal strCustomerName As String, ByVal cn As SqlClient.SqlConnection)
        Dim dtOrganizations As New Kernel.HistoryOrganizationList(strCustomerName, cn)
        FillOrganization(ddlFilterOrganization, True, dtOrganizations)
    End Sub
    Private Sub FillOrganization(ByVal ddl As DropDownList, ByVal blnAllNone As Boolean, ByVal dtOrganizations As Kernel.HistoryOrganizationList)
        If blnAllNone Then dtOrganizations.AddAllNone(True, True)
        With ddl
            .Items.Clear()
            Dim dv As DataView = dtOrganizations.DefaultView
            dv.Sort = "OrganizationName"
            .DataSource = dv
            .DataTextField = "OrganizationName"
            .DataValueField = "OrganizationName"
            .DataBind()

            If blnAllNone Then .Items.FindByValue(" - alle - ").Selected = True

        End With
    End Sub

    Private Sub FillCustomer(ByVal cn As SqlClient.SqlConnection)
        Dim dtCustomers As New Kernel.HistoryCustomerList(cn)
        dtCustomers.AddAllNone(True, True)
        With ddlFilterCustomer
            Dim dv As DataView = dtCustomers.DefaultView
            dv.Sort = "Customername"
            .DataSource = dv
            .DataTextField = "Customername"
            .DataValueField = "Customername"
            .DataBind()
            .Items.FindByValue(m_User.Customer.CustomerName).Selected = True
        End With
    End Sub

    Private Sub FillDataGrid()
        Dim strSort As String = "UserName"
        If Not ViewState("ResultSort") Is Nothing Then
            strSort = ViewState("ResultSort").ToString
        End If
        FillDataGrid(strSort)
    End Sub
    Private Sub FillDataGrid(ByVal strSort As String)
        Result.Visible = True

        Dim dvUser As DataView

        Dim cn As New SqlClient.SqlConnection(m_User.App.Connectionstring)
        cn.Open()

        Dim intTestUsers As Integer = 0
        If rbTestUserTest.Checked Then
            intTestUsers = 1
        End If
        If rbTestUserProd.Checked Then
            intTestUsers = 2
        End If
        Dim dtUser As New Kernel.HistoryUserList(txtFilterUserName.Text, _
                                                CStr(ddlFilterCustomer.SelectedItem.Value), _
                                               CStr(ddlFilterGroup.SelectedItem.Value), _
                                               CStr(ddlFilterOrganization.SelectedItem.Value), _
                                               intTestUsers, _
                                               cn)
        dvUser = dtUser.DefaultView

        Session("myUserListView") = dvUser

        dvUser.Sort = strSort
        If dvUser.Count > dgSearchResult.PageSize Then
            dgSearchResult.PagerStyle.Visible = True
        Else
            dgSearchResult.PagerStyle.Visible = False
        End If

        If dvUser.Count > 0 Then
            With dgSearchResult
                .DataSource = dvUser
                .DataBind()
            End With
            GridNavigation1.setGridElment(dgSearchResult)
        Else
            Result.Visible = False
        End If
    End Sub

    Private Function FillEdit(ByVal intUserHistoryId As Integer) As Boolean
        SearchMode(False)
        Dim _User As New HistoryUser(intUserHistoryId, m_User.App.Connectionstring)
        txtUserHistoryID.Text = _User.UserHistoryID.ToString
        txtUserName.Text = _User.UserName
        txtReference.Text = _User.Reference
        cbxTestUser.Checked = _User.IsTestUser
        cbxCustomerAdmin.Checked = _User.CustomerAdmin
        ddlCustomer.Text = _User.CustomerName
        ddlGroups.Text = _User.GroupName
        ddlOrganizations.Text = _User.OrganizationName
        lblLastPwdChange.Text = String.Format("{0:dd.MM.yy}", _User.LastPasswordChange)
        cbxPwdNeverExpires.Checked = _User.PasswordNeverExpires
        lblFailedLogins.Text = _User.FailedLogins.ToString
        cbxAccountIsLockedOut.Checked = _User.AccountIsLockedOut
        txtPassword.Text = _User.Password
        txtCreated.Text = String.Format("{0:dd.MM.yy}", _User.Created)
        cbxDeleted.Checked = _User.Deleted
        txtLastChange.Text = _User.LastChange
        txtLastChanged.Text = String.Format("{0:dd.MM.yy}", _User.LastChanged)
        txtLastChangedBy.Text = _User.LastChangedBy
        txtDeleteDate.Text = String.Format("{0:dd.MM.yy}", _User.DeleteDate)
        If txtDeleteDate.Text = "01.01.01" Then
            txtDeleteDate.Text = ""
        End If
    End Function

    Private Sub EditEditMode(ByVal intUserHistoryID As Integer)
        FillEdit(intUserHistoryID)
    End Sub

    Private Sub SearchMode(Optional ByVal blnSearchMode As Boolean = True)
        EditUser.Visible = Not blnSearchMode
        tblSearch.Visible = blnSearchMode
        Result.Visible = blnSearchMode
    End Sub

    Private Sub Search(Optional ByVal blnRefillDataGrid As Boolean = False, Optional ByVal blnResetSelectedIndex As Boolean = False, Optional ByVal blnResetPageIndex As Boolean = False, Optional ByVal blnClearCache As Boolean = False)
        SearchMode(True)
        If blnClearCache Then
            Session.Remove("myUserListView")
        End If
        If blnResetSelectedIndex Then dgSearchResult.SelectedIndex = -1
        If blnResetPageIndex Then dgSearchResult.CurrentPageIndex = 0
        If blnRefillDataGrid Then FillDataGrid()
    End Sub

#End Region

#Region " Events "
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
        End If
    End Sub

    Private Sub dgSearchResult_PageIndexChanged(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles dgSearchResult.PageIndexChanged
        dgSearchResult.CurrentPageIndex = e.NewPageIndex
        FillDataGrid()
    End Sub

    Private Sub lbtnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lbtnCancel.Click
        Search(, True)
    End Sub

    Private Sub ddlFilterCustomer_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ddlFilterCustomer.SelectedIndexChanged
        Dim strCustomerName As String = CStr(ddlFilterCustomer.SelectedItem.Value)
        Dim cn As New SqlClient.SqlConnection(m_User.App.Connectionstring)
        cn.Open()
        If Not strCustomerName = " - alle - " And Not strCustomerName = " - keine - " Then
            ddlCustomer.Text = strCustomerName
            FillGroups(strCustomerName, cn)
            FillOrganizations(strCustomerName, cn)
        Else
            Dim dtGroups As New Kernel.HistoryGroupList(strCustomerName, cn)
            FillGroup(ddlFilterGroup, True, dtGroups)
            Dim dtOrganizations As New Kernel.HistoryOrganizationList(strCustomerName, cn)
            FillOrganization(ddlFilterOrganization, True, dtOrganizations)
        End If
        Result.Visible = False
    End Sub

    Private Sub btnSuche_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSuche.Click
        Search(True, True, True, True)
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

    Private Sub GridNavigation1_PagerChanged(ByVal PageIndex As Integer) Handles GridNavigation1.PagerChanged
        dgSearchResult.CurrentPageIndex = PageIndex
        FillDataGrid()
    End Sub

#End Region

End Class