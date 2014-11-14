Imports CKG.Base.Business
Imports CKG.Base.Kernel.Common.Common
Imports CKG.Portal.PageElements
Imports System.Data.OleDb

Public Class Change01
    Inherits Page

#Region " Vom Web Form Designer generierter Code "

    'Dieser Aufruf ist für den Web Form-Designer erforderlich.
    <Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

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

    Private objBatch As ec_01

    Private Const patternNumeric As String = "[^0-9]"
    Private Const patternAlphaNumeric As String = "[^a-zA-Z0-9]"
    Private Const patternDate As String = "^(01|02|03|04|05|06|07|08|09|10|11|12)\.[1-9][0-9][0-9][0-9]"

    Protected WithEvents ucStyles As Styles
    Protected WithEvents ucHeader As Header
    Protected WithEvents lblHead As Label
    Protected WithEvents lblTask As Label
    Protected WithEvents trBack As HtmlTableRow
    Protected WithEvents cmdBack As LinkButton
    Protected WithEvents trActionSelection As HtmlTableRow
    Protected WithEvents cmdNeuanlage As LinkButton
    Protected WithEvents cmdDatenpflege As LinkButton
    Protected WithEvents trSearchFilter As HtmlTableRow
    Protected WithEvents CalAnlDatVon As Calendar
    Protected WithEvents CalAnlDatBis As Calendar
    Protected WithEvents txtFilterBatchIdVon As TextBox
    Protected WithEvents txtFilterBatchIdBis As TextBox
    Protected WithEvents txtFilterUnitVon As TextBox
    Protected WithEvents txtFilterUnitBis As TextBox
    Protected WithEvents txtFilterModelIdVon As TextBox
    Protected WithEvents txtFilterModelIdBis As TextBox
    Protected WithEvents txtFilterEinstMonatVon As TextBox
    Protected WithEvents txtFilterEinstMonatBis As TextBox
    Protected WithEvents txtFilterErfasser As TextBox
    Protected WithEvents txtFilterAnlagedatumVon As TextBox
    Protected WithEvents btnKalenderAnlagedatumVon As LinkButton
    Protected WithEvents txtFilterAnlagedatumBis As TextBox
    Protected WithEvents btnKalenderAnlagedatumBis As LinkButton
    Protected WithEvents cmdSearch As LinkButton
    Protected WithEvents trSearchResult As HtmlTableRow
    Protected WithEvents dgBatches As DataGrid
    Protected WithEvents trEditBatch As HtmlTableRow
    Protected WithEvents lblError As Label
    Protected WithEvents lblSuccess As Label
    Protected WithEvents txtModelId As TextBox
    Protected WithEvents txtModell As TextBox
    Protected WithEvents txtSippcode As TextBox
    Protected WithEvents ddlHersteller As DropDownList
    Protected WithEvents txtBatchId As TextBox
    Protected WithEvents txtDatEinsteuerung As TextBox
    Protected WithEvents txtAnzahlFahrzeuge As TextBox
    Protected WithEvents txtUnitNrVon As TextBox
    Protected WithEvents txtUnitNrBis As TextBox
    Protected WithEvents upFileUnitNr As HtmlInputFile
    Protected WithEvents txtLaufzeit As TextBox
    Protected WithEvents cbxLaufz As CheckBox
    Protected WithEvents txtBemerkung As TextBox
    Protected WithEvents txtAuftragsnummerVon As TextBox
    Protected WithEvents txtAuftragsnummerBis As TextBox
    Protected WithEvents ddlVerwendung As DropDownList
    Protected WithEvents ddlKennzeichenserie1 As DropDownList
    Protected WithEvents rbLKW As RadioButton
    Protected WithEvents rbPKW As RadioButton
    Protected WithEvents rbJ1 As RadioButton
    Protected WithEvents rbN1 As RadioButton
    Protected WithEvents ddlModellZuHersteller As DropDownList
    Protected WithEvents ddlModellZuSipp As DropDownList
    Protected WithEvents ddlModellZuLaufzeit As DropDownList
    Protected WithEvents ddlModellZuLaufzeitbindung As DropDownList
    Protected WithEvents rbJAnhaenger As RadioButton
    Protected WithEvents rbNAnhaenger As RadioButton
    Protected WithEvents rb_NaviJa As RadioButton
    Protected WithEvents rb_NaviNein As RadioButton
    Protected WithEvents rbJ2 As RadioButton
    Protected WithEvents rbN2 As RadioButton
    Protected WithEvents rbLeasingJa As RadioButton
    Protected WithEvents rbLeasingNein As RadioButton
    Protected WithEvents cbxKeepData As CheckBox
    Protected WithEvents trKeepData As HtmlTableRow
    Protected WithEvents ddlModellHidden As DropDownList
    Protected WithEvents cmdSave As LinkButton
    Protected WithEvents cmdReset As LinkButton
    Protected WithEvents txtHerstellerHidden As HtmlInputHidden
    Protected WithEvents txtHerstellerBezeichnungHidden As HtmlInputHidden

