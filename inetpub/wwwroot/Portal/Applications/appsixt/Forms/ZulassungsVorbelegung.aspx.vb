Imports CKG.Base.Business
Imports CKG.Base.Kernel
Imports CKG.Base.Kernel.Common.Common
Imports CKG.Portal.PageElements

Public Class ZulassungsVorbelegung
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
    Protected WithEvents txtVonFZN1_3 As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtVonFZN4_17 As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtBisDatum As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtAbDatum As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtBisFZN1_3 As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtBisFZN4_17 As System.Web.UI.WebControls.TextBox
    Protected WithEvents ddlHalter_SAPNr As System.Web.UI.WebControls.DropDownList
    Protected WithEvents ddlVersicherer_SAPNr As System.Web.UI.WebControls.DropDownList
    Protected WithEvents trSearchSpacer As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents valVonFZN1_3 As System.Web.UI.WebControls.RequiredFieldValidator
    Protected WithEvents valVonFZN4_17 As System.Web.UI.WebControls.RequiredFieldValidator
    Protected WithEvents valBisFZN1_3 As System.Web.UI.WebControls.RequiredFieldValidator
    Protected WithEvents valBisFZN4_17 As System.Web.UI.WebControls.RequiredFieldValidator
    Protected WithEvents valAbDatum As System.Web.UI.WebControls.RequiredFieldValidator
    Protected WithEvents valBisDatum As System.Web.UI.WebControls.RequiredFieldValidator
    Protected WithEvents txtModell As System.Web.UI.WebControls.TextBox
    Protected WithEvents chkLimo As System.Web.UI.WebControls.RadioButton
    Protected WithEvents chkKennzeichen2zeilig As System.Web.UI.WebControls.RadioButton
    Protected WithEvents chkKeineAuswahl As System.Web.UI.WebControls.RadioButton
    Protected WithEvents txtHersteller As System.Web.UI.WebControls.TextBox
    Protected WithEvents dgSearchResult As System.Web.UI.WebControls.DataGrid
    Protected WithEvents ddlFilterHersteller As System.Web.UI.WebControls.DropDownList
    Protected WithEvents trSearchSpacerTop As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents trSearch As System.Web.UI.HtmlControls.HtmlTableRow
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
#End Region

