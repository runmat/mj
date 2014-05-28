
Imports CKG.Base.Kernel.Admin
Imports CKG.Base.Kernel.Security
Imports CKG.Base.Kernel.Common.Common
Imports CKG.Portal.PageElements

Public Class ArchivManagement
    Inherits System.Web.UI.Page

    Protected WithEvents ucStyles As Styles

#Region " Vom Web Form Designer generierter Code "
    Protected WithEvents lblError As System.Web.UI.WebControls.Label
    Protected WithEvents dgSearchResult As System.Web.UI.WebControls.DataGrid
    Protected WithEvents lblMessage As System.Web.UI.WebControls.Label
    Protected WithEvents trSearchResult As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents trEditUser As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents txtArchivID As System.Web.UI.WebControls.TextBox
    Protected WithEvents lnkUserManagement As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lnkGroupManagement As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lbtnNew As System.Web.UI.WebControls.LinkButton
    Protected WithEvents lbtnSave As System.Web.UI.WebControls.LinkButton
    Protected WithEvents lbtnCancel As System.Web.UI.WebControls.LinkButton
    Protected WithEvents lbtnDelete As System.Web.UI.WebControls.LinkButton
    Protected WithEvents trSearch As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents trSearchSpacer As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents lnkCustomerManagement As System.Web.UI.WebControls.HyperLink
    Protected WithEvents Form1 As System.Web.UI.HtmlControls.HtmlForm
    Protected WithEvents lnkOrganizationManagement As System.Web.UI.WebControls.HyperLink
    Protected WithEvents btnSuche As System.Web.UI.WebControls.Button
    Protected WithEvents txtFilterEasyArchivName As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtEasyArchivName As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtEasyLagerortName As System.Web.UI.WebControls.TextBox
    Protected WithEvents lnkAppManagement As System.Web.UI.WebControls.HyperLink
    Protected WithEvents txtEasyQueryIndex As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtEasyQueryIndexName As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtArchivetype As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtEasyTitleName As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtDefaultQuery As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtSortOrder As System.Web.UI.WebControls.TextBox
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

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        ' Hier Benutzercode zur Seiteninitialisierung einfügen
        m_User = GetUser(Me)
        ucHeader.InitUser(m_User)
        ucStyles.TitleText = "Archivverwaltung"
        AdminAuth(Me, m_User, AdminLevel.Master)

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
        If dvArchiv.Count > dgSearchResult.PageSize Then
            dgSearchResult.PagerStyle.Visible = True
        Else
            dgSearchResult.PagerStyle.Visible = False
        End If

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
        lbtnCancel.Text = "Verwerfen"
    End Sub

    Private Sub EditDeleteMode(ByVal intGroupId As Integer)
        If Not FillEdit(intGroupId) Then
            lbtnDelete.Enabled = False
        Else
            lblMessage.Text = "Möchten Sie das Archiv wirklich löschen?"
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
            'm_context.Cache.Remove("myArchivListView")
            Session.Remove("myArchivListView")
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

    Private Sub Log(ByVal strIdentification As String, ByVal strDescription As String, ByVal tblParameters As DataTable, Optional ByVal strCategory As String = "Archiv")
        Dim logArchiv As New Base.Kernel.Logging.Trace(m_User.App.Connectionstring, m_User.App.SaveLogAccessSAP, m_User.App.LogLevel)

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
End Class

' ************************************************
' $History: ArchivManagement.aspx.vb $
' 
' *****************  Version 4  *****************
' User: Rudolpho     Date: 6.01.09    Time: 11:45
' Updated in $/CKAG/admin
' ITA 2503  Cache durch Session ersetzt
' 
' *****************  Version 3  *****************
' User: Jungj        Date: 6.10.08    Time: 9:19
' Updated in $/CKAG/admin
' ITA 2295 fertig
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
' *****************  Version 2  *****************
' User: Rudolpho     Date: 6.12.07    Time: 14:36
' Updated in $/CKG/Admin/AdminWeb
' ITA: 1440
' 
' *****************  Version 1  *****************
' User: Uha          Date: 27.08.07   Time: 17:12
' Created in $/CKG/Admin/AdminWeb
' ITA 1269: Archivverwaltung/-zuordnung jetzt auch per Web
' 
' ************************************************
