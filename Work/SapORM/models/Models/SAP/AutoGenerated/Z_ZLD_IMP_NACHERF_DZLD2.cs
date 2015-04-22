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
	public partial class Z_ZLD_IMP_NACHERF_DZLD2
	{
		public static void Init(ISapDataService sap)
		{
			sap.Init(typeof(Z_ZLD_IMP_NACHERF_DZLD2).Name);
		}

		public static void Init(ISapDataService sap, string inputParameterKeys, params object[] inputParameterValues)
		{
			sap.Init(typeof(Z_ZLD_IMP_NACHERF_DZLD2).Name, inputParameterKeys, inputParameterValues);
		}

		public partial class GT_IMP_BAK : IModelMappingApplied
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

			public DateTime? VE_ERDAT { get; set; }

			public string VE_ERNAM { get; set; }

			public string VE_ERZEIT { get; set; }

			public DateTime? ERDAT { get; set; }

			public string ERNAM { get; set; }

			public string STATUS { get; set; }

			public string BLTYP { get; set; }

			public string VZB_STATUS { get; set; }

			public string VZD_VKBUR { get; set; }

			public DateTime? VZERDAT { get; set; }

			public string BARCODE { get; set; }

			public string KUNNR { get; set; }

			public string ZZREFNR1 { get; set; }

			public string ZZREFNR2 { get; set; }

			public string KREISKZ { get; set; }

			public string KREISBEZ { get; set; }

			public string WUNSCHKENN_JN { get; set; }

			public string RESERVKENN_JN { get; set; }

			public string RESERVKENN { get; set; }

			public DateTime? ZZZLDAT { get; set; }

			public string ZZKENN { get; set; }

			public string KENNZFORM { get; set; }

			public string KENNZANZ { get; set; }

			public string EINKENN_JN { get; set; }

			public string BEMERKUNG { get; set; }

			public string EC_JN { get; set; }

			public string BAR_JN { get; set; }

			public string RE_JN { get; set; }

			public string ZL_LIFNR { get; set; }

			public string KUNDEBAR_JN { get; set; }

			public string LOEKZ { get; set; }

			public string KSTATUS { get; set; }

			public string ERROR_TEXT { get; set; }

			public string PRALI_PRINT { get; set; }

			public string FLIEGER { get; set; }

			public string BEB_STATUS { get; set; }

			public string INFO_TEXT { get; set; }

			public string ZL_RL_FRBNR_HIN { get; set; }

			public string ZL_RL_FRBNR_ZUR { get; set; }

			public string NACHBEARBEITEN { get; set; }

			public string LTEXT_NR { get; set; }

			public string MOBUSER { get; set; }

			public string ZZEVB { get; set; }

			public string ZAHLART { get; set; }

			public static GT_IMP_BAK Create(DataRow row, ISapConnection sapConnection = null, IDynSapProxyFactory dynSapProxyFactory = null)
			{
				var o = new GT_IMP_BAK
				{
					ZULBELN = (string)row["ZULBELN"],
					VBELN = (string)row["VBELN"],
					VKORG = (string)row["VKORG"],
					VKBUR = (string)row["VKBUR"],
					VE_ERDAT = (string.IsNullOrEmpty(row["VE_ERDAT"].ToString())) ? null : (DateTime?)row["VE_ERDAT"],
					VE_ERNAM = (string)row["VE_ERNAM"],
					VE_ERZEIT = (string)row["VE_ERZEIT"],
					ERDAT = (string.IsNullOrEmpty(row["ERDAT"].ToString())) ? null : (DateTime?)row["ERDAT"],
					ERNAM = (string)row["ERNAM"],
					STATUS = (string)row["STATUS"],
					BLTYP = (string)row["BLTYP"],
					VZB_STATUS = (string)row["VZB_STATUS"],
					VZD_VKBUR = (string)row["VZD_VKBUR"],
					VZERDAT = (string.IsNullOrEmpty(row["VZERDAT"].ToString())) ? null : (DateTime?)row["VZERDAT"],
					BARCODE = (string)row["BARCODE"],
					KUNNR = (string)row["KUNNR"],
					ZZREFNR1 = (string)row["ZZREFNR1"],
					ZZREFNR2 = (string)row["ZZREFNR2"],
					KREISKZ = (string)row["KREISKZ"],
					KREISBEZ = (string)row["KREISBEZ"],
					WUNSCHKENN_JN = (string)row["WUNSCHKENN_JN"],
					RESERVKENN_JN = (string)row["RESERVKENN_JN"],
					RESERVKENN = (string)row["RESERVKENN"],
					ZZZLDAT = (string.IsNullOrEmpty(row["ZZZLDAT"].ToString())) ? null : (DateTime?)row["ZZZLDAT"],
					ZZKENN = (string)row["ZZKENN"],
					KENNZFORM = (string)row["KENNZFORM"],
					KENNZANZ = (string)row["KENNZANZ"],
					EINKENN_JN = (string)row["EINKENN_JN"],
					BEMERKUNG = (string)row["BEMERKUNG"],
					EC_JN = (string)row["EC_JN"],
					BAR_JN = (string)row["BAR_JN"],
					RE_JN = (string)row["RE_JN"],
					ZL_LIFNR = (string)row["ZL_LIFNR"],
					KUNDEBAR_JN = (string)row["KUNDEBAR_JN"],
					LOEKZ = (string)row["LOEKZ"],
					KSTATUS = (string)row["KSTATUS"],
					ERROR_TEXT = (string)row["ERROR_TEXT"],
					PRALI_PRINT = (string)row["PRALI_PRINT"],
					FLIEGER = (string)row["FLIEGER"],
					BEB_STATUS = (string)row["BEB_STATUS"],
					INFO_TEXT = (string)row["INFO_TEXT"],
					ZL_RL_FRBNR_HIN = (string)row["ZL_RL_FRBNR_HIN"],
					ZL_RL_FRBNR_ZUR = (string)row["ZL_RL_FRBNR_ZUR"],
					NACHBEARBEITEN = (string)row["NACHBEARBEITEN"],
					LTEXT_NR = (string)row["LTEXT_NR"],
					MOBUSER = (string)row["MOBUSER"],
					ZZEVB = (string)row["ZZEVB"],
					ZAHLART = (string)row["ZAHLART"],

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

			public static IEnumerable<GT_IMP_BAK> Select(DataTable dt, ISapConnection sapConnection = null)
			{
				return dt.AsEnumerable().Select(r => Create(r, sapConnection));
			}

			public static List<GT_IMP_BAK> ToList(DataTable dt, ISapConnection sapConnection = null)
			{
				return Select(dt, sapConnection).ToListOrEmptyList();
			}

			public static IEnumerable<GT_IMP_BAK> Select(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				var tbl = dts.FirstOrDefault(t => t.TableName.ToLower() == typeof(GT_IMP_BAK).Name.ToLower());
				if (tbl == null)
					return null;

				return Select(tbl, sapConnection);
			}

			public static List<GT_IMP_BAK> ToList(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				return Select(dts, sapConnection).ToListOrEmptyList();
			}

			public static List<GT_IMP_BAK> ToList(ISapDataService sapDataService)
			{
				return ToList(sapDataService.GetExportTables(), sapDataService.SapConnection);
			}

			public static List<GT_IMP_BAK> GetExportListWithInitExecute(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<GT_IMP_BAK>();
				 
				var dts = sapDataService.GetExportTablesWithInitExecute("Z_ZLD_IMP_NACHERF_DZLD2", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_IMP_BAK> GetExportListWithExecute(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_IMP_BAK>();
				 
				var dts = sapDataService.GetExportTablesWithExecute();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_IMP_BAK> GetExportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_IMP_BAK>();
				 
				var dts = sapDataService.GetExportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_IMP_BAK> GetImportListWithInit(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<GT_IMP_BAK>();
				 
				var dts = sapDataService.GetImportTablesWithInit("Z_ZLD_IMP_NACHERF_DZLD2", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_IMP_BAK> GetImportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_IMP_BAK>();
				 
				var dts = sapDataService.GetImportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}
		}

		public partial class GT_IMP_POS : IModelMappingApplied
		{
			[SapIgnore]
			[ScriptIgnore]
			public ISapConnection SAPConnection { get; set; }

			[SapIgnore]
			[ScriptIgnore]
			public IDynSapProxyFactory DynSapProxyFactory { get; set; }

			public string ZULBELN { get; set; }

			public string ZULPOSNR { get; set; }

			public string UEPOS { get; set; }

			public string LOEKZ { get; set; }

			public decimal? MENGE { get; set; }

			public string MATNR { get; set; }

			public string MAKTX { get; set; }

			public decimal? PREIS { get; set; }

			public decimal? GEB_AMT { get; set; }

			public decimal? GEB_AMT_ADD { get; set; }

			public string WEBMTART { get; set; }

			public string SD_REL { get; set; }

			public string NULLPREIS_OK { get; set; }

			public string GBPAK { get; set; }

			public decimal? UPREIS { get; set; }

			public decimal? DIFF { get; set; }

			public string KONDTAB { get; set; }

			public string KSCHL { get; set; }

			public DateTime? CALCDAT { get; set; }

			public string WEB_STATUS { get; set; }

			public static GT_IMP_POS Create(DataRow row, ISapConnection sapConnection = null, IDynSapProxyFactory dynSapProxyFactory = null)
			{
				var o = new GT_IMP_POS
				{
					ZULBELN = (string)row["ZULBELN"],
					ZULPOSNR = (string)row["ZULPOSNR"],
					UEPOS = (string)row["UEPOS"],
					LOEKZ = (string)row["LOEKZ"],
					MENGE = (decimal?)row["MENGE"],
					MATNR = (string)row["MATNR"],
					MAKTX = (string)row["MAKTX"],
					PREIS = (decimal?)row["PREIS"],
					GEB_AMT = (decimal?)row["GEB_AMT"],
					GEB_AMT_ADD = (decimal?)row["GEB_AMT_ADD"],
					WEBMTART = (string)row["WEBMTART"],
					SD_REL = (string)row["SD_REL"],
					NULLPREIS_OK = (string)row["NULLPREIS_OK"],
					GBPAK = (string)row["GBPAK"],
					UPREIS = (decimal?)row["UPREIS"],
					DIFF = (decimal?)row["DIFF"],
					KONDTAB = (string)row["KONDTAB"],
					KSCHL = (string)row["KSCHL"],
					CALCDAT = (string.IsNullOrEmpty(row["CALCDAT"].ToString())) ? null : (DateTime?)row["CALCDAT"],
					WEB_STATUS = (string)row["WEB_STATUS"],

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

			public static IEnumerable<GT_IMP_POS> Select(DataTable dt, ISapConnection sapConnection = null)
			{
				return dt.AsEnumerable().Select(r => Create(r, sapConnection));
			}

			public static List<GT_IMP_POS> ToList(DataTable dt, ISapConnection sapConnection = null)
			{
				return Select(dt, sapConnection).ToListOrEmptyList();
			}

			public static IEnumerable<GT_IMP_POS> Select(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				var tbl = dts.FirstOrDefault(t => t.TableName.ToLower() == typeof(GT_IMP_POS).Name.ToLower());
				if (tbl == null)
					return null;

				return Select(tbl, sapConnection);
			}

			public static List<GT_IMP_POS> ToList(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				return Select(dts, sapConnection).ToListOrEmptyList();
			}

			public static List<GT_IMP_POS> ToList(ISapDataService sapDataService)
			{
				return ToList(sapDataService.GetExportTables(), sapDataService.SapConnection);
			}

			public static List<GT_IMP_POS> GetExportListWithInitExecute(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<GT_IMP_POS>();
				 
				var dts = sapDataService.GetExportTablesWithInitExecute("Z_ZLD_IMP_NACHERF_DZLD2", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_IMP_POS> GetExportListWithExecute(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_IMP_POS>();
				 
				var dts = sapDataService.GetExportTablesWithExecute();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_IMP_POS> GetExportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_IMP_POS>();
				 
				var dts = sapDataService.GetExportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_IMP_POS> GetImportListWithInit(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<GT_IMP_POS>();
				 
				var dts = sapDataService.GetImportTablesWithInit("Z_ZLD_IMP_NACHERF_DZLD2", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_IMP_POS> GetImportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_IMP_POS>();
				 
				var dts = sapDataService.GetImportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}
		}

		public partial class GT_EX_ERRORS : IModelMappingApplied
		{
			[SapIgnore]
			[ScriptIgnore]
			public ISapConnection SAPConnection { get; set; }

			[SapIgnore]
			[ScriptIgnore]
			public IDynSapProxyFactory DynSapProxyFactory { get; set; }

			public string ZULBELN { get; set; }

			public string ZULPOSNR { get; set; }

			public string ERROR_TEXT { get; set; }

			public static GT_EX_ERRORS Create(DataRow row, ISapConnection sapConnection = null, IDynSapProxyFactory dynSapProxyFactory = null)
			{
				var o = new GT_EX_ERRORS
				{
					ZULBELN = (string)row["ZULBELN"],
					ZULPOSNR = (string)row["ZULPOSNR"],
					ERROR_TEXT = (string)row["ERROR_TEXT"],

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

			public static IEnumerable<GT_EX_ERRORS> Select(DataTable dt, ISapConnection sapConnection = null)
			{
				return dt.AsEnumerable().Select(r => Create(r, sapConnection));
			}

			public static List<GT_EX_ERRORS> ToList(DataTable dt, ISapConnection sapConnection = null)
			{
				return Select(dt, sapConnection).ToListOrEmptyList();
			}

			public static IEnumerable<GT_EX_ERRORS> Select(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				var tbl = dts.FirstOrDefault(t => t.TableName.ToLower() == typeof(GT_EX_ERRORS).Name.ToLower());
				if (tbl == null)
					return null;

				return Select(tbl, sapConnection);
			}

			public static List<GT_EX_ERRORS> ToList(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				return Select(dts, sapConnection).ToListOrEmptyList();
			}

			public static List<GT_EX_ERRORS> ToList(ISapDataService sapDataService)
			{
				return ToList(sapDataService.GetExportTables(), sapDataService.SapConnection);
			}

			public static List<GT_EX_ERRORS> GetExportListWithInitExecute(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<GT_EX_ERRORS>();
				 
				var dts = sapDataService.GetExportTablesWithInitExecute("Z_ZLD_IMP_NACHERF_DZLD2", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_EX_ERRORS> GetExportListWithExecute(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_EX_ERRORS>();
				 
				var dts = sapDataService.GetExportTablesWithExecute();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_EX_ERRORS> GetExportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_EX_ERRORS>();
				 
				var dts = sapDataService.GetExportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_EX_ERRORS> GetImportListWithInit(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<GT_EX_ERRORS>();
				 
				var dts = sapDataService.GetImportTablesWithInit("Z_ZLD_IMP_NACHERF_DZLD2", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_EX_ERRORS> GetImportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_EX_ERRORS>();
				 
				var dts = sapDataService.GetImportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}
		}
	}

	public static partial class DataTableExtensions
	{

		public static DataTable ToTable(this IEnumerable<Z_ZLD_IMP_NACHERF_DZLD2.GT_IMP_BAK> list)
		{
			return SapDataServiceExtensions.ToTable(list);
		}

		public static void Apply(this IEnumerable<Z_ZLD_IMP_NACHERF_DZLD2.GT_IMP_BAK> list, DataTable dtDst)
		{
			SapDataServiceExtensions.Apply(list, dtDst);
		}


		public static DataTable ToTable(this IEnumerable<Z_ZLD_IMP_NACHERF_DZLD2.GT_IMP_POS> list)
		{
			return SapDataServiceExtensions.ToTable(list);
		}

		public static void Apply(this IEnumerable<Z_ZLD_IMP_NACHERF_DZLD2.GT_IMP_POS> list, DataTable dtDst)
		{
			SapDataServiceExtensions.Apply(list, dtDst);
		}


		public static DataTable ToTable(this IEnumerable<Z_ZLD_IMP_NACHERF_DZLD2.GT_EX_ERRORS> list)
		{
			return SapDataServiceExtensions.ToTable(list);
		}

		public static void Apply(this IEnumerable<Z_ZLD_IMP_NACHERF_DZLD2.GT_EX_ERRORS> list, DataTable dtDst)
		{
			SapDataServiceExtensions.Apply(list, dtDst);
		}

	}
}
