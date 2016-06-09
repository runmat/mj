using System.Collections.Generic;
using CkgDomainLogic.Remarketing.Models;

namespace CkgDomainLogic.Remarketing.Contracts
{
    public interface IBelastungsanzeigenDataService : IRemarketingDataService
    {
        List<Belastungsanzeige> GetBelastungsanzeigen(BelastungsanzeigenSelektor selektor);

        List<Gutachten> GetGutachten(string fahrgestellNr);

        string GetReklamationstext(string fahrgestellNr);

        string GetBlockadetext(string fahrgestellNr);

        string SetReklamation(SetReklamationModel item);

        string SetBelastungsanzeigenOffen(List<Belastungsanzeige> items);

        string UpdateBelastungsanzeigen(List<Belastungsanzeige> items);
    }
}
