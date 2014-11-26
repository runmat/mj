using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using CKG.Base.Business;
using CKG.Base.Kernel.Security;
using CKG.Base.Common;
using System.Web.UI;
using System.Text;
using System.ComponentModel;
using GeneralTools.Services;

namespace CKG.Components.Zulassung.DAL
{
    public class AddressEventArgs : EventArgs
    {
        string kennung;

        public AddressEventArgs(string kennung)
        {
            this.kennung = kennung;
        }

        public string Kennung
        {
            get { return kennung; }
            set { kennung = value; }
        }


    }

    public enum VersandAdressTyp
    {
        Halter,
        Auftraggeber,
        Anderer,
    }

    public class ZulassungDal : DatenimportBase, INotifyPropertyChanged
    {
        public const string AuswahlCol = "AUSWAHL";
        public const string HalterKennung = "HALTER";
        public const string VersichererKennung = "VERSICHERER";
        public const string VersandKennung = "SSCHILDER";
        public const string EvbKennung = "EVB";
        private const string ZulassungsartKennung = "ZULASSUNGSART";
        private const string AuftraggeberKennung = "AUFTRAGGEBER";
        private const string ZFAHRZEUGART_G = "gebraucht";

        public event PropertyChangedEventHandler PropertyChanged;
        private Page page;
        private DataTable countries;
        private DataTable customerCountries;
        private DataTable vehicles;
        private DataView selectedVehicles;
        private DataTable services;
        private DataTable selectedServices;
        private DataTable protokollarten;
        private DataTable partner;
        private AddressData halterAddress;
        private AddressData versichererAddress;
        private AddressData versandAddress;
        private AddressData auftraggeberAddress;
        private string evbNo;
        private AddressData versicherungsnehmerAddress;
        private string stva;
        private string kundenDebitorNummer;
        private string buchungscode;
        private string auftragsnummer = string.Empty;
        private object syncRoot = new object();
        private string defaultZulassungsart = null;
        private DateTime? versicherungFrom = DateTime.MinValue;
        private DateTime? versicherungUntil = DateTime.MaxValue;
        private DateTime zulassungsDate = DateTime.MinValue;
        private string zulassungsKreis = string.Empty;
        private string zulassungsTyp = string.Empty;
        private List<string> documents = new List<string>();
       

        public string ErrMsg
        {
            get;
            set;
        }

        public ZulassungDal(ref User user, App app, string filename, Page page)
            : base(ref user, app, filename)
        {
            this.page = page;
            SessionID = page.Session.SessionID;
            AppID = page.Session["AppID"] as string;
            this.SelectedCountry = "DE";
            this.Ersteller = m_objUser.UserName;
            this.KNDNummer = m_objUser.KUNNR;  
      
        }

        #region public properties

        public DataTable Countries
        {
            get
            {
                lock (syncRoot)
                {
                    if (this.countries == null)
                    {
                        this.countries = GetCountries();
                    }
                }
                return this.countries;
            }
        }

        public string SelectedCountry { get; set; }

        public DataTable CustomerCountries
        {
            get
            {
                lock (syncRoot)
                {
                    if (this.customerCountries == null)
                    {
                        this.customerCountries = GetCustomerCountries();
                    }
                }
                return this.customerCountries;
            }
        }

        public DataTable Services
        {
            get
            {
                lock (syncRoot)
                {
                    if (this.services == null)
                    {
                        this.services = GetServices();
                    }
                }
                return this.services;
            }
        }

        public DataTable SelectedServices
        {
            get
            {
                lock (syncRoot)
                {
                    return this.selectedServices;
                }
            }
            set
            {
                lock (syncRoot)
                {
                    this.selectedServices = value;
                }
            }
        }

        public DataTable Protokollarten
        {
            get
            {
                lock (syncRoot)
                {
                    if (this.protokollarten == null)
                    {
                        this.protokollarten = GetProtokollarten() ?? new DataTable();
                        if (!this.protokollarten.Columns.Contains("ID"))
                        {
                            this.protokollarten.Columns.Add("ID", System.Type.GetType("System.String"));
                            this.protokollarten.Columns.Add("Filename", System.Type.GetType("System.String"));
                        }
                    }
                }
                return this.protokollarten;
            }
        }

        public string DefaultZulassungsart
        {
            get
            {
                lock (syncRoot)
                {
                    if (this.defaultZulassungsart == null)
                    {
                        this.defaultZulassungsart = GetDefaultZulassungsart();
                    }
                    return this.defaultZulassungsart;
                }
            }
        }

        public DataTable Vehicles
        {
            get
            {
                return this.vehicles;
            }
            set
            {
                this.vehicles = value;
                OnPropertyChanged("Vehicles");
            }
        }

        public DataTable Partner
        {
            get { 
                if(partner==null )
                {
                    FillPartner(); 
                }
                return partner;
            }
            set{ this.partner = value;}
        }

        public DataView SelectedVehicles
        {
            get
            {
                if (this.vehicles != null)
                {
                    //if (this.selectedVehicles == null)
                    //{
                        this.selectedVehicles = new DataView(this.vehicles);
                        this.selectedVehicles.RowFilter = AuswahlCol +" = '99'";
                    //}
                    return this.selectedVehicles;
                }
                return null;
            }
        }

        public AddressData VersicherungsnehmerAddress
        {
            get { return this.versicherungsnehmerAddress; }
            set
            {
                if (this.versicherungsnehmerAddress != value)
                {
                    this.versicherungsnehmerAddress = value;
                    OnPropertyChanged("VersicherungsnehmerAddress");
                }
            }
        }

        public AddressData HalterAddress
        {
            get { return this.halterAddress; }
            set
            {
                if (this.halterAddress != value)
                {
                    this.halterAddress = value;
                    OnPropertyChanged("HalterAddress");
                    this.KundenDebitorNummer = null;
                }
            }
        }

        public AddressData VersichererAddress
        {
            get { return this.versichererAddress; }
            set
            {
                if (this.versichererAddress != value)
                {
                    this.versichererAddress = value;
                    OnPropertyChanged("VersichererAddress");
                }
            }
        }

        public AddressData VersandAddress
        {
            get { return this.versandAddress; }
            set
            {
                if (this.versandAddress != value)
                {
                    this.versandAddress = value;

                    if (value != null)
                    {
                        this.VersandAdressTyp = DAL.VersandAdressTyp.Anderer;
                    }

                    OnPropertyChanged("VersandAddress");
                }
            }
        }

        public AddressData AuftraggeberAddress
        {
            get
            {
                lock (syncRoot)
                {
                    if (this.auftraggeberAddress == null)
                    {
                        this.auftraggeberAddress = GetAuftraggeberAddress();
                    }
                }
                return this.auftraggeberAddress;
            }
            set
            {
                if (this.auftraggeberAddress != value)
                {
                    this.auftraggeberAddress = value;
                    OnPropertyChanged("AuftraggeberAddress");
                }
            }
        }

        public VersandAdressTyp VersandAdressTyp { get; set; }

        public string EvbNo
        {
            get { return this.evbNo; }
            set
            {
                if (!String.Equals(this.evbNo, value, StringComparison.CurrentCultureIgnoreCase))
                {
                    this.evbNo = value;
                    OnPropertyChanged("EvbNo");
                }
            }
        }

        public DateTime? VersicherungFrom
        {
            get { return this.versicherungFrom; }
            set
            {
                if (this.versicherungFrom != value)
                {
                    this.versicherungFrom = value;
                    OnPropertyChanged("VersicherungFrom");
                }
            }
        }

        public DateTime? VersicherungUntil
        {
            get { return this.versicherungUntil; }
            set
            {
                if (this.versicherungUntil != value)
                {
                    this.versicherungUntil = value;
                    OnPropertyChanged("VersicherungUntil");
                }
            }
        }

