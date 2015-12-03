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
	public partial class Z_V_UEBERF_AUFTR_KUND_PORT
	{
		public static void Init(ISapDataService sap)
		{
			sap.Init(typeof(Z_V_UEBERF_AUFTR_KUND_PORT).Name);
		}

		public static void Init(ISapDataService sap, string inputParameterKeys, params object[] inputParameterValues)
		{
			sap.Init(typeof(Z_V_UEBERF_AUFTR_KUND_PORT).Name, inputParameterKeys, inputParameterValues);
		}


		public partial class T_AUFTRAEGE : IModelMappingApplied
		{
			[SapIgnore]
			[ScriptIgnore]
			public ISapConnection SAPConnection { get; set; }

			[SapIgnore]
			[ScriptIgnore]
			public IDynSapProxyFactory DynSapProxyFactory { get; set; }

			public string AUFNR { get; set; }

			public string FAHRTNR { get; set; }

			public string ZZREFNR { get; set; }

			public string ZZKENN { get; set; }

			public string ZZFAHRG { get; set; }

			public string ZZBEZEI { get; set; }

			public string FAHRTVON { get; set; }

			public DateTime? WADAT { get; set; }

			public string FAHRTNACH { get; set; }

			public DateTime? WADAT_IST { get; set; }

			public DateTime? DAT_TERM { get; set; }

			public DateTime? DAT_ABHOL { get; set; }

			public string GEF_KM { get; set; }

			public string NAME1 { get; set; }

			public string TELNR_LONG { get; set; }

			public string SMTP_ADDR { get; set; }

			public string KVGR2 { get; set; }

			public string ZZFOTO { get; set; }

			public string ZZPROTOKOLL { get; set; }

			public string KUNNR_AG { get; set; }

			public DateTime? ERDAT { get; set; }

			public DateTime? VDATU { get; set; }

			public string KFTEXT { get; set; }

			public string WBSTK { get; set; }

			public string EXTENSION2 { get; set; }

			public string NAME_LG { get; set; }

			public string NAME_LN { get; set; }

			public string EX_KUNNR { get; set; }

			public string FAHRER_STATUS { get; set; }

			public static T_AUFTRAEGE Create(DataRow row, ISapConnection sapConnection = null, IDynSapProxyFactory dynSapProxyFactory = null)
			{
				var o = new T_AUFTRAEGE
				{
					AUFNR = (string)row["AUFNR"],
					FAHRTNR = (string)row["FAHRTNR"],
					ZZREFNR = (string)row["ZZREFNR"],
					ZZKENN = (string)row["ZZKENN"],
					ZZFAHRG = (string)row["ZZFAHRG"],
					ZZBEZEI = (string)row["ZZBEZEI"],
					FAHRTVON = (string)row["FAHRTVON"],
					WADAT = string.IsNullOrEmpty(row["WADAT"].ToString()) ? null : (DateTime?)row["WADAT"],
					FAHRTNACH = (string)row["FAHRTNACH"],
					WADAT_IST = string.IsNullOrEmpty(row["WADAT_IST"].ToString()) ? null : (DateTime?)row["WADAT_IST"],
					DAT_TERM = string.IsNullOrEmpty(row["DAT_TERM"].ToString()) ? null : (DateTime?)row["DAT_TERM"],
					DAT_ABHOL = string.IsNullOrEmpty(row["DAT_ABHOL"].ToString()) ? null : (DateTime?)row["DAT_ABHOL"],
					GEF_KM = (string)row["GEF_KM"],
					NAME1 = (string)row["NAME1"],
					TELNR_LONG = (string)row["TELNR_LONG"],
					SMTP_ADDR = (string)row["SMTP_ADDR"],
					KVGR2 = (string)row["KVGR2"],
					ZZFOTO = (string)row["ZZFOTO"],
					ZZPROTOKOLL = (string)row["ZZPROTOKOLL"],
					KUNNR_AG = (string)row["KUNNR_AG"],
					ERDAT = string.IsNullOrEmpty(row["ERDAT"].ToString()) ? null : (DateTime?)row["ERDAT"],
					VDATU = string.IsNullOrEmpty(row["VDATU"].ToString()) ? null : (DateTime?)row["VDATU"],
					KFTEXT = (string)row["KFTEXT"],
					WBSTK = (string)row["WBSTK"],
					EXTENSION2 = (string)row["EXTENSION2"],
					NAME_LG = (string)row["NAME_LG"],
					NAME_LN = (string)row["NAME_LN"],
					EX_KUNNR = (string)row["EX_KUNNR"],
					FAHRER_STATUS = (string)row["FAHRER_STATUS"],

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

			public static IEnumerable<T_AUFTRAEGE> Select(DataTable dt, ISapConnection sapConnection = null)
			{
				return dt.AsEnumerable().Select(r => Create(r, sapConnection));
			}

			public static List<T_AUFTRAEGE> ToList(DataTable dt, ISapConnection sapConnection = null)
			{
				return Select(dt, sapConnection).ToListOrEmptyList();
			}

			public static IEnumerable<T_AUFTRAEGE> Select(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				var tbl = dts.FirstOrDefault(t => t.TableName.ToLower() == typeof(T_AUFTRAEGE).Name.ToLower());
				if (tbl == null)
					return null;

				return Select(tbl, sapConnection);
			}

			public static List<T_AUFTRAEGE> ToList(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				return Select(dts, sapConnection).ToListOrEmptyList();
			}

			public static List<T_AUFTRAEGE> ToList(ISapDataService sapDataService)
			{
				return ToList(sapDataService.GetExportTables(), sapDataService.SapConnection);
			}

			public static List<T_AUFTRAEGE> GetExportListWithInitExecute(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<T_AUFTRAEGE>();
				 
				var dts = sapDataService.GetExportTablesWithInitExecute("Z_V_UEBERF_AUFTR_KUND_PORT", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<T_AUFTRAEGE> GetExportListWithExecute(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<T_AUFTRAEGE>();
				 
				var dts = sapDataService.GetExportTablesWithExecute();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<T_AUFTRAEGE> GetExportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<T_AUFTRAEGE>();
				 
				var dts = sapDataService.GetExportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<T_AUFTRAEGE> GetImportListWithInit(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<T_AUFTRAEGE>();
				 
				var dts = sapDataService.GetImportTablesWithInit("Z_V_UEBERF_AUFTR_KUND_PORT", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<T_AUFTRAEGE> GetImportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<T_AUFTRAEGE>();
				 
				var dts = sapDataService.GetImportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}
		}

		public partial class T_SELECT : IModelMappingApplied
		{
			[SapIgnore]
			[ScriptIgnore]
			public ISapConnection SAPConnection { get; set; }

			[SapIgnore]
			[ScriptIgnore]
			public IDynSapProxyFactory DynSapProxyFactory { get; set; }

			public string AUFNR { get; set; }

			public string ZZREFNR { get; set; }

			public DateTime? ERDAT { get; set; }

			public string ZZKENN { get; set; }

			public DateTime? VDATU { get; set; }

			public DateTime? VDATU_BIS { get; set; }

			public string KUNNR_AG { get; set; }

			public DateTime? ERDAT_BIS { get; set; }

			public string WBSTK { get; set; }

			public string ZORGADMIN { get; set; }

			public string NAME_LG { get; set; }

			public string NAME_LN { get; set; }

			public string EX_KUNNR { get; set; }

			public static T_SELECT Create(DataRow row, ISapConnection sapConnection = null, IDynSapProxyFactory dynSapProxyFactory = null)
			{
				var o = new T_SELECT
				{
					AUFNR = (string)row["AUFNR"],
					ZZREFNR = (string)row["ZZREFNR"],
					ERDAT = string.IsNullOrEmpty(row["ERDAT"].ToString()) ? null : (DateTime?)row["ERDAT"],
					ZZKENN = (string)row["ZZKENN"],
					VDATU = string.IsNullOrEmpty(row["VDATU"].ToString()) ? null : (DateTime?)row["VDATU"],
					VDATU_BIS = string.IsNullOrEmpty(row["VDATU_BIS"].ToString()) ? null : (DateTime?)row["VDATU_BIS"],
					KUNNR_AG = (string)row["KUNNR_AG"],
					ERDAT_BIS = string.IsNullOrEmpty(row["ERDAT_BIS"].ToString()) ? null : (DateTime?)row["ERDAT_BIS"],
					WBSTK = (string)row["WBSTK"],
					ZORGADMIN = (string)row["ZORGADMIN"],
					NAME_LG = (string)row["NAME_LG"],
					NAME_LN = (string)row["NAME_LN"],
					EX_KUNNR = (string)row["EX_KUNNR"],

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

			public static IEnumerable<T_SELECT> Select(DataTable dt, ISapConnection sapConnection = null)
			{
				return dt.AsEnumerable().Select(r => Create(r, sapConnection));
			}

			public static List<T_SELECT> ToList(DataTable dt, ISapConnection sapConnection = null)
			{
				return Select(dt, sapConnection).ToListOrEmptyList();
			}

			public static IEnumerable<T_SELECT> Select(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				var tbl = dts.FirstOrDefault(t => t.TableName.ToLower() == typeof(T_SELECT).Name.ToLower());
				if (tbl == null)
					return null;

				return Select(tbl, sapConnection);
			}

			public static List<T_SELECT> ToList(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				return Select(dts, sapConnection).ToListOrEmptyList();
			}

			public static List<T_SELECT> ToList(ISapDataService sapDataService)
			{
				return ToList(sapDataService.GetExportTables(), sapDataService.SapConnection);
			}

			public static List<T_SELECT> GetExportListWithInitExecute(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<T_SELECT>();
				 
				var dts = sapDataService.GetExportTablesWithInitExecute("Z_V_UEBERF_AUFTR_KUND_PORT", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<T_SELECT> GetExportListWithExecute(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<T_SELECT>();
				 
				var dts = sapDataService.GetExportTablesWithExecute();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<T_SELECT> GetExportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<T_SELECT>();
				 
				var dts = sapDataService.GetExportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<T_SELECT> GetImportListWithInit(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<T_SELECT>();
				 
				var dts = sapDataService.GetImportTablesWithInit("Z_V_UEBERF_AUFTR_KUND_PORT", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<T_SELECT> GetImportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<T_SELECT>();
				 
				var dts = sapDataService.GetImportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}
		}
	}

	public static partial class DataTableExtensions
	{

		public static DataTable ToTable(this IEnumerable<Z_V_UEBERF_AUFTR_KUND_PORT.T_AUFTRAEGE> list)
		{
			return SapDataServiceExtensions.ToTable(list);
		}


		public static DataTable ToTable(this IEnumerable<Z_V_UEBERF_AUFTR_KUND_PORT.T_SELECT> list)
		{
			return SapDataServiceExtensions.ToTable(list);
		}

	}
}
