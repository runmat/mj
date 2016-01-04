
namespace GeneralTools.Models
{
    public class DataConnection
    {
        public string Guid { get; set; }

        public string GuidSource { get; set; }
        public string GuidDest { get; set; }

        public DataConnection()
        {
            Guid = System.Guid.NewGuid().ToString();
        }
    }
}