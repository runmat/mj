using System;
using GeneralTools.Models;

namespace CarDocu.Models
{
    public class CardocuQueueEntity : ModelBase
    {
        private string _documentID;
        public string DocumentID
        {
            get { return _documentID; }
            set { _documentID = value; SendPropertyChanged("DocumentID"); }
        }

        private string _finNummer;
        public string FinNummer
        {
            get { return _finNummer; }
            set { _finNummer = value; SendPropertyChanged("FinNummer"); }
        }

        private DateTime? _deliveryDate; 
        public DateTime? DeliveryDate 
        { 
            get { return _deliveryDate; }
            set { _deliveryDate = value; SendPropertyChanged("DeliveryDate"); }
        }

        public string KundenNr { get; set; }

        public string StandortCode { get; set; }
    }
}
