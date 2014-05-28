
Imports CKG.Portal.PageElements
Imports CKG.Base.Kernel.Common.Common
Imports System.IO

<CLSCompliant(False)> Public Class _Report03
    Inherits System.Web.UI.Page

    Protected WithEvents ucStyles As Styles
    Protected WithEvents ucHeader As Header
    Protected WithEvents lblPageTitle As System.Web.UI.WebControls.Label
    Protected WithEvents gridServer As System.Web.UI.WebControls.DataGrid
    Protected WithEvents lblDatensatz As System.Web.UI.WebControls.Label
    Protected WithEvents lblMsg As System.Web.UI.WebControls.Label
    Protected WithEvents lblScript As System.Web.UI.WebControls.Label
    Protected WithEvents btnBearb As System.Web.UI.WebControls.LinkButton
    Private m_App As Base.Kernel.Security.App
    Protected WithEvents lblOpen As System.Web.UI.WebControls.Label
    Protected WithEvents Linkbutton1 As System.Web.UI.WebControls.LinkButton
    Private m_User As Base.Kernel.Security.User
    'Private showCheckbox As Boolean
    Protected WithEvents lnkKreditlimit As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lblHead As System.Web.UI.WebControls.Label
    Protected WithEvents lblNoData As System.Web.UI.WebControls.Label
    Protected WithEvents Table10 As System.Web.UI.HtmlControls.HtmlTable
    Protected WithEvents lblError As System.Web.UI.WebControls.Label
    Protected WithEvents btnUpload As System.Web.UI.WebControls.LinkButton
    Protected WithEvents td2 As System.Web.UI.HtmlControls.HtmlTableCell
    Protected WithEvents td3 As System.Web.UI.HtmlControls.HtmlTableCell
    Protected WithEvents td0 As System.Web.UI.HtmlControls.HtmlTableCell

    Protected WithEvents btnBack As System.Web.UI.WebControls.LinkButton
    'Private appl As String
    'Private Const fileNameDelimiter As Char = "-"c
    'Private Const fileExtension As String = ".JPG"
    'Private selectedDir As String
    Private booErr As Boolean


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

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        Session.Add("AppID", Request.QueryString("AppID"))

        m_User = GetUser(Me)

        ucHeader.InitUser(m_User)
        FormAuthNoReferrer(Me, m_User)

        lblHead.Text = m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString
        ucStyles.TitleText = lblHead.Text

        Try
            m_App = New Base.Kernel.Security.App(m_User)

            If Not IsPostBack Then
                'DirBrowse.initControl("C:\")
                loadForm()
            Else
                'selectedDir = lblCurdir.Text
                'DirBrowse.initControl(selectedDir)
            End If
        Catch ex As Exception
            lblError.Text = "Beim Laden der Seite ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
        End Try
    End Sub

    Private Sub loadForm()
        Dim uebf As Ueberfuehrung
        Dim table As DataTable

        uebf = New Ueberfuehrung(m_User, m_App, Session("AppID").ToString, Session.SessionID.ToString, "")
        table = uebf.readThumbsFromServer()

        If Not (table Is Nothing) AndAlso (table.Rows.Count > 0) Then
            Session("Serverfiles") = table
            FillGridServer(0)
        Else
            Session("Serverfiles") = Nothing
            lblError.Text = "Keine Daten gefunden."
            Linkbutton1.Visible = False
            btnUpload.Visible = False
            btnBack.Visible = False
            btnUpload.Enabled = False
        End If
    End Sub

    Private Sub FillGridServer(ByVal intPageIndex As Int32, Optional ByRef status As String = "", Optional ByVal strSort As String = "", Optional ByVal direction As String = "", Optional ByVal onlySaved As Boolean = False)
        Dim table As DataTable

        table = CType(Session("Serverfiles"), DataTable)
        status = String.Empty
        If table Is Nothing Then
            gridServer.DataSource = Nothing
            gridServer.DataBind()
            Exit Sub
        End If

        If table.Rows.Count = 0 Then
            gridServer.Visible = False
            lblNoData.Visible = True
            lblNoData.Text = "Keine Daten zur Anzeige gefunden."
            'ShowScript.Visible = False
        Else
            gridServer.Visible = True
            lblNoData.Visible = False

            Dim tmpDataView As New DataView()
            tmpDataView = table.DefaultView

            Dim intTempPageIndex As Int32 = intPageIndex
            Dim strTempSort As String = ""
            Dim strDirection As String = ""

            'Nur die zur Abrechnung oder Umkennzeichnung markierten Aufträge filtern!
            If (onlySaved = True) Then
                tmpDataView.RowFilter = "Save = 'X'"
            End If

            If tmpDataView.Count = 0 Then
                status = "Keine Dateien zum Archivieren ausgewählt."
                Exit Sub
            End If

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

            If (direction.Length > 0) Then
                tmpDataView.Sort = strTempSort & " " & direction
            End If

            '            gridFiles.CurrentPageIndex = intTempPageIndex
            gridServer.DataSource = tmpDataView
            gridServer.DataBind()

            If (Not Session("ShowOtherString") Is Nothing) AndAlso CStr(Session("ShowOtherString")).Length > 0 Then
                lblNoData.Text = CStr(Session("ShowOtherString"))
            Else
                lblNoData.Text = "Es wurden " & tmpDataView.Count.ToString & " Dokument(e) gefunden."
            End If
            If (Not Session("BackLink") Is Nothing) AndAlso CStr(Session("BackLink")) = "HistoryBack" Then
                lnkKreditlimit.Text = "Zurück"
                lnkKreditlimit.NavigateUrl = "javascript:history.back()"
            End If
            lblNoData.Visible = True

        End If
    End Sub

    Private Sub gridServer_SortCommand(ByVal source As System.Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs)
        FillGridServer(gridServer.CurrentPageIndex, e.SortExpression)
    End Sub

    Private Sub btnShowPics_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        loadData()
        FillGridServer(0)
        lblError.Text = String.Empty
    End Sub

    Private Sub loadData()
        Dim table As DataTable
        Dim uebf As Ueberfuehrung

        uebf = New Ueberfuehrung(m_User, m_App, Session("AppID").ToString, Session.SessionID.ToString, "")
        table = uebf.readDataFromServer()

        If Not (table Is Nothing) AndAlso (table.Rows.Count > 0) Then
            If (Session("Serverfiles") Is Nothing) Then
                Session.Add("Serverfiles", table)
            Else
                Session("Serverfiles") = table
            End If
        Else
            Session("Serverfiles") = Nothing
        End If
    End Sub

    Private Sub setSelectedOrderValue(ByVal item As DataGridItem)
        Dim cell As TableCell
        Dim ctl As Control
        Dim tbox As TextBox
        Dim cbx As CheckBox
        Dim ddl As DropDownList
        Dim lbtn As LinkButton
        Dim lbl As Label
        Dim ret As String
        Dim retInfo As String
        Dim tmpRows As DataRow()
        Dim tmpRow As DataRow
        Dim table As DataTable

        ret = String.Empty
        retInfo = String.Empty


        table = Session("dtTable")



        For Each cell In item.Cells
            For Each ctl In cell.Controls
                If TypeOf ctl Is DropDownList Then
                    ddl = CType(ctl, DropDownList)
                    If ddl.ID = "ddlOrders" Then
                        ret = ddl.SelectedItem.Value
                        retInfo = ddl.SelectedItem.Text
                        Dim i As Integer = ddl.SelectedIndex
                        Session.Add("AppUeberOrderIndex", i)

                        ddl = CType(item.FindControl("drpKategorie"), DropDownList)

                        ddl.Items.Clear()
                        Dim sTemp As String

                        sTemp = Right(retInfo, retInfo.Length - retInfo.LastIndexOf(":") - 1)
                        sTemp = Left(sTemp, sTemp.IndexOf("."))

                        sTemp = "And Fahrtnr = '" & sTemp & "'"
                        tmpRows = table.Select("AUFNR = '" & Right("0000000000" & Left(ret, ret.IndexOf(".")), 10) & "'" & sTemp)

                        ddl.Items.Add(New ListItem("Auswahl", 0))

                        For Each tmpRow In tmpRows

                            If tmpRow.Item("ZZPROTKAT1") <> String.Empty Then
                                ddl.Items.Add(New ListItem(tmpRow.Item("ZZPROTKAT1"), 1))
                            End If

                            If tmpRow.Item("ZZPROTKAT2") <> String.Empty Then
                                ddl.Items.Add(New ListItem(tmpRow.Item("ZZPROTKAT2"), 2))
                            End If

                            If tmpRow.Item("ZZPROTKAT3") <> String.Empty Then
                                ddl.Items.Add(New ListItem(tmpRow.Item("ZZPROTKAT3"), 3))
                            End If

                            Exit For
                        Next

                        ddl.Visible = True
                        ddl = CType(item.FindControl("ddlOrders"), DropDownList)
                        retInfo = ddl.SelectedItem.Text
                        ddl.SelectedIndex = i
                        ddl.Visible = False
                    End If

                End If
            Next
        Next
        'Wert setzen
        For Each cell In item.Cells
            For Each ctl In cell.Controls
                If TypeOf ctl Is TextBox Then
                    tbox = CType(ctl, TextBox)
                    If (tbox.ID = "txtAuftrag") Then ' And (auftrag = String.Empty) Then
                        tbox.Text = Left(ret, ret.IndexOf("."))
                        tbox.Enabled = False
                    End If
                    If (tbox.ID = "txtReferenz") Then ' And (referenz = String.Empty) Then

                        tbox.Text = Right(ret, ret.Length - ret.LastIndexOf(".") - 1)
                        tbox.Text = Left(tbox.Text, tbox.Text.IndexOf(":"))


                        tbox.Enabled = False
                    End If
                End If
                If TypeOf ctl Is Label Then     'Infofeld füllen
                    lbl = CType(ctl, Label)
                    If lbl.ID = "lblData" Then
                        lbl.Text = retInfo
                        lbl.Visible = True
                    End If
                End If
            Next
        Next

        'Zuordnungsbutton deaktivieren...
        For Each cell In item.Cells
            For Each ctl In cell.Controls
                If TypeOf ctl Is LinkButton Then
                    lbtn = CType(ctl, LinkButton)
                    'If (lbtn.ID = "btnAssignOrder") Then
                    '    lbtn.Visible = False
                    'End If
                    If (lbtn.ID = "btnReAssignOrder") Then
                        lbtn.Visible = True
                    End If
                    If (lbtn.ID = "btnSuche") Then
                        lbtn.Visible = False
                    End If
                End If
            Next
        Next
        'Checkbox für Archivierung setzen...
        For Each cell In item.Cells
            For Each ctl In cell.Controls
                If TypeOf ctl Is CheckBox Then
                    cbx = CType(ctl, CheckBox)
                    If cbx.ID = "cbxArchiv" Then
                        cbx.Checked = True
                    End If
                End If
            Next
        Next
        'Datum eintragen
        For Each cell In item.Cells
            For Each ctl In cell.Controls
                If TypeOf ctl Is TextBox Then
                    tbox = CType(ctl, TextBox)
                    If tbox.ID = "txtAbgabedatum" Then
                        '#1034 - Bardenhagen - Soll jetzt leer sein
                        tbox.Text = "" 'Now.Subtract(New TimeSpan(1, 0, 0, 0)).ToShortDateString
                        tbox.Visible = True
                    End If
                End If

            Next
        Next
        'Label sichtbar machen...
        For Each cell In item.Cells
            For Each ctl In cell.Controls
                If TypeOf ctl Is Label Then
                    lbl = CType(ctl, Label)
                    If lbl.ID = "lblAbgabedatum" Then
                        lbl.Visible = True
                    ElseIf lbl.ID = "lblKategorie" Then
                        lbl.Visible = True
                    End If
                End If
            Next
        Next
    End Sub

    Private Sub undoSetSelectedOrderValue(ByVal item As DataGridItem)
        Dim cell As TableCell
        Dim ctl As Control
        Dim tbox As TextBox
        Dim lbtn As LinkButton
        Dim ddl As DropDownList
        Dim cbx As CheckBox
        Dim lbl As Label

        'Wert setzen
        For Each cell In item.Cells
            For Each ctl In cell.Controls
                If TypeOf ctl Is TextBox Then
                    tbox = CType(ctl, TextBox)
                    If (tbox.ID = "txtAuftrag") Then ' And (auftrag = String.Empty) Then
                        tbox.Text = String.Empty
                        tbox.Enabled = True
                    End If
                    If (tbox.ID = "txtReferenz") Then ' And (referenz = String.Empty) Then
                        tbox.Text = String.Empty
                        tbox.Enabled = True
                    End If
                End If
            Next
        Next
        'Zuordnungsbutton aktivieren...
        For Each cell In item.Cells
            For Each ctl In cell.Controls
                If TypeOf ctl Is LinkButton Then
                    lbtn = CType(ctl, LinkButton)
                    If (lbtn.ID = "btnAssignOrder") Then
                        lbtn.Visible = True
                    End If
                    If (lbtn.ID = "btnReAssignOrder") Then
                        lbtn.Visible = False
                    End If
                    If (lbtn.ID = "btnSuche") Then
                        lbtn.Visible = True
                    End If
                End If
            Next
        Next
        'Checkbox für Archivierung...
        For Each cell In item.Cells
            For Each ctl In cell.Controls
                If TypeOf ctl Is CheckBox Then
                    cbx = CType(ctl, CheckBox)
                    If cbx.ID = "cbxArchiv" Then
                        cbx.Checked = False
                    End If
                End If
            Next
        Next
        'Datum löschen
        For Each cell In item.Cells
            For Each ctl In cell.Controls
                If TypeOf ctl Is TextBox Then
                    tbox = CType(ctl, TextBox)
                    If tbox.ID = "txtAbgabedatum" Then
                        tbox.Text = String.Empty
                        tbox.Visible = False
                    End If
                End If
            Next
        Next
        'Label unsichtbar machen...
        For Each cell In item.Cells
            For Each ctl In cell.Controls
                If TypeOf ctl Is Label Then
                    lbl = CType(ctl, Label)
                    If lbl.ID = "lblAbgabedatum" Then
                        lbl.Visible = False
                    ElseIf lbl.ID = "lblKategorie" Then
                        lbl.Visible = False
                    ElseIf lbl.ID = "lblInfo" Then
                        lbl.Text = String.Empty
                        lbl.Visible = False
                    ElseIf lbl.ID = "lblData" Then
                        lbl.Text = String.Empty
                        lbl.Visible = False
                    End If

                End If
            Next
        Next

        'Dropdown unsichtbar machen oder sichtbar...
        For Each cell In item.Cells
            For Each ctl In cell.Controls
                If TypeOf ctl Is DropDownList Then
                    ddl = CType(ctl, DropDownList)
                    If ddl.ID = "drpKategorie" Then
                        ddl.Visible = False
                    End If
                    If ddl.ID = "ddlOrder" Then
                        ddl.Visible = True
                    End If
                End If
            Next
        Next

    End Sub

    Private Sub getInputValues(ByVal item As DataGridItem, ByRef auftrag As String, ByRef referenz As String, ByRef tour As String, Optional ByRef Kategorie As String = "Auswahl")
        'Inhalt des Referenz-Textfeldes abfragen...

        Dim cell As TableCell
        Dim ctl As Control
        Dim tbox As TextBox
        Dim ddl As DropDownList
        Dim ret As String
        Dim lbl As Label

        ret = String.Empty

        For Each cell In item.Cells
            For Each ctl In cell.Controls
                If TypeOf ctl Is TextBox Then
                    tbox = CType(ctl, TextBox)
                    If tbox.ID = "txtReferenz" Then
                        referenz = tbox.Text
                    End If
                    If tbox.ID = "txtAuftrag" Then
                        auftrag = tbox.Text
                    End If
                End If
                If TypeOf ctl Is DropDownList Then
                    ddl = CType(ctl, DropDownList)
                    If ddl.ID = "drpKategorie" Then

                        If IsNothing(ddl.SelectedItem) = False Then
                            Kategorie = ddl.SelectedItem.Text
                        End If

                    End If
                End If
                If TypeOf ctl Is Label Then
                    lbl = CType(ctl, Label)
                    If (lbl.ID = "lblData") Then
                        tour = Left(Right(lbl.Text, lbl.Text.Length - lbl.Text.IndexOf(":") - 1), 1)
                    End If
                End If

            Next
        Next
    End Sub

    Private Sub formatListTable(ByRef table As DataTable)
        Dim row As DataRow
        table.Columns.Add("Info", System.Type.GetType("System.String"))
        table.Columns.Add("Key", System.Type.GetType("System.String"))

        For Each row In table.Rows
            row("Info") = CType(row("AUFNR"), String).TrimStart("0c") & "." & row("ZZREFNR") & ":" & row("FAHRTNR") & "." & CType(row("FAHRER"), String).TrimStart("0"c) & "." & row("FAHRTVON") & "->" & row("FAHRTNACH") & "." & row("ZZKENN") & "." & row("ZZBEZEI") & "." & row("NAME1") & "." & row("VKORG") & "~"
            row("Key") = CType(row("AUFNR"), String).TrimStart("0"c) & "." & row("FAHRTNR") & "." & row("ZZREFNR") & ":" & row("ZZKUNNR")
            table.AcceptChanges()
        Next
    End Sub

    Private Sub setAuftragsListe(ByVal item As DataGridItem, ByVal table As DataTable)
        'Dropdown-Liste mit Auftragsnummern füllen... + Tournummer!!!

        Dim cell As TableCell
        Dim ctl As Control
        Dim ddl As New DropDownList
        Dim cbx As CheckBox
        Dim tbox As TextBox
        Dim lbtn As LinkButton
        Dim lbl As Label
        Dim ret As String
        Dim ddlKat As DropDownList
        Dim tmpRows As DataRow()
        Dim tmpRow As DataRow
        Dim KatTable As DataTable


        KatTable = Session("dtTable")



        'ToDo: TourNummer!!
        ret = String.Empty

        For Each cell In item.Cells
            For Each ctl In cell.Controls
                If TypeOf ctl Is DropDownList Then
                    ddl = CType(ctl, DropDownList)
                    If ddl.ID = "ddlOrders" Then
                        ddl.DataSource = table
                        ddl.DataTextField = "Info"
                        ddl.DataValueField = "Key"
                        ddl.DataBind()
                        If (ddl.Items.Count >= 1) Then
                            ddl.Visible = True
                        End If
                        If (ddl.Items.Count = 1) Then
                            ddl.Enabled = False
                        End If
                    End If



                End If
            Next
        Next
        If ddl.Items.Count = 1 Then     'Wenn nur ein Auftrag gefunden wurde, Nr. sofort übernehmen...
            For Each cell In item.Cells
                For Each ctl In cell.Controls
                    If TypeOf ctl Is TextBox Then
                        tbox = CType(ctl, TextBox)
                        If (tbox.ID = "txtAuftrag") Then ' And (auftrag = String.Empty) Then
                            tbox.Text = Left(ddl.Items(0).Value, ddl.Items(0).Value.IndexOf("."))
                            tbox.Enabled = False
                        End If
                        If (tbox.ID = "txtReferenz") Then ' And (referenz = String.Empty) Then
                            tbox.Text = Right(ddl.Items(0).Value, ddl.Items(0).Value.Length - ddl.Items(0).Value.LastIndexOf(".") - 1)
                            tbox.Text = Left(tbox.Text, tbox.Text.IndexOf(":"))
                            tbox.Enabled = False
                        End If
                        'Datum eintragen
                        If tbox.ID = "txtAbgabedatum" Then
                            '#1034 - Bardenhagen - Soll jetzt leer sein
                            tbox.Text = "" ' Now.Subtract(New TimeSpan(1, 0, 0, 0)).ToShortDateString
                            tbox.Visible = True
                        End If
                    End If
                    'Infofeld setzen
                    If TypeOf ctl Is Label Then     'Infofeld füllen
                        lbl = CType(ctl, Label)
                        If lbl.ID = "lblInfo" Then
                            'lbl.Text = ddl.SelectedItem.Text
                        End If
                        'Datafeld füllen
                        If lbl.ID = "lblData" Then
                            lbl.Text = ddl.SelectedItem.Text
                        End If
                        'Label sichtbar machen...(Abgabedatum)
                        If lbl.ID = "lblAbgabedatum" Then
                            lbl.Visible = True
                        End If
                    End If
                    'Checkbox für Archivierung setzen...

                    If TypeOf ctl Is CheckBox Then
                        cbx = CType(ctl, CheckBox)
                        If cbx.ID = "cbxArchiv" Then
                            cbx.Checked = True
                        End If
                    End If
                Next
            Next
        End If
        'Zuordnungsbutton aktivieren...
        For Each cell In item.Cells
            For Each ctl In cell.Controls
                If TypeOf ctl Is LinkButton Then
                    lbtn = CType(ctl, LinkButton)
                    If (lbtn.ID = "btnAssignOrder") AndAlso (ddl.Items.Count > 1) Then
                        lbtn.Visible = True
                    End If
                    If (lbtn.ID = "btnKategorie") AndAlso (ddl.Items.Count <= 1) Then
                        lbtn.Visible = True
                        ddlKat = CType(item.FindControl("drpKategorie"), DropDownList)

                        ddlKat.Items.Clear()

                        ret = ddl.SelectedItem.Value


                        tmpRows = KatTable.Select("AUFNR = '" & Right("0000000000" & Left(ret, ret.IndexOf(".")), 10) & "'")

                        ddlKat.Items.Add(New ListItem("Auswahl", 0))

                        For Each tmpRow In tmpRows

                            If tmpRow.Item("ZZPROTKAT1") <> String.Empty Then
                                ddlKat.Items.Add(New ListItem(tmpRow.Item("ZZPROTKAT1"), 1))
                            End If

                            If tmpRow.Item("ZZPROTKAT2") <> String.Empty Then
                                ddlKat.Items.Add(New ListItem(tmpRow.Item("ZZPROTKAT2"), 2))
                            End If

                            If tmpRow.Item("ZZPROTKAT3") <> String.Empty Then
                                ddlKat.Items.Add(New ListItem(tmpRow.Item("ZZPROTKAT3"), 3))
                            End If

                            Exit For
                        Next


                        ddlKat.Visible = True
                    End If

                    If (lbtn.ID = "btnReAssignOrder") Then
                        lbtn.Visible = True
                    End If
                End If
            Next
        Next
    End Sub

    Private Function getAuftragsListe(ByVal auftrag As String, ByVal referenz As String, ByRef status As String) As DataTable
        Dim uebf As Ueberfuehrung
        Dim table As DataTable

        table = Nothing

        uebf = New Ueberfuehrung(m_User, m_App, Session("AppID").ToString, Session.SessionID.ToString, "")
        uebf.getAuftragsDatenByReferenz(referenz, auftrag)
        If uebf.Message <> String.Empty Then
            status = uebf.Message
        Else
            If (uebf.Result Is Nothing) OrElse (uebf.Result.Rows.Count = 0) Then
                status = "Keine Daten gefunden."
            Else
                status = String.Empty
                table = uebf.Result

                Session.Add("dtTable", table)


            End If
        End If

        Return table
    End Function

    Private Sub updateView(ByVal mode As Boolean, ByVal modeddl As Boolean)
        Dim fname As String
        Dim table As DataTable
        Dim rows() As DataRow
        Dim rowToUpdate As DataRow

        Dim item As DataGridItem
        Dim cell As TableCell
        Dim ctl As Control
        Dim lbl As Label
        Dim ddl As DropDownList
        Dim btn As LinkButton
        Dim ibtn As ImageButton
        Dim cbox As CheckBox
        Dim tbox As TextBox
        Dim toSave As Boolean

        Dim auftrag As String = ""
        Dim referenz As String = ""
        Dim kategorie As String = ""
        'Dim fahrer As String
        Dim info As String = String.Empty
        Dim abgabedatum As String = ""

        table = CType(Session("Serverfiles"), DataTable)

        'DataGrid durchackern
        For Each item In gridServer.Items

            fname = item.Cells(4).Text
            rows = table.Select("Filename = '" & fname & "'")

            If (rows.Length > 0) Then
                rowToUpdate = rows(0)
                auftrag = rowToUpdate("Auftrag").ToString()
                info = rowToUpdate("Info").ToString()
                If info <> String.Empty Then
                    referenz = Left(info, info.IndexOf(":"))
                    referenz = Right(referenz, referenz.Length - referenz.IndexOf(".") - 1)
                End If
                '§§§ JVE 20.12.2005
                If rowToUpdate("Abgabedatum") Is System.DBNull.Value Then
                    abgabedatum = String.Empty
                Else
                    abgabedatum = rowToUpdate("Abgabedatum")
                End If


                If rowToUpdate("Kategorie") Is System.DBNull.Value Then
                    kategorie = String.Empty
                Else
                    kategorie = rowToUpdate("Kategorie")
                End If


            End If

            toSave = False
            For Each cell In item.Cells
                For Each ctl In cell.Controls
                    If TypeOf ctl Is CheckBox Then
                        cbox = CType(ctl, CheckBox)
                        If (cbox.ID = "cbxArchiv") Then
                            cbox.Checked = True
                        End If
                    End If
                Next
            Next
            'Buttons, Infozeile, Dropdownlist aus/einblenden
            For Each cell In item.Cells
                For Each ctl In cell.Controls
                    If TypeOf ctl Is Label Then
                        lbl = CType(ctl, Label)
                        If (lbl.ID = "lblInfo") Then
                            lbl.Visible = mode 'True
                            lbl.Text = info
                        End If
                        '§§§ JVE 20.12.2005 <begin>
                        If (lbl.ID = "lblAbgabedatum") AndAlso (abgabedatum <> String.Empty) Then
                            lbl.Visible = True 'True
                        End If

                        If (lbl.ID = "lblKategorie") AndAlso (abgabedatum <> String.Empty) Then
                            lbl.Visible = True 'True
                        End If

                        '§§§ JVE 20.12.2005 <end>
                    End If
                    If TypeOf ctl Is TextBox Then
                        tbox = CType(ctl, TextBox)
                        If (tbox.ID = "txtAuftrag") Then
                            tbox.Text = auftrag
                            tbox.Enabled = Not mode ' False
                        End If

                        '§§§ JVE 20.12.2005 <begin>
                        If (tbox.ID = "txtAbgabedatum") Then
                            tbox.Text = abgabedatum
                            If abgabedatum <> String.Empty Then
                                tbox.Visible = True
                                '§§§ JVE 16.01.2005
                                tbox.Enabled = Not mode
                            End If
                        End If
                        '§§§ JVE 20.12.2005 <end>
                    End If
                    If TypeOf ctl Is TextBox Then
                        tbox = CType(ctl, TextBox)
                        If (tbox.ID = "txtReferenz") Then
                            tbox.Text = referenz
                            tbox.Enabled = Not mode 'False
                        End If
                        If (tbox.ID = "txtKategorie") Then

                            tbox.Text = kategorie
                            tbox.Visible = True
                        End If


                    End If
                    If TypeOf ctl Is DropDownList Then
                        ddl = CType(ctl, DropDownList)
                        If (ddl.ID = "ddlOrders") Then
                            ddl.Visible = modeddl  'False
                        End If
                    End If
                    If TypeOf ctl Is LinkButton Then
                        btn = CType(ctl, LinkButton)
                        If (btn.ID = "btnSuche") Then
                            btn.Visible = Not mode 'False
                        End If
                    End If
                    If TypeOf ctl Is ImageButton Then
                        ibtn = CType(ctl, ImageButton)
                        If (ibtn.ID = "ibtnSRDelete") Then
                            ibtn.Visible = Not mode 'False
                        End If
                    End If
                Next
            Next
        Next
    End Sub

    Private Sub updateTable(ByVal ignoreErrors As Boolean)
        Dim fname As String
        Dim table As DataTable
        Dim rows() As DataRow
        Dim rowToUpdate As DataRow

        Dim item As DataGridItem
        Dim cell As TableCell
        Dim ctl As Control
        Dim lbl As Label
        Dim cbox As CheckBox
        Dim ddlListe As DropDownList
        Dim tbox As TextBox
        Dim toSave As Boolean

        Dim auftrag As String = ""
        Dim tour As String = ""
        Dim fahrer As String = ""
        Dim info As String = ""
        Dim abgabedatum As String = ""
        Dim kunnr As String = ""
        Dim kategorie As String = ""
        Dim kategorieValue As String = ""
        Dim Vkorg As String = ""

        table = CType(Session("Serverfiles"), DataTable)

        booErr = False
        lblError.Text = String.Empty

        'DataGrid durchackern
        For Each item In gridServer.Items
            toSave = False
            For Each cell In item.Cells
                For Each ctl In cell.Controls
                    If TypeOf ctl Is CheckBox Then
                        cbox = CType(ctl, CheckBox)
                        If (cbox.ID = "cbxArchiv") AndAlso (cbox.Checked = True) Then
                            toSave = True
                        End If
                    End If
                    '§§§ JVE 20.12.2005 Abgabedatum speichern...
                    If TypeOf ctl Is TextBox Then
                        tbox = CType(ctl, TextBox)
                        If (tbox.ID = "txtAbgabedatum") Then
                            '§§§ JVE 16.01.2005 Bugfix
                            If (tbox.Text.Trim = String.Empty) Then
                                abgabedatum = String.Empty
                            Else
                                '-------------------
                                If (tbox.Text <> String.Empty) AndAlso IsDate(tbox.Text) Then

                                    If tbox.Text.Length <> 10 Then
                                        booErr = True
                                        lblError.Text = "Bitte geben Sie das Abgabedatum im Format tt.mm.jjjj ein."
                                    Else
                                        abgabedatum = tbox.Text
                                    End If
                                Else
                                    booErr = True
                                    lblError.Text = "Ungültige Eingabe in Abgabedatum."
                                    Exit For
                                End If
                            End If
                        End If

                        If (tbox.ID = "txtKategorie") Then
                            tbox.Visible = True
                        End If

                    End If

                    If TypeOf ctl Is DropDownList Then
                        ddlListe = CType(ctl, DropDownList)

                        If (ddlListe.ID = "drpKategorie") Then

                            If ddlListe.Items.Count > 0 Then
                                If ddlListe.SelectedItem.Value = "0" Then
                                    cbox = cell.FindControl("cbxArchiv")

                                    If Not ignoreErrors AndAlso cbox.Checked = True Then
                                        Dim ListtoDel As System.Collections.ArrayList = Session("AppDCLDel")

                                        If Not IsNothing(ListtoDel) Then
                                            Dim stemp As String = item.Cells(4).Text
                                            If Not ListtoDel.Contains(stemp) Then
                                                booErr = True
                                                lblError.Text = "Bitte wählen Sie eine Kategorie aus."
                                                Exit For
                                            End If
                                        Else
                                            booErr = True
                                            lblError.Text = "Bitte wählen Sie eine Kategorie aus."
                                            Exit For
                                        End If

                                        Exit Sub
                                    End If

                                Else
                                    kategorie = ddlListe.SelectedItem.Text
                                    kategorieValue = ddlListe.SelectedItem.Value
                                End If
                            End If
                        End If
                    End If



                Next
            Next

            If toSave Then  'Archivieren? Ja, weitere Daten holen..
                For Each cell In item.Cells
                    For Each ctl In cell.Controls
                        If TypeOf ctl Is Label Then
                            lbl = CType(ctl, Label)
                            If (lbl.ID = "lblData") Then
                                'If (lbl.ID = "lblInfo") 
                                info = lbl.Text
                                auftrag = Left(lbl.Text, lbl.Text.IndexOf("."))          'Auftragsnummer
                                tour = Left(Right(lbl.Text, lbl.Text.Length - lbl.Text.IndexOf(":") - 1), 1)
                                fahrer = Right(lbl.Text, lbl.Text.Length - lbl.Text.IndexOf(":") - 1)
                                fahrer = Right(fahrer, fahrer.Length - fahrer.IndexOf(".") - 1)
                                fahrer = Left(fahrer, fahrer.IndexOf("."))
                                Vkorg = Left(lbl.Text, lbl.Text.IndexOf("~"))
                                Vkorg = Mid(Vkorg, Vkorg.LastIndexOf(".") + 2)
                            End If
                            If (lbl.ID = "lblKategorie") Then
                                lbl.Visible = True
                            End If

                        End If

                        If TypeOf ctl Is DropDownList Then
                            ddlListe = CType(ctl, DropDownList)
                            If ddlListe.ID = "ddlOrders" Then
                                kunnr = ddlListe.SelectedItem.Value
                                kunnr = Right(kunnr, kunnr.Length - kunnr.LastIndexOf(":") - 1)
                            End If
                        End If
                    Next
                Next
                fname = item.Cells(4).Text
                rows = table.Select("Filename = '" & fname & "'")
                If (rows.Length > 0) Then
                    rowToUpdate = rows(0)
                    rowToUpdate("Save") = "X"
                    rowToUpdate("Auftrag") = auftrag
                    rowToUpdate("Tour") = tour
                    rowToUpdate("Fahrer") = fahrer
                    rowToUpdate("Info") = info
                    '§§§ JVE 20.12.2005
                    rowToUpdate("Abgabedatum") = abgabedatum

                    rowToUpdate("Kunnr") = kunnr

                    rowToUpdate("kategorie") = kategorie
                    rowToUpdate("kategorieValue") = kategorieValue

                    rowToUpdate("VKORG") = Vkorg


                    table.AcceptChanges()
                End If
            End If
        Next
        Session("Serverfiles") = table
    End Sub


    Private Sub CheckDrpKategorie()
        Dim table As DataTable
        Dim rows() As DataRow
        Dim rowToUpdate As DataRow
        Dim fname As String

        Dim item As DataGridItem
        Dim cell As TableCell
        Dim ctl As Control
        Dim ddlListe As DropDownList

        Dim kategorie As String = String.Empty

        table = CType(Session("Serverfiles"), DataTable)


        'DataGrid durchackern
        For Each item In gridServer.Items
            For Each cell In item.Cells
                For Each ctl In cell.Controls

                    If TypeOf ctl Is DropDownList Then
                        ddlListe = CType(ctl, DropDownList)
                        If (ddlListe.ID = "drpKategorie") Then
                            If ddlListe.Items.Count > 0 Then
                                kategorie = ddlListe.SelectedItem.Text

                            End If

                        End If
                    End If
                Next
            Next

            fname = item.Cells(4).Text
            rows = table.Select("Filename = '" & fname & "'")

            If (rows.Length > 0) Then
                rowToUpdate = rows(0)
                If kategorie <> String.Empty Then
                    rowToUpdate("kategorie") = kategorie

                    table.AcceptChanges()

                End If

            End If
        Next
        Session("Serverfiles") = table
    End Sub


    Private Sub gridServer_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles gridServer.ItemCommand
        Dim filename As String
        Dim file As System.IO.FileInfo
        Dim table As DataTable
        Dim status As String
        Dim referenz As String = ""
        Dim auftrag As String
        Dim tour As String
        Dim tbl As DataTable
        Dim item As DataGridItem
        Dim kategorie As String
        Dim ddl As DropDownList
        Dim ddlOrders As DropDownList
        Dim txtBox As TextBox
        Dim btn As LinkButton
        Dim btnAssignOrder As LinkButton
        Dim lbl As Label

        'Ausgewählen Wert der Kategorie  zuweisen
        'CheckDrpKategorie()

        table = CType(Session("Serverfiles"), DataTable)


        ddl = CType(e.Item.FindControl("drpKategorie"), DropDownList)
        ddlOrders = CType(e.Item.FindControl("ddlOrders"), DropDownList)
        txtBox = CType(e.Item.FindControl("txtKategorie"), TextBox)
        btn = CType(e.Item.FindControl("btnKategorie"), LinkButton)
        btnAssignOrder = CType(e.Item.FindControl("btnAssignOrder"), LinkButton)
        lbl = CType(e.Item.FindControl("lblInfo"), Label)

        'status = String.Empty
        lblError.Text = String.Empty

        If e.CommandName = "Delete" Then
            filename = ConfigurationManager.AppSettings("UploadpathLocalProtokoll") & e.Item.Cells(4).Text
            'filename = e.Item.Cells(4).Text
            file = New FileInfo(filename)

            Try
                If file.Exists Then
                    file.Delete()

                    '# 1003 - 02.05.07 TB
                    'Eintrag aus Tabelle entfernen anstatt neuladen
                    table.Rows.Find(e.Item.Cells(4).Text).Delete()
                    Dim ListtoDel As New ArrayList()
                    ListtoDel.Add(e.Item.Cells(4).Text)
                    e.Item.Visible = False
                    SetDatagridItemItemStyle()

                    If table.Rows.Count = 0 Then
                        gridServer.Visible = False
                        lblNoData.Text = "Keine Daten zur Anzeige gefunden."
                        lblNoData.Visible = True
                    End If
                    Session("Serverfiles") = table
                    Session.Add("AppDCLDel", ListtoDel)
                Else
                    lblError.Text = "Datei nicht gefunden."
                End If
            Catch ex As Exception
                lblError.Text = "Fehler beim Löschen der Datei."
            End Try
        End If
        'Referenz-Nr. + Auftragsnummer verarbeiten (Suche)
        If e.CommandName = "searchSAP" Then


            auftrag = String.Empty
            referenz = String.Empty
            getInputValues(e.Item, auftrag, referenz, String.Empty)

            If (auftrag.Trim <> String.Empty And referenz.Trim <> String.Empty) Or (auftrag.Trim = String.Empty And referenz.Trim = String.Empty) Then
                lblError.Text = "Bitte Auftrags-Nr. ODER Referenz eingeben."
            Else
                status = String.Empty
                tbl = getAuftragsListe(auftrag, referenz, status)

                If (status <> String.Empty) Then
                    lblError.Text = status
                Else
                    formatListTable(tbl)
                    setAuftragsListe(e.Item, tbl)

                    lblError.Text = String.Empty
                End If
            End If


        End If

        If e.CommandName = "KategorieZuordnen" Then


            auftrag = String.Empty
            kategorie = String.Empty
            tour = String.Empty

            getInputValues(e.Item, auftrag, referenz, tour, kategorie)


            Dim strCompAuftrag As String
            Dim strCompKategorie As String
            Dim strCompTour As String
            Dim booKategorieErr As Boolean

            If ddl.SelectedItem.Text = "Auswahl" Then
                lbl.Text = "Bitte wählen Sie eine Kategorie aus."
                lbl.ForeColor = Color.Red
                lbl.Visible = True
            Else

                For Each item In gridServer.Items

                    strCompAuftrag = String.Empty
                    strCompKategorie = String.Empty
                    strCompTour = String.Empty

                    If Not item Is e.Item Then

                        getInputValues(item, strCompAuftrag, referenz, strCompTour, strCompKategorie)

                        Dim stemp As String = item.Cells(4).Text
                        If strCompAuftrag = auftrag And strCompKategorie = kategorie And strCompTour = tour Then
                            Dim ListtoDel As ArrayList = Session("AppDCLDel")
                            If Not IsNothing(ListtoDel) Then
                                If Not ListtoDel.Contains(stemp) Then
                                    lbl.Text = "Auswahl zurückgesetzt: Die Kategorie '" & kategorie & "' wurde bereits zugeordnet."
                                    lbl.ForeColor = Color.Red
                                    lbl.Visible = True
                                    booKategorieErr = True
                                    Exit For
                                End If
                            Else
                                lbl.Text = "Auswahl zurückgesetzt: Die Kategorie '" & kategorie & "' wurde bereits zugeordnet."
                                lbl.ForeColor = Color.Red
                                lbl.Visible = True
                                booKategorieErr = True
                                Exit For
                            End If

                        End If


                    End If

                Next

                If booKategorieErr = False Then
                    txtBox.Text = ddl.SelectedItem.Text
                    ddl.Visible = False
                    txtBox.Visible = True
                    lbl.Visible = False
                    btn.Visible = False
                End If

            End If


        End If


        'Auftrag zuweisen...
        If e.CommandName = "assignOrder" Then
            setSelectedOrderValue(e.Item)
            btn.Visible = True
            'updateTable(e.Item)
        End If
        'Zuweisung aufheben...
        If e.CommandName = "unassignOrder" Then
            undoSetSelectedOrderValue(e.Item)
            txtBox.Text = String.Empty
            ddl.Visible = False
            ddlOrders.Items.Clear()
            ddlOrders.Visible = False
            btnAssignOrder.Visible = False
            txtBox.Visible = False
            btn.Visible = False
        End If



        '--------
        '#1003 03.05.07 TB - Festlegen, dass zu diesem Item gescrollt werden soll
        '--------
        Dim sh As New Controls.ScrollHereControl()
        'TableRow -> TableCell -> ScrollHereControl
        If e.CommandName = "Delete" AndAlso table.Rows.Count > 0 AndAlso e.Item.ItemIndex > 0 Then
            'Auf vorheriges Element scrollen
            gridServer.Items(e.Item.ItemIndex - 1).FindControl("lblDummyForScrollHere").Controls.AddAt(0, sh)
        Else
            'Auf aktuelle Element scrollen
            e.Item.FindControl("lblDummyForScrollHere").Controls.AddAt(0, sh)
        End If

    End Sub

    Private Sub Linkbutton1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Linkbutton1.Click
        Dim status As String = ""
        updateTable(False)

        If booErr = True Then Exit Sub

        FillGridServer(0, status, , , True)
        If (status <> String.Empty) Then
            lblError.Text = status
        Else
            updateView(True, False)
            btnUpload.Visible = True
            Linkbutton1.Visible = False
            btnBack.Visible = True
        End If
    End Sub

    Private Sub btnBack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBack.Click
        btnBack.Visible = False
        btnUpload.Visible = False
        Linkbutton1.Visible = True
        updateView(False, False)
    End Sub

    Private Sub btnUpload_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnUpload.Click
        Dim table As DataTable
        Dim status As String = ""
        Dim uebf As New Ueberfuehrung(m_User, m_App, Session("AppID").ToString, Session.SessionID.ToString, "")

        If (Session("Serverfiles") Is Nothing) Then
            lblError.Text = "Keine Dateien vorhanden."
        Else
            table = Session("Serverfiles")

            'If Not m_User.IsTestUser Then   '§§§ JVE 16.01.2005: Nur für PROD-User! 
            uebf.renameFilesProtocol(table, status, m_User.IsTestUser)          'Dateien umbenennen und transferieren
            'End If
            If (status = String.Empty) Then
                If Not m_User.IsTestUser Then   '§§§ JVE 16.01.2005: Nur für PROD-User!
                    'Ueberfuehrung.writeFileForImport(table, status)       'Importdatei erzeugen/erweitern...
                End If
                If (status = String.Empty) Then
                    uebf.writeSAPProtocol(Session("AppID").ToString, Session.SessionID, table, status)
                    Session("Serverfiles") = table              'Evtl. Fehlermeldungen zurückholen

                    FillGridServer(0, , , , True)
                End If
            Else
                lblError.Text = status
            End If

            If (status <> String.Empty) Then
                lblError.Text = status
            Else
                Dim row As DataRow
                Dim str As String = ""

                table = CType(Session("Serverfiles"), DataTable)
                For Each row In table.Rows
                    If (row("Save").ToString = "X") Then
                        str &= row("Status") & ";"
                    End If
                Next
                str = str.Replace("\", "'")
                Session("Status") = str
                lblOpen.Text = "<script language=""Javascript"">window.open(""_Report022.aspx?USER=" & m_User.UserName &
                    "&PAR=" & str & """, ""Übertragungsprotokoll"", ""width=700,height=480,left=0,top=0,scrollbars=YES"");location.replace(""/Portal/(S(" &
                    Session.SessionID.ToString & "))/Applications/AppDCL/Forms/_Report03.aspx?AppID=" & Session("AppID").ToString & """);</script>"
            End If
        End If
    End Sub

    Private Sub SetDatagridItemItemStyle()

        Dim item As DataGridItem
        Dim alt As Boolean = False
        For Each item In gridServer.Items
            If item.Visible Then
                If alt Then
                    item.CssClass = gridServer.AlternatingItemStyle.CssClass
                Else
                    item.CssClass = gridServer.ItemStyle.CssClass
                End If
                alt = Not alt
            End If
        Next

    End Sub

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        SetEndASPXAccess(Me)
    End Sub

    Private Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Unload
        SetEndASPXAccess(Me)
    End Sub

