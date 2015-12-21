using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using Microsoft.VisualBasic;

namespace WpfTools4.Services
{
    public class Tools
    {
        static public void StartRecordsetFlagger(Dictionary<string, string> argDict)
        {
            StartExeAsModalDialog("RecordsetFlagger.exe", string.Join(" ", argDict.Select(arg => string.Format("{0}={1}", arg.Key, arg.Value)).ToArray()));
        }

        static public void StartExeAsModalDialog(string exeFileName, string arguments)
        {
            var view = Application.Current.MainWindow;
            if (view != null) view.IsEnabled = false;
            Process p = null;
            try
            {
                p = Process.Start(exeFileName, arguments);
            }
            catch(FileNotFoundException)
            {
                MessageBox.Show(string.Format("Fehler: Das Programmm '{0}' befindet sich nicht im StartUp-Verzeichnis dieser Applikation!", exeFileName));
            }
            if (p == null) return;
            do System.Windows.Forms.Application.DoEvents();
            while (!p.WaitForExit(1000));
            if (view != null) view.IsEnabled = true;
        }

        static public void Alert(string hint)
        {
            MessageBox.Show(hint, "Hinweis", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        static public void AlertError(string hint)
        {
            MessageBox.Show(hint, "Fehler", MessageBoxButton.OK, MessageBoxImage.Exclamation);
        }

        static public void AlertCritical(string hint)
        {
            MessageBox.Show(hint, "Fehler", MessageBoxButton.OK, MessageBoxImage.Stop);
        }

        static public bool Confirm(string question)
        {
            return MessageBox.Show(question, "Frage", MessageBoxButton.OKCancel, MessageBoxImage.Question, MessageBoxResult.Cancel) != MessageBoxResult.Cancel;
        }

        static public bool Deny(string question)
        {
            return !Confirm(question);
        }

        static public string Input(string prompt)
        {
            return Interaction.InputBox(prompt, "Bitte eingeben");
        }

        static public T GetVisualChild<T>(DependencyObject parent) where T : Visual
        {
            var child = default(T);

            var numVisuals = VisualTreeHelper.GetChildrenCount(parent);
            for (var i = 0; i < numVisuals; i++)
            {
                var v = (Visual)VisualTreeHelper.GetChild(parent, i);
                child = v as T ?? GetVisualChild<T>(v);
                if (child != null)
                {
                    break;
                }
            }
            return child;
        }

        public static void ShowDesktop()
        {
            var typeShell = Type.GetTypeFromProgID("Shell.Application");
            var objShell = Activator.CreateInstance(typeShell);
            typeShell.InvokeMember("MinimizeAll", System.Reflection.BindingFlags.InvokeMethod, null, objShell, null);

        }
    }
}
