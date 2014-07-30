
'Imports CKG.Base.Kernel.Admin
'Imports CKG.Base.Kernel.Security
'Imports CKG.Base.Kernel.Common.Common
'Imports CKG.Portal.PageElements
'Imports CKG.Base.Common
'Imports CKG.Base.Business
'Imports CKG.Base.Business.HelpProcedures

'Public Class CustomerManagement
'    Inherits System.Web.UI.Page

'    Protected WithEvents ucStyles As Styles

'#Region " Vom Web Form Designer generierter Code "
'    Protected WithEvents lblError As System.Web.UI.WebControls.Label
'    Protected WithEvents btnSuche As System.Web.UI.WebControls.Button
'    Protected WithEvents dgSearchResult As System.Web.UI.WebControls.DataGrid
'    Protected WithEvents lstAppUnAssigned As System.Web.UI.WebControls.ListBox
'    Protected WithEvents btnUnAssign As System.Web.UI.WebControls.Button
'    Protected WithEvents lstAppAssigned As System.Web.UI.WebControls.ListBox
'    Protected WithEvents txtCName As System.Web.UI.WebControls.TextBox
'    Protected WithEvents txtCAddress As System.Web.UI.WebControls.TextBox
'    Protected WithEvents txtCMailDisplay As System.Web.UI.WebControls.TextBox
'    Protected WithEvents txtCMail As System.Web.UI.WebControls.TextBox
'    Protected WithEvents txtCWebDisplay As System.Web.UI.WebControls.TextBox
'    Protected WithEvents txtCWeb As System.Web.UI.WebControls.TextBox
'    Protected WithEvents lblMessage As System.Web.UI.WebControls.Label
'    Protected WithEvents trSearch As System.Web.UI.HtmlControls.HtmlTableRow
'    Protected WithEvents trSearchResult As System.Web.UI.HtmlControls.HtmlTableRow
'    Protected WithEvents trEditUser As System.Web.UI.HtmlControls.HtmlTableRow
'    Protected WithEvents trMaster As System.Web.UI.HtmlControls.HtmlTableRow
'    Protected WithEvents trPwdHistoryNEntries As System.Web.UI.HtmlControls.HtmlTableRow
'    Protected WithEvents lnkUserManagement As System.Web.UI.WebControls.HyperLink
'    Protected WithEvents lnkAppManagement As System.Web.UI.WebControls.HyperLink
'    Protected WithEvents lbtnNew As System.Web.UI.WebControls.LinkButton
'    Protected WithEvents lbtnSave As System.Web.UI.WebControls.LinkButton
'    Protected WithEvents lbtnCancel As System.Web.UI.WebControls.LinkButton
'    Protected WithEvents lbtnDelete As System.Web.UI.WebControls.LinkButton
'    Protected WithEvents lnkGroupManagement As System.Web.UI.WebControls.HyperLink
'    Protected WithEvents txtCustomerID As System.Web.UI.WebControls.TextBox
'    Protected WithEvents txtCustomerName As System.Web.UI.WebControls.TextBox
'    Protected WithEvents txtKUNNR As System.Web.UI.WebControls.TextBox
'    Protected WithEvents cbxMaster As System.Web.UI.WebControls.CheckBox
'    Protected WithEvents txtNewPwdAfterNDays As System.Web.UI.WebControls.TextBox
'    Protected WithEvents txtLockedAfterNLogins As System.Web.UI.WebControls.TextBox
'    Protected WithEvents txtPwdNNumeric As System.Web.UI.WebControls.TextBox
'    Protected WithEvents txtNSpecialCharacter As System.Web.UI.WebControls.TextBox
'    Protected WithEvents txtPwdHistoryNEntries As System.Web.UI.WebControls.TextBox
'    Protected WithEvents trPwdRules As System.Web.UI.HtmlControls.HtmlTableRow
'    Protected WithEvents txtPwdLength As System.Web.UI.WebControls.TextBox
'    Protected WithEvents txtNCapitalLetter As System.Web.UI.WebControls.TextBox
'    Protected WithEvents trLoginRules As System.Web.UI.HtmlControls.HtmlTableRow
'    Protected WithEvents trStyle As System.Web.UI.HtmlControls.HtmlTableRow
'    Protected WithEvents trSearchSpacer As System.Web.UI.HtmlControls.HtmlTableRow
'    Protected WithEvents txtFilterCustomerName As System.Web.UI.WebControls.TextBox
'    Protected WithEvents txtLogoPath As System.Web.UI.WebControls.TextBox
'    Protected WithEvents txtCssPath As System.Web.UI.WebControls.TextBox
'    Protected WithEvents Form1 As System.Web.UI.HtmlControls.HtmlForm
'    Protected WithEvents chkAllowMultipleLogin As System.Web.UI.WebControls.CheckBox
'    Protected WithEvents txtDocuPath As System.Web.UI.WebControls.TextBox
'    Protected WithEvents txtMaxUser As System.Web.UI.WebControls.TextBox
'    Protected WithEvents chkShowOrganization As System.Web.UI.WebControls.CheckBox
'    Protected WithEvents lnkOrganizationManagement As System.Web.UI.WebControls.HyperLink
'    Protected WithEvents trCustomerUser As System.Web.UI.HtmlControls.HtmlTableRow
'    Protected WithEvents trApp As System.Web.UI.HtmlControls.HtmlTableRow
'    Protected WithEvents trShowOrganization As System.Web.UI.HtmlControls.HtmlTableRow
'    Protected WithEvents cbxOrgAdminRestrictToCustomerGroup As System.Web.UI.WebControls.CheckBox
'    Protected WithEvents btnAssign As System.Web.UI.WebControls.Button
'    Protected WithEvents txtLogoPath2 As System.Web.UI.WebControls.TextBox
'    Protected WithEvents cbxPwdDontSendEmail As System.Web.UI.WebControls.CheckBox
'    Protected WithEvents cbxUsernameSendEmail As System.Web.UI.WebControls.CheckBox
'    Protected WithEvents plhConfirm As System.Web.UI.WebControls.PlaceHolder
'    Protected WithEvents trConfirm As System.Web.UI.HtmlControls.HtmlTableRow
'    Protected WithEvents lbtnConfirm As System.Web.UI.WebControls.LinkButton
'    Protected WithEvents cbxNameInputOptional As System.Web.UI.WebControls.CheckBox
'    Protected WithEvents chkShowDistrikte As System.Web.UI.WebControls.CheckBox
'    Protected WithEvents lnkArchivManagement As System.Web.UI.WebControls.HyperLink
'    Protected WithEvents btnAssignArchiv As System.Web.UI.WebControls.Button
'    Protected WithEvents lstArchivUnAssigned As System.Web.UI.WebControls.ListBox
'    Protected WithEvents btnUnAssignArchiv As System.Web.UI.WebControls.Button
'    Protected WithEvents lstArchivAssigned As System.Web.UI.WebControls.ListBox
'    Protected WithEvents trArchiv As System.Web.UI.HtmlControls.HtmlTableRow
'    Protected WithEvents cbxForcePasswordQuestion As System.Web.UI.WebControls.CheckBox
'    Protected WithEvents txtIpStandardUser As System.Web.UI.WebControls.TextBox
'    Protected WithEvents chkIpRestriction As System.Web.UI.WebControls.CheckBox
'    Protected WithEvents grdIpAddresses As System.Web.UI.WebControls.DataGrid
'    Protected WithEvents txtIpAddress As System.Web.UI.WebControls.TextBox
'    Protected WithEvents trIP As System.Web.UI.HtmlControls.HtmlTableRow
'    Protected WithEvents trAccountingArea As System.Web.UI.HtmlControls.HtmlTableRow
'    Protected WithEvents btnNewIpAddress As System.Web.UI.WebControls.LinkButton
'    Protected WithEvents ddlAccountingArea As System.Web.UI.WebControls.DropDownList
'    Protected WithEvents ucHeader As Header


'    'ita 2156 JJU20090219
'    '---------------------------------------------------------------
'    Protected WithEvents txtName As TextBox
'    Protected WithEvents txtVorname As TextBox
'    Protected WithEvents txtEmail As TextBox
'    Protected WithEvents txtTelefon As TextBox
'    Protected WithEvents txtTelefax As TextBox
'    Protected WithEvents rblPersonType As RadioButtonList
'    Protected WithEvents lbEintragen As LinkButton
'    Protected WithEvents gvBusinessOwner As GridView
'    Protected WithEvents gvAdminPerson As GridView
'    Protected WithEvents trKundenInfo As System.Web.UI.HtmlControls.HtmlTableRow
'    Protected WithEvents rblKundenAdministration As RadioButtonList
'    Protected WithEvents trKundenadministrationInfo As System.Web.UI.HtmlControls.HtmlTableRow
'    Protected WithEvents txtKundenadministrationBeschreibung As TextBox
'    Protected WithEvents chkKundenSperre As CheckBox
'    '---------------------------------------------------------------

'    Protected WithEvents txtKundenpostfach As TextBox
'    Protected WithEvents txtKundenhotline As TextBox
'    Protected WithEvents txtKundenfax As TextBox

'    Protected WithEvents txtUserLockTime As TextBox
'    Protected WithEvents txtUserDeleteTime As TextBox
'    Protected WithEvents chkTeamviewer As System.Web.UI.WebControls.CheckBox
'    Protected WithEvents ddlPortalLink As System.Web.UI.WebControls.DropDownList

'    'Dieser Aufruf ist f�r den Web Form-Designer erforderlich.
'    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

'    End Sub

'    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
'        'CODEGEN: Diese Methode ist f�r den Web Form-Designer erforderlich
'        'Verwenden Sie nicht den Code-Editor zur Bearbeitung.
'        InitializeComponent()
'    End Sub

'#End Region

'#Region " Membervariables "
'    Private m_User As User
'    Private m_App As App
'    Private m_context As HttpContext = HttpContext.Current
'#End Region

'    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load, Me.Load
'        ' Hier Benutzercode zur Seiteninitialisierung einf�gen
'        m_User = GetUser(Me)
'        ucHeader.InitUser(m_User)
'        ucStyles.TitleText = "Kundenverwaltung"
'        AdminAuth(Me, m_User, AdminLevel.Master)

'        Try
'            m_App = New App(m_User)

'            If Not IsPostBack Then
'                lblError.Text = ""
'                LoadLoginLinks()
'                FillForm()
'            End If
'        Catch ex As Exception
'            m_App.WriteErrorText(1, m_User.UserName, "CustomerManagement", "Page_Load", ex.ToString)

'            lblError.Text = ex.ToString
'            lblError.Visible = True
'        End Try
'    End Sub

