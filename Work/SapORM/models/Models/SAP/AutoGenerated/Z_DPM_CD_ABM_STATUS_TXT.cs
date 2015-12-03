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
	public partial class Z_DPM_CD_ABM_STATUS_TXT
	{
		public static void Init(ISapDataService sap)
		{
			sap.Init(typeof(Z_DPM_CD_ABM_STATUS_TXT).Name);
		}

		public static void Init(ISapDataService sap, string inputParameterKeys, params object[] inputParameterValues)
		{
			sap.Init(typeof(Z_DPM_CD_ABM_STATUS_TXT).Name, inputParameterKeys, inputParameterValues);
		}


		public partial class ET_STATUS : IModelMappingApplied
		{
			[SapIgnore]
			[ScriptIgnore]
			public ISapConnection SAPConnection { get; set; }

			[SapIgnore]
			[ScriptIgnore]
			public IDynSapProxyFactory DynSapProxyFactory { get; set; }

			public string DOMNAME { get; set; }

			public string DDLANGUAGE { get; set; }

			public string AS4LOCAL { get; set; }

			public string VALPOS { get; set; }

			public string AS4VERS { get; set; }

			public string DDTEXT { get; set; }

			public string DOMVAL_LD { get; set; }

			public string DOMVAL_HD { get; set; }

			public string DOMVALUE_L { get; set; }

			public static ET_STATUS Create(DataRow row, ISapConnection sapConnection = null, IDynSapProxyFactory dynSapProxyFactory = null)
			{
				var o = new ET_STATUS
				{
					DOMNAME = (string)row["DOMNAME"],
					DDLANGUAGE = (string)row["DDLANGUAGE"],
					AS4LOCAL = (string)row["AS4LOCAL"],
					VALPOS = (string)row["VALPOS"],
					AS4VERS = (string)row["AS4VERS"],
					DDTEXT = (string)row["DDTEXT"],
					DOMVAL_LD = (string)row["DOMVAL_LD"],
					DOMVAL_HD = (string)row["DOMVAL_HD"],
					DOMVALUE_L = (string)row["DOMVALUE_L"],

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

			public static IEnumerable<ET_STATUS> Select(DataTable dt, ISapConnection sapConnection = null)
			{
				return dt.AsEnumerable().Select(r => Create(r, sapConnection));
			}

			public static List<ET_STATUS> ToList(DataTable dt, ISapConnection sapConnection = null)
			{
				return Select(dt, sapConnection).ToListOrEmptyList();
			}

			public static IEnumerable<ET_STATUS> Select(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				var tbl = dts.FirstOrDefault(t => t.TableName.ToLower() == typeof(ET_STATUS).Name.ToLower());
				if (tbl == null)
					return null;

				return Select(tbl, sapConnection);
			}

			public static List<ET_STATUS> ToList(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				return Select(dts, sapConnection).ToListOrEmptyList();
			}

			public static List<ET_STATUS> ToList(ISapDataService sapDataService)
			{
				return ToList(sapDataService.GetExportTables(), sapDataService.SapConnection);
			}

			public static List<ET_STATUS> GetExportListWithInitExecute(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<ET_STATUS>();
				 
				var dts = sapDataService.GetExportTablesWithInitExecute("Z_DPM_CD_ABM_STATUS_TXT", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<ET_STATUS> GetExportListWithExecute(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<ET_STATUS>();
				 
				var dts = sapDataService.GetExportTablesWithExecute();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<ET_STATUS> GetExportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<ET_STATUS>();
				 
				var dts = sapDataService.GetExportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<ET_STATUS> GetImportListWithInit(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<ET_STATUS>();
				 
				var dts = sapDataService.GetImportTablesWithInit("Z_DPM_CD_ABM_STATUS_TXT", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<ET_STATUS> GetImportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<ET_STATUS>();
				 
				var dts = sapDataService.GetImportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}
		}
	}

	public static partial class DataTableExtensions
	{

		public static DataTable ToTable(this IEnumerable<Z_DPM_CD_ABM_STATUS_TXT.ET_STATUS> list)
		{
			return SapDataServiceExtensions.ToTable(list);
		}

	}
}
