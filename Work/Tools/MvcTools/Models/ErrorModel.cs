using System;

namespace MvcTools.Models
{
    public class ErrorModel
    {
        public int HttpStatusCode { get; set; }

        public Exception Exception { get; set; }

        public string OriginControllerName { get; set; }
    }
}
