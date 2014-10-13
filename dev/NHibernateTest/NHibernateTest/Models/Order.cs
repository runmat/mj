using System;
using System.Collections.Generic;

namespace NHibernateTest
{
    public class Order 
    {
        public virtual int ID { get; protected set; }

        public virtual int PersonID { get; set; }

        public virtual DateTime OrderDate { get; set; }

        public virtual string OrderComment { get; set; }
        
        public virtual ISet<OrderPosition> OrderPositions { get; set; }


        public Order()
        {
            OrderPositions = new HashSet<OrderPosition>();
        }
    }
}
