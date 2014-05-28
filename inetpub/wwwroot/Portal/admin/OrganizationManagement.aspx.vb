
Imports CKG.Base.Kernel.Admin
Imports CKG.Base.Kernel.Security
Imports CKG.Base.Kernel.Common.Common
Imports CKG.Portal.PageElements

Public Class OrganizationManagement
    Inherits System.Web.UI.Page

    Protected WithEvents ucStyles As Styles

#Region " Vom Web Form Designer generierter Code "
    Protected WithEvents lblError As System.Web.UI.WebControls.Label
    Protected WithEvents lblCustomer As System.Web.UI.WebControls.Label
    Protected WithEvents ddlFilterCustomer As System.Web.UI.WebControls.DropDownList
    Protected WithEvents dgSearchResult As System.Web.UI.WebControls.DataGrid
    Protected WithEvents lblMessage As System.Web.UI.WebControls.Label
    Protected WithEvents trSearch As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents trSearchResult As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents trEditUser As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents trSearchSpacer As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents trApp As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents lnkAppManagement As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lbtnNew As System.Web.UI.WebControls.LinkButton
    Protected WithEvents lbtnSave As System.Web.UI.WebControls.LinkButton
    Protected WithEvents lbtnCancel As System.Web.UI.WebControls.LinkButton
    Protected WithEvents lbtnDelete As System.Web.UI.WebControls.LinkButton
    Protected WithEvents lnkUserManagement As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lnkCustomerManagement As System.Web.UI.WebControls.HyperLink
    Protected WithEvents trCustomer As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents txtCustomer As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtCustomerID As System.Web.UI.WebControls.TextBox
    Protected WithEvents Form1 As System.Web.UI.HtmlControls.HtmlForm
    Protected WithEvents lnkGroupManagement As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lblOrganizationName As System.Web.UI.WebControls.Label
    Protected WithEvents txtFilterOrganizationName As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtOrganizationID As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtOrganizationName As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtOrganizationReference As System.Web.UI.WebControls.TextBox
    Protected WithEvents cbxAllOrganizations As System.Web.UI.WebControls.CheckBox
    Protected WithEvents txtCssPath As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtLogoPath As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtCWeb As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtCWebDisplay As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtCMail As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtCMailDisplay As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtCAddress As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtCName As System.Web.UI.WebControls.TextBox
    Protected WithEvents trStyle As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents trOrganizationName As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents btnSuche As System.Web.UI.WebControls.LinkButton
    Protected WithEvents lnkArchivManagement As System.Web.UI.WebControls.HyperLink
    Protected WithEvents ucHeader As Header
    Protected WithEvents txtKundenpostfach As System.Web.UI.WebControls.TextBox
    'Dieser Aufruf ist f�r den Web Form-Designer erforderlich.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: Diese Methode ist f�r den Web Form-Designer erforderlich
        'Verwenden Sie nicht den Code-Editor zur Bearbeitung.
        InitializeComponent()
    End Sub

#End Region

#Region " Membervariables "
    Private m_User As User
    Private m_App As App
    Private m_context As HttpContext = HttpContext.Current
#End Region

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        ' Hier Benutzercode zur Seiteninitialisierung einf�gen
        m_User = GetUser(Me)
        ucHeader.InitUser(m_User)
        ucStyles.TitleText = "Organisationsverwaltung"
        AdminAuth(Me, m_User, AdminLevel.Customer)

        Try
            m_App = New App(m_User)

            If Not IsPostBack Then
                lblError.Text = ""

                FillForm()
            End If
        Catch ex As Exception
            m_App.WriteErrorText(1, m_User.UserName, "OrganizationManagement", "Page_Load", ex.ToString)

            lblError.Text = ex.ToString
            lblError.Visible = True
        End Try
    End Sub

