Imports CKG.Portal.PageElements
Imports CKG.Base.Kernel.Common.Common

Public Class Report40_02
    Inherits System.Web.UI.Page

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

    Private m_App As Base.Kernel.Security.App
    Private m_User As Base.Kernel.Security.User
    Private m_objTable As DataTable
    Private m_objExcel As DataTable
    Private m_objFin01 As fin_01

    Protected WithEvents lblError As System.Web.UI.WebControls.Label
    Protected WithEvents cmdSave As System.Web.UI.WebControls.LinkButton
    Protected WithEvents Hyperlink2 As System.Web.UI.WebControls.HyperLink
    Protected WithEvents DataGrid1 As System.Web.UI.WebControls.DataGrid
    Protected WithEvents ShowScript As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents lblNoData As System.Web.UI.WebControls.Label
    Protected WithEvents lblDownloadTip As System.Web.UI.WebControls.Label
    Protected WithEvents ucHeader As Header
    Protected WithEvents ucStyles As Styles
    Protected WithEvents lblHead As System.Web.UI.WebControls.Label
    Protected WithEvents lblPageTitle As System.Web.UI.WebControls.Label
    Protected WithEvents ddlPageSize As System.Web.UI.WebControls.DropDownList
    Protected WithEvents cmdBack As System.Web.UI.WebControls.LinkButton
    Protected WithEvents lblHidden As System.Web.UI.WebControls.Label
    Protected WithEvents lnkCreateExcel As System.Web.UI.WebControls.LinkButton
    Private legende As String
    Private csv As String
    Protected WithEvents lnkBack As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lnkShowCSV As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lbl_Info As System.Web.UI.WebControls.Label
    'Private schmal As String

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        cmdSave.Enabled = False
        m_User = GetUser(Me)
        ucHeader.InitUser(m_User)
        FormAuth(Me, m_User)

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
        ActivateCmdBackButton()

        m_App = New Base.Kernel.Security.App(m_User)

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
       
    End Sub

    Private Sub cmdSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSave.Click
        '##
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

            Dim k As Int32
            Dim l As Int32
            For l = 0 To DataGrid1.Items.Count - 1
                For k = 0 To m_objTable.Columns.Count - 1
                    Select Case m_objTable.Columns(k).ExtendedProperties("Alignment").ToString
                        Case "Right"
                            DataGrid1.Items(l).Cells(k).HorizontalAlign = HorizontalAlign.Right
                        Case "Center"
                            DataGrid1.Items(l).Cells(k).HorizontalAlign = HorizontalAlign.Center
                        Case Else
                            DataGrid1.Items(l).Cells(k).HorizontalAlign = HorizontalAlign.Left
                    End Select
                Next
            Next
            
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

            Dim item As DataGridItem
            Dim cell As TableCell


            For Each item In DataGrid1.Items                    'Zeilen des DataGrids durchgehen...
                Dim datEingang As Date
                Dim daysBack As Long
                cell = item.Cells(5)                            'eingangsdatum
                If IsDate(cell.Text) Then
                    datEingang = cell.Text
                    daysBack = DateDiff(DateInterval.Day, datEingang, Now)
                    If daysBack >= 14 Then
                        Dim i As Integer
                        For i = 0 To item.Cells.Count - 1
                            cell = item.Cells(i)
                            cell.ForeColor = Color.Red
                        Next
                    End If
                End If
            Next

        End If
    End Sub

    Private Sub DataGrid1_SortCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs) Handles DataGrid1.SortCommand
        FillGrid(DataGrid1.CurrentPageIndex, e.SortExpression)
    End Sub

    Private Sub ddlPageSize_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ddlPageSize.SelectedIndexChanged
        DataGrid1.PageSize = CInt(ddlPageSize.SelectedItem.Value)
        FillGrid(0)
    End Sub

    Private Sub cmdBack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdBack.Click
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

        excelFactory.CreateDocumentAndSendAsResponse(strFileName, Me.m_objExcel, Me.Page)

     
    End Sub

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        SetEndASPXAccess(Me)
    End Sub

    Private Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Unload
        SetEndASPXAccess(Me)
    End Sub

    Private Sub DataGrid1_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles DataGrid1.ItemCommand
        If e.CommandName = "Delete" Then
            Dim strTidNR As String
            Dim strFileName As String = Format(Now, "yyyyMMdd_HHmmss_") & m_User.UserName & ".xls"

            m_objFin01 = New fin_01(m_User, m_App, strFileName)
            Session.Add("objReport", m_objFin01)
            m_objFin01.SessionID = Session.SessionID
            m_objFin01.AppID = CStr(Session("AppID"))
            strTidNR = e.Item.Cells(1).Text

            m_objFin01.DelDocuments(Session("AppID").ToString, Session.SessionID, strTidNR)
            Dim logApp As New Base.Kernel.Logging.Trace(m_App.Connectionstring, m_App.SaveLogAccessSAP, m_App.LogLevel)
            logApp.CollectDetails("Händlername", CType(e.Item.Cells(0).Text.TrimStart("0"c), Object), True)
            logApp.CollectDetails("ZB2-Nummer", CType((strTidNR), Object))
            logApp.CollectDetails("Fahrgestellnummer.", CType(e.Item.Cells(2).Text.TrimStart("0"c), Object))
            logApp.CollectDetails("Modell", CType(e.Item.Cells(3).Text.TrimStart("0"c), Object))
            logApp.CollectDetails("HEK Nummer", CType(e.Item.Cells(4).Text.TrimStart("0"c), Object))
            logApp.CollectDetails("Eingangsdatum", CType(e.Item.Cells(5).Text.TrimStart("0"c), Object))

            If Not m_objFin01.Status = 0 Then

                lblError.Text = m_objFin01.Message
                logApp.WriteEntry("ERR", m_User.UserName, Session.SessionID, CInt(Session("AppID")), m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString, strTidNR, "Fehler beim Löschen von Daten ohne Dokumente mit ZB2-Nummer" & strTidNR & ". (Fehler: " & m_objFin01.Message & ")", m_User.CustomerName, m_User.Customer.CustomerId, m_User.IsTestUser, 10, logApp.InputDetails)
            Else
                logApp.WriteEntry("APP", m_User.UserName, Session.SessionID, CInt(Session("AppID")), m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString, strTidNR, "Daten ohne Dokumente mit ZB2-Nummer" & strTidNR & " erfolgreich gelöscht.", m_User.CustomerName, m_User.Customer.CustomerId, m_User.IsTestUser, 0, logApp.InputDetails)
            End If
            Response.Redirect("Report40.aspx?AppID=" & Session("AppID").ToString)
        End If
    End Sub
