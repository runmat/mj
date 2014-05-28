Imports CKG.Base.Kernel.Common.Common
Imports CKG.Base.Kernel.Security
Imports CKG.Base.Kernel
'Imports dk.nita.saml20
'Imports dk.nita.saml20.Bindings
'Imports dk.nita.saml20.Utils

Partial Public Class Change02s
    Inherits System.Web.UI.Page

#Region "Declarations"
    Private m_App As App
    Private m_User As User
    Private mBeauftragung As Beauftragung

    Private Enum Source As Integer
        Part = 0
        Full = 1
    End Enum

#End Region

#Region "Events"


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load


        m_User = GetUser(Me)
        FormAuth(Me, m_User)
        GetAppIDFromQueryString(Me)
        m_App = New App(m_User)

        If Session("mBeauftragung") Is Nothing Then
            Session("mBeauftragung") = New Beauftragung(m_User, m_App, Session("AppID").ToString, Session.SessionID.ToString, "")
        End If
        mBeauftragung = CType(Session("mBeauftragung"), Beauftragung)

        If Not IsPostBack Then

            If Not Request.QueryString.Item("eIdentityResponse") Is Nothing Then
                'InitPage()
                SetControls()
                Exit Sub
            End If

            If Not Session("Me") Is Nothing Then
                'InitPage()
                Reset()

                InitJava()
                Exit Sub
            End If

            mBeauftragung.FillPrueforganisation(Me)
            ddlPrueforganisation.DataSource = mBeauftragung.Prueforganisationen
            ddlPrueforganisation.DataBind()

            mBeauftragung.FillArtGenehmigung(Me)
            ddlArtGenehmigung.DataSource = mBeauftragung.GenehmigungsArten
            ddlArtGenehmigung.DataBind()

        End If

        Try
            lblHead.Text = m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString

            m_App = New App(m_User)

            lblError.Text = ""


            If Not IsPostBack Then
                InitPage()

                If m_User.Reference.Length <> 8 Then

                    lblError.Text = "Kundenreferenz nicht korrekt."
                    Grunddaten.Disabled = True
                    ibtGrunddaten.Enabled = False
                    ibtFahrzeugdaten.Enabled = False
                    ibtDienstleistung.Enabled = False
                End If

            End If

            InitControls()
            PrepareDienstleistungControls()

        Catch ex As Exception
            lblError.Text = "Beim Laden der Seite ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
        End Try

    End Sub

    Private Sub ibtGrunddaten_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtGrunddaten.Click
        ibtGrunddaten.ImageUrl = "../../../Images/GrunddatenActive.jpg"
        ibtFahrzeugdaten.ImageUrl = "../../../Images/Fahrzeugdaten.jpg"
        ibtDienstleistung.ImageUrl = "../../../Images/Dienstleistung.jpg"

        Grunddaten.Visible = True
        Fahrzeugdaten.Visible = False
        Dienstleistung.Visible = False
        txtKunde.Focus()

    End Sub


    Private Sub ibtFahrzeugdaten_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtFahrzeugdaten.Click


        'User kommt aus Grunddaten(Grunddaten prüfen)
        If ibtGrunddaten.ImageUrl = "../../../Images/GrunddatenActive.jpg" Then

            If ValidateGrunddaten() = True Then
                Return
            End If

        End If


        ibtGrunddaten.ImageUrl = "../../../Images/Grunddaten.jpg"
        ibtFahrzeugdaten.ImageUrl = "../../../Images/FahrzeugdatenActive.jpg"
        ibtDienstleistung.ImageUrl = "../../../Images/Dienstleistung.jpg"

        Grunddaten.Visible = False
        Dienstleistung.Visible = False
        Fahrzeugdaten.Visible = True
        txtHersteller.Focus()


    End Sub


    Private Sub ibtDienstleistung_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtDienstleistung.Click

        InitDropdown()
        PrepareDienstleistungControls()

        'User kommt aus Grunddaten(Grunddaten prüfen)
        If ibtGrunddaten.ImageUrl = "../../../Images/GrunddatenActive.jpg" Then

            If ValidateGrunddaten() = True Then
                Return
            End If

        End If

        'User kommt aus Fahrzeugdaten
        If ValidateFahrzeugdaten(Source.Full) = True Then
            Return
        End If


        ibtGrunddaten.ImageUrl = "../../../Images/Grunddaten.jpg"
        ibtFahrzeugdaten.ImageUrl = "../../../Images/Fahrzeugdaten.jpg"
        ibtDienstleistung.ImageUrl = "../../../Images/DienstleistungActive.jpg"

        Grunddaten.Visible = False
        Fahrzeugdaten.Visible = False
        Dienstleistung.Visible = True

        'ddlDienstleistung.Focus()

        If Not Session("Dienstleistung") Is Nothing Then
            ddlDienstleistung.SelectedValue = Session("Dienstleistung")
        End If

    End Sub

    Private Sub btnDefault_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnDefault.Click

        If ibtGrunddaten.ImageUrl = "../../../Images/GrunddatenActive.jpg" Then
            txtKunde.Focus()
        ElseIf ibtFahrzeugdaten.ImageUrl = "../../../Images/FahrzeugdatenActive.jpg" Then
            txtHersteller.Focus()
        ElseIf ibtDienstleistung.ImageUrl = "../../../Images/DienstleistungActive.jpg" Then
            ddlDienstleistung.Focus()
        End If

    End Sub

    Private Sub rdbGrosskunde_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rdbGrosskunde.CheckedChanged
        If rdbGrosskunde.Checked = True Then
            ClearHalter()
            rdbHalter.Checked = False
            divHalter.Visible = False
            divNpa.Visible = False
            trGrossKunde.Visible = True
            txtGrosskundennummer.Focus()
        End If

    End Sub

    Private Sub rdbHalter_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rdbHalter.CheckedChanged
        If rdbHalter.Checked = True Then

            rdbGrosskunde.Checked = False
            trGrossKunde.Visible = False
            txtGrosskundennummer.Text = ""
            lblKundeShow.Text = ""
            divHalter.Visible = True
            txtName.Focus()
        End If


    End Sub

    Protected Sub txtKunde_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtKunde.TextChanged

        imgKunde.Visible = False

        Dim Kundenliste() As String = CType(Session("Kunden"), String())

        Dim i As Integer = Array.IndexOf(Kundenliste, txtKunde.Text)

        If i = -1 Then
            SetErrBehavior(txtKunde, lblKundeInfo, "Ungültige Kundenauswahl.")
        Else
            imgKunde.Visible = True
            If rdbGrosskunde.Checked = True Then
                txtGrosskundennummer.Focus()
            Else
                txtName.Focus()
            End If
        End If

    End Sub

    Protected Sub txtGrosskundennummer_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtGrosskundennummer.TextChanged
        imgGrossKunde.Visible = False

        If txtGrosskundennummer.Text.Length = 0 Then
            SetErrBehavior(txtGrosskundennummer, lblGrosskundeInfo, "Bitte geben Sie eine Großkundennummer ein.")
            Return
        End If

        'mBeauftragung = New Beauftragung(m_User, m_App, Session("AppID").ToString, Session.SessionID.ToString, "")
        mBeauftragung.Kundennr = Mid(txtKunde.Text, txtKunde.Text.IndexOf(" ~ ") + 4, 8)

        Dim Grosskunde As String = mBeauftragung.CheckGrosskundennummer("11000", txtGrosskundennummer.Text, Page)

        lblKundeShow.Text = Grosskunde

        If Grosskunde.Length = 0 Then
            SetErrBehavior(txtGrosskundennummer, lblGrosskundeInfo, "Keine Daten zur Großkundennummer gefunden.")
            txtGrosskundennummer.Focus()
        Else
            imgGrossKunde.Visible = True
            txtReferenz.Focus()
        End If

    End Sub

    'Protected Sub lbBack_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lbBack.Click
    '    Response.Redirect("../../../Start/Selection.aspx?AppID=" & Session("AppID").ToString, False)
    'End Sub

    Protected Sub lbCreate_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lbCreate.Click

        Dim booError As Boolean = False

        If ddlDienstleistung.SelectedItem.Value = "" Then
            If ddlDienstleistung.SelectedItem.Text = "Keine Dienstleistungen vorhanden" Then
                ddlDienstleistung.BorderColor = Drawing.Color.Red
                lblDienstleisungInfo.Text = "Es sind keine Dienstleistungen vorhanden. Wählen Sie eine andere StVA aus!"
                booError = True
            Else
                ddlDienstleistung.BorderColor = Drawing.Color.Red
                lblDienstleisungInfo.Text = "Bitte wählen Sie eine Dienstleistung aus."
                booError = True
            End If
        End If

        If txtEVB.Enabled And txtEVB.Text.Length = 0 Then
            SetErrBehavior(txtEVB, lblEVBInfo, "eVB-Nummer fehlt.")
            booError = True
        End If

        If txtZulDatum.Text.Length = 0 Then
            SetErrBehavior(txtZulDatum, lblZulDatumInfo, "Zulassungsdatum fehlt.")
            booError = True
        Else
            If IsDate(txtZulDatum.Text) = False Then
                SetErrBehavior(txtZulDatum, lblZulDatumInfo, "Ungültiges Datum.")
                booError = True
            ElseIf CDate(txtZulDatum.Text) < Today Then
                SetErrBehavior(txtZulDatum, lblZulDatumInfo, "Zulassungsdatum darf nicht in der Vergangenheit liegen.")
                booError = True
            End If
        End If

        If txtBlz.Enabled And txtKontonummer.Enabled And cbxEinzug.Enabled Then
            'ddlDienstleistung.SelectedItem.Value = "000000000000000619" OrElse ddlDienstleistung.SelectedItem.Value = "000000000000000593" Then

            'If rdbGrosskunde.Checked = False Then
            If txtBlz.Text.Length = 0 Then
                SetErrBehavior(txtBlz, lblBlzInfo, "BLZ fehlt.")
                booError = True
            Else
                If IsNumeric(txtBlz.Text) = False Then
                    SetErrBehavior(txtBlz, lblBlzInfo, "BLZ nicht numerisch.")
                    booError = True
                End If

            End If

            If txtKontonummer.Text.Length = 0 Then
                SetErrBehavior(txtKontonummer, lblKontonrInfo, "Kontonummer fehlt.")
                booError = True
            Else
                If IsNumeric(txtKontonummer.Text) = False Then
                    SetErrBehavior(txtKontonummer, lblKontonrInfo, "Kontonummer nicht numerisch.")
                    booError = True
                End If

            End If

            If cbxEinzug.Checked = False Then
                cbxEinzug.BorderColor = Drawing.Color.Red
                lblEinzugInfo.Text = "Einzugsermächtigung nicht angehakt."
                booError = True
            End If
        End If

        If trGutachten.Visible = True Then
            If ddlArtGenehmigung.SelectedItem.Value = "" Then
                ddlArtGenehmigung.BorderColor = Drawing.Color.Red
                lblArtGenehmigungInfo.Text = "Bitte wählen Sie eine Genehmigungsart aus."
                booError = True
            End If

            If ddlPrueforganisation.SelectedItem.Value = "" Then
                ddlPrueforganisation.BorderColor = Drawing.Color.Red
                lblPrueforganisationInfo.Text = "Bitte wählen Sie eine Prüforganisation aus."
                booError = True
            End If


        End If


        'End If

        'Halter ausgewählt: Kennzeichen prüfen
        If rdbGrosskunde.Checked = False Then

            If txtKennz1.Text = String.Empty OrElse Regex.IsMatch(txtKennz1.Text, "[A-Z]") = False Then
                SetErrBehavior(txtKennz1, lblKennzeichenInfo, "Kein reguläres Kennzeichen.")
                booError = True
            End If

            If txtKennz2.Text = String.Empty OrElse Regex.IsMatch(txtKennz2.Text, "[A-Z][0-9]") = False Then
                SetErrBehavior(txtKennz2, lblKennzeichenInfo, "Kein reguläres Kennzeichen.")
                booError = True
            End If

        End If

        Dim Kreise() As String = CType(Session("Kreise"), String())

        Dim i As Integer = Array.IndexOf(Kreise, txtKreise.Text)

        If i = -1 Then
            SetErrBehavior(txtKreise, lblKreiseInfo, "Ungültiger Kreis.")
            booError = True
        End If

        If booError = False Then

            txtBarcode.Focus()
            ModalPopupExtender2.Show()
        End If


    End Sub

    Protected Sub txtFinPruef_TextChanged(ByVal sender As Object, ByVal e As EventArgs) Handles txtFinPruef.TextChanged

        imgFahrgestellnummer.Visible = False


        Dim Fahrgestellnummer As String = txtFahrgestellnummer.Text

        If Fahrgestellnummer.Length < 17 OrElse txtFinPruef.Text.Length = 0 Then
            SetErrBehavior(txtFahrgestellnummer, lblPruefzifferInfo, "Fahrgestellnummer nicht 17-stellig oder Prüfziffer fehlt.")
            Return
        End If

        'If Not CType(Session("mBeauftragung"), Beauftragung) Is Nothing Then
        '    lblTypInfo.Text = CType(Session("mBeauftragung"), Beauftragung).TypdatenMessage
        'End If


        'mBeauftragung = New Beauftragung(m_User, m_App, Session("AppID").ToString, Session.SessionID.ToString, "")

        Dim Status As Integer = 0

        Status = mBeauftragung.CheckVin(Fahrgestellnummer.ToUpper, txtFinPruef.Text, Me)

        Select Case Status
            Case 1
                SetErrBehavior(txtFinPruef, lblPruefzifferInfo, "Prüfziffer nicht korrekt.")
            Case 2
                SetErrBehavior(txtFahrgestellnummer, lblPruefzifferInfo, "Fahrgestellnummer enthält unzulässige Zeichen.")
            Case Else
                imgFahrgestellnummer.Visible = True
                txtBriefnummer.Focus()
        End Select


    End Sub
    Private Sub txtBriefnummer_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtBriefnummer.TextChanged
        If ValidateBriefnummer() = False Then imgBriefnummer.Visible = True

        'If Not CType(Session("mBeauftragung"), Beauftragung) Is Nothing Then
        '    lblTypInfo.Text = CType(Session("mBeauftragung"), Beauftragung).TypdatenMessage
        'End If

    End Sub
    Protected Sub txtTypPruef_TextChanged(ByVal sender As Object, ByVal e As EventArgs) Handles txtTypPruef.TextChanged

        txtTyp.Text = txtTyp.Text.ToUpper
        txtFahrgestellnummer.Focus()

        Dim booError As Boolean = False

        imgTyp.Visible = False
        imgTypInfo.Visible = False
        imgHersteller.Visible = False
        If txtHersteller.Text.Length = 0 Then
            SetErrBehavior(txtHersteller, lblTypInfo, "Hersteller fehlt.")
            booError = True
        End If

        If txtTyp.Text.Length = 0 OrElse txtVarianteVersion.Text.Length = 0 OrElse txtTypPruef.Text.Length = 0 Then
            SetErrBehavior(txtHersteller, lblTypInfo, "Typdaten nicht vollständig.")
            booError = True
        Else
            txtTyp.Text.ToUpper()
        End If

        If booError = True Then Return


        'mBeauftragung = New Beauftragung(m_User, m_App, Session("AppID").ToString, Session.SessionID.ToString, "")

        Dim dt As DataTable

        dt = mBeauftragung.FillTypdaten(txtHersteller.Text.ToUpper, txtTyp.Text.ToUpper, txtVarianteVersion.Text.ToUpper, txtTypPruef.Text.ToUpper, Page)


        If dt.Rows.Count > 0 Then
            Dim dtRow As DataRow = dt.Rows(0)

            lblMarkeShow.Text = dtRow("ZZFABRIKNAME").ToString
            lblTypTextShow.Text = dtRow("ZZKLARTEXT_TYP").ToString
            lblVarianteShow.Text = dtRow("ZZVARIANTE").ToString
            lblVersionShow.Text = dtRow("ZZVERSION").ToString
            lblHandelsnameShow.Text = dtRow("ZZHANDELSNAME").ToString
            lblHerstellerKurzShow.Text = dtRow("ZZHERST_TEXT").ToString
        Else
            lblMarkeShow.Text = ""
            lblTypTextShow.Text = ""
            lblVarianteShow.Text = ""
            lblVersionShow.Text = ""
            lblHandelsnameShow.Text = ""
            lblHerstellerKurzShow.Text = ""
        End If


        lblTypInfo.Text = mBeauftragung.TypdatenMessage

        If lblTypInfo.Text.Length = 0 Then
            imgHersteller.Visible = True
            imgTyp.Visible = True
        Else
            imgHersteller.Visible = True
            imgTypInfo.Visible = True
            lblTypInfo.ForeColor = Drawing.Color.Gray
            lblTypInfo.Text = mBeauftragung.TypdatenMessage
        End If

        'Session("mBeauftragung") = mBeauftragung


    End Sub


    Private Sub txtKreise_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtKreise.TextChanged
        imgKreise.Visible = False

        Dim Kreise() As String = CType(Session("Kreise"), String())
        Dim booFound As Boolean = False

        If txtKreise.Text.Length < 4 Then

            For z = 0 To Kreise.Length - 1

                If txtKreise.Text.PadRight(4, "."c).ToUpper = Left(Kreise(z), 4) Then
                    txtKreise.Text = Kreise(z)
                    booFound = True
                    Exit For

                End If

            Next

            If booFound = True Then
                txtKennz1.Text = Left(txtKreise.Text, txtKreise.Text.IndexOf("."))
                imgKreise.Visible = True

                'mBeauftragung = New Beauftragung(m_User, m_App, Session("AppID").ToString, Session.SessionID.ToString, "")
                mBeauftragung.FillDienstleistungen(txtKennz1.Text, Left(m_User.Reference, 4), Me)
                Session("Dienstleistungen") = mBeauftragung.Dienstleistungen
                If mBeauftragung.Dienstleistungen.Rows.Count > 0 Then
                    ddlDienstleistung.DataSource = mBeauftragung.Dienstleistungen
                    ddlDienstleistung.DataTextField = "MAKTX"
                    ddlDienstleistung.DataValueField = "MATNR"
                Else
                    ddlDienstleistung.DataTextField = Nothing
                    ddlDienstleistung.DataValueField = Nothing
                    Dim lst As New List(Of String)
                    lst.Add("Keine Dienstleistungen vorhanden")
                    ddlDienstleistung.DataSource = lst
                End If
                ddlDienstleistung.DataBind()
            End If


        Else
            Dim i As Integer = Array.IndexOf(Kreise, txtKreise.Text)

            If i = -1 Then
                SetErrBehavior(txtKreise, lblKreiseInfo, "Ungültiger Kreis.")
            Else
                txtKennz1.Text = Left(txtKreise.Text, txtKreise.Text.IndexOf("."))
                imgKreise.Visible = True

                'mBeauftragung = New Beauftragung(m_User, m_App, Session("AppID").ToString, Session.SessionID.ToString, "")
                mBeauftragung.FillDienstleistungen(txtKennz1.Text, Left(m_User.Reference, 4), Me)
                Session("Dienstleistungen") = mBeauftragung.Dienstleistungen
                If mBeauftragung.Dienstleistungen.Rows.Count > 0 Then
                    ddlDienstleistung.DataSource = mBeauftragung.Dienstleistungen
                    ddlDienstleistung.DataTextField = "MAKTX"
                    ddlDienstleistung.DataValueField = "MATNR"
                Else
                    ddlDienstleistung.DataTextField = Nothing
                    ddlDienstleistung.DataValueField = Nothing
                    Dim lst As New List(Of String)
                    lst.Add("Keine Dienstleistungen vorhanden")
                    ddlDienstleistung.DataSource = lst
                End If
                ddlDienstleistung.DataBind()
            End If

        End If

        txtEVB.Focus()

    End Sub

    Private Sub btnOK_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnOK.Click

        lblSaveInfo.Visible = False

        If btnOK.Text = "Schließen" Then
            Response.Redirect("Change02s.aspx?AppID=" & Session("AppID").ToString, False)
            Session("mBeauftragung") = Nothing
            Exit Sub
        End If

        Dim MessageText As String = ""
        Dim booError As Boolean = False

        '*** Barcode überprüfen
        If txtBarcode.Text.Length = 0 Then

            lblSaveInfo.Text = "Bitte geben Sie einen Barcode ein."
            lblSaveInfo.ForeColor = Drawing.Color.Red
            lblSaveInfo.Visible = True
            'booError = True

            ModalPopupExtender2.Show()
            Return
        End If

        'mBeauftragung = New Beauftragung(m_User, m_App, Session("AppID").ToString, Session.SessionID.ToString, "")

        Dim Status As String = ""

        Status = mBeauftragung.CheckBarcode(txtBarcode.Text, Me.Page)

        Select Case Status

            Case "1"
                MessageText = "Barcode in Verwendung(Status: " & mBeauftragung.Statustext & ")."
            Case "2"
                MessageText = "Barcode nicht vorhanden."

        End Select

        If Status <> "0" Then
            lblSaveInfo.Text = MessageText
            lblSaveInfo.ForeColor = Drawing.Color.Red
            lblSaveInfo.Visible = True
            ModalPopupExtender2.Show()
            Return
        End If


        '*** Speichern
        With mBeauftragung

            'Grunddaten
            .Kundenname = Left(txtKunde.Text, txtKunde.Text.IndexOf(" ~ "))
            .Kundennr = Mid(txtKunde.Text, txtKunde.Text.IndexOf(" ~ ") + 4, 8)


            If rdbGrosskunde.Checked = True Then
                .Grosskundennr = txtGrosskundennummer.Text
                .Grosskunde = lblKundeShow.Text & ": " & txtGrosskundennummer.Text

            Else

                .Grosskundennr = ""
                .Grosskunde = ""

                .HalterAnrede = ddlAnrede.SelectedItem.Value

                'Firma

                Select Case ddlAnrede.SelectedItem.Value
                    Case "0"
                        .Name = txtName.Text & " " & txtName2.Text
                        .Fi = "X"
                    Case "1"
                        .Name = txtName2.Text
                        .He = "X"
                    Case "2"
                        .Name = txtName2.Text
                        .Fr = "X"
                End Select


                .Geburtsname = ""


                .Haltername1 = txtName.Text
                .Haltername2 = txtName2.Text


                If ddlAnrede.SelectedItem.Value <> "0" Then
                    .Geburtstag = txtGeburtstag.Text
                End If

                .Geburtsort = txtGeburtsort.Text
                .HalterStrasse = txtStrasse.Text
                .HalterHausnr = txtHausnummer.Text
                .HalterHausnrZusatz = txtHnrZusatz.Text
                .HalterPLZ = txtPLZ.Text
                .HalterOrt = txtOrt.Text

            End If

            .HalterReferenz = txtReferenz.Text
            .Bestellnummer = txtBestellnr.Text

            .VerkKuerz = txtVerkaeuferkuerzel.Text
            .KiReferenz = txtKundenreferenz.Text
            .Notiz = txtNotiz.Text

            'Fahrzeugdaten
            .Hersteller = txtHersteller.Text
            .Typ = txtTyp.Text
            .VarianteVersion = txtVarianteVersion.Text
            .TypPruef = txtTypPruef.Text
            .Fahrgestellnummer = txtFahrgestellnummer.Text
            .Briefnummer = txtBriefnummer.Text
            'Dienstleistung
            .StVA = txtKreise.Text
            .StVANr = Left(txtKreise.Text, txtKreise.Text.IndexOf("."))
            .Kennzeichen = txtKennz1.Text.ToUpper & "-" & txtKennz2.Text
            If txtEVB.Enabled Then
                .EVB = txtEVB.Text
            End If
            .Zulassungsdatum = txtZulDatum.Text
            .Bemerkung = txtDienstBemerkung.Text
            .NaechsteHU = txtNaechsteHU.Text.Replace("."c, "")
            .Materialnummer = ddlDienstleistung.SelectedItem.Value

            .Einzelkennzeichen = cbxEinKennz.Checked
            .Krad = cbxKrad.Checked
            .KennzeichenTyp = ddlKennzTyp.SelectedItem.Value
            .Feinstaubplakette = cbxFeinstaub.Checked
            .Wunschkennzeichen = cbxWunschkennzFlag.Checked
            .Reserviert = cbxReserviert.Checked
            .Reservierungsnr = txtReservNr.Text

            .Referenz = txtBarcode.Text

            'Bankdaten
            If txtBlz.Enabled Then
                .BLZ = txtBlz.Text
            End If
            If txtKontonummer.Enabled Then
                .Kontonummer = txtKontonummer.Text
            End If
            If cbxEinzug.Enabled Then
                .Einzug = IIf(cbxEinzug.Checked, "X", "").ToString
            End If

            'Gutachten
            If trArtGenehmigung.Visible Then
                .ArtGenehmigung = ddlArtGenehmigung.SelectedItem.Value
            End If
            If trPrueforganisation.Visible Then
                .Prueforganisation = ddlPrueforganisation.SelectedItem.Value
            End If
            If trGutachtenNr.Visible Then
                .GutachtenNummer = txtGutachtenNr.Text
            End If
        End With

        If mBeauftragung.Save(Session("AppID").ToString, Session.SessionID.ToString, Me.Page) = True Then

            CType(Session("mBeauftragung"), Beauftragung).nPaUsed = False

            pnlDiv1.Visible = False
            pnlDiv2.Visible = False
            pnlDiv3.Visible = False

            btnOK.Text = "Schließen"
            btnCancel.Width = 0
            btnCancel.Text = ""
            btnCancel.Style.Add("display", "none")


            'mb.Height = 180
            divPdf.Visible = True


            ModalPopupExtender2.Show()

            Session("Dienstleistung") = Nothing

        Else
            lblSaveInfo.Text = mBeauftragung.ErrorText
            lblSaveInfo.ForeColor = Drawing.Color.Red
            lblSaveInfo.Visible = True
            ModalPopupExtender2.Show()
        End If




    End Sub

    Private Sub ddlDienstleistung_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlDienstleistung.SelectedIndexChanged

        'If ddlDienstleistung.SelectedItem.Value = "000000000000000619" OrElse ddlDienstleistung.SelectedItem.Value = "000000000000000593" Then
        '    txtBlz.Enabled = True
        '    txtKontonummer.Enabled = True
        '    cbxEinzug.Enabled = True
        'Else
        '    txtBlz.Enabled = False
        '    txtKontonummer.Enabled = False
        '    cbxEinzug.Enabled = False
        '    cbxEinzug.Checked = False
        'End If

        Session("Dienstleistung") = ddlDienstleistung.SelectedItem.Value

        If Not ddlDienstleistung.SelectedItem.Value = "" And Not Session("Dienstleistungen") Is Nothing Then
            Dim dt As DataTable = CType(Session("Dienstleistungen"), DataTable)
            Dim DRow As DataRow = dt.Select("MATNR='" & ddlDienstleistung.SelectedItem.Value & "'")(0)
            mBeauftragung.GutachtenNeeded = CChar(DRow("GUT_ERF"))
            mBeauftragung.NaechsteHUNeeded = CChar(DRow("NHU_ERF"))
            mBeauftragung.BankdatenNeeded = CChar(DRow("BANK_ERF"))
            mBeauftragung.EvBNeeded = CChar(DRow("EVB_ERF"))
        End If

        PrepareDienstleistungControls()

        If ddlDienstleistung.SelectedItem.Value = "" Then
            EnableGutachten(False)
        End If


        txtKreise.Focus()
    End Sub

    Protected Sub ibtNpaLogo_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtNpaLogo.Click

        SetHalter()

        If Session("Me") Is Nothing Then
            Session.Add("Me", Me)

        Else
            Session("Me") = Me
        End If

        If Session("NpaAppID") Is Nothing Then
            Session.Add("NpaAppID", Page.Request.QueryString("AppID").ToString)

        Else
            Session("NpaAppID") = Page.Request.QueryString("AppID").ToString
        End If

        Dim URL As String = ConfigurationManager.AppSettings("nPaURL")

        Response.Redirect(URL, True)

    End Sub


    Private Sub lbtPDF_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles lbtPDF.Click

        'mBeauftragung = CType(Session("mBeauftragung"), Beauftragung)

        Dim SelectPath As String
        If cbxEinzug.Checked = False Then
            SelectPath = "\Components\ComCommon\documents\Zulassungsantrag.doc"
        Else
            SelectPath = "\Components\ComCommon\documents\Zulassungsantrag2.doc"
        End If

        Dim tblData As DataTable = Base.Kernel.Common.DataTableHelper.ObjectToDataTable(mBeauftragung)
        Dim imageHt As New Hashtable()
        Try
            imageHt.Add("Logo", m_User.Customer.LogoImage)
        Catch ex As Exception
            ' LogoPath am Customer nicht (korrekt) gepflegt - hier: ignorieren
        End Try
        Dim docFactory As New Base.Kernel.DocumentGeneration.WordDocumentFactory(tblData, imageHt)
        docFactory.CreateDocument("Zulassungsantrag_" & m_User.UserName, Me.Page, SelectPath)

    End Sub

    Protected Sub ddlAnrede_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ddlAnrede.SelectedIndexChanged

        SetHalter()
    End Sub

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        SetEndASPXAccess(Me)
    End Sub

    Private Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Unload
        SetEndASPXAccess(Me)
    End Sub

    'Protected Sub txtKreise_TextChanged(ByVal sender As Object, ByVal e As EventArgs) Handles txtKreise.TextChanged

    'End Sub

