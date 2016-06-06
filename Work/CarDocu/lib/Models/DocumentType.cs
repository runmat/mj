using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Xml.Serialization;
using CarDocu.Services;
using GeneralTools.Models;
using GeneralTools.Services;
using Media = System.Windows.Media;

namespace CarDocu.Models
{
    public class DocumentType : ModelBase
    {
        static private List<string> _barcodeList;
        static public List<string> BarcodeListProp => (_barcodeList ?? (_barcodeList = new List<string> { "", "EAN_8", "EAN_13", "CODE_39", "CODE_128" }));

        private string _code;
        public string Code
        {
            get { return _code; }
            set { _code = value; SendPropertyChanged("Code"); }
        }

        private string _sapCode; 
        public string SapCode 
        { 
            get { return _sapCode; }
            set { _sapCode = value; SendPropertyChanged("SapCode"); }
        }

        private string _archiveCode = "ELO"; 
        public string ArchiveCode 
        { 
            get { return _archiveCode; }
            set
            {
                var preWebServiceFunctionAvailable = WebServiceFunctionAvailable;

                _archiveCode = value;
                
                SendPropertyChanged("ArchiveCode");
                SendPropertyChanged("IconSource");
                SendPropertyChanged("WebServiceFunctionAvailable");

                if (!WebServiceFunctionAvailable || (WebServiceFunctionAvailable && !preWebServiceFunctionAvailable))
                    WebServiceFunction = "";
            }
        }

        public string PdfEncryptionHashedPassword { get; set; }

        public string CodePrefix
        {
            get
            {
                var code = Code;
                if (code.IsNullOrEmpty() || code.Length < 1)
                    return Code;

                if (code[0] == '_')
                    code = Code.SubstringTry(1);

                if (code.Contains("_"))
                    return code.Split('_')[0];

                return code;
            }
        }

        private string _name; 
        public string Name 
        { 
            get { return _name; }
            set { _name = value; SendPropertyChanged("Name"); }
        }

        private int _sort; 
        public int Sort 
        { 
            get { return _sort; }
            set { _sort = value; SendPropertyChanged("Sort"); }
        }

        private string _inputRule = (InputRules.FirstOrDefault(i => i.ID == "ST") ?? InputRules.First()).ID;
        public string InputRule
        {
            get { return _inputRule; }
            set
            {
                _inputRule = value;
                SendPropertyChanged("InputRule");
                SendPropertyChanged("InputRuleName");
                SendPropertyChanged("ShowErrorBarcodeRangeStart");
                SendPropertyChanged("ShowErrorBarcodeRangeEnd");

                SendPropertyChanged("BarcodeRangeStartForeground");
                SendPropertyChanged("BarcodeRangeStartBackground");
                SendPropertyChanged("BarcodeRangeEndForeground");
                SendPropertyChanged("BarcodeRangeEndBackground");
            }
        }

        private string _webServiceFunction = (WebServiceFunctions.FirstOrDefault(i => i.ID == "") ?? WebServiceFunctions.First()).ID;
        public string WebServiceFunction
        {
            get { return _webServiceFunction; }
            set
            {
                _webServiceFunction = value;
                SendPropertyChanged("WebServiceFunction");
                SendPropertyChanged("WebServiceFunctionName");
            }
        }

        private bool _isSystemInternal; 
        public bool IsSystemInternal 
        { 
            get { return _isSystemInternal; }
            set { _isSystemInternal = value; SendPropertyChanged("IsSystemInternal"); }
        }

        private bool _isTemplate; 
        public bool IsTemplate 
        { 
            get { return _isTemplate; }
            set { _isTemplate = value; SendPropertyChanged("IsTemplate"); }
        }

        private bool _useFileNameAbbreviationMinimumLength; 
        public bool UseFileNameAbbreviationMinimumLength 
        { 
            get { return _useFileNameAbbreviationMinimumLength; }
            set { _useFileNameAbbreviationMinimumLength = value; SendPropertyChanged("UseFileNameAbbreviationMinimumLength"); }
        }

        private bool _isBatchScanAllowed;
        public bool IsBatchScanAllowed
        {
            get { return _isBatchScanAllowed; }
            set
            {
                _isBatchScanAllowed = value; 
                SendPropertyChanged("IsBatchScanAllowed");
                SendPropertyChanged("BarcodeNumericRangeVisible");
            }
        }

        private int _startBarcodeX = 1; 
        public int StartBarcodeX {
            get{return _startBarcodeX; }
            set{_startBarcodeX = value;SendPropertyChanged("StartBarcodeX"); }
        }

        private int _startBarcodeY = 1; 
        public int StartBarcodeY
        {
            get { return _startBarcodeY; }
            set { _startBarcodeY = value; SendPropertyChanged("StartBarcodeY"); }
        }

