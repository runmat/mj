using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Script.Serialization;
using GeneralTools.Contracts;
using SapORM.Contracts;

namespace SapORM.Models
{
	public partial class Z_DAD_DATEN_EINAUS_REPORT_002
	{
		public static void Init(ISapDataService sap)
		{
			sap.Init(typeof(Z_DAD_DATEN_EINAUS_REPORT_002).Name);
		}

		public static void Init(ISapDataService sap, string inputParameterKeys, params object[] inputParameterValues)
		{
			sap.Init(typeof(Z_DAD_DATEN_EINAUS_REPORT_002).Name, inputParameterKeys, inputParameterValues);
		}

		public partial class EINNEU : IModelMappingApplied
		{
			[SapIgnore]
			[ScriptIgnore]
			public ISapConnection SAPConnection { get; set; }

			[SapIgnore]
			[ScriptIgnore]
			public IDynSapProxyFactory DynSapProxyFactory { get; set; }

			public string MANDT { get; set; }

			public string KUNNR { get; set; }

			public string ZZLABEL { get; set; }

			public string LIZNR { get; set; }

			public string ZZKENN { get; set; }

			public string CHASSIS_NUM { get; set; }

			public string TIDNR { get; set; }

			public string ZZMODID { get; set; }

			public DateTime? ERDAT { get; set; }

			public string ERZET { get; set; }

			public string ERNAM { get; set; }

			public string ERST { get; set; }

			public string FEHLERKZ { get; set; }

			public string ABCKZ { get; set; }

			public string EQUNR { get; set; }

			public DateTime? ZZTMPDT { get; set; }

			public string ADRNR { get; set; }

			public string NAME1 { get; set; }

			public string NAME2 { get; set; }

			public string CITY1 { get; set; }

			public string POST_CODE1 { get; set; }

			public string STREET { get; set; }

			public string HOUSE_NUM1 { get; set; }

			public string ZZVGRUND { get; set; }

			public string TEXT50 { get; set; }

			public string ZZREFERENZ1 { get; set; }

			public DateTime? ZZMADAT { get; set; }

			public string ZZMAHNS { get; set; }

			public string ZZCOCKZ { get; set; }

			public string ZS_SORTL { get; set; }

			public string ZS_VTEH { get; set; }

			public string ZS_NAME1 { get; set; }

			public string ANFORDERER { get; set; }

			public string FREIGEBER { get; set; }

			public string ZZHANDELSNAME { get; set; }

			public string ZZBEZEI { get; set; }

			public string ZZCARPORT { get; set; }

			public string KUNPDI { get; set; }

			public string NAME1_PDI { get; set; }

			public string NAME2_PDI { get; set; }

			public string ORT01_PDI { get; set; }

			public string PSTLZ_PDI { get; set; }

			public string STRAS1_PDI { get; set; }

			public DateTime? INBDT { get; set; }

			public string ZZREFERENZ2 { get; set; }

			public string ZZHERST_TEXT { get; set; }

			public string ZZVVS_SCHLUESSEL { get; set; }

			public string ZZTYP_SCHL { get; set; }

			public string ZZHUBRAUM { get; set; }

			public string ZZKRAFTSTOFF_TXT { get; set; }

			public string ZZNENNLEISTUNG { get; set; }

			public string VERSANDART { get; set; }

