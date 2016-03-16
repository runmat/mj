using System;
using System.Collections.Generic;
using System.Data;
using GeneralTools.Contracts;

namespace SapORM.Contracts
{
    /// <summary>
    /// Matthias Jenzen, Januar 2013
    /// </summary>
    [CLSCompliant(true)]
    public interface ISapDataService : IDisposable
    {

        #region Properties

        /// <summary>
        /// Ein Objekt für eine gültige SAP Verbindung.
        /// </summary>
        ISapConnection SapConnection { get; }

        /// <summary>
        /// Ein Factory Objekt für eine Kombination aus Zugriff (IDynSapProxyObj) + SAP BAPI Caching (IDynSapProxyCache).
        /// </summary>
        IDynSapProxyFactory DynSapProxyFactory { get; }

        /// <summary>
        /// Der Rückgabe Code aus SAP
        /// </summary>
        int ResultCode { get; }

        /// <summary>
        /// Die Rückgabe Meldung aus SAP
        /// </summary>
        string ResultMessage { get; }

        /// <summary>
        /// Funktion zum Zugriff auf einen Benutzer Logon Context
        /// </summary>
        Func<ILogonContext> GetLogonContext { get; set; }

        /// <summary>
        /// Funktion zum Zugriff auf einen Logging Service 
        /// </summary>
        Func<ILogService> GetLogService { get; set; }

        #endregion


        #region Functions

        /// <summary>
        /// Führt den SAP Zugriff mit den im Vorwege eingestellten BAPI-Parametern aus  (Call Bapi)
        /// </summary>
        void Execute();

        string ExecuteAndCatchErrors(Action sapAction, Func<string> getCustomErrorMessageFunction = null, bool ignoreResultCode = false);


        /// <summary>
        /// Initialisiert den SAP Zugriff mit einer BAPI Funktion und entsprechenden Parametern mit anschließendem „Execute“.
        /// - Die Input-Parameter Namen sind als komma-separierter String zu übergeben.
        /// - Die Input-Parameter Werte sind Typ-sicher als Parameter Array zu übergeben.
        ///    Datums-Parameter sind z. B. als „DateTime“ zu übergeben.
        /// </summary>
        void InitExecute(string sapFunction, string inputParameterKeys, params object[] inputParameterValues);

        /// <summary>
        /// Initialisiert den SAP Zugriff mit einer BAPI Funktion ohne Import-Parameter mit anschließendem „Execute“.        
        /// </summary>
        void InitExecute(string sapFunction);

        /// <summary>
        /// Initialisiert den SAP Zugriff mit einer BAPI Funktion und entsprechenden Parametern aber ohne „Execute“. 
        /// Parameter exakt wie bei „InitExecute“
        /// </summary>
        void Init(string sapFunction, string inputParameterKeys, params object[] inputParameterValues);

        /// <summary>
        /// Initialisiert den SAP Zugriff mit einer BAPI Funktion ohne Import-Parameter aber ohne „Execute“. 
        /// Parameter exakt wie bei „InitExecute“
        /// </summary>
        void Init(string sapFunction);

        /// <summary>
        /// Gibt die serialisierten Im-/Export-Strukturen eines Bapis für den Bapistruktur-Check zurück
        /// </summary>
        /// <param name="sapFunction"></param>
        /// <returns></returns>
        byte[] GetSerializedBapiStructuresForBapiCheck(string sapFunction);


        #region Export 

        /// <summary>
        /// Führt für eine SAP-Funktion (Bapi) ein „InitExecute“ aus und liefert eine Liste aller Export-Tabellen zurück.
        /// </summary>
        IEnumerable<DataTable> GetExportTablesWithInitExecute(string sapFunction, string inputParameterKeys, params object[] inputParameterValues);

        /// <summary>
        /// Führt für eine SAP-Funktion (Bapi) ein „InitExecute“ aus und liefert eine bestimmte Export-Tabelle zurück.
        /// Die Aufruf-Konvention für den Parameter „sapFunctionAndTable“ lautet: 
        /// [Bapi-Name].[Export-Tabellenname]  (Bapi und Tabelle werden mit einem Punkt getrennt)
        /// </summary>
        DataTable GetExportTableWithInitExecute(string sapFunctionAndTable, string inputParameterKeys, params object[] inputParameterValues);

        /// <summary>
        /// Führt für eine SAP-Funktion (Bapi) ein „InitExecute“ aus und liefert eine bestimmte Export-Tabelle zurück.
        /// Die Aufruf-Konvention für den Parameter „sapFunctionAndTable“ lautet: 
        /// [Bapi-Name].[Export-Tabellenname]  (Bapi und Tabelle werden mit einem Punkt getrennt)
        /// </summary>
        DataTable GetExportTableWithInitExecute(string sapFunctionAndTable);

        /// <summary>
        /// Liefert eine Liste aller SAP Export-Tabellen zurück ohne „Init“ und „Execute“.
        /// Ein „Init“ und „Execute“ muss aber im Vorwege separat durchgeführt worden sein.
        /// </summary>
        IEnumerable<DataTable> GetExportTables();

