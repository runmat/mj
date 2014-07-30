// ReSharper disable RedundantUsingDirective
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using CkgDomainLogic.DomainCommon.Models;
using CkgDomainLogic.General.Models;
using CkgDomainLogic.General.ViewModels;
using System.Web.Mvc;
using CkgDomainLogic.Uebfuehrg.Contracts;
using CkgDomainLogic.Uebfuehrg.Models;
using GeneralTools.Models;
using System.IO;
using GeneralTools.Services;
using Adresse = CkgDomainLogic.Uebfuehrg.Models.Adresse;
using Fahrzeug = CkgDomainLogic.Uebfuehrg.Models.Fahrzeug;
// ReSharper restore RedundantUsingDirective

namespace CkgDomainLogic.Uebfuehrg.ViewModels
{
    public class UebfuehrgViewModel : CkgBaseViewModel
    {
        [XmlIgnore]
        public IUebfuehrgDataService DataService { get { return CacheGet<IUebfuehrgDataService>(); } }

        public List<CommonUiModel> StepModels { get; private set; }

        [XmlIgnore]
        public string Title { get { return "Überführung"; } }

        [XmlIgnore]
        public string TitleSmall { get { return "für 1 Fahrzeug"; } }


        public string[] StepFriendlyNames { get { return PropertyCacheGet(() => StepModels.Select(s => s.HeaderShort).ToArray()); } }

        public int StepCurrentIndex { get; set; }

        public CommonUiModel StepCurrentModel { get { return StepModels[StepCurrentIndex]; } }

        public string StepCurrentFormPartialViewName
        {
            get { return GetFormPartialViewName(StepCurrentModel.ViewName); }
        }

        public string GetFormPartialViewName(string stepKey)
        {
            return string.Format("Forms/{0}", stepKey);
        }

        public string FirstStepErrorHint { get { return null; } }


        public void DataInit()
        {
            DataMarkForRefresh();

            InitStepModels();
        }

        void InitStepModels()
        {
            CommonUiModel uiModel;
            var index = 0; 
            
            var list = new List<CommonUiModel>();

            uiModel = new RgDaten
            {
                UiIndex = index,
                GroupName = "RGDATEN",
                SubGroupName = "-",
                Header = "Rechnungsdaten",
                IsMandatory = true,

                ViewName = "RgDaten",

                // TEST
                ReKundenNr = "4711",
                RgKundenNr = "9876"
            };
            list.Add(uiModel);
            index++;

            uiModel = new Fahrzeug
            {
                FahrzeugIndex = "1",

                UiIndex = index,

                GroupName = "FAHRZEUGE",
                SubGroupName = "FAHRZEUG_1",
                Header = "Fahrzeug 1 (Hinfahrt)",
                HeaderShort = "Fahrzeug 1",
                IsMandatory = true,

                ViewName = "Fahrzeug",

                // TEST
                FIN = "#4711987654",
                Fahrzeugklasse = "PKW"
            };
            list.Add(uiModel);
            index++;

            uiModel = new Adresse
            {
                TransportTypAvailable = false,

                UhrzeitwunschAvailable = true,
                AdressTyp = AdressenTyp.FahrtAdresse,
                SubTyp = "ABHOLADRESSE",
                UiIndex = index,
                GroupName = "FAHRZEUG_1",
                SubGroupName = "START",
                Header = "Abholadresse",
                IsMandatory = true,

                ViewName = "Adresse",

                // TEST
                Name1 = "Walter Zabel",
                Strasse = "Teststraße 3",
                HausNr = "3",
                PLZ = "22926",
                Ort = "Ahrensburg",
            };
            list.Add(uiModel);
            index++;

            uiModel = new CommonUiModel
            {
                UiIndex = index,
                GroupName = "SUMMARY",
                SubGroupName = "SUMMARY",
                Header = "Übersicht",
                IsMandatory = false,

                ViewName = "Summary",
            };
            list.Add(uiModel);
            index++;

            uiModel = new CommonUiModel
            {
                UiIndex = index,
                GroupName = "FINISH",
                SubGroupName = "FINISH",
                Header = "Fertig!",
                IsMandatory = false,

                ViewName = "Finish",
            };
            list.Add(uiModel);


            StepModels = list;
        }

        public void DataMarkForRefresh()
        {
            PropertyCacheClear(this, m => m.StepFriendlyNames);

            StepCurrentIndex = 0;
        }

        public void MoveToNextStep()
        {
            StepCurrentIndex++;
            if (StepCurrentIndex >= StepModels.Count)
                StepCurrentIndex = StepModels.Count - 1;
        }

        public void Validate(Action<string, string> addModelError)
        {
        }

        public CommonUiModel GetStepModel(int uiIndex=-1)
        {
            if (uiIndex == -1)
                uiIndex = StepCurrentIndex;

            return StepModels[uiIndex];
        }

        public void SetStepModel(CommonUiModel stepForm, int uiIndex=-1)
        {
            if (uiIndex == -1)
                uiIndex = StepCurrentIndex;
            
            StepModels[uiIndex] = stepForm;
        }

        /// <summary>
        /// Copies only DERIVED properties from classes deriving from UiModel to object StepForms[i]
        /// (preserves the base properties of class UiModel in the destination object (-> StepForms[i]))
        /// </summary>
        public T SaveSubModelWithPreservingUiModel<T>(T subModel, int uiIndex=-1) where T : CommonUiModel, new()
        {
            if (uiIndex == -1)
                uiIndex = StepCurrentIndex;

            var savedUiModel = ModelMapping.Copy(GetStepModel(uiIndex), new CommonUiModel());

            SetStepModel(subModel, uiIndex);
            ModelMapping.Copy(savedUiModel, GetStepModel(uiIndex));

            return (T)GetStepModel(uiIndex);
        }
    }
}
