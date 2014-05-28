Imports CKG.Base.Kernel.Common.Common
Imports CKG.Base.Kernel.Security
Imports CKG.Base.Kernel
Imports CKG.Base.Business

Partial Public Class Report02s
    Inherits System.Web.UI.Page

#Region "Declarations"
    Private m_App As App
    Private m_User As User
    Protected WithEvents GridNavigation1 As Global.CKG.Services.GridNavigation
#End Region

#Region "Events"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        m_User = GetUser(Me)
        FormAuth(Me, m_User)
        GetAppIDFromQueryString(Me)
        m_App = New App(m_User)
        GridNavigation1.setGridElment(gvBestand)

        Try
            lblHead.Text = m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString

            m_App = New App(m_User)


            If IsPostBack = False Then
                InitializeForm()
            Else
                For Each chkListItem As ListItem In chkListLeistung.Items
                    chkListItem.Attributes.Add("style", "white-space:nowrap; border:none; font-weight: normal;")
                Next
            End If


            lblError.Text = ""


        Catch ex As Exception
            lblError.Text = "Beim Laden der Seite ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
        End Try
    End Sub

    'Protected Sub ibtCancel_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtCancel.Click
    '    For Each item As ListItem In lstLeistung.Items

    '        item.Selected = False

    '    Next
    'End Sub


    Protected Sub lbCreate_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lbCreate.Click
        ' Response.Redirect("Webform1.aspx")
        DoSubmit()
    End Sub

    Private Sub GridNavigation1_PagerChanged(ByVal PageIndex As Integer) Handles GridNavigation1.PagerChanged
        gvBestand.PageIndex = PageIndex
        FillGrid(PageIndex)
    End Sub

    Private Sub GridNavigation1_PageSizeChanged() Handles GridNavigation1.PageSizeChanged
        FillGrid(0)
    End Sub

    Private Sub gvBestand_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles gvBestand.Sorting
        FillGrid(gvBestand.PageIndex, e.SortExpression)
    End Sub

    Protected Sub NewSearch_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles NewSearch.Click
        lbCreate.Visible = True
        tab1.Visible = True
        Queryfooter.Visible = True
        Result.Visible = False
    End Sub

    Protected Sub lbBack_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lbBack.Click
        Response.Redirect("/Services/(S(" & Session.SessionID & "))/Start/Selection.aspx?AppID=" & Session("AppID").ToString, False)
    End Sub

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        SetEndASPXAccess(Me)
    End Sub

    Private Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Unload
        SetEndASPXAccess(Me)
    End Sub

#End Region

