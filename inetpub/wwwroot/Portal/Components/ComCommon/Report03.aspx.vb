Option Strict On
Option Explicit On 
Option Compare Binary

Imports CKG.Base.Business
Imports CKG.Base.Kernel
Imports CKG.Base.Kernel.Common.Common
Imports CKG.Portal.PageElements

Public Class Report03
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

    Private m_User As Base.Kernel.Security.User
    Private m_App As Base.Kernel.Security.App
    Private objHandler As Report_03

    Protected WithEvents DataGrid1 As System.Web.UI.WebControls.DataGrid
    Protected WithEvents ucHeader As Header
    Protected WithEvents ShowScript As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents Literal1 As System.Web.UI.WebControls.Literal
    Protected WithEvents lblPageTitle As System.Web.UI.WebControls.Label
    Protected WithEvents lblHead As System.Web.UI.WebControls.Label
    Protected WithEvents lnkExcel As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lblDownloadTip As System.Web.UI.WebControls.Label
    Protected WithEvents lblNoData As System.Web.UI.WebControls.Label
    Protected WithEvents lblError As System.Web.UI.WebControls.Label
    Protected WithEvents cmdCreate As System.Web.UI.WebControls.LinkButton
    Protected WithEvents calAbDatum As System.Web.UI.WebControls.Calendar
    Protected WithEvents calBisDatum As System.Web.UI.WebControls.Calendar
    Protected WithEvents btnOpenSelectAb As System.Web.UI.WebControls.Button
    Protected WithEvents btnOpenSelectBis As System.Web.UI.WebControls.Button
    Protected WithEvents txtDatumVon As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtDatumBis As System.Web.UI.WebControls.TextBox
    Protected WithEvents ImageButton1 As System.Web.UI.WebControls.ImageButton
    Protected WithEvents ucStyles As Styles

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        m_User = GetUser(Me)
        ucHeader.InitUser(m_User)
        FormAuth(Me, m_User)

        GetAppIDFromQueryString(Me)

        lblHead.Text = m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString
        ucStyles.TitleText = lblHead.Text
        m_App = New Base.Kernel.Security.App(m_User)

        If Not IsPostBack Then

            txtDatumVon.Text = Now.ToShortDateString
            txtDatumBis.Text = Now.AddMonths(1).ToShortDateString

            DoSubmit()
        Else
            If Session("objHandler") Is Nothing Then
                DoSubmit()
            Else
                objHandler = CType(Session("objHandler"), Report_03)
            End If
        End If

    End Sub

    Private Sub DoSubmit()
        If Not IsDate(txtDatumVon.Text) Then
            lblError.Text = "Bitte geben Sie einen gültigen Datumswert beim Startdatum ein (TT.MM.YYYY)."
            Exit Sub
        End If

        If Not IsDate(txtDatumBis.Text) Then
            lblError.Text = "Bitte geben Sie einen gültigen Datumswert beim Enddatum ein (TT.MM.YYYY)."
            Exit Sub
        End If

        If CDate(txtDatumVon.Text) < CDate(Now.ToShortDateString) Then
            lblError.Text = "Bitte geben Sie kein Startdatum in der Vergangenheit ein."
            Exit Sub
        End If

        If CDate(txtDatumBis.Text) < CDate(txtDatumVon.Text) Then
            lblError.Text = "Das Enddatum muss nach dem Startdatum sein."
            Exit Sub
        End If

        objHandler = New Report_03(m_User, m_App, Session("AppID").ToString, Session.SessionID.ToString, "")
        objHandler.AppID = Session("AppID").ToString
        objHandler.DatumVon = CDate(txtDatumVon.Text)
        objHandler.DatumBis = CDate(txtDatumBis.Text)

        objHandler.Show()

        If objHandler.Status = 0 Then

            Dim strFileName As String = Format(Now, "yyyyMMdd_HHmmss_") & m_User.UserName & ".xls"
            Dim excelFactory As New Base.Kernel.DocumentGeneration.ExcelDocumentFactory()


            excelFactory.CreateDocumentAndWriteToFilesystem(ConfigurationManager.AppSettings("ExcelPath") & strFileName, objHandler.ResultExcel, Me)
            Session("lnkExcel") = "/Portal/Temp/Excel/" & strFileName

            FillGrid(0)
        Else
            lblError.Text = objHandler.Message
        End If
    End Sub

    Private Sub FillGrid(ByVal intPageIndex As Int32, Optional ByVal strSort As String = "")
        If objHandler.Status = 0 Then

            If objHandler.Auftraege.Rows.Count = 0 Then
                DataGrid1.Visible = False
                lblNoData.Visible = True
                lblNoData.Text = "Keine Daten zur Anzeige gefunden."
            Else
                DataGrid1.Visible = True
                lblNoData.Visible = False

                Dim tmpDataView As New DataView()
                tmpDataView = objHandler.Auftraege.DefaultView

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

                lblNoData.Text = "Es wurden " & tmpDataView.Count.ToString & " Vorgänge gefunden."
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
                Dim button As LinkButton
                Dim label As label
                Dim image As System.Web.UI.WebControls.Image
                Dim control As control
                Dim blnScriptFound As Boolean = False

                Dim strVertriebsbelegnummer As String = ""
                Dim strDurchfuehrung As String = ""
                Dim strHaendlername As String = ""
                Dim strHaendlerPLZ As String = ""
                Dim strHaendlerort As String = ""
                Dim strBemerkung As String = ""
                Dim strText As String = ""

                For Each item In DataGrid1.Items
                    cell = item.Cells(0)
                    strVertriebsbelegnummer = cell.Text
                    cell = item.Cells(2)
                    strDurchfuehrung = cell.Text
                    cell = item.Cells(6)
                    strHaendlerPLZ = cell.Text
                    cell = item.Cells(7)
                    strHaendlerort = cell.Text

                    cell = item.Cells(5)
                    For Each control In cell.Controls
                        If TypeOf control Is label Then
                            label = CType(control, label)
                            If label.ID = "lblName" Then
                                strHaendlername = label.Text
                            End If
                        End If
                    Next

                    cell = item.Cells(1)
                    For Each control In cell.Controls
                        If TypeOf control Is System.Web.UI.WebControls.Image Then
                            image = CType(control, System.Web.UI.WebControls.Image)
                            If image.ID = "imgWarning" Then
                                If CDate(strDurchfuehrung) <= CDate(Now.AddDays(7).ToShortDateString) Then
                                    image.ImageUrl = "/Portal/Images/red.jpg"
                                ElseIf CDate(Now.AddDays(7).ToShortDateString) < CDate(strDurchfuehrung) And CDate(strDurchfuehrung) <= CDate(Now.AddDays(21).ToShortDateString) Then
                                    image.ImageUrl = "/Portal/Images/yellow.jpg"
                                ElseIf CDate(Now.AddDays(21).ToShortDateString) < CDate(strDurchfuehrung) Then
                                    image.ImageUrl = "/Portal/Images/empty.gif"
                                End If
                            End If
                        End If
                    Next

                    strText = strDurchfuehrung & " - " & _
                             strHaendlername & ", " & _
                             strHaendlerPLZ & " " & _
                             strHaendlerort & "\n" & _
                             "Geben Sie eine Bemerkung für diesen Vorgang ein."
                    strText = Replace(strText, "&nbsp;", " ")

                    cell = item.Cells(item.Cells.Count - 1)
                    For Each control In cell.Controls
                        If TypeOf control Is LinkButton Then
                            button = CType(control, LinkButton)
                            strBemerkung = button.ToolTip
                            If button.CommandName = "Select" Then
                                button.Attributes.Add("onClick", "if (!BemerkungSenden('" & strVertriebsbelegnummer & "','" & strText & "','" & strBemerkung & "')) return false;")
                                button.Attributes.Add("class", "StandardButtonTable")
                                blnScriptFound = True
                            End If
                        End If
                    Next
                Next
                If blnScriptFound Then
                    ShowScript.Visible = True
                End If
            End If
        Else
            lblError.Text = objHandler.Message
            lblNoData.Visible = True
        End If
    End Sub

    Private Sub DataGrid1_PageIndexChanged(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles DataGrid1.PageIndexChanged
        FillGrid(e.NewPageIndex)
    End Sub

    Private Sub DataGrid1_SortCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs) Handles DataGrid1.SortCommand
        FillGrid(DataGrid1.CurrentPageIndex, e.SortExpression)
    End Sub

    Private Sub DataGrid1_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles DataGrid1.ItemCommand
        If e.CommandName = "Select" Then
            If Not Request.Form.Item("txtBemerkung") Is Nothing AndAlso CStr(Request.Form.Item("txtBemerkung")).Length > 0 Then
                objHandler.Vbeln = e.Item.Cells(0).Text
                objHandler.Bemerkung = CStr(Request.Form.Item("txtBemerkung"))
                objHandler.Change()
                Dim logApp As New Base.Kernel.Logging.Trace(m_App.Connectionstring, m_App.SaveLogAccessSAP, m_App.LogLevel)

                If objHandler.Status = 0 Then
                    'AENDERUNGEN LOGGEN!
                    logApp.WriteEntry("APP", m_User.UserName, Session.SessionID, CInt(Session("AppID")), m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString, e.Item.Cells(0).Text, "Bemerkung: " & CStr(Request.Form("txtBemerkung")), m_User.CustomerName, m_User.Customer.CustomerId, m_User.IsTestUser, 0)
                    logApp.WriteStandardDataAccessSAP(objHandler.IDSAP)

                    'DataGrid neu befuellen
                    DoSubmit()
                Else
                    'AENDERUNGEN LOGGEN!
                    logApp.WriteEntry("ERR", m_User.UserName, Session.SessionID, CInt(Session("AppID")), m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString, e.Item.Cells(0).Text, "Fehler beim Schreiben der Bemerkung. (" & objHandler.Message & ")", m_User.CustomerName, m_User.Customer.CustomerId, m_User.IsTestUser, 0)
                    logApp.WriteStandardDataAccessSAP(objHandler.IDSAP)

                    'Fehler ausgeben
                    lblError.Text = objHandler.Message
                End If
            End If
        End If
    End Sub

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        SetEndASPXAccess(Me)
    End Sub

    Private Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Unload
        SetEndASPXAccess(Me)
    End Sub

    Private Sub btnOpenSelectAb_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOpenSelectAb.Click
        calAbDatum.Visible = True
    End Sub

    Private Sub btnOpenSelectBis_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOpenSelectBis.Click
        calBisDatum.Visible = True
    End Sub

    Private Sub calAbDatum_SelectionChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles calAbDatum.SelectionChanged
        calAbDatum.Visible = False
        txtDatumVon.Text = calAbDatum.SelectedDate.ToShortDateString
    End Sub

    Private Sub calBisDatum_SelectionChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles calBisDatum.SelectionChanged
        calBisDatum.Visible = False
        txtDatumBis.Text = calBisDatum.SelectedDate.ToShortDateString
    End Sub

    Private Sub cmdCreate_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdCreate.Click
        DoSubmit()
    End Sub
