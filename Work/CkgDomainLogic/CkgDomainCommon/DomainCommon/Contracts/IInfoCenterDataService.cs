using System;
using System.Collections.Generic;
using CkgDomainLogic.General.Database.Models;
using CkgDomainLogic.General.Models.DataModels;

namespace CkgDomainLogic.DomainCommon.Contracts
{
    public interface IInfoCenterDataService 
    {
        List<Dokument> DokumentsForCurrentCustomer { get; }

        List<Dokument> DokumentsForCurrentGroup { get; }

        List<UserGroup> UserGroupsOfCurrentCustomer { get; }

        Dokument SaveDocument(Dokument dokument, Action<string, string> addModelError);

        bool SaveDocument(DocumentErstellenBearbeiten documentBearbeiten);

        bool DeleteDocument(Dokument dokument);

        List<DocumentType> DocumentTypes { get; }

        DocumentType CreateDocumentType(DocumentType documentType, Action<string, string> addModelError);

        DocumentType EditDocumentType(DocumentType documentType, Action<string, string> addModelError);

        int DeleteDocumentType(DocumentType documentType);

        List<DokumentRight> DocumentRights { get; }
    }
}

