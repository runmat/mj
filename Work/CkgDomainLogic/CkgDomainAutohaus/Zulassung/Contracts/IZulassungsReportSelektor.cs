using GeneralTools.Models;

namespace CkgDomainLogic.Autohaus.Models
{
    public interface IZulassungsReportSelektor
    {
        string KundenNr { get; set; }

        string Kennzeichen { get; set; }

        string AuftragsArt { get; set; }

        DateRange AuftragsDatumRange { get; set; }

        DateRange ZulassungsDatumRange { get; set; }

        string Referenz1 { get; set; }

        string Referenz2 { get; set; }

        string Referenz3 { get; set; }

        string Referenz4 { get; set; }

        string Referenz5 { get; set; }
    }
}
