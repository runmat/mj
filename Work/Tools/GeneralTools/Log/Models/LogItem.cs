using System;
using System.Web.Script.Serialization;
using System.Xml.Serialization;
using GeneralTools.Contracts;
using GeneralTools.Resources;
using GeneralTools.Services;

namespace GeneralTools.Models
{
    public enum LogItemLevel { NotSet = 0, Info = 1, Warning = 2, Error = 3, Fatal = 4 } 

    [XmlRoot("LogItem", Namespace=null, IsNullable=false)]
    public class LogItem 
    {
        public string ID { get; set; }

        [LocalizedDisplay(LocalizeConstants.Date)]
        public DateTime? Date { get; set; }

        public string HostName { get; set; }

        [LocalizedDisplay(LocalizeConstants.WebApplication)]
        public string WebAppName { get; set; }

        [LocalizedDisplay(LocalizeConstants.Application)]
        public string DomainAppName { get; set; }

        [LocalizedDisplay(LocalizeConstants.Customer)]
        public string CustomerName { get; set; }

        [LocalizedDisplay(LocalizeConstants.UserName)]
        public string UserName { get; set; }

        [XmlIgnore]
        public LogItemLevel Level
        {
            get { return _level; }
            set
            {
                _level = value;
                _levelAsInt = (int)_level;
            }
        }

        [XmlIgnore]
        public string LevelAsString { get { return Level.ToString("F"); } }

        public string Group { get; set; }

        [LocalizedDisplay(LocalizeConstants.LogMeldung)]
        public string Message { get; set; }

        [LocalizedDisplay(LocalizeConstants.Error)]
        public string ExceptionMessage { get; set; }

        [LocalizedDisplay(LocalizeConstants._Morgens)]
        public bool TestMorgens { get; set; }

        [LocalizedDisplay(LocalizeConstants._Sommer)]
        public bool TestSommer { get; set; }

        [LocalizedDisplay(LocalizeConstants._Fruehling)]
        public bool TestFruehling { get; set; }

        [LocalizedDisplay(LocalizeConstants._Herbst)]
        public bool TestHerbst { get; set; }

        [LocalizedDisplay(LocalizeConstants._Winter)]
        public bool TestWinter { get; set; }

        [LocalizedDisplay(LocalizeConstants._Geburtsdatum)]
        public DateTime? TestGeburtsdatum { get; set; }

        [LocalizedDisplay(LocalizeConstants._Eintrittsdatum)]
        public DateTime? TestEintrittsdatum { get; set; }

        [XmlIgnore]
        public bool StackContextValid { get { return _StackContextXmlData.IsNotNullOrEmpty(); } }

        [XmlIgnore]
        [ScriptIgnore]
        public IStackContext StackContext
        {
            get
            {
                if (_stackContext == null && _StackContextXmlData.IsNotNullOrEmpty() && _StackContextType.IsNotNullOrEmpty())
                {
                    var type = Type.GetType(_StackContextType);
                    if (type != null)
                        _stackContext = (IStackContext)XmlService.XmlDeserializeFromString(_StackContextXmlData, type);
                }

                return _stackContext;
            }
            set
            {
                _stackContext = value;

                if (value == null)
                    return;

                // store the type of our object (only for XML Persistence purpose)
                var type = value.GetType();
                _StackContextType = type.GetFullTypeName();

                // store our object itself as xml string
                _StackContextXmlData = XmlService.XmlSerializeRawBulkToString(value, type);
            }
        }

        [XmlIgnore]
        public bool LogonContextValid { get { return _LogonContextXmlData.IsNotNullOrEmpty(); } }

        [XmlIgnore]
        [ScriptIgnore]
        public ILogonContext LogonContext
        {
            get
            {
                if (_logonContext == null && _LogonContextXmlData.IsNotNullOrEmpty() && _LogonContextType.IsNotNullOrEmpty())
                {
                    var type = Type.GetType(_LogonContextType);
                    if (type != null)
                        _logonContext = (ILogonContext)XmlService.XmlDeserializeFromString(_LogonContextXmlData, type);
                }

                return _logonContext;
            }
            set
            {
                _logonContext = value;

                if (value == null)
                    return;

                // store the type of our object (only for XML Persistence purpose)
                var type = value.GetType();
                _LogonContextType = type.GetFullTypeName();

                // store our object itself as xml string
                _LogonContextXmlData = XmlService.XmlSerializeRawBulkToString(value, type);
            }
        }

        [XmlIgnore]
        public bool DataContextValid { get { return _DataContextXmlData.IsNotNullOrEmpty(); } }

        [XmlIgnore]
        [ScriptIgnore]
        public object DataContext
        {
            get
            {
                if (_dataContext == null && _DataContextXmlData.IsNotNullOrEmpty() && _DataContextType.IsNotNullOrEmpty())
                {
                    var type = Type.GetType(_DataContextType);
                    if (type != null)
                        _dataContext = XmlService.XmlDeserializeFromString(_DataContextXmlData, type);
                }

                return _dataContext;
            }
            set
            {
                _dataContext = value;

                if (value == null)
                    return;
                
                // store the type of our object (only for XML Persistence purpose)
                var type = value.GetType();
                _DataContextType = type.GetFullTypeName();

                // store our object itself as xml string
                _DataContextXmlData = XmlService.XmlSerializeRawBulkToString(value, type);
            }
        }

        /// <summary>
        /// used for more detailed logging information in deeper scope (like SAP)
        /// points to anothe logitem that provides info about this scope.
        /// </summary>
        public string ChildLogItemID { get; set; }


        #region internal

        // ReSharper disable InconsistentNaming

        private object _dataContext;
        private IStackContext _stackContext;
        private ILogonContext _logonContext;
        private LogItemLevel _level;
        private int _levelAsInt;

        [ScriptIgnore]
        public string _StackContextType { get; set; }
        [ScriptIgnore]
        public string _StackContextXmlData { get; set; }

        [ScriptIgnore]
        public string _LogonContextType { get; set; }
        [ScriptIgnore]
        public string _LogonContextXmlData { get; set; }

        [ScriptIgnore]
        public string _DataContextType { get; set; }
        [ScriptIgnore]
        public string _DataContextXmlData { get; set; }

        public int _LevelAsInt
        {
            get { return _levelAsInt; }
            set
            {
                _levelAsInt = value;
                _level = (LogItemLevel)Enum.Parse(typeof(LogItemLevel), _levelAsInt.ToString(), true);
            }
        }

        // ReSharper restore InconsistentNaming

        #endregion
    }
}
