using System;
using System.Configuration;

namespace SapORM.Services
{
    public class Settings
    {
        public static int DefaultUserID
        {
            get
            {
                var userID = ConfigurationManager.AppSettings["DefaultUserID"];
                return Int32.Parse(userID);
            }
        }
    }
}
