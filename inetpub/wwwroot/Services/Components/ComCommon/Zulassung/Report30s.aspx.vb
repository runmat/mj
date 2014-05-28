Imports CKG.Base.Business
Imports CKG.Base.Kernel
Imports CKG.Base.Kernel.Common.Common
Imports CKG.Base.Kernel.Security

Partial Public Class Report30s
    Inherits System.Web.UI.Page
    Private m_User As User
    Private m_App As App
    Protected WithEvents GridNavigation1 As Global.CKG.Services.GridNavigation

    Private objZLDSuche As ZLD_Suche

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        m_User = GetUser(Me)
        FormAuth(Me, m_User)
        m_App = New App(m_User)
        GridNavigation1.setGridElment(gvZuldienst)
        GetAppIDFromQueryString(Me)

        lblHead.Text = m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString

        If Not IsPostBack Then

        End If

    End Sub
    Private Sub DoSubmit()
        Try
            Dim strFileName As String = ""
            objZLDSuche = New ZLD_Suche(m_User, m_App, strFileName)
            lblError.Text = ""
            objZLDSuche.Kennzeichen = txtKennzeichen.Text
            objZLDSuche.Zulassungspartner = txtZulassungspartner.Text
            objZLDSuche.PLZ = txtPLZ.Text
            objZLDSuche.Fill(Session("AppID").ToString, Session.SessionID.ToString, Me)

            Session("ResultTable") = objZLDSuche.Result
            Session("ResultTableRaw") = objZLDSuche.ResultRaw
            If Not objZLDSuche.Status = 0 Then
                lblError.Text = "Fehler: " & objZLDSuche.Message
            Else
                If objZLDSuche.Result.Rows.Count = 0 Then
                    lblError.Text = "Keine Ergebnisse für die gewählten Kriterien."
                Else
                    FillGrid(0)
                End If
            End If
        Catch ex As Exception
            lblError.Text = "Beim Erstellen des Reportes ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
        End Try
    End Sub
    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        SetEndASPXAccess(Me)
    End Sub

    Private Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Unload
        SetEndASPXAccess(Me)
    End Sub

    Private Sub FillGrid(ByVal intPageIndex As Int32, Optional ByVal strSort As String = "")
        Dim tmpDataView As New DataView()
        tmpDataView = objZLDSuche.Result.DefaultView
        tmpDataView.RowFilter = ""

        If tmpDataView.Count = 0 Then
            gvZuldienst.Visible = False
            Result.Visible = False
            GridNavigation1.Visible = False
            Panel1.Visible = True
            cmdCreate.Visible = True
        Else
            Result.Visible = True
            lblError.Visible = False
            Panel1.Visible = False
            cmdCreate.Visible = False
            Dim intTempPageIndex As Int32 = intPageIndex
            Dim strTempSort As String = ""
            Dim strDirection As String = ""
            gvZuldienst.Visible = True

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

            gvZuldienst.DataSource = tmpDataView
            gvZuldienst.DataBind()
            gvZuldienst.PageIndex = intTempPageIndex

        End If
    End Sub
    Private Sub GridView1_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvZuldienst.PageIndexChanging
        FillGrid(e.NewPageIndex)
    End Sub

    Private Sub GridNavigation1_PagerChanged(ByVal PageIndex As Integer) Handles GridNavigation1.PagerChanged
        gvZuldienst.PageIndex = PageIndex
        FillGrid(PageIndex)
    End Sub

    Private Sub GridNavigation1_PageSizeChanged() Handles GridNavigation1.PageSizeChanged
        FillGrid(0)
    End Sub

    Private Sub NewSearch_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles NewSearch.Click
        Panel1.Visible = Not Panel1.Visible
        cmdCreate.Visible = Not cmdCreate.Visible
    End Sub

    Protected Sub lnkCreateExcel_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lnkCreateExcel.Click

        Dim control As New Control
        Dim tblTranslations As New DataTable()
        Dim tblTemp As DataTable = CType(Session("ResultTable"), DataTable).Copy
        Dim AppURL As String
        Dim col As DataControlField
        Dim col2 As DataColumn
        Dim bVisibility As Integer
        Dim i As Integer
        Dim sColName As String = ""

        AppURL = Replace(Me.Request.Url.LocalPath, "/Services", "..")
        tblTranslations = CType(Me.Session(AppURL), DataTable)
        For Each col In gvZuldienst.Columns
            For i = tblTemp.Columns.Count - 1 To 0 Step -1
                bVisibility = 0
                col2 = tblTemp.Columns(i)
                If col2.ColumnName.ToUpper = col.SortExpression.ToUpper Then
                    sColName = TranslateColLbtn(gvZuldienst, tblTranslations, col.HeaderText, bVisibility)
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

    Protected Sub btnEmpty_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnEmpty.Click
        DoSubmit()
    End Sub

    Protected Sub cmdCreate_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdCreate.Click
        DoSubmit()
    End Sub

    Protected Sub lbBack_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lbBack.Click
        Response.Redirect("/Services/(S(" & Session.SessionID & "))/Start/Selection.aspx?AppID=" & Session("AppID").ToString, False)
    End Sub

    Private Sub gvZuldienst_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvZuldienst.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim label As Label
            label = CType(e.Row.FindControl("lblDetail"), Label)
            If Not label Is Nothing Then
                Dim ImgBtn As ImageButton
                ImgBtn = CType(e.Row.FindControl("ibtnDetail"), ImageButton)
                If Not ImgBtn Is Nothing Then
                    ImgBtn.Attributes.Add("onclick", "openinfo('" & label.Text & "')")
                End If
            End If
        End If
    End Sub
End Class