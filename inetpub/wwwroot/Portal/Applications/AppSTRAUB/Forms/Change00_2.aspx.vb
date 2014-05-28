Imports CKG.Base.Business
Imports CKG.Base.Kernel
Imports CKG.Portal.PageElements
Imports CKG.Base.Kernel.Common.Common


Public Class Change00_2
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
    'Private objHaendler As SixtLease_02

    Protected WithEvents lnkKreditlimit As System.Web.UI.WebControls.HyperLink
    Protected WithEvents cmdSave As System.Web.UI.WebControls.LinkButton
    Protected WithEvents lblPageTitle As System.Web.UI.WebControls.Label
    Protected WithEvents ucHeader As Header
    Protected WithEvents ucStyles As Styles
    Protected WithEvents lblHead As System.Web.UI.WebControls.Label
    Private versandart As String
    Private upload As Boolean
    Protected WithEvents trSumme As System.Web.UI.HtmlControls.HtmlTableRow
    Private objSuche As Change_01
    Private rowID_PDI As String
    Private rowID_MOD As String
    Protected WithEvents Literal2 As System.Web.UI.WebControls.Literal
    Private infoArray As Array
    Private highlightID As String

    '-----------------------------------------------------------
    Private columnID As Integer = 0
    Private columnArt As Integer = 1
    Private columnDropDown_Kennzeichenserie As Integer = 16
    Private columnTextZuldatum As Integer = 15
    Private columnTextZulAnzahl As Integer = 17
    Private columnTextWerteMerken As Integer = 18
    Protected WithEvents DataGrid1 As System.Web.UI.WebControls.DataGrid
    Protected WithEvents lstPDI As System.Web.UI.WebControls.ListBox
    Protected WithEvents lstMOD As System.Web.UI.WebControls.ListBox
    Protected WithEvents lblFahrzeuge As System.Web.UI.WebControls.Label
    Protected WithEvents lblPDIs As System.Web.UI.WebControls.Label
    Protected WithEvents lblModelle As System.Web.UI.WebControls.Label
    Protected WithEvents lblError As System.Web.UI.WebControls.Label
    '-----------------------------------------------------------
    Protected WithEvents btnConfirm As System.Web.UI.WebControls.LinkButton

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        m_User = GetUser(Me)
        ucHeader.InitUser(m_User)
        FormAuth(Me, m_User)
        GetAppIDFromQueryString(Me)

        Try
            lblHead.Text = m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString
            ucStyles.TitleText = lblHead.Text
            m_App = New Base.Kernel.Security.App(m_User)
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
        Dim objSuche As Change_01

        objSuche = CType(Session("objSuche"), Change_01)
        objSuche.getCars()

        If (objSuche.Status = 0) AndAlso (objSuche.Result.Rows.Count > 0) Then
            Session("objSuche") = objSuche
            fillPDI()
            fillMOD(lstPDI.SelectedItem.Value)
            fillCAR(lstPDI.SelectedItem.Value, lstMOD.SelectedItem.Value)
        Else
            lblError.Text = "Keine Fahrzeuge vorhanden."
            DataGrid1.Visible = False
            lstPDI.Visible = False
            lstMOD.Visible = False
            btnConfirm.Enabled = False
        End If
    End Sub

    Private Sub PostbackLoad()
        lblError.Text = String.Empty
        fillTable()
        'insertScript()
    End Sub

    Private Sub fillPDI(Optional ByVal strSetIndex As String = "")
        Dim vwPDI As DataView
        objSuche = CType(Session("objSuche"), Change_01)

        vwPDI = objSuche.Result.DefaultView
        vwPDI.RowFilter = "Art = 'PDI'"

        If vwPDI.Count > 0 Then
            With lstPDI
                .DataSource = vwPDI
                .DataTextField = "PDIAnzahl"
                .DataValueField = "KUNPDI"
                .DataBind()
            End With
            If strSetIndex = String.Empty Then
                lstPDI.Items(0).Selected = True
            Else
                lstPDI.Items.FindByValue(strSetIndex).Selected = True
            End If
            lblPDIs.Text = vwPDI.Count
        End If
    End Sub

    Private Sub fillMOD(ByVal strPDI As String, Optional ByVal strSetIndex As String = "")
        Dim vwMOD As DataView
        Dim item As ListItem
        objSuche = CType(Session("objSuche"), Change_01)

        vwMOD = objSuche.Result.DefaultView
        vwMOD.RowFilter = "Art = 'MOD' AND KUNPDI='" & strPDI & "'"

        If vwMOD.Count > 0 Then
            With lstMOD
                .DataSource = vwMOD
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
            lblModelle.Text = vwMOD.Count
        End If
    End Sub

    Private Sub fill(ByVal tmpDataView As DataView)
        Dim item As DataGridItem
        Dim itemList As ListItem
        Dim txtBoxZul As TextBox
        Dim txtBoxBem As TextBox
        Dim txtBoxBemDat As TextBox
        'Dim ddlVerwendung As DropDownList
        Dim ddlVersicherung As DropDownList
        Dim lblInfo1 As System.Web.UI.WebControls.Image
        Dim ddlKopieren As DropDownList
        Dim cbxAuswahl As CheckBox
        Dim row As DataRow
        Dim rowBemerkung As DataRow()
        Dim selectedRow As DataRow
        Dim rowLoop As DataRow
        Dim valueSelected As String
        Dim textSelected As String
        Dim intItemCounter As Integer
        Dim tblTexte As DataTable

        tblTexte = objSuche.PTexte      'Bemerkungstabelle

        If tmpDataView.Count = 0 Then
            DataGrid1.Visible = False
        Else
            tmpDataView.Sort = "Zusatzdaten ASC, ZZDAT_EIN ASC"              'Sortierung nach Eingangsdatum (aufsteigend)
            DataGrid1.DataSource = tmpDataView
            DataGrid1.DataBind()

            lblFahrzeuge.Text = tmpDataView.Count

            'Controls befüllen
            For Each item In DataGrid1.Items

                row = objSuche.Result.Select("RowID='" & item.Cells(0).Text & "'")(0)       'Tabellenzeile holen

                'Verwendung**************
                'ddlVerwendung = CType(item.FindControl("ddlVerwendung"), DropDownList)
                'If (CStr(row("SelectedVerwendung")) <> String.Empty) Then
                '    selectedRow = objSuche.PVerwendung.Select("Valpos='" & CStr(row("SelectedVerwendung")) & "'")(0)
                '    valueSelected = CStr(selectedRow("Valpos"))
                '    textSelected = CStr(selectedRow("Zverwendung"))
                '    'Ersten Eintrag setzen (ausgewählt), da SelectedIndex nicht funktioniert!!!?
                '    itemList = New ListItem()
                '    itemList.Value = valueSelected
                '    itemList.Text = textSelected
                '    ddlVerwendung.Items.Add(itemList)
                'Else
                '    'Vorauswahl = Selbstfahrervermietfahrzeug
                '    itemList = New ListItem()
                '    itemList.Value = "0004"
                '    itemList.Text = objSuche.PVerwendung.Select("Valpos='" & itemList.Value & "'")(0)("Zverwendung")
                '    ddlVerwendung.Items.Add(itemList)
                '    valueSelected = itemList.Value
                'End If

                'For Each rowLoop In objSuche.PVerwendung.Rows
                '    If (CStr(rowLoop("Valpos")) <> valueSelected) Then
                '        itemList = New ListItem()
                '        itemList.Value = CStr(rowLoop("Valpos"))
                '        itemList.Text = CStr(rowLoop("Zverwendung"))
                '        ddlVerwendung.Items.Add(itemList)
                '    End If
                'Next

                'Versicherung***************
                ddlVersicherung = CType(item.FindControl("ddlVersicherung"), DropDownList)
                If (CStr(row("SelectedVersicherung")) <> String.Empty) Then
                    selectedRow = objSuche.PVersicherer.Select("VersichererID='" & CStr(row("SelectedVersicherung")) & "'")(0)
                    valueSelected = CStr(selectedRow("VersichererID"))
                    textSelected = CStr(selectedRow("Name1"))
                    'Ersten Eintrag setzen (ausgewählt), da SelectedIndex nicht funktioniert!!!?
                    itemList = New ListItem()
                    itemList.Value = valueSelected
                    itemList.Text = textSelected
                    ddlVersicherung.Items.Add(itemList)
                Else
                    valueSelected = String.Empty
                End If

                For Each rowLoop In objSuche.PVersicherer.Rows
                    If (CStr(rowLoop("VersichererID")) <> valueSelected) Then
                        itemList = New ListItem()
                        itemList.Value = CStr(rowLoop("VersichererID"))
                        itemList.Text = CStr(rowLoop("Name1"))
                        ddlVersicherung.Items.Add(itemList)
                    End If
                Next

                'Zulassungsdatum***************
                txtBoxZul = CType(item.FindControl("txtZulassungsdatum"), TextBox)
                txtBoxZul.Text = CStr(row("SelectedDate"))

                'Bemerkung***************
                txtBoxBem = CType(item.FindControl("txtBemerkung"), TextBox)
                txtBoxBem.Text = CStr(row("Bemerkung"))
                txtBoxBemDat = CType(item.FindControl("txtDatumBemerkung"), TextBox)
                txtBoxBemDat.Text = CStr(row("DatumBemerkung")).TrimStart("0"c)

                'Auswahlbox***************
                cbxAuswahl = CType(item.FindControl("cbxAuswahl"), CheckBox)
                If CStr(row("SelectedEinzel")) <> String.Empty Then
                    cbxAuswahl.Checked = CBool(row("SelectedEinzel"))
                End If

                'Kopieren (Dropdownlist)*************
                ddlKopieren = CType(item.FindControl("ddlKopieren"), DropDownList)
                lblInfo1 = CType(item.FindControl("lblInfo1"), System.Web.UI.WebControls.Image)

                If item.ItemIndex = DataGrid1.Items.Count - 1 Then
                    ddlKopieren.Visible = False
                    lblInfo1.Visible = False
                Else
                    For intItemCounter = 0 To DataGrid1.Items.Count - item.ItemIndex - 1
                        itemList = New ListItem()
                        'itemList.Text = CStr(intItemCounter)
                        itemList.Text = CStr(intItemCounter + 1)
                        itemList.Value = intItemCounter
                        ddlKopieren.Items.Add(itemList)
                        ddlKopieren.Width = System.Web.UI.WebControls.Unit.Pixel(40)
                    Next
                End If

                'Felder ein/ausblenden wenn keine Zulassung
                If objSuche.Task <> "Zulassung" Then
                    txtBoxBem.ReadOnly = False
                    txtBoxBem.CssClass = "TextBoxStyle"
                    txtBoxBem.BackColor = System.Drawing.Color.White
                    txtBoxBemDat.ReadOnly = False
                    txtBoxBemDat.CssClass = "TextBoxStyle"
                    txtBoxBemDat.BackColor = System.Drawing.Color.White
                End If
            Next
        End If
    End Sub


    Private Sub fillCARErrors()
        Dim tmpDataView As New DataView()

        objSuche = CType(Session("objSuche"), Change_01)

        tmpDataView = objSuche.Result.DefaultView
        tmpDataView.RowFilter = "Status<>'" & String.Empty & "'"

        fill(tmpDataView)
    End Sub

    Private Sub fillCAR(ByVal strPDI As String, ByVal strMOD As String)
        Dim tmpDataView As New DataView()
        objSuche = CType(Session("objSuche"), Change_01)

        tmpDataView = objSuche.Result.DefaultView
        tmpDataView.RowFilter = "Art='CAR' AND KUNPDI='" & strPDI & "' AND ZZMODELL='" & strMOD & "'"

        fill(tmpDataView)
    End Sub

    Private Sub fillTable()

        Dim ddlVersicherung As DropDownList
        'Dim ddlVerwendung As DropDownList
        Dim ddlKopieren As DropDownList
        Dim cbxAuswahl As CheckBox
        Dim txtZulassungsdatum As TextBox
        Dim txtBemerkungdatum As TextBox
        Dim txtBemerkung As TextBox
        Dim item As DataGridItem
        Dim row As DataRow
        Dim rowPDIs As DataRow()
        Dim rowMODs As DataRow()
        Dim anzahl As Integer = 0
        Dim rowID As String
        Dim intCounter As Integer
        Dim strSonder As String

        objSuche = CType(Session("objSuche"), Change_01)

        For Each item In DataGrid1.Items
            row = objSuche.Result.Select("RowID='" & item.Cells(0).Text & "'")(0)       'Tabellenzeile holen

            'Kopieren setzen
            ddlKopieren = CType(item.FindControl("ddlKopieren"), DropDownList)
            If (Not ddlKopieren.SelectedItem Is Nothing) AndAlso (CInt(ddlKopieren.SelectedItem.Value) > 0) Then
                For intCounter = 0 To CInt(ddlKopieren.SelectedItem.Value)
                    row = objSuche.Result.Select("RowID='" & DataGrid1.Items(item.ItemIndex + intCounter).Cells(0).Text & "'")(0)

                    row("SelectedEinzel") = True 'cbxAuswahl.Checked
                    row("SelectedAlle") = ddlKopieren.SelectedItem.Value

                    'Verwendung setzen***********
                    'ddlVerwendung = CType(item.FindControl("ddlVerwendung"), DropDownList)
                    'row("SelectedVerwendung") = ddlVerwendung.SelectedItem.Value

                    'Versicherung setzen***********
                    ddlVersicherung = CType(item.FindControl("ddlVersicherung"), DropDownList)
                    row("SelectedVersicherung") = ddlVersicherung.SelectedItem.Value
                    row("SelectedVersicherungText") = ddlVersicherung.SelectedItem.Text

                    'Auswahl setzen
                    cbxAuswahl = CType(item.FindControl("cbxAuswahl"), CheckBox)
                    row("SelectedEinzel") = cbxAuswahl.Checked

                    'Datum setzen
                    txtZulassungsdatum = CType(item.FindControl("txtZulassungsdatum"), TextBox)
                    row("SelectedDate") = txtZulassungsdatum.Text

                    'BemerkungDatum setzen
                    txtBemerkungdatum = CType(item.FindControl("txtDatumBemerkung"), TextBox)
                    row("DatumBemerkung") = txtBemerkungdatum.Text

                    'Bemerkung setzen
                    txtBemerkung = CType(item.FindControl("txtBemerkung"), TextBox)
                    row("Bemerkung") = txtBemerkung.Text
                Next
            End If

            If CInt(row("SelectedAlle")) = 0 Then
                'Verwendung setzen
                'ddlVerwendung = CType(item.FindControl("ddlVerwendung"), DropDownList)
                'row("SelectedVerwendung") = ddlVerwendung.SelectedItem.Value

                'Versicherung setzen
                ddlVersicherung = CType(item.FindControl("ddlVersicherung"), DropDownList)
                row("SelectedVersicherung") = ddlVersicherung.SelectedItem.Value
                row("SelectedVersicherungText") = ddlVersicherung.SelectedItem.Text

                'Auswahl setzen
                cbxAuswahl = CType(item.FindControl("cbxAuswahl"), CheckBox)
                row("SelectedEinzel") = cbxAuswahl.Checked

                'Datum setzen
                txtZulassungsdatum = CType(item.FindControl("txtZulassungsdatum"), TextBox)
                row("SelectedDate") = txtZulassungsdatum.Text

                'BemerkungDatum setzen
                txtBemerkungdatum = CType(item.FindControl("txtDatumBemerkung"), TextBox)
                row("DatumBemerkung") = txtBemerkungdatum.Text

                'Bemerkung setzen
                txtBemerkung = CType(item.FindControl("txtBemerkung"), TextBox)
                row("Bemerkung") = txtBemerkung.Text
            End If
        Next

        'Bereits gesetzte Werte wieder löschen...
        For Each row In objSuche.Result.Rows
            row("SelectedAlle") = 0
        Next

        'Listboxen aktualisieren : PDIs
        rowPDIs = objSuche.Result.Select("SelectedEinzel=True AND KUNPDI = '" & item.Cells(2).Text & "'")         'PDIs
        If rowPDIs.Length > 0 Then
            row = objSuche.Result.Select("Art='PDI' AND KUNPDI='" & item.Cells(2).Text & "'")(0)
            row("PDIAnzahl") = CStr(row("KUNPDI")) & " (" & rowPDIs.Length & ")"
        End If

        'Listboxen aktualisieren : MODs
        rowMODs = objSuche.Result.Select("SelectedEinzel=True AND ZZMODELL='" & item.Cells(3).Text & "' AND KUNPDI = '" & item.Cells(2).Text & "'")          'PDIs
        If rowMODs.Length > 0 Then
            row = objSuche.Result.Select("Art='MOD' AND KUNPDI = '" & item.Cells(2).Text & "' AND ZZMODELL='" & item.Cells(3).Text & "'")(0)
            row("MODAnzahl") = CStr(row("ZZMODELL")) & " (" & rowMODs.Length & ")"
        End If

        'Listbox updaten...
        fillPDI(lstPDI.SelectedItem.Value)
        fillMOD(lstPDI.SelectedItem.Value, lstMOD.SelectedItem.Value)
        fillCAR(lstPDI.SelectedItem.Value, lstMOD.SelectedItem.Value)

        objSuche.Result.AcceptChanges()
        Session("objSuche") = objSuche
    End Sub

    Private Function checkInput(ByRef intAnzahl As Integer) As Boolean
        Dim datCheck As Date
        Dim strDate As String
        Dim blnCheck As Boolean
        Dim intErrors As Integer
        Dim strStatus As String
        Dim row As DataRow

        intAnzahl = 0
        objSuche = CType(Session("objSuche"), Change_01)
        intErrors = 0

        For Each row In objSuche.Result.Rows
            strStatus = String.Empty
            blnCheck = True

            ''*** Zulassungsdatum überprüfen --------------------------
            'strDate = CStr(row("SelectedDate"))
            If CBool(row("SelectedEinzel")) = True Then 'Zeile ausgewählt?
                intAnzahl += 1

                If objSuche.Task = "Zulassung" Then
                    '1. ZULASSUNG
                    '*** Zulassungsdatum überprüfen --------------------------
                    strDate = CStr(row("SelectedDate"))

                    Try
                        '...Überhaupt Datum?
                        datCheck = CDate(strDate)
                    Catch ex As Exception
                        blnCheck = False
                        strStatus = "Ungültiges Zulassungsatum."
                    End Try

                    If blnCheck Then
                        '...Format ok. Datum < Tagesdatum+3?
                        If CDate(strDate) < Date.Today.AddDays(3) Then
                            blnCheck = False
                            strStatus = "Zulassungsdatum muß mindestens 3 Werktage in der Zukunft liegen."
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
                Else
                    '2. DISPOSITION
                    '*** Bemerkungsdatum überprüfen-------------------------------
                    strDate = CStr(row("DatumBemerkung"))

                    Try
                        '...Überhaupt Datum?
                        datCheck = CDate(strDate)
                    Catch ex As Exception
                        blnCheck = False
                        strStatus = "Ungültiges Bemerkungsdatum."
                    End Try
                End If
            End If

            If Not blnCheck Then
                intErrors += 1
            End If
            row("Status") = strStatus
            objSuche.Result.AcceptChanges()
        Next
        If (intAnzahl = 0) Then     'Keine Fahrzeuge ausgewäht -> Fehler...
            intErrors = 1
            lblError.Text = "Keine Fahrzeuge zum Zulassen ausgewählt."
        End If
        Return (intErrors = 0)
    End Function

    Private Sub insertScript()
        If Not (highlightID Is Nothing) Then
            Literal2.Text = "						<script language=""Javascript"">" & vbCrLf
            Literal2.Text &= "						  <!-- //" & vbCrLf
            Literal2.Text &= "						    window.document.location.href = ""#" & highlightID & """;" & vbCrLf
            Literal2.Text &= "						  //-->" & vbCrLf
            Literal2.Text &= "						</script>" & vbCrLf
        End If
    End Sub

    Private Sub btnConfirm_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnConfirm.Click
        Dim row As DataRow
        Dim blnStatus As String
        Dim intAnzahl As Integer

        If Not checkInput(intAnzahl) Then
            If intAnzahl > 0 Then
                fillCARErrors()
            End If
            Exit Sub
        End If
        Response.Redirect("Change00_3.aspx?AppID=" & Session("AppID").ToString)
    End Sub

    Private Sub lstPDI_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lstPDI.SelectedIndexChanged
        'objSuche = CType(Session("objSuche"), Change_01)
        'objSuche.PSelectedPDI = lstPDI.SelectedItem.Value
        'objSuche.PSelectionPDI = True
        'objSuche.PSelectionMOD = False
        'Session("objSuche") = objSuche
        fillMOD(lstPDI.SelectedItem.Value)
        fillCAR(lstPDI.SelectedItem.Value, lstMOD.SelectedItem.Value)
    End Sub

    Private Sub lstMOD_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lstMOD.SelectedIndexChanged
        'fillMOD(lstPDI.SelectedItem.Value)
        fillCAR(lstPDI.SelectedItem.Value, lstMOD.SelectedItem.Value)
    End Sub

    Private Sub DataGrid1_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles DataGrid1.ItemDataBound
        objSuche = CType(Session("objSuche"), Change_01)

        If objSuche.Task <> "Zulassung" Then
            e.Item.Cells(8).Visible = False
            e.Item.Cells(10).Visible = False
        End If

    End Sub

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        SetEndASPXAccess(Me)
    End Sub

    Private Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Unload
        SetEndASPXAccess(Me)
    End Sub
End Class

' ************************************************
' $History: Change00_2.aspx.vb $
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 8.04.08    Time: 17:38
' Created in $/CKAG/Applications/AppSTRAUB/Forms
' 
' *****************  Version 9  *****************
' User: Uha          Date: 2.07.07    Time: 14:28
' Updated in $/CKG/Applications/AppSTRAUB/AppSTRAUBWeb/Forms
' Logging der Laufzeiten der ASPX-Seiten eingeführt
' 
' *****************  Version 8  *****************
' User: Uha          Date: 8.03.07    Time: 14:18
' Updated in $/CKG/Applications/AppSTRAUB/AppSTRAUBWeb/Forms
' 
' ************************************************
