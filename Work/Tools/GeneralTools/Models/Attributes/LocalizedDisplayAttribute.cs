using System.ComponentModel;
using GeneralTools.Contracts;

namespace GeneralTools.Models
{
    public class LocalizedDisplayAttribute : DisplayNameAttribute, ILocalizedDisplayAttribute
    {
        public string ResourceID { get { return DisplayName; } }

        public object Suffix { get; private set; }


        public LocalizedDisplayAttribute(string resourceID)
            : base(resourceID)
        {
        }

        public LocalizedDisplayAttribute(string resourceID, object suffix)
            : base(resourceID)
        {
            Suffix = suffix;
        }
    }
}
