
Imports CKG.Base.Kernel.Security
Imports CKG.Base.Kernel.Common.Common
Imports CKG.Portal.PageElements

Public Class AutorisierungenLoeschen
    Inherits System.Web.UI.Page

    Protected WithEvents ucStyles As Styles

#Region " Vom Web Form Designer generierter Code "
    Protected WithEvents lblError As System.Web.UI.WebControls.Label
    Protected WithEvents lblMessage As System.Web.UI.WebControls.Label
    Protected WithEvents trSearchResult As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents trEditUser As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents lbtnCancel As System.Web.UI.WebControls.LinkButton
    Protected WithEvents lbtnDelete As System.Web.UI.WebControls.LinkButton
    Protected WithEvents Form1 As System.Web.UI.HtmlControls.HtmlForm
    Protected WithEvents trSearchSpacer As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents dgSearchResult As System.Web.UI.WebControls.DataGrid
    Protected WithEvents trSearchSpacerTop As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents txtAuthorizationID As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtAppFriendlyName As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtInitializedBy As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtInitializedWhen As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtOrganizationName As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtCustomerReference As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtProcessReference As System.Web.UI.WebControls.TextBox
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
        ucStyles.TitleText = "Autorisierungen löschen"
        FormAuth(Me, m_User)

        Try
            m_App = New App(m_User)

            If Not IsPostBack Then
                lblError.Text = ""

                FillForm()
            End If
        Catch ex As Exception
            lblError.Text = ex.ToString
            lblError.Visible = True
            m_App.WriteErrorText(1, m_User.UserName, "AutorisierungenLoeschen", "PageLoad", lblError.Text)
        End Try
    End Sub

