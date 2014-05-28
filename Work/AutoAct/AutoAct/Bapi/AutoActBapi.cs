using System;
using System.Collections.Generic;
using System.Linq;
using AutoAct.Enums;
using AutoAct.Interfaces;
using AutoAct.Resources;
using SapORM.Contracts;
using SapORM.Models;

namespace AutoAct.Bapi
{
    public class AutoActBapi : IAutoActBapi
    {
        private readonly ISapDataService _sapDataService;

        public AutoActBapi(ISapDataService sapDataService)
        {
            _sapDataService = sapDataService;
        }

        public IEnumerable<Kunde> GetAutoActKunden()
        {
            var autoActKunden = Z_DPM_READ_ZDAD_AUFTR_006.GT_WEB.GetExportListWithInitExecute(_sapDataService, "I_KENNUNG", "AUTOACT_AG").Select(quelle => new Kunde(quelle)).ToList();
            return autoActKunden;
        }

        public List<Z_DPM_READ_AUTOACT_01.GT_OUT> GetVehiclesToExportPerKunde(string kundennummer)
        {
            return Z_DPM_READ_AUTOACT_01.GT_OUT.GetExportListWithInitExecute(_sapDataService, "I_KUNNR_AG, I_STATUS", kundennummer, "2");
        }

        public IEnumerable<Z_DPM_READ_AUTOACT_01.GT_OUT> GetExportedVehiclesPerKunde(string kundennummer)
        {
            var exportedWithSuccess = Z_DPM_READ_AUTOACT_01.GT_OUT.GetExportListWithInitExecute(_sapDataService, "I_KUNNR_AG, I_STATUS", kundennummer, "3");
            var exportedWithError = Z_DPM_READ_AUTOACT_01.GT_OUT.GetExportListWithInitExecute(_sapDataService, "I_KUNNR_AG, I_STATUS", kundennummer, "4");
            var exportedWithAttachmentError = Z_DPM_READ_AUTOACT_01.GT_OUT.GetExportListWithInitExecute(_sapDataService, "I_KUNNR_AG, I_STATUS", kundennummer, "5");
            return exportedWithSuccess.Union(exportedWithError).Union(exportedWithAttachmentError).ToList();
        }

        public void ReportVehicleExportSuccess(string belegNr, long autoactId)
        {
            _sapDataService.InitExecute(ApplicationStrings.Bapi_ReportVehicleExportSuccess, ApplicationStrings.Bapi_ReportVehicleExportSuccess_Params, belegNr, "3", "", autoactId.ToString());
        }

        public void ReportVehilceExportFailure(string belegNr, string errorMessage)
        {
            _sapDataService.InitExecute(ApplicationStrings.Bapi_ReportVehicleExportStatus, ApplicationStrings.Bapi_ReportVehicleExportStatus_Params, belegNr, "4", errorMessage);
        }

        public void ReportVehilceAttachmentOrImageExportFailure(string belegNr, string errorSummary)
        {
            _sapDataService.InitExecute(ApplicationStrings.Bapi_ReportVehicleExportStatus, ApplicationStrings.Bapi_ReportVehicleExportStatus_Params, belegNr, "5", string.Format(ApplicationStrings.ReportVehilceAttachmentExportFailure, errorSummary));
        }

        public void ResetVehicle(string belegNr)
        {
            _sapDataService.InitExecute(ApplicationStrings.Bapi_ReportVehicleExportStatus, ApplicationStrings.Bapi_ReportVehicleExportStatus_Params, belegNr, "2", "");
        }
    }
}
