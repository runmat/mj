using System.ComponentModel.DataAnnotations;

namespace CkgImportLocalizationResourcesFromDb
{
    public class TranslatedResource
    {
        [Key]
        public string Resource { get; set; }

        public string Format { get; set; }

        public string en { get; set; }

        public string en_kurz { get; set; }

        public string de { get; set; }

        public string de_kurz { get; set; }

        public string de_de { get; set; }

        public string de_de_kurz { get; set; }

        public string de_at { get; set; }

        public string de_at_kurz { get; set; }

        public string de_ch { get; set; }

        public string de_ch_kurz { get; set; }
    }
}
