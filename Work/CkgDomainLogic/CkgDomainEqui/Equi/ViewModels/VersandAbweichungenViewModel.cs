// ReSharper disable RedundantUsingDirective

using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Xml.Serialization;
using CkgDomainLogic.Equi.Contracts;
using CkgDomainLogic.General.Services;
using CkgDomainLogic.General.ViewModels;
using CkgDomainLogic.Equi.Models;
using System.Linq;
using GeneralTools.Models;
// ReSharper restore RedundantUsingDirective

namespace CkgDomainLogic.Equi.ViewModels
{
    public class VersandAbweichungenViewModel : CkgBaseViewModel
    {
        [XmlIgnore]
        public IAbweichungenDataService DataService { get { return CacheGet<IAbweichungenDataService>(); } }

        public VersandAbweichungSelektor VersandAbweichungSelektor
        {
            get { return PropertyCacheGet(() => new VersandAbweichungSelektor()); }
            set { PropertyCacheSet(value); }
        }

        [XmlIgnore]
        public List<Fahrzeugbrief> VersandAbweichungen
        {
            get { return PropertyCacheGet(() => new List<Fahrzeugbrief>()); }
            private set { PropertyCacheSet(value); }
        }

        [XmlIgnore]
        public List<Fahrzeugbrief> VersandAbweichungenFiltered
        {
            get { return PropertyCacheGet(() => VersandAbweichungen); }
            private set { PropertyCacheSet(value); }
        }

        public void DataMarkForRefresh()
        {
            PropertyCacheClear(this, m => m.VersandAbweichungenFiltered);
            PropertyCacheClear(this, m => m.VersandAbweichungSelektor);
        }

        public void LoadVersandAbweichungen(Action<string, string> addModelError)
        {
            VersandAbweichungen = DataService.VersandAbweichungen;

            if (VersandAbweichungen.None())
                addModelError("", Localize.NoDataFound);

            DataMarkForRefresh();
        }

        public void SaveMemo(Fahrzeugbrief model, Action<string, string> addModelError)
        {
            var error = DataService.SaveVersandAbweichungMemo(model.Equipmentnummer, model.Memo, model.Versanddatum);
            if (error.IsNotNullOrEmpty())
            {
                addModelError("", error);
                return;
            }

            var savedModel = VersandAbweichungenFiltered.FirstOrDefault(v => v.Equipmentnummer == model.Equipmentnummer);
            if (savedModel == null)
            {
                addModelError("", Localize.NoDataFoundForSaving);
                return;
            }

            savedModel.Memo = model.Memo;
        }

        public void SaveAsErledigt(string id, Action<string, string> addModelError)
        {
            var savedModel = VersandAbweichungenFiltered.FirstOrDefault(v => v.Equipmentnummer == id);
            if (savedModel == null)
            {
                addModelError("", Localize.NoDataFoundForSaving);
                return;
            }

            var error = DataService.SaveVersandAbweichungAsErledigt(savedModel.Equipmentnummer, savedModel.Memo, savedModel.Versanddatum);
            if (error.IsNotNullOrEmpty())
            {
                addModelError("", error);
                return;
            }

            LoadVersandAbweichungen(addModelError);
            DataMarkForRefresh();
        }

        public void FilterVersandAbweichungen(string filterValue, string filterProperties)
        {
            VersandAbweichungenFiltered = VersandAbweichungen.SearchPropertiesWithOrCondition(filterValue, filterProperties);
        }
    }
}
