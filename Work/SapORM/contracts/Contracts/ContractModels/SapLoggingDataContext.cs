using System.Collections.Generic;
using System.Data;
using System.Text.RegularExpressions;
using System.Xml.Serialization;
using GeneralTools.Contracts;
using GeneralTools.Models;
using GeneralTools.Services;

namespace SapORM.Contracts
{
    public class SapLoggingDataContext : IUiView
    {
        [XmlIgnore]
        public SapConnectionWrapper SapConnection
        {
            get
            {
                if (_sapConnection == null && _sapConnectionXmlData.IsNotNullOrEmpty())
                {
                    _sapConnection = (SapConnectionWrapper)XmlService.XmlDeserializeFromString(_sapConnectionXmlData, typeof (SapConnectionWrapper));

                    // hide sensitive data:
                    _sapConnection = HideSensitiveData(_sapConnection);
                }

                return _sapConnection;
            }
            set
            {
                _sapConnection = value;

                if (value == null)
                    return;

                //
                // store our object itself as xml string
                //
                // hide sensitive data:
                _sapConnection = HideSensitiveData(_sapConnection);

                _sapConnectionXmlData = XmlService.XmlSerializeRawBulkToString(_sapConnection, typeof(SapConnectionWrapper));
            }
        }

        public string BapiName { get; set; }

        public DataTable InputParameters { get; set; }

        public List<DataTable> InputTables { get; set; }

        [XmlIgnore]
        public string ViewName { get { return "Partial/SapLoggingDataContext"; } }


        #region internal

        // ReSharper disable InconsistentNaming

        private SapConnectionWrapper _sapConnection;

        public string _sapConnectionXmlData { get; set; }

        // ReSharper restore InconsistentNaming

        static SapConnectionWrapper HideSensitiveData(SapConnectionWrapper sapConnectionWrapper)
        {
            if (sapConnectionWrapper == null)
                return null;

            sapConnectionWrapper.SAPPassword = "";
            var connStr = sapConnectionWrapper.SqlServerConnectionString;
            if (connStr.IsNotNullOrEmpty())
                sapConnectionWrapper.SqlServerConnectionString = Regex.Replace(connStr, "password=.*?;", "password=********;", RegexOptions.IgnoreCase);

            return sapConnectionWrapper;
        }

        #endregion
    }
}

