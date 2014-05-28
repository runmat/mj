using System.Web.Mvc;

namespace RefImpl.Controllers
{
    public class SimpleController : Controller
    {
        [HttpGet]
        public ActionResult Index()
        {
            return new ContentResult
                {
                    Content = "Ich war beim Index"
                };
        }

        [HttpGet]
        public ActionResult GetMitId(string id)
        {
            return new ContentResult
                {
                    Content = string.Format("Die Id ist {0}", id)
                };
        }

        [HttpGet]
        public ActionResult GetView(string id)
        {
            return new ContentResult
                {
                    Content = string.Format(@"<!DOCTYPE html><html><head><title>{0}</title></head><body><div></div></body></html>", id)
                };
        }
    }
}
