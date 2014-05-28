using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Script.Serialization;
using GeneralTools.Contracts;
using SapORM.Contracts;

namespace SapORM.Models
{
	public partial class Z_M_GET_FAHRER_AUFTRAEGE
	{
		public static void Init(ISapDataService sap)
		{
			sap.Init(typeof(Z_M_GET_FAHRER_AUFTRAEGE).Name);
		}

		public static void Init(ISapDataService sap, string inputParameterKeys, params object[] inputParameterValues)
		{
			sap.Init(typeof(Z_M_GET_FAHRER_AUFTRAEGE).Name, inputParameterKeys, inputParameterValues);
		}

		public partial class GT_ORDER : IModelMappingApplied
		{
			[SapIgnore]
			[ScriptIgnore]
			public ISapConnection SAPConnection { get; set; }

			[SapIgnore]
			[ScriptIgnore]
			public IDynSapProxyFactory DynSapProxyFactory { get; set; }

			public string VBELN { get; set; }

			public DateTime? ERDAT { get; set; }

			public string EBELN { get; set; }

			public DateTime? VDATU { get; set; }

			public DateTime? AUN_DAT { get; set; }

			public string ZB_POST_CODE1 { get; set; }

			public string ZB_CITY1 { get; set; }

			public string WE_POST_CODE1 { get; set; }

			public string WE_CITY1 { get; set; }

			public string ZR_POST_CODE1 { get; set; }

			public string ZR_CITY1 { get; set; }

			public string FAHRER_STATUS { get; set; }

			public string FAHRER { get; set; }

			public string FAHRER_NAME1 { get; set; }

			public string FAHRER_NAME2 { get; set; }

			public string KUNNR_AG { get; set; }

			public static GT_ORDER Create(DataRow row, ISapConnection sapConnection = null, IDynSapProxyFactory dynSapProxyFactory = null)
			{
				var o = new GT_ORDER
				{
					VBELN = (string)row["VBELN"],
					ERDAT = (string.IsNullOrEmpty(row["ERDAT"].ToString())) ? null : (DateTime?)row["ERDAT"],
					EBELN = (string)row["EBELN"],
					VDATU = (string.IsNullOrEmpty(row["VDATU"].ToString())) ? null : (DateTime?)row["VDATU"],
					AUN_DAT = (string.IsNullOrEmpty(row["AUN_DAT"].ToString())) ? null : (DateTime?)row["AUN_DAT"],
					ZB_POST_CODE1 = (string)row["ZB_POST_CODE1"],
					ZB_CITY1 = (string)row["ZB_CITY1"],
					WE_POST_CODE1 = (string)row["WE_POST_CODE1"],
					WE_CITY1 = (string)row["WE_CITY1"],
					ZR_POST_CODE1 = (string)row["ZR_POST_CODE1"],
					ZR_CITY1 = (string)row["ZR_CITY1"],
					FAHRER_STATUS = (string)row["FAHRER_STATUS"],
					FAHRER = (string)row["FAHRER"],
					FAHRER_NAME1 = (string)row["FAHRER_NAME1"],
					FAHRER_NAME2 = (string)row["FAHRER_NAME2"],
					KUNNR_AG = (string)row["KUNNR_AG"],

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

			public static IEnumerable<GT_ORDER> Select(DataTable dt, ISapConnection sapConnection = null)
			{
				return dt.AsEnumerable().Select(r => Create(r, sapConnection));
			}

			public static List<GT_ORDER> ToList(DataTable dt, ISapConnection sapConnection = null)
			{
				return Select(dt, sapConnection).ToList();
			}

			public static IEnumerable<GT_ORDER> Select(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				var tbl = dts.FirstOrDefault(t => t.TableName.ToLower() == typeof(GT_ORDER).Name.ToLower());
				if (tbl == null)
					return null;

				return Select(tbl, sapConnection);
			}

			public static List<GT_ORDER> ToList(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				return Select(dts, sapConnection).ToList();
			}

			public static List<GT_ORDER> ToList(ISapDataService sapDataService)
			{
				return ToList(sapDataService.GetExportTables(), sapDataService.SapConnection);
			}

			public static List<GT_ORDER> GetExportListWithInitExecute(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<GT_ORDER>();
				 
				var dts = sapDataService.GetExportTablesWithInitExecute("Z_M_GET_FAHRER_AUFTRAEGE", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToList();
			}

			public static List<GT_ORDER> GetExportListWithExecute(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_ORDER>();
				 
				var dts = sapDataService.GetExportTablesWithExecute();
				 
				return Select(dts, sapDataService.SapConnection).ToList();
			}

			public static List<GT_ORDER> GetExportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_ORDER>();
				 
				var dts = sapDataService.GetExportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToList();
			}

			public static List<GT_ORDER> GetImportListWithInit(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<GT_ORDER>();
				 
				var dts = sapDataService.GetImportTablesWithInit("Z_M_GET_FAHRER_AUFTRAEGE", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToList();
			}

			public static List<GT_ORDER> GetImportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_ORDER>();
				 
				var dts = sapDataService.GetImportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToList();
			}
		}
	}

	public static partial class DataTableExtensions
	{

		public static DataTable ToTable(this IEnumerable<Z_M_GET_FAHRER_AUFTRAEGE.GT_ORDER> list)
		{
			return SapDataServiceExtensions.ToTable(list);
		}

		public static void Apply(this IEnumerable<Z_M_GET_FAHRER_AUFTRAEGE.GT_ORDER> list, DataTable dtDst)
		{
			SapDataServiceExtensions.Apply(list, dtDst);
		}

	}
}
