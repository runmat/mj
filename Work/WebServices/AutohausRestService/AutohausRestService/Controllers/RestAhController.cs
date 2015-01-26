using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using AutohausRestService.Models;

namespace AutohausRestService.Controllers
{
    public class RestAhController : ApiController
    {
        private static readonly List<Datensatz> Datensaetze = new List<Datensatz>
        {
            new Datensatz
            {
                Kaeufer = new Kaeufer { ID = 1, Name1 = "Alfons", Name2 = "Albrecht" },
                Fahrzeuge = new List<Fahrzeug>
                {
                    new Fahrzeug { ID = 1, FahrgestellNr = "WWW12345678900001", BriefNr = "11111111", Kennzeichen = "HH-XY123" },
                    new Fahrzeug { ID = 2, FahrgestellNr = "WWW12345678900002", BriefNr = "22222222", Kennzeichen = "OD-UU555" },
                    new Fahrzeug { ID = 3, FahrgestellNr = "WWW12345678900003", BriefNr = "33333333", Kennzeichen = "RZ-WW22" }
                }
            }
        };

        // GET: /RestAh
        [HttpGet]
        [RestAuthorize]
        public IEnumerable<Datensatz> GetAllDatensaetze()
        {
            return Datensaetze;
        }

        // POST: /RestAh
        [HttpPost]
        [RestAuthorize]
        public HttpResponseMessage PostDatensatz(Datensatz ds)
        {
            if (!ModelState.IsValid)
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);

            Datensaetze.Add(ds);

            return Request.CreateResponse(HttpStatusCode.Created, ds);
        }

        // PUT: /RestAh
        [HttpPut]
        [RestAuthorize]
        public HttpResponseMessage PutDatensatz(Datensatz ds)
        {
            if (!ModelState.IsValid)
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);

            var existingDs = Datensaetze.FirstOrDefault(d => d.ID == ds.ID);
            if (existingDs == null)
                return Request.CreateResponse(HttpStatusCode.NotFound);

            Datensaetze.Remove(existingDs);
            Datensaetze.Add(ds);

            return Request.CreateResponse(HttpStatusCode.OK, ds);
        }

        // DELETE: /RestAh
        [HttpDelete]
        [RestAuthorize]
        public HttpResponseMessage DeleteDatensatz(int id)
        {
            var ds = Datensaetze.FirstOrDefault(d => d.ID == id);
            if (ds == null)
                return Request.CreateResponse(HttpStatusCode.NotFound);

            Datensaetze.Remove(ds);

            return Request.CreateResponse(HttpStatusCode.NoContent);
        }
    }
}