        public DateTime ZulassungsDate
        {
            get { return this.zulassungsDate; }
            set
            {
                if (this.zulassungsDate != value)
                {
                    this.zulassungsDate = value;
                    OnPropertyChanged("ZulassungsDate");
                }
            }
        }

        public string ZulassungsTyp
        {
            get { return this.zulassungsTyp; }
            set
            {
                if (!String.Equals(this.zulassungsTyp, value, StringComparison.CurrentCultureIgnoreCase))
                {
                    this.zulassungsTyp = value;
                    OnPropertyChanged("ZulassungsTyp");
                }
            }
        }

        public string ZulassungsKreis
        {
            get { return this.zulassungsKreis; }
            set
            {
                if (!String.Equals(this.zulassungsKreis, value, StringComparison.CurrentCultureIgnoreCase))
                {
                    this.zulassungsKreis = value;
                    OnPropertyChanged("ZulassungsKreis");
                }
            }
        }

        public string Stva
        {
            get { return this.stva; }
            private set
            {
                if (!String.Equals(this.stva, value, StringComparison.CurrentCultureIgnoreCase))
                {
                    this.stva = value;
                    OnPropertyChanged("Stva");
                }
            }
        }

        public string KundenDebitorNummer
        {
            get { return this.kundenDebitorNummer; }
            set
            {
                if (!String.Equals(this.kundenDebitorNummer, value, StringComparison.CurrentCultureIgnoreCase))
                {
                    this.kundenDebitorNummer = value;
                    this.OnPropertyChanged("KundenDebitorNummer");
                }
            }
        }

        public string Buchungscode
        {
            get { return this.buchungscode; }
            set
            {
                if (!String.Equals(this.buchungscode, value, StringComparison.CurrentCultureIgnoreCase))
                {
                    this.buchungscode = value;
                    this.OnPropertyChanged("Buchungscode");
                }
            }
        }

        public string Auftragsnummer
        {
            get { return this.auftragsnummer; }
            set
            {
                if (!String.Equals(this.auftragsnummer, value, StringComparison.CurrentCultureIgnoreCase))
                {
                    this.auftragsnummer = value;
                    this.OnPropertyChanged("Auftragsnummer");
                }
            }
        }

        public string Empfänger{ get; set; }

        public string Regulierer { get; set; }

        public string Ersteller { get; set; }

        public string KNDNummer { get; set; }

        #endregion

        #region public methods

        public bool AddVehicle(VehicleSearchCriterias criterias)
        {
            lock (this.syncRoot)
            {
                try
                {
                    var table = this.vehicles;

                    if (table == null)
                    {
                        var sapProxy = DynSapProxy.getProxy("Z_DPM_READ_EQUI_002", ref m_objApp, ref m_objUser, ref page);
                        table = sapProxy.getExportTable("GT_OUT");
                        this.AddVehicleColumns(table);
                    }

                    var row = table.NewRow();
                    row[AuswahlCol] = "99";
                    row["DummyEqui"] = "ja";
                    row["Bem"] = string.Empty;
                    row["EQUNR"] = Guid.NewGuid().ToString("N");
                    row["CHASSIS_NUM"] = criterias.ChassisNumber;
                    row["LIZNR"] = criterias.Contract;
                    row["TIDNR"] = criterias.ZB2No;

                    if (!table.Columns.Contains("ZFAHRZEUGART"))
                    {
                        table.Columns.Add("ZFAHRZEUGART");
                    }

                    table.Rows.Add(row);


                    this.Vehicles = table;
                    m_intStatus = 0;

       

                    return true;
                }
                catch
                {
                    m_strMessage = "Unbekannter Fehler.";
                    m_intStatus = -9999;
                }
                return false;
            }
        }

        public bool FindVehicles(params VehicleSearchCriterias[] searchCriteria)
        {
            lock (this.syncRoot)
            {
                bool blnFound = false;
                m_intStatus = 0;
                try
                {
                    var table = this.vehicles;

                    // In Equi-Tabelle suchen
                    var sapProxy = DynSapProxy.getProxy("Z_DPM_READ_EQUI_002", ref m_objApp, ref m_objUser, ref page);

                    // ggf. Tabellenspalten ermitteln
                    if (table == null)
                    {
                        table = sapProxy.getExportTable("GT_OUT");
                        this.AddVehicleColumns(table);

                        if (!table.Columns.Contains("ZFAHRZEUGART"))
                        {
                            table.Columns.Add("ZFAHRZEUGART");
                        }
                    }

                    sapProxy.setImportParameter("I_KUNNR_AG", m_objUser.KUNNR.PadLeft(10, '0'));

                    var importTable = sapProxy.getImportTable("GT_FIN");
                    var importTable2 = sapProxy.getImportTable("GT_LIZNR");
                    var importTable3 = sapProxy.getImportTable("GT_TIDNR");

                    if ((searchCriteria.Length == 1) && String.IsNullOrEmpty(searchCriteria[0].ChassisNumber)
                                                     && String.IsNullOrEmpty(searchCriteria[0].ZB2No)
                                                     && !String.IsNullOrEmpty(searchCriteria[0].Contract))
                    {
                        sapProxy.setImportParameter("I_LIZNR", searchCriteria[0].Contract);
                    }
                    else
                    {
                        foreach (var searchCriterion in searchCriteria)
                        {
                            if (!string.IsNullOrEmpty(searchCriterion.ChassisNumber))
                            {
                                var row = importTable.NewRow();
                                row["CHASSIS_NUM"] = searchCriterion.ChassisNumber;
                                importTable.Rows.Add(row);
                            }

                            if (!string.IsNullOrEmpty(searchCriterion.Contract))
                            {
                                var row = importTable2.NewRow();
                                row["LIZNR"] = searchCriterion.Contract;
                                importTable2.Rows.Add(row);
                            }

                            if (!string.IsNullOrEmpty(searchCriterion.ZB2No))
                            {
                                var row = importTable3.NewRow();
                                row["TIDNR"] = searchCriterion.ZB2No;
                                importTable3.Rows.Add(row);
                            }
                        }
                    }

                    sapProxy.callBapi();

                    m_intStatus = int.Parse(sapProxy.getExportParameter("E_SUBRC"));
                    m_strMessage = sapProxy.getExportParameter("E_MESSAGE");

                    var result = sapProxy.getExportTable("GT_OUT");

                    if ((result != null) && (result.Rows.Count > 0))
                    {
                        this.AddVehicleColumns(result);

                        foreach (DataRow row in result.Rows)
                        {
                            row[AuswahlCol] = string.Empty;
                            row["DummyEqui"] = "nein";
                            row["ZFAHRZEUGART"] = "G".Equals(row["ZFAHRZEUGART"]) ? ZFAHRZEUGART_G : string.Empty;
                        }

                        result.AcceptChanges();

                        if (this.vehicles == null)
                        {
                            this.Vehicles = result;
                        }
                        else
                        {
                            // Keep selected rows and merge with current search result.
                            for (int i = this.vehicles.Rows.Count - 1; i >= 0; i--)
                            {
                                var row = this.vehicles.Rows[i];
                                if (((string) row[AuswahlCol]).Length == 0)
                                {
                                    this.vehicles.Rows.Remove(row);
                                }
                                else
                                {
                                    // bereits vorhandene nicht erneut hinzufügen
                                    foreach (DataRow dRow in result.Rows)
                                    {
                                        if (dRow["CHASSIS_NUM"] == row["CHASSIS_NUM"])
                                        {
                                            this.vehicles.Rows.Remove(row);
                                            break;
                                        }
                                    }
                                }
                            }
                            result.Merge(this.vehicles);
                            result.AcceptChanges();

                            this.Vehicles = result;
                            table = this.vehicles;
                        }
                        blnFound = true;
                    }

                    // In CoC-Tabelle suchen
                    var sapProxyCoc = DynSapProxy.getProxy("Z_DPM_READ_COC_01", ref m_objApp, ref m_objUser, ref page);

                    sapProxyCoc.setImportParameter("I_AG", m_objUser.KUNNR.PadLeft(10, '0'));
                    sapProxyCoc.setImportParameter("I_AUFTRAG_DAT_INIT", "X");

                    if (searchCriteria.Length > 0)
                    {
                        if (!String.IsNullOrEmpty(searchCriteria[0].ChassisNumber))
                        {
                            sapProxyCoc.setImportParameter("I_FIN", searchCriteria[0].ChassisNumber);
                        }
                        if (!String.IsNullOrEmpty(searchCriteria[0].Contract))
                        {
                            sapProxyCoc.setImportParameter("I_AUFTR_NR_KD", searchCriteria[0].Contract);
                        }
                    }

                    sapProxyCoc.callBapi();

                    var resultCoc = sapProxyCoc.getExportTable("GT_OUT");

                    if (resultCoc != null)
                    {
                        for (int j = resultCoc.Rows.Count - 1; j >= 0; j--)
                        {
                            var rowCoc = resultCoc.Rows[j];

                            var bereitsVorhanden = false;

                            for (int k = table.Rows.Count - 1; k >= 0; k--)
                            {
                                if (table.Rows[k]["CHASSIS_NUM"].ToString() == rowCoc["VIN"].ToString())
                                {
                                    bereitsVorhanden = true;
                                    break;
                                }
                            }

                            // Nur hinzufügen, wenn noch nicht in bisherigen Suchergebnissen enthalten
                            if (!bereitsVorhanden)
                            {
                                if ((rowCoc["LAND"] != DBNull.Value) &&
                                    (!String.IsNullOrEmpty(rowCoc["LAND"].ToString())))
                                {
                                    this.SelectedCountry = rowCoc["LAND"].ToString().ToUpper();
                                }
                                if ((rowCoc["AUSLIEFER_DATUM"] != DBNull.Value) &&
                                    (!String.IsNullOrEmpty(rowCoc["AUSLIEFER_DATUM"].ToString())))
                                {
                                    this.ZulassungsDate = (DateTime) rowCoc["AUSLIEFER_DATUM"];
                                }

                                var newRow = table.NewRow();
                                newRow[AuswahlCol] = "99";
                                newRow["DummyEqui"] = "ja";
                                newRow["Bem"] = string.Empty;
                                newRow["EQUNR"] = Guid.NewGuid().ToString("N");
                                newRow["CHASSIS_NUM"] = rowCoc["VIN"];
                                newRow["LIZNR"] = rowCoc["AUFTR_NR_KD"];
                                newRow["TIDNR"] = "";
                                table.Rows.Add(newRow);
                            }
                        }

                        table.AcceptChanges();

                        this.Vehicles = table;

                        blnFound = true;
                    }
                }
                catch (Exception ex)
                {
                    if (!HelpProcedures.CastSapBizTalkErrorMessage(ex.Message).Equals("NO_DATA", StringComparison.Ordinal))
                    {
                        m_strMessage = "Unbekannter Fehler.";
                        m_intStatus = -9999;
                    }
                }
                if (!blnFound)
                {
                    this.Vehicles = null;
                }
                return blnFound;
            }
        }
       
