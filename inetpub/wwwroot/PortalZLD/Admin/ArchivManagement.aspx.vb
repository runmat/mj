
Imports CKG.Base.Kernel.Admin
Imports CKG.Base.Kernel.Security
Imports CKG.Base.Kernel.Common.Common

Partial Public Class ArchivManagement
    Inherits System.Web.UI.Page

#Region " Membervariables "
    Private m_User As User
    Private m_App As App
    Private m_context As HttpContext = HttpContext.Current
    Protected WithEvents GridNavigation1 As Global.CKG.PortalZLD.GridNavigation
#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        m_User = GetUser(Me)
        lblHead.Text = "Archivverwaltung"
        AdminAuth(Me, m_User, AdminLevel.Master)
        GridNavigation1.setGridElment(dgSearchResult)
        'wenn SuperUser und übergeordnete Firma
        If m_User.Customer.AccountingArea = -1 Then
            lnkAppManagement.Visible = True
        End If


        Try
            m_App = New App(m_User)

            If Not IsPostBack Then
                lblError.Text = ""

                FillForm()
                Dim strArchivID As String = CStr(Request.QueryString("ArchivID"))
                If Not strArchivID = String.Empty Then
                    EditEditMode(CInt(strArchivID))
                End If
            End If
        Catch ex As Exception
            lblError.Text = ex.ToString
            lblError.Visible = True
            m_App.WriteErrorText(1, m_User.UserName, "ArchivManagement", "PageLoad", lblError.Text)
        End Try
    End Sub
