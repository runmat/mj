Imports CKG.Base.Kernel.Admin
Imports CKG.Base.Kernel.Security
Imports CKG.Base.Kernel.Common.Common
Imports CKG.Base.Business.HelpProcedures
Imports CKG.Base.Common
Imports CKG.Services
Imports Telerik.Web.UI
Imports System.IO
Imports System.Net.Configuration

Imports System.Web.UI.WebControls.WebParts


Partial Public Class CustomerManagement
    Inherits Page



#Region " Membervariables "
    Private m_User As User
    Private m_App As App
    Protected WithEvents GridNavigation1 As GridNavigation

    Private ReadOnly logoVirtUploadPath As String = "/Services/Images/Kundenlogos"
    Private ReadOnly logoVirtUploadPath2 As String = "/Services/Images/Buchungskreis"
    Private ReadOnly logoVirtUploadPath3 As String = "/Services/Images/HeaderBackgrounds"
    Const UploadMaxTotalBytes As Integer = 3 * 1024 * 1024 ' 3 MB
    Private uploadTotalBytes As Integer
    Private tblApps As DataTable
    Private tblRights As DataTable

#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load

        RadAsyncUpload1.Attributes.Add("onClick", "alert('Das Logo sollte ca. 220 x 70 Pixel haben');")

        m_User = GetUser(Me)

        HandleCheckBoxesAppIsMvcIsDefaultFavorite(sender)


        'LogoUploadList unten anzeigen 
        RadAsyncUpload1.UploadedFilesRendering = AsyncUpload.UploadedFilesRendering.BelowFileInput

        'Für Postback bei Logo Upload
        ScriptManager.RegisterClientScriptBlock(Page, GetType(CustomerManagement), "async_upload", "function onUploadFailed(sender, args) { " & _
                                       "alert(args.get_message()); " & _
                                       "} " & _
                                       "function onFileUploaded(sender, args) { " & _
                                       "document.getElementById('" & btnUpload.ClientID & "').click(); }", True)

        lblHead.Text = "Kundenverwaltung"
        AdminAuth(Me, m_User, AdminLevel.Master)
        GridNavigation1.setGridElment(dgSearchResult)

        If Session("custMgmtApps") IsNot Nothing Then
            tblApps = CType(Session("custMgmtApps"), DataTable)
        End If

        Try
            m_App = New App(m_User)

            lbOk.Attributes.Add("onclick", "if(!confirm('Hinzufügen / Ändern bestätigen!')) return false;")
            btnDelBukrsLogo.Attributes.Add("onclick", "if(!confirm('Wollen Sie wirklich das Mandantenlogo löschen?')) return false;")

            lblError.Text = ""
            lblMessage.Text = ""

            If Not IsPostBack Then
                FillForm()
                Tabs.ActiveTabIndex = 0
            End If
        Catch ex As Exception
            m_App.WriteErrorText(1, m_User.UserName, "CustomerManagement", "Page_Load", ex.ToString)

            lblError.Text = ex.ToString
        End Try
    End Sub

