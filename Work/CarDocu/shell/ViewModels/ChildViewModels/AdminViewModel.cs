namespace CarDocu.ViewModels
{
    public class AdminViewModel : IMainViewModelChild
    {
        public MainViewModel Parent { get; set; }

        private DocTypesEditViewModel _docTypesEditViewModel;
        public DocTypesEditViewModel DocTypesEditViewModel
        {
            get { return (_docTypesEditViewModel ?? (_docTypesEditViewModel = new DocTypesEditViewModel())); }
        }

        private DomainUsersEditViewModel _domainUsersEditViewModel;
        public DomainUsersEditViewModel DomainUsersEditViewModel
        {
            get { return (_domainUsersEditViewModel ?? (_domainUsersEditViewModel = new DomainUsersEditViewModel())); }
        }

        private AppSettingsEditViewModel _appSettingsEditViewModel;
        public AppSettingsEditViewModel AppSettingsEditViewModel
        {
            get { return (_appSettingsEditViewModel ?? (_appSettingsEditViewModel = new AppSettingsEditViewModel())); }
        }

        private DomainLocationsEditViewModel _domainLocationsEditViewModel;
        public DomainLocationsEditViewModel DomainLocationsEditViewModel
        {
            get { return (_domainLocationsEditViewModel ?? (_domainLocationsEditViewModel = new DomainLocationsEditViewModel())); }
        }
    }
}
