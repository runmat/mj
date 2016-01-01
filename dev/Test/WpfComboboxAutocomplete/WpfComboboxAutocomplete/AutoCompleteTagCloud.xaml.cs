using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace WpfComboboxAutocomplete
{
    /// <summary>
    /// Interaktionslogik für AutoCompleteTagCloud.xaml
    /// </summary>
    public partial class AutoCompleteTagCloud : UserControl
    {
        public AutoCompleteTagCloud()
        {
            InitializeComponent();
        }

        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);

            DrowDownSetFocusDelayed();
        }

        void DrowDownSetFocusDelayed()
        {
            Task.Factory.StartNew(() =>
            {
                Thread.Sleep(500);
            }).ContinueWith(x =>
            {
                DropDown.Focus();
            }, TaskScheduler.FromCurrentSynchronizationContext());
        }

        private void ComboBox_GotFocus(object sender, RoutedEventArgs e)
        {
            ((ComboBox)sender).IsDropDownOpen = true;
        }

        private void ComboBox_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Tab)
            {
                var cb = (ComboBox)sender;

                cb.IsDropDownOpen = false;
                var enteredText = cb.Text;

                var autoCompleteTagCloudConsumer = (DataContext as IAutoCompleteTagCloudConsumer);
                if (autoCompleteTagCloudConsumer != null)
                { 
                    autoCompleteTagCloudConsumer.OnDropDownTabKey(enteredText);
                    cb.Text = "";
                    DrowDownSetFocusDelayed();
                }
            }
        }
    }
}
