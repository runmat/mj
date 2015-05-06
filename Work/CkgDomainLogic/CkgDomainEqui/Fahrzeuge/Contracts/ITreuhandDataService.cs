using System.Collections.Generic;
using CkgDomainLogic.Fahrzeuge.Models;
using CkgDomainLogic.General.Contracts;

namespace CkgDomainLogic.Fahrzeuge.Contracts
{
    public interface ITreuhandDataService
    {
        List<Treuhandbestand> GetTreuhandfreigabeFromSap(TreuhandverwaltungSelektor selector);

        List<Treuhandbestand> GetTreuhandbestandFromSap(TreuhandverwaltungSelektor selector);

        List<TreuhandKunde> GetTreuhandKundenFromSap(TreuhandverwaltungSelektor selector);

        List<TreuhandKunde> GetAuftraggeberFromSap(TreuhandverwaltungSelektor selector);

        TreuhandverwaltungSelektor GetBerechtigungenFromSap(TreuhandverwaltungSelektor selector);

        void ValidateUploadTreuhandverwaltung(TreuhandverwaltungSelektor selector);

        bool SaveUploadItems(List<TreuhandverwaltungCsvUpload> uploadItems);

        void FreigebenAblehnen(TreuhandverwaltungSelektor selector);
    }
}
