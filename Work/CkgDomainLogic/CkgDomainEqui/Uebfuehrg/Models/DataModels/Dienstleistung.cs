using System.Xml.Serialization;
using GeneralTools.Models;
using GeneralTools.Resources;

namespace CkgDomainLogic.Uebfuehrg.Models
{
    public class Dienstleistung
    {
        [XmlIgnore]
        public string ID { get { return string.Format("{0}_{1}", DienstleistungsID, TransportTyp); } }

        public string DienstleistungsID { get; set; }

        [LocalizedDisplay(LocalizeConstants.Service)]
        public string Name { get; set; }

        public string FahrtIndex { get; set; }

        [LocalizedDisplay(LocalizeConstants.TransportType)]
        public string TransportTyp { get; set; }

        [LocalizedDisplay(LocalizeConstants.Price)]
        public decimal? Preis { get; set; }

        [LocalizedDisplay(LocalizeConstants.Price)]
        public bool PreisAvailable { get { return (Preis.GetValueOrDefault(0) > 0); } }

        [LocalizedDisplay(LocalizeConstants.Price)]
        public string PreisString
        {
            get
            {
                if (!PreisAvailable)
                    return "";

                return string.Format("{0:c}", Preis);
            }
        }

        public string MaterialNummer { get; set; }

        [XmlIgnore]
        public string MaterialNummerConverted { get { return MaterialNummer.IsNullOrEmpty() ? "X" : ""; } }

        [LocalizedDisplay(LocalizeConstants.Standard)]
        [XmlIgnore]
        public bool IstStandard { get { return SapFlagVwAG == "X"; } }

        [LocalizedDisplay(LocalizeConstants.Service)]
        [XmlIgnore]
        public string CroppedName { get { return Name.Crop(44); } }

        [XmlIgnore]
        public string TextCssClass { get { return string.Format("text{0}", IstStandard ? "" : " text_extra"); } }


        #region intern

        public string SapFlagVwAG { get; set; }
        
        #endregion
    }
}
