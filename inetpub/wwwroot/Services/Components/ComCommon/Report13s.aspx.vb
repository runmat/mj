Imports CKG.Base.Business
Imports CKG.Base.Kernel
Imports CKG.Base.Kernel.Common.Common

Partial Public Class Report13s
    Inherits System.Web.UI.Page

    Private m_User As Base.Kernel.Security.User
    Private m_App As Base.Kernel.Security.App
    Protected WithEvents GridNavigation1 As Global.CKG.Services.GridNavigation
    Private m_Report As Mahnstufe3

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        m_User = GetUser(Me)
        FormAuth(Me, m_User)
        GetAppIDFromQueryString(Me)

        lblHead.Text = m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString
        GridNavigation1.setGridElment(GridView1)
        m_App = New Base.Kernel.Security.App(m_User)

        If Not IsPostBack Then
            DoSubmit()
        End If



    End Sub
    Private Sub DoSubmit()
        Dim checkInput As Boolean = True
        Session("lnkExcel") = ""
        Try
            Dim strFileName As String = Format(Now, "yyyyMMdd_HHmmss_") & m_User.UserName & ".xls"
            m_Report = New Mahnstufe3(m_User, m_App, strFileName)

            lblError.Text = ""

            m_Report.Fill(Session("AppID").ToString, Session.SessionID.ToString, Me)
            Session("App_Report") = m_Report

            If Not m_Report.Status = 0 Then
                lblError.Text = "Fehler: " & m_Report.Message
            Else
                If m_Report.Result.Rows.Count = 0 Then
                    lblError.Text = "Keine Ergebnisse für die gewählten Kriterien."
                Else
                    FillGrid(0)
                End If
            End If

        Catch ex As Exception
            lblError.Text = "Beim Erstellen des Reportes ist ein Fehler aufgetreten."
        End Try
    End Sub
    Private Sub FillGrid(ByVal intPageIndex As Int32, Optional ByVal strSort As String = "", Optional ByVal fill As Boolean = False)
        Dim tmpDataView As New DataView()
        m_Report.Result.DefaultView.RowFilter = ""
        tmpDataView = m_Report.Result.DefaultView


        If tmpDataView.Count = 0 Then
            GridView1.Visible = False
            lblNoData.Text = "Keine Daten zur Anzeige gefunden."
            lblNoData.Visible = True
            Result.Visible = False
        Else
            Result.Visible = True
            Dim intTempPageIndex As Int32 = intPageIndex
            Dim strTempSort As String = ""
            Dim strDirection As String = ""
            lblNoData.Text = ""
            GridView1.Visible = True

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

            GridView1.PageIndex = intTempPageIndex
            GridView1.PagerStyle.CssClass = "PagerStyle"
            GridView1.DataSource = tmpDataView
            GridView1.DataBind()
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

    Protected Sub lnkCreateExcel1_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lnkCreateExcel1.Click
        If Not Session("App_Report") Is Nothing Then
            m_Report = CType(Session("App_Report"), Mahnstufe3)
        End If

        Dim control As New Control
        Dim tblTranslations As New DataTable()
        Dim tblTemp As DataTable
        Dim AppURL As String
        Dim col As DataControlField
        Dim col2 As DataColumn
        Dim bVisibility As Integer
        Dim i As Integer
        Dim sColName As String = ""
        tblTemp = m_Report.Result
        AppURL = Replace(Me.Request.Url.LocalPath, "/Services", "..")
        tblTranslations = CType(Me.Session(AppURL), DataTable)
        For Each col In GridView1.Columns
            For i = tblTemp.Columns.Count - 1 To 0 Step -1
                bVisibility = 0
                col2 = tblTemp.Columns(i)
                If col2.ColumnName.ToUpper = col.SortExpression.ToUpper Then
                    sColName = TranslateColLbtn(GridView1, tblTranslations, col.HeaderText, bVisibility)
                    If bVisibility = 0 Then
                        tblTemp.Columns.Remove(col2)
                    ElseIf sColName.Length > 0 Then
                        col2.ColumnName = sColName
                    End If
                End If
            Next
            tblTemp.AcceptChanges()
        Next
        Dim excelFactory As New DocumentGeneration.ExcelDocumentFactory()
        Dim strFileName As String = Format(Now, "yyyyMMdd_HHmmss_") & m_User.UserName
        excelFactory.CreateDocumentAndSendAsResponse(strFileName, tblTemp, Me.Page)

    End Sub

    Protected Sub lbBack_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lbBack.Click
        Response.Redirect("/Services/(S(" + Session.SessionID + "))/Start/Selection.aspx")
    End Sub


    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        SetEndASPXAccess(Me)
    End Sub

    Private Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Unload
        SetEndASPXAccess(Me)
    End Sub
End Class