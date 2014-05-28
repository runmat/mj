using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Linq.Expressions;
using System.Xml.Serialization;

namespace WpfTools4.ViewModels
{
    public class ViewModelBase : INotifyPropertyChanged
    {
        /// <summary>
        /// Raised when a property on this object has a new value
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Raises this ViewModels PropertyChanged event
        /// </summary>
        /// <param name="propertyName">Name of the property that has a new value</param>
        protected void SendPropertyChanged(string propertyName)
        {
            SendPropertyChanged(new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// Raises this ViewModels PropertyChanged event
        /// </summary>
        /// <param name="e">Arguments detailing the change</param>
        protected virtual void SendPropertyChanged(PropertyChangedEventArgs e)
        {
            var handler = this.PropertyChanged;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        public void SendPropertyChanged<TProperty>(Expression<Func<TProperty>> property)
        {
            var lambda = (LambdaExpression)property;

            MemberExpression memberExpression;
            if (lambda.Body is UnaryExpression)
            {
                var unaryExpression = (UnaryExpression)lambda.Body;
                memberExpression = (MemberExpression)unaryExpression.Operand;
            }
            else
            {
                memberExpression = (MemberExpression)lambda.Body;
            }
            SendPropertyChanged(memberExpression.Member.Name);
        }

        //private bool _insideSendRadioButtonPropertyChanged;
        //protected void SendRadioButtonPropertyChanged(string propertyName, string propertyGroup, bool value)
        //{
        //    var type = this.GetType();

        //    if (_insideSendRadioButtonPropertyChanged)
        //    {
        //        SendPropertyChanged(propertyName);
        //        return;
        //    }

        //    if (!value)
        //    {
        //        // verhindern dass man einen gecheckten RadioButton durch Mausklick wieder de-checken kann (Radio Button Behaviour)
        //        var propertyInfoThis = type.GetProperty(propertyName);
        //        propertyInfoThis.SetValue(this, true, null);
        //        SendPropertyChanged(propertyName);
        //        return;
        //    }

        //    _insideSendRadioButtonPropertyChanged = true;

        //    var propertiesInGroup = type.GetProperties().Where(p => p.Name.StartsWith(propertyGroup));
        //    foreach (var propertyInfo in propertiesInGroup)
        //    {
        //        if (propertyInfo.Name != propertyName)
        //            propertyInfo.SetValue(this, false, null);

        //        SendPropertyChanged(propertyInfo.Name);
        //    }

        //    _insideSendRadioButtonPropertyChanged = false;
        //}



        #region XpsDocument Viewer helper properties

        private bool _xpsDocumentVisible;
        [XmlIgnore]
        [NotMapped]
        public bool XpsDocumentVisible
        {
            get
            {
                return _xpsDocumentVisible;
            }
            set
            {
                _xpsDocumentVisible = value;
                SendPropertyChanged("XpsDocumentVisible");
                SendPropertyChanged("XpsDocumentVisibleOpacity");

                if (!_xpsDocumentVisible)
                    XpsDocumentOnHide();

                if (_xpsDocumentVisible)
                    XpsDocumentVisibleEnabled = true;
            }
        }

        virtual protected void XpsDocumentOnHide()
        {
        }

        private bool _xpsDocumentVisibleEnabled;
        [XmlIgnore]
        [NotMapped]
        public bool XpsDocumentVisibleEnabled
        {
            get
            {
                return _xpsDocumentVisibleEnabled;
            }
            set
            {
                _xpsDocumentVisibleEnabled = value;
                SendPropertyChanged("XpsDocumentVisibleEnabled");
            }
        }

        [XmlIgnore]
        [NotMapped]
        public double XpsDocumentVisibleOpacity 
        { 
            get { return (XpsDocumentVisible ? 0.3 : 1.0); }
        }



        private bool _xpsDocument2Visible;
        [XmlIgnore]
        [NotMapped]
        public bool XpsDocument2Visible
        {
            get
            {
                return _xpsDocument2Visible;
            }
            set
            {
                _xpsDocument2Visible = value;
                SendPropertyChanged("XpsDocument2Visible");
                SendPropertyChanged("XpsDocument2VisibleOpacity");

                if (_xpsDocument2Visible)
                    XpsDocument2VisibleEnabled = true;
            }
        }

        private bool _xpsDocument2VisibleEnabled;
        [XmlIgnore]
        [NotMapped]
        public bool XpsDocument2VisibleEnabled
        {
            get
            {
                return _xpsDocument2VisibleEnabled;
            }
            set
            {
                _xpsDocument2VisibleEnabled = value;
                SendPropertyChanged("XpsDocument2VisibleEnabled");
            }
        }

        [XmlIgnore]
        [NotMapped]
        public double XpsDocument2VisibleOpacity
        {
            get { return (XpsDocument2Visible ? 0.3 : 1.0); }
        }

        private bool _xpsDocument3Visible;
        [XmlIgnore]
        [NotMapped]
        public bool XpsDocument3Visible
        {
            get
            {
                return _xpsDocument3Visible;
            }
            set
            {
                _xpsDocument3Visible = value;
                SendPropertyChanged("XpsDocument3Visible");
                SendPropertyChanged("XpsDocument3VisibleOpacity");

                if (_xpsDocument3Visible)
                    XpsDocument3VisibleEnabled = true;
            }
        }

        private bool _xpsDocument3VisibleEnabled;
        [XmlIgnore]
        [NotMapped]
        public bool XpsDocument3VisibleEnabled
        {
            get
            {
                return _xpsDocument3VisibleEnabled;
            }
            set
            {
                _xpsDocument3VisibleEnabled = value;
                SendPropertyChanged("XpsDocument3VisibleEnabled");
            }
        }

        [XmlIgnore]
        [NotMapped]
        public double XpsDocument3VisibleOpacity
        {
            get { return (XpsDocument3Visible ? 0.3 : 1.0); }
        }

        #endregion 
    }
}
