using System;
using System.Collections.Generic;
using System.Data;
using GeneralTools.Contracts;

namespace SapORM.Contracts
{
    [CLSCompliant(true)]
    public interface IDynSapProxyObject
    {
        #region Properties


        string BapiName { get; set; }

        DateTime BapiDate { get; set; }

        DateTime BapiLoaded { get; set; }


        #endregion

        #region Methods


        void SetSapConnection(ISapConnection sapConnection);

        DateTime? CallBapiForBapi(ref DataTable mExportTabelle, ref DataTable mImportTabelle, string bapiName, ISapConnection sapConnection);

        bool CallBapi(ILogService logService = null, ILogonContext logonContext = null);


        DataTable Import { get; set; }

        void SetImportParameter(string name, object wert);

        void SetImportTable(string name, DataTable table);

        DataTable GetImportTable(string name);
        
        IEnumerable<DataTable> GetImportTables();

        
        DataTable Export { get; set; }

        string GetExportParameter(string name);
        
        byte[] GetExportParameterByte(string name);
        
        DataTable GetExportTable(string name);
        
        IEnumerable<DataTable> GetExportTables();

        void Reset();

        BapiStructure GetBapiStructure();

        #endregion
    }
}