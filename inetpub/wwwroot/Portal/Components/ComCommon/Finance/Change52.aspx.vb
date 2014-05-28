Imports CKG.Base.Kernel
Imports CKG.Portal.PageElements
Imports CKG.Base.Kernel.Common.Common
'Imports System.Text.RegularExpressions


Public Class Change52
    Inherits System.Web.UI.Page
    Private m_App As Base.Kernel.Security.App
    Private m_User As Base.Kernel.Security.User
    Private m_objTable As DataTable
    Private m_objExcel As DataTable

    Protected WithEvents lblHead As System.Web.UI.WebControls.Label
    Protected WithEvents lblPageTitle As System.Web.UI.WebControls.Label
    Protected WithEvents lnkCreateExcel As System.Web.UI.WebControls.LinkButton
    Protected WithEvents lnkShowCSV As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lblDownloadTip As System.Web.UI.WebControls.Label
    Protected WithEvents lnkBack As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lblNoData As System.Web.UI.WebControls.Label
    Protected WithEvents ddlPageSize As System.Web.UI.WebControls.DropDownList
    Protected WithEvents lbl_Info As System.Web.UI.WebControls.Label
    Protected WithEvents lblError As System.Web.UI.WebControls.Label
    Protected WithEvents lblHidden As System.Web.UI.WebControls.Label
    Protected WithEvents ShowScript As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents ucHeader As Header
    Protected WithEvents ucStyles As Styles
    Private legende As String
    Private csv As String
    'Private schmal As String
    Protected WithEvents DataGrid1 As System.Web.UI.WebControls.DataGrid
    Protected WithEvents cmdweiter As System.Web.UI.WebControls.LinkButton
    Dim m_report As fin_04

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
        ucHeader.InitUser(m_User)
        FormAuth(Me, m_User)
        GetAppIDFromQueryString(Me)
        m_App = New Base.Kernel.Security.App(m_User)

        If doSubmit() Then
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


            legende = Request.QueryString.Item("legende")
            If (legende = "Ja") Then
                lbl_Info.Text = "<br>*(V)ollmacht, (H)andelsregistereintrag, (P)ersonalausweis, (G)ewerbeanmeldung, (E)inzugsermächtigung"
            End If
            If (legende = "Report201") Then
                lbl_Info.Text = "<br>'X' = vorhanden"
            End If
            If (legende = "Report203") Then
                lbl_Info.Text = "<br>*(V)ollmacht, (H)andelsregistereintrag, (P)ersonalausweis, (G)ewerbeanmeldung, (E)inzugsermächtigung, 'X' = vorhanden"
            End If

            BuildTextForLabelHead()

            legende = Request.QueryString.Item("legende")
            If (legende = "Ja") Then
                lbl_Info.Text = "<br>*(V)ollmacht, (H)andelsregistereintrag, (P)ersonalausweis, (G)ewerbeanmeldung, (E)inzugsermächtigung"
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
                    lnkCreateExcel.Visible = True
                End If

                ddlPageSize.Items.Add("10")
                ddlPageSize.Items.Add("20")
                ddlPageSize.Items.Add("50")
                ddlPageSize.Items.Add("100")
                ddlPageSize.Items.Add("200")
                ddlPageSize.Items.Add("500")
                ddlPageSize.Items.Add("1000")
                ddlPageSize.SelectedIndex = 2

                If Not Session("ApplblInfoText") Is Nothing Then
                    If lbl_Info.Text.Length = 0 Then
                        lbl_Info.Text = CStr(Session("ApplblInfoText"))
                    Else
                        lbl_Info.Text &= "<br>" & CStr(Session("ApplblInfoText"))
                    End If
                End If

                FillGrid(0)
            End If
          
        Else 'nur wegen "keine daten zur anzeige" meldung und zurückbutton! JJU2008.05.28
            cmdweiter.Visible = False
            ddlPageSize.Visible = False
            lnkCreateExcel.Visible = False
            lnkBack.Visible = True
            lblError.Visible = True
        End If

    End Sub

    Private Sub cmdSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdweiter.Click
        '##
    End Sub

    Private Function doSubmit() As Boolean

        Dim strFileName As String = Format(Now, "yyyyMMdd_HHmmss_") & m_User.UserName & ".xls"
        m_report = New fin_04(m_User, m_App, strFileName)
        Session.Add("objReport", m_report)
        m_report.SessionID = Session.SessionID
        m_report.AppID = Session("AppID")
        m_report.Fill(Session("AppID"), Session.SessionID.ToString)
        If m_report.Status And Not m_report.Status = -12 < 0 Then '-12=no-Data
            lblError.Text = "Fehler: " & m_report.Message
        Else
            If m_report.Result Is Nothing OrElse m_report.Result.Rows.Count = 0 Then
                lblError.Text = "Es wurden keine Daten zur Anzeige gefunden."
            Else
                'wiso bekomme ich den filename den ich beim erzeugen des Reportobjektes übergeben musste nicht wieder zurück? Property Eingebaut in EC_21 JJ2007.11.21


                Excel.ExcelExport.WriteExcel(m_report.Result, ConfigurationManager.AppSettings("ExcelPath") & m_report.FileName)
                m_report.Result.Columns.Add("Zuordnen", System.Type.GetType("System.Boolean"))
                Session("ResultTable") = m_report.Result
                Session("lnkExcel") = "/Portal/Temp/Excel/" & m_report.FileName
                Return True
            End If
        End If
        Return False
    End Function

    Private Sub DataGrid1_PageIndexChanged(ByVal source As System.Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs)
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

            Dim tmpDataView As DataView = m_objTable.DefaultView

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
            
            lblNoData.Visible = True

            If DataGrid1.PageCount > 1 Then
                DataGrid1.PagerStyle.CssClass = "PagerStyle"
                DataGrid1.DataBind()
                DataGrid1.PagerStyle.Visible = True
            Else
                DataGrid1.PagerStyle.Visible = False
            End If

            Dim item As DataGridItem
            Dim cell As TableCell
            Dim label As Label
            Dim control As control

            For Each item In DataGrid1.Items                    'Zeilen des DataGrids durchgehen...
                Dim datEingang As Date
                Dim daysBack As Long
                cell = item.Cells(5) 'Erste Spalte holen = CheckBox (nicht sichtbar)
                For Each control In cell.Controls
                    Dim control1 = TryCast(control, Label)
                    If (control1 IsNot Nothing) Then
                        label = control1
                        If label.ID = "Label6" Then
                            If IsDate(label.Text) Then
                                datEingang = label.Text
                                label.Text = datEingang.ToShortDateString
                                daysBack = DateDiff(DateInterval.Day, datEingang, Now)
                                If daysBack >= 14 Then
                                    Dim i As Integer
                                    For i = 0 To item.Cells.Count - 1
                                        cell = item.Cells(i)
                                        cell.ForeColor = Color.Red
                                    Next
                                End If
                            End If
                        End If
                    End If
                Next

            Next

        End If
    End Sub

    Private Sub DataGrid1_ItemDataBound(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs)
        Dim intItem As Int32

        For intItem = 0 To m_objTable.Columns.Count - 1
            If m_objTable.Columns(intItem).DataType Is System.Type.GetType("System.DateTime") Then
                If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Then
                    e.Item.Cells(intItem).Text = DataBinder.Eval(e.Item.DataItem, m_objTable.Columns(intItem).ColumnName, "{0:dd.MM.yyyy}")
                End If
            End If
        Next
    End Sub

    Private Sub DataGrid1_SortCommand(ByVal source As System.Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs) Handles DataGrid1.SortCommand
        FillGrid(DataGrid1.CurrentPageIndex, e.SortExpression)
    End Sub

    Private Sub ddlPageSize_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ddlPageSize.SelectedIndexChanged
        DataGrid1.PageSize = CInt(ddlPageSize.SelectedItem.Value)
        FillGrid(0)
    End Sub

    Private Sub cmdBack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Response.Redirect(GetUrlReferrerForCmdBack().ToString())
    End Sub

    'Private Function StripQueryStringFromUrl(ByVal pUrl As String) As String
    '    Return Regex.Replace(pUrl, "\?.*$", String.Empty)
    'End Function

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
    
    Private Sub BuildTextForLabelHead()

        'Wenn die Anzeige aus AppVFS Kennzeichenbestand aufgerufen wurde,
        'soll nicht der Friendly Name angezeigt werden.
        ucStyles.TitleText = m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString
        If (legende = "AppVFS-ADR") Then
            lblHead.Text = "Adressen"
            lblPageTitle.Text = String.Empty
            ucStyles.TitleText &= " - " & lblHead.Text
        ElseIf (legende = "AppVFS-KZL") Then
            lblHead.Text = "Kennzeichenliste"
            lblPageTitle.Text = String.Empty
            ucStyles.TitleText &= " - " & lblHead.Text
        Else
            lblHead.Text = m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString
        End If
    End Sub

    Private Sub lnkCreateExcel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lnkCreateExcel.Click
        Dim excelFactory As New Base.Kernel.DocumentGeneration.ExcelDocumentFactory()
        Dim strFileName As String = Format(Now, "yyyyMMdd_HHmmss_") & m_User.UserName

        excelFactory.CreateDocumentAndSendAsResponse(strFileName, m_objExcel, Page)
    End Sub

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        SetEndASPXAccess(Me)
    End Sub

    Private Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Unload
        SetEndASPXAccess(Me)
    End Sub

    Private Sub cmdweiter_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdweiter.Click
        Dim item As DataGridItem
        Dim cell As TableCell
        Dim label As label
        Dim chkBox As CheckBox
        Dim control As control
        Dim tmpRows() As DataRow
        Dim bCheck As Boolean
        Dim sFahrgest As String = ""
        Dim TempTable As DataTable = Session("ResultTable")

        For Each item In DataGrid1.Items

            cell = item.Cells(6)
            For Each control In cell.Controls
                Dim checkBox = TryCast(control, CheckBox)
                If (checkBox IsNot Nothing) Then
                    chkBox = checkBox
                    bCheck = chkBox.Checked
                End If
            Next
            If bCheck = True Then
                cell = item.Cells(1)
                For Each control In cell.Controls
                    Dim control1 = TryCast(control, Label)
                    If (control1 IsNot Nothing) Then
                        label = control1
                        sFahrgest = label.Text
                    End If
                Next
                tmpRows = TempTable.Select("Fahrgestellnummer='" & sFahrgest & "'")
                tmpRows(0).BeginEdit()
                tmpRows(0).Item("Zuordnen") = True
                tmpRows(0).EndEdit()
                TempTable.AcceptChanges()
            End If

        Next

        tmpRows = TempTable.Select("Zuordnen=True")
        If Not CType(tmpRows.Length, Integer) > 0 Then
            lblError.Text = "Keine Dokumente ausgewählt!"
        Else
            Session("ResultTable") = TempTable
            Response.Redirect("Change52_02.aspx?AppID=" & Session("AppID").ToString)
        End If

    End Sub
