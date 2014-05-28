using System;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Threading;
using AutoAct.Bapi;
using AutoAct.Interfaces;
using AutoAct.Resources;
using AutoAct.Rest;
using AutoAct.Steuerung;
using AutoAct.Utils;
using GeneralTools.Contracts;
using GeneralTools.Models;
using GeneralTools.Services;
using RestSharp;
using SapORM.Contracts;
using SapORM.Services;
using FileHelper = AutoAct.Utils.FileHelper;

namespace AutoAct
{
    class Program
    {

        private static ISapDataService _sap;

#if DEBUG
        static ISapDataService Sap { get { return (_sap ?? (_sap = new SapDataServiceTestSystemNoCacheFactory().Create())); } }
#endif

#if RELEASE
        static ISapDataService Sap { get { return (_sap ?? (_sap = new SapDataServiceLiveSystemNoCacheFactory().Create())); } }
#endif

        /// <summary>
        /// Einstiegspunkt
        /// Hier findet die KundeSteuerung per Kunden statt
        /// </summary>
        /// <param name="arg"></param>
        static int Main(string[] arg)
        {
            Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture; 
            Thread.CurrentThread.CurrentUICulture = CultureInfo.InvariantCulture;

            string appName = ConfigurationManager.AppSettings[ApplicationStrings.AppNameAppSetting];
            string url = ConfigurationManager.AppSettings[ApplicationStrings.AutoActUrlAppSetting];

            // Logging aufbereiten, einen Logfile pro Tag, Settings
            var logfilename = string.Concat(appName, "-", DateTime.Now.ToString(ApplicationStrings.DateTimeFormatStringIso8601, CultureInfo.InvariantCulture), ".log");
            ILogService logService = new LogService(appName, logfilename);

            logService.Log(new LogItem{ Message = ApplicationStrings.Program_Main_Begin_Import},null,null,null );

            ISettingsReader settingsReader = new SettingsReader();

            // Verbindungsdaten aufbereiten
            ConfigurationMerger.MergeTestWebConfigAppSettings();

            // Bapi vorbereiten
            IAutoActBapi autoActBapi = new AutoActBapi(Sap);

            // Kunden für die AutoAct Übertragung sammeln
            var kunden = autoActBapi.GetAutoActKunden();
            
            // Pro ermittelten Kunden die Anwendung neusteuern
            foreach (var kunde in kunden)
            {
                // Authentifizierung aus app.config holen falls Eintrag vorhanden ist
                // Vereinfacht das Testen da wir im Testbetrieb nur die Zugangsdaten des DADs nutzen
                if (ConfigurationManager.AppSettings.AllKeys.Any(x => x == ApplicationStrings.ConfigAuthenticationLogon))
                {
                    kunde.Anmeldename = settingsReader.Logon;
                    kunde.Passwort = settingsReader.Password;
                }

                IFileHelper fileHelper = new FileHelper(logService, settingsReader);
                IConsoleWrapper consoleWrapper = new ConsoleWrapper();
                IAutoActRest autoActRest = new AutoActRest(logService, url, new RestClient(), fileHelper);
                IDokumentSteuerung dokumentSteuerung = new DokumentSteuerung(logService, autoActRest, fileHelper, consoleWrapper);
                IHerstellerSteuerung herstellerSteuerung = new HerstellerSteuerung(autoActRest, consoleWrapper);
                IFahrzeugSteuerung fahrzeugSteuerung = new FahrzeugSteuerung(logService, autoActRest, autoActBapi, dokumentSteuerung, fileHelper, herstellerSteuerung);


                // Code Abschnitt darf nicht in Release ausgeführt werden und wird daher in die Konfiguration gar nicht erst kompliiert
                #if DEBUG
                if (arg.Any() && arg[0] == "reset")
                {
                    TestSteuerung testSteuerung = new TestSteuerung(logService,autoActBapi,autoActRest);
                    try
                    {
                        testSteuerung.Cleanup(kunde);
                    }
                    catch (Exception ex)
                    {
                        Console.Error.Write(ex.Message);
                        throw;
                    }
                    return 0;
                }
                #endif

                KundeSteuerung kundeSteuerung = new KundeSteuerung(logService, autoActRest, autoActBapi, fahrzeugSteuerung, consoleWrapper);
                try
                {
                    kundeSteuerung.Execute(kunde);
                }
                catch (Exception e)
                {
                    logService.LogError(e, null, null);
                    Console.Error.WriteLine(e.Message);
                    Console.Error.WriteLine(e.StackTrace);
                    return 1;
                }
            }

            return 0;
        }
    }
}
