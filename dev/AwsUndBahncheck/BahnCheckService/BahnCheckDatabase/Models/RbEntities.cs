using System;
using System.Linq;

namespace BahnCheckDatabase.Models
{
    public partial class RbEntities
    {
        private const string ConnectionString =
            "MultipleActiveResultSets=True;Data Source=(local);Initial Catalog=BahnCheck;Password=3696;User Id=sa";

        private const int OpenRbRequestProcessingExpirationSeconds = 30;

        public bool OpenRbRequestAvailable { get { return RbRequest.Any(r => r.UpdDate == null); } }

        public string WantedZug { get; set; }

        public int WantedZugOfs { get; set; }

        public bool OpenRbRequestProcessingExpired
        {
            get
            {
                return OpenRbRequestAvailable && (DateTime.Now - GetOpenRbRequest(WantedZug, WantedZugOfs).ProcessingStartDate.GetValueOrDefault(DateTime.MinValue)).TotalSeconds > OpenRbRequestProcessingExpirationSeconds;
            }
        }

        public RbRequest GetOpenRbRequest(string wantedRbZug, int wantedRbOfs)
        {
            WantedZug = wantedRbZug;
            WantedZugOfs = wantedRbOfs;

            if (!OpenRbRequestAvailable)
                RbRequest.Add(new RbRequest { InsDate = DateTime.Now, WantedZug = wantedRbZug, WantedZugOfs = wantedRbOfs });

            SaveChanges();

            return RbRequest.FirstOrDefault(r => r.UpdDate == null);
        }

        public RbRequest GetRbRequestById(int id)
        {
            return RbRequest.FirstOrDefault(r => r.ID == id);
        }
    }
}
