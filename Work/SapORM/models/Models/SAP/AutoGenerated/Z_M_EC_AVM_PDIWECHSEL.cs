using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Script.Serialization;
using GeneralTools.Contracts;
using GeneralTools.Models;
using SapORM.Contracts;

namespace SapORM.Models
{
	public partial class Z_M_EC_AVM_PDIWECHSEL
	{
		public static void Init(ISapDataService sap)
		{
			sap.Init(typeof(Z_M_EC_AVM_PDIWECHSEL).Name);
		}

		public static void Init(ISapDataService sap, string inputParameterKeys, params object[] inputParameterValues)
		{
			sap.Init(typeof(Z_M_EC_AVM_PDIWECHSEL).Name, inputParameterKeys, inputParameterValues);
		}


		public void SetImportParameter_I_ZZCARPORT(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_ZZCARPORT", value);
		}

		public void SetImportParameter_I_ZZDATBEM(ISapDataService sap, DateTime? value)
		{
			sap.SetImportParameter("I_ZZDATBEM", value);
		}

		public void SetImportParameter_ZZCARPORT(ISapDataService sap, string value)
		{
			sap.SetImportParameter("ZZCARPORT", value);
		}

		public void SetImportParameter_ZZKUNNR(ISapDataService sap, string value)
		{
			sap.SetImportParameter("ZZKUNNR", value);
		}

		public void SetImportParameter_ZZQMNUM(ISapDataService sap, string value)
		{
			sap.SetImportParameter("ZZQMNUM", value);
		}

		public partial class ZZBEMERKUNG : IModelMappingApplied
		{
			[SapIgnore]
			[ScriptIgnore]
			public ISapConnection SAPConnection { get; set; }

			[SapIgnore]
			[ScriptIgnore]
			public IDynSapProxyFactory DynSapProxyFactory { get; set; }

			public string TDFORMAT { get; set; }

			public string TDLINE { get; set; }

			public static ZZBEMERKUNG Create(DataRow row, ISapConnection sapConnection = null, IDynSapProxyFactory dynSapProxyFactory = null)
			{
				var o = new ZZBEMERKUNG
				{
					TDFORMAT = (string)row["TDFORMAT"],
					TDLINE = (string)row["TDLINE"],

					SAPConnection = sapConnection,
					DynSapProxyFactory = dynSapProxyFactory,
				};
				o.OnInitFromSap();
				return o;
			}

			partial void OnInitFromSap();

			partial void OnInitFromExtern();

			public void OnModelMappingApplied()
			{
				OnInitFromExtern();
			}

			public static IEnumerable<ZZBEMERKUNG> Select(DataTable dt, ISapConnection sapConnection = null)
			{
				return dt.AsEnumerable().Select(r => Create(r, sapConnection));
			}

			public static List<ZZBEMERKUNG> ToList(DataTable dt, ISapConnection sapConnection = null)
			{
				return Select(dt, sapConnection).ToListOrEmptyList();
			}

			public static IEnumerable<ZZBEMERKUNG> Select(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				var tbl = dts.FirstOrDefault(t => t.TableName.ToLower() == typeof(ZZBEMERKUNG).Name.ToLower());
				if (tbl == null)
					return null;

				return Select(tbl, sapConnection);
			}

			public static List<ZZBEMERKUNG> ToList(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				return Select(dts, sapConnection).ToListOrEmptyList();
			}

			public static List<ZZBEMERKUNG> ToList(ISapDataService sapDataService)
			{
				return ToList(sapDataService.GetExportTables(), sapDataService.SapConnection);
			}

			public static List<ZZBEMERKUNG> GetExportListWithInitExecute(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<ZZBEMERKUNG>();
				 
				var dts = sapDataService.GetExportTablesWithInitExecute("Z_M_EC_AVM_PDIWECHSEL", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<ZZBEMERKUNG> GetExportListWithExecute(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<ZZBEMERKUNG>();
				 
				var dts = sapDataService.GetExportTablesWithExecute();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<ZZBEMERKUNG> GetExportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<ZZBEMERKUNG>();
				 
				var dts = sapDataService.GetExportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<ZZBEMERKUNG> GetImportListWithInit(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<ZZBEMERKUNG>();
				 
				var dts = sapDataService.GetImportTablesWithInit("Z_M_EC_AVM_PDIWECHSEL", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<ZZBEMERKUNG> GetImportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<ZZBEMERKUNG>();
				 
				var dts = sapDataService.GetImportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}
		}
	}

	public static partial class DataTableExtensions
	{

		public static DataTable ToTable(this IEnumerable<Z_M_EC_AVM_PDIWECHSEL.ZZBEMERKUNG> list)
		{
			return SapDataServiceExtensions.ToTable(list);
		}

	}
}
