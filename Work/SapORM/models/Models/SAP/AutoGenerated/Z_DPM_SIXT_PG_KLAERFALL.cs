using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Script.Serialization;
using GeneralTools.Contracts;
using SapORM.Contracts;

namespace SapORM.Models
{
	public partial class Z_DPM_SIXT_PG_KLAERFALL
	{
		public static void Init(ISapDataService sap)
		{
			sap.Init(typeof(Z_DPM_SIXT_PG_KLAERFALL).Name);
		}

		public static void Init(ISapDataService sap, string inputParameterKeys, params object[] inputParameterValues)
		{
			sap.Init(typeof(Z_DPM_SIXT_PG_KLAERFALL).Name, inputParameterKeys, inputParameterValues);
		}

		public partial class GT_WEB : IModelMappingApplied
		{
			[SapIgnore]
			[ScriptIgnore]
			public ISapConnection SAPConnection { get; set; }

			[SapIgnore]
			[ScriptIgnore]
			public IDynSapProxyFactory DynSapProxyFactory { get; set; }

			public string KUNNR { get; set; }

			public string LVTNR { get; set; }

			public DateTime? IMPDAT { get; set; }

			public string STATUS { get; set; }

			public DateTime? ANFDA { get; set; }

			public DateTime? MAHNDAT1 { get; set; }

			public DateTime? MAHNDAT2 { get; set; }

			public DateTime? MAHNDAT3 { get; set; }

			public DateTime? EINGANG_VM { get; set; }

			public DateTime? EINGANG_HR { get; set; }

			public DateTime? EINGANG_GA { get; set; }

			public DateTime? EINGANG_PA { get; set; }

			public DateTime? EINGANG_EE { get; set; }

			public string EVBNR { get; set; }

			public DateTime? EINGANG_SS { get; set; }

			public string KOMPLETT { get; set; }

			public string BEMERKUNG { get; set; }

			public DateTime? ZULDAT { get; set; }

			public string VERTRAGSSTATUS { get; set; }

			public string KUNDENTYP { get; set; }

			public string LVID { get; set; }

			public string KUNDE { get; set; }

			public string KUNUM { get; set; }

			public string NAME1 { get; set; }

			public string NAME2 { get; set; }

			public string NAME3 { get; set; }

			public string TITEL { get; set; }

			public string ANREDE_SCHL { get; set; }

			public string STRASSE { get; set; }

			public string PLZ { get; set; }

			public string ORT { get; set; }

			public string TELEFON { get; set; }

			public string MOBIL { get; set; }

			public string EMAIL { get; set; }

			public string HERSTELLER { get; set; }

			public string FZG_TYP { get; set; }

			public string STEUER_ABR_KZ { get; set; }

			public string ZUL_AUF { get; set; }

			public string ZUL_DURCH { get; set; }

			public string BESTELLART { get; set; }

			public DateTime? WUNSCHDATUM { get; set; }

			public DateTime? DAT_ANNAHMEDOK { get; set; }

			public DateTime? LIEFERTERMIN { get; set; }

			public string ALT_NAME1 { get; set; }

			public string ALT_NAME2 { get; set; }

			public string ALT_NAME3 { get; set; }

			public string FAXNR { get; set; }

			public string EMAIL2 { get; set; }

			public string VERS_PRODUKT { get; set; }

			public string KUNDENBETREUER { get; set; }

