Imports CKG.Base.Kernel.Common.Common
Imports CKG.Base.Kernel.Security
Imports CKG.Base.Kernel

Namespace Beauftragung2

    Partial Public Class Change01
        Inherits System.Web.UI.Page

#Region "Declarations"
        Private m_App As App
        Private m_User As User
        Private mBeauftragung As Beauftragung2

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

            Try
                lblHead.Text = m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString

                m_App = New App(m_User)

                lblError.Text = ""


                If Not IsPostBack Then

                    Session("Kunden") = Nothing
                    Session("Kreise") = Nothing
                    Session("ActiveTab") = Nothing
                    Session.Add("ActiveTab", 0)

                    InitDropdown()

                End If

                InitControls()

            Catch ex As Exception
                lblError.Text = "Beim Laden der Seite ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
            End Try

        End Sub

        Protected Sub lbCreate_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lbCreate.Click

            Dim booError As Boolean = False

            If ddlDienstleistung.SelectedItem.Value = "" Then
                ddlDienstleistung.BorderColor = Drawing.Color.Red
                lblDienstleisungInfo.Text = "Bitte wählen Sie eine Dienstleistung aus."
                booError = True
            End If

            If txtEVB.Text.Length = 0 Then
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

        Protected Sub rdbKunde_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles rdbKunde.SelectedIndexChanged

            trGrossKunde.Visible = Not trGrossKunde.Visible
            divHalter.Visible = Not divHalter.Visible

        End Sub

        Protected Sub txtKunde_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtKunde.TextChanged

            imgKunde.Visible = False

            Dim Kundenliste() As String = CType(Session("Kunden"), String())

            Dim i As Integer = Array.IndexOf(Kundenliste, txtKunde.Text)

            If i = -1 Then
                SetErrBehavior(txtKunde, lblKundeInfo, "Ungültige Kundenauswahl.")
            Else
                imgKunde.Visible = True
            End If

        End Sub

        Protected Sub txtGrosskundennummer_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtGrosskundennummer.TextChanged
            imgGrossKunde.Visible = False

            If txtGrosskundennummer.Text.Length = 0 Then
                SetErrBehavior(txtGrosskundennummer, lblGrosskundeInfo, "Bitte geben Sie eine Großkundennummer ein.")
                Return
            End If

            mBeauftragung = New Beauftragung2(m_User, m_App, Session("AppID").ToString, Session.SessionID.ToString, "")

            Dim Grosskunde As String = mBeauftragung.CheckGrosskundennummer("11000", txtGrosskundennummer.Text, Page)

            lblKundeShow.Text = Grosskunde

            If Grosskunde.Length = 0 Then
                SetErrBehavior(txtGrosskundennummer, lblGrosskundeInfo, "Keine Daten zur Großkundennummer gefunden.")
                txtGrosskundennummer.Focus()
            Else
                imgGrossKunde.Visible = True
            End If

        End Sub

        Protected Sub txtFinPruef_TextChanged(ByVal sender As Object, ByVal e As EventArgs) Handles txtFinPruef.TextChanged

            imgFahrgestellnummer.Visible = False


            Dim Fahrgestellnummer As String = txtFahrgestellnummer.Text

            If Fahrgestellnummer.Length < 17 OrElse txtFinPruef.Text.Length = 0 Then
                SetErrBehavior(txtFahrgestellnummer, lblPruefzifferInfo, "Fahrgestellnummer nicht 17-stellig oder Prüfziffer fehlt.")
                Return
            End If


            mBeauftragung = New Beauftragung2(m_User, m_App, Session("AppID").ToString, Session.SessionID.ToString, "")

            Dim Status As Integer = 0

            Status = mBeauftragung.CheckVin(Fahrgestellnummer.ToUpper, txtFinPruef.Text, Me)

            Select Case Status
                Case 1
                    SetErrBehavior(txtFinPruef, lblPruefzifferInfo, "Prüfziffer nicht korrekt.")
                Case 2
                    SetErrBehavior(txtFahrgestellnummer, lblPruefzifferInfo, "Fahrgestellnummer enthält unzulässige Zeichen.")
                Case Else
                    imgFahrgestellnummer.Visible = True
            End Select


        End Sub

        Protected Sub txtTypPruef_TextChanged(ByVal sender As Object, ByVal e As EventArgs) Handles txtTypPruef.TextChanged

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
            End If

            If booError = True Then Return


            mBeauftragung = New Beauftragung2(m_User, m_App, Session("AppID").ToString, Session.SessionID.ToString, "")

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


        End Sub


        Private Sub txtKreise_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtKreise.TextChanged
            imgKreise.Visible = False

            Dim Kreise() As String = CType(Session("Kreise"), String())

            Dim i As Integer = Array.IndexOf(Kreise, txtKreise.Text)

            If i = -1 Then
                SetErrBehavior(txtKreise, lblKreiseInfo, "Ungültiger Kreis.")
            Else
                txtKennz1.Text = Left(txtKreise.Text, txtKreise.Text.IndexOf("."))
                imgKreise.Visible = True
            End If

            txtEVB.Focus()

        End Sub

        Private Sub TabCon_ActiveTabChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles TabCon.ActiveTabChanged

            'User kommt aus Grunddaten-Tab
            If CType(Session("ActiveTab"), Integer) = 0 Then
                If ValidateGrunddaten() = True Then
                    'Fehler: Tab darf nicht verlassen werden
                    TabCon.ActiveTabIndex = CType(Session("ActiveTab"), Integer)
                Else
                    'User will auf den Fahrzeugdaten-Tab
                    If TabCon.ActiveTabIndex = 1 Then
                        Session("ActiveTab") = TabCon.ActiveTabIndex
                    End If

                    'User will auf den Tab Dienstleistungen
                    If TabCon.ActiveTabIndex = 2 Then
                        'Prüfen, ob die Fahrzeugdaten bereits ausgefüllt wurden
                        If ValidateFahrzeugdaten(Source.Part) = True Then
                            TabCon.ActiveTabIndex = 1
                        Else
                            Session("ActiveTab") = TabCon.ActiveTabIndex
                        End If

                    End If

                End If
            End If

            'User kommt aus Fahrzeugdaten-Tab
            If CType(Session("ActiveTab"), Integer) = 1 Then
                'User will auf den Tab Dienstleistungen
                If TabCon.ActiveTabIndex = 2 Then
                    'Prüfen, ob die Fahrzeugdaten bereits ausgefüllt wurden
                    If ValidateFahrzeugdaten(Source.Full) = True Then
                        TabCon.ActiveTabIndex = 1
                    Else
                        Session("ActiveTab") = TabCon.ActiveTabIndex
                    End If

                End If
            End If

            'User kommt aus Dienstleistungstab
            If TabCon.ActiveTabIndex = 2 Then
                Session("ActiveTab") = TabCon.ActiveTabIndex
            End If

        End Sub

        Protected Sub lbBack_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lbBack.Click
            Response.Redirect("../../../Start/Selection.aspx?AppID=" & Session("AppID").ToString, False)
        End Sub

        Protected Sub Button1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Button1.Click
            ModalPopupExtender2.Show()
        End Sub

        Private Sub btnOK_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnOK.Click

            lblSaveInfo.Visible = False

            If btnOK.Text = "Schließen" Then

                Response.Redirect("Change01.aspx?AppID=" & Session("AppID").ToString, False)
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

            mBeauftragung = New Beauftragung2(m_User, m_App, Session("AppID").ToString, Session.SessionID.ToString, "")

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
                .Kundennr = Mid(txtKunde.Text, txtKunde.Text.IndexOf(" ~ ") + 4, 6)


                If rdbKunde.SelectedItem.Value = "1" Then
                    .Grosskundennr = txtGrosskundennummer.Text
                Else
                    .HalterAnrede = ddlAnrede.SelectedItem.Value
                    .Haltername1 = txtName.Text
                    .Haltername2 = txtName2.Text
                    .HalterStrasse = txtStrasse.Text
                    .HalterHausnr = txtHausnummer.Text
                    .HalterPLZ = txtPLZ.Text
                    .HalterOrt = txtOrt.Text
                End If

                .VerkKuerz = txtVerkaeuferkuerzel.Text
                .KiReferenz = txtKundenreferenz.Text
                .Notiz = txtNotiz.Text

                'Fahrzeugdaten
                .Hersteller = txtHersteller.Text
                .Typ = txtTyp.Text
                .VarianteVersion = txtVarianteVersion.Text
                .TypPruef = txtTypPruef.Text

                'Dienstleistung
                .StVA = txtKreise.Text
                .StVANr = Left(txtKreise.Text, txtKreise.Text.IndexOf("."))
                .Kennzeichen = txtKennz1.Text.ToUpper & "-" & txtKennz2.Text
                .EVB = txtEVB.Text
                .Zulassungsdatum = txtZulDatum.Text
                .Bemerkung = txtDienstBemerkung.Text

                .Einzelkennzeichen = cbxEinKennz.Checked
                .Krad = cbxKrad.Checked
                .KennzeichenTyp = ddlKennzTyp.SelectedItem.Value
                .Feinstaubplakette = cbxFeinstaub.Checked
                .Wunschkennzeichen = cbxWunschkennzFlag.Checked
                .Reserviert = cbxReserviert.Checked
                .Reservierungsnr = txtReservNr.Text

                .Referenz = txtBarcode.Text


            End With


            If mBeauftragung.Save(Session("AppID").ToString, Session.SessionID.ToString, Me.Page) = True Then
                lblSaveInfo.Text = "Ihr Auftrag wurde erfolgreich übermittelt."
                lblSaveInfo.ForeColor = Drawing.Color.Blue
                lblSaveInfo.Visible = True
                btnOK.Text = "Schließen"
                btnCancel.Width = 0
                ModalPopupExtender2.Show()
            Else
                lblSaveInfo.Text = "Ihr Auftrag konnte nicht gespeichert werden."
                lblSaveInfo.ForeColor = Drawing.Color.Red
                lblSaveInfo.Visible = True
                ModalPopupExtender2.Show()
            End If




        End Sub



        Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
            SetEndASPXAccess(Me)
        End Sub

        Private Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Unload
            SetEndASPXAccess(Me)
        End Sub


