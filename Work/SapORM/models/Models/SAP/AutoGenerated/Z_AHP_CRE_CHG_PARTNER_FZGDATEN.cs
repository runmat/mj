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
	public partial class Z_AHP_CRE_CHG_PARTNER_FZGDATEN
	{
		public static void Init(ISapDataService sap)
		{
			sap.Init(typeof(Z_AHP_CRE_CHG_PARTNER_FZGDATEN).Name);
		}

		public static void Init(ISapDataService sap, string inputParameterKeys, params object[] inputParameterValues)
		{
			sap.Init(typeof(Z_AHP_CRE_CHG_PARTNER_FZGDATEN).Name, inputParameterKeys, inputParameterValues);
		}


		public static void SetImportParameter_I_KUNNR(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_KUNNR", value);
		}

		public static void SetImportParameter_I_USER(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_USER", value);
		}

		public static string GetExportParameter_E_MESSAGE(ISapDataService sap)
		{
			return sap.GetExportParameter<string>("E_MESSAGE").NotNullOrEmpty().Trim();
		}

		public static int? GetExportParameter_E_SUBRC(ISapDataService sap)
		{
			return sap.GetExportParameter<int?>("E_SUBRC");
		}

		public partial class GT_WEB_FZG_ERR : IModelMappingApplied
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

			public static GT_WEB_FZG_ERR Create(DataRow row, ISapConnection sapConnection = null, IDynSapProxyFactory dynSapProxyFactory = null)
			{
				GT_WEB_FZG_ERR o;

				try
				{
					o = new GT_WEB_FZG_ERR
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
					o = new GT_WEB_FZG_ERR
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

			public static IEnumerable<GT_WEB_FZG_ERR> Select(DataTable dt, ISapConnection sapConnection = null)
			{
				return dt.AsEnumerable().Select(r => Create(r, sapConnection));
			}

			public static List<GT_WEB_FZG_ERR> ToList(DataTable dt, ISapConnection sapConnection = null)
			{
				return Select(dt, sapConnection).ToListOrEmptyList();
			}

			public static IEnumerable<GT_WEB_FZG_ERR> Select(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				var tbl = dts.FirstOrDefault(t => t.TableName.ToLower() == typeof(GT_WEB_FZG_ERR).Name.ToLower());
				if (tbl == null)
					return null;

				return Select(tbl, sapConnection);
			}

			public static List<GT_WEB_FZG_ERR> ToList(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				return Select(dts, sapConnection).ToListOrEmptyList();
			}

			public static List<GT_WEB_FZG_ERR> ToList(ISapDataService sapDataService)
			{
				return ToList(sapDataService.GetExportTables(), sapDataService.SapConnection);
			}

			public static List<GT_WEB_FZG_ERR> GetExportListWithInitExecute(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<GT_WEB_FZG_ERR>();
				 
				var dts = sapDataService.GetExportTablesWithInitExecute("Z_AHP_CRE_CHG_PARTNER_FZGDATEN", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_WEB_FZG_ERR> GetExportListWithExecute(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_WEB_FZG_ERR>();
				 
				var dts = sapDataService.GetExportTablesWithExecute();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_WEB_FZG_ERR> GetExportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_WEB_FZG_ERR>();
				 
				var dts = sapDataService.GetExportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_WEB_FZG_ERR> GetImportListWithInit(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<GT_WEB_FZG_ERR>();
				 
				var dts = sapDataService.GetImportTablesWithInit("Z_AHP_CRE_CHG_PARTNER_FZGDATEN", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_WEB_FZG_ERR> GetImportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_WEB_FZG_ERR>();
				 
				var dts = sapDataService.GetImportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}
		}

		public partial class GT_WEB_FZG_IMP : IModelMappingApplied
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

			public static GT_WEB_FZG_IMP Create(DataRow row, ISapConnection sapConnection = null, IDynSapProxyFactory dynSapProxyFactory = null)
			{
				GT_WEB_FZG_IMP o;

				try
				{
					o = new GT_WEB_FZG_IMP
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
					o = new GT_WEB_FZG_IMP
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

			public static IEnumerable<GT_WEB_FZG_IMP> Select(DataTable dt, ISapConnection sapConnection = null)
			{
				return dt.AsEnumerable().Select(r => Create(r, sapConnection));
			}

			public static List<GT_WEB_FZG_IMP> ToList(DataTable dt, ISapConnection sapConnection = null)
			{
				return Select(dt, sapConnection).ToListOrEmptyList();
			}

			public static IEnumerable<GT_WEB_FZG_IMP> Select(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				var tbl = dts.FirstOrDefault(t => t.TableName.ToLower() == typeof(GT_WEB_FZG_IMP).Name.ToLower());
				if (tbl == null)
					return null;

				return Select(tbl, sapConnection);
			}

			public static List<GT_WEB_FZG_IMP> ToList(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				return Select(dts, sapConnection).ToListOrEmptyList();
			}

			public static List<GT_WEB_FZG_IMP> ToList(ISapDataService sapDataService)
			{
				return ToList(sapDataService.GetExportTables(), sapDataService.SapConnection);
			}

			public static List<GT_WEB_FZG_IMP> GetExportListWithInitExecute(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<GT_WEB_FZG_IMP>();
				 
				var dts = sapDataService.GetExportTablesWithInitExecute("Z_AHP_CRE_CHG_PARTNER_FZGDATEN", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_WEB_FZG_IMP> GetExportListWithExecute(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_WEB_FZG_IMP>();
				 
				var dts = sapDataService.GetExportTablesWithExecute();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_WEB_FZG_IMP> GetExportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_WEB_FZG_IMP>();
				 
				var dts = sapDataService.GetExportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_WEB_FZG_IMP> GetImportListWithInit(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<GT_WEB_FZG_IMP>();
				 
				var dts = sapDataService.GetImportTablesWithInit("Z_AHP_CRE_CHG_PARTNER_FZGDATEN", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_WEB_FZG_IMP> GetImportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_WEB_FZG_IMP>();
				 
				var dts = sapDataService.GetImportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}
		}

		public partial class GT_WEB_FZG_OUT : IModelMappingApplied
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

			public static GT_WEB_FZG_OUT Create(DataRow row, ISapConnection sapConnection = null, IDynSapProxyFactory dynSapProxyFactory = null)
			{
				GT_WEB_FZG_OUT o;

				try
				{
					o = new GT_WEB_FZG_OUT
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
					o = new GT_WEB_FZG_OUT
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

			public static IEnumerable<GT_WEB_FZG_OUT> Select(DataTable dt, ISapConnection sapConnection = null)
			{
				return dt.AsEnumerable().Select(r => Create(r, sapConnection));
			}

			public static List<GT_WEB_FZG_OUT> ToList(DataTable dt, ISapConnection sapConnection = null)
			{
				return Select(dt, sapConnection).ToListOrEmptyList();
			}

			public static IEnumerable<GT_WEB_FZG_OUT> Select(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				var tbl = dts.FirstOrDefault(t => t.TableName.ToLower() == typeof(GT_WEB_FZG_OUT).Name.ToLower());
				if (tbl == null)
					return null;

				return Select(tbl, sapConnection);
			}

			public static List<GT_WEB_FZG_OUT> ToList(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				return Select(dts, sapConnection).ToListOrEmptyList();
			}

			public static List<GT_WEB_FZG_OUT> ToList(ISapDataService sapDataService)
			{
				return ToList(sapDataService.GetExportTables(), sapDataService.SapConnection);
			}

			public static List<GT_WEB_FZG_OUT> GetExportListWithInitExecute(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<GT_WEB_FZG_OUT>();
				 
				var dts = sapDataService.GetExportTablesWithInitExecute("Z_AHP_CRE_CHG_PARTNER_FZGDATEN", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_WEB_FZG_OUT> GetExportListWithExecute(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_WEB_FZG_OUT>();
				 
				var dts = sapDataService.GetExportTablesWithExecute();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_WEB_FZG_OUT> GetExportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_WEB_FZG_OUT>();
				 
				var dts = sapDataService.GetExportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_WEB_FZG_OUT> GetImportListWithInit(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<GT_WEB_FZG_OUT>();
				 
				var dts = sapDataService.GetImportTablesWithInit("Z_AHP_CRE_CHG_PARTNER_FZGDATEN", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_WEB_FZG_OUT> GetImportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_WEB_FZG_OUT>();
				 
				var dts = sapDataService.GetImportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}
		}

		public partial class GT_WEB_PARTNER_ERR : IModelMappingApplied
		{
			[SapIgnore]
			[ScriptIgnore]
			public ISapConnection SAPConnection { get; set; }

			[SapIgnore]
			[ScriptIgnore]
			public IDynSapProxyFactory DynSapProxyFactory { get; set; }

			public string KUNNR { get; set; }

			public string PARTART { get; set; }

			public string NAME1 { get; set; }

			public string NAME2 { get; set; }

			public string STRASSE { get; set; }

			public string HAUSNR { get; set; }

			public string PLZNR { get; set; }

			public string ORT { get; set; }

			public string LAND { get; set; }

			public string EMAIL { get; set; }

			public string TELEFON { get; set; }

			public string FAX { get; set; }

			public string BEMERKUNG { get; set; }

			public string GEWERBE { get; set; }

			public string SAVEKDDATEN { get; set; }

			public string REFKUNNR { get; set; }

			public string REFKUNNR2 { get; set; }

			public string EVBNR { get; set; }

			public DateTime? SEPA_STICHTAG { get; set; }

			public string IBAN { get; set; }

			private bool MappingErrorProcessed { get; set; }

			public static GT_WEB_PARTNER_ERR Create(DataRow row, ISapConnection sapConnection = null, IDynSapProxyFactory dynSapProxyFactory = null)
			{
				GT_WEB_PARTNER_ERR o;

				try
				{
					o = new GT_WEB_PARTNER_ERR
					{
						SAPConnection = sapConnection,
						DynSapProxyFactory = dynSapProxyFactory,

						KUNNR = (string)row["KUNNR"],
						PARTART = (string)row["PARTART"],
						NAME1 = (string)row["NAME1"],
						NAME2 = (string)row["NAME2"],
						STRASSE = (string)row["STRASSE"],
						HAUSNR = (string)row["HAUSNR"],
						PLZNR = (string)row["PLZNR"],
						ORT = (string)row["ORT"],
						LAND = (string)row["LAND"],
						EMAIL = (string)row["EMAIL"],
						TELEFON = (string)row["TELEFON"],
						FAX = (string)row["FAX"],
						BEMERKUNG = (string)row["BEMERKUNG"],
						GEWERBE = (string)row["GEWERBE"],
						SAVEKDDATEN = (string)row["SAVEKDDATEN"],
						REFKUNNR = (string)row["REFKUNNR"],
						REFKUNNR2 = (string)row["REFKUNNR2"],
						EVBNR = (string)row["EVBNR"],
						SEPA_STICHTAG = string.IsNullOrEmpty(row["SEPA_STICHTAG"].ToString()) ? null : (DateTime?)row["SEPA_STICHTAG"],
						IBAN = (string)row["IBAN"],
					};
				}
				catch(Exception e)
				{
					o = new GT_WEB_PARTNER_ERR
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

			public static IEnumerable<GT_WEB_PARTNER_ERR> Select(DataTable dt, ISapConnection sapConnection = null)
			{
				return dt.AsEnumerable().Select(r => Create(r, sapConnection));
			}

			public static List<GT_WEB_PARTNER_ERR> ToList(DataTable dt, ISapConnection sapConnection = null)
			{
				return Select(dt, sapConnection).ToListOrEmptyList();
			}

			public static IEnumerable<GT_WEB_PARTNER_ERR> Select(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				var tbl = dts.FirstOrDefault(t => t.TableName.ToLower() == typeof(GT_WEB_PARTNER_ERR).Name.ToLower());
				if (tbl == null)
					return null;

				return Select(tbl, sapConnection);
			}

			public static List<GT_WEB_PARTNER_ERR> ToList(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				return Select(dts, sapConnection).ToListOrEmptyList();
			}

			public static List<GT_WEB_PARTNER_ERR> ToList(ISapDataService sapDataService)
			{
				return ToList(sapDataService.GetExportTables(), sapDataService.SapConnection);
			}

			public static List<GT_WEB_PARTNER_ERR> GetExportListWithInitExecute(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<GT_WEB_PARTNER_ERR>();
				 
				var dts = sapDataService.GetExportTablesWithInitExecute("Z_AHP_CRE_CHG_PARTNER_FZGDATEN", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_WEB_PARTNER_ERR> GetExportListWithExecute(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_WEB_PARTNER_ERR>();
				 
				var dts = sapDataService.GetExportTablesWithExecute();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_WEB_PARTNER_ERR> GetExportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_WEB_PARTNER_ERR>();
				 
				var dts = sapDataService.GetExportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_WEB_PARTNER_ERR> GetImportListWithInit(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<GT_WEB_PARTNER_ERR>();
				 
				var dts = sapDataService.GetImportTablesWithInit("Z_AHP_CRE_CHG_PARTNER_FZGDATEN", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_WEB_PARTNER_ERR> GetImportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_WEB_PARTNER_ERR>();
				 
				var dts = sapDataService.GetImportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}
		}

		public partial class GT_WEB_PARTNER_IMP : IModelMappingApplied
		{
			[SapIgnore]
			[ScriptIgnore]
			public ISapConnection SAPConnection { get; set; }

			[SapIgnore]
			[ScriptIgnore]
			public IDynSapProxyFactory DynSapProxyFactory { get; set; }

			public string KUNNR { get; set; }

			public string PARTART { get; set; }

			public string NAME1 { get; set; }

			public string NAME2 { get; set; }

			public string STRASSE { get; set; }

			public string HAUSNR { get; set; }

			public string PLZNR { get; set; }

			public string ORT { get; set; }

			public string LAND { get; set; }

			public string EMAIL { get; set; }

			public string TELEFON { get; set; }

			public string FAX { get; set; }

			public string BEMERKUNG { get; set; }

			public string GEWERBE { get; set; }

			public string SAVEKDDATEN { get; set; }

			public string REFKUNNR { get; set; }

			public string REFKUNNR2 { get; set; }

			public string EVBNR { get; set; }

			public DateTime? SEPA_STICHTAG { get; set; }

			public string IBAN { get; set; }

			private bool MappingErrorProcessed { get; set; }

			public static GT_WEB_PARTNER_IMP Create(DataRow row, ISapConnection sapConnection = null, IDynSapProxyFactory dynSapProxyFactory = null)
			{
				GT_WEB_PARTNER_IMP o;

				try
				{
					o = new GT_WEB_PARTNER_IMP
					{
						SAPConnection = sapConnection,
						DynSapProxyFactory = dynSapProxyFactory,

						KUNNR = (string)row["KUNNR"],
						PARTART = (string)row["PARTART"],
						NAME1 = (string)row["NAME1"],
						NAME2 = (string)row["NAME2"],
						STRASSE = (string)row["STRASSE"],
						HAUSNR = (string)row["HAUSNR"],
						PLZNR = (string)row["PLZNR"],
						ORT = (string)row["ORT"],
						LAND = (string)row["LAND"],
						EMAIL = (string)row["EMAIL"],
						TELEFON = (string)row["TELEFON"],
						FAX = (string)row["FAX"],
						BEMERKUNG = (string)row["BEMERKUNG"],
						GEWERBE = (string)row["GEWERBE"],
						SAVEKDDATEN = (string)row["SAVEKDDATEN"],
						REFKUNNR = (string)row["REFKUNNR"],
						REFKUNNR2 = (string)row["REFKUNNR2"],
						EVBNR = (string)row["EVBNR"],
						SEPA_STICHTAG = string.IsNullOrEmpty(row["SEPA_STICHTAG"].ToString()) ? null : (DateTime?)row["SEPA_STICHTAG"],
						IBAN = (string)row["IBAN"],
					};
				}
				catch(Exception e)
				{
					o = new GT_WEB_PARTNER_IMP
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

			public static IEnumerable<GT_WEB_PARTNER_IMP> Select(DataTable dt, ISapConnection sapConnection = null)
			{
				return dt.AsEnumerable().Select(r => Create(r, sapConnection));
			}

			public static List<GT_WEB_PARTNER_IMP> ToList(DataTable dt, ISapConnection sapConnection = null)
			{
				return Select(dt, sapConnection).ToListOrEmptyList();
			}

			public static IEnumerable<GT_WEB_PARTNER_IMP> Select(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				var tbl = dts.FirstOrDefault(t => t.TableName.ToLower() == typeof(GT_WEB_PARTNER_IMP).Name.ToLower());
				if (tbl == null)
					return null;

				return Select(tbl, sapConnection);
			}

			public static List<GT_WEB_PARTNER_IMP> ToList(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				return Select(dts, sapConnection).ToListOrEmptyList();
			}

			public static List<GT_WEB_PARTNER_IMP> ToList(ISapDataService sapDataService)
			{
				return ToList(sapDataService.GetExportTables(), sapDataService.SapConnection);
			}

			public static List<GT_WEB_PARTNER_IMP> GetExportListWithInitExecute(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<GT_WEB_PARTNER_IMP>();
				 
				var dts = sapDataService.GetExportTablesWithInitExecute("Z_AHP_CRE_CHG_PARTNER_FZGDATEN", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_WEB_PARTNER_IMP> GetExportListWithExecute(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_WEB_PARTNER_IMP>();
				 
				var dts = sapDataService.GetExportTablesWithExecute();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_WEB_PARTNER_IMP> GetExportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_WEB_PARTNER_IMP>();
				 
				var dts = sapDataService.GetExportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_WEB_PARTNER_IMP> GetImportListWithInit(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<GT_WEB_PARTNER_IMP>();
				 
				var dts = sapDataService.GetImportTablesWithInit("Z_AHP_CRE_CHG_PARTNER_FZGDATEN", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_WEB_PARTNER_IMP> GetImportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_WEB_PARTNER_IMP>();
				 
				var dts = sapDataService.GetImportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}
		}

		public partial class GT_WEB_PARTNER_OUT : IModelMappingApplied
		{
			[SapIgnore]
			[ScriptIgnore]
			public ISapConnection SAPConnection { get; set; }

			[SapIgnore]
			[ScriptIgnore]
			public IDynSapProxyFactory DynSapProxyFactory { get; set; }

			public string KUNNR { get; set; }

			public string PARTART { get; set; }

			public string NAME1 { get; set; }

			public string NAME2 { get; set; }

			public string STRASSE { get; set; }

			public string HAUSNR { get; set; }

			public string PLZNR { get; set; }

			public string ORT { get; set; }

			public string LAND { get; set; }

			public string EMAIL { get; set; }

			public string TELEFON { get; set; }

			public string FAX { get; set; }

			public string BEMERKUNG { get; set; }

			public string GEWERBE { get; set; }

			public string SAVEKDDATEN { get; set; }

			public string REFKUNNR { get; set; }

			public string REFKUNNR2 { get; set; }

			public string EVBNR { get; set; }

			public DateTime? SEPA_STICHTAG { get; set; }

			public string IBAN { get; set; }

			private bool MappingErrorProcessed { get; set; }

			public static GT_WEB_PARTNER_OUT Create(DataRow row, ISapConnection sapConnection = null, IDynSapProxyFactory dynSapProxyFactory = null)
			{
				GT_WEB_PARTNER_OUT o;

				try
				{
					o = new GT_WEB_PARTNER_OUT
					{
						SAPConnection = sapConnection,
						DynSapProxyFactory = dynSapProxyFactory,

						KUNNR = (string)row["KUNNR"],
						PARTART = (string)row["PARTART"],
						NAME1 = (string)row["NAME1"],
						NAME2 = (string)row["NAME2"],
						STRASSE = (string)row["STRASSE"],
						HAUSNR = (string)row["HAUSNR"],
						PLZNR = (string)row["PLZNR"],
						ORT = (string)row["ORT"],
						LAND = (string)row["LAND"],
						EMAIL = (string)row["EMAIL"],
						TELEFON = (string)row["TELEFON"],
						FAX = (string)row["FAX"],
						BEMERKUNG = (string)row["BEMERKUNG"],
						GEWERBE = (string)row["GEWERBE"],
						SAVEKDDATEN = (string)row["SAVEKDDATEN"],
						REFKUNNR = (string)row["REFKUNNR"],
						REFKUNNR2 = (string)row["REFKUNNR2"],
						EVBNR = (string)row["EVBNR"],
						SEPA_STICHTAG = string.IsNullOrEmpty(row["SEPA_STICHTAG"].ToString()) ? null : (DateTime?)row["SEPA_STICHTAG"],
						IBAN = (string)row["IBAN"],
					};
				}
				catch(Exception e)
				{
					o = new GT_WEB_PARTNER_OUT
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

			public static IEnumerable<GT_WEB_PARTNER_OUT> Select(DataTable dt, ISapConnection sapConnection = null)
			{
				return dt.AsEnumerable().Select(r => Create(r, sapConnection));
			}

			public static List<GT_WEB_PARTNER_OUT> ToList(DataTable dt, ISapConnection sapConnection = null)
			{
				return Select(dt, sapConnection).ToListOrEmptyList();
			}

			public static IEnumerable<GT_WEB_PARTNER_OUT> Select(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				var tbl = dts.FirstOrDefault(t => t.TableName.ToLower() == typeof(GT_WEB_PARTNER_OUT).Name.ToLower());
				if (tbl == null)
					return null;

				return Select(tbl, sapConnection);
			}

			public static List<GT_WEB_PARTNER_OUT> ToList(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				return Select(dts, sapConnection).ToListOrEmptyList();
			}

			public static List<GT_WEB_PARTNER_OUT> ToList(ISapDataService sapDataService)
			{
				return ToList(sapDataService.GetExportTables(), sapDataService.SapConnection);
			}

			public static List<GT_WEB_PARTNER_OUT> GetExportListWithInitExecute(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<GT_WEB_PARTNER_OUT>();
				 
				var dts = sapDataService.GetExportTablesWithInitExecute("Z_AHP_CRE_CHG_PARTNER_FZGDATEN", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_WEB_PARTNER_OUT> GetExportListWithExecute(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_WEB_PARTNER_OUT>();
				 
				var dts = sapDataService.GetExportTablesWithExecute();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_WEB_PARTNER_OUT> GetExportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_WEB_PARTNER_OUT>();
				 
				var dts = sapDataService.GetExportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_WEB_PARTNER_OUT> GetImportListWithInit(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<GT_WEB_PARTNER_OUT>();
				 
				var dts = sapDataService.GetImportTablesWithInit("Z_AHP_CRE_CHG_PARTNER_FZGDATEN", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_WEB_PARTNER_OUT> GetImportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_WEB_PARTNER_OUT>();
				 
				var dts = sapDataService.GetImportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}
		}
	}

	public static partial class DataTableExtensions
	{

		public static DataTable ToTable(this IEnumerable<Z_AHP_CRE_CHG_PARTNER_FZGDATEN.GT_WEB_FZG_ERR> list)
		{
			return SapDataServiceExtensions.ToTable(list);
		}


		public static DataTable ToTable(this IEnumerable<Z_AHP_CRE_CHG_PARTNER_FZGDATEN.GT_WEB_FZG_IMP> list)
		{
			return SapDataServiceExtensions.ToTable(list);
		}


		public static DataTable ToTable(this IEnumerable<Z_AHP_CRE_CHG_PARTNER_FZGDATEN.GT_WEB_FZG_OUT> list)
		{
			return SapDataServiceExtensions.ToTable(list);
		}


		public static DataTable ToTable(this IEnumerable<Z_AHP_CRE_CHG_PARTNER_FZGDATEN.GT_WEB_PARTNER_ERR> list)
		{
			return SapDataServiceExtensions.ToTable(list);
		}


		public static DataTable ToTable(this IEnumerable<Z_AHP_CRE_CHG_PARTNER_FZGDATEN.GT_WEB_PARTNER_IMP> list)
		{
			return SapDataServiceExtensions.ToTable(list);
		}


		public static DataTable ToTable(this IEnumerable<Z_AHP_CRE_CHG_PARTNER_FZGDATEN.GT_WEB_PARTNER_OUT> list)
		{
			return SapDataServiceExtensions.ToTable(list);
		}

	}
}
