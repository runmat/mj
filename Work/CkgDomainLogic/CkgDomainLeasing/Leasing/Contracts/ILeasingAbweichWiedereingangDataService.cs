using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using CkgDomainLogic.General.Contracts;
using CkgDomainLogic.Leasing.Models.DataModels;
using CkgDomainLogic.Leasing.Models.UIModels;

namespace CkgDomainLogic.Leasing.Contracts
{
    public interface ILeasingAbweichWiedereingangDataService
    {
       List<AbweichungWiedereingang> LoadWiedereingaengeFromSap(AbweichWiedereingangSelektor selektor);

    }
}
