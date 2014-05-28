Imports System.IO
Imports CKG.Base.Kernel
Imports CKG.Base.Kernel.Common.Common
Imports CKG.Base.Kernel.Security
Imports Telerik.Web.UI


Namespace Logistik

    Partial Public Class Change02s
        Inherits System.Web.UI.Page

        Private m_App As App
        Private m_User As User



        'Achtung Nur zu Testzwecken die Variable 'useLocalTemppath' auf true setzen
        'Hochgeladene Dokumente werden dann local auf D:\Dokumente gespeichert
        Private useLocalTemppath As Boolean = False

        Const LENGTH_TEXT As Integer = 200


#Region "General"

        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            ' Remove Session-Object, if of wrong type
            If Not Session("Transfer") Is Nothing AndAlso Not TypeOf Session("Transfer") Is Transfer Then
                Session.Remove("Transfer")
            End If

            ScriptManager.RegisterClientScriptBlock(Page, GetType(Report03), "async_upload", "function onUploadFailed(sender, args) { " & _
                                            "alert(args.get_message()); " & _
                                            "} " & _
                                            "function onFileUploaded(sender, args) { " & _
                                            "document.getElementById('" & submitFiles.ClientID & "').click(); }", True)

            m_User = GetUser(Me)
            FormAuth(Me, m_User)
            m_App = New App(m_User)

            'Prüfen, ob auf ein div geklickt wurde
            CheckRequest()


            VersandTabPanel1.Style.Add("height", "600px")
            VersandTabPanel2.Style.Add("height", "600px")
            VersandTabPanel3.Style.Add("height", "600px")
            VersandTabPanel4.Style.Add("height", "600px")


            If Not IsPostBack Then
                GetAppIDFromQueryString(Me)
                If Not Session("Transfer") Is Nothing Then

                    If CType(Session("Transfer"), Transfer).ReUseAdresses = True Then
                        RefillAbholAndZiel()
                    End If

                End If

                Session("Transfer") = Nothing
                InitClass()
                FillTables()
                FillFahrzeugwert()
                FillPartner()

                If Debugger.IsAttached Then
                    TestdatenStamm()
                    TestdatenFahrt()
                    ibtnCreatePDF.Visible = True
                    lblPDFPrint.Visible = True
                End If
            Else

                lblErrorDienst.Visible = False

                If CheckMaxLength() = True Then
                    lblErrorDienst.Visible = True
                    Exit Sub
                Else
                    SaveBemerkungen()
                End If
            End If

        End Sub

        Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
            SetEndASPXAccess(Me)
        End Sub

        Private Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Unload
            SetEndASPXAccess(Me)
        End Sub

        Private Sub InitClass()

            Dim ct As New Transfer(m_User, m_App, "", "", "")

            ct.InitTables(m_User, Me.Page)
            ct.FillPartner(m_User, Me.Page, getKundennummer())

            Session.Add("Transfer", ct)

            ct = Nothing

            ddlAbUhrzeit.DataSource = TimeRange
            ddlAbUhrzeit.DataValueField = "ID"
            ddlAbUhrzeit.DataTextField = "Range"
            ddlAbUhrzeit.DataBind()

            ddlZielUhrzeit.DataSource = TimeRange
            ddlZielUhrzeit.DataValueField = "ID"
            ddlZielUhrzeit.DataTextField = "Range"
            ddlZielUhrzeit.DataBind()

            ddlRuZielUhrzeit.DataSource = TimeRange
            ddlRuZielUhrzeit.DataValueField = "ID"
            ddlRuZielUhrzeit.DataTextField = "Range"
            ddlRuZielUhrzeit.DataBind()

            ddlZuUhrzeit.DataSource = TimeRange
            ddlZuUhrzeit.DataValueField = "ID"
            ddlZuUhrzeit.DataTextField = "Range"
            ddlZuUhrzeit.DataBind()

            ddlRueckZuUhrzeit.DataSource = TimeRange
            ddlRueckZuUhrzeit.DataValueField = "ID"
            ddlRueckZuUhrzeit.DataTextField = "Range"
            ddlRueckZuUhrzeit.DataBind()



        End Sub

        Private Sub FillFahrzeugwert()

            With drpFahrzeugwert
                .Items.Add(New ListItem("Bitte auswählen", "0"))
                .Items.Add(New ListItem("...bis  50  Tsd. €", "Z00"))
                .Items.Add(New ListItem("...bis 150  Tsd. €", "Z50"))
            End With

            drpFahrzeugwert.SelectedIndex = 1

            With drpRuFahrzeugwert
                .Items.Add(New ListItem("Bitte auswählen", "0"))
                .Items.Add(New ListItem("...bis  50  Tsd. €", "Z00"))
                .Items.Add(New ListItem("...bis 150  Tsd. €", "Z50"))
            End With

            drpRuFahrzeugwert.SelectedIndex = 1


        End Sub

        Private Sub SetErrorFrame(ByVal ctrl As Control)

            Dim txt As TextBox
            Dim rdb As RadioButtonList

            If TypeOf ctrl Is TextBox Then
                txt = ctrl
                'txt.BorderWidth = 1
                txt.BorderColor = Drawing.Color.Red
            ElseIf TypeOf ctrl Is RadioButtonList Then
                rdb = ctrl
                rdb.BorderStyle = BorderStyle.Solid
                rdb.BorderWidth = 1
                rdb.BorderColor = Drawing.Color.Red
            End If

        End Sub

        Private Sub ResetErrorFrame(ByVal ctrl As Control)

            Dim txt As TextBox
            Dim rdb As RadioButtonList

            If TypeOf ctrl Is TextBox Then
                txt = ctrl
                'txt.BorderWidth = 0
                txt.BorderColor = Drawing.Color.Empty
            ElseIf TypeOf ctrl Is RadioButtonList Then
                rdb = ctrl
                rdb.BorderStyle = BorderStyle.None
                rdb.BorderWidth = 0
                rdb.BorderColor = Drawing.Color.Empty
            End If
        End Sub

#End Region

#Region "Stammdaten"

        Protected Sub lbtnNext_Click(ByVal sender As Object, ByVal e As EventArgs) Handles ibtFahrzeug1.Click, _
                                                                                    ibtFahrzeug2.Click, _
                                                                                    ibtRechnungszahler.Click, _
                                                                                    ibtAbwRechnungsadresse.Click



            If CheckStammdaten() = False Then

                WriteData() 'Daten in der Klasse Transfer speichern

                Dim ct As Transfer = CType(Session("Transfer"), Transfer)

                With ct.Fahrzeuge

                    lblAbDetailKennzeichen.Text = .Rows(0)("ZZKENN")
                    lblAbDetailTyp.Text = .Rows(0)("ZZFAHRZGTYP")
                    lblAbDetailFin.Text = .Rows(0)("ZZFAHRG")

                    lblDetailKennzeichen.Text = .Rows(0)("ZZKENN")
                    lblDetailTyp.Text = .Rows(0)("ZZFAHRZGTYP")
                    lblDetailVin.Text = .Rows(0)("ZZFAHRG")

                    lblZielDetailKennzeichen.Text = .Rows(0)("ZZKENN")
                    lblZielDetailTyp.Text = .Rows(0)("ZZFAHRZGTYP")
                    lblZielDetailFin.Text = .Rows(0)("ZZFAHRG")


                End With

                Dim SelRow As DataRow()

                SelRow = ct.Fahrzeuge.Select("FAHRZEUG = '2'")

                If SelRow.Length > 0 Then

                    trRueck.Visible = True
                    trRueckZiel.Visible = True
                    divRZusatz.Visible = True

                    lblRuDeKennzeichen.Text = SelRow(0)("ZZKENN")
                    lblRuDeTyp.Text = SelRow(0)("ZZFAHRZGTYP")
                    lblRuDeFin.Text = SelRow(0)("ZZFAHRG")

                    'Anzeige in Zusatzfahrt
                    lblRuDetailKennzeichen.Text = SelRow(0)("ZZKENN")
                    lblRuDetailTyp.Text = SelRow(0)("ZZFAHRZGTYP")
                    lblRuDetailFin.Text = SelRow(0)("ZZFAHRG")



                End If


                lblSteps.Text = "Schritt 2 von 4"
                Panel1.CssClass = "StepActive"
                Panel2.CssClass = "StepActive"
                Panel3.CssClass = "Steps"
                Panel4.CssClass = "Steps"


                VersandTabPanel1.Visible = False
                VersandTabPanel2.Visible = True
                LinkButton1.Enabled = True
                LinkButton1.CssClass = "VersandButtonStammReady"
                LinkButton2.CssClass = "LogistikButtonTour"

                CloseDlPanels()
                CloseFahrtenPanels()


                LoadFahrten()

                pnlAbholadresse.Visible = True
                ibtAbholadresseHeaderClose.Visible = True
                ibtAbholadresseHeader.Visible = False
            Else
                lblErrorStamm.Visible = True

            End If

            'CheckFahrzeugStandort()

        End Sub

        Private Sub FillPartner()


            Dim ct As Transfer = CType(Session("Transfer"), Transfer)

            Dim dv As DataView = ct.Partner.DefaultView


            dv.RowFilter = "PARVW = 'RG'"

            Dim e As Integer

            e = 0


            With ddlPartnerRG

                .Items.Add(New ListItem("Bitte auswählen", "0"))

                Do While e < dv.Count

                    .Items.Add(New ListItem(dv.Item(e)("NAME1") & ", " & dv.Item(e)("CITY1"), dv.Item(e)("KUNNR")))

                    If dv.Item(e)("DEFPA") = "X" Then

                        .Items(e + 1).Selected = True

                        Me.txtRzAnsprechpartner.Text = dv.Item(e)("NAME2")
                        Me.txtRzFirma.Text = dv.Item(e)("NAME1")
                        Me.txtRzOrt.Text = dv.Item(e)("CITY1")
                        Me.txtRzPLZ.Text = dv.Item(e)("POST_CODE1")
                        Me.txtRzStrasse.Text = dv.Item(e)("STREET") & " " & dv.Item(e)("HOUSE_NUM1")
                        Me.txtRzTelefon.Text = dv.Item(e)("TEL_NUMBER")
                    End If

                    e = e + 1
                Loop


            End With


            dv.RowFilter = "PARVW = 'RE'"

            e = 0

            With ddlPartnerRE


                .Items.Add(New ListItem("Bitte auswählen", "0"))

                Do While e < dv.Count

                    .Items.Add(New ListItem(dv.Item(e)("NAME1") & ", " & dv.Item(e)("CITY1"), dv.Item(e)("KUNNR")))

                    If dv.Item(e)("DEFPA") = "X" Then

                        .Items(e + 1).Selected = True

                        Me.txtArAnsprechpartner.Text = dv.Item(e)("NAME2")
                        Me.txtArFirma.Text = dv.Item(e)("NAME1")
                        Me.txtArOrt.Text = dv.Item(e)("CITY1")
                        Me.txtArPLZ.Text = dv.Item(e)("POST_CODE1")
                        Me.txtArStrasse.Text = dv.Item(e)("STREET") & " " & dv.Item(e)("HOUSE_NUM1")
                        Me.txtArTelefon.Text = dv.Item(e)("TEL_NUMBER")
                    End If

                    e = e + 1
                Loop


            End With



        End Sub


        Protected Sub ddlPartnerRE_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ddlPartnerRE.SelectedIndexChanged
            Dim ct As Transfer = CType(Session("Transfer"), Transfer)



            Dim dv As DataView = ct.Partner.DefaultView


            If ddlPartnerRE.SelectedItem.Value = "0" Then
                Me.txtArAnsprechpartner.Text = ""
                Me.txtArFirma.Text = ""
                Me.txtArOrt.Text = ""
                Me.txtArPLZ.Text = ""
                Me.txtArStrasse.Text = ""
                Me.txtArTelefon.Text = ""
                ct.RE = ""
            Else
                dv.RowFilter = "PARVW = 'RE' AND KUNNR = '" & ddlPartnerRE.SelectedItem.Value() & "'"
                Me.txtArAnsprechpartner.Text = dv.Item(0)("NAME2")
                Me.txtArFirma.Text = dv.Item(0)("NAME1")
                Me.txtArOrt.Text = dv.Item(0)("CITY1")
                Me.txtArPLZ.Text = dv.Item(0)("POST_CODE1")
                Me.txtArStrasse.Text = dv.Item(0)("STREET") & " " & dv.Item(0)("HOUSE_NUM1")
                Me.txtArTelefon.Text = dv.Item(0)("TEL_NUMBER")
                ct.RE = ddlPartnerRE.SelectedItem.Value
            End If

            Session("Transfer") = ct
            ct = Nothing
        End Sub

        Protected Sub ddlPartnerRG_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ddlPartnerRG.SelectedIndexChanged
            Dim ct As Transfer = CType(Session("Transfer"), Transfer)



            Dim dv As DataView = ct.Partner.DefaultView


            If ddlPartnerRG.SelectedItem.Value = "0" Then
                Me.txtRzAnsprechpartner.Text = ""
                Me.txtRzFirma.Text = ""
                Me.txtRzOrt.Text = ""
                Me.txtRzPLZ.Text = ""
                Me.txtRzStrasse.Text = ""
                Me.txtRzTelefon.Text = ""
                ct.RG = ""
            Else
                dv.RowFilter = "PARVW = 'RG' AND KUNNR = '" & ddlPartnerRG.SelectedItem.Value() & "'"
                Me.txtRzAnsprechpartner.Text = dv.Item(0)("NAME2")
                Me.txtRzFirma.Text = dv.Item(0)("NAME1")
                Me.txtRzOrt.Text = dv.Item(0)("CITY1")
                Me.txtRzPLZ.Text = dv.Item(0)("POST_CODE1")
                Me.txtRzStrasse.Text = dv.Item(0)("STREET") & " " & dv.Item(0)("HOUSE_NUM1")
                Me.txtRzTelefon.Text = dv.Item(0)("TEL_NUMBER")
                ct.RG = ddlPartnerRG.SelectedItem.Value
            End If

            Session("Transfer") = ct

            ct = Nothing

        End Sub

        Private Function CheckStammdaten() As Boolean
            Dim booError As Boolean = False
            Dim booErrorRueck As Boolean = False

            ResetStamm()

            'Fahrzeugdaten
            If txtFahrgestellnummer.Text.Length = 0 Then
                SetErrorFrame(txtFahrgestellnummer) : booError = True
            End If

            If txtTyp.Text.Length = 0 Then
                SetErrorFrame(txtTyp) : booError = True
            End If

            If rblZugelassen.SelectedValue = "" Then
                SetErrorFrame(rblZugelassen) : booError = True
            End If

            If rblBeauftragt.SelectedValue = "" Then
                SetErrorFrame(rblBeauftragt) : booError = True
            End If

            If rblBereifung.SelectedValue = "" Then
                SetErrorFrame(rblBereifung) : booError = True
            End If


            If rblFahrzeugklasse.SelectedValue = "" Then
                SetErrorFrame(rblFahrzeugklasse) : booError = True
            End If

            If drpFahrzeugwert.SelectedItem.Value = "0" Then
                divFahrzeugwert.Style.Add("border", "solid 1px red") : booError = True
            End If

            If booError = True Then
                ResetPanels()
                cpeAllData.ClientState = False
                Return booError
            End If

            'Rückholung
            'ITA 5283 - Auf fehlende Felder hinweisen, wenn z.B. nur Kennzeichen angegeben
            If txtRuFahrgestellnummer.Text.Length > 0 OrElse txtRuKennzeichen1.Text.Length > 0 OrElse txtRuTyp.Text.Length > 0 Then

                If txtRuFahrgestellnummer.Text.Length = 0 Then
                    SetErrorFrame(txtRuFahrgestellnummer) : booError = True : booErrorRueck = True
                End If

                If txtRuTyp.Text.Length = 0 Then
                    SetErrorFrame(txtRuTyp) : booError = True : booErrorRueck = True
                End If

                If rblRuZugelassen.SelectedValue = "" Then
                    SetErrorFrame(rblRuZugelassen) : booError = True : booErrorRueck = True
                End If

                If rblRuBeauftragt.SelectedValue = "" Then
                    SetErrorFrame(rblRuBeauftragt) : booError = True : booErrorRueck = True
                End If

                If rblRuBereifung.SelectedValue = "" Then
                    SetErrorFrame(rblRuBereifung) : booError = True : booErrorRueck = True
                End If


                If rblRuFahrzeugklasse.SelectedValue = "" Then
                    SetErrorFrame(rblRuFahrzeugklasse) : booError = True : booErrorRueck = True
                End If

                If drpRuFahrzeugwert.SelectedItem.Value = "0" Then
                    divRuFahrzeugwert.Style.Add("border", "solid 1px red") : booError = True : booErrorRueck = True
                End If

                If booErrorRueck = True Then
                    ResetPanels()
                    cpeRueckholung.ClientState = False
                    Return booErrorRueck
                End If


            End If

            'Rechnungszahler


            If ddlPartnerRG.SelectedItem.Value = "0" Then
                divRG.Style.Add("border", "solid 1px red") : booError = True
                ResetPanels()
                cpeRechnungszahler.ClientState = False
                Return booError
            End If

            'Abweichende Rechnungsadresse
            If ddlPartnerRE.SelectedItem.Value = "0" Then
                divRE.Style.Add("border", "solid 1px red") : booError = True
                ResetPanels()
                cpeAbwRechnungsadresse.ClientState = False
            End If


            Return booError

        End Function



        Private Sub ResetPanels() 'Alle Panels schliessen

            cpeAllData.ClientState = True
            cpeAbwRechnungsadresse.ClientState = True
            cpeRechnungszahler.ClientState = True
            cpeRueckholung.ClientState = True

        End Sub

        Private Sub ResetStamm()

            lblErrorStamm.Visible = False

            ResetErrorFrame(txtFahrgestellnummer)
            ResetErrorFrame(txtTyp)
            ResetErrorFrame(rblZugelassen)
            ResetErrorFrame(rblBeauftragt)
            ResetErrorFrame(rblBereifung)
            ResetErrorFrame(rblFahrzeugklasse)
            divFahrzeugwert.Style.Remove("border")
            ResetErrorFrame(txtRuTyp)
            ResetErrorFrame(rblRuZugelassen)
            ResetErrorFrame(rblRuBeauftragt)
            ResetErrorFrame(rblRuBereifung)
            ResetErrorFrame(rblRuFahrzeugklasse)
            divRuFahrzeugwert.Style.Remove("border")
            divRG.Style.Remove("border")
            divRE.Style.Remove("border")

        End Sub

        Private Sub WriteData()
            Dim ct As Transfer = CType(Session("Transfer"), Transfer)
            Dim FahrzeugRow As DataRow

            ct.RG = ddlPartnerRG.SelectedItem.Value
            ct.RE = ddlPartnerRE.SelectedItem.Value

            If ct.Fahrzeuge.Rows.Count > 0 Then
                ct.Fahrzeuge.Clear()
            End If



            FahrzeugRow = ct.Fahrzeuge.NewRow

            FahrzeugRow("FAHRZEUG") = "1"
            FahrzeugRow("ZZFAHRZGTYP") = txtTyp.Text
            FahrzeugRow("ZZKENN") = txtKennzeichen1.Text
            FahrzeugRow("FZGART") = rblFahrzeugklasse.SelectedValue
            FahrzeugRow("ZULGE") = rblZugelassen.SelectedValue
            FahrzeugRow("ZUL_BEI_CK_DAD") = rblBeauftragt.SelectedValue
            FahrzeugRow("SOWI") = rblBereifung.SelectedValue
            FahrzeugRow("AUGRU") = drpFahrzeugwert.SelectedValue
            FahrzeugRow("ZZREFNR") = txtReferenznummer.Text
            FahrzeugRow("ZZFAHRG") = txtFahrgestellnummer.Text


            ct.Fahrzeuge.Rows.Add(FahrzeugRow)


            If txtRuFahrgestellnummer.Text.Length > 0 Then

                FahrzeugRow = ct.Fahrzeuge.NewRow

                FahrzeugRow("FAHRZEUG") = "2"
                FahrzeugRow("ZZFAHRZGTYP") = txtRuTyp.Text
                FahrzeugRow("ZZKENN") = txtRuKennzeichen1.Text
                FahrzeugRow("FZGART") = rblRuFahrzeugklasse.SelectedValue
                FahrzeugRow("ZULGE") = rblRuZugelassen.SelectedValue
                FahrzeugRow("ZUL_BEI_CK_DAD") = rblRuBeauftragt.SelectedValue
                FahrzeugRow("SOWI") = rblRuBereifung.SelectedValue
                FahrzeugRow("AUGRU") = drpRuFahrzeugwert.SelectedValue
                FahrzeugRow("ZZREFNR") = txtRuReferenznummer.Text
                FahrzeugRow("ZZFAHRG") = txtRuFahrgestellnummer.Text

                ct.Fahrzeuge.Rows.Add(FahrzeugRow)

            Else
                'Gibt es noch Fahrten?
                For Each Row As DataRow In ct.Fahrten.Rows

                    'Dienstleistungen und Bemerkungen löschen
                    If Row("FAHRZEUG") = "2" Then
                        ct.Dienstleistungen.DefaultView.RowFilter = "FAHRT <> '" & Row("FAHRT") & "'"
                        ct.Dienstleistungen = ct.Dienstleistungen.DefaultView.ToTable
                        ct.Dienstleistungen.AcceptChanges()

                        ct.Bemerkungen.DefaultView.RowFilter = "FAHRT <> '" & Row("FAHRT") & "'"
                        ct.Bemerkungen = ct.Bemerkungen.DefaultView.ToTable
                        ct.Bemerkungen.AcceptChanges()

                    End If
                Next

                ct.Fahrten.DefaultView.RowFilter = "FAHRZEUG <> '2'"
                ct.Fahrten = ct.Fahrten.DefaultView.ToTable
                ct.Fahrten.AcceptChanges()

                ct.Fahrzeuge.DefaultView.RowFilter = "FAHRZEUG <> '2'"
                ct.Fahrzeuge = ct.Fahrzeuge.DefaultView.ToTable
                ct.Fahrzeuge.AcceptChanges()

            End If


            ct.Fahrzeuge.AcceptChanges()

            Session("Transfer") = ct
        End Sub

        'Protected Sub ibtRZSave_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtRZSave.Click
        '    SaveRueckZusatzfahrt()
        'End Sub


        Protected Sub ibtRZOpen_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtRZOpen.Click
            OpenRueckZusatzfahrt()
        End Sub

        Protected Sub ibtRZClose_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtRZClose.Click
            CloseRueckZusatzfahrt()
        End Sub

#End Region

