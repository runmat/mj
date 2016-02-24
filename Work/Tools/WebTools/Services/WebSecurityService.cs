using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq.Expressions;
using System.Web.Security;
using GeneralTools.Contracts;
using GeneralTools.Models;
using System.Linq;
using GeneralTools.Services;

namespace WebTools.Services
{
    public class WebSecurityService : ISecurityService
    {
        #region Password Security

        public string EncryptPassword(string password)
        {
            return FormsAuthentication.HashPasswordForStoringInConfigFile(password, "sha1");
        }

        public bool ValidatePasswordResetToken(string token, int tokenExpirationMinutes)
        {
            var clearTextToken = CryptoMd5.Decrypt(token);

            if (!clearTextToken.Contains("~"))
                return false;

            var tokenElements = clearTextToken.Split('~');
            if (tokenElements.Length < 2)
                return false;

            DateTime tokenDateTime;
            if (!DateTime.TryParse(tokenElements[1], CultureInfo.CreateSpecificCulture("de-DE"), DateTimeStyles.None, out tokenDateTime))
                return false;

            return (DateTime.Now - tokenDateTime).TotalMinutes < tokenExpirationMinutes;
        }

        public bool ValidatePassword(string password, IPasswordSecurityRuleDataProvider passwordSecurityRuleDataProvider, ILocalizationService localizationService,
                                    out List<string> localizedValidationErrorMessages, out List<string> localizedPasswordRuleMessages, out int passwordRuleCount)
        {
            localizedValidationErrorMessages = new List<string>();
            localizedPasswordRuleMessages = new List<string>();
            passwordRuleCount = 0;

            if (passwordSecurityRuleDataProvider == null || localizationService == null)
                throw new Exception("WebSecurityService.ValidatePassword needs valid parameters for IPasswordSecurityRuleDataProvider + ILocalizationService");

            var message = localizationService.TranslateResourceKey("PasswordShouldNotBeEmpty");
            localizedPasswordRuleMessages.Add(message);
            if (password.IsNullOrEmpty())
            {
                password = "";
                localizedValidationErrorMessages.Add(message);
            }

            if (passwordSecurityRuleDataProvider.PasswordMinLength > 0)
            {
                passwordRuleCount++;
                message = GetPropertyResourceString(p => p.PasswordMinLength, passwordSecurityRuleDataProvider, localizationService);
                localizedPasswordRuleMessages.Add(message);
                if (password.Length < passwordSecurityRuleDataProvider.PasswordMinLength)
                    localizedValidationErrorMessages.Add(message);
            }

            if (passwordSecurityRuleDataProvider.PasswordMinNumericChars > 0)
            {
                passwordRuleCount++;
                message = GetPropertyResourceString(p => p.PasswordMinNumericChars, passwordSecurityRuleDataProvider, localizationService);
                localizedPasswordRuleMessages.Add(message);
                if (!ContainsAtLeast(password, IsNumericChar, passwordSecurityRuleDataProvider.PasswordMinNumericChars))
                    localizedValidationErrorMessages.Add(message);
            }

            if (passwordSecurityRuleDataProvider.PasswordMinCapitalChars > 0)
            {
                passwordRuleCount++;
                message = GetPropertyResourceString(p => p.PasswordMinCapitalChars, passwordSecurityRuleDataProvider, localizationService);
                localizedPasswordRuleMessages.Add(message);
                if (!ContainsAtLeast(password, IsAlphaCapitalChar, passwordSecurityRuleDataProvider.PasswordMinCapitalChars))
                    localizedValidationErrorMessages.Add(message);
            }

            if (passwordSecurityRuleDataProvider.PasswordMinSpecialChars > 0)
            {
                passwordRuleCount++;
                message = GetPropertyResourceString(p => p.PasswordMinSpecialChars, passwordSecurityRuleDataProvider, localizationService);
                localizedPasswordRuleMessages.Add(message);
                if (!ContainsAtLeast(password, IsSpecialChar, passwordSecurityRuleDataProvider.PasswordMinSpecialChars))
                    localizedValidationErrorMessages.Add(message);
            }
            return localizedValidationErrorMessages.None();
        }

