using System;
using System.Collections.Generic;

namespace GeneralTools.Contracts
{
    public interface IPersistableObject 
    {
        string ObjectKey { get; set; }

        string ObjectName { get; set; }

        DateTime? EditDate { get; set; }

        string EditUser { get; set; }
    }

    public class IPersistableObjectComparer : IEqualityComparer<IPersistableObject>
    {
        bool IEqualityComparer<IPersistableObject>.Equals(IPersistableObject x, IPersistableObject y)
        {
            if (x == null || y == null)
                return false;

            return x.ObjectKey.Equals(y.ObjectKey);
        }

        int IEqualityComparer<IPersistableObject>.GetHashCode(IPersistableObject obj)
        {
            return 0;
        }
    }
}