#End Region

#Region " Events "

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
                objBatch = New ec_01(m_User, m_App, "")
                Session("objBatch") = objBatch
                InitialLoad()
            Else
                If Session("objBatch") IsNot Nothing Then
                    objBatch = CType(Session("objBatch"), ec_01)
                End If
            End If

        Catch ex As Exception
            lblError.Text = "Beim Laden der Seite ist ein Fehler aufgetreten. " & ex.Message
        End Try
    End Sub

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As EventArgs) Handles MyBase.PreRender
        SetEndASPXAccess(Me)
    End Sub

    Private Sub Page_Unload(ByVal sender As Object, ByVal e As EventArgs) Handles MyBase.Unload
        SetEndASPXAccess(Me)
    End Sub

    Private Sub cmdNeuanlage_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdNeuanlage.Click
        trActionSelection.Visible = False
        trEditBatch.Visible = True
    End Sub

    Private Sub cmdDatenpflege_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdDatenpflege.Click
        trActionSelection.Visible = False
        trSearchFilter.Visible = True
    End Sub

    Protected Sub btnKalenderAnlagedatumVon_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnKalenderAnlagedatumVon.Click
        CalAnlDatVon.Visible = True
        CalAnlDatBis.Visible = False
    End Sub

    Protected Sub btnKalenderAnlagedatumBis_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnKalenderAnlagedatumBis.Click
        CalAnlDatVon.Visible = False
        CalAnlDatBis.Visible = True
    End Sub

    Protected Sub CalAnlDatVon_SelectionChanged(ByVal sender As Object, ByVal e As EventArgs) Handles CalAnlDatVon.SelectionChanged
        txtFilterAnlagedatumVon.Text = CalAnlDatVon.SelectedDate.ToShortDateString
        CalAnlDatVon.Visible = False
    End Sub

    Protected Sub CalAnlDatBis_SelectionChanged(ByVal sender As Object, ByVal e As EventArgs) Handles CalAnlDatBis.SelectionChanged
        txtFilterAnlagedatumBis.Text = CalAnlDatBis.SelectedDate.ToShortDateString
        CalAnlDatBis.Visible = False
    End Sub

    Private Sub cmdSearch_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdSearch.Click
        If CheckFilterInputOk() Then
            Suchen()
        End If
    End Sub

    Private Sub dgBatches_SortCommand(ByVal source As Object, ByVal e As DataGridSortCommandEventArgs) Handles dgBatches.SortCommand
        FillGrid(dgBatches.CurrentPageIndex, e.SortExpression)
    End Sub

    Private Sub dgBatches_PageIndexChanged(ByVal source As Object, ByVal e As DataGridPageChangedEventArgs) Handles dgBatches.PageIndexChanged
        FillGrid(e.NewPageIndex)
    End Sub

    Private Sub dgBatches_ItemCommand(ByVal source As Object, ByVal e As DataGridCommandEventArgs) Handles dgBatches.ItemCommand
        If e.CommandName = "Bearbeiten" Then
            ShowBatch(e.CommandArgument.ToString())
        End If
    End Sub

    Private Sub cmdSave_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdSave.Click
        Dim uploadUnitnummern As Boolean
        If CheckEditInputOk(uploadUnitnummern) Then
            Speichern(uploadUnitnummern)
        End If
    End Sub

    Private Sub cmdReset_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdReset.Click
        clearControls()
    End Sub

    Protected Sub cmdBack_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdBack.Click
        If trSearchResult.Visible Then
            trBack.Visible = False
            trSearchResult.Visible = False
            trSearchFilter.Visible = True
        ElseIf trEditBatch.Visible Then
            trEditBatch.Visible = False
            trSearchResult.Visible = True
        End If
    End Sub