        #endregion


        #region Maintenance Security

        public MaintenanceResult ValidateMaintenance(List<IMaintenanceSecurityRuleDataProvider> maintenanceMessages)
        {
            if (maintenanceMessages == null)
                return new MaintenanceResult();

            var result = new MaintenanceResult
                             {
                                 Messages = maintenanceMessages.Select(message => new MaintenanceMessage
                                     {
                                         Title = message.MaintenanceTitle,
                                         Message = message.MaintenanceText,

                                         IsActive = message.MaintenanceShow,
                                         IsActiveAndLetConfirmMessageAfterLogin = message.MaintenanceShowAndLetConfirmMessageAfterLogin,
                                         LogonDisabledCore = message.MaintenanceLoginDisabled,
                                     }).ToList()
                             };

            return result;
        }

        #endregion


        #region User Security

        public bool ValidateUser(IUserSecurityRuleDataProvider userSecurityRuleProvider, IPasswordSecurityRuleDataProvider passwordSecurityRuleDataProvider, ILocalizationService localizationService, out List<string> localizedValidationErrorMessages)
        {
            localizedValidationErrorMessages = new List<string>();

            if (userSecurityRuleProvider == null)
                return true;

            if (!userSecurityRuleProvider.UserIsApproved)
            {
                localizedValidationErrorMessages.Add(localizationService.TranslateResourceKey("LoginUserNotApproved"));
                return false;
            }

            if (userSecurityRuleProvider.UserIsDisabled)
            {
                localizedValidationErrorMessages.Add(localizationService.TranslateResourceKey("LoginUserDisabled"));
                return false;
            }

            if (userSecurityRuleProvider.UserCountFailedLogins >= passwordSecurityRuleDataProvider.PasswordMaxLoginFailures)
            {
                localizedValidationErrorMessages.Add(localizationService.TranslateResourceKey("PasswordMaxLoginFailures"));
                return false;
            }

            return true;
        }

        #endregion


        #region Helpers

        static bool ContainsAtLeast(string password, Predicate<char> ruleFunc, int minChars)
        {
            if (minChars == 0)
                return true;

            var count = 0;
            password.ToList().ForEach(c => count += ruleFunc(c) ? 1 : 0);

            return count >= minChars;
        }

        static bool IsNumericChar(char c)
        {
            return c >= '0' && c <= '9';
        }

        static bool IsAlphaCapitalChar(char c)
        {
            return (c >= 'A' && c <= 'Z');
        }

        static bool IsSpecialChar(char c)
        {
            return new[] { '@', '*', '~', '&', '!', '%', '$', '§', '^', '°', '*', '(', ')', '[', ']', '{', '}', '“', '#', '+', '=', '/', '?' }.Contains(c);
        }

        static string GetPropertyResourceString<T>(Expression<Func<T, object>> expression, T model, ILocalizationService localizationService)
        {
            var propertyFormatString = GetPropertyFormatString(expression, model, localizationService);
            if (propertyFormatString.IsNullOrEmpty())
                return "";

            var propertyName = expression.GetPropertyName();
            var modelType = model.GetType();
            var propertyValue = modelType.GetProperty(propertyName).GetValue(model, null);

            var translatedText = string.Format(propertyFormatString, propertyValue);
            return translatedText;
        }

        static string GetPropertyFormatString<T>(Expression<Func<T, object>> expression, T model, ILocalizationService localizationService)
        {
            var propertyName = expression.GetPropertyName();
            var modelType = model.GetType();
            var localizeAttribute = modelType.GetProperty(propertyName).GetCustomAttributes(true).OfType<ILocalizedDisplayAttribute>().FirstOrDefault();
            if (localizeAttribute == null)
                return "";

            var resourceID = localizeAttribute.ResourceID;
            var translatedText = localizationService.TranslateResourceKey(resourceID);

            return translatedText;
        }

        #endregion
    }
}
