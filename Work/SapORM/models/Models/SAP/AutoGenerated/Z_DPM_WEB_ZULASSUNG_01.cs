using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Script.Serialization;
using GeneralTools.Contracts;
using SapORM.Contracts;

namespace SapORM.Models
{
	public partial class Z_DPM_WEB_ZULASSUNG_01
	{
		public static void Init(ISapDataService sap)
		{
			sap.Init(typeof(Z_DPM_WEB_ZULASSUNG_01).Name);
		}

		public static void Init(ISapDataService sap, string inputParameterKeys, params object[] inputParameterValues)
		{
			sap.Init(typeof(Z_DPM_WEB_ZULASSUNG_01).Name, inputParameterKeys, inputParameterValues);
		}

		public partial class GT_AUF : IModelMappingApplied
		{
			[SapIgnore]
			[ScriptIgnore]
			public ISapConnection SAPConnection { get; set; }

			[SapIgnore]
			[ScriptIgnore]
			public IDynSapProxyFactory DynSapProxyFactory { get; set; }

			public string ZZFAHRG { get; set; }

			public string ZZBRIEF { get; set; }

			public string ZZREFNR { get; set; }

			public DateTime? ZULDAT { get; set; }

			public string EVBNR { get; set; }

			public DateTime? EVBVONDAT { get; set; }

			public DateTime? EVBBISDAT { get; set; }

			public string WUNSCHKENNZ { get; set; }

			public string VERSICHERUNG { get; set; }

			public string TERMINHINWEIS { get; set; }

			public string FEINSTAUBPL { get; set; }

			public string STEUERN { get; set; }

			public string EXKUNNR_ZL { get; set; }

			public string KVGR3 { get; set; }

			public string EQUNR { get; set; }

			public string VERBVBELN { get; set; }

			public string IHREZ_E { get; set; }

			public string SFV_FZG { get; set; }

			public string ZFAHRZEUGART { get; set; }

			public string ZUL_DEZ { get; set; }

			public string ZUL_AUSLAND { get; set; }

			public string ZUL_EXPORT { get; set; }

