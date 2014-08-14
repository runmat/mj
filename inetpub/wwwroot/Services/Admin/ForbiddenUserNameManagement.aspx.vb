Imports CKG.Base.Kernel.Security
Imports CKG.Base.Kernel.Common.Common
Imports Admin.Kernel

Partial Public Class ForbiddenUserNameManagement
    Inherits System.Web.UI.Page

#Region " Membervariables "
    Private m_User As User
    Private m_App As App
    Protected WithEvents GridNavigation1 As Global.CKG.Services.GridNavigation
    Private m_context As HttpContext = HttpContext.Current
#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        m_User = GetUser(Me)
        lblHead.Text = "Verwaltung verbotener Benutzernamen"
        AdminAuth(Me, m_User, AdminLevel.Customer)
        GridNavigation1.setGridElment(dgSearchResult)
        Try
            m_App = New App(m_User)

            lblError.Text = ""
            lblMessage.Text = ""

            If Not IsPostBack Then
                FillForm()
            End If
        Catch ex As Exception
            m_App.WriteErrorText(1, m_User.UserName, "ForbiddenUserNameManagement", "Page_Load", ex.ToString)

            lblError.Text = ex.ToString
        End Try
    End Sub
    Private Sub btnSuche_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSuche.Click
        Search(True, True, True, True)
    End Sub