#Region " Data and Function "
    Private Sub FillForm()
        trEditUser.Visible = False
        trSearchResult.Visible = False
        Dim cn As New SqlClient.SqlConnection(m_User.App.Connectionstring)
        If Not m_User.HighestAdminLevel = AdminLevel.Master Then
            CustomerAdminMode()
        End If
    End Sub

    Private Sub FillDataGrid()
        Dim strSort As String = "ArchivID"
        If Not ViewState("ResultSort") Is Nothing Then
            strSort = ViewState("ResultSort").ToString
        End If
        FillDataGrid(strSort)
    End Sub

    Private Sub FillDataGrid(ByVal strSort As String)
        trSearchResult.Visible = True
        Dim dvArchiv As DataView

        'If Not m_context.Cache("myArchivListView") Is Nothing Then
        '    dvArchiv = CType(m_context.Cache("myArchivListView"), DataView)
        If Not Session("myArchivListView") Is Nothing Then
            dvArchiv = CType(Session("myArchivListView"), DataView)
        Else

            Dim dtArchiv As ArchivList
            Dim cn As New SqlClient.SqlConnection(m_User.App.Connectionstring)
            Try
                cn.Open()

                dtArchiv = New ArchivList(txtFilterEasyArchivName.Text, _
                                                          cn)
                dvArchiv = dtArchiv.DefaultView
                'm_context.Cache.Insert("myArchivListView", dvArchiv, Nothing, DateTime.Now.AddMinutes(20), TimeSpan.Zero)
                Session("myArchivListView") = dvArchiv
            Finally
                If cn.State <> ConnectionState.Closed Then
                    cn.Close()
                End If
            End Try
        End If
        dvArchiv.Sort = strSort
        dgSearchResult.DataSource = dvArchiv
        dgSearchResult.DataBind()
    End Sub

    Private Function FillEdit(ByVal intArchivId As Integer) As Boolean
        SearchMode(False)
        Dim cn As New SqlClient.SqlConnection(m_User.App.Connectionstring)
        Try
            cn.Open()
            Dim _Archiv As New Kernel.Archiv(intArchivId, cn)
            txtArchivID.Text = _Archiv.ArchivId.ToString
            txtArchivetype.Text = _Archiv.Archivetype
            txtDefaultQuery.Text = _Archiv.DefaultQuery
            txtEasyArchivName.Text = _Archiv.EasyArchivName
            txtEasyLagerortName.Text = _Archiv.EasyLagerortName
            txtEasyQueryIndex.Text = CType(_Archiv.EasyQueryIndex, String)
            txtEasyQueryIndexName.Text = _Archiv.EasyQueryIndexName
            txtEasyTitleName.Text = _Archiv.EasyTitleName

            txtSortOrder.Text = CType(_Archiv.SortOrder, String)
            Return True
        Finally
            If cn.State <> ConnectionState.Closed Then
                cn.Close()
            End If
        End Try
    End Function

    Private Sub ClearEdit()
        txtArchivID.Text = "-1"
        txtArchivetype.Text = ""
        txtDefaultQuery.Text = ""
        txtEasyArchivName.Text = ""
        txtEasyLagerortName.Text = ""
        txtEasyQueryIndex.Text = "0"
        txtEasyQueryIndexName.Text = ""
        txtEasyTitleName.Text = ""
        txtSortOrder.Text = "1"
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
        txtArchivID.Enabled = Not blnLock
        txtArchivID.BackColor = System.Drawing.Color.FromName(strBackColor)
        txtArchivetype.Enabled = Not blnLock
        txtArchivetype.BackColor = System.Drawing.Color.FromName(strBackColor)
        txtDefaultQuery.Enabled = Not blnLock
        txtDefaultQuery.BackColor = System.Drawing.Color.FromName(strBackColor)
        txtEasyArchivName.Enabled = Not blnLock
        txtEasyArchivName.BackColor = System.Drawing.Color.FromName(strBackColor)
        txtEasyLagerortName.Enabled = Not blnLock
        txtEasyLagerortName.BackColor = System.Drawing.Color.FromName(strBackColor)
        txtEasyQueryIndex.Enabled = Not blnLock
        txtEasyQueryIndex.BackColor = System.Drawing.Color.FromName(strBackColor)
        txtEasyQueryIndexName.Enabled = Not blnLock
        txtEasyQueryIndexName.BackColor = System.Drawing.Color.FromName(strBackColor)
        txtEasyTitleName.Enabled = Not blnLock
        txtEasyTitleName.BackColor = System.Drawing.Color.FromName(strBackColor)
        txtSortOrder.Enabled = Not blnLock
        txtSortOrder.BackColor = System.Drawing.Color.FromName(strBackColor)
    End Sub

    Private Sub CustomerAdminMode()
        SearchMode(False)
        'trArchiv.Visible = False

        If m_User.Groups.Count > 0 Then
            If m_User.IsCustomerAdmin Then
                LockEdit(False)
                EditEditMode(m_User.Customer.CustomerId)
            End If
        End If
    End Sub

    Private Sub EditEditMode(ByVal intGroupId As Integer)
        If Not FillEdit(intGroupId) Then
            LockEdit(True)
            lbtnSave.Enabled = False
        Else
            lbtnSave.Enabled = True
        End If
        lbtnCancel.Text = "Verwerfen&nbsp;&#187; "
    End Sub

    Private Sub EditDeleteMode(ByVal intGroupId As Integer)
        If Not FillEdit(intGroupId) Then
            lbtnDelete.Enabled = False
        Else
            lblMessage.Text = "Möchten Sie das Archiv wirklich löschen?"
            lbtnDelete.Enabled = True
        End If
        LockEdit(True)
        lbtnCancel.Text = "Abbrechen&nbsp;&#187; "
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
        DivSearch1.Visible = blnSearchMode
        QueryFooter.Visible = blnSearchMode
        Result.Visible = blnSearchMode
        Input.Visible = Not blnSearchMode
    End Sub

    Private Sub Search(Optional ByVal blnRefillDataGrid As Boolean = False, Optional ByVal blnResetSelectedIndex As Boolean = False, Optional ByVal blnResetPageIndex As Boolean = False, Optional ByVal blnClearCache As Boolean = False)
        ClearEdit()
        If blnClearCache Then
            'm_context.Cache.Remove("myArchivListView")
            Session.Remove("myArchivListView")
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

    Private Sub Log(ByVal strIdentification As String, ByVal strDescription As String, ByVal tblParameters As DataTable, Optional ByVal strCategory As String = "Archiv")
        Dim logArchiv As New CKG.Base.Kernel.Logging.Trace(m_User.App.Connectionstring, m_User.App.SaveLogAccessSAP, m_User.App.LogLevel)

        Dim strUserName As String = m_User.UserName ' strUserName
        Dim strSessionID As String = Session.SessionID ' strSessionID
        Dim intSource As Integer = CInt(Request.QueryString("ArchivID")) ' intSource 
        Dim strTask As String = "Admin - Archivverwaltung" ' strTask
        Dim strCustomerName As String = m_User.CustomerName ' strCustomername
        Dim blnIsTestUser As Boolean = m_User.IsTestUser ' blnIsTestUser
        Dim intSeverity As Integer = 0 ' intSeverity 

        logArchiv.WriteEntry(strCategory, strUserName, strSessionID, intSource, strTask, strIdentification, strDescription, strCustomerName, m_User.Customer.CustomerId, blnIsTestUser, intSeverity, tblParameters)
    End Sub

    Private Function SetOldLogParameters(ByVal intArchivId As Int32, ByVal tblPar As DataTable) As DataTable
        Dim cn As New SqlClient.SqlConnection(m_User.App.Connectionstring)
        Try

            cn.Open()
            Dim _Archiv As New Kernel.Archiv(intArchivId, cn)

            If tblPar Is Nothing Then
                tblPar = CreateLogTableStructure()
            End If
            With tblPar
                .Rows.Add(.NewRow)
                .Rows(.Rows.Count - 1)("Status") = "Alt"
                .Rows(.Rows.Count - 1)("Archiv-Name") = _Archiv.EasyArchivName
                .Rows(.Rows.Count - 1)("Lagerort-Name") = _Archiv.EasyLagerortName
                .Rows(.Rows.Count - 1)("QueryIndex") = _Archiv.EasyQueryIndex.ToString
                .Rows(.Rows.Count - 1)("QueryIndex-Name") = _Archiv.EasyQueryIndexName
                .Rows(.Rows.Count - 1)("Titel") = _Archiv.EasyTitleName
                .Rows(.Rows.Count - 1)("DefaultQuery") = _Archiv.DefaultQuery
                .Rows(.Rows.Count - 1)("Archivetype") = _Archiv.Archivetype
                .Rows(.Rows.Count - 1)("SortOrder") = _Archiv.SortOrder.ToString
            End With
            Return tblPar
        Catch ex As Exception
            m_App.WriteErrorText(1, m_User.UserName, "ArchivManagement", "SetOldLogParameters", ex.ToString)
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
                .Rows(.Rows.Count - 1)("Archiv-Name") = txtEasyArchivName.Text
                .Rows(.Rows.Count - 1)("Freundlicher Name") = txtEasyLagerortName.Text
                .Rows(.Rows.Count - 1)("QueryIndex") = txtEasyQueryIndex.Text
                .Rows(.Rows.Count - 1)("QueryIndex-Name") = txtEasyQueryIndexName.Text
                .Rows(.Rows.Count - 1)("Titel") = txtEasyTitleName.Text
                .Rows(.Rows.Count - 1)("DefaultQuery") = txtDefaultQuery.Text
                .Rows(.Rows.Count - 1)("Archivetype") = txtArchivetype.Text
                .Rows(.Rows.Count - 1)("SortOrder") = txtSortOrder.Text
            End With
            Return tblPar
        Catch ex As Exception
            m_App.WriteErrorText(1, m_User.UserName, "ArchivManagement", "SetNewLogParameters", ex.ToString)
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
            .Columns.Add("Archiv-Name", System.Type.GetType("System.String"))
            .Columns.Add("Lagerort-Name", System.Type.GetType("System.String"))
            .Columns.Add("QueryIndex", System.Type.GetType("System.String"))
            .Columns.Add("QueryIndex-Name", System.Type.GetType("System.String"))
            .Columns.Add("Titel", System.Type.GetType("System.String"))
            .Columns.Add("Gehört zu", System.Type.GetType("System.String"))
            .Columns.Add("DefaultQuery", System.Type.GetType("System.String"))
            .Columns.Add("Archivetype", System.Type.GetType("System.String"))
            .Columns.Add("SortOrder", System.Type.GetType("System.String"))
        End With
        Return tblPar
    End Function
