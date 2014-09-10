Imports CKG.Base.Business
Imports CKG.Base.Kernel
Imports CKG.Base.Kernel.Common.Common
Imports CKG.Portal.PageElements

Public Class SchluesselTuetenVorgaben
    Inherits System.Web.UI.Page

    Protected WithEvents ucStyles As Styles

#Region " Vom Web Form Designer generierter Code "
    Protected WithEvents lblError As System.Web.UI.WebControls.Label
    Protected WithEvents lblMessage As System.Web.UI.WebControls.Label
    Protected WithEvents trSearchResult As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents trEditUser As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents lbtnNew As System.Web.UI.WebControls.LinkButton
    Protected WithEvents lbtnSave As System.Web.UI.WebControls.LinkButton
    Protected WithEvents lbtnCancel As System.Web.UI.WebControls.LinkButton
    Protected WithEvents lbtnDelete As System.Web.UI.WebControls.LinkButton
    Protected WithEvents Form1 As System.Web.UI.HtmlControls.HtmlForm
    Protected WithEvents txtZVID As System.Web.UI.WebControls.TextBox
    Protected WithEvents trSearchSpacer As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents dgSearchResult As System.Web.UI.WebControls.DataGrid
    Protected WithEvents trSearchSpacerTop As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents txtCHASSIS_NUM As System.Web.UI.WebControls.TextBox
    Protected WithEvents valCHASSIS_NUM As System.Web.UI.WebControls.RequiredFieldValidator
    Protected WithEvents txtCHASSIS_NUM_BIS As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtERSSCHLUESSEL As System.Web.UI.WebControls.TextBox
    Protected WithEvents RequiredFieldValidator1 As System.Web.UI.WebControls.RequiredFieldValidator
    Protected WithEvents txtCARPASS As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtRADIOCODEKARTE As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtNAVICD As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtCHIPKARTE As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtCOCBESCH As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtSH_ERS_FB As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtNAVICODEKARTE As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtWFSCODEKARTE As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtPRUEFBUCH_LKW As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtCHASSIS_NUM_Alt As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtCHASSIS_NUM_BIS_Alt As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtERSSCHLUESSEL_Alt As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtCARPASS_Alt As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtRADIOCODEKARTE_Alt As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtNAVICD_Alt As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtCHIPKARTE_Alt As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtCOCBESCH_Alt As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtNAVICODEKARTE_Alt As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtWFSCODEKARTE_Alt As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtSH_ERS_FB_Alt As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtPRUEFBUCH_LKW_Alt As System.Web.UI.WebControls.TextBox
    Protected WithEvents valCHASSIS_NUM_BIS As System.Web.UI.WebControls.RequiredFieldValidator
    Protected WithEvents Textbox1 As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtHersteller As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtModell As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtHersteller_alt As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtModell_alt As System.Web.UI.WebControls.TextBox
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
    Private m_User As Base.Kernel.Security.User
    Private m_App As Base.Kernel.Security.App
    Private m_context As HttpContext = HttpContext.Current
    Private objVorgaben As AppSIXT.SIXT_SchluesselTuetenVorgaben
#End Region

