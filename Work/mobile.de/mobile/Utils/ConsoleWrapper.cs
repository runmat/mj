using System;
using Mobile.Interfaces;

namespace AutoAct.Utils
{
    public class ConsoleWrapper : IConsoleWrapper
    {
        public void WriteError(string error)
        {
            Console.Error.WriteLine(error);
        }

        public void WriteInfo(string info)
        {
            Console.WriteLine(info);
        }
    }
}
