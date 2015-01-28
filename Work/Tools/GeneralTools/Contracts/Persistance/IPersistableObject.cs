using System;

namespace GeneralTools.Contracts
{
    public interface IPersistableObject
    {
        string ObjectKey { get; set; }

        DateTime? EditDate { get; set; }

        string EditUser { get; set; }
    }
}
