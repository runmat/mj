Imports CKG.Portal.PageElements
Imports CKG.Base.Kernel.Common.Common

Public Class Report_002_01
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

    'Private m_App As Base.Kernel.Security.App
    Private m_User As Base.Kernel.Security.User
    Private m_objTable As DataTable

    Protected WithEvents lblError As System.Web.UI.WebControls.Label
    Protected WithEvents ShowScript As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents lblPageTitle As System.Web.UI.WebControls.Label
    Protected WithEvents lblDownloadTip As System.Web.UI.WebControls.Label
    Protected WithEvents lnkExcel As System.Web.UI.WebControls.HyperLink
    Protected WithEvents ucHeader As Header
    Protected WithEvents Datagrid1 As System.Web.UI.WebControls.DataGrid
    Protected WithEvents lblHead As System.Web.UI.WebControls.Label
    Protected WithEvents tblBanner As System.Web.UI.HtmlControls.HtmlTable
    Protected WithEvents lblNoData As System.Web.UI.WebControls.Label
    Protected WithEvents ddlPageSize As System.Web.UI.WebControls.DropDownList
    Protected WithEvents cmdweiter As System.Web.UI.WebControls.LinkButton
    Protected WithEvents ucStyles As Styles

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'cmdSave.Enabled = False
        m_User = GetUser(Me)
        ucHeader.InitUser(m_User)
        FormAuth(Me, m_User)
        If (Session("ResultTable") Is Nothing) Then
            Response.Redirect(Request.UrlReferrer.ToString)
        Else
            m_objTable = CType(Session("ResultTable"), DataTable)
        End If

        If Not (Session("Link")) = Nothing Then
            If Session("Link") = "Report_002" Then
                lblHead.Text = "Sicherungsscheine"
            Else
                lblHead.Text = m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString
            End If
        Else
            lblHead.Text = m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString
        End If

        ucStyles.TitleText = lblHead.Text
        Try
            'm_App = New Base.Kernel.Security.App(m_User)
            If InStr(Request.UrlReferrer.ToString, "Selection.aspx") Then
                cmdweiter.Visible = False
            End If
            If Not IsPostBack Then
                ddlPageSize.Items.Add("10")
                ddlPageSize.Items.Add("20")
                ddlPageSize.Items.Add("50")
                ddlPageSize.Items.Add("100")
                ddlPageSize.Items.Add("200")
                ddlPageSize.Items.Add("500")
                ddlPageSize.Items.Add("1000")
                ddlPageSize.SelectedIndex = 0

                If (Not Session("ShowLink") Is Nothing) AndAlso Session("ShowLink").ToString = "True" Then
                    'lnkKreditlimit.Visible = True
                    'lnkKreditlimit.NavigateUrl = Request.UrlReferrer.ToString
                End If
                If Not Session("lnkExcel").ToString.Length = 0 Then
                    lblDownloadTip.Visible = True
                    lnkExcel.Visible = True
                    lnkExcel.NavigateUrl = Session("lnkExcel").ToString
                End If
                FillGrid(0)
            End If
        Catch ex As Exception
            lblError.Text = "Beim Laden der Seite ist ein Fehler aufgetreten."
        End Try
    End Sub

    Private Sub FillGrid(ByVal intPageIndex As Int32, Optional ByVal strSort As String = "")
        If m_objTable.Rows.Count = 0 Then
            Datagrid1.Visible = False
            lblNoData.Visible = True
            lblNoData.Text = "Keine Daten zur Anzeige gefunden."
            ShowScript.Visible = False
            Response.Redirect("Input_002_01.aspx?AppID=" & Session("AppID").ToString)
        Else
            Try
                Datagrid1.Visible = True
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

                Datagrid1.CurrentPageIndex = intTempPageIndex
                Datagrid1.DataSource = tmpDataView
                Datagrid1.DataBind()

                If (Not Session("ShowOtherString") Is Nothing) AndAlso CStr(Session("ShowOtherString")).Length > 0 Then
                    lblNoData.Text = CStr(Session("ShowOtherString"))
                Else
                    lblNoData.Text = "Es wurden " & tmpDataView.Count.ToString & " Einträge zu """ & m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString & """ gefunden."
                End If
                If (Not Session("BackLink") Is Nothing) AndAlso CStr(Session("BackLink")) = "HistoryBack" Then
                    'lnkKreditlimit.Text = "Zurück"
                    'lnkKreditlimit.NavigateUrl = "javascript:history.back()"
                End If
                lblNoData.Visible = True

                If Datagrid1.PageCount > 1 Then
                    Datagrid1.PagerStyle.CssClass = "PagerStyle"
                    Datagrid1.DataBind()
                    Datagrid1.PagerStyle.Visible = True
                Else
                    Datagrid1.PagerStyle.Visible = False
                End If


                Dim item As DataGridItem
                Dim cell As TableCell
                Dim iCol As Integer
                For iCol = 0 To Datagrid1.Columns.Count - 1
                    For Each item In Datagrid1.Items                   'Zeilen des DataGrids durchgehen...
                        For Each cell In item.Cells
                            If IsDate(cell.Text) Then
                                cell.Text = FormatDateTime(CDate(cell.Text), DateFormat.ShortDate)
                            End If
                        Next
                    Next
                Next
            Catch ex As Exception
                lblError.Text = "Fehler beim Anzeigen der Daten."
            End Try
        End If
    End Sub

    Private Sub ddlPageSize_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ddlPageSize.SelectedIndexChanged
        Datagrid1.PageSize = CInt(ddlPageSize.SelectedItem.Value)
        FillGrid(0)
    End Sub

    Private Sub Datagrid1_SortCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs) Handles Datagrid1.SortCommand
        FillGrid(Datagrid1.CurrentPageIndex, e.SortExpression)
    End Sub

    Private Sub Datagrid1_PageIndexChanged(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles Datagrid1.PageIndexChanged
        FillGrid(e.NewPageIndex)
    End Sub

    Private Sub Datagrid1_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles Datagrid1.ItemDataBound

        Dim typ As String

        typ = Request.Item("typ")

        tblBanner.Visible = False

        e.Item.Cells(2).Visible = False     'Equipmentnr ausblenden
        If (typ = "H") Then
            lblPageTitle.Text = "Historie"
            e.Item.Cells(13).Visible = False     'Klärfallspalte am Ende ausblenden
            e.Item.Cells(14).Visible = False     'KlärfallspalteInfo am Ende ausblenden
        End If
        If (typ = "M") Then
            lblPageTitle.Text = "Mahnungen"
            e.Item.Cells(14).Visible = False     'Klärfallspalte am Ende ausblenden 
            e.Item.Cells(15).Visible = False     'KlärfallspalteInfo am Ende ausblenden
        End If
        If (typ = "HM") Then
            lblPageTitle.Text = "Klärfälle"
            e.Item.Cells(9).Visible = False     'Klärfallspalte am Ende ausblenden 
            tblBanner.Visible = True
        End If
    End Sub

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        SetEndASPXAccess(Me)
    End Sub

    Private Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Unload
        SetEndASPXAccess(Me)
    End Sub

    Private Sub cmdweiter_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdweiter.Click
        Session("Link") = "Report_002"
        Response.Redirect("Input_002_01.aspx?AppID=" & Session("AppID").ToString)
    End Sub
End Class

' ************************************************
' $History: Report_002_01.aspx.vb $
' 
' *****************  Version 3  *****************
' User: Rudolpho     Date: 2.05.08    Time: 11:54
' Updated in $/CKAG/Applications/appasl/Forms
' Migration
' 
' *****************  Version 2  *****************
' User: Rudolpho     Date: 24.04.08   Time: 14:28
' Updated in $/CKAG/Applications/appasl/Forms
' Migration
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 7.04.08    Time: 11:04
' Created in $/CKAG/Applications/appasl/Forms
' 
' *****************  Version 10  *****************
' User: Rudolpho     Date: 10.03.08   Time: 10:15
' Updated in $/CKG/Applications/AppASL/AppASLWeb/Forms
' 
' *****************  Version 9  *****************
' User: Uha          Date: 2.07.07    Time: 10:11
' Updated in $/CKG/Applications/AppASL/AppASLWeb/Forms
' Logging der Laufzeiten der ASPX-Seiten eingeführt
' 
' *****************  Version 8  *****************
' User: Uha          Date: 6.03.07    Time: 18:02
' Updated in $/CKG/Applications/AppASL/AppASLWeb/Forms
' 
' ************************************************
