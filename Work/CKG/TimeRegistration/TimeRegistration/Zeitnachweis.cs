using System;
using KBSBase;

namespace TimeRegistration
{
    public class Zeitnachweis : ErrorHandlingClass
    {
        public byte[] GetPdf(PDFPrintObj PDFPrint)
        {
            ClearErrorState();

            try
            {
                S.AP.Init("Z_HR_ZE_GET_POSTINGS_AS_PDF");

                S.AP.SetImportParameter("BD_NR", PDFPrint.Kartennummer);
                S.AP.SetImportParameter("VDATE", PDFPrint.VDate);
                S.AP.SetImportParameter("BDATE", PDFPrint.BDate);
                S.AP.SetImportParameter("MODUS", "M");

                S.AP.Execute();

                return S.AP.GetExportParameterByte("E_PDF");
            }
            catch (Exception ex)
            {
                RaiseError("9999", ex.Message);
                return null;
            }
        }
    }    
}
