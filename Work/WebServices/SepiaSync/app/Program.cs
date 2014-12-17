using System;
using SepiaSyncLib.Services;

namespace SepiaSync
{
    class Program
    {
        static void Main()
        {
            if (!SepiaSyncService.SyncUsersToSepia())
                Environment.Exit(-1);
        }
    }
}
