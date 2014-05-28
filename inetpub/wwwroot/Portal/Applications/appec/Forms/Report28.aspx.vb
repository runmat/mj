Imports CKG.Base.Kernel.Common.Common
Imports CKG.Portal.PageElements
Imports System.Globalization

''' <summary>
''' Report "Fahrzeugbestand"
''' </summary>
''' <remarks></remarks>
Public Class Report28
    Inherits System.Web.UI.Page

    Protected WithEvents ucHeader As Header
    Protected WithEvents ucStyles As Styles

    Private m_User As Base.Kernel.Security.User
    Private m_App As Base.Kernel.Security.App
    Private objBestand As ec_28

    Private Enum Kalendermodus
        EingangZB2_von
        EingangZB2_bis
        EingangFzg_von
        EingangFzg_bis
        BereitmFzg_von
        BereitmFzg_bis
        ZulDat_von
        ZulDat_bis
    End Enum

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        m_User = GetUser(Me)
        ucHeader.InitUser(m_User)
        FormAuth(Me, m_User)

        GetAppIDFromQueryString(Me)

        Try

            lblHead.Text = m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString
            ucStyles.TitleText = lblHead.Text
            m_App = New Base.Kernel.Security.App(m_User)

            If Not IsPostBack Then
                Session("ResultTable") = Nothing
                Session("lnkExcel") = Nothing
                objBestand = New ec_28(m_User, m_App, "")
                objBestand.FillHersteller(Session("AppID").ToString, Session.SessionID.ToString)
                objBestand.FillPDIStandorte(Session("AppID").ToString, Session.SessionID.ToString)
                objBestand.FillStati(Session("AppID").ToString, Session.SessionID.ToString)
                Session("objBestand") = objBestand
                FillDropdowns()
            Else
                objBestand = CType(Session("objBestand"), ec_28)
            End If
        Catch ex As Exception
            lblError.Text = "Beim Laden der Seite ist ein Fehler aufgetreten."
        End Try
    End Sub

    ''' <summary>
    ''' Auswahl-Dropdowns mit Werten füllen
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub FillDropdowns()
        'Hersteller
        ddlHersteller.Items.Add(New ListItem("alle", ""))
        Dim dvHersteller As DataView = objBestand.Hersteller.DefaultView
        dvHersteller.Sort = "HERST_T"
        For Each herst As DataRowView In dvHersteller
            ddlHersteller.Items.Add(New ListItem(herst("HERST_T"), herst("HERST_T")))
        Next
        'PDI-Standorte
        ddlPdiStandort.Items.Add(New ListItem("alle", ""))
        Dim dvPDIStandorte As DataView = objBestand.PDIStandorte.DefaultView
        dvPDIStandorte.Sort = "KUNPDINR"
        For Each sto As DataRowView In dvPDIStandorte
            ddlPdiStandort.Items.Add(New ListItem(sto("PDIWEB"), sto("KUNPDI")))
        Next
        'Stati
        ddlStatus.Items.Add(New ListItem("alle", ""))
        Dim dvStati As DataView = objBestand.Stati.DefaultView
        dvStati.Sort = "POS_KURZTEXT"
        For Each stat As DataRowView In dvStati
            ddlStatus.Items.Add(New ListItem(stat("POS_TEXT"), stat("POS_KURZTEXT")))
        Next
    End Sub

    ''' <summary>
    ''' Auswahl per Radiobutton, ob Fahrzeugauswahl manuell oder per Excel-Upload -> Controls ein-/ausblenden
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub rblFahrzeugauswahl_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles rblFahrzeugauswahl.SelectedIndexChanged
        Dim rbl As RadioButtonList = DirectCast(sender, RadioButtonList)
        If rbl.SelectedValue = "manuell" Then
            trUploadHinweis.Visible = False
            trUpload.Visible = False
            trFahrgestellnummer.Visible = True
            trKennzeichen.Visible = True
            trZB2Nummer.Visible = True
            trModelId.Visible = True
            trUnitnummer.Visible = True
            trAuftragsnummer.Visible = True
            trBatchId.Visible = True
            trSippCode.Visible = True
        Else
            trFahrgestellnummer.Visible = False
            trKennzeichen.Visible = False
            trZB2Nummer.Visible = False
            trModelId.Visible = False
            trUnitnummer.Visible = False
            trAuftragsnummer.Visible = False
            trBatchId.Visible = False
            trSippCode.Visible = False
            trUploadHinweis.Visible = True
            trUpload.Visible = True
        End If
    End Sub

    ''' <summary>
    ''' Excel-Datei hochladen und im Erfolgsfall die weitere Verarbeitung anstoßen
    ''' </summary>
    ''' <remarks></remarks>
    Private Function UploadExcelFile() As Boolean
        Dim erg As Boolean = False
        Dim strFilename As String = ""

        If (Not upFile.PostedFile Is Nothing) AndAlso (Not (upFile.PostedFile.FileName = String.Empty)) Then
            If Right(upFile.PostedFile.FileName.ToUpper, 4) <> ".XLS" Then
                lblError.Text = "Es können nur Dateien im .XLS - Format verarbeitet werden."
            Else
                'Lade Datei
                upload(upFile.PostedFile, strFilename)
                If Not String.IsNullOrEmpty(strFilename) Then
                    objBestand.ExcelDatei = strFilename
                    Dim objExcel As New Base.Kernel.DocumentGeneration.ExcelDocumentFactory()
                    Dim tblName As String = ""
                    objExcel.ReturnTableName(strFilename, tblName)

                    objBestand.LoadExcelData(tblName)
                    If objBestand.Status <> 0 Then
                        lblError.Text = objBestand.Message
                    Else
                        Session("objBestand") = objBestand
                        erg = True
                    End If
                Else
                    lblError.Text = "Fehler beim Upload."
                End If
            End If
        Else
            lblError.Text = "Keine Datei ausgewählt."
        End If

        Return erg

    End Function

    ''' <summary>
    ''' Excel-Datei auf Server hochladen und speichern
    ''' </summary>
    ''' <param name="uFile"></param>
    ''' <param name="strFilename"></param>
    ''' <remarks></remarks>
    Private Sub upload(ByVal uFile As System.Web.HttpPostedFile, ByRef strFilename As String)
        Try
            Dim filepath As String = ConfigurationManager.AppSettings("ExcelPath")
            Dim filename As String
            Dim info As System.IO.FileInfo

            'Dateiname: User_yyyyMMddhhmmss.xls
            filename = m_User.UserName & "_" & Format(Now, "yyyyMMddhhmmss") & ".xls"

            If Not (uFile Is Nothing) Then
                uFile.SaveAs(filepath & filename)
                info = New System.IO.FileInfo(filepath & filename)
                If Not (info.Exists) Then
                    lblError.Text = "Fehler beim Speichern."
                    strFilename = String.Empty
                Else
                    strFilename = filepath & filename
                End If

            End If
        Catch ex As Exception
            lblError.Text = ex.Message
        Finally
        End Try
    End Sub

    Private Sub cmdCreate_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdCreate.Click
        GetDataAndSubmit()
    End Sub

    ''' <summary>
    ''' Formulardaten einlesen bzw. Excel-Upload triggern, anschließend Suche ausführen
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub GetDataAndSubmit()
        Dim blnZeitraumAngegeben As Boolean = False

        objBestand.SelFahrzeuge.Rows.Clear()

        If Not String.IsNullOrEmpty(txtEingangZB2_von.Text) AndAlso IsDate(txtEingangZB2_von.Text) _
            AndAlso Not String.IsNullOrEmpty(txtEingangZB2_bis.Text) AndAlso IsDate(txtEingangZB2_bis.Text) Then
            blnZeitraumAngegeben = True
            DateTime.TryParseExact(txtEingangZB2_von.Text, "dd.MM.yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, objBestand.EingangZB2_von)
            DateTime.TryParseExact(txtEingangZB2_bis.Text, "dd.MM.yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, objBestand.EingangZB2_bis)
            If DateDiff(DateInterval.Month, objBestand.EingangZB2_von, objBestand.EingangZB2_bis) > 24 Then
                lblError.Text = "Die Zeitspanne für den Eingang ZB2 darf maximal 24 Monate betragen"
                Return
            End If
        End If

        If Not String.IsNullOrEmpty(txtEingangFzg_von.Text) AndAlso IsDate(txtEingangFzg_von.Text) _
            AndAlso Not String.IsNullOrEmpty(txtEingangFzg_bis.Text) AndAlso IsDate(txtEingangFzg_bis.Text) Then
            blnZeitraumAngegeben = True
            DateTime.TryParseExact(txtEingangFzg_von.Text, "dd.MM.yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, objBestand.EingangFzg_von)
            DateTime.TryParseExact(txtEingangFzg_bis.Text, "dd.MM.yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, objBestand.EingangFzg_bis)
            If DateDiff(DateInterval.Month, objBestand.EingangFzg_von, objBestand.EingangFzg_bis) > 24 Then
                lblError.Text = "Die Zeitspanne für den Eingang Fahrzeug darf maximal 24 Monate betragen"
                Return
            End If
        End If

        If Not String.IsNullOrEmpty(txtBereitmFzg_von.Text) AndAlso IsDate(txtBereitmFzg_von.Text) _
            AndAlso Not String.IsNullOrEmpty(txtBereitmFzg_bis.Text) AndAlso IsDate(txtBereitmFzg_bis.Text) Then
            blnZeitraumAngegeben = True
            DateTime.TryParseExact(txtBereitmFzg_von.Text, "dd.MM.yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, objBestand.BereitmFzg_von)
            DateTime.TryParseExact(txtBereitmFzg_bis.Text, "dd.MM.yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, objBestand.BereitmFzg_bis)
            If DateDiff(DateInterval.Month, objBestand.BereitmFzg_von, objBestand.BereitmFzg_bis) > 24 Then
                lblError.Text = "Die Zeitspanne für die Bereitmeldung Fahrzeug darf maximal 24 Monate betragen"
                Return
            End If
        End If

        If Not String.IsNullOrEmpty(txtZulassungsdatum_von.Text) AndAlso IsDate(txtZulassungsdatum_von.Text) _
            AndAlso Not String.IsNullOrEmpty(txtZulassungsdatum_bis.Text) AndAlso IsDate(txtZulassungsdatum_bis.Text) Then
            blnZeitraumAngegeben = True
            DateTime.TryParseExact(txtZulassungsdatum_von.Text, "dd.MM.yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, objBestand.ZulDat_von)
            DateTime.TryParseExact(txtZulassungsdatum_bis.Text, "dd.MM.yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, objBestand.ZulDat_bis)
            If DateDiff(DateInterval.Month, objBestand.ZulDat_von, objBestand.ZulDat_bis) > 24 Then
                lblError.Text = "Die Zeitspanne für das Zulassungsdatum darf maximal 24 Monate betragen"
                Return
            End If
        End If

        If rblFahrzeugauswahl.SelectedValue = "manuell" Then
            'manuelle Eingabe
            If Not blnZeitraumAngegeben AndAlso String.IsNullOrEmpty(txtFahrgestellnummer.Text) AndAlso String.IsNullOrEmpty(txtKennzeichen.Text) _
                AndAlso String.IsNullOrEmpty(txtZB2Nummer.Text) AndAlso String.IsNullOrEmpty(txtModelId.Text) _
                AndAlso String.IsNullOrEmpty(txtUnitnummer.Text) AndAlso String.IsNullOrEmpty(txtAuftragsnummer.Text) _
                AndAlso String.IsNullOrEmpty(txtBatchId.Text) AndAlso String.IsNullOrEmpty(txtSippCode.Text) Then
                lblError.Text = "Bitte geben Sie ein Auswahlkriterium oder einen Zeitraum an"
                Return
            End If

            Dim newRow As DataRow = objBestand.SelFahrzeuge.NewRow()
            'newRow("Fahrgestellnummer") = txtFahrgestellnummer.Text
            'newRow("Kennzeichen") = txtKennzeichen.Text
            'newRow("ZB2Nummer") = txtZB2Nummer.Text
            'newRow("ModelId") = txtModelId.Text
            'newRow("Unitnummer") = txtUnitnummer.Text
            'newRow("Auftragsnummer") = txtAuftragsnummer.Text
            'newRow("BatchId") = txtBatchId.Text
            'newRow("SippCode") = txtSippCode.Text
            'newRow("Hersteller") = ddlHersteller.SelectedValue
            'newRow("PdiStandort") = ddlPdiStandort.SelectedValue
            'newRow("Status") = ddlStatus.SelectedValue

            newRow("CHASSIS_NUM") = txtFahrgestellnummer.Text
            newRow("LICENSE_NUM") = txtKennzeichen.Text
            newRow("ZZREFERENZ1") = txtUnitnummer.Text
            newRow("LIZNR") = txtAuftragsnummer.Text
            newRow("ZUNIT_NR_BIS") = txtBatchId.Text
            newRow("ZZSIPP") = txtSippCode.Text
            newRow("ZZMODELL") = txtModelId.Text
            newRow("ZZHERST_TEXT") = ddlHersteller.SelectedValue
            newRow("TIDNR") = txtZB2Nummer.Text
            newRow("KUNPDI") = ddlPdiStandort.SelectedValue
            newRow("STATUS_TEXT") = ddlStatus.SelectedValue

            objBestand.SelFahrzeuge.Rows.Add(newRow)

            DoSubmit()
        Else
            'Excel-Upload
            If UploadExcelFile() Then
                If objBestand.SelFahrzeuge.Rows.Count = 0 Then
                    Dim newRow As DataRow = objBestand.SelFahrzeuge.NewRow()
                    'newRow("Hersteller") = ddlHersteller.SelectedValue
                    'newRow("PdiStandort") = ddlPdiStandort.SelectedValue
                    'newRow("Status") = ddlStatus.SelectedValue
                    newRow("ZZHERST_TEXT") = ddlHersteller.SelectedValue
                    newRow("KUNPDI") = ddlPdiStandort.SelectedValue
                    newRow("STATUS_TEXT") = ddlStatus.SelectedValue
                    objBestand.SelFahrzeuge.Rows.Add(newRow)
                Else
                    For i As Integer = 0 To (objBestand.SelFahrzeuge.Rows.Count - 1)
                        Dim dRow As DataRow = objBestand.SelFahrzeuge.Rows(i)
                        'dRow("Hersteller") = ddlHersteller.SelectedValue
                        'dRow("PdiStandort") = ddlPdiStandort.SelectedValue
                        'dRow("Status") = ddlStatus.SelectedValue
                        dRow("ZZHERST_TEXT") = ddlHersteller.SelectedValue
                        dRow("KUNPDI") = ddlPdiStandort.SelectedValue
                        dRow("STATUS_TEXT") = ddlStatus.SelectedValue
                    Next
                End If
                DoSubmit()
            End If
        End If
    End Sub

    ''' <summary>
    ''' Daten aus SAP lesen, Ergebnis als Excel-Datei sichern (für Excel-Download-Funktion)
    ''' </summary>
    Private Sub DoSubmit()
        Try
            Dim strFileName As String = Format(Now, "yyyyMMdd_HHmmss_") & m_User.UserName & ".xls"
            
            objBestand.Fill(Session("AppID").ToString, Session.SessionID.ToString)

            Session("ResultTable") = objBestand.Result

            If Not objBestand.Status = "0" Then
                lblError.Text = "Fehler: " & objBestand.Message
            Else
                If objBestand.Result.Rows.Count = 0 Then
                    lblError.Text = "Keine Ergebnisse für die gewählten Kriterien."
                Else
                    Try
                        'Excel.ExcelExport.WriteExcel(objBestand.Result, ConfigurationManager.AppSettings("ExcelPath") & strFileName)

                        ' ### CSV Export
                        strFileName = Format(Now, "yyyyMMdd_HHmmss_") & m_User.UserName & ".csv"
                        GeneralTools.Services.CsvService.CreateCSV(objBestand.Result, ConfigurationManager.AppSettings("ExcelPath") & strFileName)

                        ' ### Aspose
                        'Dim Excelobj As New DocumentGeneration.ExcelDocumentFactory
                        'Excelobj.CreateDocumentAndWriteToFilesystem(ConfigurationManager.AppSettings("ExcelPath") & strFileName, objBestand.Result, Me)
                    Catch
                    End Try

                    Dim AbsoluterPfadZumVirtuellenVerz As String = ConfigurationManager.AppSettings("ReplaceExcelPath")
                    'Session("lnkExcel") = ConfigurationManager.AppSettings("ExcelPath").Replace(AbsoluterPfadZumVirtuellenVerz, "") & strFileName & "".Replace("/", "\")
                    Session("lnkCSV") = ConfigurationManager.AppSettings("ExcelPath").Replace(AbsoluterPfadZumVirtuellenVerz, "") & strFileName & "".Replace("/", "\")

                    Response.Redirect("Report28_2.aspx?AppID=" & Session("AppID").ToString)
                End If
            End If
        Catch ex As Exception
            lblError.Text = "Beim Erstellen des Reportes ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
        End Try
    End Sub
    
    ''' <summary>
    ''' Anzeigen des Kalender-Controls für das jeweilige Datumsfeld
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub btnOpenCalendar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCalEingangZB2_von.Click, btnCalEingangZB2_bis.Click, _
            btnCalEingangFzg_von.Click, btnCalEingangFzg_bis.Click, btnCalBereitmFzg_von.Click, btnCalBereitmFzg_bis.Click, btnCalZulassungsdatum_von.Click, btnCalZulassungsdatum_bis.Click

        Dim btn As LinkButton = DirectCast(sender, LinkButton)
        Dim selDatum As String

        Select Case btn.ID
            Case "btnCalEingangZB2_von"
                ihSelectedCalendar.Value = Kalendermodus.EingangZB2_von
                selDatum = txtEingangZB2_von.Text
            Case "btnCalEingangZB2_bis"
                ihSelectedCalendar.Value = Kalendermodus.EingangZB2_bis
                selDatum = txtEingangZB2_bis.Text
            Case "btnCalEingangFzg_von"
                ihSelectedCalendar.Value = Kalendermodus.EingangFzg_von
                selDatum = txtEingangFzg_von.Text
            Case "btnCalEingangFzg_bis"
                ihSelectedCalendar.Value = Kalendermodus.EingangFzg_bis
                selDatum = txtEingangFzg_bis.Text
            Case "btnCalBereitmFzg_von"
                ihSelectedCalendar.Value = Kalendermodus.BereitmFzg_von
                selDatum = txtBereitmFzg_von.Text
            Case "btnCalBereitmFzg_bis"
                ihSelectedCalendar.Value = Kalendermodus.BereitmFzg_bis
                selDatum = txtBereitmFzg_bis.Text
            Case "btnCalZulassungsdatum_von"
                ihSelectedCalendar.Value = Kalendermodus.ZulDat_von
                selDatum = _txtZulassungsdatum_von.Text
            Case "btnCalZulassungsdatum_bis"
                ihSelectedCalendar.Value = Kalendermodus.ZulDat_bis
                selDatum = _txtZulassungsdatum_bis.Text
            Case Else
                ihSelectedCalendar.Value = ""
                selDatum = ""
        End Select

        If Not String.IsNullOrEmpty(selDatum) AndAlso IsDate(selDatum) Then
            calKalender.SelectedDate = DateTime.Parse(selDatum)
        Else
            calKalender.SelectedDate = Nothing
            calKalender.VisibleDate = Today
        End If

        calKalender.Visible = True
    End Sub

    ''' <summary>
    ''' Ausgewählten Datumswert in entspr. Textbox übernehmen
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub calKalender_SelectionChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles calKalender.SelectionChanged

        Dim modus As Kalendermodus = [Enum].Parse(GetType(Kalendermodus), ihSelectedCalendar.Value)
        Dim selDatum As String = calKalender.SelectedDate.ToShortDateString()

        Select Case modus
            Case Kalendermodus.EingangZB2_von
                txtEingangZB2_von.Text = selDatum
            Case Kalendermodus.EingangZB2_bis
                txtEingangZB2_bis.Text = selDatum
            Case Kalendermodus.EingangFzg_von
                txtEingangFzg_von.Text = selDatum
            Case Kalendermodus.EingangFzg_bis
                txtEingangFzg_bis.Text = selDatum
            Case Kalendermodus.BereitmFzg_von
                txtBereitmFzg_von.Text = selDatum
            Case Kalendermodus.BereitmFzg_bis
                txtBereitmFzg_bis.Text = selDatum
            Case Kalendermodus.ZulDat_von
                txtZulassungsdatum_von.Text = selDatum
            Case Kalendermodus.ZulDat_bis
                txtZulassungsdatum_bis.Text = selDatum
        End Select

        calKalender.Visible = False
    End Sub

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        SetEndASPXAccess(Me)
    End Sub

    Private Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Unload
        SetEndASPXAccess(Me)
    End Sub

End Class