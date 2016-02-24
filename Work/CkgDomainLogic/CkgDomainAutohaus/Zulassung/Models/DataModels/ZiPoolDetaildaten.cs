using System.Collections.Generic;
using CkgDomainLogic.General.Services;
using GeneralTools.Models;
using GeneralTools.Resources;
using MvcTools.Models;

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

        [LocalizedDisplay(LocalizeConstants.EvbNumber)]
        public string EvbNrErforderlich { get; set; }

        [LocalizedDisplay(LocalizeConstants.Authorization)]
        public string VollmachtErforderlich { get; set; }

        [LocalizedDisplay(LocalizeConstants.IdCard)]
        public string PersonalausweisErforderlich { get; set; }

        [LocalizedDisplay(LocalizeConstants.BusinessRegistration)]
        public string GewerbeanmeldungErforderlich { get; set; }

        [LocalizedDisplay(LocalizeConstants.CommercialRegister)]
        public string HandelsregisterErforderlich { get; set; }

        [LocalizedDisplay(LocalizeConstants.SepaMandate)]
        public string LastschrifteinzugErforderlich { get; set; }

        [LocalizedDisplay(LocalizeConstants.Comment)]
        public string Bemerkung { get; set; }

        public List<SimpleUiListItem> ErforderlicheDokumente
        {
            get
            {
                if (Dienstleistung == "XXX")
                    return new List<SimpleUiListItem> { new SimpleUiListItem { StyleCssClass = "zipool-item-error", Text = Localize.ZiPoolMessageNoInformationForThisCase } };

                var liste = new List<SimpleUiListItem>();

                TryAddErforderlicheDokumenteEintrag(ref liste, Localize.ZBII, FahrzeugbriefErforderlich);
                TryAddErforderlicheDokumenteEintrag(ref liste, Localize.ZBI, FahrzeugscheinErforderlich);
                TryAddErforderlicheDokumenteEintrag(ref liste, Localize.Coc, CocErforderlich);
                TryAddErforderlicheDokumenteEintrag(ref liste, Localize.EvbNumber, EvbNrErforderlich);
                TryAddErforderlicheDokumenteEintrag(ref liste, Localize.Authorization, VollmachtErforderlich);
                TryAddErforderlicheDokumenteEintrag(ref liste, Localize.IdCard, PersonalausweisErforderlich);
                TryAddErforderlicheDokumenteEintrag(ref liste, Localize.BusinessRegistration, GewerbeanmeldungErforderlich);
                TryAddErforderlicheDokumenteEintrag(ref liste, Localize.CommercialRegister, HandelsregisterErforderlich);
                TryAddErforderlicheDokumenteEintrag(ref liste, Localize.SepaMandate, LastschrifteinzugErforderlich);

                return liste;
            }
        }

        private void TryAddErforderlicheDokumenteEintrag(ref List<SimpleUiListItem> liste, string bezeichnung, string wert)
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
                liste.Add(new SimpleUiListItem
                {
                    Text = string.Format("{0} ({1})", bezeichnung, ergebnisWert),
                    StyleCssClass = string.Format("zipool-item-{0}", (ergebnisWert == Localize.Original ? "original" : "copy"))
                });
        }
    }
}
