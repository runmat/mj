
Imports CKG.Base.Kernel.Admin
Imports CKG.Base.Kernel.Security
Imports CKG.Base.Kernel.Common.Common
Imports CKG.Portal.PageElements

Public Class AppManagement
    Inherits System.Web.UI.Page

    Protected WithEvents ucStyles As Styles

#Region " Vom Web Form Designer generierter Code "
    Protected WithEvents lblError As System.Web.UI.WebControls.Label
    Protected WithEvents dgSearchResult As System.Web.UI.WebControls.DataGrid
    Protected WithEvents lblMessage As System.Web.UI.WebControls.Label
    Protected WithEvents trSearchResult As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents trEditUser As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents txtAppID As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtAppName As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtAppComment As System.Web.UI.WebControls.TextBox
    Protected WithEvents ddlAppParent As System.Web.UI.WebControls.DropDownList
    Protected WithEvents cbxAppInMenu As System.Web.UI.WebControls.CheckBox
    Protected WithEvents ddlAppType As System.Web.UI.WebControls.DropDownList
    Protected WithEvents txtAppFriendlyName As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtAppURL As System.Web.UI.WebControls.TextBox
    Protected WithEvents lnkUserManagement As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lnkGroupManagement As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lbtnNew As System.Web.UI.WebControls.LinkButton
    Protected WithEvents lbtnSave As System.Web.UI.WebControls.LinkButton
    Protected WithEvents lbtnCancel As System.Web.UI.WebControls.LinkButton
    Protected WithEvents lbtnDelete As System.Web.UI.WebControls.LinkButton
    Protected WithEvents trSearch As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents trSearchSpacer As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents lnkCustomerManagement As System.Web.UI.WebControls.HyperLink
    Protected WithEvents txtAppRank As System.Web.UI.WebControls.TextBox
    Protected WithEvents Form1 As System.Web.UI.HtmlControls.HtmlForm
    Protected WithEvents lnkColumnTranslation As System.Web.UI.WebControls.LinkButton
    Protected WithEvents lnkOrganizationManagement As System.Web.UI.WebControls.HyperLink
    Protected WithEvents ddlAuthorizationlevel As System.Web.UI.WebControls.DropDownList
    Protected WithEvents cbxBatchAuthorization As System.Web.UI.WebControls.CheckBox
    Protected WithEvents txtAppParam As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtFilterAppName As System.Web.UI.WebControls.TextBox
    Protected WithEvents btnSuche As System.Web.UI.WebControls.Button
    Protected WithEvents lnkZugeordneteBAPIs As System.Web.UI.WebControls.LinkButton
    Protected WithEvents lnkArchivManagement As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lnkFieldTranslation As System.Web.UI.WebControls.LinkButton
    Protected WithEvents txtFilterAppFriendlyName As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtSchwellwert As System.Web.UI.WebControls.TextBox

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
    Private m_User As User
    Private m_App As App
    Private m_context As HttpContext = HttpContext.Current
#End Region

