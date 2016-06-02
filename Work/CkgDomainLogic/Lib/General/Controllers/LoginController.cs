using System.Collections.Generic;
using System.Web.Mvc;
using CkgDomainLogic.General.Services;
using MvcTools.Web;
using CkgDomainLogic.General.Contracts;
using CkgDomainLogic.General.Models;
using CkgDomainLogic.General.ViewModels;
using GeneralTools.Contracts;
using GeneralTools.Models;
using WebTools.Services;

namespace CkgDomainLogic.General.Controllers
{
    public class LoginController : CkgDomainController
    {
        public override sealed string DataContextKey { get { return GetDataContextKey<LoginViewModel>(); } }

        public LoginViewModel ViewModel { get { return GetViewModel<LoginViewModel>(); } }

        protected override bool NeedsAuhentification { get { return false; } }


        static IGeneralConfigurationProvider GeneralConfigurationProvider { get { return DependencyResolver.Current.GetService<IGeneralConfigurationProvider>(); } }



        public LoginController(IAppSettings appSettings, ILogonContextDataService logonContext, ISecurityService securityService, ILocalizationService localizationService)
            : base(appSettings, logonContext)
        {
            InitViewModel(ViewModel, appSettings, logonContext, securityService, localizationService);
        }

        public ActionResult Index()
        {
            if (!string.IsNullOrEmpty(LogonContext.UserName))
                LogonContext.LogoutUser();

            LogonContext.MvcEnforceRawLayout = false;

            CaptchaGenerate();

            return View(ViewModel);
        }

        [CkgApplication]
        public ActionResult Kontakt()
        {
            return View();
        }

        [CkgApplication]
        public ActionResult Impressum()
        {
            return View();
        }

        static void CaptchaGenerate()
        {
            CaptchaService.GetAndSetSessionCaptchaText(5);
        }
        public ActionResult ChangePassword(string confirmation)
        {
            ViewModel.ChangePasswordModel = new ChangePasswordModel { ModePasswordReset = true, PasswordCurrent = "dummy" };

            if (!ViewModel.CacheUserAndCustomerFromConfirmationToken(confirmation))
                return View("ChangePasswordError", ViewModel);

            return View(ViewModel);
        }

        public ActionResult ChangePasswordLoggedOnInternal()
        {
            if (LogonContext.UserName.IsNullOrEmpty())
                return new EmptyResult();

            ViewModel.ResetChangePasswordModel();

            if (!ViewModel.CacheUserAndCustomerFromUserName(LogonContext.UserName))
                return View("ChangePasswordError", ViewModel);

            return View("ChangePasswordLoggedOn", ViewModel);
        }

        [HttpPost]
        public ActionResult ChangePasswordForm(ChangePasswordModel model)
        {
            if (ModelState.IsValid)
            {
                ViewModel.ChangePasswordModel = model;

                if (!model.ModePasswordReset)
                {
                    if (!LogonContext.ValidatePassword(model.PasswordCurrent, LogonContext.User))
                        ModelState.AddModelError<ChangePasswordModel>(m => m.PasswordCurrent, Localize.PasswordCurrentInvalid);
                }
            }

            var passwordSecurityRuleDataProvider = ViewModel.GetPasswordSecurityRuleDataProvider(model.UserName);

            if (ModelState.IsValid)
                ViewModel.ValidatePasswordModelAgainstRules(passwordSecurityRuleDataProvider, ModelState.AddModelError);

            if (ModelState.IsValid)
                ViewModel.ValidatePasswordModelAgainstHistory(passwordSecurityRuleDataProvider, ModelState.AddModelError);

            if (ModelState.IsValid)
            {
                // change password successful!
                var encryptedPassword = LogonContext.SecurityService.EncryptPassword(model.Password);

                LogonContext.StorePasswordToUser(passwordSecurityRuleDataProvider, model.UserName, encryptedPassword);
                if (LogonContext.User != null)
                    LogonContext.User.Password = encryptedPassword;

                LogonContext.ReturnUrl = null;
            }


            model.IsValid = ModelState.IsValid;

            return PartialView(model);
        }

