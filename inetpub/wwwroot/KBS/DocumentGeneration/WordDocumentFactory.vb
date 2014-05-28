Option Explicit On 
Option Strict On

Namespace DocumentGeneration

    '------------------------------
    'Klasse zum Erstellen von PDF-Dokumenten aus Word-Vorlagen
    '------------------------------
    Public Class WordDocumentFactory
        Inherits AbstractDocumentFactory

        Private _dataTable As DataTable
        Private _HeadTable As DataTable
        Private _imageHashTable As Hashtable

        'Speichert den letzten If-Paragraphen für bedingten Absatz. KEINE VERSCHACHTELUNG!
        Private _ifParagraph As Aspose.Words.Paragraph
        'Ergebnis der Auswertung des If-Ausdrucks
        Private _ifResult As Boolean

        ''' <summary>
        ''' ImageHasttable: Name -> Stream
        ''' </summary>
        ''' <param name="dt"></param>
        ''' <param name="imageHashTable"></param>
        ''' <remarks></remarks>
        Public Sub New(ByVal dt As DataTable, ByVal imageHashTable As Hashtable)
            _dataTable = dt
            _imageHashTable = imageHashTable
        End Sub

        ''' <summary>
        ''' Erstellt PDF-Dokument aus einer Wordvorlage und den übergebenen Daten
        ''' -Vorlagenpfad ausgehend von phys. Applikationspfad
        ''' </summary>
        ''' <param name="reportName"></param>
        ''' <param name="page"></param>
        ''' <param name="wordTemplatePath"></param>
        ''' <param name="extendDocument"></param>
        ''' <param name="elementForExtend"></param>
        ''' <remarks></remarks>
        Public Sub CreateDocument(ByVal reportName As String, ByVal page As Page, ByVal wordTemplatePath As String, Optional ByVal extendDocument As String = "", Optional ByVal elementForExtend As Object = Nothing)

            'Word-Dokument laden
            Dim docStream As New IO.FileStream(page.Request.PhysicalApplicationPath + wordTemplatePath, IO.FileMode.Open, IO.FileAccess.Read, IO.FileShare.ReadWrite)
            Dim wordDoc As New Aspose.Words.Document(docStream)
            docStream.Close()

            AddHandler wordDoc.MailMerge.MergeImageField, AddressOf MergeImageHandler
            AddHandler wordDoc.MailMerge.MergeField, AddressOf MergeFieldHandler
            wordDoc.MailMerge.RemoveEmptyParagraphs = True
            wordDoc.MailMerge.Execute(New DataTableMailMergeSource(_dataTable, True))

            Dim pdfStream As New IO.MemoryStream()

            If Not extendDocument = String.Empty Then
                Select Case extendDocument
                    Case "Tabelle"
                        addTableToWordDocument(wordDoc, CType(elementForExtend, DataTable()))
                    Case Else
                        Throw New Exception("extendBefehl konnte nicht ausgewertet werden!: " & extendDocument)
                End Select

            End If

            wordDoc.Save(pdfStream, Aspose.Words.SaveFormat.AsposePdf)

            Dim pdfDoc As New Aspose.Pdf.Pdf()
            pdfDoc.IsImagesInXmlDeleteNeeded = True
            pdfDoc.BindXML(pdfStream, Nothing)

            pdfDoc.Save(reportName + ".pdf", Aspose.Pdf.SaveType.OpenInAcrobat, page.Response)

        End Sub

        ''' <summary>
        ''' Erstellt PDF-Dokument aus einer Wordvorlage und den übergebenen Daten
        ''' -Vorlagenpfad ausgehend von phys. Applikationspfad
        ''' </summary>
        ''' <param name="reportName"></param>
        ''' <param name="page"></param>
        ''' <param name="wordTemplatePath"></param>
        ''' <param name="ht"></param>
        ''' <remarks></remarks>
        Public Sub CreateDocumentTable(ByVal reportName As String, ByVal page As Page, ByVal wordTemplatePath As String, ByVal ht As DataTable)

            'Word-Dokument laden
            _HeadTable = ht
            Dim docStream As New IO.FileStream(page.Request.PhysicalApplicationPath + wordTemplatePath, IO.FileMode.Open, IO.FileAccess.Read, IO.FileShare.ReadWrite)
            Dim wordDoc As New Aspose.Words.Document(docStream)
            docStream.Close()

            AddHandler wordDoc.MailMerge.MergeImageField, AddressOf MergeImageHandler
            AddHandler wordDoc.MailMerge.MergeField, AddressOf MergeFieldHandler
            wordDoc.MailMerge.RemoveEmptyParagraphs = True
            wordDoc.MailMerge.ExecuteWithRegions(New DataTableMailMergeSource(_HeadTable, True))
            wordDoc.MailMerge.ExecuteWithRegions(New DataTableMailMergeSource(_dataTable, False))

            Dim pdfStream As New IO.MemoryStream()
            wordDoc.Save(pdfStream, Aspose.Words.SaveFormat.AsposePdf)

            Dim pdfDoc As New Aspose.Pdf.Pdf()
            pdfDoc.IsImagesInXmlDeleteNeeded = True
            pdfDoc.BindXML(pdfStream, Nothing)

            pdfDoc.Save(reportName + ".pdf")

        End Sub

        ''' <summary>
        ''' Erstellt PDF-Dokument aus einer Wordvorlage und den übergebenen Daten
        ''' -Vorlagenpfad ausgehend von phys. Applikationspfad
        ''' </summary>
        ''' <param name="reportName"></param>
        ''' <param name="page"></param>
        ''' <param name="wordTemplatePath"></param>
        ''' <param name="ht"></param>
        ''' <remarks></remarks>
        Public Sub CreateDocumentTableAndSend(ByVal reportName As String, ByVal page As Page, ByVal wordTemplatePath As String, ByVal ht As DataTable)

            'Word-Dokument laden
            _HeadTable = ht
            Dim docStream As New IO.FileStream(page.Request.PhysicalApplicationPath + wordTemplatePath, IO.FileMode.Open, IO.FileAccess.Read, IO.FileShare.ReadWrite)
            Dim wordDoc As New Aspose.Words.Document(docStream)
            docStream.Close()

            AddHandler wordDoc.MailMerge.MergeImageField, AddressOf MergeImageHandler
            AddHandler wordDoc.MailMerge.MergeField, AddressOf MergeFieldHandler
            wordDoc.MailMerge.RemoveEmptyParagraphs = True
            wordDoc.MailMerge.ExecuteWithRegions(New DataTableMailMergeSource(_HeadTable, True))
            wordDoc.MailMerge.ExecuteWithRegions(New DataTableMailMergeSource(_dataTable, False))

            Dim pdfStream As New IO.MemoryStream()
            wordDoc.Save(pdfStream, Aspose.Words.SaveFormat.AsposePdf)

            Dim pdfDoc As New Aspose.Pdf.Pdf()
            pdfDoc.IsImagesInXmlDeleteNeeded = True
            pdfDoc.BindXML(pdfStream, Nothing)

            pdfDoc.Save(reportName + ".pdf", Aspose.Pdf.SaveType.OpenInAcrobat, page.Response)
        End Sub

        ''' <summary>
        ''' Erstellt PDF-Dokument aus einer Wordvorlage
        ''' -Vorlagenpfad ausgehend von phys. Applikationspfad
        ''' </summary>
        ''' <param name="reportName"></param>
        ''' <param name="page"></param>
        ''' <param name="wordTemplatePath"></param>
        ''' <remarks></remarks>
        Public Sub CreateDocumentAndSend(ByVal reportName As String, ByVal page As Page, ByVal wordTemplatePath As String)

            'Word-Dokument laden
            Dim docStream As New IO.FileStream(wordTemplatePath, IO.FileMode.Open, IO.FileAccess.Read, IO.FileShare.ReadWrite)
            Dim wordDoc As New Aspose.Words.Document(docStream)
            docStream.Close()

            Dim pdfStream As New IO.MemoryStream()
            wordDoc.Save(pdfStream, Aspose.Words.SaveFormat.AsposePdf)

            Dim pdfDoc As New Aspose.Pdf.Pdf()
            pdfDoc.IsImagesInXmlDeleteNeeded = True

            pdfDoc.BindXML(pdfStream, Nothing)

            pdfDoc.Save(reportName + ".pdf", Aspose.Pdf.SaveType.OpenInAcrobat, page.Response)

        End Sub

        Public Sub Returndoc(ByVal reportPath As String, ByVal reportName As String, ByVal page As Page)

            'einfach die Worddatei durchreichen
            Dim docStream As New IO.FileStream(reportPath, IO.FileMode.Open, IO.FileAccess.Read)
            Dim wordDoc As New Aspose.Words.Document(docStream)
            wordDoc.Save(reportName, Aspose.Words.SaveFormat.Doc, Aspose.Words.SaveType.OpenInWord, page.Response)

        End Sub

        Public Sub ReturnFile(ByVal reportPath As String, ByVal reportName As String, ByVal page As Page)

            'einfach die Datei durchreichen
            Dim docStream As New IO.FileStream(reportPath, IO.FileMode.Open, IO.FileAccess.Read)
            Dim wordDoc As New Aspose.Words.Document(docStream)
            wordDoc.Save(reportName, Aspose.Words.SaveFormat.Html, Aspose.Words.SaveType.OpenInBrowser, page.Response)

        End Sub

