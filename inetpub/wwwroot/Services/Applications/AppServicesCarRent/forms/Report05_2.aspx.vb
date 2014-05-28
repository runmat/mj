Imports CKG
Imports CKG.Base.Kernel
Imports CKG.Base.Kernel.Common.Common
Imports CKG.Services
Imports CKG.Base.Business
Imports CKG.EasyAccess
Imports CKG.Base.Kernel.DocumentGeneration

Partial Public Class Report05_2
    Inherits Page
#Region "Declarations"
    Private _app As Security.App
    Private _user As Security.User
    Private _historyTable As DataTable
    Private QMMIDATENTable As DataTable
    Private QMEL_DATENTable As DataTable
    Private QMEL_TueteTable As DataTable
    Private diverseFahrzeuge As DataTable
    Private _abeDaten As ABEDaten
    Private _historie As Historie
    Protected ReadOnly Property Links As HistorieLinks
        Get
            Return If(_historie Is Nothing, Nothing, _historie.Links)
        End Get
    End Property

    Protected lastTabName As String = "first"

#End Region
#Region "Events"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        _user = GetUser(Me)
        FormAuth(Me, _user)

        ShowReportHelper.Visible = False

        Try
            _app = New Security.App(_user)
            If (Session("AppHistoryTable") Is Nothing) Then
                Response.Redirect(Request.UrlReferrer.ToString & "?AppID=" & Session("AppID").ToString)
            Else
                _historyTable = CType(Session("AppHistoryTable"), DataTable)
            End If

            lblHead.Text = _user.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString

            If Not IsPostBack Then
                ' ITA 5323 MBB Ausblenden von Schlüßelinfo
                If Me._user.KUNNR = 301270 Then
                    Me.Last.Visible = False
                    Me.HistTabPanel5.Visible = False

                End If

                'Fülle Übersicht
                '&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&
                FillUebersicht()

                'Fülle Typdaten
                '&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&
                FillABEDaten()

                'Fülle Lebenslauf
                '&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&
                FillGridLebenslauf(0)

                'Fülle Schlüsselinformationen
                '&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&
                FillKeyInfo()

                'Fülle Archiv Dokumente
                '&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&
                FillDokumente()


            End If
        Catch ex As Exception
            lblError.Text = "Beim Laden der Seite ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
        End Try
    End Sub

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender

        SetEndASPXAccess(Me)
        CKG.Base.Business.HelpProcedures.FixedGridViewCols(Datagrid2)

        ClientScript.RegisterClientScriptBlock(Me.GetType(), "IsOptArc", "var lastTab =  '" + lastTabName + "';", True)
        lastTabName = "first"

        'If isOptArcClicked Then
        '    ClientScript.RegisterClientScriptBlock(Me.GetType(), "IsOptArc", "var isOptArc = true;", True)
        '    isOptArcClicked = False
        'Else
        '    ClientScript.RegisterClientScriptBlock(Me.GetType(), "IsOptArc", "var isOptArc = false;", True)
        '    isOptArcClicked = False
        'End If

    End Sub

    Private Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Unload
        SetEndASPXAccess(Me)
    End Sub

    Protected Sub OnFahrgestellnummerClick(sender As Object, e As EventArgs) Handles lbFahrgestellnummerShow.Click
        Dim qe As New QuickEasy.Documents(".1001=" & lbFahrgestellnummerShow.Text,
                ConfigurationManager.AppSettings("EasyRemoteHosts"),
                60, ConfigurationManager.AppSettings("EasySessionId"),
                ConfigurationManager.AppSettings("ExcelPath"),
                "C:\TEMP",
                "SYSTEM",
                ConfigurationManager.AppSettings("EasyPwdClear"),
                "C:\TEMP",
                "VWR",
                "VWR",
                "SGW")

        qe.GetDocument()

        If qe.ReturnStatus = 2 Then
            Response.Clear()
            Response.ContentType = "application/pdf"
            Response.Headers.Add("Content-Disposition", "attachment; filename=document.pdf")

            'Write the file directly to the HTTP output stream.
            Response.WriteFile(qe.path)
            Response.End()
        End If

    End Sub
#End Region