#Region " Data and Function "
    Private Sub FillForm()
        trEditUser.Visible = False
        trSearchResult.Visible = False
        Dim cn As New SqlClient.SqlConnection(m_User.App.Connectionstring)
        FillAppParent(cn)
        FillAppType(cn)
        If Not m_User.HighestAdminLevel = AdminLevel.Master Then
            CustomerAdminMode()
        End If
    End Sub

    Private Sub FillDataGrid()
        Dim strSort As String = "AppID"
        If Not ViewState("ResultSort") Is Nothing Then
            strSort = ViewState("ResultSort").ToString
        End If
        FillDataGrid(strSort)
    End Sub

    Private Sub FillDataGrid(ByVal strSort As String)
        trSearchResult.Visible = True
        Dim dvApplication As DataView

        'If Not m_context.Cache("myAppListView") Is Nothing Then
        '    dvApplication = CType(m_context.Cache("myAppListView"), DataView)
        If Not Session("myAppListView") Is Nothing Then
            dvApplication = CType(Session("myAppListView"), DataView)
        Else
            Dim cn As New SqlClient.SqlConnection(m_User.App.Connectionstring)
            Try
                Dim dtApplication As ApplicationList

                cn.Open()

                dtApplication = New ApplicationList(txtFilterAppName.Text, _
                                                    cn, _
                                                    txtFilterAppFriendlyName.Text)

                dvApplication = dtApplication.DefaultView
                'm_context.Cache.Insert("myAppListView", dvApplication, Nothing, DateTime.Now.AddMinutes(20), TimeSpan.Zero)
                Session("myAppListView") = dvApplication
            Finally
                If cn.State <> ConnectionState.Closed Then
                    cn.Close()
                End If
            End Try
        End If
        dvApplication.Sort = strSort
        If dvApplication.Count > dgSearchResult.PageSize Then
            dgSearchResult.PagerStyle.Visible = True
        Else
            dgSearchResult.PagerStyle.Visible = False
        End If

        dgSearchResult.DataSource = dvApplication
        dgSearchResult.DataBind()
    End Sub

    Private Sub ReFillAppParent(ByVal intAppID As Integer)
        Dim dvAppParent As DataView
        'If Not m_context.Cache("myAppParentView") Is Nothing Then
        '    dvAppParent = CType(m_context.Cache("myAppParentView"), DataView)
        If Not Session("myAppParentView") Is Nothing Then
            dvAppParent = CType(Session("myAppParentView"), DataView)
        Else
            Dim cn As New SqlClient.SqlConnection(m_User.App.Connectionstring)
            Try
                cn.Open()
                Dim dtApplication As New ApplicationList(cn)
                dvAppParent = dtApplication.DefaultView
                ' m_context.Cache.Insert("myAppParentView", dvAppParent, Nothing, DateTime.Now.AddMinutes(20), TimeSpan.Zero)
                Session("myAppParentView") = dvAppParent
            Finally
                If cn.State <> ConnectionState.Closed Then
                    cn.Close()
                End If
            End Try
        End If
        dvAppParent.RowFilter = "AppID<>" & CStr(intAppID)
        dvAppParent.Sort = "AppURL"
        With ddlAppParent
            .Items.Clear()
            .DataSource = dvAppParent
            .DataValueField = "AppId"
            '.DataTextField = "AppName"
            .DataTextField = "AppURL"
            .DataBind()
        End With
    End Sub

    Private Sub FillAppParent(ByVal cn As SqlClient.SqlConnection)
        Dim dvAppParent As DataView
        Dim dtApplication As New ApplicationList(cn)
        dvAppParent = dtApplication.DefaultView
        'm_context.Cache.Insert("myAppParentView", dvAppParent, Nothing, DateTime.Now.AddMinutes(20), TimeSpan.Zero)
        Session("myAppParentView") = dvAppParent
        dvAppParent.Sort = "AppURL"
        With ddlAppParent
            .Items.Clear()
            .DataSource = dvAppParent
            .DataValueField = "AppId"
            '.DataTextField = "AppName"
            .DataTextField = "AppURL"
            .DataBind()
        End With
    End Sub

    Private Sub FillAppType(ByVal cn As SqlClient.SqlConnection)
        Dim dtAppType As New Kernel.AppTypeList(cn)
        dtAppType.DefaultView.Sort = "AppType"
        With ddlAppType
            .DataSource = dtAppType.DefaultView
            .DataValueField = "AppType"
            .DataTextField = "AppType"
            .DataBind()
        End With
    End Sub

    Private Function FillEdit(ByVal intAppId As Integer) As Boolean
        SearchMode(False)
        Dim cn As New SqlClient.SqlConnection(m_User.App.Connectionstring)
        Try
            cn.Open()
            Dim _App As New Kernel.Application(intAppId, cn)
            ReFillAppParent(intAppId)
            txtAppID.Text = _App.AppId.ToString
            txtAppName.Text = _App.AppName
            txtAppFriendlyName.Text = _App.AppFriendlyName
            ddlAppType.SelectedItem.Selected = False
            ddlAppType.Items.FindByValue(_App.AppType.ToString).Selected = True
            txtAppURL.Text = _App.AppURL.ToString
            cbxAppInMenu.Checked = _App.AppInMenu
            cbxBatchAuthorization.Checked = _App.BatchAuthorization
            txtAppComment.Text = _App.AppComment
            txtSchwellwert.Text = _App.AppSchwellwert
            ddlAppParent.SelectedItem.Selected = False
            ddlAppParent.Items.FindByValue(_App.AppParent.ToString).Selected = True
            ddlAuthorizationlevel.SelectedItem.Selected = False
            ddlAuthorizationlevel.Items.FindByValue(_App.Authorizationlevel.ToString).Selected = True
            txtAppRank.Text = _App.AppRank.ToString
            txtAppParam.Text = CType(_App.AppParam, String)
            Return True
        Finally
            If cn.State <> ConnectionState.Closed Then
                cn.Close()
            End If
        End Try
    End Function

    Private Sub ClearEdit()
        txtAppID.Text = "-1"
        txtAppName.Text = ""
        txtAppFriendlyName.Text = ""
        ddlAppType.SelectedItem.Selected = False
        txtAppURL.Text = ""
        cbxAppInMenu.Checked = False
        cbxBatchAuthorization.Checked = False
        txtAppComment.Text = ""
        ddlAppParent.SelectedItem.Selected = False
        txtAppRank.Text = ""
        txtSchwellwert.Text = ""
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
        txtAppID.Enabled = Not blnLock
        txtAppID.BackColor = System.Drawing.Color.FromName(strBackColor)
        txtAppName.Enabled = Not blnLock
        txtAppName.BackColor = System.Drawing.Color.FromName(strBackColor)
        txtAppFriendlyName.Enabled = Not blnLock
        txtAppFriendlyName.BackColor = System.Drawing.Color.FromName(strBackColor)
        ddlAppType.Enabled = Not blnLock
        ddlAppType.BackColor = System.Drawing.Color.FromName(strBackColor)
        txtAppURL.Enabled = Not blnLock
        txtAppURL.BackColor = System.Drawing.Color.FromName(strBackColor)
        cbxAppInMenu.Enabled = Not blnLock
        cbxBatchAuthorization.Enabled = Not blnLock
        txtAppComment.Enabled = Not blnLock
        txtAppComment.BackColor = System.Drawing.Color.FromName(strBackColor)
        ddlAppParent.Enabled = Not blnLock
        ddlAppParent.BackColor = System.Drawing.Color.FromName(strBackColor)
        ddlAuthorizationlevel.Enabled = Not blnLock
        ddlAuthorizationlevel.BackColor = System.Drawing.Color.FromName(strBackColor)
        txtAppRank.Enabled = Not blnLock
        txtAppRank.BackColor = System.Drawing.Color.FromName(strBackColor)
        txtSchwellwert.Enabled = Not blnLock
        txtSchwellwert.BackColor = System.Drawing.Color.FromName(strBackColor)
    End Sub

    Private Sub CustomerAdminMode()
        SearchMode(False)
        'trApp.Visible = False

        If m_User.Groups.Count > 0 Then
            If m_User.IsCustomerAdmin Then
                LockEdit(False)
                EditEditMode(m_User.Customer.CustomerId)
            End If
        End If
    End Sub

    Private Sub EditEditMode(ByVal intAppId As Integer)
        If Not FillEdit(intAppId) Then
            LockEdit(True)
            lbtnSave.Enabled = False
        Else
            lbtnSave.Enabled = True
        End If
        lbtnCancel.Text = "Verwerfen"
    End Sub

    Private Sub EditDeleteMode(ByVal intGroupId As Integer)
        If Not FillEdit(intGroupId) Then
            lbtnDelete.Enabled = False
        Else
            lblMessage.Text = "Möchten Sie die Anwendung wirklich löschen?"
            lbtnDelete.Enabled = True
        End If
        LockEdit(True)
        lbtnCancel.Text = "Abbrechen"
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
            'm_context.Cache.Remove("myAppListView")
            Session.Remove("myAppListView")
        End If
        If blnResetSelectedIndex Then dgSearchResult.SelectedIndex = -1
        If blnResetPageIndex Then dgSearchResult.CurrentPageIndex = 0
        If m_User.HighestAdminLevel > AdminLevel.Customer Then
            SearchMode()
            If blnRefillDataGrid Then FillDataGrid()
        Else
            CustomerAdminMode()
        End If
    End Sub

    Private Sub Log(ByVal strIdentification As String, ByVal strDescription As String, ByVal tblParameters As DataTable, Optional ByVal strCategory As String = "APP")
        Dim logApp As New Base.Kernel.Logging.Trace(m_User.App.Connectionstring, m_User.App.SaveLogAccessSAP, m_User.App.LogLevel)

        ' strCategory
        Dim strUserName As String = m_User.UserName ' strUserName
        Dim strSessionID As String = Session.SessionID ' strSessionID
        Dim intSource As Integer = CInt(Request.QueryString("AppID")) ' intSource 
        Dim strTask As String = "Admin - Anwendungsverwaltung" ' strTask
        ' strIdentification
        ' strDescription
        Dim strCustomerName As String = m_User.CustomerName ' strCustomername
        Dim blnIsTestUser As Boolean = m_User.IsTestUser ' blnIsTestUser
        Dim intSeverity As Integer = 0 ' intSeverity 

        logApp.WriteEntry(strCategory, strUserName, strSessionID, intSource, strTask, strIdentification, strDescription, strCustomerName, m_User.Customer.CustomerId, blnIsTestUser, intSeverity, tblParameters)
    End Sub

    Private Function SetOldLogParameters(ByVal intAppId As Int32, ByVal tblPar As DataTable) As DataTable
        Dim cn As New SqlClient.SqlConnection(m_User.App.Connectionstring)
        Try
            cn.Open()
            Dim _App As New Kernel.Application(intAppId, cn)

            If tblPar Is Nothing Then
                tblPar = CreateLogTableStructure()
            End If
            With tblPar
                .Rows.Add(.NewRow)
                .Rows(.Rows.Count - 1)("Status") = "Alt"
                .Rows(.Rows.Count - 1)("Anwendungs-Name") = _App.AppName
                .Rows(.Rows.Count - 1)("Freundlicher Name") = _App.AppFriendlyName
                .Rows(.Rows.Count - 1)("Typ") = _App.AppType.ToString
                .Rows(.Rows.Count - 1)("URL") = _App.AppURL.ToString
                .Rows(.Rows.Count - 1)("in Menü") = _App.AppInMenu
                .Rows(.Rows.Count - 1)("Kommentar") = _App.AppComment
                .Rows(.Rows.Count - 1)("Gehört zu") = _App.AppParent.ToString
                .Rows(.Rows.Count - 1)("Autorisierungslevel") = _App.Authorizationlevel.ToString
                .Rows(.Rows.Count - 1)("Rang") = _App.AppRank.ToString
            End With
            Return tblPar
        Catch ex As Exception
            m_App.WriteErrorText(1, m_User.UserName, "AppManagement", "SetOldLogParameters", ex.ToString)
            Dim dt As New DataTable()
            dt.Columns.Add("Fehler beim Erstellen der Log-Parameter", System.Type.GetType("System.String"))
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

    Private Function SetNewLogParameters(ByVal tblPar As DataTable) As DataTable
        Try
            If tblPar Is Nothing Then
                tblPar = CreateLogTableStructure()
            End If
            With tblPar
                .Rows.Add(.NewRow)
                .Rows(.Rows.Count - 1)("Status") = "Neu"
                .Rows(.Rows.Count - 1)("Anwendungs-Name") = txtAppName.Text
                .Rows(.Rows.Count - 1)("Freundlicher Name") = txtAppFriendlyName.Text
                .Rows(.Rows.Count - 1)("Typ") = ddlAppType.SelectedItem.Text
                .Rows(.Rows.Count - 1)("URL") = txtAppURL.Text
                .Rows(.Rows.Count - 1)("in Menü") = cbxAppInMenu.Checked
                .Rows(.Rows.Count - 1)("Kommentar") = txtAppComment.Text
                .Rows(.Rows.Count - 1)("Gehört zu") = ddlAppParent.SelectedItem.Text
                .Rows(.Rows.Count - 1)("Autorisierungslevel") = ddlAuthorizationlevel.SelectedItem.Text
                .Rows(.Rows.Count - 1)("Sammelautorisierung") = cbxBatchAuthorization.Checked
                .Rows(.Rows.Count - 1)("Rang") = txtAppRank.Text
            End With
            Return tblPar
        Catch ex As Exception
            m_App.WriteErrorText(1, m_User.UserName, "AppManagement", "SetNewLogParameters", ex.ToString)
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
            .Columns.Add("Anwendungs-Name", System.Type.GetType("System.String"))
            .Columns.Add("Freundlicher Name", System.Type.GetType("System.String"))
            .Columns.Add("Typ", System.Type.GetType("System.String"))
            .Columns.Add("URL", System.Type.GetType("System.String"))
            .Columns.Add("in Menü", System.Type.GetType("System.Boolean"))
            .Columns.Add("Kommentar", System.Type.GetType("System.String"))
            .Columns.Add("Gehört zu", System.Type.GetType("System.String"))
            .Columns.Add("Autorisierungslevel", System.Type.GetType("System.String"))
            .Columns.Add("Sammelautorisierung", System.Type.GetType("System.Boolean"))
            .Columns.Add("Rang", System.Type.GetType("System.String"))
        End With
        Return tblPar
    End Function
