using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using GeneralTools.Models;
using CkgDomainLogic.CoC.Models;

namespace CkgDomainLogic.CoC.Services
{
    public class CocTypenLayoutService
    {
        public static Type CocDataType { get { return typeof(CocEntity); } }

        private static IEnumerable<CocMetaProperty> _cocAllMetaProperties;
        private static IEnumerable<CocMetaProperty> CocAllMetaProperties
        { 
            get
            {
                return (_cocAllMetaProperties ?? (_cocAllMetaProperties = CocDataType.GetProperties()
                                  .Where(p => p.GetCustomAttributes(false).OfType<CocLayoutAttribute>().Any())
                                  .Select(propertyInfo => new CocMetaProperty
                                      {
                                          CocLayoutAttribute = propertyInfo.GetCustomAttributes(false).OfType<CocLayoutAttribute>().First(),
                                          PropertyInfo = propertyInfo,
                                          GroupPropertiesFunc = GetGroupProperties,
                                      })));
            }
        }

        public static List<CocMetaProperty> Groups
        {
            get { return CocAllMetaProperties.GroupBy(g => g.GroupName).Select(g => g.First()).ToList(); }
        }

        public static List<CocMetaProperty> GetGroupProperties(string groupName)
        {
            return CocAllMetaProperties.Where(metaProperty => metaProperty.GroupName == groupName).ToList();
        }

        public static IHtmlString GetResponsiveValidationErrorUrl(Exception e)
        {
            if (e == null || e.HelpLink.IsNullOrEmpty())
                return null;

            return new HtmlString(string.Format("javascript:ScrollToY('{0}');", e.HelpLink));
        }

        public static string GetPropertyGroupName(string propertyName)
        {
            var property = CocAllMetaProperties.FirstOrDefault(p => p.Name == propertyName);
            if (property == null)
                return "";

            return property.GroupName.NotNullOrEmpty().Replace("~", "");
        }
    }
}
