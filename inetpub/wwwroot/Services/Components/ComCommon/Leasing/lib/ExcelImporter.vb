Option Strict On
Option Explicit On

Friend Class ExcelImporter
    Private Function ConnectionStringBauen(path As String) As String
        Return String.Format("Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Extended Properties=""Excel 8.0;HDR=YES;IMEX=1;ReadOnly=TRUE""", path)
    End Function

    Private Function ExceltabelleFinden(path As String) As DataTable
        Dim connectionString As String = Me.ConnectionStringBauen(path)

        Using connection As New System.Data.OleDb.OleDbConnection(connectionString)
            connection.Open()

            Dim table As DataTable = connection.GetOleDbSchemaTable(OleDb.OleDbSchemaGuid.Tables, Nothing)

            Dim names As IEnumerable(Of String) = From r In table _
                                                  Select r.Field(Of String)(2)

            Dim adaptor As New OleDb.OleDbDataAdapter()
            adaptor.SelectCommand = New OleDb.OleDbCommand
            adaptor.SelectCommand.Connection = connection

            Dim importTable As DataTable = Nothing

            For Each name As String In names
                importTable = New DataTable()
                adaptor.SelectCommand.CommandText = name
                adaptor.SelectCommand.CommandType = CommandType.TableDirect
                adaptor.Fill(importTable)

                If importTable.Rows.Count > 0 Then
                    Exit For
                End If
            Next

            Return importTable
        End Using
    End Function

    Private Sub ExceltabelleKonvertieren(excelSheet As DataTable, resultTable As DataTable)
        Dim mapper As New List(Of Action(Of DataRow, DataRow))(resultTable.Columns.Count)
        Dim cols(resultTable.Columns.Count - 1) As DataColumn
        resultTable.Columns.CopyTo(cols, 0)
        Dim colList As New List(Of DataColumn)(cols)
        colList.Reverse()

        For Each column As DataColumn In excelSheet.Columns
            Dim i As Integer = column.Ordinal
            For x As Integer = colList.Count - 1 To 0 Step -1
                Dim newColumn As DataColumn = colList(x)
                Dim j As Integer = newColumn.Ordinal

                If newColumn.ExtendedProperties.ContainsKey("SearchString") Then
                    Dim searchString As String = DirectCast(newColumn.ExtendedProperties("SearchString"), String)

                    If column.ColumnName.Contains(searchString) Then
                        mapper.Add(Sub(row As DataRow, newRow As DataRow)
                                       newRow(j) = row(i)
                                   End Sub)

                        colList.Remove(newColumn)
                        Exit For
                    End If
                Else
                    colList.Remove(newColumn)
                End If
            Next

            If colList.Count = 0 Then
                Exit For
            End If
        Next

        For Each row As DataRow In excelSheet.Rows
            Dim hasData As Boolean = row.ItemArray.Any(Function(o) As Boolean
                                                           Dim s As String = Convert.ToString(o)
                                                           Return s IsNot Nothing AndAlso Not String.IsNullOrEmpty(s.Trim())
                                                       End Function)

            If Not hasData Then
                Continue For
            End If

            Dim newRow As DataRow = resultTable.NewRow()

            For Each map As Action(Of DataRow, DataRow) In mapper
                map(row, newRow)
            Next

            resultTable.Rows.Add(newRow)
        Next

        resultTable.AcceptChanges()
    End Sub

    Friend Function ExceldatenLaden(path As String, returnTable As DataTable) As DataTable
        Dim importTable As DataTable = Me.ExceltabelleFinden(path)

        If importTable IsNot Nothing Then
            Me.ExceltabelleKonvertieren(importTable, returnTable)
            importTable = returnTable
        End If

        Return importTable
    End Function
End Class
