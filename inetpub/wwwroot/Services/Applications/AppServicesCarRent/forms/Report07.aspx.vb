Imports CKG.Base.Business
Imports CKG.Base.Kernel
Imports CKG.Base.Kernel.Common


Public Class Report07
    Inherits System.Web.UI.Page

#Region "Declarations"
    Private m_App As Security.App
    Private m_User As Security.User
    Private mMahnwesen As Mahnwesen
    Private sVorgangsart As String

    Protected WithEvents GridNavigation1 As CKG.Services.GridNavigation

#End Region


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        m_User = Common.GetUser(Me)
        Common.FormAuth(Me, m_User)
        Common.GetAppIDFromQueryString(Me)
        m_App = New Security.App(m_User)

        GridNavigation1.setGridElment(GridView1)
        If Not IsPostBack Then GridNavigation1.PageSizeIndex = 0

        lblHead.Text = m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString

        If Request.QueryString.Item("art") = Nothing Then
            lblError.Text = "Error: Fehlender URL - Parameter. Bitte wenden sie sich an Ihren DAD Administrator"
            lbCreate.Visible = False
            Return
        End If

        sVorgangsart = Request.QueryString.Item("art").ToString

        Select Case sVorgangsart
            Case "S"

            Case "F"
                Dim tmpIndex As Integer = -1
                chxTeileingang.Visible = False
                lbl_Teileing.Visible = False

                lblTeileingangShow.Visible = False
                lblMaterialbezeichnung.Visible = False


                For index = 0 To GridView1.Columns.Count - 1

                    If GridView1.Columns(index).HeaderText = "col_Materialbezeichnung" Then
                        GridView1.Columns(index).Visible = False

                    End If
                    If GridView1.Columns(index).HeaderText = "col_Teileingang" Then
                        GridView1.Columns(index).Visible = False

                    End If
                Next

        End Select

        lblError.Text = ""
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

    Protected Sub lbCreate_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lbCreate.Click
        DoSubmit()
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

            If tab1.Visible = False Then
                NewSearch.Visible = True
                'NewSearchUp.Visible = False
            Else
                NewSearch.Visible = False
                'NewSearchUp.Visible = True
            End If

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

        If cbxMahnsperre.Checked = True Then
            mMahnwesen.Mahnsperre = "X"
        Else
            mMahnwesen.Mahnsperre = ""
        End If

        If chxTeileingang.Checked Then
            mMahnwesen.Teileingang = "X"
        Else
            mMahnwesen.Teileingang = ""
        End If

        mMahnwesen.Mahnstufe = Mahnstufen
        mMahnwesen.Vorgangsart = sVorgangsart

        mMahnwesen.GetSchluesseleingaenge(Session("AppID").ToString, Session.SessionID.ToString, Page)

        If mMahnwesen.Status <> 0 Then
            lblError.Text = "Es konnten keine Daten gefunden werden."
            Exit Sub
        End If

        If mMahnwesen.ResultTable.Rows.Count > 0 Then
            Session("MahnTable") = mMahnwesen.ResultTable
            FillGrid(0)
        Else
            lblError.Text = "Es konnten keine Daten gefunden werden."
        End If
    End Sub

    Private Sub GridNavigation1_PagerChanged(ByVal PageIndex As Integer) Handles GridNavigation1.PagerChanged
        UpdateResult()
        GridView1.PageIndex = PageIndex
        FillGrid(PageIndex)
    End Sub

    Private Sub GridNavigation1_PageSizeChanged() Handles GridNavigation1.PageSizeChanged
        UpdateResult()
        FillGrid(0)
    End Sub

    Protected Sub NewSearch_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles NewSearch.Click
        'NewSearch.Visible = False
        'NewSearchUp.Visible = True
        'lbCreate.Visible = True
        'tab1.Visible = True
        'Queryfooter.Visible = True
        'FillGrid(GridView1.PageIndex, "")

        Panel1.Visible = Not Panel1.Visible
    End Sub

    'Protected Sub NewSearchUp_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles NewSearchUp.Click
    '    NewSearch.Visible = True
    '    NewSearchUp.Visible = False
    '    lbCreate.Visible = False
    '    tab1.Visible = False
    '    Queryfooter.Visible = False
    '    FillGrid(GridView1.PageIndex, "")
    'End Sub

    Private Sub GridView1_RowCommand(sender As Object, e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles GridView1.RowCommand
        If e.CommandName = "EditNew" Then
            lblID.Text = e.CommandArgument.ToString()

            Dim gvRow As GridViewRow = GridView1.Rows.Cast(Of GridViewRow).FirstOrDefault(Function(gvr)
                                                                                              Dim lbl = CType(gvr.FindControl("lblID"), Label)
                                                                                              Return lbl.Text = lblID.Text
                                                                                          End Function)
            ddlMahnsperre.SelectedValue = "0"

            txtBemerkung.Text = CType(gvRow.FindControl("lblBemerkung"), Label).Text
            txtMahn.Text = CType(gvRow.FindControl("lblMahndatumAb"), Label).Text
            lblFinShow.Text = CType(gvRow.FindControl("lblFin"), Label).Text
            lblKennzeichenShow.Text = CType(gvRow.FindControl("lblKennzeichen"), Label).Text
            lblTeileingangShow.Text = CType(gvRow.FindControl("lblTeileingang"), Label).Text
            lblMaterialbezeichnungShow.Text = CType(gvRow.FindControl("lblMaterialbezeichnung"), Label).Text
            lblMahnstufeShow.Text = CType(gvRow.FindControl("lblMahnstufe"), Label).Text
            lblMahnartShow.Text = CType(gvRow.FindControl("lblMahnart"), Label).Text
            lblMahndatum1Show.Text = CType(gvRow.FindControl("lblMahndatum1"), Label).Text
            lblAdresseShow.Text = CType(gvRow.FindControl("lblAdresse"), Label).Text
            lblMahndatum2Show.Text = CType(gvRow.FindControl("lblMahndatum2"), Label).Text
            lblMahndatum3Show.Text = CType(gvRow.FindControl("lblMahndatum3"), Label).Text
            lblMahrnsperreGesAmShow.Text = CType(gvRow.FindControl("lblMahnsperreGesAm"), Label).Text
            lblMahrnsperreGesDurchShow.Text = CType(gvRow.FindControl("lblMahnsperreGesDurch"), Label).Text
            lblMahnsperreEntfAmShow.Text = CType(gvRow.FindControl("lblMahnsperreEntfAm"), Label).Text
            lblMahnsperreEntfDurchShow.Text = CType(gvRow.FindControl("lblMahnsperreEntfDurch"), Label).Text

            btnOK.Visible = True

            divMessage.Visible = True
            divBackDisabled.Visible = True
        End If
    End Sub

    Private Sub GridView1_Sorting(sender As Object, e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles GridView1.Sorting
        UpdateResult()
        FillGrid(GridView1.PageIndex, e.SortExpression)
    End Sub

    Protected Sub lbSetMahnsperre_Click(sender As Object, e As EventArgs) Handles lbSetMahnsperre.Click
        SperrenEntsperren("MAHNSP_SETZEN")
    End Sub

    Protected Sub lbDelMahnsperre_Click(sender As Object, e As EventArgs) Handles lbDelMahnsperre.Click
        SperrenEntsperren("MAHNSP_ENTF")
    End Sub

    Private Sub SperrenEntsperren(ByVal Art As String)
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

            dt.Rows.Add(NewRow)
        Next

        mMahnwesen.ExTable = dt

        mMahnwesen.Save(Session("AppID").ToString, Session.SessionID.ToString, Page)

        If mMahnwesen.Status <> 0 Then
            lblErrorResult.Text = mMahnwesen.Message
        Else
            DoSubmit()
            lblNoDataResult.Text = "Es wurden " & Exporttable.Rows.Count & " Datensätze gespeichert."
        End If
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

    Private Sub btnOK_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnOK.Click
        If txtBemerkung.Text.Length > 132 Then
            lblErr.Text = "Der Bemerkungstext darf nicht mehr als 132 Zeichen umfassen."
            Exit Sub
        End If

        If txtMahn.Text.Length > 0 Then
            If IsDate(txtMahn.Text) = False Then
                lblErr.Text = "Bitte geben Sie ein korrektes Datum ein."
                Exit Sub
            End If
        End If

        Dim selRow() As DataRow = CType(Session("MahnTable"), DataTable).Select("ID = '" & lblID.Text & "'")

        mMahnwesen = New Mahnwesen(m_User, m_App, Session("AppID").ToString, Session.SessionID.ToString, "")

        Dim dt As DataTable = mMahnwesen.GetExporttable(Page)
        Dim NewRow As DataRow
        NewRow = dt.NewRow()
        NewRow("EQUNR") = selRow(0)("EQUNR")
        NewRow("CHASSIS_NUM") = selRow(0)("CHASSIS_NUM")
        NewRow("MATNR") = selRow(0)("MATNR")

        If ddlMahnsperre.SelectedValue <> 0 Then
            If ddlMahnsperre.SelectedValue = 1 Then
                NewRow("MAHNSP_SETZEN") = "X"
            End If
            If ddlMahnsperre.SelectedValue = 2 Then
                NewRow("MAHNSP_ENTF") = "X"
            End If
        End If

        If txtBemerkung.Text.Length > 0 Then
            NewRow("BEM") = txtBemerkung.Text
        Else
            NewRow("BEM_ENTF") = "X"
        End If

        If txtMahn.Text.Length > 0 Then
            NewRow("MAHNDATUM_AB") = txtMahn.Text
        Else
            NewRow("MAHNDATUM_AB_ENTF") = "X"
        End If

        dt.Rows.Add(NewRow)
 
        mMahnwesen.ExTable = dt
        mMahnwesen.Save(Session("AppID").ToString, Session.SessionID.ToString, Page)

        If mMahnwesen.Status <> 0 Then
            lblErr.Text = mMahnwesen.Message
        Else
            lblMessage.Text = "Daten gespeichert."

            Dim lbl As Label
            'Die Tabelle und Gridview aktualisieren
            Dim gvRow As GridViewRow

            For Each gvr As GridViewRow In GridView1.Rows
                lbl = (CType(gvr.FindControl("lblID"), Label))
                If lbl.Text = lblID.Text Then
                    gvRow = gvr
                    Exit For
                End If
            Next

            If ddlMahnsperre.SelectedValue <> 0 Then
                If ddlMahnsperre.SelectedValue = 1 Then
                    selRow(0)("MAHNSP_GES_AM") = Date.Today.ToShortDateString()
                    selRow(0)("MAHNSP_GES_US") = m_User.UserName

                    lbl = (CType(gvRow.FindControl("lblMahnsperreGesAm"), Label))
                    lbl.Text = Date.Today.ToShortDateString()

                    lbl = (CType(gvRow.FindControl("lblMahnsperreGesDurch"), Label))
                    lbl.Text = m_User.UserName
                End If
                If ddlMahnsperre.SelectedValue = 2 Then
                    selRow(0)("MAHNSP_ENTF_AM") = Date.Today.ToShortDateString()
                    selRow(0)("MAHNSP_ENTF_US") = m_User.UserName

                    lbl = (CType(gvRow.FindControl("lblMahnsperreEntfAm"), Label))
                    lbl.Text = Date.Today.ToShortDateString()

                    lbl = (CType(gvRow.FindControl("lblMahnsperreEntfDurch"), Label))
                    lbl.Text = m_User.UserName
                End If
            End If

            lbl = (CType(gvRow.FindControl("lblBemerkung"), Label))

            If txtBemerkung.Text.Length > 0 Then
                selRow(0)("BEM") = txtBemerkung.Text
                
                lbl.Text = txtBemerkung.Text
            Else
                selRow(0)("BEM") = ""

                lbl.Text = ""
            End If

            lbl = (CType(gvRow.FindControl("lblMahndatumAb"), Label))

            If txtMahn.Text.Length > 0 Then
                selRow(0)("MAHNDATUM_AB") = txtMahn.Text

                lbl.Text = Date.Today.ToShortDateString()
            Else
                selRow(0)("MAHNDATUM_AB") = DBNull.Value
                lbl.Text = ""
            End If

            btnOK.Visible = False
        End If
    End Sub

    Protected Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        divMessage.Visible = False
        divBackDisabled.Visible = False
    End Sub

    Private Sub lbBack_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lbBack.Click
        Response.Redirect("/Services/(S(" & Session.SessionID & "))/Start/Selection.aspx?AppID=" & Session("AppID").ToString, False)
    End Sub
End Class