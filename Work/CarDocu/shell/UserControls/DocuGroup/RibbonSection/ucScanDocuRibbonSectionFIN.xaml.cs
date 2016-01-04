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

            DrowDownSetFocusDelayed(1500);
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

        private void ComboBox_GotFocus(object sender, RoutedEventArgs e)
        {
            ((ComboBox)sender).IsDropDownOpen = true;
        }

        private void ComboBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Tab)
                ProcessEnteredTag();
        }

        private void DropDown_OnPreviewKeyUp(object sender, KeyEventArgs e)
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
                autoCompleteTagCloudConsumer.OnRequestProcessTag(enteredText);
                cb.Text = "";
                DrowDownSetFocusDelayed();
            }
        }

        void ComboBox_TextBox_OnMouseDown(object sender, MouseButtonEventArgs e)
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
                at.AfterDeleteAction = DrowDownSetFocusDelayed;
        }
    }
}
