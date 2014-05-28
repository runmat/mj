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
        public List<Contact> TestGroupContacts
        {
            get 
            { 
                return new List<Contact>
                {
                    new Contact { Anrede = "Frau", Name1 = "Gundulinde", Name2 = "Halmacken", Telefon = "04532 45654", Fax = null, Mail = "halmacken@test.de", Mobile = "01765461257", Abteilung = "Finanzbuchhaltung", Position = "Chefin", PictureName = "9063.jpg"},
                    new Contact { Anrede = "Frau", Name1 = "Hermine", Name2 = "Granger", Telefon = "04102 6987635", Fax = "040 97821213", Mail = "granger@dad.de", Mobile = null, Abteilung = "Verkauf", Position = "Disponentin", PictureName = "9065.jpg" },
                    new Contact { Anrede = "Herr", Name1 = "Göster", Name2 = "Reuther", Telefon = "040 454541211", Fax = null, Mail = "gr@ckg.de", Mobile = null, Abteilung = "Vertrieb", Position = "Berater", PictureName = "9155.jpg" },
                }; 
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

                        return LogonContext.TryGetGroupContacts().ToListOrEmptyList(); //.Concat(TestGroupContacts).ToList();
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
    }
}
