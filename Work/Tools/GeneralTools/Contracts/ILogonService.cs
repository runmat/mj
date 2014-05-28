using System.Collections.Generic;
using GeneralTools.Models;

namespace GeneralTools.Contracts
{
    public interface ISecurityService
    {
        string EncryptPassword(string password);

        string GenerateToken(string content);

        bool ValidatePassword(string password, IPasswordSecurityRuleDataProvider passwordSecurityRuleDataProvider, ILocalizationService localizationService, out List<string> localizedValidationErrorMessages, out int passwordRuleCount);

        bool ValidatePasswordResetToken(string token, int tokenExpirationMinutes);

        MaintenanceResult ValidateMaintenance(List<IMaintenanceSecurityRuleDataProvider> maintenanceMessages);

        bool ValidateUser(IUserSecurityRuleDataProvider userSecurityRuleProvider, IPasswordSecurityRuleDataProvider passwordSecurityRuleDataProvider, ILocalizationService localizationService, out List<string> localizedValidationErrorMessages);
    }
}