#Region "Fahrten"

        Private Sub FahrtenWeiter()
            If CheckFahrten() = False Then

                cmdJaWarnung.Visible = False
                cmdNeinWarnung.Visible = False
                cmdOKWarnung.Visible = True

                Dim booIsSunday As Boolean = False

                If IsDate(txtAbDatum.Text) = True Then
                    If CDate(txtAbDatum.Text).DayOfWeek = DayOfWeek.Sunday Then booIsSunday = True
                End If

                If IsDate(txtZielDatum.Text) = True Then
                    If CDate(txtZielDatum.Text).DayOfWeek = DayOfWeek.Sunday Then booIsSunday = True
                End If

                If IsDate(txtRuZielDatum.Text) = True Then
                    If CDate(txtRuZielDatum.Text).DayOfWeek = DayOfWeek.Sunday Then booIsSunday = True
                End If


                If booIsSunday = True Then
                    lblMsgHeader.Text = "Wunschtermin ist ein Sonntag"
                    litMessage.Text = "Sie haben einen Sonntag als Wunschtermin eingegeben.<br />Bitte prüfen Sie die Daten."
                    divMessage.Visible = True
                    Exit Sub
                End If



                If CheckDates() = True Then
                    'lblErrorFahrten.Text = "Bitte geben Sie für jede Fahrt ein Datum an, oder lassen Sie alle frei."
                    'lblErrorFahrten.Visible = True
                    lblMsgHeader.Text = "Datumsfelder"
                    litMessage.Text = "Bitte geben Sie für jede Fahrt ein Datum an, oder lassen Sie alle frei."
                    divMessage.Visible = True
                    Exit Sub
                End If
                If CheckDates2() = True Then
                    'lblErrorFahrten.Text = "Bitte beachten Sie! Der Termin der ersten Abholung darf nicht größer als die folgenden Termine sein."
                    'lblErrorFahrten.Visible = True
                    lblMsgHeader.Text = "Abholtermin"
                    litMessage.Text = "Bitte beachten Sie! Der Termin der ersten Abholung darf nicht größer als die folgenden Termine sein."
                    divMessage.Visible = True
                    Exit Sub
                End If
                If CheckDates3() = True Then
                    lblMsgHeader.Text = "Wunschdatum"
                    litMessage.Text = "Bitte beachten Sie! Ein Wunschdatum darf nicht in der Vergangenheit liegen."
                    divMessage.Visible = True
                    Exit Sub
                End If

                If pnlZusatzfahrten.Visible = True Then

                    If CheckZusatz() = False Then
                        'lblErrorFahrten.Visible = True
                        'lblErrorFahrten.Text = "Bitte bestätigen Sie Ihre Zusatzfahrt, oder schließen Sie diese."
                        litMessage.Text = "Bitte bestätigen Sie Ihre Zusatzfahrt, oder schließen Sie diese."
                    Else
                        'lblErrorFahrten.Text = "Zusatzfahrt: Bitte überprüfen Sie Ihre Eingaben."
                        litMessage.Text = "Zusatzfahrt: Bitte überprüfen Sie Ihre Eingaben."
                    End If

                    'lblErrorFahrtBottom.Text = lblErrorFahrten.Text
                    lblMsgHeader.Text = "Zusatzfahrt"

                    divMessage.Visible = True


                    Exit Sub

                End If

                If pnlRZusatzfahrten.Visible = True Then

                    If CheckRueckZusatz() = False Then

                        lblErrorFahrten.Visible = True
                        lblErrorFahrten.Text = "Bitte bestätigen Sie Ihre Zusatzfahrt, oder schließen Sie diese."
                    Else
                        lblErrorFahrten.Text = "Zusatzfahrt: Bitte überprüfen Sie Ihre Eingaben."
                    End If

                    lblErrorFahrtBottom.Text = lblErrorFahrten.Text

                    Exit Sub

                End If


                txtAbBemerkung.Attributes.Add("style", "overflow :hidden")
                txtBemerkungZusatz.Attributes.Add("style", "overflow :hidden")
                txtRueckBemerkung.Attributes.Add("style", "overflow :hidden")
                txtRueckBemZusatz.Attributes.Add("style", "overflow :hidden")

                WriteDataAbholUndZieladresse()

                If lb_Kilometer.Visible = True Then
                    SetEntfernungen()
                End If


                VersandTabPanel1.Visible = False
                VersandTabPanel2.Visible = False
                VersandTabPanel3.Visible = True

                LinkButton1.CssClass = "VersandButtonStammReady"
                LinkButton2.CssClass = "LogistikButtonTourReady"
                LinkButton2.Enabled = True
                LinkButton3.CssClass = "LogistikButtonDL"

                lblSteps.Text = "Schritt 3 von 4"
                Panel1.CssClass = "StepActive"
                Panel2.CssClass = "StepActive"
                Panel3.CssClass = "StepActive"

                Dim ct As Transfer = CType(Session("Transfer"), Transfer)


                Dim Fahrt As String

                Fahrt = ct.Fahrten.Select("FAHRT <> '0' and KENNZ_ZUS_FAHT <> 'X'")(0)("FAHRT")

                Dim AdFirma As String

                AdFirma = ct.Adressen.Select("FAHRT = '" & Fahrt & "'")(0)("NAME").ToString & vbCrLf
                AdFirma = AdFirma & ct.Adressen.Select("FAHRT = '" & Fahrt & "'")(0)("STREET").ToString & vbCrLf
                AdFirma = AdFirma & ct.Adressen.Select("FAHRT = '" & Fahrt & "'")(0)("POSTL_CODE").ToString & " " & ct.Adressen.Select("FAHRT = '" & Fahrt & "'")(0)("CITY").ToString & vbCrLf
                AdFirma = AdFirma & "Telefon: " & ct.Adressen.Select("FAHRT = '" & Fahrt & "'")(0)("TELEPHONE").ToString

                ImageButton12.ToolTip = AdFirma

                If ct.ProtokollArten Is Nothing Then ct.getProtokollarten(m_User, Me.Page, getKundennummer())

                If ct.ProtokollArten Is Nothing Then
                    ibtnProtokollUpload1.Visible = False
                    lblProtokollUpload1.Visible = False
                ElseIf ct.ProtokollArten.Rows.Count = 0 Then
                    ibtnProtokollUpload1.Visible = False
                    lblProtokollUpload1.Visible = False
                Else
                    If ct.ProtokollArten.Select("Fahrt='" & Fahrt & "'").Length = 0 Then
                        For Each dRow As DataRow In ct.ProtokollArten.Rows
                            dRow("Fahrt") = Fahrt
                            ct.ProtokollArten.AcceptChanges()
                        Next
                    End If

                    FillGridProtokolle(ct.ProtokollArten)
                End If

                If ct.Fahrzeuge.Select("FAHRZEUG = '2'").Length > 0 Then
                    tblRueckDL.Visible = True
                    FillRueckGridZusatz(True)

                    Fahrt = ct.Fahrten.Select("FAHRZEUG = '2' and KENNZ_ZUS_FAHT <> 'X'")(0)("FAHRT")

                    Dim RuAdFirma As String

                    RuAdFirma = ct.Adressen.Select("FAHRT = '" & Fahrt & "'")(0)("NAME").ToString & vbCrLf
                    RuAdFirma = RuAdFirma & ct.Adressen.Select("FAHRT = '" & Fahrt & "'")(0)("STREET").ToString & vbCrLf
                    RuAdFirma = RuAdFirma & ct.Adressen.Select("FAHRT = '" & Fahrt & "'")(0)("POSTL_CODE").ToString & " " & ct.Adressen.Select("FAHRT = '" & Fahrt & "'")(0)("CITY").ToString & vbCrLf
                    RuAdFirma = RuAdFirma & "Telefon: " & ct.Adressen.Select("FAHRT = '" & Fahrt & "'")(0)("TELEPHONE").ToString

                    ImageButton9.ToolTip = RuAdFirma

                    If ct.ProtokollArten Is Nothing Then
                        ibtnProtokollUpload2.Visible = False
                        lblProtokollUpload2.Visible = False
                    ElseIf ct.ProtokollArten.Rows.Count = 0 Then
                        ibtnProtokollUpload2.Visible = False
                        lblProtokollUpload2.Visible = False
                    Else
                        If ct.ProtokollArten.Select("Fahrt='" & Fahrt & "'").Length = 0 Then
                            Dim RowNew As DataRow
                            Dim dRow(ct.ProtokollArten.Rows.Count - 1) As DataRow
                            ct.ProtokollArten.Rows.CopyTo(dRow, 0)
                            For Each RowTemp As DataRow In dRow

                                RowNew = ct.ProtokollArten.NewRow
                                RowNew("ID") = ct.ProtokollArten.Rows.Count + 1
                                RowNew("ZZKUNNR") = RowTemp("ZZKUNNR")
                                RowNew("ZZKATEGORIE") = RowTemp("ZZKATEGORIE")
                                RowNew("ZZPROTOKOLLART") = RowTemp("ZZPROTOKOLLART")
                                RowNew("Filename") = ""
                                RowNew("Filepath") = ""
                                RowNew("Fahrt") = Fahrt
                                ct.ProtokollArten.Rows.Add(RowNew)
                            Next
                        End If
                        FillGridProtokolle2(ct.ProtokollArten)
                    End If



                End If


                PrepareDienstleistungen()

                CloseFahrtenPanels()

                FillGridZusatz(True)

                Session("Transfer") = ct

                ct = Nothing





            End If
        End Sub


        'Protected Sub lbtFartenWeiter_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lbtFartenWeiter.Click
        '    FahrtenWeiter()
        'End Sub


        Private Sub ibtCloseZusatzfahrt_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtCloseZusatzfahrt.Click
            CloseZusatzfahrt()
            lblZusatz.Text = "Zusatzfahrt hinzufügen"
        End Sub

        'Protected Sub lbtFahrtenBack_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lbtFahrtenBack.Click
        '    VersandTabPanel1.Visible = True
        '    VersandTabPanel2.Visible = False
        '    LinkButton2.CssClass = "LogistikButtonTourDisabled"
        'End Sub

        Private Sub PrepareGrid()


            Dim Last As Integer


            Last = (grvZusatz.Rows.Count - 1)


            If grvZusatz.Rows.Count = 1 Then
                CType(grvZusatz.Rows(0).FindControl("ibtArrowUp"), ImageButton).Visible = False
                CType(grvZusatz.Rows(0).FindControl("ibtArrowDown"), ImageButton).Visible = False
                Exit Sub
            End If

            'Erster Eintrag
            CType(grvZusatz.Rows(0).FindControl("ibtArrowUp"), ImageButton).Visible = False

            'Letzter Eintrag
            CType(grvZusatz.Rows(Last).FindControl("ibtArrowDown"), ImageButton).Visible = False

        End Sub

        Private Sub grvZusatz_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles grvZusatz.RowCommand

            Dim ct As Transfer = CType(Session("Transfer"), Transfer)

            Dim lbl As Label

            lbl = CType(grvZusatz.Rows(CInt(e.CommandArgument)).FindControl("ID"), Label)

            Select Case e.CommandName
                Case "arrowup"
                    ChangeSortOrder(CInt(lbl.Text), "Up")
                Case "arrowdown"
                    ChangeSortOrder(CInt(lbl.Text), "Down")
                Case "Del"
                    'Fahrten
                    ct.Fahrten.Rows(CInt(lbl.Text)).Delete()
                    ct.Fahrten.AcceptChanges()
                    'Adressen
                    ct.Adressen.Rows(CInt(lbl.Text)).Delete()
                    ct.Adressen.AcceptChanges()
                    'Dienstleistungen
                    If ct.Dienstleistungen.Select("FAHRT = '" & lbl.Text & "'").Length > 0 Then
                        ct.Dienstleistungen.Rows(CInt(lbl.Text)).Delete()
                        ct.Dienstleistungen.AcceptChanges()
                    End If

                    'Bemerkungen
                    If ct.Bemerkungen.Select("FAHRT = '" & lbl.Text & "'").Length > 0 Then
                        ct.Bemerkungen.Rows(CInt(lbl.Text)).Delete()
                        ct.Bemerkungen.AcceptChanges()
                    End If


                    'Fahrten neu aufbauen
                    For i = 0 To ct.Fahrten.Rows.Count - 1

                        ct.Fahrten.Rows(i)("FAHRT") = (i).ToString
                        ct.Fahrten.Rows(i)("REIHENFOLGE") = (i).ToString
                        ct.Adressen.Rows(i)("FAHRT") = (i).ToString

                    Next

                    'Dienstleisungen anpassen
                    For Each Row As DataRow In ct.Dienstleistungen.Rows

                        Row("FAHRT") = (CInt(Row("FAHRT") - 1)).ToString
                        ct.Dienstleistungen.AcceptChanges()
                    Next

                    'Bemerkungen anpassen
                    For Each Row As DataRow In ct.Bemerkungen.Rows

                        Row("FAHRT") = (CInt(Row("FAHRT") - 1)).ToString
                        ct.Bemerkungen.AcceptChanges()
                    Next


                    Session("Transfer") = ct

                    FillGridZusatz()


                Case "edit"
                    'Info, welche Zeile in der Tabelle geladen werden soll

                    ct.EditFahrt = lbl.Text

                    Session("Transfer") = ct

                    ct = Nothing

                    CloseFahrtenPanels()

                    LoadSingleZusatzfahrt()

            End Select



        End Sub

        Private Sub ChangeSortOrder(ByVal Fahrt As Integer, ByVal Direction As String)

            Dim ct As Transfer = CType(Session("Transfer"), Transfer)

            For i = 0 To ct.Fahrten.Rows.Count - 1

                If Fahrt = i Then

                    Select Case Direction

                        Case "Up"
                            ct.Fahrten.Rows(i)("FAHRT") = (Fahrt - 1).ToString
                            ct.Fahrten.Rows(i)("REIHENFOLGE") = (Fahrt - 1).ToString
                            ct.Fahrten.AcceptChanges()

                            ct.Fahrten.Rows(i - 1)("FAHRT") = (Fahrt).ToString
                            ct.Fahrten.Rows(i - 1)("REIHENFOLGE") = (Fahrt).ToString
                            ct.Fahrten.AcceptChanges()

                            ct.Adressen.Rows(i)("FAHRT") = (Fahrt - 1).ToString
                            ct.Adressen.AcceptChanges()
                            ct.Adressen.Rows(i - 1)("FAHRT") = (Fahrt).ToString
                            ct.Adressen.AcceptChanges()

                            'Dienstleistungen
                            For Each dr As DataRow In ct.Dienstleistungen.Rows

                                If dr("FAHRT") = Fahrt Then
                                    dr("FAHRT") = (Fahrt - 1)
                                    ct.Dienstleistungen.AcceptChanges()
                                Else
                                    If dr("FAHRT") = (Fahrt - 1) Then
                                        dr("FAHRT") = Fahrt
                                        ct.Dienstleistungen.AcceptChanges()
                                    End If
                                End If

                            Next

                            'Bemerkungen
                            For Each dr As DataRow In ct.Bemerkungen.Rows

                                If dr("FAHRT") = Fahrt Then
                                    dr("FAHRT") = (Fahrt - 1)
                                    ct.Bemerkungen.AcceptChanges()
                                Else
                                    If dr("FAHRT") = (Fahrt - 1) Then
                                        dr("FAHRT") = Fahrt
                                        ct.Bemerkungen.AcceptChanges()
                                    End If
                                End If

                            Next


                            Exit For

                        Case "Down"
                            ct.Fahrten.Rows(i)("FAHRT") = (Fahrt + 1).ToString
                            ct.Fahrten.Rows(i)("REIHENFOLGE") = (Fahrt + 1).ToString
                            ct.Fahrten.AcceptChanges()

                            ct.Fahrten.Rows(i + 1)("FAHRT") = (Fahrt).ToString
                            ct.Fahrten.Rows(i + 1)("REIHENFOLGE") = (Fahrt).ToString
                            ct.Fahrten.AcceptChanges()

                            ct.Adressen.Rows(i)("FAHRT") = (Fahrt + 1).ToString
                            ct.Adressen.AcceptChanges()
                            ct.Adressen.Rows(i + 1)("FAHRT") = (Fahrt).ToString
                            ct.Adressen.AcceptChanges()


                            'Dienstleistungen
                            For Each dr As DataRow In ct.Dienstleistungen.Rows

                                If dr("FAHRT") = Fahrt Then
                                    dr("FAHRT") = (Fahrt + 1)
                                    ct.Dienstleistungen.AcceptChanges()
                                Else
                                    If dr("FAHRT") = (Fahrt + 1) Then
                                        dr("FAHRT") = Fahrt
                                        ct.Dienstleistungen.AcceptChanges()
                                    End If
                                End If

                            Next

                            'Bemerkungen
                            For Each dr As DataRow In ct.Bemerkungen.Rows

                                If dr("FAHRT") = Fahrt Then
                                    dr("FAHRT") = (Fahrt + 1)
                                    ct.Bemerkungen.AcceptChanges()
                                Else
                                    If dr("FAHRT") = (Fahrt + 1) Then
                                        dr("FAHRT") = Fahrt
                                        ct.Bemerkungen.AcceptChanges()
                                    End If
                                End If

                            Next



                            Exit For

                    End Select


                End If


            Next


            ct.Fahrten.DefaultView.Sort = "FAHRT ASC"
            ct.Fahrten = ct.Fahrten.DefaultView.ToTable

            ct.Adressen.DefaultView.Sort = "FAHRT ASC"
            ct.Adressen = ct.Adressen.DefaultView.ToTable

            Session("Transfer") = ct

            FillGridZusatz()

        End Sub


        Private Sub grvZusatz_RowEditing(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewEditEventArgs) Handles grvZusatz.RowEditing, grvRZusatz.RowEditing

        End Sub

        Private Sub CloseZusatzfahrt()
            pnlZusatzfahrten.Visible = False
            ibtOpenZusatzfahrt.Visible = True
            ibtCloseZusatzfahrt.Visible = False

            FillGridZusatz()

        End Sub

        Protected Sub ibtOpenZusatzfahrt_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtOpenZusatzfahrt.Click

            OpenZusatzfahrt()

        End Sub

        Private Sub OpenZusatzfahrt()

            lblZusatz.Text = "Zusatzfahrt hinzufügen"

            If CheckFahrten() = True Then
                lblErrorFahrten.Visible = True
                Exit Sub
            Else
                WriteDataAbholUndZieladresse()
            End If

            txtZuFirma.Text = ""
            txtZuStrasse.Text = ""
            txtZuPLZ.Text = ""
            txtZuOrt.Text = ""
            ddlZuLand.SelectedValue = "DE"
            txtZuAnsprechpartner.Text = ""
            txtZuTelefon.Text = ""
            txtZuDatum.Text = ""
            ddlZuTransporttyp.SelectedValue = "00"
            ddlZuUhrzeit.SelectedValue = "0-0"

            If pnlZusatzfahrten.Visible = True Then
                CloseFahrtenPanels()
                FillGridZusatz()
            Else
                CloseFahrtenPanels()
                pnlZusatzfahrten.Visible = True
                ibtOpenZusatzfahrt.Visible = False
                ibtCloseZusatzfahrt.Visible = True
            End If



        End Sub

        Private Function CheckFahrten() As Boolean
            Dim booError As Boolean = False

            Dim ct As Transfer = CType(Session("Transfer"), Transfer)

            ResetFahrten()

            'Abholadresse
            If txtAbFirma.Text.Length = 0 Then
                SetErrorFrame(txtAbFirma) : booError = True
            End If

            If txtAbStrasse.Text.Length = 0 Then
                SetErrorFrame(txtAbStrasse) : booError = True
            End If

            If CInt(ct.Laender.Select("Land1='" & ddlAbLand.SelectedItem.Value & "'")(0)("Lnplz")) > 0 Then
                If Not CInt(ct.Laender.Select("Land1='" & ddlAbLand.SelectedItem.Value & "'")(0)("Lnplz")) = txtAbPLZ.Text.Trim(" "c).Length Then
                    SetErrorFrame(txtAbPLZ) : booError = True
                End If
            End If


            If txtAbPLZ.Text.Length = 0 Then
                SetErrorFrame(txtAbPLZ) : booError = True
            End If

            If txtAbOrt.Text.Length = 0 Then
                SetErrorFrame(txtAbOrt) : booError = True
            End If

            If txtAbAnsprechpartner.Text.Length = 0 Then
                SetErrorFrame(txtAbAnsprechpartner) : booError = True
            End If

            If txtAbTelefon.Text.Length = 0 Then
                SetErrorFrame(txtAbTelefon) : booError = True
            End If


            If booError = True Then
                lblErrorFahrten.Text = "Bitte geben Sie zunächst die Abholadresse ein."
                Return booError
            End If


            'Zieladresse
            If txtZielFirma.Text.Length = 0 Then
                SetErrorFrame(txtZielFirma) : booError = True
            End If

            If txtZielStrasse.Text.Length = 0 Then
                SetErrorFrame(txtZielStrasse) : booError = True
            End If

            If CInt(ct.Laender.Select("Land1='" & ddlZielland.SelectedItem.Value & "'")(0)("Lnplz")) > 0 Then
                If Not CInt(ct.Laender.Select("Land1='" & ddlZielland.SelectedItem.Value & "'")(0)("Lnplz")) = txtZielPLZ.Text.Trim(" "c).Length Then
                    SetErrorFrame(txtZielPLZ) : booError = True
                End If
            End If


            If txtZielPLZ.Text.Length = 0 Then
                SetErrorFrame(txtZielPLZ) : booError = True
            End If

            If txtZielOrt.Text.Length = 0 Then
                SetErrorFrame(txtZielOrt) : booError = True
            End If

            If txtZielAnsprechpartner.Text.Length = 0 Then
                SetErrorFrame(txtZielAnsprechpartner) : booError = True
            End If

            If txtZielTelefon.Text.Length = 0 Then
                SetErrorFrame(txtZielTelefon) : booError = True
            End If

            If ddlZielTransporttyp.SelectedItem.Value = "00" Then
                divZielTransporttyp.Style.Add("border", "solid 1px red") : booError = True
            End If

            If booError = True Then
                lblErrorFahrten.Text = "Bitte geben Sie zunächst die Zieladresse für das Fzg. 1 ein."
                Return booError
            End If


            'Rückholung
            If trRueck.Visible = True Then
                If txtRuZielFirma.Text.Length = 0 Then
                    SetErrorFrame(txtRuZielFirma) : booError = True
                End If

                If txtRuZielStrasse.Text.Length = 0 Then
                    SetErrorFrame(txtRuZielStrasse) : booError = True
                End If


                If CInt(ct.Laender.Select("Land1='" & ddlRuZielLand.SelectedItem.Value & "'")(0)("Lnplz")) > 0 Then
                    If Not CInt(ct.Laender.Select("Land1='" & ddlRuZielLand.SelectedItem.Value & "'")(0)("Lnplz")) = txtRuZielPlz.Text.Trim(" "c).Length Then
                        SetErrorFrame(txtRuZielPlz) : booError = True
                    End If
                End If

                If txtRuZielPlz.Text.Length = 0 Then
                    SetErrorFrame(txtRuZielPlz) : booError = True
                End If

                If txtRuZielOrt.Text.Length = 0 Then
                    SetErrorFrame(txtRuZielOrt) : booError = True
                End If

                If txtRuZielAnsprechpartner.Text.Length = 0 Then
                    SetErrorFrame(txtRuZielAnsprechpartner) : booError = True
                End If

                If txtRuZielTelefon.Text.Length = 0 Then
                    SetErrorFrame(txtRuZielTelefon) : booError = True
                End If

                If ddlRuTransporttyp.SelectedItem.Value = "00" Then
                    divRuTransporttyp.Style.Add("border", "solid 1px red") : booError = True
                End If

                If booError = True Then
                    lblErrorFahrten.Text = "Bitte geben Sie zunächst die Zieladresse für das Fzg. 2 ein."
                    Return booError
                End If

            End If

            'If booError = True Then lblErrorFahrten.Text = "Bitte füllen Sie die rot umrahmten Felder aus."

            Return booError

        End Function

        Private Function CheckZusatz() As Boolean
            Dim booError As Boolean = False

            Dim ct As Transfer = CType(Session("Transfer"), Transfer)

            resetZusatz()

            If txtZuFirma.Text.Length = 0 Then
                SetErrorFrame(txtZuFirma) : booError = True
            End If

            If txtZuStrasse.Text.Length = 0 Then
                SetErrorFrame(txtZuStrasse) : booError = True
            End If


            If CInt(ct.Laender.Select("Land1='" & ddlZuLand.SelectedItem.Value & "'")(0)("Lnplz")) > 0 Then
                If Not CInt(ct.Laender.Select("Land1='" & ddlZuLand.SelectedItem.Value & "'")(0)("Lnplz")) = txtZuPLZ.Text.Trim(" "c).Length Then
                    SetErrorFrame(txtZuPLZ) : booError = True
                End If
            End If


            If txtZuPLZ.Text.Length = 0 Then
                SetErrorFrame(txtZuPLZ) : booError = True
            End If

            If txtZuOrt.Text.Length = 0 Then
                SetErrorFrame(txtZuOrt) : booError = True
            End If

            If txtZuAnsprechpartner.Text.Length = 0 Then
                SetErrorFrame(txtZuAnsprechpartner) : booError = True
            End If

            If txtZuTelefon.Text.Length = 0 Then
                SetErrorFrame(txtZuTelefon) : booError = True
            End If

            If ddlZuTransporttyp.SelectedItem.Value = "00" Then
                divZuTransporttyp.Style.Add("border", "solid 1px red") : booError = True
            End If

            Return booError


        End Function


        Private Sub FillTables()

            Dim ct As Transfer = CType(Session("Transfer"), Transfer)

            ct.FillAdressen(m_User, Me.Page, "Z_WEB_UEB_LAND", "", "", "", getKundennummer())
            ct.getLaender(Me.Page)

            FillLaender()

            ct.FillTables(m_User, Me.Page, getKundennummer())

            Session("Transfer") = ct

        End Sub


        Private Sub FillLaender()
            Dim ct As Transfer = CType(Session("Transfer"), Transfer)


            'Länder DLL füllen
            ddlAbLand.DataSource = ct.Laender
            ddlAbLand.DataTextField = "FullDesc"
            ddlAbLand.DataValueField = "Land1"
            ddlAbLand.DataBind()
            ddlAbLand.Items.FindByValue("DE").Selected = True

            ddlZielland.DataSource = ct.Laender
            ddlZielland.DataTextField = "FullDesc"
            ddlZielland.DataValueField = "Land1"
            ddlZielland.DataBind()
            ddlZielland.Items.FindByValue("DE").Selected = True


            ddlZuLand.DataSource = ct.Laender
            ddlZuLand.DataTextField = "FullDesc"
            ddlZuLand.DataValueField = "Land1"
            ddlZuLand.DataBind()
            ddlZuLand.Items.FindByValue("DE").Selected = True

            ddlRueckZuLand.DataSource = ct.Laender
            ddlRueckZuLand.DataTextField = "FullDesc"
            ddlRueckZuLand.DataValueField = "Land1"
            ddlRueckZuLand.DataBind()
            ddlRueckZuLand.Items.FindByValue("DE").Selected = True

            ddlRuZielLand.DataSource = ct.Laender
            ddlRuZielLand.DataTextField = "FullDesc"
            ddlRuZielLand.DataValueField = "Land1"
            ddlRuZielLand.DataBind()
            ddlRuZielLand.Items.FindByValue("DE").Selected = True

        End Sub

        Private Sub WriteDataAbholUndZieladresse()

            Dim ct As Transfer = CType(Session("Transfer"), Transfer)

            'Fahrt Abholadresse

            Dim SelRow As DataRow() = ct.Fahrten.Select("FAHRZEUG = '1' and FAHRT = '0'")


            Dim FahrtenRow As DataRow = ct.Fahrten.NewRow

            'Neue Abholfahrt hinzufügen
            If SelRow.Length = 0 Then
                FahrtenRow("FAHRT") = "0"
                FahrtenRow("FAHRZEUG") = "1"
                FahrtenRow("REIHENFOLGE") = "0"

                If txtAbDatum.Text.Length > 0 Then
                    FahrtenRow("VDATU") = CDate(txtAbDatum.Text)
                Else
                    FahrtenRow("VDATU") = System.DBNull.Value
                End If

                'FahrtenRow("VTIMEU") = Replace(txtAbUhrzeit.Text, ":", "")

                If ddlAbUhrzeit.SelectedValue = "0-0" Then
                    FahrtenRow("AT_TIM_VON") = ""
                    FahrtenRow("AT_TIM_BIS") = ""
                Else

                    Dim TimeSpan As String() = Split(ddlAbUhrzeit.SelectedValue, "-")
                    FahrtenRow("AT_TIM_VON") = TimeSpan(0)
                    FahrtenRow("AT_TIM_BIS") = TimeSpan(1)
                End If

                FahrtenRow("KENNZ_ZUS_FAHT") = ""

                ct.Fahrten.Rows.Add(FahrtenRow)


            Else
                'Bestehende Fahrt aktualisieren
                SelRow(0)("FAHRT") = "0"
                SelRow(0)("FAHRZEUG") = "1"
                SelRow(0)("REIHENFOLGE") = "0"
                'If SelRow(0)("TRANSPORTTYP") <> ddlZielTransporttyp.SelectedItem.Value Then
                '    If SelRow(0)("TRANSPORTTYP").ToString <> "" Then
                '        UpdateDienstleistungen(ct, SelRow(0))
                '    Else
                '        SelRow(0)("TRANSPORTTYP") = ddlZielTransporttyp.SelectedItem.Value
                '    End If
                'End If

                If txtAbDatum.Text.Length > 0 Then
                    SelRow(0)("VDATU") = CDate(txtAbDatum.Text)
                Else
                    SelRow(0)("VDATU") = System.DBNull.Value
                End If

                'SelRow(0)("VTIMEU") = Replace(txtAbUhrzeit.Text, ":", "")
                If ddlAbUhrzeit.SelectedValue = "0-0" Then
                    SelRow(0)("AT_TIM_VON") = ""
                    SelRow(0)("AT_TIM_BIS") = ""
                Else

                    Dim TimeSpan As String() = Split(ddlAbUhrzeit.SelectedValue, "-")
                    SelRow(0)("AT_TIM_VON") = TimeSpan(0)
                    SelRow(0)("AT_TIM_BIS") = TimeSpan(1)
                End If



            End If

            ct.Fahrten.AcceptChanges()


            'Abholadresse
            Dim AbholRow As DataRow = ct.Adressen.NewRow

            SelRow = ct.Adressen.Select("FAHRT = '0'")

            'Neue Adresse hinzufügen
            If SelRow.Length = 0 Then
                AbholRow("FAHRT") = "0"
                AbholRow("NAME") = txtAbFirma.Text
                AbholRow("NAME_2") = txtAbAnsprechpartner.Text
                AbholRow("STREET") = txtAbStrasse.Text
                AbholRow("POSTL_CODE") = txtAbPLZ.Text
                AbholRow("CITY") = txtAbOrt.Text
                AbholRow("COUNTRY") = ddlAbLand.SelectedValue
                AbholRow("TELEPHONE") = txtAbTelefon.Text

                ct.Adressen.Rows.Add(AbholRow)
                ct.Adressen.AcceptChanges()
            Else
                SelRow(0)("FAHRT") = "0"
                SelRow(0)("NAME") = txtAbFirma.Text
                SelRow(0)("NAME_2") = txtAbAnsprechpartner.Text
                SelRow(0)("STREET") = txtAbStrasse.Text
                SelRow(0)("POSTL_CODE") = txtAbPLZ.Text
                SelRow(0)("CITY") = txtAbOrt.Text
                SelRow(0)("COUNTRY") = ddlAbLand.SelectedValue
                SelRow(0)("TELEPHONE") = txtAbTelefon.Text

            End If


            ct.Adressen.AcceptChanges()

            'Prüfung auf Fahrt Zieladresse

            Dim i As Integer
            Dim AlteZieladresse As String = ""
            'Wieviele Fahrzeuge vom Typ 1 gibt es?
            SelRow = ct.Fahrten.Select("FAHRZEUG = '1'")

            i = ct.Fahrten.DefaultView.Count

            'Wurde von diesen Fahrzeugen bereits eines als Zieladresse angelegt
            SelRow = ct.Fahrten.Select("FAHRZEUG = '1' and FAHRT <> '0' and KENNZ_ZUS_FAHT <> 'X'")

            FahrtenRow = ct.Fahrten.NewRow

            If SelRow.Length = 0 Then
                FahrtenRow("FAHRT") = i
                FahrtenRow("FAHRZEUG") = "1"
                FahrtenRow("REIHENFOLGE") = i
                FahrtenRow("TRANSPORTTYP") = ddlZielTransporttyp.SelectedValue

                If txtZielDatum.Text.Length > 0 Then
                    FahrtenRow("VDATU") = CDate(txtZielDatum.Text)
                Else
                    FahrtenRow("VDATU") = System.DBNull.Value
                End If


                'FahrtenRow("VTIMEU") = Replace(txtZielUhrzeit.Text, ":", "")

                If ddlZielUhrzeit.SelectedValue = "0-0" Then
                    FahrtenRow("AT_TIM_VON") = ""
                    FahrtenRow("AT_TIM_BIS") = ""
                Else

                    Dim TimeSpan As String() = Split(ddlZielUhrzeit.SelectedValue, "-")
                    FahrtenRow("AT_TIM_VON") = TimeSpan(0)
                    FahrtenRow("AT_TIM_BIS") = TimeSpan(1)
                End If


                FahrtenRow("KENNZ_ZUS_FAHT") = ""

                ct.Fahrten.Rows.Add(FahrtenRow)

                ct.Fahrten.AcceptChanges()

            Else

                AlteZieladresse = SelRow(0)("FAHRT") 'Benötigt für Adressen

                'i = i - 1

                SelRow(0)("FAHRZEUG") = "1"

                If SelRow(0)("TRANSPORTTYP") <> ddlZielTransporttyp.SelectedItem.Value Then
                    If SelRow(0)("TRANSPORTTYP").ToString <> "" Then
                        UpdateDienstleistungen(ct, SelRow(0), ddlZielTransporttyp.SelectedItem.Value)
                    Else
                        SelRow(0)("TRANSPORTTYP") = ddlZielTransporttyp.SelectedItem.Value
                    End If
                End If

                If txtZielDatum.Text.Length > 0 Then
                    SelRow(0)("VDATU") = CDate(txtZielDatum.Text)
                Else
                    SelRow(0)("VDATU") = System.DBNull.Value
                End If


                'SelRow(0)("VTIMEU") = Replace(txtZielUhrzeit.Text, ":", "")
                If ddlZielUhrzeit.SelectedValue = "0-0" Then
                    SelRow(0)("AT_TIM_VON") = ""
                    SelRow(0)("AT_TIM_BIS") = ""
                Else

                    Dim TimeSpan As String() = Split(ddlZielUhrzeit.SelectedValue, "-")
                    SelRow(0)("AT_TIM_VON") = TimeSpan(0)
                    SelRow(0)("AT_TIM_BIS") = TimeSpan(1)
                End If


            End If

            ct.Fahrten.AcceptChanges()

            'Prüfung Zieladresse
            Dim ZielRow As DataRow = ct.Adressen.NewRow

            SelRow = ct.Adressen.Select("FAHRT = '" & AlteZieladresse & "'")

            If SelRow.Length = 0 Then

                ZielRow("FAHRT") = i
                ZielRow("NAME") = txtZielFirma.Text
                ZielRow("NAME_2") = txtZielAnsprechpartner.Text
                ZielRow("STREET") = txtZielStrasse.Text
                ZielRow("POSTL_CODE") = txtZielPLZ.Text
                ZielRow("CITY") = txtZielOrt.Text
                ZielRow("COUNTRY") = ddlZielland.SelectedValue
                ZielRow("TELEPHONE") = txtZielTelefon.Text

                ct.Adressen.Rows.Add(ZielRow)
                ct.Adressen.AcceptChanges()
            Else
                SelRow(0)("FAHRT") = AlteZieladresse
                SelRow(0)("NAME") = txtZielFirma.Text
                SelRow(0)("NAME_2") = txtZielAnsprechpartner.Text
                SelRow(0)("STREET") = txtZielStrasse.Text
                SelRow(0)("POSTL_CODE") = txtZielPLZ.Text
                SelRow(0)("CITY") = txtZielOrt.Text
                SelRow(0)("COUNTRY") = ddlZielland.SelectedValue
                SelRow(0)("TELEPHONE") = txtZielTelefon.Text

            End If

            ct.Adressen.AcceptChanges()

            'Gibt es schon eine Rückholung
            If trRueck.Visible = True Then

                SelRow = ct.Fahrten.Select("FAHRZEUG = '2' and KENNZ_ZUS_FAHT <> 'X'")


                If SelRow.Length = 0 Then
                    Dim RueckRow As DataRow = ct.Fahrten.NewRow

                    'Fahrt
                    RueckRow("FAHRT") = ct.Fahrten.Rows.Count
                    RueckRow("FAHRZEUG") = "2"
                    RueckRow("REIHENFOLGE") = ct.Fahrten.Rows.Count
                    RueckRow("TRANSPORTTYP") = ddlRuTransporttyp.SelectedItem.Value


                    If txtRuZielDatum.Text.Length > 0 Then
                        RueckRow("VDATU") = CDate(txtRuZielDatum.Text)
                    Else
                        RueckRow("VDATU") = System.DBNull.Value
                    End If


                    'RueckRow("VTIMEU") = Replace(txtRuZielUhrzeit.Text, ":", "")
                    If ddlRuZielUhrzeit.SelectedValue = "0-0" Then
                        RueckRow("AT_TIM_VON") = ""
                        RueckRow("AT_TIM_BIS") = ""
                    Else

                        Dim TimeSpan As String() = Split(ddlRuZielUhrzeit.SelectedValue, "-")
                        RueckRow("AT_TIM_VON") = TimeSpan(0)
                        RueckRow("AT_TIM_BIS") = TimeSpan(1)
                    End If


                    RueckRow("KENNZ_ZUS_FAHT") = ""

                    ct.Fahrten.Rows.Add(RueckRow)
                    ct.Fahrten.AcceptChanges()

                    'Adresse
                    Dim RueckAdressRow As DataRow = ct.Adressen.NewRow

                    RueckAdressRow("FAHRT") = ct.Adressen.Rows.Count
                    RueckAdressRow("NAME") = txtRuZielFirma.Text
                    RueckAdressRow("NAME_2") = txtRuZielAnsprechpartner.Text
                    RueckAdressRow("STREET") = txtRuZielStrasse.Text
                    RueckAdressRow("POSTL_CODE") = txtRuZielPlz.Text
                    RueckAdressRow("CITY") = txtRuZielOrt.Text
                    RueckAdressRow("COUNTRY") = ddlRuZielLand.SelectedValue
                    RueckAdressRow("TELEPHONE") = txtRuZielTelefon.Text

                    ct.Adressen.Rows.Add(RueckAdressRow)
                    ct.Adressen.AcceptChanges()

                Else
                    'Bestehende Fahrt aktualisieren
                    'SelRow(0)("TRANSPORTTYP") = ddlRuTransporttyp.SelectedValue
                    If SelRow(0)("TRANSPORTTYP") <> ddlRuTransporttyp.SelectedItem.Value Then
                        If SelRow(0)("TRANSPORTTYP").ToString <> "" Then
                            UpdateDienstleistungen(ct, SelRow(0), ddlRuTransporttyp.SelectedItem.Value)
                        Else
                            SelRow(0)("TRANSPORTTYP") = ddlRuTransporttyp.SelectedItem.Value
                        End If
                    End If
                    If txtRuZielDatum.Text.Length > 0 Then
                        SelRow(0)("VDATU") = CDate(txtRuZielDatum.Text)
                    Else
                        SelRow(0)("VDATU") = System.DBNull.Value
                    End If



                    'SelRow(0)("VTIMEU") = Replace(txtRuZielUhrzeit.Text, ":", "")
                    If ddlRuZielUhrzeit.SelectedValue = "0-0" Then
                        SelRow(0)("AT_TIM_VON") = ""
                        SelRow(0)("AT_TIM_BIS") = ""
                    Else

                        Dim TimeSpan As String() = Split(ddlRuZielUhrzeit.SelectedValue, "-")
                        SelRow(0)("AT_TIM_VON") = TimeSpan(0)
                        SelRow(0)("AT_TIM_BIS") = TimeSpan(1)
                    End If

                    Dim Fahrt As String


                    Fahrt = SelRow(0)("FAHRT")

                    'Bestehende Adresse aktualisieren
                    SelRow = ct.Adressen.Select("FAHRT = '" & Fahrt & "'")
                    SelRow(0)("NAME") = txtRuZielFirma.Text
                    SelRow(0)("NAME_2") = txtRuZielAnsprechpartner.Text
                    SelRow(0)("STREET") = txtRuZielStrasse.Text
                    SelRow(0)("POSTL_CODE") = txtRuZielPlz.Text
                    SelRow(0)("CITY") = txtRuZielOrt.Text
                    SelRow(0)("COUNTRY") = ddlRuZielLand.SelectedValue
                    SelRow(0)("TELEPHONE") = txtRuZielTelefon.Text
                    ct.Adressen.AcceptChanges()
                End If

            End If




            Session("Transfer") = ct


        End Sub

        Private Sub ResetFahrten()
            lblErrorFahrten.Visible = False
            ResetErrorFrame(txtAbFirma)
            ResetErrorFrame(txtAbStrasse)
            ResetErrorFrame(txtAbPLZ)
            ResetErrorFrame(txtAbOrt)
            ResetErrorFrame(txtAbAnsprechpartner)
            ResetErrorFrame(txtAbTelefon)
            ResetErrorFrame(txtAbDatum)
            'ResetErrorFrame(txtAbUhrzeit)
            'divAbTransporttyp.Style.Remove("border")
            ResetErrorFrame(txtZielFirma)
            ResetErrorFrame(txtZielStrasse)
            ResetErrorFrame(txtZielPLZ)
            ResetErrorFrame(txtZielOrt)
            ResetErrorFrame(txtZielAnsprechpartner)
            ResetErrorFrame(txtZielTelefon)
            ResetErrorFrame(txtZielDatum)
            'ResetErrorFrame(txtZielUhrzeit)
            divZielTransporttyp.Style.Remove("border")

            ResetErrorFrame(txtRuZielFirma)
            ResetErrorFrame(txtRuZielStrasse)
            ResetErrorFrame(txtRuZielPlz)
            ResetErrorFrame(txtRuZielOrt)
            ResetErrorFrame(txtRuZielAnsprechpartner)
            ResetErrorFrame(txtRuZielTelefon)
            ResetErrorFrame(txtRuZielDatum)
            'ResetErrorFrame(txtRuZielUhrzeit)
            divRuTransporttyp.Style.Remove("border")
        End Sub

        Private Sub resetZusatz()
            lblErrorFahrten.Visible = False

            ResetErrorFrame(txtZuFirma)
            ResetErrorFrame(txtZuStrasse)
            ResetErrorFrame(txtZuPLZ)
            ResetErrorFrame(txtZuOrt)
            ResetErrorFrame(txtZuAnsprechpartner)
            ResetErrorFrame(txtZuTelefon)
            ResetErrorFrame(txtZuDatum)
            'ResetErrorFrame(txtZuUhrzeit)
            divZuTransporttyp.Style.Remove("border")
        End Sub

        Private Sub LoadFahrten()
            Dim ct As Transfer = CType(Session("Transfer"), Transfer)

            'Transporttyp-Dropdowns
            'ddlAbTransporttyp.DataSource = ct.Transporttyp
            'ddlAbTransporttyp.DataValueField = "ID"
            'ddlAbTransporttyp.DataTextField = "Text"
            'ddlAbTransporttyp.DataBind()

            If trRueck.Visible = True Then
                trRueckZiel.Visible = True
                divRZusatz.Visible = True
                ddlRuTransporttyp.DataSource = ct.Transporttyp
                ddlRuTransporttyp.DataValueField = "ID"
                ddlRuTransporttyp.DataTextField = "Text"
                ddlRuTransporttyp.DataBind()

            End If

            ddlZielTransporttyp.DataSource = ct.Transporttyp
            ddlZielTransporttyp.DataValueField = "ID"
            ddlZielTransporttyp.DataTextField = "Text"
            ddlZielTransporttyp.DataBind()

            ddlZuTransporttyp.DataSource = ct.Transporttyp
            ddlZuTransporttyp.DataValueField = "ID"
            ddlZuTransporttyp.DataTextField = "Text"
            ddlZuTransporttyp.DataBind()

            ddlRZuTransporttyp.DataSource = ct.Transporttyp
            ddlRZuTransporttyp.DataValueField = "ID"
            ddlRZuTransporttyp.DataTextField = "Text"
            ddlRZuTransporttyp.DataBind()

            'Abholfahrt laden
            Dim SelRow As DataRow() = ct.Fahrten.Select("FAHRZEUG = '1' and FAHRT = '0'")

            If SelRow.Length > 0 Then
                'ddlAbTransporttyp.SelectedValue = SelRow(0)("TRANSPORTTYP").ToString
                txtAbDatum.Text = SelRow(0)("VDATU").ToString
                'txtAbUhrzeit.Text = SelRow(0)("VTIMEU").ToString
                If SelRow(0)("AT_TIM_VON") = "" Then
                    ddlAbUhrzeit.SelectedValue = "0-0"
                Else
                    ddlAbUhrzeit.SelectedValue = SelRow(0)("AT_TIM_VON") & "-" & SelRow(0)("AT_TIM_BIS")

                End If



            End If

            'Abholadresse laden
            SelRow = ct.Adressen.Select("FAHRT = '0'")
            If SelRow.Length > 0 Then
                txtAbFirma.Text = SelRow(0)("NAME").ToString
                txtAbAnsprechpartner.Text = SelRow(0)("NAME_2").ToString
                txtAbStrasse.Text = SelRow(0)("STREET").ToString
                txtAbPLZ.Text = SelRow(0)("POSTL_CODE").ToString
                txtAbOrt.Text = SelRow(0)("CITY").ToString
                ddlAbLand.SelectedValue = SelRow(0)("COUNTRY").ToString
                txtAbTelefon.Text = SelRow(0)("TELEPHONE").ToString
            End If


            'Zielfahrt laden
            Dim ZielFahrtID As String = ""
            SelRow = ct.Fahrten.Select("FAHRZEUG = '1' and FAHRT <> '0' and KENNZ_ZUS_FAHT <> 'X'")
            If SelRow.Length > 0 Then
                ZielFahrtID = SelRow(0)("FAHRT").ToString
                ddlZielTransporttyp.SelectedValue = SelRow(0)("TRANSPORTTYP").ToString
                txtZielDatum.Text = SelRow(0)("VDATU").ToString
                'txtZielUhrzeit.Text = SelRow(0)("VTIMEU").ToString
                If SelRow(0)("AT_TIM_VON") = "" Then
                    ddlZielUhrzeit.SelectedValue = "0-0"
                Else
                    ddlZielUhrzeit.SelectedValue = SelRow(0)("AT_TIM_VON") & "-" & SelRow(0)("AT_TIM_BIS")

                End If

            End If

            'Zieladresse laden
            If ZielFahrtID.Length > 0 Then
                SelRow = ct.Adressen.Select("FAHRT = '" & ZielFahrtID & "'")
                If SelRow.Length > 0 Then
                    txtZielFirma.Text = SelRow(0)("NAME").ToString
                    txtZielAnsprechpartner.Text = SelRow(0)("NAME_2").ToString
                    txtZielStrasse.Text = SelRow(0)("STREET").ToString
                    txtZielPLZ.Text = SelRow(0)("POSTL_CODE").ToString
                    txtZielOrt.Text = SelRow(0)("CITY").ToString
                    ddlZielland.SelectedValue = SelRow(0)("COUNTRY").ToString
                    txtZielTelefon.Text = SelRow(0)("TELEPHONE").ToString
                End If
            End If


            If trRueck.Visible = True Then

                'Zielfahrt Rückholung laden

                SelRow = ct.Fahrten.Select("FAHRZEUG = '2' and KENNZ_ZUS_FAHT <> 'X'")
                If SelRow.Length > 0 Then
                    ZielFahrtID = SelRow(0)("FAHRT").ToString
                    ddlRuTransporttyp.SelectedValue = SelRow(0)("TRANSPORTTYP").ToString
                    txtRuZielDatum.Text = SelRow(0)("VDATU").ToString
                    'txtRuZielUhrzeit.Text = SelRow(0)("VTIMEU").ToString
                    If SelRow(0)("AT_TIM_VON") = "" Then
                        ddlZielUhrzeit.SelectedValue = "0-0"
                    Else
                        ddlZielUhrzeit.SelectedValue = SelRow(0)("AT_TIM_VON") & "-" & SelRow(0)("AT_TIM_BIS")
                    End If

                End If

                'Zieladresse laden
                If ZielFahrtID.Length > 0 Then
                    SelRow = ct.Adressen.Select("FAHRT = '" & ZielFahrtID & "'")
                    If SelRow.Length > 0 Then
                        txtRuZielFirma.Text = SelRow(0)("NAME").ToString
                        txtRuZielAnsprechpartner.Text = SelRow(0)("NAME_2").ToString
                        txtRuZielStrasse.Text = SelRow(0)("STREET").ToString
                        txtRuZielPlz.Text = SelRow(0)("POSTL_CODE").ToString
                        txtRuZielOrt.Text = SelRow(0)("CITY").ToString
                        ddlRuZielLand.SelectedValue = SelRow(0)("COUNTRY").ToString
                        txtRuZielTelefon.Text = SelRow(0)("TELEPHONE").ToString
                    End If
                End If

            End If

            'Zusatzfahrten laden
            FillGridZusatz()

            'Zusatzfahrten Rückholung laden
            FillRueckGridZusatz()



        End Sub

        Private Sub LoadSingleZusatzfahrt()

            OpenZusatzfahrt()

            lblZusatz.Text = "Zusatzfahrt bearbeiten"

            grvZusatz.Visible = False

            Dim ct As Transfer = CType(Session("Transfer"), Transfer)


            Dim SelRow As DataRow()


            SelRow = ct.Fahrten.Select("FAHRT = '" & ct.EditFahrt & "'")

            If SelRow.Length > 0 Then
                ddlZuTransporttyp.SelectedValue = SelRow(0)("TRANSPORTTYP").ToString
                txtZuDatum.Text = SelRow(0)("VDATU").ToString
                'txtZuUhrzeit.Text = SelRow(0)("VTIMEU").ToString
                If SelRow(0)("AT_TIM_VON") = "" Then
                    ddlZuUhrzeit.SelectedValue = "0-0"
                Else
                    ddlZuUhrzeit.SelectedValue = SelRow(0)("AT_TIM_VON") & "-" & SelRow(0)("AT_TIM_BIS")

                End If


            End If

            SelRow = ct.Adressen.Select("FAHRT = '" & ct.EditFahrt & "'")
            If SelRow.Length > 0 Then
                txtZuFirma.Text = SelRow(0)("NAME").ToString
                txtZuAnsprechpartner.Text = SelRow(0)("NAME_2").ToString
                txtZuStrasse.Text = SelRow(0)("STREET").ToString
                txtZuPLZ.Text = SelRow(0)("POSTL_CODE").ToString
                txtZuOrt.Text = SelRow(0)("CITY").ToString
                ddlZuLand.SelectedValue = SelRow(0)("COUNTRY").ToString

                txtZuTelefon.Text = SelRow(0)("TELEPHONE").ToString
            End If


        End Sub

        Private Sub SaveZusatzfahrt()
            If CheckFahrten() = True Then
                Exit Sub
            End If

            If CheckZusatz() = True Then
                Exit Sub
            End If




            Dim ct As Transfer = CType(Session("Transfer"), Transfer)

            'Bestehende Fahrt bearbeiten
            If ct.EditFahrt.Length > 0 Then

                Dim SelRow As DataRow()

                SelRow = ct.Fahrten.Select("FAHRT = '" & ct.EditFahrt & "'")


                SelRow(0)("TRANSPORTTYP") = ddlZuTransporttyp.SelectedItem.Value


                If txtZuDatum.Text.Length > 0 Then
                    SelRow(0)("VDATU") = CDate(txtZuDatum.Text)
                Else
                    SelRow(0)("VDATU") = DBNull.Value
                End If


                'SelRow(0)("VTIMEU") = Replace(txtZuUhrzeit.Text, ":", "")

                If ddlZuUhrzeit.SelectedValue = "0-0" Then
                    SelRow(0)("AT_TIM_VON") = ""
                    SelRow(0)("AT_TIM_BIS") = ""
                Else

                    Dim TimeSpan As String() = Split(ddlZuUhrzeit.SelectedValue, "-")
                    SelRow(0)("AT_TIM_VON") = TimeSpan(0)
                    SelRow(0)("AT_TIM_BIS") = TimeSpan(1)
                End If


                SelRow(0)("KENNZ_ZUS_FAHT") = "X"

                ct.Fahrten.AcceptChanges()

                SelRow = ct.Adressen.Select("FAHRT = '" & ct.EditFahrt & "'")

                SelRow(0)("NAME") = txtZuFirma.Text
                SelRow(0)("NAME_2") = txtZuAnsprechpartner.Text
                SelRow(0)("STREET") = txtZuStrasse.Text
                SelRow(0)("POSTL_CODE") = txtZuPLZ.Text
                SelRow(0)("CITY") = txtZuOrt.Text
                SelRow(0)("COUNTRY") = ddlZuLand.SelectedValue
                SelRow(0)("TELEPHONE") = txtZuTelefon.Text

                ct.Adressen.AcceptChanges()
                ct.EditFahrt = ""

            Else

                'Neue Zusatzfahrt hinzufügen
                Dim ZusatzFahrt As DataTable = ct.Fahrten.Clone
                Dim ZusatzAdresse As DataTable = ct.Adressen.Clone

                Dim ZusatzFahrtRow As DataRow
                Dim ZusatzAdressRow As DataRow


                Dim indexZiel As Integer = 0

                'Index für Zieladresse ermitteln
                For i = 0 To ct.Fahrten.Rows.Count - 1

                    If ct.Fahrten.Rows(i)("FAHRZEUG") = "1" AndAlso ct.Fahrten.Rows(i)("FAHRT") <> "0" AndAlso ct.Fahrten.Rows(i)("KENNZ_ZUS_FAHT") <> "X" Then
                        indexZiel = i
                        Exit For
                    End If

                Next


                'Dienstleistungen erhöhen
                For Each Row As DataRow In ct.Dienstleistungen.Rows

                    If CInt(Row("FAHRT")) >= indexZiel Then
                        Row("FAHRT") = (CInt(Row("FAHRT") + 1)).ToString
                    End If
                Next

                ct.Dienstleistungen.AcceptChanges()

                'Bemerkungen erhöhen
                For Each Row As DataRow In ct.Bemerkungen.Rows

                    If CInt(Row("FAHRT")) >= indexZiel Then
                        Row("FAHRT") = (CInt(Row("FAHRT") + 1)).ToString
                    End If
                Next

                ct.Bemerkungen.AcceptChanges()

                Dim Fahrt As Integer = 0


                For i = 0 To ct.Fahrten.Rows.Count - 1


                    If i <> 0 AndAlso i <= indexZiel AndAlso ct.Fahrten.Rows(i)("KENNZ_ZUS_FAHT") <> "X" Then

                        Fahrt += 1

                        ZusatzFahrtRow = ZusatzFahrt.NewRow()
                        ZusatzAdressRow = ZusatzAdresse.NewRow()

                        ZusatzFahrtRow("FAHRT") = Fahrt
                        ZusatzFahrtRow("FAHRZEUG") = "1"
                        ZusatzFahrtRow("REIHENFOLGE") = Fahrt
                        ZusatzFahrtRow("TRANSPORTTYP") = ddlZuTransporttyp.SelectedItem.Value

                        If txtZuDatum.Text.Length > 0 Then
                            ZusatzFahrtRow("VDATU") = CDate(txtZuDatum.Text)
                        End If


                        'ZusatzFahrtRow("VTIMEU") = Replace(txtZuUhrzeit.Text, ":", "")

                        If ddlZuUhrzeit.SelectedValue = "0-0" Then
                            ZusatzFahrtRow("AT_TIM_VON") = ""
                            ZusatzFahrtRow("AT_TIM_BIS") = ""
                        Else

                            Dim TimeSpan As String() = Split(ddlZuUhrzeit.SelectedValue, "-")
                            ZusatzFahrtRow("AT_TIM_VON") = TimeSpan(0)
                            ZusatzFahrtRow("AT_TIM_BIS") = TimeSpan(1)
                        End If


                        ZusatzFahrtRow("KENNZ_ZUS_FAHT") = "X"

                        ZusatzAdressRow("FAHRT") = Fahrt
                        ZusatzAdressRow("NAME") = txtZuFirma.Text
                        ZusatzAdressRow("NAME_2") = txtZuAnsprechpartner.Text
                        ZusatzAdressRow("STREET") = txtZuStrasse.Text
                        ZusatzAdressRow("POSTL_CODE") = txtZuPLZ.Text
                        ZusatzAdressRow("CITY") = txtZuOrt.Text
                        ZusatzAdressRow("COUNTRY") = ddlZuLand.SelectedValue
                        ZusatzAdressRow("TELEPHONE") = txtZuTelefon.Text

                        ZusatzFahrt.Rows.Add(ZusatzFahrtRow)
                        ZusatzFahrt.AcceptChanges()
                        ZusatzAdresse.Rows.Add(ZusatzAdressRow)
                        ZusatzAdresse.AcceptChanges()

                    End If

                    ZusatzFahrtRow = ZusatzFahrt.NewRow()
                    ZusatzAdressRow = ZusatzAdresse.NewRow()

                    If i > 0 Then
                        Fahrt += 1
                    End If


                    ZusatzFahrtRow("FAHRT") = Fahrt
                    ZusatzFahrtRow("FAHRZEUG") = ct.Fahrten.Rows(i)("FAHRZEUG")
                    ZusatzFahrtRow("REIHENFOLGE") = Fahrt
                    ZusatzFahrtRow("TRANSPORTTYP") = ct.Fahrten.Rows(i)("TRANSPORTTYP")
                    ZusatzFahrtRow("VDATU") = ct.Fahrten.Rows(i)("VDATU")
                    'ZusatzFahrtRow("VTIMEU") = ct.Fahrten.Rows(i)("VTIMEU")
                    ZusatzFahrtRow("AT_TIM_VON") = ct.Fahrten.Rows(i)("AT_TIM_VON")
                    ZusatzFahrtRow("AT_TIM_BIS") = ct.Fahrten.Rows(i)("AT_TIM_BIS")

                    ZusatzFahrtRow("KENNZ_ZUS_FAHT") = ct.Fahrten.Rows(i)("KENNZ_ZUS_FAHT")

                    ZusatzAdressRow("FAHRT") = Fahrt
                    ZusatzAdressRow("NAME") = ct.Adressen.Rows(i)("NAME")
                    ZusatzAdressRow("NAME_2") = ct.Adressen.Rows(i)("NAME_2")
                    ZusatzAdressRow("STREET") = ct.Adressen.Rows(i)("STREET")
                    ZusatzAdressRow("POSTL_CODE") = ct.Adressen.Rows(i)("POSTL_CODE")
                    ZusatzAdressRow("CITY") = ct.Adressen.Rows(i)("CITY")
                    ZusatzAdressRow("TELEPHONE") = ct.Adressen.Rows(i)("TELEPHONE")
                    ZusatzAdressRow("COUNTRY") = ct.Adressen.Rows(i)("COUNTRY")

                    ZusatzFahrt.Rows.Add(ZusatzFahrtRow)
                    ZusatzFahrt.AcceptChanges()
                    ZusatzAdresse.Rows.Add(ZusatzAdressRow)
                    ZusatzAdresse.AcceptChanges()


                Next

                ct.Fahrten = ZusatzFahrt
                ct.Adressen = ZusatzAdresse


            End If

            lblZusatz.Text = "Zusatzfahrt hinzufügen"

            Session("Transfer") = ct

            ct = Nothing

            CloseZusatzfahrt()

            ibtZieladresseHeaderClose.Visible = True
            ibtZieladresseHeader.Visible = False
            pnlZieladresse.Visible = True

        End Sub

        Private Sub FillGridZusatz(Optional ByRef DL As Boolean = False)

            Dim ct As Transfer = CType(Session("Transfer"), Transfer)
            Dim tmpTable As New DataTable

            tmpTable.Columns.Add("FAHRT", GetType(System.String))
            tmpTable.Columns.Add("Info", GetType(System.String))
            tmpTable.Columns.Add("Transporttyp", GetType(System.String))
            tmpTable.Columns.Add("InfoTooltip", GetType(System.String))

            tmpTable.AcceptChanges()

            Dim nRow As DataRow
            Dim FahrtNummer As String = ""
            Dim InfoTooltip As String = ""

            For Each Row As DataRow In ct.Fahrten.Rows

                nRow = tmpTable.NewRow

                InfoTooltip = ""

                If Row("FAHRZEUG") = "1" AndAlso Row("KENNZ_ZUS_FAHT") = "X" Then
                    FahrtNummer = Row("FAHRT")

                    nRow("FAHRT") = FahrtNummer
                    nRow("Transporttyp") = ct.Transporttyp.Select("ID = '" & Row("TRANSPORTTYP") & "'")(0)("Text").ToString
                    nRow("Info") = "Zusatzfahrt " & ct.Adressen.Select("FAHRT = '" & FahrtNummer & "'")(0)("CITY").ToString

                    InfoTooltip = ct.Adressen.Select("FAHRT = '" & FahrtNummer & "'")(0)("NAME").ToString & vbCrLf
                    InfoTooltip &= ct.Adressen.Select("FAHRT = '" & FahrtNummer & "'")(0)("STREET").ToString & vbCrLf
                    InfoTooltip &= ct.Adressen.Select("FAHRT = '" & FahrtNummer & "'")(0)("POSTL_CODE").ToString & " " & ct.Adressen.Select("FAHRT = '" & FahrtNummer & "'")(0)("CITY").ToString & vbCrLf
                    InfoTooltip &= "Telefon: " & ct.Adressen.Select("FAHRT = '" & FahrtNummer & "'")(0)("TELEPHONE").ToString

                    nRow("InfoTooltip") = InfoTooltip

                    tmpTable.Rows.Add(nRow)

                End If
            Next

            tmpTable.AcceptChanges()

            If tmpTable.Rows.Count > 0 Then


                If DL = False Then
                    grvZusatz.DataSource = tmpTable.DefaultView

                    grvZusatz.DataBind()

                    grvZusatz.Visible = True

                    PrepareGrid()
                Else
                    grvZusatzDL.DataSource = tmpTable.DefaultView

                    grvZusatzDL.DataBind()

                    grvZusatzDL.Visible = True
                    trZusatzDL.Visible = True

                    pnlZieladresse.Visible = False

                    ibtHeaderDlZieladresseClose.Visible = False
                    ibtHeaderDlZieladresse.Visible = True

                    trZusatz2DL.Visible = True

                    Dim ibt As ImageButton

                    ibt = CType(grvZusatzDL.Rows(CInt(0)).FindControl("ibtDlZuOpen"), ImageButton)
                    ibt.Visible = False

                    ibt = CType(grvZusatzDL.Rows(CInt(0)).FindControl("ibtDlZuClose"), ImageButton)
                    ibt.Visible = True


                    For i = 0 To grvZusatzDL.Rows.Count - 1
                        If CInt(0) <> i Then
                            ibt = CType(grvZusatzDL.Rows(i).FindControl("ibtDlZuOpen"), ImageButton)
                            ibt.Visible = False
                        End If
                    Next

                    Dim lbl As Label = CType(grvZusatzDL.Rows(CInt(0)).FindControl("ID"), Label)

                    ct.ZuActiveFahrt = lbl.Text
                    ct.EditFahrt = lbl.Text

                    ct.Dienstleistungen.DefaultView.RowFilter = "FAHRT='" & ct.ZuActiveFahrt & "'"
                    chkZusatzGruende.DataSource = ct.Dienstleistungen.DefaultView
                    chkZusatzGruende.DataValueField = "DIENSTL_NR"
                    chkZusatzGruende.DataTextField = "DIENSTL_TEXT"
                    chkZusatzGruende.DataBind()

                    For Each litem As ListItem In chkZusatzGruende.Items
                        litem.Selected = True
                        litem.Attributes.Add("onclick", "return false;")

                    Next

                    txtBemerkungZusatz.Text = ""

                    'Bemerkung laden
                    If ct.Bemerkungen.Select("FAHRT = '" & lbl.Text & "'").Length > 0 Then


                        For Each dr As DataRow In ct.Bemerkungen.Rows
                            txtBemerkungZusatz.Text = txtBemerkungZusatz.Text & dr("BEMERKUNG")
                        Next
                        'Else
                        '    txtBemerkungZusatz.Text = ""
                    End If


                    Session("Transfer") = ct

                    ct = Nothing


                End If


            Else
                grvZusatz.Visible = False
                trDLZieladresse.Visible = True
                pnlDlZieladresseFirma.Visible = True
                ibtHeaderDlZieladresse.Visible = False
                ibtHeaderDlZieladresseClose.Visible = True

            End If
        End Sub

        Private Sub FillGridProtokolle(ByVal tblProtokolle As DataTable)
            If Not tblProtokolle Is Nothing Then
                Dim tmpDataView As New DataView()
                tmpDataView = tblProtokolle.DefaultView
                tmpDataView.RowFilter = "Fahrt=1"

                If tmpDataView.Count = 0 Then
                    grvProtokollUpload1.Visible = False
                Else
                    Dim strTempSort As String = ""
                    Dim strDirection As String = ""
                    grvProtokollUpload1.Visible = True

                    grvProtokollUpload1.DataSource = tmpDataView
                    grvProtokollUpload1.DataBind()
                End If
            End If

        End Sub
        Private Sub FillGridProtokolle2(ByVal tblProtokolle As DataTable)

            If Not tblProtokolle Is Nothing Then
                Dim tmpDataView As New DataView()
                tmpDataView = tblProtokolle.DefaultView
                tmpDataView.RowFilter = "Fahrt=2"

                If tmpDataView.Count = 0 Then
                    grvProtokollUpload2.Visible = False
                Else
                    Dim strTempSort As String = ""
                    Dim strDirection As String = ""
                    grvProtokollUpload2.Visible = True

                    grvProtokollUpload2.DataSource = tmpDataView
                    grvProtokollUpload2.DataBind()

                    For Each GridRow As GridViewRow In grvProtokollUpload2.Rows
                        Dim FileUpload As AjaxControlToolkit.AsyncFileUpload

                        'FileUpload = GridRow.FindControl("AsyncFileUpload1")
                        FileUpload = GridRow.FindControl("radUpload1")

                        If Not FileUpload Is Nothing Then
                            FileUpload.Attributes.Add("onchange", "return validateFileUpload(this);")
                        End If

                    Next
                End If
            End If

        End Sub
        Private Sub SaveRueckZusatzfahrt()
            If CheckFahrten() = True Then
                Exit Sub
            End If

            If CheckRueckZusatz() = True Then
                Exit Sub
            End If


            Dim ct As Transfer = CType(Session("Transfer"), Transfer)


            If ct.EditRueckFahrt.Length > 0 Then


                Dim SelRow As DataRow()

                SelRow = ct.Fahrten.Select("FAHRT = '" & ct.EditRueckFahrt & "'")


                SelRow(0)("TRANSPORTTYP") = ddlRZuTransporttyp.SelectedItem.Value

                If txtRueckZuDatum.Text.Length > 0 Then
                    SelRow(0)("VDATU") = CDate(txtRueckZuDatum.Text)
                Else
                    SelRow(0)("VDATU") = DBNull.Value
                End If


                'SelRow(0)("VTIMEU") = Replace(txtRueckZuUhrzeit.Text, ":", "")
                If SelRow(0)("AT_TIM_VON") = "" Then
                    ddlRueckZuUhrzeit.SelectedValue = "0-0"
                Else
                    ddlRueckZuUhrzeit.SelectedValue = SelRow(0)("AT_TIM_VON") & "-" & SelRow(0)("AT_TIM_BIS")

                End If



                SelRow(0)("KENNZ_ZUS_FAHT") = "X"

                ct.Fahrten.AcceptChanges()

                SelRow = ct.Adressen.Select("FAHRT = '" & ct.EditRueckFahrt & "'")

                SelRow(0)("NAME") = txtRueckZuFirma.Text
                SelRow(0)("NAME_2") = txtRueckZuAnsprechpartner.Text
                SelRow(0)("STREET") = txtRueckZuStrasse.Text
                SelRow(0)("POSTL_CODE") = txtRueckZuPLZ.Text
                SelRow(0)("CITY") = txtRueckZuOrt.Text
                SelRow(0)("COUNTRY") = ddlRueckZuLand.SelectedValue
                SelRow(0)("TELEPHONE") = txtRueckZuTelefon.Text

                ct.Adressen.AcceptChanges()
                ct.EditRueckFahrt = ""

            Else


                Dim NewFahrten As DataTable = ct.Fahrten.Clone
                Dim NewAdressen As DataTable = ct.Adressen.Clone


                Dim ZielFahrt As String = ct.Fahrten.Select("FAHRZEUG = '2' AND KENNZ_ZUS_FAHT <> 'X'")(0)("FAHRT")


                Dim NewRow As DataRow

                'Fahrten
                For Each Row As DataRow In ct.Fahrten.Rows

                    'Bei alter Zieladresse aussteigen
                    If Row("FAHRT") = ZielFahrt Then Exit For

                    NewRow = NewFahrten.NewRow
                    For Each Col As DataColumn In ct.Fahrten.Columns
                        NewRow(Col.ColumnName) = Row(Col.ColumnName)
                    Next
                    NewFahrten.Rows.Add(NewRow)
                    NewFahrten.AcceptChanges()
                Next

                'Adressen
                For Each Row As DataRow In ct.Adressen.Rows
                    'Bei alter Zieladresse aussteigen
                    If Row("FAHRT") = ZielFahrt Then Exit For

                    NewRow = NewAdressen.NewRow
                    For Each Col As DataColumn In ct.Adressen.Columns
                        NewRow(Col.ColumnName) = Row(Col.ColumnName)
                    Next
                    NewAdressen.Rows.Add(NewRow)
                    NewAdressen.AcceptChanges()
                Next


                NewRow = NewFahrten.NewRow

                NewRow("FAHRT") = NewFahrten.Rows.Count
                NewRow("FAHRZEUG") = "2"
                NewRow("REIHENFOLGE") = NewFahrten.Rows.Count
                NewRow("TRANSPORTTYP") = ddlRZuTransporttyp.SelectedItem.Value

                If txtRueckZuDatum.Text.Length > 0 Then
                    NewRow("VDATU") = CDate(txtRueckZuDatum.Text)
                End If


                'NewRow("VTIMEU") = Replace(txtRueckZuUhrzeit.Text, ":", "")
                If ddlRueckZuUhrzeit.SelectedValue = "0-0" Then
                    NewRow("AT_TIM_VON") = ""
                    NewRow("AT_TIM_BIS") = ""
                Else

                    Dim TimeSpan As String() = Split(ddlRueckZuUhrzeit.SelectedValue, "-")
                    NewRow("AT_TIM_VON") = TimeSpan(0)
                    NewRow("AT_TIM_BIS") = TimeSpan(1)
                End If


                NewRow("KENNZ_ZUS_FAHT") = "X"

                NewFahrten.Rows.Add(NewRow)
                NewFahrten.AcceptChanges()


                'Adressen
                NewRow = NewAdressen.NewRow

                NewRow("FAHRT") = NewAdressen.Rows.Count
                NewRow("NAME") = txtRueckZuFirma.Text
                NewRow("NAME_2") = txtRueckZuAnsprechpartner.Text
                NewRow("STREET") = txtRueckZuStrasse.Text
                NewRow("POSTL_CODE") = txtRueckZuPLZ.Text
                NewRow("CITY") = txtRueckZuOrt.Text
                NewRow("COUNTRY") = ddlRueckZuLand.SelectedValue
                NewRow("TELEPHONE") = txtRueckZuTelefon.Text


                NewAdressen.Rows.Add(NewRow)
                NewAdressen.AcceptChanges()


                Dim SelRow As DataRow()

                'Daten der alten Zielfahrt ermitteln
                SelRow = ct.Fahrten.Select("FAHRZEUG = '2' AND KENNZ_ZUS_FAHT <> 'X'")

                NewRow = NewFahrten.NewRow


                NewRow("FAHRT") = (CInt(ZielFahrt) + 1).ToString
                NewRow("FAHRZEUG") = SelRow(0)("FAHRZEUG")
                NewRow("REIHENFOLGE") = (CInt(ZielFahrt) + 1).ToString
                NewRow("TRANSPORTTYP") = SelRow(0)("TRANSPORTTYP")
                NewRow("VDATU") = SelRow(0)("VDATU")
                'NewRow("VTIMEU") = SelRow(0)("VTIMEU")
                NewRow("AT_TIM_VON") = SelRow(0)("AT_TIM_VON")
                NewRow("AT_TIM_BIS") = SelRow(0)("AT_TIM_BIS")



                NewRow("KENNZ_ZUS_FAHT") = SelRow(0)("KENNZ_ZUS_FAHT")

                NewFahrten.Rows.Add(NewRow)
                NewFahrten.AcceptChanges()

                'Daten der alten Zielfahrt ermitteln
                SelRow = ct.Adressen.Select("FAHRT = '" & ZielFahrt & "'")

                NewRow = NewAdressen.NewRow

                NewRow("FAHRT") = (CInt(ZielFahrt) + 1).ToString
                NewRow("NAME") = SelRow(0)("NAME")
                NewRow("NAME_2") = SelRow(0)("NAME_2")
                NewRow("STREET") = SelRow(0)("STREET")
                NewRow("POSTL_CODE") = SelRow(0)("POSTL_CODE")
                NewRow("CITY") = SelRow(0)("CITY")
                NewRow("TELEPHONE") = SelRow(0)("TELEPHONE")
                NewRow("COUNTRY") = SelRow(0)("COUNTRY")

                NewAdressen.Rows.Add(NewRow)
                NewAdressen.AcceptChanges()


                ct.Fahrten = NewFahrten
                ct.Adressen = NewAdressen


                'Alte Dienstleistungen und Bemerkungen aktualisieren
                For Each Row As DataRow In ct.Dienstleistungen.Rows

                    If Row("FAHRT") = ZielFahrt Then
                        Row("FAHRT") = (CInt(ZielFahrt + 1)).ToString
                    End If

                    ct.Dienstleistungen.AcceptChanges()

                Next

                For Each Row As DataRow In ct.Bemerkungen.Rows

                    If Row("FAHRT") = ZielFahrt Then
                        Row("FAHRT") = (CInt(ZielFahrt + 1)).ToString
                    End If

                    ct.Bemerkungen.AcceptChanges()

                Next

            End If



            ct.EditRueckFahrt = ""

            lblZusatz.Text = "Zusatzfahrt hinzufügen"

            Session("Transfer") = ct

            ct = Nothing

            CloseRueckZusatzfahrt()

            pnlRueckZieladresse.Visible = True
            ibtHeaderZielRueckClose.Visible = True
            ibtHeaderZielRueck.Visible = False


        End Sub

        Private Sub CloseRueckZusatzfahrt()
            pnlRZusatzfahrten.Visible = False
            ibtRZOpen.Visible = True
            ibtRZClose.Visible = False
            'ibtRZSave.Visible = False

            FillRueckGridZusatz()

        End Sub


        Private Function CheckRueckZusatz() As Boolean
            Dim booError As Boolean = False

            resetRueckZusatz()
            Dim ct As Transfer = CType(Session("Transfer"), Transfer)

            If txtRueckZuFirma.Text.Length = 0 Then
                SetErrorFrame(txtRueckZuFirma) : booError = True
            End If

            If txtRueckZuStrasse.Text.Length = 0 Then
                SetErrorFrame(txtRueckZuStrasse) : booError = True
            End If


            If CInt(ct.Laender.Select("Land1='" & ddlRueckZuLand.SelectedItem.Value & "'")(0)("Lnplz")) > 0 Then
                If Not CInt(ct.Laender.Select("Land1='" & ddlRueckZuLand.SelectedItem.Value & "'")(0)("Lnplz")) = txtRueckZuPLZ.Text.Trim(" "c).Length Then
                    SetErrorFrame(txtRueckZuPLZ) : booError = True
                End If
            End If

            If txtRueckZuPLZ.Text.Length = 0 Then
                SetErrorFrame(txtRueckZuPLZ) : booError = True
            End If

            If txtRueckZuOrt.Text.Length = 0 Then
                SetErrorFrame(txtZuOrt) : booError = True
            End If

            If txtRueckZuAnsprechpartner.Text.Length = 0 Then
                SetErrorFrame(txtRueckZuAnsprechpartner) : booError = True
            End If

            If txtRueckZuTelefon.Text.Length = 0 Then
                SetErrorFrame(txtRueckZuTelefon) : booError = True
            End If

            If ddlRZuTransporttyp.SelectedItem.Value = "00" Then
                ddlRZuTransporttyp.Style.Add("border", "solid 1px red") : booError = True
            End If

            Return booError


        End Function

        Private Sub resetRueckZusatz()
            lblErrorFahrten.Visible = False

            ResetErrorFrame(txtRueckZuFirma)
            ResetErrorFrame(txtRueckZuStrasse)
            ResetErrorFrame(txtRueckZuPLZ)
            ResetErrorFrame(txtRueckZuOrt)
            ResetErrorFrame(txtRueckZuAnsprechpartner)
            ResetErrorFrame(txtRueckZuTelefon)
            ResetErrorFrame(txtRueckZuDatum)
            'ResetErrorFrame(txtRueckZuUhrzeit)
            divRZuTransporttyp.Style.Remove("border")

        End Sub


        Private Sub FillRueckGridZusatz(Optional ByRef DL As Boolean = False)

            Dim ct As Transfer = CType(Session("Transfer"), Transfer)
            Dim tmpTable As New DataTable

            tmpTable.Columns.Add("FAHRT", GetType(System.String))
            tmpTable.Columns.Add("Info", GetType(System.String))
            tmpTable.Columns.Add("Transporttyp", GetType(System.String))
            tmpTable.Columns.Add("InfoTooltip", GetType(System.String))

            tmpTable.AcceptChanges()

            Dim nRow As DataRow
            Dim FahrtNummer As String = ""
            Dim InfoTooltip As String = ""

            For Each Row As DataRow In ct.Fahrten.Rows

                nRow = tmpTable.NewRow
                InfoTooltip = ""

                If Row("FAHRZEUG") = "2" AndAlso Row("KENNZ_ZUS_FAHT") = "X" Then
                    FahrtNummer = Row("FAHRT")

                    nRow("FAHRT") = FahrtNummer
                    nRow("Transporttyp") = ct.Transporttyp.Select("ID = '" & Row("TRANSPORTTYP") & "'")(0)("Text").ToString
                    nRow("Info") = "Zusatzfahrt " & ct.Adressen.Select("FAHRT = '" & FahrtNummer & "'")(0)("CITY").ToString

                    InfoTooltip = ct.Adressen.Select("FAHRT = '" & FahrtNummer & "'")(0)("NAME").ToString & vbCrLf
                    InfoTooltip &= ct.Adressen.Select("FAHRT = '" & FahrtNummer & "'")(0)("STREET").ToString & vbCrLf
                    InfoTooltip &= ct.Adressen.Select("FAHRT = '" & FahrtNummer & "'")(0)("POSTL_CODE").ToString & " " & ct.Adressen.Select("FAHRT = '" & FahrtNummer & "'")(0)("CITY").ToString & vbCrLf
                    InfoTooltip &= "Telefon: " & ct.Adressen.Select("FAHRT = '" & FahrtNummer & "'")(0)("TELEPHONE").ToString

                    nRow("InfoTooltip") = InfoTooltip

                    tmpTable.Rows.Add(nRow)

                End If
            Next

            tmpTable.AcceptChanges()

            If tmpTable.Rows.Count > 0 Then

                If DL = False Then
                    grvRZusatz.DataSource = tmpTable.DefaultView

                    grvRZusatz.DataBind()

                    grvRZusatz.Visible = True

                    PrepareRueckGrid()
                Else
                    grvRueckZusatzDL.DataSource = tmpTable.DefaultView

                    grvRueckZusatzDL.DataBind()

                    trRueckZusatzDL.Visible = True
                End If




            Else
                grvRZusatz.Visible = False

            End If
        End Sub

        Private Sub PrepareRueckGrid()


            Dim Last As Integer


            Last = (grvRZusatz.Rows.Count - 1)


            If grvRZusatz.Rows.Count = 1 Then
                CType(grvRZusatz.Rows(0).FindControl("ibtArrowUp"), ImageButton).Visible = False
                CType(grvRZusatz.Rows(0).FindControl("ibtArrowDown"), ImageButton).Visible = False
                Exit Sub
            End If

            'Erster Eintrag
            CType(grvRZusatz.Rows(0).FindControl("ibtArrowUp"), ImageButton).Visible = False

            'Letzter Eintrag
            CType(grvRZusatz.Rows(Last).FindControl("ibtArrowDown"), ImageButton).Visible = False

        End Sub


        Private Sub grvRZusatz_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles grvRZusatz.RowCommand

            Dim ct As Transfer = CType(Session("Transfer"), Transfer)

            Dim lbl As Label

            lbl = CType(grvRZusatz.Rows(CInt(e.CommandArgument)).FindControl("ID"), Label)

            Select Case e.CommandName
                Case "arrowup"
                    ChangeSortOrder(CInt(lbl.Text), "Up")
                Case "arrowdown"
                    ChangeSortOrder(CInt(lbl.Text), "Down")
                Case "Del"
                    'Fahrten
                    ct.Fahrten.Rows(CInt(lbl.Text)).Delete()
                    ct.Fahrten.AcceptChanges()
                    'Adressen
                    ct.Adressen.Rows(CInt(lbl.Text)).Delete()
                    ct.Adressen.AcceptChanges()
                    'Dienstleistungen
                    If ct.Dienstleistungen.Select("FAHRT = '" & lbl.Text & "'").Length > 0 Then
                        ct.Dienstleistungen.Rows(CInt(lbl.Text)).Delete()
                        ct.Dienstleistungen.AcceptChanges()
                    End If

                    'Bemerkungen
                    If ct.Bemerkungen.Select("FAHRT = '" & lbl.Text & "'").Length > 0 Then
                        ct.Bemerkungen.Rows(CInt(lbl.Text)).Delete()
                        ct.Bemerkungen.AcceptChanges()
                    End If

                    'Fahrten neu aufbauen
                    For i = 0 To ct.Fahrten.Rows.Count - 1

                        ct.Fahrten.Rows(i)("FAHRT") = (i).ToString
                        ct.Fahrten.Rows(i)("REIHENFOLGE") = (i).ToString
                        ct.Adressen.Rows(i)("FAHRT") = (i).ToString

                    Next

                    'Dienstleisungen anpassen
                    For Each Row As DataRow In ct.Dienstleistungen.Rows

                        Row("FAHRT") = (CInt(Row("FAHRT") - 1)).ToString
                        ct.Dienstleistungen.AcceptChanges()
                    Next

                    'Bemerkungen anpassen
                    For Each Row As DataRow In ct.Bemerkungen.Rows

                        Row("FAHRT") = (CInt(Row("FAHRT") - 1)).ToString
                        ct.Bemerkungen.AcceptChanges()
                    Next

                    Session("Transfer") = ct

                    FillRueckGridZusatz()


                Case "edit"
                    'Info, welche Zeile in der Tabelle geladen werden soll

                    ct.EditRueckFahrt = lbl.Text

                    Session("Transfer") = ct

                    ct = Nothing


                    LoadRueckSingleZusatzfahrt()

            End Select

        End Sub

        Private Sub ChangeRueckSortOrder(ByVal Fahrt As Integer, ByVal Direction As String)

            Dim ct As Transfer = CType(Session("Transfer"), Transfer)

            For i = 0 To ct.Fahrten.Rows.Count - 1

                If Fahrt = i Then

                    Select Case Direction

                        Case "Up"
                            ct.Fahrten.Rows(i)("FAHRT") = (Fahrt - 1).ToString
                            ct.Fahrten.Rows(i)("REIHENFOLGE") = (Fahrt - 1).ToString
                            ct.Fahrten.AcceptChanges()

                            ct.Fahrten.Rows(i - 1)("FAHRT") = (Fahrt).ToString
                            ct.Fahrten.Rows(i - 1)("REIHENFOLGE") = (Fahrt).ToString
                            ct.Fahrten.AcceptChanges()

                            ct.Adressen.Rows(i)("FAHRT") = (Fahrt - 1).ToString
                            ct.Adressen.AcceptChanges()
                            ct.Adressen.Rows(i - 1)("FAHRT") = (Fahrt).ToString
                            ct.Adressen.AcceptChanges()

                            Exit For

                        Case "Down"
                            ct.Fahrten.Rows(i)("FAHRT") = (Fahrt + 1).ToString
                            ct.Fahrten.Rows(i)("REIHENFOLGE") = (Fahrt + 1).ToString
                            ct.Fahrten.AcceptChanges()

                            ct.Fahrten.Rows(i + 1)("FAHRT") = (Fahrt).ToString
                            ct.Fahrten.Rows(i + 1)("REIHENFOLGE") = (Fahrt).ToString
                            ct.Fahrten.AcceptChanges()

                            ct.Adressen.Rows(i)("FAHRT") = (Fahrt + 1).ToString
                            ct.Adressen.AcceptChanges()
                            ct.Adressen.Rows(i + 1)("FAHRT") = (Fahrt).ToString
                            ct.Adressen.AcceptChanges()

                            Exit For

                    End Select


                End If


            Next


            ct.Fahrten.DefaultView.Sort = "FAHRT ASC"
            ct.Fahrten = ct.Fahrten.DefaultView.ToTable

            ct.Adressen.DefaultView.Sort = "FAHRT ASC"
            ct.Adressen = ct.Adressen.DefaultView.ToTable

            Session("Transfer") = ct

            FillRueckGridZusatz()

        End Sub

        Private Sub LoadRueckSingleZusatzfahrt()


            OpenRueckZusatzfahrt()

            lblRueckZusatz.Text = "Rückholung: Zusatzfahrt bearbeiten"

            grvRZusatz.Visible = False

            Dim ct As Transfer = CType(Session("Transfer"), Transfer)


            Dim SelRow As DataRow()


            SelRow = ct.Fahrten.Select("FAHRT = '" & ct.EditRueckFahrt & "'")

            If SelRow.Length > 0 Then
                ddlRZuTransporttyp.SelectedValue = SelRow(0)("TRANSPORTTYP").ToString
                txtRueckZuDatum.Text = SelRow(0)("VDATU").ToString
                'txtRueckZuUhrzeit.Text = SelRow(0)("VTIMEU").ToString
                If SelRow(0)("AT_TIM_VON") = "" Then
                    ddlRueckZuUhrzeit.SelectedValue = "0-0"
                Else
                    ddlRueckZuUhrzeit.SelectedValue = SelRow(0)("AT_TIM_VON") & "-" & SelRow(0)("AT_TIM_BIS")

                End If


            End If

            SelRow = ct.Adressen.Select("FAHRT = '" & ct.EditRueckFahrt & "'")
            If SelRow.Length > 0 Then
                txtRueckZuFirma.Text = SelRow(0)("NAME").ToString
                txtRueckZuAnsprechpartner.Text = SelRow(0)("NAME_2").ToString
                txtRueckZuStrasse.Text = SelRow(0)("STREET").ToString
                txtRueckZuPLZ.Text = SelRow(0)("POSTL_CODE").ToString
                txtRueckZuOrt.Text = SelRow(0)("CITY").ToString
                ddlRueckZuLand.SelectedValue = SelRow(0)("COUNTRY").ToString
                txtRueckZuTelefon.Text = SelRow(0)("TELEPHONE").ToString
            End If


        End Sub


        Private Sub OpenRueckZusatzfahrt()

            lblRueckZusatz.Text = "Zusatzfahrt hinzufügen"

            If CheckFahrten() = True Then
                lblErrorFahrten.Visible = True
                Exit Sub
            Else
                WriteDataAbholUndZieladresse()
            End If

            'pnlRZusatzfahrten.Visible = True
            'ibtRZOpen.Visible = False
            'ibtRZClose.Visible = True
            'ibtRZSave.Visible = True

            txtRueckZuFirma.Text = ""
            txtRueckZuStrasse.Text = ""
            txtRueckZuPLZ.Text = ""
            txtRueckZuOrt.Text = ""
            txtRueckZuAnsprechpartner.Text = ""
            txtRueckZuTelefon.Text = ""
            txtRueckZuDatum.Text = ""
            'txtRueckZuUhrzeit.Text = ""
            ddlRZuTransporttyp.SelectedValue = "00"
            ddlRueckZuUhrzeit.SelectedValue = "0-0"


            If pnlRZusatzfahrten.Visible = True Then
                CloseFahrtenPanels()
                FillRueckGridZusatz()
            Else
                CloseFahrtenPanels()
                pnlRZusatzfahrten.Visible = True
                ibtRZOpen.Visible = False
                ibtRZClose.Visible = True
            End If


        End Sub

