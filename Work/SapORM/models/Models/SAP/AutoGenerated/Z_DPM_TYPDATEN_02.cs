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
	public partial class Z_DPM_TYPDATEN_02
	{
		public static void Init(ISapDataService sap)
		{
			sap.Init(typeof(Z_DPM_TYPDATEN_02).Name);
		}

		public static void Init(ISapDataService sap, string inputParameterKeys, params object[] inputParameterValues)
		{
			sap.Init(typeof(Z_DPM_TYPDATEN_02).Name, inputParameterKeys, inputParameterValues);
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

		public partial class GT_WEB : IModelMappingApplied
		{
			[SapIgnore]
			[ScriptIgnore]
			public ISapConnection SAPConnection { get; set; }

			[SapIgnore]
			[ScriptIgnore]
			public IDynSapProxyFactory DynSapProxyFactory { get; set; }

			public string ZZUNGUELTIG { get; set; }

			public string ZZRESERVE { get; set; }

			public string ZZHERSTELLER_SCH { get; set; }

			public string ZZTYP_SCHL { get; set; }

			public string ZZVVS_SCHLUESSEL { get; set; }

			public string ZZTYP_VVS_PRUEF { get; set; }

			public string ZZFAHRZEUGKLASSE { get; set; }

			public string ZZCODE_AUFBAU { get; set; }

			public string ZZFABRIKNAME { get; set; }

			public string ZZKLARTEXT_TYP { get; set; }

			public string ZZVARIANTE { get; set; }

			public string ZZVERSION { get; set; }

			public string ZZHANDELSNAME { get; set; }

			public string ZZHERST_TEXT { get; set; }

			public string ZZFHRZKLASSE_TXT { get; set; }

			public string ZZTEXT_AUFBAU { get; set; }

			public string ZZABGASRICHTL_TG { get; set; }

			public string ZZNATIONALE_EMIK { get; set; }

			public string ZZKRAFTSTOFF_TXT { get; set; }

			public string ZZCODE_KRAFTSTOF { get; set; }

			public string ZZSLD { get; set; }

			public string ZZHUBRAUM { get; set; }

			public string ZZANZACHS { get; set; }

			public string ZZANTRIEBSACHS { get; set; }

			public string ZZNENNLEISTUNG { get; set; }

			public string ZZBEIUMDREH { get; set; }

			public string ZZHOECHSTGESCHW { get; set; }

			public string ZZFASSVERMOEGEN { get; set; }

			public string ZZANZSITZE { get; set; }

			public string ZZANZSTEHPLAETZE { get; set; }

			public string ZZMASSEFAHRBMIN { get; set; }

			public string ZZMASSEFAHRBMAX { get; set; }

			public string ZZZULGESGEW { get; set; }

			public string ZZZULGESGEWSTAAT { get; set; }

			public string ZZACHSLST_ACHSE1 { get; set; }

			public string ZZACHSLST_ACHSE2 { get; set; }

			public string ZZACHSLST_ACHSE3 { get; set; }

			public string ZZACHSL_A1_STA { get; set; }

			public string ZZACHSL_A2_STA { get; set; }

			public string ZZACHSL_A3_STA { get; set; }

			public string ZZCO2KOMBI { get; set; }

			public string ZZSTANDGERAEUSCH { get; set; }

			public string ZZDREHZSTANDGER { get; set; }

			public string ZZFAHRGERAEUSCH { get; set; }

			public string ZZANHLAST_GEBR { get; set; }

			public string ZZANHLAST_UNGEBR { get; set; }

			public string ZZLEISTUNGSGEW { get; set; }

			public string ZZLAENGEMIN { get; set; }

			public string ZZLAENGEMAX { get; set; }

			public string ZZBREITEMIN { get; set; }

			public string ZZBREITEMAX { get; set; }

			public string ZZHOEHEMIN { get; set; }

			public string ZZHOEHEMAX { get; set; }

			public string ZZSTUETZLAST { get; set; }

			public string ZZBEREIFACHSE1 { get; set; }

			public string ZZBEREIFACHSE2 { get; set; }

			public string ZZBEREIFACHSE3 { get; set; }

			public string ZZGENEHMIGNR { get; set; }

			public string ZZGENEHMIGDAT { get; set; }

			public string ZZBEMER1 { get; set; }

			public string ZZBEMER2 { get; set; }

			public string ZZBEMER3 { get; set; }

			public string ZZBEMER4 { get; set; }

			public string ZZBEMER5 { get; set; }

			public string ZZBEMER6 { get; set; }

			public string ZZBEMER7 { get; set; }

			public string ZZBEMER8 { get; set; }

			public string ZZBEMER9 { get; set; }

			public string ZZBEMER10 { get; set; }

			public string ZZBEMER11 { get; set; }

			public string ZZBEMER12 { get; set; }

			public string ZZBEMER13 { get; set; }

			public string ZZBEMER14 { get; set; }

			public static GT_WEB Create(DataRow row, ISapConnection sapConnection = null, IDynSapProxyFactory dynSapProxyFactory = null)
			{
				var o = new GT_WEB
				{
					ZZUNGUELTIG = (string)row["ZZUNGUELTIG"],
					ZZRESERVE = (string)row["ZZRESERVE"],
					ZZHERSTELLER_SCH = (string)row["ZZHERSTELLER_SCH"],
					ZZTYP_SCHL = (string)row["ZZTYP_SCHL"],
					ZZVVS_SCHLUESSEL = (string)row["ZZVVS_SCHLUESSEL"],
					ZZTYP_VVS_PRUEF = (string)row["ZZTYP_VVS_PRUEF"],
					ZZFAHRZEUGKLASSE = (string)row["ZZFAHRZEUGKLASSE"],
					ZZCODE_AUFBAU = (string)row["ZZCODE_AUFBAU"],
					ZZFABRIKNAME = (string)row["ZZFABRIKNAME"],
					ZZKLARTEXT_TYP = (string)row["ZZKLARTEXT_TYP"],
					ZZVARIANTE = (string)row["ZZVARIANTE"],
					ZZVERSION = (string)row["ZZVERSION"],
					ZZHANDELSNAME = (string)row["ZZHANDELSNAME"],
					ZZHERST_TEXT = (string)row["ZZHERST_TEXT"],
					ZZFHRZKLASSE_TXT = (string)row["ZZFHRZKLASSE_TXT"],
					ZZTEXT_AUFBAU = (string)row["ZZTEXT_AUFBAU"],
					ZZABGASRICHTL_TG = (string)row["ZZABGASRICHTL_TG"],
					ZZNATIONALE_EMIK = (string)row["ZZNATIONALE_EMIK"],
					ZZKRAFTSTOFF_TXT = (string)row["ZZKRAFTSTOFF_TXT"],
					ZZCODE_KRAFTSTOF = (string)row["ZZCODE_KRAFTSTOF"],
					ZZSLD = (string)row["ZZSLD"],
					ZZHUBRAUM = (string)row["ZZHUBRAUM"],
					ZZANZACHS = (string)row["ZZANZACHS"],
					ZZANTRIEBSACHS = (string)row["ZZANTRIEBSACHS"],
					ZZNENNLEISTUNG = (string)row["ZZNENNLEISTUNG"],
					ZZBEIUMDREH = (string)row["ZZBEIUMDREH"],
					ZZHOECHSTGESCHW = (string)row["ZZHOECHSTGESCHW"],
					ZZFASSVERMOEGEN = (string)row["ZZFASSVERMOEGEN"],
					ZZANZSITZE = (string)row["ZZANZSITZE"],
					ZZANZSTEHPLAETZE = (string)row["ZZANZSTEHPLAETZE"],
					ZZMASSEFAHRBMIN = (string)row["ZZMASSEFAHRBMIN"],
					ZZMASSEFAHRBMAX = (string)row["ZZMASSEFAHRBMAX"],
					ZZZULGESGEW = (string)row["ZZZULGESGEW"],
					ZZZULGESGEWSTAAT = (string)row["ZZZULGESGEWSTAAT"],
					ZZACHSLST_ACHSE1 = (string)row["ZZACHSLST_ACHSE1"],
					ZZACHSLST_ACHSE2 = (string)row["ZZACHSLST_ACHSE2"],
					ZZACHSLST_ACHSE3 = (string)row["ZZACHSLST_ACHSE3"],
					ZZACHSL_A1_STA = (string)row["ZZACHSL_A1_STA"],
					ZZACHSL_A2_STA = (string)row["ZZACHSL_A2_STA"],
					ZZACHSL_A3_STA = (string)row["ZZACHSL_A3_STA"],
					ZZCO2KOMBI = (string)row["ZZCO2KOMBI"],
					ZZSTANDGERAEUSCH = (string)row["ZZSTANDGERAEUSCH"],
					ZZDREHZSTANDGER = (string)row["ZZDREHZSTANDGER"],
					ZZFAHRGERAEUSCH = (string)row["ZZFAHRGERAEUSCH"],
					ZZANHLAST_GEBR = (string)row["ZZANHLAST_GEBR"],
					ZZANHLAST_UNGEBR = (string)row["ZZANHLAST_UNGEBR"],
					ZZLEISTUNGSGEW = (string)row["ZZLEISTUNGSGEW"],
					ZZLAENGEMIN = (string)row["ZZLAENGEMIN"],
					ZZLAENGEMAX = (string)row["ZZLAENGEMAX"],
					ZZBREITEMIN = (string)row["ZZBREITEMIN"],
					ZZBREITEMAX = (string)row["ZZBREITEMAX"],
					ZZHOEHEMIN = (string)row["ZZHOEHEMIN"],
					ZZHOEHEMAX = (string)row["ZZHOEHEMAX"],
					ZZSTUETZLAST = (string)row["ZZSTUETZLAST"],
					ZZBEREIFACHSE1 = (string)row["ZZBEREIFACHSE1"],
					ZZBEREIFACHSE2 = (string)row["ZZBEREIFACHSE2"],
					ZZBEREIFACHSE3 = (string)row["ZZBEREIFACHSE3"],
					ZZGENEHMIGNR = (string)row["ZZGENEHMIGNR"],
					ZZGENEHMIGDAT = (string)row["ZZGENEHMIGDAT"],
					ZZBEMER1 = (string)row["ZZBEMER1"],
					ZZBEMER2 = (string)row["ZZBEMER2"],
					ZZBEMER3 = (string)row["ZZBEMER3"],
					ZZBEMER4 = (string)row["ZZBEMER4"],
					ZZBEMER5 = (string)row["ZZBEMER5"],
					ZZBEMER6 = (string)row["ZZBEMER6"],
					ZZBEMER7 = (string)row["ZZBEMER7"],
					ZZBEMER8 = (string)row["ZZBEMER8"],
					ZZBEMER9 = (string)row["ZZBEMER9"],
					ZZBEMER10 = (string)row["ZZBEMER10"],
					ZZBEMER11 = (string)row["ZZBEMER11"],
					ZZBEMER12 = (string)row["ZZBEMER12"],
					ZZBEMER13 = (string)row["ZZBEMER13"],
					ZZBEMER14 = (string)row["ZZBEMER14"],

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

			public static IEnumerable<GT_WEB> Select(DataTable dt, ISapConnection sapConnection = null)
			{
				return dt.AsEnumerable().Select(r => Create(r, sapConnection));
			}

			public static List<GT_WEB> ToList(DataTable dt, ISapConnection sapConnection = null)
			{
				return Select(dt, sapConnection).ToListOrEmptyList();
			}

			public static IEnumerable<GT_WEB> Select(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				var tbl = dts.FirstOrDefault(t => t.TableName.ToLower() == typeof(GT_WEB).Name.ToLower());
				if (tbl == null)
					return null;

				return Select(tbl, sapConnection);
			}

			public static List<GT_WEB> ToList(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				return Select(dts, sapConnection).ToListOrEmptyList();
			}

			public static List<GT_WEB> ToList(ISapDataService sapDataService)
			{
				return ToList(sapDataService.GetExportTables(), sapDataService.SapConnection);
			}

			public static List<GT_WEB> GetExportListWithInitExecute(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<GT_WEB>();
				 
				var dts = sapDataService.GetExportTablesWithInitExecute("Z_DPM_TYPDATEN_02", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_WEB> GetExportListWithExecute(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_WEB>();
				 
				var dts = sapDataService.GetExportTablesWithExecute();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_WEB> GetExportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_WEB>();
				 
				var dts = sapDataService.GetExportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_WEB> GetImportListWithInit(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<GT_WEB>();
				 
				var dts = sapDataService.GetImportTablesWithInit("Z_DPM_TYPDATEN_02", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_WEB> GetImportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_WEB>();
				 
				var dts = sapDataService.GetImportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}
		}
	}

	public static partial class DataTableExtensions
	{

		public static DataTable ToTable(this IEnumerable<Z_DPM_TYPDATEN_02.GT_WEB> list)
		{
			return SapDataServiceExtensions.ToTable(list);
		}

	}
}
