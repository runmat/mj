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
	public partial class Z_DPM_EXP_VERS_AUSWERTUNG_01
	{
		public static void Init(ISapDataService sap)
		{
			sap.Init(typeof(Z_DPM_EXP_VERS_AUSWERTUNG_01).Name);
		}

		public static void Init(ISapDataService sap, string inputParameterKeys, params object[] inputParameterValues)
		{
			sap.Init(typeof(Z_DPM_EXP_VERS_AUSWERTUNG_01).Name, inputParameterKeys, inputParameterValues);
		}


		public static void SetImportParameter_I_DAT_ANGEL_BIS(ISapDataService sap, DateTime? value)
		{
			sap.SetImportParameter("I_DAT_ANGEL_BIS", value);
		}

		public static void SetImportParameter_I_DAT_ANGEL_VON(ISapDataService sap, DateTime? value)
		{
			sap.SetImportParameter("I_DAT_ANGEL_VON", value);
		}

		public static void SetImportParameter_I_DAT_VERSAUFTR_BIS(ISapDataService sap, DateTime? value)
		{
			sap.SetImportParameter("I_DAT_VERSAUFTR_BIS", value);
		}

		public static void SetImportParameter_I_DAT_VERSAUFTR_VON(ISapDataService sap, DateTime? value)
		{
			sap.SetImportParameter("I_DAT_VERSAUFTR_VON", value);
		}

		public static void SetImportParameter_I_KUNNR_AG(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_KUNNR_AG", value);
		}

		public static void SetImportParameter_I_KUNNR_BEIM_AG(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_KUNNR_BEIM_AG", value);
		}

		public static void SetImportParameter_I_VERSANDART(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_VERSANDART", value);
		}

		public static void SetImportParameter_I_VERT_ART(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_VERT_ART", value);
		}

		public partial class GT_OUT : IModelMappingApplied
		{
			[SapIgnore]
			[ScriptIgnore]
			public ISapConnection SAPConnection { get; set; }

			[SapIgnore]
			[ScriptIgnore]
			public IDynSapProxyFactory DynSapProxyFactory { get; set; }

			public string ANF_ART { get; set; }

			public string VERS_GRUND { get; set; }

			public string KONTONR { get; set; }

			public string CIN { get; set; }

			public string PAID { get; set; }

			public string SICHERHEITSIDCMS { get; set; }

			public DateTime? DAT_ANGEL { get; set; }

			public string ZBRIEF { get; set; }

			public string VERS_ART { get; set; }

			public string EMPF_ART { get; set; }

			public string NAME_EMPF { get; set; }

			public string VORNAME_EMPF { get; set; }

			public string STRASSE_EMPF { get; set; }

			public string HNR_EMPF { get; set; }

			public string PLZ_EMPF { get; set; }

			public string ORT_EMPF { get; set; }

			public string LAND_EMPF { get; set; }

			public string SYSTEMKENNZ { get; set; }

			public string AUFTRAGGEBERID { get; set; }

			public string NAME_ANF { get; set; }

			public string VORNAME_ANF { get; set; }

			public string STRASSE_ANF { get; set; }

			public string HNR_ANF { get; set; }

			public string PLZ_ANF { get; set; }

			public string ORT_ANF { get; set; }

			public string LAND_ANF { get; set; }

			public string CLIENTLD { get; set; }

			public DateTime? DAT_GEAEND { get; set; }

			public DateTime? DAT_VERSAUFTR { get; set; }

			public DateTime? DAT_UEBERM_1 { get; set; }

			public string VERS_STORNO { get; set; }

			public string ANREDE_EMPF { get; set; }

			public string VERSANDGRUND { get; set; }

			public string ZZPLFOR { get; set; }

			public string ANREDE_ANF { get; set; }

			public string TEL1_ANF { get; set; }

			public string TEL2_ANF { get; set; }

			public string MOBIL_ANF { get; set; }

			public string FAX1_ANF { get; set; }

			public string FAX2_ANF { get; set; }

			public string EMAIL_ANF { get; set; }

			public string INFO_ANF { get; set; }

			public string ANREDE_ANSP { get; set; }

			public string NAME_ANSP { get; set; }

			public string VORNAME_ANSP { get; set; }

			public string USER_AUTOR { get; set; }

			public string ZVERT_ART { get; set; }

			public DateTime? ENDG_VERS { get; set; }

			public DateTime? ZZTMPDT { get; set; }

			public string ANFORDERUNGSWEG { get; set; }

			public string KUNNR_BEIM_AG { get; set; }

			public string NAME { get; set; }

			public string CITY1 { get; set; }

			public string UZEIT_ANGEL { get; set; }

			public string STATUS_SI { get; set; }

			public string MAHNS_SI { get; set; }

			public static GT_OUT Create(DataRow row, ISapConnection sapConnection = null, IDynSapProxyFactory dynSapProxyFactory = null)
			{
				var o = new GT_OUT
				{
					ANF_ART = (string)row["ANF_ART"],
					VERS_GRUND = (string)row["VERS_GRUND"],
					KONTONR = (string)row["KONTONR"],
					CIN = (string)row["CIN"],
					PAID = (string)row["PAID"],
					SICHERHEITSIDCMS = (string)row["SICHERHEITSIDCMS"],
					DAT_ANGEL = string.IsNullOrEmpty(row["DAT_ANGEL"].ToString()) ? null : (DateTime?)row["DAT_ANGEL"],
					ZBRIEF = (string)row["ZBRIEF"],
					VERS_ART = (string)row["VERS_ART"],
					EMPF_ART = (string)row["EMPF_ART"],
					NAME_EMPF = (string)row["NAME_EMPF"],
					VORNAME_EMPF = (string)row["VORNAME_EMPF"],
					STRASSE_EMPF = (string)row["STRASSE_EMPF"],
					HNR_EMPF = (string)row["HNR_EMPF"],
					PLZ_EMPF = (string)row["PLZ_EMPF"],
					ORT_EMPF = (string)row["ORT_EMPF"],
					LAND_EMPF = (string)row["LAND_EMPF"],
					SYSTEMKENNZ = (string)row["SYSTEMKENNZ"],
					AUFTRAGGEBERID = (string)row["AUFTRAGGEBERID"],
					NAME_ANF = (string)row["NAME_ANF"],
					VORNAME_ANF = (string)row["VORNAME_ANF"],
					STRASSE_ANF = (string)row["STRASSE_ANF"],
					HNR_ANF = (string)row["HNR_ANF"],
					PLZ_ANF = (string)row["PLZ_ANF"],
					ORT_ANF = (string)row["ORT_ANF"],
					LAND_ANF = (string)row["LAND_ANF"],
					CLIENTLD = (string)row["CLIENTLD"],
					DAT_GEAEND = string.IsNullOrEmpty(row["DAT_GEAEND"].ToString()) ? null : (DateTime?)row["DAT_GEAEND"],
					DAT_VERSAUFTR = string.IsNullOrEmpty(row["DAT_VERSAUFTR"].ToString()) ? null : (DateTime?)row["DAT_VERSAUFTR"],
					DAT_UEBERM_1 = string.IsNullOrEmpty(row["DAT_UEBERM_1"].ToString()) ? null : (DateTime?)row["DAT_UEBERM_1"],
					VERS_STORNO = (string)row["VERS_STORNO"],
					ANREDE_EMPF = (string)row["ANREDE_EMPF"],
					VERSANDGRUND = (string)row["VERSANDGRUND"],
					ZZPLFOR = (string)row["ZZPLFOR"],
					ANREDE_ANF = (string)row["ANREDE_ANF"],
					TEL1_ANF = (string)row["TEL1_ANF"],
					TEL2_ANF = (string)row["TEL2_ANF"],
					MOBIL_ANF = (string)row["MOBIL_ANF"],
					FAX1_ANF = (string)row["FAX1_ANF"],
					FAX2_ANF = (string)row["FAX2_ANF"],
					EMAIL_ANF = (string)row["EMAIL_ANF"],
					INFO_ANF = (string)row["INFO_ANF"],
					ANREDE_ANSP = (string)row["ANREDE_ANSP"],
					NAME_ANSP = (string)row["NAME_ANSP"],
					VORNAME_ANSP = (string)row["VORNAME_ANSP"],
					USER_AUTOR = (string)row["USER_AUTOR"],
					ZVERT_ART = (string)row["ZVERT_ART"],
					ENDG_VERS = string.IsNullOrEmpty(row["ENDG_VERS"].ToString()) ? null : (DateTime?)row["ENDG_VERS"],
					ZZTMPDT = string.IsNullOrEmpty(row["ZZTMPDT"].ToString()) ? null : (DateTime?)row["ZZTMPDT"],
					ANFORDERUNGSWEG = (string)row["ANFORDERUNGSWEG"],
					KUNNR_BEIM_AG = (string)row["KUNNR_BEIM_AG"],
					NAME = (string)row["NAME"],
					CITY1 = (string)row["CITY1"],
					UZEIT_ANGEL = (string)row["UZEIT_ANGEL"],
					STATUS_SI = (string)row["STATUS_SI"],
					MAHNS_SI = (string)row["MAHNS_SI"],

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
				 
				var dts = sapDataService.GetExportTablesWithInitExecute("Z_DPM_EXP_VERS_AUSWERTUNG_01", inputParameterKeys, inputParameterValues);
				 
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
				 
				var dts = sapDataService.GetImportTablesWithInit("Z_DPM_EXP_VERS_AUSWERTUNG_01", inputParameterKeys, inputParameterValues);
				 
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

		public partial class GT_OUT_SUM : IModelMappingApplied
		{
			[SapIgnore]
			[ScriptIgnore]
			public ISapConnection SAPConnection { get; set; }

			[SapIgnore]
			[ScriptIgnore]
			public IDynSapProxyFactory DynSapProxyFactory { get; set; }

			public string KUNNR_BEIM_AG { get; set; }

			public string NAME { get; set; }

			public string ZVERT_ART { get; set; }

			public string ANFORDERUNGSWEG { get; set; }

			public string ANF_ART { get; set; }

			public string VERS_GRUND { get; set; }

			public string SUMME { get; set; }

			public string BEST_HAEND { get; set; }

			public string PROZ_VERS { get; set; }

			public static GT_OUT_SUM Create(DataRow row, ISapConnection sapConnection = null, IDynSapProxyFactory dynSapProxyFactory = null)
			{
				var o = new GT_OUT_SUM
				{
					KUNNR_BEIM_AG = (string)row["KUNNR_BEIM_AG"],
					NAME = (string)row["NAME"],
					ZVERT_ART = (string)row["ZVERT_ART"],
					ANFORDERUNGSWEG = (string)row["ANFORDERUNGSWEG"],
					ANF_ART = (string)row["ANF_ART"],
					VERS_GRUND = (string)row["VERS_GRUND"],
					SUMME = (string)row["SUMME"],
					BEST_HAEND = (string)row["BEST_HAEND"],
					PROZ_VERS = (string)row["PROZ_VERS"],

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

			public static IEnumerable<GT_OUT_SUM> Select(DataTable dt, ISapConnection sapConnection = null)
			{
				return dt.AsEnumerable().Select(r => Create(r, sapConnection));
			}

			public static List<GT_OUT_SUM> ToList(DataTable dt, ISapConnection sapConnection = null)
			{
				return Select(dt, sapConnection).ToListOrEmptyList();
			}

			public static IEnumerable<GT_OUT_SUM> Select(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				var tbl = dts.FirstOrDefault(t => t.TableName.ToLower() == typeof(GT_OUT_SUM).Name.ToLower());
				if (tbl == null)
					return null;

				return Select(tbl, sapConnection);
			}

			public static List<GT_OUT_SUM> ToList(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				return Select(dts, sapConnection).ToListOrEmptyList();
			}

			public static List<GT_OUT_SUM> ToList(ISapDataService sapDataService)
			{
				return ToList(sapDataService.GetExportTables(), sapDataService.SapConnection);
			}

			public static List<GT_OUT_SUM> GetExportListWithInitExecute(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<GT_OUT_SUM>();
				 
				var dts = sapDataService.GetExportTablesWithInitExecute("Z_DPM_EXP_VERS_AUSWERTUNG_01", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_OUT_SUM> GetExportListWithExecute(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_OUT_SUM>();
				 
				var dts = sapDataService.GetExportTablesWithExecute();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_OUT_SUM> GetExportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_OUT_SUM>();
				 
				var dts = sapDataService.GetExportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_OUT_SUM> GetImportListWithInit(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<GT_OUT_SUM>();
				 
				var dts = sapDataService.GetImportTablesWithInit("Z_DPM_EXP_VERS_AUSWERTUNG_01", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_OUT_SUM> GetImportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_OUT_SUM>();
				 
				var dts = sapDataService.GetImportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}
		}
	}

	public static partial class DataTableExtensions
	{

		public static DataTable ToTable(this IEnumerable<Z_DPM_EXP_VERS_AUSWERTUNG_01.GT_OUT> list)
		{
			return SapDataServiceExtensions.ToTable(list);
		}


		public static DataTable ToTable(this IEnumerable<Z_DPM_EXP_VERS_AUSWERTUNG_01.GT_OUT_SUM> list)
		{
			return SapDataServiceExtensions.ToTable(list);
		}

	}
}
