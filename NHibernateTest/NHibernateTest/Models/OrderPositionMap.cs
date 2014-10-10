using FluentNHibernate.Mapping;

namespace NHibernateTest
{
    public class OrderPositionMap : ClassMap<OrderPosition>
    {
        public OrderPositionMap()
        {
            Id(x => x.ID);
            Map(x => x.OrderID).Not.Nullable();
            Map(x => x.ProductID).Not.Nullable();
            Map(x => x.Pos).Not.Nullable();
            Map(x => x.Amount).Not.Nullable();
            Map(x => x.PosComment).Nullable();
        }
    }
}