'#Region " Data and Function "
'    Private Sub FillForm()
'        '***********************************************
'        'Ausblenden, da noch nicht fertig implementiert*
'        '***********************************************
'        'trPwdHistoryNEntries.Visible = False          '*
'        '***********************************************

'        trEditUser.Visible = False
'        trSearchResult.Visible = False
'        trKundenInfo.Visible = False


'        If m_User.HighestAdminLevel = AdminLevel.Master Then
'            'wenn SuperUser und �bergeordnete Firma
'            If m_User.Customer.AccountingArea = -1 Then
'                lnkAppManagement.Visible = True
'            End If
'        End If

'        If Not m_User.HighestAdminLevel = AdminLevel.Master Then
'            lnkAppManagement.Visible = False
'            lnkArchivManagement.Visible = False
'            trMaster.Visible = False
'            trStyle.Visible = False
'            trIP.Visible = False

'            trCustomerUser.Visible = False
'            trShowOrganization.Visible = False
'            CustomerAdminMode()
'        End If


'    End Sub

'    Private Sub setKundenadministrationInfoVisibility()

'        If Not rblKundenAdministration.SelectedValue = 1 Then
'            trKundenadministrationInfo.Visible = False
'        Else
'            'wenn eingeschr�nkt
'            trKundenadministrationInfo.Visible = True
'        End If

'    End Sub

'    Private Sub insertIntoCustomerInfo()
'        Dim cn As New SqlClient.SqlConnection(ConfigurationManager.AppSettings("Connectionstring"))
'        Dim cmd As New SqlClient.SqlCommand
'        Try
'            cn.Open()
'            cmd.Connection = cn
'            cmd.CommandType = CommandType.Text
'            Dim SqlQuery As String

'            SqlQuery = "INSERT INTO [CustomerInfo] (CustomerID, PersonTyp, Name, Vorname, Email, Telefon, Telefax) VALUES (@CustomerID, @PersonTyp, @Name, @Vorname, @Email, @Telefon, @Telefax);"
'            With cmd
'                .Parameters.AddWithValue("@CustomerID", txtCustomerID.Text)
'                .Parameters.AddWithValue("@PersonTyp", rblPersonType.SelectedValue)
'                .Parameters.AddWithValue("@Name", txtName.Text)
'                .Parameters.AddWithValue("@Vorname", txtVorname.Text)
'                .Parameters.AddWithValue("@Email", txtEmail.Text)
'                .Parameters.AddWithValue("@Telefon", txtTelefon.Text)
'                .Parameters.AddWithValue("@Telefax", txtTelefax.Text)

'            End With
'            cmd.CommandText = SqlQuery
'            cmd.ExecuteNonQuery()
'        Catch ex As Exception
'            Throw New Exception("SCHREIBEN EINES EINTRAGES IN DIE DB: TABELLENNAME=  CustomerInfo  \ " & ex.Message & " \ " & ex.StackTrace)
'        Finally
'            cn.Close()
'        End Try
'    End Sub


'    Protected Sub lbEintragen_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lbEintragen.Click
'        If Not txtName.Text.Trim(" "c).Length = 0 AndAlso Not rblPersonType.SelectedIndex = -1 Then
'            insertIntoCustomerInfo()
'            txtName.Text = ""
'            txtVorname.Text = ""
'            txtEmail.Text = ""
'            txtTelefax.Text = ""
'            txtTelefon.Text = ""
'            fillBusinessownerGrid(gvBusinessOwner.PageIndex)
'            fillAdminpersonGrid(gvAdminPerson.PageIndex)
'        End If


'    End Sub



'    Private Function getKundenInfoDT(ByVal PersonType As String) As DataTable


'        Dim cn As New SqlClient.SqlConnection(ConfigurationManager.AppSettings("Connectionstring"))
'        Dim cmd As New SqlClient.SqlCommand
'        Try
'            getKundenInfoDT = New DataTable
'            If cn.State = ConnectionState.Closed Then
'                cn.Open()
'            End If

'            Dim daApp As New SqlClient.SqlDataAdapter("SELECT *" & _
'                                                        "FROM CustomerInfo " & _
'                                                      "Where CustomerID = '" & txtCustomerID.Text & "' AND PersonTyp = '" & PersonType & "'", cn)
'            daApp.Fill(getKundenInfoDT)
'        Finally
'            If cn.State <> ConnectionState.Closed Then
'                cn.Close()
'            End If
'        End Try

'        Return getKundenInfoDT


'    End Function



'    Private Sub deleteFromCustomerInfoTable(ByVal ID As String)

'        Dim cn As New SqlClient.SqlConnection(ConfigurationManager.AppSettings("Connectionstring"))
'        Dim cmd As New SqlClient.SqlCommand
'        Dim sqlQuery As String
'        Try
'            cmd.Connection = cn
'            If cn.State = ConnectionState.Closed Then
'                cn.Open()
'            End If

'            sqlQuery = "Delete FROM CustomerInfo WHERE ID=@ID;"
'            cmd.Parameters.AddWithValue("@ID", ID)

'            cmd.CommandText = sqlQuery
'            cmd.ExecuteNonQuery()
'        Finally
'            If cn.State <> ConnectionState.Closed Then
'                cn.Close()
'            End If
'        End Try
'    End Sub

'    Private Sub fillBusinessownerGrid(ByVal intPageIndex As Int32, Optional ByVal strSort As String = "")

'        Dim tmpDataView As New DataView(getKundenInfoDT("Businessowner"))


'        Dim intTempPageIndex As Int32 = intPageIndex
'        Dim strTempSort As String = ""
'        Dim strDirection As String = ""

'        If strSort.Trim(" "c).Length > 0 Then
'            intTempPageIndex = 0
'            strTempSort = strSort.Trim(" "c)
'            If (ViewState("Sort") Is Nothing) OrElse (ViewState("Sort").ToString = strTempSort) Then
'                If ViewState("Direction") Is Nothing Then
'                    strDirection = "desc"
'                Else
'                    strDirection = ViewState("Direction").ToString
'                End If
'            Else
'                strDirection = "desc"
'            End If

'            If strDirection = "asc" Then
'                strDirection = "desc"
'            Else
'                strDirection = "asc"
'            End If

'            ViewState("Sort") = strTempSort
'            ViewState("Direction") = strDirection
'        Else
'            If Not ViewState("Sort") Is Nothing Then
'                strTempSort = ViewState("Sort").ToString
'                If ViewState("Direction") Is Nothing Then
'                    strDirection = "asc"
'                    ViewState("Direction") = strDirection
'                Else
'                    strDirection = ViewState("Direction").ToString
'                End If
'            End If
'        End If

'        If Not strTempSort.Length = 0 Then
'            tmpDataView.Sort = strTempSort & " " & strDirection
'        End If


'        gvBusinessOwner.PageIndex = intTempPageIndex

'        gvBusinessOwner.DataSource = tmpDataView

'        gvBusinessOwner.DataBind()

'        If gvBusinessOwner.PageCount > 1 Then
'            gvBusinessOwner.PagerStyle.CssClass = "PagerStyle"
'            gvBusinessOwner.PagerSettings.Visible = True
'        Else
'            gvBusinessOwner.PagerSettings.Visible = False
'        End If



'    End Sub


'    Private Sub fillAdminpersonGrid(ByVal intPageIndex As Int32, Optional ByVal strSort As String = "")

'        Dim tmpDataView As New DataView(getKundenInfoDT("Adminperson"))


'        Dim intTempPageIndex As Int32 = intPageIndex
'        Dim strTempSort As String = ""
'        Dim strDirection As String = ""

'        If strSort.Trim(" "c).Length > 0 Then
'            intTempPageIndex = 0
'            strTempSort = strSort.Trim(" "c)
'            If (ViewState("Sort") Is Nothing) OrElse (ViewState("Sort").ToString = strTempSort) Then
'                If ViewState("Direction") Is Nothing Then
'                    strDirection = "desc"
'                Else
'                    strDirection = ViewState("Direction").ToString
'                End If
'            Else
'                strDirection = "desc"
'            End If

'            If strDirection = "asc" Then
'                strDirection = "desc"
'            Else
'                strDirection = "asc"
'            End If

'            ViewState("Sort") = strTempSort
'            ViewState("Direction") = strDirection
'        Else
'            If Not ViewState("Sort") Is Nothing Then
'                strTempSort = ViewState("Sort").ToString
'                If ViewState("Direction") Is Nothing Then
'                    strDirection = "asc"
'                    ViewState("Direction") = strDirection
'                Else
'                    strDirection = ViewState("Direction").ToString
'                End If
'            End If
'        End If

'        If Not strTempSort.Length = 0 Then
'            tmpDataView.Sort = strTempSort & " " & strDirection
'        End If


'        gvAdminPerson.PageIndex = intTempPageIndex

'        gvAdminPerson.DataSource = tmpDataView

'        gvAdminPerson.DataBind()

'        If gvAdminPerson.PageCount > 1 Then
'            gvAdminPerson.PagerStyle.CssClass = "PagerStyle"
'            gvAdminPerson.PagerSettings.Visible = True
'        Else
'            gvAdminPerson.PagerSettings.Visible = False
'        End If



'    End Sub


'    Private Sub FillDataGrid()
'        Dim strSort As String = "CustomerID"
'        If Not ViewState("ResultSort") Is Nothing Then
'            strSort = ViewState("ResultSort").ToString
'        End If
'        FillDataGrid(strSort)
'    End Sub
'    Private Sub FillDataGrid(ByVal strSort As String)
'        trSearchResult.Visible = True
'        Dim dvCustomer As DataView

'        'If Not m_context.Cache("myCustomerListView") Is Nothing Then
'        '    dvCustomer = CType(m_context.Cache("myCustomerListView"), DataView)
'        If Not Session("myCustomerListView") Is Nothing Then
'            dvCustomer = CType(Session("myCustomerListView"), DataView)
'        Else
'            Dim dtCustomer As Kernel.CustomerList
'            Dim cn As New SqlClient.SqlConnection(m_User.App.Connectionstring)
'            Try
'                cn.Open()

'                dtCustomer = New Kernel.CustomerList( _
'                                    txtFilterCustomerName.Text, _
'                                    m_User.Customer.AccountingArea, _
'                                    cn)

'                dvCustomer = dtCustomer.DefaultView
'                'm_context.Cache.Insert("myCustomerListView", dvCustomer, Nothing, DateTime.Now.AddMinutes(20), TimeSpan.Zero) 
'                Session("myCustomerListView") = dvCustomer
'            Finally
'                If cn.State <> ConnectionState.Closed Then
'                    cn.Close()
'                End If
'            End Try
'        End If
'        dvCustomer.Sort = strSort
'        If dvCustomer.Count > dgSearchResult.PageSize Then
'            dgSearchResult.PagerStyle.Visible = True
'        Else
'            dgSearchResult.PagerStyle.Visible = False
'        End If

