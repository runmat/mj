Imports CKG.Base.Kernel.Common
Imports CKG.Base.Kernel.Security
Imports CKG.Base.Business
Imports System.IO
Imports Telerik.Web.UI
Imports Telerik.Web.UI.GridExcelBuilder

Public Class Report16
    Inherits System.Web.UI.Page


#Region "Declarations"
    Private m_App As App
    Private m_User As User
    Private tmpFile As String
    Private sInfo As String
    Private isExcelExportConfigured As Boolean = False
    Private tempExcelLastRow = 0
    Private Const ResultSessionKey = "TelefonResult"
#End Region

    Protected Overrides Sub OnLoad(e As EventArgs)
        MyBase.OnLoad(e)

        m_User = Common.GetUser(Me)
        Common.FormAuth(Me, m_User)
        Common.GetAppIDFromQueryString(Me)

        m_App = New App(m_User)
        lblHead.Text = m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString

        If Not IsPostBack = False Then


        End If

        If Not String.IsNullOrEmpty(sInfo) Then
            lblError.Visible = True
            lblError.Text = sInfo
        End If

        If Not Page.IsPostBack Then
            txt_Datum_von.Text = Date.Now.AddDays(-1).ToShortDateString()
        End If

        'Common.TranslateTelerikColumns(rgTelefon)
    End Sub

    Protected Overrides Sub OnPreRender(e As EventArgs)
        MyBase.OnPreRender(e)

        Common.SetEndASPXAccess(Me)




    End Sub

    Protected Overrides Sub OnUnload(e As EventArgs)
        MyBase.OnUnload(e)

        Common.SetEndASPXAccess(Me)
    End Sub

    Private Sub LoadData(Optional ByVal rebind As Boolean = True)
        Dim table = CType(Session(ResultSessionKey), DataTable)
        If Not table Is Nothing Then
            rgTelefon.DataSource = table.DefaultView

            For i As Integer = 0 To rgTelefon.Columns.Count - 1
                rgTelefon.Columns(i).HeaderText = table.Columns(i).Caption
            Next
        Else
            rgTelefon.DataSource = Nothing
        End If

        If rebind Then rgTelefon.Rebind()
    End Sub

    Private Sub SetError(ByVal text As String)

        lblError.Text += text + "<br/>"

        If Not String.IsNullOrEmpty(lblError.Text.Trim()) Then
            lblError.Visible = True
        End If

    End Sub

    Private Function LoadFromCsv(ByVal von As DateTime, ByVal bis As DateTime, ByVal skill As String) As Boolean

        Dim sr As StreamReader = Nothing

        Try
            Dim dt = New DataTable()
            Dim headerRow = 8
            Dim delimiter As String = ","
            Dim index As Integer = 0

            'Datentypen der CSV Datei
            Dim dataTypes = New String() {"System.DateTime", "System.String", _
                                         "System.Int32", "System.Int32", "System.Int32", "System.Int32", "System.Int32", _
                                           "System.Double", "System.Double", "System.Int32", "System.Int32", "System.Int32", "System.Int32", _
                                           "System.Int32", "System.Int32", "System.Int32", "System.Int32", "System.Int32"}

            'Festlegen der Datafield Bezeichnungen für die Datatable (Mit Gridview Übereinstimmen)
            Dim dataNames = New String() {"datInterval", "datSkill", _
                             "datSumAnr", "datAnzAng", "datLost", "datAnzAngK20", "datAnzAngG20", _
                               "datSlIn20Sec", "datSlGesamt", "datGDauerMin", "datGDauerMax", "datGDauerDs", "datWDauerMin", _
                               "datWDauerMax", "datWDauerDs", "datWzVorAuflegenMin", "datWzVorAuflegenMax", "datWzVorAuflegenDs"}

            'Festlegen der Datafield Bezeichnungen für die Datatable (Mit Gridview Übereinstimmen)
            Dim dataCaptions = New String() {"Interval", "Skill", _
                             "Sum", "Anz", "Lost", "<=20", ">20", _
                               "S20", "SGes", "DMin", "DMax", "DØ", "WMin", _
                               "WMax", "WØ", "AMin", "AMax", "AØ"}

            Dim isFirstFile = True

            Dim pathToCsv = Common.GetCustomerSetting(m_User.Customer, Session("AppID").ToString, "TransferPath")

            'Wenn Virtueller Pfad dann den Physischen setzen
            If pathToCsv.StartsWith("../") Then
                pathToCsv = Server.MapPath(pathToCsv)
            End If

            If String.IsNullOrEmpty(pathToCsv) Then
                sInfo = "Übergabeverzeichnis nicht gesetzt."
                Return False
            End If


            Dim inDirInf As DirectoryInfo = New DirectoryInfo(pathToCsv)

            'dateien auslesen
            Dim datum As DateTime = von
            While datum.Date <= bis.Date

                'alle csvdateien Suchen die dem Aktuellen Datum entsprechen
                Dim fileInfos As FileInfo() = inDirInf.GetFiles(String.Format("{0:yyyyMMdd}*.csv", datum))

                If fileInfos.Length > 0 Then
                    For Each fi As FileInfo In fileInfos
                        Dim rowInd As Integer = 0

                        'Auslesen der aktuellen Datei
                        sr = New StreamReader(fi.FullName, Text.Encoding.Default)
                        While Not sr.EndOfStream

                            rowInd += 1
                            Dim line As String = sr.ReadLine

                            'Monatsreports überspringen
                            If line.Contains("Targo Lines monatl") Then
                                Exit While
                            End If

                            Dim lineArray As String() = line.Split(delimiter)

                            'Columns erstellen
                            If headerRow = rowInd And isFirstFile Then
                                For index = 0 To lineArray.Length - 1

                                    Dim dc As DataColumn = dt.Columns.Add(dataNames(index))
                                    dc.DataType = Type.GetType(dataTypes(index))

                                    '  dc.Caption = lineArray(index)
                                    dc.Caption = dataCaptions(index)
                                Next

                                isFirstFile = False

                                'Daten füllen
                            ElseIf rowInd > headerRow Then

                                If UCase(lineArray(0)).Equals("GESAMT") Then
                                    Continue While
                                End If

                                'TODO Achtung TARGOBANK Spezifische Zeichenfolge "prompt" entfernen
                                lineArray(1) = lineArray(1).Replace("prompt", "").Trim


                                'Check Datatypes
                                For index = 0 To lineArray.Length - 1

                                    If dataTypes(index).Equals("System.Double") Then
                                        Dim tmpDouble As Double
                                        If Not Double.TryParse(lineArray(index).Replace(".", ","), tmpDouble) Then
                                            lineArray(index) = "0"
                                        Else
                                            lineArray(index) = tmpDouble
                                        End If
                                    End If

                                    If dataTypes(index).Equals("System.Int32") Then
                                        Dim tmpInt As Integer
                                        If Not Integer.TryParse(lineArray(index), tmpInt) Then
                                            lineArray(index) = "0"
                                        Else
                                            lineArray(index) = tmpInt
                                        End If
                                    End If

                                    If dataTypes(index).Equals("System.DateTime") Then
                                        Dim tmpDateTime As DateTime
                                        If Not DateTime.TryParse(lineArray(index), tmpDateTime) Then
                                            lineArray(index) = Nothing
                                        Else
                                            lineArray(index) = tmpDateTime
                                        End If
                                    End If
                                Next

                                dt.Rows.Add(lineArray)
                            End If
                        End While

                        sr.Close()
                    Next

                End If

                datum = datum.AddDays(1)

            End While

            If dt.Columns.Contains("datSkill") AndAlso Not String.IsNullOrEmpty(skill) Then
                dt.DefaultView.RowFilter = String.Format("datSkill = '{0}'", skill)
                dt = dt.DefaultView.ToTable
            End If

            If dt.Rows.Count = 0 Then
                Return False
            End If

            If (dt.Columns.Count > 0) Then AddMissingRows(dt)

            Session(ResultSessionKey) = dt

            Return True

        Catch ex As Exception
            sInfo = ex.Message
            If Not sr Is Nothing Then
                sr.Close()
            End If

            Return False

        End Try
    End Function

    Private Sub AddMissingRows(table As DataTable)
        Dim skills = table.DefaultView.ToTable(True, "datSkill").Rows.Cast(Of DataRow).Select(Function(r) r("datSkill")).ToArray
        Dim days = table.DefaultView.ToTable(True, "datInterval").Rows.Cast(Of DataRow).Select(Function(r) DateTime.Parse(CStr(r("datInterval"))).Date).Distinct().ToArray

        For Each skill As String In skills
            For Each day As DateTime In days
                For i = 8 To 18
                    Dim interval = String.Format("{0:dd.MM.yyyy} {1:00}:00", day, i)
                    If table.Select(String.Format("datSkill='{0}' AND datInterval='{1}'", skill, interval)).Length = 0 Then
                        table.Rows.Add(New Object() {interval, skill, _
                                         0, 0, 0, 0, 0, _
                                           0.0, 0.0, 0, 0, 0, 0, _
                                           0, 0, 0, 0, 0})
                    End If
                Next
            Next
        Next

        table.AcceptChanges()

        table.DefaultView.Sort = "datInterval"
    End Sub

    Protected Sub ibtNewSearch_Click(sender As Object, e As ImageClickEventArgs)

        If divSelection.Visible Then

            divSelection.Visible = False
            ibtNewSearch.ImageUrl = "../../../Images/queryArrow.gif"
            PnResult.Visible = True
            lb_Execute.Visible = False

        Else

            divSelection.Visible = True
            ibtNewSearch.ImageUrl = "../../../Images/queryArrowUp.gif"
            PnResult.Visible = False
            lb_Execute.Visible = True

        End If

    End Sub

    Protected Sub ExecuteClick(sender As Object, e As EventArgs) Handles lb_Execute.Click
        Dim skill As String = ddl_Skill.SelectedValue
        Dim vonDatum As DateTime
        Dim bisDatum As DateTime
        Dim jahr As Integer

        lblError.Text = String.Empty

        Select Case ddl_Berichtstyp.SelectedValue
            Case "Tagesbericht"
                If String.IsNullOrEmpty(txt_Datum_von.Text) Then
                    SetError("Es wurde kein Datum angegeben")
                    Exit Sub
                End If
                If Not DateTime.TryParse(txt_Datum_von.Text.Trim(), vonDatum) Then
                    SetError("Das Datum ist ungültig")
                    Exit Sub
                End If
                If vonDatum.Date >= Now.Date Then
                    SetError("Für dieses Datum liegen noch keine Daten vor")
                    Exit Sub
                End If
                bisDatum = vonDatum
            Case "Monatsbericht"
                If String.IsNullOrEmpty(txtJahr.Text) Then
                    SetError("Es wurde kein Jahr angegeben")
                    Exit Sub
                End If
                If Not Integer.TryParse(txtJahr.Text.Trim(), jahr) Then
                    SetError("Das Jahr ist ungültig")
                    Exit Sub
                End If
                vonDatum = New DateTime(jahr, Integer.Parse(ddl_Zeitraum.SelectedValue), 1)
                If vonDatum.Date >= Now.Date Then
                    SetError("Für diesen Zeitraum liegen noch keine Daten vor")
                    Exit Sub
                End If
                bisDatum = vonDatum.AddMonths(1).AddDays(-1)
                If bisDatum >= Now.Date Then
                    bisDatum = Now.Date.AddDays(-1)
                End If
            Case "Quartalsbericht"
                If String.IsNullOrEmpty(txtJahr.Text) Then
                    SetError("Es wurde kein Jahr angegeben")
                    Exit Sub
                End If
                If Not Integer.TryParse(txtJahr.Text.Trim(), jahr) Then
                    SetError("Das Jahr ist ungültig")
                    Exit Sub
                End If
                Select Case ddl_Zeitraum.SelectedValue
                    Case "1"
                        vonDatum = New DateTime(jahr, 1, 1)
                    Case "2"
                        vonDatum = New DateTime(jahr, 4, 1)
                    Case "3"
                        vonDatum = New DateTime(jahr, 7, 1)
                    Case "4"
                        vonDatum = New DateTime(jahr, 10, 1)
                End Select
                If vonDatum.Date >= Now.Date Then
                    SetError("Für diesen Zeitraum liegen noch keine Daten vor")
                    Exit Sub
                End If
                bisDatum = vonDatum.AddMonths(3).AddDays(-1)
                If bisDatum >= Now.Date Then
                    bisDatum = Now.Date.AddDays(-1)
                End If
            Case "Halbjahresbericht"
                If String.IsNullOrEmpty(txtJahr.Text) Then
                    SetError("Es wurde kein Jahr angegeben")
                    Exit Sub
                End If
                If Not Integer.TryParse(txtJahr.Text.Trim(), jahr) Then
                    SetError("Das Jahr ist ungültig")
                    Exit Sub
                End If
                Select Case ddl_Zeitraum.SelectedValue
                    Case "1"
                        vonDatum = New DateTime(jahr, 1, 1)
                    Case "2"
                        vonDatum = New DateTime(jahr, 7, 1)
                End Select
                If vonDatum.Date >= Now.Date Then
                    SetError("Für diesen Zeitraum liegen noch keine Daten vor")
                    Exit Sub
                End If
                bisDatum = vonDatum.AddMonths(6).AddDays(-1)
                If bisDatum >= Now.Date Then
                    bisDatum = Now.Date.AddDays(-1)
                End If
            Case "Jahresbericht"
                If String.IsNullOrEmpty(txtJahr.Text) Then
                    SetError("Es wurde kein Jahr angegeben")
                    Exit Sub
                End If
                If Not Integer.TryParse(txtJahr.Text.Trim(), jahr) Then
                    SetError("Das Jahr ist ungültig")
                    Exit Sub
                End If
                vonDatum = New DateTime(jahr, 1, 1)
                If vonDatum.Date >= Now.Date Then
                    SetError("Für diesen Zeitraum liegen noch keine Daten vor")
                    Exit Sub
                End If
                bisDatum = vonDatum.AddYears(1).AddDays(-1)
                If bisDatum >= Now.Date Then
                    bisDatum = Now.Date.AddDays(-1)
                End If
            Case "Auswahl Periode"
                If String.IsNullOrEmpty(txt_Datum_von.Text) Then
                    SetError("Es wurde kein Von-Datum angegeben")
                    Exit Sub
                End If
                If String.IsNullOrEmpty(txt_Datum_bis.Text) Then
                    SetError("Es wurde kein Bis-Datum angegeben")
                    Exit Sub
                End If
                If Not DateTime.TryParse(txt_Datum_von.Text.Trim(), vonDatum) Then
                    SetError("Das Von-Datum ist ungültig")
                    Exit Sub
                End If
                If Not DateTime.TryParse(txt_Datum_bis.Text.Trim(), bisDatum) Then
                    SetError("Das Bis-Datum ist ungültig")
                    Exit Sub
                End If
                If vonDatum > bisDatum Then
                    SetError("Das Von-Datum darf nicht kleiner als das Bis-Datum sein")
                    Exit Sub
                End If
                If vonDatum.Date >= Now.Date Then
                    SetError("Für diesen Zeitraum liegen noch keine Daten vor")
                    Exit Sub
                End If
                If bisDatum >= Now.Date Then
                    bisDatum = Now.Date.AddDays(-1)
                End If
        End Select

        If LoadFromCsv(vonDatum, bisDatum, skill) Then
            LoadData()
            divSelection.Visible = False
            PnResult.Visible = True
            ibtNewSearch.ImageUrl = "../../../Images/queryArrow.gif"
            lb_Execute.Visible = False
        Else
            Session.Remove(ResultSessionKey)
            If String.IsNullOrEmpty(sInfo) Then
                SetError("Keine Daten gefunden")
            Else
                SetError("Fehler beim Öffnen oder Konvertieren der CSV Datei")
                SetError(sInfo)
            End If

        End If
    End Sub

    Protected Sub rgTelefonNeedDataSource(sender As Object, e As GridNeedDataSourceEventArgs)
        LoadData(False)
    End Sub

    Protected Sub rgTelefonOnCustomAggregate(sender As Object, e As GridCustomAggregateEventArgs)

        Dim table As DataTable
        table = CType(Session(ResultSessionKey), DataTable)

        Select Case e.Column.UniqueName
            Case "datSlIn20Sec"
                e.Result = Sum(table, "datAnzAngK20") / Sum(table, "datAnzAng") * 100
            Case "datSlGesamt"
                e.Result = Sum(table, "datAnzAngK20") / Sum(table, "datSumAnr") * 100
            Case "datGDauerDs"
                e.Result = NonZeroAvg(table, "datGDauerDs")
            Case "datWDauerDs"
                e.Result = NonZeroAvg(table, "datWDauerDs")
            Case "datWzVorAuflegenDs"
                e.Result = NonZeroAvg(table, "datWzVorAuflegenDs")
            Case "datGDauerMin"
                e.Result = NonZeroMin(table, "datGDauerMin")
            Case "datWDauerMin"
                e.Result = NonZeroMin(table, "datWDauerMin")
            Case "datWzVorAuflegenMin"
                e.Result = NonZeroMin(table, "datWzVorAuflegenMin")
        End Select


    End Sub

    Private Function NonZeroAvg(table As DataTable, columnName As String) As Double
        Dim avg_value As Integer = 0
        Dim avg_rows As Integer = 0

        For Each row As DataRow In table.Rows
            If DirectCast(row(columnName), Integer) > 0 Then
                avg_value += DirectCast(row(columnName), Integer)
                avg_rows += 1
            End If
        Next
        Return If(avg_rows = 0, 0, Math.Round((avg_value / avg_rows), 2))
    End Function

    Private Function Sum(table As DataTable, columnName As String) As Integer
        Dim summe As Integer = 0

        For Each row As DataRow In table.Rows
            summe += DirectCast(row(columnName), Integer)
        Next
        Return summe
    End Function

    Private Function NonZeroMin(table As DataTable, columnName As String) As Integer
        Dim value As Integer = Integer.MaxValue

        For Each row As DataRow In table.Rows
            Dim rValue = DirectCast(row(columnName), Integer)
            If rValue > 0 Then
                value = Math.Min(rValue, value)
            End If
        Next
        Return If(value = Integer.MaxValue, 0, value)
    End Function

    Protected Sub rgTelefon_ExcelMLExportRowCreated(sender As Object, e As Telerik.Web.UI.GridExcelBuilder.GridExportExcelMLRowCreatedArgs)
        tempExcelLastRow += 1

        'Anpassen der Gridview Header für den  Export
        If e.RowType = GridExportExcelMLRowType.HeaderRow Then

            For index = 0 To e.Row.Cells.Count - 1
                e.Row.Cells(index).Data.DataItem = rgTelefon.MasterTableView.Columns(index).HeaderTooltip
            Next

        End If

        If e.RowType = GridExportExcelMLRowType.DataRow Then
            'Add custom styles to the desired cells
            'CellElement cell = e.Row.Cells.GetCellByName("UnitPrice");
            'cell.StyleValue = cell.StyleValue == "itemStyle" ? "priceItemStyle" : "alternatingPriceItemStyle";

            'cell = e.Row.Cells.GetCellByName("ExtendedPrice");
            'cell.StyleValue = cell.StyleValue == "itemStyle" ? "priceItemStyle" : "alternatingPriceItemStyle";

            'cell = e.Row.Cells.GetCellByName("Discount");
            'cell.StyleValue = cell.StyleValue == "itemStyle" ? "percentItemStyle" : "alternatingPercentItemStyle";

            If Not isExcelExportConfigured Then

                'Set Worksheet name
                e.Worksheet.Name = lblHead.Text

                'Set Column widths
                For Each column As ColumnElement In e.Worksheet.Table.Columns

                    If (e.Worksheet.Table.Columns.IndexOf(column) = 2) Then
                        column.Width = Unit.Point(180) 'set width 180 to ProductName column
                    Else
                        column.Width = Unit.Point(80) 'set width 80 to the rest of the columns
                    End If
                Next

                'Set Page options
                Dim pageSetup As PageSetupElement = e.Worksheet.WorksheetOptions.PageSetup
                pageSetup.PageLayoutElement.IsCenteredVertical = True
                pageSetup.PageLayoutElement.IsCenteredHorizontal = True
                pageSetup.PageMarginsElement.Left = 0.5
                pageSetup.PageMarginsElement.Top = 0.5
                pageSetup.PageMarginsElement.Right = 0.5
                pageSetup.PageMarginsElement.Bottom = 0.5
                pageSetup.PageLayoutElement.PageOrientation = PageOrientationType.Landscape

                'Freeze panes
                e.Worksheet.WorksheetOptions.AllowFreezePanes = True
                e.Worksheet.WorksheetOptions.LeftColumnRightPaneNumber = 1
                e.Worksheet.WorksheetOptions.TopRowBottomPaneNumber = 1
                e.Worksheet.WorksheetOptions.SplitHorizontalOffset = 1
                e.Worksheet.WorksheetOptions.SplitVerticalOffest = 1

                e.Worksheet.WorksheetOptions.ActivePane = 2
                isExcelExportConfigured = True

            End If
        End If
    End Sub

    Protected Sub rgTelefon_ExcelMLExportStylesCreated(sender As Object, e As Telerik.Web.UI.GridExcelBuilder.GridExportExcelMLStyleCreatedArgs)

        'Add currency and percent styles
        Dim priceStyle As StyleElement = New StyleElement("priceItemStyle")
        priceStyle.NumberFormat.FormatType = NumberFormatType.Currency
        priceStyle.FontStyle.Color = System.Drawing.Color.Red
        e.Styles.Add(priceStyle)

        Dim alternatingPriceStyle As StyleElement = New StyleElement("alternatingPriceItemStyle")
        alternatingPriceStyle.NumberFormat.FormatType = NumberFormatType.Currency
        alternatingPriceStyle.FontStyle.Color = System.Drawing.Color.Red
        e.Styles.Add(alternatingPriceStyle)

        Dim percentStyle As StyleElement = New StyleElement("percentItemStyle")
        percentStyle.NumberFormat.FormatType = NumberFormatType.Percent
        percentStyle.FontStyle.Italic = True
        e.Styles.Add(percentStyle)

        Dim alternatingPercentStyle As StyleElement = New StyleElement("alternatingPercentItemStyle")
        alternatingPercentStyle.NumberFormat.FormatType = NumberFormatType.Percent
        alternatingPercentStyle.FontStyle.Italic = True
        e.Styles.Add(alternatingPercentStyle)

        'Apply background colors 
        For Each style As StyleElement In e.Styles
            If style.Id = "headerStyle" Then

                style.InteriorStyle.Pattern = InteriorPatternType.Solid
                style.InteriorStyle.Color = System.Drawing.Color.Gray

            End If
            If style.Id = "alternatingItemStyle" Or style.Id = "alternatingPriceItemStyle" Or style.Id = "alternatingPercentItemStyle" Or style.Id = "alternatingDateItemStyle" Then

                style.InteriorStyle.Pattern = InteriorPatternType.Solid
                style.InteriorStyle.Color = System.Drawing.Color.LightGray

            End If

            If style.Id.Contains("itemStyle") Or style.Id = "priceItemStyle" Or style.Id = "percentItemStyle" Or style.Id = "dateItemStyle" Then

                style.InteriorStyle.Pattern = InteriorPatternType.Solid
                style.InteriorStyle.Color = System.Drawing.Color.White

            End If
        Next
    End Sub

    'Protected Sub rgTelefon_ItemUpdated(ByVal source As Object, ByVal e As GridUpdatedEventArgs) Handles rgTelefon.ItemUpdated

    '    Dim item As GridEditableItem = DirectCast(e.Item, GridEditableItem)
    '    Dim id As String = item.GetDataKeyValue("ProductID").ToString()

    '    If Not e.Exception Is Nothing Then
    '        e.KeepInEditMode = True
    '        e.ExceptionHandled = True

    '    Else

    '    End If

    'End Sub

    Protected Sub lb_zurueck_Click(sender As Object, e As EventArgs) Handles lb_zurueck.Click
        Response.Redirect("../../../Start/Selection.aspx?AppID=" & Session("AppID").ToString, False)
    End Sub

    Protected Sub ddl_Berichtstyp_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ddl_Berichtstyp.SelectedIndexChanged
        Select Case ddl_Berichtstyp.SelectedValue
            Case "Tagesbericht"
                trVonDatum.Visible = True
                trBisDatum.Visible = False
                trBerichtszeitraum.Visible = False
                lbl_Datum_von.Text = "Bericht vom:"
            Case "Monatsbericht"
                trVonDatum.Visible = False
                trBisDatum.Visible = False
                trBerichtszeitraum.Visible = True
                ddl_Zeitraum.Visible = True
                ddl_Zeitraum.Items.Clear()
                For i As Integer = 1 To 12
                    ddl_Zeitraum.Items.Add(i.ToString())
                Next
                txtJahr.Text = Today.Year.ToString()
                lbl_ZeitraumBez.Text = "Monat:"
            Case "Quartalsbericht"
                trVonDatum.Visible = False
                trBisDatum.Visible = False
                trBerichtszeitraum.Visible = True
                ddl_Zeitraum.Visible = True
                ddl_Zeitraum.Items.Clear()
                For i As Integer = 1 To 4
                    ddl_Zeitraum.Items.Add(i.ToString())
                Next
                txtJahr.Text = Today.Year.ToString()
                lbl_ZeitraumBez.Text = "Quartal:"
            Case "Halbjahresbericht"
                trVonDatum.Visible = False
                trBisDatum.Visible = False
                trBerichtszeitraum.Visible = True
                ddl_Zeitraum.Visible = True
                ddl_Zeitraum.Items.Clear()
                For i As Integer = 1 To 2
                    ddl_Zeitraum.Items.Add(i.ToString())
                Next
                txtJahr.Text = Today.Year.ToString()
                lbl_ZeitraumBez.Text = "Halbjahr:"
            Case "Jahresbericht"
                trVonDatum.Visible = False
                trBisDatum.Visible = False
                trBerichtszeitraum.Visible = True
                ddl_Zeitraum.Visible = False
                lbl_ZeitraumBez.Text = "Jahr:"
            Case "Auswahl Periode"
                trVonDatum.Visible = True
                trBisDatum.Visible = True
                trBerichtszeitraum.Visible = False
                txt_Datum_bis.Text = Today.ToString("dd.MM.yyyy")
                lbl_Datum_von.Text = "Datum von:"
        End Select
    End Sub

End Class