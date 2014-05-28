using System;
using System.Collections.Generic;
using CkgDomainLogic.CoC.Models;
using CkgDomainLogic.General.Contracts;

namespace CkgDomainLogic.CoC.Contracts
{
    public interface ICocErfassungDataService : ICkgGeneralDataService
    {
        List<CocEntity> CocAuftraege { get; }

        void MarkForRefreshCocOrders();

        CocEntity SaveCocOrder(CocEntity cocTyp, Action<string, string> addModelError);

        string SaveVersandBeauftragung(List<CocEntity> cocAuftraege, DruckOptionen druckOptionen);

        string SaveVersandDuplikatDruckBeauftragung(List<CocEntity> cocAuftraege, DruckOptionen druckOptionen);

        string SaveZulassung(List<CocEntity> cocAuftraege, DruckOptionen druckOptionen, string land);

        #region CSV Upload

        void ValidateUploadCocOrders(List<CsvUploadEntityDpmCoc> uploadItems);

        bool SaveUploadCocOrdersWithProofReading(List<CsvUploadEntityDpmCoc> items);

        byte[] GetCocAsPdf(string vorgangNr, string vorlage);

        void StoreCocPdfSelbstDruck(CocEntity cocItem, string vorlage);

        #endregion
    }
}
