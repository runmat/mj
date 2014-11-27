using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Xml.Serialization;
using CkgDomainLogic.General.Services;
using GeneralTools.Models;
using GeneralTools.Resources;

namespace CkgDomainLogic.KroschkeZulassung.Models
{
    public class OptionenDienstleistungen
    {
        [XmlIgnore]
        public List<Zusatzdienstleistung> AlleDienstleistungen { get; private set; }

        [XmlIgnore]
        public List<Zusatzdienstleistung> AvailableDienstleistungen { get { return AlleDienstleistungen; } }

        // Gewählte Dienstleistungen
        private string _gewaehlteDienstleistungenString;
        public string GewaehlteDienstleistungenString
        {
            get { return _gewaehlteDienstleistungenString.NotNullOrEmpty(); }
            set
            {
                _gewaehlteDienstleistungenString = value;

                if (AvailableDienstleistungen != null)
                {
                    AvailableDienstleistungen.ForEach(dl => dl.IstGewaehlt = false);
                    GewaehlteDienstleistungen.ForEach(dl => dl.IstGewaehlt = true);
                }
            }
        }

        [XmlIgnore]
        public List<Zusatzdienstleistung> GewaehlteDienstleistungen { get { return AvailableDienstleistungen.Where(dl => GewaehlteDienstleistungenString.Split(',').Contains(dl.ID)).ToList(); } }

        [XmlIgnore]
        public List<Zusatzdienstleistung> NichtGewaehlteDienstleistungen { get { return AvailableDienstleistungen.Except(AlleDienstleistungen).ToList(); } }

        public void InitDienstleistungen(List<Zusatzdienstleistung> dienstleistungen = null)
        {
            if (dienstleistungen != null)
                AlleDienstleistungen = dienstleistungen;

            if (GewaehlteDienstleistungenString.IsNullOrEmpty())
                GewaehlteDienstleistungenString = string.Join(",", AvailableDienstleistungen.Where(dl => dl.IstGewaehlt).Select(dl => dl.ID).ToList());
        }

        [LocalizedDisplay(LocalizeConstants.OnlyOneLicensePlate)]
        public bool NurEinKennzeichen { get; set; }

        [LocalizedDisplay(LocalizeConstants.LicensePlateSpecialSize)]
        public bool KennzeichenSondergroesse { get; set; }

        [DisplayName("")]
        public int KennzeichenGroesseId { get; set; }

        public Kennzeichengroesse Kennzeichengroesse
        {
            get
            {
                if (KennzeichengroesseList == null)
                    return new Kennzeichengroesse();

                var option = KennzeichengroesseList.FirstOrDefault(kg => kg.Id == KennzeichenGroesseId);
                if (option == null)
                    return new Kennzeichengroesse();

                return option;
            }
        }

        [XmlIgnore]
        static public List<Kennzeichengroesse> KennzeichengroesseList { get; set; }

        public string KennzeichenGroesseText { get { return (Kennzeichengroesse == null ? "" : Kennzeichengroesse.Groesse); } }

        [LocalizedDisplay(LocalizeConstants.SeasonalLicensePlate)]
        public bool Saisonkennzeichen { get; set; }

        [DisplayName("")]
        public string SaisonBeginn { get; set; }

        [DisplayName("")]
        public string SaisonEnde { get; set; }

        [XmlIgnore]
        public static string SaisonMonate { get { return "01;02;03;04;05;06;07;08;09;10;11;12"; } }

        [LocalizedDisplay(LocalizeConstants.Comment)]
        public string Bemerkung { get; set; }

        public string ZulassungsartMatNr { get; set; }

        public bool IstNeuzulassung { get { return (ZulassungsartMatNr.TrimStart('0') == "593"); } }

        public bool IstGebrauchtzulassung { get { return (ZulassungsartMatNr.TrimStart('0') == "588"); } }

        public bool Ist72hVersandzulassung { get { return (ZulassungsartMatNr.TrimStart('0') == "598"); } }

        public bool IstAbmeldung { get { return (ZulassungsartMatNr.TrimStart('0') == "573"); } }

        public bool IstUmkennzeichnung { get { return (ZulassungsartMatNr.TrimStart('0') == "596"); } }

        public bool IstKurzzeitzulassung { get { return (ZulassungsartMatNr.TrimStart('0') == "592"); } }

        public bool IstFirmeneigeneZulassung { get { return (ZulassungsartMatNr.TrimStart('0') == "619"); } }

        public bool IstZollzulassung { get { return (ZulassungsartMatNr.TrimStart('0') == "600"); } }

        [LocalizedDisplay(LocalizeConstants.LicensePlatesAvailable)]
        public bool KennzeichenVorhanden { get; set; }

        [LocalizedDisplay(LocalizeConstants.ReserveExistingLicenseNo)]
        public bool VorhandenesKennzeichenReservieren { get; set; }

        [LocalizedDisplay(LocalizeConstants.HoldingPeriodUntil)]
        public DateTime? HaltedauerBis { get; set; }

        [LocalizedDisplay(LocalizeConstants.LicenseNoOld)]
        public string AltesKennzeichen { get; set; }

        public string GetSummaryString()
        {
            var s = "";

            if (GewaehlteDienstleistungen != null)
                s += String.Join("<br />", GewaehlteDienstleistungen.Select(dienstleistung => dienstleistung.Name));
 
            if (NurEinKennzeichen)
                s += String.Format("<br/>{0}", Localize.OnlyOneLicensePlate);

            if (KennzeichenSondergroesse && Kennzeichengroesse != null)
                s += String.Format("<br/>{0}: {1}", Localize.LicensePlateSpecialSize, Kennzeichengroesse.Groesse);

            if (Saisonkennzeichen)
                s += String.Format("<br/>{0}: {1}-{2}", Localize.SeasonalLicensePlate, SaisonBeginn, SaisonEnde);

            s += String.Format("<br/>{0}: {1}", Localize.Comment, Bemerkung);

            if (IstGebrauchtzulassung)
            {
                s += String.Format("<br/>{0}: {1}", Localize.LicensePlatesAvailable, KennzeichenVorhanden);
            }
            else if (IstAbmeldung)
            {
                s += String.Format("<br/>{0}: {1}", Localize.ReserveExistingLicenseNo, VorhandenesKennzeichenReservieren);
            }
            else if (IstFirmeneigeneZulassung)
            {
                s += String.Format("<br/>{0}: {1}", Localize.HoldingPeriodUntil, HaltedauerBis);
            }
            else if (IstUmkennzeichnung)
            {
                s += String.Format("<br/>{0}: {1}", Localize.LicenseNoOld, AltesKennzeichen);
            }

            return s;
        }
    }
}
