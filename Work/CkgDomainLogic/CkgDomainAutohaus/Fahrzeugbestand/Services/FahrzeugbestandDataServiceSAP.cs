// ReSharper disable RedundantUsingDirective
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using CkgDomainLogic.DomainCommon.Models;
using CkgDomainLogic.General.Services;
using CkgDomainLogic.Fahrzeugbestand.Contracts;
using CkgDomainLogic.Fahrzeugbestand.Models;
using GeneralTools.Contracts;
using GeneralTools.Models;
using GeneralTools.Resources;
using SapORM.Contracts;
using SapORM.Models;
using AppModelMappings = CkgDomainLogic.Fahrzeugbestand.Models.AppModelMappings;

// ReSharper restore RedundantUsingDirective

namespace CkgDomainLogic.Fahrzeugbestand.Services
{
    public class FahrzeugbestandDataServiceSAP : CkgGeneralDataServiceSAP, IFahrzeugbestandDataService
    {
        public string KundenNr { get { return LogonContext.KundenNr; } }

        public string AuftragGeber { get; set; }

        public string AuftragGeberOderKundenNr { get { return AuftragGeber.IsNotNullOrEmpty() ? AuftragGeber : KundenNr; } }
        

        public FahrzeugbestandDataServiceSAP(ISapDataService sap)
            : base(sap)
        {
        }

        public List<FahrzeugAkteBestand> GetFahrzeugeAkteBestand(FahrzeugAkteBestandSelektor model)
        {
            return AppModelMappings.Z_AHP_READ_FZGBESTAND_GT_WEBOUT_To_FahrzeugAkteBestand.Copy(GetSapFahrzeugeAkteBestand(model)).ToList();
        }

        private IEnumerable<Z_AHP_READ_FZGBESTAND.GT_WEBOUT> GetSapFahrzeugeAkteBestand(FahrzeugAkteBestandSelektor model)
        {
            Z_DPM_READ_SENDTAB_03.Init(SAP);

            SAP.SetImportParameter("I_KUNNR", LogonContext.KundenNr.ToSapKunnr());
                
            //SAP.SetImportParameter("I_CHECK_TRACK", "X");

            SAP.Execute();

            return Z_AHP_READ_FZGBESTAND.GT_WEBOUT.GetExportList(SAP);
        }
    }
}
