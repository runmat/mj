using DAT_BankenLinie_Connector.de.dat.www.authentication;

namespace DAT_BankenLinie_Connector
{
    public class Get_Daten
    {
        //static void Main(string[] args)
        //{
        //    string customerNumber = "1321363";
        //    string userLogin = "gidaflor";
        //    string signature = "jA0EAwMCdHGL/jscCfZgySrVgZIm3RtHDbJihjsDc+TJnoy0OAZx3Ahmsy48zclYhJJaHCu0ldyY7+Y=";

        //    string ErrorMessage = "";
        //    string HEP = "";
        //    string HVP = "";
        //    int mileage = 150000;
        //    //string datECode = "1";
        //    int constructionYear = 2006;

        //    //Get_HEP_HVP(customerNumber, userLogin, signature, mileage, datECode, constructionYear, ref HEP, ref HVP);
        //    //GetECode(customerNumber, userLogin, signature, mileage, constructionYear, "",  ref ErrorMessage);
        //}

        public static string GetECode(string customerLogin, string customerNumber, string customerSignature, string interfacePartnerNumber, string interfacePartnerSignature, int mileage, int constructionYear, string selectedContainer, ref string ErrorMessage, int[] additionalOptions, int vehicleType, int manufacturer, int baseModel, int subModel)
        {
            string ECode = "";
            try
            {
                Authentication vi = new Authentication();
                string sessionID = vi.Login(customerLogin, customerNumber, customerSignature, interfacePartnerNumber, interfacePartnerSignature);

                DAT_BankenLinie_Connector.de.dat.www.VehicleSelection viSelection = new DAT_BankenLinie_Connector.de.dat.www.VehicleSelection();
                viSelection.CookieContainer = vi.CookieContainer;

                DAT_BankenLinie_Connector.de.dat.www.locale localePeriod = new DAT_BankenLinie_Connector.de.dat.www.locale();
                localePeriod.country = "DE";
                localePeriod.datCountryIndicator = "DE";
                localePeriod.language = "de";

                DAT_BankenLinie_Connector.de.dat.www.subTypeVariantNumberSelectionRequest vsPeriod = new DAT_BankenLinie_Connector.de.dat.www.subTypeVariantNumberSelectionRequest();
                vsPeriod.sessionID = sessionID;
                vsPeriod.restriction = de.dat.www.releaseRestriction.APPRAISAL;
                vsPeriod.locale = localePeriod;
                vsPeriod.vehicleType = vehicleType;
                vsPeriod.manufacturer = manufacturer;
                vsPeriod.baseModel = baseModel;
                vsPeriod.subModel = subModel;
                if (additionalOptions.Length == 0)
                {
                    vsPeriod.selectedOptions = new int[] { -10 };
                }
                else
                {
                    vsPeriod.selectedOptions = additionalOptions;
                }

                ECode = viSelection.compileDatECode(vsPeriod);

                bool xPeriod = false;
                bool yPeriod = false;
                vi.doLogout(out xPeriod, out yPeriod);
            }
            catch
            { }
            return ECode;
        }
    }
}