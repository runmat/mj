
Imports CKG.Base.Kernel.Security
Imports CKG.Base.Kernel.Common.Common
Imports CKG.Portal.PageElements

Public Class ForbiddenUserNameManagement
    Inherits System.Web.UI.Page

    Protected WithEvents ucStyles As Styles

#Region " Vom Web Form Designer generierter Code "
    Protected WithEvents lblError As System.Web.UI.WebControls.Label
    Protected WithEvents dgSearchResult As System.Web.UI.WebControls.DataGrid
    Protected WithEvents lblMessage As System.Web.UI.WebControls.Label
    Protected WithEvents trSearch As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents trSearchResult As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents trEditUser As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents trSearchSpacer As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents trApp As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents lbtnNew As System.Web.UI.WebControls.LinkButton
    Protected WithEvents lbtnSave As System.Web.UI.WebControls.LinkButton
    Protected WithEvents lbtnCancel As System.Web.UI.WebControls.LinkButton
    Protected WithEvents lbtnDelete As System.Web.UI.WebControls.LinkButton
    Protected WithEvents trCustomer As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents Form1 As System.Web.UI.HtmlControls.HtmlForm
    Protected WithEvents txtFilterForbiddenUserNameName As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtForbiddenUserNameName As System.Web.UI.WebControls.TextBox
    Protected WithEvents trForbiddenUserNameName As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents btnSuche As System.Web.UI.WebControls.LinkButton
    Protected WithEvents lblForbiddenUserNameName As System.Web.UI.WebControls.Label
    Protected WithEvents txtID As System.Web.UI.WebControls.TextBox
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
        ucStyles.TitleText = "Verwaltung verbotener Benutzernamen"
        AdminAuth(Me, m_User, AdminLevel.Customer)

        Try
            m_App = New App(m_User)

            If Not IsPostBack Then
                lblError.Text = ""

                FillForm()
            End If
        Catch ex As Exception
            m_App.WriteErrorText(1, m_User.UserName, "ForbiddenUserNameManagement", "Page_Load", ex.ToString)

            lblError.Text = ex.ToString
            lblError.Visible = True
        End Try
    End Sub

