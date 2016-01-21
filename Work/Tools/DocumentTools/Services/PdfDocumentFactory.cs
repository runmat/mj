using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using PdfSharp.Drawing;
using GeneralTools.Models;
using GeneralTools.Services;
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
        public static void CreatePdfFromImages(IEnumerable<string> imageFileNames, string pdfFileName, bool autoCorrectOrientation = true, bool cropToImageSize = false)
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
                    ImagingService.ScaleAndSaveImage(file, processedFile, img.PixelWidth > img.PixelHeight ? img.PixelWidth : img.PixelHeight);

                    img.Dispose();
                    processedImage = XImage.FromFile(processedFile);
                }


                // compressing image
                var bm = (Bitmap)Image.FromFile(processedFile);
                var codecs = ImageCodecInfo.GetImageEncoders();
                ImageCodecInfo ici = null;
                foreach (var codec in codecs.Where(codec => codec.MimeType == "image/jpeg"))
                    ici = codec;
                var ep = new EncoderParameters { Param = new [] { new EncoderParameter(Encoder.Quality, (long)60) } };
                var compressedFile = Path.Combine(Path.GetDirectoryName(file) ?? "", Path.GetFileNameWithoutExtension(file) + "-3" + Path.GetExtension(file));
                bm.Save(compressedFile, ici, ep);
                processedImage.Dispose();
                processedImage = XImage.FromFile(compressedFile);


                // resizing image if higher/wider then pdfpage.
                if (cropToImageSize || pdfDoc.Pages[imageCount].Width < XUnit.FromPoint(processedImage.PixelWidth))
                    pdfDoc.Pages[imageCount].Width = XUnit.FromPoint(processedImage.PixelWidth);

                if (cropToImageSize || pdfDoc.Pages[imageCount].Height < XUnit.FromPoint(processedImage.PixelHeight))
                    pdfDoc.Pages[imageCount].Height = XUnit.FromPoint(processedImage.PixelHeight);

                xgr.DrawImage(processedImage, 0, 0, processedImage.PixelWidth, processedImage.PixelHeight);
                xgr.Dispose();

                if (autoCorrectOrientation)
                {
                    processedImage.Dispose();
                    FileService.TryFileDelete(processedFile);
                }
                else
                    img.Dispose();

                FileService.TryFileDelete(compressedFile);

                imageCount++;
            }

            pdfDoc.Save(pdfFileName);
            pdfDoc.Close();
        }

        public static void ScanClientCreatePdfFromImages(IEnumerable<string> imageFileNames, string pdfFileName, string pdfPassword = "")
        {
            var pdfDoc = new ITextSharpPdfDocument();
            var imageCount = 0;
            foreach (var file in imageFileNames)
            {
                var pdfPage = new ITextSharpPdfPage();
                pdfDoc.Pages.Add(pdfPage);
                var xgr = XGraphics.FromPdfPage(pdfPage);
                var img = XImage.FromFile(file);

                // resizing image if higher/wider then pdfpage.
                pdfDoc.Pages[imageCount].Width = XUnit.FromPoint(img.Size.Width);
                pdfDoc.Pages[imageCount].Height = XUnit.FromPoint(img.Size.Height);

                xgr.DrawImage(img, 0, 0, img.Size.Width, img.Size.Height);
                xgr.Dispose();
                img.Dispose();

                imageCount++;
            }

            if (pdfPassword.IsNullOrEmpty())
            {
                pdfDoc.Save(pdfFileName);
                pdfDoc.Close();
                return;
            }

            // PDF encryption goes here:
            var tempFileName = Path.Combine(Path.GetDirectoryName(pdfFileName) ?? "", Path.GetFileNameWithoutExtension(pdfFileName) + "-temp" + Path.GetExtension(pdfFileName));
            pdfDoc.Save(tempFileName);
            pdfDoc.Close();

            using (Stream output = new FileStream(pdfFileName, FileMode.Create, FileAccess.Write, FileShare.None))
            {
                var reader = new ITextsharpPdf.PdfReader(tempFileName);
                ITextsharpPdf.PdfEncryptor.Encrypt(reader, output, false, pdfPassword, pdfPassword, ITextsharpPdf.PdfWriter.ALLOW_COPY);
            }

            FileService.TryFileDelete(tempFileName);
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
            var mergedPdf = PdfMerger.MergeFiles(pdfBytes, true);
            return mergedPdf;
        }
    }
}
