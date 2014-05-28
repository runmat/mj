using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Script.Serialization;
using GeneralTools.Contracts;
using SapORM.Contracts;

namespace SapORM.Models
{
	public partial class Z_DPM_CD_ABM_HIST
	{
		public static void Init(ISapDataService sap)
		{
			sap.Init(typeof(Z_DPM_CD_ABM_HIST).Name);
		}

		public static void Init(ISapDataService sap, string inputParameterKeys, params object[] inputParameterValues)
		{
			sap.Init(typeof(Z_DPM_CD_ABM_HIST).Name, inputParameterKeys, inputParameterValues);
		}

		public partial class ET_ABM_HIST : IModelMappingApplied
		{
			[SapIgnore]
			[ScriptIgnore]
			public ISapConnection SAPConnection { get; set; }

			[SapIgnore]
			[ScriptIgnore]
			public IDynSapProxyFactory DynSapProxyFactory { get; set; }

			public string MANDT { get; set; }

			public string KUNNR_AG { get; set; }

			public string CDKFN { get; set; }

			public string LFNDR { get; set; }

			public DateTime? DATBI { get; set; }

			public string TODO_TXT { get; set; }

			public string BEMERKUNG_TXT { get; set; }

			public DateTime? ERDAT { get; set; }

			public string ERNAM { get; set; }

			public static ET_ABM_HIST Create(DataRow row, ISapConnection sapConnection = null, IDynSapProxyFactory dynSapProxyFactory = null)
			{
				var o = new ET_ABM_HIST
				{
					MANDT = (string)row["MANDT"],
					KUNNR_AG = (string)row["KUNNR_AG"],
					CDKFN = (string)row["CDKFN"],
					LFNDR = (string)row["LFNDR"],
					DATBI = (string.IsNullOrEmpty(row["DATBI"].ToString())) ? null : (DateTime?)row["DATBI"],
					TODO_TXT = (string)row["TODO_TXT"],
					BEMERKUNG_TXT = (string)row["BEMERKUNG_TXT"],
					ERDAT = (string.IsNullOrEmpty(row["ERDAT"].ToString())) ? null : (DateTime?)row["ERDAT"],
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

			public static IEnumerable<ET_ABM_HIST> Select(DataTable dt, ISapConnection sapConnection = null)
			{
				return dt.AsEnumerable().Select(r => Create(r, sapConnection));
			}

			public static List<ET_ABM_HIST> ToList(DataTable dt, ISapConnection sapConnection = null)
			{
				return Select(dt, sapConnection).ToList();
			}

			public static IEnumerable<ET_ABM_HIST> Select(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				var tbl = dts.FirstOrDefault(t => t.TableName.ToLower() == typeof(ET_ABM_HIST).Name.ToLower());
				if (tbl == null)
					return null;

				return Select(tbl, sapConnection);
			}

			public static List<ET_ABM_HIST> ToList(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				return Select(dts, sapConnection).ToList();
			}

			public static List<ET_ABM_HIST> ToList(ISapDataService sapDataService)
			{
				return ToList(sapDataService.GetExportTables(), sapDataService.SapConnection);
			}

			public static List<ET_ABM_HIST> GetExportListWithInitExecute(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<ET_ABM_HIST>();
				 
				var dts = sapDataService.GetExportTablesWithInitExecute("Z_DPM_CD_ABM_HIST", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToList();
			}

			public static List<ET_ABM_HIST> GetExportListWithExecute(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<ET_ABM_HIST>();
				 
				var dts = sapDataService.GetExportTablesWithExecute();
				 
				return Select(dts, sapDataService.SapConnection).ToList();
			}

			public static List<ET_ABM_HIST> GetExportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<ET_ABM_HIST>();
				 
				var dts = sapDataService.GetExportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToList();
			}

			public static List<ET_ABM_HIST> GetImportListWithInit(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<ET_ABM_HIST>();
				 
				var dts = sapDataService.GetImportTablesWithInit("Z_DPM_CD_ABM_HIST", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToList();
			}

			public static List<ET_ABM_HIST> GetImportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<ET_ABM_HIST>();
				 
				var dts = sapDataService.GetImportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToList();
			}
		}
	}

	public static partial class DataTableExtensions
	{

		public static DataTable ToTable(this IEnumerable<Z_DPM_CD_ABM_HIST.ET_ABM_HIST> list)
		{
			return SapDataServiceExtensions.ToTable(list);
		}

		public static void Apply(this IEnumerable<Z_DPM_CD_ABM_HIST.ET_ABM_HIST> list, DataTable dtDst)
		{
			SapDataServiceExtensions.Apply(list, dtDst);
		}

	}
}
