Imports CKG.Base.Business
Imports CKG.Base.Kernel
Imports CKG.Base.Kernel.Common.Common
Imports System.Threading
Imports System.Globalization
Partial Public Class Report02Neu
    Inherits System.Web.UI.Page

    Private m_User As Security.User
    Private m_App As Security.App
    Protected WithEvents GridNavigation1 As Global.CKG.Services.GridNavigation
    Private objHandler As VFS02
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
       
    End Sub

    Private Sub GetObjHaendler()
        If Session("objVFS02") Is Nothing = False Then
            objHandler = Session("objVFS02")
            'Daten holen
            Dim tmpTable As DataTable

            tmpTable = objHandler.DetailTable
            If tmpTable Is Nothing Or tmpTable.Rows.Count = 0 Then
                objHandler.GiveData(Session("AppID").ToString, Session.SessionID, Me.Page)
            End If
        End If


    End Sub

    Private Sub FillGrid(ByVal intPageIndex As Int32, Optional ByVal strSort As String = "")
        UpdateSessionListOfCheckedVermittlernummern()

        GetObjHaendler()

        Dim tmpDataView As New DataView()
        tmpDataView = objHandler.OverviewTable.DefaultView

        tmpDataView.RowFilter = ""


        If tmpDataView.Count = 0 Then
            Result.Visible = False
            lblError.Text = "Keine Daten zur Anzeige gefunden."
            lblError.Visible = True
        Else
            DivPlaceholder.Visible = False
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

            GridView1.PageIndex = intTempPageIndex

            GridView1.DataSource = tmpDataView
            GridView1.DataBind()


            objHandler = Nothing

            Dim item As GridViewRow
            For Each item In GridView1.Rows


                'If (item. = ListItemType.Item) OrElse (item.DataItem.ItemType = ListItemType.AlternatingItem) Then

                'MouseOver HighLighting 



                'End If
                Dim cell As TableCell
                For Each cell In item.Cells
                    If IsDate(cell.Text) Then
                        cell.Text = CDate(cell.Text).ToShortDateString
                    End If
                Next
            Next
            If GridView1.PageCount > 1 Then
                GridView1.PagerStyle.CssClass = "PagerStyle"
                GridView1.DataBind()
                Result.Visible = True
            Else
                Result.Visible = False
            End If


            Dim reportExcel As DataTable

            GetObjHaendler()

            reportExcel = objHandler.OverviewTable.Copy

            reportExcel.AcceptChanges()

            Dim excelFactory As New DocumentGeneration.ExcelDocumentFactory()
            Dim sPath As String = ConfigurationManager.AppSettings("ExcelPath")
            Dim strFileName As String = Format(Now, "yyyyMMdd_HHmmss_") & m_User.UserName
            excelFactory.CreateDocumentAndWriteToFilesystemWithPath(sPath + strFileName, reportExcel)
            lnkCreateExcel1.NavigateUrl = "\Services\Temp\Excel\" & strFileName
            tab1.Visible = False
            btnConfirm.Visible = False
        End If
        SetVermittlernummerInDataGridCheckedOrUnchecked()
        SetCommandArgsForItemKennzeichenliste()
    End Sub
    'Protected Sub imgbSelectDateVon_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgbSelectDateVon.Click
    '    Try
    '        Try
    '            Me.cldSelect.SelectedDate = Date.Parse(txtDateVon.Text)
    '        Catch ex As Exception
    '            Me.cldSelect.SelectedDate = Date.Today
    '        End Try
    '        Me.cldSelect.Visible = True
    '        'CalendarDiv.Visible = True
    '        trCalendar.Visible = True

    '        Literal1.Text = "						<script language=""Javascript"">" & vbCrLf
    '        Literal1.Text &= "						  <!-- //" & vbCrLf
    '        Literal1.Text &= "							DisplayCalender('CalendarDiv');" & vbCrLf
    '        Literal1.Text &= "						  //-->" & vbCrLf
    '        Literal1.Text &= "						</script>" & vbCrLf

    '        imgbSelectDateVon.CommandArgument = "DateVon"
    '        imgbSelectDateBis.CommandArgument = String.Empty
    '    Catch ex As Exception
    '        lblError.Text = "Beim Öffnen der Datumsauswahl ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
    '    End Try
    'End Sub

    'Protected Sub imgbSelectDateBis_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgbSelectDateBis.Click
    '    Try
    '        Try
    '            Me.cldSelect2.SelectedDate = Date.Parse(txtDateBis.Text)
    '        Catch ex As Exception
    '            Me.cldSelect2.SelectedDate = Date.Today
    '        End Try
    '        'CalendarDiv2.Visible = True
    '        Me.cldSelect2.Visible = True
    '        trCalendar.Visible = True

    '        Literal1.Text = "						<script language=""Javascript"">" & vbCrLf
    '        Literal1.Text &= "						  <!-- //" & vbCrLf
    '        Literal1.Text &= "							DisplayCalender('CalendarDiv2');" & vbCrLf
    '        Literal1.Text &= "						  //-->" & vbCrLf
    '        Literal1.Text &= "						</script>" & vbCrLf

    '        imgbSelectDatebis.CommandArgument = "DateBis"
    '        imgbSelectDateVon.CommandArgument = String.Empty
    '    Catch ex As Exception
    '        lblError.Text = "Beim Öffnen der Datumsauswahl ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
    '    End Try
    'End Sub
