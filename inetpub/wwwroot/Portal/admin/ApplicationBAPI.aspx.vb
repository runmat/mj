
Imports CKG.Base.Kernel.Security
Imports CKG.Base.Kernel.Common.Common
Imports CKG.Admin.Kernel.ApplicationBAPI

Imports CKG.Portal.PageElements

Public Class ApplicationBAPI
    Inherits System.Web.UI.Page

    Protected WithEvents ucStyles As Styles

#Region " Vom Web Form Designer generierter Code "
    Protected WithEvents lblError As System.Web.UI.WebControls.Label
    Protected WithEvents dgSearchResult As System.Web.UI.WebControls.DataGrid
    Protected WithEvents lblMessage As System.Web.UI.WebControls.Label
    Protected WithEvents trSearchResult As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents trEditUser As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents lnkUserManagement As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lnkGroupManagement As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lbtnNew As System.Web.UI.WebControls.LinkButton
    Protected WithEvents lbtnSave As System.Web.UI.WebControls.LinkButton
    Protected WithEvents lbtnCancel As System.Web.UI.WebControls.LinkButton
    Protected WithEvents lbtnDelete As System.Web.UI.WebControls.LinkButton
    Protected WithEvents trSearchSpacer As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents lnkCustomerManagement As System.Web.UI.WebControls.HyperLink
    Protected WithEvents Form1 As System.Web.UI.HtmlControls.HtmlForm
    Protected WithEvents lnkAppManagment As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lnkBack As System.Web.UI.WebControls.LinkButton
    Protected WithEvents lblAppName As System.Web.UI.WebControls.Label
    Protected WithEvents lnkOrganizationManagement As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lblAppFriendlyName As System.Web.UI.WebControls.Label
    Protected WithEvents ddlBAPI As System.Web.UI.WebControls.DropDownList
    Protected WithEvents txtAppID As System.Web.UI.WebControls.TextBox
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
        ucStyles.TitleText = "Spaltenübersetzungen"
        AdminAuth(Me, m_User, AdminLevel.Master)
        Dim cn As New SqlClient.SqlConnection(m_User.App.Connectionstring)
        Try
            m_App = New App(m_User)

            If Not IsPostBack Then
                lblError.Text = ""

                txtAppID.Text = CStr(Request.QueryString("AppID"))


                cn.Open()
                Dim _App As New Kernel.Application(CInt(txtAppID.Text), cn)
                lblAppName.Text = _App.AppName
                lblAppFriendlyName.Text = _App.AppFriendlyName

                If txtAppID.Text = String.Empty Then
                    lbtnNew.Visible = False
                Else
                    Dim dtCustomers As New Kernel.BAPIList(cn)
                    dtCustomers.AddAllNone(True)
                    With ddlBAPI
                        Dim dv As DataView = dtCustomers.DefaultView
                        dv.Sort = "BAPI"
                        .DataSource = dv
                        .DataTextField = "BAPI"
                        .DataValueField = "ID"
                        .DataBind()
                        .SelectedIndex = 0
                    End With

                    FillForm()
                End If
            End If
        Catch ex As Exception
            lblError.Text = ex.ToString
            lblError.Visible = True
            m_App.WriteErrorText(1, m_User.UserName, "ApplicationBAPI", "PageLoad", lblError.Text)
        Finally
            If cn.State <> ConnectionState.Closed Then
                cn.Close()
            End If
        End Try
    End Sub

