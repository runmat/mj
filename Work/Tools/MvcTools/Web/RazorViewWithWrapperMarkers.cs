using System.Collections.Generic;
using System.IO;
using System.Web.Mvc;

namespace MvcTools.Web
{
    public class RazorViewWithWrapperMarkers : RazorView
    {
        public RazorViewWithWrapperMarkers(ControllerContext controllerContext, string viewPath, string layoutPath, bool runViewStartPages, IEnumerable<string> viewStartFileExtensions)
            : this(controllerContext, viewPath, layoutPath, runViewStartPages, viewStartFileExtensions, null)
        {
        }

        public RazorViewWithWrapperMarkers(ControllerContext controllerContext, string viewPath, string layoutPath, bool runViewStartPages, IEnumerable<string> viewStartFileExtensions, IViewPageActivator viewPageActivator)
            : base(controllerContext, viewPath, layoutPath, runViewStartPages, viewStartFileExtensions, viewPageActivator)
        {
        }

        protected override void RenderView(ViewContext viewContext, TextWriter writer, object instance)
        {
            writer.Write("\r\n\r\n<!-- PVM BEGIN " + this.ViewPath + " -->\r\n\r\n");
            base.RenderView(viewContext, writer, instance);
            writer.Write("\r\n\r\n<!-- PVM END " + this.ViewPath + " -->\r\n\r\n");
        }
    }
}
