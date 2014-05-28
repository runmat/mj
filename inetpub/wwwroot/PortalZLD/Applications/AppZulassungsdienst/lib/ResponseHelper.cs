using System;
using System.Web;
using System.Web.UI;

namespace AppZulassungsdienst.lib
{
    /// <summary>
    /// Öffnung eines neuen Browserfensters für die Darstellung eines PDF´s. Aufgerufen von PrintDialog.aspx.cs.
    /// </summary>
    public static class ResponseHelper
    {
        /// <summary>
        /// Redirectaufruf(Javascript-String). 
        /// </summary>
        /// <param name="url">URL der Seite</param>
        /// <param name="target">"_blank"</param>
        /// <param name="windowFeatures">Browserfensteroptionen</param>
        public static void Redirect(string url, string target, string windowFeatures)
        {

            HttpContext context = HttpContext.Current;



            if ((String.IsNullOrEmpty(target) ||

                target.Equals("_self", StringComparison.OrdinalIgnoreCase)) &&

                String.IsNullOrEmpty(windowFeatures))
            {



                context.Response.Redirect(url);

            }

            else
            {

                Page page = (Page)context.Handler;

                if (page == null)
                {

                    throw new InvalidOperationException(

                        "Cannot redirect to new window outside Page context.");

                }

                url = page.ResolveClientUrl(url);



                string script;

                if (!String.IsNullOrEmpty(windowFeatures))
                {

                    script = @"window.open(""{0}"", ""{1}"", ""{2}"");";

                }

                else
                {

                    script = @"window.open(""{0}"", ""{1}"");";

                }



                script = String.Format(script, url, target, windowFeatures);

                ScriptManager.RegisterStartupScript(page,

                    typeof(Page),

                    "Redirect",

                    script,

                    true);

            }

        }

    }
}