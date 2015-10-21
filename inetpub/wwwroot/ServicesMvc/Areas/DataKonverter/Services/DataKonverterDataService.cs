using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CkgDomainLogic.Autohaus.Contracts;
using CkgDomainLogic.DataKonverter.Contracts;
using CkgDomainLogic.General.Services;
using SapORM.Contracts;

namespace CkgDomainLogic.DataKonverter.Services
{
    public class DataKonverterDataService : CkgGeneralDataServiceSAP, IDataKonverterDataService
    {
        public DataKonverterDataService(ISapDataService sap)
            : base(sap)
        {
        }
    }
}