#End Region


#Region "Methods"

    Private Sub SetControls()

        If Not Session("Me") Is Nothing Then

            Dim MeControls As Change02s = CType(Session("Me"), Change02s)

            Dim item As ListItem


            'Anrede füllen
            item = New ListItem()
            item.Text = "Auswahl"
            item.Value = "-1"
            ddlAnrede.Items.Add(item)

            item = New ListItem()
            item.Text = "Firma"
            item.Value = "0"
            ddlAnrede.Items.Add(item)

            item = New ListItem()
            item.Text = "Herr"
            item.Value = "1"
            ddlAnrede.Items.Add(item)

            item = New ListItem()
            item.Text = "Frau"
            item.Value = "2"
            ddlAnrede.Items.Add(item)


            txtKunde.Text = MeControls.txtKunde.Text
            rdbGrosskunde.Checked = False
            rdbHalter.Checked = True
            trGrossKunde.Visible = False
            divHalter.Visible = True
            ddlAnrede.SelectedValue = MeControls.ddlAnrede.SelectedValue
            txtReferenz.Text = MeControls.txtReferenz.Text
            txtBestellnr.Text = MeControls.txtBestellnr.Text
            txtVerkaeuferkuerzel.Text = MeControls.txtVerkaeuferkuerzel.Text
            txtKundenreferenz.Text = MeControls.txtKundenreferenz.Text
            txtNotiz.Text = MeControls.txtNotiz.Text

            divNpa.Visible = MeControls.divNpa.Visible

            txtName.Enabled = False
            txtName.Text = ""
            txtName2.Enabled = False
            txtName2.Text = ""
            txtGeburtstag.Enabled = False
            txtGeburtstag.Text = ""
            txtGeburtsort.Enabled = False
            txtGeburtsort.Text = ""
            txtStrasse.Enabled = False
            txtStrasse.Text = ""
            txtHausnummer.Enabled = False
            txtHausnummer.Text = ""
            txtHnrZusatz.Enabled = False
            txtHnrZusatz.Text = ""
            txtPLZ.Enabled = False
            txtPLZ.Text = ""
            txtOrt.Enabled = False
            txtOrt.Text = ""

            imgAnrede.Visible = False
            imgHaltername.Visible = False
            imgHaltername2.Visible = False
            imgHalterstrasse.Visible = False
            imgHalterPlzOrt.Visible = False

            imgHalternameNpa.Visible = True
            imgHaltername2Npa.Visible = True
            imgHalterstrasseNpa.Visible = True
            imgHalterPlzOrtNpa.Visible = True
            imgGeburtsortNpa.Visible = True
            imgGeburtstagNpa.Visible = True

        End If



        If Not Request.QueryString.Item("eIdentityResponse") Is Nothing Then


            SetHalter()


            If Not Request.QueryString.Item("GivenNames") Is Nothing Then
                txtName.Text = Request.QueryString.Item("GivenNames").ToString
                CType(Session("mBeauftragung"), Beauftragung).nPaUsed = True
            End If

            If Not Request.QueryString.Item("FamilyNames") Is Nothing Then
                txtName2.Text = Request.QueryString.Item("FamilyNames").ToString
            End If

            If Not Request.QueryString.Item("PlaceOfResidence") Is Nothing Then

                txtStrasse.Text = Request.QueryString.Item("PlaceOfResidence")

                Dim PlaceOfResidence() As String = Split(Request.QueryString.Item("PlaceOfResidence").ToString, ",")

                If Not PlaceOfResidence(3) Is Nothing Then
                    txtStrasse.Text = PlaceOfResidence(3).ToString
                End If

                If Not PlaceOfResidence(2) Is Nothing Then
                    txtOrt.Text = PlaceOfResidence(2).ToString
                End If

                If PlaceOfResidence.Length > 4 Then
                    If Not PlaceOfResidence(4) Is Nothing Then
                        txtPLZ.Text = PlaceOfResidence(4).ToString
                    End If
                End If

            End If

            If Not Request.QueryString.Item("DateOfBirth") Is Nothing Then

                If Request.QueryString.Item("DateOfBirth").ToString.Length > 0 Then

                    txtGeburtstag.Text = Request.QueryString.Item("DateOfBirth").ToString
                    txtGeburtstag.Text = CDate(txtGeburtstag.Text).ToShortDateString
                End If

            End If

            If Not Request.QueryString.Item("PlaceOfBirth") Is Nothing Then

                txtGeburtsort.Text = Request.QueryString.Item("PlaceOfBirth").ToString

            End If

            Session("Me") = Me

            Response.Redirect("Change02s.aspx?AppID=" & Session("AppID").ToString, True)

        End If


    End Sub

    Private Sub Reset()

        Dim MeControls As Change02s = CType(Session("Me"), Change02s)


        Dim item As ListItem


        'Anrede füllen
        item = New ListItem()
        item.Text = "Auswahl"
        item.Value = "-1"
        ddlAnrede.Items.Add(item)

        item = New ListItem()
        item.Text = "Firma"
        item.Value = "0"
        ddlAnrede.Items.Add(item)

        item = New ListItem()
        item.Text = "Herr"
        item.Value = "1"
        ddlAnrede.Items.Add(item)

        item = New ListItem()
        item.Text = "Frau"
        item.Value = "2"
        ddlAnrede.Items.Add(item)


        txtKunde.Text = MeControls.txtKunde.Text
        rdbGrosskunde.Checked = False
        rdbHalter.Checked = True
        trGrossKunde.Visible = False
        divHalter.Visible = True
        ddlAnrede.SelectedValue = MeControls.ddlAnrede.SelectedValue
        txtReferenz.Text = MeControls.txtReferenz.Text
        txtBestellnr.Text = MeControls.txtBestellnr.Text
        txtVerkaeuferkuerzel.Text = MeControls.txtVerkaeuferkuerzel.Text
        txtKundenreferenz.Text = MeControls.txtKundenreferenz.Text
        txtNotiz.Text = MeControls.txtNotiz.Text

        divNpa.Visible = MeControls.divNpa.Visible


        trGeburtsort.Visible = True
        trGeburtstag.Visible = True

        txtName.Text = MeControls.txtName.Text
        txtName.Enabled = False
        txtName2.Text = MeControls.txtName2.Text
        txtName2.Enabled = False
        txtGeburtstag.Text = MeControls.txtGeburtstag.Text
        txtGeburtstag.Enabled = False
        txtGeburtsort.Text = MeControls.txtGeburtsort.Text
        txtGeburtsort.Enabled = False
        txtStrasse.Text = MeControls.txtStrasse.Text
        txtStrasse.Enabled = False
        txtHausnummer.Text = MeControls.txtHausnummer.Text
        txtHausnummer.Enabled = False
        txtHnrZusatz.Text = MeControls.txtHnrZusatz.Text
        txtHnrZusatz.Enabled = False
        txtPLZ.Text = MeControls.txtPLZ.Text
        txtPLZ.Enabled = False
        txtOrt.Text = MeControls.txtOrt.Text
        txtOrt.Enabled = False
        imgHalternameNpa.Visible = True
        imgHaltername2Npa.Visible = True
        imgHalterstrasseNpa.Visible = True
        imgHalterPlzOrtNpa.Visible = True
        imgGeburtsortNpa.Visible = True
        imgGeburtstagNpa.Visible = True

        Session("Me") = Nothing

    End Sub


    Private Sub InitDropdown()

        'txtZulDatum.Text = Date.Today.ToShortDateString

        'mBeauftragung = New Beauftragung(m_User, m_App, Session("AppID").ToString, Session.SessionID.ToString, "")

        With mBeauftragung
            .Gruppe = m_User.Groups(0).GroupName
            .Verkaufsbuero = Right(m_User.Reference, 4)
            .Verkaufsorganisation = Left(m_User.Reference, 4)
        End With

        mBeauftragung.Fill(Session("AppID").ToString, Session.SessionID.ToString, Me)


        'Dim NewRow As DataRow

        'NewRow = mBeauftragung.Dienstleistungen.NewRow()
        'NewRow("MAKTX") = String.Empty
        'NewRow("MATNR") = String.Empty
        'mBeauftragung.Dienstleistungen.Rows.Add(NewRow)
        'mBeauftragung.Dienstleistungen.AcceptChanges()

        'Dim dv As DataView

        'dv = mBeauftragung.Dienstleistungen.DefaultView
        'dv.Sort = "MAKTX"
        'With ddlDienstleistung
        '    .DataSource = dv
        '    .DataTextField = "MAKTX"
        '    .DataValueField = "MATNR"
        '    .DataBind()
        'End With

        'KennzeichenTyp füllen

        Dim item As ListItem

        item = New ListItem()
        item.Text = "E - Euro"
        item.Value = "E"
        ddlKennzTyp.Items.Add(item)

        item = New ListItem()
        item.Text = "F - Fun"
        item.Value = "F"
        ddlKennzTyp.Items.Add(item)

        item = New ListItem()
        item.Text = "H - Historisch"
        item.Value = "H"
        ddlKennzTyp.Items.Add(item)

        item = New ListItem()
        item.Text = "K - Kurzzeit"
        item.Value = "K"
        ddlKennzTyp.Items.Add(item)

        item = New ListItem()
        item.Text = "S - Saison"
        item.Value = "S"
        ddlKennzTyp.Items.Add(item)

        item = New ListItem()
        item.Text = "Z - Zoll"
        item.Value = "Z"
        ddlKennzTyp.Items.Add(item)

        If Session("ddlKennzTyp") Is Nothing Then

            Session.Add("ddlKennzTyp", ddlKennzTyp)
        End If


        'Anrede füllen
        item = New ListItem()
        item.Text = "Auswahl"
        item.Value = "-1"
        ddlAnrede.Items.Add(item)

        item = New ListItem()
        item.Text = "Firma"
        item.Value = "0"
        ddlAnrede.Items.Add(item)

        item = New ListItem()
        item.Text = "Herr"
        item.Value = "1"
        ddlAnrede.Items.Add(item)

        item = New ListItem()
        item.Text = "Frau"
        item.Value = "2"
        ddlAnrede.Items.Add(item)


        Dim arrKunden() As String
        Dim i As Integer = 0

        For Each Row As DataRow In mBeauftragung.Kunden.Rows

            ReDim Preserve arrKunden(i)

            arrKunden(i) = Row("NAME1").ToString
            i += 1
        Next

        Session.Add("Kunden", arrKunden)

        Dim arrKreise() As String

        i = 0

        For Each Row As DataRow In mBeauftragung.Kreise.Rows

            ReDim Preserve arrKreise(i)

            arrKreise(i) = Row("KREISBEZ").ToString
            i += 1
        Next

        Session.Add("Kreise", arrKreise)


    End Sub



    Private Function ValidateGrunddaten() As Boolean

        Dim booError As Boolean = False
        Dim ErrText As String = ""
        Dim Name As String = ""
        imgKunde.Visible = False
        imgGrossKunde.Visible = False
        imgHaltername.Visible = False
        imgHalterstrasse.Visible = False
        imgHalterPlzOrt.Visible = False


        Dim Kundenliste() As String = CType(Session("Kunden"), String())

        Dim i As Integer = Array.IndexOf(Kundenliste, txtKunde.Text)

        If i = -1 Then
            SetErrBehavior(txtKunde, lblKundeInfo, "Ungültige Kundenauswahl.")
            booError = True
        Else
            imgKunde.Visible = True
        End If

        If rdbGrosskunde.Checked = True Then

            If txtGrosskundennummer.Text.Length = 0 Then
                SetErrBehavior(txtGrosskundennummer, lblGrosskundeInfo, "Grosskundennummer fehlt.")
                booError = True
            Else
                imgGrossKunde.Visible = True
            End If
        Else

            'Nur Enabled, wenn nicht elektronischer Perso gewählt wurde
            If txtName.Enabled = True Then

                Select Case ddlAnrede.SelectedItem.Value
                    Case "-1"
                        'SetErrBehavior(txtName, lblAnredeInfo, "Anrede fehlt.")
                        ddlAnrede.BorderColor = Drawing.Color.Red
                        'lblAnredeInfo.Visible = True
                        lblAnredeInfo.Text = "Anrede fehlt"
                        booError = True
                        Name = "Name1"
                    Case "1", "2" 'Herr, Frau
                        If txtName2.Text.Length = 0 Then
                            SetErrBehavior(txtName2, lblName2Info, "Nachname fehlt.")
                            booError = True
                            Name = "Nachname"
                        Else
                            imgHaltername2.Visible = True

                            If txtGeburtstag.Text.Length > 0 Then

                                If IsDate(txtGeburtstag.Text) = False Then
                                    SetErrBehavior(txtGeburtstag, lblGeburtstagInfo, "Kein gültiges Datum")
                                    booError = True
                                Else
                                    If CDate(txtGeburtstag.Text) > DateAdd(DateInterval.Year, -18, Date.Today) Then
                                        SetErrBehavior(txtGeburtstag, lblGeburtstagInfo, "Halter ist noch keine 18.")
                                        booError = True

                                    End If
                                End If
                            End If

                            If txtGeburtsort.Text.Length = 0 Then
                                SetErrBehavior(txtGeburtsort, lblGeburtsortInfo, "Geburtsort fehlt.")
                                booError = True
                            End If
                        End If
                    Case Else 'Firma
                        Name = "Name1"
                End Select


                If txtName.Text.Length = 0 Then
                    SetErrBehavior(txtName, lblNameInfo, Name & " fehlt.")
                    booError = True
                Else
                    imgHaltername.Visible = True
                End If

                If txtStrasse.Text.Length = 0 Then
                    SetErrBehavior(txtStrasse, lblStrasseInfo, "Strasse fehlt.")
                    booError = True
                Else

                    If txtHausnummer.Text.Length > 0 Then

                        If IsNumeric(txtHausnummer.Text) = False Then
                            SetErrBehavior(txtHausnummer, lblStrasseInfo, "Buchstaben bitte in das Feld rechts.")
                            booError = True
                        Else
                            imgHalterstrasse.Visible = True
                        End If
                    Else
                        imgHalterstrasse.Visible = True
                    End If



                End If

                If txtPLZ.Text.Length = 0 Then
                    SetErrBehavior(txtPLZ, lblOrtInfo, "PLZ fehlt.")
                    booError = True
                End If

                If txtOrt.Text.Length = 0 Then

                    If txtPLZ.Text.Length = 0 Then
                        SetErrBehavior(txtOrt, lblOrtInfo, "PLZ und Ort fehlen.")
                        booError = True
                    Else
                        SetErrBehavior(txtOrt, lblOrtInfo, "Ort fehlt.")
                        booError = True
                    End If

                End If

                If txtPLZ.Text.Length > 0 AndAlso txtOrt.Text.Length > 0 Then
                    imgHalterPlzOrt.Visible = True
                End If

            End If

        End If


        Return booError

    End Function

    Private Function ValidateFahrzeugdaten(ByVal Base As Integer) As Boolean
        Dim booError As Boolean = False



        Select Case Base
            Case Source.Part
                If imgFahrgestellnummer.Visible = False Then booError = True
                If imgTyp.Visible = False AndAlso imgTypInfo.Visible = False Then booError = True
                If imgBriefnummer.Visible = False Then booError = True
            Case Source.Full
                imgHersteller.Visible = False
                imgTyp.Visible = False
                imgTypInfo.Visible = False
                imgFahrgestellnummer.Visible = False
                imgBriefnummer.Visible = False

                If txtFahrgestellnummer.Text.Length = 0 Then
                    SetErrBehavior(txtFahrgestellnummer, lblPruefzifferInfo, "")
                    booError = True
                End If

                If txtFinPruef.Text.Length = 0 Then
                    SetErrBehavior(txtFinPruef, lblPruefzifferInfo, "")
                    booError = True
                End If

                'If txtFahrgestellnummer.Text.Length = 0 OrElse txtFinPruef.Text.Length = 0 Then
                '    lblPruefzifferInfo.Text = "Fahrgestellnummer/Prüfziffer nicht korrekt gefüllt."
                'End If

                If ValidateBriefnummer() = True Then booError = True

                If txtHersteller.Text.Length = 0 Then
                    SetErrBehavior(txtHersteller, lblTypInfo, "")
                    booError = True
                End If

                If txtTyp.Text.Length = 0 Then
                    SetErrBehavior(txtTyp, lblTypInfo, "")
                    booError = True
                End If

                If txtVarianteVersion.Text.Length = 0 Then
                    SetErrBehavior(txtVarianteVersion, lblTypInfo, "")
                    booError = True
                End If

                If txtTypPruef.Text.Length = 0 Then
                    SetErrBehavior(txtVarianteVersion, lblTypInfo, "")
                    booError = True
                End If

                If txtHersteller.Text.Length = 0 OrElse _
                    txtTyp.Text.Length = 0 OrElse _
                    txtVarianteVersion.Text.Length = 0 OrElse _
                    txtTypPruef.Text.Length = 0 Then

                    lblTypInfo.Text = "Typdaten nicht korrekt gefüllt."

                End If

        End Select

        Return booError


    End Function

    Private Function ValidateBriefnummer() As Boolean

        Dim booError As Boolean = False

        If txtBriefnummer.Text.Length < 8 Then

            SetErrBehavior(txtBriefnummer, lblBriefnummerInfo, "Bitte geben Sie eine achtstellige ZBII-Nr. ein.")
            booError = True
        Else
            If IsNumeric(Left(txtBriefnummer.Text, 1)) = True OrElse IsNumeric(Mid(txtBriefnummer.Text, 1, 1)) = True Then
                SetErrBehavior(txtBriefnummer, lblBriefnummerInfo, "Die ersten beiden Zeichen müssen Buchstaben sein.")
                booError = True
            End If

            If IsNumeric(Right(txtBriefnummer.Text, 6)) = False Then
                SetErrBehavior(txtBriefnummer, lblBriefnummerInfo, "Die letzten 6 Zeichen müssen numerisch sein.")
                booError = True
            End If


        End If

        Return booError

    End Function
    Private Sub InitControls()

        txtKunde.BorderColor = Drawing.Color.Empty
        lblKundeInfo.Text = ""

        txtGrosskundennummer.BorderColor = Drawing.Color.Empty
        lblGrosskundeInfo.Text = ""

        ddlAnrede.BorderColor = Drawing.Color.Empty
        lblAnredeInfo.Text = ""

        txtName.BorderColor = Drawing.Color.Empty
        lblNameInfo.Text = ""

        txtName2.BorderColor = Drawing.Color.Empty
        lblName2Info.Text = ""

        txtGeburtstag.BorderColor = Drawing.Color.Empty
        lblGeburtstagInfo.Text = ""

        txtGeburtsort.BorderColor = Drawing.Color.Empty
        lblGeburtsortInfo.Text = ""

        txtStrasse.BorderColor = Drawing.Color.Empty
        txtHausnummer.BorderColor = Drawing.Color.Empty
        lblStrasseInfo.Text = ""


        txtPLZ.BorderColor = Drawing.Color.Empty
        txtOrt.BorderColor = Drawing.Color.Empty
        lblOrtInfo.Text = ""

        txtFahrgestellnummer.BorderColor = Drawing.Color.Empty
        txtFinPruef.BorderColor = Drawing.Color.Empty
        lblPruefzifferInfo.Text = ""

        txtBriefnummer.BorderColor = Drawing.Color.Empty
        lblBriefnummerInfo.Text = ""

        txtHersteller.BorderColor = Drawing.Color.Empty
        lblHerstellerInfo.Text = ""

        txtTyp.BorderColor = Drawing.Color.Empty
        lblTypInfo.Text = ""

        txtVarianteVersion.BorderColor = Drawing.Color.Empty
        txtTypPruef.BorderColor = Drawing.Color.Empty

        txtEVB.BorderColor = Drawing.Color.Empty
        lblEVBInfo.Text = ""

        txtZulDatum.BorderColor = Drawing.Color.Empty
        lblZulDatumInfo.Text = ""

        txtKreise.BorderColor = Drawing.Color.Empty
        lblKreiseInfo.Text = ""

        txtBlz.BorderColor = Drawing.Color.Empty
        lblBlzInfo.Text = ""

        txtKontonummer.BorderColor = Drawing.Color.Empty
        lblKontonrInfo.Text = ""

        cbxEinzug.BorderColor = Drawing.Color.Empty
        lblEinzugInfo.Text = ""

        ddlArtGenehmigung.BorderColor = Drawing.Color.Empty
        lblArtGenehmigungInfo.Text = ""

        ddlPrueforganisation.BorderColor = Drawing.Color.Empty
        lblPrueforganisationInfo.Text = ""

        txtKennz1.BorderColor = Drawing.Color.Empty
        txtKennz2.BorderColor = Drawing.Color.Empty
        lblKennzeichenInfo.Text = ""

        lblSaveInfo.Text = ""

    End Sub

    Private Sub SetErrBehavior(ByVal txtcontrol As TextBox, ByVal lblControl As Label, ByVal ErrText As String)

        txtcontrol.BorderColor = Drawing.Color.Red

        lblControl.Text = ErrText

    End Sub

    Private Sub ClearHalter()


        ddlAnrede.SelectedIndex = 0


        txtName.Enabled = True
        txtName.Text = ""
        txtName2.Enabled = True
        txtName2.Text = ""

        txtGeburtstag.Enabled = True
        txtGeburtstag.Text = ""
        txtGeburtsort.Enabled = True
        txtGeburtsort.Text = ""

        txtStrasse.Enabled = True
        txtStrasse.Text = ""
        txtHausnummer.Enabled = True
        txtHausnummer.Text = ""
        txtHnrZusatz.Enabled = True
        txtHnrZusatz.Text = ""
        txtPLZ.Enabled = True
        txtPLZ.Text = ""
        txtOrt.Enabled = True
        txtOrt.Text = ""

        imgAnrede.Visible = False
        imgHaltername.Visible = False
        imgHaltername2.Visible = False
        imgHalterstrasse.Visible = False
        imgHalterPlzOrt.Visible = False

        imgHalternameNpa.Visible = False
        imgHaltername2Npa.Visible = False
        imgHalterstrasseNpa.Visible = False
        imgHalterPlzOrtNpa.Visible = False
        imgGeburtsortNpa.Visible = False
        imgGeburtstagNpa.Visible = False


    End Sub

    Private Sub SetHalter()

        If ddlAnrede.SelectedItem Is Nothing Then Exit Sub

        If ddlAnrede.SelectedItem.Value = "-1" Then ClearHalter()

        Select Case ddlAnrede.SelectedItem.Value
            Case "0", "-1"
                lblName.Text = "Name1*"
                lblName2.Text = "Name2"

                divNpa.Visible = False
                trGeburtsort.Visible = False
                trGeburtstag.Visible = False
            Case Else
                divNpa.Visible = True
                lblName.Text = "Vorname*"
                lblName2.Text = "Nachname*"
                trGeburtsort.Visible = True
                trGeburtstag.Visible = True
        End Select
    End Sub

    Private Sub InitPage()

        Session("Kunden") = Nothing
        Session("Kreise") = Nothing

        InitDropdown()
        InitJava()


    End Sub

    Private Sub InitJava()
        Me.txtHersteller.Attributes("onkeyup") = "autotab(" & Me.txtHersteller.ClientID & ", " & Me.txtTyp.ClientID & ")"
        Me.txtTyp.Attributes("onkeyup") = "autotab(" & Me.txtTyp.ClientID & ", " & Me.txtVarianteVersion.ClientID & ")"
        Me.txtVarianteVersion.Attributes("onkeyup") = "autotab(" & Me.txtVarianteVersion.ClientID & ", " & Me.txtTypPruef.ClientID & ")"
        Me.txtTypPruef.Attributes("onkeyup") = "autotab(" & Me.txtTypPruef.ClientID & ", " & Me.txtFahrgestellnummer.ClientID & ")"
        Me.txtFahrgestellnummer.Attributes("onkeyup") = "autotab(" & Me.txtFahrgestellnummer.ClientID & ", " & Me.txtFinPruef.ClientID & ")"
        Me.txtFinPruef.Attributes("onkeyup") = "autotab(" & Me.txtFinPruef.ClientID & ", " & Me.txtBriefnummer.ClientID & ")"
    End Sub

    Public Shared Function ChangeDate(ByVal [date] As [String]) As String

        Dim dateTime As System.DateTime = System.DateTime.FromOADate(System.Convert.ToDouble([date]) / (60 * 60 * 24 * 1000))

        dateTime += System.DateTime.Parse("Jan 1, 1970") - System.DateTime.Parse("Dec 30, 1899")


        Return dateTime.ToString()
    End Function

    Private Function UnixMillisToDateTime(ByVal str As String) As DateTime
        Dim millis As Long = Long.Parse(str)
        Dim epoch = New DateTime(1970, 1, 1, 0, 0, 0, _
        0, DateTimeKind.Utc)
        Return epoch.AddMilliseconds(millis)
    End Function

    Private Sub PrepareDienstleistungControls()

        Dim cBankdaten As Char = mBeauftragung.BankdatenNeeded
        Dim cEvB As Char = mBeauftragung.EvBNeeded
        Dim bNächsteHU As Char = mBeauftragung.NaechsteHUNeeded
        Dim bGutachten As Char = mBeauftragung.GutachtenNeeded

        Select Case cBankdaten
            Case "N"c
                EnableBankdaten(False)
            Case "H"c
                If (rdbHalter.Checked) Then
                    EnableBankdaten(True)
                Else
                    EnableBankdaten(False)
                End If
            Case "G"c
                If (rdbGrosskunde.Checked) Then
                    EnableBankdaten(True)
                Else
                    EnableBankdaten(False)
                End If
            Case "B"c
                EnableBankdaten(True)
            Case Else
                EnableBankdaten(False)
        End Select

        Select Case cEvB
            Case "N"c
                EnableEvB(False)
            Case "H"c
                If (rdbHalter.Checked) Then
                    EnableEvB(True)
                Else
                    EnableEvB(False)
                End If
            Case "G"c
                If (rdbGrosskunde.Checked) Then
                    EnableEvB(True)
                Else
                    EnableEvB(False)
                End If
            Case "B"c
                EnableEvB(True)
            Case Else
                EnableEvB(False)
        End Select

        Select Case bNächsteHU
            Case "N"c
                EnableNaechteHU(False)
            Case "H"c
                If (rdbHalter.Checked) Then
                    EnableNaechteHU(True)
                Else
                    EnableNaechteHU(False)
                End If
            Case "G"c
                If (rdbGrosskunde.Checked) Then
                    EnableNaechteHU(True)
                Else
                    EnableNaechteHU(False)
                End If
            Case "B"c
                EnableNaechteHU(True)
            Case Else
                EnableNaechteHU(False)
        End Select

        Select Case bGutachten
            Case "N"c
                EnableGutachten(False)
            Case "H"c
                If (rdbHalter.Checked) Then
                    EnableGutachten(True)
                Else
                    EnableGutachten(False)
                End If
            Case "G"c
                If (rdbGrosskunde.Checked) Then
                    EnableGutachten(True)
                Else
                    EnableGutachten(False)
                End If
            Case "B"c
                EnableGutachten(True)
            Case Else
                EnableGutachten(False)
        End Select

    End Sub

    Private Sub EnableBankdaten(ByVal doEnable As Boolean)
        If doEnable Then
            txtBlz.Enabled = True
            txtBlz.BackColor = Drawing.Color.White
            txtKontonummer.Enabled = True
            txtKontonummer.BackColor = Drawing.Color.White
            cbxEinzug.Enabled = True
        Else
            txtBlz.Enabled = False
            txtBlz.Text = ""
            txtBlz.BackColor = Drawing.Color.LightGray
            txtKontonummer.Enabled = False
            txtKontonummer.Text = ""
            txtKontonummer.BackColor = Drawing.Color.LightGray
            cbxEinzug.Enabled = False
        End If

    End Sub

    Private Sub EnableEvB(ByVal doEnable As Boolean)
        If doEnable Then
            txtEVB.Enabled = True
            txtEVB.BackColor = Drawing.Color.White
        Else
            txtEVB.Enabled = False
            txtEVB.Text = String.Empty
            txtEVB.BackColor = Drawing.Color.LightGray
        End If
    End Sub

    Private Sub EnableNaechteHU(ByVal doEnable As Boolean)
        If doEnable Then
            trNaechsteHU.Visible = True
        Else
            trNaechsteHU.Visible = False
            txtNaechsteHU.Text = String.Empty
        End If
    End Sub

    Private Sub EnableGutachten(ByVal doEnable As Boolean)
        If doEnable Then
            trGutachten.Visible = True
            trPrueforganisation.Visible = True
            trArtGenehmigung.Visible = True
            trGutachtenNr.Visible = True
        Else
            trGutachten.Visible = False
            trPrueforganisation.Visible = False
            trArtGenehmigung.Visible = False
            trGutachtenNr.Visible = False
            txtGutachtenNr.Text = ""
        End If
    End Sub

