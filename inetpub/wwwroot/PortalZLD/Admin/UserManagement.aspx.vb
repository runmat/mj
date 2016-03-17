Imports CKG.Base.Kernel.Admin
Imports CKG.Base.Kernel.Security
Imports CKG.Base.Kernel.Common.Common
Imports CKG.Base.Business

Public Structure Appl
    Dim Name As String
    Dim FriendlyName As String
    Dim Id As Integer
End Structure

Partial Public Class UserManagement
    Inherits System.Web.UI.Page

    Protected WithEvents GridNavigation1 As Global.CKG.PortalZLD.GridNavigation

#Region " Membervariables "
    Private m_User As User
    Private m_App As App
#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        ' Hier Benutzercode zur Seiteninitialisierung einfügen
        m_User = GetUser(Me)

        lblHead.Text = "Benutzerverwaltung"
        AdminAuth(Me, m_User, AdminLevel.Organization)
        GridNavigation1.setGridElment(dgSearchResult)
        Try
            m_App = New App(m_User)

            If Not IsPostBack Then
                lblError.Text = ""
                FillHierarchy()
                FillForm()
            End If
        Catch ex As Exception
            m_App.WriteErrorText(1, m_User.UserName, "UserManagement", "Page_Load", ex.ToString)
            lblError.Text = ex.ToString
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
            ddlHierarchy.SelectedValue = "1"

            Dim _HierarchyList2 As New Kernel.HierarchyList(cn, True)
            Dim vwHierarchy2 As DataView = _HierarchyList2.DefaultView
            ddlHierarchyDisplay.DataSource = vwHierarchy2
            ddlHierarchyDisplay.DataValueField = "ID"
            ddlHierarchyDisplay.DataTextField = "Level"
            ddlHierarchyDisplay.DataBind()
            ddlHierarchyDisplay.SelectedIndex = ddlHierarchyDisplay.Items.Count - 1
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
            tdHierarchyDisplay1.Visible = False
            tdHierarchyDisplay2.Visible = False
            td_EmployeeDisplay.Visible = False
            td_EmployeeDisplay1.Visible = False
            td_EmployeeDisplay2.Visible = False
            td_EmployeeDisplay3.Visible = False

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
                trEmployee01.Visible = True
                trEmployee02.Visible = True
                trEmployee03.Visible = True
                trEmployee04.Visible = True
                trEmployee05.Visible = True
                trEmployee06.Visible = True
                trEmployee07.Visible = True
                td_EmployeeDisplay.Visible = True
                td_EmployeeDisplay1.Visible = True
                td_EmployeeDisplay2.Visible = True
                td_EmployeeDisplay3.Visible = True
                tdHierarchyDisplay1.Visible = True
                tdHierarchyDisplay2.Visible = True
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
                dgSearchResult.Columns(8).Visible = False 'Spalte "Test-Zugang" ausblenden
                trTestUser.Visible = False '"Test-Zugang" aus dem Edit-Bereich ausblenden
                dgSearchResult.Columns(11).Visible = False 'Spalte "Passwort läuft nie ab" ausblenden
                dgSearchResult.Columns(7).Visible = False 'Spalte "Customer-Admin" ausblenden
                dgSearchResult.Columns(15).Visible = False 'Spalte "RemoteLoginKey" ausblenden
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
                    dgSearchResult.Columns(6).Visible = False 'Spalte "Organisation" ausblenden
                    trOrganization.Visible = False
                    trOrganizationAdministrator.Visible = False
                End If

                If Not m_User.IsCustomerAdmin Then
                    'Wenn nicht Customer-Admin:
                    lnkOrganizationManagement.Visible = False
                    lnkGroupManagement.Visible = False
                    dgSearchResult.Columns(6).Visible = False 'Spalte "Organisation" ausblenden
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
            trSearchSpacer.Visible = False 'Suchergebnis ausblenden
            Result.Visible = False 'Suchergebnis ausblenden
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
                If .Items.FindByValue(m_User.Groups(0).GroupId.ToString) IsNot Nothing Then .SelectedValue = m_User.Groups(0).GroupId.ToString
                If ddl.ID = "ddlGroups" Then
                    If .Items.FindByValue("-1") IsNot Nothing Then
                        .SelectedValue = "-1"
                    End If
                End If
            Else
                If .Items.Count <> 0 Then
                    If blnAllNone Then
                        If .Items.FindByValue("-1") IsNot Nothing Then
                            .SelectedValue = "-1"
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
                If .Items.FindByValue(m_User.Organization.OrganizationId.ToString) IsNot Nothing Then
                    .SelectedValue = m_User.Organization.OrganizationId.ToString
                End If
            Else
                If blnAllNone Then .SelectedValue = "-1"
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
            .SelectedValue = m_User.Customer.CustomerId.ToString
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
                .SelectedValue = "0"
            Else
                .Enabled = False
                .SelectedValue = m_User.Customer.CustomerId.ToString
            End If

        End With
    End Sub

    Private Sub FillDataGrid(ByVal blnNotApproved As Boolean)
        Dim strSort As String = "UserID"
        If Not ViewState("ResultSort") Is Nothing Then
            strSort = ViewState("ResultSort").ToString
        End If
        FillDataGrid(blnNotApproved, strSort)
    End Sub

    Private Sub FillDataGrid(ByVal blnNotApproved As Boolean, ByVal strSort As String)
        trSearchResult.Visible = True
        Result.Visible = True

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
        dvUser.Sort = strSort

        'prüfung das der gesetzte pageindex größer als die anzahl der ergebnisse ist,
        'ist hier nötig weil bei jedem fillGrid die neuen Selektionsparameter verwendet werden
        'JJU 20081013
        '---------------------------------------------------------
        'War nicht ganz richtig, wenn man 23 User selektiert sind und auf Seite 3 will,
        'kommt man nie auf die letzte Seite. 23 nicht größer 30!!! Sind aber noch 3 User auf Seite 3!!

        If Not dvUser.Count > ((dgSearchResult.PageIndex + 1) * dgSearchResult.PageSize) - 9 Then
            dgSearchResult.PageIndex = 0
        End If
        '---------------------------------------------------------

        With dgSearchResult
            .DataSource = dvUser
            .DataBind()
        End With

        If blnNotApproved = True Then

            Dim Item As GridViewRow

            Dim lnkButton As LinkButton
            Dim strUserText As String

            For Each Item In dgSearchResult.Rows

                If Item.Cells(17).Text = m_User.UserName Then
                    lnkButton = Item.Cells(1).Controls(0)
                    strUserText = lnkButton.Text
                    Item.Cells(1).Controls.Clear()
                    Item.Cells(1).Text = strUserText
                    Item.Cells(1).ForeColor = System.Drawing.Color.Gray
                    lnkButton.Dispose()

                    Item.Cells(13).Controls.Clear()

                End If
            Next
        End If

    End Sub

    ''' <summary>
    ''' Funktion zum Füllen des Dialogs für die Bearbeitung eines Benutzers
    ''' </summary>
    ''' <param name="intUserId">User ID</param>
    ''' <returns>liefert True falls erfolgreich</returns>
    ''' <remarks></remarks>
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
            cbxCustomerAdmin.Checked = _User.IsCustomerAdmin
            cbxFirstLevelAdmin.Checked = _User.FirstLevelAdmin
            lblUrlRemoteLoginKey.Text = _User.UrlRemoteLoginKey
            If _User.Customer.CustomerId > 0 Then
                ddlCustomer.ClearSelection()
                If ddlCustomer.Items.FindByValue(_User.Customer.CustomerId.ToString) IsNot Nothing Then
                    ddlCustomer.SelectedValue = _User.Customer.CustomerId.ToString
                End If
            End If
            cbxOrganizationAdmin.Checked = _User.Organization.OrganizationAdmin
            ddlGroups.ClearSelection()
            If _User.Groups.HasGroups Then
                'If CInt(ddlFilterCustomer.SelectedItem.Value) < 1 Then
                Dim intCustomerID As Integer = _User.Customer.CustomerId
                Dim cn As New SqlClient.SqlConnection(m_User.App.Connectionstring)
                cn.Open()
                Dim dtGroups As New Kernel.GroupList(intCustomerID, cn, m_User.Customer.AccountingArea)
                FillGroup(ddlGroups, False, dtGroups)
                'End If
                _li = ddlGroups.Items.FindByValue(_User.Groups(0).GroupId.ToString)
                If ddlGroups.Items.FindByValue(_User.Groups(0).GroupId.ToString) IsNot Nothing Then
                    ddlGroups.SelectedValue = _User.Groups(0).GroupId.ToString
                End If
            Else
                _li = New ListItem("- keine -", "-1")
                ddlGroups.Items.Add(_li)
                ddlGroups.SelectedValue = "-1"
            End If
            ddlOrganizations.ClearSelection()
            If IsNumeric(_User.Organization.OrganizationId) Then
                'If CInt(ddlFilterCustomer.SelectedItem.Value) < 1 Then
                Dim intCustomerID As Integer = _User.Customer.CustomerId
                Dim cn As New SqlClient.SqlConnection(m_User.App.Connectionstring)
                cn.Open()
                Dim dtOrganizations As New OrganizationList(intCustomerID, cn, m_User.Customer.AccountingArea)
                FillOrganization(ddlOrganizations, False, dtOrganizations)
                'End If
                _li = ddlOrganizations.Items.FindByValue(_User.Organization.OrganizationId.ToString)
                If ddlOrganizations.Items.FindByValue(_User.Organization.OrganizationId.ToString) IsNot Nothing Then
                    ddlOrganizations.SelectedValue = _User.Organization.OrganizationId.ToString
                End If
            Else
                _li = New ListItem("- keine -", "-1")
                ddlOrganizations.Items.Add(_li)
                ddlOrganizations.SelectedValue = "-1"
            End If
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
            ddlTitle.ClearSelection()

            If ddlTitle.Items.FindByValue(_User.Title) IsNot Nothing Then
                ddlTitle.SelectedValue = _User.Title
            End If

            txtFirstName.Text = _User.FirstName
            txtLastName.Text = _User.LastName
            txtStore.Text = _User.Store

            PasswordEditMode((Not _User.Customer.CustomerPasswordRules.DontSendEmail) And (_User.HighestAdminLevel = AdminLevel.None))

            'Mitarbeiterdaten
            If ddlHierarchy.Items.FindByValue(_User.HierarchyID.ToString) IsNot Nothing Then
                ddlHierarchy.SelectedValue = _User.HierarchyID.ToString
            Else
                ddlHierarchy.SelectedValue = "1"
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

            trEmployee07.Visible = False
            If CInt(txtUserID.Text) > -1 Then
                If m_User.HighestAdminLevel > AdminLevel.Customer Then
                    trEmployee07.Visible = True
                End If
            End If

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
            txtDomainUser.Text = GetDomainUser(_User.UserID)

            trUrlRemoteLoginKey.Visible = False
            If (_User.Customer.AllowUrlRemoteLogin And (m_User.HighestAdminLevel > AdminLevel.Customer Or m_User.IsCustomerAdmin)) Then
                ' Wenn Kunde für Remote URL freigeschaltet  UND  User ist Highest Admin oder Customer Admin
                ' ==> dann ist die URL Remote Login Key Gemerierung hier verfügbar:
                trUrlRemoteLoginKey.Visible = True
            End If

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
        cbxAccountIsLockedOut.Checked = False
        cbxApproved.Checked = True ' für ZLD-Benutzer sofort freigeben

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
            If ddlOrganizations.Items.FindByValue(m_User.Organization.OrganizationId.ToString) IsNot Nothing Then
                ddlOrganizations.SelectedValue = m_User.Organization.OrganizationId.ToString
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
        Session("UsernameStart") = Nothing
        Session("LockedOutStart") = Nothing

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

        lbtnDelete.Visible = True
    End Sub

    Private Sub SearchMode(Optional ByVal blnSearchMode As Boolean = True)
        lblNotApprovedMode.Visible = False
        trEditUser.Visible = Not blnSearchMode
        DivSearch.Visible = blnSearchMode
        QueryFooter.Visible = blnSearchMode
        trSearchSpacer.Visible = blnSearchMode
        trSearchResult.Visible = blnSearchMode
        Result.Visible = blnSearchMode
        lbtnSave.Visible = Not blnSearchMode
        lbtnCancel.Visible = Not blnSearchMode
        Input.Visible = Not blnSearchMode
        lbtnNew.Visible = blnSearchMode
        lbtnNotApproved.Visible = blnSearchMode
        lbtnApprove.Visible = False
    End Sub

    Private Sub SearchNotApprovedMode(ByVal search As Boolean, ByVal edit As Boolean)
        If search Or edit Then
            lblNotApprovedMode.Visible = True
        Else
            lblNotApprovedMode.Visible = False
        End If
        If (Not search) Or edit Then
            trEditUser.Visible = True
            Input.Visible = True
        Else
            trEditUser.Visible = False
            Input.Visible = False
        End If
        DivSearch.Visible = (Not edit)
        QueryFooter.Visible = (Not edit)
        trSearchSpacer.Visible = (Not edit)
        trSearchResult.Visible = (Not edit)
        Result.Visible = (Not edit)
        lbtnSave.Visible = False


        Input.Visible = False
        lbtnCancel.Visible = search OrElse edit
        If search AndAlso Not edit Then
            lbtnCancel.Text = " &#149;&nbsp;Zurück<br>zur Suche"
            lbtnCancel0.Visible = True
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
        If blnResetPageIndex Then dgSearchResult.PageIndex = 0
        If blnRefillDataGrid Then FillDataGrid(blnNotApproved)
    End Sub

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

