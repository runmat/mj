using System.Collections.Generic;
using System.Linq;
using CkgDomainLogic.General.Services;
using CkgDomainLogic.Leasing.Contracts;
using CkgDomainLogic.Leasing.Models;
using SapORM.Contracts;
using SapORM.Models;
using AppModelMappings = CkgDomainLogic.Leasing.Models.AppModelMappings;

namespace CkgDomainLogic.Leasing.Services
{
    public class UnzugelasseneFahrzeugeDataServiceSAP : CkgGeneralDataServiceSAP, IUnzugelasseneFahrzeugeDataService
    {
        public UnzugelasseneFahrzeugeDataServiceSAP(ISapDataService sap)
            : base(sap)
        {
        }

        public List<UnzugelassenesFahrzeug> LoadUnzugelasseneFahrzeuge()
        {
            var sapList = Z_M_UNZUGELASSENE_FZGE_ARVAL.T_DATA.GetExportListWithInitExecute(SAP, "I_AG", LogonContext.KundenNr.ToSapKunnr());

            return AppModelMappings.Z_M_UNZUGELASSENE_FZGE_ARVAL_T_DATA_To_UnzugelassenesFahrzeug.Copy(sapList).ToList();
        }

        public void SaveBemerkung(UnzugelassenesFahrzeug fahrzeug)
        {
            Z_DPM_EQUI_CHANGE_LTEXT_01.Init(SAP, "I_EQUNR, I_LTEXT_EQUI", fahrzeug.EquiNr, fahrzeug.Bemerkung);

            SAP.Execute();
        }
    }
}
