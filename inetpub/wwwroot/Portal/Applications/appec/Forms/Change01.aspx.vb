Imports CKG.Base.Kernel.Common.Common
Imports CKG.Portal.PageElements
Imports System.Data.OleDb

Public Class Change01
    Inherits Page

#Region " Vom Web Form Designer generierter Code "

    'Dieser Aufruf ist für den Web Form-Designer erforderlich.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub

    Private Sub Page_Init(ByVal sender As Object, ByVal e As EventArgs) Handles MyBase.Init
        'CODEGEN: Diese Methode ist für den Web Form-Designer erforderlich
        'Verwenden Sie nicht den Code-Editor zur Bearbeitung.
        InitializeComponent()
    End Sub

#End Region

#Region " Deklarationen "

    Private m_User As Base.Kernel.Security.User
    Private m_App As Base.Kernel.Security.App

    Protected WithEvents ucHeader As Header
    Protected WithEvents lblHead As Label
    Protected WithEvents lblSuccess As Label
    Protected WithEvents txtModelId As TextBox
    Protected WithEvents Image1 As Image
    Protected WithEvents txtSippcode As TextBox
    Protected WithEvents Image2 As Image
    Protected WithEvents ddlHersteller As DropDownList
    Protected WithEvents txtBatchId As TextBox
    Protected WithEvents Image4 As Image
    Protected WithEvents Label1 As Label
    Protected WithEvents txtModell As TextBox
    Protected WithEvents txtDatEinsteuerung As TextBox
    Protected WithEvents Image6 As Image
    Protected WithEvents txtAnzahlFahrzeuge As TextBox
    Protected WithEvents Image7 As Image
    Protected WithEvents txtUnitNrVon As TextBox
    Protected WithEvents txtUnitNrBis As TextBox
    Protected WithEvents Image8 As Image
    Protected WithEvents txtLaufzeit As TextBox
    Protected WithEvents Image9 As Image
    Protected WithEvents cbxLaufz As CheckBox
    Protected WithEvents txtBemerkung As TextBox
    Protected WithEvents Image12 As Image
    Protected WithEvents txtAuftragsnummerVon As TextBox
    Protected WithEvents Image3 As Image
    Protected WithEvents ddlVerwendung As DropDownList
    Protected WithEvents rbLKW As RadioButton
    Protected WithEvents rbPKW As RadioButton
    Protected WithEvents rbJ1 As RadioButton
    Protected WithEvents rbN1 As RadioButton
    Protected WithEvents rbJ2 As RadioButton
    Protected WithEvents rbN2 As RadioButton
    Protected WithEvents cmdCreate As LinkButton
    Protected WithEvents cmdReset As LinkButton
    Protected WithEvents lblError As Label
    Protected WithEvents txtAuftragsnummerBis As TextBox
    Protected WithEvents txtHerstellerHidden As HtmlInputHidden
    Protected WithEvents ucStyles As Styles
    Protected WithEvents Image5 As Image
    Protected WithEvents ddlModellHidden As DropDownList
    Protected WithEvents btnFinished As LinkButton
    Protected WithEvents txtHerstellerBezeichnungHidden As HtmlInputHidden
    Protected WithEvents ddlModellZuHersteller As DropDownList
    Protected WithEvents ddlModellZuSipp As DropDownList
    Protected WithEvents ddlModellZuLaufzeit As DropDownList
    Protected WithEvents ddlModellZuLaufzeitbindung As DropDownList
    Protected WithEvents txtTest As TextBox
    Protected WithEvents rbLeasingNein As RadioButton
    Protected WithEvents rbLeasingJa As RadioButton
    Protected WithEvents rbJAnhaenger As RadioButton
    Protected WithEvents rbNAnhaenger As RadioButton
    Protected WithEvents ddlKennzeichenserie1 As DropDownList
    Protected WithEvents upFileUnitNr As HtmlInputFile
    Private objBatch As ec_01
    Protected WithEvents rb_NaviJa As RadioButton
    Protected WithEvents rb_NaviNein As RadioButton
    Private objChange As change_01
    Protected WithEvents cbxKeepData As CheckBox

