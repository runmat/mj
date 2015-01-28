using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Web.Http.ModelBinding;
using AutohausRestService.Models;

namespace AutohausRestService.Contracts
{
    public interface IRestAhDataService
    {
        List<string> SaveDatensatz(Datensatz daten, AuthenticationHeaderValue authHeader, ModelStateDictionary modelState);
    }
}