using System;
using System.IO;
using System.Text;
using System.Web;
using CkgDomainLogic.Fahrzeuge.ViewModels;
using CkgDomainLogic.General.Services;
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
            var vm = new UploadZb2VersandViewModel();


            const string fileName = @"C:\Users\JenzenM\Downloads\Sammelauftrag 1_DAD.csv";
            const string fileNameDest = @"C:\Users\JenzenM\Downloads\Sammelauftrag 1_DAD-2.csv";

            using (var sr = new StreamReader(fileName, Encoding.UTF8))
            using (var sw = new StreamWriter(fileNameDest, false, Encoding.Unicode))
            {
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    sw.WriteLine(line);
                }
            }

            vm.ExcelUploadFileSave(fileNameDest, null);

            var item = vm.UploadItems[8].Ansprechpartner;
            Console.WriteLine(vm.UploadItems[8].Ansprechpartner);
            Console.WriteLine(vm.UploadItems[7].Ort);
        }

        static protected byte[] GetCSVFileContent(string fileName)
        {
            var sb = new StringBuilder();
            using (var sr = new StreamReader(fileName, Encoding.Default, true))
            {
                string line;
                // Read and display lines from the file until the end of 
                // the file is reached.
                while ((line = sr.ReadLine()) != null)
                {
                    sb.AppendLine(line);
                }
            }
            var allines = sb.ToString();


            var utf8 = new UTF8Encoding();


            var preamble = utf8.GetPreamble();

            var data = utf8.GetBytes(allines);


            return data;
        }
    }
}
