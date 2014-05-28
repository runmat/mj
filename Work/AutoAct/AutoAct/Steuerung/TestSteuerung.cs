using System;
using AutoAct.Bapi;
using AutoAct.Interfaces;
using GeneralTools.Contracts;

namespace AutoAct.Steuerung
{
#if DEBUG

    public class TestSteuerung
    {
        private readonly ILogService _logService;
        private readonly IAutoActBapi _autoActBapi;
        private readonly IAutoActRest _autoActRest;

        public TestSteuerung(ILogService logService, IAutoActBapi autoActBapi, IAutoActRest autoActRest)
        {
            _logService = logService;
            _autoActBapi = autoActBapi;
            _autoActRest = autoActRest;
        }

        /// <summary>
        /// Test für einen Kunden vorbereiten
        /// Sätze bei AutoAct in vollem Umfang löschen
        /// Sätze in SAP zurückstellen auf "nicht übertragen" und Übertragungshinweis löschen
        /// </summary>
        public void Cleanup(Kunde kunde)
        {
            _autoActRest.SetDiegstAuthenticator(kunde.Anmeldename, kunde.Passwort);

            var autoactFahrzeuge = _autoActRest.GetVehicles();

            foreach (var vehicle in autoactFahrzeuge.Value.Vehicles)
            {
                var fahrzeugDeleted = _autoActRest.DeleteVehicle(vehicle.Id);
                if (fahrzeugDeleted.Value)
                {
                    _autoActRest.DeletePictures(vehicle.Id);                    
                }
                else
                {
                    // Cleanup Operation verläuft nur im Test Betrieb
                    // Sollte etwas schief gehen sofort abbrechen
                    throw new ApplicationException(fahrzeugDeleted.ErrorSummary);
                }
            }

            // Achtung: Dokumente werden mitgelöscht, Bilder nicht, müssen einzeln aufgerufen werden

            // Alle Fahrzeuge in SAP: Reset so dass Status = 1, Übertragungstext leeren, Sätze können nun erneut wiederimportiert werden
            var exportierteFahrzeuge = _autoActBapi.GetExportedVehiclesPerKunde(kunde.Nummer);

            foreach (var gtOut in exportierteFahrzeuge)
            {
                _autoActBapi.ResetVehicle(gtOut.BELEGNR);
            }
        }
    }
#endif
}
