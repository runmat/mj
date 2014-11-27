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
    public class FahrzeugAkteBestandDataServiceSAP : CkgGeneralDataServiceSAP, IFahrzeugAkteBestandDataService
    {
        public string KundenNr { get { return LogonContext.KundenNr; } }

        public string AuftragGeber { get; set; }

        public string AuftragGeberOderKundenNr { get { return AuftragGeber.IsNotNullOrEmpty() ? AuftragGeber : KundenNr; } }
        

        public FahrzeugAkteBestandDataServiceSAP(ISapDataService sap)
            : base(sap)
        {
        }

        #region Load Fahrzeug-Akte-Bestand

        public List<FahrzeugAkteBestand> GetFahrzeugeAkteBestand(FahrzeugAkteBestandSelektor model)
        {
            return
                AppModelMappings.Z_AHP_READ_FZGBESTAND_GT_WEBOUT_To_FahrzeugAkteBestand.Copy(
                    GetSapFahrzeugeAkteBestand(model)).ToList();
        }

        private IEnumerable<Z_AHP_READ_FZGBESTAND.GT_WEBOUT> GetSapFahrzeugeAkteBestand(
            FahrzeugAkteBestandSelektor model)
        {
            Z_AHP_READ_FZGBESTAND.Init(SAP);

            SAP.SetImportParameter("I_KUNNR", LogonContext.KundenNr.ToSapKunnr());
            if (model.FIN.IsNotNullOrEmpty())
                SAP.SetImportParameter("I_FIN", model.FIN);

            SAP.Execute();

            return Z_AHP_READ_FZGBESTAND.GT_WEBOUT.GetExportList(SAP);
        }

        #endregion


        public string SaveFahrzeugAkteBestand(FahrzeugAkteBestand fahrzeugAkteBestand)
        {
            var error = SAP.ExecuteAndCatchErrors(

                // exception safe SAP action:
                () =>
                {
                    Z_AHP_CRE_CHG_FZG_AKT_BEST.Init(SAP);
                    SAP.SetImportParameter("I_KUNNR", LogonContext.KundenNr.ToSapKunnr());
                    SAP.SetImportParameter("I_USER", LogonContext.UserName);

                    var fzgList = Z_AHP_CRE_CHG_FZG_AKT_BEST.GT_WEB_IMP.GetImportList(SAP);

                    CreateRowForFahrzeug(fzgList, fahrzeugAkteBestand);

                    SAP.ApplyImport(fzgList);

                    SAP.Execute();
                },

                // SAP custom error handling:
                () =>
                {
                    var sapResultList = Z_AHP_CRE_CHG_FZG_AKT_BEST.GT_OUT_ERR.GetExportList(SAP);

                    var sapResult = sapResultList.FirstOrDefault();
                    if (sapResult != null)
                        return string.Format("Fehler, folgendes Fahrzeug konnte nicht gespeichert werden: FIN {0}, Fin-ID {1}", sapResult.FIN, sapResult.FIN_ID);

                    return "";
                });

            return error;
        }

        private static void CreateRowForFahrzeug(List<Z_AHP_CRE_CHG_FZG_AKT_BEST.GT_WEB_IMP> fzgList, FahrzeugAkteBestand fahrzeugAkteBestand)
        {
            var sapFahrzeug = new Z_AHP_CRE_CHG_FZG_AKT_BEST.GT_WEB_IMP
            {
                FIN_ID = fahrzeugAkteBestand.FinID,
                FIN = fahrzeugAkteBestand.FIN,

                // Fahrzeug Akte
                ZZFABRIKNAME = fahrzeugAkteBestand.FabrikName,

                // Fahrzeug Bestand
                KAEUFER = fahrzeugAkteBestand.Kaeufer,
                HALTER = fahrzeugAkteBestand.Halter,
            };

            fzgList.Add(sapFahrzeug);
        }
    }
}
