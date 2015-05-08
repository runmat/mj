Imports CKG.Base.Kernel.Common.Common
Imports CKG.Base.Kernel.Security
Imports GeneralTools.Services

Namespace Beauftragung2

    Partial Public Class Change02s
        Inherits Page

#Region "Declarations"

        Private m_App As App
        Private m_User As User
        Private mBeauftragung As Beauftragung2
        Private mNextTab As NextTab

        Private Enum Source As Integer
            Part = 0
            Full = 1
            FullWithoutType = 2
        End Enum

        Private Enum NextTab As Integer
            KeineAuswahl
            Halterdaten
            Typdaten
            Dienstleistung
            Zusatzdienstleistungen
            Zusammenfassung
            AfterNpa
        End Enum

#End Region

#Region "Events"

        Protected Sub Page_Init(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Init
            Try
                m_User = GetUser(Me)
                FormAuth(Me, m_User)

                'Altdaten löschen bei Applikationswechsel
                If CStr(Session("AppID")) <> CStr(Page.Request.QueryString("AppID")) Then
                    Session("mBeauftragung2") = Nothing
                    Session("AfterNPAUse") = Nothing
                    Session("DienstleistungChanged") = Nothing
                    Session("NextTab") = Nothing
                End If

                GetAppIDFromQueryString(Me)
                m_App = New App(m_User)

                lblHead.Text = m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString
                lblError.Text = ""

                If Session("mBeauftragung2") IsNot Nothing Then
                    mBeauftragung = CType(Session("mBeauftragung2"), Beauftragung2)
                Else
                    mBeauftragung = New Beauftragung2(m_User, m_App, Session("AppID").ToString, Session.SessionID.ToString, "")
                    FillBeauftragung()
                    Session("mBeauftragung2") = mBeauftragung
                End If

                InitLargeDropdowns()
                InitJava()

            Catch ex As Exception
                lblError.Text = "Beim Initialisieren der Seite ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
            End Try
        End Sub

        Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
            Try
                InitControls()

                If Not IsPostBack Then
                    InitSmallDropdowns()
                    InitZusatzDLCheckboxlist()
                    RestoreSelectedValues()

                    If m_User.Reference.Length <> 8 Then
                        lblError.Text = "Kundenreferenz nicht korrekt."
                        Grunddaten.Disabled = True
                        lbtGrunddaten.Enabled = False
                        lbtFahrzeugdaten.Enabled = False
                        lbtDienstleistung.Enabled = False
                        lbtZusatzdienstleistungen.Enabled = False
                        lbtZusammenfassung.Enabled = False
                    End If

                    If Not Request.QueryString.Item("eIdentityResponse") Is Nothing Then
                        'Test Ansichtssteuerung NPA
                        Session("AfterNPAUse") = True

                        SetControlsNpa()

                        'Nach NPA Info-Label wieder füllen
                        InfoLabelFuellen()

                        tblBasisdaten.Style.Item("display") = "none"

                        Exit Sub
                    End If
                Else
                    If Page.Request.Params("__EVENTTARGET").Contains("_ddl") Then
                        Dim teile As String() = Page.Request.Params("__EVENTTARGET").Split("_"c)
                        Dim ddlName As String = teile(teile.Length - 1)

                        Select Case ddlName
                            Case "ddlKunde"
                                ApplyNewKunde()
                            Case "ddlStva"
                                ApplyNewStva()
                            Case "ddlDienstleistung"
                                ApplyNewDienstleistung()
                            Case "ddlGrosskunde"
                                ApplyNewGrosskunde()
                        End Select
                    End If
                End If

            Catch ex As Exception
                lblError.Text = "Beim Laden der Seite ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
            End Try
        End Sub

        Protected Sub lbtAcceptBasisdaten_Click(sender As Object, e As EventArgs) Handles lbtAcceptBasisdaten.Click
            If ddlKunde.SelectedValue = "0" Then
                lblKundeInfo.Text = "Kein gültiger Kunde gewählt!"
                Return
            End If

            If ddlDienstleistung.SelectedValue <> "0" Then
                ' Neuer Kunde = hide Formular
                lbtGrunddaten.Visible = False
                lbtFahrzeugdaten.Visible = False
                lbtDienstleistung.Visible = False
                lbtZusatzdienstleistungen.Visible = False
                lbtZusammenfassung.Visible = False

                Grunddaten.Visible = False
                Fahrzeugdaten.Visible = False
                Dienstleistung.Visible = False
                Zusatzdienstleistungen.Visible = False
                Zusammenfassung.Visible = False

                'Ansichtsteuerung NPA
                If CBool(Session("DienstleistungChanged")) = True And CBool(Session("AfterNPAUse")) = True Then
                    'Sperre der Eingabefelder nach NPA auslesen aufheben
                    Session("AfterNPAUse") = False
                End If

                Session("DienstleistungChanged") = False

                If Not CBool(Session("AfterNPAUse")) = True Then
                    ' Tab clearen bei geändertem Kunden, 
                    ' damit keine alten Daten mitgenommen werden

                    ClearHalter()
                    ClearTypdaten()
                    ClearDienstleistung()
                End If

                mNextTab = NextTab.Halterdaten
                SelectNextTab()

                lbtGrunddaten.Visible = True
                lbtFahrzeugdaten.Visible = True
                lbtDienstleistung.Visible = True
                lbtZusatzdienstleistungen.Visible = True
                lbtZusammenfassung.Visible = True

                ' Einmalig benötigte DialogControls laden 
                PrepareDienstleistungControls()

                If ddlStva.SelectedValue <> "0" Then
                    'Grosskundenliste füllen
                    Dim kbanr As String = mBeauftragung.Kreise.Select("ZKFZKZ='" & ddlStva.SelectedValue & "'")(0)("ZKBA1").ToString()
                    mBeauftragung.FillGrosskunden(kbanr, Me)
                    Session("mBeauftragung2") = mBeauftragung
                    InitGrosskundenDropdown()
                End If

                InfoLabelFuellen()

                tblBasisdaten.Style.Item("display") = "none"

                'ggf. Daten vom Autohaus-Vorgang übernehmen
                If mBeauftragung.Autohausvorgang Then
                    RestoreAutohausVorgang()
                Else
                    If mBeauftragung.MaterialnummerAlt = ddlDienstleistung.SelectedValue AndAlso mBeauftragung.StVANrAlt = ddlStva.SelectedValue AndAlso mBeauftragung.HalterMerken Then
                        'Wenn Dienstleistung und Amt unverändert, ggf. Halterdaten merken/wiederherstellen
                        RestoreHalterdaten()
                    Else
                        'Bei geänderter Dienstleistung bzw. Amt keine Halterdaten merken/wiederherstellen
                        ResetBeauftragungHalterdaten()
                    End If

                    'Specials für Düren
                    If ddlStva.SelectedValue = "DN" Then
                        Dim nextZulDat As DateTime = DateTime.Today.AddDays(1)
                        While nextZulDat.DayOfWeek = DayOfWeek.Saturday OrElse nextZulDat.DayOfWeek = DayOfWeek.Sunday OrElse DateService.IstFeiertag(nextZulDat)
                            nextZulDat = nextZulDat.AddDays(1)
                        End While
                        txtZulDatum.Text = nextZulDat.ToShortDateString()

                        txtKennz1.Text = ""
                    End If
                End If

                mBeauftragung.MaterialnummerAlt = mBeauftragung.Materialnummer
                mBeauftragung.StVANrAlt = mBeauftragung.StVANr
            Else
                lblDienstleisungInfo.Text = "Keine gültige Dienstleistung gewählt!"
            End If
        End Sub

        Private Sub lbtGrunddaten_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lbtGrunddaten.Click
            ChangeTab(NextTab.Halterdaten)
        End Sub

        Private Sub lbtFahrzeugdaten_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lbtFahrzeugdaten.Click
            ChangeTab(NextTab.Typdaten)
        End Sub

        Private Sub lbtDienstleistung_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lbtDienstleistung.Click
            ChangeTab(NextTab.Dienstleistung)
        End Sub

        Private Sub lbtZusatzdienstleistungen_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lbtZusatzdienstleistungen.Click
            ChangeTab(NextTab.Zusatzdienstleistungen)
        End Sub

        Private Sub lbtZusammenfassung_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lbtZusammenfassung.Click
            'Vor Anzeige der Zusammenfassung Daten sichern (2. Parameter = True)
            ChangeTab(NextTab.Zusammenfassung, True)
        End Sub

        Private Sub btnDefault_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnDefault.Click

            If lbtGrunddaten.CssClass = "TabButtonActive" Then
                txtKunnr.Focus()
            ElseIf lbtFahrzeugdaten.CssClass = "TabButtonActive" Then
                txtHersteller.Focus()
            ElseIf lbtDienstleistung.CssClass = "TabButtonActive" Then
                ddlDienstleistung.Focus()
            ElseIf lbtZusatzdienstleistungen.CssClass = "TabButtonActive" Then
                'TODO
            ElseIf lbtZusammenfassung.CssClass = "TabButtonActive" Then
                'TODO
            End If

        End Sub

        Private Sub rblHalterauswahl_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles rblHalterauswahl.SelectedIndexChanged
            Select Case rblHalterauswahl.SelectedValue

                Case "Grosskunde"
                    ClearHalter()
                    divHalter.Visible = False
                    'divNpa.Visible = False
                    trGrossKunde.Visible = (mBeauftragung.HalterNeeded <> HalterErfOptionen.Nein)
                    txtGrosskundennummer.Focus()

                Case "Halter"
                    trGrossKunde.Visible = False
                    txtGrosskundennummer.Text = ""
                    divHalter.Visible = True
                    trAnrede.Visible = (mBeauftragung.HalterNeeded <> HalterErfOptionen.Nein And mBeauftragung.HalterNeeded <> HalterErfOptionen.JaNurName1)
                    trName.Visible = (mBeauftragung.HalterNeeded <> HalterErfOptionen.Nein)
                    trName2.Visible = (mBeauftragung.HalterNeeded <> HalterErfOptionen.Nein And mBeauftragung.HalterNeeded <> HalterErfOptionen.JaNurName1)
                    trGeburtsort.Visible = (mBeauftragung.HalterNeeded = HalterErfOptionen.Ja Or mBeauftragung.HalterNeeded = HalterErfOptionen.JaOhneGeburtsdatum)
                    trGeburtstag.Visible = (mBeauftragung.HalterNeeded = HalterErfOptionen.Ja Or mBeauftragung.HalterNeeded = HalterErfOptionen.JaOhneGeburtsort)
                    trStrasse.Visible = (mBeauftragung.HalterNeeded <> HalterErfOptionen.Nein And mBeauftragung.HalterNeeded <> HalterErfOptionen.JaNurName1)
                    trOrt.Visible = (mBeauftragung.HalterNeeded <> HalterErfOptionen.Nein And mBeauftragung.HalterNeeded <> HalterErfOptionen.JaNurName1)
                    txtName.Focus()

            End Select
        End Sub

        Protected Sub lbBack_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lbBack.Click
            If mBeauftragung.Autohausvorgang Then
                Response.Redirect("/Services/(S(" & Session.SessionID & "))/Components/ComCommon/Beauftragung2/Change02s_0.aspx?AppID=" & Session("AppID").ToString)
            Else
                Response.Redirect("../../../Start/Selection.aspx?AppID=" & Session("AppID").ToString, False)
            End If
        End Sub

        Protected Sub lbCreate_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lbCreate.Click

            If mBeauftragung.HalterNeeded <> HalterErfOptionen.Nein AndAlso ValidateGrunddaten() Then
                mNextTab = NextTab.Halterdaten
                SelectNextTab()
                Return
            End If

            If mBeauftragung.TypDatenNeeded = "N"c Then
                If ValidateFahrzeugdaten(Source.Part) Then
                    mNextTab = NextTab.Typdaten
                    SelectNextTab()
                    Return
                End If
            ElseIf mBeauftragung.TypDatenNeeded = "H"c Then
                If ValidateFahrzeugdaten(Source.FullWithoutType) Then
                    mNextTab = NextTab.Typdaten
                    SelectNextTab()
                    Return
                End If
            Else
                If ValidateFahrzeugdaten(Source.Full) Then
                    mNextTab = NextTab.Typdaten
                    SelectNextTab()
                    Return
                End If
            End If

            If ValidateDienstleistung() Then
                mNextTab = NextTab.Dienstleistung
                SelectNextTab()
                Return
            End If

            If mBeauftragung.BarcodeNeeded = "J"c Then
                ModalPopupExtender2.Show()
                FocusBarcode()
            Else
                CheckOkClick()
            End If

        End Sub

        Protected Sub txtFinPruef_TextChanged(ByVal sender As Object, ByVal e As EventArgs) Handles txtFinPruef.TextChanged

            imgFahrgestellnummer.Visible = False

            Dim Fahrgestellnummer As String = txtFahrgestellnummer.Text.ToUpper()

            If Fahrgestellnummer.Length < 17 OrElse txtFinPruef.Text.Length = 0 Then
                SetErrBehavior(txtFahrgestellnummer, lblPruefzifferInfo, "Fahrgestellnummer nicht 17-stellig oder Prüfziffer fehlt.")
                Return
            End If

            Dim Status As Integer = mBeauftragung.CheckVin(Fahrgestellnummer, txtFinPruef.Text, Me)

            Select Case Status
                Case 1
                    SetErrBehavior(txtFinPruef, lblPruefzifferInfo, "Prüfziffer nicht korrekt.")
                    txtFinPruef.Focus()
                Case 2
                    SetErrBehavior(txtFahrgestellnummer, lblPruefzifferInfo, "Fahrgestellnummer enthält unzulässige Zeichen.")
                    txtFinPruef.Focus()
                Case Else
                    imgFahrgestellnummer.Visible = True
                    If mBeauftragung.FarbeNeeded = "J" Then
                        ddlFarbe.Focus()
                    Else
                        txtBriefnummer.Focus()
                    End If
            End Select

        End Sub

        Private Sub txtBriefnummer_TextChanged(ByVal sender As Object, ByVal e As EventArgs) Handles txtBriefnummer.TextChanged
            If ValidateBriefnummer() = False Then
                imgBriefnummer.Visible = True
                If mBeauftragung.ZB1Needed = "J" Then
                    txtNummerZB1_1.Focus()
                End If
            End If

        End Sub

        Protected Sub txtTypPruef_TextChanged(ByVal sender As Object, ByVal e As EventArgs) Handles txtTypPruef.TextChanged

            imgTyp.Visible = False
            imgTypInfo.Visible = False
            imgHersteller.Visible = False
            If txtHersteller.Text.Length = 0 Then
                SetErrBehavior(txtHersteller, lblTypInfo, "Hersteller fehlt.")
                Return
            End If

            If txtTyp.Text.Length = 0 OrElse txtVarianteVersion.Text.Length = 0 OrElse txtTypPruef.Text.Length = 0 Then
                SetErrBehavior(txtHersteller, lblTypInfo, "Typdaten nicht vollständig.")
                Return
            End If

            Dim dt As DataTable

            dt = mBeauftragung.FillTypdaten(txtHersteller.Text.ToUpper(), txtTyp.Text.ToUpper(), txtVarianteVersion.Text.ToUpper(), txtTypPruef.Text.ToUpper(), Me)

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

            If mBeauftragung.FahrzeugdatenNeeded = "J" Then
                txtFahrzeugklasse.Focus()
            Else
                txtFahrgestellnummer.Focus()
            End If

        End Sub

        Private Sub btnOK_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnOK.Click
            CheckOkClick()
        End Sub

        Protected Sub ibtNpaLogo_Click(ByVal sender As Object, ByVal e As ImageClickEventArgs) Handles ibtNpaLogo.Click

            SetHalter()

            Session("mBeauftragung2") = mBeauftragung

            If Session("NpaAppID") Is Nothing Then
                Session.Add("NpaAppID", Page.Request.QueryString("AppID").ToString)
            Else
                Session("NpaAppID") = Page.Request.QueryString("AppID").ToString
            End If

            Dim url As String = ConfigurationManager.AppSettings("nPaURL")

            Response.Redirect(url, True)

        End Sub

        Private Sub lbtPDF_Click(ByVal sender As Object, ByVal e As ImageClickEventArgs) Handles lbtPDF.Click
            Dim selectPath As String
            If cbxEinzug.Checked = False Then
                selectPath = "\Components\ComCommon\documents\Zulassungsantrag.doc"
            Else
                selectPath = "\Components\ComCommon\documents\Zulassungsantrag2.doc"
            End If

            Dim tblData As DataTable = Base.Kernel.Common.DataTableHelper.ObjectToDataTable(mBeauftragung)
            Dim imageHt As New Hashtable()
            imageHt.Add("Logo", m_User.Customer.LogoImage)
            Dim docFactory As New Base.Kernel.DocumentGeneration.WordDocumentFactory(tblData, imageHt)
            docFactory.CreateDocument("Zulassungsantrag_" & m_User.UserName, Page, selectPath)
        End Sub

        Protected Sub ddlAnrede_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ddlAnrede.SelectedIndexChanged
            SetHalter()
        End Sub

        Private Sub Page_PreRender(ByVal sender As Object, ByVal e As EventArgs) Handles MyBase.PreRender
            SetEndASPXAccess(Me)
        End Sub

        Private Sub Page_Unload(ByVal sender As Object, ByVal e As EventArgs) Handles MyBase.Unload
            SetEndASPXAccess(Me)
        End Sub

        Private Sub ibtBasisdatenResize_Click(sender As Object, e As ImageClickEventArgs) Handles ibtBasisdatenResize.Click
            Dim strAlt As String = tblBasisdaten.Style.Item("display")
            tblBasisdaten.Style.Item("display") = IIf(strAlt = "none", "block", "none")
        End Sub

        Protected Sub txtPLZ_TextChanged(ByVal sender As Object, ByVal e As EventArgs) Handles txtPLZ.TextChanged
            If Not String.IsNullOrEmpty(txtPLZ.Text) AndAlso txtPLZ.Text.Length = 5 Then
                mBeauftragung.GetOrteZurPlz(txtPLZ.Text, Me)
                Session("mBeauftragung2") = mBeauftragung
                'Wenn Orte gefunden -> Dropdown, sonst Eingabetextfeld
                If mBeauftragung.OrteZurPlz.Rows.Count > 0 Then
                    txtOrt.Text = ""
                    txtOrt.Visible = False
                    ddlOrt.Visible = True
                    FillOrtDropdown()
                    ddlOrt.Focus()
                Else
                    txtOrt.Visible = True
                    ddlOrt.Visible = False
                    txtOrt.Focus()
                End If
            End If
        End Sub

        Private Sub txtNaechsteHU_TextChanged(sender As Object, e As EventArgs) Handles txtNaechsteHU.TextChanged
            mBeauftragung.NaechsteHU = txtNaechsteHU.Text
        End Sub

        Private Sub lbForwardGrundDat_Click(sender As Object, e As EventArgs) Handles lbForwardGrundDat.Click
            ChangeTab(NextTab.Typdaten)
        End Sub

        Private Sub lbForwardFzgDat_Click(sender As Object, e As EventArgs) Handles lbForwardFzgDat.Click
            ChangeTab(NextTab.Dienstleistung)
        End Sub

        Private Sub lbForwardDL_Click(sender As Object, e As EventArgs) Handles lbForwardDL.Click
            ChangeTab(NextTab.Zusatzdienstleistungen)
        End Sub

        Private Sub lbForwardZusatzDL_Click(sender As Object, e As EventArgs) Handles lbForwardZusatzDL.Click
            'Vor Anzeige der Zusammenfassung Daten sichern (2. Parameter = True)
            ChangeTab(NextTab.Zusammenfassung, True)
        End Sub

        Private Sub btnAgeWarningContinue_Click(sender As Object, e As EventArgs) Handles btnAgeWarningContinue.Click
            mBeauftragung.AgeConfirmationRequired = False
            Session("mBeauftragung2") = mBeauftragung
            If Session("NextTab") IsNot Nothing Then
                mNextTab = CType(Session("NextTab"), NextTab)
                Session("NextTab") = Nothing
            Else
                mNextTab = NextTab.Typdaten
            End If
            SelectNextTab()
        End Sub

        Private Sub btnVinWarningContinue_Click(sender As Object, e As EventArgs) Handles btnVinWarningContinue.Click
            mBeauftragung.VinConfirmationRequired = False
            Session("mBeauftragung2") = mBeauftragung
            If Session("NextTab") IsNot Nothing Then
                mNextTab = CType(Session("NextTab"), NextTab)
                Session("NextTab") = Nothing
            Else
                mNextTab = NextTab.Dienstleistung
            End If
            SelectNextTab()
        End Sub

#End Region

#Region "Methods"

        Private Sub ApplyNewKunde()
            imgKunde.Visible = ddlKunde.SelectedValue <> "0"
            mBeauftragung.Kundennr = ddlKunde.SelectedValue
            Session("mBeauftragung2") = mBeauftragung
        End Sub

        Private Sub ApplyNewStva()
            'Dienstleistungen initial füllen (abhängig vom gewählten Amt)
            FilterDienstleistungen()

            ' Kennzeichen Teil 1 vorbelegen für Großkunden
            txtKennz1.Text = ddlStva.SelectedValue
            If mBeauftragung.AltKennzeichenNeeded = "P"c Or mBeauftragung.AltKennzeichenNeeded = "O"c Then
                txtKennzAlt1.Text = txtKennz1.Text
            End If

            imgKreise.Visible = ddlStva.SelectedValue <> "0"
            mBeauftragung.StVANr = ddlStva.SelectedValue
            RefreshDienstleistungsdaten()
            Session("mBeauftragung2") = mBeauftragung
        End Sub

        Private Sub ApplyNewDienstleistung()

            Session("DienstleistungChanged") = True

            If ddlDienstleistung.SelectedValue <> "0" Then
                mBeauftragung.Materialnummer = ddlDienstleistung.SelectedValue
                RefreshDienstleistungsdaten()
                Session("mBeauftragung2") = mBeauftragung
            End If

            imgDienstleistung.Visible = ddlDienstleistung.SelectedValue <> "0"

        End Sub

        ''' <summary>
        ''' Zum gewählten Kunden und Amt ermitteln, ob Bank-/Halterdaten etc. erforderlich
        ''' </summary>
        ''' <remarks></remarks>
        Private Sub RefreshDienstleistungsdaten()
            If Not String.IsNullOrEmpty(mBeauftragung.Materialnummer) AndAlso Not String.IsNullOrEmpty(mBeauftragung.StVANr) Then
                Dim rows As DataRow() = mBeauftragung.Dienstleistungen.Select("MATNR='" & mBeauftragung.Materialnummer.PadLeft(18, "0"c) & "' AND AMT='" & mBeauftragung.StVANr & "'")
                If rows.Length > 0 Then
                    Dim dRow As DataRow = rows(0)
                    mBeauftragung.GutachtenNeeded = CChar(dRow("GUT_ERF"))
                    mBeauftragung.NaechsteHUNeeded = CChar(dRow("NHU_ERF"))
                    mBeauftragung.BankdatenNeeded = CChar(dRow("BANK_ERF"))
                    mBeauftragung.SEPA = (Not String.IsNullOrEmpty(dRow("SEPA").ToString()))
                    mBeauftragung.EvBNeeded = CChar(dRow("EVB_ERF"))
                    Select Case dRow("HALTER_ERF").ToString()
                        Case "J"
                            mBeauftragung.HalterNeeded = HalterErfOptionen.Ja
                            mBeauftragung.HalterMerken = False
                        Case "JM"
                            mBeauftragung.HalterNeeded = HalterErfOptionen.Ja
                            mBeauftragung.HalterMerken = True
                        Case "G"
                            mBeauftragung.HalterNeeded = HalterErfOptionen.JaOhneGeburtsort
                            mBeauftragung.HalterMerken = False
                        Case "M"
                            mBeauftragung.HalterNeeded = HalterErfOptionen.JaOhneGeburtsort
                            mBeauftragung.HalterMerken = True
                        Case "G1"
                            mBeauftragung.HalterNeeded = HalterErfOptionen.JaOhneGeburtsortUndGeburtsdatum
                            mBeauftragung.HalterMerken = False
                        Case "M1"
                            mBeauftragung.HalterNeeded = HalterErfOptionen.JaOhneGeburtsortUndGeburtsdatum
                            mBeauftragung.HalterMerken = True
                        Case "G2"
                            mBeauftragung.HalterNeeded = HalterErfOptionen.JaOhneGeburtsdatum
                            mBeauftragung.HalterMerken = False
                        Case "M2"
                            mBeauftragung.HalterNeeded = HalterErfOptionen.JaOhneGeburtsdatum
                            mBeauftragung.HalterMerken = True
                        Case "H1"
                            mBeauftragung.HalterNeeded = HalterErfOptionen.JaNurName1
                            mBeauftragung.HalterMerken = False
                        Case "H2"
                            mBeauftragung.HalterNeeded = HalterErfOptionen.JaNurName1
                            mBeauftragung.HalterMerken = True
                        Case Else
                            mBeauftragung.HalterNeeded = HalterErfOptionen.Nein
                            mBeauftragung.HalterMerken = False
                    End Select
                    mBeauftragung.TypDatenNeeded = CChar(dRow("TYPD_ERF"))
                    mBeauftragung.AltKennzeichenNeeded = CChar(dRow("ALTKENZ_ERF"))
                    mBeauftragung.BarcodeNeeded = CChar(dRow("BARCODE_ERF"))
                    mBeauftragung.AusdruckNeeded = CChar(dRow("AUSDRUCK"))
                    mBeauftragung.FahrzeugdatenNeeded = CChar(dRow("FAHRZ_ERF"))
                    mBeauftragung.FarbeNeeded = CChar(dRow("FARB_ERF"))
                    mBeauftragung.ZB1Needed = CChar(dRow("ZBINR_ERF"))
                End If
            End If      
        End Sub

        Private Sub ApplyNewGrosskunde()
            imgGrossKunde.Visible = ddlGrosskunde.SelectedValue <> "0"
            mBeauftragung.Grosskundennr = ddlGrosskunde.SelectedValue
            mBeauftragung.Grosskunde = ddlGrosskunde.SelectedItem.Text
            Session("mBeauftragung2") = mBeauftragung
        End Sub

        Private Sub FilterDienstleistungen()
            Dim gewaehlteDL As String = ddlDienstleistung.SelectedValue

            ddlDienstleistung.Items.Clear()
            ddlDienstleistung.Items.Add(New ListItem("- Keine Auswahl -", "0"))

            If ddlStva.SelectedValue <> "0" Then
                For Each dRow As DataRow In mBeauftragung.Dienstleistungen.Select("AMT='" & ddlStva.SelectedValue & "'")
                    ddlDienstleistung.Items.Add(New ListItem(dRow("MAKTX").ToString(), dRow("MATNR").ToString().TrimStart("0"c)))
                Next
            End If

            'ggf. vorherige Auswahl wiederherstellen
            If gewaehlteDL <> "0" Then
                Dim selDL As ListItem = ddlDienstleistung.Items.FindByValue(gewaehlteDL)
                If selDL IsNot Nothing Then
                    ddlDienstleistung.SelectedValue = selDL.Value
                End If
            End If
        End Sub

        Private Sub InfoLabelFuellen()
            lblHeadInfo.Text = ""

            If ddlKunde.SelectedValue <> "0" Then
                lblHeadInfo.Text = ddlKunde.SelectedValue
            End If
            If ddlStva.SelectedValue <> "0" Then
                If Not String.IsNullOrEmpty(lblHeadInfo.Text) Then
                    lblHeadInfo.Text &= " - " & ddlStva.SelectedItem.Text
                Else
                    lblHeadInfo.Text = ddlStva.SelectedItem.Text
                End If
            End If
            If ddlDienstleistung.SelectedValue <> "0" Then
                If Not String.IsNullOrEmpty(lblHeadInfo.Text) Then
                    lblHeadInfo.Text &= " - " & ddlDienstleistung.SelectedItem.Text
                Else
                    lblHeadInfo.Text = ddlDienstleistung.SelectedItem.Text
                End If
            End If
        End Sub

        Private Function CheckHerstellerVinOk(ByVal hersteller As String, ByVal vin As String) As Boolean
            Dim erg As Boolean = False

            If Not String.IsNullOrEmpty(hersteller) AndAlso Not String.IsNullOrEmpty(vin) Then
                erg = mBeauftragung.CheckHerstellerFahrgestellnummer(hersteller, vin, Me)
            End If

            Return erg
        End Function

        Private Sub DatenSichern()
            '*** Speichern
            With mBeauftragung

                'Grunddaten
                .Kundennr = ddlKunde.SelectedValue

                If rblHalterauswahl.SelectedValue = "Grosskunde" Then
                    .Grosskundennr = ddlGrosskunde.SelectedValue
                    .Grosskunde = ddlGrosskunde.SelectedItem.Text
                Else
                    .Grosskundennr = ""
                    .Grosskunde = ""
                    .HalterAnrede = ddlAnrede.SelectedValue

                    'Firma
                    Select Case ddlAnrede.SelectedValue
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

                    If ddlAnrede.SelectedValue <> "-1" AndAlso ddlAnrede.SelectedValue <> "0" Then
                        .Geburtstag = txtGeburtstag.Text
                    End If

                    .Geburtsort = txtGeburtsort.Text
                    .HalterStrasse = txtStrasse.Text
                    .HalterHausnr = txtHausnummer.Text
                    .HalterHausnrZusatz = txtHnrZusatz.Text
                    .HalterPLZ = txtPLZ.Text
                    If Not String.IsNullOrEmpty(txtOrt.Text) Then
                        .HalterOrt = txtOrt.Text
                    Else
                        .HalterOrt = ddlOrt.SelectedValue
                    End If

                End If

                .HalterReferenz = txtReferenz.Text.ToUpper()
                .Bestellnummer = txtBestellnr.Text.ToUpper()

                .VerkKuerz = txtVerkaeuferkuerzel.Text
                .KiReferenz = txtKundenreferenz.Text
                .Notiz = txtNotiz.Text

                'Fahrzeugdaten
                .Hersteller = txtHersteller.Text
                .Typ = txtTyp.Text.ToUpper()
                .VarianteVersion = txtVarianteVersion.Text.ToUpper()
                .TypPruef = txtTypPruef.Text.ToUpper()
                If txtFahrzeugklasse.Enabled Then
                    .Fahrzeugklasse = txtFahrzeugklasse.Text.ToUpper()
                End If
                If txtAufbauArt.Enabled Then
                    .AufbauArt = txtAufbauArt.Text.ToUpper()
                End If
                .Fahrgestellnummer = txtFahrgestellnummer.Text.ToUpper()
                .FahrgestellnummerPruef = txtFinPruef.Text.ToUpper()
                If ddlFarbe.Enabled Then
                    .Farbe = ddlFarbe.SelectedValue
                End If
                .Briefnummer = txtBriefnummer.Text.ToUpper()
                If txtNummerZB1_1.Enabled Then
                    .ZB1Nummer = txtNummerZB1_1.Text.ToUpper() & "-" & txtNummerZB1_2.Text.ToUpper() & "-" & txtNummerZB1_3.Text.ToUpper() & "-" & txtNummerZB1_4.Text.ToUpper() & "/" & txtNummerZB1_5.Text.ToUpper() & "-" & txtNummerZB1_6.Text.ToUpper()
                End If

                'Dienstleistung
                .StVANr = ddlStva.SelectedValue
                .Kennzeichen = txtKennz1.Text.ToUpper() & "-" & txtKennz2.Text.ToUpper()
                If mBeauftragung.AltkennzeichenSpeichern Then
                    .AltKennzeichen = txtKennzAlt1.Text.ToUpper() & "-" & txtKennzAlt2.Text.ToUpper()
                End If
                If txtEVB.Enabled Then
                    .EVB = txtEVB.Text.ToUpper()
                End If
                .Zulassungsdatum = txtZulDatum.Text
                .Bemerkung = txtDienstBemerkung.Text
                .NaechsteHU = txtNaechsteHU.Text.Replace("."c, "")
                .Materialnummer = ddlDienstleistung.SelectedItem.Value

                .Einzelkennzeichen = cbxEinKennz.Checked
                .Krad = cbxKrad.Checked
                .KennzeichenTyp = ddlKennzTyp.SelectedItem.Value
                .Wunschkennzeichen = cbxWunschkennzFlag.Checked
                .Reserviert = cbxReserviert.Checked
                .Reservierungsnr = txtReservNr.Text

                'Bankdaten
                If txtBlz.Enabled Then
                    .BLZ = txtBlz.Text
                End If
                If txtKontonummer.Enabled Then
                    .Kontonummer = txtKontonummer.Text
                End If
                If cbxEinzug.Enabled Then
                    .Einzug = cbxEinzug.Checked
                End If
                If txtIBAN.Enabled Then
                    .IBAN = txtIBAN.Text.ToUpper()
                    .SWIFT = txtSWIFT.Text.ToUpper()
                End If

                'Gutachten
                If mBeauftragung.ArtGenehmigungSpeichern Then
                    .ArtGenehmigung = "E"
                End If
                If mBeauftragung.PrueforganisationSpeichern Then
                    .Prueforganisation = ddlPrueforganisation.SelectedItem.Value
                End If
                If mBeauftragung.GutachtenNrSpeichern Then
                    .GutachtenNummer = txtGutachtenNr.Text
                End If

                'Zusatzdienstleistungen
                For Each lItem As ListItem In cblZusatzDL.Items
                    mBeauftragung.Zusatzdienstleistungen.Select("MATNR='" & lItem.Value.PadLeft(18, "0"c) & "'")(0)("AUSWAHL") = IIf(lItem.Selected, "X", "")
                Next
            End With

            Session("mBeauftragung2") = mBeauftragung
        End Sub

        Private Sub ResetBeauftragungHalterdaten()
            With mBeauftragung
                .Grosskundennr = ""
                .HalterAnrede = ""
                .Haltername1 = ""
                .Haltername2 = ""
                .Name = ""
                .Geburtsname = ""
                .Geburtstag = ""
                .Geburtsort = ""
                .HalterStrasse = ""
                .HalterHausnr = ""
                .HalterHausnrZusatz = ""
                .HalterPLZ = ""
                .HalterOrt = ""
                .HalterReferenz = ""
                .Bestellnummer = ""
                .VerkKuerz = ""
                .KiReferenz = ""
                .Notiz = ""
            End With
        End Sub

        Private Sub ResetBeauftragung()

            With mBeauftragung
                .Grosskunden = New DataTable()
                .OrteZurPlz = New DataTable()

                'Kunde, Stva und Dienstleistung sollen erhalten bleiben

                'Halterdaten nur clearen, wenn sie nicht gemerkt werden sollen
                If Not mBeauftragung.HalterMerken Then
                    ResetBeauftragungHalterdaten()
                End If

                .GrunddatenVisible = False
                .FahrzeugdatenVisible = False
                .DienstleistungenVisible = False
                .ZusatzdienstleistungenVisible = False
                .ZusammenfassungVisible = False
                .NpaVisible = False

                .BankdatenNeeded = "N"c
                .SEPA = False
                .EvBNeeded = "N"c
                .NaechsteHUNeeded = ""
                .GutachtenNeeded = ""
                .AltKennzeichenNeeded = ""
                .FahrzeugdatenNeeded = ""
                .FarbeNeeded = ""
                .ZB1Needed = ""

                .TypdatenMessage = ""

                .Hersteller = ""
                .Typ = ""
                .VarianteVersion = ""
                .TypPruef = ""
                .Fahrgestellnummer = ""
                .FahrgestellnummerPruef = ""
                .EVB = ""
                .Zulassungsdatum = ""
                .Kennzeichen = ""
                .Bemerkung = ""
                .Einzelkennzeichen = False
                .Krad = False
                .KennzeichenTyp = ""
                .Wunschkennzeichen = False
                .Reserviert = False
                .Reservierungsnr = ""
                .Statustext = ""
                .Briefnummer = ""
                .BLZ = ""
                .Kontonummer = ""
                .IBAN = ""
                .SWIFT = ""
                .Einzug = False
                .He = ""
                .Fr = ""
                .Fi = ""
                .nPaUsed = False
                .ErrorText = ""
                .SapId = ""
                .Autohausvorgang = False
                .HalterNeeded = HalterErfOptionen.Nein
                .HalterMerken = False
                .TypDatenNeeded = ""
                .NaechsteHU = ""
                .ArtGenehmigung = ""
                .Prueforganisation = ""
                .GutachtenNummer = ""
                .AltKennzeichen = ""
                .AusdruckNeeded = "J"c
                .BarcodeNeeded = "J"c
                .ReferenzCode = ""

                .Fahrzeugklasse = ""
                .AufbauArt = ""
                .Farbe = ""
                .ZB1Nummer = ""
            End With

            Session("mBeauftragung2") = mBeauftragung
        End Sub

        Private Sub CheckOkClick()
            lblSaveInfo.Visible = False

            If btnOK.Text = "Schließen" Then
                'Nach Bearbeitung 
                Dim blnBackToAHSelection As Boolean = mBeauftragung.Autohausvorgang

                ResetBeauftragung()
                Session("AfterNPAUse") = Nothing
                Session("DienstleistungChanged") = Nothing

                If blnBackToAHSelection Then
                    Response.Redirect("Change02s_0.aspx?AppID=" & Session("AppID").ToString, False)
                Else
                    Response.Redirect("Change02s.aspx?AppID=" & Session("AppID").ToString, False)
                End If

                Exit Sub

            End If

            '*** Barcode überprüfen
            If mBeauftragung.BarcodeNeeded <> "N"c Then
                pnlDiv1.Visible = True
                pnlDiv2.Visible = True
                pnlDiv3.Visible = True

                Dim messageText As String = ""

                If txtBarcode.Text.Length = 0 Then

                    lblSaveInfo.Text = "Bitte geben Sie einen Barcode ein."
                    lblSaveInfo.ForeColor = Drawing.Color.Red
                    lblSaveInfo.Visible = True

                    ModalPopupExtender2.Show()
                    FocusBarcode()
                    Return
                End If

                Dim status As String = mBeauftragung.CheckBarcode(txtBarcode.Text, Page)

                Select Case status
                    Case "1"
                        messageText = "Barcode in Verwendung(Status: " & mBeauftragung.Statustext & ")."
                    Case "2"
                        messageText = "Barcode nicht vorhanden."
                End Select

                If status <> "0" Then
                    lblSaveInfo.Text = messageText
                    lblSaveInfo.ForeColor = Drawing.Color.Red
                    lblSaveInfo.Visible = True
                    ModalPopupExtender2.Show()
                    FocusBarcode()
                    Return
                End If
            Else
                pnlDiv1.Visible = False
                pnlDiv2.Visible = False
                pnlDiv3.Visible = False
            End If

            pnlDiv1.Visible = False
            pnlDiv2.Visible = False

            mBeauftragung.ReferenzCode = txtBarcode.Text
            Session("mBeauftragung2") = mBeauftragung

            If mBeauftragung.Save2(Me) Then
                pnlDiv3.Visible = False

                btnOK.Text = "Schließen"
                btnCancel.Width = 0
                btnCancel.Text = ""
                btnCancel.Style.Add("display", "none")

                ' Erfolgreich-Meldung ausgeben
                divSuccess.Visible = True
                spanSuccessMessage.InnerText = "Ihr Auftrag wurde unter der ID " & mBeauftragung.SapId & " gespeichert."

                If mBeauftragung.AusdruckNeeded <> "N"c Then
                    divPrintPDF.Visible = True
                Else
                    divPrintPDF.Visible = False
                End If

                ModalPopupExtender2.Show()
            Else
                pnlDiv3.Visible = True

                lblSaveInfo.Text = mBeauftragung.ErrorText
                lblSaveInfo.ForeColor = Drawing.Color.Red
                lblSaveInfo.Visible = True
                ModalPopupExtender2.Show()
            End If
        End Sub

        Private Sub FocusBarcode()
            If (Not ClientScript.IsStartupScriptRegistered("MPEStartup")) Then
                Dim sb As StringBuilder = New StringBuilder()
                sb.Append("<script language=""javascript"" type=""text/javascript"">" & vbCrLf)
                sb.Append("Sys.Application.add_load(function () {" & vbCrLf)
                sb.Append("window.setTimeout(focus,1);" & vbCrLf)
                sb.Append("})" & vbCrLf)
                sb.Append("function focus() {" & vbCrLf)
                sb.Append("var txt = document.getElementById('" & txtBarcode.ClientID & "');" & vbCrLf)
                sb.Append("if (txt != null) { txt.focus(); }" & vbCrLf)
                sb.Append("}" & vbCrLf)
                sb.Append("</script>")
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "MPEStartup", sb.ToString())
            End If
        End Sub

        Private Function FormValidation() As Boolean

            'User kommt aus Grunddaten(Grunddaten prüfen)
            If Grunddaten.Visible And mBeauftragung.HalterNeeded <> HalterErfOptionen.Nein Then
                If ValidateGrunddaten() Then
                    Return False
                End If

                'Wenn sonst keine Fehler, Alter auf Plausibilität prüfen
                If Not String.IsNullOrEmpty(txtGeburtstag.Text) AndAlso IsDate(txtGeburtstag.Text) Then
                    'Halter jünger als 18 oder älter als 90 -> Warnung
                    If CDate(txtGeburtstag.Text) > DateAdd(DateInterval.Year, -18, Date.Today) OrElse CDate(txtGeburtstag.Text) < DateAdd(DateInterval.Year, -90, Date.Today) Then
                        mBeauftragung.AgeConfirmationRequired = True
                    Else
                        mBeauftragung.AgeConfirmationRequired = False
                    End If
                    Session("mBeauftragung2") = mBeauftragung
                End If
            End If

            'User kommt aus Fahrzeugdaten
            If Fahrzeugdaten.Visible Then
                If mBeauftragung.TypDatenNeeded = "N"c Then
                    If ValidateFahrzeugdaten(Source.Part) Then
                        Return False
                    End If
                ElseIf mBeauftragung.TypDatenNeeded = "H"c Then
                    If ValidateFahrzeugdaten(Source.FullWithoutType) Then
                        Return False
                    End If
                Else
                    If ValidateFahrzeugdaten(Source.Full) Then
                        Return False
                    End If
                End If

                'Wenn sonst keine Fehler, Kombination Hersteller/Fahrgestellnummer prüfen
                If Not String.IsNullOrEmpty(txtHersteller.Text) AndAlso Not String.IsNullOrEmpty(txtFahrgestellnummer.Text) AndAlso txtFahrgestellnummer.Text.Length > 2 Then
                    If Not CheckHerstellerVinOk(txtHersteller.Text, txtFahrgestellnummer.Text.ToUpper()) Then
                        mBeauftragung.VinConfirmationRequired = True
                    Else
                        mBeauftragung.VinConfirmationRequired = False
                    End If
                    Session("mBeauftragung2") = mBeauftragung
                End If
            End If

            'User kommt aus Dienstleistungsdaten
            If Dienstleistung.Visible Then
                If ValidateDienstleistung() Then
                    Return False
                End If
            End If

            'User kommt aus ZusatzDL-Daten
            If Zusatzdienstleistungen.Visible Then
                'Aktuell gibt es hier nichts zu validieren
            End If

            Return True
        End Function

        ''' <summary>
        ''' Controls/Werte für Npa setzen
        ''' </summary>
        ''' <remarks></remarks>
        Private Sub SetControlsNpa()

            If Not String.IsNullOrEmpty(mBeauftragung.Kundennr) Then ddlKunde.SelectedValue = mBeauftragung.Kundennr
            'Test Ansichtssteuerung NPA Zusätzliche Einträge für angepasste Dialogsteuerung
            If Not String.IsNullOrEmpty(mBeauftragung.StVANr) Then ddlStva.SelectedValue = mBeauftragung.StVANr

            If Not String.IsNullOrEmpty(mBeauftragung.Materialnummer) Then ddlDienstleistung.SelectedValue = mBeauftragung.Materialnummer
            lblHeadInfo.Text = ddlKunde.SelectedItem.Text & " - " & ddlStva.SelectedItem.Text & " - " & ddlDienstleistung.SelectedItem.Text
            tblBasisdaten.Style.Item("display") = "none"

            lbtGrunddaten.Visible = mBeauftragung.GrunddatenVisible
            lbtFahrzeugdaten.Visible = mBeauftragung.FahrzeugdatenVisible
            lbtDienstleistung.Visible = mBeauftragung.DienstleistungenVisible
            lbtZusatzdienstleistungen.Visible = mBeauftragung.ZusatzdienstleistungenVisible
            lbtZusammenfassung.Visible = mBeauftragung.ZusammenfassungVisible

            mNextTab = NextTab.Halterdaten
            SelectNextTab()

            rblHalterauswahl.SelectedValue = "Halter"
            trGrossKunde.Visible = False
            divHalter.Visible = True
            If Not String.IsNullOrEmpty(mBeauftragung.HalterAnrede) Then ddlAnrede.SelectedValue = mBeauftragung.HalterAnrede
            txtReferenz.Text = mBeauftragung.HalterReferenz
            txtBestellnr.Text = mBeauftragung.Bestellnummer
            txtVerkaeuferkuerzel.Text = mBeauftragung.VerkKuerz
            txtKundenreferenz.Text = mBeauftragung.KiReferenz
            txtNotiz.Text = mBeauftragung.Notiz

            'divNpa.Visible = mBeauftragung.NpaVisible

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
            ddlOrt.Enabled = False

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

            If Not Request.QueryString.Item("eIdentityResponse") Is Nothing Then

                ' Einmalig benötigte DialogControls laden 
                PrepareDienstleistungControls()

                ' Halterdaten füllen
                SetHalter()

                If Not Request.QueryString.Item("GivenNames") Is Nothing Then
                    txtName.Text = Request.QueryString.Item("GivenNames").ToString
                    CType(Session("mBeauftragung2"), Beauftragung2).nPaUsed = True
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

                        Dim datGeburtstag As Date
                        If Date.TryParse(Request.QueryString.Item("DateOfBirth").ToString(), datGeburtstag) Then
                            txtGeburtstag.Text = datGeburtstag.ToShortDateString()
                        Else
                            txtGeburtstag.Text = ""
                            txtGeburtstag.Enabled = True
                        End If

                    End If

                End If

                If Not Request.QueryString.Item("PlaceOfBirth") Is Nothing Then

                    txtGeburtsort.Text = Request.QueryString.Item("PlaceOfBirth").ToString

                End If

                'Test Ansichtsteuerung NPA
                Session("eIdentityResponse") = Nothing

            End If

        End Sub

        Private Sub FillBeauftragung()
            With mBeauftragung
                .Gruppe = m_User.Groups(0).GroupName
                .Verkaufsbuero = Right(m_User.Reference, 4)
                .Verkaufsorganisation = Left(m_User.Reference, 4)

                'Stammdaten laden
                .Fill(Me)
                .FillFarben(Me)
            End With
        End Sub

        ''' <summary>
        ''' Dropdowns mit geringen Datenmengen (mit ViewState)
        ''' </summary>
        ''' <remarks></remarks>
        Private Sub InitSmallDropdowns()

            'Stva füllen (+ Default-Wert setzen)
            Dim defaultStva As String = "0"
            ddlStva.Items.Clear()
            ddlStva.Items.Add(New ListItem("- Keine Auswahl -", "0"))
            For Each dRow As DataRow In mBeauftragung.Kreise.Rows
                ddlStva.Items.Add(New ListItem(dRow("KREISBEZ").ToString(), dRow("ZKFZKZ").ToString()))
                If dRow("ZDEFAULT").ToString() = "X" Then
                    defaultStva = dRow("ZKFZKZ").ToString()
                    txtStva.Text = defaultStva
                    Exit For
                End If
            Next
            ddlStva.SelectedValue = defaultStva
            ApplyNewStva()

            'KennzeichenTyp füllen
            ddlKennzTyp.Items.Clear()
            ddlKennzTyp.Items.Add(New ListItem("E - Euro", "E"))
            ddlKennzTyp.Items.Add(New ListItem("F - Fun", "F"))
            ddlKennzTyp.Items.Add(New ListItem("H - Historisch", "H"))
            ddlKennzTyp.Items.Add(New ListItem("K - Kurzzeit", "K"))
            ddlKennzTyp.Items.Add(New ListItem("S - Saison", "S"))
            ddlKennzTyp.Items.Add(New ListItem("Z - Zoll", "Z"))

            'Anrede füllen
            ddlAnrede.Items.Clear()
            ddlAnrede.Items.Add(New ListItem("- Keine Auswahl -", "-1"))
            ddlAnrede.Items.Add(New ListItem("Firma", "0"))
            ddlAnrede.Items.Add(New ListItem("Herr", "1"))
            ddlAnrede.Items.Add(New ListItem("Frau", "2"))

            'Prüforganisation füllen
            ddlPrueforganisation.Items.Clear()
            For Each dRow As DataRow In mBeauftragung.Prueforganisationen.Rows
                ddlPrueforganisation.Items.Add(New ListItem(dRow("DDTEXT").ToString(), dRow("DOMVALUE_L").ToString()))
            Next

            'Farbauswahl füllen
            ddlFarbe.Items.Clear()
            For Each dRow As DataRow In mBeauftragung.Farben.Rows
                ddlFarbe.Items.Add(New ListItem(dRow("DOMVALUE_L").ToString() & " - " & dRow("DDTEXT").ToString(), dRow("DOMVALUE_L").ToString()))
            Next

        End Sub

        ''' <summary>
        ''' Dropdowns mit großen Datenmengen (ohne ViewState)
        ''' </summary>
        ''' <remarks></remarks>
        Private Sub InitLargeDropdowns()

            'Kunde füllen
            ddlKunde.Items.Clear()
            ddlKunde.Items.Add(New ListItem("- Keine Auswahl -", "0"))
            For Each dRow As DataRow In mBeauftragung.Kunden.Rows
                ddlKunde.Items.Add(New ListItem(dRow("NAME1").ToString(), dRow("KUNNR").ToString().TrimStart("0"c)))
            Next

        End Sub

        Private Sub InitZusatzDLCheckboxlist()
            cblZusatzDL.Items.Clear()
            For Each dRow As DataRow In mBeauftragung.Zusatzdienstleistungen.Rows
                cblZusatzDL.Items.Add(New ListItem(dRow("MAKTX").ToString(), dRow("MATNR").ToString().TrimStart("0"c)))
            Next
        End Sub

        Private Sub InitGrosskundenDropdown()
            ddlGrosskunde.Items.Clear()
            ddlGrosskunde.Items.Add(New ListItem("- Keine Auswahl -", "0"))
            For Each dRow As DataRow In mBeauftragung.Grosskunden.Rows
                ddlGrosskunde.Items.Add(New ListItem(dRow("ZNAME1").ToString(), dRow("ZZGROSSKUNDNR").ToString().TrimStart("0"c)))
            Next
            ddlGrosskunde.SelectedValue = "0"
        End Sub

        Private Sub FillOrtDropdown()
            ddlOrt.Items.Clear()
            For Each dRow As DataRow In mBeauftragung.OrteZurPlz.Rows
                ddlOrt.Items.Add(New ListItem(dRow("ORTSNAME").ToString(), dRow("ORTSNAME").ToString()))
            Next
        End Sub

        Private Function ValidateGrunddaten() As Boolean

            Dim booError As Boolean = False
            Dim name As String
            imgKunde.Visible = False
            imgGrossKunde.Visible = False
            imgHaltername.Visible = False
            imgHalterstrasse.Visible = False
            imgHalterPlzOrt.Visible = False

            If ddlKunde.SelectedValue = "0" Then
                SetErrBehavior(txtKunnr, lblKundeInfo, "Ungültige Kundenauswahl.")
                booError = True
            End If

            If rblHalterauswahl.SelectedValue = "Grosskunde" Then
                If ddlGrosskunde.SelectedValue = "0" Then
                    SetErrBehavior(txtGrosskundennummer, lblGrosskundeInfo, "Grosskundennummer fehlt.")
                    booError = True
                End If
            Else
                'Nur Enabled, wenn nicht elektronischer Perso gewählt wurde
                If txtName.Enabled = True Then

                    Select Case ddlAnrede.SelectedValue
                        Case "-1"
                            If mBeauftragung.HalterNeeded <> HalterErfOptionen.Nein And mBeauftragung.HalterNeeded <> HalterErfOptionen.JaNurName1 Then
                                ddlAnrede.BorderColor = Drawing.Color.Red
                                lblAnredeInfo.Text = "Anrede fehlt"
                                booError = True
                            End If
                            name = "Name1"
                        Case "1", "2" 'Herr, Frau
                            name = "Vorname"

                            If txtName2.Text.Length = 0 Then
                                SetErrBehavior(txtName2, lblName2Info, "Nachname fehlt.")
                                booError = True
                            Else
                                imgHaltername2.Visible = True

                                If txtGeburtstag.Text.Length > 0 Then
                                    If IsDate(txtGeburtstag.Text) = False Then
                                        SetErrBehavior(txtGeburtstag, lblGeburtstagInfo, "Kein gültiges Datum")
                                        booError = True
                                    End If
                                End If

                                If mBeauftragung.HalterNeeded = HalterErfOptionen.Ja Or mBeauftragung.HalterNeeded = HalterErfOptionen.JaOhneGeburtsort Then
                                    If txtGeburtstag.Text.Length = 0 Then
                                        SetErrBehavior(txtGeburtstag, lblGeburtstagInfo, "Geburtsdatum fehlt")
                                        booError = True
                                    End If
                                End If

                                If mBeauftragung.HalterNeeded = HalterErfOptionen.Ja Or mBeauftragung.HalterNeeded = HalterErfOptionen.JaOhneGeburtsdatum Then
                                    If txtGeburtsort.Text.Length = 0 Then
                                        SetErrBehavior(txtGeburtsort, lblGeburtsortInfo, "Geburtsort fehlt.")
                                        booError = True
                                    End If
                                End If
                            End If
                        Case Else 'Firma
                            name = "Name1"
                    End Select

                    If mBeauftragung.HalterNeeded <> HalterErfOptionen.Nein And mBeauftragung.HalterNeeded <> HalterErfOptionen.JaNurName1 And String.IsNullOrEmpty(txtName.Text) Then
                        SetErrBehavior(txtName, lblNameInfo, name & " fehlt.")
                        booError = True
                    Else
                        imgHaltername.Visible = True
                    End If

                    If mBeauftragung.HalterNeeded <> HalterErfOptionen.Nein And mBeauftragung.HalterNeeded <> HalterErfOptionen.JaNurName1 And String.IsNullOrEmpty(txtStrasse.Text) Then
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

                    If mBeauftragung.HalterNeeded <> HalterErfOptionen.Nein And mBeauftragung.HalterNeeded <> HalterErfOptionen.JaNurName1 Then
                        If String.IsNullOrEmpty(txtPLZ.Text) Then
                            If txtOrt.Visible AndAlso String.IsNullOrEmpty(txtOrt.Text) Then
                                SetErrBehavior(txtPLZ, lblOrtInfo, "PLZ und Ort fehlen.")
                            Else
                                SetErrBehavior(txtPLZ, lblOrtInfo, "PLZ fehlt.")
                            End If
                            booError = True
                        ElseIf txtOrt.Visible AndAlso String.IsNullOrEmpty(txtOrt.Text) Then
                            SetErrBehavior(txtPLZ, lblOrtInfo, "Ort fehlt.")
                            booError = True
                        End If
                    End If

                    If Not String.IsNullOrEmpty(txtPLZ.Text) AndAlso Not (txtOrt.Visible And String.IsNullOrEmpty(txtOrt.Text)) Then
                        imgHalterPlzOrt.Visible = True
                    End If

                End If

                End If

                Return booError

        End Function

        Private Function ValidateFahrzeugdaten(ByVal base As Integer) As Boolean
            Dim booError As Boolean = False

            imgHersteller.Visible = False
            imgTyp.Visible = False
            imgTypInfo.Visible = False
            imgFahrzeugklasse.Visible = False
            imgAufbauArt.Visible = False
            imgFahrgestellnummer.Visible = False
            imgBriefnummer.Visible = False
            imgNummerZB1.Visible = False

            If mBeauftragung.FahrzeugdatenNeeded = "J" Then
                If txtFahrzeugklasse.Text.Length = 0 Then
                    SetErrBehavior(txtFahrzeugklasse, lblFahrzeugklasseInfo, "")
                    booError = True
                End If

                If txtAufbauArt.Text.Length = 0 Then
                    SetErrBehavior(txtAufbauArt, lblAufbauArtInfo, "")
                    booError = True
                End If
            End If

            If txtFahrgestellnummer.Text.Length = 0 Then
                SetErrBehavior(txtFahrgestellnummer, lblPruefzifferInfo, "")
                booError = True
            End If

            If txtFinPruef.Text.Length = 0 Then
                SetErrBehavior(txtFinPruef, lblPruefzifferInfo, "")
                booError = True
            End If

            If ValidateBriefnummer() = True Then booError = True

            If mBeauftragung.ZB1Needed = "J" AndAlso ValidateZB1Nummer() = True Then booError = True

            If mBeauftragung.FahrzeugdatenNeeded = "J" Then
                If ValidateFahrzeugklasse() = True Then booError = True
                If ValidateAufbauArt() = True Then booError = True
            End If

            If base = Source.FullWithoutType Or base = Source.Full Then

                If txtHersteller.Text.Length = 0 Then
                    SetErrBehavior(txtHersteller, lblTypInfo, "")
                    booError = True
                End If

                If base = Source.Full Then

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

                End If

            End If

            Return booError

        End Function

        Private Function ValidateDienstleistung() As Boolean
            Dim booError As Boolean = False

            If ddlDienstleistung.SelectedValue = "0" Then
                If ddlDienstleistung.Items.Count = 1 Then
                    ddlDienstleistung.BorderColor = Drawing.Color.Red
                    lblDienstleisungInfo.Text = "Es sind keine Dienstleistungen vorhanden. Wählen Sie eine andere StVA aus!"
                    booError = True
                Else
                    ddlDienstleistung.BorderColor = Drawing.Color.Red
                    lblDienstleisungInfo.Text = "Bitte wählen Sie eine Dienstleistung aus."
                    booError = True
                End If
            End If

            If txtEVB.Enabled Then
                If txtEVB.Text.Length = 0 Then
                    SetErrBehavior(txtEVB, lblEVBInfo, "eVB-Nummer fehlt.")
                    booError = True
                ElseIf txtEVB.Text.Length < 7 Then
                    SetErrBehavior(txtEVB, lblEVBInfo, "eVB-Nummer zu kurz.")
                    booError = True
                ElseIf txtEVB.Text.ToUpper().Contains("O") Or txtEVB.Text.ToUpper().Contains("I") Then
                    SetErrBehavior(txtEVB, lblEVBInfo, "eVB-Nummer darf keine O's und I's enthalten.")
                    booError = True
                End If
            End If

            If txtZulDatum.Text.Length = 0 Then
                SetErrBehavior(txtZulDatum, lblZulDatumInfo, "Datum fehlt.")
                booError = True
            Else
                If IsDate(txtZulDatum.Text) = False Then
                    SetErrBehavior(txtZulDatum, lblZulDatumInfo, "Ungültiges Datum.")
                    booError = True
                ElseIf CDate(txtZulDatum.Text) < Today Then
                    SetErrBehavior(txtZulDatum, lblZulDatumInfo, "Datum darf nicht in der Vergangenheit liegen.")
                    booError = True
                End If
            End If

            If trNaechsteHU.Visible = True Then
                If txtNaechsteHU.Text = String.Empty Then
                    SetErrBehavior(txtNaechsteHU, lblNaechsteHUInfo, "Ungültiges Datum.")
                    booError = True
                Else
                    Dim strDate As String() = txtNaechsteHU.Text.Split("."c)

                    If (strDate.Length < 2) Then
                        SetErrBehavior(txtNaechsteHU, lblNaechsteHUInfo, "Ungültiges Datum.")
                        booError = True
                    ElseIf Not (strDate(1).StartsWith("2") Or strDate(1).StartsWith("1")) Then
                        SetErrBehavior(txtNaechsteHU, lblNaechsteHUInfo, "Ungültiges Jahr.")
                        booError = True
                    ElseIf Not (strDate(0).StartsWith("0") Or strDate(0).StartsWith("1")) Or strDate(0).StartsWith("00") Then
                        SetErrBehavior(txtNaechsteHU, lblNaechsteHUInfo, "Ungültiger Monat.")
                        booError = True
                    End If
                End If
            End If

            If txtBlz.Enabled Then
                If txtBlz.Text.Length = 0 Then
                    SetErrBehavior(txtBlz, lblBlzInfo, "BLZ fehlt.")
                    booError = True
                Else
                    If IsNumeric(txtBlz.Text) = False Then
                        SetErrBehavior(txtBlz, lblBlzInfo, "BLZ nicht numerisch.")
                        booError = True
                    End If

                End If
            End If
            If txtKontonummer.Enabled Then
                If txtKontonummer.Text.Length = 0 Then
                    SetErrBehavior(txtKontonummer, lblKontonrInfo, "Kontonummer fehlt.")
                    booError = True
                Else
                    If IsNumeric(txtKontonummer.Text) = False Then
                        SetErrBehavior(txtKontonummer, lblKontonrInfo, "Kontonummer nicht numerisch.")
                        booError = True
                    End If

                End If
            End If
            If cbxEinzug.Enabled Then
                If cbxEinzug.Checked = False Then
                    cbxEinzug.BorderColor = Drawing.Color.Red
                    lblEinzugInfo.Text = "Einzugsermächtigung nicht angehakt."
                    booError = True
                End If
            End If

            'IBAN/SWIFT
            If txtIBAN.Enabled Then
                If String.IsNullOrEmpty(txtIBAN.Text) Then
                    SetErrBehavior(txtIBAN, lblIBANInfo, "IBAN fehlt.")
                    booError = True
                Else
                    'SWIFT ermitteln
                    Dim strSwift As String = mBeauftragung.GetSWIFT(txtIBAN.Text, Me)

                    If Not String.IsNullOrEmpty(mBeauftragung.Message) Then
                        'Fehler bei der IBAN-Prüfung
                        txtSWIFT.Text = strSwift
                        SetErrBehavior(txtIBAN, lblIBANInfo, mBeauftragung.Message)
                        booError = True
                    Else
                        If String.IsNullOrEmpty(strSwift) Then
                            'keine SWIFT gefunden
                            If Not txtSWIFT.Enabled Then
                                txtSWIFT.Text = ""
                                txtSWIFT.Enabled = True
                                txtSWIFT.ReadOnly = False
                            End If
                            If String.IsNullOrEmpty(txtSWIFT.Text) Then
                                SetErrBehavior(txtSWIFT, lblSWIFTInfo, "Keine SWIFT gefunden, bitte manuell eingeben. Es erfolgt keine weitere Prüfung!")
                                booError = True
                            End If
                        Else
                            txtSWIFT.Text = strSwift
                        End If
                    End If
                End If
            End If

            If trGutachten.Visible = True Then

                If ddlPrueforganisation.SelectedValue = "0" Then
                    ddlPrueforganisation.BorderColor = Drawing.Color.Red
                    lblPrueforganisationInfo.Text = "Bitte wählen Sie eine Prüforganisation aus."
                    booError = True
                End If

            End If

            Dim dRow As DataRow = mBeauftragung.Dienstleistungen.Select("MATNR='" & mBeauftragung.Materialnummer.PadLeft(18, "0"c) & "'")(0)
            Dim vorgang As String = IIf(dRow Is Nothing, "", dRow("VORG"))

            If Not dRow Is Nothing And vorgang = "NZ" And rblHalterauswahl.SelectedValue = "Grosskunde" Then
                ' Nur wenn Neuzulassung + Großkunde + Kennzeichen komplett leer
                If Not txtKennz1.Text = String.Empty Or Not txtKennz2.Text = String.Empty Then
                    If Not Beauftragung2.CheckValidKennzeichenTeil1(txtKennz1.Text.ToUpper()) Then
                        SetErrBehavior(txtKennz1, lblKennzeichenInfo, "Kein reguläres Kennzeichen.")
                        booError = True
                    End If

                    If Not Beauftragung2.CheckValidKennzeichenTeil2(txtKennz2.Text.ToUpper(), {"[A-Z]{0,2}"}) Then
                        SetErrBehavior(txtKennz2, lblKennzeichenInfo, "Kein reguläres Kennzeichen.")
                        booError = True
                    End If

                End If
            Else
                If txtKennz1.Text = String.Empty OrElse Not Beauftragung2.CheckValidKennzeichenTeil1(txtKennz1.Text.ToUpper()) Then
                    SetErrBehavior(txtKennz1, lblKennzeichenInfo, "Kein reguläres Kennzeichen.")
                    booError = True
                End If
                If txtKennz2.Text = String.Empty OrElse Not Beauftragung2.CheckValidKennzeichenTeil2(txtKennz2.Text.ToUpper()) Then
                    SetErrBehavior(txtKennz2, lblKennzeichenInfo, "Kein reguläres Kennzeichen.")
                    booError = True
                End If
            End If

            If trKennzAlt.Visible Then

                If mBeauftragung.AltKennzeichenNeeded = "P"c Then
                    If txtKennzAlt1.Text = String.Empty OrElse Not Beauftragung2.CheckValidKennzeichenTeil1(txtKennzAlt1.Text.ToUpper()) Then
                        SetErrBehavior(txtKennzAlt1, lblKennzeichenAltInfo, "Kein reguläres Kennzeichen.")
                        booError = True
                    End If

                    If txtKennzAlt2.Text = String.Empty OrElse Not Beauftragung2.CheckValidKennzeichenTeil2(txtKennzAlt2.Text.ToUpper()) Then
                        SetErrBehavior(txtKennzAlt2, lblKennzeichenAltInfo, "Kein reguläres Kennzeichen.")
                        booError = True
                    End If
                Else
                    If txtKennzAlt1.Text <> String.Empty And Not Beauftragung2.CheckValidKennzeichenTeil1(txtKennzAlt1.Text.ToUpper()) Then
                        SetErrBehavior(txtKennzAlt1, lblKennzeichenAltInfo, "Kein reguläres Kennzeichen.")
                        booError = True
                    End If

                    If txtKennzAlt2.Text = String.Empty OrElse Not Beauftragung2.CheckValidKennzeichenTeil2(txtKennzAlt2.Text.ToUpper()) Then
                        SetErrBehavior(txtKennzAlt2, lblKennzeichenAltInfo, "Kein reguläres Kennzeichen.")
                        booError = True
                    End If
                End If

            End If

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

        Private Function ValidateZB1Nummer() As Boolean

            Dim strZB1Nummer As String = txtNummerZB1_1.Text & "-" & txtNummerZB1_2.Text & "-" & txtNummerZB1_3.Text & "-" & txtNummerZB1_4.Text & "/" & txtNummerZB1_5.Text & "-" & txtNummerZB1_6.Text

            If strZB1Nummer.Length < 17 Then
                SetErrBehavior(txtNummerZB1_1, lblNummerZB1Info, "ZB1-Nummer unvollständig")
                Return True
            End If

            Dim blnFormatOk = True
            Dim expr1 As New Regex("^[a-zA-ZäöüÄÖÜ]{1,3}$")
            Dim expr2 As New Regex("^[aAkKsS]{1}$")
            Dim expr3 As New Regex("^[a-zA-Z0-9]{1}$")
            Dim expr4 As New Regex("^[0-9]{2,3}$")
            Dim expr5 As New Regex("^[0-9]{2}$")
            Dim expr6 As New Regex("^[0-9]{5}$")
            If Not expr1.IsMatch(txtNummerZB1_1.Text) Then
                SetErrBehavior(txtNummerZB1_1, lblNummerZB1Info, "ZB1-Nummer ungültig")
                blnFormatOk = False
            End If
            If Not expr2.IsMatch(txtNummerZB1_2.Text) Then
                SetErrBehavior(txtNummerZB1_2, lblNummerZB1Info, "ZB1-Nummer ungültig")
                blnFormatOk = False
            End If
            If Not expr3.IsMatch(txtNummerZB1_3.Text) Then
                SetErrBehavior(txtNummerZB1_3, lblNummerZB1Info, "ZB1-Nummer ungültig")
                blnFormatOk = False
            End If
            If Not expr4.IsMatch(txtNummerZB1_4.Text) Then
                SetErrBehavior(txtNummerZB1_4, lblNummerZB1Info, "ZB1-Nummer ungültig")
                blnFormatOk = False
            End If
            If Not expr5.IsMatch(txtNummerZB1_5.Text) Then
                SetErrBehavior(txtNummerZB1_5, lblNummerZB1Info, "ZB1-Nummer ungültig")
                blnFormatOk = False
            End If
            If Not expr6.IsMatch(txtNummerZB1_6.Text) Then
                SetErrBehavior(txtNummerZB1_6, lblNummerZB1Info, "ZB1-Nummer ungültig")
                blnFormatOk = False
            End If
            If Not blnFormatOk Then
                Return True
            End If

            Return False

        End Function

        Private Function ValidateFahrzeugklasse() As Boolean

            If Not String.IsNullOrEmpty(txtFahrzeugklasse.Text) AndAlso txtFahrzeugklasse.Text.ToUpper().Contains("O"c) Then
                SetErrBehavior(txtFahrzeugklasse, lblFahrzeugklasseInfo, "Code für die Fahrzeugklasse darf kein 'O' enthalten")
                Return True
            End If

            Return False

        End Function

        Private Function ValidateAufbauArt() As Boolean

            If Not String.IsNullOrEmpty(txtAufbauArt.Text) AndAlso txtAufbauArt.Text.ToUpper().Contains("O"c) Then
                SetErrBehavior(txtAufbauArt, lblAufbauArtInfo, "Code für die Aufbau-Art darf kein 'O' enthalten")
                Return True
            End If

            Return False

        End Function

        Private Sub InitControls()

            txtKunnr.BorderColor = Drawing.Color.Empty
            lblKundeInfo.Text = ""

            txtStva.BorderColor = Drawing.Color.Empty
            lblKreiseInfo.Text = ""

            ddlDienstleistung.BorderColor = Drawing.Color.Empty
            lblDienstleisungInfo.Text = ""

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
            ddlOrt.BorderColor = Drawing.Color.Empty
            lblOrtInfo.Text = ""

            txtFahrzeugklasse.BorderColor = Drawing.Color.Empty
            lblFahrzeugklasseInfo.Text = ""

            txtAufbauArt.BorderColor = Drawing.Color.Empty
            lblAufbauArtInfo.Text = ""

            txtFahrgestellnummer.BorderColor = Drawing.Color.Empty
            txtFinPruef.BorderColor = Drawing.Color.Empty
            lblPruefzifferInfo.Text = ""

            ddlFarbe.BorderColor = Drawing.Color.Empty
            lblFarbeInfo.Text = ""

            txtBriefnummer.BorderColor = Drawing.Color.Empty
            lblBriefnummerInfo.Text = ""

            txtNummerZB1_1.BorderColor = Drawing.Color.Empty
            txtNummerZB1_2.BorderColor = Drawing.Color.Empty
            txtNummerZB1_3.BorderColor = Drawing.Color.Empty
            txtNummerZB1_4.BorderColor = Drawing.Color.Empty
            txtNummerZB1_5.BorderColor = Drawing.Color.Empty
            txtNummerZB1_6.BorderColor = Drawing.Color.Empty
            lblNummerZB1Info.Text = ""

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

            txtNaechsteHU.BorderColor = Drawing.Color.Empty
            lblNaechsteHUInfo.Text = ""

            txtBlz.BorderColor = Drawing.Color.Empty
            lblBlzInfo.Text = ""

            txtKontonummer.BorderColor = Drawing.Color.Empty
            lblKontonrInfo.Text = ""

            cbxEinzug.BorderColor = Drawing.Color.Empty
            lblEinzugInfo.Text = ""

            txtIBAN.BorderColor = Drawing.Color.Empty
            lblIBANInfo.Text = ""

            txtSWIFT.BorderColor = Drawing.Color.Empty
            lblSWIFTInfo.Text = ""

            ddlPrueforganisation.BorderColor = Drawing.Color.Empty
            lblPrueforganisationInfo.Text = ""

            txtKennz1.BorderColor = Drawing.Color.Empty
            txtKennz2.BorderColor = Drawing.Color.Empty
            lblKennzeichenInfo.Text = ""

            txtKennzAlt1.BorderColor = Drawing.Color.Empty
            txtKennzAlt2.BorderColor = Drawing.Color.Empty
            lblKennzeichenAltInfo.Text = ""

            lblSaveInfo.Text = ""

        End Sub

        Private Sub SetErrBehavior(ByVal txtcontrol As TextBox, ByVal lblControl As Label, ByVal ErrText As String)

            txtcontrol.BorderColor = Drawing.Color.Red

            lblControl.Text = ErrText

        End Sub

        Private Sub ClearHalter()

            ddlAnrede.SelectedValue = "-1"
            rblHalterauswahl.SelectedValue = "Grosskunde"

            divHalter.Visible = False
            'divNpa.Visible = False
            trGrossKunde.Visible = (mBeauftragung.HalterNeeded <> HalterErfOptionen.Nein)

            ddlGrosskunde.SelectedValue = "0"
            ddlGrosskunde.Enabled = True
            txtGrosskundennummer.Enabled = True
            txtName.Enabled = True
            txtName.Text = ""
            lblName.Text = "Name1*"
            txtName2.Enabled = True
            txtName2.Text = ""
            lblName2.Text = "Name2"
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
            ddlOrt.Enabled = True

            If Not mBeauftragung.Autohausvorgang Then
                txtReferenz.Text = ""
                txtBestellnr.Text = ""
            End If

            txtVerkaeuferkuerzel.Text = ""
            txtKundenreferenz.Text = ""
            txtNotiz.Text = ""

            imgGrossKunde.Visible = False
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

        Private Sub ClearDienstleistung()
            txtZulDatum.Text = String.Empty
            lblZulDatumInfo.Text = String.Empty

            ' Kennzeichen Teil 1 bei DL 584 (Abmeldung (außerhalb)) nicht vorbelegen
            If Not String.IsNullOrEmpty(mBeauftragung.Materialnummer) AndAlso mBeauftragung.Materialnummer.TrimStart("0"c) = "584" Then
                txtKennz1.Text = ""
            Else
                txtKennz1.Text = ddlStva.SelectedValue
            End If

            txtKennz2.Text = String.Empty
            lblKennzeichenInfo.Text = String.Empty
            If mBeauftragung.AltKennzeichenNeeded = "P"c Or mBeauftragung.AltKennzeichenNeeded = "O"c Then
                txtKennzAlt1.Text = txtKennz1.Text.ToUpper()
            Else
                txtKennzAlt1.Text = String.Empty
            End If
            txtKennzAlt2.Text = String.Empty
            lblKennzeichenAltInfo.Text = String.Empty
            txtDienstBemerkung.Text = String.Empty
            txtNaechsteHU.Text = String.Empty
            lblNaechsteHUInfo.Text = String.Empty
            cbxEinKennz.Checked = False
            cbxKrad.Checked = False
            ddlKennzTyp.SelectedIndex = 0
            cbxWunschkennzFlag.Checked = False
            cbxReserviert.Checked = False
        End Sub

        Private Sub ClearTypdaten()
            txtHersteller.Text = ""
            txtTyp.Text = ""
            txtVarianteVersion.Text = ""
            txtTypPruef.Text = ""
            lblTypInfo.Text = ""
            txtFahrzeugklasse.Text = ""
            txtAufbauArt.Text = ""
            txtFahrgestellnummer.Text = ""
            txtFinPruef.Text = ""
            lblPruefzifferInfo.Text = ""
            ddlFarbe.SelectedIndex = 0
            txtBriefnummer.Text = ""
            lblBriefnummerInfo.Text = ""
            txtNummerZB1_1.Text = ""
            txtNummerZB1_2.Text = ""
            txtNummerZB1_3.Text = ""
            txtNummerZB1_4.Text = ""
            txtNummerZB1_5.Text = ""
            txtNummerZB1_6.Text = ""
            lblMarkeShow.Text = ""
            lblTypTextShow.Text = ""
            lblVarianteShow.Text = ""
            lblVersionShow.Text = ""
            lblHandelsnameShow.Text = ""
            lblHerstellerKurzShow.Text = ""

            imgHersteller.Visible = False
            imgTyp.Visible = False
            imgTypInfo.Visible = False
            imgFahrzeugklasse.Visible = False
            imgAufbauArt.Visible = False
            imgFahrgestellnummer.Visible = False
            imgBriefnummer.Visible = False
            imgNummerZB1.Visible = False

        End Sub

        Private Sub SetHalter()

            If ddlAnrede.SelectedValue = "-1" Then
                ClearHalter()
                Exit Sub
            End If

            Select Case ddlAnrede.SelectedValue
                Case "0"
                    lblName.Text = "Name1*"
                    lblName2.Text = "Name2"
                    'divNpa.Visible = False
                    trGeburtsort.Visible = False
                    trGeburtstag.Visible = False
                Case Else
                    lblName.Text = "Vorname*"
                    lblName2.Text = "Nachname*"
                    'divNpa.Visible = True
                    trAnrede.Visible = (mBeauftragung.HalterNeeded <> HalterErfOptionen.Nein And mBeauftragung.HalterNeeded <> HalterErfOptionen.JaNurName1)
                    trName.Visible = (mBeauftragung.HalterNeeded <> HalterErfOptionen.Nein)
                    trName2.Visible = (mBeauftragung.HalterNeeded <> HalterErfOptionen.Nein And mBeauftragung.HalterNeeded <> HalterErfOptionen.JaNurName1)
                    trGeburtsort.Visible = (mBeauftragung.HalterNeeded = HalterErfOptionen.Ja Or mBeauftragung.HalterNeeded = HalterErfOptionen.JaOhneGeburtsdatum)
                    trGeburtstag.Visible = (mBeauftragung.HalterNeeded = HalterErfOptionen.Ja Or mBeauftragung.HalterNeeded = HalterErfOptionen.JaOhneGeburtsort)
                    trStrasse.Visible = (mBeauftragung.HalterNeeded <> HalterErfOptionen.Nein And mBeauftragung.HalterNeeded <> HalterErfOptionen.JaNurName1)
                    trOrt.Visible = (mBeauftragung.HalterNeeded <> HalterErfOptionen.Nein And mBeauftragung.HalterNeeded <> HalterErfOptionen.JaNurName1)
            End Select
        End Sub

        Private Sub RestoreSelectedValues()
            If Not String.IsNullOrEmpty(mBeauftragung.Kundennr) Then
                ddlKunde.SelectedValue = mBeauftragung.Kundennr
                txtKunnr.Text = mBeauftragung.Kundennr
            End If
            If Not String.IsNullOrEmpty(mBeauftragung.StVANr) Then
                ddlStva.SelectedValue = mBeauftragung.StVANr
                txtStva.Text = mBeauftragung.StVANr
            End If
            If Not String.IsNullOrEmpty(mBeauftragung.Materialnummer) Then
                ddlDienstleistung.SelectedValue = mBeauftragung.Materialnummer
                txtDienstleistung.Text = mBeauftragung.Materialnummer
            End If
        End Sub

        Private Sub RestoreAutohausVorgang()
            With mBeauftragung

                'Halterdaten
                rblHalterauswahl.SelectedValue = "Halter"
                trGrossKunde.Visible = False
                txtGrosskundennummer.Text = ""
                divHalter.Visible = True
                trAnrede.Visible = (mBeauftragung.HalterNeeded <> HalterErfOptionen.Nein And mBeauftragung.HalterNeeded <> HalterErfOptionen.JaNurName1)
                trName.Visible = (mBeauftragung.HalterNeeded <> HalterErfOptionen.Nein)
                trName2.Visible = (mBeauftragung.HalterNeeded <> HalterErfOptionen.Nein And mBeauftragung.HalterNeeded <> HalterErfOptionen.JaNurName1)
                trGeburtsort.Visible = (mBeauftragung.HalterNeeded = HalterErfOptionen.Ja Or mBeauftragung.HalterNeeded = HalterErfOptionen.JaOhneGeburtsdatum)
                trGeburtstag.Visible = (mBeauftragung.HalterNeeded = HalterErfOptionen.Ja Or mBeauftragung.HalterNeeded = HalterErfOptionen.JaOhneGeburtsort)
                trStrasse.Visible = (mBeauftragung.HalterNeeded <> HalterErfOptionen.Nein And mBeauftragung.HalterNeeded <> HalterErfOptionen.JaNurName1)
                trOrt.Visible = (mBeauftragung.HalterNeeded <> HalterErfOptionen.Nein And mBeauftragung.HalterNeeded <> HalterErfOptionen.JaNurName1)
                txtName.Focus()
                Select Case .HalterAnrede
                    Case "Firma"
                        ddlAnrede.SelectedValue = "0"
                    Case "Herr"
                        ddlAnrede.SelectedValue = "1"
                    Case "Frau"
                        ddlAnrede.SelectedValue = "2"
                End Select
                txtName.Text = .Haltername1
                txtName2.Text = .Haltername2
                txtGeburtstag.Text = .Geburtstag
                txtGeburtsort.Text = .Geburtsort
                txtStrasse.Text = .HalterStrasse
                txtHausnummer.Text = .HalterHausnr
                txtHnrZusatz.Text = .HalterHausnrZusatz
                txtPLZ.Text = .HalterPLZ
                txtOrt.Text = .HalterOrt
                txtReferenz.Text = .HalterReferenz
                txtBestellnr.Text = .Bestellnummer
                txtVerkaeuferkuerzel.Text = .VerkKuerz
                txtKundenreferenz.Text = .KiReferenz
                txtNotiz.Text = .Notiz

                'Fahrzeugdaten
                txtHersteller.Text = .Hersteller
                txtTyp.Text = .Typ
                txtVarianteVersion.Text = .VarianteVersion
                txtTypPruef.Text = .TypPruef
                txtFahrgestellnummer.Text = .Fahrgestellnummer
                txtFinPruef.Text = .FahrgestellnummerPruef
                txtBriefnummer.Text = .Briefnummer

                'Dienstleistung
                txtEVB.Text = .EVB
                txtZulDatum.Text = .Zulassungsdatum
                If Not String.IsNullOrEmpty(.Kennzeichen) AndAlso .Kennzeichen.Contains("-"c) Then
                    txtKennz1.Text = .Kennzeichen.Substring(0, .Kennzeichen.IndexOf("-"c))
                    txtKennz2.Text = .Kennzeichen.Substring(.Kennzeichen.IndexOf("-"c) + 1)
                End If
                If Not String.IsNullOrEmpty(.AltKennzeichen) AndAlso .AltKennzeichen.Contains("-"c) Then
                    txtKennzAlt1.Text = .AltKennzeichen.Substring(0, .AltKennzeichen.IndexOf("-"c))
                    txtKennzAlt2.Text = .AltKennzeichen.Substring(.AltKennzeichen.IndexOf("-"c) + 1)
                End If
                txtDienstBemerkung.Text = .Bemerkung
                txtKontonummer.Text = .Kontonummer
                txtBlz.Text = .BLZ
                txtIBAN.Text = .IBAN
                txtSWIFT.Text = .SWIFT

                cbxEinKennz.Checked = .Einzelkennzeichen
                cbxKrad.Checked = .Krad
                If Not String.IsNullOrEmpty(.KennzeichenTyp) Then
                    ddlKennzTyp.SelectedValue = .KennzeichenTyp
                End If
                cbxWunschkennzFlag.Checked = .Wunschkennzeichen
                cbxReserviert.Checked = .Reserviert
                txtReservNr.Text = .Reservierungsnr

                'Zusatzdiensleistungen
                For Each lItem As ListItem In cblZusatzDL.Items
                    lItem.Selected = (.Zusatzdienstleistungen.Select("MATNR='" & lItem.Value.PadLeft(18, "0"c) & "'")(0)("AUSWAHL").ToString() = "X")
                Next

            End With
        End Sub

        Private Sub RestoreHalterdaten()
            With mBeauftragung

                If Not String.IsNullOrEmpty(.Grosskundennr) Then

                    rblHalterauswahl.SelectedValue = "Grosskunde"
                    trGrossKunde.Visible = True
                    divHalter.Visible = False
                    txtGrosskundennummer.Text = .Grosskundennr
                    txtReferenz.Text = .HalterReferenz
                    txtBestellnr.Text = .Bestellnummer
                    txtVerkaeuferkuerzel.Text = .VerkKuerz
                    txtKundenreferenz.Text = .KiReferenz
                    txtNotiz.Text = .Notiz

                ElseIf Not String.IsNullOrEmpty(.Haltername1) Then

                    rblHalterauswahl.SelectedValue = "Halter"
                    trGrossKunde.Visible = False
                    divHalter.Visible = True
                    trAnrede.Visible = (mBeauftragung.HalterNeeded <> HalterErfOptionen.Nein And mBeauftragung.HalterNeeded <> HalterErfOptionen.JaNurName1)
                    trName.Visible = (mBeauftragung.HalterNeeded <> HalterErfOptionen.Nein)
                    trName2.Visible = (mBeauftragung.HalterNeeded <> HalterErfOptionen.Nein And mBeauftragung.HalterNeeded <> HalterErfOptionen.JaNurName1)
                    trGeburtsort.Visible = (mBeauftragung.HalterNeeded = HalterErfOptionen.Ja Or mBeauftragung.HalterNeeded = HalterErfOptionen.JaOhneGeburtsdatum)
                    trGeburtstag.Visible = (mBeauftragung.HalterNeeded = HalterErfOptionen.Ja Or mBeauftragung.HalterNeeded = HalterErfOptionen.JaOhneGeburtsort)
                    trStrasse.Visible = (mBeauftragung.HalterNeeded <> HalterErfOptionen.Nein And mBeauftragung.HalterNeeded <> HalterErfOptionen.JaNurName1)
                    trOrt.Visible = (mBeauftragung.HalterNeeded <> HalterErfOptionen.Nein And mBeauftragung.HalterNeeded <> HalterErfOptionen.JaNurName1)
                    ddlAnrede.SelectedValue = .HalterAnrede
                    txtName.Text = .Haltername1
                    txtName2.Text = .Haltername2
                    txtGeburtstag.Text = .Geburtstag
                    txtGeburtsort.Text = .Geburtsort
                    txtStrasse.Text = .HalterStrasse
                    txtHausnummer.Text = .HalterHausnr
                    txtHnrZusatz.Text = .HalterHausnrZusatz
                    txtPLZ.Text = .HalterPLZ
                    txtOrt.Text = .HalterOrt
                    txtReferenz.Text = .HalterReferenz
                    txtBestellnr.Text = .Bestellnummer
                    txtVerkaeuferkuerzel.Text = .VerkKuerz
                    txtKundenreferenz.Text = .KiReferenz
                    txtNotiz.Text = .Notiz

                End If

            End With
        End Sub

        Private Sub InitJava()
            txtKunnr.Attributes.Add("onkeyup", "FilterItems(this.value," + ddlKunde.ClientID + ")")
            txtKunnr.Attributes.Add("onblur", "SetItemText(" + ddlKunde.ClientID + ",this)")
            ddlKunde.Attributes.Add("onchange", "SetItemText(" + ddlKunde.ClientID + "," + txtKunnr.ClientID + ")")
            txtStva.Attributes.Add("onkeyup", "FilterItems(this.value," + ddlStva.ClientID + ")")
            txtStva.Attributes.Add("onblur", "SetItemText(" + ddlStva.ClientID + ",this)")
            ddlStva.Attributes.Add("onchange", "SetItemText(" + ddlStva.ClientID + "," + txtStva.ClientID + ")")
            txtDienstleistung.Attributes.Add("onkeyup", "FilterItems(this.value," + ddlDienstleistung.ClientID + ")")
            txtDienstleistung.Attributes.Add("onblur", "SetItemText(" + ddlDienstleistung.ClientID + ",this)")
            ddlDienstleistung.Attributes.Add("onchange", "SetItemText(" + ddlDienstleistung.ClientID + "," + txtDienstleistung.ClientID + ")")
            txtGrosskundennummer.Attributes.Add("onkeyup", "FilterItems(this.value," + ddlGrosskunde.ClientID + ")")
            txtGrosskundennummer.Attributes.Add("onblur", "SetItemText(" + ddlGrosskunde.ClientID + ",this)")
            ddlGrosskunde.Attributes.Add("onchange", "SetItemText(" + ddlGrosskunde.ClientID + "," + txtGrosskundennummer.ClientID + ")")
            txtHersteller.Attributes("onkeyup") = "autotab(" & txtHersteller.ClientID & ", " & txtTyp.ClientID & ")"
            txtTyp.Attributes("onkeyup") = "autotab(" & txtTyp.ClientID & ", " & txtVarianteVersion.ClientID & ")"
            txtVarianteVersion.Attributes("onkeyup") = "autotab(" & txtVarianteVersion.ClientID & ", " & txtTypPruef.ClientID & ")"
            txtFahrzeugklasse.Attributes("onkeyup") = "autotab(" & txtFahrzeugklasse.ClientID & ", " & txtAufbauArt.ClientID & ")"
            txtAufbauArt.Attributes("onkeyup") = "autotab(" & txtAufbauArt.ClientID & ", " & txtFahrgestellnummer.ClientID & ")"
            txtTypPruef.Attributes("onkeyup") = "autotab(" & txtTypPruef.ClientID & ", " & txtFahrgestellnummer.ClientID & ")"
            txtFahrgestellnummer.Attributes("onkeyup") = "autotab(" & txtFahrgestellnummer.ClientID & ", " & txtFinPruef.ClientID & ")"
            txtFinPruef.Attributes("onkeyup") = "autotab(" & txtFinPruef.ClientID & ", " & txtBriefnummer.ClientID & ")"
            txtNummerZB1_1.Attributes("onkeyup") = "autotab(" & txtNummerZB1_1.ClientID & ", " & txtNummerZB1_2.ClientID & ")"
            txtNummerZB1_2.Attributes("onkeyup") = "autotab(" & txtNummerZB1_2.ClientID & ", " & txtNummerZB1_3.ClientID & ")"
            txtNummerZB1_3.Attributes("onkeyup") = "autotab(" & txtNummerZB1_3.ClientID & ", " & txtNummerZB1_4.ClientID & ")"
            txtNummerZB1_4.Attributes("onkeyup") = "autotab(" & txtNummerZB1_4.ClientID & ", " & txtNummerZB1_5.ClientID & ")"
            txtNummerZB1_5.Attributes("onkeyup") = "autotab(" & txtNummerZB1_5.ClientID & ", " & txtNummerZB1_6.ClientID & ")"
        End Sub

        Private Sub PrepareDienstleistungControls()
            EnableBankdaten()
            EnableEvB()
            EnableNaechsteHU()
            EnableGutachten()
            EnableAltKennzeichen()
        End Sub

        Private Sub SelectNextTab()

            ' Auswahl des ersten anzuzeigenden Tabs wenn kein Tab explizit gewählt wurde
            If mNextTab = NextTab.KeineAuswahl Or mNextTab = Nothing Then
                mNextTab = NextTab.Halterdaten
            End If

            ' Active-Button-Steuerung
            Select Case mNextTab
                Case NextTab.Halterdaten
                    Grunddaten.Visible = True
                    lbtGrunddaten.CssClass = "TabButtonActive"
                    Fahrzeugdaten.Visible = False
                    lbtFahrzeugdaten.CssClass = "TabButton"
                    Dienstleistung.Visible = False
                    lbtDienstleistung.CssClass = "TabButton"
                    Zusatzdienstleistungen.Visible = False
                    lbtZusatzdienstleistungen.CssClass = "TabButton"
                    Zusammenfassung.Visible = False
                    lbtZusammenfassung.CssClass = "TabButton"
                    trSelectGrosskundeHalter.Visible = (mBeauftragung.HalterNeeded <> HalterErfOptionen.Nein)
                    EnableHalterdaten()

                Case NextTab.Typdaten
                    'Specials für Düren
                    If ddlStva.SelectedValue = "DN" Then
                        txtReferenz.Text = txtName.Text.ToUpper()
                    End If
                    Grunddaten.Visible = False
                    lbtGrunddaten.CssClass = "TabButton"
                    Fahrzeugdaten.Visible = True
                    trHersteller.Visible = (mBeauftragung.TypDatenNeeded = "J"c Or mBeauftragung.TypDatenNeeded = "H"c)
                    trTypdaten.Visible = (mBeauftragung.TypDatenNeeded = "J"c)
                    trFahrzeugklasse.Visible = (mBeauftragung.FahrzeugdatenNeeded = "J")
                    trAufbauArt.Visible = (mBeauftragung.FahrzeugdatenNeeded = "J")
                    trFarbe.Visible = (mBeauftragung.FarbeNeeded = "J")
                    trNummerZB1.Visible = (mBeauftragung.ZB1Needed = "J")
                    lbtFahrzeugdaten.CssClass = "TabButtonActive"
                    Dienstleistung.Visible = False
                    lbtDienstleistung.CssClass = "TabButton"
                    Zusatzdienstleistungen.Visible = False
                    lbtZusatzdienstleistungen.CssClass = "TabButton"
                    Zusammenfassung.Visible = False
                    lbtZusammenfassung.CssClass = "TabButton"
                    EnableFahrzeugdaten()
                    txtHersteller.Focus()

                Case NextTab.Dienstleistung
                    Grunddaten.Visible = False
                    lbtGrunddaten.CssClass = "TabButton"
                    Fahrzeugdaten.Visible = False
                    lbtFahrzeugdaten.CssClass = "TabButton"
                    Dienstleistung.Visible = True
                    lbtDienstleistung.CssClass = "TabButtonActive"
                    Zusatzdienstleistungen.Visible = False
                    lbtZusatzdienstleistungen.CssClass = "TabButton"
                    Zusammenfassung.Visible = False
                    lbtZusammenfassung.CssClass = "TabButton"
                    GetGrosskundenBankdaten()
                    txtEVB.Focus()

                Case NextTab.Zusatzdienstleistungen
                    Grunddaten.Visible = False
                    lbtGrunddaten.CssClass = "TabButton"
                    Fahrzeugdaten.Visible = False
                    lbtFahrzeugdaten.CssClass = "TabButton"
                    Dienstleistung.Visible = False
                    lbtDienstleistung.CssClass = "TabButton"
                    Zusatzdienstleistungen.Visible = True
                    lbtZusatzdienstleistungen.CssClass = "TabButtonActive"
                    Zusammenfassung.Visible = False
                    lbtZusammenfassung.CssClass = "TabButton"
                    cblZusatzDL.Focus()

                Case NextTab.Zusammenfassung
                    Grunddaten.Visible = False
                    lbtGrunddaten.CssClass = "TabButton"
                    Fahrzeugdaten.Visible = False
                    lbtFahrzeugdaten.CssClass = "TabButton"
                    Dienstleistung.Visible = False
                    lbtDienstleistung.CssClass = "TabButton"
                    Zusatzdienstleistungen.Visible = False
                    lbtZusatzdienstleistungen.CssClass = "TabButton"
                    Zusammenfassung.Visible = True
                    lbtZusammenfassung.CssClass = "TabButtonActive"
                    EnableZusammenfassungControls()
                    ShowZusammenfassung()
                    lbCreate.Focus()

                Case NextTab.AfterNpa
                    Grunddaten.Visible = True
                    lbtGrunddaten.CssClass = "TabButtonActive"
                    Fahrzeugdaten.Visible = False
                    lbtFahrzeugdaten.CssClass = "TabButton"
                    Dienstleistung.Visible = False
                    lbtDienstleistung.CssClass = "TabButton"
                    Zusatzdienstleistungen.Visible = False
                    lbtZusatzdienstleistungen.CssClass = "TabButton"
                    Zusammenfassung.Visible = False
                    lbtZusammenfassung.CssClass = "TabButton"
                    txtReferenz.Focus()

            End Select
        End Sub

        ''' <summary>
        ''' Bei Bedarf SEPA-Daten des Großkunden als Vorbelegung anzeigen
        ''' </summary>
        ''' <remarks></remarks>
        Private Sub GetGrosskundenBankdaten()
            If (mBeauftragung.BankdatenNeeded = "G"c Or mBeauftragung.BankdatenNeeded = "B"c) AndAlso mBeauftragung.SEPA AndAlso String.IsNullOrEmpty(txtIBAN.Text) Then
                Dim grosskundenDaten As DataRow() = mBeauftragung.Grosskunden.Select("ZZGROSSKUNDNR = '" & mBeauftragung.Grosskundennr & "'")
                If grosskundenDaten IsNot Nothing AndAlso grosskundenDaten.Length > 0 AndAlso Not String.IsNullOrEmpty(grosskundenDaten(0)("IBAN").ToString()) Then
                    txtIBAN.Text = grosskundenDaten(0)("IBAN").ToString()
                    txtSWIFT.Text = grosskundenDaten(0)("SWIFT").ToString()
                    SetErrBehavior(txtIBAN, lblIBANInfo, "Bitte prüfen!")
                    SetErrBehavior(txtSWIFT, lblSWIFTInfo, "Bitte prüfen!")
                End If
            End If
        End Sub

        Private Sub ShowZusammenfassung()
            Try
                lblZusKundeData.Text = mBeauftragung.Kunden.Select("KUNNR='" & mBeauftragung.Kundennr.PadLeft(10, "0"c) & "'")(0)("NAME1").ToString()

                'Specials für Düren
                If ddlStva.SelectedValue = "DN" Then
                    trZusHalter.Visible = True
                    lblZusHalterData.Text = txtReferenz.Text
                Else
                    trZusHalter.Visible = False
                End If

                lblZusStvaData.Text = mBeauftragung.Kreise.Select("ZKFZKZ='" & mBeauftragung.StVANr & "'")(0)("KREISBEZ").ToString()
                lblZusDLData.Text = mBeauftragung.Dienstleistungen.Select("MATNR='" & mBeauftragung.Materialnummer.PadLeft(18, "0"c) & "'")(0)("MAKTX").ToString()
                lblZusZuldatData.Text = mBeauftragung.Zulassungsdatum
                lblZusKennzData.Text = mBeauftragung.Kennzeichen
                lblZuseVBData.Text = mBeauftragung.EVB
                lblZusNurEinKennzData.Text = IIf(mBeauftragung.Einzelkennzeichen, "ja", "nein")
                lblZusKradData.Text = IIf(mBeauftragung.Krad, "ja", "nein")
                lblZusKenneichenTypData.Text = mBeauftragung.KennzeichenTyp
                lblZusWunschKennzData.Text = IIf(mBeauftragung.Wunschkennzeichen, "ja", "nein")
                lblZusReserviertNrData.Text = mBeauftragung.Reservierungsnr

                listZusZusatzDLsData.Items.Clear()
                For Each dRow As DataRow In mBeauftragung.Zusatzdienstleistungen.Rows
                    If dRow("AUSWAHL").ToString() = "X" Then
                        listZusZusatzDLsData.Items.Add(dRow("MAKTX"))
                    End If
                Next

                lblZusIBANData.Text = mBeauftragung.IBAN
                lblZusSWIFTData.Text = mBeauftragung.SWIFT

            Catch ex As Exception
                lblError.Text = "Fehler beim Anzeigen der Zusammenfassung: " & ex.Message
            End Try
        End Sub

        Private Sub EnableHalterdaten()
            Dim doEnable As Boolean = False
            Dim AfterNpa As Boolean = False

            If CBool(Session("AfterNPAUse")) Then
                doEnable = True
                AfterNpa = True
            ElseIf mBeauftragung.HalterNeeded <> HalterErfOptionen.Nein Then
                doEnable = True
            End If

            Dim blnEnableNoNpa As Boolean = (doEnable And Not AfterNpa)

            rblHalterauswahl.Items(0).Enabled = blnEnableNoNpa
            rblHalterauswahl.Items(1).Enabled = doEnable
            ddlGrosskunde.Enabled = blnEnableNoNpa
            txtGrosskundennummer.Enabled = blnEnableNoNpa
            ddlAnrede.Enabled = blnEnableNoNpa
            txtName.Enabled = blnEnableNoNpa
            txtName2.Enabled = blnEnableNoNpa
            txtGeburtstag.Enabled = blnEnableNoNpa
            txtGeburtsort.Enabled = blnEnableNoNpa
            txtStrasse.Enabled = blnEnableNoNpa
            txtHausnummer.Enabled = blnEnableNoNpa
            txtHnrZusatz.Enabled = blnEnableNoNpa
            txtPLZ.Enabled = blnEnableNoNpa
            txtOrt.Enabled = blnEnableNoNpa
            ddlOrt.Enabled = blnEnableNoNpa

            Dim farbe As Drawing.Color = IIf(blnEnableNoNpa, Drawing.Color.White, Drawing.Color.LightGray)

            txtGrosskundennummer.BackColor = farbe
            ddlAnrede.BackColor = farbe
            txtName.BackColor = farbe
            txtName2.BackColor = farbe
            txtGeburtsort.BackColor = farbe
            txtGeburtsort.BackColor = farbe
            txtStrasse.BackColor = farbe
            txtHausnummer.BackColor = farbe
            txtHnrZusatz.BackColor = farbe
            txtPLZ.BackColor = farbe
            txtOrt.BackColor = farbe
            ddlOrt.BackColor = farbe

            If CBool(Session("AfterNPAUse")) OrElse mBeauftragung.HalterNeeded <> HalterErfOptionen.Nein Then
                rblHalterauswahl.Focus()
            Else
                txtReferenz.Focus()
            End If
        End Sub

        Private Sub EnableFahrzeugdaten()
            txtFahrzeugklasse.Enabled = (mBeauftragung.FahrzeugdatenNeeded = "J")
            txtAufbauArt.Enabled = (mBeauftragung.FahrzeugdatenNeeded = "J")
            ddlFarbe.Enabled = (mBeauftragung.FarbeNeeded = "J")
            txtNummerZB1_1.Enabled = (mBeauftragung.ZB1Needed = "J")
            txtNummerZB1_2.Enabled = (mBeauftragung.ZB1Needed = "J")
            txtNummerZB1_3.Enabled = (mBeauftragung.ZB1Needed = "J")
            txtNummerZB1_4.Enabled = (mBeauftragung.ZB1Needed = "J")
            txtNummerZB1_5.Enabled = (mBeauftragung.ZB1Needed = "J")
            txtNummerZB1_6.Enabled = (mBeauftragung.ZB1Needed = "J")
        End Sub

        Private Sub EnableBankdaten()
            Dim doEnable As Boolean = False
            Dim sepa As Boolean = False

            Select Case mBeauftragung.BankdatenNeeded
                Case "H"c
                    doEnable = (rblHalterauswahl.SelectedValue = "Halter")
                    sepa = ((rblHalterauswahl.SelectedValue = "Halter") AndAlso mBeauftragung.SEPA)
                Case "G"c
                    doEnable = (rblHalterauswahl.SelectedValue = "Grosskunde")
                    sepa = ((rblHalterauswahl.SelectedValue = "Grosskunde") AndAlso mBeauftragung.SEPA)
                Case "B"c
                    doEnable = True
                    sepa = mBeauftragung.SEPA
            End Select

            Dim blnEnableSepa As Boolean = (doEnable And sepa)
            Dim blnEnableNoSepa As Boolean = (doEnable And Not sepa)

            cbxEinzug.Enabled = doEnable
            cbxEinzug.Checked = doEnable

            'Entweder BLZ/Kontonummer oder IBAN/SWIFT
            lblBLZ.Visible = blnEnableNoSepa
            txtBlz.Visible = blnEnableNoSepa
            txtBlz.Enabled = blnEnableNoSepa
            lblBlzInfo.Visible = blnEnableNoSepa
            lblKontonummer.Visible = blnEnableNoSepa
            txtKontonummer.Visible = blnEnableNoSepa
            txtKontonummer.Enabled = blnEnableNoSepa
            lblKontonrInfo.Visible = blnEnableNoSepa
            lblEinzug.Visible = doEnable
            cbxEinzug.Visible = doEnable
            cbxEinzug.Enabled = doEnable
            lblEinzugInfo.Visible = doEnable
            lblIBAN.Visible = blnEnableSepa
            txtIBAN.Visible = blnEnableSepa
            txtIBAN.Enabled = blnEnableSepa
            lblIBANInfo.Visible = blnEnableSepa
            lblSWIFT.Visible = blnEnableSepa
            txtSWIFT.Visible = blnEnableSepa
            lblSWIFTInfo.Visible = blnEnableSepa

            Dim farbeSepa As Drawing.Color = IIf(blnEnableSepa, Drawing.Color.White, Drawing.Color.LightGray)
            Dim farbeNoSepa As Drawing.Color = IIf(blnEnableNoSepa, Drawing.Color.White, Drawing.Color.LightGray)

            txtBlz.BackColor = farbeNoSepa
            txtKontonummer.BackColor = farbeNoSepa
            txtIBAN.BackColor = farbeSepa
            txtSWIFT.BackColor = farbeSepa

            txtBlz.Text = String.Empty
            lblBlzInfo.Text = String.Empty
            txtKontonummer.Text = String.Empty
            lblKontonrInfo.Text = String.Empty
            lblEinzugInfo.Text = String.Empty
            txtIBAN.Text = String.Empty
            lblIBANInfo.Text = String.Empty
            txtSWIFT.Text = String.Empty
            lblSWIFTInfo.Text = String.Empty
        End Sub

        Private Sub EnableEvB()
            Dim doEnable As Boolean = False

            Select Case mBeauftragung.EvBNeeded
                Case "H"c
                    doEnable = (rblHalterauswahl.SelectedValue = "Halter")
                Case "G"c
                    doEnable = (rblHalterauswahl.SelectedValue = "Grosskunde")
                Case "B"c
                    doEnable = True
            End Select

            txtEVB.Enabled = doEnable

            If doEnable Then
                txtEVB.BackColor = Drawing.Color.White
            Else
                txtEVB.BackColor = Drawing.Color.LightGray
            End If

            txtEVB.Text = String.Empty
            lblEVBInfo.Text = String.Empty
        End Sub

        Private Sub EnableNaechsteHU()
            Dim doEnable As Boolean = False

            Select Case mBeauftragung.NaechsteHUNeeded
                Case "H"c
                    doEnable = (rblHalterauswahl.SelectedValue = "Halter")
                Case "G"c
                    doEnable = (rblHalterauswahl.SelectedValue = "Grosskunde")
                Case "B"c
                    doEnable = True
            End Select

            trNaechsteHU.Visible = doEnable
            txtNaechsteHU.Text = String.Empty
        End Sub

        Private Sub EnableGutachten()
            Dim doEnable As Boolean = False

            Select Case mBeauftragung.GutachtenNeeded
                Case "H"c
                    doEnable = (rblHalterauswahl.SelectedValue = "Halter")
                Case "G"c
                    doEnable = (rblHalterauswahl.SelectedValue = "Grosskunde")
                Case "B"c
                    doEnable = True
            End Select

            trGutachten.Visible = doEnable
            trPrueforganisation.Visible = doEnable
            trArtGenehmigung.Visible = doEnable
            trGutachtenNr.Visible = doEnable

            lblPrueforganisationInfo.Text = String.Empty
            txtGutachtenNr.Text = String.Empty
            lblGutachtenNrInfo.Text = String.Empty
            ddlPrueforganisation.SelectedIndex = 0

            'Aktuelle Sichtbarkeit der Felder merken, da beim Speichern nicht mehr sichtbar
            mBeauftragung.PrueforganisationSpeichern = doEnable
            mBeauftragung.ArtGenehmigungSpeichern = doEnable
            mBeauftragung.GutachtenNrSpeichern = doEnable
            Session("mBeauftragung2") = mBeauftragung
        End Sub

        Private Sub EnableAltKennzeichen()
            Dim doEnable As Boolean = False

            Select Case mBeauftragung.AltKennzeichenNeeded
                Case "P"c
                    doEnable = True
                Case "O"c
                    doEnable = True
            End Select

            trKennzAlt.Visible = doEnable

            If doEnable And (mBeauftragung.AltKennzeichenNeeded = "P"c Or mBeauftragung.AltKennzeichenNeeded = "O"c) Then
                txtKennzAlt1.Text = ddlStva.SelectedValue
            Else
                txtKennzAlt1.Text = String.Empty
            End If
            txtKennzAlt2.Text = String.Empty
            lblKennzeichenAltInfo.Text = String.Empty

            'Aktuelle Sichtbarkeit des Feldes merken, da beim Speichern nicht mehr sichtbar
            mBeauftragung.AltkennzeichenSpeichern = doEnable
            Session("mBeauftragung2") = mBeauftragung
        End Sub

        Private Sub EnableZusammenfassungControls()
            Dim sepa As Boolean = mBeauftragung.SEPA

            trZusBankdatenTitle.Visible = sepa
            lblZusIBAN.Visible = sepa
            lblZusIBANData.Visible = sepa
            lblZusSWIFT.Visible = sepa
            lblZusSWIFTData.Visible = sepa

            lblZusIBANData.Text = String.Empty
            lblZusSWIFTData.Text = String.Empty
        End Sub

        Private Sub ChangeTab(ByVal targetTab As NextTab, Optional ByVal saveData As Boolean = False)

            If FormValidation() Then

                If saveData Then
                    DatenSichern()
                End If

                mNextTab = targetTab
                Session("NextTab") = mNextTab

                If Grunddaten.Visible AndAlso mBeauftragung.AgeConfirmationRequired Then
                    mpeAgeWarning.Show()
                ElseIf Fahrzeugdaten.Visible AndAlso mBeauftragung.VinConfirmationRequired Then
                    mpeVinWarning.Show()
                Else
                    SelectNextTab()
                End If

            End If

        End Sub

#End Region

    End Class

End Namespace
