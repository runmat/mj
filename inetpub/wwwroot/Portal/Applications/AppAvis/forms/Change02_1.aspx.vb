Imports CKG.Base.Business
Imports CKG.Base.Kernel
Imports CKG.Base.Kernel.Common.Common
Imports CKG.Portal.PageElements

Partial Public Class Change02_1
    Inherits System.Web.UI.Page
    Private m_User As Base.Kernel.Security.User
    Private m_App As Base.Kernel.Security.App
    Private m_objTable As DataTable
    Private m_objExcel As DataTable
    Private m_report As Zul_Sperr_Entsperr
    Private m_blnDateFormatError As Boolean
    Private _mblnDateErrorMessage As String
    Private _mblnDateError As Boolean
    Private m_blnIsWeekEnd As Boolean

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Session("ShowLink") = "False"
        m_User = GetUser(Me)
        ucHeader.InitUser(m_User)
        FormAuth(Me, m_User)

        GetAppIDFromQueryString(Me)
        lnkKreditlimit.NavigateUrl = "Change02.aspx?AppID=" & Session("AppID").ToString
        Try
            lblHead.Text = m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString
            ucStyles.TitleText = lblHead.Text
            m_App = New Base.Kernel.Security.App(m_User)
            If Not Session("App_Report") Is Nothing Then
                m_report = CType(Session("App_Report"), Zul_Sperr_Entsperr)
                Session("App_ResultTablePDIs") = m_report.ResultPDIs
                Session("App_ResultTable") = m_report.ResultTable
                Session("App_Result") = m_report.Result
            End If


            If Not IsPostBack Then
                m_report.Show()
                FillGrid(-1)
            Else

                Dim blnInputPerformed As Boolean
                If Request.Form("__EVENTTARGET").ToString = "cmdAbsenden" Then
                    blnInputPerformed = False
                ElseIf Request.Form("__EVENTTARGET").ToString = "cmdWeitereAuswahl" Then

                    Dim sFilter As String
                    sFilter = m_report.FilterString
                    Dim strFileName As String = Format(Now, "yyyyMMdd_HHmmss_") & m_User.UserName & ".xls"
                    m_report = New Zul_Sperr_Entsperr(m_User, m_App, strFileName)
                    m_report.Fill(Session("AppID").ToString, Session.SessionID.ToString)

                    If Not m_report.Status = 0 Then
                        lblError.Text = m_report.Message
                    Else
                        If Not m_report.Result.Rows.Count = 0 Then

                            Session("App_ResultTablePDIs") = m_report.ResultPDIs
                            Session("App_Report") = m_report
                            Session("App_ResultTable") = m_report.Result

                        End If
                    End If
                    m_report.FilterString = sFilter
                    m_report.FilterCarports()



                End If

                If Not Request.Form.Item("PDINummer").ToString = "empty" Then
                    m_report.Carportnummer = Request.Form.Item("PDINummer").ToString
                    m_report.Show()
                    If m_report.Status = 0 Then
                        FillGrid(CInt(Request.Form.Item("NummerInGrid")))
                    Else
                        lblError.Text = "Beim Laden der Carport-Daten ist ein Fehler aufgetreten.<br>(" & m_report.Message & ")"
                    End If
                End If
                If Not Session("App_ZulDat") Is Nothing Then
                    lblZulDat.Text = Session("App_ZulDat").ToString
                End If
                If blnInputPerformed Then
                    FillGrid(-1)
                End If

            End If
            'If Not Session("App_NewCount") Is Nothing Then
            '    AktuelleAnzahl.Value = (CInt(AktuelleAnzahl.Value) + CInt(Session("App_NewCount"))).ToString
            'End If
            If SelOpen2.Value = "O" Then insertScript()
        Catch ex As Exception
            lblError.Text = "Beim Laden der Seite ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
        End Try
    End Sub

    Protected Sub HG_TemplateSelection(ByVal sender As Object, ByVal e As DBauer.Web.UI.WebControls.HierarGridTemplateSelectionEventArgs) Handles HG1.TemplateSelection
        Select Case (e.Row.Table.TableName)
            Case "Modelle"
                e.TemplateFilename = "..\Templates\Modell.ascx"
            Case Else
                Throw New NotImplementedException("Unexpected child row in TemplateSelection event")
        End Select
    End Sub
    Private Sub FillGrid(ByVal intExpand As Int32, Optional ByVal blnSelectedOnly As Boolean = False)
        'bind the DataSet to the HierarGrid
        If blnSelectedOnly Then
            HG1.DataSource = m_report.CarPort_Data_Selected
        Else
            HG1.DataSource = m_report.CarPort_Data
        End If
        HG1.DataMember = "Carports"
        HG1.DataBind()

        'If objPDIs.Task = "Entsperren" Then
        '    HG1.Columns(5).Visible = False
        'End If

        Dim item As DataGridItem
        Dim cell As TableCell
        Dim control As Control
        Dim checkbox As CheckBox
        Dim intItem As Int32 = 0

        For Each item In HG1.Items
            Dim strPDI_Nummer As String = item.Cells(1).Text
            Dim intValidCounter As Int32
            'If objPDIs.Task = "Entsperren" Then
            '    intValidCounter = CInt(item.Cells(6).Text)
            'Else
            intValidCounter = CInt(item.Cells(4).Text) + CInt(item.Cells(5).Text)
            'End If
            Dim blnDetails As Boolean = False
            Dim blnLoaded As Boolean = False
            cell = item.Cells(1)
            For Each control In cell.Controls
                If TypeOf control Is CheckBox Then
                    checkbox = CType(control, CheckBox)
                    If checkbox.ID = "chkDetails" And checkbox.Checked Then
                        blnDetails = True
                    End If
                    If checkbox.ID = "chkLoaded" And checkbox.Checked Then
                        blnLoaded = True
                    End If
                End If
            Next

            cell = item.Cells(0)
            cell.Width = New Unit("100px")
            cell.HorizontalAlign = HorizontalAlign.Center

            If Not blnDetails Then
                Dim image As New System.Web.UI.WebControls.Image()
                cell.Controls.Add(image)
                image.ImageUrl = "/Portal/Images/empty.gif"
                If intExpand = intItem Then
                    intExpand = -1
                End If
            End If

            If blnSelectedOnly Then
                HG1.RowExpanded(intItem) = True
            End If

            intItem += 1
        Next

        If intExpand > -1 Then
            HG1.RowExpanded(intExpand) = True
        End If

    End Sub
    Private Function CheckInput() As Boolean
        Dim blnReturn As Boolean = False
        Dim datrows() As DataRow
        If IsNothing(m_report) = True Then
            m_report = CType(Session("App_Report"), Zul_Sperr_Entsperr)
        End If

        m_blnDateFormatError = False
        _mblnDateError = False
        _mblnDateErrorMessage = ""

        Try
            Dim i As Int32
            Dim j As Int32
            For i = 0 To Request.Form.Count - 1
                If InStr(Request.Form.Keys.Item(i), "Fahrgestellnummer_") > 0 Then
                    Dim strFahrgestellNummer As String = (Right(Request.Form.Keys.Item(i), 17))
                    Dim strAuswahl_alt As String = CStr(Request.Form(i))
                    Dim Sperre As String = ""
                    Dim Entsperre As String = ""
                    Dim strBemerkung As String = ""
                    Dim intPos As Int32 = InStr(strAuswahl_alt, ",")
                    If intPos > 0 Then
                        strAuswahl_alt = Left(strAuswahl_alt, intPos - 1)
                    End If
                    Dim blnAuswahl_alt As Boolean = CType(strAuswahl_alt, Boolean)
                    Dim blnAuswahl_neu As Boolean = False
                    Dim strDatumErstzulassung As String = ""
                    If i + 1 = Request.Form.Count Then Exit For

                    If UCase(Request.Form(i + 1).ToString) = "ON" And InStr(Request.Form.Keys.Item(i + 1), "chkAuswahl") > 0 Then
                        blnAuswahl_neu = True
                        strDatumErstzulassung = Request.Form(i - 1).ToString
                        datrows = m_report.CarPort_Data.Tables("Fahrzeuge").Select("Fahrgestellnummer='" & strFahrgestellNummer & "'")
                        If datrows.Length > 0 Then
                            Sperre = datrows(0)("neuGesperrt").ToString
                            Entsperre = datrows(0)("neuEntsperrt").ToString
                        End If
                        If Not Sperre = "1" AndAlso Not Entsperre = "1" Then
                            If Not IsDate(strDatumErstzulassung) Then
                                If Not IsStandardDate(strDatumErstzulassung) Then
                                    If Not IsSAPDate(strDatumErstzulassung) Then
                                        m_blnDateFormatError = True
                                    End If
                                End If
                            End If

                            If Not m_blnDateFormatError Then
                                Dim dateZulassung As Date
                                dateZulassung = CType(strDatumErstzulassung, Date)

                                If dateZulassung.DayOfWeek = DayOfWeek.Saturday OrElse dateZulassung.DayOfWeek = DayOfWeek.Sunday Then
                                    'm_blnIsWeekEnd = True
                                    _mblnDateError = True
                                    _mblnDateErrorMessage = "Zulassungsdatum fällt auf einen Samstag oder Sonntag."
                                Else
                                    If m_report.DezZul = True Then
                                        If dateZulassung <= Today.Date Then
                                            _mblnDateError = True
                                            _mblnDateErrorMessage = "Das Zulassungsdatum ist unzulässig, das Datum muss in der Zukunft liegen."
                                        End If
                                    Else
                                        If dateZulassung < Today.Date Then
                                            _mblnDateError = True
                                            _mblnDateErrorMessage = "Das Zulassungsdatum darf nicht in der Vergangenheit liegen."
                                        End If
                                    End If

                                End If

                                

                               

                            End If
                        Else
                            'strBemerkung = Request.Form(i - 1).ToString()
                        End If
                    End If
                    blnReturn = True
                    Dim intModellID As Int32
                    For j = 0 To m_report.CarPort_Data.Tables("Fahrzeuge").Rows.Count - 1
                        If m_report.CarPort_Data.Tables("Fahrzeuge").Rows(j)("Fahrgestellnummer").ToString = strFahrgestellNummer Then
                            m_report.CarPort_Data.Tables("Fahrzeuge").Rows(j)("Ausgewaehlt") = blnAuswahl_neu
                            If IsDate(strDatumErstzulassung) Then
                                m_report.CarPort_Data.Tables("Fahrzeuge").Rows(j)("DatumErstzulassung") = CDate(strDatumErstzulassung)
                            End If
                            If IsDate(strDatumErstzulassung) Then
                                m_report.CarPort_Data.Tables("Fahrzeuge").Rows(j)("DatumErstzulassung") = CDate(strDatumErstzulassung)
                            End If
                            intModellID = CInt(m_report.CarPort_Data.Tables("Fahrzeuge").Rows(j)("Modell_ID"))

                        End If
                    Next
                    For j = 0 To m_report.CarPort_Data.Tables("Modelle").Rows.Count - 1
                        If CInt(m_report.CarPort_Data.Tables("Modelle").Rows(j)("ID")) = intModellID Then
                            If IsDate(strDatumErstzulassung) Then
                                m_report.CarPort_Data.Tables("Modelle").Rows(j)("DatumErstzulassung") = CDate(strDatumErstzulassung)
                            End If
                        End If
                    Next
                    For j = 0 To m_report.CarPort_Data.Tables("Fahrzeuge").Rows.Count - 1
                        If m_report.CarPort_Data.Tables("Fahrzeuge").Rows(j)("Fahrgestellnummer").ToString = strFahrgestellNummer Then
                            m_report.CarPort_Data.Tables("Fahrzeuge").Rows(j)("Ausgewaehlt") = blnAuswahl_neu
                            'If blnAuswahl_neu = True Then
                            '    m_report.CarPort_Data.Tables("Fahrzeuge").Rows(j)("Sperrvermerk") = strBemerkung
                            'End If
                        End If
                    Next

                End If

            Next

            blnReturn = True


        Catch ex As Exception
            lblError.Text = "Fehler bei der Prüfung der Dateneingaben.<br>(" & ex.Message & ")"
            blnReturn = False
        End Try

        Return blnReturn
    End Function

    Protected Sub cmdWeiter_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdWeiter.Click
        CheckInput()
        If m_blnDateFormatError Then
            lblError.Text = "Bitte geben Sie ein gültiges Datum ein (Format: TT.MM.JJJJ oder TTMMJJJJ)."
        Else
            If _mblnDateError = True Then
                lblError.Text = _mblnDateErrorMessage
            End If

            'If m_blnIsWeekEnd Then
            '    lblError.Text = "Zulassungsdatum fällt auf einen Samstag oder Sonntag"
            'End If
        End If
        If (Not m_blnDateFormatError) And (Not _mblnDateError) Then
            If m_report.SelectCars > 0 Then
                FillGrid(-1, True)
                rowSelectionExcel.Visible = False
                cmdWeiter.Visible = False
                cmdAbsenden.Visible = True
                cmdVerwerfen.Visible = True
                cmdZurueck.Visible = False
                DisableAllInput()
                EnableButton()
                'lblMessage.Text = "Aufgabe: " & m_report.Task & ", Anzahl Fahrzeuge: " & m_report.SelectedCars.ToString
            Else
                lblError.Text = "Keine Fahrzeuge ausgewählt."
                FillGrid(-1)
            End If
        End If
    End Sub
    Private Sub DisableAllInput()
        Dim strTemp As String = "			<script language=""JavaScript"">" & vbCrLf
        strTemp &= "		<!-- //" & vbCrLf
        strTemp &= "		  DisableControls();" & vbCrLf
        strTemp &= "		//-->" & vbCrLf
        strTemp &= "			</script>"
        litAddScript.Text = strTemp
        lblPageTitle.Text = "Kontrolle der Fahrzeugauswahl"
    End Sub

    Protected Sub cmdAbsenden_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdAbsenden.Click
        If IsNothing(m_report) = True Then
            m_report = CType(Session("App_Report"), Zul_Sperr_Entsperr)
        End If


        lblPageTitle.Text = "Bestätigung der Datenübernahme"
        'If m_report.DezZul = False Then
        m_report.SaveData(Session("AppID").ToString, Session.SessionID)
        'Else
        'm_report.SaveDataDez(Session("AppID").ToString, Session.SessionID, Me.Page)
        'End If

        If m_report.FehlerCount > 0 Then
            lblMessage.Text = m_report.FehlerCount & " Fahrzeug(e) konnte(n) aufgrund eines Fehlers nicht übernommen werden."
        ElseIf m_report.Message = "" Then
            lblMessage.Text = m_report.SelectedCars.ToString & " Fahrzeuge übernommen."
        End If

        If m_report.SelectCars > 0 Then
            FillGrid(-1, True)
        End If
        lnkCreateExcel2.Visible = True
        lnkCreateExcel.Visible = True
        cmdWeiter.Visible = False
        cmdAbsenden.Visible = False
        cmdVerwerfen.Visible = False
        cmdWeitereAuswahl.Visible = True
        cmdZurueck.Visible = False
        DisableAllInput()
    End Sub

    Protected Sub cmdVerwerfen_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdVerwerfen.Click
        Dim i As Int32
        For i = 0 To m_report.CarPort_Data.Tables("Fahrzeuge").Rows.Count - 1
            m_report.CarPort_Data.Tables("Fahrzeuge").Rows(i)("Ausgewaehlt") = False
            m_report.CarPort_Data.Tables("Fahrzeuge").Rows(i)("neuGesperrt") = ""
            m_report.CarPort_Data.Tables("Fahrzeuge").Rows(i)("neuEntsperrt") = ""
            m_report.CarPort_Data.Tables("Fahrzeuge").Rows(i)("DatumErstzulassung") = DBNull.Value
            Dim sDate As String = m_report.CarPort_Data.Tables("Fahrzeuge").Rows(i)("Datum_zur_Sperre").ToString
            If IsDate(sDate) Then
                m_report.CarPort_Data.Tables("Fahrzeuge").Rows(i)("Datum_zur_Sperre") = CDate(sDate).ToShortDateString
            End If

        Next
        For i = 0 To m_report.CarPort_Data.Tables("Modelle").Rows.Count - 1
            m_report.CarPort_Data.Tables("Modelle").Rows(i)("DatumErstzulassung") = DBNull.Value
        Next
        Session("App_Report") = m_report
        Response.Redirect("Change02_1.aspx?AppID=" & Session("AppID").ToString)
    End Sub

    Protected Sub lnkCreateExcel2_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lnkCreateExcel2.Click
        Dim ExcelTable As DataTable
        If Not (Session("App_Report") Is Nothing) Then
            ExcelTable = m_report.Erledigt
            Try
                Dim excelFactory As New DocumentGeneration.ExcelDocumentFactory()
                Dim strFileName As String = Format(Now, "yyyyMMdd_HHmmss_") & m_User.UserName

                excelFactory.CreateDocumentAndSendAsResponse(strFileName, ExcelTable, Me.Page)

            Catch ex As Exception
                lblError.Text = "Fehler beim Erstellen der Excel-Datei: " + ex.Message
            End Try
        End If
    End Sub

    Protected Sub lnkCreateExcel_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles lnkCreateExcel.Click
        Dim ExcelTable As DataTable
        If Not (Session("App_Report") Is Nothing) Then
            ExcelTable = m_report.Erledigt
            Try
                Dim excelFactory As New DocumentGeneration.ExcelDocumentFactory()
                Dim strFileName As String = Format(Now, "yyyyMMdd_HHmmss_") & m_User.UserName

                excelFactory.CreateDocumentAndSendAsResponse(strFileName, ExcelTable, Me.Page)

            Catch ex As Exception
                lblError.Text = "Fehler beim Erstellen der Excel-Datei: " + ex.Message
            End Try
        End If
    End Sub

    Protected Sub cmdZurueck_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdZurueck.Click
        Session("App_Report") = Nothing
        Session("App_ResultTablePDIs") = Nothing
        Session("App_ResultTable") = Nothing
        Session("App_Result") = Nothing
        Session("App_ZulDat") = Nothing
        Response.Redirect("Change02.aspx?AppID=" & Session("AppID").ToString)
    End Sub

    Protected Sub cmdWeitereAuswahl_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdWeitereAuswahl.Click
        lblMessage.Text = ""
        cmdWeiter.Visible = True
        cmdWeitereAuswahl.Visible = False
        cmdAbsenden.Visible = False
        cmdVerwerfen.Visible = False
        cmdZurueck.Visible = False
        lnkCreateExcel2.Visible = False
        lnkCreateExcel.Visible = False
        Response.Redirect("Change02_1.aspx?AppID=" & Session("AppID").ToString)
    End Sub
    Private Sub insertScript()

        Literal1.Text = "						<script language=""Javascript"">" & vbCrLf
        Literal1.Text &= "						  <!-- //" & vbCrLf
        Literal1.Text &= "							DisplayCalender();" & vbCrLf
        Literal1.Text &= "						  //-->" & vbCrLf
        Literal1.Text &= "						</script>" & vbCrLf

    End Sub

    Private Sub calVon_SelectionChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles calVon.SelectionChanged
        If Not (Session("App_Report") Is Nothing) Then
            m_report = CType(Session("App_Report"), Zul_Sperr_Entsperr)
            ZuCount(calVon.SelectedDate)
        End If

        lblZulDat.Text = calVon.SelectedDate.ToShortDateString

        Dim ctrl As Control
        Dim obj1 As HtmlInputHidden

        For Each ctrl In Form1.Controls
            obj1 = CType(ctrl.FindControl("HiddenZuldat"), HtmlInputHidden)
            If Not obj1 Is Nothing Then
                obj1.Value = lblZulDat.Text
            End If

        Next

        Session("App_ZulDat") = calVon.SelectedDate.ToShortDateString
        Literal1.Text = "						<script language=""Javascript"">" & vbCrLf
        Literal1.Text &= "						  <!-- //" & vbCrLf
        Literal1.Text &= "							document.getElementById('DivCalendar').style.display = 'none';" & vbCrLf
        Literal1.Text &= "                          document.Form1.SelOpen2.value = '';" & vbCrLf
        Literal1.Text &= "						  //-->" & vbCrLf
        Literal1.Text &= "						</script>" & vbCrLf
    End Sub
    Private Sub ZuCount(ByVal Datum As Date)
        m_report.createZulassungCount(CStr(Datum))

        If Not m_report.ZulCount Is Nothing Then
            If m_report.ZulCount.Rows.Count > 0 Then
                Dim drResult() As DataRow = m_report.ZulCount.Select("ZZCARPORT='Gesamt'")
                GesamtAnzahl.Value = drResult(0)("ZANZAHL").ToString
                AktuelleSumme.Value = GesamtAnzahl.Value
                If AktuelleAnzahl.Value = "" Or AktuelleAnzahl.Value = "0" Then
                    AktuelleAnzahl.Value = "0"
                ElseIf CInt(AktuelleAnzahl.Value) > 0 Then
                    AktuelleSumme.Value = (CInt(AktuelleSumme.Value) + CInt(AktuelleAnzahl.Value)).ToString
                End If
            Else
                GesamtAnzahl.Value = "0"
                If AktuelleSumme.Value = "" Or AktuelleSumme.Value = "0" Then
                    AktuelleSumme.Value = "0"
                ElseIf CInt(AktuelleAnzahl.Value) > 0 Then
                    AktuelleSumme.Value = (CInt(AktuelleSumme.Value) + CInt(AktuelleAnzahl.Value)).ToString
                End If
                If AktuelleAnzahl.Value = "" Or AktuelleAnzahl.Value = "0" Then
                    AktuelleAnzahl.Value = "0"
                End If
            End If
        Else
            GesamtAnzahl.Value = "0"
            If AktuelleSumme.Value = "" Or AktuelleSumme.Value = "0" Then
                AktuelleSumme.Value = "0"
            End If
            If AktuelleAnzahl.Value = "" Or AktuelleAnzahl.Value = "0" Then
                AktuelleAnzahl.Value = "0"
            End If
        End If
    End Sub

    '----------------------------------------------------------------------
    ' Methode: EnableButton
    ' Autor: O.Rudolph
    ' Beschreibung: Zugriff auf Modellebene(Templates/Modell.ascx) & Imagebutton 
    ''FindControl("DCP")-> Placeholder = immer "DCP" verwenden
    ''FindControl("Panel_Carport_Modell")-> immer mit "Panel_" + Name der Beziehung der Ebenen
    ''FindControl("ChildTemplate_Carport_Modell")->immer mit "ChildTemplate_" + Name der Beziehung der Ebenen
    ''FindControl("HG1") -> Name des Grids
    ' Erstellt am: 03.12.2008
    ' ITA: 2359
    '----------------------------------------------------------------------


    Private Sub EnableButton()
        Dim item As DataGridItem
        Dim ImaBtn As ImageButton
        For Each item In HG1.Items
            Dim c As DBauer.Web.UI.WebControls.HierarGrid


            c = CType(item.Cells(1).FindControl("DCP").FindControl("Panel_Carport_Modell"). _
                    FindControl("ChildTemplate_Carport_Modell").FindControl("HG1"),  _
                                DBauer.Web.UI.WebControls.HierarGrid)
            Dim j As Integer = 0
            ImaBtn = CType(c.Items(0).Cells(18).FindControl("Imagebutton1"), ImageButton)
            ImaBtn.Enabled = False
            ImaBtn.ImageUrl = "../../../images/Confirm_Mini_Grey.GIF"
        Next
    End Sub

    Protected Sub ibtnDelZulDat_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtnDelZulDat.Click
        lblZulDat.Text = ""
        GesamtAnzahl.Value = "0"
        AktuelleSumme.Value = AktuelleAnzahl.Value
        Session("App_ZulDat") = Nothing
    End Sub

    Protected Sub imgbExcel_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgbExcel.Click
        createSelektionExcel()
    End Sub

    Protected Sub lnkExcel_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lnkExcel.Click
        createSelektionExcel()
    End Sub

    Private Sub createSelektionExcel()
        Dim ExcelTable As New DataTable

        With ExcelTable.Columns
            .Add("Carportnr", System.Type.GetType("System.String"))
            .Add("Carport", System.Type.GetType("System.String"))
            .Add("Herstellername", System.Type.GetType("System.String"))
            .Add("Typ ID Avis", System.Type.GetType("System.String"))
            .Add("Modellbezeichnung", System.Type.GetType("System.String"))
            .Add("Reifenart", System.Type.GetType("System.String"))
            .Add("Kraftstoffart", System.Type.GetType("System.String"))
            .Add("Navigation", System.Type.GetType("System.String"))
            .Add("Fahrgestellnummer", System.Type.GetType("System.String"))
            .Add("Zulassungsort", System.Type.GetType("System.String"))
            .Add("Owner Code", System.Type.GetType("System.String"))
            .Add("Geplante Lieferung", System.Type.GetType("System.String"))
        End With



        If Not (Session("App_Report") Is Nothing) Then
            Dim tmpExcelRow As DataRow
            Dim tmpNewExcelRow As DataRow
            For Each tmpRow As DataRow In m_report.CarPort_Data.Tables("Fahrzeuge").Rows
                tmpNewExcelRow = ExcelTable.NewRow
                tmpExcelRow = m_report.Result.Select("Fahrgestellnummer='" & tmpRow("Fahrgestellnummer").ToString & "'")(0)
                With tmpNewExcelRow
                    .Item("Carportnr") = tmpExcelRow("Carportnr")
                    .Item("Carport") = tmpExcelRow("Carport")
                    .Item("Herstellername") = tmpExcelRow("Herstellername")
                    .Item("Typ ID Avis") = tmpExcelRow("Typ_ID_Avis")
                    .Item("Modellbezeichnung") = tmpExcelRow("Modellbezeichnung")
                    .Item("Reifenart") = tmpExcelRow("Reifenart")
                    .Item("Kraftstoffart") = tmpExcelRow("Kraftstoffart")
                    .Item("Navigation") = IIf(tmpExcelRow("Navigation").ToString = "J", "Ja", "Nein")
                    .Item("Fahrgestellnummer") = tmpExcelRow("Fahrgestellnummer")
                    .Item("Zulassungsort") = tmpExcelRow("Zulassungsort")
                    .Item("Owner Code") = tmpExcelRow("Owner_Code")
                    .Item("Geplante Lieferung") = tmpExcelRow("Liefermonat")
                End With
                ExcelTable.Rows.Add(tmpNewExcelRow)
            Next

            ExcelTable.AcceptChanges()

            Try
                Dim excelFactory As New DocumentGeneration.ExcelDocumentFactory()
                Dim strFileName As String = Format(Now, "yyyyMMdd_HHmmss_") & m_User.UserName

                excelFactory.CreateDocumentAndSendAsResponse(strFileName, ExcelTable, Me.Page)

            Catch ex As Exception
                lblError.Text = "Fehler beim Erstellen der Excel-Datei: " + ex.Message
            End Try
        End If
    End Sub


   
