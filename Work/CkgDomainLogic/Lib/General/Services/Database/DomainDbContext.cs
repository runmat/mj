using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Transactions;
using CkgDomainLogic.General.Database.Models;
using CkgDomainLogic.Zulassung.MobileErfassung.Models;
using GeneralTools.Services;
using GeneralTools.Models;

namespace CkgDomainLogic.General.Database.Services
{
    public class DomainDbContext : DbContext
    {
        protected string ConnectionString { get; private set; }

        public DomainDbContext(string connectionString, string userName)
            : base(connectionString)
        {
            ConnectionString = connectionString;
            UserName = userName;
        }

        public string UserName { get; private set; }

        public string UserID
        {
            get { return User == null ? "" : User.UserID.ToString(); }
        }

        public DbSet<ApplicationType> ApplicationTypes { get; set; }

        public DbSet<LoginUserMessage> LoginMessages { get; set; }

        public List<LoginUserMessage> ActiveLoginMessages { get { return LoginMessages.ToListOrEmptyList(); } }

        public List<LoginUserMessageConfirmations> GetLoginUserMessageConfirmations()
        {
            if (UserID.IsNullOrEmpty())
                return new List<LoginUserMessageConfirmations>();

            return Database.SqlQuery<LoginUserMessageConfirmations>("SELECT * FROM LoginUserMessageConfirmations WHERE UserID = {0}", UserID).ToList();
        }

        public void SetLoginUserMessageConfirmation(int messageID, DateTime showMessageFromDate)
        {
            if (UserID.IsNullOrEmpty())
                return;
            
            try
            {
                Database.ExecuteSqlCommand(
                    " insert into LoginUserMessageConfirmations (UserID, MessageID, ShowMessageFrom, ConfirmDate)" + 
                    " select {0}, {1}, {2}, getdate()", 
                    UserID, messageID, showMessageFromDate);
            }
            // ReSharper disable once EmptyGeneralCatchClause
            catch (Exception) { }
        }

        private User _user;
        public User User
        {
            get { return _user ?? (_user = GetUser(UserName)); }
        }


        public DbSet<AccountingArea> AccountingAreas { get; set; }

        public Customer GetCustomer(int customerID)
        {
            var cust = Database.SqlQuery<Customer>("SELECT * FROM Customer WHERE CustomerID = {0}", customerID).FirstOrDefault();

            if (cust != null)
            {
                var accArea = AccountingAreas.FirstOrDefault(a => a.Area == cust.AccountingArea);
                if (accArea != null)
                    cust.AccountingAreaName = accArea.Description;

                if (cust.LoginLinkID.HasValue && cust.LoginLinkID.Value > 0)
                    cust.LoginLink = GetLoginLink(cust.LoginLinkID.Value);
            }

            return cust;
        }

        public List<Customer> GetAllCustomer()
        {
            return Database.SqlQuery<Customer>("SELECT * FROM Customer").ToListOrEmptyList();
        }

        public string GetLoginLink(int loginLinkId)
        {
            return Database.SqlQuery<string>("SELECT Text FROM WebUserUploadLoginLink WHERE ID = {0}", loginLinkId).FirstOrDefault();
        }

        public string GetEmailAddressFromUserName(string userName)
        {
            return Database.SqlQuery<string>("select mail from vwWebUser inner join WebUserInfo on vwWebUser.UserID = WebUserInfo.id_user where Username = {0}", userName).FirstOrDefault();
        }

        public string GetUserAccountLastLockedBy(string userName)
        {
            return Database.SqlQuery<string>("SELECT LastChangedBy FROM AdminHistory_User WHERE ID = (SELECT MAX(ID) FROM AdminHistory_User WHERE Username = {0} AND Action = {1})", userName, "Benutzer gesperrt").FirstOrDefault();
        }

        private bool CheckUserHasCategoryRights(string userName, string categoryName)
        {
            var sql = string.Format("select COUNT(*) from WebUserCategoryRights where CategoryName = '{0}' and HasRights = 1 and UserName = '{1}'", categoryName, userName);
            var result = Database.SqlQuery<int>(sql).First();
            return (result > 0);
        }

        public bool CheckUserHasLocalizationTranslationRights(string userName)
        {
            return CheckUserHasCategoryRights(userName, "LOKALISIERUNGS_RECHT");
        }

        public User GetUserFromPasswordToken(string passwordRequestKey)
        {
            return Database.SqlQuery<User>("select * from vwWebUser where PasswordChangeRequestKey = {0}", passwordRequestKey).FirstOrDefault();
        }

        public List<User> GetUserForCustomer(Customer customer)
        {
            return Database.SqlQuery<User>("select * from vwWebUser where CustomerID = {0}", customer.CustomerID).ToListOrEmptyList();
        }

        public User GetUser(string userName)
        {
            return Database.SqlQuery<User>("SELECT * FROM vwWebUser WHERE Username = {0}", userName).FirstOrDefault();
        }