#Region "Private methods"
    'Erzeugen des Excel-Sheets für die selektierten Adressen
    Private Sub CreateExcelReportForSelectedAddresses()
        Dim objExcelExport As New Excel.ExcelExport()
        Dim strfilename As String = ConfigurationManager.AppSettings("ExcelPath") & Format(Now, "yyyyMMdd_HHmmss_") & m_User.UserName & ".xls"

        objHandler = Session("objVFS02")
        Dim reportExcel As DataTable = objHandler.AddressTable.Copy()
        Try
            reportExcel.Columns(0).ColumnName = "VD-Bezirk"
            reportExcel.AcceptChanges()
            Excel.ExcelExport.WriteExcel(reportExcel, strfilename)
            Session("lnkExcel") = strfilename
        Catch
        End Try
    End Sub


    'Erzeugen des Excel-Sheets für die selektierte Kennzeichenliste eines Vermittlers
    Private Sub CreateExcelReportForSelectedKennzeichenliste(ByVal pTblKennzeichen As DataTable)
        Dim objExcelExport As New Excel.ExcelExport()
        Dim strfilename As String = ConfigurationManager.AppSettings("ExcelPath") & Format(Now, "yyyyMMdd_HHmmss_") & m_User.UserName & ".xls"

        Try
            Excel.ExcelExport.WriteExcel(pTblKennzeichen, strfilename)
            Session("lnkExcel") = strfilename
        Catch
        End Try
    End Sub

    'Anzeige der selektierten Adressen
    Private Sub ShowReportSelectedAddresses()
        Dim listOfVermittlernummer As New System.Collections.ArrayList
        Dim countAddressesNotFound As Int32

        If Not TryGetListOfVermittlernummernFromSession(listOfVermittlernummer) Then Exit Sub

        If listOfVermittlernummer.Count > 0 Then
            objHandler = Session("objVFS02")
            objHandler.GiveAddressData(listOfVermittlernummer, Session("AppID").ToString, Session.SessionID)
            Session("ResultTable") = objHandler.AddressTable
            countAddressesNotFound = GetCountOfAddressesNotFound(objHandler.AddressTable)
            Session("ShowOtherString") = BuildOutputStringForAddressReport(objHandler.AddressTable.Rows.Count, countAddressesNotFound)
            CreateExcelReportForSelectedAddresses()
            Response.Redirect("/Services/(S(" & Session.SessionID & "))/Shared/Report01_2.aspx?AppID=" & Session("AppID").ToString() & "&legende=AppVFS-ADR")
        End If
    End Sub

    'Anzeige der Kennzeichenliste für einen Vermittler, eine Auftragsnummer und ein Versanddatum
    Private Sub ShowReportKennzeichenliste(ByVal pTblKennzeichen As DataTable)

        If pTblKennzeichen.Rows.Count > 0 Then
            Session("ResultTable") = pTblKennzeichen
            'Session("ShowOtherString") = BuildOutputStringForAddressReport(objHandler.AddressTable.Rows.Count, countAddressesNotFound)
            CreateExcelReportForSelectedKennzeichenliste(pTblKennzeichen)
            Response.Redirect("/Services/(S(" & Session.SessionID & "))/Shared/Report01_2.aspx?AppID=" & Session("AppID").ToString() & "&legende=AppVFS-KZL")
        End If
    End Sub

    'Pflegt die Liste der Vermittlernummern, deren Adressen angezeigt werden sollen.
    'Count ist 0 wenn keine Checkbox selektiert wurde. 
    Private Sub UpdateSessionListOfCheckedVermittlernummern(Optional ByVal showAllAddr As Boolean = False)
        Dim listOfVermittlernummern As New System.Collections.ArrayList
        Dim isNewList As Boolean = False

        If Not TryGetListOfVermittlernummernFromSession(listOfVermittlernummern) Then
            listOfVermittlernummern = New System.Collections.ArrayList()
            isNewList = True
        End If

        Dim item As GridViewRow
        Dim idxColVermittlernummer As Integer = GetDataGridColumnIndexByHeaderText(GridView1, "VD-Bezirk")
        Dim idxColVersantdatum As Integer = GetDataGridColumnIndexByHeaderText(GridView1, "Versand am")
        For Each item In GridView1.Rows
            'Werte ermitteln

            Dim cell As TableCell
            Dim tmpVermittlernummer As String
            Dim tmpVersDate As String
            For Each cell In item.Cells

                Dim c As System.Web.UI.Control
                For Each c In cell.Controls

                    If TypeOf c Is CheckBox Then
                        Dim chk As CheckBox = CType(c, CheckBox)

                        If chk.ID = "chkAdresseAnzeigen" Then
                            tmpVermittlernummer = item.Cells(idxColVermittlernummer).Text
                            tmpVersDate = item.Cells(idxColVersantdatum).Text
                            If chk.Checked OrElse showAllAddr Then
                                If Not listOfVermittlernummern.Contains(tmpVermittlernummer & "#" & tmpVersDate) Then
                                    listOfVermittlernummern.Add(tmpVermittlernummer & "#" & tmpVersDate)
                                End If
                            ElseIf listOfVermittlernummern.Contains(tmpVermittlernummer & "#" & tmpVersDate) Then
                                listOfVermittlernummern.Remove(tmpVermittlernummer & "#" & tmpVersDate)
                            End If
                        End If
                    End If

                Next

            Next

        Next

        If isNewList Then
            Session("App_ADR_VFS02") = listOfVermittlernummern
        End If

    End Sub

    'Liefert den Spaltenindex anhand einer Spaltenüberschrift
    'Achtung: Diese Funktion löst eine Exception aus, wenn die Spalte nicht gefunden wird
    Private Function GetDataGridColumnIndexByHeaderText(ByVal pDataGrid As GridView, ByVal pHeaderText As String) As Integer
        Dim tmpColumn As BoundField
        Dim i As Int32 = 0

        For Each tmpColumn In pDataGrid.Columns
            If tmpColumn.HeaderText = pHeaderText Then
                Return i
            End If
            i = i + 1
        Next

        Throw New Exception("Die Spalte mit der Überschrift " & pHeaderText & " wurde nicht gefunden.")

    End Function

    'Setzt den Wert der Checkboxen für die Adressauswahl im DataGrid
    Private Sub SetVermittlernummerInDataGridCheckedOrUnchecked()
        Dim listOfVermittlernummern As New System.Collections.ArrayList

        If Not TryGetListOfVermittlernummernFromSession(listOfVermittlernummern) Then Exit Sub
        If listOfVermittlernummern.Count = 0 Then Exit Sub

        Dim item As GridViewRow
        For Each item In GridView1.Rows
            'Werte ermitteln

            Dim cell As TableCell
            Dim idxColVermittlernummer As Integer = GetDataGridColumnIndexByHeaderText(GridView1, "VD-Bezirk")
            Dim idxColVersantdatum As Integer = GetDataGridColumnIndexByHeaderText(GridView1, "Versand am")
            For Each cell In item.Cells

                Dim c As System.Web.UI.Control
                For Each c In cell.Controls

                    If TypeOf c Is CheckBox Then
                        Dim chk As CheckBox = CType(c, CheckBox)

                        If chk.ID = "chkAdresseAnzeigen" Then
                            Dim tmpString As String = item.Cells(idxColVermittlernummer).Text & "#" & item.Cells(idxColVersantdatum).Text
                            If listOfVermittlernummern.Contains(tmpString) Then
                                chk.Checked = True
                            Else
                                chk.Checked = False
                            End If
                        End If
                    End If

                Next

            Next

        Next

    End Sub
    Private Sub SetVermittlernummerKennz(ByVal listOfVermittlernummern As System.Collections.ArrayList)

        If listOfVermittlernummern.Count = 0 Then Exit Sub

        Dim item As GridViewRow
        For Each item In GridView1.Rows
            'Werte ermitteln

            Dim cell As TableCell
            Dim idxColVermittlernummer As Integer = GetDataGridColumnIndexByHeaderText(GridView1, "VD-Bezirk")
            Dim idxColVersantdatum As Integer = GetDataGridColumnIndexByHeaderText(GridView1, "Versand am")
            Dim idxColAuftrag As Integer = GetDataGridColumnIndexByHeaderText(GridView1, "Auftragsnummer")
            For Each cell In item.Cells

                Dim c As System.Web.UI.Control
                For Each c In cell.Controls

                    If TypeOf c Is CheckBox Then
                        Dim chk As CheckBox = CType(c, CheckBox)

                        If chk.ID = "chkKennzAnzeigen" Then
                            Dim tmpString As String = item.Cells(idxColVermittlernummer).Text & "#" & item.Cells(idxColVersantdatum).Text & "#" & item.Cells(idxColAuftrag).Text
                            If listOfVermittlernummern.Contains(tmpString) Then
                                chk.Checked = True
                            Else
                                chk.Checked = False
                            End If
                        End If
                    End If

                Next

            Next

        Next

    End Sub
    'Setzt den Wert der Checkboxen für die Adressauswahl im DataGrid
    Private Sub SetCommandArgsForItemKennzeichenliste()

        Dim item As GridViewRow
        For Each item In GridView1.Rows
            'Werte ermitteln

            Dim cell As TableCell
            Dim idxColVermittlernummer As Integer = GetDataGridColumnIndexByHeaderText(GridView1, "VD-Bezirk")
            Dim idxColVertragsnummer As Integer = GetDataGridColumnIndexByHeaderText(GridView1, "Auftragsnummer")
            Dim idxColVersanddatum As Integer = GetDataGridColumnIndexByHeaderText(GridView1, "Versand am")
            For Each cell In item.Cells

                Dim c As System.Web.UI.Control
                For Each c In cell.Controls

                    If TypeOf c Is LinkButton Then
                        Dim lnk As LinkButton = CType(c, LinkButton)


                        If lnk.ID = "lnkKennzeichenliste" Then
                            lnk.CommandArgument = item.Cells(idxColVermittlernummer).Text & ";" & _
                            item.Cells(idxColVertragsnummer).Text & ";" & _
                            item.Cells(idxColVersanddatum).Text
                        End If

                    End If

                Next

            Next

        Next

    End Sub

    'Liefert 'TRUE', wenn Vermittlernummern als Sessionvariable existieren. In diesem
    'Fall wird der übergebene Parameter mit der Liste aus der Sessionvariablen gefüllt.
    'Wenn keine keine Sessionvariable existiert, wird 'FALSE' geliefert und der übergebene
    'Parameter bleibt unveränder
    Private Function TryGetListOfVermittlernummernFromSession(ByRef pListOfVermittlernummern As System.Collections.ArrayList) As Boolean
        Dim tmpListOfVermittlernummern As System.Collections.ArrayList

        tmpListOfVermittlernummern = CType(Session("App_ADR_VFS02"), System.Collections.ArrayList)

        If Not tmpListOfVermittlernummern Is Nothing Then
            pListOfVermittlernummern = tmpListOfVermittlernummern
            Return True
        End If
        Return False
    End Function

    'Liefert die Anzahl der Vermittler, für die keine Addressdaten gefunden wurden
    Private Function GetCountOfAddressesNotFound(ByVal pAddressTable As DataTable) As Int32
        If pAddressTable.Rows.Count = 0 Then
            Return 0
        End If

        Dim aRow As DataRow
        Dim aCount As Int32 = 0

        For Each aRow In pAddressTable.Rows
            If aRow("Ort") = VFS02.CONST_NO_ADDRESS_DATA Then
                aCount = aCount + 1
            End If
        Next
        Return aCount
    End Function

    'Erzeugt die Ausgabemeldung für den Addressreport
    Private Function BuildOutputStringForAddressReport(ByVal pCountItems As Int32, ByVal pCountAddressesNotFound As Int32) As String
        Dim outputString1 As String
        Dim outputString2 As String

        If pCountItems = pCountAddressesNotFound Then
            outputString1 = "Für die ausgewählten Vermittler in 'Durchgeführte Versendungen' wurden keine Adressdaten gefunden."
            Return outputString1
        End If

        If pCountItems - pCountAddressesNotFound > 1 Then
            outputString1 = String.Format("Es wurden {0} Adressen für 'Durchgeführte Versendungen' gefunden.", pCountItems - pCountAddressesNotFound)
        Else
            outputString1 = "Es wurde 1 Adresse für 'Durchgeführte Versendungen' gefunden."
        End If

        outputString2 = ""
        If pCountAddressesNotFound = 1 Then
            outputString2 = " Für 1 Vermittler wurde keine Adresse gefunden."
        ElseIf pCountAddressesNotFound > 1 Then
            outputString2 = String.Format(" Für {0} Vermittler wurden keine Adressen gefunden.", pCountAddressesNotFound)
        End If

        Return outputString1 & outputString2
    End Function

    Private Sub ProcessDataGridItemCommandKennzeichenliste(ByVal pFilterExpression As String)
        Dim tmpVermittlernummer As Int64
        Dim tmpAuftragsnummer As String
        Dim tmpVersanddatum As Date
        Dim tmpArray() As String = pFilterExpression.Split(";")
        Dim tmpTable As New DataTable

        tmpVermittlernummer = Int64.Parse(tmpArray.GetValue(0))
        tmpAuftragsnummer = tmpArray.GetValue(1)
        tmpVersanddatum = Date.Parse(tmpArray.GetValue(2))

        'tmpTable = CreateDataTableKennzeichenliste(tmpVermittlernummer, tmpAuftragsnummer, tmpVersanddatum)

        ShowReportKennzeichenliste(tmpTable)
    End Sub

    Private Sub SessionListVermnr_Kennz(Optional ByVal showAllKennz As Boolean = False)
        Dim listOfVermittlernummern As System.Collections.ArrayList
        Dim isNewList As Boolean = False
        listOfVermittlernummern = Session("App_ListVermKennz")
        If IsNothing(listOfVermittlernummern) Then
            listOfVermittlernummern = New System.Collections.ArrayList()
            isNewList = True
        End If

        Dim item As GridViewRow
        Dim idxColVermittlernummer As Integer = GetDataGridColumnIndexByHeaderText(GridView1, "VD-Bezirk")
        Dim idxColVersand As Integer = GetDataGridColumnIndexByHeaderText(GridView1, "Versand am")
        Dim idxColAuftrag As Integer = GetDataGridColumnIndexByHeaderText(GridView1, "Auftragsnummer")
        For Each item In GridView1.Rows
            'Werte ermitteln

            Dim cell As TableCell
            Dim tmpVermittlernummer As String
            Dim tmpVersand As String
            Dim tmpAuftrag As String
            For Each cell In item.Cells

                Dim c As System.Web.UI.Control
                For Each c In cell.Controls

                    If TypeOf c Is CheckBox Then
                        Dim chk As CheckBox = CType(c, CheckBox)

                        If chk.ID = "chkKennzAnzeigen" Then
                            tmpVermittlernummer = item.Cells(idxColVermittlernummer).Text
                            tmpVersand = item.Cells(idxColVersand).Text
                            tmpAuftrag = item.Cells(idxColAuftrag).Text
                            tmpVermittlernummer = tmpVermittlernummer & "#" & tmpVersand & "#" & tmpAuftrag

                            If chk.Checked OrElse showAllKennz Then
                                If Not listOfVermittlernummern.Contains(tmpVermittlernummer) Then
                                    listOfVermittlernummern.Add(tmpVermittlernummer)
                                End If
                            ElseIf listOfVermittlernummern.Contains(tmpVermittlernummer) Then
                                listOfVermittlernummern.Remove(tmpVermittlernummer)
                            End If
                        End If
                    End If

                Next

            Next

        Next

        If isNewList Then
            Session("App_ListVermKennz") = listOfVermittlernummern
        End If

    End Sub
    Private Function CreateDataTableKennzeichenliste(ByVal listOfVermittlernummer As System.Collections.ArrayList) As DataTable
        Dim tmpRow As DataRow
        Dim tmpTable As DataTable
        Dim newRow As DataRow
        Dim sTemp() As String
        Dim tmpVermittlernummer As String = ""
        Dim tmpVersand As String = ""
        Dim tmpAuftrag As String = ""
        Dim i As Integer
        tmpTable = New DataTable()
        With tmpTable.Columns
            .Add("Kennzeichen", System.Type.GetType("System.String"))
            .Add("Vertragsnummer", System.Type.GetType("System.String"))
            .Add("Auftragsnummer", System.Type.GetType("System.String"))
            .Add("Versand am", System.Type.GetType("System.DateTime"))
            .Add("Verkauf am", System.Type.GetType("System.DateTime"))
            .Add("Rücklauf am", System.Type.GetType("System.DateTime"))
            .Add("Verlust am", System.Type.GetType("System.DateTime"))
            .Add("VD-Bezirk", System.Type.GetType("System.Int64"))
            .Add("Name1", System.Type.GetType("System.String"))
            .Add("Name2", System.Type.GetType("System.String"))
        End With
        If listOfVermittlernummer.Count > 0 Then

            For i = 0 To listOfVermittlernummer.Count - 1
                sTemp = Split(listOfVermittlernummer.Item(i), "#")
                tmpVermittlernummer = sTemp(0)
                tmpVersand = sTemp(1)
                tmpAuftrag = sTemp(2)

                For Each tmpRow In objHandler.DetailTable.Rows

                    If tmpVermittlernummer = tmpRow("VD-Bezirk").ToString Then
                        If tmpAuftrag = tmpRow("Auftragsnummer").ToString Then
                            If tmpVersand = Date.Parse(tmpRow("Versand am").ToString) Then
                                newRow = tmpTable.NewRow()
                                newRow("Kennzeichen") = tmpRow("Kennzeichen")
                                newRow("Vertragsnummer") = tmpRow("Vertragsnummer")
                                newRow("Versand am") = tmpRow("Versand am")
                                newRow("Verkauf am") = tmpRow("Verkauf am")
                                newRow("Rücklauf am") = tmpRow("Rücklauf am")
                                newRow("Verlust am") = tmpRow("Verlust am")
                                newRow("VD-Bezirk") = tmpRow("VD-Bezirk")
                                newRow("Name1") = tmpRow("Name1")
                                newRow("Name2") = tmpRow("Name2")
                                tmpTable.Rows.Add(newRow)
                            End If
                        End If
                    End If
                Next
            Next
        End If
        tmpTable.DefaultView.Sort = "Kennzeichen"
        tmpTable.Columns.Remove("Auftragsnummer")
        tmpTable.AcceptChanges()
        Return tmpTable
    End Function



    'Erzeugt ein Datum im SQL-Format
    Private Function CreateSQLDateFromDateTime(ByVal pDate As DateTime) As String
        Dim sqlDateString As String

        sqlDateString = String.Format("#{0}/{1}/{2}#", pDate.Month, pDate.Day, pDate.Year)
        Return sqlDateString
    End Function
