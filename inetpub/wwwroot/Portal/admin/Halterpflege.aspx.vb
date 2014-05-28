
Imports CKG.Base.Kernel.Security
Imports CKG.Base.Kernel.Common.Common
Imports CKG.Portal.PageElements

Public Class Halterpflege
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
    Protected WithEvents lnkVersichererpflege As System.Web.UI.WebControls.HyperLink
    Protected WithEvents txtName2 As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtStrasseHNr As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtOrt As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtHalterID As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtKBANR As System.Web.UI.WebControls.TextBox
    Protected WithEvents ddlKundennr As System.Web.UI.WebControls.DropDownList
    Protected WithEvents txtKunnr As System.Web.UI.WebControls.TextBox
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
        ucStyles.TitleText = "Halterpflege"
        AdminAuth(Me, m_User, AdminLevel.Master)

        Try
            m_App = New App(m_User)

            If Not IsPostBack Then
                lblError.Text = ""

                Dim strAppID As String = m_User.Applications.Select("AppUrl LIKE '%" & lnkVersichererpflege.NavigateUrl & "%'")(0)("AppID").ToString
                lnkVersichererpflege.NavigateUrl &= "?AppID=" & strAppID
                FillForm()
            End If
        Catch ex As Exception
            m_App.WriteErrorText(1, m_User.UserName, "Halterpflege", "Page_Load", ex.ToString)

            lblError.Text = ex.ToString
            lblError.Visible = True
        End Try
    End Sub

