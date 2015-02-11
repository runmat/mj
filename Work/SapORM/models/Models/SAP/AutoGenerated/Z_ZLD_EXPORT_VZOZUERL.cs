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
	public partial class Z_ZLD_EXPORT_VZOZUERL
	{
		public static void Init(ISapDataService sap)
		{
			sap.Init(typeof(Z_ZLD_EXPORT_VZOZUERL).Name);
		}

		public static void Init(ISapDataService sap, string inputParameterKeys, params object[] inputParameterValues)
		{
			sap.Init(typeof(Z_ZLD_EXPORT_VZOZUERL).Name, inputParameterKeys, inputParameterValues);
		}

		public partial class GT_EX_ZUERL : IModelMappingApplied
		{
			[SapIgnore]
			[ScriptIgnore]
			public ISapConnection SAPConnection { get; set; }

			[SapIgnore]
			[ScriptIgnore]
			public IDynSapProxyFactory DynSapProxyFactory { get; set; }

			public string ZULBELN { get; set; }

			public string VBELN { get; set; }

			public string VKORG { get; set; }

			public string VKBUR { get; set; }

			public DateTime? ERDAT { get; set; }

			public string ERNAM { get; set; }

			public string BLTYP { get; set; }

			public string FLAG { get; set; }

			public string STATUS { get; set; }

			public DateTime? VZERDAT { get; set; }

			public string BARCODE { get; set; }

			public string KUNNR { get; set; }

			public string KUNDEN_NAME1 { get; set; }

			public string ZZREFNR1 { get; set; }

			public string ZZREFNR2 { get; set; }

			public string KREISKZ { get; set; }

			public string KREISBEZ { get; set; }

			public string WUNSCHKENN_JN { get; set; }

			public string RESERVKENN_JN { get; set; }

			public string RESERVKENN { get; set; }

			public string FEINSTAUBAMT { get; set; }

			public DateTime? ZZZLDAT { get; set; }

			public string ZZKENN { get; set; }

			public string KENNZTYP { get; set; }

			public string KENNZFORM { get; set; }

			public string KENNZANZ { get; set; }

			public string EINKENN_JN { get; set; }

			public string BEMERKUNG { get; set; }

			public string EC_JN { get; set; }

			public string BAR_JN { get; set; }

			public string ZL_RL_FRBNR_HIN { get; set; }

			public string ZL_RL_FRBNR_ZUR { get; set; }

			public string ZL_LIFNR { get; set; }

			public string ZULPOSNR { get; set; }

			public string UEPOS { get; set; }

			public string LOEKZ { get; set; }

			public decimal? MENGE { get; set; }

			public string MATNR { get; set; }

			public string MAKTX { get; set; }

			public decimal? PREIS { get; set; }

			public string WEBMTART { get; set; }

			public decimal? KBETR { get; set; }

			public string NAME1 { get; set; }

			public string VZB_STATUS { get; set; }

			public static GT_EX_ZUERL Create(DataRow row, ISapConnection sapConnection = null, IDynSapProxyFactory dynSapProxyFactory = null)
			{
				var o = new GT_EX_ZUERL
				{
					ZULBELN = (string)row["ZULBELN"],
					VBELN = (string)row["VBELN"],
					VKORG = (string)row["VKORG"],
					VKBUR = (string)row["VKBUR"],
					ERDAT = (string.IsNullOrEmpty(row["ERDAT"].ToString())) ? null : (DateTime?)row["ERDAT"],
					ERNAM = (string)row["ERNAM"],
					BLTYP = (string)row["BLTYP"],
					FLAG = (string)row["FLAG"],
					STATUS = (string)row["STATUS"],
					VZERDAT = (string.IsNullOrEmpty(row["VZERDAT"].ToString())) ? null : (DateTime?)row["VZERDAT"],
					BARCODE = (string)row["BARCODE"],
					KUNNR = (string)row["KUNNR"],
					KUNDEN_NAME1 = (string)row["KUNDEN_NAME1"],
					ZZREFNR1 = (string)row["ZZREFNR1"],
					ZZREFNR2 = (string)row["ZZREFNR2"],
					KREISKZ = (string)row["KREISKZ"],
					KREISBEZ = (string)row["KREISBEZ"],
					WUNSCHKENN_JN = (string)row["WUNSCHKENN_JN"],
					RESERVKENN_JN = (string)row["RESERVKENN_JN"],
					RESERVKENN = (string)row["RESERVKENN"],
					FEINSTAUBAMT = (string)row["FEINSTAUBAMT"],
					ZZZLDAT = (string.IsNullOrEmpty(row["ZZZLDAT"].ToString())) ? null : (DateTime?)row["ZZZLDAT"],
					ZZKENN = (string)row["ZZKENN"],
					KENNZTYP = (string)row["KENNZTYP"],
					KENNZFORM = (string)row["KENNZFORM"],
					KENNZANZ = (string)row["KENNZANZ"],
					EINKENN_JN = (string)row["EINKENN_JN"],
					BEMERKUNG = (string)row["BEMERKUNG"],
					EC_JN = (string)row["EC_JN"],
					BAR_JN = (string)row["BAR_JN"],
					ZL_RL_FRBNR_HIN = (string)row["ZL_RL_FRBNR_HIN"],
					ZL_RL_FRBNR_ZUR = (string)row["ZL_RL_FRBNR_ZUR"],
					ZL_LIFNR = (string)row["ZL_LIFNR"],
					ZULPOSNR = (string)row["ZULPOSNR"],
					UEPOS = (string)row["UEPOS"],
					LOEKZ = (string)row["LOEKZ"],
					MENGE = (decimal?)row["MENGE"],
					MATNR = (string)row["MATNR"],
					MAKTX = (string)row["MAKTX"],
					PREIS = (decimal?)row["PREIS"],
					WEBMTART = (string)row["WEBMTART"],
					KBETR = (decimal?)row["KBETR"],
					NAME1 = (string)row["NAME1"],
					VZB_STATUS = (string)row["VZB_STATUS"],

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

			public static IEnumerable<GT_EX_ZUERL> Select(DataTable dt, ISapConnection sapConnection = null)
			{
				return dt.AsEnumerable().Select(r => Create(r, sapConnection));
			}

			public static List<GT_EX_ZUERL> ToList(DataTable dt, ISapConnection sapConnection = null)
			{
				return Select(dt, sapConnection).ToListOrEmptyList();
			}

			public static IEnumerable<GT_EX_ZUERL> Select(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				var tbl = dts.FirstOrDefault(t => t.TableName.ToLower() == typeof(GT_EX_ZUERL).Name.ToLower());
				if (tbl == null)
					return null;

				return Select(tbl, sapConnection);
			}

			public static List<GT_EX_ZUERL> ToList(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				return Select(dts, sapConnection).ToListOrEmptyList();
			}

			public static List<GT_EX_ZUERL> ToList(ISapDataService sapDataService)
			{
				return ToList(sapDataService.GetExportTables(), sapDataService.SapConnection);
			}

			public static List<GT_EX_ZUERL> GetExportListWithInitExecute(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<GT_EX_ZUERL>();
				 
				var dts = sapDataService.GetExportTablesWithInitExecute("Z_ZLD_EXPORT_VZOZUERL", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_EX_ZUERL> GetExportListWithExecute(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_EX_ZUERL>();
				 
				var dts = sapDataService.GetExportTablesWithExecute();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_EX_ZUERL> GetExportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_EX_ZUERL>();
				 
				var dts = sapDataService.GetExportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_EX_ZUERL> GetImportListWithInit(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<GT_EX_ZUERL>();
				 
				var dts = sapDataService.GetImportTablesWithInit("Z_ZLD_EXPORT_VZOZUERL", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_EX_ZUERL> GetImportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_EX_ZUERL>();
				 
				var dts = sapDataService.GetImportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}
		}
	}

	public static partial class DataTableExtensions
	{

		public static DataTable ToTable(this IEnumerable<Z_ZLD_EXPORT_VZOZUERL.GT_EX_ZUERL> list)
		{
			return SapDataServiceExtensions.ToTable(list);
		}

		public static void Apply(this IEnumerable<Z_ZLD_EXPORT_VZOZUERL.GT_EX_ZUERL> list, DataTable dtDst)
		{
			SapDataServiceExtensions.Apply(list, dtDst);
		}

	}
}