'        Try
'            With dgSearchResult
'                .DataSource = dvCustomer
'                .DataBind()
'            End With
'        Catch ex As Exception
'            m_App.WriteErrorText(1, m_User.UserName, "CustomerManagement", "FillDataGrid", ex.ToString)

'        End Try


'    End Sub

'    Private Function FillEdit(ByVal intCustomerId As Integer) As Boolean
'        SearchMode(False)
'        Dim cn As New SqlClient.SqlConnection(m_User.App.Connectionstring)
'        Try
'            cn.Open()
'            Dim _Customer As New Customer(intCustomerId, cn)
'            txtCustomerID.Text = _Customer.CustomerId.ToString


'            txtCustomerName.Text = _Customer.CustomerName
'            txtKUNNR.Text = _Customer.KUNNR
'            txtDocuPath.Text = _Customer.DocuPath
'            cbxMaster.Checked = _Customer.IsMaster
'            chkKundenSperre.Enabled = Not _Customer.IsMaster
'            chkShowDistrikte.Checked = _Customer.ShowDistrikte
'            chkAllowMultipleLogin.Checked = _Customer.AllowMultipleLogin
'            chkShowOrganization.Checked = _Customer.ShowOrganization
'            cbxOrgAdminRestrictToCustomerGroup.Checked = _Customer.OrgAdminRestrictToCustomerGroup
'            txtMaxUser.Text = _Customer.MaxUser.ToString
'            chkTeamviewer.Checked = _Customer.ShowsTeamViewer
'            ddlPortalLink.SelectedValue = _Customer.LoginLinkID

'            'selbstadministration
'            rblKundenAdministration.SelectedValue = _Customer.Selfadministration
'            txtKundenadministrationBeschreibung.Text = _Customer.SelfadministrationInfo
'            setKundenadministrationInfoVisibility()

'            'sperren
'            chkKundenSperre.Checked = _Customer.Locked

'            'fillAccountingArea
'            FillAccountingArea(intCustomerId)

'            'LoginRegeln
'            txtLockedAfterNLogins.Text = _Customer.CustomerLoginRules.LockedAfterNLogins.ToString
'            txtNewPwdAfterNDays.Text = _Customer.CustomerLoginRules.NewPasswordAfterNDays.ToString
'            'Passwortregeln
'            txtPwdLength.Text = _Customer.CustomerPasswordRules.Length.ToString
'            txtPwdNNumeric.Text = _Customer.CustomerPasswordRules.Numeric.ToString
'            txtNCapitalLetter.Text = _Customer.CustomerPasswordRules.CapitalLetters.ToString
'            txtNSpecialCharacter.Text = _Customer.CustomerPasswordRules.SpecialCharacter.ToString
'            txtPwdHistoryNEntries.Text = _Customer.CustomerPasswordRules.PasswordHistoryEntries.ToString
'            'Kontaktdaten
'            If Not _Customer.CustomerContact Is Nothing Then
'                txtCName.Text = _Customer.CustomerContact.Name
'                txtCAddress.Text = _Customer.CustomerContact.Address
'                txtCMailDisplay.Text = _Customer.CustomerContact.MailDisplay
'                txtCMail.Text = _Customer.CustomerContact.Mail
'                txtCWebDisplay.Text = _Customer.CustomerContact.WebDisplay
'                txtCWeb.Text = _Customer.CustomerContact.Web
'                txtKundenpostfach.Text = _Customer.CustomerContact.Kundenpostfach
'                txtKundenhotline.Text = _Customer.CustomerContact.Kundenhotline.Trim
'                txtKundenfax.Text = _Customer.CustomerContact.Kundenfax.Trim
'            End If
'            'Anwendungen
'            FillAssigned(_Customer.CustomerId, cn)
'            FillUnAssigned(_Customer.CustomerId, cn)
'            'Style
'            txtLogoPath.Text = _Customer.CustomerStyle.LogoPath.ToString
'            txtCssPath.Text = _Customer.CustomerStyle.CssPath.ToString

'            'Zweites Logo.
'            txtLogoPath2.Text = _Customer.LogoPath2.ToString

'            'IP-Adress-Verwaltung
'            chkIpRestriction.Checked = _Customer.IpRestriction
'            txtIpStandardUser.Text = _Customer.IpStandardUser

'            cbxPwdDontSendEmail.Checked = _Customer.CustomerPasswordRules.DontSendEmail
'            cbxUsernameSendEmail.Checked = _Customer.CustomerUsernameRules.DontSendEmail

'            If cbxPwdDontSendEmail.Checked Then
'                cbxForcePasswordQuestion.Enabled = False
'                cbxForcePasswordQuestion.Checked = False
'            Else
'                cbxForcePasswordQuestion.Enabled = True
'                cbxForcePasswordQuestion.Checked = _Customer.ForcePasswordQuestion
'            End If

'            cbxNameInputOptional.Checked = _Customer.CustomerPasswordRules.NameInputOptional

'            'Benutzer und Organisation
'            txtUserLockTime.Text = _Customer.DaysUntilLock
'            txtUserDeleteTime.Text = _Customer.DaysUntilDelete

'            If txtCustomerID.Text = "-1" Then
'                trIP.Visible = False

'            Else
'                trIP.Visible = True

'                FillIpAddresses(_Customer)
'            End If

'            'kundenInformation Berechtigung
'            fillBusinessownerGrid(0)
'            fillAdminpersonGrid(0)

'            Return True
'        Finally
'            If cn.State <> ConnectionState.Closed Then
'                cn.Close()
'            End If
'        End Try
'    End Function

'    Private Function GetArchivAssignedView(ByVal intCustomerID As Integer, ByVal cn As SqlClient.SqlConnection) As DataView
'        'Als Extra-Funktion ausgelagert, da beim Speichern evtl.
'        'benoetigt, wenn Cache leer ist (in jenem Fall soll die
'        'List kein DataBind ausfuehren).
'        Dim _ArchivAssigned As New ArchivList(intCustomerID, cn)
'        _ArchivAssigned.GetAssigned()
'        _ArchivAssigned.DefaultView.Sort = "EasyArchivName"
'        'Cache wird befuellt, um spaeter beim Speichern darauf
'        'zu zugreifen.
'        'm_context.Cache.Insert("myCustomerArchivAssigned", _ArchivAssigned.DefaultView, Nothing, DateTime.Now.AddMinutes(20), TimeSpan.Zero)
'        Session("myCustomerArchivAssigned") = _ArchivAssigned.DefaultView
'        Return _ArchivAssigned.DefaultView
'    End Function

'    Private Function GetAppAssignedView(ByVal intCustomerID As Integer, ByVal cn As SqlClient.SqlConnection) As DataView
'        'Als Extra-Funktion ausgelagert, da beim Speichern evtl.
'        'benoetigt, wenn Cache leer ist (in jenem Fall soll die
'        'List kein DataBind ausfuehren).
'        Dim _AppAssigned As New ApplicationList(intCustomerID, cn)
'        _AppAssigned.GetAssigned()
'        _AppAssigned.DefaultView.Sort = "AppFriendlyName"
'        'Cache wird befuellt, um spaeter beim Speichern darauf
'        'zu zugreifen.
'        'm_context.Cache.Insert("myCustomerAppAssigned", _AppAssigned.DefaultView, Nothing, DateTime.Now.AddMinutes(20), TimeSpan.Zero)
'        Session("myCustomerAppAssigned") = _AppAssigned.DefaultView
'        Return _AppAssigned.DefaultView
'    End Function

'    Private Sub FillIpAddresses(ByVal mCust As Customer)
'        If mCust.IpAddresses.Rows.Count = 0 Then
'            grdIpAddresses.Visible = False
'        Else
'            grdIpAddresses.Visible = True
'            grdIpAddresses.DataSource = mCust.IpAddresses
'            grdIpAddresses.DataBind()
'        End If
'    End Sub

'    Private Sub FillAssigned(ByVal intCustomerID As Integer, ByVal cn As SqlClient.SqlConnection)
'        Dim row As DataRow

'        Dim dvAppAssigned As DataView = GetAppAssignedView(intCustomerID, cn)

'        For Each row In dvAppAssigned.Table.Rows
'            row("AppFriendlyName") = row("AppFriendlyName").ToString.ToUpper & " || " & row("AppURL").ToString
'        Next

'        lstAppAssigned.DataSource = dvAppAssigned
'        lstAppAssigned.DataTextField = "AppFriendlyName"
'        lstAppAssigned.DataValueField = "AppID"
'        lstAppAssigned.DataBind()

'        Dim dvArchivAssigned As DataView = GetArchivAssignedView(intCustomerID, cn)

'        For Each row In dvArchivAssigned.Table.Rows
'            row("EasyArchivName") = row("EasyArchivName").ToString.ToUpper & " || Lagerort-Name: " & row("EasyLagerortName").ToString & " || QueryIndex-Name: " & row("EasyQueryIndexName").ToString & " || Titel: " & row("EasyTitleName").ToString
'        Next

'        lstArchivAssigned.DataSource = dvArchivAssigned
'        lstArchivAssigned.DataTextField = "EasyArchivName"
'        lstArchivAssigned.DataValueField = "ArchivID"
'        lstArchivAssigned.DataBind()
'    End Sub


'    Private Sub FillAccountingArea(ByVal intCustomerId As Int32, Optional ByVal neuanlage As Boolean = False)
'        'AccountingArea
'        Dim cn As New SqlClient.SqlConnection(m_User.App.Connectionstring)
'        Try
'            cn.Open()
'            Dim _Customer As New Customer(intCustomerId, cn)
'            Dim _AccountingAreaList As Kernel.AccountingAreaList
'            _AccountingAreaList = New Kernel.AccountingAreaList(cn)
'            Dim vwAccountingAreaList As DataView = _AccountingAreaList.DefaultView
'            vwAccountingAreaList.Sort = "Area"
'            ddlAccountingArea.DataSource = vwAccountingAreaList
'            ddlAccountingArea.DataValueField = "Area"
'            ddlAccountingArea.DataTextField = "Description"
'            ddlAccountingArea.DataBind()
'            ddlAccountingArea.ClearSelection()




'            If m_User.Customer.AccountingArea = -1 Then
'                'User geh�rt der �bergeordneten Firma an, Grundlegend BK �nderbar
'                ddlAccountingArea.Enabled = True
'            Else
'                'kein �bergeordneter user, Grundlegend BK nicht �nderbar
'                ddlAccountingArea.Enabled = False
'            End If


