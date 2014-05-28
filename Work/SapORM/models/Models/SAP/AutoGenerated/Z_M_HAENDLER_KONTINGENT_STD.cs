using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Script.Serialization;
using GeneralTools.Contracts;
using SapORM.Contracts;

namespace SapORM.Models
{
	public partial class Z_M_HAENDLER_KONTINGENT_STD
	{
		public static void Init(ISapDataService sap)
		{
			sap.Init(typeof(Z_M_HAENDLER_KONTINGENT_STD).Name);
		}

		public static void Init(ISapDataService sap, string inputParameterKeys, params object[] inputParameterValues)
		{
			sap.Init(typeof(Z_M_HAENDLER_KONTINGENT_STD).Name, inputParameterKeys, inputParameterValues);
		}

		public partial class EX_ADRS : IModelMappingApplied
		{
			[SapIgnore]
			[ScriptIgnore]
			public ISapConnection SAPConnection { get; set; }

			[SapIgnore]
			[ScriptIgnore]
			public IDynSapProxyFactory DynSapProxyFactory { get; set; }

			public string AG { get; set; }

			public string HDGRP_EX { get; set; }

			public string HDGRP { get; set; }

			public string HAENDLER_EX { get; set; }

			public string HAENDLER { get; set; }

			public string NAME1 { get; set; }

			public string NAME2 { get; set; }

			public string STRAS { get; set; }

			public string PSTLZ { get; set; }

			public string ORT01 { get; set; }

			public string LAND1 { get; set; }

