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
	public partial class Z_DPM_ASSIST_IMP_BESTAND_01
	{
		public static void Init(ISapDataService sap)
		{
			sap.Init(typeof(Z_DPM_ASSIST_IMP_BESTAND_01).Name);
		}

		public static void Init(ISapDataService sap, string inputParameterKeys, params object[] inputParameterValues)
		{
			sap.Init(typeof(Z_DPM_ASSIST_IMP_BESTAND_01).Name, inputParameterKeys, inputParameterValues);
		}


		public void SetImportParameter_I_KUNNR_AH(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_KUNNR_AH", value);
		}

		public void SetImportParameter_I_KUNNR_FIL(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_KUNNR_FIL", value);
		}

		public void SetImportParameter_I_VKORG(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_VKORG", value);
		}

		public string GetExportParameter_E_MESSAGE(ISapDataService sap)
		{
			return sap.GetExportParameter<string>("E_MESSAGE");
		}

		public int? GetExportParameter_E_SUBRC(ISapDataService sap)
		{
			return sap.GetExportParameter<int?>("E_SUBRC");
		}

		public partial class GT_ERR : IModelMappingApplied
		{
			[SapIgnore]
			[ScriptIgnore]
			public ISapConnection SAPConnection { get; set; }

			[SapIgnore]
			[ScriptIgnore]
			public IDynSapProxyFactory DynSapProxyFactory { get; set; }

			public string LFDNR { get; set; }

			public string ANRED { get; set; }

			public string NAME1 { get; set; }

			public string NAME2 { get; set; }

			public string NAME3 { get; set; }

			public string TITEL { get; set; }

			public string LANDT { get; set; }

			public string PSTLZ { get; set; }

			public string ORT { get; set; }

			public string STREET { get; set; }

			public DateTime? GEBURTSDATUM { get; set; }

			public string STAATSANGE1 { get; set; }

			public string STAATSANGE2 { get; set; }

			public string GESCHLECHT { get; set; }

			public string KONTONUMMER { get; set; }

			public string BLZ { get; set; }

			public string INSTITUTSNAME { get; set; }

			public string LAND2 { get; set; }

			public string ABWKONTOINHABER { get; set; }

			public string KOMMTYP1 { get; set; }

			public string KOMMSNO1 { get; set; }

			public string KOMMTYP2 { get; set; }

			public string KOMMSNO2 { get; set; }

			public string VUNUMMER { get; set; }

			public string VERSNUMMER { get; set; }

			public string VERMITTLER { get; set; }

			public string ABGRP { get; set; }

			public DateTime? VERTRAGSBEGINN { get; set; }

			public DateTime? VERTRAGSABLAUF { get; set; }

			public string VERTRAGSSTATUS { get; set; }

			public string BEDINGUNGEN { get; set; }

			public string PRODUKTTYP { get; set; }

			public string DECKUNGSART { get; set; }

			public string ANZ_RISIKEN { get; set; }

			public string KENNZEICHEN { get; set; }

			public string HERSTNAME { get; set; }

			public string FIN { get; set; }

			public string MEHRFZKLAUSEL { get; set; }

			public DateTime? ERSTZULDAT { get; set; }

			public string WAGNISZIFFER { get; set; }

			public string GELTUNGSBEREICH { get; set; }

			public string KVVERSICHERUNG { get; set; }

			public string EURO_VTRG { get; set; }

			public string SBT_VK { get; set; }

			public string WAERS1 { get; set; }

			public string SBT_TK { get; set; }

			public string WAERS2 { get; set; }

			public string KASKO { get; set; }

			public string GETRIEBE { get; set; }

			public string WKS_NAME { get; set; }

			public string WKS_PLZ { get; set; }

			public string WKS_ORT { get; set; }

			public string WKS_STRASSE { get; set; }

			public string WKS_ZEIT { get; set; }

			public static GT_ERR Create(DataRow row, ISapConnection sapConnection = null, IDynSapProxyFactory dynSapProxyFactory = null)
			{
				var o = new GT_ERR
				{
					LFDNR = (string)row["LFDNR"],
					ANRED = (string)row["ANRED"],
					NAME1 = (string)row["NAME1"],
					NAME2 = (string)row["NAME2"],
					NAME3 = (string)row["NAME3"],
					TITEL = (string)row["TITEL"],
					LANDT = (string)row["LANDT"],
					PSTLZ = (string)row["PSTLZ"],
					ORT = (string)row["ORT"],
					STREET = (string)row["STREET"],
					GEBURTSDATUM = string.IsNullOrEmpty(row["GEBURTSDATUM"].ToString()) ? null : (DateTime?)row["GEBURTSDATUM"],
					STAATSANGE1 = (string)row["STAATSANGE1"],
					STAATSANGE2 = (string)row["STAATSANGE2"],
					GESCHLECHT = (string)row["GESCHLECHT"],
					KONTONUMMER = (string)row["KONTONUMMER"],
					BLZ = (string)row["BLZ"],
					INSTITUTSNAME = (string)row["INSTITUTSNAME"],
					LAND2 = (string)row["LAND2"],
					ABWKONTOINHABER = (string)row["ABWKONTOINHABER"],
					KOMMTYP1 = (string)row["KOMMTYP1"],
					KOMMSNO1 = (string)row["KOMMSNO1"],
					KOMMTYP2 = (string)row["KOMMTYP2"],
					KOMMSNO2 = (string)row["KOMMSNO2"],
					VUNUMMER = (string)row["VUNUMMER"],
					VERSNUMMER = (string)row["VERSNUMMER"],
					VERMITTLER = (string)row["VERMITTLER"],
					ABGRP = (string)row["ABGRP"],
					VERTRAGSBEGINN = string.IsNullOrEmpty(row["VERTRAGSBEGINN"].ToString()) ? null : (DateTime?)row["VERTRAGSBEGINN"],
					VERTRAGSABLAUF = string.IsNullOrEmpty(row["VERTRAGSABLAUF"].ToString()) ? null : (DateTime?)row["VERTRAGSABLAUF"],
					VERTRAGSSTATUS = (string)row["VERTRAGSSTATUS"],
					BEDINGUNGEN = (string)row["BEDINGUNGEN"],
					PRODUKTTYP = (string)row["PRODUKTTYP"],
					DECKUNGSART = (string)row["DECKUNGSART"],
					ANZ_RISIKEN = (string)row["ANZ_RISIKEN"],
					KENNZEICHEN = (string)row["KENNZEICHEN"],
					HERSTNAME = (string)row["HERSTNAME"],
					FIN = (string)row["FIN"],
					MEHRFZKLAUSEL = (string)row["MEHRFZKLAUSEL"],
					ERSTZULDAT = string.IsNullOrEmpty(row["ERSTZULDAT"].ToString()) ? null : (DateTime?)row["ERSTZULDAT"],
					WAGNISZIFFER = (string)row["WAGNISZIFFER"],
					GELTUNGSBEREICH = (string)row["GELTUNGSBEREICH"],
					KVVERSICHERUNG = (string)row["KVVERSICHERUNG"],
					EURO_VTRG = (string)row["EURO_VTRG"],
					SBT_VK = (string)row["SBT_VK"],
					WAERS1 = (string)row["WAERS1"],
					SBT_TK = (string)row["SBT_TK"],
					WAERS2 = (string)row["WAERS2"],
					KASKO = (string)row["KASKO"],
					GETRIEBE = (string)row["GETRIEBE"],
					WKS_NAME = (string)row["WKS_NAME"],
					WKS_PLZ = (string)row["WKS_PLZ"],
					WKS_ORT = (string)row["WKS_ORT"],
					WKS_STRASSE = (string)row["WKS_STRASSE"],
					WKS_ZEIT = (string)row["WKS_ZEIT"],

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

			public static IEnumerable<GT_ERR> Select(DataTable dt, ISapConnection sapConnection = null)
			{
				return dt.AsEnumerable().Select(r => Create(r, sapConnection));
			}

			public static List<GT_ERR> ToList(DataTable dt, ISapConnection sapConnection = null)
			{
				return Select(dt, sapConnection).ToListOrEmptyList();
			}

			public static IEnumerable<GT_ERR> Select(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				var tbl = dts.FirstOrDefault(t => t.TableName.ToLower() == typeof(GT_ERR).Name.ToLower());
				if (tbl == null)
					return null;

				return Select(tbl, sapConnection);
			}

			public static List<GT_ERR> ToList(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				return Select(dts, sapConnection).ToListOrEmptyList();
			}

			public static List<GT_ERR> ToList(ISapDataService sapDataService)
			{
				return ToList(sapDataService.GetExportTables(), sapDataService.SapConnection);
			}

			public static List<GT_ERR> GetExportListWithInitExecute(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<GT_ERR>();
				 
				var dts = sapDataService.GetExportTablesWithInitExecute("Z_DPM_ASSIST_IMP_BESTAND_01", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_ERR> GetExportListWithExecute(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_ERR>();
				 
				var dts = sapDataService.GetExportTablesWithExecute();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_ERR> GetExportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_ERR>();
				 
				var dts = sapDataService.GetExportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_ERR> GetImportListWithInit(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<GT_ERR>();
				 
				var dts = sapDataService.GetImportTablesWithInit("Z_DPM_ASSIST_IMP_BESTAND_01", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_ERR> GetImportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_ERR>();
				 
				var dts = sapDataService.GetImportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}
		}

		public partial class GT_IN : IModelMappingApplied
		{
			[SapIgnore]
			[ScriptIgnore]
			public ISapConnection SAPConnection { get; set; }

			[SapIgnore]
			[ScriptIgnore]
			public IDynSapProxyFactory DynSapProxyFactory { get; set; }

			public string LFDNR { get; set; }

			public string ANRED { get; set; }

			public string NAME1 { get; set; }

			public string NAME2 { get; set; }

			public string NAME3 { get; set; }

			public string TITEL { get; set; }

			public string LANDT { get; set; }

			public string PSTLZ { get; set; }

			public string ORT { get; set; }

			public string STREET { get; set; }

			public DateTime? GEBURTSDATUM { get; set; }

			public string STAATSANGE1 { get; set; }

			public string STAATSANGE2 { get; set; }

			public string GESCHLECHT { get; set; }

			public string KONTONUMMER { get; set; }

			public string BLZ { get; set; }

			public string INSTITUTSNAME { get; set; }

			public string LAND2 { get; set; }

			public string ABWKONTOINHABER { get; set; }

			public string KOMMTYP1 { get; set; }

			public string KOMMSNO1 { get; set; }

			public string KOMMTYP2 { get; set; }

			public string KOMMSNO2 { get; set; }

			public string VUNUMMER { get; set; }

			public string VERSNUMMER { get; set; }

			public string VERMITTLER { get; set; }

			public string ABGRP { get; set; }

			public DateTime? VERTRAGSBEGINN { get; set; }

			public DateTime? VERTRAGSABLAUF { get; set; }

			public string VERTRAGSSTATUS { get; set; }

			public string BEDINGUNGEN { get; set; }

			public string PRODUKTTYP { get; set; }

			public string DECKUNGSART { get; set; }

			public string ANZ_RISIKEN { get; set; }

			public string KENNZEICHEN { get; set; }

			public string HERSTNAME { get; set; }

			public string FIN { get; set; }

			public string MEHRFZKLAUSEL { get; set; }

			public DateTime? ERSTZULDAT { get; set; }

			public string WAGNISZIFFER { get; set; }

			public string GELTUNGSBEREICH { get; set; }

			public string KVVERSICHERUNG { get; set; }

			public string EURO_VTRG { get; set; }

			public string SBT_VK { get; set; }

			public string WAERS1 { get; set; }

			public string SBT_TK { get; set; }

			public string WAERS2 { get; set; }

			public string KASKO { get; set; }

			public string GETRIEBE { get; set; }

			public string WKS_NAME { get; set; }

			public string WKS_PLZ { get; set; }

			public string WKS_ORT { get; set; }

			public string WKS_STRASSE { get; set; }

			public string WKS_ZEIT { get; set; }

			public static GT_IN Create(DataRow row, ISapConnection sapConnection = null, IDynSapProxyFactory dynSapProxyFactory = null)
			{
				var o = new GT_IN
				{
					LFDNR = (string)row["LFDNR"],
					ANRED = (string)row["ANRED"],
					NAME1 = (string)row["NAME1"],
					NAME2 = (string)row["NAME2"],
					NAME3 = (string)row["NAME3"],
					TITEL = (string)row["TITEL"],
					LANDT = (string)row["LANDT"],
					PSTLZ = (string)row["PSTLZ"],
					ORT = (string)row["ORT"],
					STREET = (string)row["STREET"],
					GEBURTSDATUM = string.IsNullOrEmpty(row["GEBURTSDATUM"].ToString()) ? null : (DateTime?)row["GEBURTSDATUM"],
					STAATSANGE1 = (string)row["STAATSANGE1"],
					STAATSANGE2 = (string)row["STAATSANGE2"],
					GESCHLECHT = (string)row["GESCHLECHT"],
					KONTONUMMER = (string)row["KONTONUMMER"],
					BLZ = (string)row["BLZ"],
					INSTITUTSNAME = (string)row["INSTITUTSNAME"],
					LAND2 = (string)row["LAND2"],
					ABWKONTOINHABER = (string)row["ABWKONTOINHABER"],
					KOMMTYP1 = (string)row["KOMMTYP1"],
					KOMMSNO1 = (string)row["KOMMSNO1"],
					KOMMTYP2 = (string)row["KOMMTYP2"],
					KOMMSNO2 = (string)row["KOMMSNO2"],
					VUNUMMER = (string)row["VUNUMMER"],
					VERSNUMMER = (string)row["VERSNUMMER"],
					VERMITTLER = (string)row["VERMITTLER"],
					ABGRP = (string)row["ABGRP"],
					VERTRAGSBEGINN = string.IsNullOrEmpty(row["VERTRAGSBEGINN"].ToString()) ? null : (DateTime?)row["VERTRAGSBEGINN"],
					VERTRAGSABLAUF = string.IsNullOrEmpty(row["VERTRAGSABLAUF"].ToString()) ? null : (DateTime?)row["VERTRAGSABLAUF"],
					VERTRAGSSTATUS = (string)row["VERTRAGSSTATUS"],
					BEDINGUNGEN = (string)row["BEDINGUNGEN"],
					PRODUKTTYP = (string)row["PRODUKTTYP"],
					DECKUNGSART = (string)row["DECKUNGSART"],
					ANZ_RISIKEN = (string)row["ANZ_RISIKEN"],
					KENNZEICHEN = (string)row["KENNZEICHEN"],
					HERSTNAME = (string)row["HERSTNAME"],
					FIN = (string)row["FIN"],
					MEHRFZKLAUSEL = (string)row["MEHRFZKLAUSEL"],
					ERSTZULDAT = string.IsNullOrEmpty(row["ERSTZULDAT"].ToString()) ? null : (DateTime?)row["ERSTZULDAT"],
					WAGNISZIFFER = (string)row["WAGNISZIFFER"],
					GELTUNGSBEREICH = (string)row["GELTUNGSBEREICH"],
					KVVERSICHERUNG = (string)row["KVVERSICHERUNG"],
					EURO_VTRG = (string)row["EURO_VTRG"],
					SBT_VK = (string)row["SBT_VK"],
					WAERS1 = (string)row["WAERS1"],
					SBT_TK = (string)row["SBT_TK"],
					WAERS2 = (string)row["WAERS2"],
					KASKO = (string)row["KASKO"],
					GETRIEBE = (string)row["GETRIEBE"],
					WKS_NAME = (string)row["WKS_NAME"],
					WKS_PLZ = (string)row["WKS_PLZ"],
					WKS_ORT = (string)row["WKS_ORT"],
					WKS_STRASSE = (string)row["WKS_STRASSE"],
					WKS_ZEIT = (string)row["WKS_ZEIT"],

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

			public static IEnumerable<GT_IN> Select(DataTable dt, ISapConnection sapConnection = null)
			{
				return dt.AsEnumerable().Select(r => Create(r, sapConnection));
			}

			public static List<GT_IN> ToList(DataTable dt, ISapConnection sapConnection = null)
			{
				return Select(dt, sapConnection).ToListOrEmptyList();
			}

			public static IEnumerable<GT_IN> Select(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				var tbl = dts.FirstOrDefault(t => t.TableName.ToLower() == typeof(GT_IN).Name.ToLower());
				if (tbl == null)
					return null;

				return Select(tbl, sapConnection);
			}

			public static List<GT_IN> ToList(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				return Select(dts, sapConnection).ToListOrEmptyList();
			}

			public static List<GT_IN> ToList(ISapDataService sapDataService)
			{
				return ToList(sapDataService.GetExportTables(), sapDataService.SapConnection);
			}

			public static List<GT_IN> GetExportListWithInitExecute(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<GT_IN>();
				 
				var dts = sapDataService.GetExportTablesWithInitExecute("Z_DPM_ASSIST_IMP_BESTAND_01", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_IN> GetExportListWithExecute(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_IN>();
				 
				var dts = sapDataService.GetExportTablesWithExecute();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_IN> GetExportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_IN>();
				 
				var dts = sapDataService.GetExportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_IN> GetImportListWithInit(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<GT_IN>();
				 
				var dts = sapDataService.GetImportTablesWithInit("Z_DPM_ASSIST_IMP_BESTAND_01", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_IN> GetImportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_IN>();
				 
				var dts = sapDataService.GetImportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}
		}
	}

	public static partial class DataTableExtensions
	{

		public static DataTable ToTable(this IEnumerable<Z_DPM_ASSIST_IMP_BESTAND_01.GT_ERR> list)
		{
			return SapDataServiceExtensions.ToTable(list);
		}


		public static DataTable ToTable(this IEnumerable<Z_DPM_ASSIST_IMP_BESTAND_01.GT_IN> list)
		{
			return SapDataServiceExtensions.ToTable(list);
		}

	}
}