#Region " Data and Function "

    Private Sub FillForm()
        '***********************************************
        'Ausblenden, da noch nicht fertig implementiert*
        '***********************************************
        'trPwdHistoryNEntries.Visible = False          '*
        '***********************************************

        FillLoginLinks()
        FillPortalTypes()
        FillReferenceTypes()
        FillMvcSelectionTypes()

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

        Dim dvCustomer As DataView

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

            For Each xRow As GridViewRow In dgSearchResult.Rows
                xRow.Cells(6).Text = TranslateHTML(xRow.Cells(6).Text, TranslationDirection.ReadHTML)
            Next


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
            ihCustomerID.Value = _Customer.CustomerId.ToString
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
            ddlPortalType.SelectedValue = _Customer.PortalType
            cbxForceSpecifiedLoginLink.Checked = _Customer.ForceSpecifiedLoginLink
            txtLogoutLink.Text = _Customer.LogoutLink
            ddlReferenzTyp1.SelectedValue = _Customer.ReferenceType1
            ddlReferenzTyp2.SelectedValue = _Customer.ReferenceType2
            ddlReferenzTyp3.SelectedValue = _Customer.ReferenceType3
            ddlReferenzTyp4.SelectedValue = _Customer.ReferenceType4
            txtMvcSelectionUrl.Text = _Customer.MvcSelectionUrl
            'txtMvcSelectionType.Text = _Customer.MvcSelectionType
            ddlMvcSelectionType.SelectedValue = _Customer.MvcSelectionType

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
            FillApps(_Customer.CustomerId, _Customer.PortalType, cn)
            FillArchivesAssigned(_Customer.CustomerId, cn)
            FillArchivesUnAssigned(_Customer.CustomerId, cn)

            ' Rechte 
            FillRights(_Customer.CustomerId, _Customer.PortalType, cn)
            'FillArchivesAssigned(_Customer.CustomerId, cn)
            'FillArchivesUnAssigned(_Customer.CustomerId, cn)


            'Style
            txtLogoPath.Text = _Customer.CustomerStyle.LogoPath.ToString
            If _Customer.LogoPath.ToString <> "" Then
                imgLogoThumb.Style.Clear()
            End If

            txtCssPath.Text = _Customer.CustomerStyle.CssPath.ToString

            txtLogoPath2.Text = _Customer.LogoPath2.ToString
            txtHeaderBackgroundPath.Text = _Customer.HeaderBackgroundPath.ToString

            Fill_BilderDropDownListen()


            'IP-Adress-Verwaltung
            chkIpRestriction.Checked = _Customer.IpRestriction
            txtIpStandardUser.Text = _Customer.IpStandardUser

            cbxPwdDontSendEmail.Checked = _Customer.CustomerPasswordRules.DontSendEmail
            cbxUsernameSendEmail.Checked = _Customer.CustomerUsernameRules.DontSendEmail

            cbxNameInputOptional.Checked = _Customer.CustomerPasswordRules.NameInputOptional

            'Benutzer und Organisation
            txtUserLockTime.Text = _Customer.DaysUntilLock
            txtUserDeleteTime.Text = _Customer.DaysUntilDelete

            If ihCustomerID.Value = "-1" Then
                Tabs.Tabs(3).Visible = False
            Else
                Tabs.Tabs(3).Visible = True
                FillIpAddresses(_Customer)
            End If

            If _Customer.Selfadministration = 2 Then
                rbKeine.Checked = False
                rbeing.Checked = False
                rbvollst.Checked = True
            ElseIf _Customer.Selfadministration = 1 Then
                rbKeine.Checked = False
                rbvollst.Checked = False
                rbeing.Checked = True
            Else
                rbvollst.Checked = False
                rbeing.Checked = False
                rbKeine.Checked = True
            End If
            setKundenadministrationInfoVisibility()
            txtKundenadministrationBeschreibung.Text = _Customer.SelfadministrationInfo
            EditKundenadministrationContact.Content = TranslateHTML(_Customer.SelfadministrationContact, TranslationDirection.ReadHTML)

            'kundenInformation Berechtigung
            fillBusinessownerGrid(0)
            fillAdminpersonGrid(0)

            'SilverDAT Zugang
            txtSDCustomerNumber.Text = _Customer.SilverDATCredentials.CustomerNumber
            txtSDUserName.Text = _Customer.SilverDATCredentials.UserName
            txtSDPassword.Text = _Customer.SilverDATCredentials.Password
            txtSDLoginName.Text = _Customer.SilverDATCredentials.LoginName
            txtSDSignatur.Text = _Customer.SilverDATCredentials.Signatur
            _txtSDSignatur2.Text = _Customer.SilverDATCredentials.Signatur2
            Return True
        Finally
            If cn.State <> ConnectionState.Closed Then
                cn.Close()
            End If
        End Try
    End Function

    Private Sub Fill_BilderDropDownListen()
        'Zweites Logo.
        If txtLogoPath2.Text <> "" Then
            Image_Buchungskreis_aktiv.Style.Clear()
        End If
        Image_Buchungskreis_aktiv.ImageUrl = txtLogoPath2.Text

        'Fill DropDownlist für Logo2Tausch
        Dim sLogoPath As String = logoVirtUploadPath2
        Dim sPhysPath As String = HttpContext.Current.Server.MapPath(logoVirtUploadPath2)

        imageDDL.Items.Clear()
        imageDDL.Items.Add(New RadComboBoxItem("Bitte wählen Sie", "Choose"))
        imageDDL.Items.Add(New RadComboBoxItem("Bild hochladen", "Upload"))
        If Directory.Exists(sPhysPath) Then
            For Each file As String In Directory.GetFiles(sPhysPath)
                If Path.GetFileName(file).ToLower() <> "thumbs.db" Then
                    Dim comboItem As New RadComboBoxItem("", sLogoPath.TrimEnd("/"c) & "/" & Path.GetFileNameWithoutExtension(file) & Path.GetExtension(file))
                    comboItem.ImageUrl = comboItem.Value
                    imageDDL.Items.Add(comboItem)
                End If
            Next
        End If

        'Headerbild
        If txtHeaderBackgroundPath.Text <> "" Then
            Image_HeaderBackgroundPath_aktiv.Style.Clear()
        End If
        Image_HeaderBackgroundPath_aktiv.ImageUrl = txtHeaderBackgroundPath.Text

        'Fill DropDownlist für HeaderTausch
        Dim sLogoPath2 As String = logoVirtUploadPath3
        Dim sPhysPath2 As String = HttpContext.Current.Server.MapPath(sLogoPath2)

        imageHeaderDDL.Items.Clear()
        imageHeaderDDL.Items.Add(New RadComboBoxItem("Bitte wählen Sie", "Choose"))
        imageHeaderDDL.Items.Add(New RadComboBoxItem("Bild hochladen", "Upload"))
        If Directory.Exists(sPhysPath2) Then
            For Each file As String In Directory.GetFiles(sPhysPath2)
                If Path.GetFileName(file).ToLower() <> "thumbs.db" Then
                    Dim comboItem As New RadComboBoxItem("", sLogoPath2.TrimEnd("/"c) & "/" & Path.GetFileNameWithoutExtension(file) & Path.GetExtension(file))
                    comboItem.ImageUrl = comboItem.Value
                    imageHeaderDDL.Items.Add(comboItem)
                End If
            Next
        End If
    End Sub

    Private Function GetArchivAssignedView(ByVal intCustomerID As Integer, ByVal cn As SqlClient.SqlConnection) As DataView
        'Als Extra-Funktion ausgelagert, da beim Speichern evtl.
        'benoetigt, wenn Cache leer ist (in jenem Fall soll die
        'List kein DataBind ausfuehren).
        Dim _ArchivAssigned As New ArchivList(intCustomerID, cn)
        _ArchivAssigned.GetAssigned()
        _ArchivAssigned.DefaultView.Sort = "EasyArchivName"
        Session("myCustomerArchivAssigned") = _ArchivAssigned.DefaultView
        Return _ArchivAssigned.DefaultView
    End Function

    Private Function GetViewAppsUnassigned() As DataView
        If tblApps IsNot Nothing Then
            Dim dvAppUnassigned As New DataView(tblApps)
            dvAppUnassigned.Sort = "AppFriendlyName"

            Dim strFilter As String = ""
            If Not String.IsNullOrEmpty(txtFilterUnassignedApps.Text) Then
                strFilter = txtFilterUnassignedApps.Text.Trim().Trim("*"c)
            End If
            dvAppUnassigned.RowFilter = "Assigned <> 'X' AND (AppName LIKE '%" & strFilter & "%' OR AppFriendlyName LIKE '%" & strFilter & "%' OR AppURL LIKE '%" & strFilter & "%')"

            Return dvAppUnassigned
        End If
        Return Nothing
    End Function

    Private Function GetViewAppsAssigned() As DataView
        If tblApps IsNot Nothing Then
            Dim dvAppAssigned As New DataView(tblApps)
            dvAppAssigned.Sort = "AppFriendlyName"
            dvAppAssigned.RowFilter = "Assigned = 'X'"
            Return dvAppAssigned
        End If
        Return Nothing
    End Function

    Private Function GetRights() As DataView
        If tblRights IsNot Nothing Then
            Dim dvRights As New DataView(tblRights)
            dvRights.Sort = "CategoryID"
            Return dvRights
        End If
        Return Nothing
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

    Private Sub InitAppTable()
        tblApps = New DataTable()
        tblApps.Columns.Add("AppId")
        tblApps.Columns.Add("AppURL")
        tblApps.Columns.Add("AppName")
        tblApps.Columns.Add("AppFriendlyName")
        tblApps.Columns.Add("AppType")
        tblApps.Columns.Add("AppTechType")
        tblApps.Columns.Add("AppDescription")
        tblApps.Columns.Add("Assigned")
        tblApps.Columns.Add("AppIsMvcDefaultFavorite", Type.GetType("System.Boolean"))
    End Sub


    Private Sub InitRightsTable()
        tblRights = New DataTable()
        tblRights.Columns.Add("CustomerID")
        tblRights.Columns.Add("CategoryID")
        tblRights.Columns.Add("HasSettings")
        tblRights.Columns.Add("Description")
    End Sub


    Private Sub FillApps(ByVal intCustomerID As Integer, ByVal strCustomerPortalType As String, ByVal cn As SqlClient.SqlConnection)

        If tblApps Is Nothing OrElse tblApps.Columns.Count = 0 Then
            InitAppTable()
        Else
            tblApps.Clear()
        End If

        'Unassigned
        Dim AppUnAssigned As New ApplicationList(intCustomerID, cn)
        AppUnAssigned.GetUnassigned(strCustomerPortalType)
        For Each row As DataRow In AppUnAssigned.Rows
            Dim newRow As DataRow = tblApps.NewRow()
            newRow("AppId") = row("AppId")
            newRow("AppURL") = row("AppURL")
            newRow("AppName") = row("AppName")
            newRow("AppFriendlyName") = row("AppFriendlyName")
            newRow("AppType") = row("AppType")
            newRow("AppTechType") = row("AppTechType")
            newRow("AppDescription") = row("AppDescription")
            newRow("Assigned") = ""
            newRow("AppIsMvcDefaultFavorite") = False
            tblApps.Rows.Add(newRow)
        Next

        'Assigned
        Dim AppAssigned As New ApplicationList(intCustomerID, cn)
        AppAssigned.GetAssigned()
        For Each row As DataRow In AppAssigned.Rows
            Dim newRow As DataRow = tblApps.NewRow()
            newRow("AppId") = row("AppId")
            newRow("AppURL") = row("AppURL")
            newRow("AppName") = row("AppName")
            newRow("AppFriendlyName") = row("AppFriendlyName")
            newRow("AppType") = row("AppType")
            newRow("AppTechType") = row("AppTechType")
            newRow("AppDescription") = row("AppDescription")
            newRow("Assigned") = "X"
            newRow("AppIsMvcDefaultFavorite") = row("AppIsMvcDefaultFavorite")
            tblApps.Rows.Add(newRow)
        Next

        Session("custMgmtApps") = tblApps

        'Liste für Anwendungseinstellungen füllen
        lstCustomerSettings.DataSource = GetViewAppsAssigned()
        lstCustomerSettings.DataTextField = "AppFriendlyName"
        lstCustomerSettings.DataValueField = "AppID"
        lstCustomerSettings.DataBind()
        FillCustomerSettingsList(If(lstCustomerSettings.Items.Count > 0, 0, -1))

        'Grids binden. Setzen der DataSource geschieht über das jeweilige NeedDataSource-Event
        rgAppUnAssigned.Rebind()
        rgAppAssigned.Rebind()

    End Sub

    Private Sub FillArchivesAssigned(ByVal intCustomerID As Integer, ByVal cn As SqlClient.SqlConnection)

        Dim dvArchivAssigned As DataView = GetArchivAssignedView(intCustomerID, cn)
        For Each row As DataRow In dvArchivAssigned.Table.Rows
            row("EasyArchivName") = row("EasyArchivName").ToString.ToUpper & " || Lagerort-Name: " & row("EasyLagerortName").ToString & " || QueryIndex-Name: " & row("EasyQueryIndexName").ToString & " || Titel: " & row("EasyTitleName").ToString
        Next

        lstArchivAssigned.DataSource = dvArchivAssigned
        lstArchivAssigned.DataTextField = "EasyArchivName"
        lstArchivAssigned.DataValueField = "ArchivID"
        lstArchivAssigned.DataBind()

    End Sub

    Private Sub FillArchivesUnAssigned(ByVal intCustomerID As Integer, ByVal cn As SqlClient.SqlConnection)

        Dim ArchivUnAssigned As New ArchivList(intCustomerID, cn)
        ArchivUnAssigned.GetUnassigned()
        ArchivUnAssigned.DefaultView.Sort = "EasyArchivName"

        For Each row As DataRow In ArchivUnAssigned.Rows
            row("EasyArchivName") = row("EasyArchivName").ToString.ToUpper & " || Lagerort-Name: " & row("EasyLagerortName").ToString & " || QueryIndex-Name: " & row("EasyQueryIndexName").ToString & " || Titel: " & row("EasyTitleName").ToString
        Next

        lstArchivUnAssigned.DataSource = ArchivUnAssigned.DefaultView
        lstArchivUnAssigned.DataTextField = "EasyArchivName"
        lstArchivUnAssigned.DataValueField = "ArchivID"
        lstArchivUnAssigned.DataBind()
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
                        Dim newItem As New ListItem("Übergeordnet", "-1")
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

    Private Sub FillRights(ByVal intCustomerID As Integer, ByVal strCustomerPortalType As String, ByVal cn As SqlClient.SqlConnection)

        If tblRights Is Nothing OrElse tblRights.Columns.Count = 0 Then
            InitRightsTable()
        Else
            tblRights.Clear()
        End If

        Dim possibleRights As New RightList(intCustomerID, cn)

        possibleRights.GetAllPossibleRightsforThisCustomer()

        For Each row As DataRow In possibleRights.Rows
            Dim newRow As DataRow = tblRights.NewRow()
            newRow("CustomerID") = row("CustomerID")
            newRow("CategoryID") = row("CategoryID")
            newRow("HasSettings") = row("HasSettings")
            newRow("Description") = row("Description")
            tblRights.Rows.Add(newRow)

        Next

        rgRights.Rebind()

    End Sub


    Private Sub ClearEdit()
        ihCustomerID.Value = "-1"
        txtCustomerName.Text = ""
        txtKUNNR.Text = "0"
        txtDocuPath.Text = ""
        cbxMaster.Checked = False
        chkAllowMultipleLogin.Checked = True
        chkAllowUrlRemoteLogin.Checked = False
        chkShowOrganization.Checked = False
        cbxOrgAdminRestrictToCustomerGroup.Checked = False
        chkTeamviewer.Checked = False
        cbxForceSpecifiedLoginLink.Checked = False
        txtLogoutLink.Text = ""
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
        cbxNameInputOptional.Checked = True
        'Anwendungen
        tblApps = Nothing
        Session("custMgmtApps") = tblApps
        rgAppAssigned.Rebind()
        rgAppUnAssigned.Rebind()
        'Kontaktdaten
        txtCName.Text = ""
        EditCAddress.Content = ""
        txtCMailDisplay.Text = ""
        txtCMail.Text = ""
        txtCWebDisplay.Text = ""
        txtCWeb.Text = ""
        txtKundepostfach.Text = ""
        txtKundenhotline.Text = ""
        txtKundenfax.Text = ""
        'SilverDAT Zugang
        txtSDCustomerNumber.Text = ""
        txtSDUserName.Text = ""
        txtSDPassword.Text = ""
        txtSDLoginName.Text = ""
        txtSDSignatur.Text = ""
        txtSDSignatur2.Text = ""
        'Style
        txtLogoPath.Text = ""
        '§§§ JVE 18.09.2006: Logo2
        txtLogoPath2.Text = ""
        '------------------------
        txtCssPath.Text = ""
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
        'txtCustomerID.Enabled = Not blnLock
        'txtCustomerID.BackColor = Drawing.Color.FromName(strBackColor)
        txtCustomerName.Enabled = Not blnLock
        txtCustomerName.BackColor = Drawing.Color.FromName(strBackColor)
        If m_User.HighestAdminLevel < AdminLevel.Master Then
            txtKUNNR.Enabled = False
            txtKUNNR.CssClass = "InfoBoxFlat"
        Else
            txtKUNNR.Enabled = Not blnLock
            txtKUNNR.BackColor = Drawing.Color.FromName(strBackColor)
        End If
        cbxMaster.Enabled = Not blnLock
        chkKundenSperre.Enabled = Not blnLock
        chkTeamviewer.Enabled = Not blnLock
        ddlAccountingArea.Enabled = Not blnLock
        chkAllowMultipleLogin.Enabled = Not blnLock
        chkAllowUrlRemoteLogin.Enabled = Not blnLock
        txtDocuPath.Enabled = Not blnLock
        txtDocuPath.BackColor = Drawing.Color.FromName(strBackColor)

        'LoginRegeln
        txtLockedAfterNLogins.Enabled = Not blnLock
        txtLockedAfterNLogins.BackColor = Drawing.Color.FromName(strBackColor)
        txtNewPwdAfterNDays.Enabled = Not blnLock
        txtNewPwdAfterNDays.BackColor = Drawing.Color.FromName(strBackColor)
        'Passwortregeln
        txtPwdLength.Enabled = Not blnLock
        txtPwdLength.BackColor = Drawing.Color.FromName(strBackColor)
        txtPwdNNumeric.Enabled = Not blnLock
        txtPwdNNumeric.BackColor = Drawing.Color.FromName(strBackColor)
        txtNCapitalLetter.Enabled = Not blnLock
        txtNCapitalLetter.BackColor = Drawing.Color.FromName(strBackColor)
        txtNSpecialCharacter.Enabled = Not blnLock
        txtNSpecialCharacter.BackColor = Drawing.Color.FromName(strBackColor)
        txtPwdHistoryNEntries.Enabled = Not blnLock
        txtPwdHistoryNEntries.BackColor = Drawing.Color.FromName(strBackColor)
        cbxPwdDontSendEmail.Enabled = Not blnLock
        cbxUsernameSendEmail.Enabled = Not blnLock
        cbxNameInputOptional.Enabled = Not blnLock

        'SilverDAT Zugang
        txtSDCustomerNumber.Enabled = Not blnLock
        txtSDCustomerNumber.BackColor = Drawing.Color.FromName(strBackColor)
        txtSDUserName.Enabled = Not blnLock
        txtSDUserName.BackColor = Drawing.Color.FromName(strBackColor)
        txtSDPassword.Enabled = Not blnLock
        txtSDPassword.BackColor = Drawing.Color.FromName(strBackColor)
        txtSDLoginName.Enabled = Not blnLock
        txtSDLoginName.BackColor = Drawing.Color.FromName(strBackColor)
        txtSDSignatur.Enabled = Not blnLock
        txtSDSignatur.BackColor = Drawing.Color.FromName(strBackColor)
        txtSDSignatur2.Enabled = Not blnLock
        txtSDSignatur2.BackColor = Drawing.Color.FromName(strBackColor)

        'IP-Adressen
        chkIpRestriction.Enabled = Not blnLock
        txtIpStandardUser.Enabled = Not blnLock
        txtIpStandardUser.BackColor = Drawing.Color.FromName(strBackColor)
        txtIpAddress.Enabled = Not blnLock
        txtIpAddress.BackColor = Drawing.Color.FromName(strBackColor)

        'Anwendungen
        rgAppAssigned.Enabled = Not blnLock
        rgAppUnAssigned.Enabled = Not blnLock

        'Archive
        lstArchivAssigned.Enabled = Not blnLock
        lstArchivAssigned.BackColor = Drawing.Color.FromName(strBackColor)
        lstArchivUnAssigned.Enabled = Not blnLock
        lstArchivUnAssigned.BackColor = Drawing.Color.FromName(strBackColor)

        'Kontaktdaten
        txtCName.Enabled = Not blnLock
        txtCName.BackColor = Drawing.Color.FromName(strBackColor)
        EditCAddress.Enabled = Not blnLock
        EditCAddress.BackColor = Drawing.Color.FromName(strBackColor)
        txtCMailDisplay.Enabled = Not blnLock
        txtCMailDisplay.BackColor = Drawing.Color.FromName(strBackColor)
        txtCMail.Enabled = Not blnLock
        txtCMail.BackColor = Drawing.Color.FromName(strBackColor)
        txtCWebDisplay.Enabled = Not blnLock
        txtCWebDisplay.BackColor = Drawing.Color.FromName(strBackColor)
        txtCWeb.Enabled = Not blnLock
        txtCWeb.BackColor = Drawing.Color.FromName(strBackColor)
        txtKundepostfach.Enabled = Not blnLock
        txtKundepostfach.BackColor = Drawing.Color.FromName(strBackColor)
        txtKundenhotline.Enabled = Not blnLock
        txtKundenhotline.BackColor = Drawing.Color.FromName(strBackColor)
        txtKundenfax.Enabled = Not blnLock
        txtKundenfax.BackColor = Drawing.Color.FromName(strBackColor)
        'Style
        txtLogoPath.Enabled = Not blnLock
        txtLogoPath.BackColor = Drawing.Color.FromName(strBackColor)


        '§§§ JVE 18.09.2006: Logo2
        txtLogoPath2.Enabled = Not blnLock
        txtLogoPath2.BackColor = Drawing.Color.FromName(strBackColor)
        '-------------------------
        'CHC ITA 5968
        txtHeaderBackgroundPath.Enabled = Not blnLock
        txtHeaderBackgroundPath.BackColor = Drawing.Color.FromName(strBackColor)


        'CHC Bilder nicht mehr ausblenden nach klick auf speichern
        Bilderwahl_Switch(False)


        txtCssPath.Enabled = Not blnLock
        txtCssPath.BackColor = Drawing.Color.FromName(strBackColor)
        'ShowOrganization
        chkShowOrganization.Enabled = Not blnLock
        cbxOrgAdminRestrictToCustomerGroup.Enabled = Not blnLock
        'MaxUser
        txtMaxUser.Enabled = Not blnLock
        txtMaxUser.BackColor = Drawing.Color.FromName(strBackColor)
        'Buttons
        btnAssign.Enabled = Not blnLock
        btnUnAssign.Enabled = Not blnLock

        'Benutzer und Organisation
        txtUserLockTime.Enabled = Not blnLock
        txtUserLockTime.BackColor = Drawing.Color.FromName(strBackColor)

        txtMvcSelectionUrl.Enabled = Not blnLock
        txtMvcSelectionUrl.BackColor = Drawing.Color.FromName(strBackColor)

        txtUserDeleteTime.Enabled = Not blnLock
        txtUserDeleteTime.BackColor = Drawing.Color.FromName(strBackColor)

        rbKeine.Enabled = Not blnLock
        rbeing.Enabled = Not blnLock
        rbvollst.Enabled = Not blnLock
        txtKundenadministrationBeschreibung.Enabled = Not blnLock
        txtKundenadministrationBeschreibung.BackColor = Drawing.Color.FromName(strBackColor)
        EditKundenadministrationContact.Enabled = Not blnLock
        EditKundenadministrationContact.BackColor = Drawing.Color.FromName(strBackColor)

        ddlPortalLink.Enabled = Not blnLock
        ddlPortalType.Enabled = Not blnLock

        cbxForceSpecifiedLoginLink.Enabled = Not blnLock
        txtLogoutLink.Enabled = Not blnLock
        txtLogoutLink.BackColor = Drawing.Color.FromName(strBackColor)

        ddlReferenzTyp1.Enabled = Not blnLock
        ddlReferenzTyp2.Enabled = Not blnLock
        ddlReferenzTyp3.Enabled = Not blnLock
        ddlReferenzTyp4.Enabled = Not blnLock

        ddlMvcSelectionType.Enabled = Not blnLock
        txtMvcSelectionUrl.Enabled = Not blnLock
    End Sub

    Private Sub CustomerAdminMode()
        SearchMode(False)
        Tabs.Tabs(4).Visible = False

        If m_User.IsCustomerAdmin Then
            LockEdit(False)
            EditEditMode(m_User.Customer.CustomerId)
        End If

    End Sub

    Private Sub ConfirmMode(ByVal confirmOn As Boolean)
        LockEdit(confirmOn)
        lbtnSave.Enabled = Not confirmOn
    End Sub

    Private Sub EditEditMode(ByVal intCustomerId As Integer)
        If Not FillEdit(intCustomerId) Then
            LockEdit(True)
            lbtnSave.Enabled = False
        Else
            lbtnSave.Enabled = True
        End If
        lbtnCancel.Text = "Verwerfen&nbsp;&#187;"
    End Sub

    Private Sub EditDeleteMode(ByVal intGroupId As Integer)
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

    Private Sub SearchMode(Optional ByVal blnSearchMode As Boolean = True, Optional ByVal blnNewSearch As Boolean = False)
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
            Tabs.ActiveTabIndex = 0
        End If
    End Sub

    Private Sub Search(Optional ByVal blnShowDataGrid As Boolean = False, Optional ByVal blnResetSelectedIndex As Boolean = False, Optional ByVal blnResetPageIndex As Boolean = False, Optional ByVal blnClearCache As Boolean = False)
        ClearEdit()
        If blnClearCache Then
            Session.Remove("myCustomerListView")
        End If
        If blnResetSelectedIndex Then dgSearchResult.SelectedIndex = -1
        If blnResetPageIndex Then dgSearchResult.PageIndex = 0
        If m_User.HighestAdminLevel > AdminLevel.Customer Then
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
            CustomerAdminMode()
        End If
    End Sub

    Private Sub Log(ByVal strIdentification As String, ByVal strDescription As String, ByVal tblParameters As DataTable, Optional ByVal strCategory As String = "APP")
        Dim logApp As New CKG.Base.Kernel.Logging.Trace(m_User.App.Connectionstring, m_User.App.SaveLogAccessSAP, m_User.App.LogLevel)

        ' strCategory
        Dim strUserName As String = m_User.UserName ' strUserName
        Dim strSessionID As String = Session.SessionID ' strSessionID
        Dim intSource As Integer = CInt(Request.QueryString("AppID")) ' intSource 
        Dim strTask As String = "Admin - Kundenverwaltung" ' strTask
        ' strIdentification
        ' strDescription
        Dim strCustomerName As String = m_User.CustomerName ' strCustomername
        Dim blnIsTestUser As Boolean = m_User.IsTestUser ' blnIsTestUser
        Dim intSeverity As Integer = 0 ' intSeverity 

        logApp.WriteEntry(strCategory, strUserName, strSessionID, intSource, strTask, strIdentification, strDescription, strCustomerName, m_User.Customer.CustomerId, blnIsTestUser, intSeverity, tblParameters)
    End Sub

    Private Function SetOldLogParameters(ByVal intCustomerId As Int32) As DataTable
        Dim cn As New SqlClient.SqlConnection(m_User.App.Connectionstring)
        Try

            cn.Open()
            Dim _Customer As New Customer(intCustomerId, cn)

            Dim tblPar = CreateLogTableStructure()

            With tblPar
                .Rows.Add(.NewRow)
                .Rows(.Rows.Count - 1)("Status") = "Alt"

                .Rows(.Rows.Count - 1)("Firmenname") = _Customer.CustomerName
                .Rows(.Rows.Count - 1)("KUNNR") = _Customer.KUNNR
                .Rows(.Rows.Count - 1)("Neues Kennwort nach n Tagen") = _Customer.CustomerLoginRules.NewPasswordAfterNDays.ToString
                .Rows(.Rows.Count - 1)("Konto sperren nach n Fehlversuchen") = _Customer.CustomerLoginRules.LockedAfterNLogins.ToString
                .Rows(.Rows.Count - 1)("Mehrfaches Login") = _Customer.AllowMultipleLogin
                .Rows(.Rows.Count - 1)("URL Remote Login") = _Customer.AllowUrlRemoteLogin
                .Rows(.Rows.Count - 1)("Mindestlänge") = _Customer.CustomerPasswordRules.Length.ToString
                .Rows(.Rows.Count - 1)("n numerische Zeichen") = _Customer.CustomerPasswordRules.Numeric.ToString
                .Rows(.Rows.Count - 1)("n Großbuchstaben") = _Customer.CustomerPasswordRules.CapitalLetters.ToString
                .Rows(.Rows.Count - 1)("n Sonderzeichen") = _Customer.CustomerPasswordRules.SpecialCharacter.ToString
                .Rows(.Rows.Count - 1)("Sperre letzte n Kennworte") = _Customer.CustomerPasswordRules.PasswordHistoryEntries.ToString

                Dim dvAppAssigned As DataView = GetViewAppsAssigned()
                Dim strAnwendungen As String = ""
                Dim j As Int32
                For j = 0 To dvAppAssigned.Count - 1
                    strAnwendungen &= dvAppAssigned(j)("AppFriendlyName").ToString & vbNewLine
                Next
                .Rows(.Rows.Count - 1)("Anwendungen") = strAnwendungen

                If Not _Customer.CustomerContact Is Nothing Then
                    .Rows(.Rows.Count - 1)("Kontakt- Name") = _Customer.CustomerContact.Name
                    .Rows(.Rows.Count - 1)("Kontakt- Adresse") = _Customer.CustomerContact.Address
                    .Rows(.Rows.Count - 1)("Mailadresse Anzeigetext") = _Customer.CustomerContact.MailDisplay
                    .Rows(.Rows.Count - 1)("Mailadresse") = _Customer.CustomerContact.Mail
                    .Rows(.Rows.Count - 1)("Web-Adresse Anzeigetext") = _Customer.CustomerContact.WebDisplay
                    .Rows(.Rows.Count - 1)("Web-Adresse") = _Customer.CustomerContact.Web
                End If
                .Rows(.Rows.Count - 1)("Logo") = _Customer.CustomerStyle.LogoPath.ToString
                .Rows(.Rows.Count - 1)("Logo2") = _Customer.LogoPath2.ToString
                .Rows(.Rows.Count - 1)("Stylesheets") = _Customer.CustomerStyle.CssPath.ToString
                .Rows(.Rows.Count - 1)("Handbuch") = _Customer.DocuPath
                .Rows(.Rows.Count - 1)("Max. Anzahl Benutzer") = _Customer.MaxUser.ToString
                .Rows(.Rows.Count - 1)("Organisationsanzeige") = _Customer.ShowOrganization
                .Rows(.Rows.Count - 1)("Nur Kundengruppen administrieren") = _Customer.OrgAdminRestrictToCustomerGroup
            End With
            Return tblPar
        Catch ex As Exception
            m_App.WriteErrorText(1, m_User.UserName, "CustomerManagement", "SetOldLogParameters", ex.ToString)

            Dim dt As New DataTable()
            dt.Columns.Add("Fehler beim Erstellen der Log-Parameter", Type.GetType("String"))
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
                .Rows(.Rows.Count - 1)("Firmenname") = txtCustomerName.Text
                .Rows(.Rows.Count - 1)("KUNNR") = txtKUNNR.Text
                .Rows(.Rows.Count - 1)("Neues Kennwort nach n Tagen") = txtNewPwdAfterNDays.Text
                .Rows(.Rows.Count - 1)("Konto sperren nach n Fehlversuchen") = txtLockedAfterNLogins.Text
                .Rows(.Rows.Count - 1)("Mehrfaches Login") = chkAllowMultipleLogin.Checked
                .Rows(.Rows.Count - 1)("URL Remote Login") = chkAllowUrlRemoteLogin.Checked
                .Rows(.Rows.Count - 1)("Mindestlänge") = txtPwdLength.Text
                .Rows(.Rows.Count - 1)("n numerische Zeichen") = txtPwdNNumeric.Text
                .Rows(.Rows.Count - 1)("n Großbuchstaben") = txtNCapitalLetter.Text
                .Rows(.Rows.Count - 1)("n Sonderzeichen") = txtNSpecialCharacter.Text
                .Rows(.Rows.Count - 1)("Sperre letzte n Kennworte") = txtPwdHistoryNEntries.Text

                Dim strAnwendungen As String = ""
                Dim dvAppAssigned As DataView = GetViewAppsAssigned()
                For Each dRow As DataRowView In dvAppAssigned
                    strAnwendungen &= dRow("AppFriendlyName").ToString() & " | " & dRow("AppURL").ToString() & vbNewLine
                Next
                .Rows(.Rows.Count - 1)("Anwendungen") = strAnwendungen
                .Rows(.Rows.Count - 1)("Kontakt- Name") = txtCName.Text
                .Rows(.Rows.Count - 1)("Kontakt- Adresse") = TranslateHTML(EditCAddress.Content, TranslationDirection.SaveHTML) 'txtCAddress.Text
                .Rows(.Rows.Count - 1)("Mailadresse Anzeigetext") = txtCMailDisplay.Text
                .Rows(.Rows.Count - 1)("Mailadresse") = txtCMail.Text
                .Rows(.Rows.Count - 1)("Web-Adresse Anzeigetext") = txtCWebDisplay.Text
                .Rows(.Rows.Count - 1)("Web-Adresse") = txtCWeb.Text
                .Rows(.Rows.Count - 1)("Logo") = txtLogoPath.Text
                .Rows(.Rows.Count - 1)("Logo2") = txtLogoPath2.Text
                .Rows(.Rows.Count - 1)("Stylesheets") = txtCssPath.Text
                .Rows(.Rows.Count - 1)("Handbuch") = txtDocuPath.Text
                .Rows(.Rows.Count - 1)("Max. Anzahl Benutzer") = txtMaxUser.Text
                .Rows(.Rows.Count - 1)("Organisationsanzeige") = chkShowOrganization.Checked
                .Rows(.Rows.Count - 1)("Nur Kundengruppen administrieren") = cbxOrgAdminRestrictToCustomerGroup.Checked
            End With
            Return tblPar
        Catch ex As Exception
            m_App.WriteErrorText(1, m_User.UserName, "CustomerManagement", "SetNewLogParameters", ex.ToString)

            Dim dt As New DataTable()
            dt.Columns.Add("Fehler beim Erstellen der Log-Parameter", Type.GetType("String"))
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
            'CHC ITA 5968
            Try
                .Columns.Add("Status", Type.GetType("System.String"))
                .Columns.Add("Firmenname", Type.GetType("System.String"))
                .Columns.Add("KUNNR", Type.GetType("System.String"))
                .Columns.Add("DAD", Type.GetType("System.Boolean"))
                .Columns.Add("Neues Kennwort nach n Tagen", Type.GetType("System.String"))
                .Columns.Add("Konto sperren nach n Fehlversuchen", Type.GetType("System.String"))
                .Columns.Add("Mehrfaches Login", Type.GetType("System.Boolean"))
                .Columns.Add("URL Remote Login", Type.GetType("System.Boolean"))
                .Columns.Add("Mindestlänge", Type.GetType("System.String"))
                .Columns.Add("n numerische Zeichen", Type.GetType("System.String"))
                .Columns.Add("n Großbuchstaben", Type.GetType("System.String"))
                .Columns.Add("n Sonderzeichen", Type.GetType("System.String"))
                .Columns.Add("Sperre letzte n Kennworte", Type.GetType("System.String"))
                .Columns.Add("Anwendungen", Type.GetType("System.String"))
                .Columns.Add("Kontakt- Name", Type.GetType("System.String"))
                .Columns.Add("Kontakt- Adresse", Type.GetType("System.String"))
                .Columns.Add("Mailadresse Anzeigetext", Type.GetType("System.String"))
                .Columns.Add("Mailadresse", Type.GetType("System.String"))
                .Columns.Add("Web-Adresse Anzeigetext", Type.GetType("System.String"))
                .Columns.Add("Web-Adresse", Type.GetType("System.String"))
                .Columns.Add("Logo", Type.GetType("System.String"))

                '§§§ JVE 18.09.2006: Logo2
                .Columns.Add("Logo2", Type.GetType("System.String"))
                '-------------------------
                .Columns.Add("Stylesheets", Type.GetType("System.String"))
                .Columns.Add("Handbuch", Type.GetType("System.String"))
                .Columns.Add("Max. Anzahl Benutzer", Type.GetType("System.String"))
                .Columns.Add("Organisationsanzeige", Type.GetType("System.String"))
                .Columns.Add("Nur Kundengruppen administrieren", Type.GetType("System.String"))
            Catch
                .Columns.Add("Status", Type.GetType("System.String"))
                .Columns.Add("Firmenname", Type.GetType("System.String"))
                .Columns.Add("KUNNR", Type.GetType("System.String"))
                .Columns.Add("DAD", Type.GetType("System.Boolean"))
                .Columns.Add("Neues Kennwort nach n Tagen", Type.GetType("System.String"))
                .Columns.Add("Konto sperren nach n Fehlversuchen", Type.GetType("System.String"))
                .Columns.Add("Mehrfaches Login", Type.GetType("System.Boolean"))
                .Columns.Add("URL Remote Login", Type.GetType("System.Boolean"))
                .Columns.Add("Mindestlänge", Type.GetType("System.String"))
                .Columns.Add("n numerische Zeichen", Type.GetType("System.String"))
                .Columns.Add("n Großbuchstaben", Type.GetType("System.String"))
                .Columns.Add("n Sonderzeichen", Type.GetType("System.String"))
                .Columns.Add("Sperre letzte n Kennworte", Type.GetType("System.String"))
                .Columns.Add("Anwendungen", Type.GetType("System.String"))
                .Columns.Add("Kontakt- Name", Type.GetType("System.String"))
                .Columns.Add("Kontakt- Adresse", Type.GetType("System.String"))
                .Columns.Add("Mailadresse Anzeigetext", Type.GetType("System.String"))
                .Columns.Add("Mailadresse", Type.GetType("System.String"))
                .Columns.Add("Web-Adresse Anzeigetext", Type.GetType("System.String"))
                .Columns.Add("Web-Adresse", Type.GetType("System.String"))
                .Columns.Add("Logo", Type.GetType("System.String"))

                '§§§ JVE 18.09.2006: Logo2
                .Columns.Add("Logo2", Type.GetType("System.String"))
                '-------------------------
                .Columns.Add("Stylesheets", Type.GetType("System.String"))
                .Columns.Add("Handbuch", Type.GetType("System.String"))
                .Columns.Add("Max. Anzahl Benutzer", Type.GetType("System.String"))
                .Columns.Add("Organisationsanzeige", Type.GetType("System.String"))
                .Columns.Add("Nur Kundengruppen administrieren", Type.GetType("System.String"))
            End Try

        End With
        Return tblPar
    End Function

    Private Sub setKundenadministrationInfoVisibility()
        If rbKeine.Checked Then
            lblKundenadministrationInfo.Visible = False
            txtKundenadministrationBeschreibung.Visible = False
            lblKundenadministrationContact.Visible = False
            EditKundenadministrationContact.Visible = False
        ElseIf rbeing.Checked Then
            lblKundenadministrationInfo.Visible = True
            txtKundenadministrationBeschreibung.Visible = True
            lblKundenadministrationContact.Visible = True
            EditKundenadministrationContact.Visible = True
        Else
            lblKundenadministrationInfo.Visible = False
            txtKundenadministrationBeschreibung.Visible = False
            lblKundenadministrationContact.Visible = True
            EditKundenadministrationContact.Visible = True
        End If
    End Sub

    ''' <summary>
    ''' Füllt die DropdownList mit den Login-Links
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub FillLoginLinks()

        Dim TempTable As New DataTable
        Dim cn As New SqlClient.SqlConnection(m_User.App.Connectionstring)
        cn.Open()

        Dim daLoginLink As SqlClient.SqlDataAdapter = New SqlClient.SqlDataAdapter("SELECT * FROM WebUserUploadLoginLink", cn)
        daLoginLink.Fill(TempTable)

        cn.Close()
        cn.Dispose()

        ddlPortalLink.Items.Add(New ListItem(" --Auswahl-- ", "0"))
        For Each row As DataRow In TempTable.Rows
            ddlPortalLink.Items.Add(New ListItem(row("Text").ToString(), row("ID").ToString()))
        Next

    End Sub

    ''' <summary>
    ''' Füllt die DropdownList mit den Portaltypen
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub FillPortalTypes()

        Dim TempTable As New DataTable
        Dim cn As New SqlClient.SqlConnection(m_User.App.Connectionstring)
        cn.Open()

        Dim daPortalType As SqlClient.SqlDataAdapter = New SqlClient.SqlDataAdapter("SELECT * FROM PortalTypes", cn)
        daPortalType.Fill(TempTable)

        cn.Close()
        cn.Dispose()

        ddlPortalType.Items.Add(New ListItem(""))
        For Each row As DataRow In TempTable.Rows
            ddlPortalType.Items.Add(New ListItem(row("PortalType").ToString()))
        Next

    End Sub

    ''' <summary>
    ''' Füllt die DropdownListen mit den Referenzfeldtypen
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub FillReferenceTypes()

        Dim TempTable As New DataTable
        Dim cn As New SqlClient.SqlConnection(m_User.App.Connectionstring)
        cn.Open()

        Dim daPortalType As SqlClient.SqlDataAdapter = New SqlClient.SqlDataAdapter("SELECT * FROM ReferenzTypen", cn)
        daPortalType.Fill(TempTable)

        cn.Close()
        cn.Dispose()

        ddlReferenzTyp1.Items.Add(New ListItem("", ""))
        ddlReferenzTyp2.Items.Add(New ListItem("", ""))
        ddlReferenzTyp3.Items.Add(New ListItem("", ""))
        ddlReferenzTyp4.Items.Add(New ListItem("", ""))

        For Each row As DataRow In TempTable.Rows
            Dim strTyp As String = row("ReferenzTyp").ToString()
            Dim strName As String = row("ReferenzTypName").ToString()
            If String.IsNullOrEmpty(strName) Then
                strName = strTyp
            End If
            Dim blnCheckbox As Boolean = (Not IsDBNull(row("IstCheckbox")) AndAlso CBool(row("IstCheckbox")))

            If blnCheckbox Then
                ddlReferenzTyp4.Items.Add(New ListItem(strName, strTyp))
            Else
                ddlReferenzTyp1.Items.Add(New ListItem(strName, strTyp))
                ddlReferenzTyp2.Items.Add(New ListItem(strName, strTyp))
                ddlReferenzTyp3.Items.Add(New ListItem(strName, strTyp))
            End If
        Next

    End Sub

    Private Sub FillMvcSelectionTypes()

        ddlMvcSelectionType.Items.Add(New ListItem(""))
        ddlMvcSelectionType.Items.Add(New ListItem("Anwendungs-Favoriten", "Favorites"))
        ddlMvcSelectionType.Items.Add(New ListItem("Selection URL", "Url"))
        ddlMvcSelectionType.Items.Add(New ListItem("Dashboard", "Dashboard"))

    End Sub

    Private Sub FillCustomerSettingsList(ByVal index As Integer)

        If index < 0 Then
            Return
        End If

        Dim dtCustSettings As DataTable = GetAllCustomerSetting(CInt(ihCustomerID.Value), lstCustomerSettings.Items(index).Value)

        If Not dtCustSettings Is Nothing Then
            gvCustomerSettings.DataSource = dtCustSettings
            gvCustomerSettings.DataBind()
        End If

        For Each row As GridViewRow In gvCustomerSettings.Rows
            Try

                CType(row.Cells(6).Controls(0), ImageButton).Attributes.Add("onclick", "if(!confirm('Löschen bestätigen!')) return false;")

            Catch ex As Exception

            End Try
        Next

    End Sub

    Public Function GetEncodetString(ByVal src As String) As String

        Dim enc As Encoding = Encoding.Default
        Dim ret As Encoding = Encoding.UTF8
        Dim isoBytes As Byte() = enc.GetBytes(src)

        Return ret.GetString(isoBytes)

    End Function

    Private Function HandleFileUploaded(e As FileUploadedEventArgs, virtualPath As String, currentLogo As String, deleteExistingLogos As Boolean) As String
        LogoDebug.Text &= "1. Start upload<br>"
        Try
            'absoluten Pfad ermitteln
            Dim logoPhysUploadPath As String = Server.MapPath(virtualPath)
            LogoDebug.Text &= "2. MapPath: " & logoPhysUploadPath & "<br>"
            Dim physRoot As String = Server.MapPath("/")
            LogoDebug.Text &= "3. PhysRoot: " & physRoot & "<br>"
            Dim timeStamp As String = String.Format("{0:yyyyMMddhhmmss}", DateTime.Now)
            Dim tmpFilename As String = String.Format("{0}_{1}_Logo", ihCustomerID.Value, txtKUNNR.Text)

            If uploadTotalBytes < UploadMaxTotalBytes Then
                LogoDebug.Text &= "4. Bild ist nicht zu groß<br>"
                ' Total bytes limit has not been reached, accept the file
                e.IsValid = True
                uploadTotalBytes += e.File.ContentLength

                Dim fi As FileInfo = New FileInfo(e.File.FileName)

                LogoDebug.Text &= "5. FileInfo:" & fi.ToString() & "<br>"

                If Not Directory.Exists(logoPhysUploadPath) Then
                    Directory.CreateDirectory(logoPhysUploadPath)
                End If

                Dim newFileName As String = Path.Combine(logoPhysUploadPath, tmpFilename + "_" + timeStamp + fi.Extension)

                LogoDebug.Text &= "6. FileName:" & newFileName & "<br>"

                Dim currentLogoPhys As String = If(String.IsNullOrEmpty(currentLogo), String.Empty, HttpContext.Current.Server.MapPath(currentLogo))

                LogoDebug.Text &= "7a. Altes Logo: " & currentLogoPhys & "<br>"

                If deleteExistingLogos Then
                    Try
                        'Löschen existierender Logos (Alle Dateitypen alle zeitstempel)
                        Dim aFiles() As String = Directory.GetFiles(logoPhysUploadPath, "*" + tmpFilename + "*")
                        Array.ForEach(aFiles, Sub(item)
                                                  'existierende Logos (außer aktuelles) löschen
                                                  If File.Exists(item) AndAlso Not item.Equals(currentLogoPhys) Then
                                                      File.Delete(item)
                                                  End If
                                              End Sub)
                    Catch
                        LogoDebug.Text &= "7b. Fehler beim löschen des alten Logos<br>"
                    End Try
                End If

                e.File.SaveAs(newFileName)

                ' physical to virtual path..
                LogoDebug.Text &= "8. Fertig<br>"
                Return newFileName.Replace(physRoot, "/").Replace("\", "/")
            Else
                ' Limit reached, discard the file
                e.IsValid = False
            End If

        Catch ex As Exception
            LogoError.Text &= "Upload fehlgeschlagen!<br>"

        End Try

        Return Nothing
    End Function

    Private Sub SyncImageUrlToPath(txt As TextBox, img As Image)
        Try
            If String.IsNullOrEmpty(txt.Text) Then
                Return
            End If

            Dim sLogoPath As String = VirtualPathUtility.ToAbsolute(txt.Text.Replace("../", "~/"))
            Dim sPhysPath As String = HttpContext.Current.Server.MapPath(sLogoPath)

            If File.Exists(sPhysPath) Then
                img.ImageUrl = sLogoPath
                img.Style.Clear()
            Else
                img.ImageUrl = String.Empty
                img.Style.Clear()
            End If

        Catch ex As Exception

        End Try
    End Sub

    Private Sub insertIntoCustomerInfo()
        Dim cn As New SqlClient.SqlConnection(ConfigurationManager.AppSettings("Connectionstring"))
        Dim cmd As New SqlClient.SqlCommand
        Try
            cn.Open()
            cmd.Connection = cn
            cmd.CommandType = CommandType.Text
            Dim SqlQuery As String

            SqlQuery = "INSERT INTO [CustomerInfo] (CustomerID, PersonTyp, Name, Vorname, Email, Telefon, Telefax) VALUES (@CustomerID, @PersonTyp, @Name, @Vorname, @Email, @Telefon, @Telefax);"
            With cmd
                .Parameters.AddWithValue("@CustomerID", ihCustomerID.Value)
                .Parameters.AddWithValue("@PersonTyp", rblPersonType.SelectedValue)
                .Parameters.AddWithValue("@Name", txtName.Text)
                .Parameters.AddWithValue("@Vorname", txtVorname.Text)
                .Parameters.AddWithValue("@Email", txtEmail.Text)
                .Parameters.AddWithValue("@Telefon", txtTelefon.Text)
                .Parameters.AddWithValue("@Telefax", txtTelefax.Text)

            End With
            cmd.CommandText = SqlQuery
            cmd.ExecuteNonQuery()
        Catch ex As Exception
            Throw New Exception("SCHREIBEN EINES EINTRAGES IN DIE DB: TABELLENNAME=  CustomerInfo  \ " & ex.Message & " \ " & ex.StackTrace)
        Finally
            cn.Close()
        End Try
    End Sub

    Private Sub Bilderwahl_Switch(ByVal editierbar As Boolean)
        If editierbar Then

            Fill_BilderDropDownListen()


            RadAsyncUpload1.Visible = True
            If txtLogoPath.Text <> "" Then
                imgLogoThumb.Style.Clear()
                imgLogoThumb.Style.Add("opacity", "1")
            End If
            If txtLogoPath2.Text <> "" Then
                Image_Buchungskreis_aktiv.Style.Clear()
                Image_Buchungskreis_aktiv.ImageUrl = txtLogoPath2.Text
                Image_Buchungskreis_aktiv.Style.Add("opacity", "1")
            End If
            If txtHeaderBackgroundPath.Text <> "" Then
                Image_HeaderBackgroundPath_aktiv.Style.Clear()
                Image_HeaderBackgroundPath_aktiv.ImageUrl = txtHeaderBackgroundPath.Text
                Image_HeaderBackgroundPath_aktiv.Style.Add("opacity", "1")
            End If
        Else
            RadAsyncUpload1.Visible = False
            If txtLogoPath.Text <> "" Then
                imgLogoThumb.Style.Clear()
                imgLogoThumb.Style.Add("opacity", "0.5")
            End If
            If txtLogoPath2.Text <> "" Then
                Image_Buchungskreis_aktiv.Style.Clear()
                Image_Buchungskreis_aktiv.ImageUrl = txtLogoPath2.Text
                Image_Buchungskreis_aktiv.Style.Add("opacity", "0.5")
            End If
            If txtHeaderBackgroundPath.Text <> "" Then
                Image_HeaderBackgroundPath_aktiv.Style.Clear()
                Image_HeaderBackgroundPath_aktiv.ImageUrl = txtHeaderBackgroundPath.Text
                Image_HeaderBackgroundPath_aktiv.Style.Add("opacity", "0.5")
            End If
        End If

    End Sub

    Private Function getKundenInfoDT(ByVal PersonType As String) As DataTable


        Dim cn As New SqlClient.SqlConnection(ConfigurationManager.AppSettings("Connectionstring"))
        Try
            getKundenInfoDT = New DataTable
            If cn.State = ConnectionState.Closed Then
                cn.Open()
            End If

            Dim daApp As New SqlClient.SqlDataAdapter("SELECT *" & _
                                                        "FROM CustomerInfo " & _
                                                      "Where CustomerID = '" & ihCustomerID.Value & "' AND PersonTyp = '" & PersonType & "'", cn)
            daApp.Fill(getKundenInfoDT)
        Finally
            If cn.State <> ConnectionState.Closed Then
                cn.Close()
            End If
        End Try

        Return getKundenInfoDT


    End Function

    Private Sub deleteFromCustomerInfoTable(ByVal ID As String)

        Dim cn As New SqlClient.SqlConnection(ConfigurationManager.AppSettings("Connectionstring"))
        Dim cmd As New SqlClient.SqlCommand
        Dim sqlQuery As String
        Try
            cmd.Connection = cn
            If cn.State = ConnectionState.Closed Then
                cn.Open()
            End If

            sqlQuery = "Delete FROM CustomerInfo WHERE ID=@ID;"
            cmd.Parameters.AddWithValue("@ID", ID)

            cmd.CommandText = sqlQuery
            cmd.ExecuteNonQuery()
        Finally
            If cn.State <> ConnectionState.Closed Then
                cn.Close()
            End If
        End Try
    End Sub

    Private Sub fillBusinessownerGrid(ByVal intPageIndex As Int32, Optional ByVal strSort As String = "")

        Dim tmpDataView As New DataView(getKundenInfoDT("Businessowner"))


        Dim intTempPageIndex As Int32 = intPageIndex
        Dim strTempSort As String = ""
        Dim strDirection As String = ""

        If strSort.Trim(" "c).Length > 0 Then
            intTempPageIndex = 0
            strTempSort = strSort.Trim(" "c)
            If (ViewState("Sort") Is Nothing) OrElse (ViewState("Sort").ToString = strTempSort) Then
                If ViewState("Direction") Is Nothing Then
                    strDirection = "desc"
                Else
                    strDirection = ViewState("Direction").ToString
                End If
            Else
                strDirection = "desc"
            End If

            If strDirection = "asc" Then
                strDirection = "desc"
            Else
                strDirection = "asc"
            End If

            ViewState("Sort") = strTempSort
            ViewState("Direction") = strDirection
        Else
            If Not ViewState("Sort") Is Nothing Then
                strTempSort = ViewState("Sort").ToString
                If ViewState("Direction") Is Nothing Then
                    strDirection = "asc"
                    ViewState("Direction") = strDirection
                Else
                    strDirection = ViewState("Direction").ToString
                End If
            End If
        End If

        If Not strTempSort.Length = 0 Then
            tmpDataView.Sort = strTempSort & " " & strDirection
        End If


        gvBusinessOwner.PageIndex = intTempPageIndex

        gvBusinessOwner.DataSource = tmpDataView

        gvBusinessOwner.DataBind()

        If gvBusinessOwner.PageCount > 1 Then
            gvBusinessOwner.PagerStyle.CssClass = "PagerStyle"
            gvBusinessOwner.PagerSettings.Visible = True
        Else
            gvBusinessOwner.PagerSettings.Visible = False
        End If

    End Sub

    Private Sub fillAdminpersonGrid(ByVal intPageIndex As Int32, Optional ByVal strSort As String = "")

        Dim tmpDataView As New DataView(getKundenInfoDT("Adminperson"))


        Dim intTempPageIndex As Int32 = intPageIndex
        Dim strTempSort As String = ""
        Dim strDirection As String = ""

        If strSort.Trim(" "c).Length > 0 Then
            intTempPageIndex = 0
            strTempSort = strSort.Trim(" "c)
            If (ViewState("Sort") Is Nothing) OrElse (ViewState("Sort").ToString = strTempSort) Then
                If ViewState("Direction") Is Nothing Then
                    strDirection = "desc"
                Else
                    strDirection = ViewState("Direction").ToString
                End If
            Else
                strDirection = "desc"
            End If

            If strDirection = "asc" Then
                strDirection = "desc"
            Else
                strDirection = "asc"
            End If

            ViewState("Sort") = strTempSort
            ViewState("Direction") = strDirection
        Else
            If Not ViewState("Sort") Is Nothing Then
                strTempSort = ViewState("Sort").ToString
                If ViewState("Direction") Is Nothing Then
                    strDirection = "asc"
                    ViewState("Direction") = strDirection
                Else
                    strDirection = ViewState("Direction").ToString
                End If
            End If
        End If

        If Not strTempSort.Length = 0 Then
            tmpDataView.Sort = strTempSort & " " & strDirection
        End If


        gvAdminPerson.PageIndex = intTempPageIndex

        gvAdminPerson.DataSource = tmpDataView

        gvAdminPerson.DataBind()

        If gvAdminPerson.PageCount > 1 Then
            gvAdminPerson.PagerStyle.CssClass = "PagerStyle"
            gvAdminPerson.PagerSettings.Visible = True
        Else
            gvAdminPerson.PagerSettings.Visible = False
        End If


    End Sub

#End Region

#Region " Events "

    Private Sub btnSuche_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSuche.Click
        Search(True, True, True, True)
        If dgSearchResult.Rows.Count = 0 Then
            lblError.Text = "Keine Datensätze gefunden."
        End If
    End Sub

    Private Sub lbtnCancel_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lbtnCancel.Click
        ClearEdit()
        Search(True, True)
    End Sub

    Private Sub lbtnNew_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lbtnNew.Click

        SearchMode(False)
        ClearEdit()
        Dim cn As New SqlClient.SqlConnection(m_User.App.Connectionstring)
        Try
            cn.Open()
            FillApps(CInt(ihCustomerID.Value), "", cn)
            FillArchivesUnAssigned(CInt(ihCustomerID.Value), cn)
            'hier nicht den Wert aus dem Grid, ist logischer Weise 0, sondern des Users Kundennummer
            FillAccountingArea(m_User.Customer.CustomerId, True)
        Finally
            If cn.State <> ConnectionState.Closed Then
                cn.Close()
            End If
        End Try
    End Sub

    Private Sub lbtnOpenCopyOptions_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lbtnOpenCopyOptions.Click

        keepApplications.Checked = False
        copyOptions.Show()
    End Sub

    Private Sub lbtnCopy_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lbtnCopy.Click

        ' Reiter Kundendaten
        ihCustomerID.Value = "-1"
        txtCustomerName.Text = String.Empty
        txtKUNNR.Text = String.Empty
        ddlAccountingArea.SelectedIndex = 0
        ddlPortalLink.SelectedIndex = 0
        ddlPortalType.SelectedValue = ""
        cbxForceSpecifiedLoginLink.Checked = False
        txtLogoutLink.Text = ""
        ddlReferenzTyp1.SelectedValue = ""
        ddlReferenzTyp2.SelectedValue = ""
        ddlReferenzTyp3.SelectedValue = ""
        ddlReferenzTyp4.SelectedValue = ""
        txtMvcSelectionUrl.Text = String.Empty
        ddlMvcSelectionType.SelectedValue = ""
        chkKundenSperre.Checked = False
        chkTeamviewer.Checked = False
        txtCName.Text = String.Empty
        EditCAddress.Content = String.Empty
        txtCMailDisplay.Text = String.Empty
        txtCMail.Text = String.Empty
        txtCWebDisplay.Text = String.Empty
        txtCWeb.Text = String.Empty
        txtKundepostfach.Text = String.Empty
        txtKundenhotline.Text = String.Empty
        txtKundenfax.Text = String.Empty

        ' Reiter IP-Adressen und Style
        txtLogoPath.Text = String.Empty
        RadAsyncUpload1.UploadedFiles.Clear()
        imgLogoThumb.ImageUrl = String.Empty
        txtLogoPath2.Text = String.Empty
        imageDDL.Items.Clear()
        RadAsyncUpload2.UploadedFiles.Clear()
        Image_Buchungskreis_aktiv.ImageUrl = String.Empty
        txtHeaderBackgroundPath.Text = String.Empty
        imageHeaderDDL.Items.Clear()
        RadAsyncUpload3.UploadedFiles.Clear()
        Image_HeaderBackgroundPath_aktiv.ImageUrl = String.Empty
        txtDocuPath.Text = String.Empty

        ' Reiter Archive
        Dim assignedArchives = lstArchivAssigned.Items.Cast(Of ListItem).ToArray
        For Each i As ListItem In assignedArchives
            lstArchivUnAssigned.Items.Add(i)
            lstArchivAssigned.Items.Remove(i)
        Next

        ' abhängig von keepApplications.Checked Applikationen beibehalten
        If Not keepApplications.Checked Then
            FillApps(CInt(ihCustomerID.Value), "", New SqlClient.SqlConnection(m_User.App.Connectionstring))
        End If

    End Sub

    Private Sub lbtnSave_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lbtnSave.Click

        Dim configSkipConfirmationKeyVal = "Admin_CustomerManagement_SkipSaveConfirmation"
        Dim configSkipConfirmationKey = ConfigurationManager.AppSettings.AllKeys.FirstOrDefault(Function(r) r = configSkipConfirmationKeyVal)
        If (Not configSkipConfirmationKey Is Nothing) Then
            If (ConfigurationManager.AppSettings(configSkipConfirmationKeyVal).ToString.ToLower = "true") Then
                lbtnConfirm_Click(Nothing, Nothing)
                Return
            End If
        End If

        'Do SAP-Stuff here...
        Dim i_Kunnr As String = Right("0000000000" & txtKUNNR.Text, 10)
        Dim blnNoData As Boolean = False
        Dim tblTemp2 As DataTable = Nothing

        Try

            Session("AppID") = "00000" ' Logging (inside callBapi) needs a value here..
            Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_M_KUNDEN_ANZEIGEN", m_App, m_User, Me)

            myProxy.setImportParameter("I_KUNNR", i_Kunnr)

            myProxy.callBapi()

            tblTemp2 = myProxy.getExportTable("GS_WEB")
        Catch ex As Exception
            If CastSapBizTalkErrorMessage(ex.Message) = "NO_DATA" Then
                blnNoData = True
            Else
                plhConfirm.Controls.Add(New LiteralControl(String.Concat("<br /><b>Beim Abfragen der SAP-Daten ist ein Fehler aufgetreten:<br />", ex.Message, "</b>")))
                Return
            End If
        Finally
            Session("AppID") = Nothing
        End Try

        If blnNoData OrElse tblTemp2 Is Nothing OrElse tblTemp2.Rows.Count = 0 Then
            plhConfirm.Controls.Add(New LiteralControl("<BR><b>Keine Daten gefunden!<b/><BR><BR>"))
        Else
            Dim sb = New StringBuilder()
            With sb
                .AppendFormat("KUNNR:&nbsp{0}<BR>", tblTemp2.Rows(0)("Kunnr").ToString)
                .AppendFormat("NAME1:&nbsp{0}<BR>", tblTemp2.Rows(0)("Name1").ToString)
                .AppendFormat("NAME2:&nbsp{0}<BR>", tblTemp2.Rows(0)("Name2").ToString)
                .AppendFormat("STRAS:&nbsp{0}<BR>", tblTemp2.Rows(0)("Stras").ToString)
                .AppendFormat("PSTLZ:&nbsp{0}<BR>", tblTemp2.Rows(0)("Pstlz").ToString)
                .AppendFormat("ORT01:&nbsp{0}<BR>", tblTemp2.Rows(0)("Ort01").ToString)
            End With
            plhConfirm.Controls.Add(New LiteralControl(String.Format("<BR><b>Bitte Überprüfen Sie vor dem Bestätigen Ihre Eingaben und vergleichen Sie diese mit dem Kundendatensatz aus SAP:</b><BR><BR>{0}<BR>", sb.ToString)))
        End If

        ConfirmMode(True)

        confirmWindow.Show()
    End Sub

    Private Sub lbtnDelete_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lbtnDelete.Click
        Dim tblLogParameter As DataTable
        Dim cn As New SqlClient.SqlConnection(m_User.App.Connectionstring)
        Try

            Dim _customer As New Customer(CInt(ihCustomerID.Value))

            cn.Open()
            tblLogParameter = SetOldLogParameters(CInt(ihCustomerID.Value))
            If Not _customer.HasUser(cn) Then
                _customer.Delete(cn)
                Log(_customer.CustomerId.ToString, "Firma löschen", tblLogParameter)

                Search(True, True, True, True)
                lblMessage.Text = "Der Kunde wurde gelöscht."
            Else
                plhConfirm.Controls.Add(New LiteralControl("<b>Der Kunde kann nicht gelöscht werden, da ihm noch Benutzer zugeordnet sind.</b>"))
            End If
        Catch ex As Exception
            m_App.WriteErrorText(1, m_User.UserName, "CustomerManagement", "lbtnDelete_Click", ex.ToString)

            Dim msg = ex.Message
            If Not ex.InnerException Is Nothing Then
                msg &= ": " & ex.InnerException.Message
            End If
            tblLogParameter = New DataTable
            Log(ihCustomerID.Value, msg, tblLogParameter, "ERR")
            plhConfirm.Controls.Add(New LiteralControl(msg))
        Finally
            If cn.State <> ConnectionState.Closed Then
                cn.Close()
            End If
        End Try
    End Sub

    Private Sub lbtnConfirm_Click(ByVal sender As Object, ByVal e2 As EventArgs) Handles lbtnConfirm.Click
        Dim tblLogParameter As DataTable
        Dim cn As New SqlClient.SqlConnection(m_User.App.Connectionstring)

        Dim StrDaysLockMessage As String
        Dim StrDaysDelMessage As String

        Try

            cn.Open()
            Dim intCustomerId As Integer = CInt(ihCustomerID.Value)
            Dim strLogMsg As String = "Firma anlegen"
            If Not (intCustomerId = -1) Then
                strLogMsg = "Firma ändern"
                tblLogParameter = SetOldLogParameters(intCustomerId)
            End If
            Dim KundenAdministration As Integer
            If rbKeine.Checked = True Then
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
                                                txtHeaderBackgroundPath.Text, _
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
                                                False, _
                                                chkIpRestriction.Checked, _
                                                txtIpStandardUser.Text, _
                                                CInt(ddlAccountingArea.SelectedValue), _
                                                KundenAdministration, _
                                                txtKundenadministrationBeschreibung.Text, _
                                                TranslateHTML(EditKundenadministrationContact.Content, TranslationDirection.SaveHTML), _
                                                chkKundenSperre.Checked, _
                                                chkTeamviewer.Checked, _
                                                cbxUsernameSendEmail.Checked, _
                                                CInt(ddlPortalLink.SelectedValue), _
                                                ddlPortalType.SelectedValue, _
                                                cbxForceSpecifiedLoginLink.Checked, _
                                                txtLogoutLink.Text, _
                                                ddlReferenzTyp1.SelectedValue, _
                                                ddlReferenzTyp2.SelectedValue, _
                                                ddlReferenzTyp3.SelectedValue, _
                                                ddlReferenzTyp4.SelectedValue, _
                                                strSDCustomerNumber:=txtSDCustomerNumber.Text, _
                                                strSDUserName:=txtSDUserName.Text, _
                                                strSDPassword:=txtSDPassword.Text, strSDUserLogin:=txtSDLoginName.Text, _
                                                strSDSignatur:=txtSDSignatur.Text, strSDSignatur2:=txtSDSignatur2.Text, _
                                                strMvcSelectionUrl:=txtMvcSelectionUrl.Text, _
                                                strMvcSelectionType:=ddlMvcSelectionType.SelectedValue)
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

            tblLogParameter = SetNewLogParameters()
            Log(_customer.CustomerId.ToString, strLogMsg, tblLogParameter)

            'Anwendungen zuordnen
            Dim dvAppsOriginallyAssigned As New DataView
            If intCustomerId = -1 Then
                intCustomerId = _customer.CustomerId
                ihCustomerID.Value = intCustomerId.ToString
            Else
                Dim AppsAssigned As New ApplicationList(intCustomerId, cn)
                AppsAssigned.GetAssigned()
                dvAppsOriginallyAssigned = AppsAssigned.DefaultView
            End If
            Dim dvAppsAssigned As DataView = GetViewAppsAssigned()
            Dim lstAssignedApps As New List(Of String)
            For Each row As DataRowView In dvAppsAssigned
                lstAssignedApps.Add(row("AppID").ToString())
            Next
            Dim _assignment As New Kernel.AppAssignments(intCustomerId, Kernel.AssignmentType.Customer)
            _assignment.Save(dvAppsOriginallyAssigned, lstAssignedApps, cn)

            'Archive zuordnen
            Dim dvArchivAssigned As New DataView
            If intCustomerId = -1 Then
                intCustomerId = _customer.CustomerId
                ihCustomerID.Value = intCustomerId.ToString
            Else
                If Not Session("myCustomerArchivAssigned") Is Nothing Then
                    dvArchivAssigned = CType(Session("myCustomerArchivAssigned"), DataView)
                Else
                    dvArchivAssigned = GetArchivAssignedView(intCustomerId, cn)
                End If
            End If
            Dim _archivassignment As New Kernel.ArchivAssignments(intCustomerId, Kernel.AssignmentType.Customer)
            _archivassignment.Save(dvArchivAssigned, lstArchivAssigned.Items, cn)


            ' Rechte zuordnen
            SaveRightsForCustomer()

            Search(True, True, , True)




            lblMessage.Text = StrDaysLockMessage & StrDaysDelMessage & "Die Änderungen wurden gespeichert."
        Catch ex As Exception
            m_App.WriteErrorText(1, m_User.UserName, "CustomerManagement", "lbtnSave_Click", ex.ToString)

            lblError.Text = ex.Message
            If Not ex.InnerException Is Nothing Then
                lblError.Text &= ": " & ex.InnerException.Message
            End If
            tblLogParameter = New DataTable
            Log(ihCustomerID.Value, lblError.Text, tblLogParameter, "ERR")
        Finally
            If cn.State <> ConnectionState.Closed Then
                cn.Close()
            End If
        End Try
    End Sub

    Private Sub lbtnCancelConfirm_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lbtnCancelConfirm.Click
        ConfirmMode(False)
        Bilderwahl_Switch(True)
        confirmWindow.Hide()
    End Sub

    Private Sub btnNewIpAddress_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnNewIpAddress.Click
        If (Not ihCustomerID.Value = "-1") And (Not txtIpAddress.Text.Trim(" "c).Length = 0) Then
            Dim cn As New SqlClient.SqlConnection(m_User.App.Connectionstring)
            Try

                cn.Open()

                Dim cmdNew As New SqlClient.SqlCommand("INSERT INTO IpAddresses VALUES (" & ihCustomerID.Value & ",'" & txtIpAddress.Text & "')", cn)
                cmdNew.ExecuteNonQuery()
                EditEditMode(CInt(ihCustomerID.Value))
                txtIpAddress.Text = ""
            Catch ex As Exception
                lblError.Text = ex.Message
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

    Private Sub dgSearchResult_RowCommand(ByVal sender As Object, ByVal e As GridViewCommandEventArgs) Handles dgSearchResult.RowCommand

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

    Private Sub dgSearchResult_RowEditing(ByVal sender As Object, ByVal e As GridViewEditEventArgs) Handles dgSearchResult.RowEditing

    End Sub

    Private Sub dgSearchResult_Sorting(ByVal sender As Object, ByVal e As GridViewSortEventArgs) Handles dgSearchResult.Sorting
        Dim strSort As String = e.SortExpression
        If Not ViewState("ResultSort") Is Nothing AndAlso ViewState("ResultSort").ToString = strSort Then
            strSort &= " DESC"
        End If
        ViewState("ResultSort") = strSort
        FillDataGrid(strSort)
    End Sub

    Private Sub btnDelKundenLogo_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnDelKundenLogo.Click
        Try

            If Not String.IsNullOrEmpty(txtLogoPath.Text) Then
                'absoluten Pfad ermitteln
                Dim sLogoPath As String = txtLogoPath.Text
                Dim sPhysPath As String = HttpContext.Current.Server.MapPath(sLogoPath)

                If File.Exists(sPhysPath) Then
                    File.Delete(sPhysPath)
                    txtLogoPath.Text = ""

                    Fill_BilderDropDownListen()
                Else
                    txtLogoPath.Text = ""
                    lblError.Text = "Fehler beim Löschen des Kundenlogos: Datei nicht auf dem Server gefunden."
                End If
            End If

        Catch ex As Exception
            m_App.WriteErrorText(1, m_User.UserName, "CustomerManagement", "btnDelKundenLogo_Click", ex.ToString)
            lblError.Text = ex.Message
        End Try
    End Sub

    Private Sub btnDelBukrsLogo_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnDelBukrsLogo.Click
        Try

            If Not String.IsNullOrEmpty(txtLogoPath2.Text) Then
                'absoluten Pfad ermitteln
                Dim sLogoPath As String = txtLogoPath2.Text
                Dim sPhysPath As String = HttpContext.Current.Server.MapPath(sLogoPath)

                If File.Exists(sPhysPath) Then
                    File.Delete(sPhysPath)
                    txtLogoPath2.Text = ""

                    Fill_BilderDropDownListen()
                Else
                    txtLogoPath2.Text = ""
                    lblError.Text = "Fehler beim Löschen des Mandantenlogos: Datei nicht auf dem Server gefunden."
                End If
            End If

        Catch ex As Exception
            m_App.WriteErrorText(1, m_User.UserName, "CustomerManagement", "btnDelBukrsLogo_Click", ex.ToString)
            lblError.Text = ex.Message
        End Try
    End Sub

    Private Sub btnDelHeaderLogo_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnDelHeaderLogo.Click
        Try

            If Not String.IsNullOrEmpty(txtHeaderBackgroundPath.Text) Then
                'absoluten Pfad ermitteln
                Dim sLogoPath As String = txtHeaderBackgroundPath.Text
                Dim sPhysPath As String = HttpContext.Current.Server.MapPath(sLogoPath)

                If File.Exists(sPhysPath) Then
                    File.Delete(sPhysPath)
                    txtHeaderBackgroundPath.Text = ""

                    Fill_BilderDropDownListen()
                Else
                    txtHeaderBackgroundPath.Text = ""
                    lblError.Text = "Fehler beim Löschen des Headerlogos: Datei nicht auf dem Server gefunden."
                End If
            End If

        Catch ex As Exception
            m_App.WriteErrorText(1, m_User.UserName, "CustomerManagement", "btnDelHeaderLogo_Click", ex.ToString)
            lblError.Text = ex.Message
        End Try
    End Sub

    Protected Sub btnAssign_Click(ByVal sender As Object, ByVal e As ImageClickEventArgs) Handles btnAssign.Click
        Dim chkSelect As CheckBox

        For Each item As GridDataItem In rgAppUnAssigned.Items
            chkSelect = item("Auswahl").FindControl("chkSelect")
            If chkSelect IsNot Nothing AndAlso chkSelect.Checked Then
                tblApps.Select("AppID='" & item("AppID").Text & "'")(0)("Assigned") = "X"
                lstCustomerSettings.Items.Add(New ListItem(item("AppFriendlyName").Text, item("AppID").Text))
            End If
        Next

        Session("custMgmtApps") = tblApps
        rgAppUnAssigned.Rebind()
        rgAppAssigned.Rebind()
    End Sub

    Protected Sub btnUnAssign_Click(ByVal sender As Object, ByVal e As ImageClickEventArgs) Handles btnUnAssign.Click
        Dim chkSelect As CheckBox

        For Each item As GridDataItem In rgAppAssigned.Items
            chkSelect = item("Auswahl").FindControl("chkSelect")
            If chkSelect IsNot Nothing AndAlso chkSelect.Checked Then
                tblApps.Select("AppID='" & item("AppID").Text & "'")(0)("Assigned") = ""
                With lstCustomerSettings.Items
                    .Remove(.FindByValue(item("AppID").Text))
                End With
            End If
        Next

        Session("custMgmtApps") = tblApps
        rgAppUnAssigned.Rebind()
        rgAppAssigned.Rebind()
    End Sub

    Protected Sub btnAssignArchiv_Click(ByVal sender As Object, ByVal e As ImageClickEventArgs) Handles btnAssignArchiv.Click
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

    Protected Sub btnUnAssignArchiv_Click(ByVal sender As Object, ByVal e As ImageClickEventArgs) Handles btnUnAssignArchiv.Click
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

    Protected Sub btnEmpty_Click(ByVal sender As Object, ByVal e As ImageClickEventArgs) Handles btnEmpty.Click
        btnSuche_Click(sender, e)
    End Sub

    Private Sub Repeater1_ItemCommand(ByVal source As Object, ByVal e As RepeaterCommandEventArgs) Handles Repeater1.ItemCommand
        Dim cn As New SqlClient.SqlConnection(m_User.App.Connectionstring)
        Try
            cn.Open()

            Dim cmdNew As New SqlClient.SqlCommand("DELETE FROM IpAddresses WHERE CustomerID=" & ihCustomerID.Value & " AND IpAddress='" & e.CommandArgument & "'", cn)
            cmdNew.ExecuteNonQuery()
            EditEditMode(CInt(ihCustomerID.Value))
            txtIpAddress.Text = ""
        Finally
            If cn.State <> ConnectionState.Closed Then
                cn.Close()
            End If
        End Try
    End Sub

    Protected Sub rbKeine_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs) Handles rbKeine.CheckedChanged
        setKundenadministrationInfoVisibility()
    End Sub

    Protected Sub rbvollst_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs) Handles rbvollst.CheckedChanged
        setKundenadministrationInfoVisibility()
    End Sub

    Protected Sub rbeing_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs) Handles rbeing.CheckedChanged
        setKundenadministrationInfoVisibility()
    End Sub

    Private Sub lstArchivAssigned_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles lstArchivAssigned.SelectedIndexChanged
        For Each item As ListItem In lstArchivAssigned.Items
            item.Attributes.Add("title", item.Text)
        Next
    End Sub

    Private Sub lstArchivUnAssigned_PreRender(ByVal sender As Object, ByVal e As EventArgs) Handles lstArchivUnAssigned.PreRender
        For Each item As ListItem In lstArchivUnAssigned.Items
            item.Attributes.Add("title", item.Text)
        Next
    End Sub

    Protected Sub lstCustomerSettings_SelectedIndexChanged(sender As Object, e As EventArgs) Handles lstCustomerSettings.SelectedIndexChanged
        FillCustomerSettingsList(lstCustomerSettings.SelectedIndex)
    End Sub

    Protected Sub gvCustomerSettings_RowEditing(sender As Object, e As GridViewEditEventArgs) Handles gvCustomerSettings.RowEditing

        txtKey.Text = gvCustomerSettings.Rows(e.NewEditIndex).Cells(1).Text
        txtValue.Text = gvCustomerSettings.Rows(e.NewEditIndex).Cells(2).Text
        txtDescript.Text = gvCustomerSettings.Rows.Item(e.NewEditIndex).Cells(3).Text
        lbOk.Text = "Ändern&#187"

    End Sub

    Protected Sub gvCustomerSettings_RowDeleting(sender As Object, e As GridViewDeleteEventArgs) Handles gvCustomerSettings.RowDeleting

        Dim cusid As String = CInt(ihCustomerID.Value)
        Dim appid As String = lstCustomerSettings.Items(lstCustomerSettings.SelectedIndex).Value
        Dim key As String = gvCustomerSettings.Rows(e.RowIndex).Cells(1).Text
        DeleteCustomerSetting(cusid, appid, key)
        FillCustomerSettingsList(lstCustomerSettings.SelectedIndex)

    End Sub

    Protected Sub lbOk_Click(sender As Object, e As EventArgs) Handles lbOk.Click

        If String.IsNullOrEmpty(txtKey.Text.Trim) Then
            txtKey.Focus()
            Return
        End If

        If String.IsNullOrEmpty(txtValue.Text.Trim) Then
            txtValue.Focus()
            Return
        End If

        lbOk.Text = "Hinzufügen&#187"

        SaveCustomerSetting(CInt(ihCustomerID.Value), lstCustomerSettings.Items(lstCustomerSettings.SelectedIndex).Value, txtKey.Text.Trim, txtValue.Text.Trim, txtDescript.Text.Trim)

        FillCustomerSettingsList(lstCustomerSettings.SelectedIndex)

        txtKey.Text = String.Empty
        txtValue.Text = String.Empty
        txtDescript.Text = String.Empty

    End Sub

    Public Sub RadAsyncUpload1_FileUploaded(ByVal sender As Object, ByVal e As FileUploadedEventArgs)
        Dim newLogoPath = HandleFileUploaded(e, logoVirtUploadPath, txtLogoPath.Text, True)
        If Not String.IsNullOrEmpty(newLogoPath) Then
            txtLogoPath.Text = newLogoPath
        End If
    End Sub

    Public Sub RadAsyncUpload2_FileUploaded(ByVal sender As Object, ByVal e As FileUploadedEventArgs)
        Dim newLogoPath = HandleFileUploaded(e, logoVirtUploadPath2, txtLogoPath2.Text, False)
        If Not String.IsNullOrEmpty(newLogoPath) Then
            txtLogoPath2.Text = newLogoPath
        End If
    End Sub

    Public Sub RadAsyncUpload3_FileUploaded(ByVal sender As Object, ByVal e As FileUploadedEventArgs)
        Dim newHeaderPath = HandleFileUploaded(e, logoVirtUploadPath3, txtHeaderBackgroundPath.Text, False)
        If Not String.IsNullOrEmpty(newHeaderPath) Then
            txtHeaderBackgroundPath.Text = newHeaderPath
        End If
    End Sub

    Protected Sub txtLogoPath_PreRender(sender As Object, e As EventArgs) Handles txtLogoPath.PreRender
        SyncImageUrlToPath(txtLogoPath, imgLogoThumb)
    End Sub

    Protected Sub txtLogoPath2_PreRender(sender As Object, e As EventArgs) Handles txtLogoPath2.PreRender
        SyncImageUrlToPath(txtLogoPath2, Image_Buchungskreis_aktiv)
    End Sub

    Protected Sub txtHeaderBackgroundPath_PreRender(sender As Object, e As EventArgs) Handles txtHeaderBackgroundPath.PreRender
        SyncImageUrlToPath(txtHeaderBackgroundPath, Image_HeaderBackgroundPath_aktiv)
    End Sub

    Protected Sub lbEintragen_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lbEintragen.Click
        If Not txtName.Text.Trim(" "c).Length = 0 AndAlso Not rblPersonType.SelectedIndex = -1 Then
            insertIntoCustomerInfo()
            txtName.Text = ""
            txtVorname.Text = ""
            txtEmail.Text = ""
            txtTelefax.Text = ""
            txtTelefon.Text = ""
            fillBusinessownerGrid(gvBusinessOwner.PageIndex)
            fillAdminpersonGrid(gvAdminPerson.PageIndex)
        End If

    End Sub

    Private Sub gvBusinessOwner_RowCommand(ByVal sender As Object, ByVal e As GridViewCommandEventArgs) Handles gvBusinessOwner.RowCommand
        If e.CommandName = "Sort" Then
            fillBusinessownerGrid(gvBusinessOwner.PageIndex, e.CommandArgument)
        ElseIf e.CommandName = "entfernen" Then
            deleteFromCustomerInfoTable(e.CommandArgument)
            fillBusinessownerGrid(gvBusinessOwner.PageIndex)
        End If

    End Sub

    Private Sub gvAdminPerson_RowCommand(ByVal sender As Object, ByVal e As GridViewCommandEventArgs) Handles gvAdminPerson.RowCommand
        If e.CommandName = "Sort" Then
            fillAdminpersonGrid(gvAdminPerson.PageIndex, e.CommandArgument)
        ElseIf e.CommandName = "entfernen" Then
            deleteFromCustomerInfoTable(e.CommandArgument)
            fillAdminpersonGrid(gvAdminPerson.PageIndex)
        End If

    End Sub

    Protected Sub rgAppUnAssigned_NeedDataSource(ByVal sender As Object, ByVal e As GridNeedDataSourceEventArgs) Handles rgAppUnAssigned.NeedDataSource
        rgAppUnAssigned.DataSource = GetViewAppsUnassigned()
    End Sub


    Protected Sub rgRights_NeedDataSource(ByVal sender As Object, ByVal e As GridNeedDataSourceEventArgs) Handles rgRights.NeedDataSource
        rgRights.DataSource = GetRights()

    End Sub



    Protected Sub rgAppAssigned_NeedDataSource(ByVal sender As Object, ByVal e As GridNeedDataSourceEventArgs) Handles rgAppAssigned.NeedDataSource
        rgAppAssigned.DataSource = GetViewAppsAssigned()
    End Sub

    Protected Sub rgAppUnAssigned_ItemDataBound(ByVal sender As Object, ByVal e As GridItemEventArgs) Handles rgAppUnAssigned.ItemDataBound
        If TypeOf e.Item Is GridGroupHeaderItem Then
            Dim item As GridGroupHeaderItem = CType(e.Item, GridGroupHeaderItem)
            item.DataCell.Text = "Technologie: " & item.DataCell.Text.Split(":"c)(1)
        End If
    End Sub

    Protected Sub rgAppAssigned_ItemDataBound(ByVal sender As Object, ByVal e As GridItemEventArgs) Handles rgAppAssigned.ItemDataBound
        If TypeOf e.Item Is GridGroupHeaderItem Then
            Dim item As GridGroupHeaderItem = CType(e.Item, GridGroupHeaderItem)
            item.DataCell.Text = "Technologie: " & item.DataCell.Text.Split(":"c)(1)
        End If
    End Sub

    Protected Sub lbtFilterUnassignedApps_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lbtFilterUnassignedApps.Click
        rgAppUnAssigned.Rebind()
    End Sub

    Sub HandleCheckBoxesAppIsMvcIsDefaultFavorite(ByVal sender As Object)

        If (Not IsPostBack) Then
            Return
        End If

        Dim controlName As String = Request.Params("__EVENTTARGET")
        If (controlName Is Nothing Or Not controlName.ToLower().Contains("rgappassigned")) Then
            ' Parent dieses Controls ist nicht das Grid "rgAppAssigned" => raus hier!
            Return
        End If

        Dim control As Control = Page.FindControl(controlName)
        If (control Is Nothing) Then
            Return
        End If

        Dim checkBox As CheckBox = TryCast(control, CheckBox)
        If (checkBox Is Nothing) Then
            ' dieses control ist keine CheckBox => raus hier!
            Return
        End If

        Dim customerID As String = ihCustomerID.Value
        Dim appID As String = checkBox.ToolTip

        If (appID Is Nothing Or appID = "") Then
            ' AppID nicht verfügbar => raus hier!
            Return
        End If

        Dim sql As String
        sql = " update vwCustomerAppAssigned " & _
              " set AppIsMvcDefaultFavorite = " & IIf(checkBox.Checked, "1", "0") & " " & _
              " where CustomerID = " & customerID & " and AppID = " & appID

        Dim cn As New SqlClient.SqlConnection(ConfigurationManager.AppSettings("Connectionstring"))
        Dim cmd As New SqlClient.SqlCommand
        Try
            cn.Open()
            cmd.Connection = cn
            cmd.CommandType = CommandType.Text
            cmd.CommandText = sql
            cmd.ExecuteNonQuery()
        Catch ex As Exception
            Throw New Exception("Update des Datenbank-Flags 'Customer.AppIsMvcDefaultFavorite' fehlgeschlagen")
        Finally
            cn.Close()
        End Try
    End Sub


    Public Sub SaveRightsForCustomer()

        Dim cbxSetRight As CheckBox
        Dim isChecked As Boolean
        Dim itemCategoryValue As String
        Dim cn As New SqlClient.SqlConnection(m_User.App.Connectionstring)
        Dim customerID As Integer = ihCustomerID.Value

        m_User = GetUser(Me)

        For Each item As GridDataItem In rgRights.Items

            cbxSetRight = item("Auswahl1").FindControl("cbxSetRight")
            isChecked = cbxSetRight.Checked
            itemCategoryValue = item("CategoryID").Text

            RightList.UpdateSingleRightPerCustomer(customerID, itemCategoryValue, isChecked, m_User.UserName)
            RightList.InsertOrDeleteRightForAllUsersOfThisCustomer(customerID, itemCategoryValue, isChecked, m_User.UserName)

        Next

        rgRights.Rebind()

    End Sub


#End Region

End Class