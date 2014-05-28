using System;
using System.Linq;
using System.Text;
using AutoAct.Entities;
using AutoAct.Interfaces;
using AutoAct.Resources;

namespace AutoAct.Steuerung
{
    public class HerstellerSteuerung : IHerstellerSteuerung
    {
        private readonly IAutoActRest _autoActRest;
        private readonly IConsoleWrapper _consoleWrapper;

        public HerstellerSteuerung(IAutoActRest autoActRest, IConsoleWrapper consoleWrapper)
        {
            _autoActRest = autoActRest;
            _consoleWrapper = consoleWrapper;
        }

        public void GetHerstellerAndModel(string herstellerAngaben, Vehicle vehicle, StringBuilder sb)
        {
            if (string.IsNullOrEmpty(herstellerAngaben))
            {
                sb.Append(string.Concat(@"/", ApplicationStrings.Angaben_zum_Hersteller_und_Model_fehlen));
            }
            else
            {
                var makeParts = herstellerAngaben.Split(" ".ToCharArray()[0]);

                _consoleWrapper.WriteInfo(string.Format(ApplicationStrings.HerstellerSteuerung_Log_Fahrzeugdaten, makeParts[0], makeParts.Count() == 2 ? makeParts[1] : string.Concat(makeParts[1], " ", makeParts[2])));

                var availableMakes = _autoActRest.GetMakes(vehicle.Type);
                var make = availableMakes.Value.Where(x => x.Make.ToUpper() == makeParts[0].ToUpper() && x.LanguageCode == "de").ToArray();

                vehicle.Variant = makeParts.Skip(1).Aggregate((current, next) => string.Concat(current, " ", next)); 

                if (make.Any())
                {
                    vehicle.Make = make.First().Make;
                    _consoleWrapper.WriteInfo(string.Format(ApplicationStrings.HerstellerSteuerung_Log_Ermittelte_Make, vehicle.Make));
                    var availableModels = _autoActRest.GetModels(vehicle.Type, vehicle.Make);
                    var model = availableModels.Value.Where(x => (x.Model.ToUpper() == makeParts[1].ToUpper() || x.Model.ToUpper() == string.Concat(makeParts[1], " ", makeParts[2]).ToUpper()) && x.LanguageCode == "de").ToList();

                    if (model.Any())
                    {
                        vehicle.Model = model.First().Model;
                        _consoleWrapper.WriteInfo(string.Format(ApplicationStrings.HerstellerSteuerung_Log_Ermittelte_Model, vehicle.Model));
                        return;
                    }

                    _consoleWrapper.WriteInfo(string.Format(ApplicationStrings.HerstellerSteuerung_Model_Nicht_Ermittelt));
                    
                    vehicle.Model = ApplicationStrings.AutoAct_Model_Unknown;
                }
                else
                {
                    sb.Append(string.Concat(@"/", string.Format(ApplicationStrings.Fahrzeug_Hersteller_Nicht_Anerkannt, herstellerAngaben)));
                }
            }
        }
    }
}