#End Region

    Private Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles MyBase.Load
        Session("ShowLink") = "False"
        m_User = GetUser(Me)
        ucHeader.InitUser(m_User)
        FormAuth(Me, m_User)

        GetAppIDFromQueryString(Me)

        Try
            ddlHersteller.Attributes.Add("onChange", "SetHersteller();")
            txtModell.Attributes.Add("onFocus", "setFocus();")
            txtModelId.Attributes.Add("onBlur", "SetModell();")

            lblHead.Text = m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString
            ucStyles.TitleText = lblHead.Text
            m_App = New Base.Kernel.Security.App(m_User)

            If Not IsPostBack Then
                InitialLoad()
            End If

        Catch ex As Exception
            lblError.Text = "Beim Laden der Seite ist ein Fehler aufgetreten. " & ex.Message
        End Try
    End Sub

    Private Sub InitialLoad()
        Dim objSuche As ec_01 ' New ec_01(m_User, m_App, "")
        Dim vwHersteller As DataView
        Dim vwVerwendung As DataView
        Dim vwModell As DataView
        Dim item As ListItem

        objSuche = New ec_01(m_User, m_App, "")

        objSuche.getData(Session("AppID").ToString, Session.SessionID, Me)
        If objSuche.Message <> String.Empty Then
            lblError.Text = objSuche.Message
            Exit Sub
        Else
            'Dropdownlisten befüllen
            '1. Hersteller
            vwHersteller = objSuche.HerstellerAuswahl.DefaultView
            vwHersteller.Sort = "ZHERST asc"

            With ddlHersteller
                .DataSource = vwHersteller
                .DataTextField = "ZHERST"
                .DataValueField = "VALPOS"
                .DataBind()
            End With

            txtHerstellerHidden.Value = ddlHersteller.Items(0).Value
            txtHerstellerBezeichnungHidden.Value = ddlHersteller.Items(0).Text

            '2. Verwendungszweck
            vwVerwendung = objSuche.VerwendungszweckAuswahl.DefaultView
            vwVerwendung.Sort = "ZVERWENDUNG asc"

            With ddlVerwendung
                .DataSource = vwVerwendung
                .DataTextField = "ZVERWENDUNG"
                .DataValueField = "DOMVALUE_L"
                .DataBind()
            End With

            item = ddlVerwendung.Items.FindByValue("SELBSTFAHR")
            If Not (item Is Nothing) Then
                item.Selected = True
            End If

            '3. Modellbezeichnung
            vwModell = objSuche.ModellAuswahl.DefaultView

            With ddlModellHidden
                .DataSource = vwModell
                .DataTextField = "BEZEI"
                .DataValueField = "CODE"
                .DataBind()
            End With

            '4.Zuordnung Modell zu Hersteller
            vwModell = objSuche.ModellAuswahl.DefaultView

            With ddlModellZuHersteller
                .DataSource = vwModell
                .DataTextField = "HERST"
                .DataValueField = "CODE"
                .DataBind()
            End With

            '5.Zuordnung Modell zu Sippcode
            vwModell = objSuche.ModellAuswahl.DefaultView

            With ddlModellZuSipp
                .DataSource = vwModell
                .DataTextField = "Sipp"
                .DataValueField = "CODE"
                .DataBind()
            End With

            '6.Zuordnung Modell zur Laufzeit
            With ddlModellZuLaufzeit
                .DataSource = vwModell
                .DataTextField = "ZLAUFZEIT"
                .DataValueField = "CODE"
                .DataBind()
            End With

            '7.Zuordnung Modell zur Laufzeitbindung
            With ddlModellZuLaufzeitbindung
                .DataSource = vwModell
                .DataTextField = "ZLZBINDUNG"
                .DataValueField = "CODE"
                .DataBind()
            End With



            'füllen der Kennzeichen DropDownListe
            fillKennzeichenserie()

        End If
        Session.Add("objSuche", objSuche)
    End Sub

    Private Sub cmdCreate_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdCreate.Click
        Dim status As Boolean
        Dim uploadUnitnummern As Boolean
        CheckInput(status, uploadUnitnummern)
        If status Then
            DoSubmit(uploadUnitnummern)
        End If
    End Sub

    Private Sub CheckInput(ByRef status As Boolean, ByRef uploadUnitnummern As Boolean)
        Dim patternNumeric As String = "[^0-9]"
        Dim patternAlphaNumeric As String = "[^a-zA-Z0-9]"
        Dim patternDate As String = "^(01|02|03|04|05|06|07|08|09|10|11|12)\.[1-9][0-9][0-9][0-9]"

        Dim regEx As Text.RegularExpressions.Regex
        Dim strModelId As String
        Dim strModellBezeichnung As String
        Dim strSippcode As String
        Dim strBatchId As String
        Dim strDatEinsteuerung As String
        Dim strAnzahl As String
        Dim strUnitVon As String
        Dim strUnitBis As String
        Dim lngUnitVon As Long
        Dim lngUnitBis As Long
        Dim strLaufzeit As String

        status = True
        uploadUnitnummern = False

        'Eingabewerte prüfen
        lblError.Text = String.Empty

        'Model-Id
        strModelId = txtModelId.Text.ToUpper.Trim

        If strModelId = String.Empty Then
            lblError.Text = "Bitte eine Model-Id eingeben."
            status = False
            Exit Sub
        End If
        regEx = New Text.RegularExpressions.Regex(patternAlphaNumeric)
        If (regEx.IsMatch(strModelId)) Then
            lblError.Text = "Model-Id ungültig."
            status = False
            Exit Sub
        End If

        'Modellbezeichnung
        strModellBezeichnung = txtModell.Text
        If (strModellBezeichnung = String.Empty) Then
            lblError.Text = "Model-Id ungültig (Keine Modellbezeichnung gefunden)."
            status = False
            Exit Sub
        End If

        'Sipp-Code
        strSippcode = txtSippcode.Text.ToUpper.Trim

        If strSippcode = String.Empty Then
            lblError.Text = "Bitte einen SIPP-Code eingeben."
            status = False
            Exit Sub
        End If
        regEx = New Text.RegularExpressions.Regex(patternAlphaNumeric)
        If (regEx.IsMatch(strSippcode)) Then
            lblError.Text = "SIPP-Code ungültig."
            status = False
            Exit Sub
        End If

        'Batch-Id
        strBatchId = txtBatchId.Text.ToUpper.Trim

        If strBatchId = String.Empty Then
            lblError.Text = "Bitte eine Batch-Id eingeben."
            status = False
            Exit Sub
        End If
        regEx = New Text.RegularExpressions.Regex(patternNumeric)
        If (regEx.IsMatch(strBatchId)) Then
            lblError.Text = "Batch-Id ungültig."
            status = False
            Exit Sub
        End If

        'Einsteuerung
        strDatEinsteuerung = txtDatEinsteuerung.Text.Trim

        If strDatEinsteuerung = String.Empty Then
            lblError.Text = "Bitte eine Datum Einsteuerung eingeben."
            status = False
            Exit Sub
        End If

        regEx = New Text.RegularExpressions.Regex(patternDate)
        If Not (regEx.IsMatch(strDatEinsteuerung)) Then
            lblError.Text = "Datum Einsteuerung ungültig."
            status = False
            Exit Sub
        End If

        'Anzahl Fahrzeuge
        strAnzahl = txtAnzahlFahrzeuge.Text.Trim

        If strAnzahl = String.Empty Then
            lblError.Text = "Bitte die Anzahl Fahrzeuge eingeben."
            status = False
            Exit Sub
        End If

        regEx = New Text.RegularExpressions.Regex(patternNumeric)
        If regEx.IsMatch(strAnzahl) Then
            lblError.Text = "Anzahl Fahrzeuge ungültig."
            status = False
            Exit Sub
        End If

        'Laufzeit
        strLaufzeit = txtLaufzeit.Text.Trim

        If strLaufzeit = String.Empty Then
            lblError.Text = "Bitte Laufzeit eingeben."
            status = False
            Exit Sub
        End If

        regEx = New Text.RegularExpressions.Regex(patternNumeric)

        If regEx.IsMatch(strLaufzeit) Then
            lblError.Text = "Laufzeit ungültig."
            status = False
            Exit Sub
        End If

        'Unit-Nummer (entweder von,bis oder per Upload-File)
        strUnitVon = txtUnitNrVon.Text.Trim
        strUnitBis = txtUnitNrBis.Text.Trim

        If strUnitVon = String.Empty And strUnitBis = String.Empty Then
            uploadUnitnummern = True
            If (Not upFileUnitNr.PostedFile Is Nothing) AndAlso (Not (upFileUnitNr.PostedFile.FileName = String.Empty)) Then
                If Right(upFileUnitNr.PostedFile.FileName.ToUpper, 4) <> ".XLS" AndAlso Right(upFileUnitNr.PostedFile.FileName.ToUpper, 5) <> ".XLSX" Then
                    lblError.Text = "Es können nur Dateien im .XLS oder .XLSX-Format verarbeitet werden."
                    status = False
                    Exit Sub
                End If
            Else
                lblError.Text = "Bitte entweder Unit-Nr. (von - bis) oder eine Upload-Datei mit Unit-Nummern angeben"
                status = False
                Exit Sub
            End If
        Else
            If strUnitVon = String.Empty Or strUnitBis = String.Empty Then
                lblError.Text = "Bitte Unit-Nr. (von - bis) eingeben."
                status = False
                Exit Sub
            End If

            regEx = New Text.RegularExpressions.Regex(patternNumeric)

            If regEx.IsMatch(strUnitVon) Then
                lblError.Text = "Unit-Nr. (von) ungültig."
                status = False
                Exit Sub
            End If

            If regEx.IsMatch(strUnitBis) Then
                lblError.Text = "Unit-Nr. (bis) ungültig."
                status = False
                Exit Sub
            End If

            lngUnitVon = CType(strUnitVon, Long)
            lngUnitBis = CType(strUnitBis, Long)

            If Not ((lngUnitBis - lngUnitVon) = (CType(strAnzahl, Long) - 1)) Then
                lblError.Text = "Unit-Nr. (von-bis) ungültig."
                status = False
                Exit Sub
            End If
        End If

    End Sub

    Private Sub upload(ByVal uFile As HttpPostedFile)

        Dim filepath As String = ConfigurationManager.AppSettings("ExcelPath")
        Dim filename As String = ""
        Dim info As IO.FileInfo

        'Dateiname: User_yyyyMMddhhmmss.xls
        If Right(upFileUnitNr.PostedFile.FileName.ToUpper, 4) = ".XLS" Then
            filename = m_User.UserName & "_" & Format(Now, "yyyyMMddhhmmss") & ".xls"
        End If
        If Right(upFileUnitNr.PostedFile.FileName.ToUpper, 5) = ".XLSX" Then
            filename = m_User.UserName & "_" & Format(Now, "yyyyMMddhhmmss") & ".xlsx"
        End If

        Try
            If Not (uFile Is Nothing) Then
                uFile.SaveAs(ConfigurationManager.AppSettings("ExcelPath") & filename)
                info = New IO.FileInfo(filepath & filename)
                If Not (info.Exists) Then
                    lblError.Text = "Fehler beim Speichern."
                    Exit Sub
                End If

                'Datei gespeichert -> Auswertung (ohne Spaltenheader)
                Dim sConnectionString As String
                If Right(upFileUnitNr.PostedFile.FileName.ToUpper, 4) = ".XLS" Then
                    sConnectionString = "Provider=Microsoft.Jet.OLEDB.4.0;" & _
                     "Data Source=" & filepath & filename & ";" & _
                     "Extended Properties=""Excel 8.0;HDR=No"""
                Else
                    sConnectionString = "Provider=Microsoft.ACE.OLEDB.12.0;" + "Data Source=" + filepath + filename + _
                    ";Extended Properties=""Excel 12.0 Xml;HDR=No"""
                End If


                Dim objConn As New OleDbConnection(sConnectionString)
                objConn.Open()
                Dim objDataset1 As New DataSet()

                Dim schemaTable As DataTable
                Dim tmpObj() As Object = {Nothing, Nothing, Nothing, "Table"}
                schemaTable = objConn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, tmpObj)

                For Each sheet As DataRow In schemaTable.Rows
                    Dim tableName As String = sheet("Table_Name").ToString
                    Dim objCmdSelect As New OleDbCommand("SELECT * FROM [" & tableName & "]", objConn)
                    Dim objAdapter1 As New OleDbDataAdapter(objCmdSelect)
                    objAdapter1.Fill(objDataset1, tableName)
                Next
                Dim TempTable As DataTable = objDataset1.Tables(0)
                objConn.Close()


                objBatch.Unitnummern = New DataTable
                objBatch.Unitnummern.Columns.Add("ZUNIT_NR")
                objBatch.Unitnummern.AcceptChanges()

                Dim NewRow As DataRow

                For Each dr As DataRow In TempTable.Rows
                    NewRow = objBatch.Unitnummern.NewRow

                    If dr(0).ToString.Length > 0 Then
                        NewRow("ZUNIT_NR") = dr(0).ToString
                        objBatch.Unitnummern.Rows.Add(NewRow)
                    End If
                Next

                objBatch.Unitnummern.AcceptChanges()

            End If
        Catch ex As Exception
            lblError.Text = "Fehler beim hochladen der Datei! " & ex.Message
        End Try

    End Sub

    Private Sub fillKennzeichenserie()

        objChange = New change_01(m_User, m_App, Session("AppID").ToString, Session.SessionID, "")

        For Each row As DataRow In objChange.PKennzeichenSerie.Rows
            Dim item As New ListItem(row.Item("Serie"), row.Item("ID"))
            ddlKennzeichenserie1.Items.Add(item)
        Next

    End Sub

    Private Sub clearControls(Optional ByVal clearAll As Boolean = True)
        txtModelId.Text = String.Empty
        txtSippcode.Text = String.Empty
        txtBatchId.Text = String.Empty
        txtAnzahlFahrzeuge.Text = String.Empty
        txtUnitNrVon.Text = String.Empty
        txtUnitNrBis.Text = String.Empty

        If clearAll Then
            txtLaufzeit.Text = String.Empty
            cbxLaufz.Checked = False
            txtBemerkung.Text = String.Empty
            txtAuftragsnummerVon.Text = String.Empty
            txtAuftragsnummerBis.Text = String.Empty
            rbPKW.Checked = True
            rbN1.Checked = True
            rbNAnhaenger.Checked = True
            rb_NaviNein.Checked = True
            rbN2.Checked = True
            rbLeasingNein.Checked = True
        End If
    End Sub

    Private Sub collectData(uploadUnitnummern As Boolean)

        With objBatch
            .ModelID = txtModelId.Text
            .ModellBezeichnung = txtModell.Text
            .SippCode = txtSippcode.Text
            .Hersteller = txtHerstellerHidden.Value
            .HerstellerBezeichnung = txtHerstellerBezeichnungHidden.Value
            .BarchId = txtBatchId.Text
            .DatumEinsteuerung = txtDatEinsteuerung.Text
            .AnzahlFahrzeuge = txtAnzahlFahrzeuge.Text

            .Laufzeit = txtLaufzeit.Text
            .KennzeichenSerie = ddlKennzeichenserie1.SelectedItem.Text

            If cbxLaufz.Checked Then
                .LaufzeitBindung = True
            Else
                .LaufzeitBindung = False
            End If

            .Bemerkungen = txtBemerkung.Text
            .AuftragsNrVon = txtAuftragsnummerVon.Text
            .AuftragsNrBis = txtAuftragsnummerBis.Text
            .Verwendungszweck = ddlVerwendung.SelectedItem.Value
            .VerwendungszweckBezeichnung = ddlVerwendung.SelectedItem.Text

            If rbPKW.Checked Then
                .Fahrzeuggruppe = True
            Else
                .Fahrzeuggruppe = False
            End If

            If rbJ1.Checked Then
                .WinterBereifung = True
            Else
                .WinterBereifung = False
            End If

            If rbJAnhaenger.Checked Then
                .Anhaengerkupplung = True
            Else
                .Anhaengerkupplung = False
            End If

            If rbJ2.Checked Then
                .SecurFleet = True
            Else
                .SecurFleet = False
            End If

            If rbLeasingNein.Checked Then
                .Leasing = False
            Else
                .Leasing = True
            End If

            If rb_NaviJa.Checked Then
                .Navi = True
            Else
                .Navi = False
            End If

            If uploadUnitnummern Then
                .UnitNrVon = "0"
                .UnitNrBis = "0"
                'Unitnummern aus Excel-Uploadfile übernehmen
                upload(upFileUnitNr.PostedFile)
            Else
                .UnitNrVon = txtUnitNrVon.Text
                .UnitNrBis = txtUnitNrBis.Text
            End If

        End With

        Session("objSuche") = objBatch
    End Sub

    Private Sub DoSubmit(uploadUnitnummern As Boolean)
        Dim status As String = ""

        objBatch = New ec_01(m_User, m_App, "")

        collectData(uploadUnitnummern)

        If objBatch.Selection < 0 Then  'Neuer Datensatz
            objBatch.addNewRow()
            clearControls(Not cbxKeepData.Checked)
        Else                            'Vorhandener Datensatz, nur aktualisieren
            objBatch.updateExistingRow(status)
            If (status <> String.Empty) Then
                lblError.Text = status
            End If
        End If

        Session("objSuche") = objBatch


        Dim row As DataRow

        For Each row In objBatch.ResultTable.Rows
            If (TypeOf (row("Status")) Is DBNull) OrElse (CType(row("Status"), String) <> "Gespeichert.") Then  'Nur die noch nicht abgesendeten Vorgänge...
                objBatch.saveData(Session("AppID").ToString, Session.SessionID, row, Me)
                If (objBatch.Status = 0) Then
                    row("Status") = "Gespeichert."
                    lblSuccess.Text = "Daten gesichert."
                Else
                    row("Status") = objBatch.Message
                    lblError.Text = "Die Daten konnten nicht gespeichert werden. Bitte überprüfen Sie Ihre Eingaben. (" & objBatch.Message & ")"
                End If
            End If
            Exit For
        Next

        objBatch = Nothing

    End Sub

    Private Sub btnFinished_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnFinished.Click
        If objBatch.ResultTable.Rows.Count = 0 Then
            lblError.Text = "Es wurden noch keine Daten erfasst."
            Exit Sub
        End If
        Response.Redirect("Change01_2.aspx?AppID=" & Session("AppID").ToString)
    End Sub

    Private Sub cmdReset_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdReset.Click
        clearControls()
    End Sub

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As EventArgs) Handles MyBase.PreRender
        SetEndASPXAccess(Me)
    End Sub

    Private Sub Page_Unload(ByVal sender As Object, ByVal e As EventArgs) Handles MyBase.Unload
        SetEndASPXAccess(Me)
    End Sub
End Class
