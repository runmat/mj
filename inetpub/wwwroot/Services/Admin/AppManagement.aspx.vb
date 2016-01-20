Imports CKG.Base.Kernel.Admin
Imports CKG.Base.Kernel.Security
Imports CKG.Base.Kernel.Common.Common
Imports CKG.Services
Imports WebTools.Services

Partial Public Class AppManagement
    Inherits Page

#Region " Membervariables "

    Private m_User As User
    Private m_App As App
    Protected WithEvents GridNavigation1 As GridNavigation
#End Region

#Region " Events "

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load

        ' Hier Benutzercode zur Seiteninitialisierung einfügen
        m_User = GetUser(Me)
        AdminAuth(Me, m_User, AdminLevel.Master)
        GridNavigation1.setGridElment(dgSearchResult)
        Try
            m_App = New App(m_User)
            lblHead.Text = "Anwendungen"

            lblError.Text = ""
            lblMessage.Text = ""

            If Not IsPostBack Then
                If Not Session("App_EditID") Is Nothing Then
                    txtAppID.Text = CStr(Session("App_EditID"))
                    FillForm()
                    EditEditMode(CInt(txtAppID.Text))
                Else
                    FillForm()
                    Dim strAppID As String = CStr(Request.QueryString("AppID"))
                    If Not strAppID = String.Empty Then
                        EditEditMode(CInt(strAppID))
                    End If
                End If

            End If

        Catch ex As Exception
            lblError.Text = ex.ToString
            m_App.WriteErrorText(1, m_User.UserName, "AppManagement", "PageLoad", lblError.Text)
        End Try
    End Sub

    Private Sub lbtnCancel_Click(ByVal sender As System.Object, ByVal e As EventArgs) Handles lbtnCancel.Click
        Search(True, True)
    End Sub

    Private Sub lbtnNew_Click(ByVal sender As System.Object, ByVal e As EventArgs) Handles lbtnNew.Click
        SearchMode(False)
        ClearEdit()
    End Sub

    Private Sub lbtnSave_Click(ByVal sender As System.Object, ByVal e As EventArgs) Handles lbtnSave.Click
        Dim cn As New SqlClient.SqlConnection(m_User.App.Connectionstring)
        Try
            If String.IsNullOrEmpty(txtAppName.Text) Then
                lblError.Text = "Bitte geben Sie einen Anwendungsnamen an!"
                Exit Sub
            End If

            'Default-Reihenfolge für App ohne Menüeintrag, falls Reihenfolge-Feld nicht gefüllt
            If Not cbxAppInMenu.Checked And String.IsNullOrEmpty(txtAppRank.Text) Then
                txtAppRank.Text = "99"
            End If

            Dim conn As New SqlClient.SqlConnection(m_User.App.Connectionstring)

            cn.Open()
            Dim intAppId As Integer = CInt(txtAppID.Text)
            Dim strLogMsg As String = "Anwendung anlegen"
            If Not (intAppId = -1) Then
                strLogMsg = "Anwendung ändern"
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
                                                CInt(ddlAuthorizationlevel.SelectedItem.Value.ToString), cbxBatchAuthorization.Checked, _
                                                ddlAppTechType.SelectedValue, _
                                                txtAppDescription.Text, _
                                                CInt(txtMaxLevel.Text), CInt(txtMaxLevelsPerGroup.Text))
            Dim typ As Boolean
            _App.Save(cn, typ)

            'Save Parameters
            _App.SaveParams(conn, txtAppParam.Text, typ)

            'Save Zuordnungen, falls es sich um ein Child handelt
            _App.ReAssign(conn, _App.AppId, CInt(ddlAppParent.SelectedItem.Value.ToString))
            Dim tblLogParameter As DataTable = SetNewLogParameters()
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
            Log(txtAppID.Text, lblError.Text, New DataTable(), "ERR")
        Finally
            If cn.State <> ConnectionState.Closed Then
                cn.Close()
            End If
        End Try
    End Sub

    Private Sub lbtnDelete_Click(ByVal sender As System.Object, ByVal e As EventArgs) Handles lbtnDelete.Click
        Dim tblLogParameter As DataTable
        Dim cn As New SqlClient.SqlConnection(m_User.App.Connectionstring)
        Try
            Dim _app As New Kernel.Application(CInt(txtAppID.Text))

            cn.Open()
            tblLogParameter = SetOldLogParameters(_app.AppId)
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

    Private Sub lnkMvcReportSolution_Click(ByVal sender As System.Object, ByVal e As EventArgs) Handles lnkMvcReportSolution.Click
        Dim strAppID As String
        If CInt(ddlAppParent.SelectedItem.Value) < 1 Then
            strAppID = txtAppID.Text
        Else
            strAppID = ddlAppParent.SelectedItem.Value
        End If
        Dim url As String = "/ServicesMvc/Common/GridAdmin/ReportSolution?un=" & CryptoMd5.EncryptToUrlEncoded(strAppID & "-" & m_User.UserName & "-" & DateTime.Now.Ticks.ToString())
        Response.Redirect(url)
    End Sub

    Private Sub lnkColumnTranslation_Click(ByVal sender As System.Object, ByVal e As EventArgs) Handles lnkColumnTranslation.Click
        Dim strAppID As String
        Dim strRetAppID As String = txtAppID.Text
        If CInt(ddlAppParent.SelectedItem.Value) < 1 Then
            strAppID = txtAppID.Text
        Else
            strAppID = ddlAppParent.SelectedItem.Value
        End If
        Session("App_EditID") = strAppID
        Session.Add("AppName", txtAppName.Text)
        Response.Redirect("ColumnTranslation.aspx?AppID=" & strAppID & "&RetAppID=" & strRetAppID)
    End Sub

    Private Sub lnkAppConfiguration_Click(ByVal sender As System.Object, ByVal e As EventArgs) Handles lnkAppConfiguration.Click
        Dim strAppID As String
        Dim strRetAppID As String = txtAppID.Text
        If CInt(ddlAppParent.SelectedItem.Value) < 1 Then
            strAppID = txtAppID.Text
        Else
            strAppID = ddlAppParent.SelectedItem.Value
        End If
        Session("App_EditID") = strAppID
        Session.Add("AppName", txtAppName.Text)
        Response.Redirect("ApplicationConfiguration.aspx?AppID=" & strAppID & "&RetAppID=" & strRetAppID)
    End Sub

    Private Sub btnSuche_Click(ByVal sender As System.Object, ByVal e As EventArgs) Handles btnSuche.Click
        Search(True, True, True, True)
        If dgSearchResult.Rows.Count = 0 Then
            lblError.Text = "Keine Datensätze gefunden."
        End If
    End Sub

    Private Sub lnkFieldTranslation_Click(ByVal sender As System.Object, ByVal e As EventArgs) Handles lnkFieldTranslation.Click
        Dim strAppURL As String
        Dim strRetAppID As String = txtAppID.Text

        Dim cn As New SqlClient.SqlConnection(m_User.App.Connectionstring)
        Try
            cn.Open()
            Dim _App As New Kernel.Application(CInt(txtAppID.Text), cn)
            strAppURL = _App.AppURL.ToString
            Session("App_EditID") = strRetAppID
            Session.Add("AppName", txtAppName.Text)
            Response.Redirect("FieldTranslation.aspx?AppURL=" & strAppURL & "&RetAppID=" & strRetAppID)
        Finally
            If cn.State <> ConnectionState.Closed Then
                cn.Close()
            End If
        End Try
    End Sub

    Private Sub GridNavigation1_PagerChanged(ByVal PageIndex As Integer) Handles GridNavigation1.PagerChanged
        dgSearchResult.PageIndex = PageIndex
        FillDataGrid()
    End Sub

    Private Sub GridNavigation1_PageSizeChanged() Handles GridNavigation1.PageSizeChanged
        FillDataGrid()
    End Sub

    Private Sub dgSearchResult_RowCommand(ByVal sender As Object, ByVal e As GridViewCommandEventArgs) Handles dgSearchResult.RowCommand

        If e.CommandName = "Edit" Then
            Dim index As Integer = Convert.ToInt32(e.CommandArgument)
            Dim row As GridViewRow = dgSearchResult.Rows(index)
            EditEditMode(CInt(row.Cells(0).Text))
            dgSearchResult.SelectedIndex = row.RowIndex
        ElseIf e.CommandName = "Del" Then
            Dim index As Integer = Convert.ToInt32(e.CommandArgument)
            Dim row As GridViewRow = dgSearchResult.Rows(index)
            EditDeleteMode(CInt(row.Cells(0).Text))
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

    Protected Sub btnEmpty_Click(ByVal sender As Object, ByVal e As ImageClickEventArgs) Handles btnEmpty.Click
        btnSuche_Click(sender, e)
    End Sub

    Private Sub ddlAppParent_PreRender(ByVal sender As Object, ByVal e As EventArgs) Handles ddlAppParent.PreRender
        For Each item As ListItem In ddlAppParent.Items
            item.Attributes.Add("title", item.Text)
        Next
    End Sub

