
Imports CKG.Base.Kernel.Admin
Imports CKG.Base.Kernel.Security.Crypto
Imports CKG.Base.Kernel.Security
Imports CKG.Base.Kernel.Common.Common
Imports CKG.Portal.PageElements

Public Structure Appl
    Dim Name As String
    Dim FriendlyName As String
    Dim Id As Integer
End Structure

Public Class UserManagement
    Inherits System.Web.UI.Page

    Private objSuche As Base.Kernel.Common.Search
    Protected WithEvents ucStyles As Styles
    Private Const CONST_LOESCHKENNZEICHEN As String = "X"

#Region " Vom Web Form Designer generierter Code "
    Protected WithEvents txtFilterUserName As System.Web.UI.WebControls.TextBox
    Protected WithEvents ddlFilterGroup As System.Web.UI.WebControls.DropDownList
    Protected WithEvents ddlCustomer As System.Web.UI.WebControls.DropDownList
    Protected WithEvents tblEditUser As System.Web.UI.HtmlControls.HtmlTable
    Protected WithEvents tblSearchResult As System.Web.UI.HtmlControls.HtmlTable
    Protected WithEvents dgSearchResult As System.Web.UI.WebControls.DataGrid
    Protected WithEvents lblCustomer As System.Web.UI.WebControls.Label
    Protected WithEvents trSearchResult As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents trEditUser As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents txtUserName As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtUserID As System.Web.UI.WebControls.TextBox
    Protected WithEvents cbxTestUser As System.Web.UI.WebControls.CheckBox
    Protected WithEvents cbxCustomerAdmin As System.Web.UI.WebControls.RadioButton
    Protected WithEvents cbxNoCustomerAdmin As System.Web.UI.WebControls.RadioButton
    Protected WithEvents cbxFirstLevelAdmin As System.Web.UI.WebControls.RadioButton
    Protected WithEvents lblLastPwdChange As System.Web.UI.WebControls.Label
    Protected WithEvents cbxPwdNeverExpires As System.Web.UI.WebControls.CheckBox
    Protected WithEvents lblFailedLogins As System.Web.UI.WebControls.Label
    Protected WithEvents cbxAccountIsLockedOut As System.Web.UI.WebControls.CheckBox
    Protected WithEvents txtPassword As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtConfirmPassword As System.Web.UI.WebControls.TextBox
    Protected WithEvents trCustomerAdmin0 As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents trCustomerAdmin1 As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents trCustomerAdmin2 As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents trCustomerAdmin3 As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents trPwdNeverExpires As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents trTestUser As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents trSearch As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents txtReference As System.Web.UI.WebControls.TextBox
    Protected WithEvents ddlFilterCustomer As System.Web.UI.WebControls.DropDownList
    Protected WithEvents trCustomer As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents lblGroup As System.Web.UI.WebControls.Label
    Protected WithEvents trSearchSpacer As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents lnkCustomerManagement As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lnkAppManagement As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lbtnSave As System.Web.UI.WebControls.LinkButton
    Protected WithEvents lbtnCancel As System.Web.UI.WebControls.LinkButton
    Protected WithEvents lbtnDelete As System.Web.UI.WebControls.LinkButton
    Protected WithEvents lbtnNew As System.Web.UI.WebControls.LinkButton
    Protected WithEvents lnkGroupManagement As System.Web.UI.WebControls.HyperLink
    Protected WithEvents Form1 As System.Web.UI.HtmlControls.HtmlForm
    Protected WithEvents chkLoggedOn As System.Web.UI.WebControls.CheckBox
    Protected WithEvents lnkOrganizationManagement As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lblOrganization As System.Web.UI.WebControls.Label
    Protected WithEvents ddlFilterOrganization As System.Web.UI.WebControls.DropDownList
    Protected WithEvents ddlGroups As System.Web.UI.WebControls.DropDownList
    Protected WithEvents ddlOrganizations As System.Web.UI.WebControls.DropDownList
    Protected WithEvents cbxOrganizationAdmin As System.Web.UI.WebControls.CheckBox
    Protected WithEvents trOrganization As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents trOrganizationAdministrator As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents trGroup As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents txtReadMessageCount As System.Web.UI.WebControls.TextBox
    Protected WithEvents trReadMessageCount As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents trSelectOrganization As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents trMail As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents trPhone As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents txtMail As System.Web.UI.WebControls.TextBox
    Protected WithEvents lnkExcel As System.Web.UI.WebControls.HyperLink
    Protected WithEvents txtPasswordAutoBAK As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtPAsswordConfirmAutoBAK As System.Web.UI.WebControls.TextBox
    Protected WithEvents btnCreatePassword As System.Web.UI.WebControls.LinkButton
    Protected WithEvents chkNewPasswort As System.Web.UI.WebControls.CheckBox
    Protected WithEvents txtFirstName As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtStore As System.Web.UI.WebControls.TextBox
    Protected WithEvents ddlTitle As System.Web.UI.WebControls.DropDownList
    Protected WithEvents txtLastName As System.Web.UI.WebControls.TextBox
    Protected WithEvents cbxApproved As System.Web.UI.WebControls.CheckBox
    Protected WithEvents trNewPassword As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents trPassword As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents trConfirmPassword As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents lbtnApprove As System.Web.UI.WebControls.LinkButton
    Protected WithEvents lbtnNotApproved As System.Web.UI.WebControls.LinkButton
    Protected WithEvents lblNotApprovedMode As System.Web.UI.WebControls.Label
    Protected WithEvents trAnrede As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents trVorname As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents trNachname As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents lbtnDistrict As System.Web.UI.WebControls.LinkButton
    Protected WithEvents trMatrix As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents Table3 As System.Web.UI.WebControls.Table
    Protected WithEvents Matrix As System.Web.UI.WebControls.Table
    Protected WithEvents lblMessage As System.Web.UI.WebControls.Label
    Protected WithEvents lblError As System.Web.UI.WebControls.Label
    Protected WithEvents chk_Matrix1 As System.Web.UI.WebControls.CheckBox
    Protected WithEvents lnkArchivManagement As System.Web.UI.WebControls.HyperLink
    Protected WithEvents txtFilterReferenz As System.Web.UI.WebControls.TextBox
    Protected WithEvents btnSuche As System.Web.UI.WebControls.LinkButton
    Protected WithEvents ucHeader As Header
    Protected WithEvents trEmployee01 As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents trEmployee02 As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents trEmployee03 As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents trEmployee04 As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents trEmployee05 As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents trEmployee06 As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents trEmployee07 As System.Web.UI.HtmlControls.HtmlTableRow

    Protected WithEvents chkEmployeeDisplay As System.Web.UI.WebControls.CheckBox
    Protected WithEvents trHierarchyDisplay As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents ddlHierarchyDisplay As System.Web.UI.WebControls.DropDownList
    Protected WithEvents chkEmployee As System.Web.UI.WebControls.CheckBox
    Protected WithEvents lblPicture As System.Web.UI.WebControls.Label
    Protected WithEvents lblPictureName As System.Web.UI.WebControls.Label
    Protected WithEvents ddlHierarchy As System.Web.UI.WebControls.DropDownList
    Protected WithEvents txtDepartment As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtPosition As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtTelephone As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtFax As System.Web.UI.WebControls.TextBox
    Protected WithEvents upFile As System.Web.UI.HtmlControls.HtmlInputFile
    Protected WithEvents btnUpload As System.Web.UI.WebControls.LinkButton
    Protected WithEvents btnRemove As System.Web.UI.WebControls.LinkButton
    Protected WithEvents Image1 As System.Web.UI.WebControls.Image
    Protected WithEvents hlUserHistory As System.Web.UI.WebControls.HyperLink
    Protected WithEvents chkAngemeldet As System.Web.UI.WebControls.CheckBox
    Protected WithEvents lblBenutzer As System.Web.UI.WebControls.Label
    Protected WithEvents txtValidFrom As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtPhone As System.Web.UI.WebControls.TextBox

    Protected WithEvents chkOnlyDisabledUser As System.Web.UI.WebControls.CheckBox

    Protected WithEvents txtLastLoginBefore As System.Web.UI.WebControls.TextBox

    Protected WithEvents imgbCalendar As System.Web.UI.WebControls.ImageButton

    Protected WithEvents calLastLogin As System.Web.UI.WebControls.Calendar

    Protected WithEvents td_EmployeeDisplay1 As System.Web.UI.HtmlControls.HtmlTableCell
    Protected WithEvents td_EmployeeDisplay2 As System.Web.UI.HtmlControls.HtmlTableCell

    Protected WithEvents td_LastLoginBefore1 As System.Web.UI.HtmlControls.HtmlTableCell
    Protected WithEvents td_LastLoginBefore2 As System.Web.UI.HtmlControls.HtmlTableCell

    Protected WithEvents td_OnlyDisabledUser1 As System.Web.UI.HtmlControls.HtmlTableCell
    Protected WithEvents td_OnlyDisabledUser2 As System.Web.UI.HtmlControls.HtmlTableCell

    Protected WithEvents lblKundenadministrationInfo As Label
    Protected WithEvents lblLockedBy As Label

    Protected WithEvents btnEmpty As System.Web.UI.WebControls.ImageButton
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
    Private m_User As User
    Private m_App As App
    Private m_Rights As DataTable
    Private m_Districts As DataTable
#End Region

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load, Me.Load
        ' Hier Benutzercode zur Seiteninitialisierung einfügen
        m_User = GetUser(Me)
        ucHeader.InitUser(m_User)
        ucStyles.TitleText = "Benutzerverwaltung"
        AdminAuth(Me, m_User, AdminLevel.Organization)
        Try
            m_App = New App(m_User)

            If Not IsPostBack Then
                lblError.Text = ""
                FillHierarchy()
                FillForm()
                txtFilterUserName.Focus()

            ElseIf lbtnDistrict.Visible = True AndAlso Matrix.Visible = True Then
                Dim _User As New User(CInt(txtUserID.Text), m_User.App.Connectionstring)
                Fill_Matrix(_User.Customer.KUNNR, "")
                'Matrix.Visible = True
            End If
        Catch ex As Exception
            m_App.WriteErrorText(1, m_User.UserName, "UserManagement", "Page_Load", ex.ToString)

            lblError.Text = ex.ToString
            lblError.Visible = True
        End Try
    End Sub

