using System;
using System.Collections.Generic;
using CkgDomainLogic.CoC.Models;
using CkgDomainLogic.General.Contracts;

namespace CkgDomainLogic.CoC.Contracts
{
    public interface ICocTypenDataService : ICkgGeneralDataService 
    {
        List<CocEntity> CocTypen { get; }

        void CocTypenMarkForRefresh();

        CocEntity SaveCocTyp(CocEntity cocTyp, Action<string, string> addModelError);

        void DeleteCocTyp(CocEntity cocTyp);
    }
}
