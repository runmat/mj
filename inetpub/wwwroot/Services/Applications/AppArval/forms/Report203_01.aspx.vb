Imports CKG.Base.Kernel
Imports CKG
Imports CKG.Base.Kernel.Common.Common
Imports System.Text.RegularExpressions


Public Class Report203_01
    Inherits System.Web.UI.Page

    Private m_App As Base.Kernel.Security.App
    Private m_User As Base.Kernel.Security.User
    Private m_objTable As DataTable


    Protected WithEvents lblPageTitle As System.Web.UI.WebControls.Label
    Protected WithEvents cmdBack As System.Web.UI.WebControls.LinkButton
    Protected WithEvents lnkCreateExcel As System.Web.UI.WebControls.LinkButton
    Protected WithEvents lnkShowCSV As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lblDownloadTip As System.Web.UI.WebControls.Label
    Protected WithEvents lnkBack As System.Web.UI.WebControls.HyperLink
    Protected WithEvents ddlPageSize As System.Web.UI.WebControls.DropDownList
    Protected WithEvents lblInfo As System.Web.UI.WebControls.Label
    Protected WithEvents lblHidden As System.Web.UI.WebControls.Label
    Protected WithEvents ShowScript As System.Web.UI.HtmlControls.HtmlTableRow

    Private m_objExcel As DataTable
    Private legende As String
    Private csv As String
    Private schmal As String

#Region " Vom Web Form Designer generierter Code "

    'Dieser Aufruf ist für den Web Form-Designer erforderlich.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: Diese Methode ist für den Web Form-Designer erforderlich
        'Verwenden Sie nicht den Code-Editor zur Bearbeitung.
        InitializeComponent()
    End Sub

