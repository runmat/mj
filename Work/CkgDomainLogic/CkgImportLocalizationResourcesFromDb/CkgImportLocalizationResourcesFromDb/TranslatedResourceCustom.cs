using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CkgImportLocalizationResourcesFromDb
{
    public class TranslatedResourceCustom
    {
        [Key, Column(Order = 1)]
        public string Resource { get; set; }

        [Key, Column(Order = 2)]
        public int CustomerID { get; set; }

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

        public string fr { get; set; }

        public string fr_kurz { get; set; }
    }
}