        public void ExtractAddressAndInsuranceData()
        {
            if (this.SelectedVehicles.Count == 1)
            {
                var row = this.SelectedVehicles.Cast<DataRowView>().First();

                if ((row["ZF_NAME1"] != DBNull.Value) && this.VersandAddress == null)
                {
                    this.VersandAddress = new AddressData()
                    {
                        Name1 = (string)row["ZF_NAME1"],
                        Name2 = row["ZF_NAME2"] == DBNull.Value ? string.Empty : (string)row["ZF_NAME2"],
                        Street = row["ZF_STRASSE"] == DBNull.Value ? string.Empty : (string)row["ZF_STRASSE"],
                        ZipCode = row["ZF_PLZ"] == DBNull.Value ? string.Empty : (string)row["ZF_PLZ"],
                        City = row["ZF_ORT"] == DBNull.Value ? string.Empty : (string)row["ZF_ORT"],
                    };
                }

                if ((row["ZH_NAME1"] != DBNull.Value) && this.HalterAddress == null)
                {
                    this.HalterAddress = new AddressData()
                    {
                        Name1 = (string)row["ZH_NAME1"],
                        Name2 = row["ZH_NAME2"] == DBNull.Value ? string.Empty : (string)row["ZH_NAME2"],
                        Street = row["ZH_STRASSE"] == DBNull.Value ? string.Empty : (string)row["ZH_STRASSE"],
                        ZipCode = row["ZH_PLZ"] == DBNull.Value ? string.Empty : (string)row["ZH_PLZ"],
                        City = row["ZH_ORT"] == DBNull.Value ? string.Empty : (string)row["ZH_ORT"],
                    };
                }

                if ((row["ZEVBNR"] != DBNull.Value) && String.IsNullOrEmpty(this.EvbNo))
                {
                    this.EvbNo = (string)row["ZEVBNR"];
                }

                if (row["ZH_KUNNR"] != DBNull.Value)
                {
                    this.KundenDebitorNummer = (string)row["ZH_KUNNR"];
                }
            }
        }

        public void UpdateStva(string postcode, string town)
        {
            try
            {
                var sapProxy = DynSapProxy.getProxy("Z_Get_Zulst_By_Plz", ref m_objApp, ref m_objUser, ref page);

                sapProxy.setImportParameter("I_PLZ", postcode);
                sapProxy.setImportParameter("I_ORT", town);

                sapProxy.callBapi();

                var table = sapProxy.getExportTable("T_ZULST");
                if (table != null && table.Rows.Count > 0)
                {
                    this.Stva = table.Rows[0].Field<string>("ZKFZKZ");
                    return;
                }
            }
            catch (Exception ex)
            {
                switch (HelpProcedures.CastSapBizTalkErrorMessage(ex.Message))
                {
                    case "ERR_INV_PLZ":
                        m_strMessage = "Ungültige Postleitzahl.";
                        m_intStatus = -1118;
                        break;
                    default:
                        m_strMessage = "Unbekannter Fehler.";
                        m_intStatus = -9999;
                        break;
                }
            }

            this.Stva = null;
        }

