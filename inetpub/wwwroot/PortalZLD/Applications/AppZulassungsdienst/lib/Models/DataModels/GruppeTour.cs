using System;

namespace AppZulassungsdienst.lib.Models
{
    public class GruppeTour
    {
        public string Gruppe { get; set; }

        public string VkBur { get; set; }

        public string GruppenArt { get; set; }

        public string GruppenName { get; set; }

        public DateTime? Anlagedatum { get; set; }
    }
}