Imports CKG
Imports CKG.Base.Kernel
Imports CKG.Base.Kernel.Common.Common
Imports CKG.Base.Kernel.Security
Imports CKG.Services

Partial Public Class Report03
    Inherits System.Web.UI.Page

#Region "Declarations"
    Private m_App As App
    Private m_User As User
    Protected WithEvents GridNavigation1 As CKG.Services.GridNavigation
    Private objEingang As ZBIIEingang

    Dim date_von As Date
    Dim date_bis As Date

    Protected WithEvents lbCreate As System.Web.UI.WebControls.LinkButton
    Protected WithEvents txtDateBis As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtDatevon As System.Web.UI.WebControls.TextBox
    Protected WithEvents Result As Global.System.Web.UI.HtmlControls.HtmlGenericControl
    Protected WithEvents Gridview1 As Global.System.Web.UI.WebControls.GridView

#End Region

#Region "Events"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        m_User = GetUser(Me)
        FormAuth(Me, m_User)
        GetAppIDFromQueryString(Me)
        m_App = New App(m_User)

        GridNavigation1.setGridElment(Gridview1)

        lblHead.Text = "ZBII-Eingänge"
        lblError.Text = ""

    End Sub

    Private Sub lbCreate_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lbCreate.Click
        Dim tmpDataView As New DataView()

        If IsDate(txtDateBis.Text) = True And IsDate(txtDAtevon.Text) = True Then
            date_bis = txtDateBis.Text
            date_von = txtDatevon.Text

            If (date_bis - date_von) > TimeSpan.FromDays(30) Then
                lblError.Visible = True
                lblError.Text = "Datumsbereich darf nicht größer als 30 Tage sein."
                Exit Sub
            ElseIf (date_bis - date_von) < TimeSpan.FromDays(0) Then
                lblError.Visible = True
                lblError.Text = "Datum ""Bis"" darf nicht vor ""Von"" liegen."
                Exit Sub
            Else
                DoSubmit()
            End If
        Else
            lblError.Visible = True
            lblError.Text = "Ungültiger Datumswert"
            Exit Sub
        End If
    End Sub

    Protected Sub lnkCreateExcel1_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lnkCreateExcel1.Click
        Dim reportExcel As DataTable = Session("App_ResultTable")

        Try

            Dim excelFactory As New DocumentGeneration.ExcelDocumentFactory()
            Dim strFileName As String = Format(Now, "yyyyMMdd_HHmmss_") & m_User.UserName
            excelFactory.CreateDocumentAndSendAsResponse(strFileName, reportExcel, Me)

        Catch ex As Exception
            lblError.Text = ex.Message
        End Try
    End Sub

    Protected Sub lnkCreatePDF1_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lnkCreatePDF1.Click

        Dim reportExcel As DataTable = Session("App_ResultTable")

        Try
            Dim excelFactory As New DocumentGeneration.ExcelDocumentFactory()
            Dim strFileName As String = Format(Now, "yyyyMMdd_HHmmss_") & m_User.UserName
            excelFactory.CreateExcelDocumentAsPDFAndSendAsResponse(strFileName, reportExcel, Me)

        Catch ex As Exception
            lblError.Text = ex.Message
        End Try
    End Sub

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        SetEndASPXAccess(Me)
        Base.Business.HelpProcedures.FixedGridViewCols(Gridview1)
    End Sub

    Private Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Unload
        SetEndASPXAccess(Me)
    End Sub
#End Region

#Region "Methods"
    Private Sub FillGrid(ByVal intPageIndex As Int32, Optional ByVal strSort As String = "")
        Dim tmpDataView As New DataView()
        Dim tmpDataTable As New DataTable

        tmpDataTable = Session("App_ResultTable")
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
            Gridview1.PageIndex = intTempPageIndex
            Gridview1.DataSource = tmpDataView
            Gridview1.DataBind()
            Gridview1 = Nothing

        End If

    End Sub

    Private Sub DoSubmit()
        Dim strFileName As String = ""
        objEingang = New ZBIIEingang(m_User, m_App, Session("AppID").ToString, Session.SessionID.ToString, strFileName)

        objEingang.DatumVon = date_von
        objEingang.DatumBis = date_bis
        objEingang.Fill(Session("AppID").ToString, Session.SessionID.ToString, Me)



        If objEingang.Result Is Nothing Then
            lblError.Visible = True
            lblError.Text = "Für den gewählten Zeitraum existieren keine Daten."
        Else


            If Not objEingang.Status = 0 Then
                lblError.Text = objEingang.Message
            ElseIf objEingang.Result.Rows.Count = 0 Then
                lblError.Text = "Keine Ergebnisse für die gewählten Kriterien."
            Else
                Session("App_ResultTable") = objEingang.Result
                FillGrid(0)
            End If
        End If
    End Sub

    Private Sub GridNavigation1_PagerChanged(ByVal PageIndex As Integer) Handles GridNavigation1.PagerChanged
        Gridview1.PageIndex = PageIndex
        FillGrid(PageIndex)
    End Sub

    Private Sub GridNavigation1_PageSizeChanged() Handles GridNavigation1.PageSizeChanged
        FillGrid(0)
    End Sub
    Private Sub Gridview1_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles Gridview1.Sorting
        FillGrid(Gridview1.PageIndex, e.SortExpression)
    End Sub
#End Region

    Protected Sub lbBack_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lbBack.Click
        Response.Redirect("../../../Start/Selection.aspx", False)
    End Sub
End Class
