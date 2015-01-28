using System.Net;
using System.Net.Http;
using System.Web.Http;
using AutohausRestService.Contracts;
using AutohausRestService.Models;
using AutohausRestService.Services;

namespace AutohausRestService.Controllers
{
    public class RestAhController : ApiController
    {
        private IRestAhDataService DataService;

        public RestAhController()
        {
            DataService = new RestAhDataServiceSap();
        }

        // POST: /RestAh
        [HttpPost]
        [RestAuthorize]
        public HttpResponseMessage PostDatensatz(Datensatz ds)
        {
            if (!ModelState.IsValid)
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);

            var linkList = DataService.SaveDatensatz(ds, Request.Headers.Authorization, ModelState);

            if (!ModelState.IsValid)
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ModelState);

            return Request.CreateResponse(HttpStatusCode.Created, linkList);
        }
    }
}
