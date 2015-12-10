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
            OcrServiceZb2.CreateDefinitionFromTrainingImages();
            OcrServiceZb2.ParseImagesFromDefinition("_VIN");
        }
    }
}
