using GalaSoft.MvvmLight.Ioc;
using Microsoft.Practices.ServiceLocation;

namespace AhwToolbar.ViewModels
{
    public class ViewModelLocator
    {
        /// <summary>
        /// Initializes a new instance of the ViewModelLocator class.
        /// </summary>
        public ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            SimpleIoc.Default.Register<MainViewModel>();
            SimpleIoc.Default.Register<Content1ViewModel>();
            SimpleIoc.Default.Register<Content2ViewModel>();

            SimpleIoc.Default.Register<IDataService, DataServiceSql>();
        }

        public MainViewModel Main => ServiceLocator.Current.GetInstance<MainViewModel>();
        public Content1ViewModel Content1ViewModel => ServiceLocator.Current.GetInstance<Content1ViewModel>();
        public Content2ViewModel Content2ViewModel => ServiceLocator.Current.GetInstance<Content2ViewModel>();

        public static void Cleanup()
        {
            // TODO Clear the ViewModels
        }
    }
}