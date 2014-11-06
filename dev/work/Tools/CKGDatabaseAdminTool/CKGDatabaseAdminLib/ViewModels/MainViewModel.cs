using System;
using System.Collections.Specialized;
using System.Configuration;
using WpfTools4.ViewModels;
using System.Collections.ObjectModel;
using System.Timers;

namespace CKGDatabaseAdminLib.ViewModels
{
    public enum MessageType
    {
        Success = 1,
        Error = 2
    }

    public class MainViewModel : ViewModelBase
    {
        #region Properties

        private ViewModelBase _activeViewModel;
        public ViewModelBase ActiveViewModel
        {
            get { return _activeViewModel; }
            set { _activeViewModel = value; SendPropertyChanged("ActiveViewModel"); }
        }

        private LoginUserMessageViewModel _loginUserMessageViewModel;
        public LoginUserMessageViewModel LoginUserMessageViewModel
        {
            get { return _loginUserMessageViewModel; }
            set { _loginUserMessageViewModel = value; SendPropertyChanged("LoginUserMessageViewModel"); }
        }

        private ApplicationBapiViewModel _applicationBapiViewModel;
        public ApplicationBapiViewModel ApplicationBapiViewModel
        {
            get { return _applicationBapiViewModel; }
            set { _applicationBapiViewModel = value; SendPropertyChanged("ApplicationBapiViewModel"); }
        }

        private BapiApplicationViewModel _bapiApplicationViewModel;
        public BapiApplicationViewModel BapiApplicationViewModel
        {
            get { return _bapiApplicationViewModel; }
            set { _bapiApplicationViewModel = value; SendPropertyChanged("BapiApplicationViewModel"); }
        }

        private ApplicationCopyViewModel _applicationCopyViewModel;
        public ApplicationCopyViewModel ApplicationCopyViewModel
        {
            get { return _applicationCopyViewModel; }
            set { _applicationCopyViewModel = value; SendPropertyChanged("ApplicationCopyViewModel"); }
        }

        private FieldTranslationCopyViewModel _fieldTranslationCopyViewModel;
        public FieldTranslationCopyViewModel FieldTranslationCopyViewModel
        {
            get { return _fieldTranslationCopyViewModel; }
            set { _fieldTranslationCopyViewModel = value; SendPropertyChanged("FieldTranslationCopyViewModel"); }
        }

        private BapiCheckViewModel _bapiCheckViewModel;
        public BapiCheckViewModel BapiCheckViewModel
        {
            get { return _bapiCheckViewModel; }
            set { _bapiCheckViewModel = value; SendPropertyChanged("BapiCheckViewModel"); }
        }

        private GitBranchInfoViewModel _gitBranchViewModel;
        public GitBranchInfoViewModel GitBranchViewModel
        {
            get { return _gitBranchViewModel; }
            set { _gitBranchViewModel = value; SendPropertyChanged("GitBranchViewModel"); }
        }

        public ObservableCollection<string> DbConnections { get; private set; }

        private string _actualDatabase;
        public string ActualDatabase
        {
            get { return _actualDatabase; }
            set { _actualDatabase = value; SendPropertyChanged("ActualDatabase"); }
        }

        private bool _testSap;
        public bool TestSap
        {
            get { return _testSap; }
            set { _testSap = value; SendPropertyChanged("TestSap"); }
        }

        private string _nachricht;
        public string Nachricht
        {
            get { return _nachricht; }
            set { _nachricht = value; SendPropertyChanged("Nachricht"); }
        }

        private MessageType _nachrichtenTyp;
        public MessageType NachrichtenTyp
        {
            get { return _nachrichtenTyp; }
            set { _nachrichtenTyp = value; SendPropertyChanged("NachrichtenTyp"); }
        }

        private Timer _messageDisplayTimer;

        #endregion

        public MainViewModel()
        {
            _messageDisplayTimer = new Timer(10000);
            TestSap = (String.IsNullOrEmpty(ConfigurationManager.AppSettings["ProdSAP"]) || ConfigurationManager.AppSettings["ProdSAP"].ToUpper() != "TRUE");
            _messageDisplayTimer.Elapsed += MessageDisplayTimerOnElapsed;
            LoadDbConnections();
        }

        private void MessageDisplayTimerOnElapsed(object sender, ElapsedEventArgs elapsedEventArgs)
        {
            Nachricht = "";
        }

        private void LoadDbConnections()
        {
            DbConnections = new ObservableCollection<string>();
            var sectionData = (NameValueCollection)ConfigurationManager.GetSection("dbConnections");
            foreach (var item in sectionData.AllKeys)
            {
                DbConnections.Add(item);
            }
            if (DbConnections.Count > 0)
            {
                ActualDatabase = DbConnections[0];
            }
        }

        public void SelectDbConnection()
        {
            ActiveViewModel = null;
            InitViewModels();
        }

        private void InitViewModels()
        {
            LoginUserMessageViewModel = new LoginUserMessageViewModel(this);
            ApplicationBapiViewModel = new ApplicationBapiViewModel(this);
            BapiApplicationViewModel = new BapiApplicationViewModel(this);
            ApplicationCopyViewModel = new ApplicationCopyViewModel(this);
            FieldTranslationCopyViewModel = new FieldTranslationCopyViewModel(this);
            BapiCheckViewModel = new BapiCheckViewModel(this);

            // Git-Branch Verwaltung nur in DAD-Datenbanken
            if (!String.IsNullOrEmpty(ActualDatabase) && (ActualDatabase.ToUpper().StartsWith("DAD ")))
            {
                GitBranchViewModel = new GitBranchInfoViewModel(this);
            }
            else
            {
                GitBranchViewModel = null;
            }
        }

        public void ShowMessage(string msg, MessageType typ)
        {
            Nachricht = msg;
            NachrichtenTyp = typ;
            _messageDisplayTimer.Stop();
            _messageDisplayTimer.Start();
        }
    }
}
