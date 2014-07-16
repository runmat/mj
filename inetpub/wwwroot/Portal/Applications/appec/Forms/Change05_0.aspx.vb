'Imports CKG.Base.Business
'Imports CKG.Base.Kernel
Imports CKG.Base.Business
Imports CKG.Portal.PageElements
'Imports eWorld.UI
Imports CKG.Base.Kernel.Common.Common


Public Class Change05_0
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

    Private m_User As Base.Kernel.Security.User
    Private m_App As Base.Kernel.Security.App

    Protected WithEvents lnkKreditlimit As System.Web.UI.WebControls.HyperLink
    Protected WithEvents cmdSave As System.Web.UI.WebControls.LinkButton
    Protected WithEvents lblPageTitle As System.Web.UI.WebControls.Label
    Protected WithEvents ucHeader As Header
    Protected WithEvents ucStyles As Styles
    Protected WithEvents lblHead As System.Web.UI.WebControls.Label
    Protected WithEvents calZulassung As eWorld.UI.CalendarPopup
    Protected WithEvents Table5 As System.Web.UI.HtmlControls.HtmlTable
    Protected WithEvents Table2 As System.Web.UI.HtmlControls.HtmlTable
    Protected WithEvents trSumme As System.Web.UI.HtmlControls.HtmlTableRow
    Private objSuche As change_01
    'Private rowID_PDI As String
    'Private rowID_MOD As String
    'Private infoArray As Array
    'Private highlightID As String

    Protected WithEvents lstPDI As System.Web.UI.WebControls.ListBox
    Protected WithEvents lstMOD As System.Web.UI.WebControls.ListBox
    Protected WithEvents lblFahrzeuge As System.Web.UI.WebControls.Label
    Protected WithEvents lblPDIs As System.Web.UI.WebControls.Label
    Protected WithEvents lblModelle As System.Web.UI.WebControls.Label
    Protected WithEvents lblError As System.Web.UI.WebControls.Label
    Protected WithEvents lblTask As System.Web.UI.WebControls.Label
    Protected WithEvents btnConfirm As System.Web.UI.WebControls.LinkButton
    Protected WithEvents lblBezeichnung As System.Web.UI.WebControls.Label
    Protected WithEvents DataGrid1 As System.Web.UI.WebControls.DataGrid
    Protected WithEvents lZulassungPDIAnzahl As System.Web.UI.WebControls.Label
    Protected WithEvents lZulassungGesamtAnzahl As System.Web.UI.WebControls.Label
    Protected WithEvents dgtest As System.Web.UI.WebControls.DataGrid

    Private Const StrTaskZulassen As String = "Zulassen"
    Private Const StrTaskSperren As String = "Sperren"
    Private Const StrTaskEntsperren As String = "Entsperren"
    Private Const StrTaskVerschieben As String = "Verschieben"


    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        m_User = GetUser(Me)
        ucHeader.InitUser(m_User)
        FormAuth(Me, m_User)

        GetAppIDFromQueryString(Me)

        lblError.Text = ""

        Try
            lblHead.Text = m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString
            ucStyles.TitleText = lblHead.Text
            m_App = New Base.Kernel.Security.App(m_User)

            objSuche = CType(Session("objSuche"), change_01)

            If Not IsPostBack Then
                Initialload()
            Else
                PostbackLoad()
            End If
        Catch ex As Exception
            lblError.Text = "Beim Laden der Seite ist ein Fehler aufgetreten."
            btnConfirm.Enabled = False
        End Try


    End Sub

    Private Sub Initialload()
        Dim strStatusPdi As String = ""
        Dim strStatusMod As String = ""

        'löschen der Sessionwerte des Items aus vorhergegangen Seitenaufrufen
        Session.Remove("tblZulassungsAnzahl")

        lblTask.Text = objSuche.Task.ToUpper

        ' Redunant da bereits im Change05 gefüllt
        'objSuche.getCars(Session("AppID").ToString, Session.SessionID)

        If (objSuche.Status = 0) AndAlso (objSuche.Result.Rows.Count > 0) Then
            objSuche.getPDIs(strStatusPdi)
            objSuche.getMODs(strStatusMod)
            Session("objSuche") = objSuche

            FillPdi()
            FillMod(lstPDI.SelectedItem.Value)
            FillCar(lstPDI.SelectedItem.Value, lstMOD.SelectedItem.Value)
            SetBezeichnung()
        Else
            lblError.Text = "Keine Fahrzeuge vorhanden."
            lstMOD.Visible = False
            lstPDI.Visible = False
            DataGrid1.Visible = False
            btnConfirm.Enabled = False
        End If
    End Sub

    Private Sub PostbackLoad()
        FillTable()
    End Sub

    Private Sub FillPdi(Optional ByVal strSetIndex As String = "")
        Dim vwPdi As DataView
        objSuche = CType(Session("objSuche"), change_01)

        vwPdi = objSuche.AllPDIs.DefaultView

        If vwPdi.Count > 0 Then
            vwPdi.Sort = "KUNPDI ASC"
            With lstPDI
                .DataSource = vwPdi
                .DataTextField = "PDIAnzahl"
                .DataValueField = "KUNPDI"
                .DataBind()
            End With
            If strSetIndex = String.Empty Then
                lstPDI.Items(0).Selected = True
            Else
                lstPDI.Items.FindByValue(strSetIndex).Selected = True
            End If
            lblPDIs.Text = vwPdi.Count
        End If
    End Sub

    Private Sub FillMod(ByVal strPdi As String, Optional ByVal strSetIndex As String = "")
        Dim vwMod As DataView
        Dim item As ListItem
        objSuche = CType(Session("objSuche"), change_01)

        vwMod = objSuche.AllMODs.DefaultView
        vwMod.RowFilter = "KUNPDI='" & strPdi & "'"

        If vwMod.Count > 0 Then
            vwMod.Sort = "ZZMODELL ASC"
            With lstMOD
                .DataSource = vwMod
                .DataTextField = "MODAnzahl"
                .DataValueField = "ZZMODELL"
                .DataBind()
            End With
            If strSetIndex = String.Empty Then
                lstMOD.Items(0).Selected = True
            Else
                item = lstMOD.Items.FindByValue(strSetIndex)
                If Not item Is Nothing Then
                    item.Selected = True
                Else
                    lstMOD.Items(0).Selected = True
                End If
            End If
            lblModelle.Text = vwMod.Count
        End If
    End Sub

    Private Sub FillVerschieb(ByVal tmpDataView As DataView)
        Dim item As DataGridItem
        Dim itemList As ListItem
        Dim ddlZielPdi As DropDownList
        Dim txtMenge As TextBox
        Dim cbxAuswahl As CheckBox
        Dim row As DataRow
        Dim selectedRow As DataRow
        Dim valueSelected As String
        Dim textSelected As String
        Dim btnKopieren As Button

        Dim vwPdIs As DataView
        Dim intLoop As Integer


        If tmpDataView.Count = 0 Then
            DataGrid1.Visible = False
        Else
            tmpDataView.Sort = "Eingangsdatum ASC, RowID ASC"              'Sortierung nach Eingangsdatum (aufsteigend)
            DataGrid1.DataSource = tmpDataView
            DataGrid1.DataBind()

            lblFahrzeuge.Text = tmpDataView.Count

            Dim pdiListe As New PDIListe(m_User, m_App)
            pdiListe.getPDIs(Session("AppID").ToString, Session.SessionID)

            'Controls befüllen
            For Each item In DataGrid1.Items

                ddlZielPdi = CType(item.FindControl("ddlZielPDI"), DropDownList)
                row = objSuche.Result.Select("RowID='" & item.Cells(0).Text & "'")(0)       'Tabellenzeile holen

                'Ziel-PDI (Für Verschieben)

                If (CStr(row("SelectedZielPDI")) <> String.Empty) Then
                    selectedRow = pdiListe.PPDIs.Select("KUNPDI='" & CStr(row("SelectedZielPDI")) & "'")(0)
                    valueSelected = CStr(selectedRow("KUNPDI"))
                    textSelected = CStr(selectedRow("KUNPDI"))
                    'Ersten Eintrag setzen (ausgewählt), da SelectedIndex nicht funktioniert!!!?
                    itemList = New ListItem()
                    itemList.Value = valueSelected
                    itemList.Text = textSelected
                    ddlZielPdi.Items.Add(itemList)
                Else
                    valueSelected = String.Empty
                End If

                vwPdIs = pdiListe.PPDIs.DefaultView
                vwPdIs.Sort = "KUNPDI ASC"

                For intLoop = 0 To vwPdIs.Count - 1
                    If ((CStr(vwPdIs.Item(intLoop)("KUNPDI")) <> valueSelected) AndAlso (CStr(vwPdIs.Item(intLoop)("KUNPDI")) <> row("KUNPDI"))) Then
                        itemList = New ListItem()
                        itemList.Value = CStr(vwPdIs.Item(intLoop)("KUNPDI"))
                        itemList.Text = CStr(vwPdIs.Item(intLoop)("KUNPDI"))
                        ddlZielPdi.Items.Add(itemList)
                    End If
                Next

                If (ddlZielPdi.Items.Count = 0) Then
                    ddlZielPdi.Visible = False
                End If

                'Auswahlbox
                cbxAuswahl = CType(item.FindControl("cbxAuswahl"), CheckBox)
                If CStr(row("SelectedEinzel")) <> String.Empty Then
                    cbxAuswahl.Checked = CBool(row("SelectedEinzel"))
                End If

                'Kopieren (Dropdownlist)
                txtMenge = CType(item.FindControl("txtMenge"), TextBox)
                btnKopieren = CType(item.FindControl("btnKopieren"), Button)

                If item.ItemIndex = DataGrid1.Items.Count - 1 Then
                    txtMenge.Visible = False
                    btnKopieren.Visible = False
                End If
            Next
        End If
    End Sub

    Private Sub FillSperr(ByVal tmpDataView As DataView)
        Dim item As DataGridItem
        Dim txtBox As TextBox
        Dim btnKopieren As Button
        Dim txtMenge As TextBox
        Dim cbxAuswahl As CheckBox
        Dim row As DataRow

        If tmpDataView.Count = 0 Then
            DataGrid1.Visible = False
        Else
            DataGrid1.Visible = True
            tmpDataView.Sort = "Eingangsdatum ASC, RowID ASC"              'Sortierung nach Eingangsdatum (aufsteigend)

            DataGrid1.DataSource = tmpDataView
            DataGrid1.DataBind()

            lblFahrzeuge.Text = tmpDataView.Count
            'Controls befüllen
            For Each item In DataGrid1.Items
                row = objSuche.Result.Select("RowID='" & item.Cells(0).Text & "'")(0)       'Tabellenzeile holen

                'Textfeld (Bemerkung)
                txtBox = CType(item.FindControl("txtBemerkung"), TextBox)
                txtBox.Text = CStr(row("Bemerkung"))

                'Auswahlbox
                cbxAuswahl = CType(item.FindControl("cbxAuswahl"), CheckBox)
                If CStr(row("SelectedEinzel")) <> String.Empty Then
                    cbxAuswahl.Checked = CBool(row("SelectedEinzel"))
                End If

                'Kopieren (Dropdownlist)
                txtMenge = CType(item.FindControl("txtMenge"), TextBox)
                btnKopieren = CType(item.FindControl("btnKopieren"), Button)

                If item.ItemIndex = DataGrid1.Items.Count - 1 Then
                    txtMenge.Visible = False
                    btnKopieren.Visible = False
                End If
            Next
        End If
    End Sub

    Private Sub Fill(ByVal tmpDataView As DataView)
        Dim item As DataGridItem
        Dim itemList As ListItem
        Dim txtBoxBemerkung As TextBox
        Dim ddlKennzeichenserie As DropDownList
        Dim txtMenge As TextBox
        Dim cbxAuswahl As CheckBox
        Dim row As DataRow
        Dim selectedRow As DataRow
        Dim rowLoop As DataRow
        Dim valueSelected As String
        Dim textSelected As String
        Dim btnKopieren As Button
        Dim calControl As TextBox = Nothing
        Dim strPdi As String = lstPDI.SelectedItem.Value.ToString


        If tmpDataView.Count = 0 Then
            DataGrid1.Visible = False
        Else
            DataGrid1.Visible = True
            tmpDataView.Sort = "Eingangsdatum ASC, RowID ASC"              'Sortierung nach Eingangsdatum (aufsteigend)
            DataGrid1.DataSource = tmpDataView
            DataGrid1.DataBind()

            lblFahrzeuge.Text = tmpDataView.Count
            'Controls befüllen
            For Each item In DataGrid1.Items

                ddlKennzeichenserie = CType(item.FindControl("ddlKennzeichenserie"), DropDownList)
                row = objSuche.Result.Select("RowID='" & item.Cells(0).Text & "'")(0)       'Tabellenzeile holen

                'Kennzeichenserie
                If (CStr(row("SelectedKennzeichenserie")) <> String.Empty) Then
                    selectedRow = objSuche.PKennzeichenSerie.Select("ID='" & CStr(row("SelectedKennzeichenserie")) & "'")(0)
                    valueSelected = CStr(selectedRow("ID"))
                    textSelected = CStr(selectedRow("Serie"))
                    'Ersten Eintrag setzen (ausgewählt), da SelectedIndex nicht funktioniert!!!?
                    itemList = New ListItem()
                    itemList.Value = valueSelected
                    itemList.Text = textSelected


                    ddlKennzeichenserie.Items.Add(itemList)
                Else
                    valueSelected = String.Empty
                End If

                For Each rowLoop In objSuche.PKennzeichenSerie.Rows
                    If (CStr(rowLoop("ID")) <> valueSelected) Then
                        itemList = New ListItem()
                        itemList.Value = CStr(rowLoop("ID"))
                        itemList.Text = CStr(rowLoop("Serie"))

                        If Right(strPdi, 1).ToString = "N" Then
                            If Right(itemList.Text, 3) = "(N)" Then
                                ddlKennzeichenserie.Items.Add(itemList)
                            End If
                        Else
                            If Right(itemList.Text, 3) <> "(N)" Then
                                ddlKennzeichenserie.Items.Add(itemList)
                            End If
                        End If

                    End If
                Next

                If (objSuche.Task = StrTaskZulassen) Then
                    calControl = CType(item.FindControl("calControl"), TextBox)
                    calControl.Text = row("SelectedDate")
                End If
                'Bemerkungsfeld
                txtBoxBemerkung = CType(item.FindControl("txtBemerkung"), TextBox)
                txtBoxBemerkung.Text = CStr(row("Bemerkung"))
                txtBoxBemerkung.Enabled = False
                txtBoxBemerkung.CssClass = "InputDisableStyle"

                'Auswahlbox
                cbxAuswahl = CType(item.FindControl("cbxAuswahl"), CheckBox)
                If CStr(row("SelectedEinzel")) <> String.Empty Then
                    cbxAuswahl.Checked = CBool(row("SelectedEinzel"))
                End If

                'Kopieren (Dropdownlist)
                txtMenge = CType(item.FindControl("txtMenge"), TextBox)
                btnKopieren = CType(item.FindControl("btnKopieren"), Button)

                If item.ItemIndex = DataGrid1.Items.Count - 1 Then
                    txtMenge.Visible = False
                    btnKopieren.Visible = False

                End If

            Next
        End If

        If DataGrid1.Items.Count = 1 Then
            If Not calZulassung.SelectedDate = "01.01.0001" Then
                calControl.Text = calZulassung.SelectedDate.ToShortDateString
            End If
        End If
    End Sub

    Private Sub FillCarErrors()

        objSuche = CType(Session("objSuche"), change_01)

        Dim tmpDataView As DataView = objSuche.Result.DefaultView
        tmpDataView.RowFilter = "Status<>'" & String.Empty & "'"

        Fill(tmpDataView)
    End Sub

    Private Sub FillCar(ByVal strPdi As String, ByVal strMod As String)
        Dim tmpDataView As DataView
        objSuche = CType(Session("objSuche"), change_01)

        tmpDataView = objSuche.Result.DefaultView
        tmpDataView.RowFilter = "KUNPDI='" & strPdi & "' AND ZZMODELL='" & strMod & "'"
        Select Case objSuche.Task
            Case StrTaskZulassen
                Fill(tmpDataView)
            Case StrTaskSperren
                FillSperr(tmpDataView)
            Case StrTaskEntsperren
                FillSperr(tmpDataView)
            Case StrTaskVerschieben
                FillVerschieb(tmpDataView)
        End Select

    End Sub

    Private Sub FillTable()

        Dim ddlKennzeichenserie As DropDownList
        Dim ddlZielPdi As DropDownList
        Dim txtMenge As TextBox
        Dim cbxAuswahl As CheckBox
        Dim txtZulassungsdatum As TextBox
        Dim txtBemerkung As TextBox
        Dim item As DataGridItem = Nothing
        Dim row As DataRow
        Dim rowPdIs As DataRow()
        Dim rowMoDs As DataRow()
        'Dim anzahl As Integer = 0
        Dim intCounter As Integer
        Dim strSonder As String

        objSuche = CType(Session("objSuche"), change_01)

        For Each item In DataGrid1.Items
            row = objSuche.Result.Select("RowID='" & item.Cells(0).Text & "'")(0)       'Tabellenzeile holen

            'DropDownListen holen
            txtMenge = CType(item.FindControl("txtMenge"), TextBox)
            txtBemerkung = CType(item.FindControl("txtBemerkung"), TextBox)
            ddlKennzeichenserie = CType(item.FindControl("ddlKennzeichenserie"), DropDownList)
            ddlZielPdi = CType(item.FindControl("ddlZielPDI"), DropDownList)

            'Kopieren setzen
            'Prüfen, ob das Feld eine positive Zahl enthält...Wenn nicht, leeren.
            If (Not IsNumeric(txtMenge.Text)) OrElse (CInt(txtMenge.Text) < 0) Then
                txtMenge.Text = String.Empty
            Else
                'Prüfen, ob eingetragener Wert nicht zu groß ist...
                If CInt(txtMenge.Text) > DataGrid1.Items.Count - item.ItemIndex Then
                    txtMenge.Text = DataGrid1.Items.Count - item.ItemIndex
                End If
            End If

            If (txtMenge.Text <> String.Empty) Then
                For intCounter = 0 To CInt(txtMenge.Text) - 1
                    row = objSuche.Result.Select("RowID='" & DataGrid1.Items(item.ItemIndex + intCounter).Cells(0).Text & "'")(0)

                    row("SelectedEinzel") = True
                    row("SelectedAlle") = txtMenge.Text

                    'Kennzeichenserie/Sonderserie setzen (bei Zulassung)
                    If objSuche.Task = StrTaskZulassen Then
                        row("SelectedKennzeichenserie") = ddlKennzeichenserie.SelectedItem.Value
                        row("SelectedKennzeichenserieText") = ddlKennzeichenserie.SelectedItem.Text
                        strSonder = ddlKennzeichenserie.SelectedItem.Text
                        If strSonder.IndexOf("(", StringComparison.Ordinal) >= 0 Then
                            'Sonderserie ausgewählt...
                            strSonder = Right(strSonder, strSonder.Length - strSonder.IndexOf("(", StringComparison.Ordinal))
                            strSonder = strSonder.Substring(1, 1)
                            row("SelectedSonderserie") = strSonder
                        Else
                            row("SelectedSonderserie") = String.Empty
                        End If
                    End If

                    'Auswahl setzen
                    cbxAuswahl = CType(item.FindControl("cbxAuswahl"), CheckBox)
                    row("SelectedEinzel") = cbxAuswahl.Checked

                    'Datum setzen
                    If objSuche.Task = StrTaskZulassen Then
                        row("SelectedDate") = CStr(calZulassung.SelectedDate)
                    End If

                    'Bemerkung setzen
                    If objSuche.Task = StrTaskSperren Or objSuche.Task = StrTaskEntsperren Then
                        row("Bemerkung") = txtBemerkung.Text
                    End If

                    'Ziel-PDI setzen (bei Verschieben)
                    If Not (ddlZielPdi.SelectedItem Is Nothing) AndAlso (objSuche.Task = StrTaskVerschieben) Then
                        row("SelectedZielPDI") = ddlZielPdi.SelectedItem.Value
                        row("SelectedZielPDIText") = ddlZielPdi.SelectedItem.Text
                    End If
                Next
            End If

            If CInt(row("SelectedAlle")) = 0 Then
                'Kennzeichenserie/Sonderserie setzen (bei Zulassung)
                If objSuche.Task = StrTaskZulassen Then
                    row("SelectedKennzeichenserie") = ddlKennzeichenserie.SelectedItem.Value
                    row("SelectedKennzeichenserieText") = ddlKennzeichenserie.SelectedItem.Text
                    strSonder = ddlKennzeichenserie.SelectedItem.Text
                    If strSonder.IndexOf("(", StringComparison.Ordinal) >= 0 Then
                        'Sonderserie ausgewählt...
                        strSonder = Right(strSonder, strSonder.Length - strSonder.IndexOf("(", StringComparison.Ordinal))
                        strSonder = strSonder.Substring(1, 1)
                        row("SelectedSonderserie") = strSonder
                    Else
                        row("SelectedSonderserie") = String.Empty
                    End If
                End If

                'Auswahl setzen
                cbxAuswahl = CType(item.FindControl("cbxAuswahl"), CheckBox)
                row("SelectedEinzel") = cbxAuswahl.Checked

                'Datum setzen
                txtZulassungsdatum = CType(item.FindControl("calControl"), TextBox)
                row("SelectedDate") = CStr(txtZulassungsdatum.Text)

                'Bemerkung setzen
                txtBemerkung = CType(item.FindControl("txtBemerkung"), TextBox)
                row("Bemerkung") = txtBemerkung.Text

                'Ziel-PDI setzen (bei Verschieben)
                If Not (ddlZielPdi.SelectedItem Is Nothing) AndAlso (objSuche.Task = StrTaskVerschieben) Then
                    row("SelectedZielPDI") = ddlZielPdi.SelectedItem.Value
                    row("SelectedZielPDIText") = ddlZielPdi.SelectedItem.Text
                End If
            End If
        Next

        'Bereits gesetzte Werte wieder löschen...
        For Each row In objSuche.Result.Rows
            row("SelectedAlle") = 0
        Next

        'Listboxen aktualisieren (Anzahl ausgewählter Fahrzeuge) : PDIs
        rowPdIs = objSuche.Result.Select("SelectedEinzel=True AND KUNPDI = '" & item.Cells(2).Text & "'")         'PDIs
        If rowPdIs.Length > 0 Then
            row = objSuche.AllPDIs.Select("KUNPDI='" & item.Cells(2).Text & "'")(0)
            row("PDIAnzahl") = CStr(row("KUNPDI")) & " (" & rowPdIs.Length & ")"
        End If

        'Listboxen aktualisieren (Anzahl ausgewählter Fahrzeuge) : MODs
        rowMoDs = objSuche.Result.Select("SelectedEinzel=True AND ZZBEZEI='" & item.Cells(3).Text & "' AND KUNPDI = '" & item.Cells(2).Text & "'")          'PDIs
        If rowMoDs.Length > 0 Then
            row = objSuche.AllMODs.Select("KUNPDI = '" & item.Cells(2).Text & "' AND MODName='" & item.Cells(3).Text & "'")(0)
            row("MODAnzahl") = CStr(row("ZZMODELL")) & " (" & rowMoDs.Length & ")"
        End If

        'Listbox updaten...
        FillPdi(lstPDI.SelectedItem.Value)
        FillMod(lstPDI.SelectedItem.Value, lstMOD.SelectedItem.Value)
        FillCar(lstPDI.SelectedItem.Value, lstMOD.SelectedItem.Value)

        SetBezeichnung()

        objSuche.Result.AcceptChanges()
        Session("objSuche") = objSuche
    End Sub

    Private Function CheckInput(ByRef intAnzahl As Integer) As Boolean
        Dim strDate As String
        Dim blnCheck As Boolean
        Dim intErrors As Integer
        Dim strStatus As String
        Dim row As DataRow

        intAnzahl = 0
        objSuche = CType(Session("objSuche"), change_01)
        intErrors = 0

        For Each row In objSuche.Result.Rows
            strStatus = String.Empty
            blnCheck = True

            If CBool(row("SelectedEinzel")) = True Then 'Zeile ausgewählt?
                intAnzahl += 1

                '############ Zulassen ##################################################################
                If (objSuche.Task = StrTaskZulassen) Then
                    '*** Zulassungsdatum überprüfen --------------------------
                    strDate = CStr(row("SelectedDate"))

                    Try
                        '...Überhaupt Datum?
                        Dim datCheck As Date = CDate(strDate)
                    Catch ex As Exception
                        blnCheck = False
                        strStatus = "Ungültiges Datum."
                    End Try

                    If blnCheck Then
                        '...Datum < Tagesdatum?
                        If CDate(strDate) < Date.Today Then
                            blnCheck = False
                            strStatus = "Zulassungsdatum darf nicht in der Vergangenheit liegen."
                        End If
                    End If

                    If blnCheck Then
                        '...Datum > Tagesdatum + 14 Tage?
                        If CDate(strDate) > Date.Today.AddDays(14) Then
                            blnCheck = False
                            strStatus = "Zulassungsdatum darf nicht mehr als 14 Tage in der Zukunft liegen."
                        End If
                    End If

                    If blnCheck Then
                        '...Wochenende ?
                        If (CDate(strDate).DayOfWeek = DayOfWeek.Saturday) Or (CDate(strDate).DayOfWeek = DayOfWeek.Sunday) Then
                            blnCheck = False
                            strStatus = "Zulassungsdatum ungültig (Wochenende)."
                        End If
                    End If
                End If

                '############ Sperren / Entsperren #############################################################
                If (objSuche.Task = StrTaskSperren) Then

                End If
                If (objSuche.Task = StrTaskEntsperren) Then

                End If
                '############ Verschieben ######################################################################

                If (objSuche.Task = StrTaskVerschieben) Then

                End If

                If Not blnCheck Then
                    intErrors += 1
                End If
                row("Status") = strStatus
            End If

            objSuche.Result.AcceptChanges()
        Next
        If (intAnzahl = 0) Then     'Keine Fahrzeuge ausgewäht -> Fehler...
            intErrors = 1
            lblError.Text = String.Format("Keine Fahrzeuge zum {0} ausgewählt.", objSuche.Task.ToUpper)
        End If
        Return (intErrors = 0)
    End Function

    Private Sub btnConfirm_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnConfirm.Click

        Dim intAnzahl As Integer

        If Not CheckInput(intAnzahl) Then
            If intAnzahl > 0 Then
                FillCarErrors()
            End If
            Exit Sub
        End If
        Response.Redirect("Change05_2.aspx?AppID=" & Session("AppID").ToString)
    End Sub

    Private Sub SetBezeichnung()
        Dim rowSelectedPdi As DataRow
        Dim rowSelectedMod As DataRow()
        Dim strSelectedPdi As String
        Dim strSelectedMod As String
        Dim strSippCode As String


        objSuche = CType(Session("objSuche"), change_01)

        Try
            'PDI-Bezeichnung
            rowSelectedPdi = objSuche.AllPDIs.Select("KUNPDI='" & lstPDI.SelectedItem.Value & "'")(0)
            strSelectedPdi = CStr(rowSelectedPdi("PDIName"))
            'Modellbezeichnung
            rowSelectedMod = objSuche.AllMODs.Select("KUNPDI='" & lstPDI.SelectedItem.Value & "' AND ZZMODELL='" & lstMOD.SelectedItem.Value & "'")

            If rowSelectedMod.Length = 1 Then
                strSelectedMod = CStr(rowSelectedMod(0)("MODName"))
            Else
                strSelectedMod = String.Empty
            End If
            strSippCode = objSuche.Result.Select("KUNPDI='" & lstPDI.SelectedItem.Value & "' AND ZZMODELL='" & lstMOD.SelectedItem.Value & "'")(0)("SIPPCODE")

            lblBezeichnung.Text = String.Format("{0}&nbsp;&#124;&nbsp{1}&nbsp;&#124;&nbsp{2}", strSelectedPdi, strSelectedMod, strSippCode)
        Catch ex As Exception
            lblError.Text = "Fehler bei der Ermittlung der PDI-Bezeichnung."
        End Try

    End Sub

    Private Sub lstPDI_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lstPDI.SelectedIndexChanged

        DisplayPDIZulassungen()

    End Sub

    Private Sub lstMOD_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lstMOD.SelectedIndexChanged

    End Sub

    Private Sub DataGrid1_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles DataGrid1.ItemDataBound
        If objSuche.Task = StrTaskZulassen Then
            e.Item.Cells(15).Visible = False    'Ziel-PDI
            e.Item.Cells(16).Visible = False    'Gesperrt
        End If
        If objSuche.Task = StrTaskSperren Then
            'Zulassungsdatum ausblenden
            Table2.Visible = False
            Table5.Visible = False

            e.Item.Cells(12).Visible = False    'Zul.Datum
            e.Item.Cells(13).Visible = False    'Kennzeichenserie
            e.Item.Cells(15).Visible = False    'Ziel-PDI
            e.Item.Cells(16).Visible = False     'Gesperrt
        End If
        If objSuche.Task = StrTaskEntsperren Then
            'Zulassungsdatum ausblenden
            Table2.Visible = False
            Table5.Visible = False

            e.Item.Cells(12).Visible = False    'Zul.Datum
            e.Item.Cells(13).Visible = False    'Kennzeichenserie
            e.Item.Cells(15).Visible = False    'Ziel-PDI
            e.Item.Cells(16).Visible = True     'Gesperrt
        End If
        If objSuche.Task = StrTaskVerschieben Then
            'Zulassungsdatum ausblenden
            Table2.Visible = False
            Table5.Visible = False

            e.Item.Cells(12).Visible = False    'Zul.Datum
            e.Item.Cells(13).Visible = False    'Kennzeichenserie
            e.Item.Cells(14).Visible = False    'Bemerkung
            e.Item.Cells(15).Visible = True     'Ziel-PDI
            e.Item.Cells(16).Visible = False    'Gesperrt
        End If
    End Sub

    Private Sub DataGrid1_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles DataGrid1.ItemCommand
        If e.CommandName = "Kopieren" Then
            FillTable()
        End If
    End Sub

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        SetEndASPXAccess(Me)
    End Sub

    Private Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Unload
        SetEndASPXAccess(Me)
    End Sub

    Private Sub calZulassung_DateChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles calZulassung.DateChanged
        objSuche = CType(Session("objSuche"), change_01)
        Session.Add("tblZulassungsAnzahl", objSuche.createZulassungCountTable(calZulassung.SelectedDate))
        DisplayGesamtZulassungen()
        DisplayPdiZulassungen()
    End Sub

    Private Sub DisplayGesamtZulassungen()
        Try

            Dim dtGesamtZulassungen = CType(Session.Item("tblZulassungsAnzahl"), DataTable)
            If Not dtGesamtZulassungen.Rows.Count = 0 Then
                Dim drResult() As DataRow = dtGesamtZulassungen.Select("ZZCARPORT='Gesamt'")
                lZulassungGesamtAnzahl.Text = drResult(0)("ZANZAHL")
            Else
                lZulassungGesamtAnzahl.Text = 0
            End If
        Catch
            lblError.Text = "Fehler bei beim auslesen der bisher zugelassenen Fahrzeuganzahl (Gesamt)"
        End Try
    End Sub

    Private Sub DisplayPdiZulassungen()

        If Not lstPDI.SelectedIndex = -1 Then
            Try
                Dim dtGesamtZulassungen = CType(Session.Item("tblZulassungsAnzahl"), DataTable)

                If Not dtGesamtZulassungen Is Nothing Then
                    If Not dtGesamtZulassungen.Rows.Count = 0 Then
                        Dim drResult() As DataRow = dtGesamtZulassungen.Select("ZZCARPORT='" & lstPDI.SelectedItem.Text & " '")
                        If Not drResult.Length = 0 Then
                            lZulassungPDIAnzahl.Text = drResult(0)("ZANZAHL")
                        Else
                            lZulassungPDIAnzahl.Text = 0
                        End If
                    Else
                        lZulassungPDIAnzahl.Text = 0
                    End If
                End If
            Catch
                lblError.Text = "Fehler bei beim auslesen der bisher zugelassenen Fahrzeuganzahl (pro PID)"
            End Try

        End If

    End Sub