        public User GetUserFromUserNameAndPassword(string userName, string password)
        {
            return Database.SqlQuery<User>("SELECT * FROM vwWebUser WHERE Username = {0} and  Password = {1}", userName, password).FirstOrDefault();
        }

        public User GetUserFromUrlRemoteLoginKey(string urlRemoteLoginKey)
        {
            return Database.SqlQuery<User>("SELECT * FROM vwWebUser WHERE UrlRemoteLoginKey = {0}", urlRemoteLoginKey).FirstOrDefault();
        }

        public User GetAndSetUser(string userName)
        {
            UserName = userName;
            return _user = GetUser(userName);
        }

        public User GetAndSetUserFromUserNameAndPassword(string userName, string password)
        {
            UserName = userName;
            return _user = GetUserFromUserNameAndPassword(userName, password);
        }

        public User GetAndSetUserFromUrlRemoteLoginKey(string urlRemoteLoginKey)
        {
            _user = GetUserFromUrlRemoteLoginKey(urlRemoteLoginKey);
            if (_user != null)
                UserName = User.Username;
            return _user;
        }

        public Customer GetCustomerFromUserName(string userName)
        {
            var user = GetUser(userName);
            if (user == null)
                return null;

            return GetCustomer(user.CustomerID);
        }

        public WebUserInfo GetUserInfo()
        {
            return Database.SqlQuery<WebUserInfo>("SELECT * FROM WebUserInfo WHERE ID_User = {0}", UserID).FirstOrDefault();
        }

        public List<WebUserPwdHistory> GetUserPwdHistory()
        {
            return Database.SqlQuery<WebUserPwdHistory>("SELECT * FROM WebUserPwdHistory WHERE UserID = {0} ORDER BY DateOfChange DESC ", UserID).ToListOrEmptyList();
        }

        public List<string> GetAddressPostcodeCityMapping(string plz)
        {
            int plzInt;

            if (!Int32.TryParse(plz, out plzInt))
                return new List<string>();

            List<string> cityList;
            try
            {
                var query = Database.SqlQuery<AddressPostcodeCityMapping>("SELECT * FROM AddressPostcodeCityMapping WHERE PLZ = {0}", plzInt).ToList();
                cityList = query.OrderBy(c => c.Ort).GroupBy(c => c.Ort).Select(c => c.Key).ToList();
            }
            catch
            {
                cityList = new List<string>();
            }

            return cityList;
        }

        public DbSet<UserGroup> UserGroups { get; set; }

        private List<UserGroup> _userGroupsOfCurrentCustomer;
        public List<UserGroup> UserGroupsOfCurrentCustomer
        {
            get
            {
                return _userGroupsOfCurrentCustomer ?? (_userGroupsOfCurrentCustomer = UserGroups.Where(x => x.CustomerID == User.CustomerID).ToList());
            }
        }

        private UserGroup _userGroup;
        public UserGroup UserGroup
        {
            get
            {
                return _userGroup ?? (_userGroup = Database.SqlQuery<UserGroup>(" SELECT " +
                                                                                  " wg.* " +
                                                                                  " FROM WebUser wu " +
                                                                                  " INNER JOIN WebMember wm ON wm.UserID = wu.UserID " +
                                                                                  " INNER JOIN WebGroup wg ON wm.GroupID = wg.GroupID " +
                                                                                  " WHERE wu.UserID = {0} "
                                                                                  , UserID).FirstOrDefault());
            }
        }

        public DbSet<Organization> Organizations { get; set; }

        private UserOrganization _organization;
        public UserOrganization Organization
        {
            get
            {
                return _organization ?? (_organization = Database.SqlQuery<UserOrganization>(" SELECT " +
                                                                                  " og.*, om.OrganizationAdmin " +
                                                                                  " FROM WebUser wu " +
                                                                                  " INNER JOIN OrganizationMember om ON om.UserID = wu.UserID " +
                                                                                  " INNER JOIN Organization og ON om.OrganizationID = og.OrganizationID " +
                                                                                  " WHERE wu.UserID = {0} "
                                                                                  , UserID).FirstOrDefault());
            }
        }

        public List<Contact> GetGroupContacts(int customerID, string groupName)
        {
            return Database.SqlQuery<Contact>(string.Format(" SELECT " +
                                                            " Contact.* " +
                                                            " FROM Contact  " +
                                                            " INNER JOIN ContactGroups ON " +
                                                            "	Contact.id = ContactGroups.ContactID " +
                                                            " INNER JOIN WebGroup ON " +
                                                            "	ContactGroups.GroupID = WebGroup.GroupID " +
                                                            " WHERE     (ContactGroups.CustomerID = {0}) AND (WebGroup.GroupName = '{1}')"
                                                            , customerID, groupName )).ToListOrEmptyList();
        }

        private IEnumerable<ApplicationUser> _userApps;

