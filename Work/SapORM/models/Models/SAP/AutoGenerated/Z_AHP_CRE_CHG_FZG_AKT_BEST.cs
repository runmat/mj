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
	public partial class Z_AHP_CRE_CHG_FZG_AKT_BEST
	{
		public static void Init(ISapDataService sap)
		{
			sap.Init(typeof(Z_AHP_CRE_CHG_FZG_AKT_BEST).Name);
		}

		public static void Init(ISapDataService sap, string inputParameterKeys, params object[] inputParameterValues)
		{
			sap.Init(typeof(Z_AHP_CRE_CHG_FZG_AKT_BEST).Name, inputParameterKeys, inputParameterValues);
		}


		public static void SetImportParameter_I_KUNNR(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_KUNNR", value);
		}

		public static void SetImportParameter_I_USER(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_USER", value);
		}

		public partial class GT_OUT_ERR : IModelMappingApplied
		{
			[SapIgnore]
			[ScriptIgnore]
			public ISapConnection SAPConnection { get; set; }

			[SapIgnore]
			[ScriptIgnore]
			public IDynSapProxyFactory DynSapProxyFactory { get; set; }

			public string FIN_ID { get; set; }

			public string FIN { get; set; }

			public string KUNDENREFERENZ { get; set; }

			public string ZZHERSTELLER_SCH { get; set; }

			public string ZZTYP_SCHL { get; set; }

			public string ZZVVS_SCHLUESSEL { get; set; }

			public string ZZTYP_VVS_PRUEF { get; set; }

			public string ZZFABRIKNAME { get; set; }

			public string ZZHANDELSNAME { get; set; }

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

			public string LOEVM { get; set; }

			private bool MappingErrorProcessed { get; set; }

			public static GT_OUT_ERR Create(DataRow row, ISapConnection sapConnection = null, IDynSapProxyFactory dynSapProxyFactory = null)
			{
				GT_OUT_ERR o;

				try
				{
					o = new GT_OUT_ERR
					{
						SAPConnection = sapConnection,
						DynSapProxyFactory = dynSapProxyFactory,

						FIN_ID = (string)row["FIN_ID"],
						FIN = (string)row["FIN"],
						KUNDENREFERENZ = (string)row["KUNDENREFERENZ"],
						ZZHERSTELLER_SCH = (string)row["ZZHERSTELLER_SCH"],
						ZZTYP_SCHL = (string)row["ZZTYP_SCHL"],
						ZZVVS_SCHLUESSEL = (string)row["ZZVVS_SCHLUESSEL"],
						ZZTYP_VVS_PRUEF = (string)row["ZZTYP_VVS_PRUEF"],
						ZZFABRIKNAME = (string)row["ZZFABRIKNAME"],
						ZZHANDELSNAME = (string)row["ZZHANDELSNAME"],
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
						LOEVM = (string)row["LOEVM"],
					};
				}
				catch(Exception e)
				{
					o = new GT_OUT_ERR
					{
						SAPConnection = sapConnection,
						DynSapProxyFactory = dynSapProxyFactory,
					};
					o.OnMappingError(e, row, true);
					if (!o.MappingErrorProcessed)
						throw;
				}

				o.OnInitFromSap();
				return o;
			}

			partial void OnInitFromSap();

			partial void OnMappingError(Exception e, DataRow row, bool isExport);

			partial void OnInitFromExtern();

			public void OnModelMappingApplied()
			{
				OnInitFromExtern();
			}

			public static IEnumerable<GT_OUT_ERR> Select(DataTable dt, ISapConnection sapConnection = null)
			{
				return dt.AsEnumerable().Select(r => Create(r, sapConnection));
			}

			public static List<GT_OUT_ERR> ToList(DataTable dt, ISapConnection sapConnection = null)
			{
				return Select(dt, sapConnection).ToListOrEmptyList();
			}

			public static IEnumerable<GT_OUT_ERR> Select(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				var tbl = dts.FirstOrDefault(t => t.TableName.ToLower() == typeof(GT_OUT_ERR).Name.ToLower());
				if (tbl == null)
					return null;

				return Select(tbl, sapConnection);
			}

			public static List<GT_OUT_ERR> ToList(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				return Select(dts, sapConnection).ToListOrEmptyList();
			}

			public static List<GT_OUT_ERR> ToList(ISapDataService sapDataService)
			{
				return ToList(sapDataService.GetExportTables(), sapDataService.SapConnection);
			}

			public static List<GT_OUT_ERR> GetExportListWithInitExecute(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<GT_OUT_ERR>();
				 
				var dts = sapDataService.GetExportTablesWithInitExecute("Z_AHP_CRE_CHG_FZG_AKT_BEST", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_OUT_ERR> GetExportListWithExecute(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_OUT_ERR>();
				 
				var dts = sapDataService.GetExportTablesWithExecute();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_OUT_ERR> GetExportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_OUT_ERR>();
				 
				var dts = sapDataService.GetExportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_OUT_ERR> GetImportListWithInit(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<GT_OUT_ERR>();
				 
				var dts = sapDataService.GetImportTablesWithInit("Z_AHP_CRE_CHG_FZG_AKT_BEST", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_OUT_ERR> GetImportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_OUT_ERR>();
				 
				var dts = sapDataService.GetImportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}
		}

		public partial class GT_WEB_IMP : IModelMappingApplied
		{
			[SapIgnore]
			[ScriptIgnore]
			public ISapConnection SAPConnection { get; set; }

			[SapIgnore]
			[ScriptIgnore]
			public IDynSapProxyFactory DynSapProxyFactory { get; set; }

			public string FIN_ID { get; set; }

			public string FIN { get; set; }

			public string KUNDENREFERENZ { get; set; }

			public string ZZHERSTELLER_SCH { get; set; }

			public string ZZTYP_SCHL { get; set; }

			public string ZZVVS_SCHLUESSEL { get; set; }

			public string ZZTYP_VVS_PRUEF { get; set; }

			public string ZZFABRIKNAME { get; set; }

			public string ZZHANDELSNAME { get; set; }

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

			public string LOEVM { get; set; }

			private bool MappingErrorProcessed { get; set; }

			public static GT_WEB_IMP Create(DataRow row, ISapConnection sapConnection = null, IDynSapProxyFactory dynSapProxyFactory = null)
			{
				GT_WEB_IMP o;

				try
				{
					o = new GT_WEB_IMP
					{
						SAPConnection = sapConnection,
						DynSapProxyFactory = dynSapProxyFactory,

						FIN_ID = (string)row["FIN_ID"],
						FIN = (string)row["FIN"],
						KUNDENREFERENZ = (string)row["KUNDENREFERENZ"],
						ZZHERSTELLER_SCH = (string)row["ZZHERSTELLER_SCH"],
						ZZTYP_SCHL = (string)row["ZZTYP_SCHL"],
						ZZVVS_SCHLUESSEL = (string)row["ZZVVS_SCHLUESSEL"],
						ZZTYP_VVS_PRUEF = (string)row["ZZTYP_VVS_PRUEF"],
						ZZFABRIKNAME = (string)row["ZZFABRIKNAME"],
						ZZHANDELSNAME = (string)row["ZZHANDELSNAME"],
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
						LOEVM = (string)row["LOEVM"],
					};
				}
				catch(Exception e)
				{
					o = new GT_WEB_IMP
					{
						SAPConnection = sapConnection,
						DynSapProxyFactory = dynSapProxyFactory,
					};
					o.OnMappingError(e, row, true);
					if (!o.MappingErrorProcessed)
						throw;
				}

				o.OnInitFromSap();
				return o;
			}

			partial void OnInitFromSap();

			partial void OnMappingError(Exception e, DataRow row, bool isExport);

			partial void OnInitFromExtern();

			public void OnModelMappingApplied()
			{
				OnInitFromExtern();
			}

			public static IEnumerable<GT_WEB_IMP> Select(DataTable dt, ISapConnection sapConnection = null)
			{
				return dt.AsEnumerable().Select(r => Create(r, sapConnection));
			}

			public static List<GT_WEB_IMP> ToList(DataTable dt, ISapConnection sapConnection = null)
			{
				return Select(dt, sapConnection).ToListOrEmptyList();
			}

			public static IEnumerable<GT_WEB_IMP> Select(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				var tbl = dts.FirstOrDefault(t => t.TableName.ToLower() == typeof(GT_WEB_IMP).Name.ToLower());
				if (tbl == null)
					return null;

				return Select(tbl, sapConnection);
			}

			public static List<GT_WEB_IMP> ToList(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				return Select(dts, sapConnection).ToListOrEmptyList();
			}

			public static List<GT_WEB_IMP> ToList(ISapDataService sapDataService)
			{
				return ToList(sapDataService.GetExportTables(), sapDataService.SapConnection);
			}

			public static List<GT_WEB_IMP> GetExportListWithInitExecute(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<GT_WEB_IMP>();
				 
				var dts = sapDataService.GetExportTablesWithInitExecute("Z_AHP_CRE_CHG_FZG_AKT_BEST", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_WEB_IMP> GetExportListWithExecute(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_WEB_IMP>();
				 
				var dts = sapDataService.GetExportTablesWithExecute();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_WEB_IMP> GetExportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_WEB_IMP>();
				 
				var dts = sapDataService.GetExportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_WEB_IMP> GetImportListWithInit(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<GT_WEB_IMP>();
				 
				var dts = sapDataService.GetImportTablesWithInit("Z_AHP_CRE_CHG_FZG_AKT_BEST", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_WEB_IMP> GetImportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_WEB_IMP>();
				 
				var dts = sapDataService.GetImportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}
		}

		public partial class GT_WEB_OUT : IModelMappingApplied
		{
			[SapIgnore]
			[ScriptIgnore]
			public ISapConnection SAPConnection { get; set; }

			[SapIgnore]
			[ScriptIgnore]
			public IDynSapProxyFactory DynSapProxyFactory { get; set; }

			public string FIN_ID { get; set; }

			public string FIN { get; set; }

			public string KUNDENREFERENZ { get; set; }

			public string ZZHERSTELLER_SCH { get; set; }

			public string ZZTYP_SCHL { get; set; }

			public string ZZVVS_SCHLUESSEL { get; set; }

			public string ZZTYP_VVS_PRUEF { get; set; }

			public string ZZFABRIKNAME { get; set; }

			public string ZZHANDELSNAME { get; set; }

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

			public string LOEVM { get; set; }

			private bool MappingErrorProcessed { get; set; }

			public static GT_WEB_OUT Create(DataRow row, ISapConnection sapConnection = null, IDynSapProxyFactory dynSapProxyFactory = null)
			{
				GT_WEB_OUT o;

				try
				{
					o = new GT_WEB_OUT
					{
						SAPConnection = sapConnection,
						DynSapProxyFactory = dynSapProxyFactory,

						FIN_ID = (string)row["FIN_ID"],
						FIN = (string)row["FIN"],
						KUNDENREFERENZ = (string)row["KUNDENREFERENZ"],
						ZZHERSTELLER_SCH = (string)row["ZZHERSTELLER_SCH"],
						ZZTYP_SCHL = (string)row["ZZTYP_SCHL"],
						ZZVVS_SCHLUESSEL = (string)row["ZZVVS_SCHLUESSEL"],
						ZZTYP_VVS_PRUEF = (string)row["ZZTYP_VVS_PRUEF"],
						ZZFABRIKNAME = (string)row["ZZFABRIKNAME"],
						ZZHANDELSNAME = (string)row["ZZHANDELSNAME"],
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
						LOEVM = (string)row["LOEVM"],
					};
				}
				catch(Exception e)
				{
					o = new GT_WEB_OUT
					{
						SAPConnection = sapConnection,
						DynSapProxyFactory = dynSapProxyFactory,
					};
					o.OnMappingError(e, row, true);
					if (!o.MappingErrorProcessed)
						throw;
				}

				o.OnInitFromSap();
				return o;
			}

			partial void OnInitFromSap();

			partial void OnMappingError(Exception e, DataRow row, bool isExport);

			partial void OnInitFromExtern();

			public void OnModelMappingApplied()
			{
				OnInitFromExtern();
			}

			public static IEnumerable<GT_WEB_OUT> Select(DataTable dt, ISapConnection sapConnection = null)
			{
				return dt.AsEnumerable().Select(r => Create(r, sapConnection));
			}

			public static List<GT_WEB_OUT> ToList(DataTable dt, ISapConnection sapConnection = null)
			{
				return Select(dt, sapConnection).ToListOrEmptyList();
			}

			public static IEnumerable<GT_WEB_OUT> Select(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				var tbl = dts.FirstOrDefault(t => t.TableName.ToLower() == typeof(GT_WEB_OUT).Name.ToLower());
				if (tbl == null)
					return null;

				return Select(tbl, sapConnection);
			}

			public static List<GT_WEB_OUT> ToList(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				return Select(dts, sapConnection).ToListOrEmptyList();
			}

			public static List<GT_WEB_OUT> ToList(ISapDataService sapDataService)
			{
				return ToList(sapDataService.GetExportTables(), sapDataService.SapConnection);
			}

			public static List<GT_WEB_OUT> GetExportListWithInitExecute(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<GT_WEB_OUT>();
				 
				var dts = sapDataService.GetExportTablesWithInitExecute("Z_AHP_CRE_CHG_FZG_AKT_BEST", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_WEB_OUT> GetExportListWithExecute(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_WEB_OUT>();
				 
				var dts = sapDataService.GetExportTablesWithExecute();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_WEB_OUT> GetExportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_WEB_OUT>();
				 
				var dts = sapDataService.GetExportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_WEB_OUT> GetImportListWithInit(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<GT_WEB_OUT>();
				 
				var dts = sapDataService.GetImportTablesWithInit("Z_AHP_CRE_CHG_FZG_AKT_BEST", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_WEB_OUT> GetImportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_WEB_OUT>();
				 
				var dts = sapDataService.GetImportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}
		}
	}

	public static partial class DataTableExtensions
	{

		public static DataTable ToTable(this IEnumerable<Z_AHP_CRE_CHG_FZG_AKT_BEST.GT_OUT_ERR> list)
		{
			return SapDataServiceExtensions.ToTable(list);
		}


		public static DataTable ToTable(this IEnumerable<Z_AHP_CRE_CHG_FZG_AKT_BEST.GT_WEB_IMP> list)
		{
			return SapDataServiceExtensions.ToTable(list);
		}


		public static DataTable ToTable(this IEnumerable<Z_AHP_CRE_CHG_FZG_AKT_BEST.GT_WEB_OUT> list)
		{
			return SapDataServiceExtensions.ToTable(list);
		}

	}
}
