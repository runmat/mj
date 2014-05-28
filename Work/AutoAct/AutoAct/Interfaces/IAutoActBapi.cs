﻿using System.Collections.Generic;
using AutoAct.Bapi;
using AutoAct.Enums;
using SapORM.Models;

namespace AutoAct.Interfaces
{
    /// <summary>
    /// Kommunikation mit SAP via BAPI Implementierungen
    /// </summary>
    public interface IAutoActBapi
    {
        IEnumerable<Kunde> GetAutoActKunden();
        List<Z_DPM_READ_AUTOACT_01.GT_OUT> GetVehiclesToExportPerKunde(string kundennummer);
        IEnumerable<Z_DPM_READ_AUTOACT_01.GT_OUT> GetExportedVehiclesPerKunde(string kundennummer);
        void ReportVehicleExportSuccess(string belegNr, long autoactId);
        void ReportVehilceExportFailure(string belegNr, string errorMessage);
        void ReportVehilceAttachmentOrImageExportFailure(string belegNr, string error);
        void ResetVehicle(string belegNr);
    }
}