        public IEnumerable<ApplicationUser> UserApps
        {
            get
            {
                if (_userApps != null)
                    return _userApps;

                Database.ExecuteSqlCommand("exec ApplicationUserSettingsPrepareCustomerDefaults {0}", UserID);
                _userApps = Database.SqlQuery<ApplicationUser>("SELECT * FROM vwApplicationWebUser WHERE UserID = {0}", UserID);

                return _userApps;
            }
        }

        public void UserAppsRefresh()
        {
            _userApps = null;
        }

        public bool TryLogin(string password)
        {
            if (String.IsNullOrEmpty(password))
                return false;

            var tempUser = Database.SqlQuery<User>("SELECT * FROM vwWebUser WHERE Username = {0} AND password = {1}",
                    UserName,
                    System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(password, "sha1")).FirstOrDefault();

            if (tempUser == null)
                return false;

            return true;
        }

        public void FailedLoginsIncrementAndSave(string userName)
        {
            Database.ExecuteSqlCommand("UPDATE WebUser SET FailedLogins = FailedLogins + 1 WHERE Username = {0}", userName);
        }

        public void LockUserAndSave(string userName)
        {
            Database.ExecuteSqlCommand("UPDATE WebUser SET AccountIsLockedOut = 1, LastChangedBy = {0} WHERE Username = {1}", UserName, userName);
        }

        public void UnlockUserAndSave()
        {
            Database.ExecuteSqlCommand("UPDATE WebUser SET AccountIsLockedOut = 0, LastChangedBy = {0} WHERE UserID = {1}", UserName, UserID);
        }

        public void FailedLoginsResetAndSave()
        {
            Database.ExecuteSqlCommand("UPDATE WebUser SET FailedLogins = 0 WHERE UserID = {0}", UserID);
        }

        public void StorePasswordToUser(string password, int passwordMinHistoryEntries)
        {
            if (User != null)
                User.Password = password;

            Database.ExecuteSqlCommand("UPDATE WebUser SET Password = {0}, LastPwdChange = getdate() WHERE UserID = {1}", password, UserID);

            Database.ExecuteSqlCommand("INSERT INTO WebUserPwdHistory (UserID, Password, DateOfChange, InitialPwd) VALUES ({0}, {1}, getdate(), 0)", UserID, password);

            var pwdHistory = GetUserPwdHistory();
            if (pwdHistory.Count > passwordMinHistoryEntries)
            {
                for (var i = passwordMinHistoryEntries; i < pwdHistory.Count; i++)
                {
                    Database.ExecuteSqlCommand("DELETE FROM WebUserPwdHistory WHERE ID = {0}", pwdHistory[i].ID);
                }
            }

            FailedLoginsResetAndSave();
            UnlockUserAndSave();
        }

        public void StorePasswordRequestKeyToUser(string userName, string passwordRequestKey)
        {
            if (User != null)
                User.Password = passwordRequestKey;

            Database.ExecuteSqlCommand("UPDATE WebUser SET PasswordChangeRequestKey = {0} WHERE Username = {1}", passwordRequestKey, userName);
        }

        public bool TryChangePassword(string oldPassword, string newPassword)
        {
            if ((String.IsNullOrEmpty(oldPassword)) || (String.IsNullOrEmpty(newPassword)))
                return false;

            var erg = Database.ExecuteSqlCommand("UPDATE WebUser SET Password = {0} WHERE Username = {1} AND Password = {2}",
                    System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(newPassword, "sha1"),
                    UserName,
                    System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(oldPassword, "sha1"));

            return (erg > 0);
        }

        public void SetLastLogin(DateTime zeitpunkt)
        {
            Database.ExecuteSqlCommand("UPDATE WebUser SET LastLogin = {0} WHERE Username = {1}",
                    zeitpunkt,
                    UserName);
        }

        public void DataContextPersist(object dataContext)
        {
            var type = dataContext.GetType();

            var contextKey = type.GetFullTypeName();
            var data = XmlService.XmlSerializeToString(dataContext);

            Database.ExecuteSqlCommand("EXEC WebUserSessionContextSave {0}, {1}, {2}", UserName, contextKey, data);
        }

        public object DataContextRestore(string typeName)
        {
            var sessionContext = Database.SqlQuery<WebUserSessionContext>("SELECT * FROM WebUserSessionContext WHERE UserName = {0} and ContextKey = {1}", UserName, typeName).FirstOrDefault();
            if (sessionContext == null)
                return null;

            var type = Type.GetType(typeName);
            if (type == null)
                return null;

            return XmlService.XmlDeserializeFromString(sessionContext.ContextData, type);
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            System.Data.Entity.Database.SetInitializer<DomainDbContext>(null);

            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }


        #region Dokument

        public DbSet<Document> Documents { get; set; }

        public List<Document> DocumentsForAll
        {
            get
            {
                var doks = Documents.Where(x => x.CustomerID == 1).ToList();

                doks.ForEach(x => x.DocTypeName = DocumentTypesForCustomer.Find(t => t.DocumentTypeID == x.DocTypeID).DocTypeName);

                return doks;
            }
        }