        private int _barcodeWidth = 210; //400;
        public int BarcodeWidth
        {
            get { return _barcodeWidth; }
            set { _barcodeWidth = value; SendPropertyChanged("BarcodeWidth"); }
        }

        private int _barcodeHeight = 70; //200;
        public int BarcodeHeight
        {
            get { return _barcodeHeight; }
            set { _barcodeHeight = value; SendPropertyChanged("BarcodeHeight"); }
        }

        public int TranslateXmmToPixel(int mm, int totalWidthPixel)
        {
            return (int)(mm * (double)totalWidthPixel / 211.0);      
        }

        public int TranslateYmmToPixel(int mm, int totalHeightPixel)
        {
            return (int)(mm * (double)totalHeightPixel / 297.0);     
        }

        private bool _isOcrAllowed;
        public bool IsOcrAllowed
        {
            get { return _isOcrAllowed; }
            set
            {
                _isOcrAllowed = value;

                if (value == false)
                    IsBatchScanAllowed = false;

                SendPropertyChanged("IsOcrAllowed");
            }
        }

        private bool _barcodeAlphanumericAllowed;
        public bool BarcodeAlphanumericAllowed
        {
            get { return _barcodeAlphanumericAllowed; }
            set
            {
                _barcodeAlphanumericAllowed = value;
                SendPropertyChanged("BarcodeAlphanumericAllowed");
                SendPropertyChanged("BarcodeNumericRangeVisible");
            }
        }
        
        public bool BarcodeNumericRangeVisible => IsBatchScanAllowed && !BarcodeAlphanumericAllowed;

        private long _barcodeRangeStart = 1000000000009;
        public long BarcodeRangeStart
        { 
            get { return _barcodeRangeStart; }
            set 
            {
                _barcodeRangeStart = value;
                SendPropertyChangedBarcodeStart();
            }
        }

        private long _barcodeRangeEnd = 1000000108781;
        public long BarcodeRangeEnd
        {
            get { return _barcodeRangeEnd; }
            set
            {
                _barcodeRangeEnd = value;
                SendPropertyChangedBarcodeEnd();
            }
        }

        private long _enforceExactPageCount;
        public long EnforceExactPageCount
        {
            get { return _enforceExactPageCount; }
            set
            {
                _enforceExactPageCount = value;
                SendPropertyChanged("EnforceExactPageCount");
            }
        }

        private bool _deleteAndBackupFileAfterDelivery;

        public bool DeleteAndBackupFileAfterDelivery
        {
            get
            {
                return _deleteAndBackupFileAfterDelivery;
            }
            set
            {
                _deleteAndBackupFileAfterDelivery = value;
                if (value && DomainService.Repository.GlobalSettings.BackupArchive.Path.IsNullOrEmpty())
                {
                    _deleteAndBackupFileAfterDelivery = false;
                    Tools.Alert("Diese Option kann erst aktiviert werden, wenn unter den Domain Einstellungen ein Backup Pfad hinterlegt wurde!");
                    return;
                }
                SendPropertyChanged("DeleteAndBackupFileAfterDelivery");
            }
        }

        private string _externalCommandlineProgramPath;

        public string ExternalCommandlineProgramPath
        {
            get { return _externalCommandlineProgramPath; }
            set
            {
                _externalCommandlineProgramPath = value;
                SendPropertyChanged("ExternalCommandlineProgramPath");
            }
        }

        public bool UseExternalCommandline => ExternalCommandlineProgramPath.IsNotNullOrEmpty();

        private string _externalCommandlineArguments;

        public string ExternalCommandlineArguments
        {
            get { return _externalCommandlineArguments; }
            set
            {
                _externalCommandlineArguments = value;
                SendPropertyChanged("ExternalCommandlineArguments");
            }
        }

        private string _inlineNetworkDeliveryArchiveFolder;

        public static bool IsCurrentylLoadingFromRepository { get; set; }

        public string InlineNetworkDeliveryArchiveFolder
        {
            get { return _inlineNetworkDeliveryArchiveFolder; }
            set
            {
                if (!IsCurrentylLoadingFromRepository && value.IsNotNullOrEmpty() && !FileService.TryDirectoryCreate(value))
                {
                    Tools.AlertError("Das Verzeichnis existiert nicht und konnte auch nicht erstellt werden!");
                    
                    SendPropertyChanged("InlineNetworkDeliveryArchiveFolder");
                    SendPropertyChanged("TagCollectionCheckBoxHint");
                    return;
                }

                _inlineNetworkDeliveryArchiveFolder = value;
                SendPropertyChanged("InlineNetworkDeliveryArchiveFolder");
                SendPropertyChanged("TagCollectionCheckBoxHint");
            }
        }

