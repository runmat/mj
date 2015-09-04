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
	public partial class Z_AHP_READ_VERSUNTERNEHMEN
	{
		public static void Init(ISapDataService sap)
		{
			sap.Init(typeof(Z_AHP_READ_VERSUNTERNEHMEN).Name);
		}

		public static void Init(ISapDataService sap, string inputParameterKeys, params object[] inputParameterValues)
		{
			sap.Init(typeof(Z_AHP_READ_VERSUNTERNEHMEN).Name, inputParameterKeys, inputParameterValues);
		}

		public partial class GT_OUT : IModelMappingApplied
		{
			[SapIgnore]
			[ScriptIgnore]
			public ISapConnection SAPConnection { get; set; }

			[SapIgnore]
			[ScriptIgnore]
			public IDynSapProxyFactory DynSapProxyFactory { get; set; }

			public string MANDT { get; set; }

			public string EVB2 { get; set; }

			public string VSU_NR { get; set; }

			public string NAME { get; set; }

			public string POST_CODE1 { get; set; }

			public string CITY1 { get; set; }

			public string COUNTRY { get; set; }

			public string STREET { get; set; }

			public string HOUSE_NUM1 { get; set; }

			public string TEL_NUMBER { get; set; }

			public string FAX_NUMBER { get; set; }

			public static GT_OUT Create(DataRow row, ISapConnection sapConnection = null, IDynSapProxyFactory dynSapProxyFactory = null)
			{
				var o = new GT_OUT
				{
					MANDT = (string)row["MANDT"],
					EVB2 = (string)row["EVB2"],
					VSU_NR = (string)row["VSU_NR"],
					NAME = (string)row["NAME"],
					POST_CODE1 = (string)row["POST_CODE1"],
					CITY1 = (string)row["CITY1"],
					COUNTRY = (string)row["COUNTRY"],
					STREET = (string)row["STREET"],
					HOUSE_NUM1 = (string)row["HOUSE_NUM1"],
					TEL_NUMBER = (string)row["TEL_NUMBER"],
					FAX_NUMBER = (string)row["FAX_NUMBER"],

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
				return Select(dt, sapConnection).ToListOrEmptyList();
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
				return Select(dts, sapConnection).ToListOrEmptyList();
			}

			public static List<GT_OUT> ToList(ISapDataService sapDataService)
			{
				return ToList(sapDataService.GetExportTables(), sapDataService.SapConnection);
			}

			public static List<GT_OUT> GetExportListWithInitExecute(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<GT_OUT>();
				 
				var dts = sapDataService.GetExportTablesWithInitExecute("Z_AHP_READ_VERSUNTERNEHMEN", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_OUT> GetExportListWithExecute(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_OUT>();
				 
				var dts = sapDataService.GetExportTablesWithExecute();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_OUT> GetExportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_OUT>();
				 
				var dts = sapDataService.GetExportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_OUT> GetImportListWithInit(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<GT_OUT>();
				 
				var dts = sapDataService.GetImportTablesWithInit("Z_AHP_READ_VERSUNTERNEHMEN", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_OUT> GetImportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_OUT>();
				 
				var dts = sapDataService.GetImportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}
		}
	}

	public static partial class DataTableExtensions
	{

		public static DataTable ToTable(this IEnumerable<Z_AHP_READ_VERSUNTERNEHMEN.GT_OUT> list)
		{
			return SapDataServiceExtensions.ToTable(list);
		}

		public static void Apply(this IEnumerable<Z_AHP_READ_VERSUNTERNEHMEN.GT_OUT> list, DataTable dtDst)
		{
			SapDataServiceExtensions.Apply(list, dtDst);
		}

	}
}