        public string Submit()
        {
            m_intStatus = 0;
            m_strMessage = "";
            this.ErrMsg = "";

            string strKUNNR = m_objUser.Customer.KUNNR.PadLeft(10, '0');

            try
            {
                string bapiName = ApplicationConfiguration.GetApplicationConfigValue("ZulassungsbeauftragungSpeicherBapi", AppID, m_objUser.Customer.CustomerId);

                if (String.IsNullOrEmpty(bapiName))
                {
                    //default, falls gar nichts konfiguriert wurde
                    bapiName = "Z_DPM_WEB_ZULASSUNG_01";
                }

                DynSapProxyObj myProxy = DynSapProxy.getProxy(bapiName, ref m_objApp, ref m_objUser, ref page);
                myProxy.setImportParameter("AG", strKUNNR);
                myProxy.setImportParameter("WEB_USER", m_objUser.UserName);

                var importZul = myProxy.getImportTable("GT_AUF");
                var partnerZul = myProxy.getImportTable("GT_PARTNER");
                var dienstlZul = myProxy.getImportTable("GT_DIENSTL");
                var dokZul = myProxy.getImportTable("GT_DOK");

                foreach (DataRowView row in SelectedVehicles)
                {
                    var importRow = importZul.NewRow();

                    string wunschkennzeichenData = string.Empty;

                    if (!string.IsNullOrEmpty(row["Wunschkennz1"] as string))
                    {
                        wunschkennzeichenData = (string)row["Wunschkennz1"];
                        if (!string.IsNullOrEmpty(row["Wunschkennz2"] as string))
                        {
                            wunschkennzeichenData = wunschkennzeichenData + "," + (string)row["Wunschkennz2"];
                            if (!string.IsNullOrEmpty(row["Wunschkennz3"] as string))
                            {
                                wunschkennzeichenData = wunschkennzeichenData + "," + (string)row["Wunschkennz3"];

                            }
                        }

                    }
                    //Reservierungsname und -nummer in das Wunschkennzeichen eintragen
                    if (!string.IsNullOrEmpty(row["ResNr"] as string))
                    {
                        if (wunschkennzeichenData != string.Empty)
                        {
                            wunschkennzeichenData = wunschkennzeichenData + ", " + (string)row["ResNr"];
                        }
                        else
                        {
                            wunschkennzeichenData = (string)row["ResNr"];
                        }
                    }

                    if (!string.IsNullOrEmpty(row["ResName"] as string))
                    {
                        if (wunschkennzeichenData != string.Empty)
                        {
                            wunschkennzeichenData = wunschkennzeichenData + ", " + (string)row["ResName"];
                        }
                        else
                        {
                            wunschkennzeichenData = (string)row["ResName"];
                        }
                    }

                    importRow["EQUNR"] = ((string)row["DummyEqui"]).Equals("ja") ? null : (string)row["EQUNR"];
                    importRow["ZZFAHRG"] = (string)row["CHASSIS_NUM"];
                    importRow["ZZBRIEF"] = (string)row["TIDNR"];
                    importRow["ZZREFNR"] = (string)row["LIZNR"]; 
                    importRow["ZFAHRZEUGART"] = ZFAHRZEUGART_G.Equals(row["ZFAHRZEUGART"]) ? "G" : string.Empty;

                    importRow["ZULDAT"] = ZulassungsDate.ToShortDateString();
                    importRow["EVBNR"] = evbNo;
                    if (this.VersicherungFrom.HasValue)
                    {
                        importRow["EVBVONDAT"] = VersicherungFrom.Value.ToShortDateString();
                    }
                    if (this.VersicherungUntil.HasValue)
                    {
                        importRow["EVBBISDAT"] = versicherungUntil.Value.ToShortDateString();
                    }
                    importRow["WUNSCHKENNZ"] = wunschkennzeichenData;
                    importRow["IHREZ_E"] = Buchungscode;
                    importRow["SFV_FZG"] = ZulassungsTyp;
                    importRow["VERSICHERUNG"] = versichererAddress.Name1;
                    importRow["TERMINHINWEIS"] = "";
                    importRow["FEINSTAUBPL"] = ""; //TODO: Map Dienstleistung
                    importRow["STEUERN"] = "";
                    importRow["EXKUNNR_ZL"] = "";
                    importRow["KVGR3"] = "";
                    if (this.HalterAddress != null)
                    {
                        if (this.HalterAddress.Country.ToUpper() == "DE")
                        {
                            importRow["ZUL_DEZ"] = "1";
                            importRow["ZUL_AUSLAND"] = "0";
                            importRow["ZUL_EXPORT"] = "0";
                        }
                        else
                        {
                            importRow["ZUL_DEZ"] = "0";
                            importRow["ZUL_AUSLAND"] = "1";
                            importRow["ZUL_EXPORT"] = "0";
                        }
                    }
                    else
                    {
                        importRow["ZUL_DEZ"] = "1";
                        importRow["ZUL_AUSLAND"] = "0";
                        importRow["ZUL_EXPORT"] = "0";
                    }

                    importZul.Rows.Add(importRow);

                    //### Partner Zulassung ###
                    var partnerZulRow = partnerZul.NewRow();

                    //Rechnungsemfänger
                    partnerZulRow = partnerZul.NewRow();

                    partnerZulRow["ZZFAHRG"] = (string)row["CHASSIS_NUM"];
                    partnerZulRow["ZZBRIEF"] = (string)row["TIDNR"];
                    partnerZulRow["ZZREFNR"] = (string)row["LIZNR"];
                    partnerZulRow["PARTN_ROLE"] = "RE";

                    if (string.IsNullOrEmpty(Empfänger))
                    {
                        Empfänger = strKUNNR;
                    }
                    partnerZulRow["PARTN_NUMB"] = Empfänger.PadLeft(10, '0');
                    partnerZul.Rows.Add(partnerZulRow);

                    //Regulierer
                    partnerZulRow = partnerZul.NewRow();

                    partnerZulRow["ZZFAHRG"] = (string)row["CHASSIS_NUM"];
                    partnerZulRow["ZZBRIEF"] = (string)row["TIDNR"];
                    partnerZulRow["ZZREFNR"] = (string)row["LIZNR"];
                    partnerZulRow["PARTN_ROLE"] = "RG";

                    if (string.IsNullOrEmpty(Regulierer))
                    {
                        Regulierer = strKUNNR;
                    }
                    partnerZulRow["PARTN_NUMB"] = Regulierer.PadLeft(10, '0');
                    partnerZul.Rows.Add(partnerZulRow);

                    if (HalterAddress != null)
                    {
                        partnerZulRow = partnerZul.NewRow();

                        //Halter
                        partnerZulRow["ZZFAHRG"] = (string)row["CHASSIS_NUM"];
                        partnerZulRow["ZZBRIEF"] = (string)row["TIDNR"];
                        partnerZulRow["ZZREFNR"] = (string)row["LIZNR"];
                        partnerZulRow["PARTN_ROLE"] = "ZH";
                        partnerZulRow["NAME"] = HalterAddress.Name1;
                        partnerZulRow["NAME_2"] = HalterAddress.Name2;
                        partnerZulRow["STREET"] = HalterAddress.Street;
                        partnerZulRow["POSTL_CODE"] = HalterAddress.ZipCode;
                        partnerZulRow["CITY"] = HalterAddress.City;
                        partnerZulRow["COUNTRY"] = HalterAddress.Country;

                        partnerZul.Rows.Add(partnerZulRow);
                    }

                    var versicherter = VersicherungsnehmerAddress ?? HalterAddress;
                    if (versicherter != null)
                    {
                        partnerZulRow = partnerZul.NewRow();

                        // Versicherungsnehmer
                        partnerZulRow["ZZFAHRG"] = (string)row["CHASSIS_NUM"];
                        partnerZulRow["ZZBRIEF"] = (string)row["TIDNR"];
                        partnerZulRow["ZZREFNR"] = (string)row["LIZNR"];
                        partnerZulRow["PARTN_ROLE"] = "ZC";
                        partnerZulRow["NAME"] = versicherter.Name1;
                        partnerZulRow["NAME_2"] = versicherter.Name2;
                        partnerZulRow["STREET"] = versicherter.Street;
                        partnerZulRow["POSTL_CODE"] = versicherter.ZipCode;
                        partnerZulRow["CITY"] = versicherter.City;
                        partnerZulRow["COUNTRY"] = versicherter.Country;
                        partnerZulRow["FLGDEFAULT"] = "X";

                        partnerZul.Rows.Add(partnerZulRow);
                    }

                    var versand = VersandAddress ?? (this.VersandAdressTyp == DAL.VersandAdressTyp.Halter ? this.HalterAddress : this.AuftraggeberAddress);
                    if (versand != null)
                    {
                        partnerZulRow = partnerZul.NewRow();

                        //Versand Schein und Schilder
                        partnerZulRow["ZZFAHRG"] = (string)row["CHASSIS_NUM"];
                        partnerZulRow["ZZBRIEF"] = (string)row["TIDNR"];
                        partnerZulRow["ZZREFNR"] = (string)row["LIZNR"];
                        partnerZulRow["PARTN_ROLE"] = "ZE";
                        partnerZulRow["NAME"] = versand.Name1;
                        partnerZulRow["NAME_2"] = versand.Name2;
                        partnerZulRow["STREET"] = versand.Street;
                        partnerZulRow["POSTL_CODE"] = versand.ZipCode;
                        partnerZulRow["CITY"] = versand.City;
                        partnerZulRow["COUNTRY"] = versand.Country;

                        partnerZul.Rows.Add(partnerZulRow);
                    }



                    //### Dienstleistungen Zulassung ###
                    foreach (DataRow dienstlRow in SelectedServices.Rows)
                    {
                        var dienstlZulRow = dienstlZul.NewRow();

                        dienstlZulRow["ZZFAHRG"] = (string)row["CHASSIS_NUM"];
                        dienstlZulRow["ZZBRIEF"] = (string)row["TIDNR"];
                        dienstlZulRow["ZZREFNR"] = (string)row["LIZNR"];
                        dienstlZulRow["DIENSTL_NR"] = dienstlRow["DIENSTL_NR"];
                        dienstlZulRow["DIENSTL_TEXT"] = dienstlRow["DIENSTL_TEXT"];
                        dienstlZulRow["MATNR"] = dienstlRow["MATNR"];

                        dienstlZul.Rows.Add(dienstlZulRow);
                    }


                    //### Dokumente Zulassung ###
                    foreach (DataRow docRow in Protokollarten.Select("Filename <> ''"))
                    {
                        var dokZulRow = dokZul.NewRow();

                        dokZulRow["ZZFAHRG"] = (string)row["CHASSIS_NUM"];
                        dokZulRow["ZZBRIEF"] = (string)row["TIDNR"];
                        dokZulRow["ZZREFNR"] = (string)row["LIZNR"];
                        dokZulRow["DOKUMENTENART"] = docRow["ZZPROTOKOLLART"];

                        dokZul.Rows.Add(dokZulRow);
                    }
                }


                myProxy.callBapi();
                
                var exportTable = myProxy.getExportTable("GT_RETURN");

                if (exportTable.Rows.Count > 0)
                {
                    Auftragsnummer = exportTable.Rows[0]["VBELN"].ToString();
                    this.ErrMsg = exportTable.Rows[0]["MESSAGE"].ToString();
                    return Auftragsnummer;
                }
            }
            catch (Exception ex)
            {
                m_intStatus = -5555;

                switch (Base.Business.HelpProcedures.CastSapBizTalkErrorMessage(ex.Message))
                {
                    case "NO_VKORG":
                        m_strMessage = "VKORG konnte nicht ermittelt werden.";
                        break;
                    case "STATUS_ERROR":
                        m_strMessage = "Fehler beim Statusändern des ÜberfOrders.";
                        break;
                    case "NO_MAT":
                        m_strMessage = "Kein Material für AG gefunden.";
                        break;
                    case "ZH_NOT_FOUND":
                        m_strMessage = "Halter nicht gefunden.";
                        break;
                    case "ZH_UNVOLLSTAENDIG":
                        m_strMessage = "Halteradresse ist unvollständig.";
                        break;
                    case "ZV_NOT_FOUND":
                        m_strMessage = "Versicherer nicht gefunden.";
                        break;
                    case "ZE_UNVOLLSTAENDIG":
                        m_strMessage = "Empfängeradresse Schein ist unvollständig.";
                        break;
                    case "ZC_NOT_FOUND":
                        m_strMessage = "abw.Versicherungsnehmer nicht gefunden.";
                        break;
                    case "ZC_UNVOLLSTAENDIG":
                        m_strMessage = "abw.Versicherungsnehmer ist unvollständig.";
                        break;
                    case "RE_NOT_FOUND":
                        m_strMessage = "Rechnungsempfänger nicht gefunden.";
                        break;
                    case "RG_NOT_FOUND":
                        m_strMessage = "Regulierer nicht gefunden.";
                        break;
                    case "NO_ZH":
                        m_strMessage = "Halter nicht angegeben.";
                        break;
                    case "NO_ZE":
                        m_strMessage = "Empfänger Scheinr nicht angegeben.";
                        break;
                    case "NO_RE":
                        m_strMessage = "Rechnungsempfänger nicht angegeben.";
                        break;
                    case "NO_RG":
                        m_strMessage = "Regulierer nicht angegeben.";
                        break;
                    case "NO_ZS_ZUM_RE":
                        m_strMessage = "Zum Rechnungsempfänger konnte kein Empfänger des Briefes gefunden werden.";
                        break;
                    case "INV_PLZ":
                        m_strMessage = "Postleitzahl ZH falsch.";
                        break;
                    case "NO_ZULST":
                        m_strMessage = "Keine Zulassungsstelle gefunden.";
                        break;
                    case "SMTP_ERROR":
                        m_strMessage = "E-Mail Adresse fehlerhaft.";
                        break;
                    default:
                        m_intStatus = -9999;
                        m_strMessage = "Fehler beim Speichern.<br>(" + ex.Message + ")";
                        break;
                }

                if (m_intStatus != -9999)
                {
                    m_strMessage = m_strMessage + " Bitte korrigieren Sie Ihre Eingaben.";
                }
            }

            return string.Empty;
        }

