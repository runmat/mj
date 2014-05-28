using GeneralTools.Models;

namespace CarDocu.Models.Settings
{
    public class SapSettings : ModelBase
    {
        private string _kundenNr = "10051385"; 
        public string KundenNr 
        { 
            get { return _kundenNr; }
            set { _kundenNr = value; SendPropertyChanged("KundenNr"); }
        }

        private string _webServiceUrl = "https://sgwt.kroschke.de/CarDocu/Service.asmx"; 
        public string WebServiceUrl 
        { 
            get { return _webServiceUrl; }
            set { _webServiceUrl = value; SendPropertyChanged("WebServiceUrl"); }
        }
    }
}