#Region " Data and Function "
    Private Sub FillForm()
        trEditUser.Visible = False
        trSearchResult.Visible = False
        Dim cn As New SqlClient.SqlConnection(m_User.App.Connectionstring)
        Search(True, True, True, True)
    End Sub

    Private Sub FillDataGrid()
        Dim strSort As String = "InitializedWhen"
        If Not ViewState("ResultSort") Is Nothing Then
            strSort = ViewState("ResultSort").ToString
        End If
        FillDataGrid(strSort)
    End Sub
    Private Sub FillDataGrid(ByVal strSort As String)
        trSearchResult.Visible = True
        Dim dvZV As DataView

        'If Not m_context.Cache("myAutorisierungenLoeschenListView") Is Nothing Then
        '    dvZV = CType(m_context.Cache("myAutorisierungenLoeschenListView"), DataView)
        If Not Session("myAutorisierungenLoeschenListView") Is Nothing Then
            dvZV = CType(Session("myAutorisierungenLoeschenListView"), DataView)
        Else
            Dim dtAutorisierungenLoeschenList As New DataTable()
            Dim cn As New SqlClient.SqlConnection(m_User.App.Connectionstring)
            Try
                cn.Open()

                Dim daApp As New SqlClient.SqlDataAdapter("SELECT * " & _
                                                          "FROM vwAuthorizationToDelete", cn)
                daApp.Fill(dtAutorisierungenLoeschenList)

                dvZV = dtAutorisierungenLoeschenList.DefaultView
                'm_context.Cache.Insert("myAutorisierungenLoeschenListView", dvZV, Nothing, DateTime.Now.AddMinutes(20), TimeSpan.Zero)
                Session("myAutorisierungenLoeschenListView") = dvZV
            Finally
                If cn.State <> ConnectionState.Closed Then
                    cn.Close()
                End If
            End Try
        End If
        dvZV.Sort = strSort
        If dvZV.Count > dgSearchResult.PageSize Then
            dgSearchResult.PagerStyle.Visible = True
        Else
            dgSearchResult.PagerStyle.Visible = False
        End If

        With dgSearchResult
            .DataSource = dvZV
            .DataBind()
        End With

        Dim str As String
        If dvZV.Count = 1 Then
            str = "Es wurde 1 Eintrag gefunden."
        Else
            str = String.Format("Es wurden {0} Einträge gefunden.", dvZV.Count)
        End If
        lblMessage.Text = str
    End Sub

    Private Function FillEdit(ByVal intZVId As Integer) As Boolean
        SearchMode(False)
        Dim cn As New SqlClient.SqlConnection(m_User.App.Connectionstring)
        Try
            cn.Open()
            Dim dtAutorisierungenLoeschenList As New DataTable()
            Dim daApp As New SqlClient.SqlDataAdapter("SELECT * " & _
                                                      "FROM vwAuthorizationToDelete WHERE AuthorizationID = " & intZVId.ToString, cn)
            daApp.Fill(dtAutorisierungenLoeschenList)

            txtAuthorizationID.Text = CStr(dtAutorisierungenLoeschenList.Rows(0)("AuthorizationID"))
            txtAppFriendlyName.Text = CStr(dtAutorisierungenLoeschenList.Rows(0)("AppFriendlyName"))
            txtInitializedBy.Text = CStr(dtAutorisierungenLoeschenList.Rows(0)("InitializedBy"))
            txtInitializedWhen.Text = CStr(dtAutorisierungenLoeschenList.Rows(0)("InitializedWhen"))
            txtOrganizationName.Text = CStr(dtAutorisierungenLoeschenList.Rows(0)("OrganizationName"))
            txtCustomerReference.Text = CStr(dtAutorisierungenLoeschenList.Rows(0)("CustomerReference"))
            txtProcessReference.Text = CStr(dtAutorisierungenLoeschenList.Rows(0)("ProcessReference"))
            Return True
        Finally
            If cn.State <> ConnectionState.Closed Then
                cn.Close()
            End If
        End Try
    End Function

    Private Sub LockEdit(ByVal blnLock As Boolean)
        Dim strBackColor As String = "White"
        If blnLock Then
            strBackColor = "LightGray"
        End If
        txtAuthorizationID.Enabled = Not blnLock
        txtAuthorizationID.BackColor = System.Drawing.Color.FromName(strBackColor)
        txtAppFriendlyName.Enabled = Not blnLock
        txtAppFriendlyName.BackColor = System.Drawing.Color.FromName(strBackColor)
        txtInitializedBy.Enabled = Not blnLock
        txtInitializedBy.BackColor = System.Drawing.Color.FromName(strBackColor)
        txtInitializedWhen.Enabled = Not blnLock
        txtInitializedWhen.BackColor = System.Drawing.Color.FromName(strBackColor)
        txtOrganizationName.Enabled = Not blnLock
        txtOrganizationName.BackColor = System.Drawing.Color.FromName(strBackColor)
        txtCustomerReference.Enabled = Not blnLock
        txtCustomerReference.BackColor = System.Drawing.Color.FromName(strBackColor)
        txtProcessReference.Enabled = Not blnLock
        txtProcessReference.BackColor = System.Drawing.Color.FromName(strBackColor)
    End Sub

    Private Sub EditEditMode(ByVal intGroupId As Integer)
        If Not FillEdit(intGroupId) Then
            LockEdit(True)
        End If
        lbtnCancel.Text = "Verwerfen"
    End Sub

    Private Sub EditDeleteMode(ByVal intGroupId As Integer)
        If Not FillEdit(intGroupId) Then
            lbtnDelete.Enabled = False
        Else
            lblMessage.Text = "Möchten Sie die Autorisierung wirklich löschen?"
            lbtnDelete.Enabled = True
        End If
        LockEdit(True)
        lbtnCancel.Text = "Abbrechen"
        lbtnDelete.Visible = True
    End Sub

    Private Sub SearchMode(Optional ByVal blnSearchMode As Boolean = True)
        trEditUser.Visible = Not blnSearchMode
        trSearchResult.Visible = blnSearchMode
        trSearchSpacerTop.Visible = blnSearchMode
        lbtnCancel.Visible = Not blnSearchMode
    End Sub

    Private Sub Search(Optional ByVal blnRefillDataGrid As Boolean = False, Optional ByVal blnResetSelectedIndex As Boolean = False, Optional ByVal blnResetPageIndex As Boolean = False, Optional ByVal blnClearCache As Boolean = False)
        If blnClearCache Then
            'm_context.Cache.Remove("myAutorisierungenLoeschenListView")
            Session.Remove("myAutorisierungenLoeschenListView")
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
        Dim strTask As String = m_User.Applications.Select("AppID = '" & CStr(Request.QueryString("AppID")) & "'")(0)("AppFriendlyName").ToString ' strTask
        ' strIdentification
        ' strDescription
        Dim strCustomerName As String = m_User.CustomerName ' strCustomername
        Dim blnIsTestUser As Boolean = m_User.IsTestUser ' blnIsTestUser
        Dim intSeverity As Integer = 0 ' intSeverity 

        logApp.WriteEntry(strCategory, strUserName, strSessionID, intSource, strTask, strIdentification, strDescription, strCustomerName, m_User.Customer.CustomerId, blnIsTestUser, intSeverity, tblParameters)
    End Sub

    Private Function SetOldLogParameters(ByVal intZVId As Int32, ByVal tblPar As DataTable) As DataTable
        Dim cn As New SqlClient.SqlConnection(m_User.App.Connectionstring)
        Try

            cn.Open()
            Dim dtAutorisierungenLoeschenList As New DataTable()
            Dim daApp As New SqlClient.SqlDataAdapter("SELECT * " & _
                                                      "FROM vwAuthorizationToDelete WHERE AuthorizationID = " & intZVId.ToString, cn)
            daApp.Fill(dtAutorisierungenLoeschenList)

            If tblPar Is Nothing Then
                tblPar = CreateLogTableStructure()
            End If
            With tblPar
                .Rows.Add(.NewRow)
                .Rows(.Rows.Count - 1)("Status") = "Alt"
                .Rows(.Rows.Count - 1)("Anwendung") = CStr(dtAutorisierungenLoeschenList.Rows(0)("AppFriendlyName"))
                .Rows(.Rows.Count - 1)("Angelegt von") = CStr(dtAutorisierungenLoeschenList.Rows(0)("InitializedBy"))
                .Rows(.Rows.Count - 1)("Angelegt am") = CDate(dtAutorisierungenLoeschenList.Rows(0)("InitializedWhen"))
                .Rows(.Rows.Count - 1)("Organisation") = CStr(dtAutorisierungenLoeschenList.Rows(0)("OrganizationName"))
                .Rows(.Rows.Count - 1)("Kundenreferenz") = CStr(dtAutorisierungenLoeschenList.Rows(0)("CustomerReference"))
                .Rows(.Rows.Count - 1)("Prozessreferenz") = CStr(dtAutorisierungenLoeschenList.Rows(0)("ProcessReference"))
            End With
            Return tblPar
        Catch ex As Exception
            m_App.WriteErrorText(1, m_User.UserName, "AutorisierungenLoeschen", "SetOldLogParameters", ex.ToString)

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

    Private Function CreateLogTableStructure() As DataTable
        Dim tblPar As New DataTable()
        With tblPar
            .Columns.Add("Status", System.Type.GetType("System.String"))
            .Columns.Add("Anwendung", System.Type.GetType("System.String"))
            .Columns.Add("Angelegt von", System.Type.GetType("System.String"))
            .Columns.Add("Angelegt am", System.Type.GetType("System.DateTime"))
            .Columns.Add("Organisation", System.Type.GetType("System.String"))
            .Columns.Add("Kundenreferenz", System.Type.GetType("System.String"))
            .Columns.Add("Prozessreferenz", System.Type.GetType("System.String"))
        End With
        Return tblPar
    End Function
