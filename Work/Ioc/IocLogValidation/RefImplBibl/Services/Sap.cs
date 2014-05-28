using System.Collections.Generic;
using System.Linq;
using Autofac.Extras.DynamicProxy2;
using RefImplBibl.Interfaces;
using RefImplBibl.Logging;
using RefImplBibl.Models;

namespace RefImplBibl.Services
{
    /// <summary>
    /// Klasse stellt SAP Aufrufe dar
    /// </summary>
    [Intercept(typeof(SapInterceptor))]
    public class Sap : ISap
    {
        public IQueryable<EQUI> Z_DPM_EQUI_GET()
        {
            return new List<EQUI>{ new EQUI{ EQUI_NR = 10, NAME = "EQUI" }}.AsQueryable();
        }

        public int Z_DPM_INSERT_EQUI(EQUI equi)
        {
            return 0;
        }

        public int Z_DPM_UPDATE_EQUI(EQUI equi)
        {
            return 0;
        }
    }
}