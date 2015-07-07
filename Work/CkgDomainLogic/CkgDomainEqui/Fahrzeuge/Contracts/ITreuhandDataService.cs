using System.Collections.Generic;
using CkgDomainLogic.Fahrzeuge.Models;

namespace CkgDomainLogic.Fahrzeuge.Contracts
{
    public interface ITreuhandDataService
    {
        List<Treuhandbestand> GetTreuhandfreigabeFromSap(TreuhandverwaltungSelektor selector);

        List<Treuhandbestand> GetTreuhandbestandFromSap(TreuhandverwaltungSelektor selector);

        List<TreuhandKunde> GetTreuhandKundenFromSap(TreuhandverwaltungSelektor selector);

        void GetBerechtigungenFromSap(TreuhandverwaltungSelektor selector);

        void ValidateUploadTreuhandverwaltung(TreuhandverwaltungSelektor selector);

        string SaveUploadItems(List<TreuhandverwaltungCsvUpload> uploadItems);

        string FreigebenAblehnen(TreuhandverwaltungSelektor selector, bool freigeben);
    }
}
