using System;
using System.Text;
using System.Web.Http;
using System.Web.Http.Controllers;
using AutohausRestService.Services;
using CkgDomainLogic.General.Database.Services;

namespace AutohausRestService.Models
{
    public class RestAuthorizeAttribute : AuthorizeAttribute
    {
        public override void OnAuthorization(HttpActionContext actionContext)
        {
            var auth = actionContext.Request.Headers.Authorization;
            if (auth == null || auth.Scheme != "Basic")
            {
                HandleUnauthorizedRequest(actionContext);
                return;
            }

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
            if (tokens.Length < 2 || !IsUserValid(tokens[0], tokens[1]))
                HandleUnauthorizedRequest(actionContext);
        }

        private bool IsUserValid(string username, string password)
        {
            var dbContext = new DomainDbContext(ConfigurationService.DbConnection, username);

            if (dbContext.User == null)
                return false;

            return dbContext.TryLogin(password);
        }
    }
}
