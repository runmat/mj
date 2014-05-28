using System.Collections.Generic;
using System.Linq;

namespace CkgDomainLogic.Charts.Models
{
    public class KgsStatistic
    {
        public Dictionary<string, int> CountPerKgs { get; private set; }

        public int CountMax { get; private set; }


        public void SetData(List<KbaPlzKgs> kbaItems)
        {
            CountPerKgs = kbaItems.OrderBy(o => o.KGS).GroupBy(g => g.KGS).ToDictionary(k => k.Key, v => v.Sum(d => d.FahrzeugAnzahl));
            CountMax = CountPerKgs.Max(c => c.Value);

            //CountMax = (from g in kbaItems group g by g.KGS into kgs select kgs.Sum(d => d.FahrzeugAnzahl)).Max();
        }
    }
}