#End Region

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        m_User = GetUser(Me)
        FormAuth(Me, m_User)
        GridNavigation1.setGridElment(DataGrid1)

        If (Session("ResultTable") Is Nothing) Then
            Response.Redirect(Request.UrlReferrer.ToString)
        Else
            m_objTable = CType(Session("ResultTable"), DataTable)
        End If

        If (Session("ExcelTable") Is Nothing) Then
            m_objExcel = CType(Session("ResultTable"), DataTable)
        Else
            m_objExcel = CType(Session("ExcelTable"), DataTable)
        End If

        Try
            legende = Request.QueryString.Item("legende")
            If (legende = "Ja") Then
                lblInfo.Text = "<br>*(V)ollmacht, (H)andelsregistereintrag, (P)ersonalausweis, (G)ewerbeanmeldung, (E)inzugsermächtigung"
            End If
            If (legende = "Report201") Then
                lblInfo.Text = "<br>'X' = vorhanden"
            End If
            If (legende = "Report203") Then
                lblInfo.Text = "<br>*(V)ollmacht, (H)andelsregistereintrag, (P)ersonalausweis, (G)ewerbeanmeldung, (E)inzugsermächtigung, 'X' = vorhanden"
            End If

            BuildTextForLabelHead()
            'ActivateCmdBackButton()

            m_App = New Base.Kernel.Security.App(m_User)

            legende = Request.QueryString.Item("legende")
            If (legende = "Ja") Then
                lblInfo.Text = "<br>*(V)ollmacht, (H)andelsregistereintrag, (P)ersonalausweis, (G)ewerbeanmeldung, (E)inzugsermächtigung"
            End If

            If Not IsPostBack Then
                csv = Request.QueryString.Item("csv")
                If csv = "Ja" Then
                    lnkShowCSV.Visible = True
                    lblDownloadTip.Visible = True
                    lnkCreateExcel.Visible = False

                    Dim excelFactory As New Base.Kernel.DocumentGeneration.ExcelDocumentFactory()
                    Dim strFileName As String = Format(Now, "yyyyMMdd_HHmmss_") & m_User.UserName & ".csv"

                    excelFactory.CreateDocumentAndWriteToFilesystem(ConfigurationManager.AppSettings("ExcelPath") & strFileName, Me.m_objExcel, Me.Page, , , , , False)
                    lnkShowCSV.NavigateUrl = "/Portal/Temp/Excel/" & strFileName
                Else
                    lnkShowCSV.Visible = False
                    lblDownloadTip.Visible = False
                    'lnkCreateExcel.Visible = True
                End If

                If Not Session("ApplblInfoText") Is Nothing Then
                    If lblInfo.Text.Length = 0 Then
                        lblInfo.Text = CStr(Session("ApplblInfoText"))
                    Else
                        lblInfo.Text &= "<br>" & CStr(Session("ApplblInfoText"))
                    End If
                End If

                FillGrid(0)
            End If
        Catch ex As Exception
            lblError.Text = "Beim Laden der Seite ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
        End Try
    End Sub
    Private Sub GridNavigation1_PagerChanged(ByVal PageIndex As Integer) Handles GridNavigation1.PagerChanged
        FillGrid(PageIndex)
    End Sub

    Private Sub GridNavigation1_PageSizeChanged() Handles GridNavigation1.PageSizeChanged
        FillGrid(0)
    End Sub

    Private Sub DataGrid1_PageIndexChanged(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles DataGrid1.PageIndexChanged
        FillGrid(e.NewPageIndex)
    End Sub

    Private Sub FillGrid(ByVal intPageIndex As Int32, Optional ByVal strSort As String = "")
        If m_objTable.Rows.Count = 0 Then
            DataGrid1.Visible = False
            lblNoData.Visible = True
            lblNoData.Text = "Keine Daten zur Anzeige gefunden."
            ShowScript.Visible = False
        Else
            DataGrid1.Visible = True
            lblNoData.Visible = False

            Dim tmpDataView As New DataView()
            tmpDataView = m_objTable.DefaultView

            Dim intTempPageIndex As Int32 = intPageIndex
            Dim strTempSort As String = ""
            Dim strDirection As String = ""

            If strSort.Trim(" "c).Length > 0 Then
                intTempPageIndex = 0
                strTempSort = strSort.Trim(" "c)
                If (ViewState("Sort") Is Nothing) OrElse (ViewState("Sort").ToString = strTempSort) Then
                    If ViewState("Direction") Is Nothing Then
                        strDirection = "desc"
                    Else
                        strDirection = ViewState("Direction").ToString
                    End If
                Else
                    strDirection = "desc"
                End If

                If strDirection = "asc" Then
                    strDirection = "desc"
                Else
                    strDirection = "asc"
                End If

                ViewState("Sort") = strTempSort
                ViewState("Direction") = strDirection
            Else
                If Not ViewState("Sort") Is Nothing Then
                    strTempSort = ViewState("Sort").ToString
                    If ViewState("Direction") Is Nothing Then
                        strDirection = "asc"
                        ViewState("Direction") = strDirection
                    Else
                        strDirection = ViewState("Direction").ToString
                    End If
                End If
            End If

            If Not strTempSort.Length = 0 Then
                tmpDataView.Sort = strTempSort & " " & strDirection
            End If

            DataGrid1.CurrentPageIndex = intTempPageIndex
            DataGrid1.DataSource = tmpDataView

            DataGrid1.DataBind()

            If (Not Session("ShowOtherString") Is Nothing) AndAlso CStr(Session("ShowOtherString")).Length > 0 Then
                lblNoData.Text = CStr(Session("ShowOtherString"))
            Else
                lblNoData.Text = "Es wurden " & tmpDataView.Count.ToString & " Einträge zu """ & m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString & """ gefunden."
            End If
            'If (Not Session("BackLink") Is Nothing) AndAlso CStr(Session("BackLink")) = "HistoryBack" Then
            '    lnkKreditlimit.Text = "Zurück"
            '    lnkKreditlimit.NavigateUrl = "javascript:history.back()"
            'End If
            lblNoData.Visible = True

            If DataGrid1.PageCount > 1 Then
                DataGrid1.PagerStyle.CssClass = "PagerStyle"
                DataGrid1.DataBind()
                DataGrid1.PagerStyle.Visible = True
            Else
                DataGrid1.PagerStyle.Visible = False
            End If
        End If
    End Sub

    'Private Sub DataGrid1_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles DataGrid1.ItemDataBound
    '    Dim intItem As Int32

    '    For intItem = 0 To m_objTable.Columns.Count - 1
    '        If m_objTable.Columns(intItem).DataType Is System.Type.GetType("System.DateTime") Then
    '            If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Then
    '                e.Item.Cells(intItem).Text = DataBinder.Eval(e.Item.DataItem, m_objTable.Columns(intItem).ColumnName, "{0:dd.MM.yyyy}")
    '            End If
    '        End If
    '    Next
    'End Sub

    'Private Sub DataGrid1_SortCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs) Handles DataGrid1.SortCommand
    '    FillGrid(DataGrid1.CurrentPageIndex, e.SortExpression)
    'End Sub

    'Private Sub ddlPageSize_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ddlPageSize.SelectedIndexChanged
    '    DataGrid1.PageSize = CInt(ddlPageSize.SelectedItem.Value)
    '    FillGrid(0)
    'End Sub

    'Private Sub cmdBack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdBack.Click
    '    Response.Redirect(GetUrlReferrerForCmdBack().ToString())
    'End Sub

    Private Function StripQueryStringFromUrl(ByVal pUrl As String) As String
        Return Regex.Replace(pUrl, "\?.*$", String.Empty)
    End Function

    Private Function GetUrlReferrerForCmdBack() As System.Uri
        Dim aUri As System.Uri
        Dim aString As String

        aUri = Request.UrlReferrer
        aString = Request.QueryString("legende")
        If aString = "AppVFS-ADR" Or aString = "AppVFS-KZL" Then
            aUri = New Uri(lblHidden.Text)
        End If
        Return aUri
    End Function

    Private Sub ActivateCmdBackButton()
        If legende = "AppVFS-ADR" Or legende = "AppVFS-KZL" Then
            cmdBack.Enabled = True
            cmdBack.Visible = True
            If Not IsPostBack Then
                lblHidden.Text = Request.UrlReferrer.ToString()
            End If
        Else
            cmdBack.Visible = False
            cmdBack.Enabled = False
            lblHidden.Text = String.Empty
        End If
    End Sub

    Private Sub BuildTextForLabelHead()

        'Wenn die Anzeige aus AppVFS Kennzeichenbestand aufgerufen wurde,
        'soll nicht der Friendly Name angezeigt werden.
        'ucStyles.TitleText = m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString
        If (legende = "AppVFS-ADR") Then
            lblHead.Text = "Adressen"
            lblPageTitle.Text = String.Empty
        ElseIf (legende = "AppVFS-KZL") Then
            lblHead.Text = "Kennzeichenliste"
            lblPageTitle.Text = String.Empty
        Else
            lblHead.Text = m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString
        End If
    End Sub

    Private Sub lnkCreateExcel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lnkCreateExcel.Click
        Try
            Dim excelFactory As New Base.Kernel.DocumentGeneration.ExcelDocumentFactory()
            Dim strFileName As String = Format(Now, "yyyyMMdd_HHmmss_") & m_User.UserName

            excelFactory.CreateDocumentAndSendAsResponse(strFileName, Me.m_objExcel, Me.Page)

        Catch ex As Exception
            lblError.Text = "Fehler beim Erstellen der Excel-Datei: " + ex.Message
        End Try
    End Sub

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        SetEndASPXAccess(Me)
    End Sub

    Private Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Unload
        SetEndASPXAccess(Me)
    End Sub
End Class


