using System;
using System.Text;
using System.Web.Services;
using NReco.PdfGenerator;

namespace HtmlToPdfService
{
    [WebService(Namespace = "http://kroschke.de/")]
    public class ServiceData : WebService
    {
        [WebMethod]
        public string GetPdf(string htmlBase64String, float zoomFaktor = 1, bool orientationLandscape = false)
        {
            try
            {
                var strHtml = Encoding.Default.GetString(Convert.FromBase64String(htmlBase64String));

                var pdfConv = new HtmlToPdfConverter
                {
                    Size = PageSize.A4,
                    Zoom = zoomFaktor,
                    ExecutionTimeout = TimeSpan.FromMinutes(1),
                    Orientation = (orientationLandscape ? PageOrientation.Landscape : PageOrientation.Portrait)
                };

                var pdfBytes = pdfConv.GeneratePdf(strHtml);

                return Convert.ToBase64String(pdfBytes);
            }
            catch (Exception ex)
            {
                throw new Exception("GetPdf, Fehler :  " + ex.Message);
            }
        }
    }
}