        public List<Document> DocumentsForCustomer
        {
            get
            {
                var doks = Documents.Where(x => x.CustomerID == User.CustomerID).ToList();

                doks.ForEach(x => x.DocTypeName = DocumentTypesForCustomer.Find(t => t.DocumentTypeID == x.DocTypeID).DocTypeName);

                return doks;
            }
        }

        public List<Document> DocumentsForGroup
        {
            get
            {
                var documentIds = from documentRight in DocumentRights
                                 where documentRight.GroupID == UserGroup.GroupID
                                 select documentRight.DocumentID;

                var doks = Documents.Where(x => documentIds.Contains(x.DocumentID)).ToList();

                doks.ForEach(x => x.DocTypeName = DocumentTypesForCustomer.Find(t => t.DocumentTypeID == x.DocTypeID).DocTypeName);

                return doks;
            }
        }

        public Document SaveDocument(Document document)
        {
            // ID = 0 => neues Dokument
            if (document.DocumentID == 0)
                Documents.Add(document);                

            SaveChanges();

            return document;
        }

        /// <summary>
        /// Dokument DocTypeId anpassen und die Zuordnung zu den Gruppen neu erstellen
        /// </summary>
        /// <returns></returns>
        public bool SaveDocument(int documentId, int? docTypeId, List<string> selectedWebGroups, string tags)
        {
            // DokumentRights für das Dokument ausfüllen
            var documentRights = DocumentRights.Where(x => x.DocumentID == documentId);

            // Dokument ermitteln
            var document = Documents.Single(x => x.DocumentID == documentId);

            // Alte DokumentRights Elemente als gelöscht kennzeichnen, keine Prüfung hinsichtlich wiederverwendbarkeit
            documentRights.ToList().ForEach(x => DocumentRights.Remove(x));

            // Neue DokumentRights eintragen
            if (selectedWebGroups != null)
            {
                foreach (var webGroup in selectedWebGroups)
                {
                    var groupID = UserGroupsOfCurrentCustomer.Single(x => x.GroupID == int.Parse(webGroup)).GroupID;

                    var newDocumentRight = new DocumentRight
                    {
                        DocumentID = documentId,
                        GroupID = groupID
                    };

                    DocumentRights.Add(newDocumentRight);
                }
            }

            document.DocTypeID = docTypeId;
            document.Tags = tags;
            document.LastEdited = DateTime.Now;

            // Jetzt alles auf ein mal schreiben! EF kümmert sich um die Transaction
            var itemsSaved = SaveChanges();
            return itemsSaved > 0;
        }

        public bool DeleteDocument(int documentId)
        {
            var documentRights = DocumentRights.Where(x => x.DocumentID == documentId);
            var documentToRemove = Documents.Single(x => x.DocumentID == documentId);

            using (var scope = new TransactionScope())
            {
                // Alte DokumentRights Elemente als gelöscht kennzeichnen, keine Prüfung hinsichtlich wiederverwendbarkeit
                documentRights.ToList().ForEach(x => DocumentRights.Remove(x));
                SaveChanges();

                Documents.Remove(documentToRemove);
                SaveChanges();

                scope.Complete();

                return true;
            }
        }

        #endregion

        #region DocumentType

        public DbSet<DocumentType> DocumentTypes { get; set; }

        public List<DocumentType> DocumentTypesForAll
        {
            get
            {
                // DocType mit ID 1 ist der default-Wert für alle Kunden
                return DocumentTypes.Where(x => x.CustomerID == 1 || x.DocumentTypeID == 1).ToList();
            }
        }

        public List<DocumentType> DocumentTypesForCustomer
        {
            get
            {
                // DocType mit ID 1 ist der default-Wert für alle Kunden
                return DocumentTypes.Where(x => x.CustomerID == User.CustomerID || x.DocumentTypeID == 1).ToList();
            }
        }

        public DocumentType SaveDocumentType(DocumentType documentType)
        {
            // ID = 0 => neuer Dokumenttyp
            if (documentType.DocumentTypeID == 0)
            {
                DocumentTypes.Add(documentType);
            }
            else
            {
                var documentTypeToUpdate = DocumentTypes.Single(x => x.DocumentTypeID == documentType.DocumentTypeID);
                documentTypeToUpdate.DocTypeName = documentType.DocTypeName;
            }

            SaveChanges();

            return documentType;
        }

        public bool DeleteDocumentType(int documentTypeId)
        {
            var documentTypeToRemove = DocumentTypes.Single(x => x.DocumentTypeID == documentTypeId);
            DocumentTypes.Remove(documentTypeToRemove);
            var itemsDeleted = SaveChanges();
            return itemsDeleted > 0;
        }

        #endregion

        #region DocumentRight

        public DbSet<DocumentRight> DocumentRights { get; set; }

        #endregion

        #region TranslatedResource

        public DbSet<TranslatedResource> TranslatedResources { get; set; }

