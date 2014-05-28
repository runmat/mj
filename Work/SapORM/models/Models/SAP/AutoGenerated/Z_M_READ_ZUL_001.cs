using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Script.Serialization;
using GeneralTools.Contracts;
using SapORM.Contracts;

namespace SapORM.Models
{
	public partial class Z_M_READ_ZUL_001
	{
		public static void Init(ISapDataService sap)
		{
			sap.Init(typeof(Z_M_READ_ZUL_001).Name);
		}

		public static void Init(ISapDataService sap, string inputParameterKeys, params object[] inputParameterValues)
		{
			sap.Init(typeof(Z_M_READ_ZUL_001).Name, inputParameterKeys, inputParameterValues);
		}

		public partial class GT_WEB : IModelMappingApplied
		{
			[SapIgnore]
			[ScriptIgnore]
			public ISapConnection SAPConnection { get; set; }

			[SapIgnore]
			[ScriptIgnore]
			public IDynSapProxyFactory DynSapProxyFactory { get; set; }

			public string KUNPDI { get; set; }

			public string NAME1_PDI { get; set; }

			public string NAME2_PDI { get; set; }

			public string HERST_NUMMER { get; set; }

			public string MAKE_CODE { get; set; }

			public string ZZMODELL { get; set; }

			public string ZZBEZEI { get; set; }

			public string GEPL_LIEFTERMIN { get; set; }

			public string REIFENART { get; set; }

			public string ANTRIEBSART { get; set; }

			public string KRAFTSTOFF { get; set; }

			public string NAVI_CD { get; set; }

			public string FARBE_DE { get; set; }

			public string VERMIET_GRP { get; set; }

			public string FZG_ART { get; set; }

			public string AUFBAUART { get; set; }

			public string BEZAHLTKENNZ { get; set; }

			public string LIEFERANT { get; set; }

			public string LIEF_KURZNAME { get; set; }

			public string EINK_INDIKATOR { get; set; }

			public string VERWENDUNGSZWECK { get; set; }

			public string NAVIGATION { get; set; }

			public string REIFENGR { get; set; }

			public string SPERRVERMERK { get; set; }

			public string OWNER_CODE { get; set; }

			public string CHASSIS_NUM { get; set; }

			public string ZKENNZEICHEN { get; set; }

			public string MVA_NUMMER { get; set; }

			public DateTime? ZZDAT_EIN { get; set; }

			public DateTime? ZZDAT_BER { get; set; }

			public DateTime? DAT_FREIS_ZUL { get; set; }

			public DateTime? ZULDAT { get; set; }

			public DateTime? DAT_MEL_ZUL { get; set; }

			public string EQUNR { get; set; }

			public string ZZCARPORT { get; set; }