#Region " Data and Function "
    Private Sub FillForm()
        trEditUser.Visible = False
        trSearchResult.Visible = False
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
            cn.Close()
            cn.Dispose()
        End Try
    End Sub

    Private Sub FillDataGrid()
        Dim strSort As String = "HalterID"
        If Not ViewState("ResultSort") Is Nothing Then
            strSort = ViewState("ResultSort").ToString
        End If
        FillDataGrid(strSort)
    End Sub
    Private Sub FillDataGrid(ByVal strSort As String)
        trSearchResult.Visible = True
        Dim dvHalter As DataView

        'If Not m_context.Cache("myHalterListView") Is Nothing Then
        '    dvHalter = CType(m_context.Cache("myHalterListView"), DataView)
        'Else
        Dim dtHalter As Base.Kernel.Admin.HalterList
        Dim cn As New SqlClient.SqlConnection(m_User.App.Connectionstring)
        Dim intKundennr As Integer
        Dim blnError As Boolean = False

        Try
            intKundennr = CInt(Left(ddlKundennr.SelectedItem.Text, ddlKundennr.SelectedItem.Text.IndexOf("-")).Trim)
        Catch ex As Exception
            blnError = True
        End Try

        If Not blnError Then
            Try
                cn.Open()

                dtHalter = New Base.Kernel.Admin.HalterList(cn, intKundennr)
                cn.Close()

                If (dtHalter.Rows.Count > 0) Then
                    dvHalter = dtHalter.DefaultView
                    'm_context.Cache.Insert("myHalterListView", dvHalter, Nothing, DateTime.Now.AddMinutes(20), TimeSpan.Zero)
                    'End If
                    dvHalter.Sort = strSort
                    If dvHalter.Count > dgSearchResult.PageSize Then
                        dgSearchResult.PagerStyle.Visible = True
                    Else
                        dgSearchResult.PagerStyle.Visible = False
                    End If

                    With dgSearchResult
                        .DataSource = dvHalter
                        .DataBind()
                    End With
                    lblError.Text = String.Empty
                    dgSearchResult.Visible = True

                Else
                    lblError.Text = "Keine Daten gefunden."
                    dgSearchResult.DataSource = Nothing
                    dgSearchResult.Visible = False
                End If
            Finally
                If cn.State <> ConnectionState.Closed Then
                    cn.Close()
                End If
            End Try
        Else
                lblError.Text = "Kundennummer konnte nicht ermittelt werden."
        End If
    End Sub

    Private Function FillEdit(ByVal intHalterId As Integer) As Boolean
        SearchMode(False)
        Dim cn As New SqlClient.SqlConnection(m_User.App.Connectionstring)
        Try
            cn.Open()
            Dim _Halter As New Base.Business.Halter(intHalterId, cn)
            txtHalterID.Text = _Halter.HalterId.ToString
            txtSAPNr.Text = _Halter.SAPNr
            txtName1.Text = _Halter.Name1
            txtName2.Text = _Halter.Name2
            txtKunnr.Text = ddlKundennr.SelectedItem.Text
            txtStrasseHNr.Text = _Halter.StrasseHNr
            txtOrt.Text = _Halter.Ort
            txtKBANR.Text = _Halter.KBANR
            Return True
        Finally
            If cn.State <> ConnectionState.Closed Then
                cn.Close()
            End If
        End Try
    End Function

    Private Sub ClearEdit()
        txtHalterID.Text = "-1"
        txtSAPNr.Text = ""
        txtName1.Text = ""
        txtName2.Text = ""
        txtStrasseHNr.Text = ""
        txtOrt.Text = ""
        txtKBANR.Text = ""
        txtKunnr.Text = ddlKundennr.SelectedItem.Text
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
        txtHalterID.Enabled = Not blnLock
        txtHalterID.BackColor = System.Drawing.Color.FromName(strBackColor)
        txtSAPNr.Enabled = Not blnLock
        txtSAPNr.BackColor = System.Drawing.Color.FromName(strBackColor)
        txtName1.Enabled = Not blnLock
        txtName1.BackColor = System.Drawing.Color.FromName(strBackColor)
        txtName2.Enabled = Not blnLock
        txtName2.BackColor = System.Drawing.Color.FromName(strBackColor)
        txtStrasseHNr.Enabled = Not blnLock
        txtStrasseHNr.BackColor = System.Drawing.Color.FromName(strBackColor)
        txtOrt.Enabled = Not blnLock
        txtOrt.BackColor = System.Drawing.Color.FromName(strBackColor)
        txtKBANR.Enabled = Not blnLock
        txtKBANR.BackColor = System.Drawing.Color.FromName(strBackColor)
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
            lblMessage.Text = "Möchten Sie den Halter wirklich löschen?"
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
        'If blnClearCache Then
        '    m_context.Cache.Remove("myHalterListView")
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

    Private Function SetOldLogParameters(ByVal intHalterId As Int32, ByVal tblPar As DataTable) As DataTable
        Dim cn As New SqlClient.SqlConnection(m_User.App.Connectionstring)
        Try

            cn.Open()
            Dim _Halter As New Base.Business.Halter(intHalterId, cn)

            If tblPar Is Nothing Then
                tblPar = CreateLogTableStructure()
            End If
            With tblPar
                .Rows.Add(.NewRow)
                .Rows(.Rows.Count - 1)("Status") = "Alt"
                .Rows(.Rows.Count - 1)("SAP-Nr") = _Halter.SAPNr
                .Rows(.Rows.Count - 1)("Name1") = _Halter.Name1
                .Rows(.Rows.Count - 1)("Name2") = _Halter.Name2
                .Rows(.Rows.Count - 1)("Strasse und Hausnr.") = _Halter.StrasseHNr
                .Rows(.Rows.Count - 1)("Ort") = _Halter.Ort
                .Rows(.Rows.Count - 1)("KBANR") = _Halter.KBANR
            End With
            Return tblPar
        Catch ex As Exception
            m_App.WriteErrorText(1, m_User.UserName, "Halterpflege", "SetOldLogParameters", ex.ToString)

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
                .Rows(.Rows.Count - 1)("SAP-Nr") = txtSAPNr.Text
                .Rows(.Rows.Count - 1)("Name1") = txtName1.Text
                .Rows(.Rows.Count - 1)("Name2") = txtName2.Text
                .Rows(.Rows.Count - 1)("Strasse und Hausnr.") = txtStrasseHNr.Text
                .Rows(.Rows.Count - 1)("Ort") = txtOrt.Text
                .Rows(.Rows.Count - 1)("KBANR") = txtKBANR.Text
            End With
            Return tblPar
        Catch ex As Exception
            m_App.WriteErrorText(1, m_User.UserName, "Halterpflege", "SetNewLogParameters", ex.ToString)

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
            .Columns.Add("Name2", System.Type.GetType("System.String"))
            .Columns.Add("Strasse und Hausnr.", System.Type.GetType("System.String"))
            .Columns.Add("Ort", System.Type.GetType("System.String"))
            .Columns.Add("KBANR", System.Type.GetType("System.String"))
        End With
        Return tblPar
    End Function