			public static EX_ADRS Create(DataRow row, ISapConnection sapConnection = null, IDynSapProxyFactory dynSapProxyFactory = null)
			{
				var o = new EX_ADRS
				{
					AG = (string)row["AG"],
					HDGRP_EX = (string)row["HDGRP_EX"],
					HDGRP = (string)row["HDGRP"],
					HAENDLER_EX = (string)row["HAENDLER_EX"],
					HAENDLER = (string)row["HAENDLER"],
					NAME1 = (string)row["NAME1"],
					NAME2 = (string)row["NAME2"],
					STRAS = (string)row["STRAS"],
					PSTLZ = (string)row["PSTLZ"],
					ORT01 = (string)row["ORT01"],
					LAND1 = (string)row["LAND1"],

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

			public static IEnumerable<EX_ADRS> Select(DataTable dt, ISapConnection sapConnection = null)
			{
				return dt.AsEnumerable().Select(r => Create(r, sapConnection));
			}

			public static List<EX_ADRS> ToList(DataTable dt, ISapConnection sapConnection = null)
			{
				return Select(dt, sapConnection).ToList();
			}

			public static IEnumerable<EX_ADRS> Select(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				var tbl = dts.FirstOrDefault(t => t.TableName.ToLower() == typeof(EX_ADRS).Name.ToLower());
				if (tbl == null)
					return null;

				return Select(tbl, sapConnection);
			}

			public static List<EX_ADRS> ToList(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				return Select(dts, sapConnection).ToList();
			}

			public static List<EX_ADRS> ToList(ISapDataService sapDataService)
			{
				return ToList(sapDataService.GetExportTables(), sapDataService.SapConnection);
			}

			public static List<EX_ADRS> GetExportListWithInitExecute(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<EX_ADRS>();
				 
				var dts = sapDataService.GetExportTablesWithInitExecute("Z_M_HAENDLER_KONTINGENT_STD", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToList();
			}

			public static List<EX_ADRS> GetExportListWithExecute(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<EX_ADRS>();
				 
				var dts = sapDataService.GetExportTablesWithExecute();
				 
				return Select(dts, sapDataService.SapConnection).ToList();
			}

			public static List<EX_ADRS> GetExportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<EX_ADRS>();
				 
				var dts = sapDataService.GetExportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToList();
			}

			public static List<EX_ADRS> GetImportListWithInit(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<EX_ADRS>();
				 
				var dts = sapDataService.GetImportTablesWithInit("Z_M_HAENDLER_KONTINGENT_STD", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToList();
			}

			public static List<EX_ADRS> GetImportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<EX_ADRS>();
				 
				var dts = sapDataService.GetImportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToList();
			}
		}

		public partial class GT_LIMIT : IModelMappingApplied
		{
			[SapIgnore]
			[ScriptIgnore]
			public ISapConnection SAPConnection { get; set; }

			[SapIgnore]
			[ScriptIgnore]
			public IDynSapProxyFactory DynSapProxyFactory { get; set; }

			public string KKBER { get; set; }

			public string RECART { get; set; }

			public string SPEERKZ { get; set; }

			public decimal? KLIMK { get; set; }

			public decimal? SKFOR { get; set; }

			public decimal? FREIKONTI { get; set; }

			public static GT_LIMIT Create(DataRow row, ISapConnection sapConnection = null, IDynSapProxyFactory dynSapProxyFactory = null)
			{
				var o = new GT_LIMIT
				{
					KKBER = (string)row["KKBER"],
					RECART = (string)row["RECART"],
					SPEERKZ = (string)row["SPEERKZ"],
					KLIMK = (decimal?)row["KLIMK"],
					SKFOR = (decimal?)row["SKFOR"],
					FREIKONTI = (decimal?)row["FREIKONTI"],

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

			public static IEnumerable<GT_LIMIT> Select(DataTable dt, ISapConnection sapConnection = null)
			{
				return dt.AsEnumerable().Select(r => Create(r, sapConnection));
			}

			public static List<GT_LIMIT> ToList(DataTable dt, ISapConnection sapConnection = null)
			{
				return Select(dt, sapConnection).ToList();
			}

			public static IEnumerable<GT_LIMIT> Select(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				var tbl = dts.FirstOrDefault(t => t.TableName.ToLower() == typeof(GT_LIMIT).Name.ToLower());
				if (tbl == null)
					return null;

				return Select(tbl, sapConnection);
			}

			public static List<GT_LIMIT> ToList(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				return Select(dts, sapConnection).ToList();
			}

			public static List<GT_LIMIT> ToList(ISapDataService sapDataService)
			{
				return ToList(sapDataService.GetExportTables(), sapDataService.SapConnection);
			}

			public static List<GT_LIMIT> GetExportListWithInitExecute(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<GT_LIMIT>();
				 
				var dts = sapDataService.GetExportTablesWithInitExecute("Z_M_HAENDLER_KONTINGENT_STD", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToList();
			}

			public static List<GT_LIMIT> GetExportListWithExecute(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_LIMIT>();
				 
				var dts = sapDataService.GetExportTablesWithExecute();
				 
				return Select(dts, sapDataService.SapConnection).ToList();
			}

			public static List<GT_LIMIT> GetExportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_LIMIT>();
				 
				var dts = sapDataService.GetExportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToList();
			}

			public static List<GT_LIMIT> GetImportListWithInit(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<GT_LIMIT>();
				 
				var dts = sapDataService.GetImportTablesWithInit("Z_M_HAENDLER_KONTINGENT_STD", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToList();
			}

			public static List<GT_LIMIT> GetImportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_LIMIT>();
				 
				var dts = sapDataService.GetImportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToList();
			}
		}
	}

	public static partial class DataTableExtensions
	{

		public static DataTable ToTable(this IEnumerable<Z_M_HAENDLER_KONTINGENT_STD.EX_ADRS> list)
		{
			return SapDataServiceExtensions.ToTable(list);
		}

		public static void Apply(this IEnumerable<Z_M_HAENDLER_KONTINGENT_STD.EX_ADRS> list, DataTable dtDst)
		{
			SapDataServiceExtensions.Apply(list, dtDst);
		}


		public static DataTable ToTable(this IEnumerable<Z_M_HAENDLER_KONTINGENT_STD.GT_LIMIT> list)
		{
			return SapDataServiceExtensions.ToTable(list);
		}

		public static void Apply(this IEnumerable<Z_M_HAENDLER_KONTINGENT_STD.GT_LIMIT> list, DataTable dtDst)
		{
			SapDataServiceExtensions.Apply(list, dtDst);
		}

	}
}
