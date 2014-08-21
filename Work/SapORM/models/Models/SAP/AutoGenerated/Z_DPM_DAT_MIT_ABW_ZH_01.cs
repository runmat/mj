using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Script.Serialization;
using GeneralTools.Contracts;
using SapORM.Contracts;

namespace SapORM.Models
{
	public partial class Z_DPM_DAT_MIT_ABW_ZH_01
	{
		public static void Init(ISapDataService sap)
		{
			sap.Init(typeof(Z_DPM_DAT_MIT_ABW_ZH_01).Name);
		}

		public static void Init(ISapDataService sap, string inputParameterKeys, params object[] inputParameterValues)
		{
			sap.Init(typeof(Z_DPM_DAT_MIT_ABW_ZH_01).Name, inputParameterKeys, inputParameterValues);
		}

		public partial class GT_OUT : IModelMappingApplied
		{
			[SapIgnore]
			[ScriptIgnore]
			public ISapConnection SAPConnection { get; set; }

			[SapIgnore]
			[ScriptIgnore]
			public IDynSapProxyFactory DynSapProxyFactory { get; set; }

			public string CHASSIS_NUM { get; set; }

			public string LIZNR { get; set; }

			public string NAME1_ZL { get; set; }

			public string NAME2_ZL { get; set; }

			public string STREET_ZL { get; set; }

			public string HOUSE_NUM1_ZL { get; set; }

			public string POST_CODE1_ZL { get; set; }

			public string CITY1_ZL { get; set; }

			public string NAME1_ZH { get; set; }

			public string NAME2_ZH { get; set; }

			public string STREET_ZH { get; set; }

			public string HOUSE_NUM1_ZH { get; set; }

			public string POST_CODE1_ZH { get; set; }

			public string CITY1_ZH { get; set; }

			public DateTime? DAT_VERTR_BEG { get; set; }

			public DateTime? DAT_VERTR_END { get; set; }

			public static GT_OUT Create(DataRow row, ISapConnection sapConnection = null, IDynSapProxyFactory dynSapProxyFactory = null)
			{
				var o = new GT_OUT
				{
					CHASSIS_NUM = (string)row["CHASSIS_NUM"],
					LIZNR = (string)row["LIZNR"],
					NAME1_ZL = (string)row["NAME1_ZL"],
					NAME2_ZL = (string)row["NAME2_ZL"],
					STREET_ZL = (string)row["STREET_ZL"],
					HOUSE_NUM1_ZL = (string)row["HOUSE_NUM1_ZL"],
					POST_CODE1_ZL = (string)row["POST_CODE1_ZL"],
					CITY1_ZL = (string)row["CITY1_ZL"],
					NAME1_ZH = (string)row["NAME1_ZH"],
					NAME2_ZH = (string)row["NAME2_ZH"],
					STREET_ZH = (string)row["STREET_ZH"],
					HOUSE_NUM1_ZH = (string)row["HOUSE_NUM1_ZH"],
					POST_CODE1_ZH = (string)row["POST_CODE1_ZH"],
					CITY1_ZH = (string)row["CITY1_ZH"],
					DAT_VERTR_BEG = (string.IsNullOrEmpty(row["DAT_VERTR_BEG"].ToString())) ? null : (DateTime?)row["DAT_VERTR_BEG"],
					DAT_VERTR_END = (string.IsNullOrEmpty(row["DAT_VERTR_END"].ToString())) ? null : (DateTime?)row["DAT_VERTR_END"],

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
				return Select(dt, sapConnection).ToList();
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
				return Select(dts, sapConnection).ToList();
			}

			public static List<GT_OUT> ToList(ISapDataService sapDataService)
			{
				return ToList(sapDataService.GetExportTables(), sapDataService.SapConnection);
			}

			public static List<GT_OUT> GetExportListWithInitExecute(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<GT_OUT>();
				 
				var dts = sapDataService.GetExportTablesWithInitExecute("Z_DPM_DAT_MIT_ABW_ZH_01", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToList();
			}

			public static List<GT_OUT> GetExportListWithExecute(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_OUT>();
				 
				var dts = sapDataService.GetExportTablesWithExecute();
				 
				return Select(dts, sapDataService.SapConnection).ToList();
			}

			public static List<GT_OUT> GetExportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_OUT>();
				 
				var dts = sapDataService.GetExportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToList();
			}

			public static List<GT_OUT> GetImportListWithInit(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<GT_OUT>();
				 
				var dts = sapDataService.GetImportTablesWithInit("Z_DPM_DAT_MIT_ABW_ZH_01", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToList();
			}

			public static List<GT_OUT> GetImportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_OUT>();
				 
				var dts = sapDataService.GetImportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToList();
			}
		}
	}

	public static partial class DataTableExtensions
	{

		public static DataTable ToTable(this IEnumerable<Z_DPM_DAT_MIT_ABW_ZH_01.GT_OUT> list)
		{
			return SapDataServiceExtensions.ToTable(list);
		}

		public static void Apply(this IEnumerable<Z_DPM_DAT_MIT_ABW_ZH_01.GT_OUT> list, DataTable dtDst)
		{
			SapDataServiceExtensions.Apply(list, dtDst);
		}

	}
}
