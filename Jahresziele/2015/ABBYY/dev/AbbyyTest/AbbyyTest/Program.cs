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
            IOcrService ocr = new OcrServiceZb2();

            //ocr.CreateDefinitionFromTrainingImages();
            ocr.ParseImagesFromDefinition();
        }
    }
}
