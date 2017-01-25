using System.Diagnostics;
using System.Linq;

namespace PdfPrint
{
    public class Helper
    {
        public static void KillAllProcessesOf(string processName)
        {
            var otherBahnCheckerProcess = Process.GetProcessesByName(processName).Where(p => p.Id != Process.GetCurrentProcess().Id).ToList();
            if (otherBahnCheckerProcess.Any())
                otherBahnCheckerProcess.ForEach(p => p.Kill());
        }

        public static void PrintPdf(string pdfFileName)
        {
            KillAllProcessesOf("AcroRd32");

            // ReSharper disable PossibleNullReferenceException
            var processFilename = Microsoft.Win32.Registry.LocalMachine.OpenSubKey("Software").OpenSubKey("Microsoft").OpenSubKey("Windows").OpenSubKey("CurrentVersion").OpenSubKey("App Paths").OpenSubKey("AcroRd32.exe").GetValue(string.Empty).ToString();
            // ReSharper restore PossibleNullReferenceException

            var info = new ProcessStartInfo
            {
                Verb = "print",
                FileName = processFilename,
                Arguments = $"/p /h /s {pdfFileName}",
                CreateNoWindow = true,
                WindowStyle = ProcessWindowStyle.Hidden,
                UseShellExecute = false
            };
            // it won't be hidden anyway... thanks Adobe!

            var p = Process.Start(info);
            p.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;

            var counter = 0;
            while (!p.HasExited)
            {
                System.Threading.Thread.Sleep(1000);
                counter += 1;
                if (counter == 5) break;
            }
            if (p.HasExited) return;

            p.CloseMainWindow();
            p.Kill();
        }
    }
}
