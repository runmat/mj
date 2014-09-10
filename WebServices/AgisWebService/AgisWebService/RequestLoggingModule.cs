using System;
using System.IO;
using System.Text;
using System.Web;

namespace AgisWebService
{
    public class RequestLoggingModule : IHttpModule
    {
        public void Init(HttpApplication context)
        {
        }

        public void Dispose()
        {
        }

        protected void App_BeginRequest(object sender, EventArgs e)
        {
        }

        protected void App_EndRequest(object sender, EventArgs e)
        {

        }


    }
}