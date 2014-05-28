using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AutoAct.Entities;
using AutoAct.Enums;
using AutoAct.Interfaces;
using AutoAct.Resources;
using GeneralTools.Contracts;

namespace AutoAct.Steuerung
{
    public class DokumentSteuerung : IDokumentSteuerung
    {
        private readonly ILogService _logService;
        private readonly IAutoActRest _autoActRest;
        private readonly IFileHelper _fileHelper;
        private readonly IConsoleWrapper _consoleWrapper;

        public DokumentSteuerung(ILogService logService, IAutoActRest autoActRest, IFileHelper fileHelper, IConsoleWrapper consoleWrapper)
        {
            _logService = logService;
            _autoActRest = autoActRest;
            _fileHelper = fileHelper;
            _consoleWrapper = consoleWrapper;
        }

        public void LoadDokumentForVehicle(Vehicle vehicle, string kundennummer, IEnumerable<Attachment> attachments, StringBuilder sb)
        {
            foreach (var attachment in attachments)
            {
                if (string.IsNullOrEmpty(attachment.FileName))
                {
                    continue;
                }

                // Keine weitere Prüfung ob die Datei existiert oder nicht da dies bereits geschehen ist
                var path = _fileHelper.DeterminePathToFile(kundennummer, vehicle.Vin, attachment.FileName);

                var postAttachmentResult = _autoActRest.PostAttachment(vehicle.Id.ToString(), attachment.AttachmentType, attachment.FileName, path);
                if (postAttachmentResult.Errors.errors.Any())
                {
                    sb.Append(string.Concat(@"/", string.Format(ApplicationStrings.ReportVehilceAttachmentExportFailure, attachment.FileName, postAttachmentResult.ErrorSummary)));
                }                  
            }
        }

        public void CheckDokumentsForVehicle(string kundennummer, string fin, IEnumerable<Attachment> attachments, StringBuilder sb)
        {
            foreach (var attachment in attachments)
            {
                if (string.IsNullOrEmpty(attachment.FileName))
                {
                    continue;
                }

                var pathToFile = _fileHelper.DeterminePathToFile(kundennummer, fin, attachment.FileName);                
                if (_fileHelper.Exists(pathToFile))
                {
                    continue;
                }

                _consoleWrapper.WriteInfo(string.Format(ApplicationStrings.Dokument_fehlt, pathToFile, attachment.AttachmentType.ToString()));
                sb.Append(string.Concat(@"/", string.Format(ApplicationStrings.Dokument_fehlt, pathToFile, attachment.AttachmentType.ToString())));
            }
        }
    }
}
