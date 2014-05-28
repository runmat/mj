using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Script.Serialization;
using GeneralTools.Contracts;
using SapORM.Contracts;

namespace SapORM.Models
{
	public partial class Z_ZLD_MOB_KREISKZ
	{

		public partial class GT_KREISKZ : IModelMappingApplied
		{
			[SapIgnore]
			[ScriptIgnore]
			public ISapConnection SAPConnection { get; set; }

			[SapIgnore]
			[ScriptIgnore]
			public IDynSapProxyFactory DynSapProxyFactory { get; set; }

			public string KREISKZ { get; set; }

			public string KREISBEZ { get; set; }

			public static GT_KREISKZ Create(DataRow row, ISapConnection sapConnection = null, IDynSapProxyFactory dynSapProxyFactory = null)
			{
				var o = new GT_KREISKZ
				{
					KREISKZ = (string)row["KREISKZ"],
					KREISBEZ = (string)row["KREISBEZ"],

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

			public static IEnumerable<GT_KREISKZ> Select(DataTable dt, ISapConnection sapConnection = null)
			{
				return dt.AsEnumerable().Select(r => Create(r, sapConnection));
			}

			public static List<GT_KREISKZ> ToList(DataTable dt, ISapConnection sapConnection = null)
			{
				return Select(dt, sapConnection).ToList();
			}

			public static IEnumerable<GT_KREISKZ> Select(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				var tbl = dts.FirstOrDefault(t => t.TableName.ToLower() == typeof(GT_KREISKZ).Name.ToLower());
				if (tbl == null)
					return null;

				return Select(tbl, sapConnection);
			}

			public static List<GT_KREISKZ> ToList(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				return Select(dts, sapConnection).ToList();
			}

			public static List<GT_KREISKZ> ToList(ISapDataService sapDataService)
			{
				return ToList(sapDataService.GetExportTables(), sapDataService.SapConnection);
			}

			public static List<GT_KREISKZ> GetExportListWithInitExecute(ISapDataService sapDataService, string inputParameterKeys, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<GT_KREISKZ>();
				 
				var dts = sapDataService.GetExportTablesWithInitExecute("Z_ZLD_MOB_KREISKZ", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToList();
			}

			public static List<GT_KREISKZ> GetExportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_KREISKZ>();
				 
				var dts = sapDataService.GetExportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToList();
			}

			public static List<GT_KREISKZ> GetImportListWithInit(ISapDataService sapDataService, string inputParameterKeys, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<GT_KREISKZ>();
				 
				var dts = sapDataService.GetImportTablesWithInit("Z_ZLD_MOB_KREISKZ", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToList();
			}

			public static List<GT_KREISKZ> GetImportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_KREISKZ>();
				 
				var dts = sapDataService.GetImportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToList();
			}
		}
	}

	public static partial class DataTableExtensions
	{

		public static DataTable ToTable(this IEnumerable<Z_ZLD_MOB_KREISKZ.GT_KREISKZ> list)
		{
			return SapDataServiceExtensions.ToTable(list);
		}

		public static void Apply(this IEnumerable<Z_ZLD_MOB_KREISKZ.GT_KREISKZ> list, DataTable dtDst)
		{
			SapDataServiceExtensions.Apply(list, dtDst);
		}

	}
}
