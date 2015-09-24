using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using CkgDomainLogic.General.Contracts;
using CkgDomainLogic.General.Database.Models;
using CkgDomainLogic.General.Database.Services;
using GeneralTools.Contracts;
using GeneralTools.Models;
using GeneralTools.Services;

namespace CkgDomainLogic.DomainCommon.Services
{
    public class TranslationFormatService : ITranslationFormatService
    {
        private static DateTime _timeOfLastResourceRefresh;
        private static List<TranslatedResource> _resources;
        private static List<TranslatedResourceCustom> _resourcesForCustomers;
        private readonly List<TranslatedResourceCustom> _resourcesForCurrentCustomer; 

        static TranslationFormatService()
        {
            InitResources();
        }

        public TranslationFormatService(ISessionDataHelper sessionDataHelper)
        {
            var logonContext = sessionDataHelper.GetLogoncontext();
            var customerId = 0;
            if (logonContext != null)
                customerId = logonContext.KundenNr.ToInt(0);

            CheckRefreshResources();

            _resourcesForCurrentCustomer = _resourcesForCustomers.Where(r => r.CustomerID == customerId).ToList();
        }

        private static void InitResources()
        {
            var ddbc = new DomainDbContext(ConfigurationManager.AppSettings["Connectionstring"], "");
            _timeOfLastResourceRefresh = DateTime.Now;
            _resources = ddbc.Resources;
            _resourcesForCustomers = ddbc.ResourcesForCustomers;
        }

        private static void CheckRefreshResources()
        {
            var timeOfLastResourceUpdate = GeneralConfiguration.GetConfigValue("Localization", "TimeOfLastResourceUpdate").ToNullableDateTime("yyyyMMddHHmmss");
            if (timeOfLastResourceUpdate == null)
                return;

            if (timeOfLastResourceUpdate.Value > _timeOfLastResourceRefresh)
                InitResources();
        }

        public string GetTranslation(string resource)
        {
            var translatedResource = GetTranslatedResource(resource);

            if (translatedResource == null)
                return resource;

            return translatedResource.GetTranslation();
        }

        public string GetTranslationKurz(string resource)
        {
            var translatedResource = GetTranslatedResource(resource);

            if (translatedResource == null)
                return resource;

            return translatedResource.GetTranslationKurz();
        }

        public string GetFormat(string resource)
        {
            var translatedResource = GetTranslatedResource(resource);

            if (translatedResource == null)
                return resource;

            return translatedResource.Format;
        }

        private TranslatedResource GetTranslatedResource(string resource)
        {
            var translatedResource = (from translation in _resources
                                      where translation.Resource == resource
                                      select translation).FirstOrDefault();

            if (translatedResource == null)
                return null;

            var translatedResourceForCurrentCustomer = (from translation in _resourcesForCurrentCustomer
                                                        where translation.Resource == resource
                                                        select translation).FirstOrDefault();

            var translatedResourceCloned = ModelMapping.Copy(translatedResource);
            if (translatedResourceForCurrentCustomer != null)
                translatedResourceCloned.MergeTranslatedResourceCustom(translatedResourceForCurrentCustomer);

            return translatedResourceCloned;
        }
    }
}



/*

	const string insert = "INSERT INTO [dbo].[TranslatedResource] ([Resource], [Format], [en], [de], [de_de], [de_at], [de_ch]) VALUES (N'{0}', NULL, N'{1}', N'{2}', N'{3}', N'{4}', N'{5}');";
	
	System.Text.StringBuilder sb = new System.Text.StringBuilder();
	var invariantResourceSet = CkgDomainLogic.Resources.DomainCommonResources.ResourceManager.GetResourceSet(System.Globalization.CultureInfo.InvariantCulture, true, true);
	var deutschResourceSet = CkgDomainLogic.Resources.DomainCommonResources.ResourceManager.GetResourceSet(new System.Globalization.CultureInfo("de-de"), true, true);
	foreach (DictionaryEntry invariantEntry in invariantResourceSet)
	{
		var s = deutschResourceSet.GetString(invariantEntry.Key.ToString()); // de un de-de Wert
		if (string.IsNullOrEmpty(s))
		{
			sb.AppendLine(string.Format(insert, invariantEntry.Key.ToString(), invariantEntry.Value.ToString().Replace("'", "''"), "", "", "", ""));
			continue;
		}
		
		sb.AppendLine(string.Format(insert, invariantEntry.Key.ToString(), invariantEntry.Value.ToString().Replace("'", "''"), s, "", "", ""));
	}
	
	sb.ToString().Dump();

*/