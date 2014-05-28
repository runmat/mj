Imports CKG.Base.Business
Imports CKG.Base.Kernel
Imports CKG.Base.Kernel.Common.Common

Partial Public Class Report03New
    Inherits System.Web.UI.Page
    Private objHandler As VFS02
    Private m_App As Security.App
    Private m_User As Security.User
    Protected WithEvents GridNavigation1 As Global.CKG.Services.GridNavigation

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

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
            Result.Visible = True
            'lblDownloadTip.Visible = True
            'lnkCreateExcel.Visible = True
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
            Session("objVFS02") = objHandler
            objHandler = Nothing
            tab1.Visible = False
            btnConfirm.Visible = False
        End If
        SetVermittlernummerInDataGridCheckedOrUnchecked()
        SetCommandArgsForItemKennzeichenliste()

    End Sub



#Region "Private methods"
    'Erzeugen des Excel-Sheets für die selektierten Adressen
    Private Sub CreateExcelReportForSelectedAddresses()
        Dim objExcelExport As New Excel.ExcelExport()
        Dim strfilename As String = ConfigurationManager.AppSettings("ExcelPath") & Format(Now, "yyyyMMdd_HHmmss_") & m_User.UserName & ".xls"

        objHandler = Session("objVFS02")
        Dim reportExcel As DataTable = objHandler.AddressTable.Copy()
        Try
            reportExcel.Columns(0).ColumnName = "Agenturnr."
            reportExcel.AcceptChanges()
            Excel.ExcelExport.WriteExcel(reportExcel, strfilename)
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
        Dim idxColVermittlernummer As Integer = GetDataGridColumnIndexByHeaderText(GridView1, "Agenturnr.")
        For Each item In GridView1.Rows
            'Werte ermitteln

            Dim cell As TableCell
            Dim tmpVermittlernummer As String
            For Each cell In item.Cells

                Dim c As System.Web.UI.Control
                For Each c In cell.Controls

                    If TypeOf c Is CheckBox Then
                        Dim chk As CheckBox = CType(c, CheckBox)

                        If chk.ID = "chkAdresseAnzeigen" Then
                            tmpVermittlernummer = item.Cells(idxColVermittlernummer).Text
                            If chk.Checked OrElse showAllAddr Then
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
            Session("App_ADR_VFS02") = listOfVermittlernummern
        End If

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
        Dim idxColVermittlernummer As Integer = GetDataGridColumnIndexByHeaderText(GridView1, "Agenturnr.")
        For Each item In GridView1.Rows
            'Werte ermitteln

            Dim cell As TableCell
            Dim tmpVermittlernummer As String
            For Each cell In item.Cells

                Dim c As System.Web.UI.Control
                For Each c In cell.Controls

                    If TypeOf c Is CheckBox Then
                        Dim chk As CheckBox = CType(c, CheckBox)

                        If chk.ID = "chkKennzAnzeigen" Then
                            tmpVermittlernummer = item.Cells(idxColVermittlernummer).Text
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
    'Liefert den Spaltenindex anhand einer Spaltenüberschrift
    'Achtung: Diese Funktion löst eine Exception aus, wenn die Spalte nicht gefunden wird
    Private Function GetDataGridColumnIndexByHeaderText(ByVal pDataGrid As GridView, ByVal pHeaderText As String) As Integer
        Dim tmpColumn As DataControlField
        Dim i As Int32 = 0

        For Each tmpColumn In pDataGrid.Columns
            If tmpColumn.HeaderText = pHeaderText Then
                Return i
            End If
            i = i + 1
        Next

        Throw New Exception("Die Spalte mit der Überschrift " & pHeaderText & " wurde nicht gefunden.")

    End Function
    Private Sub SetVermittlernummerKennz(ByVal listOfVermittlernummern As System.Collections.ArrayList)

        If listOfVermittlernummern.Count = 0 Then Exit Sub

        Dim item As GridViewRow
        For Each item In GridView1.Rows
            'Werte ermitteln

            Dim cell As TableCell
            Dim idxColVermittlernummer As Integer = GetDataGridColumnIndexByHeaderText(GridView1, "Agenturnr.")
            For Each cell In item.Cells

                Dim c As System.Web.UI.Control
                For Each c In cell.Controls

                    If TypeOf c Is CheckBox Then
                        Dim chk As CheckBox = CType(c, CheckBox)

                        If chk.ID = "chkKennzAnzeigen" Then
                            Dim tmpString As String = item.Cells(idxColVermittlernummer).Text
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
    Private Sub SetVermittlernummerInDataGridCheckedOrUnchecked()
        Dim listOfVermittlernummern As New System.Collections.ArrayList

        If Not TryGetListOfVermittlernummernFromSession(listOfVermittlernummern) Then Exit Sub
        If listOfVermittlernummern.Count = 0 Then Exit Sub

        Dim item As GridViewRow
        For Each item In GridView1.Rows
            'Werte ermitteln

            Dim cell As TableCell
            Dim idxColVermittlernummer As Integer = GetDataGridColumnIndexByHeaderText(GridView1, "Agenturnr.")
            For Each cell In item.Cells

                Dim c As System.Web.UI.Control
                For Each c In cell.Controls

                    If TypeOf c Is CheckBox Then
                        Dim chk As CheckBox = CType(c, CheckBox)

                        If chk.ID = "chkAdresseAnzeigen" Then
                            If listOfVermittlernummern.Contains(item.Cells(idxColVermittlernummer).Text) Then
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
            Dim idxColVermittlernummer As Integer = GetDataGridColumnIndexByHeaderText(GridView1, "Agenturnr.")
            For Each cell In item.Cells

                Dim c As System.Web.UI.Control
                For Each c In cell.Controls

                    If TypeOf c Is LinkButton Then
                        Dim lnk As LinkButton = CType(c, LinkButton)
                        If lnk.ID = "lnkKennzeichenliste" Then
                            lnk.CommandArgument = item.Cells(idxColVermittlernummer).Text
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
        Dim tmpTable As New DataTable

        tmpVermittlernummer = Int64.Parse(pFilterExpression)

        'tmpTable = CreateDataTableKennzeichenliste(tmpVermittlernummer)

        ShowReportKennzeichenliste(tmpTable)
    End Sub

    'Hier wird aus der Gesamtmenge der Tabelle Details eine DataTable mit den
    'Datensätzen erzeugt, die den ausgewählten Werten für Vermittlernummer entsprechen
    Private Function CreateDataTableKennzeichenliste(ByVal listOfVermittlernummer As System.Collections.ArrayList) As DataTable
        Dim tmpRow As DataRow
        Dim tmpTable As DataTable
        Dim newRow As DataRow
        Dim tmpVermittlernummer As String
        Dim i As Integer

        GetObjHaendler()

        tmpTable = New DataTable()
        With tmpTable.Columns
            .Add("Kennzeichen", System.Type.GetType("System.String"))
            .Add("Bestellnummer", System.Type.GetType("System.String"))
            .Add("Versand am", System.Type.GetType("System.DateTime"))
            .Add("Verkauf am", System.Type.GetType("System.DateTime"))
            .Add("Rücklauf am", System.Type.GetType("System.DateTime"))
            .Add("Verlust/Storno", System.Type.GetType("System.DateTime"))
            .Add("Agenturnr.", System.Type.GetType("System.String"))
            .Add("Name1", System.Type.GetType("System.String"))
            .Add("Name2", System.Type.GetType("System.String"))
        End With
        If listOfVermittlernummer.Count > 0 Then

            For i = 0 To listOfVermittlernummer.Count - 1
                tmpVermittlernummer = listOfVermittlernummer.Item(i)
                For Each tmpRow In objHandler.DetailTable.Rows
                    If tmpVermittlernummer = tmpRow("VD-Bezirk").ToString Then
                        newRow = tmpTable.NewRow()
                        newRow("Kennzeichen") = tmpRow("Kennzeichen")
                        newRow("Bestellnummer") = tmpRow("Bestellnummer")
                        newRow("Versand am") = tmpRow("Versand am")
                        newRow("Verkauf am") = tmpRow("Verkauf am")
                        newRow("Rücklauf am") = tmpRow("Rücklauf am")
                        newRow("Verlust/Storno") = tmpRow("Verlust/Storno")
                        newRow("Agenturnr.") = tmpRow("VD-Bezirk")
                        newRow("Name1") = tmpRow("Name1")
                        newRow("Name2") = tmpRow("Name2")
                        tmpTable.Rows.Add(newRow)
                    End If
                Next
            Next
        End If
        tmpTable.DefaultView.Sort = "Kennzeichen"
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
        ElseIf e.CommandName = "KennzAnzeigen" Then
            Dim tmpTable As DataTable
            SessionListVermnr_Kennz()
            Dim listOfVermittlernummer As System.Collections.ArrayList
            listOfVermittlernummer = Session("App_ListVermKennz")
            tmpTable = CreateDataTableKennzeichenliste(listOfVermittlernummer)
            ShowReportKennzeichenliste(tmpTable)
        End If
    End Sub


    Private Sub btnConfirm2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnConfirm.Click

        Dim Orgnummer As String = ""
        Dim sNummerTrenn As String = ""
        For i As Integer = 1 To txtOrgNr.Text.Length


            If Not (Mid(txtOrgNr.Text, i, 1)) = "-" AndAlso Not (Mid(txtOrgNr.Text, i, 1)) = "_" Then
                sNummerTrenn = txtOrgNr.Text
                Orgnummer &= Mid(txtOrgNr.Text, i, 1)
            End If

        Next


        If Not IsPageValid(Orgnummer) Then Exit Sub

        objHandler = New VFS02(m_User, m_App, "")

        objHandler.Versicherungsjahr = txtVJahr.Text
        objHandler.OrgNr = Orgnummer.Trim()
        objHandler.Kennzeichen = txtKennzeichen.Text.Trim.ToUpper
        objHandler.GiveDataByOrgNrAndKennzeichenDyn(Me)
        If objHandler.Status = 0 Then
            Session("objVFS02") = objHandler

            'Wenn Zeichen für Kennzeichensuche eingegeben wurde,
            'dann wird direkt der Report 'Kennzeichenliste' angezeigt
            If txtKennzeichen.Text.Trim().Length > 0 Then
                ShowReportKennzeichenliste()
                objHandler = Nothing
                Exit Sub
            Else
                FillGrid(0)
                Dim tmpListeVermittlernummer As New System.Collections.ArrayList
                If TryGetListOfVermittlernummernFromSession(tmpListeVermittlernummer) Then
                    SetVermittlernummerInDataGridCheckedOrUnchecked()
                End If
                tmpListeVermittlernummer = CType(Session("App_ListVermKennz"), System.Collections.ArrayList)
                If Not IsNothing(tmpListeVermittlernummer) Then
                    SetVermittlernummerKennz(tmpListeVermittlernummer)
                End If

            End If
        Else
            lblError.Visible = True
            lblError.Text = "Es ist ein Fehler aufgetreten: " & objHandler.Message
        End If

    End Sub

    'Es wurde ein Kennzeichen eingegeben. Daher wird direkt der Report für
    'die Kennzeichenliste aufgerufen
    Private Sub ShowReportKennzeichenliste()
        Dim tmpTable As DataTable

        tmpTable = CreateDataTableKennzeichenliste(objhandler.DetailTable)
        Session("ResultTable") = tmpTable
        CreateExcelReportForSelectedKennzeichenliste(tmpTable)
        Response.Redirect("/Services/(S(" & Session.SessionID & "))/Shared/Report01_2.aspx?AppID=" & Session("AppID").ToString)

    End Sub

    'Hier wird aus der Gesamtmenge der Tabelle Details eine DataTable mit den
    'Datensätzen erzeugt, die den ausgewählten Werten für Vermittlernummer,
    'Auftragsnummer und Versanddatum entsprechen
    Private Function CreateDataTableKennzeichenliste(ByVal pDetailTable As DataTable) As DataTable
        Dim tmpRow As DataRow
        Dim tmpTable As DataTable
        Dim newRow As DataRow

        tmpTable = New DataTable()
        With tmpTable.Columns
            .Add("Kennzeichen", System.Type.GetType("System.String"))
            .Add("Bestellnummer", System.Type.GetType("System.String"))
            .Add("Versand am", System.Type.GetType("System.DateTime"))
            .Add("Verkauf am", System.Type.GetType("System.DateTime"))
            .Add("Rücklauf am", System.Type.GetType("System.DateTime"))
            .Add("Verlust/Storno", System.Type.GetType("System.DateTime"))
            .Add("Agenturnr.", System.Type.GetType("System.String"))
            .Add("Name1", System.Type.GetType("System.String"))
            .Add("Name2", System.Type.GetType("System.String"))
        End With

        For Each tmpRow In pDetailTable.Rows
            newRow = tmpTable.NewRow()
            newRow("Kennzeichen") = tmpRow("Kennzeichen")
            newRow("Bestellnummer") = tmpRow("Bestellnummer")
            newRow("Versand am") = tmpRow("Versand am")
            newRow("Agenturnr.") = tmpRow("VD-Bezirk")
            newRow("Rücklauf am") = tmpRow("Rücklauf am")
            newRow("Name1") = tmpRow("Name1")
            newRow("Name2") = tmpRow("Name2")
            newRow("Verlust/Storno") = tmpRow("Verlust/Storno")
            newRow("Verkauf am") = tmpRow("Verkauf am")
            tmpTable.Rows.Add(newRow)
        Next

        tmpTable.AcceptChanges()
        Return tmpTable
    End Function

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

    Private Function IsPageValid(ByVal OrgNr As String) As Boolean

        If OrgNr.Trim().Length < 2 Then
            If txtKennzeichen.Text.Trim().Length < 3 Then
                lblError.Text = "Eingabe von mindestens 2 Zeichen inkl * für die Agenturnr. oder 3 Zeichen inkl. * für das Kennzeichen erforderlich."
                Return False
            End If
        End If

        Return True
    End Function

    Private Sub GetObjHaendler()
        If Session("objVFS02") Is Nothing = False Then
            objHandler = Session("objVFS02")
        End If

        'Daten holen
        Dim detailsTable As DataTable

        detailsTable = objHandler.DetailTable
        If detailsTable Is Nothing OrElse detailsTable.Rows.Count = 0 Then
            objHandler.GiveDataByOrgNrAndKennzeichenDyn(Me)
        End If
    End Sub

    Private Sub Page_PreLoad(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreLoad
        Session("ShowLink") = "False"
        m_User = GetUser(Me)

        FormAuth(Me, m_User)

        GetAppIDFromQueryString(Me)
        GridNavigation1.setGridElment(GridView1)

        Try
            lblHead.Text = m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString

            m_App = New Security.App(m_User)


            lblError.Text = ""
            If Not IsPostBack Then
                txtKennzeichen.Text = String.Empty
            Else
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

                End If
            End If
        Catch ex As Exception
            lblError.Text = "Beim Laden der Seite ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
        End Try

    End Sub
    Private Sub GridNavigation1_ddlPageSizeChanged() Handles GridNavigation1.PageSizeChanged
        FillGrid(0)
    End Sub

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        SetEndASPXAccess(Me)
        'HelpProcedures.FixedGridViewCols(GridView1)
    End Sub

    Private Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Unload
        SetEndASPXAccess(Me)
    End Sub


    Private Sub DataGrid1_PageIndexChanged(ByVal pageindex As Int32) Handles GridNavigation1.PagerChanged
        GridView1.EditIndex = -1
        FillGrid(pageindex)

    End Sub
    Private Sub GridView1_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles GridView1.Sorting
        FillGrid(GridView1.PageIndex, e.SortExpression)
    End Sub

   

    Protected Sub lnkCreateExcel_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lnkCreateExcel.Click
        objHandler = CType(Session("objVFS02"), VFS02)
        Dim reportExcel As DataTable
        Dim excelFactory As New DocumentGeneration.ExcelDocumentFactory()
        Dim sPath As String = ConfigurationManager.AppSettings("ExcelPath")
        reportExcel = objHandler.OverviewTable.Copy

        reportExcel.AcceptChanges()
        Dim strFileName As String = Format(Now, "yyyyMMdd_HHmmss_") & m_User.UserName
        ' excelFactory.CreateDocumentAndWriteToFilesystemWithPath(sPath + strFileName, reportExcel)
        excelFactory.CreateDocumentAndSendAsResponse(strFileName, reportExcel, Me)
    End Sub

    Protected Sub chkKennzAnzeigenAll_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs)
        Dim item As GridViewRow
        Dim cell As TableCell
        Dim chkSel As CheckBox
        Dim bChecked As Boolean = CType(sender, CheckBox).Checked
        FillGrid(0)
        For Each item In GridView1.Rows
            cell = item.Cells(3)
            chkSel = CType(cell.FindControl("chkKennzAnzeigen"), CheckBox)
            If Not chkSel Is Nothing Then
                chkSel.Checked = bChecked
            End If
        Next
        chkSel = CType(GridView1.HeaderRow.Cells(3).FindControl("chkKennzAnzeigenAll"), CheckBox)
        If Not chkSel Is Nothing Then
            chkSel.Checked = bChecked
        End If

    End Sub
    Protected Sub chkAlleAdrAnzeigen_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs)
        Dim item As GridViewRow
        Dim cell As TableCell
        Dim chkSel As CheckBox
        Dim bChecked As Boolean = CType(sender, CheckBox).Checked
        FillGrid(0)
        For Each item In GridView1.Rows
            cell = item.Cells(3)
            chkSel = CType(cell.FindControl("chkAdresseAnzeigen"), CheckBox)
            If Not chkSel Is Nothing Then
                chkSel.Checked = bChecked
            End If
        Next
        chkSel = CType(GridView1.HeaderRow.Cells(3).FindControl("chkAlleAdrAnzeigen"), CheckBox)
        If Not chkSel Is Nothing Then
            chkSel.Checked = bChecked
        End If
    End Sub

    Private Sub NewSearch_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles NewSearch.Click
        tab1.Visible = Not tab1.Visible
        btnConfirm.Visible = Not btnConfirm.Visible
    End Sub

    Protected Sub lbBack_Click(sender As Object, e As EventArgs) Handles lbBack.Click
        Response.Redirect("../../../Start/Selection.aspx?AppID=" & Session("AppID").ToString, False)
    End Sub
End Class