
Imports CKG.Base.Kernel.Security.Crypto
Imports CKG.Base.Kernel.Security
Imports CKG.Base.Kernel.Common.Common
Imports CKG.Portal.PageElements

Public Class UserHistory
    Inherits System.Web.UI.Page

    Protected WithEvents ucStyles As Styles

#Region " Vom Web Form Designer generierter Code "
    Protected WithEvents lblError As System.Web.UI.WebControls.Label
    Protected WithEvents ddlFilterGroup As System.Web.UI.WebControls.DropDownList
    Protected WithEvents tblEditUser As System.Web.UI.HtmlControls.HtmlTable
    Protected WithEvents tblSearchResult As System.Web.UI.HtmlControls.HtmlTable
    Protected WithEvents dgSearchResult As System.Web.UI.WebControls.DataGrid
    Protected WithEvents lblCustomer As System.Web.UI.WebControls.Label
    Protected WithEvents trSearchResult As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents trEditUser As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents lblMessage As System.Web.UI.WebControls.Label
    Protected WithEvents cbxTestUser As System.Web.UI.WebControls.CheckBox
    Protected WithEvents cbxCustomerAdmin As System.Web.UI.WebControls.CheckBox
    Protected WithEvents lblLastPwdChange As System.Web.UI.WebControls.Label
    Protected WithEvents cbxPwdNeverExpires As System.Web.UI.WebControls.CheckBox
    Protected WithEvents lblFailedLogins As System.Web.UI.WebControls.Label
    Protected WithEvents cbxAccountIsLockedOut As System.Web.UI.WebControls.CheckBox
    Protected WithEvents trCustomerAdmin As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents trPwdNeverExpires As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents trTestUser As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents trSearch As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents ddlFilterCustomer As System.Web.UI.WebControls.DropDownList
    Protected WithEvents trCustomer As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents lblGroup As System.Web.UI.WebControls.Label
    Protected WithEvents trSearchSpacer As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents lbtnCancel As System.Web.UI.WebControls.LinkButton
    Protected WithEvents Form1 As System.Web.UI.HtmlControls.HtmlForm
    Protected WithEvents lblOrganization As System.Web.UI.WebControls.Label
    Protected WithEvents ddlFilterOrganization As System.Web.UI.WebControls.DropDownList
    Protected WithEvents cbxOrganizationAdmin As System.Web.UI.WebControls.CheckBox
    Protected WithEvents trOrganization As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents trOrganizationAdministrator As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents trGroup As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents trSelectOrganization As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents trMail As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents lnkExcel As System.Web.UI.WebControls.HyperLink
    Protected WithEvents txtFilterUserName As System.Web.UI.WebControls.TextBox
    Protected WithEvents btnSuche As System.Web.UI.WebControls.LinkButton
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
    Protected WithEvents rbTestUserAll As System.Web.UI.WebControls.RadioButton
    Protected WithEvents rbTestUserProd As System.Web.UI.WebControls.RadioButton
    Protected WithEvents rbTestUserTest As System.Web.UI.WebControls.RadioButton
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
#End Region

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        ' Hier Benutzercode zur Seiteninitialisierung einfügen
        m_User = GetUser(Me)
        ucHeader.InitUser(m_User)
        ucStyles.TitleText = "Benutzerverwaltung"
        AdminAuth(Me, m_User, AdminLevel.Organization)

        Try
            m_App = New Base.Kernel.Security.App(m_User)

            If Not IsPostBack Then
                lblError.Text = ""

                FillForm()
            End If
        Catch ex As Exception
            m_App.WriteErrorText(1, m_User.UserName, "UserHistory", "Page_Load", ex.ToString)

            lblError.Text = ex.ToString
            lblError.Visible = True
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

        lblCustomer.Visible = False 'Label mit Customer-Namen ausblenden
        ddlFilterCustomer.Visible = True 'DropDown zur Customer-Auswahl einblenden
        trMail.Visible = True   'Mailadresse einblenden...

        trEditUser.Visible = False 'Editbereich ausblenden
        trSearchResult.Visible = False 'Suchergebnis ausblenden
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
            'If m_User.Groups.HasGroups Then
            '    Dim _li As ListItem = .Items.FindByValue(m_User.Groups(0).GroupName)
            '    If Not _li Is Nothing Then _li.Selected = True
            'Else
            If .Items.Count <> 0 Then
                If blnAllNone Then
                    Dim _li As ListItem = .Items.FindByValue(" - alle - ")
                    If Not _li Is Nothing Then
                        _li.Selected = True
                    End If
                End If
            End If
            'End If
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
            'If IsNumeric(m_User.Organization.OrganizationId) Then
            '    Dim _li As ListItem = .Items.FindByValue(m_User.Organization.OrganizationName)
            '    If Not (_li Is Nothing) Then
            '        _li.Selected = True
            '    End If
            'Else
            If blnAllNone Then .Items.FindByValue(" - alle - ").Selected = True
            'End If
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
        trSearchResult.Visible = True

        Dim _context As HttpContext = HttpContext.Current
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
        ' _context.Cache.Insert("myUserListView", dvUser, Nothing, DateTime.Now.AddMinutes(20), TimeSpan.Zero)
        Session("myUserListView") = dvUser
        'End If

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
        Else
            trSearchResult.Visible = False
            trSearchSpacer.Visible = False
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
        trEditUser.Visible = Not blnSearchMode
        trSearch.Visible = blnSearchMode
        trSearchSpacer.Visible = blnSearchMode
        trSearchResult.Visible = blnSearchMode
        lbtnCancel.Visible = Not blnSearchMode
    End Sub

    Private Sub Search(Optional ByVal blnRefillDataGrid As Boolean = False, Optional ByVal blnResetSelectedIndex As Boolean = False, Optional ByVal blnResetPageIndex As Boolean = False, Optional ByVal blnClearCache As Boolean = False)
        SearchMode()
        If blnClearCache Then
            'Dim _context As HttpContext = HttpContext.Current
            '_context.Cache.Remove("myUserListView")
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
        trSearchResult.Visible = False
        trSearchSpacer.Visible = False
    End Sub

    Private Sub btnSuche_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSuche.Click
        Search(True, True, True, True)

        Dim _context As HttpContext = HttpContext.Current
        Dim dvUser As DataView
        Dim tableExport As New DataTable()

        Dim strFileName As String = Format(Now, "yyyyMMdd_HHmmss_") & m_User.UserName & ".xls"
        Dim showExcel As Boolean
        Dim customerList As String

        lnkExcel.Visible = False
        showExcel = False
        customerList = ConfigurationManager.AppSettings("ShowExcelLinkUserDownload").ToString  'Liste aller Kundennummern, für die der Excel-Download sichtbar sein soll...

        showExcel = True

        If (showExcel = True) Then
            dvUser = CType(Session("myUserListView"), DataView)
            If dvUser.Count > 0 Then
                tableExport = dvUser.Table

                Dim objExcelExport As New Base.Kernel.Excel.ExcelExport()

                Try
                    Base.Kernel.Excel.ExcelExport.WriteExcel(tableExport, ConfigurationManager.AppSettings("ExcelPath") & strFileName)
                Catch ex As Exception
                End Try
                lnkExcel.NavigateUrl = "/Portal/Temp/Excel/" & strFileName
                lnkExcel.Visible = True
            End If
        End If

    End Sub
#End Region
End Class

' ************************************************
' $History: UserHistory.aspx.vb $
' 
' *****************  Version 3  *****************
' User: Rudolpho     Date: 6.01.09    Time: 11:45
' Updated in $/CKAG/admin
' ITA 2503  Cache durch Session ersetzt
' 
' *****************  Version 2  *****************
' User: Rudolpho     Date: 11.04.08   Time: 15:47
' Updated in $/CKAG/admin
' Migration
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 4.04.08    Time: 14:47
' Created in $/CKAG/admin
' 
' *****************  Version 8  *****************
' User: Uha          Date: 22.05.07   Time: 14:23
' Updated in $/CKG/Admin/AdminWeb
' Nacharbeiten + Bereinigungen
' 
' *****************  Version 7  *****************
' User: Uha          Date: 15.05.07   Time: 15:29
' Updated in $/CKG/Admin/AdminWeb
' Änderungen aus StartApplication vom 11.05.2007
' 
' *****************  Version 6  *****************
' User: Uha          Date: 13.03.07   Time: 10:53
' Updated in $/CKG/Admin/AdminWeb
' History-Eintrag vorbereitet
' 
' ************************************************