#Region " Data and Function "
    Private Sub FillForm()
        trEditUser.Visible = False
        trSearchResult.Visible = False
        Dim cn As New SqlClient.SqlConnection(m_User.App.Connectionstring)
        FillHalter(cn)
        FillVersicherer(cn)
        Search(True, True, True, True)
    End Sub

    Private Sub FillHalter(ByVal cn As SqlClient.SqlConnection)
        Dim hl As New Admin.HalterList(cn, m_User.KUNNR)
        hl.DefaultView.Sort = "Name1"
        With ddlHalter_SAPNr
            .DataSource = hl.DefaultView
            .DataValueField = "SAP-Nr"
            .DataTextField = "Name1"
            .DataBind()
        End With
    End Sub

    Private Sub FillVersicherer(ByVal cn As SqlClient.SqlConnection)
        'Dim vl As New VersichererList(cn)  '§§§ JVE 19.10.2006
        Dim vl As New Admin.VersichererList(cn, m_User.KUNNR)

        vl.DefaultView.Sort = "Name1"
        With ddlVersicherer_SAPNr
            .DataSource = vl.DefaultView
            .DataValueField = "SAP-Nr"
            .DataTextField = "Name1"
            .DataBind()
        End With
    End Sub

    Private Sub FillHersteller(ByVal dt As DataTable)
        With ddlFilterHersteller
            If Not .Items Is Nothing Then
                .Items.Clear()
            End If
            dt.DefaultView.Sort = "HstText"
            .DataSource = dt.DefaultView
            .DataTextField = "HstText"
            .DataValueField = "HstValue"
            .DataBind()
            If Not .SelectedItem Is Nothing Then
                .SelectedItem.Selected = False
            End If
            Dim strSelected As String = "%"
            If Not ViewState("Hersteller") Is Nothing Then
                If Not .Items.FindByValue(CStr(ViewState("Hersteller"))) Is Nothing Then
                    strSelected = CStr(ViewState("Hersteller"))
                End If
            End If
            Dim _li As ListItem
            For Each _li In .Items
                If _li.Value = strSelected Then
                    _li.Selected = True
                    Exit For
                End If
            Next
        End With
    End Sub

    Private Sub FillDataGrid()
        Dim strSort As String = "VonFZN1_3"
        If Not ViewState("ResultSort") Is Nothing Then
            strSort = ViewState("ResultSort").ToString
        End If
        FillDataGrid(strSort)
    End Sub
    Private Sub FillDataGrid(ByVal strSort As String)
        trSearchResult.Visible = True
        Dim dvZV As DataView

        '§§§ JVE 19.10.2006: Kein Caching!!
        'If Not m_context.Cache("myZulassungsVorbelegungListView") Is Nothing Then
        '    dvZV = CType(m_context.Cache("myZulassungsVorbelegungListView"), DataView)
        'Else
        Dim dtZV As Admin.ZulassungsVorbelegungList
        Dim cn As New SqlClient.SqlConnection(m_User.App.Connectionstring)
        cn.Open()

        dtZV = New Admin.ZulassungsVorbelegungList(cn, m_User.KUNNR)
        dvZV = dtZV.DefaultView
        'm_context.Cache.Insert("myZulassungsVorbelegungListView", dvZV, Nothing, DateTime.Now.AddMinutes(20), TimeSpan.Zero)
        FillHersteller(dtZV.HerstellerList)
        cn.Close()
        'End If
        dvZV.Sort = strSort
        If Not ddlFilterHersteller.SelectedItem Is Nothing Then
            dvZV.RowFilter = "Hersteller LIKE '" & ddlFilterHersteller.SelectedItem.Value & "'"
        End If
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
            Dim _ZV As New Base.Business.ZulassungsVorbelegung(intZVId, cn)
            txtZVID.Text = _ZV.ZVID.ToString
            txtVonFZN1_3.Text = _ZV.VonFZN1_3
            txtVonFZN4_17.Text = _ZV.VonFZN4_17
            txtBisFZN1_3.Text = _ZV.BisFZN1_3
            txtBisFZN4_17.Text = _ZV.BisFZN4_17
            SetDdl(ddlHalter_SAPNr, _ZV.Halter_SAPNr)
            SetDdl(ddlVersicherer_SAPNr, _ZV.Versicherer_SAPNr)
            txtAbDatum.Text = _ZV.AbDatum.ToShortDateString
            txtBisDatum.Text = _ZV.BisDatum.ToShortDateString
            txtModell.Text = _ZV.Modell
            txtHersteller.Text = _ZV.Hersteller
            chkKennzeichen2zeilig.Checked = _ZV.Kennzeichen2zeilig
            chkLimo.Checked = _ZV.Limo
            If txtVonFZN1_3.Text = "_VM" And txtVonFZN4_17.Text = "VERMITT_FHRZG" Then
                txtVonFZN1_3.Enabled = False
                txtVonFZN4_17.Enabled = False
                txtBisFZN1_3.Enabled = False
                txtBisFZN4_17.Enabled = False
                txtModell.Enabled = False
                chkKennzeichen2zeilig.Enabled = False
                chkLimo.Enabled = False
                chkKeineAuswahl.Enabled = False
            End If
            Return True
        Finally
            cn.Close()
            cn.Dispose()
        End Try
    End Function

    Private Sub SetDdl(ByVal ddl As DropDownList, ByVal strValue As String)
        If Not ddl.SelectedItem Is Nothing Then ddl.SelectedItem.Selected = False
        Dim _li As ListItem = ddl.Items.FindByValue(strValue)
        If Not _li Is Nothing Then _li.Selected = True
    End Sub

    Private Sub ClearEdit()
        txtZVID.Text = "-1"
        txtVonFZN1_3.Text = ""
        txtVonFZN4_17.Text = ""
        txtBisFZN1_3.Text = ""
        txtBisFZN4_17.Text = ""
        SetDdl(ddlHalter_SAPNr, "")
        SetDdl(ddlVersicherer_SAPNr, "")
        txtAbDatum.Text = Today.ToShortDateString
        txtBisDatum.Text = "31.12.9999"
        txtModell.Text = ""
        txtHersteller.Text = "unbekannt"
        chkKennzeichen2zeilig.Checked = False
        chkLimo.Checked = False
        chkKeineAuswahl.Checked = True
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
        txtZVID.Enabled = Not blnLock
        txtZVID.BackColor = System.Drawing.Color.FromName(strBackColor)
        txtVonFZN1_3.Enabled = Not blnLock
        txtVonFZN1_3.BackColor = System.Drawing.Color.FromName(strBackColor)
        txtVonFZN4_17.Enabled = Not blnLock
        txtVonFZN4_17.BackColor = System.Drawing.Color.FromName(strBackColor)
        txtBisFZN1_3.Enabled = Not blnLock
        txtBisFZN1_3.BackColor = System.Drawing.Color.FromName(strBackColor)
        txtBisFZN4_17.Enabled = Not blnLock
        txtBisFZN4_17.BackColor = System.Drawing.Color.FromName(strBackColor)
        ddlHalter_SAPNr.Enabled = Not blnLock
        ddlHalter_SAPNr.BackColor = System.Drawing.Color.FromName(strBackColor)
        ddlVersicherer_SAPNr.Enabled = Not blnLock
        ddlVersicherer_SAPNr.BackColor = System.Drawing.Color.FromName(strBackColor)
        txtAbDatum.Enabled = Not blnLock
        txtAbDatum.BackColor = System.Drawing.Color.FromName(strBackColor)
        txtBisDatum.Enabled = Not blnLock
        txtBisDatum.BackColor = System.Drawing.Color.FromName(strBackColor)
        txtModell.Enabled = Not blnLock
        txtModell.BackColor = System.Drawing.Color.FromName(strBackColor)
        chkKennzeichen2zeilig.Enabled = Not blnLock
        chkKennzeichen2zeilig.BackColor = System.Drawing.Color.FromName(strBackColor)
        chkLimo.Enabled = Not blnLock
        chkLimo.BackColor = System.Drawing.Color.FromName(strBackColor)
        chkKeineAuswahl.Enabled = Not blnLock
        chkKeineAuswahl.BackColor = System.Drawing.Color.FromName(strBackColor)
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
        trSearch.Visible = blnSearchMode
        trSearchSpacerTop.Visible = blnSearchMode
        lbtnSave.Visible = Not blnSearchMode
        lbtnCancel.Visible = Not blnSearchMode
        lbtnNew.Visible = blnSearchMode
    End Sub

    Private Sub Search(Optional ByVal blnRefillDataGrid As Boolean = False, Optional ByVal blnResetSelectedIndex As Boolean = False, Optional ByVal blnResetPageIndex As Boolean = False, Optional ByVal blnClearCache As Boolean = False)
        ClearEdit()
        '§§§ JVE 19.10.2006: Kein Caching!!!
        'If blnClearCache Then
        '    m_context.Cache.Remove("myZulassungsVorbelegungListView")
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

    Private Function SetOldLogParameters(ByVal intZVId As Int32, ByVal tblPar As DataTable) As DataTable
        Dim cn As New SqlClient.SqlConnection
        Try
            cn.ConnectionString = m_User.App.Connectionstring
            cn.Open()
            Dim _ZV As New Base.Business.ZulassungsVorbelegung(intZVId, cn)

            If tblPar Is Nothing Then
                tblPar = CreateLogTableStructure()
            End If
            With tblPar
                .Rows.Add(.NewRow)
                .Rows(.Rows.Count - 1)("Status") = "Alt"
                .Rows(.Rows.Count - 1)("Von FN Ziff. 1-3") = _ZV.VonFZN1_3
                .Rows(.Rows.Count - 1)("Von FN Ziff. 4-17") = _ZV.VonFZN4_17
                .Rows(.Rows.Count - 1)("Bis FN Ziff. 1-3") = _ZV.BisFZN1_3
                .Rows(.Rows.Count - 1)("Bis FN Ziff. 4-17") = _ZV.BisFZN4_17
                .Rows(.Rows.Count - 1)("Halter") = _ZV.Halter_SAPNr
                .Rows(.Rows.Count - 1)("Versicherer") = _ZV.Versicherer_SAPNr
                .Rows(.Rows.Count - 1)("gültig ab Zul.-Datum") = _ZV.AbDatum.ToShortDateString
                .Rows(.Rows.Count - 1)("gültig bis Zul.-Datum") = _ZV.BisDatum.ToShortDateString
                .Rows(.Rows.Count - 1)("Modell") = _ZV.Modell
                .Rows(.Rows.Count - 1)("zweizeilig") = _ZV.Kennzeichen2zeilig
                .Rows(.Rows.Count - 1)("Limo") = _ZV.Limo
                .Rows(.Rows.Count - 1)("Hersteller") = _ZV.Hersteller
            End With
            Return tblPar
        Catch ex As Exception
            m_App.WriteErrorText(1, m_User.UserName, "ZulassungsVorbelegung", "SetOldLogParameters", ex.ToString)

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
            cn.Close()
            cn.Dispose()
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
                .Rows(.Rows.Count - 1)("Von FN Ziff. 1-3") = txtVonFZN1_3.Text
                .Rows(.Rows.Count - 1)("Von FN Ziff. 4-17") = txtVonFZN4_17.Text
                .Rows(.Rows.Count - 1)("Bis FN Ziff. 1-3") = txtBisFZN1_3.Text
                .Rows(.Rows.Count - 1)("Bis FN Ziff. 4-17") = txtBisFZN4_17.Text
                .Rows(.Rows.Count - 1)("Halter") = ddlHalter_SAPNr.SelectedItem.Value
                .Rows(.Rows.Count - 1)("Versicherer") = ddlVersicherer_SAPNr.SelectedItem.Value
                .Rows(.Rows.Count - 1)("gültig ab Zul.-Datum") = txtAbDatum.Text
                .Rows(.Rows.Count - 1)("gültig bis Zul.-Datum") = txtBisDatum.Text
                .Rows(.Rows.Count - 1)("Modell") = txtModell.Text
                .Rows(.Rows.Count - 1)("zweizeilig") = chkKennzeichen2zeilig.Checked
                .Rows(.Rows.Count - 1)("Limo") = chkLimo.Checked
                .Rows(.Rows.Count - 1)("Hersteller") = txtHersteller.Text
            End With
            Return tblPar
        Catch ex As Exception
            m_App.WriteErrorText(1, m_User.UserName, "ZulassungsVorbelegung", "SetNewLogParameters", ex.ToString)

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
            .Columns.Add("Von FN Ziff. 1-3", System.Type.GetType("System.String"))
            .Columns.Add("Von FN Ziff. 4-17", System.Type.GetType("System.String"))
            .Columns.Add("Bis FN Ziff. 1-3", System.Type.GetType("System.String"))
            .Columns.Add("Bis FN Ziff. 4-17", System.Type.GetType("System.String"))
            .Columns.Add("Halter", System.Type.GetType("System.String"))
            .Columns.Add("Versicherer", System.Type.GetType("System.String"))
            .Columns.Add("gültig ab Zul.-Datum", System.Type.GetType("System.String"))
            .Columns.Add("gültig bis Zul.-Datum", System.Type.GetType("System.String"))
            .Columns.Add("Modell", System.Type.GetType("System.String"))
            .Columns.Add("zweizeilig", System.Type.GetType("System.Boolean"))
            .Columns.Add("Limo", System.Type.GetType("System.Boolean"))
            .Columns.Add("Hersteller", System.Type.GetType("System.String"))
        End With
        Return tblPar
    End Function