			public static GT_WEB Create(DataRow row, ISapConnection sapConnection = null, IDynSapProxyFactory dynSapProxyFactory = null)
			{
				var o = new GT_WEB
				{
					KUNPDI = (string)row["KUNPDI"],
					NAME1_PDI = (string)row["NAME1_PDI"],
					NAME2_PDI = (string)row["NAME2_PDI"],
					HERST_NUMMER = (string)row["HERST_NUMMER"],
					MAKE_CODE = (string)row["MAKE_CODE"],
					ZZMODELL = (string)row["ZZMODELL"],
					ZZBEZEI = (string)row["ZZBEZEI"],
					GEPL_LIEFTERMIN = (string)row["GEPL_LIEFTERMIN"],
					REIFENART = (string)row["REIFENART"],
					ANTRIEBSART = (string)row["ANTRIEBSART"],
					KRAFTSTOFF = (string)row["KRAFTSTOFF"],
					NAVI_CD = (string)row["NAVI_CD"],
					FARBE_DE = (string)row["FARBE_DE"],
					VERMIET_GRP = (string)row["VERMIET_GRP"],
					FZG_ART = (string)row["FZG_ART"],
					AUFBAUART = (string)row["AUFBAUART"],
					BEZAHLTKENNZ = (string)row["BEZAHLTKENNZ"],
					LIEFERANT = (string)row["LIEFERANT"],
					LIEF_KURZNAME = (string)row["LIEF_KURZNAME"],
					EINK_INDIKATOR = (string)row["EINK_INDIKATOR"],
					VERWENDUNGSZWECK = (string)row["VERWENDUNGSZWECK"],
					NAVIGATION = (string)row["NAVIGATION"],
					REIFENGR = (string)row["REIFENGR"],
					SPERRVERMERK = (string)row["SPERRVERMERK"],
					OWNER_CODE = (string)row["OWNER_CODE"],
					CHASSIS_NUM = (string)row["CHASSIS_NUM"],
					ZKENNZEICHEN = (string)row["ZKENNZEICHEN"],
					MVA_NUMMER = (string)row["MVA_NUMMER"],
					ZZDAT_EIN = (string.IsNullOrEmpty(row["ZZDAT_EIN"].ToString())) ? null : (DateTime?)row["ZZDAT_EIN"],
					ZZDAT_BER = (string.IsNullOrEmpty(row["ZZDAT_BER"].ToString())) ? null : (DateTime?)row["ZZDAT_BER"],
					DAT_FREIS_ZUL = (string.IsNullOrEmpty(row["DAT_FREIS_ZUL"].ToString())) ? null : (DateTime?)row["DAT_FREIS_ZUL"],
					ZULDAT = (string.IsNullOrEmpty(row["ZULDAT"].ToString())) ? null : (DateTime?)row["ZULDAT"],
					DAT_MEL_ZUL = (string.IsNullOrEmpty(row["DAT_MEL_ZUL"].ToString())) ? null : (DateTime?)row["DAT_MEL_ZUL"],
					EQUNR = (string)row["EQUNR"],
					ZZCARPORT = (string)row["ZZCARPORT"],

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

			public static IEnumerable<GT_WEB> Select(DataTable dt, ISapConnection sapConnection = null)
			{
				return dt.AsEnumerable().Select(r => Create(r, sapConnection));
			}

			public static List<GT_WEB> ToList(DataTable dt, ISapConnection sapConnection = null)
			{
				return Select(dt, sapConnection).ToList();
			}

			public static IEnumerable<GT_WEB> Select(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				var tbl = dts.FirstOrDefault(t => t.TableName.ToLower() == typeof(GT_WEB).Name.ToLower());
				if (tbl == null)
					return null;

				return Select(tbl, sapConnection);
			}

			public static List<GT_WEB> ToList(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				return Select(dts, sapConnection).ToList();
			}

			public static List<GT_WEB> ToList(ISapDataService sapDataService)
			{
				return ToList(sapDataService.GetExportTables(), sapDataService.SapConnection);
			}

			public static List<GT_WEB> GetExportListWithInitExecute(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<GT_WEB>();
				 
				var dts = sapDataService.GetExportTablesWithInitExecute("Z_M_READ_ZUL_001", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToList();
			}

			public static List<GT_WEB> GetExportListWithExecute(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_WEB>();
				 
				var dts = sapDataService.GetExportTablesWithExecute();
				 
				return Select(dts, sapDataService.SapConnection).ToList();
			}

			public static List<GT_WEB> GetExportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_WEB>();
				 
				var dts = sapDataService.GetExportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToList();
			}

			public static List<GT_WEB> GetImportListWithInit(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<GT_WEB>();
				 
				var dts = sapDataService.GetImportTablesWithInit("Z_M_READ_ZUL_001", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToList();
			}

			public static List<GT_WEB> GetImportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_WEB>();
				 
				var dts = sapDataService.GetImportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToList();
			}
		}
	}

	public static partial class DataTableExtensions
	{

		public static DataTable ToTable(this IEnumerable<Z_M_READ_ZUL_001.GT_WEB> list)
		{
			return SapDataServiceExtensions.ToTable(list);
		}

		public static void Apply(this IEnumerable<Z_M_READ_ZUL_001.GT_WEB> list, DataTable dtDst)
		{
			SapDataServiceExtensions.Apply(list, dtDst);
		}

	}
}
