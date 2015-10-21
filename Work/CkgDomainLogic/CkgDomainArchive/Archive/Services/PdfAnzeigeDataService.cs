using System;
using System.IO;
using CkgDomainLogic.General.Services;
using CkgDomainLogic.Archive.Contracts;

namespace CkgDomainLogic.Archive.Services
{
    public class PdfAnzeigeDataService : CkgGeneralDataService, IPdfAnzeigeDataService
    {
        public byte[] GetPdf(string serverPfad, bool carportIdVerwenden, string carportId)
        {
            var dateiPfad = (carportIdVerwenden ? String.Format(serverPfad, carportId) : serverPfad);

            if (File.Exists(dateiPfad))
                return File.ReadAllBytes(dateiPfad);

            return new byte[] {};
        }
    }
}
