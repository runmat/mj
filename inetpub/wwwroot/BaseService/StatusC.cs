using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Collections;

namespace BaseService
{
    public class StatusC : CollectionBase
    {
        public void Add(Status dStatus)
        {
            List.Add(dStatus);

        }

        public virtual Status this[int index]
        {
            get { return (Status)this.List[index]; }
        }
    }
}
