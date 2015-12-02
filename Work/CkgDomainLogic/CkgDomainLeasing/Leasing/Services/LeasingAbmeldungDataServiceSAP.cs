using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using CkgDomainLogic.General.Services;
using CkgDomainLogic.Leasing.Contracts;
using CkgDomainLogic.Leasing.Models;
using SapORM.Contracts;
using SapORM.Models;
using AppModelMappings = CkgDomainLogic.Leasing.Models.AppModelMappings;

namespace CkgDomainLogic.Leasing.Services
{
    public class LeasingAbmeldungDataServiceSAP : CkgGeneralDataServiceSAP, ILeasingAbmeldungDataService
    {
        public ZB1KopieSuchparameter Suchparameter { get; set; }
        public List<Abmeldedaten> AbzumeldendeFzge { get { return PropertyCacheGet(() => LoadAbzumeldendeFzgeFromSap().ToList()); } }

        public LeasingAbmeldungDataServiceSAP(ISapDataService sap)
            : base(sap)
        {
        }

        public void MarkForRefreshAbzumeldendeFzge()
        {
            PropertyCacheClear(this, m => m.AbzumeldendeFzge);
        }

        private IEnumerable<Abmeldedaten> LoadAbzumeldendeFzgeFromSap()
        {
            Z_M_ABM_FEHL_UNTERL_SIXT_LEAS.Init(SAP);
            SAP.SetImportParameter("KUNNR", LogonContext.KundenNr.ToSapKunnr());

            var sapList = Z_M_ABM_FEHL_UNTERL_SIXT_LEAS.AUSGABE.GetExportListWithExecute(SAP);

            return AppModelMappings.Z_M_Abm_Fehl_Unterl_Sixt_Leas_AUSGABE_To_Abmeldedaten.Copy(sapList);
        }

        public List<Abmeldedaten> SaveAbmeldungen(List<Abmeldedaten> fzge)
        {
            List<Abmeldedaten> erg = new List<Abmeldedaten>();

            string fehlertext;

            Z_M_ABMELDUNG_SIXT_LEASING.Init(SAP);
            SAP.SetImportParameter("I_KUNNR_AG", LogonContext.KundenNr.ToSapKunnr());

            var fzgList = AppModelMappings.Z_M_ABMELDUNG_SIXT_LEASING_I_AUF_From_Abmeldedaten.CopyBack(fzge).ToList();
            SAP.ApplyImport(fzgList);

            var sapItems_out = Z_M_ABMELDUNG_SIXT_LEASING.I_AUF.GetExportListWithExecute(SAP);
            var webItems_out = AppModelMappings.Z_M_ABMELDUNG_SIXT_LEASING_I_AUF_To_Abmeldedaten.Copy(sapItems_out).ToList();

            foreach (Abmeldedaten fzg in fzge)
            {
                Abmeldedaten item = webItems_out.Find(f => f.Fahrgestellnummer == fzg.Fahrgestellnummer);

                fehlertext = "";

                if ((item != null) && (!String.IsNullOrEmpty(item.Fehler)))
                {
                    switch (item.Fehler)
                    {
                        case "1":
                            fehlertext = "Fehler: Datenaktualisieung fehlgeschlagen.";
                            break;
                        case "2":
                            fehlertext = "Fehler: Brief bereits angefordert.";
                            break;
                        case "3":
                            fehlertext = "Fehler: Keine Daten gefunden.";
                            break;
                        case "4":
                            fehlertext = "Fehler: Kein Kundenauftrag angelegt.";
                            break;
                        case "5":
                            fehlertext = "Fehler: Brief bereits in Abmeldung.";
                            break;
                        case "6":
                            fehlertext = "Fehler: Datensatz gesperrt.";
                            break;
                        default:
                            fehlertext = "Unbekannter Fehler.";
                            break;
                    }
                }

                if (fehlertext.Length > 0)
                {
                    fzg.Status = fehlertext;
                }
                else
                {
                    fzg.Status = "Vorgang OK.";
                }

                erg.Add(fzg);
            }

            return erg;
        }

    }
}
