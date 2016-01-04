using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using CarDocu.Contracts;

namespace CarDocu.UserControls.DocuGroup
{
    /// <summary>
    /// Interaktionslogik für AutoCompleteTagCloud.xaml
    /// </summary>
    public partial class AutoCompleteTagCloud 
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

        private void ComboBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Tab)
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

        private void ComboBox_TextBox_OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            DrowDownSetFocusDelayed();
        }
    }
}
