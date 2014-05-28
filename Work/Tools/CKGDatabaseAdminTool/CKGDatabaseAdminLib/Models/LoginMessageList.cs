using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace CKGDatabaseAdminLib.Models
{
    public class LoginMessageList : ObservableCollection<DbEntities.LoginUserMessage>
    {
        private DbEntities.DbEntities _context;
        public DbEntities.DbEntities Context()
        {
            return _context;
        }

        public LoginMessageList(IEnumerable<DbEntities.LoginUserMessage> items, DbEntities.DbEntities context) 
            : base(items)
        {
            _context = context;
        }

        protected override void InsertItem(int index, DbEntities.LoginUserMessage item)
        {
            Context().AddToLoginUserMessage(item);
            base.InsertItem(index, item);
        }

        protected override void RemoveItem(int index)
        {
            Context().DeleteObject(this[index]);
            base.RemoveItem(index);
        }
    }
}
