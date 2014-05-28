using System;
using LogMaintenance.Services;

namespace LogMaintenance
{
    class Program
    {
        static void Main()
        {
            BusinessDataCopyService.CopyToLogsDb(Console.WriteLine);
        }
    }
}
