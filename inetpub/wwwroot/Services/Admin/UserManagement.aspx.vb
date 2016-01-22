Imports CKG.Base.Kernel.Admin
Imports CKG.Base.Kernel.Common.Common
Imports CKG.Base.Kernel.Security
Imports Telerik.Web.UI
Imports Telerik.Web.UI.GridExcelBuilder
Imports System.Data.SqlClient
Imports System.Net.Mime
Imports GeneralTools.Models

Public Structure Appl
    Dim Name As String
    Dim FriendlyName As String
    Dim Id As Integer
End Structure

Partial Public Class UserManagement
    Inherits Page

    Private Const CONST_LOESCHKENNZEICHEN As String = "X"
    Private isExcelExportConfigured As Boolean = False

#Region " Membervariables "
    Private m_User As User
    Private m_App As App
    Private m_Rights As DataTable
    Private m_Districts As DataTable
#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load

        ' Hier Benutzercode zur Seiteninitialisierung einfügen
        m_User = GetUser(Me)

        lblHead.Text = "Benutzerverwaltung"
        AdminAuth(Me, m_User, AdminLevel.Organization)

        Try
            m_App = New App(m_User)

            lblError.Text = ""
            lblMessage.Text = ""

            If Not IsPostBack Then
                FillHierarchy()
                FillForm()
            ElseIf lbtnDistrict.Visible = True AndAlso Matrix.Visible = True Then
                Dim _User As New User(CInt(txtUserID.Text), m_User.App.Connectionstring)
                Fill_Matrix(_User.Customer.KUNNR, "")
            End If

            'erst jetzt sollen beim "NeedDataSource"-Event des Grids Daten geladen werden.
            'ohne dieses Flag würden bereits beim ersten Page_Load ungefiltert alle Datensätze geladen werden.
            ihIsInitialDataLoad.Value = "0"
        Catch ex As Exception
            m_App.WriteErrorText(1, m_User.UserName, "UserManagement", "Page_Load", ex.ToString)

            lblError.Text = ex.ToString
        End Try

    End Sub