			public static GT_WEB Create(DataRow row, ISapConnection sapConnection = null, IDynSapProxyFactory dynSapProxyFactory = null)
			{
				var o = new GT_WEB
				{
					KUNNR = (string)row["KUNNR"],
					LVTNR = (string)row["LVTNR"],
					IMPDAT = (string.IsNullOrEmpty(row["IMPDAT"].ToString())) ? null : (DateTime?)row["IMPDAT"],
					STATUS = (string)row["STATUS"],
					ANFDA = (string.IsNullOrEmpty(row["ANFDA"].ToString())) ? null : (DateTime?)row["ANFDA"],
					MAHNDAT1 = (string.IsNullOrEmpty(row["MAHNDAT1"].ToString())) ? null : (DateTime?)row["MAHNDAT1"],
					MAHNDAT2 = (string.IsNullOrEmpty(row["MAHNDAT2"].ToString())) ? null : (DateTime?)row["MAHNDAT2"],
					MAHNDAT3 = (string.IsNullOrEmpty(row["MAHNDAT3"].ToString())) ? null : (DateTime?)row["MAHNDAT3"],
					EINGANG_VM = (string.IsNullOrEmpty(row["EINGANG_VM"].ToString())) ? null : (DateTime?)row["EINGANG_VM"],
					EINGANG_HR = (string.IsNullOrEmpty(row["EINGANG_HR"].ToString())) ? null : (DateTime?)row["EINGANG_HR"],
					EINGANG_GA = (string.IsNullOrEmpty(row["EINGANG_GA"].ToString())) ? null : (DateTime?)row["EINGANG_GA"],
					EINGANG_PA = (string.IsNullOrEmpty(row["EINGANG_PA"].ToString())) ? null : (DateTime?)row["EINGANG_PA"],
					EINGANG_EE = (string.IsNullOrEmpty(row["EINGANG_EE"].ToString())) ? null : (DateTime?)row["EINGANG_EE"],
					EVBNR = (string)row["EVBNR"],
					EINGANG_SS = (string.IsNullOrEmpty(row["EINGANG_SS"].ToString())) ? null : (DateTime?)row["EINGANG_SS"],
					KOMPLETT = (string)row["KOMPLETT"],
					BEMERKUNG = (string)row["BEMERKUNG"],
					ZULDAT = (string.IsNullOrEmpty(row["ZULDAT"].ToString())) ? null : (DateTime?)row["ZULDAT"],
					VERTRAGSSTATUS = (string)row["VERTRAGSSTATUS"],
					KUNDENTYP = (string)row["KUNDENTYP"],
					LVID = (string)row["LVID"],
					KUNDE = (string)row["KUNDE"],
					KUNUM = (string)row["KUNUM"],
					NAME1 = (string)row["NAME1"],
					NAME2 = (string)row["NAME2"],
					NAME3 = (string)row["NAME3"],
					TITEL = (string)row["TITEL"],
					ANREDE_SCHL = (string)row["ANREDE_SCHL"],
					STRASSE = (string)row["STRASSE"],
					PLZ = (string)row["PLZ"],
					ORT = (string)row["ORT"],
					TELEFON = (string)row["TELEFON"],
					MOBIL = (string)row["MOBIL"],
					EMAIL = (string)row["EMAIL"],
					HERSTELLER = (string)row["HERSTELLER"],
					FZG_TYP = (string)row["FZG_TYP"],
					STEUER_ABR_KZ = (string)row["STEUER_ABR_KZ"],
					ZUL_AUF = (string)row["ZUL_AUF"],
					ZUL_DURCH = (string)row["ZUL_DURCH"],
					BESTELLART = (string)row["BESTELLART"],
					WUNSCHDATUM = (string.IsNullOrEmpty(row["WUNSCHDATUM"].ToString())) ? null : (DateTime?)row["WUNSCHDATUM"],
					DAT_ANNAHMEDOK = (string.IsNullOrEmpty(row["DAT_ANNAHMEDOK"].ToString())) ? null : (DateTime?)row["DAT_ANNAHMEDOK"],
					LIEFERTERMIN = (string.IsNullOrEmpty(row["LIEFERTERMIN"].ToString())) ? null : (DateTime?)row["LIEFERTERMIN"],
					ALT_NAME1 = (string)row["ALT_NAME1"],
					ALT_NAME2 = (string)row["ALT_NAME2"],
					ALT_NAME3 = (string)row["ALT_NAME3"],
					FAXNR = (string)row["FAXNR"],
					EMAIL2 = (string)row["EMAIL2"],
					VERS_PRODUKT = (string)row["VERS_PRODUKT"],
					KUNDENBETREUER = (string)row["KUNDENBETREUER"],

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
				return Select(dt, sapConnection).ToList();
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
				return Select(dts, sapConnection).ToList();
			}

			public static List<GT_WEB> ToList(ISapDataService sapDataService)
			{
				return ToList(sapDataService.GetExportTables(), sapDataService.SapConnection);
			}

			public static List<GT_WEB> GetExportListWithInitExecute(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<GT_WEB>();
				 
				var dts = sapDataService.GetExportTablesWithInitExecute("Z_DPM_SIXT_PG_KLAERFALL", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToList();
			}

			public static List<GT_WEB> GetExportListWithExecute(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_WEB>();
				 
				var dts = sapDataService.GetExportTablesWithExecute();
				 
				return Select(dts, sapDataService.SapConnection).ToList();
			}

			public static List<GT_WEB> GetExportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_WEB>();
				 
				var dts = sapDataService.GetExportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToList();
			}

			public static List<GT_WEB> GetImportListWithInit(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<GT_WEB>();
				 
				var dts = sapDataService.GetImportTablesWithInit("Z_DPM_SIXT_PG_KLAERFALL", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToList();
			}

			public static List<GT_WEB> GetImportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_WEB>();
				 
				var dts = sapDataService.GetImportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToList();
			}
		}
	}

	public static partial class DataTableExtensions
	{

		public static DataTable ToTable(this IEnumerable<Z_DPM_SIXT_PG_KLAERFALL.GT_WEB> list)
		{
			return SapDataServiceExtensions.ToTable(list);
		}

		public static void Apply(this IEnumerable<Z_DPM_SIXT_PG_KLAERFALL.GT_WEB> list, DataTable dtDst)
		{
			SapDataServiceExtensions.Apply(list, dtDst);
		}

	}
}
