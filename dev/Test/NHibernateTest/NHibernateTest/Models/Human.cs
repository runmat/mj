using System;

namespace NHibernateTest
{
    public class Human
    {
        public int ID { get; protected set; }

        public string Vorname { get; set; }

        public string Nachname { get; set; }

        public DateTime? GeburtsDatum { get; set; }

        public DateTime? ErstellDatum { get; set; }

        public int? Jahre { get; set; }
    }
}
