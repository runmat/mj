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
        public IEquiHistorieDataService DataService { get { return CacheGet<IEquiHistorieDataService>(); } }

        public EquiHistorieSuchparameter Suchparameter
        {
            get { return PropertyCacheGet(() => new EquiHistorieSuchparameter()); }
            set { PropertyCacheSet(value); }
        }

        [XmlIgnore]
        public List<EquiHistorieVermieterInfo> HistorieInfos
        {
            get { return PropertyCacheGet(() => new List<EquiHistorieVermieterInfo>()); }
            private set { PropertyCacheSet(value); }
        }

        public EquiHistorieVermieter EquipmentHistorie { get; set; }

        public List<SelectItem> DocTypes
        {
            get
            {
                return PropertyCacheGet(() => DataService.GetFahrzeugAnforderungenDocTypes()
                                                .ToListOrEmptyList()
                                                    .CopyAndInsertAtTop(new SelectItem("", Localize.DropdownDefaultOptionPleaseChoose)));
            }
        }

        public bool FahrzeugAnforderungenAnzeigen
        {
            get { return GetApplicationConfigValueForCustomer("FzgHistorieAnforderungenAnzeigen", true).ToBool(); }
        }

        public int LoadHistorieInfos(ModelStateDictionary state)
        {
            HistorieInfos = DataService.GetHistorieVermieterInfos(Suchparameter);

            PropertyCacheClear(this, m => m.HistorieInfosFiltered);

            Suchparameter.AnzahlTreffer = HistorieInfos.Count;

            if (HistorieInfos.None())
                state.AddModelError("", Localize.NoDataFound);

            return Suchparameter.AnzahlTreffer;
        }

        public void LoadHistorie(string fin)
        {
            if (!string.IsNullOrEmpty(fin))
                EquipmentHistorie = DataService.GetHistorieVermieterDetail(fin);
            else if (HistorieInfos.Count == 1)
                EquipmentHistorie = DataService.GetHistorieVermieterDetail(HistorieInfos[0].FahrgestellNr);

            LoadFahrzeugAnforderungen();
        }

        public void LoadFahrzeugAnforderungen()
        {
            if (EquipmentHistorie == null || EquipmentHistorie.HistorieInfo == null)
                return;

            EquipmentHistorie.FahrzeugAnforderungen = DataService.GetFahrzeugAnforderungen(EquipmentHistorie.HistorieInfo.FahrgestellNr).ToListOrEmptyList();
            EquipmentHistorie.FahrzeugAnforderungenAnzeigen = FahrzeugAnforderungenAnzeigen;
        }

        public FahrzeugAnforderung FahrzeugAnforderungNew()
        {
            return new FahrzeugAnforderung
                {
                    Fahrgestellnummer = (EquipmentHistorie == null || EquipmentHistorie.HistorieInfo == null) ? "" : EquipmentHistorie.HistorieInfo.FahrgestellNr,
                    Kennzeichen = (EquipmentHistorie == null || EquipmentHistorie.HistorieInfo == null) ? "" : EquipmentHistorie.HistorieInfo.Kennzeichen,
                    AnlageDatum = DateTime.Today,
                    AnlageUser = LogonContext.UserName,
                    EmailAnlageUser = LogonContext.GetEmailAddressForUser(),
                    SendEmailToAnlageUser = true
                };
        }

        public void FahrzeugAnforderungSave(FahrzeugAnforderung item, Action<string, string> addModelError)
        {
            if (!item.SendEmailToAnlageUser)
                item.EmailAnlageUser = "";

            DataService.SaveFahrzeugAnforderung(item);

            LoadFahrzeugAnforderungen();
        }

        public byte[] GetHistorieAsPdf()
        {
            return DataService.GetHistorieVermieterAsPdf(EquipmentHistorie.HistorieInfo.FahrgestellNr);
        }

        #region Filter

        [XmlIgnore]
        public List<EquiHistorieVermieterInfo> HistorieInfosFiltered
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
