Imports CKG.Base.Kernel.Common.Common
Imports CKG.Base.Kernel.Security
Imports CKG.Base.Kernel
Imports CKG.Base.Business

Partial Public Class Report05
    Inherits System.Web.UI.Page

#Region "Declarations"
    Private m_App As App
    Private m_User As User
    Protected WithEvents GridNavigation1 As CKG.Services.GridNavigation
    Private objAbmeldungen As Abmeldungen
#End Region

#Region "Events"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        m_User = GetUser(Me)
        FormAuth(Me, m_User)
        GetAppIDFromQueryString(Me)
        m_App = New App(m_User)
        GridNavigation1.setGridElment(gvAbmeldungen)

        Try
            lblHead.Text = m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString

            m_App = New App(m_User)


            lblError.Text = ""
            If Not IsPostBack Then

            Else
                If Not Session("App_objAbmeldungen") Is Nothing Then
                    objAbmeldungen = CType(Session("App_objAbmeldungen"), Abmeldungen)
                End If
            End If
        Catch ex As Exception
            lblError.Text = "Beim Laden der Seite ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
        End Try



    End Sub

    Protected Sub lbCreate_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lbCreate.Click
        DoSubmit()
    End Sub


    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        SetEndASPXAccess(Me)
        HelpProcedures.FixedGridViewCols(gvAbmeldungen)
    End Sub

    Private Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Unload
        SetEndASPXAccess(Me)
    End Sub
    Private Sub GridNavigation1_PagerChanged(ByVal PageIndex As Integer) Handles GridNavigation1.PagerChanged
        gvAbmeldungen.PageIndex = PageIndex
        FillGrid(PageIndex)
    End Sub

    Private Sub GridNavigation1_PageSizeChanged() Handles GridNavigation1.PageSizeChanged
        FillGrid(0)
    End Sub
    Private Sub gvAbmeldungen_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles gvAbmeldungen.Sorting
        FillGrid(gvAbmeldungen.PageIndex, e.SortExpression)
    End Sub

    Protected Sub lnkCreatePDF1_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lnkCreatePDF1.Click

        Dim reportExcel As DataTable = Session("App_ResultTable")

        Try
            If reportExcel.Columns.Contains("EQUNR") Then reportExcel.Columns.Remove("EQUNR")
            Dim excelFactory As New DocumentGeneration.ExcelDocumentFactory()
            Dim strFileName As String = Format(Now, "yyyyMMdd_HHmmss_") & m_User.UserName
            excelFactory.CreateExcelDocumentAsPDFAndSendAsResponse(strFileName, reportExcel, Me)

        Catch ex As Exception
            lblError.Text = ex.Message
        End Try
    End Sub

    Protected Sub lnkCreateExcel1_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lnkCreateExcel1.Click
        Dim reportExcel As DataTable = Session("App_ResultTable")

        Try
            If reportExcel.Columns.Contains("EQUNR") Then reportExcel.Columns.Remove("EQUNR")
            Dim excelFactory As New DocumentGeneration.ExcelDocumentFactory()
            Dim strFileName As String = Format(Now, "yyyyMMdd_HHmmss_") & m_User.UserName
            excelFactory.CreateDocumentAndSendAsResponse(strFileName, reportExcel, Me)

        Catch ex As Exception
            lblError.Text = ex.Message
        End Try
    End Sub

    Protected Sub NewSearch_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles NewSearch.Click
        lbCreate.Visible = Not lbCreate.Visible
        tab1.Visible = Not tab1.Visible
        Queryfooter.Visible = Not Queryfooter.Visible
    End Sub


    Protected Sub btnEmpty_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnEmpty.Click
        DoSubmit()
    End Sub

    Protected Sub lbBack_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lbBack.Click
        Response.Redirect("../../../Start/Selection.aspx?AppID=" & Session("AppID").ToString, False)
    End Sub
#End Region

#Region "Methods"
    Private Sub FillGrid(ByVal intPageIndex As Int32, Optional ByVal strSort As String = "")
        Dim tmpDataView As New DataView()
        tmpDataView = objAbmeldungen.Result.DefaultView

        tmpDataView.RowFilter = ""

        If tmpDataView.Count = 0 Then
            Result.Visible = False
        Else
            Result.Visible = True
            lbCreate.Visible = False
            tab1.Visible = False
            Queryfooter.Visible = False
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
            gvAbmeldungen.PageIndex = intTempPageIndex
            gvAbmeldungen.DataSource = tmpDataView
            gvAbmeldungen.DataBind()
            Session("App_objAbmeldungen") = objAbmeldungen
            objAbmeldungen = Nothing



        End If

    End Sub


    Private Sub DoSubmit()
        Session("lnkExcel") = ""
        Try
            If checkDate() Then

                Dim strFileName As String = Format(Now, "yyyyMMdd_HHmmss_") & m_User.UserName & ".xls"
                objAbmeldungen = New Abmeldungen(m_User, m_App, Session("AppID").ToString, Session.SessionID.ToString, strFileName)

                If txtDatumVon.Text.Length = 0 AndAlso txtDatumBis.Text.Length = 0 Then
                    lblError.Text = "Bitte geben Sie einen Zeitraum an!"
                    Exit Sub
                End If

                If txtDatumVon.Text.Length = 0 Then
                    lblError.Text = "Bitte geben Sie das ""Abmeldedatum von"" an!"
                    Exit Sub
                End If

                If txtDatumBis.Text.Length = 0 Then
                    lblError.Text = "Bitte geben Sie das ""Abmeldedatum bis"" an!"
                    Exit Sub
                End If

                objAbmeldungen.DatumVon = txtDatumVon.Text
                objAbmeldungen.DatumBis = txtDatumBis.Text

                If DateDiff(DateInterval.Day, objAbmeldungen.DatumVon, objAbmeldungen.DatumBis) > 90 Then
                    lblError.Text = "Der ausgewählt Zeitraum darf nicht größer als 90 Tage sein."
                    Exit Sub
                End If

                objAbmeldungen.Fill(Session("AppID").ToString, Session.SessionID.ToString, Me)

                Session("App_ResultTable") = objAbmeldungen.Result

                If Not objAbmeldungen.Status = 0 Then
                    lblError.Text = objAbmeldungen.Message
                ElseIf objAbmeldungen.Result.Rows.Count = 0 Then
                    lblError.Text = "Keine Ergebnisse für die gewählten Kriterien."
                Else
                    FillGrid(0)
                End If
            End If
        Catch ex As Exception
            lblError.Text = "Beim Erstellen des Reportes ist ein Fehler aufgetreten.(" & ex.Message & ")"
        End Try
    End Sub

    Private Function checkDate() As Boolean
        Dim tmpbool As Boolean = True
        For Each ctrl As BaseValidator In Me.Validators
            If ctrl.IsValid = False Then
                tmpbool = False
            End If
        Next
        Return tmpbool
    End Function
#End Region


 
End Class
