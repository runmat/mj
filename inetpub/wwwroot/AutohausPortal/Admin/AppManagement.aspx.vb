Imports CKG.Base.Kernel.Admin
Imports CKG.Base.Kernel.Security
Imports CKG.Base.Kernel.Common.Common

Partial Public Class AppManagement
    Inherits System.Web.UI.Page

#Region " Membervariables "

    Private m_User As User
    Private m_App As App
    Protected WithEvents GridNavigation1 As Global.Admin.GridNavigation

#End Region

#Region " Data and Function "

    Private Sub FillForm()
        trEditUser.Visible = False
        trSearchResult.Visible = False
        Dim cn As New SqlClient.SqlConnection(m_User.App.Connectionstring)
        FillAppParent(cn)
        FillAppType(cn)
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
            ddlAppParent.SelectedItem.Selected = False
            ddlAppParent.Items.FindByValue(_App.AppParent.ToString).Selected = True
            ddlAuthorizationlevel.SelectedItem.Selected = False
            ddlAuthorizationlevel.Items.FindByValue(_App.Authorizationlevel.ToString).Selected = True
            txtAppRank.Text = _App.AppRank.ToString
            txtSchwellwert.Text = _App.AppSchwellwert
            txtAppParam.Text = CType(_App.AppParam, String)
            cbxIsList.Checked = _App.AppIsList
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
        txtAppURL.Text = ""
        cbxAppInMenu.Checked = False
        cbxBatchAuthorization.Checked = False
        txtAppComment.Text = ""
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

    Private Sub SearchMode(Optional ByVal blnSearchMode As Boolean = True)
        trEditUser.Visible = Not blnSearchMode
        tableSearch.Visible = blnSearchMode
        trSearchSpacer.Visible = blnSearchMode
        trSearchResult.Visible = blnSearchMode
        lbtnSave.Visible = Not blnSearchMode
        lbtnCancel.Visible = Not blnSearchMode
        lbtnNew.Visible = blnSearchMode
        DivSearch.Visible = blnSearchMode
        QueryFooter.Visible = blnSearchMode
        Result.Visible = blnSearchMode
        Input.Visible = Not blnSearchMode
    End Sub

    Private Sub Search(Optional ByVal blnRefillDataGrid As Boolean = False, Optional ByVal blnResetSelectedIndex As Boolean = False, Optional ByVal blnResetPageIndex As Boolean = False, Optional ByVal blnClearCache As Boolean = False)
        ClearEdit()
        If blnClearCache Then
            'm_context.Cache.Remove("myAppListView")
            Session.Remove("myAppListView")
            Session.Remove("App_EditID")
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

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

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

    Private Sub lbtnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lbtnCancel.Click
        If dgSearchResult.Rows.Count = 0 Then
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
        Dim cn As New SqlClient.SqlConnection(m_User.App.Connectionstring)
        Try

            Dim conn As New SqlClient.SqlConnection(m_User.App.Connectionstring)

            cn.Open()
            Dim intAppId As Integer = CInt(txtAppID.Text)

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
                                                cbxIsList.Checked)
            Dim typ As Boolean
            _App.Save(cn, typ)

            'Save Parameters
            _App.SaveParams(conn, txtAppParam.Text, typ)

            'Save Zuordnungen, falls es sich um ein Child handelt
            _App.ReAssign(conn, _App.AppId, CInt(ddlAppParent.SelectedItem.Value.ToString))

            FillAppParent(cn)
            Search(True, True, , True)


            lblInputMessage.Text = "Die Änderungen wurden gespeichert."
        Catch ex As Exception
            m_App.WriteErrorText(1, m_User.UserName, "AppManagement", "lbtnSave_Click", ex.ToString)
            lblInputError.Text = ex.Message
            If Not ex.InnerException Is Nothing Then
                lblInputError.Text &= ": " & ex.InnerException.Message
            End If
        Finally
            If cn.State <> ConnectionState.Closed Then
                cn.Close()
            End If
        End Try
    End Sub

    Private Sub lbtnDelete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lbtnDelete.Click
        Dim cn As New SqlClient.SqlConnection(m_User.App.Connectionstring)
        Try
            Dim _app As New Kernel.Application(CInt(txtAppID.Text))

            cn.Open()

            If Not _app.HasChildren(cn) Then
                _app.Delete(cn)
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
        Session("App_EditID") = strAppID
        Session.Add("AppName", txtAppName.Text)
        Response.Redirect("ColumnTranslation.aspx?AppID=" & strAppID & "&RetAppID=" & strRetAppID)
    End Sub

    Private Sub btnSuche_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSuche.Click
        Search(True, True, True, True)
    End Sub

    Private Sub lnkFieldTranslation_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lnkFieldTranslation.Click
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

    Private Sub dgSearchResult_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgSearchResult.RowCommand

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

    Protected Sub btnEmpty_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnEmpty.Click
        btnSuche_Click(sender, e)
    End Sub

    Private Sub ddlAppParent_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlAppParent.PreRender
        For Each item As ListItem In ddlAppParent.Items
            item.Attributes.Add("title", item.Text)
        Next
    End Sub

End Class