using FluentNHibernate.Mapping;

namespace SepiaSyncLib.Models
{
    public class WebUserMap : ClassMap<WebUser>
    {
        public WebUserMap()
        {
            Id(x => x.UserID);
            Map(x => x.Username).Nullable();
            Map(x => x.FirstName).Nullable();
            Map(x => x.LastName).Nullable();
            Map(x => x.UrlRemoteLoginKey).Nullable();

            Map(x => x.Approved);
            Map(x => x.AccountIsLockedOut);
        }
    }
}