'            If Not ddlAccountingArea.Items.FindByValue(_Customer.AccountingArea.ToString) Is Nothing Then
'                ddlAccountingArea.Items.FindByValue(_Customer.AccountingArea.ToString).Selected = True
'            Else
'                If _Customer.AccountingArea = -1 Then 'es wurde schon eine Vorselektion der Kunden nach BK vorgenommen
'                    'wenn firma 1 aufgerufen wird, soll der buchungskreis nicht �nderbar sein,
'                    'wenn aber ein User der Firma 1 eine Neuanlage t�tigt, soll der Buchungskreis ausw�hlbar sein
'                    'daher optionaler Parameter "Neuanlage", au�erdem soll keine �bergeordnete Firma angelegt werden
'                    If neuanlage Then
'                        ddlAccountingArea.Enabled = True
'                    Else
'                        Dim newItem As New System.Web.UI.WebControls.ListItem("�bergeordnet", "-1")
'                        ddlAccountingArea.Items.Add(newItem)
'                        ddlAccountingArea.Items.FindByValue("-1").Selected = True
'                        ddlAccountingArea.Enabled = False

'                    End If
'                Else
'                    'der Buchungskreis wurde nicht gefunden ist aber auch keine �bergeordnete Firma mit -1? gibts nicht
'                    Throw New Exception("Der Buchungskreis: " & _Customer.AccountingArea & " ist nicht bekannt!")
'                End If
'            End If
'        Catch ex As Exception
'            lblError.Text = "Fehler beim Bef�llen der Accountingareas: " & ex.Message
'        End Try

'    End Sub
'    Private Sub FillUnAssigned(ByVal intCustomerID As Integer, ByVal cn As SqlClient.SqlConnection)
'        Dim row As DataRow

'        Dim AppUnAssigned As New ApplicationList(intCustomerID, cn)
'        AppUnAssigned.GetUnassigned()
'        AppUnAssigned.DefaultView.Sort = "AppFriendlyName"

'        For Each row In AppUnAssigned.Rows
'            row("AppFriendlyName") = row("AppFriendlyName").ToString.ToUpper & " || " & row("AppURL").ToString
'        Next

'        lstAppUnAssigned.DataSource = AppUnAssigned.DefaultView
'        lstAppUnAssigned.DataTextField = "AppFriendlyName"
'        lstAppUnAssigned.DataValueField = "AppID"
'        lstAppUnAssigned.DataBind()

'        Dim ArchivUnAssigned As New ArchivList(intCustomerID, cn)
'        ArchivUnAssigned.GetUnassigned()
'        ArchivUnAssigned.DefaultView.Sort = "EasyArchivName"

'        For Each row In ArchivUnAssigned.Rows
'            row("EasyArchivName") = row("EasyArchivName").ToString.ToUpper & " || Lagerort-Name: " & row("EasyLagerortName").ToString & " || QueryIndex-Name: " & row("EasyQueryIndexName").ToString & " || Titel: " & row("EasyTitleName").ToString
'        Next

'        lstArchivUnAssigned.DataSource = ArchivUnAssigned.DefaultView
'        lstArchivUnAssigned.DataTextField = "EasyArchivName"
'        lstArchivUnAssigned.DataValueField = "ArchivID"
'        lstArchivUnAssigned.DataBind()
'    End Sub

'    Private Sub ClearEdit()
'        txtCustomerID.Text = "-1"
'        txtCustomerName.Text = ""

'        rblKundenAdministration.SelectedValue = 0
'        txtKundenadministrationBeschreibung.Text = ""
'        setKundenadministrationInfoVisibility()

'        chkKundenSperre.Checked = False
'        chkTeamviewer.Checked = False

'        txtKUNNR.Text = "0"
'        txtDocuPath.Text = ""
'        cbxMaster.Checked = False
'        chkAllowMultipleLogin.Checked = True
'        chkShowOrganization.Checked = False
'        cbxOrgAdminRestrictToCustomerGroup.Checked = False

'        'LoginRegeln
'        txtLockedAfterNLogins.Text = "3"
'        txtNewPwdAfterNDays.Text = "60"
'        'Passwortregeln
'        txtPwdLength.Text = "8"
'        txtPwdNNumeric.Text = "1"
'        txtNCapitalLetter.Text = "1"
'        txtNSpecialCharacter.Text = "1"
'        txtPwdHistoryNEntries.Text = "6"
'        cbxPwdDontSendEmail.Checked = True
'        cbxUsernameSendEmail.Checked = True
'        cbxForcePasswordQuestion.Checked = False
'        cbxForcePasswordQuestion.Enabled = False
'        cbxNameInputOptional.Checked = True
'        'Anwendungen
'        lstAppAssigned.Items.Clear()
'        lstAppUnAssigned.Items.Clear()
'        'Kontaktdaten
'        txtCName.Text = ""
'        txtCAddress.Text = ""
'        txtCMailDisplay.Text = ""
'        txtCMail.Text = ""
'        txtCWebDisplay.Text = ""
'        txtCWeb.Text = ""
'        txtKundenpostfach.Text = ""
'        txtKundenhotline.Text = ""
'        txtKundenfax.Text = ""
'        'Style
'        txtLogoPath.Text = "../Images/Logo.gif"
'        '��� JVE 18.09.2006: Logo2
'        txtLogoPath2.Text = "../Images/Logo.gif"
'        '------------------------
'        txtCssPath.Text = "Styles.css"
'        'Buttons
'        lbtnSave.Visible = True
'        lbtnDelete.Visible = False
'        LockEdit(False)

'    End Sub

'    Private Sub LockEdit(ByVal blnLock As Boolean)
'        Dim strBackColor As String = "White"
'        If blnLock Then
'            strBackColor = "LightGray"
'        End If
'        txtCustomerID.Enabled = Not blnLock
'        txtCustomerID.BackColor = System.Drawing.Color.FromName(strBackColor)
'        txtCustomerName.Enabled = Not blnLock
'        txtCustomerName.BackColor = System.Drawing.Color.FromName(strBackColor)
'        If m_User.HighestAdminLevel < AdminLevel.Master Then
'            txtKUNNR.Enabled = False
'            txtKUNNR.CssClass = "InfoBoxFlat"
'        Else
'            txtKUNNR.Enabled = Not blnLock
'            txtKUNNR.BackColor = System.Drawing.Color.FromName(strBackColor)
'        End If
'        cbxMaster.Enabled = False
'        chkAllowMultipleLogin.Enabled = Not blnLock
'        txtDocuPath.Enabled = Not blnLock
'        txtDocuPath.BackColor = System.Drawing.Color.FromName(strBackColor)
'        '.cbxMaster.BackColor = System.Drawing.Color.FromName(strBackColor)
'        'LoginRegeln
'        txtLockedAfterNLogins.Enabled = Not blnLock
'        txtLockedAfterNLogins.BackColor = System.Drawing.Color.FromName(strBackColor)
'        txtNewPwdAfterNDays.Enabled = Not blnLock
'        txtNewPwdAfterNDays.BackColor = System.Drawing.Color.FromName(strBackColor)
'        'Passwortregeln
'        txtPwdLength.Enabled = Not blnLock
'        txtPwdLength.BackColor = System.Drawing.Color.FromName(strBackColor)
'        txtPwdNNumeric.Enabled = Not blnLock
'        txtPwdNNumeric.BackColor = System.Drawing.Color.FromName(strBackColor)
'        txtNCapitalLetter.Enabled = Not blnLock
'        txtNCapitalLetter.BackColor = System.Drawing.Color.FromName(strBackColor)
'        txtNSpecialCharacter.Enabled = Not blnLock
'        txtNSpecialCharacter.BackColor = System.Drawing.Color.FromName(strBackColor)
'        txtPwdHistoryNEntries.Enabled = Not blnLock
'        txtPwdHistoryNEntries.BackColor = System.Drawing.Color.FromName(strBackColor)
'        cbxPwdDontSendEmail.Enabled = Not blnLock
'        cbxUsernameSendEmail.Enabled = Not blnLock
'        cbxNameInputOptional.Enabled = Not blnLock

'        If cbxPwdDontSendEmail.Checked Then
'            cbxForcePasswordQuestion.Enabled = False
'            cbxForcePasswordQuestion.Checked = False
'        Else
'            cbxForcePasswordQuestion.Enabled = Not blnLock
'        End If

'        'IP-Adressen
'        chkIpRestriction.Enabled = Not blnLock
'        txtIpStandardUser.Enabled = Not blnLock
'        txtIpStandardUser.BackColor = System.Drawing.Color.FromName(strBackColor)
'        txtIpAddress.Enabled = Not blnLock
'        txtIpAddress.BackColor = System.Drawing.Color.FromName(strBackColor)

'        'Anwendungen
'        lstAppAssigned.Enabled = Not blnLock
'        lstAppAssigned.BackColor = System.Drawing.Color.FromName(strBackColor)
'        lstAppUnAssigned.Enabled = Not blnLock
'        lstAppUnAssigned.BackColor = System.Drawing.Color.FromName(strBackColor)
'        'Kontaktdaten
'        txtCName.Enabled = Not blnLock
'        txtCName.BackColor = System.Drawing.Color.FromName(strBackColor)
'        txtCAddress.Enabled = Not blnLock
'        txtCAddress.BackColor = System.Drawing.Color.FromName(strBackColor)
'        txtCMailDisplay.Enabled = Not blnLock
'        txtCMailDisplay.BackColor = System.Drawing.Color.FromName(strBackColor)
'        txtCMail.Enabled = Not blnLock
'        txtCMail.BackColor = System.Drawing.Color.FromName(strBackColor)
'        txtCWebDisplay.Enabled = Not blnLock
'        txtCWebDisplay.BackColor = System.Drawing.Color.FromName(strBackColor)
'        txtKundenpostfach.Enabled = Not blnLock
'        txtKundenpostfach.BackColor = System.Drawing.Color.FromName(strBackColor)
'        txtKundenhotline.Enabled = Not blnLock
'        txtKundenhotline.BackColor = System.Drawing.Color.FromName(strBackColor)
'        txtKundenfax.Enabled = Not blnLock
'        txtKundenfax.BackColor = System.Drawing.Color.FromName(strBackColor)
'        txtCWeb.Enabled = Not blnLock
'        txtCWeb.BackColor = System.Drawing.Color.FromName(strBackColor)
'        'Style
'        txtLogoPath.Enabled = Not blnLock
'        txtLogoPath.BackColor = System.Drawing.Color.FromName(strBackColor)

'        '��� JVE 18.09.2006: Logo2
'        txtLogoPath2.Enabled = Not blnLock
'        txtLogoPath2.BackColor = System.Drawing.Color.FromName(strBackColor)
'        '-------------------------

