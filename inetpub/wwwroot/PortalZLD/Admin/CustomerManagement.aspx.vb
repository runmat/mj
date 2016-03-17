Imports CKG.Base.Kernel.Admin
Imports CKG.Base.Kernel.Security
Imports CKG.Base.Kernel.Common.Common
Imports CKG.Base.Business
Imports CKG.Base.Business.HelpProcedures
Imports CKG.Base.Common

Partial Public Class CustomerManagement
    Inherits Web.UI.Page

#Region " Membervariables "

    Private m_User As User
    Private m_App As App
    Protected WithEvents GridNavigation1 As Global.CKG.PortalZLD.GridNavigation

#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        m_User = GetUser(Me)
        lblHead.Text = "Kundenverwaltung"
        AdminAuth(Me, m_User, AdminLevel.Master)
        GridNavigation1.setGridElment(dgSearchResult)
        Try
            m_App = New App(m_User)

            If Not IsPostBack Then
                lblError.Text = ""
                txtFilterCustomerName.Focus()
                LoadLoginLinks()
                FillForm()
                Tabs.ActiveTabIndex = 0
            End If
        Catch ex As Exception
            m_App.WriteErrorText(1, m_User.UserName, "CustomerManagement", "Page_Load", ex.ToString)

            lblError.Text = ex.ToString
            lblError.Visible = True
        End Try
    End Sub

