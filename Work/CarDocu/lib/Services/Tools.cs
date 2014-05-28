using System.Windows;
using Microsoft.VisualBasic;

namespace CarDocu.Services
{
    public class Tools
    {
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
            return Input(prompt, "Bitte eingeben");
        }

        static public string Input(string prompt, string title)
        {
            return Interaction.InputBox(prompt, title);
        }
    }

}
