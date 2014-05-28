using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TimeRegistration
{
    class RuestzeitList : List<RuestzeitObj>
    {
        public RuestzeitObj getÖffnung()
        {
            return this.Find(new Predicate<RuestzeitObj>(IsÖffnung));
        }

        public RuestzeitObj getAbrechnung()
        {
            return this.Find(new Predicate<RuestzeitObj>(IsAbrechnung));
        }

        public RuestzeitObj getEinzahlung()
        {
            return this.Find(new Predicate<RuestzeitObj>(IsEinzahlung));
        }

        public RuestzeitObj getAbrechnungEinzahlung()
        {
            return this.Find(new Predicate<RuestzeitObj>(IsAbrechnungEinzahlung));
        }


        private bool IsÖffnung(RuestzeitObj objRuest)
        {
            if (objRuest.Ruestzeitschluessel == "0")
            { return true; }
            else { return false; }
        }

        private bool IsAbrechnung(RuestzeitObj objRuest)
        {
            if (objRuest.Ruestzeitschluessel == "1")    // || objRuest.Ruestzeitschluessel == "3"
            { return true; }
            else { return false; }
        }

        private bool IsEinzahlung(RuestzeitObj objRuest)
        {
            if (objRuest.Ruestzeitschluessel == "2")    // || objRuest.Ruestzeitschluessel == "3"
            { return true; }
            else { return false; }
        }

        private bool IsAbrechnungEinzahlung(RuestzeitObj objRuest)
        {
            if (objRuest.Ruestzeitschluessel == "3")
            { return true; }
            else { return false; }
        }
    }
}
