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
	public partial class Z_VB_IMPORT_FALL
	{
		public static void Init(ISapDataService sap)
		{
			sap.Init(typeof(Z_VB_IMPORT_FALL).Name);
		}

		public static void Init(ISapDataService sap, string inputParameterKeys, params object[] inputParameterValues)
		{
			sap.Init(typeof(Z_VB_IMPORT_FALL).Name, inputParameterKeys, inputParameterValues);
		}


		public partial class GT_VERBANDBUCH : IModelMappingApplied
		{
			[SapIgnore]
			[ScriptIgnore]
			public ISapConnection SAPConnection { get; set; }

			[SapIgnore]
			[ScriptIgnore]
			public IDynSapProxyFactory DynSapProxyFactory { get; set; }

			public string ID { get; set; }

			public string VKBUR { get; set; }

			public string NAME_VERL { get; set; }

			public DateTime? DATUM_UNF { get; set; }

			public string ZEIT_UNF { get; set; }

			public string ORT { get; set; }

			public string HERGANG { get; set; }

			public string NAME_ZEUG { get; set; }

			public string ART_VERL { get; set; }

			public DateTime? DATUM_HILF { get; set; }

			public string ZEIT_HILF { get; set; }

			public string ART_HILF { get; set; }

			public string NAME_HELFER { get; set; }

			public static GT_VERBANDBUCH Create(DataRow row, ISapConnection sapConnection = null, IDynSapProxyFactory dynSapProxyFactory = null)
			{
				var o = new GT_VERBANDBUCH
				{
					ID = (string)row["ID"],
					VKBUR = (string)row["VKBUR"],
					NAME_VERL = (string)row["NAME_VERL"],
					DATUM_UNF = string.IsNullOrEmpty(row["DATUM_UNF"].ToString()) ? null : (DateTime?)row["DATUM_UNF"],
					ZEIT_UNF = (string)row["ZEIT_UNF"],
					ORT = (string)row["ORT"],
					HERGANG = (string)row["HERGANG"],
					NAME_ZEUG = (string)row["NAME_ZEUG"],
					ART_VERL = (string)row["ART_VERL"],
					DATUM_HILF = string.IsNullOrEmpty(row["DATUM_HILF"].ToString()) ? null : (DateTime?)row["DATUM_HILF"],
					ZEIT_HILF = (string)row["ZEIT_HILF"],
					ART_HILF = (string)row["ART_HILF"],
					NAME_HELFER = (string)row["NAME_HELFER"],

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

			public static IEnumerable<GT_VERBANDBUCH> Select(DataTable dt, ISapConnection sapConnection = null)
			{
				return dt.AsEnumerable().Select(r => Create(r, sapConnection));
			}

			public static List<GT_VERBANDBUCH> ToList(DataTable dt, ISapConnection sapConnection = null)
			{
				return Select(dt, sapConnection).ToListOrEmptyList();
			}

			public static IEnumerable<GT_VERBANDBUCH> Select(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				var tbl = dts.FirstOrDefault(t => t.TableName.ToLower() == typeof(GT_VERBANDBUCH).Name.ToLower());
				if (tbl == null)
					return null;

				return Select(tbl, sapConnection);
			}

			public static List<GT_VERBANDBUCH> ToList(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				return Select(dts, sapConnection).ToListOrEmptyList();
			}

			public static List<GT_VERBANDBUCH> ToList(ISapDataService sapDataService)
			{
				return ToList(sapDataService.GetExportTables(), sapDataService.SapConnection);
			}

			public static List<GT_VERBANDBUCH> GetExportListWithInitExecute(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<GT_VERBANDBUCH>();
				 
				var dts = sapDataService.GetExportTablesWithInitExecute("Z_VB_IMPORT_FALL", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_VERBANDBUCH> GetExportListWithExecute(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_VERBANDBUCH>();
				 
				var dts = sapDataService.GetExportTablesWithExecute();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_VERBANDBUCH> GetExportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_VERBANDBUCH>();
				 
				var dts = sapDataService.GetExportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_VERBANDBUCH> GetImportListWithInit(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<GT_VERBANDBUCH>();
				 
				var dts = sapDataService.GetImportTablesWithInit("Z_VB_IMPORT_FALL", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_VERBANDBUCH> GetImportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_VERBANDBUCH>();
				 
				var dts = sapDataService.GetImportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}
		}
	}

	public static partial class DataTableExtensions
	{

		public static DataTable ToTable(this IEnumerable<Z_VB_IMPORT_FALL.GT_VERBANDBUCH> list)
		{
			return SapDataServiceExtensions.ToTable(list);
		}

	}
}