End Class

' ************************************************
' $History: Change05_0.aspx.vb $
' 
' *****************  Version 4  *****************
' User: Jungj        Date: 29.06.09   Time: 9:23
' Updated in $/CKAG/Applications/appec/Forms
' ITA 2918 Z_M_Fehlend_Unfall_001, Z_M_Schlue_Set_Mahnsp_001
' 
' *****************  Version 3  *****************
' User: Jungj        Date: 25.06.09   Time: 14:20
' Updated in $/CKAG/Applications/appec/Forms
' ITA 2918 Z_M_WARENKORB_SPERRE, Z_MASSENZULASSUNG,
' Z_M_EC_AVM_KENNZ_SERIE, Z_M_EC_AVM_PDIWECHSEL,
' Z_M_EC_AVM_ZULASSUNGSSPERRE
' 
' *****************  Version 2  *****************
' User: Rudolpho     Date: 17.06.09   Time: 8:47
' Updated in $/CKAG/Applications/appec/Forms
' Warnungen entfernt!
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 4.04.08    Time: 15:49
' Created in $/CKAG/Applications/appec/Forms
' 
' *****************  Version 29  *****************
' User: Fassbenders  Date: 15.01.08   Time: 16:54
' Updated in $/CKG/Applications/AppEC/AppECWeb/Forms
' 
' *****************  Version 28  *****************
' User: Jungj        Date: 4.12.07    Time: 11:34
' Updated in $/CKG/Applications/AppEC/AppECWeb/Forms
' 
' *****************  Version 27  *****************
' User: Jungj        Date: 22.10.07   Time: 17:20
' Updated in $/CKG/Applications/AppEC/AppECWeb/Forms
' 
' *****************  Version 26  *****************
' User: Jungj        Date: 22.10.07   Time: 14:43
' Updated in $/CKG/Applications/AppEC/AppECWeb/Forms
' 
' *****************  Version 25  *****************
' User: Jungj        Date: 22.10.07   Time: 13:34
' Updated in $/CKG/Applications/AppEC/AppECWeb/Forms
' 
' *****************  Version 24  *****************
' User: Uha          Date: 2.07.07    Time: 12:29
' Updated in $/CKG/Applications/AppEC/AppECWeb/Forms
' Logging der Laufzeiten der ASPX-Seiten eingeführt
' 
' *****************  Version 23  *****************
' User: Uha          Date: 22.05.07   Time: 13:31
' Updated in $/CKG/Applications/AppEC/AppECWeb/Forms
' Nacharbeiten + Bereinigungen
' 
' *****************  Version 22  *****************
' User: Uha          Date: 7.03.07    Time: 11:02
' Updated in $/CKG/Applications/AppEC/AppECWeb/Forms
' 
' ************************************************
