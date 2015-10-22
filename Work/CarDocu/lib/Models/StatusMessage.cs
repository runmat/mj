using GeneralTools.Models;
using System.Xml.Serialization;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Input;
using WpfTools4.Commands;
using CarDocu.Services;

namespace CarDocu.Models
{
    public class StatusMessage : ModelBase
    {
        public enum MessageType { Warning, Alert, Info, BarcodeOk, None }

        private string _statusmessageIconSource;
        [XmlIgnore]
        public string StatusmessageIconSource
        {
            get { return ((Image)Application.Current.TryFindResource(_statusmessageIconSource)).Source.ToString(); }
        }

        [XmlIgnore]
        public string Statusmessage { get; private set; }

        [XmlIgnore]
        public string StatusmessageTip { get; private set; }

        [XmlIgnore]
        public MessageType StatusmessageType { get; private set; }

        [XmlIgnore]
        public ICommand RemoveFromListCommand { get; private set; }

        public void ClearStatus() 
        {
            _statusmessageIconSource = "";
            Statusmessage = "";
            StatusmessageTip = "";

            SendPropertyChanged("StatusmessageIconSource");
            SendPropertyChanged("Statusmessage");
        }

        public StatusMessage(MessageType messageType, string messageText)
        {
            RemoveFromListCommand = new DelegateCommand(e => DomainService.StatusMessages.Remove(this));

            SetMessage(messageType, messageText);
        }

        public void SetMessage(MessageType messageType, string messageText) 
        {
            StatusmessageType = messageType;
            CheckMessageType();
            Statusmessage = messageText;
            SendPropertyChanged("Statusmessage");
        }

        private void CheckMessageType() 
        { 
            switch(StatusmessageType)
            {
                case MessageType.Alert:
                    _statusmessageIconSource = "image/16x16/error";           
                    StatusmessageTip = "Fehler";
                    break;
                case MessageType.Info:
                    _statusmessageIconSource = "image/16x16/information";
                    StatusmessageTip = "Information";
                    break;
                case MessageType.BarcodeOk:
                    _statusmessageIconSource = "image/16x16/barcode";
                    StatusmessageTip = "Barcode erkannt";
                    break;
                case MessageType.Warning:
                    _statusmessageIconSource = "image/16x16/warning";           
                    StatusmessageTip = "Warnung";
                    break;
                default:
                    _statusmessageIconSource = "";           
                    StatusmessageTip = "";
                    break;
            }

            SendPropertyChanged("StatusmessageIconSource");
            SendPropertyChanged("StatusmessageTip");            
        }
    }
}
