
namespace CKGDatabaseAdminLib.Models
{
    public class BapiCheckAbweichung
    {
        public string BapiName { get; set; }

        public bool HasChanged { get; set; }

        public bool IsNew { get; set; }

        public bool DoesNotExistInSap { get; set; }
    }
}