#Region " Data and Function "
    Private Sub FillForm()
        trEditUser.Visible = False
        trSearchResult.Visible = False
        Search(True, True, True, True)
    End Sub

    Private Sub FillDataGrid()
        Dim strSort As String = "BAPI"
        If Not ViewState("ResultSort") Is Nothing Then
            strSort = ViewState("ResultSort").ToString
        End If
        FillDataGrid(strSort)
    End Sub
    Private Sub FillDataGrid(ByVal strSort As String)


        trSearchResult.Visible = True
        Dim dvApplicationBAPI As DataView

        'If Not m_context.Cache("myBapiListView") Is Nothing Then
        '    dvApplicationBAPI = CType(m_context.Cache("myBapiListView"), DataView)
        If Not Session("myBapiListView") Is Nothing Then
            dvApplicationBAPI = CType(Session("myBapiListView"), DataView)
        Else

            Dim dtApplicationBAPI As Kernel.ApplicationBAPIList
            Dim cn As New SqlClient.SqlConnection(m_User.App.Connectionstring)
            Try
                cn.Open()

                dtApplicationBAPI = New Kernel.ApplicationBAPIList(CInt(txtAppID.Text), _
                                                                            cn)
                dvApplicationBAPI = dtApplicationBAPI.DefaultView
                'm_context.Cache.Insert("myBapiListView", dvApplicationBAPI, Nothing, DateTime.Now.AddMinutes(20), TimeSpan.Zero)
                Session("myBapiListView") = dvApplicationBAPI
            Finally
                If cn.State <> ConnectionState.Closed Then
                    cn.Close()
                End If
            End Try
        End If
        dvApplicationBAPI.Sort = strSort
        If dvApplicationBAPI.Count > dgSearchResult.PageSize Then
            dgSearchResult.PagerStyle.Visible = True
        Else
            dgSearchResult.PagerStyle.Visible = False
        End If

        With dgSearchResult
            .DataSource = dvApplicationBAPI
            .DataBind()
        End With

    End Sub

    Private Function FillEdit(ByVal intAppId As Integer, ByVal intBapiId As Integer) As Boolean
        SearchMode(False)
        Dim cn As New SqlClient.SqlConnection(m_User.App.Connectionstring)
        Try
            cn.Open()
            Dim _ColTrans As New Kernel.ApplicationBAPI(intAppId, intBapiId, cn)
            txtAppID.Text = _ColTrans.AppId.ToString

            ddlBAPI.Items.FindByValue(intBapiId).Selected = True
            Return True
        Finally
            If cn.State <> ConnectionState.Closed Then
                cn.Close()
            End If
        End Try

    End Function

    Private Sub ClearEdit()
        'Buttons
        lbtnSave.Visible = True
        lbtnDelete.Visible = False
        Me.ddlBAPI.SelectedIndex = 0
        LockEdit(False)
    End Sub

    Private Sub LockEdit(ByVal blnLock As Boolean)
        Dim strBackColor As String = "White"
        If blnLock Then
            strBackColor = "LightGray"
        End If
        ddlBAPI.Enabled = Not blnLock
        ddlBAPI.BackColor = System.Drawing.Color.FromName(strBackColor)
    End Sub

    Private Sub EditEditMode(ByVal intGroupId As Integer, ByVal strOrgName As String)
        If Not FillEdit(intGroupId, strOrgName) Then
            LockEdit(True)
            lbtnSave.Enabled = False
        Else
            lbtnSave.Enabled = True
        End If
        lbtnCancel.Text = "Verwerfen"
    End Sub

    Private Sub EditDeleteMode(ByVal intGroupId As Integer, ByVal strOrgName As String)
        If Not FillEdit(intGroupId, strOrgName) Then
            lbtnDelete.Enabled = False
        Else
            lblMessage.Text = "Möchten Sie die Spalenübersetzung wirklich löschen?"
            lbtnDelete.Enabled = True
        End If
        LockEdit(True)
        lbtnCancel.Text = "Abbrechen"
        lbtnSave.Visible = False
        lbtnDelete.Visible = True
    End Sub

    Private Sub SearchMode(Optional ByVal blnSearchMode As Boolean = True)
        trEditUser.Visible = Not blnSearchMode
        trSearchSpacer.Visible = blnSearchMode
        trSearchResult.Visible = blnSearchMode
        lbtnSave.Visible = Not blnSearchMode
        lbtnCancel.Visible = Not blnSearchMode
        lbtnNew.Visible = blnSearchMode
    End Sub

    Private Sub Search(Optional ByVal blnRefillDataGrid As Boolean = False, Optional ByVal blnResetSelectedIndex As Boolean = False, Optional ByVal blnResetPageIndex As Boolean = False, Optional ByVal blnClearCache As Boolean = False)
        ClearEdit()
        If blnClearCache Then
            'm_context.Cache.Remove("myBapiListView")
            Session.Remove("myBapiListView")
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
        Dim intSource As Integer = 0 ' intSource 
        'Dim intSource As Integer = CInt(Request.QueryString("AppID")) ' intSource 
        Dim strTask As String = "Admin - Spaltenübersetzungen" ' strTask
        ' strIdentification
        ' strDescription
        Dim strCustomerName As String = m_User.CustomerName ' strCustomername
        Dim blnIsTestUser As Boolean = m_User.IsTestUser ' blnIsTestUser
        Dim intSeverity As Integer = 0 ' intSeverity 

        logApp.WriteEntry(strCategory, strUserName, strSessionID, intSource, strTask, strIdentification, strDescription, strCustomerName, m_User.Customer.CustomerId, blnIsTestUser, intSeverity, tblParameters)
    End Sub

    'Private Function SetOldLogParameters(ByVal intAppId As Int32, ByVal strOrgName As String, ByVal tblPar As DataTable) As DataTable
    '    Try
    '        Dim cn As New SqlClient.SqlConnection(m_User.App.Connectionstring)
    '        cn.Open()
    '        Dim _ColTrans As New BasePage.ApplicationBAPI(intAppId, strOrgName, cn)

    '        If tblPar Is Nothing Then
    '            tblPar = CreateLogTableStructure()
    '        End If
    '        With tblPar
    '            .Rows.Add(.NewRow)
    '            .Rows(.Rows.Count - 1)("Status") = "Alt"
    '            .Rows(.Rows.Count - 1)("Anwendung") = lblAppFriendlyName.Text
    '            .Rows(.Rows.Count - 1)("SAP-Name") = _ColTrans.OrgNameAlt
    '            .Rows(.Rows.Count - 1)("Übersetzung") = _ColTrans.NewName
    '            If Not _ColTrans.DisplayOrder = 0 Then .Rows(.Rows.Count - 1)("Reihenfolge-Nr.") = _ColTrans.DisplayOrder.ToString
    '            .Rows(.Rows.Count - 1)("Nullen entfernen") = _ColTrans.NullenEntfernen
    '            .Rows(.Rows.Count - 1)("Text bereinigen") = _ColTrans.TextBereinigen
    '            .Rows(.Rows.Count - 1)("ist Datum") = _ColTrans.IstDatum
    '            .Rows(.Rows.Count - 1)("ABE-Daten") = _ColTrans.ABEDaten
    '            .Rows(.Rows.Count - 1)("Ausrichtung") = _ColTrans.Alignment
    '        End With
    '        Return tblPar
    '    Catch ex As Exception
    '        m_App.WriteErrorText(1, m_User.UserName, "ApplicationBAPI", "SetOldLogParameters", ex.ToString)

    '        Dim dt As New DataTable()
    '        dt.Columns.Add("Fehler beim Erstellen der Log-Parameter", System.Type.GetType("System.String"))
    '        dt.Rows.Add(dt.NewRow)
    '        Dim str As String = ex.Message
    '        If Not ex.InnerException Is Nothing Then
    '            str &= ": " & ex.InnerException.Message
    '        End If
    '        dt.Rows(0)("Fehler beim Erstellen der Log-Parameter") = str
    '        Return dt
    '    End Try
    'End Function

    'Private Function SetNewLogParameters(ByVal tblPar As DataTable) As DataTable
    '    Try
    '        If tblPar Is Nothing Then
    '            tblPar = CreateLogTableStructure()
    '        End If
    '        With tblPar
    '            .Rows.Add(.NewRow)
    '            .Rows(.Rows.Count - 1)("Status") = "Neu"
    '            .Rows(.Rows.Count - 1)("Anwendung") = lblAppFriendlyName.Text
    '            .Rows(.Rows.Count - 1)("BAPI") = ddlBAPI.SelectedItem.Text
    '        End With
    '        Return tblPar
    '    Catch ex As Exception
    '        m_App.WriteErrorText(1, m_User.UserName, "ApplicationBAPI", "SetNewLogParameters", ex.ToString)

    '        Dim dt As New DataTable()
    '        dt.Columns.Add("Fehler beim Erstellen der Log-Parameter", System.Type.GetType("System.String"))
    '        dt.Rows.Add(dt.NewRow)
    '        Dim str As String = ex.Message
    '        If Not ex.InnerException Is Nothing Then
    '            str &= ": " & ex.InnerException.Message
    '        End If
    '        dt.Rows(0)("Fehler beim Erstellen der Log-Parameter") = str
    '        Return dt
    '    End Try
    'End Function

    'Private Function CreateLogTableStructure() As DataTable
    '    Dim tblPar As New DataTable()
    '    With tblPar
    '        .Columns.Add("Status", System.Type.GetType("System.String"))
    '        .Columns.Add("Anwendung", System.Type.GetType("System.String"))
    '        .Columns.Add("SAP-Name", System.Type.GetType("System.String"))
    '        .Columns.Add("Übersetzung", System.Type.GetType("System.String"))
    '        .Columns.Add("Reihenfolge-Nr.", System.Type.GetType("System.String"))
    '        .Columns.Add("Nullen entfernen", System.Type.GetType("System.Boolean"))
    '        .Columns.Add("Text bereinigen", System.Type.GetType("System.Boolean"))
    '        .Columns.Add("ist Datum", System.Type.GetType("System.Boolean"))
    '        .Columns.Add("ABE-Daten", System.Type.GetType("System.Boolean"))
    '        .Columns.Add("Ausrichtung", System.Type.GetType("System.String"))
    '    End With
    '    Return tblPar
    'End Function
