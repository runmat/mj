Imports KBS.KBS_BASE

Partial Public Class Change09_2
    Inherits Page

    Private mObjKasse As Kasse
    Private mObjInventur As Inventur
    Private mZaehlung As Boolean

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
            mZaehlung = CBool(IIf(Not mObjInventur.Produktierarchie.Select("PRODH = '" & mObjInventur.ProdHNr & "'")(0)("ZAEHLVH") Is String.Empty, True, False))
        Else
            Throw New Exception("benötigtes Session Objekt nicht vorhanden")
        End If

        If Not IsPostBack Then
            lblProdH.Text = mObjInventur.ProdHBezeichnung

            If Not mObjInventur.InvMaterialien Is Nothing Then
                If mObjInventur.InvMaterialien.Rows.Count > 0 Then

                    If mZaehlung = True Then
                        FillalreadyInserted()
                    End If
                End If
            End If
        End If
    End Sub

    Public Sub txtMenge_TextChanged(ByVal sender As Object, ByVal e As EventArgs)

    End Sub

    Private Sub txtEAN_TextChanged(ByVal sender As Object, ByVal e As EventArgs) Handles txtEAN.TextChanged
        If Not mObjInventur.getArtikel(txtEAN.Text, txtMaterialnummer.Text, lblArtikelbezeichnung.Text) Then
            If mObjInventur.ErrorOccured Then
                lblError.Text = mObjInventur.ErrorMessage
            Else
                lblError.Text = "Artikel nicht vorhanden"
            End If

            lblArtikelbezeichnung.Text = "(wird automatisch ausgefüllt)"
            txtMaterialnummer.Text = ""
            lbtnInsert.Visible = False
        Else
            lbtnInsert.Visible = True
            SetFocus(txtMenge)
        End If
    End Sub

    Protected Sub lbtnInsert_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lbtnInsert.Click
        doinsertEan()
    End Sub

    Private Sub FillalreadyInserted()

        With mObjInventur
            If .InvMaterialien.Select("ERFMG > 0").Count > 0 Then
                FillGrid()
            End If
        End With

    End Sub

    Private Sub doinsertEan()
        lblError.Text = ""
        lblError.ForeColor = Drawing.Color.Red
        If txtMenge.Text.Trim(" "c).Length > 0 Then
            If Not txtMenge.Text = "" Then
                'wenn dies gefüllt, dann Artikel korrekt
                With mObjInventur
                    If .InvMaterialien.Select("MATNR='" & txtMaterialnummer.Text.TrimStart("0"c) & "'").Count = 1 Then
                        Dim dRow As DataRow = .InvMaterialien.Select("MATNR='" & txtMaterialnummer.Text.TrimStart("0"c) & "'")(0)
                        If Not CInt(dRow("ERFMG")) > 0 Then
                            dRow("ERFMG") = txtMenge.Text
                        Else
                            dRow("ERFMG") = CInt(dRow("ERFMG")) + CInt(txtMenge.Text)
                            lblError.Text = "Artikel bereits vorhanden, Menge wurde addiert!"
                        End If
                        txtEAN.Text = ""
                        txtMaterialnummer.Text = ""
                        txtMenge.Text = ""
                        lblArtikelbezeichnung.Text = "(wird automatisch ausgefüllt)"
                        lbAbsenden.Visible = True
                        lbohneArtikel.Visible = False
                        FillGrid()
                        SetFocus(txtEAN)
                    Else
                        lblError.Text = "Artikel konnte nicht hinzugefügt werden! Bitte wenden Sie sich an die Abteilung Einkauf in Ahrensburg."
                    End If
                End With

            End If
        Else
            lblError.Text = "Bitte geben sie eine Menge ein"
        End If

    End Sub

    Private Sub FillGrid(Optional ByVal strSort As String = "")

        Dim tmpDataView As New DataView(mObjInventur.InvMaterialien)

        tmpDataView.RowFilter = "ERFMG > 0" '=

        If tmpDataView.Count = 0 Then
            GridView1.Visible = False
            lblNoData.Visible = True
        Else
            GridView1.Visible = True
            lblNoData.Visible = False

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

            GridView1.DataSource = tmpDataView

            GridView1.DataBind()
        End If
    End Sub

    Private Sub FillGrid2()
        Dim tmpDataView As New DataView(mObjInventur.InvMaterialien)
        tmpDataView.RowFilter = "ERFMG = '0'"

        If tmpDataView.Count = 0 Then
            GridView2.Visible = False
            lblNoData.Visible = True
        Else
            GridView2.Visible = True
            lblNoData.Visible = False

            GridView2.DataSource = tmpDataView
            GridView2.DataBind()
        End If
    End Sub

    Protected Sub lbAbsenden_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lbAbsenden.Click
        checkGrid()
        doSubmit()
    End Sub

    Private Sub checkGrid()
        For Each row As GridViewRow In GridView1.Rows
            Dim ArtikelRow As DataRow = mObjInventur.InvMaterialien.Select("MATNR='" & CType(row.FindControl("lblMatnr"), Label).Text & "'")(0)
            ArtikelRow("ERFMG") = CType(row.FindControl("txtMenge"), TextBox).Text
            mObjInventur.InvMaterialien.AcceptChanges()
        Next

    End Sub

    Private Sub doSubmit()

        mObjInventur.Zaehlung = mObjInventur.Create_ZaehlungTab()

        Dim Row As DataRow
        Dim NewRow As DataRow

        For Each Row In mObjInventur.InvMaterialien.Rows
            If CInt(Row("ERFMG")) > 0 Or CBool(Row("Delete")) = True Then
                NewRow = mObjInventur.Zaehlung.NewRow
                NewRow("MATNR") = Row("MATNR").ToString.PadLeft(18, CChar("0"))
                If (CBool(Row("Delete")) = True) Then
                    NewRow("ERFMG") = "0"
                Else
                    NewRow("ERFMG") = CInt(Row("ERFMG")).ToString
                End If
                mObjInventur.Zaehlung.Rows.Add(NewRow)

            Else
                NewRow = mObjInventur.Zaehlung.NewRow
                NewRow("MATNR") = Row("MATNR").ToString.PadLeft(18, CChar("0"))
                NewRow("ERFMG") = "0"
                mObjInventur.Zaehlung.Rows.Add(NewRow)

            End If
        Next

        mObjInventur.SetMengeMaterialERP()

        If mObjInventur.ErrorOccured Then
            lblError.ForeColor = Drawing.Color.Red
            lblError.Text = "Ihre Inventur für " & mObjInventur.ProdHBezeichnung & "  ist fehlgeschlagen. <br><br> " & mObjInventur.ErrorMessage
            lblError.Visible = True
        Else
            lblError.ForeColor = Drawing.Color.Green
            lblError.Text = "Ihre Zählung wurde gespeichert. "
            lblError.Visible = True
        End If

    End Sub

    Protected Sub lb_zurueck_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lb_zurueck.Click
        mObjInventur = Nothing
        Session("mObjInventur") = Nothing
        Response.Redirect("Change09.aspx")
    End Sub

    Private Sub GridView1_RowCommand(ByVal sender As Object, ByVal e As GridViewCommandEventArgs) Handles GridView1.RowCommand
        If e.CommandName = "entfernen" Then
            For Each row As GridViewRow In GridView1.Rows
                If CType(row.FindControl("lblMatnr"), Label).Text = e.CommandArgument.ToString Then
                    mObjInventur.InvMaterialien.Select("MATNR='" & e.CommandArgument.ToString & "'")(0)("ERFMG") = "0"
                    mObjInventur.InvMaterialien.Select("MATNR='" & e.CommandArgument.ToString & "'")(0)("Delete") = True
                    Session("mObjInventur") = mObjInventur
                    Exit For
                End If
            Next
        ElseIf e.CommandName = "plusMenge" Then
            For Each row As GridViewRow In GridView1.Rows
                If CType(row.FindControl("lblMatnr"), Label).Text = e.CommandArgument.ToString Then
                    mObjInventur.InvMaterialien.Select("MATNR='" & e.CommandArgument.ToString & "'")(0)("ERFMG") = _
                    CInt(CType(row.FindControl("txtMenge"), TextBox).Text) + 1
                    Session("mObjInventur") = mObjInventur
                    Exit For
                End If
            Next
        ElseIf e.CommandName = "minusMenge" Then
            For Each row As GridViewRow In GridView1.Rows
                If CType(row.FindControl("lblMatnr"), Label).Text = e.CommandArgument.ToString Then
                    If Not CInt(CType(row.FindControl("txtMenge"), TextBox).Text) - 1 < 0 Then
                        mObjInventur.InvMaterialien.Select("MATNR='" & e.CommandArgument.ToString & "'")(0)("ERFMG") = _
                        CInt(CType(row.FindControl("txtMenge"), TextBox).Text) - 1
                    End If

                    Session("mObjInventur") = mObjInventur
                    Exit For
                End If
            Next
        End If
        FillGrid()
    End Sub

    Protected Sub lbohneArtikel_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lbohneArtikel.Click

    End Sub

    Private Sub lbBestellungOk_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lbBestellungOk.Click

        mpeBestellungsCheck.Hide()
    End Sub

    Private Sub lbOffeneArtikel_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lbOffeneArtikel.Click
        FillGrid2()
        mpeBestellungsCheck.Show()
    End Sub

End Class