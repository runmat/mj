using System;
using System.Web;
using CkgDomainLogic.General.Services;
using DocumentTools.Services;
using GeneralTools.Models;
using GeneralTools.Services;
using MvcTools.Web;
using SapORM.Services;
using ServicesMvc;

namespace ServicesMvcTest
{
    class Program
    {
        static void Main()
        {
            Test();
        }

        static void Test()
        {
            const string fileName = @"C:\Users\JenzenM\Pictures\Susi50\test.jpg";
            const string pdfFileName = @"C:\Users\JenzenM\Pictures\Susi50\test.pdf";

            PdfDocumentFactory.CreatePdfFromImages(new[] { fileName }, pdfFileName);
        }
    }
}
