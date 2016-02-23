using System;
using System.Collections.Generic;
using System.Linq;
using AppZulassungsdienst.lib.Models;
using CKG.Base.Business;
using GeneralTools.Models;
using SapORM.Contracts;
using SapORM.Models;
using Telerik.Web.UI;

namespace AppZulassungsdienst.lib
{
    public class DruckKennzeichenetiketten : SapOrmBusinessBase
    {
        #region "Properties"

        public string SapId { get; set; }
        public string Zulassungsdatum { get; set; }
        public string Kennzeichen { get; set; }
        public int DruckAbZeile { get; set; }
        public int DruckAbSpalte { get; set; }
        public bool Deltaliste { get; set; }

        public List<Kennzeichenetikett> Vorgaenge { get; private set; }

        public byte[] PDFXString { get; private set; }

        #endregion

        #region "Methods"

        public DruckKennzeichenetiketten(string userReferenz)
        {
            VKORG = ZLDCommon.GetVkOrgFromUserReference(userReferenz);
            VKBUR = ZLDCommon.GetVkBurFromUserReference(userReferenz);
        }

        public void LoadVorgaenge()
        {
            ExecuteSapZugriff(() =>
                {
                    Z_ZLD_AH_2015_ETIKETT_SEL.Init(SAP);

                    SAP.SetImportParameter("I_VKBUR", VKBUR);
                    SAP.SetImportParameter("I_ZLDAT", Zulassungsdatum);
                    
                    if (!String.IsNullOrEmpty(SapId))
                        SAP.SetImportParameter("I_ID", SapId.PadLeft0(10));

                    if (!String.IsNullOrEmpty(Kennzeichen))
                        SAP.SetImportParameter("I_KENNZ", Kennzeichen);

                    SAP.SetImportParameter("I_DELTA_LISTE", Deltaliste.BoolToX());

                    CallBapi();

                    Vorgaenge = AppModelMappings.Z_ZLD_AH_2015_ETIKETT_SEL_ET_BAK_To_Kennzeichenetikett.Copy(Z_ZLD_AH_2015_ETIKETT_SEL.ET_BAK.GetExportList(SAP)).ToList();

                    Vorgaenge.ForEach(v => v.IsSelected = true);
                });
        }

        public void SelectVorgang(string sapId, bool isSelected)
        {
            var vg = Vorgaenge.FirstOrDefault(v => v.SapId == sapId);
            if (vg != null)
                vg.IsSelected = isSelected;
        }

        public void PrintEtiketten(GridSortExpression sortierung)
        {
            ExecuteSapZugriff(() =>
                {
                    Z_ZLD_AH_2015_ETIKETT_DRU.Init(SAP);

                    SAP.SetImportParameter("I_DRU_POS_ZEILE", DruckAbZeile);
                    SAP.SetImportParameter("I_DRU_POS_SPALTE", DruckAbSpalte);
                    SAP.SetImportParameter("I_DRUCK_KENNZEICHNEN", "X");
                    SAP.SetImportParameter("I_COMMIT", "X");

                    var sortProperty = typeof (Kennzeichenetikett).GetProperty(sortierung.FieldName);

                    var vorgListe = (sortierung.SortOrder == GridSortOrder.Ascending ? Vorgaenge.Where(v => v.IsSelected).OrderBy(v => sortProperty.GetValue(v, null)) : Vorgaenge.Where(v => v.IsSelected).OrderByDescending(v => sortProperty.GetValue(v, null)));

                    var sapList = AppModelMappings.Z_ZLD_AH_2015_ETIKETT_DRU_IT_BELN_From_Kennzeichenetikett.CopyBack(vorgListe);

                    SAP.ApplyImport(sapList);

                    CallBapi();

                    PDFXString = SAP.GetExportParameterByte("E_PDF");
                });
        }

        #endregion
    }
}