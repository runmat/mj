Imports CKG.Base.Kernel.Security
Imports CKG.Base.Business
Imports CKG.Base.Kernel.Common.Common

Partial Public Class Report02_2
    Inherits Page

#Region "Declarations"

    Private m_App As App
    Private m_User As User
    Private BRIEFLEBENSLAUF_LPTable As DataTable
    Private QMMIDATENTable As DataTable
    Private QMEL_DATENTable As DataTable
    Private LLSCHDATENTable As DataTable
    Private objPDIs As ABEDaten

    Public ReadOnly Property ShowZweitschluessel As Boolean
        Get
            Return (Session("History_ShowZweitschluessel") IsNot Nothing AndAlso CBool(Session("History_ShowZweitschluessel")))
        End Get
    End Property

#End Region

#Region "Events"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        m_User = GetUser(Me)
        FormAuth(Me, m_User)

        Try
            m_App = New App(m_User)
            If (Session("BRIEFLEBENSLAUF_LPTable") Is Nothing) Then
                Response.Redirect(Request.UrlReferrer.ToString & "?AppID=" & Session("AppID").ToString)
            Else
                BRIEFLEBENSLAUF_LPTable = CType(Session("BRIEFLEBENSLAUF_LPTable"), DataTable)
            End If

            lblHead.Text = m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString

            If Not IsPostBack Then

                If Not Request.QueryString.Item("cw") Is Nothing Then
                    lbBack.Text = "schließen"
                End If

                'Fülle Übersicht
                FillUebersicht()

                'Fülle Typdaten
                FillABEDaten()

                'Fülle Lebenslauf
                FillGrid2(0)

                'Fülle Übermittlung
                FillGrid(0)

                'Fülle Händlerdaten
                FillHaendleradresse()

                'Fülle Zweitschlüssel-Lebenslauf
                If ShowZweitschluessel Then FillGrid3(0)

            End If

            ihShowZweitschluessel.Value = ShowZweitschluessel.ToString.ToLower()

        Catch ex As Exception
            lblError.Text = "Beim Laden der Seite ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
        End Try
    End Sub

    Private Sub DataGrid1_PageIndexChanged(ByVal source As Object, ByVal e As DataGridPageChangedEventArgs) Handles DataGrid1.PageIndexChanged
        FillGrid(e.NewPageIndex)
    End Sub

    Private Sub DataGrid1_SortCommand(ByVal source As Object, ByVal e As DataGridSortCommandEventArgs) Handles DataGrid1.SortCommand
        FillGrid(DataGrid1.CurrentPageIndex, e.SortExpression)
    End Sub

    Private Sub Datagrid2_PageIndexChanged(ByVal source As Object, ByVal e As DataGridPageChangedEventArgs) Handles Datagrid2.PageIndexChanged
        FillGrid2(e.NewPageIndex)
    End Sub

    Private Sub Datagrid2_SortCommand(ByVal source As Object, ByVal e As DataGridSortCommandEventArgs) Handles Datagrid2.SortCommand
        FillGrid2(Datagrid2.CurrentPageIndex, e.SortExpression)
    End Sub

    Private Sub DataGrid3_PageIndexChanged(ByVal source As Object, ByVal e As DataGridPageChangedEventArgs) Handles Datagrid3.PageIndexChanged
        FillGrid3(e.NewPageIndex)
    End Sub

    Private Sub DataGrid3_SortCommand(ByVal source As Object, ByVal e As DataGridSortCommandEventArgs) Handles Datagrid3.SortCommand
        FillGrid3(Datagrid2.CurrentPageIndex, e.SortExpression)
    End Sub

    Protected Sub lbBack_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lbBack.Click

        If Not Request.QueryString.Item("cw") Is Nothing Then
            Dim strscript As String = "<script language=javascript>window.top.close();</script>"

            If (Not ClientScript.IsStartupScriptRegistered("clientScript")) Then

                ClientScript.RegisterStartupScript(Me.GetType(), "clientScript", strscript)
            End If

            Exit Sub
        End If

        If Not Request.QueryString.Item("LinkedID") Is Nothing Then
            Dim dRow() As DataRow
            dRow = m_User.Applications.Select("AppName ='" & Request.QueryString.Item("LinkedID") & "'")
            If dRow.Length > 0 Then
                Dim sUrl As String
                sUrl = dRow(0)("AppUrl").ToString
                Response.Redirect(sUrl & "?AppID=" & Request.QueryString.Item("LinkedID"), False)
            Else
                Response.Redirect("../../../Start/Selection.aspx?AppID=" & Session("AppID").ToString, False)
            End If
        ElseIf Request.UrlReferrer.OriginalString.Contains("Report02.aspx") Then
            Response.Redirect("../../../Start/Selection.aspx?AppID=" & Session("AppID").ToString, False)
        Else
            Response.Redirect("Report02.aspx?AppID=" & Session("AppID").ToString)
        End If

    End Sub

    Protected Sub lbCreatePDF_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lbCreatePDF.Click
        Try
            Dim imageHt As New Hashtable()
            Dim mReport As Historie
            mReport = CType(Session("objReport"), Historie)
            imageHt.Add("Logo", m_User.Customer.LogoImage)
            If objPDIs Is Nothing Then
                objPDIs = CType(Session("objPDIs"), ABEDaten)
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

            End With

            Dim tblData As DataTable = CKG.Base.Kernel.Common.DataTableHelper.ObjectToDataTable(mReport)

            Dim docFactory As New CKG.Base.Kernel.DocumentGeneration.WordDocumentFactory(tblData, imageHt)

            Dim dataTables(1) As DataTable

            dataTables(0) = createLebenslaufTable()
            dataTables(1) = createUebermittlungsTable()
            docFactory.CreateDocument("Fahrzeughistorie_" & lblFahrgestellnummerShow.Text, Page, "\Applications\appF2\docu\FahrzeughistoriePrint.doc", "Tabelle", dataTables)
        Catch ex As Exception
            lblError.Text = "Fehler beim Erstellen des Ausdrucks: " + ex.Message
        End Try

    End Sub

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As EventArgs) Handles MyBase.PreRender
        SetEndASPXAccess(Me)
    End Sub

    Private Sub Page_Unload(ByVal sender As Object, ByVal e As EventArgs) Handles MyBase.Unload
        SetEndASPXAccess(Me)
    End Sub

