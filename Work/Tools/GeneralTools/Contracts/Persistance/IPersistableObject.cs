using System;
using System.Collections.Generic;

namespace GeneralTools.Contracts
{
    public interface IPersistableObject
    {
        string ObjectKey { get; set; }

        DateTime? EditDate { get; set; }

        string EditUser { get; set; }
    }

    public class IPersistableObjectComparer : IEqualityComparer<IPersistableObject>
    {
        bool IEqualityComparer<IPersistableObject>.Equals(IPersistableObject x, IPersistableObject y)
        {
            return x.ObjectKey.Equals(y.ObjectKey);
        }

        int IEqualityComparer<IPersistableObject>.GetHashCode(IPersistableObject obj)
        {
            return obj.ObjectKey.GetHashCode();
        }
    }
}
