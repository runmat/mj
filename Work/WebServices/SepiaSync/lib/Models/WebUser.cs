namespace SepiaSyncLib.Models
{
    public class WebUser
    {
        public virtual int UserID { get; protected set; }

        public virtual string Username { get; set; }

        public virtual string UrlRemoteLoginKey { get; set; }

        public virtual string FirstName { get; set; }

        public virtual string LastName { get; set; }

        public virtual bool Approved { get; set; }

        public virtual bool AccountIsLockedOut { get; set; }
    }
}
