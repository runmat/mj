using System;
using System.Collections.Generic;
using CkgDomainLogic.Fahrzeuge.Models;

namespace CkgDomainLogic.Fahrzeuge.Contracts
{
    public interface IFehlteilEtikettenDataService
    {
        void ValidateUploadItems(List<FehlteilEtikett> items);

        string InsertItems(List<FehlteilEtikett> items);

        FehlteilEtikett SaveItem(FehlteilEtikett item, Action<string, string> addModelError);

        FehlteilEtikett LoadItem(string vin);

        void GetEtikettAsPdf(FehlteilEtikett item, out string errorMessage, out byte[] pdfBytes);
    }
}