'        txtCssPath.Enabled = Not blnLock
'        txtCssPath.BackColor = System.Drawing.Color.FromName(strBackColor)
'        'ShowOrganization
'        chkShowOrganization.Enabled = Not blnLock
'        cbxOrgAdminRestrictToCustomerGroup.Enabled = Not blnLock
'        'MaxUser
'        txtMaxUser.Enabled = Not blnLock
'        txtMaxUser.BackColor = System.Drawing.Color.FromName(strBackColor)

'        'selfadministration
'        rblKundenAdministration.Enabled = Not blnLock
'        txtKundenadministrationBeschreibung.Enabled = Not blnLock
'        txtKundenadministrationBeschreibung.BackColor = System.Drawing.Color.FromName(strBackColor)

'        'sperre
'        chkKundenSperre.Enabled = Not blnLock
'        chkTeamviewer.Enabled = Not blnLock

'        'Buttons
'        btnAssign.Enabled = Not blnLock
'        btnUnAssign.Enabled = Not blnLock

'        'Benutzer und Organisation
'        txtUserLockTime.Enabled = Not blnLock
'        txtUserLockTime.BackColor = System.Drawing.Color.FromName(strBackColor)
'        txtUserDeleteTime.Enabled = Not blnLock
'        txtUserDeleteTime.BackColor = System.Drawing.Color.FromName(strBackColor)

'        ddlPortalLink.Enabled = Not blnLock
'    End Sub

'    Private Sub CustomerAdminMode()
'        SearchMode(False)
'        trApp.Visible = False

'        'If m_User.Groups.Count > 0 Then
'        If m_User.IsCustomerAdmin Then
'            LockEdit(False)
'            EditEditMode(m_User.Customer.CustomerId)
'        End If
'        'End If
'    End Sub

'    Private Sub ConfirmMode(ByVal confirmOn As Boolean)
'        trConfirm.Visible = confirmOn
'        lbtnConfirm.Visible = confirmOn
'        LockEdit(confirmOn)
'        lbtnSave.Enabled = Not confirmOn
'        If confirmOn Then
'            lbtnCancel.Text = " &#149;&nbsp;�ndern"
'        Else
'            lbtnCancel.Text = " &#149;&nbsp;Abbrechen"
'        End If
'    End Sub

'    Private Sub EditEditMode(ByVal intCustomerId As Integer)
'        trConfirm.Visible = False
'        lbtnConfirm.Visible = False
'        If Not FillEdit(intCustomerId) Then
'            LockEdit(True)
'            lbtnSave.Enabled = False
'        Else
'            lbtnSave.Enabled = True
'        End If
'        lbtnCancel.Text = " &#149;&nbsp;Verwerfen"
'    End Sub

'    Private Sub EditDeleteMode(ByVal intGroupId As Integer)
'        trConfirm.Visible = False
'        lbtnConfirm.Visible = False
'        If Not FillEdit(intGroupId) Then
'            lbtnDelete.Enabled = False
'        Else
'            lblMessage.Text = "M�chten Sie den Kunden wirklich l�schen?"
'            lbtnDelete.Enabled = True
'        End If
'        LockEdit(True)
'        lbtnCancel.Text = " &#149;&nbsp;Abbrechen"
'        lbtnSave.Visible = False
'        lbtnDelete.Visible = True
'    End Sub

'    Private Sub SearchMode(Optional ByVal blnSearchMode As Boolean = True)
'        trConfirm.Visible = False
'        lbtnConfirm.Visible = False
'        trEditUser.Visible = Not blnSearchMode
'        trSearch.Visible = blnSearchMode
'        trSearchSpacer.Visible = blnSearchMode
'        trSearchResult.Visible = blnSearchMode
'        lbtnSave.Visible = Not blnSearchMode
'        lbtnCancel.Visible = Not blnSearchMode
'        lbtnNew.Visible = blnSearchMode
'        trKundenInfo.Visible = Not blnSearchMode
'    End Sub

'    Private Sub Search(Optional ByVal blnRefillDataGrid As Boolean = False, Optional ByVal blnResetSelectedIndex As Boolean = False, Optional ByVal blnResetPageIndex As Boolean = False, Optional ByVal blnClearCache As Boolean = False)
'        ClearEdit()
'        If blnClearCache Then
'            'm_context.Cache.Remove("myCustomerListView")
'            Session.Remove("myCustomerListView")
'        End If
'        If blnResetSelectedIndex Then dgSearchResult.SelectedIndex = -1
'        If blnResetPageIndex Then dgSearchResult.CurrentPageIndex = 0
'        If m_User.HighestAdminLevel > AdminLevel.Customer Then
'            SearchMode()
'            If blnRefillDataGrid Then FillDataGrid()
'        Else
'            CustomerAdminMode()
'        End If
'    End Sub

'    Private Sub Log(ByVal strIdentification As String, ByVal strDescription As String, ByVal tblParameters As DataTable, Optional ByVal strCategory As String = "APP")
'        Dim logApp As New Base.Kernel.Logging.Trace(m_User.App.Connectionstring, m_User.App.SaveLogAccessSAP, m_User.App.LogLevel)

'        ' strCategory
'        Dim strUserName As String = m_User.UserName ' strUserName
'        Dim strSessionID As String = Session.SessionID ' strSessionID
'        Dim intSource As Integer = CInt(Request.QueryString("AppID")) ' intSource 
'        Dim strTask As String = "Admin - Kundenverwaltung" ' strTask
'        ' strIdentification
'        ' strDescription
'        Dim strCustomerName As String = m_User.CustomerName ' strCustomername
'        Dim blnIsTestUser As Boolean = m_User.IsTestUser ' blnIsTestUser
'        Dim intSeverity As Integer = 0 ' intSeverity 

'        logApp.WriteEntry(strCategory, strUserName, strSessionID, intSource, strTask, strIdentification, strDescription, strCustomerName, m_User.Customer.CustomerId, blnIsTestUser, intSeverity, tblParameters)
'    End Sub

'    Private Function SetOldLogParameters(ByVal intCustomerId As Int32, ByVal tblPar As DataTable) As DataTable
'        Dim cn As New SqlClient.SqlConnection(m_User.App.Connectionstring)
'        Try

'            cn.Open()
'            Dim _Customer As New Customer(intCustomerId, cn)

'            If tblPar Is Nothing Then
'                tblPar = CreateLogTableStructure()
'            End If
'            With tblPar
'                .Rows.Add(.NewRow)
'                .Rows(.Rows.Count - 1)("Status") = "Alt"

'                .Rows(.Rows.Count - 1)("Firmenname") = _Customer.CustomerName
'                .Rows(.Rows.Count - 1)("KUNNR") = _Customer.KUNNR
'                .Rows(.Rows.Count - 1)("Neues Kennwort nach n Tagen") = _Customer.CustomerLoginRules.NewPasswordAfterNDays.ToString
'                .Rows(.Rows.Count - 1)("Konto sperren nach n Fehlversuchen") = _Customer.CustomerLoginRules.LockedAfterNLogins.ToString
'                .Rows(.Rows.Count - 1)("Mehrfaches Login") = _Customer.AllowMultipleLogin
'                .Rows(.Rows.Count - 1)("Mindestl�nge") = _Customer.CustomerPasswordRules.Length.ToString
'                .Rows(.Rows.Count - 1)("n numerische Zeichen") = _Customer.CustomerPasswordRules.Numeric.ToString
'                .Rows(.Rows.Count - 1)("n Gro�buchstaben") = _Customer.CustomerPasswordRules.CapitalLetters.ToString
'                .Rows(.Rows.Count - 1)("n Sonderzeichen") = _Customer.CustomerPasswordRules.SpecialCharacter.ToString
'                .Rows(.Rows.Count - 1)("Sperre letzte n Kennworte") = _Customer.CustomerPasswordRules.PasswordHistoryEntries.ToString
'                .Rows(.Rows.Count - 1)("Selbstadministration") = rblKundenAdministration.Items.FindByValue(_Customer.Selfadministration).Text
'                .Rows(.Rows.Count - 1)("SelbstadministrationInfo") = _Customer.SelfadministrationInfo.ToString
'                .Rows(.Rows.Count - 1)("Kundensperrre") = _Customer.Locked.ToString

'                Dim dvAppAssigned As DataView = GetAppAssignedView(intCustomerId, cn)
'                Dim strAnwendungen As String = ""
'                Dim j As Int32
'                For j = 0 To dvAppAssigned.Count - 1
'                    strAnwendungen &= dvAppAssigned(j)("AppFriendlyName").ToString & vbNewLine
'                Next
'                .Rows(.Rows.Count - 1)("Anwendungen") = strAnwendungen

'                If Not _Customer.CustomerContact Is Nothing Then
'                    .Rows(.Rows.Count - 1)("Kontakt- Name") = _Customer.CustomerContact.Name
'                    .Rows(.Rows.Count - 1)("Kontakt- Adresse") = _Customer.CustomerContact.Address
'                    .Rows(.Rows.Count - 1)("Mailadresse Anzeigetext") = _Customer.CustomerContact.MailDisplay
'                    .Rows(.Rows.Count - 1)("Mailadresse") = _Customer.CustomerContact.Mail
'                    .Rows(.Rows.Count - 1)("Web-Adresse Anzeigetext") = _Customer.CustomerContact.WebDisplay
'                    .Rows(.Rows.Count - 1)("Web-Adresse") = _Customer.CustomerContact.Web
'                End If
'                .Rows(.Rows.Count - 1)("Logo") = _Customer.CustomerStyle.LogoPath.ToString
'                .Rows(.Rows.Count - 1)("Logo2") = _Customer.LogoPath2.ToString
'                .Rows(.Rows.Count - 1)("Stylesheets") = _Customer.CustomerStyle.CssPath.ToString
'                .Rows(.Rows.Count - 1)("Handbuch") = _Customer.DocuPath
'                .Rows(.Rows.Count - 1)("Max. Anzahl Benutzer") = _Customer.MaxUser.ToString
'                .Rows(.Rows.Count - 1)("Organisationsanzeige") = _Customer.ShowOrganization
'                .Rows(.Rows.Count - 1)("Nur Kundengruppen administrieren") = _Customer.OrgAdminRestrictToCustomerGroup
'            End With
'            Return tblPar
'        Catch ex As Exception
'            m_App.WriteErrorText(1, m_User.UserName, "CustomerManagement", "SetOldLogParameters", ex.ToString)

'            Dim dt As New DataTable()
'            dt.Columns.Add("Fehler beim Erstellen der Log-Parameter", System.Type.GetType("System.String"))
'            dt.Rows.Add(dt.NewRow)
'            Dim str As String = ex.Message
'            If Not ex.InnerException Is Nothing Then
'                str &= ": " & ex.InnerException.Message
'            End If
'            dt.Rows(0)("Fehler beim Erstellen der Log-Parameter") = str
'            Return dt
'        Finally
'            If cn.State <> ConnectionState.Closed Then
'                cn.Close()
'            End If
'        End Try
'    End Function