#Region "Methods"

    Private Sub FillGrid(ByVal intPageIndex As Int32, Optional ByVal strSort As String = "")
        Dim tmpDataView As New DataView()

        Dim tblTemp As DataTable = CType(Session("mObjAuswertung"), DataTable)


        tblTemp.AcceptChanges()

        tmpDataView = tblTemp.DefaultView


        If tmpDataView.Count = 0 Then
            Result.Visible = False
            lblError.Text = "zu Ihrer Selektion konnten keine Werte ermittelt werden"
        Else
            tblTemp.DefaultView.RowFilter = "TG='X'"
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
            gvBestand.PageIndex = intTempPageIndex
            gvBestand.DataSource = tmpDataView
            gvBestand.DataBind()

            For i As Integer = 0 To gvBestand.HeaderRow.Cells.Count - 1
                For Each ctrl As Control In gvBestand.HeaderRow.Cells(i).Controls

                    If TypeOf ctrl Is LinkButton Then
                        Dim LinkHead As LinkButton
                        LinkHead = CType(ctrl, LinkButton)
                        If LinkHead.Text.Contains("Erl. in Proz.") Then
                            LinkHead.Text = "%"
                        End If
                    End If

                Next
            Next
            gvBestand.HeaderRow.Cells(1).Visible = False
            For Each mycell As GridViewRow In gvBestand.Rows

                For i As Integer = 2 To mycell.Cells.Count - 1 Step 2
                    Dim ilartCount As Integer = 0
                    ilartCount = CInt(mycell.Cells(i).Text)
                    If IsNumeric(gvBestand.FooterRow.Cells(i).Text) Then
                        ilartCount += CInt(gvBestand.FooterRow.Cells(i).Text)
                    End If

                    gvBestand.FooterRow.Cells(i).Text = ilartCount.ToString

                    gvBestand.FooterRow.Visible = True
                    gvBestand.FooterRow.Cells(i).Visible = True
                    gvBestand.FooterRow.Cells(i).Style("color") = "#ffffff"
                    gvBestand.FooterRow.Cells(i).Style("text-align") = "center"
                    gvBestand.FooterRow.Cells(i).Style("padding-left") = "15px"
                    mycell.Cells(i).Style("text-align") = "center"
                    mycell.Cells(1).Visible = False
                Next

            Next
            gvBestand.FooterRow.Cells(0).Text = "Gesamt"
            gvBestand.FooterRow.Cells(0).Style("color") = "#ffffff"
            gvBestand.FooterRow.Cells(0).Style("padding-left") = "15px"
            gvBestand.FooterRow.Style("font-weight") = "bold"
            gvBestand.FooterRow.Style("height") = "28px"
            gvBestand.FooterRow.Cells(1).Visible = False
            gvBestand.FooterRow.BackColor = Drawing.Color.FromArgb(118, 146, 204)
            lblAnzahl.Text = "Gesamtanzahl Vorgänge: " & CType(Session("mObjAuswertungAnzahl"), String)

            Result.Visible = True

        End If

    End Sub


    Private Sub DoSubmit()

        Dim booFalse As Boolean = False

        If txtDatumVon.Text.Trim.Length = 0 Or txtDatumBis.Text.Trim.Length = 0 Then
            lblError.Text = "Die Felder ""Datum von"" und ""Datum bis"" müssen Werte enthalten!"
            lblError.Visible = True
            Exit Sub
        End If
        If cv_txtDatumVon.IsValid = False Then
            Exit Sub
        End If

        Dim GetData As Auswertung = New Auswertung
        GetData.DatumVon = txtDatumVon.Text
        GetData.DatumBis = txtDatumBis.Text

        Dim ItemList As ListItem
        Dim tmpTable As New DataTable
        tmpTable.Columns.Add("LARNT", GetType(System.String))

        For Each ItemList In chkListLeistung.Items
            If ItemList.Selected = True Then
                Dim NewRow As DataRow = tmpTable.NewRow
                NewRow("LARNT") = ItemList.Value
                tmpTable.Rows.Add(NewRow)
            End If
        Next

        If tmpTable.Rows.Count > 0 Then
            GetData.LeistArt = tmpTable
        End If

        GetData.GetData(m_User, m_App, Me.Page)

        Session("mObjAuswertung") = GetData.Gesamt.Copy
        Session("mObjAuswertungAnzahl") = GetData.Anzahl
        FillGrid(0)

    End Sub


    Private Sub InitializeForm()

        'Leistungsarten
        Dim GetData As New Bestand
        GetData.GetLeistungsart(m_User, m_App, Me.Page)


        If GetData.Leistungsart.Rows.Count > 0 Then

            Dim rdbText As String
            Dim rdbValue As String


            For xAGS As Integer = 0 To GetData.Leistungsart.Rows.Count - 1
                Dim rdvitem As New ListItem

                rdbText = GetData.Leistungsart.Rows(xAGS)("LTXA1").ToString
                rdbValue = GetData.Leistungsart.Rows(xAGS)("LARNT").ToString

                rdvitem.Text = rdbText
                rdvitem.Value = rdbValue
                rdvitem.Attributes.Add("style", "white-space:nowrap; border:none; font-weight: normal;")
                chkListLeistung.Items.Add(rdvitem)
            Next
        End If
    End Sub

    Protected Sub lnkCreateExcel1_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lnkCreateExcel1.Click
        Dim reportExcel As DataTable = CType(Session("mObjAuswertung"), DataTable).Copy
        If reportExcel.Columns.Contains("TG") Then
            reportExcel.Columns.Remove("TG")
        End If


        Try
            Dim excelFactory As New DocumentGeneration.ExcelDocumentFactory()
            Dim strFileName As String = Format(Now, "yyyyMMdd_HHmmss_") & m_User.UserName
            excelFactory.CreateDocumentAndSendAsResponse(strFileName, reportExcel, Me)

        Catch ex As Exception
            lblError.Text = ex.Message
        End Try
    End Sub
#End Region



    'Protected Sub lnkCreatePDF1_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lnkCreatePDF1.Click
    '    Response.Redirect("WebForm1.aspx")
    'End Sub
End Class