#Region " Data and Function "
    Private Sub FillForm()
        Dim cn As New SqlClient.SqlConnection(m_User.App.Connectionstring)
        cn.Open()
        FillCustomer(cn)
        trEditUser.Visible = False
        trSearchResult.Visible = False

        If m_User.HighestAdminLevel = AdminLevel.Master Then
            ddlFilterCustomer.Visible = True
            txtFilterOrganizationName.Visible = True
            lblCustomer.Visible = False
            'wenn SuperUser und �bergeordnete Firma
            If m_User.Customer.AccountingArea = -1 Then
                lnkAppManagement.Visible = True
            End If

        Else
            lnkArchivManagement.Visible = False
            lnkCustomerManagement.Visible = False
            lnkAppManagement.Visible = False
            If m_User.IsCustomerAdmin Then
                txtFilterOrganizationName.Visible = True
            Else
                lnkCustomerManagement.Visible = False
                OrganizationAdminMode()
                trStyle.Visible = False
            End If
        End If
        lblCustomer.Text = m_User.Customer.CustomerName
        lblOrganizationName.Visible = Not txtFilterOrganizationName.Visible
    End Sub

    Private Sub FillCustomer(ByVal cn As SqlClient.SqlConnection)
        Dim dtCustomers As Kernel.CustomerList
        dtCustomers = New Kernel.CustomerList(m_User.Customer.AccountingArea, cn, True, False)

        Dim dv As DataView = dtCustomers.DefaultView
        dv.Sort = "Customername"
        'm_context.Cache.Insert("myCustomerListView", dv, Nothing, DateTime.Now.AddMinutes(20), TimeSpan.Zero)
        Session("myCustomerListView") = dv
        With ddlFilterCustomer
            .DataSource = dv
            .DataTextField = "Customername"
            .DataValueField = "CustomerID"
            .DataBind()
            .Items.FindByValue(m_User.Customer.CustomerId.ToString).Selected = True
        End With
    End Sub

    Private Sub FillDataGrid()
        Dim strSort As String = "OrganizationID"
        If Not ViewState("ResultSort") Is Nothing Then
            strSort = ViewState("ResultSort").ToString
        End If
        FillDataGrid(strSort)
    End Sub
    Private Sub FillDataGrid(ByVal strSort As String)
        trSearchResult.Visible = True
        Dim dvOrganization As DataView

        'If Not m_context.Cache("myOrganizationListView") Is Nothing Then
        '    dvOrganization = CType(m_context.Cache("myOrganizationListView"), DataView)
        If Not Session("myOrganizationListView") Is Nothing Then
            dvOrganization = CType(Session("myOrganizationListView"), DataView)
        Else
            Dim dtOrganization As OrganizationList
            Dim cn As New SqlClient.SqlConnection(m_User.App.Connectionstring)
            cn.Open()

            Dim intTemp As Integer
            If m_User.HighestAdminLevel = AdminLevel.Master Then
                intTemp = CInt(ddlFilterCustomer.SelectedItem.Value)
            Else
                intTemp = m_User.Customer.CustomerId
            End If

            dtOrganization = New OrganizationList(txtFilterOrganizationName.Text, _
                                                intTemp, _
                                                cn, _
                                                m_User.Customer.AccountingArea)
            dvOrganization = dtOrganization.DefaultView
            'm_context.Cache.Insert("myOrganizationListView", dvOrganization, Nothing, DateTime.Now.AddMinutes(20), TimeSpan.Zero)
            Session("myOrganizationListView") = dvOrganization
        End If
        dvOrganization.Sort = strSort
        If dvOrganization.Count > dgSearchResult.PageSize Then
            dgSearchResult.PagerStyle.Visible = True
        Else
            dgSearchResult.PagerStyle.Visible = False
        End If

        With dgSearchResult
            .DataSource = dvOrganization
            .DataBind()
        End With
    End Sub

    Private Function FillEdit(ByVal intOrganizationId As Integer) As Boolean
        SearchMode(False)
        Dim cn As New SqlClient.SqlConnection(m_User.App.Connectionstring)
        cn.Open()
        Dim _Organization As New Organization(intOrganizationId, cn)
        txtOrganizationID.Text = _Organization.OrganizationId.ToString
        txtOrganizationName.Text = _Organization.OrganizationName
        txtOrganizationReference.Text = _Organization.OrganizationReference
        cbxAllOrganizations.Checked = _Organization.AllOrganizations
        txtLogoPath.Text = _Organization.LogoPath
        txtCssPath.Text = _Organization.CssPath
        If Not _Organization.OrganizationContact Is Nothing Then
            txtCName.Text = _Organization.OrganizationContact.Name
            txtCAddress.Text = _Organization.OrganizationContact.Address
            txtCMailDisplay.Text = _Organization.OrganizationContact.MailDisplay
            txtCMail.Text = _Organization.OrganizationContact.Mail
            txtCWebDisplay.Text = _Organization.OrganizationContact.WebDisplay
            txtCWeb.Text = _Organization.OrganizationContact.Web
        End If
        Dim dvCustomer As New DataView
        'If Not m_context.Cache("myCustomerListView") Is Nothing Then
        '    dvCustomer = CType(m_context.Cache("myCustomerListView"), DataView)
        If Not Session("myCustomerListView") Is Nothing Then
            dvCustomer = CType(Session("myCustomerListView"), DataView)
        Else
            Dim dtCustomers As Kernel.CustomerList
            dtCustomers = New Kernel.CustomerList(m_User.Customer.AccountingArea, cn, True, False)

            dvCustomer.Sort = "Customername"
            dvCustomer = dtCustomers.DefaultView
            'm_context.Cache.Insert("myCustomerListView", dvCustomer, Nothing, DateTime.Now.AddMinutes(20), TimeSpan.Zero)
            Session("myCustomerListView") = dvCustomer
        End If
        txtCustomerID.Text = _Organization.CustomerId.ToString
        If _Organization.CustomerId > 0 Then
            dvCustomer.Sort = "CustomerID"
            txtCustomer.Text = dvCustomer(dvCustomer.Find(_Organization.CustomerId)).Item("CustomerName").ToString
        End If
        Return True
    End Function

    Private Sub ClearEdit()
        txtOrganizationID.Text = "-1"
        txtOrganizationName.Text = ""
        txtOrganizationReference.Text = "999"
        txtCName.Text = ""
        txtCAddress.Text = ""
        txtCMailDisplay.Text = ""
        txtCMail.Text = ""
        txtCWebDisplay.Text = ""
        txtCWeb.Text = ""
        txtLogoPath.Text = ""
        txtCssPath.Text = ""
        Dim intCustomerID As Integer = CInt(ddlFilterCustomer.SelectedItem.Value)
        If intCustomerID > 0 Then
            txtCustomerID.Text = intCustomerID.ToString
            txtCustomer.Text = ddlFilterCustomer.SelectedItem.Text
        Else
            txtCustomerID.Text = m_User.Customer.CustomerId.ToString
            txtCustomer.Text = m_User.Customer.CustomerName
        End If
        lbtnSave.Visible = True
        lbtnDelete.Visible = False
        LockEdit(False)
    End Sub

    Private Sub LockEdit(ByVal blnLock As Boolean)
        Dim strBackColor As String = "White"
        If blnLock Then
            strBackColor = "LightGray"
        End If
        txtOrganizationID.Enabled = Not blnLock
        txtOrganizationID.BackColor = System.Drawing.Color.FromName(strBackColor)
        txtOrganizationName.Enabled = Not blnLock
        txtOrganizationName.BackColor = System.Drawing.Color.FromName(strBackColor)
        txtOrganizationReference.Enabled = Not blnLock
        txtOrganizationReference.BackColor = System.Drawing.Color.FromName(strBackColor)
        cbxAllOrganizations.Enabled = Not blnLock
        txtCName.Enabled = Not blnLock
        txtCName.BackColor = System.Drawing.Color.FromName(strBackColor)
        txtCAddress.Enabled = Not blnLock
        txtCAddress.BackColor = System.Drawing.Color.FromName(strBackColor)
        txtCMailDisplay.Enabled = Not blnLock
        txtCMailDisplay.BackColor = System.Drawing.Color.FromName(strBackColor)
        txtCMail.Enabled = Not blnLock
        txtCMail.BackColor = System.Drawing.Color.FromName(strBackColor)
        txtCWebDisplay.Enabled = Not blnLock
        txtCWebDisplay.BackColor = System.Drawing.Color.FromName(strBackColor)
        txtCWeb.Enabled = Not blnLock
        txtCWeb.BackColor = System.Drawing.Color.FromName(strBackColor)
        txtLogoPath.Enabled = Not blnLock
        txtLogoPath.BackColor = System.Drawing.Color.FromName(strBackColor)
        txtCssPath.Enabled = Not blnLock
        txtCssPath.BackColor = System.Drawing.Color.FromName(strBackColor)
    End Sub

    Private Sub OrganizationAdminMode()
        SearchMode(False)
        trApp.Visible = False

        If Not m_User.Organization Is Nothing Then
            If m_User.Organization.OrganizationAdmin Then
                EditEditMode(m_User.Organization.OrganizationId)
            End If
        End If
    End Sub

    Private Sub EditEditMode(ByVal intOrganizationId As Integer)
        If Not FillEdit(intOrganizationId) Then
            LockEdit(True)
            lbtnSave.Enabled = False
        Else
            lbtnSave.Enabled = True
        End If
        lbtnCancel.Text = "&#149;&nbsp;Verwerfen"
    End Sub

    Private Sub EditDeleteMode(ByVal intOrganizationId As Integer)
        If Not FillEdit(intOrganizationId) Then
            lbtnDelete.Enabled = False
        Else
            lblMessage.Text = "M�chten Sie die Organisation wirklich l�schen?"
            lbtnDelete.Enabled = True
        End If
        LockEdit(True)
        lbtnCancel.Text = "&#149;&nbsp;Abbrechen"
        lbtnSave.Visible = False
        lbtnDelete.Visible = True
    End Sub

    Private Sub SearchMode(Optional ByVal blnSearchMode As Boolean = True)
        trEditUser.Visible = Not blnSearchMode
        trSearch.Visible = blnSearchMode
        trSearchSpacer.Visible = blnSearchMode
        trSearchResult.Visible = blnSearchMode
        lbtnSave.Visible = Not blnSearchMode
        lbtnCancel.Visible = Not blnSearchMode
        lbtnNew.Visible = blnSearchMode
    End Sub

    Private Sub Search(Optional ByVal blnRefillDataGrid As Boolean = False, Optional ByVal blnResetSelectedIndex As Boolean = False, Optional ByVal blnResetPageIndex As Boolean = False, Optional ByVal blnClearCache As Boolean = False)
        ClearEdit()
        If blnClearCache Then
            'm_context.Cache.Remove("myOrganizationListView")
            Session.Remove("myOrganizationListView")
        End If
        If blnResetSelectedIndex Then dgSearchResult.SelectedIndex = -1
        If blnResetPageIndex Then dgSearchResult.CurrentPageIndex = 0
        If m_User.HighestAdminLevel > AdminLevel.Organization Then
            SearchMode()
            If blnRefillDataGrid Then FillDataGrid()
        Else
            OrganizationAdminMode()
        End If
    End Sub

    Private Sub Log(ByVal strIdentification As String, ByVal strDescription As String, ByVal tblParameters As DataTable, Optional ByVal strCategory As String = "APP")
        Dim logApp As New Base.Kernel.Logging.Trace(m_User.App.Connectionstring, m_User.App.SaveLogAccessSAP, m_User.App.LogLevel)

        ' strCategory
        Dim strUserName As String = m_User.UserName ' strUserName
        Dim strSessionID As String = Session.SessionID ' strSessionID
        Dim intSource As Integer = CInt(Request.QueryString("AppID")) ' intSource 
        Dim strTask As String = "Admin - Organisationsverwaltung" ' strTask
        ' strIdentification
        ' strDescription
        Dim strCustomerName As String = m_User.CustomerName ' strCustomername
        Dim blnIsTestUser As Boolean = m_User.IsTestUser ' blnIsTestUser
        Dim intSeverity As Integer = 0 ' intSeverity 

        logApp.WriteEntry(strCategory, strUserName, strSessionID, intSource, strTask, strIdentification, strDescription, strCustomerName, m_User.Customer.CustomerId, blnIsTestUser, intSeverity, tblParameters)
    End Sub

    Private Function SetOldLogParameters(ByVal intOrganizationId As Int32, ByVal tblPar As DataTable) As DataTable
        Try
            Dim cn As New SqlClient.SqlConnection(m_User.App.Connectionstring)
            cn.Open()
            Dim _Organization As New Organization(intOrganizationId, cn)

            If tblPar Is Nothing Then
                tblPar = CreateLogTableStructure()
            End If
            With tblPar
                .Rows.Add(.NewRow)
                .Rows(.Rows.Count - 1)("Status") = "Alt"
                .Rows(.Rows.Count - 1)("Organisationsname") = _Organization.OrganizationName
                .Rows(.Rows.Count - 1)("Organisationsreferenz") = _Organization.OrganizationReference
                .Rows(.Rows.Count - 1)("Zeige alle Organisationen") = _Organization.AllOrganizations
                .Rows(.Rows.Count - 1)("Logo") = _Organization.LogoPath
                .Rows(.Rows.Count - 1)("Stylesheets") = _Organization.CssPath

                If Not _Organization.OrganizationContact Is Nothing Then
                    .Rows(.Rows.Count - 1)("Kontakt- Name") = _Organization.OrganizationContact.Name
                    .Rows(.Rows.Count - 1)("Kontakt- Adresse") = _Organization.OrganizationContact.Address
                    .Rows(.Rows.Count - 1)("Mailadresse Anzeigetext") = _Organization.OrganizationContact.MailDisplay
                    .Rows(.Rows.Count - 1)("Mailadresse") = _Organization.OrganizationContact.Mail
                    .Rows(.Rows.Count - 1)("Web-Adresse Anzeigetext") = _Organization.OrganizationContact.WebDisplay
                    .Rows(.Rows.Count - 1)("Web-Adresse") = _Organization.OrganizationContact.Web
                End If

                Dim dvCustomer As New DataView
                'If Not m_context.Cache("myCustomerListView") Is Nothing Then
                '    dvCustomer = CType(m_context.Cache("myCustomerListView"), DataView)
                If Not Session("myCustomerListView") Is Nothing Then
                    dvCustomer = CType(Session("myCustomerListView"), DataView)
                Else
                    Dim dtCustomers As Kernel.CustomerList
                    dtCustomers = New Kernel.CustomerList(m_User.Customer.AccountingArea, cn, True, False)

                    dvCustomer.Sort = "Customername"
                    dvCustomer = dtCustomers.DefaultView
                    'm_context.Cache.Insert("myCustomerListView", dvCustomer, Nothing, DateTime.Now.AddMinutes(20), TimeSpan.Zero)
                    Session("myCustomerListView") = dvCustomer
                End If
                If _Organization.CustomerId > 0 Then
                    dvCustomer.Sort = "CustomerID"
                    .Rows(.Rows.Count - 1)("Firma") = dvCustomer(dvCustomer.Find(_Organization.CustomerId)).Item("CustomerName").ToString
                End If
            End With
            Return tblPar
        Catch ex As Exception
            m_App.WriteErrorText(1, m_User.UserName, "OrganizationManagement", "SetOldLogParameters", ex.ToString)

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

    Private Function SetNewLogParameters(ByVal tblPar As DataTable) As DataTable
        Try
            If tblPar Is Nothing Then
                tblPar = CreateLogTableStructure()
            End If
            With tblPar
                .Rows.Add(.NewRow)
                .Rows(.Rows.Count - 1)("Status") = "Neu"
                .Rows(.Rows.Count - 1)("Organisationsname") = txtOrganizationName.Text
                .Rows(.Rows.Count - 1)("Organisationsreferenz") = txtOrganizationReference.Text
                .Rows(.Rows.Count - 1)("Firma") = txtCustomer.Text
                .Rows(.Rows.Count - 1)("Zeige alle Organisationen") = cbxAllOrganizations.Checked
                .Rows(.Rows.Count - 1)("Kontakt- Name") = txtCName.Text
                .Rows(.Rows.Count - 1)("Kontakt- Adresse") = txtCAddress.Text
                .Rows(.Rows.Count - 1)("Mailadresse Anzeigetext") = txtCMailDisplay.Text
                .Rows(.Rows.Count - 1)("Mailadresse") = txtCMail.Text
                .Rows(.Rows.Count - 1)("Web-Adresse Anzeigetext") = txtCWebDisplay.Text
                .Rows(.Rows.Count - 1)("Web-Adresse") = txtCWeb.Text
                .Rows(.Rows.Count - 1)("Logo") = txtLogoPath.Text
                .Rows(.Rows.Count - 1)("Stylesheets") = txtCssPath.Text
            End With
            Return tblPar
        Catch ex As Exception
            m_App.WriteErrorText(1, m_User.UserName, "OrganizationManagement", "SetNewLogParameters", ex.ToString)

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
            .Columns.Add("Organisationsname", System.Type.GetType("System.String"))
            .Columns.Add("Organisationsreferenz", System.Type.GetType("System.String"))
            .Columns.Add("Firma", System.Type.GetType("System.String"))
            .Columns.Add("Zeige alle Organisationen", System.Type.GetType("System.Boolean"))
            .Columns.Add("Kontakt- Name", System.Type.GetType("System.String"))
            .Columns.Add("Kontakt- Adresse", System.Type.GetType("System.String"))
            .Columns.Add("Mailadresse Anzeigetext", System.Type.GetType("System.String"))
            .Columns.Add("Mailadresse", System.Type.GetType("System.String"))
            .Columns.Add("Web-Adresse Anzeigetext", System.Type.GetType("System.String"))
            .Columns.Add("Web-Adresse", System.Type.GetType("System.String"))
            .Columns.Add("Logo", System.Type.GetType("System.String"))
            .Columns.Add("Stylesheets", System.Type.GetType("System.String"))
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
        FillDataGrid(strSort)
    End Sub

    Private Sub dgSearchResult_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles dgSearchResult.ItemCommand
        If e.CommandName = "Edit" Then
            EditEditMode(CInt(e.Item.Cells(0).Text))
            dgSearchResult.SelectedIndex = e.Item.ItemIndex
        ElseIf e.CommandName = "Delete" Then
            EditDeleteMode(CInt(e.Item.Cells(0).Text))
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

    Private Sub lbtnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lbtnNew.Click
        Dim intCustomer As Integer = CInt(ddlFilterCustomer.SelectedItem.Value)

        If intCustomer < 1 Then
            lblError.Text = "W�hlen Sie bitte zun�chst eine Firma aus!"
        Else
            SearchMode(False)
            ClearEdit()
            Dim cn As New SqlClient.SqlConnection(m_User.App.Connectionstring)
            cn.Open()
        End If
    End Sub

    Private Sub lbtnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lbtnSave.Click
        Dim tblLogParameter As DataTable
        Try
            If txtOrganizationReference.Text = String.Empty And Not cbxAllOrganizations.Checked Then
                Throw New Exception("Das Feld Organisationsreferenz darf nicht leer sein, wenn ""Zeige ALLE Organisationen"" nicht gesetzt ist!")
            End If
            Dim cn As New SqlClient.SqlConnection(m_User.App.Connectionstring)
            cn.Open()
            Dim intOrganizationId As Integer = CInt(txtOrganizationID.Text)
            Dim strLogMsg As String = "Organisation anlegen"
            Dim blnNew As Boolean = True
            If Not (intOrganizationId = -1) Then
                blnNew = False
                strLogMsg = "Organisation �ndern"
                tblLogParameter = New DataTable
                tblLogParameter = SetOldLogParameters(intOrganizationId, tblLogParameter)
            End If

            Dim _Organization As New Organization(intOrganizationId, txtOrganizationName.Text, _
                                                CInt(txtCustomerID.Text), txtOrganizationReference.Text, _
                                                cbxAllOrganizations.Checked, False, _
                                                txtLogoPath.Text, txtCssPath.Text, _
                                                txtCName.Text, txtCAddress.Text, _
                                                txtCMailDisplay.Text, txtCMail.Text, _
                                                txtCWebDisplay.Text, txtCWeb.Text, _
                                                blnNew)
            _Organization.Save(cn)
            tblLogParameter = New DataTable
            tblLogParameter = SetNewLogParameters(tblLogParameter)
            Log(_Organization.OrganizationId.ToString, strLogMsg, tblLogParameter)

            Search(True, True, True, True)
            lblMessage.Text = "Die �nderungen wurden gespeichert."
        Catch ex As Exception
            m_App.WriteErrorText(1, m_User.UserName, "OrganizationManagement", "lbtnSave_Click", ex.ToString)

            lblError.Text = ex.Message
            If Not ex.InnerException Is Nothing Then
                lblError.Text &= ": " & ex.InnerException.Message
            End If
            tblLogParameter = New DataTable
            Log(txtOrganizationID.Text, lblError.Text, tblLogParameter, "ERR")
        End Try
    End Sub

    Private Sub lbtnDelete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lbtnDelete.Click
        Dim tblLogParameter As DataTable
        Try
            Dim _Organization As New Organization(CInt(txtOrganizationID.Text), txtOrganizationName.Text, _
                CInt(ddlFilterCustomer.SelectedItem.Value), txtOrganizationReference.Text, _
                cbxAllOrganizations.Checked, False, _
                txtLogoPath.Text, txtCssPath.Text, _
                txtCName.Text, txtCAddress.Text, _
                txtCMailDisplay.Text, txtCMail.Text, _
                txtCWebDisplay.Text, txtCWeb.Text, _
                False)
            Dim cn As New SqlClient.SqlConnection(m_User.App.Connectionstring)
            cn.Open()
            tblLogParameter = New DataTable
            tblLogParameter = SetOldLogParameters(CInt(txtOrganizationID.Text), tblLogParameter)

            If Not _Organization.HasUser(cn) Then
                _Organization.Delete(cn)
                Log(_Organization.OrganizationId.ToString, "Organisation l�schen", tblLogParameter)

                Search(True, True, True, True)
                lblMessage.Text = "Die Organisation wurde gel�scht."
            Else
                lblMessage.Text = "Die Organisation kann nicht gel�scht werden, da ihr noch Benutzer zugeordnet sind."
            End If
        Catch ex As Exception
            m_App.WriteErrorText(1, m_User.UserName, "OrganizationManagement", "lbtnDelete_Click", ex.ToString)

            lblError.Text = ex.Message
            If Not ex.InnerException Is Nothing Then
                lblError.Text &= ": " & ex.InnerException.Message
            End If
            tblLogParameter = New DataTable
            Log(txtOrganizationID.Text, lblError.Text, tblLogParameter, "ERR")
        End Try
    End Sub