        [HttpPost]
        public ActionResult PasswordPrecheck(string password)
        {
            List<string> localizedPasswordValidationErrorMessages;
            List<string> localizedPasswordRuleMessages;

            var passwordSecurityRuleDataProvider = ViewModel.GetPasswordSecurityRuleDataProvider();

            ViewModel.ValidatePasswordAgainstRules(passwordSecurityRuleDataProvider, password, out localizedPasswordValidationErrorMessages, out localizedPasswordRuleMessages);

            return Json(new
            {
                passwordRuleCount = ViewModel.PasswordRuleCount,
                localizedPasswordValidationErrorMessages,
                localizedPasswordRuleMessages
            });
        }

        [HttpPost]
        public ActionResult ConfirmActiveMessagesDontShowAgain()
        {
            LogonContext.MaintenanceMessageConfirmAndDontShowAgain();

            return new EmptyResult();
        }

        [HttpPost]
        public ActionResult LoginForm(LoginModel model)
        {
            if (model.ModeCaptchaReset)
            {
                CaptchaGenerate();

                ModelState.Clear();
                ViewModel.LoginModel.ModeCaptchaReset = model.ModeCaptchaReset;
                return PartialView(model);
            }

            if (ViewModel.LoginModel.ModePasswordReset != model.ModePasswordReset)
            {
                // only switch between "login mode <=> password reset mode" here, don't perform any validation
                ModelState.Clear();
                ViewModel.LoginModel.ModePasswordReset = model.ModePasswordReset;
                return PartialView(model);
            }

            var userEmail = "";
            if (ModelState.IsValid)
                if (!model.ModePasswordReset)
                {
                    ILogonContextDataService logonContext;
                    ViewModel.TryLogonUser(model, ModelState.AddModelError, out logonContext);
                    ViewModel.Init(null, logonContext);
                }
                else
                    userEmail = ViewModel.TryGetEmailAddressFromUsername(model, ModelState.AddModelError);

            if (ModelState.IsValid)
            {
                model.MaintenanceInfo = ViewModel.MaintenanceInfo;

                if (!model.ModePasswordReset)
                {
                    // Login successfull:
                    LogonContext = ViewModel.LogonContext;

                    var showCustomImage = GeneralConfigurationProvider.GetConfigVal("Login", "DisplayCustomPictureInsteadOfAvatar");
                    if (showCustomImage.ToUpper() == "TRUE")
                        LogonContext.UserNameForDisplay = "Pia Schmidt";                    
                }
                else
                {
                    ViewModel.CheckIfPasswordResetAllowed(model, ModelState.AddModelError);

                    if (ModelState.IsValid)
                    {
                        if (CaptchaService.GetSessionCaptchaText() != model.CaptchaText)
                        {
                            ModelState.AddModelError<LoginModel>(m => m.CaptchaText, Localize.CaptchaResponseInvalid);
                            model.IsValid = ModelState.IsValid;
                            return PartialView(model);
                        }

                        var storedUserName = model.UserName;

                        if (Request.Url == null)
                            return new EmptyResult();

                        // send e-mail with password reset link
                        var passwordResetCustomerAdminInfo =
                            ViewModel.TryGetPasswordResetCustomerAdminInfo(storedUserName);
                        var mailSendValid = passwordResetCustomerAdminInfo.IsNullOrEmpty();
                        if (userEmail.IsNotNullOrEmpty() && mailSendValid)
                            ViewModel.TrySendPasswordResetEmail(storedUserName, userEmail, Request.Url.ToString(),
                                ModelState.AddModelError);

                        if (ModelState.IsValid)
                        {
                            model.PasswordResetCustomerAdminInfo = passwordResetCustomerAdminInfo;
                            model.EmailForPasswordReset = userEmail;
                            SetViewModel<LoginViewModel>(null);
                        }
                    }
                }
            }

            model.IsValid = ModelState.IsValid;

            return PartialView(model);
        }
    }
}


