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
using System.Xml;

namespace CkgAbbyyPresentation.Views
{
    /// <summary>
    /// Interaktionslogik für XmlViewer.xaml
    /// </summary>
    public partial class XmlViewer : UserControl
    {
        private XmlDocument _xmldocument;
        public XmlViewer()
        {
            InitializeComponent();

            var xmlDocument = new XmlDocument();
            try
            {
                xmlDocument.Load(System.IO.Path.Combine(@"C:\Backup\ABBYY\ZBII\export", "JMZBK14Z261351772.xml"));
            }
            catch (XmlException)
            {
                MessageBox.Show("The XML file is invalid");
                return;
            }

            XmlDocument = xmlDocument;
        }

        public XmlDocument XmlDocument
        {
            get { return _xmldocument; }
            set
            {
                _xmldocument = value;
                BindXmlDocument();
            }
        }

        private void BindXmlDocument()
        {
            if (_xmldocument == null)
            {
                XmlTree.ItemsSource = null;
                return;
            }

            var provider = new XmlDataProvider
            {
                Document = _xmldocument
            };
            var binding = new Binding
            {
                Source = provider,
                XPath = "child::node()"
            };

            XmlTree.SetBinding(ItemsControl.ItemsSourceProperty, binding);
        }
    }
}

