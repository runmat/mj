Imports CKG.Base.Business
Imports CKG.Base.Kernel
Imports CKG.Services.PageElements
Imports CKG.Base.Kernel.Common.Common
Imports CKG


Partial Public Class Report_002_01
    Inherits System.Web.UI.Page

    Private m_App As Base.Kernel.Security.App
    Private m_User As Base.Kernel.Security.User
    Private m_objTable As DataTable


    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'cmdSave.Enabled = False
        m_User = GetUser(Me)
        FormAuth(Me, m_User)
        GridNavigation1.setGridElment(DataGrid1)

        If (Session("ResultTable") Is Nothing) Then
            Response.Redirect(Request.UrlReferrer.ToString)
        Else
            m_objTable = CType(Session("ResultTable"), DataTable)
        End If
        lblHead.Text = m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString
        Try
            m_App = New Base.Kernel.Security.App(m_User)

            If Not IsPostBack Then

                If (Not Session("ShowLink") Is Nothing) AndAlso Session("ShowLink").ToString = "True" Then
                    'lnkKreditlimit.Visible = True
                    'lnkKreditlimit.NavigateUrl = Request.UrlReferrer.ToString
                End If
                If Not Session("lnkExcel").ToString.Length = 0 Then
                    lnkExcel.Visible = True
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
            End If
            If (Not Session("BackLink") Is Nothing) AndAlso CStr(Session("BackLink")) = "HistoryBack" Then
                'lnkKreditlimit.Text = "Zurück"
                'lnkKreditlimit.NavigateUrl = "javascript:history.back()"
            End If
            lblNoData.Visible = True

        End If
    End Sub


    Private Sub Datagrid1_SortCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs) Handles DataGrid1.SortCommand
        FillGrid(0, e.SortExpression)
    End Sub

    Private Sub PagerChanged(ByVal pageindex As Int32) Handles GridNavigation1.PagerChanged
        FillGrid(pageindex)
    End Sub

    Private Sub GridNavigation1_ddlPageSizeChanged() Handles GridNavigation1.PageSizeChanged
        FillGrid(0)
    End Sub



    Private Sub Datagrid1_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles DataGrid1.ItemDataBound
        Dim typ As String = ""

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

    Protected Sub lnkCreateExcel_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lnkExcel.Click
        Try
            Dim excelFactory As New Base.Kernel.DocumentGeneration.ExcelDocumentFactory()
            Dim strFileName As String = Format(Now, "yyyyMMdd_HHmmss_") & m_User.UserName

            excelFactory.CreateDocumentAndSendAsResponse(strFileName, CType(Session("ExcelTable"), DataTable), Me.Page)

        Catch ex As Exception
            lblError.Text = "Fehler beim Erstellen der Excel-Datei: " + ex.Message
            lblError.Visible = True
        End Try
    End Sub

End Class

' ************************************************
' $History: Report_002_01.aspx.vb $
' 
' *****************  Version 1  *****************
' User: Jungj        Date: 22.04.09   Time: 12:34
' Created in $/CKAG2/Applications/AppArval/forms
' 
' *****************  Version 2  *****************
' User: Rudolpho     Date: 16.04.08   Time: 11:53
' Updated in $/CKAG/Applications/apparval/Forms
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 4.04.08    Time: 16:18
' Created in $/CKAG/Applications/apparval/Forms
' 
' *****************  Version 4  *****************
' User: Uha          Date: 2.07.07    Time: 10:05
' Updated in $/CKG/Applications/AppARVAL/AppARVALWeb/Forms
' Logging der Laufzeiten der ASPX-Seiten eingeführt
' 
' *****************  Version 3  *****************
' User: Uha          Date: 6.03.07    Time: 14:48
' Updated in $/CKG/Applications/AppARVAL/AppARVALWeb/Forms
' 
' ************************************************
