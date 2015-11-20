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
	public partial class Z_DPM_VERTRAGSBESTAND_01
	{
		public static void Init(ISapDataService sap)
		{
			sap.Init(typeof(Z_DPM_VERTRAGSBESTAND_01).Name);
		}

		public static void Init(ISapDataService sap, string inputParameterKeys, params object[] inputParameterValues)
		{
			sap.Init(typeof(Z_DPM_VERTRAGSBESTAND_01).Name, inputParameterKeys, inputParameterValues);
		}


		public void SetImportParameter_I_CIN(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_CIN", value);
		}

		public void SetImportParameter_I_KONTONR(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_KONTONR", value);
		}

		public void SetImportParameter_I_KUNNR_AG(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_KUNNR_AG", value);
		}

		public void SetImportParameter_I_NAME(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_NAME", value);
		}

		public void SetImportParameter_I_PAID(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_PAID", value);
		}

		public void SetImportParameter_I_VERT_ART(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_VERT_ART", value);
		}

		public partial class GT_NOTIZ : IModelMappingApplied
		{
			[SapIgnore]
			[ScriptIgnore]
			public ISapConnection SAPConnection { get; set; }

			[SapIgnore]
			[ScriptIgnore]
			public IDynSapProxyFactory DynSapProxyFactory { get; set; }

			public string KUNNR { get; set; }

			public string KONTONR { get; set; }

			public string CIN { get; set; }

			public string PAID { get; set; }

			public DateTime? DATUM { get; set; }

			public string UHRZEIT { get; set; }

			public string USERNAME { get; set; }

			public string HERKUNFT_D_NOTIZ { get; set; }

			public string GESPRAECHSPARTNER { get; set; }

			public string GESPRAECHSNOTIZ { get; set; }

			public static GT_NOTIZ Create(DataRow row, ISapConnection sapConnection = null, IDynSapProxyFactory dynSapProxyFactory = null)
			{
				var o = new GT_NOTIZ
				{
					KUNNR = (string)row["KUNNR"],
					KONTONR = (string)row["KONTONR"],
					CIN = (string)row["CIN"],
					PAID = (string)row["PAID"],
					DATUM = string.IsNullOrEmpty(row["DATUM"].ToString()) ? null : (DateTime?)row["DATUM"],
					UHRZEIT = (string)row["UHRZEIT"],
					USERNAME = (string)row["USERNAME"],
					HERKUNFT_D_NOTIZ = (string)row["HERKUNFT_D_NOTIZ"],
					GESPRAECHSPARTNER = (string)row["GESPRAECHSPARTNER"],
					GESPRAECHSNOTIZ = (string)row["GESPRAECHSNOTIZ"],

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

			public static IEnumerable<GT_NOTIZ> Select(DataTable dt, ISapConnection sapConnection = null)
			{
				return dt.AsEnumerable().Select(r => Create(r, sapConnection));
			}

			public static List<GT_NOTIZ> ToList(DataTable dt, ISapConnection sapConnection = null)
			{
				return Select(dt, sapConnection).ToListOrEmptyList();
			}

			public static IEnumerable<GT_NOTIZ> Select(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				var tbl = dts.FirstOrDefault(t => t.TableName.ToLower() == typeof(GT_NOTIZ).Name.ToLower());
				if (tbl == null)
					return null;

				return Select(tbl, sapConnection);
			}

			public static List<GT_NOTIZ> ToList(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				return Select(dts, sapConnection).ToListOrEmptyList();
			}

			public static List<GT_NOTIZ> ToList(ISapDataService sapDataService)
			{
				return ToList(sapDataService.GetExportTables(), sapDataService.SapConnection);
			}

			public static List<GT_NOTIZ> GetExportListWithInitExecute(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<GT_NOTIZ>();
				 
				var dts = sapDataService.GetExportTablesWithInitExecute("Z_DPM_VERTRAGSBESTAND_01", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_NOTIZ> GetExportListWithExecute(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_NOTIZ>();
				 
				var dts = sapDataService.GetExportTablesWithExecute();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_NOTIZ> GetExportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_NOTIZ>();
				 
				var dts = sapDataService.GetExportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_NOTIZ> GetImportListWithInit(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<GT_NOTIZ>();
				 
				var dts = sapDataService.GetImportTablesWithInit("Z_DPM_VERTRAGSBESTAND_01", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_NOTIZ> GetImportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_NOTIZ>();
				 
				var dts = sapDataService.GetImportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}
		}

		public partial class GT_OUT : IModelMappingApplied
		{
			[SapIgnore]
			[ScriptIgnore]
			public ISapConnection SAPConnection { get; set; }

			[SapIgnore]
			[ScriptIgnore]
			public IDynSapProxyFactory DynSapProxyFactory { get; set; }

			public string KONTONR { get; set; }

			public string PAID { get; set; }

			public string CIN { get; set; }

			public string HAENDL_VERK_WERT { get; set; }

			public DateTime? PRUEFDAT_VERK_WERT { get; set; }

			public string HAENDL_EINK_WERT { get; set; }

			public DateTime? PRUEFDAT_EINK_WER { get; set; }

			public string CHASSIS_NUM { get; set; }

			public string NAME_KRED { get; set; }

			public string STREET_KRED { get; set; }

			public string CITY1_KRED { get; set; }

			public string NAME_FH { get; set; }

			public string STREET_FH { get; set; }

			public string CITY1_FH { get; set; }

			public DateTime? DAT_ERST_IMP { get; set; }

			public DateTime? AENDAT { get; set; }

			public DateTime? EINGANG_ZB2 { get; set; }

			public DateTime? EINGANG_COC { get; set; }

			public DateTime? EINGANG_SUE { get; set; }

			public string NAME1_VA { get; set; }

			public string NAME2_VA { get; set; }

			public string STREET_VA { get; set; }

			public string CITY1_VA { get; set; }

			public string FZG_STANDORT { get; set; }

			public DateTime? ZZTMPDT { get; set; }

			public string NAME_KRED2 { get; set; }

			public string STREET_KRED2 { get; set; }

			public string CITY1_KRED2 { get; set; }

			public DateTime? DAT_VERTR_REAKT { get; set; }

			public DateTime? VERS_INFO_PAK { get; set; }

			public DateTime? ENDG_VERS { get; set; }

			public string WEB_USER_REAKT { get; set; }

			public DateTime? MELD_ABWEICH { get; set; }

			public string GRUND_ABWEICH { get; set; }

			public DateTime? DAT_VERTR_ENDE { get; set; }

			public DateTime? MAHNSP_GES_AM { get; set; }

			public string MAHNSP_GES_US { get; set; }

			public string BEM { get; set; }

			public DateTime? MAHNDATUM_AB { get; set; }

			public string GRUND_ABW_TEXT { get; set; }

			public string ZVERT_ART { get; set; }

			public DateTime? MAX_EINR_FRIST { get; set; }

			public string KORREKTURFAKTOR { get; set; }

			public static GT_OUT Create(DataRow row, ISapConnection sapConnection = null, IDynSapProxyFactory dynSapProxyFactory = null)
			{
				var o = new GT_OUT
				{
					KONTONR = (string)row["KONTONR"],
					PAID = (string)row["PAID"],
					CIN = (string)row["CIN"],
					HAENDL_VERK_WERT = (string)row["HAENDL_VERK_WERT"],
					PRUEFDAT_VERK_WERT = string.IsNullOrEmpty(row["PRUEFDAT_VERK_WERT"].ToString()) ? null : (DateTime?)row["PRUEFDAT_VERK_WERT"],
					HAENDL_EINK_WERT = (string)row["HAENDL_EINK_WERT"],
					PRUEFDAT_EINK_WER = string.IsNullOrEmpty(row["PRUEFDAT_EINK_WER"].ToString()) ? null : (DateTime?)row["PRUEFDAT_EINK_WER"],
					CHASSIS_NUM = (string)row["CHASSIS_NUM"],
					NAME_KRED = (string)row["NAME_KRED"],
					STREET_KRED = (string)row["STREET_KRED"],
					CITY1_KRED = (string)row["CITY1_KRED"],
					NAME_FH = (string)row["NAME_FH"],
					STREET_FH = (string)row["STREET_FH"],
					CITY1_FH = (string)row["CITY1_FH"],
					DAT_ERST_IMP = string.IsNullOrEmpty(row["DAT_ERST_IMP"].ToString()) ? null : (DateTime?)row["DAT_ERST_IMP"],
					AENDAT = string.IsNullOrEmpty(row["AENDAT"].ToString()) ? null : (DateTime?)row["AENDAT"],
					EINGANG_ZB2 = string.IsNullOrEmpty(row["EINGANG_ZB2"].ToString()) ? null : (DateTime?)row["EINGANG_ZB2"],
					EINGANG_COC = string.IsNullOrEmpty(row["EINGANG_COC"].ToString()) ? null : (DateTime?)row["EINGANG_COC"],
					EINGANG_SUE = string.IsNullOrEmpty(row["EINGANG_SUE"].ToString()) ? null : (DateTime?)row["EINGANG_SUE"],
					NAME1_VA = (string)row["NAME1_VA"],
					NAME2_VA = (string)row["NAME2_VA"],
					STREET_VA = (string)row["STREET_VA"],
					CITY1_VA = (string)row["CITY1_VA"],
					FZG_STANDORT = (string)row["FZG_STANDORT"],
					ZZTMPDT = string.IsNullOrEmpty(row["ZZTMPDT"].ToString()) ? null : (DateTime?)row["ZZTMPDT"],
					NAME_KRED2 = (string)row["NAME_KRED2"],
					STREET_KRED2 = (string)row["STREET_KRED2"],
					CITY1_KRED2 = (string)row["CITY1_KRED2"],
					DAT_VERTR_REAKT = string.IsNullOrEmpty(row["DAT_VERTR_REAKT"].ToString()) ? null : (DateTime?)row["DAT_VERTR_REAKT"],
					VERS_INFO_PAK = string.IsNullOrEmpty(row["VERS_INFO_PAK"].ToString()) ? null : (DateTime?)row["VERS_INFO_PAK"],
					ENDG_VERS = string.IsNullOrEmpty(row["ENDG_VERS"].ToString()) ? null : (DateTime?)row["ENDG_VERS"],
					WEB_USER_REAKT = (string)row["WEB_USER_REAKT"],
					MELD_ABWEICH = string.IsNullOrEmpty(row["MELD_ABWEICH"].ToString()) ? null : (DateTime?)row["MELD_ABWEICH"],
					GRUND_ABWEICH = (string)row["GRUND_ABWEICH"],
					DAT_VERTR_ENDE = string.IsNullOrEmpty(row["DAT_VERTR_ENDE"].ToString()) ? null : (DateTime?)row["DAT_VERTR_ENDE"],
					MAHNSP_GES_AM = string.IsNullOrEmpty(row["MAHNSP_GES_AM"].ToString()) ? null : (DateTime?)row["MAHNSP_GES_AM"],
					MAHNSP_GES_US = (string)row["MAHNSP_GES_US"],
					BEM = (string)row["BEM"],
					MAHNDATUM_AB = string.IsNullOrEmpty(row["MAHNDATUM_AB"].ToString()) ? null : (DateTime?)row["MAHNDATUM_AB"],
					GRUND_ABW_TEXT = (string)row["GRUND_ABW_TEXT"],
					ZVERT_ART = (string)row["ZVERT_ART"],
					MAX_EINR_FRIST = string.IsNullOrEmpty(row["MAX_EINR_FRIST"].ToString()) ? null : (DateTime?)row["MAX_EINR_FRIST"],
					KORREKTURFAKTOR = (string)row["KORREKTURFAKTOR"],

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
				 
				var dts = sapDataService.GetExportTablesWithInitExecute("Z_DPM_VERTRAGSBESTAND_01", inputParameterKeys, inputParameterValues);
				 
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
				 
				var dts = sapDataService.GetImportTablesWithInit("Z_DPM_VERTRAGSBESTAND_01", inputParameterKeys, inputParameterValues);
				 
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
	}

	public static partial class DataTableExtensions
	{

		public static DataTable ToTable(this IEnumerable<Z_DPM_VERTRAGSBESTAND_01.GT_NOTIZ> list)
		{
			return SapDataServiceExtensions.ToTable(list);
		}


		public static DataTable ToTable(this IEnumerable<Z_DPM_VERTRAGSBESTAND_01.GT_OUT> list)
		{
			return SapDataServiceExtensions.ToTable(list);
		}

	}
}
