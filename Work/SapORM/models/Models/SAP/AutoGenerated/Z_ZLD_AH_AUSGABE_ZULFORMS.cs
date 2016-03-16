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
	public partial class Z_ZLD_AH_AUSGABE_ZULFORMS
	{
		public static void Init(ISapDataService sap)
		{
			sap.Init(typeof(Z_ZLD_AH_AUSGABE_ZULFORMS).Name);
		}

		public static void Init(ISapDataService sap, string inputParameterKeys, params object[] inputParameterValues)
		{
			sap.Init(typeof(Z_ZLD_AH_AUSGABE_ZULFORMS).Name, inputParameterKeys, inputParameterValues);
		}


		public static void SetImportParameter_I_KREISKZ(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_KREISKZ", value);
		}

		public static void SetImportParameter_I_KUNNR_AG(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_KUNNR_AG", value);
		}

		public partial class GT_BAK_IN : IModelMappingApplied
		{
			[SapIgnore]
			[ScriptIgnore]
			public ISapConnection SAPConnection { get; set; }

			[SapIgnore]
			[ScriptIgnore]
			public IDynSapProxyFactory DynSapProxyFactory { get; set; }

			public string ZULBELN { get; set; }

			public string VKORG { get; set; }

			public string VKBUR { get; set; }

			public string VE_ERNAM { get; set; }

			public string BLTYP { get; set; }

			public string VZD_VKBUR { get; set; }

			public string KUNNR { get; set; }

			public string ZZREFNR1 { get; set; }

			public string ZZREFNR2 { get; set; }

			public string ZZREFNR3 { get; set; }

			public string ZZREFNR4 { get; set; }

			public string KREISKZ { get; set; }

			public string KREISBEZ { get; set; }

			public string WUNSCHKENN_JN { get; set; }

			public string RESERVKENN_JN { get; set; }

			public string RESERVKENN { get; set; }

			public string FEINSTAUB { get; set; }

			public DateTime? ZZZLDAT { get; set; }

			public string ZZKENN { get; set; }

			public string WU_KENNZ2 { get; set; }

			public string WU_KENNZ3 { get; set; }

			public string KENNZTYP { get; set; }

			public string KENNZFORM { get; set; }

			public string EINKENN_JN { get; set; }

			public string ZUSKENNZ { get; set; }

			public string BEMERKUNG { get; set; }

			public string ZL_RL_FRBNR_HIN { get; set; }

			public string ZL_RL_FRBNR_ZUR { get; set; }

			public string ZL_LIFNR { get; set; }

			public string VK_KUERZEL { get; set; }

			public string KUNDEN_REF { get; set; }

			public string KUNDEN_NOTIZ { get; set; }

			public string KENNZ_VH { get; set; }

			public string ALT_KENNZ { get; set; }

			public string ZBII_ALT_NEU { get; set; }

			public string VH_KENNZ_RES { get; set; }

			public DateTime? STILL_DAT { get; set; }

			public string FAHRZ_ART { get; set; }

			public string MNRESW { get; set; }

			public string ZZEVB { get; set; }

			public string SERIE { get; set; }

			public string SAISON_KNZ { get; set; }

			public string SAISON_BEG { get; set; }

			public string SAISON_END { get; set; }

			public string TUEV_AU { get; set; }

			public string KURZZEITVS { get; set; }

			public string ZOLLVERS { get; set; }

			public string ZOLLVERS_DAUER { get; set; }

			public string VORFUEHR { get; set; }

			public string LTEXT_NR { get; set; }

			public string BEB_STATUS { get; set; }

			public string BANKL { get; set; }

			public string BANKN { get; set; }

			public string EBPP_ACCNAME { get; set; }

			public string KOINH { get; set; }

			public string SWIFT { get; set; }

			public string IBAN { get; set; }

			public string EINZ_JN { get; set; }

			public string RECH_JN { get; set; }

			public string BARZ_JN { get; set; }

			public DateTime? HALTE_DAUER { get; set; }

			public string O_G_VERSSCHEIN { get; set; }

			public string KENNZ_UEBERNAHME { get; set; }

			public string ZZREFNR5 { get; set; }

			public string BRIEFNR { get; set; }

			public string WEBUSER_ID { get; set; }

			public string WEBGOUP_ID { get; set; }

			public string ETIKETT { get; set; }

			public string FZGTYP { get; set; }

			public string FARBE { get; set; }

			public string APPID { get; set; }

			public string BEAUFTRAGUNGSART { get; set; }

			public string RES_NAME { get; set; }

			public int? HALTEDAUER { get; set; }

			public DateTime? VE_ERDAT { get; set; }

			public string VE_ERZEIT { get; set; }

			public string VE_AENAM { get; set; }

			public string GEWEBLICH { get; set; }

			public string GEBRAUCHT { get; set; }

			private bool MappingErrorProcessed { get; set; }

			public static GT_BAK_IN Create(DataRow row, ISapConnection sapConnection = null, IDynSapProxyFactory dynSapProxyFactory = null)
			{
				GT_BAK_IN o;

				try
				{
					o = new GT_BAK_IN
					{
						SAPConnection = sapConnection,
						DynSapProxyFactory = dynSapProxyFactory,

						ZULBELN = (string)row["ZULBELN"],
						VKORG = (string)row["VKORG"],
						VKBUR = (string)row["VKBUR"],
						VE_ERNAM = (string)row["VE_ERNAM"],
						BLTYP = (string)row["BLTYP"],
						VZD_VKBUR = (string)row["VZD_VKBUR"],
						KUNNR = (string)row["KUNNR"],
						ZZREFNR1 = (string)row["ZZREFNR1"],
						ZZREFNR2 = (string)row["ZZREFNR2"],
						ZZREFNR3 = (string)row["ZZREFNR3"],
						ZZREFNR4 = (string)row["ZZREFNR4"],
						KREISKZ = (string)row["KREISKZ"],
						KREISBEZ = (string)row["KREISBEZ"],
						WUNSCHKENN_JN = (string)row["WUNSCHKENN_JN"],
						RESERVKENN_JN = (string)row["RESERVKENN_JN"],
						RESERVKENN = (string)row["RESERVKENN"],
						FEINSTAUB = (string)row["FEINSTAUB"],
						ZZZLDAT = string.IsNullOrEmpty(row["ZZZLDAT"].ToString()) ? null : (DateTime?)row["ZZZLDAT"],
						ZZKENN = (string)row["ZZKENN"],
						WU_KENNZ2 = (string)row["WU_KENNZ2"],
						WU_KENNZ3 = (string)row["WU_KENNZ3"],
						KENNZTYP = (string)row["KENNZTYP"],
						KENNZFORM = (string)row["KENNZFORM"],
						EINKENN_JN = (string)row["EINKENN_JN"],
						ZUSKENNZ = (string)row["ZUSKENNZ"],
						BEMERKUNG = (string)row["BEMERKUNG"],
						ZL_RL_FRBNR_HIN = (string)row["ZL_RL_FRBNR_HIN"],
						ZL_RL_FRBNR_ZUR = (string)row["ZL_RL_FRBNR_ZUR"],
						ZL_LIFNR = (string)row["ZL_LIFNR"],
						VK_KUERZEL = (string)row["VK_KUERZEL"],
						KUNDEN_REF = (string)row["KUNDEN_REF"],
						KUNDEN_NOTIZ = (string)row["KUNDEN_NOTIZ"],
						KENNZ_VH = (string)row["KENNZ_VH"],
						ALT_KENNZ = (string)row["ALT_KENNZ"],
						ZBII_ALT_NEU = (string)row["ZBII_ALT_NEU"],
						VH_KENNZ_RES = (string)row["VH_KENNZ_RES"],
						STILL_DAT = string.IsNullOrEmpty(row["STILL_DAT"].ToString()) ? null : (DateTime?)row["STILL_DAT"],
						FAHRZ_ART = (string)row["FAHRZ_ART"],
						MNRESW = (string)row["MNRESW"],
						ZZEVB = (string)row["ZZEVB"],
						SERIE = (string)row["SERIE"],
						SAISON_KNZ = (string)row["SAISON_KNZ"],
						SAISON_BEG = (string)row["SAISON_BEG"],
						SAISON_END = (string)row["SAISON_END"],
						TUEV_AU = (string)row["TUEV_AU"],
						KURZZEITVS = (string)row["KURZZEITVS"],
						ZOLLVERS = (string)row["ZOLLVERS"],
						ZOLLVERS_DAUER = (string)row["ZOLLVERS_DAUER"],
						VORFUEHR = (string)row["VORFUEHR"],
						LTEXT_NR = (string)row["LTEXT_NR"],
						BEB_STATUS = (string)row["BEB_STATUS"],
						BANKL = (string)row["BANKL"],
						BANKN = (string)row["BANKN"],
						EBPP_ACCNAME = (string)row["EBPP_ACCNAME"],
						KOINH = (string)row["KOINH"],
						SWIFT = (string)row["SWIFT"],
						IBAN = (string)row["IBAN"],
						EINZ_JN = (string)row["EINZ_JN"],
						RECH_JN = (string)row["RECH_JN"],
						BARZ_JN = (string)row["BARZ_JN"],
						HALTE_DAUER = string.IsNullOrEmpty(row["HALTE_DAUER"].ToString()) ? null : (DateTime?)row["HALTE_DAUER"],
						O_G_VERSSCHEIN = (string)row["O_G_VERSSCHEIN"],
						KENNZ_UEBERNAHME = (string)row["KENNZ_UEBERNAHME"],
						ZZREFNR5 = (string)row["ZZREFNR5"],
						BRIEFNR = (string)row["BRIEFNR"],
						WEBUSER_ID = (string)row["WEBUSER_ID"],
						WEBGOUP_ID = (string)row["WEBGOUP_ID"],
						ETIKETT = (string)row["ETIKETT"],
						FZGTYP = (string)row["FZGTYP"],
						FARBE = (string)row["FARBE"],
						APPID = (string)row["APPID"],
						BEAUFTRAGUNGSART = (string)row["BEAUFTRAGUNGSART"],
						RES_NAME = (string)row["RES_NAME"],
						HALTEDAUER = string.IsNullOrEmpty(row["HALTEDAUER"].ToString()) ? null : (int?)row["HALTEDAUER"],
						VE_ERDAT = string.IsNullOrEmpty(row["VE_ERDAT"].ToString()) ? null : (DateTime?)row["VE_ERDAT"],
						VE_ERZEIT = (string)row["VE_ERZEIT"],
						VE_AENAM = (string)row["VE_AENAM"],
						GEWEBLICH = (string)row["GEWEBLICH"],
						GEBRAUCHT = (string)row["GEBRAUCHT"],
					};
				}
				catch(Exception e)
				{
					o = new GT_BAK_IN
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

			public static IEnumerable<GT_BAK_IN> Select(DataTable dt, ISapConnection sapConnection = null)
			{
				return dt.AsEnumerable().Select(r => Create(r, sapConnection));
			}

			public static List<GT_BAK_IN> ToList(DataTable dt, ISapConnection sapConnection = null)
			{
				return Select(dt, sapConnection).ToListOrEmptyList();
			}

			public static IEnumerable<GT_BAK_IN> Select(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				var tbl = dts.FirstOrDefault(t => t.TableName.ToLower() == typeof(GT_BAK_IN).Name.ToLower());
				if (tbl == null)
					return null;

				return Select(tbl, sapConnection);
			}

			public static List<GT_BAK_IN> ToList(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				return Select(dts, sapConnection).ToListOrEmptyList();
			}

			public static List<GT_BAK_IN> ToList(ISapDataService sapDataService)
			{
				return ToList(sapDataService.GetExportTables(), sapDataService.SapConnection);
			}

			public static List<GT_BAK_IN> GetExportListWithInitExecute(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<GT_BAK_IN>();
				 
				var dts = sapDataService.GetExportTablesWithInitExecute("Z_ZLD_AH_AUSGABE_ZULFORMS", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_BAK_IN> GetExportListWithExecute(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_BAK_IN>();
				 
				var dts = sapDataService.GetExportTablesWithExecute();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_BAK_IN> GetExportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_BAK_IN>();
				 
				var dts = sapDataService.GetExportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_BAK_IN> GetImportListWithInit(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<GT_BAK_IN>();
				 
				var dts = sapDataService.GetImportTablesWithInit("Z_ZLD_AH_AUSGABE_ZULFORMS", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_BAK_IN> GetImportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_BAK_IN>();
				 
				var dts = sapDataService.GetImportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}
		}

		public partial class GT_FILENAME : IModelMappingApplied
		{
			[SapIgnore]
			[ScriptIgnore]
			public ISapConnection SAPConnection { get; set; }

			[SapIgnore]
			[ScriptIgnore]
			public IDynSapProxyFactory DynSapProxyFactory { get; set; }

			public string ZULBELN { get; set; }

			public string FILENAME { get; set; }

			public string FORMART { get; set; }

			public string NAME { get; set; }

			private bool MappingErrorProcessed { get; set; }

			public static GT_FILENAME Create(DataRow row, ISapConnection sapConnection = null, IDynSapProxyFactory dynSapProxyFactory = null)
			{
				GT_FILENAME o;

				try
				{
					o = new GT_FILENAME
					{
						SAPConnection = sapConnection,
						DynSapProxyFactory = dynSapProxyFactory,

						ZULBELN = (string)row["ZULBELN"],
						FILENAME = (string)row["FILENAME"],
						FORMART = (string)row["FORMART"],
						NAME = (string)row["NAME"],
					};
				}
				catch(Exception e)
				{
					o = new GT_FILENAME
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

			public static IEnumerable<GT_FILENAME> Select(DataTable dt, ISapConnection sapConnection = null)
			{
				return dt.AsEnumerable().Select(r => Create(r, sapConnection));
			}

			public static List<GT_FILENAME> ToList(DataTable dt, ISapConnection sapConnection = null)
			{
				return Select(dt, sapConnection).ToListOrEmptyList();
			}

			public static IEnumerable<GT_FILENAME> Select(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				var tbl = dts.FirstOrDefault(t => t.TableName.ToLower() == typeof(GT_FILENAME).Name.ToLower());
				if (tbl == null)
					return null;

				return Select(tbl, sapConnection);
			}

			public static List<GT_FILENAME> ToList(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				return Select(dts, sapConnection).ToListOrEmptyList();
			}

			public static List<GT_FILENAME> ToList(ISapDataService sapDataService)
			{
				return ToList(sapDataService.GetExportTables(), sapDataService.SapConnection);
			}

			public static List<GT_FILENAME> GetExportListWithInitExecute(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<GT_FILENAME>();
				 
				var dts = sapDataService.GetExportTablesWithInitExecute("Z_ZLD_AH_AUSGABE_ZULFORMS", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_FILENAME> GetExportListWithExecute(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_FILENAME>();
				 
				var dts = sapDataService.GetExportTablesWithExecute();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_FILENAME> GetExportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_FILENAME>();
				 
				var dts = sapDataService.GetExportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_FILENAME> GetImportListWithInit(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<GT_FILENAME>();
				 
				var dts = sapDataService.GetImportTablesWithInit("Z_ZLD_AH_AUSGABE_ZULFORMS", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_FILENAME> GetImportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_FILENAME>();
				 
				var dts = sapDataService.GetImportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}
		}
	}

	public static partial class DataTableExtensions
	{

		public static DataTable ToTable(this IEnumerable<Z_ZLD_AH_AUSGABE_ZULFORMS.GT_BAK_IN> list)
		{
			return SapDataServiceExtensions.ToTable(list);
		}


		public static DataTable ToTable(this IEnumerable<Z_ZLD_AH_AUSGABE_ZULFORMS.GT_FILENAME> list)
		{
			return SapDataServiceExtensions.ToTable(list);
		}

	}
}