#Region " Data and Function "
    Private Function CheckChassisNo(ByVal strIn As String) As String
        strIn = UCase(strIn.Trim(" "c))
        If strIn = "*" Or strIn.Length = 0 Then
            Return ""
        Else
            If strIn.Length = 17 Then
                Return strIn
            Else
                If Right(strIn, 1) = "*" Then
                    Return strIn
                Else
                    Return strIn & "*"
                End If
            End If
        End If
    End Function

    Private Sub FillForm()
        trEditUser.Visible = False
        trSearchResult.Visible = False
        Search(True, True, True, True)
    End Sub

    Private Sub FillDataGrid()
        Dim strSort As String = "Fahrgestellnummer von"
        If Not ViewState("ResultSort") Is Nothing Then
            strSort = ViewState("ResultSort").ToString
        End If
        FillDataGrid(strSort)
    End Sub

    Private Sub FillDataGrid(ByVal strSort As String)
        trSearchResult.Visible = True
        Dim dvZV As DataView

        Try
            If Not m_context.Cache("mySchluesselTuetenVorgabenListView") Is Nothing Then
                dvZV = CType(m_context.Cache("mySchluesselTuetenVorgabenListView"), DataView)
            Else
                objVorgaben = New AppSIXT.SIXT_SchluesselTuetenVorgaben(m_User, m_App, Session("AppID").ToString, Session.SessionID.ToString, "")
                objVorgaben.Customer = m_User.KUNNR
                objVorgaben.Show(Session("AppID").ToString, Session.SessionID.ToString, Me)
                dvZV = objVorgaben.Vorgaben.DefaultView
                m_context.Cache.Insert("mySchluesselTuetenVorgabenListView", dvZV, Nothing, DateTime.Now.AddMinutes(20), TimeSpan.Zero)
            End If
            dvZV.Sort = strSort
            If dvZV.Count > dgSearchResult.PageSize Then
                dgSearchResult.PagerStyle.Visible = True
            Else
                dgSearchResult.PagerStyle.Visible = False
            End If

            dgSearchResult.Visible = True
            dgSearchResult.DataSource = dvZV
            dgSearchResult.DataBind()

            Dim str As String
            If dvZV.Count = 1 Then
                str = "Es wurde 1 Eintrag gefunden."
            Else
                str = String.Format("Es wurden {0} Einträge gefunden.", dvZV.Count)
            End If
            lblMessage.Text = str
        Catch ex As Exception
            m_App.WriteErrorText(1, m_User.UserName, "SchluesselTuetenVorgaben", "FillDataGrid", ex.ToString)

            dgSearchResult.Visible = False
            lblMessage.Text = ex.Message
        End Try
    End Sub

    Private Function FillEdit(ByVal intZVId As Integer) As Boolean
        SearchMode(False)
        objVorgaben = New AppSIXT.SIXT_SchluesselTuetenVorgaben(m_User, m_App, Session("AppID").ToString, Session.SessionID.ToString, "")
        objVorgaben.Customer = m_User.KUNNR
        objVorgaben.Show(Session("AppID").ToString, Session.SessionID.ToString, Me)
        Dim rowsTemp As DataRow() = objVorgaben.Vorgaben.Select("VorgabeID=" & intZVId.ToString)

        txtZVID.Text = CStr(rowsTemp(0)("VorgabeID"))
        txtCHASSIS_NUM.Text = CStr(rowsTemp(0)("Fahrgestellnummer von"))
        txtCHASSIS_NUM_BIS.Text = CStr(rowsTemp(0)("Fahrgestellnummer bis"))
        txtModell.Text = CStr(rowsTemp(0)("Modell"))
        txtHersteller.Text = CStr(rowsTemp(0)("Hersteller"))
        txtERSSCHLUESSEL.Text = CStr(rowsTemp(0)("Ersatzschlüssel"))
        txtCARPASS.Text = CStr(rowsTemp(0)("Carpass"))
        txtRADIOCODEKARTE.Text = CStr(rowsTemp(0)("Radio Codekarte"))
        txtNAVICD.Text = CStr(rowsTemp(0)("CD-Navigationssystem"))
        txtCHIPKARTE.Text = CStr(rowsTemp(0)("Chipkarte"))
        txtCOCBESCH.Text = CStr(rowsTemp(0)("COC-Papier"))
        txtNAVICODEKARTE.Text = CStr(rowsTemp(0)("Navigationssystem Codekarte"))
        txtWFSCODEKARTE.Text = CStr(rowsTemp(0)("Codekarte Wegfahrsperre"))
        txtSH_ERS_FB.Text = CStr(rowsTemp(0)("Ersatzfernbedienung Standheizung"))
        txtPRUEFBUCH_LKW.Text = CStr(rowsTemp(0)("Prüfbuch bei LKW"))

        txtCHASSIS_NUM_Alt.Text = CStr(rowsTemp(0)("Fahrgestellnummer von"))
        txtCHASSIS_NUM_BIS_Alt.Text = CStr(rowsTemp(0)("Fahrgestellnummer bis"))
        txtModell_alt.Text = CStr(rowsTemp(0)("Modell"))
        txtHersteller_alt.Text = CStr(rowsTemp(0)("Hersteller"))
        txtERSSCHLUESSEL_Alt.Text = CStr(rowsTemp(0)("Ersatzschlüssel"))
        txtCARPASS_Alt.Text = CStr(rowsTemp(0)("Carpass"))
        txtRADIOCODEKARTE_Alt.Text = CStr(rowsTemp(0)("Radio Codekarte"))
        txtNAVICD_Alt.Text = CStr(rowsTemp(0)("CD-Navigationssystem"))
        txtCHIPKARTE_Alt.Text = CStr(rowsTemp(0)("Chipkarte"))
        txtCOCBESCH_Alt.Text = CStr(rowsTemp(0)("COC-Papier"))
        txtNAVICODEKARTE_Alt.Text = CStr(rowsTemp(0)("Navigationssystem Codekarte"))
        txtWFSCODEKARTE_Alt.Text = CStr(rowsTemp(0)("Codekarte Wegfahrsperre"))
        txtSH_ERS_FB_Alt.Text = CStr(rowsTemp(0)("Ersatzfernbedienung Standheizung"))
        txtPRUEFBUCH_LKW_Alt.Text = CStr(rowsTemp(0)("Prüfbuch bei LKW"))

        Return True
    End Function

    Private Sub SetDdl(ByVal ddl As DropDownList, ByVal strValue As String)
        If Not ddl.SelectedItem Is Nothing Then ddl.SelectedItem.Selected = False
        Dim _li As ListItem = ddl.Items.FindByValue(strValue)
        If Not _li Is Nothing Then _li.Selected = True
    End Sub

    Private Sub ClearEdit()
        txtZVID.Text = "-1"
        txtCHASSIS_NUM.Text = ""
        txtModell.Text = ""
        txtHersteller.Text = ""
        txtCHASSIS_NUM_BIS.Text = ""
        txtERSSCHLUESSEL.Text = "0"
        txtCARPASS.Text = "0"
        txtRADIOCODEKARTE.Text = "0"
        txtNAVICD.Text = "0"
        txtCHIPKARTE.Text = "0"
        txtCOCBESCH.Text = "0"
        txtNAVICODEKARTE.Text = "0"
        txtWFSCODEKARTE.Text = "0"
        txtSH_ERS_FB.Text = "0"
        txtPRUEFBUCH_LKW.Text = "0"
        LockEdit(False)
    End Sub

    Private Sub LockEdit(ByVal blnLock As Boolean)
        Dim strBackColor As String = "White"
        If blnLock Then
            strBackColor = "LightGray"
        End If
        txtZVID.Enabled = Not blnLock
        txtZVID.BackColor = System.Drawing.Color.FromName(strBackColor)
        txtCHASSIS_NUM.Enabled = Not blnLock
        txtCHASSIS_NUM.BackColor = System.Drawing.Color.FromName(strBackColor)
        txtCHASSIS_NUM_BIS.Enabled = Not blnLock
        txtCHASSIS_NUM_BIS.BackColor = System.Drawing.Color.FromName(strBackColor)
        txtModell.Enabled = Not blnLock
        txtModell.BackColor = System.Drawing.Color.FromName(strBackColor)
        txtHersteller.Enabled = Not blnLock
        txtHersteller.BackColor = System.Drawing.Color.FromName(strBackColor)
        txtERSSCHLUESSEL.Enabled = Not blnLock
        txtERSSCHLUESSEL.BackColor = System.Drawing.Color.FromName(strBackColor)
        txtCARPASS.Enabled = Not blnLock
        txtCARPASS.BackColor = System.Drawing.Color.FromName(strBackColor)
        txtRADIOCODEKARTE.Enabled = Not blnLock
        txtRADIOCODEKARTE.BackColor = System.Drawing.Color.FromName(strBackColor)
        txtNAVICD.Enabled = Not blnLock
        txtNAVICD.BackColor = System.Drawing.Color.FromName(strBackColor)
        txtCHIPKARTE.Enabled = Not blnLock
        txtCHIPKARTE.BackColor = System.Drawing.Color.FromName(strBackColor)
        txtCOCBESCH.Enabled = Not blnLock
        txtCOCBESCH.BackColor = System.Drawing.Color.FromName(strBackColor)
        txtNAVICODEKARTE.Enabled = Not blnLock
        txtNAVICODEKARTE.BackColor = System.Drawing.Color.FromName(strBackColor)
        txtWFSCODEKARTE.Enabled = Not blnLock
        txtWFSCODEKARTE.BackColor = System.Drawing.Color.FromName(strBackColor)
        txtSH_ERS_FB.Enabled = Not blnLock
        txtSH_ERS_FB.BackColor = System.Drawing.Color.FromName(strBackColor)
        txtPRUEFBUCH_LKW.Enabled = Not blnLock
        txtPRUEFBUCH_LKW.BackColor = System.Drawing.Color.FromName(strBackColor)
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
            lblMessage.Text = "Möchten Sie die Vorbelegung wirklich löschen?"
            lbtnDelete.Enabled = True
        End If
        LockEdit(True)
        lbtnCancel.Text = "Abbrechen"
        lbtnSave.Visible = False
        lbtnDelete.Visible = True
    End Sub

    Private Sub SearchMode(Optional ByVal blnSearchMode As Boolean = True)
        trEditUser.Visible = Not blnSearchMode
        trSearchResult.Visible = blnSearchMode
        trSearchSpacerTop.Visible = blnSearchMode
        lbtnSave.Visible = Not blnSearchMode
        lbtnCancel.Visible = Not blnSearchMode
        lbtnNew.Visible = blnSearchMode
    End Sub

    Private Sub Search(Optional ByVal blnRefillDataGrid As Boolean = False, Optional ByVal blnResetSelectedIndex As Boolean = False, Optional ByVal blnResetPageIndex As Boolean = False, Optional ByVal blnClearCache As Boolean = False)
        ClearEdit()
        If blnClearCache Then
            m_context.Cache.Remove("mySchluesselTuetenVorgabenListView")
        End If
        If blnResetSelectedIndex Then dgSearchResult.SelectedIndex = -1
        If blnResetPageIndex Then dgSearchResult.CurrentPageIndex = 0
        SearchMode()
        If blnRefillDataGrid Then FillDataGrid()
    End Sub

    Private Sub Log(ByVal strIdentification As String, ByVal strDescription As String, ByVal tblParameters As DataTable, ByVal intIDSAP As Int32, Optional ByVal strCategory As String = "APP")
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
        logApp.WriteStandardDataAccessSAP(intIDSAP)
    End Sub

    Private Function SetOldLogParameters(ByVal intAppId As Int32, ByVal tblPar As DataTable) As DataTable
        Try
            If tblPar Is Nothing Then
                tblPar = CreateLogTableStructure()
            End If
            With tblPar
                .Rows.Add(.NewRow)
                .Rows(.Rows.Count - 1)("Status") = "Alt"
                .Rows(.Rows.Count - 1)("Fahrgestellnummer von") = txtCHASSIS_NUM_Alt.Text
                .Rows(.Rows.Count - 1)("Fahrgestellnummer bis") = txtCHASSIS_NUM_BIS_Alt.Text
                .Rows(.Rows.Count - 1)("Hersteller") = txtHersteller_alt.Text
                .Rows(.Rows.Count - 1)("Modell") = txtModell_alt.Text
                .Rows(.Rows.Count - 1)("Ersatzschlüssel") = CInt(txtERSSCHLUESSEL_Alt.Text)
                .Rows(.Rows.Count - 1)("Carpass") = CInt(txtCARPASS_Alt.Text)
                .Rows(.Rows.Count - 1)("Radio Codekarte") = CInt(txtRADIOCODEKARTE_Alt.Text)
                .Rows(.Rows.Count - 1)("CD-Navigationssystem") = CInt(txtNAVICD_Alt.Text)
                .Rows(.Rows.Count - 1)("Chipkarte") = CInt(txtCHIPKARTE_Alt.Text)
                .Rows(.Rows.Count - 1)("COC-Papier") = CInt(txtCOCBESCH_Alt.Text)
                .Rows(.Rows.Count - 1)("Navigationssystem Codekarte") = CInt(txtNAVICODEKARTE_Alt.Text)
                .Rows(.Rows.Count - 1)("Codekarte Wegfahrsperre") = CInt(txtWFSCODEKARTE_Alt.Text)
                .Rows(.Rows.Count - 1)("Ersatzfernbedienung Standheizung") = CInt(txtSH_ERS_FB_Alt.Text)
                .Rows(.Rows.Count - 1)("Prüfbuch bei LKW") = CInt(txtPRUEFBUCH_LKW_Alt.Text)
            End With
            Return tblPar
        Catch ex As Exception
            m_App.WriteErrorText(1, m_User.UserName, "SchluesselTuetenVorgaben", "SetOldLogParameters", ex.ToString)

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
                .Rows(.Rows.Count - 1)("Fahrgestellnummer von") = txtCHASSIS_NUM.Text
                .Rows(.Rows.Count - 1)("Fahrgestellnummer bis") = txtCHASSIS_NUM_BIS.Text
                .Rows(.Rows.Count - 1)("Hersteller") = txtHersteller.Text
                .Rows(.Rows.Count - 1)("Modell") = txtModell.Text
                .Rows(.Rows.Count - 1)("Ersatzschlüssel") = CInt(txtERSSCHLUESSEL.Text)
                .Rows(.Rows.Count - 1)("Carpass") = CInt(txtCARPASS.Text)
                .Rows(.Rows.Count - 1)("Radio Codekarte") = CInt(txtRADIOCODEKARTE.Text)
                .Rows(.Rows.Count - 1)("CD-Navigationssystem") = CInt(txtNAVICD.Text)
                .Rows(.Rows.Count - 1)("Chipkarte") = CInt(txtCHIPKARTE.Text)
                .Rows(.Rows.Count - 1)("COC-Papier") = CInt(txtCOCBESCH.Text)
                .Rows(.Rows.Count - 1)("Navigationssystem Codekarte") = CInt(txtNAVICODEKARTE.Text)
                .Rows(.Rows.Count - 1)("Codekarte Wegfahrsperre") = CInt(txtWFSCODEKARTE.Text)
                .Rows(.Rows.Count - 1)("Ersatzfernbedienung Standheizung") = CInt(txtSH_ERS_FB.Text)
                .Rows(.Rows.Count - 1)("Prüfbuch bei LKW") = CInt(txtPRUEFBUCH_LKW.Text)
            End With
            Return tblPar
        Catch ex As Exception
            m_App.WriteErrorText(1, m_User.UserName, "SchluesselTuetenVorgaben", "SetNewLogParameters", ex.ToString)

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
            .Columns.Add("Fahrgestellnummer von", System.Type.GetType("System.String"))
            .Columns.Add("Fahrgestellnummer bis", System.Type.GetType("System.String"))
            .Columns.Add("Hersteller", System.Type.GetType("System.String"))
            .Columns.Add("Modell", System.Type.GetType("System.String"))
            .Columns.Add("Ersatzschlüssel", System.Type.GetType("System.Int32"))
            .Columns.Add("Carpass", System.Type.GetType("System.Int32"))
            .Columns.Add("Radio Codekarte", System.Type.GetType("System.Int32"))
            .Columns.Add("CD-Navigationssystem", System.Type.GetType("System.Int32"))
            .Columns.Add("Chipkarte", System.Type.GetType("System.Int32"))
            .Columns.Add("COC-Papier", System.Type.GetType("System.Int32"))
            .Columns.Add("Navigationssystem Codekarte", System.Type.GetType("System.Int32"))
            .Columns.Add("Codekarte Wegfahrsperre", System.Type.GetType("System.Int32"))
            .Columns.Add("Ersatzfernbedienung Standheizung", System.Type.GetType("System.Int32"))
            .Columns.Add("Prüfbuch bei LKW", System.Type.GetType("System.Int32"))
        End With
        Return tblPar
    End Function
