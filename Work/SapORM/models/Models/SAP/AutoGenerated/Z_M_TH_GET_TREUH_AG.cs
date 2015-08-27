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
	public partial class Z_M_TH_GET_TREUH_AG
	{
		public static void Init(ISapDataService sap)
		{
			sap.Init(typeof(Z_M_TH_GET_TREUH_AG).Name);
		}

		public static void Init(ISapDataService sap, string inputParameterKeys, params object[] inputParameterValues)
		{
			sap.Init(typeof(Z_M_TH_GET_TREUH_AG).Name, inputParameterKeys, inputParameterValues);
		}

		public partial class GT_EXP : IModelMappingApplied
		{
			[SapIgnore]
			[ScriptIgnore]
			public ISapConnection SAPConnection { get; set; }

			[SapIgnore]
			[ScriptIgnore]
			public IDynSapProxyFactory DynSapProxyFactory { get; set; }

			public string MANDT { get; set; }

			public string TREU { get; set; }

			public string AG { get; set; }

			public string EQTYP { get; set; }

			public string TREUH_YA { get; set; }

			public string TREUH_YZ { get; set; }

			public string TREUH_YE { get; set; }

			public string TREUH_YT { get; set; }

			public string TREUH_KEY { get; set; }

			public string SMTP_ADDR { get; set; }

			public string NAME1_AG { get; set; }

			public string ZSELECT { get; set; }

			public string NAME1_TG { get; set; }

			public static GT_EXP Create(DataRow row, ISapConnection sapConnection = null, IDynSapProxyFactory dynSapProxyFactory = null)
			{
				var o = new GT_EXP
				{
					MANDT = (string)row["MANDT"],
					TREU = (string)row["TREU"],
					AG = (string)row["AG"],
					EQTYP = (string)row["EQTYP"],
					TREUH_YA = (string)row["TREUH_YA"],
					TREUH_YZ = (string)row["TREUH_YZ"],
					TREUH_YE = (string)row["TREUH_YE"],
					TREUH_YT = (string)row["TREUH_YT"],
					TREUH_KEY = (string)row["TREUH_KEY"],
					SMTP_ADDR = (string)row["SMTP_ADDR"],
					NAME1_AG = (string)row["NAME1_AG"],
					ZSELECT = (string)row["ZSELECT"],
					NAME1_TG = (string)row["NAME1_TG"],

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

			public static IEnumerable<GT_EXP> Select(DataTable dt, ISapConnection sapConnection = null)
			{
				return dt.AsEnumerable().Select(r => Create(r, sapConnection));
			}

			public static List<GT_EXP> ToList(DataTable dt, ISapConnection sapConnection = null)
			{
				return Select(dt, sapConnection).ToListOrEmptyList();
			}

			public static IEnumerable<GT_EXP> Select(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				var tbl = dts.FirstOrDefault(t => t.TableName.ToLower() == typeof(GT_EXP).Name.ToLower());
				if (tbl == null)
					return null;

				return Select(tbl, sapConnection);
			}

			public static List<GT_EXP> ToList(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				return Select(dts, sapConnection).ToListOrEmptyList();
			}

			public static List<GT_EXP> ToList(ISapDataService sapDataService)
			{
				return ToList(sapDataService.GetExportTables(), sapDataService.SapConnection);
			}

			public static List<GT_EXP> GetExportListWithInitExecute(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<GT_EXP>();
				 
				var dts = sapDataService.GetExportTablesWithInitExecute("Z_M_TH_GET_TREUH_AG", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_EXP> GetExportListWithExecute(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_EXP>();
				 
				var dts = sapDataService.GetExportTablesWithExecute();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_EXP> GetExportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_EXP>();
				 
				var dts = sapDataService.GetExportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_EXP> GetImportListWithInit(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<GT_EXP>();
				 
				var dts = sapDataService.GetImportTablesWithInit("Z_M_TH_GET_TREUH_AG", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_EXP> GetImportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_EXP>();
				 
				var dts = sapDataService.GetImportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}
		}
	}

	public static partial class DataTableExtensions
	{

		public static DataTable ToTable(this IEnumerable<Z_M_TH_GET_TREUH_AG.GT_EXP> list)
		{
			return SapDataServiceExtensions.ToTable(list);
		}

		public static void Apply(this IEnumerable<Z_M_TH_GET_TREUH_AG.GT_EXP> list, DataTable dtDst)
		{
			SapDataServiceExtensions.Apply(list, dtDst);
		}

	}
}