#End Region

#Region " Events "
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        ' Hier Benutzercode zur Seiteninitialisierung einfügen
        m_User = GetUser(Me)
        ucHeader.InitUser(m_User)
        ucStyles.TitleText = "Zulassungsvorbelegung"
        FormAuth(Me, m_User)

        Try
            m_App = New Base.Kernel.Security.App(m_User)

            If Not IsPostBack Then
                lblError.Text = ""

                FillForm()
            End If
        Catch ex As Exception
            m_App.WriteErrorText(1, m_User.UserName, "ZulassungsVorbelegung", "Page_Load", ex.ToString)

            lblError.Text = ex.ToString
            lblError.Visible = True
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
        'Dim cn As New SqlClient.SqlConnection(m_User.App.Connectionstring)
        'cn.Open()
    End Sub

    Private Sub lbtnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lbtnSave.Click
        Dim tblLogParameter As DataTable = Nothing
        Dim cn As New SqlClient.SqlConnection
        Try
            If Not IsDate(txtAbDatum.Text) Then
                If Not IsStandardDate(txtAbDatum.Text) Then
                    If Not IsSAPDate(txtAbDatum.Text) Then
                        lblError.Text = "Bitte geben Sie ein Datum ein."
                        Exit Sub
                    End If
                End If
            End If
            Dim datAbDatum As Date = CDate(txtAbDatum.Text)
            If Not IsDate(txtBisDatum.Text) Then
                If Not IsStandardDate(txtBisDatum.Text) Then
                    If Not IsSAPDate(txtBisDatum.Text) Then
                        lblError.Text = "Bitte geben Sie ein Datum ein."
                        Exit Sub
                    End If
                End If
            End If
            Dim datBisDatum As Date = CDate(txtBisDatum.Text)
            cn.ConnectionString = m_User.App.Connectionstring
            cn.Open()
            Dim intZVId As Integer = CInt(txtZVID.Text)
            Dim strLogMsg As String = "Zulassungsvorbelegung anlegen"
            If Not (intZVId = -1) Then
                strLogMsg = "Zulassungsvorbelegung ändern"
                tblLogParameter = SetOldLogParameters(intZVId, tblLogParameter)
            End If

            Dim _ZV As New Base.Business.ZulassungsVorbelegung(intZVId, _
                                                txtVonFZN1_3.Text, _
                                                txtVonFZN4_17.Text, _
                                                txtBisFZN1_3.Text, _
                                                txtBisFZN4_17.Text, _
                                                ddlHalter_SAPNr.SelectedItem.Value, _
                                                ddlVersicherer_SAPNr.SelectedItem.Value, _
                                                datAbDatum, _
                                                datBisDatum, _
                                                txtModell.Text, _
                                                chkKennzeichen2zeilig.Checked, _
                                                chkLimo.Checked, _
                                                txtHersteller.Text, m_User.KUNNR)      '§§§ JVE 19.10.2006: Kunnr
            _ZV.Save(cn)

            tblLogParameter = SetNewLogParameters(tblLogParameter)
            Log(_ZV.ZVID.ToString, strLogMsg, tblLogParameter)
            Search(True, True, , True)
            lblMessage.Text = "Die Änderungen wurden gespeichert."
        Catch ex As Exception
            m_App.WriteErrorText(1, m_User.UserName, "ZulassungsVorbelegung", "lbtnSave_Click", ex.ToString)

            lblError.Text = ex.Message
            If Not ex.InnerException Is Nothing Then
                lblError.Text &= ": " & ex.InnerException.Message
            End If
            Log(txtZVID.Text, lblError.Text, tblLogParameter, "ERR")
        Finally
            cn.Close()
            cn.Dispose()
        End Try
    End Sub

    Private Sub lbtnDelete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lbtnDelete.Click
        Dim tblLogParameter As DataTable = Nothing
        Dim cn As New SqlClient.SqlConnection
        Try
            Dim _ZV As New Base.Business.ZulassungsVorbelegung(CInt(txtZVID.Text))
            cn.ConnectionString = m_User.App.Connectionstring
            cn.Open()
            tblLogParameter = SetOldLogParameters(_ZV.ZVID, tblLogParameter)
            _ZV.Delete(cn)
            Log(_ZV.ZVID.ToString, "Zulassungsvorbelegung löschen", tblLogParameter)
            Search(True, True, True, True)
            lblMessage.Text = "Die Vorbelegung wurde gelöscht."
        Catch ex As Exception
            m_App.WriteErrorText(1, m_User.UserName, "ZulassungsVorbelegung", "lbtnDelete_Click", ex.ToString)

            lblError.Text = ex.Message
            If Not ex.InnerException Is Nothing Then
                lblError.Text &= ": " & ex.InnerException.Message
            End If
            Log(txtZVID.Text, lblError.Text, tblLogParameter, "ERR")
        Finally
            cn.Close()
            cn.Dispose()
        End Try
    End Sub

    Private Sub ddlFilterHersteller_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ddlFilterHersteller.SelectedIndexChanged
        ViewState.Add("Hersteller", ddlFilterHersteller.SelectedItem.Value)
        Search(True, True, True)
    End Sub
#End Region
End Class

' ************************************************
' $History: ZulassungsVorbelegung.aspx.vb $
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
' *****************  Version 7  *****************
' User: Rudolpho     Date: 6.12.07    Time: 14:37
' Updated in $/CKG/Applications/AppSIXT/AppSIXTWeb/Forms
' ITA 1440
' 
' *****************  Version 6  *****************
' User: Rudolpho     Date: 6.12.07    Time: 14:36
' Updated in $/CKG/Applications/AppSIXT/AppSIXTWeb/Forms
' ITA: 1440
' 
' *****************  Version 5  *****************
' User: Uha          Date: 2.07.07    Time: 14:10
' Updated in $/CKG/Applications/AppSIXT/AppSIXTWeb/Forms
' Logging der Laufzeiten der ASPX-Seiten eingeführt
' 
' *****************  Version 4  *****************
' User: Uha          Date: 8.03.07    Time: 13:15
' Updated in $/CKG/Applications/AppSIXT/AppSIXTWeb/Forms
' 
' ************************************************