#Region " Data and Function "

    Private Sub FillForm()
        '***********************************************
        'Ausblenden, da noch nicht fertig implementiert*
        '***********************************************
        'trPwdHistoryNEntries.Visible = False          '*
        '***********************************************

        trSearchResult.Visible = False

        If m_User.HighestAdminLevel = AdminLevel.Master Then
            'wenn SuperUser und übergeordnete Firma
            If m_User.Customer.AccountingArea = -1 Then
                lnkAppManagement.Visible = True
            End If
        End If

        If Not m_User.HighestAdminLevel = AdminLevel.Master Then

            trMaster.Visible = False
            Tabs.Tabs(2).Visible = False
            Benutzer.Visible = False
            CustomerAdminMode()
        End If
    End Sub

    Private Sub FillDataGrid()
        Dim strSort As String = "CustomerID"
        If Not ViewState("ResultSort") Is Nothing Then
            strSort = ViewState("ResultSort").ToString
        End If
        FillDataGrid(strSort)
    End Sub

    Private Sub FillDataGrid(ByVal strSort As String)
        trSearchResult.Visible = True

        Dim dvCustomer As DataView

        'If Not m_context.Cache("myCustomerListView") Is Nothing Then
        '    dvCustomer = CType(m_context.Cache("myCustomerListView"), DataView)
        If Not Session("myCustomerListView") Is Nothing Then
            dvCustomer = CType(Session("myCustomerListView"), DataView)
        Else
            Dim dtCustomer As Kernel.CustomerList
            Dim cn As New SqlClient.SqlConnection(m_User.App.Connectionstring)
            Try
                cn.Open()

                dtCustomer = New Kernel.CustomerList( _
                                    txtFilterCustomerName.Text, _
                                    m_User.Customer.AccountingArea, _
                                    cn)

                dvCustomer = dtCustomer.DefaultView
                'm_context.Cache.Insert("myCustomerListView", dvCustomer, Nothing, DateTime.Now.AddMinutes(20), TimeSpan.Zero) 
                Session("myCustomerListView") = dvCustomer
            Finally
                If cn.State <> ConnectionState.Closed Then
                    cn.Close()
                End If
            End Try
        End If
        dvCustomer.Sort = strSort
        Try
            With dgSearchResult
                .DataSource = dvCustomer
                .DataBind()
            End With
        Catch ex As Exception
            m_App.WriteErrorText(1, m_User.UserName, "CustomerManagement", "FillDataGrid", ex.ToString)

        End Try


    End Sub

    Private Function FillEdit(ByVal intCustomerId As Integer) As Boolean
        SearchMode(False)
        Dim cn As New SqlClient.SqlConnection(m_User.App.Connectionstring)
        Try
            cn.Open()
            Dim _Customer As New Customer(intCustomerId, cn)
            txtCustomerID.Text = _Customer.CustomerId.ToString
            txtCustomerName.Text = _Customer.CustomerName
            txtKUNNR.Text = _Customer.KUNNR
            txtDocuPath.Text = _Customer.DocuPath
            cbxMaster.Checked = _Customer.IsMaster
            chkShowDistrikte.Checked = _Customer.ShowDistrikte
            chkAllowMultipleLogin.Checked = _Customer.AllowMultipleLogin
            chkAllowUrlRemoteLogin.Checked = _Customer.AllowUrlRemoteLogin
            chkShowOrganization.Checked = _Customer.ShowOrganization
            cbxOrgAdminRestrictToCustomerGroup.Checked = _Customer.OrgAdminRestrictToCustomerGroup
            txtMaxUser.Text = _Customer.MaxUser.ToString
            chkTeamviewer.Checked = _Customer.ShowsTeamViewer
            ddlPortalLink.SelectedValue = _Customer.LoginLinkID

            'fillAccountingArea
            FillAccountingArea(intCustomerId)

            'LoginRegeln
            txtLockedAfterNLogins.Text = _Customer.CustomerLoginRules.LockedAfterNLogins.ToString
            txtNewPwdAfterNDays.Text = _Customer.CustomerLoginRules.NewPasswordAfterNDays.ToString
            'Passwortregeln
            txtPwdLength.Text = _Customer.CustomerPasswordRules.Length.ToString
            txtPwdNNumeric.Text = _Customer.CustomerPasswordRules.Numeric.ToString
            txtNCapitalLetter.Text = _Customer.CustomerPasswordRules.CapitalLetters.ToString
            txtNSpecialCharacter.Text = _Customer.CustomerPasswordRules.SpecialCharacter.ToString
            txtPwdHistoryNEntries.Text = _Customer.CustomerPasswordRules.PasswordHistoryEntries.ToString
            'Kontaktdaten
            If Not _Customer.CustomerContact Is Nothing Then
                txtCName.Text = _Customer.CustomerContact.Name
                'txtCAddress.Text = _Customer.CustomerContact.Address
                EditCAddress.Content = TranslateHTML(_Customer.CustomerContact.Address, TranslationDirection.ReadHTML)
                txtCMailDisplay.Text = _Customer.CustomerContact.MailDisplay
                txtCMail.Text = _Customer.CustomerContact.Mail
                txtCWebDisplay.Text = _Customer.CustomerContact.WebDisplay
                txtCWeb.Text = _Customer.CustomerContact.Web
                txtKundepostfach.Text = _Customer.CustomerContact.Kundenpostfach.Trim
                txtKundenhotline.Text = _Customer.CustomerContact.Kundenhotline.Trim
                txtKundenfax.Text = _Customer.CustomerContact.Kundenfax.Trim
            End If
            'Anwendungen
            FillAssigned(_Customer.CustomerId, cn)
            FillUnAssigned(_Customer.CustomerId, cn)
            'Style
            txtLogoPath.Text = _Customer.CustomerStyle.LogoPath.ToString
            txtCssPath.Text = _Customer.CustomerStyle.CssPath.ToString

            'Zweites Logo.
            txtLogoPath2.Text = _Customer.LogoPath2.ToString

            'IP-Adress-Verwaltung
            chkIpRestriction.Checked = _Customer.IpRestriction
            txtIpStandardUser.Text = _Customer.IpStandardUser

            cbxPwdDontSendEmail.Checked = _Customer.CustomerPasswordRules.DontSendEmail
            cbxUsernameSendEmail.Checked = _Customer.CustomerUsernameRules.DontSendEmail

            If cbxPwdDontSendEmail.Checked Then
                cbxForcePasswordQuestion.Enabled = False
                cbxForcePasswordQuestion.Checked = False
            Else
                cbxForcePasswordQuestion.Enabled = True
                cbxForcePasswordQuestion.Checked = _Customer.ForcePasswordQuestion
            End If

            cbxNameInputOptional.Checked = _Customer.CustomerPasswordRules.NameInputOptional

            'Benutzer und Organisation
            txtUserLockTime.Text = _Customer.DaysUntilLock
            txtUserDeleteTime.Text = _Customer.DaysUntilDelete

            If txtCustomerID.Text = "-1" Then
                Tabs.Tabs(3).Visible = False
            Else
                Tabs.Tabs(3).Visible = True
                FillIpAddresses(_Customer)
            End If
            Return True
        Finally
            If cn.State <> ConnectionState.Closed Then
                cn.Close()
            End If
        End Try
    End Function

    Private Function GetArchivAssignedView(ByVal intCustomerID As Integer, ByVal cn As SqlClient.SqlConnection) As DataView
        'Als Extra-Funktion ausgelagert, da beim Speichern evtl.
        'benoetigt, wenn Cache leer ist (in jenem Fall soll die
        'List kein DataBind ausfuehren).
        Dim _ArchivAssigned As New ArchivList(intCustomerID, cn)
        _ArchivAssigned.GetAssigned()
        _ArchivAssigned.DefaultView.Sort = "EasyArchivName"
        'Cache wird befuellt, um spaeter beim Speichern darauf
        'zu zugreifen.
        'm_context.Cache.Insert("myCustomerArchivAssigned", _ArchivAssigned.DefaultView, Nothing, DateTime.Now.AddMinutes(20), TimeSpan.Zero)
        Session("myCustomerArchivAssigned") = _ArchivAssigned.DefaultView
        Return _ArchivAssigned.DefaultView
    End Function

    Private Function GetAppAssignedView(ByVal intCustomerID As Integer, ByVal cn As SqlClient.SqlConnection) As DataView
        'Als Extra-Funktion ausgelagert, da beim Speichern evtl.
        'benoetigt, wenn Cache leer ist (in jenem Fall soll die
        'List kein DataBind ausfuehren).
        Dim _AppAssigned As New ApplicationList(intCustomerID, cn)
        _AppAssigned.GetAssigned()
        _AppAssigned.DefaultView.Sort = "AppFriendlyName"
        'Cache wird befuellt, um spaeter beim Speichern darauf
        'zu zugreifen.
        'm_context.Cache.Insert("myCustomerAppAssigned", _AppAssigned.DefaultView, Nothing, DateTime.Now.AddMinutes(20), TimeSpan.Zero)
        Session("myCustomerAppAssigned") = _AppAssigned.DefaultView
        Return _AppAssigned.DefaultView
    End Function

    Private Sub FillIpAddresses(ByVal mCust As Customer)
        If mCust.IpAddresses.Rows.Count = 0 Then
            Repeater1.Visible = False
        Else
            Repeater1.Visible = True
            Repeater1.DataSource = mCust.IpAddresses
            Repeater1.DataBind()
        End If
    End Sub

    Private Sub FillAssigned(ByVal intCustomerID As Integer, ByVal cn As SqlClient.SqlConnection)
        Dim row As DataRow

        Dim dvAppAssigned As DataView = GetAppAssignedView(intCustomerID, cn)

        For Each row In dvAppAssigned.Table.Rows
            row("AppFriendlyName") = row("AppFriendlyName").ToString.ToUpper & " || " & row("AppURL").ToString
        Next

        lstAppAssigned.DataSource = dvAppAssigned
        lstAppAssigned.DataTextField = "AppFriendlyName"
        lstAppAssigned.DataValueField = "AppID"
        lstAppAssigned.DataBind()

        Dim dvArchivAssigned As DataView = GetArchivAssignedView(intCustomerID, cn)

        For Each row In dvArchivAssigned.Table.Rows
            row("EasyArchivName") = row("EasyArchivName").ToString.ToUpper & " || Lagerort-Name: " & row("EasyLagerortName").ToString & " || QueryIndex-Name: " & row("EasyQueryIndexName").ToString & " || Titel: " & row("EasyTitleName").ToString
        Next

        lstArchivAssigned.DataSource = dvArchivAssigned
        lstArchivAssigned.DataTextField = "EasyArchivName"
        lstArchivAssigned.DataValueField = "ArchivID"
        lstArchivAssigned.DataBind()
    End Sub

    Private Sub FillAccountingArea(ByVal intCustomerId As Int32, Optional ByVal neuanlage As Boolean = False)
        'AccountingArea
        Dim cn As New SqlClient.SqlConnection(m_User.App.Connectionstring)
        Try
            cn.Open()
            Dim _Customer As New Customer(intCustomerId, cn)
            Dim _AccountingAreaList As Kernel.AccountingAreaList
            _AccountingAreaList = New Kernel.AccountingAreaList(cn)
            Dim vwAccountingAreaList As DataView = _AccountingAreaList.DefaultView
            vwAccountingAreaList.Sort = "Area"
            ddlAccountingArea.DataSource = vwAccountingAreaList
            ddlAccountingArea.DataValueField = "Area"
            ddlAccountingArea.DataTextField = "Description"
            ddlAccountingArea.DataBind()
            ddlAccountingArea.ClearSelection()




            If m_User.Customer.AccountingArea = -1 Then
                'User gehört der Übergeordneten Firma an, Grundlegend BK änderbar
                ddlAccountingArea.Enabled = True
            Else
                'kein Übergeordneter user, Grundlegend BK nicht änderbar
                ddlAccountingArea.Enabled = False
            End If


            If Not ddlAccountingArea.Items.FindByValue(_Customer.AccountingArea.ToString) Is Nothing Then
                ddlAccountingArea.Items.FindByValue(_Customer.AccountingArea.ToString).Selected = True
            Else
                If _Customer.AccountingArea = -1 Then 'es wurde schon eine Vorselektion der Kunden nach BK vorgenommen
                    'wenn firma 1 aufgerufen wird, soll der buchungskreis nicht änderbar sein,
                    'wenn aber ein User der Firma 1 eine Neuanlage tätigt, soll der Buchungskreis auswählbar sein
                    'daher optionaler Parameter "Neuanlage", außerdem soll keine Übergeordnete Firma angelegt werden
                    If neuanlage Then
                        ddlAccountingArea.Enabled = True
                    Else
                        Dim newItem As New Web.UI.WebControls.ListItem("Übergeordnet", "-1")
                        ddlAccountingArea.Items.Add(newItem)
                        ddlAccountingArea.Items.FindByValue("-1").Selected = True
                        ddlAccountingArea.Enabled = False

                    End If
                Else
                    'der Buchungskreis wurde nicht gefunden ist aber auch keine Übergeordnete Firma mit -1? gibts nicht
                    Throw New Exception("Der Buchungskreis: " & _Customer.AccountingArea & " ist nicht bekannt!")
                End If
            End If
        Catch ex As Exception
            lblError.Text = "Fehler beim Befüllen der Accountingareas: " & ex.Message
        End Try

    End Sub

    Private Sub FillUnAssigned(ByVal intCustomerID As Integer, ByVal cn As SqlClient.SqlConnection)
        Dim row As DataRow

        Dim AppUnAssigned As New ApplicationList(intCustomerID, cn)
        AppUnAssigned.GetUnassigned()
        AppUnAssigned.DefaultView.Sort = "AppFriendlyName"

        For Each row In AppUnAssigned.Rows
            row("AppFriendlyName") = row("AppFriendlyName").ToString.ToUpper & " || " & row("AppURL").ToString
        Next

        lstAppUnAssigned.DataSource = AppUnAssigned.DefaultView
        lstAppUnAssigned.DataTextField = "AppFriendlyName"
        lstAppUnAssigned.DataValueField = "AppID"
        lstAppUnAssigned.DataBind()

        Dim ArchivUnAssigned As New ArchivList(intCustomerID, cn)
        ArchivUnAssigned.GetUnassigned()
        ArchivUnAssigned.DefaultView.Sort = "EasyArchivName"

        For Each row In ArchivUnAssigned.Rows
            row("EasyArchivName") = row("EasyArchivName").ToString.ToUpper & " || Lagerort-Name: " & row("EasyLagerortName").ToString & " || QueryIndex-Name: " & row("EasyQueryIndexName").ToString & " || Titel: " & row("EasyTitleName").ToString
        Next

        lstArchivUnAssigned.DataSource = ArchivUnAssigned.DefaultView
        lstArchivUnAssigned.DataTextField = "EasyArchivName"
        lstArchivUnAssigned.DataValueField = "ArchivID"
        lstArchivUnAssigned.DataBind()
    End Sub

    Private Sub ClearEdit()
        txtCustomerID.Text = "-1"
        txtCustomerName.Text = ""
        txtKUNNR.Text = "0"
        txtDocuPath.Text = ""
        cbxMaster.Checked = False
        chkAllowMultipleLogin.Checked = True
        chkAllowUrlRemoteLogin.Checked = False
        chkShowOrganization.Checked = False
        cbxOrgAdminRestrictToCustomerGroup.Checked = False
        chkTeamviewer.Checked = False
        'LoginRegeln
        txtLockedAfterNLogins.Text = "3"
        txtNewPwdAfterNDays.Text = "60"
        'Passwortregeln
        txtPwdLength.Text = "8"
        txtPwdNNumeric.Text = "1"
        txtNCapitalLetter.Text = "1"
        txtNSpecialCharacter.Text = "1"
        txtPwdHistoryNEntries.Text = "6"
        cbxPwdDontSendEmail.Checked = True
        cbxUsernameSendEmail.Checked = True
        cbxForcePasswordQuestion.Checked = False
        cbxForcePasswordQuestion.Enabled = False
        cbxNameInputOptional.Checked = True
        'Anwendungen
        lstAppAssigned.Items.Clear()
        lstAppUnAssigned.Items.Clear()
        'Kontaktdaten
        txtCName.Text = ""
        'txtCAddress.Text = ""
        EditCAddress.Content = ""
        txtCMailDisplay.Text = ""
        txtCMail.Text = ""
        txtCWebDisplay.Text = ""
        txtCWeb.Text = ""
        txtKundepostfach.Text = ""
        txtKundenhotline.Text = ""
        txtKundenfax.Text = ""

        'Style
        txtLogoPath.Text = "../Images/Logo.gif"
        '§§§ JVE 18.09.2006: Logo2
        txtLogoPath2.Text = "../Images/Logo.gif"
        '------------------------
        txtCssPath.Text = "Styles.css"
        'Buttons
        lbtnSave.Visible = True
        lbtnDelete.Visible = False
        LockEdit(False)
    End Sub

    Private Sub LockEdit(ByVal blnLock As Boolean)
        Dim strBackColor As String = "White"
        If blnLock Then
            strBackColor = "LightGray"
        End If
        txtCustomerID.Enabled = Not blnLock
        txtCustomerID.BackColor = System.Drawing.Color.FromName(strBackColor)
        txtCustomerName.Enabled = Not blnLock
        txtCustomerName.BackColor = System.Drawing.Color.FromName(strBackColor)
        If m_User.HighestAdminLevel < AdminLevel.Master Then
            txtKUNNR.Enabled = False
            txtKUNNR.CssClass = "InfoBoxFlat"
        Else
            txtKUNNR.Enabled = Not blnLock
            txtKUNNR.BackColor = System.Drawing.Color.FromName(strBackColor)
        End If
        cbxMaster.Enabled = Not blnLock
        chkKundenSperre.Enabled = Not blnLock
        chkTeamviewer.Enabled = False
        ddlAccountingArea.Enabled = Not blnLock
        chkAllowMultipleLogin.Enabled = Not blnLock
        chkAllowUrlRemoteLogin.Enabled = Not blnLock
        txtDocuPath.Enabled = Not blnLock
        txtDocuPath.BackColor = System.Drawing.Color.FromName(strBackColor)
        '.cbxMaster.BackColor = System.Drawing.Color.FromName(strBackColor)
        'LoginRegeln
        txtLockedAfterNLogins.Enabled = Not blnLock
        txtLockedAfterNLogins.BackColor = System.Drawing.Color.FromName(strBackColor)
        txtNewPwdAfterNDays.Enabled = Not blnLock
        txtNewPwdAfterNDays.BackColor = System.Drawing.Color.FromName(strBackColor)
        'Passwortregeln
        txtPwdLength.Enabled = Not blnLock
        txtPwdLength.BackColor = System.Drawing.Color.FromName(strBackColor)
        txtPwdNNumeric.Enabled = Not blnLock
        txtPwdNNumeric.BackColor = System.Drawing.Color.FromName(strBackColor)
        txtNCapitalLetter.Enabled = Not blnLock
        txtNCapitalLetter.BackColor = System.Drawing.Color.FromName(strBackColor)
        txtNSpecialCharacter.Enabled = Not blnLock
        txtNSpecialCharacter.BackColor = System.Drawing.Color.FromName(strBackColor)
        txtPwdHistoryNEntries.Enabled = Not blnLock
        txtPwdHistoryNEntries.BackColor = System.Drawing.Color.FromName(strBackColor)
        cbxPwdDontSendEmail.Enabled = Not blnLock
        cbxUsernameSendEmail.Enabled = Not blnLock
        cbxNameInputOptional.Enabled = Not blnLock

        If cbxPwdDontSendEmail.Checked Then
            cbxForcePasswordQuestion.Enabled = False
            cbxForcePasswordQuestion.Checked = False
        Else
            cbxForcePasswordQuestion.Enabled = Not blnLock
        End If

        'IP-Adressen
        chkIpRestriction.Enabled = Not blnLock
        txtIpStandardUser.Enabled = Not blnLock
        txtIpStandardUser.BackColor = System.Drawing.Color.FromName(strBackColor)
        txtIpAddress.Enabled = Not blnLock
        txtIpAddress.BackColor = System.Drawing.Color.FromName(strBackColor)

        'Anwendungen
        lstAppAssigned.Enabled = Not blnLock
        lstAppAssigned.BackColor = System.Drawing.Color.FromName(strBackColor)
        lstAppUnAssigned.Enabled = Not blnLock
        lstAppUnAssigned.BackColor = System.Drawing.Color.FromName(strBackColor)
        'Archive
        lstArchivAssigned.Enabled = Not blnLock
        lstArchivAssigned.BackColor = System.Drawing.Color.FromName(strBackColor)
        lstArchivUnAssigned.Enabled = Not blnLock
        lstArchivUnAssigned.BackColor = System.Drawing.Color.FromName(strBackColor)

        'Kontaktdaten
        txtCName.Enabled = Not blnLock
        txtCName.BackColor = System.Drawing.Color.FromName(strBackColor)
        'txtCAddress.Enabled = Not blnLock
        'txtCAddress.BackColor = System.Drawing.Color.FromName(strBackColor)
        EditCAddress.Enabled = Not blnLock
        EditCAddress.BackColor = System.Drawing.Color.FromName(strBackColor)
        txtCMailDisplay.Enabled = Not blnLock
        txtCMailDisplay.BackColor = System.Drawing.Color.FromName(strBackColor)
        txtCMail.Enabled = Not blnLock
        txtCMail.BackColor = System.Drawing.Color.FromName(strBackColor)
        txtCWebDisplay.Enabled = Not blnLock
        txtCWebDisplay.BackColor = System.Drawing.Color.FromName(strBackColor)
        txtCWeb.Enabled = Not blnLock
        txtCWeb.BackColor = System.Drawing.Color.FromName(strBackColor)
        'Dauerhaft deaktiviert da nicht verwendet
        txtKundepostfach.Enabled = False
        txtKundepostfach.BackColor = System.Drawing.Color.FromName("LightGray")
        txtKundenhotline.Enabled = False
        txtKundenhotline.BackColor = System.Drawing.Color.FromName("LightGray")
        txtKundenfax.Enabled = False
        txtKundenfax.BackColor = System.Drawing.Color.FromName("LightGray")

        'Style
        txtLogoPath.Enabled = Not blnLock
        txtLogoPath.BackColor = System.Drawing.Color.FromName(strBackColor)

        '§§§ JVE 18.09.2006: Logo2
        txtLogoPath2.Enabled = Not blnLock
        txtLogoPath2.BackColor = System.Drawing.Color.FromName(strBackColor)
        '-------------------------

        txtCssPath.Enabled = Not blnLock
        txtCssPath.BackColor = System.Drawing.Color.FromName(strBackColor)
        'ShowOrganization
        chkShowOrganization.Enabled = Not blnLock
        cbxOrgAdminRestrictToCustomerGroup.Enabled = Not blnLock
        'MaxUser
        txtMaxUser.Enabled = Not blnLock
        txtMaxUser.BackColor = System.Drawing.Color.FromName(strBackColor)
        'Buttons
        btnAssign.Enabled = Not blnLock
        btnUnAssign.Enabled = Not blnLock
        'Benutzer und Organisation
        txtUserLockTime.Enabled = Not blnLock
        txtUserLockTime.BackColor = System.Drawing.Color.FromName(strBackColor)
        txtUserDeleteTime.Enabled = Not blnLock
        txtUserDeleteTime.BackColor = System.Drawing.Color.FromName(strBackColor)

        ddlPortalLink.Enabled = Not blnLock
    End Sub

    Private Sub CustomerAdminMode()
        SearchMode(False)
        Tabs.Tabs(4).Visible = False

        'If m_User.Groups.Count > 0 Then
        If m_User.IsCustomerAdmin Then
            LockEdit(False)
            EditEditMode(m_User.Customer.CustomerId)
        End If
        'End If
    End Sub

    Private Sub ConfirmMode(ByVal confirmOn As Boolean)
        trConfirm.Visible = confirmOn
        lbtnConfirm.Visible = confirmOn
        LockEdit(confirmOn)
        lbtnSave.Enabled = Not confirmOn
        If confirmOn Then
            lbtnCancel.Text = " &#149;&nbsp;Ändern"
        Else
            lbtnCancel.Text = " &#149;&nbsp;Abbrechen"
        End If
    End Sub

    Private Sub EditEditMode(ByVal intCustomerId As Integer)
        trConfirm.Visible = False
        lbtnConfirm.Visible = False
        If Not FillEdit(intCustomerId) Then
            LockEdit(True)
            lbtnSave.Enabled = False
        Else
            lbtnSave.Enabled = True
        End If
        lbtnCancel.Text = "Verwerfen&nbsp;&#187;"
    End Sub

    Private Sub EditDeleteMode(ByVal intGroupId As Integer)
        trConfirm.Visible = False
        lbtnConfirm.Visible = False
        If Not FillEdit(intGroupId) Then
            lbtnDelete.Enabled = False
        Else
            lblMessage.Text = "Möchten Sie den Kunden wirklich löschen?"
            lbtnDelete.Enabled = True
        End If
        LockEdit(True)
        lbtnCancel.Text = "Abbrechen&nbsp;&#187;"
        lbtnSave.Visible = False
        lbtnDelete.Visible = True
    End Sub

    Private Sub SearchMode(Optional ByVal blnSearchMode As Boolean = True)
        trConfirm.Visible = False
        lbtnConfirm.Visible = False
        tableSearch.Visible = blnSearchMode
        trSearchSpacer.Visible = blnSearchMode
        trSearchResult.Visible = blnSearchMode
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
            'm_context.Cache.Remove("myCustomerListView")
            Session.Remove("myCustomerListView")
        End If
        If blnResetSelectedIndex Then dgSearchResult.SelectedIndex = -1
        If blnResetPageIndex Then dgSearchResult.PageIndex = 0
        If m_User.HighestAdminLevel > AdminLevel.Customer Then
            SearchMode()
            If blnRefillDataGrid Then FillDataGrid()
        Else
            CustomerAdminMode()
        End If
    End Sub

