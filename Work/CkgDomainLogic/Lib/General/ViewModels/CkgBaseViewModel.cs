using System.Xml.Serialization;
using CkgDomainLogic.General.Contracts;
using GeneralTools.Contracts;
using GeneralTools.Services;

namespace CkgDomainLogic.General.ViewModels
{
    public class CkgBaseViewModel : Store 
    {
        [XmlIgnore]
        public IAppSettings AppSettings { get; private set; }

        [XmlIgnore]
        public ILogonContextDataService LogonContext { get; private set; }


        private void InitDataService<T>(T dataService) where T : class
        {
            //if (!(dataService is ICkgGeneralDataService))
            //    return;

            var cachedService = CacheGet<T>();
            if (cachedService == null)
                CacheSet(dataService);

            var generalCachedService = (CacheGet<T>() as ICkgGeneralDataService);
            if (generalCachedService != null)
                generalCachedService.Init(this.AppSettings, this.LogonContext);
        }

        public virtual void Init(IAppSettings appSettings, ILogonContextDataService logonContext)
        {
            this.AppSettings = appSettings;
            this.LogonContext = logonContext;
        }

        public void Init<T>(
            IAppSettings appSettings, ILogonContextDataService logonContext,
            T dataService1)
            where T : class
        {
            Init(appSettings, logonContext);

            InitDataService(dataService1);
        }

        public void Init<T, TU>(
            IAppSettings appSettings, ILogonContextDataService logonContext,
            T dataService1, TU dataService2)
            where T : class
            where TU : class
        {
            Init(appSettings, logonContext);

            InitDataService(dataService1);
            InitDataService(dataService2);
        }

        public void Init<T, TU, TV>(
            IAppSettings appSettings, ILogonContextDataService logonContext,
            T dataService1, TU dataService2, TV dataService3)
            where T : class
            where TU : class
            where TV : class
        {
            Init(appSettings, logonContext);

            InitDataService(dataService1);
            InitDataService(dataService2);
            InitDataService(dataService3);
        }

        public void Init<T, TU, TV, TW>(
            IAppSettings appSettings, ILogonContextDataService logonContext,
            T dataService1, TU dataService2, TV dataService3, TW dataService4)
            where T : class
            where TU : class
            where TV : class
            where TW : class
        {
            Init(appSettings, logonContext);

            InitDataService(dataService1);
            InitDataService(dataService2);
            InitDataService(dataService3);
            InitDataService(dataService4);
        }
    }
}
