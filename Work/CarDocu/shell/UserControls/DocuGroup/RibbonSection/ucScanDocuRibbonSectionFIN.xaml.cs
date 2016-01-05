using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using CarDocu.Contracts;
using CarDocu.ViewModels;

namespace CarDocu.UserControls.DocuGroup
{
    /// <summary>
    /// Interaktionslogik für ucScanDocuRibbonSection.xaml
    /// </summary>
    public partial class ucScanDocuRibbonSectionFIN 
    {
        public ucScanDocuRibbonSectionFIN()
        {
            InitializeComponent();
        }

        void DrowDownSetFocusDelayed()
        {
            DrowDownSetFocusDelayed(100);
        }

        void DrowDownSetFocusDelayed(int delayMilliseconds)
        {
            if (DropDown.Visibility != Visibility.Visible)
                return;

            Task.Factory.StartNew(() =>
            {
                Thread.Sleep(delayMilliseconds);
            }).ContinueWith(x =>
            {
                DropDown.Focus();
            }, TaskScheduler.FromCurrentSynchronizationContext());
        }

        void DummySetFocusDelayed()
        {
            if (DropDown.Visibility != Visibility.Visible)
                return;

            Task.Factory.StartNew(() =>
            {
                Thread.Sleep(100);
            }).ContinueWith(x =>
            {
                DummyButton.Focus();
            }, TaskScheduler.FromCurrentSynchronizationContext());
        }

        private void ComboBox_GotFocus(object sender, RoutedEventArgs e)
        {
            ((ComboBox)sender).IsDropDownOpen = true;
        }

        private void ComboBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Tab)
                ProcessEnteredTag();
        }

        private void ComboBox_OnPreviewKeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (!DropDown.IsDropDownOpen)
                    DropDown.IsDropDownOpen = true;

                e.Handled = true;

                var docuVm = (DataContext as DocuViewModel);
                if (docuVm != null)
                {
                    Task.Factory.StartNew(() =>
                    {
                        docuVm.FinNumberAlertHintVisible = true;
                        Thread.Sleep(1500);
                    }).ContinueWith(x =>
                    {
                        docuVm.FinNumberAlertHintVisible = false;
                    }, TaskScheduler.FromCurrentSynchronizationContext());
                }
            }
        }

        void ProcessEnteredTag()
        { 
            var cb = DropDown;

            cb.IsDropDownOpen = false;
            var enteredText = cb.Text;

            var autoCompleteTagCloudConsumer = (DataContext as IAutoCompleteTagCloudConsumer);
            if (autoCompleteTagCloudConsumer != null)
            {
                var enteredTextTags = enteredText.Split(' ', '-', '_');
                enteredTextTags.ToList().ForEach(enteredTextTag => autoCompleteTagCloudConsumer.OnRequestProcessTag(enteredTextTag));

                cb.Text = "";
                DrowDownSetFocusDelayed();
            }
        }

        void ComboBox_TextBox_OnPreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            if (e.OriginalSource is TextBox)
                DrowDownSetFocusDelayed();

            if (e.OriginalSource is TextBlock)
                Task.Factory.StartNew(() =>
                {
                    Thread.Sleep(200);
                }).ContinueWith(x =>
                {
                    ProcessEnteredTag();
                }, TaskScheduler.FromCurrentSynchronizationContext());
        }

        private void UcScanDocuRibbonSectionFIN_OnDataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            var at = (DataContext as IAutoCompleteTagCloudConsumer);
            if (at != null)
                at.OnDeleteTagAction = DrowDownSetFocusDelayed;

            var dvm = (DataContext as DocuViewModel);
            if (dvm != null)
            {
                dvm.FocusDocumentNameSectionAction = DummySetFocusDelayed;
                dvm.OnSelectedDocTypeChangedAction = DrowDownSetFocusDelayed;
            }
        }

        private void ComboBox_OnIsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (DropDown.IsVisible)
                DrowDownSetFocusDelayed(1000);
        }

        private void ComboBox_OnPreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var mousePosition = e.MouseDevice.GetPosition(DropDown);
            if (mousePosition.X > 0 && mousePosition.X < DropDown.ActualWidth &&
                mousePosition.Y > 0) // && mousePosition.Y < DropDown.ActualHeight)
                return;

            if (!DropDown.IsDropDownOpen)
                return;

            DropDown.IsDropDownOpen = false;
        }
    }
}