#Region "Methods"

    Private Sub FillUebersicht()
        lblKennzeichenShow.Text = _historyTable.Rows(0)("ZZKENN").ToString
        lblEhemaligesKennzeichenShow.Text = _historyTable.Rows(0)("ZZKENN_OLD").ToString
        lblBriefnummerShow.Text = _historyTable.Rows(0)("ZZBRIEF").ToString
        lblEhemaligeBriefnummerShow.Text = _historyTable.Rows(0)("ZZBRIEF_OLD").ToString
        If _historyTable.Rows(0)("ZZBRIEF_A").ToString = "X" Then
            chkBriefaufbietung.Checked = True
        Else
            chkBriefaufbietung.Checked = False
        End If
        lbFahrgestellnummerShow.Text = _historyTable.Rows(0)("ZZFAHRG").ToString
        'lnkSchluesselinformationen.NavigateUrl &= lblFahrgestellnummerShow.Text


        lblErstzulassungsdatumShow.Text = MakeStandardDateString(_historyTable.Rows(0)("REPLA_DATE").ToString)
        lblAbmeldedatumShow.Text = MakeStandardDateString(_historyTable.Rows(0)("EXPIRY_DATE").ToString)

        If _historyTable(0)("ZZSTATUS_ZUB").ToString = "X" Then
            lblStatusShow.Text = "Zulassungsfähig"
        End If
        If _historyTable.Rows(0)("ZZSTATUS_ZUL").ToString = "X" Then
            lblStatusShow.Text = "Zugelassen"
        End If
        If _historyTable.Rows(0)("ZZSTATUS_ABG").ToString = "X" Then
            lblStatusShow.Text = "Abgemeldet"
        End If
        If _historyTable.Rows(0)("ZZSTATUS_BAG").ToString = "X" Then
            lblStatusShow.Text = "Bei Abmeldung"
        End If
        If _historyTable.Rows(0)("ZZSTATUS_OZU").ToString = "X" Then
            lblStatusShow.Text = "Ohne Zulassung"
        End If
        If _historyTable.Rows(0)("ZZAKTSPERRE").ToString = "X" Then
            lblStatusShow.Text = "Gesperrt"
        End If

        If _historyTable.Rows(0)("ZZCOCKZ").ToString = "X" Then   'COC?
            cbxCOC.Checked = True
        End If
        lblStandortShow.Text = _historyTable.Rows(0)("NAME1_VS").ToString
        If _historyTable.Rows(0)("NAME2_VS").ToString.Length > 0 Then
            lblStandortShow.Text &= "<br>" & _historyTable.Rows(0)("NAME2_VS").ToString
        End If
        lblStandortShow.Text &= "<br>"
        If _historyTable.Rows(0)("STRAS_VS").ToString.Length > 0 Then
            lblStandortShow.Text &= _historyTable.Rows(0)("STRAS_VS").ToString
        End If
        If _historyTable.Rows(0)("HSNR_VS").ToString.Length > 0 Then
            lblStandortShow.Text &= " " & _historyTable.Rows(0)("HSNR_VS").ToString
        End If
        lblStandortShow.Text &= "<br>"
        If _historyTable.Rows(0)("PSTLZ_VS").ToString.Length > 0 Then
            lblStandortShow.Text &= _historyTable.Rows(0)("PSTLZ_VS").ToString
        End If
        If _historyTable.Rows(0)("ORT01_VS").ToString.Length > 0 Then
            lblStandortShow.Text &= " " & _historyTable.Rows(0)("ORT01_VS").ToString
        End If
        lblFahrzeughalterShow.Text = _historyTable.Rows(0)("NAME1_ZH").ToString
        If _historyTable.Rows(0)("NAME2_ZH").ToString.Length > 0 Then
            lblFahrzeughalterShow.Text &= "<br>" & _historyTable.Rows(0)("NAME2_ZH").ToString
        End If
        lblFahrzeughalterShow.Text &= "<br>"
        If _historyTable.Rows(0)("STRAS_ZH").ToString.Length > 0 Then
            lblFahrzeughalterShow.Text &= _historyTable.Rows(0)("STRAS_ZH").ToString
        End If
        If _historyTable.Rows(0)("HSNR_ZH").ToString.Length > 0 Then
            lblFahrzeughalterShow.Text &= " " & _historyTable.Rows(0)("HSNR_ZH").ToString
        End If
        lblFahrzeughalterShow.Text &= "<br>"
        If _historyTable.Rows(0)("PSTLZ_ZH").ToString.Length > 0 Then
            lblFahrzeughalterShow.Text &= _historyTable.Rows(0)("PSTLZ_ZH").ToString
        End If
        If _historyTable.Rows(0)("ORT01_ZH").ToString.Length > 0 Then
            lblFahrzeughalterShow.Text &= " " & _historyTable.Rows(0)("ORT01_ZH").ToString
        End If

        Select Case _historyTable.Rows(0)("ABCKZ").ToString
            Case ""
                lblLagerortShow.Text = "DAD"
            Case "0"
                lblLagerortShow.Text = "DAD"
            Case "1"
                If _historyTable.Rows(0)("ZZTMPDT").ToString = "" OrElse _historyTable.Rows(0)("ZZTMPDT").ToString = "00000000" Then
                    lblLagerortShow.Text = "temporär angefordert"
                Else
                    lblLagerortShow.Text = "temporär versendet"
                End If
            Case "2"
                If _historyTable.Rows(0)("ZZTMPDT").ToString = "" OrElse _historyTable.Rows(0)("ZZTMPDT").ToString = "00000000" Then
                    lblLagerortShow.Text = "endgültig angefordert"
                Else
                    lblLagerortShow.Text = "endgültig versendet"
                End If
        End Select

        lblUmgemeldetAmShow.Text = MakeStandardDateString(_historyTable.Rows(0)("UDATE").ToString)


        'Bemerkungen in die Übersicht eintragen
        'If Not Session("AnzBemerkungen") Is Nothing Then
        '    lblAnzBemerkungenShow.Text = CType(Session("AnzBemerkungen"), String)
        'End If


        Dim mReport As Historie
        mReport = CType(Session("objReport"), Historie)

        If Len(_historyTable.Rows(0)("ZZVGRUND").ToString) > 0 Then
            ' lblVersandgrundShow.Text = mReport.Abrufgrund(_historyTable.Rows(0)("ZZVGRUND").ToString).ToString
        End If
        If Not (Session("AppDiverseFahrzeuge") Is Nothing) Then
            diverseFahrzeuge = CType(Session("AppDiverseFahrzeuge"), DataTable)
            If diverseFahrzeuge.Rows.Count > 0 Then
                Dim datRow() As DataRow
                datRow = diverseFahrzeuge.Select("ZZFAHRG='" & lbFahrgestellnummerShow.Text & "'")
                If datRow.Length = 1 Then
                    lblOrdernummerShow.Text = datRow(0)("ZZREFERENZ1").ToString
                End If

            End If
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

        _abeDaten = New ABEDaten(_user, _app, Session("AppID").ToString, Session.SessionID.ToString, "", _user.KUNNR, "ZDAD", "", "")

        If Not _abeDaten Is Nothing Then
            If _historyTable.Rows(0)("EQUNR") Is Nothing OrElse CType(_historyTable.Rows(0)("EQUNR"), String).Trim(" "c).Length = 0 Then
                lblError.Text = "Fehler: Die Daten enthalten keine Fahrzeugnummer."
            Else

                _abeDaten.FillDatenABE(Session("AppID").ToString, Session.SessionID.ToString, Me, CType(_historyTable.Rows(0)("EQUNR"), String))
                If _abeDaten.Status = 0 Then
                    With _abeDaten.ABE_Daten
                        'lbl_00.Text = .Farbziffer

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
                            Case Else

                        End Select
                        Session.Add("App_objPDIs", _abeDaten)
                    End With
                Else
                    lblError.Text = _abeDaten.Message
                End If
            End If
        End If
    End Sub

    Private Sub FillGridTuete(ByVal intPageIndex As Int32, Optional ByVal strSort As String = "")
        If Not (Session("AppQMEL_TueteTable") Is Nothing) Then
            QMEL_TueteTable = CType(Session("AppQMEL_TueteTable"), DataTable)

            If QMEL_TueteTable.Rows.Count > 0 Then
                Dim tmpDataView As New DataView()
                tmpDataView = QMEL_TueteTable.DefaultView

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

                gv_LebelaufTuete.PageIndex = intTempPageIndex
                gv_LebelaufTuete.DataSource = tmpDataView

                gv_LebelaufTuete.DataBind()

                Dim item As GridViewRow
                Dim cell As TableCell
                Dim control As Control
                Dim label As Label
                Dim literal As Literal
                Dim text As String

                For Each item In gv_LebelaufTuete.Rows
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

    Private Sub FillGridLebenslauf(ByVal intPageIndex As Int32, Optional ByVal strSort As String = "")
        If Not (Session("AppQMEL_DATENTable") Is Nothing) Then
            QMEL_DATENTable = CType(Session("AppQMEL_DATENTable"), DataTable)

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

                Datagrid2.PageIndex = intTempPageIndex

                Datagrid2.DataSource = tmpDataView
                Datagrid2.DataBind()



                Dim item As GridViewRow
                Dim cell As TableCell
                Dim control As Control
                Dim label As Label
                Dim literal As Literal
                Dim text As String

                For Each item In Datagrid2.Rows
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

    Private Sub FillKeyInfo()
        Dim strFahrg As String = lbFahrgestellnummerShow.Text
        If Not Session("objReport") Is Nothing Then
            _historie = CType(Session("objReport"), Historie)
            _historie.Fill(Session("AppID").ToString, Session.SessionID.ToString, Me, strFahrg)
            With _historie

                If Not .Result Is Nothing AndAlso Not .Result.Rows Is Nothing AndAlso .Result.Rows.Count > 0 Then
                    'Allgemein
                    lblFahrgestellnummer.Text = .Result.Rows(0)("CHASSIS_NUM").ToString

                    lblEingangSchluessel.Text = MakeStandardDateString(.Result.Rows(0)("ERDAT").ToString)

                    If .Result.Rows(0)("ABCKZ").ToString = "1" Then
                        rbTemporaer.Checked = True
                        rbEndgueltig.Checked = False
                    ElseIf .Result.Rows(0)("ABCKZ").ToString = "2" Then
                        rbTemporaer.Checked = False
                        rbEndgueltig.Checked = True
                    Else
                        rbTemporaer.Checked = False
                        rbEndgueltig.Checked = False
                    End If
                    'Letzter Versanddaten
                    'Datum
                    lblAngefordertAm.Text = MakeStandardDateString(.Result.Rows(0)("ZZTMPDT").ToString)
                    'Versandanschrift
                    lblVersandanschrift.Text = .Result.Rows(0)("NAME1_VS").ToString
                    If .Result.Rows(0)("NAME2_VS").ToString.Length > 0 Then
                        lblVersandanschrift.Text &= "<br>" & .Result.Rows(0)("NAME2_VS").ToString
                    End If
                    lblVersandanschrift.Text &= "<br>"
                    If .Result.Rows(0)("STRAS_VS").ToString.Length > 0 Then
                        lblVersandanschrift.Text &= .Result.Rows(0)("STRAS_VS").ToString
                    End If
                    If .Result.Rows(0)("HSNR_VS").ToString.Length > 0 Then
                        lblVersandanschrift.Text &= " " & .Result.Rows(0)("HSNR_VS").ToString
                    End If
                    lblVersandanschrift.Text &= "<br>"
                    If .Result.Rows(0)("PSTLZ_VS").ToString.Length > 0 Then
                        lblVersandanschrift.Text &= .Result.Rows(0)("PSTLZ_VS").ToString
                    End If
                    If .Result.Rows(0)("ORT01_VS").ToString.Length > 0 Then
                        lblVersandanschrift.Text &= " " & .Result.Rows(0)("ORT01_VS").ToString
                    End If
                    If Not .QMEL_TueteTable Is Nothing AndAlso Not .QMEL_TueteTable.Rows Is Nothing AndAlso .QMEL_TueteTable.Rows.Count > 0 Then
                        Session("AppQMEL_TueteTable") = .QMEL_TueteTable
                        FillGridTuete(0)
                    Else
                        gv_LebelaufTuete.Visible = False
                    End If
                End If
            End With
        End If


    End Sub

    Private Function MakeStandardDateString(ByVal strSAPDate As String) As String
        If IsDate(strSAPDate) Then
            Return FormatDateTime(CDate(strSAPDate), DateFormat.ShortDate)
        Else
            Return ""
        End If

    End Function

