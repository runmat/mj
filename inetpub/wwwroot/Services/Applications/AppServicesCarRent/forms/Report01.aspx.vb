Imports CKG
Imports CKG.Base.Kernel
Imports CKG.Base.Kernel.Common.Common
Imports CKG.Services
Imports CKG.Base.Business

Partial Public Class Report01
    Inherits System.Web.UI.Page

#Region "Declarations"
    Private m_App As Security.App
    Private m_User As Security.User
    Private objEingang As ZulFahrzeuge

    Protected WithEvents GridNavigation1 As Global.CKG.Services.GridNavigation


    Dim date_von As Date
    Dim date_bis As Date

#End Region

#Region "Events"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        m_User = GetUser(Me)
        FormAuth(Me, m_User)
        GetAppIDFromQueryString(Me)
        m_App = New Security.App(m_User)
        GridNavigation1.setGridElment(GridView1)
        If Not IsPostBack Then GridNavigation1.PageSizeIndex = 0

        lblHead.Text = m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString

    End Sub

    Private Sub lbCreate_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lbCreate.Click
        Dim tmpDataView As New DataView()

        If IsDate(txtDateBis.Text) = True And IsDate(txtDateVon.Text) = True Then
            date_bis = txtDateBis.Text
            date_von = txtDateVon.Text

            If (date_bis - date_von) > TimeSpan.FromDays(180) Then
                lblError.Visible = True
                lblError.Text = "Datumsbereich darf nicht größer als 180 Tage sein."
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
        Dim col2 As DataColumn
        Dim bVisibility As Integer
        Dim i As Integer
        Dim sColName As String = ""
        Dim tblTranslations As New DataTable()
        Dim tblTemp As New DataTable()
        Dim AppURL As String

        Try

            AppURL = Replace(Me.Request.Url.LocalPath, "/Services", "..")
            tblTranslations = CType(Me.Session(AppURL), DataTable)
            tblTemp = reportExcel.Copy

            For Each col In GridView1.Columns
                For i = tblTemp.Columns.Count - 1 To 0 Step -1
                    bVisibility = 0
                    col2 = tblTemp.Columns(i)
                    If col2.ColumnName.ToUpper = col.SortExpression.ToUpper OrElse col2.ColumnName.ToUpper = col.HeaderText.ToUpper.Replace("COL_", "") Then
                        sColName = TranslateColLbtn(GridView1, tblTranslations, col.HeaderText, bVisibility)
                        If bVisibility = 0 Then
                            tblTemp.Columns.Remove(col2)
                        ElseIf sColName.Length > 0 Then
                            col2.ColumnName = sColName
                        End If
                    End If
                    'EQUNR nicht mit in Excel ausgeben, wird meist nur als boundcolumn versteckt als schlüssel verwendet JJU2008.10.23
                    If col2.ColumnName.ToUpper = "EQUNR" Then
                        tblTemp.Columns.Remove(col2)
                    End If
                Next
                tblTemp.AcceptChanges()
            Next
            Dim excelFactory As New DocumentGeneration.ExcelDocumentFactory()
            Dim strFileName As String = Format(Now, "yyyyMMdd_HHmmss_") & m_User.UserName
            excelFactory.CreateDocumentAndSendAsResponse(strFileName, tblTemp, Me.Page)

        Catch ex As Exception
            lblError.Text = ex.Message
        End Try
        'Dim reportExcel As DataTable = Session("App_ResultTable")

        'Try

        '    Dim excelFactory As New DocumentGeneration.ExcelDocumentFactory()
        '    Dim strFileName As String = Format(Now, "yyyyMMdd_HHmmss_") & m_User.UserName
        '    excelFactory.CreateDocumentAndSendAsResponse(strFileName, reportExcel, Me)

        'Catch ex As Exception
        '    lblError.Text = ex.Message
        'End Try
    End Sub

    Private Sub lbBack_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbBack.Click
        Response.Redirect("/Services/(S(" & Session.SessionID & "))/Start/Selection.aspx?AppID=" & Session("AppID").ToString, False)
    End Sub

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        SetEndASPXAccess(Me)
        HelpProcedures.FixedGridViewCols(GridView1)

        'CheckAllControl Visible Checkbox
        cbxLizFzgAnzeige.Visible = lbl_LizFzgAnzeige.Visible

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
            GridView1.PageIndex = intTempPageIndex
            GridView1.DataSource = tmpDataView
            GridView1.DataBind()

        End If

    End Sub

    Private Sub DoSubmit()
        Dim strFileName As String = ""
        lblError.Text = ""
        objEingang = New ZulFahrzeuge(m_User, m_App, Session("AppID").ToString, Session.SessionID.ToString, strFileName)

        objEingang.DatumVon = date_von
        objEingang.DatumBis = date_bis
        objEingang.ShowLizFahrzeuge = cbxLizFzgAnzeige.Checked

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