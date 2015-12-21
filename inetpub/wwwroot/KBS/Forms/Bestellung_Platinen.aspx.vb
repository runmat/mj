Imports KBS.KBS_BASE
Imports System.IO

Public Class Bestellung_Platinen
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
            If Session("mPlatinen") IsNot Nothing Then
                mObjPlatinen = CType(Session("mPlatinen"), Platinen)
            Else
                mObjPlatinen = mObjKasse.Platinen(Me)
                Session("mPlatinen") = mObjPlatinen
            End If
        End If

        If Not IsPostBack Then

            mObjPlatinen.SelReiter = "1"
            mObjPlatinen.KostStelle = mObjKasse.Lagerort
            Session("mPlatinen") = mObjPlatinen

            'Wurde schon an SAP übermittelt?
            Session.Add("SendToSap", False)
            Session("Commited") = Nothing
            If mObjKasse.Master Then
                txtKST.Enabled = True
                ddlLieferant.Enabled = False
                chkGeliefert.Enabled = False
                txtLieferscheinnummer.Enabled = False
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
                ApplyKst()
            End If
        Else
            If CType(Session("Commited"), Boolean) = True Then
                Session("SendToSap") = False
                Session("Commited") = Nothing
                FillGridTopSeller(mObjPlatinen.SelReiter, "SortPos")
                FillGrid(mObjPlatinen.SelReiter)
            End If

            addButtonHandler()

            Dim eventTar As String = Request("__EVENTTARGET")
            If eventTar = txtBedienerkarte.UniqueID OrElse eventTar = "ctl00_ContentPlaceHolder1_lbBestellungOk" Then
                Dim eventArg As String = Request("__EVENTARGUMENT")
                txtBedienerkarte_TextChanged(eventArg)
            End If

            If eventTar = "ChangeLiefOk" Then
                Dim eventArg As Integer = CInt(Request("__EVENTARGUMENT"))
                lbtnOK_Click(eventArg)
            End If

        End If
    End Sub

    ''' <summary>
    ''' Eventhandler an TabButtons binden
    ''' </summary>
    ''' <remarks></remarks>
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

    ''' <summary>
    ''' Dropdown der Lieferanten füllen
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub fillDropdown()
        mObjPlatinen.getLieferantenERP(mObjKasse.Master, mObjKasse.Firma)

        If Not mObjPlatinen.ErrorOccured Then

            ddlLieferant.DataSource = mObjPlatinen.Lieferanten
            ddlLieferant.DataValueField = "LIFNR"
            ddlLieferant.DataTextField = "NAME1"
            ddlLieferant.DataBind()

            Dim hauptLiefRows() As DataRow = mObjPlatinen.Lieferanten.Select("HAUPT = 'X'")
            If hauptLiefRows.Length > 0 Then
                ddlLieferant.SelectedValue = hauptLiefRows(0)("LIFNR")
            End If

            ApplyLieferantennr()

        Else
            lblError.Text = mObjPlatinen.ErrorMessage '"Es konnten keine Lieferanten geladen werden!"
        End If

        Session("mPlatinen") = mObjPlatinen
    End Sub

    ''' <summary>
    ''' Daten für Grids holen und anzeigen
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub CreateDataDisplay()
        lblError.Text = ""
        mObjPlatinen.getArtikelReiterERP()
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

        Session("mPlatinen") = mObjPlatinen
    End Sub

    ''' <summary>
    ''' Topseller Grid füllen
    ''' </summary>
    ''' <param name="Reiter">gewählter Reiter</param>
    ''' <param name="strSort">Sortierung</param>
    ''' <remarks></remarks>
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
        End If

        GridView1.DataSource = tmpDataView
        GridView1.DataBind()
    End Sub

    ''' <summary>
    ''' Hauptgrid füllen
    ''' </summary>
    ''' <param name="Reiter">gewählter Reiter</param>
    ''' <param name="strSort">Sortierung</param>
    ''' <remarks></remarks>
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
        End If

        GridView3.DataSource = tmpDataView
        GridView3.DataBind()
    End Sub

    ''' <summary>
    ''' Gridview nach Absenden im PopUp füllen
    ''' </summary>
    ''' <remarks></remarks>
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
        ApplyLieferantennr()
    End Sub

    ''' <summary>
    ''' Hilfsmethode die den Tabbuttons zugeordnet wird
    ''' </summary>
    ''' <param name="sender">Sender</param>
    ''' <param name="e">EventArguments</param>
    ''' <remarks></remarks>
    Sub LinkButton1_Click(ByVal sender As Object, ByVal e As CommandEventArgs)
        collectGridChanges()

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
        Session("mPlatinen") = mObjPlatinen
        FillGridTopSeller(mObjPlatinen.SelReiter, "SortPos")
        FillGrid(mObjPlatinen.SelReiter, "ARTBEZ")

    End Sub

    Private Sub lbtnOK_Click(ByVal lifnr As String)
        CreateDataDisplay()
        Session("mPlatinen") = mObjPlatinen
        ChangeLiefHidden.Value = 1
    End Sub

    ''' <summary>
    ''' Überprüft mehrere Grids auf Gültigkeit
    ''' </summary>
    ''' <returns><c>True</c> wenn Gridviews alle gültig</returns>
    ''' <remarks></remarks>
    Private Function CheckGrids() As Boolean
        Return (checkGrid(GridView1) AndAlso checkGrid(GridView3))
    End Function

    ''' <summary>
    ''' Überprüft ein Grid auf Gültigkeit
    ''' </summary>
    ''' <param name="GridV">Das zu prüfende GridView</param>
    ''' <returns><c>True</c> wenn Gridview gültig</returns>
    ''' <remarks></remarks>
    Private Function checkGrid(GridV As GridView) As Boolean
        Dim bValid As Boolean = True

        For Each row As GridViewRow In GridV.Rows
            Dim ArtikelRow As DataRow = mObjPlatinen.Artikel.Select("ARTLIF='" & CType(row.FindControl("lblMatnr"), Label).Text & "'")(0)
            If ArtikelRow("ZUSINFO").ToString.ToUpper = "X" Then
                If String.IsNullOrEmpty(CType(row.FindControl("txtBeschreibung"), TextBox).Text) AndAlso Not String.IsNullOrEmpty(CType(row.FindControl("txtMenge"), TextBox).Text) Then
                    row.BackColor = Drawing.ColorTranslator.FromHtml("#D87D7D")
                    lblError.Text = "Beschreibung ist ein Pflichtfeld! Bitte füllen!"
                    bValid = False
                    CType(row.FindControl("txtBeschreibung"), TextBox).Focus()
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

    ''' <summary>
    ''' Übernimmt die Datenänderungen mehrerer Grids in die Datencollection
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub collectGridChanges()
        collectGridChanges(GridView1)
        collectGridChanges(GridView3)
    End Sub

    ''' <summary>
    ''' Übernimmt die Datenänderungen eines Grids in die Datencollection
    ''' </summary>
    ''' <param name="GridV">Das zu verarbeitende GridView</param>
    ''' <remarks></remarks>
    Private Sub collectGridChanges(GridV As GridView)
        Dim lblMatnr As Label
        Dim txtMenge As TextBox
        Dim txtBeschreibung As TextBox

        For Each row As GridViewRow In GridV.Rows
            lblMatnr = CType(row.FindControl("lblMatnr"), Label)
            txtMenge = CType(row.FindControl("txtMenge"), TextBox)
            txtBeschreibung = CType(row.FindControl("txtBeschreibung"), TextBox)

            Dim ArtikelRow As DataRow = mObjPlatinen.Artikel.Select("ARTLIF='" & lblMatnr.Text & "'")(0)
            If String.IsNullOrEmpty(txtMenge.Text) OrElse txtMenge.Text = "0" OrElse Not IsNumeric(txtMenge.Text) Then
                ArtikelRow("Menge") = DBNull.Value
            Else
                ArtikelRow("Menge") = Integer.Parse(txtMenge.Text)
            End If
            If String.IsNullOrEmpty(txtBeschreibung.Text) Then
                ArtikelRow("Beschreibung") = DBNull.Value
            Else
                ArtikelRow("Beschreibung") = txtBeschreibung.Text
            End If
        Next

        mObjPlatinen.Artikel.AcceptChanges()
        Session("mPlatinen") = mObjPlatinen
    End Sub

    Protected Sub lbAbsenden_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lbAbsenden.Click

        collectGridChanges()

        If mObjPlatinen.Artikel.Select("Menge > 0").Length = 0 Then
            lblError.Text = "Es wurde für keinen Artikel eine Bestellmenge erfasst."
        Else
            Dim dt As DataTable = mObjPlatinen.GetListeAusparkenERP()
            Dim dRowParken() As DataRow
            If Not String.IsNullOrEmpty(mObjPlatinen.BestellnummerParken) Then
                dRowParken = dt.Select("LIFNR = '" & ddlLieferant.SelectedValue & "' AND BSTNR <> '" & mObjPlatinen.BestellnummerParken & "'")
            Else
                dRowParken = dt.Select("LIFNR = '" & ddlLieferant.SelectedValue & "'")
            End If

            If dRowParken.Length > 0 Then
                lblError.Text = "Achtung! Sie haben schon eine Bestellung für Lieferant " & ddlLieferant.SelectedValue & " geparkt. Bitte diese erst ausparken."
                Exit Sub
            End If

            If CheckGrids() AndAlso ProofLieferschein() Then
                FillGridBestell()
                BestellCheckHidden.Value = 0
            End If
        End If

    End Sub

    ''' <summary>
    ''' Überprüft die eingegebene Lieferscheinnummer, falls vorhanden
    ''' </summary>
    ''' <returns><c>true</c> wenn Lieferscheinnummer korrekt</returns>
    ''' <remarks></remarks>
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

    ''' <summary>
    ''' Überprüft den mitgegebenen Kartennummern-String auf seine Gültigkeit
    ''' </summary>
    ''' <param name="Kartennummer">unformartierter Kartennummern String</param>
    ''' <returns><c>True</c> wenn gültig</returns>
    ''' <remarks></remarks>
    Private Function CheckBedienerKarte(ByVal Kartennummer As String) As Boolean

        If Kartennummer = String.Empty Then
            lblBedienError.Text = "Bitte lesen Sie die Bedienerkarte ein!"
            Return False
        ElseIf Kartennummer.Length <> 15 Then
            lblBedienError.Text = "Fehler beim einlesen der Bedienerkarte. Barcode hat die falsche Länge!"
            Return False
        Else
            Try
                Dim strCode As String
                Dim strBediener As String
                strCode = Left(Kartennummer, 14)
                strCode = Right(strCode, 13)
                strBediener = strCode.Substring(3, 1)
                strBediener &= strCode.Substring(6, 1)
                strBediener &= strCode.Substring(8, 1)
                strBediener &= strCode.Substring(11, 1)
                mObjPlatinen.BedienerNr = strBediener
                Session("mPlatinen") = mObjPlatinen
                Return True
            Catch ex As Exception
                lblBedienError.Text = "Fehler beim einlesen der Bedienerkarte. Versuchen Sie es nochmal!"
                Return False
            End Try
        End If
    End Function

    ''' <summary>
    ''' Die Methode übergibt die aktuellen Daten an SAP
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub doSubmit()
        Try

            If CType(Session("SendToSap"), Boolean) = False AndAlso Not ihIsSaving.Value = "1" Then
                ihIsSaving.Value = "1"

                CollectFormData()

                mObjPlatinen.sendOrderToSAPERP()
                Session("Commited") = mObjPlatinen.Commited
                Session("SendToSap") = True

                If Session("Commited") IsNot Nothing Then
                    If CType(Session("Commited"), Boolean) = True Then
                        lblMeldung.ForeColor = Drawing.Color.Green
                        lblMeldung.Text = "Ihre Bestellung war erfolgreich!"
                        MessageHidden.Value = 0
                        BestellCheckHidden.Value = 1

                        mObjPlatinen = mObjKasse.Platinen(Me)
                        mObjPlatinen.SelReiter = "1"
                        chkGeliefert.Checked = False
                        txtLieferdatum.Text = String.Empty

                        fillDropdown()
                    Else
                        lblBestellMeldung.ForeColor = Drawing.Color.Red
                        lblBestellMeldung.Text = "Ihre Bestellung ist fehlgeschlagen: <br><br> " & mObjPlatinen.ErrorMessage

                        MessageHidden.Value = 1
                        BestellCheckHidden.Value = 0
                    End If
                Else
                    lblBestellMeldung.ForeColor = Drawing.Color.Red
                    lblBestellMeldung.Text = "Ihre Bestellung ist fehlgeschlagen."

                    MessageHidden.Value = 1
                    BestellCheckHidden.Value = 0
                End If
                Session("mPlatinen") = mObjPlatinen
                ihIsSaving.Value = "0"
            End If

        Catch ex As Exception
            lblError.Text = "Beim Absenden der Bestellung ist ein unbekannter Fehler aufgetreten!"
            EventLog.WriteEntry("Bestellung_Platinen.DoSubmit()", ex.Message)
            ihIsSaving.Value = "0"
        End Try
    End Sub

    Private Sub CollectFormData()
        With mObjPlatinen
            .Lieferantennr = ddlLieferant.SelectedValue

            If chkGeliefert.Checked Then
                .Geliefert = "X"
                .Lieferscheinnummer = txtLieferscheinnummer.Text
            Else
                .Geliefert = ""
                .Lieferscheinnummer = ""
            End If

            Dim tmpDate As DateTime
            If trLiefertermin.Visible AndAlso Not String.IsNullOrEmpty(txtLieferdatum.Text) AndAlso DateTime.TryParse(txtLieferdatum.Text, tmpDate) Then
                .Lieferdatum = tmpDate
            Else
                .Lieferdatum = Nothing
            End If
        End With

        Session("mPlatinen") = mObjPlatinen
    End Sub

    Protected Sub lb_zurueck_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lb_zurueck.Click
        Response.Redirect("../Selection.aspx")
    End Sub
   
    Protected Function proofFileExist(ByVal Path As String) As Boolean
        Dim bReturn As Boolean = False
        If File.Exists("C:/inetpub/wwwroot/" & Path) Then
            bReturn = True
        End If
        Return bReturn
    End Function

    Public Sub txtBedienerkarte_TextChanged(ByVal Kassierernummer As String)
        If CheckBedienerKarte(Kassierernummer) Then
            doSubmit()
        Else
            BestellCheckHidden.Value = 0
        End If
    End Sub

    Protected Sub chkGeliefert_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs) Handles chkGeliefert.CheckedChanged

    End Sub

    Protected Sub txtKST_TextChanged(sender As Object, e As EventArgs) Handles txtKST.TextChanged
        If mObjKasse.Master Then
            If Not String.IsNullOrEmpty(txtKST.Text) Then
                ApplyKst(True)
            End If
        Else
            SetFocus(txtKST)
            lblKSTText.Visible = False
            lblKSTText.Text = ""
            ddlLieferant.Enabled = False
            chkGeliefert.Enabled = False
            txtLieferscheinnummer.Enabled = False
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

    Private Sub lbAusparken_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lbAusparken.Click
        MPEAusparken.Show()
    End Sub

    Private Sub lbParken_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lbParken.Click

        collectGridChanges()

        If mObjPlatinen.Artikel.Select("Menge > 0").Length = 0 Then
            lblError.Text = "Es wurde für keinen Artikel eine Bestellmenge erfasst."
            Exit Sub
        End If

        Dim dt As DataTable = mObjPlatinen.GetListeAusparkenERP()
        Dim dRowParken() As DataRow
        If Not String.IsNullOrEmpty(mObjPlatinen.BestellnummerParken) Then
            dRowParken = dt.Select("LIFNR = '" & ddlLieferant.SelectedValue & "' AND BSTNR <> '" & mObjPlatinen.BestellnummerParken & "'")
        Else
            dRowParken = dt.Select("LIFNR = '" & ddlLieferant.SelectedValue & "'")
        End If

        If dRowParken.Length > 0 Then
            lblError.Text = "Achtung! Sie haben schon eine Bestellung für Lieferant " & ddlLieferant.SelectedValue & " geparkt. Bitte diese erst ausparken."
            Exit Sub
        End If

        CollectFormData()

        mObjPlatinen.ParkenERP(ddlLieferant.SelectedValue)
        If mObjPlatinen.ErrorOccured Then
            lblError.Text = mObjPlatinen.ErrorMessage
        Else
            chkGeliefert.Checked = False
            txtLieferscheinnummer.Text = ""
            txtLieferdatum.Text = ""
        End If
        Session("mPlatinen") = mObjPlatinen
        ChangeLiefHidden.Value = 1
        Hidden1.Value = ""
        FillGridTopSeller(mObjPlatinen.SelReiter, "SortPos")
        FillGrid(mObjPlatinen.SelReiter)

    End Sub

    Private Sub lbAusparkenClose_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lbAusparkenClose.Click
        MPEAusparken.Hide()
        FillGridTopSeller(mObjPlatinen.SelReiter, "SortPos")
        FillGrid(mObjPlatinen.SelReiter)
    End Sub

    Protected Sub gvAusparken_RowCommand(ByVal sender As Object, ByVal e As GridViewCommandEventArgs) Handles gvAusparken.RowCommand
        Select Case e.CommandName

            Case "ausparken"
                With mObjPlatinen
                    .AusparkenERP(e.CommandArgument)
                    If mObjPlatinen.ErrorOccured Then
                        lblError.Text = mObjPlatinen.ErrorMessage
                    Else
                        If ddlLieferant.Items.FindByValue(.Lieferantennr) IsNot Nothing Then
                            ddlLieferant.SelectedValue = .Lieferantennr
                            ApplyLieferantennr()
                        End If
                        .GeparktePositionenUebernehmen()
                        chkGeliefert.Checked = (.Geliefert = "X")
                        If Not String.IsNullOrEmpty(.Lieferscheinnummer) Then
                            txtLieferscheinnummer.Text = .Lieferscheinnummer
                        Else
                            txtLieferscheinnummer.Text = ""
                        End If
                        If .Lieferdatum.HasValue Then
                            txtLieferdatum.Text = .Lieferdatum.Value.ToShortDateString()
                        Else
                            txtLieferdatum.Text = ""
                        End If
                    End If
                End With

            Case "löschen"
                mObjPlatinen.GeparktLoeschenERP(e.CommandArgument, "X")
                If mObjPlatinen.ErrorOccured Then
                    lblError.Text = mObjPlatinen.ErrorMessage
                End If
                MPEAusparken.Show()

        End Select
        Session("mPlatinen") = mObjPlatinen
        FillGridTopSeller(mObjPlatinen.SelReiter, "SortPos")
        FillGrid(mObjPlatinen.SelReiter)
    End Sub

    Private Sub Bestellung_Platinen_PreRender(ByVal sender As Object, ByVal e As EventArgs) Handles Me.PreRender
        Dim dt As DataTable = mObjPlatinen.GetListeAusparkenERP()
        If dt.Rows.Count > 0 Then
            lbAusparken.Visible = True
        Else
            lbAusparken.Visible = False
        End If
        gvAusparken.DataSource = dt
        gvAusparken.DataBind()
    End Sub

    Private Function ApplyKst(Optional ByVal skipIfNotChanged As Boolean = False) As Boolean
        With mObjPlatinen

            If skipIfNotChanged AndAlso .KostStelle = txtKST.Text.Trim Then
                'Kst nicht geändert
                Return True
            End If

            .CheckKostStelleERP(txtKST.Text.Trim)

            If .ErrorOccured Then
                lblError.Text = .ErrorMessage
                lblKSTText.Visible = False
                lblKSTText.Text = ""
                ddlLieferant.Enabled = False
                chkGeliefert.Enabled = False
                txtLieferscheinnummer.Enabled = False
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
                SetFocus(txtKST)
                Return False
            End If

            .KostStelle = txtKST.Text.Trim()
            lblKSTText.Visible = True
            lblKSTText.Text = .KostText
            fillDropdown()
            ddlLieferant.Enabled = True
            chkGeliefert.Enabled = True
            txtLieferscheinnummer.Enabled = True
            GridView1.Enabled = True
            GridView3.Enabled = True
            For Each ctrl As Control In tabContainer.Controls
                If TypeOf ctrl Is LinkButton Then
                    Dim lnkBut As LinkButton
                    lnkBut = CType(ctrl, LinkButton)
                    lnkBut.Enabled = True
                End If
            Next
            lbAbsenden.Enabled = True
            SetFocus(ddlLieferant)

        End With

        Session("mPlatinen") = mObjPlatinen

        Return True
    End Function

    Private Sub ApplyLieferantennr()
        mObjPlatinen.Lieferantennr = ddlLieferant.SelectedValue

        CreateDataDisplay()
        addButtonHandler()
        lblError.Text = ""

        With mObjPlatinen
            .getOffeneBestellungERP()
            If Not .ErrorOccured Then
                If Not .OffBestellungen Is Nothing Then
                    If .OffBestellungen.Rows.Count > 0 Then
                        Dim dRow() As DataRow = .OffBestellungen.Select("BEDAT='" & Now.ToShortDateString & "' AND LIFNR= '" & ddlLieferant.SelectedValue & "'")

                        If dRow.Length > 0 Then
                            .vorhandeneBest = True
                            lblError.Text = "ACHTUNG! Bestellung(en) bereits unter gleichem Datum vorhanden"""
                        Else
                            .vorhandeneBest = False
                        End If
                    End If
                End If
            Else
                lblError.Text = .ErrorMessage
            End If
        End With

        If ddlLieferant.SelectedValue = "0000900030" Then ' Bei Utsch max. 7 Stellen
            txtLieferscheinnummer.MaxLength = 7
        Else
            txtLieferscheinnummer.MaxLength = 10
        End If

        Session("mPlatinen") = mObjPlatinen
    End Sub

End Class