#Region "Image-Handling"

        ''' <summary>
        ''' Liefert aus dem HastTable den Stream zum Bildnamen
        ''' </summary>
        ''' <param name="name"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Private Function GetImageAsStream(ByVal name As String) As IO.Stream
            If Not _imageHashTable Is Nothing AndAlso _imageHashTable.ContainsKey(name) Then
                Return CType(_imageHashTable(name), IO.Stream)
            Else
                Return Nothing
            End If
        End Function

        ''' <summary>
        ''' Wird aufgerufen, wenn in das Word-Dokument ein Bild eingefügt werden soll
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Private Sub MergeImageHandler(ByVal sender As Object, ByVal e As Aspose.Words.Reporting.MergeImageFieldEventArgs)

            Dim s As IO.Stream = GetImageAsStream(e.FieldName)
            If Not s Is Nothing Then
                e.ImageStream = s
            End If
        End Sub

#End Region

#Region "Merge-Handling"

        ''' <summary>
        ''' Verarbeitet bedingte Absätze
        ''' Nimmt Formatierung der Datumswerte und Booleans vor
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Private Sub MergeFieldHandler(ByVal sender As Object, ByVal e As Aspose.Words.Reporting.MergeFieldEventArgs)

            If e.FieldName.StartsWith("IF:") Then
                _ifParagraph = e.Field.Start.ParentParagraph
                Try
                    _ifResult = CBool(_dataTable.Rows(e.RecordIndex)(e.FieldName.Substring(3)))
                    If _ifResult Then
                        e.Text = ""
                    Else
                        'Wird beim Endif mit gelöscht
                    End If
                Catch ex As Exception
                    Throw New Exception("Nach IF: muss ein Boolean kommen: " + e.FieldName)
                End Try

            ElseIf e.FieldName = "ENDIF" Then
                If Not _ifResult Then
                    DeleteBetweenParagraphs(_ifParagraph, e.Field.End.ParentParagraph)
                End If
                e.Text = ""
            Else
                '-------
                'Formatierungen
                '-------
                If TypeOf e.FieldValue Is DateTime Then
                    e.Text = CDate(e.FieldValue).ToShortDateString()
                ElseIf TypeOf e.FieldValue Is Boolean Then
                    If CBool(e.FieldValue) Then
                        e.Text = "Ja"
                    Else
                        e.Text = "Nein"
                    End If
                End If

            End If
        End Sub

#End Region

#Region "Helper"

        ''' <summary>
        ''' Löscht den Inhalt zwischen zwei Absätzen im Word-Dokument
        ''' </summary>
        ''' <param name="start"></param>
        ''' <param name="ende"></param>
        ''' <remarks></remarks>
        Private Sub DeleteBetweenParagraphs(ByVal start As Aspose.Words.Paragraph, ByVal ende As Aspose.Words.Paragraph)
            Dim curNode As Aspose.Words.Node = start
            While Not curNode Is Nothing AndAlso Not curNode Is ende
                Dim nextNode As Aspose.Words.Node = curNode.NextSibling
                curNode.ParentNode.RemoveChild(curNode)
                curNode = nextNode
            End While

        End Sub

#End Region

#Region "Word-Tabellen"

        ''' <summary>
        ''' fügt einem WordDokument eine oder mehrere tabellen mit Überschrift am Ende hinzu
        ''' </summary>
        ''' <param name="doc"></param>
        ''' <param name="werteArray"></param>
        ''' <remarks></remarks>
        Public Sub addTableToWordDocument(ByRef doc As Aspose.Words.Document, ByVal werteArray As DataTable())

            For Each werte As DataTable In werteArray
                Dim builder As Aspose.Words.DocumentBuilder = New Aspose.Words.DocumentBuilder(doc)
                Dim lastParagraph As Aspose.Words.Paragraph = New Aspose.Words.Paragraph(doc)
                Dim tmprun As Aspose.Words.Run
                tmprun = New Aspose.Words.Run(doc, werte.TableName)
                tmprun.Font.Name = "Arial"
                tmprun.Font.Size = 12
                tmprun.Font.Underline = Aspose.Words.Underline.Single
                tmprun.Font.Bold = True
                lastParagraph.Runs.Add(tmprun)
                lastParagraph.ParagraphFormat.Style.Font.Bold = True 'seltsamer weise formatiert er das ganze dokument um, dh alles was im dokument fett ist wird normal und anderstrum
                doc.LastSection.Body.Paragraphs.Add(lastParagraph)
                builder.MoveTo(lastParagraph) 'unten beim word dokument beginnen
                builder.InsertBreak(Aspose.Words.BreakType.LineBreak)
                lastParagraph.ParagraphFormat.Style.Font.Name = "Arial"
                builder.Font.Size = 8
                builder.StartTable() 'muss keine Rows anfangen man muss sie nur beenden
                For Each tmpColumn As DataColumn In werte.Columns 'titelRow 
                    builder.InsertCell()
                    builder.CellFormat.Width = 100
                    builder.CellFormat.VerticalAlignment = Aspose.Words.CellVerticalAlignment.Center
                    builder.CellFormat.Shading.BackgroundPatternColor = Drawing.Color.LightGray
                    builder.Writeln(tmpColumn.ColumnName)
                Next
                builder.RowFormat.Height = 10
                builder.RowFormat.HeightRule = Aspose.Words.HeightRule.Exactly
                builder.RowFormat.Borders.LineStyle = Aspose.Words.LineStyle.Single
                builder.RowFormat.Borders.LineWidth = 0.5
                builder.RowFormat.Borders.Color = Drawing.Color.Black
                builder.EndRow()
                builder.Font.Bold = False
                builder.PushFont()
                Dim zaehler As Integer
                For Each tmpRow As DataRow In werte.Rows 'daten rows
                    zaehler = 0
                    While zaehler < tmpRow.ItemArray.Length
                        builder.InsertCell()
                        builder.CellFormat.VerticalAlignment = Aspose.Words.CellVerticalAlignment.Center
                        builder.CellFormat.Shading.BackgroundPatternColor = Drawing.Color.White
                        builder.Writeln(tmpRow.Item(zaehler).ToString)
                        zaehler += 1
                    End While
                    builder.EndRow()
                Next
                builder.EndTable()

            Next
        End Sub

#End Region

    End Class

End Namespace

' ************************************************
' $History: WordDocumentFactory.vb $
' 
' *****************  Version 2  *****************
' User: Rudolpho     Date: 9.03.10    Time: 17:44
' Updated in $/CKAG2/KBS/DocumentGeneration
' 
' *****************  Version 1  *****************
' User: Rudolpho     Date: 24.02.10   Time: 17:59
' Created in $/CKAG2/KBS/DocumentGeneration
' 
' *****************  Version 5  *****************
' User: Rudolpho     Date: 2.06.09    Time: 9:01
' Updated in $/CKAG/Base/Kernel/DocumentGeneration
' 
' *****************  Version 4  *****************
' User: Jungj        Date: 9.07.08    Time: 10:32
' Updated in $/CKAG/Base/Kernel/DocumentGeneration
' ITA 2035
' 
' *****************  Version 3  *****************
' User: Jungj        Date: 9.07.08    Time: 8:43
' Updated in $/CKAG/Base/Kernel/DocumentGeneration
' ITA 2035
' 
' *****************  Version 2  *****************
' User: Jungj        Date: 7.07.08    Time: 10:26
' Updated in $/CKAG/Base/Kernel/DocumentGeneration
' ITA 2047/2035 Fahrzeughistorie Druckversion 
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 3.04.08    Time: 16:42
' Created in $/CKAG/Base/Kernel/DocumentGeneration
' 
' *****************  Version 7  *****************
' User: Rudolpho     Date: 18.02.08   Time: 16:14
' Updated in $/CKG/Base/Base/Kernel/DocumentGeneration
' Bugfix ORUDO
' 
' *****************  Version 6  *****************
' User: Rudolpho     Date: 18.02.08   Time: 13:22
' Updated in $/CKG/Base/Base/Kernel/DocumentGeneration
' Ita:1690
' 
' *****************  Version 5  *****************
' User: Rudolpho     Date: 25.01.08   Time: 12:41
' Updated in $/CKG/Base/Base/Kernel/DocumentGeneration
' 
' *****************  Version 4  *****************
' User: Rudolpho     Date: 23.01.08   Time: 10:19
' Updated in $/CKG/Base/Base/Kernel/DocumentGeneration
' ITA: 1647
' 
' *****************  Version 3  *****************
' User: Rudolpho     Date: 9.07.07    Time: 15:16
' Updated in $/CKG/Base/Base/Kernel/DocumentGeneration
' 
' *****************  Version 2  *****************
' User: Rudolpho     Date: 6.07.07    Time: 14:11
' Updated in $/CKG/Base/Base/Kernel/DocumentGeneration
' 
' *****************  Version 1  *****************
' User: Uha          Date: 15.05.07   Time: 11:04
' Created in $/CKG/Base/Base/Kernel/DocumentGeneration
' Änderungen aus StartApplication vom 11.05.2007 (Aspose-Tool,
' DataTableHelper.vb, Customer.vb)
' 
' ************************************************
