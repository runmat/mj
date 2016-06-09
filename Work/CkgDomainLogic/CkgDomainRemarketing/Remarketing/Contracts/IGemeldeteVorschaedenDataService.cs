using System.Collections.Generic;
using CkgDomainLogic.Remarketing.Models;

namespace CkgDomainLogic.Remarketing.Contracts
{
    public interface IGemeldeteVorschaedenDataService : IRemarketingDataService
    {
        List<Schadensmeldung> GetGemeldeteVorschaeden(GemeldeteVorschaedenSelektor selektor);

        string UpdateVorschaden(EditVorschadenModel item);
    }
}
