// ReSharper disable RedundantUsingDirective
using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using GeneralTools.Models;
using GeneralTools.Services;
using WpfTools4.Commands;

namespace PdfPrint
{
    public class MainViewModel : ModelBase
    {
        private bool _isPrinting;
        private int _page;
        private int _startPage;
        private int _endPage;

        public bool IsPrinting
        {
            get { return _isPrinting; }
            set { _isPrinting = value; SendPropertyChanged("IsPrinting"); }
        }

        public int Page
        {
            get { return _page; }
            set { _page = value; SendPropertyChanged("Page"); SendPropertyChanged("PageHint"); SendPropertyChanged("PageHint2"); }
        }

        public string PageHint => $"Drucke Seite {Page}";

        public string PageHint2 => $"(von {StartPage}-{EndPage})";

        public int StartPage
        {
            get { return _startPage; }
            set { _startPage = value; SendPropertyChanged("StartPage"); }
        }

        public int EndPage
        {
            get { return _endPage; }
            set { _endPage = value; SendPropertyChanged("EndPage"); }
        }

        public ICommand PrintCommand { get; private set; }

        public MainViewModel()
        {
            PrintCommand = new DelegateCommand(e => Print(), e => true);
        }

        private void Print()
        {
            var pdfFileName = @"C:\Users\JenzenM\Downloads\opel.pdf";
            var pdfOutFileName = @"C:\Users\JenzenM\Downloads\tmp.pdf";

            IsPrinting = true;
            Page = 0;
            StartPage = 21;
            EndPage = 24;
            Task.Factory.StartNew(() =>
            {
                for (var i = StartPage; i <= EndPage; i++)
                {
                    Page = i;
                    Thread.Sleep(10);

                    Thread.Sleep(700);
                    //                    Helper.PdfSplitDocument(pdfFileName, pdfOutFileName, i);
                    //                    Helper.PdfPrint(pdfOutFileName);
                    //                    FileService.TryFileDelete(pdfOutFileName);
                }
            })
            .ContinueWith(t =>
            {
                IsPrinting = false;
                Page = 0;
            }, 
            TaskScheduler.FromCurrentSynchronizationContext());
        }
    }
}