        public DbSet<TranslatedResourceCustom> TranslatedResourceCustoms { get; set; }

        public List<TranslatedResource> Resources
        {
            get { return TranslatedResources.ToList(); }
        }

        public List<TranslatedResourceCustom> ResourcesForCustomers
        {
            get { return TranslatedResourceCustoms.ToList(); }
        }

        public TranslatedResource TranslatedResourceLoad(string resourceKey)
        {
            var t = Database.SqlQuery<TranslatedResource>("SELECT * FROM TranslatedResource WHERE Resource = {0}", resourceKey).FirstOrDefault();
            return (t ?? new TranslatedResource { Resource = resourceKey });
        }

        public TranslatedResourceCustom TranslatedResourceCustomerLoad(string resourceKey, int customerID)
        {
            var t = Database.SqlQuery<TranslatedResourceCustom>("SELECT * FROM TranslatedResourceCustom WHERE Resource = {0} and CustomerID = {1}", resourceKey, customerID).FirstOrDefault();
            return (t ?? new TranslatedResourceCustom { Resource = resourceKey, CustomerID = customerID });
        }

        public void TranslatedResourceUpdate(TranslatedResource r, string userName)
        {
            if (r.de.IsNullOrEmpty() || r.en.IsNullOrEmpty())
                return;

            Database.ExecuteSqlCommand(
                " if not exists(select Resource from TranslatedResource where Resource = {0}) " +
                "   insert into TranslatedResource (Resource, en, en_kurz, de, de_kurz, fr, fr_kurz) select {0}, {1}, {1}, {1}, {1}, {1}, {1}", 
                r.Resource, "[INSERTED]");

            Database.ExecuteSqlCommand(
                " insert into TranslatedResource_UserLogs ( " +
                "  Resource, " +
                "  de_alt, " +
                "  de_kurz_alt, " +
                "  de_neu, " +
                "  de_kurz_neu, " +
                "  en_alt, " +
                "  en_kurz_alt, " +
                "  en_neu, " +
                "  en_kurz_neu, " +
                "  fr_alt, " +
                "  fr_kurz_alt, " +
                "  fr_neu, " +
                "  fr_kurz_neu, " +
                "  ChangeDate, " +
                "  ChangeUser " +
                "  ) " +
                " select " +
                "  tr.Resource, " +
                "  tr.de, " +
                "  tr.de_kurz, " +
                "  {0}, " +
                "  {1}, " +
                "  tr.en, " +
                "  tr.en_kurz, " +
                "  {2}, " +
                "  {3}, " +
                "  tr.fr, " +
                "  tr.fr_kurz, " +
                "  {4}, " +
                "  {5}, " +
                "  getdate(), " +
                "  {6} " +
                " from TranslatedResource tr " +
                " where tr.Resource = {7} " +
                " and ( " +
                "       convert(varchar(4096),isnull(tr.de,'')) <> isnull({0},'') or isnull(tr.de_kurz,'') <> isnull({1},'') or " +
                "       convert(varchar(4096),isnull(tr.en,'')) <> isnull({2},'') or isnull(tr.en_kurz,'') <> isnull({3},'') or " +
                "       convert(varchar(4096),isnull(tr.fr,'')) <> isnull({4},'') or isnull(tr.fr_kurz,'') <> isnull({5},'') " +
                "     ) ",
                    r.de, r.de_kurz,
                    r.en, r.en_kurz,
                    r.fr, r.fr_kurz,
                    userName, r.Resource);

            Database.ExecuteSqlCommand(
                " update TranslatedResource set " +
                "  en = {0}, " +
                "  en_kurz = {1}, " +
                "  de = {2}, " +
                "  de_kurz = {3}, " +
                "  fr = {4}, " +
                "  fr_kurz = {5} " +
                " where Resource = {6}",
                    r.en, r.en_kurz,
                    r.de, r.de_kurz,
                    r.fr, r.fr_kurz,
                    r.Resource);
        }

        public void TranslatedResourceDelete(TranslatedResource r, string userName)
        {
            Database.ExecuteSqlCommand(
                " insert into TranslatedResource_UserLogs ( " +
                "  Resource, " +
                "  de_alt, " +
                "  de_kurz_alt, " +
                "  de_neu, " +
                "  de_kurz_neu, " +
                "  en_alt, " +
                "  en_kurz_alt, " +
                "  en_neu, " +
                "  en_kurz_neu, " +
                "  fr_alt, " +
                "  fr_kurz_alt, " +
                "  fr_neu, " +
                "  fr_kurz_neu, " +
                "  ChangeDate, " +
                "  ChangeUser " +
                "  ) " +
                " select " +
                "  tr.Resource, " +
                "  tr.de, " +
                "  tr.de_kurz, " +
                "  '[DELETED]', " +
                "  '[DELETED]', " +
                "  tr.en, " +
                "  tr.en_kurz, " +
                "  '[DELETED]', " +
                "  '[DELETED]', " +
                "  tr.fr, " +
                "  tr.fr_kurz, " +
                "  '[DELETED]', " +
                "  '[DELETED]', " +
                "  getdate(), " +
                "  {0} " +
                " from TranslatedResource tr " +
                " where tr.Resource = {1} ",
                    userName, r.Resource);

            Database.ExecuteSqlCommand(
                " delete from TranslatedResource where Resource = {0}",
                    r.Resource);
        }