#End Region

#Region "Testdaten"

        Private Sub TestdatenStamm()

            txtFahrgestellnummer.Text = "WAUZZZ8E77A195842"

            'txtRuFahrgestellnummer.Text = "WBAVU31080KY40023"
            drpFahrzeugwert.SelectedIndex = 1

            rblZugelassen.SelectedIndex = 0
            rblBeauftragt.SelectedIndex = 0
            rblBereifung.SelectedIndex = 0
            rblFahrzeugklasse.SelectedIndex = 0

        End Sub

        Private Sub TestdatenFahrt()
            txtAbAnsprechpartner.Text = "Muster Ansprechpartner"
            'txtAbDatum.Text = Date.Now.ToShortDateString
            txtAbFirma.Text = "Muster AG"
            txtAbOrt.Text = "Unterschleißheim"
            txtAbPLZ.Text = "85716"
            txtAbStrasse.Text = "Konrad-Zuse-Straße 1"
            txtAbTelefon.Text = "893475577"
            'ddlAbTransporttyp.SelectedIndex = 1

            txtZielAnsprechpartner.Text = "Zielansprech"
            'txtZielDatum.Text = Date.Now.ToShortDateString
            txtZielFirma.Text = "Ziel GmbH"
            txtZielOrt.Text = "Ahrensburg"
            txtZielPLZ.Text = "22926"
            txtZielStrasse.Text = "Ladestraße 1"
            txtZielTelefon.Text = "84363666"
            ddlZielTransporttyp.SelectedIndex = 1



        End Sub

