using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using CkgDomainLogic.General.Database.Models;
using CkgDomainLogic.General.Database.Services;
using GeneralTools.Contracts;

namespace CkgDomainLogic.DomainCommon.Services
{
    public class TranslationFormatService : ITranslationFormatService
    {

        private readonly DomainDbContext _domainDbContext;
        private readonly List<TranslatedResource> _resourcesForCurrentCustomer;

        public TranslationFormatService(ISessionDataHelper sessionDataHelper)
        {
            var logonContext = sessionDataHelper.GetLogoncontext();
            var username = string.Empty;
            if (logonContext != null)
            {
                username = logonContext.UserName;
            }

            _domainDbContext = new DomainDbContext(ConfigurationManager.AppSettings["Connectionstring"], username);    
            
            _resourcesForCurrentCustomer = _domainDbContext.ResourcesForCurrentCustomer;
        }

        public string GetTranslation(string resource)
        {
            var translatedResource = (from translation in _resourcesForCurrentCustomer
                                     where translation.Resource == resource
                                     select translation).FirstOrDefault();

            if (translatedResource == null)
            {
                return resource;
            }

            return translatedResource.GetTranslation();
        }

        public string GetTranslationKurz(string resource)
        {
            var translatedResource = (from translation in _resourcesForCurrentCustomer
                                      where translation.Resource == resource
                                      select translation).FirstOrDefault();

            if (translatedResource == null)
            {
                return resource;
            }

            return translatedResource.GetTranslationKurz();
        }

        public string GetFormat(string resource)
        {
            var translatedResource = (from translation in _resourcesForCurrentCustomer
                                      where translation.Resource == resource
                                      select translation).FirstOrDefault();

            if (translatedResource == null)
            {
                return string.Empty;
            }

            return translatedResource.Format;
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