using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Xml.Serialization;
using CkgDomainLogic.General.Services;
using GeneralTools.Models;
using GeneralTools.Resources;

namespace CkgDomainLogic.Autohaus.Models
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

                SetGewaehlteDienstleistungen();
            }
        }

        private void SetGewaehlteDienstleistungen()
        {
            if (AvailableDienstleistungen != null)
            {
                AvailableDienstleistungen.ForEach(dl => dl.IstGewaehlt = false);
                GewaehlteDienstleistungen.ForEach(dl => dl.IstGewaehlt = true);
            }
        }

        [XmlIgnore]
        public List<Zusatzdienstleistung> GewaehlteDienstleistungen { get { return AvailableDienstleistungen.Where(dl => GewaehlteDienstleistungenString.Split(',').Contains(dl.MaterialNr)).ToList(); } }

        [XmlIgnore]
        public List<Zusatzdienstleistung> NichtGewaehlteDienstleistungen { get { return AvailableDienstleistungen.Except(AlleDienstleistungen).ToList(); } }

        public void InitDienstleistungen(List<Zusatzdienstleistung> dienstleistungen = null)
        {
            if (dienstleistungen != null)
                AlleDienstleistungen = dienstleistungen.Copy();

            if (GewaehlteDienstleistungenString.IsNullOrEmpty())
                GewaehlteDienstleistungenString = string.Join(",", AvailableDienstleistungen.Where(dl => dl.IstGewaehlt).Select(dl => dl.MaterialNr).ToList());
            else
                SetGewaehlteDienstleistungen();
        }

        public string ZulassungsartMatNr { get; set; }

        [LocalizedDisplay(LocalizeConstants.OnlyOneLicensePlate)]
        public bool NurEinKennzeichen { get; set; }

        [LocalizedDisplay(LocalizeConstants.LicensePlateSpecialSize)]
        public bool KennzeichenSondergroesse { get; set; }

        [DisplayName("")]
        public int? KennzeichenGroesseId { get; set; }

        // 20150826 MMA
        public bool Kennzeichenlabel { get; set; }

        public Kennzeichengroesse Kennzeichengroesse
        {
            get
            {
                if (KennzeichengroesseListForMatNr == null)
                    return new Kennzeichengroesse();

                var option = KennzeichengroesseListForMatNr.FirstOrDefault(kg => kg.Id == KennzeichenGroesseId);
                if (option == null)
                    return new Kennzeichengroesse();

                return option;
            }
        }

        [XmlIgnore]
        public static List<Kennzeichengroesse> KennzeichengroesseList { get; set; }

        [XmlIgnore]
        public List<Kennzeichengroesse> KennzeichengroesseListForMatNr
        {
            get
            {
                var liste = KennzeichengroesseList.Where(k => k.MatNr == ZulassungsartMatNr.ToInt()).OrderBy(k => k.Position).ToList();

                if (liste.Count == 0)
                {
                    liste.Add(new Kennzeichengroesse
                    {
                        Id = 1,
                        Groesse = "520x114",
                        MatNr = ZulassungsartMatNr.ToInt(),
                        Position = 1
                    });
                }
                    
                var maxPos = liste.Max(k => k.Position);

                liste.Add(new Kennzeichengroesse
                {
                    Id = 9999,
                    Groesse = "Sondermass",
                    MatNr = ZulassungsartMatNr.ToInt(),
                    Position = (maxPos + 1)
                });

                return liste;
            }
        }

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

            if (KennzeichenSondergroesse)
                s += String.Format("<br/>{0}", Localize.LicensePlateSpecialSize);

            if (Kennzeichengroesse != null)
                s += String.Format("<br/>{0}: {1}", Localize.LicensePlateSize, Kennzeichengroesse.Groesse);

            if (Saisonkennzeichen)
                s += String.Format("<br/>{0}: {1}-{2}", Localize.SeasonalLicensePlate, SaisonBeginn, SaisonEnde);

            // 20150826 MMA
            if (Kennzeichenlabel)
                s += String.Format("<br/>{0}", Localize.Etikettendruck);

            s += String.Format("<br/>{0}: {1}", Localize.Comment, Bemerkung);

            if (Zulassungsdaten.IstGebrauchtzulassung(ZulassungsartMatNr) && KennzeichenVorhanden)
            {
                s += String.Format("<br/>{0}", Localize.LicensePlatesAvailable);
            }
            else if (Zulassungsdaten.IstFirmeneigeneZulassung(ZulassungsartMatNr))
            {
                s += String.Format("<br/>{0}: {1}", Localize.HoldingPeriodUntil, (HaltedauerBis.HasValue ? HaltedauerBis.Value.ToShortDateString() : ""));
            }
            else if (Zulassungsdaten.IstUmkennzeichnung(ZulassungsartMatNr))
            {
                s += String.Format("<br/>{0}: {1}", Localize.LicenseNoOld, AltesKennzeichen);
            }

            if (s.StartsWith("<br/>"))
                s = s.SubstringTry(5);

            return s;
        }
    }
}