#End Region


        Private Sub LinkButton1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LinkButton1.Click
            VersandTabPanel1.Visible = True
            VersandTabPanel2.Visible = False
            VersandTabPanel3.Visible = False
            VersandTabPanel4.Visible = False
            LinkButton1.CssClass = "VersandButtonStamm"
            LinkButton2.CssClass = "LogistikButtonTourDisabled"
            LinkButton3.CssClass = "LogistikButtonDL_Disabled"
            LinkButton4.CssClass = "VersandButtonOverviewEnabled"
            'LinkButton3.Enabled = False


            lblSteps.Text = "Schritt 1 von 4"
            Panel1.CssClass = "StepActive"
            Panel2.CssClass = "Steps"
            Panel3.CssClass = "Steps"
            Panel4.CssClass = "Steps"

            CloseDlPanels()
            CloseFahrtenPanels()

        End Sub

        Protected Sub ibtnShowOptions_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtnShowOptions.Click

            Dim ct As Transfer = CType(Session("Transfer"), Transfer)

            Dim ZielTransporttyp As String


            ZielTransporttyp = ct.Fahrten.Select("FAHRZEUG = '1' and FAHRT <> '0' and KENNZ_ZUS_FAHT <> 'X'")(0)("TRANSPORTTYP").ToString
            ct.EditFahrt = ct.Fahrten.Select("FAHRZEUG = '1' and FAHRT <> '0' and KENNZ_ZUS_FAHT <> 'X'")(0)("FAHRT").ToString


            ct.DienstAuswahl.DefaultView.RowFilter = "EXTGROUP='" & ZielTransporttyp & "' AND ASNUM <> '' AND KTEXT1_H2 = ''"
            chkListGruende.DataSource = ct.DienstAuswahl.DefaultView
            chkListGruende.DataValueField = "ASNUM"
            chkListGruende.DataTextField = "ASKTX"
            chkListGruende.DataBind()

            grvDL.Columns(2).Visible = True
            grvDL.Columns(3).Visible = True


            grvDL.DataSource = ct.DienstAuswahl.DefaultView
            grvDL.DataBind()




            Dim cbx As CheckBox
            Dim lbl As Label
            Dim ibt As ImageButton

            Dim booInfo As Boolean = False
            Dim booPreis As Boolean = False

            'Dim ListGruendeItem As ListItem

            For Each litem As ListItem In chkGruende.Items
                litem.Selected = True

                For Each dr As GridViewRow In grvDL.Rows

                    lbl = CType(dr.FindControl("lblDL"), Label)
                    cbx = CType(dr.FindControl("cbxDL"), CheckBox)

                    If lbl.Text = litem.Value Then
                        cbx.Checked = True
                        Exit For
                    End If

                Next



                'litem.Attributes.Add("onclick", "return false;")

                'ListGruendeItem = chkListGruende.Items.FindByValue(litem.Value)

                'ListGruendeItem.Selected = True

            Next

            For Each dr As GridViewRow In grvDL.Rows

                ibt = CType(dr.FindControl("ibtDL"), ImageButton)

                If dr.Cells(2).Text <> "0,00 €" AndAlso dr.Cells(2).Text <> "" Then
                    booPreis = True
                Else
                    dr.Cells(2).Text = ""
                End If

                If ibt.ToolTip.Length > 0 Then
                    booInfo = True
                End If

            Next

            grvDL.Columns(2).Visible = booPreis
            grvDL.Columns(3).Visible = booInfo

            divOptions.Visible = True
            'divOptions.Style("height") = VersandTabPanel3.Style("height") + 200
            divBackDisabled.Visible = True


            Session("Transfer") = ct

            ct = Nothing

        End Sub

        Protected Sub lbtnCloseOption_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lbtnCloseOption.Click
            divOptions.Visible = False
            divBackDisabled.Visible = False

            For Each litem As ListItem In chkGruende.Items
                litem.Selected = True
                litem.Attributes.Add("onclick", "return false;")
            Next

            ResetOptions()

        End Sub

        Protected Sub lbtnSelectGruende_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lbtnSelectGruende.Click
            Dim ct As Transfer = CType(Session("Transfer"), Transfer)


            Dim cbl As CheckBoxList = chkGruende
            Dim SelRow As DataRow()

            SelRow = ct.Fahrten.Select("FAHRT = '" & ct.EditFahrt & "'")

            If SelRow(0)("FAHRZEUG") = "1" AndAlso SelRow(0)("KENNZ_ZUS_FAHT") <> "X" Then
                cbl = chkGruende
            ElseIf SelRow(0)("FAHRZEUG") = "1" AndAlso SelRow(0)("KENNZ_ZUS_FAHT") = "X" Then
                cbl = chkZusatzGruende
            ElseIf SelRow(0)("FAHRZEUG") = "2" AndAlso SelRow(0)("KENNZ_ZUS_FAHT") <> "X" Then
                cbl = chkGruendeRueck
            ElseIf SelRow(0)("FAHRZEUG") = "2" AndAlso SelRow(0)("KENNZ_ZUS_FAHT") = "X" Then
                cbl = chkRueckZusatzGruende
            End If


            'Die alten Einträge entfernen

            ct.Dienstleistungen.DefaultView.RowFilter = "FAHRT <> '" & ct.EditFahrt & "'"

            ct.Dienstleistungen = ct.Dienstleistungen.DefaultView.ToTable


            Dim DLRow As DataRow
            Dim DLNR As String

            'For Each ListGruendeItem As ListItem In chkListGruende.Items
            '    DLRow = ct.Dienstleistungen.NewRow

            '    If ListGruendeItem.Selected = True Then
            '        DLNR = ListGruendeItem.Value

            '        SelRow = ct.DienstAuswahl.Select("ASNUM = '" & DLNR & "'")
            '        DLRow("Fahrt") = ct.EditFahrt
            '        DLRow("DIENSTL_NR") = SelRow(0)("ASNUM")
            '        DLRow("DIENSTL_TEXT") = SelRow(0)("ASKTX")
            '        DLRow("MATNR") = SelRow(0)("EAN11")
            '        ct.Dienstleistungen.Rows.Add(DLRow)
            '        ct.Dienstleistungen.AcceptChanges()
            '    End If

            'Next

            Dim lbl As Label
            Dim cbx As CheckBox

            For Each dr As GridViewRow In grvDL.Rows

                lbl = CType(dr.FindControl("lblDL"), Label)
                cbx = CType(dr.FindControl("cbxDL"), CheckBox)


                If cbx.Checked = True Then
                    DLRow = ct.Dienstleistungen.NewRow

                    DLNR = lbl.Text

                    SelRow = ct.DienstAuswahl.Select("ASNUM = '" & DLNR & "'")
                    DLRow("Fahrt") = ct.EditFahrt
                    DLRow("DIENSTL_NR") = SelRow(0)("ASNUM")
                    DLRow("DIENSTL_TEXT") = SelRow(0)("ASKTX")
                    DLRow("MATNR") = SelRow(0)("EAN11")
                    ct.Dienstleistungen.Rows.Add(DLRow)
                    ct.Dienstleistungen.AcceptChanges()
                End If


            Next


            ct.Dienstleistungen.DefaultView.RowFilter = "Fahrt = '" & ct.EditFahrt & "'"

            cbl.DataSource = ct.Dienstleistungen.DefaultView
            cbl.DataValueField = "DIENSTL_NR"
            cbl.DataTextField = "DIENSTL_TEXT"
            cbl.DataBind()

            For Each LItem As ListItem In cbl.Items
                LItem.Selected = True
                LItem.Attributes.Add("onclick", "return false;")
            Next


            divOptions.Visible = False
            divBackDisabled.Visible = False
            ResetOptions()

            Session("Transfer") = ct


        End Sub

        Private Sub grvZusatzDL_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles grvZusatzDL.RowCommand


            Dim ibt As ImageButton

            Dim NewCommand As String = e.CommandName

            If NewCommand = "Edit" Then
                If trZusatz2DL.Visible = True Then
                    NewCommand = "Close"
                Else
                    NewCommand = "Open"
                End If

            End If


            Select Case NewCommand
                Case "Open"
                    CloseDlPanels()
                    trZusatz2DL.Visible = True

                    ibt = CType(grvZusatzDL.Rows(CInt(e.CommandArgument)).FindControl("ibtDlZuOpen"), ImageButton)
                    ibt.Visible = False

                    ibt = CType(grvZusatzDL.Rows(CInt(e.CommandArgument)).FindControl("ibtDlZuClose"), ImageButton)
                    ibt.Visible = True


                    For i = 0 To grvZusatzDL.Rows.Count - 1
                        If CInt(e.CommandArgument) <> i Then
                            ibt = CType(grvZusatzDL.Rows(i).FindControl("ibtDlZuOpen"), ImageButton)
                            ibt.Visible = False
                        End If
                    Next


                    Dim ct As Transfer = CType(Session("Transfer"), Transfer)
                    Dim lbl As Label = CType(grvZusatzDL.Rows(CInt(e.CommandArgument)).FindControl("ID"), Label)

                    ct.ZuActiveFahrt = lbl.Text
                    ct.EditFahrt = lbl.Text

                    ct.Dienstleistungen.DefaultView.RowFilter = "FAHRT='" & ct.ZuActiveFahrt & "'"
                    chkZusatzGruende.DataSource = ct.Dienstleistungen.DefaultView
                    chkZusatzGruende.DataValueField = "DIENSTL_NR"
                    chkZusatzGruende.DataTextField = "DIENSTL_TEXT"
                    chkZusatzGruende.DataBind()

                    For Each litem As ListItem In chkZusatzGruende.Items
                        litem.Selected = True
                        litem.Attributes.Add("onclick", "return false;")

                    Next

                    txtBemerkungZusatz.Text = ""

                    'Bemerkung laden
                    If ct.Bemerkungen.Select("FAHRT = '" & lbl.Text & "'").Length > 0 Then


                        For Each dr As DataRow In ct.Bemerkungen.Rows
                            txtBemerkungZusatz.Text = txtBemerkungZusatz.Text & dr("BEMERKUNG")
                        Next
                        'Else
                        '    txtBemerkungZusatz.Text = ""
                    End If


                    Session("Transfer") = ct

                    ct = Nothing

                Case "Close"

                    Dim ct As Transfer = CType(Session("Transfer"), Transfer)

                    ct.ZuActiveFahrt = ""
                    Session("Transfer") = ct

                    ct = Nothing

                    trZusatz2DL.Visible = False

                    For i = 0 To grvZusatzDL.Rows.Count - 1
                        ibt = CType(grvZusatzDL.Rows(i).FindControl("ibtDlZuOpen"), ImageButton)
                        ibt.Visible = True

                        ibt = CType(grvZusatzDL.Rows(i).FindControl("ibtDlZuClose"), ImageButton)
                        ibt.Visible = False

                    Next


            End Select
        End Sub

        Protected Sub lbtnSend_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lbtnSend.Click

            Dim ct As Transfer = CType(Session("Transfer"), Transfer)

            'Vor dem Speichern ggf. nochmal prüfen, ob alle hochgeladenen Protokolle auch wirklich auf dem Server vorhanden sind
            If Not ct.ProtokollArten Is Nothing Then
                Dim blnDateiNichtGefunden As Boolean = False
                Dim strDateienFehlgeschlagen As String = ""
                Dim filename As String = ""
                Dim filepath As String = ""
                Dim kunnummer As String = getKundennummer()
                Dim uploadPath = ConfigurationManager.AppSettings("LogistikCCProtocolUploadTemp")
                If String.IsNullOrEmpty(uploadPath) Then
                    uploadPath = "C:\inetpub\wwwroot\Services\Components\ComCommon\Logistik\Dokumente\TempDoc\" 'default
                End If
                For Each dr As DataRow In ct.ProtokollArten.Rows
                    filename = dr("Filename")
                    If filename <> "" Then
                        filepath = Path.Combine(uploadPath, kunnummer)
                        If Not File.Exists(Path.Combine(filepath, filename)) Then
                            dr("Filename") = ""
                            blnDateiNichtGefunden = True
                            strDateienFehlgeschlagen &= " " & filename
                        End If
                    End If
                Next
                If blnDateiNichtGefunden Then
                    ct.ProtokollArten.AcceptChanges()
                    Session("Transfer") = ct
                    lblError.Visible = True
                    lblError.Text = "Ihr Auftrag konnte nicht gespeichert werden. Folgende hochgeladene Datei(en) nicht gefunden: " & strDateienFehlgeschlagen
                    Exit Sub
                End If
            End If

            '# Debugausgabe der Tabelleninhalte

            'ct.Fahrten.WriteXml("C:/Temp/Fahrten.xml")
            'ct.Adressen.WriteXml("C:/Temp/Adressen.xml")
            'ct.Dienstleistungen.WriteXml("C:/Temp/Dienstleistungen.xml")
            'ct.Fahrzeuge.WriteXml("C:/Temp/Fahrzeuge.xml")
            'ct.Bemerkungen.WriteXml("C:/Temp/Bemerkungen.xml")
            'ct.ProtokollArten.WriteXml("C:/Temp/ProtokollArten.xml")

            '#

            ct.Save(m_User, Me.Page, getKundennummer())

            Dim ReturnTable As DataTable = ct.ReturnTable
            Dim booError As Boolean = False
            Dim strExmessage As String = ""

            Try
                For Each Row As DataRow In ReturnTable.Rows
                    If Row("VBELN") = "" Then
                        booError = True
                    Else
                        If Not ct.ProtokollArten Is Nothing Then
                            TransferFiles(ct, Row("VBELN").ToString, Row("Fahrt").ToString, getKundennummer())
                        End If
                    End If
                Next
            Catch ex As Exception
                booError = True
                strExmessage = ex.Message
            End Try

            lblError.Text = ""
            lblError.Visible = True

            If booError = True Then
                lblError.Text = "Ihr Auftrag konnte nicht gespeichert werden. (Fehler: " & strExmessage & ")"
                lblError.Font.Size = 12
            Else
                lblError.Text = "Ihr Auftrag wurde erfolgreich übermittelt."
                lblError.ForeColor = Drawing.Color.Green
                lblError.Font.Size = 12
                LinkButton4.CssClass = "VersandButtonOverviewReady"
                lblPDFPrint.Visible = True
                ibtnCreatePDF.Visible = True
            End If

            lbtnBackToStart.Visible = True
            lbtNewOrderSameAdress.Visible = True
            lbtnSend.Visible = False
            LinkButton1.Enabled = False
            LinkButton2.Enabled = False
            LinkButton3.Enabled = False
            LinkButton4.Enabled = False

        End Sub

        'Private Sub lbtnNextToOverView_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbtnNextToOverView.Click

        '    NextToOverview()

        'End Sub

        Private Sub NextToOverview()
            lblErrorDienst.Visible = False


            cmdJaWarnung.Visible = True
            cmdNeinWarnung.Visible = True
            cmdOKWarnung.Visible = False


            If CheckMaxLength() = True Then
                lblErrorDienst.Visible = True
                Exit Sub
            End If
            Dim ct As Transfer = CType(Session("Transfer"), Transfer)

            VersandTabPanel1.Visible = False
            VersandTabPanel2.Visible = False
            VersandTabPanel3.Visible = False
            VersandTabPanel4.Visible = True
            LinkButton1.CssClass = "VersandButtonStammReady"
            LinkButton2.CssClass = "LogistikButtonTourReady"
            LinkButton3.CssClass = "LogistikButtonDL_Ready"
            LinkButton4.CssClass = "VersandButtonOverview"

            LinkButton3.Enabled = True

            lblSteps.Text = "Schritt 4 von 4"
            Panel1.CssClass = "StepActive"
            Panel2.CssClass = "StepActive"
            Panel3.CssClass = "StepActive"
            Panel4.CssClass = "StepActive"

            Dim Abhol As New DataTable
            Dim Rechnungsdaten As New DataTable


            Abhol.Columns.Add("Adresse", GetType(System.String))
            Abhol.Columns.Add("PLZOrt", GetType(System.String))
            Abhol.Columns.Add("Termin", GetType(System.String))

            Abhol.AcceptChanges()


            Rechnungsdaten = Abhol.Clone
            Rechnungsdaten.AcceptChanges()




            Dim Fahrt As String

            'Fahrzeug 1
            Dim SelRow As DataRow()

            SelRow = ct.Fahrten.Select("FAHRZEUG = '1'")

            Dim NewRow As DataRow

            lblUeKennzeichen.Text = "Fahrzeug 1" & " (" & ct.Fahrzeuge.Select("FAHRZEUG = '1'")(0)("ZZKENN") & ")"

            For i = 0 To SelRow.Length - 1
                NewRow = Abhol.NewRow

                If i = 0 Then
                    NewRow("Adresse") = "Abholadresse"
                End If

                If i > 0 AndAlso SelRow(i)("KENNZ_ZUS_FAHT") <> "X" Then
                    NewRow("Adresse") = "Zieladresse"
                End If

                If SelRow(i)("KENNZ_ZUS_FAHT") = "X" Then
                    NewRow("Adresse") = "Zusatzfahrt " & SelRow(i)("FAHRT") & " / " & ct.Transporttyp.Select("ID = '" & SelRow(i)("TRANSPORTTYP").ToString & "'")(0)("Text").ToString
                End If

                Fahrt = SelRow(i)("FAHRT")
                ct.Adressen.DefaultView.RowFilter = "FAHRT = '" & Fahrt & "'"
                NewRow("PLZOrt") = ct.Adressen.DefaultView(0)("COUNTRY") & "-" & ct.Adressen.DefaultView(0)("POSTL_CODE") & " / " & ct.Adressen.DefaultView(0)("CITY")
                ct.Adressen.DefaultView.RowFilter = ""

                If String.IsNullOrEmpty(SelRow(i)("VDATU").ToString) = False Then
                    NewRow("Termin") = CDate(SelRow(i)("VDATU").ToString).ToShortDateString
                End If


                If String.IsNullOrEmpty(SelRow(i)("VTIMEU").ToString) = False Then
                    NewRow("Termin") = NewRow("Termin") & " " & Left(SelRow(i)("VTIMEU"), 2) & ":" & Right(SelRow(i)("VTIMEU"), 2) & " Uhr"
                End If

                Abhol.Rows.Add(NewRow)

            Next

            grvUeHin.DataSource = Abhol.DefaultView
            grvUeHin.DataBind()

            'Rückholung Fahrzeug 2
            SelRow = ct.Fahrten.Select("FAHRZEUG = '2'")


            If SelRow.Length > 0 Then

                lblUeRueckKennzeichen.Text = "Fahrzeug 2" & " (" & ct.Fahrzeuge.Select("FAHRZEUG = '2'")(0)("ZZKENN") & ")"

                Dim Rueck As New DataTable

                Rueck = Abhol.Clone
                Rueck.AcceptChanges()

                For i = 0 To SelRow.Length - 1
                    NewRow = Rueck.NewRow

                    'If i = 0 Then
                    '    NewRow("Adresse") = "Zieladresse Fzg 1 / Abholadresse"
                    'End If

                    If SelRow(i)("KENNZ_ZUS_FAHT") <> "X" Then
                        NewRow("Adresse") = "Zieladresse"
                    End If

                    If SelRow(i)("KENNZ_ZUS_FAHT") = "X" Then
                        NewRow("Adresse") = "Zusatzfahrt " & SelRow(i)("FAHRT") & " / " & ct.Transporttyp.Select("ID = '" & SelRow(i)("TRANSPORTTYP").ToString & "'")(0)("Text").ToString
                    End If

                    Fahrt = SelRow(i)("FAHRT")
                    ct.Adressen.DefaultView.RowFilter = "FAHRT = '" & Fahrt & "'"
                    NewRow("PLZOrt") = ct.Adressen.DefaultView(0)("COUNTRY") & "-" & ct.Adressen.DefaultView(0)("POSTL_CODE") & " / " & ct.Adressen.DefaultView(0)("CITY")
                    ct.Adressen.DefaultView.RowFilter = ""

                    If String.IsNullOrEmpty(SelRow(i)("VDATU").ToString) = False Then
                        NewRow("Termin") = CDate(SelRow(i)("VDATU").ToString).ToShortDateString
                    End If

                    If String.IsNullOrEmpty(SelRow(i)("VTIMEU").ToString) = False Then
                        NewRow("Termin") = NewRow("Termin") & " " & Left(SelRow(i)("VTIMEU"), 2) & ":" & Right(SelRow(i)("VTIMEU"), 2) & " Uhr"
                    End If

                    Rueck.Rows.Add(NewRow)
                    Rueck.AcceptChanges()

                Next

                trUeRueck.Visible = True



                grvUeRueck.DataSource = Rueck.DefaultView
                grvUeRueck.DataBind()


            End If

            'Rechnungsdaten
            NewRow = Rechnungsdaten.NewRow

            NewRow("Adresse") = "Rechnungszahler"
            NewRow("PLZOrt") = txtRzFirma.Text & " / " & txtRzPLZ.Text & " / " & txtRzOrt.Text

            Rechnungsdaten.Rows.Add(NewRow)
            Rechnungsdaten.AcceptChanges()

            NewRow = Rechnungsdaten.NewRow
            NewRow("Adresse") = "Rechnungsempfänger"
            NewRow("PLZOrt") = txtArFirma.Text & " / " & txtArPLZ.Text & " / " & txtArOrt.Text

            Rechnungsdaten.Rows.Add(NewRow)
            Rechnungsdaten.AcceptChanges()

            grvRechnungsdaten.DataSource = Rechnungsdaten.DefaultView
            grvRechnungsdaten.DataBind()

            '****Sonstige Dienstleistung autom. hinzufügen, wenn eine Bemerkung eingetragen wurde und in den Dienstleistungen
            '*   SONSTIGES im Feld USERF2_TXT zum Transporttyp hinterlegt ist.
            '***

            'Dienstleistungen bereinigen
            Dim Transporttyp As String = ""

            If ct.Dienstleistungen.Rows.Count > 0 Then

                Dim i As Integer = ct.Dienstleistungen.Rows.Count - 1

                Do While i > 0
                    'Transporttyp aus Fahrt
                    Transporttyp = ct.Fahrten.Select("FAHRT = '" & ct.Dienstleistungen.Rows(i)("FAHRT") & "'")(0)("TRANSPORTTYP")

                    ct.DienstAuswahl.DefaultView.RowFilter = "ASNUM = '" & ct.Dienstleistungen.Rows(i)("DIENSTL_NR").ToString & "' AND EXTGROUP = '" & Transporttyp & "' AND USERF2_TXT = 'SONSTIGES'"

                    If ct.DienstAuswahl.DefaultView.Count > 0 Then
                        ct.Dienstleistungen.Rows(i).Delete()
                    End If

                    i -= 1
                Loop


            End If


            ct.DienstAuswahl.DefaultView.RowFilter = ""

            ct.Dienstleistungen.AcceptChanges()


            Dim AktuelleFahrt As String = ""
            Dim NewDlRow As DataRow

            For Each BemRow As DataRow In ct.Bemerkungen.Rows

                If AktuelleFahrt <> BemRow("FAHRT") Then

                    'Transporttyp aus Fahrt
                    Transporttyp = ct.Fahrten.Select("FAHRT = '" & BemRow("FAHRT") & "'")(0)("TRANSPORTTYP")

                    ct.DienstAuswahl.DefaultView.RowFilter = "EXTGROUP = '" & Transporttyp & "' AND USERF2_TXT = 'SONSTIGES'"

                    If ct.DienstAuswahl.DefaultView.Count > 0 Then

                        NewDlRow = ct.Dienstleistungen.NewRow

                        NewDlRow("FAHRT") = BemRow("FAHRT")
                        NewDlRow("DIENSTL_NR") = ct.DienstAuswahl.DefaultView(0)("ASNUM")
                        NewDlRow("DIENSTL_TEXT") = ct.DienstAuswahl.DefaultView(0)("ASKTX")
                        NewDlRow("MATNR") = ct.DienstAuswahl.DefaultView(0)("EAN11")

                        ct.Dienstleistungen.Rows.Add(NewDlRow)

                    End If



                    AktuelleFahrt = BemRow("FAHRT")
                End If

            Next

            ct.DienstAuswahl.DefaultView.RowFilter = ""

            'ListViews aktualisieren
            Fahrt = ct.Fahrten.Select("FAHRZEUG = '1' and FAHRT <> '0' and KENNZ_ZUS_FAHT <> 'X'")(0)("FAHRT").ToString

            ct.Dienstleistungen.DefaultView.RowFilter = "FAHRT='" & Fahrt & "'"
            chkGruende.DataSource = ct.Dienstleistungen.DefaultView
            chkGruende.DataValueField = "DIENSTL_NR"
            chkGruende.DataTextField = "DIENSTL_TEXT"
            chkGruende.DataBind()

            For Each litem As ListItem In chkGruende.Items
                litem.Selected = True
                litem.Attributes.Add("onclick", "return false;")

            Next
            If ct.Fahrten.Select("FAHRZEUG = '2' and KENNZ_ZUS_FAHT <> 'X'").Length > 0 Then
                Fahrt = ct.Fahrten.Select("FAHRZEUG = '2' and KENNZ_ZUS_FAHT <> 'X'")(0)("FAHRT").ToString
                ct.Dienstleistungen.DefaultView.RowFilter = "FAHRT='" & Fahrt & "'"
                chkGruendeRueck.DataSource = ct.Dienstleistungen.DefaultView
                chkGruendeRueck.DataValueField = "DIENSTL_NR"
                chkGruendeRueck.DataTextField = "DIENSTL_TEXT"
                chkGruendeRueck.DataBind()

                For Each litem As ListItem In chkGruendeRueck.Items
                    litem.Selected = True
                    litem.Attributes.Add("onclick", "return false;")

                Next
            End If

            trZusatz2DL.Visible = False
            trRuZusatzDL.Visible = False

            Dim ibt As ImageButton

            For Each gvRow As GridViewRow In grvZusatzDL.Rows

                ibt = CType(gvRow.FindControl("ibtDlZuOpen"), ImageButton)
                ibt.Visible = True

                ibt = CType(gvRow.FindControl("ibtDlZuClose"), ImageButton)
                ibt.Visible = False

            Next

            For Each gvRowRueck As GridViewRow In grvRueckZusatzDL.Rows
                ibt = CType(gvRowRueck.FindControl("ibtDlRuOpen"), ImageButton)
                ibt.Visible = True

                ibt = CType(gvRowRueck.FindControl("ibtDlRuClose"), ImageButton)
                ibt.Visible = False
            Next


            Dim booZusatzdienstleistung As Boolean = False
            Dim Meldungstext As String = ""
            Dim Meldungsheader As String = ""





            litMessage.Text = ""

            If IsDate(txtAbDatum.Text) Then

                '***********************************************
                'Fallen die berücksichtigten ExpressDays auf ein Wochenende? Dann entsprechend verlängern.

                ct.Dienstleistungen.DefaultView.RowFilter = "DIENSTL_NR = '" & ct.ExpressDlNummer & "'"
                If ct.Dienstleistungen.DefaultView.Count = 0 Then
                    If ct.Express = True Then
                        Dim ZuDate As Date = CType(txtAbDatum.Text, Date)

                        Dim DiffDate As Integer

                        DiffDate = DateDiff(DateInterval.Day, Now, ZuDate)
                        Dim Weekend As Int16 = 0

                        For i As Integer = 0 To DiffDate

                            If DateAdd(DateInterval.Day, i, Now).DayOfWeek = DayOfWeek.Saturday OrElse _
                               DateAdd(DateInterval.Day, i, Now).DayOfWeek = DayOfWeek.Sunday Then

                                Weekend += 1
                            End If
                        Next


                        If DateAdd(DateInterval.Day, ct.ExpressDays + Weekend, Now.Date) >= ZuDate Then
                            booZusatzdienstleistung = True
                            Meldungsheader = "Expressauftrag"
                            Meldungstext = "Das Wunschlieferdatum liegt weniger als " & ct.ExpressDays & " Werktag(e) <br />in der Zukunft. Sie beauftragen hiermit einen Expressauftrag."


                        End If
                    End If
                End If

                If booZusatzdienstleistung = False Then

                    Dim booDateDiffer As Boolean = False

                    If (CDate(txtZielDatum.Text) > CDate(txtAbDatum.Text)) Then
                        booDateDiffer = True
                    End If

                    SelRow = ct.Fahrten.Select("FAHRZEUG = '1' and KENNZ_ZUS_FAHT = 'X'")

                    If SelRow.Length > 0 Then
                        If CDate(SelRow(0)("VDATU")) > CDate(txtAbDatum.Text) Then
                            booDateDiffer = True
                        End If
                    End If


                    If booDateDiffer = True Then
                        ct.Dienstleistungen.DefaultView.RowFilter = "DIENSTL_NR = '" & ct.VorholDlNummer & "'"
                        If ct.Dienstleistungen.DefaultView.Count = 0 Then
                            If ct.Vorholung = True Then

                                booZusatzdienstleistung = True
                                Meldungsheader = "Vorholung"
                                Meldungstext = "Möchten Sie eine Vorholung des Fahrzeugs <br />am " & txtAbDatum.Text & " beauftragen?"


                            End If
                        End If
                    End If

                End If

            End If


            If ct.SamstagDlNummer <> 0 Then
                If booZusatzdienstleistung = False Then


                    If IsDate(txtAbDatum.Text) = True OrElse IsDate(txtRuZielDatum.Text) = True OrElse IsDate(txtZielDatum.Text) Then


                        Dim booIsSaturday As Boolean = False

                        If IsDate(txtAbDatum.Text) = True Then
                            If CDate(txtAbDatum.Text).DayOfWeek = DayOfWeek.Saturday Then booIsSaturday = True
                        End If

                        If IsDate(txtZielDatum.Text) = True Then
                            If CDate(txtZielDatum.Text).DayOfWeek = DayOfWeek.Saturday Then booIsSaturday = True
                        End If

                        If IsDate(txtRuZielDatum.Text) = True Then
                            If CDate(txtRuZielDatum.Text).DayOfWeek = DayOfWeek.Saturday Then booIsSaturday = True
                        End If

                        ct.Dienstleistungen.DefaultView.RowFilter = "DIENSTL_NR = '" & ct.SamstagDlNummer & "'"
                        If ct.Dienstleistungen.DefaultView.Count = 0 Then
                            If booIsSaturday = True Then
                                Meldungsheader = "Wunschtermin ist ein Samstag"
                                Meldungstext = "Sie haben einen Samstag <br />als Wunschtermin eingegeben.<br />Wollen Sie den Termin beibehalten?"
                                booZusatzdienstleistung = True
                            End If
                        End If

                    End If
                End If
            End If


            If booZusatzdienstleistung = True Then
                lblMsgHeader.Text = Meldungsheader
                litMessage.Text = Meldungstext
                divMessage.Visible = True
                VersandTabPanel1.Visible = False
                VersandTabPanel2.Visible = False
                VersandTabPanel3.Visible = True
                VersandTabPanel4.Visible = False
                LinkButton1.CssClass = "VersandButtonStammReady"
                LinkButton2.CssClass = "LogistikButtonTourReady"
                LinkButton3.CssClass = "LogistikButtonDL"
                LinkButton4.CssClass = "VersandButtonOverviewEnabled"

                LinkButton3.Enabled = False
                LinkButton4.Enabled = False

                lblSteps.Text = "Schritt 3 von 4"
                Panel1.CssClass = "StepActive"
                Panel2.CssClass = "StepActive"
                Panel3.CssClass = "StepActive"
                Panel4.CssClass = "Steps"
            End If



            Session("Transfer") = ct

            ct = Nothing
        End Sub

        Private Sub UpdateDienstleistungen(ByVal ct As Transfer, ByVal SelRow As DataRow, ByVal NewTransporttyp As String)
            Dim Fahrt As String = ""
            ct.Dienstleistungen.DefaultView.RowFilter = "FAHRT='" & SelRow("FAHRT") & "'"

            If ct.Dienstleistungen.DefaultView.Count > 0 Then
                Dim delRow() As DataRow = ct.Dienstleistungen.Select("FAHRT='" & SelRow("FAHRT") & "'")

                If delRow.Length > 0 Then

                    For i = 0 To delRow.Length - 1
                        ct.Dienstleistungen.Rows.Remove(delRow(i))
                        ct.Dienstleistungen.AcceptChanges()
                    Next

                End If
                ct.DienstAuswahl.DefaultView.RowFilter = "EXTGROUP='" & NewTransporttyp & "' AND ASNUM <> '' AND VW_AG = 'X' AND KTEXT1_H2 = ''"
                For i = 0 To ct.DienstAuswahl.DefaultView.Count - 1
                    Dim NewRow As DataRow = ct.Dienstleistungen.NewRow

                    NewRow("FAHRT") = SelRow("FAHRT")
                    NewRow("DIENSTL_NR") = ct.DienstAuswahl.DefaultView(i)("ASNUM")
                    NewRow("DIENSTL_TEXT") = ct.DienstAuswahl.DefaultView(i)("ASKTX")
                    NewRow("MATNR") = ct.DienstAuswahl.DefaultView(i)("EAN11")

                    ct.Dienstleistungen.Rows.Add(NewRow)
                    ct.Dienstleistungen.AcceptChanges()
                Next
            Else
                ct.DienstAuswahl.DefaultView.RowFilter = "EXTGROUP='" & NewTransporttyp & "' AND ASNUM <> '' AND VW_AG = 'X' AND KTEXT1_H2 = ''"
                For i = 0 To ct.DienstAuswahl.DefaultView.Count - 1
                    Dim NewRow As DataRow = ct.Dienstleistungen.NewRow

                    NewRow("FAHRT") = SelRow("FAHRT")
                    NewRow("DIENSTL_NR") = ct.DienstAuswahl.DefaultView(i)("ASNUM")
                    NewRow("DIENSTL_TEXT") = ct.DienstAuswahl.DefaultView(i)("ASKTX")
                    NewRow("MATNR") = ct.DienstAuswahl.DefaultView(i)("EAN11")

                    ct.Dienstleistungen.Rows.Add(NewRow)
                    ct.Dienstleistungen.AcceptChanges()
                Next
            End If
            SelRow("TRANSPORTTYP") = NewTransporttyp
            'Dienstleistungen zur Zieladresse
            Fahrt = ct.Fahrten.Select("FAHRZEUG = '1' and FAHRT <> '0' and KENNZ_ZUS_FAHT <> 'X'")(0)("FAHRT").ToString

            ct.Dienstleistungen.DefaultView.RowFilter = "FAHRT='" & Fahrt & "'"
            chkGruende.DataSource = ct.Dienstleistungen.DefaultView
            chkGruende.DataValueField = "DIENSTL_NR"
            chkGruende.DataTextField = "DIENSTL_TEXT"
            chkGruende.DataBind()

            For Each litem As ListItem In chkGruende.Items
                litem.Selected = True
                litem.Attributes.Add("onclick", "return false;")

            Next
            If ct.Fahrten.Select("FAHRZEUG = '2' and KENNZ_ZUS_FAHT <> 'X'").Length > 0 Then
                Fahrt = ct.Fahrten.Select("FAHRZEUG = '2' and KENNZ_ZUS_FAHT <> 'X'")(0)("FAHRT").ToString
                ct.Dienstleistungen.DefaultView.RowFilter = "FAHRT='" & Fahrt & "'"
                chkGruendeRueck.DataSource = ct.Dienstleistungen.DefaultView
                chkGruendeRueck.DataValueField = "DIENSTL_NR"
                chkGruendeRueck.DataTextField = "DIENSTL_TEXT"
                chkGruendeRueck.DataBind()

                For Each litem As ListItem In chkGruendeRueck.Items
                    litem.Selected = True
                    litem.Attributes.Add("onclick", "return false;")

                Next
            End If

        End Sub

        Private Sub PrepareDienstleistungen()

            Dim ct As Transfer = CType(Session("Transfer"), Transfer)


            If ct.Dienstleistungen.Rows.Count > 0 Then Exit Sub


            Dim Transporttyp As String = ""
            Dim Fahrt As String = ""
            Dim NewRow As DataRow



            'Standarddienstleistungen zu den Fahrten füllen
            For Each Row As DataRow In ct.Fahrten.Rows
                If Row("FAHRT") <> "0" Then

                    'Wurden der Fahrt schon Dienstleistungen zugeordnet 

                    ct.Dienstleistungen.DefaultView.RowFilter = "FAHRT='" & Row("FAHRT") & "'"

                    If ct.Dienstleistungen.DefaultView.Count = 0 Then
                        ct.DienstAuswahl.DefaultView.RowFilter = "EXTGROUP='" & Row("TRANSPORTTYP") & "' AND ASNUM <> '' AND VW_AG = 'X' AND KTEXT1_H2 = ''"

                        If ct.DienstAuswahl.DefaultView.Count > 0 Then

                            For i = 0 To ct.DienstAuswahl.DefaultView.Count - 1
                                NewRow = ct.Dienstleistungen.NewRow

                                NewRow("FAHRT") = Row("FAHRT")
                                NewRow("DIENSTL_NR") = ct.DienstAuswahl.DefaultView(i)("ASNUM")
                                NewRow("DIENSTL_TEXT") = ct.DienstAuswahl.DefaultView(i)("ASKTX")
                                NewRow("MATNR") = ct.DienstAuswahl.DefaultView(i)("EAN11")

                                ct.Dienstleistungen.Rows.Add(NewRow)
                                ct.Dienstleistungen.AcceptChanges()
                            Next

                        End If
                    End If

                End If
            Next


            'Dienstleistungen zur Zieladresse
            Fahrt = ct.Fahrten.Select("FAHRZEUG = '1' and FAHRT <> '0' and KENNZ_ZUS_FAHT <> 'X'")(0)("FAHRT").ToString

            ct.Dienstleistungen.DefaultView.RowFilter = "FAHRT='" & Fahrt & "'"
            chkGruende.DataSource = ct.Dienstleistungen.DefaultView
            chkGruende.DataValueField = "DIENSTL_NR"
            chkGruende.DataTextField = "DIENSTL_TEXT"
            chkGruende.DataBind()

            For Each litem As ListItem In chkGruende.Items
                litem.Selected = True
                litem.Attributes.Add("onclick", "return false;")

            Next


            If tblRueckDL.Visible = True Then
                Fahrt = ct.Fahrten.Select("FAHRZEUG = '2' and KENNZ_ZUS_FAHT <> 'X'")(0)("FAHRT").ToString

                ct.Dienstleistungen.DefaultView.RowFilter = "FAHRT='" & Fahrt & "'"
                chkGruendeRueck.DataSource = ct.Dienstleistungen.DefaultView
                chkGruendeRueck.DataValueField = "DIENSTL_NR"
                chkGruendeRueck.DataTextField = "DIENSTL_TEXT"
                chkGruendeRueck.DataBind()

                For Each litem As ListItem In chkGruendeRueck.Items
                    litem.Selected = True
                    litem.Attributes.Add("onclick", "return false;")

                Next
            End If


            Session("Transfer") = ct


        End Sub

        Protected Sub ibtnShowOptionsZusatz_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtnShowOptionsZusatz.Click
            Dim ct As Transfer = CType(Session("Transfer"), Transfer)

            Dim Transporttyp As String


            Transporttyp = ct.Fahrten.Select("FAHRT = '" & ct.ZuActiveFahrt & "'")(0)("TRANSPORTTYP").ToString


            ct.DienstAuswahl.DefaultView.RowFilter = "EXTGROUP='" & Transporttyp & "' AND ASNUM <> '' AND KTEXT1_H2 = ''"
            chkListGruende.DataSource = ct.DienstAuswahl.DefaultView
            chkListGruende.DataValueField = "ASNUM"
            chkListGruende.DataTextField = "ASKTX"
            chkListGruende.DataBind()

            grvDL.Columns(2).Visible = True
            grvDL.Columns(3).Visible = True

            grvDL.DataSource = ct.DienstAuswahl.DefaultView
            grvDL.DataBind()


            Dim cbx As CheckBox
            Dim lbl As Label
            Dim ibt As ImageButton

            Dim booInfo As Boolean = False
            Dim booPreis As Boolean = False


            'Dim ListGruendeItem As ListItem

            For Each litem As ListItem In chkZusatzGruende.Items
                litem.Selected = True

                For Each dr As GridViewRow In grvDL.Rows

                    lbl = CType(dr.FindControl("lblDL"), Label)
                    cbx = CType(dr.FindControl("cbxDL"), CheckBox)

                    If lbl.Text = litem.Value Then
                        cbx.Checked = True
                        Exit For
                    End If


                    'litem.Attributes.Add("onclick", "return false;")
                    'ListGruendeItem = chkListGruende.Items.FindByValue(litem.Value)

                    'ListGruendeItem.Selected = True
                Next
            Next

            For Each dr As GridViewRow In grvDL.Rows

                ibt = CType(dr.FindControl("ibtDL"), ImageButton)

                If dr.Cells(2).Text <> "0,00 €" AndAlso dr.Cells(2).Text <> "" Then
                    booPreis = True
                Else
                    dr.Cells(2).Text = ""
                End If

                If ibt.ToolTip.Length > 0 Then
                    booInfo = True
                End If

            Next

            grvDL.Columns(2).Visible = booPreis
            grvDL.Columns(3).Visible = booInfo


            divOptions.Visible = True
            'divOptions.Style("height") = VersandTabPanel3.Style("height") + 200
            divBackDisabled.Visible = True


            Session("Transfer") = ct

            ct = Nothing
        End Sub

        Private Sub grvRueckZusatzDL_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles grvRueckZusatzDL.RowCommand
            Dim ibt As ImageButton

            Dim NewCommand As String = e.CommandName

            If NewCommand = "Edit" Then
                If trRuZusatzDL.Visible = True Then
                    NewCommand = "Close"
                Else
                    NewCommand = "Open"
                End If

            End If


            Select Case NewCommand
                Case "Open"

                    CloseDlPanels()

                    trRuZusatzDL.Visible = True

                    ibt = CType(grvRueckZusatzDL.Rows(CInt(e.CommandArgument)).FindControl("ibtDlRuOpen"), ImageButton)
                    ibt.Visible = False

                    ibt = CType(grvRueckZusatzDL.Rows(CInt(e.CommandArgument)).FindControl("ibtDlRuClose"), ImageButton)
                    ibt.Visible = True

                    For i = 0 To grvRueckZusatzDL.Rows.Count - 1
                        If CInt(e.CommandArgument) <> i Then
                            ibt = CType(grvRueckZusatzDL.Rows(i).FindControl("ibtDlRuOpen"), ImageButton)
                            ibt.Visible = False
                        End If
                    Next


                    Dim ct As Transfer = CType(Session("Transfer"), Transfer)
                    Dim lbl As Label = CType(grvRueckZusatzDL.Rows(CInt(e.CommandArgument)).FindControl("ID"), Label)

                    ct.EditFahrt = lbl.Text
                    ct.ZuRueckActiveFahrt = lbl.Text
                    ct.Dienstleistungen.DefaultView.RowFilter = "FAHRT='" & ct.ZuRueckActiveFahrt & "'"
                    chkRueckZusatzGruende.DataSource = ct.Dienstleistungen.DefaultView
                    chkRueckZusatzGruende.DataValueField = "DIENSTL_NR"
                    chkRueckZusatzGruende.DataTextField = "DIENSTL_TEXT"
                    chkRueckZusatzGruende.DataBind()

                    For Each litem As ListItem In chkRueckZusatzGruende.Items
                        litem.Selected = True
                        litem.Attributes.Add("onclick", "return false;")

                    Next

                    txtRueckBemZusatz.Text = ""

                    'Bemerkung laden
                    If ct.Bemerkungen.Select("FAHRT = '" & lbl.Text & "'").Length > 0 Then

                        For Each dr As DataRow In ct.Bemerkungen.Rows
                            txtRueckBemZusatz.Text = txtRueckBemZusatz.Text & dr("BEMERKUNG")
                        Next
                        'Else
                        '    txtRueckBemZusatz.Text = ""
                    End If


                    Session("Transfer") = ct

                    ct = Nothing

                Case "Close"
                    trRuZusatzDL.Visible = False

                    For i = 0 To grvRueckZusatzDL.Rows.Count - 1
                        ibt = CType(grvRueckZusatzDL.Rows(i).FindControl("ibtDlRuOpen"), ImageButton)
                        ibt.Visible = True

                        ibt = CType(grvRueckZusatzDL.Rows(i).FindControl("ibtDlRuClose"), ImageButton)
                        ibt.Visible = False

                    Next


            End Select
        End Sub

        Private Sub ibtRueckDL_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtRueckDL.Click
            Dim ct As Transfer = CType(Session("Transfer"), Transfer)

            Dim Transporttyp As String


            Transporttyp = ct.Fahrten.Select("FAHRZEUG = '2' and KENNZ_ZUS_FAHT <> 'X'")(0)("TRANSPORTTYP").ToString
            ct.EditFahrt = ct.Fahrten.Select("FAHRZEUG = '2' and KENNZ_ZUS_FAHT <> 'X'")(0)("FAHRT").ToString


            ct.DienstAuswahl.DefaultView.RowFilter = "EXTGROUP='" & Transporttyp & "' AND ASNUM <> '' AND KTEXT1_H2 = ''"
            chkListGruende.DataSource = ct.DienstAuswahl.DefaultView
            chkListGruende.DataValueField = "ASNUM"
            chkListGruende.DataTextField = "ASKTX"
            chkListGruende.DataBind()

            grvDL.Columns(2).Visible = True
            grvDL.Columns(3).Visible = True

            grvDL.DataSource = ct.DienstAuswahl.DefaultView
            grvDL.DataBind()


            Dim cbx As CheckBox
            Dim lbl As Label
            Dim ibt As ImageButton



            Dim booInfo As Boolean = False
            Dim booPreis As Boolean = False

            'Dim ListGruendeItem As ListItem

            For Each litem As ListItem In chkGruendeRueck.Items
                litem.Selected = True

                For Each dr As GridViewRow In grvDL.Rows

                    lbl = CType(dr.FindControl("lblDL"), Label)
                    cbx = CType(dr.FindControl("cbxDL"), CheckBox)

                    If lbl.Text = litem.Value Then
                        cbx.Checked = True
                        Exit For
                    End If

                Next

                'litem.Attributes.Add("onclick", "return false;")

                'ListGruendeItem = chkListGruende.Items.FindByValue(litem.Value)

                'ListGruendeItem.Selected = True

            Next

            For Each dr As GridViewRow In grvDL.Rows

                ibt = CType(dr.FindControl("ibtDL"), ImageButton)

                If dr.Cells(2).Text <> "0,00 €" AndAlso dr.Cells(2).Text <> "" Then
                    booPreis = True
                Else
                    dr.Cells(2).Text = ""
                End If

                If ibt.ToolTip.Length > 0 Then
                    booInfo = True
                End If

            Next

            grvDL.Columns(2).Visible = booPreis
            grvDL.Columns(3).Visible = booInfo

            divOptions.Visible = True
            divBackDisabled.Visible = True

            Transporttyp.TrimStart("0"c)
            Session("Transfer") = ct

            ct = Nothing
        End Sub

        Private Sub ibtShowOptionsRueckZusatz_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtShowOptionsRueckZusatz.Click
            Dim ct As Transfer = CType(Session("Transfer"), Transfer)

            Dim Transporttyp As String


            Transporttyp = ct.Fahrten.Select("FAHRT = '" & ct.EditFahrt & "'")(0)("TRANSPORTTYP").ToString


            ct.DienstAuswahl.DefaultView.RowFilter = "EXTGROUP='" & Transporttyp & "' AND ASNUM <> '' AND KTEXT1_H2 = ''"
            chkListGruende.DataSource = ct.DienstAuswahl.DefaultView
            chkListGruende.DataValueField = "ASNUM"
            chkListGruende.DataTextField = "ASKTX"
            chkListGruende.DataBind()

            grvDL.Columns(2).Visible = True
            grvDL.Columns(3).Visible = True

            grvDL.DataSource = ct.DienstAuswahl.DefaultView
            grvDL.DataBind()


            Dim cbx As CheckBox
            Dim lbl As Label
            Dim ibt As ImageButton

            Dim booInfo As Boolean = False
            Dim booPreis As Boolean = False


            'Dim ListGruendeItem As ListItem

            For Each litem As ListItem In chkRueckZusatzGruende.Items
                litem.Selected = True

                For Each dr As GridViewRow In grvDL.Rows

                    lbl = CType(dr.FindControl("lblDL"), Label)
                    cbx = CType(dr.FindControl("cbxDL"), CheckBox)

                    If lbl.Text = litem.Value Then
                        cbx.Checked = True
                        Exit For
                    End If

                Next

                'litem.Attributes.Add("onclick", "return false;")

                'ListGruendeItem = chkListGruende.Items.FindByValue(litem.Value)

                'ListGruendeItem.Selected = True

            Next

            For Each dr As GridViewRow In grvDL.Rows

                ibt = CType(dr.FindControl("ibtDL"), ImageButton)

                If dr.Cells(2).Text <> "0,00 €" AndAlso dr.Cells(2).Text <> "" Then
                    booPreis = True
                Else
                    dr.Cells(2).Text = ""
                End If

                If ibt.ToolTip.Length > 0 Then
                    booInfo = True
                End If

            Next

            grvDL.Columns(2).Visible = booPreis
            grvDL.Columns(3).Visible = booInfo

            divOptions.Visible = True
            divBackDisabled.Visible = True


            Session("Transfer") = ct

            ct = Nothing
        End Sub

        'Protected Sub lbtnBackToTour_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lbtnBackToTour.Click

        '    VersandTabPanel1.Visible = False
        '    VersandTabPanel2.Visible = True
        '    VersandTabPanel3.Visible = False
        '    VersandTabPanel4.Visible = False


        '    If LinkButton3.CssClass = "LogistikButtonDL_Ready" Then
        '        LinkButton3.CssClass = "LogistikButtonDL"
        '    Else
        '        LinkButton3.CssClass = "LogistikButtonDL_Disabled"
        '    End If

        '    LinkButton1.CssClass = "VersandButtonStammReady"
        '    LinkButton2.CssClass = "LogistikButtonTourReady"



        '    lblSteps.Text = "Schritt 2 von 4"
        '    Panel1.CssClass = "StepActive"
        '    Panel2.CssClass = "StepActive"
        '    Panel3.CssClass = "Steps"
        '    Panel4.CssClass = "Steps"
        'End Sub


        Private Sub SaveBemerkungen()

            Dim ct As Transfer = CType(Session("Transfer"), Transfer)
            Dim Fahrt As String = ""
            Dim NewRow As DataRow
            Dim Texte As New ArrayList

            'Bemerkung Zielfahrt
            If ct.Fahrten.Select("FAHRZEUG = '1' and FAHRT <> '0' and KENNZ_ZUS_FAHT <> 'X'").Length > 0 Then

                Fahrt = ct.Fahrten.Select("FAHRZEUG = '1' and FAHRT <> '0' and KENNZ_ZUS_FAHT <> 'X'")(0)("FAHRT").ToString

                'Die alten Einträge löschen
                ct.Bemerkungen.DefaultView.RowFilter = "FAHRT <> '" & Fahrt & "'"

                ct.Bemerkungen = ct.Bemerkungen.DefaultView.ToTable


            End If

            If txtAbBemerkung.Text.Length > 0 Then

                Texte = RowsToInsert(txtAbBemerkung.Text)

                For Each Text As String In Texte
                    NewRow = ct.Bemerkungen.NewRow
                    NewRow("FAHRT") = Fahrt
                    NewRow("TEXT_ID") = "0018"
                    NewRow("BEMERKUNG") = Text
                    ct.Bemerkungen.Rows.Add(NewRow)
                    ct.Bemerkungen.AcceptChanges()
                Next

            End If

            'Rückholung
            If ct.Fahrten.Select("FAHRZEUG = '2' and KENNZ_ZUS_FAHT <> 'X'").Length > 0 Then

                Fahrt = ct.Fahrten.Select("FAHRZEUG = '2' and KENNZ_ZUS_FAHT <> 'X'")(0)("FAHRT").ToString

                'Die alten Einträge löschen
                ct.Bemerkungen.DefaultView.RowFilter = "FAHRT <> '" & Fahrt & "'"

                ct.Bemerkungen = ct.Bemerkungen.DefaultView.ToTable


            End If

            If txtRueckBemerkung.Text.Length > 0 Then

                Texte = RowsToInsert(txtRueckBemerkung.Text)

                For Each Text As String In Texte
                    NewRow = ct.Bemerkungen.NewRow
                    NewRow("FAHRT") = Fahrt
                    NewRow("TEXT_ID") = "0018"
                    NewRow("BEMERKUNG") = Text
                    ct.Bemerkungen.Rows.Add(NewRow)
                    ct.Bemerkungen.AcceptChanges()
                Next
            End If

            'Aktuelle Zusatzfahrt
            If ct.ZuActiveFahrt.Length > 0 Then
                If ct.Fahrten.Select("FAHRT = '" & ct.ZuActiveFahrt & "'").Length > 0 Then

                    Fahrt = ct.ZuActiveFahrt

                    'Die alten Einträge löschen
                    ct.Bemerkungen.DefaultView.RowFilter = "FAHRT <> '" & Fahrt & "'"

                    ct.Bemerkungen = ct.Bemerkungen.DefaultView.ToTable

                End If

                If txtBemerkungZusatz.Text.Length > 0 Then

                    Texte = RowsToInsert(txtBemerkungZusatz.Text)

                    For Each Text As String In Texte
                        NewRow = ct.Bemerkungen.NewRow
                        NewRow("FAHRT") = Fahrt
                        NewRow("TEXT_ID") = "0018"
                        NewRow("BEMERKUNG") = Text
                        ct.Bemerkungen.Rows.Add(NewRow)
                        ct.Bemerkungen.AcceptChanges()
                    Next

                End If
            End If

            'Aktuelle Rückholung(Zusatz)
            If ct.ZuRueckActiveFahrt.Length > 0 Then
                If ct.Fahrten.Select("FAHRT = '" & ct.ZuRueckActiveFahrt & "'").Length > 0 Then

                    Fahrt = ct.ZuRueckActiveFahrt

                    'Die alten Einträge löschen
                    ct.Bemerkungen.DefaultView.RowFilter = "FAHRT <> '" & Fahrt & "'"

                    ct.Bemerkungen = ct.Bemerkungen.DefaultView.ToTable


                End If

                If txtRueckBemZusatz.Text.Length > 0 Then

                    Texte = RowsToInsert(txtRueckBemZusatz.Text)

                    For Each Text As String In Texte
                        NewRow = ct.Bemerkungen.NewRow
                        NewRow("FAHRT") = Fahrt
                        NewRow("TEXT_ID") = "0018"
                        NewRow("BEMERKUNG") = Text
                        ct.Bemerkungen.Rows.Add(NewRow)
                        ct.Bemerkungen.AcceptChanges()
                    Next
                End If
            End If


        End Sub

        Private Function RowsToInsert(ByVal Text As String) As ArrayList


            Dim i As Integer = Text.Length
            Dim Texte As New ArrayList


            Do Until i < Text.Length

                Texte.Add(Left(Text, 40))

                Text = Mid(Text, 41)

                i -= 40
            Loop

            If Text.Length > 0 Then
                Texte.Add(Left(Text, Text.Length))
            End If

            Return Texte
        End Function

        Protected Sub lbtnBackToStart_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lbtnBackToStart.Click
            Response.Redirect("Change02s.aspx?AppID=" & Session("AppID").ToString, False)
        End Sub

        Private Sub LinkButton2_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LinkButton2.Click
            VersandTabPanel1.Visible = False
            VersandTabPanel2.Visible = True
            VersandTabPanel3.Visible = False
            VersandTabPanel4.Visible = False
            LinkButton1.CssClass = "VersandButtonStammReady"
            LinkButton2.CssClass = "LogistikButtonTour"
            LinkButton3.CssClass = "LogistikButtonDL_Disabled"
            LinkButton4.CssClass = "VersandButtonOverviewEnabled"

            LinkButton2.Enabled = False
            LinkButton3.Enabled = False
            LinkButton4.Enabled = False

            lblSteps.Text = "Schritt 2 von 4"
            Panel1.CssClass = "StepActive"
            Panel2.CssClass = "StepActive"
            Panel3.CssClass = "Steps"
            Panel4.CssClass = "Steps"

            CloseDlPanels()
            CloseFahrtenPanels()

        End Sub

        Private Sub LinkButton3_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LinkButton3.Click
            VersandTabPanel1.Visible = False
            VersandTabPanel2.Visible = False
            VersandTabPanel3.Visible = True
            VersandTabPanel4.Visible = False
            LinkButton1.CssClass = "VersandButtonStammReady"
            LinkButton2.CssClass = "LogistikButtonTourReady"
            LinkButton3.CssClass = "LogistikButtonDL"
            LinkButton4.CssClass = "VersandButtonOverviewEnabled"

            LinkButton3.Enabled = False
            LinkButton4.Enabled = False

            lblSteps.Text = "Schritt 3 von 4"
            Panel1.CssClass = "StepActive"
            Panel2.CssClass = "StepActive"
            Panel3.CssClass = "StepActive"
            Panel4.CssClass = "Steps"
            Dim ct As Transfer = CType(Session("Transfer"), Transfer)
            FillGridProtokolle(ct.ProtokollArten)
            FillGridProtokolle2(ct.ProtokollArten)

            CloseDlPanels()
            CloseFahrtenPanels()

        End Sub

        Protected Sub ibtAbholsuche_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtAbholsuche.Click

            trAbholadresse.Visible = True


            txtAbFirma.BorderColor = Drawing.Color.Empty
            txtAbPLZ.BorderColor = Drawing.Color.Empty
            txtAbOrt.BorderColor = Drawing.Color.Empty

            If Not (txtAbFirma.Text.Length & txtAbPLZ.Text.Length & txtAbOrt.Text.Length) > 0 Then
                ddlAbholadresse.Visible = False
                lblErrAbholSuche.Visible = True
                lblErrAbholSuche.Text = "Bitte geben Sie mindestens 1 Suchkriterium an."

                txtAbFirma.BorderColor = Drawing.Color.Red
                txtAbPLZ.BorderColor = Drawing.Color.Red
                txtAbOrt.BorderColor = Drawing.Color.Red

                Exit Sub
            End If

            lblErrAbholSuche.Visible = False

            Dim ct As Transfer = CType(Session("Transfer"), Transfer)



            ct.FillAdressen(m_User, Me.Page, "Abholadresse", txtAbFirma.Text, txtAbPLZ.Text, txtAbOrt.Text, getKundennummer())

            If ct.Abholadresse.Rows.Count > 0 Then

                ibtAbholsuche.Visible = False
                ibtAbholReset.Visible = True
                ddlAbholadresse.Visible = True

                Dim dv As DataView = ct.Abholadresse.DefaultView
                Dim i As Integer = 0



                With ddlAbholadresse


                    .Items.Add(New ListItem("Bitte auswählen", "0"))

                    Do While i < dv.Count

                        .Items.Add(New ListItem(dv.Item(i)("NAME1") & ", " & dv.Item(i)("ORT01"), dv.Item(i)("KUNNR")))

                        i = i + 1
                    Loop


                End With
            Else
                ddlAbholadresse.Visible = False
                lblErrAbholSuche.Visible = True
                lblErrAbholSuche.Text = "Es wurden keine Adressen gefunden."
            End If

            Session("Transfer") = ct



        End Sub

        Protected Sub ddlAbholadresse_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ddlAbholadresse.SelectedIndexChanged
            Dim ct As Transfer = CType(Session("Transfer"), Transfer)



            Dim dv As DataView = ct.Abholadresse.DefaultView


            If ddlAbholadresse.SelectedItem.Value = "0" Then
                Me.txtAbAnsprechpartner.Text = ""
                Me.txtAbFirma.Text = ""
                Me.txtAbOrt.Text = ""
                Me.txtAbPLZ.Text = ""
                ddlAbLand.SelectedValue = "DE"
                Me.txtAbStrasse.Text = ""
                Me.txtAbTelefon.Text = ""
            Else
                dv.RowFilter = "KUNNR = '" & ddlAbholadresse.SelectedItem.Value & "'"
                Me.txtAbAnsprechpartner.Text = dv.Item(0)("NAME2")
                Me.txtAbFirma.Text = dv.Item(0)("NAME1")
                Me.txtAbOrt.Text = dv.Item(0)("ORT01")
                Me.txtAbPLZ.Text = dv.Item(0)("PSTLZ")
                Me.txtAbStrasse.Text = dv.Item(0)("STRAS")
                Me.txtAbTelefon.Text = dv.Item(0)("TELNR")


                Dim LItem As New ListItem

                LItem = ddlAbLand.Items.FindByValue(dv.Item(0)("LAND1"))

                If Not LItem Is Nothing Then
                    ddlAbLand.SelectedValue = dv.Item(0)("LAND1")
                End If

            End If

            ct = Nothing

        End Sub

        Protected Sub ibtAbholReset_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtAbholReset.Click
            ddlAbholadresse.Items.Clear()
            trAbholadresse.Visible = False
            ibtAbholReset.Visible = False
            ibtAbholsuche.Visible = True

            Me.txtAbAnsprechpartner.Text = ""
            Me.txtAbFirma.Text = ""
            Me.txtAbOrt.Text = ""
            Me.txtAbPLZ.Text = ""
            Me.txtAbStrasse.Text = ""
            Me.txtAbTelefon.Text = ""

        End Sub

        Protected Sub ibtZielsuche_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtZielSuche.Click

            trZielAdresse.Visible = True


            txtZielFirma.BorderColor = Drawing.Color.Empty
            txtZielPLZ.BorderColor = Drawing.Color.Empty
            txtZielOrt.BorderColor = Drawing.Color.Empty

            If Not (txtZielFirma.Text.Length & txtZielPLZ.Text.Length & txtZielOrt.Text.Length) > 0 Then
                ddlZieladresse.Visible = False
                lblErrZielSuche.Visible = True
                lblErrZielSuche.Text = "Bitte geben Sie mindestens 1 Suchkriterium an."

                txtZielFirma.BorderColor = Drawing.Color.Red
                txtZielPLZ.BorderColor = Drawing.Color.Red
                txtZielOrt.BorderColor = Drawing.Color.Red

                Exit Sub
            End If

            lblErrZielSuche.Visible = False

            Dim ct As Transfer = CType(Session("Transfer"), Transfer)


            ct.FillAdressen(m_User, Me.Page, "Auslieferung", txtZielFirma.Text, txtZielPLZ.Text, txtZielOrt.Text, getKundennummer())

            If ct.Zieladresse.Rows.Count > 0 Then

                ibtZielSuche.Visible = False
                ibtZielReset.Visible = True
                ddlZieladresse.Visible = True

                Dim dv As DataView = ct.Zieladresse.DefaultView
                Dim i As Integer = 0



                With ddlZieladresse


                    .Items.Add(New ListItem("Bitte auswählen", "0"))

                    Do While i < dv.Count

                        .Items.Add(New ListItem(dv.Item(i)("NAME1") & ", " & dv.Item(i)("ORT01"), dv.Item(i)("KUNNR")))

                        i = i + 1
                    Loop


                End With
            Else
                ddlZieladresse.Visible = False
                lblErrZielSuche.Visible = True
                lblErrZielSuche.Text = "Es wurden keine Adressen gefunden."
            End If

            Session("Transfer") = ct



        End Sub

        Protected Sub ddlZieladresse_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ddlZieladresse.SelectedIndexChanged
            Dim ct As Transfer = CType(Session("Transfer"), Transfer)



            Dim dv As DataView = ct.Zieladresse.DefaultView


            If ddlZieladresse.SelectedItem.Value = "0" Then
                Me.txtZielAnsprechpartner.Text = ""
                Me.txtZielFirma.Text = ""
                Me.txtZielOrt.Text = ""
                Me.txtZielPLZ.Text = ""
                Me.ddlZielland.SelectedValue = "DE"
                Me.txtZielStrasse.Text = ""
                Me.txtZielTelefon.Text = ""
            Else

                dv.RowFilter = "KUNNR = '" & ddlZieladresse.SelectedItem.Value & "'"

                Me.txtZielAnsprechpartner.Text = dv.Item(0)("NAME2")
                Me.txtZielFirma.Text = dv.Item(0)("NAME1")
                Me.txtZielOrt.Text = dv.Item(0)("ORT01")
                Me.txtZielPLZ.Text = dv.Item(0)("PSTLZ")
                Me.txtZielStrasse.Text = dv.Item(0)("STRAS")
                Me.txtZielTelefon.Text = dv.Item(0)("TELNR")

                Dim LItem As New ListItem

                LItem = ddlZielland.Items.FindByValue(dv.Item(0)("LAND1"))

                If Not LItem Is Nothing Then
                    ddlZielland.SelectedValue = dv.Item(0)("LAND1")
                End If



            End If


            ct = Nothing



        End Sub

        Protected Sub ibtZielReset_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtZielReset.Click
            ddlZieladresse.Items.Clear()
            trZielAdresse.Visible = False
            ibtZielReset.Visible = False
            ibtZielSuche.Visible = True

            Me.txtZielAnsprechpartner.Text = ""
            Me.txtZielFirma.Text = ""
            Me.txtZielOrt.Text = ""
            Me.txtZielPLZ.Text = ""
            Me.txtZielStrasse.Text = ""
            Me.txtZielTelefon.Text = ""
        End Sub

        'Rückführung
        Protected Sub ibtZielsucheRu_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtZielSucheRu.Click

            trZielAdresseRu.Visible = True


            txtRuZielFirma.BorderColor = Drawing.Color.Empty
            txtRuZielPlz.BorderColor = Drawing.Color.Empty
            txtRuZielOrt.BorderColor = Drawing.Color.Empty

            If Not (txtRuZielFirma.Text.Length & txtRuZielPlz.Text.Length & txtRuZielOrt.Text.Length) > 0 Then
                ddlZieladresseRu.Visible = False
                lblErrZielSucheRu.Visible = True
                lblErrZielSucheRu.Text = "Bitte geben Sie mindestens 1 Suchkriterium an."

                txtRuZielFirma.BorderColor = Drawing.Color.Red
                txtRuZielPlz.BorderColor = Drawing.Color.Red
                txtRuZielOrt.BorderColor = Drawing.Color.Red

                Exit Sub
            End If

            lblErrZielSucheRu.Visible = False

            Dim ct As Transfer = CType(Session("Transfer"), Transfer)


            ct.FillAdressen(m_User, Me.Page, "Rückholung", txtRuZielFirma.Text, txtRuZielPlz.Text, txtRuZielOrt.Text, getKundennummer())

            If ct.ZieladresseRueck.Rows.Count > 0 Then

                ibtZielSucheRu.Visible = False
                ibtZielResetRu.Visible = True
                ddlZieladresseRu.Visible = True

                Dim dv As DataView = ct.ZieladresseRueck.DefaultView
                Dim i As Integer = 0



                With ddlZieladresseRu


                    .Items.Add(New ListItem("Bitte auswählen", "0"))

                    Do While i < dv.Count

                        .Items.Add(New ListItem(dv.Item(i)("NAME1") & ", " & dv.Item(i)("ORT01"), dv.Item(i)("KUNNR")))

                        i = i + 1
                    Loop


                End With
            Else
                ddlZieladresseRu.Visible = False
                lblErrZielSucheRu.Visible = True
                lblErrZielSucheRu.Text = "Es wurden keine Adressen gefunden."
            End If

            Session("Transfer") = ct



        End Sub

        Protected Sub ddlZieladresseRu_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ddlZieladresseRu.SelectedIndexChanged
            Dim ct As Transfer = CType(Session("Transfer"), Transfer)



            Dim dv As DataView = ct.ZieladresseRueck.DefaultView


            If ddlZieladresseRu.SelectedItem.Value = "0" Then
                Me.txtRuZielAnsprechpartner.Text = ""
                Me.txtRuZielFirma.Text = ""
                Me.txtRuZielOrt.Text = ""
                Me.txtRuZielPlz.Text = ""
                Me.ddlRuZielLand.SelectedValue = "DE"
                Me.txtRuZielStrasse.Text = ""
                Me.txtRuZielTelefon.Text = ""
            Else

                dv.RowFilter = "KUNNR = '" & ddlZieladresseRu.SelectedItem.Value & "'"

                Me.txtRuZielAnsprechpartner.Text = dv.Item(0)("NAME2")
                Me.txtRuZielFirma.Text = dv.Item(0)("NAME1")
                Me.txtRuZielOrt.Text = dv.Item(0)("ORT01")
                Me.txtRuZielPlz.Text = dv.Item(0)("PSTLZ")
                Me.txtRuZielStrasse.Text = dv.Item(0)("STRAS")
                Me.txtRuZielTelefon.Text = dv.Item(0)("TELNR")

                Dim LItem As New ListItem

                LItem = ddlRuZielLand.Items.FindByValue(dv.Item(0)("LAND1"))

                If Not LItem Is Nothing Then
                    ddlRuZielLand.SelectedValue = dv.Item(0)("LAND1")
                End If



            End If


            ct = Nothing



        End Sub

        Protected Sub ibtZielResetRu_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtZielResetRu.Click
            ZielResetRu()

        End Sub

        Private Sub ZielResetRu()
            ddlZieladresseRu.Items.Clear()
            trZielAdresseRu.Visible = False
            ibtZielResetRu.Visible = False
            ibtZielSucheRu.Visible = True

            Me.txtRuZielAnsprechpartner.Text = ""
            Me.txtRuZielFirma.Text = ""
            Me.txtRuZielOrt.Text = ""
            Me.txtRuZielPlz.Text = ""
            Me.txtRuZielStrasse.Text = ""
            Me.txtRuZielTelefon.Text = ""

        End Sub

        Private Function CheckDates() As Boolean

            Dim booError As Boolean = False
            Dim booDateSet As Boolean = False 'Wurde irgendwo ein Datum gesetzt?
            Dim SelRow As DataRow()

            Dim ct As Transfer = CType(Session("Transfer"), Transfer)

            If (txtAbDatum.Text & txtZielDatum.Text).Length > 0 Then
                booDateSet = True
            End If

            If ct.Fahrzeuge.Select("FAHRZEUG = '2'").Length > 0 Then
                If txtRuZielDatum.Text.Length > 0 Then
                    booDateSet = True
                End If
            End If


            SelRow = ct.Fahrten.Select("KENNZ_ZUS_FAHT = 'X'")

            If SelRow.Length > 0 Then

                For i = 0 To (SelRow.Length - 1)
                    If Not SelRow(i)("VDATU") Is System.DBNull.Value Then
                        booDateSet = True
                        Exit For
                    End If
                Next
            End If


            'Prüfen, ob alle Datumsfelder gesetzt wurden
            If booDateSet = True Then

                If txtAbDatum.Text.Length = 0 Then booError = True
                If txtZielDatum.Text.Length = 0 Then booError = True

                If ct.Fahrzeuge.Select("FAHRZEUG = '2'").Length > 0 Then
                    If txtRuZielDatum.Text.Length = 0 Then booError = True
                End If

                If SelRow.Length > 0 Then

                    For i = 0 To (SelRow.Length - 1)
                        If SelRow(i)("VDATU") Is System.DBNull.Value Then
                            booError = True
                            Exit For
                        End If
                    Next
                End If


            End If

            Return booError


        End Function

        Private Function CheckDates2() As Boolean

            Dim booError As Boolean = False
            Dim SelRow As DataRow()

            Dim ct As Transfer = CType(Session("Transfer"), Transfer)

            If Not String.IsNullOrEmpty(txtAbDatum.Text) OrElse Not String.IsNullOrEmpty(txtZielDatum.Text) Then

                Dim abDatum, zielDatum As DateTime
                If Not DateTime.TryParse(txtAbDatum.Text, abDatum) OrElse Not DateTime.TryParse(txtZielDatum.Text, zielDatum) OrElse abDatum > zielDatum Then
                    booError = True
                End If
            End If

            SelRow = ct.Fahrten.Select("KENNZ_ZUS_FAHT = 'X'")

            If SelRow.Length > 0 Then

                For i = 0 To (SelRow.Length - 1)
                    If Not SelRow(i)("VDATU") Is System.DBNull.Value Then
                        If Not (CDate(txtAbDatum.Text) <= CDate(SelRow(i)("VDATU").ToString)) Then
                            booError = True
                        End If
                    End If
                Next
            End If

            Return booError


        End Function

        Private Function CheckDates3() As Boolean

            Dim booError As Boolean = False
            Dim tempDatum As DateTime

            If Not String.IsNullOrEmpty(txtAbDatum.Text) AndAlso IsDate(txtAbDatum.Text) AndAlso DateTime.TryParse(txtAbDatum.Text, tempDatum) Then
                If tempDatum.Date < Now.Date Then
                    booError = True
                End If
            End If
            If Not String.IsNullOrEmpty(txtZuDatum.Text) AndAlso IsDate(txtZuDatum.Text) AndAlso DateTime.TryParse(txtZuDatum.Text, tempDatum) Then
                If tempDatum.Date < Now.Date Then
                    booError = True
                End If
            End If
            If Not String.IsNullOrEmpty(txtZielDatum.Text) AndAlso IsDate(txtZielDatum.Text) AndAlso DateTime.TryParse(txtZielDatum.Text, tempDatum) Then
                If tempDatum.Date < Now.Date Then
                    booError = True
                End If
            End If
            If Not String.IsNullOrEmpty(txtRueckZuDatum.Text) AndAlso IsDate(txtRueckZuDatum.Text) AndAlso DateTime.TryParse(txtRueckZuDatum.Text, tempDatum) Then
                If tempDatum.Date < Now.Date Then
                    booError = True
                End If
            End If
            If Not String.IsNullOrEmpty(txtRuZielDatum.Text) AndAlso IsDate(txtRuZielDatum.Text) AndAlso DateTime.TryParse(txtRuZielDatum.Text, tempDatum) Then
                If tempDatum.Date < Now.Date Then
                    booError = True
                End If
            End If

            Return booError

        End Function

        Private Sub ResetOptions()
            divOptions.Style("margin-top") = "10%"
            divBackDisabled.Style("height") = "75%"
        End Sub

        Private Sub AddMaxLength()

            Dim lengthFunction As String = "function isMaxLength(txtBox) {"
            lengthFunction += " if(txtBox) { "
            lengthFunction += "     return ( txtBox.value.length <=" & LENGTH_TEXT & ");"
            lengthFunction += " }"
            lengthFunction += "}"

            Me.txtAbBemerkung.Attributes.Add("onkeypress", "return isMaxLength(this);")
            Me.txtBemerkungZusatz.Attributes.Add("onkeypress", "return isMaxLength(this);")
            Me.txtRueckBemZusatz.Attributes.Add("onkeypress", "return isMaxLength(this);")
            Me.txtRueckBemerkung.Attributes.Add("onkeypress", "return isMaxLength(this);")



            ClientScript.RegisterClientScriptBlock(Me.[GetType](), "txtAbBemerkung", lengthFunction, True)
            ClientScript.RegisterClientScriptBlock(Me.[GetType](), "txtBemerkungZusatz", lengthFunction, True)
            ClientScript.RegisterClientScriptBlock(Me.[GetType](), "txtRueckBemZusatz", lengthFunction, True)
            ClientScript.RegisterClientScriptBlock(Me.[GetType](), "txtRueckBemerkung", lengthFunction, True)



        End Sub

        Private Function CheckMaxLength() As Boolean

            lblErrorDienst.Visible = False

            Dim booError As Boolean = False

            If txtAbBemerkung.Text.Length > 200 Then
                booError = True
            ElseIf txtBemerkungZusatz.Text.Length > 200 Then
                booError = True
            ElseIf txtRueckBemZusatz.Text.Length > 200 Then
                booError = True
            ElseIf txtRueckBemerkung.Text.Length > 200 Then
                booError = True
            End If


            Return booError

        End Function

        Protected Sub ibtnSuche1_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtnSuche1.Click
            lblErrorStamm.Visible = False
            lblErrorStamm.Text = ""
            ResetErrorFrame(txtFahrgestellnummer)
            ResetErrorFrame(txtKennzeichen1)
            ResetErrorFrame(txtReferenznummer)

            If txtFahrgestellnummer.Text.Trim.Length + txtKennzeichen1.Text.Trim.Length + txtReferenznummer.Text.Trim.Length > 0 Then

                txtFahrgestellnummer.Text = txtFahrgestellnummer.Text.Replace(" ", "")
                txtKennzeichen1.Text = txtKennzeichen1.Text.Replace(" ", "")
                txtReferenznummer.Text = txtReferenznummer.Text.Replace(" ", "")

                Dim ct As Transfer
                If Session("Transfer") Is Nothing Then
                    ct = New Transfer(m_User, m_App, "", "", "")
                Else
                    ct = CType(Session("Transfer"), Transfer)
                End If
                ct.SuchFahrzeugestellnummer = txtFahrgestellnummer.Text.Trim
                ct.Suchkennzeichen = txtKennzeichen1.Text.Trim
                ct.SuchReferenz = txtReferenznummer.Text.Trim
                ct.FillFahrzeuge(m_User, Me.Page, getKundennummer())
                If ct.Status = 0 Then
                    If ct.SuchFahrzeuge.Rows.Count > 0 Then
                        txtFahrgestellnummer.Text = ct.SuchFahrzeuge.Rows(0)("CHASSIS_NUM").ToString
                        txtKennzeichen1.Text = ct.SuchFahrzeuge.Rows(0)("LICENSE_NUM").ToString
                        txtReferenznummer.Text = ct.SuchFahrzeuge.Rows(0)("LIZNR").ToString

                        Dim iGewicht As Integer

                        If IsNumeric(ct.SuchFahrzeuge.Rows(0)("ZZZULGESGEW").ToString) Then
                            iGewicht = CInt(ct.SuchFahrzeuge.Rows(0)("ZZZULGESGEW"))
                            Select Case (iGewicht <= 7500)
                                Case True
                                    rblFahrzeugklasse.SelectedValue = "PKW"
                                Case Else
                                    rblFahrzeugklasse.SelectedValue = "LKW"
                            End Select
                        End If
                        If ct.SuchFahrzeuge.Rows(0)("ZZHERST_TEXT").Length + ct.SuchFahrzeuge.Rows(0)("ZZHANDELSNAME").ToString.Length > 25 Then
                            txtTyp.Text = ct.SuchFahrzeuge.Rows(0)("ZZHANDELSNAME").ToString
                        Else
                            txtTyp.Text = ct.SuchFahrzeuge.Rows(0)("ZZHERST_TEXT").ToString + " " + ct.SuchFahrzeuge.Rows(0)("ZZHANDELSNAME").ToString
                        End If

                    End If
                Else
                    lblErrorStamm.Visible = True
                    lblErrorStamm.Text = ct.Message
                End If

            Else
                If txtFahrgestellnummer.Text.Length = 0 Then
                    SetErrorFrame(txtFahrgestellnummer)
                End If
                If txtKennzeichen1.Text.Length = 0 Then
                    SetErrorFrame(txtKennzeichen1)
                End If
                If txtReferenznummer.Text.Length = 0 Then
                    SetErrorFrame(txtReferenznummer)
                End If
                lblErrorStamm.Visible = True
            End If
        End Sub

        Protected Sub ibtnSuche2_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtnSuche2.Click
            lblErrorStamm.Visible = False
            lblErrorStamm.Text = ""
            ResetErrorFrame(txtRuFahrgestellnummer)
            ResetErrorFrame(txtRuKennzeichen1)
            ResetErrorFrame(txtRuReferenznummer)

            If txtRuFahrgestellnummer.Text.Trim.Length + txtRuKennzeichen1.Text.Trim.Length + txtRuReferenznummer.Text.Trim.Length > 0 Then

                txtRuFahrgestellnummer.Text = txtRuFahrgestellnummer.Text.Replace(" ", "")
                txtRuKennzeichen1.Text = txtRuKennzeichen1.Text.Replace(" ", "")
                txtRuReferenznummer.Text = txtRuReferenznummer.Text.Replace(" ", "")


                Dim ct As Transfer
                If Session("Transfer") Is Nothing Then
                    ct = New Transfer(m_User, m_App, "", "", "")
                Else
                    ct = CType(Session("Transfer"), Transfer)
                End If
                ct.SuchFahrzeugestellnummer = txtRuFahrgestellnummer.Text.Trim
                ct.Suchkennzeichen = txtRuKennzeichen1.Text.Trim
                ct.SuchReferenz = txtRuReferenznummer.Text.Trim
                ct.FillFahrzeuge(m_User, Me.Page, getKundennummer())
                If ct.Status = 0 Then
                    If ct.SuchFahrzeuge.Rows.Count > 0 Then
                        txtRuFahrgestellnummer.Text = ct.SuchFahrzeuge.Rows(0)("CHASSIS_NUM").ToString
                        txtRuKennzeichen1.Text = ct.SuchFahrzeuge.Rows(0)("LICENSE_NUM").ToString
                        txtRuReferenznummer.Text = ct.SuchFahrzeuge.Rows(0)("LIZNR").ToString

                        Dim iGewicht As Integer

                        If IsNumeric(ct.SuchFahrzeuge.Rows(0)("ZZZULGESGEW").ToString) Then
                            iGewicht = CInt(ct.SuchFahrzeuge.Rows(0)("ZZZULGESGEW"))
                            Select Case (iGewicht <= 7500)
                                Case True
                                    rblRuFahrzeugklasse.SelectedValue = "PKW"
                                Case Else
                                    rblRuFahrzeugklasse.SelectedValue = "LKW"
                            End Select
                        End If
                        If ct.SuchFahrzeuge.Rows(0)("ZZHERST_TEXT").Length + ct.SuchFahrzeuge.Rows(0)("ZZHANDELSNAME").ToString.Length > 25 Then
                            txtRuTyp.Text = ct.SuchFahrzeuge.Rows(0)("ZZHANDELSNAME").ToString
                        Else
                            txtRuTyp.Text = ct.SuchFahrzeuge.Rows(0)("ZZHERST_TEXT").ToString + " " + ct.SuchFahrzeuge.Rows(0)("ZZHANDELSNAME").ToString
                        End If
                    End If
                Else
                    lblErrorStamm.Visible = True
                    lblErrorStamm.Text = ct.Message
                End If

            Else
                If txtRuFahrgestellnummer.Text.Length = 0 Then
                    SetErrorFrame(txtRuFahrgestellnummer)
                End If
                If txtRuKennzeichen1.Text.Length = 0 Then
                    SetErrorFrame(txtRuKennzeichen1)
                End If
                If txtRuReferenznummer.Text.Length = 0 Then
                    SetErrorFrame(txtRuReferenznummer)
                End If
                lblErrorStamm.Visible = True
            End If
        End Sub

        Protected Sub ibtnCreatePDF_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtnCreatePDF.Click
            Dim pdfDataSet As New DataSet()
            Dim ct As Transfer
            If Session("Transfer") Is Nothing Then
                ct = New Transfer(m_User, m_App, "", "", "")
            Else
                ct = CType(Session("Transfer"), Transfer)
            End If

            Dim headTable As New DataTable
            headTable.TableName = "Kopf"
            headTable.Columns.Add("Datum", GetType(System.String))
            headTable.Columns.Add("User", GetType(System.String))
            Dim headRow As DataRow = headTable.NewRow
            headRow("Datum") = Now.ToShortDateString
            headRow("User") = m_User.UserName
            headTable.Rows.Add(headRow)

            pdfDataSet.Tables.Add(CreateTableStammdaten(ct))
            pdfDataSet.Tables.Add(CreateTableFahrten(ct))
            pdfDataSet.Tables.Add(CreateTableDienstleistungen(ct))
            pdfDataSet.Tables.Add(CreateTableProtokolle(ct))


            Dim AuftragsNrTable As New DataTable
            AuftragsNrTable.TableName = "Auftragsnummer"
            AuftragsNrTable.Columns.Add("ID", GetType(System.String))
            AuftragsNrTable.Columns.Add("Auftragsnummer", GetType(System.String))
            Dim AuftragsNrRow As DataRow
            Dim ilfdnr As Int32 = 1

            If Debugger.IsAttached Then
                AuftragsNrRow = AuftragsNrTable.NewRow
                AuftragsNrRow("ID") = "1"
                AuftragsNrRow("Auftragsnummer") = "1231598498"
                AuftragsNrTable.Rows.Add(AuftragsNrRow)
            Else
                For Each RowData As DataRow In ct.ReturnTable.Rows
                    AuftragsNrRow = AuftragsNrTable.NewRow
                    AuftragsNrRow("ID") = ilfdnr
                    AuftragsNrRow("Auftragsnummer") = RowData("VBELN")
                    ilfdnr += 1
                    AuftragsNrTable.Rows.Add(AuftragsNrRow)
                Next
            End If
            pdfDataSet.Tables.Add(AuftragsNrTable)
            Dim imageHt As New Hashtable()
            imageHt.Add("Logo", m_User.Customer.LogoImage)

            Dim docFactory As New DocumentGeneration.WordDocumentFactory(Nothing, imageHt)
            docFactory.CreateDocumentDataset("UebersichtUeberfuehrung", Me.Page, "Components\ComCommon\Logistik\Dokumente\UebersichtUeberfuehrung.doc", headTable, pdfDataSet)
        End Sub

        Private Function CreateTableStammdaten(ByVal ct As Transfer) As DataTable

            Dim tmptable As New DataTable
            tmptable.TableName = "Stammdaten"
            tmptable.Columns.Add("FIN1", GetType(System.String))
            tmptable.Columns.Add("Kennz1", GetType(System.String))
            tmptable.Columns.Add("Typ1", GetType(System.String))
            tmptable.Columns.Add("RefNr1", GetType(System.String))
            tmptable.Columns.Add("FzgWert1", GetType(System.String))
            tmptable.Columns.Add("FzgZulBereit1", GetType(System.String))
            tmptable.Columns.Add("FzgZulDAD1", GetType(System.String))
            tmptable.Columns.Add("Reifen1", GetType(System.String))
            tmptable.Columns.Add("FzgKlasse1", GetType(System.String))
            tmptable.Columns.Add("FIN2", GetType(System.String))
            tmptable.Columns.Add("Kennz2", GetType(System.String))
            tmptable.Columns.Add("Typ2", GetType(System.String))
            tmptable.Columns.Add("RefNr2", GetType(System.String))
            tmptable.Columns.Add("FzgWert2", GetType(System.String))
            tmptable.Columns.Add("FzgZulBereit2", GetType(System.String))
            tmptable.Columns.Add("FzgZulDAD2", GetType(System.String))
            tmptable.Columns.Add("Reifen2", GetType(System.String))
            tmptable.Columns.Add("FzgKlasse2", GetType(System.String))
            tmptable.Columns.Add("NameRG", GetType(System.String))
            tmptable.Columns.Add("StrasseRG", GetType(System.String))
            tmptable.Columns.Add("PLZ_OrtRG", GetType(System.String))
            tmptable.Columns.Add("APartnerRG", GetType(System.String))
            tmptable.Columns.Add("TelefonRG", GetType(System.String))
            tmptable.Columns.Add("NameRE", GetType(System.String))
            tmptable.Columns.Add("StrasseRE", GetType(System.String))
            tmptable.Columns.Add("PLZ_OrtRE", GetType(System.String))
            tmptable.Columns.Add("APartnerRE", GetType(System.String))
            tmptable.Columns.Add("TelefonRE", GetType(System.String))


            Dim NewRowStamm As DataRow = tmptable.NewRow

            NewRowStamm("FIN1") = txtFahrgestellnummer.Text
            NewRowStamm("Kennz1") = txtKennzeichen1.Text
            NewRowStamm("Typ1") = txtTyp.Text
            NewRowStamm("RefNr1") = txtReferenznummer.Text
            NewRowStamm("FzgWert1") = drpFahrzeugwert.SelectedItem.Text
            NewRowStamm("FzgZulBereit1") = rblZugelassen.SelectedValue
            NewRowStamm("FzgZulDAD1") = rblBeauftragt.SelectedValue
            NewRowStamm("Reifen1") = rblBereifung.SelectedValue
            NewRowStamm("FzgKlasse1") = rblFahrzeugklasse.SelectedItem.Text
            If txtRuFahrgestellnummer.Text.Length > 0 Then
                NewRowStamm("FIN2") = txtRuFahrgestellnummer.Text
                NewRowStamm("Kennz2") = txtRuKennzeichen1.Text
                NewRowStamm("Typ2") = txtRuTyp.Text
                NewRowStamm("RefNr2") = txtRuReferenznummer.Text
                NewRowStamm("FzgWert2") = drpRuFahrzeugwert.SelectedItem.Text
                NewRowStamm("FzgZulBereit2") = rblRuZugelassen.SelectedValue
                NewRowStamm("FzgZulDAD2") = rblRuBeauftragt.SelectedValue
                NewRowStamm("Reifen2") = rblRuBereifung.SelectedValue
                NewRowStamm("FzgKlasse2") = rblRuFahrzeugklasse.SelectedItem.Text
            Else
                NewRowStamm("FIN2") = ""
                NewRowStamm("Kennz2") = ""
                NewRowStamm("Typ2") = ""
                NewRowStamm("RefNr2") = ""
                NewRowStamm("FzgWert2") = ""
                NewRowStamm("FzgZulBereit2") = ""
                NewRowStamm("FzgZulDAD2") = ""
                NewRowStamm("Reifen2") = ""
                NewRowStamm("FzgKlasse2") = ""
            End If


            Dim RowData() As DataRow = ct.Partner.Select("KUNNR='" & ddlPartnerRG.SelectedItem.Value & "'")

            If RowData.Length > 0 Then
                NewRowStamm("NameRG") = RowData(0)("NAME1")
                NewRowStamm("StrasseRG") = RowData(0)("STREET") & " " & RowData(0)("HOUSE_NUM1")
                NewRowStamm("PLZ_OrtRG") = RowData(0)("POST_CODE1") & " " & RowData(0)("CITY1")
                NewRowStamm("APartnerRG") = RowData(0)("NAME2")
                NewRowStamm("TelefonRG") = RowData(0)("TEL_NUMBER")
            End If
            RowData = ct.Partner.Select("KUNNR='" & ddlPartnerRE.SelectedItem.Value & "'")

            If RowData.Length > 0 Then
                NewRowStamm("NameRE") = RowData(0)("NAME1")
                NewRowStamm("StrasseRE") = RowData(0)("STREET") & " " & RowData(0)("HOUSE_NUM1")
                NewRowStamm("PLZ_OrtRE") = RowData(0)("POST_CODE1") & " " & RowData(0)("CITY1")
                NewRowStamm("APartnerRE") = RowData(0)("NAME2")
                NewRowStamm("TelefonRE") = RowData(0)("TEL_NUMBER")
            End If

            tmptable.Rows.Add(NewRowStamm)

            Return tmptable

        End Function

        Private Function CreateTableFahrten(ByVal ct As Transfer) As DataTable



            Dim tmptable As New DataTable
            tmptable.TableName = "Fahrten"
            tmptable.Columns.Add("Adresstyp", GetType(System.String))
            tmptable.Columns.Add("Adresse", GetType(System.String))
            tmptable.Columns.Add("APartner", GetType(System.String))
            tmptable.Columns.Add("Telefon", GetType(System.String))
            tmptable.Columns.Add("Datum", GetType(System.String))
            tmptable.Columns.Add("Uhrzeit", GetType(System.String))
            tmptable.Columns.Add("Fahrzeug", GetType(System.String))
            tmptable.Columns.Add("KM", GetType(System.String))



            'Fahrzeug 1
            Dim NewRowStamm As DataRow
            Dim SelRow As DataRow()
            Dim adressRow As DataRow()
            Dim EntfernungRow As DataRow()
            Dim Adresse As String
            SelRow = ct.Fahrten.Select("FAHRZEUG = '1'")

            Dim Fahrt As String

            For i = 0 To SelRow.Length - 1
                NewRowStamm = tmptable.NewRow
                NewRowStamm("Fahrzeug") = "Fahrzeug 1"
                If i = 0 Then
                    NewRowStamm("Adresstyp") = "Abholadresse"
                End If
                If i > 0 AndAlso SelRow(i)("KENNZ_ZUS_FAHT") <> "X" Then
                    ' NewRowStamm("Adresstyp") = "Zieladresse"
                    NewRowStamm("Adresstyp") = ct.Transporttyp.Select("ID = '" & SelRow(i)("TRANSPORTTYP").ToString & "'")(0)("Text").ToString
                End If
                If SelRow(i)("KENNZ_ZUS_FAHT") = "X" Then
                    NewRowStamm("Adresstyp") = ct.Transporttyp.Select("ID = '" & SelRow(i)("TRANSPORTTYP").ToString & "'")(0)("Text").ToString
                End If
                Fahrt = SelRow(i)("FAHRT")
                adressRow = ct.Adressen.Select("FAHRT = '" & Fahrt & "'")

                If Not ct.Entfernungen Is Nothing Then
                    EntfernungRow = ct.Entfernungen.Select("FAHRT = '" & Fahrt & "'")
                    If EntfernungRow.Length > 0 Then
                        NewRowStamm("KM") = EntfernungRow(0)("KM")
                    End If
                End If


                If adressRow.Length > 0 Then
                    Adresse = adressRow(0)("NAME").ToString & vbCrLf
                    Adresse += adressRow(0)("STREET").ToString & vbCrLf
                    Adresse += adressRow(0)("POSTL_CODE").ToString & " " & adressRow(0)("CITY").ToString & vbCrLf
                    Adresse += adressRow(0)("COUNTRY").ToString & vbCrLf

                    NewRowStamm("APartner") = adressRow(0)("NAME_2").ToString
                    NewRowStamm("Telefon") = adressRow(0)("TELEPHONE").ToString
                    NewRowStamm("Adresse") = Adresse
                End If


                If String.IsNullOrEmpty(SelRow(i)("VDATU").ToString) = False Then
                    NewRowStamm("Datum") = CDate(SelRow(i)("VDATU").ToString).ToShortDateString
                End If

                'SelRow(0)("AT_TIM_VON") & "-" & SelRow(0)("AT_TIM_BIS")
                If String.IsNullOrEmpty(SelRow(i)("AT_TIM_VON").ToString) = False Then
                    NewRowStamm("Uhrzeit") = Left(SelRow(i)("AT_TIM_VON"), 2) & ":" & Right(SelRow(i)("AT_TIM_VON"), 2) & " - " & Left(SelRow(i)("AT_TIM_BIS"), 2) & ":" & Right(SelRow(i)("AT_TIM_BIS"), 2)
                End If
                tmptable.Rows.Add(NewRowStamm)
            Next

            SelRow = ct.Fahrten.Select("FAHRZEUG = '2'")

            For i = 0 To SelRow.Length - 1
                NewRowStamm = tmptable.NewRow
                NewRowStamm("Fahrzeug") = "Fahrzeug 2"
                If i = 0 Then
                    NewRowStamm("Adresstyp") = ct.Transporttyp.Select("ID = '" & SelRow(i)("TRANSPORTTYP").ToString & "'")(0)("Text").ToString
                End If
                If i > 0 AndAlso SelRow(i)("KENNZ_ZUS_FAHT") <> "X" Then
                    'NewRowStamm("Adresstyp") = "Zieladresse"
                    NewRowStamm("Adresstyp") = ct.Transporttyp.Select("ID = '" & SelRow(i)("TRANSPORTTYP").ToString & "'")(0)("Text").ToString
                End If
                If SelRow(i)("KENNZ_ZUS_FAHT") = "X" Then
                    NewRowStamm("Adresstyp") = ct.Transporttyp.Select("ID = '" & SelRow(i)("TRANSPORTTYP").ToString & "'")(0)("Text").ToString
                End If
                Fahrt = SelRow(i)("FAHRT")

                If Not ct.Entfernungen Is Nothing Then
                    EntfernungRow = ct.Entfernungen.Select("FAHRT = '" & Fahrt & "'")
                    If EntfernungRow.Length > 0 Then
                        NewRowStamm("KM") = EntfernungRow(0)("KM")
                    End If
                End If


                adressRow = ct.Adressen.Select("FAHRT = '" & Fahrt & "'")
                If adressRow.Length > 0 Then
                    Adresse = adressRow(0)("NAME").ToString & vbCrLf
                    Adresse += adressRow(0)("STREET").ToString & vbCrLf
                    Adresse += adressRow(0)("POSTL_CODE").ToString & " " & adressRow(0)("CITY").ToString & vbCrLf
                    Adresse += adressRow(0)("COUNTRY").ToString & vbCrLf

                    NewRowStamm("APartner") = adressRow(0)("NAME_2").ToString
                    NewRowStamm("Telefon") = adressRow(0)("TELEPHONE").ToString
                    NewRowStamm("Adresse") = Adresse
                End If
                If String.IsNullOrEmpty(SelRow(i)("VDATU").ToString) = False Then
                    NewRowStamm("Datum") = CDate(SelRow(i)("VDATU").ToString).ToShortDateString
                End If

                If String.IsNullOrEmpty(SelRow(i)("AT_TIM_VON").ToString) = False Then
                    NewRowStamm("Uhrzeit") = Left(SelRow(i)("AT_TIM_VON"), 2) & ":" & Right(SelRow(i)("AT_TIM_VON"), 2) & " - " & Left(SelRow(i)("AT_TIM_BIS"), 2) & ":" & Right(SelRow(i)("AT_TIM_BIS"), 2)
                End If
                tmptable.Rows.Add(NewRowStamm)
            Next



            Return tmptable
        End Function

        Private Function CreateTableDienstleistungen(ByVal ct As Transfer) As DataTable



            Dim tmptable As New DataTable
            tmptable.TableName = "Dienstleistungen"
            tmptable.Columns.Add("Adresstyp", GetType(System.String))
            tmptable.Columns.Add("Dienstleistungen", GetType(System.String))
            tmptable.Columns.Add("Bemerkung", GetType(System.String))


            'Fahrzeug 1
            Dim NewRowStamm As DataRow
            Dim SelRow As DataRow()

            SelRow = ct.Fahrten.Select("FAHRZEUG = '1'")

            Dim Fahrt As String

            For i = 0 To SelRow.Length - 1
                NewRowStamm = tmptable.NewRow
                If i > 0 Then


                    If i > 0 AndAlso SelRow(i)("KENNZ_ZUS_FAHT") <> "X" Then
                        'NewRowStamm("Adresstyp") = "Zieladresse"
                        NewRowStamm("Adresstyp") = ct.Transporttyp.Select("ID = '" & SelRow(i)("TRANSPORTTYP").ToString & "'")(0)("Text").ToString
                    End If
                    If SelRow(i)("KENNZ_ZUS_FAHT") = "X" Then
                        NewRowStamm("Adresstyp") = ct.Transporttyp.Select("ID = '" & SelRow(i)("TRANSPORTTYP").ToString & "'")(0)("Text").ToString
                    End If
                    Fahrt = SelRow(i)("FAHRT")
                    Dim SelDienstRow As DataRow() = ct.Dienstleistungen.Select("FAHRT = '" & Fahrt & "'")

                    For iDienst = 0 To SelDienstRow.Length - 1
                        NewRowStamm("Dienstleistungen") += SelDienstRow(iDienst)("DIENSTL_TEXT").ToString & vbCrLf
                    Next
                    Dim SelBemerkRow As DataRow() = ct.Bemerkungen.Select("FAHRT = '" & Fahrt & "'")

                    For iBemCount = 0 To SelBemerkRow.Length - 1
                        NewRowStamm("Bemerkung") += SelBemerkRow(iBemCount)("BEMERKUNG").ToString & vbCrLf
                    Next

                    tmptable.Rows.Add(NewRowStamm)
                End If
            Next

            SelRow = ct.Fahrten.Select("FAHRZEUG = '2'")

            For i = 0 To SelRow.Length - 1
                NewRowStamm = tmptable.NewRow

                If i = 0 Then
                    NewRowStamm("Adresstyp") = ct.Transporttyp.Select("ID = '" & SelRow(i)("TRANSPORTTYP").ToString & "'")(0)("Text").ToString
                End If
                If i > 0 AndAlso SelRow(i)("KENNZ_ZUS_FAHT") <> "X" Then
                    NewRowStamm("Adresstyp") = ct.Transporttyp.Select("ID = '" & SelRow(i)("TRANSPORTTYP").ToString & "'")(0)("Text").ToString
                    'NewRowStamm("Adresstyp") = "Zieladresse"
                End If
                If SelRow(i)("KENNZ_ZUS_FAHT") = "X" Then
                    NewRowStamm("Adresstyp") = ct.Transporttyp.Select("ID = '" & SelRow(i)("TRANSPORTTYP").ToString & "'")(0)("Text").ToString
                End If
                Fahrt = SelRow(i)("FAHRT")
                Dim SelDienstRow As DataRow() = ct.Dienstleistungen.Select("FAHRT = '" & Fahrt & "'")

                For iDienst = 0 To SelDienstRow.Length - 1
                    NewRowStamm("Dienstleistungen") += SelDienstRow(iDienst)("DIENSTL_TEXT").ToString & vbCrLf
                Next
                Dim SelBemerkRow As DataRow() = ct.Bemerkungen.Select("FAHRT = '" & Fahrt & "'")

                For iBemCount = 0 To SelBemerkRow.Length - 1
                    NewRowStamm("Bemerkung") += SelBemerkRow(iBemCount)("BEMERKUNG").ToString & vbCrLf
                Next

                tmptable.Rows.Add(NewRowStamm)
            Next



            Return tmptable
        End Function

        Private Function CreateTableProtokolle(ByVal ct As Transfer) As DataTable



            Dim tmptable As New DataTable
            tmptable.TableName = "Protokolle"
            tmptable.Columns.Add("Adresstyp", GetType(System.String))
            tmptable.Columns.Add("Protokolle", GetType(System.String))


            If Not ct.ProtokollArten Is Nothing Then
                'Fahrzeug 1
                Dim NewRowStamm As DataRow
                Dim SelRow As DataRow()

                SelRow = ct.Fahrten.Select("FAHRZEUG = '1'")

                Dim Fahrt As String

                For i = 0 To SelRow.Length - 1
                    NewRowStamm = tmptable.NewRow
                    If i > 0 Then
                        Fahrt = SelRow(i)("FAHRT")
                        Dim ProtkollRow As DataRow() = ct.ProtokollArten.Select("FAHRT = '" & Fahrt & "'")
                        If ProtkollRow.Length > 0 Then
                            If i > 0 AndAlso SelRow(i)("KENNZ_ZUS_FAHT") <> "X" Then
                                'NewRowStamm("Adresstyp") = "Zieladresse"
                                NewRowStamm("Adresstyp") = ct.Transporttyp.Select("ID = '" & SelRow(i)("TRANSPORTTYP").ToString & "'")(0)("Text").ToString
                            End If
                            If SelRow(i)("KENNZ_ZUS_FAHT") = "X" Then
                                NewRowStamm("Adresstyp") = ct.Transporttyp.Select("ID = '" & SelRow(i)("TRANSPORTTYP").ToString & "'")(0)("Text").ToString
                            End If
                            For iProtkoll = 0 To ProtkollRow.Length - 1
                                NewRowStamm("Protokolle") += ProtkollRow(iProtkoll)("Filename").ToString & vbCrLf
                            Next
                            tmptable.Rows.Add(NewRowStamm)
                        End If

                    End If
                Next

                SelRow = ct.Fahrten.Select("FAHRZEUG = '2'")

                For i = 0 To SelRow.Length - 1
                    NewRowStamm = tmptable.NewRow
                    Fahrt = SelRow(i)("FAHRT")
                    Dim ProtkollRow As DataRow() = ct.ProtokollArten.Select("FAHRT = '" & Fahrt & "'")
                    If ProtkollRow.Length > 0 Then
                        If i = 0 Then
                            NewRowStamm("Adresstyp") = ct.Transporttyp.Select("ID = '" & SelRow(i)("TRANSPORTTYP").ToString & "'")(0)("Text").ToString
                        End If
                        If i > 0 AndAlso SelRow(i)("KENNZ_ZUS_FAHT") <> "X" Then
                            NewRowStamm("Adresstyp") = ct.Transporttyp.Select("ID = '" & SelRow(i)("TRANSPORTTYP").ToString & "'")(0)("Text").ToString
                            'NewRowStamm("Adresstyp") = "Zieladresse"
                        End If
                        If SelRow(i)("KENNZ_ZUS_FAHT") = "X" Then
                            NewRowStamm("Adresstyp") = ct.Transporttyp.Select("ID = '" & SelRow(i)("TRANSPORTTYP").ToString & "'")(0)("Text").ToString
                        End If
                        For iProtkoll = 0 To ProtkollRow.Length - 1
                            NewRowStamm("Protokolle") += ProtkollRow(iProtkoll)("Filename").ToString & vbCrLf
                        Next
                        tmptable.Rows.Add(NewRowStamm)
                    End If

                Next
            End If

            Return tmptable
        End Function

        Protected Sub ibtCopyAbholadresse_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtCopyAbholadresse.Click
            ZielResetRu()

            txtRuZielFirma.Text = txtAbFirma.Text
            txtRuZielStrasse.Text = txtAbStrasse.Text
            txtRuZielPlz.Text = txtAbPLZ.Text
            txtRuZielOrt.Text = txtAbOrt.Text
            txtRuZielAnsprechpartner.Text = txtAbAnsprechpartner.Text
            txtRuZielTelefon.Text = txtAbTelefon.Text

        End Sub

        Private Sub RefillAbholAndZiel()

            Dim ct As Transfer = CType(Session("Transfer"), Transfer)


            ddlZielTransporttyp.DataSource = ct.Transporttyp
            ddlZielTransporttyp.DataValueField = "ID"
            ddlZielTransporttyp.DataTextField = "Text"
            ddlZielTransporttyp.DataBind()

            'Abholfahrt laden
            Dim SelRow As DataRow() = ct.Fahrten.Select("FAHRZEUG = '1' and FAHRT = '0'")

            'If SelRow.Length > 0 Then
            '    txtAbDatum.Text = SelRow(0)("VDATU").ToString
            '    txtAbUhrzeit.Text = SelRow(0)("VTIMEU").ToString
            'End If

            'Abholadresse laden
            SelRow = ct.Adressen.Select("FAHRT = '0'")
            If SelRow.Length > 0 Then
                txtAbFirma.Text = SelRow(0)("NAME").ToString
                txtAbAnsprechpartner.Text = SelRow(0)("NAME_2").ToString
                txtAbStrasse.Text = SelRow(0)("STREET").ToString
                txtAbPLZ.Text = SelRow(0)("POSTL_CODE").ToString
                txtAbOrt.Text = SelRow(0)("CITY").ToString
                ddlAbLand.SelectedValue = SelRow(0)("COUNTRY").ToString
                txtAbTelefon.Text = SelRow(0)("TELEPHONE").ToString
            End If


            'Zielfahrt laden
            Dim ZielFahrtID As String = ""
            SelRow = ct.Fahrten.Select("FAHRZEUG = '1' and FAHRT <> '0' and KENNZ_ZUS_FAHT <> 'X'")
            If SelRow.Length > 0 Then
                ZielFahrtID = SelRow(0)("FAHRT").ToString
                ddlZielTransporttyp.SelectedValue = SelRow(0)("TRANSPORTTYP").ToString
                txtZielDatum.Text = SelRow(0)("VDATU").ToString
                'txtZielUhrzeit.Text = SelRow(0)("VTIMEU").ToString

                If SelRow(0)("AT_TIM_VON") = "" Then
                    ddlZielUhrzeit.SelectedValue = "0-0"
                Else
                    ddlZielUhrzeit.SelectedValue = SelRow(0)("AT_TIM_VON") & "-" & SelRow(0)("AT_TIM_BIS")

                End If

            End If

            'Zieladresse laden
            If ZielFahrtID.Length > 0 Then
                SelRow = ct.Adressen.Select("FAHRT = '" & ZielFahrtID & "'")
                If SelRow.Length > 0 Then
                    txtZielFirma.Text = SelRow(0)("NAME").ToString
                    txtZielAnsprechpartner.Text = SelRow(0)("NAME_2").ToString
                    txtZielStrasse.Text = SelRow(0)("STREET").ToString
                    txtZielPLZ.Text = SelRow(0)("POSTL_CODE").ToString
                    txtZielOrt.Text = SelRow(0)("CITY").ToString
                    ddlZielland.SelectedValue = SelRow(0)("COUNTRY").ToString
                    txtZielTelefon.Text = SelRow(0)("TELEPHONE").ToString
                End If
            End If

        End Sub

        Protected Sub lbtNewOrderSameAdress_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lbtNewOrderSameAdress.Click
            Dim obj As Transfer = Session("Transfer")

            obj.ReUseAdresses = True

            Session("Transfer") = obj

            Response.Redirect("Change02s.aspx?AppID=" & Session("AppID").ToString, False)

        End Sub

        Protected Sub ibtnProtokollUpload1_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtnProtokollUpload1.Click
            Dim ct As Transfer = Session("Transfer")
            FillGridProtokolle(ct.ProtokollArten)
            FillGridProtokolle2(ct.ProtokollArten)
            trProtokollUpload1.Visible = Not trProtokollUpload1.Visible
            trProtokollUpload0.Visible = Not trProtokollUpload0.Visible
        End Sub

        Private Function upload(ByVal uFile As System.Web.HttpPostedFile, ByVal Filename As String) As Boolean

            Dim uploadPath = ConfigurationManager.AppSettings("LogistikCCProtocolUploadTemp")
            If String.IsNullOrEmpty(uploadPath) Then
                uploadPath = "C:\inetpub\wwwroot\Services\Components\ComCommon\Logistik\Dokumente\TempDoc\"
            End If

            Dim filepath As String = Path.Combine(uploadPath, getKundennummer())

            Dim info As FileInfo
            Try
                If Not Directory.Exists(filepath) Then
                    Directory.CreateDirectory(filepath)
                End If

                uFile.SaveAs(Path.Combine(filepath, Filename))

                info = New FileInfo(Path.Combine(filepath, Filename))

                If Not (info.Exists) Then
                    Return True
                    Exit Function
                End If
                Return False
            Catch ex As Exception

                lblError.Text = ex.Message
                lblError.Visible = True

                Return True
            End Try
        End Function

        Private Sub TransferFiles(ByVal ct As Transfer, ByVal sVBELN As String, ByVal sFahrt As String, ByVal kunnummer As String)
            Dim ProtokollRows() As DataRow = ct.ProtokollArten.Select("Fahrt =" & sFahrt)
            Dim sKategorie As String


            Dim uploadPath = ConfigurationManager.AppSettings("LogistikCCProtocolUploadTemp")
            If String.IsNullOrEmpty(uploadPath) Then
                uploadPath = "C:\inetpub\wwwroot\Services\Components\ComCommon\Logistik\Dokumente\TempDoc\" 'default
            End If

            Dim filepath As String = Path.Combine(uploadPath, kunnummer)

            Dim filepathToCopy As String = String.Empty

            If useLocalTemppath Then
                filepathToCopy = "D:\Dokumente\" + kunnummer.PadLeft(10, "0"c).ToString + "\" + sVBELN + "\Vertraege\"
            Else
                filepathToCopy = ConfigurationManager.AppSettings("UploadPathSambaArchive") + kunnummer.PadLeft(10, "0"c).ToString + "\" + sVBELN + "\Vertraege\"
            End If


            Dim filename As String = ""
            Dim sDokArt As String = ""

            If ProtokollRows.Length > 0 Then
                For Each ProtokollRow As DataRow In ProtokollRows

                    filename = ProtokollRow("Filename")
                    If filename <> "" Then
                        sDokArt = Replace(ProtokollRow("ZZPROTOKOLLART"), ".", "")
                        sKategorie = ProtokollRow("ZZKATEGORIE").ToString
                        Dim filenameNew As String = sVBELN + "_" + sFahrt + "_" + sKategorie + "_" + sDokArt + ".pdf"
                        Dim info As System.IO.FileInfo
                        If Directory.Exists(filepathToCopy) Then
                            File.Copy(Path.Combine(filepath, filename), filepathToCopy & "/" & filenameNew)
                        Else
                            Directory.CreateDirectory(filepathToCopy)
                            File.Copy(Path.Combine(filepath, filename), filepathToCopy & "/" & filenameNew)
                        End If

                        info = New FileInfo(Path.Combine(filepath, filename))

                        If Not (info.Exists) Then

                            Exit Sub
                        End If
                        ProtokollRow("Filename") = filenameNew
                    End If

                Next
            End If
        End Sub

        Protected Sub grvProtokollUpload1_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles grvProtokollUpload1.RowCommand
            Dim ct As Transfer = CType(Session("Transfer"), Transfer)
            If e.CommandName = "Loeschen" Then
                Dim lfdNr As String = e.CommandArgument
                Dim ProtokollRow As DataRow = ct.ProtokollArten.Select("ID=" & lfdNr)(0)
                ProtokollRow("Filename") = ""
                ct.ProtokollArten.AcceptChanges()
                Session("Transfer") = ct
                FillGridProtokolle(ct.ProtokollArten)
            End If
        End Sub
        Protected Sub ibtnProtokollUpload2_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtnProtokollUpload2.Click
            Dim ct As Transfer = Session("Transfer")
            FillGridProtokolle(ct.ProtokollArten)
            FillGridProtokolle2(ct.ProtokollArten)
            trProtokollUpload2.Visible = Not trProtokollUpload2.Visible
            trProtokollUpload3.Visible = Not trProtokollUpload3.Visible
        End Sub
        Protected Sub grvProtokollUpload2_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles grvProtokollUpload2.RowCommand
            Dim ct As Transfer = CType(Session("Transfer"), Transfer)
            If e.CommandName = "Loeschen" Then
                Dim lfdNr As String = e.CommandArgument
                Dim ProtokollRow As DataRow = ct.ProtokollArten.Select("ID=" & lfdNr)(0)
                ProtokollRow("Filename") = ""
                ct.ProtokollArten.AcceptChanges()
                Session("Transfer") = ct
                FillGridProtokolle(ct.ProtokollArten)
            End If
        End Sub

        Protected Sub cmdJaWarnung_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdJaWarnung.Click

            Dim ct As Transfer = Session("Transfer")
            Dim booFahrzeug1AlreadySet As Boolean = False
            Dim SelRow() As DataRow

            Dim FahrtNr As String

            SelRow = ct.Fahrten.Select("FAHRZEUG = '1' and FAHRT <> '0' and KENNZ_ZUS_FAHT <> 'X'")
            FahrtNr = SelRow(0)("FAHRT").ToString

            If litMessage.Text.Contains("Vorholung") Then
                SetSonderdienstleistung(ct, ct.VorholDlNummer, CInt(FahrtNr), False)
                Session("Transfer") = ct
                ct = Nothing
                divMessage.Visible = False
                Exit Sub
            End If


            'Samstag wurde gewählt
            If litMessage.Text.Contains("Samstag") Then

                If IsDate(txtAbDatum.Text) Then
                    If CDate(txtAbDatum.Text).DayOfWeek = DayOfWeek.Saturday Then
                        SetSonderdienstleistung(ct, ct.SamstagDlNummer, CInt(FahrtNr), False)
                        booFahrzeug1AlreadySet = True
                    End If
                End If


                If booFahrzeug1AlreadySet = False Then
                    If IsDate(txtZielDatum.Text) Then
                        If CDate(txtZielDatum.Text).DayOfWeek = DayOfWeek.Saturday Then
                            SetSonderdienstleistung(ct, ct.SamstagDlNummer, CInt(FahrtNr), False)
                        End If
                    End If
                End If

                If IsDate(txtRuZielDatum.Text) Then
                    If CDate(txtRuZielDatum.Text).DayOfWeek = DayOfWeek.Saturday Then

                        'Fahrt für das 2te Fahrzeug ermitteln
                        Dim RueckFahrt As String


                        SelRow = ct.Fahrten.Select("FAHRZEUG = '2' and KENNZ_ZUS_FAHT <> 'X'")

                        RueckFahrt = SelRow(0)("FAHRT").ToString

                        SetSonderdienstleistung(ct, ct.SamstagDlNummer, CInt(RueckFahrt), True)
                    End If
                End If

            Else 'Express wurde gewählt
                SetSonderdienstleistung(ct, ct.ExpressDlNummer, CInt(FahrtNr), False)
            End If


            Session("Transfer") = ct
            ct = Nothing
            divMessage.Visible = False
        End Sub

        Protected Sub cmdNeinWarnung_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdNeinWarnung.Click
            divMessage.Visible = False
            LinkButton2_Click(sender, e)
        End Sub

        Private Sub SetSonderdienstleistung(ByRef ct As Transfer, ByVal ASNUM As String, ByVal Fahrt As Integer, ByVal Rueckfahrt As Boolean)

            Dim NewRow As DataRow
            Dim cbl As CheckBoxList


            If Rueckfahrt = False Then
                cbl = chkGruende
            Else
                cbl = chkGruendeRueck
            End If

            NewRow = ct.Dienstleistungen.NewRow

            ct.DienstAuswahl.DefaultView.RowFilter = "ASNUM = '" & ASNUM & "'"

            NewRow("FAHRT") = Fahrt.ToString
            NewRow("DIENSTL_NR") = ct.DienstAuswahl.DefaultView(0)("ASNUM")
            NewRow("DIENSTL_TEXT") = ct.DienstAuswahl.DefaultView(0)("ASKTX")
            NewRow("MATNR") = ct.DienstAuswahl.DefaultView(0)("EAN11")
            ct.Dienstleistungen.Rows.Add(NewRow)
            ct.Dienstleistungen.AcceptChanges()
            ct.Dienstleistungen.DefaultView.RowFilter = "Fahrt = '" & Fahrt & "'"
            cbl.DataSource = ct.Dienstleistungen.DefaultView
            cbl.DataValueField = "DIENSTL_NR"
            cbl.DataTextField = "DIENSTL_TEXT"
            cbl.DataBind()

            For Each litem As ListItem In cbl.Items
                litem.Selected = True
                litem.Attributes.Add("onclick", "return false;")

            Next



        End Sub

        ' Protected Sub AsyncFileUpload1_UploadedComplete(ByVal sender As Object, ByVal e As FileUploadedEventArgs)
        Protected Sub AsyncFileUpload1_UploadedComplete(ByVal sender As Object, ByVal e As AjaxControlToolkit.AsyncFileUploadEventArgs)
            Dim FileUpload As AjaxControlToolkit.AsyncFileUpload = sender
            If Not FileUpload Is Nothing Then
                If FileUpload.HasFile = True Then
                    If FileUpload.ContentType = "application/pdf" Then
                        Dim ct As Transfer = CType(Session("Transfer"), Transfer)
                        Dim grvrow As GridViewRow = grvProtokollUpload1.Rows(CInt(FileUpload.ToolTip))
                        Dim lblFahrt As Label = CType(grvrow.Cells(1).FindControl("lblFahrt"), Label)
                        Dim Fahrt As String = lblFahrt.Text
                        Dim lfdNr As String = grvrow.Cells(0).Text
                        Dim DokArt As String = grvrow.Cells(2).Text

                        Dim ProtokollRow As DataRow = ct.ProtokollArten.Select("ID=" & lfdNr)(0)
                        Dim filename As String = System.IO.Path.GetFileName(FileUpload.FileName)
                        Dim uploadPath = ConfigurationManager.AppSettings("LogistikCCProtocolUploadTemp")
                        If String.IsNullOrEmpty(uploadPath) Then
                            uploadPath = "C:\inetpub\wwwroot\Services\Components\ComCommon\Logistik\Dokumente\TempDoc\"
                        End If

                        Dim filepath As String = Path.Combine(uploadPath, getKundennummer())
                        If Not System.IO.Directory.Exists(filepath) Then
                            System.IO.Directory.CreateDirectory(filepath)
                        End If
                        filename = String.Format("{0}_{1}_{2:yyyyMMddhhmmss}.pdf", Fahrt, Replace(DokArt, ".", ""), DateTime.Now)
                        FileUpload.SaveAs(Path.Combine(filepath, filename))
                        ProtokollRow("Filename") = filename
                        ct.ProtokollArten.AcceptChanges()
                        Session("Transfer") = ct
                    End If
                End If

            End If
        End Sub

        Protected Sub UploadedComplete(ByVal sender As Object, ByVal e As FileUploadedEventArgs)

            Try

                Dim FileUpload = DirectCast(sender, RadAsyncUpload)
                If Not FileUpload Is Nothing Then
                    If FileUpload.UploadedFiles.Count > 0 Then
                        If e.File.ContentType = "application/pdf" Then

                            Dim ct As Transfer = CType(Session("Transfer"), Transfer)
                            Dim grvrow As GridViewRow = grvProtokollUpload1.Rows(CInt(FileUpload.ToolTip))
                            Dim lblFahrt As Label = CType(grvrow.Cells(1).FindControl("lblFahrt"), Label)
                            Dim Fahrt As String = lblFahrt.Text
                            Dim lfdNr As String = grvrow.Cells(0).Text
                            Dim DokArt As String = grvrow.Cells(2).Text

                            Dim ProtokollRow As DataRow = ct.ProtokollArten.Select("ID=" & lfdNr)(0)

                            Dim filename As String = System.IO.Path.GetFileName(e.File.FileName)

                            Dim uploadPath = ConfigurationManager.AppSettings("LogistikCCProtocolUploadTemp")
                            If String.IsNullOrEmpty(uploadPath) Then
                                uploadPath = "C:\inetpub\wwwroot\Services\Components\ComCommon\Logistik\Dokumente\TempDoc\"
                            End If

                            Dim filepath As String = Path.Combine(uploadPath, getKundennummer())

                            If Not System.IO.Directory.Exists(filepath) Then
                                System.IO.Directory.CreateDirectory(filepath)
                            End If

                            filename = String.Format("{0}_{1}_{2:yyyyMMddhhmmss}.pdf", Fahrt, Replace(DokArt, ".", ""), DateTime.Now)

                            e.File.SaveAs(Path.Combine(filepath, filename))

                            Dim lbltmp As Label = grvrow.FindControl("lblUplFile")
                            lbltmp.Text = filename
                            lbltmp.Visible = True
                            grvrow.FindControl("ibtnDelUploadFile1").Visible = True
                            grvrow.FindControl("radUpload1").Visible = False

                            ProtokollRow("Filename") = filename
                            ct.ProtokollArten.AcceptChanges()

                            Session("Transfer") = ct

                        End If
                    Else

                        lblError.Text = "Keine Datein zum Hochladen"
                        lblError.Visible = True
                    End If


                End If


            Catch ex As Exception
                lblError.Text = "Error: " + ex.Message
                lblError.Visible = True
            End Try

        End Sub


        'für ajax Toolkit
        Protected Sub AsyncFileUpload1_UploadedComplete2(ByVal sender As Object, ByVal e As AjaxControlToolkit.AsyncFileUploadEventArgs)
            Dim FileUpload As AjaxControlToolkit.AsyncFileUpload = sender
            If Not FileUpload Is Nothing Then
                If FileUpload.HasFile = True Then
                    If FileUpload.ContentType = "application/pdf" Then
                        Dim ct As Transfer = CType(Session("Transfer"), Transfer)
                        Dim grvrow As GridViewRow = grvProtokollUpload2.Rows(CInt(FileUpload.ToolTip))
                        Dim lblFahrt As Label = CType(grvrow.Cells(1).FindControl("lblFahrt"), Label)
                        Dim Fahrt As String = lblFahrt.Text
                        Dim lfdNr As String = grvrow.Cells(0).Text
                        Dim DokArt As String = grvrow.Cells(2).Text

                        Dim ProtokollRow As DataRow = ct.ProtokollArten.Select("ID=" & lfdNr)(0)
                        Dim filename As String = Path.GetFileName(FileUpload.FileName)
                        Dim uploadPath = ConfigurationManager.AppSettings("LogistikCCProtocolUploadTemp")
                        If String.IsNullOrEmpty(uploadPath) Then
                            uploadPath = "C:\inetpub\wwwroot\Services\Components\ComCommon\Logistik\Dokumente\TempDoc\"
                        End If

                        Dim filepath As String = Path.Combine(uploadPath, getKundennummer())
                        If Not Directory.Exists(filepath) Then
                            Directory.CreateDirectory(filepath)
                        End If
                        filename = String.Format("{0}_{1}_{2:yyyyMMddhhmmss}.pdf", Fahrt, Replace(DokArt, ".", ""), DateTime.Now)
                        FileUpload.SaveAs(Path.Combine(filepath, filename))
                        ProtokollRow("Filename") = filename
                        ct.ProtokollArten.AcceptChanges()
                        Session("Transfer") = ct
                    End If
                End If

            End If
        End Sub

        Protected Sub UploadedComplete2(ByVal sender As Object, ByVal e As FileUploadedEventArgs)
            Try
                Dim FileUpload = DirectCast(sender, RadAsyncUpload)

                If Not FileUpload Is Nothing Then
                    If FileUpload.UploadedFiles.Count > 0 Then

                        If e.File.ContentType = "application/pdf" Then
                            Dim ct As Transfer = CType(Session("Transfer"), Transfer)
                            Dim grvrow As GridViewRow = grvProtokollUpload2.Rows(CInt(FileUpload.ToolTip))
                            Dim lblFahrt As Label = CType(grvrow.Cells(1).FindControl("lblFahrt"), Label)
                            Dim Fahrt As String = lblFahrt.Text
                            Dim lfdNr As String = grvrow.Cells(0).Text
                            Dim DokArt As String = grvrow.Cells(2).Text

                            Dim ProtokollRow As DataRow = ct.ProtokollArten.Select("ID=" & lfdNr)(0)
                            Dim filename As String = Path.GetFileName(e.File.FileName)

                            Dim filepath As String = "C:\inetpub\wwwroot\Services\Components\ComCommon\Logistik\Dokumente\TempDoc\" & getKundennummer()
                            If Not Directory.Exists(filepath) Then
                                Directory.CreateDirectory(filepath)
                            End If
                            filename = Fahrt & "_" & Replace(DokArt, ".", "") & "_" & Format(Now, "yyyyMMddhhmmss") & ".pdf"

                            e.File.SaveAs(filepath + "\" + filename)

                            Dim lbltmp As Label = grvrow.FindControl("lblUplFile")
                            lbltmp.Text = filename
                            lbltmp.Visible = True
                            grvrow.FindControl("ibtnDelUploadFile1").Visible = True
                            grvrow.FindControl("radUpload2").Visible = False

                            ProtokollRow("Filename") = filename

                            ct.ProtokollArten.AcceptChanges()

                            Session("Transfer") = ct

                        End If
                    Else
                        lblError.Text = "Keine Datein zum Hochladen"
                        lblError.Visible = True
                    End If
                End If

            Catch ex As Exception
                lblError.Text = "Error: " + ex.Message
                lblError.Visible = True
            End Try
        End Sub

        'Protected Sub lbBack_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lbBack.Click
        '    Response.Redirect("../../../Start/Selection.aspx?AppID=" & Session("AppID").ToString, False)
        'End Sub

        Protected Sub lb_Kilometer_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles lb_Kilometer.Click


            If CheckFahrten() = False Then

                If CheckDates() = True Then
                    lblErrorFahrten.Text = "Bitte geben Sie für jede Fahrt ein Datum an, oder lassen Sie alle frei."
                    lblErrorFahrten.Visible = True
                    Exit Sub
                End If
                If CheckDates2() = True Then
                    lblErrorFahrten.Text = "Bitte beachten Sie! Der Termin der ersten Abholung darf nicht größer als die folgenenden Termine."
                    lblErrorFahrten.Visible = True
                    Exit Sub
                End If

                If pnlZusatzfahrten.Visible = True Then

                    If CheckZusatz() = False Then
                        lblErrorFahrten.Visible = True
                        lblErrorFahrten.Text = "Bitte bestätigen Sie Ihre Zusatzfahrt, oder schließen Sie diese."
                    Else
                        lblErrorFahrten.Text = "Zusatzfahrt: Bitte überprüfen Sie Ihre Eingaben."
                    End If

                    lblErrorFahrtBottom.Text = lblErrorFahrten.Text

                    Exit Sub

                End If

                If pnlRZusatzfahrten.Visible = True Then

                    If CheckRueckZusatz() = False Then

                        lblErrorFahrten.Visible = True
                        lblErrorFahrten.Text = "Bitte bestätigen Sie Ihre Zusatzfahrt, oder schließen Sie diese."
                    Else
                        lblErrorFahrten.Text = "Zusatzfahrt: Bitte überprüfen Sie Ihre Eingaben."
                    End If

                    lblErrorFahrtBottom.Text = lblErrorFahrten.Text

                    Exit Sub

                End If

                WriteDataAbholUndZieladresse()


                SetEntfernungen()

                divKilometer.Visible = True

                Dim EmptyFiller As String = "-&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;"

                GridView1.Rows(0).Cells(4).Text = "-"

                'Auffüllen
                For Each dr As GridViewRow In GridView1.Rows
                    If Not dr.Cells(4).Text.Contains("-") Then
                        dr.Cells(4).Text = dr.Cells(4).Text & " km"
                    Else
                        dr.Cells(4).Text = EmptyFiller
                    End If
                Next


            End If



        End Sub

        Private Sub SetEntfernungen()
            Dim ct As Transfer = CType(Session("Transfer"), Transfer)

            ct.Entfernungen = ct.Adressen.Copy

            ct.Entfernungen.Columns.Add("KM", GetType(System.String))
            ct.Entfernungen.AcceptChanges()

            ct.Entfernungen.Rows(0)("KM") = " - "

            ct.FillEntfernungen(m_User, Me.Page)

            GridView1.DataSource = ct.Entfernungen
            GridView1.DataBind()

            Session("Transfer") = ct
        End Sub

        Protected Sub lbtKmClose_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lbtKmClose.Click
            divKilometer.Visible = False
        End Sub

        Protected Sub ibtMsgCancel_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtMsgCancel.Click
            divMessage.Visible = False
        End Sub

        Protected Sub cmdOKWarnung_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdOKWarnung.Click
            divMessage.Visible = False
        End Sub

        Private Sub CheckRequest()

            Dim Open As Boolean

            Select Case Me.Request.Form("__EVENTTARGET")
                Case "divAbholadresse"
                    If CheckAbholadresse() = True Then
                        Exit Select
                    End If

                    If pnlZieladresse.Visible = True Then
                        If CheckZieladresse() = True Then
                            Exit Select
                        End If
                    End If

                    Open = pnlAbholadresse.Visible
                    CloseFahrtenPanels()
                    ibtAbholadresseHeaderClose.Visible = Not Open
                    ibtAbholadresseHeader.Visible = Open
                    pnlAbholadresse.Visible = Not Open
                Case "divZieladresse"
                    If CheckAbholadresse() = True Then
                        Exit Select
                    End If

                    If pnlZieladresse.Visible = True Then
                        If CheckZieladresse() = True Then
                            Exit Select
                        End If
                    End If

                    Open = pnlZieladresse.Visible
                    CloseFahrtenPanels()
                    ibtZieladresseHeaderClose.Visible = Not Open
                    ibtZieladresseHeader.Visible = Open
                    pnlZieladresse.Visible = Not Open
                Case "divZusatz"
                    'If CheckAbholadresse() = True Then
                    '    Exit Select
                    'End If


                    'If CheckZieladresse() = True Then
                    '    Exit Select
                    'End If

                    'Open = pnlZusatzfahrten.Visible
                    'CloseFahrtenPanels()
                    'ibtCloseZusatzfahrt.Visible = Not Open
                    'ibtOpenZusatzfahrt.Visible = Open
                    'pnlZusatzfahrten.Visible = Not Open

                    'If pnlZusatzfahrten.Visible = True Then
                    '    OpenZusatzfahrt()
                    'End If

                    OpenZusatzfahrt()

                Case "divZieladresseRueck"
                    If CheckAbholadresse() = True Then
                        Exit Select
                    End If

                    If CheckZieladresse() = True Then
                        Exit Select
                    End If

                    Open = pnlRueckZieladresse.Visible
                    CloseFahrtenPanels()
                    ibtHeaderZielRueckClose.Visible = Not Open
                    ibtHeaderZielRueck.Visible = Open
                    pnlRueckZieladresse.Visible = Not Open
                Case "divRZusatz"
                    'If CheckAbholadresse() = True Then
                    '    Exit Select
                    'End If

                    'If pnlZieladresse.Visible = True Then
                    '    If CheckZieladresse() = True Then
                    '        Exit Select
                    '    End If
                    'End If

                    'Open = pnlRZusatzfahrten.Visible
                    'CloseFahrtenPanels()
                    'ibtRZClose.Visible = Not Open
                    'ibtRZOpen.Visible = Open
                    'pnlRZusatzfahrten.Visible = Not Open

                    'If pnlRZusatzfahrten.Visible = True Then
                    '    OpenRueckZusatzfahrt()
                    'End If

                    OpenRueckZusatzfahrt()

                Case "divDlZieladresse"
                    Open = trDLZieladresse.Visible
                    CloseDlPanels()
                    ibtHeaderDlZieladresseClose.Visible = Not Open
                    ibtHeaderDlZieladresse.Visible = Open
                    trDLZieladresse.Visible = Not Open
                    pnlDlZieladresseFirma.Visible = Not Open
                Case "divDlRuZieladresse"
                    Open = pnlDlRuZieladresse.Visible
                    CloseDlPanels()
                    ibtHeaderDlRuZieladresseClose.Visible = Not Open
                    ibtHeaderDlRuZieladresse.Visible = Open
                    pnlDlRuZieladresse.Visible = Not Open
                    pnlDlRuZieladresseFirma.Visible = Not Open
            End Select



        End Sub

        Private Sub CloseFahrtenPanels()

            pnlAbholadresse.Visible = False
            ibtAbholadresseHeaderClose.Visible = False
            ibtAbholadresseHeader.Visible = True


            pnlZieladresse.Visible = False
            ibtZieladresseHeaderClose.Visible = False
            ibtZieladresseHeader.Visible = True

            pnlZusatzfahrten.Visible = False
            ibtCloseZusatzfahrt.Visible = False
            ibtOpenZusatzfahrt.Visible = True

            pnlRueckZieladresse.Visible = False
            ibtHeaderZielRueckClose.Visible = False
            ibtHeaderZielRueck.Visible = True

            pnlRZusatzfahrten.Visible = False
            ibtRZClose.Visible = False
            ibtRZOpen.Visible = True

        End Sub

        Private Sub CloseDlPanels()

            trDLZieladresse.Visible = False
            pnlDlZieladresseFirma.Visible = False
            ibtHeaderDlZieladresseClose.Visible = False
            ibtHeaderDlZieladresse.Visible = True

            trZusatz2DL.Visible = False

            If grvZusatzDL.Rows.Count > 0 Then

                Dim ibt As ImageButton

                For Each dr As GridViewRow In grvZusatzDL.Rows

                    ibt = CType(dr.FindControl("ibtDlZuClose"), ImageButton)




                    If Not ibt Is Nothing Then
                        ibt.Visible = False
                    End If

                    ibt = CType(dr.FindControl("ibtDlZuOpen"), ImageButton)
                    If Not ibt Is Nothing Then
                        ibt.Visible = True
                    End If
                Next

            End If

            pnlDlRuZieladresse.Visible = False
            pnlDlRuZieladresseFirma.Visible = False
            ibtHeaderDlRuZieladresseClose.Visible = False
            ibtHeaderDlRuZieladresse.Visible = True

            trRuZusatzDL.Visible = False
            If grvRueckZusatzDL.Rows.Count > 0 Then

                Dim ibt As ImageButton

                For Each dr As GridViewRow In grvRueckZusatzDL.Rows

                    ibt = CType(dr.FindControl("ibtDlRuClose"), ImageButton)
                    If Not ibt Is Nothing Then
                        ibt.Visible = False
                    End If
                    ibt = CType(dr.FindControl("ibtDlRuOpen"), ImageButton)
                    If Not ibt Is Nothing Then
                        ibt.Visible = True
                    End If
                Next

            End If


        End Sub

        Protected Sub ibtZusatzfahrtNext_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtZusatzfahrtNext.Click
            SaveZusatzfahrt()

        End Sub

        Protected Sub ibtAbholadresseNext_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtAbholadresseNext.Click

            If CheckAbholadresse() = True Then
                Exit Sub
            End If
            CloseFahrtenPanels()

            pnlZieladresse.Visible = True
            ibtZieladresseHeaderClose.Visible = True
            ibtZieladresseHeader.Visible = False

        End Sub

        Private Function CheckZieladresse() As Boolean

            Dim booError As Boolean = False

            Dim ct As Transfer = CType(Session("Transfer"), Transfer)

            If txtZielFirma.Text.Length = 0 Then
                SetErrorFrame(txtZielFirma) : booError = True
            End If

            If txtZielStrasse.Text.Length = 0 Then
                SetErrorFrame(txtZielStrasse) : booError = True
            End If

            If CInt(ct.Laender.Select("Land1='" & ddlZielland.SelectedItem.Value & "'")(0)("Lnplz")) > 0 Then
                If Not CInt(ct.Laender.Select("Land1='" & ddlZielland.SelectedItem.Value & "'")(0)("Lnplz")) = txtZielPLZ.Text.Trim(" "c).Length Then
                    SetErrorFrame(txtZielPLZ) : booError = True
                End If
            End If


            If txtZielPLZ.Text.Length = 0 Then
                SetErrorFrame(txtZielPLZ) : booError = True
            End If

            If txtZielOrt.Text.Length = 0 Then
                SetErrorFrame(txtZielOrt) : booError = True
            End If

            If txtZielAnsprechpartner.Text.Length = 0 Then
                SetErrorFrame(txtZielAnsprechpartner) : booError = True
            End If

            If txtZielTelefon.Text.Length = 0 Then
                SetErrorFrame(txtZielTelefon) : booError = True
            End If

            If ddlZielTransporttyp.SelectedItem.Value = "00" Then
                divZielTransporttyp.Style.Add("border", "solid 1px red") : booError = True
            End If

            If booError = True Then
                lblErrorFahrten.Visible = True
                lblErrorFahrten.Text = "Bitte geben Sie zunächst die Zieladresse Fzg. 1 ein."
            End If

            Return booError

        End Function

        Private Function CheckAbholadresse() As Boolean


            Dim booError As Boolean = False

            Dim ct As Transfer = CType(Session("Transfer"), Transfer)

            'Abholadresse
            If txtAbFirma.Text.Length = 0 Then
                SetErrorFrame(txtAbFirma) : booError = True
            End If

            If txtAbStrasse.Text.Length = 0 Then
                SetErrorFrame(txtAbStrasse) : booError = True
            End If

            If CInt(ct.Laender.Select("Land1='" & ddlAbLand.SelectedItem.Value & "'")(0)("Lnplz")) > 0 Then
                If Not CInt(ct.Laender.Select("Land1='" & ddlAbLand.SelectedItem.Value & "'")(0)("Lnplz")) = txtAbPLZ.Text.Trim(" "c).Length Then
                    SetErrorFrame(txtAbPLZ) : booError = True
                End If
            End If


            If txtAbPLZ.Text.Length = 0 Then
                SetErrorFrame(txtAbPLZ) : booError = True
            End If

            If txtAbOrt.Text.Length = 0 Then
                SetErrorFrame(txtAbOrt) : booError = True
            End If

            If txtAbAnsprechpartner.Text.Length = 0 Then
                SetErrorFrame(txtAbAnsprechpartner) : booError = True
            End If

            If txtAbTelefon.Text.Length = 0 Then
                SetErrorFrame(txtAbTelefon) : booError = True
            End If

            If booError = True Then
                lblErrorFahrten.Visible = True
                lblErrorFahrten.Text = "Bitte geben Sie zunächst die Abholadresse ein."
            End If

            Return booError

        End Function

        Protected Sub ibtZieladresseNext_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles ibtZieladresseNext.Click

            If CheckZieladresse() = True Then
                Exit Sub
            End If

            If trRueck.Visible = False Then
                FahrtenWeiter()
                If trZusatzDL.Visible = True Then
                    trDLZieladresse.Visible = False
                    pnlDlZieladresseFirma.Visible = False
                End If
            Else
                CloseFahrtenPanels()

                pnlZieladresse.Visible = False
                ibtZieladresseHeaderClose.Visible = False
                ibtZieladresseHeader.Visible = True

                pnlRueckZieladresse.Visible = True
                ibtHeaderZielRueckClose.Visible = True
                ibtHeaderZielRueck.Visible = False

            End If
        End Sub

        Protected Sub ibtZusatzRueckNext_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles ibtZusatzRueckNext.Click
            SaveRueckZusatzfahrt()

        End Sub

        Protected Sub ibtRueckZielNext_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles ibtRueckZielNext.Click
            FahrtenWeiter()

            If trZusatzDL.Visible = True Then
                trDLZieladresse.Visible = False
                pnlDlZieladresseFirma.Visible = False
            End If
        End Sub

        Private Sub grvZusatzDL_RowDataBound(sender As Object, e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles grvZusatzDL.RowDataBound
            e.Row.Attributes.Add("onclick", Page.ClientScript.GetPostBackEventReference(DirectCast(sender, Control), "Edit$" & e.Row.RowIndex.ToString()))

        End Sub

        Protected Sub ibtDlZusatz_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles ibtDlZusatz.Click

            Dim ct As Transfer = CType(Session("Transfer"), Transfer)

            ct.ZuActiveFahrt = ""
            Session("Transfer") = ct

            ct = Nothing

            Dim ibt As ImageButton

            For i = 0 To grvZusatzDL.Rows.Count - 1
                ibt = CType(grvZusatzDL.Rows(i).FindControl("ibtDlZuOpen"), ImageButton)
                ibt.Visible = True

                ibt = CType(grvZusatzDL.Rows(i).FindControl("ibtDlZuClose"), ImageButton)
                ibt.Visible = False

            Next


            CloseDlPanels()
            trDLZieladresse.Visible = True
            pnlDlZieladresseFirma.Visible = True
            ibtHeaderDlZieladresseClose.Visible = True
            ibtHeaderDlZieladresse.Visible = False





        End Sub

        Protected Sub ibtDlZielNext_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles ibtDlZielNext.Click

            'Ggf. prüfen, ob Dateiupload wirklich geklappt hat
            Dim ct As Transfer = CType(Session("Transfer"), Transfer)
            If Not ct.ProtokollArten Is Nothing Then
                Dim blnDateiNichtGefunden As Boolean = False
                Dim strDateienFehlgeschlagen As String = ""
                Dim filename As String = ""
                Dim filepath As String = ""
                Dim kunnummer As String = getKundennummer()
                Dim uploadPath = ConfigurationManager.AppSettings("LogistikCCProtocolUploadTemp")
                If String.IsNullOrEmpty(uploadPath) Then
                    uploadPath = "C:\inetpub\wwwroot\Services\Components\ComCommon\Logistik\Dokumente\TempDoc\" 'default
                End If
                For Each dr As DataRow In ct.ProtokollArten.Rows
                    filename = dr("Filename")
                    If filename <> "" Then
                        filepath = Path.Combine(uploadPath, kunnummer)
                        If Not File.Exists(Path.Combine(filepath, filename)) Then
                            dr("Filename") = ""
                            blnDateiNichtGefunden = True
                            strDateienFehlgeschlagen &= " " & filename
                        End If
                    End If
                Next

                If blnDateiNichtGefunden Then
                    ct.ProtokollArten.AcceptChanges()
                    Session("Transfer") = ct
                    FillGridProtokolle(ct.ProtokollArten)
                    lblError.Visible = True
                    lblError.Text = "Der Dateiupload von " & strDateienFehlgeschlagen & " ist fehlgeschlagen."
                    Exit Sub
                End If
            End If

            lblError.Text = ""

            If tblRueckDL.Visible = True Then

                CloseDlPanels()

                If trRueckZusatzDL.Visible = True Then
                    trRuZusatzDL.Visible = True
                Else

                    pnlDlRuZieladresse.Visible = True
                    pnlDlRuZieladresseFirma.Visible = True
                    ibtHeaderDlRuZieladresseClose.Visible = True
                    ibtHeaderDlRuZieladresse.Visible = False


                End If
            Else
                NextToOverview()
            End If

        End Sub

        Private Sub grvRueckZusatzDL_RowDataBound(sender As Object, e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles grvRueckZusatzDL.RowDataBound
            e.Row.Attributes.Add("onclick", Page.ClientScript.GetPostBackEventReference(DirectCast(sender, Control), "Edit$" & e.Row.RowIndex.ToString()))
        End Sub

        Protected Sub ibtRuZielNext_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles ibtRuZielNext.Click

            'Ggf. prüfen, ob Dateiupload wirklich geklappt hat
            Dim ct As Transfer = CType(Session("Transfer"), Transfer)
            If Not ct.ProtokollArten Is Nothing Then
                Dim blnDateiNichtGefunden As Boolean = False
                Dim strDateienFehlgeschlagen As String = ""
                Dim filename As String = ""
                Dim filepath As String = ""
                Dim kunnummer As String = getKundennummer()
                Dim uploadPath = ConfigurationManager.AppSettings("LogistikCCProtocolUploadTemp")
                If String.IsNullOrEmpty(uploadPath) Then
                    uploadPath = "C:\inetpub\wwwroot\Services\Components\ComCommon\Logistik\Dokumente\TempDoc\" 'default
                End If
                For Each dr As DataRow In ct.ProtokollArten.Rows
                    filename = dr("Filename")
                    If filename <> "" Then
                        filepath = Path.Combine(uploadPath, kunnummer)
                        If Not File.Exists(Path.Combine(filepath, filename)) Then
                            dr("Filename") = ""
                            blnDateiNichtGefunden = True
                            strDateienFehlgeschlagen &= " " & filename
                        End If
                    End If
                Next

                If blnDateiNichtGefunden Then
                    ct.ProtokollArten.AcceptChanges()
                    Session("Transfer") = ct
                    FillGridProtokolle(ct.ProtokollArten)
                    lblError.Visible = True
                    lblError.Text = "Der Dateiupload von " & strDateienFehlgeschlagen & " ist fehlgeschlagen."
                    Exit Sub
                End If
            End If

            lblError.Text = ""

            NextToOverview()
        End Sub

        Protected Sub ibtDLRuZusatz_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles ibtDLRuZusatz.Click
            Dim ct As Transfer = CType(Session("Transfer"), Transfer)

            ct.ZuRueckActiveFahrt = ""

            Session("Transfer") = ct

            ct = Nothing

            Dim ibt As ImageButton

            For i = 0 To grvRueckZusatzDL.Rows.Count - 1
                ibt = CType(grvRueckZusatzDL.Rows(i).FindControl("ibtDlRuOpen"), ImageButton)
                ibt.Visible = True

                ibt = CType(grvRueckZusatzDL.Rows(i).FindControl("ibtDlRuClose"), ImageButton)
                ibt.Visible = False

            Next

            CloseDlPanels()
            pnlDlRuZieladresse.Visible = True
            pnlDlRuZieladresseFirma.Visible = True
            ibtHeaderDlRuZieladresseClose.Visible = True
            ibtHeaderDlRuZieladresse.Visible = False

        End Sub

        Private Sub grvZusatzDL_RowEditing(sender As Object, e As System.Web.UI.WebControls.GridViewEditEventArgs) Handles grvZusatzDL.RowEditing

        End Sub

        Private Sub grvRueckZusatzDL_RowEditing(sender As Object, e As System.Web.UI.WebControls.GridViewEditEventArgs) Handles grvRueckZusatzDL.RowEditing

        End Sub

        Private Function getKundennummer() As String

            Dim Kundennummer As String = m_User.KUNNR

            If m_User.Store = "AUTOHAUS" AndAlso m_User.Reference.Trim(" "c).Length > 0 AndAlso m_User.KUNNR = "261510" Then
                Kundennummer = m_User.Reference
            End If

            Return Kundennummer

        End Function

        Protected Sub cbxShowZusatzfahrten_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs) Handles cbxShowZusatzfahrten.CheckedChanged
            trZusatz.Visible = cbxShowZusatzfahrten.Checked
            trRZusatz.Visible = cbxShowZusatzfahrten.Checked
        End Sub

    End Class
End Namespace