        public string InlineNetworkDeliveryTagsFolder
        {
            get
            {
                var tagsDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "_tags_scanclient");

                return tagsDirectory;
            }
        }

        bool _useTagCollectionForDocumentNameEditing;

        public bool UseTagCollectionForDocumentNameEditing
        {
            get { return _useTagCollectionForDocumentNameEditing; }
            set
            {
                _useTagCollectionForDocumentNameEditing = value;
                SendPropertyChanged("UseTagCollectionForDocumentNameEditing");
            }
        }

        public string TagCollectionCheckBoxHint =>
            $"Schlagwortliste für neue Dokumentennamen verwenden (Schlagwortliste wird hier gespeichert: {InlineNetworkDeliveryTagsFolder})";

        private readonly Media.Brush _brushBackgroundValid = Media.Brushes.LightGoldenrodYellow;
        private readonly Media.Brush _brushBackgroundInvalid = Media.Brushes.LightPink;
        private readonly Media.Brush _brushForegroundValid = Media.Brushes.Blue;
        private readonly Media.Brush _brushForegroundInvalid = Media.Brushes.Red;

        [XmlIgnore]
        public Media.Brush BarcodeRangeStartBackground => BarcodeRangeBackgroundOk(BarcodeRangeStart) ? _brushBackgroundValid : _brushBackgroundInvalid;

        [XmlIgnore]
        public Media.Brush BarcodeRangeStartForeground => BarcodeRangeBackgroundOk(BarcodeRangeStart) ? _brushForegroundValid : _brushForegroundInvalid;

        [XmlIgnore]
        public Media.Brush BarcodeRangeEndBackground => BarcodeRangeBackgroundOk(BarcodeRangeEnd) ? _brushBackgroundValid : _brushBackgroundInvalid;

        [XmlIgnore]
        public Media.Brush BarcodeRangeEndForeground => BarcodeRangeBackgroundOk(BarcodeRangeEnd) ? _brushForegroundValid : _brushForegroundInvalid;

        internal bool BarcodeRangeBackgroundOk(long barcode)
        {
            return InputRuleObject.AllowedLengths.Contains(barcode.ToString().Length);
        }

        private string _barcodeType = "EAN_8";
        public string BarcodeType {
            get { return _barcodeType; }
            set { _barcodeType = value; SendPropertyChanged("BarcodeType"); }
        }

        [XmlIgnore]
        public bool FileNameAbbreviationMinimumLengthAvailable => FileNameAbbreviationMinimumLength > 0;

        [XmlIgnore]
        public int FileNameAbbreviationMinimumLength => InputRuleObject?.FileNameAbbreviationAllowedMinimumLength ?? 0;

        [XmlIgnore]
        public string FileNameAbbreviationMinimumLengthText => $"Länge des Dateinamens auf {FileNameAbbreviationMinimumLength} Zeichen kürzen";

        [XmlIgnore]
        public DocumentTypeInputRule InputRuleObject { get { return InputRules.First(i => i.ID == InputRule); } }

        public string InputRuleName => InputRuleObject.InputRuleName;

        [XmlIgnore]
        static public List<DocumentTypeInputRule> InputRules => new List<DocumentTypeInputRule>
        {
            new DocumentTypeInputRule
            {
                ID = "FG",  Name = "FIN", 
                AllowedLengths = new List<int> {10, 17}, 
                FileNameAbbreviationAllowedMinimumLength = 10
            },
            new DocumentTypeInputRule
            {
                ID = "ST",  Name = "Strafzettel", 
                AllowedLengths = new List<int> {13}
            },
            new DocumentTypeInputRule
            {
                ID = "ST8",  Name = "Strafzettel", 
                AllowedLengths = new List<int> {8}
            },
            new DocumentTypeInputRule
            {
                ID = "TP",  Name = "Scan-Template", 
                AllowedLengths = new List<int>(), 
                InputRuleName = "Beliebiger Freitext:"
            },
        };

        [XmlIgnore]
        public DocumentTypeWebServiceFunction WebServiceFunctionObject { get { return WebServiceFunctions.First(i => i.ID == WebServiceFunction); } }

        [XmlIgnore]
        public bool WebServiceFunctionAvailable => ArchiveCode == "EASY";

        [XmlIgnore]
        public bool IsDisabledGlobalDeleteAndBackupFileAfterDelivery => !IsEnabledGlobalDeleteAndBackupFileAfterDelivery;

        [XmlIgnore]
        public bool IsEnabledGlobalDeleteAndBackupFileAfterDelivery => DomainService.Repository.AppSettings.GlobalDeleteAndBackupFileAfterDelivery;

        [XmlIgnore]
        public double OpacityGlobalDeleteAndBackupFileAfterDelivery => IsDisabledGlobalDeleteAndBackupFileAfterDelivery ? 1.0 : 0.4;

