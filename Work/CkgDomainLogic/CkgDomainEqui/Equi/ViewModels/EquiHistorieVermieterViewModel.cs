using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Xml.Serialization;
using CkgDomainLogic.Equi.Contracts;
using CkgDomainLogic.Equi.Models;
using CkgDomainLogic.General.Services;
using CkgDomainLogic.General.ViewModels;
using GeneralTools.Models;

namespace CkgDomainLogic.Equi.ViewModels
{
    public class EquiHistorieVermieterViewModel : CkgBaseViewModel 
    {
        [XmlIgnore]
        public IEquiHistorieVermieterDataService DataService { get { return CacheGet<IEquiHistorieVermieterDataService>(); } }

        [XmlIgnore]
        public List<EquiHistorieInfoVermieter> HistorieInfos { get { return DataService.HistorieInfos; } }

        public EquiHistorieVermieter EquipmentHistorie { get; set; }

        public List<SelectItem> DocTypes
        {
            get
            {
                return PropertyCacheGet(() => DataService.FahrzeugAnforderungenLoadDocTypes()
                                                .ToListOrEmptyList()
                                                    .CopyAndInsertAtTop(new SelectItem("", Localize.DropdownDefaultOptionPleaseChoose)));
            }
        }

        public bool FahrzeugAnforderungenAnzeigen
        {
            get { return GetApplicationConfigValueForCustomer("FzgHistorieAnforderungenAnzeigen").ToBool(); }
        }

        public void LoadHistorieInfos(ref EquiHistorieSuchparameter suchparameter, ModelStateDictionary state)
        {
            DataService.Suchparameter = suchparameter;
            DataService.MarkForRefreshHistorieInfos();
            PropertyCacheClear(this, m => m.HistorieInfosFiltered);

            suchparameter.AnzahlTreffer = HistorieInfos.Count;

            if (HistorieInfos.None())
                state.AddModelError("", Localize.NoDataFound);
        }

        public void LoadHistorie(string equiNr, string meldungsNr)
        {
            if (!String.IsNullOrEmpty(equiNr))
            {
                EquipmentHistorie = DataService.GetEquiHistorie(equiNr, meldungsNr);
            }
            else if (HistorieInfos.Count == 1)
            {
                var item = HistorieInfos[0];
                EquipmentHistorie = DataService.GetEquiHistorie(item.EquipmentNr, item.MeldungsNr);
            }

            LoadFahrzeugAnforderungen();
        }

        public void LoadFahrzeugAnforderungen()
        {
            if (EquipmentHistorie == null || EquipmentHistorie.HistorieInfo == null)
                return;

            EquipmentHistorie.FahrzeugAnforderungen = DataService.FahrzeugAnforderungenLoad(EquipmentHistorie.HistorieInfo.FahrgestellNr).ToListOrEmptyList();
            EquipmentHistorie.FahrzeugAnforderungenAnzeigen = FahrzeugAnforderungenAnzeigen;
        }

        public FahrzeugAnforderung FahrzeugAnforderungNew()
        {
            return new FahrzeugAnforderung
                {
                    Fahrgestellnummer = (EquipmentHistorie == null || EquipmentHistorie.HistorieInfo == null) ? "" : EquipmentHistorie.HistorieInfo.FahrgestellNr,
                    AnlageDatum = DateTime.Today,
                    AnlageUser = LogonContext.UserName,
                    EmailAnlageUser = LogonContext.GetEmailAddressForUser(),
                    SendEmailToAnlageUser = true,
                };
        }

        public void FahrzeugAnforderungSave(FahrzeugAnforderung item, Action<string, string> addModelError)
        {
            if (!item.SendEmailToAnlageUser)
                item.EmailAnlageUser = "";

            DataService.FahrzeugAnforderungSave(item);

            LoadFahrzeugAnforderungen();
        }

        public byte[] GetHistorieAsPdf()
        {
            return DataService.GetHistorieAsPdf(EquipmentHistorie.HistorieInfo.EquipmentNr, EquipmentHistorie.HistorieInfo.MeldungsNr);
        }

        #region Filter

        [XmlIgnore]
        public List<EquiHistorieInfoVermieter> HistorieInfosFiltered
        {
            get { return PropertyCacheGet(() => HistorieInfos); }
            private set { PropertyCacheSet(value); }
        }

        public void FilterHistorieInfos(string filterValue, string filterProperties)
        {
            HistorieInfosFiltered = HistorieInfos.SearchPropertiesWithOrCondition(filterValue, filterProperties);
        }

        #endregion
    }
}
