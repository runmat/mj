using GeneralTools.Services;

namespace CkgDomainLogic.General.Models
{
    public class WeatherWidgetUserSettings 
    {
        public string Country { get; set; }

        public string City { get; set; }

        public int RowSpanOverride { get; set; }
    }

    public class WeatherWidgetUserSettingsCollection : Store
    {
        public WeatherWidgetUserSettings[] Collection { get; set; }
    }
}