        [XmlIgnore]
        static public List<DocumentTypeWebServiceFunction> WebServiceFunctions => new List<DocumentTypeWebServiceFunction>
        {
            new DocumentTypeWebServiceFunction
            {
                ID = "", FriendlyName = "(Keine Schnittstelle)"
            },
            new DocumentTypeWebServiceFunction
            {
                ID = "CARDOCU", FriendlyName = "CarDocu Strafzettel"
            },
            new DocumentTypeWebServiceFunction
            {
                ID = "VWL", FriendlyName = "VW Leasing Klärfälle"
            },
            new DocumentTypeWebServiceFunction
            {
                ID = "WKDA", FriendlyName = "WKDA Wiesbaden"
            },
        };

        [XmlIgnore]
        static public List<Archive> Archives => DomainService.Repository.GlobalSettings.ArchivesForDocTypes;

        private Archive _archive;
        [XmlIgnore] 
        public Archive Archive
        {
            get
            {
                return (_archive ?? Archives.FirstOrDefault(archives => archives.ID == ArchiveCode));
            }
            set { _archive = value; }
        }

        [XmlIgnore]
        public string IconSource => ((Image)Application.Current.TryFindResource(Archive.GetIconSourceKey(ArchiveCode))).Source.ToString();

        [XmlIgnore]
        public string IsBatchScanAllowedText => "Aktiviert die Batchverarbeitung für diesen Dokumententyp.";

        [XmlIgnore]
        public string IsBatchScanAllowedTooltip => "Batchverarbeitung für diesen Dokumententyp ist aktiviert.";

        [XmlIgnore]
        public string IsOcrAllowedText => "Aktiviert die Barcode-Erkennung für diesen Dokumententyp.";

        [XmlIgnore]
        public string BarcodeAlphanumericAllowedText => "Alphanumerischer Barcode erlaubt (ansonsten rein numerisch).";

        [XmlIgnore]
        public bool ShowErrorBarcodeRangeStart => !BarcodeRangeBackgroundOk(BarcodeRangeStart);

        [XmlIgnore]
        public string ErrorTextBarcodeRangeStart => "Barcode hat die falsche Länge!";

        [XmlIgnore]
        public bool ShowErrorBarcodeRangeEnd => !BarcodeRangeBackgroundOk(BarcodeRangeEnd);

        [XmlIgnore]
        public string ErrorTextBarcodeRangeEnd => "Barcode hat die falsche Länge!";


        /// <summary>
        /// Prüft ob die angegebene Fin innerhalb des aktuellen Nummernkreises des Dokumententyps liegt.
        /// </summary>
        /// <param name="finNumber">Zu prüfende FIN/ Barcode/ ID</param>
        /// <returns>True, wenn in Range</returns>
        internal bool IsBarcodeInRange(long finNumber)
        {
            if (finNumber >= BarcodeRangeStart && finNumber <= BarcodeRangeEnd)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Prüft ob die angegebene Fin innerhalb des aktuellen Nummernkreises des Dokumententyps liegt.
        /// </summary>
        /// <param name="finNumber">Zu prüfende FIN/ Barcode/ ID</param>
        /// <returns>True, wenn in Range</returns>
        internal bool IsBarcodeInRange(string finNumber)
        {
            long longFinNumber;
            if (!long.TryParse(finNumber, out longFinNumber))
                return false;
            
            return IsBarcodeInRange(long.Parse(finNumber));
        }

        /// <summary>
        /// Führt SendPropertyChanged für alle mit BarcodeStart verbunden Objekte aus
        /// </summary>
        internal void SendPropertyChangedBarcodeStart()
        {
            SendPropertyChanged("BarcodeRangeStart");
            SendPropertyChanged("BarcodeRangeStartBackground");
            SendPropertyChanged("BarcodeRangeStartForeground");
            SendPropertyChanged("ShowErrorBarcodeRangeStart");
        }

        /// <summary>
        /// Führt SendPropertyChanged für alle mit BarcodeEnd verbunden Objekte aus
        /// </summary>
        internal void SendPropertyChangedBarcodeEnd()
        {
            SendPropertyChanged("BarcodeRangeEnd");
            SendPropertyChanged("BarcodeRangeEndBackground");
            SendPropertyChanged("BarcodeRangeEndForeground");
            SendPropertyChanged("ShowErrorBarcodeRangeEnd");
        }

        public void SendPropertyChangedGlobalSettings()
        {
            SendPropertyChanged("IsDisabledGlobalDeleteAndBackupFileAfterDelivery");
            SendPropertyChanged("IsEnabledGlobalDeleteAndBackupFileAfterDelivery");
            SendPropertyChanged("OpacityGlobalDeleteAndBackupFileAfterDelivery");
            SendPropertyChanged("DeleteAndBackupFileAfterDelivery");
        }
    }
}
