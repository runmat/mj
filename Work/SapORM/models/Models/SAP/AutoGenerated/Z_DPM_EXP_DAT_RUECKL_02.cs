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
	public partial class Z_DPM_EXP_DAT_RUECKL_02
	{
		public static void Init(ISapDataService sap)
		{
			sap.Init(typeof(Z_DPM_EXP_DAT_RUECKL_02).Name);
		}

		public static void Init(ISapDataService sap, string inputParameterKeys, params object[] inputParameterValues)
		{
			sap.Init(typeof(Z_DPM_EXP_DAT_RUECKL_02).Name, inputParameterKeys, inputParameterValues);
		}


		public static void SetImportParameter_I_KUNNR_AG(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_KUNNR_AG", value);
		}

		public partial class GT_EXP_DAT : IModelMappingApplied
		{
			[SapIgnore]
			[ScriptIgnore]
			public ISapConnection SAPConnection { get; set; }

			[SapIgnore]
			[ScriptIgnore]
			public IDynSapProxyFactory DynSapProxyFactory { get; set; }

			public string VORGANGS_ID { get; set; }

			public string FEHLERCODE { get; set; }

			public string STD_ZULETZT_AKTUALISIERT { get; set; }

			public string RUECKGAB_OPTION { get; set; }

			public string KUNNR_HLA { get; set; }

			public string KUNNAME_HLA { get; set; }

			public string NUTZER { get; set; }

			public string VERTRAGSNR_HLA { get; set; }

			public string LICENSE_NUM { get; set; }

			public string CHASSIS_NUM { get; set; }

			public string ERST_ZUL_DAT { get; set; }

			public string HERST { get; set; }

			public string SERIE { get; set; }

			public string KRAFTSTOFF { get; set; }

			public string AUFBAU { get; set; }

			public string LEISTUNG { get; set; }

			public string WINTERREIFEN { get; set; }

			public string ABH_ZULETZT_AKTUALISIERT { get; set; }

			public string TRANSPORTART { get; set; }

			public string WLIEFDAT_VON { get; set; }

			public string WLIEFDAT_BIS { get; set; }

			public string BEM { get; set; }

			public string NAME_ABH { get; set; }

			public string NAME_2_ABH { get; set; }

			public string STREET_ABH { get; set; }

			public string POSTL_CODE_ABH { get; set; }

			public string CITY_ABH { get; set; }

			public string COUNTRY_ABH { get; set; }

			public string TELEPHONE_ABH { get; set; }

			public string SMTP_ADDR_ABH { get; set; }

			public string NAME_ZI { get; set; }

			public string NAME_2_ZI { get; set; }

			public string STREET_ZI { get; set; }

			public string POSTL_CODE_ZI { get; set; }

			public string CITY_ZI { get; set; }

			public string COUNTRY_ZI { get; set; }

			public string TELEPHONE_ZI { get; set; }

			public string SMTP_ADDR_ZI { get; set; }

			public string ABHT_ZULETZT_AKTUALISIERT { get; set; }

			public string BEST_ABH_TERMIN { get; set; }

			public string EIZO_ZULETZT_AKTUALISIERT { get; set; }

			public string DAT_EING_ZO { get; set; }

			public string NAME_ZO { get; set; }

			public string NAME_2_ZO { get; set; }

			public string STREET_ZO { get; set; }

			public string POSTL_CODE_ZO { get; set; }

			public string CITY_ZO { get; set; }

			public string COUNTRY_ZO { get; set; }

			public string TELEPHONE_ZO { get; set; }

			public string SMTP_ADDR_ZO { get; set; }

			public string PROT_EING_O { get; set; }

			public string PROT_EING_D { get; set; }

			public string PROT_ZULETZT_AKTUALISIERT { get; set; }

			public string EING_DAT_PROT { get; set; }

			public string RUECKNAHMEDATUM { get; set; }

			public string RUECKNAHMEZEIT { get; set; }

			public string RUECKNAHMEORT { get; set; }

			public string STANDORT { get; set; }

			public string KM_BEI_UEBERG { get; set; }

			public string KM_BEI_UEBERNAHM { get; set; }

			public string ZB1_SCHGEIN { get; set; }

			public string SERVHEFT_BORDBU { get; set; }

			public string HU_AU_GUELT_BIS { get; set; }

			public string RADIO_CODCARD_NR { get; set; }

			public string ORIG_NAVI_DVD_CD { get; set; }

			public string BORDWERKZEUG { get; set; }

			public string RESERVERAD { get; set; }

			public string REIF_REP_SET { get; set; }

			public string LADERAUMABDECK { get; set; }

			public string RUECKN_VORBEH { get; set; }

			public string RUE_SCHAED_I { get; set; }

			public string RUE_SCHAED_A { get; set; }

			public string BEMERKUNG { get; set; }

			public string PROT_ABHOLUNG { get; set; }

			public string STIL_ZULETZT_AKTUALISIERT { get; set; }

			public string ABMELDEDATUM { get; set; }

			public string AUFB_ZULETZT_AKTUALISIERT { get; set; }

			public string EING_AUFBER_AUFTR { get; set; }

			public string BEM_AUFB { get; set; }

			public string AUBF_ZULETZT_AKTUALISIERT { get; set; }

			public string AUFBER_FERTIG { get; set; }

			public string VERW_ZULETZT_AKTUALISIERT { get; set; }

			public string ENTSCHEIDUNG { get; set; }

			public string VERKAUFSKANAL { get; set; }

			public string EING_ZULETZT_AKTUALISIERT { get; set; }

			public string ANGEBOTSNR { get; set; }

			public string LINK { get; set; }

			public string VABG_ZULETZT_AKTUALISIERT { get; set; }

			public string VORG_ABGESCHL { get; set; }

			public string PROT_EING_NAMEO { get; set; }

			public string PROT_EING_NAMED { get; set; }

			public string EIG_ANL { get; set; }

			public string GUTA_ERSTELLEN { get; set; }

			public static GT_EXP_DAT Create(DataRow row, ISapConnection sapConnection = null, IDynSapProxyFactory dynSapProxyFactory = null)
			{
				var o = new GT_EXP_DAT
				{
					VORGANGS_ID = (string)row["VORGANGS_ID"],
					FEHLERCODE = (string)row["FEHLERCODE"],
					STD_ZULETZT_AKTUALISIERT = (string)row["STD_ZULETZT_AKTUALISIERT"],
					RUECKGAB_OPTION = (string)row["RUECKGAB_OPTION"],
					KUNNR_HLA = (string)row["KUNNR_HLA"],
					KUNNAME_HLA = (string)row["KUNNAME_HLA"],
					NUTZER = (string)row["NUTZER"],
					VERTRAGSNR_HLA = (string)row["VERTRAGSNR_HLA"],
					LICENSE_NUM = (string)row["LICENSE_NUM"],
					CHASSIS_NUM = (string)row["CHASSIS_NUM"],
					ERST_ZUL_DAT = (string)row["ERST_ZUL_DAT"],
					HERST = (string)row["HERST"],
					SERIE = (string)row["SERIE"],
					KRAFTSTOFF = (string)row["KRAFTSTOFF"],
					AUFBAU = (string)row["AUFBAU"],
					LEISTUNG = (string)row["LEISTUNG"],
					WINTERREIFEN = (string)row["WINTERREIFEN"],
					ABH_ZULETZT_AKTUALISIERT = (string)row["ABH_ZULETZT_AKTUALISIERT"],
					TRANSPORTART = (string)row["TRANSPORTART"],
					WLIEFDAT_VON = (string)row["WLIEFDAT_VON"],
					WLIEFDAT_BIS = (string)row["WLIEFDAT_BIS"],
					BEM = (string)row["BEM"],
					NAME_ABH = (string)row["NAME_ABH"],
					NAME_2_ABH = (string)row["NAME_2_ABH"],
					STREET_ABH = (string)row["STREET_ABH"],
					POSTL_CODE_ABH = (string)row["POSTL_CODE_ABH"],
					CITY_ABH = (string)row["CITY_ABH"],
					COUNTRY_ABH = (string)row["COUNTRY_ABH"],
					TELEPHONE_ABH = (string)row["TELEPHONE_ABH"],
					SMTP_ADDR_ABH = (string)row["SMTP_ADDR_ABH"],
					NAME_ZI = (string)row["NAME_ZI"],
					NAME_2_ZI = (string)row["NAME_2_ZI"],
					STREET_ZI = (string)row["STREET_ZI"],
					POSTL_CODE_ZI = (string)row["POSTL_CODE_ZI"],
					CITY_ZI = (string)row["CITY_ZI"],
					COUNTRY_ZI = (string)row["COUNTRY_ZI"],
					TELEPHONE_ZI = (string)row["TELEPHONE_ZI"],
					SMTP_ADDR_ZI = (string)row["SMTP_ADDR_ZI"],
					ABHT_ZULETZT_AKTUALISIERT = (string)row["ABHT_ZULETZT_AKTUALISIERT"],
					BEST_ABH_TERMIN = (string)row["BEST_ABH_TERMIN"],
					EIZO_ZULETZT_AKTUALISIERT = (string)row["EIZO_ZULETZT_AKTUALISIERT"],
					DAT_EING_ZO = (string)row["DAT_EING_ZO"],
					NAME_ZO = (string)row["NAME_ZO"],
					NAME_2_ZO = (string)row["NAME_2_ZO"],
					STREET_ZO = (string)row["STREET_ZO"],
					POSTL_CODE_ZO = (string)row["POSTL_CODE_ZO"],
					CITY_ZO = (string)row["CITY_ZO"],
					COUNTRY_ZO = (string)row["COUNTRY_ZO"],
					TELEPHONE_ZO = (string)row["TELEPHONE_ZO"],
					SMTP_ADDR_ZO = (string)row["SMTP_ADDR_ZO"],
					PROT_EING_O = (string)row["PROT_EING_O"],
					PROT_EING_D = (string)row["PROT_EING_D"],
					PROT_ZULETZT_AKTUALISIERT = (string)row["PROT_ZULETZT_AKTUALISIERT"],
					EING_DAT_PROT = (string)row["EING_DAT_PROT"],
					RUECKNAHMEDATUM = (string)row["RUECKNAHMEDATUM"],
					RUECKNAHMEZEIT = (string)row["RUECKNAHMEZEIT"],
					RUECKNAHMEORT = (string)row["RUECKNAHMEORT"],
					STANDORT = (string)row["STANDORT"],
					KM_BEI_UEBERG = (string)row["KM_BEI_UEBERG"],
					KM_BEI_UEBERNAHM = (string)row["KM_BEI_UEBERNAHM"],
					ZB1_SCHGEIN = (string)row["ZB1_SCHGEIN"],
					SERVHEFT_BORDBU = (string)row["SERVHEFT_BORDBU"],
					HU_AU_GUELT_BIS = (string)row["HU_AU_GUELT_BIS"],
					RADIO_CODCARD_NR = (string)row["RADIO_CODCARD_NR"],
					ORIG_NAVI_DVD_CD = (string)row["ORIG_NAVI_DVD_CD"],
					BORDWERKZEUG = (string)row["BORDWERKZEUG"],
					RESERVERAD = (string)row["RESERVERAD"],
					REIF_REP_SET = (string)row["REIF_REP_SET"],
					LADERAUMABDECK = (string)row["LADERAUMABDECK"],
					RUECKN_VORBEH = (string)row["RUECKN_VORBEH"],
					RUE_SCHAED_I = (string)row["RUE_SCHAED_I"],
					RUE_SCHAED_A = (string)row["RUE_SCHAED_A"],
					BEMERKUNG = (string)row["BEMERKUNG"],
					PROT_ABHOLUNG = (string)row["PROT_ABHOLUNG"],
					STIL_ZULETZT_AKTUALISIERT = (string)row["STIL_ZULETZT_AKTUALISIERT"],
					ABMELDEDATUM = (string)row["ABMELDEDATUM"],
					AUFB_ZULETZT_AKTUALISIERT = (string)row["AUFB_ZULETZT_AKTUALISIERT"],
					EING_AUFBER_AUFTR = (string)row["EING_AUFBER_AUFTR"],
					BEM_AUFB = (string)row["BEM_AUFB"],
					AUBF_ZULETZT_AKTUALISIERT = (string)row["AUBF_ZULETZT_AKTUALISIERT"],
					AUFBER_FERTIG = (string)row["AUFBER_FERTIG"],
					VERW_ZULETZT_AKTUALISIERT = (string)row["VERW_ZULETZT_AKTUALISIERT"],
					ENTSCHEIDUNG = (string)row["ENTSCHEIDUNG"],
					VERKAUFSKANAL = (string)row["VERKAUFSKANAL"],
					EING_ZULETZT_AKTUALISIERT = (string)row["EING_ZULETZT_AKTUALISIERT"],
					ANGEBOTSNR = (string)row["ANGEBOTSNR"],
					LINK = (string)row["LINK"],
					VABG_ZULETZT_AKTUALISIERT = (string)row["VABG_ZULETZT_AKTUALISIERT"],
					VORG_ABGESCHL = (string)row["VORG_ABGESCHL"],
					PROT_EING_NAMEO = (string)row["PROT_EING_NAMEO"],
					PROT_EING_NAMED = (string)row["PROT_EING_NAMED"],
					EIG_ANL = (string)row["EIG_ANL"],
					GUTA_ERSTELLEN = (string)row["GUTA_ERSTELLEN"],

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

			public static IEnumerable<GT_EXP_DAT> Select(DataTable dt, ISapConnection sapConnection = null)
			{
				return dt.AsEnumerable().Select(r => Create(r, sapConnection));
			}

			public static List<GT_EXP_DAT> ToList(DataTable dt, ISapConnection sapConnection = null)
			{
				return Select(dt, sapConnection).ToListOrEmptyList();
			}

			public static IEnumerable<GT_EXP_DAT> Select(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				var tbl = dts.FirstOrDefault(t => t.TableName.ToLower() == typeof(GT_EXP_DAT).Name.ToLower());
				if (tbl == null)
					return null;

				return Select(tbl, sapConnection);
			}

			public static List<GT_EXP_DAT> ToList(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				return Select(dts, sapConnection).ToListOrEmptyList();
			}

			public static List<GT_EXP_DAT> ToList(ISapDataService sapDataService)
			{
				return ToList(sapDataService.GetExportTables(), sapDataService.SapConnection);
			}

			public static List<GT_EXP_DAT> GetExportListWithInitExecute(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<GT_EXP_DAT>();
				 
				var dts = sapDataService.GetExportTablesWithInitExecute("Z_DPM_EXP_DAT_RUECKL_02", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_EXP_DAT> GetExportListWithExecute(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_EXP_DAT>();
				 
				var dts = sapDataService.GetExportTablesWithExecute();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_EXP_DAT> GetExportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_EXP_DAT>();
				 
				var dts = sapDataService.GetExportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_EXP_DAT> GetImportListWithInit(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<GT_EXP_DAT>();
				 
				var dts = sapDataService.GetImportTablesWithInit("Z_DPM_EXP_DAT_RUECKL_02", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_EXP_DAT> GetImportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_EXP_DAT>();
				 
				var dts = sapDataService.GetImportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}
		}

		public partial class GT_EXP_REP : IModelMappingApplied
		{
			[SapIgnore]
			[ScriptIgnore]
			public ISapConnection SAPConnection { get; set; }

			[SapIgnore]
			[ScriptIgnore]
			public IDynSapProxyFactory DynSapProxyFactory { get; set; }

			public string VORGANGS_ID { get; set; }

			public string CODE { get; set; }

			public string BEZEICHNUNG { get; set; }

			public string MASSNAHME { get; set; }

			public string REP_KOSTEN { get; set; }

			public string TYP { get; set; }

			public static GT_EXP_REP Create(DataRow row, ISapConnection sapConnection = null, IDynSapProxyFactory dynSapProxyFactory = null)
			{
				var o = new GT_EXP_REP
				{
					VORGANGS_ID = (string)row["VORGANGS_ID"],
					CODE = (string)row["CODE"],
					BEZEICHNUNG = (string)row["BEZEICHNUNG"],
					MASSNAHME = (string)row["MASSNAHME"],
					REP_KOSTEN = (string)row["REP_KOSTEN"],
					TYP = (string)row["TYP"],

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

			public static IEnumerable<GT_EXP_REP> Select(DataTable dt, ISapConnection sapConnection = null)
			{
				return dt.AsEnumerable().Select(r => Create(r, sapConnection));
			}

			public static List<GT_EXP_REP> ToList(DataTable dt, ISapConnection sapConnection = null)
			{
				return Select(dt, sapConnection).ToListOrEmptyList();
			}

			public static IEnumerable<GT_EXP_REP> Select(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				var tbl = dts.FirstOrDefault(t => t.TableName.ToLower() == typeof(GT_EXP_REP).Name.ToLower());
				if (tbl == null)
					return null;

				return Select(tbl, sapConnection);
			}

			public static List<GT_EXP_REP> ToList(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				return Select(dts, sapConnection).ToListOrEmptyList();
			}

			public static List<GT_EXP_REP> ToList(ISapDataService sapDataService)
			{
				return ToList(sapDataService.GetExportTables(), sapDataService.SapConnection);
			}

			public static List<GT_EXP_REP> GetExportListWithInitExecute(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<GT_EXP_REP>();
				 
				var dts = sapDataService.GetExportTablesWithInitExecute("Z_DPM_EXP_DAT_RUECKL_02", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_EXP_REP> GetExportListWithExecute(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_EXP_REP>();
				 
				var dts = sapDataService.GetExportTablesWithExecute();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_EXP_REP> GetExportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_EXP_REP>();
				 
				var dts = sapDataService.GetExportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_EXP_REP> GetImportListWithInit(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<GT_EXP_REP>();
				 
				var dts = sapDataService.GetImportTablesWithInit("Z_DPM_EXP_DAT_RUECKL_02", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_EXP_REP> GetImportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_EXP_REP>();
				 
				var dts = sapDataService.GetImportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}
		}

		public partial class GT_EXP_TRANSP : IModelMappingApplied
		{
			[SapIgnore]
			[ScriptIgnore]
			public ISapConnection SAPConnection { get; set; }

			[SapIgnore]
			[ScriptIgnore]
			public IDynSapProxyFactory DynSapProxyFactory { get; set; }

			public string VORGANGS_ID { get; set; }

			public string ZULETZT_AKTUALISIERT { get; set; }

			public string TRANSPORTART { get; set; }

			public string WLIEFDAT_VON { get; set; }

			public string WLIEFDAT_BIS { get; set; }

			public string BEM { get; set; }

			public string NUMMER { get; set; }

			public string NAME_ABH { get; set; }

			public string NAME_2_ABH { get; set; }

			public string STREET_ABH { get; set; }

			public string POSTL_CODE_ABH { get; set; }

			public string CITY_ABH { get; set; }

			public string COUNTRY_ABH { get; set; }

			public string TELEPHONE_ABH { get; set; }

			public string SMTP_ADDR_ABH { get; set; }

			public string NAME_ZI { get; set; }

			public string NAME_2_ZI { get; set; }

			public string STREET_ZI { get; set; }

			public string POSTL_CODE_ZI { get; set; }

			public string CITY_ZI { get; set; }

			public string COUNTRY_ZI { get; set; }

			public string TELEPHONE_ZI { get; set; }

			public string SMTP_ADDR_ZI { get; set; }

			public string ABHT_ZULETZT_AKTUALISIERT { get; set; }

			public string BEST_ABH_TERMIN { get; set; }

			public string EIZO_ZULETZT_AKTUALISIERT { get; set; }

			public string DAT_EING_ZO { get; set; }

			public string NAME_ZO { get; set; }

			public string NAME_2_ZO { get; set; }

			public string STREET_ZO { get; set; }

			public string POSTL_CODE_ZO { get; set; }

			public string CITY_ZO { get; set; }

			public string COUNTRY_ZO { get; set; }

			public string TELEPHONE_ZO { get; set; }

			public string SMTP_ADDR_ZO { get; set; }

			public string PROT_EING_O { get; set; }

			public string PROT_EING_D { get; set; }

			public string PROT_EING_NAMEO { get; set; }

			public string PROT_EING_NAMED { get; set; }

			public static GT_EXP_TRANSP Create(DataRow row, ISapConnection sapConnection = null, IDynSapProxyFactory dynSapProxyFactory = null)
			{
				var o = new GT_EXP_TRANSP
				{
					VORGANGS_ID = (string)row["VORGANGS_ID"],
					ZULETZT_AKTUALISIERT = (string)row["ZULETZT_AKTUALISIERT"],
					TRANSPORTART = (string)row["TRANSPORTART"],
					WLIEFDAT_VON = (string)row["WLIEFDAT_VON"],
					WLIEFDAT_BIS = (string)row["WLIEFDAT_BIS"],
					BEM = (string)row["BEM"],
					NUMMER = (string)row["NUMMER"],
					NAME_ABH = (string)row["NAME_ABH"],
					NAME_2_ABH = (string)row["NAME_2_ABH"],
					STREET_ABH = (string)row["STREET_ABH"],
					POSTL_CODE_ABH = (string)row["POSTL_CODE_ABH"],
					CITY_ABH = (string)row["CITY_ABH"],
					COUNTRY_ABH = (string)row["COUNTRY_ABH"],
					TELEPHONE_ABH = (string)row["TELEPHONE_ABH"],
					SMTP_ADDR_ABH = (string)row["SMTP_ADDR_ABH"],
					NAME_ZI = (string)row["NAME_ZI"],
					NAME_2_ZI = (string)row["NAME_2_ZI"],
					STREET_ZI = (string)row["STREET_ZI"],
					POSTL_CODE_ZI = (string)row["POSTL_CODE_ZI"],
					CITY_ZI = (string)row["CITY_ZI"],
					COUNTRY_ZI = (string)row["COUNTRY_ZI"],
					TELEPHONE_ZI = (string)row["TELEPHONE_ZI"],
					SMTP_ADDR_ZI = (string)row["SMTP_ADDR_ZI"],
					ABHT_ZULETZT_AKTUALISIERT = (string)row["ABHT_ZULETZT_AKTUALISIERT"],
					BEST_ABH_TERMIN = (string)row["BEST_ABH_TERMIN"],
					EIZO_ZULETZT_AKTUALISIERT = (string)row["EIZO_ZULETZT_AKTUALISIERT"],
					DAT_EING_ZO = (string)row["DAT_EING_ZO"],
					NAME_ZO = (string)row["NAME_ZO"],
					NAME_2_ZO = (string)row["NAME_2_ZO"],
					STREET_ZO = (string)row["STREET_ZO"],
					POSTL_CODE_ZO = (string)row["POSTL_CODE_ZO"],
					CITY_ZO = (string)row["CITY_ZO"],
					COUNTRY_ZO = (string)row["COUNTRY_ZO"],
					TELEPHONE_ZO = (string)row["TELEPHONE_ZO"],
					SMTP_ADDR_ZO = (string)row["SMTP_ADDR_ZO"],
					PROT_EING_O = (string)row["PROT_EING_O"],
					PROT_EING_D = (string)row["PROT_EING_D"],
					PROT_EING_NAMEO = (string)row["PROT_EING_NAMEO"],
					PROT_EING_NAMED = (string)row["PROT_EING_NAMED"],

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

			public static IEnumerable<GT_EXP_TRANSP> Select(DataTable dt, ISapConnection sapConnection = null)
			{
				return dt.AsEnumerable().Select(r => Create(r, sapConnection));
			}

			public static List<GT_EXP_TRANSP> ToList(DataTable dt, ISapConnection sapConnection = null)
			{
				return Select(dt, sapConnection).ToListOrEmptyList();
			}

			public static IEnumerable<GT_EXP_TRANSP> Select(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				var tbl = dts.FirstOrDefault(t => t.TableName.ToLower() == typeof(GT_EXP_TRANSP).Name.ToLower());
				if (tbl == null)
					return null;

				return Select(tbl, sapConnection);
			}

			public static List<GT_EXP_TRANSP> ToList(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				return Select(dts, sapConnection).ToListOrEmptyList();
			}

			public static List<GT_EXP_TRANSP> ToList(ISapDataService sapDataService)
			{
				return ToList(sapDataService.GetExportTables(), sapDataService.SapConnection);
			}

			public static List<GT_EXP_TRANSP> GetExportListWithInitExecute(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<GT_EXP_TRANSP>();
				 
				var dts = sapDataService.GetExportTablesWithInitExecute("Z_DPM_EXP_DAT_RUECKL_02", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_EXP_TRANSP> GetExportListWithExecute(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_EXP_TRANSP>();
				 
				var dts = sapDataService.GetExportTablesWithExecute();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_EXP_TRANSP> GetExportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_EXP_TRANSP>();
				 
				var dts = sapDataService.GetExportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_EXP_TRANSP> GetImportListWithInit(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<GT_EXP_TRANSP>();
				 
				var dts = sapDataService.GetImportTablesWithInit("Z_DPM_EXP_DAT_RUECKL_02", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_EXP_TRANSP> GetImportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_EXP_TRANSP>();
				 
				var dts = sapDataService.GetImportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}
		}

		public partial class GT_IMP_VORGANGSID : IModelMappingApplied
		{
			[SapIgnore]
			[ScriptIgnore]
			public ISapConnection SAPConnection { get; set; }

			[SapIgnore]
			[ScriptIgnore]
			public IDynSapProxyFactory DynSapProxyFactory { get; set; }

			public string VORGANGS_ID { get; set; }

			public static GT_IMP_VORGANGSID Create(DataRow row, ISapConnection sapConnection = null, IDynSapProxyFactory dynSapProxyFactory = null)
			{
				var o = new GT_IMP_VORGANGSID
				{
					VORGANGS_ID = (string)row["VORGANGS_ID"],

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

			public static IEnumerable<GT_IMP_VORGANGSID> Select(DataTable dt, ISapConnection sapConnection = null)
			{
				return dt.AsEnumerable().Select(r => Create(r, sapConnection));
			}

			public static List<GT_IMP_VORGANGSID> ToList(DataTable dt, ISapConnection sapConnection = null)
			{
				return Select(dt, sapConnection).ToListOrEmptyList();
			}

			public static IEnumerable<GT_IMP_VORGANGSID> Select(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				var tbl = dts.FirstOrDefault(t => t.TableName.ToLower() == typeof(GT_IMP_VORGANGSID).Name.ToLower());
				if (tbl == null)
					return null;

				return Select(tbl, sapConnection);
			}

			public static List<GT_IMP_VORGANGSID> ToList(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				return Select(dts, sapConnection).ToListOrEmptyList();
			}

			public static List<GT_IMP_VORGANGSID> ToList(ISapDataService sapDataService)
			{
				return ToList(sapDataService.GetExportTables(), sapDataService.SapConnection);
			}

			public static List<GT_IMP_VORGANGSID> GetExportListWithInitExecute(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<GT_IMP_VORGANGSID>();
				 
				var dts = sapDataService.GetExportTablesWithInitExecute("Z_DPM_EXP_DAT_RUECKL_02", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_IMP_VORGANGSID> GetExportListWithExecute(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_IMP_VORGANGSID>();
				 
				var dts = sapDataService.GetExportTablesWithExecute();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_IMP_VORGANGSID> GetExportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_IMP_VORGANGSID>();
				 
				var dts = sapDataService.GetExportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_IMP_VORGANGSID> GetImportListWithInit(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<GT_IMP_VORGANGSID>();
				 
				var dts = sapDataService.GetImportTablesWithInit("Z_DPM_EXP_DAT_RUECKL_02", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_IMP_VORGANGSID> GetImportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_IMP_VORGANGSID>();
				 
				var dts = sapDataService.GetImportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}
		}
	}

	public static partial class DataTableExtensions
	{

		public static DataTable ToTable(this IEnumerable<Z_DPM_EXP_DAT_RUECKL_02.GT_EXP_DAT> list)
		{
			return SapDataServiceExtensions.ToTable(list);
		}


		public static DataTable ToTable(this IEnumerable<Z_DPM_EXP_DAT_RUECKL_02.GT_EXP_REP> list)
		{
			return SapDataServiceExtensions.ToTable(list);
		}


		public static DataTable ToTable(this IEnumerable<Z_DPM_EXP_DAT_RUECKL_02.GT_EXP_TRANSP> list)
		{
			return SapDataServiceExtensions.ToTable(list);
		}


		public static DataTable ToTable(this IEnumerable<Z_DPM_EXP_DAT_RUECKL_02.GT_IMP_VORGANGSID> list)
		{
			return SapDataServiceExtensions.ToTable(list);
		}

	}
}
