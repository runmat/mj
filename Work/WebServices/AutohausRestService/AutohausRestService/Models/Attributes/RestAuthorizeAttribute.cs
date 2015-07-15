using System;
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
            var authHeader = actionContext.Request.Headers.Authorization;
            if (authHeader == null || authHeader.Scheme != "Basic")
            {
                HandleUnauthorizedRequest(actionContext);
                return;
            }

            string uname;
            string pword;
            RestAuthentication.GetUsernameAndPasswordFromAuthorizationHeader(authHeader, out uname, out pword);

            if (String.IsNullOrEmpty(uname) || String.IsNullOrEmpty(pword) || !IsUserValid(uname, pword))
                HandleUnauthorizedRequest(actionContext);
        }

        private bool IsUserValid(string username, string password)
        {
            var dbContext = new DomainDbContext(ConfigurationService.DbConnection, username);

            if (dbContext.User == null || dbContext.User.UserIsDisabled)
                return false;

            return dbContext.TryLogin(password);
        }
    }
}
