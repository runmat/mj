Imports CKG.Base.Business
Imports CKG.Base.Kernel
Imports CKG.Portal.PageElements
Imports CKG.Base.Kernel.Common.Common

Public Class Report46_2
    Inherits System.Web.UI.Page

#Region " Vom Web Form Designer generierter Code "

    'Dieser Aufruf ist für den Web Form-Designer erforderlich.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: Diese Methode ist für den Web Form-Designer erforderlich
        'Verwenden Sie nicht den Code-Editor zur Bearbeitung.
        InitializeComponent()
    End Sub

#End Region

    Private m_App As Base.Kernel.Security.App
    Private m_User As Base.Kernel.Security.User
    Private BRIEFLEBENSLAUF_LPTable As DataTable
    Private QMMIDATENTable As DataTable
    Private QMEL_DATENTable As DataTable
    Private objPDIs As Base.Business.ABEDaten
    Private mAddressdaten As DataTable



    Protected WithEvents lblError As System.Web.UI.WebControls.Label
    'Protected WithEvents Hyperlink2 As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lnkKreditlimit As System.Web.UI.WebControls.HyperLink
    Protected WithEvents ucHeader As Header
    Protected WithEvents lnkSchluesselinformationen As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lblPageTitle As System.Web.UI.WebControls.Label
    Protected WithEvents lblHead As System.Web.UI.WebControls.Label
    Protected WithEvents cbxCOC As System.Web.UI.WebControls.CheckBox
    Protected WithEvents trUebersicht As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents trTypdaten As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents trLebenslauf As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents trUebermittlung As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents trPartnerdaten As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents chkBriefaufbietung As System.Web.UI.WebControls.CheckBox
    Protected WithEvents lbError As System.Web.UI.WebControls.Label
    Protected WithEvents lbl_30 As System.Web.UI.WebControls.Label
    Protected WithEvents lbl_27 As System.Web.UI.WebControls.Label
    Protected WithEvents lbl_25 As System.Web.UI.WebControls.Label
    Protected WithEvents lbl_24 As System.Web.UI.WebControls.Label
    Protected WithEvents lbl_23 As System.Web.UI.WebControls.Label
    Protected WithEvents lbl_22 As System.Web.UI.WebControls.Label
    Protected WithEvents lbl_18 As System.Web.UI.WebControls.Label
    Protected WithEvents lbl_17 As System.Web.UI.WebControls.Label
    Protected WithEvents lbl_21 As System.Web.UI.WebControls.Label
    Protected WithEvents lbl_16 As System.Web.UI.WebControls.Label
    Protected WithEvents lbl_15 As System.Web.UI.WebControls.Label
    Protected WithEvents lbl_20 As System.Web.UI.WebControls.Label
    Protected WithEvents lbl_19 As System.Web.UI.WebControls.Label
    Protected WithEvents lbl_12 As System.Web.UI.WebControls.Label
    Protected WithEvents lbl_14 As System.Web.UI.WebControls.Label
    Protected WithEvents lbl_13 As System.Web.UI.WebControls.Label
    Protected WithEvents lbl_11 As System.Web.UI.WebControls.Label
    Protected WithEvents lbl_33 As System.Web.UI.WebControls.Label
    Protected WithEvents lbl_32 As System.Web.UI.WebControls.Label
    Protected WithEvents lbl_31 As System.Web.UI.WebControls.Label
    Protected WithEvents lbl_28 As System.Web.UI.WebControls.Label
    Protected WithEvents lbl_26 As System.Web.UI.WebControls.Label
    Protected WithEvents lbl_10 As System.Web.UI.WebControls.Label
    Protected WithEvents lbl_9 As System.Web.UI.WebControls.Label
    Protected WithEvents lbl_8 As System.Web.UI.WebControls.Label
    Protected WithEvents lbl_29 As System.Web.UI.WebControls.Label
    Protected WithEvents lbl_0 As System.Web.UI.WebControls.Label
    Protected WithEvents lbl_4 As System.Web.UI.WebControls.Label
    Protected WithEvents lbl_5 As System.Web.UI.WebControls.Label
    Protected WithEvents lbl_00 As System.Web.UI.WebControls.Label
    Protected WithEvents lbl_99 As System.Web.UI.WebControls.Label
    Protected WithEvents lbl_98 As System.Web.UI.WebControls.Label
    Protected WithEvents lbl_97 As System.Web.UI.WebControls.Label
    Protected WithEvents lbl_96 As System.Web.UI.WebControls.Label
    Protected WithEvents lbl_95 As System.Web.UI.WebControls.Label
    Protected WithEvents lbl_94 As System.Web.UI.WebControls.Label
    Protected WithEvents lbl_93 As System.Web.UI.WebControls.Label
    Protected WithEvents lbl_92 As System.Web.UI.WebControls.Label
    Protected WithEvents lbl_91 As System.Web.UI.WebControls.Label
    Protected WithEvents lbl_55 As System.Web.UI.WebControls.Label
    Protected WithEvents lbl_3 As System.Web.UI.WebControls.Label
    Protected WithEvents lbl_2 As System.Web.UI.WebControls.Label
    Protected WithEvents lbl_1 As System.Web.UI.WebControls.Label
    Protected WithEvents lbl_7 As System.Web.UI.WebControls.Label
    Protected WithEvents lbl_6 As System.Web.UI.WebControls.Label
    Protected WithEvents DataGrid1 As System.Web.UI.WebControls.DataGrid
    Protected WithEvents Datagrid2 As System.Web.UI.WebControls.DataGrid
    Protected WithEvents lbl_155 As System.Web.UI.WebControls.Label
    Protected WithEvents lbl_191 As System.Web.UI.WebControls.Label
    Protected WithEvents lbl_192 As System.Web.UI.WebControls.Label
    Protected WithEvents lbl_193 As System.Web.UI.WebControls.Label
    Protected WithEvents lbl_194 As System.Web.UI.WebControls.Label
    Protected WithEvents lbl_195 As System.Web.UI.WebControls.Label
    Protected WithEvents lbl_196 As System.Web.UI.WebControls.Label
    Protected WithEvents lbl_197 As System.Web.UI.WebControls.Label
    Protected WithEvents lbl_198 As System.Web.UI.WebControls.Label
    Protected WithEvents lbl_199 As System.Web.UI.WebControls.Label
    Protected WithEvents lbl_200 As System.Web.UI.WebControls.Label
    Protected WithEvents lbl_Fahrzeugdaten As System.Web.UI.WebControls.Label
    Protected WithEvents lblFahrgestellnummerShow As System.Web.UI.WebControls.Label
    Protected WithEvents lbl_Fahrgestellnummer As System.Web.UI.WebControls.Label
    Protected WithEvents tr_Fahrgestellnummer As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents lblKennzeichenShow As System.Web.UI.WebControls.Label
    Protected WithEvents lblStatusShow As System.Web.UI.WebControls.Label
    Protected WithEvents lbl_Kennzeichen As System.Web.UI.WebControls.Label
    Protected WithEvents tr_Kennzeichen As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents lblLagerortShow As System.Web.UI.WebControls.Label
    Protected WithEvents lbl_Status As System.Web.UI.WebControls.Label
    Protected WithEvents tr_Status As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents lblHerstellerShow As System.Web.UI.WebControls.Label
    Protected WithEvents lbl_Lagerort As System.Web.UI.WebControls.Label
    Protected WithEvents lblHerstellerSchluesselShow As System.Web.UI.WebControls.Label
    Protected WithEvents lbl_Hersteller As System.Web.UI.WebControls.Label
    Protected WithEvents lblBriefnummerShow As System.Web.UI.WebControls.Label
    Protected WithEvents lbl_HerstellerSchluessel As System.Web.UI.WebControls.Label
    Protected WithEvents lbl_Briefdaten As System.Web.UI.WebControls.Label
    Protected WithEvents lblOrdernummerShow As System.Web.UI.WebControls.Label
    Protected WithEvents lbl_Briefnummer As System.Web.UI.WebControls.Label
    Protected WithEvents lblUmgemeldetAmShow As System.Web.UI.WebControls.Label
    Protected WithEvents lbl_Ordernummer As System.Web.UI.WebControls.Label
    Protected WithEvents lbl_CoC As System.Web.UI.WebControls.Label
    Protected WithEvents lbl_Aenderungsdaten As System.Web.UI.WebControls.Label
    Protected WithEvents lblFahrzeugmodellShow As System.Web.UI.WebControls.Label
    Protected WithEvents lbl_UmgemeldetAm As System.Web.UI.WebControls.Label
    Protected WithEvents lbl_Briefaufbietung As System.Web.UI.WebControls.Label
    Protected WithEvents lblTypschluesselShow As System.Web.UI.WebControls.Label
    Protected WithEvents lbl_Fahrzeugmodell As System.Web.UI.WebControls.Label
    Protected WithEvents lblErstzulassungsdatumShow As System.Web.UI.WebControls.Label
    Protected WithEvents lbl_Typschluessel As System.Web.UI.WebControls.Label
    Protected WithEvents lblFahrzeughalterShow As System.Web.UI.WebControls.Label
    Protected WithEvents lbl_Erstzulassungsdatum As System.Web.UI.WebControls.Label
    Protected WithEvents lblEhemaligesKennzeichenShow As System.Web.UI.WebControls.Label
    Protected WithEvents lbl_Fahrzeughalter As System.Web.UI.WebControls.Label
    Protected WithEvents lblEhemaligeBriefnummerShow As System.Web.UI.WebControls.Label
    Protected WithEvents lbl_EhemaligesKennzeichen As System.Web.UI.WebControls.Label
    Protected WithEvents lblVarianteVersionShow As System.Web.UI.WebControls.Label
    Protected WithEvents lbl_EhemaligeBriefnummer As System.Web.UI.WebControls.Label
    Protected WithEvents lbl_Farbe As System.Web.UI.WebControls.Label
    Protected WithEvents lblAbmeldedatumShow As System.Web.UI.WebControls.Label
    Protected WithEvents lbl_VarianteVersion As System.Web.UI.WebControls.Label
    Protected WithEvents lblStandortShow As System.Web.UI.WebControls.Label
    Protected WithEvents lbl_Abmeldedatum As System.Web.UI.WebControls.Label
    Protected WithEvents lb_Uebersicht As System.Web.UI.WebControls.LinkButton
    Protected WithEvents lbl_Standort As System.Web.UI.WebControls.Label
    Protected WithEvents lb_Typdaten As System.Web.UI.WebControls.LinkButton
    Protected WithEvents lb_Lebenslauf As System.Web.UI.WebControls.LinkButton
    Protected WithEvents lb_Uebermittlung As System.Web.UI.WebControls.LinkButton
    Protected WithEvents tr_Lagerort As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents lblVersanddatumShow As System.Web.UI.WebControls.Label
    Protected WithEvents ucStyles As Styles
    Protected WithEvents lb_Drucken As System.Web.UI.WebControls.LinkButton
    Protected WithEvents lb_Partnerdaten As System.Web.UI.WebControls.LinkButton
    Protected WithEvents dtgPartner As System.Web.UI.WebControls.DataGrid
    Protected WithEvents cmdBack As System.Web.UI.WebControls.LinkButton
    'pdiMeldung
    Protected WithEvents lb_PDIMeldung As System.Web.UI.WebControls.LinkButton
    Protected WithEvents trPDIMeldung As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents lbl_StandortPDI As System.Web.UI.WebControls.Label
    Protected WithEvents lblStandortPDIValue As System.Web.UI.WebControls.Label
    Protected WithEvents lbl_Lieferant As System.Web.UI.WebControls.Label
    Protected WithEvents lblLieferantValue As System.Web.UI.WebControls.Label
    Protected WithEvents lbl_Navi As System.Web.UI.WebControls.Label
    Protected WithEvents lblNaviValue As System.Web.UI.WebControls.Label
    Protected WithEvents lbl_Bereifung As System.Web.UI.WebControls.Label
    Protected WithEvents lblBereifungValue As System.Web.UI.WebControls.Label
    Protected WithEvents lbl_Kraftstoff As System.Web.UI.WebControls.Label
    Protected WithEvents lblKraftstoffValue As System.Web.UI.WebControls.Label
    Protected WithEvents lbl_Getriebe As System.Web.UI.WebControls.Label
    Protected WithEvents lblGetriebeValue As System.Web.UI.WebControls.Label
    Protected WithEvents lbl_Eingangsdatum As System.Web.UI.WebControls.Label
    Protected WithEvents lblEingangsdatumValue As System.Web.UI.WebControls.Label
    Protected WithEvents lbl_Bereitdatum As System.Web.UI.WebControls.Label
    Protected WithEvents lblBereitdatumValue As System.Web.UI.WebControls.Label

    Protected WithEvents lbl_Versandgrund As System.Web.UI.WebControls.Label
    Protected WithEvents lblVersandgrundShow As System.Web.UI.WebControls.Label
    Protected WithEvents lbl_UebBemerkungen As System.Web.UI.WebControls.Label
    Protected WithEvents lbl_AnzBemerkungen As System.Web.UI.WebControls.Label
    Protected WithEvents lblAnzBemerkungenShow As System.Web.UI.WebControls.Label
    Protected WithEvents lbl_Referenz2 As System.Web.UI.WebControls.Label
    Protected WithEvents lblReferenz2Show As System.Web.UI.WebControls.Label

    Private mObjFin_14 As fin_14


    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'cmdSave.Enabled = False
        m_User = GetUser(Me)
        ucHeader.InitUser(m_User)
        FormAuth(Me, m_User)
       

            m_App = New Base.Kernel.Security.App(m_User)
            If (Session("BRIEFLEBENSLAUF_LPTable") Is Nothing) Then
                Response.Redirect(Request.UrlReferrer.ToString & "?AppID=" & Session("AppID").ToString)
            Else
                BRIEFLEBENSLAUF_LPTable = CType(Session("BRIEFLEBENSLAUF_LPTable"), DataTable)
            End If
            lnkKreditlimit.NavigateUrl = Request.UrlReferrer.ToString & "?AppID=" & Session("AppID").ToString

            lblHead.Text = m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString
            ucStyles.TitleText = lblHead.Text

            If mObjFin_14 Is Nothing Then
                If Session("objReport") Is Nothing Then
                    Throw New Exception("benötigtiges SessionObjekt nicht vorhanden")
                Else
                    mObjFin_14 = CType(Session("objReport"), fin_14)
                End If
            End If


            If Not IsPostBack Then

            If Request.UrlReferrer.OriginalString.Contains("Report46.aspx") Then
                cmdBack.Visible = True
            Else
                If Request.QueryString.Item("Linked") = "true" Then
                    ucHeader.Visible = False
                End If
            End If




                'Fülle Übersicht
                '&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&
                FillUebersicht()

                'Fülle Typdaten
                '&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&
                FillABEDaten()

                'Fülle Lebenslauf
                '&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&
                FillGrid2(0)

                'Fülle Übermittlung
                '&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&
                FillGrid(0)

                'Fülle Partnerdaten
                '&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&
                FillPartnerdaten(0)

                'Fülle PDIMeldung
                '&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&
                fillPDIMeldung()

                'am anfang soll die übersicht sein
                HideAllTRs()
                ShowUebersicht()



            End If
    End Sub

    Private Sub FillUebersicht()
        lblKennzeichenShow.Text = BRIEFLEBENSLAUF_LPTable.Rows(0)("ZZKENN").ToString
        lblEhemaligesKennzeichenShow.Text = BRIEFLEBENSLAUF_LPTable.Rows(0)("ZZKENN_OLD").ToString
        lblBriefnummerShow.Text = BRIEFLEBENSLAUF_LPTable.Rows(0)("ZZBRIEF").ToString
        lblEhemaligeBriefnummerShow.Text = BRIEFLEBENSLAUF_LPTable.Rows(0)("ZZBRIEF_OLD").ToString
        If BRIEFLEBENSLAUF_LPTable.Rows(0)("ZZBRIEF_A").ToString = "X" Then
            chkBriefaufbietung.Checked = True
        Else
            chkBriefaufbietung.Checked = False
        End If
        lblFahrgestellnummerShow.Text = BRIEFLEBENSLAUF_LPTable.Rows(0)("ZZFAHRG").ToString
        lnkSchluesselinformationen.NavigateUrl &= lblFahrgestellnummerShow.Text
        lblOrdernummerShow.Text = BRIEFLEBENSLAUF_LPTable.Rows(0)("ZZREF1").ToString
        lblReferenz2Show.Text = BRIEFLEBENSLAUF_LPTable.Rows(0)("ZZREFERENZ2").ToString
        If TypeOf BRIEFLEBENSLAUF_LPTable.Rows(0)("REPLA_DATE") Is DBNull Then
            lblErstzulassungsdatumShow.Text = ""
        Else
            lblErstzulassungsdatumShow.Text = CDate(BRIEFLEBENSLAUF_LPTable.Rows(0)("REPLA_DATE").ToString).ToShortDateString()
        End If
        If TypeOf BRIEFLEBENSLAUF_LPTable.Rows(0)("EXPIRY_DATE") Is DBNull Then
            lblAbmeldedatumShow.Text = ""
        Else
            lblAbmeldedatumShow.Text = CDate(BRIEFLEBENSLAUF_LPTable.Rows(0)("EXPIRY_DATE").ToString).ToShortDateString()
        End If
        If BRIEFLEBENSLAUF_LPTable.Rows(0)("ZZSTATUS_ZUB").ToString = "X" Then
            lblStatusShow.Text = "Zulassungsfähig"
        End If
        If BRIEFLEBENSLAUF_LPTable.Rows(0)("ZZSTATUS_ZUL").ToString = "X" Then
            lblStatusShow.Text = "Zugelassen"
        End If
        If BRIEFLEBENSLAUF_LPTable.Rows(0)("ZZSTATUS_ABG").ToString = "X" Then
            lblStatusShow.Text = "Abgemeldet"
        End If
        If BRIEFLEBENSLAUF_LPTable.Rows(0)("ZZSTATUS_BAG").ToString = "X" Then
            lblStatusShow.Text = "Bei Abmeldung"
        End If
        If BRIEFLEBENSLAUF_LPTable.Rows(0)("ZZSTATUS_OZU").ToString = "X" Then
            lblStatusShow.Text = "Ohne Zulassung"
        End If
        If BRIEFLEBENSLAUF_LPTable.Rows(0)("ZZAKTSPERRE").ToString = "X" Then
            lblStatusShow.Text = "Gesperrt"
        End If

        If BRIEFLEBENSLAUF_LPTable.Rows(0)("ZZCOCKZ").ToString <> "" Then   'COC?
            cbxCOC.Checked = True
        End If
        lblStandortShow.Text = BRIEFLEBENSLAUF_LPTable.Rows(0)("NAME1_VS").ToString
        If BRIEFLEBENSLAUF_LPTable.Rows(0)("NAME2_VS").ToString.Length > 0 Then
            lblStandortShow.Text &= "<br>" & BRIEFLEBENSLAUF_LPTable.Rows(0)("NAME2_VS").ToString
        End If
        lblStandortShow.Text &= "<br>"
        If BRIEFLEBENSLAUF_LPTable.Rows(0)("STRAS_VS").ToString.Length > 0 Then
            lblStandortShow.Text &= BRIEFLEBENSLAUF_LPTable.Rows(0)("STRAS_VS").ToString
        End If
        If BRIEFLEBENSLAUF_LPTable.Rows(0)("HSNR_VS").ToString.Length > 0 Then
            lblStandortShow.Text &= " " & BRIEFLEBENSLAUF_LPTable.Rows(0)("HSNR_VS").ToString
        End If
        lblStandortShow.Text &= "<br>"
        If BRIEFLEBENSLAUF_LPTable.Rows(0)("PSTLZ_VS").ToString.Length > 0 Then
            lblStandortShow.Text &= BRIEFLEBENSLAUF_LPTable.Rows(0)("PSTLZ_VS").ToString
        End If
        If BRIEFLEBENSLAUF_LPTable.Rows(0)("ORT01_VS").ToString.Length > 0 Then
            lblStandortShow.Text &= " " & BRIEFLEBENSLAUF_LPTable.Rows(0)("ORT01_VS").ToString
        End If
        lblFahrzeughalterShow.Text = BRIEFLEBENSLAUF_LPTable.Rows(0)("NAME1_ZH").ToString
        If BRIEFLEBENSLAUF_LPTable.Rows(0)("NAME2_ZH").ToString.Length > 0 Then
            lblFahrzeughalterShow.Text &= "<br>" & BRIEFLEBENSLAUF_LPTable.Rows(0)("NAME2_ZH").ToString
        End If
        lblFahrzeughalterShow.Text &= "<br>"
        If BRIEFLEBENSLAUF_LPTable.Rows(0)("STRAS_ZH").ToString.Length > 0 Then
            lblFahrzeughalterShow.Text &= BRIEFLEBENSLAUF_LPTable.Rows(0)("STRAS_ZH").ToString
        End If
        If BRIEFLEBENSLAUF_LPTable.Rows(0)("HSNR_ZH").ToString.Length > 0 Then
            lblFahrzeughalterShow.Text &= " " & BRIEFLEBENSLAUF_LPTable.Rows(0)("HSNR_ZH").ToString
        End If
        lblFahrzeughalterShow.Text &= "<br>"
        If BRIEFLEBENSLAUF_LPTable.Rows(0)("PSTLZ_ZH").ToString.Length > 0 Then
            lblFahrzeughalterShow.Text &= BRIEFLEBENSLAUF_LPTable.Rows(0)("PSTLZ_ZH").ToString
        End If
        If BRIEFLEBENSLAUF_LPTable.Rows(0)("ORT01_ZH").ToString.Length > 0 Then
            lblFahrzeughalterShow.Text &= " " & BRIEFLEBENSLAUF_LPTable.Rows(0)("ORT01_ZH").ToString
        End If

        If BRIEFLEBENSLAUF_LPTable.Rows(0)("ZZTMPDT") Is DBNull.Value OrElse BRIEFLEBENSLAUF_LPTable.Rows(0)("ZZTMPDT") Is Nothing Then
            Select Case BRIEFLEBENSLAUF_LPTable.Rows(0)("ABCKZ").ToString
                Case "1"
                    lblLagerortShow.Text = "temporär angefordert"
                Case "2"
                    'bei gmac gibt kein endgültig schreiben, da sie nur endgültig anfordern können
                    If m_User.KUNNR = "347452" Then
                        lblLagerortShow.Text = "angefordert"
                    Else
                        lblLagerortShow.Text = "endgültig angefordert"
                    End If
                Case Else
                    lblLagerortShow.Text = "eingelagert"
            End Select
        Else
            Select Case BRIEFLEBENSLAUF_LPTable.Rows(0)("ABCKZ").ToString
                Case "1"
                    lblLagerortShow.Text = "temporär versendet"
                Case "2"
                    'bei gmac gibt kein endgültig schreiben, da sie nur endgültig anfordern können
                    If m_User.KUNNR = "347452" Then
                        lblLagerortShow.Text = "versendet"
                    Else
                        lblLagerortShow.Text = "endgültig versendet"
                    End If

                Case Else
                    lblLagerortShow.Text = "eingelagert"
            End Select
        End If



        If TypeOf BRIEFLEBENSLAUF_LPTable.Rows(0)("ZZTMPDT") Is DBNull Then
            lblVersanddatumShow.Text = ""
        Else
            lblVersanddatumShow.Text = CDate(BRIEFLEBENSLAUF_LPTable.Rows(0)("ZZTMPDT").ToString).ToShortDateString()
        End If
        
        If TypeOf BRIEFLEBENSLAUF_LPTable.Rows(0)("UDATE") Is DBNull Then
            lblUmgemeldetAmShow.Text = ""
        Else
            lblUmgemeldetAmShow.Text = CDate(BRIEFLEBENSLAUF_LPTable.Rows(0)("UDATE").ToString).ToShortDateString()
        End If

        lblFahrzeugmodellShow.Text = BRIEFLEBENSLAUF_LPTable.Rows(0)("ZZHANDELSNAME").ToString

        lblHerstellerShow.Text = BRIEFLEBENSLAUF_LPTable.Rows(0)("ZZHERST_TEXT").ToString
        lblHerstellerSchluesselShow.Text = BRIEFLEBENSLAUF_LPTable.Rows(0)("ZZHERSTELLER_SCH").ToString
        lblTypschluesselShow.Text = BRIEFLEBENSLAUF_LPTable.Rows(0)("ZZTYP_SCHL").ToString
        'lblFarbe.Text = BRIEFLEBENSLAUF_LPTable.Rows(0)("ZFARBE").ToString
        lblVarianteVersionShow.Text = BRIEFLEBENSLAUF_LPTable.Rows(0)("ZZVVS_SCHLUESSEL").ToString

        'Bemerkungen in die Übersicht eintragen

        If Not Session("AnzBemerkungen") Is Nothing Then
            lblAnzBemerkungenShow.Text = CType(Session("AnzBemerkungen"), String)
        End If


        Dim GetVersandgrund As New fin_05(m_User, m_App, Session("AppID").ToString, Session.SessionID.ToString, "")


        If Len(BRIEFLEBENSLAUF_LPTable.Rows(0)("ZZVGRUND").ToString) > 0 Then
            lblVersandgrundShow.Text = GetVersandgrund.getAbrufgrund(BRIEFLEBENSLAUF_LPTable.Rows(0)("ZZVGRUND").ToString).ToString
        End If



    End Sub

    Private Sub fillPDIMeldung()

        With BRIEFLEBENSLAUF_LPTable
            lblStandortPDIValue.Text = .Rows(0)("DADPDI_NAME1").ToString & " " & .Rows(0)("DADPDI_ORT").ToString
            lblNaviValue.Text = .Rows(0)("ZZNAVI").ToString
            lblBereifungValue.Text = .Rows(0)("ZZREIFEN").ToString
            lblKraftstoffValue.Text = .Rows(0)("ZZANTR").ToString
            lblGetriebeValue.Text = .Rows(0)("ZZSIPP3").ToString
            lblEingangsdatumValue.Text = MakeStandardDateString(.Rows(0)("ZZDAT_EIN").ToString)
            lblBereitdatumValue.Text = MakeStandardDateString(.Rows(0)("ZZDAT_BER").ToString)
        End With

        If mObjFin_14.GT_ADDR.Select("ADDRTYP='ZP'").Length > 0 Then
            lblLieferantValue.Text = mObjFin_14.GT_ADDR.Select("ADDRTYP='ZP'")(0)("LieferantAdresse").ToString
        End If

    End Sub


    Private Sub FillABEDaten()
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

        objPDIs = New ABEDaten(m_User, m_App, Session("AppID").ToString, Session.SessionID.ToString, "", m_User.KUNNR, "ZDAD", "", "")

        If Not objPDIs Is Nothing Then
            If BRIEFLEBENSLAUF_LPTable.Rows(0)("EQUNR") Is Nothing OrElse CType(BRIEFLEBENSLAUF_LPTable.Rows(0)("EQUNR"), String).Trim(" "c).Length = 0 Then
                lblError.Text = "Fehler: Die Daten enthalten keine Fahrzeugnummer."
            Else
                'TrimStart("0"c)
                objPDIs.FillDatenABE(Session("AppID").ToString, Session.SessionID.ToString, Me.Page, CType(BRIEFLEBENSLAUF_LPTable.Rows(0)("EQUNR"), String))
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

                        Select Case .Farbziffer
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
                        Session.Add("objPDIs", objPDIs)
                    End With
                Else
                    lblError.Text = objPDIs.Message
                End If
            End If
        End If
    End Sub

    Private Sub FillGrid(ByVal intPageIndex As Int32, Optional ByVal strSort As String = "")
        If Not (Session("QMMIDATENTable") Is Nothing) Then
            QMMIDATENTable = CType(Session("QMMIDATENTable"), DataTable)

            If QMMIDATENTable.Rows.Count > 0 Then
                Dim tmpDataView As New DataView()
                tmpDataView = QMMIDATENTable.DefaultView

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

    Private Sub FillPartnerdaten(ByVal intPageIndex As Int32, Optional ByVal strSort As String = "")
        If Not mObjFin_14.GT_ADDR Is Nothing Then

            If mObjFin_14.GT_ADDR.Rows.Count > 0 Then

                'hier müssen ein paar spalten raugenommen werden, da die nur 
                'intern benötigt werden bzw für die PDI_Meldung von autovermietern
                mAddressdaten = mObjFin_14.GT_ADDR.Copy

                mAddressdaten.Columns.Remove("LieferantAdresse")
                mAddressdaten.Columns.Remove("ADDRTYP")
                mAddressdaten.AcceptChanges()


                Dim tmpDataView As New DataView()
                tmpDataView = mAddressdaten.DefaultView

                If Not tmpDataView.Count = 0 Then
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

                    dtgPartner.CurrentPageIndex = intTempPageIndex
                    dtgPartner.DataSource = tmpDataView

                    dtgPartner.DataBind()

                    If dtgPartner.PageCount > 1 Then
                        dtgPartner.PagerStyle.CssClass = "PagerStyle"
                        dtgPartner.DataBind()
                        dtgPartner.PagerStyle.Visible = True
                    Else
                        DataGrid1.PagerStyle.Visible = False
                    End If
                End If
            End If

        End If
    End Sub

    Private Sub FillGrid2(ByVal intPageIndex As Int32, Optional ByVal strSort As String = "")
        If Not (Session("QMEL_DATENTable") Is Nothing) Then
            QMEL_DATENTable = CType(Session("QMEL_DATENTable"), DataTable)

            If QMEL_DATENTable.Rows.Count > 0 Then
                Dim tmpDataView As New DataView()
                tmpDataView = QMEL_DATENTable.DefaultView

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

    Private Sub ShowUebersicht()
        HideAllTRs()
        trUebersicht.Visible = True
        lb_Uebersicht.Enabled = False
    End Sub

    Private Sub showPDIMeldung()
        HideAllTRs()
        trPDIMeldung.Visible = True
        lb_PDIMeldung.Enabled = False
    End Sub

    Private Sub ShowPartnerdaten()
        HideAllTRs()
        trPartnerdaten.Visible = True
        lb_Partnerdaten.Enabled = False
    End Sub

    Private Sub HideAllTRs()
        trUebersicht.Visible = False
        trUebermittlung.Visible = False
        trTypdaten.Visible = False
        trLebenslauf.Visible = False
        trPartnerdaten.Visible = False
        trPDIMeldung.Visible = False

        lb_Uebersicht.Enabled = True
        lb_Uebermittlung.Enabled = True
        lb_Typdaten.Enabled = True
        lb_Lebenslauf.Enabled = True
        lb_Drucken.Enabled = True
        lb_Partnerdaten.Enabled = True
        lb_PDIMeldung.Enabled = True
    End Sub

    Protected Sub lb_Drucken_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lb_Drucken.Click

        'Response.Write("<script>window.open('UeberfPrint.aspx?AppID=" & Session("AppID").ToString & "')</script>")

        Try

            Dim imageHt As New Hashtable()
            Dim mReport As fin_14
            mReport = CType(Session("objReport"), fin_14)
            imageHt.Add("Logo", m_User.Customer.LogoImage)
            If objPDIs Is Nothing Then
                objPDIs = CType(Session("objPDIs"), Base.Business.ABEDaten)
            End If

            With mReport
                .Fahrgestellnummer = lblFahrgestellnummerShow.Text
                .Abmeldedatum = lblAbmeldedatumShow.Text
                .BriefStatus = lblStatusShow.Text
                .Kennzeichen = lblKennzeichenShow.Text
                .Lagerort = lblLagerortShow.Text
                .Hersteller = lblHerstellerShow.Text
                .Fahrzeugmodell = lblFahrzeugmodellShow.Text
                .Farbe = objPDIs.ABE_Daten.ZZFARBE
                .Herstellerschluessel = lblHerstellerSchluesselShow.Text
                .Typschluessel = lblTypschluesselShow.Text
                .VarianteVersion = lblVarianteVersionShow.Text
                .ZBIINummerBEZ = lbl_Briefnummer.Text
                .ZBIINummer = lblBriefnummerShow.Text
                .Erstzulassungsdatum = lblErstzulassungsdatumShow.Text
                .Abmeldedatum = lblAbmeldedatumShow.Text
                .Finanzierungsnummer = lblOrdernummerShow.Text
                .FinanzierungsnummerBEZ = lbl_Ordernummer.Text
                'traurig aber wahr, nach string "<br>" splittet er nicht richtig JJU2008.07.01
                .FahrzeughalterName1Name2 = lblFahrzeughalterShow.Text.Replace("<br>", ";").Split(CStr(";"))(0)
                .FahrzeughalterStrasseNummer = lblFahrzeughalterShow.Text.Replace("<br>", ";").Split(CStr(";"))(1)
                .FahrzeughalterPLZOrt = lblFahrzeughalterShow.Text.Replace("<br>", ";").Split(CStr(";"))(2)
                .VersandadresseName1Name2 = lblStandortShow.Text.Replace("<br>", ";").Split(CStr(";"))(0)
                .VersandadresseStrasseNummer = lblStandortShow.Text.Replace("<br>", ";").Split(CStr(";"))(1)
                .VersandadressePLZOrt = lblStandortShow.Text.Replace("<br>", ";").Split(CStr(";"))(2)
                .VersandadresseBEZ = lbl_Standort.Text
                .AusdruckDatumUhrzeit = Date.Now.ToShortDateString & " / " & Date.Now.ToShortTimeString
                .Username = m_User.UserName
                If cbxCOC.Checked = True Then
                    .COC = "Ja"
                Else
                    .COC = "Nein"
                End If
                .COCBEZ = lbl_CoC.Text
                .UmgemeldetAM = lblUmgemeldetAmShow.Text
                .EhmaligesKennzeichen = lblEhemaligesKennzeichenShow.Text
                If chkBriefaufbietung.Checked = True Then
                    .ZBIIAufbietung = "Ja"
                Else
                    .ZBIIAufbietung = "Nein"
                End If
                .EhmaligeZBIINummer = lblEhemaligeBriefnummerShow.Text
                .Referenz2BEZ = lbl_Referenz2.Text
                .Referenz2 = lblReferenz2Show.Text
            End With



            Dim tblData As DataTable = Base.Kernel.Common.DataTableHelper.ObjectToDataTable(mReport)

            Dim docFactory As New Base.Kernel.DocumentGeneration.WordDocumentFactory(tblData, imageHt)

            Dim dataTables(1) As DataTable

            dataTables(0) = createLebenslaufTable()
            dataTables(1) = createUebermittlungsTable()
            docFactory.CreateDocument("Fahrzeughistorie_" & lblFahrgestellnummerShow.Text, Me.Page, "\Applications\appffe\docu\FahrzeughistoriePrint.doc", "Tabelle", dataTables)

        Catch ex As Exception

            lbError.Visible = True
            lbError.Text = ex.Message

        End Try
    End Sub

    Private Function createLebenslaufTable() As DataTable
        Dim tmpTable As DataTable = CType(Session("QMEL_DATENTable"), DataTable)
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
            tmpLebenslaufRow.Item("Beauftragt durch") = tmpRow("QMNAM").ToString

            Lebenslauf.Rows.Add(tmpLebenslaufRow)
            Lebenslauf.AcceptChanges()

        Next
        Lebenslauf.TableName = "Lebenslauf"
        Return Lebenslauf


    End Function

    Private Function createUebermittlungsTable() As DataTable
        Dim tmpTable As DataTable = CType(Session("QMMIDATENTable"), DataTable)
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



    Private Function MakeStandardDateString(ByVal strSAPDate As String) As String
        Return Right(strSAPDate, 2) & "." & Mid(strSAPDate, 5, 2) & "." & Left(strSAPDate, 4)
    End Function

    Private Sub DataGrid1_PageIndexChanged(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles DataGrid1.PageIndexChanged
        FillGrid(e.NewPageIndex)
    End Sub

    Private Sub DataGrid1_SortCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs) Handles DataGrid1.SortCommand
        FillGrid(DataGrid1.CurrentPageIndex, e.SortExpression)
    End Sub

    Private Sub Datagrid2_PageIndexChanged(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles Datagrid2.PageIndexChanged
        FillGrid2(e.NewPageIndex)
    End Sub

    Private Sub Datagrid2_SortCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs) Handles Datagrid2.SortCommand
        FillGrid2(Datagrid2.CurrentPageIndex, e.SortExpression)
    End Sub

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        SetEndASPXAccess(Me)
    End Sub

    Private Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Unload
        SetEndASPXAccess(Me)
    End Sub

    Private Sub lb_Uebersicht_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lb_Uebersicht.Click
        ShowUebersicht()
    End Sub

    Private Sub lb_Typdaten_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lb_Typdaten.Click
        HideAllTRs()
        trTypdaten.Visible = True
        lb_Typdaten.Enabled = False
    End Sub

    Private Sub lb_Lebenslauf_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lb_Lebenslauf.Click
        HideAllTRs()
        trLebenslauf.Visible = True
        lb_Lebenslauf.Enabled = False
    End Sub

    Private Sub lb_Uebermittlung_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lb_Uebermittlung.Click
        HideAllTRs()
        trUebermittlung.Visible = True
        lb_Uebermittlung.Enabled = False
    End Sub

    Protected Sub lb_Partnerdaten_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lb_Partnerdaten.Click
        ShowPartnerdaten()
    End Sub

    Private Sub dtgPartner_PageIndexChanged(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles dtgPartner.PageIndexChanged
        FillPartnerdaten(e.NewPageIndex)
    End Sub

    Private Sub dtgPartner_SortCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs) Handles dtgPartner.SortCommand
        FillPartnerdaten(dtgPartner.CurrentPageIndex, e.SortExpression)
    End Sub

    Private Sub dtgPartner_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgPartner.ItemDataBound
        Dim intItem As Int32

        For intItem = 0 To mAddressdaten.Columns.Count - 1
            If mAddressdaten.Columns(intItem).DataType Is System.Type.GetType("System.DateTime") Then
                If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Then
                    e.Item.Cells(intItem).Text = DataBinder.Eval(e.Item.DataItem, mAddressdaten.Columns(intItem).ColumnName, "{0:dd.MM.yyyy}")
                End If
            End If
        Next
    End Sub

    Protected Sub lb_PDIMeldung_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lb_PDIMeldung.Click
        showPDIMeldung()
    End Sub

    Protected Sub cmdBack_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdBack.Click
        Response.Redirect("Report46.aspx?AppID=" & Session("AppID").ToString)
    End Sub
