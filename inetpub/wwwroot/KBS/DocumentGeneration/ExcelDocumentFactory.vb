Option Explicit On
Option Strict On

Imports System.Configuration
Imports System.IO
Imports System.Text
Imports System

Namespace DocumentGeneration

    Public Class ExcelDocumentFactory
        Inherits AbstractDocumentFactory

        ''' <summary>
        ''' Erstellt PDF-Dokument für DataTable und sendet es an den Client
        ''' </summary>
        ''' <param name="reportName"></param>
        ''' <param name="data"></param>
        ''' <param name="page"></param>
        ''' <param name="useSmartMarker"></param>
        ''' <param name="excelTemplatePath"></param>
        ''' <param name="colOffSet"></param>
        ''' <param name="rowOffSet"></param>
        ''' <remarks></remarks>
        Public Sub CreateDocumentAsPDFAndSendAsResponse(ByVal reportName As String, ByVal data As DataTable, ByVal page As Page, Optional ByVal useSmartMarker As Boolean = False, Optional ByVal excelTemplatePath As String = Nothing, Optional ByVal colOffSet As Integer = 0, Optional ByVal rowOffSet As Integer = 0)

            Dim xlsDoc As Aspose.Cells.WorkbookDesigner = CreateDocument(data, useSmartMarker, page.Request.PhysicalApplicationPath + excelTemplatePath, colOffSet, rowOffSet, True)

            'Datei muss physikalisch gespeichert werden, damit die Bilddateien mit nach PDF konvertiert werden
            Dim tmpFilename As String = ConfigurationManager.AppSettings("ExcelPath").ToString() + Guid.NewGuid.ToString() + ".xml"
            xlsDoc.Workbook.Save(tmpFilename, Aspose.Cells.FileFormatType.AsposePdf)

            Dim xmlDoc As New Xml.XmlDocument()
            xmlDoc.Load(tmpFilename)

            Dim pdfDoc As New Aspose.Pdf.Pdf()
            pdfDoc.IsImagesInXmlDeleteNeeded = True
            pdfDoc.BindXML(tmpFilename, Nothing)

            File.Delete(tmpFilename)

            pdfDoc.Save(reportName + ".pdf", Aspose.Pdf.SaveType.OpenInAcrobat, page.Response)

        End Sub

        Public Sub CreateExcelDocumentAsPDFAndSendAsResponse(ByVal reportName As String, ByVal data As DataTable, ByVal page As Page, Optional ByVal useSmartMarker As Boolean = False, Optional ByVal excelTemplatePath As String = Nothing, Optional ByVal colOffSet As Integer = 0, Optional ByVal rowOffSet As Integer = 0)

            Dim xlsDoc As Aspose.Cells.WorkbookDesigner = CreateDocument(data, useSmartMarker, excelTemplatePath, colOffSet, rowOffSet, True)

            'Datei muss physikalisch gespeichert werden, damit die Bilddateien mit nach PDF konvertiert werden
            Dim tmpFilename As String = ConfigurationManager.AppSettings("ExcelPath").ToString() + Guid.NewGuid.ToString() + ".xml"
            xlsDoc.Workbook.Save(tmpFilename, Aspose.Cells.FileFormatType.AsposePdf)

            Dim xmlDoc As New Xml.XmlDocument()
            xmlDoc.Load(tmpFilename)

            Dim pdfDoc As New Aspose.Pdf.Pdf()
            pdfDoc.IsImagesInXmlDeleteNeeded = True
            pdfDoc.BindXML(tmpFilename, Nothing)

            File.Delete(tmpFilename)

            pdfDoc.Save(reportName + ".pdf", Aspose.Pdf.SaveType.OpenInAcrobat, page.Response)

        End Sub

        ''' <summary>
        ''' Erstellt PDF-Dokument für DataTable und schreibt es ins Filesystem
        ''' </summary>
        ''' <param name="reportName"></param>
        ''' <param name="data"></param>
        ''' <param name="page"></param>
        ''' <param name="useSmartMarker"></param>
        ''' <param name="excelTemplatePath"></param>
        ''' <param name="colOffSet"></param>
        ''' <param name="rowOffSet"></param>
        ''' <remarks></remarks>
        Public Sub CreateDocumentAsPDFAndWriteToFilesystem(ByVal reportName As String, ByVal data As DataTable, ByVal page As Page, Optional ByVal useSmartMarker As Boolean = False, Optional ByVal excelTemplatePath As String = Nothing, Optional ByVal colOffSet As Integer = 0, Optional ByVal rowOffSet As Integer = 0)

            Dim xlsDoc As Aspose.Cells.WorkbookDesigner = CreateDocument(data, useSmartMarker, page.Request.PhysicalApplicationPath + excelTemplatePath, colOffSet, rowOffSet, True)

            'Datei muss physikalisch gespeichert werden, damit die Bilddateien mit nach PDF konvertiert werden
            Dim tmpFilename As String = ConfigurationManager.AppSettings("ExcelPath").ToString() + Guid.NewGuid.ToString() + ".xml"
            xlsDoc.Workbook.Save(tmpFilename, Aspose.Cells.FileFormatType.AsposePdf)

            Dim xmlDoc As New Xml.XmlDocument()
            xmlDoc.Load(tmpFilename)

            Dim pdfDoc As New Aspose.Pdf.Pdf()
            pdfDoc.IsImagesInXmlDeleteNeeded = True
            pdfDoc.BindXML(tmpFilename, Nothing)

            File.Delete(tmpFilename)

            pdfDoc.Save(reportName + ".pdf")

        End Sub
        
        ''' <summary>
        ''' Erstellt Excel-Dokument für DataTable und schreibt es ins Filesystem
        ''' </summary>
        ''' <param name="reportName"></param>
        ''' <param name="data"></param>
        ''' <param name="page"></param>
        ''' <param name="useSmartMarker"></param>
        ''' <param name="excelTemplatePath"></param>
        ''' <param name="colOffSet"></param>
        ''' <param name="rowOffSet"></param>
        ''' <param name="blnExcelFormat"></param>
        ''' <remarks></remarks>
        Public Sub CreateDocumentAndWriteToFilesystem(ByVal reportName As String, ByVal data As DataTable, ByVal page As Page, Optional ByVal useSmartMarker As Boolean = False, Optional ByVal excelTemplatePath As String = Nothing, Optional ByVal colOffSet As Integer = 0, Optional ByVal rowOffSet As Integer = 0, Optional ByVal blnExcelFormat As Boolean = True)

            If Not excelTemplatePath Is Nothing Then
                excelTemplatePath = page.Request.PhysicalApplicationPath + excelTemplatePath
            End If
            Dim xlsDoc As Aspose.Cells.WorkbookDesigner = CreateDocument(data, useSmartMarker, excelTemplatePath, colOffSet, rowOffSet)
            If blnExcelFormat Then
                xlsDoc.Workbook.Save(reportName, Aspose.Cells.FileFormatType.Excel2000)
            Else
                xlsDoc.Workbook.Save(Replace(reportName, "xls", "csv"), ";"c)
            End If
        End Sub

        ''' <summary>
        ''' Erstellt Excel-Dokument für DataTable und schreibt es ins Filesystem mit Pfadangabe
        ''' </summary>
        ''' <param name="reportPath"></param>
        ''' <param name="data"></param>
        ''' <param name="useSmartMarker"></param>
        ''' <param name="excelTemplatePath"></param>
        ''' <param name="colOffSet"></param>
        ''' <param name="rowOffSet"></param>
        ''' <param name="blnExcelFormat"></param>
        ''' <remarks></remarks>
        Public Sub CreateDocumentAndWriteToFilesystemWithPath(ByVal reportPath As String, ByVal data As DataTable, Optional ByVal useSmartMarker As Boolean = False, Optional ByVal excelTemplatePath As String = Nothing, Optional ByVal colOffSet As Integer = 0, Optional ByVal rowOffSet As Integer = 0, Optional ByVal blnExcelFormat As Boolean = True)

            Dim xlsDoc As Aspose.Cells.WorkbookDesigner = CreateDocument(data, useSmartMarker, excelTemplatePath, colOffSet, rowOffSet)
            If blnExcelFormat Then
                xlsDoc.Workbook.Save(reportPath, Aspose.Cells.FileFormatType.Excel2000)
            Else
                xlsDoc.Workbook.Save(Replace(reportPath, "xls", "csv"), ";"c)
            End If
        End Sub

        ''' <summary>
        ''' Erstellt Excel-Dokument für DataTable und sendet es an den Client
        ''' </summary>
        ''' <param name="reportName"></param>
        ''' <param name="data"></param>
        ''' <param name="page"></param>
        ''' <param name="useSmartMarker"></param>
        ''' <param name="excelTemplatePath"></param>
        ''' <param name="colOffSet"></param>
        ''' <param name="rowOffSet"></param>
        ''' <remarks></remarks>
        Public Sub CreateDocumentAndSendAsResponse(ByVal reportName As String, ByVal data As DataSet, ByVal page As Page, Optional ByVal useSmartMarker As Boolean = False, Optional ByVal excelTemplatePath As String = Nothing, Optional ByVal colOffSet As Integer = 0, Optional ByVal rowOffSet As Integer = 0)

            Dim xlsDoc As Aspose.Cells.WorkbookDesigner = CreateDocument(data, colOffSet, rowOffSet)

            xlsDoc.Workbook.Save(reportName + ".xls", Aspose.Cells.FileFormatType.Excel2000, Aspose.Cells.SaveType.OpenInExcel, page.Response)
        End Sub

        Public Sub CreateDocumentAndSendAsResponse(ByVal reportName As String, ByVal data As DataTable, ByVal page As Page, Optional ByVal useSmartMarker As Boolean = False, Optional ByVal excelTemplatePath As String = Nothing, Optional ByVal colOffSet As Integer = 0, Optional ByVal rowOffSet As Integer = 0)

            If Not excelTemplatePath Is Nothing Then
                excelTemplatePath = page.Request.PhysicalApplicationPath + excelTemplatePath
            End If
            Dim xlsDoc As Aspose.Cells.WorkbookDesigner = CreateDocument(data, useSmartMarker, excelTemplatePath, colOffSet, rowOffSet)

            xlsDoc.Workbook.Save(reportName + ".xls", Aspose.Cells.FileFormatType.Excel2000, Aspose.Cells.SaveType.OpenInExcel, page.Response)

        End Sub

        Public Sub CreateDocumentAndSendAsResponse(ByVal reportName As String, ByVal data As DataTable, ByVal page As HttpResponse, Optional ByVal useSmartMarker As Boolean = False, Optional ByVal excelTemplatePath As String = Nothing, Optional ByVal colOffSet As Integer = 0, Optional ByVal rowOffSet As Integer = 0)

            Dim xlsDoc As Aspose.Cells.WorkbookDesigner = CreateDocument(data, useSmartMarker, excelTemplatePath, colOffSet, rowOffSet)

            xlsDoc.Workbook.Save(reportName + ".xls", Aspose.Cells.FileFormatType.Excel2000, Aspose.Cells.SaveType.OpenInExcel, page)
        End Sub

        Private Function CreateDocument(ByVal data As DataSet, Optional ByVal colOffSet As Integer = 0, Optional ByVal rowOffSet As Integer = 0, Optional ByVal doAlternatingRowStyle As Boolean = False) As Aspose.Cells.WorkbookDesigner

            '-----
            'Excel-Dokument laden
            '-----
            Dim xlsDoc As New Aspose.Cells.WorkbookDesigner()

            Dim iReport As Integer = 0
            Dim table As DataTable
            For Each table In data.Tables
                If iReport > 0 Then
                    xlsDoc.Workbook.Worksheets.Add(Aspose.Cells.SheetType.Worksheet)
                End If
                If table.TableName.Length > 0 Then
                    xlsDoc.Workbook.Worksheets(iReport).Name = table.TableName
                Else
                    xlsDoc.Workbook.Worksheets(iReport).Name = "Report" & (iReport + 1).ToString
                End If

                '-----
                'Füllen des Workbooks mit Daten
                '-----
                FillWorkbookDynamically(xlsDoc.Workbook, iReport, table, colOffSet, rowOffSet, doAlternatingRowStyle)

                xlsDoc.Workbook.Worksheets(iReport).AutoFitColumns()
                iReport += 1
            Next

            Return xlsDoc

        End Function

        Private Function CreateDocument(ByVal data As DataTable, Optional ByVal useSmartMarker As Boolean = False, Optional ByVal excelTemplatePath As String = Nothing, Optional ByVal colOffSet As Integer = 0, Optional ByVal rowOffSet As Integer = 0, Optional ByVal doAlternatingRowStyle As Boolean = False) As Aspose.Cells.WorkbookDesigner

            '-----
            'Excel-Dokument laden
            '-----
            Dim xlsDoc As New Aspose.Cells.WorkbookDesigner()
            If Not excelTemplatePath Is Nothing Then
                Dim xlsStream As New FileStream(excelTemplatePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite)
                xlsDoc.Workbook.Open(xlsStream, Aspose.Cells.FileFormatType.Default, Nothing)
                xlsStream.Close()
            Else
                xlsDoc.Workbook.Worksheets(0).Name = "Report"
            End If

            '-----
            'Füllen des Workbooks mit Daten
            '-----
            If useSmartMarker Then
                FillWorkbookForSmartMarkers(xlsDoc, data)
            Else
                FillWorkbookDynamically(xlsDoc.Workbook, data, colOffSet, rowOffSet, doAlternatingRowStyle)
            End If

            xlsDoc.Workbook.Worksheets(0).AutoFitColumns()

            Return xlsDoc

        End Function

        Public Sub ReturnTableName(ByVal reportPath As String, ByRef tblName As String)

            Dim xlsDoc As New Aspose.Cells.Workbook()

            xlsDoc.Open(reportPath)
            tblName = xlsDoc.Worksheets.Item(0).Name
            xlsDoc = Nothing
        End Sub

        Public Sub ReturnExcelTab(ByVal reportPath As String, ByVal reportName As String, ByVal page As Page)

            'einfach die Exceldatei durchreichen
            Dim xlsDoc As New Aspose.Cells.WorkbookDesigner()
            xlsDoc.Workbook.Open(reportPath, Aspose.Cells.FileFormatType.Excel2003)
            xlsDoc.Workbook.Save(reportName, Aspose.Cells.FileFormatType.Excel2003, Aspose.Cells.SaveType.OpenInExcel, page.Response)
        End Sub

        Public Sub CSVGetExcelTab(ByVal reportPath As String, ByVal reportName As String, ByVal page As Page)

            Dim encode As Encoding = Encoding.GetEncoding("iso-8859-1") ' Westeuropäisch
            Dim filestream As New StreamReader(reportPath, encode)

            Dim input As String
            Dim dt As New DataTable()
            'CSV aufsplitten und in eine Datatable schreiben
            If filestream IsNot Nothing Then
                input = filestream.ReadLine()
                Dim s As String() = input.Split(New Char() {";"c})
                For i As Integer = 0 To s.Length - 1
                    dt.Columns.Add(s(i))
                Next

                Do While filestream.Peek >= 0

                    input = filestream.ReadLine()
                    s = input.Split(";"c)
                    dt.Rows.Add(s)
                Loop
            End If
            filestream.Close()
            'Exceldokument aus Datatable erstellen und ausgeben!
            CreateDocumentAndSendAsResponse(reportName, dt, page)
        End Sub

        Public Sub ReturnCsvInExcel(ByVal reportPath As String, ByVal reportName As String, ByVal page As Page)

            Dim xlsDoc As New Aspose.Cells.WorkbookDesigner()
            xlsDoc.Workbook.Open(reportPath, Aspose.Cells.FileFormatType.CSV)
            xlsDoc.Workbook.Save(reportName, Aspose.Cells.FileFormatType.CSV, Aspose.Cells.SaveType.OpenInExcel, page.Response)
        End Sub

