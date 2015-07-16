using System;
using System.Net.Http.Headers;
using System.Text;

namespace AutohausRestService.Services
{
    public class RestAuthentication
    {
        public static void GetUsernameAndPasswordFromAuthorizationHeader(AuthenticationHeaderValue auth, out string username, out string password)
        {
            string tokenString;

            try
            {
                var authBytes = Convert.FromBase64String(auth.Parameter);
                tokenString = Encoding.UTF8.GetString(authBytes);
            }
            catch (Exception)
            {
                tokenString = "";
            }

            var tokens = tokenString.Split(':');

            username = (tokens.Length > 0 ? tokens[0] : "");
            password = (tokens.Length > 1 ? tokens[1] : "");
        }
    }
}