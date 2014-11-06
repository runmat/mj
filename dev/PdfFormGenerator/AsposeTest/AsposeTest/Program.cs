using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.IO;
using Aspose.Pdf;
using DocumentTools.Services;
using GeneralTools.Services;
using Loader;
using System.Linq;
using ToolboxLibrary;

namespace AsposeTest
{
    class Program
    {
        static void Main(string[] args)
        {
            //TestAsposeWordImageReplace();

            TestDeserializeObjects();
        }


        #region Winword Image Replace

        static void TestAsposeWordImageReplace()
        {
            var docFactory = new PdfDocumentFactory();

            var path = @"..\..\docs\";
            var srcDoc = "lasagne.doc";
            var dstDoc = "asposeword.doc";
            var dstPdf = "asposeword.pdf";

            var doc = new Aspose.Words.Document(Path.Combine(path, srcDoc));
            var builder = new Aspose.Words.DocumentBuilder(doc);

            var shape = (Aspose.Words.Drawing.Shape)doc.GetChild(Aspose.Words.NodeType.Shape, 0, true);
            builder.MoveTo(shape);
            var para = shape.GetAncestor(Aspose.Words.NodeType.Paragraph);
            builder.MoveTo(para);

            shape.Remove();
            builder.InsertImage(Path.Combine(path, @"img\ship.png"), shape.SizeInPoints.Width, shape.SizeInPoints.Height);

            //doc.Save(Path.Combine(path, dstDoc));

            var pdfStream = new MemoryStream();
            doc.Save(pdfStream, Aspose.Words.SaveFormat.AsposePdf);

            var pdfDoc = new Aspose.Pdf.Pdf { IsImagesInXmlDeleteNeeded = true };
            pdfDoc.BindXML(pdfStream, null);
            pdfDoc.Save(Path.Combine(path, dstPdf));
        }

        #endregion


        #region Create PDF from XML form

        static void TestDeserializeObjects()
        {
            var xmlFileName = @"..\..\..\..\DesignerHosting\Shell\bin\Debug\test.xml";

            var loader = new BasicHostLoader(xmlFileName);
            loader.PerformLoad();

            var form = loader.PdfForm;
            var image = form.BackgroundImage;
            var labels = form.Controls.OfType<PdfLabel>().ToList();

            var tempFolder = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            var tmpFileName = Path.Combine(tempFolder, "pdfBackground.jpg");
            image.Save(tmpFileName, ImageFormat.Jpeg);

            CreatePdfFromForm(tmpFileName, labels);

            FileService.TryFileDelete(tmpFileName);
        }

        private static void CreatePdfFromForm(string backgroundImageFileName, IList<PdfLabel> labels)
        {
            var docFactory = new PdfDocumentFactory();

            var path = @"..\..\docs\";
            var dstPdf = "Form.pdf";

            var pdf = new Aspose.Pdf.Pdf();

            var sec = pdf.Sections.Add();
            sec.BackgroundImageFile = backgroundImageFileName;

            foreach (var label in labels)
            {
                var text = new Aspose.Pdf.Text(string.Format("[{0}]", label.Text))
                {
                    Left = label.Left,
                    Top = label.Top,
                    PositioningType = PositioningType.PageRelative
                };
                sec.Paragraphs.Add(text);
            }

            pdf.Save(Path.Combine(path, dstPdf));
        }

        #endregion
    }
}