#Region " Data and Function "
    Private Sub FillHierarchy()
        Dim cn As New SqlClient.SqlConnection(m_User.App.Connectionstring)
        Try
            cn.Open()
            Dim _HierarchyList As New Kernel.HierarchyList(cn)
            Dim vwHierarchy As DataView = _HierarchyList.DefaultView
            ddlHierarchy.DataSource = vwHierarchy
            ddlHierarchy.DataValueField = "ID"
            ddlHierarchy.DataTextField = "Level"
            ddlHierarchy.DataBind()
            ddlHierarchy.ClearSelection()
            ddlHierarchy.Items.FindByValue("1").Selected = True

            Dim _HierarchyList2 As New Kernel.HierarchyList(cn, True)
            Dim vwHierarchy2 As DataView = _HierarchyList2.DefaultView
            ddlHierarchyDisplay.DataSource = vwHierarchy2
            ddlHierarchyDisplay.DataValueField = "ID"
            ddlHierarchyDisplay.DataTextField = "Level"
            ddlHierarchyDisplay.DataBind()
            ddlHierarchyDisplay.ClearSelection()
            ddlHierarchyDisplay.Items(ddlHierarchyDisplay.Items.Count - 1).Selected = True
        Catch ex As Exception
            Throw
        Finally
            If cn.State <> ConnectionState.Closed Then
                cn.Close()
            End If
        End Try
    End Sub

    Private Sub FillForm()
        Dim cn As New SqlClient.SqlConnection(m_User.App.Connectionstring)
        Try
            cn.Open()
            FillCustomer(cn) 'DropDowns fuer Customer fuellen
            FillGroups(CInt(ddlFilterCustomer.SelectedItem.Value), cn) 'DropDowns fuer Group fuellen
            FillOrganizations(CInt(ddlFilterCustomer.SelectedItem.Value), cn) 'DropDowns fuer Group fuellen

            trEmployee01.Visible = False
            trEmployee02.Visible = False
            trEmployee03.Visible = False
            trEmployee04.Visible = False
            trEmployee05.Visible = False
            trEmployee06.Visible = False
            trEmployee07.Visible = False
            trHierarchyDisplay.Visible = False

            td_EmployeeDisplay1.Visible = False
            td_EmployeeDisplay2.Visible = False

            td_LastLoginBefore1.Visible = False
            td_LastLoginBefore2.Visible = False

            td_OnlyDisabledUser1.Visible = False
            td_OnlyDisabledUser2.Visible = False


            If m_User.HighestAdminLevel > AdminLevel.Customer Then
                'Wenn DAD-SuperUser:
                lblCustomer.Visible = False 'Label mit Customer-Namen ausblenden
                ddlFilterCustomer.Visible = True 'DropDown zur Customer-Auswahl einblenden
                'wenn SuperUser und übergeordnete Firma
                If m_User.Customer.AccountingArea = -1 Then
                    lnkAppManagement.Visible = True
                End If


                'trMail.Visible = True   'Mailadresse einblenden...
                'trEmployee01.Visible = True
                'trEmployee02.Visible = True
                'trEmployee03.Visible = True
                'trEmployee04.Visible = True
                'trEmployee05.Visible = True
                'trEmployee06.Visible = True
                'trEmployee07.Visible = True
                'td_EmployeeDisplay1.Visible = True
                'td_EmployeeDisplay2.Visible = True
                trHierarchyDisplay.Visible = True
                td_LastLoginBefore1.Visible = True
                td_LastLoginBefore2.Visible = True
                td_OnlyDisabledUser1.Visible = True
                td_OnlyDisabledUser2.Visible = True

            Else
                'Wenn nicht DAD-Super-User:
                'trMail.Visible = False
                lnkArchivManagement.Visible = False
                lnkCustomerManagement.Visible = False 'Link fuer die Kundenverwaltung ausblenden
                lblCustomer.Text = m_User.Customer.CustomerName 'Customer des angemeldeten Benutzers
                trCustomer.Visible = False 'Customer-Auswahl im Edit-bereich ausblenden
                dgSearchResult.Columns(6).Visible = False 'Spalte "Test-Zugang" ausblenden
                trTestUser.Visible = False '"Test-Zugang" aus dem Edit-Bereich ausblenden
                dgSearchResult.Columns(8).Visible = False 'Spalte "Passwort läuft nie ab" ausblenden
                dgSearchResult.Columns(5).Visible = False 'Spalte "Customer-Admin" ausblenden
                trCustomerAdmin0.Visible = False 'Customer-Admin im Editbereich ausblenden
                trCustomerAdmin1.Visible = False 'Customer-Admin im Editbereich ausblenden
                trCustomerAdmin2.Visible = False 'Customer-Admin im Editbereich ausblenden
                trCustomerAdmin3.Visible = False 'Customer-Admin im Editbereich ausblenden
                trPwdNeverExpires.Visible = False '"Passwort laeuft nie ab" aus dem Edit-Bereich ausblenden
                lnkAppManagement.Visible = False 'Link fuer die Anwendungsverwaltung ausblenden
                trReadMessageCount.Visible = False


                '--------------------------------
                'selektionsspalten nach letztem login und gesperrte benutzer 
                'auch für customeradmins sichtbar JJU20081013
                '--------------------------------
                td_LastLoginBefore1.Visible = True
                td_LastLoginBefore2.Visible = True
                td_OnlyDisabledUser1.Visible = True
                td_OnlyDisabledUser2.Visible = True
                '--------------------------------



                If Not m_User.Customer.ShowOrganization Then
                    lnkOrganizationManagement.Visible = False
                    trSelectOrganization.Visible = False
                    dgSearchResult.Columns(4).Visible = False 'Spalte "Organisation" ausblenden
                    trOrganization.Visible = False
                    trOrganizationAdministrator.Visible = False
                End If

                If Not m_User.IsCustomerAdmin Then
                    'Wenn nicht Customer-Admin:
                    lnkOrganizationManagement.Visible = False
                    lnkGroupManagement.Visible = False
                    dgSearchResult.Columns(4).Visible = False 'Spalte "Organisation" ausblenden
                    trGroup.Visible = False 'Gruppenauswahl im Edit-Bereich ausblenden
                    trOrganization.Visible = False 'Organisationsauswahl im Edit-Bereich ausblenden
                    trOrganizationAdministrator.Visible = False 'OrganisationAdmin-Auswahl im Edit-Bereich ausblenden
                    If m_User.Groups.Count > 0 Then lblGroup.Text = m_User.Groups(0).GroupName 'Gruppe des angemeldeten Benutzers
                    'lblGroup.Visible = True 'Label mit Gruppen-Namen einblenden
                    'ddlFilterGroup.Visible = False 'DropDown zur Gruppenauswahl ausblenden
                    lblOrganization.Text = m_User.Organization.OrganizationName 'Organisation des angemeldeten Benutzers
                    lblOrganization.Visible = True 'Label mit Organisationsnamen einblenden
                    ddlFilterOrganization.Visible = False 'DropDown zur Organisationsauswahl ausblenden
                End If
            End If

            trEditUser.Visible = False 'Editbereich ausblenden
            trSearchResult.Visible = False 'Suchergebnis ausblenden
        Catch ex As Exception
            Throw
        Finally
            If cn.State <> ConnectionState.Closed Then
                cn.Close()
            End If
        End Try
    End Sub

    Private Sub FillGroups(ByVal intCustomerID As Integer, ByVal cn As SqlClient.SqlConnection)
        Dim dtGroups As New Kernel.GroupList(intCustomerID, cn, m_User.Customer.AccountingArea)
        FillGroup(ddlGroups, False, dtGroups)
        FillGroup(ddlFilterGroup, True, dtGroups)
        If ddlFilterGroup.Items.Count = 0 Then
            ddlFilterGroup.Enabled = False
            btnSuche.Enabled = False
        Else
            If (Not ddlGroups.Items.Count = 0) And _
                (ddlGroups.Items.Count > ddlFilterGroup.SelectedIndex) And _
                (ddlFilterGroup.SelectedIndex > 0) Then
                ddlGroups.SelectedIndex = ddlFilterGroup.SelectedIndex
            End If
        End If
    End Sub
    Private Sub FillGroup(ByVal ddl As DropDownList, ByVal blnAllNone As Boolean, ByVal dtgroups As Kernel.GroupList)
        If blnAllNone Then dtgroups.AddAllNone(True, True)
        With ddl
            .Items.Clear()
            Dim dv As DataView = dtgroups.DefaultView
            dv.Sort = "GroupName"
            If m_User.HighestAdminLevel = AdminLevel.Organization AndAlso m_User.Customer.OrgAdminRestrictToCustomerGroup Then
                dv.RowFilter = "IsCustomerGroup=1"
            End If
            .DataSource = dv
            .DataTextField = "GroupName"
            .DataValueField = "GroupID"
            .DataBind()
            If m_User.Groups.HasGroups Then
                Dim _li As ListItem
                _li = .Items.FindByValue(m_User.Groups(0).GroupId.ToString)
                If Not _li Is Nothing Then _li.Selected = True
                If ddl.ID = "ddlGroups" Then
                    _li = .Items.FindByValue("-1")
                    If Not _li Is Nothing Then
                        ddl.ClearSelection()
                        _li.Selected = True
                    End If
                End If
            Else
                If .Items.Count <> 0 Then
                    If blnAllNone Then
                        Dim _li As ListItem = .Items.FindByValue("-1")
                        If Not _li Is Nothing Then
                            _li.Selected = True
                        End If
                    End If
                End If
            End If
        End With
    End Sub

    Private Sub FillOrganizations(ByVal intCustomerID As Integer, ByVal cn As SqlClient.SqlConnection)
        Dim dtOrganizations As New OrganizationList(intCustomerID, cn, m_User.Customer.AccountingArea)
        FillOrganization(ddlOrganizations, False, dtOrganizations)
        FillOrganization(ddlFilterOrganization, True, dtOrganizations)
    End Sub
    Private Sub FillOrganization(ByVal ddl As DropDownList, ByVal blnAllNone As Boolean, ByVal dtOrganizations As OrganizationList)
        If blnAllNone Then dtOrganizations.AddAllNone(True, True)
        With ddl
            .Items.Clear()
            Dim dv As DataView = dtOrganizations.DefaultView
            dv.Sort = "OrganizationName"
            .DataSource = dv
            .DataTextField = "OrganizationName"
            .DataValueField = "OrganizationID"
            .DataBind()
            If IsNumeric(m_User.Organization.OrganizationId) Then
                Dim _li As ListItem = .Items.FindByValue(m_User.Organization.OrganizationId.ToString)
                If Not (_li Is Nothing) Then
                    _li.Selected = True
                End If
            Else
                If blnAllNone Then .Items.FindByValue("-1").Selected = True
            End If
        End With
    End Sub

    Private Sub FillCustomer(ByVal cn As SqlClient.SqlConnection)
        Dim dtCustomers As Kernel.CustomerList
        dtCustomers = New Kernel.CustomerList(m_User.Customer.AccountingArea, cn)

        With ddlCustomer
            Dim dv As DataView = dtCustomers.DefaultView
            dv.Sort = "Customername"
            .DataSource = dv
            .DataTextField = "Customername"
            .DataValueField = "CustomerID"
            .DataBind()
            .Items.FindByValue(m_User.Customer.CustomerId.ToString).Selected = True



        End With
        dtCustomers.AddAllNone(True, True)
        With ddlFilterCustomer
            Dim dv As DataView = dtCustomers.DefaultView
            dv.Sort = "Customername"
            .DataSource = dv
            .DataTextField = "Customername"
            .DataValueField = "CustomerID"
            .DataBind()
            If m_User.HighestAdminLevel = AdminLevel.Master Or m_User.HighestAdminLevel = AdminLevel.FirstLevel Then
                .Items.FindByValue("0").Selected = True
            Else
                .Enabled = False
                .Items.FindByValue(m_User.Customer.CustomerId.ToString).Selected = True
            End If

        End With
    End Sub

    Private Sub FillDataGrid(ByVal blnNotAppoved As Boolean)
        Dim strSort As String = "UserID"
        If Not ViewState("ResultSort") Is Nothing Then
            strSort = ViewState("ResultSort").ToString
        End If
        FillDataGrid(blnNotAppoved, strSort)
    End Sub
    Private Sub FillDataGrid(ByVal blnNotApproved As Boolean, ByVal strSort As String)
        trSearchResult.Visible = True

        Dim _context As HttpContext = HttpContext.Current
        Dim dvUser As DataView

        Dim cn As New SqlClient.SqlConnection(m_User.App.Connectionstring)
        cn.Open()

        Dim intTemp As Integer
        If m_User.HighestAdminLevel = AdminLevel.Master Or m_User.HighestAdminLevel = AdminLevel.FirstLevel Then
            intTemp = CInt(ddlFilterCustomer.SelectedItem.Value)
        Else
            intTemp = m_User.Customer.CustomerId
        End If

        Dim loginBeforeDate As Date = Now
        Dim errorText As String = ""
        If CKG.Base.Business.HelpProcedures.checkDate(txtLastLoginBefore, errorText, True) Then
            If Not txtLastLoginBefore.Text.Trim(" "c) = "" Then
                loginBeforeDate = CDate(txtLastLoginBefore.Text)
            End If
        Else
            lblError.Text = errorText
            lblError.Visible = True
            Exit Sub
        End If

        Dim intTemp2 As Integer
        If m_User.HighestAdminLevel > AdminLevel.Organization Then
            intTemp2 = CInt(ddlFilterOrganization.SelectedItem.Value)
        Else
            intTemp2 = m_User.Organization.OrganizationId
        End If

        Dim dtUser As Kernel.UserList
        Dim intHierarchy As Integer = -1
        If chkEmployeeDisplay.Checked And CInt(ddlHierarchyDisplay.SelectedValue) > -1 Then
            intHierarchy = CInt(ddlHierarchyDisplay.SelectedValue)
        End If
        If Not blnNotApproved Then
            dtUser = New Kernel.UserList(txtFilterUserName.Text, _
                                  intTemp, _
                                  CInt(ddlFilterGroup.SelectedItem.Value), _
                                  intTemp2, _
                                  False, _
                                  String.Empty, _
                                  cn, _
                                  txtFilterReferenz.Text, _
                                  chkEmployeeDisplay.Checked, _
                                  intHierarchy, _
                                  m_User.Customer.AccountingArea, chkAngemeldet.Checked, chkOnlyDisabledUser.Checked, loginBeforeDate)

        Else
            dtUser = New Kernel.UserList(txtFilterUserName.Text, _
                                  intTemp, _
                                  CInt(ddlFilterGroup.SelectedItem.Value), _
                                  intTemp2, _
                                  blnNotApproved, _
                                  m_User.UserName, _
                                  cn, _
                                  String.Empty, _
                                  chkEmployeeDisplay.Checked, _
                                  intHierarchy, _
                                  m_User.Customer.AccountingArea)
            chkAngemeldet.Checked = False
        End If
        dvUser = dtUser.DefaultView
        '_context.Cache.Insert("myUserListView", dvUser, Nothing, DateTime.Now.AddMinutes(20), TimeSpan.Zero)
        Session("myUserListView") = dvUser
        lblBenutzer.Text = dvUser.Count.ToString & " Benutzer gefunden."

        dvUser.Sort = strSort
        If dvUser.Count > dgSearchResult.PageSize Then
            dgSearchResult.PagerStyle.Visible = True
        Else
            dgSearchResult.PagerStyle.Visible = False
        End If

        'prüfung das der gesetzte pageindex größer als die anzahl der ergebnisse ist,
        'ist hier nötig weil bei jedem fillGrid die neuen Selektionsparameter verwendet werden
        'JJU 20081013
        '---------------------------------------------------------
        'War nicht ganz richtig, wenn man 23 User selektiert sind und auf Seite 3 will,
        'kommt man nie auf die letzte Seite. 23 nicht größer 30!!! Sind aber noch 3 User auf Seite 3!!

        If Not dvUser.Count >= ((dgSearchResult.CurrentPageIndex + 1) * dgSearchResult.PageSize) - 9 Then
            dgSearchResult.CurrentPageIndex = 0
        End If
        '---------------------------------------------------------


        With dgSearchResult
            .DataSource = dvUser
            .DataBind()
        End With


        If m_User.HighestAdminLevel = AdminLevel.Master Then
            'prüfung ob Kunde Kundenadministrationsberechtigung
            'das Betrifft nur Masteradmins, dass die sehen was sie nicht dürfen
            'JJU20090319 ITA 2156
            For Each item As DataGridItem In dgSearchResult.Items
                If Not dtUser.Select("UserID='" & item.Cells(0).Text & "'")(0)("SelfAdministration") Is DBNull.Value Then
                    Select Case CInt(dtUser.Select("UserID='" & item.Cells(0).Text & "'")(0)("SelfAdministration"))
                        Case 1
                            item.BackColor = Color.Orange
                        Case 2
                            item.BackColor = Color.Red
                            If Not m_User.Customer.AccountingArea = "-1" Then 'superadmin
                                item.FindControl("ibtnSRDelete").Visible = False
                            End If

                    End Select
                End If
            Next
        End If




        If blnNotApproved = True Then

            Dim Item As DataGridItem
            Dim lnkButton As LinkButton
            Dim strUserText As String

            For Each Item In dgSearchResult.Items

                If Item.Cells(16).Text = m_User.UserName Then
                    lnkButton = New LinkButton()

                    lnkButton = Item.Cells(1).Controls(0)
                    strUserText = lnkButton.Text
                    Item.Cells(1).Controls.Clear()
                    Item.Cells(1).Text = strUserText
                    Item.Cells(1).ForeColor = System.Drawing.Color.Gray
                    lnkButton.Dispose()

                    Item.Cells(15).Controls.Clear()

                End If
            Next
        End If

    End Sub

    Private Function FillEdit(ByVal intUserId As Integer) As Boolean
        Dim _li As ListItem
        hlUserHistory.Visible = False
        Try
            SearchMode(False)
            Dim _User As New User(intUserId, m_User.App.Connectionstring)
            txtUserID.Text = _User.UserID.ToString
            txtUserName.Text = _User.UserName
            Session("UsernameStart") = _User.UserName
            Session("LockedOutStart") = _User.AccountIsLockedOut
            txtReference.Text = _User.Reference
            txtMail.Text = _User.Email
            txtPhone.Text = _User.Telephone
            txtReadMessageCount.Text = _User.ReadMessageCount.ToString
            cbxTestUser.Checked = _User.IsTestUser
            chkLoggedOn.Checked = _User.LoggedOn
            chk_Matrix1.Checked = _User.Matrixfilled
            If Not chk_Matrix1.Checked Then chk_Matrix1.Enabled = False
            cbxCustomerAdmin.Checked = _User.IsCustomerAdmin
            cbxFirstLevelAdmin.Checked = _User.FirstLevelAdmin
            If _User.Customer.CustomerId > 0 Then
                If Not ddlCustomer.SelectedItem Is Nothing Then
                    ddlCustomer.SelectedItem.Selected = False

                End If
                _li = ddlCustomer.Items.FindByValue(_User.Customer.CustomerId.ToString)
                If Not _li Is Nothing Then
                    _li.Selected = True
                End If
            End If
            cbxOrganizationAdmin.Checked = _User.Organization.OrganizationAdmin
            If Not ddlGroups.SelectedItem Is Nothing Then
                ddlGroups.SelectedItem.Selected = False
            End If
            If _User.Groups.HasGroups Then
                'If CInt(ddlFilterCustomer.SelectedItem.Value) < 1 Then
                Dim intCustomerID As Integer = _User.Customer.CustomerId
                Dim cn As New SqlClient.SqlConnection(m_User.App.Connectionstring)
                cn.Open()
                Dim dtGroups As New Kernel.GroupList(intCustomerID, cn, m_User.Customer.AccountingArea)
                FillGroup(ddlGroups, False, dtGroups)
                'End If
                _li = ddlGroups.Items.FindByValue(_User.Groups(0).GroupId.ToString)
                If Not _li Is Nothing Then
                    If Not ddlGroups.SelectedItem Is Nothing Then
                        ddlGroups.SelectedItem.Selected = False
                    End If
                    _li.Selected = True
                End If
            Else
                _li = New ListItem("- keine -", "-1")
                _li.Selected = True
                ddlGroups.Items.Add(_li)
            End If
            If Not ddlOrganizations.SelectedItem Is Nothing Then
                ddlOrganizations.SelectedItem.Selected = False
            End If
            If IsNumeric(_User.Organization.OrganizationId) Then
                'If CInt(ddlFilterCustomer.SelectedItem.Value) < 1 Then
                Dim intCustomerID As Integer = _User.Customer.CustomerId
                Dim cn As New SqlClient.SqlConnection(m_User.App.Connectionstring)
                cn.Open()
                Dim dtOrganizations As New OrganizationList(intCustomerID, cn, m_User.Customer.AccountingArea)
                FillOrganization(ddlOrganizations, False, dtOrganizations)
                'End If
                _li = ddlOrganizations.Items.FindByValue(_User.Organization.OrganizationId.ToString)
                If Not _li Is Nothing Then
                    If Not ddlOrganizations.SelectedItem Is Nothing Then
                        ddlOrganizations.SelectedItem.Selected = False
                    End If
                    _li.Selected = True
                End If
            Else
                _li = New ListItem("- keine -", "-1")
                _li.Selected = True
                ddlOrganizations.Items.Add(_li)
            End If
            lblLastPwdChange.Text = String.Format("{0:dd.MM.yy}", _User.LastPasswordChange)
            cbxPwdNeverExpires.Checked = _User.PasswordNeverExpires
            lblFailedLogins.Text = _User.FailedLogins.ToString
            If _User.AccountIsLockedOut Then
                Dim sLockedBy As String = CStr(GetHistoryInfos(_User))
                If sLockedBy.ToLower <> _User.UserName.ToLower AndAlso sLockedBy <> "" Then
                    If sLockedBy.ToLower = "[admin-regelprozess]" Then
                        lblLockedBy.Text = "durch Regelprozess"
                    Else
                        lblLockedBy.Text = "durch Administrator"
                    End If
                    lblLockedBy.Visible = True
                ElseIf sLockedBy.ToLower = _User.UserName.ToLower Then
                    lblLockedBy.Text = "durch User"
                    lblLockedBy.Visible = True
                End If
            End If
            cbxAccountIsLockedOut.Checked = _User.AccountIsLockedOut
            cbxApproved.Checked = _User.Approved
            txtReadMessageCount.Text = _User.ReadMessageCount.ToString
            If Not ddlTitle.SelectedItem Is Nothing Then
                ddlTitle.SelectedItem.Selected = False
            End If

            For Each _li In ddlTitle.Items
                If _li.Value = _User.Title Then
                    _li.Selected = True
                    Exit For
                End If
            Next
            txtFirstName.Text = _User.FirstName
            txtLastName.Text = _User.LastName
            txtStore.Text = _User.Store

            If _User.ValidFrom.Length > 0 Then
                txtValidFrom.Text = CDate(_User.ValidFrom).ToShortDateString
            End If

            PasswordEditMode((Not _User.Customer.CustomerPasswordRules.DontSendEmail) And (_User.HighestAdminLevel = AdminLevel.None))

            If Not ddlCustomer.SelectedItem Is Nothing Then
                Dim intddlCustID As Integer = CInt(ddlCustomer.SelectedItem.Value)
                Dim _customer As New Customer(intddlCustID, m_User.App.Connectionstring)
                '#########  ITA: Distriktzuordnung FFD
                If _customer.ShowDistrikte = True AndAlso txtUserID.Text <> "-1" Then
                    lbtnDistrict.Visible = True
                    trMatrix.Visible = True
                End If
            End If

            'Mitarbeiterdaten
            ddlHierarchy.ClearSelection()
            _li = ddlHierarchy.Items.FindByValue(_User.HierarchyID.ToString)
            If Not _li Is Nothing Then
                _li.Selected = True
            Else
                ddlHierarchy.Items.FindByValue("1").Selected = True
            End If
            chkEmployee.Checked = _User.Employee
            If _User.Picture Then
                lblPictureName.Text = _User.UserID.ToString & ".jpg"
            Else
                lblPictureName.Text = ""
            End If
            txtDepartment.Text = _User.Department
            txtPosition.Text = _User.Position
            txtTelephone.Text = _User.PhoneEmployee
            txtFax.Text = _User.Fax

            'NameEditMode(Not _User.Customer.NameInputOptional)
            If _User.IsSuperiorTo(m_User) Then
                lblMessage.Text = "Sie können kein übergeordnetes Benutzerkonto bearbeiten!"
                Return False
            End If

            'trEmployee07.Visible = False
            'If CInt(txtUserID.Text) > -1 Then
            '    If m_User.HighestAdminLevel > AdminLevel.Customer Then
            '        trEmployee07.Visible = True
            '    End If
            'End If

            btnRemove.Enabled = False
            Image1.Visible = False
            If _User.Picture Then
                btnRemove.Enabled = True

                Dim info As System.IO.FileInfo
                info = New System.IO.FileInfo(System.Configuration.ConfigurationManager.AppSettings("UploadPathLocal") & "responsible\" & txtUserID.Text & ".jpg")
                If (info.Exists) Then
                    Image1.Visible = True
                    Image1.ImageUrl = Replace(System.Configuration.ConfigurationManager.AppSettings("UploadPath"), "\", "/") & "responsible/" & txtUserID.Text & ".jpg"
                End If
            End If

            hlUserHistory.NavigateUrl = "SingleUserHistory.aspx?UserID=" & _User.UserID.ToString
            hlUserHistory.Visible = True


            'aktionen nach selfadministrationlvl
            Select Case _User.Customer.Selfadministration
                Case 1
                    'info anzeigen
                    If m_User.HighestAdminLevel = AdminLevel.Master Then
                        'betrifft nur dad admins
                        lblKundenadministrationInfo.Text = _User.Customer.SelfadministrationInfo
                    End If
                Case 2
                    If m_User.HighestAdminLevel = AdminLevel.Master Then
                        'dad admins
                        lbtnSave.Enabled = False
                    End If
                    If Not m_User.Customer.AccountingArea = "-1" Then 'superadmin darf immer administrieren
                        lbtnSave.Enabled = True
                    End If
            End Select



            Return True
        Catch ex As Exception
            m_App.WriteErrorText(1, m_User.UserName, "UserManagement", "FillEdit", ex.ToString)
            lblMessage.Text = ex.ToString
        End Try
    End Function

    Private Sub ClearEdit()
    
        'neuordnung des clearens + ergänzung +änderung ITA 2315 JJU 20081013
        'texboxen
        '----------------------------------------
        txtUserID.Text = "-1"
        txtUserName.Text = ""
        txtReference.Text = ""
        txtPassword.Text = ""
        txtConfirmPassword.Text = ""
        txtFirstName.Text = ""
        txtLastName.Text = ""
        txtStore.Text = ""
        txtDepartment.Text = ""
        txtPosition.Text = ""
        txtTelephone.Text = ""
        txtFax.Text = ""
        txtMail.Text = ""
        txtPhone.Text = ""
        txtReadMessageCount.Text = "0"
        txtValidFrom.Text = ""
        lblLockedBy.Text = ""
        lblLockedBy.Visible = False




        '----------------------------------------

        'checkboxen
        '----------------------------------------
        chkEmployee.Checked = False
        'Default: Produktivuser, da Checkbox für Firmenadmins nicht sichtbar!!
        cbxTestUser.Checked = m_User.IsTestUser
        cbxAccountIsLockedOut.Checked = m_User.AccountIsLockedOut
        cbxApproved.Checked = Not ((m_User.HighestAdminLevel = AdminLevel.Master) Or (m_User.HighestAdminLevel = AdminLevel.FirstLevel))
        chkLoggedOn.Checked = False
        chkNewPasswort.Checked = False
        'eigentlich radiobuttons, aber gut 
        '----------------------
        cbxCustomerAdmin.Checked = False
        cbxFirstLevelAdmin.Checked = False
        cbxNoCustomerAdmin.Checked = True
        '----------------------
        cbxOrganizationAdmin.Checked = False
        cbxPwdNeverExpires.Checked = False
        chk_Matrix1.Checked = False
        '----------------------------------------


        'labels
        '----------------------------------------
        lblPictureName.Text = ""
        lblLastPwdChange.Text = ""
        lblFailedLogins.Text = ""
        lblKundenadministrationInfo.Text = ""
        '----------------------------------------

        'linkbuttons
        '----------------------------------------
        lbtnSave.Visible = True
        lbtnDelete.Visible = False
        '----------------------------------------

        'dropDownListen
        '----------------------------------------
        ddlOrganizations.ClearSelection()
        If IsNumeric(m_User.Organization.OrganizationId) Then
            Dim _li As ListItem = ddlOrganizations.Items.FindByValue(m_User.Organization.OrganizationId.ToString)
            If Not (_li Is Nothing) Then
                _li.Selected = True
            End If
        Else
            ddlOrganizations.SelectedIndex = 0
        End If

        ddlGroups.ClearSelection()
        If (Not ddlGroups.Items.Count = 0) And _
            (ddlGroups.Items.Count > ddlFilterGroup.SelectedIndex) And _
            (ddlFilterGroup.SelectedIndex > 0) Then
            ddlGroups.SelectedIndex = ddlFilterGroup.SelectedIndex
        Else
            ddlGroups.SelectedIndex = -1
        End If
        ddlTitle.SelectedIndex = 0
        ddlHierarchy.SelectedValue = "1"
        '----------------------------------------


        LockEdit(False)

    End Sub

    Private Sub LockEdit(ByVal blnLock As Boolean)
        Dim enabled As Boolean = Not blnLock
        Dim strBackColor As String = "White"
        If blnLock Then
            strBackColor = "LightGray"
        End If
        Dim backColor As System.Drawing.Color = System.Drawing.Color.FromName(strBackColor)

        txtReadMessageCount.Enabled = enabled
        txtReadMessageCount.BackColor = backColor
        txtUserName.Enabled = enabled
        txtUserName.BackColor = backColor
        txtReference.Enabled = enabled
        txtReference.BackColor = backColor
        cbxTestUser.Enabled = enabled
        chkLoggedOn.Enabled = enabled
        chkNewPasswort.Enabled = enabled
        cbxCustomerAdmin.Enabled = enabled
        ddlCustomer.Enabled = enabled
        ddlCustomer.BackColor = backColor
        cbxOrganizationAdmin.Enabled = enabled
        ddlOrganizations.Enabled = enabled
        ddlOrganizations.BackColor = backColor
        ddlGroups.Enabled = enabled
        ddlGroups.BackColor = backColor
        lblLastPwdChange.Enabled = enabled
        cbxPwdNeverExpires.Enabled = enabled
        lblFailedLogins.Enabled = enabled
        cbxAccountIsLockedOut.Enabled = enabled
        ddlTitle.Enabled = enabled
        ddlTitle.BackColor = backColor
        txtFirstName.Enabled = enabled
        txtFirstName.BackColor = backColor
        txtLastName.Enabled = enabled
        txtLastName.BackColor = backColor
        txtStore.Enabled = enabled
        txtStore.BackColor = backColor
        txtValidFrom.Enabled = enabled
        txtValidFrom.BackColor = backColor
        txtPassword.Enabled = enabled
        txtPassword.BackColor = backColor
        txtConfirmPassword.Enabled = enabled
        txtConfirmPassword.BackColor = backColor
        txtMail.Enabled = enabled
        txtMail.BackColor = backColor
        txtPhone.Enabled = enabled
        txtPhone.BackColor = backColor
        chkEmployee.Enabled = enabled
        ddlHierarchy.Enabled = enabled
        ddlHierarchy.BackColor = backColor
        txtDepartment.Enabled = enabled
        txtDepartment.BackColor = backColor
        txtPosition.Enabled = enabled
        txtPosition.BackColor = backColor
        txtTelephone.Enabled = enabled
        txtTelephone.BackColor = backColor
        txtFax.Enabled = enabled
        txtFax.BackColor = backColor
    End Sub

    Private Sub EditEditMode(ByVal intUserID As Integer)
        If Not FillEdit(intUserID) Then
            LockEdit(True)
            lbtnSave.Enabled = False
        Else
            lbtnSave.Enabled = True
        End If
        lbtnCancel.Text = " &#149;&nbsp;Verwerfen"
    End Sub

    Private Sub PasswordEditMode(ByVal automatic As Boolean) 'während Validation neues Passwort unterbinden, ByVal noPWChange As Boolean
        If automatic Then
            trNewPassword.Visible = True
            trPassword.Visible = False
            trConfirmPassword.Visible = False
        Else
            trNewPassword.Visible = False
            trPassword.Visible = True
            trConfirmPassword.Visible = True
        End If

        'während Validation neues Passwort unterbinden,
        'Dim strBackColor As String = "White"

        'If noPWChange Then
        '    chkNewPasswort.Enabled = Not noPWChange
        '    txtPassword.Enabled = Not noPWChange
        '    txtConfirmPassword.Enabled = Not noPWChange
        '    strBackColor = "LightGray"
        'End If

        'Dim backColor As System.Drawing.Color = System.Drawing.Color.FromName(strBackColor)
        ''chkNewPasswort.BackColor = backColor
        'txtPassword.BackColor = backColor
        'txtConfirmPassword.BackColor = backColor
    End Sub

    Private Sub EditDeleteMode(ByVal intUserID As Integer)
        Session("UsernameStart") = Nothing
        Session("LockedOutStart") = Nothing

        If Not FillEdit(intUserID) Then
            lbtnDelete.Enabled = False
        Else
            lblMessage.Text = "Möchten Sie den Benutzer wirklich löschen?"
            lbtnDelete.Enabled = True
        End If
        LockEdit(True)
        lbtnCancel.Text = " &#149;&nbsp;Abbrechen"
        lbtnSave.Visible = False
        lbtnDistrict.Visible = False
        trMatrix.Visible = False
        lbtnDelete.Visible = True
    End Sub

    Private Sub SearchMode(Optional ByVal blnSearchMode As Boolean = True)
        lblNotApprovedMode.Visible = False
        trEditUser.Visible = Not blnSearchMode
        lblKundenadministrationInfo.Visible = Not blnSearchMode
        trSearch.Visible = blnSearchMode
        trSearchSpacer.Visible = blnSearchMode
        trSearchResult.Visible = blnSearchMode
        lbtnSave.Visible = Not blnSearchMode
        lbtnCancel.Visible = Not blnSearchMode
        lbtnNew.Visible = blnSearchMode
        lbtnNotApproved.Visible = blnSearchMode
        lbtnApprove.Visible = False
    End Sub

    Private Sub SearchNotApprovedMode(ByVal search As Boolean, ByVal edit As Boolean)
        lblNotApprovedMode.Visible = search OrElse edit
        trEditUser.Visible = (Not search) OrElse edit
        lblKundenadministrationInfo.Visible = (Not search) OrElse edit
        trSearch.Visible = (Not edit)
        trSearchSpacer.Visible = (Not edit)
        trSearchResult.Visible = (Not edit)
        lbtnSave.Visible = False
        lbtnDistrict.Visible = False
        trMatrix.Visible = False
        lbtnCancel.Visible = search OrElse edit
        If search AndAlso Not edit Then
            lbtnCancel.Text = " &#149;&nbsp;Zurück<br>zur Suche"
        End If
        lbtnNew.Visible = (Not search) AndAlso (Not edit)
        lbtnNotApproved.Visible = (Not search) AndAlso (Not edit)
        lbtnApprove.Visible = (Not search) AndAlso edit
        ViewState("searchNotApproved") = search
        ViewState("editNotApproved") = edit
    End Sub

    Private Sub ApproveMode(ByVal intUserID As Integer)
        Session("UsernameStart") = Nothing
        Session("LockedOutStart") = Nothing

        If Not FillEdit(intUserID) Then
            lbtnApprove.Enabled = False
        Else
            lblMessage.Text = "Benutzer freigeben?"
            lbtnApprove.Enabled = True
        End If
        LockEdit(True)
        lbtnCancel.Text = " &#149;&nbsp;Abbrechen"
        SearchNotApprovedMode(False, True)
    End Sub

    Private Sub Search(Optional ByVal blnRefillDataGrid As Boolean = False, _
                       Optional ByVal blnResetSelectedIndex As Boolean = False, _
                       Optional ByVal blnResetPageIndex As Boolean = False, _
                       Optional ByVal blnClearCache As Boolean = False, _
                       Optional ByVal blnNotApproved As Boolean = False)
        ClearEdit()
        If Not blnNotApproved Then
            SearchMode()
        Else
            SearchNotApprovedMode(True, False)
        End If
        If blnClearCache Then
            'Dim _context As HttpContext = HttpContext.Current
            '_context.Cache.Remove("myUserListView")
            Session.Remove("myUserListview")
        End If
        If blnResetSelectedIndex Then dgSearchResult.SelectedIndex = -1
        If blnResetPageIndex Then dgSearchResult.CurrentPageIndex = 0
        If blnRefillDataGrid Then FillDataGrid(blnNotApproved)
    End Sub

    Private Sub Log(ByVal strIdentification As String, ByVal strDescription As String, ByVal tblParameters As DataTable, Optional ByVal strCategory As String = "APP")
        Dim logApp As New Base.Kernel.Logging.Trace(m_User.App.Connectionstring, m_User.App.SaveLogAccessSAP, m_User.App.LogLevel)

        ' strCategory
        Dim strUserName As String = m_User.UserName ' strUserName
        Dim strSessionID As String = Session.SessionID ' strSessionID
        Dim intSource As Integer = CInt(Request.QueryString("AppID")) ' intSource 
        Dim strTask As String = "Admin - Benutzerverwaltung" ' strTask
        ' strIdentification
        ' strDescription
        Dim strCustomerName As String = m_User.CustomerName ' strCustomername
        Dim blnIsTestUser As Boolean = m_User.IsTestUser ' blnIsTestUser
        Dim intSeverity As Integer = 0 ' intSeverity 

        logApp.WriteEntry(strCategory, strUserName, strSessionID, intSource, strTask, strIdentification, strDescription, strCustomerName, m_User.Customer.CustomerId, blnIsTestUser, intSeverity, tblParameters)
    End Sub

    Private Function SetOldLogParameters(ByVal intUserId As Int32, ByVal tblPar As DataTable) As DataTable
        Try
            Dim _User As New User(intUserId, m_User.App.Connectionstring)

            If tblPar Is Nothing Then
                tblPar = CreateLogTableStructure()
            End If

            Dim r = tblPar.NewRow
            r("Status") = "Alt"
            r("Benutzername") = _User.UserName
            r("Kunden- referenz") = _User.Reference
            r("Test") = _User.IsTestUser
            r("Firmen- Administrator") = _User.IsCustomerAdmin
            r("Firma") = _User.Customer.CustomerName
            r("Gruppe") = If(_User.Groups.HasGroups, _User.Groups(0).GroupName, "-")
            r("Organisations- Administrator") = _User.Organization.OrganizationAdmin
            r("Organisation") = _User.Organization.OrganizationName
            r("letzte Kennwortänderung") = String.Format("{0:dd.MM.yy}", _User.LastPasswordChange)
            r("Kennwort läuft nie ab") = _User.PasswordNeverExpires
            r("fehlgeschlagene Anmeldungen") = _User.FailedLogins.ToString
            r("Konto gesperrt") = _User.AccountIsLockedOut
            r("Angemeldet") = _User.LoggedOn
            r("ReadMessageCount") = _User.ReadMessageCount

            tblPar.Rows.Add(r)
            Return tblPar
        Catch ex As Exception
            m_App.WriteErrorText(1, m_User.UserName, "UserManagement", "SetOldLogParameters", ex.ToString)

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

    Private Function SetNewLogParameters(ByVal _User As User, ByVal tblPar As DataTable) As DataTable
        Try
            If tblPar Is Nothing Then
                tblPar = CreateLogTableStructure()
            End If

            Dim r = tblPar.NewRow
            r("Status") = "Neu"
            r("Benutzername") = txtUserName.Text
            r("Kunden- referenz") = txtReference.Text
            r("Test") = cbxTestUser.Checked
            r("Firmen- Administrator") = cbxCustomerAdmin.Checked
            r("Firma") = ddlCustomer.SelectedItem.Text
            r("Gruppe") = ddlGroups.SelectedItem.Text
            r("Organisations- Administrator") = cbxOrganizationAdmin.Checked
            r("Organisation") = ddlOrganizations.SelectedItem.Text
            r("Kennwort läuft nie ab") = cbxPwdNeverExpires.Checked
            Dim strPw As String = ""
            'Dim intCount As Integer
            'For intCount = 1 To txtPassword.Text.Length
            strPw &= "*"
            'Next
            r("neues Kennwort") = strPw
            Dim strPw2 As String = ""
            'For intCount = 1 To txtConfirmPassword.Text.Length
            strPw2 &= "*"
            'Next
            r("Kennwortbestätigung") = strPw2
            r("letzte Kennwortänderung") = String.Format("{0:dd.MM.yy}", _User.LastPasswordChange)
            r("fehlgeschlagene Anmeldungen") = _User.FailedLogins.ToString
            r("Konto gesperrt") = _User.AccountIsLockedOut
            r("Angemeldet") = _User.LoggedOn
            r("ReadMessageCount") = CInt(txtReadMessageCount.Text)

            tblPar.Rows.Add(r)
            Return tblPar
        Catch ex As Exception
            m_App.WriteErrorText(1, m_User.UserName, "UserManagement", "SetNewLogParameters", ex.ToString)

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
            .Columns.Add("Benutzername", System.Type.GetType("System.String"))
            .Columns.Add("Kunden- referenz", System.Type.GetType("System.String"))
            .Columns.Add("Test", System.Type.GetType("System.Boolean"))
            .Columns.Add("Firmen- Administrator", System.Type.GetType("System.Boolean"))
            .Columns.Add("Firma", System.Type.GetType("System.String"))
            .Columns.Add("Gruppe", System.Type.GetType("System.String"))
            .Columns.Add("Organisations- Administrator", System.Type.GetType("System.Boolean"))
            .Columns.Add("Organisation", System.Type.GetType("System.String"))
            .Columns.Add("letzte Kennwortänderung", System.Type.GetType("System.String"))
            .Columns.Add("Kennwort läuft nie ab", System.Type.GetType("System.Boolean"))
            .Columns.Add("fehlgeschlagene Anmeldungen", System.Type.GetType("System.String"))
            .Columns.Add("Konto gesperrt", System.Type.GetType("System.Boolean"))
            .Columns.Add("Angemeldet", System.Type.GetType("System.Boolean"))
            .Columns.Add("neues Kennwort", System.Type.GetType("System.String"))
            .Columns.Add("Kennwortbestätigung", System.Type.GetType("System.String"))
            .Columns.Add("ReadMessageCount", System.Type.GetType("System.Int32"))
        End With
        Return tblPar
    End Function
#End Region

#Region " Events "
    'Private Sub btnSuche_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSuche.Click
    '    Search(True, True, True, True)
    'End Sub

    Private Sub dgSearchResult_SortCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs) Handles dgSearchResult.SortCommand
        Dim strSort As String = e.SortExpression
        If Not ViewState("ResultSort") Is Nothing AndAlso ViewState("ResultSort").ToString = strSort Then
            strSort &= " DESC"
        End If
        ViewState("ResultSort") = strSort
        FillDataGrid(lblNotApprovedMode.Visible, strSort)
    End Sub

    Private Sub dgSearchResult_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles dgSearchResult.ItemCommand
        If e.CommandName = "Edit" Then
            Dim searchNotApproved As Boolean = False
            If Not viewstate("searchNotApproved") Is Nothing Then
                searchNotApproved = CBool(viewstate("searchNotApproved"))
            End If
            If Not searchNotApproved Then
                'normales edit
                EditEditMode(CInt(e.Item.Cells(0).Text))
            Else
                ApproveMode(CInt(e.Item.Cells(0).Text))
            End If
            dgSearchResult.SelectedIndex = e.Item.ItemIndex
            btnCreatePassword.Enabled = True

        ElseIf e.CommandName = "Delete" Then
            EditDeleteMode(CInt(e.Item.Cells(0).Text))
            dgSearchResult.SelectedIndex = e.Item.ItemIndex
        End If

#If DEBUG Then
        With Me
            DebugDdl(.ddlCustomer)
            DebugDdl(.ddlFilterCustomer)
            DebugDdl(.ddlFilterGroup)
            DebugDdl(.ddlFilterOrganization)
            DebugDdl(.ddlGroups)
            DebugDdl(.ddlOrganizations)
            DebugDdl(.ddlTitle)
        End With
#End If

    End Sub

#If DEBUG Then
    Private Sub DebugDdl(ByVal ddl As DropDownList)
        Dim _li As ListItem
        Dim count As Integer = 0
        For Each _li In ddl.Items
            If _li.Selected Then
                count += 1
            End If
        Next
        If count > 1 Then
            Throw New Exception(String.Concat("hier: ", ddl.ID))
        End If
    End Sub
#End If

    Private Sub dgSearchResult_PageIndexChanged(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles dgSearchResult.PageIndexChanged
        dgSearchResult.CurrentPageIndex = e.NewPageIndex
        FillDataGrid(False)
    End Sub

    Private Sub lbtnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lbtnCancel.Click
        Dim editNotApproved As Boolean = False
        lbtnDistrict.Visible = False
        If Not viewstate("editNotApproved") Is Nothing Then
            editNotApproved = CBool(viewstate("editNotApproved"))
        End If
        If Not editNotApproved Then
            Dim searchNotApproved As Boolean = False
            If Not viewstate("searchNotApproved") Is Nothing Then
                searchNotApproved = CBool(viewstate("searchNotApproved"))
            End If
            If searchNotApproved Then
                'zurücksetzen
                SearchNotApprovedMode(False, False)
                Search(True, True, True, True)
            Else
                'normales cancel
                Search(, True)
            End If
        Else
            Search(True, True, True, True, True)
            BuildExcel()
            SearchNotApprovedMode(True, False)

        End If

        Session("UsernameStart") = Nothing
        Session("LockedOutStart") = Nothing
    End Sub

    Private Sub lbtnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lbtnNew.Click
        btnCreatePassword.Enabled = False
        SearchMode(False)
        ClearEdit()

        Dim intCustomerID As Integer = CInt(ddlCustomer.SelectedItem.Value)

        If intCustomerID > 0 Then
            Dim _customer As New Customer(intCustomerID, m_User.App.Connectionstring)
            '' AutoPasswort wenn Passwort per Mail OR Kein Kundenadmin OR kein Orga-Admin
            'PasswordEditMode((Not _customer.CustomerPasswordRules.DontSendEmail) Or _
            '                 cbxNoCustomerAdmin.Checked Or _
            '                 cbxOrganizationAdmin.Checked = False)

            Dim autoPW As Boolean = False
            ' AutoPasswort wenn Passwort per Mail OR Kein Kundenadmin OR kein Orga-Admin
            If cbxNoCustomerAdmin.Checked And cbxOrganizationAdmin.Checked = False Then
                If Not _customer.CustomerPasswordRules.DontSendEmail Then
                    autoPW = True
                End If
            End If

            PasswordEditMode(autoPW)

            If Not _customer.CustomerPasswordRules.DontSendEmail Then
                chkNewPasswort.Checked = True
                chkNewPasswort.Enabled = False
            End If
        End If

        'trEmployee07.Visible = False
    End Sub

    Private Sub lbtnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lbtnSave.Click
        Dim tblLogParameter As DataTable = Nothing
        Dim strPwd As String = String.Empty
        Dim bInitialPswd As Boolean = False
        Try

            Dim _customer As New Customer(CInt(ddlCustomer.SelectedItem.Value), m_User.App.Connectionstring)
            lbtnDistrict.Visible = False
            If Not txtPassword.Visible Then 'eMail-Adresse soll nicht Pflichtfeld sein, wenn manuelle Passwort-Eingabe
                If txtMail.Text.Trim(" "c).Length = 0 Then
                    lblMessage.Text = "Bitte geben Sie eine Email-Adresse an."
                    Exit Sub
                Else
                    If (InStr(txtMail.Text, "@") = 0) Or (InStr(txtMail.Text, ".") = 0) Then
                        lblMessage.Text = "Bitte geben Sie eine Email-Adresse im Format ""account@server.de"" an."
                        Exit Sub
                    End If
                End If
            End If
            If Not IsNumeric(txtReadMessageCount.Text) Then
                lblMessage.Text = "Bitte geben Sie einen Zahlenwert für die Anzahl der Startmeldungs-Anzeigen ein."
                Exit Sub
            End If
            If ddlTitle.SelectedItem Is Nothing OrElse ddlTitle.SelectedItem.Value = "-" Then
                If Not _customer.NameInputOptional Then
                    lblMessage.Text = "Bitte wählen Sie eine Anrede aus."
                    Exit Sub
                End If
            End If
            If txtFirstName.Text = String.Empty Then
                If Not _customer.NameInputOptional Then
                    lblMessage.Text = "Bitte geben Sie einen Vornamen an."
                    Exit Sub
                End If
            End If
            If txtLastName.Text = String.Empty Then
                If Not _customer.NameInputOptional Then
                    lblMessage.Text = "Bitte geben Sie einen Nachnamen an."
                    Exit Sub
                End If
            End If

            If Trim(txtValidFrom.Text).Length > 0 Then
                If IsDate(txtValidFrom.Text) = False Then
                    lblMessage.Text = "Bitte geben Sie ein korrektes Gültig ab Datum ein."
                    Exit Sub
                End If
            End If


            If txtUserID.Text = "-1" Then
                Dim intLoop As Integer
                Dim dvForbiddenUserName As DataView
                Dim dtForbiddenUserNameAll As Kernel.ForbiddenUserNameAllList
                Dim cn As New SqlClient.SqlConnection(m_User.App.Connectionstring)
                cn.Open()

                dtForbiddenUserNameAll = New Kernel.ForbiddenUserNameAllList(cn)
                dvForbiddenUserName = dtForbiddenUserNameAll.DefaultView
                For intLoop = 0 To dvForbiddenUserName.Count - 1
                    If InStr(UCase(txtUserName.Text), UCase(CStr(dvForbiddenUserName(intLoop)("UserName")))) > 0 Then
                        lblError.Text = "Bitte wählen Sie einen anderen Namen für den neuen Benutzer!"
                        lblError.Text &= " <br>(Der Name oder ein Teil davon ist eine gesperrte Zeichenfolge.)"
                        Exit Sub
                    End If
                Next
            End If

            If chkEmployee.Checked Then
                'Der Benutzer soll als verantwortlicher Mitarbeiter verwendbar sein
                '=> Die später anzuzeigenden Daten müssen komplett sein!

                If txtLastName.Text = String.Empty Then
                    lblError.Text = "Bitte geben Sie für den Mitarbeiter einen Nachnamen an!"
                    Exit Sub
                End If

                If txtFirstName.Text = String.Empty Then
                    lblError.Text = "Bitte geben Sie für den Mitarbeiter einen Vornamen an!"
                    Exit Sub
                End If

                If txtMail.Text = String.Empty Then
                    lblError.Text = "Bitte geben Sie für den Mitarbeiter eine Email-Adresse an!"
                    Exit Sub
                End If

                If txtDepartment.Text = String.Empty Then
                    lblError.Text = "Bitte geben Sie für den Mitarbeiter eine Abteilung an!"
                    Exit Sub
                End If

                If txtPosition.Text = String.Empty Then
                    lblError.Text = "Bitte geben Sie für den Mitarbeiter eine Position an!"
                    Exit Sub
                End If

                If txtTelephone.Text = String.Empty Then
                    lblError.Text = "Bitte geben Sie für den Mitarbeiter eine Telefonnummer an!"
                    Exit Sub
                End If
            End If

            Dim _User As User
            'If _customer.NameInputOptional And Not chkEmployee.Checked Then
            '    _User = New User(CInt(txtUserID.Text), _
            '                                      txtUserName.Text, _
            '                                      txtReference.Text, _
            '                                      cbxTestUser.Checked, _
            '                                      CInt(ddlCustomer.SelectedItem.Value), _
            '                                      cbxCustomerAdmin.Checked, _
            '                                      cbxPwdNeverExpires.Checked, _
            '                                      cbxAccountIsLockedOut.Checked, _
            '                                      cbxFirstLevelAdmin.Checked, _
            '                                      chkLoggedOn.Checked, cbxOrganizationAdmin.Checked, _
            '                                      m_User.App.Connectionstring, _
            '                                      CInt(txtReadMessageCount.Text), _
            '                                      m_User.UserName, _
            '                                      cbxApproved.Checked, _
            '                                      txtStore.Text, _
            '                                      chk_Matrix1.Checked, _
            '                                      txtValidFrom.Text)
            'Else
            _User = New User(CInt(txtUserID.Text), _
                                              txtUserName.Text, _
                                              txtReference.Text, _
                                              cbxTestUser.Checked, _
                                              CInt(ddlCustomer.SelectedItem.Value), _
                                              cbxCustomerAdmin.Checked, _
                                              cbxPwdNeverExpires.Checked, _
                                              cbxAccountIsLockedOut.Checked, _
                                              cbxFirstLevelAdmin.Checked, _
                                              chkLoggedOn.Checked, cbxOrganizationAdmin.Checked, _
                                              m_User.App.Connectionstring, _
                                              CInt(txtReadMessageCount.Text), _
                                              m_User.UserName, _
                                              cbxApproved.Checked, _
                                              txtFirstName.Text, _
                                              txtLastName.Text, _
                                              ddlTitle.SelectedItem.Value, _
                                              txtStore.Text, _
                                              chk_Matrix1.Checked, _
                                              txtValidFrom.Text)
            'End If

            _User.Email = txtMail.Text
            _User.Telephone = txtPhone.Text 'Telefonnr. des Benutzers nicht des Mitarbeiters
            _User.Employee = chkEmployee.Checked
            _User.Picture = Len(lblPictureName.Text) > 0
            _User.HierarchyID = CInt(ddlHierarchy.SelectedValue)
            _User.Department = txtDepartment.Text
            _User.Position = txtPosition.Text
            _User.PhoneEmployee = txtTelephone.Text
            _User.Fax = txtFax.Text

            Dim strLogMsg As String = "User anlegen"
            Dim strTemp As String = txtUserID.Text
            If Not (txtUserID.Text = "-1") Then
                strLogMsg = "User ändern"
                tblLogParameter = SetOldLogParameters(CInt(txtUserID.Text), tblLogParameter)
            End If

            Dim intGroupID As Integer

            If Not ddlGroups.Items.Count = 0 Then
                intGroupID = CInt(ddlGroups.SelectedItem.Value)
            Else
                intGroupID = 0
            End If

            If intGroupID > 0 Then
                'Gruppe ausgewählt
                If Not _User.Groups.IsInGroups(intGroupID) Then
                    'gewaehlte Gruppe ist neu
                    'vorhandene Gruppen loeschen
                    '(da nur eine Gruppe je User erlaubt)
                    If Not _User.Groups.Count = 0 Then
                        Dim gr As Group
                        For Each gr In _User.Groups
                            gr.MarkDeleted()
                        Next
                    End If
                    'neue Gruppe hinzufuegen
                    '_User.Groups.Add(New DADWebClass.Group(CInt(ddlGroups.SelectedItem.Value), CInt(ddlCustomer.SelectedItem.Value)))
                    _User.Groups.Add(New Group(intGroupID, CInt(ddlCustomer.SelectedItem.Value)))
                End If
            Else
                lblError.Text = "Bitte geben Sie für den Mitarbeiter eine Gruppe an!"
                Exit Sub
            End If

            Dim intOrganizationID As Integer
            If Not ddlOrganizations.Items.Count = 0 Then
                intOrganizationID = CInt(ddlOrganizations.SelectedItem.Value)
            Else
                intOrganizationID = 0
            End If

            Dim blnSuccess As Boolean = False

            'User speichern
            If _User.Save() Then

                ' Passwort zu User speichern

                ' Passwort generieren    Wenn neuer Benutzer OR neues Passwort gewählt OR neues Passwort eingegeben
                If txtUserID.Text = "-1" _
                    OrElse chkNewPasswort.Checked = True _
                    OrElse txtPassword.Text <> String.Empty Then '_
                    'OrElse (cbxAccountIsLockedOut.Checked = False And CBool(Session("LockedOutStart")) = True) Then
                    If _customer.CustomerPasswordRules.DontSendEmail Or Not _User.HighestAdminLevel = AdminLevel.None Or cbxOrganizationAdmin.Checked Then   ' Wenn Passwort nicht per Mail txtFelder abfragen
                        If txtPassword.Text = String.Empty Then
                            lblError.Text = "Bitte geben Sie für den neuen Benutzer ein Passwort an!"
                            Exit Sub
                        ElseIf txtPassword.Text <> txtConfirmPassword.Text Then
                            lblError.Text = "Die eingegebenen Passwörter stimmen nicht überein!"
                            Exit Sub
                        End If

                        strPwd = txtPassword.Text
                    Else    ' Sonst nach Kundeneinstellungen ein neues Passwort generieren
                        strPwd = _customer.CustomerPasswordRules.CreateNewPasswort(lblError.Text)
                        'txtPassword.Attributes.Add("value", strPwd)
                        'txtConfirmPassword.Attributes.Add("value", strPwd)
                        bInitialPswd = True
                    End If
                End If

                ' Wenn Passwortänderung
                If Not (strPwd = String.Empty) Then
                    Dim pword As String = strPwd
                    Dim pwordconfirm As String = strPwd

                    If Not _User.ChangePasswordNew("", pword, pwordconfirm, m_User.UserName, True, bInitialPswd) Then
                        txtUserID.Text = _User.UserID.ToString
                        lblError.Text = _User.ErrorMessage
                    Else
                        blnSuccess = True
                    End If
                Else
                    blnSuccess = True
                End If

                _User.SetLastLogin(Now)
                _User.Organization.ReAssignUserToOrganization(m_User.UserName, strTemp, _User.UserID, intOrganizationID, cbxOrganizationAdmin.Checked, m_User.App.Connectionstring)

                'EmployeeInfos ggf. speichern
            Else
                lblError.Text = _User.ErrorMessage
            End If
            tblLogParameter = SetNewLogParameters(_User, tblLogParameter)
            Log(_User.UserID.ToString, strLogMsg, tblLogParameter)

            If blnSuccess Then
                lblMessage.Text = "Die Änderungen wurden gespeichert."
                Search(True, True, , True)
                Dim errorMessage As String = ""

                ' Versandt von neuen Benutzerdaten erst nach Freigabe, daher in lbtnApproved_Click

                ' Ausnahme für Orgaadmins und Kundenadmins, die Benutzer anlegen ++++++++++++++++++
                If txtUserID.Text = "-1" And cbxApproved.Checked And Session("UsernameStart") Is Nothing And Not strPwd = String.Empty Then
                    ' Neuanlage Benutzer (ohne Adminrechte) Authentifizierungs-Email versenden
                    If _User.HighestAdminLevel = AdminLevel.None Then
                        ' Wenn Passwort und Username per Mail dann Validierungsprozess
                        If Not _User.Customer.CustomerUsernameRules.DontSendEmail And Not _User.Customer.CustomerPasswordRules.DontSendEmail Then

                            Dim LinkKey As String = ""
                            'Dim pword As String = ""
                            Dim RightKey As String = ""
                            Dim WrongKey As String = ""

                            'Linkschlüssel generieren
                            LinkKey = _User.Customer.CustomerPasswordRules.CreateNewPasswort(lblError.Text)

                            'Passwort generieren
                            'pword = _User.Customer.CustomerPasswordRules.CreateNewPasswort(lblError.Text)
                            '_User.ChangePasswordNew("", pword, pword, "Freigabeprozess - " + m_User.UserName, True, False)

                            'Erstellt einen Eintrag in der Tabelle für den Freigabe-Workflow
                            InsertIntoWebUserUpload(_User.UserID, strPwd, _User.UserName, LinkKey, RightKey, WrongKey, _User.Customer.LoginLinkID)

                            'Mail versenden
                            If Not _User.SendUsernameMail(errorMessage, _User.Customer.LoginLinkID, RightKey, WrongKey, m_User, False) Then
                                lblError.Text = errorMessage
                            Else
                                'Status auf erfolgreich versandt setzen
                                _User.UpdateWebUserUploadMailSend(True)
                            End If

                        Else
                            ' Sonst prüfen ob Passwort oder Username per Mail und diese verschicken
                            If _User.Customer.CustomerUsernameRules.DontSendEmail Then
                                If Not _User.Customer.CustomerPasswordRules.DontSendEmail Then
                                    _User.ChangePasswordNew("", strPwd, strPwd, "Freigabeprozess - " + m_User.UserName, True, False)
                                    _User.SendPasswordMail(strPwd, errorMessage, False)
                                End If
                            ElseIf _User.Customer.CustomerPasswordRules.DontSendEmail Then
                                _User.SendUsernameMail(errorMessage, False, False)
                            End If
                        End If
                    End If
                Else ' Neue Benutzer nicht freigegeben und geänderte Benutzer

                    ' Neue Benutzer werden im lbtnApproved_Click behandelt
                    ' für alle anderen gilt

                    ' Passwort per Mail +++++++++++++++++++++++++
                    ' Kein Passwort bis sendPW = True
                    Dim sendPW As Boolean = False

                    ' Falls Benutzer kein Admin und Passwort Generierungsregeln ein Passwort liefern
                    If _User.HighestAdminLevel = AdminLevel.None And strPwd <> String.Empty And Not (Session("UsernameStart") Is Nothing) Then
                        sendPW = True
                    End If

                    If sendPW Then
                        ' sendPasswordMail prüft Restriktionen fürs senden 
                        If Not _User.SendPasswordMail(strPwd, errorMessage, False) Then
                            lblError.Text = errorMessage
                        End If
                    End If

                    ' Benutzername per Mail +++++++++++++++++++++++++
                    ' Wenn vorhandener Benutzer geändert
                    If Not (Session("UsernameStart") Is Nothing) Then
                        ' Username geändert AND Username nicht leer AND User kein Admin
                        If _User.UserName <> CStr(Session("UsernameStart")) And _User.UserName <> String.Empty And _
                            _User.HighestAdminLevel = AdminLevel.None And Not cbxOrganizationAdmin.Checked Then
                            If Not _User.SendUsernameChangedMail(errorMessage, False) Then
                                lblError.Text = errorMessage
                            End If
                        End If
                        ' Falls Benutzer entsperrt wurde
                        If Not Session("LockedOutStart") Is Nothing Then
                            If _User.Approved And _User.AccountIsLockedOut = False And CBool(Session("LockedOutStart")) = True Then
                                If Not _User.SendUserUnlockMail(errorMessage, m_User, False) Then
                                    lblError.Text = errorMessage
                                End If
                            End If
                        End If
                    End If

                End If

                Session("UsernameStart") = Nothing
                Session("LockedOutStart") = Nothing

                If _customer.ShowDistrikte AndAlso Session("Changed") = 1 Then

                    ErmitteleRechteAusCheckBoxen(Matrix.Rows.GetEnumerator, m_Rights)
                    Dim selectedDistrict As String = GetSelectedDistrict()
                    SetzeVorbelegswertFuerDistrikt(selectedDistrict, Session("UserID").ToString, _customer.KUNNR, True, False)
                    SetzeVorbelegswertFuerDistrikt(selectedDistrict, Session("UserID").ToString, _customer.KUNNR, False, True)
                    SetDistrictRights(m_Rights)

                End If
            End If

        Catch ex As Exception
            m_App.WriteErrorText(1, m_User.UserName, "UserManagement", "lbtnSave_Click", ex.ToString)

            lblError.Text = ex.Message
            If Not ex.InnerException Is Nothing Then
                lblError.Text &= ": " & ex.InnerException.Message
            End If
            tblLogParameter = New DataTable
            Log(txtUserID.Text, lblError.Text, tblLogParameter, "ERR")
        End Try
    End Sub

    Private Sub lbtnDelete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lbtnDelete.Click
        Dim tblLogParameter As DataTable
        Try
            Dim _User As New User()
            tblLogParameter = New DataTable
            tblLogParameter = SetOldLogParameters(CInt(txtUserID.Text), tblLogParameter)
            If _User.Delete(CInt(txtUserID.Text), m_User.App.Connectionstring, m_User.UserName) Then
                lblMessage.Text = "Das Benutzerkonto wurde gelöscht."
                Search(True, True, , True)
            Else
                lblError.Text = _User.ErrorMessage
            End If
            Log(_User.UserID.ToString, "User löschen", tblLogParameter)
        Catch ex As Exception
            m_App.WriteErrorText(1, m_User.UserName, "UserManagement", "lbtnDelete_Click", ex.ToString)

            lblError.Text = ex.Message
            If Not ex.InnerException Is Nothing Then
                lblError.Text &= ": " & ex.InnerException.Message
            End If
            tblLogParameter = New DataTable
            Log(txtUserID.Text, lblError.Text, tblLogParameter, "ERR")
        End Try
        Session("UsernameStart") = Nothing
        Session("LockedOutStart") = Nothing
    End Sub

    Private Sub ddlFilterCustomer_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ddlFilterCustomer.SelectedIndexChanged
        Dim intCustomerID As Integer = CInt(ddlFilterCustomer.SelectedItem.Value)
        Dim cn As New SqlClient.SqlConnection(m_User.App.Connectionstring)
        cn.Open()
        If intCustomerID > 0 Then
            ddlCustomer.SelectedItem.Selected = False
            ddlCustomer.Items.FindByValue(intCustomerID.ToString).Selected = True
            FillGroups(intCustomerID, cn)
            FillOrganizations(intCustomerID, cn)
            Dim _customer As New Customer(intCustomerID, m_User.App.Connectionstring)
            '' AutoPasswort wenn Passwort per Mail OR Kein Kundenadmin OR kein Orga-Admin
            'PasswordEditMode((Not _customer.CustomerPasswordRules.DontSendEmail) Or _
            '                 cbxNoCustomerAdmin.Checked Or _
            '                 cbxOrganizationAdmin.Checked = False)
            Dim autoPW As Boolean = False
            ' AutoPasswort wenn Passwort per Mail OR Kein Kundenadmin OR kein Orga-Admin
            If cbxNoCustomerAdmin.Checked And cbxOrganizationAdmin.Checked = False Then
                If Not _customer.CustomerPasswordRules.DontSendEmail Then
                    autoPW = True
                End If
            End If

            PasswordEditMode(autoPW)
            If Not _customer.CustomerPasswordRules.DontSendEmail Then
                chkNewPasswort.Checked = True
                chkNewPasswort.Enabled = False
            End If
        Else
            Dim dtGroups As New Kernel.GroupList(intCustomerID, cn, m_User.Customer.AccountingArea)
            FillGroup(ddlFilterGroup, True, dtGroups)
            Dim dtOrganizations As New OrganizationList(intCustomerID, cn, m_User.Customer.AccountingArea)
            FillOrganization(ddlFilterOrganization, True, dtOrganizations)
        End If
    End Sub

    Private Sub ddlCustomer_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ddlCustomer.SelectedIndexChanged
        Dim intCustomerID As Integer = CInt(ddlCustomer.SelectedItem.Value)
        Dim cn As New SqlClient.SqlConnection(m_User.App.Connectionstring)
        cn.Open()
        Dim dtGroups As New Kernel.GroupList(intCustomerID, cn, m_User.Customer.AccountingArea, , True)
        FillGroup(ddlGroups, False, dtGroups)
        Dim dtOrganizations As New OrganizationList(intCustomerID, cn, m_User.Customer.AccountingArea)
        FillOrganization(ddlOrganizations, False, dtOrganizations)
        Dim _customer As New Customer(intCustomerID, m_User.App.Connectionstring)

        ' AutoPasswort wenn Passwort per Mail OR Kein Kundenadmin OR kein Orga-Admin
        'PasswordEditMode((Not _customer.CustomerPasswordRules.DontSendEmail) Or _
        '                 cbxNoCustomerAdmin.Checked Or _
        '                 cbxOrganizationAdmin.Checked = False)

        Dim autoPW As Boolean = False
        ' AutoPasswort wenn Passwort per Mail OR Kein Kundenadmin OR kein Orga-Admin
        If cbxNoCustomerAdmin.Checked And cbxOrganizationAdmin.Checked = False Then
            If Not _customer.CustomerPasswordRules.DontSendEmail Then
                autoPW = True
            End If
        End If

        PasswordEditMode(autoPW)
       
        'NameEditMode(Not _customer.CustomerPasswordRules.NameInputOptional)
    End Sub

    Private Sub btnSuche_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSuche.Click
        If Not lblNotApprovedMode.Visible Then
            'normale Suche
            Search(True, True, True, True)
        Else
            'nur nicht freigegebene
            Search(True, True, True, True, True)
        End If
        BuildExcel()
    End Sub

    Private Sub lbtnApprove_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lbtnApprove.Click
        Dim _User As New User(CInt(txtUserID.Text), m_User.App.Connectionstring)
        _User.Approve(m_User.UserName)
        Dim errorMessage As String = ""

        ' Neuanlage Benutzer (ohne Adminrechte) Authentifizierungs-Email versenden
        If _User.HighestAdminLevel = AdminLevel.None Then
            ' Wenn Passwort und Username per Mail dann Validierungsprozess
            If Not _User.Customer.CustomerUsernameRules.DontSendEmail And Not _User.Customer.CustomerPasswordRules.DontSendEmail Then

                Dim LinkKey As String = ""
                Dim pword As String = ""
                Dim RightKey As String = ""
                Dim WrongKey As String = ""

                'Linkschlüssel generieren
                LinkKey = _User.Customer.CustomerPasswordRules.CreateNewPasswort(lblError.Text)

                'Passwort generieren
                pword = _User.Customer.CustomerPasswordRules.CreateNewPasswort(lblError.Text)
                _User.ChangePasswordNew("", pword, pword, "Freigabeprozess - " + m_User.UserName, True, False)

                'Erstellt einen Eintrag in der Tabelle für den Freigabe-Workflow
                InsertIntoWebUserUpload(_User.UserID, pword, _User.UserName, LinkKey, RightKey, WrongKey, _User.Customer.LoginLinkID)

                'Mail versenden
                If Not _User.SendUsernameMail(errorMessage, _User.Customer.LoginLinkID, RightKey, WrongKey, m_User, False) Then
                    lblError.Text = errorMessage
                Else
                    'Status auf erfolgreich versandt setzen
                    _User.UpdateWebUserUploadMailSend(True)
                End If
            Else
                ' Sonst prüfen ob Passwort oder Username per Mail und diese verschicken
                If _User.Customer.CustomerUsernameRules.DontSendEmail Then
                    If Not _User.Customer.CustomerPasswordRules.DontSendEmail Then
                        Dim pword As String = ""
                        'Passwort generieren
                        pword = _User.Customer.CustomerPasswordRules.CreateNewPasswort(lblError.Text)
                        _User.ChangePasswordNew("", pword, pword, "Freigabeprozess - " + m_User.UserName, True, False)
                        _User.SendPasswordMail(pword, errorMessage, False)
                    End If
                ElseIf _User.Customer.CustomerPasswordRules.DontSendEmail Then
                    _User.SendUsernameMail(errorMessage, False, False)
                End If
            End If

        End If

        lbtnNotApproved_Click(sender, e)
    End Sub

    Private Sub lbtnNotApproved_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lbtnNotApproved.Click
        SearchNotApprovedMode(True, False)
        btnSuche_Click(sender, e)
    End Sub

    Private Sub lbtnDistrict_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lbtnDistrict.Click
        Try
            Page_LoadDistikte()
            trMatrix.Visible = True
            Session("UserID") = txtUserID.Text

            'Response.Redirect("../Applications/AppFFD/Forms/Change16.aspx?AppID=601")
        Catch ex As Exception
            lblError.Text = ex.Message
        End Try

    End Sub

    Private Sub chk_Matrix1_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chk_Matrix1.CheckedChanged
        If chk_Matrix1.Checked = False Then
            Matrix.Visible = True
            Session("Changed") = 1
            Dim _User As New User(CInt(txtUserID.Text), m_User.App.Connectionstring)
            chk_Matrix1.Enabled = False
            Fill_MatrixEmptyRights(_User.Customer.KUNNR, "") ' zurücksetzen der Matrix 
        End If
    End Sub

    Protected Sub btnUpload_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnUpload.Click
        Try
            If Not (upFile.PostedFile.FileName = String.Empty) Then
                Dim fname As String = upFile.PostedFile.FileName
                If (upFile.PostedFile.ContentLength > CType(System.Configuration.ConfigurationManager.AppSettings("MaxUploadSize"), Integer)) Then
                    lblError.Text = "Datei '" & Right(fname, fname.Length - fname.LastIndexOf("\") - 1).ToUpper & "' ist zu gross (>300 KB)."
                    Exit Sub
                End If
                '------------------
                If Right(upFile.PostedFile.FileName.ToUpper, 4) <> ".JPG" Then
                    lblError.Text = "Es können nur Bilddateien im JPG - Format verarbeitet werden."
                    Exit Sub
                End If

                'upFile.PostedFile
                If Not (upFile.PostedFile Is Nothing) Then
                    Dim info As System.IO.FileInfo
                    Dim uFile As System.Web.HttpPostedFile = upFile.PostedFile

                    Dim fnameNew As String = System.Configuration.ConfigurationManager.AppSettings("UploadPathLocal") & "responsible\" & txtUserID.Text & ".jpg"
                    info = New System.IO.FileInfo(fnameNew)
                    If (info.Exists) Then
                        System.IO.File.Delete(fnameNew)
                    End If

                    uFile.SaveAs(fnameNew)
                    info = New System.IO.FileInfo(fnameNew)
                    If Not (info.Exists) Then
                        lblError.Text = "Fehler beim Speichern."
                    End If

                    Dim _User As New User(CInt(txtUserID.Text), m_User.App.Connectionstring)
                    _User.SetEmployeePicture(True, m_User.UserName)

                    FillEdit(CInt(txtUserID.Text))
                End If
            End If
        Catch ex As Exception
            lblError.Text = "Fehler beim Hochladen. (" & ex.ToString & ")"
        End Try
    End Sub

    Protected Sub btnRemove_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnRemove.Click
        Try
            Dim info As System.IO.FileInfo

            Dim fnameNew As String = System.Configuration.ConfigurationManager.AppSettings("UploadPathLocal") & "responsible\" & txtUserID.Text & ".jpg"
            info = New System.IO.FileInfo(fnameNew)
            If (info.Exists) Then
                System.IO.File.Delete(fnameNew)
            End If

            Dim _User As New User(CInt(txtUserID.Text), m_User.App.Connectionstring)
            _User.SetEmployeePicture(False, m_User.UserName)

            FillEdit(CInt(txtUserID.Text))
        Catch ex As Exception
            lblError.Text = "Fehler beim Löschen. (" & ex.ToString & ")"
        End Try
    End Sub

    Protected Sub imgbCalendar_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgbCalendar.Click
        calLastLogin.Visible = Not calLastLogin.Visible
    End Sub

    Protected Sub calLastLogin_SelectionChanged(ByVal sender As Object, ByVal e As EventArgs) Handles calLastLogin.SelectionChanged
        txtLastLoginBefore.Text = calLastLogin.SelectedDate.ToShortDateString
        calLastLogin.Visible = Not calLastLogin.Visible
    End Sub

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
        SetEndASPXAccess(Me)
    End Sub

    Private Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Unload
        SetEndASPXAccess(Me)
    End Sub

    Protected Sub btnEmpty_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnEmpty.Click
        If Not lblNotApprovedMode.Visible Then
            'normale Suche
            Search(True, True, True, True)
        Else
            'nur nicht freigegebene
            Search(True, True, True, True, True)
        End If
        BuildExcel()
    End Sub

    Private Sub cbxFirstLevelAdmin_CheckedChanged(sender As Object, e As System.EventArgs) Handles cbxFirstLevelAdmin.CheckedChanged
        If Not ddlCustomer.SelectedItem Is Nothing Then
            Dim intddlCustID As Integer = CInt(ddlCustomer.SelectedItem.Value)
            Dim _customer As New Customer(intddlCustID, m_User.App.Connectionstring)
            Dim autoPW As Boolean = False
            ' AutoPasswort wenn Passwort per Mail OR Kein Kundenadmin OR kein Orga-Admin
            If cbxNoCustomerAdmin.Checked And cbxOrganizationAdmin.Checked = False Then
                If Not _customer.CustomerPasswordRules.DontSendEmail Then
                    autoPW = True
                End If
            End If

            PasswordEditMode(autoPW)
        Else
            PasswordEditMode(True)
        End If
    End Sub

    Protected Sub cbxOrganizationAdmin_CheckedChanged(sender As Object, e As EventArgs) Handles cbxOrganizationAdmin.CheckedChanged
        If Not ddlCustomer.SelectedItem Is Nothing Then
            Dim intddlCustID As Integer = CInt(ddlCustomer.SelectedItem.Value)
            Dim _customer As New Customer(intddlCustID, m_User.App.Connectionstring)
            Dim autoPW As Boolean = False
            ' AutoPasswort wenn Passwort per Mail OR Kein Kundenadmin OR kein Orga-Admin
            If cbxNoCustomerAdmin.Checked And cbxOrganizationAdmin.Checked = False Then
                If Not _customer.CustomerPasswordRules.DontSendEmail Then
                    autoPW = True
                End If
            End If

            PasswordEditMode(autoPW)
        Else
            PasswordEditMode(True)
        End If
    End Sub

    Protected Sub cbxNoCustomerAdmin_CheckedChanged(sender As Object, e As EventArgs) Handles cbxNoCustomerAdmin.CheckedChanged
        If Not ddlCustomer.SelectedItem Is Nothing Then
            Dim intddlCustID As Integer = CInt(ddlCustomer.SelectedItem.Value)
            Dim _customer As New Customer(intddlCustID, m_User.App.Connectionstring)
            Dim autoPW As Boolean = False
            ' AutoPasswort wenn Passwort per Mail OR Kein Kundenadmin OR kein Orga-Admin
            If cbxNoCustomerAdmin.Checked And cbxOrganizationAdmin.Checked = False Then
                If Not _customer.CustomerPasswordRules.DontSendEmail Then
                    autoPW = True
                End If
            End If

            PasswordEditMode(autoPW)
        Else
            PasswordEditMode(True)
        End If
    End Sub

    Protected Sub cbxCustomerAdmin_CheckedChanged(sender As Object, e As EventArgs) Handles cbxCustomerAdmin.CheckedChanged
        If Not ddlCustomer.SelectedItem Is Nothing Then
            Dim intddlCustID As Integer = CInt(ddlCustomer.SelectedItem.Value)
            Dim _customer As New Customer(intddlCustID, m_User.App.Connectionstring)
            Dim autoPW As Boolean = False
            ' AutoPasswort wenn Passwort per Mail OR Kein Kundenadmin OR kein Orga-Admin
            If cbxNoCustomerAdmin.Checked And cbxOrganizationAdmin.Checked = False Then
                If Not _customer.CustomerPasswordRules.DontSendEmail Then
                    autoPW = True
                End If
            End If

            PasswordEditMode(autoPW)
        Else
            PasswordEditMode(True)
        End If
    End Sub
#End Region

    Private Sub BuildExcel()
        Dim _context As HttpContext = HttpContext.Current
        Dim dvUser As DataView
        Dim tableExport As New DataTable()

        Dim strFileName As String = Format(Now, "yyyyMMdd_HHmmss_") & m_User.UserName & ".xls"
        Dim showExcel As Boolean
        Dim customerList As String

        lnkExcel.Visible = False
        showExcel = False
        customerList = ConfigurationManager.AppSettings("ShowExcelLinkUserDownload").ToString  'Liste aller Kundennummern, für die der Excel-Download sichtbar sein soll...

        If (m_User.HighestAdminLevel = AdminLevel.Master) Then
            showExcel = True
        End If
        If (m_User.HighestAdminLevel = AdminLevel.Customer) Then
            If (Not (m_User.Customer.KUNNR Is Nothing)) AndAlso (customerList.IndexOf(m_User.Customer.KUNNR.ToString) >= 0) Then    'Kundennummer in Liste drin?
                showExcel = True                        'Ja, Link sichtbar machen...
            End If
        End If

        If (showExcel = True) Then
            'dvUser = CType(_context.Cache("myUserListView"), DataView)
            dvUser = CType(Session("myUserListView"), DataView)
            tableExport = dvUser.Table      'DAD-Admin darf alles sehen

            'If (m_User.Customer.AccountingArea <> -1) Then
            '    dvUser.RowFilter = "CustomerID = " & m_User.Customer.CustomerId     'Nochmal filtern...Sicht
            '    'Wenn Zeilen in der Sicht <> Zeilen in Excel, dann Tabelle löschen. Das darf nicht sein!
            '    If dvUser.Count <> dvUser.Table.Rows.Count Then
            '        tableExport = Nothing
            '        lnkExcel.Visible = False
            '    End If
            'End If

            Dim objExcelExport As New Base.Kernel.Excel.ExcelExport()

            Try
                Base.Kernel.Excel.ExcelExport.WriteExcel(tableExport, ConfigurationManager.AppSettings("ExcelPath") & strFileName)
            Catch ex As Exception
            End Try
            lnkExcel.NavigateUrl = "/Portal/Temp/Excel/" & strFileName
            lnkExcel.Visible = True
        End If

    End Sub

    '############################# Distriktzuordnung #############################################
#Region "Helper"

    '------------
    'Liefert einen Filterausdruck für das Rights-DataTable
    '------------
    Private Function GetFilterExpression(ByVal kundennr As String, ByVal districtID As String, ByVal groupname As String, ByVal vorbelegung As String, ByVal ApplicationId As String, ByVal ohneGeloeschte As Boolean, ByVal invertDistrikt As Boolean) As String

        'Werte aufbereiten
        kundennr = Right("0000000000" + kundennr, 10)

        'Ausdruck erstellen
        Dim needAND As Boolean = False

        Dim res As New System.Text.StringBuilder()
        If Not kundennr Is Nothing Then
            If needAND Then res.Append(" AND ")
            res.Append(" KUNNR = '" + kundennr + "' ")
            needAND = True
        End If
        If Not districtID Is Nothing Then
            If needAND Then res.Append(" AND ")
            res.Append(" DISTRIKT ")
            If invertDistrikt Then
                res.Append(" <> ")
            Else
                res.Append(" = ")
            End If
            res.Append(" '" + districtID + "' ")
            needAND = True
        End If
        If Not groupname Is Nothing Then
            If needAND Then res.Append(" AND ")
            res.Append(" BENGRP = '" + groupname + "' ") ' groupname=UserID
            needAND = True
        End If
        If Not vorbelegung Is Nothing Then
            If needAND Then res.Append(" AND ")
            res.Append(" VORBELEGT = '" + vorbelegung + "' ")
            needAND = True
        End If
        If Not ApplicationId Is Nothing Then
            If needAND Then res.Append(" AND ")
            res.Append(" ANWENDUNG = '" + ApplicationId + "' ")
            needAND = True
        End If
        If ohneGeloeschte Then
            If needAND Then res.Append(" AND ")
            res.Append(" LOEKZ = '' ")
            needAND = True
        End If

        Return res.ToString()

    End Function

    '-----------
    'Liefert alle Rights-Checkboxes für alle, ein Distrikt oder alle Distrikte außer dem einem
    '-----------
    Private Function GetRightsCheckboxes(ByVal ctrls As IEnumerator, ByVal DistriktID As String, ByVal invertDistrikt As Boolean) As CheckBox()
        Dim cbox As CheckBox
        Dim row As TableRow
        Dim cell As TableCell
        Dim res As New ArrayList()
        While ctrls.MoveNext
            If TypeOf ctrls.Current Is TableRow Then
                row = CType(ctrls.Current, TableRow)
                If row.HasControls Then
                    res.AddRange(GetRightsCheckboxes(row.Controls.GetEnumerator(), DistriktID, invertDistrikt))
                End If
            End If
            If TypeOf ctrls.Current Is TableCell Then
                cell = CType(ctrls.Current, TableCell)
                If cell.HasControls Then
                    res.AddRange(GetRightsCheckboxes(cell.Controls.GetEnumerator(), DistriktID, invertDistrikt))
                End If
            End If
            If TypeOf ctrls.Current Is CheckBox Then
                cbox = CType(ctrls.Current, CheckBox)
                If Not cbox.Attributes("ApplicationID") Is Nothing Then
                    'Alle Distrikte ODER (einen distrikt ODER die Invertierung)
                    If (DistriktID Is Nothing) OrElse (invertDistrikt Xor DistriktID = cbox.Attributes("DistriktId")) Then
                        res.Add(cbox)
                    End If
                End If
            End If
        End While

        Return CType(res.ToArray(GetType(CheckBox)), CheckBox())

    End Function

    '-------
    'Liefert den ausgewählten Radio-Button
    '-------
    Private Function GetSelectedDistrictRadioButton(ByVal ctrls As IEnumerator) As RadioButton
        Dim rdo As RadioButton
        Dim row As TableRow
        Dim cell As TableCell
        While ctrls.MoveNext
            If TypeOf ctrls.Current Is TableRow Then
                row = CType(ctrls.Current, TableRow)
                If row.HasControls Then
                    Dim res As RadioButton = GetSelectedDistrictRadioButton(row.Controls.GetEnumerator())
                    If Not res Is Nothing Then Return res
                End If
            End If
            If TypeOf ctrls.Current Is TableCell Then
                cell = CType(ctrls.Current, TableCell)
                If cell.HasControls Then
                    Dim res As RadioButton = GetSelectedDistrictRadioButton(cell.Controls.GetEnumerator())
                    If Not res Is Nothing Then Return res
                End If
            End If
            If TypeOf ctrls.Current Is RadioButton Then
                rdo = CType(ctrls.Current, RadioButton)
                If rdo.Checked Then
                    Return rdo
                End If
            End If
        End While

        Return Nothing

    End Function

    '-------
    'Liefert den ausgewählten Distrikt
    '-------
    Private Function GetSelectedDistrict() As String
        Dim rdo As RadioButton = GetSelectedDistrictRadioButton(Matrix.Controls.GetEnumerator())
        If rdo Is Nothing Then
            Return String.Empty
            ' Throw New Exception("Bitte wählen Sie einen Distrikt als Vorbelegung aus.")
        Else
            Return rdo.Attributes("DistriktId")
        End If

    End Function
    Public Function WriteMatrixFilld(ByVal iUserID As Integer) As Int32
        Dim cn As SqlClient.SqlConnection
        Dim cmdCommand As SqlClient.SqlCommand
        Dim intTemp As Int32 = 0
        cn = New SqlClient.SqlConnection(ConfigurationManager.AppSettings("Connectionstring"))
        Try

            cn.Open()
            cmdCommand = New SqlClient.SqlCommand("Update WebUser Set Matrix=1 where UserID=" & iUserID)
            cmdCommand.Connection = cn
            cmdCommand.CommandType = CommandType.Text
            cmdCommand.ExecuteNonQuery()
            'strTemp = ""
            'intTemp = CType(cmdTable.ExecuteScalar, Int32)

        Catch ex As Exception
            lblError.Text = ex.Message
            intTemp = -1
        End Try
        cn.Close()
        Return intTemp
    End Function

    '-------
    'Sperrung durch
    '-------
    Private Function GetHistoryInfos(ByVal objUser As User) As String

        Dim cn As New SqlClient.SqlConnection(m_User.App.Connectionstring)
        Try
            cn.Open()
            If cn.State = ConnectionState.Closed Then
                cn.Open()
            End If
            Dim cmd As SqlClient.SqlCommand = New SqlClient.SqlCommand("SELECT LastChangedBy,Max(ID) as ID  " & _
                                                          "FROM AdminHistory_User  " & _
                                                          "WHERE Username = @Username And " & _
                                                          "Action='Benutzer gesperrt' Group By LastChangedBy ORDER BY ID DESC", cn)

            cmd.Parameters.AddWithValue("@Username", objUser.UserName)
            Dim sUser As String = CStr(cmd.ExecuteScalar)

            If Not sUser Is Nothing Then
                Return sUser
            Else
                Return ""
            End If
        Catch ex As Exception
            Throw
        Finally
            If cn.State <> ConnectionState.Closed Then
                cn.Close()
            End If
        End Try
    End Function
#End Region

#Region "SAP / BAPI-Aufrufe"

    '-----------
    'Liest die Distrikte und Berechtigungen aus SAP
    '-----------
    Private Sub ReadDistrictsAndRights()
        Dim Districts As Base.Kernel.Common.Search
        Dim SessionID As String = Session.SessionID.ToString
        Dim AppID As String = "601"
        Dim i As Integer
        Dim _customer As New Customer(CInt(ddlCustomer.SelectedItem.Value), m_User.App.Connectionstring)
        Dim _User As New User(CInt(txtUserID.Text), m_User.App.Connectionstring)

        If Session("AppID") Is Nothing Then
            Session.Add("AppID", AppID)
        End If
        Districts = New Base.Kernel.Common.Search(m_App, _User, SessionID, AppID)


        i = Districts.Show(txtUserID.Text, Right("0000000000" & _customer.KUNNR, 10))

        m_Rights = Districts.Rights
        Dim count As Integer = m_Rights.Rows.Count
        m_Districts = Districts.Districts
    End Sub

    '-----------
    'Speichert die Berechtigungen nach SAP
    '-----------
    Private Sub SetDistrictRights(ByVal Rights As DataTable)
        Dim Districts As Base.Kernel.Common.Search
        Dim SessionID As String = Session.SessionID.ToString
        Dim AppID As String = "601"
        Dim _User As New User(CInt(txtUserID.Text), m_User.App.Connectionstring)
        If Session("AppID") Is Nothing Then
            Session.Add("AppID", AppID)
        End If
        Districts = New Base.Kernel.Common.Search(m_App, _User, SessionID, AppID)
        Districts.Rights = Rights
        Districts.Change()
    End Sub

#End Region

#Region "Daten laden"

    '-----------
    'Laden der Applikationen aus der Datenbank
    '-Nur Parent-Applikationen
    '-----------
    Private Function Get_Applications(ByVal GroupId As Integer) As ArrayList
        Dim Connection As New SqlClient.SqlConnection()
        Dim Command As New SqlClient.SqlCommand()
        Dim Applications As ArrayList = New ArrayList()

        Try
            Connection.ConnectionString = ConfigurationManager.AppSettings("Connectionstring")

            With Command
                .Connection = Connection
                .CommandType = CommandType.Text
                'Nur Parent-Applikationen lesen
                .CommandText = "SELECT DISTINCT Application.AppID As AppID, AppName, AppFriendlyName FROM Rights, Application WHERE Rights.GroupID = @GroupID AND Rights.AppID = Application.AppID AND Application.AppParent = 0 ORDER BY AppFriendlyName ASC"
                .Parameters.AddWithValue("@GroupID", GroupId)
            End With

            Connection.Open()
            Dim DataReader As SqlClient.SqlDataReader
            DataReader = Command.ExecuteReader()
            Dim Application As Appl
            While DataReader.Read
                Application = New Appl()
                Application.Id = CType(DataReader("AppID"), Integer)
                Application.Name = DataReader("AppName").ToString
                Application.FriendlyName = DataReader("AppFriendlyName").ToString
                Applications.Add(Application)
            End While
            Connection.Close()
        Catch ex As Exception
            If Connection.State = ConnectionState.Open Then
                Connection.Close()
            End If
            lblError.Text = "Beim Laden der Anwendungen ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
        End Try

        Return Applications

    End Function

    '-----------
    'Nachladen der Tabelle für die Rechte
    '-----------
    Private Sub RefillMatrix()
        Fill_Matrix(ddlFilterCustomer.SelectedItem.Value, "")
    End Sub

    '-----------
    'Erstellt die Tabelle für das Setzen der Rechte
    '-----------
    Private Sub Fill_Matrix(ByVal KUNNR As String, ByVal Mandt As String)
        Dim CurrentRowIndex As Integer = 0
        Dim CurrentColumnIndex As Integer = 0
        Dim GroupId As String
        Dim GroupName As String

        Matrix.Rows.Clear() ' zurücksetzen der Matrix bei Gruppenauswahl

        'GroupId = GroupList.SelectedItem.Value
        'GroupName = GroupList.SelectedItem.Text

        GroupId = ddlGroups.SelectedItem.Value
        GroupName = txtUserID.Text
        ReadDistrictsAndRights()

        Dim Applications As ArrayList = Get_Applications(GroupId)
        Dim Rows As Integer = Applications.Count + 3 ' Für 'Vorbelegung' + 'Alles auswählen' + Trennlinie, 'Überschrift' in Zeile 0
        Dim Columns As Integer = m_Districts.Rows.Count
        Dim ApplNameEnumerator As IEnumerator = Applications.GetEnumerator()
        Dim DistrEnumerator As IEnumerator = m_Districts.Select().GetEnumerator()
        Dim CurrentApplication As Appl

        For CurrentRowIndex = 0 To Rows
            Dim Row As New TableRow()

            'New Row (Application): Reset District Enumerator
            DistrEnumerator.Reset()

            For CurrentColumnIndex = 0 To Columns
                Dim Cell As New TableCell()
                Dim preventNewCell As Boolean = False
                If CurrentColumnIndex = 0 And CurrentRowIndex = 0 Then
                    'Do Nothing

                ElseIf CurrentColumnIndex = 0 And CurrentRowIndex = 1 Then
                    ''New Row (Application): Reset District Enumerator
                    'DistrEnumerator.Reset()
                    Dim lblVorbelegt As Label = New Label()
                    lblVorbelegt.ID = "lblVorbelegt"
                    lblVorbelegt.Text = "Vorbelegt"
                    Cell.Controls.Add(lblVorbelegt)

                ElseIf CurrentColumnIndex = 0 And CurrentRowIndex = 2 Then
                    ''New Row (Application): Reset District Enumerator
                    'DistrEnumerator.Reset()
                    Dim lblAuswaehlen As Label = New Label()
                    lblAuswaehlen.ID = "lblAuswaehlen"
                    lblAuswaehlen.Text = "Alle auswählen"
                    lblAuswaehlen.Font.Bold = True
                    Cell.Controls.Add(lblAuswaehlen)

                ElseIf CurrentRowIndex = 3 Then
                    DistrEnumerator.MoveNext()
                    If CurrentColumnIndex = 0 Then
                        '---
                        'Trennlinie
                        '---
                        Cell.ColumnSpan = 99
                        Cell.Controls.Add(New HtmlGenericControl("HR"))
                    Else
                        preventNewCell = True
                    End If

                ElseIf CurrentColumnIndex = 0 Then
                    'Display Application Friendly Name
                    ApplNameEnumerator.MoveNext()
                    CurrentApplication = CType(ApplNameEnumerator.Current, Appl)
                    Dim ApplName As Label = New Label()
                    ApplName.Text = CurrentApplication.FriendlyName
                    Cell.Controls.Add(ApplName)

                ElseIf CurrentRowIndex = 0 Then
                    'Display District Name
                    DistrEnumerator.MoveNext()
                    Dim lblDistrict As Label = New Label()
                    Dim Text As String = CType(DistrEnumerator.Current, DataRow).Item("DDTEXT")
                    lblDistrict.ID = "lblDistrict_" & CurrentColumnIndex.ToString()
                    lblDistrict.Text = Text
                    Cell.Controls.Add(lblDistrict)

                ElseIf CurrentRowIndex = 1 Then
                    '---
                    'Vorbelegung
                    '---
                    DistrEnumerator.MoveNext()
                    Dim rdoVorb As New RadioButton()
                    rdoVorb.ID = "rdoVorb_" & CurrentColumnIndex.ToString()
                    rdoVorb.GroupName = "rdoVorb"
                    Dim DistriktID As String = CType(DistrEnumerator.Current, DataRow).Item("DOMVALUE_L")
                    rdoVorb.Attributes.Add("DistriktID", DistriktID)
                    rdoVorb.AutoPostBack = True
                    AddHandler rdoVorb.CheckedChanged, AddressOf VorbelegungChanged
                    rdoVorb.Checked = IstDistriktVorbelegt(DistriktID, GroupName, KUNNR, Mandt)
                    Cell.Controls.Add(rdoVorb)

                ElseIf CurrentRowIndex = 2 Then
                    '----
                    'Alles auswählen
                    '----
                    DistrEnumerator.MoveNext()
                    Dim auswCheckBox As New CheckBox()
                    auswCheckBox.ID = "chkAllesAuswaehlen_" & CurrentColumnIndex.ToString() & "_" & CurrentRowIndex
                    Dim DistriktID As String = CType(DistrEnumerator.Current, DataRow).Item("DOMVALUE_L")
                    auswCheckBox.Attributes.Add("DistriktID", DistriktID)
                    auswCheckBox.AutoPostBack = True
                    auswCheckBox.Checked = IstAllesSelektiert(Applications.Count, DistriktID, GroupName, KUNNR, Mandt)
                    AddHandler auswCheckBox.CheckedChanged, AddressOf AllesAuswaehlenChanged
                    Cell.Controls.Add(auswCheckBox)

                Else
                    Dim RightCheckBox As New CheckBox()
                    'Display Right (true/false)
                    DistrEnumerator.MoveNext()
                    RightCheckBox = New CheckBox()
                    RightCheckBox.ID = "chkRight_" & CurrentColumnIndex.ToString() & "_" & CurrentRowIndex
                    Dim DistriktID As String = CType(DistrEnumerator.Current, DataRow).Item("DOMVALUE_L")
                    RightCheckBox.Checked = IstRechtVorhanden(m_Rights, CurrentApplication.Id, DistriktID, GroupName, KUNNR, Mandt)
                    RightCheckBox.Attributes.Add("DistriktID", DistriktID)
                    RightCheckBox.Attributes.Add("ApplicationID", CurrentApplication.Id)
                    RightCheckBox.Attributes.Add("GroupName", GroupName)
                    RightCheckBox.Attributes.Add("KundenNr", KUNNR)
                    'RightCheckBox.Attributes.Add("Mandt", Mandt)
                    RightCheckBox.AutoPostBack = True
                    'Readonly, wenn Vorbelegung gesetzt ist
                    RightCheckBox.Enabled = Not (RightCheckBox.Checked AndAlso IstDistriktVorbelegt(DistriktID, GroupName, KUNNR, Mandt))
                    AddHandler RightCheckBox.CheckedChanged, AddressOf RechtChanged
                    Cell.Controls.Add(RightCheckBox)

                End If

                'Zelle nur hinzufügen, wenn nicht es nicht verhindert werden soll, z.B. durch ColSpan
                If Not preventNewCell Then
                    Row.Cells.Add(Cell)
                End If


            Next
            Matrix.Rows.Add(Row)
        Next
    End Sub
    '-----------
    'Erstellt die Tabelle für das Setzen der Rechte
    '-----------
    Private Sub Fill_MatrixEmptyRights(ByVal KUNNR As String, ByVal Mandt As String)
        Dim CurrentRowIndex As Integer = 0
        Dim CurrentColumnIndex As Integer = 0
        Dim GroupId As String
        Dim GroupName As String

        Matrix.Rows.Clear() ' zurücksetzen der Matrix bei Gruppenauswahl

        'GroupId = GroupList.SelectedItem.Value
        'GroupName = GroupList.SelectedItem.Text

        GroupId = ddlGroups.SelectedItem.Value
        GroupName = txtUserID.Text
        ReadDistrictsAndRights()

        Dim Applications As ArrayList = Get_Applications(GroupId)
        Dim Rows As Integer = Applications.Count + 3 ' Für 'Vorbelegung' + 'Alles auswählen' + Trennlinie, 'Überschrift' in Zeile 0
        Dim Columns As Integer = m_Districts.Rows.Count
        Dim ApplNameEnumerator As IEnumerator = Applications.GetEnumerator()
        Dim DistrEnumerator As IEnumerator = m_Districts.Select().GetEnumerator()
        Dim CurrentApplication As Appl

        For CurrentRowIndex = 0 To Rows
            Dim Row As New TableRow()

            'New Row (Application): Reset District Enumerator
            DistrEnumerator.Reset()

            For CurrentColumnIndex = 0 To Columns
                Dim Cell As New TableCell()
                Dim preventNewCell As Boolean = False
                If CurrentColumnIndex = 0 And CurrentRowIndex = 0 Then
                    'Do Nothing

                ElseIf CurrentColumnIndex = 0 And CurrentRowIndex = 1 Then
                    ''New Row (Application): Reset District Enumerator
                    'DistrEnumerator.Reset()
                    Dim lblVorbelegt As Label = New Label()
                    lblVorbelegt.ID = "lblVorbelegt"
                    lblVorbelegt.Text = "Vorbelegt"
                    Cell.Controls.Add(lblVorbelegt)

                ElseIf CurrentColumnIndex = 0 And CurrentRowIndex = 2 Then
                    ''New Row (Application): Reset District Enumerator
                    'DistrEnumerator.Reset()
                    Dim lblAuswaehlen As Label = New Label()
                    lblAuswaehlen.ID = "lblAuswaehlen"
                    lblAuswaehlen.Text = "Alle auswählen"
                    lblAuswaehlen.Font.Bold = True
                    Cell.Controls.Add(lblAuswaehlen)

                ElseIf CurrentRowIndex = 3 Then
                    DistrEnumerator.MoveNext()
                    If CurrentColumnIndex = 0 Then
                        '---
                        'Trennlinie
                        '---
                        Cell.ColumnSpan = 99
                        Cell.Controls.Add(New HtmlGenericControl("HR"))
                    Else
                        preventNewCell = True
                    End If

                ElseIf CurrentColumnIndex = 0 Then
                    'Display Application Friendly Name
                    ApplNameEnumerator.MoveNext()
                    CurrentApplication = CType(ApplNameEnumerator.Current, Appl)
                    Dim ApplName As Label = New Label()
                    ApplName.Text = CurrentApplication.FriendlyName
                    Cell.Controls.Add(ApplName)

                ElseIf CurrentRowIndex = 0 Then
                    'Display District Name
                    DistrEnumerator.MoveNext()
                    Dim lblDistrict As Label = New Label()
                    Dim Text As String = CType(DistrEnumerator.Current, DataRow).Item("DDTEXT")
                    lblDistrict.ID = "lblDistrict_" & CurrentColumnIndex.ToString()
                    lblDistrict.Text = Text
                    Cell.Controls.Add(lblDistrict)
                ElseIf CurrentRowIndex = 1 Then
                    '---
                    'Vorbelegung
                    '---
                    DistrEnumerator.MoveNext()
                    Dim rdoVorb As New RadioButton()
                    rdoVorb.ID = "rdoVorb_" & CurrentColumnIndex.ToString()
                    rdoVorb.GroupName = "rdoVorb"
                    Dim DistriktID As String = CType(DistrEnumerator.Current, DataRow).Item("DOMVALUE_L")
                    rdoVorb.Attributes.Add("DistriktID", DistriktID)
                    rdoVorb.AutoPostBack = True
                    rdoVorb.Checked = False
                    Cell.Controls.Add(rdoVorb)

                ElseIf CurrentRowIndex = 2 Then
                    '----
                    'Alles auswählen
                    '----
                    DistrEnumerator.MoveNext()
                    Dim auswCheckBox As New CheckBox()
                    auswCheckBox.ID = "chkAllesAuswaehlen_" & CurrentColumnIndex.ToString() & "_" & CurrentRowIndex
                    Dim DistriktID As String = CType(DistrEnumerator.Current, DataRow).Item("DOMVALUE_L")
                    auswCheckBox.Attributes.Add("DistriktID", DistriktID)
                    auswCheckBox.AutoPostBack = True
                    auswCheckBox.Checked = False
                    'AddHandler auswCheckBox.CheckedChanged, AddressOf AllesAuswaehlenChanged
                    Cell.Controls.Add(auswCheckBox)

                Else
                    Dim RightCheckBox As New CheckBox()
                    'Display Right (true/false)
                    DistrEnumerator.MoveNext()
                    RightCheckBox = New CheckBox()
                    RightCheckBox.ID = "chkRight_" & CurrentColumnIndex.ToString() & "_" & CurrentRowIndex
                    Dim DistriktID As String = CType(DistrEnumerator.Current, DataRow).Item("DOMVALUE_L")
                    RightCheckBox.Checked = False
                    RightCheckBox.Attributes.Add("DistriktID", DistriktID)
                    RightCheckBox.Attributes.Add("ApplicationID", CurrentApplication.Id)
                    RightCheckBox.Attributes.Add("GroupName", GroupName)
                    RightCheckBox.Attributes.Add("KundenNr", KUNNR)
                    'RightCheckBox.Attributes.Add("Mandt", Mandt)
                    RightCheckBox.AutoPostBack = True
                    'Readonly, wenn Vorbelegung gesetzt ist
                    RightCheckBox.Enabled = Not (RightCheckBox.Checked AndAlso IstDistriktVorbelegt(DistriktID, GroupName, KUNNR, Mandt))
                    Cell.Controls.Add(RightCheckBox)
                End If

                'Zelle nur hinzufügen, wenn nicht es nicht verhindert werden soll, z.B. durch ColSpan
                If Not preventNewCell Then
                    Row.Cells.Add(Cell)
                End If


            Next
            Matrix.Rows.Add(Row)
        Next
    End Sub
    '--------
    'Füllt die Auswahl für Benutzergruppen
    '--------
    Private Sub Fill_UserList()
        Dim Connection As New SqlClient.SqlConnection()
        Dim Command As New SqlClient.SqlCommand()

        Try
            Connection.ConnectionString = ConfigurationManager.AppSettings("Connectionstring")
            If Not Session("UserID") = Nothing Then
                Dim iUserID As Integer
                iUserID = Session("UserID").ToString
                With Command
                    .Connection = Connection
                    .CommandType = CommandType.Text
                    '.CommandText = "SELECT GroupName,GroupID,CustomerID FROM WebGroup WHERE CustomerID = @Customer ORDER BY GroupName ASC"
                    '.Parameters.Add("@Customer", m_User.Customer.CustomerId)
                    .CommandText = "SELECT     dbo.WebUser.UserID, dbo.WebUser.Username, dbo.WebGroup.GroupName, dbo.WebGroup.CustomerID, dbo.WebMember.GroupID " & _
                    " FROM         dbo.WebUser INNER JOIN " & _
                    " dbo.WebMember ON dbo.WebUser.UserID = dbo.WebMember.UserID INNER JOIN " & _
                    " dbo.WebGroup ON dbo.WebMember.GroupID = dbo.WebGroup.GroupID " & _
                    " WHERE dbo.WebUser.UserID  = @UserID ORDER BY GroupName ASC"
                    .Parameters.AddWithValue("@UserID", iUserID)
                End With
                Connection.Open()
                Dim dReader As SqlClient.SqlDataReader = Command.ExecuteReader()
                While dReader.Read()
                    Session("GroupID") = CInt(dReader.GetValue(4))
                End While

                'GroupList.Visible = False
                'Connection.Open()
                'Dim DataReader As SqlClient.SqlDataReader
                'DataReader = Command.ExecuteReader()

                ''Drop Down Liste mit den Gruppen füllen
                'GroupList.DataSource = DataReader
                'GroupList.DataValueField = "GroupID"
                'GroupList.DataTextField = "GroupName"
                'GroupList.DataBind()
                'GroupList.SelectedIndex = 0

                Connection.Close()
            Else
                lblError.Text = "Es wurde kein Benutzer übergeben!"
            End If

        Catch ex As Exception
            If Connection.State = ConnectionState.Open Then
                Connection.Close()
            End If
            lblError.Text = "Beim Laden der Gruppen ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
        End Try

    End Sub

#End Region

#Region "Rights-DataTable-Zugriffe"

    '-----
    'Prüft, ob der Distrikt als vorbelegt markiert ist
    '-----
    Private Function IstDistriktVorbelegt(ByVal DistrictId As String, ByVal GroupName As String, ByVal KUNNR As String, ByVal Mandt As String) As Boolean
        Dim DataRow() As DataRow = m_Rights.Select(GetFilterExpression(KUNNR, DistrictId, GroupName, "1", Nothing, True, False))
        Return DataRow.Length > 0
    End Function

    '-----------
    'Prüft, ob alle Applikatonen Rechte zugeordnet sind 
    '-----------
    Private Function IstAllesSelektiert(ByVal countApps As Integer, ByVal DistrictId As String, ByVal GroupName As String, ByVal KUNNR As String, ByVal Mandt As String) As Boolean
        Dim Kunde As String = "0000000000" + KUNNR
        Kunde = Kunde.Substring(Kunde.Length - 10)

        Dim DataRow() As DataRow = m_Rights.Select(GetFilterExpression(Kunde, DistrictId, GroupName, Nothing, Nothing, True, False))
        Return DataRow.Length = countApps
    End Function

    '---------
    'Gibt an, ob für eine Applikation und ein Distrikt Rechte vorhanden sind
    '---------
    Private Function IstRechtVorhanden(ByVal Rights As DataTable, ByVal ApplicationId As String, ByVal DistrictId As String, ByVal GroupName As String, ByVal KUNNR As String, ByVal Mandt As String) As Boolean
        Dim DataRow() As DataRow = Rights.Select(GetFilterExpression(KUNNR, DistrictId, GroupName, Nothing, ApplicationId, True, False))
        Session("Changed") = 1
        Return DataRow.Length > 0
    End Function

    '---------
    'Fügt ein neues Recht hinzu
    '---------
    Private Sub AddRight(ByVal Rights As DataTable, ByVal ApplicationId As String, ByVal DistrictId As String, ByVal GroupName As String, ByVal KUNNR As String, ByVal Mandt As String, ByVal Vorb As String)

        'Sonst neuen Datensatz anlegen
        Dim NewRow As DataRow = m_Rights.NewRow()
        NewRow("Distrikt") = DistrictId
        NewRow("Anwendung") = ApplicationId
        NewRow("BenGrp") = GroupName
        NewRow("KunNr") = Right("0000000000" + KUNNR, 10)
        'NewRow("Mandt") = Mandt
        NewRow("Vorbelegt") = Vorb
        NewRow("Loekz") = ""
        Rights.Rows.Add(NewRow)
        Session("Changed") = 1
    End Sub

    '---------
    'Entfernt ein Recht, d.h. setzt das Löschkennzeichen
    '---------
    Private Sub RemoveRight(ByVal Rights As DataTable, ByVal ApplicationId As String, ByVal DistrictId As String, ByVal GroupName As String, ByVal KUNNR As String, ByVal Mandt As String)
        Dim row As DataRow
        For Each row In Rights.Select(GetFilterExpression(KUNNR, DistrictId, GroupName, Nothing, ApplicationId, False, False))
            row("Loekz") = CONST_LOESCHKENNZEICHEN
            'Rights.Rows.Remove(row)
        Next
    End Sub

    '---------
    'Setzt den Vorbelegungswert für ein Distrikt
    'Der Wert wird in alle Rechte des Distriktes denormalisiert!!!
    '---------
    Private Sub SetzeVorbelegswertFuerDistrikt(ByVal DistrictId As String, ByVal GroupName As String, ByVal KUNNR As String, ByVal istVorbelegt As Boolean, ByVal invertDistrikt As Boolean)

        Dim row As DataRow
        For Each row In m_Rights.Select(GetFilterExpression(KUNNR, DistrictId, GroupName, Nothing, Nothing, False, invertDistrikt))
            If istVorbelegt Then
                row("Vorbelegt") = "1"
            Else
                row("Vorbelegt") = "0"
            End If
        Next

    End Sub

#End Region

#Region "Oberflächen-Zugriff"

    '-----------
    'Setzt oder entfernt den Haken bei allen enableden Checkboxen
    'Optional wird auch anschließend disabled
    '-----------
    Private Sub SetRightsCheckBoxes(ByVal ctrls As IEnumerator, ByVal DistriktID As String, ByVal Checked As Boolean, ByVal setReadonly As Boolean)
        Dim cbox As CheckBox
        For Each cbox In GetRightsCheckboxes(ctrls, DistriktID, False)
            If cbox.Enabled Then
                cbox.Checked = Checked
                cbox.Enabled = Not setReadonly
            End If
        Next
        Session("Changed") = 1
    End Sub

    '-----------
    'Enabaled/Disabled alle Checkboxes für einen Distrikt bzw. wenn invertDistrikt, dann für alle ungleich distrikt
    '-----------
    Private Sub SetProtectionForCheckboxes(ByVal ctrls As IEnumerator, ByVal DistriktID As String, ByVal invertDistrikt As Boolean, ByVal isReadonly As Boolean)
        Dim cbox As CheckBox
        For Each cbox In GetRightsCheckboxes(ctrls, DistriktID, invertDistrikt)
            cbox.Enabled = Not isReadonly
        Next
        Session("Changed") = 1
    End Sub

    '---------
    'Ermittelt die Recht aus den Checkboxen
    '---------
    Private Sub ErmitteleRechteAusCheckBoxen(ByVal Controls As IEnumerator, ByVal rights As DataTable)
        Dim cbox As CheckBox
        For Each cbox In GetRightsCheckboxes(Controls, Nothing, False)
            If cbox.Checked Then
                If Not IstRechtVorhanden(rights, cbox.Attributes("ApplicationID"), cbox.Attributes("DistriktID"), cbox.Attributes("GroupName"), cbox.Attributes("KundenNr"), cbox.Attributes("Mandt")) Then
                    AddRight(rights, cbox.Attributes("ApplicationID"), cbox.Attributes("DistriktID"), cbox.Attributes("GroupName"), cbox.Attributes("KundenNr"), cbox.Attributes("Mandt"), cbox.Attributes("Vorb"))
                End If
            Else
                If IstRechtVorhanden(rights, cbox.Attributes("ApplicationID"), cbox.Attributes("DistriktID"), cbox.Attributes("GroupName"), cbox.Attributes("KundenNr"), cbox.Attributes("Mandt")) Then
                    RemoveRight(rights, cbox.Attributes("ApplicationID"), cbox.Attributes("DistriktID"), cbox.Attributes("GroupName"), cbox.Attributes("KundenNr"), cbox.Attributes("Mandt"))
                End If
            End If
        Next
    End Sub

#End Region

#Region "Load"

    Private Sub Page_LoadDistikte()

        Try
            Dim _User As New User(CInt(txtUserID.Text), m_User.App.Connectionstring)
            Fill_Matrix(_User.KUNNR, "")
            Matrix.Visible = True
        Catch ex As Exception
            lblError.Text = "Beim Laden der Seite ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
        End Try
    End Sub

#End Region

#Region "Selektion"

    '-------
    'Es wurde eine andere Benutzergruppe ausgewählt
    '-------
    Private Sub GroupList_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Try
            Fill_Matrix(m_User.KUNNR, "")
        Catch ex As Exception
            lblError.Text = "Fehler: " & ex.Message
        End Try
    End Sub

#End Region

#Region "Grid"

    '--------
    'Es wurde ein anderer Distrikt als Vorbelegung ausgewählt
    '--------
    Public Sub VorbelegungChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim rdoButton As RadioButton = CType(sender, RadioButton)
        Dim Distrikt As String = rdoButton.Attributes("DistriktID")

        'Alle Applications für Distrikt auswählen auswählen inkl Schreibschutz
        SetRightsCheckBoxes(Matrix.Controls.GetEnumerator(), Distrikt, rdoButton.Checked, True)
        SetProtectionForCheckboxes(Matrix.Controls.GetEnumerator(), Distrikt, True, False)
        If rdoButton.Checked = True Then
            chk_Matrix1.Checked = True
            chk_Matrix1.Enabled = True
        End If
        Session("Changed") = 1
    End Sub

    '--------
    'Wird aufgerufen, wenn die "Alle auswählen"-Checkbox geändert wird
    'Setzt alle Application-Checkboxes auf den gleichen Wert
    '--------
    Public Sub AllesAuswaehlenChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim CheckBox As CheckBox = CType(sender, CheckBox)
        Dim Distrikt As String = CheckBox.Attributes("DistriktID")
        'Ggf Schreibschutz beibehalten
        SetRightsCheckBoxes(Matrix.Controls.GetEnumerator(), Distrikt, CheckBox.Checked, False)
        If CheckBox.Checked = True Then
            chk_Matrix1.Checked = True
            chk_Matrix1.Enabled = True
        End If
        Session("Changed") = 1
    End Sub
    '--------
    'Wird aufgerufen, wenn die Checkbox(Recht) geändert wird
    '--------
    Public Sub RechtChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim CheckBox As CheckBox = CType(sender, CheckBox)
        If CheckBox.Checked = True Then
            chk_Matrix1.Checked = True
            chk_Matrix1.Enabled = True
        End If
        Session("Changed") = 1
    End Sub
#End Region

    Private Sub InsertIntoWebUserUpload(ByVal UserID As Integer, ByRef PWord As String, ByVal Username As String, ByVal LinkKey As String, ByRef RightKey As String, ByRef WrongKey As String, ByVal LoginLinkID As Integer)
        Dim cn As New SqlClient.SqlConnection(m_User.App.Connectionstring)
        cn.Open()

        Dim cmdInsert As New SqlClient.SqlCommand("INSERT INTO WebUserUpload(UserID,Password,RightUserLink,WrongUserLink) Values(@UserID,@Password,@RightUserLink,@WrongUserLink)", cn)
        Dim RightUser As String
        Dim WrongUser As String

        Dim Crypto As New Crypt


        RightUser = System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(Username & LinkKey & "Right", "sha1")
        WrongUser = System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(Username & LinkKey & "Wrong", "sha1")

        PWord = Crypto.psEncrypt(PWord)

        With cmdInsert.Parameters
            .AddWithValue("@UserID", UserID)
            .AddWithValue("@Password", PWord)
            .AddWithValue("@RightUserLink", RightUser)
            .AddWithValue("@WrongUserLink", WrongUser)
            .AddWithValue("@LoginLinkID", LoginLinkID)
        End With
        cmdInsert.ExecuteNonQuery()

        cn.Close()
        cn.Dispose()

        RightKey = RightUser
        WrongKey = WrongUser
    End Sub

   
End Class

' ************************************************
' $History: UserManagement.aspx.vb $
' 
' *****************  Version 34  *****************
' User: Fassbenders  Date: 23.03.11   Time: 13:56
' Updated in $/CKAG/admin
' 
' *****************  Version 33  *****************
' User: Rudolpho     Date: 8.09.10    Time: 17:17
' Updated in $/CKAG/admin
' 
' *****************  Version 32  *****************
' User: Rudolpho     Date: 8.09.10    Time: 12:30
' Updated in $/CKAG/admin
' 
' *****************  Version 31  *****************
' User: Rudolpho     Date: 22.07.10   Time: 13:45
' Updated in $/CKAG/admin
' 
' *****************  Version 30  *****************
' User: Rudolpho     Date: 28.06.10   Time: 9:50
' Updated in $/CKAG/admin
' 
' *****************  Version 29  *****************
' User: Rudolpho     Date: 21.06.10   Time: 9:03
' Updated in $/CKAG/admin
' 
' *****************  Version 28  *****************
' User: Rudolpho     Date: 16.06.10   Time: 15:33
' Updated in $/CKAG/admin
' 
' *****************  Version 27  *****************
' User: Rudolpho     Date: 16.06.10   Time: 8:57
' Updated in $/CKAG/admin
' 
' *****************  Version 26  *****************
' User: Fassbenders  Date: 23.03.10   Time: 14:53
' Updated in $/CKAG/admin
' 
' *****************  Version 25  *****************
' User: Fassbenders  Date: 16.03.10   Time: 11:27
' Updated in $/CKAG/admin
' 
' *****************  Version 24  *****************
' User: Fassbenders  Date: 11.03.10   Time: 12:43
' Updated in $/CKAG/admin
' ITA: 2918
' 
' *****************  Version 23  *****************
' User: Rudolpho     Date: 28.04.09   Time: 16:42
' Updated in $/CKAG/admin
' 
' *****************  Version 22  *****************
' User: Rudolpho     Date: 28.04.09   Time: 13:45
' Updated in $/CKAG/admin
' 
' *****************  Version 21  *****************
' User: Fassbenders  Date: 17.04.09   Time: 16:59
' Updated in $/CKAG/admin
' 
' *****************  Version 20  *****************
' User: Jungj        Date: 31.03.09   Time: 15:40
' Updated in $/CKAG/admin
' ITa 2156 nachbesserungen
' 
' *****************  Version 19  *****************
' User: Jungj        Date: 19.03.09   Time: 16:25
' Updated in $/CKAG/admin
' ITA 2156 testfertig
' 
' *****************  Version 18  *****************
' User: Fassbenders  Date: 24.02.09   Time: 11:31
' Updated in $/CKAG/admin
' 
' *****************  Version 17  *****************
' User: Rudolpho     Date: 11.02.09   Time: 15:12
' Updated in $/CKAG/admin
' 
' *****************  Version 16  *****************
' User: Rudolpho     Date: 6.01.09    Time: 11:45
' Updated in $/CKAG/admin
' ITA 2503  Cache durch Session ersetzt
' 
' *****************  Version 15  *****************
' User: Rudolpho     Date: 25.11.08   Time: 11:45
' Updated in $/CKAG/admin
' 
' *****************  Version 14  *****************
' User: Rudolpho     Date: 7.11.08    Time: 16:10
' Updated in $/CKAG/admin
' ITA: 2369
' 
' *****************  Version 13  *****************
' User: Jungj        Date: 3.11.08    Time: 17:26
' Updated in $/CKAG/admin
' 2358 testfertig
' 
' *****************  Version 12  *****************
' User: Jungj        Date: 13.10.08   Time: 13:28
' Updated in $/CKAG/admin
' ITA 2315 testfertig
' 
' *****************  Version 11  *****************
' User: Jungj        Date: 7.10.08    Time: 12:49
' Updated in $/CKAG/admin
' ÎTA 2277 Nachbesserungen
' 
' *****************  Version 10  *****************
' User: Jungj        Date: 7.10.08    Time: 9:19
' Updated in $/CKAG/admin
' ITA 2277
' 
' *****************  Version 9  *****************
' User: Jungj        Date: 6.10.08    Time: 9:19
' Updated in $/CKAG/admin
' ITA 2295 fertig
' 
' *****************  Version 8  *****************
' User: Rudolpho     Date: 18.09.08   Time: 15:20
' Updated in $/CKAG/admin
' 
' *****************  Version 7  *****************
' User: Rudolpho     Date: 15.09.08   Time: 14:36
' Updated in $/CKAG/admin
' Gruppenauswahl: Customerchange -> Gruppe vorbelegt mit - keine- !
' 
' *****************  Version 6  *****************
' User: Hartmannu    Date: 11.09.08   Time: 13:41
' Updated in $/CKAG/admin
' 
' *****************  Version 5  *****************
' User: Hartmannu    Date: 11.09.08   Time: 11:34
' Updated in $/CKAG/admin
' Fixing Admin-Änderungen
' 
' *****************  Version 4  *****************
' User: Hartmannu    Date: 10.09.08   Time: 17:28
' Updated in $/CKAG/admin
' ITA 2027 - Anzeige der erweiterten Benutzerhistorie
' 
' *****************  Version 3  *****************
' User: Hartmannu    Date: 9.09.08    Time: 13:42
' Updated in $/CKAG/admin
' ITA 2152 und 2158
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
' *****************  Version 36  *****************
' User: Fassbenders  Date: 14.12.07   Time: 14:01
' Updated in $/CKG/Admin/AdminWeb
' 
' *****************  Version 35  *****************
' User: Fassbenders  Date: 27.11.07   Time: 17:36
' Updated in $/CKG/Admin/AdminWeb
' 
' *****************  Version 34  *****************
' User: Uha          Date: 30.08.07   Time: 12:36
' Updated in $/CKG/Admin/AdminWeb
' ITA 1280: Paßwortversand im Web auf Benutzerwunsch
' 
' *****************  Version 33  *****************
' User: Uha          Date: 27.08.07   Time: 17:13
' Updated in $/CKG/Admin/AdminWeb
' ITA 1269: Archivverwaltung/-zuordnung jetzt auch per Web
' 
' *****************  Version 32  *****************
' User: Fassbenders  Date: 31.07.07   Time: 17:13
' Updated in $/CKG/Admin/AdminWeb
' 
' *****************  Version 31  *****************
' User: Rudolpho     Date: 26.07.07   Time: 15:00
' Updated in $/CKG/Admin/AdminWeb
' 
' *****************  Version 30  *****************
' User: Rudolpho     Date: 26.07.07   Time: 9:55
' Updated in $/CKG/Admin/AdminWeb
' Fehlerbehandlung eingefügt(Fehlerlokalisierung ohne schwer!)
' 
' *****************  Version 29  *****************
' User: Uha          Date: 22.05.07   Time: 14:23
' Updated in $/CKG/Admin/AdminWeb
' Nacharbeiten + Bereinigungen
' 
' *****************  Version 28  *****************
' User: Uha          Date: 15.05.07   Time: 15:29
' Updated in $/CKG/Admin/AdminWeb
' Änderungen aus StartApplication vom 11.05.2007
' 
' *****************  Version 27  *****************
' User: Uha          Date: 13.03.07   Time: 10:53
' Updated in $/CKG/Admin/AdminWeb
' History-Eintrag vorbereitet
' 
' ************************************************
