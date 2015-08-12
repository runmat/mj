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
	public partial class Z_WFM_READ_AUFTRAEGE_01
	{
		public static void Init(ISapDataService sap)
		{
			sap.Init(typeof(Z_WFM_READ_AUFTRAEGE_01).Name);
		}

		public static void Init(ISapDataService sap, string inputParameterKeys, params object[] inputParameterValues)
		{
			sap.Init(typeof(Z_WFM_READ_AUFTRAEGE_01).Name, inputParameterKeys, inputParameterValues);
		}

		public partial class GT_DATEN : IModelMappingApplied
		{
			[SapIgnore]
			[ScriptIgnore]
			public ISapConnection SAPConnection { get; set; }

			[SapIgnore]
			[ScriptIgnore]
			public IDynSapProxyFactory DynSapProxyFactory { get; set; }

			public string KUNNR { get; set; }

			public string VORG_NR_ABM_AUF { get; set; }

			public string BELNR_ZABMK { get; set; }

			public string KUNDENAUFTRAGSNR { get; set; }

			public string FAHRG { get; set; }

			public string KENNZ { get; set; }

			public string ZB1_NR { get; set; }

			public string ZB2_NR { get; set; }

			public string EQUNR_ZB1 { get; set; }

			public string EQUNR_ZB2 { get; set; }

			public string ABMELDEART { get; set; }

			public string ABMELDESTATUS { get; set; }

			public DateTime? ABMELDEDATUM { get; set; }

			public DateTime? STORNODATUM { get; set; }

			public DateTime? ZB1_VORHANDEN { get; set; }

			public DateTime? ZB2_VORHANDEN { get; set; }

			public DateTime? KENNZ_VORN_VORH { get; set; }

			public DateTime? KENNZ_HINTEN_VORH { get; set; }

			public DateTime? KENNZV_ENTWERTET { get; set; }

			public DateTime? KENNZH_ENTWERTET { get; set; }

			public DateTime? KENNZV_DIEBSTAHL { get; set; }

			public DateTime? KENNZH_DIEBSTAHL { get; set; }

			public string BELNR_ZCARPK { get; set; }

			public string CARPORT { get; set; }

			public DateTime? ERDAT_ZCARPK { get; set; }

			public DateTime? WIEDVORLAGE_KUNDE { get; set; }

			public DateTime? WIEDVORLAGE_SC { get; set; }

			public DateTime? ZUSTIMMUNG_ZLS { get; set; }

			public string ZUSTIMM_EMPF { get; set; }

			public string ZUSTIMM_USER { get; set; }

			public DateTime? NAUANF_ZLS { get; set; }

			public string EMPF_NEUANFORD { get; set; }

			public string NAUANF_USER { get; set; }

			public string SELEKTION1 { get; set; }

			public string SELEKTION2 { get; set; }

			public string SELEKTION3 { get; set; }

			public string REFERENZ1 { get; set; }

			public string REFERENZ2 { get; set; }

			public string REFERENZ3 { get; set; }

			public string VORGAENGER_AUFTR { get; set; }

			public string LFD_NR { get; set; }

			public string AUFGABE { get; set; }

			public decimal? ERFASST { get; set; }

			public decimal? ENDE { get; set; }

			public string TASK_ID { get; set; }

			public string CHASSIS_NUM { get; set; }

			public string TODO_WER { get; set; }

			public DateTime? STARTDATUM { get; set; }

			public string STARTZEIT { get; set; }

			public DateTime? SOLL_DATUM { get; set; }

			public string SOLL_ZEIT { get; set; }

			public DateTime? IST_DATUM { get; set; }

			public string IST_ZEIT { get; set; }

			public string ZUSER { get; set; }

			public string ANMERKUNG { get; set; }

			public string STATUS { get; set; }

			public string FOLGE_TASK_ID { get; set; }

			public string VERLUST_ZB1 { get; set; }

			public string Z_FARBE { get; set; }

			public string MA_VORGANG { get; set; }

			public static GT_DATEN Create(DataRow row, ISapConnection sapConnection = null, IDynSapProxyFactory dynSapProxyFactory = null)
			{
				var o = new GT_DATEN
				{
					KUNNR = (string)row["KUNNR"],
					VORG_NR_ABM_AUF = (string)row["VORG_NR_ABM_AUF"],
					BELNR_ZABMK = (string)row["BELNR_ZABMK"],
					KUNDENAUFTRAGSNR = (string)row["KUNDENAUFTRAGSNR"],
					FAHRG = (string)row["FAHRG"],
					KENNZ = (string)row["KENNZ"],
					ZB1_NR = (string)row["ZB1_NR"],
					ZB2_NR = (string)row["ZB2_NR"],
					EQUNR_ZB1 = (string)row["EQUNR_ZB1"],
					EQUNR_ZB2 = (string)row["EQUNR_ZB2"],
					ABMELDEART = (string)row["ABMELDEART"],
					ABMELDESTATUS = (string)row["ABMELDESTATUS"],
					ABMELDEDATUM = (string.IsNullOrEmpty(row["ABMELDEDATUM"].ToString())) ? null : (DateTime?)row["ABMELDEDATUM"],
					STORNODATUM = (string.IsNullOrEmpty(row["STORNODATUM"].ToString())) ? null : (DateTime?)row["STORNODATUM"],
					ZB1_VORHANDEN = (string.IsNullOrEmpty(row["ZB1_VORHANDEN"].ToString())) ? null : (DateTime?)row["ZB1_VORHANDEN"],
					ZB2_VORHANDEN = (string.IsNullOrEmpty(row["ZB2_VORHANDEN"].ToString())) ? null : (DateTime?)row["ZB2_VORHANDEN"],
					KENNZ_VORN_VORH = (string.IsNullOrEmpty(row["KENNZ_VORN_VORH"].ToString())) ? null : (DateTime?)row["KENNZ_VORN_VORH"],
					KENNZ_HINTEN_VORH = (string.IsNullOrEmpty(row["KENNZ_HINTEN_VORH"].ToString())) ? null : (DateTime?)row["KENNZ_HINTEN_VORH"],
					KENNZV_ENTWERTET = (string.IsNullOrEmpty(row["KENNZV_ENTWERTET"].ToString())) ? null : (DateTime?)row["KENNZV_ENTWERTET"],
					KENNZH_ENTWERTET = (string.IsNullOrEmpty(row["KENNZH_ENTWERTET"].ToString())) ? null : (DateTime?)row["KENNZH_ENTWERTET"],
					KENNZV_DIEBSTAHL = (string.IsNullOrEmpty(row["KENNZV_DIEBSTAHL"].ToString())) ? null : (DateTime?)row["KENNZV_DIEBSTAHL"],
					KENNZH_DIEBSTAHL = (string.IsNullOrEmpty(row["KENNZH_DIEBSTAHL"].ToString())) ? null : (DateTime?)row["KENNZH_DIEBSTAHL"],
					BELNR_ZCARPK = (string)row["BELNR_ZCARPK"],
					CARPORT = (string)row["CARPORT"],
					ERDAT_ZCARPK = (string.IsNullOrEmpty(row["ERDAT_ZCARPK"].ToString())) ? null : (DateTime?)row["ERDAT_ZCARPK"],
					WIEDVORLAGE_KUNDE = (string.IsNullOrEmpty(row["WIEDVORLAGE_KUNDE"].ToString())) ? null : (DateTime?)row["WIEDVORLAGE_KUNDE"],
					WIEDVORLAGE_SC = (string.IsNullOrEmpty(row["WIEDVORLAGE_SC"].ToString())) ? null : (DateTime?)row["WIEDVORLAGE_SC"],
					ZUSTIMMUNG_ZLS = (string.IsNullOrEmpty(row["ZUSTIMMUNG_ZLS"].ToString())) ? null : (DateTime?)row["ZUSTIMMUNG_ZLS"],
					ZUSTIMM_EMPF = (string)row["ZUSTIMM_EMPF"],
					ZUSTIMM_USER = (string)row["ZUSTIMM_USER"],
					NAUANF_ZLS = (string.IsNullOrEmpty(row["NAUANF_ZLS"].ToString())) ? null : (DateTime?)row["NAUANF_ZLS"],
					EMPF_NEUANFORD = (string)row["EMPF_NEUANFORD"],
					NAUANF_USER = (string)row["NAUANF_USER"],
					SELEKTION1 = (string)row["SELEKTION1"],
					SELEKTION2 = (string)row["SELEKTION2"],
					SELEKTION3 = (string)row["SELEKTION3"],
					REFERENZ1 = (string)row["REFERENZ1"],
					REFERENZ2 = (string)row["REFERENZ2"],
					REFERENZ3 = (string)row["REFERENZ3"],
					VORGAENGER_AUFTR = (string)row["VORGAENGER_AUFTR"],
					LFD_NR = (string)row["LFD_NR"],
					AUFGABE = (string)row["AUFGABE"],
					ERFASST = (decimal?)row["ERFASST"],
					ENDE = (decimal?)row["ENDE"],
					TASK_ID = (string)row["TASK_ID"],
					CHASSIS_NUM = (string)row["CHASSIS_NUM"],
					TODO_WER = (string)row["TODO_WER"],
					STARTDATUM = (string.IsNullOrEmpty(row["STARTDATUM"].ToString())) ? null : (DateTime?)row["STARTDATUM"],
					STARTZEIT = (string)row["STARTZEIT"],
					SOLL_DATUM = (string.IsNullOrEmpty(row["SOLL_DATUM"].ToString())) ? null : (DateTime?)row["SOLL_DATUM"],
					SOLL_ZEIT = (string)row["SOLL_ZEIT"],
					IST_DATUM = (string.IsNullOrEmpty(row["IST_DATUM"].ToString())) ? null : (DateTime?)row["IST_DATUM"],
					IST_ZEIT = (string)row["IST_ZEIT"],
					ZUSER = (string)row["ZUSER"],
					ANMERKUNG = (string)row["ANMERKUNG"],
					STATUS = (string)row["STATUS"],
					FOLGE_TASK_ID = (string)row["FOLGE_TASK_ID"],
					VERLUST_ZB1 = (string)row["VERLUST_ZB1"],
					Z_FARBE = (string)row["Z_FARBE"],
					MA_VORGANG = (string)row["MA_VORGANG"],

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
				 
				var dts = sapDataService.GetExportTablesWithInitExecute("Z_WFM_READ_AUFTRAEGE_01", inputParameterKeys, inputParameterValues);
				 
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
				 
				var dts = sapDataService.GetImportTablesWithInit("Z_WFM_READ_AUFTRAEGE_01", inputParameterKeys, inputParameterValues);
				 
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

		public partial class GT_SEL_ABMART : IModelMappingApplied
		{
			[SapIgnore]
			[ScriptIgnore]
			public ISapConnection SAPConnection { get; set; }

			[SapIgnore]
			[ScriptIgnore]
			public IDynSapProxyFactory DynSapProxyFactory { get; set; }

			public string ABMELDEART { get; set; }

			public static GT_SEL_ABMART Create(DataRow row, ISapConnection sapConnection = null, IDynSapProxyFactory dynSapProxyFactory = null)
			{
				var o = new GT_SEL_ABMART
				{
					ABMELDEART = (string)row["ABMELDEART"],

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

			public static IEnumerable<GT_SEL_ABMART> Select(DataTable dt, ISapConnection sapConnection = null)
			{
				return dt.AsEnumerable().Select(r => Create(r, sapConnection));
			}

			public static List<GT_SEL_ABMART> ToList(DataTable dt, ISapConnection sapConnection = null)
			{
				return Select(dt, sapConnection).ToListOrEmptyList();
			}

			public static IEnumerable<GT_SEL_ABMART> Select(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				var tbl = dts.FirstOrDefault(t => t.TableName.ToLower() == typeof(GT_SEL_ABMART).Name.ToLower());
				if (tbl == null)
					return null;

				return Select(tbl, sapConnection);
			}

			public static List<GT_SEL_ABMART> ToList(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				return Select(dts, sapConnection).ToListOrEmptyList();
			}

			public static List<GT_SEL_ABMART> ToList(ISapDataService sapDataService)
			{
				return ToList(sapDataService.GetExportTables(), sapDataService.SapConnection);
			}

			public static List<GT_SEL_ABMART> GetExportListWithInitExecute(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<GT_SEL_ABMART>();
				 
				var dts = sapDataService.GetExportTablesWithInitExecute("Z_WFM_READ_AUFTRAEGE_01", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_SEL_ABMART> GetExportListWithExecute(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_SEL_ABMART>();
				 
				var dts = sapDataService.GetExportTablesWithExecute();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_SEL_ABMART> GetExportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_SEL_ABMART>();
				 
				var dts = sapDataService.GetExportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_SEL_ABMART> GetImportListWithInit(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<GT_SEL_ABMART>();
				 
				var dts = sapDataService.GetImportTablesWithInit("Z_WFM_READ_AUFTRAEGE_01", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_SEL_ABMART> GetImportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_SEL_ABMART>();
				 
				var dts = sapDataService.GetImportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}
		}

		public partial class GT_SEL_ABMSTAT : IModelMappingApplied
		{
			[SapIgnore]
			[ScriptIgnore]
			public ISapConnection SAPConnection { get; set; }

			[SapIgnore]
			[ScriptIgnore]
			public IDynSapProxyFactory DynSapProxyFactory { get; set; }

			public string ABMELDESTATUS { get; set; }

			public static GT_SEL_ABMSTAT Create(DataRow row, ISapConnection sapConnection = null, IDynSapProxyFactory dynSapProxyFactory = null)
			{
				var o = new GT_SEL_ABMSTAT
				{
					ABMELDESTATUS = (string)row["ABMELDESTATUS"],

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

			public static IEnumerable<GT_SEL_ABMSTAT> Select(DataTable dt, ISapConnection sapConnection = null)
			{
				return dt.AsEnumerable().Select(r => Create(r, sapConnection));
			}

			public static List<GT_SEL_ABMSTAT> ToList(DataTable dt, ISapConnection sapConnection = null)
			{
				return Select(dt, sapConnection).ToListOrEmptyList();
			}

			public static IEnumerable<GT_SEL_ABMSTAT> Select(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				var tbl = dts.FirstOrDefault(t => t.TableName.ToLower() == typeof(GT_SEL_ABMSTAT).Name.ToLower());
				if (tbl == null)
					return null;

				return Select(tbl, sapConnection);
			}

			public static List<GT_SEL_ABMSTAT> ToList(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				return Select(dts, sapConnection).ToListOrEmptyList();
			}

			public static List<GT_SEL_ABMSTAT> ToList(ISapDataService sapDataService)
			{
				return ToList(sapDataService.GetExportTables(), sapDataService.SapConnection);
			}

			public static List<GT_SEL_ABMSTAT> GetExportListWithInitExecute(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<GT_SEL_ABMSTAT>();
				 
				var dts = sapDataService.GetExportTablesWithInitExecute("Z_WFM_READ_AUFTRAEGE_01", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_SEL_ABMSTAT> GetExportListWithExecute(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_SEL_ABMSTAT>();
				 
				var dts = sapDataService.GetExportTablesWithExecute();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_SEL_ABMSTAT> GetExportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_SEL_ABMSTAT>();
				 
				var dts = sapDataService.GetExportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_SEL_ABMSTAT> GetImportListWithInit(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<GT_SEL_ABMSTAT>();
				 
				var dts = sapDataService.GetImportTablesWithInit("Z_WFM_READ_AUFTRAEGE_01", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_SEL_ABMSTAT> GetImportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_SEL_ABMSTAT>();
				 
				var dts = sapDataService.GetImportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}
		}

		public partial class GT_SEL_KDAUF : IModelMappingApplied
		{
			[SapIgnore]
			[ScriptIgnore]
			public ISapConnection SAPConnection { get; set; }

			[SapIgnore]
			[ScriptIgnore]
			public IDynSapProxyFactory DynSapProxyFactory { get; set; }

			public string KDAUF_VON { get; set; }

			public string KDAUF_BIS { get; set; }

			public static GT_SEL_KDAUF Create(DataRow row, ISapConnection sapConnection = null, IDynSapProxyFactory dynSapProxyFactory = null)
			{
				var o = new GT_SEL_KDAUF
				{
					KDAUF_VON = (string)row["KDAUF_VON"],
					KDAUF_BIS = (string)row["KDAUF_BIS"],

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

			public static IEnumerable<GT_SEL_KDAUF> Select(DataTable dt, ISapConnection sapConnection = null)
			{
				return dt.AsEnumerable().Select(r => Create(r, sapConnection));
			}

			public static List<GT_SEL_KDAUF> ToList(DataTable dt, ISapConnection sapConnection = null)
			{
				return Select(dt, sapConnection).ToListOrEmptyList();
			}

			public static IEnumerable<GT_SEL_KDAUF> Select(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				var tbl = dts.FirstOrDefault(t => t.TableName.ToLower() == typeof(GT_SEL_KDAUF).Name.ToLower());
				if (tbl == null)
					return null;

				return Select(tbl, sapConnection);
			}

			public static List<GT_SEL_KDAUF> ToList(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				return Select(dts, sapConnection).ToListOrEmptyList();
			}

			public static List<GT_SEL_KDAUF> ToList(ISapDataService sapDataService)
			{
				return ToList(sapDataService.GetExportTables(), sapDataService.SapConnection);
			}

			public static List<GT_SEL_KDAUF> GetExportListWithInitExecute(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<GT_SEL_KDAUF>();
				 
				var dts = sapDataService.GetExportTablesWithInitExecute("Z_WFM_READ_AUFTRAEGE_01", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_SEL_KDAUF> GetExportListWithExecute(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_SEL_KDAUF>();
				 
				var dts = sapDataService.GetExportTablesWithExecute();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_SEL_KDAUF> GetExportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_SEL_KDAUF>();
				 
				var dts = sapDataService.GetExportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_SEL_KDAUF> GetImportListWithInit(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<GT_SEL_KDAUF>();
				 
				var dts = sapDataService.GetImportTablesWithInit("Z_WFM_READ_AUFTRAEGE_01", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_SEL_KDAUF> GetImportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_SEL_KDAUF>();
				 
				var dts = sapDataService.GetImportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}
		}

		public partial class GT_SEL_REF1 : IModelMappingApplied
		{
			[SapIgnore]
			[ScriptIgnore]
			public ISapConnection SAPConnection { get; set; }

			[SapIgnore]
			[ScriptIgnore]
			public IDynSapProxyFactory DynSapProxyFactory { get; set; }

			public string REF1_VON { get; set; }

			public string REF1_BIS { get; set; }

			public static GT_SEL_REF1 Create(DataRow row, ISapConnection sapConnection = null, IDynSapProxyFactory dynSapProxyFactory = null)
			{
				var o = new GT_SEL_REF1
				{
					REF1_VON = (string)row["REF1_VON"],
					REF1_BIS = (string)row["REF1_BIS"],

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

			public static IEnumerable<GT_SEL_REF1> Select(DataTable dt, ISapConnection sapConnection = null)
			{
				return dt.AsEnumerable().Select(r => Create(r, sapConnection));
			}

			public static List<GT_SEL_REF1> ToList(DataTable dt, ISapConnection sapConnection = null)
			{
				return Select(dt, sapConnection).ToListOrEmptyList();
			}

			public static IEnumerable<GT_SEL_REF1> Select(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				var tbl = dts.FirstOrDefault(t => t.TableName.ToLower() == typeof(GT_SEL_REF1).Name.ToLower());
				if (tbl == null)
					return null;

				return Select(tbl, sapConnection);
			}

			public static List<GT_SEL_REF1> ToList(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				return Select(dts, sapConnection).ToListOrEmptyList();
			}

			public static List<GT_SEL_REF1> ToList(ISapDataService sapDataService)
			{
				return ToList(sapDataService.GetExportTables(), sapDataService.SapConnection);
			}

			public static List<GT_SEL_REF1> GetExportListWithInitExecute(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<GT_SEL_REF1>();
				 
				var dts = sapDataService.GetExportTablesWithInitExecute("Z_WFM_READ_AUFTRAEGE_01", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_SEL_REF1> GetExportListWithExecute(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_SEL_REF1>();
				 
				var dts = sapDataService.GetExportTablesWithExecute();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_SEL_REF1> GetExportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_SEL_REF1>();
				 
				var dts = sapDataService.GetExportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_SEL_REF1> GetImportListWithInit(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<GT_SEL_REF1>();
				 
				var dts = sapDataService.GetImportTablesWithInit("Z_WFM_READ_AUFTRAEGE_01", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_SEL_REF1> GetImportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_SEL_REF1>();
				 
				var dts = sapDataService.GetImportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}
		}

		public partial class GT_SEL_REF2 : IModelMappingApplied
		{
			[SapIgnore]
			[ScriptIgnore]
			public ISapConnection SAPConnection { get; set; }

			[SapIgnore]
			[ScriptIgnore]
			public IDynSapProxyFactory DynSapProxyFactory { get; set; }

			public string REF2_VON { get; set; }

			public string REF2_BIS { get; set; }

			public static GT_SEL_REF2 Create(DataRow row, ISapConnection sapConnection = null, IDynSapProxyFactory dynSapProxyFactory = null)
			{
				var o = new GT_SEL_REF2
				{
					REF2_VON = (string)row["REF2_VON"],
					REF2_BIS = (string)row["REF2_BIS"],

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

			public static IEnumerable<GT_SEL_REF2> Select(DataTable dt, ISapConnection sapConnection = null)
			{
				return dt.AsEnumerable().Select(r => Create(r, sapConnection));
			}

			public static List<GT_SEL_REF2> ToList(DataTable dt, ISapConnection sapConnection = null)
			{
				return Select(dt, sapConnection).ToListOrEmptyList();
			}

			public static IEnumerable<GT_SEL_REF2> Select(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				var tbl = dts.FirstOrDefault(t => t.TableName.ToLower() == typeof(GT_SEL_REF2).Name.ToLower());
				if (tbl == null)
					return null;

				return Select(tbl, sapConnection);
			}

			public static List<GT_SEL_REF2> ToList(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				return Select(dts, sapConnection).ToListOrEmptyList();
			}

			public static List<GT_SEL_REF2> ToList(ISapDataService sapDataService)
			{
				return ToList(sapDataService.GetExportTables(), sapDataService.SapConnection);
			}

			public static List<GT_SEL_REF2> GetExportListWithInitExecute(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<GT_SEL_REF2>();
				 
				var dts = sapDataService.GetExportTablesWithInitExecute("Z_WFM_READ_AUFTRAEGE_01", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_SEL_REF2> GetExportListWithExecute(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_SEL_REF2>();
				 
				var dts = sapDataService.GetExportTablesWithExecute();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_SEL_REF2> GetExportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_SEL_REF2>();
				 
				var dts = sapDataService.GetExportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_SEL_REF2> GetImportListWithInit(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<GT_SEL_REF2>();
				 
				var dts = sapDataService.GetImportTablesWithInit("Z_WFM_READ_AUFTRAEGE_01", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_SEL_REF2> GetImportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_SEL_REF2>();
				 
				var dts = sapDataService.GetImportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}
		}

		public partial class GT_SEL_REF3 : IModelMappingApplied
		{
			[SapIgnore]
			[ScriptIgnore]
			public ISapConnection SAPConnection { get; set; }

			[SapIgnore]
			[ScriptIgnore]
			public IDynSapProxyFactory DynSapProxyFactory { get; set; }

			public string REF3_VON { get; set; }

			public string REF3_BIS { get; set; }

			public static GT_SEL_REF3 Create(DataRow row, ISapConnection sapConnection = null, IDynSapProxyFactory dynSapProxyFactory = null)
			{
				var o = new GT_SEL_REF3
				{
					REF3_VON = (string)row["REF3_VON"],
					REF3_BIS = (string)row["REF3_BIS"],

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

			public static IEnumerable<GT_SEL_REF3> Select(DataTable dt, ISapConnection sapConnection = null)
			{
				return dt.AsEnumerable().Select(r => Create(r, sapConnection));
			}

			public static List<GT_SEL_REF3> ToList(DataTable dt, ISapConnection sapConnection = null)
			{
				return Select(dt, sapConnection).ToListOrEmptyList();
			}

			public static IEnumerable<GT_SEL_REF3> Select(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				var tbl = dts.FirstOrDefault(t => t.TableName.ToLower() == typeof(GT_SEL_REF3).Name.ToLower());
				if (tbl == null)
					return null;

				return Select(tbl, sapConnection);
			}

			public static List<GT_SEL_REF3> ToList(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				return Select(dts, sapConnection).ToListOrEmptyList();
			}

			public static List<GT_SEL_REF3> ToList(ISapDataService sapDataService)
			{
				return ToList(sapDataService.GetExportTables(), sapDataService.SapConnection);
			}

			public static List<GT_SEL_REF3> GetExportListWithInitExecute(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<GT_SEL_REF3>();
				 
				var dts = sapDataService.GetExportTablesWithInitExecute("Z_WFM_READ_AUFTRAEGE_01", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_SEL_REF3> GetExportListWithExecute(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_SEL_REF3>();
				 
				var dts = sapDataService.GetExportTablesWithExecute();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_SEL_REF3> GetExportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_SEL_REF3>();
				 
				var dts = sapDataService.GetExportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_SEL_REF3> GetImportListWithInit(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<GT_SEL_REF3>();
				 
				var dts = sapDataService.GetImportTablesWithInit("Z_WFM_READ_AUFTRAEGE_01", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_SEL_REF3> GetImportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_SEL_REF3>();
				 
				var dts = sapDataService.GetImportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}
		}

		public partial class GT_SEL_SOLLDAT : IModelMappingApplied
		{
			[SapIgnore]
			[ScriptIgnore]
			public ISapConnection SAPConnection { get; set; }

			[SapIgnore]
			[ScriptIgnore]
			public IDynSapProxyFactory DynSapProxyFactory { get; set; }

			public DateTime? SOLLDATUM_VON { get; set; }

			public DateTime? SOLLDATUM_BIS { get; set; }

			public static GT_SEL_SOLLDAT Create(DataRow row, ISapConnection sapConnection = null, IDynSapProxyFactory dynSapProxyFactory = null)
			{
				var o = new GT_SEL_SOLLDAT
				{
					SOLLDATUM_VON = (string.IsNullOrEmpty(row["SOLLDATUM_VON"].ToString())) ? null : (DateTime?)row["SOLLDATUM_VON"],
					SOLLDATUM_BIS = (string.IsNullOrEmpty(row["SOLLDATUM_BIS"].ToString())) ? null : (DateTime?)row["SOLLDATUM_BIS"],

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

			public static IEnumerable<GT_SEL_SOLLDAT> Select(DataTable dt, ISapConnection sapConnection = null)
			{
				return dt.AsEnumerable().Select(r => Create(r, sapConnection));
			}

			public static List<GT_SEL_SOLLDAT> ToList(DataTable dt, ISapConnection sapConnection = null)
			{
				return Select(dt, sapConnection).ToListOrEmptyList();
			}

			public static IEnumerable<GT_SEL_SOLLDAT> Select(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				var tbl = dts.FirstOrDefault(t => t.TableName.ToLower() == typeof(GT_SEL_SOLLDAT).Name.ToLower());
				if (tbl == null)
					return null;

				return Select(tbl, sapConnection);
			}

			public static List<GT_SEL_SOLLDAT> ToList(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				return Select(dts, sapConnection).ToListOrEmptyList();
			}

			public static List<GT_SEL_SOLLDAT> ToList(ISapDataService sapDataService)
			{
				return ToList(sapDataService.GetExportTables(), sapDataService.SapConnection);
			}

			public static List<GT_SEL_SOLLDAT> GetExportListWithInitExecute(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<GT_SEL_SOLLDAT>();
				 
				var dts = sapDataService.GetExportTablesWithInitExecute("Z_WFM_READ_AUFTRAEGE_01", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_SEL_SOLLDAT> GetExportListWithExecute(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_SEL_SOLLDAT>();
				 
				var dts = sapDataService.GetExportTablesWithExecute();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_SEL_SOLLDAT> GetExportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_SEL_SOLLDAT>();
				 
				var dts = sapDataService.GetExportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_SEL_SOLLDAT> GetImportListWithInit(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<GT_SEL_SOLLDAT>();
				 
				var dts = sapDataService.GetImportTablesWithInit("Z_WFM_READ_AUFTRAEGE_01", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_SEL_SOLLDAT> GetImportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_SEL_SOLLDAT>();
				 
				var dts = sapDataService.GetImportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}
		}
	}

	public static partial class DataTableExtensions
	{

		public static DataTable ToTable(this IEnumerable<Z_WFM_READ_AUFTRAEGE_01.GT_DATEN> list)
		{
			return SapDataServiceExtensions.ToTable(list);
		}

		public static void Apply(this IEnumerable<Z_WFM_READ_AUFTRAEGE_01.GT_DATEN> list, DataTable dtDst)
		{
			SapDataServiceExtensions.Apply(list, dtDst);
		}


		public static DataTable ToTable(this IEnumerable<Z_WFM_READ_AUFTRAEGE_01.GT_SEL_ABMART> list)
		{
			return SapDataServiceExtensions.ToTable(list);
		}

		public static void Apply(this IEnumerable<Z_WFM_READ_AUFTRAEGE_01.GT_SEL_ABMART> list, DataTable dtDst)
		{
			SapDataServiceExtensions.Apply(list, dtDst);
		}


		public static DataTable ToTable(this IEnumerable<Z_WFM_READ_AUFTRAEGE_01.GT_SEL_ABMSTAT> list)
		{
			return SapDataServiceExtensions.ToTable(list);
		}

		public static void Apply(this IEnumerable<Z_WFM_READ_AUFTRAEGE_01.GT_SEL_ABMSTAT> list, DataTable dtDst)
		{
			SapDataServiceExtensions.Apply(list, dtDst);
		}


		public static DataTable ToTable(this IEnumerable<Z_WFM_READ_AUFTRAEGE_01.GT_SEL_KDAUF> list)
		{
			return SapDataServiceExtensions.ToTable(list);
		}

		public static void Apply(this IEnumerable<Z_WFM_READ_AUFTRAEGE_01.GT_SEL_KDAUF> list, DataTable dtDst)
		{
			SapDataServiceExtensions.Apply(list, dtDst);
		}


		public static DataTable ToTable(this IEnumerable<Z_WFM_READ_AUFTRAEGE_01.GT_SEL_REF1> list)
		{
			return SapDataServiceExtensions.ToTable(list);
		}

		public static void Apply(this IEnumerable<Z_WFM_READ_AUFTRAEGE_01.GT_SEL_REF1> list, DataTable dtDst)
		{
			SapDataServiceExtensions.Apply(list, dtDst);
		}


		public static DataTable ToTable(this IEnumerable<Z_WFM_READ_AUFTRAEGE_01.GT_SEL_REF2> list)
		{
			return SapDataServiceExtensions.ToTable(list);
		}

		public static void Apply(this IEnumerable<Z_WFM_READ_AUFTRAEGE_01.GT_SEL_REF2> list, DataTable dtDst)
		{
			SapDataServiceExtensions.Apply(list, dtDst);
		}


		public static DataTable ToTable(this IEnumerable<Z_WFM_READ_AUFTRAEGE_01.GT_SEL_REF3> list)
		{
			return SapDataServiceExtensions.ToTable(list);
		}

		public static void Apply(this IEnumerable<Z_WFM_READ_AUFTRAEGE_01.GT_SEL_REF3> list, DataTable dtDst)
		{
			SapDataServiceExtensions.Apply(list, dtDst);
		}


		public static DataTable ToTable(this IEnumerable<Z_WFM_READ_AUFTRAEGE_01.GT_SEL_SOLLDAT> list)
		{
			return SapDataServiceExtensions.ToTable(list);
		}

		public static void Apply(this IEnumerable<Z_WFM_READ_AUFTRAEGE_01.GT_SEL_SOLLDAT> list, DataTable dtDst)
		{
			SapDataServiceExtensions.Apply(list, dtDst);
		}

	}
}