#Region "Domain-User"

    Private Sub SetDomainUser(ByVal intUserId As Integer)
        Dim Connection As New SqlClient.SqlConnection()
        Dim Command As New SqlClient.SqlCommand()

        Try
            Connection.ConnectionString = ConfigurationManager.AppSettings("Connectionstring")

            With Command
                .Connection = Connection
                .CommandType = CommandType.Text
                .CommandText = "SELECT Count(ID)" & _
                                " FROM DomainUser" & _
                                " Where UserID = @UserID"

                With Command.Parameters
                    .AddWithValue("@UserID", intUserId)
                End With
                Dim iCount As Integer = 0
                Connection.Open()
                iCount = Command.ExecuteScalar()
                Connection.Close()

                If iCount = 0 Then
                    .CommandText = "Insert Into DomainUser(UserID, " & _
                                                 "DomainName, " & _
                                                 "UserName) " & _
                                                 "VALUES(@UserID, " & _
                                                 "@DomainName, " & _
                                                 "@UserName);"
                Else
                    .CommandText = "Update DomainUser " & _
                                    "SET UserID=@UserID, " & _
                                    "DomainName=@DomainName, " & _
                                    "UserName=@UserName "
                End If
            End With
            With Command.Parameters
                .AddWithValue("@DomainName", txtDomainUser.Text.Trim.ToUpper)
                .AddWithValue("@UserName", txtUserName.Text)
            End With

            Connection.Open()
            Command.ExecuteNonQuery()
            Connection.Close()
        Catch ex As Exception
            If Connection.State = ConnectionState.Open Then
                Connection.Close()
            End If
            lblError.Text = "Es ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
        End Try

    End Sub

    Private Sub DeleteDomainUser(ByVal intUserId As Integer)
        Dim Connection As New SqlClient.SqlConnection()
        Dim Command As New SqlClient.SqlCommand()

        Try
            Connection.ConnectionString = ConfigurationManager.AppSettings("Connectionstring")

            With Command
                .Connection = Connection
                .CommandType = CommandType.Text

                .CommandText = "Delete " & _
                                " FROM DomainUser" & _
                                " Where UserID = @UserID"

                With .Parameters
                    .AddWithValue("@UserID", intUserId)
                End With

            End With

            Connection.Open()
            Command.ExecuteNonQuery()
            Connection.Close()

        Catch ex As Exception
            If Connection.State = ConnectionState.Open Then
                Connection.Close()
            End If

            lblError.Text = "Es ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
        End Try

    End Sub

    Private Function GetDomainUser(ByVal intUserId As Integer) As String
        Dim Connection As New SqlClient.SqlConnection()
        Dim Command As New SqlClient.SqlCommand()

        Try
            Connection.ConnectionString = ConfigurationManager.AppSettings("Connectionstring")

            With Command
                .Connection = Connection
                .CommandType = CommandType.Text

                .CommandText = "SELECT DomainName" & _
                                " FROM DomainUser" & _
                                " Where UserID = @UserID"

                With .Parameters
                    .AddWithValue("@UserID", intUserId)
                End With

            End With

            Dim DomainUser As String = ""
            Connection.Open()
            DomainUser = Command.ExecuteScalar()
            Connection.Close()
            Return DomainUser

        Catch ex As Exception
            If Connection.State = ConnectionState.Open Then
                Connection.Close()
            End If
            Return ""
        End Try

    End Function

