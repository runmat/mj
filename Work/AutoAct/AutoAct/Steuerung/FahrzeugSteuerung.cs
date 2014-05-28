using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AutoAct.Bapi;
using AutoAct.Entities;
using AutoAct.Enums;
using AutoAct.Interfaces;
using AutoAct.Resources;
using GeneralTools.Contracts;
using SapORM.Models;

namespace AutoAct.Steuerung
{
    public class FahrzeugSteuerung : IFahrzeugSteuerung
    {
        private readonly ILogService _logService;
        private readonly IAutoActRest _autoActRest;
        private readonly IAutoActBapi _autoActBapi;
        private readonly IDokumentSteuerung _dokumentSteuerung;
        private readonly IFileHelper _fileHelper;
        private readonly IHerstellerSteuerung _herstellerSteuerung;

        public FahrzeugSteuerung(ILogService logService, IAutoActRest autoActRest, IAutoActBapi autoActBapi, IDokumentSteuerung dokumentSteuerung, IFileHelper fileHelper, IHerstellerSteuerung herstellerSteuerung)
        {
            _logService = logService;
            _autoActRest = autoActRest;
            _autoActBapi = autoActBapi;
            _dokumentSteuerung = dokumentSteuerung;
            _fileHelper = fileHelper;
            _herstellerSteuerung = herstellerSteuerung;
        }

        public void Execute(Kunde kunde, IEnumerable<Z_DPM_READ_AUTOACT_01.GT_OUT> fahrzeuge)
        {
            foreach (var fahrzeug in fahrzeuge)
            {
                // STartdatum für die Auktion muss >= heute sein
                if (fahrzeug.STARTDATUM.Value.Date < DateTime.Now.Date)
                {
                    _autoActBapi.ReportVehilceExportFailure(fahrzeug.BELEGNR, ApplicationStrings.Startdatum_darf_nicht_in_der_Vergangheit_liegen);
                    continue;
                }

                // Aus SAP erhalte ich einen Z_DPM_READ_AUTOACT_01.GT_OUT Objekt Daten zum
                // - Fahrzeug
                // - Auction
                // - Attachments
                // - Bilder
                // AutoAct kann nur mit einzelnen Objekten umgehen und diese werden deswegen auseinandergenommen

                Auction auction = new Auction(fahrzeug);

                Vehicle vehicle = new Vehicle(fahrzeug);
                vehicle.Auction = auction;

                // Prüfungen
                StringBuilder sb = new StringBuilder();

                var attachments = new List<Attachment>
                    {
                        new Attachment
                            {
                                AttachmentType = AttachmentType.STATUS_REPORT,
                                FileName = fahrzeug.ZUSTANDSBERICHT
                            },
                        new Attachment
                            {
                                AttachmentType = AttachmentType.MAINTENANCE_MANUAL,                                
                                FileName = fahrzeug.UNTERLAGEN1
                            },
                        new Attachment
                            {
                                AttachmentType = AttachmentType.DAMAGE_REPORT,
                                FileName = fahrzeug.UNTERLAGEN2
                            },
                        new Attachment
                            {
                                AttachmentType = AttachmentType.CUSTOM_DOCUMENT,
                                FileName = fahrzeug.UNTERLAGEN3
                            }
                    };

                _dokumentSteuerung.CheckDokumentsForVehicle(kunde.Nummer, vehicle.Vin, attachments, sb);
                _herstellerSteuerung.GetHerstellerAndModel(fahrzeug.ZZFABRIKNAME, vehicle, sb);

                // Wenn der StringBuilder einen Inhalt aufweist dann hat es Fehler gegeben, nächstes Fahrzeug
                if (sb.Length > 0)
                {
                    _autoActBapi.ReportVehilceExportFailure(vehicle.Belegnummer, sb.ToString());
                    continue;
                }

                // Datenübertragung zur Zielanwendung
                var postVehicleResult = _autoActRest.PostVehicle(vehicle);  

                if (postVehicleResult.Errors.errors.Any())
                {
                    _autoActBapi.ReportVehilceExportFailure(vehicle.Belegnummer, postVehicleResult.ErrorSummary);
                    continue;
                }

                vehicle.Id = postVehicleResult.Value.Id;
                
                _autoActBapi.ReportVehicleExportSuccess(vehicle.Belegnummer, vehicle.Id);

                _dokumentSteuerung.LoadDokumentForVehicle(vehicle, kunde.Nummer, attachments, sb);

                // Uploaden der Bilder
                var postImageResult = _autoActRest.PostPictures(vehicle.Id.ToString(), _fileHelper.GetImageNamesForFahrzeug(kunde.Nummer, vehicle.Vin));
                if (postImageResult.Errors.errors.Any())
                {
                    sb.Append(string.Concat(@"/", postImageResult.ErrorSummary));
                }

                if (sb.Length > 0)
                {
                    _autoActBapi.ReportVehilceAttachmentOrImageExportFailure(fahrzeug.BELEGNR, sb.ToString());
                }
            }
        }
    }
}
