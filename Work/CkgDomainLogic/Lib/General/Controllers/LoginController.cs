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

        public LoginViewModel ViewModel { get { return GetViewModel<LoginViewModel>(); }  }

        protected override bool NeedsAuhentification { get { return false; } }


        public LoginController(IAppSettings appSettings, ILogonContextDataService logonContext, ISecurityService securityService, ILocalizationService localizationService)
            : base(appSettings, logonContext)
        {
            InitViewModel(ViewModel, appSettings, logonContext, securityService, localizationService);
        }

        public ActionResult Index()
        {
            if (LogonContext != null)
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
                    ViewModel.TryLogonUser(model, ModelState.AddModelError);
                else
                    userEmail = ViewModel.TryGetEmailAddressFromUsername(model, ModelState.AddModelError);

            if (ModelState.IsValid)
            {
                model.MaintenanceInfo = ViewModel.MaintenanceInfo;

                if (!model.ModePasswordReset)
                    // Login successfull:
                    LogonContext = ViewModel.LogonContext;
                else
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
                    if (userEmail.IsNotNullOrEmpty())
                        ViewModel.TrySendPasswordResetEmail(storedUserName, userEmail, Request.Url.ToString(), ModelState.AddModelError);

                    if (ModelState.IsValid)
                    {
                        model.EmailForPasswordReset = userEmail;
                        SetViewModel<LoginViewModel>(null);
                    }
                }
            }

            model.IsValid = ModelState.IsValid;

            return PartialView(model);
        }

        public ActionResult ChangePassword(string confirmation)
        {
            ViewModel.ChangePasswordModel = new ChangePasswordModel { ModePasswordReset = true, PasswordCurrent = "dummy" };
            
            if (!ViewModel.CacheUserAndCustomerFromConfirmationToken(confirmation))
                return View("ChangePasswordError", ViewModel);

            return View(ViewModel);
        }

        public ActionResult ChangePasswordLoggedOn()
        {
            if (LogonContext.UserName.IsNullOrEmpty())
                return new EmptyResult();

            if (!ViewModel.CacheUserAndCustomerFromUserName(LogonContext.UserName))
                return View("ChangePasswordError", ViewModel);

            return View(ViewModel);
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

            if (ModelState.IsValid)
                ViewModel.ValidatePasswordModelAgainstRules(ModelState.AddModelError);

            if (ModelState.IsValid)
            {
                // change password successful!
                var encryptedPassword = LogonContext.SecurityService.EncryptPassword(model.Password);
                    
                LogonContext.StorePasswordToUser(model.UserName, encryptedPassword);
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

            ViewModel.ValidatePasswordAgainstRules(password, out localizedPasswordValidationErrorMessages, out localizedPasswordRuleMessages);

            return Json(new
                            {
                                passwordRuleCount = ViewModel.PasswordRuleCount, 
                                localizedPasswordValidationErrorMessages,
                                localizedPasswordRuleMessages
                            });
        }
    }
}