#End Region

    Protected Sub btnUpload_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnUpload.Click
        Try
            If Not (upFile.PostedFile.FileName = String.Empty) Then
                Dim fname As String = upFile.PostedFile.FileName
                If (upFile.PostedFile.ContentLength > CType(System.Configuration.ConfigurationManager.AppSettings("MaxUploadSize"), Integer)) Then
                    lblErrorSave.Text = "Datei '" & Right(fname, fname.Length - fname.LastIndexOf("\") - 1).ToUpper & "' ist zu gross (>300 KB)."
                    Exit Sub
                End If
                '------------------
                If Right(upFile.PostedFile.FileName.ToUpper, 4) <> ".JPG" Then
                    lblErrorSave.Text = "Es können nur Bilddateien im JPG - Format verarbeitet werden."
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
                        lblErrorSave.Text = "Fehler beim Speichern."
                    End If

                    Dim _User As New User(CInt(txtUserID.Text), m_User.App.Connectionstring)
                    _User.SetEmployeePicture(True, m_User.UserName)

                    FillEdit(CInt(txtUserID.Text))
                End If
            End If
        Catch ex As Exception
            lblErrorSave.Text = "Fehler beim Hochladen. (" & ex.ToString & ")"
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
            lblErrorSave.Text = "Fehler beim Löschen. (" & ex.ToString & ")"
        End Try
    End Sub

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
        SetEndASPXAccess(Me)
        HelpProcedures.FixedGridViewCols(dgSearchResult)
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

#Region " Events "

    Private Sub lbtnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lbtnCancel.Click
        Dim editNotApproved As Boolean = False


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

            'Auswahl Gruppen/Organisationen füllen
            Dim cn As New SqlClient.SqlConnection(m_User.App.Connectionstring)
            cn.Open()
            Dim dtGroups As New Kernel.GroupList(intCustomerID, cn, m_User.Customer.AccountingArea)
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

            If Not _customer.CustomerPasswordRules.DontSendEmail Then
                chkNewPasswort.Checked = True
                chkNewPasswort.Enabled = False
            End If
        End If

        trEmployee07.Visible = False
    End Sub

    Private Sub lbtnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lbtnSave.Click
        Dim strPwd As String = String.Empty
        Dim bInitialPswd As Boolean = False

        Try
            Dim _customer As New Customer(CInt(ddlCustomer.SelectedItem.Value), m_User.App.Connectionstring)


            If Not txtPassword.Visible Then 'eMail-Adresse soll nicht Pflichtfeld sein, wenn manuelle Passwort-Eingabe
                If txtMail.Text.Trim(" "c).Length = 0 Then
                    lblMessageSave.Visible = True
                    lblMessageSave.Text = "Bitte geben Sie eine Email-Adresse an."
                    Exit Sub
                Else
                    If (InStr(txtMail.Text, "@") = 0) Or (InStr(txtMail.Text, ".") = 0) Then
                        lblMessageSave.Visible = True
                        lblMessageSave.Text = "Bitte geben Sie eine Email-Adresse im Format ""account@server.de"" an."
                        Exit Sub
                    End If
                End If
            End If
            If Not IsNumeric(txtReadMessageCount.Text) Then
                lblMessageSave.Text = "Bitte geben Sie einen Zahlenwert für die Anzahl der Startmeldungs-Anzeigen ein."
                lblMessageSave.Visible = True
                Exit Sub
            End If
            If ddlTitle.SelectedItem Is Nothing OrElse ddlTitle.SelectedItem.Value = "-" Then
                If Not _customer.NameInputOptional Then
                    lblMessageSave.Visible = True
                    lblMessageSave.Text = "Bitte wählen Sie eine Anrede aus."
                    Exit Sub
                End If
            End If
            If txtFirstName.Text = String.Empty Then
                If Not _customer.NameInputOptional Then
                    lblMessageSave.Visible = True
                    lblMessageSave.Text = "Bitte geben Sie einen Vornamen an."
                    Exit Sub
                End If
            End If
            If txtLastName.Text = String.Empty Then
                If Not _customer.NameInputOptional Then
                    lblMessageSave.Visible = True
                    lblMessageSave.Text = "Bitte geben Sie einen Nachnamen an."
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
                        lblErrorSave.Text = "Bitte wählen Sie einen anderen Namen für den neuen Benutzer!"
                        lblErrorSave.Text &= " <br>(Der Name oder ein Teil davon ist eine gesperrte Zeichenfolge.)"
                        Exit Sub
                    End If
                Next
            End If

            If chkEmployee.Checked Then
                'Der Benutzer soll als verantwortlicher Mitarbeiter verwendbar sein
                '=> Die später anzuzeigenden Daten müssen komplett sein!

                If txtLastName.Text = String.Empty Then
                    lblErrorSave.Visible = True
                    lblErrorSave.Text = "Bitte geben Sie für den Mitarbeiter einen Nachnamen an!"
                    Exit Sub
                End If

                If txtFirstName.Text = String.Empty Then
                    lblErrorSave.Visible = True
                    lblErrorSave.Text = "Bitte geben Sie für den Mitarbeiter einen Vornamen an!"
                    Exit Sub
                End If

                If txtMail.Text = String.Empty Then
                    lblErrorSave.Visible = True
                    lblErrorSave.Text = "Bitte geben Sie für den Mitarbeiter eine Email-Adresse an!"
                    Exit Sub
                End If

                If txtDepartment.Text = String.Empty Then
                    lblErrorSave.Visible = True
                    lblErrorSave.Text = "Bitte geben Sie für den Mitarbeiter eine Abteilung an!"
                    Exit Sub
                End If

                If txtPosition.Text = String.Empty Then
                    lblErrorSave.Visible = True
                    lblErrorSave.Text = "Bitte geben Sie für den Mitarbeiter eine Position an!"
                    Exit Sub
                End If

                If txtTelephone.Text = String.Empty Then
                    lblErrorSave.Visible = True
                    lblErrorSave.Text = "Bitte geben Sie für den Mitarbeiter eine Telefonnummer an!"
                    Exit Sub
                End If
            End If

            Dim _User As User
            If _customer.NameInputOptional And Not chkEmployee.Checked Then
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
                                                  False, _
                                                  txtValidFrom.Text,
                                                  0)
            Else
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
                                                  False, _
                                                  txtValidFrom.Text,
                                                  0)
            End If

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

            Dim strTemp As String = txtUserID.Text

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
                lblErrorSave.Text = "Bitte geben Sie für den Mitarbeiter eine Gruppe an!"
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
                    OrElse txtPassword.Text <> String.Empty Then
                    If _customer.CustomerPasswordRules.DontSendEmail Or Not _User.HighestAdminLevel = AdminLevel.None Or cbxOrganizationAdmin.Checked Then   ' Wenn Passwort nicht per Mail txtFelder abfragen
                        If txtPassword.Text = String.Empty Then
                            lblErrorSave.Text = "Bitte geben Sie für den neuen Benutzer ein Passwort an!"
                            Exit Sub
                        ElseIf txtPassword.Text <> txtConfirmPassword.Text Then
                            lblErrorSave.Text = "Die eingegebenen Passwörter stimmen nicht überein!"
                            Exit Sub
                        End If

                        strPwd = txtPassword.Text
                    Else    ' Sonst nach Kundeneinstellungen ein neues Passwort generieren
                        strPwd = _customer.CustomerPasswordRules.CreateNewPasswort(lblErrorSave.Text)


                        bInitialPswd = True
                    End If
                End If

                ' Wenn Passwortänderung
                If Not (strPwd = String.Empty) Then
                    Dim pword As String = strPwd
                    Dim pwordconfirm As String = strPwd

                    If Not _User.ChangePasswordNew("", pword, pwordconfirm, m_User.UserName, True, bInitialPswd) Then
                        txtUserID.Text = _User.UserID.ToString
                        lblErrorSave.Text = _User.ErrorMessage
                    Else
                        blnSuccess = True
                    End If
                Else
                    blnSuccess = True
                End If

                If txtDomainUser.Text.Trim().Length > 0 Then
                    SetDomainUser(_User.UserID)
                Else
                    DeleteDomainUser(_User.UserID)
                End If
                _User.SetLastLogin(Now)
                _User.Organization.ReAssignUserToOrganization(m_User.UserName, strTemp, _User.UserID, intOrganizationID, cbxOrganizationAdmin.Checked, m_User.App.Connectionstring)

                'EmployeeInfos ggf. speichern
            Else
                lblErrorSave.Text = _User.ErrorMessage
            End If

            If blnSuccess Then
                lblMessageSave.Text = "Die Änderungen wurden gespeichert."
                Search(True, True, , True)
                Dim errorMessage As String = ""

                ' Versandt von neuen Benutzerdaten erst nach Freigabe, daher in lbtnApproved_Click

                ' Ausnahme für Orgaadmins und Kundenadmins, die Benutzer anlegen ++++++++++++++++++
                If txtUserID.Text = "-1" And cbxApproved.Checked And Session("UsernameStart") Is Nothing Then


                    ' Neuanlage Benutzer (ohne Adminrechte) Authentifizierungs-Email versenden
                    If _User.HighestAdminLevel = AdminLevel.None Then
                        ' Wenn Passwort und Username per Mail dann Validierungsprozess
                        If Not _User.Customer.CustomerUsernameRules.DontSendEmail And Not _User.Customer.CustomerPasswordRules.DontSendEmail Then

                            Dim LinkKey As String = ""
                            'Dim pword As String = ""
                            Dim RightKey As String = ""
                            Dim WrongKey As String = ""

                            'Linkschlüssel generieren
                            LinkKey = _User.Customer.CustomerPasswordRules.CreateNewPasswort(lblErrorSave.Text)

                            'Passwort generieren
                            'pword = _User.Customer.CustomerPasswordRules.CreateNewPasswort(lblError.Text)
                            '_User.ChangePasswordNew("", pword, pword, "Freigabeprozess - " + m_User.UserName, True, False)

                            'Erstellt einen Eintrag in der Tabelle für den Freigabe-Workflow
                            InsertIntoWebUserUpload(_User.UserID, strPwd, _User.UserName, LinkKey, RightKey, WrongKey, _User.Customer.LoginLinkID)

                            'Mail versenden
                            If Not _User.SendUsernameMail(errorMessage, _User.Customer.LoginLinkID, RightKey, WrongKey, m_User, False) Then

                                lblErrorSave.Text = errorMessage
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
                            lblErrorSave.Text = errorMessage
                        End If
                    End If

                    ' Benutzername per Mail +++++++++++++++++++++++++
                    ' Wenn vorhandener Benutzer geändert
                    If Not (Session("UsernameStart") Is Nothing) Then
                        ' Username geändert AND Username nicht leer AND User kein Admin
                        If _User.UserName <> CStr(Session("UsernameStart")) And _User.UserName <> String.Empty And _
                            _User.HighestAdminLevel = AdminLevel.None And Not cbxOrganizationAdmin.Checked Then
                            If Not _User.SendUsernameChangedMail(errorMessage, False) Then
                                lblErrorSave.Text = errorMessage
                            End If
                        End If
                        ' Falls Benutzer entsperrt wurde
                        If Not Session("LockedOutStart") Is Nothing Then
                            If _User.Approved And _User.AccountIsLockedOut = False And CBool(Session("LockedOutStart")) = True Then
                                If Not _User.SendUserUnlockMail(errorMessage, m_User, False) Then
                                    lblErrorSave.Text = errorMessage
                                End If
                            End If
                        End If
                    End If

                End If

                Session("UsernameStart") = Nothing
                Session("LockedOutStart") = Nothing

            End If

        Catch ex As Exception
            m_App.WriteErrorText(1, m_User.UserName, "UserManagement", "lbtnSave_Click", ex.ToString)

            lblErrorSave.Text = ex.Message
            If Not ex.InnerException Is Nothing Then
                lblErrorSave.Text &= ": " & ex.InnerException.Message
            End If
        End Try
    End Sub

    Private Sub lbtnDelete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lbtnDelete.Click
        Try
            Dim _User As New User()
            If _User.Delete(CInt(txtUserID.Text), m_User.App.Connectionstring, m_User.UserName) Then
                DeleteDomainUser(_User.UserID)
                lblMessage.Text = "Das Benutzerkonto wurde gelöscht."
                Search(True, True, , True)
            Else
                lblErrorSave.Text = _User.ErrorMessage
            End If
        Catch ex As Exception
            m_App.WriteErrorText(1, m_User.UserName, "UserManagement", "lbtnDelete_Click", ex.ToString)

            lblErrorSave.Text = ex.Message
            If Not ex.InnerException Is Nothing Then
                lblError.Text &= ": " & ex.InnerException.Message
            End If
        End Try

        Session("UsernameStart") = Nothing
        Session("LockedOutStart") = Nothing

    End Sub

    Private Sub ddlFilterCustomer_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ddlFilterCustomer.SelectedIndexChanged
        Dim intCustomerID As Integer = CInt(ddlFilterCustomer.SelectedItem.Value)
        Dim cn As New SqlClient.SqlConnection(m_User.App.Connectionstring)
        cn.Open()
        If intCustomerID > 0 Then
            ddlCustomer.ClearSelection()
            If ddlCustomer.Items.FindByValue(intCustomerID.ToString) IsNot Nothing Then
                ddlCustomer.SelectedValue = intCustomerID.ToString
            End If
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

