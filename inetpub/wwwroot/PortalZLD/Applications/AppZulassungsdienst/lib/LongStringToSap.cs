using System;
using CKG.Base.Business;
using SapORM.Models;

namespace AppZulassungsdienst.lib
{
    public class LongStringToSap : SapOrmBusinessBase
    {
        public void UpdateString(String Text, String sLTextNr, String sUName)
        {
            ExecuteSapZugriff(() =>
                {
                    Z_BC_LTEXT_UPDATE.Init(SAP);

                    SAP.SetImportParameter("I_LTEXT_NR", sLTextNr);
                    SAP.SetImportParameter("I_STRING", Text);
                    SAP.SetImportParameter("I_UNAME", sUName);

                    CallBapi();
                });
        }

        public void DeleteString(String sLTextNr)
        {
            ExecuteSapZugriff(() =>
                {
                    Z_BC_LTEXT_DELETE.Init(SAP, "I_LTEXT_NR", sLTextNr);

                    CallBapi();
                });
        }

        public string ReadString(String sLTextNr)
        {
            var LText = "";

            ExecuteSapZugriff(() =>
                {
                    Z_BC_LTEXT_READ.Init(SAP, "I_LTEXT_NR", sLTextNr);

                    CallBapi();

                    LText = SAP.GetExportParameter("E_STRING");
                });

            return LText;
        }

        public string InsertString(String Text, String sUName)
        {
            var LTextNr = "";

            ExecuteSapZugriff(() =>
                {
                    Z_BC_LTEXT_INSERT.Init(SAP);

                    SAP.SetImportParameter("I_LTEXT_ID", "UMLT");
                    SAP.SetImportParameter("I_STRING", Text);
                    SAP.SetImportParameter("I_UNAME", sUName);

                    CallBapi();

                    LTextNr = SAP.GetExportParameter("E_LTEXT_NR");
                });

            return LTextNr;
        }
    } 
}
