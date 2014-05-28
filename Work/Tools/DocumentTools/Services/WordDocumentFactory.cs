using System;
using System.IO;
using System.Collections;
using System.Data;
using System.Diagnostics;
using GeneralTools.Models;

namespace DocumentTools.Services
{
    /// <summary>
    /// Erstellen von PDF-Dokumenten aus Word-Vorlagen
    /// </summary>
    public class WordDocumentFactory : AbstractDocumentFactory
    {
        private readonly DataTable _dataTable;
        private DataTable _headTable;
        private readonly Hashtable _imageHashTable;

        //Speichert den letzten If-Paragraphen für bedingten Absatz. KEINE VERSCHACHTELUNG!
        private Aspose.Words.Paragraph _ifParagraph;

        //Ergebnis der Auswertung des If-Ausdrucks
        private bool _ifResult;
        
        /// <summary>
        /// ImageHasttable: Name -> Stream
        /// </summary>
        public WordDocumentFactory(DataTable dt, Hashtable imageHashTable)
        {
            _dataTable = dt;
            _imageHashTable = imageHashTable;
        }

        /// <summary>
        /// Erstellt PDF-Dokument aus einer Wordvorlage und den übergebenen Daten
        /// </summary>
        public string CreateDocument(string reportName, string wordTemplatePath, string tempPDFPath, string extendDocument = "", object elementForExtend = null)
        {
            //Word-Dokument laden

            var docStream = new FileStream(wordTemplatePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            var wordDoc = new Aspose.Words.Document(docStream);
            docStream.Close();

            wordDoc.MailMerge.MergeImageField += MergeImageHandler;
            wordDoc.MailMerge.MergeField += MergeFieldHandler;
            wordDoc.MailMerge.RemoveEmptyParagraphs = true;
            wordDoc.MailMerge.Execute(new DataTableMailMergeSource(_dataTable, true));

            var pdfStream = new MemoryStream();

            if (extendDocument.IsNotNullOrEmpty())
            {
                switch (extendDocument)
                {
                    case "Tabelle":
                        AddTableToWordDocument(ref wordDoc, (DataTable[]) elementForExtend);
                        break;
                    default:
                        throw new Exception("Extend-Befehl konnte nicht ausgewertet werden: " + extendDocument);
                }
            }

            wordDoc.Save(pdfStream, Aspose.Words.SaveFormat.AsposePdf);

            var pdfDoc = new Aspose.Pdf.Pdf {IsImagesInXmlDeleteNeeded = true};
            pdfDoc.BindXML(pdfStream, null);

            var dir = new DirectoryInfo(tempPDFPath);

            if (!dir.Exists)
                dir.Create();

            var fi = new FileInfo(tempPDFPath + reportName + ".pdf");
            var counter = 0;

            while ((fi.Exists))
            {
                fi = new FileInfo(tempPDFPath + reportName + counter + ".pdf");
                counter += 1;
            }

            pdfDoc.Save(fi.FullName);

            return fi.FullName;
        }

        public void CreateDocumentAndSave(string reportName, string wordTemplatePath, string extendDocument = "", object elementForExtend = null)
        {
            //Word-Dokument laden

            var docStream = new FileStream(wordTemplatePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            var wordDoc = new Aspose.Words.Document(docStream);
            docStream.Close();

            wordDoc.MailMerge.MergeImageField += MergeImageHandler;
            wordDoc.MailMerge.MergeField += MergeFieldHandler;
            wordDoc.MailMerge.RemoveEmptyParagraphs = true;
            wordDoc.MailMerge.Execute(new DataTableMailMergeSource(_dataTable, true));

            var pdfStream = new MemoryStream();

            if (extendDocument.IsNotNullOrEmpty())
            {
                switch (extendDocument)
                {
                    case "Tabelle":
                        AddTableToWordDocument(ref wordDoc, (DataTable[]) elementForExtend);
                        break;
                    default:
                        throw new Exception("Extend-Befehl konnte nicht ausgewertet werden: " + extendDocument);
                }
            }

            wordDoc.Save(pdfStream, Aspose.Words.SaveFormat.AsposePdf);

            var pdfDoc = new Aspose.Pdf.Pdf {IsImagesInXmlDeleteNeeded = true};
            pdfDoc.BindXML(pdfStream, null);

            pdfDoc.Save(reportName + ".pdf");
        }

        public string CreateDocumentTable(string reportName, string tempPDFPath, string wordTemplatePath, DataTable ht)
        {
            //Word-Dokument laden
            _headTable = ht;
            var docStream = new FileStream(wordTemplatePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            var wordDoc = new Aspose.Words.Document(docStream);
            docStream.Close();

            wordDoc.MailMerge.MergeImageField += MergeImageHandler;
            wordDoc.MailMerge.MergeField += MergeFieldHandler;
            wordDoc.MailMerge.RemoveEmptyParagraphs = true;
            wordDoc.MailMerge.ExecuteWithRegions(new DataTableMailMergeSource(_dataTable, false));
            wordDoc.MailMerge.ExecuteWithRegions(new DataTableMailMergeSource(_headTable, true));
            var pdfStream = new MemoryStream();
            wordDoc.Save(pdfStream, Aspose.Words.SaveFormat.AsposePdf);

            var pdfDoc = new Aspose.Pdf.Pdf {IsImagesInXmlDeleteNeeded = true};
            pdfDoc.BindXML(pdfStream, null);

            var dir = new DirectoryInfo(tempPDFPath);

            if (!dir.Exists)
                dir.Create();

            var fi = new FileInfo(tempPDFPath + reportName + ".pdf");
            var counter = 0;

            while ((fi.Exists))
            {
                fi = new FileInfo(tempPDFPath + reportName + counter + ".pdf");
                counter += 1;
            }

            pdfDoc.Save(fi.FullName);

            return fi.FullName;
        }

        public string CreateDocumentAndSend(string reportName, string wordTemplatePath, string tempPDFPath)
        {
            //Word-Dokument laden
            var docStream = new FileStream(wordTemplatePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            var wordDoc = new Aspose.Words.Document(docStream);
            docStream.Close();

            var pdfStream = new MemoryStream();
            wordDoc.Save(pdfStream, Aspose.Words.SaveFormat.AsposePdf);

            var pdfDoc = new Aspose.Pdf.Pdf {IsImagesInXmlDeleteNeeded = true};

            pdfDoc.BindXML(pdfStream, null);

            var dir = new DirectoryInfo(tempPDFPath);

            if (!dir.Exists)
                dir.Create();

            var fi = new FileInfo(tempPDFPath + reportName + ".pdf");
            var counter = 0;

            while ((fi.Exists))
            {
                fi = new FileInfo(tempPDFPath + reportName + counter + ".pdf");
                counter += 1;
            }

            pdfDoc.Save(fi.FullName);

            return fi.FullName;
        }

        public void CreateDocumentTableAndSave(string reportName, string wordTemplatePath, DataTable ht)
        {
            //Word-Dokument laden
            _headTable = ht;
            var docStream = new FileStream(wordTemplatePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            var wordDoc = new Aspose.Words.Document(docStream);
            docStream.Close();

            wordDoc.MailMerge.MergeImageField += MergeImageHandler;
            wordDoc.MailMerge.MergeField += MergeFieldHandler;
            wordDoc.MailMerge.RemoveEmptyParagraphs = true;
            wordDoc.MailMerge.ExecuteWithRegions(new DataTableMailMergeSource(_dataTable, false));
            wordDoc.MailMerge.ExecuteWithRegions(new DataTableMailMergeSource(_headTable, true));
            var pdfStream = new MemoryStream();
            wordDoc.Save(pdfStream, Aspose.Words.SaveFormat.AsposePdf);

            var pdfDoc = new Aspose.Pdf.Pdf {IsImagesInXmlDeleteNeeded = true};
            pdfDoc.BindXML(pdfStream, null);

            pdfDoc.Save(reportName + ".pdf");
        }

        public void ReturnDoc(string reportPath, string reportName)
        {
            //einfach die Worddatei durchreichen
            var docStream = new FileStream(reportPath, FileMode.Open, FileAccess.Read);
            var wordDoc = new Aspose.Words.Document(docStream);
            wordDoc.Save(reportName, Aspose.Words.SaveFormat.Doc);
        }

        public void ReturnFile(string reportPath, string reportName)
        {
            //einfach die Datei durchreichen
            var docStream = new FileStream(reportPath, FileMode.Open, FileAccess.Read);
            var wordDoc = new Aspose.Words.Document(docStream);
            wordDoc.Save(reportName, Aspose.Words.SaveFormat.Html);
        }

        public string CreateDocumentDataset(string reportName, string wordTemplatePath, string tempPDFPath, DataTable ht, DataSet ds)
        {
            try
            {
                //Word-Dokument laden
                _headTable = ht;
                var docStream = new FileStream(wordTemplatePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                var wordDoc = new Aspose.Words.Document(docStream);
                docStream.Close();

                wordDoc.MailMerge.MergeImageField += MergeImageHandler;
                wordDoc.MailMerge.MergeField += MergeFieldHandler;
                wordDoc.MailMerge.RemoveEmptyParagraphs = true;
                foreach (DataTable dsTable in ds.Tables)
                {
                    wordDoc.MailMerge.ExecuteWithRegions(new DataTableMailMergeSource(dsTable, false));
                }

                wordDoc.MailMerge.ExecuteWithRegions(new DataTableMailMergeSource(_headTable, true));
                var pdfStream = new MemoryStream();
                wordDoc.Save(pdfStream, Aspose.Words.SaveFormat.AsposePdf);

                var pdfDoc = new Aspose.Pdf.Pdf {IsImagesInXmlDeleteNeeded = true};
                pdfDoc.BindXML(pdfStream, null);

                var dir = new DirectoryInfo(tempPDFPath);

                if (!dir.Exists)
                    dir.Create();

                var fi = new FileInfo(tempPDFPath + reportName + ".pdf");
                var counter = 0;

                while ((fi.Exists))
                {
                    fi = new FileInfo(tempPDFPath + reportName + counter + ".pdf");
                    counter += 1;
                }

                pdfDoc.Save(fi.FullName);

                return fi.FullName;
            }
            catch (Exception ex)
            {
                EventLog.WriteEntry("WordDocumentFactory", "Fehler beim erzeugen eines PDFs durch " + reportName + ": " + ex.Message);
            }

            return "";
        }

        public void CreateDocumentDatasetandSave(string reportName, string wordTemplatePath, DataTable ht, DataSet ds)
        {
            //Word-Dokument laden
            _headTable = ht;
            var docStream = new FileStream(wordTemplatePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            var wordDoc = new Aspose.Words.Document(docStream);
            docStream.Close();

            wordDoc.MailMerge.MergeImageField += MergeImageHandler;
            wordDoc.MailMerge.MergeField += MergeFieldHandler;
            wordDoc.MailMerge.RemoveEmptyParagraphs = true;
            foreach (DataTable dsTable in ds.Tables)
            {
                wordDoc.MailMerge.ExecuteWithRegions(new DataTableMailMergeSource(dsTable, false));
            }

            wordDoc.MailMerge.ExecuteWithRegions(new DataTableMailMergeSource(_headTable, true));
            var pdfStream = new MemoryStream();
            wordDoc.Save(pdfStream, Aspose.Words.SaveFormat.AsposePdf);

            var pdfDoc = new Aspose.Pdf.Pdf {IsImagesInXmlDeleteNeeded = true};
            pdfDoc.BindXML(pdfStream, null);

            pdfDoc.Save(reportName + ".pdf");
        }

        #region "Image-Handling"

        /// <summary>
        /// Liefert aus dem HastTable den Stream zum Bildnamen
        /// </summary>
        private Stream GetImageAsStream(string name)
        {
            if ((_imageHashTable != null) && _imageHashTable.ContainsKey(name))
            {
                return (Stream) _imageHashTable[name];
            }
            return null;
        }

        /// <summary>
        /// Wird aufgerufen, wenn in das Word-Dokument ein Bild eingef�gt werden soll
        /// </summary>
        private void MergeImageHandler(object sender, Aspose.Words.Reporting.MergeImageFieldEventArgs e)
        {
            var s = GetImageAsStream(e.FieldName);
            if ((s != null))
            {
                e.ImageStream = s;
            }
        }

        #endregion

        #region "Merge-Handling"

        /// <summary>
        /// Verarbeitet bedingte Absätze, nimmt Formatierung der Datumswerte und Booleans vor
        /// </summary>
        private void MergeFieldHandler(object sender, Aspose.Words.Reporting.MergeFieldEventArgs e)
        {
            if (_mBuilder == null)
            {
                _mBuilder = new Aspose.Words.DocumentBuilder(e.Document);
            }
            if (e.FieldName.StartsWith("IF:"))
            {
                _ifParagraph = e.Field.Start.ParentParagraph;
                try
                {
                    _ifResult = Convert.ToBoolean(_dataTable.Rows[e.RecordIndex][e.FieldName.Substring(3)]);
                    if (_ifResult)
                        e.Text = "";
                }
                catch (Exception)
                {
                    throw new Exception("Nach IF: muss ein Boolean kommen: " + e.FieldName);
                }
            }
            else if (e.FieldName == "ENDIF")
            {
                if (!_ifResult)
                    DeleteBetweenParagraphs(_ifParagraph, e.Field.End.ParentParagraph);
                e.Text = "";
            }
            else
            {
                //-------
                //Formatierungen
                //-------
                if (e.FieldValue is DateTime)
                {
                    e.Text = Convert.ToDateTime(e.FieldValue).ToShortDateString();
                }
                else if (e.FieldValue is bool)
                {
                    // Move the "cursor" to the current merge field.
                    _mBuilder.MoveToMergeField(e.FieldName);
                    // It is nice to give names to check boxes. Lets generate a name such as MyField21 or so.
                    var checkBoxName = string.Format("{0}{1}", e.FieldName, e.RecordIndex);
                    // Insert a check box.
                    _mBuilder.InsertCheckBox(checkBoxName, Convert.ToBoolean(e.FieldValue), 0);
                    // Nothing else to do for this field.
                }
            }
        }

        private Aspose.Words.DocumentBuilder _mBuilder;

        #endregion

        #region "Helper"

        /// <summary>
        /// Löscht den Inhalt zwischen zwei Absätzen im Word-Dokument
        /// </summary>
        private static void DeleteBetweenParagraphs(Aspose.Words.Paragraph start, Aspose.Words.Paragraph ende)
        {
            Aspose.Words.Node curNode = start;
            while ((curNode != null) && (!ReferenceEquals(curNode, ende)))
            {
                var nextNode = curNode.NextSibling;
                curNode.ParentNode.RemoveChild(curNode);
                curNode = nextNode;
            }
        }

        #endregion

        #region "Word-Tabellen"

        /// <summary>
        /// Fügt einem WordDokument eine oder mehrere tabellen mit Überschrift am Ende hinzu
        /// </summary>
        public void AddTableToWordDocument(ref Aspose.Words.Document doc, DataTable[] werteArray)
        {
            foreach (var werte in werteArray)
            {
                var builder = new Aspose.Words.DocumentBuilder(doc);
                var lastParagraph = new Aspose.Words.Paragraph(doc);
                Aspose.Words.Run tmprun;
                tmprun = new Aspose.Words.Run(doc, werte.TableName);
                tmprun.Font.Name = "Arial";
                tmprun.Font.Size = 12;
                tmprun.Font.Underline = Aspose.Words.Underline.Single;
                tmprun.Font.Bold = true;
                lastParagraph.Runs.Add(tmprun);
                lastParagraph.ParagraphFormat.Style.Font.Bold = true;
                //seltsamer weise formatiert er das ganze dokument um, dh alles was im dokument fett ist wird normal und anderstrum
                doc.LastSection.Body.Paragraphs.Add(lastParagraph);
                builder.MoveTo(lastParagraph);
                //unten beim word dokument beginnen
                builder.InsertBreak(Aspose.Words.BreakType.LineBreak);
                lastParagraph.ParagraphFormat.Style.Font.Name = "Arial";
                builder.Font.Size = 8;
                builder.StartTable();
                //muss keine Rows anfangen man muss sie nur beenden
                //titelRow 
                foreach (DataColumn tmpColumn in werte.Columns)
                {
                    builder.InsertCell();
                    builder.CellFormat.Width = 85;
                    builder.CellFormat.VerticalAlignment = Aspose.Words.CellVerticalAlignment.Center;
                    builder.CellFormat.Shading.BackgroundPatternColor = System.Drawing.Color.LightGray;
                    builder.Writeln(tmpColumn.ColumnName);
                }
                builder.RowFormat.Height = 10;
                builder.RowFormat.HeightRule = Aspose.Words.HeightRule.Exactly;
                builder.RowFormat.Borders.LineStyle = Aspose.Words.LineStyle.Single;
                builder.RowFormat.Borders.LineWidth = 0.5;
                builder.RowFormat.Borders.Color = System.Drawing.Color.Black;
                builder.EndRow();
                builder.Font.Bold = false;
                builder.PushFont();
                //daten rows
                foreach (DataRow tmpRow in werte.Rows)
                {
                    var zaehler = 0;
                    while (zaehler < tmpRow.ItemArray.Length)
                    {
                        builder.InsertCell();
                        builder.CellFormat.VerticalAlignment = Aspose.Words.CellVerticalAlignment.Center;
                        builder.CellFormat.Shading.BackgroundPatternColor = System.Drawing.Color.White;
                        builder.Writeln(tmpRow[zaehler].ToString());
                        zaehler += 1;
                    }
                    builder.EndRow();
                }
                builder.EndTable();
            }
        }

        #endregion
    }
}
