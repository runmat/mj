using System;
using System.Collections.Generic;
using Mobile.Bapi;
using Mobile.Interfaces;
using SapORM.Contracts;
using SapORM.Models;
using SapORM.Services;

namespace Mobile.Utils
{
    public class MobileBapi : IMobileBapi
    {
        private readonly ISettingsReader _settingsReader;
        private ISapDataService _sapService;


        public MobileBapi(ISettingsReader settingsReader)
        {
            _settingsReader = settingsReader;
            _sapService = _settingsReader.SapProdSystem ? new SapDataServiceLiveSystemNoCacheFactory().Create() : new SapDataServiceTestSystemNoCacheFactory().Create();
        }

        public IEnumerable<Kunde> GetMobileKunden()
        {
            throw new NotImplementedException();
        }

        public List<Z_DPM_READ_AUTOACT_01.GT_OUT> GetVehiclesToExportPerKunde(string kundennummer)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Z_DPM_READ_AUTOACT_01.GT_OUT> GetExportedVehiclesPerKunde(string kundennummer)
        {
            throw new NotImplementedException();
        }

        public void ReportVehicleExportSuccess(string belegNr, long autoactId)
        {
            throw new NotImplementedException();
        }

        public void ReportVehilceExportFailure(string belegNr, string errorMessage)
        {
            throw new NotImplementedException();
        }

        public void ReportVehilceAttachmentOrImageExportFailure(string belegNr, string error)
        {
            throw new NotImplementedException();
        }

        public void ResetVehicle(string belegNr)
        {
            throw new NotImplementedException();
        }
    }
}
