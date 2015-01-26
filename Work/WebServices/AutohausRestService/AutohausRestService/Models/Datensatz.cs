using System.Collections.Generic;

namespace AutohausRestService.Models
{
    public class Datensatz
    {
        public int ID { get { return (Kaeufer != null ? Kaeufer.ID : 0); } } 

        public Kaeufer Kaeufer { get; set; }

        public List<Fahrzeug> Fahrzeuge { get; set; }
    }
}