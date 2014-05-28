using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Script.Serialization;
using GeneralTools.Contracts;
using SapORM.Contracts;

namespace SapORM.Models
{
	public partial class Z_M_UNB_HAENDLER
	{
		public static void Init(ISapDataService sap)
		{
			sap.Init(typeof(Z_M_UNB_HAENDLER).Name);
		}

		public static void Init(ISapDataService sap, string inputParameterKeys, params object[] inputParameterValues)
		{
			sap.Init(typeof(Z_M_UNB_HAENDLER).Name, inputParameterKeys, inputParameterValues);
		}

		public partial class GT_WEB : IModelMappingApplied
		{
			[SapIgnore]
			[ScriptIgnore]
			public ISapConnection SAPConnection { get; set; }

			[SapIgnore]
			[ScriptIgnore]
			public IDynSapProxyFactory DynSapProxyFactory { get; set; }

			public string LIZNR { get; set; }

			public string ZZFINART { get; set; }

			public string CHASSIS_NUM { get; set; }

			public string TIDNR { get; set; }

			public string LICENSE_NUM { get; set; }

			public string ZZCOCKZ { get; set; }

			public DateTime? ERDAT { get; set; }

			public string ZZMODID { get; set; }

			public string ZZHANDELSNAME { get; set; }

			public string ORI_HAENDLER { get; set; }

			public string LOESCHBAR { get; set; }

			public static GT_WEB Create(DataRow row, ISapConnection sapConnection = null, IDynSapProxyFactory dynSapProxyFactory = null)
			{
				var o = new GT_WEB
				{
					LIZNR = (string)row["LIZNR"],
					ZZFINART = (string)row["ZZFINART"],
					CHASSIS_NUM = (string)row["CHASSIS_NUM"],
					TIDNR = (string)row["TIDNR"],
					LICENSE_NUM = (string)row["LICENSE_NUM"],
					ZZCOCKZ = (string)row["ZZCOCKZ"],
					ERDAT = (string.IsNullOrEmpty(row["ERDAT"].ToString())) ? null : (DateTime?)row["ERDAT"],
					ZZMODID = (string)row["ZZMODID"],
					ZZHANDELSNAME = (string)row["ZZHANDELSNAME"],
					ORI_HAENDLER = (string)row["ORI_HAENDLER"],
					LOESCHBAR = (string)row["LOESCHBAR"],

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
				return Select(dt, sapConnection).ToList();
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
				return Select(dts, sapConnection).ToList();
			}

			public static List<GT_WEB> ToList(ISapDataService sapDataService)
			{
				return ToList(sapDataService.GetExportTables(), sapDataService.SapConnection);
			}

			public static List<GT_WEB> GetExportListWithInitExecute(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<GT_WEB>();
				 
				var dts = sapDataService.GetExportTablesWithInitExecute("Z_M_UNB_HAENDLER", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToList();
			}

			public static List<GT_WEB> GetExportListWithExecute(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_WEB>();
				 
				var dts = sapDataService.GetExportTablesWithExecute();
				 
				return Select(dts, sapDataService.SapConnection).ToList();
			}

			public static List<GT_WEB> GetExportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_WEB>();
				 
				var dts = sapDataService.GetExportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToList();
			}

			public static List<GT_WEB> GetImportListWithInit(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<GT_WEB>();
				 
				var dts = sapDataService.GetImportTablesWithInit("Z_M_UNB_HAENDLER", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToList();
			}

			public static List<GT_WEB> GetImportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_WEB>();
				 
				var dts = sapDataService.GetImportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToList();
			}
		}
	}

	public static partial class DataTableExtensions
	{

		public static DataTable ToTable(this IEnumerable<Z_M_UNB_HAENDLER.GT_WEB> list)
		{
			return SapDataServiceExtensions.ToTable(list);
		}

		public static void Apply(this IEnumerable<Z_M_UNB_HAENDLER.GT_WEB> list, DataTable dtDst)
		{
			SapDataServiceExtensions.Apply(list, dtDst);
		}

	}
}
