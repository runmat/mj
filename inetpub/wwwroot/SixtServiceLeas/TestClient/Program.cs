using System;
using System.Linq;

namespace TestClient
{
    internal class Program
    {
        private const int ParrallelSize = 25;


        /*  IIS Anwendungspool muss so konfiguriert sein, dass der
         * Max Anzahl von Arbeitsprozessen > 1 ist. */
        private static void TestITA5368()
        {
            var client = new Client();
            client.TestWMGetFreisetzung_Status();
            var threads = (from _ in Enumerable.Repeat(0, Program.ParrallelSize)
                           select new System.Threading.Thread(client.TestWMGetFreisetzung_Status)).ToArray();

            foreach (var t in threads)
            {
                t.Start();
            }

            foreach (var t in threads)
            {
                t.Join();
            }
        }

        public static void Main(string[] args)
        {
            TestITA5368();
        }
    }
}
