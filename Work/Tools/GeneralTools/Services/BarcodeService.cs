using System;
using System.Drawing.Imaging;
using System.IO;
using BarcodeLib;

namespace GeneralTools.Services
{
    public class BarcodeService
    {
        public static MemoryStream CreateBarcode(string sCode)
        {
            try
            {
                var BarCode = new Barcode();
                var w = sCode.Length * 150;

                var oImage = BarCode.Encode(TYPE.CODE39, sCode, w, 75);

                var ms = new MemoryStream();
                oImage.Save(ms, ImageFormat.Jpeg);
                oImage.Dispose();

                return ms;
            }
            catch (Exception ex)
            {
                throw new Exception("Fehler beim generieren des Barcodes: " + ex.Message);
            }
        }

        public static bool CheckBarcodeEan(string barcode)
        {
            var summe = 0;
            var multiplikator = 1;

            for (var i = barcode.Length - 1; i >= 0; i--)
            {
                summe += multiplikator * (barcode[i] - '0');
                multiplikator = (multiplikator == 3 ? 1 : 3);
            }

            return ((summe % 10) == 0);
        }
    }
}
