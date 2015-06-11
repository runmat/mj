using System.Collections.Generic;
using System.IO;
using PdfSharp.Drawing;
using GeneralTools.Models;

using ITextsharpHtml = iTextSharp.text.html.simpleparser;
using ITextsharpPdf = iTextSharp.text.pdf;
using ITextSharpText = iTextSharp.text;
using ITextSharpPdfDocument = PdfSharp.Pdf.PdfDocument;
using ITextSharpPdfPage = PdfSharp.Pdf.PdfPage;

namespace DocumentTools.Services
{
    public class PdfDocumentFactory : AbstractDocumentFactory
    {
        public static void CreatePdfFromImages(IEnumerable<string> imageFileNames, string pdfFileName)
        {
            var pdfDoc = new ITextSharpPdfDocument();

            foreach (var file in imageFileNames)
            {
                var pdfPage = new ITextSharpPdfPage();
                pdfDoc.Pages.Add(pdfPage);
                var xgr = XGraphics.FromPdfPage(pdfPage);
                var img = XImage.FromFile(file);
                xgr.DrawImage(img, 0, 0);
                xgr.Dispose();
            }

            pdfDoc.Save(pdfFileName);
            pdfDoc.Close();
        }

        public static byte[] HtmlToPdf(string html, string logoFileName = null, int logoX = 395, int logoY = 745)
        {
            html = html.Replace("\r\n", "");
            var document = new ITextSharpText.Document(iTextSharp.text.PageSize.A4, 50, 50, 25, 25);
            var output = new MemoryStream();
            ITextsharpPdf.PdfWriter.GetInstance(document, output);
            document.Open();

            if (logoFileName.IsNotNullOrEmpty())
            {
                var logo = iTextSharp.text.Image.GetInstance(logoFileName);
                logo.SetAbsolutePosition(logoX, logoY);
                document.Add(logo);
            }

            var parsedHtmlElements = ITextsharpHtml.HTMLWorker.ParseToList(new StringReader(html), null);
            foreach (var htmlElement in parsedHtmlElements)
                document.Add(htmlElement as ITextSharpText.IElement);

            document.Close();

            return output.ToArray();
        }
    }
}
