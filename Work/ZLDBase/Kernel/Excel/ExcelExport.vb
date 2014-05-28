Option Explicit On 
Option Strict On

Namespace Kernel.Excel

    Public Class ExcelExport
        REM § Schreibt die Ergebnisse einer Suche in eine Excel Datei (Format Excel2.1)
        REM § (Excel 2.1 unterstützt keine Hyperlinks in den Zellen)

        Public Shared Sub WriteExcel(ByRef tblData As DataTable, ByVal strPath As String)
            REM § Schreibt ein Datatable Objekt in eine Excel-Datei.

            ' Datatable als ExcelDatei wegschreiben:
            Dim objExFile As New ExcelFile()

            Dim rw As DataRow
            Dim cl As DataColumn
            Dim intCol As Integer = 1
            Dim intRow As Integer = 1
            Dim intRC As Integer
            Dim XlType As ExcelFile.ValueTypes
            Dim XLAlign As ExcelFile.CellAlignment
            Dim objExcel As New ExcelFile()

            If tblData.Rows.Count > 0 Then

                intRC = objExcel.CreateFile(strPath)
                ' Schriften setzen
                objExcel.SetFont("Arial", 10, ExcelFile.FontFormatting.xlsNoFormat)
                objExcel.SetFont("Arial", 10, ExcelFile.FontFormatting.xlsBold)

                Dim intMaxColLen As Integer = 0
                If intRC <> 0 Then
                    ' Fehler
                    Throw New ApplicationException("Fehler bei der Erzeugung der Excel-Datei.")
                Else
                    Dim objValue As Object

                    For Each cl In tblData.Columns
                        If Not cl.ColumnName = "ABE-Daten" Then
                            System.Diagnostics.Debug.WriteLine(cl.DataType.Name)
                            ' Alignment und Typ wird abhängig vom Datentyp eingestellt:
                            If cl.DataType.Name = "String" Then
                                XlType = ExcelFile.ValueTypes.xlsText
                                XLAlign = ExcelFile.CellAlignment.xlsLeftAlign
                            ElseIf cl.DataType.Name = "Integer" OrElse cl.DataType.Name = "Int32" OrElse cl.DataType.Name = "Decimal" Then
                                XlType = ExcelFile.ValueTypes.xlsnumber  ' Bei Verwendung von xlsInteger tritt ein Fehler auf, wenn die Werte außerhalb des Bereichs von Int16 liegen!!
                                XLAlign = ExcelFile.CellAlignment.xlsGeneralAlign
                            ElseIf cl.DataType.Name = "Date" OrElse cl.DataType.Name = "DateTime" Then
                                XlType = ExcelFile.ValueTypes.xlsdate
                                XLAlign = ExcelFile.CellAlignment.xlsCentreAlign
                            ElseIf cl.DataType.Name = "Boolean" OrElse cl.DataType.Name = "Bit" Then
                                XlType = ExcelFile.ValueTypes.xlsText
                                XLAlign = ExcelFile.CellAlignment.xlsCentreAlign
                            Else
                                XlType = ExcelFile.ValueTypes.xlsText
                                XLAlign = ExcelFile.CellAlignment.xlsLeftAlign
                            End If
                            ' Header Schreiben
                            If (cl.ExtendedProperties("HeadLine") Is Nothing) OrElse cl.ExtendedProperties("HeadLine").ToString.Length = 0 Then
                                intRC = objExcel.WriteValue(ExcelFile.ValueTypes.xlsText, ExcelFile.CellFont.xlsFont1, ExcelFile.CellAlignment.xlsLeftAlign, ExcelFile.CellHiddenLocked.xlsNormal, 1, intCol, CType(cl.ColumnName, Object))
                            Else
                                intRC = objExcel.WriteValue(ExcelFile.ValueTypes.xlsText, ExcelFile.CellFont.xlsFont1, ExcelFile.CellAlignment.xlsLeftAlign, ExcelFile.CellHiddenLocked.xlsNormal, 1, intCol, CType(cl.ExtendedProperties("HeadLine").ToString, Object))
                            End If

                            intMaxColLen = cl.ColumnName.Length '+ 5
                            If intRC <> 0 Then
                                'Fehler!!
                                ' Und Schluß
                                objExcel.CloseFile()
                                Throw New ApplicationException("Fehler beim Schreiben der Excel-Datei.")
                            Else
                                intRow = 2
                                'objExcel.SetFont("Arial", 10, ExcelFile.FontFormatting.xlsNoFormat)
                                For Each rw In tblData.Rows
                                    If rw.RowState <> DataRowState.Deleted And rw.RowState <> DataRowState.Detached Then
                                        objValue = rw(cl)
                                        If TypeOf objValue Is System.DBNull Then
                                            objValue = Nothing
                                        End If
                                        If TypeOf objValue Is System.String Then
                                            If objValue.ToString.Length > 255 Then
                                                objValue = objValue.ToString.Substring(0, 255)
                                            End If
                                        End If
                                        If Not objValue Is Nothing Then
                                            intRC = objExcel.WriteValue(XlType, ExcelFile.CellFont.xlsFont0, XLAlign, ExcelFile.CellHiddenLocked.xlsNormal, intRow, intCol, objValue)
                                        End If
                                        If Not objValue Is Nothing AndAlso objValue.ToString.Length > intMaxColLen Then
                                            intMaxColLen = objValue.ToString.Length
                                        End If
                                        If intRC <> 0 Then
                                            'Fehler!!
                                            objExcel.CloseFile()
                                            Throw New ApplicationException("Fehler beim Schreiben der Excel-Datei.")
                                            Exit For
                                        End If
                                        intRow += 1
                                    End If
                                Next
                                intMaxColLen += 5
                                If intMaxColLen > 255 Then
                                    intMaxColLen = 255
                                End If
                                objExcel.SetColumnWidth(CByte(intCol), CByte(intCol), CShort(intMaxColLen))
                                intCol += 1
                            End If
                        End If
                    Next
                    objExcel.CloseFile()
                End If
            End If
        End Sub
    End Class
End Namespace

' ************************************************
' $History: ExcelExport.vb $
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 3.04.08    Time: 16:42
' Created in $/CKAG/Base/Kernel/Excel
' 
' *****************  Version 4  *****************
' User: Uha          Date: 15.05.07   Time: 11:04
' Updated in $/CKG/Base/Base/Kernel/Excel
' Änderungen aus StartApplication vom 11.05.2007 (Aspose-Tool,
' DataTableHelper.vb, Customer.vb)
' 
' ************************************************
