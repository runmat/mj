Imports CKG.Base.Business
Imports CKG.Base.Kernel
Imports CKG.Portal.PageElements
Imports CKG.Base.Kernel.Common.Common

Public Class Report31
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
    Private Result As DataTable
    Private ResultTable As DataTable

    Protected WithEvents lblError As System.Web.UI.WebControls.Label
    Protected WithEvents lnkKreditlimit As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lblPageTitle As System.Web.UI.WebControls.Label
    Protected WithEvents ucHeader As Header
    Protected WithEvents ddlPageSize As System.Web.UI.WebControls.DropDownList
    Protected WithEvents lblNoData As System.Web.UI.WebControls.Label
    Protected WithEvents DataGrid1 As System.Web.UI.WebControls.DataGrid
    Protected WithEvents ucStyles As Styles
    Protected WithEvents lblDownloadTip As System.Web.UI.WebControls.Label
    Protected WithEvents lnkCreateExcel As System.Web.UI.WebControls.LinkButton
    Protected WithEvents Datagrid2 As System.Web.UI.WebControls.DataGrid
    Protected WithEvents Datagrid3 As System.Web.UI.WebControls.DataGrid
    Protected WithEvents lblHead As System.Web.UI.WebControls.Label

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        m_User = GetUser(Me)
        ucHeader.InitUser(m_User)
        FormAuth(Me, m_User)

        GetAppIDFromQueryString(Me)

        lnkKreditlimit.NavigateUrl = "Report30.aspx?AppID=" & Session("AppID").ToString

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
            FillGrid(0, , True)
            FillGrid2()
        End If

    End Sub

    Private Sub FillGrid2()
        Dim tmpDataView As New DataView()

        ResultTable = CType(Session("ResultTable"), DataTable)

        If (ResultTable Is Nothing) OrElse (ResultTable.Rows.Count = 0) Then
            Datagrid2.Visible = False
            ddlPageSize.Visible = False
            lblNoData.Text = "Keine Daten zur Anzeige gefunden."
            lblNoData.Visible = True
            'ShowScript.Visible = False
        Else
            tmpDataView = ResultTable.DefaultView
            tmpDataView.RowFilter = "Suche = 1"

            Datagrid2.DataSource = tmpDataView
            Datagrid2.DataBind()

            tmpDataView = ResultTable.DefaultView
            tmpDataView.RowFilter = "Suche = 0"

            Datagrid3.DataSource = tmpDataView
            Datagrid3.DataBind()
        End If
    End Sub

    Private Sub FillGrid(ByVal intPageIndex As Int32, Optional ByVal strSort As String = "", Optional ByVal fill As Boolean = False)
        Dim tmpDataView As New DataView()

        Result = CType(Session("Result"), DataTable)

        If (Result Is Nothing) OrElse (Result.Rows.Count = 0) Then
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
            tmpDataView = Result.DefaultView
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
            If Result.Rows.Count = 1 Then
                lblNoData.Text = "Es wurde 1 Vorgang gefunden."
            Else
                lblNoData.Text = "Es wurden " & tmpDataView.Count.ToString & "  Vorgänge gefunden."
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
            For Each item In DataGrid1.Items
                For Each cell In item.Cells
                    cell.Text = Replace(cell.Text, "00:00:00", "")
                Next
            Next
        End If
    End Sub

    Private Sub ddlPageSize_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ddlPageSize.SelectedIndexChanged
        DataGrid1.PageSize = CInt(ddlPageSize.SelectedItem.Value)
        FillGrid(0)
    End Sub

    Private Sub DataGrid1_PageIndexChanged(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles DataGrid1.PageIndexChanged
        FillGrid(e.NewPageIndex)
    End Sub

    Private Sub DataGrid1_SortCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs) Handles DataGrid1.SortCommand
        FillGrid(DataGrid1.CurrentPageIndex, e.SortExpression)
    End Sub

    Private Sub lnkCreateExcel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lnkCreateExcel.Click

        Result = CType(Session("Result"), DataTable)
        Result.TableName = "Details"
        ResultTable = CType(Session("ResultTable"), DataTable)
        Dim tmpTable As DataTable = ResultTable.Copy
        tmpTable.Columns.RemoveAt(ResultTable.Columns.Count - 1)
        tmpTable.TableName = "Zusammenfassung"

        If ((Not Result Is Nothing) AndAlso (Result.Rows.Count > 0)) And ((Not ResultTable Is Nothing) AndAlso (ResultTable.Rows.Count > 0)) Then
            Dim excelFactory As New Base.Kernel.DocumentGeneration.ExcelDocumentFactory()
            Dim strFileName As String = Format(Now, "yyyyMMdd_HHmmss_") & m_User.UserName

            Dim dsResult As New DataSet()
            dsResult.Tables.Add(tmpTable)
            dsResult.Tables.Add(Result)
            excelFactory.CreateDocumentAndSendAsResponse(strFileName, dsResult, Me.Page)
        End If
    End Sub

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        SetEndASPXAccess(Me)
    End Sub

    Private Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Unload
        SetEndASPXAccess(Me)
    End Sub
End Class

' ************************************************
' $History: Report31.aspx.vb $
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
' *****************  Version 5  *****************
' User: Uha          Date: 9.08.07    Time: 16:39
' Updated in $/CKG/Components/ComCommon/ComCommonWeb
' Bugfixing im Excelreport von "Vergabe von Feinstaubplaketten"
' 
' *****************  Version 4  *****************
' User: Uha          Date: 9.08.07    Time: 13:18
' Updated in $/CKG/Components/ComCommon/ComCommonWeb
' Oberflächenänderung in "Vergabe von Feinstaubplaketten"
' 
' *****************  Version 3  *****************
' User: Uha          Date: 12.07.07   Time: 13:40
' Updated in $/CKG/Components/ComCommon/ComCommonWeb
' Report "Vergabe von Feinstaubplaketten" als Testversion erzeugt
' 
' *****************  Version 2  *****************
' User: Uha          Date: 12.07.07   Time: 9:28
' Updated in $/CKG/Components/ComCommon/ComCommonWeb
' Logging der Laufzeiten der ASPX-Seiten eingeführt
' 
' *****************  Version 1  *****************
' User: Uha          Date: 11.07.07   Time: 18:22
' Created in $/CKG/Components/ComCommon/ComCommonWeb
' Report "Vergabe von Feinstaubplaketten" im Rohbau erzeugt
' 
' ************************************************
