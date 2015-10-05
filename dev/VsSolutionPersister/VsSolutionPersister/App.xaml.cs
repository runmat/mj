using System.Diagnostics;
using System.Windows;

namespace VsSolutionPersister
{
    public partial class App 
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            if (e.Args.Length >= 1)
            {
                if (e.Args[0].ToLower() == "clipboard")
                    Clipboard.SetText("seE17igEl");

                Shutdown(0);
            }

            base.OnStartup(e);
        }
    }
}
