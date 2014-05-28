Imports CKG.Base.Business
Imports CKG.Base.Kernel
Imports CKG.Portal.PageElements
Imports CKG.Base.Kernel.Common.Common
Imports System.Data.OleDb
Imports System.Data

Public Class Change80
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
    Private objHandler As Change_80

    Protected WithEvents Hyperlink1 As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lnkLogout As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lblError As System.Web.UI.WebControls.Label
    Protected WithEvents lblPageTitle As System.Web.UI.WebControls.Label
    Protected WithEvents ucHeader As Header
    Protected WithEvents ucStyles As Styles
    Protected WithEvents lblHead As System.Web.UI.WebControls.Label
    Protected WithEvents trcmdSearch As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents lbl_Auswahl As System.Web.UI.WebControls.Label
    Protected WithEvents rb_Alle As System.Web.UI.WebControls.RadioButton
    Protected WithEvents rb_Geplant As System.Web.UI.WebControls.RadioButton
    Protected WithEvents rb_InDurchfuehrung As System.Web.UI.WebControls.RadioButton
    Protected WithEvents rb_Erledigt As System.Web.UI.WebControls.RadioButton
    Protected WithEvents txtGebiet As System.Web.UI.WebControls.TextBox
    Protected WithEvents tr_Auswahl As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents tr_Gebiet As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents lbl_Gebiet As System.Web.UI.WebControls.Label
    Protected WithEvents txtVon As System.Web.UI.WebControls.TextBox
    Protected WithEvents tr_Von As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents lbl_Von As System.Web.UI.WebControls.Label
    Protected WithEvents btnOpenSelectAb As System.Web.UI.WebControls.Button
    Protected WithEvents calAbDatum As System.Web.UI.WebControls.Calendar
    Protected WithEvents lbl_Bis As System.Web.UI.WebControls.Label
    Protected WithEvents txtBis As System.Web.UI.WebControls.TextBox
    Protected WithEvents tr_Bis As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents btnOpenSelectBis As System.Web.UI.WebControls.Button
    Protected WithEvents calBisDatum As System.Web.UI.WebControls.Calendar
    Protected WithEvents DataGrid1 As System.Web.UI.WebControls.DataGrid
    Protected WithEvents lb_Search As System.Web.UI.WebControls.LinkButton
    Protected WithEvents lnkCreateExcel As System.Web.UI.WebControls.LinkButton
    Protected WithEvents ImageButton1 As System.Web.UI.WebControls.ImageButton
    Protected WithEvents Literal1 As System.Web.UI.WebControls.Literal

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        m_User = GetUser(Me)
        ucHeader.InitUser(m_User)
        FormAuth(Me, m_User)

        GetAppIDFromQueryString(Me)

        lblHead.Text = m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString
        ucStyles.TitleText = lblHead.Text
        m_App = New Base.Kernel.Security.App(m_User)

        If (Session("objHandler") Is Nothing) Then
            objHandler = New Change_80(m_User, m_App, Session("AppID").ToString, Session.SessionID.ToString, "")
            HideResults()
        Else
            objHandler = CType(Session("objHandler"), Change_80)
            If Not objHandler.ResultSummary Is Nothing AndAlso objHandler.ResultSummary.Rows.Count > 0 Then
                If Request.Form.Item("txtReturnSearch") Is Nothing OrElse Not CStr(Request.Form.Item("txtReturnSearch")) = "Search" Then
                    FillGrid(0)
                    txtBis.Text = objHandler.Pruefdat_bis
                    txtVon.Text = objHandler.Pruefdat_von
                    txtGebiet.Text = objHandler.Gebiet
                End If
            Else
                HideResults()
            End If
        End If

        Session("objHandler") = objHandler
        If Not IsPostBack Then
            lb_Search.Visible = True

            Literal1.Text = "		<script language=""JavaScript"">" & vbCrLf
            Literal1.Text &= "			<!-- //" & vbCrLf
            Literal1.Text &= "			window.document.Form1.txtGebiet.focus();" & vbCrLf
            Literal1.Text &= "			//-->" & vbCrLf
            Literal1.Text &= "		</script>" & vbCrLf

            lb_Search.Attributes.Add("onclick", "submitsearch();")
        End If
       
    End Sub

    Private Sub HideResults()
        DataGrid1.Visible = False
        lnkCreateExcel.Visible = False
    End Sub

    Private Sub ShowResults()
        DataGrid1.Visible = True
        lnkCreateExcel.Visible = True
    End Sub

    Private Sub CheckDateInput(ByVal blnCheckRange As Boolean)
        If Not IsDate(txtVon.Text) Then
            Throw New Exception("Startdatum hat ungültiges Format.")
        End If
        If Not IsDate(txtBis.Text) Then
            Throw New Exception("Enddatum hat ungültiges Format.")
        End If
        If CDate(txtBis.Text) < CDate(txtVon.Text) Then
            Throw New Exception("Enddatum darf nicht vor Startdatum liegen.")
        End If
        If blnCheckRange And DateAdd(DateInterval.Month, 3, CDate(txtVon.Text)) < CDate(txtBis.Text) Then
            Throw New Exception("Start- und Enddatum dürfen nicht länger als drei Monate auseinander liegen.")
        End If
        objHandler.Pruefdat_von = CDate(txtVon.Text).ToShortDateString
        objHandler.Pruefdat_bis = CDate(txtBis.Text).ToShortDateString
    End Sub

    Private Sub DoSubmit()

        Dim logApp As New Base.Kernel.Logging.Trace(m_App.Connectionstring, m_App.SaveLogAccessSAP, m_App.LogLevel)
        logApp.InitEntry(m_User.UserName, Session.SessionID, CInt(Session("AppID")), m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString, m_User.CustomerName, m_User.Customer.CustomerId, m_User.IsTestUser, 0)
        Session.Add("logObj", logApp)

        lblError.Text = ""

        If txtGebiet.Text.Trim(" "c).Length = 0 Then
            objHandler.Gebiet = ""
        Else
            objHandler.Gebiet = txtGebiet.Text.Trim(" "c)
        End If

        If rb_Alle.Checked Then
            objHandler.StatusInput = "A"
        ElseIf rb_Erledigt.Checked Then
            objHandler.StatusInput = "E"
        ElseIf rb_Geplant.Checked Then
            objHandler.StatusInput = "G"
        ElseIf rb_InDurchfuehrung.Checked Then
            objHandler.StatusInput = "D"
        End If

        Select Case objHandler.StatusInput
            Case "D"
                objHandler.Pruefdat_von = ""
                txtVon.Text = ""
                objHandler.Pruefdat_bis = ""
                txtBis.Text = ""
            Case "E"
                CheckDateInput(True)
            Case Else
                CheckDateInput(False)
        End Select

        objHandler.Show()

        If Not objHandler.Status = 0 Then
            lblError.Text = objHandler.Message
            lblError.Visible = True
        Else
            If objHandler.Result.Rows.Count = 0 Then
                lblError.Text = "Zu den gewählten Kriterien wurden keine Ergebnisse gefunden."
            Else
                FillGrid(0)
                Session("objHandler") = objHandler
            End If
        End If

    End Sub

    Private Sub FillGrid(ByVal intPageIndex As Int32, Optional ByVal strSort As String = "")
        Dim tmpDataView As New DataView()
        tmpDataView = objHandler.ResultSummary.DefaultView

        If tmpDataView.Count = 0 Then
            HideResults()
        Else
            ShowResults()

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

            If DataGrid1.PageCount > 1 Then
                DataGrid1.PagerStyle.CssClass = "PagerStyle"
                DataGrid1.DataBind()
                DataGrid1.PagerStyle.Visible = True
            Else
                DataGrid1.PagerStyle.Visible = False
            End If
        End If
    End Sub


    Private Function RemoveBlankAndPlaceholder(ByVal strIn As String) As String
        Dim strReturn As String = strIn.Trim(" "c)
        strReturn = Replace(strReturn, "*", "")
        strReturn = Replace(strReturn, "%", "")
        Return strReturn
    End Function

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
        txtVon.Text = calAbDatum.SelectedDate.ToShortDateString
    End Sub

    Private Sub calBisDatum_SelectionChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles calBisDatum.SelectionChanged
        calBisDatum.Visible = False
        txtBis.Text = calBisDatum.SelectedDate.ToShortDateString
    End Sub

    Private Sub lb_Search_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lb_Search.Click
        DoSubmit()
    End Sub

    Private Sub lnkCreateExcel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lnkCreateExcel.Click

        Dim excelFactory As New Base.Kernel.DocumentGeneration.ExcelDocumentFactory()
        Dim strFileName As String = Format(Now, "yyyyMMdd_HHmmss_") & m_User.UserName

        objHandler.CreateExcelFromFieldTranslation(Session(Replace(Request.Url.LocalPath, "/Portal", "..")), objHandler.ResultSummary)
        excelFactory.CreateDocumentAndSendAsResponse(strFileName, objHandler.ResultExcel, Me.Page)

        
    End Sub

    Private Sub DataGrid1_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles DataGrid1.ItemCommand
        If e.CommandName = "Select" Then
            Dim strRedirect As String = "Change80_2.aspx?AppID=" & Session("AppID").ToString & "&Gebiet=" & e.Item.Cells(0).Text & "&Kunnr=" & e.Item.Cells(1).Text
            strRedirect = Replace(strRedirect, "&nbsp;", "")
            Response.Redirect(strRedirect)
        End If
    End Sub

    Private Sub DataGrid1_SortCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs) Handles DataGrid1.SortCommand

        FillGrid(0, e.SortExpression)
        
    End Sub

    Private Sub DataGrid1_PageIndexChanged(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles DataGrid1.PageIndexChanged

        FillGrid(e.NewPageIndex)
        
    End Sub
End Class

' ************************************************
' $History: Change80.aspx.vb $
' 
' *****************  Version 3  *****************
' User: Jungj        Date: 5.06.09    Time: 13:51
' Updated in $/CKAG/Components/ComCommon
' Try Catch entfernt wenn möglich
' 
' *****************  Version 2  *****************
' User: Rudolpho     Date: 10.04.08   Time: 17:37
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
' *****************  Version 4  *****************
' User: Uha          Date: 27.09.07   Time: 10:27
' Updated in $/CKG/Components/ComCommon/ComCommonWeb
' Controlnamen für Feldübersetzungen geändert
' 
' *****************  Version 3  *****************
' User: Uha          Date: 26.09.07   Time: 13:22
' Updated in $/CKG/Components/ComCommon/ComCommonWeb
' Bugfixing in ITA 1237, 1181 und 1238 (Alle Floorcheck)
' 
' *****************  Version 2  *****************
' User: Uha          Date: 20.09.07   Time: 16:34
' Updated in $/CKG/Components/ComCommon/ComCommonWeb
' ITA 1181: Testversion
' 
' *****************  Version 1  *****************
' User: Uha          Date: 19.09.07   Time: 17:29
' Created in $/CKG/Components/ComCommon/ComCommonWeb
' ITA 1181: Funktionslose Rohversion
' 
' ************************************************
