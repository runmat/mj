Imports KBS.KBS_BASE

Public Class Change09_KBS
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
            lblProdH.Text = mObjInventur.ProdHBezeichnung
        End If
    End Sub

    Private Sub txtEAN_TextChanged(ByVal sender As Object, ByVal e As EventArgs) Handles txtEAN.TextChanged
        Dim SapMenge As String = ""
        If Not mObjInventur.getEANFromSAPKBSERP(txtEAN.Text, txtMaterialnummer.Text, lblArtikelbezeichnung.Text, SapMenge) Then
            If mObjInventur.E_MESSAGE.Length > 0 Then
                lblError.Text = mObjInventur.E_MESSAGE
            Else
                lblError.Text = "Artikel nicht vorhanden"
            End If

            lblArtikelbezeichnung.Text = "(wird automatisch ausgefüllt)"
            txtMaterialnummer.Text = ""
            lblSapMenge.Text = ""
            lbtnInsert.Visible = False
        Else
            lbtnInsert.Visible = True

            lblSapMenge.Text = SapMenge
            txtMenge.Text = "0"
            If CInt(SapMenge) > 0 Then
                lbtnAdd.Visible = True
                lbtnOverWrite.Visible = True
                lbtnInsert.Visible = False
            Else
                lbtnAdd.Visible = False
                lbtnOverWrite.Visible = False
                lbtnInsert.Visible = True
            End If
            SetFocus(txtMenge)
        End If
    End Sub

    Protected Sub lbtnInsert_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lbtnInsert.Click
        doinsertEan("", "erfasst")
    End Sub

    Private Sub doinsertEan(ByVal sAdd As String, ByVal Status As String)
        lblError.Text = ""
        lblError.ForeColor = Drawing.Color.Red

        If txtMenge.Text.Trim(" "c).Length > 0 Then
            If Not txtMenge.Text = "" Then
                'wenn dies gefüllt, dann Artikel korrekt
                With mObjInventur

                    mObjInventur.SetMengeMaterialKBSERP(txtMaterialnummer.Text, txtMenge.Text, sAdd)
                    If mObjInventur.E_MESSAGE.Length = 0 Then
                        If sAdd = "X" Then
                            Dim iMengeAdd As Integer = 0
                            Try
                                iMengeAdd = CInt(lblSapMenge.Text) + CInt(txtMenge.Text)
                            Catch ex As Exception
                                lblError.Text = "Fehler bei der Addition der Mengen!"
                                Exit Sub
                            End Try
                            mObjInventur.AddHistorieEntry(txtEAN.Text, lblArtikelbezeichnung.Text, txtMaterialnummer.Text, iMengeAdd, txtMenge.Text, Status)
                        Else
                            mObjInventur.AddHistorieEntry(txtEAN.Text, lblArtikelbezeichnung.Text, txtMaterialnummer.Text, txtMenge.Text, txtMenge.Text, Status)
                        End If

                        FillGrid()
                        txtEAN.Text = ""
                        txtMaterialnummer.Text = ""
                        txtMenge.Text = ""
                        lblSapMenge.Text = ""
                        lblArtikelbezeichnung.Text = "(wird automatisch ausgefüllt)"
                        SetFocus(txtEAN)
                    Else
                        lblError.Text = mObjInventur.E_MESSAGE
                    End If

                End With

            End If
        Else
            lblError.Text = "Bitte geben sie eine Menge ein"
        End If

    End Sub

    Private Sub FillGrid(Optional ByVal strSort As String = "")

        Dim tmpDataView As New DataView(mObjInventur.tblHistorie)

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

    Protected Sub lb_zurueck_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lb_zurueck.Click
        mObjInventur = Nothing
        Session("mObjInventur") = Nothing
        Response.Redirect("Change09.aspx")
    End Sub

    Private Sub GridView1_RowCommand(ByVal sender As Object, ByVal e As GridViewCommandEventArgs) Handles GridView1.RowCommand
        If e.CommandName = "entfernen" Then
            Dim ArrRows() As DataRow = mObjInventur.tblHistorie.Select("Index='" & e.CommandArgument.ToString & "'")
            If ArrRows.Length = 1 Then
                mObjInventur.SetMengeMaterialKBSERP(ArrRows(0)("Matnr").ToString, 0, "")
                mObjInventur.tblHistorie.Rows.Remove(ArrRows(0))
                FillGrid()
            End If
            Session("mObjInventur") = mObjInventur
        ElseIf e.CommandName = "bearbeiten" Then
            Dim ArrRows() As DataRow = mObjInventur.tblHistorie.Select("Index='" & e.CommandArgument.ToString & "'")
            If ArrRows.Length = 1 Then
                lblArtikelbez.Text = ArrRows(0)("MAKTX").ToString
                lblEANShow.Text = ArrRows(0)("EAN").ToString
                lblStatusShow.Text = ArrRows(0)("Status").ToString
                lblMengealt.Text = ArrRows(0)("Menge_erfasst").ToString
                txtEditMenge.Text = ArrRows(0)("Menge_erfasst").ToString
                lblMaterial.Text = ArrRows(0)("MATNR").ToString
                mpeBestellungsCheck.Show()
            End If

        End If
    End Sub

    Protected Sub lbtnOverWrite_Click(sender As Object, e As EventArgs) Handles lbtnOverWrite.Click
        doinsertEan("", "überschrieben")
    End Sub

    Protected Sub lbtnAdd_Click(sender As Object, e As EventArgs) Handles lbtnAdd.Click
        doinsertEan("X", "addiert")
    End Sub

    Protected Sub lbtnEditOverWrite_Click(sender As Object, e As EventArgs) Handles lbtnEditOverWrite.Click
        lblEditError.Text = ""
        If txtEditMenge.Text.Trim(" "c).Length > 0 Then
            If Not txtEditMenge.Text = "" Then
                'wenn dies gefüllt, dann Artikel korrekt
                With mObjInventur

                    mObjInventur.SetMengeMaterialKBSERP(lblMaterial.Text, txtEditMenge.Text, "")
                    If mObjInventur.E_MESSAGE.Length = 0 Then

                        mObjInventur.AddHistorieEntry(lblEANShow.Text, lblArtikelbez.Text, lblMaterial.Text, txtEditMenge.Text, txtEditMenge.Text, "überschrieben")
                        FillGrid()
                        lblEANShow.Text = ""
                        lblArtikelbez.Text = ""
                        lblMaterial.Text = ""
                        lblArtikelbez.Text = ""
                        SetFocus(txtEAN)
                        mpeBestellungsCheck.Hide()
                    Else
                        lblError.Text = mObjInventur.E_MESSAGE
                        mpeBestellungsCheck.Show()
                    End If

                End With

            End If
        Else
            lblEditError.Text = "Bitte geben sie eine Menge ein"
            mpeBestellungsCheck.Show()
        End If
    End Sub

    Protected Sub lbtnEditAdd_Click(sender As Object, e As EventArgs) Handles lbtnEditAdd.Click
        lblEditError.Text = ""
        If txtEditMenge.Text.Trim(" "c).Length > 0 Then
            If Not txtEditMenge.Text = "" Then
                'wenn dies gefüllt, dann Artikel korrekt
                With mObjInventur

                    mObjInventur.SetMengeMaterialKBSERP(lblMaterial.Text, txtEditMenge.Text, "X")
                    If mObjInventur.E_MESSAGE.Length = 0 Then
                        Dim iMengeAdd As Integer = 0
                        Try
                            iMengeAdd = CInt(lblErfMenge.Text) + CInt(txtMenge.Text)
                        Catch ex As Exception
                            lblError.Text = "Fehler bei der Addition der Mengen!"
                            Exit Sub
                        End Try
                        mObjInventur.AddHistorieEntry(txtEAN.Text, lblArtikelbezeichnung.Text, txtMaterialnummer.Text, iMengeAdd, txtMenge.Text, "addiert")

                        FillGrid()
                        lblEANShow.Text = ""
                        lblArtikelbez.Text = ""
                        lblMaterial.Text = ""
                        lblArtikelbez.Text = ""
                        SetFocus(txtEAN)
                        mpeBestellungsCheck.Hide()
                    Else
                        lblError.Text = mObjInventur.E_MESSAGE
                        mpeBestellungsCheck.Show()
                    End If

                End With

            End If
        Else
            lblEditError.Text = "Bitte geben sie eine Menge ein"
            mpeBestellungsCheck.Show()
        End If
    End Sub

End Class