        public void TranslatedResourceCustomerUpdate(TranslatedResourceCustom r, string userName)
        {
            if (r.de.IsNullOrEmpty() || r.en.IsNullOrEmpty())
                return;

            Database.ExecuteSqlCommand(
                " if not exists (select Resource from TranslatedResourceCustom where Resource = {0} and CustomerID = {1}) " +
                "   insert into TranslatedResourceCustom (Resource, CustomerID, en, en_kurz, de, de_kurz, fr, fr_kurz) select {0}, {1}, {2}, {2}, {2}, {2}, {2}, {2}",
                r.Resource, r.CustomerID, "[INSERTED]");

            Database.ExecuteSqlCommand(
                " insert into TranslatedResourceCustom_UserLogs ( " +
                "  Resource, " +
                "  CustomerID, " +
                "  de_alt, " +
                "  de_kurz_alt, " +
                "  de_neu, " +
                "  de_kurz_neu, " +
                "  en_alt, " +
                "  en_kurz_alt, " +
                "  en_neu, " +
                "  en_kurz_neu, " +
                "  fr_alt, " +
                "  fr_kurz_alt, " +
                "  fr_neu, " +
                "  fr_kurz_neu, " +
                "  ChangeDate, " +
                "  ChangeUser " +
                "  ) " +
                " select " +
                "  tr.Resource, " +
                "  tr.CustomerID, " +
                "  tr.de, " +
                "  tr.de_kurz, " +
                "  {0}, " +
                "  {1}, " +
                "  tr.en, " +
                "  tr.en_kurz, " +
                "  {2}, " +
                "  {3}, " +
                "  tr.fr, " +
                "  tr.fr_kurz, " +
                "  {4}, " +
                "  {5}, " +
                "  getdate(), " +
                "  {6} " +
                " from TranslatedResourceCustom tr " +
                " where tr.Resource = {7} and tr.CustomerID = {8} " +
                " and ( " +
                "       convert(varchar(4096),isnull(tr.de,'')) <> isnull({0},'') or isnull(tr.de_kurz,'') <> isnull({1},'') or " +
                "       convert(varchar(4096),isnull(tr.en,'')) <> isnull({2},'') or isnull(tr.en_kurz,'') <> isnull({3},'') or " +
                "       convert(varchar(4096),isnull(tr.fr,'')) <> isnull({4},'') or isnull(tr.fr_kurz,'') <> isnull({5},'') " +
                "     ) ",
                    r.de, r.de_kurz,
                    r.en, r.en_kurz,
                    r.fr, r.fr_kurz,
                    userName, r.Resource, r.CustomerID);

            Database.ExecuteSqlCommand(
                " update TranslatedResourceCustom set " +
                "  en = {0}, " +
                "  en_kurz = {1}, " +
                "  de = {2}, " +
                "  de_kurz = {3}, " +
                "  fr = {4}, " +
                "  fr_kurz = {5} " +
                " where Resource = {6} and CustomerID = {7}",
                    r.en, r.en_kurz,
                    r.de, r.de_kurz,
                    r.fr, r.fr_kurz,
                    r.Resource, r.CustomerID);
        }

        public void TranslatedResourceCustomerDelete(TranslatedResourceCustom r, string userName)
        {
            Database.ExecuteSqlCommand(
                " insert into TranslatedResourceCustom_UserLogs ( " +
                "  Resource, " +
                "  CustomerID, " +
                "  de_alt, " +
                "  de_kurz_alt, " +
                "  de_neu, " +
                "  de_kurz_neu, " +
                "  en_alt, " +
                "  en_kurz_alt, " +
                "  en_neu, " +
                "  en_kurz_neu, " +
                "  fr_alt, " +
                "  fr_kurz_alt, " +
                "  fr_neu, " +
                "  fr_kurz_neu, " +
                "  ChangeDate, " +
                "  ChangeUser " +
                "  ) " +
                " select " +
                "  tr.Resource, " +
                "  tr.CustomerID, " +
                "  tr.de, " +
                "  tr.de_kurz, " +
                "  '[DELETED]', " +
                "  '[DELETED]', " +
                "  tr.en, " +
                "  tr.en_kurz, " +
                "  '[DELETED]', " +
                "  '[DELETED]', " +
                "  tr.fr, " +
                "  tr.fr_kurz, " +
                "  '[DELETED]', " +
                "  '[DELETED]', " +
                "  getdate(), " +
                "  {0} " +
                " from TranslatedResourceCustom tr " +
                " where tr.Resource = {1} and tr.CustomerID = {2} ",
                    userName, r.Resource, r.CustomerID);

            Database.ExecuteSqlCommand(
                " delete from TranslatedResourceCustom where Resource = {0} and CustomerID = {1}",
                    r.Resource, r.CustomerID);
        }
        
