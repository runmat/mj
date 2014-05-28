Imports CKG.Base.Business
Imports CKG.Base.Kernel
Imports CKG.Portal.PageElements
Imports CKG.Base.Kernel.Common.Common

Public Class Change80_2
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
    Private m_strFilter As String

    Protected WithEvents lblError As System.Web.UI.WebControls.Label
    Protected WithEvents lblPageTitle As System.Web.UI.WebControls.Label
    Protected WithEvents ucHeader As Header
    Protected WithEvents lblNoData As System.Web.UI.WebControls.Label
    Protected WithEvents ucStyles As Styles
    Protected WithEvents cmdBack As System.Web.UI.WebControls.LinkButton
    Protected WithEvents lnkCreateExcel As System.Web.UI.WebControls.LinkButton
    Protected WithEvents Datagrid2 As System.Web.UI.WebControls.DataGrid
    Protected WithEvents DataGrid1 As System.Web.UI.WebControls.DataGrid
    Protected WithEvents lb_Back As System.Web.UI.WebControls.LinkButton
    Protected WithEvents lbl_Gebiet As System.Web.UI.WebControls.Label
    Protected WithEvents lbl_Kunnr As System.Web.UI.WebControls.Label
    Protected WithEvents tr_Gebiet As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents tr_Kunnr As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents lblGebietShow As System.Web.UI.WebControls.Label
    Protected WithEvents lblKunnrShow As System.Web.UI.WebControls.Label
    Protected WithEvents lblVbeln As System.Web.UI.WebControls.Label
    Protected WithEvents lnkCreateExcel2 As System.Web.UI.WebControls.LinkButton
    Protected WithEvents lblHead As System.Web.UI.WebControls.Label

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        m_User = GetUser(Me)
        ucHeader.InitUser(m_User)
        FormAuth(Me, m_User)

        GetAppIDFromQueryString(Me)

        lblHead.Text = m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString
        If m_User.Organization.OrganizationAdmin Then
            lblHead.Text = Replace(lblHead.Text, " (Beauftragung)", "")
        End If
        ucStyles.TitleText = lblHead.Text
        m_App = New Base.Kernel.Security.App(m_User)

        If Session("objHandler") Is Nothing Then
            Response.Redirect("Change80.aspx?AppID=" & Session("AppID").ToString)
        End If

        objHandler = CType(Session("objHandler"), Change_80)

        If Not Request.QueryString.Item("Gebiet") Is Nothing AndAlso CStr(Request.QueryString.Item("Gebiet")).Length > 0 Then
            m_strFilter = "Gebiet='" & CStr(Request.QueryString.Item("Gebiet")) & "'"
        Else
            m_strFilter = "Gebiet=''"
        End If
        If Not Request.QueryString.Item("Kunnr") Is Nothing AndAlso CStr(Request.QueryString.Item("Kunnr")).Length > 0 Then
            m_strFilter &= " AND Kunnr='" & CStr(Request.QueryString.Item("Kunnr")) & "'"
        Else
            m_strFilter &= " AND Kunnr=''"
        End If

        If Not IsPostBack Then
            DataGrid1.BackColor = System.Drawing.Color.White
            FillGrid(0)
        End If

    End Sub

    Private Sub FillGrid(ByVal intPageIndex As Int32, Optional ByVal strSort As String = "")
        Dim tmpDataView As New DataView()
        tmpDataView = objHandler.Result.DefaultView
        tmpDataView.RowFilter = m_strFilter

        If tmpDataView.Count = 0 Then
            DataGrid1.Visible = False
            lblNoData.Text = "Keine Daten zur Anzeige gefunden."
            lblNoData.Visible = True
        Else
            DataGrid1.Visible = True
            lblGebietShow.Text = tmpDataView(0)("Gebiet")
            lblKunnrShow.Text = tmpDataView(0)("Kunnr")
            If CStr(tmpDataView(0)("Name1") & " " & tmpDataView(0)("Name2")).Trim(" "c).Length > 0 Then
                lblKunnrShow.Text &= ", " & tmpDataView(0)("Name1") & " " & tmpDataView(0)("Name2")
            End If
            If CStr(tmpDataView(0)("Post_code1") & " " & tmpDataView(0)("City1")).Trim(" "c).Length > 0 Then
                lblKunnrShow.Text &= ", " & tmpDataView(0)("Post_code1") & " " & tmpDataView(0)("City1")
            End If

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

            Dim item As DataGridItem
            For Each item In DataGrid1.Items
                If lblVbeln.Text = item.Cells(0).Text Then
                    item.CssClass = "GridTableHighlight"
                End If
            Next
        End If
    End Sub

    Private Sub FillGrid2(ByVal intPageIndex As Int32, Optional ByVal strSort As String = "")
        Dim tmpDataView As New DataView()
        tmpDataView = objHandler.ResultPositionen.DefaultView
        tmpDataView.RowFilter = "Vbeln='" & lblVbeln.Text & "'"

        If tmpDataView.Count = 0 Then
            Datagrid2.Visible = False
            lnkCreateExcel2.Visible = False
            lblNoData.Text = "Keine Daten zur Anzeige gefunden."
            lblNoData.Visible = True
        Else
            Datagrid2.Visible = True
            lnkCreateExcel2.Visible = True

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

            Datagrid2.CurrentPageIndex = intTempPageIndex

            Datagrid2.DataSource = tmpDataView
            Datagrid2.DataBind()

            If Datagrid2.PageCount > 1 Then
                Datagrid2.PagerStyle.CssClass = "PagerStyle"
                Datagrid2.DataBind()
                Datagrid2.PagerStyle.Visible = True
            Else
                Datagrid2.PagerStyle.Visible = False
            End If
        End If
    End Sub

    Private Sub DataGrid1_PageIndexChanged(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles DataGrid1.PageIndexChanged

        FillGrid(e.NewPageIndex)
        
    End Sub

    Private Sub DataGrid1_SortCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs) Handles DataGrid1.SortCommand

        FillGrid(0, e.SortExpression)
       
    End Sub

    Private Sub lnkCreateExcel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lnkCreateExcel.Click

        Dim excelFactory As New Base.Kernel.DocumentGeneration.ExcelDocumentFactory()
        Dim strFileName As String = Format(Now, "yyyyMMdd_HHmmss_") & m_User.UserName

        objHandler.CreateExcelFromFieldTranslation(Session(Replace(Request.Url.LocalPath, "/Portal", "..")), objHandler.Result, m_strFilter)
        excelFactory.CreateDocumentAndSendAsResponse(strFileName, objHandler.ResultExcel, Me.Page)


    End Sub

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        SetEndASPXAccess(Me)
    End Sub

    Private Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Unload
        SetEndASPXAccess(Me)
    End Sub

    Private Sub lb_Back_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lb_Back.Click
        Response.Redirect("Change80.aspx?AppID=" & Session("AppID").ToString)
    End Sub

    Private Sub DataGrid1_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles DataGrid1.ItemCommand
        If e.CommandName = "Select" Then
            lblVbeln.Text = e.Item.Cells(0).Text
            FillGrid2(0)
            FillGrid(DataGrid1.CurrentPageIndex)
        End If
    End Sub

    Private Sub Datagrid2_SortCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs) Handles Datagrid2.SortCommand

        FillGrid2(0, e.SortExpression)

    End Sub

    Private Sub Datagrid2_PageIndexChanged(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles Datagrid2.PageIndexChanged

        FillGrid2(e.NewPageIndex)
       
    End Sub

    Private Sub lnkCreateExcel2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lnkCreateExcel2.Click

        Dim excelFactory As New Base.Kernel.DocumentGeneration.ExcelDocumentFactory()
        Dim strFileName As String = Format(Now, "yyyyMMdd_HHmmss_") & m_User.UserName

        objHandler.CreateExcelFromFieldTranslation(Session(Replace(Request.Url.LocalPath, "/Portal", "..")), objHandler.ResultPositionen, "Vbeln='" & lblVbeln.Text & "'")
        excelFactory.CreateDocumentAndSendAsResponse(strFileName, objHandler.ResultExcel, Me.Page)

    End Sub
End Class

' ************************************************
' $History: Change80_2.aspx.vb $
' 
' *****************  Version 4  *****************
' User: Jungj        Date: 5.06.09    Time: 13:51
' Updated in $/CKAG/Components/ComCommon
' Try Catch entfernt wenn möglich
' 
' *****************  Version 3  *****************
' User: Rudolpho     Date: 23.07.08   Time: 17:02
' Updated in $/CKAG/Components/ComCommon
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
' *****************  Version 5  *****************
' User: Uha          Date: 27.09.07   Time: 10:27
' Updated in $/CKG/Components/ComCommon/ComCommonWeb
' Controlnamen für Feldübersetzungen geändert
' 
' *****************  Version 4  *****************
' User: Uha          Date: 26.09.07   Time: 16:49
' Updated in $/CKG/Components/ComCommon/ComCommonWeb
' In Change01, Change03 und Change80 neues Format "GridTableHighlight"
' verwendet.
' 
' *****************  Version 3  *****************
' User: Uha          Date: 25.09.07   Time: 17:03
' Updated in $/CKG/Components/ComCommon/ComCommonWeb
' ITA 1124 hinzugefügt (Change03/Change03_2) und allgemeines Bugfix
' Floorcheck
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
