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

        public ObservableCollection<string> DbConnections { get; private set; }

        private string _actualDatabase;
        public string ActualDatabase
        {
            get { return _actualDatabase; }
            set { _actualDatabase = value; SendPropertyChanged("ActualDatabase"); }
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
            _messageDisplayTimer = new Timer(5000);
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
            ApplicationCopyViewModel = new ApplicationCopyViewModel(this);
            FieldTranslationCopyViewModel = new FieldTranslationCopyViewModel(this);
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
