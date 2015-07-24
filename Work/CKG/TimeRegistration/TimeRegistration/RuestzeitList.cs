using System.Collections.Generic;

namespace TimeRegistration
{
    class RuestzeitList : List<RuestzeitObj>
    {
        public RuestzeitObj getÖffnung()
        {
            return this.Find(IsÖffnung);
        }

        public RuestzeitObj getAbrechnung()
        {
            return this.Find(IsAbrechnung);
        }

        public RuestzeitObj getEinzahlung()
        {
            return this.Find(IsEinzahlung);
        }

        public RuestzeitObj getAbrechnungEinzahlung()
        {
            return this.Find(IsAbrechnungEinzahlung);
        }

        private bool IsÖffnung(RuestzeitObj objRuest)
        {
            return (objRuest.Ruestzeitschluessel == "0");
        }

        private bool IsAbrechnung(RuestzeitObj objRuest)
        {
            return (objRuest.Ruestzeitschluessel == "1") ;
        }

        private bool IsEinzahlung(RuestzeitObj objRuest)
        {
            return (objRuest.Ruestzeitschluessel == "2");
        }

        private bool IsAbrechnungEinzahlung(RuestzeitObj objRuest)
        {
            return (objRuest.Ruestzeitschluessel == "3");
        }
    }
}
