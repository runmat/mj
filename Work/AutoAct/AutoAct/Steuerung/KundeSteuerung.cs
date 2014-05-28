using System;
using AutoAct.Bapi;
using AutoAct.Interfaces;
using AutoAct.Resources;
using GeneralTools.Contracts;

namespace AutoAct.Steuerung
{
    public class KundeSteuerung
    {
        private readonly ILogService _logService;
        private readonly IAutoActRest _autoActRest;
        private readonly IAutoActBapi _autoActBapi;
        private readonly IFahrzeugSteuerung _fahrzeugSteuerung;
        private readonly IConsoleWrapper _consoleWrapper;

        public KundeSteuerung(ILogService logService, IAutoActRest autoActRest, IAutoActBapi autoActBapi, IFahrzeugSteuerung fahrzeugSteuerung, IConsoleWrapper consoleWrapper )
        {
            _logService = logService;
            _autoActRest = autoActRest;
            _autoActBapi = autoActBapi;
            _fahrzeugSteuerung = fahrzeugSteuerung;
            _consoleWrapper = consoleWrapper;
        }

        public void Execute(Kunde kunde)
        {
            _consoleWrapper.WriteInfo(string.Format(ApplicationStrings.KundeSteuerung_Start_Nachricht, kunde.Nummer));
            _autoActRest.SetDiegstAuthenticator(kunde.Anmeldename, kunde.Passwort);

            if (!_autoActRest.IsAlive())
            {
                var message = string.Format(ApplicationStrings.Koennte_keine_Verbindung_zur_AutoAct_hergesetllt_werden, kunde.Nummer);
                _logService.LogError(new ApplicationException(message), null, null); // Muss Optional Param behalten da sonst das Mocking nicht funktioniert
                _consoleWrapper.WriteError(message);
                return;
            }

            var fahrzeuge = _autoActBapi.GetVehiclesToExportPerKunde(kunde.Nummer);

            _fahrzeugSteuerung.Execute(kunde, fahrzeuge);
        }
    }
}
