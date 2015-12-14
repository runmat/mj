using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using CkgDomainLogic.Autohaus.Contracts;
using CkgDomainLogic.DataKonverter.Contracts;
using CkgDomainLogic.General.Services;
using DocumentTools.Services;
using GeneralTools.Models;
using Microsoft.VisualBasic.FileIO;
using SapORM.Contracts;
using ServicesMvc.Areas.DataKonverter.Models;
using LumenWorks.Framework.IO.Csv;
using FieldType = ServicesMvc.Areas.DataKonverter.Models.FieldType;

namespace CkgDomainLogic.DataKonverter.Services
{
    public class DataKonverterDataService : CkgGeneralDataServiceSAP, IDataKonverterDataService
    {
        public DataKonverterDataService(ISapDataService sap)
            : base(sap)
        {
        }

    }
}