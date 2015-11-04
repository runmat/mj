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
    public class LeasingUnzugelFzgDataServiceSAP : CkgGeneralDataServiceSAP, ILeasingUnzugelFzgDataService
    {
        public ZB1KopieSuchparameter Suchparameter { get; set; }
        public List<UnzugelFzg> UnzugelFzge { get { return PropertyCacheGet(() => LoadUnzugelFzgeFromSap().ToList()); } }

        public LeasingUnzugelFzgDataServiceSAP(ISapDataService sap)
            : base(sap)
        {
        }

        public void MarkForRefreshUnzugelFzge()
        {
            PropertyCacheClear(this, m => m.UnzugelFzge);
        }

        private IEnumerable<UnzugelFzg> LoadUnzugelFzgeFromSap()
        {
            Z_M_Unzugelassene_Fzge_Sixt_L.Init(SAP);
            SAP.SetImportParameter("I_AG", LogonContext.KundenNr.ToSapKunnr());

            var sapList = Z_M_Unzugelassene_Fzge_Sixt_L.T_DATA.GetExportListWithExecute(SAP);

            // ggf. Dummy-Leasingvertragsnummern entfernen
            sapList.ForEach(f => f.ZZLVNR = "");

            return AppModelMappings.Z_M_Unzugelassene_Fzge_Sixt_L_T_DATA_To_UnzugelFzg.Copy(sapList);
        }

        public void SaveBriefLVNummern(List<UnzugelFzg> fzge, string bapiName = "Z_M_EINGABE_LVNUMMER_SIXTLEAS")
        {
            SAP.Init(bapiName);

            foreach (var fzg in fzge)
            {
                SAP.SetImportParameter("LF_EQUNR", fzg.Equipmentnummer);
                SAP.SetImportParameter("LF_LIZNR", fzg.Leasingvertragsnummer);
                SAP.SetImportParameter("LF_KUNNR", LogonContext.KundenNr.ToSapKunnr());

                var intStatus = SAP.GetExportParameterWithExecute("LF_RETURN");
                
                switch (intStatus)
                {
                    case "0":
                        fzg.Status = "Vorgang OK";
                        break;
                    case "5":
                        fzg.Status = "FEHLER: LV-Nr. bereits vorhanden.";
                        break;
                    default:
                        fzg.Status = "WARNUNG: Status unbekannt.";
                        break;
                }
            }
        }
    }
}
