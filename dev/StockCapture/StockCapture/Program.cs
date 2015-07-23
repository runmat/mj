using System;
using System.Diagnostics;
using System.Threading;

namespace StockCapture
{
    class Program
    {
        static void Main()
        {
            var killTask = new Thread(KillSelfAfterDelay);

            CaptureStockQuotes();

            killTask.Abort();
        }

        static void KillSelfAfterDelay()
        {
            Thread.Sleep(58000);
            Process.GetCurrentProcess().Kill();
        }

        private static void CaptureStockQuotes()
        {
            try
            {
                var maxCalls = StockService.QueryCallsPerMinute;
                var timeSliceSeconds = (60 / (double)maxCalls);

                for (var i = 1; i <= maxCalls; i++)
                {
                    var timeStart = DateTime.Now;
                    
                    //StockService.CaptureStockQuote();

                    if (i == maxCalls)
                        break;

                    var timeEnd = DateTime.Now;
                    while ((timeEnd - timeStart).TotalSeconds < timeSliceSeconds)
                    {
                        Thread.Sleep(250);
                        timeEnd = DateTime.Now;
                    }
                }
            }
            catch {}
        }
    }
}
