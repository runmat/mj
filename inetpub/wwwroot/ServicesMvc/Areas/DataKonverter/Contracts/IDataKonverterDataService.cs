using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CkgDomainLogic.General.Contracts;
using ServicesMvc.Areas.DataKonverter.Models;

namespace CkgDomainLogic.DataKonverter.Contracts
{
    public interface IDataKonverterDataService : ICkgGeneralDataService
    {
        SourceFile GetSourceFile(string filename, bool firstRowIsCaption, string delimiter = ";");
    }
}