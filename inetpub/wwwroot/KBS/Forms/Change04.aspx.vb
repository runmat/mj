Imports KBS.KBS_BASE

Partial Public Class Change04
    Inherits Page
    Private mObjKasse As Kasse
    Private mObjZentrallager As Zentrallager

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
        If mObjZentrallager Is Nothing Then
            mObjZentrallager = mObjKasse.Zentrallager(Me)
        End If
        If Not IsPostBack Then

            If mObjKasse.Master Then
                txtKST.Enabled = True
                ddlArtikel.Enabled = False
                txtMenge.Enabled = False
                lbtnInsert.Enabled = False
                txtFreitext.Enabled = False
                lbtFreitextSend.Enabled = False
            Else
                txtKST.Enabled = False
                txtKST.Text = mObjKasse.Lagerort
                fillDropdown()
            End If
        End If

    End Sub

    Public Sub fillDropdown()
        mObjZentrallager.KostStelle = mObjKasse.Lagerort
        mObjZentrallager.ShowERP()
        If mObjZentrallager.ErrorOccured Then
            lblError.Text = "Es konnten keine Artikel geladen werden!"
        Else
            With mObjZentrallager

                Dim tmpItem As ListItem
                Dim i As Int32 = 0
                Dim dtview As DataView = .Artikel.DefaultView
                dtview.Sort = "ARTBEZ ASC"
                ddlArtikel.Items.Clear()
                Do While i < .Artikel.Rows.Count
                    tmpItem = New ListItem(dtview.Item(i)("ARTBEZ").ToString, dtview.Item(i)("ARTLIF").ToString)
                    ddlArtikel.Items.Add(tmpItem)
                    i += 1
                Loop

            End With
            Dim sVEinheit As String = mObjZentrallager.Artikel.Select("ARTLIF='" & ddlArtikel.SelectedValue & "'")(0)("VMEINS").ToString
            lblVerpEinheit.Text = sVEinheit
            Session("mObjZentrallager") = mObjZentrallager
            FillGrid()
        End If
    End Sub

    Protected Sub lbtnInsert_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lbtnInsert.Click
        doinsert()
    End Sub

    Private Sub doinsert()

        If txtMenge.Text.Trim(" "c).Length > 0 Then
            If Not txtMenge.Text = "" Then
                'wenn dies gefüllt, dann Artikel korrekt
                If mObjZentrallager.Bestellungen.Select("ARTLIF='" & ddlArtikel.SelectedValue & "'").Count = 0 Then
                    Dim sVEinheit As String = mObjZentrallager.Artikel.Select("ARTLIF='" & ddlArtikel.SelectedValue & "'")(0)("VMEINS").ToString

                    mObjZentrallager.insertIntoBestellungen(ddlArtikel.SelectedValue, CInt(txtMenge.Text), ddlArtikel.SelectedItem.Text, sVEinheit)
                    txtMenge.Text = ""
                    FillGrid()
                Else
                    lblError.Text = "Artikel ist in der aktuellen Bestellung schon enthalten"
                End If
            End If
        Else
            lblError.Text = "Bitte geben sie eine Menge ein"
        End If
    End Sub

    Private Sub FillGrid(Optional ByVal strSort As String = "")

        Dim tmpDataView As New DataView(mObjZentrallager.Bestellungen)

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

        Dim tmpDataView As New DataView(mObjZentrallager.Bestellungen)

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

    Private Sub FillGridLetzteBestellungen()

        Dim tmpDataView As New DataView(mObjZentrallager.LetzteBestellungen)
        tmpDataView.Sort = "Bestelldatum DESC"

        If tmpDataView.Count = 0 Then
            gvLetzteBestellungen.Visible = False
            lblNoDataLetzteBestellungen.Visible = True
        Else
            gvLetzteBestellungen.Visible = True
            lblNoDataLetzteBestellungen.Visible = False

            gvLetzteBestellungen.DataSource = tmpDataView
            gvLetzteBestellungen.DataBind()
        End If

    End Sub

    Private Sub doSubmit()
        mObjZentrallager.KostStelle = mObjKasse.Lagerort
        mObjZentrallager.SendToKost = txtKST.Text
        mObjZentrallager.ChangeERP(mObjKasse.Master)
        If mObjZentrallager.ErrorOccured Then
            lblBestellMeldung.ForeColor = Drawing.Color.Red
            lblBestellMeldung.Text = "Ihre Bestellung ist fehlgeschlagen: <br><br> " & mObjZentrallager.ErrorMessage
            MPEBestellResultat.Show()
        Else
            lblBestellMeldung.ForeColor = Drawing.Color.Green
            lblBestellMeldung.Text = "Ihre Bestellung war erfolgreich!"
            MPEBestellResultat.Show()
            mObjZentrallager.Bestellungen.Clear()
            mObjZentrallager = Nothing
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
            mObjZentrallager.Bestellungen.Select("ARTLIF='" & e.CommandArgument.ToString & "'")(0).Delete()
        End If
        FillGrid()
    End Sub

    Private Sub lbAbsenden_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lbAbsenden.Click
        If mObjZentrallager.Bestellungen.Rows.Count = 0 Then
            lblError.Text = "Sie haben keine Artikel für eine Bestellung hinzugefügt"
        Else
            If txtFreitext.Text.Trim.Length > 0 Then
                Dim BestellRow As DataRow = mObjZentrallager.Bestellungen.NewRow
                BestellRow("FREITEXT") = txtFreitext.Text
                mObjZentrallager.Bestellungen.Rows.Add(BestellRow)

            End If
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
        If mObjZentrallager.Bestellungen.Select("ARTLIF='" & ddlArtikel.SelectedValue & "'").Count = 0 Then
            Dim sVEinheit As String = mObjZentrallager.Artikel.Select("ARTLIF='" & ddlArtikel.SelectedValue & "'")(0)("VMEINS").ToString
            lblVerpEinheit.Text = sVEinheit
        End If
    End Sub

    Protected Sub lbtFreitextSend_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lbtFreitextSend.Click
        lblMessage.Text = ""
        If txtFreitext.Text.Trim.Length > 0 Then
            mObjZentrallager.Freitext = txtFreitext.Text
            mObjZentrallager.KostStelle = mObjKasse.Lagerort
            mObjZentrallager.ChangeERP(mObjKasse.Master, "X")
            If mObjZentrallager.ErrorOccured Then
                lblMessage.ForeColor = Drawing.Color.Red
                lblMessage.Text = "Die Anfrage konnte nicht gesendet werden: <br><br> " & mObjZentrallager.ErrorMessage
            Else
                lblMessage.ForeColor = Drawing.Color.Green
                lblMessage.Text = "Die Anfrage wurde erfolgreich gesendet!"
                txtFreitext.Text = ""
                mObjZentrallager.Freitext = ""
            End If
        Else
            lblMessage.ForeColor = Drawing.Color.Red
            lblMessage.Text = "Bitte geben Sie einen Text ein!"
        End If
    End Sub

    Protected Sub txtKST_TextChanged(sender As Object, e As EventArgs) Handles txtKST.TextChanged
        divLetzteBestellungen.Visible = False

        If mObjKasse.Master Then
            If Not String.IsNullOrEmpty(txtKST.Text) Then
                With mObjZentrallager
                    .CheckKostStelleERP(txtKST.Text.Trim)
                    If .ErrorOccured Then
                        lblError.Text = .ErrorMessage
                        SetFocus(txtKST)
                        lblKSTText.Visible = False
                        lblKSTText.Text = ""
                        ddlArtikel.Enabled = False
                        txtMenge.Enabled = False
                        lbtnInsert.Enabled = False
                        txtFreitext.Enabled = False
                        lbtFreitextSend.Enabled = False
                    Else
                        lblKSTText.Visible = True
                        lblKSTText.Text = .KostText
                        fillDropdown()
                        ddlArtikel.Enabled = True
                        txtMenge.Enabled = True
                        lbtnInsert.Enabled = True
                        txtFreitext.Enabled = True
                        lbtFreitextSend.Enabled = True
                        SetFocus(ddlArtikel)
                    End If
                End With
            End If
        Else
            SetFocus(txtKST)
            lblKSTText.Visible = False
            lblKSTText.Text = ""
            ddlArtikel.Enabled = False
            txtMenge.Enabled = False
            lbtnInsert.Enabled = False
            txtFreitext.Enabled = False
            lbtFreitextSend.Enabled = False
        End If
    End Sub

    Private Sub lbLetzteBestellungen_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lbLetzteBestellungen.Click
        If String.IsNullOrEmpty(txtKST.Text) Then
            lblError.Text = "Bitte geben Sie eine Kostenstelle an!"
            Exit Sub
        End If

        ShowLetzteBestellungen()
    End Sub

    Private Sub ShowLetzteBestellungen()
        divLetzteBestellungen.Visible = True
        mObjZentrallager.FillLetzteBestellungen(txtKST.Text)
        If mObjZentrallager.ErrorOccured AndAlso Not mObjZentrallager.ErrorCode = "141" Then
            lblError.Text = "Fehler beim Abrufen der letzten Bestellungen: " & mObjZentrallager.ErrorMessage
        Else
            FillGridLetzteBestellungen()
        End If
    End Sub

End Class