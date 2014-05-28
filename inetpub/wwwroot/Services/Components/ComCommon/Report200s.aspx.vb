Imports CKG.Base.Business
Imports CKG.Base.Kernel
Imports CKG.Base.Kernel.Common.Common

Partial Public Class Report200s
    Inherits System.Web.UI.Page

    Private m_User As Security.User
    Private m_App As Security.App

    Protected WithEvents GridNavigation1 As CKG.Services.GridNavigation

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        m_User = GetUser(Me)
        FormAuth(Me, m_User)
        GetAppIDFromQueryString(Me)
        lblHead.Text = m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString
        m_App = New Base.Kernel.Security.App(m_User)
        GridNavigation1.setGridElment(GridView1)

    End Sub

    Protected Sub cmdCreate_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdCreate.Click
        DoSubmit()
    End Sub

    Protected Sub ibtNewSearch_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles ibtNewSearch.Click
        divSelection.Visible = Not divSelection.Visible
        cmdCreate.Visible = Not cmdCreate.Visible
        Result.Visible = Not Result.Visible
    End Sub

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        SetEndASPXAccess(Me)
        HelpProcedures.FixedGridViewCols(GridView1)
    End Sub

    Private Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Unload
        SetEndASPXAccess(Me)
    End Sub

    Private Sub GridNavigation1_PagerChanged(ByVal PageIndex As Integer) Handles GridNavigation1.PagerChanged
        GridView1.PageIndex = PageIndex
        FillGrid(PageIndex)
    End Sub

    Private Sub GridNavigation1_PageSizeChanged() Handles GridNavigation1.PageSizeChanged
        FillGrid(0)
    End Sub

    Private Sub GridView1_Sorting(sender As Object, e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles GridView1.Sorting
        FillGrid(GridView1.PageIndex, e.SortExpression)
    End Sub

    Protected Sub lnkCreateExcel1_Click(sender As Object, e As EventArgs) Handles lnkCreateExcel1.Click
        Dim control As New Control()
        Dim tblTranslations As New DataTable()
        Dim tblTemp As DataTable = CType(Session("Result"), Report_200s).Result.Copy
        Dim AppURL As String = Nothing
        Dim col2 As DataColumn = Nothing
        Dim bVisibility As Integer = 0
        Dim i As Integer = 0
        Dim sColName As String = ""
        AppURL = Me.Request.Url.LocalPath.Replace("/Services", "..")
        tblTranslations = DirectCast(Me.Session(AppURL), DataTable)



        'Reihenfolge anpassen ************************************

        Dim ExcelTable As New DataTable()
        'DataColumn dc;

        For Each col As DataControlField In GridView1.Columns
            If col.Visible = True Then

                ExcelTable.Columns.Add(tblTemp.Columns(col.SortExpression.ToUpper()).ColumnName, tblTemp.Columns(col.SortExpression.ToUpper()).DataType)

            End If
        Next

        ExcelTable.AcceptChanges()


        Dim NewRow As DataRow

        For Each dr As DataRow In tblTemp.Rows

            NewRow = ExcelTable.NewRow()

            For Each dCol As DataColumn In ExcelTable.Columns


                NewRow(dCol.ColumnName) = dr(dCol.ColumnName)
            Next


            ExcelTable.Rows.Add(NewRow)
        Next

        '*********************************************************





        For Each col As DataControlField In GridView1.Columns

            If col.Visible = True Then

                i = ExcelTable.Columns.Count - 1
                While i >= 0
                    bVisibility = 0
                    col2 = ExcelTable.Columns(i)
                    If col2.ColumnName.ToUpper() = col.SortExpression.ToUpper() Then


                        sColName = CKG.Base.Kernel.Common.Common.TranslateColLbtn(GridView1, tblTranslations, col.HeaderText, bVisibility)
                        If bVisibility = 0 Then
                            ExcelTable.Columns.Remove(col2)
                        ElseIf sColName.Length > 0 Then
                            col2.ColumnName = sColName
                        End If
                    End If
                    i += -1
                End While



                ExcelTable.AcceptChanges()



            End If
        Next


        i = ExcelTable.Columns.Count - 1
        While i >= 0

            Dim colFound As [Boolean] = False
            Dim colTempName As String = ExcelTable.Columns(i).ColumnName

            For Each dr As DataRow In tblTranslations.Rows
                If colTempName = dr(4).ToString() Then
                    colFound = True
                    Exit For

                End If
            Next

            If colFound = False Then
                ExcelTable.Columns.Remove(colTempName)

            End If
            i += -1
        End While

        ExcelTable.AcceptChanges()





        Dim excelFactory As New CKG.Base.Kernel.DocumentGeneration.ExcelDocumentFactory()
        Dim filename As String = [String].Format("{0:yyyyMMdd_HHmmss_}", System.DateTime.Now) + m_User.UserName
        excelFactory.CreateDocumentAndSendAsResponse(filename, ExcelTable, Me.Page, False, Nothing, 0, _
         0)

    End Sub

    Protected Sub lbBack_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lbBack.Click
        Response.Redirect("/Services/(S(" + Session.SessionID + "))/Start/Selection.aspx")
    End Sub

    Private Sub FillGrid(ByVal intPageIndex As Int32, Optional ByVal strSort As String = "")
        Dim tmpDataView As New DataView()
        tmpDataView = CType(Session("Result"), Report_200s).Result.DefaultView
        tmpDataView.RowFilter = ""

        If tmpDataView.Count = 0 Then
            GridView1.Visible = False
            Result.Visible = False
            GridNavigation1.Visible = False

        Else
            Dim intTempPageIndex As Int32 = intPageIndex
            Dim strTempSort As String = ""
            Dim strDirection As String = ""
            GridView1.Visible = True
            Result.Visible = True

            GridNavigation1.Visible = True
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

            GridView1.DataSource = tmpDataView
            GridView1.DataBind()
            GridView1.PageIndex = intTempPageIndex

        End If
    End Sub

    Private Sub DoSubmit()
        Session("lnkExcel") = ""
        Dim logApp As New Base.Kernel.Logging.Trace(m_App.Connectionstring, m_App.SaveLogAccessSAP, m_App.LogLevel)
        logApp.InitEntry(m_User.UserName, Session.SessionID, CInt(Session("AppID")), m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString, m_User.CustomerName, m_User.Customer.CustomerId, m_User.IsTestUser, 0)


        Dim strFileName As String = Format(Now, "yyyyMMdd_HHmmss_") & m_User.UserName & ".xls"
        Dim m_Report As New Report_200s(m_User, m_App, strFileName)
        Dim strKennzeichen As String
        Dim strBriefnummer As String
        Dim datDurchfuehrungVon As String
        Dim datDurchfuehrungBis As String
        Dim datAbmeldedatumVon As String
        Dim datAbmeldedatumBis As String
        Dim strFahrgestellnummer As String

        lblError.Text = ""
        If (txtDurchfuehrungVon.Text = String.Empty And _
            txtDurchfuehrungBis.Text = String.Empty) And _
            (txtKennzeichen.Text = String.Empty And txtFahrgestellnummer.Text = String.Empty And txtNummerZB2.Text = String.Empty) And _
            (txtAbmeldedatumVon.Text = String.Empty And txtAbmeldedatumBis.Text = String.Empty) Then
            lblError.Text = "Keine Abfragekriterien eingegeben."
            Exit Sub
        End If


        strKennzeichen = txtKennzeichen.Text
        strFahrgestellnummer = txtFahrgestellnummer.Text
        strBriefnummer = txtNummerZB2.Text
        datDurchfuehrungVon = txtDurchfuehrungVon.Text
        datDurchfuehrungBis = txtDurchfuehrungBis.Text
        datAbmeldedatumVon = txtAbmeldedatumVon.Text
        datAbmeldedatumBis = txtAbmeldedatumBis.Text

        If datDurchfuehrungVon.Length > 0 Then
            If IsDate(datDurchfuehrungVon) = False Then
                Exit Sub
            End If
        End If

        If CheckIsDate(datDurchfuehrungVon) = False OrElse _
            CheckIsDate(datDurchfuehrungBis) = False OrElse _
            CheckIsDate(datAbmeldedatumVon) = False OrElse _
            CheckIsDate(datAbmeldedatumBis) = False Then Exit Sub



        If datDurchfuehrungVon.Length > 0 And datDurchfuehrungBis.Length > 0 Then
            If DateDiff(DateInterval.Day, CDate(datDurchfuehrungVon), CDate(datDurchfuehrungBis)) > 180 Then
                lblError.Text = "Es kann maximal ein Zeitraum von 180 Tagen aufgerufen werden ."
                Exit Sub
            End If
        End If

        If datAbmeldedatumVon.Length > 0 And datAbmeldedatumBis.Length > 0 Then
            If DateDiff(DateInterval.Day, CDate(datAbmeldedatumVon), CDate(datAbmeldedatumBis)) > 180 Then
                lblError.Text = "Es kann maximal ein Zeitraum von 180 Tagen aufgerufen werden ."
                Exit Sub
            End If
        End If


        If lblError.Text.Length = 0 Then
            m_Report.Fill(Session("AppID").ToString, Session.SessionID.ToString, datDurchfuehrungVon, datDurchfuehrungBis, datAbmeldedatumVon, datAbmeldedatumBis, strKennzeichen, strFahrgestellnummer, strBriefnummer, Me.Page)

            Session("Result") = m_Report

            If Not m_Report.Status = 0 Then
                lblError.Text = "Fehler: " & m_Report.Message
            Else
                If m_Report.Result.Rows.Count = 0 Then
                    lblError.Text = "Keine Ergebnisse für die gewählten Kriterien."
                Else

                    divSelection.Visible = False
                    FillGrid(0)
                    cmdCreate.Visible = False
                End If
            End If
        End If

    End Sub

    Private Function CheckIsDate(ByVal CheckString As String) As Boolean

        If CheckString.Length > 0 Then
            If IsDate(CheckString) = False Then Return False Else Return True
        Else
            Return True
        End If


    End Function


End Class