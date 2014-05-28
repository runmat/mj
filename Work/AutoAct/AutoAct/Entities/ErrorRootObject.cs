using System.Collections.Generic;

namespace AutoAct.Entities
{
    public class ErrorRootObject
    {
        public ErrorRootObject()
        {
            errors = new List<Error>();
        }

// ReSharper disable InconsistentNaming JSON bedingt
        public List<Error> errors { get; set; }
// ReSharper restore InconsistentNaming
    }
}