#Region " Events "

    Private Sub dgSearchResult_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles dgSearchResult.RowCreated
        If e.Row.RowType = DataControlRowType.DataRow Then


            Dim addButton As LinkButton = CType(e.Row.Cells(1).Controls(0).FindControl("btnSelect"), LinkButton)
            addButton.CommandArgument = e.Row.RowIndex.ToString()
            Dim addImgButton As ImageButton = CType(e.Row.Cells(2).Controls(0).FindControl("ibtnSRDelete"), ImageButton)
            addImgButton.CommandArgument = e.Row.RowIndex.ToString()
        End If
    End Sub
    'Private Sub btnSuche_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSuche.Click
    '    Search(True, True, True, True)
    'End Sub
    Private Sub dgSearchResult_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles dgSearchResult.Sorting
        Dim strSort As String = e.SortExpression
        If Not ViewState("ResultSort") Is Nothing AndAlso ViewState("ResultSort").ToString = strSort Then
            strSort &= " DESC"
        End If
        ViewState("ResultSort") = strSort
        FillDataGrid(strSort)
    End Sub
    Private Sub dgSearchResult_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgSearchResult.RowCommand
        Dim id As Integer
        Dim index As Integer
        Dim row As GridViewRow
        Dim lblID As Label
        index = Convert.ToInt32(e.CommandArgument)
        row = dgSearchResult.Rows(index)
        lblID = CType(row.Cells(0).FindControl("lblID"), Label)
        id = CInt(lblID.Text)
        If e.CommandName = "Select" Then
            EditEditMode(id)
        ElseIf e.CommandName = "Del" Then
            EditDeleteMode(id)
        End If
    End Sub



    Private Sub lbtnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lbtnCancel.Click

        Search(, True)
    End Sub

    Private Sub lbtnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lbtnNew.Click

        SearchMode(False)
        ClearEdit()
        'Dim cn As New SqlClient.SqlConnection(m_User.App.Connectionstring)
        'cn.Open()
    End Sub

    Private Sub lbtnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lbtnSave.Click
        Dim tblLogParameter As DataTable
        Dim cn As SqlClient.SqlConnection
        cn = New SqlClient.SqlConnection(m_User.App.Connectionstring)
        Try

            cn.Open()
            Dim intId As Integer = CInt(txtID.Text)
            Dim strLogMsg As String = "Eintrag (" & txtForbiddenUserNameName.Text & ") anlegen"
            Dim blnNew As Boolean = True
            If Not (intId = -1) Then
                blnNew = False
                strLogMsg = "Eintrag (" & txtForbiddenUserNameName.Text & ") ändern"
                tblLogParameter = SetOldLogParameters(intId)
            End If

            Dim _ForbiddenUserName As New ForbiddenUserName(intId, txtForbiddenUserNameName.Text, _
                                                m_User.UserName, blnNew)
            _ForbiddenUserName.Save(cn)
            tblLogParameter = SetNewLogParameters()
            Log(_ForbiddenUserName.Id.ToString, strLogMsg, tblLogParameter)

            Search(True, True, True, True)
            lblMessage.Text = "Die Änderungen wurden gespeichert."
        Catch ex As Exception
            m_App.WriteErrorText(1, m_User.UserName, "ForbiddenUserNameManagement", "lbtnSave_Click", ex.ToString)

            lblError.Text = ex.Message
            If Not ex.InnerException Is Nothing Then
                lblError.Text &= ": " & ex.InnerException.Message
            End If
            tblLogParameter = New DataTable
            Log(txtID.Text, lblError.Text, tblLogParameter, "ERR")
        Finally
            If cn.State <> ConnectionState.Closed Then
                cn.Close()
            End If
        End Try
    End Sub

    Private Sub lbtnDelete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lbtnDelete.Click
        Dim tblLogParameter As DataTable
        Dim cn As SqlClient.SqlConnection
        cn = New SqlClient.SqlConnection(m_User.App.Connectionstring)
        Try

            Dim strLogMsg As String = "Eintrag (" & txtForbiddenUserNameName.Text & ") löschen"
            Dim _ForbiddenUserName As New Kernel.ForbiddenUserName(CInt(txtID.Text), txtForbiddenUserNameName.Text, _
                 "", False)

            cn.Open()
            _ForbiddenUserName.Delete(cn)
            tblLogParameter = SetOldLogParameters(CInt(txtID.Text))
            Log(_ForbiddenUserName.Id.ToString, strLogMsg, tblLogParameter)

            Search(True, True, True, True)
        Catch ex As Exception
            m_App.WriteErrorText(1, m_User.UserName, "ForbiddenUserNameManagement", "lbtnDelete_Click", ex.ToString)

            lblError.Text = ex.Message
            If Not ex.InnerException Is Nothing Then
                If InStr(ex.InnerException.Message, "UNIQUE_UserName-Index") > 0 Then
                    lblError.Text &= ": Doppelte Einträge sind verboten."
                Else
                    lblError.Text &= ": " & ex.InnerException.Message
                End If
            End If
            tblLogParameter = New DataTable
            Log(txtID.Text, lblError.Text, tblLogParameter, "ERR")
        Finally
            If cn.State <> ConnectionState.Closed Then
                cn.Close()
            End If
        End Try
    End Sub
