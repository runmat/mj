using FluentNHibernate.Mapping;

namespace SepiaSyncLib.Models
{
    public class WebUserSepiaAccessMap : ClassMap<WebUserSepiaAccess>
    {
        public WebUserSepiaAccessMap()
        {
            Table("vwWebUserSepiaAccess");

            Id(x => x.UserID);

            Map(x => x.Username).Nullable();
            Map(x => x.FirstName).Nullable();
            Map(x => x.LastName).Nullable();
            Map(x => x.UrlRemoteLoginKey).Nullable();
            Map(x => x.SepiaSyncDate).Nullable();
            Map(x => x.LastChangeDate).Nullable();

            Map(x => x.Approved);
            Map(x => x.AccountIsLockedOut);
        }
    }
}
