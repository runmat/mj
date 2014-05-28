Imports CKG.Base.Business
Imports CKG.Base.Kernel
Imports CKG.Base.Kernel.Common

Public Class Report08
    Inherits System.Web.UI.Page

#Region "Declarations"
    Private m_App As Security.App
    Private m_User As Security.User
    Private mMahnwesen As Mahnwesen
#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Me.m_User = Common.GetUser(Me)
        Common.FormAuth(Me, Me.m_User)
        Common.GetAppIDFromQueryString(Me)
        Me.m_App = New Security.App(m_User)

        Me.GridNavigation1.setGridElment(Me.GridView1)

        Me.lblHead.Text = Me.m_User.Applications.Select("AppID = '" & DirectCast(Session("AppID"), String) & "'")(0)("AppFriendlyName").ToString
        Me.lblError.Text = ""
    End Sub

    Protected Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        Common.SetEndASPXAccess(Me)
        HelpProcedures.FixedGridViewCols(Me.GridView1)

        NewSearch.ImageUrl = String.Format("/Services/Images/queryArrow{0}.gif", IIf(Panel1.Visible, "Up", ""))
        NewSearch2.ImageUrl = NewSearch.ImageUrl
    End Sub

    Protected Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Unload
        Common.SetEndASPXAccess(Me)
    End Sub

    Protected Sub NewSearch_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles NewSearch.Click
        'Me.NewSearch.Visible = False
        'Me.NewSearchUp.Visible = True
        'Me.lbCreate.Visible = True
        'Me.tab1.Visible = True
        'Me.Queryfooter.Visible = True
        'Me.FillGrid(GridView1.PageIndex)

        Panel1.Visible = Not Panel1.Visible
    End Sub

    'Protected Sub NewSearchUp_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles NewSearchUp.Click
    '    Me.NewSearch.Visible = True
    '    Me.NewSearchUp.Visible = False
    '    Me.lbCreate.Visible = False
    '    Me.tab1.Visible = False
    '    Me.Queryfooter.Visible = False
    '    Me.FillGrid(GridView1.PageIndex)
    'End Sub

    Protected Sub lbCreate_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lbCreate.Click
        Me.DoSubmit()
    End Sub

    Protected Sub lnkCreateExcel1_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lnkCreateExcel1.Click
        Dim tblTemp As DataTable = CType(Session("MahnTable"), DataTable).Copy()

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

    Private Sub GridNavigation1_PagerChanged(ByVal PageIndex As Integer) Handles GridNavigation1.PagerChanged
        Me.UpdateResult()
        GridView1.PageIndex = PageIndex
        Me.FillGrid(PageIndex)
    End Sub

    Private Sub GridNavigation1_PageSizeChanged() Handles GridNavigation1.PageSizeChanged
        Me.UpdateResult()
        Me.FillGrid(0)
    End Sub

    Protected Sub lbSetMahnsperre_Click(sender As Object, e As EventArgs) Handles lbSetMahnsperre.Click
        Me.SperrenEntsperren("MAHNSP_SETZEN", txtSperrgrund.Text)
    End Sub

    Protected Sub lbDelMahnsperre_Click(sender As Object, e As EventArgs) Handles lbDelMahnsperre.Click
        Me.SperrenEntsperren("MAHNSP_ENTF")
    End Sub

    Private Sub lbBack_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lbBack.Click
        Response.Redirect("/Services/(S(" & Session.SessionID & "))/Start/Selection.aspx?AppID=" & Session("AppID").ToString, False)
    End Sub

    Private Sub DoSubmit()
        Dim strFileName As String = ""
        mMahnwesen = New Mahnwesen(m_User, m_App, Session("AppID").ToString, Session.SessionID.ToString, strFileName)

        mMahnwesen.DatumVon = txtDatumVon.Text
        mMahnwesen.DatumBis = txtDatumBis.Text
        mMahnwesen.Kennzeichen = txtKennzeichen.Text
        mMahnwesen.Fahrgestellnummer = txtFahrgestellnummer.Text

        Dim Mahnstufen As String = ""

        For i = 0 To cblMahnstufe.Items.Count - 1
            If cblMahnstufe.Items(i).Selected Then
                Mahnstufen &= (i + 1).ToString() & ";"
            End If
        Next

        If Mahnstufen.Length > 0 Then
            Mahnstufen = Mahnstufen.TrimEnd(";"c)
        End If

        mMahnwesen.Mahnstufe = Mahnstufen
        mMahnwesen.Mahnsperre = IIf(cbxMahnsperre.Checked, "X", " ")
        mMahnwesen.Vorgangsart = "D"

        mMahnwesen.GetSchluesseleingaenge(Session("AppID").ToString, Session.SessionID.ToString, Page)

        If mMahnwesen.Status <> 0 Then
            lblError.Text = "Es konnten keine Daten gefunden werden."
            Me.Result.Visible = False
            'Me.NewSearch.Visible = False
            'Me.NewSearchUp.Visible = False
            Exit Sub
        End If

        If mMahnwesen.ResultTable.Rows.Count > 0 Then
            Session("MahnTable") = mMahnwesen.ResultTable
            FillGrid(0)
        Else
            lblError.Text = "Es konnten keine Daten gefunden werden."
            Me.Result.Visible = False
            'Me.NewSearch.Visible = False
            'Me.NewSearchUp.Visible = False
        End If
    End Sub

    Private Sub FillGrid(ByVal intPageIndex As Int32, Optional ByVal strSort As String = "")

        'NewSearchUp.Visible = False

        Dim tmpDataView As New DataView()
        Dim tmpDataTable As New DataTable

        tmpDataTable = Session("MahnTable")
        tmpDataView = tmpDataTable.DefaultView

        tmpDataView.RowFilter = ""
        If tmpDataView.Count = 0 Then
            Result.Visible = False
        Else

            Result.Visible = True

            If hField.Value = "0" Then
                lblNoData.Visible = False

                Panel1.Visible = False

                'lbCreate.Visible = False
                'tab1.Visible = False
                'Queryfooter.Visible = False
            End If

            hField.Value = "1"

            'If tab1.Visible = False Then
            '    NewSearch.Visible = True
            '    'NewSearchUp.Visible = False
            'Else
            '    NewSearch.Visible = False
            '    'NewSearchUp.Visible = True
            'End If




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

    Private Sub SperrenEntsperren(ByVal Art As String, Optional ByVal sperrgrund As String = "")
        UpdateResult()

        If CType(Session("MahnTable"), DataTable).Select("Selected = 'X'").Length < 1 Then
            lblError.Text = "Es wurden keinen Datensätze ausgewählt."
            Exit Sub
        End If

        mMahnwesen = New Mahnwesen(m_User, m_App, Session("AppID").ToString, Session.SessionID.ToString, "")

        Dim dt As DataTable = mMahnwesen.GetExporttable(Page)
        Dim Exporttable As DataTable = CType(Session("MahnTable"), DataTable).Select("Selected = 'X'").CopyToDataTable

        Dim NewRow As DataRow

        For Each dr As DataRow In Exporttable.Rows
            NewRow = dt.NewRow()
            NewRow("EQUNR") = dr("EQUNR")
            NewRow("CHASSIS_NUM") = dr("CHASSIS_NUM")
            NewRow("MATNR") = dr("MATNR")
            NewRow(Art) = "X"
            If Art = "MAHNSP_SETZEN" And Not String.IsNullOrEmpty(sperrgrund) Then
                NewRow("BEM") = sperrgrund
            Else
                NewRow("BEM_ENTF") = "X"
            End If
            dt.Rows.Add(NewRow)
        Next

        mMahnwesen.ExTable = dt
        mMahnwesen.Save(Session("AppID").ToString, Session.SessionID.ToString, Page)

        If mMahnwesen.Status <> 0 Then
            lblErrorResult.Text = mMahnwesen.Message
        Else
            lblNoDataResult.Text = "Es wurden " & Exporttable.Rows.Count & " Datensätze gespeichert."
        End If
    End Sub

    Private Sub GridView1_Sorting(sender As Object, e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles GridView1.Sorting
        UpdateResult()
        FillGrid(GridView1.PageIndex, e.SortExpression)
    End Sub

    Private Sub UpdateResult()
        Dim chk As CheckBox
        Dim lbl As Label

        For Each dgr As GridViewRow In GridView1.Rows
            chk = dgr.FindControl("chkAuswahl")
            lbl = dgr.FindControl("lblID")

            If chk.Checked = True Then
                CType(Session("MahnTable"), DataTable).Select("ID = " & lbl.Text)(0)("Selected") = "X"
            Else
                CType(Session("MahnTable"), DataTable).Select("ID = " & lbl.Text)(0)("Selected") = ""
            End If
        Next
    End Sub
End Class