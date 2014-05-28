
Imports CKG.Base.Kernel.Security
Imports CKG.Base.Kernel.Common.Common
Imports CKG.Portal.PageElements

Public Class Versichererpflege
    Inherits System.Web.UI.Page

    Protected WithEvents ucStyles As Styles

#Region " Vom Web Form Designer generierter Code "
    Protected WithEvents lblError As System.Web.UI.WebControls.Label
    Protected WithEvents dgSearchResult As System.Web.UI.WebControls.DataGrid
    Protected WithEvents lblMessage As System.Web.UI.WebControls.Label
    Protected WithEvents trSearchResult As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents trEditUser As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents lbtnNew As System.Web.UI.WebControls.LinkButton
    Protected WithEvents lbtnSave As System.Web.UI.WebControls.LinkButton
    Protected WithEvents lbtnCancel As System.Web.UI.WebControls.LinkButton
    Protected WithEvents lbtnDelete As System.Web.UI.WebControls.LinkButton
    Protected WithEvents btnSuche As System.Web.UI.WebControls.Button
    Protected WithEvents trSearch As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents trSearchSpacer As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents Form1 As System.Web.UI.HtmlControls.HtmlForm
    Protected WithEvents txtFilterSAPNr As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtSAPNr As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtName1 As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtFilterName1 As System.Web.UI.WebControls.TextBox
    Protected WithEvents lnkHalterpflege As System.Web.UI.WebControls.HyperLink
    Protected WithEvents txtVersichererID As System.Web.UI.WebControls.TextBox
    Protected WithEvents ddlKundennr As System.Web.UI.WebControls.DropDownList
    Protected WithEvents txtKunde As System.Web.UI.WebControls.TextBox
    Protected WithEvents trKunde As System.Web.UI.HtmlControls.HtmlTableRow
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
        trKunde.Visible = False
        Dim cn As New SqlClient.SqlConnection(m_User.App.Connectionstring)
        Dim tblCustomer As New DataTable()
        Dim row As DataRow

        Try
            cn.Open()

            Dim daApp As New SqlClient.SqlDataAdapter("SELECT CustomerID, KUNNR, Customername FROM Customer ORDER BY Customername ASC", cn)
            daApp.Fill(tblCustomer)

            For Each row In tblCustomer.Rows
                row("Customername") = CStr(row("KUNNR")) & " - " & CStr(row("Customername"))
            Next

            tblCustomer.AcceptChanges()

            With ddlKundennr
                .DataSource = tblCustomer.DefaultView
                .DataTextField = "Customername"
                .DataValueField = "CustomerID"
                .DataBind()
            End With

        Catch ex As Exception
            lblError.Text = "Kundenliste konnte nicht ermittelt werden!"
            btnSuche.Enabled = False
            lbtnNew.Enabled = False
        Finally
            cn.Dispose()
        End Try
    End Sub

    Private Sub FillDataGrid()
        Dim strSort As String = "VersichererID"
        If Not ViewState("ResultSort") Is Nothing Then
            strSort = ViewState("ResultSort").ToString
        End If
        FillDataGrid(strSort)
    End Sub
    Private Sub FillDataGrid(ByVal strSort As String)
        trSearchResult.Visible = True
        Dim dvVersicherer As DataView

        '§§§ JVE 19.10.2006: Cache nicht verwenden!!!
        'If Not m_context.Cache("myVersichererListView") Is Nothing Then
        '    dvVersicherer = CType(m_context.Cache("myVersichererListView"), DataView)
        'Else
        Dim dtVersicherer As Base.Kernel.Admin.VersichererList
        Dim cn As New SqlClient.SqlConnection(m_User.App.Connectionstring)
        Dim intKundennr As Integer
        Dim blnError As Boolean = False

        Try
            intKundennr = CInt(Left(ddlKundennr.SelectedItem.Text, ddlKundennr.SelectedItem.Text.IndexOf("-")).Trim)
        Catch ex As Exception
            blnError = True
        End Try

        If Not blnError Then
            cn.Open()

            'dtVersicherer = New VersichererList(txtFilterSAPNr.Text, txtFilterName1.Text, cn)  '§§§ JVE 19.10.2006
            dtVersicherer = New Base.Kernel.Admin.VersichererList(cn, m_User, intKundennr)
            cn.Close()      '§§§ JVE 19.10.2006

            If dtVersicherer.Rows.Count > 0 Then
                dvVersicherer = dtVersicherer.DefaultView
                'm_context.Cache.Insert("myVersichererListView", dvVersicherer, Nothing, DateTime.Now.AddMinutes(20), TimeSpan.Zero)

                'End If
                dvVersicherer.Sort = strSort
                If dvVersicherer.Count > dgSearchResult.PageSize Then
                    dgSearchResult.PagerStyle.Visible = True
                Else
                    dgSearchResult.PagerStyle.Visible = False
                End If

                With dgSearchResult
                    .DataSource = dvVersicherer
                    .DataBind()
                End With
                dgSearchResult.Visible = True
                lblError.Text = String.Empty
            Else
                dgSearchResult.DataSource = Nothing
                dgSearchResult.Visible = False
                lblError.Text = "Keine Daten gefunden."
            End If
        Else
            lblError.Text = "Kundennummer konnte nicht ermittelt werden."
        End If
    End Sub

    Private Function FillEdit(ByVal intVersichererId As Integer) As Boolean
        SearchMode(False)
        Dim cn As New SqlClient.SqlConnection(m_User.App.Connectionstring)
        cn.Open()
        Dim _Versicherer As New Base.Business.Versicherer(intVersichererId, cn)
        txtVersichererID.Text = _Versicherer.VersichererId.ToString
        txtSAPNr.Text = _Versicherer.SAPNr
        txtName1.Text = _Versicherer.Name1
        Return True
    End Function

    Private Sub ClearEdit()
        txtVersichererID.Text = "-1"
        txtSAPNr.Text = ""
        txtName1.Text = ""
        txtKunde.Text = ddlKundennr.SelectedItem.Text
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
        txtVersichererID.Enabled = Not blnLock
        txtVersichererID.BackColor = System.Drawing.Color.FromName(strBackColor)
        txtSAPNr.Enabled = Not blnLock
        txtSAPNr.BackColor = System.Drawing.Color.FromName(strBackColor)
        txtName1.Enabled = Not blnLock
        txtName1.BackColor = System.Drawing.Color.FromName(strBackColor)
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
            lblMessage.Text = "Möchten Sie den Versicherer wirklich löschen?"
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
        trKunde.Visible = Not blnSearchMode
    End Sub

    Private Sub Search(Optional ByVal blnRefillDataGrid As Boolean = False, Optional ByVal blnResetSelectedIndex As Boolean = False, Optional ByVal blnResetPageIndex As Boolean = False, Optional ByVal blnClearCache As Boolean = False)
        ClearEdit()
        '§§§ JVE 19.10.2006: Kein Cache verwenden!!!
        'If blnClearCache Then
        '    m_context.Cache.Remove("myVersichererListView")
        'End If
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
        Dim strTask As String = m_User.Applications.Select("AppID = '" & CStr(Request.QueryString("AppID")) & "'")(0)("AppFriendlyName").ToString ' strTask
        ' strIdentification
        ' strDescription
        Dim strCustomerName As String = m_User.CustomerName ' strCustomername
        Dim blnIsTestUser As Boolean = m_User.IsTestUser ' blnIsTestUser
        Dim intSeverity As Integer = 0 ' intSeverity 

        logApp.WriteEntry(strCategory, strUserName, strSessionID, intSource, strTask, strIdentification, strDescription, strCustomerName, m_User.Customer.CustomerId, blnIsTestUser, intSeverity, tblParameters)
    End Sub

    Private Function SetOldLogParameters(ByVal intVersichererId As Int32, ByVal tblPar As DataTable) As DataTable
        Try
            Dim cn As New SqlClient.SqlConnection(m_User.App.Connectionstring)
            cn.Open()
            Dim _Versicherer As New Base.Business.Versicherer(intVersichererId, cn)

            If tblPar Is Nothing Then
                tblPar = CreateLogTableStructure()
            End If
            With tblPar
                .Rows.Add(.NewRow)
                .Rows(.Rows.Count - 1)("Status") = "Alt"
                .Rows(.Rows.Count - 1)("SAP-Nr") = _Versicherer.SAPNr
                .Rows(.Rows.Count - 1)("Name1") = _Versicherer.Name1
            End With
            Return tblPar
        Catch ex As Exception
            m_App.WriteErrorText(1, m_User.UserName, "Versichererpflege", "SetOldLogParameters", ex.ToString)

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
                .Rows(.Rows.Count - 1)("SAP-Nr") = txtSAPNr.Text
                .Rows(.Rows.Count - 1)("Name1") = txtName1.Text
            End With
            Return tblPar
        Catch ex As Exception
            m_App.WriteErrorText(1, m_User.UserName, "Versichererpflege", "SetNewLogParameters", ex.ToString)

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
            .Columns.Add("SAP-Nr", System.Type.GetType("System.String"))
            .Columns.Add("Name1", System.Type.GetType("System.String"))
        End With
        Return tblPar
    End Function
