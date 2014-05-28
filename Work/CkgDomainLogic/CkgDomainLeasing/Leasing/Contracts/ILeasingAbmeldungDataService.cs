using System;
using System.Collections.Generic;
using CkgDomainLogic.General.Contracts;
using CkgDomainLogic.Leasing.Models;

namespace CkgDomainLogic.Leasing.Contracts
{
    public interface ILeasingAbmeldungDataService : ICkgGeneralDataService
    {
        List<Abmeldedaten> AbzumeldendeFzge { get; }

        void MarkForRefreshAbzumeldendeFzge();

        List<Abmeldedaten> SaveAbmeldungen(List<Abmeldedaten> fzge);
    }
}