#Region "FillMethoden"

        ''' <summary>
        ''' Füllt das Workbook auf Basis der Spalten und Daten des DataTable
        ''' </summary>
        ''' <param name="wb"></param>
        ''' <param name="worksheet"></param>
        ''' <param name="data"></param>
        ''' <param name="colOffSet"></param>
        ''' <param name="rowOffSet"></param>
        ''' <param name="doAlternatingRowStyle"></param>
        ''' <remarks></remarks>
        Private Sub FillWorkbookDynamically(ByVal wb As Aspose.Cells.Workbook, ByVal worksheet As Integer, ByVal data As DataTable, ByVal colOffSet As Integer, ByVal rowOffSet As Integer, ByVal doAlternatingRowStyle As Boolean)

            Dim dc As DataColumn
            Dim columnIndex As Integer = colOffSet

            Dim headerColor As Drawing.Color = Drawing.Color.LightGray
            Dim alternateColor As Drawing.Color = Drawing.Color.FromArgb(245, 245, 245)

            wb.ChangePalette(headerColor, 54)
            wb.ChangePalette(alternateColor, 36)

            For Each dc In data.Columns
                With wb.Worksheets(worksheet).Cells(rowOffSet, columnIndex)
                    .PutValue(Replace(dc.ColumnName, "-<br>", ""))
                    .Style.Font.IsBold = True
                    .Style.ForegroundColor = headerColor
                    .Style.Pattern = Aspose.Cells.BackgroundType.Solid
                End With

                Dim rowIndex As Integer = rowOffSet + 1
                Dim row As DataRow
                For Each row In data.Rows
                    With wb.Worksheets(worksheet).Cells(rowIndex, columnIndex)
                        .PutValue(row(dc))
                        If dc.DataType Is GetType(DateTime) Then
                            .Style.Custom = "dd.MM.yyyy"
                        End If

                        If (dc.DataType Is GetType(Int16)) _
                            Or dc.DataType Is GetType(Int32) _
                            Or dc.DataType Is GetType(Int64) _
                            Or dc.DataType Is GetType(Integer) Then
                            .Style.Custom = "0"
                        End If

                        If doAlternatingRowStyle AndAlso (rowIndex - rowOffSet) Mod 2 = 0 Then
                            .Style.ForegroundColor = alternateColor
                            .Style.Pattern = Aspose.Cells.BackgroundType.Solid
                        End If
                    End With
                    rowIndex += 1
                Next

                columnIndex += 1
            Next

        End Sub

        Private Sub FillWorkbookDynamically(ByVal wb As Aspose.Cells.Workbook, ByVal data As DataTable, ByVal colOffSet As Integer, ByVal rowOffSet As Integer, ByVal doAlternatingRowStyle As Boolean)

            Dim dc As DataColumn
            Dim columnIndex As Integer = colOffSet

            Dim headerColor As Drawing.Color = Drawing.Color.LightGray
            Dim alternateColor As Drawing.Color = Drawing.Color.FromArgb(245, 245, 245)

            wb.ChangePalette(headerColor, 54)
            wb.ChangePalette(alternateColor, 36)

            For Each dc In data.Columns
                With wb.Worksheets(0).Cells(rowOffSet, columnIndex)
                    .PutValue(Replace(dc.ColumnName, "-<br>", ""))
                    .Style.Font.IsBold = True
                    .Style.ForegroundColor = headerColor
                    .Style.Pattern = Aspose.Cells.BackgroundType.Solid
                End With

                Dim rowIndex As Integer = rowOffSet + 1
                Dim row As DataRow
                For Each row In data.Rows
                    With wb.Worksheets(0).Cells(rowIndex, columnIndex)
                        .PutValue(row(dc))
                        If dc.DataType Is GetType(DateTime) Then
                            .Style.Custom = "dd.MM.yyyy"
                        End If

                        If (dc.DataType Is GetType(Int16)) _
                            Or dc.DataType Is GetType(Int32) _
                            Or dc.DataType Is GetType(Int64) _
                            Or dc.DataType Is GetType(Integer) Then
                            .Style.Custom = "0"
                        End If

                        If doAlternatingRowStyle AndAlso (rowIndex - rowOffSet) Mod 2 = 0 Then
                            .Style.ForegroundColor = alternateColor
                            .Style.Pattern = Aspose.Cells.BackgroundType.Solid
                        End If
                    End With
                    rowIndex += 1
                Next

                columnIndex += 1
            Next

        End Sub

        ''' <summary>
        ''' Füllt das Workbook auf Basis der SmartMarker mit den Daten des DataTable
        ''' </summary>
        ''' <param name="designer"></param>
        ''' <param name="data"></param>
        ''' <remarks></remarks>
        Private Sub FillWorkbookForSmartMarkers(ByVal designer As Aspose.Cells.WorkbookDesigner, ByVal data As DataTable)

            designer.SetDataSource(data)
            designer.Process()

        End Sub