        public DataTable SearchAddress(string type, string name, string zipCode, string city)
        {
            try
            {
                var sapProxy = DynSapProxy.getProxy("Z_M_IMP_AUFTRDAT_007", ref m_objApp, ref m_objUser, ref page);

                sapProxy.setImportParameter("I_KUNNR", m_objUser.KUNNR.PadLeft(10, '0').ToString());
                sapProxy.setImportParameter("I_KENNUNG", type.ToUpper());
                sapProxy.setImportParameter("I_NAME1", name);
                sapProxy.setImportParameter("I_PSTLZ", zipCode);
                sapProxy.setImportParameter("I_ORT01", city);

                sapProxy.callBapi();

                return sapProxy.getExportTable("GT_WEB");
            }
            catch (Exception ex)
            {
                if (!HelpProcedures.CastSapBizTalkErrorMessage(ex.Message).Equals("NO_DATA", StringComparison.Ordinal))
                {
                    m_strMessage = "Unbekannter Fehler.";
                    m_intStatus = -9999;
                }
            }
            return null;
        }

        public VorhandeneDokumente FindVorhandeneDokumente()
        {
            m_intStatus = 0;
            m_strMessage = String.Empty;

            if (!String.IsNullOrEmpty(this.KundenDebitorNummer))
            {
                try
                {
                    var myProxy = DynSapProxy.getProxy("Z_M_Zuldokumente_Arval", ref m_objApp, ref m_objUser, ref page);

                    myProxy.setImportParameter("I_AG", m_objUser.KUNNR.PadLeft(10, '0'));
                    myProxy.setImportParameter("I_ZHNAME", this.HalterAddress.Name1.ToUpperInvariant());
                    myProxy.setImportParameter("I_ZHPLZ", this.HalterAddress.ZipCode.ToUpperInvariant());

                    myProxy.callBapi();

                    var tblResult = myProxy.getExportTable("T_ZULDOKUMENTE");
                    var row = tblResult.Select("KUNNR_SAP = '" + this.KundenDebitorNummer + "'").FirstOrDefault();

                    if (row != null)
                    {
                        if (row.Field<string>("KARTE") == "000")
                        {
                            row.SetField("KARTE", "");
                        }
                        else
                        {
                            row.SetField("KARTE", "X");
                        }

                        return new VorhandeneDokumente()
                        {
                            Vollmacht = row.Field<string>("VOLLM").Equals("X", StringComparison.Ordinal),
                            Register = row.Field<string>("REGISTER").Equals("X", StringComparison.Ordinal),
                            Perso = row.Field<string>("PERSO").Equals("X", StringComparison.Ordinal),
                            Gewerbe = row.Field<string>("GEWERBE").Equals("X", StringComparison.Ordinal),
                            Einzug = row.Field<string>("EINZUG").Equals("X", StringComparison.Ordinal),
                            Karte = row.Field<string>("KARTE").Equals("X", StringComparison.Ordinal),
                            Vollst = row.Field<string>("VOLLST").Equals("X", StringComparison.Ordinal),
                        };
                    }
                }
                catch (Exception ex)
                {
                    switch (HelpProcedures.CastSapBizTalkErrorMessage(ex.Message))
                    {
                        case "ERR_INV_AG":
                            m_intStatus = -3331;
                            m_strMessage = "Ungültige Kundennummer.";
                            break;
                        case "ERR_NO_DATA":
                            m_intStatus = -3332;
                            m_strMessage = "Keine Daten gefunden.";
                            break;
                        case "ERR_NO_PARAMETER":
                            m_intStatus = -3333;
                            m_strMessage = "Unzureichende Parameter.";
                            break;
                        default:
                            m_intStatus = -9999;
                            break;
                    }
                }
            }
            return null;
        }

