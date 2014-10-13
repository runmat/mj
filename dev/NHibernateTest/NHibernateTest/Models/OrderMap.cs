using FluentNHibernate.Mapping;

namespace NHibernateTest
{
    public class OrderMap : ClassMap<Order>
    {
        public OrderMap()
        {
            Id(x => x.ID);
            Map(x => x.PersonID).Not.Nullable();
            Map(x => x.OrderDate).Not.Nullable();
            Map(x => x.OrderComment).Nullable();

            HasMany(x => x.OrderPositions)
                .KeyColumn("OrderID")
                .AsSet()
                .Cascade.All().Inverse().LazyLoad();
        }
    }
}