#End Region

#Region " Events "
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
        'Dim cn As New SqlClient.SqlConnection(m_User.App.Connectionstring)
        'cn.Open()
    End Sub

    Private Sub lbtnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lbtnSave.Click
        Dim tblLogParameter As DataTable
        Dim cn As New SqlClient.SqlConnection(m_User.App.Connectionstring)
        Try

            cn.Open()
            Dim intHalterId As Integer = CInt(txtHalterID.Text)
            Dim strLogMsg As String = "Halter anlegen"
            If Not (intHalterId = -1) Then
                strLogMsg = "Halter ändern"
                tblLogParameter = New DataTable
                tblLogParameter = SetOldLogParameters(intHalterId, tblLogParameter)
            End If

            Dim intKundennr As Integer
            Dim blnError As Boolean = False

            Try
                intKundennr = CInt(Left(ddlKundennr.SelectedItem.Text, ddlKundennr.SelectedItem.Text.IndexOf("-")).Trim)
            Catch ex As Exception
                blnError = True
            End Try

            If Not blnError Then
                Dim _Halter As New Base.Business.Halter(intHalterId, txtSAPNr.Text, txtName1.Text, txtName2.Text, txtStrasseHNr.Text, txtOrt.Text, txtKBANR.Text, intKundennr)
                _Halter.Save(cn)
                tblLogParameter = New DataTable
                tblLogParameter = SetNewLogParameters(tblLogParameter)
                Log(_Halter.HalterId.ToString, strLogMsg, tblLogParameter)
                Search(True, True, , True)
                lblMessage.Text = "Die Änderungen wurden gespeichert."
            Else
                lblError.Text = "Kundennummer konnte nicht ermittelt werden. Daten nicht gespeichert!"
            End If

        Catch ex As Exception
            m_App.WriteErrorText(1, m_User.UserName, "Halterpflege", "lbtnSave_Click", ex.ToString)

            lblError.Text = ex.Message
            If Not ex.InnerException Is Nothing Then
                lblError.Text &= ": " & ex.InnerException.Message
            End If
            tblLogParameter = New DataTable
            Log(txtHalterID.Text, lblError.Text, tblLogParameter, "ERR")
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
            Dim _Halter As New Base.Business.Halter(CInt(txtHalterID.Text))
            cn.Open()
            tblLogParameter = New DataTable
            tblLogParameter = SetOldLogParameters(_Halter.HalterId, tblLogParameter)
            _Halter.Delete(cn)
            Log(_Halter.HalterId.ToString, "Halter gelöscht", tblLogParameter)
            Search(True, True, True, True)
            lblMessage.Text = "Der Halter wurde gelöscht."
        Catch ex As Exception
            m_App.WriteErrorText(1, m_User.UserName, "Halterpflege", "lbtnDelete_Click", ex.ToString)

            lblError.Text = ex.Message
            If Not ex.InnerException Is Nothing Then
                lblError.Text &= ": " & ex.InnerException.Message
            End If
            tblLogParameter = New DataTable
            Log(txtHalterID.Text, lblError.Text, tblLogParameter, "ERR")
        Finally
            If cn.State <> ConnectionState.Closed Then
                cn.Close()
            End If
        End Try
    End Sub
#End Region
End Class

' ************************************************
' $History: Halterpflege.aspx.vb $
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
' User: Rudolpho     Date: 6.12.07    Time: 14:36
' Updated in $/CKG/Admin/AdminWeb
' ITA: 1440
' 
' *****************  Version 6  *****************
' User: Uha          Date: 13.03.07   Time: 10:53
' Updated in $/CKG/Admin/AdminWeb
' History-Eintrag vorbereitet
' 
' ************************************************
