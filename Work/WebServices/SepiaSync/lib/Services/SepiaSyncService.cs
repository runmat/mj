using System.Linq;
using SepiaSyncLib.Models;

namespace SepiaSyncLib.Services
{
    public class SepiaSyncService
    {
        public static bool SyncUsersToSepia()
        {
            try
            {
                var repository = new SqlDbRepository();

                var usersToSync = repository.GetUsers();

                usersToSync.ToList().ForEach(user =>
                    {
                        if (InvokeSepiaSyncHttpRequest(user))
                            repository.SetSepiaSyncDateForUser(user);
                    });
            }
            catch 
            {
                return false;
            }

            return true;
        }

        static bool InvokeSepiaSyncHttpRequest(WebUserSepiaAccess user)
        {
            return user.Username == "DADS";
        }
    }
}
