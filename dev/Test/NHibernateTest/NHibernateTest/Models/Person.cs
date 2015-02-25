using System;
using System.Collections.Generic;

namespace NHibernateTest
{
    public class Person
    {
        public virtual int ID { get; protected set; }

        public virtual string PersonType { get; set; }

        public virtual string FirstName { get; set; }
        
        public virtual string LastName { get; set; }

        public virtual ISet<Order> Orders { get; set; }


        public Person()
        {
            Orders = new HashSet<Order>();
        }
    }
}
