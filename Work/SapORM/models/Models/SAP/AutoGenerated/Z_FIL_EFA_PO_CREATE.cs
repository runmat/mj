using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Script.Serialization;
using GeneralTools.Contracts;
using SapORM.Contracts;

namespace SapORM.Models
{
	public partial class Z_FIL_EFA_PO_CREATE
	{
		public static void Init(ISapDataService sap)
		{
			sap.Init(typeof(Z_FIL_EFA_PO_CREATE).Name);
		}

		public static void Init(ISapDataService sap, string inputParameterKeys, params object[] inputParameterValues)
		{
			sap.Init(typeof(Z_FIL_EFA_PO_CREATE).Name, inputParameterKeys, inputParameterValues);
		}

		public partial class GT_POS : IModelMappingApplied
		{
			[SapIgnore]
			[ScriptIgnore]
			public ISapConnection SAPConnection { get; set; }

			[SapIgnore]
			[ScriptIgnore]
			public IDynSapProxyFactory DynSapProxyFactory { get; set; }

			public string ARTLIF { get; set; }

			public decimal? MENGE { get; set; }

			public string ZUSINFO_TXT { get; set; }

			public decimal? PREIS { get; set; }

			public string LTEXT_NR { get; set; }

			public static GT_POS Create(DataRow row, ISapConnection sapConnection = null, IDynSapProxyFactory dynSapProxyFactory = null)
			{
				var o = new GT_POS
				{
					ARTLIF = (string)row["ARTLIF"],
					MENGE = (decimal?)row["MENGE"],
					ZUSINFO_TXT = (string)row["ZUSINFO_TXT"],
					PREIS = (decimal?)row["PREIS"],
					LTEXT_NR = (string)row["LTEXT_NR"],

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

			public static IEnumerable<GT_POS> Select(DataTable dt, ISapConnection sapConnection = null)
			{
				return dt.AsEnumerable().Select(r => Create(r, sapConnection));
			}

			public static List<GT_POS> ToList(DataTable dt, ISapConnection sapConnection = null)
			{
				return Select(dt, sapConnection).ToList();
			}

			public static IEnumerable<GT_POS> Select(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				var tbl = dts.FirstOrDefault(t => t.TableName.ToLower() == typeof(GT_POS).Name.ToLower());
				if (tbl == null)
					return null;

				return Select(tbl, sapConnection);
			}

			public static List<GT_POS> ToList(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				return Select(dts, sapConnection).ToList();
			}

			public static List<GT_POS> ToList(ISapDataService sapDataService)
			{
				return ToList(sapDataService.GetExportTables(), sapDataService.SapConnection);
			}

			public static List<GT_POS> GetExportListWithInitExecute(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<GT_POS>();
				 
				var dts = sapDataService.GetExportTablesWithInitExecute("Z_FIL_EFA_PO_CREATE", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToList();
			}

			public static List<GT_POS> GetExportListWithExecute(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_POS>();
				 
				var dts = sapDataService.GetExportTablesWithExecute();
				 
				return Select(dts, sapDataService.SapConnection).ToList();
			}

			public static List<GT_POS> GetExportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_POS>();
				 
				var dts = sapDataService.GetExportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToList();
			}

			public static List<GT_POS> GetImportListWithInit(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<GT_POS>();
				 
				var dts = sapDataService.GetImportTablesWithInit("Z_FIL_EFA_PO_CREATE", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToList();
			}

			public static List<GT_POS> GetImportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_POS>();
				 
				var dts = sapDataService.GetImportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToList();
			}
		}
	}

	public static partial class DataTableExtensions
	{

		public static DataTable ToTable(this IEnumerable<Z_FIL_EFA_PO_CREATE.GT_POS> list)
		{
			return SapDataServiceExtensions.ToTable(list);
		}

		public static void Apply(this IEnumerable<Z_FIL_EFA_PO_CREATE.GT_POS> list, DataTable dtDst)
		{
			SapDataServiceExtensions.Apply(list, dtDst);
		}

	}
}
