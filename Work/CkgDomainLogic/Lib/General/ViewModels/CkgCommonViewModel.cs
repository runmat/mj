using System.Collections.Generic;
using System.Xml.Serialization;
using CkgDomainLogic.General.Database.Models;
using GeneralTools.Models;
// ReSharper disable RedundantUsingDirective
using System.Linq;
// ReSharper restore RedundantUsingDirective

namespace CkgDomainLogic.General.ViewModels
{
    public class CkgCommonViewModel : CkgBaseViewModel
    {
        public MaintenanceResult MaintenanceInfo
        {
            get
            {
                if (LogonContext == null || LogonContext.SecurityService == null)
                    return null;

                return LogonContext.ValidateMaintenance();
            }
        }

        [XmlIgnore]
        public List<Contact> CustomerGroupContacts
        {
            get
            {
                return PropertyCacheGet(() =>
                    {
                        if (LogonContext == null)
                            return new List<Contact>();

                        return LogonContext.TryGetGroupContacts().ToListOrEmptyList(); 
                    });
            }
        }


        public bool ContactImageUrlAvailable(Contact contact)
        {
            return contact.PictureName.IsNotNullOrEmpty();
        }

        public string GetContactImageUrl(Contact contact)
        {
            return string.Format("{0}{1}", AppSettings.WebPictureContactsRelativePath.ToWebPath(), contact.PictureName);
        }

        
        #region AppFavorites

        public bool AppFavoritesAvailable { get { return LogonContext != null && LogonContext.Customer != null && LogonContext.Customer.MvcSelectionType.NotNullOrEmpty().ToLower() == "favorites"; } }

        public bool BoolDummy { get; set; }

        #endregion
    }
}
