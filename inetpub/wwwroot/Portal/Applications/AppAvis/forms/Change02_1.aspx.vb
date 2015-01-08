Imports CKG.Base.Kernel
Imports CKG.Base.Kernel.Common.Common

Partial Public Class Change02_1
    Inherits Page

    Protected WithEvents UcStyles As Portal.PageElements.Styles
    Protected WithEvents UcHeader As Portal.PageElements.Header
    Protected WithEvents ScriptManager1 As ScriptManager
    Protected WithEvents UpdatePanel1 As UpdatePanel

    Private _mUser As Base.Kernel.Security.User
    Private _mApp As Base.Kernel.Security.App
    Private _mReport As Zul_Sperr_Entsperr
    Private _mBlnDateFormatError As Boolean
    Private _mblnDateErrorMessage As String
    Private _mblnDateError As Boolean

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        Session("ShowLink") = "False"
        _mUser = GetUser(Me)
        UcHeader.InitUser(_mUser)
        FormAuth(Me, _mUser)

        GetAppIDFromQueryString(Me)
        lnkKreditlimit.NavigateUrl = "Change02.aspx?AppID=" & Session("AppID").ToString
        Try
            lblHead.Text = _mUser.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString
            UcStyles.TitleText = lblHead.Text
            _mApp = New Base.Kernel.Security.App(_mUser)
            If Not Session("App_Report") Is Nothing Then
                _mReport = CType(Session("App_Report"), Zul_Sperr_Entsperr)
                Session("App_ResultTablePDIs") = _mReport.ResultPDIs
                Session("App_ResultTable") = _mReport.ResultTable
                Session("App_Result") = _mReport.Result
            End If

            If Not IsPostBack Then
                _mReport.Show()
                _mReport.ArtDerZulassung = CType([Enum].Parse(GetType(Zul_Sperr_Entsperr.Zulassungstyp), ddlDatumswahl.SelectedValue), Zul_Sperr_Entsperr.Zulassungstyp)
                FillGrid(-1)
            Else

                Dim blnInputPerformed As Boolean
                If Request.Form("__EVENTTARGET").ToString = "cmdAbsenden" Then
                    blnInputPerformed = False
                ElseIf Request.Form("__EVENTTARGET").ToString = "cmdWeitereAuswahl" Then

                    Dim sFilter As String
                    sFilter = _mReport.FilterString
                    Dim strFileName As String = Format(Now, "yyyyMMdd_HHmmss_") & _mUser.UserName & ".xls"
                    _mReport = New Zul_Sperr_Entsperr(_mUser, _mApp, strFileName)
                    _mReport.Fill(Session("AppID").ToString, Session.SessionID.ToString)

                    If Not _mReport.Status = 0 Then
                        lblError.Text = _mReport.Message
                    Else
                        If Not _mReport.Result.Rows.Count = 0 Then

                            Session("App_ResultTablePDIs") = _mReport.ResultPDIs
                            Session("App_Report") = _mReport
                            Session("App_ResultTable") = _mReport.Result

                        End If
                    End If
                    _mReport.FilterString = sFilter
                    _mReport.FilterCarports()

                End If

                If Not Request.Form.Item("PDINummer").ToString = "empty" Then
                    _mReport.Carportnummer = Request.Form.Item("PDINummer").ToString
                    _mReport.Show()
                    If _mReport.Status = 0 Then
                        FillGrid(CInt(Request.Form.Item("NummerInGrid")))
                    Else
                        lblError.Text = "Beim Laden der Carport-Daten ist ein Fehler aufgetreten.<br>(" & _mReport.Message & ")"
                    End If
                End If
                If blnInputPerformed Then
                    FillGrid(-1)
                End If

            End If

            If Not String.IsNullOrEmpty(SelOpen2.Value) Then InsertScript(SelOpen2.Value)
        Catch ex As Exception
            lblError.Text = "Beim Laden der Seite ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
        End Try
    End Sub

    Protected Sub HgTemplateSelection(ByVal sender As Object, ByVal e As DBauer.Web.UI.WebControls.HierarGridTemplateSelectionEventArgs) Handles HG1.TemplateSelection
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
            HG1.DataSource = _mReport.CarPort_Data_Selected
        Else
            HG1.DataSource = _mReport.CarPort_Data
        End If
        HG1.DataMember = "Carports"
        HG1.DataBind()

        _mReport.ArtDerZulassung = CType([Enum].Parse(GetType(Zul_Sperr_Entsperr.Zulassungstyp), ddlDatumswahl.SelectedValue), Zul_Sperr_Entsperr.Zulassungstyp)

        Dim item As DataGridItem
        Dim cell As TableCell
        Dim control As Control
        Dim checkbox As CheckBox
        Dim intItem As Int32 = 0

        For Each item In HG1.Items
            Dim blnDetails As Boolean = False
            cell = item.Cells(1)
            For Each control In cell.Controls
                If TypeOf control Is CheckBox Then
                    checkbox = CType(control, CheckBox)
                    If checkbox.ID = "chkDetails" And checkbox.Checked Then
                        blnDetails = True
                    End If
                End If
            Next

            cell = item.Cells(0)
            cell.Width = New Unit("100px")
            cell.HorizontalAlign = HorizontalAlign.Center

            If Not blnDetails Then
                Dim image As New Image()
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
        Dim blnReturn As Boolean
        Dim datrows() As DataRow
        If _mReport Is Nothing Then
            _mReport = CType(Session("App_Report"), Zul_Sperr_Entsperr)
        End If

        _mBlnDateFormatError = False
        _mblnDateError = False
        _mblnDateErrorMessage = ""

        Dim dateVerarbeitung As Date
        Dim blnVerarbeitungsdatumGesetzt As Boolean = False

        Try

            Dim blnPlanzulassung As Boolean = (_mReport.ArtDerZulassung = Zul_Sperr_Entsperr.Zulassungstyp.Planzulassung)

            _mReport.Verarbeitungsdatum = lblVerarbeitungsdatum.Text
            If blnPlanzulassung AndAlso Not String.IsNullOrEmpty(_mReport.Verarbeitungsdatum) Then
                If Not IsDate(_mReport.Verarbeitungsdatum) AndAlso Not IsStandardDate(_mReport.Verarbeitungsdatum) AndAlso Not IsSAPDate(_mReport.Verarbeitungsdatum) Then
                    _mBlnDateFormatError = True
                Else
                    dateVerarbeitung = CType(_mReport.Verarbeitungsdatum, Date)
                    blnVerarbeitungsdatumGesetzt = True
                End If
            End If

            For i As Integer = 0 To Request.Form.Count - 1
                If InStr(Request.Form.Keys.Item(i), "Fahrgestellnummer_") > 0 Then
                    Dim strFahrgestellNummer As String = (Right(Request.Form.Keys.Item(i), 17))
                    Dim sperre As String = ""
                    Dim entsperre As String = ""
                    Dim blnAuswahlNeu As Boolean = False
                    Dim strDatumErstzulassung As String = ""
                    If i + 1 = Request.Form.Count Then Exit For

                    If UCase(Request.Form(i + 1).ToString) = "ON" And InStr(Request.Form.Keys.Item(i + 1), "chkAuswahl") > 0 Then
                        blnAuswahlNeu = True
                        strDatumErstzulassung = Request.Form(i - 1).ToString
                        datrows = _mReport.CarPort_Data.Tables("Fahrzeuge").Select("Fahrgestellnummer='" & strFahrgestellNummer & "'")
                        If datrows.Length > 0 Then
                            sperre = datrows(0)("neuGesperrt").ToString
                            entsperre = datrows(0)("neuEntsperrt").ToString
                        End If
                        If Not sperre = "1" AndAlso Not entsperre = "1" Then
                            If Not IsDate(strDatumErstzulassung) AndAlso Not IsStandardDate(strDatumErstzulassung) AndAlso Not IsSAPDate(strDatumErstzulassung) Then
                                _mBlnDateFormatError = True
                            End If

                            If Not _mBlnDateFormatError Then
                                Dim dateZulassung As Date
                                dateZulassung = CType(strDatumErstzulassung, Date)

                                If dateZulassung.DayOfWeek = DayOfWeek.Saturday OrElse dateZulassung.DayOfWeek = DayOfWeek.Sunday Then
                                    _mblnDateError = True
                                    _mblnDateErrorMessage = "Zulassungsdatum fällt auf einen Samstag oder Sonntag."

                                ElseIf _mReport.DezZul AndAlso dateZulassung <= Today.Date Then
                                    _mblnDateError = True
                                    _mblnDateErrorMessage = "Das Zulassungsdatum ist unzulässig, das Datum muss in der Zukunft liegen."

                                ElseIf Not _mReport.DezZul AndAlso dateZulassung < Today.Date Then
                                    _mblnDateError = True
                                    _mblnDateErrorMessage = "Das Zulassungsdatum darf nicht in der Vergangenheit liegen."

                                ElseIf blnPlanzulassung AndAlso blnVerarbeitungsdatumGesetzt AndAlso dateVerarbeitung >= dateZulassung Then
                                    _mblnDateError = True
                                    _mblnDateErrorMessage = "Das Verarbeitungsdatum muss vor dem Zulassungsdatum liegen."

                                End If

                            End If

                        End If
                    End If
                    Dim intModellId As Int32
                    For j As Integer = 0 To _mReport.CarPort_Data.Tables("Fahrzeuge").Rows.Count - 1
                        If _mReport.CarPort_Data.Tables("Fahrzeuge").Rows(j)("Fahrgestellnummer").ToString = strFahrgestellNummer Then
                            _mReport.CarPort_Data.Tables("Fahrzeuge").Rows(j)("Ausgewaehlt") = blnAuswahlNeu
                            If IsDate(strDatumErstzulassung) Then
                                _mReport.CarPort_Data.Tables("Fahrzeuge").Rows(j)("DatumErstzulassung") = CDate(strDatumErstzulassung)
                            End If
                            intModellId = CInt(_mReport.CarPort_Data.Tables("Fahrzeuge").Rows(j)("Modell_ID"))

                        End If
                    Next
                    For j As Integer = 0 To _mReport.CarPort_Data.Tables("Modelle").Rows.Count - 1
                        If CInt(_mReport.CarPort_Data.Tables("Modelle").Rows(j)("ID")) = intModellId Then
                            If IsDate(strDatumErstzulassung) Then
                                _mReport.CarPort_Data.Tables("Modelle").Rows(j)("DatumErstzulassung") = CDate(strDatumErstzulassung)
                            End If
                        End If
                    Next
                    For j As Integer = 0 To _mReport.CarPort_Data.Tables("Fahrzeuge").Rows.Count - 1
                        If _mReport.CarPort_Data.Tables("Fahrzeuge").Rows(j)("Fahrgestellnummer").ToString = strFahrgestellNummer Then
                            _mReport.CarPort_Data.Tables("Fahrzeuge").Rows(j)("Ausgewaehlt") = blnAuswahlNeu

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

    Protected Sub CmdWeiterClick(ByVal sender As Object, ByVal e As EventArgs) Handles cmdWeiter.Click
        CheckInput()
        If _mBlnDateFormatError Then
            lblError.Text = "Bitte geben Sie ein gültiges Datum ein (Format: TT.MM.JJJJ oder TTMMJJJJ)."
        Else
            If _mblnDateError = True Then
                lblError.Text = _mblnDateErrorMessage
            End If

        End If
        If (Not _mBlnDateFormatError) And (Not _mblnDateError) Then
            If _mReport.SelectCars > 0 Then
                FillGrid(-1, True)
                rowSelectionExcel.Visible = False
                cmdWeiter.Visible = False
                cmdAbsenden.Visible = True
                cmdVerwerfen.Visible = True
                cmdZurueck.Visible = False
                DisableAllInput()
                EnableButton()

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

    Protected Sub CmdAbsendenClick(ByVal sender As Object, ByVal e As EventArgs) Handles cmdAbsenden.Click
        If _mReport Is Nothing Then
            _mReport = CType(Session("App_Report"), Zul_Sperr_Entsperr)
        End If

        lblPageTitle.Text = "Bestätigung der Datenübernahme"

        _mReport.SaveData(Session("AppID").ToString, Session.SessionID)

        If _mReport.FehlerCount > 0 Then
            lblMessage.Text = _mReport.FehlerCount & " Fahrzeug(e) konnte(n) aufgrund eines Fehlers nicht übernommen werden."
        ElseIf _mReport.Message = "" Then
            lblMessage.Text = _mReport.SelectedCars.ToString & " Fahrzeuge übernommen."
        End If

        If _mReport.SelectCars > 0 Then
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

    Protected Sub CmdVerwerfenClick(ByVal sender As Object, ByVal e As EventArgs) Handles cmdVerwerfen.Click
        Dim i As Int32
        For i = 0 To _mReport.CarPort_Data.Tables("Fahrzeuge").Rows.Count - 1
            _mReport.CarPort_Data.Tables("Fahrzeuge").Rows(i)("Ausgewaehlt") = False
            _mReport.CarPort_Data.Tables("Fahrzeuge").Rows(i)("neuGesperrt") = ""
            _mReport.CarPort_Data.Tables("Fahrzeuge").Rows(i)("neuEntsperrt") = ""
            _mReport.CarPort_Data.Tables("Fahrzeuge").Rows(i)("DatumErstzulassung") = DBNull.Value
            Dim sDate As String = _mReport.CarPort_Data.Tables("Fahrzeuge").Rows(i)("Datum_zur_Sperre").ToString
            If IsDate(sDate) Then
                _mReport.CarPort_Data.Tables("Fahrzeuge").Rows(i)("Datum_zur_Sperre") = CDate(sDate).ToShortDateString
            End If

        Next
        For i = 0 To _mReport.CarPort_Data.Tables("Modelle").Rows.Count - 1
            _mReport.CarPort_Data.Tables("Modelle").Rows(i)("DatumErstzulassung") = DBNull.Value
        Next
        Session("App_Report") = _mReport
        Response.Redirect("Change02_1.aspx?AppID=" & Session("AppID").ToString)
    End Sub

    Protected Sub LnkCreateExcel2Click(ByVal sender As Object, ByVal e As EventArgs) Handles lnkCreateExcel2.Click
        Dim excelTable As DataTable
        If Not (Session("App_Report") Is Nothing) Then
            excelTable = _mReport.Erledigt
            Try
                Dim excelFactory As New DocumentGeneration.ExcelDocumentFactory()
                Dim strFileName As String = Format(Now, "yyyyMMdd_HHmmss_") & _mUser.UserName

                excelFactory.CreateDocumentAndSendAsResponse(strFileName, excelTable, Page)

            Catch ex As Exception
                lblError.Text = "Fehler beim Erstellen der Excel-Datei: " + ex.Message
            End Try
        End If
    End Sub

    Protected Sub LnkCreateExcelClick(ByVal sender As Object, ByVal e As ImageClickEventArgs) Handles lnkCreateExcel.Click
        Dim excelTable As DataTable
        If Not (Session("App_Report") Is Nothing) Then
            excelTable = _mReport.Erledigt
            Try
                Dim excelFactory As New DocumentGeneration.ExcelDocumentFactory()
                Dim strFileName As String = Format(Now, "yyyyMMdd_HHmmss_") & _mUser.UserName

                excelFactory.CreateDocumentAndSendAsResponse(strFileName, excelTable, Page)

            Catch ex As Exception
                lblError.Text = "Fehler beim Erstellen der Excel-Datei: " + ex.Message
            End Try
        End If
    End Sub

    Protected Sub CmdZurueckClick(ByVal sender As Object, ByVal e As EventArgs) Handles cmdZurueck.Click
        Session("App_Report") = Nothing
        Session("App_ResultTablePDIs") = Nothing
        Session("App_ResultTable") = Nothing
        Session("App_Result") = Nothing
        Session("App_ZulDat") = Nothing
        Response.Redirect("Change02.aspx?AppID=" & Session("AppID").ToString)
    End Sub

    Protected Sub CmdWeitereAuswahlClick(ByVal sender As Object, ByVal e As EventArgs) Handles cmdWeitereAuswahl.Click
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

    Private Sub InsertScript(ByVal divId As String)
        Literal1.Text = "						<script language=""Javascript"">" & vbCrLf
        Literal1.Text &= "						  <!-- //" & vbCrLf
        Literal1.Text &= "							DisplayCalendar('" & divId & "', 'btn" & divId & "');" & vbCrLf
        Literal1.Text &= "						  //-->" & vbCrLf
        Literal1.Text &= "						</script>" & vbCrLf
    End Sub

    Private Sub calZulassungsdatum_SelectionChanged(ByVal sender As Object, ByVal e As EventArgs) Handles calZulassungsdatum.SelectionChanged
        If Not (Session("App_Report") Is Nothing) Then
            _mReport = CType(Session("App_Report"), Zul_Sperr_Entsperr)
            ZuCount(calZulassungsdatum.SelectedDate)
        End If

        lblZulassungsdatum.Text = calZulassungsdatum.SelectedDate.ToShortDateString
        HiddenZulassungsdatum.Value = lblZulassungsdatum.Text

        Session("App_ZulDat") = calZulassungsdatum.SelectedDate.ToShortDateString

        Literal1.Text = "						<script language=""Javascript"">" & vbCrLf
        Literal1.Text &= "						  <!-- //" & vbCrLf
        Literal1.Text &= "							document.getElementById('DivCalendarZulassungsdatum').style.display = 'none';" & vbCrLf
        Literal1.Text &= "                          document.Form1.SelOpen2.value = '';" & vbCrLf
        Literal1.Text &= "						  //-->" & vbCrLf
        Literal1.Text &= "						</script>" & vbCrLf
    End Sub

    Private Sub calVerarbeitungsdatum_SelectionChanged(ByVal sender As Object, ByVal e As EventArgs) Handles calVerarbeitungsdatum.SelectionChanged

        lblVerarbeitungsdatum.Text = calVerarbeitungsdatum.SelectedDate.ToShortDateString
        HiddenVerarbeitungsdatum.Value = lblVerarbeitungsdatum.Text

        Literal1.Text = "						<script language=""Javascript"">" & vbCrLf
        Literal1.Text &= "						  <!-- //" & vbCrLf
        Literal1.Text &= "							document.getElementById('DivCalendarVerarbeitungsdatum').style.display = 'none';" & vbCrLf
        Literal1.Text &= "                          document.Form1.SelOpen2.value = '';" & vbCrLf
        Literal1.Text &= "						  //-->" & vbCrLf
        Literal1.Text &= "						</script>" & vbCrLf
    End Sub

    Private Sub ZuCount(ByVal datum As Date, Optional ByVal blnResetAnzahl As Boolean = False)
        _mReport.createZulassungCount(CStr(datum))

        If _mReport.ZulCount IsNot Nothing Then
            If _mReport.ZulCount.Rows.Count > 0 Then
                Dim drResult() As DataRow = _mReport.ZulCount.Select("ZZCARPORT='Gesamt'")
                GesamtAnzahl.Value = drResult(0)("ZANZAHL").ToString
                AktuelleSumme.Value = GesamtAnzahl.Value

                If blnResetAnzahl Then
                    AktuelleAnzahl.Value = "0"
                Else
                    If AktuelleAnzahl.Value = "" Or AktuelleAnzahl.Value = "0" Then
                        AktuelleAnzahl.Value = "0"
                    ElseIf CInt(AktuelleAnzahl.Value) > 0 Then
                        AktuelleSumme.Value = (CInt(AktuelleSumme.Value) + CInt(AktuelleAnzahl.Value)).ToString
                    End If
                End If
                
            Else
                GesamtAnzahl.Value = "0"

                If blnResetAnzahl Then
                    AktuelleAnzahl.Value = "0"
                    AktuelleSumme.Value = "0"
                Else
                    If AktuelleSumme.Value = "" Or AktuelleSumme.Value = "0" Then
                        AktuelleSumme.Value = "0"
                    ElseIf CInt(AktuelleAnzahl.Value) > 0 Then
                        AktuelleSumme.Value = (CInt(AktuelleSumme.Value) + CInt(AktuelleAnzahl.Value)).ToString
                    End If
                    If AktuelleAnzahl.Value = "" Or AktuelleAnzahl.Value = "0" Then
                        AktuelleAnzahl.Value = "0"
                    End If
                End If
            End If

        Else
            GesamtAnzahl.Value = "0"

            If blnResetAnzahl Then
                AktuelleAnzahl.Value = "0"
                AktuelleSumme.Value = "0"
            Else
                If AktuelleSumme.Value = "" Or AktuelleSumme.Value = "0" Then
                    AktuelleSumme.Value = "0"
                End If
                If AktuelleAnzahl.Value = "" Or AktuelleAnzahl.Value = "0" Then
                    AktuelleAnzahl.Value = "0"
                End If
            End If
            
        End If
    End Sub

    ''' <summary>
    '''----------------------------------------------------------------------
    ''' Methode: EnableButton
    ''' Autor: O.Rudolph
    ''' Beschreibung: Zugriff auf Modellebene(Templates/Modell.ascx) & Imagebutton 
    ''' FindControl("DCP")-> Placeholder = immer "DCP" verwenden
    ''' FindControl("Panel_Carport_Modell")-> immer mit "Panel_" + Name der Beziehung der Ebenen
    ''' FindControl("ChildTemplate_Carport_Modell")->immer mit "ChildTemplate_" + Name der Beziehung der Ebenen
    ''' FindControl("HG1") -> Name des Grids
    ''' Erstellt am: 03.12.2008
    ''' ITA: 2359
    '''----------------------------------------------------------------------
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub EnableButton()
        Dim item As DataGridItem
        Dim imaBtn As ImageButton
        For Each item In HG1.Items
            Dim c As DBauer.Web.UI.WebControls.HierarGrid


            c = CType(item.Cells(1).FindControl("DCP").FindControl("Panel_Carport_Modell"). _
                    FindControl("ChildTemplate_Carport_Modell").FindControl("HG1"),  _
                                DBauer.Web.UI.WebControls.HierarGrid)

            imaBtn = CType(c.Items(0).Cells(18).FindControl("Imagebutton1"), ImageButton)
            imaBtn.Enabled = False
            imaBtn.ImageUrl = "../../../images/Confirm_Mini_Grey.GIF"
        Next
    End Sub

    Protected Sub ibtnDelZulassungsdatum_Click(ByVal sender As Object, ByVal e As ImageClickEventArgs) Handles ibtnDelZulassungsdatum.Click
        lblZulassungsdatum.Text = ""
        GesamtAnzahl.Value = "0"
        AktuelleSumme.Value = AktuelleAnzahl.Value
        Session("App_ZulDat") = Nothing
    End Sub

    Protected Sub ibtnDelVerarbeitungsdatum_Click(ByVal sender As Object, ByVal e As ImageClickEventArgs) Handles ibtnDelVerarbeitungsdatum.Click
        lblVerarbeitungsdatum.Text = ""
    End Sub

    Protected Sub ImgbExcelClick(ByVal sender As Object, ByVal e As ImageClickEventArgs) Handles imgbExcel.Click
        CreateSelektionExcel()
    End Sub

    Protected Sub LnkExcelClick(ByVal sender As Object, ByVal e As EventArgs) Handles lnkExcel.Click
        createSelektionExcel()
    End Sub

    Private Sub CreateSelektionExcel()
        Dim excelTable As New DataTable

        With excelTable.Columns
            .Add("Carportnr", Type.GetType("System.String"))
            .Add("Carport", Type.GetType("System.String"))
            .Add("Herstellername", Type.GetType("System.String"))
            .Add("Typ ID Avis", Type.GetType("System.String"))
            .Add("Modellbezeichnung", Type.GetType("System.String"))
            .Add("Reifenart", Type.GetType("System.String"))
            .Add("Kraftstoffart", Type.GetType("System.String"))
            .Add("Navigation", Type.GetType("System.String"))
            .Add("Fahrgestellnummer", Type.GetType("System.String"))
            .Add("Zulassungsort", Type.GetType("System.String"))
            .Add("Owner Code", Type.GetType("System.String"))
            .Add("Geplante Lieferung", Type.GetType("System.String"))
        End With

        If Not (Session("App_Report") Is Nothing) Then
            Dim tmpExcelRow As DataRow
            Dim tmpNewExcelRow As DataRow
            For Each tmpRow As DataRow In _mReport.CarPort_Data.Tables("Fahrzeuge").Rows
                tmpNewExcelRow = excelTable.NewRow
                tmpExcelRow = _mReport.Result.Select("Fahrgestellnummer='" & tmpRow("Fahrgestellnummer").ToString & "'")(0)
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
                excelTable.Rows.Add(tmpNewExcelRow)
            Next

            excelTable.AcceptChanges()

            Try
                Dim excelFactory As New DocumentGeneration.ExcelDocumentFactory()
                Dim strFileName As String = Format(Now, "yyyyMMdd_HHmmss_") & _mUser.UserName

                excelFactory.CreateDocumentAndSendAsResponse(strFileName, excelTable, Page)

            Catch ex As Exception
                lblError.Text = "Fehler beim Erstellen der Excel-Datei: " + ex.Message
            End Try
        End If
    End Sub

    Protected Sub DdlDatumswahlSelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlDatumswahl.SelectedIndexChanged

        _mReport.ArtDerZulassung = CType([Enum].Parse(GetType(Zul_Sperr_Entsperr.Zulassungstyp), ddlDatumswahl.SelectedValue), Zul_Sperr_Entsperr.Zulassungstyp)
        Dim blnPlanzulassung As Boolean = (_mReport.ArtDerZulassung = Zul_Sperr_Entsperr.Zulassungstyp.Planzulassung)

        If Not (Session("App_Report") Is Nothing) Then
            _mReport = CType(Session("App_Report"), Zul_Sperr_Entsperr)
            ZuCount(calZulassungsdatum.SelectedDate, blnPlanzulassung)
        End If

        SetVerarbeitungsDatumVisibility(blnPlanzulassung)

        If blnPlanzulassung Then lblVerarbeitungsdatum.Text = ""

        FillGrid(-1)

    End Sub

    Private Sub SetVerarbeitungsDatumVisibility(ByVal show As Boolean)
        lblTitelVerarbeitungsdatum.Visible = show
        lblVerarbeitungsdatum.Visible = show
        btnDivCalendarVerarbeitungsdatum.Visible = show
        ibtnDelVerarbeitungsdatum.Visible = show
    End Sub

End Class

