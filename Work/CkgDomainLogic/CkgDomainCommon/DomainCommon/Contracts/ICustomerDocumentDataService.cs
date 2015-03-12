using System.Collections.Generic;
using CkgDomainLogic.General.Contracts;
using CkgDomainLogic.General.Database.Models;

namespace CkgDomainLogic.DomainCommon.Contracts
{
    public interface ICustomerDocumentDataService : ICkgGeneralDataService
    {
        string ApplicationKey { get; set; }
        string ReferenceKey { get; set; }

        List<CustomerDocument> AllDocuments { get; }

        List<CustomerDocument> Documents { get; }

        List<CustomerDocumentCategory> Categories { get; }

        CustomerDocument SaveDocument(CustomerDocument doc);

        int DeleteDocument(int id);

        CustomerDocumentCategory SaveCategory(CustomerDocumentCategory cat);

        int DeleteCategory(int id);
    }
}