End Class
' ************************************************
' $History: Change52.aspx.vb $
' 
' *****************  Version 10  *****************
' User: Dittbernerc  Date: 19.06.09   Time: 11:25
' Updated in $/CKAG/Components/ComCommon/Finance
' ITA 2918 - Abschaltung .Net Connector
' 
' Bapis:
' 
' Z_M_Brief_Ohne_Daten
' 
' *****************  Version 9  *****************
' User: Jungj        Date: 5.06.09    Time: 15:58
' Updated in $/CKAG/Components/ComCommon/Finance
' mögliche try catches entfernt
' 
' *****************  Version 8  *****************
' User: Jungj        Date: 18.05.09   Time: 11:32
' Updated in $/CKAG/Components/ComCommon/Finance
' BugFix AKF offene Anforderungen
' 
' *****************  Version 7  *****************
' User: Rudolpho     Date: 4.05.09    Time: 11:50
' Updated in $/CKAG/Components/ComCommon/Finance
' 
' *****************  Version 6  *****************
' User: Jungj        Date: 29.05.08   Time: 8:36
' Updated in $/CKAG/Components/ComCommon/Finance
' ITA 1945
' 
' *****************  Version 5  *****************
' User: Jungj        Date: 18.04.08   Time: 10:51
' Updated in $/CKAG/Components/ComCommon/Finance
' Code Kopiert, Migrationsscheiss
' 
' *****************  Version 2  *****************
' User: Rudolpho     Date: 26.02.08   Time: 17:24
' Updated in $/CKG/Components/ComCommon/ComCommonWeb/Finance
' ITA: 1733
' 
' ************************************************
