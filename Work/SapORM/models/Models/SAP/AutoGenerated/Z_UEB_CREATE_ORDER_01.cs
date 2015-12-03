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
	public partial class Z_UEB_CREATE_ORDER_01
	{
		public static void Init(ISapDataService sap)
		{
			sap.Init(typeof(Z_UEB_CREATE_ORDER_01).Name);
		}

		public static void Init(ISapDataService sap, string inputParameterKeys, params object[] inputParameterValues)
		{
			sap.Init(typeof(Z_UEB_CREATE_ORDER_01).Name, inputParameterKeys, inputParameterValues);
		}


		public static void SetImportParameter_AG(ISapDataService sap, string value)
		{
			sap.SetImportParameter("AG", value);
		}

		public static void SetImportParameter_EMAIL_WEB_USER(ISapDataService sap, string value)
		{
			sap.SetImportParameter("EMAIL_WEB_USER", value);
		}

		public static void SetImportParameter_INFO_ZUM_AG(ISapDataService sap, string value)
		{
			sap.SetImportParameter("INFO_ZUM_AG", value);
		}

		public static void SetImportParameter_LIFSK(ISapDataService sap, string value)
		{
			sap.SetImportParameter("LIFSK", value);
		}

		public static void SetImportParameter_MATVKTXT(ISapDataService sap, string value)
		{
			sap.SetImportParameter("MATVKTXT", value);
		}

		public static void SetImportParameter_RE(ISapDataService sap, string value)
		{
			sap.SetImportParameter("RE", value);
		}

		public static void SetImportParameter_RG(ISapDataService sap, string value)
		{
			sap.SetImportParameter("RG", value);
		}

		public static void SetImportParameter_WEB_USER(ISapDataService sap, string value)
		{
			sap.SetImportParameter("WEB_USER", value);
		}

		public static void SetImportParameter_ZVERBVBELN(ISapDataService sap, string value)
		{
			sap.SetImportParameter("ZVERBVBELN", value);
		}

		public partial class GT_ADRESSEN : IModelMappingApplied
		{
			[SapIgnore]
			[ScriptIgnore]
			public ISapConnection SAPConnection { get; set; }

			[SapIgnore]
			[ScriptIgnore]
			public IDynSapProxyFactory DynSapProxyFactory { get; set; }

			public string FAHRT { get; set; }

			public string PARTN_NUMB { get; set; }

			public string NAME { get; set; }

			public string NAME_2 { get; set; }

			public string NAME_3 { get; set; }

			public string STREET { get; set; }

			public string POSTL_CODE { get; set; }

			public string CITY { get; set; }

			public string COUNTRY { get; set; }

			public string TELEPHONE { get; set; }

			public string FAX_NUMBER { get; set; }

			public string SMTP_ADDR { get; set; }

			public static GT_ADRESSEN Create(DataRow row, ISapConnection sapConnection = null, IDynSapProxyFactory dynSapProxyFactory = null)
			{
				var o = new GT_ADRESSEN
				{
					FAHRT = (string)row["FAHRT"],
					PARTN_NUMB = (string)row["PARTN_NUMB"],
					NAME = (string)row["NAME"],
					NAME_2 = (string)row["NAME_2"],
					NAME_3 = (string)row["NAME_3"],
					STREET = (string)row["STREET"],
					POSTL_CODE = (string)row["POSTL_CODE"],
					CITY = (string)row["CITY"],
					COUNTRY = (string)row["COUNTRY"],
					TELEPHONE = (string)row["TELEPHONE"],
					FAX_NUMBER = (string)row["FAX_NUMBER"],
					SMTP_ADDR = (string)row["SMTP_ADDR"],

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

			public static IEnumerable<GT_ADRESSEN> Select(DataTable dt, ISapConnection sapConnection = null)
			{
				return dt.AsEnumerable().Select(r => Create(r, sapConnection));
			}

			public static List<GT_ADRESSEN> ToList(DataTable dt, ISapConnection sapConnection = null)
			{
				return Select(dt, sapConnection).ToListOrEmptyList();
			}

			public static IEnumerable<GT_ADRESSEN> Select(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				var tbl = dts.FirstOrDefault(t => t.TableName.ToLower() == typeof(GT_ADRESSEN).Name.ToLower());
				if (tbl == null)
					return null;

				return Select(tbl, sapConnection);
			}

			public static List<GT_ADRESSEN> ToList(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				return Select(dts, sapConnection).ToListOrEmptyList();
			}

			public static List<GT_ADRESSEN> ToList(ISapDataService sapDataService)
			{
				return ToList(sapDataService.GetExportTables(), sapDataService.SapConnection);
			}

			public static List<GT_ADRESSEN> GetExportListWithInitExecute(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<GT_ADRESSEN>();
				 
				var dts = sapDataService.GetExportTablesWithInitExecute("Z_UEB_CREATE_ORDER_01", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_ADRESSEN> GetExportListWithExecute(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_ADRESSEN>();
				 
				var dts = sapDataService.GetExportTablesWithExecute();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_ADRESSEN> GetExportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_ADRESSEN>();
				 
				var dts = sapDataService.GetExportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_ADRESSEN> GetImportListWithInit(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<GT_ADRESSEN>();
				 
				var dts = sapDataService.GetImportTablesWithInit("Z_UEB_CREATE_ORDER_01", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_ADRESSEN> GetImportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_ADRESSEN>();
				 
				var dts = sapDataService.GetImportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}
		}

		public partial class GT_BEM : IModelMappingApplied
		{
			[SapIgnore]
			[ScriptIgnore]
			public ISapConnection SAPConnection { get; set; }

			[SapIgnore]
			[ScriptIgnore]
			public IDynSapProxyFactory DynSapProxyFactory { get; set; }

			public string FAHRT { get; set; }

			public string TEXT_ID { get; set; }

			public string BEMERKUNG { get; set; }

			public static GT_BEM Create(DataRow row, ISapConnection sapConnection = null, IDynSapProxyFactory dynSapProxyFactory = null)
			{
				var o = new GT_BEM
				{
					FAHRT = (string)row["FAHRT"],
					TEXT_ID = (string)row["TEXT_ID"],
					BEMERKUNG = (string)row["BEMERKUNG"],

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

			public static IEnumerable<GT_BEM> Select(DataTable dt, ISapConnection sapConnection = null)
			{
				return dt.AsEnumerable().Select(r => Create(r, sapConnection));
			}

			public static List<GT_BEM> ToList(DataTable dt, ISapConnection sapConnection = null)
			{
				return Select(dt, sapConnection).ToListOrEmptyList();
			}

			public static IEnumerable<GT_BEM> Select(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				var tbl = dts.FirstOrDefault(t => t.TableName.ToLower() == typeof(GT_BEM).Name.ToLower());
				if (tbl == null)
					return null;

				return Select(tbl, sapConnection);
			}

			public static List<GT_BEM> ToList(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				return Select(dts, sapConnection).ToListOrEmptyList();
			}

			public static List<GT_BEM> ToList(ISapDataService sapDataService)
			{
				return ToList(sapDataService.GetExportTables(), sapDataService.SapConnection);
			}

			public static List<GT_BEM> GetExportListWithInitExecute(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<GT_BEM>();
				 
				var dts = sapDataService.GetExportTablesWithInitExecute("Z_UEB_CREATE_ORDER_01", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_BEM> GetExportListWithExecute(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_BEM>();
				 
				var dts = sapDataService.GetExportTablesWithExecute();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_BEM> GetExportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_BEM>();
				 
				var dts = sapDataService.GetExportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_BEM> GetImportListWithInit(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<GT_BEM>();
				 
				var dts = sapDataService.GetImportTablesWithInit("Z_UEB_CREATE_ORDER_01", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_BEM> GetImportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_BEM>();
				 
				var dts = sapDataService.GetImportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}
		}

		public partial class GT_DIENSTLSTGN : IModelMappingApplied
		{
			[SapIgnore]
			[ScriptIgnore]
			public ISapConnection SAPConnection { get; set; }

			[SapIgnore]
			[ScriptIgnore]
			public IDynSapProxyFactory DynSapProxyFactory { get; set; }

			public string FAHRT { get; set; }

			public string DIENSTL_NR { get; set; }

			public string DIENSTL_TEXT { get; set; }

			public string MATNR { get; set; }

			public string FLAG_TEXT { get; set; }

			public string AUSWAHL_NR { get; set; }

			public string AUSWAHL_TEXT { get; set; }

			public string HAUPT_DL { get; set; }

			public static GT_DIENSTLSTGN Create(DataRow row, ISapConnection sapConnection = null, IDynSapProxyFactory dynSapProxyFactory = null)
			{
				var o = new GT_DIENSTLSTGN
				{
					FAHRT = (string)row["FAHRT"],
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

			public static IEnumerable<GT_DIENSTLSTGN> Select(DataTable dt, ISapConnection sapConnection = null)
			{
				return dt.AsEnumerable().Select(r => Create(r, sapConnection));
			}

			public static List<GT_DIENSTLSTGN> ToList(DataTable dt, ISapConnection sapConnection = null)
			{
				return Select(dt, sapConnection).ToListOrEmptyList();
			}

			public static IEnumerable<GT_DIENSTLSTGN> Select(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				var tbl = dts.FirstOrDefault(t => t.TableName.ToLower() == typeof(GT_DIENSTLSTGN).Name.ToLower());
				if (tbl == null)
					return null;

				return Select(tbl, sapConnection);
			}

			public static List<GT_DIENSTLSTGN> ToList(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				return Select(dts, sapConnection).ToListOrEmptyList();
			}

			public static List<GT_DIENSTLSTGN> ToList(ISapDataService sapDataService)
			{
				return ToList(sapDataService.GetExportTables(), sapDataService.SapConnection);
			}

			public static List<GT_DIENSTLSTGN> GetExportListWithInitExecute(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<GT_DIENSTLSTGN>();
				 
				var dts = sapDataService.GetExportTablesWithInitExecute("Z_UEB_CREATE_ORDER_01", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_DIENSTLSTGN> GetExportListWithExecute(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_DIENSTLSTGN>();
				 
				var dts = sapDataService.GetExportTablesWithExecute();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_DIENSTLSTGN> GetExportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_DIENSTLSTGN>();
				 
				var dts = sapDataService.GetExportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_DIENSTLSTGN> GetImportListWithInit(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<GT_DIENSTLSTGN>();
				 
				var dts = sapDataService.GetImportTablesWithInit("Z_UEB_CREATE_ORDER_01", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_DIENSTLSTGN> GetImportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_DIENSTLSTGN>();
				 
				var dts = sapDataService.GetImportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}
		}

		public partial class GT_FAHRTEN : IModelMappingApplied
		{
			[SapIgnore]
			[ScriptIgnore]
			public ISapConnection SAPConnection { get; set; }

			[SapIgnore]
			[ScriptIgnore]
			public IDynSapProxyFactory DynSapProxyFactory { get; set; }

			public string VORGANG { get; set; }

			public string FAHRT { get; set; }

			public string FAHRZEUG { get; set; }

			public string REIHENFOLGE { get; set; }

			public string TRANSPORTTYP { get; set; }

			public DateTime? VDATU { get; set; }

			public string VTIMEU { get; set; }

			public string KENNZ_ZUS_FAHT { get; set; }

			public string AT_TIM_VON { get; set; }

			public string AT_TIM_BIS { get; set; }

			public string TRANSPORTTYPNR { get; set; }

			public static GT_FAHRTEN Create(DataRow row, ISapConnection sapConnection = null, IDynSapProxyFactory dynSapProxyFactory = null)
			{
				var o = new GT_FAHRTEN
				{
					VORGANG = (string)row["VORGANG"],
					FAHRT = (string)row["FAHRT"],
					FAHRZEUG = (string)row["FAHRZEUG"],
					REIHENFOLGE = (string)row["REIHENFOLGE"],
					TRANSPORTTYP = (string)row["TRANSPORTTYP"],
					VDATU = string.IsNullOrEmpty(row["VDATU"].ToString()) ? null : (DateTime?)row["VDATU"],
					VTIMEU = (string)row["VTIMEU"],
					KENNZ_ZUS_FAHT = (string)row["KENNZ_ZUS_FAHT"],
					AT_TIM_VON = (string)row["AT_TIM_VON"],
					AT_TIM_BIS = (string)row["AT_TIM_BIS"],
					TRANSPORTTYPNR = (string)row["TRANSPORTTYPNR"],

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

			public static IEnumerable<GT_FAHRTEN> Select(DataTable dt, ISapConnection sapConnection = null)
			{
				return dt.AsEnumerable().Select(r => Create(r, sapConnection));
			}

			public static List<GT_FAHRTEN> ToList(DataTable dt, ISapConnection sapConnection = null)
			{
				return Select(dt, sapConnection).ToListOrEmptyList();
			}

			public static IEnumerable<GT_FAHRTEN> Select(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				var tbl = dts.FirstOrDefault(t => t.TableName.ToLower() == typeof(GT_FAHRTEN).Name.ToLower());
				if (tbl == null)
					return null;

				return Select(tbl, sapConnection);
			}

			public static List<GT_FAHRTEN> ToList(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				return Select(dts, sapConnection).ToListOrEmptyList();
			}

			public static List<GT_FAHRTEN> ToList(ISapDataService sapDataService)
			{
				return ToList(sapDataService.GetExportTables(), sapDataService.SapConnection);
			}

			public static List<GT_FAHRTEN> GetExportListWithInitExecute(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<GT_FAHRTEN>();
				 
				var dts = sapDataService.GetExportTablesWithInitExecute("Z_UEB_CREATE_ORDER_01", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_FAHRTEN> GetExportListWithExecute(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_FAHRTEN>();
				 
				var dts = sapDataService.GetExportTablesWithExecute();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_FAHRTEN> GetExportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_FAHRTEN>();
				 
				var dts = sapDataService.GetExportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_FAHRTEN> GetImportListWithInit(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<GT_FAHRTEN>();
				 
				var dts = sapDataService.GetImportTablesWithInit("Z_UEB_CREATE_ORDER_01", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_FAHRTEN> GetImportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_FAHRTEN>();
				 
				var dts = sapDataService.GetImportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}
		}

		public partial class GT_FZG : IModelMappingApplied
		{
			[SapIgnore]
			[ScriptIgnore]
			public ISapConnection SAPConnection { get; set; }

			[SapIgnore]
			[ScriptIgnore]
			public IDynSapProxyFactory DynSapProxyFactory { get; set; }

			public string FAHRZEUG { get; set; }

			public string ZZFAHRZGTYP { get; set; }

			public string ZZKENN { get; set; }

			public string FZGART { get; set; }

			public string ZULGE { get; set; }

			public string ZUL_BEI_CK_DAD { get; set; }

			public string SOWI { get; set; }

			public string ROTKENN { get; set; }

			public string AUGRU { get; set; }

			public string ZZREFNR { get; set; }

			public string ZZFAHRG { get; set; }

			public DateTime? ERSTZULDAT { get; set; }

			public string ZFZGKAT { get; set; }

			public string EXKUNNR_ZL { get; set; }

			public static GT_FZG Create(DataRow row, ISapConnection sapConnection = null, IDynSapProxyFactory dynSapProxyFactory = null)
			{
				var o = new GT_FZG
				{
					FAHRZEUG = (string)row["FAHRZEUG"],
					ZZFAHRZGTYP = (string)row["ZZFAHRZGTYP"],
					ZZKENN = (string)row["ZZKENN"],
					FZGART = (string)row["FZGART"],
					ZULGE = (string)row["ZULGE"],
					ZUL_BEI_CK_DAD = (string)row["ZUL_BEI_CK_DAD"],
					SOWI = (string)row["SOWI"],
					ROTKENN = (string)row["ROTKENN"],
					AUGRU = (string)row["AUGRU"],
					ZZREFNR = (string)row["ZZREFNR"],
					ZZFAHRG = (string)row["ZZFAHRG"],
					ERSTZULDAT = string.IsNullOrEmpty(row["ERSTZULDAT"].ToString()) ? null : (DateTime?)row["ERSTZULDAT"],
					ZFZGKAT = (string)row["ZFZGKAT"],
					EXKUNNR_ZL = (string)row["EXKUNNR_ZL"],

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

			public static IEnumerable<GT_FZG> Select(DataTable dt, ISapConnection sapConnection = null)
			{
				return dt.AsEnumerable().Select(r => Create(r, sapConnection));
			}

			public static List<GT_FZG> ToList(DataTable dt, ISapConnection sapConnection = null)
			{
				return Select(dt, sapConnection).ToListOrEmptyList();
			}

			public static IEnumerable<GT_FZG> Select(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				var tbl = dts.FirstOrDefault(t => t.TableName.ToLower() == typeof(GT_FZG).Name.ToLower());
				if (tbl == null)
					return null;

				return Select(tbl, sapConnection);
			}

			public static List<GT_FZG> ToList(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				return Select(dts, sapConnection).ToListOrEmptyList();
			}

			public static List<GT_FZG> ToList(ISapDataService sapDataService)
			{
				return ToList(sapDataService.GetExportTables(), sapDataService.SapConnection);
			}

			public static List<GT_FZG> GetExportListWithInitExecute(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<GT_FZG>();
				 
				var dts = sapDataService.GetExportTablesWithInitExecute("Z_UEB_CREATE_ORDER_01", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_FZG> GetExportListWithExecute(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_FZG>();
				 
				var dts = sapDataService.GetExportTablesWithExecute();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_FZG> GetExportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_FZG>();
				 
				var dts = sapDataService.GetExportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_FZG> GetImportListWithInit(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<GT_FZG>();
				 
				var dts = sapDataService.GetImportTablesWithInit("Z_UEB_CREATE_ORDER_01", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_FZG> GetImportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_FZG>();
				 
				var dts = sapDataService.GetImportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}
		}

		public partial class GT_PROT : IModelMappingApplied
		{
			[SapIgnore]
			[ScriptIgnore]
			public ISapConnection SAPConnection { get; set; }

			[SapIgnore]
			[ScriptIgnore]
			public IDynSapProxyFactory DynSapProxyFactory { get; set; }

			public string FAHRT { get; set; }

			public string ZZKATEGORIE { get; set; }

			public string ZZPROTOKOLLART { get; set; }

			public static GT_PROT Create(DataRow row, ISapConnection sapConnection = null, IDynSapProxyFactory dynSapProxyFactory = null)
			{
				var o = new GT_PROT
				{
					FAHRT = (string)row["FAHRT"],
					ZZKATEGORIE = (string)row["ZZKATEGORIE"],
					ZZPROTOKOLLART = (string)row["ZZPROTOKOLLART"],

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

			public static IEnumerable<GT_PROT> Select(DataTable dt, ISapConnection sapConnection = null)
			{
				return dt.AsEnumerable().Select(r => Create(r, sapConnection));
			}

			public static List<GT_PROT> ToList(DataTable dt, ISapConnection sapConnection = null)
			{
				return Select(dt, sapConnection).ToListOrEmptyList();
			}

			public static IEnumerable<GT_PROT> Select(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				var tbl = dts.FirstOrDefault(t => t.TableName.ToLower() == typeof(GT_PROT).Name.ToLower());
				if (tbl == null)
					return null;

				return Select(tbl, sapConnection);
			}

			public static List<GT_PROT> ToList(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				return Select(dts, sapConnection).ToListOrEmptyList();
			}

			public static List<GT_PROT> ToList(ISapDataService sapDataService)
			{
				return ToList(sapDataService.GetExportTables(), sapDataService.SapConnection);
			}

			public static List<GT_PROT> GetExportListWithInitExecute(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<GT_PROT>();
				 
				var dts = sapDataService.GetExportTablesWithInitExecute("Z_UEB_CREATE_ORDER_01", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_PROT> GetExportListWithExecute(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_PROT>();
				 
				var dts = sapDataService.GetExportTablesWithExecute();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_PROT> GetExportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_PROT>();
				 
				var dts = sapDataService.GetExportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_PROT> GetImportListWithInit(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<GT_PROT>();
				 
				var dts = sapDataService.GetImportTablesWithInit("Z_UEB_CREATE_ORDER_01", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_PROT> GetImportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_PROT>();
				 
				var dts = sapDataService.GetImportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}
		}

		public partial class GT_RET : IModelMappingApplied
		{
			[SapIgnore]
			[ScriptIgnore]
			public ISapConnection SAPConnection { get; set; }

			[SapIgnore]
			[ScriptIgnore]
			public IDynSapProxyFactory DynSapProxyFactory { get; set; }

			public string VORGANG { get; set; }

			public DateTime? VDATU { get; set; }

			public string FAHRZEUG { get; set; }

			public string FAHRT { get; set; }

			public string VBELN { get; set; }

			public string NAME_ST { get; set; }

			public string NAME_2_ST { get; set; }

			public string STREET_ST { get; set; }

			public string POSTL_CODE_ST { get; set; }

			public string CITY_ST { get; set; }

			public string COUNTRY_ST { get; set; }

			public string NAME_ZI { get; set; }

			public string NAME_2_ZI { get; set; }

			public string STREET_ZI { get; set; }

			public string POSTL_CODE_ZI { get; set; }

			public string CITY_ZI { get; set; }

			public string COUNTRY_ZI { get; set; }

			public string BEMERKUNG { get; set; }

			public static GT_RET Create(DataRow row, ISapConnection sapConnection = null, IDynSapProxyFactory dynSapProxyFactory = null)
			{
				var o = new GT_RET
				{
					VORGANG = (string)row["VORGANG"],
					VDATU = string.IsNullOrEmpty(row["VDATU"].ToString()) ? null : (DateTime?)row["VDATU"],
					FAHRZEUG = (string)row["FAHRZEUG"],
					FAHRT = (string)row["FAHRT"],
					VBELN = (string)row["VBELN"],
					NAME_ST = (string)row["NAME_ST"],
					NAME_2_ST = (string)row["NAME_2_ST"],
					STREET_ST = (string)row["STREET_ST"],
					POSTL_CODE_ST = (string)row["POSTL_CODE_ST"],
					CITY_ST = (string)row["CITY_ST"],
					COUNTRY_ST = (string)row["COUNTRY_ST"],
					NAME_ZI = (string)row["NAME_ZI"],
					NAME_2_ZI = (string)row["NAME_2_ZI"],
					STREET_ZI = (string)row["STREET_ZI"],
					POSTL_CODE_ZI = (string)row["POSTL_CODE_ZI"],
					CITY_ZI = (string)row["CITY_ZI"],
					COUNTRY_ZI = (string)row["COUNTRY_ZI"],
					BEMERKUNG = (string)row["BEMERKUNG"],

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

			public static IEnumerable<GT_RET> Select(DataTable dt, ISapConnection sapConnection = null)
			{
				return dt.AsEnumerable().Select(r => Create(r, sapConnection));
			}

			public static List<GT_RET> ToList(DataTable dt, ISapConnection sapConnection = null)
			{
				return Select(dt, sapConnection).ToListOrEmptyList();
			}

			public static IEnumerable<GT_RET> Select(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				var tbl = dts.FirstOrDefault(t => t.TableName.ToLower() == typeof(GT_RET).Name.ToLower());
				if (tbl == null)
					return null;

				return Select(tbl, sapConnection);
			}

			public static List<GT_RET> ToList(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				return Select(dts, sapConnection).ToListOrEmptyList();
			}

			public static List<GT_RET> ToList(ISapDataService sapDataService)
			{
				return ToList(sapDataService.GetExportTables(), sapDataService.SapConnection);
			}

			public static List<GT_RET> GetExportListWithInitExecute(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<GT_RET>();
				 
				var dts = sapDataService.GetExportTablesWithInitExecute("Z_UEB_CREATE_ORDER_01", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_RET> GetExportListWithExecute(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_RET>();
				 
				var dts = sapDataService.GetExportTablesWithExecute();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_RET> GetExportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_RET>();
				 
				var dts = sapDataService.GetExportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_RET> GetImportListWithInit(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<GT_RET>();
				 
				var dts = sapDataService.GetImportTablesWithInit("Z_UEB_CREATE_ORDER_01", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_RET> GetImportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_RET>();
				 
				var dts = sapDataService.GetImportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}
		}
	}

	public static partial class DataTableExtensions
	{

		public static DataTable ToTable(this IEnumerable<Z_UEB_CREATE_ORDER_01.GT_ADRESSEN> list)
		{
			return SapDataServiceExtensions.ToTable(list);
		}


		public static DataTable ToTable(this IEnumerable<Z_UEB_CREATE_ORDER_01.GT_BEM> list)
		{
			return SapDataServiceExtensions.ToTable(list);
		}


		public static DataTable ToTable(this IEnumerable<Z_UEB_CREATE_ORDER_01.GT_DIENSTLSTGN> list)
		{
			return SapDataServiceExtensions.ToTable(list);
		}


		public static DataTable ToTable(this IEnumerable<Z_UEB_CREATE_ORDER_01.GT_FAHRTEN> list)
		{
			return SapDataServiceExtensions.ToTable(list);
		}


		public static DataTable ToTable(this IEnumerable<Z_UEB_CREATE_ORDER_01.GT_FZG> list)
		{
			return SapDataServiceExtensions.ToTable(list);
		}


		public static DataTable ToTable(this IEnumerable<Z_UEB_CREATE_ORDER_01.GT_PROT> list)
		{
			return SapDataServiceExtensions.ToTable(list);
		}


		public static DataTable ToTable(this IEnumerable<Z_UEB_CREATE_ORDER_01.GT_RET> list)
		{
			return SapDataServiceExtensions.ToTable(list);
		}

	}
}
