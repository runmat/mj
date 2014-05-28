Imports KBS.KBS_BASE
Imports System.IO

''' <summary>
''' Veraltete Seite!!! Wurde abgelöst durch <see cref="Bestellung_Platinen">Bestellung_Platinen</see> 
''' </summary>
''' <remarks></remarks>

Partial Public Class Change03
    Inherits Page

    Private mObjKasse As Kasse
    Private mObjPlatinen As Platinen

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
        If mObjPlatinen Is Nothing Then
            mObjPlatinen = mObjKasse.Platinen(Me)
        End If

        If Not IsPostBack Then

            mObjPlatinen.SelReiter = "1"

            'Wurde schon an SAP übermittelt?
            Session.Add("SendToSap", False)
            Session("Commited") = Nothing

            If mObjKasse.Master Then
                txtKST.Enabled = True
                ddlLieferant.Enabled = False
                chkGeliefert.Enabled = False
                txtLieferscheinnummer.Enabled = False
                ibtnOptionalShow.Enabled = False
                GridView1.Enabled = False
                GridView3.Enabled = False
                For Each ctrl As Control In tabContainer.Controls
                    If TypeOf ctrl Is LinkButton Then
                        Dim lnkBut As LinkButton
                        lnkBut = CType(ctrl, LinkButton)
                        lnkBut.Enabled = False
                    End If
                Next
                lbAbsenden.Enabled = False
            Else
                txtKST.Enabled = False
                txtKST.Text = mObjKasse.Lagerort
                fillDropdown()
            End If
        Else
            Dim eventArg As String = Request("__EVENTARGUMENT")
            If eventArg = "MyCustomArgument" Then
                txtBedienerkarte_TextChanged()
            Else
                addButtonHandler()
            End If

        End If
        txtBedienerkarte.Attributes.Add("onkeyup", "javascript:ControlField(this);")
    End Sub

    Sub addButtonHandler()
        If Not mObjPlatinen.Reiter Is Nothing Then
            If mObjPlatinen.Reiter.Rows.Count > 0 Then
                tabContainer.Controls.Clear()

                For iReiter As Integer = 0 To mObjPlatinen.Reiter.Rows.Count - 1
                    Dim imgButton As New LinkButton
                    imgButton.CssClass = "TabButton"
                    If iReiter = 0 AndAlso mObjPlatinen.SelReiter = "1" Then imgButton.CssClass = "TabButtonActive"
                    imgButton.Width = Unit.Pixel(90)
                    imgButton.Height = Unit.Pixel(16)
                    imgButton.Text = mObjPlatinen.Reiter.Rows(iReiter)("BEZ").ToString
                    imgButton.ID = mObjPlatinen.Reiter.Rows(iReiter)("REITER").ToString & ddlLieferant.SelectedValue
                    imgButton.CommandArgument = imgButton.ID
                    AddHandler imgButton.Command, AddressOf LinkButton1_Click
                    tabContainer.Controls.Add(imgButton)

                    Dim UPTrigger As New PostBackTrigger
                    UPTrigger.ControlID = imgButton.ID

                Next
            Else
                GridView1.Visible = False
                GridView3.Visible = False

                lblError.Text = "Es konnten keine Artikel zum ausgewählten Lieferanten geladen werden!"
            End If
        Else
            GridView1.Visible = False
            GridView3.Visible = False
            lblError.Text = "Es konnten keine Artikel zum ausgewählten Lieferanten geladen werden!"
        End If


    End Sub

    Public Sub fillDropdown()
        mObjPlatinen.KostStelle = mObjKasse.Lagerort
        If mObjPlatinen.SendToKost <> "" Then
            mObjPlatinen.getLieferantenERP(mObjPlatinen.SendToKost, mObjKasse.Master, mObjKasse.Firma)
        Else
            mObjPlatinen.getLieferantenERP(mObjKasse.Lagerort, mObjKasse.Master, mObjKasse.Firma)
        End If

        If Not mObjPlatinen.ErrorOccured Then

            With mObjPlatinen

                Dim tmpItem As ListItem
                ddlLieferant.DataSource = .Lieferanten
                Dim i As Int32 = 0
                ddlLieferant.Items.Clear()
                .SelLief = 0
                Do While i < mObjPlatinen.Lieferanten.Rows.Count
                    tmpItem = New ListItem(.Lieferanten.Rows(i)("NAME1").ToString, .Lieferanten.Rows(i)("LIFNR").ToString)
                    ddlLieferant.Items.Add(tmpItem)

                    If .Lieferanten.Rows(i)("HAUPT").ToString = "X" Then
                        ddlLieferant.Items(ddlLieferant.Items.Count - 1).Selected = True
                        .SelLief = ddlLieferant.SelectedIndex
                    End If
                    i += 1
                Loop
                CreateDataDisplay()
                addButtonHandler()
                .getOffeneBestellungERP(ddlLieferant.SelectedValue)
                If Not .ErrorOccured Then
                    If Not .OffBestellungen Is Nothing Then
                        If .OffBestellungen.Rows.Count > 0 Then
                            Dim dRow() As DataRow = .OffBestellungen.Select("BEDAT='" & Now.ToShortDateString & "' AND LIFNR= '" & ddlLieferant.SelectedValue & "'")

                            If dRow.Length > 0 Then
                                .vorhandeneBest = True
                                lblMessage.Text = "ACHTUNG! Bestellung(en) bereits unter gleichem Datum vorhanden"
                            Else
                                .vorhandeneBest = False
                            End If
                        End If
                    End If
                Else
                    lblError.Text = .ErrorMessage
                End If
                If ddlLieferant.SelectedValue = "0000900030" Then ' Bei Utsch max. 7 Stellen
                    txtLieferscheinnummer.MaxLength = 7
                Else
                    txtLieferscheinnummer.MaxLength = 10
                End If
            End With

        Else
            lblError.Text = mObjPlatinen.ErrorMessage '"Es konnten keine Lieferanten geladen werden!"
        End If
    End Sub

    Private Sub CreateDataDisplay()
        lblError.Text = ""
        mObjPlatinen.getArtikelReiterERP(ddlLieferant.SelectedValue)
        If Not mObjPlatinen.ErrorOccured Then

            tabContainer.Visible = True
            FillGridTopSeller(mObjPlatinen.SelReiter, "SortPos")
            FillGrid(mObjPlatinen.SelReiter, "ARTBEZ")
        Else
            GridView1.Visible = False
            GridView3.Visible = False
            tabContainer.Visible = False
            lblError.Text = "Es konnten keine Artikel zum ausgewählten Lieferanten geladen werden!"
        End If
    End Sub

    Private Sub FillGridTopSeller(ByVal Reiter As String, Optional ByVal strSort As String = "")

        Dim tmpDataView As New DataView(mObjPlatinen.Artikel)
        If Reiter.Length > 0 Then
            tmpDataView.RowFilter = "REITER='" & Reiter & "' AND TOPSELLER='X'"
        End If

        If tmpDataView.Count = 0 Then
            GridView1.Visible = False
            lblNoData.Visible = True
        Else
            GridView1.Visible = True
            lblNoData.Visible = False

            If Not strSort.Length = 0 Then
                tmpDataView.Sort = strSort & " asc"
            End If

            GridView1.DataSource = tmpDataView

            GridView1.DataBind()
        End If
    End Sub

    Private Sub FillGrid(ByVal Reiter As String, Optional ByVal strSort As String = "")

        Dim tmpDataView As New DataView(mObjPlatinen.Artikel)
        If Reiter.Length > 0 Then
            tmpDataView.RowFilter = "REITER='" & Reiter & "' AND TOPSELLER <> 'X'"
        End If
        If tmpDataView.Count = 0 Then
            GridView3.Visible = False
            lblNoData.Visible = True
        Else
            GridView3.Visible = True
            lblNoData.Visible = False

            If Not strSort.Length = 0 Then
                tmpDataView.Sort = strSort & " asc"
            End If

            GridView3.DataSource = tmpDataView

            GridView3.DataBind()
        End If
    End Sub

    Private Sub FillGridBestell()

        Dim tmpDataView As New DataView(mObjPlatinen.Artikel)
        tmpDataView.RowFilter = "Menge > 0"
        If tmpDataView.Count = 0 Then
            GridView2.Visible = False
            lblNoData.Visible = True
        Else
            GridView2.Visible = True
            lblNoData.Visible = False

            GridView2.DataSource = tmpDataView
            GridView2.DataBind()

            For Each row As GridViewRow In GridView2.Rows
                If CInt(mObjPlatinen.Artikel.Select("ARTLIF='" & CType(row.FindControl("lblMatnr"), Label).Text & "'")(0)("Menge")) > 2000 Then
                    row.BackColor = Drawing.ColorTranslator.FromHtml("#D87D7D")
                    For Each cell As TableCell In row.Cells
                        cell.ForeColor = Drawing.ColorTranslator.FromHtml("#ffffff")
                    Next

                    CType(row.FindControl("lblStatus"), Label).Text = "Bestellmenge über 2000!"
                    GridView2.Columns(GridView2.Columns.Count - 1).Visible = True
                End If
            Next
        End If

    End Sub

    Protected Sub ddlLieferant_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ddlLieferant.SelectedIndexChanged
        If Hidden1.Value = "1" Then
            MPE_ChangeLieferant.Show()
            Hidden1.Value = ""
        Else
            mObjPlatinen.SelLief = ddlLieferant.SelectedIndex
            CreateDataDisplay()
            addButtonHandler()
            lblMessage.Text = ""
            With mObjPlatinen
                .getOffeneBestellungERP(ddlLieferant.SelectedValue)
                If Not .ErrorOccured Then
                    If Not .OffBestellungen Is Nothing Then
                        If .OffBestellungen.Rows.Count > 0 Then
                            Dim dRow() As DataRow = .OffBestellungen.Select("BEDAT='" & Now.ToShortDateString & "' AND LIFNR= '" & ddlLieferant.SelectedValue & "'")

                            If dRow.Length > 0 Then
                                .vorhandeneBest = True
                                lblMessage.Text = "ACHTUNG! Bestellung(en) bereits unter gleichem Datum vorhanden"""
                            Else
                                .vorhandeneBest = False
                            End If
                        End If
                    End If
                Else
                    lblError.Text = .ErrorMessage
                End If
            End With
            Hidden1.Value = ""
        End If
        If ddlLieferant.SelectedValue = "0000900030" Then ' Bei Utsch max. 7 Stellen
            txtLieferscheinnummer.MaxLength = 7
        Else
            txtLieferscheinnummer.MaxLength = 10
        End If
    End Sub

    Sub LinkButton1_Click(ByVal sender As Object, ByVal e As CommandEventArgs)
        If CheckGrids() Then
            Dim ctrl As Control
            For Each ctrl In tabContainer.Controls
                If TypeOf ctrl Is LinkButton Then
                    Dim lnkBut As LinkButton
                    lnkBut = CType(ctrl, LinkButton)
                    lnkBut.CssClass = "TabButton"
                End If
            Next
            ctrl = tabContainer.FindControl(e.CommandArgument.ToString)
            If TypeOf ctrl Is LinkButton Then
                Dim lnkBut As LinkButton
                lnkBut = CType(ctrl, LinkButton)
                lnkBut.CssClass = "TabButtonActive"
            End If
            mObjPlatinen.SelReiter = Left(e.CommandArgument.ToString, 1)
            FillGridTopSeller(mObjPlatinen.SelReiter, "SortPos")
            FillGrid(mObjPlatinen.SelReiter, "ARTBEZ")
        End If

    End Sub

    Protected Sub txtMenge_TextChanged(ByVal sender As Object, ByVal e As EventArgs)
        If IsNumeric(CType(sender, TextBox).Text) Then
            Dim dRow As GridViewRow = CType(CType(sender, TextBox).Parent.Parent, GridViewRow)
            Dim lblMatnr As Label = CType(dRow.FindControl("lblMatnr"), Label)
            If CType(sender, TextBox).Text = "0" Then
                mObjPlatinen.Artikel.Select("ARTLIF='" & lblMatnr.Text & "'")(0)("Menge") = DBNull.Value
                mObjPlatinen.Artikel.AcceptChanges()
            Else
                ' Menge festlegen
                mObjPlatinen.Artikel.Select("ARTLIF='" & lblMatnr.Text & "'")(0)("Menge") = CInt(CType(sender, TextBox).Text)
                mObjPlatinen.Artikel.AcceptChanges()

                Dim ArtikelRow As DataRow = mObjPlatinen.Artikel.Select("ARTLIF='" & lblMatnr.Text & "'")(0)
                If ArtikelRow("ZUSINFO").ToString.ToUpper = "X" Then
                    If CType(dRow.FindControl("txtBeschreibung"), TextBox).Text.Trim = String.Empty AndAlso CType(dRow.FindControl("txtMenge"), TextBox).Text.Trim <> String.Empty Then
                        dRow.BackColor = Drawing.ColorTranslator.FromHtml("#D87D7D")
                        lblError.Text = "Beschreibung ist ein Pflichfeld! Bitte füllen!"
                        CType(dRow.FindControl("txtBeschreibung"), TextBox).Focus()
                    Else
                        ' Wenn Beschreibung gültig dann Änderung speichern
                        ArtikelRow("Beschreibung") = CType(dRow.FindControl("txtBeschreibung"), TextBox).Text
                        mObjPlatinen.Artikel.AcceptChanges()
                    End If

                End If
                If ArtikelRow("UMREZ").ToString <> "1" Then
                    If IsNumeric(CType(sender, TextBox).Text) Then
                        Dim iStueck As Decimal = CDec(CType(sender, TextBox).Text)
                        Dim iUmrez As Decimal = CDec(ArtikelRow("UMREZ").ToString)
                        Dim iErgebnis As Decimal = iStueck / iUmrez

                        If iErgebnis.ToString.Contains(",") Then
                            dRow.BackColor = Drawing.ColorTranslator.FromHtml("#D87D7D")
                            lblError.Text = "Bestellung so nicht möglich. Bitte Verpackungseinheit beachten!"
                        End If
                    End If
                End If
            End If
        ElseIf CType(sender, TextBox).Text = String.Empty Then
            Dim dRow As GridViewRow = CType(CType(sender, TextBox).Parent.Parent, GridViewRow)
            Dim lblMatnr As Label = CType(dRow.FindControl("lblMatnr"), Label)
            mObjPlatinen.Artikel.Select("ARTLIF='" & lblMatnr.Text & "'")(0)("Menge") = DBNull.Value
            mObjPlatinen.Artikel.AcceptChanges()
        End If
    End Sub

    Private Sub lbtnOK_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lbtnOK.Click
        CreateDataDisplay()
        mObjPlatinen.SelLief = ddlLieferant.SelectedIndex
    End Sub

    Private Sub lbtnCancel_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lbtnCancel.Click
        ddlLieferant.SelectedIndex = mObjPlatinen.SelLief
    End Sub

    Private Function CheckGrids() As Boolean
        Dim bAllOk = False

        If checkGrid(GridView1) AndAlso checkGrid(GridView3) Then
            bAllOk = True
        End If

        Return bAllOk
    End Function

    Private Function checkGrid(GridV As GridView) As Boolean
        Dim bValid As Boolean = True

        For Each row As GridViewRow In GridV.Rows
            Dim ArtikelRow As DataRow = mObjPlatinen.Artikel.Select("ARTLIF='" & CType(row.FindControl("lblMatnr"), Label).Text & "'")(0)
            If ArtikelRow("ZUSINFO").ToString.ToUpper = "X" Then
                If CType(row.FindControl("txtBeschreibung"), TextBox).Text.Trim = String.Empty AndAlso CType(row.FindControl("txtMenge"), TextBox).Text.Trim <> String.Empty Then
                    row.BackColor = Drawing.ColorTranslator.FromHtml("#D87D7D")
                    lblError.Text = "Beschreibung ist ein Pflichfeld! Bitte füllen!"
                    bValid = False
                    CType(row.FindControl("txtBeschreibung"), TextBox).Focus()
                Else
                    ArtikelRow("Beschreibung") = CType(row.FindControl("txtBeschreibung"), TextBox).Text
                    mObjPlatinen.Artikel.AcceptChanges()
                End If
            End If
            If ArtikelRow("UMREZ").ToString <> "1" Then
                If IsNumeric(CType(row.FindControl("txtMenge"), TextBox).Text) Then
                    Dim iStueck As Decimal
                    Dim iUmrez As Decimal
                    Dim iErgebnis As Decimal
                    iStueck = CDec(CType(row.FindControl("txtMenge"), TextBox).Text)
                    iUmrez = CDec(ArtikelRow("UMREZ").ToString)
                    iErgebnis = iStueck / iUmrez
                    If iErgebnis.ToString.Contains(",") Then
                        row.BackColor = Drawing.ColorTranslator.FromHtml("#D87D7D")
                        lblError.Text = "Bestellung so nicht möglich. Bitte Verpackungseinheit beachten!"
                        bValid = False
                    End If
                End If
            End If
        Next

        Return bValid
    End Function

    Protected Sub lbAbsenden_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lbAbsenden.Click

        If mObjPlatinen.Artikel.Select("Menge > 0").Length > 0 Then
            If CheckGrids() And ProofLieferschein() Then
                FillGridBestell()
                mpeBestellungsCheck.Show()
                WorkaroundSetFocus()
            End If
        Else
            lblError.Text = "Es wurde für keinen Artikel ein Bestellmenge erfasst."
        End If

    End Sub

    Private Function ProofLieferschein() As Boolean
        Dim blProofed As Boolean = False

        If chkGeliefert.Checked Then
            If txtLieferscheinnummer.Text.Trim().Length > 0 Then
                blProofed = True
            End If
        Else
            blProofed = True
        End If

        If blProofed = False Then
            lblError.Text = "Geben Sie eine Lieferscheinnummer ein!"
        End If

        Return blProofed
    End Function

    Private Sub WorkaroundSetFocus()
        If (Not ClientScript.IsStartupScriptRegistered("Startup")) Then
            Dim sb As StringBuilder = New StringBuilder()
            sb.Append("<script type=""text/javascript"">")
            sb.Append("Sys.Application.add_load(modalSetup);")
            sb.Append("function modalSetup() {")
            sb.Append(String.Format("var modalPopup = $find('{0}');", mpeBestellungsCheck.BehaviorID))
            sb.Append("modalPopup.add_shown(SetFocusOnControl); }")
            sb.Append("function SetFocusOnControl(sender, e) {")
            sb.Append(String.Format("var textBox1 = $get('{0}');", txtBedienerkarte.ClientID))
            sb.Append("if (textBox1 != null){textBox1.focus();}}")
            sb.Append("</script>")
            Page.ClientScript.RegisterStartupScript(Page.GetType(), "Startup", sb.ToString())
        End If
    End Sub

    Private Sub lbBestellungOk_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lbBestellungOk.Click
        If CheckBedienerKarte() Then
            doSubmit()
        Else
            FillGridBestell()
            mpeBestellungsCheck.Show()
        End If
    End Sub

    Private Function CheckBedienerKarte() As Boolean

        If txtBedienerkarte.Text = String.Empty Then
            lblBedienError.Text = "Bitte lesen Sie die Bedienerkarte ein!"
            Return False
        ElseIf txtBedienerkarte.Text.Length <> 15 Then
            lblBedienError.Text = "Fehler beim einlesen der Bedienerkarte. Barcode hat die falsche Länge!"
            Return False
        Else
            Try
                Dim strCode As String
                Dim strBediener As String
                strCode = Left(txtBedienerkarte.Text, 14)
                strCode = Right(strCode, 13)
                strBediener = strCode.Substring(3, 1)
                strBediener &= strCode.Substring(6, 1)
                strBediener &= strCode.Substring(8, 1)
                strBediener &= strCode.Substring(11, 1)
                mObjPlatinen.BedienerNr = strBediener
                Return True
            Catch ex As Exception
                lblBedienError.Text = "Fehler beim einlesen der Bedienerkarte. Versuchen Sie es nochmal!"
                Return False

            End Try

        End If

    End Function

    Private Sub doSubmit()
        '----------------------------------------------------------------------
        'Methode:      doSubmit
        'Autor:         ORu
        'Beschreibung:  je nach SAP status wird hier eine Meldung ausgegeben, im Fehlerfalle bleibt das OBJ bestehen, sonst kill
        'Erstellt am:   11.02.2010
        '----------------------------------------------------------------------

        If CType(Session("SendToSap"), Boolean) = False Then
            If chkGeliefert.Checked Then
                mObjPlatinen.Geliefert = "X"
                mObjPlatinen.Lieferscheinnummer = txtLieferscheinnummer.Text
            Else
                mObjPlatinen.Geliefert = ""
                mObjPlatinen.Lieferscheinnummer = ""
            End If

            If trLiefertermin.Visible Then
                If txtLieferdatum.Text <> String.Empty Then
                    Date.TryParse(txtLieferdatum.Text, mObjPlatinen.Lieferdatum)
                Else
                    Date.TryParse("01.01.1900", mObjPlatinen.Lieferdatum)
                End If
            Else
                Date.TryParse("01.01.1900", mObjPlatinen.Lieferdatum)
            End If
            mObjPlatinen.SendToKost = txtKST.Text
            mObjPlatinen.sendOrderToSAPERP(ddlLieferant.SelectedValue, mObjKasse.Master)
            Session("Commited") = mObjPlatinen.Commited
            Session("SendToSap") = True
        End If

        If Session("Commited") IsNot Nothing Then
            If CType(Session("Commited"), Boolean) = True Then

                lblBestellMeldung.ForeColor = Drawing.Color.Green
                lblBestellMeldung.Text = "Ihre Bestellung war erfolgreich!"

                mObjPlatinen = Nothing

            Else
                lblBestellMeldung.ForeColor = Drawing.Color.Red
                lblBestellMeldung.Text = "Ihre Bestellung ist fehlgeschlagen: <br><br> " & mObjPlatinen.ErrorMessage

            End If
        Else
            lblBestellMeldung.ForeColor = Drawing.Color.Red
            lblBestellMeldung.Text = "Ihre Bestellung ist fehlgeschlagen."
        End If

        lbBestellFinalize.Visible = True
        lblBestellMeldung.Visible = True
        lblStatus.Visible = True
        lbBestellungOk.Visible = False
        trGridview.Visible = False
        txtBedienerkarte.Visible = False

        lblBedienError.Visible = False
        trInfo.Visible = False
        lbBestellungKorrektur.Visible = False
        mpeBestellungsCheck.Show()
    End Sub

    Private Sub responseBack()
        Response.Redirect("../Selection.aspx")
    End Sub

    Protected Sub lb_zurueck_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lb_zurueck.Click
        responseBack()
    End Sub

    Protected Sub lbBestellFinalize_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lbBestellFinalize.Click
        Response.Redirect("../Selection.aspx")
    End Sub

    Private Sub lbBestellungKorrektur_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lbBestellungKorrektur.Click
        mpeBestellungsCheck.Hide()
    End Sub

    Protected Function proofFileExist(ByVal Path As String) As Boolean
        Dim bReturn As Boolean = False
        If File.Exists("C:/inetpub/wwwroot/" & Path) Then
            bReturn = True
        End If
        Return bReturn
    End Function

    Public Sub txtBedienerkarte_TextChanged()
        If CheckBedienerKarte() Then
            doSubmit()
        Else
            mpeBestellungsCheck.Show()
        End If
    End Sub

    Protected Sub chkGeliefert_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs) Handles chkGeliefert.CheckedChanged
        If chkGeliefert.Checked Then
            trLieferscheinnummer.Visible = True
        Else
            trLieferscheinnummer.Visible = False
        End If
    End Sub

    Protected Sub ibtnOptionalShow_Click(sender As Object, e As ImageClickEventArgs) Handles ibtnOptionalShow.Click
        trLiefertermin.Visible = Not trLiefertermin.Visible
    End Sub

    Protected Sub txtKST_TextChanged(sender As Object, e As EventArgs) Handles txtKST.TextChanged
        If mObjKasse.Master Then
            If txtKST.Text.Length > 0 Then
                With mObjPlatinen
                    .CheckKostStelleERP(txtKST.Text.Trim)
                    chkGeliefert.Checked = False
                    txtLieferscheinnummer.Text = ""
                    txtLieferdatum.Text = ""
                    If .ErrorOccured Then
                        lblError.Text = .ErrorMessage
                        SetFocus(txtKST)
                        lblKSTText.Visible = False
                        lblKSTText.Text = ""
                        ddlLieferant.Enabled = False
                        chkGeliefert.Enabled = False
                        txtLieferscheinnummer.Enabled = False
                        ibtnOptionalShow.Enabled = False
                        GridView1.Enabled = False
                        GridView3.Enabled = False
                        For Each ctrl As Control In tabContainer.Controls
                            If TypeOf ctrl Is LinkButton Then
                                Dim lnkBut As LinkButton
                                lnkBut = CType(ctrl, LinkButton)
                                lnkBut.Enabled = False
                            End If
                        Next
                        lbAbsenden.Enabled = False
                    Else
                        lblKSTText.Visible = True
                        lblKSTText.Text = .KostText
                        fillDropdown()
                        ddlLieferant.Enabled = True
                        chkGeliefert.Enabled = True
                        txtLieferscheinnummer.Enabled = True
                        ibtnOptionalShow.Enabled = True
                        GridView1.Enabled = True
                        GridView3.Enabled = True
                        For Each ctrl As Control In tabContainer.Controls
                            If TypeOf ctrl Is LinkButton Then
                                Dim lnkBut As LinkButton
                                lnkBut = CType(ctrl, LinkButton)
                                lnkBut.Enabled = True
                            End If
                        Next
                        SetFocus(ddlLieferant)
                        lbAbsenden.Enabled = True
                    End If

                End With
            End If
        Else
            SetFocus(txtKST)
            lblKSTText.Visible = False
            lblKSTText.Text = ""
            ddlLieferant.Enabled = False
            chkGeliefert.Enabled = False
            txtLieferscheinnummer.Enabled = False
            ibtnOptionalShow.Enabled = False
            GridView1.Enabled = False
            GridView3.Enabled = False
            For Each ctrl As Control In tabContainer.Controls
                If TypeOf ctrl Is LinkButton Then
                    Dim lnkBut As LinkButton
                    lnkBut = CType(ctrl, LinkButton)
                    lnkBut.Enabled = False
                End If
            Next
            lbAbsenden.Enabled = False
        End If
    End Sub

End Class