			public static EINNEU Create(DataRow row, ISapConnection sapConnection = null, IDynSapProxyFactory dynSapProxyFactory = null)
			{
				var o = new EINNEU
				{
					MANDT = (string)row["MANDT"],
					KUNNR = (string)row["KUNNR"],
					ZZLABEL = (string)row["ZZLABEL"],
					LIZNR = (string)row["LIZNR"],
					ZZKENN = (string)row["ZZKENN"],
					CHASSIS_NUM = (string)row["CHASSIS_NUM"],
					TIDNR = (string)row["TIDNR"],
					ZZMODID = (string)row["ZZMODID"],
					ERDAT = (string.IsNullOrEmpty(row["ERDAT"].ToString())) ? null : (DateTime?)row["ERDAT"],
					ERZET = (string)row["ERZET"],
					ERNAM = (string)row["ERNAM"],
					ERST = (string)row["ERST"],
					FEHLERKZ = (string)row["FEHLERKZ"],
					ABCKZ = (string)row["ABCKZ"],
					EQUNR = (string)row["EQUNR"],
					ZZTMPDT = (string.IsNullOrEmpty(row["ZZTMPDT"].ToString())) ? null : (DateTime?)row["ZZTMPDT"],
					ADRNR = (string)row["ADRNR"],
					NAME1 = (string)row["NAME1"],
					NAME2 = (string)row["NAME2"],
					CITY1 = (string)row["CITY1"],
					POST_CODE1 = (string)row["POST_CODE1"],
					STREET = (string)row["STREET"],
					HOUSE_NUM1 = (string)row["HOUSE_NUM1"],
					ZZVGRUND = (string)row["ZZVGRUND"],
					TEXT50 = (string)row["TEXT50"],
					ZZREFERENZ1 = (string)row["ZZREFERENZ1"],
					ZZMADAT = (string.IsNullOrEmpty(row["ZZMADAT"].ToString())) ? null : (DateTime?)row["ZZMADAT"],
					ZZMAHNS = (string)row["ZZMAHNS"],
					ZZCOCKZ = (string)row["ZZCOCKZ"],
					ZS_SORTL = (string)row["ZS_SORTL"],
					ZS_VTEH = (string)row["ZS_VTEH"],
					ZS_NAME1 = (string)row["ZS_NAME1"],
					ANFORDERER = (string)row["ANFORDERER"],
					FREIGEBER = (string)row["FREIGEBER"],
					ZZHANDELSNAME = (string)row["ZZHANDELSNAME"],
					ZZBEZEI = (string)row["ZZBEZEI"],
					ZZCARPORT = (string)row["ZZCARPORT"],
					KUNPDI = (string)row["KUNPDI"],
					NAME1_PDI = (string)row["NAME1_PDI"],
					NAME2_PDI = (string)row["NAME2_PDI"],
					ORT01_PDI = (string)row["ORT01_PDI"],
					PSTLZ_PDI = (string)row["PSTLZ_PDI"],
					STRAS1_PDI = (string)row["STRAS1_PDI"],
					INBDT = (string.IsNullOrEmpty(row["INBDT"].ToString())) ? null : (DateTime?)row["INBDT"],
					ZZREFERENZ2 = (string)row["ZZREFERENZ2"],
					ZZHERST_TEXT = (string)row["ZZHERST_TEXT"],
					ZZVVS_SCHLUESSEL = (string)row["ZZVVS_SCHLUESSEL"],
					ZZTYP_SCHL = (string)row["ZZTYP_SCHL"],
					ZZHUBRAUM = (string)row["ZZHUBRAUM"],
					ZZKRAFTSTOFF_TXT = (string)row["ZZKRAFTSTOFF_TXT"],
					ZZNENNLEISTUNG = (string)row["ZZNENNLEISTUNG"],
					VERSANDART = (string)row["VERSANDART"],

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

			public static IEnumerable<EINNEU> Select(DataTable dt, ISapConnection sapConnection = null)
			{
				return dt.AsEnumerable().Select(r => Create(r, sapConnection));
			}

			public static List<EINNEU> ToList(DataTable dt, ISapConnection sapConnection = null)
			{
				return Select(dt, sapConnection).ToList();
			}

			public static IEnumerable<EINNEU> Select(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				var tbl = dts.FirstOrDefault(t => t.TableName.ToLower() == typeof(EINNEU).Name.ToLower());
				if (tbl == null)
					return null;

				return Select(tbl, sapConnection);
			}

			public static List<EINNEU> ToList(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				return Select(dts, sapConnection).ToList();
			}

			public static List<EINNEU> ToList(ISapDataService sapDataService)
			{
				return ToList(sapDataService.GetExportTables(), sapDataService.SapConnection);
			}

			public static List<EINNEU> GetExportListWithInitExecute(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<EINNEU>();
				 
				var dts = sapDataService.GetExportTablesWithInitExecute("Z_DAD_DATEN_EINAUS_REPORT_002", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToList();
			}

			public static List<EINNEU> GetExportListWithExecute(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<EINNEU>();
				 
				var dts = sapDataService.GetExportTablesWithExecute();
				 
				return Select(dts, sapDataService.SapConnection).ToList();
			}

			public static List<EINNEU> GetExportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<EINNEU>();
				 
				var dts = sapDataService.GetExportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToList();
			}

			public static List<EINNEU> GetImportListWithInit(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<EINNEU>();
				 
				var dts = sapDataService.GetImportTablesWithInit("Z_DAD_DATEN_EINAUS_REPORT_002", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToList();
			}

			public static List<EINNEU> GetImportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<EINNEU>();
				 
				var dts = sapDataService.GetImportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToList();
			}
		}
	}

	public static partial class DataTableExtensions
	{

		public static DataTable ToTable(this IEnumerable<Z_DAD_DATEN_EINAUS_REPORT_002.EINNEU> list)
		{
			return SapDataServiceExtensions.ToTable(list);
		}

		public static void Apply(this IEnumerable<Z_DAD_DATEN_EINAUS_REPORT_002.EINNEU> list, DataTable dtDst)
		{
			SapDataServiceExtensions.Apply(list, dtDst);
		}

	}
}
