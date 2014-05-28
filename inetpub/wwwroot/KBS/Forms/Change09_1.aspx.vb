Imports KBS.KBS_BASE

Partial Public Class Change09_1
    Inherits Page

    Private mObjKasse As Kasse
    Private mObjInventur As Inventur

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        FormAuth(Me)

        Title = lblHead.Text
        If mObjKasse Is Nothing Then
            If Not Session("mKasse") Is Nothing Then
                mObjKasse = CType(Session("mKasse"), Kasse)
            Else
                Throw New Exception("benötigtes Session Objekt nicht vorhanden")
            End If
        End If
        If Not Session("mObjInventur") Is Nothing Then
            mObjInventur = CType(Session("mObjInventur"), Inventur)
        Else
            Throw New Exception("benötigtes Session Objekt nicht vorhanden")
        End If

        If Not IsPostBack Then
            lblProdHBezeichnung.Text = mObjInventur.ProdHBezeichnung
            If mObjInventur.ErfTyp = "1" Then
                FillGrid(0)
                checkgrid()
                lblGesamt.Text = mObjInventur.ProdHBezeichnung & " gesamt:"
            End If

        End If
    End Sub

    Private Sub FillGrid(ByVal intPageIndex As Int32, Optional ByVal strSort As String = "")

        Dim tmpDataView As New DataView(mObjInventur.InvMaterialien)

        If tmpDataView.Count = 0 Then
            GridView3.Visible = False
        Else
            GridView3.Visible = True

            Dim intTempPageIndex As Int32 = intPageIndex
            Dim strTempSort As String = ""
            Dim strDirection As String = ""

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

            GridView3.PageIndex = intTempPageIndex
            GridView3.DataSource = tmpDataView

            GridView3.DataBind()

        End If
    End Sub

    Protected Sub lb_zurueck_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lb_zurueck.Click
        mObjInventur = Nothing
        Session("mObjInventur") = Nothing
        Response.Redirect("Change09.aspx")
    End Sub

    Protected Sub lbAbsenden_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lbAbsenden.Click
        checkgrid()
        doSubmit()
    End Sub

    Private Sub checkgrid()
        Dim i As Integer = 0
        For Each tmprow As GridViewRow In GridView3.Rows
            Dim tmpPosition As DataRow = mObjInventur.InvMaterialien.Select("MATNR='" & CType(tmprow.FindControl("lblMatnr"), Label).Text & "'")(0)
            Dim tmpMenge As String

            tmpMenge = CType(tmprow.FindControl("txtMenge"), TextBox).Text
            If tmpMenge.Trim = "" Then
                tmpPosition("ERFMG") = 0
            Else
                tmpPosition("ERFMG") = tmpMenge
            End If

            i += CInt(tmpPosition("ERFMG"))
            mObjInventur.InvMaterialien.AcceptChanges()
        Next
        lblGesamtShow.Text = i.ToString
    End Sub

    Private Sub doSubmit()

        mObjInventur.Zaehlung = mObjInventur.Create_ZaehlungTab()

        Dim Row As DataRow
        Dim NewRow As DataRow

        For Each Row In mObjInventur.InvMaterialien.Rows
            If CInt(Row("ERFMG").ToString) >= 0 Then
                NewRow = mObjInventur.Zaehlung.NewRow
                NewRow("MATNR") = Row("MATNR").ToString.PadLeft(18, CChar("0"))
                NewRow("ERFMG") = Row("ERFMG").ToString
                mObjInventur.Zaehlung.Rows.Add(NewRow)
            End If
        Next

        If CInt(lblGesamtShow.Text) <= 3000 Then

            mObjInventur.SetMengeMaterialERP()

            Select Case mObjInventur.E_SUBRC
                Case 0
                    'ok
                    lblError.ForeColor = Drawing.Color.Green
                    lblError.Text = "Ihre Zählung wurde gespeichert. "
                    lblError.Visible = True
                    Session("mObjInventur") = mObjInventur
                Case -1
                    lblError.ForeColor = Drawing.Color.Red
                    lblError.Text = "Ihre Inventur für " & mObjInventur.ProdHBezeichnung & "  ist fehlgeschlagen. <br><br> " & mObjInventur.E_MESSAGE
                    lblError.Visible = True
                Case Else
                    lblError.ForeColor = Drawing.Color.Red
                    lblError.Text = "Ihre Inventur für " & mObjInventur.ProdHBezeichnung & "  ist fehlgeschlagen. <br><br> " & mObjInventur.E_MESSAGE
                    lblError.Visible = True
            End Select
        Else
            lblErrorMenge.Text = "Überprüfen Sie Ihre Eingaben! Die Gesamtsumme ist ungewöhnlich hoch!<br/>Sollen die Daten trotzdem gespeichert werden?"
            mpeBestellungsCheck.Show()
            lblError.ForeColor = Drawing.Color.Red
            lblError.Text = "Bitte überprüfen Sie Ihre Eingaben! Die Gesamtsumme ist ungewöhnlich hoch!"
            lblError.Visible = True

        End If

    End Sub

    Protected Sub GridView3_RowDataBound(ByVal sender As Object, ByVal e As GridViewRowEventArgs) Handles GridView3.RowDataBound

    End Sub

    Protected Sub lbBestellungOk_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lbBestellungOk.Click
        mObjInventur.Zaehlung = mObjInventur.Create_ZaehlungTab()

        Dim Row As DataRow
        Dim NewRow As DataRow

        For Each Row In mObjInventur.InvMaterialien.Rows
            If CInt(Row("ERFMG").ToString) >= 0 Then
                NewRow = mObjInventur.Zaehlung.NewRow
                NewRow("MATNR") = Row("MATNR").ToString.PadLeft(18, CChar("0"))
                NewRow("ERFMG") = Row("ERFMG").ToString
                mObjInventur.Zaehlung.Rows.Add(NewRow)
            End If
        Next

        mObjInventur.SetMengeMaterialERP()

        Select Case mObjInventur.E_SUBRC
            Case 0
                'ok
                lblError.ForeColor = Drawing.Color.Green
                lblError.Text = "Ihre Inventur für " & mObjInventur.ProdHBezeichnung & "  war erfolgreich. "
                lblError.Visible = True
                Session("mObjInventur") = mObjInventur
            Case -1
                lblError.ForeColor = Drawing.Color.Red
                lblError.Text = "Ihre Inventur für " & mObjInventur.ProdHBezeichnung & "  ist fehlgeschlagen. <br><br> " & mObjInventur.E_MESSAGE
                lblError.Visible = True
            Case Else
                lblError.ForeColor = Drawing.Color.Red
                lblError.Text = "Ihre Inventur für " & mObjInventur.ProdHBezeichnung & "  ist fehlgeschlagen. <br><br> " & mObjInventur.E_MESSAGE
                lblError.Visible = True
        End Select

    End Sub

End Class