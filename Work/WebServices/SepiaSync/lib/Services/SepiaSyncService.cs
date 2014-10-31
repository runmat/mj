using System;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using GeneralTools.Models;
using SepiaSyncLib.Models;
using WebTools.Services;

namespace SepiaSyncLib.Services
{
    public class SepiaSyncService
    {
        private static string ExpirationToken { get { return UserSecurityService.UrlRemoteEncryptHourAndDate(); } }

        private static string XarisRootUrl { get { return ConfigurationManager.AppSettings["XarisSepiaRootUrl"]; } }

        private static string XarisSepiaUserSyncRelativeUrl { get { return ConfigurationManager.AppSettings["XarisSepiaUserSyncRelativeUrl"]; } }

        public static string XarisSepiaUserSyncUrl { get { return string.Format("{0}{1}", XarisRootUrl, XarisSepiaUserSyncRelativeUrl); } }


        public static bool SyncUsersToSepia()
        {
            try
            {
                var repository = new SqlDbRepository();

                var usersToSync = repository.GetUsers();

                usersToSync.ToList().ForEach(user =>
                    {
                        if (InvokeSepiaSyncHttpRequest(user))
                            repository.SetSepiaSyncDateForUser(user);
                    });
            }
            catch 
            {
                return false;
            }

            return true;
        }

        static bool InvokeSepiaSyncHttpRequest(WebUserSepiaAccess user)
        {
            var sepiaSyncUrl = string.Format(XarisSepiaUserSyncUrl, 
                                                user.UrlRemoteLoginKey, 
                                                ExpirationToken, 
                                                HttpUtility.UrlEncode(user.Username), 
                                                HttpUtility.UrlEncode(user.LastName.IsNotNullOrEmpty() ? user.LastName : user.Username), 
                                                HttpUtility.UrlEncode(user.FirstName));


            // Create a request for the URL. 
            var request = WebRequest.Create(sepiaSyncUrl);
            // If required by the server, set the credentials.
            request.Credentials = CredentialCache.DefaultCredentials;
            // Get the response.
            var response = request.GetResponse();
            // Display the status.
            // Get the stream containing content returned by the server.
            var dataStream = response.GetResponseStream();
            if (dataStream == null)
                return false;

            // Open the stream using a StreamReader for easy access.
            var reader = new StreamReader(dataStream);
            // Read the content.
            var responseFromServer = reader.ReadToEnd();
            // Display the content.
            // Clean up the streams and the response.
            reader.Close();
            response.Close();

            var validResponse = responseFromServer.NotNullOrEmpty().ToUpper().SubstringTry(0, 2);
            return validResponse == "OK";
        }
    }
}
