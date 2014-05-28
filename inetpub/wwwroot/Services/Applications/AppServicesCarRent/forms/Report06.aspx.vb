Imports CKG.Base.Business
Imports CKG.Base.Kernel
Imports CKG.Base.Kernel.Common
Imports CKG.Services

Partial Public Class Report06
    Inherits System.Web.UI.Page

#Region "Declarations"
    Private m_App As Security.App
    Private m_User As Security.User
    Private mMahnwesen As Mahnwesen

    Protected WithEvents GridNavigation1 As CKG.Services.GridNavigation

#End Region

#Region "Events"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        m_User = Common.GetUser(Me)
        Common.FormAuth(Me, m_User)
        Common.GetAppIDFromQueryString(Me)
        m_App = New Security.App(m_User)

        GridNavigation1.setGridElment(GridView1)
        If Not IsPostBack Then GridNavigation1.PageSizeIndex = 0

        lblHead.Text = m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString

        lblError.Text = ""
    End Sub

    Protected Sub lnkCreateExcel1_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lnkCreateExcel1.Click
        Dim TempTable As DataTable = CType(Session("ResultTable"), DataTable)
        Dim excelFactory As New CKG.Base.Kernel.DocumentGeneration.ExcelDocumentFactory()
        Dim strFileName As String = Format(Now, "yyyyMMdd_HHmmss_") & m_User.UserName


        TempTable.AcceptChanges()
        excelFactory.CreateDocumentAndSendAsResponse(strFileName, TempTable, Me.Page, , , , )
    End Sub
    Protected Sub lbCreate_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lbCreate.Click
        DoSubmit()
    End Sub

    Private Sub lbBack_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbBack.Click
        Response.Redirect("/Services/(S(" & Session.SessionID & "))/Start/Selection.aspx?AppID=" & Session("AppID").ToString, False)
    End Sub

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        Common.SetEndASPXAccess(Me)
        HelpProcedures.FixedGridViewCols(GridView1)
    End Sub

    Private Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Unload
        Common.SetEndASPXAccess(Me)
    End Sub
#End Region

#Region "Methods"
    Private Sub FillGrid(ByVal intPageIndex As Int32, Optional ByVal strSort As String = "")
        Dim tmpDataView As New DataView()
        Dim tmpDataTable As New DataTable

        tmpDataTable = Session("ResultTable")
        tmpDataView = tmpDataTable.DefaultView

        tmpDataView.RowFilter = ""
        If tmpDataView.Count = 0 Then
            Result.Visible = False
        Else

            Result.Visible = True
            Dim strTempSort As String = ""
            Dim strDirection As String = ""
            Dim intTempPageIndex As Int32 = intPageIndex

            If strSort.Trim(" "c).Length > 0 Then
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
            GridView1.PageIndex = intTempPageIndex
            GridView1.DataSource = tmpDataView
            GridView1.DataBind()

        End If

    End Sub

    Private Sub DoSubmit()
        Dim strFileName As String = ""
        mMahnwesen = New Mahnwesen(m_User, m_App, Session("AppID").ToString, Session.SessionID.ToString, strFileName)


        mMahnwesen.Fill(Session("AppID").ToString, Session.SessionID.ToString, Me)

        If mMahnwesen.Result Is Nothing Then
            lblError.Visible = True
            lblError.Text = "Keine Daten vorhanden."
        Else
            If Not mMahnwesen.Status = 0 Then
                lblError.Text = mMahnwesen.Message
            ElseIf mMahnwesen.Result.Rows.Count = 0 Then
                lblError.Visible = True
                lblError.Text = "Keine Daten vorhanden."
            Else
                Session("ResultTable") = mMahnwesen.Result
                FillGrid(0)
            End If
        End If
    End Sub

    Private Sub GridNavigation1_PagerChanged(ByVal PageIndex As Integer) Handles GridNavigation1.PagerChanged
        GridView1.PageIndex = PageIndex
        FillGrid(PageIndex)
    End Sub

    Private Sub GridNavigation1_PageSizeChanged() Handles GridNavigation1.PageSizeChanged
        FillGrid(0)
    End Sub
    Private Sub Gridview1_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles GridView1.Sorting
        FillGrid(GridView1.PageIndex, e.SortExpression)
    End Sub

#End Region

   
End Class
' ************************************************
' $History: Report06.aspx.vb $
' 
' *****************  Version 3  *****************
' User: Fassbenders  Date: 16.12.09   Time: 9:47
' Updated in $/CKAG2/Applications/AppServicesCarRent/forms
' 
' *****************  Version 2  *****************
' User: Fassbenders  Date: 16.12.09   Time: 9:46
' Updated in $/CKAG2/Applications/AppServicesCarRent/forms
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 15.12.09   Time: 16:52
' Created in $/CKAG2/Applications/AppServicesCarRent/forms
' ITA: 3381
' 