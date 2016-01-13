Imports CKG.Base.Kernel
Imports CKG.Base.Kernel.Admin
Imports CKG.Base.Kernel.Security
Imports CKG.Base.Kernel.Common.Common
Imports CKG.Services
Imports System.Data.SqlClient
Imports System.Linq
Imports GeneralTools.Models

Partial Public Class UserCategoryRights
    Inherits System.Web.UI.Page

#Region " Membervariables "
    Private m_User As User
    Private m_App As App
    Protected WithEvents GridNavigation1 As GridNavigation
#End Region

    Private m_CategoryName As String = "LOKALISIERUNGS_RECHT"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        m_User = GetUser(Me)

        If (Not Request.QueryString("category") Is Nothing And Not String.IsNullOrEmpty(Request.QueryString("category"))) Then
            m_CategoryName = Request.QueryString("category")
        End If

        lblHead.Text = m_CategoryName.ToLowerFirstUpperWithFragments
        cbNurMitRechten.Text = "Nur User mit " & lblHead.Text
        Dim categoryTemplateField = dgSearchResult.Columns.OfType(Of TemplateField).FirstOrDefault(Function(r) r.HeaderText = "Category")
        If (Not categoryTemplateField Is Nothing) Then
            categoryTemplateField.HeaderText = lblHead.Text
        End If

        FormAuth(Me, m_User)
        GridNavigation1.setGridElment(dgSearchResult)

        Try
            m_App = New App(m_User)

            lblError.Text = ""
            lblMessage.Text = ""

            If Not IsPostBack Then
                FillForm()
            End If
        Catch ex As Exception
            m_App.WriteErrorText(1, m_User.UserName, "UserLocalizationAdmin", "Page_Load", ex.ToString)

            lblError.Text = ex.ToString
        End Try
    End Sub

