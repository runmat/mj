// ReSharper disable RedundantUsingDirective
using System;
using System.IO;
using System.Linq;
using System.Windows.Input;
using GeneralTools.Models;
using WpfTools4.Commands;

namespace PdfPrint
{
    public class MainViewModel
    {
        public ICommand PrintCommand { get; private set; }

        public MainViewModel()
        {
            PrintCommand = new DelegateCommand(e => Print(), e => true);
        }

        void Print()
        {
            var pdfFileName = @"C:\Users\JenzenM\Downloads\opel-seite-10.pdf";

            Helper.PrintPdf(pdfFileName);
        }
    }
}
