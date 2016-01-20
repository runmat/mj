using System.Collections.Generic;
using CkgDomainLogic.General.Services;
using GeneralTools.Models;
using GeneralTools.Resources;

namespace CkgDomainLogic.Autohaus.Models
{
    public class ZiPoolDetaildaten
    {
        [LocalizedDisplay(LocalizeConstants.Service)]
        public string Dienstleistung { get; set; }

        [LocalizedDisplay(LocalizeConstants.Commercial)]
        public bool Gewerblich { get; set; }

        [LocalizedDisplay(LocalizeConstants.ZBII)]
        public string FahrzeugbriefErforderlich { get; set; }

        [LocalizedDisplay(LocalizeConstants.ZBI)]
        public string FahrzeugscheinErforderlich { get; set; }

        [LocalizedDisplay(LocalizeConstants.Coc)]
        public string CocErforderlich { get; set; }

        [LocalizedDisplay(LocalizeConstants.CoverageCard)]
        public string DeckungskarteErforderlich { get; set; }

        [LocalizedDisplay(LocalizeConstants.Authorization)]
        public string VollmachtErforderlich { get; set; }

        [LocalizedDisplay(LocalizeConstants.IdCard)]
        public string PersonalausweisErforderlich { get; set; }

        [LocalizedDisplay(LocalizeConstants.BusinessRegistration)]
        public string GewerbeanmeldungErforderlich { get; set; }

        [LocalizedDisplay(LocalizeConstants.CommercialRegister)]
        public string HandelsregisterErforderlich { get; set; }

        [LocalizedDisplay(LocalizeConstants.DirectDebitMandate)]
        public string LastschrifteinzugErforderlich { get; set; }

        [LocalizedDisplay(LocalizeConstants.Comment)]
        public string Bemerkung { get; set; }

        public List<string> ErforderlicheDokumente
        {
            get
            {
                var liste = new List<string>();

                TryAddErforderlicheDokumenteEintrag(ref liste, Localize.ZBII, FahrzeugbriefErforderlich);
                TryAddErforderlicheDokumenteEintrag(ref liste, Localize.ZBI, FahrzeugscheinErforderlich);
                TryAddErforderlicheDokumenteEintrag(ref liste, Localize.Coc, CocErforderlich);
                TryAddErforderlicheDokumenteEintrag(ref liste, Localize.CoverageCard, DeckungskarteErforderlich);
                TryAddErforderlicheDokumenteEintrag(ref liste, Localize.Authorization, VollmachtErforderlich);
                TryAddErforderlicheDokumenteEintrag(ref liste, Localize.IdCard, PersonalausweisErforderlich);
                TryAddErforderlicheDokumenteEintrag(ref liste, Localize.BusinessRegistration, GewerbeanmeldungErforderlich);
                TryAddErforderlicheDokumenteEintrag(ref liste, Localize.CommercialRegister, HandelsregisterErforderlich);
                TryAddErforderlicheDokumenteEintrag(ref liste, Localize.DirectDebitMandate, LastschrifteinzugErforderlich);

                return liste;
            }
        }

        private void TryAddErforderlicheDokumenteEintrag(ref List<string> liste, string bezeichnung, string wert)
        {
            string ergebnisWert;

            switch (wert)
            {
                case "o":
                case "O":
                    ergebnisWert = Localize.Original;
                    break;

                case "k":
                case "K":
                    ergebnisWert = Localize.Copy;
                    break;

                default:
                    ergebnisWert = null;
                    break;
            }

            if (ergebnisWert != null)
                liste.Add(string.Format("{0} ({1})", bezeichnung, ergebnisWert));
        }
    }
}