End Class


' ************************************************
' $History: _Report03.aspx.vb $
' 
' *****************  Version 8  *****************
' User: Rudolpho     Date: 23.09.10   Time: 15:33
' Updated in $/CKAG/Applications/appdcl/Forms
' 
' *****************  Version 7  *****************
' User: Rudolpho     Date: 22.10.09   Time: 14:37
' Updated in $/CKAG/Applications/appdcl/Forms
' 
' *****************  Version 6  *****************
' User: Rudolpho     Date: 23.06.09   Time: 16:29
' Updated in $/CKAG/Applications/appdcl/Forms
' ITA: 2885
' 
' *****************  Version 5  *****************
' User: Rudolpho     Date: 30.04.09   Time: 9:25
' Updated in $/CKAG/Applications/appdcl/Forms
' ITA: 2837
' 
' *****************  Version 4  *****************
' User: Fassbenders  Date: 21.04.09   Time: 17:24
' Updated in $/CKAG/Applications/appdcl/Forms
' ITA: 2807
' 
' *****************  Version 3  *****************
' User: Jungj        Date: 2.12.08    Time: 14:52
' Updated in $/CKAG/Applications/appdcl/Forms
' ITA 2377 testfertig
' 
' *****************  Version 2  *****************
' User: Fassbenders  Date: 15.05.08   Time: 14:54
' Updated in $/CKAG/Applications/appdcl/Forms
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 7.04.08    Time: 13:00
' Created in $/CKAG/Applications/appdcl/Forms
' 
' *****************  Version 20  *****************
' User: Fassbenders  Date: 13.03.08   Time: 10:39
' Updated in $/CKG/Applications/AppDCL/AppDCLWeb/Forms
' 
' *****************  Version 19  *****************
' User: Fassbenders  Date: 21.02.08   Time: 10:06
' Updated in $/CKG/Applications/AppDCL/AppDCLWeb/Forms
' 
' *****************  Version 18  *****************
' User: Rudolpho     Date: 21.01.08   Time: 9:49
' Updated in $/CKG/Applications/AppDCL/AppDCLWeb/Forms
' 
' *****************  Version 17  *****************
' User: Rudolpho     Date: 8.08.07    Time: 9:56
' Updated in $/CKG/Applications/AppDCL/AppDCLWeb/Forms
' 
' *****************  Version 16  *****************
' User: Rudolpho     Date: 25.07.07   Time: 7:45
' Updated in $/CKG/Applications/AppDCL/AppDCLWeb/Forms
' ITA: 1058
' 
' *****************  Version 15  *****************
' User: Rudolpho     Date: 18.07.07   Time: 10:07
' Updated in $/CKG/Applications/AppDCL/AppDCLWeb/Forms
' _Report03- SessionID beim Aufruf der Selection.aspx mitgegeben
' 
' *****************  Version 14  *****************
' User: Rudolpho     Date: 28.06.07   Time: 17:32
' Updated in $/CKG/Applications/AppDCL/AppDCLWeb/Forms
' 
' *****************  Version 13  *****************
' User: Rudolpho     Date: 28.06.07   Time: 16:26
' Updated in $/CKG/Applications/AppDCL/AppDCLWeb/Forms
' 
' *****************  Version 12  *****************
' User: Uha          Date: 21.06.07   Time: 12:36
' Updated in $/CKG/Applications/AppDCL/AppDCLWeb/Forms
' Logging der Laufzeiten der ASPX-Seiten eingeführt
' 
' *****************  Version 11  *****************
' User: Uha          Date: 15.05.07   Time: 15:58
' Updated in $/CKG/Applications/AppDCL/AppDCLWeb/Forms
' Änderungen aus StartApplication vom 11.05.2007
' 
' ************************************************