#Region " Data and Function "
    Private Sub FillForm()
        'Dim cn As New SqlClient.SqlConnection(m_User.App.Connectionstring)
        'cn.Open()
        trEditUser.Visible = False
        trSearchResult.Visible = False
        lblForbiddenUserNameName.Visible = Not txtFilterForbiddenUserNameName.Visible
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
            Dim dtForbiddenUserName As Kernel.ForbiddenUserNameList
            Dim cn As New SqlClient.SqlConnection(m_User.App.Connectionstring)
            cn.Open()

            dtForbiddenUserName = New Kernel.ForbiddenUserNameList(txtFilterForbiddenUserNameName.Text, cn)
            dvForbiddenUserName = dtForbiddenUserName.DefaultView
            'm_context.Cache.Insert("myForbiddenUserNameListView", dvForbiddenUserName, Nothing, DateTime.Now.AddMinutes(20), TimeSpan.Zero)
            Session("myForbiddenUserNameListView") = dvForbiddenUserName
        End If
        dvForbiddenUserName.Sort = strSort
        If dvForbiddenUserName.Count > dgSearchResult.PageSize Then
            dgSearchResult.PagerStyle.Visible = True
        Else
            dgSearchResult.PagerStyle.Visible = False
        End If

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

    'Private Sub ForbiddenUserNameAdminMode()
    '    SearchMode(False)
    '    trApp.Visible = False

    '    If Not m_User.ForbiddenUserName Is Nothing Then
    '        If m_User.ForbiddenUserName.ForbiddenUserNameAdmin Then
    '            EditEditMode(m_User.ForbiddenUserName.Id)
    '        End If
    '    End If
    'End Sub

    Private Sub EditEditMode(ByVal intId As Integer)
        If Not FillEdit(intId) Then
            LockEdit(True)
            lbtnSave.Enabled = False
        Else
            lbtnSave.Enabled = True
        End If
        lbtnCancel.Text = "&#149;&nbsp;Verwerfen"
        txtFilterForbiddenUserNameName.Visible = False
        lblForbiddenUserNameName.Visible = Not txtFilterForbiddenUserNameName.Visible
    End Sub

    Private Sub EditDeleteMode(ByVal intId As Integer)
        If Not FillEdit(intId) Then
            lbtnDelete.Enabled = False
        Else
            lblMessage.Text = "Möchten Sie den Eintrag wirklich löschen?"
            lbtnDelete.Enabled = True
        End If
        LockEdit(True)
        lbtnCancel.Text = "&#149;&nbsp;Abbrechen"
        lbtnSave.Visible = False
        lbtnDelete.Visible = True
        txtFilterForbiddenUserNameName.Visible = False
        lblForbiddenUserNameName.Visible = Not txtFilterForbiddenUserNameName.Visible
    End Sub

    Private Sub SearchMode(Optional ByVal blnSearchMode As Boolean = True)
        trEditUser.Visible = Not blnSearchMode
        trSearch.Visible = blnSearchMode
        trSearchSpacer.Visible = blnSearchMode
        trSearchResult.Visible = blnSearchMode
        lbtnSave.Visible = Not blnSearchMode
        lbtnCancel.Visible = Not blnSearchMode
        lbtnNew.Visible = blnSearchMode
        txtFilterForbiddenUserNameName.Visible = True
        lblForbiddenUserNameName.Visible = Not txtFilterForbiddenUserNameName.Visible
    End Sub

    Private Sub Search(Optional ByVal blnRefillDataGrid As Boolean = False, Optional ByVal blnResetSelectedIndex As Boolean = False, Optional ByVal blnResetPageIndex As Boolean = False, Optional ByVal blnClearCache As Boolean = False)
        ClearEdit()
        If blnClearCache Then
            'm_context.Cache.Remove("myForbiddenUserNameListView")
            Session.Remove("myForbiddenUserNameListView")
        End If
        If blnResetSelectedIndex Then dgSearchResult.SelectedIndex = -1
        If blnResetPageIndex Then dgSearchResult.CurrentPageIndex = 0
        SearchMode()
        If blnRefillDataGrid Then FillDataGrid()
    End Sub

    Private Sub Log(ByVal strIdentification As String, ByVal strDescription As String, ByVal tblParameters As DataTable, Optional ByVal strCategory As String = "APP")
        Dim logApp As New Base.Kernel.Logging.Trace(m_User.App.Connectionstring, m_User.App.SaveLogAccessSAP, m_User.App.LogLevel)

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

    Private Function SetOldLogParameters(ByVal intId As Int32, ByVal tblPar As DataTable) As DataTable
        Dim cn As SqlClient.SqlConnection
        cn = New SqlClient.SqlConnection(m_User.App.Connectionstring)
        Try

            cn.Open()
            Dim _ForbiddenUserName As New Kernel.ForbiddenUserName(intId, cn)

            If tblPar Is Nothing Then
                tblPar = CreateLogTableStructure()
            End If
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

    Private Function SetNewLogParameters(ByVal tblPar As DataTable) As DataTable
        Try
            If tblPar Is Nothing Then
                tblPar = CreateLogTableStructure()
            End If
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
                tblLogParameter = New DataTable
                tblLogParameter = SetOldLogParameters(intId, tblLogParameter)
            End If

            Dim _ForbiddenUserName As New Kernel.ForbiddenUserName(intId, txtForbiddenUserNameName.Text, _
                                                m_User.UserName, blnNew)
            _ForbiddenUserName.Save(cn)
            tblLogParameter = New DataTable
            tblLogParameter = SetNewLogParameters(tblLogParameter)
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
            tblLogParameter = New DataTable
            tblLogParameter = SetOldLogParameters(CInt(txtID.Text), tblLogParameter)
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

    Private Sub btnSuche_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSuche.Click
        Search(True, True, True, True)
    End Sub
End Class

' ************************************************
' $History: ForbiddenUserNameManagement.aspx.vb $
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
' *****************  Version 5  *****************
' User: Rudolpho     Date: 6.12.07    Time: 14:36
' Updated in $/CKG/Admin/AdminWeb
' ITA: 1440
' 
' *****************  Version 4  *****************
' User: Uha          Date: 13.03.07   Time: 10:53
' Updated in $/CKG/Admin/AdminWeb
' History-Eintrag vorbereitet
' 
' ************************************************
