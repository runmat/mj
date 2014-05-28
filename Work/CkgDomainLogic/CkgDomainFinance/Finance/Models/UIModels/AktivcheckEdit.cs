using System.Collections.Generic;
using CkgDomainLogic.DomainCommon.Models;

namespace CkgDomainLogic.Finance.Models
{
    /// <summary>
    /// Model für Aktivcheck-Bearbeitung
    /// </summary>
    public class AktivcheckEdit
    {
        public AktivcheckTreffer AktivcheckItem { get; set; }

        public List<Domaenenfestwert> AuswahlKlassifizierung { get; set; }
    }
}
