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
	public partial class Z_DPM_READ_AUFTR_FAHR_EDISPO_1
	{
		public static void Init(ISapDataService sap)
		{
			sap.Init(typeof(Z_DPM_READ_AUFTR_FAHR_EDISPO_1).Name);
		}

		public static void Init(ISapDataService sap, string inputParameterKeys, params object[] inputParameterValues)
		{
			sap.Init(typeof(Z_DPM_READ_AUFTR_FAHR_EDISPO_1).Name, inputParameterKeys, inputParameterValues);
		}


		public static void SetImportParameter_I_FAHRER(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_FAHRER", value);
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

			public string NAME1_AG { get; set; }

			public string CITY1_ZB { get; set; }

			public string POST_CODE1_ZB { get; set; }

			public string CITY1_WE { get; set; }

			public string POST_CODE1_WE { get; set; }

			public string CITY1_ZR { get; set; }

			public string POST_CODE1_ZR { get; set; }

			public string FAHRZEUGTYP_H { get; set; }

			public string FAHRZEUGTYP_R { get; set; }

			public DateTime? AUN_DAT { get; set; }

			public string AUN_TIM_VON_H { get; set; }

			public string AUN_TIM_BIS_H { get; set; }

			public DateTime? AUG_DAT { get; set; }

			public string AUN_TIM_VON_R { get; set; }

			public string AUN_TIM_BIS_R { get; set; }

			private bool MappingErrorProcessed { get; set; }

			public static GT_ORDER Create(DataRow row, ISapConnection sapConnection = null, IDynSapProxyFactory dynSapProxyFactory = null)
			{
				GT_ORDER o;

				try
				{
					o = new GT_ORDER
					{
						SAPConnection = sapConnection,
						DynSapProxyFactory = dynSapProxyFactory,

						VBELN = (string)row["VBELN"],
						NAME1_AG = (string)row["NAME1_AG"],
						CITY1_ZB = (string)row["CITY1_ZB"],
						POST_CODE1_ZB = (string)row["POST_CODE1_ZB"],
						CITY1_WE = (string)row["CITY1_WE"],
						POST_CODE1_WE = (string)row["POST_CODE1_WE"],
						CITY1_ZR = (string)row["CITY1_ZR"],
						POST_CODE1_ZR = (string)row["POST_CODE1_ZR"],
						FAHRZEUGTYP_H = (string)row["FAHRZEUGTYP_H"],
						FAHRZEUGTYP_R = (string)row["FAHRZEUGTYP_R"],
						AUN_DAT = string.IsNullOrEmpty(row["AUN_DAT"].ToString()) ? null : (DateTime?)row["AUN_DAT"],
						AUN_TIM_VON_H = (string)row["AUN_TIM_VON_H"],
						AUN_TIM_BIS_H = (string)row["AUN_TIM_BIS_H"],
						AUG_DAT = string.IsNullOrEmpty(row["AUG_DAT"].ToString()) ? null : (DateTime?)row["AUG_DAT"],
						AUN_TIM_VON_R = (string)row["AUN_TIM_VON_R"],
						AUN_TIM_BIS_R = (string)row["AUN_TIM_BIS_R"],
					};
				}
				catch(Exception e)
				{
					o = new GT_ORDER
					{
						SAPConnection = sapConnection,
						DynSapProxyFactory = dynSapProxyFactory,
					};
					o.OnMappingError(e, row, true);
					if (!o.MappingErrorProcessed)
						throw;
				}

				o.OnInitFromSap();
				return o;
			}

			partial void OnInitFromSap();

			partial void OnMappingError(Exception e, DataRow row, bool isExport);

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
				return Select(dt, sapConnection).ToListOrEmptyList();
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
				return Select(dts, sapConnection).ToListOrEmptyList();
			}

			public static List<GT_ORDER> ToList(ISapDataService sapDataService)
			{
				return ToList(sapDataService.GetExportTables(), sapDataService.SapConnection);
			}

			public static List<GT_ORDER> GetExportListWithInitExecute(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<GT_ORDER>();
				 
				var dts = sapDataService.GetExportTablesWithInitExecute("Z_DPM_READ_AUFTR_FAHR_EDISPO_1", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_ORDER> GetExportListWithExecute(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_ORDER>();
				 
				var dts = sapDataService.GetExportTablesWithExecute();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_ORDER> GetExportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_ORDER>();
				 
				var dts = sapDataService.GetExportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_ORDER> GetImportListWithInit(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<GT_ORDER>();
				 
				var dts = sapDataService.GetImportTablesWithInit("Z_DPM_READ_AUFTR_FAHR_EDISPO_1", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_ORDER> GetImportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_ORDER>();
				 
				var dts = sapDataService.GetImportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}
		}
	}

	public static partial class DataTableExtensions
	{

		public static DataTable ToTable(this IEnumerable<Z_DPM_READ_AUFTR_FAHR_EDISPO_1.GT_ORDER> list)
		{
			return SapDataServiceExtensions.ToTable(list);
		}

	}
}
