Imports CKG.Base.Business
Imports CKG.Base.Kernel
Imports CKG.Portal.PageElements
Imports CKG.Base.Kernel.Common.Common

Public Class Report012
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

    Private m_User As Security.User
    Private m_App As Security.App
    'Private objSuche As Search
    Private objHandler As Kernel.Monatslisten

    Protected WithEvents lblError As System.Web.UI.WebControls.Label
    Protected WithEvents lnkKreditlimit As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lblPageTitle As System.Web.UI.WebControls.Label
    Protected WithEvents ucHeader As Header
    Protected WithEvents ddlPageSize As System.Web.UI.WebControls.DropDownList
    Protected WithEvents lblNoData As System.Web.UI.WebControls.Label
    Protected WithEvents DataGrid1 As System.Web.UI.WebControls.DataGrid
    Protected WithEvents ucStyles As Styles
    Protected WithEvents lnkDruck As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lnkCreateExcel As System.Web.UI.WebControls.LinkButton
    Protected WithEvents lblHead As System.Web.UI.WebControls.Label

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        m_User = GetUser(Me)
        ucHeader.InitUser(m_User)
        FormAuth(Me, m_User)

        Dim mon As Integer
        Dim yr As Integer

        If (Request.QueryString.Item("M") <> Nothing) Then  'Monat holen
            mon = CType(Request.QueryString.Item("M"), Integer)
        End If
        If (Request.QueryString.Item("Y") <> Nothing) Then  'Jahr holen
            yr = CType(Request.QueryString.Item("Y"), Integer)
        End If

        GetAppIDFromQueryString(Me)

        lnkKreditlimit.NavigateUrl = "Report01.aspx?AppID=" & Session("AppID").ToString

        lblHead.Text = m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString
        ucStyles.TitleText = lblHead.Text
        m_App = New Security.App(m_User)

        If Not IsPostBack Then
            ddlPageSize.Items.Add("10")
            ddlPageSize.Items.Add("20")
            ddlPageSize.Items.Add("50")
            ddlPageSize.Items.Add("100")
            ddlPageSize.Items.Add("250")
            ddlPageSize.SelectedIndex = 2
            DoSubmit(mon, yr)
            FillGrid(0, , True)
        End If


    End Sub

    Private Sub DoSubmit(ByVal mon As Integer, ByVal yr As Integer)
        Dim dat As Date
        Dim strFileName As String = Format(Now, "yyyyMMdd_HHmmss_") & m_User.UserName
        dat = New Date(yr, mon, 1)

        Session("lnkExcel") = Nothing

        objHandler = New Kernel.Monatslisten(m_User, m_App, strFileName)
        objHandler.day_start = dat.ToShortDateString
        objHandler.day_end = New Date(yr, mon, Date.DaysInMonth(yr, mon)).ToShortDateString
        objHandler.FILL(Session("AppID").ToString, Session.SessionID)

        If Session("ResultTable") Is Nothing Then
            Session.Add("ResultTable", objHandler)
        Else
            Session("ResultTable") = objHandler
        End If
        'Excel-Datei + Link erzeugen
        If (objHandler.Result Is Nothing OrElse objHandler.Result.Rows.Count = 0) Then
            lnkDruck.Visible = False
            lnkCreateExcel.Visible = False
        Else
            Session("lnkExcel") = "/Portal/Temp/Excel/" & strFileName & ".xls"
            lnkDruck.NavigateUrl = "Report013.aspx?AppID=" & Session("AppID").ToString
            lnkDruck.Target = "_blank"
            lnkDruck.Visible = True
            lnkCreateExcel.Visible = True
        End If
    End Sub

    Private Sub FillGrid(ByVal intPageIndex As Int32, Optional ByVal strSort As String = "", Optional ByVal fill As Boolean = False)
        Dim tmpDataView As New DataView()

        objHandler = CType(Session("ResultTable"), Kernel.Monatslisten)

        If (objHandler.Result Is Nothing) OrElse (objHandler.Result.Rows.Count = 0) Then
            DataGrid1.Visible = False
            ddlPageSize.Visible = False
            lblNoData.Text = "Keine Daten zur Anzeige gefunden."
            lblNoData.Visible = True
            'ShowScript.Visible = False
        Else
            Dim intTempPageIndex As Int32 = intPageIndex
            Dim strTempSort As String = ""
            Dim strDirection As String = ""

            ddlPageSize.Visible = True
            tmpDataView = objHandler.Result.DefaultView
            tmpDataView.RowFilter = ""

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

            lblNoData.Text = "Es wurde(n) " & tmpDataView.Count.ToString & "  Überführung(en) gefunden."
            lblNoData.Visible = True

            If DataGrid1.PageCount > 1 Then
                DataGrid1.PagerStyle.CssClass = "PagerStyle"
                DataGrid1.DataBind()
                DataGrid1.PagerStyle.Visible = True
            Else
                DataGrid1.PagerStyle.Visible = False
            End If


            'Dim item As DataGridItem
            'Dim cell As TableCell
            'Dim chkBox As CheckBox
            'Dim control As control
            'Dim blnScriptFound As Boolean = False
            'Dim intZaehl As Int32

            'For Each item In DataGrid1.Items
            '    intZaehl = 1
            '    Dim strKKB As String = "0"
            '    Dim blnBezahlt As Boolean

            '    For Each cell In item.Cells
            '        If intZaehl = 2 Then
            '            strKKB = cell.Text
            '        End If
            '        For Each control In cell.Controls
            '            If TypeOf control Is CheckBox Then
            '                chkBox = CType(control, CheckBox)
            '                Select Case chkBox.ID
            '                    Case "chk0000"
            '                        If strKKB = "99" Then
            '                            chkBox.Checked = True
            '                        End If
            '                End Select
            '            End If
            '        Next
            '        intZaehl += 1
            '    Next
            'Next
            'If objHandler.Fahrzeuge.Select("MANDT='11'").GetUpperBound(0) > -1 Then
            '    DataGrid1.Columns(DataGrid1.Columns.Count - 1).Visible = True
            'Else
            '    DataGrid1.Columns(DataGrid1.Columns.Count - 1).Visible = False
            'End If
        End If
    End Sub

    Private Sub ddlPageSize_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ddlPageSize.SelectedIndexChanged
        DataGrid1.PageSize = CInt(ddlPageSize.SelectedItem.Value)
        FillGrid(0)
    End Sub

    Private Sub DataGrid1_PageIndexChanged(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles DataGrid1.PageIndexChanged
        'CheckGrid()
        FillGrid(e.NewPageIndex)
    End Sub

    Private Sub DataGrid1_SortCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs) Handles DataGrid1.SortCommand
        'CheckGrid()
        FillGrid(DataGrid1.CurrentPageIndex, e.SortExpression)
    End Sub

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        SetEndASPXAccess(Me)
    End Sub

    Private Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Unload
        SetEndASPXAccess(Me)
    End Sub

    Private Sub lnkCreateExcel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lnkCreateExcel.Click

        objHandler = CType(Session("ResultTable"), Kernel.Monatslisten)
        Dim excelFactory As New Base.Kernel.DocumentGeneration.ExcelDocumentFactory()
        Dim strFileName As String = Format(Now, "yyyyMMdd_HHmmss_") & m_User.UserName
        excelFactory.CreateDocumentAndSendAsResponse(strFileName, objHandler.Result, Me.Page)
      
    End Sub
