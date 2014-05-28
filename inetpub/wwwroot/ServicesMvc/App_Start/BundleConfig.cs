using System.Web.Optimization;

namespace ServicesMvc
{
    public class BundleConfig
    {
        // Weitere Informationen zu Bundling finden Sie unter "http://go.microsoft.com/fwlink/?LinkId=254725".
        public static void RegisterBundles(BundleCollection bundles)
        {
        //    //
        //    // Scripts
        //    //
        //    bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
        //                //"~/Scripts/jquery-{version}.js"
        //                ));

        //    bundles.Add(new ScriptBundle("~/bundles/jqueryui").Include(
        //                //"~/Scripts/jquery.selectBox.js",
        //                //"~/Scripts/jquery-ui.js"
        //                //"~/Scripts/jquery.fancybox.pack.js",
        //                //"~/Scripts/jquery.ezmark.js",
        //                //"~/Scripts/jquery.blockUI.js"
        //                ));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.unobtrusive-*",
                        "~/Scripts/jquery.validate.*"
                        ));

        //    // Verwenden Sie die Entwicklungsversion von Modernizr zum Entwickeln und Erweitern Ihrer Kenntnisse. Wenn Sie dann
        //    // für die Produktion bereit sind, verwenden Sie das Buildtool unter "http://modernizr.com", um nur die benötigten Tests auszuwählen.
        //    //bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
        //    //            "~/Scripts/modernizr-*"
        //    //            ));


        //    bundles.Add(new ScriptBundle("~/bundles/site").Include(
        //                //"~/Scripts/site.js"
        //                ));


        //    //
        //    // Styles
        //    //
        //    bundles.Add(new StyleBundle("~/Content/css").Include(
        //                //"~/Content/site.css",
        //                //"~/Content/ezmark.css"
        //                ));

        //    bundles.Add(new StyleBundle("~/Content/jqueryui").Include(
        //        //"~/Content/themes/base/jquery-ui.css"
        //        ));

        //    //bundles.Add(new StyleBundle("~/Content/themes/base/jqueryui").Include(
        //    //            "~/Content/themes/base/jquery.ui.core.css",
        //    //            "~/Content/themes/base/jquery.ui.resizable.css",
        //    //            "~/Content/themes/base/jquery.ui.selectable.css",
        //    //            "~/Content/themes/base/jquery.ui.accordion.css",
        //    //            "~/Content/themes/base/jquery.ui.autocomplete.css",
        //    //            "~/Content/themes/base/jquery.ui.button.css",
        //    //            "~/Content/themes/base/jquery.ui.dialog.css",
        //    //            "~/Content/themes/base/jquery.ui.slider.css",
        //    //            "~/Content/themes/base/jquery.ui.tabs.css",
        //    //            "~/Content/themes/base/jquery.ui.datepicker.css",
        //    //            "~/Content/themes/base/jquery.ui.progressbar.css",
        //    //            "~/Content/themes/base/jquery.ui.theme.css"
        //    //            ));
        }
    }
}