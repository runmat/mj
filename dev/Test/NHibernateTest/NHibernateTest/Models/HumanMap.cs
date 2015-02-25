using FluentNHibernate.Mapping;

namespace NHibernateTest
{
    public class HumanMap : ClassMap<Human>
    {
        public HumanMap()
        {
            Id(x => x.ID);
            Map(x => x.Vorname).Nullable();
            Map(x => x.Nachname).Nullable();
            Map(x => x.GeburtsDatum).Nullable();
            Map(x => x.ErstellDatum).Nullable();
            Map(x => x.Jahre).Nullable();

            Not.LazyLoad();
        }
    }
}
