using FluentNHibernate.Mapping;

namespace StockCapture.Models
{
    class StockQuoteMap : ClassMap<StockQuote>
    {
        public StockQuoteMap()
        {
            Id(x => x.ID);
            Map(x => x.Val).Nullable();
            Map(x => x.Date).Nullable();
            Map(x => x.InsertDate).Nullable();

            //Not.LazyLoad();
        }
    }
}
