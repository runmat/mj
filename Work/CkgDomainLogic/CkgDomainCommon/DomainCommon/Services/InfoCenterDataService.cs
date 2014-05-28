using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using CkgDomainLogic.DomainCommon.Contracts;
using CkgDomainLogic.General.Contracts;
using CkgDomainLogic.General.Database.Models;
using CkgDomainLogic.General.Database.Services;
using CkgDomainLogic.General.Models;
using CkgDomainLogic.General.Models.DataModels;
using GeneralTools.Contracts;

namespace CkgDomainLogic.DomainCommon.Services
{
    public class InfoCenterDataService : IInfoCenterDataService, ICkgGeneralDataService
    {
        private DomainDbContext _domainDbContext;
        private ILogonContextDataService _logonContext;

        public InfoCenterDataService(ILogonContextDataService logonContext)
        {
            if (logonContext.User != null)
            {
                _domainDbContext = new DomainDbContext(ConfigurationManager.AppSettings["Connectionstring"], logonContext.User.UserID.ToString());    
            }
            else
            {
                _domainDbContext = new DomainDbContext(ConfigurationManager.AppSettings["Connectionstring"], string.Empty);    
            }

            _logonContext = logonContext;
        }

        public List<Dokument> DokumentsForCurrentCustomer { get { return _domainDbContext.DokumentsForAdmin; } }
        public List<Dokument> DokumentsForCurrentGroup { get { return _domainDbContext.DokumentsForGroup; }  }

        public List<UserGroup> UserGroupsOfCurrentCustomer { get { return _domainDbContext.UserGroupsOfCurrentCustomer; } }

        public Dokument SaveDocument(Dokument dokument, Action<string, string> addModelError)
        {
            return _domainDbContext.SaveDokument(dokument);
        }

        public bool DeleteDocument(Dokument dokument)
        {
            return _domainDbContext.DeleteDokument(dokument);
        }

        public bool SaveDocument(DocumentErstellenBearbeiten documentBearbeiten)
        {
            return _domainDbContext.SaveDokument(documentBearbeiten);
        }

        public List<DocumentType> DocumentTypes { get { return _domainDbContext.DocumentTypesForCustomer; } }

        public DocumentType CreateDocumentType(DocumentType documentType, Action<string, string> addModelError)
        {
            return _domainDbContext.CreateDocumentType(documentType);
        }

        public DocumentType EditDocumentType(DocumentType documentType, Action<string, string> addModelError)
        {
            return _domainDbContext.UpdateDocumentType(documentType);
        }

        public int DeleteDocumentType(DocumentType documentType)
        {
           return _domainDbContext.DeleteDokumentType(documentType);
        }

        public List<DokumentRight> DocumentRights { get { return _domainDbContext.DokumentRights.ToList(); } }

        public List<Land> Laender { get; private set; }
        public List<VersandOption> VersandOptionen { get; private set; }
        public List<ZulassungsOption> ZulassungsOptionen { get; private set; }
        public List<ZulassungsDienstleistung> ZulassungsDienstleistungen { get; private set; }
        public List<FahrzeugStatus> FahrzeugStatusWerte { get; private set; }

        public string ToDataStoreKundenNr(string kundenNr)
        {
            throw new NotImplementedException();
        }

        public void Init(IAppSettings appSettings, ILogonContext logonContext)
        {
            _domainDbContext = new DomainDbContext(ConfigurationManager.AppSettings["Connectionstring"], logonContext.UserName);    
        }

        public string GetZulassungskreisFromPostcodeAndCity(string postCode, string city)
        {
            throw new NotImplementedException();
        }
    }
}
