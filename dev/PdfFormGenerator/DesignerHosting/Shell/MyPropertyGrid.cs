using System;
using System.ComponentModel;
using System.Linq;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace Shell
{
    public class MyPropertyGrid : PropertyGrid
    {

        private readonly Container _components = null;

        public MyPropertyGrid()
        {
            InitializeComponent();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_components != null)
                {
                    _components.Dispose();
                }
            }
            base.Dispose(disposing);
        }

        #region Codice generato da Progettazione componenti
        /// <summary> 
        /// Metodo necessario per il supporto della finestra di progettazione. Non modificare 
        /// il contenuto del metodo con l'editor di codice.
        /// </summary>
        private void InitializeComponent()
        {
            // 
            // UserControl1
            // 
            this.Name = "MyPropertyGrid";

        }
        #endregion

        protected override PropertyTab CreatePropertyTab(Type tabType)
        {
            var t = new MyTab();
            return t;
        }
    }

    public class MyTab : PropertyTab
    {
        // get the properties of the selected component
        public override PropertyDescriptorCollection GetProperties(object component, Attribute[] attributes)
        {
            var properties = TypeDescriptor.GetProperties(component, attributes);
            var newProps = new PropertyDescriptorCollection(new PropertyDescriptor[0]);

            string[] allowedProperties =
                {
                    "BackgroundImage", 
                    "Font",
                    "ForeColor",
                    "Location",
                    "Size",
                    //"Text",
                    //"Name",
                    "Visible",
                };

            foreach (PropertyDescriptor property in properties)
            {
                if (allowedProperties.Contains(property.Name))
                    newProps.Add(property);

            }

            return newProps;
        }

        public override PropertyDescriptorCollection GetProperties(object component)
        {
// ReSharper disable AssignNullToNotNullAttribute
            return this.GetProperties(component, null);
// ReSharper restore AssignNullToNotNullAttribute
        }

        // PropertyTab Name
        public override string TabName
        {
            get
            {
                return "Properties";
            }
        }

        //Image of the property tab (return a blank 16x16 Bitmap)
        public override Bitmap Bitmap
        {
            get
            {
                return new Bitmap(16, 16);
            }
        }
    }

}
