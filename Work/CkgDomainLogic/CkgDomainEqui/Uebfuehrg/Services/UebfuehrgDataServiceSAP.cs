// ReSharper disable RedundantUsingDirective
using System.Collections.Generic;
using System.Linq;
using CkgDomainLogic.General.Services;
using CkgDomainLogic.Uebfuehrg.Contracts;
using CkgDomainLogic.Uebfuehrg.Models;
using GeneralTools.Models;
using SapORM.Contracts;
using SapORM.Models;
using AppModelMappings = CkgDomainLogic.Uebfuehrg.Models.AppModelMappings;
// ReSharper restore RedundantUsingDirective

namespace CkgDomainLogic.Uebfuehrg.Services
{
    public class UebfuehrgDataServiceSAP : CkgGeneralDataServiceSAP, IUebfuehrgDataService
    {
        public UebfuehrgDataServiceSAP(ISapDataService sap)
            :base(sap)
        {
        }
    }
}
