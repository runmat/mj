using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using PdfSharp.Drawing;
using GeneralTools.Models;
using GeneralTools.Services;
using PdfSharp;
using SmartSoft.PdfLibrary;
using ITextsharpHtml = iTextSharp.text.html.simpleparser;
using ITextsharpPdf = iTextSharp.text.pdf;
using ITextSharpText = iTextSharp.text;
using ITextSharpPdfDocument = PdfSharp.Pdf.PdfDocument;
using ITextSharpPdfPage = PdfSharp.Pdf.PdfPage;

namespace DocumentTools.Services
{
    public class PdfDocumentFactory : AbstractDocumentFactory
    {
        public static void CreatePdfFromImages(IEnumerable<string> imageFileNames, string pdfFileName, bool autoCorrectOrientation = true)
        {
            var pdfDoc = new ITextSharpPdfDocument();
            var imageCount = 0;
            foreach (var file in imageFileNames)
            {
                var pdfPage = new ITextSharpPdfPage();
                pdfDoc.Pages.Add(pdfPage);
                var xgr = XGraphics.FromPdfPage(pdfPage);
                var img = XImage.FromFile(file);

                // try to set orientation due to Exif image information:
                var processedImage = img;
                var processedFile = "";
                if (autoCorrectOrientation)
                {
                    processedFile = Path.Combine(Path.GetDirectoryName(file) ?? "", Path.GetFileNameWithoutExtension(file) + "-2" + Path.GetExtension(file));
                    ImagingService.ScaleAndSaveImage(file, processedFile, (int) (img.Size.Width > img.Size.Height ? img.Size.Width : img.Size.Height));
                    processedImage = XImage.FromFile(processedFile);
                }

                // resizing image if higher/wider then pdfpage.
                if (pdfDoc.Pages[imageCount].Width < XUnit.FromPoint(processedImage.Size.Width))
                    pdfDoc.Pages[imageCount].Width = XUnit.FromPoint(processedImage.Size.Width);

                if (pdfDoc.Pages[imageCount].Height < XUnit.FromPoint(processedImage.Size.Height))
                    pdfDoc.Pages[imageCount].Height = XUnit.FromPoint(processedImage.Size.Height);

                xgr.DrawImage(processedImage, 0, 0, processedImage.Size.Width, processedImage.Size.Height);
                xgr.Dispose();

                if (autoCorrectOrientation)
                {
                    processedImage.Dispose();
                    FileService.TryFileDelete(processedFile);
                }

                imageCount++;
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

        /// <summary>
        /// 20150528 MMA Erstellt aus mehreren PDF-Documenten eine einzige PDF-Datei und gibt diese als byte[] zurück.
        /// </summary>
        /// <param name="pdfBytes"></param>
        /// <returns>PDF-Datei als byte[]</returns>
        public static byte[] MergePdfDocuments(List<byte[]> pdfBytes)
        {
            byte[] mergedPdf = null;
            mergedPdf = PdfMerger.MergeFiles(pdfBytes, true);
            return mergedPdf;

            #region Example
            //var subDoc1 = PdfDocumentFactory.HtmlToPdf("test 1");
            //var subDoc2 = PdfDocumentFactory.HtmlToPdf("test 22222");
            //var docList = new List<byte[]>
            //    {
            //        subDoc1, subDoc2
            //    };
            //// MergePdfDocuments
            //var foo1 = PdfDocumentFactory.MergePdfDocuments(docList);
            #endregion

        }

    }
}
