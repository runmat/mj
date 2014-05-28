Option Explicit On
Option Strict On

Imports Microsoft.VisualBasic
Imports System
Imports System.Text.RegularExpressions
Imports System.Web.UI.WebControls
Imports System.Drawing
Imports System.Web.UI

Namespace Business

    Public NotInheritable Class HelpProcedures

        Public Enum TranslationDirection
            ReadHTML
            SaveHTML
        End Enum

#Region "Konstruktor"

        Private Sub New()
            'es sollen Klassenmethoden verwendet werden
        End Sub
#End Region
        
#Region "Methoden"
        ''' <summary>
        ''' Wandelt die angegebenen Spalten (Übergabe als Spaltenname) der übergebenen DataTable in einen Date-Typ um
        ''' </summary> 
        ''' <param name="alDateColums">alle Namen der Datums-Spalten als String enthält</param>
        '''<param name="dataSource">deren angegebene Spalten in einen Date-Typ umgewandelt werden sollen</param>
        ''' <returns>die umgewandelte DataTable</returns>

        Public Shared Function FormatGridDateColumn(ByVal alDateColums As ArrayList, ByVal dataSource As DataTable) As DataTable
            Dim dr As DataRow
            Dim tmpColumn As DataColumn
            Dim alColumnNumbers As New ArrayList()
            Dim i As Int32 = 0
            'Dim z As Int32 = 0
            Dim tmprow As DataRow
            Dim strDate As String
            Dim tmpTable As New DataTable()

            For Each tmpColumn In dataSource.Columns
                If alDateColums.Contains(tmpColumn.ColumnName.ToString().ToUpper) = True Then
                    tmpTable.Columns.Add(tmpColumn.ColumnName, Type.GetType("System.DateTime"))
                    alColumnNumbers.Add(i)  'speichern der Columns einer Row die Date sein müssen
                Else
                    tmpTable.Columns.Add(tmpColumn.ColumnName.ToString, tmpColumn.DataType)
                End If
                i = i + 1
            Next


            For Each dr In dataSource.Rows
                tmprow = tmpTable.NewRow()
                For i = 0 To dr.ItemArray.Length - 1
                    If alColumnNumbers.Contains(i) Then
                        strDate = dr.Item(i).ToString.Substring(6, 2) & "." & _
                                                                         dr.Item(i).ToString.Substring(4, 2) & "." & _
                                                                         dr.Item(i).ToString.Substring(0, 4)
                        Try

                            tmprow.Item(i) = CDate(strDate)
                        Catch
                            tmprow.Item(i) = DBNull.Value 'wenn SAPdatum= 00.00.0000
                        End Try
                    Else
                        tmprow.Item(i) = dr.Item(i)
                    End If

                Next i
                tmpTable.Rows.Add(tmprow)
            Next
            FormatGridDateColumn = tmpTable
            
        End Function

        ''' <summary>
        ''' Prüft eine Textbox auf eine korrekte Datumseingabe, oder nichteingabe, erzeugt eine Fehlermeldung und gibts das Prüfungsergebniss zurück
        ''' </summary> 
        ''' <param name="date1">Feld der Datumseingabe</param>
        '''<param name="errorText">Fehlermeldung</param>
        '''<param name="allowEmpty">Ist eine nicht-Eingabe erlaubt</param>
        ''' <returns>Das Ergebniss der Prüfung</returns>
        Public Overloads Shared Function checkDate(ByRef date1 As System.Web.UI.WebControls.TextBox, ByRef errorText As String, ByVal allowEmpty As Boolean) As Boolean

            If IsDate(date1.Text) Then
                checkDate = True
            Else
                If date1.Text = "" And allowEmpty = True Then
                    checkDate = True
                Else
                    errorText = "Geben Sie bitte ein gültiges Datum ein: " & date1.ToolTip
                    checkDate = False
                End If
            End If
        End Function

        ''' <summary>
        ''' Prüft Textboxen auf korrekte Datumseingabe, füllt leere Textbox mit Wert der gefüllten (von-bis Restriktion), Kleiner als Vergleich des Bis-Datums,
        ''' berechnet optional eine Zeispannen-Restriktion, erzeugt eine Fehlermeldung und gibt das Prüfungsergebniss zurück
        ''' </summary> 
        '''<param name="date1">Feld 1 der Datumseingabe</param>
        '''<param name="date2">Feld 2 der Datumseingabe</param>
        '''<param name="errorText">Fehlermeldung</param>
        '''<param name="allowEmpty">Ist eine nicht-Eingabe erlaubt</param>
        '''<param name="maxDateDiffInYears">DifferenzZeitspanne in Jahren</param>
        ''' <returns>Das Ergebniss der Prüfung</returns>
        Public Overloads Shared Function checkDate(ByRef date1 As System.Web.UI.WebControls.TextBox, ByRef date2 As System.Web.UI.WebControls.TextBox,
                                                   ByRef errorText As String, ByVal allowEmpty As Boolean,
                                                   Optional ByVal maxDateDiffInYears As Int32 = 0) As Boolean

            If date1.Text = "" And date2.Text = "" And allowEmpty = False Or
                IsDate(date1.Text) = False And (date1.Text = "") = False Or
                IsDate(date2.Text) = False And (date2.Text = "") = False Then

                If IsDate(date1.Text) = False And Not date1.Text = "" And date2.Text = "" Or IsDate(date2.Text) = True Then
                    errorText = "Geben Sie bitte ein gültiges Datum ein: " & date1.ToolTip
                    checkDate = False
                    Exit Function
                End If

                If IsDate(date2.Text) = False And Not date2.Text = "" And date1.Text = "" Or IsDate(date1.Text) = True Then
                    errorText = "Geben Sie bitte ein gültiges Datum ein: " & date2.ToolTip
                    checkDate = False
                    Exit Function
                End If
                'wenn beidesmal das Falsche Datumsformat eingegeben oder beide leer sind
                errorText = "Geben Sie bitte ein gültiges Datum ein "
                checkDate = False
                Exit Function
            Else
                If (date1.Text = "") = False Or (date2.Text = "") = False Then
                    If date1.Text = "" Then
                        date1.Text = date2.Text
                    ElseIf date2.Text = "" Then
                        date2.Text = date1.Text
                    End If

                    If CDate(date1.Text) <= CDate(date2.Text) Then
                        If Not maxDateDiffInYears = 0 Then
                            Dim maxDateDiffInDays As Int32 = maxDateDiffInYears * 365
                            If Not DateDiff(DateInterval.DayOfYear, CDate(date1.Text), CDate(date2.Text)) <= maxDateDiffInDays Then
                                errorText = "Zeitraum darf maximal " & maxDateDiffInYears & " Jahr betragen"
                                checkDate = False
                                Exit Function
                            Else
                                checkDate = True
                            End If
                        Else
                            checkDate = True
                        End If
                    Else
                        errorText = date1.ToolTip & " darf nicht kleiner sein als " & date2.ToolTip
                        checkDate = False
                        Exit Function
                    End If
                Else
                    checkDate = True
                End If
            End If
        End Function


        '----------------------------------------------------------------------
        'Methode:       DataTableAlphabeticSort
        'Autor:         JJU/ORU
        'Beschreibung:  Sortier eine DT
        'Erstellt am:   8.7.2009
        '----------------------------------------------------------------------

        ''' <summary>
        ''' Sortiert eine DataTable anhand einer Spalte und der Sortierungsart, gibt die sortierte DT wieder zurück
        ''' </summary> 
        '''<param name="dtTable">Die zu sortierende DT</param>
        '''<param name="ColumnName">Spalte der Sortierung</param>
        '''<param name="sortOrder">Sortierrichtung 0=aufsteigend, 1=absteigend</param>
        ''' <returns>Sortierte DT</returns>
        Public Shared Function DataTableAlphabeticSort(ByVal dtTable As DataTable, ByVal ColumnName As String, ByVal sortOrder As Integer) As DataTable
            Dim columnKey As String = ColumnName
            Dim sortDirection As String = ""
            Dim sortFormat As String = "{0} {1}"
            Dim newDT As DataTable = dtTable.Clone
            Dim newRow As DataRow

            Select Case sortOrder
                Case 0
                    sortDirection = "ASC"
                Case 1
                    sortDirection = "DESC"
                Case Else
                    sortDirection = "ASC"
            End Select
            dtTable.DefaultView.Sort = String.Format(sortFormat, columnKey, sortDirection)


            Dim i As Int32

            For i = 0 To dtTable.DefaultView.Count - 1 Step 1
                newRow = newDT.NewRow
                newRow.ItemArray = dtTable.DefaultView.Item(i).Row.ItemArray
                newDT.Rows.Add(newRow)
            Next
            newDT.AcceptChanges()

            Return newDT
        End Function
        
        ''' <summary>
        ''' gibt die Parameterliste für eine Applikation in der SQL-Datenbank zurück
        ''' 
        ''' </summary> 
        '''<param name="strAppID">AppID</param>
        '''<param name="paramlist">Parameterliste wie in der Datenbank</param>
        '''<param name="ConnectionString">der aktuelle Connectionstring, bitte aus WebConfig lesen</param>
        ''' <returns>ob Parameter vorhanden</returns>

        Public Shared Function getAppParameters(ByVal strAppID As String, ByRef paramlist As String, ByVal connectionString As String) As Boolean


            Dim conn As New SqlClient.SqlConnection()
            Dim command As New SqlClient.SqlCommand()
            Dim adapter As New SqlClient.SqlDataAdapter()
            Dim result As New DataTable()

            command.CommandType = CommandType.Text
            command.CommandText = "SELECT * FROM ApplicationParamlist WHERE id_app = " & strAppID
            conn.ConnectionString = connectionString
            command.Connection = conn
            Try

                conn.Open()
                adapter.SelectCommand = command
                adapter.Fill(result)
                paramlist = String.Empty
                If Not (result.Rows.Count = 0) Then
                    paramlist = result.Rows(0)("paramlist").ToString
                End If
                Return True
            Catch ex As Exception
                paramlist = String.Empty
                Return False
            Finally
                conn.Close()
                conn.Dispose()
            End Try
        End Function

        Public Shared Function isAlphaNumeric(ByVal wert As String) As Boolean


            Dim allowedValues As String = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZÜÄÖ"
            Dim x As Char
            For Each x In wert.ToUpper
                If allowedValues.IndexOf(x) < 0 Then
                    Return False
                End If
            Next
            Return True
        End Function

        Public Shared Sub killAllDBNullValuesInDataTable(ByRef datentabelle As DataTable)
            '----------------------------------------------------------------------
            'Methode:       killAllDBNullValuesInDataTable
            'Autor:         Julian Jung
            'Beschreibung:  wandelt alle dbnull werte in einer datatable in leere strings ums
            'Erstellt am:   17.7.2008
            '----------------------------------------------------------------------
            For Each tmpRow As DataRow In datentabelle.Rows
                For i As Int32 = 0 To tmpRow.ItemArray.Length - 1
                    If tmpRow(i) Is DBNull.Value Then
                        tmpRow(i) = String.Empty
                    End If
                    If TypeOf (tmpRow(i)) Is DataTable Then
                        killAllDBNullValuesInDataTable(CType(tmpRow(i), DataTable))
                    End If
                Next
            Next
            datentabelle.AcceptChanges()

        End Sub

        Public Shared Function MakeDateSAP(ByVal datInput As Date) As String
            REM $ Formt Date-Input in String YYYYMMDD um
            Return Year(datInput) & Right("0" & Month(datInput), 2) & Right("0" & Day(datInput), 2)
        End Function


        Public Shared Function MakeDateSAP(ByVal datInput As String) As String
            REM $ Formt Date-Input in String YYYYMMDD um
            Dim dat As Date

            dat = CType(datInput, Date)
            Return Year(dat) & Right("0" & Month(dat), 2) & Right("0" & Day(dat), 2)
        End Function


        Public Shared Function MakeDateStandard(ByVal strInput As String) As Date
            REM § Formt String-Input im SAP-Format in Standard-Date um. Gibt "01.01.1900" zurück, wenn Umwandlung nicht möglich ist.
            Dim strTemp As String = Right(strInput, 2) & "." & Mid(strInput, 5, 2) & "." & Left(strInput, 4)
            If IsDate(strTemp) Then
                Return CDate(strTemp)
            Else
                Return CDate("01.01.1900")
            End If
        End Function

        Public Shared Function MakeTimeStandard(ByVal strInput As String) As Date
            Dim strTemp As String = Left(strInput, 2) & ":" & Mid(strInput, 3, 2) & ":" & Right(strInput, 2)
            If IsDate(strTemp) Then
                Return CDate(strTemp)
            Else
                Return CDate("00:00:00")
            End If
        End Function

        Public Shared Function CastSapBizTalkErrorMessage(ByVal errorMessage As String) As String
            If errorMessage.Contains("SapErrorMessage") = True Then

                Dim ErrMessage As String = Mid(errorMessage, errorMessage.IndexOf("SapErrorMessage") + 17, _
                            errorMessage.Substring((errorMessage.IndexOf("SapErrorMessage") + 16)).IndexOf("."))

                ErrMessage = Replace(ErrMessage, "EXCEPTION", "")
                ErrMessage = Replace(ErrMessage, "RAISED", "").Trim

                Return ErrMessage

            Else
                Return errorMessage

            End If
        End Function

        Public Shared Sub fakeLogEntryForAutorisierung(ByVal logAppObj As Base.Kernel.Logging.Trace, ByVal m_User As CKG.Base.Kernel.Security.User, ByVal sessionID As String, _
                                                       ByVal appID As String, ByVal strMessage As String, _
                                                       Optional ByVal strType As String = "APP", _
                                                       Optional ByVal AppName As String = "")
            '----------------------------------------------------------------------
            'Methode:       fakeLogEntryForAutorisierung
            'Autor:         Julian Jung
            'Beschreibung:  Diese Methode schreibt einen Eintrag ins Logging der Autorisierung, wenn ein report nicht in der autorisierung selbst autorisiert wird, gibt es dafür auch kein log eintrag und somit keine 
            '               Benutzerhistorie für die autorisierung. 
            'Erstellt am:   9.7.2008
            '----------------------------------------------------------------------
            Dim appIDofAutorisation As Int32


            'APP ID von "autorisierung" holen
            Dim appAssigned As New Base.Kernel.Admin.ApplicationList(m_User.Customer.CustomerId, m_User.App.Connectionstring)
            appAssigned.GetAssigned()

            Dim dvLinks As DataView = appAssigned.DefaultView
            dvLinks.RowFilter = "AppFriendlyName='Autorisierung' AND AppInMenu=1"

            If Not dvLinks.Count = 1 Then
                Throw New Exception("Die Applikation 'Autorisierung' wurde in den Berechtigungen nicht gefunden, LogEintrag konnte nicht erstellt werden")
            Else
                appIDofAutorisation = CInt(dvLinks.Item(0)("AppID"))
            End If
            'Bei der Autorisierung kann es sein das die zu autorisierende Anwendung nicht dem User zugeordnet ist.
            'Bsp. appFFE/Change06 (Händler) Autorisation mit appFFE/Change08(Bank)
            'deshalb hier den AppName mitgeben. OR 18.07.2008
            If AppName.Length = 0 Then
                AppName = m_User.Applications.Select("AppID = '" & appID & "'")(0)("AppFriendlyName").ToString()
            End If

            If Not logAppObj.WriteEntry(strType, m_User.UserName, sessionID, appIDofAutorisation, "Autorisierung", AppName, strMessage, m_User.CustomerName, m_User.Customer.CustomerId, m_User.IsTestUser) Then
                Throw New Exception("Fehler bei der Funktion WriteEntry() in HelpProcedures.fakeLogEntryForAutorisierung")
            End If

        End Sub

        Public Shared Function EmailAddressCheck(ByVal emailAddress As String) As Boolean

            If Regex.IsMatch(emailAddress, _
               "^(([A-Za-z0-9]+_+)|([A-Za-z0-9]+\-+)|([A-Za-z0-9]+\.+)|" & _
               "([A-Za-z0-9]+\++))*[A-Za-z0-9]+@((\w+\-+)|(\w+\.))" & _
               "*\w{1,63}\.[a-zA-Z]{2,6}$") Then
                EmailAddressCheck = True
                ' Email gültig
            Else
                ' Email ungültig
                EmailAddressCheck = False
            End If



        End Function
        '----------------------------------------------------------------------
        'Methode:       CreateBarcode
        'Autor:         Oliver Rudolph
        'Beschreibung:  Diese Methode erstellt aus einem String einen Barcode als Image und 
        '               gibt es als MemoryStream zurück. 
        'Erstellt am:   10.06.2010
        '----------------------------------------------------------------------
        Public Shared Function CreateBarcode(ByVal sCode As String) As IO.MemoryStream
            Try
                Dim BarCode As New BarcodeLib.Barcode
                Dim oImage As System.Drawing.Image
                Dim w As Integer = sCode.Length * 150

                oImage = BarCode.Encode(BarcodeLib.TYPE.CODE39, sCode, w, 75)

                Dim ms As New IO.MemoryStream
                oImage.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg)
                oImage.Dispose()
                Return ms
            Catch ex As Exception
                Throw New Exception("Fehler beim generieren des Barcodes in HelpProcedures.CreateBarcode")
            End Try

        End Function

        '----------------------------------------------------------------------
        'Methode:       Linebreaker
        'Autor:         Christian Dittberner
        'Beschreibung:  Diese Methode erzeugt automatische Zeilenumbrüche und 
        '               dient der Aufbereitung eines GridView für die 
        '               FixedGridViewCols Methode
        'Erstellt am:   13.04.2011
        '----------------------------------------------------------------------
        Public Shared Sub Linebreaker(ByVal Row As DataRow, ByVal ColName As String, ByVal width As Integer)

            ' For Each dRow As DataRow In Table.Rows
            If CStr(Row(ColName)).Length > width Then

                Dim strIn As String = CStr(Row(ColName))
                Dim strOut As String = ""

                Dim strArray As String() = strIn.Split(CChar(" "))
                Dim strtemp As String = ""
                Dim iLängeStr As Integer = 0

                For i As Integer = 0 To strArray.GetLength(0) - 1

                    If iLängeStr + strArray(i).Length <= width Then
                        strtemp = strtemp + strArray(i) + " "
                        iLängeStr = iLängeStr + strArray(i).Length + 1
                    Else
                        If strArray(i).Length >= width Then

                            strOut = strOut + strtemp + "<br />" + strArray(i) + "<br />"
                            iLängeStr = 0
                            strtemp = ""
                        Else
                            strOut = strOut + strtemp + "<br />"
                            iLängeStr = strArray(i).Length + 1
                            strtemp = strArray(i) + " "
                        End If
                    End If
                Next
                If strtemp.Length > 0 Then
                    strOut = strOut + strtemp
                End If
                Row(ColName) = strOut

            End If
            'Next
        End Sub

        Public Shared Sub FixedGridViewCols(ByVal Grid As GridView)
            If Not Grid Is Nothing Then
                If Grid.Rows.Count > 0 Then
                    For Each headerCell As TableCell In Grid.HeaderRow.Cells
                        Dim g As Graphics
                        Dim font As New Font("Verdana", 8)
                        Dim bmp As New Bitmap(1, 1)
                        Dim ColWidth As Integer = 30

                        'Prüfe Headerzeile auf Breite
                        If headerCell.Controls.Count > 0 Then
                            For Each ColumnsControl As Control In headerCell.Controls
                                g = Graphics.FromImage(bmp)
                                If (TypeOf (ColumnsControl) Is LinkButton) Then
                                    Dim lbtn As LinkButton = CType(ColumnsControl, LinkButton)
                                    ColWidth = CInt(g.MeasureString(lbtn.Text, font).Width)
                                    headerCell.Width = Unit.Pixel(ColWidth)
                                ElseIf (TypeOf (ColumnsControl) Is DropDownList) Then
                                    Dim ddl As DropDownList = CType(ColumnsControl, DropDownList)
                                    headerCell.Width = ddl.Width
                                ElseIf ColumnsControl Is Nothing Then
                                    If (headerCell.Text.Length > 0) Then
                                        ColWidth = CInt(g.MeasureString(headerCell.Text, font).Width)
                                        headerCell.Width = Unit.Pixel(ColWidth)
                                    End If
                                End If
                                g.Dispose()
                            Next
                        Else
                            g = Graphics.FromImage(bmp)
                            If (headerCell.Text.Length > 0) Then
                                ColWidth = CInt(g.MeasureString(headerCell.Text, font).Width)
                                headerCell.Width = Unit.Pixel(ColWidth)
                            End If
                        End If

                    Next

                    'Prüfe alle Zeilen auf Breite
                    For Each dRow As TableRow In Grid.Rows
                        Dim g As Graphics
                        Dim font As New Font("Verdana", 8)
                        Dim bmp As New Bitmap(1, 1)
                        Dim width As Integer
                        Dim ColWidth As Integer = 30

                        For Each itemCell As TableCell In dRow.Cells
                            Dim ColCount As Integer = dRow.Cells.GetCellIndex(itemCell)

                            If itemCell.Controls.Count > 0 Then
                                For Each itemControl As Control In itemCell.Controls
                                    width = 100
                                    g = Graphics.FromImage(bmp)
                                    If (TypeOf (itemControl) Is Label) Then
                                        Dim lbl As Label = CType(itemControl, Label)
                                        Dim tmpStrings As String()
                                        Dim strToCheck As String = String.Empty
                                        Dim strSplitter As String() = {"<br />"}

                                        ' berücksichtigt Zeilenumbrüche in der Spaltenbreitenberechnung
                                        tmpStrings = lbl.Text.Split(strSplitter, System.StringSplitOptions.RemoveEmptyEntries)
                                        For Each Str As String In tmpStrings
                                            If Str.Length > strToCheck.Length Then
                                                strToCheck = Str
                                            End If
                                        Next

                                        width = CInt(g.MeasureString(strToCheck, font).Width)
                                        If ColWidth > width Then
                                            If Grid.HeaderRow.Cells(ColCount).Width.Value < ColWidth Then
                                                Grid.HeaderRow.Cells(ColCount).Width = Unit.Pixel(ColWidth)
                                            End If
                                        Else
                                            If Grid.HeaderRow.Cells(ColCount).Width.Value < width Then
                                                Grid.HeaderRow.Cells(ColCount).Width = Unit.Pixel(width)
                                            End If
                                        End If
                                    ElseIf (TypeOf (itemControl) Is HyperLink) Then
                                        Dim hl As HyperLink = CType(itemControl, HyperLink)
                                        Dim tmpStrings As String()
                                        Dim strToCheck As String = String.Empty
                                        Dim strSplitter As String() = {"<br />"}

                                        ' berücksichtigt Zeilenumbrüche in der Spaltenbreitenberechnung
                                        tmpStrings = hl.Text.Split(strSplitter, System.StringSplitOptions.RemoveEmptyEntries)
                                        For Each Str As String In tmpStrings
                                            If Str.Length > strToCheck.Length Then
                                                strToCheck = Str
                                            End If
                                        Next

                                        width = CInt(g.MeasureString(strToCheck, font).Width)
                                        If ColWidth > width Then
                                            If Grid.HeaderRow.Cells(ColCount).Width.Value < ColWidth Then
                                                Grid.HeaderRow.Cells(ColCount).Width = Unit.Pixel(ColWidth)
                                            End If
                                        Else
                                            If Grid.HeaderRow.Cells(ColCount).Width.Value < width Then
                                                Grid.HeaderRow.Cells(ColCount).Width = Unit.Pixel(width)
                                            End If
                                        End If
                                    ElseIf (TypeOf (itemControl) Is LinkButton) Then
                                        Dim lbtn As LinkButton = CType(itemControl, LinkButton)
                                        width = CInt(g.MeasureString(lbtn.Text, font).Width)
                                        If ColWidth > width Then
                                            If Grid.HeaderRow.Cells(ColCount).Width.Value < ColWidth Then
                                                Grid.HeaderRow.Cells(ColCount).Width = Unit.Pixel(ColWidth)
                                            End If
                                        Else

                                            If Grid.HeaderRow.Cells(ColCount).Width.Value < width Then
                                                Grid.HeaderRow.Cells(ColCount).Width = Unit.Pixel(width)
                                            End If
                                        End If

                                    ElseIf (TypeOf (itemControl) Is CheckBox) Then
                                        Dim chkBx As CheckBox = CType(itemControl, CheckBox)
                                        If ColWidth > chkBx.Width.Value Then
                                            If Grid.HeaderRow.Cells(ColCount).Width.Value < ColWidth Then
                                                Grid.HeaderRow.Cells(ColCount).Width = Unit.Pixel(ColWidth)
                                            End If
                                        Else
                                            If Grid.HeaderRow.Cells(ColCount).Width.Value < chkBx.Width.Value Then
                                                Grid.HeaderRow.Cells(ColCount).Width = Unit.Pixel(CInt(chkBx.Width.Value))
                                            End If
                                        End If
                                    ElseIf (TypeOf (itemControl) Is DropDownList) Then
                                        Dim ddl As DropDownList = CType(itemControl, DropDownList)
                                        If ColWidth > ddl.Width.Value Then
                                            If Grid.HeaderRow.Cells(ColCount).Width.Value < ColWidth Then
                                                Grid.HeaderRow.Cells(ColCount).Width = Unit.Pixel(ColWidth)
                                            End If
                                        Else
                                            If Grid.HeaderRow.Cells(ColCount).Width.Value < ddl.Width.Value Then
                                                Grid.HeaderRow.Cells(ColCount).Width = Unit.Pixel(CInt(ddl.Width.Value))
                                            End If
                                        End If
                                    ElseIf (TypeOf (itemControl) Is TextBox) Then
                                        Dim txtBx As TextBox = CType(itemControl, TextBox)
                                        Dim iElementWidth As Integer = CInt(txtBx.BorderWidth.Value + txtBx.Width.Value)
                                        If ColWidth > iElementWidth Then
                                            If Grid.HeaderRow.Cells(ColCount).Width.Value < ColWidth Then
                                                Grid.HeaderRow.Cells(ColCount).Width = Unit.Pixel(ColWidth)
                                            End If
                                        Else
                                            If Grid.HeaderRow.Cells(ColCount).Width.Value < iElementWidth Then
                                                Grid.HeaderRow.Cells(ColCount).Width = Unit.Pixel(iElementWidth)
                                            End If
                                        End If
                                    ElseIf (TypeOf (itemControl) Is Table) Then
                                        Dim tbl As Table = CType(itemControl, Table)
                                        Dim iElementWidth As Integer = CInt(tbl.BorderWidth.Value + tbl.Width.Value)
                                        If ColWidth > iElementWidth Then
                                            If Grid.HeaderRow.Cells(ColCount).Width.Value < ColWidth Then
                                                Grid.HeaderRow.Cells(ColCount).Width = Unit.Pixel(ColWidth)
                                            End If
                                        Else
                                            If Grid.HeaderRow.Cells(ColCount).Width.Value < iElementWidth Then
                                                Grid.HeaderRow.Cells(ColCount).Width = Unit.Pixel(iElementWidth)
                                            End If
                                        End If
                                    End If
                                    g.Dispose()
                                Next
                            Else
                                g = Graphics.FromImage(bmp)
                                If (itemCell.Text.Length > 0) Then
                                    width = CInt(g.MeasureString(itemCell.Text, font).Width)
                                    If ColWidth > width Then
                                        If Grid.HeaderRow.Cells(ColCount).Width.Value < ColWidth Then
                                            Grid.HeaderRow.Cells(ColCount).Width = Unit.Pixel(ColWidth)
                                        End If
                                    Else
                                        If Grid.HeaderRow.Cells(ColCount).Width.Value < width Then
                                            Grid.HeaderRow.Cells(ColCount).Width = Unit.Pixel(width)
                                        End If
                                    End If
                                End If
                            End If
                        Next
                    Next
                End If
            End If
        End Sub

        Public Shared Function TranslateHTML(ByVal Text As String, ByVal Direction As TranslationDirection) As String
            Select Case Direction
                Case TranslationDirection.ReadHTML
                    Text = Text.Replace("{", "<")
                    Text = Text.Replace("}", ">")
                Case TranslationDirection.SaveHTML
                    Text = Text.Replace("<", "{")
                    Text = Text.Replace(">", "}")
            End Select
            Return Text
        End Function