#Region " Data and Function "
    Private Sub FillForm()
        Dim cn As New SqlClient.SqlConnection(m_User.App.Connectionstring)
        cn.Open()
        EnsureWebUserCategoryRights(cn)
        FillCustomer(cn) 'DropDowns fuer Customer fuellen
        FillGroups(CInt(ddlFilterCustomer.SelectedItem.Value), cn) 'DropDowns fuer Group fuellen
        FillOrganizations(CInt(ddlFilterCustomer.SelectedItem.Value), cn) 'DropDowns fuer Group fuellen

        If m_User.HighestAdminLevel = AdminLevel.None Then
            btnSuche.Enabled = False
            Throw New Exception("Sie verfügen nicht über die nötigen Rechte für die Anwendung.")
        ElseIf m_User.HighestAdminLevel = AdminLevel.FirstLevel AndAlso m_User.Customer.Selfadministration > 0 Then
            'ITA:3768
            lblHead.Text = "Benutzerdaten"
            lblCustomer.Text = m_User.Customer.CustomerName 'Customer des angemeldeten Benutzers
            ddlFilterCustomer.SelectedValue = m_User.Customer.CustomerId
            ddlCustomer.SelectedValue = m_User.Customer.CustomerId
            FillOrganizations(m_User.Customer.CustomerId, cn)
            FillGroups(m_User.Customer.CustomerId, cn)
            LockEdit(True)
        ElseIf m_User.HighestAdminLevel > AdminLevel.Customer Then
            'Wenn DAD-SuperUser:
            lblHead.Text = "Benutzer entsperren / Passwort ändern"
            lblCustomer.Visible = False 'Label mit Customer-Namen ausblenden
            ddlFilterCustomer.Visible = True 'DropDown zur Customer-Auswahl einblenden
        Else
            'Wenn nicht DAD-Super-User:
            lblHead.Text = "Benutzer entsperren"
            lblCustomer.Visible = False 'Label mit Customer-Namen ausblenden
            ddlFilterCustomer.Visible = True 'DropDown zur Customer-Auswahl einblenden          

            'lblCustomer.Text = m_User.Customer.CustomerName 'Customer des angemeldeten Benutzers
            trCustomer.Visible = False 'Customer-Auswahl im Edit-bereich ausblenden
            trTestUser.Visible = False '"Test-Zugang" aus dem Edit-Bereich ausblenden
            trCustomerAdmin1.Visible = False 'Customer-Admin im Editbereich ausblenden
            trCustomerAdmin2.Visible = False 'Customer-Admin im Editbereich ausblenden
            trPwdNeverExpires.Visible = False '"Passwort laeuft nie ab" aus dem Edit-Bereich ausblenden
            trReadMessageCount.Visible = False
            If Not m_User.IsCustomerAdmin Then
                'Wenn nicht Customer-Admin:
                trGroup.Visible = False 'Gruppenauswahl im Edit-Bereich ausblenden
                trOrganization.Visible = False 'Organisationsauswahl im Edit-Bereich ausblenden
                trOrganizationAdministrator.Visible = False 'OrganisationAdmin-Auswahl im Edit-Bereich ausblenden
                If m_User.Groups.Count > 0 Then lblGroup.Text = m_User.Groups(0).GroupName 'Gruppe des angemeldeten Benutzers
            End If
        End If
        Input.Visible = False 'Editbereich ausblenden

        trEditUser.Visible = False 'Editbereich ausblenden

        trSearchResult.Visible = False 'Suchergebnis ausblenden
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
                Dim _li As ListItem = .Items.FindByValue(m_User.Groups(0).GroupId.ToString)
                If Not _li Is Nothing Then _li.Selected = True
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
        FillOrganization(ddlFilterOrganization, False, dtOrganizations)
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
                    If m_User.HighestAdminLevel = AdminLevel.Organization Then
                        .Enabled = False
                    End If
                End If
            Else
                If blnAllNone Then .Items.FindByValue("-1").Selected = True
            End If
        End With
    End Sub

    Private Sub EnsureWebUserCategoryRights(ByVal cn As SqlClient.SqlConnection)
        Dim sc As New SqlCommand(
                                        String.Format(
                                            " INSERT INTO WebUserCategoryRights (UserName,CategoryName,HasRights)" &
                                            " select UserName, '{0}', 0 from WebUser u where AccountIsLockedOut = 0 and Approved = 1 " &
                                            " and not exists(select UserName, CategoryName from WebUserCategoryRights c where c.UserName = u.UserName and c.CategoryName = '{0}')",
                                            m_CategoryName),
                                        cn)
        sc.ExecuteNonQuery()
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
                .Items.FindByValue(m_User.Customer.CustomerId.ToString).Selected = True
                .Enabled = False
            End If

        End With
    End Sub

    Private Sub FillDataGrid()
        Dim strSort As String = "UserID"
        If Not ViewState("ResultSort") Is Nothing Then
            strSort = ViewState("ResultSort").ToString
        End If
        FillDataGrid(strSort)
    End Sub
    Private Sub FillDataGrid(ByVal strSort)
        trSearchResult.Visible = True

        Dim dvUser As DataView
        If Not Session("myUserListView") Is Nothing Then
            dvUser = CType(Session("myUserListView"), DataView)
        Else
            Dim cn As New SqlClient.SqlConnection(m_User.App.Connectionstring)
            cn.Open()

            Dim dtUser As New Kernel.UserListCategoryRights(txtFilterUserName.Text, _
                                                   CInt(ddlFilterCustomer.SelectedItem.Value), _
                                                   CInt(ddlFilterGroup.SelectedItem.Value), _
                                                   CInt(ddlFilterOrganization.SelectedItem.Value), _
                                                   cn, _
                                                   False, _
                                                   -1, _
                                                   m_User.Customer.AccountingArea, _
                                                   m_CategoryName, _
                                                   cbNurMitRechten.Checked)
            dvUser = dtUser.DefaultView
            Session("myUserListView") = dvUser
        End If

        dvUser.Sort = strSort

        With dgSearchResult
            .DataSource = dvUser
            .DataBind()
        End With
    End Sub

    Public Sub cbxHasCategoryRights_CheckedChanged(sender As Object, e As EventArgs)
        Dim cb As CheckBox = DirectCast(sender, CheckBox)
        Dim row As GridViewRow = DirectCast((cb.NamingContainer), GridViewRow)
        Dim cell As DataControlFieldCell = row.Cells(1)
        Dim userName As String = cell.Text

        Dim cn As New SqlClient.SqlConnection(m_User.App.Connectionstring)
        cn.Open()
        Dim sc As New SqlCommand(String.Format(" update WebUserCategoryRights set " &
                                               "HasRights = {2}, CreateUserName='{3}', CreateDate=getdate()" &
                                               " where UserName = '{0}' and CategoryName = '{1}'",
                                                userName, m_CategoryName, IIf(cb.Checked, 1, 0), m_User.UserName),
                                                cn)
        sc.ExecuteNonQuery()
        cn.Close()
    End Sub

    Private Function FillEdit(ByVal intUserId As Integer) As Boolean
        SearchMode(False)
        Dim _User As New User(intUserId, m_User.App.Connectionstring)
        txtUserID.Text = _User.UserID.ToString
        txtUserName.Text = _User.UserName
        txtReference.Text = _User.Reference

        txtReadMessageCount.Text = _User.ReadMessageCount.ToString
        cbxTestUser.Checked = _User.IsTestUser
        chkLoggedOn.Checked = _User.LoggedOn
        cbxCustomerAdmin.Checked = _User.IsCustomerAdmin

        'cbxAccountIsLockedOut.Enabled = _User.AccountIsLockedOut
        cbxApproved.Checked = _User.Approved

        If _User.Customer.CustomerId > 0 Then
            If Not ddlCustomer.SelectedItem Is Nothing Then
                ddlCustomer.SelectedItem.Selected = False
            End If
            Dim _li As ListItem = ddlCustomer.Items.FindByValue(_User.Customer.CustomerId.ToString)
            If Not _li Is Nothing Then
                _li.Selected = True
            End If
        End If
        cbxOrganizationAdmin.Checked = _User.Organization.OrganizationAdmin
        If Not ddlGroups.SelectedItem Is Nothing Then
            ddlGroups.SelectedItem.Selected = False
        End If
        If _User.Groups.HasGroups Then
            If CInt(ddlFilterCustomer.SelectedItem.Value) < 1 Then
                Dim intCustomerID As Integer = _User.Customer.CustomerId
                Dim cn As New SqlClient.SqlConnection(m_User.App.Connectionstring)
                cn.Open()
                Dim dtGroups As New Kernel.GroupList(intCustomerID, cn, m_User.Customer.AccountingArea)
                FillGroup(ddlGroups, False, dtGroups)
            End If
            Dim _li As ListItem = ddlGroups.Items.FindByValue(_User.Groups(0).GroupId.ToString)
            If Not _li Is Nothing Then
                _li.Selected = True
            End If
        Else
            Dim _li As New ListItem("- keine -", "-1")
            _li.Selected = True
            ddlGroups.Items.Add(_li)
        End If
        If Not ddlOrganizations.SelectedItem Is Nothing Then
            ddlOrganizations.SelectedItem.Selected = False
        End If
        If IsNumeric(_User.Organization.OrganizationId) Then
            If CInt(ddlFilterCustomer.SelectedItem.Value) < 1 Then
                Dim intCustomerID As Integer = _User.Customer.CustomerId
                Dim cn As New SqlClient.SqlConnection(m_User.App.Connectionstring)
                cn.Open()
                Dim dtOrganizations As New OrganizationList(intCustomerID, cn, m_User.Customer.AccountingArea)
                FillOrganization(ddlOrganizations, False, dtOrganizations)
            End If
            Dim _li As ListItem = ddlOrganizations.Items.FindByValue(_User.Organization.OrganizationId.ToString)
            If Not _li Is Nothing Then
                _li.Selected = True
            End If
        Else
            Dim _li As New ListItem("- keine -", "-1")
            _li.Selected = True
            ddlOrganizations.Items.Add(_li)
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
        If Not ddlTitle.SelectedItem Is Nothing Then
            ddlTitle.SelectedItem.Selected = False
        End If
        Dim _item As ListItem
        For Each _item In ddlTitle.Items
            If _item.Value = _User.Title Then
                _item.Selected = True
                Exit For
            End If
        Next
        txtFirstName.Text = _User.FirstName
        txtLastName.Text = _User.LastName
        txtStore.Text = _User.Store
        txtValidFrom.Text = _User.ValidFrom
        txtValidTo.Text = _User.ValidTo
        PasswordEditMode(Not _User.Customer.CustomerPasswordRules.DontSendEmail, m_User.HighestAdminLevel > AdminLevel.Organization)

        If _User.IsSuperiorTo(m_User) Then
            lblMessage.Text = "Sie können kein übergeordnetes Benutzerkonto bearbeiten!"
            Return False
        Else
            Return True
        End If
    End Function

    Private Sub PasswordEditMode(ByVal automatic As Boolean, ByVal isAdmin As Boolean)
        trPassword.Visible = False
        trConfirmPassword.Visible = False
        trNewPassword.Visible = False
        If isAdmin Then
            If automatic Then
                trNewPassword.Visible = True
            Else
                trPassword.Visible = True
                trConfirmPassword.Visible = True
            End If
        End If
    End Sub

    Private Sub ClearEdit()
        txtUserID.Text = "-1"
        txtUserName.Text = ""
        txtReference.Text = ""
        cbxTestUser.Checked = False
        cbxCustomerAdmin.Checked = False
        cbxOrganizationAdmin.Checked = False
        If IsNumeric(m_User.Organization.OrganizationId) Then
            Dim _li As ListItem = ddlOrganizations.Items.FindByValue(m_User.Organization.OrganizationId.ToString)
            If Not (_li Is Nothing) Then
                _li.Selected = True
            End If
        Else
            ddlFilterOrganization.SelectedIndex = 0
        End If

        If (Not ddlGroups.Items.Count = 0) And _
            (ddlGroups.Items.Count > ddlFilterGroup.SelectedIndex) And _
            (ddlFilterGroup.SelectedIndex > 0) Then
            ddlGroups.SelectedIndex = ddlFilterGroup.SelectedIndex
            'Else
            '    ddlGroups.SelectedIndex = 0
        End If

        lblLastPwdChange.Text = ""
        cbxPwdNeverExpires.Checked = False
        lblFailedLogins.Text = "0"
        cbxAccountIsLockedOut.Checked = False
        cbxApproved.Checked = False
        chkLoggedOn.Checked = False
        txtPassword.Text = ""
        txtConfirmPassword.Text = ""
        lbtnSave.Visible = True
        txtReadMessageCount.Text = "0"
        ddlTitle.SelectedIndex = 0
        txtFirstName.Text = String.Empty
        txtLastName.Text = String.Empty
        txtStore.Text = String.Empty
        txtValidFrom.Text = String.Empty
        txtValidTo.Text = String.Empty
        LockEdit(False)
    End Sub

    Private Sub LockEdit(ByVal blnLock As Boolean)
        Dim strBackColor As String = "White"
        If blnLock Then
            strBackColor = "LightGray"
        End If
        txtReadMessageCount.Enabled = Not blnLock
        txtReadMessageCount.BackColor = System.Drawing.Color.FromName(strBackColor)
        txtUserName.Enabled = Not blnLock
        txtUserName.BackColor = System.Drawing.Color.FromName(strBackColor)
        txtReference.Enabled = Not blnLock
        txtReference.BackColor = System.Drawing.Color.FromName(strBackColor)
        cbxTestUser.Enabled = Not blnLock
        cbxCustomerAdmin.Enabled = Not blnLock
        ddlCustomer.Enabled = Not blnLock
        ddlCustomer.BackColor = System.Drawing.Color.FromName(strBackColor)
        cbxOrganizationAdmin.Enabled = Not blnLock
        ddlOrganizations.Enabled = Not blnLock
        ddlOrganizations.BackColor = System.Drawing.Color.FromName(strBackColor)
        ddlGroups.Enabled = Not blnLock
        ddlGroups.BackColor = System.Drawing.Color.FromName(strBackColor)
        lblLastPwdChange.Enabled = Not blnLock
        cbxPwdNeverExpires.Enabled = Not blnLock
        lblFailedLogins.Enabled = Not blnLock
        txtPassword.Enabled = Not blnLock
        txtPassword.BackColor = System.Drawing.Color.FromName(strBackColor)
        txtConfirmPassword.Enabled = Not blnLock
        txtConfirmPassword.BackColor = System.Drawing.Color.FromName(strBackColor)
        ddlTitle.Enabled = Not blnLock
        ddlTitle.BackColor = System.Drawing.Color.FromName(strBackColor)
        txtFirstName.Enabled = Not blnLock
        txtFirstName.BackColor = System.Drawing.Color.FromName(strBackColor)
        txtLastName.Enabled = Not blnLock
        txtLastName.BackColor = System.Drawing.Color.FromName(strBackColor)
        txtStore.Enabled = Not blnLock
        txtStore.BackColor = System.Drawing.Color.FromName(strBackColor)
        txtValidFrom.Enabled = Not blnLock
        txtValidFrom.BackColor = System.Drawing.Color.FromName(strBackColor)
        txtValidTo.Enabled = Not blnLock
        txtValidTo.BackColor = System.Drawing.Color.FromName(strBackColor)


        'cbxAccountIsLockedOut.Enabled = True
        'chkLoggedOn.Enabled = True
    End Sub

    Private Sub EditEditMode(ByVal intUserID As Integer)
        If Not FillEdit(intUserID) Then
            LockEdit(True)
            lbtnSave.Enabled = False
        ElseIf m_User.HighestAdminLevel = AdminLevel.FirstLevel AndAlso m_User.Customer.Selfadministration > 0 Then
            lbtnSave.Enabled = False
        Else
            lbtnSave.Enabled = True
        End If
        lbtnCancel.Text = "Verwerfen"
    End Sub

    Private Sub SearchMode(Optional ByVal blnSearchMode As Boolean = True)
        Input.Visible = Not blnSearchMode
        trEditUser.Visible = Not blnSearchMode
        DivSearch1.Visible = blnSearchMode
        trSearchSpacer.Visible = blnSearchMode
        trSearchResult.Visible = blnSearchMode
        Result.Visible = blnSearchMode
        lbtnSave.Visible = Not blnSearchMode
        lbtnCancel.Visible = Not blnSearchMode
    End Sub

    Private Sub Search(Optional ByVal blnRefillDataGrid As Boolean = False, Optional ByVal blnResetSelectedIndex As Boolean = False, Optional ByVal blnResetPageIndex As Boolean = False, Optional ByVal blnClearCache As Boolean = False)
        ClearEdit()
        SearchMode()
        If blnClearCache Then
            Session.Remove("myUserListView")
        End If
        If blnResetSelectedIndex Then dgSearchResult.SelectedIndex = -1
        If blnResetPageIndex Then dgSearchResult.PageIndex = 0
        If blnRefillDataGrid Then FillDataGrid()
    End Sub

    Private Sub Log(ByVal strIdentification As String, ByVal strDescription As String, ByVal tblParameters As DataTable, Optional ByVal strCategory As String = "APP")
        Dim logApp As New Logging.Trace(m_User.App.Connectionstring, m_User.App.SaveLogAccessSAP, m_User.App.LogLevel)

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
            m_App.WriteErrorText(1, m_User.UserName, "UserUnlock", "SetOldLogParameters", ex.ToString)

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
                Dim intCount As Integer
                For intCount = 1 To txtPassword.Text.Length
                    strPw &= "*"
                Next
                .Rows(.Rows.Count - 1)("neues Kennwort") = strPw
                Dim strPw2 As String = ""
                For intCount = 1 To txtConfirmPassword.Text.Length
                    strPw2 &= "*"
                Next
                .Rows(.Rows.Count - 1)("Kennwortbestätigung") = strPw2

                .Rows(.Rows.Count - 1)("letzte Kennwortänderung") = String.Format("{0:dd.MM.yy}", _User.LastPasswordChange)
                .Rows(.Rows.Count - 1)("fehlgeschlagene Anmeldungen") = _User.FailedLogins.ToString
                .Rows(.Rows.Count - 1)("Konto gesperrt") = _User.AccountIsLockedOut
                .Rows(.Rows.Count - 1)("Angemeldet") = _User.LoggedOn
                .Rows(.Rows.Count - 1)("ReadMessageCount") = CInt(txtReadMessageCount.Text)
            End With
            Return tblPar
        Catch ex As Exception
            m_App.WriteErrorText(1, m_User.UserName, "UserUnlock", "SetNewLogParameters", ex.ToString)

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



    Private Sub GridNavigation1_PagerChanged(ByVal PageIndex As Integer) Handles GridNavigation1.PagerChanged
        dgSearchResult.PageIndex = PageIndex
        FillDataGrid()
    End Sub

    Private Sub GridNavigation1_PageSizeChanged() Handles GridNavigation1.PageSizeChanged
        FillDataGrid()
    End Sub

    Private Sub dgSearchResult_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles dgSearchResult.Sorting
        Dim strSort As String = e.SortExpression
        If Not ViewState("ResultSort") Is Nothing AndAlso ViewState("ResultSort").ToString = strSort Then
            strSort &= " DESC"
        End If
        ViewState("ResultSort") = strSort
        FillDataGrid(strSort)
    End Sub

    Private Sub dgSearchResult_RowEditing(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewEditEventArgs) Handles dgSearchResult.RowEditing

    End Sub


    Private Sub dgSearchResult_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgSearchResult.RowCommand

        Select Case e.CommandName
            Case "Edit"
                'Wenn SmallAdmin, dann darf er keine Änderungen außer Freischalten machen...
                txtUserName.Enabled = False
                ddlTitle.Enabled = False
                txtFirstName.Enabled = False
                txtLastName.Enabled = False
                txtStore.Enabled = False
                txtValidFrom.Enabled = False
                txtValidTo.Enabled = False
                cbxFirstLevelAdmin.Enabled = False
                chk_Matrix1.Enabled = False
                chkLoggedOn.Enabled = False
                txtReference.Enabled = False
                cbxTestUser.Enabled = False
                cbxCustomerAdmin.Enabled = False
                ddlOrganizations.Enabled = False
                ddlGroups.Enabled = False
                cbxOrganizationAdmin.Enabled = False
                cbxPwdNeverExpires.Enabled = False
                txtReadMessageCount.Enabled = False
                ddlCustomer.Enabled = False
                Dim index As Integer = Convert.ToInt32(e.CommandArgument)
                Dim row As GridViewRow = dgSearchResult.Rows(index)
                Dim searchNotApproved As Boolean = False
                If Not ViewState("searchNotApproved") Is Nothing Then
                    searchNotApproved = CBool(ViewState("searchNotApproved"))
                End If
                Dim CtrlLabel As Label
                CtrlLabel = row.Cells(0).FindControl("lblUserID")
                EditEditMode(CInt(CtrlLabel.Text))
                cbxAccountIsLockedOut.Enabled = False
                txtPassword.Enabled = False
                txtConfirmPassword.Enabled = False
                chkNewPasswort.Enabled = False
                If m_User.HighestAdminLevel = AdminLevel.Master Then
                    cbxAccountIsLockedOut.Enabled = True

                    txtPassword.Enabled = True
                    txtConfirmPassword.Enabled = True
                    chkNewPasswort.Enabled = True
                ElseIf CInt(lblFailedLogins.Text) > 0 Then
                    cbxAccountIsLockedOut.Enabled = True
                    txtPassword.Enabled = True
                    txtConfirmPassword.Enabled = True
                    chkNewPasswort.Enabled = True
                ElseIf CInt(lblFailedLogins.Text) = 0 Then
                    Dim TempUser As New Security.User(CInt(CtrlLabel.Text))
                    Dim TempHUser As New Security.HistoryUser(TempUser.UserHistoryID, m_App.Connectionstring)
                    If TempHUser.LastChangedBy = "[Admin-Regelprozess]" Then
                        cbxAccountIsLockedOut.Enabled = True
                        txtPassword.Enabled = True
                        txtConfirmPassword.Enabled = True
                        chkNewPasswort.Enabled = True
                    End If
                ElseIf Not cbxAccountIsLockedOut.Checked Then
                    txtPassword.Enabled = True
                    txtConfirmPassword.Enabled = True
                    chkNewPasswort.Enabled = True
                End If

            Case "Del"


        End Select

    End Sub

    Private Sub lbtnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lbtnCancel.Click
        Search(, True)
    End Sub

    Private Sub lbtnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lbtnSave.Click
        Dim tblLogParameter As DataTable
        Dim strPwd As String = String.Empty
        Try
            Dim _customer As New Customer(CInt(ddlCustomer.SelectedItem.Value), m_User.App.Connectionstring)

            Dim _User As User
            If _customer.NameInputOptional Then
                _User = New User(CInt(txtUserID.Text), _
                                                  txtUserName.Text, _
                                                  txtReference.Text, _
                                                  "", _
                                                  "", _
                                                  False, _
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
                                                  txtStore.Text, _
                                                  chk_Matrix1.Checked, _
                                                  txtValidFrom.Text, _
                                                  txtValidTo.Text)
            Else
                _User = New User(CInt(txtUserID.Text), _
                                                  txtUserName.Text, _
                                                  txtReference.Text, _
                                                  "", _
                                                  "", _
                                                  False, _
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
            End If
            If chkNewPasswort.Checked = True _
              OrElse txtPassword.Text <> String.Empty Then
                If Not _customer.CustomerPasswordRules.DontSendEmail Then
                    strPwd = _customer.CustomerPasswordRules.CreateNewPasswort(lblError.Text)
                    txtPassword.Text = strPwd
                    txtConfirmPassword.Text = strPwd
                    _User.GetEmail(CInt(txtUserID.Text), True)
                Else
                    strPwd = txtPassword.Text
                End If
            End If

            Dim strLogMsg As String = "User ändern"
            tblLogParameter = SetOldLogParameters(CInt(txtUserID.Text))

            Dim intGroupID As Integer

            If Not ddlGroups.Items.Count = 0 Then
                intGroupID = CInt(ddlGroups.SelectedItem.Value)
            Else
                intGroupID = 0
            End If

            Dim blnSuccess As Boolean = False
            Dim pword As String = ""
            Dim pwordconfirm As String = ""

            If _User.Save() Then
                If Not (strPwd = String.Empty) Then
                    If _User.Customer.CustomerPasswordRules.DontSendEmail Then
                        pword = txtPassword.Text
                        pwordconfirm = txtConfirmPassword.Text
                    Else
                        pword = strPwd
                        pwordconfirm = strPwd
                    End If

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
            Else
                lblError.Text = _User.ErrorMessage
            End If
            tblLogParameter = SetNewLogParameters(_User)
            Log(_User.UserID.ToString, strLogMsg, tblLogParameter)

            If blnSuccess Then
                lblMessage.Text = "Die Änderungen wurden gespeichert."
                Search(True, True, , True)

                If (pword <> String.Empty) Then 'Nur bei Passwortänderung
                    Dim errorMessage As String = ""
                    _User.SendPasswordResetMail(errorMessage, CKG.Base.Kernel.Security.User.PasswordMailMode.Neu)
                    If Not String.IsNullOrEmpty(errorMessage) Then lblError.Text = errorMessage
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

    Private Sub ddlFilterCustomer_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ddlFilterCustomer.SelectedIndexChanged
        Dim intCustomerID As Integer = CInt(ddlFilterCustomer.SelectedItem.Value)
        Dim cn As New SqlClient.SqlConnection(m_User.App.Connectionstring)
        cn.Open()
        If intCustomerID > 0 Then
            ddlCustomer.SelectedItem.Selected = False
            ddlCustomer.Items.FindByValue(intCustomerID.ToString).Selected = True
            FillGroups(intCustomerID, cn)
            FillOrganizations(intCustomerID, cn)
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
        Dim dtGroups As New Kernel.GroupList(intCustomerID, cn, m_User.Customer.AccountingArea)
        FillGroup(ddlGroups, False, dtGroups)
        Dim dtOrganizations As New OrganizationList(intCustomerID, cn, m_User.Customer.AccountingArea)
        FillOrganization(ddlFilterOrganization, False, dtOrganizations)
    End Sub

    Private Sub btnSuche_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSuche.Click
        Search(True, True, True, True)
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
            Dim cmd As SqlClient.SqlCommand = New SqlClient.SqlCommand("SELECT LastChangedBy FROM AdminHistory_User " & _
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
End Class