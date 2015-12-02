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
	public partial class Z_ZLD_EXPORT_AUSWERTUNG_1
	{
		public static void Init(ISapDataService sap)
		{
			sap.Init(typeof(Z_ZLD_EXPORT_AUSWERTUNG_1).Name);
		}

		public static void Init(ISapDataService sap, string inputParameterKeys, params object[] inputParameterValues)
		{
			sap.Init(typeof(Z_ZLD_EXPORT_AUSWERTUNG_1).Name, inputParameterKeys, inputParameterValues);
		}


		public static void SetImportParameter_I_ABRKZ(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_ABRKZ", value);
		}

		public static void SetImportParameter_I_BAR_JN(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_BAR_JN", value);
		}

		public static void SetImportParameter_I_EC_JN(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_EC_JN", value);
		}

		public static void SetImportParameter_I_GRUPPE(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_GRUPPE", value);
		}

		public static void SetImportParameter_I_KREISKZ(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_KREISKZ", value);
		}

		public static void SetImportParameter_I_KUNNR(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_KUNNR", value);
		}

		public static void SetImportParameter_I_LOEKZ(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_LOEKZ", value);
		}

		public static void SetImportParameter_I_MATNR(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_MATNR", value);
		}

		public static void SetImportParameter_I_NNDGF(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_NNDGF", value);
		}

		public static void SetImportParameter_I_STATUS(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_STATUS", value);
		}

		public static void SetImportParameter_I_VKBUR(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_VKBUR", value);
		}

		public static void SetImportParameter_I_VKORG(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_VKORG", value);
		}

		public static void SetImportParameter_I_ZAHLART(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_ZAHLART", value);
		}

		public static void SetImportParameter_I_ZULBELN(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_ZULBELN", value);
		}

		public static void SetImportParameter_I_ZZKENN(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_ZZKENN", value);
		}

		public static void SetImportParameter_I_ZZREFNR1(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_ZZREFNR1", value);
		}

		public static void SetImportParameter_I_ZZZLDAT_BIS(ISapDataService sap, DateTime? value)
		{
			sap.SetImportParameter("I_ZZZLDAT_BIS", value);
		}

		public static void SetImportParameter_I_ZZZLDAT_VON(ISapDataService sap, DateTime? value)
		{
			sap.SetImportParameter("I_ZZZLDAT_VON", value);
		}

		public partial class GT_LISTE1 : IModelMappingApplied
		{
			[SapIgnore]
			[ScriptIgnore]
			public ISapConnection SAPConnection { get; set; }

			[SapIgnore]
			[ScriptIgnore]
			public IDynSapProxyFactory DynSapProxyFactory { get; set; }

			public string ZULBELN { get; set; }

			public string KUNNR { get; set; }

			public string KUNDENNAME { get; set; }

			public string ZZREFNR1 { get; set; }

			public string ZZREFNR2 { get; set; }

			public DateTime? ZZZLDAT { get; set; }

			public string ZZKENN { get; set; }

			public string KREISKZ { get; set; }

			public decimal? MENGE { get; set; }

			public string MAKTX { get; set; }

			public string BEMERKUNG { get; set; }

			public string BLTYP { get; set; }

			public string STATUS { get; set; }

			public decimal? PREIS_DL { get; set; }

			public decimal? PREIS_GB { get; set; }

			public decimal? GEB_AMT { get; set; }

			public decimal? PREIS_ST { get; set; }

			public decimal? PREIS_KZ { get; set; }

			public string FEINSTAUBAMT { get; set; }

			public string RESWUNSCH { get; set; }

			public string EC_JN { get; set; }

			public string BAR_JN { get; set; }

			public string RE_JN { get; set; }

			public string ERNAM { get; set; }

			public string LOEKZ_K { get; set; }

			public string LOEKZ_P { get; set; }

			public string KSTATUS { get; set; }

			public string ZAHLART { get; set; }

			public string VE_ERNAM { get; set; }

			public string VE_ERDAT_ZEIT { get; set; }

			public string BARQ_NR { get; set; }

			public string OBJECT_ID { get; set; }

			public string CPD_ADRESSE { get; set; }

			public string BA_VKBUR { get; set; }

			public string KTEXT { get; set; }

			public string MOBUSER { get; set; }

			public string INFO_TEXT { get; set; }

			public string NACHBEARBEITEN { get; set; }

			public string SOFORT_ABR_ERL { get; set; }

			public string SA_PFAD { get; set; }

			public static GT_LISTE1 Create(DataRow row, ISapConnection sapConnection = null, IDynSapProxyFactory dynSapProxyFactory = null)
			{
				var o = new GT_LISTE1
				{
					ZULBELN = (string)row["ZULBELN"],
					KUNNR = (string)row["KUNNR"],
					KUNDENNAME = (string)row["KUNDENNAME"],
					ZZREFNR1 = (string)row["ZZREFNR1"],
					ZZREFNR2 = (string)row["ZZREFNR2"],
					ZZZLDAT = string.IsNullOrEmpty(row["ZZZLDAT"].ToString()) ? null : (DateTime?)row["ZZZLDAT"],
					ZZKENN = (string)row["ZZKENN"],
					KREISKZ = (string)row["KREISKZ"],
					MENGE = string.IsNullOrEmpty(row["MENGE"].ToString()) ? null : (decimal?)row["MENGE"],
					MAKTX = (string)row["MAKTX"],
					BEMERKUNG = (string)row["BEMERKUNG"],
					BLTYP = (string)row["BLTYP"],
					STATUS = (string)row["STATUS"],
					PREIS_DL = string.IsNullOrEmpty(row["PREIS_DL"].ToString()) ? null : (decimal?)row["PREIS_DL"],
					PREIS_GB = string.IsNullOrEmpty(row["PREIS_GB"].ToString()) ? null : (decimal?)row["PREIS_GB"],
					GEB_AMT = string.IsNullOrEmpty(row["GEB_AMT"].ToString()) ? null : (decimal?)row["GEB_AMT"],
					PREIS_ST = string.IsNullOrEmpty(row["PREIS_ST"].ToString()) ? null : (decimal?)row["PREIS_ST"],
					PREIS_KZ = string.IsNullOrEmpty(row["PREIS_KZ"].ToString()) ? null : (decimal?)row["PREIS_KZ"],
					FEINSTAUBAMT = (string)row["FEINSTAUBAMT"],
					RESWUNSCH = (string)row["RESWUNSCH"],
					EC_JN = (string)row["EC_JN"],
					BAR_JN = (string)row["BAR_JN"],
					RE_JN = (string)row["RE_JN"],
					ERNAM = (string)row["ERNAM"],
					LOEKZ_K = (string)row["LOEKZ_K"],
					LOEKZ_P = (string)row["LOEKZ_P"],
					KSTATUS = (string)row["KSTATUS"],
					ZAHLART = (string)row["ZAHLART"],
					VE_ERNAM = (string)row["VE_ERNAM"],
					VE_ERDAT_ZEIT = (string)row["VE_ERDAT_ZEIT"],
					BARQ_NR = (string)row["BARQ_NR"],
					OBJECT_ID = (string)row["OBJECT_ID"],
					CPD_ADRESSE = (string)row["CPD_ADRESSE"],
					BA_VKBUR = (string)row["BA_VKBUR"],
					KTEXT = (string)row["KTEXT"],
					MOBUSER = (string)row["MOBUSER"],
					INFO_TEXT = (string)row["INFO_TEXT"],
					NACHBEARBEITEN = (string)row["NACHBEARBEITEN"],
					SOFORT_ABR_ERL = (string)row["SOFORT_ABR_ERL"],
					SA_PFAD = (string)row["SA_PFAD"],

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

			public static IEnumerable<GT_LISTE1> Select(DataTable dt, ISapConnection sapConnection = null)
			{
				return dt.AsEnumerable().Select(r => Create(r, sapConnection));
			}

			public static List<GT_LISTE1> ToList(DataTable dt, ISapConnection sapConnection = null)
			{
				return Select(dt, sapConnection).ToListOrEmptyList();
			}

			public static IEnumerable<GT_LISTE1> Select(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				var tbl = dts.FirstOrDefault(t => t.TableName.ToLower() == typeof(GT_LISTE1).Name.ToLower());
				if (tbl == null)
					return null;

				return Select(tbl, sapConnection);
			}

			public static List<GT_LISTE1> ToList(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				return Select(dts, sapConnection).ToListOrEmptyList();
			}

			public static List<GT_LISTE1> ToList(ISapDataService sapDataService)
			{
				return ToList(sapDataService.GetExportTables(), sapDataService.SapConnection);
			}

			public static List<GT_LISTE1> GetExportListWithInitExecute(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<GT_LISTE1>();
				 
				var dts = sapDataService.GetExportTablesWithInitExecute("Z_ZLD_EXPORT_AUSWERTUNG_1", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_LISTE1> GetExportListWithExecute(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_LISTE1>();
				 
				var dts = sapDataService.GetExportTablesWithExecute();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_LISTE1> GetExportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_LISTE1>();
				 
				var dts = sapDataService.GetExportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_LISTE1> GetImportListWithInit(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<GT_LISTE1>();
				 
				var dts = sapDataService.GetImportTablesWithInit("Z_ZLD_EXPORT_AUSWERTUNG_1", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_LISTE1> GetImportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_LISTE1>();
				 
				var dts = sapDataService.GetImportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}
		}
	}

	public static partial class DataTableExtensions
	{

		public static DataTable ToTable(this IEnumerable<Z_ZLD_EXPORT_AUSWERTUNG_1.GT_LISTE1> list)
		{
			return SapDataServiceExtensions.ToTable(list);
		}

	}
}