#End Region


    End Class

End Namespace
' ************************************************
' $History: HelpProcedures.vb $
' 
' *****************  Version 28  *****************
' User: Dittbernerc  Date: 9.05.11    Time: 13:39
' Updated in $/CKAG/Base/Business
' 
' *****************  Version 27  *****************
' User: Dittbernerc  Date: 13.04.11   Time: 13:48
' Updated in $/CKAG/Base/Business
' 
' *****************  Version 26  *****************
' User: Dittbernerc  Date: 11.04.11   Time: 17:01
' Updated in $/CKAG/Base/Business
' 
' *****************  Version 25  *****************
' User: Dittbernerc  Date: 7.04.11    Time: 14:28
' Updated in $/CKAG/Base/Business
' 
' *****************  Version 24  *****************
' User: Dittbernerc  Date: 5.04.11    Time: 16:30
' Updated in $/CKAG/Base/Business
' FixGridViewCols:
' 
' Elemente wie Textboxen, Dropdownlisten, und Checkboxen werden in der
' Berechnung mit berücksichtigt.
' Die Headrow wird nur noch einmalig abgefragt nicht pro abgefragter Row
' 
' *****************  Version 23  *****************
' User: Dittbernerc  Date: 4.04.11    Time: 17:04
' Updated in $/CKAG/Base/Business
' Spaltenberechnung berücksichtigt Zeilenumbrüche in Labels
' 
' *****************  Version 22  *****************
' User: Rudolpho     Date: 22.03.11   Time: 14:50
' Updated in $/CKAG/Base/Business
' 
' *****************  Version 21  *****************
' User: Rudolpho     Date: 2.02.11    Time: 8:37
' Updated in $/CKAG/Base/Business
' 
' *****************  Version 20  *****************
' User: Rudolpho     Date: 4.01.11    Time: 11:28
' Updated in $/CKAG/Base/Business
' 
' *****************  Version 19  *****************
' User: Rudolpho     Date: 14.06.10   Time: 11:33
' Updated in $/CKAG/Base/Business
' 
' *****************  Version 18  *****************
' User: Rudolpho     Date: 14.06.10   Time: 9:57
' Updated in $/CKAG/Base/Business
' 
' *****************  Version 17  *****************
' User: Fassbenders  Date: 7.10.09    Time: 16:31
' Updated in $/CKAG/Base/Business
' 
' *****************  Version 16  *****************
' User: Jungj        Date: 15.07.09   Time: 17:33
' Updated in $/CKAG/Base/Business
' Anpassung isalphanumeric
' 
' *****************  Version 15  *****************
' User: Jungj        Date: 8.07.09    Time: 11:40
' Updated in $/CKAG/Base/Business
' Helpprocedures DataTableAlphabeticSort hinzugefügt
' 
' *****************  Version 14  *****************
' User: Jungj        Date: 9.12.08    Time: 13:25
' Updated in $/CKAG/Base/Business
' ssv historie hinzugefügt
' 
' ************************************************