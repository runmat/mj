using System.Collections.Generic;
using System.Linq;
using CkgDomainLogic.Fahrzeugbestand.Contracts;
using CkgDomainLogic.Fahrzeugbestand.Models;
using CkgDomainLogic.Partner.Services;
using GeneralTools.Models;
using SapORM.Contracts;
using SapORM.Models;
using AppModelMappings = CkgDomainLogic.Fahrzeugbestand.Models.AppModelMappings;

namespace CkgDomainLogic.Fahrzeugbestand.Services
{
    public class FahrzeugAkteBestandDataServiceSAP : PartnerDataServiceSAP, IFahrzeugAkteBestandDataService
    {
        public FahrzeugAkteBestandDataServiceSAP(ISapDataService sap)
            : base(sap)
        {
        }

        public FahrzeugAkteBestand GetTypDaten(string herstellerSchluessel, string typSchluessel, string vvsSchluessel)
        {
            Z_AHP_READ_TYPDAT_BESTAND.Init(SAP);

            SAP.SetImportParameter("I_KUNNR", LogonContext.KundenNr.ToSapKunnr());
            SAP.SetImportParameter("I_ZZHERSTELLER_SCH", herstellerSchluessel);
            SAP.SetImportParameter("I_ZZTYP_SCHL", typSchluessel);
            SAP.SetImportParameter("I_ZZVVS_SCHLUESSEL", vvsSchluessel);

            SAP.Execute();

            var sapList = Z_AHP_READ_TYPDAT_BESTAND.GT_WEB_TYPDATEN.GetExportList(SAP);

            return AppModelMappings.Z_AHP_READ_TYPDAT_BESTAND_GT_TYPDATEN_To_FahrzeugAkteBestand.Copy(sapList).FirstOrDefault();
        }

        public List<FahrzeugAkteBestand> GetFahrzeuge(FahrzeugAkteBestandSelektor model)
        {
            Z_AHP_READ_FZGBESTAND.Init(SAP);

            SAP.SetImportParameter("I_KUNNR", LogonContext.KundenNr.ToSapKunnr());
            SAP.SetImportParameter("I_FIN", model.FIN);
            SAP.SetImportParameter("I_FIN_ID", model.FinId);
            SAP.SetImportParameter("I_HALTER", model.Halter);
            SAP.SetImportParameter("I_KAEUFER", model.Kaeufer);
            SAP.SetImportParameter("I_BRIEFBESTAND", model.BriefbestandsInfo);
            SAP.SetImportParameter("I_LGORT", model.BriefLagerort);
            SAP.SetImportParameter("I_STANDORT", model.FahrzeugStandort);
            SAP.SetImportParameter("I_ERSTZULDAT", model.ErstZulassungsgDatum);
            SAP.SetImportParameter("I_AKTZULDAT", model.ZulassungsgDatumAktuell);
            SAP.SetImportParameter("I_ABMDAT", model.AbmeldeDatum);
            SAP.SetImportParameter("I_KENNZ", model.Kennzeichen);
            SAP.SetImportParameter("I_BRIEFNR", model.Briefnummer);
            SAP.SetImportParameter("I_COCVORHANDEN", model.CocVorhanden.BoolToX());
            SAP.SetImportParameter("I_KD_REF", model.KundenReferenz);

            SAP.Execute();

            var sapList = Z_AHP_READ_FZGBESTAND.GT_WEBOUT.GetExportList(SAP);

            return AppModelMappings.Z_AHP_READ_FZGBESTAND_GT_WEBOUT_To_FahrzeugAkteBestand.Copy(sapList).ToList();
        }

        public string SaveFahrzeuge(IEnumerable<FahrzeugAkteBestand> fahrzeuge)
        {
            var error = SAP.ExecuteAndCatchErrors(

                // exception safe SAP action:
                () =>
                {
                    Z_AHP_CRE_CHG_FZG_AKT_BEST.Init(SAP);

                    SAP.SetImportParameter("I_KUNNR", LogonContext.KundenNr.ToSapKunnr());
                    SAP.SetImportParameter("I_USER", LogonContext.UserName);

                    var fzgList = AppModelMappings.Z_AHP_CRE_CHG_FZG_AKT_BEST_GT_WEB_IMP_To_FahrzeugAkteBestand.CopyBack(fahrzeuge);

                    SAP.ApplyImport(fzgList);

                    SAP.Execute();
                },

                // SAP custom error handling:
                () =>
                {
                    var sapResultList = Z_AHP_CRE_CHG_FZG_AKT_BEST.GT_OUT_ERR.GetExportList(SAP);

                    if (sapResultList.Any())
                    {
                        var erg = "Fehler, folgende Fahrzeuge konnten nicht gespeichert werden: ";

                        sapResultList.ForEach(sapResult => erg += string.Format(" FIN {0}, Fin-ID {1},", sapResult.FIN, sapResult.FIN_ID));

                        return erg.TrimEnd(',');
                    }

                    return "";
                });

            return error;
        }
    }
}
