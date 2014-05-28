using System;
using System.IO;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using WebTools.Services;

namespace MvcTools.Controllers
{
    public class SharedController : Controller
    {
        public ActionResult Images(string id)
        {
            var assembly = Assembly.GetExecutingAssembly();
            var imageName = id.Replace("_", ".");
            var stream = assembly.GetManifestResourceStream("MvcTools.Images." + imageName);
            if (stream == null) return File(Imaging.Imaging.CreateEmptyHintImage(string.Format("Image '{0}' wurde nicht gefunden!", id), "ff0000", 300, 25), "image/png");
            var bytes = new BinaryReader(stream).ReadBytes((int)stream.Length);

            return File(bytes, "image/png");
        }

        public ActionResult Scripts(string id)
        {
            var assembly = Assembly.GetExecutingAssembly();
            var scriptName = id.Replace("_", ".");
            var stream = assembly.GetManifestResourceStream("MvcTools.Scripts." + scriptName);
            if (stream == null) return new EmptyResult();
            var bytes = new BinaryReader(stream).ReadBytes((int)stream.Length);

            return File(bytes, "text/javascript");
        }

        public ActionResult Styles(string id)
        {
            var assembly = Assembly.GetExecutingAssembly();
            var scriptName = id.Replace("_", ".");
            var stream = assembly.GetManifestResourceStream("MvcTools.Styles." + scriptName);
            if (stream == null) return new EmptyResult();
            var bytes = new BinaryReader(stream).ReadBytes((int)stream.Length);

            return File(bytes, "text/css");
        }

        public ActionResult Captcha()
        {
            var strCaptcha = CaptchaService.GetSessionCaptchaText();

            return File(CaptchaService.CreateCaptchaImage(strCaptcha), "image/png");
        }
    }
}