#End Region

#Region " Events "
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        ' Hier Benutzercode zur Seiteninitialisierung einfügen
        m_User = GetUser(Me)
        ucHeader.InitUser(m_User)
        ucStyles.TitleText = "Versichererpflege"
        AdminAuth(Me, m_User, AdminLevel.Master)

        Try
            m_App = New App(m_User)

            If Not IsPostBack Then
                lblError.Text = ""

                Dim strAppID As String = m_User.Applications.Select("AppUrl LIKE '%" & lnkHalterpflege.NavigateUrl & "%'")(0)("AppID").ToString
                lnkHalterpflege.NavigateUrl &= "?AppID=" & strAppID
                FillForm()
            End If
        Catch ex As Exception
            m_App.WriteErrorText(1, m_User.UserName, "Versichererpflege", "Page_Load", ex.ToString)

            lblError.Text = ex.ToString
            lblError.Visible = True
        End Try
    End Sub

    Private Sub btnSuche_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSuche.Click
        Search(True, True, True, True)
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
        Search(, True)
    End Sub

    Private Sub lbtnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lbtnNew.Click
        SearchMode(False)
        ClearEdit()
        Dim cn As New SqlClient.SqlConnection(m_User.App.Connectionstring)
        cn.Open()
    End Sub

    Private Sub lbtnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lbtnSave.Click
        Dim tblLogParameter As DataTable
        Try
            Dim cn As New SqlClient.SqlConnection(m_User.App.Connectionstring)
            cn.Open()
            Dim intVersichererId As Integer = CInt(txtVersichererID.Text)
            Dim strLogMsg As String = "Versicherer anlegen"
            If Not (intVersichererId = -1) Then
                strLogMsg = "Versicherer ändern"
                tblLogParameter = New DataTable
                tblLogParameter = SetOldLogParameters(intVersichererId, tblLogParameter)
            End If

            Dim _Versicherer As New Base.Business.Versicherer(intVersichererId, _
                                                txtSAPNr.Text, _
                                                txtName1.Text)
            Dim intKundennr As Integer
            Dim blnError As Boolean = False

            Try
                intKundennr = CInt(Left(ddlKundennr.SelectedItem.Text, ddlKundennr.SelectedItem.Text.IndexOf("-")).Trim)
            Catch ex As Exception
                blnError = True
            End Try

            If Not blnError Then
                _Versicherer.Save(cn, intKundennr)   '§§§ JVE 19.10.2006: Kundennummer mit übergeben!
                tblLogParameter = New DataTable
                tblLogParameter = SetNewLogParameters(tblLogParameter)
                Log(_Versicherer.VersichererId.ToString, strLogMsg, tblLogParameter)
                Search(True, True, , True)
                lblMessage.Text = "Die Änderungen wurden gespeichert."
            Else
                lblError.Text = "Fehler bei der Ermittlung der Kundennr! Daten konnten nicht gespeichert werden."
            End If

        Catch ex As Exception
            m_App.WriteErrorText(1, m_User.UserName, "Versichererpflege", "lbtnSave_Click", ex.ToString)

            lblError.Text = ex.Message
            If Not ex.InnerException Is Nothing Then
                lblError.Text &= ": " & ex.InnerException.Message
            End If
            tblLogParameter = New DataTable
            Log(txtVersichererID.Text, lblError.Text, tblLogParameter, "ERR")
        End Try
    End Sub

    Private Sub lbtnDelete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lbtnDelete.Click
        Dim tblLogParameter As DataTable
        Try
            Dim _Versicherer As New Base.Business.Versicherer(CInt(txtVersichererID.Text))
            Dim cn As New SqlClient.SqlConnection(m_User.App.Connectionstring)
            cn.Open()
            tblLogParameter = New DataTable
            tblLogParameter = SetOldLogParameters(_Versicherer.VersichererId, tblLogParameter)
            _Versicherer.Delete(cn)
            Log(_Versicherer.VersichererId.ToString, "Versicherer löschen", tblLogParameter)
            Search(True, True, True, True)
            lblMessage.Text = "Der Versicherer wurde gelöscht."
        Catch ex As Exception
            m_App.WriteErrorText(1, m_User.UserName, "Versichererpflege", "lbtnDelete_Click", ex.ToString)

            lblError.Text = ex.Message
            If Not ex.InnerException Is Nothing Then
                lblError.Text &= ": " & ex.InnerException.Message
            End If
            tblLogParameter = New DataTable
            Log(txtVersichererID.Text, lblError.Text, tblLogParameter, "ERR")
        End Try
    End Sub
#End Region
End Class

' ************************************************
' $History: Versichererpflege.aspx.vb $
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
' User: Uha          Date: 13.03.07   Time: 10:53
' Updated in $/CKG/Admin/AdminWeb
' History-Eintrag vorbereitet
' 
' ************************************************