#End Region

#Region "Methods"

    Private Sub FillUebersicht()
        Dim row As DataRow = BRIEFLEBENSLAUF_LPTable.Rows(0)

        lblKennzeichenShow.Text = row("ZZKENN").ToString
        lblEhemaligesKennzeichenShow.Text = row("ZZKENN_OLD").ToString
        lblBriefnummerShow.Text = row("ZZBRIEF").ToString
        lblEhemaligeBriefnummerShow.Text = row("ZZBRIEF_OLD").ToString
        If row("ZZBRIEF_A").ToString = "X" Then
            chkBriefaufbietung.Checked = True
        Else
            chkBriefaufbietung.Checked = False
        End If
        lblFahrgestellnummerShow.Text = row("ZZFAHRG").ToString

        lblOrdernummerShow.Text = row("ZZREF1").ToString

        lblErstzulassungsdatumShow.Text = FormatToDate(row("REPLA_DATE").ToString)

        lblAbmeldedatumShow.Text = FormatToDate(row("EXPIRY_DATE").ToString)

        If row("ZZSTATUS_ZUB").ToString = "X" Then
            lblStatusShow.Text = "Zulassungsfähig"
        End If
        If row("ZZSTATUS_ZUL").ToString = "X" Then
            lblStatusShow.Text = "Zugelassen"
        End If
        If row("ZZSTATUS_ABG").ToString = "X" Then
            lblStatusShow.Text = "Abgemeldet"
        End If
        If row("ZZSTATUS_BAG").ToString = "X" Then
            lblStatusShow.Text = "Bei Abmeldung"
        End If
        If row("ZZSTATUS_OZU").ToString = "X" Then
            lblStatusShow.Text = "Ohne Zulassung"
        End If
        If row("ZZAKTSPERRE").ToString = "X" Then
            lblStatusShow.Text = "Gesperrt"
        End If

        If row("ZZCOCKZ").ToString = "X" Then   'COC?
            cbxCOC.Checked = True
        End If
        lblStandortShow.Text = row("NAME1_VS").ToString
        If row("NAME2_VS").ToString.Length > 0 Then
            lblStandortShow.Text &= "<br>" & row("NAME2_VS").ToString
        End If
        lblStandortShow.Text &= "<br>"
        If row("STRAS_VS").ToString.Length > 0 Then
            lblStandortShow.Text &= row("STRAS_VS").ToString
        End If
        If row("HSNR_VS").ToString.Length > 0 Then
            lblStandortShow.Text &= " " & row("HSNR_VS").ToString
        End If
        lblStandortShow.Text &= "<br>"
        If row("PSTLZ_VS").ToString.Length > 0 Then
            lblStandortShow.Text &= row("PSTLZ_VS").ToString
        End If
        If row("ORT01_VS").ToString.Length > 0 Then
            lblStandortShow.Text &= " " & row("ORT01_VS").ToString
        End If
        lblFahrzeughalterShow.Text = row("NAME1_ZH").ToString
        If row("NAME2_ZH").ToString.Length > 0 Then
            lblFahrzeughalterShow.Text &= "<br>" & row("NAME2_ZH").ToString
        End If
        lblFahrzeughalterShow.Text &= "<br>"
        If row("STRAS_ZH").ToString.Length > 0 Then
            lblFahrzeughalterShow.Text &= row("STRAS_ZH").ToString
        End If
        If row("HSNR_ZH").ToString.Length > 0 Then
            lblFahrzeughalterShow.Text &= " " & row("HSNR_ZH").ToString
        End If
        lblFahrzeughalterShow.Text &= "<br>"
        If row("PSTLZ_ZH").ToString.Length > 0 Then
            lblFahrzeughalterShow.Text &= row("PSTLZ_ZH").ToString
        End If
        If row("ORT01_ZH").ToString.Length > 0 Then
            lblFahrzeughalterShow.Text &= " " & row("ORT01_ZH").ToString
        End If
        If row("ZZFINART_TXT").ToString.Length > 0 Then
            lblFinanzierungsartShow.Text = row("ZZFINART_TXT").ToString
        End If

        If row("KUNNR_ZF").ToString.Length > 0 Then
            lblHaendlernrShow.Text = row("KUNNR_ZF").ToString
        End If

        lblLagerortShow.Text = row("STANDORT_VERSSTAT_TEXT").ToString

        lblUmgemeldetAmShow.Text = FormatToDate(row("UDATE").ToString)

        lblFahrzeugmodellShow.Text = row("ZZHANDELSNAME").ToString

        lblHerstellerShow.Text = row("ZZHERST_TEXT").ToString
        lblHerstellerSchluesselShow.Text = row("ZZHERSTELLER_SCH").ToString
        lblTypschluesselShow.Text = row("ZZTYP_SCHL").ToString
        lblVarianteVersionShow.Text = row("ZZVVS_SCHLUESSEL").ToString

        'Bemerkungen in die Übersicht eintragen

        If Not Session("AnzBemerkungen") Is Nothing Then
            lblAnzBemerkungenShow.Text = CType(Session("AnzBemerkungen"), String)
        End If


        Dim GetVersandgrund As New TempZuEndg(m_User, m_App, Session("AppID").ToString, Session.SessionID.ToString, "")

        If Len(row("ZZVGRUND").ToString) > 0 Then
            lblVersandgrundShow.Text = GetVersandgrund.Abrufgrund(row("ZZVGRUND").ToString).ToString
        End If
        lblRef1Show.Text = row("ZZREFERENZ1").ToString
        lblRef2Show.Text = row("ZZREFERENZ2").ToString

        lblCarportEingangShow.Text = FormatToDate(row("ZZCARPORT_EING").ToString())
        lblKennzeichenEingangShow.Text = FormatToDate(row("ZZKENN_EING").ToString())
        lblCheckInShow.Text = FormatToDate(row("CHECK_IN").ToString())
        cbxFahrzeugschein.Checked = (row("SCHEIN_PHY").ToString() = "X")
        cbxBeideKennzVorhanden.Checked = (row("SCHILDER_PHY").ToString() = "X")
        lblStilllegungShow.Text = FormatToDate(row("EXPIRY_DATE").ToString())

    End Sub

    Private Function FormatToDate(inStr As String) As String

        Dim outStr As String = ""

        If IsDate(inStr) Then
            outStr = CDate(inStr).ToShortDateString
        End If

        Return outStr
    End Function

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
                objPDIs.FillDatenABE(Session("AppID").ToString, Session.SessionID.ToString, Page, CType(BRIEFLEBENSLAUF_LPTable.Rows(0)("EQUNR"), String))
                If objPDIs.Status = 0 Then
                    With objPDIs.ABE_Daten
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
            Else
                DivPlaceholder.Visible = True
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

    Private Sub FillGrid3(ByVal intPageIndex As Int32, Optional ByVal strSort As String = "")
        If Not (Session("LLSCHDATENTable") Is Nothing) Then
            LLSCHDATENTable = CType(Session("LLSCHDATENTable"), DataTable)

            If LLSCHDATENTable.Rows.Count > 0 Then
                Dim tmpDataView As DataView = LLSCHDATENTable.DefaultView

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

                Datagrid3.CurrentPageIndex = intTempPageIndex
                Datagrid3.DataSource = tmpDataView

                Datagrid3.DataBind()

                If Datagrid3.PageCount > 1 Then
                    Datagrid3.PagerStyle.CssClass = "PagerStyle"
                    Datagrid3.DataBind()
                    Datagrid3.PagerStyle.Visible = True
                Else
                    Datagrid3.PagerStyle.Visible = False
                End If

                Dim item As DataGridItem
                Dim cell As TableCell
                Dim control As Control
                Dim label As Label
                Dim literal As Literal
                Dim text As String

                For Each item In Datagrid3.Items
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

    Private Sub FillHaendleradresse()

        Dim TempTable As DataTable

        TempTable = CType(Session("GT_ADDR"), DataTable)

        Dim TempRow As DataRow() = TempTable.Select("ADDRTYP = 'ZF'")

        If Not TempRow Is Nothing Then
            If TempRow.Length > 0 Then
                lblHaendleradresseShow.Text = TempRow(0)("Anschrift")
            End If

        End If

    End Sub

    Private Function createLebenslaufTable() As DataTable
        Dim tmpTable As DataTable = CType(Session("QMEL_DATENTable"), DataTable)
        Dim Lebenslauf As New DataTable

        Lebenslauf.Columns.Add("Vorgang", Type.GetType("System.String"))
        Lebenslauf.Columns.Add("Durchführungsdatum", Type.GetType("System.String"))
        Lebenslauf.Columns.Add("Versandadresse", Type.GetType("System.String"))
        Lebenslauf.Columns.Add("Versandart", Type.GetType("System.String"))
        Lebenslauf.Columns.Add("Beauftragt durch", Type.GetType("System.String"))

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

        Uebersicht.Columns.Add("Aktionscode", Type.GetType("System.String"))
        Uebersicht.Columns.Add("Vorgang", Type.GetType("System.String"))
        Uebersicht.Columns.Add("Statusdatum", Type.GetType("System.String"))
        Uebersicht.Columns.Add("Übermittlungsdatum", Type.GetType("System.String"))

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

#End Region

End Class