#End Region

End Class
' ************************************************
' $History: Change02s.aspx.vb $
' 
' *****************  Version 53  *****************
' User: Fassbenders  Date: 12.05.11   Time: 15:02
' Updated in $/CKAG2/Services/Components/ComCommon/Beauftragung
' 
' *****************  Version 52  *****************
' User: Fassbenders  Date: 11.05.11   Time: 16:34
' Updated in $/CKAG2/Services/Components/ComCommon/Beauftragung
' 
' *****************  Version 51  *****************
' User: Fassbenders  Date: 11.05.11   Time: 13:12
' Updated in $/CKAG2/Services/Components/ComCommon/Beauftragung
' 
' *****************  Version 50  *****************
' User: Fassbenders  Date: 10.05.11   Time: 17:12
' Updated in $/CKAG2/Services/Components/ComCommon/Beauftragung
' 
' *****************  Version 49  *****************
' User: Fassbenders  Date: 9.05.11    Time: 19:25
' Updated in $/CKAG2/Services/Components/ComCommon/Beauftragung
' 
' *****************  Version 48  *****************
' User: Fassbenders  Date: 6.05.11    Time: 14:12
' Updated in $/CKAG2/Services/Components/ComCommon/Beauftragung
' 
' *****************  Version 47  *****************
' User: Dittbernerc  Date: 5.05.11    Time: 15:24
' Updated in $/CKAG2/Services/Components/ComCommon/Beauftragung
' 
' *****************  Version 46  *****************
' User: Dittbernerc  Date: 21.04.11   Time: 11:36
' Updated in $/CKAG2/Services/Components/ComCommon/Beauftragung
' 
' *****************  Version 45  *****************
' User: Fassbenders  Date: 12.04.11   Time: 11:32
' Updated in $/CKAG2/Services/Components/ComCommon/Beauftragung
' 
' *****************  Version 44  *****************
' User: Dittbernerc  Date: 8.04.11    Time: 14:45
' Updated in $/CKAG2/Services/Components/ComCommon/Beauftragung
' 
' *****************  Version 43  *****************
' User: Fassbenders  Date: 24.02.11   Time: 9:53
' Updated in $/CKAG2/Services/Components/ComCommon/Beauftragung
' 
' *****************  Version 42  *****************
' User: Fassbenders  Date: 17.02.11   Time: 13:25
' Updated in $/CKAG2/Services/Components/ComCommon/Beauftragung
' 
' *****************  Version 41  *****************
' User: Fassbenders  Date: 15.02.11   Time: 17:34
' Updated in $/CKAG2/Services/Components/ComCommon/Beauftragung
' 
' *****************  Version 40  *****************
' User: Fassbenders  Date: 15.02.11   Time: 10:00
' Updated in $/CKAG2/Services/Components/ComCommon/Beauftragung
' 
' *****************  Version 39  *****************
' User: Fassbenders  Date: 31.01.11   Time: 15:15
' Updated in $/CKAG2/Services/Components/ComCommon/Beauftragung
' 
' *****************  Version 38  *****************
' User: Fassbenders  Date: 31.01.11   Time: 8:53
' Updated in $/CKAG2/Services/Components/ComCommon/Beauftragung
' 
' *****************  Version 37  *****************
' User: Fassbenders  Date: 19.01.11   Time: 13:39
' Updated in $/CKAG2/Services/Components/ComCommon/Beauftragung
' 
' *****************  Version 36  *****************
' User: Fassbenders  Date: 12.01.11   Time: 14:50
' Updated in $/CKAG2/Services/Components/ComCommon/Beauftragung
' 
' *****************  Version 35  *****************
' User: Fassbenders  Date: 10.01.11   Time: 15:07
' Updated in $/CKAG2/Services/Components/ComCommon/Beauftragung
' 
' *****************  Version 34  *****************
' User: Fassbenders  Date: 21.12.10   Time: 13:51
' Updated in $/CKAG2/Services/Components/ComCommon/Beauftragung
' 
' *****************  Version 33  *****************
' User: Fassbenders  Date: 25.10.10   Time: 9:50
' Updated in $/CKAG2/Services/Components/ComCommon/Beauftragung
' 
' *****************  Version 32  *****************
' User: Fassbenders  Date: 4.10.10    Time: 15:08
' Updated in $/CKAG2/Services/Components/ComCommon/Beauftragung
' 
' *****************  Version 31  *****************
' User: Fassbenders  Date: 10.06.10   Time: 15:04
' Updated in $/CKAG2/Services/Components/ComCommon/Beauftragung
' 
' *****************  Version 30  *****************
' User: Fassbenders  Date: 4.05.10    Time: 16:58
' Updated in $/CKAG2/Services/Components/ComCommon/Beauftragung
' 
' *****************  Version 29  *****************
' User: Fassbenders  Date: 4.05.10    Time: 16:07
' Updated in $/CKAG2/Services/Components/ComCommon/Beauftragung
' 
' *****************  Version 28  *****************
' User: Fassbenders  Date: 4.05.10    Time: 10:31
' Updated in $/CKAG2/Services/Components/ComCommon/Beauftragung
' 
' *****************  Version 27  *****************
' User: Fassbenders  Date: 3.05.10    Time: 15:42
' Updated in $/CKAG2/Services/Components/ComCommon/Beauftragung
' 
' *****************  Version 26  *****************
' User: Fassbenders  Date: 5.03.10    Time: 13:34
' Updated in $/CKAG2/Services/Components/ComCommon/Beauftragung
' 
' *****************  Version 25  *****************
' User: Fassbenders  Date: 1.03.10    Time: 17:14
' Updated in $/CKAG2/Services/Components/ComCommon/Beauftragung
' 
' *****************  Version 24  *****************
' User: Fassbenders  Date: 1.03.10    Time: 16:27
' Updated in $/CKAG2/Services/Components/ComCommon/Beauftragung
' 
' *****************  Version 23  *****************
' User: Fassbenders  Date: 28.02.10   Time: 19:49
' Updated in $/CKAG2/Services/Components/ComCommon/Beauftragung
' 
' *****************  Version 22  *****************
' User: Fassbenders  Date: 26.02.10   Time: 16:45
' Updated in $/CKAG2/Services/Components/ComCommon/Beauftragung
' 
' *****************  Version 21  *****************
' User: Fassbenders  Date: 26.02.10   Time: 14:43
' Updated in $/CKAG2/Services/Components/ComCommon/Beauftragung
' 
' *****************  Version 20  *****************
' User: Fassbenders  Date: 26.02.10   Time: 13:39
' Updated in $/CKAG2/Services/Components/ComCommon/Beauftragung
' 
' *****************  Version 19  *****************
' User: Fassbenders  Date: 23.02.10   Time: 18:46
' Updated in $/CKAG2/Services/Components/ComCommon/Beauftragung
' 
' *****************  Version 18  *****************
' User: Fassbenders  Date: 19.02.10   Time: 17:01
' Updated in $/CKAG2/Services/Components/ComCommon/Beauftragung
' 
' *****************  Version 17  *****************
' User: Fassbenders  Date: 18.02.10   Time: 10:47
' Updated in $/CKAG2/Services/Components/ComCommon/Beauftragung
' 
' *****************  Version 16  *****************
' User: Fassbenders  Date: 17.02.10   Time: 16:30
' Updated in $/CKAG2/Services/Components/ComCommon/Beauftragung
' 
' *****************  Version 15  *****************
' User: Fassbenders  Date: 17.02.10   Time: 15:24
' Updated in $/CKAG2/Services/Components/ComCommon/Beauftragung
' 
' *****************  Version 14  *****************
' User: Fassbenders  Date: 17.02.10   Time: 13:12
' Updated in $/CKAG2/Services/Components/ComCommon/Beauftragung
' 
' *****************  Version 13  *****************
' User: Fassbenders  Date: 17.02.10   Time: 10:19
' Updated in $/CKAG2/Services/Components/ComCommon/Beauftragung
' 
' *****************  Version 12  *****************
' User: Martinp      Date: 16.02.10   Time: 16:07
' Updated in $/CKAG2/Services/Components/ComCommon/Beauftragung
' 
' *****************  Version 11  *****************
' User: Fassbenders  Date: 16.02.10   Time: 12:58
' Updated in $/CKAG2/Services/Components/ComCommon/Beauftragung
' 
' *****************  Version 10  *****************
' User: Fassbenders  Date: 16.02.10   Time: 8:31
' Updated in $/CKAG2/Services/Components/ComCommon/Beauftragung
' 
' *****************  Version 9  *****************
' User: Fassbenders  Date: 4.02.10    Time: 11:56
' Updated in $/CKAG2/Services/Components/ComCommon/Beauftragung
' 
' *****************  Version 8  *****************
' User: Fassbenders  Date: 3.02.10    Time: 19:33
' Updated in $/CKAG2/Services/Components/ComCommon/Beauftragung
' 
' *****************  Version 7  *****************
' User: Fassbenders  Date: 2.02.10    Time: 17:21
' Updated in $/CKAG2/Services/Components/ComCommon/Beauftragung
' 
' *****************  Version 6  *****************
' User: Fassbenders  Date: 14.12.09   Time: 11:01
' Updated in $/CKAG2/Services/Components/ComCommon/Beauftragung
' 
' *****************  Version 5  *****************
' User: Fassbenders  Date: 9.12.09    Time: 17:37
' Updated in $/CKAG2/Services/Components/ComCommon/Beauftragung
' 
' *****************  Version 4  *****************
' User: Fassbenders  Date: 7.12.09    Time: 12:42
' Updated in $/CKAG2/Services/Components/ComCommon/Beauftragung
' 
' *****************  Version 3  *****************
' User: Fassbenders  Date: 2.12.09    Time: 17:36
' Updated in $/CKAG2/Services/Components/ComCommon/Beauftragung
' 
' *****************  Version 2  *****************
' User: Fassbenders  Date: 2.12.09    Time: 14:21
' Updated in $/CKAG2/Services/Components/ComCommon/Beauftragung
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 1.12.09    Time: 11:27
' Created in $/CKAG2/Services/Components/ComCommon/Beauftragung
' ITA: 3264
' 