End Class
' ************************************************
' $History: Report40_02.aspx.vb $
' 
' *****************  Version 8  *****************
' User: Dittbernerc  Date: 17.06.09   Time: 15:56
' Updated in $/CKAG/Components/ComCommon/Finance
' .Net Connector Umstellung
' 
' Bapis:
' 
' Z_M_Daten_Ohne_Brief
' Z_M_Daten_Ohne_Brief_Del
' 
' *****************  Version 7  *****************
' User: Jungj        Date: 5.06.09    Time: 15:58
' Updated in $/CKAG/Components/ComCommon/Finance
' mögliche try catches entfernt
' 
' *****************  Version 6  *****************
' User: Rudolpho     Date: 20.05.09   Time: 11:37
' Updated in $/CKAG/Components/ComCommon/Finance
' 
' *****************  Version 5  *****************
' User: Jungj        Date: 4.06.08    Time: 15:03
' Updated in $/CKAG/Components/ComCommon/Finance
' ITA 1925
' 
' *****************  Version 4  *****************
' User: Jungj        Date: 20.05.08   Time: 11:30
' Updated in $/CKAG/Components/ComCommon/Finance
' ITA 1925
' 
' *****************  Version 3  *****************
' User: Rudolpho     Date: 11.04.08   Time: 13:43
' Updated in $/CKAG/Components/ComCommon/Finance
' Migration
' 
' *****************  Version 2  *****************
' User: Rudolpho     Date: 10.04.08   Time: 17:37
' Updated in $/CKAG/Components/ComCommon/Finance
' Migration
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 4.04.08    Time: 13:32
' Created in $/CKAG/Components/ComCommon/Finance
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 4.04.08    Time: 10:48
' Created in $/CKAG/portal/Components/ComCommon/Finance
' 
' *****************  Version 3  *****************
' User: Rudolpho     Date: 26.03.08   Time: 16:06
' Updated in $/CKG/Components/ComCommon/ComCommonWeb/Finance
' ITA: 1800
' 
' ************************************************