End Class

' ************************************************
' $History: Report012.aspx.vb $
' 
' *****************  Version 4  *****************
' User: Jungj        Date: 5.06.09    Time: 15:58
' Updated in $/CKAG/Components/ComCommon
' mögliche try catches entfernt
' 
' *****************  Version 3  *****************
' User: Rudolpho     Date: 11.04.08   Time: 13:43
' Updated in $/CKAG/Components/ComCommon
' Migration
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
' *****************  Version 9  *****************
' User: Rudolpho     Date: 5.12.07    Time: 16:31
' Updated in $/CKG/Components/ComCommon/ComCommonWeb
' ITA: 1413
' 
' *****************  Version 8  *****************
' User: Uha          Date: 20.09.07   Time: 16:34
' Updated in $/CKG/Components/ComCommon/ComCommonWeb
' ITA 1181: Testversion
' 
' *****************  Version 7  *****************
' User: Uha          Date: 12.07.07   Time: 9:28
' Updated in $/CKG/Components/ComCommon/ComCommonWeb
' Logging der Laufzeiten der ASPX-Seiten eingeführt
' 
' *****************  Version 6  *****************
' User: Uha          Date: 22.05.07   Time: 9:45
' Updated in $/CKG/Components/ComCommon/ComCommonWeb
' Nacharbeiten + Bereinigungen
' 
' *****************  Version 5  *****************
' User: Uha          Date: 15.03.07   Time: 12:49
' Updated in $/CKG/Components/ComCommon/ComCommonWeb
' Image- und Excel-Pfade korrigiert
' 
' *****************  Version 4  *****************
' User: Uha          Date: 5.03.07    Time: 14:57
' Updated in $/CKG/Components/ComCommon/ComCommonWeb
' 
' ************************************************
