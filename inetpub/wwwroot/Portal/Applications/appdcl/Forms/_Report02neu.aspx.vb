Imports CKG.Portal.PageElements
Imports CKG.Base.Kernel.Common.Common
Imports System.IO

<CLSCompliant(False)> Public Class _Report02neu
    Inherits System.Web.UI.Page

    Protected WithEvents ucStyles As Styles
    Protected WithEvents ucHeader As Header
    Protected WithEvents lblPageTitle As System.Web.UI.WebControls.Label
    Protected WithEvents lblDatensatz As System.Web.UI.WebControls.Label
    Protected WithEvents lblMsg As System.Web.UI.WebControls.Label
    Protected WithEvents lblScript As System.Web.UI.WebControls.Label
    Protected WithEvents btnBearb As System.Web.UI.WebControls.LinkButton
    'Protected WithEvents cbxFinished As System.Web.UI.WebControls.CheckBox
    Protected WithEvents lblOpen As System.Web.UI.WebControls.Label
    Protected WithEvents cbxArc As System.Web.UI.WebControls.CheckBox
    Private m_App As Base.Kernel.Security.App
    Private m_User As Base.Kernel.Security.User
    'Private showCheckbox As Boolean
    Protected WithEvents lnkKreditlimit As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lblHead As System.Web.UI.WebControls.Label
    Protected WithEvents lblNoData As System.Web.UI.WebControls.Label
    Protected WithEvents Table10 As System.Web.UI.HtmlControls.HtmlTable
    Protected WithEvents lblError As System.Web.UI.WebControls.Label
    Protected WithEvents gridServer As System.Web.UI.WebControls.DataGrid
    Protected WithEvents lbxAuftrag As System.Web.UI.WebControls.ListBox
    ' Protected WithEvents btnFinish As System.Web.UI.WebControls.LinkButton
    Protected WithEvents btnConfirm As System.Web.UI.WebControls.LinkButton
    Protected WithEvents td01 As System.Web.UI.HtmlControls.HtmlTableCell
    Protected WithEvents td03 As System.Web.UI.HtmlControls.HtmlTableCell
    Protected WithEvents td04 As System.Web.UI.HtmlControls.HtmlTableCell
    'Protected WithEvents btnShowPics As System.Web.UI.WebControls.LinkButton
    'Protected WithEvents btnBack As System.Web.UI.WebControls.LinkButton

    'Private appl As String
    Private Const fileNameDelimiter As Char = "-"c
    'Private Const fileExtension As String = ".JPG"

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
        m_User = GetUser(Me)

        ucHeader.InitUser(m_User)
        FormAuth(Me, m_User)

        GetAppIDFromQueryString(Me)

        lblHead.Text = m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString
        ucStyles.TitleText = lblHead.Text

        ' Wird in der Global.asax als Schalter verwendet, um bei Sessionabbruch die Zugriffssperre zu lösen.
        Session("FreeDBFromAccess") = True

        Try
            m_App = New Base.Kernel.Security.App(m_User)

            If Not IsPostBack Then
                loadData()
                loadForm()
                UeberfuehrungNeu.RemoveEmptyDirectories()
                UeberfuehrungNeu.DropAuftragAccess(m_User.UserID, m_App.Connectionstring)
            End If

        Catch ex As Exception
            lblError.Text = "Beim Laden der Seite ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
        End Try
    End Sub

    Private Sub loadData()
        Dim table As DataTable

        table = readDataFromServer()
        If (Not (table Is Nothing)) AndAlso (table.Rows.Count > 0) Then
            If Session("Serverfiles") Is Nothing Then
                Session.Add("Serverfiles", table)
            Else
                Session("Serverfiles") = table
            End If
            'FillGridServer(0)
        End If
    End Sub

    Private Sub loadForm()
        Dim table As DataTable
        Dim tableFiles As New DataTable()
        Dim row As DataRow
        Dim rowFiles As DataRow
        Dim strItem As String
        Dim auftrag As String
        Dim fahrer As String
        Dim tour As String
        'Aufträge aus den Dateinamen sammeln....

        If Not (Session("Serverfiles") Is Nothing) Then

            tableFiles.Columns.Add("Auftrag", GetType(System.String))
            tableFiles.Columns.Add("Tour", GetType(System.String))
            tableFiles.Columns.Add("Fahrer", GetType(System.String))
            tableFiles.Columns.Add("Daten", GetType(System.String))

            table = CType(Session("Serverfiles"), DataTable)

            For Each row In table.Rows
                auftrag = UeberfuehrungNeu.getAuftragFromFilename(row("Filename").ToString)
                tour = UeberfuehrungNeu.getTourFromFilename(row("Filename").ToString)
                fahrer = UeberfuehrungNeu.getFahrerFromFilename(row("Filename").ToString)

                If (tableFiles.Select("Auftrag = '" & auftrag & "' AND Tour = '" & tour & "'").Length = 0) Then
                    rowFiles = tableFiles.NewRow()
                    rowFiles("Auftrag") = auftrag
                    rowFiles("Tour") = tour
                    rowFiles("Fahrer") = fahrer
                    rowFiles("Daten") = auftrag & "." & tour & "." & fahrer
                    tableFiles.Rows.Add(rowFiles)
                End If

            Next

            'Daten aus SAP holen
            Dim uebf As UeberfuehrungNeu

            uebf = New UeberfuehrungNeu(m_User, m_App, Session("AppID").ToString, Session.SessionID.ToString, "")

            uebf.getAuftragsDaten(tableFiles)
            If Not uebf.Status = 0 Then
                If uebf.Status = -2201 Then
                    lbxAuftrag.DataSource = tableFiles
                    lbxAuftrag.DataBind()
                End If

                lblError.Text = uebf.Message
                Exit Sub
            End If


            'For Each row In table.Rows
            '    Dim Rows() As DataRow = uebf.Result.Select("AUFNR='" & Right("0000000000" & row("Auftrag"), 10) & "'")
            '    If Rows.Length > 0 Then
            '        row("Kunnr") = CStr(Rows(0)("ZZKUNNR")) '"0000000000"
            '    End If
            'Next

            'tableFiles = New DataTable()
            tableFiles = uebf.Result
            tableFiles.Columns.Add("Daten", Type.GetType("System.String"))

            For Each row In tableFiles.Rows
                strItem = String.Empty

                If Not (row("AUFNR") Is Nothing) AndAlso (row("AUFNR").ToString.Length > 0) Then           'Auftragsnr
                    strItem &= row("AUFNR").ToString.TrimStart("0"c)
                End If
                If Not (row("Fahrtnr") Is Nothing) AndAlso (row("Fahrtnr").ToString.Length > 0) Then           'Auftragsnr
                    strItem &= "." & row("Fahrtnr").ToString
                End If
                If Not (row("Fahrer") Is Nothing) AndAlso (row("Fahrer").ToString.Length > 0) Then           'Auftragsnr
                    strItem &= "." & row("Fahrer").ToString.TrimStart("0"c)
                End If
                If Not (row("Fahrtvon") Is Nothing) AndAlso (row("Fahrtvon").ToString.Length > 0) Then           'Auftragsnr
                    strItem &= "." & row("Fahrtvon").ToString
                End If
                If Not (row("Fahrtnach") Is Nothing) AndAlso (row("Fahrtnach").ToString.Length > 0) Then           'Auftragsnr
                    strItem &= "->" & row("Fahrtnach").ToString
                End If
                If Not (row("ZZKENN") Is Nothing) AndAlso (row("ZZKENN").ToString.Length > 0) Then           'Auftragsnr
                    strItem &= "." & row("ZZKENN").ToString
                End If
                If Not (row("ZZBEZEI") Is Nothing) AndAlso (row("ZZBEZEI").ToString.Length > 0) Then           'Auftragsnr
                    strItem &= "." & row("ZZBEZEI").ToString
                End If

                row("Daten") = strItem
                tableFiles.AcceptChanges()
            Next

            'In Liste einfügen
            lbxAuftrag.DataSource = tableFiles
            lbxAuftrag.DataTextField = "Daten"
            lbxAuftrag.DataBind()
        Else
            lblError.Text = "Verzeichnis ist leer."

            btnConfirm.Enabled = False
        End If
    End Sub

    Private Sub FillGridServer(ByVal intPageIndex As Int32, ByVal auftrag As String, ByVal tour As String, ByRef status As String,
                               Optional ByVal onlySaved As Boolean = False, Optional ByVal strSort As String = "",
                               Optional ByVal direction As String = "", Optional ByVal Empty As Boolean = False)
        Dim table As DataTable

        If Empty Then
            table = Nothing
        Else
            table = CType(Session("Serverfiles"), DataTable)
        End If

        If (table Is Nothing) Then
            gridServer.DataSource = Nothing
            gridServer.DataBind()
            Exit Sub
        End If

        If table.Rows.Count = 0 Then
            gridServer.Visible = False
            lblNoData.Visible = True
            lblNoData.Text = "Keine Daten zur Anzeige gefunden."
        Else
            gridServer.Visible = True
            lblNoData.Visible = False
            btnConfirm.Visible = True

            Dim tmpDataView As DataView = table.DefaultView

            'Filter setzen
            'Auftrag + Tour
            If (auftrag <> String.Empty) And (tour <> String.Empty) Then
                tmpDataView.RowFilter = "Auftrag = '" & auftrag & "' AND Tour = '" & tour & "'"
            End If
            'Auftrag
            If auftrag <> String.Empty And tour = String.Empty Then
                tmpDataView.RowFilter = "Auftrag = '" & auftrag & "'"
            End If
            'Tour
            If auftrag = String.Empty And tour <> String.Empty Then
                tmpDataView.RowFilter = "Tour = '" & tour & "'"
            End If
            'Nur die zur Abrechnung oder Umkennzeichnung markierten Aufträge filtern!
            If (onlySaved = True) Then
                tmpDataView.RowFilter = "Save = 'X'"
            End If

            If tmpDataView.Count = 0 Then
                status = "Keine Dateien zum Archivieren ausgewählt."
                Exit Sub
            End If

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

            If (direction.Length > 0) Then
                tmpDataView.Sort = strTempSort & " " & direction
            End If

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

    Private Function readDataFromServer(Optional ByVal auftragsnr As String = "", Optional ByVal tournr As String = "") As DataTable
        Dim uebf As UeberfuehrungNeu
        Dim table As DataTable

        uebf = New UeberfuehrungNeu(m_User, m_App, Session("AppID").ToString, Session.SessionID.ToString, "")
        table = uebf.readDataFromServer(auftragsnr, tournr, "", True)

        If Not (table Is Nothing) AndAlso (table.Rows.Count > 0) Then
            Session("Serverfiles") = table
        Else
            Session("Serverfiles") = Nothing
        End If
        Return table
    End Function

    Private Sub gridServer_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles gridServer.ItemCommand
        Dim filename As String
        Dim file As FileInfo
        Dim table As DataTable
        Dim row As DataRow
        Dim status As String

        table = CType(Session("Serverfiles"), DataTable)
        status = String.Empty

        If e.CommandName = "Delete" Then
            filename = ConfigurationManager.AppSettings("UploadpathLocal") & e.CommandArgument & e.Item.Cells(4).Text
            file = New FileInfo(filename)

            Try
                If file.Exists Then
                    file.Delete()
                    row = table.Select("Filename='" & e.Item.Cells(4).Text & "'")(0)
                    table.Rows.Remove(row)
                Else
                    status = "Datei nicht gefunden."
                End If
            Catch ex As Exception
                status = "Fehler beim Löschen der Datei."
            Finally
                If (status <> String.Empty) Then
                    row = table.Select("Filename = '" & e.Item.Cells(4).Text & "'")(0)
                    row("Status") = status
                End If

                table.AcceptChanges()
                Session("Serverfiles") = table

                Dim auftragsnr As String = getAuftragsNr()
                Dim tournr As String = getTourNr()
                If table.Select("Auftrag = '" & auftragsnr & "' AND Tour = '" & tournr & "'").Length > 0 Then
                    FillGridServer(0, auftragsnr, tournr, status)
                Else
                    ' Auftragsliste neu laden und Grid leeren
                    loadForm()
                    FillGridServer(0, auftragsnr, tournr, status, False, "", "asc", True)
                    btnConfirm.Enabled = False
                End If

                fillList(table)
                fillView()
            End Try
        End If
    End Sub

    Public Function getAuftragsNr() As String
        Dim auftragsnr As String

        If lbxAuftrag.SelectedIndex >= 0 Then
            auftragsnr = lbxAuftrag.Items.Item(lbxAuftrag.SelectedIndex).Text
            auftragsnr = Left(auftragsnr, auftragsnr.IndexOf("."))
        Else
            auftragsnr = String.Empty
        End If

        Return auftragsnr
    End Function

    Public Function getTourNr() As String
        Dim tournr As String

        If lbxAuftrag.SelectedIndex >= 0 Then
            tournr = lbxAuftrag.Items.Item(lbxAuftrag.SelectedIndex).Text
            tournr = Right(tournr, tournr.Length - tournr.IndexOf(".") - 1)
            tournr = Left(tournr, 1)
        Else
            tournr = String.Empty
        End If

        Return tournr
    End Function

    Private Sub gridServer_SortCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs) Handles gridServer.SortCommand
        FillGridServer(gridServer.CurrentPageIndex, getAuftragsNr(), getTourNr(), e.SortExpression)
    End Sub

    Private Sub fillView()
        Dim table As DataTable
        Dim item As DataGridItem
        Dim cell As TableCell
        Dim ctl As Control
        Dim ddl As DropDownList
        Dim cbx As CheckBox
        Dim row As DataRow
        Dim fileName As String

        table = CType(Session("Serverfiles"), DataTable)

        If (Not table Is Nothing) AndAlso (table.Rows.Count > 0) Then
            For Each item In gridServer.Items
                For Each cell In item.Cells
                    For Each ctl In cell.Controls
                        'Dropdownlist abfragen
                        Dim dropDownList = TryCast(ctl, DropDownList)
                        If (dropDownList IsNot Nothing) Then
                            ddl = dropDownList
                            'fileName = item.Cells(4).Text           'Dateiname 
                            'fileName = Left(fileName, fileName.IndexOf(fileNameDelimiter)) & "." & Left(Right(fileName, fileName.Length - fileName.LastIndexOf("_") - 1), 1)
                        End If
                        'Checkbox abfragen
                        Dim checkBox = TryCast(ctl, CheckBox)
                        If (checkBox IsNot Nothing) Then
                            cbx = checkBox
                            If cbx.ID = "cbxArchiv" Then
                                cbx.Checked = False
                                fileName = item.Cells(13).Text       'Dateiname
                                If table.Select("FilenameOld = '" & fileName & "'").Length > 0 Then
                                    row = table.Select("FilenameOld = '" & fileName & "'")(0) '

                                    If (row("Save") = "X") Then
                                        cbx.Checked = True
                                    Else
                                        cbx.Checked = False
                                    End If
                                End If

                            End If
                        End If
                    Next
                Next
            Next

            fillDDL(table)
        End If
    End Sub

    Private Sub fillList(ByVal table As DataTable)
        Dim item As DataGridItem
        Dim cell As TableCell
        Dim ctl As Control
        Dim ctlDDl As Control
        Dim ddl As DropDownList
        Dim cbx As CheckBox
        Dim lItem As ListItem
        Dim row As DataRow
        Dim auftrag As String
        Dim tour As String
        Dim fileName As String
        Dim filePath As String

        'Tabelle mit Eingabedaten füllen...

        If (Not table Is Nothing) AndAlso (table.Rows.Count > 0) Then
            For Each item In gridServer.Items
                For Each cell In item.Cells
                    For Each ctl In cell.Controls
                        'Checkbox abfragen
                        Dim checkBox = TryCast(ctl, CheckBox)
                        If (checkBox IsNot Nothing) Then
                            cbx = checkBox
                            If cbx.ID = "cbxArchiv" Then
                                If cbx.Checked = True Then
                                    fileName = item.Cells(4).Text       'Dateiname
                                    Dim rows As DataRow() = table.Select("Filename = '" & fileName & "'")
                                    If rows.Length > 0 Then
                                        rows(0)("Save") = "X" 'Merken zum wegspeichern!
                                        table.AcceptChanges()
                                    End If
                                End If
                            End If
                            If cbx.ID = "cbxMove" Then
                                If cbx.Checked = True Then
                                    For Each ctlDDl In cell.Controls
                                        Dim dropDownList = TryCast(ctlDDl, DropDownList)
                                        If (dropDownList IsNot Nothing) Then
                                            ddl = dropDownList
                                            'DDL-Auswahl in Spalte "FilenameNew" merken...
                                            lItem = ddl.SelectedItem
                                            filePath = item.Cells(0).Text       'Pfad
                                            fileName = item.Cells(4).Text       'Dateiname
                                            auftrag = Left(fileName, fileName.IndexOf(fileNameDelimiter))
                                            tour = Left(Right(fileName, fileName.Length - fileName.LastIndexOf(fileNameDelimiter) - 1), 1)

                                            '...nur, wenn Auftrag oder Tour abweichend!
                                            If Not (lItem Is Nothing) Then
                                                If (auftrag <> Left(lItem.Text, lItem.Text.IndexOf("."))) Or (tour <> Right(lItem.Text, 1)) Then
                                                    row = table.Select("Filename = '" & fileName & "'")(0)
                                                    row("FilenameNew") = lItem.Text
                                                    table.AcceptChanges()
                                                Else
                                                    row = table.Select("Filename = '" & fileName & "'")(0)
                                                    row("FilenameNew") = String.Empty
                                                    table.AcceptChanges()
                                                End If
                                            End If
                                        End If
                                    Next
                                End If
                            End If
                        End If
                    Next
                Next
            Next
            Session("Serverfiles") = table
        End If
    End Sub

    Public Function getID(ByVal index As Integer) As Integer
        Return 1
    End Function

    Private Sub fillDDL(ByVal table As DataTable)
        Dim item As DataGridItem
        Dim cell As TableCell
        Dim ctl As Control
        Dim ddl As DropDownList
        Dim lItem As ListItem
        Dim row As DataRow
        Dim cbx As CheckBox
        Dim count As Integer
        Dim str As String
        Dim auftrag As String
        Dim tour As String
        Dim fahrerSelected As String
        Dim fahrerTable As String
        'Dim tblDDL As New DataTable()

        Try
            For Each item In gridServer.Items
                For Each cell In item.Cells
                    For Each ctl In cell.Controls
                        'Dropdownlist abfragen
                        Dim dropDownList = TryCast(ctl, DropDownList)
                        If (dropDownList IsNot Nothing) Then
                            ddl = dropDownList
                            count = 0
                            For Each row In table.Rows
                                str = row("Filename")

                                auftrag = UeberfuehrungNeu.getAuftragFromFilename(str)
                                tour = UeberfuehrungNeu.getTourFromFilename(str)
                                fahrerTable = UeberfuehrungNeu.getFahrerFromFilename(str)

                                'Fahrer aus gewähltem Auftrag extrahieren
                                fahrerSelected = lbxAuftrag.SelectedItem.Text
                                fahrerSelected = Right(fahrerSelected, fahrerSelected.Length - fahrerSelected.IndexOf(".") - 1)
                                fahrerSelected = Right(fahrerSelected, fahrerSelected.Length - fahrerSelected.IndexOf(".") - 1)
                                'fahrerSelected = Left(fahrerSelected, fahrerSelected.IndexOf("."))

                                If (fahrerTable = fahrerSelected) Then       'Nur Aufträge für eigenen Fahrer!
                                    lItem = ddl.Items.FindByText(auftrag & "." & tour)
                                    If lItem Is Nothing Then    'Hinzufügen
                                        If ((item.Cells(11).Text & "." & item.Cells(12).Text) <> (auftrag & "." & tour)) Then
                                            lItem = New ListItem()
                                            lItem.Text = auftrag & "." & tour
                                            lItem.Value = count
                                            ddl.Items.Add(lItem)
                                            count += 1
                                        End If
                                    End If
                                End If

                            Next
                            If ddl.Items.Count <= 0 Then    'Wenn nur ein Auftrag, Verschieben nicht möglich...
                                ddl.Enabled = False

                                cbx = cell.FindControl("cbxMove")
                                cbx.Enabled = False
                            End If
                            'str = item.Cells(4).Text      'aktueller Dateiname...
                            'auftrag = Left(str, str.IndexOf(fileNameDelimiter))
                            'tour = Left(Right(str, str.Length - str.LastIndexOf(fileNameDelimiter) - 1), 1)
                        End If
                    Next
                Next
            Next
        Catch ex As Exception
            If ex.Message = "Das Argument 'Length' muss größer als oder gleich Null sein." AndAlso count = 0 Then
                lblError.Text = "Die Zielauswahl konnte nicht gefüllt werden, prüfen sie die Bezeichnung der Aufträge im linken Fenster!"
            Else
                lblError.Text = ex.Message
            End If
        End Try
    End Sub

    Private Sub btnConfirm_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnConfirm.Click
        Dim table As DataTable
        Dim status As String = ""
        Dim uebf As New UeberfuehrungNeu(m_User, m_App, Session("AppID").ToString, Session.SessionID.ToString, "")
        Dim row As DataRow

        table = CType(Session("Serverfiles"), DataTable)

        For Each row In table.Rows
            row("FilenameNew") = row("Filename")
            table.AcceptChanges()
        Next
        
        If (status <> String.Empty) Then
            lblError.Text = "Fehler: " & status
        Else

            uebf.transferFiles(table, status, m_User.IsTestUser)        'Dateien transferieren...
           
            If (status <> String.Empty) Then
                lblError.Text = "Fehler: " & status
            Else
                uebf = New UeberfuehrungNeu(m_User, m_App, Session("AppID").ToString, Session.SessionID.ToString, "")
                uebf.writeSAP(Session("AppID").ToString, Session.SessionID.ToString, table, status)               'SAP aktualisieren
                If (status <> String.Empty) Then
                    lblError.Text = "Fehler: " & status
                Else
                    'Alles ok!
                    'Quittung ausgeben
                    showResult()
                    UeberfuehrungNeu.DeleteAuftragFromDB(getAuftragsNr(), getTourNr(), m_User.UserID, m_App.Connectionstring)
                End If
            End If
        End If
    End Sub

    Private Sub showResult()
        Dim table As DataTable
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
        Dim appid As String = Session("AppID").ToString
        lblOpen.Text = "<script language=""Javascript"">window.open(""_Report022.aspx?USER=" & m_User.UserName & "&PAR=" & str &
            """, ""Übertragungsprotokoll"", ""width=640,height=480,left=0,top=0,scrollbars=YES"");location.replace('../../appdcl/Forms/_Report02neu.aspx?AppID=" & appid & "')</script>"
    End Sub

    Private Sub saveChanges()
        Dim table As DataTable
        Dim status As String

        status = String.Empty
        lblError.Text = status

        If Not (Session("Serverfiles") Is Nothing) Then
            table = CType(Session("Serverfiles"), DataTable)
            fillList(table)                                 'Werte in Tabelle füllen (Auswahl Archivierung und Verschieben)
            UeberfuehrungNeu.moveFiles(table)                                 'Dateien verschieben
            fillView()                                      'Steuerelemente füllen            
        End If
    End Sub

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        SetEndASPXAccess(Me)
    End Sub

    Private Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Unload
        SetEndASPXAccess(Me)
    End Sub

    Private Sub lbxAuftrag_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles lbxAuftrag.SelectedIndexChanged
        saveChanges()       'Evtl. Änderungen speichern

        ' alten Auftrag freigeben
        If Not (Session("LastAccessAuftrag") = Nothing And Session("LastAccessTour") = Nothing) And
            (Session("LastAccessAuftrag") <> "" And Session("LastAccessTour") <> "") Then

            UeberfuehrungNeu.DropAuftragAccess(CStr(Session("LastAccessAuftrag")), CStr(Session("LastAccessTour")), m_User.UserID, m_App.Connectionstring)
        End If

        ' Zugriff auf neuen Auftrag anfordern
        Dim auftragsnr As String = String.Empty
        Dim tournr As String = String.Empty
        Dim table As DataTable
        Dim status As String = String.Empty

        If lbxAuftrag.SelectedIndex >= 0 Then
            auftragsnr = getAuftragsNr()
            tournr = getTourNr()
            If UeberfuehrungNeu.GetAuftragAccess(auftragsnr, tournr, m_User.UserID, Session.SessionID, m_App.Connectionstring) Then
                ' aktuelle Bilder auf speichern setzen
                table = Session("Serverfiles")
                If Not table Is Nothing Then
                    For Each row As DataRow In table.Rows
                        If row("Auftrag") = auftragsnr And row("Tour") = tournr Then
                            row("Save") = "X"
                        Else
                            row("Save") = String.Empty
                        End If
                    Next
                End If

                FillGridServer(0, auftragsnr, tournr, status)
                btnConfirm.Enabled = True
            Else
                FillGridServer(0, auftragsnr, tournr, status, , , , True)
                btnConfirm.Enabled = False
                lblError.Text = "Auftrag wird zur Zeit von einem anderen Benutzer bearbeitet. Versuchen Sie es später erneut."
            End If
            
            fillView()
        Else
            lblError.Text = "Kein Auftrag ausgewählt."
        End If

        ' neue Auftrag als letzen Auftrag speichern
        Session("LastAccessAuftrag") = auftragsnr
        Session("LastAccessTour") = tournr
    End Sub

End Class

' ************************************************
' $History: _Report02.aspx.vb $
' 
' *****************  Version 6  *****************
' User: Rudolpho     Date: 4.02.10    Time: 14:38
' Updated in $/CKAG/Applications/appdcl/Forms
' ITA: 2918
' 
' *****************  Version 5  *****************
' User: Rudolpho     Date: 30.04.09   Time: 9:25
' Updated in $/CKAG/Applications/appdcl/Forms
' ITA: 2837
' 
' *****************  Version 4  *****************
' User: Jungj        Date: 13.01.09   Time: 11:07
' Updated in $/CKAG/Applications/appdcl/Forms
' fehlerbehandlung eingeführt
' 
' *****************  Version 3  *****************
' User: Jungj        Date: 12.01.09   Time: 15:13
' Updated in $/CKAG/Applications/appdcl/Forms
' ITA 2528
' 
' *****************  Version 2  *****************
' User: Fassbenders  Date: 18.06.08   Time: 13:46
' Updated in $/CKAG/Applications/appdcl/Forms
' Nicht verwendete Variablen gelöscht.
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 7.04.08    Time: 13:00
' Created in $/CKAG/Applications/appdcl/Forms
' 
' *****************  Version 10  *****************
' User: Rudolpho     Date: 9.08.07    Time: 11:39
' Updated in $/CKG/Applications/AppDCL/AppDCLWeb/Forms
' Bugfix: Fehlerbehandlung  _Report02.aspx Methode Fillddl eingefügt
' 
' *****************  Version 9  *****************
' User: Uha          Date: 26.06.07   Time: 11:44
' Updated in $/CKG/Applications/AppDCL/AppDCLWeb/Forms
' Methodenaufruf korrigiert (AppDCL, _Report02.aspx.vb)
' 
' *****************  Version 8  *****************
' User: Uha          Date: 21.06.07   Time: 12:36
' Updated in $/CKG/Applications/AppDCL/AppDCLWeb/Forms
' Logging der Laufzeiten der ASPX-Seiten eingeführt
' 
' *****************  Version 7  *****************
' User: Uha          Date: 7.03.07    Time: 10:26
' Updated in $/CKG/Applications/AppDCL/AppDCLWeb/Forms
' 
' ************************************************
