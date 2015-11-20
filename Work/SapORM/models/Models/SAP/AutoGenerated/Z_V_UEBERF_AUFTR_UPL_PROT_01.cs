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
	public partial class Z_V_UEBERF_AUFTR_UPL_PROT_01
	{
		public static void Init(ISapDataService sap)
		{
			sap.Init(typeof(Z_V_UEBERF_AUFTR_UPL_PROT_01).Name);
		}

		public static void Init(ISapDataService sap, string inputParameterKeys, params object[] inputParameterValues)
		{
			sap.Init(typeof(Z_V_UEBERF_AUFTR_UPL_PROT_01).Name, inputParameterKeys, inputParameterValues);
		}


		public static void SetImportParameter_I_FAHRER(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_FAHRER", value);
		}

		public static void SetImportParameter_I_VBELN(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_VBELN", value);
		}

		public static string GetExportParameter_E_CITY1(ISapDataService sap)
		{
			return sap.GetExportParameter<string>("E_CITY1").NotNullOrEmpty().Trim();
		}

		public static string GetExportParameter_E_FAHRER(ISapDataService sap)
		{
			return sap.GetExportParameter<string>("E_FAHRER").NotNullOrEmpty().Trim();
		}

		public static string GetExportParameter_E_NAME1(ISapDataService sap)
		{
			return sap.GetExportParameter<string>("E_NAME1").NotNullOrEmpty().Trim();
		}

		public static string GetExportParameter_E_PSTCD1(ISapDataService sap)
		{
			return sap.GetExportParameter<string>("E_PSTCD1").NotNullOrEmpty().Trim();
		}

		public static string GetExportParameter_E_STREET(ISapDataService sap)
		{
			return sap.GetExportParameter<string>("E_STREET").NotNullOrEmpty().Trim();
		}

		public partial class GT_OUT : IModelMappingApplied
		{
			[SapIgnore]
			[ScriptIgnore]
			public ISapConnection SAPConnection { get; set; }

			[SapIgnore]
			[ScriptIgnore]
			public IDynSapProxyFactory DynSapProxyFactory { get; set; }

			public string KUNNR_AG { get; set; }

			public string VBELN { get; set; }

			public string FAHRTNR { get; set; }

			public DateTime? WADAT { get; set; }

			public string EQUNR { get; set; }

			public string ZZKENN { get; set; }

			public string ZZFAHRG { get; set; }

			public string ZZREFNR { get; set; }

			public string ZZBEZEI { get; set; }

			public string FAHRTVON { get; set; }

			public string FAHRTNACH { get; set; }

			public string KVGR2 { get; set; }

			public string ZZPROTKAT1 { get; set; }

			public string ZZPROTKAT2 { get; set; }

			public string ZZPROTKAT3 { get; set; }

			public string ZZPOSPROTKAT1 { get; set; }

			public string ZZPOSPROTKAT2 { get; set; }

			public string ZZPOSPROTKAT3 { get; set; }

			public static GT_OUT Create(DataRow row, ISapConnection sapConnection = null, IDynSapProxyFactory dynSapProxyFactory = null)
			{
				var o = new GT_OUT
				{
					KUNNR_AG = (string)row["KUNNR_AG"],
					VBELN = (string)row["VBELN"],
					FAHRTNR = (string)row["FAHRTNR"],
					WADAT = string.IsNullOrEmpty(row["WADAT"].ToString()) ? null : (DateTime?)row["WADAT"],
					EQUNR = (string)row["EQUNR"],
					ZZKENN = (string)row["ZZKENN"],
					ZZFAHRG = (string)row["ZZFAHRG"],
					ZZREFNR = (string)row["ZZREFNR"],
					ZZBEZEI = (string)row["ZZBEZEI"],
					FAHRTVON = (string)row["FAHRTVON"],
					FAHRTNACH = (string)row["FAHRTNACH"],
					KVGR2 = (string)row["KVGR2"],
					ZZPROTKAT1 = (string)row["ZZPROTKAT1"],
					ZZPROTKAT2 = (string)row["ZZPROTKAT2"],
					ZZPROTKAT3 = (string)row["ZZPROTKAT3"],
					ZZPOSPROTKAT1 = (string)row["ZZPOSPROTKAT1"],
					ZZPOSPROTKAT2 = (string)row["ZZPOSPROTKAT2"],
					ZZPOSPROTKAT3 = (string)row["ZZPOSPROTKAT3"],

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

			public static IEnumerable<GT_OUT> Select(DataTable dt, ISapConnection sapConnection = null)
			{
				return dt.AsEnumerable().Select(r => Create(r, sapConnection));
			}

			public static List<GT_OUT> ToList(DataTable dt, ISapConnection sapConnection = null)
			{
				return Select(dt, sapConnection).ToListOrEmptyList();
			}

			public static IEnumerable<GT_OUT> Select(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				var tbl = dts.FirstOrDefault(t => t.TableName.ToLower() == typeof(GT_OUT).Name.ToLower());
				if (tbl == null)
					return null;

				return Select(tbl, sapConnection);
			}

			public static List<GT_OUT> ToList(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				return Select(dts, sapConnection).ToListOrEmptyList();
			}

			public static List<GT_OUT> ToList(ISapDataService sapDataService)
			{
				return ToList(sapDataService.GetExportTables(), sapDataService.SapConnection);
			}

			public static List<GT_OUT> GetExportListWithInitExecute(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<GT_OUT>();
				 
				var dts = sapDataService.GetExportTablesWithInitExecute("Z_V_UEBERF_AUFTR_UPL_PROT_01", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_OUT> GetExportListWithExecute(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_OUT>();
				 
				var dts = sapDataService.GetExportTablesWithExecute();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_OUT> GetExportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_OUT>();
				 
				var dts = sapDataService.GetExportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_OUT> GetImportListWithInit(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<GT_OUT>();
				 
				var dts = sapDataService.GetImportTablesWithInit("Z_V_UEBERF_AUFTR_UPL_PROT_01", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_OUT> GetImportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_OUT>();
				 
				var dts = sapDataService.GetImportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}
		}
	}

	public static partial class DataTableExtensions
	{

		public static DataTable ToTable(this IEnumerable<Z_V_UEBERF_AUFTR_UPL_PROT_01.GT_OUT> list)
		{
			return SapDataServiceExtensions.ToTable(list);
		}

	}
}
