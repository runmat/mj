using CkgDomainLogic.General.Contracts;

namespace CkgDomainLogic.Archive.Contracts
{
    public interface IPdfAnzeigeDataService : ICkgGeneralDataService
    {
        byte[] GetPdf(string serverPfad, bool carportIdVerwenden, string carportId);
    }
}