        #endregion

        #region CustomerDocument

        /// <summary>
        /// Alle Kundendokumente aus der DB
        /// </summary>
        public DbSet<CustomerDocument> CustomerDocuments { get; set; }

        /// <summary>
        /// Alle Kundendokumente des jeweiligen Kunden
        /// </summary>
        public List<CustomerDocument> CustomerDocumentsForCustomer
        {
            get
            {
                var doks = CustomerDocuments.Where(x => x.CustomerID == User.CustomerID).ToList();

                doks.ForEach(x => x.Category = CustomerDocumentCategoriesForCustomer.Find(c => c.ID == x.CategoryID).CategoryName);

                return doks;
            }
        }

        /// <summary>
        /// Kundendokumente des jeweiligen Kunden für den angegebenen Anwendungsfall
        /// </summary>
        public List<CustomerDocument> CustomerDocumentsForCustomerApplication(string appKey)
        {
            return CustomerDocumentsForCustomer.Where(x => x.ApplicationKey == appKey).ToList();
        }

        /// <summary>
        /// Kundendokumente des jeweiligen Kunden für den angegebenen Anwendungsfall und Referenzschlüssel (z.B. VIN, VorgangID, ...)
        /// </summary>
        public List<CustomerDocument> CustomerDocumentsForCustomerApplicationForReferenceKey(string appKey, string referenceKey)
        {
            return CustomerDocumentsForCustomerApplication(appKey).Where(x => x.ReferenceField.ToString().PadLeft(10,'0') == referenceKey).ToList();
        }

        public CustomerDocument SaveCustomerDocument(CustomerDocument doc)
        {
            if (doc.ID == 0)
            {
                CustomerDocuments.Add(doc);
            }
            else
            {
                var docToUpdate = CustomerDocuments.Single(d => d.ID == doc.ID);
                docToUpdate.ReferenceField = doc.ReferenceField;
                docToUpdate.FileType = doc.FileType;
                docToUpdate.FileName = doc.FileName;
                docToUpdate.CategoryID = doc.CategoryID;
                docToUpdate.Uploaded = doc.Uploaded;
                docToUpdate.AdditionalData = doc.AdditionalData;
                docToUpdate.CustomerID = doc.CustomerID;
                docToUpdate.ApplicationKey = doc.ApplicationKey;
            }

            SaveChanges();

            return doc;
        }

        public int DeleteCustomerDocument(CustomerDocument doc)
        {
            var docToRemove = CustomerDocuments.Single(x => x.ID == doc.ID);
            CustomerDocuments.Remove(docToRemove);
            return SaveChanges();
        }

        public int DeleteCustomerDocument(int id)
        {
            var docToRemove = CustomerDocuments.Single(x => x.ID == id);
            CustomerDocuments.Remove(docToRemove);
            return SaveChanges();
        }

        #endregion

        #region CustomerDocumentCategory

        /// <summary>
        /// Alle Kundendokument-Kategorien aus der DB
        /// </summary>
        public DbSet<CustomerDocumentCategory> CustomerDocumentCategories { get; set; }

        /// <summary>
        /// Alle Kundendokument-Kategorien des jeweiligen Kunden
        /// </summary>
        public List<CustomerDocumentCategory> CustomerDocumentCategoriesForCustomer
        {
            get
            {
                return CustomerDocumentCategories.Where(x => x.CustomerID == User.CustomerID).ToList();
            }
        }

        /// <summary>
        /// Kundendokumente des jeweiligen Kunden für den angegebenen Anwendungsfall
        /// </summary>
        public List<CustomerDocumentCategory> CustomerDocumentCategoriesForCustomerApplication(string appKey)
        {
            return CustomerDocumentCategoriesForCustomer.Where(x => x.ApplicationKey == appKey).ToList();
        }

        public CustomerDocumentCategory SaveCustomerDocumentCategory(CustomerDocumentCategory cat)
        {
            if (cat.ID == 0)
            {
                CustomerDocumentCategories.Add(cat);
            }
            else
            {
                var catToUpdate = CustomerDocumentCategories.Single(c => c.ID == cat.ID);
                catToUpdate.CategoryName = cat.CategoryName;
                catToUpdate.CustomerID = cat.CustomerID;
                catToUpdate.DisplayOrder = cat.DisplayOrder;
                catToUpdate.ApplicationKey = cat.ApplicationKey;
            }

            SaveChanges();

            return cat;
        }

        public int DeleteCustomerDocumentCategory(CustomerDocumentCategory cat)
        {
            var catToRemove = CustomerDocumentCategories.Single(x => x.ID == cat.ID);
            CustomerDocumentCategories.Remove(catToRemove);
            return SaveChanges();
        }