#End Region

#Region " Methods "

    Private Sub InitialLoad()
        Dim vwHersteller As DataView
        Dim vwVerwendung As DataView
        Dim vwModell As DataView
        Dim item As ListItem

        With objBatch
            .getStammdaten(Session("AppID").ToString, Session.SessionID)
            If Not String.IsNullOrEmpty(.Message) Then
                lblError.Text = .Message
                Exit Sub
            Else
                'Dropdownlisten befüllen
                '1. Hersteller
                vwHersteller = .HerstellerAuswahl.DefaultView
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
                vwVerwendung = .VerwendungszweckAuswahl.DefaultView
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
                vwModell = .ModellAuswahl.DefaultView

                With ddlModellHidden
                    .DataSource = vwModell
                    .DataTextField = "BEZEI"
                    .DataValueField = "CODE"
                    .DataBind()
                End With

                '4. Zuordnung Modell zu Hersteller
                With ddlModellZuHersteller
                    .DataSource = vwModell
                    .DataTextField = "HERST"
                    .DataValueField = "CODE"
                    .DataBind()
                End With

                '5. Zuordnung Modell zu Sippcode
                With ddlModellZuSipp
                    .DataSource = vwModell
                    .DataTextField = "Sipp"
                    .DataValueField = "CODE"
                    .DataBind()
                End With

                '6. Zuordnung Modell zur Laufzeit
                With ddlModellZuLaufzeit
                    .DataSource = vwModell
                    .DataTextField = "ZLAUFZEIT"
                    .DataValueField = "CODE"
                    .DataBind()
                End With

                '7. Zuordnung Modell zur Laufzeitbindung
                With ddlModellZuLaufzeitbindung
                    .DataSource = vwModell
                    .DataTextField = "ZLZBINDUNG"
                    .DataValueField = "CODE"
                    .DataBind()
                End With

                '8. Kennzeichenserie
                Dim objChange As New change_01(m_User, m_App, Session("AppID").ToString, Session.SessionID, "")
                With ddlKennzeichenserie1
                    .DataSource = objChange.PKennzeichenSerie.DefaultView
                    .DataTextField = "Serie"
                    .DataValueField = "SONDERSERIE"
                    .DataBind()
                End With

            End If
        End With
        
        Session("objBatch") = objBatch
    End Sub

    Private Function CheckFilterInputOk() As Boolean
        Try
            Dim regEx As Text.RegularExpressions.Regex

            'Eingabewerte prüfen
            lblError.Text = String.Empty

            'Batch-Id
            Dim strBatchIdVon As String = txtFilterBatchIdVon.Text.ToUpper.Trim
            Dim strBatchIdBis As String = txtFilterBatchIdBis.Text.ToUpper.Trim

            If (Not String.IsNullOrEmpty(strBatchIdVon) OrElse Not String.IsNullOrEmpty(strBatchIdBis)) Then
                regEx = New Text.RegularExpressions.Regex(patternNumeric)

                If regEx.IsMatch(strBatchIdVon) Then
                    lblError.Text = "Batch-ID (von) ungültig."
                    Return False
                End If
                If regEx.IsMatch(strBatchIdBis) Then
                    lblError.Text = "Batch-ID (bis) ungültig."
                    Return False
                End If

                Dim lngBatchVon As Long = CType(strBatchIdVon, Long)
                Dim lngBatchBis As Long = CType(strBatchIdBis, Long)

                If (lngBatchBis < lngBatchVon) Then
                    lblError.Text = "Batch-ID (von-bis) ungültig."
                    Return False
                End If
            End If

            'Unit-Nummer
            Dim strUnitVon As String = txtFilterUnitVon.Text.Trim
            Dim strUnitBis As String = txtFilterUnitBis.Text.Trim

            If (Not String.IsNullOrEmpty(strUnitVon) OrElse Not String.IsNullOrEmpty(strUnitBis)) Then
                regEx = New Text.RegularExpressions.Regex(patternNumeric)

                If regEx.IsMatch(strUnitVon) Then
                    lblError.Text = "Unit-Nr. (von) ungültig."
                    Return False
                End If
                If regEx.IsMatch(strUnitBis) Then
                    lblError.Text = "Unit-Nr. (bis) ungültig."
                    Return False
                End If

                Dim lngUnitVon As Long = CType(strUnitVon, Long)
                Dim lngUnitBis As Long = CType(strUnitBis, Long)

                If (lngUnitBis < lngUnitVon) Then
                    lblError.Text = "Unit-Nr. (von-bis) ungültig."
                    Return False
                End If
            End If

            'Model-Id
            Dim strModelIdVon As String = txtFilterModelIdVon.Text.ToUpper.Trim
            Dim strModelIdBis As String = txtFilterModelIdBis.Text.ToUpper.Trim

            If (Not String.IsNullOrEmpty(strModelIdVon) OrElse Not String.IsNullOrEmpty(strModelIdBis)) Then
                regEx = New Text.RegularExpressions.Regex(patternAlphaNumeric)

                If regEx.IsMatch(strModelIdVon) Then
                    lblError.Text = "Model-ID (von) ungültig."
                    Return False
                End If
                If regEx.IsMatch(strModelIdBis) Then
                    lblError.Text = "Model-ID (bis) ungültig."
                    Return False
                End If
            End If

            'Einsteuerung
            Dim strDatEinsteuerungVon As String = txtFilterEinstMonatVon.Text.Trim
            Dim strDatEinsteuerungBis As String = txtFilterEinstMonatBis.Text.Trim

            If (Not String.IsNullOrEmpty(strDatEinsteuerungVon) OrElse Not String.IsNullOrEmpty(strDatEinsteuerungBis)) Then
                regEx = New Text.RegularExpressions.Regex(patternDate)

                If Not (regEx.IsMatch(strDatEinsteuerungVon)) Then
                    lblError.Text = "Datum Einsteuerung (von) ungültig."
                    Return False
                End If
                If Not (regEx.IsMatch(strDatEinsteuerungBis)) Then
                    lblError.Text = "Datum Einsteuerung (bis) ungültig."
                    Return False
                End If
            End If

            'Anlagedatum
            Dim errorText As String
            If Not HelpProcedures.checkDate(txtFilterAnlagedatumVon, txtFilterAnlagedatumBis, errorText, True) Then
                lblError.Text = errorText
                Return False
            End If

            Return True

        Catch ex As Exception
            lblError.Text = "Fehler bei der Prüfung der eingegebenen Daten: " & ex.Message
            Return False
        End Try

    End Function

    Private Sub collectFilterData()

        With objBatch
            .FilterBatchIdVon = txtFilterBatchIdVon.Text
            .FilterBatchIdBis = txtFilterBatchIdBis.Text
            .FilterUnitnrVon = txtFilterUnitVon.Text
            .FilterUnitnrBis = txtFilterUnitBis.Text
            .FilterModelIdVon = txtFilterModelIdVon.Text
            .FilterModelIdBis = txtFilterModelIdBis.Text
            .FilterEinsteuerungVon = txtFilterEinstMonatVon.Text
            .FilterEinsteuerungBis = txtFilterEinstMonatBis.Text
            .FilterErfasser = txtFilterErfasser.Text
            .FilterAnlagedatumVon = txtFilterAnlagedatumVon.Text
            .FilterAnlagedatumBis = txtFilterAnlagedatumBis.Text
        End With

        Session("objBatch") = objBatch
    End Sub

    Private Sub Suchen(Optional ByVal keepFilterValues As Boolean = False)

        If Not keepFilterValues Then
            collectFilterData()
        End If

        objBatch.loadData(Session("AppID").ToString, Session.SessionID)

        Session("objBatch") = objBatch

        If objBatch.Status = 0 Then
            FillGrid(0)
        Else
            lblError.Text = "Fehler beim Laden der Daten: " & objBatch.Message
        End If

    End Sub

    Private Sub FillGrid(ByVal intPageIndex As Int32, Optional ByVal strSort As String = "")
        If objBatch.Batche.Rows.Count = 0 Then
            trSearchFilter.Visible = True
            lblError.Text = "Keine Ergebnisse für die gewählten Kriterien."
        Else
            trBack.Visible = True
            trSearchFilter.Visible = False
            trSearchResult.Visible = True

            Dim tmpDataView As New DataView(objBatch.Batche)

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

            dgBatches.CurrentPageIndex = intTempPageIndex

            dgBatches.DataSource = tmpDataView
            dgBatches.DataBind()

            If dgBatches.PageCount > 1 Then
                dgBatches.PagerStyle.Visible = True
                dgBatches.PagerStyle.CssClass = "PagerStyle"
                If dgBatches.CurrentPageIndex = dgBatches.PageCount - 1 Then
                    dgBatches.PagerStyle.NextPageText = "<img border=""0"" src=""/Portal/Images/empty.gif"" width=""12"" height=""11"">"
                Else
                    dgBatches.PagerStyle.NextPageText = "<img border=""0"" src=""/Portal/Images/arrow_right.gif"" width=""12"" height=""11"">"
                End If

                If dgBatches.CurrentPageIndex = 0 Then
                    dgBatches.PagerStyle.PrevPageText = "<img border=""0"" src=""/Portal/Images/empty.gif"" width=""12"" height=""11"">"
                Else
                    dgBatches.PagerStyle.PrevPageText = "<img border=""0"" src=""/Portal/Images/arrow_left.gif"" width=""12"" height=""11"">"
                End If
                dgBatches.DataBind()
            Else
                dgBatches.PagerStyle.Visible = False
            End If
        End If
    End Sub

    Private Sub ShowBatch(ByVal batchid As String)
        With objBatch
            Dim rows() As DataRow = .Batche.Select("ZBATCH_ID = '" & batchid & "'")
            If rows.Length > 0 Then
                Dim row As DataRow = rows(0)

                trSearchResult.Visible = False
                trEditBatch.Visible = True
                trKeepData.Visible = False
                txtBatchId.Enabled = False
                cmdReset.Visible = False

                txtModelId.Text = row("ZMODEL_ID").ToString()
                txtModell.Text = row("ZMOD_DESCR").ToString()
                txtSippcode.Text = row("ZSIPP_CODE").ToString()
                ddlHersteller.SelectedValue = ddlHersteller.Items.FindByText(row("ZMAKE").ToString()).Value
                txtBatchId.Text = row("ZBATCH_ID").ToString()
                txtDatEinsteuerung.Text = row("ZPURCH_MTH").ToString()
                txtAnzahlFahrzeuge.Text = row("ZANZAHL").ToString()
                txtLaufzeit.Text = row("ZLAUFZEIT").ToString()
                ddlKennzeichenserie1.SelectedValue = row("ZSONDERSERIE").ToString()
                cbxLaufz.Checked = (row("ZLZBINDUNG").ToString() = "X")
                txtBemerkung.Text = row("ZBEMERKUNG").ToString()
                txtAuftragsnummerVon.Text = row("ZAUFNR_VON").ToString()
                txtAuftragsnummerBis.Text = row("ZAUFNR_BIS").ToString()
                ddlVerwendung.SelectedValue = row("ZVERWENDUNG").ToString()
                
                rbPKW.Checked = row("ZFZG_GROUP").ToString() = "X"
                rbLKW.Checked = row("ZFZG_GROUP").ToString() <> "X"

                rbJ1.Checked = row("ZMS_REIFEN").ToString() = "X"
                rbN1.Checked = row("ZMS_REIFEN").ToString() <> "X"

                rbJAnhaenger.Checked = row("ZAHK").ToString() = "X"
                rbNAnhaenger.Checked = row("ZAHK").ToString() <> "X"

                rbJ2.Checked = row("ZSECU_FLEET").ToString() = "X"
                rbN2.Checked = row("ZSECU_FLEET").ToString() <> "X"

                rbLeasingJa.Checked = row("ZLEASING").ToString() = "X"
                rbLeasingNein.Checked = row("ZLEASING").ToString() <> "X"

                rb_NaviJa.Checked = row("ZNAVI").ToString() = "X"
                rb_NaviNein.Checked = row("ZNAVI").ToString() <> "X"

                txtUnitNrVon.Text = row("ZUNIT_NR_VON").ToString()
                txtUnitNrBis.Text = row("ZUNIT_NR_BIS").ToString()

            Else
                lblError.Text = "Fehler beim Anzeigen des Batches"

            End If
        End With

        Session("objBatch") = objBatch
    End Sub

    Private Function CheckEditInputOk(ByRef uploadUnitnummern As Boolean) As Boolean
        Try
            Dim regEx As Text.RegularExpressions.Regex

            'Eingabewerte prüfen
            lblError.Text = String.Empty

            'Model-Id
            Dim strModelId As String = txtModelId.Text.ToUpper.Trim

            If strModelId = String.Empty Then
                lblError.Text = "Bitte eine Model-Id eingeben."
                Return False
            End If
            regEx = New Text.RegularExpressions.Regex(patternAlphaNumeric)
            If (regEx.IsMatch(strModelId)) Then
                lblError.Text = "Model-Id ungültig."
                Return False
            End If

            'Modellbezeichnung
            Dim strModellBezeichnung As String = txtModell.Text

            If (strModellBezeichnung = String.Empty) Then
                lblError.Text = "Model-Id ungültig (Keine Modellbezeichnung gefunden)."
                Return False
            End If

            'Sipp-Code
            Dim strSippcode As String = txtSippcode.Text.ToUpper.Trim

            If strSippcode = String.Empty Then
                lblError.Text = "Bitte einen SIPP-Code eingeben."
                Return False
            End If
            regEx = New Text.RegularExpressions.Regex(patternAlphaNumeric)
            If (regEx.IsMatch(strSippcode)) Then
                lblError.Text = "SIPP-Code ungültig."
                Return False
            End If

            'Batch-Id
            Dim strBatchId As String = txtBatchId.Text.ToUpper.Trim

            If strBatchId = String.Empty Then
                lblError.Text = "Bitte eine Batch-Id eingeben."
                Return False
            End If
            regEx = New Text.RegularExpressions.Regex(patternNumeric)
            If (regEx.IsMatch(strBatchId)) Then
                lblError.Text = "Batch-Id ungültig."
                Return False
            End If

            'Einsteuerung
            Dim strDatEinsteuerung As String = txtDatEinsteuerung.Text.Trim

            If strDatEinsteuerung = String.Empty Then
                lblError.Text = "Bitte eine Datum Einsteuerung eingeben."
                Return False
            End If

            regEx = New Text.RegularExpressions.Regex(patternDate)
            If Not (regEx.IsMatch(strDatEinsteuerung)) Then
                lblError.Text = "Datum Einsteuerung ungültig."
                Return False
            End If

            'Anzahl Fahrzeuge
            Dim strAnzahl As String = txtAnzahlFahrzeuge.Text.Trim

            If strAnzahl = String.Empty Then
                lblError.Text = "Bitte die Anzahl Fahrzeuge eingeben."
                Return False
            End If

            regEx = New Text.RegularExpressions.Regex(patternNumeric)
            If regEx.IsMatch(strAnzahl) Then
                lblError.Text = "Anzahl Fahrzeuge ungültig."
                Return False
            End If

            'Laufzeit
            Dim strLaufzeit As String = txtLaufzeit.Text.Trim

            If strLaufzeit = String.Empty Then
                lblError.Text = "Bitte Laufzeit eingeben."
                Return False
            End If

            regEx = New Text.RegularExpressions.Regex(patternNumeric)

            If regEx.IsMatch(strLaufzeit) Then
                lblError.Text = "Laufzeit ungültig."
                Return False
            End If

            'Unit-Nummer (entweder von,bis oder per Upload-File)
            Dim strUnitVon As String = txtUnitNrVon.Text.Trim
            Dim strUnitBis As String = txtUnitNrBis.Text.Trim

            If strUnitVon = String.Empty And strUnitBis = String.Empty Then
                uploadUnitnummern = True
                If (Not upFileUnitNr.PostedFile Is Nothing) AndAlso (Not (upFileUnitNr.PostedFile.FileName = String.Empty)) Then
                    If Right(upFileUnitNr.PostedFile.FileName.ToUpper, 4) <> ".XLS" AndAlso Right(upFileUnitNr.PostedFile.FileName.ToUpper, 5) <> ".XLSX" Then
                        lblError.Text = "Es können nur Dateien im .XLS oder .XLSX-Format verarbeitet werden."
                        Return False
                    End If
                Else
                    lblError.Text = "Bitte entweder Unit-Nr. (von - bis) oder eine Upload-Datei mit Unit-Nummern angeben"
                    Return False
                End If
            Else
                uploadUnitnummern = False
                If strUnitVon = String.Empty Or strUnitBis = String.Empty Then
                    lblError.Text = "Bitte Unit-Nr. (von - bis) eingeben."
                    Return False
                End If

                regEx = New Text.RegularExpressions.Regex(patternNumeric)

                If regEx.IsMatch(strUnitVon) Then
                    lblError.Text = "Unit-Nr. (von) ungültig."
                    Return False
                End If

                If regEx.IsMatch(strUnitBis) Then
                    lblError.Text = "Unit-Nr. (bis) ungültig."
                    Return False
                End If

                Dim lngUnitVon As Long = CType(strUnitVon, Long)
                Dim lngUnitBis As Long = CType(strUnitBis, Long)

                If Not ((lngUnitBis - lngUnitVon) = (CType(strAnzahl, Long) - 1)) Then
                    lblError.Text = "Unit-Nr. (von-bis) ungültig."
                    Return False
                End If
            End If

            Return True

        Catch ex As Exception
            lblError.Text = "Fehler bei der Prüfung der eingegebenen Daten: " & ex.Message
            Return False
        End Try

    End Function

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

    Private Sub collectEditData(uploadUnitnummern As Boolean)

        With objBatch
            .ModelID = txtModelId.Text
            .ModellBezeichnung = txtModell.Text
            .SippCode = txtSippcode.Text
            .Hersteller = txtHerstellerHidden.Value
            .HerstellerBezeichnung = txtHerstellerBezeichnungHidden.Value
            .BatchId = txtBatchId.Text
            .DatumEinsteuerung = txtDatEinsteuerung.Text
            .AnzahlFahrzeuge = txtAnzahlFahrzeuge.Text
            .Laufzeit = txtLaufzeit.Text
            .KennzeichenSerie = ddlKennzeichenserie1.SelectedItem.Text
            .LaufzeitBindung = cbxLaufz.Checked
            .Bemerkungen = txtBemerkung.Text
            .AuftragsNrVon = txtAuftragsnummerVon.Text
            .AuftragsNrBis = txtAuftragsnummerBis.Text
            .Verwendungszweck = ddlVerwendung.SelectedItem.Value
            .VerwendungszweckBezeichnung = ddlVerwendung.SelectedItem.Text
            .Fahrzeuggruppe = rbPKW.Checked
            .WinterBereifung = rbJ1.Checked
            .Anhaengerkupplung = rbJAnhaenger.Checked
            .SecurFleet = rbJ2.Checked
            .Leasing = rbLeasingJa.Checked
            .Navi = rb_NaviJa.Checked

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

        Session("objBatch") = objBatch
    End Sub

    Private Sub Speichern(uploadUnitnummern As Boolean)

        collectEditData(uploadUnitnummern)

        objBatch.saveData(Session("AppID").ToString, Session.SessionID)

        Session("objBatch") = objBatch

        If objBatch.Status = 0 Then
            lblSuccess.Text = "Daten gesichert."

            If txtBatchId.Enabled Then
                'nach Neuanlage
                clearControls(Not cbxKeepData.Checked)
            Else
                'nach Bearbeitung
                trEditBatch.Visible = False
                Suchen(True)
            End If
        Else
            lblError.Text = "Die Daten konnten nicht gespeichert werden. Bitte überprüfen Sie Ihre Eingaben. (" & objBatch.Message & ")"
        End If

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

#End Region

End Class