#End Region

#Region " Events "

    Private Sub Page_InitComplete(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.InitComplete

    End Sub
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        ' Hier Benutzercode zur Seiteninitialisierung einfügen
        m_User = GetUser(Me)
        ucHeader.InitUser(m_User)
        ucStyles.TitleText = "Anwendungsverwaltung"
        AdminAuth(Me, m_User, AdminLevel.Master)

        Try
            m_App = New App(m_User)

            If Not IsPostBack Then
                lblError.Text = ""

                'Fülle ddlAuthorizationlevel mit festen Vorgaben
                Dim strAuthorLevels(4) As String
                strAuthorLevels(0) = "0 - ohne"
                strAuthorLevels(1) = "1 - niedrig"
                strAuthorLevels(2) = "2 - mittel"
                strAuthorLevels(3) = "3 - hoch"
                Dim i As Int32
                For i = 0 To 3
                    Dim listitem As New ListItem()
                    listitem.Value = CStr(i)
                    listitem.Text = strAuthorLevels(i)
                    ddlAuthorizationlevel.Items.Add(listitem)
                Next

                FillForm()
                Dim strAppID As String = CStr(Request.QueryString("AppID"))
                If Not strAppID = String.Empty Then
                    EditEditMode(CInt(strAppID))
                End If
            End If
        Catch ex As Exception
            lblError.Text = ex.ToString
            lblError.Visible = True
            m_App.WriteErrorText(1, m_User.UserName, "AppManagement", "PageLoad", lblError.Text)
        End Try
    End Sub

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
        If dgSearchResult.Items.Count = 0 Then
            Search(True, True)
        Else
            Search(, True)
        End If
    End Sub

    Private Sub lbtnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lbtnNew.Click
        SearchMode(False)
        ClearEdit()
        'Dim cn As New SqlClient.SqlConnection(m_User.App.Connectionstring)
        'cn.Open()
    End Sub

    Private Sub lbtnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lbtnSave.Click
        Dim tblLogParameter As DataTable
        Dim cn As New SqlClient.SqlConnection(m_User.App.Connectionstring)
        Try

            If Len(Trim(txtSchwellwert.Text)) > 0 Then
                If IsNumeric(Trim(txtSchwellwert.Text)) = False Then Err.Raise(-1, , "Nur Ziffern für Schwellwert erlaubt.")
                If Trim(txtSchwellwert.Text) = 0 Then txtSchwellwert.Text = Nothing
            End If


            Dim conn As New SqlClient.SqlConnection(m_User.App.Connectionstring)

            cn.Open()
            Dim intAppId As Integer = CInt(txtAppID.Text)
            Dim strLogMsg As String = "Anwendung anlegen"
            If Not (intAppId = -1) Then
                strLogMsg = "Anwendung ändern"
                tblLogParameter = New DataTable
                tblLogParameter = SetOldLogParameters(intAppId, tblLogParameter)
            End If
            Dim _App As New Kernel.Application(intAppId, _
                                                txtAppName.Text, _
                                                txtAppFriendlyName.Text, _
                                                ddlAppType.SelectedItem.Value.ToString, _
                                                txtAppURL.Text, _
                                                cbxAppInMenu.Checked, _
                                                txtAppComment.Text, _
                                                CInt(ddlAppParent.SelectedItem.Value.ToString), _
                                                CInt(txtAppRank.Text), _
                                                CInt(ddlAuthorizationlevel.SelectedItem.Value.ToString), _
                                                cbxBatchAuthorization.Checked, _
                                                txtSchwellwert.Text)

            Dim typ As Boolean
            _App.Save(cn, typ)

            'Save Parameters
            _App.SaveParams(conn, txtAppParam.Text, typ)

            'Save Zuordnungen, falls es sich um ein Child handelt
            _App.ReAssign(conn, _App.AppId, CInt(ddlAppParent.SelectedItem.Value.ToString))
            tblLogParameter = New DataTable
            tblLogParameter = SetNewLogParameters(tblLogParameter)
            Log(_App.AppId.ToString, strLogMsg, tblLogParameter)
            FillAppParent(cn)
            Search(True, True, , True)


            lblMessage.Text = "Die Änderungen wurden gespeichert."
        Catch ex As Exception
            m_App.WriteErrorText(1, m_User.UserName, "AppManagement", "lbtnSave_Click", ex.ToString)
            lblError.Text = ex.Message
            If Not ex.InnerException Is Nothing Then
                lblError.Text &= ": " & ex.InnerException.Message
            End If
            tblLogParameter = New DataTable
            Log(txtAppID.Text, lblError.Text, tblLogParameter, "ERR")
        Finally
            If cn.State <> ConnectionState.Closed Then
                cn.Close()
            End If
        End Try
    End Sub

    Private Sub lbtnDelete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lbtnDelete.Click
        Dim tblLogParameter As DataTable
        Dim cn As New SqlClient.SqlConnection(m_User.App.Connectionstring)
        Try
            Dim _app As New Kernel.Application(CInt(txtAppID.Text))

            cn.Open()
            tblLogParameter = New DataTable
            tblLogParameter = SetOldLogParameters(_app.AppId, tblLogParameter)
            If Not _app.HasChildren(cn) Then
                _app.Delete(cn)
                Log(_app.AppId.ToString, "Anwendung löschen", tblLogParameter)
                FillAppParent(cn)
                Search(True, True, True, True)
                lblMessage.Text = "Die Anwendung wurde gelöscht."
            Else
                lblMessage.Text = "Die Anwendung kann nicht gelöscht werden, da ihr noch Unteranwendungen zugeordnet sind."
            End If
        Catch ex As Exception
            m_App.WriteErrorText(1, m_User.UserName, "AppManagement", "lbtnDelete_Click", ex.ToString)
            lblError.Text = ex.Message
            If Not ex.InnerException Is Nothing Then
                lblError.Text &= ": " & ex.InnerException.Message
            End If
            tblLogParameter = New DataTable
            Log(txtAppID.Text, lblError.Text, tblLogParameter, "ERR")
        Finally
            If cn.State <> ConnectionState.Closed Then
                cn.Close()
            End If
        End Try
    End Sub

    Private Sub lnkColumnTranslation_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lnkColumnTranslation.Click
        Dim strAppID As String
        Dim strRetAppID As String = txtAppID.Text
        If CInt(ddlAppParent.SelectedItem.Value) < 1 Then
            strAppID = txtAppID.Text
        Else
            strAppID = ddlAppParent.SelectedItem.Value
        End If
        Session.Add("AppName", txtAppName.Text)
        Response.Redirect("ColumnTranslation.aspx?AppID=" & strAppID & "&RetAppID=" & strRetAppID)
    End Sub

    Private Sub btnSuche_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSuche.Click
        Search(True, True, True, True)
    End Sub

    Private Sub lnkZugeordneteBAPIs_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lnkZugeordneteBAPIs.Click
        Dim strAppID As String
        Dim strRetAppID As String = txtAppID.Text
        If CInt(ddlAppParent.SelectedItem.Value) < 1 Then
            strAppID = txtAppID.Text
        Else
            strAppID = ddlAppParent.SelectedItem.Value
        End If
        Session.Add("AppName", txtAppName.Text)
        Response.Redirect("ApplicationBAPI.aspx?AppID=" & strAppID & "&RetAppID=" & strRetAppID)
    End Sub

    Private Sub lnkFieldTranslation_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lnkFieldTranslation.Click
        Dim strAppURL As String
        Dim strRetAppID As String = txtAppID.Text

        Dim cn As New SqlClient.SqlConnection(m_User.App.Connectionstring)
        Try
            cn.Open()
            Dim _App As New Kernel.Application(CInt(txtAppID.Text), cn)
            strAppURL = _App.AppURL.ToString

            Session.Add("AppName", txtAppName.Text)
            Response.Redirect("FieldTranslation.aspx?AppURL=" & strAppURL & "&RetAppID=" & strRetAppID)
        Finally
            If cn.State <> ConnectionState.Closed Then
                cn.Close()
            End If
        End Try
    End Sub
#End Region

End Class

' ************************************************
' $History: AppManagement.aspx.vb $
' 
' *****************  Version 5  *****************
' User: Fassbenders  Date: 17.06.09   Time: 17:50
' Updated in $/CKAG/admin
' 
' *****************  Version 4  *****************
' User: Fassbenders  Date: 5.03.09    Time: 14:04
' Updated in $/CKAG/admin
' ITA: 2633
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
' *****************  Version 13  *****************
' User: Rudolpho     Date: 6.12.07    Time: 14:36
' Updated in $/CKG/Admin/AdminWeb
' ITA: 1440
' 
' *****************  Version 12  *****************
' User: Fassbenders  Date: 27.11.07   Time: 17:36
' Updated in $/CKG/Admin/AdminWeb
' 
' *****************  Version 11  *****************
' User: Uha          Date: 12.09.07   Time: 15:17
' Updated in $/CKG/Admin/AdminWeb
' ITA 1263: Pflege der Feldübersetzungen
' 
' *****************  Version 10  *****************
' User: Uha          Date: 27.08.07   Time: 17:13
' Updated in $/CKG/Admin/AdminWeb
' ITA 1269: Archivverwaltung/-zuordnung jetzt auch per Web
' 
' *****************  Version 9  *****************
' User: Uha          Date: 4.07.07    Time: 15:21
' Updated in $/CKG/Admin/AdminWeb
' Bugfixing für Application.ReAssign (Save Zuordnungen, falls es sich um
' ein Child handelt)
' 
' *****************  Version 8  *****************
' User: Uha          Date: 3.07.07    Time: 19:40
' Updated in $/CKG/Admin/AdminWeb
' Änderung einer Child-Applikation setzt Rechte bezüglich Kunden und
' Gruppen neu laut Parent-Applikation
' 
' *****************  Version 7  *****************
' User: Uha          Date: 13.03.07   Time: 10:53
' Updated in $/CKG/Admin/AdminWeb
' History-Eintrag vorbereitet
' 
' ************************************************
