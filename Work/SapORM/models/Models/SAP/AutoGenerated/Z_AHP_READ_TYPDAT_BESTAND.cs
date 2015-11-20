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
	public partial class Z_AHP_READ_TYPDAT_BESTAND
	{
		public static void Init(ISapDataService sap)
		{
			sap.Init(typeof(Z_AHP_READ_TYPDAT_BESTAND).Name);
		}

		public static void Init(ISapDataService sap, string inputParameterKeys, params object[] inputParameterValues)
		{
			sap.Init(typeof(Z_AHP_READ_TYPDAT_BESTAND).Name, inputParameterKeys, inputParameterValues);
		}


		public void SetImportParameter_I_FIN(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_FIN", value);
		}

		public void SetImportParameter_I_KUNNR(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_KUNNR", value);
		}

		public void SetImportParameter_I_PRUEF(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_PRUEF", value);
		}

		public void SetImportParameter_I_ZZHERSTELLER_SCH(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_ZZHERSTELLER_SCH", value);
		}

		public void SetImportParameter_I_ZZTYP_SCHL(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_ZZTYP_SCHL", value);
		}

		public void SetImportParameter_I_ZZVVS_SCHLUESSEL(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_ZZVVS_SCHLUESSEL", value);
		}

		public partial class GT_WEB_BESTAND : IModelMappingApplied
		{
			[SapIgnore]
			[ScriptIgnore]
			public ISapConnection SAPConnection { get; set; }

			[SapIgnore]
			[ScriptIgnore]
			public IDynSapProxyFactory DynSapProxyFactory { get; set; }

			public string FIN_ID { get; set; }

			public string FIN { get; set; }

			public DateTime? ERDAT { get; set; }

			public string ZZHERSTELLER_SCH { get; set; }

			public string ZZTYP_SCHL { get; set; }

			public string ZZVVS_SCHLUESSEL { get; set; }

			public string ZZTYP_VVS_PRUEF { get; set; }

			public string ZZFABRIKNAME { get; set; }

			public string ZZHANDELSNAME { get; set; }

			public string KUNNR { get; set; }

			public string KAEUFER { get; set; }

			public string HALTER { get; set; }

			public string BRIEFBESTAND { get; set; }

			public string LGORT { get; set; }

			public string STANDORT { get; set; }

			public DateTime? ERSTZULDAT { get; set; }

			public DateTime? AKTZULDAT { get; set; }

			public DateTime? ABMDAT { get; set; }

			public string KENNZ { get; set; }

			public string BRIEFNR { get; set; }

			public string COCVORHANDEN { get; set; }

			public string BEMERKUNG { get; set; }

			public string FZGART { get; set; }

			public string VKSPARTE { get; set; }

			public string FZGNR { get; set; }

			public string AUFNR { get; set; }

			public string FAREF1 { get; set; }

			public string FAREF2 { get; set; }

			public string KOSTL { get; set; }

			public string KONTOINHABER { get; set; }

			public static GT_WEB_BESTAND Create(DataRow row, ISapConnection sapConnection = null, IDynSapProxyFactory dynSapProxyFactory = null)
			{
				var o = new GT_WEB_BESTAND
				{
					FIN_ID = (string)row["FIN_ID"],
					FIN = (string)row["FIN"],
					ERDAT = string.IsNullOrEmpty(row["ERDAT"].ToString()) ? null : (DateTime?)row["ERDAT"],
					ZZHERSTELLER_SCH = (string)row["ZZHERSTELLER_SCH"],
					ZZTYP_SCHL = (string)row["ZZTYP_SCHL"],
					ZZVVS_SCHLUESSEL = (string)row["ZZVVS_SCHLUESSEL"],
					ZZTYP_VVS_PRUEF = (string)row["ZZTYP_VVS_PRUEF"],
					ZZFABRIKNAME = (string)row["ZZFABRIKNAME"],
					ZZHANDELSNAME = (string)row["ZZHANDELSNAME"],
					KUNNR = (string)row["KUNNR"],
					KAEUFER = (string)row["KAEUFER"],
					HALTER = (string)row["HALTER"],
					BRIEFBESTAND = (string)row["BRIEFBESTAND"],
					LGORT = (string)row["LGORT"],
					STANDORT = (string)row["STANDORT"],
					ERSTZULDAT = string.IsNullOrEmpty(row["ERSTZULDAT"].ToString()) ? null : (DateTime?)row["ERSTZULDAT"],
					AKTZULDAT = string.IsNullOrEmpty(row["AKTZULDAT"].ToString()) ? null : (DateTime?)row["AKTZULDAT"],
					ABMDAT = string.IsNullOrEmpty(row["ABMDAT"].ToString()) ? null : (DateTime?)row["ABMDAT"],
					KENNZ = (string)row["KENNZ"],
					BRIEFNR = (string)row["BRIEFNR"],
					COCVORHANDEN = (string)row["COCVORHANDEN"],
					BEMERKUNG = (string)row["BEMERKUNG"],
					FZGART = (string)row["FZGART"],
					VKSPARTE = (string)row["VKSPARTE"],
					FZGNR = (string)row["FZGNR"],
					AUFNR = (string)row["AUFNR"],
					FAREF1 = (string)row["FAREF1"],
					FAREF2 = (string)row["FAREF2"],
					KOSTL = (string)row["KOSTL"],
					KONTOINHABER = (string)row["KONTOINHABER"],

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

			public static IEnumerable<GT_WEB_BESTAND> Select(DataTable dt, ISapConnection sapConnection = null)
			{
				return dt.AsEnumerable().Select(r => Create(r, sapConnection));
			}

			public static List<GT_WEB_BESTAND> ToList(DataTable dt, ISapConnection sapConnection = null)
			{
				return Select(dt, sapConnection).ToListOrEmptyList();
			}

			public static IEnumerable<GT_WEB_BESTAND> Select(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				var tbl = dts.FirstOrDefault(t => t.TableName.ToLower() == typeof(GT_WEB_BESTAND).Name.ToLower());
				if (tbl == null)
					return null;

				return Select(tbl, sapConnection);
			}

			public static List<GT_WEB_BESTAND> ToList(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				return Select(dts, sapConnection).ToListOrEmptyList();
			}

			public static List<GT_WEB_BESTAND> ToList(ISapDataService sapDataService)
			{
				return ToList(sapDataService.GetExportTables(), sapDataService.SapConnection);
			}

			public static List<GT_WEB_BESTAND> GetExportListWithInitExecute(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<GT_WEB_BESTAND>();
				 
				var dts = sapDataService.GetExportTablesWithInitExecute("Z_AHP_READ_TYPDAT_BESTAND", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_WEB_BESTAND> GetExportListWithExecute(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_WEB_BESTAND>();
				 
				var dts = sapDataService.GetExportTablesWithExecute();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_WEB_BESTAND> GetExportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_WEB_BESTAND>();
				 
				var dts = sapDataService.GetExportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_WEB_BESTAND> GetImportListWithInit(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<GT_WEB_BESTAND>();
				 
				var dts = sapDataService.GetImportTablesWithInit("Z_AHP_READ_TYPDAT_BESTAND", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_WEB_BESTAND> GetImportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_WEB_BESTAND>();
				 
				var dts = sapDataService.GetImportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}
		}

		public partial class GT_WEB_IMP_MASS : IModelMappingApplied
		{
			[SapIgnore]
			[ScriptIgnore]
			public ISapConnection SAPConnection { get; set; }

			[SapIgnore]
			[ScriptIgnore]
			public IDynSapProxyFactory DynSapProxyFactory { get; set; }

			public string FIN { get; set; }

			public string ZZHERSTELLER_SCH { get; set; }

			public string ZZTYP_SCHL { get; set; }

			public string ZZVVS_SCHLUESSEL { get; set; }

			public string ZZTYP_VVS_PRUEF { get; set; }

			public string KENNZ { get; set; }

			public DateTime? ERSTZULDAT { get; set; }

			public DateTime? AKTZULDAT { get; set; }

			public DateTime? ABMDAT { get; set; }

			public string FZGART { get; set; }

			public string VKSPARTE { get; set; }

			public string FZGNR { get; set; }

			public string AUFNR { get; set; }

			public string FAREF1 { get; set; }

			public string FAREF2 { get; set; }

			public static GT_WEB_IMP_MASS Create(DataRow row, ISapConnection sapConnection = null, IDynSapProxyFactory dynSapProxyFactory = null)
			{
				var o = new GT_WEB_IMP_MASS
				{
					FIN = (string)row["FIN"],
					ZZHERSTELLER_SCH = (string)row["ZZHERSTELLER_SCH"],
					ZZTYP_SCHL = (string)row["ZZTYP_SCHL"],
					ZZVVS_SCHLUESSEL = (string)row["ZZVVS_SCHLUESSEL"],
					ZZTYP_VVS_PRUEF = (string)row["ZZTYP_VVS_PRUEF"],
					KENNZ = (string)row["KENNZ"],
					ERSTZULDAT = string.IsNullOrEmpty(row["ERSTZULDAT"].ToString()) ? null : (DateTime?)row["ERSTZULDAT"],
					AKTZULDAT = string.IsNullOrEmpty(row["AKTZULDAT"].ToString()) ? null : (DateTime?)row["AKTZULDAT"],
					ABMDAT = string.IsNullOrEmpty(row["ABMDAT"].ToString()) ? null : (DateTime?)row["ABMDAT"],
					FZGART = (string)row["FZGART"],
					VKSPARTE = (string)row["VKSPARTE"],
					FZGNR = (string)row["FZGNR"],
					AUFNR = (string)row["AUFNR"],
					FAREF1 = (string)row["FAREF1"],
					FAREF2 = (string)row["FAREF2"],

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

			public static IEnumerable<GT_WEB_IMP_MASS> Select(DataTable dt, ISapConnection sapConnection = null)
			{
				return dt.AsEnumerable().Select(r => Create(r, sapConnection));
			}

			public static List<GT_WEB_IMP_MASS> ToList(DataTable dt, ISapConnection sapConnection = null)
			{
				return Select(dt, sapConnection).ToListOrEmptyList();
			}

			public static IEnumerable<GT_WEB_IMP_MASS> Select(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				var tbl = dts.FirstOrDefault(t => t.TableName.ToLower() == typeof(GT_WEB_IMP_MASS).Name.ToLower());
				if (tbl == null)
					return null;

				return Select(tbl, sapConnection);
			}

			public static List<GT_WEB_IMP_MASS> ToList(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				return Select(dts, sapConnection).ToListOrEmptyList();
			}

			public static List<GT_WEB_IMP_MASS> ToList(ISapDataService sapDataService)
			{
				return ToList(sapDataService.GetExportTables(), sapDataService.SapConnection);
			}

			public static List<GT_WEB_IMP_MASS> GetExportListWithInitExecute(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<GT_WEB_IMP_MASS>();
				 
				var dts = sapDataService.GetExportTablesWithInitExecute("Z_AHP_READ_TYPDAT_BESTAND", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_WEB_IMP_MASS> GetExportListWithExecute(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_WEB_IMP_MASS>();
				 
				var dts = sapDataService.GetExportTablesWithExecute();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_WEB_IMP_MASS> GetExportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_WEB_IMP_MASS>();
				 
				var dts = sapDataService.GetExportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_WEB_IMP_MASS> GetImportListWithInit(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<GT_WEB_IMP_MASS>();
				 
				var dts = sapDataService.GetImportTablesWithInit("Z_AHP_READ_TYPDAT_BESTAND", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_WEB_IMP_MASS> GetImportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_WEB_IMP_MASS>();
				 
				var dts = sapDataService.GetImportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}
		}

		public partial class GT_WEB_TYPDATEN : IModelMappingApplied
		{
			[SapIgnore]
			[ScriptIgnore]
			public ISapConnection SAPConnection { get; set; }

			[SapIgnore]
			[ScriptIgnore]
			public IDynSapProxyFactory DynSapProxyFactory { get; set; }

			public string FIN_ID { get; set; }

			public string FIN { get; set; }

			public DateTime? ERDAT { get; set; }

			public string ZZHERSTELLER_SCH { get; set; }

			public string ZZTYP_SCHL { get; set; }

			public string ZZVVS_SCHLUESSEL { get; set; }

			public string ZZTYP_VVS_PRUEF { get; set; }

			public string ZZFABRIKNAME { get; set; }

			public string ZZHANDELSNAME { get; set; }

			public static GT_WEB_TYPDATEN Create(DataRow row, ISapConnection sapConnection = null, IDynSapProxyFactory dynSapProxyFactory = null)
			{
				var o = new GT_WEB_TYPDATEN
				{
					FIN_ID = (string)row["FIN_ID"],
					FIN = (string)row["FIN"],
					ERDAT = string.IsNullOrEmpty(row["ERDAT"].ToString()) ? null : (DateTime?)row["ERDAT"],
					ZZHERSTELLER_SCH = (string)row["ZZHERSTELLER_SCH"],
					ZZTYP_SCHL = (string)row["ZZTYP_SCHL"],
					ZZVVS_SCHLUESSEL = (string)row["ZZVVS_SCHLUESSEL"],
					ZZTYP_VVS_PRUEF = (string)row["ZZTYP_VVS_PRUEF"],
					ZZFABRIKNAME = (string)row["ZZFABRIKNAME"],
					ZZHANDELSNAME = (string)row["ZZHANDELSNAME"],

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

			public static IEnumerable<GT_WEB_TYPDATEN> Select(DataTable dt, ISapConnection sapConnection = null)
			{
				return dt.AsEnumerable().Select(r => Create(r, sapConnection));
			}

			public static List<GT_WEB_TYPDATEN> ToList(DataTable dt, ISapConnection sapConnection = null)
			{
				return Select(dt, sapConnection).ToListOrEmptyList();
			}

			public static IEnumerable<GT_WEB_TYPDATEN> Select(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				var tbl = dts.FirstOrDefault(t => t.TableName.ToLower() == typeof(GT_WEB_TYPDATEN).Name.ToLower());
				if (tbl == null)
					return null;

				return Select(tbl, sapConnection);
			}

			public static List<GT_WEB_TYPDATEN> ToList(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				return Select(dts, sapConnection).ToListOrEmptyList();
			}

			public static List<GT_WEB_TYPDATEN> ToList(ISapDataService sapDataService)
			{
				return ToList(sapDataService.GetExportTables(), sapDataService.SapConnection);
			}

			public static List<GT_WEB_TYPDATEN> GetExportListWithInitExecute(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<GT_WEB_TYPDATEN>();
				 
				var dts = sapDataService.GetExportTablesWithInitExecute("Z_AHP_READ_TYPDAT_BESTAND", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_WEB_TYPDATEN> GetExportListWithExecute(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_WEB_TYPDATEN>();
				 
				var dts = sapDataService.GetExportTablesWithExecute();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_WEB_TYPDATEN> GetExportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_WEB_TYPDATEN>();
				 
				var dts = sapDataService.GetExportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_WEB_TYPDATEN> GetImportListWithInit(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<GT_WEB_TYPDATEN>();
				 
				var dts = sapDataService.GetImportTablesWithInit("Z_AHP_READ_TYPDAT_BESTAND", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_WEB_TYPDATEN> GetImportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_WEB_TYPDATEN>();
				 
				var dts = sapDataService.GetImportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}
		}
	}

	public static partial class DataTableExtensions
	{

		public static DataTable ToTable(this IEnumerable<Z_AHP_READ_TYPDAT_BESTAND.GT_WEB_BESTAND> list)
		{
			return SapDataServiceExtensions.ToTable(list);
		}


		public static DataTable ToTable(this IEnumerable<Z_AHP_READ_TYPDAT_BESTAND.GT_WEB_IMP_MASS> list)
		{
			return SapDataServiceExtensions.ToTable(list);
		}


		public static DataTable ToTable(this IEnumerable<Z_AHP_READ_TYPDAT_BESTAND.GT_WEB_TYPDATEN> list)
		{
			return SapDataServiceExtensions.ToTable(list);
		}

	}
}