        public bool UpdateAddress(string type, string name, string zipCode, string city)
        {
            try
            {
                var data = SearchAddress(type, name, zipCode, city);

                if (data != null && data.Rows.Count == 1)
                {
                    var addressData = new AddressData();
                    addressData.Name1 = data.Rows[0]["NAME1"].ToString();
                    addressData.Name2 = data.Rows[0]["NAME2"].ToString();
                    addressData.Street = data.Rows[0]["STRAS"].ToString();
                    addressData.ZipCode = data.Rows[0]["PSTLZ"].ToString();
                    addressData.City = data.Rows[0]["ORT01"].ToString();
                    addressData.Description = data.Rows[0]["POS_Text"].ToString();

                    switch (type)
                    {
                        case HalterKennung:
                            HalterAddress = addressData;
                            break;
                        case VersichererKennung:
                            VersichererAddress = addressData;
                            break;
                        case VersandKennung:
                            VersandAddress = addressData;
                            break;
                        case EvbKennung:
                            EvbNo = addressData.Description;
                            break;
                        default:
                            return false;
                    }

                    return true;
                }
            }
            catch (Exception ex)
            {
                if (!HelpProcedures.CastSapBizTalkErrorMessage(ex.Message).Equals("NO_DATA", StringComparison.Ordinal))
                {
                    m_strMessage = "Unbekannter Fehler.";
                    m_intStatus = -9999;
                }
            }
            return false;
        }

        public void SetDefaultData()
        {
            UpdateAddress(HalterKennung, string.Empty, string.Empty, string.Empty);
            UpdateAddress(VersichererKennung, string.Empty, string.Empty, string.Empty);
            UpdateAddress(EvbKennung, string.Empty, string.Empty, string.Empty);
            UpdateAddress(VersandKennung, string.Empty, string.Empty, string.Empty);
        }

        public DataSet GetPrintData(out DataTable header)
        {
            header = new System.Data.DataTable("Kopf");
            header.Columns.Add("LogonUser");
            header.Columns.Add("Regulierer");
            header.Columns.Add("Empfaenger");  

            header.Columns.Add("HName");
            header.Columns.Add("HName2");
            header.Columns.Add("HStrasse");
            header.Columns.Add("HPlz");
            header.Columns.Add("HOrt");
            header.Columns.Add("HLand");

            header.Columns.Add("VName");
            header.Columns.Add("VName2");
            header.Columns.Add("VStrasse");
            header.Columns.Add("VPlz");
            header.Columns.Add("VOrt");
            header.Columns.Add("VLand");

            header.Columns.Add("VsGesellschaft");
            header.Columns.Add("eVBNummer");
            header.Columns.Add("VsVon");
            header.Columns.Add("VsBis");

            header.Columns.Add("VsName");
            header.Columns.Add("VsName2");
            header.Columns.Add("VsStrasse");
            header.Columns.Add("VsPlz");
            header.Columns.Add("VsOrt");
            header.Columns.Add("VsLand");

            header.Columns.Add("ZlArt");
            header.Columns.Add("ZlDatum");
            header.Columns.Add("ZlKreis");
            header.Columns.Add("Auftragsnummer");

            var row = header.NewRow();
            row["LogonUser"] = m_objUser.UserName;


            //Neman der Regulierer ermitteln
            foreach (DataRow r in Partner.Rows)
            {
                if (r["KUNNR"].ToString().Equals(Regulierer.PadLeft(10, '0')) && r["PARVW"].ToString().Equals("RG"))
                {
                    row["Regulierer"] = r["NAME1"] + " (" + Regulierer + ")";
                    continue; 
                }

                if (r["KUNNR"].ToString().Equals(Empfänger.PadLeft(10, '0')) && r["PARVW"].ToString().Equals("RE"))
                {
                    row["Empfaenger"] = r["NAME1"] + " (" + Empfänger + ")";
                    continue;
                }
            }
            //Wenn Name nicht gefunden dann nur die Nummer ausgeben
            if (string.IsNullOrEmpty(row["Regulierer"].ToString())) row["Regulierer"] = Regulierer;
            if (string.IsNullOrEmpty(row["Empfaenger"].ToString())) row["Empfaenger"] = Empfänger;

            row["HName"] = this.HalterAddress.Name1;
            row["HName2"] = this.HalterAddress.Name2;
            row["HStrasse"] = this.HalterAddress.Street;
            row["HPLZ"] = this.HalterAddress.ZipCode;
            row["HOrt"] = this.HalterAddress.City;
            row["HLand"] = this.HalterAddress.Country;

            var vAddress = this.VersandAddress ?? (this.VersandAdressTyp == DAL.VersandAdressTyp.Halter ? this.HalterAddress : this.AuftraggeberAddress);

            row["VName"] = vAddress.Name1;
            row["VName2"] = vAddress.Name2;
            row["VStrasse"] = vAddress.Street;
            row["VPLZ"] = vAddress.ZipCode;
            row["VOrt"] = vAddress.City;
            row["VLand"] = vAddress.Country;

            row["VsGesellschaft"] = this.VersichererAddress.Name1;
            row["eVBNummer"] = this.EvbNo;
            row["VsVon"] = this.VersicherungFrom.HasValue ? this.VersicherungFrom.Value.ToShortDateString() : String.Empty;
            row["VsBis"] = this.VersicherungUntil.HasValue ? this.VersicherungUntil.Value.ToShortDateString() : String.Empty;

            vAddress = this.VersicherungsnehmerAddress ?? this.HalterAddress;

            row["VsName"] = vAddress.Name1;
            row["VsName2"] = vAddress.Name2;
            row["VsStrasse"] = vAddress.Street;
            row["VsPLZ"] = vAddress.ZipCode;
            row["VsOrt"] = vAddress.City;
            row["VsLand"] = vAddress.Country;

            row["ZlArt"] = this.ZulassungsTyp;
            row["ZlDatum"] = this.ZulassungsDate.ToShortDateString();
            row["ZlKreis"] = this.ZulassungsKreis;
            row["Auftragsnummer"] = this.Auftragsnummer;

            header.Rows.Add(row);

            var ds = new System.Data.DataSet();
            var fahrzeuge = new System.Data.DataTable("Fahrzeuge");
            ds.Tables.Add(fahrzeuge);

            fahrzeuge.Columns.Add("Fahrgestellnummer");
            fahrzeuge.Columns.Add("LizNr");
            fahrzeuge.Columns.Add("Kz1");
            fahrzeuge.Columns.Add("Kz2");
            fahrzeuge.Columns.Add("Kz3");
            fahrzeuge.Columns.Add("ResNummer");
            fahrzeuge.Columns.Add("ResName");

            foreach (System.Data.DataRowView kfz in this.SelectedVehicles)
            {
                row = fahrzeuge.NewRow();
                row["Fahrgestellnummer"] = kfz["CHASSIS_NUM"];
                row["LizNr"] = kfz["LIZNR"];
                row["Kz1"] = kfz["Wunschkennz1"];
                row["Kz2"] = kfz["Wunschkennz2"];
                row["Kz3"] = kfz["Wunschkennz3"];
                row["ResNummer"] = kfz["ResNr"];
                row["ResName"] = kfz["ResName"];
                fahrzeuge.Rows.Add(row);
            }

            var dienstleistungen = new System.Data.DataTable("Dienstleistungen");
            ds.Tables.Add(dienstleistungen);

            dienstleistungen.Columns.Add("Dienstleistung");

            foreach (System.Data.DataRow dienstleistung in this.SelectedServices.Rows)
            {
                row = dienstleistungen.NewRow();
                row["Dienstleistung"] = dienstleistung["DIENSTL_TEXT"];
                dienstleistungen.Rows.Add(row);
            }

            var dokumente = new System.Data.DataTable("Dokumente");
            ds.Tables.Add(dokumente);

            dokumente.Columns.Add("Dokument");

            foreach (System.Data.DataRow dokument in this.Protokollarten.Rows)
            {
                var filename = (string)dokument["Filename"];
                if (!String.IsNullOrEmpty(filename))
                {
                    row = dokumente.NewRow();
                    row["Dokument"] = filename;
                    dokumente.Rows.Add(row);
                }
            }

            return ds;
        }