#End Region

#Region " Data and Function "

    Private Sub FillForm()
        Dim cn As New SqlClient.SqlConnection(m_User.App.Connectionstring)
        FillAppParent(cn)
        FillAppType(cn)
        FillAppTechType(cn)
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
        Dim dvApplication As DataView

        If Not Session("myAppListView") Is Nothing Then
            dvApplication = CType(Session("myAppListView"), DataView)
        Else
            Dim cn As New SqlClient.SqlConnection(m_User.App.Connectionstring)
            Try
                Dim dtApplication As ApplicationList

                cn.Open()

                dtApplication = New ApplicationList(txtFilterAppName.Text, _
                                                    cn, _
                                                    txtFilterAppFriendlyName.Text, _
                                                    ddlFilterAppTechType.SelectedValue)

                dvApplication = dtApplication.DefaultView
                Session("myAppListView") = dvApplication
            Finally
                If cn.State <> ConnectionState.Closed Then
                    cn.Close()
                End If
            End Try
        End If
        dvApplication.Sort = strSort
        dgSearchResult.DataSource = dvApplication
        dgSearchResult.DataBind()
    End Sub

    Private Sub ReFillAppParent(ByVal intAppID As Integer)
        Dim dvAppParent As DataView

        If Not Session("myAppParentView") Is Nothing Then
            dvAppParent = CType(Session("myAppParentView"), DataView)
        Else
            Dim cn As New SqlClient.SqlConnection(m_User.App.Connectionstring)
            Try
                cn.Open()
                Dim dtApplication As New ApplicationList(cn)
                dvAppParent = dtApplication.DefaultView
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
            .DataTextField = "AppURL"
            .DataBind()
        End With
    End Sub

    Private Sub InitReportSolutionToolSettings(intAppID As Integer)
        _lnkMvcReportSolution.Visible = False

        Dim dtAppConfiguration As New Kernel.AppConfigurationList(intAppID, 1, 0, m_User.App.Connectionstring)
        Dim dvAppConfiguration As DataView = dtAppConfiguration.DefaultView
        dvAppConfiguration.RowFilter = "ConfigKey = 'ReportSolutionTool'"
        If (dvAppConfiguration.Count = 0) Then
            Return
        End If

        Dim sConfigValue As String = dvAppConfiguration(0)("ConfigValue")
        If (Not sConfigValue Is Nothing And sConfigValue.ToLower() = "true") Then
            _lnkMvcReportSolution.Visible = True
        End If
    End Sub

    Private Sub FillAppParent(ByVal cn As SqlClient.SqlConnection)
        Dim dvAppParent As DataView
        Dim dtApplication As New ApplicationList(cn)
        dvAppParent = dtApplication.DefaultView
        Session("myAppParentView") = dvAppParent
        dvAppParent.Sort = "AppURL"
        With ddlAppParent
            .Items.Clear()
            .DataSource = dvAppParent
            .DataValueField = "AppId"
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

    Private Sub FillAppTechType(ByVal cn As SqlClient.SqlConnection)
        Dim dtAppTechType As New Kernel.AppTechTypeList(cn)
        dtAppTechType.DefaultView.Sort = "AppTechType"

        ddlFilterAppTechType.Items.Clear()
        ddlAppTechType.Items.Clear()
        ddlFilterAppTechType.Items.Add(New ListItem(""))
        ddlAppTechType.Items.Add(New ListItem(""))

        For Each dRow As DataRowView In dtAppTechType.DefaultView
            ddlFilterAppTechType.Items.Add(New ListItem(dRow("AppTechType").ToString()))
            ddlAppTechType.Items.Add(New ListItem(dRow("AppTechType").ToString()))
        Next
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
            ddlAppType.SelectedValue = _App.AppType
            ddlAppTechType.SelectedValue = _App.AppTechType
            txtAppURL.Text = _App.AppURL.ToString
            cbxAppInMenu.Checked = _App.AppInMenu
            cbxBatchAuthorization.Checked = _App.BatchAuthorization
            txtAppComment.Text = _App.AppComment
            txtAppDescription.Text = _App.AppDescription
            ddlAppParent.SelectedItem.Selected = False
            ddlAppParent.Items.FindByValue(_App.AppParent.ToString).Selected = True
            ddlAuthorizationlevel.SelectedItem.Selected = False
            ddlAuthorizationlevel.Items.FindByValue(_App.Authorizationlevel.ToString).Selected = True
            txtAppRank.Text = _App.AppRank.ToString
            txtSchwellwert.Text = _App.AppSchwellwert
            txtAppParam.Text = _App.AppParam
            txtMaxLevel.Text = _App.MaxLevel.ToString
            txtMaxLevelsPerGroup.Text = _App.MaxLevelsPerGroup.ToString

            InitReportSolutionToolSettings(_App.AppId)

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
        If Not ddlAppType.SelectedItem Is Nothing Then ddlAppType.SelectedItem.Selected = False
        ddlAppTechType.SelectedValue = ""
        txtAppURL.Text = ""
        cbxAppInMenu.Checked = False
        cbxBatchAuthorization.Checked = False
        txtAppComment.Text = ""
        txtAppDescription.Text = ""
        If Not ddlAppParent.SelectedItem Is Nothing Then ddlAppParent.SelectedItem.Selected = False
        txtAppRank.Text = ""
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
        txtAppID.BackColor = Drawing.Color.FromName(strBackColor)
        txtAppName.Enabled = Not blnLock
        txtAppName.BackColor = Drawing.Color.FromName(strBackColor)
        txtAppFriendlyName.Enabled = Not blnLock
        txtAppFriendlyName.BackColor = Drawing.Color.FromName(strBackColor)
        ddlAppType.Enabled = Not blnLock
        ddlAppType.BackColor = Drawing.Color.FromName(strBackColor)
        ddlAppTechType.Enabled = Not blnLock
        ddlAppTechType.BackColor = Drawing.Color.FromName(strBackColor)
        txtAppURL.Enabled = Not blnLock
        txtAppURL.BackColor = Drawing.Color.FromName(strBackColor)
        cbxAppInMenu.Enabled = Not blnLock
        cbxBatchAuthorization.Enabled = Not blnLock
        txtAppComment.Enabled = Not blnLock
        txtAppComment.BackColor = Drawing.Color.FromName(strBackColor)
        txtAppDescription.Enabled = Not blnLock
        txtAppDescription.BackColor = Drawing.Color.FromName(strBackColor)
        ddlAppParent.Enabled = Not blnLock
        ddlAppParent.BackColor = Drawing.Color.FromName(strBackColor)
        ddlAuthorizationlevel.Enabled = Not blnLock
        ddlAuthorizationlevel.BackColor = Drawing.Color.FromName(strBackColor)
        txtAppRank.Enabled = Not blnLock
        txtAppRank.BackColor = Drawing.Color.FromName(strBackColor)
    End Sub

    Private Sub CustomerAdminMode()
        SearchMode(False)

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
        lbtnCancel.Text = "Verwerfen&nbsp;&#187;"
    End Sub

    Private Sub EditDeleteMode(ByVal intGroupId As Integer)
        If Not FillEdit(intGroupId) Then
            lbtnDelete.Enabled = False
        Else
            lblMessage.Text = "Möchten Sie die Anwendung wirklich löschen?"
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
        End If
    End Sub

    Private Sub Search(Optional ByVal blnShowDataGrid As Boolean = False, Optional ByVal blnResetSelectedIndex As Boolean = False, Optional ByVal blnResetPageIndex As Boolean = False, Optional ByVal blnClearCache As Boolean = False)
        ClearEdit()
        If blnClearCache Then
            Session.Remove("myAppListView")
            Session.Remove("App_EditID")
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
        Dim strTask As String = "Admin - Anwendungsverwaltung" ' strTask
        ' strIdentification
        ' strDescription
        Dim strCustomerName As String = m_User.CustomerName ' strCustomername
        Dim blnIsTestUser As Boolean = m_User.IsTestUser ' blnIsTestUser
        Dim intSeverity As Integer = 0 ' intSeverity 

        logApp.WriteEntry(strCategory, strUserName, strSessionID, intSource, strTask, strIdentification, strDescription, strCustomerName, m_User.Customer.CustomerId, blnIsTestUser, intSeverity, tblParameters)
    End Sub

    Private Function SetOldLogParameters(ByVal intAppId As Int32) As DataTable
        Dim cn As New SqlClient.SqlConnection(m_User.App.Connectionstring)
        Try
            cn.Open()
            Dim _App As New Kernel.Application(intAppId, cn)

            Dim tblPar = CreateLogTableStructure()

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
            dt.Columns.Add("Fehler beim Erstellen der Log-Parameter", Type.GetType("System.String"))
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
            .Columns.Add("Anwendungs-Name", Type.GetType("System.String"))
            .Columns.Add("Freundlicher Name", Type.GetType("System.String"))
            .Columns.Add("Typ", Type.GetType("System.String"))
            .Columns.Add("URL", Type.GetType("System.String"))
            .Columns.Add("in Menü", Type.GetType("System.Boolean"))
            .Columns.Add("Kommentar", Type.GetType("System.String"))
            .Columns.Add("Gehört zu", Type.GetType("System.String"))
            .Columns.Add("Autorisierungslevel", Type.GetType("System.String"))
            .Columns.Add("Sammelautorisierung", Type.GetType("System.Boolean"))
            .Columns.Add("Rang", Type.GetType("System.String"))
        End With
        Return tblPar
    End Function

#End Region

End Class