#End Region

    End Class

End Namespace

' ************************************************
' $History: ExcelDocumentFactory.vb $
' 
' *****************  Version 1  *****************
' User: Rudolpho     Date: 24.02.10   Time: 17:59
' Created in $/CKAG2/KBS/DocumentGeneration
' 
' *****************  Version 12  *****************
' User: Rudolpho     Date: 5.08.09    Time: 15:14
' Updated in $/CKAG/Base/Kernel/DocumentGeneration
' 
' *****************  Version 11  *****************
' User: Rudolpho     Date: 3.08.09    Time: 14:04
' Updated in $/CKAG/Base/Kernel/DocumentGeneration
' 
' *****************  Version 10  *****************
' User: Rudolpho     Date: 28.07.09   Time: 10:40
' Updated in $/CKAG/Base/Kernel/DocumentGeneration
' 
' *****************  Version 9  *****************
' User: Rudolpho     Date: 2.06.09    Time: 9:01
' Updated in $/CKAG/Base/Kernel/DocumentGeneration
' 
' *****************  Version 8  *****************
' User: Fassbenders  Date: 27.05.09   Time: 10:17
' Updated in $/CKAG/Base/Kernel/DocumentGeneration
' ITA: 2681
' 
' *****************  Version 7  *****************
' User: Rudolpho     Date: 13.05.09   Time: 15:45
' Updated in $/CKAG/Base/Kernel/DocumentGeneration
' 
' *****************  Version 6  *****************
' User: Rudolpho     Date: 5.09.08    Time: 9:55
' Updated in $/CKAG/Base/Kernel/DocumentGeneration
' ITA: 2197
' 
' *****************  Version 5  *****************
' User: Rudolpho     Date: 18.08.08   Time: 15:32
' Updated in $/CKAG/Base/Kernel/DocumentGeneration
' 
' *****************  Version 4  *****************
' User: Rudolpho     Date: 14.08.08   Time: 17:33
' Updated in $/CKAG/Base/Kernel/DocumentGeneration
' ITA: 2176
' 
' *****************  Version 3  *****************
' User: Rudolpho     Date: 11.04.08   Time: 11:57
' Updated in $/CKAG/Base/Kernel/DocumentGeneration
' Migration OR
' 
' *****************  Version 2  *****************
' User: Rudolpho     Date: 10.04.08   Time: 17:37
' Updated in $/CKAG/Base/Kernel/DocumentGeneration
' Migration
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 3.04.08    Time: 16:42
' Created in $/CKAG/Base/Kernel/DocumentGeneration
' 
' *****************  Version 7  *****************
' User: Rudolpho     Date: 10.08.07   Time: 11:26
' Updated in $/CKG/Base/Base/Kernel/DocumentGeneration
' Bugfix: Zugriff auf ExcelTabelle geändert AppEC- ec_17
' 
' *****************  Version 6  *****************
' User: Uha          Date: 8.08.07    Time: 17:22
' Updated in $/CKG/Base/Base/Kernel/DocumentGeneration
' Bugfixing CSV-Ausgabe
' 
' *****************  Version 5  *****************
' User: Uha          Date: 8.08.07    Time: 15:39
' Updated in $/CKG/Base/Base/Kernel/DocumentGeneration
' CSV-Schreiben in ExcelDocumentFactory integriert
' 
' *****************  Version 4  *****************
' User: Uha          Date: 12.07.07   Time: 13:39
' Updated in $/CKG/Base/Base/Kernel/DocumentGeneration
' In Excel werden jetzt auch mehrere Worksheets unterstützt
' (CreateDocument auch mit DataSet als Übergabeparameter)
' 
' *****************  Version 3  *****************
' User: Uha          Date: 9.07.07    Time: 11:45
' Updated in $/CKG/Base/Base/Kernel/DocumentGeneration
' Methode "CreateDocumentAndWriteToFilesystem" für Excel hinzugefügt
' 
' *****************  Version 2  *****************
' User: Uha          Date: 20.06.07   Time: 18:56
' Updated in $/CKG/Base/Base/Kernel/DocumentGeneration
' Excelausgabe in Aspose formatiert ganze Zahlen jetzt als ganze Zahlen
' 
' *****************  Version 1  *****************
' User: Uha          Date: 15.05.07   Time: 11:04
' Created in $/CKG/Base/Base/Kernel/DocumentGeneration
' Änderungen aus StartApplication vom 11.05.2007 (Aspose-Tool,
' DataTableHelper.vb, Customer.vb)
' 
' ************************************************