#End Region

    Private Function ClearBrForExcelOutput(ByVal strIN As String) As String
        Dim strOut As String = Replace(strIN, "-<br>", "")
        strOut = Replace(strOut, " -<br>", "")
        strOut = Replace(strOut, "- <br>", "")
        strOut = Replace(strOut, " - <br>", "")
        strOut = Replace(strOut, "<br>", " ")
        Return strOut
    End Function


    Private Sub GridView1_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles GridView1.RowCommand
        If e.CommandName = "AdressenAnzeigen" Then
            UpdateSessionListOfCheckedVermittlernummern()
            ShowReportSelectedAddresses()
        ElseIf e.CommandName = "AlleAdressenAnzeigen" Then
            UpdateSessionListOfCheckedVermittlernummern(True)
            ShowReportSelectedAddresses()
        ElseIf e.CommandName = "KennzAnzeigen" Then
            Dim tmpTable As DataTable
            SessionListVermnr_Kennz()
            Dim listOfVermittlernummer As System.Collections.ArrayList
            listOfVermittlernummer = Session("App_ListVermKennz")
            tmpTable = CreateDataTableKennzeichenliste(listOfVermittlernummer)
            ShowReportKennzeichenliste(tmpTable)
        ElseIf e.CommandName = "AlleKennzAnzeigen" Then
            Dim tmpTable As DataTable
            SessionListVermnr_Kennz(True)
            Dim listOfVermittlernummer As System.Collections.ArrayList
            listOfVermittlernummer = Session("App_ListVermKennz")
            tmpTable = CreateDataTableKennzeichenliste(listOfVermittlernummer)
            ShowReportKennzeichenliste(tmpTable)
        ElseIf String.Compare(e.CommandName, "Kennzeichenliste") = 0 Then
            ProcessDataGridItemCommandKennzeichenliste(e.CommandArgument)
        End If
    End Sub

    Private Sub GridView1_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles GridView1.RowDataBound
        If e.Row.RowType = DataControlRowType.Header Then
            Dim ctrl As Control
            Dim tmpddl As DropDownList
            For Each ctrl In e.Row.Cells(6).Controls
                If ctrl.ID = "ddlGridKennz" Then
                    tmpddl = CType(ctrl, DropDownList)
                    tmpddl.SelectedIndex = 0
                    Exit For
                End If
            Next
            For Each ctrl In e.Row.Cells(7).Controls
                If ctrl.ID = "ddlGridAdr" Then
                    tmpddl = CType(ctrl, DropDownList)
                    tmpddl.SelectedIndex = 0
                    Exit For
                End If
            Next
        End If
    End Sub

    Private Sub GridView1_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles GridView1.Sorting
        FillGrid(GridView1.PageIndex, e.SortExpression)
    End Sub
    Private Function IsPageValid() As Boolean

        'Wenn die Suche über eingegebene Kennzeichen stattfinden soll
        'werden die anderen Eingaben nicht mehr geprüft
        If txtKennzeichen.Text.Trim().Length > 0 Then
            Return True
        End If

        If txtOrgNr.Text.Trim().Length = 0 Then
            lblError.Text = "Eingabe von mindestens 2 Zeichen inkl. * für die Organistionsnummer erforderlich."
            Return False
        End If

        Dim tmpStr As String = ""
        If Not HelpProcedures.checkDate(txtDateVon, txtDateBis, tmpStr, True) Then
            lblError.Text = tmpStr
            Return False
        End If


        Return True
    End Function

    Private Sub Report02Neu_PreLoad(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreLoad
        m_User = GetUser(Me)
        FormAuth(Me, m_User)
        Session("ShowOtherString") = Nothing
        GridNavigation1.setGridElment(GridView1)
        GetAppIDFromQueryString(Me)

        Try
            lblHead.Text = m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString

            m_App = New Security.App(m_User)

            objHandler = Session("objVFS02")
            lblError.Text = ""

            If IsPostBack Then
                If Not Session("objVFS02") Is Nothing Then
                    objHandler = CType(Session("objVFS02"), VFS02)

                    If objHandler.Versicherungsjahr <> txtVJahr.Text Then
                        objHandler.Versicherungsjahr = txtVJahr.Text
                    Else
                        txtVJahr.Text = objHandler.Versicherungsjahr
                    End If
                    If txtOrgNr.Text <> objHandler.OrgNr Then
                        objHandler.OrgNr = txtOrgNr.Text
                    End If

                    If IsDate(objHandler.DatumVon) Then
                        If txtDateVon.Text <> objHandler.DatumVon Then
                            objHandler.DatumVon = txtDateVon.Text
                        End If
                    End If
                    If IsDate(objHandler.DatumBis) Then
                        If txtDateVon.Text <> objHandler.DatumBis Then
                            objHandler.DatumBis = txtDateBis.Text
                        End If
                    End If

                End If
            End If

        Catch ex As Exception
            lblError.Text = "Beim Laden der Seite ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
        End Try
    End Sub

    Protected Sub btnConfirm_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnConfirm.Click



        If Not IsPageValid() Then Exit Sub
        objHandler = New VFS02(m_User, m_App, "")

        objHandler.Versicherungsjahr = txtVJahr.Text.Trim()
        objHandler.OrgNr = txtOrgNr.Text.Trim()
        objHandler.Kennzeichen = txtKennzeichen.Text.Trim().ToUpper

        objHandler.DatumVon = txtDateVon.Text.Trim
        objHandler.DatumBis = txtDateBis.Text.Trim


        objHandler.GiveData(Session("AppID").ToString, Session.SessionID, Me.Page)

        Session("objVFS02") = objHandler
        FillGrid(0)



        Dim tmpListeVermittlernummer As New System.Collections.ArrayList
        If TryGetListOfVermittlernummernFromSession(tmpListeVermittlernummer) Then
            SetVermittlernummerInDataGridCheckedOrUnchecked()
        End If
    End Sub

    Sub ddlIndexChanging(ByVal sender As Object, ByVal e As System.EventArgs)

        Dim tempddl As DropDownList

        tempddl = CType(sender, DropDownList)
        If tempddl.SelectedValue = "3" Then
            UpdateSessionListOfCheckedVermittlernummern()
            ShowReportSelectedAddresses()
        ElseIf tempddl.SelectedValue = "4" Then
            UpdateSessionListOfCheckedVermittlernummern(True)
            ShowReportSelectedAddresses()
        ElseIf tempddl.SelectedValue = "2" Then
            Dim tmpTable As DataTable
            SessionListVermnr_Kennz()
            Dim listOfVermittlernummer As System.Collections.ArrayList
            listOfVermittlernummer = Session("App_ListVermKennz")
            tmpTable = CreateDataTableKennzeichenliste(listOfVermittlernummer)
            ShowReportKennzeichenliste(tmpTable)
        ElseIf tempddl.SelectedValue = "1" Then
            Dim tmpTable As DataTable
            SessionListVermnr_Kennz(True)
            Dim listOfVermittlernummer As System.Collections.ArrayList
            listOfVermittlernummer = Session("App_ListVermKennz")
            tmpTable = CreateDataTableKennzeichenliste(listOfVermittlernummer)
            ShowReportKennzeichenliste(tmpTable)
            'ElseIf tempddl.SelectedValue Then
            '    ProcessDataGridItemCommandKennzeichenliste(e.CommandArgument)
        End If
    End Sub
    Private Sub GridNavigation1_PagerChanged(ByVal PageIndex As Integer) Handles GridNavigation1.PagerChanged
        FillGrid(PageIndex)
    End Sub

    Private Sub GridNavigation1_PageSizeChanged() Handles GridNavigation1.PageSizeChanged
        FillGrid(0)
    End Sub
 
    Private Sub NewSearch_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles NewSearch.Click
        tab1.Visible = Not tab1.Visible
        btnConfirm.Visible = Not btnConfirm.Visible
    End Sub

    Protected Sub lbBack_Click(sender As Object, e As EventArgs) Handles lbBack.Click
        Response.Redirect("../../../Start/Selection.aspx?AppID=" & Session("AppID").ToString, False)
    End Sub
End Class