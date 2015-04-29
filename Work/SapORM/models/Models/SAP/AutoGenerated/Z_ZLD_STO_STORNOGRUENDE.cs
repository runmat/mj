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
	public partial class Z_ZLD_STO_STORNOGRUENDE
	{
		public static void Init(ISapDataService sap)
		{
			sap.Init(typeof(Z_ZLD_STO_STORNOGRUENDE).Name);
		}

		public static void Init(ISapDataService sap, string inputParameterKeys, params object[] inputParameterValues)
		{
			sap.Init(typeof(Z_ZLD_STO_STORNOGRUENDE).Name, inputParameterKeys, inputParameterValues);
		}

		public partial class GT_GRUENDE : IModelMappingApplied
		{
			[SapIgnore]
			[ScriptIgnore]
			public ISapConnection SAPConnection { get; set; }

			[SapIgnore]
			[ScriptIgnore]
			public IDynSapProxyFactory DynSapProxyFactory { get; set; }

			public string STORNOGRUND { get; set; }

			public string KUNDE_CHG { get; set; }

			public string PREISE_CHG { get; set; }

			public string AMT_CHG { get; set; }

			public string KENNZ_CHG { get; set; }

			public string GRUND_PFLICHT { get; set; }

			public string TXT40 { get; set; }

			public static GT_GRUENDE Create(DataRow row, ISapConnection sapConnection = null, IDynSapProxyFactory dynSapProxyFactory = null)
			{
				var o = new GT_GRUENDE
				{
					STORNOGRUND = (string)row["STORNOGRUND"],
					KUNDE_CHG = (string)row["KUNDE_CHG"],
					PREISE_CHG = (string)row["PREISE_CHG"],
					AMT_CHG = (string)row["AMT_CHG"],
					KENNZ_CHG = (string)row["KENNZ_CHG"],
					GRUND_PFLICHT = (string)row["GRUND_PFLICHT"],
					TXT40 = (string)row["TXT40"],

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

			public static IEnumerable<GT_GRUENDE> Select(DataTable dt, ISapConnection sapConnection = null)
			{
				return dt.AsEnumerable().Select(r => Create(r, sapConnection));
			}

			public static List<GT_GRUENDE> ToList(DataTable dt, ISapConnection sapConnection = null)
			{
				return Select(dt, sapConnection).ToListOrEmptyList();
			}

			public static IEnumerable<GT_GRUENDE> Select(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				var tbl = dts.FirstOrDefault(t => t.TableName.ToLower() == typeof(GT_GRUENDE).Name.ToLower());
				if (tbl == null)
					return null;

				return Select(tbl, sapConnection);
			}

			public static List<GT_GRUENDE> ToList(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				return Select(dts, sapConnection).ToListOrEmptyList();
			}

			public static List<GT_GRUENDE> ToList(ISapDataService sapDataService)
			{
				return ToList(sapDataService.GetExportTables(), sapDataService.SapConnection);
			}

			public static List<GT_GRUENDE> GetExportListWithInitExecute(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<GT_GRUENDE>();
				 
				var dts = sapDataService.GetExportTablesWithInitExecute("Z_ZLD_STO_STORNOGRUENDE", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_GRUENDE> GetExportListWithExecute(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_GRUENDE>();
				 
				var dts = sapDataService.GetExportTablesWithExecute();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_GRUENDE> GetExportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_GRUENDE>();
				 
				var dts = sapDataService.GetExportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_GRUENDE> GetImportListWithInit(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<GT_GRUENDE>();
				 
				var dts = sapDataService.GetImportTablesWithInit("Z_ZLD_STO_STORNOGRUENDE", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_GRUENDE> GetImportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_GRUENDE>();
				 
				var dts = sapDataService.GetImportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}
		}
	}

	public static partial class DataTableExtensions
	{

		public static DataTable ToTable(this IEnumerable<Z_ZLD_STO_STORNOGRUENDE.GT_GRUENDE> list)
		{
			return SapDataServiceExtensions.ToTable(list);
		}

		public static void Apply(this IEnumerable<Z_ZLD_STO_STORNOGRUENDE.GT_GRUENDE> list, DataTable dtDst)
		{
			SapDataServiceExtensions.Apply(list, dtDst);
		}

	}
}
