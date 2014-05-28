Imports CKG.Base.Business
Imports CKG.Base.Kernel
Imports CKG.Base.Kernel.Common
Imports CKG.Services

Partial Public Class Report04
    Inherits System.Web.UI.Page

#Region "Declarations"
    Private m_App As Security.App
    Private m_User As Security.User
    Private mMahnstufe As Mahnstufe

    Protected WithEvents GridNavigation1 As CKG.Services.GridNavigation

    'Private Equityp As String
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
        'Equityp = Request.QueryString.Item("art").ToString
        lblError.Text = ""
    End Sub

    Private Sub lbCreate_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lbCreate.Click
        DoSubmit()
    End Sub

    Protected Sub lnkCreateExcel1_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lnkCreateExcel1.Click
        Dim tblTemp As DataTable = CType(Session("ResultTable"), DataTable).Copy()

        Dim control As New Control()
        Dim tblTranslations As New DataTable()

        Dim AppURL As String = Nothing
        Dim col2 As DataColumn = Nothing
        Dim bVisibility As Integer = 0
        Dim i As Integer = 0
        Dim sColName As String = ""
        Dim gefunden As Boolean = False
        AppURL = Me.Request.Url.LocalPath.Replace("/Services", "..")
        tblTranslations = CType(Session(AppURL), DataTable)

        ' Nur die Spalten in Excel-Export übernehmen, die auch angezeigt werden
        For i = tblTemp.Columns.Count - 1 To 0 Step -1
            gefunden = False
            bVisibility = 0
            col2 = tblTemp.Columns(i)
            For Each col As DataControlField In GridView1.Columns
                If col2.ColumnName.ToUpper() = col.SortExpression.ToUpper() Then
                    gefunden = True
                    sColName = Common.TranslateColLbtn(GridView1, tblTranslations, col.HeaderText, bVisibility)
                    If bVisibility = 0 Then
                        tblTemp.Columns.Remove(col2)
                    ElseIf sColName.Length > 0 Then
                        col2.ColumnName = sColName
                    End If
                End If
            Next

            If Not gefunden Then
                tblTemp.Columns.Remove(col2)
            End If
        Next

        tblTemp.AcceptChanges()

        Dim excelFactory As New CKG.Base.Kernel.DocumentGeneration.ExcelDocumentFactory()
        Dim filename As String = String.Format("{0:yyyyMMdd_HHmmss_}", System.DateTime.Now) & m_User.UserName
        excelFactory.CreateDocumentAndSendAsResponse(filename, tblTemp, Me.Page, False, Nothing, 0, 0)
    End Sub

    Private Sub lbBack_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbBack.Click
        Response.Redirect("/Services/(S(" & Session.SessionID & "))/Start/Selection.aspx?AppID=" & Session("AppID").ToString, False)
    End Sub

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        Common.SetEndASPXAccess(Me)
        HelpProcedures.FixedGridViewCols(GridView1)

        NewSearch.ImageUrl = String.Format("/Services/Images/queryArrow{0}.gif", IIf(Panel1.Visible, "Up", ""))
        NewSearch2.ImageUrl = NewSearch.ImageUrl
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

            'If Equityp.ToUpper = "T" Then
            '    Me.GridView1.Columns(3).Visible = False
            'End If

            Panel1.Visible = False

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
        mMahnstufe = New Mahnstufe(m_User, m_App, Session("AppID").ToString, Session.SessionID.ToString, strFileName)

        mMahnstufe.Equityp = rblEqui.SelectedValue
        mMahnstufe.Mahnstufe = rblMahnstufe.SelectedValue

        mMahnstufe.Fill(Session("AppID").ToString, Session.SessionID.ToString, Me)

        If mMahnstufe.Result Is Nothing Then
            Result.Visible = False
            lblError.Visible = True
            lblError.Text = "Keine Daten vorhanden."
        Else
            If Not mMahnstufe.Status = 0 Then
                Result.Visible = False
                lblError.Visible = True
                lblError.Text = mMahnstufe.Message
            ElseIf mMahnstufe.Result.Rows.Count = 0 Then
                Result.Visible = False
                lblError.Visible = True
                lblError.Text = "Keine Daten vorhanden."
            Else
                Session("ResultTable") = mMahnstufe.Result
                FillGrid(0)
            End If
        End If
        'sollte eigentlich per Feldübersetzung gesteuert werden
        'If m_User.CustomerName.ToString() = "CharterWay Miete" Then
        '    GridView1.Columns(1).Visible = True
        'End If
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

    Protected Sub NewSearch_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles NewSearch.Click
        Panel1.Visible = Not Panel1.Visible
    End Sub

#End Region

End Class
' ************************************************
' $History: Report04.aspx.vb $
' 
' *****************  Version 5  *****************
' User: Fassbenders  Date: 2.12.10    Time: 14:22
' Updated in $/CKAG2/Applications/AppServicesCarRent/forms
' 
' *****************  Version 4  *****************
' User: Fassbenders  Date: 2.12.10    Time: 11:24
' Updated in $/CKAG2/Applications/AppServicesCarRent/forms
' 
' *****************  Version 3  *****************
' User: Fassbenders  Date: 16.12.09   Time: 9:46
' Updated in $/CKAG2/Applications/AppServicesCarRent/forms
' 
' *****************  Version 2  *****************
' User: Fassbenders  Date: 15.12.09   Time: 10:52
' Updated in $/CKAG2/Applications/AppServicesCarRent/forms
' ITA: 3384
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 15.12.09   Time: 9:20
' Created in $/CKAG2/Applications/AppServicesCarRent/forms
' 