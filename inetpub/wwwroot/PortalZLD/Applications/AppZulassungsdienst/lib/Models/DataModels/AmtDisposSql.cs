
namespace AppZulassungsdienst.lib.Models
{
    public class AmtDisposSql
    {
        public string Amt { get; set; }

        public string MobileUserId { get; set; }

        public bool Vorschuss { get; set; }

        public decimal? VorschussBetrag { get; set; }

        public AmtDisposSql(string amt, string mobileUserId, bool vorschuss, decimal? vorschussBetrag)
        {
            Amt = amt;
            MobileUserId = mobileUserId;
            Vorschuss = vorschuss;
            VorschussBetrag = vorschussBetrag;
        }
    }
}