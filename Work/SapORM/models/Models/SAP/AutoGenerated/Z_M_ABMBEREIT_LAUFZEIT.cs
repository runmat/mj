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
	public partial class Z_M_ABMBEREIT_LAUFZEIT
	{
		public static void Init(ISapDataService sap)
		{
			sap.Init(typeof(Z_M_ABMBEREIT_LAUFZEIT).Name);
		}

		public static void Init(ISapDataService sap, string inputParameterKeys, params object[] inputParameterValues)
		{
			sap.Init(typeof(Z_M_ABMBEREIT_LAUFZEIT).Name, inputParameterKeys, inputParameterValues);
		}


		public void SetImportParameter_KUNNR(ISapDataService sap, string value)
		{
			sap.SetImportParameter("KUNNR", value);
		}

		public partial class AUSGABE : IModelMappingApplied
		{
			[SapIgnore]
			[ScriptIgnore]
			public ISapConnection SAPConnection { get; set; }

			[SapIgnore]
			[ScriptIgnore]
			public IDynSapProxyFactory DynSapProxyFactory { get; set; }

			public string KUNNR { get; set; }

			public string QMNUM { get; set; }

			public string EQUNR { get; set; }

			public DateTime? ERDAT { get; set; }

			public string ZZKENN { get; set; }

			public string ZZBRIEF { get; set; }

			public string ZZFAHRG { get; set; }

			public string DADPDI { get; set; }

			public string KUNPDI { get; set; }

			public string DADPDI_NAME1 { get; set; }

			public string ZZCODE { get; set; }

			public string ZZBEZEI { get; set; }

			public string HERST_K { get; set; }

			public string ZZSIPP3 { get; set; }

			public string ZZAUSF { get; set; }

			public string ZZANTR { get; set; }

			public string ZZREIFEN { get; set; }

			public string ZZNAVI { get; set; }

			public DateTime? ZZDAT_EIN { get; set; }

			public string ZZKLEBE { get; set; }

			public string ZZAKTSPERRE { get; set; }

			public string ANZAHL_ZUL { get; set; }

			public string ANZAHL_GSP { get; set; }

			public string KUNNR_ZP { get; set; }

			public string NAME1_ZP { get; set; }

			public DateTime? REPLA_DATE { get; set; }

			public DateTime? VDATU { get; set; }

			public string ZZREF1 { get; set; }

			public string ZZREF2 { get; set; }

			public DateTime? ERDAT_LOW { get; set; }

			public DateTime? ERDAT_HIGH { get; set; }

			public string FLEET_VIN { get; set; }

			public string KUNNR_ZK { get; set; }

			public DateTime? ZZDATBEM { get; set; }

			public string ZZMODELL { get; set; }

			public string ZZFARBE { get; set; }

			public string ZZHERST_TEXT { get; set; }

			public string ZZKLARTEXT_TYP { get; set; }

			public string ZZHUBRAUM { get; set; }

			public string ZZNENNLEISTUNG { get; set; }

			public string ZZKRAFTSTOFF_TXT { get; set; }

			public DateTime? ZZDAT_BER { get; set; }

			public string ZZHANDELSNAME { get; set; }

			public string ZZLAUFZEIT { get; set; }

			public string ZZREFERENZ1 { get; set; }

			public string ZSTATUS { get; set; }

			public DateTime? PICKDAT { get; set; }

			public DateTime? ZZTMPDT { get; set; }

			public string BELNR { get; set; }

			public string NAME1_ZH { get; set; }

			public string NAME2_ZH { get; set; }

			public string STREET_ZH { get; set; }

			public string HOUSE_NUM1_ZH { get; set; }

			public string POST_CODE1_ZH { get; set; }

			public string CITY1_ZH { get; set; }

			public static AUSGABE Create(DataRow row, ISapConnection sapConnection = null, IDynSapProxyFactory dynSapProxyFactory = null)
			{
				var o = new AUSGABE
				{
					KUNNR = (string)row["KUNNR"],
					QMNUM = (string)row["QMNUM"],
					EQUNR = (string)row["EQUNR"],
					ERDAT = string.IsNullOrEmpty(row["ERDAT"].ToString()) ? null : (DateTime?)row["ERDAT"],
					ZZKENN = (string)row["ZZKENN"],
					ZZBRIEF = (string)row["ZZBRIEF"],
					ZZFAHRG = (string)row["ZZFAHRG"],
					DADPDI = (string)row["DADPDI"],
					KUNPDI = (string)row["KUNPDI"],
					DADPDI_NAME1 = (string)row["DADPDI_NAME1"],
					ZZCODE = (string)row["ZZCODE"],
					ZZBEZEI = (string)row["ZZBEZEI"],
					HERST_K = (string)row["HERST_K"],
					ZZSIPP3 = (string)row["ZZSIPP3"],
					ZZAUSF = (string)row["ZZAUSF"],
					ZZANTR = (string)row["ZZANTR"],
					ZZREIFEN = (string)row["ZZREIFEN"],
					ZZNAVI = (string)row["ZZNAVI"],
					ZZDAT_EIN = string.IsNullOrEmpty(row["ZZDAT_EIN"].ToString()) ? null : (DateTime?)row["ZZDAT_EIN"],
					ZZKLEBE = (string)row["ZZKLEBE"],
					ZZAKTSPERRE = (string)row["ZZAKTSPERRE"],
					ANZAHL_ZUL = (string)row["ANZAHL_ZUL"],
					ANZAHL_GSP = (string)row["ANZAHL_GSP"],
					KUNNR_ZP = (string)row["KUNNR_ZP"],
					NAME1_ZP = (string)row["NAME1_ZP"],
					REPLA_DATE = string.IsNullOrEmpty(row["REPLA_DATE"].ToString()) ? null : (DateTime?)row["REPLA_DATE"],
					VDATU = string.IsNullOrEmpty(row["VDATU"].ToString()) ? null : (DateTime?)row["VDATU"],
					ZZREF1 = (string)row["ZZREF1"],
					ZZREF2 = (string)row["ZZREF2"],
					ERDAT_LOW = string.IsNullOrEmpty(row["ERDAT_LOW"].ToString()) ? null : (DateTime?)row["ERDAT_LOW"],
					ERDAT_HIGH = string.IsNullOrEmpty(row["ERDAT_HIGH"].ToString()) ? null : (DateTime?)row["ERDAT_HIGH"],
					FLEET_VIN = (string)row["FLEET_VIN"],
					KUNNR_ZK = (string)row["KUNNR_ZK"],
					ZZDATBEM = string.IsNullOrEmpty(row["ZZDATBEM"].ToString()) ? null : (DateTime?)row["ZZDATBEM"],
					ZZMODELL = (string)row["ZZMODELL"],
					ZZFARBE = (string)row["ZZFARBE"],
					ZZHERST_TEXT = (string)row["ZZHERST_TEXT"],
					ZZKLARTEXT_TYP = (string)row["ZZKLARTEXT_TYP"],
					ZZHUBRAUM = (string)row["ZZHUBRAUM"],
					ZZNENNLEISTUNG = (string)row["ZZNENNLEISTUNG"],
					ZZKRAFTSTOFF_TXT = (string)row["ZZKRAFTSTOFF_TXT"],
					ZZDAT_BER = string.IsNullOrEmpty(row["ZZDAT_BER"].ToString()) ? null : (DateTime?)row["ZZDAT_BER"],
					ZZHANDELSNAME = (string)row["ZZHANDELSNAME"],
					ZZLAUFZEIT = (string)row["ZZLAUFZEIT"],
					ZZREFERENZ1 = (string)row["ZZREFERENZ1"],
					ZSTATUS = (string)row["ZSTATUS"],
					PICKDAT = string.IsNullOrEmpty(row["PICKDAT"].ToString()) ? null : (DateTime?)row["PICKDAT"],
					ZZTMPDT = string.IsNullOrEmpty(row["ZZTMPDT"].ToString()) ? null : (DateTime?)row["ZZTMPDT"],
					BELNR = (string)row["BELNR"],
					NAME1_ZH = (string)row["NAME1_ZH"],
					NAME2_ZH = (string)row["NAME2_ZH"],
					STREET_ZH = (string)row["STREET_ZH"],
					HOUSE_NUM1_ZH = (string)row["HOUSE_NUM1_ZH"],
					POST_CODE1_ZH = (string)row["POST_CODE1_ZH"],
					CITY1_ZH = (string)row["CITY1_ZH"],

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

			public static IEnumerable<AUSGABE> Select(DataTable dt, ISapConnection sapConnection = null)
			{
				return dt.AsEnumerable().Select(r => Create(r, sapConnection));
			}

			public static List<AUSGABE> ToList(DataTable dt, ISapConnection sapConnection = null)
			{
				return Select(dt, sapConnection).ToListOrEmptyList();
			}

			public static IEnumerable<AUSGABE> Select(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				var tbl = dts.FirstOrDefault(t => t.TableName.ToLower() == typeof(AUSGABE).Name.ToLower());
				if (tbl == null)
					return null;

				return Select(tbl, sapConnection);
			}

			public static List<AUSGABE> ToList(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				return Select(dts, sapConnection).ToListOrEmptyList();
			}

			public static List<AUSGABE> ToList(ISapDataService sapDataService)
			{
				return ToList(sapDataService.GetExportTables(), sapDataService.SapConnection);
			}

			public static List<AUSGABE> GetExportListWithInitExecute(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<AUSGABE>();
				 
				var dts = sapDataService.GetExportTablesWithInitExecute("Z_M_ABMBEREIT_LAUFZEIT", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<AUSGABE> GetExportListWithExecute(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<AUSGABE>();
				 
				var dts = sapDataService.GetExportTablesWithExecute();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<AUSGABE> GetExportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<AUSGABE>();
				 
				var dts = sapDataService.GetExportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<AUSGABE> GetImportListWithInit(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<AUSGABE>();
				 
				var dts = sapDataService.GetImportTablesWithInit("Z_M_ABMBEREIT_LAUFZEIT", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<AUSGABE> GetImportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<AUSGABE>();
				 
				var dts = sapDataService.GetImportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}
		}
	}

	public static partial class DataTableExtensions
	{

		public static DataTable ToTable(this IEnumerable<Z_M_ABMBEREIT_LAUFZEIT.AUSGABE> list)
		{
			return SapDataServiceExtensions.ToTable(list);
		}

	}
}
