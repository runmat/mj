using DAT_BankenLinie_Connector.de.dat.gold.authentication;

namespace DAT_BankenLinie_Connector
{
    public class Get_Daten
    {
        public static string GetECode(string customerLogin, string customerNumber, string customerSignature, string interfacePartnerNumber, string interfacePartnerSignature, int mileage, int constructionYear, string selectedContainer, ref string ErrorMessage, int[] additionalOptions, int vehicleType, int manufacturer, int baseModel, int subModel)
        {
            string ECode = "";
            try
            {
                Authentication vi = new Authentication();
                string sessionID = vi.Login(customerLogin, customerNumber, customerSignature, interfacePartnerNumber, interfacePartnerSignature);

                de.dat.gold.VehicleSelectionService viSelection = new de.dat.gold.VehicleSelectionService();
                viSelection.CookieContainer = vi.CookieContainer;

                de.dat.gold.locale localePeriod = new de.dat.gold.locale();
                localePeriod.country = "DE";
                localePeriod.datCountryIndicator = "DE";
                localePeriod.language = "de";

                de.dat.gold.subTypeVariantNumberSelectionRequest vsPeriod = new de.dat.gold.subTypeVariantNumberSelectionRequest();
                vsPeriod.sessionID = sessionID;
                vsPeriod.restriction = de.dat.gold.releaseRestriction.APPRAISAL;
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