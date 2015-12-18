using System;
using CkgAbbyy;

namespace AbbyyTest
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            PdfToImages();
        }

        static void PdfToImages()
        {
            IOcrService ocr = new OcrServiceRg();

            //ocr.CreateDefinitionFromTrainingImages();
            ocr.ParseImagesFromDefinition();
        }
    }
}
