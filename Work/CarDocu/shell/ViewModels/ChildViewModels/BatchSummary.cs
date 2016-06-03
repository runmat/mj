using WpfTools4.ViewModels;

namespace CarDocu.ViewModels
{
    public class BatchSummary: ViewModelBase
    {
        private bool _available;
        public bool Available
        {
            get { return _available; }
            set
            {
                _available = value;
                SendPropertyChanged("Available");
            }
        }

        private string _title;
        public string Title
        {
            get { return _title; }
            set
            {
                _title = value;
                SendPropertyChanged("Title");
            }
        }

        private bool _resultsAvailable;
        public bool ResultsAvailable
        {
            get { return _resultsAvailable; }
            set
            {
                _resultsAvailable = value;
                SendPropertyChanged("ResultsAvailable");
            }
        }

        private int _resultsTotalItems;
        public int ResultsTotalItems
        {
            get { return _resultsTotalItems; }
            set
            {
                _resultsTotalItems = value;
                SendPropertyChanged("ResultsTotalItems");
            }
        }

        private int _resultsGoodItems;
        public int ResultsGoodItems
        {
            get { return _resultsGoodItems; }
            set
            {
                _resultsGoodItems = value;
                SendPropertyChanged("ResultsGoodItems");
            }
        }

        private int _resultsBadItems;
        public int ResultsBadItems
        {
            get { return _resultsBadItems; }
            set
            {
                _resultsBadItems = value;
                SendPropertyChanged("ResultsBadItems");
            }
        }

        private string _lastAcceptedScan;
        public string LastAcceptedScan
        {
            get { return _lastAcceptedScan; }
            set
            {
                _lastAcceptedScan = value;
                SendPropertyChanged("LastAcceptedScan");
            }
        }

        private string _lastRecognizedBarcode;
        public string LastRecognizedBarcode
        {
            get { return _lastRecognizedBarcode; }
            set
            {
                _lastRecognizedBarcode = value;
                SendPropertyChanged("LastRecognizedBarcode");
            }
        }

        public bool IsCancelled { get; set; }
    }
}
