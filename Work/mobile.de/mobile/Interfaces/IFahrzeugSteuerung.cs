using System.Collections.Generic;
using Mobile.Bapi;
using SapORM.Models;

namespace Mobile.Interfaces
{
    /// <summary>
    /// Zuständig für die Übermittlung von Fahrzeugen eines Kunden an AutoAct
    /// </summary>
    public interface IFahrzeugSteuerung
    {
        /// <summary>
        /// Übermittle die Fahrzeuge des Kunden an AutoAct
        /// </summary>
        /// <param name="kunde">Kundennummer wird verwendet um die Verzeichnise der Dateien zu ermitteln</param>
        /// <param name="fahrzeuge">Liste der Fahrzeuge die übermittelt weden sollen</param>
        void Execute(Kunde kunde, IEnumerable<Z_DPM_READ_AUTOACT_01.GT_OUT> fahrzeuge);
    }
}
