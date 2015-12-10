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
            OcrService.CreateDefinitionFromTrainingImages();
            OcrService.ParseImagesFromDefinition("_RG_NR");
        }
    }
}