        public string GetAuslZulMailadresse()
        {
            m_intStatus = 0;
            m_strMessage = String.Empty;

            try
            {
                var sapProxy = DynSapProxy.getProxy("Z_M_IMP_AUFTRDAT_007", ref m_objApp, ref m_objUser, ref page);

                sapProxy.setImportParameter("I_KUNNR", m_objUser.KUNNR.PadLeft(10, '0').ToString());
                sapProxy.setImportParameter("I_KENNUNG", "MAIL_AUSLZULBEAUFTRAGUNG");

                sapProxy.callBapi();

                DataTable erg = sapProxy.getExportTable("GT_WEB");

                if (erg.Rows.Count > 0)
                {
                    return erg.Rows[0]["EMAIL"].ToString();
                }
            }
            catch (Exception ex)
            {
                if (!HelpProcedures.CastSapBizTalkErrorMessage(ex.Message).Equals("NO_DATA", StringComparison.Ordinal))
                {
                    m_strMessage = "Unbekannter Fehler.";
                    m_intStatus = -9999;
                }
            }
            return "";
        }

        #endregion
        
        #region private methods

        private void AddVehicleColumns(DataTable vehiclesTable)
        {
            vehiclesTable.Columns["EQUNR"].MaxLength = 32;
            vehiclesTable.PrimaryKey = new[] { vehiclesTable.Columns["EQUNR"] };
            vehiclesTable.Columns.Add("DummyEqui", typeof(string));
            vehiclesTable.Columns.Add(AuswahlCol, typeof(string));
            vehiclesTable.Columns.Add("ZulKreis", typeof(string));
            vehiclesTable.Columns.Add("Wunschkennz1", typeof(string));
            vehiclesTable.Columns.Add("Wunschkennz2", typeof(string));
            vehiclesTable.Columns.Add("Wunschkennz3", typeof(string));
            vehiclesTable.Columns.Add("ResNr", typeof(string));
            vehiclesTable.Columns.Add("ResName", typeof(string));
            vehiclesTable.Columns.Add("Auftragsnr", typeof(string));
        }

        private string GetDefaultZulassungsart()
        {
            try
            {
                var sapProxy = DynSapProxy.getProxy("Z_M_IMP_AUFTRDAT_007", ref m_objApp, ref m_objUser, ref page);

                sapProxy.setImportParameter("I_KUNNR", m_objUser.KUNNR.PadLeft(10, '0').ToString());
                sapProxy.setImportParameter("I_KENNUNG", ZulassungsartKennung);

                sapProxy.callBapi();

                var data = sapProxy.getExportTable("GT_WEB");

                var row = data.Rows.Cast<DataRow>().FirstOrDefault();

                if (row != null)
                {
                    return (string)row["POS_KURZTEXT"];
                }

                return string.Empty;
            }
            catch (Exception ex)
            {
                if (!HelpProcedures.CastSapBizTalkErrorMessage(ex.Message).Equals("NO_DATA", StringComparison.Ordinal))
                {
                    m_strMessage = "Unbekannter Fehler.";
                    m_intStatus = -9999;
                }
            }
            return null;
        }

        private AddressData GetAuftraggeberAddress()
        {
            try
            {
                var sapProxy = DynSapProxy.getProxy("Z_M_IMP_AUFTRDAT_007", ref m_objApp, ref m_objUser, ref page);

                sapProxy.setImportParameter("I_KUNNR", m_objUser.KUNNR.PadLeft(10, '0').ToString());
                sapProxy.setImportParameter("I_KENNUNG", AuftraggeberKennung);

                sapProxy.callBapi();

                var data = sapProxy.getExportTable("GT_WEB");

                var row = data.Rows.Cast<DataRow>().FirstOrDefault();

                if (row != null)
                {
                    return new AddressData()
                        {
                            Name1 = row.Field<string>("NAME1"),
                            Name2 = row.Field<string>("NAME2"),
                            Street = row.Field<string>("STRAS"),
                            ZipCode = row.Field<string>("PSTLZ"),
                            City = row.Field<string>("ORT01"),
                            Country = row.Field<string>("LAND1"),
                            Description = row.Field<string>("POS_TEXT")
                        };
                }

                return null;
            }
            catch (Exception ex)
            {
                if (!HelpProcedures.CastSapBizTalkErrorMessage(ex.Message).Equals("NO_DATA", StringComparison.Ordinal))
                {
                    m_strMessage = "Unbekannter Fehler.";
                    m_intStatus = -9999;
                }
            }
            return null;
        }