#End Region

#Region " Events "
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
            EditEditMode(CInt(e.Item.Cells(0).Text), e.CommandArgument.ToString)
            dgSearchResult.SelectedIndex = e.Item.ItemIndex
        ElseIf e.CommandName = "Delete" Then
            EditDeleteMode(CInt(e.Item.Cells(0).Text), e.CommandArgument.ToString)
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
        Dim cn As New SqlClient.SqlConnection(m_User.App.Connectionstring)
        Try

            cn.Open()
            Dim intAppId As Integer = CInt(txtAppID.Text)

            Dim _ColTrans As New Kernel.ApplicationBAPI(intAppId, _
                                                ddlBAPI.SelectedItem.Value)
            _ColTrans.Save(cn)
            Search(True, True, , True)
            lblMessage.Text = "Die Änderungen wurden gespeichert."
        Catch ex As Exception
            m_App.WriteErrorText(1, m_User.UserName, "ApplicationBAPI", "lbtnSave_Click", ex.ToString)

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

    Private Sub lbtnDelete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lbtnDelete.Click
        Dim cn As New SqlClient.SqlConnection(m_User.App.Connectionstring)
        Try
            Dim _ColTrans As New Kernel.ApplicationBAPI(CInt(txtAppID.Text), ddlBAPI.SelectedItem.Text)

            cn.Open()
            _ColTrans.Delete(cn)
            Search(True, True, True, True)
            lblMessage.Text = "Die Spaltenübersetzung wurde gelöscht."
        Catch ex As Exception
            m_App.WriteErrorText(1, m_User.UserName, "ApplicationBAPI", "lbtnDelete_Click", ex.ToString)

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

    Private Sub lnkBack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lnkBack.Click
        Dim strRetAppID As String = CStr(Request.QueryString("RetAppID"))
        Response.Redirect("AppManagement.aspx?AppID=" & strRetAppID)
    End Sub
#End Region

End Class

' ************************************************
' $History: ApplicationBAPI.aspx.vb $
' 
' *****************  Version 2  *****************
' User: Rudolpho     Date: 6.01.09    Time: 11:45
' Updated in $/CKAG/admin
' ITA 2503  Cache durch Session ersetzt
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