'    Private Function SetNewLogParameters(ByVal tblPar As DataTable) As DataTable
'        Try
'            If tblPar Is Nothing Then
'                tblPar = CreateLogTableStructure()
'            End If
'            With tblPar
'                .Rows.Add(.NewRow)
'                .Rows(.Rows.Count - 1)("Status") = "Neu"
'                .Rows(.Rows.Count - 1)("Firmenname") = txtCustomerName.Text
'                .Rows(.Rows.Count - 1)("KUNNR") = txtKUNNR.Text
'                .Rows(.Rows.Count - 1)("Neues Kennwort nach n Tagen") = txtNewPwdAfterNDays.Text
'                .Rows(.Rows.Count - 1)("Konto sperren nach n Fehlversuchen") = txtLockedAfterNLogins.Text
'                .Rows(.Rows.Count - 1)("Mehrfaches Login") = chkAllowMultipleLogin.Checked
'                .Rows(.Rows.Count - 1)("Mindestl�nge") = txtPwdLength.Text
'                .Rows(.Rows.Count - 1)("n numerische Zeichen") = txtPwdNNumeric.Text
'                .Rows(.Rows.Count - 1)("n Gro�buchstaben") = txtNCapitalLetter.Text
'                .Rows(.Rows.Count - 1)("n Sonderzeichen") = txtNSpecialCharacter.Text
'                .Rows(.Rows.Count - 1)("Sperre letzte n Kennworte") = txtPwdHistoryNEntries.Text
'                .Rows(.Rows.Count - 1)("Selbstadministration") = rblKundenAdministration.SelectedItem.Text
'                .Rows(.Rows.Count - 1)("SelbstadministrationInfo") = txtKundenadministrationBeschreibung.Text
'                .Rows(.Rows.Count - 1)("Kundensperre") = chkKundenSperre.Checked
'                Dim _li As ListItem
'                Dim strAnwendungen As String = ""
'                For Each _li In lstAppAssigned.Items
'                    strAnwendungen &= _li.Text & vbNewLine
'                Next
'                .Rows(.Rows.Count - 1)("Anwendungen") = strAnwendungen
'                .Rows(.Rows.Count - 1)("Kontakt- Name") = txtCName.Text
'                .Rows(.Rows.Count - 1)("Kontakt- Adresse") = txtCAddress.Text
'                .Rows(.Rows.Count - 1)("Mailadresse Anzeigetext") = txtCMailDisplay.Text
'                .Rows(.Rows.Count - 1)("Mailadresse") = txtCMail.Text
'                .Rows(.Rows.Count - 1)("Web-Adresse Anzeigetext") = txtCWebDisplay.Text
'                .Rows(.Rows.Count - 1)("Web-Adresse") = txtCWeb.Text
'                .Rows(.Rows.Count - 1)("Logo") = txtLogoPath.Text
'                .Rows(.Rows.Count - 1)("Logo2") = txtLogoPath2.Text
'                .Rows(.Rows.Count - 1)("Stylesheets") = txtCssPath.Text
'                .Rows(.Rows.Count - 1)("Handbuch") = txtDocuPath.Text
'                .Rows(.Rows.Count - 1)("Max. Anzahl Benutzer") = txtMaxUser.Text
'                .Rows(.Rows.Count - 1)("Organisationsanzeige") = chkShowOrganization.Checked
'                .Rows(.Rows.Count - 1)("Nur Kundengruppen administrieren") = cbxOrgAdminRestrictToCustomerGroup.Checked
'            End With
'            Return tblPar
'        Catch ex As Exception
'            m_App.WriteErrorText(1, m_User.UserName, "CustomerManagement", "SetNewLogParameters", ex.ToString)

'            Dim dt As New DataTable()
'            dt.Columns.Add("Fehler beim Erstellen der Log-Parameter", System.Type.GetType("System.String"))
'            dt.Rows.Add(dt.NewRow)
'            Dim str As String = ex.Message
'            If Not ex.InnerException Is Nothing Then
'                str &= ": " & ex.InnerException.Message
'            End If
'            dt.Rows(0)("Fehler beim Erstellen der Log-Parameter") = str
'            Return dt
'        End Try
'    End Function

'    Private Function CreateLogTableStructure() As DataTable
'        Dim tblPar As New DataTable()
'        With tblPar
'            .Columns.Add("Status", System.Type.GetType("System.String"))
'            .Columns.Add("Firmenname", System.Type.GetType("System.String"))
'            .Columns.Add("KUNNR", System.Type.GetType("System.String"))
'            .Columns.Add("DAD", System.Type.GetType("System.Boolean"))
'            .Columns.Add("Neues Kennwort nach n Tagen", System.Type.GetType("System.String"))
'            .Columns.Add("Konto sperren nach n Fehlversuchen", System.Type.GetType("System.String"))
'            .Columns.Add("Mehrfaches Login", System.Type.GetType("System.Boolean"))
'            .Columns.Add("Mindestl�nge", System.Type.GetType("System.String"))
'            .Columns.Add("n numerische Zeichen", System.Type.GetType("System.String"))
'            .Columns.Add("n Gro�buchstaben", System.Type.GetType("System.String"))
'            .Columns.Add("n Sonderzeichen", System.Type.GetType("System.String"))
'            .Columns.Add("Sperre letzte n Kennworte", System.Type.GetType("System.String"))
'            .Columns.Add("Anwendungen", System.Type.GetType("System.String"))
'            .Columns.Add("Kontakt- Name", System.Type.GetType("System.String"))
'            .Columns.Add("Kontakt- Adresse", System.Type.GetType("System.String"))
'            .Columns.Add("Mailadresse Anzeigetext", System.Type.GetType("System.String"))
'            .Columns.Add("Mailadresse", System.Type.GetType("System.String"))
'            .Columns.Add("Web-Adresse Anzeigetext", System.Type.GetType("System.String"))
'            .Columns.Add("Web-Adresse", System.Type.GetType("System.String"))
'            .Columns.Add("Logo", System.Type.GetType("System.String"))
'            .Columns.Add("Selbstadministration", System.Type.GetType("System.String"))
'            .Columns.Add("SelbstadministrationInfo", System.Type.GetType("System.String"))
'            .Columns.Add("Kundensperre", System.Type.GetType("System.String"))

'            '��� JVE 18.09.2006: Logo2
'            .Columns.Add("Logo2", System.Type.GetType("System.String"))
'            '-------------------------
'            .Columns.Add("Stylesheets", System.Type.GetType("System.String"))
'            .Columns.Add("Handbuch", System.Type.GetType("System.String"))
'            .Columns.Add("Max. Anzahl Benutzer", System.Type.GetType("System.String"))
'            .Columns.Add("Organisationsanzeige", System.Type.GetType("System.String"))
'            .Columns.Add("Nur Kundengruppen administrieren", System.Type.GetType("System.String"))
'        End With
'        Return tblPar
'    End Function
'#End Region

'#Region " Events "
'    Private Sub btnSuche_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSuche.Click
'        Search(True, True, True, True)
'    End Sub

'    Private Sub dgSearchResult_SortCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs) Handles dgSearchResult.SortCommand
'        Dim strSort As String = e.SortExpression
'        If Not ViewState("ResultSort") Is Nothing AndAlso ViewState("ResultSort").ToString = strSort Then
'            strSort &= " DESC"
'        End If
'        ViewState("ResultSort") = strSort
'        FillDataGrid(strSort)
'    End Sub

'    Private Sub dgSearchResult_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles dgSearchResult.ItemCommand
'        If e.CommandName = "Edit" Then
'            EditEditMode(CInt(e.Item.Cells(0).Text))
'            dgSearchResult.SelectedIndex = e.Item.ItemIndex
'        ElseIf e.CommandName = "Delete" Then
'            EditDeleteMode(CInt(e.Item.Cells(0).Text))
'            dgSearchResult.SelectedIndex = e.Item.ItemIndex
'        End If
'    End Sub

'    Private Sub dgSearchResult_PageIndexChanged(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles dgSearchResult.PageIndexChanged
'        dgSearchResult.CurrentPageIndex = e.NewPageIndex
'        FillDataGrid()
'    End Sub

'    Private Sub lbtnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lbtnCancel.Click
'        If Not trConfirm.Visible Then
'            Search(, True)
'        Else
'            ConfirmMode(False)
'        End If
'    End Sub

'    Private Sub lbtnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lbtnNew.Click
'        SearchMode(False)
'        ClearEdit()
'        Dim cn As New SqlClient.SqlConnection(m_User.App.Connectionstring)
'        Try
'            cn.Open()
'            FillUnAssigned(CInt(txtCustomerID.Text), cn)
'            'hier nicht den wert aus dem Grid, ist logischer weise 0, sondern des Users Kundennummer
'            FillAccountingArea(m_User.Customer.CustomerId, True)
'        Finally
'            If cn.State <> ConnectionState.Closed Then
'                cn.Close()
'            End If
'        End Try
'    End Sub
'    Private Sub lbtnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lbtnSave.Click
'        Dim intCustomerId As Integer = CInt(txtCustomerID.Text)
'        'Do SAP-Stuff here...
'        Dim i_Kunnr As String = Right("0000000000" & Me.txtKUNNR.Text, 10)
'        Dim blnNoData As Boolean = False
'        Session("AppID") = "00000"
'        Try
'            Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_M_KUNDEN_ANZEIGEN", m_App, m_User, Me)

'            myProxy.setImportParameter("I_KUNNR", i_Kunnr)
'            myProxy.callBapi()

'            Dim tblTemp2 As DataTable = myProxy.getExportTable("GS_WEB")
'            'Dim tblTemp2 As String = myProxy.getExportParameter("GS_WEB")

'            If blnNoData Then
'                plhConfirm.Controls.Add(New LiteralControl("<BR><b>Keine Daten gefunden!<b/><BR><BR>"))
'            ElseIf tblTemp2.Rows.Count > 0 Then

