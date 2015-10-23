Imports CKG.Base.Business
Imports CKG.Base.Kernel
Imports CKG.Base.Kernel.Common.Common
Imports CKG.Portal.PageElements

Imports SapORM.Contracts


Partial Public Class Report07_2
    Inherits System.Web.UI.Page

    Protected WithEvents ucStyles As styles
    Protected WithEvents ucHeader As header


    Private m_App As Base.Kernel.Security.App
    Private m_User As Base.Kernel.Security.User
    Private m_Report As Fahrzeughistorie
    Private objPDIs As Base.Business.ABEDaten
    Private Eqinnum As String

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Session("ShowLink") = "False"
        m_User = GetUser(Me)
        ucHeader.InitUser(m_User)
        FormAuth(Me, m_User)

        GetAppIDFromQueryString(Me)

        Try
            lblHead.Text = m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString
            ucStyles.TitleText = lblHead.Text
            m_App = New Base.Kernel.Security.App(m_User)


            If Not IsPostBack Then
                If Not Session("App_Report") Is Nothing Then
                    m_Report = CType(Session("App_Report"), Fahrzeughistorie)

                    If Not Session("App_Grunddaten") Is Nothing Then

                        m_Report.Grunddaten = CType(Session("App_Grunddaten"), DataTable)
                        'Übersicht
                        FillUebersicht()
                        'Typdaten
                        TypdatenFill()
                    End If
                    If Not Session("App_Historie") Is Nothing Then
                        'Lebenslauf
                        m_Report.Historie = CType(Session("App_Historie"), DataTable)
                        FillGrid2(0)
                    End If
                    If Not Session("App_Adressen") Is Nothing Then
                        'Adressdaten
                        m_Report.Adressen = CType(Session("App_Adressen"), DataTable)
                    End If
                    If Not Session("") Is Nothing Then
                        'Übermittlung
                        m_Report.LastChange = CType(Session("App_LastChange"), DataTable)
                        FillGrid(0)
                    End If
                    If Not Session("App_Equidaten") Is Nothing Then
                        'Übermittlung
                        m_Report.Equidaten = CType(Session("App_Equidaten"), DataTable)
                        FillEqui()
                    End If
                    ShowUebersicht()
                Else
                    lblError.Text = "Beim Laden der Seite ist ein Fehler aufgetreten."
                End If
            End If
        Catch ex As Exception
            lblError.Text = "Beim Laden der Seite ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
        End Try

    End Sub

    Protected Sub lb_Uebersicht_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lb_Uebersicht.Click
        ShowUebersicht()
    End Sub

    Protected Sub lb_Typdaten_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lb_Typdaten.Click
        HideAllTRs()
        trTypdaten.Visible = True
        lb_Typdaten.CssClass = "ButtonActive"
    End Sub

    Private Sub HideAllTRs()
        trUebersicht.Visible = False
        trUebermittlung.Visible = False
        trTypdaten.Visible = False
        trLebenslauf.Visible = False
        trZweit.Visible = False
        lb_Uebersicht.Enabled = True
        lb_Uebermittlung.Enabled = True
        lb_Typdaten.Enabled = True
        lb_Lebenslauf.Enabled = True
        lb_Schluessel.Enabled = True
        lb_Uebersicht.CssClass = "StandardButton"
        lb_Uebermittlung.CssClass = "StandardButton"
        lb_Typdaten.CssClass = "StandardButton"
        lb_Lebenslauf.CssClass = "StandardButton"
        lb_Schluessel.CssClass = "StandardButton"
    End Sub

    Private Sub ShowUebersicht()
        HideAllTRs()
        trUebersicht.Visible = True
        lb_Uebersicht.CssClass = "ButtonActive"

    End Sub

    Protected Sub lb_Lebenslauf_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lb_Lebenslauf.Click
        HideAllTRs()
        trLebenslauf.Visible = True
        lb_Lebenslauf.CssClass = "ButtonActive"
    End Sub

    Protected Sub lb_Uebermittlung_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lb_Uebermittlung.Click
        HideAllTRs()
        trUebermittlung.Visible = True
        lb_Uebermittlung.CssClass = "ButtonActive"
    End Sub
    Private Sub FillEqui()
        With m_Report.Equidaten


            For i = 0 To .Rows.Count - 1
                lblFahrgestellnummer.Text = .Rows(i)("CHASSIS_NUM").ToString

                if (.Rows(i)("EQTYP").ToString = "B") Then 
                    cbTreuhandsperre.Checked = (.Rows(i)("TREUHANDSPERRE_ENDG").ToString = "X")
                    lbTreugeber.Text = .Rows(i)("TREUGEBER").ToString
                End If 

                If .Rows(i)("EQTYP").ToString = "T" Then
                    If IsDBNull(.Rows(i)("ERDAT")) OrElse Not IsDate(.Rows(i)("ERDAT")) OrElse CType(.Rows(i)("ERDAT"), DateTime).Year < 1901 Then
                        lblEingangSchluessel.Text = ""
                    Else
                        lblEingangSchluessel.Text = CType(.Rows(i)("ERDAT"), DateTime).ToShortDateString()
                    End If
                    'Datum
                    If IsDBNull(.Rows(i)("ZZTMPDT")) OrElse Not IsDate(.Rows(i)("ZZTMPDT")) OrElse CType(.Rows(i)("ZZTMPDT"), DateTime).Year < 1901 Then
                        lblAngefordertAm0.Text = ""
                    Else
                        lblAngefordertAm0.Text = CType(.Rows(i)("ZZTMPDT"), DateTime).ToShortDateString()
                    End If
                    'Versandanschrift
                    lblVersandanschrift0.Text = .Rows(i)("NAME1").ToString
                    If .Rows(i)("NAME2").ToString.Length > 0 Then
                        ' lblVersandanschrift0.Text &= "<br>" & .Rows(i)("NAME2").ToString
                        lblVersandanschrift0.Text &= .Rows(i)("NAME2").ToString

                    End If
                    lblVersandanschrift0.Text &= "<br>"
                    If .Rows(i)("STREET").ToString.Length > 0 Then
                        lblVersandanschrift0.Text &= .Rows(i)("STREET").ToString
                    End If
                    If .Rows(i)("HOUSE_NUM1").ToString.Length > 0 Then
                        lblVersandanschrift0.Text &= " " & .Rows(i)("HOUSE_NUM1").ToString
                    End If
                    lblVersandanschrift0.Text &= "<br>"
                    If .Rows(i)("POST_CODE1").ToString.Length > 0 Then
                        lblVersandanschrift0.Text &= .Rows(i)("POST_CODE1").ToString
                    End If
                    If .Rows(i)("CITY1").ToString.Length > 0 Then
                        lblVersandanschrift0.Text &= " " & .Rows(i)("CITY1").ToString
                    End If

                End If
            Next
        End With
    End Sub
    Private Sub FillUebersicht()
        With m_Report.Grunddaten

            If .Rows.Count > 0 Then

                Try

                    cmdPrint.Visible = True
                    lnkCreateExcel.Visible = True
                    Eqinnum = .Rows(0)("EQUNR").ToString
                    lblKennzeichenShow.Text = .Rows(0)("ZZKENN").ToString
                    lblEhemaligesKennzeichenShow.Text = .Rows(0)("ZZKENN_OLD").ToString
                    lblBriefnummerShow.Text = .Rows(0)("ZZBRIEF").ToString
                    lblEhemaligeBriefnummerShow.Text = .Rows(0)("ZZBRIEF_OLD").ToString
                    If .Rows(0)("ZZBRIEF_A").ToString = "X" Then
                        chkBriefaufbietung.Checked = True
                    Else
                        chkBriefaufbietung.Checked = False
                    End If
                    lblFahrgestellnummerShow.Text = .Rows(0)("ZZFAHRG").ToString
                    lnkSchluesselinformationen.NavigateUrl &= lblFahrgestellnummerShow.Text
                    lblOrdernummerShow.Text = .Rows(0)("MVA_NUMMER").ToString
                    ' ITA 2761 Produktionskennziffer hinzugefügt CDI 20090609
                    lblProduktionskennzifferShow.Text = .Rows(0)("PROD_KENNZIFFER").ToString

                    If IsDBNull(.Rows(0)("DAT_SPERRE")) OrElse Not IsDate(.Rows(0)("DAT_SPERRE")) OrElse CType(.Rows(0)("DAT_SPERRE"), DateTime).Year < 1901 Then
                        lblSperrebis.Text = ""
                    Else
                        lblSperrebis.Text = CType(.Rows(0)("DAT_SPERRE"), DateTime).ToShortDateString()
                    End If

                    If IsDBNull(.Rows(0)("REPLA_DATE")) OrElse Not IsDate(.Rows(0)("REPLA_DATE")) OrElse CType(.Rows(0)("REPLA_DATE"), DateTime).Year < 1901 Then
                        lblErstzulassungsdatumShow.Text = ""
                    Else
                        lblErstzulassungsdatumShow.Text = CType(.Rows(0)("REPLA_DATE"), DateTime).ToShortDateString()
                    End If

                    If IsDBNull(.Rows(0)("ZZZLDAT")) OrElse Not IsDate(.Rows(0)("ZZZLDAT")) OrElse CType(.Rows(0)("ZZZLDAT"), DateTime).Year < 1901 Then
                        lblAktZulassungsdatumShow.Text = ""
                    Else
                        lblAktZulassungsdatumShow.Text = CType(.Rows(0)("ZZZLDAT"), DateTime).ToShortDateString()
                    End If

                    If IsDBNull(.Rows(0)("EXPIRY_DATE")) OrElse Not IsDate(.Rows(0)("EXPIRY_DATE")) OrElse CType(.Rows(0)("EXPIRY_DATE"), DateTime).Year < 1901 Then
                        lblAbmeldedatumShow.Text = ""
                    Else
                        lblAbmeldedatumShow.Text = CType(.Rows(0)("EXPIRY_DATE"), DateTime).ToShortDateString()
                    End If

                    If .Rows(0)("ZZSTATUS_ZUB").ToString = "X" Then
                        lblStatusShow.Text = "Zulassungsfähig"
                    End If
                    If .Rows(0)("ZZSTATUS_ZUL").ToString = "X" Then
                        lblStatusShow.Text = "Zugelassen"
                    End If
                    If .Rows(0)("ZZSTATUS_ABG").ToString = "X" Then
                        lblStatusShow.Text = "Abgemeldet"
                    End If
                    If .Rows(0)("ZZSTATUS_BAG").ToString = "X" Then
                        lblStatusShow.Text = "Bei Abmeldung"
                    End If
                    If .Rows(0)("ZZSTATUS_OZU").ToString = "X" Then
                        lblStatusShow.Text = "Ohne Zulassung"
                    End If
                    If .Rows(0)("ZZAKTSPERRE").ToString = "X" Then
                        lblStatusShow.Text = "Gesperrt"
                    End If

                    If .Rows(0)("ZZCOCKZ").ToString = "X" Then   'COC?
                        cbxCOC.Checked = True
                    End If
                    lblStandortShow.Text = .Rows(0)("NAME1_VS").ToString
                    If .Rows(0)("NAME2_VS").ToString.Length > 0 Then
                        lblStandortShow.Text &= "<br>" & .Rows(0)("NAME2_VS").ToString
                    End If
                    lblStandortShow.Text &= "<br>"
                    If .Rows(0)("STRAS_VS").ToString.Length > 0 Then
                        lblStandortShow.Text &= .Rows(0)("STRAS_VS").ToString
                    End If
                    If .Rows(0)("HSNR_VS").ToString.Length > 0 Then
                        lblStandortShow.Text &= " " & .Rows(0)("HSNR_VS").ToString
                    End If
                    lblStandortShow.Text &= "<br>"
                    If .Rows(0)("PSTLZ_VS").ToString.Length > 0 Then
                        lblStandortShow.Text &= .Rows(0)("PSTLZ_VS").ToString
                    End If
                    If .Rows(0)("ORT01_VS").ToString.Length > 0 Then
                        lblStandortShow.Text &= " " & .Rows(0)("ORT01_VS").ToString
                    End If
                    lblFahrzeughalterShow.Text = .Rows(0)("NAME1_ZH").ToString
                    If .Rows(0)("NAME2_ZH").ToString.Length > 0 Then
                        lblFahrzeughalterShow.Text &= "<br>" & .Rows(0)("NAME2_ZH").ToString
                    End If
                    lblFahrzeughalterShow.Text &= "<br>"
                    If .Rows(0)("STRAS_ZH").ToString.Length > 0 Then
                        lblFahrzeughalterShow.Text &= .Rows(0)("STRAS_ZH").ToString
                    End If
                    If .Rows(0)("HSNR_ZH").ToString.Length > 0 Then
                        lblFahrzeughalterShow.Text &= " " & .Rows(0)("HSNR_ZH").ToString
                    End If
                    lblFahrzeughalterShow.Text &= "<br>"
                    If .Rows(0)("PSTLZ_ZH").ToString.Length > 0 Then
                        lblFahrzeughalterShow.Text &= .Rows(0)("PSTLZ_ZH").ToString
                    End If
                    If .Rows(0)("ORT01_ZH").ToString.Length > 0 Then
                        lblFahrzeughalterShow.Text &= " " & .Rows(0)("ORT01_ZH").ToString
                    End If
                    Select Case .Rows(0)("ABCKZ").ToString
                        Case ""
                            lblLagerortShow.Text = "DAD"
                        Case "0"
                            lblLagerortShow.Text = "DAD"
                        Case "1"
                            lblLagerortShow.Text = "temporär versendet"
                        Case "2"
                            lblLagerortShow.Text = "endgültig versendet"
                    End Select

                    If IsDBNull(.Rows(0)("UDATE")) OrElse Not IsDate(.Rows(0)("UDATE")) OrElse CType(.Rows(0)("UDATE"), DateTime).Year < 1901 Then
                        lblUmgemeldetAmShow.Text = ""
                    Else
                        lblUmgemeldetAmShow.Text = CType(.Rows(0)("UDATE"), DateTime).ToShortDateString()
                    End If

                    If IsDBNull(.Rows(0)("ZZDAT_EIN")) OrElse Not IsDate(.Rows(0)("ZZDAT_EIN")) OrElse CType(.Rows(0)("ZZDAT_EIN"), DateTime).Year < 1901 Then
                        lblEingangsdatum.Text = ""
                    Else
                        lblEingangsdatum.Text = CType(.Rows(0)("ZZDAT_EIN"), DateTime).ToShortDateString()
                    End If

                    If IsDBNull(.Rows(0)("ZZDAT_BER")) OrElse Not IsDate(.Rows(0)("ZZDAT_BER")) OrElse CType(.Rows(0)("ZZDAT_BER"), DateTime).Year < 1901 Then
                        lblBereitdatum.Text = ""
                    Else
                        lblBereitdatum.Text = CType(.Rows(0)("ZZDAT_BER"), DateTime).ToShortDateString()
                    End If

                    lblPDI.Text = .Rows(0)("KUNPDI").ToString
                    lblPDIName.Text = " / " & .Rows(0)("DADPDI_NAME1").ToString

                    lblFahrzeugmodellShow.Text = .Rows(0)("TYP").ToString & " / " & .Rows(0)("ZZBEZEI").ToString

                    lblHerstellerShow.Text = .Rows(0)("MAKE_CODE").ToString
                    lblHerstellerSchluesselShow.Text = .Rows(0)("ZZHERSTELLER_SCH").ToString
                    lblTypschluesselShow.Text = .Rows(0)("ZZTYP_SCHL").ToString
                    lblVarianteVersionShow.Text = .Rows(0)("ZZVVS_SCHLUESSEL").ToString

                    If .Rows(0)("SCHILDER_PHY").ToString = "X" Then
                        chkVorhandeneElemente.Checked = True
                    Else
                        chkVorhandeneElemente.Checked = False
                    End If
                    If .Rows(0)("SCHEIN_PHY").ToString = "X" Then
                        chkFahzeugschein.Checked = True
                    Else
                        chkFahzeugschein.Checked = False
                    End If

                    If IsDBNull(.Rows(0)("CHECK_IN")) OrElse Not IsDate(.Rows(0)("CHECK_IN")) OrElse CType(.Rows(0)("CHECK_IN"), DateTime).Year < 1901 Then
                        lblCheckIn.Text = ""
                    Else
                        lblCheckIn.Text = CType(.Rows(0)("CHECK_IN"), DateTime).ToShortDateString()
                    End If

                    If IsDBNull(.Rows(0)("ZZTMPDT")) OrElse Not IsDate(.Rows(0)("ZZTMPDT")) OrElse CType(.Rows(0)("ZZTMPDT"), DateTime).Year < 1901 Then
                        lblVersendetAm.Text = ""
                    Else
                        lblVersendetAm.Text = CType(.Rows(0)("ZZTMPDT"), DateTime).ToShortDateString()
                        lblVersendetAm.Visible = True
                    End If

                Catch ex As Exception
                    lblError.Text = "Es ist ein Fehler aufgetreten. Die Informationen werden möglicherweise nicht korrekt angezeigt. " & ex.Message
                End Try
            Else
                lblError.Text = "Keine Grundaten gefunden!"
            End If

        End With
    End Sub
    Private Sub FillTypdaten()
        'Farben erstmal ausblenden
        lbl_55.Visible = False
        lbl_91.Visible = False
        lbl_92.Visible = False
        lbl_93.Visible = False
        lbl_94.Visible = False
        lbl_95.Visible = False
        lbl_96.Visible = False
        lbl_97.Visible = False
        lbl_98.Visible = False
        lbl_99.Visible = False

        lbl_155.Visible = False
        lbl_191.Visible = False
        lbl_192.Visible = False
        lbl_193.Visible = False
        lbl_194.Visible = False
        lbl_195.Visible = False
        lbl_196.Visible = False
        lbl_197.Visible = False
        lbl_198.Visible = False
        lbl_199.Visible = False

        With m_Report.Grunddaten

            If .Rows.Count > 0 Then

                'lbl_0.Text = .Rows(0)("ZZKLARTEXT_TYP").ToString
                lbl_1.Text = .Rows(0)("ZZHERST_TEXT").ToString
                lbl_2.Text = .Rows(0)("ZZHERSTELLER_SCH").ToString
                lbl_3.Text = .Rows(0)("ZZBEZEI").ToString
                'lbl_4.Text = .Rows(0)("ZZGENEHMIGNR").ToString
                'lbl_5.Text = .Rows(0)("ZZGENEHMIGDAT").ToString
                'lbl_6.Text = .Rows(0)("ZZFHRZKLASSE_TXT").ToString
                'lbl_7.Text = .Rows(0)("ZZTEXT_AUFBAU").ToString
                'lbl_8.Text = .Rows(0)("ZZFABRIKNAME").ToString
                'lbl_9.Text = .Rows(0)("ZZVARIANTE").ToString
                'lbl_10.Text = .Rows(0)("ZZVERSION").ToString
                'lbl_11.Text = .Rows(0)("ZZHUBRAUM").ToString.TrimStart("0"c)
                'lbl_13.Text = .Rows(0)("ENGINE_POWER").ToString.TrimStart("0"c)
                'lbl_14.Text = .Rows(0)("ZZBEIUMDREH").ToString.TrimStart("0"c)
                'lbl_12.Text = .Rows(0)("ZZHOECHSTGESCHW").ToString
                'lbl_19.Text = .Rows(0)("ZZSTANDGERAEUSCH").ToString.TrimStart("0"c)
                'lbl_20.Text = .Rows(0)("ZZFAHRGERAEUSCH").ToString.TrimStart("0"c)
                'lbl_15.Text = .Rows(0)("ZZKRAFTSTOFF_TXT").ToString
                'lbl_16.Text = .Rows(0)("ZZCODE_KRAFTSTOF").ToString
                'lbl_21.Text = .Rows(0)("ZZFASSVERMOEGEN").ToString
                'lbl_17.Text = .Rows(0)("ZZCO2KOMBI").ToString
                'lbl_18.Text = .Rows(0)("ZZSLD").ToString & " / " & .Rows(0)("ZZNATIONALE_EMIK").ToString
                'lbl_22.Text = .Rows(0)("ZZABGASRICHTL_TG").ToString
                'lbl_23.Text = .Rows(0)("ZZANZACHS").ToString.TrimStart("0"c)
                'lbl_24.Text = .Rows(0)("ZZANTRIEBSACHS").ToString.TrimStart("0"c)
                'lbl_26.Text = .Rows(0)("ZZANZSITZE").ToString.TrimStart("0"c)
                'lbl_25.Text = .Rows(0)("ZZACHSL_A1_STA").ToString.TrimStart("0"c) & ", " & .Rows(0)("ZZACHSL_A2_STA").ToString.TrimStart("0"c) & ", " & .Rows(0)("ZZACHSL_A3_STA").ToString.TrimStart("0"c)
                'If Right(lbl_25.Text, 2) = ", " Then
                '    lbl_25.Text = Left(lbl_25.Text, lbl_25.Text.Length - 2)
                'End If
                'lbl_27.Text = .Rows(0)("ZZBEREIFACHSE1").ToString & ", " & .Rows(0)("ZZBEREIFACHSE2").ToString & ", " & .Rows(0)("ZZBEREIFACHSE3").ToString
                'If Right(lbl_27.Text, 2) = ", " Then
                '    lbl_27.Text = Left(lbl_27.Text, lbl_27.Text.Length - 2)
                'End If

                'lbl_28.Text = .Rows(0)("ZZZULGESGEW").ToString.TrimStart("0"c)
                lbl_29.Text = .Rows(0)("ZZTYP_SCHL").ToString
                'lbl_30.Text = .Rows(0)("ZZBEMER1").ToString & _
                '"<br>" & .Rows(0)("ZZBEMER2").ToString & _
                '"<br>" & .Rows(0)("ZZBEMER3").ToString & _
                '"<br>" & .Rows(0)("ZZBEMER4").ToString & _
                '"<br>" & .Rows(0)("ZZBEMER5").ToString & _
                '"<br>" & .Rows(0)("ZZBEMER6").ToString & _
                '"<br>" & .Rows(0)("ZZBEMER7").ToString & _
                '"<br>" & .Rows(0)("ZZBEMER8").ToString & _
                '"<br>" & .Rows(0)("ZZBEMER9").ToString & _
                '"<br>" & .Rows(0)("ZZBEMER10").ToString & _
                '"<br>" & .Rows(0)("ZZBEMER11").ToString & _
                '"<br>" & .Rows(0)("ZZBEMER12").ToString & _
                '"<br>" & .Rows(0)("ZZBEMER13").ToString & _
                '"<br>" & .Rows(0)("ZZBEMER14").ToString

                'lbl_31.Text = .Rows(0)("ZZLAENGEMIN").ToString.TrimStart("0"c)
                'lbl_32.Text = .Rows(0)("ZZBREITEMIN").ToString.TrimStart("0"c)
                'lbl_33.Text = .Rows(0)("ZZHOEHEMIN").ToString

                lbl_00.Text = .Rows(0)("ZFARBE").ToString '& " (" & .Farbziffer & ")"
                lbl_55.Text = "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;"
                lbl_91.Text = "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;"
                lbl_92.Text = "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;"
                lbl_93.Text = "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;"
                lbl_94.Text = "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;"
                lbl_95.Text = "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;"
                lbl_96.Text = "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;"
                lbl_97.Text = "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;"
                lbl_98.Text = "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;"
                lbl_99.Text = "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;"

                lbl_200.Text = .Rows(0)("ZFARBE").ToString '& " (" & .Farbziffer & ")"
                lbl_155.Text = "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;"
                lbl_191.Text = "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;"
                lbl_192.Text = "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;"
                lbl_193.Text = "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;"
                lbl_194.Text = "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;"
                lbl_195.Text = "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;"
                lbl_196.Text = "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;"
                lbl_197.Text = "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;"
                lbl_198.Text = "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;"
                lbl_199.Text = "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;"

                Select Case CInt(.Rows(0)("ZFARBE"))
                    Case 0
                        lbl_99.Visible = True
                        lbl_199.Visible = True
                    Case 1
                        lbl_98.Visible = True
                        lbl_198.Visible = True
                    Case 2
                        lbl_97.Visible = True
                        lbl_197.Visible = True
                    Case 3
                        lbl_96.Visible = True
                        lbl_196.Visible = True
                    Case 4
                        lbl_95.Visible = True
                        lbl_195.Visible = True
                    Case 5
                        lbl_94.Visible = True
                        lbl_194.Visible = True
                    Case 6
                        lbl_93.Visible = True
                        lbl_193.Visible = True
                    Case 7
                        lbl_92.Visible = True
                        lbl_192.Visible = True
                    Case 8
                        lbl_91.Visible = True
                        lbl_191.Visible = True
                    Case 9
                        lbl_55.Visible = True
                        lbl_155.Visible = True
                    Case Else

                End Select
            End If
        End With

    End Sub
    Private Sub TypdatenFill()

        objPDIs = New Base.Business.ABEDaten(m_User, m_App, Session("AppID").ToString, Session.SessionID.ToString, "", m_User.KUNNR, "ZDAD", "", "")
        objPDIs.FillDatenABE(Session("AppID").ToString, Session.SessionID.ToString, Me.Page, Eqinnum)
        Session("App_objPDIs") = objPDIs
        lbl_55.Visible = False
        lbl_91.Visible = False
        lbl_92.Visible = False
        lbl_93.Visible = False
        lbl_94.Visible = False
        lbl_95.Visible = False
        lbl_96.Visible = False
        lbl_97.Visible = False
        lbl_98.Visible = False
        lbl_99.Visible = False

        lbl_155.Visible = False
        lbl_191.Visible = False
        lbl_192.Visible = False
        lbl_193.Visible = False
        lbl_194.Visible = False
        lbl_195.Visible = False
        lbl_196.Visible = False
        lbl_197.Visible = False
        lbl_198.Visible = False
        lbl_199.Visible = False

        If objPDIs.Status = 0 Then
            With objPDIs.ABE_Daten
                'lbl_00.Text = .Farbziffer
                lbl_0.Text = .ZZKLARTEXT_TYP
                lbl_1.Text = .ZZHERST_TEXT
                lbl_2.Text = .ZZHERSTELLER_SCH
                lbl_3.Text = .ZZHANDELSNAME
                lbl_4.Text = .ZZGENEHMIGNR
                lbl_5.Text = .ZZGENEHMIGDAT
                lbl_6.Text = .ZZFHRZKLASSE_TXT
                lbl_7.Text = .ZZTEXT_AUFBAU
                lbl_8.Text = .ZZFABRIKNAME
                lbl_9.Text = .ZZVARIANTE
                lbl_10.Text = .ZZVERSION
                lbl_11.Text = .ZZHUBRAUM.TrimStart("0"c)
                lbl_13.Text = .ZZNENNLEISTUNG.TrimStart("0"c)
                lbl_14.Text = .ZZBEIUMDREH.TrimStart("0"c)
                lbl_12.Text = .ZZHOECHSTGESCHW
                lbl_19.Text = .ZZSTANDGERAEUSCH.TrimStart("0"c)
                lbl_20.Text = .ZZFAHRGERAEUSCH.TrimStart("0"c)
                lbl_15.Text = .ZZKRAFTSTOFF_TXT
                lbl_16.Text = .ZZCODE_KRAFTSTOF
                lbl_21.Text = .ZZFASSVERMOEGEN
                lbl_17.Text = .ZZCO2KOMBI
                lbl_18.Text = .ZZSLD & " / " & .ZZNATIONALE_EMIK
                lbl_22.Text = .ZZABGASRICHTL_TG
                lbl_23.Text = .ZZANZACHS.TrimStart("0"c)
                lbl_24.Text = .ZZANTRIEBSACHS.TrimStart("0"c)
                lbl_26.Text = .ZZANZSITZE.TrimStart("0"c)
                lbl_25.Text = .ZZACHSL_A1_STA.TrimStart("0"c) & ", " & .ZZACHSL_A2_STA.TrimStart("0"c) & ", " & .ZZACHSL_A3_STA.TrimStart("0"c)
                If Right(lbl_25.Text, 2) = ", " Then
                    lbl_25.Text = Left(lbl_25.Text, lbl_25.Text.Length - 2)
                End If
                lbl_27.Text = .ZZBEREIFACHSE1 & ", " & .ZZBEREIFACHSE2 & ", " & .ZZBEREIFACHSE3
                If Right(lbl_27.Text, 2) = ", " Then
                    lbl_27.Text = Left(lbl_27.Text, lbl_27.Text.Length - 2)
                End If

                lbl_28.Text = .ZZZULGESGEW.TrimStart("0"c)
                lbl_29.Text = .ZZTYP_SCHL
                lbl_30.Text = .ZZBEMER1 & "<br>" & .ZZBEMER2 & "<br>" & .ZZBEMER3 & "<br>" & .ZZBEMER4 & "<br>" & .ZZBEMER5 & "<br>" & .ZZBEMER6 & "<br>" & .ZZBEMER7 & "<br>" & .ZZBEMER8 & "<br>" & .ZZBEMER9 & "<br>" & .ZZBEMER10 & "<br>" & .ZZBEMER11 & "<br>" & .ZZBEMER12 & "<br>" & .ZZBEMER13 & "<br>" & .ZZBEMER14
                lbl_31.Text = .ZZLAENGEMIN.TrimStart("0"c)
                lbl_32.Text = .ZZBREITEMIN.TrimStart("0"c)
                lbl_33.Text = .ZZHOEHEMIN

                lbl_00.Text = .ZZFARBE & " (" & .Farbziffer & ")"
                lbl_200.Text = .ZZFARBE & " (" & .Farbziffer & ")"
                lbl_55.Text = "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;"
                lbl_91.Text = "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;"
                lbl_92.Text = "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;"
                lbl_93.Text = "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;"
                lbl_94.Text = "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;"
                lbl_95.Text = "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;"
                lbl_96.Text = "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;"
                lbl_97.Text = "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;"
                lbl_98.Text = "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;"
                lbl_99.Text = "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;"
                lbl_200.Text = .ZZFARBE & " (" & .Farbziffer & ")"
                lbl_155.Text = "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;"
                lbl_191.Text = "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;"
                lbl_192.Text = "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;"
                lbl_193.Text = "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;"
                lbl_194.Text = "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;"
                lbl_195.Text = "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;"
                lbl_196.Text = "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;"
                lbl_197.Text = "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;"
                lbl_198.Text = "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;"
                lbl_199.Text = "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;"

                If IsNumeric(.Farbziffer) Then
                    Select Case CInt(.Farbziffer)
                        Case 0
                            lbl_99.Visible = True
                            lbl_199.Visible = True
                        Case 1
                            lbl_98.Visible = True
                            lbl_198.Visible = True
                        Case 2
                            lbl_97.Visible = True
                            lbl_197.Visible = True
                        Case 3
                            lbl_96.Visible = True
                            lbl_196.Visible = True
                        Case 4
                            lbl_95.Visible = True
                            lbl_195.Visible = True
                        Case 5
                            lbl_94.Visible = True
                            lbl_194.Visible = True
                        Case 6
                            lbl_93.Visible = True
                            lbl_193.Visible = True
                        Case 7
                            lbl_92.Visible = True
                            lbl_192.Visible = True
                        Case 8
                            lbl_91.Visible = True
                            lbl_191.Visible = True
                        Case 9
                            lbl_55.Visible = True
                            lbl_155.Visible = True

                    End Select
                End If



            End With
        End If
    End Sub
    Private Sub FillGrid(ByVal intPageIndex As Int32, Optional ByVal strSort As String = "")

        If Not m_Report.Equidaten Is Nothing Then

            If m_Report.Equidaten.Rows.Count > 0 Then
                Dim tmpDataView As New DataView()
                tmpDataView = m_Report.Equidaten.DefaultView

                Dim intTempPageIndex As Int32 = intPageIndex
                Dim strTempSort As String = ""
                Dim strDirection As String = ""

                If strSort.Trim(" "c).Length > 0 Then
                    intTempPageIndex = 0
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

                DataGrid1.CurrentPageIndex = intTempPageIndex
                DataGrid1.DataSource = tmpDataView

                DataGrid1.DataBind()

                If DataGrid1.PageCount > 1 Then
                    DataGrid1.PagerStyle.CssClass = "PagerStyle"
                    DataGrid1.DataBind()
                    DataGrid1.PagerStyle.Visible = True
                Else
                    DataGrid1.PagerStyle.Visible = False
                End If
            End If

        End If
    End Sub

    Private Sub FillGrid2(ByVal intPageIndex As Int32, Optional ByVal strSort As String = "")
        If Not m_Report.Historie Is Nothing Then


            If m_Report.Historie.Rows.Count > 0 Then
                Dim tmpDataView As New DataView()
                tmpDataView = m_Report.Historie.DefaultView

                Dim intTempPageIndex As Int32 = intPageIndex
                Dim strTempSort As String = ""
                Dim strDirection As String = ""

                If strSort.Trim(" "c).Length > 0 Then
                    intTempPageIndex = 0
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

                Datagrid2.CurrentPageIndex = intTempPageIndex
                Datagrid2.DataSource = tmpDataView

                Datagrid2.DataBind()

                If Datagrid2.PageCount > 1 Then
                    Datagrid2.PagerStyle.CssClass = "PagerStyle"
                    Datagrid2.DataBind()
                    Datagrid2.PagerStyle.Visible = True
                Else
                    Datagrid2.PagerStyle.Visible = False
                End If

                Dim item As DataGridItem
                Dim cell As TableCell
                Dim control As Control
                Dim label As Label
                Dim literal As Literal
                Dim text As String

                For Each item In Datagrid2.Items
                    cell = item.Cells(2)

                    text = ""
                    For Each control In cell.Controls
                        If TypeOf control Is Label Then
                            If control.ID = "Label1" Or control.ID = "Label2" Or control.ID = "Label3" Then
                                label = CType(control, Label)
                                text &= label.Text
                            End If
                        End If
                    Next
                    If text.Trim(" "c).Length = 0 Then
                        For Each control In cell.Controls
                            If TypeOf control Is Literal Then
                                literal = CType(control, Literal)
                                literal.Text = ""
                            End If
                        Next
                    End If
                Next

            End If
        End If
    End Sub

    Protected Sub cmdBack_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdBack.Click
        Session("App_Historie") = Nothing
        Session("App_Adressen") = Nothing
        Session("App_LastChange") = Nothing
        Session("App_Equidaten") = Nothing
        Session("App_Report") = Nothing
        Response.Redirect("Report07.aspx?AppID=" & Session("AppID").ToString)
    End Sub
    Private Sub Print()
        Try
            Dim imageHt As New Hashtable()
            Dim mReport As Fahrzeughistorie
            mReport = CType(Session("App_Report"), Fahrzeughistorie)

            imageHt.Add("Logo", m_User.Customer.LogoImage)
            If objPDIs Is Nothing Then
                objPDIs = CType(Session("App_objPDIs"), Base.Business.ABEDaten)
            End If

            Dim tblData As DataTable = createUebersichtTable()

            Dim docFactory As New Base.Kernel.DocumentGeneration.WordDocumentFactory(tblData, imageHt)

            Dim dataTables(1) As DataTable

            dataTables(0) = createLebenslaufTable()
            dataTables(1) = createUebermittlungsTable()
            docFactory.CreateDocument("Fahrzeughistorie_" & lblFahrgestellnummerShow.Text, Me.Page, "\Applications\AppAvis\Documents\FahrzeughistoriePrint.doc", "Tabelle", dataTables)
        Catch ex As Exception
            lblError.Text = "Fehler beim Erstellen des Ausdrucks: " + ex.Message
        End Try
    End Sub
    Protected Sub cmdPrint_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdPrint.Click
        Print()
    End Sub
    Private Function createUebersichtTable() As DataTable

        Dim Uebersicht As New DataTable
        'Dim tmpRow As DataRow

        Uebersicht.Columns.Add("Fahrgestellnummer", System.Type.GetType("System.String"))
        Uebersicht.Columns.Add("Kennzeichen", System.Type.GetType("System.String"))
        Uebersicht.Columns.Add("Status", System.Type.GetType("System.String"))
        Uebersicht.Columns.Add("Lagerort", System.Type.GetType("System.String"))

        Uebersicht.Columns.Add("Hersteller", System.Type.GetType("System.String"))
        Uebersicht.Columns.Add("Fahrzeugmodell", System.Type.GetType("System.String"))
        Uebersicht.Columns.Add("Farbe", System.Type.GetType("System.String"))
        Uebersicht.Columns.Add("Herstellerschluessel", System.Type.GetType("System.String"))
        Uebersicht.Columns.Add("Typschluessel", System.Type.GetType("System.String"))
        Uebersicht.Columns.Add("VarianteVersion", System.Type.GetType("System.String"))

        Uebersicht.Columns.Add("Eingang", System.Type.GetType("System.String"))
        Uebersicht.Columns.Add("Carport", System.Type.GetType("System.String"))
        Uebersicht.Columns.Add("Bereitdatum", System.Type.GetType("System.String"))
        Uebersicht.Columns.Add("ZBIINummer", System.Type.GetType("System.String"))

        Uebersicht.Columns.Add("Erstzulassungsdatum", System.Type.GetType("System.String"))
        Uebersicht.Columns.Add("AktZulassung", System.Type.GetType("System.String"))

        Uebersicht.Columns.Add("Abmeldedatum", System.Type.GetType("System.String"))
        Uebersicht.Columns.Add("Mva_Nummer", System.Type.GetType("System.String"))

        Uebersicht.Columns.Add("FahrzeughalterName1Name2", System.Type.GetType("System.String"))
        Uebersicht.Columns.Add("FahrzeughalterStrasseNummer", System.Type.GetType("System.String"))
        Uebersicht.Columns.Add("FahrzeughalterPLZOrt", System.Type.GetType("System.String"))
        Uebersicht.Columns.Add("VersandadresseName1Name2", System.Type.GetType("System.String"))
        Uebersicht.Columns.Add("VersandadresseStrasseNummer", System.Type.GetType("System.String"))
        Uebersicht.Columns.Add("VersandadressePLZOrt", System.Type.GetType("System.String"))
        Uebersicht.Columns.Add("COC", System.Type.GetType("System.String"))
        ' ITA 2761 Produktionskennziffer hinzugefügt CDI 20090610
        Uebersicht.Columns.Add("Produktionskennziffer", System.Type.GetType("System.String"))

        Uebersicht.Columns.Add("AusdruckDatumUhrzeit", System.Type.GetType("System.String"))
        Uebersicht.Columns.Add("Username", System.Type.GetType("System.String"))
        Uebersicht.Columns.Add("UmgemeldetAm", System.Type.GetType("System.String"))
        Uebersicht.Columns.Add("EhmaligesKennzeichen", System.Type.GetType("System.String"))
        Uebersicht.Columns.Add("ZBIIAufbietung", System.Type.GetType("System.String"))
        Uebersicht.Columns.Add("EhmaligeZBIINummer", System.Type.GetType("System.String"))
        Uebersicht.Columns.Add("EingangSchluessel", System.Type.GetType("System.String"))
        Uebersicht.Columns.Add("Versendet", System.Type.GetType("System.String"))
        Uebersicht.Columns.Add("SchluesselName1Name2", System.Type.GetType("System.String"))
        Uebersicht.Columns.Add("SchluesselStrasseNummer", System.Type.GetType("System.String"))
        Uebersicht.Columns.Add("SchluesselPLZOrt", System.Type.GetType("System.String"))

        Dim tmpRow As DataRow = Uebersicht.NewRow

        tmpRow("Fahrgestellnummer") = lblFahrgestellnummerShow.Text
        tmpRow("Abmeldedatum") = lblAbmeldedatumShow.Text
        tmpRow("Status") = lblStatusShow.Text
        tmpRow("Kennzeichen") = lblKennzeichenShow.Text
        tmpRow("Lagerort") = lblLagerortShow.Text
        tmpRow("Hersteller") = lblHerstellerShow.Text
        tmpRow("Fahrzeugmodell") = lblFahrzeugmodellShow.Text
        tmpRow("Farbe") = objPDIs.ABE_Daten.ZZFARBE
        tmpRow("Herstellerschluessel") = lblHerstellerSchluesselShow.Text
        tmpRow("Typschluessel") = lblTypschluesselShow.Text
        tmpRow("VarianteVersion") = lblVarianteVersionShow.Text
        tmpRow("ZBIINummer") = lblBriefnummerShow.Text
        tmpRow("Erstzulassungsdatum") = lblErstzulassungsdatumShow.Text
        tmpRow("AktZulassung") = lblAktZulassungsdatumShow.Text
        tmpRow("Abmeldedatum") = lblAbmeldedatumShow.Text
        ' ITA 2761 Produktionskennziffer hinzugefügt CDI 20090610
        tmpRow("Produktionskennziffer") = lblProduktionskennzifferShow.Text

        tmpRow("Carport") = lblPDI.Text & " / " & lblPDIName.Text
        tmpRow("Mva_Nummer") = lblOrdernummerShow.Text
        'traurig aber wahr, nach string "<br>" splittet er nicht richtig JJU2008.07.01
        tmpRow("FahrzeughalterName1Name2") = lblFahrzeughalterShow.Text.Replace("<br>", ";").Split(CChar(CStr(";")))(0)
        tmpRow("FahrzeughalterStrasseNummer") = lblFahrzeughalterShow.Text.Replace("<br>", ";").Split(CChar(CStr(";")))(1)
        tmpRow("FahrzeughalterPLZOrt") = lblFahrzeughalterShow.Text.Replace("<br>", ";").Split(CChar(CStr(";")))(2)
        tmpRow("VersandadresseName1Name2") = lblStandortShow.Text.Replace("<br>", ";").Split(CChar(CStr(";")))(0)
        tmpRow("VersandadresseStrasseNummer") = lblStandortShow.Text.Replace("<br>", ";").Split(CChar(CStr(";")))(1)
        tmpRow("VersandadressePLZOrt") = lblStandortShow.Text.Replace("<br>", ";").Split(CChar(CStr(";")))(2)
        tmpRow("AusdruckDatumUhrzeit") = Date.Now.ToShortDateString & " / " & Date.Now.ToShortTimeString
        tmpRow("Username") = m_User.UserName
        tmpRow("Eingang") = lblEingangsdatum.Text
        tmpRow("Bereitdatum") = lblBereitdatum.Text
        tmpRow("EingangSchluessel") = lblEingangSchluessel.Text
        tmpRow("Versendet") = lblAngefordertAm0.Text


        If Not lblVersandanschrift0.Text Is "" Then
            tmpRow("SchluesselName1Name2") = lblVersandanschrift0.Text.Replace("<br>", ";").Split(";"c)(0)
            tmpRow("SchluesselStrasseNummer") = lblVersandanschrift0.Text.Replace("<br>", ";").Split(";"c)(1)
            tmpRow("SchluesselPLZOrt") = lblVersandanschrift0.Text.Replace("<br>", ";").Split(";"c)(2)
        End If

        If cbxCOC.Checked = True Then
            tmpRow("COC") = "Ja"
        Else
            tmpRow("COC") = "Nein"
        End If
        tmpRow("UmgemeldetAM") = lblUmgemeldetAmShow.Text
        tmpRow("EhmaligesKennzeichen") = lblEhemaligesKennzeichenShow.Text
        If chkBriefaufbietung.Checked = True Then
            tmpRow("ZBIIAufbietung") = "Ja"
        Else
            tmpRow("ZBIIAufbietung") = "Nein"
        End If
        tmpRow("EhmaligeZBIINummer") = lblEhemaligeBriefnummerShow.Text

        Uebersicht.Rows.Add(tmpRow)
        Return Uebersicht

    End Function
    Private Function createLebenslaufTable() As DataTable
        Dim tmpTable As DataTable = CType(Session("App_Historie"), DataTable)
        Dim Lebenslauf As New DataTable
        'Dim tmpRow As DataRow

        Lebenslauf.Columns.Add("Vorgang", System.Type.GetType("System.String"))
        Lebenslauf.Columns.Add("Durchführungsdatum", System.Type.GetType("System.String"))
        Lebenslauf.Columns.Add("Versandadresse", System.Type.GetType("System.String"))
        Lebenslauf.Columns.Add("Versandart", System.Type.GetType("System.String"))
        Lebenslauf.Columns.Add("Beauftragt durch", System.Type.GetType("System.String"))

        For Each tmpRow As DataRow In tmpTable.Rows

            Dim tmpLebenslaufRow As DataRow = Lebenslauf.NewRow()

            tmpLebenslaufRow.Item("Vorgang") = tmpRow("KURZTEXT").ToString
            tmpLebenslaufRow.Item("Durchführungsdatum") = tmpRow("STRMN").ToString
            tmpLebenslaufRow.Item("Versandadresse") = tmpRow("NAME1_Z5").ToString & " " & tmpRow("NAME2_Z5").ToString & " / " & tmpRow("STREET_Z5").ToString & " " & tmpRow("HOUSE_NUM1_Z5").ToString & " / " & tmpRow("POST_CODE1_Z5").ToString & " " & tmpRow("CITY1_Z5").ToString
            tmpLebenslaufRow.Item("Versandart") = tmpRow("ZZDIEN1").ToString
            tmpLebenslaufRow.Item("Beauftragt durch") = tmpRow("ERNAM").ToString

            Lebenslauf.Rows.Add(tmpLebenslaufRow)
            Lebenslauf.AcceptChanges()

        Next
        Lebenslauf.TableName = "Lebenslauf"
        Return Lebenslauf


    End Function

    Private Function createUebermittlungsTable() As DataTable
        Dim tmpTable As DataTable = CType(Session("App_LastChange"), DataTable)
        Dim Uebersicht As New DataTable
        'Dim tmpRow As DataRow

        Uebersicht.Columns.Add("Aktionscode", System.Type.GetType("System.String"))
        Uebersicht.Columns.Add("Vorgang", System.Type.GetType("System.String"))
        Uebersicht.Columns.Add("Statusdatum", System.Type.GetType("System.String"))
        Uebersicht.Columns.Add("Übermittlungsdatum", System.Type.GetType("System.String"))

        For Each tmpRow As DataRow In tmpTable.Rows

            Dim tmpUebersichtsRow As DataRow = Uebersicht.NewRow()

            tmpUebersichtsRow.Item("Aktionscode") = tmpRow("MNCOD").ToString
            tmpUebersichtsRow.Item("Vorgang") = tmpRow("MATXT").ToString
            tmpUebersichtsRow.Item("Statusdatum") = tmpRow("PSTER").ToString
            tmpUebersichtsRow.Item("Übermittlungsdatum") = tmpRow("ZZUEBER").ToString


            Uebersicht.Rows.Add(tmpUebersichtsRow)
            Uebersicht.AcceptChanges()

        Next
        Uebersicht.TableName = "Übermittlung"
        Return Uebersicht


    End Function
    Protected Sub lnkCreateExcel_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles lnkCreateExcel.Click
        Print()
    End Sub
    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        SetEndASPXAccess(Me)
    End Sub

    Private Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Unload
        SetEndASPXAccess(Me)
    End Sub

    Protected Sub lb_Schluessel_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lb_Schluessel.Click
        HideAllTRs()
        trZweit.Visible = True
        lb_Schluessel.CssClass = "ButtonActive"
    End Sub
