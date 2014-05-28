using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using UniversalFileBasedLogging;

namespace XmlLoggingTool
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        LoggingClass LC;
        LogDataset ldsTemp;

        public MainWindow()
        {
            InitializeComponent();

            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.DefaultExt = "";
            dlg.Filter = "";
            dlg.Multiselect = false;
            
            // Show open file dialog box
            Nullable<bool> result = dlg.ShowDialog();  
          
            // Process open file dialog box results
            if (result == true)
            {
                // Open document
                string Path = dlg.FileName.Remove(dlg.FileName.LastIndexOf('\\'));
                LC = new LoggingClass(Path,"TheLog");
                txtPath.Text = Path;
            }

        }

        private void btnOpenXML_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                System.Diagnostics.Process.Start(LC.LogfilePathXML);
            }
            catch(Exception ex)
            {lblError.Content = ex.InnerException; }
        }

        private void btnXMLEntry_Click(object sender, RoutedEventArgs e)
        {
            LogDataset LD = new LogDataset();
            LogCustomer LC1 = new LogCustomer("CharterWay","CW12",LD);
            LogFile LF = new LogFile("File1", "Easy1", LC1, 0);
            LogEntry LE1 = new LogEntry("FileNotFound", "innerException", LF);

            LogCustomer LC2 = new LogCustomer("CharterWay2", "CW2012", LD);
            LogFile LF2 = new LogFile("File1", "Easy1", LC2, 0);
            LF2.Finished = true;
            LF2.Trys = 1;
            LogEntry LE2 = new LogEntry("Success", "innerException", LF2);
            
            LC.NewXMLEntry(LE1);
            LC.NewXMLEntry(LE2);
            LC.WriteToXML();
        }

        private void txtPath_TextChanged(object sender, TextChangedEventArgs e)
        {
            lblError.Content = "";

            string Path = txtPath.Text;

            try
            {
                LC = new LoggingClass(Path, "TheLog");
            }
            catch (Exception ex)
            {
                lblError.Content = ex.InnerException;
            }
        }

        private void btnReadXML_Click(object sender, RoutedEventArgs e)
        {
            ldsTemp = LC.ReadLog();
        }

        private void btnSaveTemp_Click(object sender, RoutedEventArgs e)
        {
            LC.NewXMLEntry(ldsTemp);
            LC.WriteToXML();
        }
    }
}