'                Dim sb As New System.Text.StringBuilder()
'                With sb
'                    .AppendFormat("KUNNR:&nbsp{0}<BR>", tblTemp2.Rows(0)("Kunnr").ToString)
'                    .AppendFormat("NAME1:&nbsp{0}<BR>", tblTemp2.Rows(0)("Name1").ToString)
'                    .AppendFormat("NAME2:&nbsp{0}<BR>", tblTemp2.Rows(0)("Name2").ToString)
'                    .AppendFormat("STRAS:&nbsp{0}<BR>", tblTemp2.Rows(0)("Stras").ToString)
'                    .AppendFormat("PSTLZ:&nbsp{0}<BR>", tblTemp2.Rows(0)("Pstlz").ToString)
'                    .AppendFormat("ORT01:&nbsp{0}<BR>", tblTemp2.Rows(0)("Ort01").ToString)
'                End With
'                plhConfirm.Controls.Add(New LiteralControl(String.Format("<BR><b>Bitte �berpr�fen Sie vor dem Best�tigen Ihre eingaben und vergleichen Sie diese mit dem Kundendatensatz aus SAP:</b><BR><BR>{0}<BR>", sb.ToString)))
'            Else
'                plhConfirm.Controls.Add(New LiteralControl("<BR><b>Keine Daten gefunden!<b/><BR><BR>"))
'            End If
'            ConfirmMode(True)
'            txtCAddress.Text = TranslateHTML(txtCAddress.Text, TranslationDirection.SaveHTML)
'        Catch ex As Exception
'            If HelpProcedures.CastSapBizTalkErrorMessage(ex.Message) = "NO_DATA" Then
'                blnNoData = True
'            End If
'        Finally

'        End Try
'    End Sub

'    Private Sub lbtnDelete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lbtnDelete.Click
'        Dim tblLogParameter As DataTable
'        Dim cn As New SqlClient.SqlConnection(m_User.App.Connectionstring)
'        Try
'            Dim _customer As New Customer(CInt(txtCustomerID.Text))

'            cn.Open()
'            tblLogParameter = New DataTable
'            tblLogParameter = SetOldLogParameters(CInt(txtCustomerID.Text), tblLogParameter)
'            If Not _customer.HasUser(cn) Then
'                _customer.Delete(cn)
'                Log(_customer.CustomerId.ToString, "Firma l�schen", tblLogParameter)

'                Search(True, True, True, True)
'                lblMessage.Text = "Der Kunde wurde gel�scht."
'            Else
'                lblMessage.Text = "Der Kunde kann nicht gel�scht werden, da ihm noch Benutzer zugeordnet sind."
'            End If
'        Catch ex As Exception
'            m_App.WriteErrorText(1, m_User.UserName, "CustomerManagement", "lbtnDelete_Click", ex.ToString)

'            lblError.Text = ex.Message
'            If Not ex.InnerException Is Nothing Then
'                lblError.Text &= ": " & ex.InnerException.Message
'            End If
'            tblLogParameter = New DataTable
'            Log(txtCustomerID.Text, lblError.Text, tblLogParameter, "ERR")
'        Finally
'            If cn.State <> ConnectionState.Closed Then
'                cn.Close()
'            End If
'        End Try
'    End Sub

'    Private Sub btnAssign_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAssign.Click
'        Dim _item As ListItem
'        Dim _coll As New ListItemCollection()

'        For Each _item In lstAppUnAssigned.Items
'            If _item.Selected = True Then
'                _item.Selected = False
'                _coll.Add(_item)
'            End If
'        Next

'        For Each _item In _coll
'            lstAppAssigned.Items.Add(_item)
'            lstAppUnAssigned.Items.Remove(_item)
'        Next
'    End Sub

'    Private Sub btnUnAssign_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnUnAssign.Click
'        Dim _item As ListItem
'        Dim _coll As New ListItemCollection()

'        For Each _item In lstAppAssigned.Items
'            If _item.Selected = True Then
'                _item.Selected = False
'                _coll.Add(_item)
'            End If
'        Next

'        For Each _item In _coll
'            lstAppUnAssigned.Items.Add(_item)
'            lstAppAssigned.Items.Remove(_item)
'        Next
'    End Sub

'    Private Sub lbtnConfirm_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lbtnConfirm.Click
'        Dim tblLogParameter As DataTable
'        Dim cn As New SqlClient.SqlConnection(m_User.App.Connectionstring)

'        Dim StrDaysLockMessage As String
'        Dim StrDaysDelMessage As String

'        Try

'            cn.Open()
'            Dim intCustomerId As Integer = CInt(txtCustomerID.Text)
'            Dim strLogMsg As String = "Firma anlegen"
'            If Not (intCustomerId = -1) Then
'                strLogMsg = "Firma �ndern"
'                tblLogParameter = New DataTable
'                tblLogParameter = SetOldLogParameters(intCustomerId, tblLogParameter)
'            End If
'            Dim _customer As New Customer(intCustomerId, _
'                                                txtCustomerName.Text, _
'                                                txtKUNNR.Text, _
'                                                cbxMaster.Checked, _
'                                                False, _
'                                                txtCName.Text, _
'                                                TranslateHTML(txtCAddress.Text, TranslationDirection.SaveHTML), _
'                                                txtCMailDisplay.Text, _
'                                                txtCMail.Text, _
'                                                txtKundenpostfach.Text, _
'                                                txtKundenhotline.Text, _
'                                                txtKundenfax.Text, _
'                                                txtCWebDisplay.Text, _
'                                                txtCWeb.Text, _
'                                                CInt(txtNewPwdAfterNDays.Text), _
'                                                CInt(txtLockedAfterNLogins.Text), _
'                                                CInt(txtPwdNNumeric.Text), _
'                                                CInt(txtPwdLength.Text), _
'                                                CInt(txtNCapitalLetter.Text), _
'                                                CInt(txtNSpecialCharacter.Text), _
'                                                CInt(txtPwdHistoryNEntries.Text), _
'                                                txtLogoPath.Text, _
'                                                txtLogoPath2.Text, _
'                                                txtDocuPath.Text, _
'                                                txtCssPath.Text, _
'                                                chkAllowMultipleLogin.Checked, _
'                                                CInt(txtMaxUser.Text), _
'                                                chkShowOrganization.Checked, _
'                                                cbxOrgAdminRestrictToCustomerGroup.Checked, _
'                                                cbxPwdDontSendEmail.Checked, _
'                                                cbxNameInputOptional.Checked, _
'                                                chkShowDistrikte.Checked, _
'                                                cbxForcePasswordQuestion.Checked, _
'                                                chkIpRestriction.Checked, _
'                                                txtIpStandardUser.Text, _
'                                                CInt(ddlAccountingArea.SelectedValue), _
'                                                rblKundenAdministration.SelectedValue, _
'                                                txtKundenadministrationBeschreibung.Text, _
'                                                chkKundenSperre.Checked, _
'                                                chkTeamviewer.Checked, _
'                                                cbxUsernameSendEmail.Checked, _
'                                                CInt(ddlPortalLink.SelectedValue))

'            If (txtUserLockTime.Text.Trim() <> "") Then
'                If CInt(txtUserLockTime.Text) >= 5 Then
'                    _customer.DaysUntilLock = CInt(txtUserLockTime.Text)
'                    StrDaysLockMessage = ""
'                Else
'                    _customer.DaysUntilLock = 90
'                    StrDaysLockMessage = "Der Wert f�r Tage bis zur automatischen Sperrung muss mindestens 5 betragen! Der Wert wurde automatisch auf den Standardwert (90 Tage) gesetzt. </br>"
'                End If
'            Else
'                _customer.DaysUntilLock = 90    'Standardwert f�r Tage bis Sperrung
'                StrDaysLockMessage = "Der Wert f�r Tage bis zur automatischen Sperrung muss mindestens 5 betragen! Der Wert wurde automatisch auf den Standardwert (90 Tage) gesetzt. </br>"
'            End If
'            If (txtUserDeleteTime.Text.Trim() <> "") Then
'                If CInt(txtUserDeleteTime.Text) >= 5 Then
'                    _customer.DaysUntilDelete = CInt(txtUserDeleteTime.Text)
'                    StrDaysDelMessage = ""
'                Else
'                    _customer.DaysUntilDelete = 9999
'                    StrDaysDelMessage = "Der Wert f�r Tage bis zur automatischen L�schung muss mindestens 5 betragen! Der Wert wurde automatisch auf den Standardwert (9999 Tage) gesetzt. </br>"
'                End If
'            Else
'                _customer.DaysUntilDelete = 9999 'Standardwert f�r Tage bis L�schen
'                StrDaysDelMessage = "Der Wert f�r Tage bis zur automatischen L�schung muss mindestens 5 betragen! Der Wert wurde automatisch auf den Standardwert (9999 Tage) gesetzt. </br>"
'            End If
'            _customer.Save(cn)
'            tblLogParameter = New DataTable
'            tblLogParameter = SetNewLogParameters(tblLogParameter)
'            Log(_customer.CustomerId.ToString, strLogMsg, tblLogParameter)

'            'Anwendungen zuordnen
'            Dim dvAppAssigned As DataView
'            If intCustomerId = -1 Then
'                intCustomerId = _customer.CustomerId
'                txtCustomerID.Text = intCustomerId.ToString
'            End If
'            'If Not m_context.Cache("myCustomerAppAssigned") Is Nothing Then
'            '    dvAppAssigned = CType(m_context.Cache("myCustomerAppAssigned"), DataView)
'            If Not Session("myCustomerAppAssigned") Is Nothing Then
'                dvAppAssigned = CType(Session("myCustomerAppAssigned"), DataView)
'            Else
'                dvAppAssigned = GetAppAssignedView(intCustomerId, cn)
'            End If
'            Dim _assignment As New Kernel.AppAssignments(intCustomerId, Kernel.AssignmentType.Customer)
'            _assignment.Save(dvAppAssigned, lstAppAssigned.Items, cn)

'            'Archive zuordnen
'            Dim dvArchivAssigned As DataView
'            If intCustomerId = -1 Then
'                intCustomerId = _customer.CustomerId
'                txtCustomerID.Text = intCustomerId.ToString
'            End If
'            'If Not m_context.Cache("myCustomerArchivAssigned") Is Nothing Then
'            '    dvArchivAssigned = CType(m_context.Cache("myCustomerArchivAssigned"), DataView)
'            If Not Session("myCustomerArchivAssigned") Is Nothing Then
'                dvArchivAssigned = CType(Session("myCustomerArchivAssigned"), DataView)
'            Else
'                dvArchivAssigned = GetArchivAssignedView(intCustomerId, cn)
'            End If
'            Dim _archivassignment As New Kernel.ArchivAssignments(intCustomerId, Kernel.AssignmentType.Customer)
'            _archivassignment.Save(dvArchivAssigned, lstArchivAssigned.Items, cn)

'            Search(True, True, , True)
'            lblMessage.Text = "Die �nderungen wurden gespeichert."
'        Catch ex As Exception
'            m_App.WriteErrorText(1, m_User.UserName, "CustomerManagement", "lbtnSave_Click", ex.ToString)

