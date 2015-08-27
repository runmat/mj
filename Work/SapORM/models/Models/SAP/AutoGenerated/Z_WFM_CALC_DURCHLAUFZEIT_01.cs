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
	public partial class Z_WFM_CALC_DURCHLAUFZEIT_01
	{
		public static void Init(ISapDataService sap)
		{
			sap.Init(typeof(Z_WFM_CALC_DURCHLAUFZEIT_01).Name);
		}

		public static void Init(ISapDataService sap, string inputParameterKeys, params object[] inputParameterValues)
		{
			sap.Init(typeof(Z_WFM_CALC_DURCHLAUFZEIT_01).Name, inputParameterKeys, inputParameterValues);
		}

		public partial class ES_STATISTIK : IModelMappingApplied
		{
			[SapIgnore]
			[ScriptIgnore]
			public ISapConnection SAPConnection { get; set; }

			[SapIgnore]
			[ScriptIgnore]
			public IDynSapProxyFactory DynSapProxyFactory { get; set; }

			public string DURCHSCHNITT_DAUER { get; set; }

			public string ANZ_GES { get; set; }

			public string ANZ_STD_LE_10 { get; set; }

			public string ANZ_STD_11_20 { get; set; }

			public string ANZ_STD_21_30 { get; set; }

			public string ANZ_STD_31_40 { get; set; }

			public string ANZ_STD_GT_40 { get; set; }

			public string ANZ_KLAER_LE_10 { get; set; }

			public string ANZ_KLAER_11_20 { get; set; }

			public string ANZ_KLAER_21_30 { get; set; }

			public string ANZ_KLAER_31_40 { get; set; }

			public string ANZ_KLAER_GT_40 { get; set; }

			public string ANZ_ALLE_LE_10 { get; set; }

			public string ANZ_ALLE_11_20 { get; set; }

			public string ANZ_ALLE_21_30 { get; set; }

			public string ANZ_ALLE_31_40 { get; set; }

			public string ANZ_ALLE_GT_40 { get; set; }

			public static ES_STATISTIK Create(DataRow row, ISapConnection sapConnection = null, IDynSapProxyFactory dynSapProxyFactory = null)
			{
				var o = new ES_STATISTIK
				{
					DURCHSCHNITT_DAUER = (string)row["DURCHSCHNITT_DAUER"],
					ANZ_GES = (string)row["ANZ_GES"],
					ANZ_STD_LE_10 = (string)row["ANZ_STD_LE_10"],
					ANZ_STD_11_20 = (string)row["ANZ_STD_11_20"],
					ANZ_STD_21_30 = (string)row["ANZ_STD_21_30"],
					ANZ_STD_31_40 = (string)row["ANZ_STD_31_40"],
					ANZ_STD_GT_40 = (string)row["ANZ_STD_GT_40"],
					ANZ_KLAER_LE_10 = (string)row["ANZ_KLAER_LE_10"],
					ANZ_KLAER_11_20 = (string)row["ANZ_KLAER_11_20"],
					ANZ_KLAER_21_30 = (string)row["ANZ_KLAER_21_30"],
					ANZ_KLAER_31_40 = (string)row["ANZ_KLAER_31_40"],
					ANZ_KLAER_GT_40 = (string)row["ANZ_KLAER_GT_40"],
					ANZ_ALLE_LE_10 = (string)row["ANZ_ALLE_LE_10"],
					ANZ_ALLE_11_20 = (string)row["ANZ_ALLE_11_20"],
					ANZ_ALLE_21_30 = (string)row["ANZ_ALLE_21_30"],
					ANZ_ALLE_31_40 = (string)row["ANZ_ALLE_31_40"],
					ANZ_ALLE_GT_40 = (string)row["ANZ_ALLE_GT_40"],

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

			public static IEnumerable<ES_STATISTIK> Select(DataTable dt, ISapConnection sapConnection = null)
			{
				return dt.AsEnumerable().Select(r => Create(r, sapConnection));
			}

			public static List<ES_STATISTIK> ToList(DataTable dt, ISapConnection sapConnection = null)
			{
				return Select(dt, sapConnection).ToListOrEmptyList();
			}

			public static IEnumerable<ES_STATISTIK> Select(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				var tbl = dts.FirstOrDefault(t => t.TableName.ToLower() == typeof(ES_STATISTIK).Name.ToLower());
				if (tbl == null)
					return null;

				return Select(tbl, sapConnection);
			}

			public static List<ES_STATISTIK> ToList(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				return Select(dts, sapConnection).ToListOrEmptyList();
			}

			public static List<ES_STATISTIK> ToList(ISapDataService sapDataService)
			{
				return ToList(sapDataService.GetExportTables(), sapDataService.SapConnection);
			}

			public static List<ES_STATISTIK> GetExportListWithInitExecute(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<ES_STATISTIK>();
				 
				var dts = sapDataService.GetExportTablesWithInitExecute("Z_WFM_CALC_DURCHLAUFZEIT_01", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<ES_STATISTIK> GetExportListWithExecute(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<ES_STATISTIK>();
				 
				var dts = sapDataService.GetExportTablesWithExecute();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<ES_STATISTIK> GetExportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<ES_STATISTIK>();
				 
				var dts = sapDataService.GetExportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<ES_STATISTIK> GetImportListWithInit(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<ES_STATISTIK>();
				 
				var dts = sapDataService.GetImportTablesWithInit("Z_WFM_CALC_DURCHLAUFZEIT_01", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<ES_STATISTIK> GetImportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<ES_STATISTIK>();
				 
				var dts = sapDataService.GetImportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}
		}

		public partial class ET_OUT : IModelMappingApplied
		{
			[SapIgnore]
			[ScriptIgnore]
			public ISapConnection SAPConnection { get; set; }

			[SapIgnore]
			[ScriptIgnore]
			public IDynSapProxyFactory DynSapProxyFactory { get; set; }

			public string KUNNR { get; set; }

			public string VORG_NR_ABM_AUF { get; set; }

			public DateTime? ERLEDIGT_DATUM { get; set; }

			public string ABMELDEART { get; set; }

			public string SELEKTION1 { get; set; }

			public string SELEKTION2 { get; set; }

			public string SELEKTION3 { get; set; }

			public string REFERENZ1 { get; set; }

			public string REFERENZ2 { get; set; }

			public string REFERENZ3 { get; set; }

			public string FAHRG { get; set; }

			public string KENNZ { get; set; }

			public string DURCHLAUFZEIT_STUNDEN { get; set; }

			public string DURCHLAUFZEIT_TAGE { get; set; }

			public DateTime? ANLAGEDATUM { get; set; }

			public static ET_OUT Create(DataRow row, ISapConnection sapConnection = null, IDynSapProxyFactory dynSapProxyFactory = null)
			{
				var o = new ET_OUT
				{
					KUNNR = (string)row["KUNNR"],
					VORG_NR_ABM_AUF = (string)row["VORG_NR_ABM_AUF"],
					ERLEDIGT_DATUM = (string.IsNullOrEmpty(row["ERLEDIGT_DATUM"].ToString())) ? null : (DateTime?)row["ERLEDIGT_DATUM"],
					ABMELDEART = (string)row["ABMELDEART"],
					SELEKTION1 = (string)row["SELEKTION1"],
					SELEKTION2 = (string)row["SELEKTION2"],
					SELEKTION3 = (string)row["SELEKTION3"],
					REFERENZ1 = (string)row["REFERENZ1"],
					REFERENZ2 = (string)row["REFERENZ2"],
					REFERENZ3 = (string)row["REFERENZ3"],
					FAHRG = (string)row["FAHRG"],
					KENNZ = (string)row["KENNZ"],
					DURCHLAUFZEIT_STUNDEN = (string)row["DURCHLAUFZEIT_STUNDEN"],
					DURCHLAUFZEIT_TAGE = (string)row["DURCHLAUFZEIT_TAGE"],
					ANLAGEDATUM = (string.IsNullOrEmpty(row["ANLAGEDATUM"].ToString())) ? null : (DateTime?)row["ANLAGEDATUM"],

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

			public static IEnumerable<ET_OUT> Select(DataTable dt, ISapConnection sapConnection = null)
			{
				return dt.AsEnumerable().Select(r => Create(r, sapConnection));
			}

			public static List<ET_OUT> ToList(DataTable dt, ISapConnection sapConnection = null)
			{
				return Select(dt, sapConnection).ToListOrEmptyList();
			}

			public static IEnumerable<ET_OUT> Select(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				var tbl = dts.FirstOrDefault(t => t.TableName.ToLower() == typeof(ET_OUT).Name.ToLower());
				if (tbl == null)
					return null;

				return Select(tbl, sapConnection);
			}

			public static List<ET_OUT> ToList(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				return Select(dts, sapConnection).ToListOrEmptyList();
			}

			public static List<ET_OUT> ToList(ISapDataService sapDataService)
			{
				return ToList(sapDataService.GetExportTables(), sapDataService.SapConnection);
			}

			public static List<ET_OUT> GetExportListWithInitExecute(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<ET_OUT>();
				 
				var dts = sapDataService.GetExportTablesWithInitExecute("Z_WFM_CALC_DURCHLAUFZEIT_01", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<ET_OUT> GetExportListWithExecute(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<ET_OUT>();
				 
				var dts = sapDataService.GetExportTablesWithExecute();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<ET_OUT> GetExportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<ET_OUT>();
				 
				var dts = sapDataService.GetExportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<ET_OUT> GetImportListWithInit(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<ET_OUT>();
				 
				var dts = sapDataService.GetImportTablesWithInit("Z_WFM_CALC_DURCHLAUFZEIT_01", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<ET_OUT> GetImportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<ET_OUT>();
				 
				var dts = sapDataService.GetImportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}
		}
	}

	public static partial class DataTableExtensions
	{

		public static DataTable ToTable(this IEnumerable<Z_WFM_CALC_DURCHLAUFZEIT_01.ES_STATISTIK> list)
		{
			return SapDataServiceExtensions.ToTable(list);
		}

		public static void Apply(this IEnumerable<Z_WFM_CALC_DURCHLAUFZEIT_01.ES_STATISTIK> list, DataTable dtDst)
		{
			SapDataServiceExtensions.Apply(list, dtDst);
		}


		public static DataTable ToTable(this IEnumerable<Z_WFM_CALC_DURCHLAUFZEIT_01.ET_OUT> list)
		{
			return SapDataServiceExtensions.ToTable(list);
		}

		public static void Apply(this IEnumerable<Z_WFM_CALC_DURCHLAUFZEIT_01.ET_OUT> list, DataTable dtDst)
		{
			SapDataServiceExtensions.Apply(list, dtDst);
		}

	}
}
