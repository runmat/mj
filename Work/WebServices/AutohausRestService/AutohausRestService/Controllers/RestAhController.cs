using System;
using System.Collections.Generic;
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

        //// GET: /RestAh
        //[HttpGet]
        //[RestAuthorize]
        //public Datensatz GetDummyDatensatz()
        //{
        //    return new Datensatz
        //        {
        //            Partnerdaten = new Partner
        //            {
        //                Bemerkung = "test1",
        //                Email = "a@b.c",
        //                Fax = "1212121212",
        //                Gewerblich = false,
        //                HausNr = "10a",
        //                KundendatenSpeichern = true,
        //                Land = "DE",
        //                Name1 = "Alfons",
        //                Name2 = "Ahrens",
        //                Ort = "Ahrensburg",
        //                Partnerrolle = "1",
        //                Plz = "22926",
        //                Referenz1 = "1111",
        //                Referenz2 = "2222",
        //                Strasse = "Holzweg",
        //                Telefon = "13131313"
        //            },
        //            Fahrzeuge = new List<Fahrzeug>
        //                {
        //                    new Fahrzeug
        //                    {
        //                        Abmeldedatum = new DateTime(2015, 1, 20),
        //                        AktZulassung = new DateTime(2015, 1, 23),
        //                        AuftragsNr = "4711",
        //                        Briefbestand = "0",
        //                        BriefNr = "B1234567",
        //                        CocVorhanden = true,
        //                        Erstzulassung = new DateTime(2015, 1, 22),
        //                        FahrgestellNr = "WVW12345678901234",
        //                        Fahrzeugart = "PKW",
        //                        FahrzeugNr = "444",
        //                        Firmenreferenz1 = "1000",
        //                        Firmenreferenz2 = "bla",
        //                        HerstellerSchluessel = "0049",
        //                        Kennzeichen = "OD-WW123",
        //                        Kostenstelle = "123456",
        //                        Lagerort = "DAD",
        //                        Standort = "1234",
        //                        TypSchluessel = "345",
        //                        Verkaufssparte = "10",
        //                        VvsPruefziffer = "1",
        //                        VvsSchluessel = "54321"
        //                    }
        //                }
        //        };
        //}

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
