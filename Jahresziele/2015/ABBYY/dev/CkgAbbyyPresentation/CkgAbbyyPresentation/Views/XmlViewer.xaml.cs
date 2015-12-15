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

        protected override void OnContentStringFormatChanged(string oldContentStringFormat, string newContentStringFormat)
        {
            base.OnContentStringFormatChanged(oldContentStringFormat, newContentStringFormat);

            var xmlDocument = new XmlDocument();
            try
            {
                xmlDocument.Load(newContentStringFormat);
            }
            catch (XmlException)
            {
                MessageBox.Show("Die XML Datei ist ungültig");
                return;
            }

            XmlDocument = xmlDocument;
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