			public static GT_AUF Create(DataRow row, ISapConnection sapConnection = null, IDynSapProxyFactory dynSapProxyFactory = null)
			{
				var o = new GT_AUF
				{
					ZZFAHRG = (string)row["ZZFAHRG"],
					ZZBRIEF = (string)row["ZZBRIEF"],
					ZZREFNR = (string)row["ZZREFNR"],
					ZULDAT = (string.IsNullOrEmpty(row["ZULDAT"].ToString())) ? null : (DateTime?)row["ZULDAT"],
					EVBNR = (string)row["EVBNR"],
					EVBVONDAT = (string.IsNullOrEmpty(row["EVBVONDAT"].ToString())) ? null : (DateTime?)row["EVBVONDAT"],
					EVBBISDAT = (string.IsNullOrEmpty(row["EVBBISDAT"].ToString())) ? null : (DateTime?)row["EVBBISDAT"],
					WUNSCHKENNZ = (string)row["WUNSCHKENNZ"],
					VERSICHERUNG = (string)row["VERSICHERUNG"],
					TERMINHINWEIS = (string)row["TERMINHINWEIS"],
					FEINSTAUBPL = (string)row["FEINSTAUBPL"],
					STEUERN = (string)row["STEUERN"],
					EXKUNNR_ZL = (string)row["EXKUNNR_ZL"],
					KVGR3 = (string)row["KVGR3"],
					EQUNR = (string)row["EQUNR"],
					VERBVBELN = (string)row["VERBVBELN"],
					IHREZ_E = (string)row["IHREZ_E"],
					SFV_FZG = (string)row["SFV_FZG"],
					ZFAHRZEUGART = (string)row["ZFAHRZEUGART"],
					ZUL_DEZ = (string)row["ZUL_DEZ"],
					ZUL_AUSLAND = (string)row["ZUL_AUSLAND"],
					ZUL_EXPORT = (string)row["ZUL_EXPORT"],

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

			public static IEnumerable<GT_AUF> Select(DataTable dt, ISapConnection sapConnection = null)
			{
				return dt.AsEnumerable().Select(r => Create(r, sapConnection));
			}

			public static List<GT_AUF> ToList(DataTable dt, ISapConnection sapConnection = null)
			{
				return Select(dt, sapConnection).ToList();
			}

			public static IEnumerable<GT_AUF> Select(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				var tbl = dts.FirstOrDefault(t => t.TableName.ToLower() == typeof(GT_AUF).Name.ToLower());
				if (tbl == null)
					return null;

				return Select(tbl, sapConnection);
			}

			public static List<GT_AUF> ToList(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				return Select(dts, sapConnection).ToList();
			}

			public static List<GT_AUF> ToList(ISapDataService sapDataService)
			{
				return ToList(sapDataService.GetExportTables(), sapDataService.SapConnection);
			}

			public static List<GT_AUF> GetExportListWithInitExecute(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<GT_AUF>();
				 
				var dts = sapDataService.GetExportTablesWithInitExecute("Z_DPM_WEB_ZULASSUNG_01", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToList();
			}

			public static List<GT_AUF> GetExportListWithExecute(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_AUF>();
				 
				var dts = sapDataService.GetExportTablesWithExecute();
				 
				return Select(dts, sapDataService.SapConnection).ToList();
			}

			public static List<GT_AUF> GetExportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_AUF>();
				 
				var dts = sapDataService.GetExportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToList();
			}

			public static List<GT_AUF> GetImportListWithInit(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<GT_AUF>();
				 
				var dts = sapDataService.GetImportTablesWithInit("Z_DPM_WEB_ZULASSUNG_01", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToList();
			}

			public static List<GT_AUF> GetImportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_AUF>();
				 
				var dts = sapDataService.GetImportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToList();
			}
		}

		public partial class GT_PARTNER : IModelMappingApplied
		{
			[SapIgnore]
			[ScriptIgnore]
			public ISapConnection SAPConnection { get; set; }

			[SapIgnore]
			[ScriptIgnore]
			public IDynSapProxyFactory DynSapProxyFactory { get; set; }

			public string ZZFAHRG { get; set; }

			public string ZZBRIEF { get; set; }

			public string ZZREFNR { get; set; }

			public string PARTN_ROLE { get; set; }

			public string PARTN_NUMB { get; set; }

			public string ITM_NUMBER { get; set; }

			public string TITLE { get; set; }

			public string NAME { get; set; }

			public string NAME_2 { get; set; }

			public string NAME_3 { get; set; }

			public string NAME_4 { get; set; }

			public string STREET { get; set; }

			public string COUNTRY { get; set; }

			public string COUNTR_ISO { get; set; }

			public string POSTL_CODE { get; set; }

			public string POBX_PCD { get; set; }

			public string POBX_CTY { get; set; }

			public string CITY { get; set; }

			public string DISTRICT { get; set; }

			public string REGION { get; set; }

			public string PO_BOX { get; set; }

			public string TELEPHONE { get; set; }

			public string TELEPHONE2 { get; set; }

			public string TELEBOX { get; set; }

			public string FAX_NUMBER { get; set; }

			public string TELETEX_NO { get; set; }

			public string TELEX_NO { get; set; }

			public string LANGU { get; set; }

			public string LANGU_ISO { get; set; }

			public string UNLOAD_PT { get; set; }

			public string TRANSPZONE { get; set; }

			public string TAXJURCODE { get; set; }

			public string ADDRESS { get; set; }

			public string PRIV_ADDR { get; set; }

			public string ADDR_TYPE { get; set; }

			public string ADDR_ORIG { get; set; }

			public string ADDR_LINK { get; set; }

			public string REFOBJTYPE { get; set; }

			public string REFOBJKEY { get; set; }

			public string REFLOGSYS { get; set; }

			public string SMTP_ADDR { get; set; }

			public string FLGDEFAULT { get; set; }

			public static GT_PARTNER Create(DataRow row, ISapConnection sapConnection = null, IDynSapProxyFactory dynSapProxyFactory = null)
			{
				var o = new GT_PARTNER
				{
					ZZFAHRG = (string)row["ZZFAHRG"],
					ZZBRIEF = (string)row["ZZBRIEF"],
					ZZREFNR = (string)row["ZZREFNR"],
					PARTN_ROLE = (string)row["PARTN_ROLE"],
					PARTN_NUMB = (string)row["PARTN_NUMB"],
					ITM_NUMBER = (string)row["ITM_NUMBER"],
					TITLE = (string)row["TITLE"],
					NAME = (string)row["NAME"],
					NAME_2 = (string)row["NAME_2"],
					NAME_3 = (string)row["NAME_3"],
					NAME_4 = (string)row["NAME_4"],
					STREET = (string)row["STREET"],
					COUNTRY = (string)row["COUNTRY"],
					COUNTR_ISO = (string)row["COUNTR_ISO"],
					POSTL_CODE = (string)row["POSTL_CODE"],
					POBX_PCD = (string)row["POBX_PCD"],
					POBX_CTY = (string)row["POBX_CTY"],
					CITY = (string)row["CITY"],
					DISTRICT = (string)row["DISTRICT"],
					REGION = (string)row["REGION"],
					PO_BOX = (string)row["PO_BOX"],
					TELEPHONE = (string)row["TELEPHONE"],
					TELEPHONE2 = (string)row["TELEPHONE2"],
					TELEBOX = (string)row["TELEBOX"],
					FAX_NUMBER = (string)row["FAX_NUMBER"],
					TELETEX_NO = (string)row["TELETEX_NO"],
					TELEX_NO = (string)row["TELEX_NO"],
					LANGU = (string)row["LANGU"],
					LANGU_ISO = (string)row["LANGU_ISO"],
					UNLOAD_PT = (string)row["UNLOAD_PT"],
					TRANSPZONE = (string)row["TRANSPZONE"],
					TAXJURCODE = (string)row["TAXJURCODE"],
					ADDRESS = (string)row["ADDRESS"],
					PRIV_ADDR = (string)row["PRIV_ADDR"],
					ADDR_TYPE = (string)row["ADDR_TYPE"],
					ADDR_ORIG = (string)row["ADDR_ORIG"],
					ADDR_LINK = (string)row["ADDR_LINK"],
					REFOBJTYPE = (string)row["REFOBJTYPE"],
					REFOBJKEY = (string)row["REFOBJKEY"],
					REFLOGSYS = (string)row["REFLOGSYS"],
					SMTP_ADDR = (string)row["SMTP_ADDR"],
					FLGDEFAULT = (string)row["FLGDEFAULT"],

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

			public static IEnumerable<GT_PARTNER> Select(DataTable dt, ISapConnection sapConnection = null)
			{
				return dt.AsEnumerable().Select(r => Create(r, sapConnection));
			}

			public static List<GT_PARTNER> ToList(DataTable dt, ISapConnection sapConnection = null)
			{
				return Select(dt, sapConnection).ToList();
			}

			public static IEnumerable<GT_PARTNER> Select(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				var tbl = dts.FirstOrDefault(t => t.TableName.ToLower() == typeof(GT_PARTNER).Name.ToLower());
				if (tbl == null)
					return null;

				return Select(tbl, sapConnection);
			}

			public static List<GT_PARTNER> ToList(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				return Select(dts, sapConnection).ToList();
			}

			public static List<GT_PARTNER> ToList(ISapDataService sapDataService)
			{
				return ToList(sapDataService.GetExportTables(), sapDataService.SapConnection);
			}

			public static List<GT_PARTNER> GetExportListWithInitExecute(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<GT_PARTNER>();
				 
				var dts = sapDataService.GetExportTablesWithInitExecute("Z_DPM_WEB_ZULASSUNG_01", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToList();
			}

			public static List<GT_PARTNER> GetExportListWithExecute(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_PARTNER>();
				 
				var dts = sapDataService.GetExportTablesWithExecute();
				 
				return Select(dts, sapDataService.SapConnection).ToList();
			}

			public static List<GT_PARTNER> GetExportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_PARTNER>();
				 
				var dts = sapDataService.GetExportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToList();
			}

			public static List<GT_PARTNER> GetImportListWithInit(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<GT_PARTNER>();
				 
				var dts = sapDataService.GetImportTablesWithInit("Z_DPM_WEB_ZULASSUNG_01", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToList();
			}

			public static List<GT_PARTNER> GetImportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_PARTNER>();
				 
				var dts = sapDataService.GetImportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToList();
			}
		}

		public partial class GT_DIENSTL : IModelMappingApplied
		{
			[SapIgnore]
			[ScriptIgnore]
			public ISapConnection SAPConnection { get; set; }

			[SapIgnore]
			[ScriptIgnore]
			public IDynSapProxyFactory DynSapProxyFactory { get; set; }

			public string ZZFAHRG { get; set; }

			public string ZZBRIEF { get; set; }

			public string ZZREFNR { get; set; }

			public string DIENSTL_NR { get; set; }

			public string DIENSTL_TEXT { get; set; }

			public string MATNR { get; set; }

			public string FLAG_TEXT { get; set; }

			public string AUSWAHL_NR { get; set; }

			public string AUSWAHL_TEXT { get; set; }

			public string HAUPT_DL { get; set; }

			public static GT_DIENSTL Create(DataRow row, ISapConnection sapConnection = null, IDynSapProxyFactory dynSapProxyFactory = null)
			{
				var o = new GT_DIENSTL
				{
					ZZFAHRG = (string)row["ZZFAHRG"],
					ZZBRIEF = (string)row["ZZBRIEF"],
					ZZREFNR = (string)row["ZZREFNR"],
					DIENSTL_NR = (string)row["DIENSTL_NR"],
					DIENSTL_TEXT = (string)row["DIENSTL_TEXT"],
					MATNR = (string)row["MATNR"],
					FLAG_TEXT = (string)row["FLAG_TEXT"],
					AUSWAHL_NR = (string)row["AUSWAHL_NR"],
					AUSWAHL_TEXT = (string)row["AUSWAHL_TEXT"],
					HAUPT_DL = (string)row["HAUPT_DL"],

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

			public static IEnumerable<GT_DIENSTL> Select(DataTable dt, ISapConnection sapConnection = null)
			{
				return dt.AsEnumerable().Select(r => Create(r, sapConnection));
			}

			public static List<GT_DIENSTL> ToList(DataTable dt, ISapConnection sapConnection = null)
			{
				return Select(dt, sapConnection).ToList();
			}

			public static IEnumerable<GT_DIENSTL> Select(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				var tbl = dts.FirstOrDefault(t => t.TableName.ToLower() == typeof(GT_DIENSTL).Name.ToLower());
				if (tbl == null)
					return null;

				return Select(tbl, sapConnection);
			}

			public static List<GT_DIENSTL> ToList(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				return Select(dts, sapConnection).ToList();
			}

			public static List<GT_DIENSTL> ToList(ISapDataService sapDataService)
			{
				return ToList(sapDataService.GetExportTables(), sapDataService.SapConnection);
			}

			public static List<GT_DIENSTL> GetExportListWithInitExecute(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<GT_DIENSTL>();
				 
				var dts = sapDataService.GetExportTablesWithInitExecute("Z_DPM_WEB_ZULASSUNG_01", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToList();
			}

			public static List<GT_DIENSTL> GetExportListWithExecute(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_DIENSTL>();
				 
				var dts = sapDataService.GetExportTablesWithExecute();
				 
				return Select(dts, sapDataService.SapConnection).ToList();
			}

			public static List<GT_DIENSTL> GetExportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_DIENSTL>();
				 
				var dts = sapDataService.GetExportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToList();
			}

			public static List<GT_DIENSTL> GetImportListWithInit(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<GT_DIENSTL>();
				 
				var dts = sapDataService.GetImportTablesWithInit("Z_DPM_WEB_ZULASSUNG_01", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToList();
			}

			public static List<GT_DIENSTL> GetImportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_DIENSTL>();
				 
				var dts = sapDataService.GetImportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToList();
			}
		}

		public partial class GT_DOK : IModelMappingApplied
		{
			[SapIgnore]
			[ScriptIgnore]
			public ISapConnection SAPConnection { get; set; }

			[SapIgnore]
			[ScriptIgnore]
			public IDynSapProxyFactory DynSapProxyFactory { get; set; }

			public string ZZFAHRG { get; set; }

			public string ZZBRIEF { get; set; }

			public string ZZREFNR { get; set; }

			public string DOKUMENTENART { get; set; }

			public static GT_DOK Create(DataRow row, ISapConnection sapConnection = null, IDynSapProxyFactory dynSapProxyFactory = null)
			{
				var o = new GT_DOK
				{
					ZZFAHRG = (string)row["ZZFAHRG"],
					ZZBRIEF = (string)row["ZZBRIEF"],
					ZZREFNR = (string)row["ZZREFNR"],
					DOKUMENTENART = (string)row["DOKUMENTENART"],

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

			public static IEnumerable<GT_DOK> Select(DataTable dt, ISapConnection sapConnection = null)
			{
				return dt.AsEnumerable().Select(r => Create(r, sapConnection));
			}

			public static List<GT_DOK> ToList(DataTable dt, ISapConnection sapConnection = null)
			{
				return Select(dt, sapConnection).ToList();
			}

			public static IEnumerable<GT_DOK> Select(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				var tbl = dts.FirstOrDefault(t => t.TableName.ToLower() == typeof(GT_DOK).Name.ToLower());
				if (tbl == null)
					return null;

				return Select(tbl, sapConnection);
			}

			public static List<GT_DOK> ToList(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				return Select(dts, sapConnection).ToList();
			}

			public static List<GT_DOK> ToList(ISapDataService sapDataService)
			{
				return ToList(sapDataService.GetExportTables(), sapDataService.SapConnection);
			}

			public static List<GT_DOK> GetExportListWithInitExecute(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<GT_DOK>();
				 
				var dts = sapDataService.GetExportTablesWithInitExecute("Z_DPM_WEB_ZULASSUNG_01", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToList();
			}

			public static List<GT_DOK> GetExportListWithExecute(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_DOK>();
				 
				var dts = sapDataService.GetExportTablesWithExecute();
				 
				return Select(dts, sapDataService.SapConnection).ToList();
			}

			public static List<GT_DOK> GetExportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_DOK>();
				 
				var dts = sapDataService.GetExportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToList();
			}

			public static List<GT_DOK> GetImportListWithInit(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<GT_DOK>();
				 
				var dts = sapDataService.GetImportTablesWithInit("Z_DPM_WEB_ZULASSUNG_01", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToList();
			}

			public static List<GT_DOK> GetImportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_DOK>();
				 
				var dts = sapDataService.GetImportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToList();
			}
		}

		public partial class GT_RETURN : IModelMappingApplied
		{
			[SapIgnore]
			[ScriptIgnore]
			public ISapConnection SAPConnection { get; set; }

			[SapIgnore]
			[ScriptIgnore]
			public IDynSapProxyFactory DynSapProxyFactory { get; set; }

			public string ZZFAHRG { get; set; }

			public string ZZBRIEF { get; set; }

			public string ZZREFNR { get; set; }

			public string VBELN { get; set; }

			public string MESSAGE { get; set; }

			public static GT_RETURN Create(DataRow row, ISapConnection sapConnection = null, IDynSapProxyFactory dynSapProxyFactory = null)
			{
				var o = new GT_RETURN
				{
					ZZFAHRG = (string)row["ZZFAHRG"],
					ZZBRIEF = (string)row["ZZBRIEF"],
					ZZREFNR = (string)row["ZZREFNR"],
					VBELN = (string)row["VBELN"],
					MESSAGE = (string)row["MESSAGE"],

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

			public static IEnumerable<GT_RETURN> Select(DataTable dt, ISapConnection sapConnection = null)
			{
				return dt.AsEnumerable().Select(r => Create(r, sapConnection));
			}

			public static List<GT_RETURN> ToList(DataTable dt, ISapConnection sapConnection = null)
			{
				return Select(dt, sapConnection).ToList();
			}

			public static IEnumerable<GT_RETURN> Select(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				var tbl = dts.FirstOrDefault(t => t.TableName.ToLower() == typeof(GT_RETURN).Name.ToLower());
				if (tbl == null)
					return null;

				return Select(tbl, sapConnection);
			}

			public static List<GT_RETURN> ToList(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				return Select(dts, sapConnection).ToList();
			}

			public static List<GT_RETURN> ToList(ISapDataService sapDataService)
			{
				return ToList(sapDataService.GetExportTables(), sapDataService.SapConnection);
			}

			public static List<GT_RETURN> GetExportListWithInitExecute(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<GT_RETURN>();
				 
				var dts = sapDataService.GetExportTablesWithInitExecute("Z_DPM_WEB_ZULASSUNG_01", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToList();
			}

			public static List<GT_RETURN> GetExportListWithExecute(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_RETURN>();
				 
				var dts = sapDataService.GetExportTablesWithExecute();
				 
				return Select(dts, sapDataService.SapConnection).ToList();
			}

			public static List<GT_RETURN> GetExportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_RETURN>();
				 
				var dts = sapDataService.GetExportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToList();
			}

			public static List<GT_RETURN> GetImportListWithInit(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<GT_RETURN>();
				 
				var dts = sapDataService.GetImportTablesWithInit("Z_DPM_WEB_ZULASSUNG_01", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToList();
			}

			public static List<GT_RETURN> GetImportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_RETURN>();
				 
				var dts = sapDataService.GetImportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToList();
			}
		}
	}

	public static partial class DataTableExtensions
	{

		public static DataTable ToTable(this IEnumerable<Z_DPM_WEB_ZULASSUNG_01.GT_AUF> list)
		{
			return SapDataServiceExtensions.ToTable(list);
		}

		public static void Apply(this IEnumerable<Z_DPM_WEB_ZULASSUNG_01.GT_AUF> list, DataTable dtDst)
		{
			SapDataServiceExtensions.Apply(list, dtDst);
		}


		public static DataTable ToTable(this IEnumerable<Z_DPM_WEB_ZULASSUNG_01.GT_PARTNER> list)
		{
			return SapDataServiceExtensions.ToTable(list);
		}

		public static void Apply(this IEnumerable<Z_DPM_WEB_ZULASSUNG_01.GT_PARTNER> list, DataTable dtDst)
		{
			SapDataServiceExtensions.Apply(list, dtDst);
		}


		public static DataTable ToTable(this IEnumerable<Z_DPM_WEB_ZULASSUNG_01.GT_DIENSTL> list)
		{
			return SapDataServiceExtensions.ToTable(list);
		}

		public static void Apply(this IEnumerable<Z_DPM_WEB_ZULASSUNG_01.GT_DIENSTL> list, DataTable dtDst)
		{
			SapDataServiceExtensions.Apply(list, dtDst);
		}


		public static DataTable ToTable(this IEnumerable<Z_DPM_WEB_ZULASSUNG_01.GT_DOK> list)
		{
			return SapDataServiceExtensions.ToTable(list);
		}

		public static void Apply(this IEnumerable<Z_DPM_WEB_ZULASSUNG_01.GT_DOK> list, DataTable dtDst)
		{
			SapDataServiceExtensions.Apply(list, dtDst);
		}


		public static DataTable ToTable(this IEnumerable<Z_DPM_WEB_ZULASSUNG_01.GT_RETURN> list)
		{
			return SapDataServiceExtensions.ToTable(list);
		}

		public static void Apply(this IEnumerable<Z_DPM_WEB_ZULASSUNG_01.GT_RETURN> list, DataTable dtDst)
		{
			SapDataServiceExtensions.Apply(list, dtDst);
		}

	}
}
