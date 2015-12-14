Imports KBS.KBS_BASE
Partial Public Class Change05
    Inherits Page
    Private mObjKasse As Kasse
    Private mObjVersicherungen As Versicherungen

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load

        FormAuth(Me)
        lblError.Text = ""
        Title = lblHead.Text

        If mObjKasse Is Nothing Then
            If Not Session("mKasse") Is Nothing Then
                mObjKasse = CType(Session("mKasse"), Kasse)
            Else
                Throw New Exception("benötigtes Session Objekt nicht vorhanden")
            End If
        End If
        If mObjVersicherungen Is Nothing Then
            mObjVersicherungen = mObjKasse.Versicherungen(Me)
        End If
        If Not IsPostBack Then
            fillDropdown()
        End If
    End Sub

    Public Sub fillDropdown()

        mObjVersicherungen.ShowERP()
        If mObjVersicherungen.ErrorOccured Then
            lblError.Text = "Es konnten keine Artikel geladen werden!"
        Else
            With mObjVersicherungen

                Dim tmpItem As ListItem
                Dim i As Int32 = 0
                Dim dtview As DataView = .Versicherungen.DefaultView
                dtview.Sort = "MAKTX ASC"
                ddlArtikel.Items.Clear()
                Do While i < .Versicherungen.Rows.Count
                    tmpItem = New ListItem(dtview.Item(i)("MAKTX").ToString, dtview.Item(i)("MATNR").ToString)
                    ddlArtikel.Items.Add(tmpItem)
                    i += 1
                Loop

            End With

            FillGrid()
            Session("mObjVersicherungen") = mObjVersicherungen
        End If
    End Sub

    Protected Sub lbtnInsert_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lbtnInsert.Click
        If IsNumeric(txtMenge.Text) Then
            If CInt(txtMenge.Text) > 50 Then
                MPE_plMenge.Show()
            Else
                doinsert()
            End If
        Else
            lblError.Text = "Bitte geben Sie eine Menge ein!"
        End If

    End Sub

    Private Sub doinsert()

        If IsNumeric(txtPreis.Text) Then
            If CInt(txtPreis.Text) > 0 Then
                Dim dRows() As DataRow = mObjVersicherungen.Versicherungen.Select("MATNR='" & ddlArtikel.SelectedValue & "'")
                If dRows.Length > 0 Then
                    If IsNumeric(dRows(0)("MINDVK")) Then
                        If CInt(txtPreis.Text) < CInt(dRows(0)("MINDVK")) Then
                            lblError.Text = "Mindestpreis unterschritten. Der Mindestpreis liegt bei " & _
                            dRows(0)("MINDVK").ToString & "€. Die Mindestpreise gelten für alle Filialen und sind vom Versicherungstyp abhängig."
                            Exit Sub
                        End If
                    End If
                End If
            ElseIf CInt(txtPreis.Text) = 0 Then
                lblError.Text = "Mindestpreis unterschritten. Eingabe 0,00 € nicht möglich! "
                Exit Sub
            End If
        Else
            lblError.Text = "Geben Sie einen Preis ein!"
            Exit Sub
        End If
        'wenn dies gefüllt, dann Artikel korrekt
        If mObjVersicherungen.Bestellungen.Select("MATNR='" & ddlArtikel.SelectedValue & "'").Count = 0 Then
            mObjVersicherungen.insertIntoBestellungen(ddlArtikel.SelectedValue, CInt(txtMenge.Text), _
                                                         ddlArtikel.SelectedItem.Text, CDbl(txtPreis.Text))
            txtMenge.Text = ""
            FillGrid()
            txtMenge.Text = ""
            txtPreis.Text = ""
            lblError.Text = ""
            lblMessage.Text = ""
            ddlArtikel.SelectedIndex = 0
        ElseIf mObjVersicherungen.Bestellungen.Select("MATNR='" & ddlArtikel.SelectedValue & "'").Count > 0 Then
            Dim RowBest() As DataRow = mObjVersicherungen.Bestellungen.Select("MATNR='" & ddlArtikel.SelectedValue & "'")
            Dim RowEinzel As DataRow
            Dim ProofFlag As Boolean = False
            For Each RowEinzel In RowBest
                If RowEinzel("VKP").ToString <> txtPreis.Text Then
                    ProofFlag = True
                Else
                    ProofFlag = False
                    lblError.Text = "Artikel ist mit dem gewählten Preis bereits in der aktuellen Bestellung enthalten!"
                    Exit For
                End If
            Next
            If ProofFlag = True Then
                mObjVersicherungen.insertIntoBestellungen(ddlArtikel.SelectedValue, CInt(txtMenge.Text), _
                                 ddlArtikel.SelectedItem.Text, CDbl(txtPreis.Text))
                txtMenge.Text = ""
                FillGrid()
                txtMenge.Text = ""
                txtPreis.Text = ""
                lblError.Text = ""
                lblMessage.Text = ""
                ddlArtikel.SelectedIndex = 0
            End If

        End If

    End Sub

    Private Sub FillGrid(Optional ByVal strSort As String = "")

        Dim tmpDataView As New DataView(mObjVersicherungen.Bestellungen)

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

        Dim tmpDataView As New DataView(mObjVersicherungen.Bestellungen)

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

    Private Sub doSubmit()
        mObjVersicherungen.KostStelle = mObjKasse.Lagerort
        mObjVersicherungen.ChangeERP()
        If mObjVersicherungen.ErrorOccured Then
            lblBestellMeldung.ForeColor = Drawing.Color.Red
            lblBestellMeldung.Text = "Ihre Bestellung ist fehlgeschlagen: <br><br> " & mObjVersicherungen.ErrorMessage
            MPEBestellResultat.Show()
        Else
            lblBestellMeldung.ForeColor = Drawing.Color.Green
            lblBestellMeldung.Text = "Ihre Bestellung wurde unter folgenden ID´s angelegt!"
            mObjVersicherungen.Bestellungen.Clear()
            Dim tmpTable As DataTable = mObjVersicherungen.tblErgebnis
            Dim tmpDataView As New DataView(tmpTable)

            If tmpDataView.Count = 0 Then
                GridView3.Visible = False
                lblNoData.Visible = True
            Else
                GridView3.Visible = True
                lblNoData.Visible = False

                GridView3.DataSource = tmpDataView
                GridView3.DataBind()
            End If
            MPEBestellResultat.Show()

            mObjVersicherungen = Nothing
        End If

    End Sub

    Private Sub responseBack()
        Response.Redirect("../Selection.aspx")
    End Sub

    Protected Sub lb_zurueck_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lb_zurueck.Click
        responseBack()
    End Sub

    Private Sub GridView1_RowCommand(ByVal sender As Object, ByVal e As GridViewCommandEventArgs) Handles GridView1.RowCommand
        If e.CommandName = "entfernen" Then
            mObjVersicherungen.Bestellungen.Select("KEY='" & e.CommandArgument.ToString & "'")(0).Delete()
            Dim NewKey As Integer = 1
            For Each dRow As DataRow In mObjVersicherungen.Bestellungen.Rows
                dRow("KEY") = NewKey
                NewKey += 1
            Next
        End If
        FillGrid()
    End Sub

    Private Sub lbAbsenden_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lbAbsenden.Click
        If mObjVersicherungen.Bestellungen.Rows.Count = 0 Then
            lblError.Text = "Sie haben keine Artikel für eine Bestellung hinzugefügt"
        Else
            FillGrid2()
            mpeBestellungsCheck.Show()
        End If
    End Sub

    Private Sub lbBestellungOk_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lbBestellungOk.Click
        mpeBestellungsCheck.Hide()
        doSubmit()
    End Sub

    Protected Sub lbBestellFinalize_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lbBestellFinalize.Click
        Response.Redirect("../Selection.aspx")
    End Sub

    Private Sub ddlArtikel_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ddlArtikel.SelectedIndexChanged
        txtMenge.Text = ""
        txtPreis.Text = ""
        lblError.Text = ""
        lblMessage.Text = ""
    End Sub

    Private Sub lbtnOK_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lbtnOK.Click
        doinsert()
    End Sub

End Class