using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Xml.Serialization;
using CkgDomainInternal.Verbandbuch.Contracts;
using CkgDomainInternal.Verbandbuch.Models;
using CkgDomainLogic.General.ViewModels;
using GeneralTools.Models;

namespace CkgDomainInternal.Verbandbuch.ViewModels
{
    public class VerbandbuchViewModel : CkgBaseViewModel
    {
        [XmlIgnore]
        public IVerbandbuchDataService DataService { get { return CacheGet<IVerbandbuchDataService>(); } }

        [XmlIgnore]
        public List<VerbandbuchModel> Verbandbuch
        {
            get { return PropertyCacheGet(() => new List<VerbandbuchModel>()); }
            private set { PropertyCacheSet(value); }
        }

        public void GetVerbandbuchEntries(string vkbur)
        {
            Verbandbuch =  DataService.GetVerbandbuchEntries(vkbur);
        }


        public void DataInit()
        {
            DataMarkForRefresh();
        }

        public void DataMarkForRefresh()
        {
            PropertyCacheClear(this, m => m.Verbandbuch);
        }

        public VerbandbuchModel GetVerbandBuchModel(string vkbur)
        {

            return new VerbandbuchModel
                                        {
                                            Vkbur = vkbur,
                                            PlaceOfAccident = vkbur
                                        };
        }


        #region Filter

        [XmlIgnore]
        public List<VerbandbuchModel> VerbandbuchFiltered
        {
            get { return PropertyCacheGet(() => Verbandbuch); }
            private set { PropertyCacheSet(value); }
        }

        public void FilterVerbandbuch(string filterValue, string filterProperties)
        {
            VerbandbuchFiltered = Verbandbuch.SearchPropertiesWithOrCondition(filterValue, filterProperties);
        }

        #endregion

        public void SaveVorfall(VerbandbuchModel vbModel, ModelStateDictionary state)
        {
            /*
            Set correct time value 
            */

            vbModel.TimeOfAccident = vbModel.DateOfAccident.Value.ToString("HHmmss");
            vbModel.TimeOfFirstAid= vbModel.DateOfFirstAid.Value.ToString("HHmmss");

           var result =  DataService.SaveVorfallSAP(vbModel);

            if (!String.IsNullOrEmpty(result))
            {
                state.AddModelError("", "Fehler beim Speichern: " + result);
            } 
        }
    }
}