#End Region

#Region " Events "
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        m_User = GetUser(Me)
        ucHeader.InitUser(m_User)
        ucStyles.TitleText = "Schlüsseltüten - Vorgaben"
        FormAuth(Me, m_User)

        GetAppIDFromQueryString(Me)

        lblError.Text = ""
        lblMessage.Text = ""

        Try
            m_App = New Base.Kernel.Security.App(m_User)

            If Not IsPostBack Then
                FillForm()
            Else
                txtCHASSIS_NUM.Text = CheckChassisNo(txtCHASSIS_NUM.Text)
                txtCHASSIS_NUM_BIS.Text = CheckChassisNo(txtCHASSIS_NUM_BIS.Text)
            End If
        Catch ex As Exception
            m_App.WriteErrorText(1, m_User.UserName, "SchluesselTuetenVorgaben", "Page_Load", ex.ToString)
            lblError.Text = ex.ToString
        End Try

    End Sub

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        SetEndASPXAccess(Me)
    End Sub

    Private Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Unload
        SetEndASPXAccess(Me)
    End Sub

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

    Private Sub lbtnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lbtnNew.Click
        SearchMode(False)
        ClearEdit()
    End Sub

    Private Sub lbtnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lbtnSave.Click
        Dim tblLogParameter As DataTable = Nothing
        Try
            If (txtCHASSIS_NUM.Text.Trim(" "c).Length = 0) Or _
                (txtCHASSIS_NUM_BIS.Text.Trim(" "c).Length = 0) Then
                lblError.Text = "Bitte geben Sie Start- und Endwert ein."
                Exit Sub
            End If

            If (Not IsNumeric(txtERSSCHLUESSEL.Text)) Or _
                (Not IsNumeric(txtCARPASS.Text)) Or _
                (Not IsNumeric(txtRADIOCODEKARTE.Text)) Or _
                (Not IsNumeric(txtNAVICD.Text)) Or _
                (Not IsNumeric(txtCHIPKARTE.Text)) Or _
                (Not IsNumeric(txtCOCBESCH.Text)) Or _
                (Not IsNumeric(txtNAVICODEKARTE.Text)) Or _
                (Not IsNumeric(txtWFSCODEKARTE.Text)) Or _
                (Not IsNumeric(txtSH_ERS_FB.Text)) Or _
                (Not IsNumeric(txtPRUEFBUCH_LKW.Text)) Then
                lblError.Text = "Bitte geben Sie ganzzahlige positive Werte ein."
                Exit Sub
            End If

            Dim strLogMsg As String
            objVorgaben = New AppSIXT.SIXT_SchluesselTuetenVorgaben(m_User, m_App, Session("AppID").ToString, Session.SessionID.ToString, "")
            objVorgaben.Customer = m_User.KUNNR
            objVorgaben.Show(Session("AppID").ToString, Session.SessionID.ToString, Me)
            objVorgaben.Delete = False
            objVorgaben.VorgabeID = CInt(txtZVID.Text)
            If objVorgaben.VorgabeID = -1 Then
                strLogMsg = "Schlüsseltüten - Vorgabe erstellt"
                Dim rowAdd As DataRow
                rowAdd = objVorgaben.Vorgaben.NewRow
                rowAdd("VorgabeID") = -1
                rowAdd("Fahrgestellnummer von") = txtCHASSIS_NUM.Text
                rowAdd("Fahrgestellnummer bis") = txtCHASSIS_NUM_BIS.Text
                rowAdd("Hersteller") = txtHersteller.Text
                rowAdd("Modell") = txtModell.Text
                rowAdd("Ersatzschlüssel") = CInt(txtERSSCHLUESSEL.Text)
                rowAdd("Carpass") = CInt(txtCARPASS.Text)
                rowAdd("Radio Codekarte") = CInt(txtRADIOCODEKARTE.Text)
                rowAdd("CD-Navigationssystem") = CInt(txtNAVICD.Text)
                rowAdd("Chipkarte") = CInt(txtCHIPKARTE.Text)
                rowAdd("COC-Papier") = CInt(txtCOCBESCH.Text)
                rowAdd("Navigationssystem Codekarte") = CInt(txtNAVICODEKARTE.Text)
                rowAdd("Codekarte Wegfahrsperre") = CInt(txtWFSCODEKARTE.Text)
                rowAdd("Ersatzfernbedienung Standheizung") = CInt(txtSH_ERS_FB.Text)
                rowAdd("Prüfbuch bei LKW") = CInt(txtPRUEFBUCH_LKW.Text)
                objVorgaben.Vorgaben.Rows.Add(rowAdd)
            Else
                strLogMsg = "Schlüsseltüten - Vorgabe geändert"
                tblLogParameter = SetOldLogParameters(objVorgaben.VorgabeID, tblLogParameter)
                objVorgaben.Vorgaben.AcceptChanges()
                Dim rowChange As DataRow() = objVorgaben.Vorgaben.Select("VorgabeID = " & objVorgaben.VorgabeID.ToString)
                rowChange(0)("Fahrgestellnummer von") = txtCHASSIS_NUM.Text
                rowChange(0)("Fahrgestellnummer bis") = txtCHASSIS_NUM_BIS.Text
                rowChange(0)("Hersteller") = txtHersteller.Text
                rowChange(0)("Modell") = txtModell.Text
                rowChange(0)("Ersatzschlüssel") = CInt(txtERSSCHLUESSEL.Text)
                rowChange(0)("Carpass") = CInt(txtCARPASS.Text)
                rowChange(0)("Radio Codekarte") = CInt(txtRADIOCODEKARTE.Text)
                rowChange(0)("CD-Navigationssystem") = CInt(txtNAVICD.Text)
                rowChange(0)("Chipkarte") = CInt(txtCHIPKARTE.Text)
                rowChange(0)("COC-Papier") = CInt(txtCOCBESCH.Text)
                rowChange(0)("Navigationssystem Codekarte") = CInt(txtNAVICODEKARTE.Text)
                rowChange(0)("Codekarte Wegfahrsperre") = CInt(txtWFSCODEKARTE.Text)
                rowChange(0)("Ersatzfernbedienung Standheizung") = CInt(txtSH_ERS_FB.Text)
                rowChange(0)("Prüfbuch bei LKW") = CInt(txtPRUEFBUCH_LKW.Text)
                objVorgaben.Vorgaben.AcceptChanges()
            End If
            objVorgaben.Change(Session("AppID").ToString, Session.SessionID.ToString, Me)
            FillForm()

            tblLogParameter = SetNewLogParameters(tblLogParameter)
            Log(objVorgaben.VorgabeID.ToString, strLogMsg, tblLogParameter, objVorgaben.IDSAP)
            Search(True, True, , True)
            lblMessage.Text = "Die Änderungen wurden gespeichert."
        Catch ex As Exception
            m_App.WriteErrorText(1, m_User.UserName, "SchluesselTuetenVorgaben", "lbtnSave_Click", ex.ToString)

            lblError.Text = ex.Message
            If Not ex.InnerException Is Nothing Then
                lblError.Text &= ": " & ex.InnerException.Message
            End If
            Log(txtZVID.Text, lblError.Text, tblLogParameter, objVorgaben.IDSAP, "ERR")
        End Try
    End Sub

    Private Sub lbtnDelete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lbtnDelete.Click
        Dim tblLogParameter As DataTable = Nothing
        Try
            objVorgaben = New AppSIXT.SIXT_SchluesselTuetenVorgaben(m_User, m_App, Session("AppID").ToString, Session.SessionID.ToString, "")
            objVorgaben.Customer = m_User.KUNNR
            objVorgaben.Show(Session("AppID").ToString, Session.SessionID.ToString, Me)
            objVorgaben.Delete = True
            objVorgaben.VorgabeID = CInt(txtZVID.Text)
            tblLogParameter = SetOldLogParameters(objVorgaben.VorgabeID, tblLogParameter)
            objVorgaben.Change(Session("AppID").ToString, Session.SessionID.ToString, Me)
            FillForm()

            Log(objVorgaben.VorgabeID.ToString, "Schlüsseltüten - Vorgabe gelöscht", tblLogParameter, objVorgaben.IDSAP)
            Search(True, True, True, True)
            lblMessage.Text = "Die Vorbelegung wurde gelöscht."
        Catch ex As Exception
            m_App.WriteErrorText(1, m_User.UserName, "SchluesselTuetenVorgaben", "lbtnDelete_Click", ex.ToString)

            lblError.Text = ex.Message
            If Not ex.InnerException Is Nothing Then
                lblError.Text &= ": " & ex.InnerException.Message
            End If
            Log(txtZVID.Text, lblError.Text, tblLogParameter, objVorgaben.IDSAP, "ERR")
        End Try
    End Sub
