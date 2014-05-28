using System;
using System.Drawing;
using System.Windows;
using TwainDotNet;

namespace CarDocu.Services
{
    public class TwainService 
    {
        Twain _twain;
        ScanSettings _settings;
        
        private readonly Window _window;

        private static TwainService _instance;
        public static TwainService Instance { get { return (_instance ?? (_instance = new TwainService())); } }

        private Action<Bitmap> OnPageScanned { get; set; }
        private Func<Exception, bool> OnComplete { get; set; }

        public TwainService()
        {
            _window = Application.Current.MainWindow;

            InitTwain();
        }

        private void InitTwain()
        {
            try
            {
                _twain = new Twain(new WpfWindowMessageHook(_window));
            }
            catch (Exception )
            {
                _twain = null;
            }

            if (_twain == null)
            {
                _instance = null;
                return;
            }

            _twain.TransferImage += delegate(Object sender, TransferImageEventArgs args)
            {
                if (args.Image != null)
                {
                    if (OnPageScanned != null)
                        OnPageScanned(args.Image);
                }
            };

            _twain.ScanningComplete += delegate(Object sender, ScanningCompleteEventArgs args)
            {
                _window.IsEnabled = true;

                if (OnComplete != null)
                {
                    if (OnComplete(args.Exception))
                    {
                        OnComplete = null;
                    }
                    else
                        StartScan();
                }
            };

            //var sourceList = _twain.SourceNames;
            //ManualSource.ItemsSource = sourceList;
            //if (sourceList != null && sourceList.Count > 0)
            //{
            //    ManualSource.SelectedItem = sourceList[0];
            //}
        }

        private static ResolutionSettings _resolutionSettings;

        public TwainService StartScan(ResolutionSettings resolutionSettings=null, bool showTwainUI = false, bool? useDuplex=null)
        {
            if (_twain == null)
            {
                _instance = null;
                return null;
            }

            if (resolutionSettings != null)
                _resolutionSettings = resolutionSettings;

            _window.IsEnabled = false;

            _settings = new ScanSettings
            {
                Resolution = _resolutionSettings ?? ResolutionSettings.CarDocu,
                ShowTwainUI = showTwainUI,
                UseDuplex = useDuplex,
                UseDocumentFeeder = false,
            };

            try
            {
                //if (SourceUserSelected.IsChecked == true)
                //    _twain.SelectSource(ManualSource.SelectedItem.ToString());

                _twain.StartScanning(_settings);
            }
            catch (TwainException ex)
            {
                _window.IsEnabled = true;
                MessageBox.Show(ex.Message);
            }

            return this;
        }

        public TwainService Complete(Func<Exception, bool> predicate)
        {
            OnComplete = predicate;

            return this;
        }

        public TwainService PageScanned(Action<Bitmap> action)
        {
            OnPageScanned = action;

            return this;
        }
    }
}
