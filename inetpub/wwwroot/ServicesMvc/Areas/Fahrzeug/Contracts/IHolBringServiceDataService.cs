using System.Collections.Generic;
using System.Linq;
using CkgDomainLogic.DomainCommon.Models;
using CkgDomainLogic.General.Contracts;
using CkgDomainLogic.Autohaus.Models;
using SapORM.Models;
using ServicesMvc.Areas.Fahrzeug.Models.HolBringService;

namespace CkgDomainLogic.Fahrzeuge.Contracts
{
    public interface IHolBringServiceDataService : ICkgGeneralDataService 
    {
        List<Domaenenfestwert> GetFahrzeugarten { get; }
        List<Domaenenfestwert> GetAnsprechpartner { get; }

        string GetUsername { get; }
        string GetUserTel { get; }

        IOrderedEnumerable<Z_ZLD_AH_KUNDEN_ZUR_HIERARCHIE.GT_DEB> LoadKundenFromSap();

        List<Z_ZLD_AH_2015_HOLUNDBRING_PDF.IS_DATEN> GenerateSapPdf(List<BapiParameterSet> bapiParameterSets,
                                                                    out byte[] pdfGenerated, out int retCode,
                                                                    out string retMessage);

    }
}
