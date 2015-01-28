using System;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using GeneralTools.Contracts;
using GeneralTools.Services;
using CKG.Base.Kernel.Security;
using CKG.Base.Kernel.Common;

namespace AutohausPortal
{
    public partial class Log : Page
    {
        public static readonly int PortalType = 6;  // 1 = Portal, 2 = Services, 3 = ServicesMvc, 4 = KBS, 5 = PortalZLD, 6 = AutohausPortal

        /// <summary>
        /// Page Visit Logging
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            // Hole die url aus dem query und leite den Anwender dorthin wo er eigentlich hinwollte
            if (Request.QueryString.AllKeys.Any(k => k.ToUpper() == "URL"))
            {
                var url = Request.QueryString["url"];
                url = Encoding.UTF8.GetString(Convert.FromBase64String(url));
                url = HttpUtility.UrlDecode(url);
                // durch den Aufruf setzt die Runtime die SessionId
                url = Response.ApplyAppPathModifier(url);
                Response.Redirect(url, false); // False bewirkt dass das Redirect gesetzt wird ohne dass die Ausführung unterbrochen wird.
            }

            if (!Request.QueryString.AllKeys.Any(key => key.ToUpper() == "APP-ID"))
            {
                // Loggen nicht möglich, keine gültige App ID
                return;
            }

            User userObject = Common.GetUser(this);
            if (userObject == null || userObject.Customer == null)
            {
                // Loggen nicht möglich, User nicht eingeloggt oder kein Kunde gesetzt
                return;
            }

            string appId = Request.QueryString["APP-ID"];

            ILogService logService = new LogService();
            logService.LogPageVisit(Int32.Parse(appId), userObject.UserID, userObject.Customer.CustomerId, Int32.Parse(userObject.Customer.KUNNR), PortalType);
        }
    }
}