#Region " Data and Function "
    Protected Function GetSelfAdministrationImageURL(ByVal intSelfAdmLvl As Integer) As String
        Dim strErg As String = "../Images/blank.gif"

        Try
            Select Case intSelfAdmLvl
                Case 1
                    strErg = "../Images/SelfAdmIcon1.gif"
                Case 2
                    strErg = "../Images/SelfAdmIcon2.gif"
            End Select
        Catch ex As Exception

        End Try

        Return strErg
    End Function

    Private Sub FillHierarchy()
        Dim cn As New SqlConnection(m_User.App.Connectionstring)
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

        Dim cn As New SqlConnection(m_User.App.Connectionstring)
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
            tdHierarchyDisplay1.Visible = False
            tdHierarchyDisplay2.Visible = False
            td_OnlyEmployees1.Visible = False
            td_OnlyEmployees2.Visible = False
            td_OnlyLoggedinUser1.Visible = False
            td_OnlyLoggedinUser2.Visible = False

            td_LastLoginBefore1.Visible = False
            td_LastLoginBefore2.Visible = False

            td_OnlyDisabledUser1.Visible = False
            td_OnlyDisabledUser2.Visible = False

            If m_User.HighestAdminLevel > AdminLevel.Customer Then

                'Wenn DAD-SuperUser:
                lblCustomer.Visible = False 'Label mit Customer-Namen ausblenden
                ddlFilterCustomer.Visible = True 'DropDown zur Customer-Auswahl einblenden
                divLegende.Visible = True 'SelfAdministration-Legende anzeigen
                'wenn SuperUser und übergeordnete Firma
                If m_User.Customer.AccountingArea = -1 Then
                    lnkAppManagement.Visible = True
                End If

                lnkContact.Visible = True

                td_LastLoginBefore1.Visible = True
                td_LastLoginBefore2.Visible = True
                td_OnlyLoggedinUser1.Visible = True
                td_OnlyLoggedinUser2.Visible = True
                td_OnlyDisabledUser1.Visible = True
                td_OnlyDisabledUser2.Visible = True

            Else
                'Wenn nicht DAD-Super-User:
                lnkContact.Visible = False
                lnkArchivManagement.Visible = False
                lnkJahresArchivManagement.Visible = False
                lnkCustomerManagement.Visible = False 'Link fuer die Kundenverwaltung ausblenden
                lblCustomer.Text = m_User.Customer.CustomerName 'Customer des angemeldeten Benutzers
                trCustomer.Visible = False 'Customer-Auswahl im Edit-bereich ausblenden
                rgSearchResult.Columns(14).Visible = False 'Spalte "Test-Zugang" ausblenden
                trTestUser.Visible = False '"Test-Zugang" aus dem Edit-Bereich ausblenden
                rgSearchResult.Columns(17).Visible = False 'Spalte "Passwort läuft nie ab" ausblenden
                rgSearchResult.Columns(13).Visible = False 'Spalte "Customer-Admin" ausblenden
                rgSearchResult.Columns(20).Visible = False 'Spalte "RemoteLoginKey" ausblenden
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
                td_OnlyLoggedinUser1.Visible = True
                td_OnlyLoggedinUser2.Visible = True
                td_OnlyDisabledUser1.Visible = True
                td_OnlyDisabledUser2.Visible = True
                '--------------------------------



                If Not m_User.Customer.ShowOrganization Then
                    lnkOrganizationManagement.Visible = False
                    trSelectOrganization.Visible = False
                    rgSearchResult.Columns(9).Visible = False 'Spalte "Organisation" ausblenden
                    trOrganization.Visible = False
                    trOrganizationAdministrator.Visible = False
                End If

                If Not m_User.IsCustomerAdmin Then
                    'Wenn nicht Customer-Admin:
                    lnkOrganizationManagement.Visible = False
                    lnkGroupManagement.Visible = False
                    rgSearchResult.Columns(9).Visible = False 'Spalte "Organisation" ausblenden
                    trGroup.Visible = False 'Gruppenauswahl im Edit-Bereich ausblenden
                    trOrganization.Visible = False 'Organisationsauswahl im Edit-Bereich ausblenden
                    trOrganizationAdministrator.Visible = False 'OrganisationAdmin-Auswahl im Edit-Bereich ausblenden
                    If m_User.Groups.Count > 0 Then lblGroup.Text = m_User.Groups(0).GroupName 'Gruppe des angemeldeten Benutzers
                    lblOrganization.Text = m_User.Organization.OrganizationName 'Organisation des angemeldeten Benutzers
                    lblOrganization.Visible = True 'Label mit Organisationsnamen einblenden
                    ddlFilterOrganization.Visible = False 'DropDown zur Organisationsauswahl ausblenden
                End If
            End If

        Catch ex As Exception
            Throw
        Finally
            If cn.State <> ConnectionState.Closed Then
                cn.Close()
            End If
        End Try
    End Sub

    Private Sub FillGroups(ByVal intCustomerID As Integer, ByVal cn As SqlConnection)
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

    Private Sub FillOrganizations(ByVal intCustomerID As Integer, ByVal cn As SqlConnection)
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

    Private Sub FillCustomer(ByVal cn As SqlConnection)
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

    Private Sub FillDataGrid(ByVal blnNotApproved As Boolean, Optional ByVal rebind As Boolean = True, Optional ByVal blnWithoutLoadingData As Boolean = False)
        Dim strSort As String = "UserID"
        If Not ViewState("ResultSort") Is Nothing Then
            strSort = ViewState("ResultSort").ToString
        End If
        FillDataGrid(blnNotApproved, strSort, rebind, blnWithoutLoadingData)
    End Sub

    Private Sub FillDataGrid(ByVal blnNotApproved As Boolean, ByVal strSort As String, Optional ByVal rebind As Boolean = True, Optional ByVal blnWithoutLoadingData As Boolean = False)

        Dim dvUser As DataView

        Dim cn As New SqlConnection(m_User.App.Connectionstring)
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

        If Not blnWithoutLoadingData Then
            If Not blnNotApproved Then
                dtUser = New Kernel.UserList(txtFilterUserName.Text, _
                                      txtFilterFirstName.Text, _
                                      txtFilterLastName.Text, _
                                      txtFilterMail.Text, _
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
                                      txtFilterFirstName.Text, _
                                      txtFilterLastName.Text, _
                                      txtFilterMail.Text, _
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
        End If
        dvUser = dtUser.DefaultView
        Session("myUserListView") = dvUser
        dvUser.Sort = strSort

        'prüfung das der gesetzte pageindex größer als die anzahl der ergebnisse ist,
        'ist hier nötig weil bei jedem fillGrid die neuen Selektionsparameter verwendet werden
        'JJU 20081013
        '---------------------------------------------------------
        'War nicht ganz richtig, wenn man 23 User selektiert sind und auf Seite 3 will,
        'kommt man nie auf die letzte Seite. 23 nicht größer 30!!! Sind aber noch 3 User auf Seite 3!!

        If Not dvUser.Count > ((rgSearchResult.CurrentPageIndex + 1) * rgSearchResult.PageSize) - 9 Then
            rgSearchResult.CurrentPageIndex = 0
        End If

        '---------------------------------------------------------

        With rgSearchResult

            .DataSource = dvUser

            'Für Masteradmins Spalte mit Hinweis anzeigen, dass Kunde die User selbst administriert
            If m_User.HighestAdminLevel = AdminLevel.Master Then
                .MasterTableView.GetColumn("SelfAdministration").Visible = True
            End If

            If (rebind) Then .Rebind()

        End With

        If Not dvUser Is Nothing And dvUser.Table.Rows.Count > 0 Then

            Dim fitTable As DataTable = New DataTable()
            Dim startIndex As Integer = 0

            For Each col As DataColumn In dvUser.Table.Columns
                fitTable.Columns.Add(col.ColumnName)
            Next

            'Aktuelle Startindex anhand der Seitenzahl und der Seitengröße ermitteln
            If rgSearchResult.CurrentPageIndex > 0 Then
                startIndex = (rgSearchResult.CurrentPageIndex - 1) * rgSearchResult.PageSize
            End If

            For index = 0 To rgSearchResult.PageSize - 1
                If dvUser.Table.Rows.Count <= index Then
                    Exit For
                End If

                fitTable.Rows.Add(dvUser.Table.Rows(startIndex + index).ItemArray)
            Next

            CKG.Base.Kernel.Common.Common.ResizeTelerikColumns(rgSearchResult, fitTable)
            fitTable = Nothing

        End If

        If blnNotApproved = True Then

            Dim Item As GridDataItem

            Dim lnkButton As LinkButton
            Dim strUserText As String

            For Each Item In rgSearchResult.Items

                If Item.Cells(22).Text = m_User.UserName Then

                    lnkButton = New LinkButton()

                    lnkButton = Item.Cells(1).Controls(0)
                    strUserText = lnkButton.Text
                    Item.Cells(1).Controls.Clear()
                    Item.Cells(1).Text = strUserText
                    Item.Cells(1).ForeColor = System.Drawing.Color.Gray
                    lnkButton.Dispose()

                    Item.Cells(10).Controls.Clear()

                End If
            Next

        End If

    End Sub

    Private Function FillEdit(ByVal intUserId As Integer, Optional ByVal approveMode As Boolean = False) As Boolean
        Dim _li As ListItem
        hlUserHistory.Visible = False
        Try
            SearchMode(False, , intUserId)
            Dim _User As New User(intUserId, m_User.App.Connectionstring)
            txtUserID.Text = _User.UserID.ToString
            txtUserName.Text = _User.UserName
            trMasterUser.Visible = _User.UserID > 0 AndAlso m_User.HighestAdminLevel = AdminLevel.Master
            Session("UsernameStart") = _User.UserName
            Session("LockedOutStart") = _User.AccountIsLockedOut
            txtReference.Text = _User.Reference
            txtReference2.Text = _User.Reference2
            txtReference3.Text = _User.Reference3
            cbxReference4.Checked = _User.Reference4
            txtMail.Text = _User.Email
            txtPhone.Text = _User.Telephone
            txtValidFrom.Text = _User.ValidFrom
            txtValidTo.Text = _User.ValidTo
            txtReadMessageCount.Text = _User.ReadMessageCount.ToString
            cbxTestUser.Checked = _User.IsTestUser
            chkLoggedOn.Checked = _User.LoggedOn
            chk_Matrix1.Checked = _User.Matrixfilled
            If Not chk_Matrix1.Checked Then chk_Matrix1.Enabled = False
            cbxCustomerAdmin.Checked = _User.IsCustomerAdmin
            cbxFirstLevelAdmin.Checked = _User.FirstLevelAdmin
            lblUrlRemoteLoginKey.Text = _User.UrlRemoteLoginKey

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
                Dim intCustomerID As Integer = _User.Customer.CustomerId
                Dim cn As New SqlConnection(m_User.App.Connectionstring)
                cn.Open()
                Dim dtGroups As New Kernel.GroupList(intCustomerID, cn, m_User.Customer.AccountingArea)
                FillGroup(ddlGroups, False, dtGroups)
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
                Dim intCustomerID As Integer = _User.Customer.CustomerId
                Dim cn As New SqlConnection(m_User.App.Connectionstring)
                cn.Open()
                Dim dtOrganizations As New OrganizationList(intCustomerID, cn, m_User.Customer.AccountingArea)
                FillOrganization(ddlOrganizations, False, dtOrganizations)
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

            initialize_ReferenceFields()

            lblLastPwdChange.Text = String.Format("{0:dd.MM.yy}", _User.LastPasswordChange)
            cbxPwdNeverExpires.Checked = _User.PasswordNeverExpires
            lblFailedLogins.Text = _User.FailedLogins.ToString
            cbxAccountIsLockedOut.Checked = _User.AccountIsLockedOut
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

            If _User.IsSuperiorTo(m_User) Then
                lblMessage.Text = "Sie können kein übergeordnetes Benutzerkonto bearbeiten!"
                Return False
            End If

            'Für Benutzerkonten von Kunden mit SelfAdmin-Level 2 keine Bearbeitung durch fremde Masteradmins zulassen
            If _User.Customer.Selfadministration > 1 AndAlso m_User.HighestAdminLevel = AdminLevel.Master AndAlso m_User.Customer.CustomerId <> _User.Customer.CustomerId Then
                lblMessage.Text = "Dieses Benutzerkonto kann nicht bearbeitet werden, da der Kunde exklusive Änderungsrechte besitzt!"
                Return False
            End If

            btnRemove.Enabled = False
            Image1.Visible = False
            If _User.Picture Then
                btnRemove.Enabled = True

                Dim info As IO.FileInfo
                info = New IO.FileInfo(ConfigurationManager.AppSettings("UploadPathLocal") & "responsible\" & txtUserID.Text & ".jpg")
                If (info.Exists) Then
                    Image1.Visible = True
                    Image1.ImageUrl = Replace(ConfigurationManager.AppSettings("UploadPath"), "\", "/") & "responsible/" & txtUserID.Text & ".jpg"
                End If
            End If

            hlUserHistory.NavigateUrl = "SingleUserHistory.aspx?UserID=" & _User.UserID.ToString
            hlUserHistory.Visible = True

            trUrlRemoteLoginKey.Visible = False
            If (_User.Customer.AllowUrlRemoteLogin And (m_User.HighestAdminLevel > AdminLevel.Customer Or m_User.IsCustomerAdmin)) Then
                ' Wenn Kunde für Remote URL freigeschaltet  UND  User ist Highest Admin oder Customer Admin
                ' ==> dann ist die URL Remote Login Key Gemerierung hier verfügbar:
                trUrlRemoteLoginKey.Visible = True
            End If

            If approveMode AndAlso _User.CreatedBy = m_User.UserName Then
                lblMessage.Text = "Sie können kein Benutzerkonto freigeben, das Sie selbst angelegt haben!"
                Return False
            End If

            ShowRightsPerUser(_User.UserName)

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
        txtReference2.Text = ""
        txtReference3.Text = ""
        cbxReference4.Checked = False
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
        txtValidTo.Text = ""
        lblLockedBy.Text = ""
        lblLockedBy.Visible = False


        '----------------------------------------

        'checkboxen
        '----------------------------------------
        chkEmployee.Checked = False
        'Default: Produktivuser, da Checkbox für Firmenadmins nicht sichtbar!!
        cbxTestUser.Checked = m_User.IsTestUser
        cbxAccountIsLockedOut.Checked = False
        cbxApproved.Checked = Not ((m_User.HighestAdminLevel = AdminLevel.Master) Or (m_User.HighestAdminLevel = AdminLevel.FirstLevel))
        chkLoggedOn.Checked = False
        chkNewPasswort.Checked = False
        'eigentlich radiobuttons, aber gut 
        '----------------------
        cbxCustomerAdmin.Checked = False
        cbxFirstLevelAdmin.Checked = False
        lblUrlRemoteLoginKey.Text = ""
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
        Dim backColor As Drawing.Color = Drawing.Color.FromName(strBackColor)

        txtReadMessageCount.Enabled = enabled
        txtReadMessageCount.BackColor = backColor
        txtUserName.Enabled = enabled
        txtUserName.BackColor = backColor

        txtReference.Enabled = enabled
        txtReference.BackColor = backColor
        txtReference2.Enabled = enabled
        txtReference2.BackColor = backColor
        txtReference3.Enabled = enabled
        txtReference3.BackColor = backColor
        cbxReference4.Enabled = enabled
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
        lbtnCancel.Text = "Verwerfen&nbsp;&#187;"
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
    End Sub

    Private Sub EditDeleteMode(ByVal intUserID As Integer)
        If Not FillEdit(intUserID) Then
            lbtnDelete.Enabled = False
        Else
            lblMessage.Text = "Möchten Sie den Benutzer wirklich löschen?"
            lbtnDelete.Enabled = True
        End If
        LockEdit(True)
        lbtnCancel.Text = "Abbrechen&nbsp;&#187;"
        lbtnSave.Visible = False
        Input.Visible = True
        lbtnDistrict.Visible = False
        trMatrix.Visible = False
        lbtnDelete.Visible = True
    End Sub

    Private Sub SearchMode(Optional ByVal blnSearchMode As Boolean = True, Optional ByVal blnNewSearch As Boolean = False, Optional ByVal userId As Integer = -1)
        'gewünschte Expand/Collapse-Stati für die Seitenabschnitte in hidden fields setzen, werden dann von JQuery ausgewertet
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

        lblNotApprovedMode.Visible = False
        ihNotApprovedMode.Value = "0"
        lbtnCopy.Visible = (Not blnSearchMode) AndAlso userId <> -1
        lbtnApprove.Visible = False
    End Sub

    Private Sub SearchNotApprovedMode(ByVal search As Boolean, ByVal edit As Boolean)
        'gewünschte Expand/Collapse-Stati für die Seitenabschnitte in hidden fields setzen, werden dann von JQuery ausgewertet
        If search Or edit Then
            lblNotApprovedMode.Visible = True
            ihNotApprovedMode.Value = "1"
        Else
            lblNotApprovedMode.Visible = False
            ihNotApprovedMode.Value = "0"
        End If

        If search Then
            ihExpandstatusSearchFilterArea.Value = "0"
            ihExpandstatusSearchResultArea.Value = "1"
            ihExpandStatusInputArea.Value = "0"
        ElseIf edit Then
            ihExpandstatusSearchFilterArea.Value = "0"
            ihExpandstatusSearchResultArea.Value = "0"
            ihExpandStatusInputArea.Value = "1"
        Else
            ihExpandstatusSearchFilterArea.Value = "1"
            ihExpandstatusSearchResultArea.Value = "0"
            ihExpandStatusInputArea.Value = "0"
        End If

        lbtnSave.Visible = False
        lbtnDistrict.Visible = False
        trMatrix.Visible = False
        lbtnCancel.Visible = search OrElse edit
        lbtnCancel0.Visible = search OrElse edit
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
        If Not FillEdit(intUserID, True) Then
            lbtnApprove.Enabled = False
        Else
            lblMessage.Text = "Benutzer freigeben?"
            lbtnApprove.Enabled = True
        End If
        LockEdit(True)
        lbtnCancel.Text = " &#149;&nbsp;Abbrechen"
        SearchNotApprovedMode(False, True)
    End Sub

    Private Sub Search(Optional ByVal blnShowDataGrid As Boolean = False, _
                       Optional ByVal blnResetSelectedIndex As Boolean = False, _
                       Optional ByVal blnResetPageIndex As Boolean = False, _
                       Optional ByVal blnClearCache As Boolean = False, _
                       Optional ByVal blnNotApproved As Boolean = False)
        ClearEdit()
        If blnClearCache Then
            Session.Remove("myUserListview")
        End If
        If blnResetSelectedIndex Then rgSearchResult.SelectedIndexes.Add(-1)
        If blnResetPageIndex Then rgSearchResult.CurrentPageIndex = 0
        If blnShowDataGrid Then FillDataGrid(blnNotApproved)
        If blnNotApproved Then
            SearchNotApprovedMode(True, False)
        Else
            If blnShowDataGrid Then
                Dim dvUSer As DataView = CType(Session("myUserListView"), DataView)
                If dvUSer.Table.Rows.Count > 0 Then
                    SearchMode()
                Else
                    SearchMode(, True)
                End If
            Else
                SearchMode(, True)
            End If
        End If
    End Sub

    Private Sub Log(ByVal strIdentification As String, ByVal strDescription As String, ByVal tblParameters As DataTable, Optional ByVal strCategory As String = "APP")
        Dim logApp As New CKG.Base.Kernel.Logging.Trace(m_User.App.Connectionstring, m_User.App.SaveLogAccessSAP, m_User.App.LogLevel)

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

    Private Function SetOldLogParameters(ByVal intUserId As Int32) As DataTable
        Try
            Dim _User As New User(intUserId, m_User.App.Connectionstring)

            Dim tblPar = CreateLogTableStructure()

            With tblPar
                .Rows.Add(.NewRow)
                .Rows(.Rows.Count - 1)("Status") = "Alt"
                .Rows(.Rows.Count - 1)("Benutzername") = _User.UserName
                .Rows(.Rows.Count - 1)("Kunden- referenz") = _User.Reference
                .Rows(.Rows.Count - 1)("Test") = _User.IsTestUser
                .Rows(.Rows.Count - 1)("Firmen- Administrator") = _User.IsCustomerAdmin
                .Rows(.Rows.Count - 1)("Firma") = _User.Customer.CustomerName
                If _User.Groups.HasGroups Then
                    .Rows(.Rows.Count - 1)("Gruppe") = _User.Groups(0).GroupName
                Else
                    .Rows(.Rows.Count - 1)("Gruppe") = "-"
                End If
                .Rows(.Rows.Count - 1)("Organisations- Administrator") = _User.Organization.OrganizationAdmin
                .Rows(.Rows.Count - 1)("Organisation") = _User.Organization.OrganizationName
                .Rows(.Rows.Count - 1)("letzte Kennwortänderung") = String.Format("{0:dd.MM.yy}", _User.LastPasswordChange)
                .Rows(.Rows.Count - 1)("Kennwort läuft nie ab") = _User.PasswordNeverExpires
                .Rows(.Rows.Count - 1)("fehlgeschlagene Anmeldungen") = _User.FailedLogins.ToString
                .Rows(.Rows.Count - 1)("Konto gesperrt") = _User.AccountIsLockedOut
                .Rows(.Rows.Count - 1)("Angemeldet") = _User.LoggedOn
                .Rows(.Rows.Count - 1)("ReadMessageCount") = _User.ReadMessageCount
            End With
            Return tblPar
        Catch ex As Exception
            m_App.WriteErrorText(1, m_User.UserName, "UserManagement", "SetOldLogParameters", ex.ToString)

            Dim dt As New DataTable()
            dt.Columns.Add("Fehler beim Erstellen der Log-Parameter", Type.GetType("System.String"))
            dt.Rows.Add(dt.NewRow)
            Dim str As String = ex.Message
            If Not ex.InnerException Is Nothing Then
                str &= ": " & ex.InnerException.Message
            End If
            dt.Rows(0)("Fehler beim Erstellen der Log-Parameter") = str
            Return dt
        End Try
    End Function

    Private Function SetNewLogParameters(ByVal _User As User) As DataTable
        Try
            Dim tblPar = CreateLogTableStructure()

            With tblPar
                .Rows.Add(.NewRow)
                .Rows(.Rows.Count - 1)("Status") = "Neu"
                .Rows(.Rows.Count - 1)("Benutzername") = txtUserName.Text
                .Rows(.Rows.Count - 1)("Kunden- referenz") = txtReference.Text
                .Rows(.Rows.Count - 1)("Test") = cbxTestUser.Checked
                .Rows(.Rows.Count - 1)("Firmen- Administrator") = cbxCustomerAdmin.Checked
                .Rows(.Rows.Count - 1)("Firma") = ddlCustomer.SelectedItem.Text
                .Rows(.Rows.Count - 1)("Gruppe") = ddlGroups.SelectedItem.Text
                .Rows(.Rows.Count - 1)("Organisations- Administrator") = cbxOrganizationAdmin.Checked
                .Rows(.Rows.Count - 1)("Organisation") = ddlOrganizations.SelectedItem.Text
                .Rows(.Rows.Count - 1)("Kennwort läuft nie ab") = cbxPwdNeverExpires.Checked
                Dim strPw As String = ""
                strPw &= "*"
                .Rows(.Rows.Count - 1)("neues Kennwort") = strPw
                Dim strPw2 As String = ""
                strPw2 &= "*"
                .Rows(.Rows.Count - 1)("Kennwortbestätigung") = strPw2

                .Rows(.Rows.Count - 1)("letzte Kennwortänderung") = String.Format("{0:dd.MM.yy}", _User.LastPasswordChange)
                .Rows(.Rows.Count - 1)("fehlgeschlagene Anmeldungen") = _User.FailedLogins.ToString
                .Rows(.Rows.Count - 1)("Konto gesperrt") = _User.AccountIsLockedOut
                .Rows(.Rows.Count - 1)("Angemeldet") = _User.LoggedOn
                .Rows(.Rows.Count - 1)("ReadMessageCount") = CInt(txtReadMessageCount.Text)
            End With
            Return tblPar
        Catch ex As Exception
            m_App.WriteErrorText(1, m_User.UserName, "UserManagement", "SetNewLogParameters", ex.ToString)

            Dim dt As New DataTable()
            dt.Columns.Add("Fehler beim Erstellen der Log-Parameter", Type.GetType("System.String"))
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
            .Columns.Add("Status", Type.GetType("System.String"))
            .Columns.Add("Benutzername", Type.GetType("System.String"))

            .Columns.Add("Vorname", Type.GetType("System.String"))
            .Columns.Add("Nachname", Type.GetType("System.String"))

            .Columns.Add("Kunden- referenz", Type.GetType("System.String"))
            .Columns.Add("Test", Type.GetType("System.Boolean"))
            .Columns.Add("Firmen- Administrator", Type.GetType("System.Boolean"))
            .Columns.Add("Firma", Type.GetType("System.String"))
            .Columns.Add("Gruppe", Type.GetType("System.String"))
            .Columns.Add("Organisations- Administrator", Type.GetType("System.Boolean"))
            .Columns.Add("Organisation", Type.GetType("System.String"))
            .Columns.Add("letzte Kennwortänderung", Type.GetType("System.String"))
            .Columns.Add("Kennwort läuft nie ab", Type.GetType("System.Boolean"))
            .Columns.Add("fehlgeschlagene Anmeldungen", Type.GetType("System.String"))
            .Columns.Add("Konto gesperrt", Type.GetType("System.Boolean"))
            .Columns.Add("Angemeldet", Type.GetType("System.Boolean"))
            .Columns.Add("neues Kennwort", Type.GetType("System.String"))
            .Columns.Add("Kennwortbestätigung", Type.GetType("System.String"))
            .Columns.Add("ReadMessageCount", Type.GetType("System.Int32"))
        End With
        Return tblPar
    End Function

    '-----------
    'Erstellt die Tabelle für das Setzen der Rechte
    '-----------
    Private Sub Fill_Matrix(ByVal KUNNR As String, ByVal Mandt As String)
        Dim CurrentRowIndex As Integer = 0
        Dim CurrentColumnIndex As Integer = 0
        Dim GroupId As String
        Dim GroupName As String

        Matrix.Rows.Clear() ' zurücksetzen der Matrix bei Gruppenauswahl

        GroupId = ddlGroups.SelectedItem.Value
        GroupName = txtUserID.Text
        ReadDistrictsAndRights()

        Dim Applications As ArrayList = Get_Applications(GroupId)
        Dim Rows As Integer = Applications.Count + 3 ' Für 'Vorbelegung' + 'Alles auswählen' + Trennlinie, 'Überschrift' in Zeile 0

        If m_Districts IsNot Nothing Then
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
                        Dim lblVorbelegt As Label = New Label()
                        lblVorbelegt.ID = "lblVorbelegt"
                        lblVorbelegt.Text = "Vorbelegt"
                        Cell.Controls.Add(lblVorbelegt)

                    ElseIf CurrentColumnIndex = 0 And CurrentRowIndex = 2 Then
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
        End If

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
                    Dim lblVorbelegt As Label = New Label()
                    lblVorbelegt.ID = "lblVorbelegt"
                    lblVorbelegt.Text = "Vorbelegt"
                    Cell.Controls.Add(lblVorbelegt)

                ElseIf CurrentColumnIndex = 0 And CurrentRowIndex = 2 Then
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


    '-------
    'Sperrung durch
    '-------
    Private Function GetHistoryInfos(ByVal objUser As User) As String

        Dim cn As New SqlConnection(m_User.App.Connectionstring)
        Try
            cn.Open()
            If cn.State = ConnectionState.Closed Then
                cn.Open()
            End If
            Dim cmd As SqlClient.SqlCommand = New SqlCommand("SELECT LastChangedBy FROM AdminHistory_User " & _
                "WHERE ID = (SELECT MAX(ID) FROM AdminHistory_User WHERE Username = @Username AND Action = 'Benutzer gesperrt')", cn)

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
        Dim Districts As CKG.Base.Kernel.Common.Search
        Dim SessionID As String = Session.SessionID.ToString
        Dim AppID As String = "601"
        Dim i As Integer
        Dim _customer As New Customer(CInt(ddlCustomer.SelectedItem.Value), m_User.App.Connectionstring)
        Dim _User As New User(CInt(txtUserID.Text), m_User.App.Connectionstring)
        Districts = New CKG.Base.Kernel.Common.Search(m_App, _User, SessionID, AppID)
        i = Districts.Show(txtUserID.Text, Right("0000000000" & _customer.KUNNR, 10))

        m_Rights = Districts.Rights
        m_Districts = Districts.Districts
    End Sub

    '-----------
    'Speichert die Berechtigungen nach SAP
    '-----------
    Private Sub SetDistrictRights(ByVal Rights As DataTable)
        Dim Districts As CKG.Base.Kernel.Common.Search
        Dim SessionID As String = Session.SessionID.ToString
        Dim AppID As String = "601"
        Dim _User As New User(CInt(txtUserID.Text), m_User.App.Connectionstring)

        Districts = New CKG.Base.Kernel.Common.Search(m_App, _User, SessionID, AppID)
        Districts.Rights = Rights
        Districts.Change()
    End Sub

    Private Function Get_Applications(ByVal GroupId As Integer) As ArrayList
        Dim Connection As New SqlConnection()
        Dim Command As New SqlCommand()
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
            Dim DataReader As SqlDataReader
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

#Region "Helper"

    '------------
    'Liefert einen Filterausdruck für das Rights-DataTable
    '------------
    Private Function GetFilterExpression(ByVal kundennr As String, ByVal districtID As String, ByVal groupname As String, ByVal vorbelegung As String, ByVal ApplicationId As String, ByVal ohneGeloeschte As Boolean, ByVal invertDistrikt As Boolean) As String

        'Werte aufbereiten
        kundennr = Right("0000000000" + kundennr, 10)

        'Ausdruck erstellen
        Dim needAND As Boolean = False

        Dim res As New StringBuilder()
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

        Catch ex As Exception
            lblError.Text = ex.Message
            intTemp = -1
        End Try
        cn.Close()
        Return intTemp
    End Function
#End Region

#Region "Grid"

    '--------
    'Es wurde ein anderer Distrikt als Vorbelegung ausgewählt
    '--------
    Public Sub VorbelegungChanged(ByVal sender As Object, ByVal e As EventArgs)
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
    Public Sub AllesAuswaehlenChanged(ByVal sender As Object, ByVal e As EventArgs)
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
    Public Sub RechtChanged(ByVal sender As Object, ByVal e As EventArgs)
        Dim CheckBox As CheckBox = CType(sender, CheckBox)
        If CheckBox.Checked = True Then
            chk_Matrix1.Checked = True
            chk_Matrix1.Enabled = True
        End If
        Session("Changed") = 1
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

    Private Sub chk_Matrix1_CheckedChanged(ByVal sender As System.Object, ByVal e As EventArgs) Handles chk_Matrix1.CheckedChanged
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
                If (upFile.PostedFile.ContentLength > CType(ConfigurationManager.AppSettings("MaxUploadSize"), Integer)) Then
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
                    Dim info As IO.FileInfo
                    Dim uFile As HttpPostedFile = upFile.PostedFile

                    Dim fnameNew As String = ConfigurationManager.AppSettings("UploadPathLocal") & "responsible\" & txtUserID.Text & ".jpg"
                    info = New IO.FileInfo(fnameNew)
                    If (info.Exists) Then
                        IO.File.Delete(fnameNew)
                    End If

                    uFile.SaveAs(fnameNew)
                    info = New IO.FileInfo(fnameNew)
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

            Dim info As IO.FileInfo

            Dim fnameNew As String = ConfigurationManager.AppSettings("UploadPathLocal") & "responsible\" & txtUserID.Text & ".jpg"
            info = New IO.FileInfo(fnameNew)
            If (info.Exists) Then
                IO.File.Delete(fnameNew)
            End If

            Dim _User As New User(CInt(txtUserID.Text), m_User.App.Connectionstring)
            _User.SetEmployeePicture(False, m_User.UserName)

            FillEdit(CInt(txtUserID.Text))
        Catch ex As Exception
            lblError.Text = "Fehler beim Löschen. (" & ex.ToString & ")"
        End Try
    End Sub

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As EventArgs) Handles Me.PreRender

        SetEndASPXAccess(Me)

        ' Focussierung findet nun Javascriptseitig statt
        'If Not IsPostBack Then
        '    txtFilterUserName.Focus()
        'End If


    End Sub

    Private Sub Page_Unload(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Unload
        SetEndASPXAccess(Me)
    End Sub

    Protected Sub btnEmpty_Click(ByVal sender As Object, ByVal e As ImageClickEventArgs) Handles btnEmpty.Click
        btnSuche_Click(sender, e)
    End Sub

#Region " Events "

    Private Sub lbtnCancel_Click(ByVal sender As System.Object, ByVal e As EventArgs) Handles lbtnCancel.Click

        Dim editNotApproved As Boolean = False
        lbtnDistrict.Visible = False
        If Not ViewState("editNotApproved") Is Nothing Then
            editNotApproved = CBool(ViewState("editNotApproved"))
        End If
        If Not editNotApproved Then
            Dim searchNotApproved As Boolean = False
            If Not ViewState("searchNotApproved") Is Nothing Then
                searchNotApproved = CBool(ViewState("searchNotApproved"))
            End If
            If searchNotApproved Then
                'zurücksetzen
                SearchNotApprovedMode(False, False)
                Search(True, True, True, True)
            Else
                'normales cancel
                Search(True, True)
            End If
        Else
            Search(True, True, True, True, True)
            SearchNotApprovedMode(True, False)

        End If
    End Sub

    Private Sub lbtnNew_Click(ByVal sender As System.Object, ByVal e As EventArgs) Handles lbtnNew.Click

        Session("UsernameStart") = Nothing
        Session("LockedOutStart") = Nothing

        btnCreatePassword.Enabled = False
        SearchMode(False)
        ClearEdit()

        Dim intCustomerID As Integer = CInt(ddlCustomer.SelectedItem.Value)

        If intCustomerID > 0 Then
            Dim _customer As New Customer(intCustomerID, m_User.App.Connectionstring)
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

        txtUserName.Focus()
        trMasterUser.Visible = False

        refill_Groups()
        initialize_ReferenceFields()

    End Sub

    Private Sub lbtnSave_Click(ByVal sender As System.Object, ByVal e As EventArgs) Handles lbtnSave.Click
        Dim tblLogParameter As DataTable
        Dim strPwd As String = String.Empty
        Dim blnEingabenOK As Boolean = True

        Try
            If String.IsNullOrEmpty(txtUserName.Text) Then
                lblError.Text = "Bitte geben Sie einen Benutzernamen an!"
                blnEingabenOK = False
            End If

            Dim _customer As New Customer(CInt(ddlCustomer.SelectedItem.Value), m_User.App.Connectionstring)
            lbtnDistrict.Visible = False
            If Not txtPassword.Visible Then 'eMail-Adresse soll nicht Pflichtfeld sein, wenn manuelle Passwort-Eingabe
                If txtMail.Text.Trim(" "c).Length = 0 Then
                    lblError.Text &= "Bitte geben Sie eine Email-Adresse an.<br /><br />"
                    blnEingabenOK = False
                Else
                    If (InStr(txtMail.Text, "@") = 0) Or (InStr(txtMail.Text, ".") = 0) Then
                        lblError.Text &= "Bitte geben Sie eine Email-Adresse im Format ""account@server.de"" an.<br /><br />"
                        blnEingabenOK = False
                    End If
                End If
            End If
            If Not IsNumeric(txtReadMessageCount.Text) Then
                lblError.Text &= "Bitte geben Sie einen Zahlenwert für die Anzahl der Startmeldungs-Anzeigen ein.<br /><br />"
                blnEingabenOK = False
            End If
            If ddlTitle.SelectedItem Is Nothing OrElse ddlTitle.SelectedItem.Value = "-" Then
                If Not _customer.NameInputOptional Then
                    lblError.Text &= "Bitte wählen Sie eine Anrede aus.<br /><br />"
                    blnEingabenOK = False
                End If
            End If
            If txtFirstName.Text = String.Empty Then
                If Not _customer.NameInputOptional Then
                    lblError.Text &= "Bitte geben Sie einen Vornamen an.<br /><br />"
                    blnEingabenOK = False
                End If
            End If
            If txtLastName.Text = String.Empty Then
                If Not _customer.NameInputOptional Then
                    lblError.Text &= "Bitte geben Sie einen Nachnamen an.<br /><br />"
                    blnEingabenOK = False
                End If
            End If

            If txtUserID.Text = "-1" Then
                'bei neuen Benutzern prüfen ob Name gesperrt oder bereits genutzt
                'gesperrte
                Dim intLoop As Integer
                Dim dvForbiddenUserName As DataView
                Dim dtForbiddenUserNameAll As Kernel.ForbiddenUserNameAllList
                Dim cn As New SqlConnection(m_User.App.Connectionstring)
                cn.Open()

                dtForbiddenUserNameAll = New Kernel.ForbiddenUserNameAllList(cn)
                dvForbiddenUserName = dtForbiddenUserNameAll.DefaultView
                For intLoop = 0 To dvForbiddenUserName.Count - 1
                    If InStr(UCase(txtUserName.Text), UCase(CStr(dvForbiddenUserName(intLoop)("UserName")))) > 0 Then
                        lblError.Text &= "Bitte wählen Sie einen anderen Namen für den neuen Benutzer!"
                        lblError.Text &= " <br>(Der Name oder ein Teil davon ist eine gesperrte Zeichenfolge.)<br /><br />"
                        blnEingabenOK = False
                        Exit For
                    End If
                Next

                'bereits genutzte
                Dim dvGivvenUserName As DataView
                Dim dtGivvenUserNameAll As Kernel.GivvenUserNameAllList
                Dim cnGivven As New SqlConnection(m_User.App.Connectionstring)
                cnGivven.Open()

                dtGivvenUserNameAll = New Kernel.GivvenUserNameAllList(cn)
                dvGivvenUserName = dtGivvenUserNameAll.DefaultView
                For intLoop = 0 To dvGivvenUserName.Count - 1
                    If UCase(txtUserName.Text) = UCase(CStr(dvGivvenUserName(intLoop)("UserName"))) Then
                        lblError.Text &= "Bitte wählen Sie einen anderen Namen für den neuen Benutzer!"
                        lblError.Text &= " <br>(Der Benutzername wird bereits benutzt.)<br /><br />"
                        blnEingabenOK = False
                        Exit For
                    End If
                Next

            End If

            Dim _User As User = New User(CInt(txtUserID.Text), _
                                              txtUserName.Text, _
                                              IIf(trReference.Visible, txtReference.Text, ""), _
                                              IIf(trReference2.Visible, txtReference2.Text, ""), _
                                              IIf(trReference3.Visible, txtReference3.Text, ""), _
                                              (trReference4.Visible AndAlso cbxReference4.Checked), _
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
                                              txtValidFrom.Text, _
                                              txtValidTo.Text)

            _User.Email = txtMail.Text
            _User.Employee = chkEmployee.Checked
            _User.Telephone = txtPhone.Text 'Telefonnr. des Benutzers nicht des Mitarbeiters
            _User.Picture = Len(lblPictureName.Text) > 0
            _User.HierarchyID = CInt(ddlHierarchy.SelectedValue)
            _User.Department = txtDepartment.Text
            _User.Position = txtPosition.Text
            _User.PhoneEmployee = txtTelephone.Text
            _User.Fax = txtFax.Text
            _User.UrlRemoteLoginKey = lblUrlRemoteLoginKey.Text

            Dim strLogMsg As String = "User anlegen"
            Dim strTemp As String = txtUserID.Text
            If txtUserID.Text <> "-1" Then
                strLogMsg = "User ändern"
                tblLogParameter = SetOldLogParameters(CInt(txtUserID.Text))
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
                    _User.Groups.Add(New Group(intGroupID, CInt(ddlCustomer.SelectedItem.Value)))
                End If
            Else
                lblError.Text &= "Bitte geben Sie für den Mitarbeiter eine Gruppe an!<br /><br />"
                blnEingabenOK = False
            End If

            Dim intOrganizationID As Integer
            If Not ddlOrganizations.Items.Count = 0 Then
                intOrganizationID = CInt(ddlOrganizations.SelectedItem.Value)
            Else
                intOrganizationID = 0
            End If

            Dim isNewUser = (txtUserID.Text = "-1")
            Dim isApproved = cbxApproved.Checked
            Dim newPassword = chkNewPasswort.Checked

            ' Erst validieren, dann speichern..
            If isNewUser OrElse Not String.IsNullOrEmpty(txtPassword.Text) Then

                If _customer.CustomerPasswordRules.DontSendEmail Or Not _User.HighestAdminLevel = AdminLevel.None Or cbxOrganizationAdmin.Checked Then
                    ' Wenn Passwort nicht per Mail txtFelder abfragen
                    If txtPassword.Text = String.Empty Then
                        lblError.Text &= "Bitte geben Sie für den neuen Benutzer ein Passwort an!<br /><br />"
                        blnEingabenOK = False
                    ElseIf txtPassword.Text <> txtConfirmPassword.Text Then
                        lblError.Text &= "Die eingegebenen Passwörter stimmen nicht überein!<br /><br />"
                        blnEingabenOK = False
                    End If
                    strPwd = txtPassword.Text
                End If
            End If

            ' Falls Benutzer entsperrt wurde, Feld "gültig bis" zurücksetzen
            If Not Session("LockedOutStart") Is Nothing Then
                If _User.Approved And _User.AccountIsLockedOut = False And CBool(Session("LockedOutStart")) = True Then
                    _User.ValidTo = ""
                End If
            End If

            'wenn alle Eingaben in Ordnung -> speichern
            If blnEingabenOK Then
                Dim blnSuccess As Boolean = False

                'User speichern
                If _User.Save() Then
                    txtUserID.Text = _User.UserID.ToString

                    ' Rechte zuordnen
                    SaveRightsForUser(_User.UserID.ToString)

                    ' Wenn Passwortänderung
                    If Not String.IsNullOrEmpty(strPwd) Then
                        Dim pword As String = strPwd
                        Dim pwordconfirm As String = strPwd

                        If Not _User.ChangePassword("", pword, pwordconfirm, m_User.UserName, True) Then
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


                Else
                    lblError.Text = _User.ErrorMessage
                End If
                tblLogParameter = SetNewLogParameters(_User)
                Log(_User.UserID.ToString, strLogMsg, tblLogParameter)

                If blnSuccess Then
                    lblMessage.Text = "Die Änderungen wurden gespeichert."
                    Search(True, True, , True)
                    Dim errorMessage As String = ""

                    ' Versand von neuen Benutzerdaten erst nach Freigabe, daher in lbtnApproved_Click

                    ' Ausnahme für Orgaadmins und Kundenadmins, die Benutzer anlegen ++++++++++++++++++
                    If isNewUser And isApproved And Session("UsernameStart") Is Nothing Then

                        ' Neuanlage Benutzer (ohne Adminrechte) Authentifizierungs-Email versenden
                        If _User.HighestAdminLevel = AdminLevel.None Then
                            ' Wenn Passwort und Username per Mail dann Validierungsprozess
                            If Not _User.Customer.CustomerUsernameRules.DontSendEmail And Not _User.Customer.CustomerPasswordRules.DontSendEmail Then

                                'Mail versenden
                                If Not _User.SendUsernameMail(errorMessage) Then
                                    lblError.Text = errorMessage
                                End If

                            Else

                                ' Sonst prüfen ob Passwort oder Username per Mail und diese verschicken
                                If _User.Customer.CustomerUsernameRules.DontSendEmail Then
                                    If Not _User.Customer.CustomerPasswordRules.DontSendEmail Then
                                        _User.SendPasswordResetMail(errorMessage, CKG.Base.Kernel.Security.User.PasswordMailMode.Neu)
                                    End If
                                ElseIf _User.Customer.CustomerPasswordRules.DontSendEmail Then
                                    _User.SendUsernameMail(errorMessage, False)
                                End If

                            End If
                        End If

                    Else ' Neue Benutzer nicht freigegeben und geänderte Benutzer

                        ' Neue Benutzer werden im lbtnApproved_Click behandelt
                        ' für alle anderen gilt

                        ' Link für neues Passwort verschicken
                        If Not isNewUser AndAlso newPassword Then
                            _User.SendPasswordResetMail(errorMessage, CKG.Base.Kernel.Security.User.PasswordMailMode.Zuruecksetzen)
                        End If

                        ' Benutzername per Mail +++++++++++++++++++++++++
                        ' Wenn vorhandener Benutzer geändert
                        If Not (Session("UsernameStart") Is Nothing) Then
                            ' Username geändert AND Username nicht leer AND User kein Admin
                            If _User.UserName <> CStr(Session("UsernameStart")) And _User.UserName <> String.Empty And _
                                _User.HighestAdminLevel = AdminLevel.None And Not cbxOrganizationAdmin.Checked Then
                                If Not _User.SendUsernameChangedMail(errorMessage) Then
                                    lblError.Text = errorMessage
                                End If
                            End If
                            ' Falls Benutzer entsperrt wurde
                            If Not Session("LockedOutStart") Is Nothing Then
                                If _User.Approved And _User.AccountIsLockedOut = False And CBool(Session("LockedOutStart")) = True Then
                                    If Not _User.SendUserUnlockMail(errorMessage, m_User) Then
                                        lblError.Text = errorMessage
                                    End If
                                End If
                            End If
                        End If

                    End If

                    If _customer.ShowDistrikte AndAlso Session("Changed") = 1 Then

                        ErmitteleRechteAusCheckBoxen(Matrix.Rows.GetEnumerator, m_Rights)
                        Dim selectedDistrict As String = GetSelectedDistrict()
                        SetzeVorbelegswertFuerDistrikt(selectedDistrict, Session("UserID").ToString, _customer.KUNNR, True, False)
                        SetzeVorbelegswertFuerDistrikt(selectedDistrict, Session("UserID").ToString, _customer.KUNNR, False, True)
                        SetDistrictRights(m_Rights)

                    End If
                End If
            End If

        Catch ex As Exception
            m_App.WriteErrorText(1, m_User.UserName, "UserManagement", "lbtnSave_Click", ex.ToString)

            lblError.Text &= ex.Message
            If Not ex.InnerException Is Nothing Then
                lblError.Text &= ": " & ex.InnerException.Message
            End If
            tblLogParameter = New DataTable
            Log(txtUserID.Text, lblError.Text, tblLogParameter, "ERR")
        End Try
    End Sub

    Private Sub lbtnDelete_Click(ByVal sender As System.Object, ByVal e As EventArgs) Handles lbtnDelete.Click
        Dim tblLogParameter As DataTable
        Try

            Dim _User As New User()
            tblLogParameter = SetOldLogParameters(CInt(txtUserID.Text))
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

    End Sub

    Private Sub ddlFilterCustomer_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As EventArgs) Handles ddlFilterCustomer.SelectedIndexChanged
        Dim intCustomerID As Integer = CInt(ddlFilterCustomer.SelectedItem.Value)
        Dim cn As New SqlConnection(m_User.App.Connectionstring)
        cn.Open()
        If intCustomerID > 0 Then
            ddlCustomer.SelectedItem.Selected = False
            ddlCustomer.Items.FindByValue(intCustomerID.ToString).Selected = True
            FillGroups(intCustomerID, cn)
            FillOrganizations(intCustomerID, cn)
            Dim _customer As New Customer(intCustomerID, m_User.App.Connectionstring)
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

    Private Sub ddlCustomer_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As EventArgs) Handles ddlCustomer.SelectedIndexChanged
        refill_Groups()
        initialize_ReferenceFields()

        'NameEditMode(Not _customer.CustomerPasswordRules.NameInputOptional)
    End Sub

    Private Sub refill_Groups()
        Dim intCustomerID As Integer = CInt(ddlCustomer.SelectedItem.Value)
        Dim cn As New SqlConnection(m_User.App.Connectionstring)
        cn.Open()
        Dim dtGroups As New Kernel.GroupList(intCustomerID, cn, m_User.Customer.AccountingArea, , True)
        FillGroup(ddlGroups, False, dtGroups)
        Dim dtOrganizations As New OrganizationList(intCustomerID, cn, m_User.Customer.AccountingArea)
        FillOrganization(ddlOrganizations, False, dtOrganizations)
        Dim _customer As New Customer(intCustomerID, m_User.App.Connectionstring)

        Dim autoPW As Boolean = False
        ' AutoPasswort wenn Passwort per Mail OR Kein Kundenadmin OR kein Orga-Admin
        If cbxNoCustomerAdmin.Checked And cbxOrganizationAdmin.Checked = False Then
            If Not _customer.CustomerPasswordRules.DontSendEmail Then
                autoPW = True
            End If
        End If

        PasswordEditMode(autoPW)

        ddlCustomer.Focus()
    End Sub

    Private Sub initialize_ReferenceFields()
        Dim intCustomerID As Integer = CInt(ddlCustomer.SelectedItem.Value)
        Dim _customer As New Customer(intCustomerID, m_User.App.Connectionstring)

        ' Referenzfelder initialisieren
        If Not String.IsNullOrEmpty(_customer.ReferenceType1) Then
            trReference.Visible = True
            lblReferenceType.Text = String.Format("{0}:", _customer.ReferenceType1Name)
        Else
            trReference.Visible = False
        End If
        If Not String.IsNullOrEmpty(_customer.ReferenceType2) Then
            trReference2.Visible = True
            lblReferenceType2.Text = String.Format("{0}:", _customer.ReferenceType2Name)
        Else
            trReference2.Visible = False
        End If
        If Not String.IsNullOrEmpty(_customer.ReferenceType3) Then
            trReference3.Visible = True
            lblReferenceType3.Text = String.Format("{0}:", _customer.ReferenceType3Name)
        Else
            trReference3.Visible = False
        End If
        If Not String.IsNullOrEmpty(_customer.ReferenceType4) Then
            trReference4.Visible = True
            lblReferenceType4.Text = String.Format("{0}:", _customer.ReferenceType4Name)
        Else
            trReference4.Visible = False
        End If
    End Sub

    Private Sub btnSuche_Click(ByVal sender As System.Object, ByVal e As EventArgs) Handles btnSuche.Click

        If ihNotApprovedMode.Value = "0" Then
            'normale Suche
            Search(True, True, True, True)
        Else
            'nur nicht freigegebene
            Search(True, True, True, True, True)
        End If

    End Sub

    Private Sub lbtnApprove_Click(ByVal sender As System.Object, ByVal e As EventArgs) Handles lbtnApprove.Click
        Dim _User As New User(CInt(txtUserID.Text), m_User.App.Connectionstring)
        _User.Approve(m_User.UserName)

        Dim errorMessage As String = ""

        ' Neuanlage Benutzer (ohne Adminrechte) Authentifizierungs-Email versenden
        If _User.HighestAdminLevel = AdminLevel.None Then
            ' Wenn Passwort und Username per Mail dann Validierungsprozess
            If Not _User.Customer.CustomerUsernameRules.DontSendEmail And Not _User.Customer.CustomerPasswordRules.DontSendEmail Then

                'Mail versenden
                errorMessage = String.Empty
                If Not _User.SendUsernameMail(errorMessage) Then
                    lblError.Text &= errorMessage & "<br /><br />"
                End If

            Else

                ' Sonst prüfen ob Passwort oder Username per Mail und diese verschicken
                If _User.Customer.CustomerUsernameRules.DontSendEmail Then
                    If Not _User.Customer.CustomerPasswordRules.DontSendEmail Then
                        errorMessage = String.Empty
                        _User.SendPasswordResetMail(errorMessage, CKG.Base.Kernel.Security.User.PasswordMailMode.Neu)
                        If Not String.IsNullOrEmpty(errorMessage) Then lblError.Text &= errorMessage & "<br /><br />"
                    End If
                ElseIf _User.Customer.CustomerPasswordRules.DontSendEmail Then
                    errorMessage = String.Empty
                    _User.SendUsernameMail(errorMessage, False)
                    If Not String.IsNullOrEmpty(errorMessage) Then lblError.Text &= errorMessage & "<br /><br />"
                End If

            End If

        End If

        lbtnNotApproved_Click(sender, e)
    End Sub

    Private Sub lbtnNotApproved_Click(ByVal sender As System.Object, ByVal e As EventArgs) Handles lbtnNotApproved.Click
        SearchNotApprovedMode(True, False)
        btnSuche_Click(sender, e)
    End Sub

    Private Sub cbxFirstLevelAdmin_CheckedChanged(sender As Object, e As EventArgs) Handles cbxFirstLevelAdmin.CheckedChanged
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

        cbxFirstLevelAdmin.Focus()
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
        cbxOrganizationAdmin.Focus()

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

        cbxNoCustomerAdmin.Focus()
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
        cbxCustomerAdmin.Focus()

    End Sub

    Private Sub lbtnDistrict_Click(ByVal sender As System.Object, ByVal e As EventArgs) Handles lbtnDistrict.Click
        Try
            Page_LoadDistikte()
            Session("UserID") = txtUserID.Text

        Catch ex As Exception
            lblError.Text = ex.Message
        End Try

    End Sub

#End Region

#Region "Load"

    Private Sub Page_LoadDistikte()
        Try
            Dim _User As New User(CInt(txtUserID.Text), m_User.App.Connectionstring)
            Fill_Matrix(_User.KUNNR, "")
            Matrix.Visible = True
            trMatrix.Visible = True
        Catch ex As Exception
            lblError.Text = "Beim Laden der Seite ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"

        End Try
    End Sub

#End Region

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

    Protected Sub lbtnCancel0_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lbtnCancel0.Click
        Dim editNotApproved As Boolean = False
        lbtnDistrict.Visible = False
        If Not ViewState("editNotApproved") Is Nothing Then
            editNotApproved = CBool(ViewState("editNotApproved"))
        End If
        If Not editNotApproved Then
            Dim searchNotApproved As Boolean = False
            If Not ViewState("searchNotApproved") Is Nothing Then
                searchNotApproved = CBool(ViewState("searchNotApproved"))
            End If
            If searchNotApproved Then
                'zurücksetzen
                SearchNotApprovedMode(False, False)
                Search(False, True, True, True)
            Else
                'normales cancel
                Search(True, True)
            End If
        Else
            Search(True, True, True, True, True)
            SearchNotApprovedMode(True, False)
        End If
    End Sub

    Protected Sub rgSearchResultSortCommand(sender As Object, e As GridSortCommandEventArgs) Handles rgSearchResult.SortCommand

        Dim blnIsInNotApprovedMode = (ihNotApprovedMode.Value = "1")
        FillDataGrid(blnIsInNotApprovedMode)

    End Sub

    Protected Sub rgSearchResult_ExcelMLExportRowCreated(sender As Object, e As GridExportExcelMLRowCreatedArgs) Handles rgSearchResult.ExcelMLExportRowCreated

        If e.RowType = GridExportExcelMLRowType.DataRow Then

            If Not isExcelExportConfigured Then

                'Set Worksheet name
                e.Worksheet.Name = "Benutzerliste"

                'Set Column widths
                For Each column As ColumnElement In e.Worksheet.Table.Columns

                    If (e.Worksheet.Table.Columns.IndexOf(column) = 2) Then
                        column.Width = Unit.Point(180) 'set width 180 to ProductName column
                    Else
                        column.Width = Unit.Point(80) 'set width 80 to the rest of the columns
                    End If
                Next

                'Set Page options
                Dim pageSetup As PageSetupElement = e.Worksheet.WorksheetOptions.PageSetup
                pageSetup.PageLayoutElement.IsCenteredVertical = True
                pageSetup.PageLayoutElement.IsCenteredHorizontal = True
                pageSetup.PageMarginsElement.Left = 0.5
                pageSetup.PageMarginsElement.Top = 0.5
                pageSetup.PageMarginsElement.Right = 0.5
                pageSetup.PageMarginsElement.Bottom = 0.5
                pageSetup.PageLayoutElement.PageOrientation = PageOrientationType.Landscape

                'Freeze panes
                e.Worksheet.WorksheetOptions.AllowFreezePanes = True
                e.Worksheet.WorksheetOptions.LeftColumnRightPaneNumber = 1
                e.Worksheet.WorksheetOptions.TopRowBottomPaneNumber = 1
                e.Worksheet.WorksheetOptions.SplitHorizontalOffset = 1
                e.Worksheet.WorksheetOptions.SplitVerticalOffest = 1


                e.Worksheet.WorksheetOptions.ActivePane = 2
                isExcelExportConfigured = True

            End If
        End If
    End Sub

    Protected Sub rgSearchResult_ExcelMLExportStylesCreated(sender As Object, e As GridExportExcelMLStyleCreatedArgs) Handles rgSearchResult.ExcelMLExportStylesCreated

        'Add currency and percent styles

        Dim priceStyle As StyleElement = New StyleElement("priceItemStyle")
        priceStyle.NumberFormat.FormatType = NumberFormatType.Currency
        priceStyle.FontStyle.Color = Drawing.Color.Red
        e.Styles.Add(priceStyle)

        Dim alternatingPriceStyle As StyleElement = New StyleElement("alternatingPriceItemStyle")
        alternatingPriceStyle.NumberFormat.FormatType = NumberFormatType.Currency
        alternatingPriceStyle.FontStyle.Color = Drawing.Color.Red
        e.Styles.Add(alternatingPriceStyle)

        Dim percentStyle As StyleElement = New StyleElement("percentItemStyle")
        percentStyle.NumberFormat.FormatType = NumberFormatType.Percent
        percentStyle.FontStyle.Italic = True
        e.Styles.Add(percentStyle)

        Dim alternatingPercentStyle As StyleElement = New StyleElement("alternatingPercentItemStyle")
        alternatingPercentStyle.NumberFormat.FormatType = NumberFormatType.Percent
        alternatingPercentStyle.FontStyle.Italic = True
        e.Styles.Add(alternatingPercentStyle)

        'Apply background colors 
        For Each style As StyleElement In e.Styles

            If style.Id = "headerStyle" Then

                style.InteriorStyle.Pattern = InteriorPatternType.Solid
                style.InteriorStyle.Color = Drawing.Color.Gray

            End If
            If style.Id = "alternatingItemStyle" Or style.Id = "alternatingPriceItemStyle" Or style.Id = "alternatingPercentItemStyle" Or style.Id = "alternatingDateItemStyle" Then

                style.InteriorStyle.Pattern = InteriorPatternType.Solid
                style.InteriorStyle.Color = Drawing.Color.LightGray

            End If

            If style.Id.Contains("itemStyle") Or style.Id = "priceItemStyle" Or style.Id = "percentItemStyle" Or style.Id = "dateItemStyle" Then

                style.InteriorStyle.Pattern = InteriorPatternType.Solid
                style.InteriorStyle.Color = Drawing.Color.White

            End If
        Next
    End Sub

    Protected Sub rgSearchResultItemCommand(sender As Object, e As GridCommandEventArgs)

    End Sub

    Protected Sub rgSearchResultNeedDataSource(sender As Object, e As GridNeedDataSourceEventArgs) Handles rgSearchResult.NeedDataSource

        Dim blnIsInNotApprovedMode = (ihNotApprovedMode.Value = "1")
        If ihIsInitialDataLoad.Value = "1" Then
            FillDataGrid(blnIsInNotApprovedMode, False, True)
        Else
            FillDataGrid(blnIsInNotApprovedMode, False)
        End If

    End Sub

    Protected Sub rgSearchResultPageChanged(sender As Object, e As GridPageChangedEventArgs) Handles rgSearchResult.PageIndexChanged
        Dim blnIsInNotApprovedMode = (ihNotApprovedMode.Value = "1")
        FillDataGrid(blnIsInNotApprovedMode)
    End Sub

    Protected Sub rgSearchResultPageSizeChanged(sender As Object, e As GridPageSizeChangedEventArgs) Handles rgSearchResult.PageSizeChanged
        Dim blnIsInNotApprovedMode = (ihNotApprovedMode.Value = "1")
        FillDataGrid(blnIsInNotApprovedMode)
    End Sub

    Protected Sub rgSearchResulttemCommand(sender As Object, e As GridCommandEventArgs) Handles rgSearchResult.ItemCommand

        If e.CommandName = "Edit" Then
            Dim searchNotApproved As Boolean = False
            If Not ViewState("searchNotApproved") Is Nothing Then
                searchNotApproved = CBool(ViewState("searchNotApproved"))
            End If
            Dim CtrlLabel As Label
            CtrlLabel = e.Item.Cells(0).FindControl("lblUserID")
            If Not searchNotApproved Then
                'normales edit
                EditEditMode(CInt(CtrlLabel.Text))

            Else
                ApproveMode(CInt(CtrlLabel.Text))
            End If
            btnCreatePassword.Enabled = True

        ElseIf e.CommandName = "Del" Then
            Dim CtrlLabel As Label
            CtrlLabel = e.Item.Cells(0).FindControl("lblUserID")
            EditDeleteMode(CInt(CtrlLabel.Text))
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

    Protected Sub rgSearchResult_EditCommand(sender As Object, e As GridCommandEventArgs) Handles rgSearchResult.EditCommand

    End Sub

    Protected Sub rgSearchResult_ItemDataBound(sender As Object, e As GridItemEventArgs) Handles rgSearchResult.ItemDataBound


        Dim isExport As Boolean = True

        If isExport And e.Item.ItemType = GridItemType.EditFormItem Then


            For Each cell As TableCell In e.Item.Cells

                cell.Text = "test"

            Next


        End If

        If e.Item.ItemType = GridItemType.Item Then
            e.Item.Edit = False

        End If

    End Sub

    Protected Sub lbtnCopy_Click(sender As Object, e As EventArgs) Handles lbtnCopy.Click
        Session("UsernameStart") = Nothing

        btnCreatePassword.Enabled = False
        SearchMode(False)

        txtUserID.Text = "-1"
        chkLoggedOn.Checked = False
        cbxApproved.Checked = True 'default für kopierte User

        ' Nicht kopiert wird:
        txtUserName.Text = String.Empty
        ddlTitle.SelectedIndex = 0
        txtFirstName.Text = String.Empty
        txtLastName.Text = String.Empty
        txtMail.Text = String.Empty
        cbxCustomerAdmin.Checked = False
        cbxFirstLevelAdmin.Checked = False
        lblUrlRemoteLoginKey.Text = ""
        cbxNoCustomerAdmin.Checked = True
        cbxOrganizationAdmin.Checked = False
        txtTelephone.Text = String.Empty

        Dim intCustomerID As Integer = CInt(ddlCustomer.SelectedItem.Value)
        If intCustomerID > 0 Then
            Dim _customer As New Customer(intCustomerID, m_User.App.Connectionstring)
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

        txtUserName.Focus()
    End Sub

    Protected Sub lbtnOpenMasterUserOptions_Click(sender As Object, e As EventArgs) Handles lbtnOpenMasterUserOptions.Click
        If Not m_User.HighestAdminLevel = AdminLevel.Master Then Return

        lblMasterUser.Text = "Wollen Sie ihren Masteruser hierher umhängen?"
        lblMasterUser.CssClass = ""
        lbtnMasterUser.Visible = True
        lbtnCancelMasterUser.Text = "Abbrechen"

        Dim masterUser = FindMasterUser()
        If masterUser Is Nothing Then
            ' kein MasterUser gefunden - anlegen!
            masterUser = CreateMasterUser()
            lblMasterUser.Text &= String.Format("<br />(Es wurde ein neuer Masteruser '{0}' für Sie angelegt.)", masterUser.UserName)
        End If

        masterUserOptions.Show()
    End Sub

    Protected Sub lbtnUrlRemoteLoginKey_Click(sender As Object, e As EventArgs) Handles lbtnUrlRemoteLoginKey.Click
        lblUrlRemoteLoginKey.Text = HttpUtility.UrlEncode(Guid.NewGuid().ToString)
    End Sub

    Protected Sub lbtnUrlRemoteLoginKeyRemove_Click(sender As Object, e As EventArgs) Handles lbtnUrlRemoteLoginKeyRemove.Click
        lblUrlRemoteLoginKey.Text = ""
    End Sub

    Protected Sub lbtnMasterUser_Click(sender As Object, e As EventArgs) Handles lbtnMasterUser.Click
        Dim masterUser = FindMasterUser()

        Dim changedUser = New User(masterUser.UserID, masterUser.UserName, txtReference.Text, txtReference2.Text, txtReference3.Text, cbxReference4.Checked, masterUser.IsTestUser, CInt(ddlCustomer.SelectedValue),
                                   masterUser.IsCustomerAdmin, masterUser.PasswordNeverExpires, masterUser.AccountIsLockedOut,
                                   masterUser.FirstLevelAdmin, masterUser.LoggedOn, masterUser.Organization.OrganizationAdmin, m_User.App.Connectionstring, masterUser.ReadMessageCount,
                                   m_User.UserName, masterUser.Approved, masterUser.FirstName, masterUser.LastName, masterUser.Title, txtStore.Text, masterUser.Matrixfilled, masterUser.ValidFrom, masterUser.ValidTo)

        Dim intGroupID = If(ddlGroups.Items.Count = 0, 0, CInt(ddlGroups.SelectedValue))
        If intGroupID > 0 Then
            'Gruppe ausgewählt
            If Not changedUser.Groups.IsInGroups(intGroupID) Then
                'gewaehlte Gruppe ist neu
                'vorhandene Gruppen loeschen
                '(da nur eine Gruppe je User erlaubt)
                If Not changedUser.Groups.Count = 0 Then
                    Dim gr As Group
                    For Each gr In changedUser.Groups
                        gr.MarkDeleted()
                    Next
                End If
                'neue Gruppe hinzufuegen
                changedUser.Groups.Add(New Group(intGroupID, CInt(ddlCustomer.SelectedItem.Value)))
            End If
        Else
            lblError.Text = "Bitte geben Sie für den Mitarbeiter eine Gruppe an!"
            Exit Sub
        End If

        Dim intOrganizationID = If(ddlOrganizations.Items.Count = 0, 0, CInt(ddlOrganizations.SelectedValue))

        If changedUser.Save() Then
            changedUser.Organization.ReAssignUserToOrganization(m_User.UserName, changedUser.UserID.ToString, changedUser.UserID, intOrganizationID, masterUser.Organization.OrganizationAdmin, m_User.App.Connectionstring)

            lblMasterUser.Text = "Der Masteruser wurde geändert."
            lblMasterUser.CssClass = ""
            masterUserOptions.Hide()
        Else
            lblMasterUser.Text = changedUser.ErrorMessage
            lblMasterUser.CssClass = "TextError"
            lbtnCancelMasterUser.Text = "Schließen"
            lbtnMasterUser.Visible = False
            masterUserOptions.Show()
        End If
    End Sub

    Private Function FindMasterUser() As User
        Dim connectionString = m_User.App.Connectionstring

        Dim userRow = New Kernel.UserList(m_User.UserName & "Master1", 0, 0, 0, connectionString, False, 0, 0).Rows.Cast(Of DataRow).FirstOrDefault()
        If userRow Is Nothing Then Return Nothing

        Return New User(CInt(userRow("UserID")), connectionString)
    End Function

    Private Function CreateMasterUser() As User
        Dim connectionString = m_User.App.Connectionstring

        Try
            Dim newUser = New User(-1, m_User.UserName & "Master1", m_User.Reference, m_User.Reference2, m_User.Reference3, m_User.Reference4, m_User.IsTestUser, m_User.Customer.CustomerId, m_User.IsCustomerAdmin, _
                                   m_User.PasswordNeverExpires, m_User.AccountIsLockedOut, m_User.FirstLevelAdmin, m_User.LoggedOn, m_User.Organization.OrganizationAdmin, _
                                   connectionString, m_User.ReadMessageCount, m_User.UserName, m_User.Approved, m_User.Store, m_User.Matrixfilled, m_User.ValidFrom, m_User.ValidTo)
            newUser.Groups.Add(New Group(m_User.Groups(0).GroupId, m_User.Groups(0).CustomerId))
            If newUser.Save() AndAlso newUser.UserID > 0 Then
                Dim newUserId = newUser.UserID

                newUser.Organization.ReAssignUserToOrganization(m_User.UserName, newUserId.ToString, newUserId, m_User.Organization.OrganizationId, m_User.Organization.OrganizationAdmin, connectionString)

                Dim cnn = New SqlConnection(connectionString)
                If (cnn.State = ConnectionState.Closed) Then cnn.Open()

                Dim cmd = cnn.CreateCommand
                cmd.CommandText = "update WebUser set Password=(select i.Password from WebUser i where i.UserID=@UserId) where UserID=@NewUserId"
                cmd.Parameters.Add(New SqlParameter("UserId", m_User.UserID))
                cmd.Parameters.Add(New SqlParameter("NewUserId", newUserId))
                cmd.ExecuteNonQuery()

                If (cnn.State = ConnectionState.Open) Then cnn.Close()

                Return New User(newUserId, connectionString)
            End If
        Catch ex As Exception
            Diagnostics.Trace.WriteLine(ex)
        End Try

        Return Nothing
    End Function

    Public Sub ShowRightsPerUser(ByVal strUsername As String)

        '        Dim cn As New SqlClient.SqlConnection(m_User.App.Connectionstring)
        drUserRights.DataSource = RightList.ShowRightsPerUser(strUsername)
        drUserRights.Rebind()

    End Sub

    Public Sub SaveRightsForUser(ByVal userId As String)

        Dim txtbox As TextBox
        Dim cbxSetRight As CheckBox
        Dim isChecked As Boolean
        Dim itemCategoryValue As String
        Dim strRightFieldtype As String
        Dim strUserRightValue As String
        Dim strUserName As String
        Dim strCategoryID As String

        strUserName = txtUserName.Text

        Dim cn As New SqlClient.SqlConnection(m_User.App.Connectionstring)

        m_User = GetUser(Me)

        For Each item As GridDataItem In drUserRights.Items

            itemCategoryValue = item("CategoryID").Text

            If item("SettingsValue").FindControl("Recht1").Visible = True Then
                txtbox = item("SettingsValue").FindControl("Recht1")
                strUserRightValue = txtbox.Text
                strRightFieldtype = "txtfield"
            End If

            If item("SettingsValue").FindControl("Recht2").Visible = True Then
                cbxSetRight = item("SettingsValue").FindControl("Recht2")
                strUserRightValue = cbxSetRight.Checked
                strRightFieldtype = "chkbox"
            End If

            RightList.UpdateRightPerUser(strUserName, itemCategoryValue, strUserRightValue, strRightFieldtype)

        Next

    End Sub

End Class