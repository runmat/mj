using System;
using System.Windows;
using WpfTools4.Common.Exceptions;
using System.Reflection;

namespace WpfTools4.Common
{
    public static class Actions
    {
        public static Action NoOp = () => { };

        public static Action<Exception> OnErrorMessageBox =
            exception =>
                {
                    var ex = (exception.InnerException ?? exception);

                    MessageBox.Show(ex.Message,
                                    Assembly.GetCallingAssembly().GetName().Name,
                                    MessageBoxButton.OK,
                                    GetMessageBoxImage(exception),
                                    MessageBoxResult.OK);
                    
                    MessageBox.Show(exception.Message,
                                    Assembly.GetCallingAssembly().GetName().Name,
                                    MessageBoxButton.OK,
                                    GetMessageBoxImage(exception),
                                    MessageBoxResult.OK);
                };

        public static Action<Exception> OnErrorResume = exception => { };
        public static Action<Exception> OnErrorThrow = exception => { throw exception; };

        private static MessageBoxImage GetMessageBoxImage(Exception ex)
        {
            if (ex is PublicException)
                return MessageBoxImage.Information;

            return MessageBoxImage.Error;
        }
    }
}
