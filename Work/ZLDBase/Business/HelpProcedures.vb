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
        ''' Prüft eine Textbox auf eine korrekte Datumseingabe, oder nichteingabe, erzeugt eine Fehlermeldung und gibts das Prüfungsergebniss zurück
        ''' </summary> 
        ''' <param name="date1">Feld der Datumseingabe</param>
        '''<param name="errorText">Fehlermeldung</param>
        '''<param name="allowEmpty">Ist eine nicht-Eingabe erlaubt</param>
        ''' <returns>Das Ergebniss der Prüfung</returns>
        Public Overloads Shared Function checkDate(ByRef date1 As TextBox, ByRef errorText As String, ByVal allowEmpty As Boolean) As Boolean

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

        Public Shared Function CastSapBizTalkErrorMessage(ByVal errorMessage As String) As String
            If errorMessage.Contains("SapErrorMessage") = True Then

                Return Mid(errorMessage, errorMessage.IndexOf("SapErrorMessage") + 17, _
                            errorMessage.Substring((errorMessage.IndexOf("SapErrorMessage") + 16)).IndexOf("."))

            Else
                Return errorMessage

            End If
        End Function

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