#End Region
End Class

' ************************************************
' $History: SchluesselTuetenVorgaben.aspx.vb $
' 
' *****************  Version 3  *****************
' User: Rudolpho     Date: 16.02.10   Time: 11:40
' Updated in $/CKAG/Applications/appsixt/Forms
' ITA: 2918
' 
' *****************  Version 2  *****************
' User: Rudolpho     Date: 4.05.09    Time: 10:16
' Updated in $/CKAG/Applications/appsixt/Forms
' ITA:2837
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 8.04.08    Time: 13:37
' Created in $/CKAG/Applications/appsixt/Forms
' 
' *****************  Version 6  *****************
' User: Uha          Date: 2.07.07    Time: 14:10
' Updated in $/CKG/Applications/AppSIXT/AppSIXTWeb/Forms
' Logging der Laufzeiten der ASPX-Seiten eingeführt
' 
' *****************  Version 5  *****************
' User: Uha          Date: 22.05.07   Time: 11:27
' Updated in $/CKG/Applications/AppSIXT/AppSIXTWeb/Forms
' Nacharbeiten + Bereinigungen
' 
' *****************  Version 4  *****************
' User: Uha          Date: 8.03.07    Time: 13:15
' Updated in $/CKG/Applications/AppSIXT/AppSIXTWeb/Forms
' 
' ************************************************
