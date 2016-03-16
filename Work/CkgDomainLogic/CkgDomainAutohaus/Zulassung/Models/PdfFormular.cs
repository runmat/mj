using CkgDomainLogic.General.Services;
using GeneralTools.Models;

namespace CkgDomainLogic.Autohaus.Models
{
    public class PdfFormular
    {
        public string Belegnummer { get; set; }

        public string Typ { get; set; }

        public string Label { get; set; }

        public string DateiPfad { get; set; }



        public bool IstAuftragsZettel { get { return Typ.IsNullOrEmpty() && !DateiPfad.NotNullOrEmpty().ToLower().Contains("auftragsliste"); } }

        public bool IstAuftragsListe { get { return Typ.IsNullOrEmpty() && DateiPfad.NotNullOrEmpty().ToLower().Contains("auftragsliste"); } }

        public bool IstVersandLabel { get { return Typ == "VS-LABEL"; } }

        public string LabelForGui { get { return IstAuftragsZettel ? Localize.OrderForm : Label; } }
    }
}
