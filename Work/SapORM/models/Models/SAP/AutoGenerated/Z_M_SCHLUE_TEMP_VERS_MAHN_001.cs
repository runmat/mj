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
	public partial class Z_M_SCHLUE_TEMP_VERS_MAHN_001
	{
		public static void Init(ISapDataService sap)
		{
			sap.Init(typeof(Z_M_SCHLUE_TEMP_VERS_MAHN_001).Name);
		}

		public static void Init(ISapDataService sap, string inputParameterKeys, params object[] inputParameterValues)
		{
			sap.Init(typeof(Z_M_SCHLUE_TEMP_VERS_MAHN_001).Name, inputParameterKeys, inputParameterValues);
		}


		public static void SetImportParameter_I_KUNNR(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_KUNNR", value);
		}

		public partial class GT_WEB : IModelMappingApplied
		{
			[SapIgnore]
			[ScriptIgnore]
			public ISapConnection SAPConnection { get; set; }

			[SapIgnore]
			[ScriptIgnore]
			public IDynSapProxyFactory DynSapProxyFactory { get; set; }

			public string LICENSE_NUM { get; set; }

			public string CHASSIS_NUM { get; set; }

			public string TIDNR { get; set; }

			public string ADRNR { get; set; }

			public string NAME1 { get; set; }

			public string POST_CODE1 { get; set; }

			public string CITY1 { get; set; }

			public string STREET { get; set; }

			public DateTime? ZZTMPDT { get; set; }

			public string ZZVGRUND { get; set; }

			public string ZZVGRUND_TEXT { get; set; }

			public string EQUNR { get; set; }

			public string ERNAM { get; set; }

			public static GT_WEB Create(DataRow row, ISapConnection sapConnection = null, IDynSapProxyFactory dynSapProxyFactory = null)
			{
				var o = new GT_WEB
				{
					LICENSE_NUM = (string)row["LICENSE_NUM"],
					CHASSIS_NUM = (string)row["CHASSIS_NUM"],
					TIDNR = (string)row["TIDNR"],
					ADRNR = (string)row["ADRNR"],
					NAME1 = (string)row["NAME1"],
					POST_CODE1 = (string)row["POST_CODE1"],
					CITY1 = (string)row["CITY1"],
					STREET = (string)row["STREET"],
					ZZTMPDT = string.IsNullOrEmpty(row["ZZTMPDT"].ToString()) ? null : (DateTime?)row["ZZTMPDT"],
					ZZVGRUND = (string)row["ZZVGRUND"],
					ZZVGRUND_TEXT = (string)row["ZZVGRUND_TEXT"],
					EQUNR = (string)row["EQUNR"],
					ERNAM = (string)row["ERNAM"],

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

			public static IEnumerable<GT_WEB> Select(DataTable dt, ISapConnection sapConnection = null)
			{
				return dt.AsEnumerable().Select(r => Create(r, sapConnection));
			}

			public static List<GT_WEB> ToList(DataTable dt, ISapConnection sapConnection = null)
			{
				return Select(dt, sapConnection).ToListOrEmptyList();
			}

			public static IEnumerable<GT_WEB> Select(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				var tbl = dts.FirstOrDefault(t => t.TableName.ToLower() == typeof(GT_WEB).Name.ToLower());
				if (tbl == null)
					return null;

				return Select(tbl, sapConnection);
			}

			public static List<GT_WEB> ToList(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				return Select(dts, sapConnection).ToListOrEmptyList();
			}

			public static List<GT_WEB> ToList(ISapDataService sapDataService)
			{
				return ToList(sapDataService.GetExportTables(), sapDataService.SapConnection);
			}

			public static List<GT_WEB> GetExportListWithInitExecute(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<GT_WEB>();
				 
				var dts = sapDataService.GetExportTablesWithInitExecute("Z_M_SCHLUE_TEMP_VERS_MAHN_001", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_WEB> GetExportListWithExecute(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_WEB>();
				 
				var dts = sapDataService.GetExportTablesWithExecute();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_WEB> GetExportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_WEB>();
				 
				var dts = sapDataService.GetExportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_WEB> GetImportListWithInit(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<GT_WEB>();
				 
				var dts = sapDataService.GetImportTablesWithInit("Z_M_SCHLUE_TEMP_VERS_MAHN_001", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_WEB> GetImportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_WEB>();
				 
				var dts = sapDataService.GetImportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}
		}
	}

	public static partial class DataTableExtensions
	{

		public static DataTable ToTable(this IEnumerable<Z_M_SCHLUE_TEMP_VERS_MAHN_001.GT_WEB> list)
		{
			return SapDataServiceExtensions.ToTable(list);
		}

	}
}
