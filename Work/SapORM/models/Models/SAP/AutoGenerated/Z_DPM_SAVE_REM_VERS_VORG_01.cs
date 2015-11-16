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
	public partial class Z_DPM_SAVE_REM_VERS_VORG_01
	{
		public static void Init(ISapDataService sap)
		{
			sap.Init(typeof(Z_DPM_SAVE_REM_VERS_VORG_01).Name);
		}

		public static void Init(ISapDataService sap, string inputParameterKeys, params object[] inputParameterValues)
		{
			sap.Init(typeof(Z_DPM_SAVE_REM_VERS_VORG_01).Name, inputParameterKeys, inputParameterValues);
		}

		public partial class GT_TAB : IModelMappingApplied
		{
			[SapIgnore]
			[ScriptIgnore]
			public ISapConnection SAPConnection { get; set; }

			[SapIgnore]
			[ScriptIgnore]
			public IDynSapProxyFactory DynSapProxyFactory { get; set; }

			public string LAND_CODE { get; set; }

			public string HAENDLER { get; set; }

			public string VERSANDSPERRE { get; set; }

			public string BRIEF_SCHLUE_ADR { get; set; }

			public string NAME1_B { get; set; }

			public string NAME2_B { get; set; }

			public string STRASSE_B { get; set; }

			public string HAUSNR_B { get; set; }

			public string PLZ_B { get; set; }

			public string ORT_B { get; set; }

			public string NAME1_T { get; set; }

			public string NAME2_T { get; set; }

			public string STRASSE_T { get; set; }

			public string HAUSNR_T { get; set; }

			public string PLZ_T { get; set; }

			public string ORT_T { get; set; }

			public string CLIENT_NR { get; set; }

			public string CLIENTNAME { get; set; }

			public string BEM { get; set; }

			public string FORM_SPRAS { get; set; }

			public static GT_TAB Create(DataRow row, ISapConnection sapConnection = null, IDynSapProxyFactory dynSapProxyFactory = null)
			{
				var o = new GT_TAB
				{
					LAND_CODE = (string)row["LAND_CODE"],
					HAENDLER = (string)row["HAENDLER"],
					VERSANDSPERRE = (string)row["VERSANDSPERRE"],
					BRIEF_SCHLUE_ADR = (string)row["BRIEF_SCHLUE_ADR"],
					NAME1_B = (string)row["NAME1_B"],
					NAME2_B = (string)row["NAME2_B"],
					STRASSE_B = (string)row["STRASSE_B"],
					HAUSNR_B = (string)row["HAUSNR_B"],
					PLZ_B = (string)row["PLZ_B"],
					ORT_B = (string)row["ORT_B"],
					NAME1_T = (string)row["NAME1_T"],
					NAME2_T = (string)row["NAME2_T"],
					STRASSE_T = (string)row["STRASSE_T"],
					HAUSNR_T = (string)row["HAUSNR_T"],
					PLZ_T = (string)row["PLZ_T"],
					ORT_T = (string)row["ORT_T"],
					CLIENT_NR = (string)row["CLIENT_NR"],
					CLIENTNAME = (string)row["CLIENTNAME"],
					BEM = (string)row["BEM"],
					FORM_SPRAS = (string)row["FORM_SPRAS"],

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

			public static IEnumerable<GT_TAB> Select(DataTable dt, ISapConnection sapConnection = null)
			{
				return dt.AsEnumerable().Select(r => Create(r, sapConnection));
			}

			public static List<GT_TAB> ToList(DataTable dt, ISapConnection sapConnection = null)
			{
				return Select(dt, sapConnection).ToListOrEmptyList();
			}

			public static IEnumerable<GT_TAB> Select(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				var tbl = dts.FirstOrDefault(t => t.TableName.ToLower() == typeof(GT_TAB).Name.ToLower());
				if (tbl == null)
					return null;

				return Select(tbl, sapConnection);
			}

			public static List<GT_TAB> ToList(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				return Select(dts, sapConnection).ToListOrEmptyList();
			}

			public static List<GT_TAB> ToList(ISapDataService sapDataService)
			{
				return ToList(sapDataService.GetExportTables(), sapDataService.SapConnection);
			}

			public static List<GT_TAB> GetExportListWithInitExecute(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<GT_TAB>();
				 
				var dts = sapDataService.GetExportTablesWithInitExecute("Z_DPM_SAVE_REM_VERS_VORG_01", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_TAB> GetExportListWithExecute(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_TAB>();
				 
				var dts = sapDataService.GetExportTablesWithExecute();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_TAB> GetExportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_TAB>();
				 
				var dts = sapDataService.GetExportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_TAB> GetImportListWithInit(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<GT_TAB>();
				 
				var dts = sapDataService.GetImportTablesWithInit("Z_DPM_SAVE_REM_VERS_VORG_01", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_TAB> GetImportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_TAB>();
				 
				var dts = sapDataService.GetImportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}
		}
	}

	public static partial class DataTableExtensions
	{

		public static DataTable ToTable(this IEnumerable<Z_DPM_SAVE_REM_VERS_VORG_01.GT_TAB> list)
		{
			return SapDataServiceExtensions.ToTable(list);
		}

		public static void Apply(this IEnumerable<Z_DPM_SAVE_REM_VERS_VORG_01.GT_TAB> list, DataTable dtDst)
		{
			SapDataServiceExtensions.Apply(list, dtDst);
		}

	}
}