#End Region

    Private Sub lbCreatePDF_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lbCreatePDF.Click
        Try
            Dim imageHt As New Hashtable()
            imageHt.Add("Logo", _user.Customer.LogoImage)

            If _abeDaten Is Nothing Then
                _abeDaten = CType(Session("App_objPDIs"), ABEDaten)
            End If

            Dim tblData = createUebersichtTable()

            Dim docFactory = New WordDocumentFactory(tblData, imageHt)

            Dim dataTables(1) As DataTable
            dataTables(0) = createLebenslaufTable()
            dataTables(1) = createUebermittlungsTable()

            docFactory.CreateDocument("Fahrzeughistorie_" & lbFahrgestellnummerShow.Text, Me, "\Applications\AppServicesCarRent\Documents\FahrzeughistoriePrint.doc", "Tabelle", dataTables)
        Catch ex As Exception
            lblError.Text = "Fehler beim Erstellen des Ausdrucks: " + ex.Message
        End Try
    End Sub

    Private Function createUebersichtTable() As DataTable
        Dim uebersicht = New DataTable

        uebersicht.Columns.Add("Fahrgestellnummer", GetType(String))
        uebersicht.Columns.Add("Kennzeichen", GetType(String))
        uebersicht.Columns.Add("Status", GetType(String))
        uebersicht.Columns.Add("Lagerort", GetType(String))

        uebersicht.Columns.Add("Hersteller", GetType(String))
        uebersicht.Columns.Add("Fahrzeugmodell", GetType(String))
        uebersicht.Columns.Add("Farbe", GetType(String))
        uebersicht.Columns.Add("Herstellerschluessel", GetType(String))
        uebersicht.Columns.Add("Typschluessel", GetType(String))
        uebersicht.Columns.Add("VarianteVersion", GetType(String))

        uebersicht.Columns.Add("Eingang", GetType(String))
        uebersicht.Columns.Add("Carport", GetType(String))
        uebersicht.Columns.Add("Bereitdatum", GetType(String))
        uebersicht.Columns.Add("ZBIINummer", GetType(String))

        uebersicht.Columns.Add("Erstzulassungsdatum", GetType(String))
        uebersicht.Columns.Add("Abmeldedatum", GetType(String))
        uebersicht.Columns.Add("Mva_Nummer", GetType(String))

        uebersicht.Columns.Add("FahrzeughalterName1Name2", GetType(String))
        uebersicht.Columns.Add("FahrzeughalterStrasseNummer", GetType(String))
        uebersicht.Columns.Add("FahrzeughalterPLZOrt", GetType(String))
        uebersicht.Columns.Add("VersandadresseName1Name2", GetType(String))
        uebersicht.Columns.Add("VersandadresseStrasseNummer", GetType(String))
        uebersicht.Columns.Add("VersandadressePLZOrt", GetType(String))
        uebersicht.Columns.Add("COC", GetType(String))
        ' ITA 2761 Produktionskennziffer hinzugefügt CDI 20090610
        uebersicht.Columns.Add("Produktionskennziffer", GetType(String))

        uebersicht.Columns.Add("AusdruckDatumUhrzeit", GetType(String))
        uebersicht.Columns.Add("Username", GetType(String))
        uebersicht.Columns.Add("UmgemeldetAm", GetType(String))
        uebersicht.Columns.Add("EhmaligesKennzeichen", GetType(String))
        uebersicht.Columns.Add("ZBIIAufbietung", GetType(String))
        uebersicht.Columns.Add("EhmaligeZBIINummer", GetType(String))
        uebersicht.Columns.Add("EingangSchluessel", GetType(String))
        uebersicht.Columns.Add("Versendet", GetType(String))
        uebersicht.Columns.Add("SchluesselName1Name2", GetType(String))
        uebersicht.Columns.Add("SchluesselStrasseNummer", GetType(String))
        uebersicht.Columns.Add("SchluesselPLZOrt", GetType(String))

        Dim row = uebersicht.NewRow

        row("Fahrgestellnummer") = lbFahrgestellnummerShow.Text
        row("Abmeldedatum") = lblAbmeldedatumShow.Text
        row("Status") = lblStatusShow.Text
        row("Kennzeichen") = lblKennzeichenShow.Text
        row("Lagerort") = lblLagerortShow.Text
        row("Hersteller") = lblHerstellerShow.Text
        row("Fahrzeugmodell") = lblFahrzeugmodellShow.Text
        row("Farbe") = _abeDaten.ABE_Daten.ZZFARBE
        row("Herstellerschluessel") = lblHerstellerSchluesselShow.Text
        row("Typschluessel") = lblTypschluesselShow.Text
        row("VarianteVersion") = lblVarianteVersionShow.Text
        row("ZBIINummer") = lblBriefnummerShow.Text
        row("Erstzulassungsdatum") = lblErstzulassungsdatumShow.Text
        row("Abmeldedatum") = lblAbmeldedatumShow.Text
        ' ITA 2761 Produktionskennziffer hinzugefügt CDI 20090610
        'row("Produktionskennziffer") = lblProduktionskennzifferShow.Text

        'row("Carport") = lblPDI.Text & " / " & lblPDIName.Text
        row("Mva_Nummer") = lblOrdernummerShow.Text
        'traurig aber wahr, nach string "<br>" splittet er nicht richtig JJU2008.07.01
        row("FahrzeughalterName1Name2") = lblFahrzeughalterShow.Text.Replace("<br>", ";").Split(CChar(CStr(";")))(0)
        row("FahrzeughalterStrasseNummer") = lblFahrzeughalterShow.Text.Replace("<br>", ";").Split(CChar(CStr(";")))(1)
        row("FahrzeughalterPLZOrt") = lblFahrzeughalterShow.Text.Replace("<br>", ";").Split(CChar(CStr(";")))(2)
        row("VersandadresseName1Name2") = lblStandortShow.Text.Replace("<br>", ";").Split(CChar(CStr(";")))(0)
        row("VersandadresseStrasseNummer") = lblStandortShow.Text.Replace("<br>", ";").Split(CChar(CStr(";")))(1)
        row("VersandadressePLZOrt") = lblStandortShow.Text.Replace("<br>", ";").Split(CChar(CStr(";")))(2)
        row("AusdruckDatumUhrzeit") = Date.Now.ToShortDateString & " / " & Date.Now.ToShortTimeString
        row("Username") = _user.UserName
        'row("Eingang") = lblEingangsdatum.Text
        'row("Bereitdatum") = lblBereitdatum.Text
        'row("EingangSchluessel") = lblEingangSchluessel.Text
        'row("Versendet") = lblAngefordertAm.Text


        'If Not lblVersandanschrift.Text Is "" Then
        'row("SchluesselName1Name2") = lblVersandanschrift.Text.Replace("<br>", ";").Split(";"c)(0)
        'row("SchluesselStrasseNummer") = lblVersandanschrift.Text.Replace("<br>", ";").Split(";"c)(1)
        'row("SchluesselPLZOrt") = lblVersandanschrift.Text.Replace("<br>", ";").Split(";"c)(2)
        'End If

        If cbxCOC.Checked = True Then
            row("COC") = "Ja"
        Else
            row("COC") = "Nein"
        End If
        row("UmgemeldetAM") = lblUmgemeldetAmShow.Text
        row("EhmaligesKennzeichen") = lblEhemaligesKennzeichenShow.Text
        If chkBriefaufbietung.Checked = True Then
            row("ZBIIAufbietung") = "Ja"
        Else
            row("ZBIIAufbietung") = "Nein"
        End If
        row("EhmaligeZBIINummer") = lblEhemaligeBriefnummerShow.Text

        uebersicht.Rows.Add(row)
        Return uebersicht

    End Function

    Private Function createLebenslaufTable() As DataTable
        Dim lebenslauf = New DataTable

        lebenslauf.Columns.Add("Vorgang", GetType(String))
        lebenslauf.Columns.Add("Komponentenbezeichnung", GetType(String))
        lebenslauf.Columns.Add("Durchführungsdatum", GetType(String))
        lebenslauf.Columns.Add("Versandadresse", GetType(String))
        lebenslauf.Columns.Add("Versandart", GetType(String))
        lebenslauf.Columns.Add("Beauftragt durch", GetType(String))

        Dim tmpTable As DataTable = CType(Session("AppQMEL_DATENTable"), DataTable)
        If Not tmpTable Is Nothing Then
            For Each row In tmpTable.Rows

                Dim newRow = lebenslauf.NewRow()

                newRow.Item("Vorgang") = row("KURZTEXT").ToString
                newRow.Item("Komponentenbezeichnung") = row("BAUTL").ToString
                newRow.Item("Durchführungsdatum") = row("STRMN").ToString
                newRow.Item("Versandadresse") = row("NAME1_Z5").ToString & " " & row("NAME2_Z5").ToString & " / " & row("STREET_Z5").ToString & " " & row("HOUSE_NUM1_Z5").ToString & " / " & row("POST_CODE1_Z5").ToString & " " & row("CITY1_Z5").ToString
                newRow.Item("Versandart") = row("ZZDIEN1").ToString
                newRow.Item("Beauftragt durch") = row("ERNAM").ToString

                lebenslauf.Rows.Add(newRow)
                lebenslauf.AcceptChanges()
            Next
        End If
        lebenslauf.TableName = "Lebenslauf"
        Return lebenslauf
    End Function

    Private Function createUebermittlungsTable() As DataTable
        Dim uebersicht As New DataTable
        uebersicht.Columns.Add("Vorgang", GetType(String))
        uebersicht.Columns.Add("Komponentenbezeichnung", GetType(String))
        uebersicht.Columns.Add("Durchführungsdatum", GetType(String))
        uebersicht.Columns.Add("Versandadresse", GetType(String))
        uebersicht.Columns.Add("Versandart", GetType(String))
        uebersicht.Columns.Add("Beauftragt durch", GetType(String))

        Dim tueteTable As DataTable = CType(Session("AppQMEL_TueteTable"), DataTable)
        If Not tueteTable Is Nothing Then
            For Each row In tueteTable.Rows

                Dim newRow = uebersicht.NewRow()

                newRow.Item("Vorgang") = row("KURZTEXT").ToString
                newRow.Item("Komponentenbezeichnung") = row("BAUTL").ToString
                newRow.Item("Durchführungsdatum") = row("STRMN").ToString
                newRow.Item("Versandadresse") = row("NAME1_Z5").ToString & " " & row("NAME2_Z5").ToString & " / " & row("STREET_Z5").ToString & " " & row("HOUSE_NUM1_Z5").ToString & " / " & row("POST_CODE1_Z5").ToString & " " & row("CITY1_Z5").ToString
                newRow.Item("Versandart") = row("ZZDIEN1").ToString
                newRow.Item("Beauftragt durch") = row("ERNAM").ToString

                uebersicht.Rows.Add(newRow)
                uebersicht.AcceptChanges()
            Next
        End If

        uebersicht.TableName = "Lebenslauf Schlüssel"
        Return uebersicht
    End Function

    Private Sub FillDokumente()
        _historie = CType(Session("objReport"), Historie)
        _historie.FillLinks(lbFahrgestellnummerShow.Text, Session("AppID").ToString, Session.SessionID.ToString, Me)
        'If _historie.Links Is Nothing Then
        '    ' auskommentiert weil Archivzugriff nicht sauber ITA 5680
        '    'ibtnShowDokument.Visible = False
        'End If

        If _historie.ResultOptArchiv Is Nothing Then
            'Fourth.Visible = False
            HistTabPanel4.Disabled = True
        Else
            If _historie.ResultOptArchiv.Rows.Count <= 0 Then
                HistTabPanel4.Disabled = True
            Else
                HistTabPanel4.Disabled = False
                'Füllen der Listbox

                Session("OptArcTable") = _historie.ResultOptArchiv
                FillGrid("OptArcTable", gvArchiv, 0, "")
                'gvArchiv.DataSource = _historie.ResultOptArchiv
                'gvArchiv.DataBind()
            End If
        End If
    End Sub

    '' # auskommentiert weil Archivzugriff nicht sauber ITA 5680
    'Private Sub ibtnShowDokument_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles ibtnShowDokument.Click
    '    _historie = CType(Session("objReport"), Historie)
    '    If Links Is Nothing Then
    '        Return
    '    End If
    '    Links.OpenZBII(ShowReportHelper)
    'End Sub

    Private Sub lbBack_Click(sender As Object, e As EventArgs) Handles lbBack.Click
        Response.Redirect("Report05.aspx?AppID=" & Session("AppID").ToString())
    End Sub

    Private Sub gvArchiv_RowCommand(ByVal sender As Object, ByVal e As GridViewCommandEventArgs) Handles gvArchiv.RowCommand
        Try
            If e.CommandName = "download" Then
                Dim b = CType(e.CommandSource, ImageButton)
                Dim index = Convert.ToInt32(b.CommandArgument)
                Dim row = gvArchiv.Rows(index)

                lastTabName = "fourth"

                Dim lbl As String = row.Cells(2).Text

                If _historie Is Nothing Then
                    _historie = CType(Session("objReport"), Historie)
                End If

                If Links Is Nothing Then
                    Return
                End If


                Dim FiletypeFilter As String = lbl.ToUpper()

                Dim k As Integer = -1
                For k = 0 To Links.FileList.GetUpperBound(0)
                    If (Links.FileList(k, 3) = FiletypeFilter) Then
                        Links.DownloadSingleFile(ShowReportHelper, Links.FileList(k, 2), Links.FileList(k, 0), Links.FileList(k, 1))
                    End If
                Next


            End If

        Catch ex As Exception
            lblError.Text = ex.Message
        End Try
    End Sub

    Private Sub FillGrid(ByVal sessionKey As String, ByRef gv As GridView, pageIndex As Integer, Optional ByVal sort As String = "")

        Dim dataTable = CType(Session(sessionKey), DataTable)
        Dim view = dataTable.DefaultView

        view.RowFilter = ""

        Dim tempSort = ""
        Dim direction = ""
        Dim tempPageIndex = pageIndex


        If sort.Trim(" "c).Length > 0 Then
            tempSort = sort.Trim(" "c)
            If (ViewState("Sort") Is Nothing) OrElse (ViewState("Sort").ToString = tempSort) Then
                If ViewState("Direction") Is Nothing Then
                    direction = "desc"
                Else
                    direction = ViewState("Direction").ToString
                End If
            Else
                direction = "desc"
            End If

            If direction = "asc" Then
                direction = "desc"
            Else
                direction = "asc"
            End If

            ViewState("Sort") = tempSort
            ViewState("Direction") = direction
        Else
            If Not ViewState("Sort") Is Nothing Then
                tempSort = ViewState("Sort").ToString
                If ViewState("Direction") Is Nothing Then
                    direction = "asc"
                    ViewState("Direction") = direction
                Else
                    direction = ViewState("Direction").ToString
                End If
            End If
        End If

        If Not tempSort.Length = 0 Then
            view.Sort = tempSort & " " & direction
        End If

        gv.PageIndex = tempPageIndex
        gv.DataSource = view
        gv.DataBind()

    End Sub


    Protected Sub gvArchiv_Sorting(sender As Object, e As GridViewSortEventArgs) Handles gvArchiv.Sorting
        FillGrid("OptArcTable", gvArchiv, gvArchiv.PageIndex, e.SortExpression)
        lastTabName = "fourth"

    End Sub

    Protected Sub Datagrid2_Sorting(sender As Object, e As GridViewSortEventArgs) Handles Datagrid2.Sorting
        FillGridLebenslauf(Datagrid2.PageIndex, e.SortExpression)
        lastTabName = "third"
    End Sub

    Protected Sub gv_LebelaufTuete_Sorting(sender As Object, e As GridViewSortEventArgs) Handles gv_LebelaufTuete.Sorting
        FillGridTuete(gv_LebelaufTuete.PageIndex, e.SortExpression)
        lastTabName = "last"
    End Sub

    Protected Sub ibtNewSearch_Click(sender As Object, e As ImageClickEventArgs) Handles ibtNewSearch.Click
        Response.Redirect("./Report05.aspx?AppID=" & Session("AppID").ToString())
    End Sub
End Class
