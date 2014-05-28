Imports CKG.Base.Business
Imports CKG.Portal.PageElements
Imports CKG.Base.Kernel.Common.Common

Public Class Report206_2
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
    Private ResultTable As DataTable
    Private QMMIDATENTable As DataTable
    Private QMEL_DATENTable As DataTable
    Private objPDIs As Base.Business.ABEDaten

    Protected WithEvents lblError As System.Web.UI.WebControls.Label
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
    Protected WithEvents lbl_Ordernummer As System.Web.UI.WebControls.Label
    Protected WithEvents lbl_CoC As System.Web.UI.WebControls.Label
    Protected WithEvents lbl_Aenderungsdaten As System.Web.UI.WebControls.Label
    Protected WithEvents lblFahrzeugmodellShow As System.Web.UI.WebControls.Label
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
    Protected WithEvents lblBriefeingangShow As System.Web.UI.WebControls.Label
    Protected WithEvents lbl_Briefeingang As System.Web.UI.WebControls.Label
    Protected WithEvents lblCheckIn As System.Web.UI.WebControls.Label
    Protected WithEvents lblPDIEingang As System.Web.UI.WebControls.Label
    Protected WithEvents chkFahzeugschein As System.Web.UI.WebControls.CheckBox
    Protected WithEvents chkVorhandeneElemente As System.Web.UI.WebControls.CheckBox

    Protected WithEvents ucStyles As Styles

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        m_User = GetUser(Me)
        ucHeader.InitUser(m_User)
        FormAuth(Me, m_User)

        Try
            m_App = New Base.Kernel.Security.App(m_User)
            If (Session("ResultTable") Is Nothing) Then
                Response.Redirect(Request.UrlReferrer.ToString & "?AppID=" & Session("AppID").ToString)
            Else
                ResultTable = CType(Session("ResultTable"), DataTable)
            End If
            lnkKreditlimit.NavigateUrl = Request.UrlReferrer.ToString & "?AppID=" & Session("AppID").ToString

            lblHead.Text = m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString
            ucStyles.TitleText = lblHead.Text

            If Not IsPostBack Then
                'Fülle Übersicht
                FillUebersicht()

                'Fülle Typdaten
                FillABEDaten()

                'Fülle Lebenslauf
                FillGrid2(0)

                'Fülle Übermittlung
                FillGrid(0)

                ShowUebersicht()
            End If
        Catch ex As Exception
            lblError.Text = "Beim Laden der Seite ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
        End Try
    End Sub

    Private Sub FillUebersicht()
        lblKennzeichenShow.Text = ResultTable.Rows(0)("ZZKENN").ToString
        lblEhemaligesKennzeichenShow.Text = ResultTable.Rows(0)("ZZKENN_OLD").ToString
        lblBriefnummerShow.Text = ResultTable.Rows(0)("ZZBRIEF").ToString
        lblEhemaligeBriefnummerShow.Text = ResultTable.Rows(0)("ZZBRIEF_OLD").ToString
       
        lblFahrgestellnummerShow.Text = ResultTable.Rows(0)("ZZFAHRG").ToString
        lnkSchluesselinformationen.NavigateUrl &= lblFahrgestellnummerShow.Text
        lblOrdernummerShow.Text = ResultTable.Rows(0)("ZZREF1").ToString

        lblErstzulassungsdatumShow.Text = MakeStandardDateString(ResultTable.Rows(0)("REPLA_DATE").ToString)

        lblAbmeldedatumShow.Text = MakeStandardDateString(ResultTable.Rows(0)("EXPIRY_DATE").ToString)
        lblBriefeingangShow.Text = MakeStandardDateString(ResultTable.Rows(0)("ERDAT").ToString)

        If ResultTable.Rows(0)("ZZSTATUS_ZUB").ToString = "X" Then
            lblStatusShow.Text = "Zulassungsfähig"
        End If
        If ResultTable.Rows(0)("ZZSTATUS_ZUL").ToString = "X" Then
            lblStatusShow.Text = "Zugelassen"
        End If
        If ResultTable.Rows(0)("ZZSTATUS_ABG").ToString = "X" Then
            lblStatusShow.Text = "Abgemeldet"
        End If
        If ResultTable.Rows(0)("ZZSTATUS_BAG").ToString = "X" Then
            lblStatusShow.Text = "Bei Abmeldung"
        End If
        If ResultTable.Rows(0)("ZZSTATUS_OZU").ToString = "X" Then
            lblStatusShow.Text = "Ohne Zulassung"
        End If
        If ResultTable.Rows(0)("ZZAKTSPERRE").ToString = "X" Then
            lblStatusShow.Text = "Gesperrt"
        End If

        If ResultTable.Rows(0)("ZZCOCKZ").ToString = "X" Then   'COC?
            cbxCOC.Checked = True
        End If

        lblStandortShow.Text = ResultTable.Rows(0)("NAME1_VS").ToString

        If ResultTable.Rows(0)("NAME2_VS").ToString.Length > 0 Then
            lblStandortShow.Text &= "<br>" & ResultTable.Rows(0)("NAME2_VS").ToString
        End If

        lblStandortShow.Text &= "<br>"

        If ResultTable.Rows(0)("STRAS_VS").ToString.Length > 0 Then
            lblStandortShow.Text &= ResultTable.Rows(0)("STRAS_VS").ToString
        End If
        If ResultTable.Rows(0)("HSNR_VS").ToString.Length > 0 Then
            lblStandortShow.Text &= " " & ResultTable.Rows(0)("HSNR_VS").ToString
        End If

        lblStandortShow.Text &= "<br>"

        If ResultTable.Rows(0)("PSTLZ_VS").ToString.Length > 0 Then
            lblStandortShow.Text &= ResultTable.Rows(0)("PSTLZ_VS").ToString
        End If
        If ResultTable.Rows(0)("ORT01_VS").ToString.Length > 0 Then
            lblStandortShow.Text &= " " & ResultTable.Rows(0)("ORT01_VS").ToString
        End If

        lblFahrzeughalterShow.Text = ResultTable.Rows(0)("NAME1_ZH").ToString

        If ResultTable.Rows(0)("NAME2_ZH").ToString.Length > 0 Then
            lblFahrzeughalterShow.Text &= "<br>" & ResultTable.Rows(0)("NAME2_ZH").ToString
        End If

        lblFahrzeughalterShow.Text &= "<br>"

        If ResultTable.Rows(0)("STRAS_ZH").ToString.Length > 0 Then
            lblFahrzeughalterShow.Text &= ResultTable.Rows(0)("STRAS_ZH").ToString
        End If
        If ResultTable.Rows(0)("HSNR_ZH").ToString.Length > 0 Then
            lblFahrzeughalterShow.Text &= " " & ResultTable.Rows(0)("HSNR_ZH").ToString
        End If

        lblFahrzeughalterShow.Text &= "<br>"

        If ResultTable.Rows(0)("PSTLZ_ZH").ToString.Length > 0 Then
            lblFahrzeughalterShow.Text &= ResultTable.Rows(0)("PSTLZ_ZH").ToString
        End If

        If ResultTable.Rows(0)("ORT01_ZH").ToString.Length > 0 Then
            lblFahrzeughalterShow.Text &= " " & ResultTable.Rows(0)("ORT01_ZH").ToString
        End If

        If ResultTable.Rows(0)("SCHILDER_PHY").ToString = "X" Then
            chkVorhandeneElemente.Checked = True
        Else
            chkVorhandeneElemente.Checked = False
        End If

        If ResultTable.Rows(0)("SCHEIN_PHY").ToString = "X" Then
            chkFahzeugschein.Checked = True
        Else
            chkFahzeugschein.Checked = False
        End If

        If IsDate(ResultTable.Rows(0)("ZZCARPORT_EING").ToString) Then
            lblPDIEingang.Text = CStr(CDate(ResultTable.Rows(0)("ZZCARPORT_EING")).ToShortDateString)
        End If

        If IsDate(ResultTable.Rows(0)("CHECK_IN").ToString) Then
            lblCheckIn.Text = CStr(CDate(ResultTable.Rows(0)("CHECK_IN")).ToShortDateString)
        End If

        Select Case ResultTable.Rows(0)("ABCKZ").ToString
            Case ""
                lblLagerortShow.Text = "DAD"
            Case "0"
                lblLagerortShow.Text = "DAD"
            Case "1"
                lblLagerortShow.Text = "temporär versendet"
            Case "2"
                lblLagerortShow.Text = "endgültig versendet"
        End Select
        
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
            If ResultTable.Rows(0)("EQUNR") Is Nothing OrElse CType(ResultTable.Rows(0)("EQUNR"), String).Trim(" "c).Length = 0 Then
                lblError.Text = "Fehler: Die Daten enthalten keine Fahrzeugnummer."
            Else

                objPDIs.FillDatenABE(Session("AppID").ToString, Session.SessionID.ToString, Me, CType(ResultTable.Rows(0)("EQUNR"), String))
                If objPDIs.Status = 0 Then
                    With objPDIs.ABE_Daten
                        
                        lblFahrzeugmodellShow.Text = .ZZHANDELSNAME
                        lblHerstellerShow.Text = .ZZHERST_TEXT
                        lblHerstellerSchluesselShow.Text = .ZZHERSTELLER_SCH
                        lblTypschluesselShow.Text = .ZZTYP_SCHL
                        lblVarianteVersionShow.Text = .ZZVVS_SCHLUESSEL
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
                            Case "0"
                                lbl_99.Visible = True
                                lbl_199.Visible = True
                            Case "1"
                                lbl_98.Visible = True
                                lbl_198.Visible = True
                            Case "2"
                                lbl_97.Visible = True
                                lbl_197.Visible = True
                            Case "3"
                                lbl_96.Visible = True
                                lbl_196.Visible = True
                            Case "4"
                                lbl_95.Visible = True
                                lbl_195.Visible = True
                            Case "5"
                                lbl_94.Visible = True
                                lbl_194.Visible = True
                            Case "6"
                                lbl_93.Visible = True
                                lbl_193.Visible = True
                            Case "7"
                                lbl_92.Visible = True
                                lbl_192.Visible = True
                            Case "8"
                                lbl_91.Visible = True
                                lbl_191.Visible = True
                            Case "9"
                                lbl_55.Visible = True
                                lbl_155.Visible = True
                        End Select
                        Session.Add("App_objPDIs", objPDIs)
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
                Dim tmpDataView As DataView = QMMIDATENTable.DefaultView

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
        If Not (Session("QMEL_DATENTable") Is Nothing) Then
            QMEL_DATENTable = CType(Session("QMEL_DATENTable"), DataTable)

            If QMEL_DATENTable.Rows.Count > 0 Then
                Dim tmpDataView As DataView = QMEL_DATENTable.DefaultView

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
                        Dim control1 = TryCast(control, Label)
                        If (control1 IsNot Nothing) Then
                            If control.ID = "Label1" Or control.ID = "Label2" Or control.ID = "Label3" Then
                                label = control1
                                text &= label.Text
                            End If
                        End If
                    Next
                    If text.Trim(" "c).Length = 0 Then
                        For Each control In cell.Controls
                            Dim control1 = TryCast(control, Literal)
                            If (control1 IsNot Nothing) Then
                                literal = control1
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

    Private Sub HideAllTRs()
        trUebersicht.Visible = False
        trUebermittlung.Visible = False
        trTypdaten.Visible = False
        trLebenslauf.Visible = False

        lb_Uebersicht.Enabled = True
        lb_Uebermittlung.Enabled = True
        lb_Typdaten.Enabled = True
        lb_Lebenslauf.Enabled = True
    End Sub

    Private Function MakeStandardDateString(ByVal strSapDate As String) As String
        If IsDate(strSapDate) Then
            strSapDate = FormatDateTime(CDate(strSapDate), DateFormat.ShortDate)
            Return strSapDate
        Else
            Return ""
        End If
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

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        SetEndASPXAccess(Me)
    End Sub

    Private Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Unload
        SetEndASPXAccess(Me)
    End Sub