#End Region

#Region " Events "
    Private Sub dgSearchResult_SortCommand(ByVal source As System.Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs) Handles dgSearchResult.SortCommand
        Dim strSort As String = e.SortExpression
        If Not ViewState("ResultSort") Is Nothing AndAlso ViewState("ResultSort").ToString = strSort Then
            strSort &= " DESC"
        End If
        ViewState("ResultSort") = strSort
        FillDataGrid(strSort)
    End Sub

    Private Sub dgSearchResult_ItemCommand(ByVal source As System.Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles dgSearchResult.ItemCommand
        If e.CommandName = "Edit" Then
            EditEditMode(CInt(e.Item.Cells(0).Text))
            dgSearchResult.SelectedIndex = e.Item.ItemIndex
        ElseIf e.CommandName = "Delete" Then
            EditDeleteMode(CInt(e.Item.Cells(0).Text))
            dgSearchResult.SelectedIndex = e.Item.ItemIndex
        End If
    End Sub

    Private Sub dgSearchResult_PageIndexChanged(ByVal source As System.Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles dgSearchResult.PageIndexChanged
        dgSearchResult.CurrentPageIndex = e.NewPageIndex
        FillDataGrid()
    End Sub

    Private Sub lbtnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lbtnCancel.Click
        Search(, True)
    End Sub

    Private Sub lbtnDelete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lbtnDelete.Click
        Dim tblLogParameter As DataTable
        Dim cn As New SqlClient.SqlConnection(m_User.App.Connectionstring)
        Try

            cn.Open()
            tblLogParameter = New DataTable
            tblLogParameter = SetOldLogParameters(CInt(txtAuthorizationID.Text), tblLogParameter)

            Dim strDeleteSQL As String = "DELETE " & _
                                          "FROM [Authorization] " & _
                                          "WHERE AuthorizationID=@AuthorizationID"

            Dim cmd As New SqlClient.SqlCommand()
            cmd.Connection = cn
            cmd.Parameters.AddWithValue("@AuthorizationID", CInt(txtAuthorizationID.Text))

            cmd.CommandText = strDeleteSQL
            cmd.ExecuteNonQuery()

            Log(txtAuthorizationID.Text, "Autorisierung löschen", tblLogParameter)
            Search(True, True, True, True)
            lblMessage.Text = "Die Autorisierung wurde gelöscht."
        Catch ex As Exception
            m_App.WriteErrorText(1, m_User.UserName, "AutorisierungenLoeschen", "lbtnDelete_Click", ex.ToString)

            lblError.Text = ex.Message
            If Not ex.InnerException Is Nothing Then
                lblError.Text &= ": " & ex.InnerException.Message
            End If
            tblLogParameter = New DataTable
            Log(txtAuthorizationID.Text, lblError.Text, tblLogParameter, "ERR")
        Finally
            If cn.State <> ConnectionState.Closed Then
                cn.Close()
            End If
        End Try
    End Sub
#End Region
End Class

' ************************************************
' $History: AutorisierungenLoeschen.aspx.vb $
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
