using GeneralTools.Models;

namespace CkgDomainLogic.Fahrer.Contracts
{
    public interface IFahrerAuftragsFahrt
    {
        string AuftragsNr { get; set; }

        string AuftragsNrFriendly { get; }

        [SelectListKey]
        string UniqueKey { get; }

        string Fahrt { get; }

        [SelectListText]
        string AuftragsDetails { get; }

        bool IstSonstigerAuftrag { get; set; }

        string ProtokollName { get; set; }
    }
}
