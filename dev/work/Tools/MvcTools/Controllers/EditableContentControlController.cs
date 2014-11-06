using System.Linq;
using System.Web.Mvc;
using MvcTools.Data;
using MvcTools.Models;

namespace MvcTools.Controllers
{
    public class EditableContentController : Controller
    {
        [HttpPost]
        public ActionResult ShowEditableContentEditor(string key)
        {
            var model = MvcRepository.ContentEntities.FirstOrDefault(i => i.ID == key);
            if (model == null)
                return Json("");

            return PartialView("EditableContent_Once_Editor", model);
        }

        [ValidateInput(false)]
        [HttpPost]
        public ActionResult SaveEditableContentText(string key, string text)
        {
            var model = new ContentEntity { ID = key, Text = text };
            model.DbSave();

            return Json(new { key = model.ID });
        }
    }
}