#End Region

#Region " Events "
    'Private Sub btnSuche_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSuche.Click
    '    Search(True, True, True, True)
    'End Sub


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
        Dim tblLogParameter As DataTable
        Dim cn As New SqlClient.SqlConnection(m_User.App.Connectionstring)
        Try

            Dim conn As New SqlClient.SqlConnection(m_User.App.Connectionstring)

            cn.Open()
            Dim intArchivId As Integer = CInt(txtArchivID.Text)
            Dim strLogMsg As String = "Archiv anlegen"
            If Not (intArchivId = -1) Then
                strLogMsg = "Archiv ändern"
                tblLogParameter = New DataTable
                tblLogParameter = SetOldLogParameters(intArchivId, tblLogParameter)
            End If
            Dim _Archiv As New Kernel.Archiv(intArchivId, _
                                                 txtEasyLagerortName.Text, _
                                                 txtEasyArchivName.Text, _
                                                 CInt(txtEasyQueryIndex.Text), _
                                                 txtEasyQueryIndexName.Text, _
                                                 txtEasyTitleName.Text, _
                                                 txtDefaultQuery.Text, _
                                                 txtArchivetype.Text, _
                                                 CInt(txtSortOrder.Text))
            Dim typ As Boolean
            _Archiv.Save(cn, typ)
            tblLogParameter = New DataTable
            tblLogParameter = SetNewLogParameters(tblLogParameter)
            Log(_Archiv.ArchivId.ToString, strLogMsg, tblLogParameter)
            Search(True, True, , True)


            lblMessage.Text = "Die Änderungen wurden gespeichert."
        Catch ex As Exception
            m_App.WriteErrorText(1, m_User.UserName, "ArchivManagement", "lbtnSave_Click", ex.ToString)
            lblError.Text = ex.Message
            If Not ex.InnerException Is Nothing Then
                lblError.Text &= ": " & ex.InnerException.Message
            End If
            tblLogParameter = New DataTable
            Log(txtArchivID.Text, lblError.Text, tblLogParameter, "ERR")
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
            Dim _Archiv As New Kernel.Archiv(CInt(txtArchivID.Text))

            cn.Open()
            tblLogParameter = New DataTable
            tblLogParameter = SetOldLogParameters(_Archiv.ArchivId, tblLogParameter)
            _Archiv.Delete(cn)
            Log(_Archiv.ArchivId.ToString, "Archiv löschen", tblLogParameter)
            Search(True, True, True, True)
            lblMessage.Text = "Das Archiv wurde gelöscht."
        Catch ex As Exception
            m_App.WriteErrorText(1, m_User.UserName, "ArchivManagement", "lbtnDelete_Click", ex.ToString)
            lblError.Text = ex.Message
            If Not ex.InnerException Is Nothing Then
                lblError.Text &= ": " & ex.InnerException.Message
            End If
            tblLogParameter = New DataTable
            Log(txtArchivID.Text, lblError.Text, tblLogParameter, "ERR")
        Finally
            If cn.State <> ConnectionState.Closed Then
                cn.Close()
            End If
        End Try
    End Sub

    Private Sub btnSuche_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSuche.Click
        Search(True, True, True, True)
    End Sub
#End Region

    Private Sub GridNavigation1_PagerChanged(ByVal PageIndex As Integer) Handles GridNavigation1.PagerChanged
        dgSearchResult.PageIndex = PageIndex
        FillDataGrid()
    End Sub

    Private Sub GridNavigation1_PageSizeChanged() Handles GridNavigation1.PageSizeChanged
        FillDataGrid()
    End Sub

    Private Sub dgSearchResult_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgSearchResult.RowCommand
        Dim index As Integer
        Dim row As GridViewRow
        Dim CtrlLabel As Label


        If e.CommandName = "Edit" Then
            index = Convert.ToInt32(e.CommandArgument)
            row = dgSearchResult.Rows(index)
            CtrlLabel = row.Cells(0).FindControl("lblArchivID")
            EditEditMode(CInt(CtrlLabel.Text))
            dgSearchResult.SelectedIndex = row.RowIndex
        ElseIf e.CommandName = "Del" Then
            index = Convert.ToInt32(e.CommandArgument)
            row = dgSearchResult.Rows(index)
            CtrlLabel = row.Cells(0).FindControl("lblArchivID")
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

    Protected Sub btnEmpty_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnEmpty.Click
        Search(True, True, True, True)
    End Sub
End Class