#End Region

    Private Sub btnSuche_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSuche.Click
        Search(True, True, True, True)
    End Sub
End Class

' ************************************************
' $History: OrganizationManagement.aspx.vb $
' 
' *****************  Version 6  *****************
' User: Rudolpho     Date: 26.10.09   Time: 11:44
' Updated in $/CKAG/admin
' 
' *****************  Version 5  *****************
' User: Rudolpho     Date: 6.01.09    Time: 11:45
' Updated in $/CKAG/admin
' ITA 2503  Cache durch Session ersetzt
' 
' *****************  Version 4  *****************
' User: Jungj        Date: 6.10.08    Time: 9:19
' Updated in $/CKAG/admin
' ITA 2295 fertig
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
' *****************  Version 7  *****************
' User: Uha          Date: 30.08.07   Time: 12:36
' Updated in $/CKG/Admin/AdminWeb
' ITA 1280: Pa�wortversand im Web auf Benutzerwunsch
' 
' *****************  Version 6  *****************
' User: Uha          Date: 27.08.07   Time: 17:13
' Updated in $/CKG/Admin/AdminWeb
' ITA 1269: Archivverwaltung/-zuordnung jetzt auch per Web
' 
' *****************  Version 5  *****************
' User: Uha          Date: 13.03.07   Time: 10:53
' Updated in $/CKG/Admin/AdminWeb
' History-Eintrag vorbereitet
' 
' ************************************************