End Class

' ************************************************
' $History: Report206_2.aspx.vb $
' 
' *****************  Version 5  *****************
' User: Rudolpho     Date: 4.06.10    Time: 10:13
' Updated in $/CKAG/Applications/apparval/Forms
' ITA: 3780
' 
' *****************  Version 4  *****************
' User: Rudolpho     Date: 5.01.10    Time: 15:01
' Updated in $/CKAG/Applications/apparval/Forms
' 
' *****************  Version 3  *****************
' User: Rudolpho     Date: 5.01.10    Time: 13:50
' Updated in $/CKAG/Applications/apparval/Forms
' 
' *****************  Version 2  *****************
' User: Rudolpho     Date: 30.06.09   Time: 13:03
' Updated in $/CKAG/Applications/apparval/Forms
' ITA: 2918
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 4.04.08    Time: 16:18
' Created in $/CKAG/Applications/apparval/Forms
' 
' *****************  Version 6  *****************
' User: Uha          Date: 2.07.07    Time: 10:05
' Updated in $/CKG/Applications/AppARVAL/AppARVALWeb/Forms
' Logging der Laufzeiten der ASPX-Seiten eingeführt
' 
' *****************  Version 5  *****************
' User: Uha          Date: 6.03.07    Time: 14:48
' Updated in $/CKG/Applications/AppARVAL/AppARVALWeb/Forms
' 
' ************************************************