        public int DeleteCustomerDocumentCategory(int id)
        {
            if (CustomerDocuments.Any(x => x.CategoryID == id))
            {
                return 0;
            }

            var catToRemove = CustomerDocumentCategories.Single(x => x.ID == id);
            CustomerDocumentCategories.Remove(catToRemove);
            return SaveChanges();
        }

        #endregion

        #region Abrufgruende

        public string GetAbrufgrundBezeichnung(string grundId)
        {
            var ergs =
                Database.SqlQuery<string>(
                    "SELECT WebBezeichnung FROM CustomerAbrufgruende WHERE CustomerID = {0} AND SAPWert = {1}",
                    User.CustomerID, grundId).ToList();

            return (ergs.Count > 0 ? ergs[0] : "");
        }

        #endregion

        #region ZLD (Mobile)

        public CkgUserInfo GetCkgUserInfo(string username)
        {
            return Database.SqlQuery<CkgUserInfo>("SELECT * FROM vwCkgUser WHERE Username = {0}", username).FirstOrDefault();
        }

        #endregion

        #region Archive

        public IEnumerable<GroupArchiveAssigned> GetArchives(int customerId = 0, int groupId = 0)
        {
            var query = "SELECT * FROM vwGroupArchivAssigned";

            if (customerId > 0)
            {
                query += " WHERE CustomerID = " + customerId;

                if (groupId > 0)
                    query += " AND GroupID = " + groupId;
            }

            return Database.SqlQuery<GroupArchiveAssigned>(query);
        }

        #endregion

        #region DataConverter

        public IEnumerable<string> GetDataConverterProcessStructureNames()
        {
            return Database.SqlQuery<string>("SELECT ProcessName FROM ProcessStructure");
        }

        public DataConverterProcessStructure GetDataConverterProcessStructure(string processName)
        {
            return Database.SqlQuery<DataConverterProcessStructure>("SELECT * FROM ProcessStructure WHERE ProcessName = {0}", processName).FirstOrDefault();
        }

        public bool SaveDataConverterProcessStructure(DataConverterProcessStructure processStructure)
        {
            int rowsAffected;

            var existingElement = Database.SqlQuery<DataConverterProcessStructure>("SELECT * FROM ProcessStructure WHERE ProcessName = {0}", processStructure.ProcessName).FirstOrDefault();

            if (existingElement == null)
                rowsAffected = Database.ExecuteSqlCommand("INSERT INTO ProcessStructure (ProcessName, DestinationStructure) VALUES ({0}, {1})",
                    processStructure.ProcessName, processStructure.DestinationStructure);
            else
                rowsAffected = Database.ExecuteSqlCommand("UPDATE ProcessStructure SET DestinationStructure = {0} WHERE ProcessName = {1}",
                    processStructure.DestinationStructure, processStructure.ProcessName);

            return (rowsAffected > 0);
        }

        public bool DeleteDataConverterProcessStructure(string processName)
        {
            var rowsAffected = Database.ExecuteSqlCommand("DELETE FROM ProcessStructure WHERE ProcessName = {0}", processName);

            return (rowsAffected > 0);
        }

        public IEnumerable<DataConverterMappingInfo> GetDataConverterMappingInfos(int customerId, string processName)
        {
            var query = "SELECT * FROM vwDataMapping";
            var filterByCustomer = false;

            if (customerId > 0)
            {
                query += " WHERE CustomerId = " + customerId;
                filterByCustomer = true;
            }

            if (!string.IsNullOrEmpty(processName))
                query += " " + (filterByCustomer ? "AND" : "WHERE") + " Process = '" + processName + "'";

            return Database.SqlQuery<DataConverterMappingInfo>(query);
        }

        public DataConverterMappingData GetDataConverterMappingData(int mappingId)
        {
            return Database.SqlQuery<DataConverterMappingData>("SELECT * FROM DataMapping WHERE Id = {0}", mappingId).FirstOrDefault();
        }

        public bool SaveDataConverterMapping(DataConverterMappingData mapping)
        {
            if (mapping.Id == 0)
            {
                var newId = Database.SqlQuery<decimal>("SET NOCOUNT ON; INSERT INTO DataMapping (Title, CustomerId, Process, Mapping) VALUES ({0}, {1}, {2}, {3}); SELECT SCOPE_IDENTITY(); SET NOCOUNT OFF;",
                    mapping.Title, mapping.CustomerId, mapping.Process, mapping.Mapping).FirstOrDefault();

                mapping.Id = (int)newId;

                return (newId > 0);
            }

            var rowsAffected = Database.ExecuteSqlCommand("UPDATE DataMapping SET Mapping = {0} WHERE Id = {1}", mapping.Mapping, mapping.Id);

            return (rowsAffected > 0);
        }

        public bool DeleteDataConverterMapping(int mappingId)
        {
            var rowsAffected = Database.ExecuteSqlCommand("DELETE FROM DataMapping WHERE Id = {0}", mappingId);

            return (rowsAffected > 0);
        }

        #endregion
    }
}
