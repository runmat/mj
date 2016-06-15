
using System;

namespace AppZulassungsdienst.lib.Models
{
    public class ZLDPositionVorerfassung
    {
        public string SapId { get; set; }

        public string PositionsNr { get; set; }

        public string UebergeordnetePosition { get; set; }

        public decimal? Menge { get; set; }

        public string MaterialNr { get; set; }

        public string MaterialName { get; set; }

        public string WebMaterialart { get; set; }

        public bool? NullpreisErlaubt { get; set; }

        public string FehlerText { get; set; }

        /// <summary>
        /// O = OK, A = Angenommen, L = Löschen, V = Versand, ...
        /// </summary>
        public string WebBearbeitungsStatus { get; set; }

        public string CombineBezeichnungMenge()
        {
            var bezeichnung = MaterialName;

            var strMengeAddon = String.Format(" x{0}", Menge);

            var iCut = bezeichnung.LastIndexOf(" x");

            // Alter Werte vorhanden?
            if (iCut != -1)
            {
                var count = bezeichnung.Length - iCut;
                bezeichnung = bezeichnung.Remove(iCut, count);
            }

            if (Menge.HasValue && Menge > 1)
            {
                // Gesamtlänge mehr als n Zeichen
                if (bezeichnung.Length + strMengeAddon.Length > 40)
                {
                    var idxRemove = (40 - 1) - strMengeAddon.Length;
                    var count = bezeichnung.Length - idxRemove;
                    bezeichnung = bezeichnung.Remove(idxRemove, count);
                }

                bezeichnung += strMengeAddon;
            }

            return bezeichnung;
        }
    }
}