End Class

' ************************************************
' $History: Report46_2.aspx.vb $
' 
' *****************  Version 22  *****************
' User: Fassbenders  Date: 5.10.10    Time: 14:16
' Updated in $/CKAG/Components/ComCommon/Finance
' 
' *****************  Version 21  *****************
' User: Fassbenders  Date: 10.03.10   Time: 16:51
' Updated in $/CKAG/Components/ComCommon/Finance
' ITA: 2918
' 
' *****************  Version 20  *****************
' User: Rudolpho     Date: 29.10.09   Time: 15:14
' Updated in $/CKAG/Components/ComCommon/Finance
' 
' *****************  Version 19  *****************
' User: Rudolpho     Date: 28.10.09   Time: 16:46
' Updated in $/CKAG/Components/ComCommon/Finance
' 
' *****************  Version 18  *****************
' User: Fassbenders  Date: 26.10.09   Time: 17:13
' Updated in $/CKAG/Components/ComCommon/Finance
' 
' *****************  Version 17  *****************
' User: Rudolpho     Date: 7.10.09    Time: 17:53
' Updated in $/CKAG/Components/ComCommon/Finance
' ITA: 3114
' 
' *****************  Version 16  *****************
' User: Fassbenders  Date: 4.09.09    Time: 9:55
' Updated in $/CKAG/Components/ComCommon/Finance
' ITA: 3050
' 
' *****************  Version 15  *****************
' User: Jungj        Date: 5.06.09    Time: 15:58
' Updated in $/CKAG/Components/ComCommon/Finance
' mögliche try catches entfernt
' 
' *****************  Version 14  *****************
' User: Jungj        Date: 30.03.09   Time: 14:13
' Updated in $/CKAG/Components/ComCommon/Finance
' ITA 2760 
' 
' *****************  Version 13  *****************
' User: Jungj        Date: 23.03.09   Time: 17:35
' Updated in $/CKAG/Components/ComCommon/Finance
' ITA 2679
' 
' *****************  Version 12  *****************
' User: Jungj        Date: 27.02.09   Time: 10:54
' Updated in $/CKAG/Components/ComCommon/Finance
' Bugfix Briefanforderung partnerdaten
' 
' *****************  Version 11  *****************
' User: Fassbenders  Date: 26.02.09   Time: 17:57
' Updated in $/CKAG/Components/ComCommon/Finance
' 
' *****************  Version 10  *****************
' User: Jungj        Date: 19.02.09   Time: 13:57
' Updated in $/CKAG/Components/ComCommon/Finance
' ITA 2549 testfertig
' 
' *****************  Version 9  *****************
' User: Jungj        Date: 16.02.09   Time: 17:29
' Updated in $/CKAG/Components/ComCommon/Finance
' ITa 2413
' 
' *****************  Version 8  *****************
' User: Jungj        Date: 2.02.09    Time: 13:47
' Updated in $/CKAG/Components/ComCommon/Finance
' unfertig
' 
' *****************  Version 7  *****************
' User: Jungj        Date: 2.02.09    Time: 13:02
' Updated in $/CKAG/Components/ComCommon/Finance
' ITA 2549 unfertig
' 
' *****************  Version 6  *****************
' User: Jungj        Date: 20.10.08   Time: 17:49
' Updated in $/CKAG/Components/ComCommon/Finance
' ITA 2287 testfertig
' 
' *****************  Version 5  *****************
' User: Jungj        Date: 31.07.08   Time: 11:50
' Updated in $/CKAG/Components/ComCommon/Finance
' ITA 2154
' 
' *****************  Version 4  *****************
' User: Jungj        Date: 29.07.08   Time: 15:05
' Updated in $/CKAG/Components/ComCommon/Finance
' Nachbesserung LagerOrt=DAD wenn kein fall zutrifft
' 
' *****************  Version 3  *****************
' User: Jungj        Date: 28.04.08   Time: 16:25
' Updated in $/CKAG/Components/ComCommon/Finance
' ITA 1811
' 
' *****************  Version 2  *****************
' User: Rudolpho     Date: 11.04.08   Time: 13:43
' Updated in $/CKAG/Components/ComCommon/Finance
' Migration
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 4.04.08    Time: 13:32
' Created in $/CKAG/Components/ComCommon/Finance
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 4.04.08    Time: 10:48
' Created in $/CKAG/portal/Components/ComCommon/Finance
' 
' *****************  Version 1  *****************
' User: Uha          Date: 20.12.07   Time: 11:16
' Created in $/CKG/Components/ComCommon/ComCommonWeb/Finance
' ITA 1505 Fahrzeughistorie in Testversion
' 
' ************************************************