#End Region

#Region "Methods"

        Private Sub InitDropdown()

            txtZulDatum.Text = Date.Today.ToShortDateString

            mBeauftragung = New Beauftragung2(m_User, m_App, Session("AppID").ToString, Session.SessionID.ToString, "")

            With mBeauftragung
                .Gruppe = m_User.Groups(0).GroupName
                .Verkaufsbuero = Right(m_User.Reference, 4)
                .Verkaufsorganisation = Left(m_User.Reference, 4)
            End With


            mBeauftragung.Fill(Session("AppID").ToString, Session.SessionID.ToString, Me)


            Dim NewRow As DataRow

            NewRow = mBeauftragung.Dienstleistungen.NewRow()
            NewRow("MAKTX") = String.Empty
            NewRow("MATNR") = String.Empty
            mBeauftragung.Dienstleistungen.Rows.Add(NewRow)
            mBeauftragung.Dienstleistungen.AcceptChanges()

            Dim dv As DataView

            dv = mBeauftragung.Dienstleistungen.DefaultView
            dv.Sort = "MAKTX"
            With ddlDienstleistung
                .DataSource = dv
                .DataTextField = "MAKTX"
                .DataValueField = "MATNR"
                .DataBind()
            End With

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

            'Anrede füllen
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

            If rdbKunde.SelectedItem.Value = "1" Then

                If txtGrosskundennummer.Text.Length = 0 Then
                    SetErrBehavior(txtGrosskundennummer, lblGrosskundeInfo, "Grosskundennummer fehlt.")
                    booError = True
                Else
                    imgGrossKunde.Visible = True
                End If
            Else
                If txtName.Text.Length = 0 Then
                    SetErrBehavior(txtName, lblNameInfo, "Haltername fehlt.")
                    booError = True
                Else
                    imgHaltername.Visible = True
                End If

                If txtStrasse.Text.Length = 0 Then
                    SetErrBehavior(txtStrasse, lblStrasseInfo, "Strasse fehlt.")
                    booError = True
                Else
                    imgHalterstrasse.Visible = True
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


            Return booError

        End Function

        Private Function ValidateFahrzeugdaten(ByVal Base As Integer) As Boolean
            Dim booError As Boolean = False



            Select Case Base
                Case Source.Part
                    If imgFahrgestellnummer.Visible = False Then booError = True
                    If imgTyp.Visible = False AndAlso imgTypInfo.Visible = False Then booError = True
                Case Source.Full
                    imgHersteller.Visible = False
                    imgTyp.Visible = False
                    imgTypInfo.Visible = False
                    imgFahrgestellnummer.Visible = False

                    If txtFahrgestellnummer.Text.Length = 0 Then
                        SetErrBehavior(txtFahrgestellnummer, lblPruefzifferInfo, "")
                        booError = True
                    End If

                    If txtFinPruef.Text.Length = 0 Then
                        SetErrBehavior(txtFinPruef, lblPruefzifferInfo, "")
                        booError = True
                    End If

                    If txtFahrgestellnummer.Text.Length = 0 OrElse txtFinPruef.Text.Length = 0 Then
                        lblPruefzifferInfo.Text = "Fahrgestellnummer/Prüfziffer nicht korrekt gefüllt."
                    End If



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


        Private Sub InitControls()

            txtKunde.BorderColor = Drawing.Color.Empty
            lblKundeInfo.Text = ""

            txtGrosskundennummer.BorderColor = Drawing.Color.Empty
            lblGrosskundeInfo.Text = ""

            txtName.BorderColor = Drawing.Color.Empty
            lblNameInfo.Text = ""

            txtStrasse.BorderColor = Drawing.Color.Empty
            lblStrasseInfo.Text = ""

            txtPLZ.BorderColor = Drawing.Color.Empty
            txtOrt.BorderColor = Drawing.Color.Empty
            lblOrtInfo.Text = ""

            txtFahrgestellnummer.BorderColor = Drawing.Color.Empty
            txtFinPruef.BorderColor = Drawing.Color.Empty
            lblPruefzifferInfo.Text = ""

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

            lblSaveInfo.Text = ""

        End Sub

        Private Sub SetErrBehavior(ByVal txtcontrol As TextBox, ByVal lblControl As Label, ByVal ErrText As String)

            txtcontrol.BorderColor = Drawing.Color.Red

            lblControl.Text = ErrText

        End Sub

