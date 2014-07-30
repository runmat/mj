using System;
using System.Collections.Generic;

namespace GeneralTools.Services
{
    public class DeutscheFeiertageEinesJahres
    {
        public int Jahr { get; private set; }

        public List<Feiertag> Feiertage { get; private set; }

        public List<Feiertag> FesteFeiertage { get { return Feiertage.FindAll(f => f.IstFix); } }
	
        public DeutscheFeiertageEinesJahres(int jahr)
        {
            Jahr = jahr;

            Feiertage = new List<Feiertag>
                {
                    new Feiertag(true, new DateTime(Jahr, 1, 1), "Neujahr"),
                    //new Feiertag(true, new DateTime(Jahr, 1, 6), "Heilige Drei Könige"),
                    new Feiertag(true, new DateTime(Jahr, 5, 1), "Tag der Arbeit"),
                    new Feiertag(true, new DateTime(Jahr, 10, 3), "Tag der dt. Einheit"),
                    //new Feiertag(true, new DateTime(Jahr, 10, 31), "Reformationstag"),
                    //new Feiertag(true, new DateTime(Jahr, 11, 1), "Allerheiligen "),
                    new Feiertag(true, new DateTime(Jahr, 12, 25), "1. Weihnachtstag"),
                    new Feiertag(true, new DateTime(Jahr, 12, 26), "2. Weihnachtstag")
                };

            var osterSonntag = GetOsterSonntag();
            Feiertage.Add(new Feiertag(false, osterSonntag, "Ostersonntag"));
            //Feiertage.Add(new Feiertag(false, osterSonntag.AddDays(-3), "Gründonnerstag"));
            Feiertage.Add(new Feiertag(false, osterSonntag.AddDays(-2), "Karfreitag"));
            Feiertage.Add(new Feiertag(false, osterSonntag.AddDays(1), "Ostermontag"));
            Feiertage.Add(new Feiertag(false, osterSonntag.AddDays(39), "Christi Himmelfahrt"));
            Feiertage.Add(new Feiertag(false, osterSonntag.AddDays(49), "Pfingstsonntag"));
            Feiertage.Add(new Feiertag(false, osterSonntag.AddDays(50), "Pfingstmontag"));
            //Feiertage.Add(new Feiertag(false, osterSonntag.AddDays(60), "Fronleichnam"));
        }

        private DateTime GetOsterSonntag()
        {
            int  g,h,c,j,l,i;

            g = Jahr % 19;
            c = Jahr / 100;
            h = ((c-(c/4)) - (((8*c)+13)/25) + (19*g) + 15) % 30;
            i = h - (h/28) *(1- (29/(h+1)) * ((21-g)/11));
            j = (Jahr + (Jahr / 4) + i + 2 - c + (c / 4)) % 7;

            l = i - j;
            var month = 3+ ((l+40)/44);
            var day = l + 28 - 31 * (month / 4);
            
            return new DateTime(Jahr, month, day);

        }
    }

    public class Feiertag : IComparable<Feiertag>
    {
        public string Name { get; set; }

        public DateTime Datum { get; set; }

        public bool IstFix { get; set; }

        
        public Feiertag(bool isFix, DateTime datum, string name)
        {
            this.IstFix = isFix;
            this.Datum = datum;
            this.Name = name;
        }

        #region IComparable<Feiertag> Member

        public int CompareTo(Feiertag other)
        {
            return this.Datum.Date.CompareTo(other.Datum.Date);
        }

        #endregion
    }
}