        protected void OnPropertyChanged(string name)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(name));
            }
        }

        private DataTable GetProtokollarten()
        {
            m_intStatus = 0;
            try
            {
                var sapProxy = DynSapProxy.getProxy("Z_DPM_READ_TAB_PROT_01", ref m_objApp, ref m_objUser, ref page);

                sapProxy.setImportParameter("I_KUNNR_AG", m_objUser.KUNNR.PadLeft(10, '0').ToString());
                sapProxy.setImportParameter("I_ANWENDUNG", "Z");

                sapProxy.callBapi();
                var returnTable = sapProxy.getExportTable("GT_OUT");

                var protokolle = returnTable.Clone();
                protokolle.Columns.Add("ID", System.Type.GetType("System.String"));
                protokolle.Columns.Add("Filename", System.Type.GetType("System.String"));
                protokolle.Columns.Add("Filepath", System.Type.GetType("System.String"));
                protokolle.Columns.Add("Fahrt", System.Type.GetType("System.String"));

                int i = 1;
                DataRow NewRow = null;
                foreach (DataRow Row in returnTable.Select("WEB_UPLOAD = 'X'"))
                {
                    NewRow = protokolle.NewRow();
                    NewRow["ID"] = i;
                    NewRow["Filename"] = "";
                    NewRow["Filepath"] = "";
                    NewRow["Fahrt"] = "";
                    NewRow["ZZKUNNR"] = Row["ZZKUNNR"];
                    NewRow["ZZKATEGORIE"] = Row["ZZKATEGORIE"];
                    NewRow["ZZPROTOKOLLART"] = Row["ZZPROTOKOLLART"];
                    NewRow["WEB_UPLOAD"] = Row["WEB_UPLOAD"];
                    protokolle.Rows.Add(NewRow);
                    protokolle.AcceptChanges();
                    i++;
                }

                return protokolle;
            }
            catch (Exception ex)
            {
                switch (HelpProcedures.CastSapBizTalkErrorMessage(ex.Message))
                {
                    case "NO_DATA":
                        m_strMessage = "Es konnten keine Protokollarten ermittelt werden.";
                        m_intStatus = -1118;
                        break;
                    default:
                        m_strMessage = "Unbekannter Fehler.";
                        m_intStatus = -9999;
                        break;
                }
            }
            return null;
        }

        private DataTable GetServices()
        {
            var sapProxy = DynSapProxy.getProxy("Z_DPM_READ_LV_001", ref m_objApp, ref m_objUser, ref page);

            sapProxy.setImportParameter("I_VWAG", "X");

            var tblAG = sapProxy.getImportTable("GT_IN_AG");
            var tblProzess = sapProxy.getImportTable("GT_IN_PROZESS");

            var rowAG = tblAG.NewRow();
            rowAG["AG"] = m_objUser.KUNNR.PadLeft(10, '0');
            tblAG.Rows.Add(rowAG);

            var rowProzess = tblProzess.NewRow();
            rowProzess["SORT1"] = 1;
            tblProzess.Rows.Add(rowProzess);

            sapProxy.callBapi();

            var table = sapProxy.getExportTable("GT_OUT_DL");


            table.Columns.Add("Description", typeof(System.String));
            table.Columns["Description"].DefaultValue = string.Empty;

            var selectionRow = table.NewRow();
            selectionRow["EXTGROUP"] = "0";
            selectionRow["KTEXT1_H1"] = "Zulassungsart wählen";
            selectionRow["KTEXT1_H2"] = string.Empty;
            selectionRow["ASNUM"] = string.Empty;
            table.Rows.Add(selectionRow);

            var langTextTable = sapProxy.getExportTable("GT_OUT_ESLL_LTXT");
            if (langTextTable.Rows.Count > 0)
            {
                foreach (DataRow dr in table.Rows)
                {
                    var strText = new StringBuilder();
                    langTextTable.DefaultView.RowFilter = string.Format("SRVPOS = '{0}'", dr["ASNUM"].ToString().PadLeft(18, '0'));
                    if (langTextTable.DefaultView.Count > 0)
                    {
                        for (var i = 0; i < langTextTable.DefaultView.Count; i++)
                        {
                            strText.Append(langTextTable.DefaultView[i]["TDLINE"].ToString() + " ");
                        }
                    }
                    dr["Description"] = strText.ToString();
                }
            }
            return table;
        }

        private DataTable GetCustomerCountries()
        {
            try
            {
                var sapProxy = DynSapProxy.getProxy("Z_M_IMP_AUFTRDAT_007", ref m_objApp, ref m_objUser, ref page);

                sapProxy.setImportParameter("I_KUNNR", m_objUser.KUNNR.PadLeft(10, '0').ToString());
                sapProxy.setImportParameter("I_KENNUNG", "Z_WEB_UEB_LAND");

                sapProxy.callBapi();

                var result = sapProxy.getExportTable("GT_WEB");

                return result;
            }
            catch (Exception ex)
            {
                if (!HelpProcedures.CastSapBizTalkErrorMessage(ex.Message).Equals("NO_DATA", StringComparison.Ordinal))
                {
                    m_strMessage = "Unbekannter Fehler.";
                    m_intStatus = -9999;
                }
            }
            return null;
        }

        private DataTable GetCountries()
        {
            try
            {
                var sapProxy = DynSapProxy.getProxy("Z_M_Land_Plz_001", ref m_objApp, ref m_objUser, ref page);

                sapProxy.callBapi();

                var result = sapProxy.getExportTable("GT_WEB");

                if (result != null)
                {
                    result.Columns.Add("Beschreibung", typeof(string));
                    result.Columns.Add("FullDesc", typeof(string));

                    foreach (DataRow row in result.Rows)
                    {
                        if (Convert.ToInt32(row["LNPLZ"]) > 0)
                        {
                            row["Beschreibung"] = string.Format("{0} ({1})", row["Landx"], row["LNPLZ"]);
                        }
                        else
                        {
                            row["Beschreibung"] = row["Landx"].ToString();
                        }
                        row["FullDesc"] = string.Format("{0} {1}", row["Land1"], row["Beschreibung"]);
                    }

                    result.AcceptChanges();

                    // Get customer specific countries
                    var customerCountries = CustomerCountries;
                    if (customerCountries != null && customerCountries.Rows.Count > 0)
                    {
                        var newResult = result.Clone();

                        foreach (DataRow row in customerCountries.Rows)
                        {
                            var countryRows = result.Select(string.Format("LAND1 = '{0}'", row["POS_KURZTEXT"]));
                            if (countryRows.Length > 0)
                            {
                                var newRow = newResult.NewRow();

                                newRow[0] = countryRows[0][0].ToString();
                                newRow[1] = countryRows[0][1].ToString();
                                newRow[2] = countryRows[0][2].ToString();
                                newRow[3] = countryRows[0][3].ToString();
                                newRow[4] = countryRows[0][4].ToString();
                                newRow[5] = countryRows[0][5].ToString();

                                newResult.Rows.Add(newRow);
                            }
                        }

                        newResult.AcceptChanges();
                        result = newResult;
                    }

                    return result;
                }
            }
            catch (Exception ex)
            {
                if (HelpProcedures.CastSapBizTalkErrorMessage(ex.Message).Equals("ERR_INV_PLZ", StringComparison.Ordinal))
                {
                    m_strMessage = "Ungültige Postleitzahl.";
                    m_intStatus = -1118;
                }
                else
                {
                    m_strMessage = "Unbekannter Fehler.";
                    m_intStatus = -9999;
                }
            }
            return null;
        }

        #endregion

        public void FillPartner()
        {

            m_strClassAndMethod = "Transfer.FillPartner";
            m_strAppID = AppID;
            m_strSessionID = SessionID;

            DataTable TempTable = new DataTable();

            if (!m_blnGestartet)
            {
                this.m_blnGestartet = true;

                try
                {

                    var sapProxy = DynSapProxy.getProxy("Z_M_PARTNER_AUS_KNVP_LESEN", ref m_objApp, ref m_objUser, ref page);
                    sapProxy.setImportParameter("KUNNR", m_objUser.KUNNR.PadLeft(10, '0').ToString());

                    if (!(m_objUser.Store == "AUTOHAUS" && m_objUser.Reference.Trim(' ').Length > 0 && m_objUser.KUNNR == "261510"))
                    {
                        if (m_objUser.Reference.Trim().Length > 0)
                        {
                            sapProxy.setImportParameter("EIKTO", m_objUser.Reference.Trim());
                        }
                    }

                    sapProxy.callBapi();


                    partner = sapProxy.getExportTable("AUSGABE");
                }
                catch (Exception ex)
                {
                    m_intStatus = -9999;
                    m_strMessage = "Beim Erstellen des Reportes ist ein Fehler aufgetreten.<br>(" + ex.Message + ")";
                }
                finally
                {

                    m_blnGestartet = false;
                }//end try
            }//end if
        }

    }
}