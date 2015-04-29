using System.Collections.Generic;

namespace AppZulassungsdienst.lib.Models
{
    public class ZLDVorgangVorerfassung
    {
        public ZLDKopfdaten Kopfdaten { get; set; }

        public ZLDBankdaten Bankdaten { get; set; }

        public ZLDAdressdaten Adressdaten { get; set; }

        public List<ZLDPositionVorerfassung> Positionen { get; set; }

        public ZLDVorgangVorerfassung(string VkOrg, string VkBur)
        {
            Kopfdaten = new ZLDKopfdaten { VkOrg = VkOrg, VkBur = VkBur };
            Bankdaten = new ZLDBankdaten();
            Adressdaten = new ZLDAdressdaten();
            Positionen = new List<ZLDPositionVorerfassung>();
        }
    }
}