#End Region
#Region " Data and Function "
    Private Sub FillForm()
        'Dim cn As New SqlClient.SqlConnection(m_User.App.Connectionstring)
        'cn.Open()
        trEditUser.Visible = False
        Result.Visible = False
        lblForbiddenUserNameName.Visible = Not txtFilterForbiddenUserNameName.Visible
        Input.Visible = Not txtFilterForbiddenUserNameName.Visible
    End Sub

    Private Sub FillDataGrid()
        Dim strSort As String = "UserName"
        If Not ViewState("ResultSort") Is Nothing Then
            strSort = ViewState("ResultSort").ToString
        End If
        FillDataGrid(strSort)
    End Sub
    Private Sub FillDataGrid(ByVal strSort As String)
        trSearchResult.Visible = True
        Dim dvForbiddenUserName As DataView

        'If Not m_context.Cache("myForbiddenUserNameListView") Is Nothing Then
        '    dvForbiddenUserName = CType(m_context.Cache("myForbiddenUserNameListView"), DataView)
        If Not Session("myForbiddenUserNameListView") Is Nothing Then
            dvForbiddenUserName = CType(Session("myForbiddenUserNameListView"), DataView)

        Else
            Dim dtForbiddenUserName As ForbiddenUserNameList
            Dim cn As New SqlClient.SqlConnection(m_User.App.Connectionstring)
            cn.Open()

            dtForbiddenUserName = New ForbiddenUserNameList(txtFilterForbiddenUserNameName.Text, cn)
            dvForbiddenUserName = dtForbiddenUserName.DefaultView
            'm_context.Cache.Insert("myForbiddenUserNameListView", dvForbiddenUserName, Nothing, DateTime.Now.AddMinutes(20), TimeSpan.Zero)
            Session("myForbiddenUserNameListView") = dvForbiddenUserName
        End If
        dvForbiddenUserName.Sort = strSort
        With dgSearchResult
            .DataSource = dvForbiddenUserName
            .DataBind()
        End With
    End Sub

    Private Function FillEdit(ByVal intId As Integer) As Boolean
        SearchMode(False)
        Dim cn As New SqlClient.SqlConnection(m_User.App.Connectionstring)
        Try
            cn.Open()
            Dim _ForbiddenUserName As New Kernel.ForbiddenUserName(intId, cn)
            txtID.Text = _ForbiddenUserName.Id.ToString
            txtForbiddenUserNameName.Text = _ForbiddenUserName.UserName
            Return True
        Finally
            If cn.State <> ConnectionState.Closed Then
                cn.Close()
            End If
        End Try
    End Function

    Private Sub ClearEdit()
        txtID.Text = "-1"
        txtForbiddenUserNameName.Text = ""
        lbtnSave.Visible = True
        lbtnDelete.Visible = False
        LockEdit(False)
    End Sub

    Private Sub LockEdit(ByVal blnLock As Boolean)
        Dim strBackColor As String = "White"
        If blnLock Then
            strBackColor = "LightGray"
        End If
        txtID.Enabled = Not blnLock
        txtID.BackColor = System.Drawing.Color.FromName(strBackColor)
        txtForbiddenUserNameName.Enabled = Not blnLock
        txtForbiddenUserNameName.BackColor = System.Drawing.Color.FromName(strBackColor)
    End Sub


    Private Sub EditEditMode(ByVal intId As Integer)
        If Not FillEdit(intId) Then
            LockEdit(True)
            lbtnSave.Enabled = False
        Else
            lbtnSave.Enabled = True
        End If
        lbtnCancel.Text = "&nbsp;&#187;Verwerfen"
        DivSearch1.Visible = False
        txtFilterForbiddenUserNameName.Visible = False
        lblForbiddenUserNameName.Visible = Not DivSearch1.Visible
        Input.Visible = Not DivSearch1.Visible
    End Sub

    Private Sub EditDeleteMode(ByVal intId As Integer)
        If Not FillEdit(intId) Then
            lbtnDelete.Enabled = False
        Else
            lblMessage.Text = "Möchten Sie den Eintrag wirklich löschen?"
            lbtnDelete.Enabled = True
        End If
        LockEdit(True)
        Input.Visible = True
        lblForbiddenUserNameName.Visible = Input.Visible
        lbtnDelete.Visible = True
        lbtnCancel.Text = "&nbsp;&#187;Abbrechen"
        lbtnSave.Visible = False
        DivSearch1.Visible = False
        txtFilterForbiddenUserNameName.Visible = False


    End Sub

    Private Sub SearchMode(Optional ByVal blnSearchMode As Boolean = True)
        trEditUser.Visible = Not blnSearchMode
        lbtnSave.Visible = Not blnSearchMode
        lbtnCancel.Visible = Not blnSearchMode
        lblForbiddenUserNameName.Visible = Not blnSearchMode
        Input.Visible = Not blnSearchMode
        DivSearch1.Visible = blnSearchMode
        tableSearch.Visible = blnSearchMode
        Result.Visible = blnSearchMode
        lbtnNew.Visible = blnSearchMode
        txtFilterForbiddenUserNameName.Visible = blnSearchMode


    End Sub

    Private Sub Search(Optional ByVal blnRefillDataGrid As Boolean = False, Optional ByVal blnResetSelectedIndex As Boolean = False, Optional ByVal blnResetPageIndex As Boolean = False, Optional ByVal blnClearCache As Boolean = False)
        ClearEdit()
        If blnClearCache Then
            'm_context.Cache.Remove("myForbiddenUserNameListView")
            Session.Remove("myForbiddenUserNameListView")
        End If
        If blnResetSelectedIndex Then dgSearchResult.SelectedIndex = -1
        If blnResetPageIndex Then dgSearchResult.PageIndex = 0
        SearchMode()
        If blnRefillDataGrid Then FillDataGrid()
    End Sub

    Private Sub Log(ByVal strIdentification As String, ByVal strDescription As String, ByVal tblParameters As DataTable, Optional ByVal strCategory As String = "APP")
        Dim logApp As New CKG.Base.Kernel.Logging.Trace(m_User.App.Connectionstring, m_User.App.SaveLogAccessSAP, m_User.App.LogLevel)

        ' strCategory
        Dim strUserName As String = m_User.UserName ' strUserName
        Dim strSessionID As String = Session.SessionID ' strSessionID
        Dim intSource As Integer = CInt(Request.QueryString("AppID")) ' intSource 
        Dim strTask As String = "Admin - Verwaltung verbotener Benutzernamen" ' strTask
        ' strIdentification
        ' strDescription
        Dim strCustomerName As String = m_User.CustomerName ' strCustomername
        Dim blnIsTestUser As Boolean = m_User.IsTestUser ' blnIsTestUser
        Dim intSeverity As Integer = 0 ' intSeverity 

        logApp.WriteEntry(strCategory, strUserName, strSessionID, intSource, strTask, strIdentification, strDescription, strCustomerName, m_User.Customer.CustomerId, blnIsTestUser, intSeverity, tblParameters)
    End Sub

    Private Function SetOldLogParameters(ByVal intId As Int32) As DataTable
        Dim cn As SqlClient.SqlConnection
        cn = New SqlClient.SqlConnection(m_User.App.Connectionstring)
        Try

            cn.Open()
            Dim _ForbiddenUserName As New Kernel.ForbiddenUserName(intId, cn)

            Dim tblPar = CreateLogTableStructure()

            With tblPar
                .Rows.Add(.NewRow)
                .Rows(.Rows.Count - 1)("Status") = "Alt"
                .Rows(.Rows.Count - 1)("Benutzername") = _ForbiddenUserName.UserName
            End With
            Return tblPar
        Catch ex As Exception
            m_App.WriteErrorText(1, m_User.UserName, "ForbiddenUserNameManagement", "SetOldLogParameters", ex.ToString)

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

    Private Function SetNewLogParameters() As DataTable
        Try
            Dim tblPar = CreateLogTableStructure()

            With tblPar
                .Rows.Add(.NewRow)
                .Rows(.Rows.Count - 1)("Status") = "Neu"
                .Rows(.Rows.Count - 1)("Benutzername") = txtForbiddenUserNameName.Text
            End With
            Return tblPar
        Catch ex As Exception
            m_App.WriteErrorText(1, m_User.UserName, "ForbiddenUserNameManagement", "SetNewLogParameters", ex.ToString)

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
        End With
        Return tblPar
    End Function

    Private Sub GridNavigation1_PagerChanged(ByVal PageIndex As Integer) Handles GridNavigation1.PagerChanged
        dgSearchResult.PageIndex = PageIndex
        FillDataGrid(False)
    End Sub

    Private Sub GridNavigation1_PageSizeChanged() Handles GridNavigation1.PageSizeChanged
        FillDataGrid(False)
    End Sub
#End Region




End Class