namespace NHibernateTest
{
    public class OrderPosition
    {
        public virtual int ID { get; protected set; }

        public virtual int OrderID { get; set; }

        public virtual int ProductID { get; set; }

        public virtual int Pos { get; set; }

        public virtual int Amount { get; set; }

        public virtual string PosComment { get; set; }
    }
}
