using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using CkgDomainLogic.General.Services;
using CkgDomainLogic.General.ViewModels;
using CkgDomainLogic.Finance.Contracts;
using CkgDomainLogic.Finance.Models;
using GeneralTools.Models;

namespace CkgDomainLogic.Finance.ViewModels
{
    public class FinanceTelefonieReportViewModel : CkgBaseViewModel
    {
        [XmlIgnore]
        public IFinanceTelefonieReportDataService DataService { get { return CacheGet<IFinanceTelefonieReportDataService>(); } }

        [XmlIgnore]
        public List<TelefoniedatenItem> Telefoniedaten { get { return DataService.Telefoniedaten; } }

        public void LoadTelefoniedaten(TelefoniedatenSuchparameter suchparameter)
        {
            DataService.Suchparameter.Vertragsart = suchparameter.Vertragsart;
            DataService.Suchparameter.DatumRange = suchparameter.DatumRange;
            DataService.Suchparameter.Anrufart = suchparameter.Anrufart;
            DataService.MarkForRefreshTelefoniedaten();
            PropertyCacheClear(this, m => m.TelefoniedatenFiltered);
        }

        public void FillVertragsarten()
        {
            var vertragsArten = new List<string>();

            var myLogonContext = (LogonContext as LogonContextDataServiceDadServices);
            if (myLogonContext != null)
            {
                if ((myLogonContext.Organization != null) && (!String.IsNullOrEmpty(myLogonContext.Organization.OrganizationName)))
                {
                    var vArten = myLogonContext.Organization.OrganizationName.Split('+');
                    Array.Sort(vArten);
                    foreach (var vArt in vArten)
                    {
                        vertragsArten.Add(vArt.Trim());
                    }
                }
            }

            DataService.Suchparameter.AuswahlVertragsart = vertragsArten;
            if ((String.IsNullOrEmpty(DataService.Suchparameter.Vertragsart)) && (vertragsArten.Count > 0))
            {
                DataService.Suchparameter.Vertragsart = vertragsArten[0];
            }
        }

        #region Filter

        [XmlIgnore]
        public List<TelefoniedatenItem> TelefoniedatenFiltered
        {
            get { return PropertyCacheGet(() => Telefoniedaten); }
            private set { PropertyCacheSet(value); }
        }

        public void FilterTelefoniedaten(string filterValue, string filterProperties)
        {
            TelefoniedatenFiltered = Telefoniedaten.SearchPropertiesWithOrCondition(filterValue, filterProperties);
        }

        #endregion
    }
}