        /// <summary>
        /// Liefert eine bestimmte SAP Export-Tabelle zurück ohne „Init“ und „Execute“.
        /// Ein „Init“ und „Execute“ muss aber im Vorwege separat durchgeführt worden sein.
        /// </summary>
        DataTable GetExportTable(string tableName);

        /// <summary>
        /// Liefert eine Liste aller SAP Export-Tabellen zurück ohne „Init“ aber mit  „Execute“.
        /// Ein „Init“ muss aber im Vorwege separat durchgeführt worden sein.
        /// </summary>
        IEnumerable<DataTable> GetExportTablesWithExecute();

        /// <summary>
        /// Liefert eine bestimmte SAP Export-Tabelle zurück ohne „Init“ aber mit „Execute“.
        /// Ein „Init“ muss aber im Vorwege separat durchgeführt worden sein.
        /// </summary>
        DataTable GetExportTableWithExecute(string tableName);

        /// <summary>
        /// Liefert den Wert eines bestimmten Export-Parameters aus SAP zurück.
        /// </summary>
        string GetExportParameter(string paramName);

        /// <summary>
        /// Liefert einen Byte Array-Wert eines bestimmten Export-Parameters aus SAP zurück. 
        /// </summary>
        /// <param name="paramName"></param>
        /// <returns></returns>
        byte[] GetExportParameterByte(string paramName);

        T GetExportParameter<T>(string paramName);

        /// <summary>
        /// Führt für eine SAP-Funktion (Bapi) ein „InitExecute“ aus und liefert 
        /// den Wert eines bestimmten Export-Parameters aus SAP als Byte Array zurück mit „Init“ + „Execute“.
        /// </summary>
        byte[] GetExportParameterByteWithInitExecute(string sapFunction, string paramName, string inputParameterKeys, params object[] inputParameterValues);

        /// <summary>
        /// Liefert den Wert eines bestimmten Export-Parameters aus SAP zurück ohne „Init“ aber mit  „Execute“.
        /// Ein „Init“ muss aber im Vorwege separat durchgeführt worden sein.
        /// </summary>
        string GetExportParameterWithExecute(string paramName);

        /// <summary>
        /// Führt für eine SAP-Funktion (Bapi) ein „InitExecute“ aus und liefert 
        /// den Wert eines bestimmten Export-Parameters aus SAP zurück mit „Init“ + „Execute“.
        /// </summary>
        string GetExportParameterWithInitExecute(string sapFunction, string paramName, string inputParameterKeys, params object[] inputParameterValues);

        #endregion


        #region Import


        /// <summary>
        /// Führt für eine SAP-Funktion (Bapi) ein „Init“ aus und liefert eine Liste aller Import-Tabellen zurück.
        /// </summary>
        IEnumerable<DataTable> GetImportTablesWithInit(string sapFunction, string inputParameterKeys, params object[] inputParameterValues);

        /// <summary>
        /// Führt für eine SAP-Funktion (Bapi) ein „Init“ aus und liefert eine Liste aller Import-Tabellen zurück.
        /// </summary>
        IEnumerable<DataTable> GetImportTablesWithInit(string sapFunction);

        /// <summary>
        /// Führt für eine SAP-Funktion (Bapi) ein „Init“ aus und liefert eine bestimmte Import-Tabelle zurück.
        /// Die Aufruf-Konvention führ den Parameter „sapFunctionAndTable“ lautet: 
        /// [Bapi-Name].[Import-Tabellenname]  (Bapi und Tabelle werden mit einem Punkt getrennt)
        /// </summary>
        DataTable GetImportTableWithInit(string sapFunctionAndTable, string inputParameterKeys, params object[] inputParameterValues);

        /// <summary>
        /// Führt für eine SAP-Funktion (Bapi) ein „Init“ aus und liefert eine bestimmte Import-Tabelle zurück.
        /// Die Aufruf-Konvention führ den Parameter „sapFunctionAndTable“ lautet: 
        /// [Bapi-Name].[Import-Tabellenname]  (Bapi und Tabelle werden mit einem Punkt getrennt)
        /// </summary>
        DataTable GetImportTableWithInit(string sapFunctionAndTable);

        /// <summary>
        /// Liefert eine Liste aller SAP Import-Tabellen zurück ohne „Init“.
        /// Ein „Init“ muss aber im Vorwege separat durchgeführt worden sein.
        /// </summary>
        IEnumerable<DataTable> GetImportTables();

        /// <summary>
        /// Liefert eine bestimmte SAP Import-Tabelle zurück ohne „Init“.
        /// Ein „Init“ muss aber im Vorwege separat durchgeführt worden sein.
        /// </summary>
        DataTable GetImportTable(string tableName);

        /// <summary>
        /// Wendet  eine Liste von ORM-Objekten auf die passende SAP Import-Tabelle automatisiert an.
        /// Hiermit ist ein Typ-sichere (ORM) Erzeugung + Modifikation von SAP Import Objekten möglich! 
        /// </summary>
        void ApplyImport<T>(IEnumerable<T> list);

        void SetImportParameter(string name, object wert);

        void SetImportTable(string name, DataTable table);

        #endregion

        #endregion
    }
}
