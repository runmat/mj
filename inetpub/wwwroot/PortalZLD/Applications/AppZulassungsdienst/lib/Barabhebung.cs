using System.Collections.Generic;
using AppZulassungsdienst.lib.Models;
using CKG.Base.Business;
using SapORM.Models;

namespace AppZulassungsdienst.lib
{
    public class clsBarabhebung : SapOrmBusinessBase
    {
        public Barabhebung Barabhebung { get; private set; }

        public byte[] PDFXString { get; private set; }

        public clsBarabhebung(string userReferenz)
        {
            Barabhebung = new Barabhebung
            {
                VkBur = ZLDCommon.GetVkBurFromUserReference(userReferenz),
                Waehrung = "EUR"
            };
        }

        public void Save() 
        {
            ExecuteSapZugriff(() =>
                {
                    Z_ZLD_BARABHEBUNG.Init(SAP);

                    SAP.ApplyImport(AppModelMappings.Z_ZLD_BARABHEBUNG_IS_BARABHEBUNG_From_Barabhebung.CopyBack(new List<Barabhebung> { Barabhebung }));

                    CallBapi();

                    PDFXString = SAP.GetExportParameterByte("E_PDF");
                });
        }
    }
}