#End Region

    Private Sub BuildExcel()
        Dim _context As HttpContext = HttpContext.Current
        Dim dvUser As DataView
        Dim tableExport As New DataTable()

        Dim strFileName As String = Format(Now, "yyyyMMdd_HHmmss_") & m_User.UserName & ".xls"
        Dim showExcel As Boolean
        Dim customerList As String

        lnkExcel.Visible = False
        trSearchSpacer.Visible = False
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

            'Nur Master-Admin darf die Remote-Schlüssel sehen
            If Not m_User.HighestAdminLevel = AdminLevel.Master Then
                tableExport.Columns.Remove("URLRemoteLoginKey")
            End If

            Dim objExcelExport As New CKG.Base.Kernel.Excel.ExcelExport()

            Try
                CKG.Base.Kernel.Excel.ExcelExport.WriteExcel(tableExport, ConfigurationManager.AppSettings("ExcelPath") & strFileName)
            Catch ex As Exception
            End Try
            lnkExcel.NavigateUrl = "/PortalZLD/Temp/Excel/" & strFileName
            lnkExcel.Visible = True
            trSearchSpacer.Visible = True
        End If

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
                LinkKey = _User.Customer.CustomerPasswordRules.CreateNewPasswort(lblErrorSave.Text)

                'Passwort generieren
                pword = _User.Customer.CustomerPasswordRules.CreateNewPasswort(lblErrorSave.Text)
                _User.ChangePasswordNew("", pword, pword, "Freigabeprozess - " + m_User.UserName, True, False)

                'Erstellt einen Eintrag in der Tabelle für den Freigabe-Workflow
                InsertIntoWebUserUpload(_User.UserID, pword, _User.UserName, LinkKey, RightKey, WrongKey, _User.Customer.LoginLinkID)

                'Mail versenden
                If Not _User.SendUsernameMail(errorMessage, _User.Customer.LoginLinkID, RightKey, WrongKey, m_User, False) Then
                    lblErrorSave.Text = errorMessage
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
                        pword = _User.Customer.CustomerPasswordRules.CreateNewPasswort(lblErrorSave.Text)
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

    Protected Sub lbtnUrlRemoteLoginKey_Click(sender As Object, e As EventArgs) Handles lbtnUrlRemoteLoginKey.Click
        lblUrlRemoteLoginKey.Text = HttpUtility.UrlEncode(Guid.NewGuid().ToString)
    End Sub

    Protected Sub lbtnUrlRemoteLoginKeyRemove_Click(sender As Object, e As EventArgs) Handles lbtnUrlRemoteLoginKeyRemove.Click
        lblUrlRemoteLoginKey.Text = ""
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

    Private Sub GridNavigation1_PagerChanged(ByVal PageIndex As Integer) Handles GridNavigation1.PagerChanged
        dgSearchResult.PageIndex = PageIndex
        'FillDataGrid(False)
        FillDataGrid(lblNotApprovedMode.Visible)
    End Sub

    Private Sub GridNavigation1_PageSizeChanged() Handles GridNavigation1.PageSizeChanged
        ' FillDataGrid(False)
        FillDataGrid(lblNotApprovedMode.Visible)
    End Sub

    Private Sub dgSearchResult_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgSearchResult.RowCommand

        If e.CommandName = "Edit" Then
            Dim index As Integer = Convert.ToInt32(e.CommandArgument)
            Dim row As GridViewRow = dgSearchResult.Rows(index)
            Dim searchNotApproved As Boolean = False
            If Not ViewState("searchNotApproved") Is Nothing Then
                searchNotApproved = CBool(ViewState("searchNotApproved"))
            End If
            Dim CtrlLabel As Label
            CtrlLabel = row.Cells(0).FindControl("lblUserID")
            If Not searchNotApproved Then
                'normales edit

                EditEditMode(CInt(CtrlLabel.Text))
            Else
                ApproveMode(CInt(CtrlLabel.Text))
            End If
            'dgSearchResult.SelectedIndex = e.Item.ItemIndex
            btnCreatePassword.Enabled = True

        ElseIf e.CommandName = "Del" Then
            Dim index As Integer = Convert.ToInt32(e.CommandArgument)
            Dim row As GridViewRow = dgSearchResult.Rows(index)
            Dim CtrlLabel As Label
            CtrlLabel = row.Cells(0).FindControl("lblUserID")
            EditDeleteMode(CInt(CtrlLabel.Text))
            dgSearchResult.SelectedIndex = row.RowIndex
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

    Private Sub dgSearchResult_RowEditing(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewEditEventArgs) Handles dgSearchResult.RowEditing

    End Sub

    Private Sub dgSearchResult_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles dgSearchResult.Sorting
        Dim strSort As String = e.SortExpression
        If Not ViewState("ResultSort") Is Nothing AndAlso ViewState("ResultSort").ToString = strSort Then
            strSort &= " DESC"
        End If
        ViewState("ResultSort") = strSort
        FillDataGrid(lblNotApprovedMode.Visible, strSort)
    End Sub

    Protected Sub lbtnCancel0_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lbtnCancel0.Click
        Dim editNotApproved As Boolean = False


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
                Search(, True)
            End If
        Else
            Search(True, True, True, True, True)
            BuildExcel()
            SearchNotApprovedMode(True, False)
        End If
    End Sub

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