End Class
' ************************************************
' $History: Report07_2.aspx.vb $
' 
' *****************  Version 10  *****************
' User: Fassbenders  Date: 29.10.10   Time: 9:45
' Updated in $/CKAG/Applications/AppAvis/forms
' 
' *****************  Version 9  *****************
' User: Fassbenders  Date: 10.03.10   Time: 16:45
' Updated in $/CKAG/Applications/AppAvis/forms
' 
' *****************  Version 8  *****************
' User: Dittbernerc  Date: 10.06.09   Time: 11:51
' Updated in $/CKAG/Applications/AppAvis/forms
' ITA 2761 Fertig zum Test
' 
' *****************  Version 7  *****************
' User: Dittbernerc  Date: 9.06.09    Time: 17:17
' Updated in $/CKAG/Applications/AppAvis/forms
' 
' *****************  Version 6  *****************
' User: Rudolpho     Date: 28.04.09   Time: 17:09
' Updated in $/CKAG/Applications/AppAvis/forms
' 
' *****************  Version 5  *****************
' User: Rudolpho     Date: 2.02.09    Time: 13:23
' Updated in $/CKAG/Applications/AppAvis/forms
' 
' *****************  Version 4  *****************
' User: Rudolpho     Date: 2.02.09    Time: 11:37
' Updated in $/CKAG/Applications/AppAvis/forms
' ITA: 2389
' 
' *****************  Version 3  *****************
' User: Rudolpho     Date: 30.01.09   Time: 14:00
' Updated in $/CKAG/Applications/AppAvis/forms
' ITA: 2389
' 
' *****************  Version 2  *****************
' User: Rudolpho     Date: 5.01.09    Time: 17:07
' Updated in $/CKAG/Applications/AppAvis/forms
' ITA: 2389
' 
' ************************************************