#End Region

#Region " Events "

    Private Sub btnSuche_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSuche.Click
        Search(True, True, True, True)
    End Sub

    Private Sub lbtnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lbtnCancel.Click
        If Not trConfirm.Visible Then
            Search(, True)
        Else
            ConfirmMode(False)
        End If
    End Sub

    Private Sub lbtnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lbtnNew.Click
        SearchMode(False)
        ClearEdit()
        Dim cn As New SqlClient.SqlConnection(m_User.App.Connectionstring)
        Try
            cn.Open()
            FillUnAssigned(CInt(txtCustomerID.Text), cn)
            'hier nicht den Wert aus dem Grid, ist logischer Weise 0, sondern des Users Kundennummer
            FillAccountingArea(m_User.Customer.CustomerId, True)
        Finally
            If cn.State <> ConnectionState.Closed Then
                cn.Close()
            End If
        End Try
    End Sub

    Private Sub lbtnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lbtnSave.Click
        Dim intCustomerId As Integer = CInt(txtCustomerID.Text)
        'Do SAP-Stuff here...
        Dim i_Kunnr As String = Right("0000000000" & Me.txtKUNNR.Text, 10)
        Dim blnNoData As Boolean = False
        Session("AppID") = "00000"
        Try
            Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_M_KUNDEN_ANZEIGEN", m_App, m_User, Me)

            myProxy.setImportParameter("I_KUNNR", i_Kunnr)

            myProxy.callBapi()

            Dim tblTemp2 As DataTable = myProxy.getExportTable("GS_WEB")

            If blnNoData Then
                plhConfirm.Controls.Add(New LiteralControl("<BR><b>Keine Daten gefunden!<b/><BR><BR>"))
            ElseIf tblTemp2.Rows.Count > 0 Then

                Dim sb As New System.Text.StringBuilder()
                With sb
                    .AppendFormat("KUNNR:&nbsp{0}<BR>", tblTemp2.Rows(0)("Kunnr").ToString)
                    .AppendFormat("NAME1:&nbsp{0}<BR>", tblTemp2.Rows(0)("Name1").ToString)
                    .AppendFormat("NAME2:&nbsp{0}<BR>", tblTemp2.Rows(0)("Name2").ToString)
                    .AppendFormat("STRAS:&nbsp{0}<BR>", tblTemp2.Rows(0)("Stras").ToString)
                    .AppendFormat("PSTLZ:&nbsp{0}<BR>", tblTemp2.Rows(0)("Pstlz").ToString)
                    .AppendFormat("ORT01:&nbsp{0}<BR>", tblTemp2.Rows(0)("Ort01").ToString)
                End With
                plhConfirm.Controls.Add(New LiteralControl(String.Format("<BR><b>Bitte Überprüfen Sie vor dem Bestätigen Ihre eingaben und vergleichen Sie diese mit dem Kundendatensatz aus SAP:</b><BR><BR>{0}<BR>", sb.ToString)))
            Else
                plhConfirm.Controls.Add(New LiteralControl("<BR><b>Keine Daten gefunden!<b/><BR><BR>"))
            End If
            ConfirmMode(True)
            Session("AppID") = Nothing
        Catch ex As Exception
            If HelpProcedures.CastSapBizTalkErrorMessage(ex.Message) = "NO_DATA" Then
                blnNoData = True
            End If
        Finally

        End Try
    End Sub

    Private Sub lbtnDelete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lbtnDelete.Click
        Dim cn As New SqlClient.SqlConnection(m_User.App.Connectionstring)
        Try
            Dim _customer As New Customer(CInt(txtCustomerID.Text))

            cn.Open()
            If Not _customer.HasUser(cn) Then
                _customer.Delete(cn)
                Search(True, True, True, True)
                lblMessage.Text = "Der Kunde wurde gelöscht."
            Else
                lblMessage.Text = "Der Kunde kann nicht gelöscht werden, da ihm noch Benutzer zugeordnet sind."
            End If
        Catch ex As Exception
            m_App.WriteErrorText(1, m_User.UserName, "CustomerManagement", "lbtnDelete_Click", ex.ToString)

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

    Private Sub lbtnConfirm_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lbtnConfirm.Click
        Dim cn As New SqlClient.SqlConnection(m_User.App.Connectionstring)

        Dim StrDaysLockMessage As String
        Dim StrDaysDelMessage As String

        Try

            cn.Open()
            Dim intCustomerId As Integer = CInt(txtCustomerID.Text)

            Dim KundenAdministration As Integer
            If rbeing.Checked = True Then
                KundenAdministration = 0
            ElseIf rbvollst.Checked = True Then
                KundenAdministration = 2
            ElseIf rbeing.Checked = True Then
                KundenAdministration = 1
            End If
            Dim _customer As New Customer(intCustomerId, _
                                                txtCustomerName.Text, _
                                                txtKUNNR.Text, _
                                                cbxMaster.Checked, _
                                                False, _
                                                txtCName.Text, _
                                                TranslateHTML(EditCAddress.Content, TranslationDirection.SaveHTML), _
                                                txtCMailDisplay.Text, _
                                                txtCMail.Text, _
                                                txtKundepostfach.Text, _
                                                txtKundenhotline.Text, _
                                                txtKundenfax.Text, _
                                                txtCWebDisplay.Text, _
                                                txtCWeb.Text, _
                                                CInt(txtNewPwdAfterNDays.Text), _
                                                CInt(txtLockedAfterNLogins.Text), _
                                                CInt(txtPwdNNumeric.Text), _
                                                CInt(txtPwdLength.Text), _
                                                CInt(txtNCapitalLetter.Text), _
                                                CInt(txtNSpecialCharacter.Text), _
                                                CInt(txtPwdHistoryNEntries.Text), _
                                                txtLogoPath.Text, _
                                                txtLogoPath2.Text, _
                                                "", _
                                                txtDocuPath.Text, _
                                                txtCssPath.Text, _
                                                chkAllowMultipleLogin.Checked, _
                                                chkAllowUrlRemoteLogin.Checked, _
                                                CInt(txtMaxUser.Text), _
                                                chkShowOrganization.Checked, _
                                                cbxOrgAdminRestrictToCustomerGroup.Checked, _
                                                cbxPwdDontSendEmail.Checked, _
                                                cbxNameInputOptional.Checked, _
                                                chkShowDistrikte.Checked, _
                                                cbxForcePasswordQuestion.Checked, _
                                                chkIpRestriction.Checked, _
                                                txtIpStandardUser.Text, _
                                                CInt(ddlAccountingArea.SelectedValue), _
                                                KundenAdministration, _
                                                txtKundenadministrationBeschreibung.Text, _
                                                chkKundenSperre.Checked, _
                                                chkTeamviewer.Checked, _
                                                cbxUsernameSendEmail.Checked, _
                                                CInt(ddlPortalLink.SelectedValue),
                                                "",
                                                0)
            If (txtUserLockTime.Text.Trim() <> "") Then
                If CInt(txtUserLockTime.Text) >= 5 Then
                    _customer.DaysUntilLock = CInt(txtUserLockTime.Text)
                    StrDaysLockMessage = ""
                Else
                    _customer.DaysUntilLock = 90
                    StrDaysLockMessage = "Der Wert für Tage bis zur automatischen Sperrung muss mindestens 5 betragen! Der Wert wurde automatisch auf den Standardwert (90 Tage) gesetzt. </br>"
                End If
            Else
                _customer.DaysUntilLock = 90    'Standardwert für Tage bis Sperrung
                StrDaysLockMessage = "Der Wert für Tage bis zur automatischen Sperrung muss mindestens 5 betragen! Der Wert wurde automatisch auf den Standardwert (90 Tage) gesetzt. </br>"
            End If
            If (txtUserDeleteTime.Text.Trim() <> "") Then
                If CInt(txtUserDeleteTime.Text) >= 5 Then
                    _customer.DaysUntilDelete = CInt(txtUserDeleteTime.Text)
                    StrDaysDelMessage = ""
                Else
                    _customer.DaysUntilDelete = 9999
                    StrDaysDelMessage = "Der Wert für Tage bis zur automatischen Löschung muss mindestens 5 betragen! Der Wert wurde automatisch auf den Standardwert (9999 Tage) gesetzt. </br>"
                End If
            Else
                _customer.DaysUntilDelete = 9999 'Standardwert für Tage bis Löschen
                StrDaysDelMessage = "Der Wert für Tage bis zur automatischen Löschung muss mindestens 5 betragen! Der Wert wurde automatisch auf den Standardwert (9999 Tage) gesetzt. </br>"
            End If

            _customer.Save(cn)

            'Anwendungen zuordnen
            Dim dvAppAssigned As DataView
            If intCustomerId = -1 Then
                intCustomerId = _customer.CustomerId
                txtCustomerID.Text = intCustomerId.ToString
            End If
            'If Not m_context.Cache("myCustomerAppAssigned") Is Nothing Then
            '    dvAppAssigned = CType(m_context.Cache("myCustomerAppAssigned"), DataView)
            If Not Session("myCustomerAppAssigned") Is Nothing Then
                dvAppAssigned = CType(Session("myCustomerAppAssigned"), DataView)
            Else
                dvAppAssigned = GetAppAssignedView(intCustomerId, cn)
            End If
            Dim _assignment As New Kernel.AppAssignments(intCustomerId, Kernel.AssignmentType.Customer)
            _assignment.Save(dvAppAssigned, lstAppAssigned.Items, cn)

            'Archive zuordnen
            Dim dvArchivAssigned As DataView
            If intCustomerId = -1 Then
                intCustomerId = _customer.CustomerId
                txtCustomerID.Text = intCustomerId.ToString
            End If
            'If Not m_context.Cache("myCustomerArchivAssigned") Is Nothing Then
            '    dvArchivAssigned = CType(m_context.Cache("myCustomerArchivAssigned"), DataView)
            If Not Session("myCustomerArchivAssigned") Is Nothing Then
                dvArchivAssigned = CType(Session("myCustomerArchivAssigned"), DataView)
            Else
                dvArchivAssigned = GetArchivAssignedView(intCustomerId, cn)
            End If
            Dim _archivassignment As New Kernel.ArchivAssignments(intCustomerId, Kernel.AssignmentType.Customer)
            _archivassignment.Save(dvArchivAssigned, lstArchivAssigned.Items, cn)

            Search(True, True, , True)
            lblMessage.Text = StrDaysLockMessage & StrDaysDelMessage & "Die Änderungen wurden gespeichert."
        Catch ex As Exception
            m_App.WriteErrorText(1, m_User.UserName, "CustomerManagement", "lbtnSave_Click", ex.ToString)

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

    Private Sub btnNewIpAddress_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNewIpAddress.Click
        If (Not txtCustomerID.Text = "-1") And (Not txtIpAddress.Text.Trim(" "c).Length = 0) Then
            Dim cn As New SqlClient.SqlConnection(m_User.App.Connectionstring)
            Try
                cn.Open()

                Dim cmdNew As New SqlClient.SqlCommand("INSERT INTO IpAddresses VALUES (" & txtCustomerID.Text & ",'" & txtIpAddress.Text & "')", cn)
                cmdNew.ExecuteNonQuery()
                EditEditMode(CInt(txtCustomerID.Text))
                txtIpAddress.Text = ""
            Catch ex As Exception
                Me.lblError.Text = ex.Message
                txtIpAddress.Text = "s. Fehlertext"
            Finally
                If cn.State <> ConnectionState.Closed Then
                    cn.Close()
                End If
            End Try
        End If
    End Sub

    Private Sub GridNavigation1_PagerChanged(ByVal PageIndex As Integer) Handles GridNavigation1.PagerChanged
        dgSearchResult.PageIndex = PageIndex
        FillDataGrid()
    End Sub

    Private Sub GridNavigation1_PageSizeChanged() Handles GridNavigation1.PageSizeChanged
        FillDataGrid()
    End Sub

    Private Sub dgSearchResult_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgSearchResult.RowCommand

        Dim CtrlLabel As Label
        Dim index As Integer
        Dim row As GridViewRow
        If e.CommandName = "Edit" Then
            index = Convert.ToInt32(e.CommandArgument)
            row = dgSearchResult.Rows(index)
            CtrlLabel = row.Cells(0).FindControl("lblCustomerID")
            EditEditMode(CInt(CtrlLabel.Text))
            dgSearchResult.SelectedIndex = row.RowIndex
        ElseIf e.CommandName = "Del" Then
            index = Convert.ToInt32(e.CommandArgument)
            row = dgSearchResult.Rows(index)
            CtrlLabel = row.Cells(0).FindControl("lblCustomerID")
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

