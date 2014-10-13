using FluentNHibernate.Mapping;

namespace NHibernateTest
{
    public class PersonMap : ClassMap<Person>
    {
        public PersonMap()
        {
            Id(x => x.ID);
            Map(x => x.FirstName).Nullable();
            Map(x => x.LastName).Nullable();
            Map(x => x.PersonType).Nullable();

            HasMany(x => x.Orders)
                .KeyColumn("PersonID")
                .AsSet()
                .Cascade.All().Inverse().LazyLoad();
        }
    }
}