End Class
' ************************************************
' $History: Change02_1.aspx.vb $
' 
' *****************  Version 17  *****************
' User: Fassbenders  Date: 17.03.11   Time: 16:26
' Updated in $/CKAG/Applications/AppAvis/forms
' 
' *****************  Version 16  *****************
' User: Fassbenders  Date: 7.02.11    Time: 11:57
' Updated in $/CKAG/Applications/AppAvis/forms
' 
' *****************  Version 15  *****************
' User: Rudolpho     Date: 10.09.09   Time: 13:15
' Updated in $/CKAG/Applications/AppAvis/forms
' 
' *****************  Version 14  *****************
' User: Jungj        Date: 23.06.09   Time: 13:30
' Updated in $/CKAG/Applications/AppAvis/forms
' ITA 2931 testfertig
' 
' *****************  Version 13  *****************
' User: Rudolpho     Date: 23.03.09   Time: 8:52
' Updated in $/CKAG/Applications/AppAvis/forms
' 
' *****************  Version 12  *****************
' User: Rudolpho     Date: 30.12.08   Time: 15:50
' Updated in $/CKAG/Applications/AppAvis/forms
' 
' *****************  Version 11  *****************
' User: Rudolpho     Date: 5.12.08    Time: 9:16
' Updated in $/CKAG/Applications/AppAvis/forms
' ITA: 2359
' 
' *****************  Version 10  *****************
' User: Rudolpho     Date: 3.12.08    Time: 15:24
' Updated in $/CKAG/Applications/AppAvis/forms
' ITA: 2359
' 
' *****************  Version 9  *****************
' User: Rudolpho     Date: 1.12.08    Time: 16:13
' Updated in $/CKAG/Applications/AppAvis/forms
' ITA: 2359 & kleinere Anpassungen
' 
' ************************************************