End Class

' ************************************************
' $History: Report03.aspx.vb $
' 
' *****************  Version 3  *****************
' User: Jungj        Date: 5.06.09    Time: 15:58
' Updated in $/CKAG/Components/ComCommon
' mögliche try catches entfernt
' 
' *****************  Version 2  *****************
' User: Rudolpho     Date: 11.04.08   Time: 13:43
' Updated in $/CKAG/Components/ComCommon
' Migration
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 4.04.08    Time: 13:32
' Created in $/CKAG/Components/ComCommon
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 4.04.08    Time: 10:47
' Created in $/CKAG/portal/Components/ComCommon
' 
' *****************  Version 6  *****************
' User: Uha          Date: 26.09.07   Time: 13:22
' Updated in $/CKG/Components/ComCommon/ComCommonWeb
' Bugfixing in ITA 1237, 1181 und 1238 (Alle Floorcheck)
' 
' *****************  Version 5  *****************
' User: Uha          Date: 25.09.07   Time: 17:03
' Updated in $/CKG/Components/ComCommon/ComCommonWeb
' ITA 1124 hinzugefügt (Change03/Change03_2) und allgemeines Bugfix
' Floorcheck
' 
' *****************  Version 4  *****************
' User: Uha          Date: 20.09.07   Time: 17:19
' Updated in $/CKG/Components/ComCommon/ComCommonWeb
' ITA 1181 Testversion
' 
' *****************  Version 3  *****************
' User: Uha          Date: 17.09.07   Time: 14:40
' Updated in $/CKG/Components/ComCommon/ComCommonWeb
' ITA 1237: Bugfixing; Bemerkung: BAPI zum Schreiben der Bemerkung steht
' noch aus
' 
' *****************  Version 2  *****************
' User: Uha          Date: 13.09.07   Time: 18:46
' Updated in $/CKG/Components/ComCommon/ComCommonWeb
' ITA 1237 - Report "Geplante Floorchecks": Bugfix 1
' 
' *****************  Version 1  *****************
' User: Uha          Date: 5.09.07    Time: 17:21
' Created in $/CKG/Components/ComCommon/ComCommonWeb
' ITA 1237 - Report "Geplante Floorchecks": Testversion fertig (keine
' Testdaten im CKQ)
' 
' ************************************************