#End Region

        Private Sub TabCon_DataBinding(ByVal sender As Object, ByVal e As System.EventArgs) Handles TabCon.DataBinding

        End Sub

        Private Sub TabCon_Disposed(ByVal sender As Object, ByVal e As System.EventArgs) Handles TabCon.Disposed

        End Sub

        Private Sub TabCon_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles TabCon.Init
            TabCon.ActiveTabIndex = 0
        End Sub

        Private Sub TabCon_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles TabCon.Load

        End Sub




        Private Sub TabCon_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles TabCon.PreRender

        End Sub

        Private Sub TabCon_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles TabCon.Unload

        End Sub
    End Class

End Namespace
' ************************************************
' $History: Change01.aspx.vb $
' 
' *****************  Version 10  *****************
' User: Fassbenders  Date: 26.11.09   Time: 14:44
' Updated in $/CKAG2/Services/Components/ComCommon/Beauftragung
' 
' *****************  Version 9  *****************
' User: Fassbenders  Date: 25.11.09   Time: 16:24
' Updated in $/CKAG2/Services/Components/ComCommon/Beauftragung
' 
' *****************  Version 8  *****************
' User: Fassbenders  Date: 20.11.09   Time: 16:45
' Updated in $/CKAG2/Services/Components/ComCommon/Beauftragung
' 
' *****************  Version 7  *****************
' User: Fassbenders  Date: 20.11.09   Time: 14:32
' Updated in $/CKAG2/Services/Components/ComCommon/Beauftragung
' 
' *****************  Version 6  *****************
' User: Fassbenders  Date: 18.11.09   Time: 9:00
' Updated in $/CKAG2/Services/Components/ComCommon/Beauftragung
' 
' *****************  Version 5  *****************
' User: Fassbenders  Date: 17.11.09   Time: 17:56
' Updated in $/CKAG2/Services/Components/ComCommon/Beauftragung
' 
' *****************  Version 4  *****************
' User: Fassbenders  Date: 17.11.09   Time: 8:35
' Updated in $/CKAG2/Services/Components/ComCommon/Beauftragung
' 
' *****************  Version 3  *****************
' User: Fassbenders  Date: 16.11.09   Time: 14:55
' Updated in $/CKAG2/Services/Components/ComCommon/Beauftragung
' 
' *****************  Version 2  *****************
' User: Fassbenders  Date: 13.11.09   Time: 15:52
' Updated in $/CKAG2/Services/Components/ComCommon/Beauftragung
' 