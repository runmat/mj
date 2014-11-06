using System.IO;
using Aspose.Pdf;
using DocumentTools.Services;

namespace AsposeTest
{
    class Program
    {
        static void Main(string[] args)
        {
            //TestAsposeWordImageReplace();
            TestAsposePdf();
        }

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

        static void TestAsposePdf()
        {
            var docFactory = new PdfDocumentFactory();

            var path = @"..\..\docs\";
            var dstPdf = "TestPdf.pdf";

            var pdf = new Aspose.Pdf.Pdf();

            var sec = pdf.Sections.Add();
            sec.BackgroundImageFile = Path.Combine(path, @"img\ship.png");

            var text = new Aspose.Pdf.Text("main text kdsksdj hrfksdj hkdsjhr ksdjfh kjsf hkjsdhfkds jhfkd jshfkdsfkjdshf dskjfhdksj hdskjfhdsk j\r\nXXXX")
            {
                Left = 1,
                Top = 1,
                PositioningType = PositioningType.PageRelative
            };
            sec.Paragraphs.Add(text);

            var text2 = new Aspose.Pdf.Text("TEXT 2\r\nkmdfnkjsdfkj")
            {
                Left = 1,
                Top = 10,
                PositioningType = PositioningType.PageRelative
            };
            sec.Paragraphs.Add(text2);

            pdf.Save(Path.Combine(path, dstPdf));
        }
    }
}
