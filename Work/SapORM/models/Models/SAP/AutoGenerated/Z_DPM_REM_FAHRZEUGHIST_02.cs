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
	public partial class Z_DPM_REM_FAHRZEUGHIST_02
	{
		public static void Init(ISapDataService sap)
		{
			sap.Init(typeof(Z_DPM_REM_FAHRZEUGHIST_02).Name);
		}

		public static void Init(ISapDataService sap, string inputParameterKeys, params object[] inputParameterValues)
		{
			sap.Init(typeof(Z_DPM_REM_FAHRZEUGHIST_02).Name, inputParameterKeys, inputParameterValues);
		}


		public static void SetImportParameter_I_FAHRGNR(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_FAHRGNR", value);
		}

		public static void SetImportParameter_I_KENNZ(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_KENNZ", value);
		}

		public static void SetImportParameter_I_KUNNR_AG(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_KUNNR_AG", value);
		}

		public static string GetExportParameter_E_MESSAGE(ISapDataService sap)
		{
			return sap.GetExportParameter<string>("E_MESSAGE").NotNullOrEmpty().Trim();
		}

		public static int? GetExportParameter_E_SUBRC(ISapDataService sap)
		{
			return sap.GetExportParameter<int?>("E_SUBRC");
		}

		public partial class GT_ADDR_B : IModelMappingApplied
		{
			[SapIgnore]
			[ScriptIgnore]
			public ISapConnection SAPConnection { get; set; }

			[SapIgnore]
			[ScriptIgnore]
			public IDynSapProxyFactory DynSapProxyFactory { get; set; }

			public string ADDRTYP { get; set; }

			public string ADDRTYP_TEXT { get; set; }

			public string EX_KUNNR { get; set; }

			public string NAME1 { get; set; }

			public string NAME2 { get; set; }

			public string NAME3 { get; set; }

			public string STREET { get; set; }

			public string HOUSE_NUM1 { get; set; }

			public string POST_CODE1 { get; set; }

			public string CITY1 { get; set; }

			public string COUNTRY { get; set; }

			public string TEL_NUMBER { get; set; }

			public string FAX_NUMBER { get; set; }

			public string SMTP_ADDR { get; set; }

			public DateTime? ERDAT { get; set; }

			public DateTime? AEDAT { get; set; }

			private bool MappingErrorProcessed { get; set; }

			public static GT_ADDR_B Create(DataRow row, ISapConnection sapConnection = null, IDynSapProxyFactory dynSapProxyFactory = null)
			{
				GT_ADDR_B o;

				try
				{
					o = new GT_ADDR_B
					{
						SAPConnection = sapConnection,
						DynSapProxyFactory = dynSapProxyFactory,

						ADDRTYP = (string)row["ADDRTYP"],
						ADDRTYP_TEXT = (string)row["ADDRTYP_TEXT"],
						EX_KUNNR = (string)row["EX_KUNNR"],
						NAME1 = (string)row["NAME1"],
						NAME2 = (string)row["NAME2"],
						NAME3 = (string)row["NAME3"],
						STREET = (string)row["STREET"],
						HOUSE_NUM1 = (string)row["HOUSE_NUM1"],
						POST_CODE1 = (string)row["POST_CODE1"],
						CITY1 = (string)row["CITY1"],
						COUNTRY = (string)row["COUNTRY"],
						TEL_NUMBER = (string)row["TEL_NUMBER"],
						FAX_NUMBER = (string)row["FAX_NUMBER"],
						SMTP_ADDR = (string)row["SMTP_ADDR"],
						ERDAT = string.IsNullOrEmpty(row["ERDAT"].ToString()) ? null : (DateTime?)row["ERDAT"],
						AEDAT = string.IsNullOrEmpty(row["AEDAT"].ToString()) ? null : (DateTime?)row["AEDAT"],
					};
				}
				catch(Exception e)
				{
					o = new GT_ADDR_B
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

			public static IEnumerable<GT_ADDR_B> Select(DataTable dt, ISapConnection sapConnection = null)
			{
				return dt.AsEnumerable().Select(r => Create(r, sapConnection));
			}

			public static List<GT_ADDR_B> ToList(DataTable dt, ISapConnection sapConnection = null)
			{
				return Select(dt, sapConnection).ToListOrEmptyList();
			}

			public static IEnumerable<GT_ADDR_B> Select(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				var tbl = dts.FirstOrDefault(t => t.TableName.ToLower() == typeof(GT_ADDR_B).Name.ToLower());
				if (tbl == null)
					return null;

				return Select(tbl, sapConnection);
			}

			public static List<GT_ADDR_B> ToList(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				return Select(dts, sapConnection).ToListOrEmptyList();
			}

			public static List<GT_ADDR_B> ToList(ISapDataService sapDataService)
			{
				return ToList(sapDataService.GetExportTables(), sapDataService.SapConnection);
			}

			public static List<GT_ADDR_B> GetExportListWithInitExecute(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<GT_ADDR_B>();
				 
				var dts = sapDataService.GetExportTablesWithInitExecute("Z_DPM_REM_FAHRZEUGHIST_02", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_ADDR_B> GetExportListWithExecute(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_ADDR_B>();
				 
				var dts = sapDataService.GetExportTablesWithExecute();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_ADDR_B> GetExportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_ADDR_B>();
				 
				var dts = sapDataService.GetExportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_ADDR_B> GetImportListWithInit(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<GT_ADDR_B>();
				 
				var dts = sapDataService.GetImportTablesWithInit("Z_DPM_REM_FAHRZEUGHIST_02", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_ADDR_B> GetImportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_ADDR_B>();
				 
				var dts = sapDataService.GetImportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}
		}

		public partial class GT_AUSST : IModelMappingApplied
		{
			[SapIgnore]
			[ScriptIgnore]
			public ISapConnection SAPConnection { get; set; }

			[SapIgnore]
			[ScriptIgnore]
			public IDynSapProxyFactory DynSapProxyFactory { get; set; }

			public string FAHRGNR { get; set; }

			public string PRNR_TYP { get; set; }

			public string PACKIDENT { get; set; }

			public string BEZ_PRNR { get; set; }

			private bool MappingErrorProcessed { get; set; }

			public static GT_AUSST Create(DataRow row, ISapConnection sapConnection = null, IDynSapProxyFactory dynSapProxyFactory = null)
			{
				GT_AUSST o;

				try
				{
					o = new GT_AUSST
					{
						SAPConnection = sapConnection,
						DynSapProxyFactory = dynSapProxyFactory,

						FAHRGNR = (string)row["FAHRGNR"],
						PRNR_TYP = (string)row["PRNR_TYP"],
						PACKIDENT = (string)row["PACKIDENT"],
						BEZ_PRNR = (string)row["BEZ_PRNR"],
					};
				}
				catch(Exception e)
				{
					o = new GT_AUSST
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

			public static IEnumerable<GT_AUSST> Select(DataTable dt, ISapConnection sapConnection = null)
			{
				return dt.AsEnumerable().Select(r => Create(r, sapConnection));
			}

			public static List<GT_AUSST> ToList(DataTable dt, ISapConnection sapConnection = null)
			{
				return Select(dt, sapConnection).ToListOrEmptyList();
			}

			public static IEnumerable<GT_AUSST> Select(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				var tbl = dts.FirstOrDefault(t => t.TableName.ToLower() == typeof(GT_AUSST).Name.ToLower());
				if (tbl == null)
					return null;

				return Select(tbl, sapConnection);
			}

			public static List<GT_AUSST> ToList(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				return Select(dts, sapConnection).ToListOrEmptyList();
			}

			public static List<GT_AUSST> ToList(ISapDataService sapDataService)
			{
				return ToList(sapDataService.GetExportTables(), sapDataService.SapConnection);
			}

			public static List<GT_AUSST> GetExportListWithInitExecute(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<GT_AUSST>();
				 
				var dts = sapDataService.GetExportTablesWithInitExecute("Z_DPM_REM_FAHRZEUGHIST_02", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_AUSST> GetExportListWithExecute(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_AUSST>();
				 
				var dts = sapDataService.GetExportTablesWithExecute();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_AUSST> GetExportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_AUSST>();
				 
				var dts = sapDataService.GetExportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_AUSST> GetImportListWithInit(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<GT_AUSST>();
				 
				var dts = sapDataService.GetImportTablesWithInit("Z_DPM_REM_FAHRZEUGHIST_02", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_AUSST> GetImportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_AUSST>();
				 
				var dts = sapDataService.GetImportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}
		}

		public partial class GT_BELAS : IModelMappingApplied
		{
			[SapIgnore]
			[ScriptIgnore]
			public ISapConnection SAPConnection { get; set; }

			[SapIgnore]
			[ScriptIgnore]
			public IDynSapProxyFactory DynSapProxyFactory { get; set; }

			public string FIN { get; set; }

			public string LFDNR { get; set; }

			public DateTime? ERDAT { get; set; }

			public decimal? SUMME { get; set; }

			public string GUTA { get; set; }

			public string GUTAID { get; set; }

			public string KMSTAND { get; set; }

			public string STATU { get; set; }

			public string STATUS_TEXT { get; set; }

			public string RENNR { get; set; }

			public DateTime? REDAT { get; set; }

			public decimal? MINWERT_AV { get; set; }

			public string SACHB { get; set; }

			public string REKLM { get; set; }

			public DateTime? WIDDAT { get; set; }

			public DateTime? BLOCKDAT { get; set; }

			public string BLOCKTEXT { get; set; }

			public string BLOCKUSER { get; set; }

			private bool MappingErrorProcessed { get; set; }

			public static GT_BELAS Create(DataRow row, ISapConnection sapConnection = null, IDynSapProxyFactory dynSapProxyFactory = null)
			{
				GT_BELAS o;

				try
				{
					o = new GT_BELAS
					{
						SAPConnection = sapConnection,
						DynSapProxyFactory = dynSapProxyFactory,

						FIN = (string)row["FIN"],
						LFDNR = (string)row["LFDNR"],
						ERDAT = string.IsNullOrEmpty(row["ERDAT"].ToString()) ? null : (DateTime?)row["ERDAT"],
						SUMME = string.IsNullOrEmpty(row["SUMME"].ToString()) ? null : (decimal?)row["SUMME"],
						GUTA = (string)row["GUTA"],
						GUTAID = (string)row["GUTAID"],
						KMSTAND = (string)row["KMSTAND"],
						STATU = (string)row["STATU"],
						STATUS_TEXT = (string)row["STATUS_TEXT"],
						RENNR = (string)row["RENNR"],
						REDAT = string.IsNullOrEmpty(row["REDAT"].ToString()) ? null : (DateTime?)row["REDAT"],
						MINWERT_AV = string.IsNullOrEmpty(row["MINWERT_AV"].ToString()) ? null : (decimal?)row["MINWERT_AV"],
						SACHB = (string)row["SACHB"],
						REKLM = (string)row["REKLM"],
						WIDDAT = string.IsNullOrEmpty(row["WIDDAT"].ToString()) ? null : (DateTime?)row["WIDDAT"],
						BLOCKDAT = string.IsNullOrEmpty(row["BLOCKDAT"].ToString()) ? null : (DateTime?)row["BLOCKDAT"],
						BLOCKTEXT = (string)row["BLOCKTEXT"],
						BLOCKUSER = (string)row["BLOCKUSER"],
					};
				}
				catch(Exception e)
				{
					o = new GT_BELAS
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

			public static IEnumerable<GT_BELAS> Select(DataTable dt, ISapConnection sapConnection = null)
			{
				return dt.AsEnumerable().Select(r => Create(r, sapConnection));
			}

			public static List<GT_BELAS> ToList(DataTable dt, ISapConnection sapConnection = null)
			{
				return Select(dt, sapConnection).ToListOrEmptyList();
			}

			public static IEnumerable<GT_BELAS> Select(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				var tbl = dts.FirstOrDefault(t => t.TableName.ToLower() == typeof(GT_BELAS).Name.ToLower());
				if (tbl == null)
					return null;

				return Select(tbl, sapConnection);
			}

			public static List<GT_BELAS> ToList(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				return Select(dts, sapConnection).ToListOrEmptyList();
			}

			public static List<GT_BELAS> ToList(ISapDataService sapDataService)
			{
				return ToList(sapDataService.GetExportTables(), sapDataService.SapConnection);
			}

			public static List<GT_BELAS> GetExportListWithInitExecute(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<GT_BELAS>();
				 
				var dts = sapDataService.GetExportTablesWithInitExecute("Z_DPM_REM_FAHRZEUGHIST_02", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_BELAS> GetExportListWithExecute(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_BELAS>();
				 
				var dts = sapDataService.GetExportTablesWithExecute();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_BELAS> GetExportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_BELAS>();
				 
				var dts = sapDataService.GetExportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_BELAS> GetImportListWithInit(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<GT_BELAS>();
				 
				var dts = sapDataService.GetImportTablesWithInit("Z_DPM_REM_FAHRZEUGHIST_02", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_BELAS> GetImportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_BELAS>();
				 
				var dts = sapDataService.GetImportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}
		}

		public partial class GT_DATEN : IModelMappingApplied
		{
			[SapIgnore]
			[ScriptIgnore]
			public ISapConnection SAPConnection { get; set; }

			[SapIgnore]
			[ScriptIgnore]
			public IDynSapProxyFactory DynSapProxyFactory { get; set; }

			public string FAHRGNR { get; set; }

			public string MODELL { get; set; }

			public string FARBE { get; set; }

			public string INAUST { get; set; }

			public string PRNR { get; set; }

			public string INVENTAR { get; set; }

			public string AVNR { get; set; }

			public DateTime? AUSLDAT { get; set; }

			public DateTime? INDATUM { get; set; }

			public string INTIME { get; set; }

			public string EQUNR { get; set; }

			public string KENNZ { get; set; }

			public string BRIEFNR { get; set; }

			public DateTime? ZULDAT { get; set; }

			public DateTime? GENZULDAT { get; set; }

			public DateTime? VLMDAT { get; set; }

			public DateTime? HCEINGDAT { get; set; }

			public string HCORT { get; set; }

			public string KMSTAND { get; set; }

			public DateTime? STILLDAT { get; set; }

			public DateTime? EGZB2DAT { get; set; }

			public DateTime? EGZWSLDAT { get; set; }

			public DateTime? EGRCKDAT { get; set; }

			public DateTime? VSZB2DAT { get; set; }

			public DateTime? VSZWSLDAT { get; set; }

			public DateTime? VSRCKDAT { get; set; }

			public string EREIGNIS { get; set; }

			public string S_BETRAG { get; set; }

			public DateTime? UEBERM_RE { get; set; }

			public string FLAG_VS { get; set; }

			public string BETRAG_RE { get; set; }

			public string NUMMER_RE { get; set; }

			public DateTime? DATUM_RE { get; set; }

			public string BEM_1 { get; set; }

			public string BEM_2 { get; set; }

			public string EQUNR_T { get; set; }

			public DateTime? EGNCDDAT { get; set; }

			public DateTime? UESVM { get; set; }

			public DateTime? UEVERLUST { get; set; }

			public DateTime? SCHADMELDDAT { get; set; }

			public DateTime? MELDDATGUTA { get; set; }

			public DateTime? AEDAT { get; set; }

			public string AENAM { get; set; }

			public string ZZCOCKZ { get; set; }

			public DateTime? UEBMZLDAT { get; set; }

			public DateTime? IMPDAT_SCHAEDEN { get; set; }

			public string LIZNR { get; set; }

			public string EREIGNIS_TEXT { get; set; }

			public DateTime? DATSVM { get; set; }

			public string USERSVM { get; set; }

			public DateTime? DAT_VERT_WID { get; set; }

			public string ART_VERT_WID { get; set; }

			public DateTime? DAT_BELAST_FREI { get; set; }

			public DateTime? DAT_TUEV_BEAUF_RUECK { get; set; }

			public DateTime? DAT_TUEV_BEAUF { get; set; }

			public string USER_TUEV_BEAUF { get; set; }

			public DateTime? DAT_ABRECHNUNG { get; set; }

			public DateTime? RUECK_DAT { get; set; }

			private bool MappingErrorProcessed { get; set; }

			public static GT_DATEN Create(DataRow row, ISapConnection sapConnection = null, IDynSapProxyFactory dynSapProxyFactory = null)
			{
				GT_DATEN o;

				try
				{
					o = new GT_DATEN
					{
						SAPConnection = sapConnection,
						DynSapProxyFactory = dynSapProxyFactory,

						FAHRGNR = (string)row["FAHRGNR"],
						MODELL = (string)row["MODELL"],
						FARBE = (string)row["FARBE"],
						INAUST = (string)row["INAUST"],
						PRNR = (string)row["PRNR"],
						INVENTAR = (string)row["INVENTAR"],
						AVNR = (string)row["AVNR"],
						AUSLDAT = string.IsNullOrEmpty(row["AUSLDAT"].ToString()) ? null : (DateTime?)row["AUSLDAT"],
						INDATUM = string.IsNullOrEmpty(row["INDATUM"].ToString()) ? null : (DateTime?)row["INDATUM"],
						INTIME = (string)row["INTIME"],
						EQUNR = (string)row["EQUNR"],
						KENNZ = (string)row["KENNZ"],
						BRIEFNR = (string)row["BRIEFNR"],
						ZULDAT = string.IsNullOrEmpty(row["ZULDAT"].ToString()) ? null : (DateTime?)row["ZULDAT"],
						GENZULDAT = string.IsNullOrEmpty(row["GENZULDAT"].ToString()) ? null : (DateTime?)row["GENZULDAT"],
						VLMDAT = string.IsNullOrEmpty(row["VLMDAT"].ToString()) ? null : (DateTime?)row["VLMDAT"],
						HCEINGDAT = string.IsNullOrEmpty(row["HCEINGDAT"].ToString()) ? null : (DateTime?)row["HCEINGDAT"],
						HCORT = (string)row["HCORT"],
						KMSTAND = (string)row["KMSTAND"],
						STILLDAT = string.IsNullOrEmpty(row["STILLDAT"].ToString()) ? null : (DateTime?)row["STILLDAT"],
						EGZB2DAT = string.IsNullOrEmpty(row["EGZB2DAT"].ToString()) ? null : (DateTime?)row["EGZB2DAT"],
						EGZWSLDAT = string.IsNullOrEmpty(row["EGZWSLDAT"].ToString()) ? null : (DateTime?)row["EGZWSLDAT"],
						EGRCKDAT = string.IsNullOrEmpty(row["EGRCKDAT"].ToString()) ? null : (DateTime?)row["EGRCKDAT"],
						VSZB2DAT = string.IsNullOrEmpty(row["VSZB2DAT"].ToString()) ? null : (DateTime?)row["VSZB2DAT"],
						VSZWSLDAT = string.IsNullOrEmpty(row["VSZWSLDAT"].ToString()) ? null : (DateTime?)row["VSZWSLDAT"],
						VSRCKDAT = string.IsNullOrEmpty(row["VSRCKDAT"].ToString()) ? null : (DateTime?)row["VSRCKDAT"],
						EREIGNIS = (string)row["EREIGNIS"],
						S_BETRAG = (string)row["S_BETRAG"],
						UEBERM_RE = string.IsNullOrEmpty(row["UEBERM_RE"].ToString()) ? null : (DateTime?)row["UEBERM_RE"],
						FLAG_VS = (string)row["FLAG_VS"],
						BETRAG_RE = (string)row["BETRAG_RE"],
						NUMMER_RE = (string)row["NUMMER_RE"],
						DATUM_RE = string.IsNullOrEmpty(row["DATUM_RE"].ToString()) ? null : (DateTime?)row["DATUM_RE"],
						BEM_1 = (string)row["BEM_1"],
						BEM_2 = (string)row["BEM_2"],
						EQUNR_T = (string)row["EQUNR_T"],
						EGNCDDAT = string.IsNullOrEmpty(row["EGNCDDAT"].ToString()) ? null : (DateTime?)row["EGNCDDAT"],
						UESVM = string.IsNullOrEmpty(row["UESVM"].ToString()) ? null : (DateTime?)row["UESVM"],
						UEVERLUST = string.IsNullOrEmpty(row["UEVERLUST"].ToString()) ? null : (DateTime?)row["UEVERLUST"],
						SCHADMELDDAT = string.IsNullOrEmpty(row["SCHADMELDDAT"].ToString()) ? null : (DateTime?)row["SCHADMELDDAT"],
						MELDDATGUTA = string.IsNullOrEmpty(row["MELDDATGUTA"].ToString()) ? null : (DateTime?)row["MELDDATGUTA"],
						AEDAT = string.IsNullOrEmpty(row["AEDAT"].ToString()) ? null : (DateTime?)row["AEDAT"],
						AENAM = (string)row["AENAM"],
						ZZCOCKZ = (string)row["ZZCOCKZ"],
						UEBMZLDAT = string.IsNullOrEmpty(row["UEBMZLDAT"].ToString()) ? null : (DateTime?)row["UEBMZLDAT"],
						IMPDAT_SCHAEDEN = string.IsNullOrEmpty(row["IMPDAT_SCHAEDEN"].ToString()) ? null : (DateTime?)row["IMPDAT_SCHAEDEN"],
						LIZNR = (string)row["LIZNR"],
						EREIGNIS_TEXT = (string)row["EREIGNIS_TEXT"],
						DATSVM = string.IsNullOrEmpty(row["DATSVM"].ToString()) ? null : (DateTime?)row["DATSVM"],
						USERSVM = (string)row["USERSVM"],
						DAT_VERT_WID = string.IsNullOrEmpty(row["DAT_VERT_WID"].ToString()) ? null : (DateTime?)row["DAT_VERT_WID"],
						ART_VERT_WID = (string)row["ART_VERT_WID"],
						DAT_BELAST_FREI = string.IsNullOrEmpty(row["DAT_BELAST_FREI"].ToString()) ? null : (DateTime?)row["DAT_BELAST_FREI"],
						DAT_TUEV_BEAUF_RUECK = string.IsNullOrEmpty(row["DAT_TUEV_BEAUF_RUECK"].ToString()) ? null : (DateTime?)row["DAT_TUEV_BEAUF_RUECK"],
						DAT_TUEV_BEAUF = string.IsNullOrEmpty(row["DAT_TUEV_BEAUF"].ToString()) ? null : (DateTime?)row["DAT_TUEV_BEAUF"],
						USER_TUEV_BEAUF = (string)row["USER_TUEV_BEAUF"],
						DAT_ABRECHNUNG = string.IsNullOrEmpty(row["DAT_ABRECHNUNG"].ToString()) ? null : (DateTime?)row["DAT_ABRECHNUNG"],
						RUECK_DAT = string.IsNullOrEmpty(row["RUECK_DAT"].ToString()) ? null : (DateTime?)row["RUECK_DAT"],
					};
				}
				catch(Exception e)
				{
					o = new GT_DATEN
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

			public static IEnumerable<GT_DATEN> Select(DataTable dt, ISapConnection sapConnection = null)
			{
				return dt.AsEnumerable().Select(r => Create(r, sapConnection));
			}

			public static List<GT_DATEN> ToList(DataTable dt, ISapConnection sapConnection = null)
			{
				return Select(dt, sapConnection).ToListOrEmptyList();
			}

			public static IEnumerable<GT_DATEN> Select(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				var tbl = dts.FirstOrDefault(t => t.TableName.ToLower() == typeof(GT_DATEN).Name.ToLower());
				if (tbl == null)
					return null;

				return Select(tbl, sapConnection);
			}

			public static List<GT_DATEN> ToList(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				return Select(dts, sapConnection).ToListOrEmptyList();
			}

			public static List<GT_DATEN> ToList(ISapDataService sapDataService)
			{
				return ToList(sapDataService.GetExportTables(), sapDataService.SapConnection);
			}

			public static List<GT_DATEN> GetExportListWithInitExecute(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<GT_DATEN>();
				 
				var dts = sapDataService.GetExportTablesWithInitExecute("Z_DPM_REM_FAHRZEUGHIST_02", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_DATEN> GetExportListWithExecute(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_DATEN>();
				 
				var dts = sapDataService.GetExportTablesWithExecute();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_DATEN> GetExportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_DATEN>();
				 
				var dts = sapDataService.GetExportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_DATEN> GetImportListWithInit(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<GT_DATEN>();
				 
				var dts = sapDataService.GetImportTablesWithInit("Z_DPM_REM_FAHRZEUGHIST_02", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_DATEN> GetImportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_DATEN>();
				 
				var dts = sapDataService.GetImportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}
		}

		public partial class GT_DATEN2 : IModelMappingApplied
		{
			[SapIgnore]
			[ScriptIgnore]
			public ISapConnection SAPConnection { get; set; }

			[SapIgnore]
			[ScriptIgnore]
			public IDynSapProxyFactory DynSapProxyFactory { get; set; }

			public string FAHRGNR { get; set; }

			public DateTime? GUTAUFTRAGDAT { get; set; }

			public DateTime? DAT_HC_AUSG { get; set; }

			public string VERTRAGSJAHR { get; set; }

			public string UPEPREIS { get; set; }

			private bool MappingErrorProcessed { get; set; }

			public static GT_DATEN2 Create(DataRow row, ISapConnection sapConnection = null, IDynSapProxyFactory dynSapProxyFactory = null)
			{
				GT_DATEN2 o;

				try
				{
					o = new GT_DATEN2
					{
						SAPConnection = sapConnection,
						DynSapProxyFactory = dynSapProxyFactory,

						FAHRGNR = (string)row["FAHRGNR"],
						GUTAUFTRAGDAT = string.IsNullOrEmpty(row["GUTAUFTRAGDAT"].ToString()) ? null : (DateTime?)row["GUTAUFTRAGDAT"],
						DAT_HC_AUSG = string.IsNullOrEmpty(row["DAT_HC_AUSG"].ToString()) ? null : (DateTime?)row["DAT_HC_AUSG"],
						VERTRAGSJAHR = (string)row["VERTRAGSJAHR"],
						UPEPREIS = (string)row["UPEPREIS"],
					};
				}
				catch(Exception e)
				{
					o = new GT_DATEN2
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

			public static IEnumerable<GT_DATEN2> Select(DataTable dt, ISapConnection sapConnection = null)
			{
				return dt.AsEnumerable().Select(r => Create(r, sapConnection));
			}

			public static List<GT_DATEN2> ToList(DataTable dt, ISapConnection sapConnection = null)
			{
				return Select(dt, sapConnection).ToListOrEmptyList();
			}

			public static IEnumerable<GT_DATEN2> Select(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				var tbl = dts.FirstOrDefault(t => t.TableName.ToLower() == typeof(GT_DATEN2).Name.ToLower());
				if (tbl == null)
					return null;

				return Select(tbl, sapConnection);
			}

			public static List<GT_DATEN2> ToList(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				return Select(dts, sapConnection).ToListOrEmptyList();
			}

			public static List<GT_DATEN2> ToList(ISapDataService sapDataService)
			{
				return ToList(sapDataService.GetExportTables(), sapDataService.SapConnection);
			}

			public static List<GT_DATEN2> GetExportListWithInitExecute(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<GT_DATEN2>();
				 
				var dts = sapDataService.GetExportTablesWithInitExecute("Z_DPM_REM_FAHRZEUGHIST_02", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_DATEN2> GetExportListWithExecute(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_DATEN2>();
				 
				var dts = sapDataService.GetExportTablesWithExecute();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_DATEN2> GetExportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_DATEN2>();
				 
				var dts = sapDataService.GetExportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_DATEN2> GetImportListWithInit(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<GT_DATEN2>();
				 
				var dts = sapDataService.GetImportTablesWithInit("Z_DPM_REM_FAHRZEUGHIST_02", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_DATEN2> GetImportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_DATEN2>();
				 
				var dts = sapDataService.GetImportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}
		}

		public partial class GT_EQUI_B : IModelMappingApplied
		{
			[SapIgnore]
			[ScriptIgnore]
			public ISapConnection SAPConnection { get; set; }

			[SapIgnore]
			[ScriptIgnore]
			public IDynSapProxyFactory DynSapProxyFactory { get; set; }

			public string EQUNR { get; set; }

			public string CHASSIS_NUM { get; set; }

			public string LICENSE_NUM { get; set; }

			public string LIZNR { get; set; }

			public string TIDNR { get; set; }

			public string ZZREFERENZ1 { get; set; }

			public DateTime? ERDAT { get; set; }

			public DateTime? ZZTMPDT { get; set; }

			public string ABCKZ { get; set; }

			public string EX_KUNNR { get; set; }

			public string OBJNR { get; set; }

			public string ZZFINART_TXT { get; set; }

			public string ZZVGRUND { get; set; }

			public string ZZREFERENZ2 { get; set; }

			private bool MappingErrorProcessed { get; set; }

			public static GT_EQUI_B Create(DataRow row, ISapConnection sapConnection = null, IDynSapProxyFactory dynSapProxyFactory = null)
			{
				GT_EQUI_B o;

				try
				{
					o = new GT_EQUI_B
					{
						SAPConnection = sapConnection,
						DynSapProxyFactory = dynSapProxyFactory,

						EQUNR = (string)row["EQUNR"],
						CHASSIS_NUM = (string)row["CHASSIS_NUM"],
						LICENSE_NUM = (string)row["LICENSE_NUM"],
						LIZNR = (string)row["LIZNR"],
						TIDNR = (string)row["TIDNR"],
						ZZREFERENZ1 = (string)row["ZZREFERENZ1"],
						ERDAT = string.IsNullOrEmpty(row["ERDAT"].ToString()) ? null : (DateTime?)row["ERDAT"],
						ZZTMPDT = string.IsNullOrEmpty(row["ZZTMPDT"].ToString()) ? null : (DateTime?)row["ZZTMPDT"],
						ABCKZ = (string)row["ABCKZ"],
						EX_KUNNR = (string)row["EX_KUNNR"],
						OBJNR = (string)row["OBJNR"],
						ZZFINART_TXT = (string)row["ZZFINART_TXT"],
						ZZVGRUND = (string)row["ZZVGRUND"],
						ZZREFERENZ2 = (string)row["ZZREFERENZ2"],
					};
				}
				catch(Exception e)
				{
					o = new GT_EQUI_B
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

			public static IEnumerable<GT_EQUI_B> Select(DataTable dt, ISapConnection sapConnection = null)
			{
				return dt.AsEnumerable().Select(r => Create(r, sapConnection));
			}

			public static List<GT_EQUI_B> ToList(DataTable dt, ISapConnection sapConnection = null)
			{
				return Select(dt, sapConnection).ToListOrEmptyList();
			}

			public static IEnumerable<GT_EQUI_B> Select(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				var tbl = dts.FirstOrDefault(t => t.TableName.ToLower() == typeof(GT_EQUI_B).Name.ToLower());
				if (tbl == null)
					return null;

				return Select(tbl, sapConnection);
			}

			public static List<GT_EQUI_B> ToList(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				return Select(dts, sapConnection).ToListOrEmptyList();
			}

			public static List<GT_EQUI_B> ToList(ISapDataService sapDataService)
			{
				return ToList(sapDataService.GetExportTables(), sapDataService.SapConnection);
			}

			public static List<GT_EQUI_B> GetExportListWithInitExecute(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<GT_EQUI_B>();
				 
				var dts = sapDataService.GetExportTablesWithInitExecute("Z_DPM_REM_FAHRZEUGHIST_02", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_EQUI_B> GetExportListWithExecute(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_EQUI_B>();
				 
				var dts = sapDataService.GetExportTablesWithExecute();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_EQUI_B> GetExportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_EQUI_B>();
				 
				var dts = sapDataService.GetExportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_EQUI_B> GetImportListWithInit(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<GT_EQUI_B>();
				 
				var dts = sapDataService.GetImportTablesWithInit("Z_DPM_REM_FAHRZEUGHIST_02", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_EQUI_B> GetImportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_EQUI_B>();
				 
				var dts = sapDataService.GetImportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}
		}

		public partial class GT_GUTA : IModelMappingApplied
		{
			[SapIgnore]
			[ScriptIgnore]
			public ISapConnection SAPConnection { get; set; }

			[SapIgnore]
			[ScriptIgnore]
			public IDynSapProxyFactory DynSapProxyFactory { get; set; }

			public string FAHRGNR { get; set; }

			public string LFDNR { get; set; }

			public DateTime? INDATUM { get; set; }

			public string INTIME { get; set; }

			public string GUTA { get; set; }

			public string GUTAID { get; set; }

			public string KMSTAND { get; set; }

			public DateTime? GUTADAT { get; set; }

			public string SCHADKZ { get; set; }

			public decimal? SCHADBETR { get; set; }

			public decimal? SCHADBETR_AV { get; set; }

			public string REPKZ { get; set; }

			public decimal? AUFBETR { get; set; }

			public decimal? AUFBETR_AV { get; set; }

			public decimal? WRTMBETR { get; set; }

			public decimal? WRTMBETR_AV { get; set; }

			public decimal? FEHLTBETR { get; set; }

			public decimal? FEHLTBETR_AV { get; set; }

			public decimal? RESTWERT { get; set; }

			public string REPKALK { get; set; }

			public DateTime? REPKALKDAT { get; set; }

			private bool MappingErrorProcessed { get; set; }

			public static GT_GUTA Create(DataRow row, ISapConnection sapConnection = null, IDynSapProxyFactory dynSapProxyFactory = null)
			{
				GT_GUTA o;

				try
				{
					o = new GT_GUTA
					{
						SAPConnection = sapConnection,
						DynSapProxyFactory = dynSapProxyFactory,

						FAHRGNR = (string)row["FAHRGNR"],
						LFDNR = (string)row["LFDNR"],
						INDATUM = string.IsNullOrEmpty(row["INDATUM"].ToString()) ? null : (DateTime?)row["INDATUM"],
						INTIME = (string)row["INTIME"],
						GUTA = (string)row["GUTA"],
						GUTAID = (string)row["GUTAID"],
						KMSTAND = (string)row["KMSTAND"],
						GUTADAT = string.IsNullOrEmpty(row["GUTADAT"].ToString()) ? null : (DateTime?)row["GUTADAT"],
						SCHADKZ = (string)row["SCHADKZ"],
						SCHADBETR = string.IsNullOrEmpty(row["SCHADBETR"].ToString()) ? null : (decimal?)row["SCHADBETR"],
						SCHADBETR_AV = string.IsNullOrEmpty(row["SCHADBETR_AV"].ToString()) ? null : (decimal?)row["SCHADBETR_AV"],
						REPKZ = (string)row["REPKZ"],
						AUFBETR = string.IsNullOrEmpty(row["AUFBETR"].ToString()) ? null : (decimal?)row["AUFBETR"],
						AUFBETR_AV = string.IsNullOrEmpty(row["AUFBETR_AV"].ToString()) ? null : (decimal?)row["AUFBETR_AV"],
						WRTMBETR = string.IsNullOrEmpty(row["WRTMBETR"].ToString()) ? null : (decimal?)row["WRTMBETR"],
						WRTMBETR_AV = string.IsNullOrEmpty(row["WRTMBETR_AV"].ToString()) ? null : (decimal?)row["WRTMBETR_AV"],
						FEHLTBETR = string.IsNullOrEmpty(row["FEHLTBETR"].ToString()) ? null : (decimal?)row["FEHLTBETR"],
						FEHLTBETR_AV = string.IsNullOrEmpty(row["FEHLTBETR_AV"].ToString()) ? null : (decimal?)row["FEHLTBETR_AV"],
						RESTWERT = string.IsNullOrEmpty(row["RESTWERT"].ToString()) ? null : (decimal?)row["RESTWERT"],
						REPKALK = (string)row["REPKALK"],
						REPKALKDAT = string.IsNullOrEmpty(row["REPKALKDAT"].ToString()) ? null : (DateTime?)row["REPKALKDAT"],
					};
				}
				catch(Exception e)
				{
					o = new GT_GUTA
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

			public static IEnumerable<GT_GUTA> Select(DataTable dt, ISapConnection sapConnection = null)
			{
				return dt.AsEnumerable().Select(r => Create(r, sapConnection));
			}

			public static List<GT_GUTA> ToList(DataTable dt, ISapConnection sapConnection = null)
			{
				return Select(dt, sapConnection).ToListOrEmptyList();
			}

			public static IEnumerable<GT_GUTA> Select(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				var tbl = dts.FirstOrDefault(t => t.TableName.ToLower() == typeof(GT_GUTA).Name.ToLower());
				if (tbl == null)
					return null;

				return Select(tbl, sapConnection);
			}

			public static List<GT_GUTA> ToList(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				return Select(dts, sapConnection).ToListOrEmptyList();
			}

			public static List<GT_GUTA> ToList(ISapDataService sapDataService)
			{
				return ToList(sapDataService.GetExportTables(), sapDataService.SapConnection);
			}

			public static List<GT_GUTA> GetExportListWithInitExecute(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<GT_GUTA>();
				 
				var dts = sapDataService.GetExportTablesWithInitExecute("Z_DPM_REM_FAHRZEUGHIST_02", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_GUTA> GetExportListWithExecute(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_GUTA>();
				 
				var dts = sapDataService.GetExportTablesWithExecute();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_GUTA> GetExportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_GUTA>();
				 
				var dts = sapDataService.GetExportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_GUTA> GetImportListWithInit(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<GT_GUTA>();
				 
				var dts = sapDataService.GetImportTablesWithInit("Z_DPM_REM_FAHRZEUGHIST_02", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_GUTA> GetImportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_GUTA>();
				 
				var dts = sapDataService.GetImportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}
		}

		public partial class GT_LEB_B : IModelMappingApplied
		{
			[SapIgnore]
			[ScriptIgnore]
			public ISapConnection SAPConnection { get; set; }

			[SapIgnore]
			[ScriptIgnore]
			public IDynSapProxyFactory DynSapProxyFactory { get; set; }

			public string KUNNR { get; set; }

			public string NAME1_AG { get; set; }

			public string ORT01_AG { get; set; }

			public string QMNUM { get; set; }

			public string EQUNR { get; set; }

			public string ZZKENN { get; set; }

			public string ZZBRIEF { get; set; }

			public string ZZKENN_OLD { get; set; }

			public string ZZBRIEF_OLD { get; set; }

			public string ZZBRIEF_A { get; set; }

			public DateTime? UDATE { get; set; }

			public string ZZFAHRG { get; set; }

			public DateTime? REPLA_DATE { get; set; }

			public DateTime? EXPIRY_DATE { get; set; }

			public string ZZSTATUS_ZUL { get; set; }

			public string ZZSTATUS_ABG { get; set; }

			public string ZZSTATUS_BAG { get; set; }

			public string ZZSTATUS_OZU { get; set; }

			public string ZZSTATUS_ZUB { get; set; }

			public DateTime? ZZTMPDT { get; set; }

			public string SCHILDER_PHY { get; set; }

			public string SCHEIN_PHY { get; set; }

			public DateTime? ERDAT { get; set; }

			public DateTime? MINDBIS { get; set; }

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

			public DateTime? ZZCARPORT_EING { get; set; }

			public DateTime? ZZKENN_EING { get; set; }

			public string ADRNR_VS { get; set; }

			public string NAME1_VS { get; set; }

			public string NAME2_VS { get; set; }

			public string ORT01_VS { get; set; }

			public string PSTLZ_VS { get; set; }

			public string STRAS_VS { get; set; }

			public string HSNR_VS { get; set; }

			public string KUNNR_ZH { get; set; }

			public string ADRNR_ZH { get; set; }

			public string NAME1_ZH { get; set; }

			public string NAME2_ZH { get; set; }

			public string ORT01_ZH { get; set; }

			public string PSTLZ_ZH { get; set; }

			public string STRAS_ZH { get; set; }

			public string HSNR_ZH { get; set; }

			public string ZZREF1 { get; set; }

			public string ZZREF2 { get; set; }

			public decimal? ENGINE_POWER { get; set; }

			public DateTime? CHECK_IN { get; set; }

			public string ABCKZ { get; set; }

			public string ZZCOCKZ { get; set; }

			public string ZZVGRUND { get; set; }

			public string ZZCO2KOMBI { get; set; }

			public DateTime? ZZDAT_BER { get; set; }

			public string KUNNR_ZF { get; set; }

			public string ADRNR_ZF { get; set; }

			public string NAME1_ZF { get; set; }

			public string NAME2_ZF { get; set; }

			public string ORT01_ZF { get; set; }

			public string PSTLZ_ZF { get; set; }

			public string STRAS_ZF { get; set; }

			public string HSNR_ZF { get; set; }

			public string KUNNR_ZL { get; set; }

			public string ADRNR_ZL { get; set; }

			public string NAME1_ZL { get; set; }

			public string NAME2_ZL { get; set; }

			public string ORT01_ZL { get; set; }

			public string PSTLZ_ZL { get; set; }

			public string STRAS_ZL { get; set; }

			public string HSNR_ZL { get; set; }

			public string KONZS_ZK { get; set; }

			public string ZZHERST_TEXT { get; set; }

			public string ZZHERSTELLER_SCH { get; set; }

			public string ZZHANDELSNAME { get; set; }

			public string ZZTYP_SCHL { get; set; }

			public string ZZVVS_SCHLUESSEL { get; set; }

			public string ZFARBE { get; set; }

			public string KUNNR_ZA { get; set; }

			public string ADRNR_ZA { get; set; }

			public string NAME1_ZA { get; set; }

			public string NAME2_ZA { get; set; }

			public string STRAS_ZA { get; set; }

			public string HSNR_ZA { get; set; }

			public string PSTLZ_ZA { get; set; }

			public string ORT01_ZA { get; set; }

			public DateTime? ZZZLDAT { get; set; }

			public string ZZREFERENZ1 { get; set; }

			public string ZZLABEL { get; set; }

			public string BANK { get; set; }

			public string BRANDING { get; set; }

			public string BEZEICHNUNG_1 { get; set; }

			public string BEZEICHNUNG_2 { get; set; }

			public string BEZEICHNUNG_3 { get; set; }

			public string DADPDI_ORT { get; set; }

			public string ZZFINART_TXT { get; set; }

			public string ZZREFERENZ2 { get; set; }

			public string STANDORT_VERSSTAT_TEXT { get; set; }

			private bool MappingErrorProcessed { get; set; }

			public static GT_LEB_B Create(DataRow row, ISapConnection sapConnection = null, IDynSapProxyFactory dynSapProxyFactory = null)
			{
				GT_LEB_B o;

				try
				{
					o = new GT_LEB_B
					{
						SAPConnection = sapConnection,
						DynSapProxyFactory = dynSapProxyFactory,

						KUNNR = (string)row["KUNNR"],
						NAME1_AG = (string)row["NAME1_AG"],
						ORT01_AG = (string)row["ORT01_AG"],
						QMNUM = (string)row["QMNUM"],
						EQUNR = (string)row["EQUNR"],
						ZZKENN = (string)row["ZZKENN"],
						ZZBRIEF = (string)row["ZZBRIEF"],
						ZZKENN_OLD = (string)row["ZZKENN_OLD"],
						ZZBRIEF_OLD = (string)row["ZZBRIEF_OLD"],
						ZZBRIEF_A = (string)row["ZZBRIEF_A"],
						UDATE = string.IsNullOrEmpty(row["UDATE"].ToString()) ? null : (DateTime?)row["UDATE"],
						ZZFAHRG = (string)row["ZZFAHRG"],
						REPLA_DATE = string.IsNullOrEmpty(row["REPLA_DATE"].ToString()) ? null : (DateTime?)row["REPLA_DATE"],
						EXPIRY_DATE = string.IsNullOrEmpty(row["EXPIRY_DATE"].ToString()) ? null : (DateTime?)row["EXPIRY_DATE"],
						ZZSTATUS_ZUL = (string)row["ZZSTATUS_ZUL"],
						ZZSTATUS_ABG = (string)row["ZZSTATUS_ABG"],
						ZZSTATUS_BAG = (string)row["ZZSTATUS_BAG"],
						ZZSTATUS_OZU = (string)row["ZZSTATUS_OZU"],
						ZZSTATUS_ZUB = (string)row["ZZSTATUS_ZUB"],
						ZZTMPDT = string.IsNullOrEmpty(row["ZZTMPDT"].ToString()) ? null : (DateTime?)row["ZZTMPDT"],
						SCHILDER_PHY = (string)row["SCHILDER_PHY"],
						SCHEIN_PHY = (string)row["SCHEIN_PHY"],
						ERDAT = string.IsNullOrEmpty(row["ERDAT"].ToString()) ? null : (DateTime?)row["ERDAT"],
						MINDBIS = string.IsNullOrEmpty(row["MINDBIS"].ToString()) ? null : (DateTime?)row["MINDBIS"],
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
						ZZCARPORT_EING = string.IsNullOrEmpty(row["ZZCARPORT_EING"].ToString()) ? null : (DateTime?)row["ZZCARPORT_EING"],
						ZZKENN_EING = string.IsNullOrEmpty(row["ZZKENN_EING"].ToString()) ? null : (DateTime?)row["ZZKENN_EING"],
						ADRNR_VS = (string)row["ADRNR_VS"],
						NAME1_VS = (string)row["NAME1_VS"],
						NAME2_VS = (string)row["NAME2_VS"],
						ORT01_VS = (string)row["ORT01_VS"],
						PSTLZ_VS = (string)row["PSTLZ_VS"],
						STRAS_VS = (string)row["STRAS_VS"],
						HSNR_VS = (string)row["HSNR_VS"],
						KUNNR_ZH = (string)row["KUNNR_ZH"],
						ADRNR_ZH = (string)row["ADRNR_ZH"],
						NAME1_ZH = (string)row["NAME1_ZH"],
						NAME2_ZH = (string)row["NAME2_ZH"],
						ORT01_ZH = (string)row["ORT01_ZH"],
						PSTLZ_ZH = (string)row["PSTLZ_ZH"],
						STRAS_ZH = (string)row["STRAS_ZH"],
						HSNR_ZH = (string)row["HSNR_ZH"],
						ZZREF1 = (string)row["ZZREF1"],
						ZZREF2 = (string)row["ZZREF2"],
						ENGINE_POWER = string.IsNullOrEmpty(row["ENGINE_POWER"].ToString()) ? null : (decimal?)row["ENGINE_POWER"],
						CHECK_IN = string.IsNullOrEmpty(row["CHECK_IN"].ToString()) ? null : (DateTime?)row["CHECK_IN"],
						ABCKZ = (string)row["ABCKZ"],
						ZZCOCKZ = (string)row["ZZCOCKZ"],
						ZZVGRUND = (string)row["ZZVGRUND"],
						ZZCO2KOMBI = (string)row["ZZCO2KOMBI"],
						ZZDAT_BER = string.IsNullOrEmpty(row["ZZDAT_BER"].ToString()) ? null : (DateTime?)row["ZZDAT_BER"],
						KUNNR_ZF = (string)row["KUNNR_ZF"],
						ADRNR_ZF = (string)row["ADRNR_ZF"],
						NAME1_ZF = (string)row["NAME1_ZF"],
						NAME2_ZF = (string)row["NAME2_ZF"],
						ORT01_ZF = (string)row["ORT01_ZF"],
						PSTLZ_ZF = (string)row["PSTLZ_ZF"],
						STRAS_ZF = (string)row["STRAS_ZF"],
						HSNR_ZF = (string)row["HSNR_ZF"],
						KUNNR_ZL = (string)row["KUNNR_ZL"],
						ADRNR_ZL = (string)row["ADRNR_ZL"],
						NAME1_ZL = (string)row["NAME1_ZL"],
						NAME2_ZL = (string)row["NAME2_ZL"],
						ORT01_ZL = (string)row["ORT01_ZL"],
						PSTLZ_ZL = (string)row["PSTLZ_ZL"],
						STRAS_ZL = (string)row["STRAS_ZL"],
						HSNR_ZL = (string)row["HSNR_ZL"],
						KONZS_ZK = (string)row["KONZS_ZK"],
						ZZHERST_TEXT = (string)row["ZZHERST_TEXT"],
						ZZHERSTELLER_SCH = (string)row["ZZHERSTELLER_SCH"],
						ZZHANDELSNAME = (string)row["ZZHANDELSNAME"],
						ZZTYP_SCHL = (string)row["ZZTYP_SCHL"],
						ZZVVS_SCHLUESSEL = (string)row["ZZVVS_SCHLUESSEL"],
						ZFARBE = (string)row["ZFARBE"],
						KUNNR_ZA = (string)row["KUNNR_ZA"],
						ADRNR_ZA = (string)row["ADRNR_ZA"],
						NAME1_ZA = (string)row["NAME1_ZA"],
						NAME2_ZA = (string)row["NAME2_ZA"],
						STRAS_ZA = (string)row["STRAS_ZA"],
						HSNR_ZA = (string)row["HSNR_ZA"],
						PSTLZ_ZA = (string)row["PSTLZ_ZA"],
						ORT01_ZA = (string)row["ORT01_ZA"],
						ZZZLDAT = string.IsNullOrEmpty(row["ZZZLDAT"].ToString()) ? null : (DateTime?)row["ZZZLDAT"],
						ZZREFERENZ1 = (string)row["ZZREFERENZ1"],
						ZZLABEL = (string)row["ZZLABEL"],
						BANK = (string)row["BANK"],
						BRANDING = (string)row["BRANDING"],
						BEZEICHNUNG_1 = (string)row["BEZEICHNUNG_1"],
						BEZEICHNUNG_2 = (string)row["BEZEICHNUNG_2"],
						BEZEICHNUNG_3 = (string)row["BEZEICHNUNG_3"],
						DADPDI_ORT = (string)row["DADPDI_ORT"],
						ZZFINART_TXT = (string)row["ZZFINART_TXT"],
						ZZREFERENZ2 = (string)row["ZZREFERENZ2"],
						STANDORT_VERSSTAT_TEXT = (string)row["STANDORT_VERSSTAT_TEXT"],
					};
				}
				catch(Exception e)
				{
					o = new GT_LEB_B
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

			public static IEnumerable<GT_LEB_B> Select(DataTable dt, ISapConnection sapConnection = null)
			{
				return dt.AsEnumerable().Select(r => Create(r, sapConnection));
			}

			public static List<GT_LEB_B> ToList(DataTable dt, ISapConnection sapConnection = null)
			{
				return Select(dt, sapConnection).ToListOrEmptyList();
			}

			public static IEnumerable<GT_LEB_B> Select(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				var tbl = dts.FirstOrDefault(t => t.TableName.ToLower() == typeof(GT_LEB_B).Name.ToLower());
				if (tbl == null)
					return null;

				return Select(tbl, sapConnection);
			}

			public static List<GT_LEB_B> ToList(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				return Select(dts, sapConnection).ToListOrEmptyList();
			}

			public static List<GT_LEB_B> ToList(ISapDataService sapDataService)
			{
				return ToList(sapDataService.GetExportTables(), sapDataService.SapConnection);
			}

			public static List<GT_LEB_B> GetExportListWithInitExecute(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<GT_LEB_B>();
				 
				var dts = sapDataService.GetExportTablesWithInitExecute("Z_DPM_REM_FAHRZEUGHIST_02", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_LEB_B> GetExportListWithExecute(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_LEB_B>();
				 
				var dts = sapDataService.GetExportTablesWithExecute();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_LEB_B> GetExportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_LEB_B>();
				 
				var dts = sapDataService.GetExportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_LEB_B> GetImportListWithInit(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<GT_LEB_B>();
				 
				var dts = sapDataService.GetImportTablesWithInit("Z_DPM_REM_FAHRZEUGHIST_02", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_LEB_B> GetImportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_LEB_B>();
				 
				var dts = sapDataService.GetImportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}
		}

		public partial class GT_LEB_T : IModelMappingApplied
		{
			[SapIgnore]
			[ScriptIgnore]
			public ISapConnection SAPConnection { get; set; }

			[SapIgnore]
			[ScriptIgnore]
			public IDynSapProxyFactory DynSapProxyFactory { get; set; }

			public string CHASSIS_NUM { get; set; }

			public string EQUNR { get; set; }

			public string EQTYP { get; set; }

			public DateTime? ERDAT { get; set; }

			public DateTime? ZZTMPDT { get; set; }

			public string ERSATZSCHLUESSEL { get; set; }

			public string CARPASS { get; set; }

			public string RADIOCODEKARTE { get; set; }

			public string NAVICD { get; set; }

			public string CHIPKARTE { get; set; }

			public string COCBESCHEINIGUNG { get; set; }

			public string NAVICODEKARTE { get; set; }

			public string WEGFAHRSPCODEKARTE { get; set; }

			public string ERSATZFERNBSH { get; set; }

			public string PRUEFBUCH_LKW { get; set; }

			public string ADRNR_VS { get; set; }

			public string NAME1_VS { get; set; }

			public string NAME2_VS { get; set; }

			public string ORT01_VS { get; set; }

			public string PSTLZ_VS { get; set; }

			public string STRAS_VS { get; set; }

			public string HSNR_VS { get; set; }

			public string ABCKZ { get; set; }

			private bool MappingErrorProcessed { get; set; }

			public static GT_LEB_T Create(DataRow row, ISapConnection sapConnection = null, IDynSapProxyFactory dynSapProxyFactory = null)
			{
				GT_LEB_T o;

				try
				{
					o = new GT_LEB_T
					{
						SAPConnection = sapConnection,
						DynSapProxyFactory = dynSapProxyFactory,

						CHASSIS_NUM = (string)row["CHASSIS_NUM"],
						EQUNR = (string)row["EQUNR"],
						EQTYP = (string)row["EQTYP"],
						ERDAT = string.IsNullOrEmpty(row["ERDAT"].ToString()) ? null : (DateTime?)row["ERDAT"],
						ZZTMPDT = string.IsNullOrEmpty(row["ZZTMPDT"].ToString()) ? null : (DateTime?)row["ZZTMPDT"],
						ERSATZSCHLUESSEL = (string)row["ERSATZSCHLUESSEL"],
						CARPASS = (string)row["CARPASS"],
						RADIOCODEKARTE = (string)row["RADIOCODEKARTE"],
						NAVICD = (string)row["NAVICD"],
						CHIPKARTE = (string)row["CHIPKARTE"],
						COCBESCHEINIGUNG = (string)row["COCBESCHEINIGUNG"],
						NAVICODEKARTE = (string)row["NAVICODEKARTE"],
						WEGFAHRSPCODEKARTE = (string)row["WEGFAHRSPCODEKARTE"],
						ERSATZFERNBSH = (string)row["ERSATZFERNBSH"],
						PRUEFBUCH_LKW = (string)row["PRUEFBUCH_LKW"],
						ADRNR_VS = (string)row["ADRNR_VS"],
						NAME1_VS = (string)row["NAME1_VS"],
						NAME2_VS = (string)row["NAME2_VS"],
						ORT01_VS = (string)row["ORT01_VS"],
						PSTLZ_VS = (string)row["PSTLZ_VS"],
						STRAS_VS = (string)row["STRAS_VS"],
						HSNR_VS = (string)row["HSNR_VS"],
						ABCKZ = (string)row["ABCKZ"],
					};
				}
				catch(Exception e)
				{
					o = new GT_LEB_T
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

			public static IEnumerable<GT_LEB_T> Select(DataTable dt, ISapConnection sapConnection = null)
			{
				return dt.AsEnumerable().Select(r => Create(r, sapConnection));
			}

			public static List<GT_LEB_T> ToList(DataTable dt, ISapConnection sapConnection = null)
			{
				return Select(dt, sapConnection).ToListOrEmptyList();
			}

			public static IEnumerable<GT_LEB_T> Select(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				var tbl = dts.FirstOrDefault(t => t.TableName.ToLower() == typeof(GT_LEB_T).Name.ToLower());
				if (tbl == null)
					return null;

				return Select(tbl, sapConnection);
			}

			public static List<GT_LEB_T> ToList(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				return Select(dts, sapConnection).ToListOrEmptyList();
			}

			public static List<GT_LEB_T> ToList(ISapDataService sapDataService)
			{
				return ToList(sapDataService.GetExportTables(), sapDataService.SapConnection);
			}

			public static List<GT_LEB_T> GetExportListWithInitExecute(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<GT_LEB_T>();
				 
				var dts = sapDataService.GetExportTablesWithInitExecute("Z_DPM_REM_FAHRZEUGHIST_02", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_LEB_T> GetExportListWithExecute(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_LEB_T>();
				 
				var dts = sapDataService.GetExportTablesWithExecute();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_LEB_T> GetExportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_LEB_T>();
				 
				var dts = sapDataService.GetExportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_LEB_T> GetImportListWithInit(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<GT_LEB_T>();
				 
				var dts = sapDataService.GetImportTablesWithInit("Z_DPM_REM_FAHRZEUGHIST_02", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_LEB_T> GetImportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_LEB_T>();
				 
				var dts = sapDataService.GetImportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}
		}

		public partial class GT_QMEL_B : IModelMappingApplied
		{
			[SapIgnore]
			[ScriptIgnore]
			public ISapConnection SAPConnection { get; set; }

			[SapIgnore]
			[ScriptIgnore]
			public IDynSapProxyFactory DynSapProxyFactory { get; set; }

			public string QMNUM { get; set; }

			public string ERNAM { get; set; }

			public DateTime? ERDAT { get; set; }

			public DateTime? STRMN { get; set; }

			public string KURZTEXT { get; set; }

			public string QMNAM { get; set; }

			public string KUNNR_Z5 { get; set; }

			public string ADRNR_Z5 { get; set; }

			public string NAME1_Z5 { get; set; }

			public string NAME2_Z5 { get; set; }

			public string STREET_Z5 { get; set; }

			public string HOUSE_NUM1_Z5 { get; set; }

			public string POST_CODE1_Z5 { get; set; }

			public string CITY1_Z5 { get; set; }

			public string LANDX_Z5 { get; set; }

			public string ZZDIEN1 { get; set; }

			public string QMGRP { get; set; }

			public string LICENSE_NUM { get; set; }

			public string ZZULDAT { get; set; }

			public string LIZNR { get; set; }

			public string ZZREFERENZ1 { get; set; }

			private bool MappingErrorProcessed { get; set; }

			public static GT_QMEL_B Create(DataRow row, ISapConnection sapConnection = null, IDynSapProxyFactory dynSapProxyFactory = null)
			{
				GT_QMEL_B o;

				try
				{
					o = new GT_QMEL_B
					{
						SAPConnection = sapConnection,
						DynSapProxyFactory = dynSapProxyFactory,

						QMNUM = (string)row["QMNUM"],
						ERNAM = (string)row["ERNAM"],
						ERDAT = string.IsNullOrEmpty(row["ERDAT"].ToString()) ? null : (DateTime?)row["ERDAT"],
						STRMN = string.IsNullOrEmpty(row["STRMN"].ToString()) ? null : (DateTime?)row["STRMN"],
						KURZTEXT = (string)row["KURZTEXT"],
						QMNAM = (string)row["QMNAM"],
						KUNNR_Z5 = (string)row["KUNNR_Z5"],
						ADRNR_Z5 = (string)row["ADRNR_Z5"],
						NAME1_Z5 = (string)row["NAME1_Z5"],
						NAME2_Z5 = (string)row["NAME2_Z5"],
						STREET_Z5 = (string)row["STREET_Z5"],
						HOUSE_NUM1_Z5 = (string)row["HOUSE_NUM1_Z5"],
						POST_CODE1_Z5 = (string)row["POST_CODE1_Z5"],
						CITY1_Z5 = (string)row["CITY1_Z5"],
						LANDX_Z5 = (string)row["LANDX_Z5"],
						ZZDIEN1 = (string)row["ZZDIEN1"],
						QMGRP = (string)row["QMGRP"],
						LICENSE_NUM = (string)row["LICENSE_NUM"],
						ZZULDAT = (string)row["ZZULDAT"],
						LIZNR = (string)row["LIZNR"],
						ZZREFERENZ1 = (string)row["ZZREFERENZ1"],
					};
				}
				catch(Exception e)
				{
					o = new GT_QMEL_B
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

			public static IEnumerable<GT_QMEL_B> Select(DataTable dt, ISapConnection sapConnection = null)
			{
				return dt.AsEnumerable().Select(r => Create(r, sapConnection));
			}

			public static List<GT_QMEL_B> ToList(DataTable dt, ISapConnection sapConnection = null)
			{
				return Select(dt, sapConnection).ToListOrEmptyList();
			}

			public static IEnumerable<GT_QMEL_B> Select(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				var tbl = dts.FirstOrDefault(t => t.TableName.ToLower() == typeof(GT_QMEL_B).Name.ToLower());
				if (tbl == null)
					return null;

				return Select(tbl, sapConnection);
			}

			public static List<GT_QMEL_B> ToList(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				return Select(dts, sapConnection).ToListOrEmptyList();
			}

			public static List<GT_QMEL_B> ToList(ISapDataService sapDataService)
			{
				return ToList(sapDataService.GetExportTables(), sapDataService.SapConnection);
			}

			public static List<GT_QMEL_B> GetExportListWithInitExecute(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<GT_QMEL_B>();
				 
				var dts = sapDataService.GetExportTablesWithInitExecute("Z_DPM_REM_FAHRZEUGHIST_02", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_QMEL_B> GetExportListWithExecute(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_QMEL_B>();
				 
				var dts = sapDataService.GetExportTablesWithExecute();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_QMEL_B> GetExportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_QMEL_B>();
				 
				var dts = sapDataService.GetExportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_QMEL_B> GetImportListWithInit(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<GT_QMEL_B>();
				 
				var dts = sapDataService.GetImportTablesWithInit("Z_DPM_REM_FAHRZEUGHIST_02", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_QMEL_B> GetImportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_QMEL_B>();
				 
				var dts = sapDataService.GetImportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}
		}

		public partial class GT_QMMA_B : IModelMappingApplied
		{
			[SapIgnore]
			[ScriptIgnore]
			public ISapConnection SAPConnection { get; set; }

			[SapIgnore]
			[ScriptIgnore]
			public IDynSapProxyFactory DynSapProxyFactory { get; set; }

			public string QMNUM { get; set; }

			public string MANUM { get; set; }

			public string MNCOD { get; set; }

			public string MATXT { get; set; }

			public DateTime? AEDAT { get; set; }

			public string AEZEIT { get; set; }

			public DateTime? ZZUEBER { get; set; }

			public DateTime? PSTER { get; set; }

			private bool MappingErrorProcessed { get; set; }

			public static GT_QMMA_B Create(DataRow row, ISapConnection sapConnection = null, IDynSapProxyFactory dynSapProxyFactory = null)
			{
				GT_QMMA_B o;

				try
				{
					o = new GT_QMMA_B
					{
						SAPConnection = sapConnection,
						DynSapProxyFactory = dynSapProxyFactory,

						QMNUM = (string)row["QMNUM"],
						MANUM = (string)row["MANUM"],
						MNCOD = (string)row["MNCOD"],
						MATXT = (string)row["MATXT"],
						AEDAT = string.IsNullOrEmpty(row["AEDAT"].ToString()) ? null : (DateTime?)row["AEDAT"],
						AEZEIT = (string)row["AEZEIT"],
						ZZUEBER = string.IsNullOrEmpty(row["ZZUEBER"].ToString()) ? null : (DateTime?)row["ZZUEBER"],
						PSTER = string.IsNullOrEmpty(row["PSTER"].ToString()) ? null : (DateTime?)row["PSTER"],
					};
				}
				catch(Exception e)
				{
					o = new GT_QMMA_B
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

			public static IEnumerable<GT_QMMA_B> Select(DataTable dt, ISapConnection sapConnection = null)
			{
				return dt.AsEnumerable().Select(r => Create(r, sapConnection));
			}

			public static List<GT_QMMA_B> ToList(DataTable dt, ISapConnection sapConnection = null)
			{
				return Select(dt, sapConnection).ToListOrEmptyList();
			}

			public static IEnumerable<GT_QMMA_B> Select(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				var tbl = dts.FirstOrDefault(t => t.TableName.ToLower() == typeof(GT_QMMA_B).Name.ToLower());
				if (tbl == null)
					return null;

				return Select(tbl, sapConnection);
			}

			public static List<GT_QMMA_B> ToList(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				return Select(dts, sapConnection).ToListOrEmptyList();
			}

			public static List<GT_QMMA_B> ToList(ISapDataService sapDataService)
			{
				return ToList(sapDataService.GetExportTables(), sapDataService.SapConnection);
			}

			public static List<GT_QMMA_B> GetExportListWithInitExecute(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<GT_QMMA_B>();
				 
				var dts = sapDataService.GetExportTablesWithInitExecute("Z_DPM_REM_FAHRZEUGHIST_02", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_QMMA_B> GetExportListWithExecute(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_QMMA_B>();
				 
				var dts = sapDataService.GetExportTablesWithExecute();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_QMMA_B> GetExportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_QMMA_B>();
				 
				var dts = sapDataService.GetExportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_QMMA_B> GetImportListWithInit(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<GT_QMMA_B>();
				 
				var dts = sapDataService.GetImportTablesWithInit("Z_DPM_REM_FAHRZEUGHIST_02", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_QMMA_B> GetImportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_QMMA_B>();
				 
				var dts = sapDataService.GetImportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}
		}

		public partial class GT_RECHNG : IModelMappingApplied
		{
			[SapIgnore]
			[ScriptIgnore]
			public ISapConnection SAPConnection { get; set; }

			[SapIgnore]
			[ScriptIgnore]
			public IDynSapProxyFactory DynSapProxyFactory { get; set; }

			public string MANDT { get; set; }

			public string KUNNR { get; set; }

			public string AVNR { get; set; }

			public string RENNR { get; set; }

			public DateTime? REDAT { get; set; }

			public DateTime? ERDAT { get; set; }

			public decimal? NETWR { get; set; }

			public decimal? UMSST { get; set; }

			public string GUTFG { get; set; }

			public string REFNR { get; set; }

			public string KTEXT { get; set; }

			public string VWDEBNR { get; set; }

			public decimal? ENDSUMME { get; set; }

			public string REFIN { get; set; }

			public string BELART { get; set; }

			public string EMPFAENGER { get; set; }

			public DateTime? RUECKK_PDF_DATUM { get; set; }

			public string RUECKK_FIN { get; set; }

			public DateTime? MERKANTILER { get; set; }

			public string STORNO { get; set; }

			public DateTime? DATUM_STORNO { get; set; }

			public string USER_STORNO { get; set; }

			public string TEXT_STORNO { get; set; }

			public DateTime? DAT_NEW_RE_GU_ERZ { get; set; }

			public string FLAG_MIETFZG { get; set; }

			public string FLAG_MEHRKM { get; set; }

			public decimal? SUMMEKMWRT { get; set; }

			private bool MappingErrorProcessed { get; set; }

			public static GT_RECHNG Create(DataRow row, ISapConnection sapConnection = null, IDynSapProxyFactory dynSapProxyFactory = null)
			{
				GT_RECHNG o;

				try
				{
					o = new GT_RECHNG
					{
						SAPConnection = sapConnection,
						DynSapProxyFactory = dynSapProxyFactory,

						MANDT = (string)row["MANDT"],
						KUNNR = (string)row["KUNNR"],
						AVNR = (string)row["AVNR"],
						RENNR = (string)row["RENNR"],
						REDAT = string.IsNullOrEmpty(row["REDAT"].ToString()) ? null : (DateTime?)row["REDAT"],
						ERDAT = string.IsNullOrEmpty(row["ERDAT"].ToString()) ? null : (DateTime?)row["ERDAT"],
						NETWR = string.IsNullOrEmpty(row["NETWR"].ToString()) ? null : (decimal?)row["NETWR"],
						UMSST = string.IsNullOrEmpty(row["UMSST"].ToString()) ? null : (decimal?)row["UMSST"],
						GUTFG = (string)row["GUTFG"],
						REFNR = (string)row["REFNR"],
						KTEXT = (string)row["KTEXT"],
						VWDEBNR = (string)row["VWDEBNR"],
						ENDSUMME = string.IsNullOrEmpty(row["ENDSUMME"].ToString()) ? null : (decimal?)row["ENDSUMME"],
						REFIN = (string)row["REFIN"],
						BELART = (string)row["BELART"],
						EMPFAENGER = (string)row["EMPFAENGER"],
						RUECKK_PDF_DATUM = string.IsNullOrEmpty(row["RUECKK_PDF_DATUM"].ToString()) ? null : (DateTime?)row["RUECKK_PDF_DATUM"],
						RUECKK_FIN = (string)row["RUECKK_FIN"],
						MERKANTILER = string.IsNullOrEmpty(row["MERKANTILER"].ToString()) ? null : (DateTime?)row["MERKANTILER"],
						STORNO = (string)row["STORNO"],
						DATUM_STORNO = string.IsNullOrEmpty(row["DATUM_STORNO"].ToString()) ? null : (DateTime?)row["DATUM_STORNO"],
						USER_STORNO = (string)row["USER_STORNO"],
						TEXT_STORNO = (string)row["TEXT_STORNO"],
						DAT_NEW_RE_GU_ERZ = string.IsNullOrEmpty(row["DAT_NEW_RE_GU_ERZ"].ToString()) ? null : (DateTime?)row["DAT_NEW_RE_GU_ERZ"],
						FLAG_MIETFZG = (string)row["FLAG_MIETFZG"],
						FLAG_MEHRKM = (string)row["FLAG_MEHRKM"],
						SUMMEKMWRT = string.IsNullOrEmpty(row["SUMMEKMWRT"].ToString()) ? null : (decimal?)row["SUMMEKMWRT"],
					};
				}
				catch(Exception e)
				{
					o = new GT_RECHNG
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

			public static IEnumerable<GT_RECHNG> Select(DataTable dt, ISapConnection sapConnection = null)
			{
				return dt.AsEnumerable().Select(r => Create(r, sapConnection));
			}

			public static List<GT_RECHNG> ToList(DataTable dt, ISapConnection sapConnection = null)
			{
				return Select(dt, sapConnection).ToListOrEmptyList();
			}

			public static IEnumerable<GT_RECHNG> Select(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				var tbl = dts.FirstOrDefault(t => t.TableName.ToLower() == typeof(GT_RECHNG).Name.ToLower());
				if (tbl == null)
					return null;

				return Select(tbl, sapConnection);
			}

			public static List<GT_RECHNG> ToList(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				return Select(dts, sapConnection).ToListOrEmptyList();
			}

			public static List<GT_RECHNG> ToList(ISapDataService sapDataService)
			{
				return ToList(sapDataService.GetExportTables(), sapDataService.SapConnection);
			}

			public static List<GT_RECHNG> GetExportListWithInitExecute(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<GT_RECHNG>();
				 
				var dts = sapDataService.GetExportTablesWithInitExecute("Z_DPM_REM_FAHRZEUGHIST_02", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_RECHNG> GetExportListWithExecute(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_RECHNG>();
				 
				var dts = sapDataService.GetExportTablesWithExecute();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_RECHNG> GetExportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_RECHNG>();
				 
				var dts = sapDataService.GetExportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_RECHNG> GetImportListWithInit(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<GT_RECHNG>();
				 
				var dts = sapDataService.GetImportTablesWithInit("Z_DPM_REM_FAHRZEUGHIST_02", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_RECHNG> GetImportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_RECHNG>();
				 
				var dts = sapDataService.GetImportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}
		}

		public partial class GT_SCHADEN : IModelMappingApplied
		{
			[SapIgnore]
			[ScriptIgnore]
			public ISapConnection SAPConnection { get; set; }

			[SapIgnore]
			[ScriptIgnore]
			public IDynSapProxyFactory DynSapProxyFactory { get; set; }

			public string MANDT { get; set; }

			public string KUNNR { get; set; }

			public string FAHRGNR { get; set; }

			public string KENNZ { get; set; }

			public string LFDNR { get; set; }

			public DateTime? ERDAT { get; set; }

			public decimal? PREIS { get; set; }

			public DateTime? SCHAD_DAT { get; set; }

			public string BESCHREIBUNG { get; set; }

			public string FILEKEY { get; set; }

			public string FLAGLAST { get; set; }

			public DateTime? DAT_UPD_VORSCH { get; set; }

			public string REPARIERT { get; set; }

			public decimal? WRTMBETR { get; set; }

			private bool MappingErrorProcessed { get; set; }

			public static GT_SCHADEN Create(DataRow row, ISapConnection sapConnection = null, IDynSapProxyFactory dynSapProxyFactory = null)
			{
				GT_SCHADEN o;

				try
				{
					o = new GT_SCHADEN
					{
						SAPConnection = sapConnection,
						DynSapProxyFactory = dynSapProxyFactory,

						MANDT = (string)row["MANDT"],
						KUNNR = (string)row["KUNNR"],
						FAHRGNR = (string)row["FAHRGNR"],
						KENNZ = (string)row["KENNZ"],
						LFDNR = (string)row["LFDNR"],
						ERDAT = string.IsNullOrEmpty(row["ERDAT"].ToString()) ? null : (DateTime?)row["ERDAT"],
						PREIS = string.IsNullOrEmpty(row["PREIS"].ToString()) ? null : (decimal?)row["PREIS"],
						SCHAD_DAT = string.IsNullOrEmpty(row["SCHAD_DAT"].ToString()) ? null : (DateTime?)row["SCHAD_DAT"],
						BESCHREIBUNG = (string)row["BESCHREIBUNG"],
						FILEKEY = (string)row["FILEKEY"],
						FLAGLAST = (string)row["FLAGLAST"],
						DAT_UPD_VORSCH = string.IsNullOrEmpty(row["DAT_UPD_VORSCH"].ToString()) ? null : (DateTime?)row["DAT_UPD_VORSCH"],
						REPARIERT = (string)row["REPARIERT"],
						WRTMBETR = string.IsNullOrEmpty(row["WRTMBETR"].ToString()) ? null : (decimal?)row["WRTMBETR"],
					};
				}
				catch(Exception e)
				{
					o = new GT_SCHADEN
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

			public static IEnumerable<GT_SCHADEN> Select(DataTable dt, ISapConnection sapConnection = null)
			{
				return dt.AsEnumerable().Select(r => Create(r, sapConnection));
			}

			public static List<GT_SCHADEN> ToList(DataTable dt, ISapConnection sapConnection = null)
			{
				return Select(dt, sapConnection).ToListOrEmptyList();
			}

			public static IEnumerable<GT_SCHADEN> Select(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				var tbl = dts.FirstOrDefault(t => t.TableName.ToLower() == typeof(GT_SCHADEN).Name.ToLower());
				if (tbl == null)
					return null;

				return Select(tbl, sapConnection);
			}

			public static List<GT_SCHADEN> ToList(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				return Select(dts, sapConnection).ToListOrEmptyList();
			}

			public static List<GT_SCHADEN> ToList(ISapDataService sapDataService)
			{
				return ToList(sapDataService.GetExportTables(), sapDataService.SapConnection);
			}

			public static List<GT_SCHADEN> GetExportListWithInitExecute(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<GT_SCHADEN>();
				 
				var dts = sapDataService.GetExportTablesWithInitExecute("Z_DPM_REM_FAHRZEUGHIST_02", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_SCHADEN> GetExportListWithExecute(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_SCHADEN>();
				 
				var dts = sapDataService.GetExportTablesWithExecute();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_SCHADEN> GetExportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_SCHADEN>();
				 
				var dts = sapDataService.GetExportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_SCHADEN> GetImportListWithInit(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<GT_SCHADEN>();
				 
				var dts = sapDataService.GetImportTablesWithInit("Z_DPM_REM_FAHRZEUGHIST_02", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_SCHADEN> GetImportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_SCHADEN>();
				 
				var dts = sapDataService.GetImportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}
		}

		public partial class GT_VERS : IModelMappingApplied
		{
			[SapIgnore]
			[ScriptIgnore]
			public ISapConnection SAPConnection { get; set; }

			[SapIgnore]
			[ScriptIgnore]
			public IDynSapProxyFactory DynSapProxyFactory { get; set; }

			public string CHASSIS_NUM { get; set; }

			public DateTime? DAT_IMP { get; set; }

			public string ZEIT_IMP { get; set; }

			public string STORNO { get; set; }

			public string TIDNR { get; set; }

			public string BELNR { get; set; }

			public string BETRAG_RE { get; set; }

			public DateTime? BELDT { get; set; }

			public DateTime? VALDT { get; set; }

			public DateTime? RELDT { get; set; }

			public string DZLART { get; set; }

			public string LICENSE_NUM { get; set; }

			public string KUNNR_ZF { get; set; }

			public string RDEALER { get; set; }

			public string NAME1_ZF { get; set; }

			public string NAME2_ZF { get; set; }

			public string NAME3_ZF { get; set; }

			public string STREET_ZF { get; set; }

			public string POST_CODE1_ZF { get; set; }

			public string CITY1_ZF { get; set; }

			public string LAND_CODE_ZF { get; set; }

			public string LAND_BEZ_ZF { get; set; }

			public string KUNNR_BANK { get; set; }

			public string NAME1_BANK { get; set; }

			public string NAME2_BANK { get; set; }

			public string NAME3_BANK { get; set; }

			public string STREET_BANK { get; set; }

			public string POST_CODE1_BANK { get; set; }

			public string CITY1_BANK { get; set; }

			public string LAND_CODE_BANK { get; set; }

			public string LAND_BEZ_BANK { get; set; }

			public DateTime? B_VERSAUFTR_DAT { get; set; }

			public DateTime? T_VERSAUFTR_DAT { get; set; }

			public string MATNR { get; set; }

			public string VERSANDSPERR { get; set; }

			public string WEB_USER { get; set; }

			private bool MappingErrorProcessed { get; set; }

			public static GT_VERS Create(DataRow row, ISapConnection sapConnection = null, IDynSapProxyFactory dynSapProxyFactory = null)
			{
				GT_VERS o;

				try
				{
					o = new GT_VERS
					{
						SAPConnection = sapConnection,
						DynSapProxyFactory = dynSapProxyFactory,

						CHASSIS_NUM = (string)row["CHASSIS_NUM"],
						DAT_IMP = string.IsNullOrEmpty(row["DAT_IMP"].ToString()) ? null : (DateTime?)row["DAT_IMP"],
						ZEIT_IMP = (string)row["ZEIT_IMP"],
						STORNO = (string)row["STORNO"],
						TIDNR = (string)row["TIDNR"],
						BELNR = (string)row["BELNR"],
						BETRAG_RE = (string)row["BETRAG_RE"],
						BELDT = string.IsNullOrEmpty(row["BELDT"].ToString()) ? null : (DateTime?)row["BELDT"],
						VALDT = string.IsNullOrEmpty(row["VALDT"].ToString()) ? null : (DateTime?)row["VALDT"],
						RELDT = string.IsNullOrEmpty(row["RELDT"].ToString()) ? null : (DateTime?)row["RELDT"],
						DZLART = (string)row["DZLART"],
						LICENSE_NUM = (string)row["LICENSE_NUM"],
						KUNNR_ZF = (string)row["KUNNR_ZF"],
						RDEALER = (string)row["RDEALER"],
						NAME1_ZF = (string)row["NAME1_ZF"],
						NAME2_ZF = (string)row["NAME2_ZF"],
						NAME3_ZF = (string)row["NAME3_ZF"],
						STREET_ZF = (string)row["STREET_ZF"],
						POST_CODE1_ZF = (string)row["POST_CODE1_ZF"],
						CITY1_ZF = (string)row["CITY1_ZF"],
						LAND_CODE_ZF = (string)row["LAND_CODE_ZF"],
						LAND_BEZ_ZF = (string)row["LAND_BEZ_ZF"],
						KUNNR_BANK = (string)row["KUNNR_BANK"],
						NAME1_BANK = (string)row["NAME1_BANK"],
						NAME2_BANK = (string)row["NAME2_BANK"],
						NAME3_BANK = (string)row["NAME3_BANK"],
						STREET_BANK = (string)row["STREET_BANK"],
						POST_CODE1_BANK = (string)row["POST_CODE1_BANK"],
						CITY1_BANK = (string)row["CITY1_BANK"],
						LAND_CODE_BANK = (string)row["LAND_CODE_BANK"],
						LAND_BEZ_BANK = (string)row["LAND_BEZ_BANK"],
						B_VERSAUFTR_DAT = string.IsNullOrEmpty(row["B_VERSAUFTR_DAT"].ToString()) ? null : (DateTime?)row["B_VERSAUFTR_DAT"],
						T_VERSAUFTR_DAT = string.IsNullOrEmpty(row["T_VERSAUFTR_DAT"].ToString()) ? null : (DateTime?)row["T_VERSAUFTR_DAT"],
						MATNR = (string)row["MATNR"],
						VERSANDSPERR = (string)row["VERSANDSPERR"],
						WEB_USER = (string)row["WEB_USER"],
					};
				}
				catch(Exception e)
				{
					o = new GT_VERS
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

			public static IEnumerable<GT_VERS> Select(DataTable dt, ISapConnection sapConnection = null)
			{
				return dt.AsEnumerable().Select(r => Create(r, sapConnection));
			}

			public static List<GT_VERS> ToList(DataTable dt, ISapConnection sapConnection = null)
			{
				return Select(dt, sapConnection).ToListOrEmptyList();
			}

			public static IEnumerable<GT_VERS> Select(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				var tbl = dts.FirstOrDefault(t => t.TableName.ToLower() == typeof(GT_VERS).Name.ToLower());
				if (tbl == null)
					return null;

				return Select(tbl, sapConnection);
			}

			public static List<GT_VERS> ToList(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				return Select(dts, sapConnection).ToListOrEmptyList();
			}

			public static List<GT_VERS> ToList(ISapDataService sapDataService)
			{
				return ToList(sapDataService.GetExportTables(), sapDataService.SapConnection);
			}

			public static List<GT_VERS> GetExportListWithInitExecute(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<GT_VERS>();
				 
				var dts = sapDataService.GetExportTablesWithInitExecute("Z_DPM_REM_FAHRZEUGHIST_02", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_VERS> GetExportListWithExecute(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_VERS>();
				 
				var dts = sapDataService.GetExportTablesWithExecute();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_VERS> GetExportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_VERS>();
				 
				var dts = sapDataService.GetExportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_VERS> GetImportListWithInit(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<GT_VERS>();
				 
				var dts = sapDataService.GetImportTablesWithInit("Z_DPM_REM_FAHRZEUGHIST_02", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_VERS> GetImportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_VERS>();
				 
				var dts = sapDataService.GetImportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}
		}
	}

	public static partial class DataTableExtensions
	{

		public static DataTable ToTable(this IEnumerable<Z_DPM_REM_FAHRZEUGHIST_02.GT_ADDR_B> list)
		{
			return SapDataServiceExtensions.ToTable(list);
		}


		public static DataTable ToTable(this IEnumerable<Z_DPM_REM_FAHRZEUGHIST_02.GT_AUSST> list)
		{
			return SapDataServiceExtensions.ToTable(list);
		}


		public static DataTable ToTable(this IEnumerable<Z_DPM_REM_FAHRZEUGHIST_02.GT_BELAS> list)
		{
			return SapDataServiceExtensions.ToTable(list);
		}


		public static DataTable ToTable(this IEnumerable<Z_DPM_REM_FAHRZEUGHIST_02.GT_DATEN> list)
		{
			return SapDataServiceExtensions.ToTable(list);
		}


		public static DataTable ToTable(this IEnumerable<Z_DPM_REM_FAHRZEUGHIST_02.GT_DATEN2> list)
		{
			return SapDataServiceExtensions.ToTable(list);
		}


		public static DataTable ToTable(this IEnumerable<Z_DPM_REM_FAHRZEUGHIST_02.GT_EQUI_B> list)
		{
			return SapDataServiceExtensions.ToTable(list);
		}


		public static DataTable ToTable(this IEnumerable<Z_DPM_REM_FAHRZEUGHIST_02.GT_GUTA> list)
		{
			return SapDataServiceExtensions.ToTable(list);
		}


		public static DataTable ToTable(this IEnumerable<Z_DPM_REM_FAHRZEUGHIST_02.GT_LEB_B> list)
		{
			return SapDataServiceExtensions.ToTable(list);
		}


		public static DataTable ToTable(this IEnumerable<Z_DPM_REM_FAHRZEUGHIST_02.GT_LEB_T> list)
		{
			return SapDataServiceExtensions.ToTable(list);
		}


		public static DataTable ToTable(this IEnumerable<Z_DPM_REM_FAHRZEUGHIST_02.GT_QMEL_B> list)
		{
			return SapDataServiceExtensions.ToTable(list);
		}


		public static DataTable ToTable(this IEnumerable<Z_DPM_REM_FAHRZEUGHIST_02.GT_QMMA_B> list)
		{
			return SapDataServiceExtensions.ToTable(list);
		}


		public static DataTable ToTable(this IEnumerable<Z_DPM_REM_FAHRZEUGHIST_02.GT_RECHNG> list)
		{
			return SapDataServiceExtensions.ToTable(list);
		}


		public static DataTable ToTable(this IEnumerable<Z_DPM_REM_FAHRZEUGHIST_02.GT_SCHADEN> list)
		{
			return SapDataServiceExtensions.ToTable(list);
		}


		public static DataTable ToTable(this IEnumerable<Z_DPM_REM_FAHRZEUGHIST_02.GT_VERS> list)
		{
			return SapDataServiceExtensions.ToTable(list);
		}

	}
}
