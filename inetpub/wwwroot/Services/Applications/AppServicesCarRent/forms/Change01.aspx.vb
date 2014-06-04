Imports CKG.Base.Kernel
Imports CKG.Base.Kernel.Common.Common
Imports GeneralTools.Services

Partial Public Class Change01
    Inherits Page
    Private m_User As Security.User
    Private m_App As Security.App

    Private objSuche As CarRent01

    Protected WithEvents GridNavigation1 As CKG.Services.GridNavigation

    Private Enum Zulassungsart As Integer

        UngueltigeAuswahl = 0
        Zentral = 1
        Dezentral = 2
        Beides = 3
    End Enum

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        m_User = GetUser(Me)
        FormAuth(Me, m_User, True)
        GetAppIDFromQueryString(Me)

        lblHead.Text = m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString
        m_App = New Security.App(m_User)
        GridNavigation1.setGridElment(gvFahrzeuge)
        Try
            lblError.Text = ""
            btnConfirm.Enabled = True
            If Not Session("objSuche") Is Nothing Then
                objSuche = CType(Session("objSuche"), CarRent01)
            End If
            If Not IsPostBack Then
                InitialSelection()
                If (Not Request.QueryString("Back") Is Nothing) AndAlso (Request.QueryString("Back").Length > 0) Then
                    loadNew()
                End If
            End If

        Catch ex As Exception
            lblError.Text = "Beim Laden der Seite ist ein Fehler aufgetreten."
            btnConfirm.Enabled = False
        End Try
    End Sub

    Private Sub Initialload()
        Dim strStatusPDI As String = ""
        Dim strStatusMod As String = ""

        objSuche = New CarRent01(m_User, m_App, Session("AppID").ToString, Session.SessionID, "")


        objSuche.getCars(Session("AppID").ToString, Session.SessionID, Me)

        If (objSuche.Status = 0) AndAlso (objSuche.Result.Rows.Count > 0) Then
            objSuche.getPDIs(strStatusPDI)
            objSuche.getMODs(strStatusMod)
            Session("objSuche") = objSuche

            fillPDI()
            fillMOD(lstPDI.SelectedItem.Value)
            fillCAR(lstPDI.SelectedItem.Value, lstMOD.SelectedItem.Value)
            fillVersicherer()
            setBezeichnung()
            Session("objSuche") = objSuche
            tab1.Visible = True
            Result.Visible = True
        Else
            lblError.Text = "Keine Fahrzeuge vorhanden."
            lstMOD.Visible = False
            lstPDI.Visible = False
            Result.Visible = False

        End If
    End Sub

    Private Sub loadNew()
        Dim strStatusPDI As String = ""
        Dim strStatusMod As String = ""

        If (objSuche.Status = 0) AndAlso (objSuche.Result.Rows.Count > 0) Then

            If objSuche.Zentral = True Then
                ddlZulKreis.Visible = False
                trZulassungskreis.Visible = False
                rblAuswahl.SelectedValue = "1"
            Else
                fillKreise()
                trZulassungskreis.Visible = True
                ddlZulKreis.Visible = True
                ddlZulKreis.SelectedValue = objSuche.ZulKreisID
                rblAuswahl.SelectedValue = "2"
            End If

            objSuche.getPDIs(strStatusPDI)
            objSuche.getMODs(strStatusMod)
            Session("objSuche") = objSuche

            fillPDI(objSuche.PSelectedPDI)
            fillMOD(lstPDI.SelectedItem.Value, objSuche.PSelectedMOD)
            fillCAR(lstPDI.SelectedItem.Value, lstMOD.SelectedItem.Value)
            fillVersicherer()
            setBezeichnung()

            Session("objSuche") = objSuche
            tab1.Visible = True
            Result.Visible = True
        Else
            lblError.Text = "Keine Fahrzeuge vorhanden."
            lstMOD.Visible = False
            lstPDI.Visible = False
            Result.Visible = False

        End If
    End Sub

    Private Sub InitialloadDezentral()

        objSuche = New CarRent01(m_User, m_App, Session("AppID").ToString, Session.SessionID, "")


        objSuche.getKreise(Session("AppID").ToString, Session.SessionID, Me)

        If (objSuche.Status = 0) AndAlso (objSuche.Zulassungskreise.Rows.Count > 0) Then

            Session("objSuche") = objSuche
            fillKreise()
        Else
            lblError.Text = "Keine Fahrzeuge vorhanden."
            lstMOD.Visible = False
            lstPDI.Visible = False
            Result.Visible = False

        End If
    End Sub

    Private Sub FillDezentral()
        Dim strStatusPDI As String = ""
        Dim strStatusMod As String = ""

        If Session("objSuche") Is Nothing Then
            objSuche = New CarRent01(m_User, m_App, Session("AppID").ToString, Session.SessionID, "")
        End If



        objSuche.getCarsDezentral(Session("AppID").ToString, Session.SessionID, Me, ddlZulKreis.SelectedItem.Text)

        If (objSuche.Status = 0) AndAlso (objSuche.Result.Rows.Count > 0) Then


            objSuche.ZulKreisID = ddlZulKreis.SelectedItem.Value
            objSuche.getPDIs(strStatusPDI)
            objSuche.getMODs(strStatusMod)
            Session("objSuche") = objSuche

            fillPDI()
            fillMOD(lstPDI.SelectedItem.Value)
            fillCAR(lstPDI.SelectedItem.Value, lstMOD.SelectedItem.Value)
            fillVersicherer()
            setBezeichnung()
            Session("objSuche") = objSuche
            tab1.Visible = True
            Result.Visible = True
        Else
            lblError.Text = "Keine Fahrzeuge vorhanden."
            lstMOD.Visible = False
            lstPDI.Visible = False
            Result.Visible = False

        End If
    End Sub

    Private Sub InitialSelection()

        Dim GetData As New CarRent01(m_User, m_App, Session("AppID").ToString, Session.SessionID, "")

        GetData.getZulassungsarten(Session("AppID").ToString, Session.SessionID, Page)

        Select Case GetData.ZulArt
            Case Zulassungsart.UngueltigeAuswahl
                lblDezError.Text = "Es konnten keine zulassungsfähigen Fahrzeuge ermittelt werden. "

            Case Zulassungsart.Zentral
                tab1.Visible = True
                Initialload()
            Case Zulassungsart.Dezentral
                trZulassungskreis.Visible = True
                InitialloadDezentral()
            Case Zulassungsart.Beides
                trZulassungsart.Visible = True

        End Select

    End Sub

    Private Sub fillTable(Optional ByVal Index As Integer = 0)

        Dim txtMenge As TextBox
        Dim row As DataRow
        Dim anzahl As Integer
        Dim cbxAuswahl As CheckBox
        Dim txtGridDatum As TextBox

        objSuche = CType(Session("objSuche"), CarRent01)

        'Menge holen
        txtMenge = CType(gvFahrzeuge.Rows(Index).FindControl("txtMenge"), TextBox)

        'Kopieren setzen
        'Prüfen, ob das Feld eine positive Zahl enthält...Wenn nicht, leeren.
        If (Not IsNumeric(txtMenge.Text)) OrElse (CInt(txtMenge.Text) < 0) Then
            txtMenge.Text = String.Empty
        Else
            'Prüfen, ob eingetragener Wert nicht zu groß ist...
            If CInt(txtMenge.Text) > gvFahrzeuge.Rows.Count - Index Then
                txtMenge.Text = gvFahrzeuge.Rows.Count - Index
                lblError.Text = "Eingabe überschreitet die zulässige Anzahl für die aktuelle Seite."
            End If
        End If
        If txtMenge.Text <> String.Empty Then
            anzahl = CInt(txtMenge.Text)
        Else
            anzahl = 0
        End If

        Dim iCount As Integer
        If anzahl > 0 Then

            For iCount = 0 To anzahl - 1

                Dim lblTempRow As Label
                lblTempRow = gvFahrzeuge.Rows(iCount + Index).Cells(0).FindControl("lblRowId")
                row = objSuche.Result.Select("RowID='" & lblTempRow.Text & "'")(0)
                row("SelectedEinzel") = True
                If iCount + Index = Index Then
                    row("SelectedAlle") = CStr(anzahl)
                End If
                'Datum setzen
                row("SelectedDate") = CStr(txtZulDate.Text)
                row("Status") = ""
            Next
        Else
            For iCount = 0 To gvFahrzeuge.Rows.Count - 1
                Dim lblTempRow As Label
                lblTempRow = gvFahrzeuge.Rows(iCount).Cells(0).FindControl("lblRowId")
                row = objSuche.Result.Select("RowID='" & lblTempRow.Text & "'")(0)

                cbxAuswahl = CType(gvFahrzeuge.Rows(iCount).Cells(0).FindControl("cbxAuswahl"), CheckBox)
                txtGridDatum = CType(gvFahrzeuge.Rows(iCount).Cells(0).FindControl("calControl"), TextBox)
                'Datum setzen
                If cbxAuswahl.Checked = True Then
                    If CStr(txtZulDate.Text).Length > 0 Then
                        row("SelectedDate") = CStr(txtZulDate.Text)
                    ElseIf CStr(txtGridDatum.Text).Length > 0 Then
                        row("SelectedDate") = CStr(txtGridDatum.Text)
                    Else
                        row("SelectedDate") = ""
                    End If

                    row("SelectedEinzel") = cbxAuswahl.Checked
                Else
                    row("SelectedDate") = ""
                    row("SelectedEinzel") = False

                End If
                row("SelectedAlle") = ""
                row("Status") = ""
            Next
        End If
        'Listbox updaten...
        fillPDI(lstPDI.SelectedItem.Value)
        fillMOD(lstPDI.SelectedItem.Value, lstMOD.SelectedItem.Value)
        fillCAR(lstPDI.SelectedItem.Value, lstMOD.SelectedItem.Value)
        fillVersicherer()
        setBezeichnung()

        objSuche.Result.AcceptChanges()
        Session("objSuche") = objSuche
    End Sub

    Private Sub fillPDI(Optional ByVal strSetIndex As String = "")
        Dim vwPDI As DataView
        objSuche = CType(Session("objSuche"), CarRent01)

        vwPDI = objSuche.AllPDIs.DefaultView

        If vwPDI.Count > 0 Then
            vwPDI.Sort = "DADPDI ASC"
            With lstPDI
                .DataSource = vwPDI
                .DataTextField = "PDIName"
                .DataValueField = "DADPDI"
                .DataBind()
            End With
            If strSetIndex = String.Empty Then
                lstPDI.Items(0).Selected = True
                objSuche.PSelectedPDI = lstPDI.Items(0).Value
            Else
                lstPDI.Items.FindByValue(strSetIndex).Selected = True
                objSuche.PSelectedPDI = strSetIndex
            End If
        End If
    End Sub

    Private Sub fillMOD(ByVal strPDI As String, Optional ByVal strSetIndex As String = "")
        Dim vwMOD As DataView
        Dim item As ListItem
        objSuche = CType(Session("objSuche"), CarRent01)

        vwMOD = objSuche.AllMODs.DefaultView
        vwMOD.RowFilter = "DADPDI='" & strPDI & "'"

        If vwMOD.Count > 0 Then
            vwMOD.Sort = "ZZHANDELSNAME ASC"
            With lstMOD
                .DataSource = vwMOD
                .DataTextField = "HandelsnameAnzahl"
                .DataValueField = "ZZHANDELSNAME"
                .DataBind()
            End With
            If strSetIndex = String.Empty Then
                lstMOD.Items(0).Selected = True
            Else
                item = lstMOD.Items.FindByValue(strSetIndex)
                If Not item Is Nothing Then
                    item.Selected = True
                    objSuche.PSelectedMOD = strSetIndex
                Else
                    lstMOD.Items(0).Selected = True
                    objSuche.PSelectedMOD = lstMOD.Items(0).Value
                End If
            End If
            'lblModelle.Text = vwMOD.Count
        End If
    End Sub

    Private Sub fillCAR(ByVal strPDI As String, ByVal strHandelsname As String)
        objSuche = CType(Session("objSuche"), CarRent01)

        Dim tmpDataView As DataView = objSuche.Result.DefaultView
        tmpDataView.RowFilter = "DADPDI='" & strPDI & "' AND ZZHANDELSNAME ='" & strHandelsname & "'"

        FillGrid(0, tmpDataView)

    End Sub

    Private Sub FillGrid(ByVal intPageIndex As Int32, ByVal tmpDataView As DataView, Optional ByVal strSort As String = "")

        If objSuche.Status = 0 OrElse objSuche.Status = -1111 Then
            If objSuche.Result.Rows.Count = 0 Then
                Result.Visible = False
            Else
                Result.Visible = True
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

                gvFahrzeuge.PageIndex = intTempPageIndex

                gvFahrzeuge.DataSource = tmpDataView
                gvFahrzeuge.DataBind()

            End If
        Else
            gvFahrzeuge.Visible = False
            lblError.Text = objSuche.Message
        End If
    End Sub

    Private Sub fillVersicherer(Optional ByVal strSetIndex As String = "")
        Dim vwVersicherer As DataView
        Dim item As ListItem
        objSuche = CType(Session("objSuche"), CarRent01)

        objSuche.Versicherer.DefaultView.Sort = "NAME1"
        vwVersicherer = objSuche.Versicherer.DefaultView

        If vwVersicherer.Count > 0 Then
            With ddlVersicherer
                .DataSource = vwVersicherer
                .DataTextField = "NAME1"
                .DataValueField = "VERSICHERER"
                .DataBind()
            End With
            If strSetIndex = String.Empty Then
                ddlVersicherer.Items(0).Selected = True
            Else
                item = ddlVersicherer.Items.FindByValue(strSetIndex)
                If Not item Is Nothing Then
                    item.Selected = True
                Else
                    ddlVersicherer.Items(0).Selected = True
                    If vwVersicherer.Count > 0 Then
                        ddlVersicherer.Enabled = True
                        ddlVersicherer.Items(0).Selected = True
                    Else
                        ddlVersicherer.Enabled = False
                    End If
                End If
            End If

        End If
    End Sub

    Private Sub fillKreise()
        Dim vwKreise As DataView
        objSuche = CType(Session("objSuche"), CarRent01)

        objSuche.Zulassungskreise.DefaultView.Sort = "Kreis"
        vwKreise = objSuche.Zulassungskreise.DefaultView

        If vwKreise.Count > 0 Then
            With ddlZulKreis
                .DataSource = vwKreise
                .DataTextField = "Kreis"
                .DataValueField = "ID"
                .DataBind()
            End With
        End If
    End Sub

    Private Sub setBezeichnung()
        Dim rowSelectedPDI As DataRow
        Dim rowSelectedMOD As DataRow()
        Dim strSelectedPDI As String
        Dim strSelectedMOD As String

        objSuche = CType(Session("objSuche"), CarRent01)

        Try
            'PDI-Bezeichnung
            rowSelectedPDI = objSuche.AllPDIs.Select("DADPDI='" & lstPDI.SelectedItem.Value & "'")(0)
            strSelectedPDI = CStr(rowSelectedPDI("PDIName"))
            'Modellbezeichnung
            rowSelectedMOD = objSuche.AllMODs.Select("DADPDI='" & lstPDI.SelectedItem.Value & "' AND ZZHANDELSNAME='" & lstMOD.SelectedItem.Value & "'")

            If rowSelectedMOD.Length = 1 Then
                strSelectedMOD = CStr(rowSelectedMOD(0)("HandelsnameAnzahl"))
            Else
                strSelectedMOD = String.Empty
            End If
        Catch ex As Exception
            lblError.Text = "Fehler bei der Ermittlung der PDI-Bezeichnung."
        End Try
    End Sub

    Private Sub gvFahrzeuge_PageIndexChanged(ByVal pageindex As Int32) Handles GridNavigation1.PagerChanged
        gvFahrzeuge.EditIndex = -1
        objSuche = CType(Session("objSuche"), CarRent01)

        Dim tmpDataView As DataView = objSuche.Result.DefaultView
        tmpDataView.RowFilter = "DADPDI='" & lstPDI.SelectedItem.Value & "' AND ZZHANDELSNAME ='" & lstMOD.SelectedItem.Value & "'"
        FillGrid(pageindex, tmpDataView)
    End Sub

    Private Sub GridNavigation1_ddlPageSizeChanged() Handles GridNavigation1.PageSizeChanged
        objSuche = CType(Session("objSuche"), CarRent01)

        Dim tmpDataView As DataView = objSuche.Result.DefaultView
        tmpDataView.RowFilter = "DADPDI='" & lstPDI.SelectedItem.Value & "' AND ZZHANDELSNAME ='" & lstMOD.SelectedItem.Value & "'"
        FillGrid(gvFahrzeuge.PageIndex, tmpDataView)
    End Sub

    Private Sub gvFahrzeuge_RowCommand(ByVal sender As Object, ByVal e As GridViewCommandEventArgs) Handles gvFahrzeuge.RowCommand
        If e.CommandName = "Copy" Then
            Dim index As Integer = CType(e.CommandArgument, Integer)
            fillTable(index)
        End If

    End Sub

    Private Sub gvFahrzeuge_RowCreated(ByVal sender As Object, ByVal e As GridViewRowEventArgs) Handles gvFahrzeuge.RowCreated
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim addImgButton As ImageButton = CType(e.Row.Cells(0).Controls(0).FindControl("ImageButton1"), ImageButton)
            addImgButton.CommandArgument = e.Row.RowIndex.ToString()
        End If
    End Sub

    Private Sub gvFahrzeuge_RowDataBound(ByVal sender As Object, ByVal e As GridViewRowEventArgs) Handles gvFahrzeuge.RowDataBound
        Dim cbxAuswahl As CheckBox
        Dim row As DataRow
        Dim calControl As TextBox
        Dim lblRowID As Label

        If e.Row.RowType = DataControlRowType.DataRow Then

            lblRowID = e.Row.Cells(0).FindControl("lblRowId")
            row = objSuche.Result.Select("RowID='" & lblRowID.Text & "'")(0)       'Tabellenzeile holen
            calControl = CType(e.Row.FindControl("calControl"), TextBox)
            calControl.Text = row("SelectedDate")
            cbxAuswahl = CType(e.Row.FindControl("cbxAuswahl"), CheckBox)
            If CStr(row("SelectedEinzel")) <> String.Empty Then
                cbxAuswahl.Checked = CBool(row("SelectedEinzel"))
            End If

        End If

    End Sub

    Private Function checkInput(ByRef intAnzahl As Integer) As Boolean
        Dim datCheck As Date
        Dim strDate As String
        Dim blnCheck As Boolean
        Dim intErrors As Integer
        Dim strStatus As String
        Dim row As DataRow
        Dim cbxAuswahl As CheckBox
        Dim txtGridDatum As TextBox

        intAnzahl = 0
        intErrors = 0

        If ddlZulKreis.Visible = True Then
            objSuche.Zentral = False
        Else
            objSuche.Zentral = True
        End If

        For iCount = 0 To gvFahrzeuge.Rows.Count - 1
            Dim lblTempRow As Label
            lblTempRow = gvFahrzeuge.Rows(iCount).Cells(0).FindControl("lblRowId")
            row = objSuche.Result.Select("RowID='" & lblTempRow.Text & "'")(0)

            cbxAuswahl = CType(gvFahrzeuge.Rows(iCount).Cells(0).FindControl("cbxAuswahl"), CheckBox)
            txtGridDatum = CType(gvFahrzeuge.Rows(iCount).Cells(0).FindControl("calControl"), TextBox)
            'Datum setzen
            If cbxAuswahl.Checked = True Then
                If CStr(txtGridDatum.Text).Length > 0 Then
                    row("SelectedDate") = CStr(txtGridDatum.Text)
                ElseIf CStr(txtZulDate.Text).Length > 0 Then
                    row("SelectedDate") = CStr(txtZulDate.Text)
                Else
                    row("SelectedDate") = ""
                End If

                row("SelectedEinzel") = cbxAuswahl.Checked
            Else
                row("SelectedDate") = ""
                row("SelectedEinzel") = False

            End If
            row("SelectedAlle") = ""
            row("Status") = ""
        Next

        For Each row In objSuche.Result.Rows
            strStatus = String.Empty
            blnCheck = True

            If CBool(row("SelectedEinzel")) = True Then 'Zeile ausgewählt?
                intAnzahl += 1

                '*** Zulassungsdatum überprüfen --------------------------
                strDate = CStr(row("SelectedDate"))

                Try
                    '...Überhaupt Datum?
                    datCheck = CDate(strDate)
                Catch ex As Exception
                    blnCheck = False
                    strStatus = "Ungültiges Datum."
                End Try

                If blnCheck Then
                    '...Datum < Tagesdatum?
                    If datCheck < Date.Today Then
                        blnCheck = False
                        strStatus = "Zulassungsdatum darf nicht in der Vergangenheit liegen"
                    End If
                End If

                If blnCheck Then
                    '...Datum = Tagesdatum?
                    If datCheck = Date.Today Then
                        blnCheck = False
                        strStatus = "Zulassungsdatum darf nicht auf den heutigen Tag fallen"
                    End If
                End If

                If blnCheck Then
                    '...Wochenende ?
                    If (datCheck.DayOfWeek = DayOfWeek.Saturday) Or (datCheck.DayOfWeek = DayOfWeek.Sunday) Then
                        blnCheck = False
                        strStatus = "Zulassungsdatum ungültig (Wochenende)."
                    End If
                End If

                If blnCheck Then
                    '...Feiertag ?
                    Dim feiertagsListe As New DeutscheFeiertageEinesJahres(datCheck.Year)
                    If feiertagsListe.Feiertage.Any(Function(tag) tag.Datum.Date = datCheck.Date) Then
                        blnCheck = False
                        strStatus = "Zulassungsdatum ungültig (Feiertag)."
                    End If
                End If

                If blnCheck Then
                    If ddlVersicherer.SelectedIndex = -1 Then
                        blnCheck = False
                        strStatus = "Bitte wählen Sie eine Versicherer aus."
                    Else
                        row("Versicherer") = ddlVersicherer.SelectedItem.Value
                    End If
                End If

                If Not blnCheck Then
                    intErrors += 1
                    gvFahrzeuge.Columns(gvFahrzeuge.Columns.Count - 2).Visible = True
                End If
                row("Status") = strStatus
            End If

            objSuche.Result.AcceptChanges()
        Next

        If (intAnzahl = 0) Then     'Keine Fahrzeuge ausgewäht -> Fehler...
            intErrors = 1
            lblError.Text = "Keine Fahrzeuge zum Zulassen ausgewählt."
        End If

        Session("objSuche") = objSuche

        Return (intErrors = 0)

    End Function

    Private Sub fillCARErrors()
        objSuche = CType(Session("objSuche"), CarRent01)

        Dim tmpDataView As DataView = objSuche.Result.DefaultView
        tmpDataView.RowFilter = "Status<>'" & String.Empty & "'"

        FillGrid(0, tmpDataView)
    End Sub

    Protected Sub lstPDI_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles lstPDI.SelectedIndexChanged
        setBezeichnung()
        fillTable()
    End Sub

    Protected Sub lstMOD_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles lstMOD.SelectedIndexChanged
        setBezeichnung()
        fillTable()
    End Sub

    Protected Sub ImageButton1_Click(ByVal sender As Object, ByVal e As ImageClickEventArgs)
        fillTable()
    End Sub

    Private Sub gvFahrzeuge_Sorting(ByVal sender As Object, ByVal e As GridViewSortEventArgs) Handles gvFahrzeuge.Sorting
        objSuche = CType(Session("objSuche"), CarRent01)

        Dim tmpDataView As DataView = objSuche.Result.DefaultView
        tmpDataView.RowFilter = "DADPDI='" & lstPDI.SelectedItem.Value & "' AND ZZHANDELSNAME ='" & lstMOD.SelectedItem.Value & "'"

        FillGrid(gvFahrzeuge.PageIndex, tmpDataView, e.SortExpression)
    End Sub

    Protected Sub btnConfirm_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnConfirm.Click
        Dim intAnzahl As Integer
        If Not Session("objSuche") Is Nothing Then
            objSuche = CType(Session("objSuche"), CarRent01)
        End If
        If objSuche Is Nothing Then
            Exit Sub
        End If

        If rblAuswahl.Visible = True Then

            If rblAuswahl.SelectedValue = "" Then

                lblDezError.Text = "Bitte wählen Sie eine Zulassungsart aus."
                Exit Sub

            End If
        End If

        If ddlZulKreis.Visible = True Then
            objSuche.Zentral = False
            If ddlZulKreis.SelectedValue = "0" Then
                lblDezError.Text = "Bitte wählen Sie einen Zulassungskreis aus."
                Exit Sub
            Else
                objSuche.ZulKreis = ddlZulKreis.SelectedItem.Text
            End If
        Else
            objSuche.Zentral = True
        End If

        If Not checkInput(intAnzahl) Then
            If intAnzahl > 0 Then
                fillCARErrors()
            End If
            Exit Sub
        End If
        Response.Redirect("Change01_2.aspx?AppID=" & Session("AppID").ToString, False)
    End Sub

    Protected Sub lbtSelect_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lbtSelect.Click
        fillTable()
    End Sub

    Protected Sub cbxAuswahl_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs)
        fillTable()
    End Sub

    Private Sub lbBack_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lbBack.Click
        Response.Redirect("/Services/(S(" & Session.SessionID & "))/Start/Selection.aspx?AppID=" & Session("AppID").ToString, False)
    End Sub

    Protected Sub rblAuswahl_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles rblAuswahl.SelectedIndexChanged
        If rblAuswahl.SelectedValue = "1" Then 'Zentral
            trZulassungskreis.Visible = False
            tab1.Visible = True
            Initialload()
            btnConfirm.Enabled = True
            Session("objSuche") = objSuche
        Else 'Dezentral
            trZulassungskreis.Visible = True
            tab1.Visible = False
            Result.Visible = False
            InitialloadDezentral()
            Session("objSuche") = objSuche
        End If
    End Sub

    Protected Sub ddlZulKreis_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ddlZulKreis.SelectedIndexChanged
        If ddlZulKreis.SelectedValue = "0" Then
            Result.Visible = False
            tab1.Visible = False
            lblDezError.Text = "Bitte wählen Sie einen Zulassungskreis aus."
            Exit Sub
        Else
            tab1.Visible = True
            btnConfirm.Enabled = True
            FillDezentral()
            Session("objSuche") = objSuche
        End If
    End Sub

    Protected Sub lbtSelectAll_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lbtSelectAll.Click

        Dim Counter As Integer

        Counter = gvFahrzeuge.Rows.Count


        If Counter > 0 Then

            CType(gvFahrzeuge.Rows(0).FindControl("txtMenge"), TextBox).Text = Counter

            fillTable()
            Session("objSuche") = objSuche
        End If



    End Sub

End Class