'            lblError.Text = ex.Message
'            If Not ex.InnerException Is Nothing Then
'                lblError.Text &= ": " & ex.InnerException.Message
'            End If
'            tblLogParameter = New DataTable
'            Log(txtCustomerID.Text, lblError.Text, tblLogParameter, "ERR")
'        Finally
'            If cn.State <> ConnectionState.Closed Then
'                cn.Close()
'            End If
'        End Try
'    End Sub

'    Private Sub btnAssignArchiv_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAssignArchiv.Click
'        Dim _item As ListItem
'        Dim _coll As New ListItemCollection()

'        For Each _item In lstArchivUnAssigned.Items
'            If _item.Selected = True Then
'                _item.Selected = False
'                _coll.Add(_item)
'            End If
'        Next

'        For Each _item In _coll
'            lstArchivAssigned.Items.Add(_item)
'            lstArchivUnAssigned.Items.Remove(_item)
'        Next
'    End Sub

'    Private Sub btnUnAssignArchiv_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnUnAssignArchiv.Click
'        Dim _item As ListItem
'        Dim _coll As New ListItemCollection()

'        For Each _item In lstArchivAssigned.Items
'            If _item.Selected = True Then
'                _item.Selected = False
'                _coll.Add(_item)
'            End If
'        Next

'        For Each _item In _coll
'            lstArchivUnAssigned.Items.Add(_item)
'            lstArchivAssigned.Items.Remove(_item)
'        Next
'    End Sub

'    Private Sub btnNewIpAddress_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNewIpAddress.Click
'        If (Not txtCustomerID.Text = "-1") And (Not txtIpAddress.Text.Trim(" "c).Length = 0) Then
'            Dim cn As New SqlClient.SqlConnection(m_User.App.Connectionstring)
'            Try
'                cn.Open()

'                Dim cmdNew As New SqlClient.SqlCommand("INSERT INTO IpAddresses VALUES (" & txtCustomerID.Text & ",'" & txtIpAddress.Text & "')", cn)
'                cmdNew.ExecuteNonQuery()
'                EditEditMode(CInt(txtCustomerID.Text))
'                txtIpAddress.Text = ""
'            Catch ex As Exception
'                Me.lblError.Text = ex.Message
'                txtIpAddress.Text = "s. Fehlertext"
'            Finally
'                If cn.State <> ConnectionState.Closed Then
'                    cn.Close()
'                End If
'            End Try
'        End If
'    End Sub

'    Private Sub grdIpAddresses_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles grdIpAddresses.ItemCommand
'        If e.CommandName = "Delete" Then
'            Dim cn As New SqlClient.SqlConnection(m_User.App.Connectionstring)
'            Try
'                cn.Open()

'                Dim cmdNew As New SqlClient.SqlCommand("DELETE FROM IpAddresses WHERE CustomerID=" & txtCustomerID.Text & " AND IpAddress='" & e.Item.Cells(0).Text & "'", cn)
'                cmdNew.ExecuteNonQuery()
'                EditEditMode(CInt(txtCustomerID.Text))
'                txtIpAddress.Text = ""
'            Finally
'                If cn.State <> ConnectionState.Closed Then
'                    cn.Close()
'                End If
'            End Try
'        End If
'    End Sub
'#End Region

'    Private Sub gvBusinessOwner_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvBusinessOwner.RowCommand
'        If e.CommandName = "Sort" Then
'            fillBusinessownerGrid(gvBusinessOwner.PageIndex, e.CommandArgument)
'        ElseIf e.CommandName = "entfernen" Then
'            deleteFromCustomerInfoTable(e.CommandArgument)
'            fillBusinessownerGrid(gvBusinessOwner.PageIndex)
'        End If

'    End Sub

'    Private Sub gvAdminPerson_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvAdminPerson.RowCommand
'        If e.CommandName = "Sort" Then
'            fillAdminpersonGrid(gvAdminPerson.PageIndex, e.CommandArgument)
'        ElseIf e.CommandName = "entfernen" Then
'            deleteFromCustomerInfoTable(e.CommandArgument)
'            fillAdminpersonGrid(gvAdminPerson.PageIndex)
'        End If

'    End Sub

'    Protected Sub rblKundenAdministration_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles rblKundenAdministration.SelectedIndexChanged
'        setKundenadministrationInfoVisibility()
'    End Sub

'    ''' <summary>
'    ''' F�llt die DropdownList mit den Login-Links
'    ''' </summary>
'    ''' <remarks></remarks>
'    Private Sub LoadLoginLinks()

'        Dim TempTable As New DataTable
'        Dim cn As New SqlClient.SqlConnection(m_User.App.Connectionstring)
'        cn.Open()

'        Dim daLoginLink As SqlClient.SqlDataAdapter
'        daLoginLink = New SqlClient.SqlDataAdapter("SELECT * FROM WebUserUploadLoginLink", cn)

'        daLoginLink.Fill(TempTable)

'        cn.Close()
'        cn.Dispose()

'        ddlPortalLink.Items.Clear()

'        Dim dItem As New ListItem

'        dItem.Text = " --Auswahl-- "
'        dItem.Value = 0

'        ddlPortalLink.Items.Add(dItem)

'        For Each row As DataRow In TempTable.Rows
'            dItem = New ListItem

'            dItem.Text = row("Text").ToString
'            dItem.Value = row("ID")

'            ddlPortalLink.Items.Add(dItem)
'        Next

'    End Sub

'    Protected Sub cbxPwdDontSendEmail_CheckedChanged(sender As Object, e As EventArgs) Handles cbxPwdDontSendEmail.CheckedChanged
'        If cbxPwdDontSendEmail.Checked Then
'            cbxForcePasswordQuestion.Enabled = False
'            cbxForcePasswordQuestion.Checked = False
'        Else
'            cbxForcePasswordQuestion.Enabled = True
'        End If
'    End Sub
'End Class

'' ************************************************
'' $History: CustomerManagement.aspx.vb $
'' 
'' *****************  Version 17  *****************
'' User: Dittbernerc  Date: 11.05.11   Time: 11:31
'' Updated in $/CKAG/admin
'' 
'' *****************  Version 16  *****************
'' User: Dittbernerc  Date: 9.05.11    Time: 13:39
'' Updated in $/CKAG/admin
'' 
'' *****************  Version 15  *****************
'' User: Rudolpho     Date: 4.02.10    Time: 15:32
'' Updated in $/CKAG/admin
'' ITA: 2918
'' 
'' *****************  Version 14  *****************
'' User: Rudolpho     Date: 26.10.09   Time: 11:44
'' Updated in $/CKAG/admin
'' 
'' *****************  Version 13  *****************
'' User: Rudolpho     Date: 28.04.09   Time: 16:42
'' Updated in $/CKAG/admin
'' 
'' *****************  Version 12  *****************
'' User: Jungj        Date: 19.03.09   Time: 16:25
'' Updated in $/CKAG/admin
'' ITA 2156 testfertig
'' 
'' *****************  Version 11  *****************
'' User: Jungj        Date: 19.03.09   Time: 11:27
'' Updated in $/CKAG/admin
'' ITA 2156 fertig
'' 
'' *****************  Version 10  *****************
'' User: Jungj        Date: 18.03.09   Time: 17:59
'' Updated in $/CKAG/admin
'' ITA 2156 unfertig
'' 
'' *****************  Version 9  *****************
'' User: Jungj        Date: 18.03.09   Time: 15:17
'' Updated in $/CKAG/admin
'' ITa 2156 unfertig
'' 
'' *****************  Version 8  *****************
'' User: Rudolpho     Date: 6.01.09    Time: 11:45
'' Updated in $/CKAG/admin
'' ITA 2503  Cache durch Session ersetzt
'' 
'' *****************  Version 7  *****************
'' User: Jungj        Date: 6.10.08    Time: 10:05
'' Updated in $/CKAG/admin
'' ITA 2295 Nachbesserungen
'' 
'' *****************  Version 6  *****************
'' User: Jungj        Date: 6.10.08    Time: 9:19
'' Updated in $/CKAG/admin
'' ITA 2295 fertig
'' 
'' *****************  Version 5  *****************
'' User: Jungj        Date: 2.10.08    Time: 11:15
'' Updated in $/CKAG/admin
'' ITA 2295 berichtigung der Buchungskreisauswahl und einschr�nkung der
'' Buchungskreisauswahl auf den Buchungskreis des Users
'' 
'' *****************  Version 4  *****************
'' User: Hartmannu    Date: 11.09.08   Time: 11:34
'' Updated in $/CKAG/admin
'' Fixing Admin-�nderungen
'' 
'' *****************  Version 3  *****************
'' User: Hartmannu    Date: 9.09.08    Time: 13:42
'' Updated in $/CKAG/admin
'' ITA 2152 und 2158
'' 
'' *****************  Version 2  *****************
'' User: Rudolpho     Date: 11.04.08   Time: 15:47
'' Updated in $/CKAG/admin
'' Migration
'' 
'' *****************  Version 1  *****************
'' User: Fassbenders  Date: 4.04.08    Time: 14:47
'' Created in $/CKAG/admin
'' 
'' *****************  Version 18  *****************
'' User: Uha          Date: 21.01.08   Time: 18:09
'' Updated in $/CKG/Admin/AdminWeb
'' ITA 1644: Erm�glicht Login nur mit IP und festgelegtem Benutzer
'' 
'' *****************  Version 17  *****************
'' User: Rudolpho     Date: 6.12.07    Time: 14:36
'' Updated in $/CKG/Admin/AdminWeb
'' ITA: 1440
'' 
'' *****************  Version 16  *****************
'' User: Uha          Date: 30.08.07   Time: 15:17
'' Updated in $/CKG/Admin/AdminWeb
'' ITA 1280: Bugfix
'' 
'' *****************  Version 15  *****************
'' User: Uha          Date: 30.08.07   Time: 12:36
'' Updated in $/CKG/Admin/AdminWeb
'' ITA 1280: Pa�wortversand im Web auf Benutzerwunsch
'' 
'' *****************  Version 14  *****************
'' User: Uha          Date: 27.08.07   Time: 17:13
'' Updated in $/CKG/Admin/AdminWeb
'' ITA 1269: Archivverwaltung/-zuordnung jetzt auch per Web
'' 
'' *****************  Version 13  *****************
'' User: Fassbenders  Date: 27.07.07   Time: 16:01
'' Updated in $/CKG/Admin/AdminWeb
'' 
'' *****************  Version 12  *****************
'' User: Uha          Date: 15.05.07   Time: 15:29
'' Updated in $/CKG/Admin/AdminWeb
'' �nderungen aus StartApplication vom 11.05.2007
'' 
'' *****************  Version 11  *****************
'' User: Uha          Date: 13.03.07   Time: 10:53
'' Updated in $/CKG/Admin/AdminWeb
'' History-Eintrag vorbereitet
'' 
'' ************************************************
