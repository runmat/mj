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
	public partial class Z_V_UEBERF_AUFTR_REFERENZ
	{
		public static void Init(ISapDataService sap)
		{
			sap.Init(typeof(Z_V_UEBERF_AUFTR_REFERENZ).Name);
		}

		public static void Init(ISapDataService sap, string inputParameterKeys, params object[] inputParameterValues)
		{
			sap.Init(typeof(Z_V_UEBERF_AUFTR_REFERENZ).Name, inputParameterKeys, inputParameterValues);
		}


		public static void SetImportParameter_AUFNR(ISapDataService sap, string value)
		{
			sap.SetImportParameter("AUFNR", value);
		}

		public static void SetImportParameter_REFNR(ISapDataService sap, string value)
		{
			sap.SetImportParameter("REFNR", value);
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

			public string FAHRER { get; set; }

			public string NAME1 { get; set; }

			public string CITY1 { get; set; }

			public string PSTCD1 { get; set; }

			public string STREET { get; set; }

			public DateTime? WADAT { get; set; }

			public string EQUNR { get; set; }

			public string ZZKENN { get; set; }

			public string ZZFAHRG { get; set; }

			public string ZZREFNR { get; set; }

			public string ZZBEZEI { get; set; }

			public string FAHRTVON { get; set; }

			public string FAHRTNACH { get; set; }

			public string ZZKUNNR { get; set; }

			public string ZZPROTKAT1 { get; set; }

			public string ZZPROTKAT2 { get; set; }

			public string ZZPROTKAT3 { get; set; }

			public string VKORG { get; set; }

			public DateTime? IUG_DAT { get; set; }

			public string SEITANZ_PROTKAT1 { get; set; }

			public string SEITANZ_PROTKAT2 { get; set; }

			public string SEITANZ_PROTKAT3 { get; set; }

			public static T_AUFTRAEGE Create(DataRow row, ISapConnection sapConnection = null, IDynSapProxyFactory dynSapProxyFactory = null)
			{
				var o = new T_AUFTRAEGE
				{
					AUFNR = (string)row["AUFNR"],
					FAHRTNR = (string)row["FAHRTNR"],
					FAHRER = (string)row["FAHRER"],
					NAME1 = (string)row["NAME1"],
					CITY1 = (string)row["CITY1"],
					PSTCD1 = (string)row["PSTCD1"],
					STREET = (string)row["STREET"],
					WADAT = string.IsNullOrEmpty(row["WADAT"].ToString()) ? null : (DateTime?)row["WADAT"],
					EQUNR = (string)row["EQUNR"],
					ZZKENN = (string)row["ZZKENN"],
					ZZFAHRG = (string)row["ZZFAHRG"],
					ZZREFNR = (string)row["ZZREFNR"],
					ZZBEZEI = (string)row["ZZBEZEI"],
					FAHRTVON = (string)row["FAHRTVON"],
					FAHRTNACH = (string)row["FAHRTNACH"],
					ZZKUNNR = (string)row["ZZKUNNR"],
					ZZPROTKAT1 = (string)row["ZZPROTKAT1"],
					ZZPROTKAT2 = (string)row["ZZPROTKAT2"],
					ZZPROTKAT3 = (string)row["ZZPROTKAT3"],
					VKORG = (string)row["VKORG"],
					IUG_DAT = string.IsNullOrEmpty(row["IUG_DAT"].ToString()) ? null : (DateTime?)row["IUG_DAT"],
					SEITANZ_PROTKAT1 = (string)row["SEITANZ_PROTKAT1"],
					SEITANZ_PROTKAT2 = (string)row["SEITANZ_PROTKAT2"],
					SEITANZ_PROTKAT3 = (string)row["SEITANZ_PROTKAT3"],

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
				 
				var dts = sapDataService.GetExportTablesWithInitExecute("Z_V_UEBERF_AUFTR_REFERENZ", inputParameterKeys, inputParameterValues);
				 
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
				 
				var dts = sapDataService.GetImportTablesWithInit("Z_V_UEBERF_AUFTR_REFERENZ", inputParameterKeys, inputParameterValues);
				 
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

		public partial class T_SMTP : IModelMappingApplied
		{
			[SapIgnore]
			[ScriptIgnore]
			public ISapConnection SAPConnection { get; set; }

			[SapIgnore]
			[ScriptIgnore]
			public IDynSapProxyFactory DynSapProxyFactory { get; set; }

			public string AUFNR { get; set; }

			public string FAHRTNR { get; set; }

			public string SMTP_ADDR { get; set; }

			public static T_SMTP Create(DataRow row, ISapConnection sapConnection = null, IDynSapProxyFactory dynSapProxyFactory = null)
			{
				var o = new T_SMTP
				{
					AUFNR = (string)row["AUFNR"],
					FAHRTNR = (string)row["FAHRTNR"],
					SMTP_ADDR = (string)row["SMTP_ADDR"],

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

			public static IEnumerable<T_SMTP> Select(DataTable dt, ISapConnection sapConnection = null)
			{
				return dt.AsEnumerable().Select(r => Create(r, sapConnection));
			}

			public static List<T_SMTP> ToList(DataTable dt, ISapConnection sapConnection = null)
			{
				return Select(dt, sapConnection).ToListOrEmptyList();
			}

			public static IEnumerable<T_SMTP> Select(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				var tbl = dts.FirstOrDefault(t => t.TableName.ToLower() == typeof(T_SMTP).Name.ToLower());
				if (tbl == null)
					return null;

				return Select(tbl, sapConnection);
			}

			public static List<T_SMTP> ToList(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				return Select(dts, sapConnection).ToListOrEmptyList();
			}

			public static List<T_SMTP> ToList(ISapDataService sapDataService)
			{
				return ToList(sapDataService.GetExportTables(), sapDataService.SapConnection);
			}

			public static List<T_SMTP> GetExportListWithInitExecute(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<T_SMTP>();
				 
				var dts = sapDataService.GetExportTablesWithInitExecute("Z_V_UEBERF_AUFTR_REFERENZ", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<T_SMTP> GetExportListWithExecute(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<T_SMTP>();
				 
				var dts = sapDataService.GetExportTablesWithExecute();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<T_SMTP> GetExportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<T_SMTP>();
				 
				var dts = sapDataService.GetExportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<T_SMTP> GetImportListWithInit(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<T_SMTP>();
				 
				var dts = sapDataService.GetImportTablesWithInit("Z_V_UEBERF_AUFTR_REFERENZ", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<T_SMTP> GetImportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<T_SMTP>();
				 
				var dts = sapDataService.GetImportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}
		}
	}

	public static partial class DataTableExtensions
	{

		public static DataTable ToTable(this IEnumerable<Z_V_UEBERF_AUFTR_REFERENZ.T_AUFTRAEGE> list)
		{
			return SapDataServiceExtensions.ToTable(list);
		}


		public static DataTable ToTable(this IEnumerable<Z_V_UEBERF_AUFTR_REFERENZ.T_SMTP> list)
		{
			return SapDataServiceExtensions.ToTable(list);
		}

	}
}
