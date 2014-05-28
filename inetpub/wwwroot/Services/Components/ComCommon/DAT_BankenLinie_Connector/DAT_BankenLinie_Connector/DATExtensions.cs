using DAT_BankenLinie_Connector.de.dat.www.authentication;

namespace DAT_BankenLinie_Connector
{
    public static class DATExtensions
    {
        public static string Login(this Authentication auth, string customerLogin, string customerNumber,
            string customerSignature, string interfacePartnerNumber, string interfacePartnerSignature)
        {
            auth.CookieContainer = new System.Net.CookieContainer();

            string sessionID = auth.doLogin(new doLoginRequest
            {
                customerLogin = customerLogin,
                customerNumber = customerNumber,
                customerSignature = customerSignature,
                interfacePartnerNumber = interfacePartnerNumber,
                interfacePartnerSignature = interfacePartnerSignature
            });

            return sessionID;
        }
    }
}
