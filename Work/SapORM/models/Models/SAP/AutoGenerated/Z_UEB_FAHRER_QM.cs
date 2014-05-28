using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Script.Serialization;
using GeneralTools.Contracts;
using SapORM.Contracts;

namespace SapORM.Models
{
	public partial class Z_UEB_FAHRER_QM
	{
		public static void Init(ISapDataService sap)
		{
			sap.Init(typeof(Z_UEB_FAHRER_QM).Name);
		}

		public static void Init(ISapDataService sap, string inputParameterKeys, params object[] inputParameterValues)
		{
			sap.Init(typeof(Z_UEB_FAHRER_QM).Name, inputParameterKeys, inputParameterValues);
		}

		public partial class ET_QM : IModelMappingApplied
		{
			[SapIgnore]
			[ScriptIgnore]
			public ISapConnection SAPConnection { get; set; }

			[SapIgnore]
			[ScriptIgnore]
			public IDynSapProxyFactory DynSapProxyFactory { get; set; }

			public string FEGRP { get; set; }

			public string FECOD { get; set; }

			public string KATALOGTXT { get; set; }

			public decimal? MENGE_SEL { get; set; }

			public decimal? MENGE_VORJAHR { get; set; }

			public static ET_QM Create(DataRow row, ISapConnection sapConnection = null, IDynSapProxyFactory dynSapProxyFactory = null)
			{
				var o = new ET_QM
				{
					FEGRP = (string)row["FEGRP"],
					FECOD = (string)row["FECOD"],
					KATALOGTXT = (string)row["KATALOGTXT"],
					MENGE_SEL = (decimal?)row["MENGE_SEL"],
					MENGE_VORJAHR = (decimal?)row["MENGE_VORJAHR"],

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

			public static IEnumerable<ET_QM> Select(DataTable dt, ISapConnection sapConnection = null)
			{
				return dt.AsEnumerable().Select(r => Create(r, sapConnection));
			}

			public static List<ET_QM> ToList(DataTable dt, ISapConnection sapConnection = null)
			{
				return Select(dt, sapConnection).ToList();
			}

			public static IEnumerable<ET_QM> Select(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				var tbl = dts.FirstOrDefault(t => t.TableName.ToLower() == typeof(ET_QM).Name.ToLower());
				if (tbl == null)
					return null;

				return Select(tbl, sapConnection);
			}

			public static List<ET_QM> ToList(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				return Select(dts, sapConnection).ToList();
			}

			public static List<ET_QM> ToList(ISapDataService sapDataService)
			{
				return ToList(sapDataService.GetExportTables(), sapDataService.SapConnection);
			}

			public static List<ET_QM> GetExportListWithInitExecute(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<ET_QM>();
				 
				var dts = sapDataService.GetExportTablesWithInitExecute("Z_UEB_FAHRER_QM", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToList();
			}

			public static List<ET_QM> GetExportListWithExecute(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<ET_QM>();
				 
				var dts = sapDataService.GetExportTablesWithExecute();
				 
				return Select(dts, sapDataService.SapConnection).ToList();
			}

			public static List<ET_QM> GetExportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<ET_QM>();
				 
				var dts = sapDataService.GetExportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToList();
			}

			public static List<ET_QM> GetImportListWithInit(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<ET_QM>();
				 
				var dts = sapDataService.GetImportTablesWithInit("Z_UEB_FAHRER_QM", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToList();
			}

			public static List<ET_QM> GetImportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<ET_QM>();
				 
				var dts = sapDataService.GetImportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToList();
			}
		}

		public partial class ET_FLEET : IModelMappingApplied
		{
			[SapIgnore]
			[ScriptIgnore]
			public ISapConnection SAPConnection { get; set; }

			[SapIgnore]
			[ScriptIgnore]
			public IDynSapProxyFactory DynSapProxyFactory { get; set; }

			public string BEWERTUNG { get; set; }

			public decimal? FRAGE5 { get; set; }

			public decimal? FRAGE6 { get; set; }

			public decimal? FRAGE7 { get; set; }

			public decimal? FRAGE8 { get; set; }

			public decimal? FRAGE9 { get; set; }

			public decimal? FRAGE10 { get; set; }

			public decimal? FRAGE11 { get; set; }

			public static ET_FLEET Create(DataRow row, ISapConnection sapConnection = null, IDynSapProxyFactory dynSapProxyFactory = null)
			{
				var o = new ET_FLEET
				{
					BEWERTUNG = (string)row["BEWERTUNG"],
					FRAGE5 = (decimal?)row["FRAGE5"],
					FRAGE6 = (decimal?)row["FRAGE6"],
					FRAGE7 = (decimal?)row["FRAGE7"],
					FRAGE8 = (decimal?)row["FRAGE8"],
					FRAGE9 = (decimal?)row["FRAGE9"],
					FRAGE10 = (decimal?)row["FRAGE10"],
					FRAGE11 = (decimal?)row["FRAGE11"],

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

			public static IEnumerable<ET_FLEET> Select(DataTable dt, ISapConnection sapConnection = null)
			{
				return dt.AsEnumerable().Select(r => Create(r, sapConnection));
			}

			public static List<ET_FLEET> ToList(DataTable dt, ISapConnection sapConnection = null)
			{
				return Select(dt, sapConnection).ToList();
			}

			public static IEnumerable<ET_FLEET> Select(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				var tbl = dts.FirstOrDefault(t => t.TableName.ToLower() == typeof(ET_FLEET).Name.ToLower());
				if (tbl == null)
					return null;

				return Select(tbl, sapConnection);
			}

			public static List<ET_FLEET> ToList(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				return Select(dts, sapConnection).ToList();
			}

			public static List<ET_FLEET> ToList(ISapDataService sapDataService)
			{
				return ToList(sapDataService.GetExportTables(), sapDataService.SapConnection);
			}

			public static List<ET_FLEET> GetExportListWithInitExecute(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<ET_FLEET>();
				 
				var dts = sapDataService.GetExportTablesWithInitExecute("Z_UEB_FAHRER_QM", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToList();
			}

			public static List<ET_FLEET> GetExportListWithExecute(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<ET_FLEET>();
				 
				var dts = sapDataService.GetExportTablesWithExecute();
				 
				return Select(dts, sapDataService.SapConnection).ToList();
			}

			public static List<ET_FLEET> GetExportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<ET_FLEET>();
				 
				var dts = sapDataService.GetExportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToList();
			}

			public static List<ET_FLEET> GetImportListWithInit(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<ET_FLEET>();
				 
				var dts = sapDataService.GetImportTablesWithInit("Z_UEB_FAHRER_QM", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToList();
			}

			public static List<ET_FLEET> GetImportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<ET_FLEET>();
				 
				var dts = sapDataService.GetImportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToList();
			}
		}
	}

	public static partial class DataTableExtensions
	{

		public static DataTable ToTable(this IEnumerable<Z_UEB_FAHRER_QM.ET_QM> list)
		{
			return SapDataServiceExtensions.ToTable(list);
		}

		public static void Apply(this IEnumerable<Z_UEB_FAHRER_QM.ET_QM> list, DataTable dtDst)
		{
			SapDataServiceExtensions.Apply(list, dtDst);
		}


		public static DataTable ToTable(this IEnumerable<Z_UEB_FAHRER_QM.ET_FLEET> list)
		{
			return SapDataServiceExtensions.ToTable(list);
		}

		public static void Apply(this IEnumerable<Z_UEB_FAHRER_QM.ET_FLEET> list, DataTable dtDst)
		{
			SapDataServiceExtensions.Apply(list, dtDst);
		}

	}
}
