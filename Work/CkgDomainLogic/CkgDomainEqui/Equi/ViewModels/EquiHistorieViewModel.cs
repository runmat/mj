﻿using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Xml.Serialization;
using CkgDomainLogic.Equi.Contracts;
using CkgDomainLogic.Equi.Models;
using CkgDomainLogic.General.Services;
using CkgDomainLogic.General.ViewModels;
using GeneralTools.Models;
using GeneralTools.Services;

namespace CkgDomainLogic.Equi.ViewModels
{
    public class EquiHistorieViewModel : CkgBaseViewModel 
    {
        [XmlIgnore]
        public IEquiHistorieDataService DataService { get { return CacheGet<IEquiHistorieDataService>(); } }

        [XmlIgnore]
        public List<EquiHistorieInfo> HistorieInfos { get { return DataService.HistorieInfos; } }

        public EquiHistorie EquipmentHistorie { get; set; }

        public int CurrentAppID { get; set; }

        public string EquiBezeichnung
        {
            get
            {
                return string.Format("{0}: {1}<br/>{2}: {3}<br/>{4}: {5}<br/>{6}: {7}",
                    Localize.ChassisNo, EquipmentHistorie.Fahrgestellnummer,
                    Localize.LicenseNo, EquipmentHistorie.Kennzeichen,
                    Localize.Status, EquipmentHistorie.Status,
                    Localize.StorageLocation, EquipmentHistorie.Lagerort);
            }
        }

        private GeneralEntity HistorieHeader
        {
            get
            {
                return new GeneralEntity
                {
                    Title = Localize.Vehicle,
                    Body = EquiBezeichnung,
                    Tag = "SummaryMainItem"
                };
            }
        }

        public void LoadHistorieInfos(EquiHistorieSuchparameter suchparameter, ModelStateDictionary state)
        {
            DataService.Suchparameter = suchparameter;
            DataService.MarkForRefreshHistorieInfos();
            PropertyCacheClear(this, m => m.HistorieInfosFiltered);

            if (HistorieInfos.Count == 0)
            {
                state.AddModelError("", Localize.NoDataFound);
            }
        }

        public void LoadHistorie(string fin)
        {
            GetCurrentAppID();
            EquipmentHistorie = DataService.GetEquiHistorie(fin, CurrentAppID);
        }

        public GeneralSummary CreateSummaryModel()
        {
            var itemsList = new ListNotEmpty<GeneralEntity>
                (
                HistorieHeader,

                new GeneralEntity
                    {
                        Title = Localize.Overview,
                        Body = EquipmentHistorie.GetUebersichtSummaryString(),
                    }
                );

            if (EquipmentHistorie.ShowTypdaten)
            {
                itemsList.Add(
                    new GeneralEntity
                    {
                        Title = Localize.TypeData,
                        Body = EquipmentHistorie.Typdaten.GetSummaryString(),
                    }
                );
            }

            if (EquipmentHistorie.ShowMeldungen)
            {
                itemsList.Add(
                    new GeneralEntity
                    {
                        Title = Localize.Vita,
                        Body = EquipmentHistorie.GetMeldungenSummaryString(),
                    }
                );
            }

            if (EquipmentHistorie.ShowAktionen)
            {
                itemsList.Add(
                    new GeneralEntity
                    {
                        Title = Localize.Transmission,
                        Body = EquipmentHistorie.GetAktionenSummaryString(),
                    }
                );
            }

            if (EquipmentHistorie.ShowHaendlerdaten)
            {
                itemsList.Add(
                    new GeneralEntity
                    {
                        Title = Localize.DealerData,
                        Body = EquipmentHistorie.Haendlerdaten.GetSummaryString(),
                    }
                );
            }

            var summaryModel = new GeneralSummary
            {
                Header = Localize.Equi_Fahrzeughistorie,
                Items = itemsList
            };

            return summaryModel;
        }

        #region Filter

        [XmlIgnore]
        public List<EquiHistorieInfo> HistorieInfosFiltered
        {
            get { return PropertyCacheGet(() => HistorieInfos); }
            private set { PropertyCacheSet(value); }
        }

        public void FilterHistorieInfos(string filterValue, string filterProperties)
        {
            HistorieInfosFiltered = HistorieInfos.SearchPropertiesWithOrCondition(filterValue, filterProperties);
        }

        private void GetCurrentAppID()
        {
            var tmpObject = HttpContextService.TryGetValueFromSession("LastAppID");
            int tmpInt;

            if (tmpObject != null && Int32.TryParse(tmpObject.ToString(), out tmpInt))
            {
                CurrentAppID = tmpInt;
            }
        }

        #endregion
    }
}
