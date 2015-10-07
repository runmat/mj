using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Media;
using Microsoft.VisualBasic;

namespace WpfTools4.Services
{
    public class Tools
    {
        [DllImportAttribute("User32.dll")]
        public static extern int FindWindow(string className, string windowName);

        [DllImportAttribute("User32.dll")]
        public static extern int SetForegroundWindow(IntPtr hWnd);

        [DllImport("user32.dll")]
        public static extern IntPtr SetActiveWindow(IntPtr hWnd);

        [DllImport("coredll.dll")]
        public static extern bool ShowWindow(IntPtr wHnd, int cmdShow);

        [DllImport("user32.dll")]
        public static extern IntPtr SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int x, int Y, int cx, int cy,
            int wFlags);

        [DllImport("user32.dll")]
        public static extern bool BringWindowToTop(IntPtr hWnd);


        public static void StartExeAsModalDialog(string exeFileName, string arguments)
        {
            var view = Application.Current.MainWindow;
            if (view != null) view.IsEnabled = false;
            Process p = null;
            try
            {
                p = Process.Start(exeFileName, arguments);
            }
            catch (FileNotFoundException)
            {
                MessageBox.Show(
                    string.Format(
                        "Fehler: Das Programmm '{0}' befindet sich nicht im StartUp-Verzeichnis dieser Applikation!",
                        exeFileName));
            }
            if (p == null) return;
            do System.Windows.Forms.Application.DoEvents(); while (!p.WaitForExit(1000));
            if (view != null) view.IsEnabled = true;
        }

        public static void Alert(string hint)
        {
            MessageBox.Show(hint, "Hinweis", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        public static void AlertError(string hint)
        {
            MessageBox.Show(hint, "Fehler", MessageBoxButton.OK, MessageBoxImage.Exclamation);
        }

        public static void AlertCritical(string hint)
        {
            MessageBox.Show(hint, "Fehler", MessageBoxButton.OK, MessageBoxImage.Stop);
        }

        public static bool Confirm(string question)
        {
            return
                MessageBox.Show(question, "Frage", MessageBoxButton.OKCancel, MessageBoxImage.Question,
                    MessageBoxResult.Cancel) != MessageBoxResult.Cancel;
        }

        public static bool Deny(string question)
        {
            return !Confirm(question);
        }

        public static string Input(string prompt)
        {
            return Interaction.InputBox(prompt, "Bitte eingeben");
        }

        public static T GetVisualChild<T>(DependencyObject parent) where T : Visual
        {
            var child = default(T);

            var numVisuals = VisualTreeHelper.GetChildrenCount(parent);
            for (var i = 0; i < numVisuals; i++)
            {
                var v = (Visual) VisualTreeHelper.GetChild(parent, i);
                child = v as T ?? GetVisualChild<T>(v);
                if (child != null)
                {
                    break;
                }
            }
            return child;
        }

        public static bool IsWindowOpenForProcessNamePartAndTitlePart(string processNamePart, string captionPart,
            Action actionIfYes = null)
        {
            try
            {
                foreach (var process in Process.GetProcessesByName(processNamePart))
                    if (process.MainWindowTitle.ToLower().Contains(captionPart.ToLower()))
                    {
                        if (actionIfYes != null)
                            actionIfYes();

                        var hWnd = process.MainWindowHandle;
                        SetForegroundWindow(hWnd);

                        return true;
                    }
            }
            catch
            {
                return false;
            }

            return false;
        }

        public static void ShowDesktop()
        {
            var typeShell = Type.GetTypeFromProgID("Shell.Application");
            var objShell = Activator.CreateInstance(typeShell);
            typeShell.InvokeMember("MinimizeAll", System.Reflection.BindingFlags.InvokeMethod, null, objShell, null);

        }
    }
}