#End Region

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

    Protected Sub btnEmpty_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnEmpty.Click
        Search(True, True, True, True)
    End Sub

    Private Sub Repeater1_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.RepeaterCommandEventArgs) Handles Repeater1.ItemCommand
        Dim cn As New SqlClient.SqlConnection(m_User.App.Connectionstring)
        Try
            cn.Open()

            Dim cmdNew As New SqlClient.SqlCommand("DELETE FROM IpAddresses WHERE CustomerID=" & txtCustomerID.Text & " AND IpAddress='" & e.CommandArgument & "'", cn)
            cmdNew.ExecuteNonQuery()
            EditEditMode(CInt(txtCustomerID.Text))
            txtIpAddress.Text = ""
        Finally
            If cn.State <> ConnectionState.Closed Then
                cn.Close()
            End If
        End Try
    End Sub

    Private Function setKundenadministrationInfoVisibility(Optional ByVal Value As Integer = 0) As Boolean

        If Not Value = 1 Then
            trKundenadministrationInfo.Visible = False
        Else
            'wenn eingeschränkt
            trKundenadministrationInfo.Visible = True
        End If

    End Function

    Protected Sub rbKeine_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs) Handles rbKeine.CheckedChanged
        setKundenadministrationInfoVisibility()
    End Sub

    Protected Sub rbvollst_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs) Handles rbvollst.CheckedChanged
        setKundenadministrationInfoVisibility()
    End Sub

    Protected Sub rbeing_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs) Handles rbeing.CheckedChanged
        setKundenadministrationInfoVisibility(1)
    End Sub

    Private Sub lstAppAssigned_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles lstAppAssigned.PreRender
        For Each item As ListItem In lstAppAssigned.Items
            item.Attributes.Add("title", item.Text)
        Next
    End Sub

    Private Sub lstAppUnAssigned_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles lstAppUnAssigned.PreRender
        For Each item As ListItem In lstAppUnAssigned.Items
            item.Attributes.Add("title", item.Text)
        Next
    End Sub

    Private Sub lstArchivAssigned_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles lstArchivAssigned.SelectedIndexChanged
        For Each item As ListItem In lstArchivAssigned.Items
            item.Attributes.Add("title", item.Text)
        Next
    End Sub

    Private Sub lstArchivUnAssigned_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles lstArchivUnAssigned.PreRender
        For Each item As ListItem In lstArchivUnAssigned.Items
            item.Attributes.Add("title", item.Text)
        Next
    End Sub

    ''' <summary>
    ''' Füllt die DropdownList mit den Login-Links
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub LoadLoginLinks()

        Dim TempTable As New DataTable
        Dim cn As New SqlClient.SqlConnection(m_User.App.Connectionstring)
        cn.Open()

        Dim daLoginLink As SqlClient.SqlDataAdapter
        daLoginLink = New SqlClient.SqlDataAdapter("SELECT * FROM WebUserUploadLoginLink", cn)

        daLoginLink.Fill(TempTable)

        cn.Close()
        cn.Dispose()

        Dim dItem As New ListItem

        dItem.Text = " --Auswahl-- "
        dItem.Value = 0

        ddlPortalLink.Items.Add(dItem)

        For Each row As DataRow In TempTable.Rows
            dItem = New ListItem

            dItem.Text = row("Text").ToString
            dItem.Value = row("ID")

            ddlPortalLink.Items.Add(dItem)
        Next

    End Sub

    Protected Sub cbxPwdDontSendEmail_CheckedChanged(sender As Object, e As EventArgs) Handles cbxPwdDontSendEmail.CheckedChanged
        If cbxPwdDontSendEmail.Checked Then
            cbxForcePasswordQuestion.Enabled = False
            cbxForcePasswordQuestion.Checked = False
        Else
            cbxForcePasswordQuestion.Enabled = True
        End If
    End Sub

End Class